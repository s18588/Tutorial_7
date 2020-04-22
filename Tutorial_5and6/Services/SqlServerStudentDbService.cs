using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tutorial_5and6.Controllers;
using Tutorial_5and6.Requests;
using Tutorial_5and6.Services;


namespace Services
{
    public class SqlServerStudentDbService : IStudentsDBService
    {
        public void EnrollStudent(EnrollStudentRequest request)
        {
            string connstring =
                "Data Source=10.1.1.36,1433;Initial Catalog=s18588;User ID=apbds18588;Password=admin";
            var response = new EnrollStudentResponse();
            var cl = new HttpClient();
            cl.BaseAddress = new Uri("http://localhost:5001/api/enrollment");
            
            using (var c = new SqlConnection(connstring))
            using (var com = new SqlCommand())
            {
                com.Connection = c;
                c.Open();

                var transaction = c.BeginTransaction();
                com.Transaction = transaction;
                

                var IdStudies = Methods.CheckIfStudiesExist(request.Studies);

                if (IdStudies == -1 )
                {
                    transaction.Rollback();
                    
                }
                
                com.CommandText = "Select max(IdEnrollment) as IdEnrollment from Enrollment";
                var dr = com.ExecuteReader();

                int IdEnrollment = 0;
                if (dr.Read())
                {
                    IdEnrollment = (int) dr[0]+1;
                    dr.Close();
                    DateTime StartDate = DateTime.Now;
                    com.CommandText = "Insert into Enrollment " +
                                      "(IdEnrollment, Semester, IdStudy, StartDate) Values" +
                                      "(@IdEnrollment, 1, @IdStudies, @StartDate)";
                    com.Parameters.AddWithValue("IdStudies", IdStudies);
                    com.Parameters.AddWithValue("IdEnrollment", IdEnrollment);
                    com.Parameters.AddWithValue("StartDate", StartDate);
                    com.ExecuteNonQuery();
                }
                dr.Close();
                com.CommandText = "Select * From Student Where IndexNumber=@Id";
                com.Parameters.AddWithValue("Id", request.IndexNumber);
                dr = com.ExecuteReader();
                var Enrollment = IdEnrollment;
                com.Parameters.Clear();
                if (!dr.Read())
                {
                    dr.Close();
                    com.CommandText = "Insert Into Student(IndexNumber, FirstName, LastName, Birthdate, IdEnrollment) Values( @IndexNumber, @FirstName, @LastName, @Birthdate, @IdEnrollment)";
                    com.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                    com.Parameters.AddWithValue("FirstName", request.FirstName);
                    com.Parameters.AddWithValue("LastName", request.LastName);
                    com.Parameters.AddWithValue("BirthDate", request.BirthDate);
                    com.Parameters.AddWithValue("IdEnrollment", Enrollment);
                    com.ExecuteNonQuery();
                    response.Semester = "1";
                    response.LastName = request.LastName;
                    Console.WriteLine("Student inserted.");
                }
                else
                {
                    dr.Close();
                    transaction.Rollback();
                    
                }
                transaction.Commit();
            }

        }

        public void PromoteStudents(PromoteStudentRequest request)
        {
            string connstring =
                "Data Source=10.1.1.36,1433;Initial Catalog=s18588;User ID=apbds18588;Password=admin";
            var response = new PromoteStudentResponse(); 
            var cl = new HttpClient();
            cl.BaseAddress = new Uri("http://localhost:5001/api/enrollment/promote");
            
            using (var c = new SqlConnection(connstring))
            using (var com = new SqlCommand())
            {
                com.Connection = c;
                c.Open();
            
                var transaction = c.BeginTransaction();
                com.Transaction = transaction;
                var IdStudies = Methods.CheckIfStudiesExist(request.Studies);
                if (IdStudies == -1)
                {
                    transaction.Rollback();
                    
                }
            
                com.CommandText =
                    "select * from Enrollment where IdStudy = @Studies and Semester = @Semester";
                com.Parameters.AddWithValue("Studies", IdStudies);
                com.Parameters.AddWithValue("Semester", request.Semester);
                var dr = com.ExecuteReader();
                com.Parameters.Clear();
            
                if (dr.Read())
                {
                    dr.Close();
                    com.CommandText = "update Enrollment set Semester = @SemesterNo where IdStudy = @Studies and Semester = @Semester";
                    com.Parameters.AddWithValue("Studies", IdStudies);
                    com.Parameters.AddWithValue("Semester", (int)request.Semester);
                    com.Parameters.AddWithValue("SemesterNo", request.Semester + 1);
                    com.ExecuteNonQuery();
                    response.Studies = request.Studies;
                    response.Semester = request.Semester + 1;
                    Console.WriteLine("Ok");
                }
            
            }   
        }

        bool IStudentsDBService.CheckIndex(string index)
        {
            return CheckIndex(index);
        }


        public bool CheckIndex(string index)
        {
            string connstring =
                "Data Source=10.1.1.36,1433;Initial Catalog=s18588;User ID=apbds18588;Password=admin";
            var response = new EnrollStudentResponse();
            var cl = new HttpClient();
            cl.BaseAddress = new Uri("http://localhost:5001/api/enrollment");
            
            using (var c = new SqlConnection(connstring))
            using (var com = new SqlCommand())
            {
                com.Connection = c;
                c.Open();
                com.CommandText = "select * from students where IndexNumber = @id";
                com.Parameters.AddWithValue("id", index);
                var dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
