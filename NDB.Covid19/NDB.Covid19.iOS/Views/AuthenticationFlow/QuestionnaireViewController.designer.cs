// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//

using System.CodeDom.Compiler;
using Foundation;

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
        CustomSubclasses.DefaultBorderButton NextBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        CustomSubclasses.RadioButton NoBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        CustomSubclasses.RadioButtonOverlayButton NoLargeBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        CustomSubclasses.SettingsPageContentLabel NoLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        CustomSubclasses.RadioButton SkipBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        CustomSubclasses.RadioButtonOverlayButton SkipLargeBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        CustomSubclasses.SettingsPageContentLabel SkipLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        CustomSubclasses.SettingsPageContentLabel SubtitleLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        CustomSubclasses.SetttingsPageTitleLabel TitleLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        CustomSubclasses.RadioButton YesButBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        CustomSubclasses.RadioButtonOverlayButton YesButLargeBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        CustomSubclasses.SettingsPageContentLabel YesButLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        CustomSubclasses.SettingsPageContentLabel YesLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        CustomSubclasses.RadioButton YesSinceBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        CustomSubclasses.RadioButtonOverlayButton YesSinceLargeBtn { get; set; }


        [Action ("NextBtnTapped:")]
        partial void NextBtnTapped (CustomSubclasses.DefaultBorderButton sender);


        [Action ("OnCloseBtnTapped:")]
        partial void OnCloseBtnTapped (UIKit.UIButton sender);


        [Action ("OnNoBtnTapped:")]
        partial void OnNoBtnTapped (CustomSubclasses.RadioButton sender);


        [Action ("OnNoLayoutTapped:")]
        partial void OnNoLayoutTapped (UIKit.UIButton sender);


        [Action ("OnSkipBtnTapped:")]
        partial void OnSkipBtnTapped (CustomSubclasses.RadioButton sender);


        [Action ("OnSkipLayoutTapped:")]
        partial void OnSkipLayoutTapped (UIKit.UIButton sender);


        [Action ("OnYesButBtnTapped:")]
        partial void OnYesButBtnTapped (CustomSubclasses.RadioButton sender);


        [Action ("OnYesButLayoutTapped:")]
        partial void OnYesButLayoutTapped (UIKit.UIButton sender);


        [Action ("OnYesSinceBtnTapped:")]
        partial void OnYesSinceBtnTapped (CustomSubclasses.RadioButton sender);


        [Action ("OnYesSinceLayoutTapped:")]
        partial void OnYesSinceLayoutTapped (UIKit.UIButton sender);

        [Action ("InfoButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void InfoButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
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