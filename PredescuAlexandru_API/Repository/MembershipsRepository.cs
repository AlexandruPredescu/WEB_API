using Microsoft.EntityFrameworkCore;
using PredescuAlexandru_API.DataContext;
using PredescuAlexandru_API.Exceptions;
using PredescuAlexandru_API.Helpers.Enum;
using PredescuAlexandru_API.Models;
using PredescuAlexandru_API.Repository.Interfaces;

namespace PredescuAlexandru_API.Repository
{
    public class MembershipsRepository :IMembershipsRepository
    {
        private readonly ClubLibraDataContext _context;

        public MembershipsRepository(ClubLibraDataContext context)
        {
            _context = context;
        }


        //Get
        public async Task<IEnumerable<Membership>> GetMembershipsAsync()
        {
            return await _context.Memberships.ToListAsync();
        }


        //Get by id
        public async Task<Membership> GetMembershipByIdAsync(Guid id)
        {
            return await _context.Memberships.SingleOrDefaultAsync(a => a.IdMembership == id);
        }


        //Post
        public async Task CreateMembershipAsync(Membership membership)
        {
            membership.IdMembership = Guid.NewGuid();
            //membership.IdMember = Guid.NewGuid();
            //membership.IdMembershipType = Guid.NewGuid();
            _context.Memberships.Add(membership);
            await _context.SaveChangesAsync();
        }

        //Put
        public async Task<Membership> UpdateMembershipAsync(Guid id, Membership membership)
        {
            if (!await ExistMembershipAsync(id))
            {
                return null;
            }
            if (membership != null)
            {
                _context.Memberships.Update(membership);
                await _context.SaveChangesAsync();
            }
            return membership;
        }


        //Delete
        public async Task<bool> DeleteMemberAsync(Guid id)
        {
            if (!await ExistMembershipAsync(id))
            {
                return false;
            }

            _context.Memberships.Remove(new Membership { IdMembership = id });
            await _context.SaveChangesAsync();
            return true;
        }


        //Patch
        public async Task<Membership> UpdatePartiallyMembershipAsync(Guid id, Membership membership)
        {
            var membershipFromDatabase = await GetMembershipByIdAsync(id);
            bool membershipIsChanged = false;
            if (membershipFromDatabase == null)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(membership.IdMember.ToString()) && membershipFromDatabase.IdMember != membership.IdMember)
            {
                membershipFromDatabase.IdMember = membership.IdMember;
                membershipIsChanged = true;
            }
            if (!string.IsNullOrEmpty(membership.IdMembershipType.ToString()) && membershipFromDatabase.IdMembershipType != membership.IdMembershipType)
            {
                membershipFromDatabase.IdMembershipType = membership.IdMembershipType;
                membershipIsChanged = true;
            }
            if (membership.StartDate != null && membershipFromDatabase.StartDate != membership.StartDate)
            {
                membershipFromDatabase.StartDate = membership.StartDate;
                membershipIsChanged = true;
            }
            if (membership.EndTime != null && membershipFromDatabase.EndTime != membership.EndTime)
            {
                membershipFromDatabase.EndTime = membership.EndTime;
                membershipIsChanged = true;
            }
            if (membership.Level != null && membershipFromDatabase.Level != membership.Level)
            {
                membershipFromDatabase.Level = membership.Level;
                membershipIsChanged = true;
            }

            if (!membershipIsChanged)
            {
                throw new ModelValidationException(ErrorMessagesEnum.Announcement.ZeroUpdateToSave);
            }
            _context.Update(membershipFromDatabase);
            _context.SaveChanges();
            return membershipFromDatabase;
        }



        public async Task<bool> ExistMembershipAsync(Guid id)
        {
            return await _context.Memberships.CountAsync(a => a.IdMembership == id) > 0;
        }
    }
}
