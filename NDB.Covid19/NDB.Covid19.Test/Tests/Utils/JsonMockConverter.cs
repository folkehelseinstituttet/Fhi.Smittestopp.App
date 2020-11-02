using System;
using System.Collections.Generic;
using Moq;
using Newtonsoft.Json;

namespace NDB.Covid19.Test.Tests.Utils
{
    public class JsonMockConverter : JsonConverter
    {
        private static readonly Dictionary<object, Func<object>> MockSerializers =
            new Dictionary<object, Func<object>>();

        private static readonly HashSet<Type> MockTypes = new HashSet<Type>();

        public static void RegisterMock<T>(Mock<T> mock, Func<object> serializer) where T : class
        {
            MockSerializers[mock.Object] = serializer;
            MockTypes.Add(mock.Object.GetType());
        }

        public override bool CanConvert(Type objectType) => MockTypes.Contains(objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer) =>
            throw new NotImplementedException();

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!MockSerializers.TryGetValue(value, out var mockSerializer))
            {
                throw new InvalidOperationException("Attempt to serialize unregistered mock.");
            }

            serializer.Serialize(writer, mockSerializer());
        }
    }
}