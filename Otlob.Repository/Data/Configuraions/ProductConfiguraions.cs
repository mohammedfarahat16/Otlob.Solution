using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otlob.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Repository.Data.Configuraions
{
    internal class ProductConfiguraions : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(p => p.ProductBrand)
                .WithMany()
                .HasForeignKey(P => P.ProductBrandId);

            builder.HasOne(P => P.ProductType)
                   .WithMany()
                   .HasForeignKey(p=>p.ProductTypeId);


            builder.Property(p =>p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p =>p.Description).IsRequired();

            builder.Property(p=>p.PictureUrl).IsRequired();
            builder.Property(p=>p.Price).HasColumnType("decimal(18,2)");


        }
    }
}
