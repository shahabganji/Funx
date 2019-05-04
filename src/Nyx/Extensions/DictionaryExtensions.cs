using System.Collections.Concurrent;
using System.Collections.Generic;
using static Nyx.Helpers.OptionHelpers;


namespace Nyx.Extensions
{
    public static class DictionaryExtensions
    {
        public static Option<TValue> Lookup<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key)
            => @this.TryGetValue(key, out var value) ? Some(value) : (Option<TValue>) None;


        public static Option<TValue> Lookup<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> @this, TKey key)
            => @this.TryGetValue(key, out var value) ? Some(value) : (Option<TValue>) None;
    }
}