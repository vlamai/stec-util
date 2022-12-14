namespace stec_util.Data;

public class PageSate
{
  private DateTime? _startLoadTime;
  private string _message = string.Empty;
  public bool Loading { get; private set; }

  public string Message
  {
    get => _message;
    private set
    {
      _message = value;
      // Console.WriteLine($"{DateTime.UtcNow.ToLongTimeString()} {value}");
    }
  }

  public PageSate()
  {
    StartLoading();
  }

  public void StartLoading()
  {
    Loading = true;
    Message = "Loading...";
    _startLoadTime = DateTime.UtcNow;
  }

  public void StopLoading()
  {
    Loading = false;
    if (_startLoadTime != null)
    {
      var loadTime = DateTime.UtcNow - _startLoadTime;
      if (loadTime.Value.TotalSeconds < 60)
      {
        Message = $"Loaded in {loadTime.Value.TotalSeconds:F1} seconds";
      } else
      {
        Message = $"Loaded in {loadTime.Value.TotalMinutes:F2} minutes";
      }

    }
    else
    {
      Message = "Done";
    }
  }

  public void SetMessage(string message)
  {
    Message = message;
  }
}
