using PredescuAlexandru_API.Models;

namespace PredescuAlexandru_API.Repository.Interfaces
{
    public interface IMembershipsRepository
    {
        Task<IEnumerable<Membership>> GetMembershipsAsync();

        Task<Membership> GetMembershipByIdAsync(Guid id);

        Task CreateMembershipAsync(Membership membership);

        Task<Membership> UpdateMembershipAsync(Guid id, Membership membership);

        Task<Membership> UpdatePartiallyMembershipAsync(Guid id, Membership membership);

        Task<bool> DeleteMemberAsync(Guid id);
    }
}
