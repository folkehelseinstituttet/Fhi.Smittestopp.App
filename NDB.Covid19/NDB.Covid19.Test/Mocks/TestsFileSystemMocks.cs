using NDB.Covid19.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace NDB.Covid19.Test.Mocks
{
    public class TestsFileSystemMocks : IFileSystem
    {
        public Task<Stream> OpenAppPackageFileAsync(string filename)
        {
            throw new System.NotImplementedException();
        }

        public string CacheDirectory => "/tmp/";
        public string AppDataDirectory => "/tmp/";
    }
}