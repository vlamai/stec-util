using Atlassian.Jira;
using stec_util.Data.jira.Dto;

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
      var cancellationToken = new CancellationTokenSource(180000);

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
      var cancellationToken = new CancellationTokenSource(180000);
      var issue = await _jira.Issues.GetIssueAsync(taskId, cancellationToken.Token);
      var remoteLinks = await issue.GetRemoteLinksAsync(cancellationToken.Token);
      return remoteLinks.Select(x => x.RemoteUrl.Split("/-/")[0]).Distinct();
    } catch (Exception ex)
    {
      throw new Exception($"Error getting task services for {taskId}", ex);
    }
  }

  public async Task<IEnumerable<IssueDto>> GetReleaseIssues(string fixVersion)
  {
    try
    {
      var cancellationToken = new CancellationTokenSource(180000);
      fixVersion = "3.33.0";
      var result = new List<IssueDto>();
      var issues = await _jira.Issues.GetIssuesFromJqlAsync($"project = GPM AND fixVersion = {fixVersion}", maxIssues: 10, token: cancellationToken.Token);
      foreach (var issue in issues)
      {
        var remoteLinks = await issue.GetRemoteLinksAsync(cancellationToken.Token);
        var services = remoteLinks.Select(x => x.RemoteUrl.Split("/-/")[0]).Distinct();
        result.Add(new IssueDto(issue,services));
        
        // var subTasks = await issue.GetSubTasksAsync();
        // foreach (var subTask in subTasks)
        // {
        //   var subRemoteLinks = await subTask.GetRemoteLinksAsync();
        //   var subServices = subRemoteLinks.Select(x => x.RemoteUrl.Split("/-/")[0]).Distinct();
        //   result.Add(new IssueDto(subTask,subServices,"---    "));
        // }
      }

      return result;
    } catch (Exception ex)
    {
      Console.WriteLine(ex);
      throw;
    }
  }
}
