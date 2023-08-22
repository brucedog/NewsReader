namespace NewsReader.ViewModels;

public class DeleteConfirmationWindowModel : ViewModelBase
{
    public DeleteConfirmationWindowModel(object feed)
    {
        Feed = (string)feed;
    }

    public string? Feed { get; set; }

    public void Delete(object content)
    {
        
    }
}