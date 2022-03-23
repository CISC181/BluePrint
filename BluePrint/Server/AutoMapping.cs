using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BluePrint.Shared.DTO;
using BluePrint.Shared.Models;
using BluePrint.Server.Areas.Identity.CustomProvider;

namespace BluePrint.Server
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<AspNetUser, ApplicationUser>();
            CreateMap<ApplicationUser, AspNetUser>();

            CreateMap<Student, StudentDto>();
            CreateMap<StudentDto, Student>();
        }
    }
}
