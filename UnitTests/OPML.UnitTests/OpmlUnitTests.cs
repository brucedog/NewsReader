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
            var loadedOpml = opml.LoadOpml(file2load);

            Assert.NotNull(loadedOpml);
            Assert.NotNull(loadedOpml.Body);
            Assert.True(loadedOpml.Body.Outlines.Count > 0);
        }
    }
}