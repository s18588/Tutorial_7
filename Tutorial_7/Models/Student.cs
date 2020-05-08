
using DateTime = System.DateTime;

namespace Tutorial_5and6.Models
{
    public class Student
    {

        public int IdStudent { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Semester { get; set; }
        public string Studies { get; set; }
    }
}