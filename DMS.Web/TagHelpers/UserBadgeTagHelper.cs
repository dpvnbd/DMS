using DMS.Application.DTOs.Users;
using DMS.Domain.Entities;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.TagHelpers
{
  public class UserBadgeTagHelper : TagHelper
  {
    public UserSummaryDto User { get; set; }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      
      output.TagName = "span";
      if (User == null)
      {
        output.Content.SetHtmlContent("null");
        return;
      }
      string badgeColor = "";

      switch (User.Role)
      {
        case UserRole.Customer:
          badgeColor = "badge-secondary";
          break;
        case UserRole.Operator:
          badgeColor = "badge-info";
          break;
        case UserRole.Expert:
          badgeColor = "badge-primary";
          break;
      }

      output.Content.SetHtmlContent($"<span class = \"badge {badgeColor}\">{User.Role.ToString()}</span> {User.FullName}");

      output.TagMode = TagMode.StartTagAndEndTag;
    }
  }
}
