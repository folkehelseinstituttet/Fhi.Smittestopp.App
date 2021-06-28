using System;
using NDB.Covid19.ViewModels;
using NDB.Covid19.iOS.Utils;
using UIKit;
using static NDB.Covid19.ViewModels.DailyNumbersViewModel;
using Foundation;
using NDB.Covid19.Utils;

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

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			SetStyling();
			MessagingCenter.Subscribe<object>(this, MessagingCenterKeys.KEY_APP_RETURNS_FROM_BACKGROUND, OnAppReturnsFromBackground);
			UIAccessibility.PostNotification(UIAccessibilityPostNotification.ScreenChanged, DailyNumbersTitleOne);
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
			DailyNumbersView5.BackgroundColor = ColorHelper.PRIMARY_COLOR;
			DailyNumbersView5.Layer.CornerRadius = 12;
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
			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber1Lbl, StyleUtil.FontType.FontBold, ConfirmedCasesTotal, 1.14, fontMin, fontMax);
			TotalDailyNumbersNumber1Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber3Lbl, StyleUtil.FontType.FontBold, TestsConductedTotal, 1.14, fontMin, fontMax);
			TotalDailyNumbersNumber3Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber4Lbl, StyleUtil.FontType.FontBold, PatientsAdmittedTotal, 1.14, fontMin, fontMax);
			TotalDailyNumbersNumber4Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			StyleUtil.InitLabelWithSpacing(DailyNumbersNumber5Lbl, StyleUtil.FontType.FontBold, NumberOfPositiveTestsResultsLast7Days, 1.14, fontMin, fontMax);
			DailyNumbersNumber5Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			StyleUtil.InitLabelWithSpacing(DailyNumbersNumber6Lbl, StyleUtil.FontType.FontBold, SmittestopDownloadsTotal, 1.14, fontMin, fontMax);
			DailyNumbersNumber6Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber7Lbl, StyleUtil.FontType.FontBold, PatientsIntensiveCareTotal, 1.14, fontMin, fontMax);
			TotalDailyNumbersNumber7Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber8Lbl, StyleUtil.FontType.FontBold, DeathsTotal, 1.14, fontMin, fontMax);
			TotalDailyNumbersNumber8Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber9Lbl, StyleUtil.FontType.FontBold, VaccinationsDoseOneTotal, 1.14, fontMin,fontMax);
			TotalDailyNumbersNumber9Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber10Lbl, StyleUtil.FontType.FontBold, VaccinationsDoseTwoTotal, 1.14, fontMin, fontMax);
			TotalDailyNumbersNumber10Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;

			// Labels not dependable on device width
			StyleUtil.InitLabelWithSpacing(DailyNumbersTitleOne, StyleUtil.FontType.FontBold, DAILY_NUMBERS_HEADER, 1.14, 30, 36);
			DailyNumbersTitleOne.AccessibilityTraits = UIAccessibilityTrait.Header;

			StyleUtil.InitLabelWithSpacing(DailyNumbersLbl, StyleUtil.FontType.FontBold, DAILY_NUMBERS_TITLE_ONE, 1.14, 20, 36);
			DailyNumbersLbl.AccessibilityTraits = UIAccessibilityTrait.Header;
			SetupSubTextWithLink(LastUpdateStringSubHeader, DailyNumbersOfTheDayTextLbl);
			DailyNumbersOfTheDayTextLbl.WeakDelegate = new OpenTextViewUrlInWebviewDelegate(this);

			StyleUtil.InitLabelWithSpacing(KeyFeature1Lbl, StyleUtil.FontType.FontRegular, KEY_FEATURE_ONE_LABEL, 1.14, 16, 18);
			KeyFeature1Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			

			StyleUtil.InitLabelWithSpacing(KeyFeature3Lbl, StyleUtil.FontType.FontRegular, KEY_FEATURE_THREE_LABEL, 1.14, 16, 18);
			KeyFeature3Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			
			StyleUtil.InitLabelWithSpacing(KeyFeature4Lbl, StyleUtil.FontType.FontRegular, KEY_FEATURE_FOUR_LABEL, 1.14, 16, 18, false, true);
			KeyFeature4Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;

			StyleUtil.InitLabelWithSpacing(KeyFeature6Lbl, StyleUtil.FontType.FontRegular, KEY_FEATURE_SIX_LABEL, 1.14, 16, 18);
			KeyFeature6Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;

			StyleUtil.InitLabelWithSpacing(KeyFeature5Lbl, StyleUtil.FontType.FontRegular, KEY_FEATURE_FIVE_LABEL, 1.14, 16, 18);
			StyleUtil.InitLabelWithSpacing(TotalDailyNumbersNumber5Lbl, StyleUtil.FontType.FontRegular, NumberOfPositiveTestsResultsTotal, 1.14, 12, 14);
			KeyFeature5Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;
			TotalDailyNumbersNumber5Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;

			StyleUtil.InitLabelWithSpacing(KeyFeature7Lbl, StyleUtil.FontType.FontRegular, KEY_FEATURE_SEVEN_LABEL, 1.14, 16, 18);
			KeyFeature7Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;

			StyleUtil.InitLabelWithSpacing(KeyFeature8Lbl, StyleUtil.FontType.FontRegular, KEY_FEATURE_EIGHT_LABEL, 1.14, 16, 18);
			KeyFeature8Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;


			StyleUtil.InitLabelWithSpacing(DailyNumbersTitleTwo, StyleUtil.FontType.FontBold, DAILY_NUMBERS_TITLE_TWO, 1.14, 20, 36);
			SetupSubTextWithLink(LastUpdateStringSubTextTwo, DailyNumbersSubtextTwo);
			DailyNumbersSubtextTwo.WeakDelegate = new OpenTextViewUrlInWebviewDelegate(this);
			DailyNumbersTitleTwo.AccessibilityTraits = UIAccessibilityTrait.Header;

			StyleUtil.InitLabelWithSpacing(KeyFeature9Lbl, StyleUtil.FontType.FontRegular, KEY_FEATURE_NINE_LABEL, 1.14, 16, 18);
			KeyFeature9Lbl.AccessibilityLabel = KEY_FEATURE_NINE_ACCESSIBILITY_LABEL;
			KeyFeature9Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;

			StyleUtil.InitLabelWithSpacing(KeyFeature10Lbl, StyleUtil.FontType.FontRegular, KEY_FEATURE_TEN_LABEL, 1.14, 16, 18);
			KeyFeature10Lbl.AccessibilityLabel = KEY_FEATURE_TEN_ACCESSIBILITY_LABEL;
			KeyFeature10Lbl.TextColor = ColorHelper.TEXT_COLOR_ON_PRIMARY;


			StyleUtil.InitLabelWithSpacing(DailyNumbersTitleThree, StyleUtil.FontType.FontBold, DAILY_NUMBERS_TITLE_THREE, 1.14, 20, 36);
			SetupSubTextWithLink(LastUpdateStringSubSubHeader, DailyNumbersSubSubHeader);
			DailyNumbersSubSubHeader.WeakDelegate = new OpenTextViewUrlInWebviewDelegate(this);
			DailyNumbersTitleThree.AccessibilityTraits = UIAccessibilityTrait.Header;

			//Setting up accessibility grouping
			Statistics_StackView.ShouldGroupAccessibilityChildren = true;
			Vaccinations_StackView.ShouldGroupAccessibilityChildren = true;
			Smittestopp_StackView.ShouldGroupAccessibilityChildren = true;
			ConfirmedCases_StackView.ShouldGroupAccessibilityChildren = true;
			NumberOfTests_StackView.ShouldGroupAccessibilityChildren = true;
			PatientsAdmitted_StackView.ShouldGroupAccessibilityChildren = true;
			TotalDownloads_StackView.ShouldGroupAccessibilityChildren = true;
			NumberOfPositiveResults_StackView.ShouldGroupAccessibilityChildren = true;
			VaccinationsDose1_StackView.ShouldGroupAccessibilityChildren = true;
			VaccinationsDose2_StackView.ShouldGroupAccessibilityChildren = true;
			Deaths_StackView.ShouldGroupAccessibilityChildren = true;

			// Back button styling and accessibility
			BackButton.AccessibilityLabel = BACK_BUTTON_ACCESSIBILITY_TEXT;

			//Implemented for correct voiceover due to smitte|stop, removing pronunciation of lodretstreg
			KeyFeature5Lbl.AccessibilityAttributedLabel = AccessibilityUtils.RemovePoorlySpokenSymbols(KEY_FEATURE_FIVE_LABEL);
		}

		partial void BackButton_tapped(UIButton sender)
		{
			NavigationController?.PopToRootViewController(true);
		}

		private void SetupSubTextWithLink(string text, UITextView textView)
		{
			// Necessary to unify horizontal alignment with the rest of the text on the page
			textView.TextContainerInset = UIEdgeInsets.Zero;
			textView.TextContainer.LineFragmentPadding = 0;

			//Defining attibutes inorder to format the embedded link
			NSAttributedStringDocumentAttributes documentAttributes = new NSAttributedStringDocumentAttributes { DocumentType = NSDocumentType.HTML };
			documentAttributes.StringEncoding = NSStringEncoding.UTF8;
			NSError error = null;
			NSAttributedString attributedString = new NSAttributedString(NSData.FromString(text, NSStringEncoding.UTF8), documentAttributes, ref error);

			//Ensuring text is resiezed correctly when font size is increased
			StyleUtil.InitTextViewWithSpacingAndUrl(textView, StyleUtil.FontType.FontRegular, attributedString, 1.28, 16, 22);

			textView.TextColor = ColorHelper.TEXT_COLOR_ON_BACKGROUND;

			//ForegroundColor sets the color of the links. UnderlineStyle determins if the link is underlined, 0 without underline 1 with underline.
			textView.WeakLinkTextAttributes = new NSDictionary(UIStringAttributeKey.ForegroundColor, ColorHelper.LINK_COLOR, UIStringAttributeKey.UnderlineStyle, new NSNumber(1));
		}

		private void OnAppReturnsFromBackground(object o)
		{
			RequestFHIDataUpdate(() => InvokeOnMainThread(SetStyling));
		}
	}
}