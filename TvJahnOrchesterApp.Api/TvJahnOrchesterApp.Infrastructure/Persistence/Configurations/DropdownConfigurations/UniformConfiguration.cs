using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TvJahnOrchesterApp.Domain.Common.Entities;
using TvJahnOrchesterApp.Domain.Common.Enums;

namespace TvJahnOrchesterApp.Infrastructure.Persistence.Configurations.DropdownConfigurations
{
    internal class UniformConfiguration : IEntityTypeConfiguration<Uniform>
    {
        public void Configure(EntityTypeBuilder<Uniform> builder)
        {
            builder.ToTable("Uniform");
            builder.HasKey(m => m.Id);

            builder.HasData(new[]
            {
                Uniform.Create((int)UniformEnum.BlauesHemd, "Blaues Hemd"),
                Uniform.Create((int)UniformEnum.WeißeHose, "Weiße Hose"),
                Uniform.Create((int)UniformEnum.Jacket, "Jacket"),
                Uniform.Create((int)UniformEnum.WinterJacke, "Winter Jacke"),
                Uniform.Create((int)UniformEnum.Zivil, "Zivil"),
            });
        }
    }
}
