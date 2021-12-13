using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WT.Ecommerce.Domain.Models;

namespace WT.Ecommerce.Data.Configurations
{
    public class CustomerInformationConfiguration: IEntityTypeConfiguration<CustomerInformation>
    {
        public void Configure(EntityTypeBuilder<CustomerInformation> builder)
        {
            builder.ToTable("CustomerInformation");

            builder.HasKey(e => e.Id);
        }
    }
}
