using Microsoft.EntityFrameworkCore;

namespace TodoList.Infrastructure.Model
{
    public partial class WorkshopdbContext : DbContext
    {
        public WorkshopdbContext()
        {
        }

        public WorkshopdbContext(DbContextOptions<WorkshopdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ToDo> ToDo { get; set; }
        public virtual DbSet<ToDoItem> ToDoItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ToDo>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Description  ");

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Status).HasColumnName("Status ");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Title ");
            });

            modelBuilder.Entity<ToDoItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.AssignedTo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Description  ");

                entity.Property(e => e.Status).HasColumnName("Status ");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Title ");

                entity.HasOne(d => d.Todo)
                    .WithMany(p => p.ToDoItem)
                    .HasForeignKey(d => d.TodoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TodoItem_Todo");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        private partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}