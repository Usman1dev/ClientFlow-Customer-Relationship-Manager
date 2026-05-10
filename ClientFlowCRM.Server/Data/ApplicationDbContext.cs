using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ClientFlowCRM.Server.Models;
namespace ClientFlowCRM.Server.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<UploadedDocument> UploadedDocuments => Set<UploadedDocument>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ── Customer ───────────────────────────────────────────────────
        builder.Entity<Customer>(entity =>
        {
            entity.HasIndex(c => c.Email);
            entity.HasOne(c => c.CreatedByUser)
                  .WithMany(u => u.Customers)
                  .HasForeignKey(c => c.CreatedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Lead ───────────────────────────────────────────────────────
        builder.Entity<Lead>(entity =>
        {
            entity.HasIndex(l => l.Email);
            entity.HasOne(l => l.AssignedToUser)
                  .WithMany(u => u.AssignedLeads)
                  .HasForeignKey(l => l.AssignedToUserId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // ── Appointment ────────────────────────────────────────────────
        builder.Entity<Appointment>(entity =>
        {
            entity.HasOne(a => a.Customer)
                  .WithMany(c => c.Appointments)
                  .HasForeignKey(a => a.CustomerId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(a => a.CreatedByUser)
                  .WithMany(u => u.Appointments)
                  .HasForeignKey(a => a.CreatedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Invoice ────────────────────────────────────────────────────
        builder.Entity<Invoice>(entity =>
        {
            entity.HasIndex(i => i.InvoiceNumber).IsUnique();

            entity.HasOne(i => i.Customer)
                  .WithMany(c => c.Invoices)
                  .HasForeignKey(i => i.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Payment ────────────────────────────────────────────────────
        builder.Entity<Payment>(entity =>
        {
            entity.HasOne(p => p.Invoice)
                  .WithMany(i => i.Payments)
                  .HasForeignKey(p => p.InvoiceId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── TaskItem ───────────────────────────────────────────────────
        builder.Entity<TaskItem>(entity =>
        {
            entity.HasOne(t => t.AssignedToUser)
                  .WithMany(u => u.AssignedTasks)
                  .HasForeignKey(t => t.AssignedToUserId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(t => t.CreatedByUser)
                  .WithMany(u => u.CreatedTasks)
                  .HasForeignKey(t => t.CreatedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Notification ───────────────────────────────────────────────
        builder.Entity<Notification>(entity =>
        {
            entity.HasOne(n => n.User)
                  .WithMany(u => u.Notifications)
                  .HasForeignKey(n => n.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ── UploadedDocument ───────────────────────────────────────────
        builder.Entity<UploadedDocument>(entity =>
        {
            entity.HasOne(d => d.Customer)
                  .WithMany(c => c.Documents)
                  .HasForeignKey(d => d.CustomerId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.UploadedByUser)
                  .WithMany(u => u.UploadedDocuments)
                  .HasForeignKey(d => d.UploadedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
