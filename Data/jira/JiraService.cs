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
    try
    {
      var cancellationToken = new CancellationTokenSource(2500);

      var issue = await _jira.Issues.GetIssueAsync(taskId, cancellationToken.Token);
      return issue != null ? issue.Summary : string.Empty;
    } catch (Exception ex)
    {
      throw new Exception($"Error getting task name for {taskId}", ex);
    }
  }

  public async Task<IEnumerable<string>> GetTaskServices(string taskId)
  {
    try
    {
      var issue = await _jira.Issues.GetIssueAsync(taskId);
      var remoteLinks = await issue.GetRemoteLinksAsync();
      return remoteLinks.Select(x => x.RemoteUrl.Split("/-/")[0]).Distinct();
    } catch (Exception ex)
    {
      throw new Exception($"Error getting task services for {taskId}", ex);
    }
  }
}
