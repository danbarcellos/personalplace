using System.Collections.Generic;
using System.Linq;

namespace PersonalPlace.Domain.Common.RBAC
{
    public sealed class Roles
    {
        private static Dictionary<string, string[]> _assignors = new Dictionary<string, string[]>();

        static Roles()
        {
            LoadAssignors();
        }
        
        public const string User = "{A0115065-7D08-43C1-897B-A9554B055B7D}";
        public const string Client = "{3CC9B6F3-C6D4-4E9E-AF4F-4AF2AC410BB2}";
        public const string Trainees = "{BEE09314-88BE-48DF-9D5D-BC59C82DA97A}";
        public const string Trainers = "{FF62276E-9E1B-4ADB-AC04-2D7368158B89}";
        //public const string Agent = "{9AADA121-D95C-47FF-BFA0-BC97F6AB9B73}";
        public const string QualityMonitor = "{BD27B2F2-1808-4A9C-B81A-FEAA8FB7CD9F}";
        public const string Supervisor = "{88EBEA5C-763D-4BE4-953F-49AC76E6AD41}";
        public const string CentreManager = "{55FE21E1-1397-4C33-9CBE-B51B47169B8C}";
        public const string OperationalManager = "{44DCC213-D3AC-4371-B751-CE4CCE09B983}";
        public const string HumanResources = "{C36007CD-8056-45DD-AC31-6EE6BB7F972A}";
        public const string Administrator = "{BA447994-21F9-4029-94E0-A8AC90AB216D}";
        public const string Technical = "{D3AA9AA9-7371-4C9D-9B22-DF3BAC715D21}";
        public const string TechnicalManager = "{EEFE8C6F-557D-4339-B3D0-B6EEA6B08053}";
        // reserva {CDCCCA1A-8162-4A51-B849-E75DA77C38BA}
        // reserva {ACCA2413-BE63-4AAC-BCCB-A5BC00D0BD90}

        public static RBACInfo GetRBACInfo(string key)
        {
            return RBACInformationHelper.GetRBACInfo(typeof(Roles), key);
        }

        public static IEnumerable<RBACInfo> GetRBACInfo()
        {
            return RBACInformationHelper.GetRBACInfo(typeof(Roles));
        }

        public static string[] GetAssignors(string key)
        {
            if (!_assignors.ContainsKey(key))
            {
                throw new System.Exception("Invalid role");
            }

            return _assignors[key];
        }

        public static string[] GetAssignables(string key)
        {
            return _assignors.Where(x => x.Value.Contains(key)).Select(x => x.Key).ToArray();
        }

        public static string[] GetAssignables(IEnumerable<string> keys)
        {
            var assignables = new List<string>();
            foreach (var key in keys)
            {
                assignables.AddRange(GetAssignables(key));
            }

            return assignables.Distinct().ToArray();
        }

        private static void LoadAssignors()
        {
            _assignors.Add(User, new[]
            {
                TechnicalManager,
                Technical
            });
            _assignors.Add(Trainees, new[]
            {
                TechnicalManager,
                Technical,
                Supervisor,
                CentreManager,
                OperationalManager,
                Administrator
            });
            _assignors.Add(Trainers, new[]
            {
                TechnicalManager,
                Technical,
                Supervisor,
                CentreManager,
                OperationalManager,
                Administrator
            });
            //_assignors.Add(Agent, new[]
            //{
            //    TechnicalManager,
            //    Technical,
            //    Supervisor,
            //    CentreManager,
            //    OperationalManager,
            //    Administrator
            //});
            _assignors.Add(QualityMonitor, new[]
            {
                TechnicalManager,
                Technical,
                CentreManager,
                OperationalManager,
                Administrator
            });
            _assignors.Add(Supervisor, new[]
            {
                TechnicalManager,
                CentreManager,
                OperationalManager,
                Administrator
            });
            _assignors.Add(CentreManager, new[]
            {
                TechnicalManager,
                OperationalManager,
                Administrator
            });
            _assignors.Add(OperationalManager, new[]
            {
                TechnicalManager,
                Administrator
            });
            _assignors.Add(HumanResources, new[]
            {
                TechnicalManager,
                Administrator
            });
            _assignors.Add(Administrator, new[]
            {
                TechnicalManager,
                Administrator
            });
            _assignors.Add(Technical, new[]
            {
                TechnicalManager,
                Administrator
            });
            _assignors.Add(TechnicalManager, new[]
            {
                TechnicalManager
            });
        }
    }
}