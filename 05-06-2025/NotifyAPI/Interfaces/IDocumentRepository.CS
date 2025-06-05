using System.Collections.Generic;
using System.Threading.Tasks;
using NotifyAPI.Models;
namespace NotifyAPI.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Document> AddDocumentAsync(Document document);
        Task<Document?> GetDocumentByIdAsync(int id);
        Task<IEnumerable<Document>> GetAllDocumentsAsync();
    }
}