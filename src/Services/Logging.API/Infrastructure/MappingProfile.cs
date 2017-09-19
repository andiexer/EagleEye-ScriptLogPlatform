using AutoMapper;
using EESLP.Services.Logging.API.Entities;
using EESLP.Services.Logging.API.Enums;
using EESLP.Services.Logging.API.ViewModel;
using EESLP.Services.Logging.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Services.Logging.API.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ScriptInstanceAddModel, ScriptInstance>();
            CreateMap<ScriptInstance, ScriptInstanceViewModel>();
            CreateMap<LogAddModel, Log>();
            CreateMap<Log, LogViewModel>();
            CreateMap<ScriptAddModel, Script>();
            CreateMap<Script, ScriptViewModel>();
            CreateMap<ScriptUpdateModel, Script>()
                .ForAllMembers(opt => opt.Condition(
                    (source, destination, sourceMember, destMember) => (sourceMember != null)));
            CreateMap<Host, HostViewModel>();
            CreateMap<HostAddModel, Host>();
            CreateMap<HostUpdateModel, Host>()
                .ForAllMembers(opt => opt.Condition(
                    (source, destination, sourceMember, destMember) => (sourceMember != null)));
        }
    }
}
