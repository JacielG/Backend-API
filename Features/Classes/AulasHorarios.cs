using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatriculaApi.Features.Classes
{
    public class AulasHorarios
    {
        public int Id { get; set; }
        public int CodeHorarios { get; set; }
        public int NumeroAula { get; set; }
        public int Disponibilidad { get; set; }
    }
}
