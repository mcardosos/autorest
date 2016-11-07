// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

using AutoRest.Core.ClientModel;
using AutoRest.Core.Utilities;

namespace AutoRest.Go.TemplateModels
{
    public class ModelTemplateModel : CompositeType
    {
        // Refactor -> Namer
        private readonly IScopeProvider _scope = new VariableScopeProvider();

        // True if the type is returned by a method
        // Refactor -> CodeModelTransformer
        public bool IsResponseType;

        // Name of the field containing the URL used to retrieve the next result set
        // (null or empty if the model is not paged).
        // Refactor -> This is used for too many things... Ideally it should be in the CodeModelTransformer, and later used in the namer 
        public string NextLink;

        // Refactor -> CodeModelTransformer
        public bool PreparerNeeded = false;

        // Refactor -> CodeModelTransformer
        public ModelTemplateModel(CompositeType source)
        {
            this.LoadFrom(source);

            PropertyTemplateModels = new List<PropertyTemplateModel>();
            source.Properties.ForEach(p => PropertyTemplateModels.Add(new PropertyTemplateModel(p)));
        }

        // Refactor -> Nobody uses this
        public IScopeProvider Scope
        {
            get { return _scope; }
        }

        // Refactor -> Nobody uses this
        public virtual IEnumerable<string> Imports
        {
            get
            {
                var imports = new HashSet<string>();
                (this as CompositeType).AddImports(imports);
                return imports;
            }
        }

        // Refactor -> CodeModelTransformer
        public List<PropertyTemplateModel> PropertyTemplateModels { get; private set; }
    }
}