using System;
using System.Data.SqlClient;
using System.Net.Http;
using System.Runtime;
using Microsoft.AspNetCore.Mvc;
using Tutorial5.Models;
using Tutorial_5and6.Requests;
using Tutorial_5and6.Services;

namespace Tutorial_5and6.Controllers
{
    
    [ApiController]
    [Route("api/enrollment")]
    public class EnrollmentController : ControllerBase
    {
        private IStudentsDBService _service;
        public EnrollmentController(IStudentsDBService service)
        {
            _service = service;
        }
        
        
        private string connstring =
            "Data Source=10.1.1.36,1433;Initial Catalog=s18588;User ID=apbds18588;Password=admin";

        
        
        public IActionResult addStudent(EnrollStudentRequest request)
        {
            // var response = new EnrollStudentResponse();
            // var cl = new HttpClient();
            // cl.BaseAddress = new Uri("http://localhost:5001/api/enrollment");
            //
            // using (var c = new SqlConnection(connstring))
            // using (var com = new SqlCommand())
            // {
            //     com.Connection = c;
            //     c.Open();
            //
            //     var transaction = c.BeginTransaction();
            //     com.Transaction = transaction;
            //     
            //
            //     var IdStudies = Methods.CheckIfStudiesExist(request.Studies);
            //
            //     if (IdStudies == -1 )
            //     {
            //         transaction.Rollback();
            //         return BadRequest("Wrong studies name!");
            //     }
            //     
            //     com.CommandText = "Select max(IdEnrollment) as IdEnrollment from Enrollment";
            //     var dr = com.ExecuteReader();
            //
            //     int IdEnrollment = 0;
            //     if (dr.Read())
            //     {
            //         IdEnrollment = (int) dr[0]+1;
            //         dr.Close();
            //         DateTime StartDate = DateTime.Now;
            //         com.CommandText = "Insert into Enrollment " +
            //                           "(IdEnrollment, Semester, IdStudy, StartDate) Values" +
            //                           "(@IdEnrollment, 1, @IdStudies, @StartDate)";
            //         com.Parameters.AddWithValue("IdStudies", IdStudies);
            //         com.Parameters.AddWithValue("IdEnrollment", IdEnrollment);
            //         com.Parameters.AddWithValue("StartDate", StartDate);
            //         com.ExecuteNonQuery();
            //     }
            //     dr.Close();
            //     com.CommandText = "Select * From Student Where IndexNumber=@Id";
            //     com.Parameters.AddWithValue("Id", request.IndexNumber);
            //     dr = com.ExecuteReader();
            //     var Enrollment = IdEnrollment;
            //     com.Parameters.Clear();
            //     if (!dr.Read())
            //     {
            //         dr.Close();
            //         com.CommandText = "Insert Into Student(IndexNumber, FirstName, LastName, Birthdate, IdEnrollment) Values( @IndexNumber, @FirstName, @LastName, @Birthdate, @IdEnrollment)";
            //         com.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
            //         com.Parameters.AddWithValue("FirstName", request.FirstName);
            //         com.Parameters.AddWithValue("LastName", request.LastName);
            //         com.Parameters.AddWithValue("BirthDate", request.BirthDate);
            //         com.Parameters.AddWithValue("IdEnrollment", Enrollment);
            //         com.ExecuteNonQuery();
            //         response.Semester = "1";
            //         response.LastName = request.LastName;
            //         Console.WriteLine("Student inserted.");
            //     }
            //     else
            //     {
            //         dr.Close();
            //         transaction.Rollback();
            //         return BadRequest("Duplicated student number.");
            //     }
            //     transaction.Commit();
            // }
            _service.EnrollStudents(request);
            var response = new EnrollStudentResponse();
            return Ok( response);


        }

        [HttpPost("Promote")]
        public IActionResult promoteStudent(PromoteStudentRequest request)
        {
            _service.PromoteStudents(request);
            var response = new PromoteStudentResponse();
            return Ok( response);
        }
    }
}