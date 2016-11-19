// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using AutoRest.Core;
using AutoRest.Core.Model;

namespace AutoRest.Go.Model
{
    public class CodeModelGo : CodeModel
    {
        public CodeModelGo(){
            Documentation = string.Format("Package {0} implements the Azure ARM {1} service API version {2}.\n\n{3}", PackageName, ServiceName, ApiVersion,
                                    !string.IsNullOrEmpty(Documentation) ? Documentation.UnwrapAnchorTags() : "");

            // everything models

            //adds all named enums
            EnumTemplateModels = new List<EnumTypeGo>();
            foreach (var enumType in EnumTypes)
            {
                if (!enumType.IsNamed())
                {
                    continue;
                }
                EnumTemplateModels.Add(new EnumTypeGo(enumType));
            }

            // And add any others with a defined name and value list (but not already located)
            ModelTypes.ToList()
                .ForEach(mt => {
                    mt.Properties.Where(p => p.ModelType is EnumType && (p.ModelType as EnumType).IsNamed()).ToList()
                        .ForEach(p => {
                            if (!EnumTemplateModels.Any(etm => etm.Equals(p.ModelType)))
                            {
                                EnumTemplateModels.Add(new EnumTypeGo(p.ModelType as EnumType));
                            }
                        });
                });
            
            
            EnumTemplateModels.Sort(delegate(EnumTypeGo x, EnumTypeGo y)
            {
                return x.Name.FixedValue.CompareTo(y.Name);
            });

            // Ensure all enumerated type values have the simplest possible unique names
            // -- The code assumes that all public type names are unique within the client and that the values
            //    of an enumerated type are unique within that type. To safely promote the enumerated value name
            //    to the top-level, it must not conflict with other existing types. If it does, prepending the
            //    value name with the (assumed to be unique) enumerated type name will make it unique.

            // First, collect all type names (since these cannot change)
            var topLevelNames = new HashSet<string>();
            ModelTypes.ToList()
                .ForEach(mt => topLevelNames.Add(mt.Name));

            // Then, note each enumerated type with one or more conflicting values and collect the values
            // from those enumerated types without conflicts
            EnumTemplateModels
                .ForEach(em => 
                {
                    if (em.Values.Where(v => topLevelNames.Contains(v.Name) || GoCodeNamer.UserDefinedNames.Contains(v.Name)).Count() > 0)
                    {
                        em.HasUniqueNames = false;
                    }
                    else
                    {
                        em.HasUniqueNames = true;
                        topLevelNames.UnionWith(em.Values.Select(ev => ev.Name).ToList());
                    }
                });

            // Collect defined models
            ModelTemplateModels = new List<ModelGo>();
            ModelTypes.ToList()
                .ForEach(mt =>
                {
                    ModelTemplateModels.Add(new ModelGo(mt));
                });
            ModelTemplateModels.Sort(delegate(ModelGo x, ModelGo y)
            {
                return x.Name.FixedValue.CompareTo(y.Name.FixedValue);
            });

            // Find all methods that returned paged results
            PagedTypes = new Dictionary<IModelType, string>();
            NextMethodUndefined = new List<IModelType>();
            Methods
                .Where(m => m.IsPageable()).ToList()
                .ForEach(m =>
                {
                    if (!PagedTypes.ContainsKey(m.ReturnValue().Body))
                    {
                        PagedTypes.Add(m.ReturnValue().Body, m.NextLink());
                    }
                    if (!m.NextMethodExists(Methods)) {
                        NextMethodUndefined.Add(m.ReturnValue().Body);
                    }
                });

            // Mark all models returned by one or more methods and note any "next link" fields used with paged data
            ModelTemplateModels
                .Where(mtm =>
                {
                    return Methods.Any(m => m.HasReturnValue() && m.ReturnValue().Body.Equals(mtm));
                }).ToList()
                .ForEach(mtm =>
                {
                    mtm.IsResponseType = true;
                    if (PagedTypes.ContainsKey(mtm))
                    {
                        mtm.NextLink = GoCodeNamer.PascalCaseWithoutChar(PagedTypes[mtm], '.');
                        if (NextMethodUndefined.Contains(mtm)) {
                            mtm.PreparerNeeded = true;
                        } else {
                            mtm.PreparerNeeded = false;
                        }
                    }
                });
        }

        public string[] Version => GoCodeNamer.SDKVersionFromPackageVersion(
                                            !string.IsNullOrEmpty(Settings.Instance.PackageVersion)
                                                    ? Settings.Instance.PackageVersion
                                                    : "0.0.0");
        public string PackageName => GoCodeNamer.PackageNameFromNamespace(Namespace) == null
                            ? string.Empty
                            : GoCodeNamer.PackageNameFromNamespace(Namespace);

        public string ServiceName
        {
            get
            {
                if (!string.IsNullOrEmpty(PackageName))
                {
                    return GoCodeNamer.Instance.PascalCase(PackageName);
                }
                return string.Empty;
            }
        }

        public string BaseClient = "ManagementClient";

        public IEnumerable<string> Imports
        {
            get
            {
                var imports = new HashSet<string>();
                imports.UnionWith(GoCodeNamer.AutorestImports);
                return imports.OrderBy(i => i);
            }
        }
        public string ClientDocumentation => string.Format("{0} is the base client for {1}.", BaseClient, ServiceName);

        public readonly MethodScopeProvider MethodScope;
        public readonly List<MethodGo> MethodTemplateModels;

        // public ModelsGo models;
        public List<EnumTypeGo> EnumTemplateModels { get; set; }
        public List<ModelGo> ModelTemplateModels { get; set; }
        public Dictionary<IModelType, string> PagedTypes { get; set; }
        // NextMethodUndefined is used to keep track of those models which are returned by paged methods,
        // but the next method is not defined in the service client, so these models need a preparer.
        public List<IModelType> NextMethodUndefined { get; set; }

        public IEnumerable<string> ModelImports
        {
            get
            {
                // Create an ordered union of the imports each model requires
                var imports = new HashSet<string>();
                if (ModelTemplateModels != null && ModelTemplateModels.Any(mtm => mtm.IsResponseType))
                {
                    imports.Add("github.com/Azure/go-autorest/autorest");
                } 
                ModelTypes.ToList()
                    .ForEach(mt =>
                    {
                        (mt as CompositeType).AddImports(imports);
                        if (NextMethodUndefined.Count > 0)
                        {
                            imports.UnionWith(GoCodeNamer.PageableImports);
                        }
                    });
                return imports.OrderBy(i => i);
            }
        }

        

        public virtual IEnumerable<MethodGroupGo> MethodGroupModels => Operations.Cast<MethodGroupGo>();

        public string GlobalParameters
        {
            get
            {
                List<string> declarations = new List<string>();
                foreach (var p in Properties)
                {                    
                    if (!p.SerializedName.FixedValue.IsApiVersion())
                    {
                        declarations.Add(
                                string.Format(
                                        (p.IsRequired || p.ModelType.CanBeEmpty() ? "{0} {1}" : "{0} *{1}"), 
                                         p.Name.FixedValue.ToSentence(), p.ModelType.Name));
                    }
                }
                return string.Join(", ", declarations);
            }
        }

        public string HelperGlobalParameters
        {
            get
            {
                List<string> invocationParams = new List<string>();
                foreach (var p in Properties)
                {
                    if (!p.SerializedName.FixedValue.IsApiVersion())
                    {
                        invocationParams.Add(p.Name.FixedValue.ToSentence());
                    }
                }
                return string.Join(", ", invocationParams);
            }
        }
    }
}
