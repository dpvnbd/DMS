using DMS.Domain.Entities;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.TagHelpers
{
  public class DocumentStatusBadgeTagHelper : TagHelper
  {
    public DocumentStatus Status { get; set; }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      output.TagName = "span";
      string badgeColor = "badge-secondary";

      output.Content.SetHtmlContent(Status.ToString());

      switch (Status)
      {
        case DocumentStatus.Created:
          badgeColor = "badge-secondary";
          break;
        case DocumentStatus.Submitted:
          badgeColor = "badge-info";
          break;
        case DocumentStatus.Rejected:
          badgeColor = "badge-warning";
          break;
        case DocumentStatus.Approved:
          badgeColor = "badge-primary";
          break;
        case DocumentStatus.Accepted:
          badgeColor = "badge-success";
          break;
        case DocumentStatus.Declined:
          badgeColor = "badge-danger";
          break;        
      }

      output.Attributes.Add("class", "badge " + badgeColor);

      output.TagMode = TagMode.StartTagAndEndTag;
    }
  }
}
