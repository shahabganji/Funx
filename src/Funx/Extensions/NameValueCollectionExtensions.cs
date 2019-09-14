using System.Collections.Specialized;

namespace Funx.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static Option<string> Lookup(this NameValueCollection @this, string key)
            => @this[key];
    }
}