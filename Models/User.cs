using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;   // Add this for [NotMapped]

namespace wedding_planner.Models
    {
        public class User
        {
            [Key]
            [Column("id")]            
            public int UserId {get;set;}

            [Column("first_name")]
            [Required]
            [MinLength(2)]
            public string FirstName {get;set;}

            [Column("last_name")]
            [Required]
            [MinLength(2)]
            public string LastName {get;set;}

            [Column("email")]
            [Required]
            [EmailAddress]
            public string Email {get;set;}

            [Column("password")]
            [Required]
            [MinLength(8)]
            [DataType(DataType.Password)]
            public string Password {get;set;}

            [Column("created_at")]
            public DateTime CreatedAt {get;set;}

            [Column("updated_at")]
            public DateTime UpdatedAt {get;set;}

            [NotMapped]
            [Required]
            [Compare("Password")]
            [DataType(DataType.Password)]
            public string Confirm {get;set;}

            // Navigation Properties
            public List<Rsvp> AllWeddings {get;set;}
        }
    }