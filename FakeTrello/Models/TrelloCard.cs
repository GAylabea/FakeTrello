using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeTrello.Models
{
    public class TrelloCard
    {
        [Key]
        public int CardId { get; set; }
        public string CardName { get; set; }
        public string CardDescription { get; set; }
        public TrelloList BelongsTo { get; set; } // this is an auxillary navigation property 
                                                  // (but it assumes you have an instance of a TrelloList before hand)
                                                  // this will return the TrelloList it belongs to with the .BelongsTo notation
    }
}