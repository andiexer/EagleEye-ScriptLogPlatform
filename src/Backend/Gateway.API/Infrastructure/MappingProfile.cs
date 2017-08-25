using AutoMapper;
using EESLP.Backend.Gateway.API.Entities;
using EESLP.Backend.Gateway.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Backend.Gateway.API.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ScriptInstance, ScriptInstanceViewModel>();
            CreateMap<Script, ScriptInstanceViewModel>()
                .ForMember(d => d.Script, a => a.MapFrom(s => s.Id));
            CreateMap<Host, ScriptInstanceViewModel>()
                .ForMember(d => d.Host, a => a.MapFrom(s => s.Id));
        }
    }
}
