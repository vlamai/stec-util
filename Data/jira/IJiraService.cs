namespace stec_util.Data.jira;

public interface IJiraService
{
  public Task<string> GetTaskName(string taskId);
}
