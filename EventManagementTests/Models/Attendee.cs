using System.ComponentModel.DataAnnotations;
﻿using System.ComponentModel.DataAnnotations.Schema;


namespace EventManagementTests.Models
{
    public class Attendee
    {
        [Key]
        public int AttendeeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // this navigation property
        public Event Event { get; set; }


    }
}
