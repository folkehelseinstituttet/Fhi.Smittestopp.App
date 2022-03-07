// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.InfectionStatus
{
	[Register ("InfectionStatusViewController")]
	partial class InfectionStatusViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel ActivityExplainerLbl { get; set; }

		[Outlet]
		UIKit.UIImageView appLogo { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel AreYouInfectetLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView AreYouInfectetView { get; set; }

		[Outlet]
		UIKit.UIImageView dailyNumbersIcon { get; set; }

		[Outlet]
		UIKit.UILabel dailyNumbersLbl { get; set; }

		[Outlet]
		UIKit.UILabel dailyNumbersUpdatedLbl { get; set; }

		[Outlet]
		UIKit.UIView DailyNumbersView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIImageView fhiLogo { get; set; }

		[Outlet]
		UIKit.UILabel InformationBannerLbl { get; set; }

		[Outlet]
		UIKit.UIView InformationBannerView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel LogInAndRegisterLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton MenuIcon { get; set; }

		[Outlet]
		UIKit.UILabel MenuLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIImageView MessageIcon { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel MessageLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView MessageView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView NewIndicatorView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel NewRegistrationLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton OnOffBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView OnOffBtnContainer { get; set; }

		[Outlet]
		UIKit.UIPickerView Picker { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView ScrollDownBackgroundView { get; set; }

		[Outlet]
		UIKit.UIButton SpinnerDialogButton { get; set; }

		[Outlet]
		UIKit.UILabel SpinnerDialogMessage { get; set; }

		[Outlet]
		UIKit.UIStackView SpinnerDialogStackView { get; set; }

		[Outlet]
		UIKit.UILabel SpinnerDialogTitle { get; set; }

		[Outlet]
		UIKit.UIView SpinnerMainView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView StatusContainer { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel StatusText { get; set; }

		[Outlet]
		UIKit.UIView TopBar { get; set; }

		[Action ("OnMenubtnTapped:")]
		partial void OnMenubtnTapped (UIKit.UIButton sender);

		[Action ("OnOffBtnTapped:")]
		partial void OnOffBtnTapped (UIKit.UIButton sender);

		[Action ("OnSpinnerDialogButton_TouchUpInside:")]
		partial void OnSpinnerDialogButton_TouchUpInside (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ActivityExplainerLbl != null) {
				ActivityExplainerLbl.Dispose ();
				ActivityExplainerLbl = null;
			}

			if (appLogo != null) {
				appLogo.Dispose ();
				appLogo = null;
			}

			if (AreYouInfectetLbl != null) {
				AreYouInfectetLbl.Dispose ();
				AreYouInfectetLbl = null;
			}

			if (AreYouInfectetView != null) {
				AreYouInfectetView.Dispose ();
				AreYouInfectetView = null;
			}

			if (dailyNumbersIcon != null) {
				dailyNumbersIcon.Dispose ();
				dailyNumbersIcon = null;
			}

			if (dailyNumbersLbl != null) {
				dailyNumbersLbl.Dispose ();
				dailyNumbersLbl = null;
			}

			if (dailyNumbersUpdatedLbl != null) {
				dailyNumbersUpdatedLbl.Dispose ();
				dailyNumbersUpdatedLbl = null;
			}

			if (DailyNumbersView != null) {
				DailyNumbersView.Dispose ();
				DailyNumbersView = null;
			}

			if (fhiLogo != null) {
				fhiLogo.Dispose ();
				fhiLogo = null;
			}

			if (InformationBannerLbl != null) {
				InformationBannerLbl.Dispose ();
				InformationBannerLbl = null;
			}

			if (LogInAndRegisterLbl != null) {
				LogInAndRegisterLbl.Dispose ();
				LogInAndRegisterLbl = null;
			}

			if (MenuIcon != null) {
				MenuIcon.Dispose ();
				MenuIcon = null;
			}

			if (MenuLabel != null) {
				MenuLabel.Dispose ();
				MenuLabel = null;
			}

			if (MessageIcon != null) {
				MessageIcon.Dispose ();
				MessageIcon = null;
			}

			if (MessageLbl != null) {
				MessageLbl.Dispose ();
				MessageLbl = null;
			}

			if (MessageView != null) {
				MessageView.Dispose ();
				MessageView = null;
			}

			if (NewIndicatorView != null) {
				NewIndicatorView.Dispose ();
				NewIndicatorView = null;
			}

			if (NewRegistrationLbl != null) {
				NewRegistrationLbl.Dispose ();
				NewRegistrationLbl = null;
			}

			if (OnOffBtn != null) {
				OnOffBtn.Dispose ();
				OnOffBtn = null;
			}

			if (OnOffBtnContainer != null) {
				OnOffBtnContainer.Dispose ();
				OnOffBtnContainer = null;
			}

			if (Picker != null) {
				Picker.Dispose ();
				Picker = null;
			}

			if (ScrollDownBackgroundView != null) {
				ScrollDownBackgroundView.Dispose ();
				ScrollDownBackgroundView = null;
			}

			if (SpinnerDialogButton != null) {
				SpinnerDialogButton.Dispose ();
				SpinnerDialogButton = null;
			}

			if (SpinnerDialogMessage != null) {
				SpinnerDialogMessage.Dispose ();
				SpinnerDialogMessage = null;
			}

			if (SpinnerDialogStackView != null) {
				SpinnerDialogStackView.Dispose ();
				SpinnerDialogStackView = null;
			}

			if (SpinnerDialogTitle != null) {
				SpinnerDialogTitle.Dispose ();
				SpinnerDialogTitle = null;
			}

			if (SpinnerMainView != null) {
				SpinnerMainView.Dispose ();
				SpinnerMainView = null;
			}

			if (StatusContainer != null) {
				StatusContainer.Dispose ();
				StatusContainer = null;
			}

			if (StatusText != null) {
				StatusText.Dispose ();
				StatusText = null;
			}

			if (TopBar != null) {
				TopBar.Dispose ();
				TopBar = null;
			}

			if (InformationBannerView != null) {
				InformationBannerView.Dispose ();
				InformationBannerView = null;
			}
		}
	}
}
