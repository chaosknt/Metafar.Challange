using Metafar.Challange.Entities.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metafar.Challange.Data.Service.Managers
{
    public class Response<T> where T : class
    {
        public T Content { get; set; }

        public bool WasSuccessfullyProcceded { get; set; }

        public string Message { get; set; }
    }
}
