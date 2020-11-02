namespace NDB.Covid19.WebServices.ErrorHandlers
{
    public interface IDialogService
    {
        void ShowMessageDialog(
            string title,
            string message,
            string okBtn,
            PlatformDialogServiceArguments platformArguments = null);
    }


    /// <summary>
    /// In some cases it is hard to detect a ViewController on the hierachy which we have to pass to it manually
    /// </summary>
    public class PlatformDialogServiceArguments
    {
        /// <summary>
        /// ViewController or Activity
        /// </summary>
        public object Context { get; set; }
    }
}
