// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

using AutoRest.Core;
using AutoRest.Core.Model;
using AutoRest.Go.Model;
using AutoRest.Go.Templates;
using AutoRest.Extensions.Azure;

namespace AutoRest.Go
{
    public class GoCodeGenerator : CodeGenerator
    {
        private readonly GoCodeNamer _namingFramework;

        public GoCodeGenerator()
        {

        }

        public string Name
        {
            get { return "Go"; }
        }

        public string Description
        {
            get { return "Go Client Libraries"; }
        }

        public override string UsageInstructions
        {
            get { return ""; }
        }

        public override string ImplementationFileExtension
        {
            get { return ".go"; }
        }

        /// <summary>
        /// Normalizes client model by updating names and types to be language specific.
        /// </summary>
        /// <param name="cm"></param>
        public void NormalizeCodeModel(CodeModel cm)
        {
            // Add the current package name as a reserved keyword
            _namingFramework.ReserveNamespace(cm.Namespace);
            _namingFramework.NormalizeCodeModel(cm);
            AzureExtensions.ProcessGlobalParameters(cm);
        }

        /// <summary>
        /// Generates Go code for service client.
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <returns></returns>
        public override async Task Generate(CodeModel cm)
        {
            
            var codeModel = cm as CodeModelGo;
            if (codeModel == null)
            {                
                throw new Exception("Code model is not a Go Code Model");
            }

            // Service client
             var serviceClientTemplate = new ServiceClientTemplate
            {
                Model = codeModel
            };
            
            await Write(serviceClientTemplate, GoCodeNamer.FormatFileName("client"));

            foreach (var methodGroupModel in codeModel.MethodGroupModels)
            {
                var methodGroupTemplate = new MethodGroupTemplate
                {
                    Model = methodGroupModel
                };
                await Write(methodGroupTemplate, GoCodeNamer.FormatFileName((string) methodGroupModel.TypeName).ToLowerInvariant());
            }

            // Models
            var modelsTemplate = new ModelsTemplate
            {
                Model = codeModel
            };
            await Write(modelsTemplate, GoCodeNamer.FormatFileName("models"));

            // Version
            var versionTemplate = new VersionTemplate { Model = codeModel };
            await Write(versionTemplate, GoCodeNamer.FormatFileName("version"));
        }
    }
}
