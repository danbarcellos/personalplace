namespace PersonalPlace.Domain.Common.RBAC
{
    public class RBACInfo
    {
        public RBACInfo(string nome, string descricao, string chave)
        {
            Nome = nome;
            Descricao = descricao;
            Chave = chave;
        }

        public string Nome { get; internal set; }

        public string Descricao { get; internal set; }

        public string Chave { get; internal set; }
    }
}