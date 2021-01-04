using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

namespace LoginUserManager.Models
{
    public class LoginErrorCountLimit
    {
        [SugarColumn(IsPrimaryKey =true,IsIdentity =true)]
        public int Id { get; set; }

        public int Count { get; set; }
    }
}
