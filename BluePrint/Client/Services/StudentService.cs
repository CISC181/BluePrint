using BluePrint.Shared;
using BluePrint.Shared.DTO;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Telerik.DataSource;

namespace BluePrint.Client.Services
{
    public class StudentService
    {
        protected HttpClient Http { get; set; }

        public StudentService(HttpClient client)
        {
            Http = client;
        }

        public async Task<DataEnvelope<StudentDto>> GetStudentListAsync(DataSourceRequest gridRequest)
        {
            //options very important here !!!!!!!!!!
            var options = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                PropertyNameCaseInsensitive = true
            };
            HttpResponseMessage response = await Http.PostAsJsonAsync("api/student/GetStudentData", gridRequest);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var JsonData = await response.Content.ReadFromJsonAsync<DataEnvelope<StudentDto>>(options);
                return JsonData;
            }

            throw new Exception($"The service returned with status {response.StatusCode}");
        }
    }
}
