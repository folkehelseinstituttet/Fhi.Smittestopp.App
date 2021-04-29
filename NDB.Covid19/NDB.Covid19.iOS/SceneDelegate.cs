using Foundation;
﻿using System;
using NDB.Covid19.OAuth2;
using NDB.Covid19.Utils;
using UIKit;

namespace NDB.Covid19.iOS
{
    [Register("SceneDelegate")]
    public class SceneDelegate : UIResponder, IUIWindowSceneDelegate
    {
        public static bool DidEnterBackgroundState { get; private set; }

        [Export("window")]
        public UIWindow Window { get; set; }

        [Export("scene:willConnectToSession:options:")]
        public void WillConnect(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
        {
            // Use this method to optionally configure and attach the UIWindow `window` to the provided UIWindowScene `scene`.
            // If using a storyboard, the `window` property will automatically be initialized and attached to the scene.
            // This delegate does not imply the connecting scene or session are new (see UIApplicationDelegate `GetConfiguration` instead).
        }

        [Export("sceneDidDisconnect:")]
        public void DidDisconnect(UIScene scene)
        {
            // Called as the scene is being released by the system.
            // This occurs shortly after the scene enters the background, or when its session is discarded.
            // Release any resources associated with this scene that can be re-created the next time the scene connects.
            // The scene may re-connect later, as its session was not neccessarily discarded (see UIApplicationDelegate `DidDiscardSceneSessions` instead).
        }

        [Export("sceneDidBecomeActive:")]
        public void DidBecomeActive(UIScene scene)
        {
            // Called when the scene has moved from an inactive state to an active state.
            // Use this method to restart any tasks that were paused (or not yet started) when the scene was inactive.
            MessagingCenter.Send<object>(this, MessagingCenterKeys.KEY_APP_BECAME_ACTIVE);
        }

        [Export("sceneWillResignActive:")]
        public void WillResignActive(UIScene scene)
        {
            // Called when the scene will move from an active state to an inactive state.
            // This may occur due to temporary interruptions (ex. an incoming phone call).
            MessagingCenter.Send<object>(this, MessagingCenterKeys.KEY_APP_RESIGN_ACTIVE);
        }

        [Export("sceneWillEnterForeground:")]
        public void WillEnterForeground(UIScene scene)
        {
            // Called as the scene transitions from the background to the foreground.
            // Use this method to undo the changes made on entering the background.

            System.Diagnostics.Debug.Print("WillEnterForeground");
            DidEnterBackgroundState = false;
            MessagingCenter.Send<object>(this, MessagingCenterKeys.KEY_APP_RETURNS_FROM_BACKGROUND);
        }

        [Export("sceneDidEnterBackground:")]
        public void DidEnterBackground(UIScene scene)
        {
            // Called as the scene transitions from the foreground to the background.
            // Use this method to save data, release shared resources, and store enough scene-specific state information
            // to restore the scene back to its current state.
            DidEnterBackgroundState = true;
            MessagingCenter.Send<object>(this, MessagingCenterKeys.KEY_APP_WILL_ENTER_BACKGROUND);
        }

        [Export("scene:openURLContexts:")]
        public void OpenUrlContexts(UIScene scene, NSSet<UIOpenUrlContext> urlContexts)
        {
            System.Diagnostics.Debug.Print("Open url from scene delegate");
            try
            {
                UIOpenUrlContext[] array = urlContexts.ToArray();
                Uri uri_netfx = new Uri(array[0].Url.AbsoluteString);
                AuthenticationState.Authenticator.OnPageLoading(uri_netfx);
            }
            catch (Exception e) {
                LogUtils.LogException(Enums.LogSeverity.WARNING, e, $"{nameof(SceneDelegate)}.{nameof(OpenUrlContexts)}: Failed to redirect the user to the app ID Porten login in browser");
            }
        }

    }
}
