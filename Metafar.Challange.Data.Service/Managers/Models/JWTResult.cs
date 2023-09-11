using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metafar.Challange.Data.Service.Managers.Models
{
    public class JWTResult
    {
        public string Token
        {
            get;
            set;
        }

        public string RefreshToken
        {
            get;
            set;
        }

        public DateTime ExpirationTime
        {
            get;
            set;
        }
    }
}
