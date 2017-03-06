using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

// Entity is our Object Relational Mapper (ORM) - it knows to interpret this info, our get/set properties

namespace FakeTrello.Models
{
    public class TrelloUser
    {
        [Key]
        public int TrelloUserId { get; set; } // Primary Key

        // Stacking of properties applied multiple annotations
        // to the following property
        [MinLength(10)]
        [MaxLength(60)]
        public string Email { get; set; } 

        [MaxLength(60)]
        public string UserName { get; set; }

        [MaxLength(60)]
        public string FullName { get; set; }

        public ApplicationUser BaseUser { get; set; } // this represents a 1 to 1 relationship

        public List<TrelloBoard> Boards { get; set; } // this represents a 1 to many (boards) relationship; 
                                                       // this is a navigation property; it will hold true even if it's not in the Board Class

    }
}