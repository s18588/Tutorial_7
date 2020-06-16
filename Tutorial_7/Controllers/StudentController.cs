using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens;
using Tutorial_5and6.Handlers;
using Tutorial_5and6.Models;
using Tutorial_5and6.Requests;


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
            public IActionResult Login(LoginRequest req)
            {
                var hash = new Hashish();
                var i = req.Username;
                int loginId = 0;;
                using (var client = new SqlConnection(connstring))
                using (var com = new SqlCommand())
                {
                    com.Connection = client;
                    com.CommandText = "Select password from Student Where IndexNumber = @Username";
                    com.Parameters.AddWithValue("IndexNumber", i);
                    
                    client.Open();
                    var res = com.ExecuteReader();
                    if (res.Read())
                    {
                        loginId = int.Parse(res["Password"].ToString());
                        var pass = res["Password"];
                        var salt = res["PasswordSalt"].ToString();
                        
                    }

                    Claim[] claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, loginId.ToString()),
                        new Claim(ClaimTypes.Name, i),
                        new Claim(ClaimTypes.Role, "admin"),
                        new Claim(ClaimTypes.Role, "student")
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Secret Key"));
                    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    
                    var token = new JwtSecurityToken
                    (
                        issuer: "Gakko",
                        audience: "Students",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: credentials
                    );

                    LoginResponse loginResponse = new LoginResponse()
                    {
                        LoginToken = new JwtSecurityTokenHandler().WriteToken(token),
                        R_Token = Guid.NewGuid()
                    };

                    com.CommandText = "UPDATE Username SET RefreshToken = @Refresh WHERE IdLogin = @Id";
                    com.Parameters.AddWithValue("id", loginId);
                    com.Parameters.AddWithValue("Refresh", loginResponse.R_Token);
                    com.ExecuteNonQuery();

                    return Ok(loginResponse);
                    
                }
            }

            [HttpPost]
            [HttpPost("refresh-token")]
            public IActionResult refreshToken(TokenRefreshRequest requestToken)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(requestToken.R_Token,
                    new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = "Gakko",
                        ValidAudience = "Students",
                        ValidateLifetime = true,

                    }, out validatedToken);

                var jwtToken = validatedToken as JwtSecurityToken;

                //if null or algorithm doesn't match the one we used before -> return SecurityTokenEception???

                if (jwtToken == null || jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                    return BadRequest(new SecurityTokenException("Invalid token"));

                var index = principal.Identity.Name;

                //check in db if refreshToken exists
                using (var client = new SqlConnection(connstring))
                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    command.CommandText = "Select RefreshToken from UserLogin where IndexNumber = @index";
                    command.Parameters.AddWithValue("index", index);
                    client.Open();
                    var response = command.ExecuteReader();
                    if (response.Read())
                    {
                        var oldToken = response["RefreshToken"];
                        if (!oldToken.Equals(requestToken.R_Token))
                            return BadRequest("invalid refresh token");
                    }

                    // Create login response with new refreshToken 
                    LoginResponse loginResponse = new LoginResponse
                    {
                        LoginToken = requestToken.A_Token,
                        R_Token = Guid.NewGuid()
                    };


                    // Add new refresh token into the database 
                    command.CommandText = "UPDATE UserLogin SET RefreshToken = @refresh WHERE IndexNumber = @index";
                    command.Parameters.AddWithValue("index", index);
                    command.Parameters.AddWithValue("refresh", loginResponse.R_Token);
                    var addRefresh = command.ExecuteNonQuery();

                    return Ok(loginResponse);
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
