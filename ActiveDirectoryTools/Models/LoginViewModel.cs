using ADServiceLibCore.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActiveDirectoryTools.Models
{
    public class LoginViewModel
    {
        public async Task<List<DomainInfo>> GetDomainInfoList(IConfiguration configuration)
        {
            List<DomainInfo> domainList = new List<DomainInfo>();
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    var section = configuration.GetSection("DomainInfo").GetChildren();
                    foreach (var s in section)
                    {
                        var info = new DomainInfo();
                        info.DomainName = s.GetSection("DomainName").Value;
                        info.ContainerString = s.GetSection("ContainerString").Value;
                        domainList.Add(info);
                    }
                });
                return domainList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DomainInfo> GetSelectedDomainInfo(string domainName, IConfiguration configuration)
        {
            var list = await GetDomainInfoList(configuration);
            var info = list.First(d => d.DomainName == domainName);
            return info;
        }
    }
}
