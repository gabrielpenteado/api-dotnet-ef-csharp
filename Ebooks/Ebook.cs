namespace api_dotnet_ef_csharp.Ebooks;

public class Ebook
{
    public Guid Id { get; init; }
    public string Title { get; private set; }
    public bool Available { get; private set; }

    public Ebook(string title)
    {
        Title = title;
        Id = Guid.NewGuid();
        Available = true;
    }

    public void updateTitle(string title)
    {
        Title = title;
    }

    public void turnNotAvailable()
    {
        Available = false;
    }
}
