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
            AulasHorarios horarios;
            AulasHorarios nuevoHorario;
            AulasHorarios nuevoHorarioExtendido;
            List <AulasHorarios> listaHorarioSabado;
            DiasHorario dias;
            DiasHorario codigoNoExtendido;
            DiasHorario codigoDiasExtendido;
            Horarios horarioCompleto;
            string diaSeccion = "";
            int asignado = 0;
            foreach (var item in section)
            {
                materia = _context.Materias.FirstOrDefault(x => x.Codigo.Equals(item.Materia));
                if(materia != null)
                {
                    IEnumerable<AulasHorarios> query = from a in _context.Aulas
                                                       join ah in _context.AulasHorarios on a.NumeroAula equals ah.NumeroAula
                                                       where
                                                       a.TipoAula == materia.Tipo &&
                                                       ah.Disponibilidad == 0 &&
                                                       a.Capacidad >= item.Capacidad
                                                       orderby a.Capacidad ascending
                                                       select ah;
                    horarios = query.FirstOrDefault();
                    if (horarios == null)
                    {
                        NoAsignada(item, materia);
                    }
                    else
                    {
                        IEnumerable<DiasHorario> query2 = from dh in _context.DiasHorario
                                                          join d in _context.Dias on dh.CodeDias equals d.Code
                                                          join ah in _context.AulasHorarios on dh.Id equals ah.CodeHorarios
                                                          where ah.Id == horarios.Id
                                                          orderby dh.CodeDias ascending
                                                          select dh;
                        dias = query2.FirstOrDefault();
                        //dias.CodeDias = 5;
                        horarioCompleto = _context.Horarios.FirstOrDefault(x => x.Codigo.Equals(dias.CodeHorarios));

                        //No Extendida Lunes y Miercoles
                        if (materia.Categoria != 2 && dias.CodeDias == 1)
                        {
                            IEnumerable<DiasHorario> queryDiasFiltrado = from dh in _context.DiasHorario
                                                                         where dh.CodeDias == 3 && dh.CodeHorarios == dias.CodeHorarios
                                                                         select dh;
                            codigoNoExtendido = queryDiasFiltrado.FirstOrDefault();
                            IEnumerable<AulasHorarios> queryAulasFiltrado = from ah in _context.AulasHorarios
                                                                            where ah.CodeHorarios == codigoNoExtendido.Id &&
                                                                            ah.NumeroAula == horarios.NumeroAula
                                                                            select ah;
                            nuevoHorario = queryAulasFiltrado.FirstOrDefault();
                            horarios.Disponibilidad = 1;
                            nuevoHorario.Disponibilidad = 1;
                            _context.AulasHorarios.Update(horarios);
                            _context.AulasHorarios.Update(nuevoHorario);
                            diaSeccion = "1 3";
                            asignado = 1;
                        }
                        //No extendida Martes y Jueves
                        if (materia.Categoria != 2 && dias.CodeDias == 2)
                        {
                            IEnumerable<DiasHorario> queryDiasFiltrado = from dh in _context.DiasHorario
                                                                         where dh.CodeDias == 4 && dh.CodeHorarios == dias.CodeHorarios
                                                                         select dh;
                            codigoNoExtendido = queryDiasFiltrado.FirstOrDefault();
                            IEnumerable<AulasHorarios> queryAulasFiltrado = from ah in _context.AulasHorarios
                                                                            where ah.CodeHorarios == codigoNoExtendido.Id &&
                                                                            ah.NumeroAula == horarios.NumeroAula
                                                                            select ah;
                            nuevoHorario = queryAulasFiltrado.FirstOrDefault();
                            horarios.Disponibilidad = 1;
                            nuevoHorario.Disponibilidad = 1;
                            _context.AulasHorarios.Update(horarios);
                            _context.AulasHorarios.Update(nuevoHorario);
                            diaSeccion = "2 4";
                            asignado = 1;
                        }
                        //Extendidas Lunes, Miercoles y Viernes
                        if (materia.Categoria == 2 && dias.CodeDias == 1)
                        {
                            IEnumerable<DiasHorario> queryNoExtendido = from dh in _context.DiasHorario
                                                                        where dh.CodeDias == 3 || dh.CodeDias == 5 && dh.CodeHorarios == dias.CodeHorarios
                                                                        select dh;
                            codigoNoExtendido = queryNoExtendido.FirstOrDefault(x => x.CodeDias == 3);
                            codigoDiasExtendido = queryNoExtendido.FirstOrDefault(x => x.CodeDias == 5);
                            IEnumerable<AulasHorarios> queryNoExtendidoHorario = from ah in _context.AulasHorarios
                                                                                 where ah.CodeHorarios == codigoNoExtendido.Id || ah.CodeHorarios == codigoDiasExtendido.Id &&
                                                                                 ah.NumeroAula == horarios.NumeroAula
                                                                                 select ah;
                            nuevoHorario = queryNoExtendidoHorario.FirstOrDefault(x => x.CodeHorarios == codigoNoExtendido.Id);
                            nuevoHorarioExtendido = queryNoExtendidoHorario.FirstOrDefault(x => x.CodeHorarios == codigoDiasExtendido.Id);
                            horarios.Disponibilidad = 1;
                            nuevoHorario.Disponibilidad = 1;
                            nuevoHorarioExtendido.Disponibilidad = 1;
                            _context.AulasHorarios.Update(horarios);
                            _context.AulasHorarios.Update(nuevoHorario);
                            _context.AulasHorarios.Update(nuevoHorarioExtendido);
                            diaSeccion = "1 3 5";
                            asignado = 1;
                        }
                        //Viernes
                        if (materia.Categoria != 2 && dias.CodeDias == 5)
                        {
                            for (var i = 1; i <= 4; i++)
                            {
                                IEnumerable<AulasHorarios> queryDisponibles = from ah in _context.AulasHorarios
                                                                              join dh in _context.DiasHorario on ah.CodeHorarios equals dh.Id
                                                                              where ah.Disponibilidad == 0 && ah.Sede.Equals(item.Sede)
                                                                              && dh.CodeDias == dias.CodeDias && ah.NumeroAula == horarios.NumeroAula
                                                                              && dh.Inicia == i
                                                                              orderby ah.Id ascending
                                                                              select ah;
                                if (queryDisponibles.ToList().Count() > 1)
                                {
                                    foreach (var data in queryDisponibles)
                                    {
                                        data.Disponibilidad = 1;
                                        _context.AulasHorarios.Update(data);
                                    }
                                }

                                diaSeccion = dias.CodeDias.ToString();
                                asignado = 1;
                            }

                        }
                        //Sabados No Extendido
                        if (materia.Categoria != 2 && (dias.CodeDias == 6 || dias.CodeDias == 7))
                        {
                            IEnumerable<AulasHorarios> queryDisponibles = (from ah in _context.AulasHorarios
                                                                           join dh in _context.DiasHorario on ah.CodeHorarios equals dh.Id
                                                                           where ah.Disponibilidad == 0 && ah.Sede.Equals(item.Sede)
                                                                           && dh.CodeDias == dias.CodeDias && ah.NumeroAula == horarios.NumeroAula
                                                                           orderby ah.Id ascending
                                                                           select ah).Take(2);
                            listaHorarioSabado = queryDisponibles.ToList();

                            foreach (var dato in listaHorarioSabado)
                            {
                                dato.Disponibilidad = 1;
                                _context.AulasHorarios.Update(dato);
                            }

                            diaSeccion = dias.CodeDias.ToString();
                            asignado = 1;
                        }

                        //Sabados Domingo Extendido
                        if (materia.Categoria == 2 && (dias.CodeDias == 6 || dias.CodeDias == 7))
                        {
                            IEnumerable<AulasHorarios> queryDisponibles = (from ah in _context.AulasHorarios
                                                                           join dh in _context.DiasHorario on ah.CodeHorarios equals dh.Id
                                                                           where ah.Disponibilidad == 0 && ah.Sede.Equals(item.Sede)
                                                                           && dh.CodeDias == dias.CodeDias && ah.NumeroAula == horarios.NumeroAula
                                                                           orderby ah.Id ascending
                                                                           select ah).Take(3);
                            listaHorarioSabado = queryDisponibles.ToList();

                            foreach (var dato in listaHorarioSabado)
                            {
                                dato.Disponibilidad = 1;
                                _context.AulasHorarios.Update(dato);
                            }

                            diaSeccion = dias.CodeDias.ToString();
                            asignado = 1;
                        }
                        if (asignado == 1)
                        {
                            Seccion.Add(new Secciones
                            {
                                Seccion = item.Seccion,
                                Materia = item.Materia,
                                NumeroAula = horarios.NumeroAula,
                                Modalidad = materia.Categoria,
                                Horarios = horarioCompleto.Inicio.ToString() + ' ' + diaSeccion,
                                Maestro = item.Maestro,
                                Anio = item.Anio,
                                Asignada = 1
                            });
                        }
                        else if (asignado == 0)
                        {
                            Seccion.Add(new Secciones
                            {
                                Seccion = item.Seccion,
                                Materia = item.Materia,
                                NumeroAula = horarios.NumeroAula,
                                Modalidad = materia.Categoria,
                                Horarios = horarioCompleto.Inicio.ToString() + ' ' + diaSeccion,
                                Maestro = item.Maestro,
                                Anio = item.Anio,
                                Asignada = 0
                            });
                        }
                        _context.Secciones.AddRange(Seccion);
                        _context.SaveChanges();
                        Seccion.Clear();
                    }
                }
            }
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
                
            return Seccion;
        }

        private void NoAsignada(SeccionesDTO section, Materias materia)
        {
            Secciones Seccion = new Secciones();
            Seccion = (new Secciones
            {
                Seccion = section.Seccion,
                Materia = section.Materia,
                NumeroAula = null,
                Modalidad = materia.Categoria,
                Horarios = null,
                Maestro = section.Maestro,
                Anio = section.Anio,
                Asignada = 0
            });

            _context.Secciones.AddRange(Seccion);
            _context.SaveChanges();
        }
    }
}