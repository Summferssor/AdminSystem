using AdminSystem.Models.Admin.AdminModels.Model;
using AdminSystem.Models.Admin.AdminModels.ModelView;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Models.Admin.MyDbContext.ModelConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.RoleId);
            builder.Property(x => x.RoleName).HasMaxLength(50);
        }
    }
}
