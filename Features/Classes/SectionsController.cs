using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using MatriculaApi.DataContext;
using Microsoft.EntityFrameworkCore.Internal;

namespace MatriculaApi.Features.Classes
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class SectionsController : ControllerBase
    {
        private Context _context;

        public SectionsController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Secciones> GetAll()
        {
            return _context.Secciones.ToList();
        }

        [HttpPost]
        [Route("Add")]
        public List<Secciones> PostTodoItem([FromBody]List<SeccionesDTO> section)
        {
            List<Secciones> Seccion = new List<Secciones>();
            Materias materia;
            Aulas aula;
            AulasHorarios horarios;
            DiasHorario dias;
            foreach (var item in section){
                materia = _context.Materias.FirstOrDefault(x => x.Codigo.Equals(item.Materia));
                //horarios = _context.AulasHorarios.Join(_context.Aulas, a => a.Disponibilidad == item.Capacidad);
                /*IEnumerable<Aulas> query = from a in _context.Aulas
                            join ah in _context.AulasHorarios on a.NumeroAula equals ah.NumeroAula
                            where
                            a.TipoAula == materia.Tipo &&
                            ah.Disponibilidad == 0 &&
                            a.Capacidad >= item.Capacidad
                            orderby a.Capacidad ascending
                            select a;
                aula = query.FirstOrDefault();*/
                IEnumerable<AulasHorarios> query = from a in _context.Aulas
                                           join ah in _context.AulasHorarios on a.NumeroAula equals ah.NumeroAula
                                           where
                                           a.TipoAula == materia.Tipo &&
                                           ah.Disponibilidad == 0 &&
                                           a.Capacidad >= item.Capacidad
                                           orderby a.Capacidad ascending
                                           select ah;
                horarios = query.FirstOrDefault();
                IEnumerable<DiasHorario> query2 = from dh in _context.DiasHorario
                                                  join d in _context.Dias on dh.CodeDias equals d.Code
                                                  join ah in _context.AulasHorarios on dh.Id equals ah.CodeHorarios
                                                  where ah.Id == horarios.Id
                                                  select dh;
                //if()
                var xd = "lol";
            }
            _context.Secciones.AddRange(Seccion);
            _context.SaveChanges();
            return Seccion;
        }
    }
}