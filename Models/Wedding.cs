using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;   // Add this for [NotMapped]

namespace wedding_planner.Models
{
    public class Wedding
    {
        [Key]
        [Column("id")]
        public int WeddingId {get;set;}

        [Column("user_id")]
        public int CreatorId {get;set;}

        [Column("wedder_one")]
        [Required]
        public string WedderOne {get;set;}

        [Column("wedder_two")]
        [Required]
        public string WedderTwo {get;set;}

        [Column("date")]
        [Required]
        [DataType(DataType.Date)]
        [ValidateDate]
        public String Date {get;set;}

        [Column("address")]
        [Required]
        public string Address {get;set;}

        // Naviation Properties
        public User Creator {get;set;}
        public List<Rsvp> AllGuests {get;set;}
    }

    public class ValidateDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime wedDate = Convert.ToDateTime(value);
            DateTime currDate = DateTime.Now;
            if(wedDate < currDate)
            {
                return new ValidationResult("Not a Valid Date");
            }
            return ValidationResult.Success;
        }
    }
}