using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using CleanCityCore.Model;
using Newtonsoft.Json.Linq;

namespace CleanCityCore
{
    public class ResponsibleFounder : IResponsibleFounder
    {
        private readonly IResponsibleRepository responsibleRepository;
        private string geoLocationApiUri = "https://екатеринбург.рф/data-send/quarterly/searchqlybymap";
        private string fetchApiUri = "https://екатеринбург.рф/data-send/quarterly/qlydata";
        private string fetchDistrictsUri = "https://екатеринбург.рф/data-send/quarterly/quarterly";
        private string fetchSingleDistrictUri = "https://екатеринбург.рф/data-send/quarterly/qly2district";
        private static readonly HttpClient HttpClient = new HttpClient();

        public ResponsibleFounder(IResponsibleRepository responsibleRepository)
        {
            this.responsibleRepository = responsibleRepository;
        }

        public Responsible[] GetAllResponsibles()
        {
            var districts = GetDistricts();
            return districts.SelectMany(GetDistrictResponsibles).ToArray();
        }

        private Responsible[] GetDistrictResponsibles(Guid districtId)
        {
            var responsibles = PostData(fetchSingleDistrictUri, new Dictionary<string, string>
            {
                {"district_id", districtId.ToString()}
            });
            Console.WriteLine($"Processing district: {districtId}");
            var ids = responsibles["data"]["qly"]["list"].ToArray().Select(d => Guid.Parse(d["userId"].ToString()))
                .ToArray();
            return ids.Select(GetResponsible).ToArray();
        }

        private Guid[] GetDistricts()
        {
            var districts = PostData(fetchDistrictsUri, new Dictionary<string, string>());
            return districts["data"]["districts"].ToArray().Select(d => Guid.Parse(d["id"].ToString())).ToArray();
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
            Console.WriteLine($"Get responsible with id: {responsibleId}");
            var values = new Dictionary<string, string>
            {
                {"user_id", responsibleId.ToString()},
                {"page", "1"}
            };
            var data = PostData(fetchApiUri, values);
            Console.WriteLine(data);
            return new Responsible
            {
                Name = data["data"]["qly"]["name"].ToString(),
                Email = data["data"]["qly"]["email"].ToString(),
                ResponseRegion = $"{data["data"]["qly"]["districtTitle"]}, {data["data"]["qly"]["quarterTitle"]}",
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