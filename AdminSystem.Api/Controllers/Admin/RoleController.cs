using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminSystem.IRepositories.Admin;
using AdminSystem.Models.Admin.AdminModels.Model;
using AdminSystem.Models.Admin.AdminModels.ModelCreation;
using AdminSystem.Models.Admin.AdminModels.ModelModification;
using AdminSystem.Models.Admin.AdminModels.ModelView;
using AdminSystem.Models.Admin.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using IActionResult = Microsoft.AspNetCore.Mvc.IActionResult;

namespace AdminSystem.Api.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public RoleController(IRoleRepository roleRepository, IMapper mapper, ILogger<RoleController> logger, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _roleRepository.All.ToListAsync();
            var results = _mapper.Map<IEnumerable<RoleView>>(items);
            return Ok(results);
        }

        [HttpGet]
        [Route("{id}", Name = "GetRole")]
        public async Task<IActionResult> GetRole(string id)
        {
            var item = await _roleRepository.GetSingleAsync(x => x.RoleId == id);
            if (item == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<RoleView>(item);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostRole([FromBody] RoleCreation roleCreation)
        {
            if (roleCreation == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newItem = _mapper.Map<Role>(roleCreation);
            _roleRepository.Add(newItem);
            if (!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, "添加角色出错");
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(string id, [FromBody] RoleModification roleModification)
        {
            if (roleModification == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dbItem = await _roleRepository.GetSingleAsync(x => x.RoleId == id);
            if (dbItem == null)
            {
                return NotFound();
            }
            _mapper.Map(roleModification, dbItem);
            _roleRepository.Update(dbItem);
            if (!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, "修改角色出错");
            }
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchRole(string id, [FromBody] JsonPatchDocument<RoleModification> jsonPatchDoc)
        {
            if (jsonPatchDoc == null)
            {
                return BadRequest();
            }
            var dbItem = await _roleRepository.GetSingleAsync(x => x.RoleId == id);
            if (dbItem == null)
            {
                return NotFound();
            }
            var toPatchVm = Mapper.Map<RoleModification>(dbItem);
            jsonPatchDoc.ApplyTo(toPatchVm, ModelState);

            TryValidateModel(toPatchVm);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(toPatchVm, dbItem);

            if (!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, "更新的时候出错");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await _roleRepository.GetSingleAsync(x=>x.RoleId == id);
            if (model == null)
            {
                return NotFound();
            }
            _roleRepository.Delete(model);
            if (!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, "删除的时候出错");
            }
            return NoContent();
        }
    }
}