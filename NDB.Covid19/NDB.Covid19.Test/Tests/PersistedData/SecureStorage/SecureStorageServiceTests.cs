using FluentAssertions;
using NDB.Covid19.PersistedData.SecureStorage;
using NDB.Covid19.Test.Mocks;
using Xunit;

namespace NDB.Covid19.Test.Tests.PersistedData.SecureStorage
{
    public class SecureStorageServiceTests
    {
        [Fact]
        public void SaveValue_SetValueSecureStore_ValueSaved()
        {
            var secureStorageMock = new SecureStorageMock();
            var secureStorageService = new SecureStorageService();
            secureStorageService.SetSecureStorageInstance(secureStorageMock);

            var key = "testSaveValue_Key";
            var value = "testSaveValue_Value";
            secureStorageService.SaveValue(key, value);
            var savedValue = secureStorageMock.GetValue(key);
            bool isSameValue = value.Equals(savedValue);
            isSameValue.Should().BeTrue();
        }

        [Fact]
        public void GetValue_GetExistedValueWithCorrectKey_ReturnCorrectValue()
        {
            var secureStorageMock = new SecureStorageMock();
            var secureStorageService = new SecureStorageService();
            secureStorageService.SetSecureStorageInstance(secureStorageMock);
            var key = "testGetValue_Key";
            var value = "testGetValue_Value";
            secureStorageMock.SetValue(key, value);
            var existedValue = secureStorageService.GetValue(key);
            bool isSameValue = value.Equals(existedValue);
            isSameValue.Should().BeTrue();
        }

        [Fact]
        public void KeyExists_CheckExsitingForExistedKey_KeyExistedInStore()
        {
            var secureStorageMock = new SecureStorageMock();
            var secureStorageService = new SecureStorageService();
            secureStorageService.SetSecureStorageInstance(secureStorageMock);
            var key = "testKeyExists_Key";
            var value = "testKeyExists_Value";
            secureStorageMock.SetValue(key, value);
            var existed = secureStorageService.KeyExists(key);
            existed.Should().BeTrue();
        }

        [Fact]
        public void KeyExists_CheckExsitingForNotExistedKey_KeyNotExistedInStore()
        {
            var secureStorageMock = new SecureStorageMock();
            var secureStorageService = new SecureStorageService();
            secureStorageService.SetSecureStorageInstance(secureStorageMock);
            var key = "testKeyExists_Key";
            var value = "testKeyExists_Value";
            secureStorageMock.SetValue(key, value);
            var notExistedKey = "testKeyExists_NotExistedKey";
            var existed = secureStorageService.KeyExists(notExistedKey);
            existed.Should().BeFalse();
        }

        [Fact]
        public void Delete_DeleteExistedKeyValuePair_KeyValuePairDeleted()
        {
            var secureStorageMock = new SecureStorageMock();
            var secureStorageService = new SecureStorageService();
            secureStorageService.SetSecureStorageInstance(secureStorageMock);
            var key = "testDelete_Key";
            var value = "testDelete_Value";
            secureStorageMock.SetValue(key, value);
            var added = secureStorageMock.HasKey(key);
            secureStorageService.Delete(key);
            var deleted = !secureStorageMock.HasKey(key);
            (added && deleted).Should().BeTrue();
        }
    }
}
