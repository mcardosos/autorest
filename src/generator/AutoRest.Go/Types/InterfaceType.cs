// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using AutoRest.Core.Model;
using AutoRest.Core.Utilities;


namespace AutoRest.Go
{
    /// <summary>
    /// Defines a pseudo-PrimaryType to support Go interface types.
    /// </summary>
    public class InterfaceType : IModelType
    {
        public Fixable<string> Name
        {
            get { return "interface{}"; }
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            var knownType = obj as InterfaceType;

            if (knownType != null)
            {
                return knownType.Name == Name;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public string ExtendedDocumentation { get; }
        public string DefaultValue { get; }
        public bool IsConstant { get; }
        public string ClassName { get; }
        public CodeModel CodeModel { get; }
        public IEnumerable<IIdentifier> IdentifiersInScope { get; }
        public IParent Parent { get; }
        public IEnumerable<IChild> Children { get; }
        public string Qualifier { get; }
        public IEnumerable<string> MyReservedNames { get; }
        public HashSet<string> LocallyUsedNames { get; }
        public string QualifierType { get; }
        public string DeclarationName { get; }
        public void Disambiguate()
        {

        }
    }
}
