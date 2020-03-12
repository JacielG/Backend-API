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
        public string NumeroAula { get; set; }
        public int Modalidad { get; set; }
        public string Horarios { get; set; }
        public int Anio { get; set; }
        public string Maestro { get; set; }
    }
}
