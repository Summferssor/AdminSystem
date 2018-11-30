using AdminSystem.Common;
using AdminSystem.IRepositories.Admin;
using AdminSystem.Models.Admin.AdminModels.Model;
using AdminSystem.Models.Admin.AdminModels.ModelView;
using AdminSystem.Models.Admin.Infrastructure;
using AdminSystem.Models.Admin.MyDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Repositories.Admin
{
    public class RoleRepository : BaseRepository<Role> ,IRoleRepository 
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public override void Update(Role entity)
        {
            EntityEntry<Role> dbEntityEntry = Context.Entry(entity);
            dbEntityEntry.State = EntityState.Modified;
            dbEntityEntry.Property(x => x.RoleId).IsModified = false;
        }
        public override Role GetSingle(string id)
        {
            return  Context.Set<Role>().FirstOrDefault(x => x.RoleId == id);
        }
        public override async Task<Role> GetSingleAsync(string id)
        {
            return await Context.Set<Role>().FirstOrDefaultAsync(x => x.RoleId == id);
        }
    }
}
