using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BluePrint.Shared.DTO;
using BluePrint.Shared.Models;

namespace BluePrint.Server
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Student, StudentDto>();
            CreateMap<StudentDto, Student>();
        }
    }
}
