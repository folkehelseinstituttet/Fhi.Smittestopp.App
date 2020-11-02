namespace NDB.Covid19.ViewModels
{
    public class DialogViewModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string OkBtnTxt { get; set; } = null;

        /// <summary>
        /// If this is null it means that there should be no CancelBtn at all.
        /// </summary>
        public string CancelbtnTxt { get; set; } = null;
    }
}
