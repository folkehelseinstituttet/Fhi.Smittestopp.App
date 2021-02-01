using System;
using System.Runtime.InteropServices;
using NDB.Covid19.PersistedData;
using UIKit;

namespace NDB.Covid19.iOS.Utils
{
    public class LayoutUtils
    {
        [DllImport(ObjCRuntime.Constants.ObjectiveCLibrary, EntryPoint = "objc_msgSend")]
        internal extern static IntPtr IntPtr_objc_msgSend(IntPtr receiver, IntPtr selector, UISemanticContentAttribute arg1);
        public static void OnLayoutDirectionChange()
        {
            ObjCRuntime.Selector selector = new ObjCRuntime.Selector("setSemanticContentAttribute:");
            IntPtr_objc_msgSend(UIView.Appearance.Handle, selector.Handle, GetSemanticContentAttribute());
        }

        private static UISemanticContentAttribute GetSemanticContentAttribute()
        {
            string appLanguage = LocalPreferencesHelper.GetAppLanguage();
            switch (appLanguage)
            {
                case "ar":
                    return UISemanticContentAttribute.ForceRightToLeft;
                case "ur":
                    return UISemanticContentAttribute.ForceRightToLeft;
                default:
                    return UISemanticContentAttribute.ForceLeftToRight;
            }
        }
    }
}
