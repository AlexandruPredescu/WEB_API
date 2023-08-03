using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PredescuAlexandru_API.DataContext;
using PredescuAlexandru_API.Exceptions;
using PredescuAlexandru_API.Helpers.Enum;
using PredescuAlexandru_API.Models;
using PredescuAlexandru_API.Repository.Interfaces;

namespace PredescuAlexandru_API.Repository
{
    public class MembersRepository : IMembersRepository
    {
        private readonly ClubLibraDataContext _context;

        public MembersRepository(ClubLibraDataContext context)
        {
            _context = context;
        }


        //Get
        public async Task<IEnumerable<Member>> GetMembersAsync()
        {
            return await _context.Members.ToListAsync();
        }


        //Get by Id
        public async Task<Member> GetMemberByIdAsync(Guid id)
        {
            return await _context.Members.SingleOrDefaultAsync(a => a.IdMember == id);
        }


        //Post

        public async Task CreateMemberAsync(Member member)
        {
            member.IdMember = Guid.NewGuid();
            bool title = await TitleExist(member.Title);
            if(title)
            {
                throw new ModelValidationException(ErrorMessagesEnum.Announcement.TitleExistError);
            }
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
        }


        //Put
        public async Task<Member> UpdateMemberAsync(Guid id, Member member)
        {
            if(!await ExistMemberAsync(id))
            {
                return null;
            }
            if(member != null) 
            { 
                _context.Members.Update(member);
                await _context.SaveChangesAsync();
            }
            return member;
        }


        //Delete
        public async Task<bool> DeleteMemberAsync(Guid id)
        {
            if(!await ExistMemberAsync(id))
            {
                return false;
            }
            
            _context.Members.Remove(new Member { IdMember = id });
            await _context.SaveChangesAsync();
            return true;
        }


        //Patch
        public async Task<Member> UpdatePartiallyMemberAsync(Guid id, Member member)
        {
            var memberFromDatabase = await GetMemberByIdAsync(id);
            bool memberIsChanged = false;
            if (memberFromDatabase == null)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(member.Name) && memberFromDatabase.Name != member.Name)
            {
                memberFromDatabase.Name = member.Name;
                memberIsChanged = true;
            }
            if (!string.IsNullOrEmpty(member.Title) && memberFromDatabase.Title != member.Title)
            {
                memberFromDatabase.Title = member.Title;
                memberIsChanged = true;
            }
            if (!string.IsNullOrEmpty(member.Position) && memberFromDatabase.Position != member.Position)
            {
                memberFromDatabase.Position = member.Position;
                memberIsChanged = true;
            }
            if (!string.IsNullOrEmpty(member.Description) && memberFromDatabase.Description != member.Description)
            {
                memberFromDatabase.Description = member.Description;
                memberIsChanged = true;
            }
            if (!string.IsNullOrEmpty(member.Resume) && memberFromDatabase.Resume != member.Resume)
            {
                memberFromDatabase.Resume = member.Resume;
                memberIsChanged = true;
            }
            if (!string.IsNullOrEmpty(member.Username) && memberFromDatabase.Username != member.Username)
            {
                memberFromDatabase.Username = member.Username;
                memberIsChanged = true;
            }
            if (!string.IsNullOrEmpty(member.Password) && memberFromDatabase.Password != member.Password)
            {
                memberFromDatabase.Password = member.Password;
                memberIsChanged = true;
            }
            _context.Update(memberFromDatabase);
            _context.SaveChanges();
            return memberFromDatabase;
        }


        private async Task<bool> TitleExist(string Title)
        {
            return await _context.Members.CountAsync(a => a.Title == Title) > 0;
        }

        private async Task<bool> ExistMemberAsync(Guid id)
        {
            return await _context.Members.CountAsync(a => a.IdMember == id) > 0;
        }
    }
    

}
