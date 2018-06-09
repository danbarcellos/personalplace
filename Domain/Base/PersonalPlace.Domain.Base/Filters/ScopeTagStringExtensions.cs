using System.Collections.Generic;

namespace PersonalPlace.Domain.Base.Filters
{
    public static class ScopeTagStringExtensions
    {
        public static List<string> ToAscendingScopeTags(this string scopeTag)
        {
            var tags = new List<string>();

            for (var i = 0; i < scopeTag.Length; i++)
            {
                if (scopeTag[i] == '.')
                    tags.Add(scopeTag.Substring(0, i + 1));
            }
            return tags;
        }

        public static List<string> ToParenthoodAscendingScopeTags(this string scopeTag)
        {
            var tags = ToAscendingScopeTags(scopeTag);
            return tags.Count > 1 ? tags.GetRange(0, tags.Count - 1) : tags;
        }
    }
}