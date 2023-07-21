using OPMLCore.NET;

namespace NewsReader.Interfaces
{
    public interface IOPML
    {
        Opml LoadOpml(string opml);
    }
}