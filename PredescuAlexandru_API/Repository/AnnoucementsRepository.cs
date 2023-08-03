using Microsoft.EntityFrameworkCore;
using PredescuAlexandru_API.DataContext;
using PredescuAlexandru_API.Exceptions;
using PredescuAlexandru_API.Helpers.Enum;
using PredescuAlexandru_API.Models;
using PredescuAlexandru_API.Repository.Interfaces;

namespace PredescuAlexandru_API.Repository
{
    public class AnnoucementsRepository : IAnnouncementsRepository
    {
        private readonly ClubLibraDataContext _context;

        public AnnoucementsRepository(ClubLibraDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Announcement>> GetAnnouncementsAsync()
        {
            return await _context.Announcements.ToListAsync();
        }

        public async Task<Announcement> GetAnnouncementByIdAsync(Guid id)
        {
            return await _context.Announcements.SingleOrDefaultAsync(a => a.IdAnnouncement == id);
        }

        public async Task CreateAnnouncementAsync(Announcement announcement)
        {
            announcement.IdAnnouncement = Guid.NewGuid();
            ValidationFunctions.ThrowExceptionWhenDateIsNotValid(announcement.ValidFrom, announcement.ValidTo);
            bool title = await TitleExist(announcement.Title);
            if (title)
            {
                throw new ModelValidationException(ErrorMessagesEnum.Announcement.TitleExistError);
            }
            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();
        }

        public async Task<Announcement> UpdateAnnouncementAsync(Guid id, Announcement announcement)
        {
            if(!await ExistAnnoucementAsync(id))
            {
                return null;
            }
            if (announcement != null)
            {
                _context.Announcements.Update(announcement);
                await _context.SaveChangesAsync();
            }
            return announcement;
        }

        public async Task<Announcement> UpdatePartiallyAnnouncementAsync(Guid id, Announcement announcement)
        {
            var announcementFromDatabase = await GetAnnouncementByIdAsync(id);
            bool announcementIsChanged = false;
            if (announcementFromDatabase == null)
            {
                return null;
            }
            if(!string.IsNullOrEmpty(announcement.Tags) && announcementFromDatabase.Tags != announcement.Tags)
            {
                announcementFromDatabase.Tags = announcement.Tags;
                announcementIsChanged = true;
            }
            if (!string.IsNullOrEmpty(announcement.Text) && announcementFromDatabase.Text != announcement.Text)
            {
                announcementFromDatabase.Text = announcement.Text;
                announcementIsChanged = true;
            }
            if (!string.IsNullOrEmpty(announcement.Title) && announcementFromDatabase.Title != announcement.Title)
            {
                announcementFromDatabase.Title = announcement.Title;
                announcementIsChanged = true;
            }
            if(announcement.ValidFrom != null && announcementFromDatabase.ValidFrom != announcement.ValidFrom)
            {
                announcementIsChanged = true;
                announcementFromDatabase.ValidFrom = announcement.ValidFrom;
            }
            if (announcement.ValidTo != null && announcementFromDatabase.ValidTo != announcement.ValidTo)
            {
                announcementIsChanged = true;
                announcementFromDatabase.ValidTo = announcement.ValidTo;
            }
            if (announcement.EventDate != null && announcementFromDatabase.EventDate != announcement.EventDate)
            {
                announcementIsChanged = true;
                announcementFromDatabase.EventDate = announcement.EventDate;
            }

            if (!announcementIsChanged)
            {
                throw new ModelValidationException(ErrorMessagesEnum.Announcement.ZeroUpdateToSave);
            }
            ValidationFunctions.ThrowExceptionWhenDateIsNotValid(announcementFromDatabase.ValidFrom, announcementFromDatabase.ValidTo);
            _context.Update(announcementFromDatabase);
            _context.SaveChanges();
            return announcementFromDatabase;
        }

        public async Task<bool> DeleteAnnouncementAsync(Guid id)
        {
            if(!await ExistAnnoucementAsync(id))
            {
                return false;
            }

            _context.Announcements.Remove(new Announcement { IdAnnouncement = id });
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> ExistAnnoucementAsync(Guid id)
        {
            return await _context.Announcements.CountAsync( a => a.IdAnnouncement == id) > 0;
        }

        private async Task<bool> TitleExist(string Title)
        {
            return await _context.Announcements.CountAsync(a => a.Title == Title) > 0;
        }


    }
}
