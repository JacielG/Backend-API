using MatriculaApi.Features.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatriculaApi.DataContext
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Secciones> Secciones { get; set; }
        public DbSet<Materias> Materias { get; set; }
        public DbSet<Aulas> Aulas { get; set; }
        public DbSet<AulasHorarios> AulasHorarios { get; set; }
        public DbSet<DiasHorario> DiasHorario { get; set; }
        public DbSet<Dias> Dias { get; set; }
        public DbSet<Horarios> Horarios { get; set; }
    }
}
