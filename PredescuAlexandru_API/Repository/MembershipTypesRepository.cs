using Microsoft.EntityFrameworkCore;
using PredescuAlexandru_API.DataContext;
using PredescuAlexandru_API.Exceptions;
using PredescuAlexandru_API.Helpers.Enum;
using PredescuAlexandru_API.Models;
using PredescuAlexandru_API.Repository.Interfaces;

namespace PredescuAlexandru_API.Repository
{
    public class MembershipTypesRepository :IMembershipTypesRepository
    {
        private readonly ClubLibraDataContext _context;

        public MembershipTypesRepository(ClubLibraDataContext context)
        {
            _context = context;
        }

        //Get
        public async Task<IEnumerable<MembershipType>> GetMembershipTypesAsync()
        {
            return await _context.MembershipTypes.ToListAsync();
        }

        //Get by id
        public async Task<MembershipType> GetMembershipTypeByIdAsync(Guid id)
        {
            return await _context.MembershipTypes.SingleOrDefaultAsync(a => a.IdMembershipType == id);
        }

        //Post
        public async Task CreateMembershipTypeAsync(MembershipType membershipType)
        {
            membershipType.IdMembershipType = Guid.NewGuid();
            _context.MembershipTypes.Add(membershipType);
            await _context.SaveChangesAsync();
        }

        //Update
        public async Task<MembershipType> UpdateMembershipTypeAsync(Guid id, MembershipType membershipType)
        {
            if (!await ExistMembershipTypeAsync(id))
            {
                return null;
            }
            if (membershipType != null)
            {
                _context.MembershipTypes.Update(membershipType);
                await _context.SaveChangesAsync();
            }
            return membershipType;
        }

        //Delete
        public async Task<bool> DeleteMembershipTypetAsync(Guid id)
        {
            if (!await ExistMembershipTypeAsync(id))
            {
                return false;
            }

            _context.MembershipTypes.Remove(new MembershipType { IdMembershipType = id });
            await _context.SaveChangesAsync();
            return true;
        }

        //Patch
        public async Task<MembershipType> UpdatePartiallyMembershipTypeAsync(Guid id, MembershipType membershipType)
        {
            var membershipTypeFromDatabase = await GetMembershipTypeByIdAsync(id);
            bool membershipTypeIsChanged = false;
            if (membershipTypeFromDatabase == null)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(membershipType.Name) && membershipTypeFromDatabase.Name != membershipType.Name)
            {
                membershipTypeFromDatabase.Name = membershipType.Name;
                membershipTypeIsChanged = true;
            }
            if (!string.IsNullOrEmpty(membershipType.Description) && membershipTypeFromDatabase.Description != membershipType.Description)
            {
                membershipTypeFromDatabase.Description = membershipType.Description;
                membershipTypeIsChanged = true;
            }
            if (membershipType.SubcriptionLenghtInMonths != null && membershipTypeFromDatabase.SubcriptionLenghtInMonths != membershipType.SubcriptionLenghtInMonths);
            {
                membershipTypeFromDatabase.SubcriptionLenghtInMonths = membershipType.SubcriptionLenghtInMonths;
                membershipTypeIsChanged = true;
            }

            if (!membershipTypeIsChanged)
            {
                throw new ModelValidationException(ErrorMessagesEnum.Announcement.ZeroUpdateToSave);
            }
            _context.Update(membershipTypeFromDatabase);
            _context.SaveChanges();
            return membershipTypeFromDatabase;
        }


        private async Task<bool> ExistMembershipTypeAsync(Guid id)
        {
            return await _context.MembershipTypes.CountAsync(a => a.IdMembershipType == id) > 0;
        }
    }
}
