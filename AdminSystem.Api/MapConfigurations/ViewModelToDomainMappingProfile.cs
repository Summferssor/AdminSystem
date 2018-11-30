using AdminSystem.Models.Admin.AdminModels.Model;
using AdminSystem.Models.Admin.AdminModels.ModelModification;
using AdminSystem.Models.Admin.AdminModels.ModelView;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminSystem.Api.MapConfigurations
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName => "ViewModelToDomainMappings";
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<RoleModification, Role>();
            CreateMap<RoleView, Role>();
            CreateMap<RoleModification, Role>();
        }
    }
}
