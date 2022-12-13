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
      // fixVersion = "3.32.0";
      var result = new List<IssueDto>();

      var jql = $"project = GPM AND fixVersion = {fixVersion} AND type not in (subTaskIssueTypes())";
      var q = new IssueSearchOptions(jql)
      {
        MaxIssuesPerRequest = 1000, FetchBasicFields = true,
      };

      var issues = await _jira.Issues.GetIssuesFromJqlAsync(q, token: cancellationToken.Token);
      foreach (var issue in issues)
      {
        var remoteLinks = await issue.GetRemoteLinksAsync(cancellationToken.Token);
        var services = remoteLinks.Select(x => x.RemoteUrl.Split("/-/")[0]).ToList();
        var subTasks = await issue.GetSubTasksAsync();
        foreach (var subTask in subTasks)
        {
          var subRemoteLinks = await subTask.GetRemoteLinksAsync();
          var subServices = subRemoteLinks.Select(x => x.RemoteUrl.Split("/-/")[0]);
          services.AddRange(subServices);
        }

        var allServices = services.Where(x => x.StartsWith("https://gitlab"))
          .Select(x => x.Split("/").Last())
          .ToList()
          .Distinct();

        result.Add(new IssueDto(issue, allServices));
      }

      return result.DistinctBy(x => x.Key);
    } catch (Exception ex)
    {
      Console.WriteLine(ex);
      throw;
    }
  }

  public async Task<IEnumerable<string>> GetReleaseVersions()
  {
    try
    {
      var cancellationToken = new CancellationTokenSource(180000);
      var project = await _jira.Projects.GetProjectAsync("GPM", cancellationToken.Token);
      var versions = await project.GetVersionsAsync(cancellationToken.Token);
      return versions.Where(x => x.IsReleased == false && x.IsArchived == false).Select(x => x.Name)
        .OrderBy(q => q);
    } catch (Exception ex)
    {
      throw new Exception("Error getting release versions", ex);
    }
  }
}
