using FakeTrello.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FakeTrello.DAL
{
    public class FakeTrelloContext : ApplicationDbContext
    {
        public virtual DbSet<Board> Boards { get; set; }  // so now it will list items in the Boards table
        public virtual DbSet<List> Lists { get; set; }  
        public virtual DbSet<Card> Cards { get; set; }       
    }
}