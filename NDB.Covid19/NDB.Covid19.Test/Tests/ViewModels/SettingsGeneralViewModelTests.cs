using NDB.Covid19.ViewModels;
using Xunit;

namespace NDB.Covid19.Test.Tests.ViewModels
{
    public class SettingsGeneralViewModelTests
    {
        public SettingsGeneralViewModelTests()
        {
            DependencyInjectionConfig.Init();
        }

        [Fact]
        public void GetStoredCheckedState_Default_IsTrue()
        {
            SettingsGeneralViewModel vm = new SettingsGeneralViewModel();
            bool storedCheckedState = vm.GetStoredCheckedState();
            Assert.True(storedCheckedState);
        }

        [Fact]
        public void OnCheckedChange_SetsValue_True()
        {
            SettingsGeneralViewModel vm = new SettingsGeneralViewModel();
            vm.OnCheckedChange(true);
            bool storedCheckedState = vm.GetStoredCheckedState();
            Assert.True(storedCheckedState);
        }

        [Fact]
        public void OnCheckedChange_SetsValue_False()
        {
            SettingsGeneralViewModel vm = new SettingsGeneralViewModel();
            vm.OnCheckedChange(false);
            bool storedCheckedState = vm.GetStoredCheckedState();
            Assert.False(storedCheckedState);
        }
    }
}
