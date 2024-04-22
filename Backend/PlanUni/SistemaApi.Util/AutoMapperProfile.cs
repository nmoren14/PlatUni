using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SistemaApi.DTO;
using SistemaApi.Model;
using SistemaApi.Model.Models;

namespace SistemaUni.Util
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            #region Student
            CreateMap<Student,StudentDTO>().ReverseMap();
            #endregion

            #region Course
            CreateMap<Course, CourseDTO>().ReverseMap();
            #endregion

            #region Professor
            CreateMap<Professor, ProfessorDTO>()
                .ForMember(destino =>
                    destino.FisrtClassDescrip,
                     opt => opt.MapFrom(origen => origen.FisrtClassNavigation.CourseName))
                .ForMember(destino =>
                    destino.SecondClassDecrip,
                     opt => opt.MapFrom(origen => origen.SecondClassNavigation.CourseName));
            #endregion

            #region Enrollment
            CreateMap<Enrollment, EnrollmentDTO>().ReverseMap();
            #endregion
        }
    }
}
