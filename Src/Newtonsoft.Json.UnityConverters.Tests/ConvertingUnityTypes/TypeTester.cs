﻿using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Newtonsoft.Json.UnityConverters.Tests.ConvertingUnityTypes
{
    public abstract class TypeTesterBase
    {
        protected virtual void ConfigureSettings(JsonSerializerSettings settings)
        {
            // No settings changes by default
        }

        protected JsonSerializerSettings GetSettings()
        {
            JsonSerializerSettings settings = UnityTypeConverterInitializer.GetUnityJsonSerializerSettings();
            settings.Formatting = Formatting.None;
            ConfigureSettings(settings);
            return settings;
        }

        protected TypeTesterBase()
        {
        }
    }

    public abstract class TypeTester<T> : TypeTesterBase
    {
        [Test]
        [TestCaseSource("representations")]
        public void SerializesAsExpected((T input, object anonymous) representation)
        {
            // Arrange
            JsonSerializerSettings settings = GetSettings();
            string expected = JObject.FromObject(representation.anonymous).ToString(Formatting.None);

            // Act
            string result = JsonConvert.SerializeObject(representation.input, settings);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCaseSource("representations")]
        public void DeserializesAsExpected((T expected, object anonymous) representation)
        {
            // Arrange
            JsonSerializerSettings settings = GetSettings();
            string input = JObject.FromObject(representation.anonymous).ToString(Formatting.None);

            // Act
            T result = JsonConvert.DeserializeObject<T>(input, settings);

            // Assert
            Assert.AreEqual(representation.expected, result);
        }

        [Test]
        [TestCaseSource("representations")]
        public void SerializesArrayAsExpected((T input, object anonymous) representation)
        {
            // Arrange
            JsonSerializerSettings settings = GetSettings();
            string expected = JArray.FromObject(new [] { representation.anonymous }).ToString(Formatting.None);
            T[] input = new[] { representation.input };

            // Act
            string result = JsonConvert.SerializeObject(input, settings);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCaseSource("representations")]
        public void DeserializesArrayAsExpected((T expected, object anonymous) representation)
        {
            // Arrange
            JsonSerializerSettings settings = GetSettings();
            string input = JArray.FromObject(new [] { representation.anonymous }).ToString(Formatting.None);

            // Act
            T[] result = JsonConvert.DeserializeObject<T[]>(input, settings);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(representation.expected, result[0]);
        }
    }
}