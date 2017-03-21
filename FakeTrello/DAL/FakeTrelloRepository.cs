using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FakeTrello.Models;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Diagnostics;

namespace FakeTrello.DAL
{
    public class FakeTrelloRepository : IRepository
    {

        //public FakeTrelloContext Context { get; set; }
        //private FakeTrelloContext context; // Data member
        SqlConnection _trelloConnection; // private fields on a class should look like this - with the underscore

        // string mystring = "";

        public FakeTrelloRepository() // btw: this is called the default constructor
        {
            //Context = new FakeTrelloContext();
            _trelloConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefualtConnection"].ConnectionString);         
            // now we can make a SQL connection  
        }

        //public FakeTrelloRepository(FakeTrelloContext context)
        //{
            //Context = context;
        //}

       public void AddBoard(string name, ApplicationUser owner)
        {
            //Board board = new Board { Name = name, Owner = owner };
            //Context.Boards.Add(board);
            //Context.SaveChanges();

            _trelloConnection.Open();

            try
            {
                // insert statement; inserts things into a database!
                // we insert into a table called Boards - these two colums, Name and Owner

                var addBoardCommand = _trelloConnection.CreateCommand();
                addBoardCommand.CommandText = "Insert into Boards(Name,Owner_Id) values(@name,@ownerId)"; // this is basically our old method
                var nameParameter = new SqlParameter("name", SqlDbType.VarChar);
                nameParameter.Value = name;
                addBoardCommand.Parameters.Add(nameParameter);

                var ownerParameter = new SqlParameter("owner", SqlDbType.Int);
                ownerParameter.Value = owner.Id;
                addBoardCommand.Parameters.Add(ownerParameter);

                addBoardCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally // you could just do this one line; but you NEED to make sure the connection closes
            {
                _trelloConnection.Close(); // be nice and close your connectin at the end!
            }
        }

        public void AddCard(string name, int listId, string ownerId)
        {
            throw new NotImplementedException();
        }

        public void AddCard(string name, List list, ApplicationUser owner)
        {
            throw new NotImplementedException();
        }

        public void AddList(string name, int boardId)
        {
            throw new NotImplementedException();
        }

        public void AddList(string name, Board board)
        {
            throw new NotImplementedException();
        }

        public bool AttachUser(string userId, int cardId)
        {
            throw new NotImplementedException();
        }

        public bool CopyCard(int cardId, int newListId, string newOwnerId)
        {
            throw new NotImplementedException();
        }

        public Board GetBoard(int boardId)
        {
            _trelloConnection.Open();

            try
            {
                var getBoardCommand = _trelloConnection.CreateCommand();
                getBoardCommand.CommandText = @"SELECT boardId, Name, URL, Owner_Id 
                                FROM Boards 
                                WHERE BoardId = @boardId"; // the @ means it is a variable that is treated as parameter here
                var boardIdParam = new SqlParameter("boardId", SqlDbType.Int); // now we create the parameter
                boardIdParam.Value = boardId;

                getBoardCommand.Parameters.Add(boardIdParam);

                var reader = getBoardCommand.ExecuteReader(); // this will be a SQL data reader; now we can iterate over it

                reader.Read(); // this says get the first record; go from no row to having the first one

                if (reader.Read())
                {
                    var board = new Board();
                    {
                        board.BoardId = reader.GetInt32(0);
                        board.Name = reader.GetString(1);
                        board.URL = reader.GetString(2);
                        board.Owner = new ApplicationUser { Id = reader.GetString(3) };
                    };

                    return board;
                }

            }
            finally
            {
                _trelloConnection.Close();
            }
            return null;// we need the null for the items that had an exception

            // SELECT * FROM Boards WHERE BoardId = boardId 


            //Board found_board = Context.Boards.FirstOrDefault(b => b.BoardId == boardId); // returns null if nothing is found
            //return found_board;

            /* Using .First() throws an exception if nothing is found
             * try {
             * Board found_board = Context.Boards.First(b => b.BoardId == boardId); 
             * return found_board;
             * } catch(Exception e) {
             * return null;
             * }
             */
        }

        public List<Board> GetBoardsFromUser(string userId)
        {
            _trelloConnection.Open();

            try
            {
                var getBoardCommand = _trelloConnection.CreateCommand();
                getBoardCommand.CommandText = @"SELECT boardId, Name, URL, Owner_Id 
                                FROM Boards 
                                WHERE Ownder_Id = @userId"; // the @ means it is a variable that is treated as parameter here
                var boardIdParam = new SqlParameter("userId", SqlDbType.VarChar); // now we create the parameter
                boardIdParam.Value = userId;

                getBoardCommand.Parameters.Add(boardIdParam);

                var reader = getBoardCommand.ExecuteReader(); // this will be a SQL data reader; now we can iterate over it

                reader.Read(); // this says get the first record; go from no row to having the first one

                var boards = new List<Board>();
                while (reader.Read()) //returns true if there is a row, false if not; we loop thru and Add a board to the boards list
                {
                    var board = new Board();
                    {
                        board.BoardId = reader.GetInt32(0);
                        board.Name = reader.GetString(1);
                        board.URL = reader.GetString(2);
                        board.Owner = new ApplicationUser { Id = reader.GetString(3) };
                    };

                    boards.Add(board); // remember - Add() is a class generated method
                }
                return boards;
            }
            catch (Exception);
            finally
            {
                _trelloConnection.Close();
            }
            return new List<Board>();// we need the null for the items that had an exception

        }

        public Card GetCard(int cardId)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationUser> GetCardAttendees(int cardId)
        {
            throw new NotImplementedException();
        }

        public List<Card> GetCardsFromBoard(int boardId)
        {
            throw new NotImplementedException();
        }

        public List<Card> GetCardsFromList(int listId)
        {
            throw new NotImplementedException();
        }

        public List GetList(int listId)
        {
            throw new NotImplementedException();
        }

        public List<List> GetListsFromBoard(int boardId)
        {
            throw new NotImplementedException();
        }

        public bool MoveCard(int cardId, int oldListId, int newListId)
        {
            throw new NotImplementedException();
        }

        public bool removeBoardCommand(int boardId)
        {
            _trelloConnection.Open();

            try
            {
                var removeBoardCommand = _trelloConnection.CreateCommand();
                removeBoardCommand.CommandText = @"
                    Delete
                    From Boards
                    Where boardId = @boardId"; // pretty much a Select statement but it's delete
                var boardIdParameter = new SqlParameter("name", SqlDbType.Int);
                boardIdParameter.Value = boardId;
                removeBoardCommand.Parameters.Add(boardIdParameter);

                var ownerParameter = new SqlParameter("owner", SqlDbType.Int);
                boardIdParameter.Value = boardId;
                removeBoardCommand.Parameters.Add(boardIdParameter);

                removeBoardCommand.ExecuteNonQuery();
                return true;
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally // you could just do this one line; but you NEED to make sure the connection closes
            {
                _trelloConnection.Close(); // be nice and close your connectin at the end!
            }
            return false;
        }

        public bool RemoveCard(int cardId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveList(int listId)
        {
            throw new NotImplementedException();
        }

       public void updateBoardCommand(int boardId, string newname)
        {
            _trelloConnection.Open();

            try
            {
                var updateBoardCommand = _trelloConnection.CreateCommand();
                updateBoardCommand.CommandText = @"
                        Update Boards
                        Set Name = @name
                        Where boardId = @boardId"; 
                var nameParameter = new SqlParameter("name", SqlDbType.VarChar);
                nameParameter.Value = newname;
                updateBoardCommand.Parameters.Add(nameParameter);

                var boardIdParameter = new SqlParameter("boardId", SqlDbType.Int);
                boardIdParameter.Value = boardId;
                updateBoardCommand.Parameters.Add(boardIdParameter);

                updateBoardCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            finally // you could just do this one line; but you NEED to make sure the connection closes
            {
                _trelloConnection.Close(); // be nice and close your connectin at the end!
            }
        }
    }
}