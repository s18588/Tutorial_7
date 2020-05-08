using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Tutorial_5and6.Models;


namespace Tutorial_5and6.Controllers
    {
        [ApiController]
        [Route("api/students")]
    
        public class StudentsController : ControllerBase
        {
            private string connstring =
                "Data Source=10.1.1.36,1433;Initial Catalog=s18588;User ID=apbds18588;Password=admin";
        
            [HttpGet("{id}")]
            public IActionResult getStudent(string id)
            {
            
                using (var client = new SqlConnection(connstring))
                using (var com = new SqlCommand())
                {
                    com.Connection = client;
                    com.CommandText = "use s18588; select * from Student left join Enrollment E on Student.IdEnrollment = E.IdEnrollment left join Studies S on E.IdStudy = S.IdStudy where Student.IndexNumber=@id;";
                    com.Parameters.AddWithValue("id", id);
                    client.Open();
                    var dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        var st = new Student();
                        st.FirstName = dr["FirstName"].ToString();
                        st.LastName = dr["LastName"].ToString();
                        st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
                        st.Semester = dr["Semester"].ToString();
                        st.Studies = dr["Name"].ToString();
                        return Ok(st);
                    }

                    return NotFound("Not found");

                }

            }
        

            [HttpPost]
            public IActionResult CreateStudent()
            {
                var s = new Student();
                // s.IndexNumber = $"s{new Random().Next(1, 2000)}";
                s.IdStudent = 1;
                s.FirstName = "A";
                s.LastName = "B";
                return Ok(s);
            }

            [HttpPut]
            public IActionResult putStudent()
            {
                var s = new Student();
                // s.IndexNumber = $"s{new Random().Next(1, 2000)}";
                s.IdStudent = 1;
                s.FirstName = "A";
                s.LastName = "B";
                return Ok(s);
            }

            [HttpDelete]
            public IActionResult removeStudent(int id)
            {
            
                return Ok(id);
            }
        }
    }
