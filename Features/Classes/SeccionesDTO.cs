using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatriculaApi.Features.Classes
{
    public class SeccionesDTO
    {
        public string Seccion { get; set; }
        public string Materia { get; set; }
        public int Anio { get; set; }
        public string Maestro { get; set; }
        public int Capacidad { get; set; }
    }
}
