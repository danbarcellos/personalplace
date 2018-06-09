using System.IO;

namespace Pactor.Infra.Crosscutting.Security
{
    public interface IShuffleService
    {
        string ComputeHash(string texto);
        string ComputeHash(Stream stream);
        string ComputeRandomPassword();
    }
}