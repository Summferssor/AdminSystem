using AdminSystem.Common;
using AdminSystem.IRepositories.Admin;
using AdminSystem.Models.Admin.AdminModels.ModelView;
using AdminSystem.Models.Admin.MyDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdminSystem.Repositories.Admin
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AdminDbContext _context;
        public RoleRepository(AdminDbContext adminDbContext)
        {
            _context = adminDbContext;
        }
        public void Add(Role role)
        {
            try
            {
                role.RoleId = Method.GetGuid32();
                //_context.Stu.Add(entity);
                _context.Entry<Role>(role).State = EntityState.Added;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<Role> GetAll()
        {
            try
            {
                var List = from roles in _context.Role
                               select roles;
                return List.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Save()
        {

            try
            {
                return _context.SaveChanges() >= 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}
