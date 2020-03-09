using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatriculaApi.Features.Classes
{
    public class Aulas
    {
        public int Id { get; set; }
        public int NumeroAula { get; set; }
        public string TipoAula { get; set; }
        public string Sede { get; set; }
        public int Capacidad { get; set; }
    }
}
