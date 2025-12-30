namespace SharedScriptsApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class TypeNameAttribute : Attribute
    {

        #region fields/properties

        public string TypeName { get; set; }
        public string CollectionName { get; set; }

        #endregion

        #region .ctors

        public TypeNameAttribute(string typeName, string collectionName)
        {
            TypeName = typeName;
            CollectionName = collectionName;
        }

        public TypeNameAttribute(string typeName)
        {
            TypeName = typeName;
            CollectionName = $"{typeName}[]";
        }

        #endregion

        public static TAttribute[] GetCustomAttributes<TAttribute>(Type source, bool inherit)
           where TAttribute : Attribute
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>().ToArray();
        }
    }


}
