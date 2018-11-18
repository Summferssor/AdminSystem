using AdminSystem.Models.Admin.AdminModels.ModelCreation;
using AdminSystem.Models.Admin.AdminModels.ModelModification;
using AdminSystem.Models.Admin.AdminModels.ModelView;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminSystem.Api.MapConfigurations
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName => "DomainToViewModelMappings";
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Role, RoleModification>();
            CreateMap<Role, RoleCreation>();
        }
    }
}
