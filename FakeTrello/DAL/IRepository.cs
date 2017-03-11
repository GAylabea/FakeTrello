
using FakeTrello.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeTrello.DAL
{
    public interface IRepository
    {
        // List of methods to help deliver business features (CRUD)
        // Create
        void AddBoard(string name, ApplicationUser owner);
        void AddList(string name, Board board);
        void AddList(string name, int boardId); // an overload; having this option makes it flexible
        void AddCard(string name, List list, ApplicationUser owner);
        void AddCard(string name, int listId, string ownerId);

        // Read
        List<Card> GetCardsFromList(int listId); // the user is looking at the cards on the Board so we need the user to see the list
        List<Card> GetCardsFromBoard(int boardId); // we want to get all the cards on one Board
        Card GetCard(int cardId); // we want to get cards by its Id
        List GetList(int listId); // list if we only know the Id
        List<List> GetListsFromBoard(int boardId); // list of Trello Lists
        List<Board> GetBoardsFromUser(string userId);
        Board GetBoard(int boardId);
        List<ApplicationUser> GetCardAttendees(int cardId); // lists the users that are attached to the card

        // Update
        bool AttachUser(string userId, int cardId); // we need to know if we've reached the max so changed to bool; true = success, false = not
        bool MoveCard(int cardId, int oldListId, int newListId);
        bool CopyCard(int cardId, int newListId, string newOwnerId); // takes the content, places in new list

        // Delete
        bool RemoveBoard(int boardId); // bool is telling if it was successful or not
        bool RemoveList(int listId);
        bool RemoveCard(int cardId);
    }
}
