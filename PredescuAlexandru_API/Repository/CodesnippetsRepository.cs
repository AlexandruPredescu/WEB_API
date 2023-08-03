using Microsoft.EntityFrameworkCore;
using PredescuAlexandru_API.DataContext;
using PredescuAlexandru_API.Exceptions;
using PredescuAlexandru_API.Helpers.Enum;
using PredescuAlexandru_API.Models;
using PredescuAlexandru_API.Repository.Interfaces;

namespace PredescuAlexandru_API.Repository
{
    public class CodesnippetsRepository : ICodesnippetsRepository
    {
        private readonly ClubLibraDataContext _context;

        public CodesnippetsRepository(ClubLibraDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CodeSnippet>> GetSnippetsAsync()
        {
            return await _context.CodeSnippets.ToListAsync();
        }

        public async Task<CodeSnippet> GetSnippetByIdAsync(Guid id)
        {
            return await _context.CodeSnippets.SingleOrDefaultAsync(a => a.IdCodeSnippet == id);
        }

        public async Task CreateSnippetAsync(CodeSnippet codeSnippet)
        {
            codeSnippet.IdCodeSnippet = Guid.NewGuid();
            bool title = await TitleExist(codeSnippet.Title);
            if (title)
            {
                throw new ModelValidationException(ErrorMessagesEnum.Announcement.TitleExistError);
            }
            _context.CodeSnippets.Add(codeSnippet);
            await _context.SaveChangesAsync();
        }

        public async Task<CodeSnippet> UpdateCodeSnippetAsync(Guid id, CodeSnippet codeSnippet)
        {
            if (!await ExistCodesnippetAsync(id))
            {
                return null;
            }
            if (codeSnippet != null)
            {
                _context.CodeSnippets.Update(codeSnippet);
                await _context.SaveChangesAsync();
            }
            return codeSnippet;
        }

        public async Task<CodeSnippet> UpdatePartiallyCodeSnippetAsync(Guid id, CodeSnippet codeSnippet)
        {
            var codesnippetFromDatabase = await GetSnippetByIdAsync(id);
            bool codesnippetIsChanged = false;
            if (codesnippetFromDatabase == null)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(codeSnippet.ContentCode) && codesnippetFromDatabase.ContentCode != codeSnippet.ContentCode)
            {
                codesnippetFromDatabase.ContentCode = codeSnippet.ContentCode;
                codesnippetIsChanged = true;
            }
            if (!string.IsNullOrEmpty(codeSnippet.Title) && codesnippetFromDatabase.Title != codeSnippet.Title)
            {
                codesnippetFromDatabase.Title = codeSnippet.Title;
                codesnippetIsChanged = true;
            }
            if (!string.IsNullOrEmpty(codeSnippet.IdMember.ToString()) && codesnippetFromDatabase.IdMember != codeSnippet.IdMember)
            {
                codesnippetFromDatabase.IdMember = codeSnippet.IdMember;
                codesnippetIsChanged = true;
            }
            if (codeSnippet.DateTimeAdded != null && codesnippetFromDatabase.DateTimeAdded != codeSnippet.DateTimeAdded)
            {
                codesnippetIsChanged = true;
                codesnippetFromDatabase.DateTimeAdded = codeSnippet.DateTimeAdded;
            }
            if (codeSnippet.Revision != null && codesnippetFromDatabase.Revision != codeSnippet.Revision)
            {
                codesnippetIsChanged = true;
                codesnippetFromDatabase.Revision = codeSnippet.Revision;
            }
            if (codeSnippet.IsPublished != null && codesnippetFromDatabase.IsPublished != codeSnippet.IsPublished)
            {
                codesnippetIsChanged = true;
                codesnippetFromDatabase.IsPublished = codeSnippet.IsPublished;
            }

            if (!codesnippetIsChanged)
            {
                throw new ModelValidationException(ErrorMessagesEnum.Announcement.ZeroUpdateToSave);
            }
            // ValidationFunctions.ThrowExceptionWhenDateIsNotValid(codesnippetFromDatabase.ValidFrom, announcementFromDatabase.ValidTo);
            _context.Update(codesnippetFromDatabase);
            _context.SaveChanges();
            return codesnippetFromDatabase;


        }

        public async Task<bool> DeleteCodesnippetAsync(Guid id)
        {
            if (!await ExistCodesnippetAsync(id))
            {
                return false;
            }

            _context.CodeSnippets.Remove(new CodeSnippet { IdCodeSnippet = id });
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> ExistCodesnippetAsync(Guid id)
        {
            return await _context.CodeSnippets.CountAsync(a => a.IdCodeSnippet == id) > 0;
        }

        private async Task<bool> TitleExist(string Title)
        {
            return await _context.CodeSnippets.CountAsync(a => a.Title == Title) > 0;
        }
    }

}
