using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeTrello.Models
{
    public class TrelloBoard
    {
        [Key]
        public int BoardId { get; set; }

        public string Name { get; set; }

        public string URL { get; set; }
        
        // Auxiliary (not required to define relationship; it's in TrelloUser)
        public TrelloUser Owner { get; set; }

        public TrelloUser User { get; set; } // this is the 1 to 1 relationship 

        public List<TrelloList> BoardLists { get; set; } // since Lists are on a board; this represents 1 to many

     }
}