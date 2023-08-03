using PredescuAlexandru_API.Models;

namespace PredescuAlexandru_API.Repository.Interfaces
{
    public interface IMembershipTypesRepository
    {
        Task<IEnumerable<MembershipType>> GetMembershipTypesAsync();

        Task<MembershipType> GetMembershipTypeByIdAsync(Guid id);

        Task CreateMembershipTypeAsync(MembershipType membershipType);

        Task<MembershipType> UpdateMembershipTypeAsync(Guid id, MembershipType membershipType);

        Task<MembershipType> UpdatePartiallyMembershipTypeAsync(Guid id, MembershipType membershipType);

        Task<bool> DeleteMembershipTypetAsync(Guid id);
    }
}
