using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace SharedScriptsApi.Extensions
{
    public static class JTokenExtends
    {
        private static string AssemblyFormat => "Saltus.digiTICKET.Data{0}";
        private static string DefaultVersion => "0110000000";
        private static string DefaultModelAssemblyName => ModelAssemblyFormatter(DefaultVersion);
        public static string ModelAssemblyFormatter(string version) => string.Format(AssemblyFormat, version);
        public static Assembly DefaultModelAssembly => Assembly.Load(DefaultModelAssemblyName);
        public static Assembly GetVersionedModelAssembly(string version) => Assembly.Load(ModelAssemblyFormatter(version));

        public static bool TryGetValue<T>(this JObject source, string propertyName, StringComparison comparison, out T result)
        {
            result = default;
            return source.TryGetValue(propertyName, comparison, out JToken token) &&
                token.TryConvertTo(out result);
        }

        public static bool TryGetValue<T>(this JObject source, string propertyName, StringComparison comparison, string implAssemblyName, out T result)
        {
            result = default;
            return source.TryGetValue(propertyName, comparison, out JToken token) &&
                token.TryConvertTo(implAssemblyName, out result);
        }

        public static bool TryGetValue<T>(this JObject source, string propertyName, StringComparison comparison, Assembly implAssembly, out T result)
        {
            result = default;
            return source.TryGetValue(propertyName, comparison, out JToken token) &&
                token.TryConvertTo(serializer: null, implAssembly, out result);
        }

        public static bool TryGetValue<T>(this JToken source, string propertyName, StringComparison comparison, out T result)
        {
            result = default;
            return source is JObject jobject && TryGetValue(jobject, propertyName, comparison, out result);
        }

        /// <summary>
        /// Checks if a conversion from the supplied <see cref="JToken"/> to a <typeparamref name="T"/> can be made.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="source">The <see cref="JToken"/>.</param>
        public static bool CanConvertTo<T>(this JToken source)
        {
            return source.TryConvertTo<T>(out _);
        }
        /// <summary>
        /// Checks if a conversion from the supplied <see cref="JToken"/> to a <typeparamref name="TType"/> can be made.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="source">The <see cref="JToken"/>.</param>
        /// <param name="result">The result.</param>
        /// <returns>true is the conversion is successful</returns>
        public static bool TryConvertTo<T>(this JToken source, out T result)
        {
            return TryConvertTo(source, serializer: null, null, out result);
        }

        public static bool TryConvertTo<T>(this JToken source, string implAssemblyName, out T result)
        {
            return TryConvertTo(source, serializer: null, Assembly.Load(implAssemblyName), out result);
        }


        public static bool TryConvertTo<T>(this JToken source, JsonSerializerSettings settings, Assembly assembly, out T result)
        {
            var serializer = JsonSerializer.Create(settings);
            return TryConvertTo(source, serializer, assembly, out result);
        }

        /// <summary>
        /// Checks if a conversion from the supplied <see cref="JToken"/> to a <typeparamref name="TType"/> can be made.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="source">The <see cref="JToken"/>.</param>
        /// <param name="result">The result.</param>
        /// <returns>true is the conversion is successful</returns>
        public static bool TryConvertTo<T>(this JToken source, JsonSerializer serializer, Assembly assembly, out T result)
        {
            bool retVal = false;
            result = default;
            Type typeT = typeof(T);

            // if the result is an interface
            if (typeT.IsInterface)
            {
                // if the assembly/assembly name has been provided
                // this uses some fuzzy logic to get the type that implements the interface
                // this assumes that propertyName supplied in TryGetValue is the name of the type that implements the interface
                // [WebTeam] if there is a use case not using the above assumption,
                //          an overload of the TryGetValue method can be created to include the type/type name of the implementing class
                //          and more processing will need to be done to get the type that implements the interface

                if (assembly != null)
                {
                    object impl = null;
                    Type implType = null;

                    try
                    {
                        // get the first type that implements the interface from the assembly
                        var implTypes = assembly
                            .GetTypes()
                            .Where(p => typeT
                                        .IsAssignableFrom(p) &&
                                        !p.IsInterface).ToList();
                        // if there is more than one type that inmpements the interface
                        if (implTypes.Count() > 1)
                        {
                            // get the type by the name of the path of the JToken
                            implType = implTypes
                                .Where(p => p.Name.Equals(source.Path, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        }
                        else
                        {
                            // else we get first type that implements the interface
                            implType = implTypes.FirstOrDefault();

                        }

                        if (implType != null)
                        {
                            impl = source.ToObject(implType, serializer ?? JsonSerializer.CreateDefault());
                            result = (T)impl;
                            retVal = true;
                        }
                        else
                        {
                            result = default;
                            retVal = false;
                        }
                    }
                    catch
                    {
                        retVal = false;
                        result = default;
                    }
                }
                else
                {
                    object impl = null;
                    Type implType = null;
                    try
                    {
                        // limit assemblies to the Saltus namespace
                        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                            .Where(x => x.GetName().Name.StartsWith("Saltus"));
                        // Get all types that implement the interface
                        var implTypes = assemblies
                            .SelectMany(s => s.GetTypes())
                            .Where(p => typeT
                                        .IsAssignableFrom(p) &&
                                        !p.IsInterface).ToList();

                        // if there is more than one type
                        if (implTypes.Count() > 1)
                        {
                            // Get the type by the name of the path of the JToken
                            var implTypesFromName = implTypes
                                .Where(p => p.Name.Equals(source.Path, StringComparison.OrdinalIgnoreCase)).ToList();

                            // if there is more than one type with the same name    
                            if (implTypesFromName.Count() > 1)
                            {
                                // use the default Model assembly
                                var asssembly = Assembly.Load(DefaultModelAssemblyName);
                                // we can assume that only one type with the path name will be in the default model assembly
                                // so get the first one from the assembly
                                implType = implTypesFromName.Where(x => x.Assembly == asssembly).FirstOrDefault();
                            }
                            else
                            {
                                // get the first type that implements the interface
                                implType = implTypes.FirstOrDefault();
                            }
                        }
                        else
                        {
                            // get the first type that implements the interface
                            implType = implTypes.FirstOrDefault();
                        }

                        if (implType != null)
                        {
                            impl = source.ToObject(implType, serializer ?? JsonSerializer.CreateDefault());
                            result = (T)impl;
                            retVal = true;
                        }
                        else
                        {
                            result = default;
                            retVal = false;
                        }
                    }
                    catch
                    {
                        result = default;
                        retVal = false;
                    }
                }
            }
            else
            {
                JToken temp = source;
                if (temp is JProperty jProperty)
                {
                    temp = jProperty.Value;
                }
                object sourceValue = temp is JValue jvalue
                    ? jvalue.Value
                    : source;

                if (sourceValue == null)
                {
                    result = default;
                    retVal = typeT.IsClass || typeT.GetGenericTypeDefinition() == typeof(Nullable<>);
                }
                else if (sourceValue.TryCast(out T cast))
                {
                    result = cast;
                    retVal = true;
                }
                else
                {
                    try
                    {
                        result = source.ToObject<T>(serializer ?? JsonSerializer.CreateDefault());
                        retVal = !Equals(result, default);
                    }
                    catch
                    {
                        result = default;
                        retVal = false;
                    }
                }
            }


            return retVal;
        }



    }
}
