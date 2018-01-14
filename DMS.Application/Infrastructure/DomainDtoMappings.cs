using AutoMapper;
using DMS.Application.DTOs;
using DMS.Application.DTOs.Documents;
using DMS.Application.DTOs.Users;
using DMS.Domain.Entities;
using DMS.Infrastructure.Data.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Application.Infrastructure
{
  public class DomainDtoMappings : Profile
  {
    public DomainDtoMappings()
    {
      CreateMap<AppUser, UserSummaryDto>();
      CreateMap<DocumentHistoryEntry, HistoryEntryDto>();
      CreateMap<Document, DocumentWithHistoryDto>();
      CreateMap<Document, DocumentSummaryDto>();

      CreateMap<Document, DocumentContentsDto>().ForMember(dto => dto.AuthorId, m => m.MapFrom(d => d.Author.Id));

      CreateMap<AppUser, UserFullDto>();

      CreateMap<AppIdentityUser, UserIdentityDto>();

    }
  }
}
