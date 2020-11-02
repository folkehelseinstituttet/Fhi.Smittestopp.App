using System.Linq;
using Moq;

namespace NDB.Covid19.Test.Tests.Utils
{
    internal static class MockExtensions
    {
        public static Mock<T> RegisterForJsonSerialization<T>(this Mock<T> mock) where T : class
        {
            JsonMockConverter.RegisterMock(
                mock,
                () => typeof(T).GetProperties().ToDictionary(p => p.Name, p => p.GetValue(mock.Object))
            );
            return mock;
        }
    }
}