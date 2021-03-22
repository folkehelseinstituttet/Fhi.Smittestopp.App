// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.AuthenticationFlow
{
	[Register ("QuestionnaireViewController")]
	partial class QuestionnaireViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton CloseButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView DateContainer { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DateLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIDatePicker DatePicker { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIStackView DatepickerStackView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton InfoButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.DefaultBorderButton NextBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.CustomRadioButton NoBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.RadioButtonOverlayButton NoLargeBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel NoLbl { get; set; }

		[Outlet]
		UIKit.UIScrollView ScrollView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.CustomRadioButton SkipBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.RadioButtonOverlayButton SkipLargeBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel SkipLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel SubtitleLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SetttingsPageTitleLabel TitleLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.CustomRadioButton YesButBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.RadioButtonOverlayButton YesButLargeBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel YesButLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.SettingsPageContentLabel YesLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.CustomRadioButton YesSinceBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		NDB.Covid19.iOS.Views.CustomSubclasses.RadioButtonOverlayButton YesSinceLargeBtn { get; set; }

		[Action ("InfoButton_TouchUpInside:")]
		partial void InfoButton_TouchUpInside (UIKit.UIButton sender);

		[Action ("NextBtnTapped:")]
		partial void NextBtnTapped (NDB.Covid19.iOS.Views.CustomSubclasses.DefaultBorderButton sender);

		[Action ("OnCloseBtnTapped:")]
		partial void OnCloseBtnTapped (UIKit.UIButton sender);

		[Action ("OnNoBtnTapped:")]
		partial void OnNoBtnTapped (NDB.Covid19.iOS.Views.CustomSubclasses.CustomRadioButton sender);

		[Action ("OnNoLayoutTapped:")]
		partial void OnNoLayoutTapped (UIKit.UIButton sender);

		[Action ("OnSkipBtnTapped:")]
		partial void OnSkipBtnTapped (NDB.Covid19.iOS.Views.CustomSubclasses.CustomRadioButton sender);

		[Action ("OnSkipLayoutTapped:")]
		partial void OnSkipLayoutTapped (UIKit.UIButton sender);

		[Action ("OnYesButBtnTapped:")]
		partial void OnYesButBtnTapped (NDB.Covid19.iOS.Views.CustomSubclasses.CustomRadioButton sender);

		[Action ("OnYesButLayoutTapped:")]
		partial void OnYesButLayoutTapped (UIKit.UIButton sender);

		[Action ("OnYesSinceBtnTapped:")]
		partial void OnYesSinceBtnTapped (NDB.Covid19.iOS.Views.CustomSubclasses.CustomRadioButton sender);

		[Action ("OnYesSinceLayoutTapped:")]
		partial void OnYesSinceLayoutTapped (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ScrollView != null) {
				ScrollView.Dispose ();
				ScrollView = null;
			}

			if (CloseButton != null) {
				CloseButton.Dispose ();
				CloseButton = null;
			}

			if (DateContainer != null) {
				DateContainer.Dispose ();
				DateContainer = null;
			}

			if (DateLbl != null) {
				DateLbl.Dispose ();
				DateLbl = null;
			}

			if (DatePicker != null) {
				DatePicker.Dispose ();
				DatePicker = null;
			}

			if (DatepickerStackView != null) {
				DatepickerStackView.Dispose ();
				DatepickerStackView = null;
			}

			if (InfoButton != null) {
				InfoButton.Dispose ();
				InfoButton = null;
			}

			if (NextBtn != null) {
				NextBtn.Dispose ();
				NextBtn = null;
			}

			if (NoBtn != null) {
				NoBtn.Dispose ();
				NoBtn = null;
			}

			if (NoLargeBtn != null) {
				NoLargeBtn.Dispose ();
				NoLargeBtn = null;
			}

			if (NoLbl != null) {
				NoLbl.Dispose ();
				NoLbl = null;
			}

			if (SkipBtn != null) {
				SkipBtn.Dispose ();
				SkipBtn = null;
			}

			if (SkipLargeBtn != null) {
				SkipLargeBtn.Dispose ();
				SkipLargeBtn = null;
			}

			if (SkipLbl != null) {
				SkipLbl.Dispose ();
				SkipLbl = null;
			}

			if (SubtitleLbl != null) {
				SubtitleLbl.Dispose ();
				SubtitleLbl = null;
			}

			if (TitleLbl != null) {
				TitleLbl.Dispose ();
				TitleLbl = null;
			}

			if (YesButBtn != null) {
				YesButBtn.Dispose ();
				YesButBtn = null;
			}

			if (YesButLargeBtn != null) {
				YesButLargeBtn.Dispose ();
				YesButLargeBtn = null;
			}

			if (YesButLbl != null) {
				YesButLbl.Dispose ();
				YesButLbl = null;
			}

			if (YesLbl != null) {
				YesLbl.Dispose ();
				YesLbl = null;
			}

			if (YesSinceBtn != null) {
				YesSinceBtn.Dispose ();
				YesSinceBtn = null;
			}

			if (YesSinceLargeBtn != null) {
				YesSinceLargeBtn.Dispose ();
				YesSinceLargeBtn = null;
			}
		}
	}
}
