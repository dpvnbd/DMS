using AutoMapper;
using DMS.Application.DTOs;
using DMS.Application.DTOs.Documents;
using DMS.Application.DTOs.Users;
using DMS.Domain.Entities;
using DMS.Infrastructure.Data.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DMS.Application.Infrastructure
{
  public class DomainDtoMappings : Profile
  {
    public DomainDtoMappings()
    {
      CreateMap<AppUser, UserSummaryDto>();
      CreateMap<DocumentHistoryEntry, HistoryEntryDto>();
      CreateMap<Document, DocumentWithHistoryDto>().ForMember(d=>d.History, m => m.Ignore());

      CreateMap<Document, DocumentSummaryDto>();

      CreateMap<Document, DocumentContentsDto>();

      CreateMap<AppUser, UserFullDto>();

      CreateMap<AppIdentityUser, UserIdentityDto>();

    }
  }
}
