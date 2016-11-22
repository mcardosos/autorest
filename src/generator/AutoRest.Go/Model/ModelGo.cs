// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

using AutoRest.Core.Utilities;
using AutoRest.Core.Model;

namespace AutoRest.Go.Model
{
    public class ModelGo : CompositeType
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
        public ModelGo(CompositeType source)
        {
            this.LoadFrom(source);

            PropertyGoModels = new List<PropertyGo>();
            source.Properties.ForEach(p => PropertyGoModels.Add(new PropertyGo()));
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
        public List<PropertyGo> PropertyGoModels { get; private set; }
    }
}