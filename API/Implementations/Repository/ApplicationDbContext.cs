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
    public DbSet<Student> Students { get; set; }
    public DbSet<Email> Emails { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<AcreditationType> AcreditationTypes { get; set; }
    public DbSet<InstitutionType> InstitutionTypes { get; set; }
    public DbSet<CareerCampus> CareerCampuses { get; set; }
    public DbSet<CareerCampusStats> CareerCampusStats { get; set; }
    public DbSet<CareerInstitution> CareerInstitutions { get; set; }
    public DbSet<CareerInstitutionStats> CareerInstitutionStats { get; set; }
    public DbSet<InstitutionCampus> InstitutionCampuses { get; set; }
    public DbSet<InstitutionDetails> InstitutionDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Region>().ToTable("region");
        modelBuilder.Entity<Schedule>().ToTable("schedule");
        modelBuilder.Entity<KnowledgeArea>().ToTable("knowledgearea");
        modelBuilder.Entity<Institution>().ToTable("institution");
        modelBuilder.Entity<Career>().ToTable("career");
        modelBuilder.Entity<Student>().ToTable("student");
        modelBuilder.Entity<Email>().ToTable("email");
        modelBuilder.Entity<Question>().ToTable("question");
        modelBuilder.Entity<InstitutionType>().ToTable("institutiontype");
        modelBuilder.Entity<AcreditationType>().ToTable("acreditationtype");
        modelBuilder.Entity<CareerCampus>().ToTable("careercampus");
        modelBuilder.Entity<CareerCampusStats>().ToTable("careercampusstats");
        modelBuilder.Entity<CareerInstitution>().ToTable("careerinstitution");
        modelBuilder.Entity<CareerInstitutionStats>().ToTable("careerinstitutionstats");
        modelBuilder.Entity<InstitutionCampus>().ToTable("institutioncampus");
        modelBuilder.Entity<InstitutionDetails>().ToTable("institutiondetails");

        modelBuilder.Entity<Career>()
            .HasOne(c => c.KnowledgeArea)
            .WithMany()
            .HasForeignKey(c => c.KnowledgeAreaId);

        modelBuilder.Entity<Email>()
            .HasOne(e => e.Student)
            .WithMany()
            .HasForeignKey(e => e.StudentId);
        
        modelBuilder.Entity<CareerCampus>()
            .HasOne(e => e.CareerInstitution)
            .WithMany(x => x.CareerCampuses)
            .HasForeignKey(e => e.CareerInstitutionId);
        
        modelBuilder.Entity<CareerCampus>()
            .HasOne(e => e.Schedule)
            .WithMany()
            .HasForeignKey(e => e.ScheduleId);
        
        modelBuilder.Entity<CareerCampus>()
            .HasOne(e => e.InstitutionCampus)
            .WithMany()
            .HasForeignKey(e => e.InstitutionCampusId);
        
        modelBuilder.Entity<CareerCampusStats>()
            .HasOne(e => e.CareerCampus)
            .WithMany(x => x.CareerCampusStats)
            .HasForeignKey(e => e.CareerCampusId);
        
        modelBuilder.Entity<CareerInstitution>()
            .HasOne(e => e.Institution)
            .WithMany()
            .HasForeignKey(e => e.InstitutionId);
        
        modelBuilder.Entity<CareerInstitution>()
            .HasOne(e => e.Career)
            .WithMany(x => x.CareerInstitutions)
            .HasForeignKey(e => e.CarrerId);
        
        modelBuilder.Entity<CareerInstitutionStats>()
            .HasOne(e => e.CareerInstitution)
            .WithMany(x => x.CareerInstitutionStats)
            .HasForeignKey(e => e.CareerInstitutionId);
        
        modelBuilder.Entity<InstitutionCampus>()
            .HasOne(e => e.Institution)
            .WithMany(x => x.InstitutionCampuses)
            .HasForeignKey(e => e.InstitutionId);
        
        modelBuilder.Entity<InstitutionCampus>()
            .HasOne(e => e.Region)
            .WithMany()
            .HasForeignKey(e => e.RegionId);
        
        modelBuilder.Entity<InstitutionDetails>()
            .HasOne(e => e.Institution)
            .WithMany(x => x.InstitutionDetails)
            .HasForeignKey(e => e.InstitutionId);
        
        modelBuilder.Entity<InstitutionDetails>()
            .HasOne(e => e.AcreditationType)
            .WithMany()
            .HasForeignKey(e => e.AcreditationTypeId);
        
        modelBuilder.Entity<Institution>()
            .HasOne(i => i.InstitutionType)
            .WithMany()
            .HasForeignKey(i => i.InstitutionTypeId);
    }
}