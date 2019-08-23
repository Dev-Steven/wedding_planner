
using System.ComponentModel.DataAnnotations;

namespace wedding_planner.Models
{
    public class LogInUser
    {
        // No other fields!
        public string LoginEmail {get;set;}

        [DataType(DataType.Password)]
        public string LoginPassword {get;set;}
    }
}