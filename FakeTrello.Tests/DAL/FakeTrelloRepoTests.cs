using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakeTrello.DAL;
using Moq;
using FakeTrello.Models;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace FakeTrello.Tests.DAL
{
    [TestClass]
    public class FakeTrelloRepoTests
    {
        public Mock<FakeTrelloContext> fake_context { get; set; }
        public FakeTrelloRepository repo { get; set; }
        public Mock<DbSet<Board>> mock_boards_set { get; set; }
        public IQueryable<Board> query_boards { get; set; }
        public List<Board> fake_board_table { get; set; }

        [TestInitialize]
        public void Setup()
        {
            fake_context = new Mock<FakeTrelloContext>();
            mock_boards_set = new Mock<DbSet<Board>>();
            FakeTrelloRepository repo = new FakeTrelloRepository(fake_context.Object);
        }

        public void CreateFakeDatabase()
        {
            query_boards = fake_board_table.AsQueryable();
            mock_boards_set.As<IQueryable<Board>>().Setup(b => b.Provider).Returns(query_boards.Provider);
            // we've told LINQ to use the Provider from our fake board table/list
            mock_boards_set.As<IQueryable<Board>>().Setup(b => b.Expression).Returns(query_boards.Expression);
            // this will build the expression (SQL statement) 
            mock_boards_set.As<IQueryable<Board>>().Setup(b => b.ElementType).Returns(query_boards.ElementType);
            // make sure it stays a board instance by profiding the Element Type
            mock_boards_set.As<IQueryable<Board>>().Setup(b => b.GetEnumerator()).Returns(() => query_boards.GetEnumerator());
            // tells the system the order number of items; items will change according to test so it must be aware that items will change    
            mock_boards_set.Setup(b => b.Add(It.IsAny<Board>())).Callback((Board board) => fake_board_table.Add(board));
            // this mocks the Add call
            fake_context.Setup(c => c.Boards).Returns(mock_boards_set.Object);
            // so ANY context.boards gives them the fake_board_table set which is a List
        }

        [TestMethod]
        public void EnsureICanCreateInstanceOfRepo()
        {
            FakeTrelloRepository repo = new FakeTrelloRepository();

            Assert.IsNotNull(repo);
        }
        [TestMethod]
        public void EnsureICanInjectContextInstance()
        {
            FakeTrelloContext context = new FakeTrelloContext();
            FakeTrelloRepository repo = new FakeTrelloRepository(context); // this is so we can inject a mock

            Assert.IsNotNull(repo.Context);
        }
        [TestMethod]
        public void EnsureIHaveANotNullContext()
        {
            FakeTrelloRepository repo = new FakeTrelloRepository();

            Assert.IsNotNull(repo.Context);
        }
        [TestMethod]
        public void EnsureICanAddBoard()
        {
            // Arrange

            CreateFakeDatabase();

            ApplicationUser a_user = new ApplicationUser {
                  Id = "my-user-id",
                  UserName = "GBHall",
                  Email = "gbhall@hotmail.com"
            };
            
            // Act
            repo.AddBoard("My Board", a_user);

            // Assert
            Assert.AreEqual(1, repo.Context.Boards.Count()); // we had to add using the LINQ stuff which is good!
                                                             // we want the SQL query to return 1
        }
        [TestMethod]
        public void EnsureICanReturnBoards()
        {
            //Arrange
            fake_board_table.Add(new Board { Name = "My Board" });
            CreateFakeDatabase();

            //Act
            int expected_board_count = 1;
            int actual_board_count = repo.Context.Boards.Count();

            //Assert
            Assert.AreEqual(expected_board_count, actual_board_count);

        }
        [TestMethod]
        public void EnsureICanFindABoard()
        {
            // Arrange
            fake_board_table.Add(new Board { BoardId = 1, Name = "My Board" });
            CreateFakeDatabase();

            // Act
            string expected_board_name = "My Board";
            Board actual_board = repo.GetBoard(1);
            string actual_board_name = repo.GetBoard(1).Name;

            // Assert
            Assert.IsNotNull(actual_board);
            Assert.AreEqual(expected_board_name, actual_board_name);
        }
    }
}
