// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Rest.Generator.ClientModel;

namespace Microsoft.Rest.Generator.Go
{
    /// <summary>
    /// Defines a synthesized composite type that wraps a primary type, array, or dictionary method response.
    /// </summary>
    public class SyntheticType : CompositeType
    {
        public SyntheticType(IType baseType)
        {
            if (!ShouldBeSyntheticType(baseType))
            {
                throw new ArgumentException("{0} is not a valid type for SyntheticType", baseType.ToString());
            }

            // TODO (gosdk): Ensure the generated name does not collide with existing type names
            IType elementType = baseType is PrimaryType || baseType is PackageType || baseType is EnumType
                                ? baseType
                                : baseType is SequenceType
                                    ? (baseType as SequenceType).ElementType
                                    : (baseType as DictionaryType).ValueType;

            if (elementType is PrimaryType)
            {
                if (elementType == PrimaryType.Boolean)
                {
                    Name = "Bool";
                }
                else if (elementType == PrimaryType.ByteArray)
                {
                    Name = "ByteArray";
                }
                else if (elementType == PrimaryType.Double)
                {
                    Name = "Float64";
                }
                else if (elementType == PrimaryType.Int)
                {
                    Name = "Int32";
                }
                else if (elementType == PrimaryType.Long)
                {
                    Name = "Int64";
                }
                else if (elementType == PrimaryType.Stream)
                {
                    Name = "ReadCloser";
                }
                else if (elementType == PrimaryType.String)
                {
                    Name = "String";
                }
                else if (elementType == PrimaryType.TimeSpan)
                {
                    Name = "TimeSpan";
                }

            }
            else if (elementType is InterfaceType)
            {
                Name = "Object";
            }
            else if (elementType is PackageType)
            {
                Name = (elementType as PackageType).Member;
            }
            else if (elementType is EnumType)
            {
                Name = "String";
            }
            else
            {
                Name = elementType.Name;
            }

            if (baseType is SequenceType)
            {
                Name += "List";
            }
            else if (baseType is DictionaryType)
            {
                Name += "Set";
            }

            Property p = new Property();
            p.SerializedName = "value";
            p.Name = "Value";
            p.Type = elementType;

            Properties.Add(p);
        }

        public static bool ShouldBeSyntheticType(IType type)
        {
            return (type is PrimaryType || type is PackageType || type is SequenceType || type is DictionaryType || type is EnumType);
        }
    }
}
