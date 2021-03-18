// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NDB.Covid19.iOS.Views.DailyNumbers
{
	[Register ("DailyNumbersViewController")]
	partial class DailyNumbersViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton BackButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIStackView ConfirmedCases_StackView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DailyNumbersLbl { get; set; }

		[Outlet]
		UIKit.UILabel DailyNumbersNumber10Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DailyNumbersNumber1Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DailyNumbersNumber3Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DailyNumbersNumber4Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DailyNumbersNumber5Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel DailyNumbersNumber6Lbl { get; set; }

		[Outlet]
		UIKit.UILabel DailyNumbersNumber7Lbl { get; set; }

		[Outlet]
		UIKit.UILabel DailyNumbersNumber9Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITextView DailyNumbersOfTheDayTextLbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIScrollView DailyNumbersScrollView { get; set; }

		[Outlet]
		UIKit.UITextView DailyNumbersSubSubHeader { get; set; }

		[Outlet]
		UIKit.UITextView DailyNumbersSubtextTwo { get; set; }

		[Outlet]
		UIKit.UILabel DailyNumbersTitleOne { get; set; }

		[Outlet]
		UIKit.UILabel DailyNumbersTitleThree { get; set; }

		[Outlet]
		UIKit.UILabel DailyNumbersTitleTwo { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView DailyNumbersView1 { get; set; }

		[Outlet]
		UIKit.UIView DailyNumbersView10 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView DailyNumbersView2 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView DailyNumbersView3 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView DailyNumbersView4 { get; set; }

		[Outlet]
		UIKit.UIView DailyNumbersView7 { get; set; }

		[Outlet]
		UIKit.UIView DailyNumbersView8 { get; set; }

		[Outlet]
		UIKit.UIView DailyNumbersView9 { get; set; }

		[Outlet]
		UIKit.UIStackView IntensiveCare_StackView { get; set; }

		[Outlet]
		UIKit.UILabel KeyFeature10Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel KeyFeature1Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel KeyFeature3Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel KeyFeature4Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel KeyFeature5Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel KeyFeature6Lbl { get; set; }

		[Outlet]
		UIKit.UILabel KeyFeature7Lbl { get; set; }

		[Outlet]
		UIKit.UILabel KeyFeature9Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIStackView NumberOfPositiveResults_StackView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIStackView NumberOfTests_StackView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIStackView PatientsAdmitted_StackView { get; set; }

		[Outlet]
		UIKit.UIStackView Smittestopp_StackView { get; set; }

		[Outlet]
		UIKit.UIStackView Statistics_StackView { get; set; }

		[Outlet]
		UIKit.UILabel TotalDailyNumbersNumber10Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel TotalDailyNumbersNumber1Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UILabel TotalDailyNumbersNumber3Lbl { get; set; }

		[Outlet]
		UIKit.UILabel TotalDailyNumbersNumber5Lbl { get; set; }

		[Outlet]
		UIKit.UILabel TotalDailyNumbersNumber9Lbl { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIStackView TotalDownloads_StackView { get; set; }

		[Outlet]
		UIKit.UIStackView Vaccinations_StackView { get; set; }

		[Outlet]
		UIKit.UIStackView VaccinationsDose1_StackView { get; set; }

		[Outlet]
		UIKit.UIStackView VaccinationsDose2_StackView { get; set; }

		[Action ("BackButton_tapped:")]
		partial void BackButton_tapped (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (BackButton != null) {
				BackButton.Dispose ();
				BackButton = null;
			}

			if (ConfirmedCases_StackView != null) {
				ConfirmedCases_StackView.Dispose ();
				ConfirmedCases_StackView = null;
			}

			if (DailyNumbersLbl != null) {
				DailyNumbersLbl.Dispose ();
				DailyNumbersLbl = null;
			}

			if (DailyNumbersNumber10Lbl != null) {
				DailyNumbersNumber10Lbl.Dispose ();
				DailyNumbersNumber10Lbl = null;
			}

			if (DailyNumbersNumber1Lbl != null) {
				DailyNumbersNumber1Lbl.Dispose ();
				DailyNumbersNumber1Lbl = null;
			}

			if (DailyNumbersNumber3Lbl != null) {
				DailyNumbersNumber3Lbl.Dispose ();
				DailyNumbersNumber3Lbl = null;
			}

			if (DailyNumbersNumber4Lbl != null) {
				DailyNumbersNumber4Lbl.Dispose ();
				DailyNumbersNumber4Lbl = null;
			}

			if (DailyNumbersNumber5Lbl != null) {
				DailyNumbersNumber5Lbl.Dispose ();
				DailyNumbersNumber5Lbl = null;
			}

			if (DailyNumbersNumber6Lbl != null) {
				DailyNumbersNumber6Lbl.Dispose ();
				DailyNumbersNumber6Lbl = null;
			}

			if (DailyNumbersNumber7Lbl != null) {
				DailyNumbersNumber7Lbl.Dispose ();
				DailyNumbersNumber7Lbl = null;
			}

			if (DailyNumbersNumber9Lbl != null) {
				DailyNumbersNumber9Lbl.Dispose ();
				DailyNumbersNumber9Lbl = null;
			}

			if (DailyNumbersOfTheDayTextLbl != null) {
				DailyNumbersOfTheDayTextLbl.Dispose ();
				DailyNumbersOfTheDayTextLbl = null;
			}

			if (DailyNumbersScrollView != null) {
				DailyNumbersScrollView.Dispose ();
				DailyNumbersScrollView = null;
			}

			if (DailyNumbersSubSubHeader != null) {
				DailyNumbersSubSubHeader.Dispose ();
				DailyNumbersSubSubHeader = null;
			}

			if (DailyNumbersSubtextTwo != null) {
				DailyNumbersSubtextTwo.Dispose ();
				DailyNumbersSubtextTwo = null;
			}

			if (DailyNumbersTitleOne != null) {
				DailyNumbersTitleOne.Dispose ();
				DailyNumbersTitleOne = null;
			}

			if (DailyNumbersTitleThree != null) {
				DailyNumbersTitleThree.Dispose ();
				DailyNumbersTitleThree = null;
			}

			if (DailyNumbersTitleTwo != null) {
				DailyNumbersTitleTwo.Dispose ();
				DailyNumbersTitleTwo = null;
			}

			if (DailyNumbersView1 != null) {
				DailyNumbersView1.Dispose ();
				DailyNumbersView1 = null;
			}

			if (DailyNumbersView10 != null) {
				DailyNumbersView10.Dispose ();
				DailyNumbersView10 = null;
			}

			if (DailyNumbersView2 != null) {
				DailyNumbersView2.Dispose ();
				DailyNumbersView2 = null;
			}

			if (DailyNumbersView3 != null) {
				DailyNumbersView3.Dispose ();
				DailyNumbersView3 = null;
			}

			if (DailyNumbersView4 != null) {
				DailyNumbersView4.Dispose ();
				DailyNumbersView4 = null;
			}

			if (DailyNumbersView7 != null) {
				DailyNumbersView7.Dispose ();
				DailyNumbersView7 = null;
			}

			if (DailyNumbersView8 != null) {
				DailyNumbersView8.Dispose ();
				DailyNumbersView8 = null;
			}

			if (DailyNumbersView9 != null) {
				DailyNumbersView9.Dispose ();
				DailyNumbersView9 = null;
			}

			if (IntensiveCare_StackView != null) {
				IntensiveCare_StackView.Dispose ();
				IntensiveCare_StackView = null;
			}

			if (KeyFeature10Lbl != null) {
				KeyFeature10Lbl.Dispose ();
				KeyFeature10Lbl = null;
			}

			if (KeyFeature1Lbl != null) {
				KeyFeature1Lbl.Dispose ();
				KeyFeature1Lbl = null;
			}

			if (KeyFeature3Lbl != null) {
				KeyFeature3Lbl.Dispose ();
				KeyFeature3Lbl = null;
			}

			if (KeyFeature4Lbl != null) {
				KeyFeature4Lbl.Dispose ();
				KeyFeature4Lbl = null;
			}

			if (KeyFeature5Lbl != null) {
				KeyFeature5Lbl.Dispose ();
				KeyFeature5Lbl = null;
			}

			if (KeyFeature6Lbl != null) {
				KeyFeature6Lbl.Dispose ();
				KeyFeature6Lbl = null;
			}

			if (KeyFeature7Lbl != null) {
				KeyFeature7Lbl.Dispose ();
				KeyFeature7Lbl = null;
			}

			if (KeyFeature9Lbl != null) {
				KeyFeature9Lbl.Dispose ();
				KeyFeature9Lbl = null;
			}

			if (NumberOfPositiveResults_StackView != null) {
				NumberOfPositiveResults_StackView.Dispose ();
				NumberOfPositiveResults_StackView = null;
			}

			if (NumberOfTests_StackView != null) {
				NumberOfTests_StackView.Dispose ();
				NumberOfTests_StackView = null;
			}

			if (PatientsAdmitted_StackView != null) {
				PatientsAdmitted_StackView.Dispose ();
				PatientsAdmitted_StackView = null;
			}

			if (TotalDailyNumbersNumber10Lbl != null) {
				TotalDailyNumbersNumber10Lbl.Dispose ();
				TotalDailyNumbersNumber10Lbl = null;
			}

			if (TotalDailyNumbersNumber1Lbl != null) {
				TotalDailyNumbersNumber1Lbl.Dispose ();
				TotalDailyNumbersNumber1Lbl = null;
			}

			if (TotalDailyNumbersNumber3Lbl != null) {
				TotalDailyNumbersNumber3Lbl.Dispose ();
				TotalDailyNumbersNumber3Lbl = null;
			}

			if (TotalDailyNumbersNumber5Lbl != null) {
				TotalDailyNumbersNumber5Lbl.Dispose ();
				TotalDailyNumbersNumber5Lbl = null;
			}

			if (TotalDailyNumbersNumber9Lbl != null) {
				TotalDailyNumbersNumber9Lbl.Dispose ();
				TotalDailyNumbersNumber9Lbl = null;
			}

			if (Statistics_StackView != null) {
				Statistics_StackView.Dispose ();
				Statistics_StackView = null;
			}

			if (TotalDownloads_StackView != null) {
				TotalDownloads_StackView.Dispose ();
				TotalDownloads_StackView = null;
			}

			if (Vaccinations_StackView != null) {
				Vaccinations_StackView.Dispose ();
				Vaccinations_StackView = null;
			}

			if (VaccinationsDose1_StackView != null) {
				VaccinationsDose1_StackView.Dispose ();
				VaccinationsDose1_StackView = null;
			}

			if (VaccinationsDose2_StackView != null) {
				VaccinationsDose2_StackView.Dispose ();
				VaccinationsDose2_StackView = null;
			}

			if (Smittestopp_StackView != null) {
				Smittestopp_StackView.Dispose ();
				Smittestopp_StackView = null;
			}
		}
	}
}
