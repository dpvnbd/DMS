using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DMS.Application.Documents;
using DMS.Application.DTOs.Documents;
using DMS.Domain.Abstract;
using DMS.Domain.Entities;
using DMS.Infrastructure.Data.Identity;
using DMS.Web.Models.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Web.Controllers
{
  [Authorize]
  public class DocumentController : Controller
  {
    private readonly UserManager<AppIdentityUser> userManager;
    private readonly IDocumentService documentService;
    private readonly IMapper mapper;

    public DocumentController(UserManager<AppIdentityUser> userManager, IDocumentService documentService, IMapper mapper)
    {
      this.userManager = userManager;
      this.documentService = documentService;
      this.mapper = mapper;
    }

    private async Task<int> GetDomainUserId()
    {
      var identityUser = await userManager.GetUserAsync(HttpContext.User);
      return identityUser.AppUserId;
    }


    public IActionResult Index()
    {
      return View();
    }

    [HttpGet]
    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateDocumentViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var userId = await GetDomainUserId();

      var documentDto = mapper.Map<DocumentContentsDto>(model);
      documentDto.AuthorId = userId;
      var documentId = await documentService.CreateDocument(documentDto);

      return RedirectToAction("Index", "Home");
    }
  }
}