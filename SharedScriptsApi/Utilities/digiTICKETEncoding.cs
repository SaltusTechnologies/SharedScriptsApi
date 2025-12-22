using System.Text;

namespace SharedScriptsApi.Utilities
{
    public static class digiTICKETEncoding
    {

        private static Encoding _Utf8NoBomEncoding = new UTF8Encoding(false);
        private static Encoding _Default = Utf8NoBomEncoding;
        public static Encoding Default { get { return _Default; } }
        public static Encoding Utf8NoBomEncoding { get { return _Utf8NoBomEncoding; } }

    }
}
