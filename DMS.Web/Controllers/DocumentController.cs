using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DMS.Application.Authentication;
using DMS.Application.Documents;
using DMS.Application.DTOs.Documents;
using DMS.Application.DTOs.Users;
using DMS.Application.Users;
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
    private readonly IAppDocumentService documentService;
    private readonly IAppUserService userService;
    private readonly IAuthService authService;
    private readonly IMapper mapper;

    public DocumentController(IAppDocumentService documentService, IAppUserService userService, IAuthService authService, IMapper mapper)
    {
      this.documentService = documentService;
      this.userService = userService;
      this.authService = authService;
      this.mapper = mapper;
    }

    //[HttpGet]
    //public async Task<IActionResult> Filter(DocumentsFilterViewModel model)
    //{
    //  return await Index(model.AuthorId, model.Status, model.SearchString);
    //}

    [HttpGet]
    public async Task<IActionResult> Index(int? authorId, DocumentStatus? status, string searchString, bool requireAttention)
    {
      var userId = await authService.GetUserIdByClaims(User);
      

      bool predicate(Document d)
      {
        if (authorId.HasValue && d.Author.Id != authorId.Value)
        {
          return false;
        }

        if (status.HasValue && d.CurrentStatus != status.Value)
        {
          return false;
        }

        if (!string.IsNullOrWhiteSpace(searchString))
        {
          if (!d.Title.Contains(searchString)
          && !d.Body.Contains(searchString))
          {
            return false;
          }
        }
        return true;
      }

      var documents = await documentService.FindDocuments(predicate, userId, requireAttention);
      if (documents == null)
      {
        return NotFound();
      }
      ViewData["documents"] = documents;

      var filterModel = new DocumentsFilterViewModel
      {
        SearchString = searchString,
        Status = status,
        RequireAttention = requireAttention
      };
      if (authorId.HasValue)
      {
        var author = await userService.GetUserSummary(authorId.Value, userId);
        ViewData["authorFilter"] = author;
      }

      return View("Index", filterModel);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
      var userId = await authService.GetUserIdByClaims(User);
      var document = await documentService.GetFullDocument(id, userId);
      if (document == null)
      {
        return NotFound();
      }
      ViewData["Document"] = document;
      return View();
    }

    [HttpGet]
    public async Task<IActionResult> Create(int onBehalfOfUserId = -1)
    {
      var model = new DocumentEditingViewModel();
      ViewData["Title"] = "Create document";
      ViewData["Action"] = "Create";
      if (onBehalfOfUserId != -1)
      {
        var user = await userService.GetUserSummary(onBehalfOfUserId);
        if (user == null)
        {
          return NotFound();
        }
        ViewData["OnBehalfUser"] = user;
        model.OnBehalfOfUserId = user.Id;
      }
      return View("Edit", model);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
      ViewData["Title"] = "Edit document";
      ViewData["Action"] = "Edit";
      var documentDto = await documentService.GetDocumentContents(id);
      var document = mapper.Map<DocumentEditingViewModel>(documentDto);
      if (document == null)
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
        return View("Edit", model);
      }

      var currentUserId = await authService.GetUserIdByClaims(User);

      var contents = mapper.Map<DocumentContentsDto>(model);


      if (model.Id != 0)
      {
        var success = await documentService.EditDocument(contents, model.Id, currentUserId);
        if (!success)
        {
          ModelState.AddModelError(string.Empty, "Unable to edit the document");
          return View("Edit", model);
        }
        return RedirectToAction("Details", "Document", new { model.Id });

      }
      else
      {
        int id;
        if (model.OnBehalfOfUserId.HasValue)
        {
          id = await documentService.CreateDocument(contents, model.OnBehalfOfUserId.Value, currentUserId);
        }
        else
        {
          id = await documentService.CreateDocument(contents, currentUserId);
        }
        return RedirectToAction("Details", "Document", new { id });
      }
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

      var userId = await authService.GetUserIdByClaims(User);

      var success = await documentService.ChangeStatus(model.DocumentId, userId, status, model.Message);

      if (!success)
      {
        ModelState.AddModelError(string.Empty, "An error has occured while changing document status");
      }

      return RedirectToAction("Details", "Document", new { id = model.DocumentId });
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
    public async Task<IActionResult> Cancel(SubmitStatusChangeViewModel model)
    {
      return await ChangeStatus(DocumentStatus.Canceled, model);
    }

    [HttpPost]
    public async Task<IActionResult> Register(SubmitStatusChangeViewModel model)
    {
      return await ChangeStatus(DocumentStatus.Registered, model);
    }

    [HttpPost]
    public async Task<IActionResult> Resubmit(SubmitStatusChangeViewModel model)
    {
      return await ChangeStatus(DocumentStatus.Resubmitted, model);
    }

    [HttpPost]
    public async Task<IActionResult> Done(SubmitStatusChangeViewModel model)
    {
      return await ChangeStatus(DocumentStatus.Done, model);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
      var documents = await documentService.FindDocuments(d => d.Id == id);
      var document = documents.FirstOrDefault();
      if (document == null)
      {
        return NotFound();
      }
      return View(document);
    }

    [ActionName("Delete")]
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteDocument(int id)
    {
      var success = await documentService.Delete(id);
      if (!success)
      {
        return RedirectToAction("Details", new { id })
;
      }
      return RedirectToAction("Index");
    }
  }
}