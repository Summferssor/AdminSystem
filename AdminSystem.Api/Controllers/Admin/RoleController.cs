using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminSystem.IRepositories.Admin;
using AdminSystem.Models.Admin.AdminModels.ModelModification;
using AdminSystem.Models.Admin.AdminModels.ModelView;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminSystem.Api.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleController(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }
        [HttpPost]
        public IActionResult AddRole([FromBody] RoleModification roleModification)
        {
            try
            {
                var newRole = _mapper.Map<Role>(roleModification);
                _roleRepository.Add(newRole);
                if (!_roleRepository.Save())
                {
                    return BadRequest("222");
                }
                return Ok("123");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}