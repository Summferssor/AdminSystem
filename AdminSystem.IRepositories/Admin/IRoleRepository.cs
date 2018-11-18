using AdminSystem.Models.Admin.AdminModels.ModelView;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.IRepositories.Admin
{
    public interface IRoleRepository
    {
        void Add(Role role);
        IEnumerable<Role> GetAll();

        bool Save();
    }
}
