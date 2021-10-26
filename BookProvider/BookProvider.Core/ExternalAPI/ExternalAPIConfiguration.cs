using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookProvider.Core.ExternalAPI
{
    public class ExternalAPIConfiguration
    {
        public string ExternalAPIAddress { get; set; }
        public string GetUrlAddress(int number)
        {
            return $"{ExternalAPIAddress}/api/v1/books?numberOfBooks={number}";
        }
    }
}
