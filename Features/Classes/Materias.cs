using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatriculaApi.Features.Classes
{
    public class Materias
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string NombreMateria { get; set; }
        public string Tipo { get; set; }
        public int Categoria { get; set; }
    }
}
