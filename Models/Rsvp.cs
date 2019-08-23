using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;   // Add this for [NotMapped]

namespace wedding_planner.Models
{
    public class Rsvp
    {
        [Key]
        [Column("id")]
        public int GuestId {get;set;}

        [Column("user_id")]
        public int UserId {get;set;}

        [Column("wedding_id")]
        public int WeddingId {get;set;}

        // Navigation Properties
        public User Rsvped {get;set;}
        public Wedding Rsvps {get;set;}
    }
}

