using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BluePrint.Shared;
using BluePrint.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource;
using BluePrint.EF;
using Microsoft.EntityFrameworkCore;
using Telerik.DataSource.Extensions;
using Microsoft.AspNetCore.Http;
using BluePrint.Shared.Models;
using Telerik.Blazor.Components;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;

namespace BluePrint.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : Controller
    {
        private readonly BluePrintOracleContext _context;
        private readonly IMapper _mapper;
        public StudentController(BluePrintOracleContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpPut]
        public async Task<IActionResult> Put(StudentDto speaker)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var sdnt = await _context.Students.SingleAsync(s => s.StudentId == speaker.StudentId);
                _context.Entry(sdnt).CurrentValues.SetValues(speaker);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return NoContent();
        }

        //[HttpPut]
        //public async Task<IActionResult> Post([FromBody] StudentDto stu)
        //{
        //    Student student = _context.Students.Where(x => x.StudentId == stu.StudentId).FirstOrDefault();
        //    _context.Entry(student).CurrentValues.SetValues(stu);
        //    _context.SaveChanges();

        //    return NoContent();
        //}

        [HttpGet]
        [Route("salutations")]
        public async Task<IActionResult> GetSalutations()
        {
            var lstSalutations = await _context.Salutations.ToListAsync();
            return Ok(lstSalutations);
        }

        [HttpPost]
        [Route("GetStudentData")]
        public async Task<DataEnvelope<StudentDto>> Post([FromBody] DataSourceRequest gridRequest)
        {
            DataEnvelope<StudentDto> dataToReturn = null;

            //ICollection<Student> studentsEF = await _context.Students.Include(x => x.Salutation).ToListAsync();
            
            //var speakerDTO = _mapper.Map<StudentDto>(studentsEF);

            ICollection<StudentDto> students = await _context.Students.Include(x=>x.Salutation)
                .Select(sp => new StudentDto
                {
                    StudentId = sp.StudentId,
                    FirstName = sp.FirstName,
                    LastName = sp.LastName,
                    Phone = sp.Phone,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    Employer = sp.Employer,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    RegistrationDate = sp.RegistrationDate,
                    SalutationId = sp.SalutationId,
                    Salutation = sp.Salutation.Salutation1,
                    StreetAddress = sp.StreetAddress,
                    Zip = sp.Zip
                }).ToListAsync();
            DataSourceResult processedData = await students.ToDataSourceResultAsync(gridRequest);

            if (gridRequest.Groups.Count > 0)
            {
                // If there is grouping, use the field for grouped data
                // The app must be able to serialize and deserialize it
                // Example helper methods for this are available in this project
                // See the GroupDataHelper.DeserializeGroups and JsonExtensions.Deserialize methods
                dataToReturn = new DataEnvelope<StudentDto>
                {
                    GroupedData = processedData.Data.Cast<AggregateFunctionsGroup>().ToList(),
                    TotalItemCount = processedData.Total
                };
            }
            else
            {
                // When there is no grouping, the simplistic approach of 
                // just serializing and deserializing the flat data is enough
                dataToReturn = new DataEnvelope<StudentDto>
                {
                    CurrentPageData = processedData.Data.Cast<StudentDto>().ToList(),
                    TotalItemCount = processedData.Total
                };
            }

            return dataToReturn;

        }


    }

}
