using AutoMapper;
using DMS.Application.DTOs;
using DMS.Application.DTOs.Documents;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Application.Infrastructure
{
  public class DomainDtoMappings : Profile
  {
    public DomainDtoMappings()
    {
      CreateMap<AppUser, AppUserDto>();
      CreateMap<StatusChange, StatusChangeDto>();
      CreateMap<Document, DocumentWithHistoryDto>().ForMember(dto => dto.History, m => m.MapFrom(d => d.StatusChanges));
      CreateMap<Document, DocumentSummaryDto>().ForMember(dto => dto.AuthorId, m => m.MapFrom(d => d.Author.Id))
        .ForMember(dto => dto.AuthorFullName, m => m.MapFrom(d => d.Author.FullName));
    }
  }
}
