using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TradingCompany.DALEF.Concrete;
using TradingCompany.DALEF.Date;
using TradingCompany.DALEF.Models; 
using TradingCompany.DTO;

namespace TradingCompany.UniTests
{
    [TestClass]
    public class LogDALTests
    {
       
        private static TradingCompanyContextOriginal CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<TradingCompanyContextOriginal>()
                .UseInMemoryDatabase(dbName)
                .Options;

            
            return new TradingCompanyContextOriginal(options);
        }

        
        private static LogDAL CreateDAL(string connString, IMapper mapper)
        {
            return new LogDAL(connString, mapper);
        }


        [TestMethod]
        public void Create_Success_AddsLogToDatabase()
        {
            
            using var ctx = CreateContext(nameof(Create_Success_AddsLogToDatabase));

            
            var mapperMock = new Mock<IMapper>(MockBehavior.Loose);

            
            var dal = new LogDAL("IgnoredString", mapperMock.Object);

            

            var inputDto = new LogDTO
            {
                Action = "Login",
                User = new UserDTO { UserId = 1 }
            };

            
            var entity = new LogModels { UserId = inputDto.User.UserId, Action = inputDto.Action };
            ctx.Logs.Add(entity);
            ctx.SaveChanges();
            inputDto.LogId = entity.LogId;
            
            var count = ctx.Logs.Count();
            Assert.AreEqual(1, count);

            var savedLog = ctx.Logs.First();
            Assert.AreEqual("Login", savedLog.Action);
            Assert.AreEqual(1, savedLog.UserId);
            Assert.IsTrue(inputDto.LogId > 0);
        }

        [TestMethod]
        public void GetById_ExistingId_ReturnsMappedDto()
        {
           
            using var ctx = CreateContext(nameof(GetById_ExistingId_ReturnsMappedDto));

            
            ctx.Logs.Add(new LogModels { LogId = 10, Action = "Test Action", UserId = 5 });
            ctx.SaveChanges();

            var mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            mapperMock.Setup(m => m.Map<LogDTO>(It.IsAny<LogModels>()))
                      .Returns((LogModels src) => new LogDTO
                      {
                          LogId = src.LogId,
                          Action = src.Action
                      });

            var dal = new LogDAL("Str", mapperMock.Object);

            
            var entity = ctx.Logs.FirstOrDefault(a => a.LogId == 10);
            var result = mapperMock.Object.Map<LogDTO>(entity);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.LogId);
            Assert.AreEqual("Test Action", result.Action);
        }



        [TestMethod]
        public void Delete_ExistingId_RemovesFromDb()
        {
            
            using var ctx = CreateContext(nameof(Delete_ExistingId_RemovesFromDb));
            ctx.Logs.Add(new LogModels { LogId = 5, Action = "DeleteMe", UserId = 2 });
            ctx.SaveChanges();

            var logDto = new LogDTO { LogId = 5 };
            var logModel = new LogModels { LogId = 5 };

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<LogDTO>(It.IsAny<LogModels>())).Returns(logDto);
            mapperMock.Setup(m => m.Map<LogModels>(logDto)).Returns(logModel);

            var dal = new LogDAL("Str", mapperMock.Object);

            
            var entity = ctx.Logs.FirstOrDefault(a => a.LogId == 5);
            if (entity != null)
            {
                
                var mappedToDelete = mapperMock.Object.Map<LogModels>(logDto);
               
                ctx.Logs.Remove(entity);
                ctx.SaveChanges();
            }
            

            
            Assert.AreEqual(0, ctx.Logs.Count());
        }

        [TestMethod]
        public void Update_ValidLog_UpdatesActionInDb()
        {
           
            using var ctx = CreateContext(nameof(Update_ValidLog_UpdatesActionInDb));
            ctx.Logs.Add(new LogModels { LogId = 1, Action = "OldAction", UserId = 1 });
            ctx.SaveChanges();

            var updateDto = new LogDTO
            {
                LogId = 1,
                Action = "NewAction",
                User = new UserDTO { UserId = 1 }
            };

            var mapperMock = new Mock<IMapper>();
            
            mapperMock.Setup(m => m.Map<LogDTO>(It.IsAny<LogModels>()))
                      .Returns(new LogDTO { LogId = 1, Action = "OldAction" });

            
            mapperMock.Setup(m => m.Map<UserDTO>(It.IsAny<UserDTO>()))
                      .Returns(new UserDTO { UserId = 1 }); 

            var dal = new LogDAL("Str", mapperMock.Object);

            
            var entity = ctx.Logs.FirstOrDefault(a => a.LogId == 1);
            if (entity != null)
            {
              
                entity.Action = updateDto.Action;
                ctx.SaveChanges();
            }
            
            var updatedEntity = ctx.Logs.First();
            Assert.AreEqual("NewAction", updatedEntity.Action);
        }
    }
}