using Atlassian.Jira;

namespace stec_util.Data.jira.Dto;

public class IssueDto
{
    public IssueDto(Issue issue, IEnumerable<string> services, string? prefix = null)
    {
        Services = services;
        Key = issue.Key.Value;
        Summary = prefix != null ? prefix + issue.Summary : issue.Summary;
        Type = issue.Type.Name;
        Assignee = issue.AssigneeUser.DisplayName;
    }
    public string Key { get; }
    public string Type { get; }
    public string Summary { get; }
    public IEnumerable<string> Services { get; }
    public string Assignee { get; }

}
