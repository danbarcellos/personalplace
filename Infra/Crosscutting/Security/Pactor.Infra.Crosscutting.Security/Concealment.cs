namespace Pactor.Infra.Crosscutting.Security
{
    public static class Concealment
    {
        private static IEncryptionService _encryptionService;
        private static IShuffleService _suShuffleService;

        public static IEncryptionService Encryption => _encryptionService ?? (_encryptionService = new EncryptionServiceRijndael());
        public static IShuffleService Shuffle => _suShuffleService ?? (_suShuffleService = new ShuffleService());
    }
}