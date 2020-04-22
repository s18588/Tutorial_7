using System;
using System.Data.SqlClient;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Tutorial_5and6.Controllers;
using Tutorial_5and6.Requests;

namespace Tutorial_5and6.Services
{
    
    public interface IStudentsDBService
    {
        public void EnrollStudents(EnrollStudentRequest request)
        {
        }
        public void PromoteStudents(PromoteStudentRequest request);
        public bool CheckIndex(string index);
    }
}