using System.IO;

namespace Pactor.Infra.Crosscutting.Log.Base
{
    public class EnvironmentSetting
    {
        public const string NomePadraoSubdiretorioLog = "AlarisLog";
        public const string DatePatternThatComposeName = "yyyy.MM.dd";

        public static string GetLogFilesPath()
        {
            var logFilesPath = Path.Combine(Path.GetTempPath(), EnvironmentSetting.NomePadraoSubdiretorioLog);

            return logFilesPath;
        }
    }
}