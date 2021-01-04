using ADServiceLibCore.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ADServiceLibCore.Services
{
    internal class ADDmainInfoHelper
    {
        public async Task<IADResult<DomainInfo>> SetDomainInfoToDatabase(IADServiceDb db, DomainInfo info)
        {
            var result = new ADResult<DomainInfo>();

            if (info == null || string.IsNullOrWhiteSpace(info.DomainName)
                             || string.IsNullOrWhiteSpace(info.ContainerString))
            {
                return result.ToReturn("DomainInfo Is NullOrWhiteSpace");
            }

            try
            {
                var all = await db.Tbl_DomainInfo.GetListAsync();
                if (all != null && all.Any())
                {
                    await db.Tbl_DomainInfo.AsDeleteable().Where(all).ExecuteCommandAsync();
                }

                var set = await db.Tbl_DomainInfo.InsertAsync(info);
                if (set)
                {
                    return result.ToReturn(await db.Tbl_DomainInfo.AsQueryable().FirstAsync());
                }

                return result.ToReturn("Cannot Set DomainInfo.");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return result.ToReturn(ex.InnerException.Message);
                }
                return result.ToReturn(ex.Message);
            }
        }

        public async Task<IADResult<DomainInfo>> GetDomainInfo(IADServiceDb db)
        {
            var result = new ADResult<DomainInfo>();

            try
            {
                var first = await db.Tbl_DomainInfo.AsQueryable().FirstAsync();
                if (first != null)
                {
                    return result.ToReturn(first);
                }

                return result.ToReturn("Cannot Find DomainInfo in Database.");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return result.ToReturn(ex.InnerException.Message);
                }
                return result.ToReturn(ex.Message);
            }
        }
    }
}
