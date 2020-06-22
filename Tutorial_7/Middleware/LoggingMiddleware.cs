using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Tutorial_5and6.Services;

namespace Tutorial_5and6.Middleware
{
    public class LoggingMiddleware
    {

        private readonly RequestDelegate _next;
        
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IStudentsDBService service)
        {
            // logic
            if (httpContext.Request != null)
            {
                try
                {
                    string path = httpContext.Request.Path; // "/student/1"
                    string queryString = httpContext.Request?.QueryString.ToString();
                    string method = httpContext.Request.Method.ToString();
                    string bodyParameters = "";

                    using (StreamReader reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true))
                    {
                        bodyParameters = await reader.ReadToEndAsync();
                    }
                    
                    var LogWriter = new FileStream("requestLog.txt", FileMode.Create);
                    using(var writer = new StreamWriter(LogWriter))
                    {
                        string text = $"Path: {path} \n" +
                                      $"QueryString:{queryString} \n" +
                                      $"Method: {method} \n" +
                                      $"Body Parameters: {bodyParameters}";
                        writer.WriteLine(text);
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLog(e.Message);
                }
                
                
            }
            
            
            await _next(httpContext);
        }
    }
}