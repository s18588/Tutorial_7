using System;
using System.Data.SqlClient;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Tutorial5.Models;

namespace Tutorial_5.Controllers
{
    
    [ApiController]
    [Route("api/enrollment")]
    public class EnrollmentController
    {
        private string connstring =
            "Data Source=10.1.1.36,1433;Initial Catalog=s18588;User ID=apbds18588;Password=admin";
        
        [HttpPost]
        public IActionResult addStudent(Student s)
        {
            var cl = new HttpClient();
            cl.BaseAddress = new Uri("http://localhost:5001/api/enrollment");
            SqlConnection c = new SqlConnection();
            using (var client = new SqlConnection(connstring))
            using (var com = new SqlCommand())
            {
               var insert = cl.PostAsJson
                }

                return NotFound("Not found");

            }
        }
    }
}