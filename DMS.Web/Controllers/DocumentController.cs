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
    private readonly IAppDocumentService documentService;
    private readonly IMapper mapper;

    public DocumentController(UserManager<AppIdentityUser> userManager, IAppDocumentService documentService, IMapper mapper)
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

    [HttpGet]
    public async Task<IActionResult> Index()
    {
      var documents = documentService.FindDocuments(d => true);
      if (documents == null)
      {
        return NotFound();
      }
      return View(documents);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
      var userId = await GetDomainUserId();
      var document = await documentService.GetFullDocument(id, userId);
      if (document == null)
      {
        return NotFound();
      }
      ViewData["Document"] = document;
      return View();
    }

    [HttpGet]
    public IActionResult Create()
    {
      ViewData["Title"] = "Create document";
      ViewData["Action"] = "Create";
      return View("Edit", new DocumentEditingViewModel());
    }


    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
      ViewData["Title"] = "Edit document";
      ViewData["Action"] = "Edit";
      var documentDto = await documentService.GetDocumentContents(id);
      var document = mapper.Map<DocumentEditingViewModel>(documentDto);
      if(document == null)
      {
        return NotFound();
      }

      return View("Edit", document);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEdit(DocumentEditingViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var userId = await GetDomainUserId();

      var documentDto = mapper.Map<DocumentContentsDto>(model);
      documentDto.AuthorId = userId;
      int id;
      if (model.Id != 0)
      {
        id = await documentService.EditDocument(documentDto);
      }
      else
      {
        id = await documentService.CreateDocument(documentDto);
      }

      return RedirectToAction("Details", "Document", new { id });
    }

    /// <summary>
    /// It seems that it's impossible to pass different parameters to the 
    /// same action from a razor view form, so instead different actions are
    /// called from the view and they are relayed here with corresponding status 
    /// </summary>    
    private async Task<IActionResult> ChangeStatus(DocumentStatus status, SubmitStatusChangeViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View("Details", model);
      }

      var userId = await GetDomainUserId();

      var success = await documentService.ChangeStatus(model.DocumentId, userId, status, model.Message);

      if (!success)
      {
        ModelState.AddModelError(string.Empty, "An error has occured while changing document status");
      }

      return RedirectToAction("Details", "Document", new { id = model.DocumentId });
    }

    [HttpPost]
    public async Task<IActionResult> Submit(SubmitStatusChangeViewModel model)
    {
      return await ChangeStatus(DocumentStatus.Submitted, model);
    }

    [HttpPost]
    public async Task<IActionResult> Reject(SubmitStatusChangeViewModel model)
    {
      return await ChangeStatus(DocumentStatus.Rejected, model);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(SubmitStatusChangeViewModel model)
    {
      return await ChangeStatus(DocumentStatus.Approved, model);
    }

    [HttpPost]
    public async Task<IActionResult> Decline(SubmitStatusChangeViewModel model)
    {
      return await ChangeStatus(DocumentStatus.Declined, model);
    }

    [HttpPost]
    public async Task<IActionResult> Accept(SubmitStatusChangeViewModel model)
    {
      return await ChangeStatus(DocumentStatus.Accepted, model);
    }
  }
}