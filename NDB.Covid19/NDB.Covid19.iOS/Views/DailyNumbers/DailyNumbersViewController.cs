using System;
using NDB.Covid19.ViewModels;
using NDB.Covid19.iOS.Utils;
using UIKit;
using static NDB.Covid19.ViewModels.DailyNumbersViewModel;

namespace NDB.Covid19.iOS.Views.DailyNumbers
{
    public partial class DailyNumbersViewController : BaseViewController
	{
		private readonly DailyNumbersViewModel _viewModel;

		public DailyNumbersViewController(IntPtr handle) : base(handle)
		{
			_viewModel = new DailyNumbersViewModel();
		}

		public static DailyNumbersViewController Create()
		{
			UIStoryboard storyboard = UIStoryboard.FromName("DailyNumbers", null);
			DailyNumbersViewController vc = (DailyNumbersViewController)storyboard.InstantiateInitialViewController();
			vc.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
			return vc;
		}

		public static UINavigationController GetDailyNumbersPageControllerInNavigationController()
		{
			UIViewController vc = Create();
			UINavigationController navigationController = new UINavigationController(vc);
			navigationController.SetNavigationBarHidden(true, false);
			navigationController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
			return navigationController;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			SetStyling();
		}

		private void SetStyling()
		{
			// Labels dependable on device width
			double width = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width;
			int fontMin = width <= 700 ? 18 : 23;
			int fontMax = width <= 700 ? 20 : 25;
			StyleUtil.InitLabelWithSpacing(DailyNumbersNumber1Lbl, StyleUtil.FontType.FontRegular, ConfirmedCasesToday, 1.14, fontMin, fontMax, UITextAlignment.Center);
			StyleUtil.InitLabelWithSpacing(DailyNumbersNumber2Lbl, StyleUtil.FontType.FontRegular, DeathsToday, 1.14, fontMin, fontMax, UITextAlignment.Center);
			StyleUtil.InitLabelWithSpacing(DailyNumbersNumber3Lbl, StyleUtil.FontType.FontRegular, TestsConductedToday, 1.14, fontMin, fontMax, UITextAlignment.Center);
			StyleUtil.InitLabelWithSpacing(DailyNumbersNumber4Lbl, StyleUtil.FontType.FontRegular, PatientsAdmittedToday, 1.14, fontMin, fontMax, UITextAlignment.Center);
			StyleUtil.InitLabelWithSpacing(DailyNumbersNumber5Lbl, StyleUtil.FontType.FontRegular, SmittestopDownloadsTotal, 1.14, fontMin, fontMax, UITextAlignment.Center);
			StyleUtil.InitLabelWithSpacing(DailyNumbersNumber6Lbl, StyleUtil.FontType.FontRegular, NumberOfPositiveTestsResultsLast7Days, 1.14, fontMin, fontMax, UITextAlignment.Center);

			StyleUtil.InitLabelWithSpacing(DailyNumbersNumber9Lbl, StyleUtil.FontType.FontRegular, VaccinationsDoseOneToday, 1.14, fontMin, fontMax, UITextAlignment.Center);
			StyleUtil.InitLabelWithSpacing(DailyNumbersNumber10Lbl, StyleUtil.FontType.FontRegular, VaccinationsDoseTwoToday, 1.14, fontMin, fontMax, UITextAlignment.Center);

			// Labels not dependable on device width
			StyleUtil.InitLabelWithSpacing(DailyNumbersTitleOne, StyleUtil.FontType.FontBold, DAILY_NUMBERS_TITLE_ONE, 1.14, 24, 36);

			StyleUtil.InitLabelWithSpacing(DailyNumbersLbl, StyleUtil.FontType.FontBold, DAILY_NUMBERS_HEADER, 1.14, 24, 36);
			StyleUtil.InitLabelWithSpacing(DailyNumbersOfTheDayTextLbl, StyleUtil.FontType.FontRegular, LastUpdateStringSubHeader, 1.14, 12, 14);

			StyleUtil.InitLabelWithSpacing(KeyFeature1Lbl, StyleUtil.FontType.FontBold, KEY_FEATURE_ONE_LABEL, 1.14, 16, 18, UITextAlignment.Center);
			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber1Lbl, StyleUtil.FontType.FontRegular, ConfirmedCasesTotal, 1.14, 12, 14, UITextAlignment.Center);
			BackgroundView1.BackgroundColor = UIColor.FromRGB(197, 226, 252);
			BackgroundView1.Layer.CornerRadius = 12;

			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber2Lbl, StyleUtil.FontType.FontRegular, DeathsTotal, 1.14, 12, 14, UITextAlignment.Center);
			StyleUtil.InitLabelWithSpacing(KeyFeature2Lbl, StyleUtil.FontType.FontBold, KEY_FEATURE_TWO_LABEL, 1.14, 16, 18, UITextAlignment.Center);
			BackgroundView2.BackgroundColor = UIColor.FromRGB(197, 226, 252);
			BackgroundView2.Layer.CornerRadius = 12;

			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber3Lbl, StyleUtil.FontType.FontRegular, TestsConductedTotal, 1.14, 12, 14, UITextAlignment.Center);
			StyleUtil.InitLabelWithSpacing(KeyFeature3Lbl, StyleUtil.FontType.FontBold, KEY_FEATURE_THREE_LABEL, 1.14, 16, 18, UITextAlignment.Center);
			BackgroundView3.BackgroundColor = UIColor.FromRGB(197, 226, 252);
			BackgroundView3.Layer.CornerRadius = 12;

			StyleUtil.InitLabelWithSpacing(KeyFeature4Lbl, StyleUtil.FontType.FontBold, KEY_FEATURE_FOUR_LABEL, 1.14, 16, 18, UITextAlignment.Center);
			BackgroundView4.BackgroundColor = UIColor.FromRGB(197, 226, 252);
			BackgroundView4.Layer.CornerRadius = 12;

			StyleUtil.InitLabelWithSpacing(KeyFeature6Lbl, StyleUtil.FontType.FontBold, KEY_FEATURE_SIX_LABEL, 1.14, 16, 18, UITextAlignment.Center);
			BackgroundView5.BackgroundColor = UIColor.FromRGB(197, 226, 252);
			BackgroundView5.Layer.CornerRadius = 12;

			StyleUtil.InitLabelWithSpacing(KeyFeature5Lbl, StyleUtil.FontType.FontBold, KEY_FEATURE_FIVE_LABEL, 1.14, 16, 18, UITextAlignment.Center);
			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber5Lbl, StyleUtil.FontType.FontRegular, NumberOfPositiveTestsResultsTotal, 1.14, 12, 14, UITextAlignment.Center);
			BackgroundView6.BackgroundColor = UIColor.FromRGB(197, 226, 252);
			BackgroundView6.Layer.CornerRadius = 12;

			StyleUtil.InitUITextViewWithSpacingAndUrl(DailyNumbersSubLbl, StyleUtil.FontType.FontRegular, LastUpdateStringSubSubHeader, 1.14, 12, 12);
			DailyNumbersSubLbl.SizeToFit();

			StyleUtil.InitLabelWithSpacing(DailyNumbersTitleTwo, StyleUtil.FontType.FontBold, DAILY_NUMBERS_TITLE_TWO, 1.14, 24, 36);
			StyleUtil.InitLabelWithSpacing(DailyNumbersSubtextTwo, StyleUtil.FontType.FontRegular, DAILY_NUMBERS_SUBTEXT_TWO, 1.14, 12, 14);

			StyleUtil.InitLabelWithSpacing(KeyFeature9Lbl, StyleUtil.FontType.FontBold, KEY_FEATURE_NINE_LABEL, 1.14, 16, 18, UITextAlignment.Center);
			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber9Lbl, StyleUtil.FontType.FontRegular, VaccinationsDoseOneTotal, 1.14, 12, 14, UITextAlignment.Center);
			BackgroundView7.BackgroundColor = UIColor.FromRGB(197, 226, 252);
			BackgroundView7.Layer.CornerRadius = 12;

			StyleUtil.InitLabelWithSpacing(KeyFeature10Lbl, StyleUtil.FontType.FontBold, KEY_FEATURE_TEN_LABEL, 1.14, 16, 18, UITextAlignment.Center);
			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber10Lbl, StyleUtil.FontType.FontRegular, VaccinationsDoseTwoTotal, 1.14, 12, 14, UITextAlignment.Center);
			BackgroundView8.BackgroundColor = UIColor.FromRGB(197, 226, 252);
			BackgroundView8.Layer.CornerRadius = 12;

			StyleUtil.InitLabelWithSpacing(DailyNumbersTitleThree, StyleUtil.FontType.FontBold, DAILY_NUMBERS_TITLE_THREE, 1.14, 24, 36);
			StyleUtil.InitLabelWithSpacing(DailyNumbersSubSubHeader, StyleUtil.FontType.FontRegular, DAILY_NUMBERS_SUBSUBHEADER, 1.14, 12, 14);

			//Setting up accessibility grouping
			ConfirmedCases_StackView.ShouldGroupAccessibilityChildren = true;
			NumberOfDeaths_StackView.ShouldGroupAccessibilityChildren = true;
			NumberOfTests_StackView.ShouldGroupAccessibilityChildren = true;
			PatientsAdmitted_StackView.ShouldGroupAccessibilityChildren = true;
			TotalDownloads_StackView.ShouldGroupAccessibilityChildren = true;
			NumberOfPositiveResults_StackView.ShouldGroupAccessibilityChildren = true;

			BackButton.AccessibilityLabel = SettingsViewModel.SETTINGS_ITEM_ACCESSIBILITY_CLOSE_BUTTON;

			//Implemented for correct voiceover due to smitte|stop, removing pronunciation of lodretstreg
			KeyFeature5Lbl.AccessibilityAttributedLabel = AccessibilityUtils.RemovePoorlySpokenSymbols(KEY_FEATURE_SIX_LABEL);

			string contentText = DailyNumbersViewModel.LastUpdateStringSubSubHeader;
			DailyNumbersSubLbl.AccessibilityValue = AccessibilityUtils.RemovePoorlySpokenSymbolsString(contentText);
			DailyNumbersSubLbl.IsAccessibilityElement = true;
		}

		partial void BackButton_tapped(UIButton sender)
		{
			NavigationController.PopViewController(true);
			NavigationHelper.GoToResultPage(this, false);
		}
	}
}
