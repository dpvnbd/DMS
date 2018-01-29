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
using DMS.Domain.Abstract.Services;

namespace DMS.Application.Documents
{
  public class AppDocumentService : IAppDocumentService
  {
    private readonly IRepository<Document> docRepo;
    private readonly IRepository<AppUser> userRepo;
    private readonly IMapper mapper;
    private readonly IDocumentFilteringService filteringService;

    public AppDocumentService(IRepository<Document> docRepo, IRepository<AppUser> userRepo, IDocumentFilteringService filteringService,
      IMapper mapper)
    {
      this.docRepo = docRepo;
      this.userRepo = userRepo;
      this.mapper = mapper;
      this.filteringService = filteringService;
    }
    
    public async Task<int> CreateDocument(DocumentContentsDto d, int authorId)
    {
      return await CreateDocument(d, authorId, -1);      
    }

    public async Task<int> CreateDocument(DocumentContentsDto d, int authorId, int userActingOnBehalfId = -1)
    {
      var author = await userRepo.GetById(authorId);

      if (author == null)
      {
        throw new ArgumentException("Document Author not found", nameof(d));
      }

      Document document;

      if (userActingOnBehalfId != -1)
      {
        var actingUser = await userRepo.GetById(userActingOnBehalfId);
        if (actingUser == null)
        {
          throw new ArgumentException("Acting User not found", nameof(userActingOnBehalfId));
        }
        document = new Document(d.Title, d.Body, author, d.Message, actingUser);
      }
      else
      {
        document = new Document(d.Title, d.Body, author, d.Message);
      }

      await docRepo.Create(document);
      return document.Id;
    }

    public async Task<bool> EditDocument(DocumentContentsDto d, int documentId, int editorId)
    {
      try
      {
        var editor = await userRepo.GetById(editorId);
        var document = await docRepo.GetById(documentId);
        document.Edit(editor, d.Title, d.Body, d.Message);
        await docRepo.Update(document);
      }
      catch (Exception)
      {
        return false;
      }
      return true;
    }

    public async Task<DocumentWithHistoryDto> GetFullDocument(int documentId, int requestingUserId = -1)
    {
      var document = await docRepo.GetById(documentId);
      if (document == null)
      {
        return null;
      }

      var dto = mapper.Map<DocumentWithHistoryDto>(document);
      if (requestingUserId == -1)
      {
        return dto;
      }

      var user = await userRepo.GetById(requestingUserId);
      dto.AvailableStatusChanges = document.AvailableStatusChanges(user);
      dto.CanEdit = document.CanEdit(user);
      return dto;
    }

    public async Task<DocumentContentsDto> GetDocumentContents(int id)
    {
      var document = await docRepo.GetById(id);
      var dto = mapper.Map<DocumentContentsDto>(document);
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

    public async Task<IEnumerable<DocumentSummaryDto>> FindDocuments(int authorId = -1, DocumentStatus? status = null,
      string searchString = null, int requestingUserId = -1, bool canBeReviewed = false,
      DateTime? fromDate = null, DateTime? toDate = null)
    {
      AppUser author = null;
      AppUser requestingUser = null;

      if(authorId != -1)
      {
        author = await userRepo.GetById(authorId);
        if(author == null)
        {
          return Array.Empty<DocumentSummaryDto>();
        }
      }

      if (requestingUserId != -1)
      {
        requestingUser = await userRepo.GetById(requestingUserId);
        if (requestingUser == null)
        {
          return Array.Empty<DocumentSummaryDto>();
        }
      }

      var documents = filteringService.Filter(docRepo.GetAll(),
        author, status, searchString, canBeReviewed ? requestingUser : null, fromDate, toDate);

      if(requestingUser != null)
      {
        var dtos = new List<DocumentSummaryDto>(documents.Count());
        foreach(var d in documents)
        {
          var summary = mapper.Map<DocumentSummaryDto>(d);
          summary.CanEdit = d.CanEdit(requestingUser);
          dtos.Add(summary);
        }
        return dtos;
      }

      return mapper.Map<IEnumerable<DocumentSummaryDto>>(documents);
    }

    public async Task<bool> Delete(int documentId)
    {
      var document = await docRepo.GetById(documentId);
      if (document == null)
      {
        return false;
      }
      try
      {
        await docRepo.Delete(documentId);
      }
      catch (Exception)
      {
        return false;
      }
      return true;
    }

    public async Task<DocumentSummaryDto> GetDocumentSummary(int id)
    {
      var document = await docRepo.GetById(id);
      return mapper.Map<DocumentSummaryDto>(document);
    }
  }
}
