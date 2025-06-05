using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotifyAPI.Data; 
using NotifyAPI.Models;
using NotifyAPI.Interfaces;
public class DocumentRepository : IDocumentRepository
{
    private readonly AppDbContext _context;

    public DocumentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Document> AddDocumentAsync(Document document)
    {
        _context.Documents.Add(document);
        await _context.SaveChangesAsync();
        return document;
    }

    public async Task<Document?> GetDocumentByIdAsync(int id)
    {
        return await _context.Documents.FindAsync(id);
    }

    public async Task<IEnumerable<Document>> GetAllDocumentsAsync()
    {
        return await _context.Documents.ToListAsync();
    }
}
