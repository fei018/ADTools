using System;
using SqlSugar;

namespace LoginUserManager.Models
{
    public class LoginUser
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] //通过特性设置主键和自增列 
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string RoleName { get; set; }

        public int LoginErrorCount { get; set; }

        public bool AccountLocked { get; set; }
    }
}
