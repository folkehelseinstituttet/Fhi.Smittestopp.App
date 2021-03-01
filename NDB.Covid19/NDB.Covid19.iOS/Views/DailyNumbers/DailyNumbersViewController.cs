﻿using System;
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
			// Set background color and corner radius for views
			DailyNumbersView1.BackgroundColor = ColorHelper.PRIMARY_COLOR;
			DailyNumbersView1.Layer.CornerRadius = 12;
			DailyNumbersView2.BackgroundColor = ColorHelper.PRIMARY_COLOR;
			DailyNumbersView2.Layer.CornerRadius = 12;
			DailyNumbersView3.BackgroundColor = ColorHelper.PRIMARY_COLOR;
			DailyNumbersView3.Layer.CornerRadius = 12;
			DailyNumbersView4.BackgroundColor = ColorHelper.PRIMARY_COLOR;
			DailyNumbersView4.Layer.CornerRadius = 12;
			DailyNumbersView7.BackgroundColor = ColorHelper.PRIMARY_COLOR;
			DailyNumbersView7.Layer.CornerRadius = 12;
			DailyNumbersView8.BackgroundColor = ColorHelper.PRIMARY_COLOR;
			DailyNumbersView8.Layer.CornerRadius = 12;
			DailyNumbersView9.BackgroundColor = ColorHelper.PRIMARY_COLOR;
			DailyNumbersView9.Layer.CornerRadius = 12;
			DailyNumbersView10.BackgroundColor = ColorHelper.PRIMARY_COLOR;
			DailyNumbersView10.Layer.CornerRadius = 12;


			// Labels dependable on device width
			double width = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width;
			int fontMin = width <= 700 ? 22 : 27;
			int fontMax = width <= 700 ? 24 : 29;
			StyleUtil.InitLabelWithSpacing(DailyNumbersNumber1Lbl, StyleUtil.FontType.FontBold, ConfirmedCasesToday, 1.14, fontMin, fontMax);
			DailyNumbersNumber1Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			StyleUtil.InitLabelWithSpacing(DailyNumbersNumber3Lbl, StyleUtil.FontType.FontBold, TestsConductedToday, 1.14, fontMin, fontMax);
			DailyNumbersNumber3Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			StyleUtil.InitLabelWithSpacing(DailyNumbersNumber4Lbl, StyleUtil.FontType.FontBold, PatientsAdmittedToday, 1.14, fontMin, fontMax);
			DailyNumbersNumber4Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			StyleUtil.InitLabelWithSpacing(DailyNumbersNumber5Lbl, StyleUtil.FontType.FontBold, SmittestopDownloadsTotal, 1.14, fontMin, fontMax);
			DailyNumbersNumber5Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			StyleUtil.InitLabelWithSpacing(DailyNumbersNumber6Lbl, StyleUtil.FontType.FontBold, NumberOfPositiveTestsResultsLast7Days, 1.14, fontMin, fontMax);
			DailyNumbersNumber6Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			StyleUtil.InitLabelWithSpacing(DailyNumbersNumber7Lbl, StyleUtil.FontType.FontBold, PatientsIntensiveCare, 1.14, fontMin, fontMax);
			DailyNumbersNumber7Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			StyleUtil.InitLabelWithSpacing(DailyNumbersNumber9Lbl, StyleUtil.FontType.FontBold, VaccinationsDoseOneToday, 1.14, fontMin, fontMax);
			DailyNumbersNumber9Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			StyleUtil.InitLabelWithSpacing(DailyNumbersNumber10Lbl, StyleUtil.FontType.FontBold, VaccinationsDoseTwoToday, 1.14, fontMin, fontMax);
			DailyNumbersNumber10Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;

			// Labels not dependable on device width
			StyleUtil.InitLabelWithSpacing(DailyNumbersTitleOne, StyleUtil.FontType.FontBold, DAILY_NUMBERS_HEADER, 1.14, 30, 36);

			StyleUtil.InitLabelWithSpacing(DailyNumbersLbl, StyleUtil.FontType.FontBold, DAILY_NUMBERS_TITLE_ONE, 1.14, 20, 36);
			StyleUtil.InitLabelWithSpacing(DailyNumbersOfTheDayTextLbl, StyleUtil.FontType.FontRegular, LastUpdateStringSubHeader, 1.14, 12, 14);

			StyleUtil.InitLabelWithSpacing(KeyFeature1Lbl, StyleUtil.FontType.FontRegular, KEY_FEATURE_ONE_LABEL, 1.14, 16, 18);
			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber1Lbl, StyleUtil.FontType.FontRegular, ConfirmedCasesTotal, 1.14, 12, 14);
			KeyFeature1Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			TotalDailyNumbersNumber1Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;

			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber3Lbl, StyleUtil.FontType.FontRegular, TestsConductedTotal, 1.14, 12, 14);
			StyleUtil.InitLabelWithSpacing(KeyFeature3Lbl, StyleUtil.FontType.FontRegular, KEY_FEATURE_THREE_LABEL, 1.14, 16, 18);
			KeyFeature3Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			TotalDailyNumbersNumber3Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;

			StyleUtil.InitLabelWithSpacing(KeyFeature4Lbl, StyleUtil.FontType.FontRegular, KEY_FEATURE_FOUR_LABEL, 1.14, 16, 18);
			KeyFeature4Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;

			StyleUtil.InitLabelWithSpacing(KeyFeature6Lbl, StyleUtil.FontType.FontRegular, KEY_FEATURE_SIX_LABEL, 1.14, 16, 18);
			KeyFeature6Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;

			StyleUtil.InitLabelWithSpacing(KeyFeature5Lbl, StyleUtil.FontType.FontRegular, KEY_FEATURE_FIVE_LABEL, 1.14, 16, 18);
			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber5Lbl, StyleUtil.FontType.FontRegular, NumberOfPositiveTestsResultsTotal, 1.14, 12, 14);
			KeyFeature5Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			TotalDailyNumbersNumber5Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;

			StyleUtil.InitLabelWithSpacing(KeyFeature7Lbl, StyleUtil.FontType.FontRegular, KEY_FEATURE_SEVEN_LABEL, 1.14, 16, 18);
			KeyFeature7Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;

			StyleUtil.InitLabelWithSpacing(DailyNumbersTitleTwo, StyleUtil.FontType.FontBold, DAILY_NUMBERS_TITLE_TWO, 1.14, 20, 36);
			StyleUtil.InitLabelWithSpacing(DailyNumbersSubtextTwo, StyleUtil.FontType.FontRegular, LastUpdateStringSubTextTwo, 1.14, 12, 14);

			StyleUtil.InitLabelWithSpacing(KeyFeature9Lbl, StyleUtil.FontType.FontRegular, KEY_FEATURE_NINE_LABEL, 1.14, 16, 18);
			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber9Lbl, StyleUtil.FontType.FontRegular, VaccinationsDoseOneTotal, 1.14, 12, 14);
			KeyFeature9Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			TotalDailyNumbersNumber9Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;

			StyleUtil.InitLabelWithSpacing(KeyFeature10Lbl, StyleUtil.FontType.FontRegular, KEY_FEATURE_TEN_LABEL, 1.14, 16, 18);
			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber10Lbl, StyleUtil.FontType.FontRegular, VaccinationsDoseTwoTotal, 1.14, 12, 14);
			KeyFeature10Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			TotalDailyNumbersNumber10Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;

			StyleUtil.InitLabelWithSpacing(DailyNumbersTitleThree, StyleUtil.FontType.FontBold, DAILY_NUMBERS_TITLE_THREE, 1.14, 20, 36);
			StyleUtil.InitLabelWithSpacing(DailyNumbersSubSubHeader, StyleUtil.FontType.FontRegular, LastUpdateStringSubSubHeader, 1.14, 12, 14);

			//Setting up accessibility grouping
			ConfirmedCases_StackView.ShouldGroupAccessibilityChildren = true;
			NumberOfTests_StackView.ShouldGroupAccessibilityChildren = true;
			PatientsAdmitted_StackView.ShouldGroupAccessibilityChildren = true;
			IntensiveCare_StackView.ShouldGroupAccessibilityChildren = true;
			VaccinationsDose1_StackView.ShouldGroupAccessibilityChildren = true;
			VaccinationsDose2_StackView.ShouldGroupAccessibilityChildren = true;
			TotalDownloads_StackView.ShouldGroupAccessibilityChildren = true;
			NumberOfPositiveResults_StackView.ShouldGroupAccessibilityChildren = true;

			// Back button styling and accessibility
			BackButton.AccessibilityLabel = SettingsViewModel.SETTINGS_ITEM_ACCESSIBILITY_CLOSE_BUTTON;
		}

		partial void BackButton_tapped(UIButton sender)
		{
			NavigationController.PopViewController(true);
			NavigationHelper.GoToResultPage(this, false);
		}
	}
}
