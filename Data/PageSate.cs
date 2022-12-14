namespace stec_util.Data;

public class PageSate
{
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
  }
  
  public void StopLoading()
  {
    Loading = false;
    Message = "Done";
  }
  
  public void SetMessage(string message)
  {
    Message = message;
  }
}
