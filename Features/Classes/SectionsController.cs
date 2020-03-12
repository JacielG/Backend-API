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
            AulasHorarios nuevoHorario;
            DiasHorario dias;
            DiasHorario codigoNoExtendido;
            Horarios horarioCompleto;
            string diaSeccion = "";
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
                dias = query2.FirstOrDefault();

                horarioCompleto = _context.Horarios.FirstOrDefault(x => x.Codigo.Equals(dias.CodeHorarios));

                if (materia.Categoria != 2 && dias.CodeDias == 1)
                {
                    IEnumerable<DiasHorario> queryNoExtendido = from dh in _context.DiasHorario
                                                                where dh.CodeDias == 3 && dh.CodeHorarios == dias.CodeHorarios
                                                                select dh;
                    codigoNoExtendido = queryNoExtendido.FirstOrDefault();
                    IEnumerable<AulasHorarios> queryNoExtendidoHorario = from ah in _context.AulasHorarios
                                                                         where ah.CodeHorarios == codigoNoExtendido.Id &&
                                                                         ah.NumeroAula == horarios.NumeroAula
                                                                         select ah;
                    nuevoHorario = queryNoExtendidoHorario.FirstOrDefault();
                    horarios.Disponibilidad = 1;
                    nuevoHorario.Disponibilidad = 1;
                    _context.AulasHorarios.Update(horarios);
                    _context.AulasHorarios.Update(nuevoHorario);
                    diaSeccion = "1 3";
                }
                Seccion.Add(new Secciones
                {
                    Seccion = item.Seccion,
                    Materia = item.Materia,
                    NumeroAula = horarios.NumeroAula,
                    Modalidad = materia.Categoria,
                    Horarios = horarioCompleto.Inicio.ToString() + ' ' + diaSeccion,
                    Maestro = item.Maestro,
                    Anio = item.Anio
                });
                
                var xd = "lol";
            }
            
            _context.Secciones.AddRange(Seccion);
            _context.SaveChanges();
            return Seccion;
        }
    }
}