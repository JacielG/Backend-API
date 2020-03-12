using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatriculaApi.Features.Classes
{
    public class Horarios
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public TimeSpan Inicio { get; set; }
        public TimeSpan Final { get; set; }
    }
}
