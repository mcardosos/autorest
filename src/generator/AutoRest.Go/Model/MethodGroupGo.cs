// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using AutoRest.Core.Model;
using AutoRest.Core.Utilities;
using AutoRest.Go;

namespace AutoRest.Go.Model
{
    public class MethodGroupGo : MethodGroup
    {
        public string ClientName;
        public string Documentation;
        public string PackageName;
        public string BaseClient;

        public string GlobalParameters;
        public string HelperGlobalParameters;
        public IEnumerable<string> Imports { get; set; }
        public IEnumerable<MethodGo> MethodTemplateModels => Methods.Cast<MethodGo>();

        public MethodGroupGo()
        {
            var cm = CodeModel as CodeModelGo;
            ClientName = string.IsNullOrEmpty(TypeName)
                            ? cm.BaseClient
                            : TypeName.FixedValue.IsNamePlural(cm.PackageName)
                                             ? TypeName + "Client"
                                             : (this.Name + "Client").TrimPackageName(cm.PackageName);

            Documentation = string.Format("{0} is the {1} ", ClientName,
                                    string.IsNullOrEmpty(cm.Documentation)
                                        ? string.Format("client for the {0} methods of the {1} service.", TypeName, cm.ServiceName)
                                        : cm.Documentation.ToSentence());

            PackageName = cm.PackageName;
            BaseClient = cm.BaseClient;
            GlobalParameters = cm.GlobalParameters;
            HelperGlobalParameters = cm.HelperGlobalParameters;



            //Imports
            var imports = new HashSet<string>();
            imports.UnionWith(GoCodeNamer.AutorestImports);
            imports.UnionWith(GoCodeNamer.StandardImports);

            bool validationImports = false;
            cm.MethodTemplateModels
                .ForEach(mtm =>
                {
                    mtm.Parameters.ForEach(p => p.AddImports(imports));
                        if (mtm.HasReturnValue() && !mtm.ReturnValue().Body.IsPrimaryType(KnownPrimaryType.Stream))
                        {
                            mtm.ReturnType.Body.AddImports(imports);
                        }
                        if (!string.IsNullOrEmpty(mtm.ParameterValidations))
                            validationImports = true;
                        });
            if (validationImports)
            {
                imports.UnionWith(GoCodeNamer.ValidationImport);
            }

            imports.OrderBy(i => i);
            Imports = imports;
        }
    }
}