﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BluePrint.Client.Services;
using BluePrint.Shared;
using BluePrint.Shared.DTO;
using BluePrint.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
namespace BluePrint.Client.Pages.Student
{
    public partial class StudentList : ComponentBase
    {

        [Inject]
        protected HttpClient Http { get; set; }

        [Inject]
        protected IJSRuntime Js { get; set; }

        [Inject]
        NavigationManager NavManager { get; set; }

        [Inject]
        StudentService StudentService { get; set; }

        private IEnumerable<StudentDto> Students { get; set; }

        //private SPARC.Shared.Models.Prjct CurrentlyEditedProject { get; set; }
        private bool IsLoading { get; set; }
        public int Total { get; set; } = 0;
        private bool ExportAllPages { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //await GetProjects();
            //await GetProjectTypes();
            await base.OnInitializedAsync();
        }

        private async Task GetStudents()
        {
            IsLoading = true;
            var options = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                PropertyNameCaseInsensitive = true
            };
            Students = await Http.GetFromJsonAsync<IEnumerable<StudentDto>>("api/student", options);
            IsLoading = false;
        }

        //private async Task GetProjectTypes()
        //{
        //    var options = new JsonSerializerOptions()
        //    {
        //        ReferenceHandler = ReferenceHandler.Preserve,
        //        PropertyNameCaseInsensitive = true
        //    };

        //    ProjectTypes = await Http.GetFromJsonAsync<IEnumerable<SPARC.Shared.Models.PrjctType>>("api/project/projecttypes", options);
        //}

        protected async Task ReadItems(GridReadEventArgs args)
        {
            IsLoading = true;
            DataEnvelope<StudentDto> result = await StudentService.GetStudentListAsync(args.Request);

            if (args.Request.Groups.Count > 0)
            {
                /***
                NO GROUPING FOR THE TIME BEING
                var data = GroupDataHelpers.DeserializeGroups<WeatherForecast>(result.GroupedData);
                GridData = data.Cast<object>().ToList();
                ***/
            }
            else
            {
                Students = result.CurrentPageData.ToList();
            }

            Total = result.TotalItemCount;
            IsLoading = false;

            StateHasChanged();
        }

        private void Detail(GridCommandEventArgs e)
        {
            StudentDto stu = e.Item as StudentDto;
            NavManager.NavigateTo($"student/detail/{stu.StudentId}");

        }

        private async Task CreateItem(GridCommandEventArgs e)
        {
           StudentDto stu = e.Item as StudentDto;
            await Http.PostAsJsonAsync("api/student", stu);

            await GetStudents();
        }

        private async Task UpdateItem(GridCommandEventArgs e)
        {
            StudentDto stu = e.Item as StudentDto;
            var options = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                PropertyNameCaseInsensitive = true
            };

            await Http.PutAsJsonAsync($"api/student", stu, options);

            //await GetStudents();
        }

        private async Task DeleteItem(GridCommandEventArgs e)
        {
            StudentDto stu = e.Item as StudentDto;
            await Http.DeleteAsync($"api/stu/{stu.StudentId}");

            await GetStudents();
        }
    }
}