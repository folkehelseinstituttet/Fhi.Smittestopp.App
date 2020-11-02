using System;
using System.Collections.Generic;
using NDB.Covid19.Interfaces;
using SQLite;
using Xamarin.Essentials;

namespace NDB.Covid19.Test.Mocks
{
    public class TestConnectivityMocks : IConnectivity
    {

        [Preserve(Conditional = true)]
        public TestConnectivityMocks() { }

        NetworkAccess IConnectivity.NetworkAccess
            => NetworkAccess.Internet;

        IEnumerable<ConnectionProfile> IConnectivity.ConnectionProfiles
            => new List<ConnectionProfile>{ ConnectionProfile.WiFi };

        event EventHandler<ConnectivityChangedEventArgs> IConnectivity.ConnectivityChanged
        {
            add => Connectivity.ConnectivityChanged += value;
            remove => Connectivity.ConnectivityChanged -= value;
        }
    }

}