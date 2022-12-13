using Atlassian.Jira;

namespace stec_util.Data.jira.Dto;

public class IssueDto
{
    public IssueDto(Issue issue, IEnumerable<string> services, string? prefix = null)
    {
        Services = services;
        Key = issue.Key.Value;
        if (prefix != null)
        {
            Summary = prefix + issue.Summary;
        }else
        {
            Summary = issue.Summary;
        }
        Type = issue.Type.Name;
        Assignee = issue.AssigneeUser.DisplayName;
    }
    public string Key { get; set; }
    public string Type { get; set; }
    public string Summary { get; set; }
    public IEnumerable<string> Services { get; set; }
    public string Assignee { get; set; }

}
