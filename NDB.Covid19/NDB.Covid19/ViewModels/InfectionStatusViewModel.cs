using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using I18NPortable;
using NDB.Covid19.Models.SQLite;
using NDB.Covid19.Utils;
using Xamarin.ExposureNotifications;

namespace NDB.Covid19.ViewModels
{
    public class InfectionStatusViewModel
    {
        public static string SMITTESPORING_FHI_LOGO_ACCESSIBILITY => "SMITTESPORING_FHI_LOGO_ACCESSIBILITY".Translate();
        public static string SMITTESPORING_APP_TITLE_ACCESSIBILITY => "SMITTESPORING_APP_TITLE_ACCESSIBILITY".Translate();
        public static string INFECTION_STATUS_ACTIVE_TEXT => "SMITTESPORING_ACTIVE_HEADER".Translate();
        public static string INFECTION_STATUS_INACTIVE_TEXT => "SMITTESPORING_INACTIVE_HEADER".Translate();
        public static string INFECTION_STATUS_ACTIVITY_STATUS_DESCRIPTION_TEXT => "SMITTESPORING_ACTIVE_DESCRIPTION".Translate();
        public static string SMITTESPORING_INACTIVE_DESCRIPTION => "SMITTESPORING_INACTIVE_DESCRIPTION".Translate();
        public static string INFECTION_STATUS_MESSAGE_HEADER_TEXT => "SMITTESPORING_MESSAGE_HEADER".Translate();
        public static string INFECTION_STATUS_MESSAGE_ACCESSIBILITY_TEXT => "SMITTESPORING_MESSAGE_HEADER_ACCESSIBILITY".Translate();
        public static string INFECTION_STATUS_MESSAGE_SUBHEADER_TEXT => "SMITTESPORING_MESSAGE_DESCRIPTION".Translate();
        public static string INFECTION_STATUS_NO_NEW_MESSAGE_SUBHEADER_TEXT => "SMITTESPORING_NO_NEW_MESSAGE_DESCRIPTION".Translate();
        public static string INFECTION_STATUS_REGISTRATION_HEADER_TEXT => "SMITTESPORING_REGISTER_HEADER".Translate();
        public static string INFECTION_STATUS_REGISTRATION_SUBHEADER_TEXT => "SMITTESPORING_REGISTER_DESCRIPTION".Translate();
        public static string INFECTION_STATUS_MENU_ACCESSIBILITY_TEXT => "MENU_TEXT".Translate();
        public static string INFECTION_STATUS_MENU_TEXT => "SMITTESPORING_MENU_TEXT".Translate();
        public static string INFECTION_STATUS_APP_TITLE_TEXT => "SMITTESPORING_APP_NAME".Translate();
        public static string INFECTION_STATUS_START_BUTTON_ACCESSIBILITY_TEXT => "SMITTESPORING_START_BUTTON_ACCESSIBILITY".Translate();
        public static string INFECTION_STATUS_STOP_BUTTON_ACCESSIBILITY_TEXT => "SMITTESPORING_STOP_BUTTON_ACCESSIBILITY".Translate();
        public static string INFECTION_STATUS_START_BUTTON_TEXT => "SMITTESPORING_START_BUTTON_TEXT".Translate();
        public static string INFECTION_STATUS_STOP_BUTTON_TEXT => "SMITTESPORING_STOP_BUTTON_TEXT".Translate();

        public async Task<string> StatusTxt(bool hasLocation = true) =>
            await IsRunning(hasLocation) 
                ? INFECTION_STATUS_ACTIVE_TEXT 
                : INFECTION_STATUS_INACTIVE_TEXT;

        public async Task<string> StatusTxtDescription(bool hasLocation = true) => 
            await IsRunning(hasLocation) 
                ? INFECTION_STATUS_ACTIVITY_STATUS_DESCRIPTION_TEXT 
                : SMITTESPORING_INACTIVE_DESCRIPTION;

        private DateTime _latestMessageDateTime = DateTime.Today;
        public bool ShowNewMessageIcon { get; private set; }
        public EventHandler NewMessagesIconVisibilityChanged { get; set; }
        public bool IsAppRestricted { get; set; }
        
        public string NewMessageSubheaderTxt =>
            ShowNewMessageIcon
                ? $"{INFECTION_STATUS_MESSAGE_SUBHEADER_TEXT} {DateUtils.GetDateFromDateTime(_latestMessageDateTime, "d. MMMMM")}"
                : INFECTION_STATUS_NO_NEW_MESSAGE_SUBHEADER_TEXT;

        public string NewMessageAccessibilityText =>
            INFECTION_STATUS_MESSAGE_ACCESSIBILITY_TEXT + ". " + NewMessageSubheaderTxt;

        public string NewRegistrationAccessibilityText =>
            INFECTION_STATUS_REGISTRATION_HEADER_TEXT + ". " + INFECTION_STATUS_REGISTRATION_SUBHEADER_TEXT;

        /// <summary>
        /// Show when trying to stop the scanner.
        /// </summary>
        public DialogViewModel OffDialogViewModel => new DialogViewModel
        {
            Title = "SMITTESPORING_TOGGLE_OFF_HEADER".Translate(),
            Body = "SMITTESPORING_TOGGLE_OFF_DESCRIPTION".Translate(),
            OkBtnTxt = "SMITTESPORING_TOGGLE_OFF_CONFIRM".Translate(),
            CancelbtnTxt = "SMITTESPORING_TOGGLE_OFF_CANCEL".Translate()
        };

        /// <summary>
        /// Show when trying to start the scanner.
        /// </summary>
        public DialogViewModel OnDialogViewModel => new DialogViewModel
        {
            Title = "SMITTESPORING_TOGGLE_ON_HEADER".Translate(),
            Body = "SMITTESPORING_TOGGLE_ON_DESCRIPTION".Translate(),
            OkBtnTxt = "SMITTESPORING_TOGGLE_ON_CONFIRM".Translate(),
            CancelbtnTxt = "SMITTESPORING_TOGGLE_ON_CANCEL".Translate()
        };

        /// <summary>
        /// Show this on iOS when user has denied access to EN-api but tries to start scanning any way.
        /// </summary>
        public DialogViewModel PermissionViewModel => new DialogViewModel
        {
            Title = "SMITTESPORING_EN_PERMISSION_DENIED_HEADER".Translate(),
            Body = "SMITTESPORING_EN_PERMISSION_DENIED_BODY".Translate(),
            OkBtnTxt = "SMITTESPORING_EN_PERMISSION_DENIED_OK_BTN".Translate()
        };

        /// <summary>
        /// Show this when user tries to report ill while the scanner is not running.
        /// </summary>
        public DialogViewModel ReportingIllDialogViewModel => new DialogViewModel
        {
            Title = "SMITTESPORING_REPORTING_ILL_DIALOG_HEADER".Translate(),
            Body = "SMITTESPORING_REPORTING_ILL_DIALOG_BODY".Translate(),
            OkBtnTxt = "SMITTESPORING_REPORTING_ILL_DIALOG_OK_BTN".Translate()
        };

        public InfectionStatusViewModel()
        {
            //This subscribe is intentional placed here, as the InfectionStatus is not supposed to be garbed collected
            SubscribeMessages();
        }

        public async Task<bool> IsRunning()
        {
            if (IsAppRestricted)
            {
                return false;
            }
            try
            {
                return (await ExposureNotification.GetStatusAsync() == Status.Active);
            }
            catch (Exception e)
            {
                if (!e.HandleExposureNotificationException(nameof(InfectionStatusViewModel), nameof(IsRunning)))
                {
                    throw e;
                }
                return false;
            }
        }

        public async Task<bool> IsRunning(bool hasLocation) => 
            await IsRunning() && hasLocation;

        public async Task<bool> IsEnabled()
        {
            try
            {
                return await ExposureNotification.IsEnabledAsync();
            }
            catch (Exception e)
            {
                if (!e.HandleExposureNotificationException(nameof(InfectionStatusViewModel), nameof(IsEnabled)))
                {
                    throw e;
                }
                return false;
            }
        }

        public async Task<bool> StartENService()
        {
            if (IsAppRestricted)
            {
                return false;
            }
            try
            {
                await ExposureNotification.StartAsync();
            }
            catch (Exception e)
            {
                if (!e.HandleExposureNotificationException(nameof(InfectionStatusViewModel), nameof(StartENService)))
                {
                    throw e;
                }
            }

            return await IsRunning();
        }

        public async Task<bool> StopENService()
        {
            if (IsAppRestricted)
            {
                return false;
            }
            try
            {
                await ExposureNotification.StopAsync();
            }
            catch (Exception e)
            {
                if (!e.HandleExposureNotificationException(nameof(InfectionStatusViewModel), nameof(StopENService)))
                {
                    throw e;
                }
            }
            return await IsRunning();
        }

        public async void CheckIfAppIsRestricted(Action action = null)
        {
            try
            {
                if (await IsEnabled())
                {
                    if (await IsRunning())
                    {
                        await ExposureNotification.StartAsync();
                    }
                    else
                    {
                        await ExposureNotification.StopAsync();
                    }
                }
                IsAppRestricted = false;
            }
            catch (Exception)
            {
                IsAppRestricted = true;
            }
            action?.Invoke();
        }
        
        async Task NewMessagesFetched()
        {
            List<MessageItemViewModel> orderedMessages =
                MessageUtils.ToMessageItemViewModelList((await MessageUtils.GetMessages())
                    .OrderByDescending(message => message.TimeStamp)
                    .ToList());

            if (orderedMessages.Any())
            {
                _latestMessageDateTime = orderedMessages[0].TimeStamp;
            }

            List<MessageSQLiteModel> unreadMessagesResult = await MessageUtils.GetAllUnreadMessages();
            ShowNewMessageIcon = unreadMessagesResult.Any();

            NewMessagesIconVisibilityChanged?.Invoke(this, null);
        }

        public void SubscribeMessages()
        {
            MessagingCenter.Subscribe<object>(
                this,
                MessagingCenterKeys.KEY_MESSAGE_RECEIVED, async obj => { await NewMessagesFetched(); });
        }

        public async void UpdateNotificationDot()
        {
            await NewMessagesFetched();
        }

        
    }
}