using ADServiceLibCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ADServiceLibCore.Services
{
    internal class ADLoginHelper
    {
        private readonly static ConcurrentDictionary<string, DomainInfo> DomainInfoStaticDb = new ConcurrentDictionary<string, DomainInfo>();

        private void AddLoginDomainInfo(DomainInfo domain)
        {
            var info = new DomainInfo
            {
                AdminName = domain.AdminName,
                AdminPassword = domain.AdminPassword,
                ContainerString = domain.ContainerString,
                DomainName = domain.DomainName,
                ConnectedServer = domain.ConnectedServer
            };

            try
            {
                var add = DomainInfoStaticDb.AddOrUpdate(domain.DomainStaticKey, domain, (key, oldvalue) => domain);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IADResult<DomainInfo> GetLoginDomainInfo(HttpContext httpContext)
        {
            var result = new ADResult<DomainInfo>();
            var identity = httpContext.User.Identity as ClaimsIdentity;
            var key = identity.FindFirst(nameof(DomainInfo.DomainStaticKey)).Value;
            var get = DomainInfoStaticDb.TryGetValue(key, out DomainInfo info);
            if (get)
            {
                return result.ToReturn(get, info);
            }
            else
            {
                return result.ToReturn(key + " try to get DomainInfo in DomainInfoStaticDb return false.");
            }
        }

        public async Task<IADResult<DomainInfo>> SignIn(DomainInfo domain, HttpContext httpContext)
        {
            var result = new ADResult<DomainInfo>();
            try
            {
                var claimIndentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                claimIndentity.AddClaim(new Claim(nameof(DomainInfo.AdminName), domain.AdminName));
                claimIndentity.AddClaim(new Claim(nameof(DomainInfo.DomainName), domain.DomainName));
                claimIndentity.AddClaim(new Claim(nameof(DomainInfo.DomainStaticKey), domain.DomainStaticKey));

                var principal = new ClaimsPrincipal(claimIndentity);

                //验证参数内容
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,

                    //是否永久保存cookie
                    IsPersistent = false
                };

                await httpContext?.SignInAsync(principal, authProperties);
                AddLoginDomainInfo(domain);
                return result.ToReturn(true,domain);
            }
            catch (Exception ex)
            {
                return result.ToReturn(ex.Message);
            }
        }
    }
}
