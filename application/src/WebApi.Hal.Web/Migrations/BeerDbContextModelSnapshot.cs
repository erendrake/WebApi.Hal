using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using WebApi.Hal.Web.Data;

namespace WebApi.Hal.Web.Migrations
{
    [DbContext(typeof(BeerDbContext))]
    partial class BeerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApi.Hal.Web.Models.Beer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BreweryId");

                    b.Property<string>("Name");

                    b.Property<int?>("StyleId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("WebApi.Hal.Web.Models.BeerStyle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("WebApi.Hal.Web.Models.Brewery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("WebApi.Hal.Web.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Beer_Id");

                    b.Property<string>("Content");

                    b.Property<string>("Title");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("WebApi.Hal.Web.Models.Beer", b =>
                {
                    b.HasOne("WebApi.Hal.Web.Models.Brewery")
                        .WithMany()
                        .HasForeignKey("BreweryId");

                    b.HasOne("WebApi.Hal.Web.Models.BeerStyle")
                        .WithMany()
                        .HasForeignKey("StyleId");
                });
        }
    }
}
