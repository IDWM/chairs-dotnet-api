using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chairs_dotnet8_api
{
    public class Chair
    {
        public int Id { get; set; }
        public string Nombre { get; set; }  = string.Empty;
        public string Tipo { get; set; }  = string.Empty;
        public string Material { get; set; }  = string.Empty;
        public string Color { get; set; }  = string.Empty;
        public int Altura { get; set; }  
        public int Anchura { get; set; } 
        public int Profundidad { get; set; }  
        public int Precio { get; set; } 
        public int Stock { get; set; }
    }
}