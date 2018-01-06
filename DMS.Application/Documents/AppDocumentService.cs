using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DMS.Application.DTOs;
using DMS.Application.DTOs.Documents;
using DMS.Domain.Abstract;
using DMS.Domain.Entities;

namespace DMS.Application.Documents
{
  public class AppDocumentService : IAppDocumentService
  {
    private readonly IDocumentRepository docRepo;
    private readonly IRepository<AppUser> userRepo;
    private readonly IMapper mapper;

    public AppDocumentService(IDocumentRepository docRepo, IRepository<AppUser> userRepo, IMapper mapper)
    {
      this.docRepo = docRepo;
      this.userRepo = userRepo;
      this.mapper = mapper;
    }

    public async Task<int> CreateDocument(DocumentContentsDto d)
    {
      var author = await userRepo.GetById(d.AuthorId);
      var document = new Document(d.Title, d.Body, author);
      await docRepo.Create(document);
      return document.Id;
    }

    public async Task<DocumentWithHistoryDto> GetDocument(int documentId, int userId)
    {
      var document = await docRepo.GetById(documentId);
      var user = await userRepo.GetById(userId);
      var dto = mapper.Map<DocumentWithHistoryDto>(document);
      dto.AvailableStatusChanges = document?.AvailableStatusChanges(user);
      return dto;
    }

    public async Task<bool> ChangeStatus(int documentId, int appUserId, DocumentStatus status, string message)
    {      
      try
      {
        var document = await docRepo.GetById(documentId);
        var user = await userRepo.GetById(appUserId);

        document.ChangeStatus(user, status, message);
        await docRepo.Update(document);
      }
      catch (Exception e)
      {
        return false;
      }
      return true;
    }
  }
}
