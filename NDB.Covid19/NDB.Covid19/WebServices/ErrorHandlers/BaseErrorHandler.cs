using CommonServiceLocator;
using I18NPortable;

namespace NDB.Covid19.WebServices.ErrorHandlers
{
    public class BaseErrorHandler
    {
        public IDialogService DialogServiceInstance => ServiceLocator.Current.GetInstance<IDialogService>();
        public virtual string ErrorMessageTitle => "BASE_ERROR_TITLE".Translate();
        public virtual string ErrorMessage => "BASE_ERROR_MESSAGE".Translate();
        public virtual string OkBtnText => "ERROR_OK_BTN".Translate();

        public void ShowErrorToUser()
        {
            DialogServiceInstance.ShowMessageDialog(ErrorMessageTitle, ErrorMessage, OkBtnText);
        }
    }
}
