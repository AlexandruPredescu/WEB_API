using PredescuAlexandru_API.Models;

namespace PredescuAlexandru_API.Repository.Interfaces
{
    public interface IMembersRepository
    {
        Task<IEnumerable<Member>> GetMembersAsync();

        Task<Member> GetMemberByIdAsync(Guid id);

        Task CreateMemberAsync(Member member);

        Task<Member> UpdateMemberAsync(Guid id, Member member);

        Task<Member> UpdatePartiallyMemberAsync(Guid id,Member member);

        Task<bool> DeleteMemberAsync (Guid id);
    }
}
