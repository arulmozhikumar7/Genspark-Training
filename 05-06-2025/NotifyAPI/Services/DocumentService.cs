using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NotifyAPI.Models;
using NotifyAPI.Interfaces;
using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;

public class DocumentService
{
    private readonly IDocumentRepository _documentRepository;
    private readonly string _documentsPath;

    private readonly int _maxFileSizeInBytes = 10 * 1024 * 1024; // 10 MB max size

    public DocumentService(IWebHostEnvironment env, IDocumentRepository documentRepository)
    {
        // Fallback if WebRootPath is null
        var rootPath = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        _documentsPath = Path.Combine(rootPath, "documents");

        if (!Directory.Exists(_documentsPath))
        {
            Directory.CreateDirectory(_documentsPath);
        }

        _documentRepository = documentRepository;
    }

    public async Task<Document> UploadDocumentAsync(IFormFile file, string uploadedBy)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty");

        if (file.Length > _maxFileSizeInBytes)
            throw new ArgumentException("File size exceeds the 10MB limit");

        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var filePath = Path.Combine(_documentsPath, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var document = new Document
        {
            FileName = file.FileName,
            FilePath = $"/documents/{uniqueFileName}",
            UploadedAt = DateTime.UtcNow,
            UploadedBy = uploadedBy
        };

        return await _documentRepository.AddDocumentAsync(document);
    }

    public async Task<Document?> GetDocumentAsync(int id)
    {
        return await _documentRepository.GetDocumentByIdAsync(id);
    }

    public async Task<IEnumerable<Document>> GetDocumentsAsync()
    {
        return await _documentRepository.GetAllDocumentsAsync();
    }
}
