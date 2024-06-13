//using IGT.Data.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IGT.Data.EntityTypeConfigurations
//{
//    public class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : class
//    {
//        public virtual void Configure(EntityTypeBuilder<T> builder)
//        {
//            builder.Property<bool>("IsDeleted")
//                .IsRequired()
//                .HasDefaultValue(false);
//            builder.Property<DateTime>("InsertedAt")
//                .IsRequired()
//                .HasDefaultValueSql("GETUTCDATE()")
//                .ValueGeneratedOnAdd();
//            builder.Property<int>("InsertedBy")
//                .IsRequired()
//                .HasDefaultValue(-99);
//            builder.Property<DateTime>("UpdatedAt")
//                .IsRequired()
//                .HasDefaultValueSql("SYSDATETIME()")
//                .ValueGeneratedOnUpdate();
//            builder.Property<int>("UpdatedBy");
//            builder.Property<int>("FormID");
//        }
//    }
//}
