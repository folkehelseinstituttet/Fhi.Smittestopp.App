using NDB.Covid19.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

//Generated from http://essential-interfaces.azurewebsites.net/
namespace NDB.Covid19.Interfaces
{
	public interface IEssentialsImplementation
    {

    }

	public interface IAccelerometer
	{
		void Start(SensorSpeed sensorSpeed);
		void Stop();
		bool IsMonitoring { get; }
		event EventHandler<AccelerometerChangedEventArgs> ReadingChanged;
		event EventHandler ShakeDetected;
	}
	public interface IAppInfo
	{
		void ShowSettingsUI();
		string PackageName { get; }
		string Name { get; }
		string VersionString { get; }
		Version Version { get; }
		string BuildString { get; }
		AppTheme RequestedTheme { get; }
	}
	public interface IBarometer
	{
		void Start(SensorSpeed sensorSpeed);
		void Stop();
		bool IsMonitoring { get; }
		event EventHandler<BarometerChangedEventArgs> ReadingChanged;
	}
	public interface IBattery
	{
		double ChargeLevel { get; }
		BatteryState State { get; }
		BatteryPowerSource PowerSource { get; }
		EnergySaverStatus EnergySaverStatus { get; }
		event EventHandler<BatteryInfoChangedEventArgs> BatteryInfoChanged;
		event EventHandler<EnergySaverStatusChangedEventArgs> EnergySaverStatusChanged;
	}
	public interface IBrowser
	{
		Task OpenAsync(string uri);
		Task OpenAsync(string uri, BrowserLaunchMode launchMode);
		Task OpenAsync(string uri, BrowserLaunchOptions options);
		Task OpenAsync(Uri uri);
		Task OpenAsync(Uri uri, BrowserLaunchMode launchMode);
		Task<bool> OpenAsync(Uri uri, BrowserLaunchOptions options);
	}
	public interface IClipboard
	{
		Task SetTextAsync(string text);
		Task<string> GetTextAsync();
		bool HasText { get; }
		event EventHandler<EventArgs> ClipboardContentChanged;
	}
	public interface ICompass
	{
		void Start(SensorSpeed sensorSpeed);
		void Start(SensorSpeed sensorSpeed, bool applyLowPassFilter);
		void Stop();
		bool IsMonitoring { get; }
		event EventHandler<CompassChangedEventArgs> ReadingChanged;
	}
	public interface IConnectivity
	{
		NetworkAccess NetworkAccess { get; }
		IEnumerable<ConnectionProfile> ConnectionProfiles { get; }
		event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged;
	}
	public interface IDeviceDisplay
	{
		bool KeepScreenOn { get; set; }
		DisplayInfo MainDisplayInfo { get; }
		event EventHandler<DisplayInfoChangedEventArgs> MainDisplayInfoChanged;
	}
	public interface IDeviceInfo
	{
		string Model { get; }
		string Manufacturer { get; }
		string Name { get; }
		string VersionString { get; }
		Version Version { get; }
		DevicePlatform Platform { get; }
		DeviceIdiom Idiom { get; }
		DeviceType DeviceType { get; }
	}
	public interface IEmail
	{
		Task ComposeAsync();
		Task ComposeAsync(string subject, string body, params string[] to);
		Task ComposeAsync(EmailMessage message);
	}
	public interface IFileSystem
	{
		Task<Stream> OpenAppPackageFileAsync(string filename);
		string CacheDirectory { get; }
		string AppDataDirectory { get; }
	}
	public interface IFlashlight
	{
		Task TurnOnAsync();
		Task TurnOffAsync();
	}
	public interface IGeocoding
	{
		Task<IEnumerable<Placemark>> GetPlacemarksAsync(Location location);
		Task<IEnumerable<Placemark>> GetPlacemarksAsync(double latitude, double longitude);
		Task<IEnumerable<Location>> GetLocationsAsync(string address);
	}
	public interface IGeolocation
	{
		Task<Location> GetLastKnownLocationAsync();
		Task<Location> GetLocationAsync();
		Task<Location> GetLocationAsync(GeolocationRequest request);
		Task<Location> GetLocationAsync(GeolocationRequest request, CancellationToken cancelToken);
	}
	public interface IGyroscope
	{
		void Start(SensorSpeed sensorSpeed);
		void Stop();
		bool IsMonitoring { get; }
		event EventHandler<GyroscopeChangedEventArgs> ReadingChanged;
	}
	public interface ILauncher
	{
		Task<bool> CanOpenAsync(string uri);
		Task<bool> CanOpenAsync(Uri uri);
		Task OpenAsync(string uri);
		Task OpenAsync(Uri uri);
		Task OpenAsync(OpenFileRequest request);
		Task<bool> TryOpenAsync(string uri);
		Task<bool> TryOpenAsync(Uri uri);
	}
	public interface IMagnetometer
	{
		void Start(SensorSpeed sensorSpeed);
		void Stop();
		bool IsMonitoring { get; }
		event EventHandler<MagnetometerChangedEventArgs> ReadingChanged;
	}
	public interface IMainThread
	{
		void BeginInvokeOnMainThread(Action action);
		Task InvokeOnMainThreadAsync(Action action);
		Task<T> InvokeOnMainThreadAsync<T>(Func<T> func);
		Task InvokeOnMainThreadAsync(Func<Task> funcTask);
		Task<T> InvokeOnMainThreadAsync<T>(Func<Task<T>> funcTask);
		Task<SynchronizationContext> GetMainThreadSynchronizationContextAsync();
		bool IsMainThread { get; }
	}
	public interface IMap
	{
		Task OpenAsync(Location location);
		Task OpenAsync(Location location, MapLaunchOptions options);
		Task OpenAsync(double latitude, double longitude);
		Task OpenAsync(double latitude, double longitude, MapLaunchOptions options);
		Task OpenAsync(Placemark placemark);
		Task OpenAsync(Placemark placemark, MapLaunchOptions options);
	}
	public interface IOrientationSensor
	{
		void Start(SensorSpeed sensorSpeed);
		void Stop();
		bool IsMonitoring { get; }
		event EventHandler<OrientationSensorChangedEventArgs> ReadingChanged;
	}
	public interface IPermissions
	{
		Task<PermissionStatus> CheckStatusAsync<TPermission>() where TPermission : Xamarin.Essentials.Permissions.BasePermission, new();
		Task<PermissionStatus> RequestAsync<TPermission>() where TPermission : Xamarin.Essentials.Permissions.BasePermission, new();
	}
	public interface IPhoneDialer
	{
		void Open(string number);
	}
	public interface IPreferences
	{
		bool ContainsKey(string key);
		void Remove(string key);
		void Clear();
		string Get(string key, string defaultValue);
		bool Get(string key, bool defaultValue);
		int Get(string key, int defaultValue);
		double Get(string key, double defaultValue);
		float Get(string key, float defaultValue);
		long Get(string key, long defaultValue);
		void Set(string key, string value);
		void Set(string key, bool value);
		void Set(string key, int value);
		void Set(string key, double value);
		void Set(string key, float value);
		void Set(string key, long value);
		bool ContainsKey(string key, string sharedName);
		void Remove(string key, string sharedName);
		void Clear(string sharedName);
		string Get(string key, string defaultValue, string sharedName);
		bool Get(string key, bool defaultValue, string sharedName);
		int Get(string key, int defaultValue, string sharedName);
		double Get(string key, double defaultValue, string sharedName);
		float Get(string key, float defaultValue, string sharedName);
		long Get(string key, long defaultValue, string sharedName);
		void Set(string key, string value, string sharedName);
		void Set(string key, bool value, string sharedName);
		void Set(string key, int value, string sharedName);
		void Set(string key, double value, string sharedName);
		void Set(string key, float value, string sharedName);
		void Set(string key, long value, string sharedName);
		DateTime Get(string key, DateTime defaultValue);
		void Set(string key, DateTime value);
		DateTime Get(string key, DateTime defaultValue, string sharedName);
		void Set(string key, DateTime value, string sharedName);
    }
	public interface ISecureStorage
	{
		Task<string> GetAsync(string key);
		Task SetAsync(string key, string value);
		bool Remove(string key);
		void RemoveAll();
	}
	public interface IShare
	{
		Task RequestAsync(string text);
		Task RequestAsync(string text, string title);
		Task RequestAsync(ShareTextRequest request);
		Task RequestAsync(ShareFileRequest request);
	}
	public interface ISms
	{
		Task ComposeAsync();
		Task ComposeAsync(SmsMessage message);
	}
	public interface ITextToSpeech
	{
		Task<IEnumerable<Locale>> GetLocalesAsync();
		Task SpeakAsync(string text, CancellationToken cancelToken = default);
		Task SpeakAsync(string text, SpeechOptions options, CancellationToken cancelToken = default);
	}
	public interface IVersionTracking
	{
		void Track();
		bool IsFirstLaunchForVersion(string version);
		bool IsFirstLaunchForBuild(string build);
		bool IsFirstLaunchEver { get; }
		bool IsFirstLaunchForCurrentVersion { get; }
		bool IsFirstLaunchForCurrentBuild { get; }
		string CurrentVersion { get; }
		string CurrentBuild { get; }
		string PreviousVersion { get; }
		string PreviousBuild { get; }
		string FirstInstalledVersion { get; }
		string FirstInstalledBuild { get; }
		IEnumerable<string> VersionHistory { get; }
		IEnumerable<string> BuildHistory { get; }
	}
	public interface IVibration
	{
		void Vibrate();
		void Vibrate(double duration);
		void Vibrate(TimeSpan duration);
		void Cancel();
	}
	public interface IWebAuthenticator
	{
		Task<WebAuthenticatorResult> AuthenticateAsync(Uri url, Uri callbackUrl);
	}
}

//Generated from http://essential-interfaces.azurewebsites.net/
namespace NDB.Covid19.Implementation
{
	public class AccelerometerImplementation : IEssentialsImplementation, IAccelerometer
	{

		[Preserve(Conditional = true)]
		public AccelerometerImplementation() { }

		void IAccelerometer.Start(SensorSpeed sensorSpeed)
		 => Xamarin.Essentials.Accelerometer.Start(sensorSpeed);

		void IAccelerometer.Stop()
			 => Xamarin.Essentials.Accelerometer.Stop();

		bool IAccelerometer.IsMonitoring
			 => Xamarin.Essentials.Accelerometer.IsMonitoring;

		event EventHandler<AccelerometerChangedEventArgs> IAccelerometer.ReadingChanged
		{
			add => Xamarin.Essentials.Accelerometer.ReadingChanged += value;
			remove => Xamarin.Essentials.Accelerometer.ReadingChanged -= value;
		}

		event EventHandler IAccelerometer.ShakeDetected
		{
			add => Xamarin.Essentials.Accelerometer.ShakeDetected += value;
			remove => Xamarin.Essentials.Accelerometer.ShakeDetected -= value;
		}
	}
	public class AppInfoImplementation : IEssentialsImplementation, IAppInfo
	{

		[Preserve(Conditional = true)]
		public AppInfoImplementation() { }

		void IAppInfo.ShowSettingsUI()
		 => Xamarin.Essentials.AppInfo.ShowSettingsUI();

		string IAppInfo.PackageName
			 => Xamarin.Essentials.AppInfo.PackageName;

		string IAppInfo.Name
			 => Xamarin.Essentials.AppInfo.Name;

		string IAppInfo.VersionString
			 => Xamarin.Essentials.AppInfo.VersionString;

		Version IAppInfo.Version
			 => Xamarin.Essentials.AppInfo.Version;

		string IAppInfo.BuildString
			 => Xamarin.Essentials.AppInfo.BuildString;

		AppTheme IAppInfo.RequestedTheme
			 => Xamarin.Essentials.AppInfo.RequestedTheme;
	}
	public class BarometerImplementation : IEssentialsImplementation, IBarometer
	{

		[Preserve(Conditional = true)]
		public BarometerImplementation() { }

		void IBarometer.Start(SensorSpeed sensorSpeed)
		 => Xamarin.Essentials.Barometer.Start(sensorSpeed);

		void IBarometer.Stop()
			 => Xamarin.Essentials.Barometer.Stop();

		bool IBarometer.IsMonitoring
			 => Xamarin.Essentials.Barometer.IsMonitoring;

		event EventHandler<BarometerChangedEventArgs> IBarometer.ReadingChanged
		{
			add => Xamarin.Essentials.Barometer.ReadingChanged += value;
			remove => Xamarin.Essentials.Barometer.ReadingChanged -= value;
		}
	}
	public class BatteryImplementation : IEssentialsImplementation, IBattery
	{

		[Preserve(Conditional = true)]
		public BatteryImplementation() { }

		double IBattery.ChargeLevel
		 => Xamarin.Essentials.Battery.ChargeLevel;

		BatteryState IBattery.State
			 => Xamarin.Essentials.Battery.State;

		BatteryPowerSource IBattery.PowerSource
			 => Xamarin.Essentials.Battery.PowerSource;

		EnergySaverStatus IBattery.EnergySaverStatus
			 => Xamarin.Essentials.Battery.EnergySaverStatus;

		event EventHandler<BatteryInfoChangedEventArgs> IBattery.BatteryInfoChanged
		{
			add => Xamarin.Essentials.Battery.BatteryInfoChanged += value;
			remove => Xamarin.Essentials.Battery.BatteryInfoChanged -= value;
		}

		event EventHandler<EnergySaverStatusChangedEventArgs> IBattery.EnergySaverStatusChanged
		{
			add => Xamarin.Essentials.Battery.EnergySaverStatusChanged += value;
			remove => Xamarin.Essentials.Battery.EnergySaverStatusChanged -= value;
		}
	}
	public class BrowserImplementation : IEssentialsImplementation, IBrowser
	{

		[Preserve(Conditional = true)]
		public BrowserImplementation() { }

		Task IBrowser.OpenAsync(string uri)
		 => Xamarin.Essentials.Browser.OpenAsync(uri);

		Task IBrowser.OpenAsync(string uri, BrowserLaunchMode launchMode)
			 => Xamarin.Essentials.Browser.OpenAsync(uri, launchMode);

		Task IBrowser.OpenAsync(string uri, BrowserLaunchOptions options)
			 => Xamarin.Essentials.Browser.OpenAsync(uri, options);

		Task IBrowser.OpenAsync(Uri uri)
			 => Xamarin.Essentials.Browser.OpenAsync(uri);

		Task IBrowser.OpenAsync(Uri uri, BrowserLaunchMode launchMode)
			 => Xamarin.Essentials.Browser.OpenAsync(uri, launchMode);

		Task<bool> IBrowser.OpenAsync(Uri uri, BrowserLaunchOptions options)
			 => Xamarin.Essentials.Browser.OpenAsync(uri, options);
	}
	public class ClipboardImplementation : IEssentialsImplementation, IClipboard
	{

		[Preserve(Conditional = true)]
		public ClipboardImplementation() { }

		Task IClipboard.SetTextAsync(string text)
		 => Xamarin.Essentials.Clipboard.SetTextAsync(text);

		Task<string> IClipboard.GetTextAsync()
			 => Xamarin.Essentials.Clipboard.GetTextAsync();

		bool IClipboard.HasText
			 => Xamarin.Essentials.Clipboard.HasText;

		event EventHandler<EventArgs> IClipboard.ClipboardContentChanged
		{
			add => Xamarin.Essentials.Clipboard.ClipboardContentChanged += value;
			remove => Xamarin.Essentials.Clipboard.ClipboardContentChanged -= value;
		}
	}
	public class CompassImplementation : IEssentialsImplementation, ICompass
	{

		[Preserve(Conditional = true)]
		public CompassImplementation() { }

		void ICompass.Start(SensorSpeed sensorSpeed)
		 => Xamarin.Essentials.Compass.Start(sensorSpeed);

		void ICompass.Start(SensorSpeed sensorSpeed, bool applyLowPassFilter)
			 => Xamarin.Essentials.Compass.Start(sensorSpeed, applyLowPassFilter);

		void ICompass.Stop()
			 => Xamarin.Essentials.Compass.Stop();

		bool ICompass.IsMonitoring
			 => Xamarin.Essentials.Compass.IsMonitoring;

		event EventHandler<CompassChangedEventArgs> ICompass.ReadingChanged
		{
			add => Xamarin.Essentials.Compass.ReadingChanged += value;
			remove => Xamarin.Essentials.Compass.ReadingChanged -= value;
		}
	}
	public class ConnectivityImplementation : IEssentialsImplementation, IConnectivity
	{

		[Preserve(Conditional = true)]
		public ConnectivityImplementation() { }

		NetworkAccess IConnectivity.NetworkAccess
		 => Xamarin.Essentials.Connectivity.NetworkAccess;

		IEnumerable<ConnectionProfile> IConnectivity.ConnectionProfiles
			 => Xamarin.Essentials.Connectivity.ConnectionProfiles;

		event EventHandler<ConnectivityChangedEventArgs> IConnectivity.ConnectivityChanged
		{
			add => Xamarin.Essentials.Connectivity.ConnectivityChanged += value;
			remove => Xamarin.Essentials.Connectivity.ConnectivityChanged -= value;
		}
	}
	public class DeviceDisplayImplementation : IEssentialsImplementation, IDeviceDisplay
	{

		[Preserve(Conditional = true)]
		public DeviceDisplayImplementation() { }

		bool IDeviceDisplay.KeepScreenOn
		{
			get { return Xamarin.Essentials.DeviceDisplay.KeepScreenOn; }
			set { Xamarin.Essentials.DeviceDisplay.KeepScreenOn = value; }
		}

		DisplayInfo IDeviceDisplay.MainDisplayInfo
			 => Xamarin.Essentials.DeviceDisplay.MainDisplayInfo;

		event EventHandler<DisplayInfoChangedEventArgs> IDeviceDisplay.MainDisplayInfoChanged
		{
			add => Xamarin.Essentials.DeviceDisplay.MainDisplayInfoChanged += value;
			remove => Xamarin.Essentials.DeviceDisplay.MainDisplayInfoChanged -= value;
		}
	}
	public class DeviceInfoImplementation : IEssentialsImplementation, IDeviceInfo
	{

		[Preserve(Conditional = true)]
		public DeviceInfoImplementation() { }

		string IDeviceInfo.Model
		 => Xamarin.Essentials.DeviceInfo.Model;

		string IDeviceInfo.Manufacturer
			 => Xamarin.Essentials.DeviceInfo.Manufacturer;

		string IDeviceInfo.Name
			 => Xamarin.Essentials.DeviceInfo.Name;

		string IDeviceInfo.VersionString
			 => Xamarin.Essentials.DeviceInfo.VersionString;

		Version IDeviceInfo.Version
			 => Xamarin.Essentials.DeviceInfo.Version;

		DevicePlatform IDeviceInfo.Platform
			 => Xamarin.Essentials.DeviceInfo.Platform;

		DeviceIdiom IDeviceInfo.Idiom
			 => Xamarin.Essentials.DeviceInfo.Idiom;

		DeviceType IDeviceInfo.DeviceType
			 => Xamarin.Essentials.DeviceInfo.DeviceType;
	}
	public class EmailImplementation : IEssentialsImplementation, IEmail
	{

		[Preserve(Conditional = true)]
		public EmailImplementation() { }

		Task IEmail.ComposeAsync()
		 => Xamarin.Essentials.Email.ComposeAsync();

		Task IEmail.ComposeAsync(string subject, string body, params string[] to)
			 => Xamarin.Essentials.Email.ComposeAsync(subject, body, to);

		Task IEmail.ComposeAsync(EmailMessage message)
			 => Xamarin.Essentials.Email.ComposeAsync(message);
	}
	public class FileSystemImplementation : IEssentialsImplementation, IFileSystem
	{

		[Preserve(Conditional = true)]
		public FileSystemImplementation() { }

		Task<Stream> IFileSystem.OpenAppPackageFileAsync(string filename)
		 => Xamarin.Essentials.FileSystem.OpenAppPackageFileAsync(filename);

		string IFileSystem.CacheDirectory
			 => Xamarin.Essentials.FileSystem.CacheDirectory;

		string IFileSystem.AppDataDirectory
			 => Xamarin.Essentials.FileSystem.AppDataDirectory;
	}
	public class FlashlightImplementation : IEssentialsImplementation, IFlashlight
	{

		[Preserve(Conditional = true)]
		public FlashlightImplementation() { }

		Task IFlashlight.TurnOnAsync()
		 => Xamarin.Essentials.Flashlight.TurnOnAsync();

		Task IFlashlight.TurnOffAsync()
			 => Xamarin.Essentials.Flashlight.TurnOffAsync();
	}
	public class GeocodingImplementation : IEssentialsImplementation, IGeocoding
	{

		[Preserve(Conditional = true)]
		public GeocodingImplementation() { }

		Task<IEnumerable<Placemark>> IGeocoding.GetPlacemarksAsync(Location location)
		 => Xamarin.Essentials.Geocoding.GetPlacemarksAsync(location);

		Task<IEnumerable<Placemark>> IGeocoding.GetPlacemarksAsync(double latitude, double longitude)
			 => Xamarin.Essentials.Geocoding.GetPlacemarksAsync(latitude, longitude);

		Task<IEnumerable<Location>> IGeocoding.GetLocationsAsync(string address)
			 => Xamarin.Essentials.Geocoding.GetLocationsAsync(address);
	}
	public class GeolocationImplementation : IEssentialsImplementation, IGeolocation
	{

		[Preserve(Conditional = true)]
		public GeolocationImplementation() { }

		Task<Location> IGeolocation.GetLastKnownLocationAsync()
		 => Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();

		Task<Location> IGeolocation.GetLocationAsync()
			 => Xamarin.Essentials.Geolocation.GetLocationAsync();

		Task<Location> IGeolocation.GetLocationAsync(GeolocationRequest request)
			 => Xamarin.Essentials.Geolocation.GetLocationAsync(request);

		Task<Location> IGeolocation.GetLocationAsync(GeolocationRequest request, CancellationToken cancelToken)
			 => Xamarin.Essentials.Geolocation.GetLocationAsync(request, cancelToken);
	}
	public class GyroscopeImplementation : IEssentialsImplementation, IGyroscope
	{

		[Preserve(Conditional = true)]
		public GyroscopeImplementation() { }

		void IGyroscope.Start(SensorSpeed sensorSpeed)
		 => Xamarin.Essentials.Gyroscope.Start(sensorSpeed);

		void IGyroscope.Stop()
			 => Xamarin.Essentials.Gyroscope.Stop();

		bool IGyroscope.IsMonitoring
			 => Xamarin.Essentials.Gyroscope.IsMonitoring;

		event EventHandler<GyroscopeChangedEventArgs> IGyroscope.ReadingChanged
		{
			add => Xamarin.Essentials.Gyroscope.ReadingChanged += value;
			remove => Xamarin.Essentials.Gyroscope.ReadingChanged -= value;
		}
	}
	public class LauncherImplementation : IEssentialsImplementation, ILauncher
	{

		[Preserve(Conditional = true)]
		public LauncherImplementation() { }

		Task<bool> ILauncher.CanOpenAsync(string uri)
		 => Xamarin.Essentials.Launcher.CanOpenAsync(uri);

		Task<bool> ILauncher.CanOpenAsync(Uri uri)
			 => Xamarin.Essentials.Launcher.CanOpenAsync(uri);

		Task ILauncher.OpenAsync(string uri)
			 => Xamarin.Essentials.Launcher.OpenAsync(uri);

		Task ILauncher.OpenAsync(Uri uri)
			 => Xamarin.Essentials.Launcher.OpenAsync(uri);

		Task ILauncher.OpenAsync(OpenFileRequest request)
			 => Xamarin.Essentials.Launcher.OpenAsync(request);

		Task<bool> ILauncher.TryOpenAsync(string uri)
			 => Xamarin.Essentials.Launcher.TryOpenAsync(uri);

		Task<bool> ILauncher.TryOpenAsync(Uri uri)
			 => Xamarin.Essentials.Launcher.TryOpenAsync(uri);
	}
	public class MagnetometerImplementation : IEssentialsImplementation, IMagnetometer
	{

		[Preserve(Conditional = true)]
		public MagnetometerImplementation() { }

		void IMagnetometer.Start(SensorSpeed sensorSpeed)
		 => Xamarin.Essentials.Magnetometer.Start(sensorSpeed);

		void IMagnetometer.Stop()
			 => Xamarin.Essentials.Magnetometer.Stop();

		bool IMagnetometer.IsMonitoring
			 => Xamarin.Essentials.Magnetometer.IsMonitoring;

		event EventHandler<MagnetometerChangedEventArgs> IMagnetometer.ReadingChanged
		{
			add => Xamarin.Essentials.Magnetometer.ReadingChanged += value;
			remove => Xamarin.Essentials.Magnetometer.ReadingChanged -= value;
		}
	}
	public class MainThreadImplementation : IEssentialsImplementation, IMainThread
	{

		[Preserve(Conditional = true)]
		public MainThreadImplementation() { }

		void IMainThread.BeginInvokeOnMainThread(Action action)
		 => Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(action);

		Task IMainThread.InvokeOnMainThreadAsync(Action action)
			 => Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(action);

		Task<T> IMainThread.InvokeOnMainThreadAsync<T>(Func<T> func)
			 => Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync<T>(func);

		Task IMainThread.InvokeOnMainThreadAsync(Func<Task> funcTask)
			 => Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync(funcTask);

		Task<T> IMainThread.InvokeOnMainThreadAsync<T>(Func<Task<T>> funcTask)
			 => Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync<T>(funcTask);

		Task<SynchronizationContext> IMainThread.GetMainThreadSynchronizationContextAsync()
			 => Xamarin.Essentials.MainThread.GetMainThreadSynchronizationContextAsync();

		bool IMainThread.IsMainThread
			 => Xamarin.Essentials.MainThread.IsMainThread;
	}
	public class MapImplementation : IEssentialsImplementation, IMap
	{

		[Preserve(Conditional = true)]
		public MapImplementation() { }

		Task IMap.OpenAsync(Location location)
		 => Xamarin.Essentials.Map.OpenAsync(location);

		Task IMap.OpenAsync(Location location, MapLaunchOptions options)
			 => Xamarin.Essentials.Map.OpenAsync(location, options);

		Task IMap.OpenAsync(double latitude, double longitude)
			 => Xamarin.Essentials.Map.OpenAsync(latitude, longitude);

		Task IMap.OpenAsync(double latitude, double longitude, MapLaunchOptions options)
			 => Xamarin.Essentials.Map.OpenAsync(latitude, longitude, options);

		Task IMap.OpenAsync(Placemark placemark)
			 => Xamarin.Essentials.Map.OpenAsync(placemark);

		Task IMap.OpenAsync(Placemark placemark, MapLaunchOptions options)
			 => Xamarin.Essentials.Map.OpenAsync(placemark, options);
	}
	public class OrientationSensorImplementation : IEssentialsImplementation, IOrientationSensor
	{

		[Preserve(Conditional = true)]
		public OrientationSensorImplementation() { }

		void IOrientationSensor.Start(SensorSpeed sensorSpeed)
		 => Xamarin.Essentials.OrientationSensor.Start(sensorSpeed);

		void IOrientationSensor.Stop()
			 => Xamarin.Essentials.OrientationSensor.Stop();

		bool IOrientationSensor.IsMonitoring
			 => Xamarin.Essentials.OrientationSensor.IsMonitoring;

		event EventHandler<OrientationSensorChangedEventArgs> IOrientationSensor.ReadingChanged
		{
			add => Xamarin.Essentials.OrientationSensor.ReadingChanged += value;
			remove => Xamarin.Essentials.OrientationSensor.ReadingChanged -= value;
		}
	}
	public class PermissionsImplementation : IEssentialsImplementation, IPermissions
	{

		[Preserve(Conditional = true)]
		public PermissionsImplementation() { }

		Task<PermissionStatus> IPermissions.CheckStatusAsync<TPermission>()
		 => Xamarin.Essentials.Permissions.CheckStatusAsync<TPermission>();

		Task<PermissionStatus> IPermissions.RequestAsync<TPermission>()
			 => Xamarin.Essentials.Permissions.RequestAsync<TPermission>();
	}
	public class PhoneDialerImplementation : IEssentialsImplementation, IPhoneDialer
	{

		[Preserve(Conditional = true)]
		public PhoneDialerImplementation() { }

		void IPhoneDialer.Open(string number)
		 => Xamarin.Essentials.PhoneDialer.Open(number);
	}
	public class PreferencesImplementation : IEssentialsImplementation, IPreferences
	{

		[Preserve(Conditional = true)]
		public PreferencesImplementation() { }

		bool IPreferences.ContainsKey(string key)
		 => Xamarin.Essentials.Preferences.ContainsKey(key);

		void IPreferences.Remove(string key)
			 => Xamarin.Essentials.Preferences.Remove(key);

		void IPreferences.Clear()
			 => Xamarin.Essentials.Preferences.Clear();

		string IPreferences.Get(string key, string defaultValue)
			 => Xamarin.Essentials.Preferences.Get(key, defaultValue);

		bool IPreferences.Get(string key, bool defaultValue)
			 => Xamarin.Essentials.Preferences.Get(key, defaultValue);

		int IPreferences.Get(string key, int defaultValue)
			 => Xamarin.Essentials.Preferences.Get(key, defaultValue);

		double IPreferences.Get(string key, double defaultValue)
			 => Xamarin.Essentials.Preferences.Get(key, defaultValue);

		float IPreferences.Get(string key, float defaultValue)
			 => Xamarin.Essentials.Preferences.Get(key, defaultValue);

		long IPreferences.Get(string key, long defaultValue)
			 => Xamarin.Essentials.Preferences.Get(key, defaultValue);

		void IPreferences.Set(string key, string value)
			 => Xamarin.Essentials.Preferences.Set(key, value);

		void IPreferences.Set(string key, bool value)
			 => Xamarin.Essentials.Preferences.Set(key, value);

		void IPreferences.Set(string key, int value)
			 => Xamarin.Essentials.Preferences.Set(key, value);

		void IPreferences.Set(string key, double value)
			 => Xamarin.Essentials.Preferences.Set(key, value);

		void IPreferences.Set(string key, float value)
			 => Xamarin.Essentials.Preferences.Set(key, value);

		void IPreferences.Set(string key, long value)
			 => Xamarin.Essentials.Preferences.Set(key, value);

		bool IPreferences.ContainsKey(string key, string sharedName)
			 => Xamarin.Essentials.Preferences.ContainsKey(key, sharedName);

		void IPreferences.Remove(string key, string sharedName)
			 => Xamarin.Essentials.Preferences.Remove(key, sharedName);

		void IPreferences.Clear(string sharedName)
			 => Xamarin.Essentials.Preferences.Clear(sharedName);

		string IPreferences.Get(string key, string defaultValue, string sharedName)
			 => Xamarin.Essentials.Preferences.Get(key, defaultValue, sharedName);

		bool IPreferences.Get(string key, bool defaultValue, string sharedName)
			 => Xamarin.Essentials.Preferences.Get(key, defaultValue, sharedName);

		int IPreferences.Get(string key, int defaultValue, string sharedName)
			 => Xamarin.Essentials.Preferences.Get(key, defaultValue, sharedName);

		double IPreferences.Get(string key, double defaultValue, string sharedName)
			 => Xamarin.Essentials.Preferences.Get(key, defaultValue, sharedName);

		float IPreferences.Get(string key, float defaultValue, string sharedName)
			 => Xamarin.Essentials.Preferences.Get(key, defaultValue, sharedName);

		long IPreferences.Get(string key, long defaultValue, string sharedName)
			 => Xamarin.Essentials.Preferences.Get(key, defaultValue, sharedName);

		void IPreferences.Set(string key, string value, string sharedName)
			 => Xamarin.Essentials.Preferences.Set(key, value, sharedName);

		void IPreferences.Set(string key, bool value, string sharedName)
			 => Xamarin.Essentials.Preferences.Set(key, value, sharedName);

		void IPreferences.Set(string key, int value, string sharedName)
			 => Xamarin.Essentials.Preferences.Set(key, value, sharedName);

		void IPreferences.Set(string key, double value, string sharedName)
			 => Xamarin.Essentials.Preferences.Set(key, value, sharedName);

		void IPreferences.Set(string key, float value, string sharedName)
			 => Xamarin.Essentials.Preferences.Set(key, value, sharedName);

		void IPreferences.Set(string key, long value, string sharedName)
			 => Xamarin.Essentials.Preferences.Set(key, value, sharedName);

		DateTime IPreferences.Get(string key, DateTime defaultValue)
			 => Xamarin.Essentials.Preferences.Get(key, defaultValue);

		void IPreferences.Set(string key, DateTime value)
			 => Xamarin.Essentials.Preferences.Set(key, value);

		DateTime IPreferences.Get(string key, DateTime defaultValue, string sharedName)
			 => Xamarin.Essentials.Preferences.Get(key, defaultValue, sharedName);

		void IPreferences.Set(string key, DateTime value, string sharedName)
			 => Xamarin.Essentials.Preferences.Set(key, value, sharedName);


	}
	public class SecureStorageImplementation : IEssentialsImplementation, ISecureStorage
	{

		[Preserve(Conditional = true)]
		public SecureStorageImplementation() { }

		Task<string> ISecureStorage.GetAsync(string key)
		 => Xamarin.Essentials.SecureStorage.GetAsync(key);

		Task ISecureStorage.SetAsync(string key, string value)
			 => Xamarin.Essentials.SecureStorage.SetAsync(key, value);

		bool ISecureStorage.Remove(string key)
			 => Xamarin.Essentials.SecureStorage.Remove(key);

		void ISecureStorage.RemoveAll()
			 => Xamarin.Essentials.SecureStorage.RemoveAll();
	}
	public class ShareImplementation : IEssentialsImplementation, IShare
	{

		[Preserve(Conditional = true)]
		public ShareImplementation() { }

		Task IShare.RequestAsync(string text)
		 => Xamarin.Essentials.Share.RequestAsync(text);

		Task IShare.RequestAsync(string text, string title)
			 => Xamarin.Essentials.Share.RequestAsync(text, title);

		Task IShare.RequestAsync(ShareTextRequest request)
			 => Xamarin.Essentials.Share.RequestAsync(request);

		Task IShare.RequestAsync(ShareFileRequest request)
			 => Xamarin.Essentials.Share.RequestAsync(request);
	}
	public class SmsImplementation : IEssentialsImplementation, ISms
	{

		[Preserve(Conditional = true)]
		public SmsImplementation() { }

		Task ISms.ComposeAsync()
		 => Xamarin.Essentials.Sms.ComposeAsync();

		Task ISms.ComposeAsync(SmsMessage message)
			 => Xamarin.Essentials.Sms.ComposeAsync(message);
	}
	public class TextToSpeechImplementation : IEssentialsImplementation, ITextToSpeech
	{

		[Preserve(Conditional = true)]
		public TextToSpeechImplementation() { }

		Task<IEnumerable<Locale>> ITextToSpeech.GetLocalesAsync()
		 => Xamarin.Essentials.TextToSpeech.GetLocalesAsync();

        Task ITextToSpeech.SpeakAsync(string text, CancellationToken cancelToken)
			 => Xamarin.Essentials.TextToSpeech.SpeakAsync(text, cancelToken);

		Task ITextToSpeech.SpeakAsync(string text, SpeechOptions options, CancellationToken cancelToken)
			 => Xamarin.Essentials.TextToSpeech.SpeakAsync(text, options, cancelToken);
	}
	public class VersionTrackingImplementation : IEssentialsImplementation, IVersionTracking
	{

		[Preserve(Conditional = true)]
		public VersionTrackingImplementation() { }

		void IVersionTracking.Track()
		 => Xamarin.Essentials.VersionTracking.Track();

		bool IVersionTracking.IsFirstLaunchForVersion(string version)
			 => Xamarin.Essentials.VersionTracking.IsFirstLaunchForVersion(version);

		bool IVersionTracking.IsFirstLaunchForBuild(string build)
			 => Xamarin.Essentials.VersionTracking.IsFirstLaunchForBuild(build);

		bool IVersionTracking.IsFirstLaunchEver
			 => Xamarin.Essentials.VersionTracking.IsFirstLaunchEver;

		bool IVersionTracking.IsFirstLaunchForCurrentVersion
			 => Xamarin.Essentials.VersionTracking.IsFirstLaunchForCurrentVersion;

		bool IVersionTracking.IsFirstLaunchForCurrentBuild
			 => Xamarin.Essentials.VersionTracking.IsFirstLaunchForCurrentBuild;

		string IVersionTracking.CurrentVersion
			 => Xamarin.Essentials.VersionTracking.CurrentVersion;

		string IVersionTracking.CurrentBuild
			 => Xamarin.Essentials.VersionTracking.CurrentBuild;

		string IVersionTracking.PreviousVersion
			 => Xamarin.Essentials.VersionTracking.PreviousVersion;

		string IVersionTracking.PreviousBuild
			 => Xamarin.Essentials.VersionTracking.PreviousBuild;

		string IVersionTracking.FirstInstalledVersion
			 => Xamarin.Essentials.VersionTracking.FirstInstalledVersion;

		string IVersionTracking.FirstInstalledBuild
			 => Xamarin.Essentials.VersionTracking.FirstInstalledBuild;

		IEnumerable<string> IVersionTracking.VersionHistory
			 => Xamarin.Essentials.VersionTracking.VersionHistory;

		IEnumerable<string> IVersionTracking.BuildHistory
			 => Xamarin.Essentials.VersionTracking.BuildHistory;
	}
	public class VibrationImplementation : IEssentialsImplementation, IVibration
	{

		[Preserve(Conditional = true)]
		public VibrationImplementation() { }

		void IVibration.Vibrate()
		 => Xamarin.Essentials.Vibration.Vibrate();

		void IVibration.Vibrate(double duration)
			 => Xamarin.Essentials.Vibration.Vibrate(duration);

		void IVibration.Vibrate(TimeSpan duration)
			 => Xamarin.Essentials.Vibration.Vibrate(duration);

		void IVibration.Cancel()
			 => Xamarin.Essentials.Vibration.Cancel();
	}
	public class WebAuthenticatorImplementation : IEssentialsImplementation, IWebAuthenticator
	{

		[Preserve(Conditional = true)]
		public WebAuthenticatorImplementation() { }

		Task<WebAuthenticatorResult> IWebAuthenticator.AuthenticateAsync(Uri url, Uri callbackUrl)
		 => Xamarin.Essentials.WebAuthenticator.AuthenticateAsync(url, callbackUrl);
	}
}