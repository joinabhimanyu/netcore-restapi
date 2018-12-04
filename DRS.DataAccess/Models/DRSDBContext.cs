using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DRS.DataAccess.Models
{
    public partial class DRSDBContext : DbContext
    {
        public DRSDBContext(DbContextOptions<DRSDBContext> options)
            : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server=ksju-intdb-ut.stb.intra,43305;Database=DRSDB;Trusted_Connection=True;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Archive>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UI_Archive_Name")
                    .IsUnique();

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Organization).HasColumnType("varchar(50)");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasIndex(e => e.DocumentIdentity)
                    .HasName("UI_DocumentIdentity")
                    .IsUnique();

                entity.Property(e => e.BatchClass).HasColumnType("varchar(20)");

                entity.Property(e => e.CompanyCode).HasColumnType("varchar(10)");

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.DocumentCategoryId).HasDefaultValueSql("0");

                entity.Property(e => e.DocumentIdentity)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.DocumentSourceId).HasDefaultValueSql("0");

                entity.Property(e => e.ExportFileType).HasColumnType("varchar(20)");

                entity.Property(e => e.IsActive).HasDefaultValueSql("1");

                entity.Property(e => e.Link).HasColumnType("varchar(500)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Pages).HasDefaultValueSql("1");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Version)
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("1.0");

                entity.HasOne(d => d.DocumentCategory)
                    .WithMany(p => p.Document)
                    .HasForeignKey(d => d.DocumentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Document_DocumentCategory");

                entity.HasOne(d => d.DocumentSource)
                    .WithMany(p => p.Document)
                    .HasForeignKey(d => d.DocumentSourceId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Document_DocumentSource");
            });

            modelBuilder.Entity<DocumentCategory>(entity =>
            {
                entity.HasIndex(e => e.Category)
                    .HasName("IX_DocumentCategoryNo");

                entity.HasIndex(e => new { e.ArchiveId, e.Category })
                    .HasName("UI_DocumentArchiveCategory")
                    .IsUnique();

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("varchar(max)");

                entity.Property(e => e.IsActive).HasDefaultValueSql("0");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.HasOne(d => d.Archive)
                    .WithMany(p => p.DocumentCategory)
                    .HasForeignKey(d => d.ArchiveId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_DocumentCategory_Archive");
            });

            modelBuilder.Entity<DocumentField>(entity =>
            {
                entity.HasKey(e => new { e.DocumentId, e.FieldId })
                    .HasName("PK_DocumentField");

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Name).HasColumnType("varchar(50)");

                entity.Property(e => e.Parameter).HasColumnType("varchar(500)");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Value).HasColumnType("varchar(500)");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.DocumentField)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK_DocumentField_Document");

                entity.HasOne(d => d.Field)
                    .WithMany(p => p.DocumentField)
                    .HasForeignKey(d => d.FieldId)
                    .HasConstraintName("FK_DocumentField_Field");

                entity.HasOne(d => d.ParameterNavigation)
                    .WithMany(p => p.DocumentField)
                    .HasPrincipalKey(p => p.Parameter)
                    .HasForeignKey(d => d.Parameter)
                    .HasConstraintName("FK_DocumentField_DocumentFieldParameter");
            });

            modelBuilder.Entity<DocumentFieldParameter>(entity =>
            {
                entity.HasIndex(e => e.Parameter)
                    .HasName("UI_DocumentFieldParameterName")
                    .IsUnique();

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.Parameter)
                    .IsRequired()
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Value).HasColumnType("varchar(500)");
            });

            modelBuilder.Entity<DocumentFieldSetting>(entity =>
            {
                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.IsActive).HasDefaultValueSql("1");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnType("varchar(500)");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.DocumentFieldSetting)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_DocumentFieldSetting_Document");

                entity.HasOne(d => d.Field)
                    .WithMany(p => p.DocumentFieldSetting)
                    .HasForeignKey(d => d.FieldId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_DocumentFieldSetting_Field");
            });

            modelBuilder.Entity<DocumentRule>(entity =>
            {
                entity.HasIndex(e => new { e.DocumentId, e.Name })
                    .HasName("UI_DocumentRuleName")
                    .IsUnique();

                entity.HasIndex(e => new { e.DocumentId, e.RuleOrder, e.IsActive })
                    .HasName("UI_DocumentRule")
                    .IsUnique();

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.DocumentRuleGuid).HasDefaultValueSql("newid()");

                entity.Property(e => e.FieldQueryRule).HasColumnType("varchar(max)");

                entity.Property(e => e.IsActive).HasDefaultValueSql("1");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.RuleOrder).HasDefaultValueSql("100");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.DocumentRule)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK_DocumentRule_Document");

                entity.HasOne(d => d.ProcessSchema)
                    .WithMany(p => p.DocumentRule)
                    .HasForeignKey(d => d.ProcessSchemaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_DocumentRule_ProcessSchema");
            });

            modelBuilder.Entity<DocumentRuleSetting>(entity =>
            {
                entity.HasIndex(e => new { e.DocumentRuleId, e.Name })
                    .HasName("UI_DocumentRuleSetting")
                    .IsUnique();

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnType("varchar(500)");

                entity.HasOne(d => d.DocumentRule)
                    .WithMany(p => p.DocumentRuleSetting)
                    .HasForeignKey(d => d.DocumentRuleId)
                    .HasConstraintName("FK_DocumentRuleSetting_DocumentRule");
            });

            modelBuilder.Entity<DocumentSource>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UI_DocumentSource_Name");

                entity.Property(e => e.DocumentSourceId).ValueGeneratedNever();

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("varchar(max)");

                entity.Property(e => e.DocumentUriPath).HasColumnType("varchar(500)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Organization).HasColumnType("varchar(50)");

                entity.Property(e => e.Priority).HasDefaultValueSql("10");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");
            });

            modelBuilder.Entity<DocumentWorkLog>(entity =>
            {
                entity.HasKey(e => new { e.DocumentId, e.WorkLogId })
                    .HasName("PK_DocumrntWorkLog");

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Done)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");
            });

            modelBuilder.Entity<Field>(entity =>
            {
                entity.HasIndex(e => e.FieldDataTypeNo)
                    .HasName("UI_FieldDataTypeNo")
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .HasName("UI_Field_Name")
                    .IsUnique();

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.FieldDataTypeNo)
                    .IsRequired()
                    .HasColumnType("char(2)");

                entity.Property(e => e.IsActive).HasDefaultValueSql("1");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");
            });

            modelBuilder.Entity<ImportMapping>(entity =>
            {
                entity.HasIndex(e => new { e.ProcessHandlerId, e.Name })
                    .HasName("UI_ImportMapping_Source_Name")
                    .IsUnique();

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.ImportMappingGuid).HasDefaultValueSql("newid()");

                entity.Property(e => e.IsActive).HasDefaultValueSql("1");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ProcessSchemaId).HasDefaultValueSql("0");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.HasOne(d => d.ProcessHandler)
                    .WithMany(p => p.ImportMapping)
                    .HasForeignKey(d => d.ProcessHandlerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ImportMapping_ProcessHandler");

                entity.HasOne(d => d.ProcessSchema)
                    .WithMany(p => p.ImportMapping)
                    .HasForeignKey(d => d.ProcessSchemaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ImportMapping_ProcessSchema");
            });

            modelBuilder.Entity<LsCommand>(entity =>
            {
                entity.ToTable("ls_Command");

                entity.Property(e => e.Command)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Request)
                    .IsRequired()
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<ProcessHandler>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UI_ProcessHandler_Name")
                    .IsUnique();

                entity.Property(e => e.AssemblyName)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.ClassName)
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.IsActive).HasDefaultValueSql("1");

                entity.Property(e => e.LastExecuteDate).HasColumnType("datetime");

                entity.Property(e => e.LastProcessDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.NextProcessDate).HasColumnType("datetime");

                entity.Property(e => e.NumberOfErrors).HasDefaultValueSql("10");

                entity.Property(e => e.NumberOfItemsToProcess).HasDefaultValueSql("100");

                entity.Property(e => e.NumberOfRetries).HasDefaultValueSql("0");

                entity.Property(e => e.Priority).HasDefaultValueSql("10");

                entity.Property(e => e.ProcessErrorCount).HasDefaultValueSql("0");

                entity.Property(e => e.ProcessHandlerGuid).HasDefaultValueSql("newid()");

                entity.Property(e => e.ProcessHandlerTypeId).HasDefaultValueSql("0");

                entity.Property(e => e.ProcessSchemaId).HasDefaultValueSql("0");

                entity.Property(e => e.ProcessServers).HasColumnType("varchar(max)");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.WaitIntervalBetweenRetries).HasDefaultValueSql("0");

                entity.Property(e => e.WaitIntervalOnErrors).HasDefaultValueSql("0");

                entity.HasOne(d => d.ProcessHandlerType)
                    .WithMany(p => p.ProcessHandler)
                    .HasForeignKey(d => d.ProcessHandlerTypeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProcessHandler_ProcessHandlerType");

                entity.HasOne(d => d.ProcessSchema)
                    .WithMany(p => p.ProcessHandler)
                    .HasForeignKey(d => d.ProcessSchemaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProcessHandler_ProcessSchema");
            });

            modelBuilder.Entity<ProcessHandlerField>(entity =>
            {
                entity.HasIndex(e => new { e.ProcessHandlerId, e.FieldId })
                    .HasName("UI_ProcessHandlerField_HandlerId_FieldId")
                    .IsUnique();

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.FieldName)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.FieldValueFormat).HasColumnType("varchar(50)");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.HasOne(d => d.Field)
                    .WithMany(p => p.ProcessHandlerField)
                    .HasForeignKey(d => d.FieldId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProcessHandlerField_Field");

                entity.HasOne(d => d.ProcessHandler)
                    .WithMany(p => p.ProcessHandlerField)
                    .HasForeignKey(d => d.ProcessHandlerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProcessHandlerField_ProcessHandler");
            });

            modelBuilder.Entity<ProcessHandlerSchedule>(entity =>
            {
                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.DailyFrequencyEndAt)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("'23:59:59'");

                entity.Property(e => e.DailyFrequencyInterval).HasDefaultValueSql("1");

                entity.Property(e => e.DailyFrequencyIntervalType).HasDefaultValueSql("1");

                entity.Property(e => e.DailyFrequencyOccursAt).HasColumnType("smalldatetime");

                entity.Property(e => e.DailyFrequencyStartAt)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("'00:00:00'");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.DurationEndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.DurationStartDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.FrequencyFlags).HasDefaultValueSql("127");

                entity.Property(e => e.FrequencyTypeId).HasDefaultValueSql("2");

                entity.Property(e => e.IsActive).HasDefaultValueSql("1");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.HasOne(d => d.ProcessHandler)
                    .WithMany(p => p.ProcessHandlerSchedule)
                    .HasForeignKey(d => d.ProcessHandlerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProcessHandlerSchedule_ProcessHandler");
            });

            modelBuilder.Entity<ProcessHandlerSetting>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_ProcessHandlerSetting_Name");

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnType("varchar(4000)");

                entity.Property(e => e.Visible).HasDefaultValueSql("0");

                entity.HasOne(d => d.ProcessHandler)
                    .WithMany(p => p.ProcessHandlerSetting)
                    .HasForeignKey(d => d.ProcessHandlerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProcessHandlerSetting_ProcessHandler");
            });

            modelBuilder.Entity<ProcessHandlerType>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UI_ProcessHandlerType_Name");

                entity.Property(e => e.ProcessHandlerTypeId).ValueGeneratedNever();

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");
            });

            modelBuilder.Entity<ProcessLog>(entity =>
            {
                entity.HasIndex(e => e.ProcessHandlerId)
                    .HasName("IX_ProcessLog_ProcessHandlerId");

                entity.HasIndex(e => e.Severity)
                    .HasName("IX_ProcessLog_Severity");

                entity.HasIndex(e => e.Timestamp)
                    .HasName("IX_ProcessLog_Timestamp");

                entity.HasIndex(e => new { e.ProcessQueueId, e.EventId })
                    .HasName("IX_ProcessLog_EventId");

                entity.HasIndex(e => new { e.ProcessQueueId, e.ProcessHandlerId })
                    .HasName("IX_ProcessLog_QueueId_HandlerId");

                entity.Property(e => e.EventId).HasDefaultValueSql("0");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnType("varchar(max)");

                entity.Property(e => e.Severity)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'Information'");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.TraceEventTypeId).HasDefaultValueSql("0");

                entity.HasOne(d => d.ProcessHandler)
                    .WithMany(p => p.ProcessLog)
                    .HasForeignKey(d => d.ProcessHandlerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ProcessLog_ProcessHandler");

                entity.HasOne(d => d.ProcessQueue)
                    .WithMany(p => p.ProcessLog)
                    .HasForeignKey(d => d.ProcessQueueId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ProcessLog_ProcessQueue");

                entity.HasOne(d => d.ProcessSchemaStep)
                    .WithMany(p => p.ProcessLog)
                    .HasForeignKey(d => d.ProcessSchemaStepId)
                    .HasConstraintName("FK_ProcessLog_ProcessSchemaStep");
            });

            modelBuilder.Entity<ProcessQueue>(entity =>
            {
                entity.HasIndex(e => e.NextProcessSchemaStepId)
                    .HasName("IX_ProcessQueueSchemaStepId");

                entity.HasIndex(e => e.ProcessQueueStateId)
                    .HasName("IX_ProcessQueueState");

                entity.HasIndex(e => new { e.ImportHandlerId, e.DocumentKey })
                    .HasName("IX_ProcessQueueDocumentKey");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.DocumentArchiveKey).HasColumnType("varchar(50)");

                entity.Property(e => e.DocumentCategory).HasColumnType("varchar(50)");

                entity.Property(e => e.DocumentId).HasDefaultValueSql("0");

                entity.Property(e => e.DocumentIdentity).HasColumnType("varchar(50)");

                entity.Property(e => e.DocumentKey).HasColumnType("varchar(50)");

                entity.Property(e => e.DocumentName).HasColumnType("varchar(500)");

                entity.Property(e => e.DocumentPages).HasDefaultValueSql("1");

                entity.Property(e => e.DocumentStamp).HasColumnType("datetime");

                entity.Property(e => e.LastProcessDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.ProcessErrorCount).HasDefaultValueSql("0");

                entity.Property(e => e.ProcessQueueStateId).HasDefaultValueSql("0");

                entity.Property(e => e.ProcessSchemaId).HasDefaultValueSql("0");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.ProcessQueue)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK_ProcessQueue_Document");

                entity.HasOne(d => d.DocumentRule)
                    .WithMany(p => p.ProcessQueue)
                    .HasForeignKey(d => d.DocumentRuleId)
                    .HasConstraintName("FK_ProcessQueue_DocumentRule");

                entity.HasOne(d => d.ImportHandler)
                    .WithMany(p => p.ProcessQueue)
                    .HasForeignKey(d => d.ImportHandlerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProcessQueue_ProcessHandler");

                entity.HasOne(d => d.NextProcessSchemaStep)
                    .WithMany(p => p.ProcessQueue)
                    .HasForeignKey(d => d.NextProcessSchemaStepId)
                    .HasConstraintName("FK_ProcessQueue_ProcessSchemaStep");

                entity.HasOne(d => d.ProcessQueueState)
                    .WithMany(p => p.ProcessQueue)
                    .HasForeignKey(d => d.ProcessQueueStateId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProcessQueue_ProcessState");

                entity.HasOne(d => d.ProcessSchema)
                    .WithMany(p => p.ProcessQueue)
                    .HasForeignKey(d => d.ProcessSchemaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProcessQueue_ProcessSchema");
            });

            modelBuilder.Entity<ProcessQueueField>(entity =>
            {
                entity.HasIndex(e => new { e.FieldId, e.FieldValue })
                    .HasName("IX_ProcessQueueFieldIdAndValue");

                entity.HasIndex(e => new { e.FieldId, e.FieldValue, e.ProcessQueueId })
                    .HasName("IX_ProcessQueueField_ProcessQueueId");

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.FieldId).HasDefaultValueSql("0");

                entity.Property(e => e.FieldValue)
                    .IsRequired()
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.Field)
                    .WithMany(p => p.ProcessQueueField)
                    .HasForeignKey(d => d.FieldId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProcessQueueField_Field");

                entity.HasOne(d => d.ProcessQueue)
                    .WithMany(p => p.ProcessQueueField)
                    .HasForeignKey(d => d.ProcessQueueId)
                    .HasConstraintName("FK_ProcessQueueField_ProcessQueue");
            });

            modelBuilder.Entity<ProcessQueueFile>(entity =>
            {
                entity.HasIndex(e => e.ProcessQueueId)
                    .HasName("IX_ProcessQueueFile_ProcessQueueId");

                entity.HasIndex(e => new { e.Path, e.Filename })
                    .HasName("IX_ProcessQueueFile_FilePathName");

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.FileType).HasDefaultValueSql("0");

                entity.Property(e => e.Filename)
                    .IsRequired()
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.ProcessState).HasDefaultValueSql("0");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.ProcessQueue)
                    .WithMany(p => p.ProcessQueueFile)
                    .HasForeignKey(d => d.ProcessQueueId)
                    .HasConstraintName("FK_ProcessQueueFile_ProcessQueue");
            });

            modelBuilder.Entity<ProcessQueueState>(entity =>
            {
                entity.Property(e => e.ProcessQueueStateId).ValueGeneratedNever();

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.StateImage).HasColumnType("image");

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");
            });

            modelBuilder.Entity<ProcessSchema>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UI_ProcessSchemaName")
                    .IsUnique();

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.ProcessSchemaGuid).HasDefaultValueSql("newid()");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");
            });

            modelBuilder.Entity<ProcessSchemaStep>(entity =>
            {
                entity.HasIndex(e => e.ProcessHandlerId)
                    .HasName("IX_ProcessHandler");

                entity.HasIndex(e => new { e.ProcessSchemaId, e.StepOrder })
                    .HasName("UI_ProcessSchemaStep_SchemaStepOrder")
                    .IsUnique();

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(500)");

                entity.Property(e => e.FieldQueryRule).HasColumnType("varchar(max)");

                entity.Property(e => e.Name).HasColumnType("varchar(100)");

                entity.Property(e => e.ProcessSchemaStepGuid).HasDefaultValueSql("newid()");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.StepOrder).HasDefaultValueSql("1");

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.HasOne(d => d.ProcessHandler)
                    .WithMany(p => p.ProcessSchemaStep)
                    .HasForeignKey(d => d.ProcessHandlerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProcessSchemaStep_ProcessHandler");

                entity.HasOne(d => d.ProcessSchema)
                    .WithMany(p => p.ProcessSchemaStep)
                    .HasForeignKey(d => d.ProcessSchemaId)
                    .HasConstraintName("FK_ProcessSchemaStep_ProcessSchema");
            });

            modelBuilder.Entity<ProcessSchemaStepSetting>(entity =>
            {
                entity.HasIndex(e => e.ProcessSchemaStepId)
                    .HasName("IX_ProcessSchemaStepSetting_ProcessSchemaStepId");

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnType("varchar(500)");

                entity.HasOne(d => d.ProcessSchemaStep)
                    .WithMany(p => p.ProcessSchemaStepSetting)
                    .HasForeignKey(d => d.ProcessSchemaStepId)
                    .HasConstraintName("FK_ProcessStepSettings_ProcessStep");
            });

            modelBuilder.Entity<ProductGroupWeightingHelpingTable>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Value).IsRequired();
            });

            modelBuilder.Entity<SettingSelection>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK_SettingSelection");

                entity.Property(e => e.Name).HasColumnType("varchar(100)");

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.IsVisible).HasDefaultValueSql("1");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");
            });

            modelBuilder.Entity<SettingSelectionValue>(entity =>
            {
                entity.HasKey(e => new { e.Name, e.Value })
                    .HasName("PK_SettingSelectionValue");

                entity.Property(e => e.Name).HasColumnType("varchar(100)");

                entity.Property(e => e.Value).HasColumnType("varchar(500)");

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.IsVisible).HasDefaultValueSql("1");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");
            });

            modelBuilder.Entity<SystemLog>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK_SystemLog");

                entity.Property(e => e.LogId).HasColumnName("LogID");

                entity.Property(e => e.AppDomainName)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.MachineName)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.Message).HasMaxLength(1500);

                entity.Property(e => e.ProcessName)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.Severity)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<SystemSetting>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UI_SystemSetting.Name");

                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnType("varchar(500)");

                entity.HasOne(d => d.SystemSettingType)
                    .WithMany(p => p.SystemSetting)
                    .HasForeignKey(d => d.SystemSettingTypeId)
                    .HasConstraintName("FK_SystemSetting_SystemSettingType");
            });

            modelBuilder.Entity<SystemSettingType>(entity =>
            {
                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");
            });

            modelBuilder.Entity<WorkLog>(entity =>
            {
                entity.Property(e => e.Created)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");

                entity.Property(e => e.Done)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Stamp)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Text).HasColumnType("varchar(max)");

                entity.Property(e => e.Updated)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("suser_sname()");
            });
        }

        public virtual DbSet<Archive> Archive { get; set; }
        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<DocumentCategory> DocumentCategory { get; set; }
        public virtual DbSet<DocumentField> DocumentField { get; set; }
        public virtual DbSet<DocumentFieldParameter> DocumentFieldParameter { get; set; }
        public virtual DbSet<DocumentFieldSetting> DocumentFieldSetting { get; set; }
        public virtual DbSet<DocumentRule> DocumentRule { get; set; }
        public virtual DbSet<DocumentRuleSetting> DocumentRuleSetting { get; set; }
        public virtual DbSet<DocumentSource> DocumentSource { get; set; }
        public virtual DbSet<DocumentWorkLog> DocumentWorkLog { get; set; }
        public virtual DbSet<Field> Field { get; set; }
        public virtual DbSet<ImportMapping> ImportMapping { get; set; }
        public virtual DbSet<LsCommand> LsCommand { get; set; }
        public virtual DbSet<ProcessHandler> ProcessHandler { get; set; }
        public virtual DbSet<ProcessHandlerField> ProcessHandlerField { get; set; }
        public virtual DbSet<ProcessHandlerSchedule> ProcessHandlerSchedule { get; set; }
        public virtual DbSet<ProcessHandlerSetting> ProcessHandlerSetting { get; set; }
        public virtual DbSet<ProcessHandlerType> ProcessHandlerType { get; set; }
        public virtual DbSet<ProcessLog> ProcessLog { get; set; }
        public virtual DbSet<ProcessQueue> ProcessQueue { get; set; }
        public virtual DbSet<ProcessQueueField> ProcessQueueField { get; set; }
        public virtual DbSet<ProcessQueueFile> ProcessQueueFile { get; set; }
        public virtual DbSet<ProcessQueueState> ProcessQueueState { get; set; }
        public virtual DbSet<ProcessSchema> ProcessSchema { get; set; }
        public virtual DbSet<ProcessSchemaStep> ProcessSchemaStep { get; set; }
        public virtual DbSet<ProcessSchemaStepSetting> ProcessSchemaStepSetting { get; set; }
        public virtual DbSet<ProductGroupWeightingHelpingTable> ProductGroupWeightingHelpingTable { get; set; }
        public virtual DbSet<SettingSelection> SettingSelection { get; set; }
        public virtual DbSet<SettingSelectionValue> SettingSelectionValue { get; set; }
        public virtual DbSet<SystemLog> SystemLog { get; set; }
        public virtual DbSet<SystemSetting> SystemSetting { get; set; }
        public virtual DbSet<SystemSettingType> SystemSettingType { get; set; }
        public virtual DbSet<WorkLog> WorkLog { get; set; }

        // Unable to generate entity type for table 'dbo.ProductGroupWeightingTable'. Please see the warning messages.
    }
}