using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatriculaApi.DataContext;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatriculaApi.Features.Classes
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    public class ClassesController : Controller
    {
        private Context _context;

        public ClassesController(Context context)
        {
            _context = context;
        }


        [HttpGet]
        public List<Class> GetAll()
        {
            return _context.Classes.ToList();
        }

        // POST: api/TodoItems
        [HttpPost]
        [Route("posttest")]

        public Student PostTodoItem(Student test)
        {
            _context.Students.Add(test);
             _context.SaveChangesAsync();
            return test;
         }

    }

   
}