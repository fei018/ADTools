using System;
using System.Collections.Generic;
using System.Text;

namespace LoginUserManager
{
    public interface ILoginErrorAllowCount
    {
        int Id { get; set; }

        int Count { get; set; }
    }
}
