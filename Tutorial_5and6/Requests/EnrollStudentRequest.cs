using System.ComponentModel.DataAnnotations;

namespace Tutorial_5and6.Requests
{
    public class EnrollStudentRequest
    {

        [Required]
        public string IndexNumber { get; set; }

        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string BirthDate { get; set; }
        [Required] public string Studies { get; set; }
    }
}