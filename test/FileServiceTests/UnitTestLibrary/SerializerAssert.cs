using System.Text.Json;
using NUnit.Framework;

namespace LT.DigitalOffice.FileServiceUnitTests.UnitTestLibrary
{
    public static class SerializerAssert
    {
        public static void AreEqual(object expected, object actual)
        {
            var expectedJson = JsonSerializer.Serialize(expected);
            var actualJson = JsonSerializer.Serialize(actual);

            Assert.AreEqual(expectedJson, actualJson);
        }
    }
}