using SqlSugar;

namespace ADServiceLibCore.Models
{
    /// <summary>
    /// domainName (e.g. "hiphing.nws")<br/>
    /// containerString (e.g. "DC=hiphing,DC=nws")
    /// </summary>
    /// <param name="domainName">e.g. "hiphing.nws"</param>
    /// <param name="containerString">e.g. "DC=hiphing,DC=nws"</param>
    /// <param name="adminName"></param>
    /// <param name="AdminPassword"></param>
    [SugarTable("DomainInfo")]
    public class DomainInfo
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string DomainName { get; set; }

        public string ContainerString { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string AdminName { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string AdminPassword { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string ConnectedServer { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string DomainStaticKey => DomainName + ":" + AdminName;
    }
}
