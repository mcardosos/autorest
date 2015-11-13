﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

using Microsoft.Rest.Generator.ClientModel;
using Microsoft.Rest.Generator.Go;
using Microsoft.Rest.Generator.Utilities;

namespace Microsoft.Rest.Generator.Go
{
    public class VersionTemplateModel : ServiceClient
    {
        public static readonly List<string> StandardImports = new List<string> { "fmt" };

        public readonly string PackageName;

        public VersionTemplateModel(ServiceClient serviceClient, string packageName)
        {
            this.LoadFrom(serviceClient);

            PackageName = packageName;
        }

        public virtual IEnumerable<string> Imports
        {
            get
            {
                var imports = new HashSet<string>();
                imports.UnionWith(StandardImports);
                return imports.OrderBy(i => i);
            }
        }
    }
}
