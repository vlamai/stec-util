using stec_util.Data.jira.Dto;

namespace stec_util.Data.jira;

public interface IJiraService
{
  public Task<string> GetTaskName(string taskId);
  
  public Task<IEnumerable<string>> GetTaskServices(string taskId);
  
  public Task<IEnumerable<IssueDto>> GetReleaseIssues(string fixVersion);
  
  public Task<IEnumerable<string>> GetReleaseVersions();
}
