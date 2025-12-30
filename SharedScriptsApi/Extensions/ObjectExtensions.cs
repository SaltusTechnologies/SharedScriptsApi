using Newtonsoft.Json.Linq;
using SharedScriptsApi.Extensions;
using SharedScriptsApi.Helpers;
using System.Reflection;

public static class ObjectExtensions
{
    public static bool TryGetProperty<T>(this object instance, string path, out T result)
    {
        result = default;

        try
        {
            if (instance == null || path == null)
            {
                return false;
            }

            var json = JObject.FromObject(instance, Newtonsoft.Json.JsonSerializer.Create(JsonHelper.Settings));

            if (json != null)
            {
                var token = json.SelectToken(path);

                if (token != null)
                {
                    result = token.ToObject<T>();
                    return true;
                }
            }
        }
        catch 
        {
            return false;
        }

        return false;
    }
    /// <summary>
    /// Gets the property from a path.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="path">The path.</param>
    /// <returns>System.Object.</returns>
    public static object GetProperty(this object obj, string path)
    {
        // set the obj to a new variable
        object o = obj;

        // split the path into separate property names
        var propNames = path.Split('.');

        if (obj is JToken)
        {
            propNames = propNames.Select(ToCamelCase).ToArray();
        }

        // enumerate the list or property names
        foreach (var propName in propNames)
        {
            // get the property value of the current object
            // and set the current object to the new value
            // to get the next property value

            if (o is JToken jObj)
            {
                o = jObj[propName];
            }
            else
            {
                o = o?.GetType().GetProperty(propName).GetValue(o);
            }
        }

        if (o is JValue jVal)
        {
            return jVal.Value;
        }

        // return the final property value
        return o;
    }

    public static void CopyPropertiesTo<T>(this T source, T dest, List<string> ignoredProperties = null)
    {
        HashSet<string> ignore = new HashSet<string>(ignoredProperties ?? Enumerable.Empty<String>(), StringComparer.OrdinalIgnoreCase);
        List<PropertyInfo> sourceProps = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(x => x.CanRead && !ignore.Contains(x.Name)).ToList();

        foreach (PropertyInfo destProp in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(x => x.CanWrite))
        {
            PropertyInfo sourceProp = sourceProps.FirstOrDefault(x => x.Name == destProp.Name);
            if (sourceProp != null && destProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
            {
                destProp.SetValue(dest, sourceProp.GetValue(source));
            }
        }
    }

    public static IEnumerable<object> GetPropertyValues(this object obj, string path)
    {
        // if the object is null, return an empty list
        if (obj == null) return new List<object>();

        // create a list to hold the current set of objects/values
        List<object> objs = new List<object> { obj };

        // split the path into separate property names
        var propNames = path.Split('.');

        if (obj is JToken)
        {
            propNames = propNames.Select(ToCamelCase).ToArray();
        }

        // enumerate the list or property names
        foreach (var propName in propNames)
        {
            // create a list for the new values
            List<object> newObjects = new List<object>();

            // enumerate through the list of current objects/values
            foreach (var o in objs)
            {
                // get the property value of the current object
                var propValue = o?.GetType().GetProperty(propName)?.GetValue(o);

                // skip to the next item in the list if the value is null
                if (propValue == null) continue;

                // check if the property value is a list of values
                if (propValue is IEnumerable<object> list)
                {
                    // add all the objects/values to the new list
                    newObjects.AddRange(list);
                }
                else
                {
                    // add the object/value to the new list
                    newObjects.Add(propValue);
                }
            }

            // set the new list as the current list
            objs = newObjects;
        }

        // return the list of objects/values
        return objs;
    }

    private static string ToCamelCase(string propName)
    {
        return $"{propName.Substring(0, 1).ToLower()}{propName.Substring(1)}";
    }

    public static bool TryCast<T>(this object value, out T result)
    {
        bool retVal = false;
        if (value is T cast)
        {
            result = cast;
            retVal = true;
        }
        else
        {
            result = default;
        }
        return retVal;
    }

    private static readonly MethodInfo CloneMethod = typeof(Object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

    public static bool IsPrimitive(this Type type)
    {
        if (type == typeof(String)) return true;
        return (type.IsValueType & type.IsPrimitive);
    }

    //public static Object Copy(this Object originalObject)
    //{
    //    return InternalCopy(originalObject, new Dictionary<Object, Object>(new ReferenceEqualityComparer()));
    //}
    //private static Object InternalCopy(Object originalObject, IDictionary<Object, Object> visited)
    //{
    //    if (originalObject == null) return null;
    //    var typeToReflect = originalObject.GetType();
    //    if (IsPrimitive(typeToReflect)) return originalObject;
    //    if (visited.ContainsKey(originalObject)) return visited[originalObject];
    //    if (typeof(Delegate).IsAssignableFrom(typeToReflect)) return null;
    //    var cloneObject = CloneMethod.Invoke(originalObject, null);
    //    if (typeToReflect.IsArray)
    //    {
    //        var arrayType = typeToReflect.GetElementType();
    //        if (IsPrimitive(arrayType) == false)
    //        {
    //            Array clonedArray = (Array)cloneObject;
    //            clonedArray.ForEach((array, indices) => array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
    //        }

    //    }
    //    visited.Add(originalObject, cloneObject);
    //    CopyFields(originalObject, visited, cloneObject, typeToReflect);
    //    RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
    //    return cloneObject;
    //}

    //private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
    //{
    //    if (typeToReflect.BaseType != null)
    //    {
    //        RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
    //        CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
    //    }
    //}

    //private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
    //{
    //    foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
    //    {
    //        if (filter != null && filter(fieldInfo) == false) continue;
    //        if (IsPrimitive(fieldInfo.FieldType)) continue;
    //        var originalFieldValue = fieldInfo.GetValue(originalObject);
    //        var clonedFieldValue = InternalCopy(originalFieldValue, visited);
    //        fieldInfo.SetValue(cloneObject, clonedFieldValue);
    //    }
    //}
    //https://github.com/Burtsev-Alexey/net-object-deep-copy
    public static T Copy<T>(this T original)
    {
        return (T)Copy((Object)original);
    }

    public class ReferenceEqualityComparer : EqualityComparer<Object>
    {
        public override bool Equals(object x, object y)
        {
            return ReferenceEquals(x, y);
        }
        public override int GetHashCode(object obj)
        {
            if (obj == null) return 0;
            return obj.GetHashCode();
        }
    }

    private static void SetValue<T>(T value, dynamic instance, PropertyInfo prop, JToken jObject)
    {
        if (jObject.TryGetValue(prop.Name, StringComparison.OrdinalIgnoreCase, out T newValue))
        {
            if (!value.Equals(newValue))
            {
                prop.SetValue(instance, newValue);
            }
        }
    }

    public static IEnumerable<PropertyInfo> GetPropertyInfos<T>(this T _) => typeof(T).GetProperties();

    public static dynamic GetPropertyValue<T>(this T instance, string name) => instance.GetProperty(name);

    //public static void SetDifferences<T>(this T instance, JObject data, string[] excludedProperties, bool includeCollections)
    //{
    //    SetDifferencesForUpdate(instance, data, excludedProperties, includeCollections, null, out Dictionary<Type, List<JToken>> values);
    //}
    ///// <summary>
    ///// Sets the property value differences from a JObject to an IAuditable data object
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="instance">T</param>
    ///// <param name="data">JObject</param>
    //public static void SetDifferencesForUpdate<T>(this T instance, JObject data, string[] excludedProperties, bool includeCollections, Dictionary<Type, List<JToken>> seed, out Dictionary<Type, List<JToken>> additionalObjects)
    //{
    //    additionalObjects = seed;

    //    var properties = instance.GetPropertyInfos().Where(x =>
    //    !excludedProperties.Any(y => y.Equals(x.Name, StringComparison.OrdinalIgnoreCase)) &&
    //    x.SetMethod != null)
    //    .ToList();

    //    foreach (var prop in properties)
    //    {
    //        if (prop.PropertyType.IsDtModel())
    //        {
    //            dynamic value = properties[properties.IndexOf(prop)].GetValue(instance);

    //            if (value != null)
    //            {
    //                var subObject = data.GetValue(prop.Name, StringComparison.OrdinalIgnoreCase);
    //                if (subObject != null)
    //                {
    //                    SetDifferencesForUpdate(value, (JObject)subObject, excludedProperties, includeCollections, seed, out additionalObjects);
    //                }
    //            }
    //        }
    //        else if (prop.PropertyType.IsDatabaseType())
    //        {
    //            dynamic value = properties[properties.IndexOf(prop)].GetValue(instance);

    //            if (value != null)
    //            {
    //                SetValue(value, instance, prop, data);
    //            }
    //        }
    //        else if (prop.PropertyType.IsCollection())
    //        {
    //            if (includeCollections)
    //            {
    //                IEnumerable<dynamic> collection = (IEnumerable<dynamic>)properties[properties.IndexOf(prop)].GetValue(instance);

    //                var items = collection.ToList();

    //                if (data.TryGetValue(prop.Name, StringComparison.OrdinalIgnoreCase, out JArray jArray))
    //                {
    //                    if (items != null && items.Any())
    //                    {
    //                        string idName = $"{items.First().GetType().Name}Id";
    //                        List<int> existingIds = items.Select(x => x[idName]).Cast<int>().ToList();

    //                        foreach (dynamic item in items)
    //                        {
    //                            dynamic id = GetPropertyValue(item, idName);

    //                            if (id != null)
    //                            {
    //                                var jToken = jArray.FirstOrDefault(x => x[idName] == id);

    //                                if (jToken != null)
    //                                {
    //                                    SetDifferencesForUpdate(item, (JObject)jToken, excludedProperties, true, seed, out additionalObjects);
    //                                }
    //                            }
    //                        }

    //                        // this should never be hit be exectued if the additional objects we recieved 
    //                        // on the first run for the queried object
    //                        if (jArray.Count > items.Count())
    //                        {
    //                            Type castType = items.First().GetType();
    //                            var list = jArray.Where(x => !(x as JObject).ContainsKey(idName)).ToList();
    //                            additionalObjects.Add(castType, list);
    //                        }
    //                    }
    //                }
    //            }
    //        }
       // }
    //}
}