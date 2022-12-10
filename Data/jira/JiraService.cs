using Atlassian.Jira;

namespace stec_util.Data.jira;

public class JiraService : IJiraService
{
  private readonly Jira _jira;

  public JiraService(JiraConfig config)
  {
    _jira = Jira.CreateRestClient(config.Host, config.Username, config.Password);
  }

  public async Task<string> GetTaskName(string taskId)
  {
    var issue = await _jira.Issues.GetIssueAsync(taskId);
    return issue.Summary;
  }
}
