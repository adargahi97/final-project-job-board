﻿using Job_Board.Controllers;
using Job_Board.Daos;
using Moq;

namespace JobBoard.UnitTest
{
    [TestClass]
    public class CandidateControllerTests
    {

        [TestMethod]

        public void CallDao()
        {
            Mock<ICandidateDao> mockCandidateDao = new Mock<ICandidateDao>();

            CandidateController sut = new CandidateController(mockCandidateDao.Object);

            //sut.CallDao();

            mockCandidateDao.Verify(candidateDao => candidateDao.GetCandidate(), Times.Once);
        }


        
    }
}
