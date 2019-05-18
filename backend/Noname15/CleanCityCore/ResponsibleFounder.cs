using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using CleanCityCore.Model;
using CleanCityCore.Sql;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CleanCityCore
{
    public class ResponsibleFounder : IResponsibleFounder
    {
        private readonly IResponsibleRepository responsibleRepository;
        private string geoLocationApiUri = "https://екатеринбург.рф/data-send/quarterly/searchqlybymap";
        private string fetchApiUri = "https://екатеринбург.рф/data-send/quarterly/qlydata";
        private static readonly HttpClient HttpClient = new HttpClient();

        public ResponsibleFounder(IResponsibleRepository responsibleRepository)
        {
            this.responsibleRepository = responsibleRepository;
        }

        public Responsible GetResponsible(GeoLocation location)
        {
            var responsibleId = GetResponsibleId(location);
            var storedResponsible = responsibleRepository.ReadResponsible(responsibleId);
            if (storedResponsible != null)
                return storedResponsible;
            var responsible = GetResponsible(responsibleId);
            responsibleRepository.AddResponsible(responsible);
            return responsible;
        }

        private Responsible GetResponsible(Guid responsibleId)
        {
            var values = new Dictionary<string, string>
            {
                {"user_id", responsibleId.ToString()},
                {"page", "1"}
            };
            var data = PostData(fetchApiUri, values);
            return new Responsible
            {
                Name = data["data"]["qly"]["name"].ToString(),
                Email = data["data"]["qly"]["email"].ToString(),
                Id = responsibleId,
                IsActive = true,
            };
        }

        private Guid GetResponsibleId(GeoLocation location)
        {
            var data = PostData(geoLocationApiUri, new Dictionary<string, string>
            {
                {"lat", location.Latitude.ToString(CultureInfo.InvariantCulture)},
                {"lng", location.Longitude.ToString(CultureInfo.InvariantCulture)},
            });
            return Guid.Parse(data["data"]["id"].ToString());
        }

        private JObject PostData(string uri, Dictionary<string, string> data)
        {
            var response = HttpClient.PostAsync(uri, new FormUrlEncodedContent(data)).GetAwaiter().GetResult();
            var responseText = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JObject.Parse(responseText);
        }
    }
}