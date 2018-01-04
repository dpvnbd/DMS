using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DMS.Application.DTOs.Documents;
using DMS.Domain.Abstract;
using DMS.Domain.Entities;

namespace DMS.Application.Documents
{
  public class DocumentService : IDocumentService
  {
    private readonly IRepository<Document> docRepo;
    private readonly IRepository<AppUser> userRepo;

    public DocumentService(IRepository<Document> docRepo, IRepository<AppUser> userRepo)
    {
      this.docRepo = docRepo;
      this.userRepo = userRepo;
    }

    public async Task<int> CreateDocument(DocumentContentsDto d)
    {
      var author = await userRepo.GetById(d.AuthorId);
      var document = new Document(d.Title, d.Body, author);
      await docRepo.Create(document);
      return document.Id;
    }
  }
}
