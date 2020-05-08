using System.ComponentModel.DataAnnotations;

namespace Tutorial_5and6.Requests
{
    public class PromoteStudentRequest
    {

        [Required] public int Semester { get; set; }
        [Required] public string Studies { get; set; }
    }
}