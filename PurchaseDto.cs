using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chairs_dotnet8_api
{
    public class PurchaseDto
    {
        public int Id { get; set; }

        public int Cantidad { get; set; }

        public int Pago { get; set; }
    }
}