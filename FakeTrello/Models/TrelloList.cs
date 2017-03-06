using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeTrello.Models
{
    public class TrelloList
    {
        [Key]
        public int ListId { get; set; }

        public string ListName { get; set; }

        public TrelloBoard Board { get; set; } // this is the 1 to 1 relationship 

        public List<TrelloCard> Cards { get; set; } // since Lists are on a board; this represents 1 to many

    }
}