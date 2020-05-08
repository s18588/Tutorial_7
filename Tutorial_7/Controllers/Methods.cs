using System.Data.SqlClient;

namespace Tutorial_5and6.Controllers
{
    public class Methods
    {
        private static string connstring =
            "Data Source=10.1.1.36,1433;Initial Catalog=s18588;User ID=apbds18588;Password=admin";
        public static int CheckIfStudiesExist(string Name)
        {
            using (var c = new SqlConnection(connstring))
            using (var com = new SqlCommand())
            {
                com.Connection = c;
                com.CommandText = "Select * from studies where name = @Name";
                com.Parameters.AddWithValue("Name", Name);
                c.Open();
                var dr = com.ExecuteReader();

                if (!dr.Read())
                {
                    dr.Close();
                    return -1;
                }
                else
                {
                    
                    int IdStudies = (int) dr["IdStudy"];
                    dr.Close();
                    return IdStudies;
                }
            }
        }
    }
}