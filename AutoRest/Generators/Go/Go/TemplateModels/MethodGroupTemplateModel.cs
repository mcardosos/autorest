﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Rest.Generator.ClientModel;
using Microsoft.Rest.Generator.Go;
using Microsoft.Rest.Generator.Utilities;

namespace Microsoft.Rest.Generator.Go
{
    public class MethodGroupTemplateModel : ServiceClientTemplateModel
    {
        public MethodGroupTemplateModel(ServiceClient serviceClient, string packageName, string methodGroupName)
            : base(serviceClient, packageName, methodGroupName)
        {
            this.LoadFrom(serviceClient);

            Documentation = string.Format("{0} is the {1} ", ClientName,
                                    string.IsNullOrEmpty(Documentation)
                                        ? string.Format("client for the {0} methods of the {1} service.", MethodGroupName, ServiceName)
                                        : Documentation.ToSentence());
        }
    }
}