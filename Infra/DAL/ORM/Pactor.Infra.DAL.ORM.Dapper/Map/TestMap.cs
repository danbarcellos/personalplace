namespace Pactor.Infra.DAL.ORM.Dapper.Map
{
    public class EuSouEntidade : Entity
    {
        public string Nome { get; set; }
    }

    public class TestMap : EntityMapper<EuSouEntidade>
    {
        public TestMap()
        {
            Map(x => x.Nome).Column("Seila");
            Map("Version");
        }

    }
}