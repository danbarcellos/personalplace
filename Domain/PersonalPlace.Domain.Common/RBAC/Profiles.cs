using System.Collections.Generic;

namespace PersonalPlace.Domain.Common.RBAC
{
    public sealed class Profiles
    {
        public const string Usuario = "[FAC03639-AD8E-4F43-B8A3-80E5C8D09628]";
        public const string Agente = "[40750707-7538-4568-B16D-F5323CCD50C9]";
        public const string AgenteOperador = "[0DFF3146-7DB8-4870-BAC7-3429CDB25CD6]";
        public const string AgenteDeCampo = "[90C093C4-84C6-4728-9F4B-3404AD61BA43]";
        public const string SupervisorAgenteOperador = "[D08BC3C1-139A-4DD5-9405-7DF857F88D1F]";
        public const string SupervisorAgenteDeCampo = "[6686F2A2-B0A1-43DB-A661-9745FA55226C]";
        public const string CaixaAcertoAgenteDeCampo = "[FE958A9A-7502-48F5-A012-EBC70A2CA33D]";
        public const string DiretorTecnico = "[3FB85540-E3BC-4364-AB99-D6D93A129689]";
        public const string SupervisorTecnico = "[42EEBFC6-36A9-4FE7-BC90-DDB6D6CF832C]";
        public const string OperadorTecnico = "[11B41FD4-C34D-4B19-BD2C-81A78CAC0C29]";
        public const string AdministradorGeral = "[B51D1234-7AD4-46A0-A2BA-39FBB6D163A5]";
        public const string GerenteFinanceiro = "[2ADBEB5B-E791-49E6-BBDD-3795D9F4609A]";
        public const string GerenteGeral = "[F6796528-777A-4308-AA92-6D3480FF9982]";
        public const string GerenteCentral = "[1D3763FA-0863-49FE-8806-7326F48F9635]";
        public const string GerenteUnidade = "[C6235E49-94B4-4767-B5F5-4388F3E400F1]";
        public const string EmissorFaturas = "[2E99FBB3-2CE4-4043-B1F2-EBB7CD10A8C1]";
        public const string DespachanteFaturas = "[11DD63C6-F63B-44FD-82C4-3143F59603F6]";

        static public RBACInfo ObterInformacaoRBAC(string chave)
        {
            return RBACInformationHelper.GetRBACInfo(typeof(Profiles), chave);
        }

        static public IEnumerable<RBACInfo> ObterInformacoesRBAC()
        {
            return RBACInformationHelper.GetRBACInfo(typeof(Profiles));
        }
    }
}