using System;
using System.Collections.Generic;
using NDB.Covid19.Interfaces;

namespace NDB.Covid19.Test.Mocks
{
    public class TestPreferencesMock : IPreferences
    {
        private readonly Dictionary<string, string> _dictString;
        private readonly Dictionary<string, DateTime> _dictDateTime;
        private readonly Dictionary<string, int> _dictInt;
        private readonly Dictionary<string, bool> _dictBool;
        private readonly Dictionary<string, double> _dictDouble;


        public TestPreferencesMock()
        {
            _dictString = new Dictionary<string, string>();
            _dictDateTime = new Dictionary<string, DateTime>();
            _dictInt = new Dictionary<string, int>();
            _dictBool = new Dictionary<string, bool>();
            _dictDouble = new Dictionary<string, double>();
        }

        public void Clear()
        {
            _dictString.Clear();
            _dictDateTime.Clear();
            _dictInt.Clear();
            _dictBool.Clear();
            _dictDouble.Clear();
        }

        public void Clear(string sharedName)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {

            if (_dictString.ContainsKey(key))
                return true;
            if (_dictDateTime.ContainsKey(key))
                return true;
            if (_dictInt.ContainsKey(key))
                return true;
            if (_dictBool.ContainsKey(key))
                return true;

            return false;
        }

        public bool ContainsKey(string key, string sharedName)
        {
            throw new NotImplementedException();
        }

        public string Get(string key, string defaultValue)
        {
            try
            {
                return _dictString[key];
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public bool Get(string key, bool defaultValue)
        {
            try
            {
                return _dictBool[key];
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public int Get(string key, int defaultValue)
        {
            try
            {
                return _dictInt[key];

            }
            catch (Exception)
            {
                return default;
            }
        }

        public double Get(string key, double defaultValue)
        {
            try
            {
                return _dictDouble[key];

            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public float Get(string key, float defaultValue)
        {
            throw new NotImplementedException();
        }

        public long Get(string key, long defaultValue)
        {
            throw new NotImplementedException();
        }

        public string Get(string key, string defaultValue, string sharedName)
        {
            throw new NotImplementedException();
        }

        public bool Get(string key, bool defaultValue, string sharedName)
        {
            throw new NotImplementedException();
        }

        public int Get(string key, int defaultValue, string sharedName)
        {
            throw new NotImplementedException();
        }

        public double Get(string key, double defaultValue, string sharedName)
        {
            throw new NotImplementedException();
        }

        public float Get(string key, float defaultValue, string sharedName)
        {
            throw new NotImplementedException();
        }

        public long Get(string key, long defaultValue, string sharedName)
        {
            throw new NotImplementedException();
        }

        public DateTime Get(string key, DateTime defaultValue)
        {
            try
            {
                return _dictDateTime[key];
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public DateTime Get(string key, DateTime defaultValue, string sharedName)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            _dictDateTime.Remove(key);
            _dictString.Remove(key);
        }

        public void Remove(string key, string sharedName)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, string value)
        {
            _dictString[key] = value;
        }

        public void Set(string key, bool value)
        {
            _dictBool[key] = value;
        }

        public void Set(string key, int value)
        {
            _dictInt[key] = value;
        }

        public void Set(string key, double value)
        {
            _dictDouble[key] = value;
        }

        public void Set(string key, float value)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, long value)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, string value, string sharedName)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, bool value, string sharedName)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, int value, string sharedName)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, double value, string sharedName)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, float value, string sharedName)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, long value, string sharedName)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, DateTime value)
        {
            _dictDateTime[key] = value;
        }

        public void Set(string key, DateTime value, string sharedName)
        {
            throw new NotImplementedException();
        }
    }
}
