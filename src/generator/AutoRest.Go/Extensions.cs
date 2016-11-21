// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

using AutoRest.Core.Model;
using AutoRest.Core.Properties;
using static AutoRest.Core.Utilities.DependencyInjection;
using AutoRest.Core.Utilities;
using AutoRest.Core.Utilities.Collections;
using AutoRest.Go.Model;
using AutoRest.Extensions.Azure;
using AutoRest.Extensions.Azure.Model;

namespace AutoRest.Go
{
    public static class Extensions
    {
        // Refactor -> CodeModelTransformer
        public const string ApiVersionName = "APIVersion";
        // Refactor -> CodeModelTransformer
        public const string ApiVersionSerializedName = "api-version";
        // Refactor -> CodeModelTransformer
        public const string NullConstraint = "Null";
        // Refactor -> CodeModelTransformer
        public const string ReadOnlyConstraint = "ReadOnly";

        // Refactor -> As how it works currently, generator.
        // This should be with the CodeModelTransformer
        public const string SkipUrlEncoding = "x-ms-skip-url-encoding";
        
        // Refactor -> Namer
        private static readonly Regex SplitPattern = new Regex(@"(\p{Lu}\p{Ll}+)");

        private static Dictionary<string, string> plural = new Dictionary<string, string>()
        {
            { "eventhub", "eventhubs" }
        };

        /////////////////////////////////////////////////////////////////////////////////////////
        //
        // General Extensions
        //
        /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method changes string to sentence where is make the first word 
        /// of sentence to lowercase. The sentence is coming directly from swagger.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSentence(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            else
            {
                value = value.Trim();
                return value.First().ToString().ToLowerInvariant() + (value.Length > 1 ? value.Substring(1) : "");
            }
        }

        /// <summary>
        /// Makes first word of the sentence as upper case.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Capitalize(this string value)
        {
            return string.IsNullOrWhiteSpace(value)
                    ? string.Empty
                    : value.First()
                           .ToString()
                           .ToUpperInvariant() + (value.Length > 1 ? value.Substring(1) : "");
        }

        /// <summary>
        /// String manipulation function converts all words in a sentence to lowercase.
        /// Refactor -> Namer
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToPhrase(this string value)
        {
            List<string> words = new List<string>(value.ToWords());
            for (int i = 0; i < words.Count; i++)
            {
                words[i] = words[i].ToLowerInvariant();
            }
            return string.Join(" ", words.ToArray());
        }
        
        /// <summary>
        /// Split sentence into words.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string[] ToWords(this string value)
        {
            return SplitPattern.Split(value).Where(s => !string.IsNullOrEmpty(s)).ToArray();
        }

        /// <summary>
        /// This method checks if MethodGroupName is plural of package name. 
        /// It returns false for packages not listed in dictionary 'plural'.
        /// Example, group EventHubs in package EventHub.
        /// Refactor -> Namer, but also could be used by the CodeModelTransformer
        /// </summary>
        /// <param name="value"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public static bool IsNamePlural(this string value, string packageName)
        {
            return plural.ContainsKey(packageName) && plural[packageName] == value.ToLower();
        }

        /// <summary>
        /// Gets substring from value string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TrimStartsWith(this string value, string s)
        {
            if (!string.IsNullOrEmpty(s) && s.Length < value.Length && value.StartsWith(s, StringComparison.InvariantCultureIgnoreCase))
            {
                value = value.Substring(s.Length);
            }
            return value;
        }

        /// <summary>
        /// Gets package name from the value string.
        /// Refactor -> Namer, but could be used by the CodeModelTransformer too
        /// </summary>
        /// <param name="value"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public static string TrimPackageName(this string value, string packageName)
        {
            var cchValue = value.Length;
            value = value.TrimStartsWith(packageName);
            if (value.Length == cchValue && packageName.EndsWith("s") && (value.Length - packageName.Length + 1) > 1)
            {
                value = value.TrimStartsWith(packageName.Substring(0, packageName.Length-1));
            }
            return value;
        }

        /// <summary>
        /// Converts List to formatted string of arguments.
        /// Refactor -> Generator
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static string EmitAsArguments(this IList<string> arguments)
        {
            var sb = new StringBuilder();

            if (arguments.Count() > 0)
            {
                sb.Append(arguments[0]);

                for (var i = 1; i < arguments.Count(); i++)
                {
                    sb.AppendLine(",");
                    sb.Append(arguments[i]);
                }
            }
            return sb.ToString();
        }


        // This function removes html anchor tags and reformats the comment text.
        // For example, Swagger documentation text --> "This is a documentation text. For information see <a href=LINK">CONTENT.</a>"
        // reformats to  --> "This is a documentation text. For information see CONTENT (LINK)."
        // Refactor -> Namer
        // Still, nobody uses this...
        public static string UnwrapAnchorTags(this string comments)
        {
            string pattern = "([^<>]*)<a\\s*.*\\shref\\s*=\\s*[\'\"]([^\'\"]*)[\'\"][^>]*>(.*)</a>";
            Regex r = new Regex(pattern);
            Match match = r.Match(comments);

            if (match.Success)
            {
                string content = match.Groups[3].Value;
                string link = match.Groups[2].Value;

                return (".?!;:".Contains(content[content.Length - 1])
                        ? match.Groups[1].Value + content.Substring(0, content.Length - 1) + " (" + link + ")" + content[content.Length - 1]
                        : match.Groups[1].Value + content + " (" + link + ")");
            }

            return comments;
        }
        
        /////////////////////////////////////////////////////////////////////////////////////////
        //
        // Parameter Extensions
        //
        /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Return a Go map of required parameters.
        // Refactor -> Generator
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="mapVariable"></param>
        /// <returns></returns>
        public static string BuildParameterMap(this IEnumerable<Parameter> parameters, string mapVariable)
        {
            var builder = new StringBuilder();

            builder.Append(mapVariable);
            builder.Append(" := map[string]interface{} {");

            if (parameters.Count() > 0)
            {
                builder.AppendLine();
                var indented = new IndentedStringBuilder("  ");
                parameters
                    .Where(p => p.IsRequired)
                    .OrderBy(p => p.SerializedName)
                    .ForEach(p => indented.AppendLine("\"{0}\": {1},", p.NameForMap(), p.ValueForMap()));
                builder.Append(indented);
            }
            builder.AppendLine("}");
            return builder.ToString();
        }

        /// <summary>
        /// Return string with formatted Go map.
        /// xyz["abc"] = 123
        /// Refactor -> Generator
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="mapVariable"></param>
        /// <returns></returns>
        public static string AddToMap(this Parameter parameter, string mapVariable)
        {
            return string.Format("{0}[\"{1}\"] = {2}", mapVariable, parameter.NameForMap(), parameter.ValueForMap());
        }

        /// <summary>
        /// Add imports for the parameter in parameter type.
        /// Refactor -> Probably CodeModelTransformer, still, 0 references
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="imports"></param>
        public static void AddImports(this Parameter parameter, HashSet<string> imports)
        {
            parameter.ModelType.AddImports(imports);
        }

        /// <summary>
        /// Get Name for parameter for Go map. 
        /// If parameter is client parameter, then return client.<parametername>
        /// Refactor -> Generator
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static string NameForMap(this Parameter parameter)
        {
            return IsApiVersion(parameter.SerializedName.FixedValue)
                       ? ApiVersionSerializedName
                        : parameter.SerializedName.FixedValue;
        }

        /// <summary>
        /// Return formatted value string for the parameter.
        /// Refactor -> Namer and generator
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static string ValueForMap(this Parameter parameter)
        {
            if (IsApiVersion(parameter.SerializedName))
            {
                return "client." + ApiVersionName;
            }
            var value = parameter.IsClientProperty()
                ? "client." + GoCodeNamer.Instance.PascalCase(parameter.Name.FixedValue)
                : parameter.Name.FixedValue;

            var format = parameter.IsRequired || parameter.ModelType.CanBeEmpty()
                                          ? "{0}"
                                          : "*{0}";

            var s = parameter.CollectionFormat != CollectionFormat.None
                                  ? $"{format},\"{parameter.CollectionFormat.GetSeparator()}\""
                                  : $"{format}";

            return string.Format(
                parameter.RequiresUrlEncoding()
                    ? $"autorest.Encode(\"{parameter.Location.ToString().ToLower()}\",{s})"
                    : $"{s}",
                value);
        }

        /// <summary>
        /// Return list of parameters for specified location passed in an argument.
        /// Refactor -> Probably CodeModeltransformer, but even with 5 references, the other mkethods are not used anywhere
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static IEnumerable<Parameter> ByLocation(this IEnumerable<Parameter> parameters, ParameterLocation location)
        {
            return parameters
                .Where(p => p.Location == location)
                .AsEnumerable();
        }

        /// <summary>
        /// Return list of retuired parameters for specified location passed in an argument.
        /// Refactor -> CodeModelTransformer, still, 3 erefences, but no one uses the other methods.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static IEnumerable<Parameter> ByLocationAsRequired(this IEnumerable<Parameter> parameters, ParameterLocation location, bool isRequired)
        {
            return parameters
                .Where(p => p.Location == location && p.IsRequired == isRequired)
                .AsEnumerable();
        }

        /// <summary>
        /// Return list of parameters as per their location in request.
        /// Refactor -> CodeModelGenerator, 0 references
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Parameter BodyParameter(this IEnumerable<Parameter> parameters)
        {
            var bodyParameters = parameters.ByLocation(ParameterLocation.Body);
            return bodyParameters.Count() > 0
                    ? bodyParameters.First()
                    : null;
        }

        /// Refactor -> CodeModelGenerator, 0 references
        public static IEnumerable<Parameter> BodyParameters(this IEnumerable<Parameter> parameters, bool isRequired)
        {
            return parameters.ByLocationAsRequired(ParameterLocation.Body, isRequired);
        }

        /// Refactor -> CodeModelGenerator, 0 references
        public static IEnumerable<Parameter> FormDataParameters(this IEnumerable<Parameter> parameters)
        {
            return parameters.ByLocation(ParameterLocation.FormData);
        }

        /// Refactor -> CodeModelGenerator, 0 references
        public static IEnumerable<Parameter> HeaderParameters(this IEnumerable<Parameter> parameters)
        {
            return parameters.ByLocation(ParameterLocation.Header);
        }

        /// Refactor -> CodeModelGenerator, 0 references
        public static IEnumerable<Parameter> HeaderParameters(this IEnumerable<Parameter> parameters, bool isRequired)
        {
            return parameters.ByLocationAsRequired(ParameterLocation.Header, isRequired);
        }

        /// Refactor -> CodeModelGenerator, 0 references
        public static IEnumerable<Parameter> PathParameters(this IEnumerable<Parameter> parameters)
        {
            return parameters.ByLocation(ParameterLocation.Path);
        }

        /// Refactor -> CodeModelGenerator, 0 references
        public static IEnumerable<Parameter> QueryParameters(this IEnumerable<Parameter> parameters)
        {
            return parameters.ByLocation(ParameterLocation.Query);
        }

        /// Refactor -> CodeModelGenerator, 0 references
        public static IEnumerable<Parameter> QueryParameters(this IEnumerable<Parameter> parameters, bool isRequired)
        {
            return parameters.ByLocationAsRequired(ParameterLocation.Query, isRequired);
        }

        /// <summary>
        /// Return the separator associated with a given collectionFormat
        /// It looks like other generators use this for split / join operations ?
        /// Refactor -> I think CodeMoedelTransformer
        /// </summary>
        /// <param name="format">The collection format</param>
        /// <returns>The separator</returns>
        private static string GetSeparator(this CollectionFormat format)
        {
            switch (format)
            {
                case CollectionFormat.Csv:
                    return ",";
                case CollectionFormat.Pipes:
                    return "|";
                case CollectionFormat.Ssv:
                    return " ";
                case CollectionFormat.Tsv:
                    return "\t";
                default:
                    throw new NotSupportedException(string.Format("Collection format {0} is not supported.", format));
            }
        }

        // Refactor -> CodeModelTransformer
        public static bool IsClientProperty(this Parameter parameter)
        {
            return parameter.ClientProperty != null || IsApiVersion(parameter.SerializedName);
        }

        public static string GetParameterName(this ParameterGo parameter)
        {
            return parameter.IsClientProperty()
                            ? "client." + Capitalize(parameter.Name.FixedValue)
                            : parameter.Name.FixedValue;
        }

        public static bool IsMethodArgument(this Parameter parameter)
        {
            return !parameter.IsClientProperty();
        }

        public static bool IsApiVersion(this string name)
        {
            string rgx = @"^api[^a-zA-Z0-9_]?version";
            return Regex.IsMatch(name, rgx, RegexOptions.IgnoreCase);
        }

        public static bool RequiresUrlEncoding(this Parameter parameter)
        {
            return (parameter.Location == ParameterLocation.Query || parameter.Location == ParameterLocation.Path) && !parameter.Extensions.ContainsKey(SkipUrlEncoding);
        }

        public static string JsonTag(this Property property, bool omitEmpty = true)
        {
            return string.Format("`json:\"{0}{1}\"`", property.SerializedName, omitEmpty ? ",omitempty" : "");
        }

        /////////////////////////////////////////////////////////////////////////////////////////
        //
        // Type Extensions
        //
        /////////////////////////////////////////////////////////////////////////////////////////

        public static bool IsStreamType(this IModelType body)
        {
            var r = body as CompositeTypeGo;
            return r != null && (r.BaseType.PrimaryType(KnownPrimaryType.Stream));
        }


        public static bool PrimaryType(this IModelType type, KnownPrimaryType typeToMatch)
        {
            if (type == null)
            {
                return false;
            }

            PrimaryType primaryType = type as PrimaryType;
            return primaryType != null && primaryType.KnownPrimaryType == typeToMatch;
        }

        public static bool CanBeEmpty(this IModelType type)
        {
            var dictionaryType = type as DictionaryType;
            var interfaceType = type as InterfaceType;
            var primaryType = type as PrimaryType;
            var sequenceType = type as SequenceType;
            var enumType = type as EnumType;

            return dictionaryType != null
                || interfaceType !=  null
                || (    primaryType != null
                 && (primaryType.KnownPrimaryType == KnownPrimaryType.ByteArray
                        || primaryType.KnownPrimaryType == KnownPrimaryType.Stream
                        || primaryType.KnownPrimaryType == KnownPrimaryType.String))
                || sequenceType != null
                || enumType != null;
        }

        public static bool CanBeNull(this IModelType type)
        {
            var dictionaryType = type as DictionaryType;
            var interfaceType = type as InterfaceType;
            var primaryType = type as PrimaryType;
            var sequenceType = type as SequenceType;

            return dictionaryType != null
                || interfaceType != null
                || (    primaryType != null
                   && (primaryType.KnownPrimaryType == KnownPrimaryType.ByteArray
                      || primaryType.KnownPrimaryType == KnownPrimaryType.Stream))
                || sequenceType != null;
        }
        
        public static string GetEmptyCheck(this IModelType type, string valueReference, bool asEmpty = true)
        {
            if (type is PrimaryType)
            {
                return (type as PrimaryType).GetEmptyCheck(valueReference, asEmpty);
            }
            else if (type is SequenceType)
            {
                return (type as SequenceType).GetEmptyCheck(valueReference, asEmpty);
            }
            else if (type is DictionaryType)
            {
                return (type as DictionaryType).GetEmptyCheck(valueReference, asEmpty);
            }
            else if (type is EnumType)
            {
                return (type as EnumType).GetEmptyCheck(valueReference, asEmpty);
            }
            else
            {
                return string.Format(asEmpty
                                        ? "{0} == nil"
                                        : "{0} != nil", valueReference);
            }
        }

        public static string GetEmptyCheck(this DictionaryType type, string valueReference, bool asEmpty)
        {
            return string.Format(asEmpty
                                    ? "{0} == nil || len({0}) == 0"
                                    : "{0} != nil && len({0}) > 0", valueReference);
        }

        public static string GetEmptyCheck(this EnumType type, string valueReference, bool asEmpty)
        {
            return string.Format(asEmpty
                                    ? "len(string({0})) == 0"
                                    : "len(string({0})) > 0", valueReference);
        }

        public static string GetEmptyCheck(this PrimaryType type, string valueReference, bool asEmpty)
        {
            if (type.PrimaryType(KnownPrimaryType.ByteArray))
            {
                return string.Format(asEmpty
                                        ? "{0} == nil || len({0}) == 0"
                                        : "{0} != nil && len({0}) > 0", valueReference);
            }
            else if (type.PrimaryType(KnownPrimaryType.String))
            {
                return string.Format(asEmpty
                                        ? "len({0}) == 0"
                                        : "len({0}) > 0", valueReference);
            }
            else
            {
                return string.Format(asEmpty
                                        ? "{0} == nil"
                                        : "{0} != nil", valueReference);
            }
        }

        public static string GetEmptyCheck(this SequenceType type, string valueReference, bool asEmpty)
        {
            return string.Format(asEmpty
                                    ? "{0} == nil || len({0}) == 0"
                                    : "{0} != nil && len({0}) > 0", valueReference);
        }

        public static string GetNullCheck(this IModelType type, string valueReference, bool asNull = true)
        {
            return string.Format(asNull
                                        ? "{0} == nil"
                                        : "{0} != nil", valueReference);
        }

        public static string Fields(this CompositeType compositeType)
        {
            var indented = new IndentedStringBuilder("    ");
            var properties = compositeType.Properties;

            if (compositeType.BaseModelType != null)
            {
                indented.Append(compositeType.BaseModelType.Fields());
            }

            // If the type is a paged model type, ensure the nextLink field exists
            // Note: Inject the field into a copy of the property list so as to not pollute the original list
            if (    compositeType is ModelGo
                &&  !String.IsNullOrEmpty((compositeType as ModelGo).NextLink))
            {
                var nextLinkField = (compositeType as ModelGo).NextLink;
                foreach (Property p in properties) {
                    p.Name = GoCodeNamer.PascalCaseWithoutChar(p.Name, '.');
                    if (p.Name.EqualsIgnoreCase(nextLinkField)) {
                        p.Name = nextLinkField;
                    }
                }
                if (!properties.Any(p => p.Name.EqualsIgnoreCase(nextLinkField)))
                {
                    // var property = new Property();
                    // property.Name = nextLinkField;
                    // property.ModelType = new PrimaryType(KnownPrimaryType.String) { Name = "string" };
                    // properties = new List<Property>(properties);
                    // properties.Add(property);
                    var property = New<Property>();
                    property.Name = nextLinkField;
                    property.ModelType = New<PrimaryType>(KnownPrimaryType.String);
                    // property.ModelType.Name = "string";
                    properties.ToList().Add(property);
                }
            }

            // Emit each property, except for named Enumerated types, as a pointer to the type
            foreach (var property in properties)
            {
                EnumType enumType = property.ModelType as EnumType;
                if (enumType != null && enumType.IsNamed())
                {
                    indented.AppendFormat("{0} {1} {2}\n",
                                    property.Name,
                                    enumType.Name,
                                    property.JsonTag());

                }
                else if (property.ModelType is DictionaryType)
                {
                    indented.AppendFormat("{0} *{1} {2}\n", property.Name, (property.ModelType as DictionaryTypeGo).FieldName, property.JsonTag());
                }
                else
                {
                    indented.AppendFormat("{0} *{1} {2}\n", property.Name, property.ModelType.Name, property.JsonTag());
                }
            }
            
            return indented.ToString();
        }

        public static string PreparerMethodName(this CompositeType compositeType)
        {
            return compositeType.Name + "Preparer";
        }

        public static bool IsNamed(this EnumType enumType)
        {
            return !string.IsNullOrEmpty(enumType.Name)
                && enumType.Name != "string"
                && enumType.Values.Count > 0;
        }

        /// <summary>
        /// Add imports for a type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="imports"></param>
        public static void AddImports(this IModelType type, HashSet<string> imports)
        {
            if (type is DictionaryType)
            {
                (type as DictionaryType).AddImports(imports);
            }
            else if (type is PackageType)
            {
                (type as PackageType).AddImports(imports);
            }
            else if (type is PrimaryType)
            {
                (type as PrimaryType).AddImports(imports);
            }
            else if (type is SequenceType)
            {
                (type as SequenceType).AddImports(imports);
            }
        }

        /// <summary>
        /// Add imports for composite types.
        /// </summary>
        /// <param name="compositeType"></param>
        /// <param name="imports"></param>
        public static void AddImports(this CompositeType compositeType, HashSet<string> imports)
        {
            compositeType
                .Properties
                .ForEach(p => p.ModelType.AddImports(imports));
        }

        /// <summary>
        /// Add imports for dictionary type.
        /// </summary>
        /// <param name="dictionaryType"></param>
        /// <param name="imports"></param>
        public static void AddImports(this DictionaryType dictionaryType, HashSet<string> imports)
        {
            dictionaryType.ValueType.AddImports(imports);
        }

        /// <summary>
        /// Add imports for package type.
        /// </summary>
        /// <param name="packageType"></param>
        /// <param name="imports"></param>
        public static void AddImports(this PackageType packageType, HashSet<string> imports)
        {
            imports.Add(packageType.Import);
        }

        /// <summary>
        /// Add imports for primary type.
        /// </summary>
        /// <param name="primaryType"></param>
        /// <param name="imports"></param>
        public static void AddImports(this PrimaryType primaryType, HashSet<string> imports)
        {
            if (primaryType.KnownPrimaryType == KnownPrimaryType.Stream)
            {
                imports.Add("io");
            }
        }

        /// <summary>
        /// Add imports for sequence type.
        /// </summary>
        /// <param name="sequenceType"></param>
        /// <param name="imports"></param>
        public static void AddImports(this SequenceType sequenceType, HashSet<string> imports)
        {
            sequenceType.ElementType.AddImports(imports);
        }


        /////////////////////////////////////////////////////////////////////////////////////////
        // Validate code
        //
        // This code generates a validation object which is defined in 
        // go-autorest/autorest/validation package and is used to validate 
        // constraints. 
        // See PR: https://github.com/Azure/go-autorest/tree/master/autorest/validation
        //
        /////////////////////////////////////////////////////////////////////////////////////////

        public static string Validate(this IEnumerable<Parameter> parameters, HttpMethod method)
        {
            List<string> v = new List<string>();
            HashSet<string> ancestors = new HashSet<string>();

            foreach (var p in parameters)
            {
                var name = IsApiVersion(p.SerializedName)
                    ? "client." + ApiVersionName
                    : !p.IsClientProperty()
                        ? p.Name.FixedValue
                        : "client." + Capitalize(p.Name.FixedValue);

                List<string> x = new List<string>();
                if (p.ModelType is CompositeType)
                {
                    ancestors.Add(p.ModelType.Name);
                    x.AddRange(p.ValidateCompositeType(name, method, ancestors));
                    ancestors.Remove(p.ModelType.Name);
                }
                else
                    x.AddRange(p.ValidateType(name, method));

                if (x.Count != 0)
                    v.Add($"{{ TargetValue: {name},\n Constraints: []validation.Constraint{{{string.Join(",\n", x)}}}}}");
            }
            return string.Join(",\n", v);
        }

        /// <summary>
        /// Return list of validations for primary, map, sequence and rest of the types.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="name"></param>
        /// <param name="method"></param>
        /// <param name="isCompositeProperties"></param>
        /// <returns></returns>
        public static List<string> ValidateType(this IVariable p, string name, HttpMethod method,
            bool isCompositeProperties = false)
        {
            List<string> x = new List<string>();
            if (method != HttpMethod.Patch || !p.IsBodyParameter() || isCompositeProperties)
            {
                x.AddRange(p.Constraints.Select(c => GetConstraint(name, c.Key.ToString(), c.Value)).ToList());
            }

            List<string> y = new List<string>();
            if (x.Count > 0)
            {
                if (p.CheckNull() || isCompositeProperties)
                    y.AddRange(x.AddChain(name, NullConstraint, p.IsRequired));
                else
                    y.AddRange(x);
            }
            else
            {
                if (p.IsRequired && (p.CheckNull() || isCompositeProperties))
                    y.AddNullValidation(name, p.IsRequired);
            }
            return y;
        }

        /// <summary>
        /// Return list of validations for composite type.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="name"></param>
        /// <param name="method"></param>
        /// <param name="ancestors"></param>
        /// <param name="isCompositeProperties"></param>
        /// <returns></returns>
        public static List<string> ValidateCompositeType(this IVariable p, string name, HttpMethod method, HashSet<string> ancestors,
            bool isCompositeProperties = false)
        {
            List<string> x = new List<string>();
            if (method != HttpMethod.Patch || !p.IsBodyParameter() || isCompositeProperties)
            {
                foreach (var prop in ((CompositeType)p.ModelType).Properties)
                {
                    var primary = prop.ModelType as PrimaryType;
                    var sequence = prop.ModelType as SequenceType;
                    var map = prop.ModelType as DictionaryTypeGo;
                    var composite = prop.ModelType as CompositeType;

                    if (primary != null || sequence != null || map != null)
                        x.AddRange(prop.ValidateType($"{name}.{prop.Name}", method, true));
                    else if (composite != null)
                    {
                        if (ancestors.Contains(composite.Name))
                        {
                            x.AddNullValidation($"{name}.{prop.Name}", p.IsRequired);
                        }
                        else
                        {
                            ancestors.Add(composite.Name);
                            x.AddRange(prop.ValidateCompositeType($"{name}.{prop.Name}", method, ancestors, true));
                            ancestors.Remove(composite.Name);
                        }  
                    }   
                }
            }

            x.AddRange(from prop in ((CompositeType)p.ModelType).Properties
                       where prop.IsReadOnly
                       select GetConstraint($"{name}.{prop.Name}", ReadOnlyConstraint, "true"));

            List<string> y = new List<string>();
            if (x.Count > 0)
            {
                if (p.CheckNull() || isCompositeProperties)
                    y.AddRange(x.AddChain(name, NullConstraint, p.IsRequired));
                else
                    y.AddRange(x);
            }
            else
            {
                if (p.IsRequired && (p.CheckNull() || isCompositeProperties))
                    y.AddNullValidation(name, p.IsRequired);
            }
            return y;
        }

        /// <summary>
        /// Add null validation in validation object.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="name"></param>
        /// <param name="isRequired"></param>
        public static void AddNullValidation(this List<string> v, string name, bool isRequired = false)
        {
            v.Add(GetConstraint(name, NullConstraint, $"{isRequired}".ToLower()));
        }

        /// <summary>
        /// Add chain of validation for composite type.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="name"></param>
        /// <param name="constraint"></param>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        public static List<string> AddChain(this List<string> x, string name, string constraint, bool isRequired)
        {
            List<string> a = new List<string>
            {
                GetConstraint(name, constraint, $"{isRequired}".ToLower(), true),
                $"Chain: []validation.Constraint{{{x[0]}"
            };
            a.AddRange(x.GetRange(1, x.Count - 1));
            a.Add("}}");
            return a;
        }
        /// <summary>
        /// CheckNull 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        // Check if type is not a null or pointer type.
        public static bool CheckNull(this IVariable p)
        { 
            return p is Parameter && (p.ModelType.IsNullValueType() || !(p.IsRequired || p.ModelType.CanBeEmpty()));
        }

        /// <summary>
        ///  Check whether a type is empty type.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsEmptyValueType(this IModelType t)
        {
            var dictionaryType = t as DictionaryType;
            var interfaceType = t as InterfaceType;
            var primaryType = t as PrimaryType;
            var sequenceType = t as SequenceType;
            var enumType = t as EnumType;

            return dictionaryType != null
                || interfaceType != null
                || (primaryType != null
                 && (primaryType.KnownPrimaryType == KnownPrimaryType.ByteArray
                        || primaryType.KnownPrimaryType == KnownPrimaryType.String))
                || sequenceType != null
                || enumType != null;
        }

        /// <summary>
        /// Check whether a type is nullable type.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsNullValueType(this IModelType t)
        {
            var dictionaryType = t as DictionaryType;
            var interfaceType = t as InterfaceType;
            var primaryType = t as PrimaryType;
            var sequenceType = t as SequenceType;

            return dictionaryType != null
                || interfaceType != null
                || (primaryType != null
                   && primaryType.KnownPrimaryType == KnownPrimaryType.ByteArray)
                || sequenceType != null;
        }

        /// <summary>
        /// Check if parameter is a body parameter.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsBodyParameter(this IVariable p)
        {
            return p is Parameter && ((Parameter)p).Location == ParameterLocation.Body;
        }

        /// <summary>
        /// Construct validation string for validation object for the passed constraint.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="constraintName"></param>
        /// <param name="constraintValue"></param>
        /// <param name="chain"></param>
        /// <returns></returns>
        public static string GetConstraint(string name, string constraintName, string constraintValue, bool chain = false)
        {
            var value = constraintName == Constraint.Pattern.ToString()  
                                          ? $"`{constraintValue}`"
                                          : constraintValue;
            return string.Format(chain
                                    ? "\t{{Target: \"{0}\", Name: validation.{1}, Rule: {2} "
                                    : "\t{{Target: \"{0}\", Name: validation.{1}, Rule: {2}, Chain: nil }}",
                                    name, constraintName, value);
        }

        /////////////////////////////////////////////////////////////////////////////////////////
        //
        // ClientModel Extensions
        //
        /////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Check if method belongs to group in the package.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="groupName"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public static bool BelongsToGroup(this Method method, string groupName, string packageName)
        {
            return string.IsNullOrEmpty(groupName)
                     ? string.IsNullOrEmpty(method.Group)
                     : string.Equals(method.Group, groupName);
        }

        /// <summary>
        /// Check if method has a return response.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool HasReturnValue(this Method method)
        {
            return method.ReturnValue() != null && method.ReturnValue().Body != null;
        }

        /// <summary>
        /// Return response object for the method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static Response ReturnValue(this Method method)
        {
            return method.ReturnType != null
                    ? method.ReturnType
                    : method.DefaultResponse;
        }

        /// <summary>
        /// Checks if method has pageable extension (x-ms-pageable) enabled.  
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>

        public static bool IsPageable(this Method method)
        {
            return !string.IsNullOrEmpty(method.NextLink());
        }

        /// <summary>
        /// Checks if method for next page of results on paged methods is already present in the method list.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="methods"></param>
        /// <returns></returns>
        public static bool NextMethodExists(this Method method, IEnumerableWithIndex<Method> methods) {
            if (method.Extensions.ContainsKey(AzureExtensions.PageableExtension))
            {
                var pageableExtension = JsonConvert.DeserializeObject<PageableExtension>(method.Extensions[AzureExtensions.PageableExtension].ToString());
                if (pageableExtension != null && !string.IsNullOrWhiteSpace(pageableExtension.OperationName))
                {
                    return methods.Any(m => m.SerializedName.EqualsIgnoreCase(pageableExtension.OperationName));
                }
            }
            return false;
        }

        /// <summary>
        /// Check if method has long running extension (x-ms-long-running-operation) enabled. 
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool IsLongRunningOperation(this Method method)
        {
            try {
                return method.Extensions.ContainsKey(AzureExtensions.LongRunningExtension) && (bool)method.Extensions[AzureExtensions.LongRunningExtension];
            }
            catch(InvalidCastException e)
            {
                throw new Exception(string.Format(e.Message+" The value \'{0}\' for extension {1} for method {2} is invalid in Swagger. It should be boolean.",
                            method.Extensions[AzureExtensions.LongRunningExtension], AzureExtensions.LongRunningExtension, method.Group + "." + method.Name));
            }
        }

        /// <summary>
        /// Add NextLink attribute for pageable extension for the method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string NextLink(this Method method)
        {
            var nextLink = "";

            // Note:
            // -- The CSharp generator applies a default link name if the extension is present but the link name is not.
            //    Yet, the MSDN for methods whose nextLink is missing are not paged methods. It appears the CSharp code is
            //    outdated vis a vis the specification.
            // TODO (gosdk): Ensure obtaining the nextLink is correct.
            if (method.Extensions.ContainsKey(AzureExtensions.PageableExtension))
            {
                var pageableExtension = method.Extensions[AzureExtensions.PageableExtension] as Newtonsoft.Json.Linq.JContainer;
                if (pageableExtension != null)
                {
                    var nextLinkName = (string)pageableExtension["nextLinkName"];
                    if (!string.IsNullOrEmpty(nextLinkName))
                    {
                        nextLink = GoCodeNamer.PascalCaseWithoutChar(nextLinkName, '.');
                    }
                }
            }

            return nextLink;
        }

       /// <summary>
       /// This method that user-defined method names do not conflict with parameter
       /// defined in swagger. If there is a conflict, it adds package name before method name. 
       /// For example, StorageVersion().
       /// </summary>
       /// <param name="model"></param>
       /// <param name="attributeName"></param>
       /// <param name="packageName"></param>
       /// <returns></returns>
        public static string UpdateNameIfDuplicate(this CodeModel cm, string attributeName, string packageName)
        { 
            bool isDuplicateEnumName = false;
            foreach (var e in cm.EnumTypes)
            {
                if (e.Values.Any(v => v.Name == attributeName))
                {
                    isDuplicateEnumName = true;
                }
                if (isDuplicateEnumName)
                    break;
            }

            return cm.ModelTypes.Any(p => p.Name == attributeName) || isDuplicateEnumName
                ? $"{attributeName}{packageName.Capitalize()}"
                : attributeName;
        }

    }
}
