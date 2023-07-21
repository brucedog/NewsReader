using NewsReader.Interfaces;

namespace OPML.UnitTests
{
    public class OpmlUnitTests
    {
        [Fact]
        public void LoadOpmlUnitTest()
        {
            string file2load = Path.Combine(Directory.GetCurrentDirectory(), "subscriptions.opml");
            
            IOPML opml = new OpmlDataProvider();
            
            Assert.NotNull(opml.LoadOpml(file2load));
        }
    }
}