using AutoMapper;
using DMS.Application.DTOs.Authentication;
using DMS.Application.DTOs.Documents;
using DMS.Web.Models.Account;
using DMS.Web.Models.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Infrastructure
{
  public class ViewModelMappings : Profile
  {
    public ViewModelMappings()
    {
      CreateMap<RegisterViewModel, RegisterDto>();
      CreateMap<LoginViewModel, LoginDto>();
      CreateMap<CreateDocumentViewModel, DocumentContentsDto>();
    }
  }
}
