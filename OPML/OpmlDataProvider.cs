using NewsReader.Interfaces;
using OPMLCore.NET;

namespace OPML;

public class OpmlDataProvider : IOPML
{
    public Opml LoadOpml(string opml)
    {
        // todo determine file or web
        try
        {
            return new Opml(opml);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return null;
        }
    }
}