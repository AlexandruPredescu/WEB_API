using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PredescuAlexandru_API.DataContext;
using PredescuAlexandru_API.Models;
using PredescuAlexandru_API.Repository;
using PredescuAlexandru_API.Tests.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredescuAlexandru_API.Tests.RepositoryTest
{
    public class AnnoucementRepositoryTest
    {
        private readonly AnnoucementsRepository _announcementeRepository;
        private readonly ClubLibraDataContext _context;

        public AnnoucementRepositoryTest()
        {
            _context = DbContextHelper.GetDatabaseContext();
            _announcementeRepository = new AnnoucementsRepository(_context);
        }


        [Fact]
        public async Task GetAllAnnouncements_ExistAnnoucements()
        {
            //Arrage -> voi crea cateva anunturi fake in memorie
            Announcement announcement1 = CreateAnnouncement(Guid.NewGuid(), "Anunt1");
            Announcement announcement2 = CreateAnnouncement(Guid.NewGuid(), "Anunt2");
            DbContextHelper.AddAnnouncement(_context, announcement1);
            DbContextHelper.AddAnnouncement(_context, announcement2);

            //Act -> chem metoda pe care vreau sa o testez
            var dbAnnouncements = await _announcementeRepository.GetAnnouncementsAsync();

            //Assert -> Verifica rezultatul
            Assert.Equal(2, dbAnnouncements.Count());
        }


        [Fact]
        public async Task GetAllAnnouncements_WithoutDataInDatabase()
        {
            //Act
            var dbAnnouncements = await _announcementeRepository.GetAnnouncementsAsync();

            //Assert
            Assert.Empty(dbAnnouncements);
        }


        [Fact]
        public async Task GetAnnouncementById_WithData()
        {
            //Arrage -> creez un anunt fals
            Guid id = Guid.NewGuid();
            Announcement announcement = CreateAnnouncement(id, "Anunt1");
            DbContextHelper.AddAnnouncement(_context, announcement);

            //Act
            var result = await _announcementeRepository.GetAnnouncementByIdAsync(id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(announcement.IdAnnouncement, result.IdAnnouncement);
            Assert.Equal(announcement.Title, result.Title);
            var serializedAnnoucement = JsonConvert.SerializeObject(announcement);
            var serializedResult = JsonConvert.SerializeObject(result);
            Assert.Equal(serializedAnnoucement, serializedResult);
        }


        [Fact]
        public async Task GetAnnouncementById_WithoutData()
        {
            Guid id = Guid.NewGuid() ;

            //Act
            var result = await _announcementeRepository.GetAnnouncementByIdAsync(id);

            //Assert
            Assert.Null(result);
        }


        [Fact]
        public async Task DeleteAnnouncement_WhenExist()
        {
            //Arrange
            Announcement announcement = CreateAnnouncement(Guid.NewGuid(), "Anunt de sters");
            DbContextHelper.AddAnnouncement(_context, announcement);

            //Act
            bool isDelete = await _announcementeRepository.DeleteAnnouncementAsync(announcement.IdAnnouncement);
            var result = await _announcementeRepository.GetAnnouncementByIdAsync(announcement.IdAnnouncement);

            //Assert
            Assert.True(isDelete);
            Assert.Null(result);    
        }


        [Fact]
        public async Task DeleteAnnouncement_WhenNotExists()
        {
            Guid id = Guid.NewGuid() ;
            
            bool result = await _announcementeRepository.DeleteAnnouncementAsync(id);

            Assert.False(result);
        }


        [Fact]
        public async Task UpdatePartially_WhenExist()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            Announcement announcement = CreateAnnouncement(id, "Anunt");
            DbContextHelper.AddAnnouncement(_context, announcement);

            //Act
            announcement.EventDate = DateTime.Now.Date;
            var result = await _announcementeRepository.UpdatePartiallyAnnouncementAsync(announcement.IdAnnouncement, announcement);

            //Assert
            Assert.Equal(announcement.EventDate, DateTime.Now.Date);
        }

        private Announcement CreateAnnouncement(Guid id, string title)
        {
            Announcement announcement = new Announcement()
            {
                IdAnnouncement = id,
                Title = title,
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow,
                Tags = "#Tags",
                Text = "test",
                EventDate = DateTime.UtcNow
            };
            return announcement;
        }
    }
}
