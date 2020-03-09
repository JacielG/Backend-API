using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatriculaApi.Features.Classes
{
    public class Secciones
    {
        public int Id { get; set; }
        public string Seccion { get; set; }
        public string Materia { get; set; }
        public int NumeroAula { get; set; }
        public string Modalidad { get; set; }
        public int Horarios { get; set; }
        public int Anio { get; set; }
        public string Maestro { get; set; }
    }
}
