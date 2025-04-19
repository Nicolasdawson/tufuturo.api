using API.Implementations.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Implementations.Repository;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Region> Regions { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<KnowledgeArea> KnowledgeAreas { get; set; }
    public DbSet<Institution> Institutions { get; set; }
    public DbSet<Career> Careers { get; set; }
    public DbSet<EmploymentIncome> EmploymentIncomes { get; set; }
    public DbSet<Statistics> Statistics { get; set; }
    public DbSet<WeightingCareer> WeightingCareers { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<WeightingStudent> WeightingStudents { get; set; }
    public DbSet<Email> Emails { get; set; }
    public DbSet<Question> Questions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Region>().ToTable("region");
        modelBuilder.Entity<Schedule>().ToTable("schedule");
        modelBuilder.Entity<KnowledgeArea>().ToTable("knowledgeArea");
        modelBuilder.Entity<Institution>().ToTable("institution");
        modelBuilder.Entity<Career>().ToTable("career");
        modelBuilder.Entity<EmploymentIncome>().ToTable("employmentIncome");
        modelBuilder.Entity<Statistics>().ToTable("statistics");
        modelBuilder.Entity<WeightingCareer>().ToTable("weightingCareer");
        modelBuilder.Entity<Student>().ToTable("student");
        modelBuilder.Entity<WeightingStudent>().ToTable("weightingStudent");
        modelBuilder.Entity<Email>().ToTable("email");
        modelBuilder.Entity<Question>().ToTable("question");
        
        modelBuilder.Entity<Institution>()
            .HasOne(i => i.Region)
            .WithMany()
            .HasForeignKey(i => i.RegionId);

        modelBuilder.Entity<Career>()
            .HasOne(c => c.Institution)
            .WithMany()
            .HasForeignKey(c => c.InstitutionId);

        modelBuilder.Entity<Career>()
            .HasOne(c => c.KnowledgeArea)
            .WithMany()
            .HasForeignKey(c => c.KnowledgeAreaId);

        modelBuilder.Entity<Career>()
            .HasOne(c => c.Schedule)
            .WithMany()
            .HasForeignKey(c => c.ScheduleId);

        modelBuilder.Entity<EmploymentIncome>()
            .HasOne(e => e.Career)
            .WithMany()
            .HasForeignKey(e => e.CareerId);

        modelBuilder.Entity<Statistics>()
            .HasOne(s => s.Career)
            .WithMany()
            .HasForeignKey(s => s.CareerId);

        modelBuilder.Entity<WeightingCareer>()
            .HasOne(w => w.Career)
            .WithMany()
            .HasForeignKey(w => w.CareerId);

        modelBuilder.Entity<WeightingStudent>()
            .HasOne(w => w.Student)
            .WithMany()
            .HasForeignKey(w => w.StudentId);

        modelBuilder.Entity<Email>()
            .HasOne(e => e.Student)
            .WithMany()
            .HasForeignKey(e => e.StudentId);
    }
}