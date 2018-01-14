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
    public DocumentStatus? Status { get; set; }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      output.TagName = "span";
      string badgeColor = "badge-secondary";

      if (Status.HasValue)
      {
        output.Content.SetHtmlContent(Status.ToString());

        switch (Status.Value)
        {
          case DocumentStatus.Created:
            badgeColor = "badge-secondary";
            break;
          case DocumentStatus.Rejected:
            badgeColor = "badge-warning";
            break;
          case DocumentStatus.Resubmitted:
            badgeColor = "badge-secondary";
            break;
          case DocumentStatus.Registered:
            badgeColor = "badge-primary";
            break;
          case DocumentStatus.Approved:
            badgeColor = "badge-success";
            break;
          case DocumentStatus.Canceled:
            badgeColor = "badge-danger";
            break;
          case DocumentStatus.Done:
            badgeColor = "badge-success";
            break;
        }
      }
      else
      {
        output.Content.SetHtmlContent("Edited");
      }

      output.Attributes.Add("class", "badge " + badgeColor);

      output.TagMode = TagMode.StartTagAndEndTag;
    }
  }
}
