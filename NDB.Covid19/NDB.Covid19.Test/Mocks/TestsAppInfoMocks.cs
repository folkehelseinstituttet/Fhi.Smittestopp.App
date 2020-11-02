using System;
using NDB.Covid19.Interfaces;
using Xamarin.Essentials;

namespace NDB.Covid19.Test.Mocks
{
    public class TestsAppInfoMocks: IAppInfo
    {
        public void ShowSettingsUI()
        {
            throw new NotImplementedException();
        }

        public string PackageName { get; } = "";
        public string Name { get; } = "";
        public string VersionString { get; } = "";
        public Version Version { get; } = Version.Parse("1.0.0");
        public string BuildString { get; } = "";
        public AppTheme RequestedTheme { get; } = AppTheme.Unspecified;
    }
}