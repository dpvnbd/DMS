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
using DMS.Application.DTOs.Users;

namespace DMS.Application.Documents
{
  public class AppDocumentService : IAppDocumentService
  {
    private readonly IRepository<Document> docRepo;
    private readonly IRepository<AppUser> userRepo;
    private readonly IMapper mapper;

    public AppDocumentService(IRepository<Document> docRepo, IRepository<AppUser> userRepo, IMapper mapper)
    {
      this.docRepo = docRepo;
      this.userRepo = userRepo;
      this.mapper = mapper;
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

      dto.History = document.History.Select(e =>
      {
        var entryDto = mapper.Map<HistoryEntryDto>(e);
        var user = userRepo.GetById(e.UserId).Result;
        entryDto.User = mapper.Map<UserSummaryDto>(user);
        if (e.UserActingOnBehalfId.HasValue)
        {
          var actingUser = userRepo.GetById(e.UserActingOnBehalfId.Value).Result;
          entryDto.UserActingOnBehalf = mapper.Map<UserSummaryDto>(actingUser);
        }
        return entryDto;
      });
      
      if (requestingUserId == -1)
      {
        return dto;
      }

      var author = await userRepo.GetById(document.AuthorId);
      dto.Author = mapper.Map<UserSummaryDto>(author);

      if (document.CreatorOnBehalfOfAuthorId.HasValue)
      {
        var createorOnBehalf = await userRepo.GetById(document.CreatorOnBehalfOfAuthorId.Value);
        dto.CreatorOnBehalfOfAuthor = mapper.Map<UserSummaryDto>(createorOnBehalf);
      }

      var requestingUser = await userRepo.GetById(requestingUserId);
      dto.AvailableStatusChanges = document.AvailableStatusChanges(requestingUser);
      dto.CanEdit = document.CanEdit(requestingUser);
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

    public async Task<IEnumerable<DocumentSummaryDto>> FindDocuments(Func<Document, bool> predicate, int requestingUserId = -1)
    {
      var documents = docRepo.GetAll().Where(predicate);


      return documents.Select(d =>
      {
        var summary = mapper.Map<DocumentSummaryDto>(d);
        if (requestingUserId != -1)
        {
          var requestingUser = userRepo.GetById(requestingUserId).Result;
          if (requestingUser != null)
          {
            summary.CanEdit = d.CanEdit(requestingUser);
          }
        }

        var author = userRepo.GetById(d.AuthorId).Result;
        summary.Author = mapper.Map<UserSummaryDto>(author);
        return summary;
      });
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


  }
}
