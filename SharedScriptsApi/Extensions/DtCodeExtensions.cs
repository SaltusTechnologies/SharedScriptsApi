using SharedScriptsApi.Enums;
using System.ComponentModel;
using System.Reflection;

namespace SharedScriptsApi.Extensions
{
    public static class DtCodeExtensions
    {
        public static string GetDescription(this DtCode code)
        {
            return code.GetType()?.GetMember(code.ToString()).FirstOrDefault().GetCustomAttribute<DescriptionAttribute>()?.Description!;
        }

        public static string GetDescription(int enumInteger)
        {
            return GetDescription((DtCode)enumInteger);
        }
    }
}
