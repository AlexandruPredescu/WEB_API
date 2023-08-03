using PredescuAlexandru_API.Models;

namespace PredescuAlexandru_API.Repository.Interfaces
{
    public interface ICodesnippetsRepository
    {
        Task<IEnumerable<CodeSnippet>> GetSnippetsAsync();

        Task<CodeSnippet> GetSnippetByIdAsync(Guid id);

        Task CreateSnippetAsync(CodeSnippet codeSnippet);

        Task<CodeSnippet> UpdateCodeSnippetAsync(Guid id, CodeSnippet codeSnippet);

        Task<CodeSnippet> UpdatePartiallyCodeSnippetAsync(Guid id, CodeSnippet codeSnippet);

        Task<bool> DeleteCodesnippetAsync(Guid id);
    }
}
