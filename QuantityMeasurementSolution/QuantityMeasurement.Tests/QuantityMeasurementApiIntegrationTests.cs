using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuantityMeasurement.Tests
{
    [TestClass]
    public class QuantityMeasurementApiIntegrationTests
    {
        [TestMethod]
        public async Task Api_InvalidAddRequest_ReturnsStructuredBadRequest()
        {
            await using var factory = new WebApplicationFactory<Program>();
            using var client = factory.CreateClient();

            var payload = new
            {
                firstQuantity = new { value = 1.0, unit = "FOOT", measurementType = "Length" },
                secondQuantity = new { value = 12.0, unit = "INCHES", measurementType = "Length" },
                targetUnit = "FEET"
            };

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/QuantityMeasurement/add", payload);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            string json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            Assert.AreEqual(400, doc.RootElement.GetProperty("status").GetInt32());
            Assert.AreEqual("Quantity Measurement Error", doc.RootElement.GetProperty("error").GetString());
            StringAssert.Contains(doc.RootElement.GetProperty("path").GetString() ?? string.Empty, "/api/QuantityMeasurement/add");
        }

        [TestMethod]
        public async Task Api_HistoryAndCountEndpoints_WorkAfterOperation()
        {
            await using var factory = new WebApplicationFactory<Program>();
            using var client = factory.CreateClient();

            var comparePayload = new
            {
                firstQuantity = new { value = 12.0, unit = "INCHES", measurementType = "Length" },
                secondQuantity = new { value = 1.0, unit = "FEET", measurementType = "Length" }
            };

            HttpResponseMessage compareResponse = await client.PostAsJsonAsync("/api/QuantityMeasurement/compare", comparePayload);
            Assert.AreEqual(HttpStatusCode.OK, compareResponse.StatusCode);

            HttpResponseMessage historyResponse = await client.GetAsync("/api/QuantityMeasurement/history/operation/COMPARE");
            Assert.AreEqual(HttpStatusCode.OK, historyResponse.StatusCode);

            HttpResponseMessage countResponse = await client.GetAsync("/api/QuantityMeasurement/count/COMPARE");
            Assert.AreEqual(HttpStatusCode.OK, countResponse.StatusCode);

            string historyJson = await historyResponse.Content.ReadAsStringAsync();
            using var historyDoc = JsonDocument.Parse(historyJson);
            var historyData = historyDoc.RootElement.GetProperty("data");
            Assert.IsGreaterThanOrEqualTo(historyData.GetArrayLength(), 1);

            string countJson = await countResponse.Content.ReadAsStringAsync();
            using var countDoc = JsonDocument.Parse(countJson);
            int count = countDoc.RootElement.GetProperty("data").GetInt32();
            Assert.IsGreaterThanOrEqualTo(count, 1);
        }

        [TestMethod]
        public async Task Api_ErroredHistoryEndpoint_ReturnsList()
        {
            await using var factory = new WebApplicationFactory<Program>();
            using var client = factory.CreateClient();

            var invalidPayload = new
            {
                firstQuantity = new { value = 1.0, unit = "FEET", measurementType = "Length" },
                secondQuantity = new { value = 1.0, unit = "KILOGRAM", measurementType = "Weight" },
                targetUnit = "FEET"
            };

            _ = await client.PostAsJsonAsync("/api/QuantityMeasurement/add", invalidPayload);

            HttpResponseMessage historyResponse = await client.GetAsync("/api/QuantityMeasurement/history/errored");
            Assert.AreEqual(HttpStatusCode.OK, historyResponse.StatusCode);

            string json = await historyResponse.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            Assert.AreEqual(JsonValueKind.Array, doc.RootElement.GetProperty("data").ValueKind);
        }
    }
}
