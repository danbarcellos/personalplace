using System;
using System.Globalization;

namespace PersonalPlace.Domain.Base
{
    public static class DomainContextConstants
    {
        public static Guid DefaultId = new Guid("{A4008BDA-989B-45CC-8B88-5C94463D6054}");
        public const string DefaultLogin = "alaris";
        public const string DefaultName = "Alaris Contact System";
        public const string DefaultToken = "";
        public const string RootScopeTag = "1.";
        public const string DefaultScopeTag = RootScopeTag;
        public const string EmptyScopeTag = "..";
        public static readonly string DefaultCulture;

        static DomainContextConstants()
        {
            DefaultCulture = CultureInfo.InstalledUICulture.Name;
        }
    }
}