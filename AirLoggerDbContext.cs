using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WinFormsApp1
{
    [Table("Student")]
    public class Student
    {
        [Key]
        public int Id_Student { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Id_Group { get; set; }
    }

    [Table("Professor")]
    public class Professor
    {
        [Key]
        public int Id_Professor { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Id_Department { get; set; }
    }

    [Table("Project")]
    public class Project
    {
        [Key]
        public int Id_Project { get; set; }
        public int Id_Student { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string? versionProject { get; set; }
        public string? CatalogName { get; set; }
    }

    [Table("UserException")]
    public class UserException
    {
        [Key]
        public int Id { get; set; }
        public string? Message { get; set; }
        public string? TargetSite { get; set; }
        public DateTime dateTimeExc { get; set; }
        public int IndexForm { get; set; }
    }

    [Table("MetaData")]
    public class MetaData
    {
        [Key]
        public int Id_MetaData { get; set; }
        public int Id_Professor { get; set; }
        public int Id_Task { get; set; }
        public int Id_Student { get; set; }
        public int Id_Project { get; set; }
        public int Id { get; set; }
        public int Id_Incident { get; set; }
        public int? indexReplica_1 { get; set; }
        public int? indexReplica_2 { get; set; }
        public bool hasTextLog { get; set; }
    }

    public class AirLoggerDbContext : DbContext
    {
        public const string ConnectionString =
            @"Server=(localdb)\MSSQLLocalDB;Database=AirLogger;Trusted_Connection=True;TrustServerCertificate=True;";

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Professor> Professors { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<UserException> UserExceptions { get; set; } = null!;
        public DbSet<MetaData> MetaDataRecords { get; set; } = null!;

        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                   .AddProvider(new AirLoggerProvider());
        });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }
    }
}