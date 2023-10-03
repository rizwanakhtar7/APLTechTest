using APLTechChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace APLTechChallenge.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<ImageAudit> ImageAudits { get; set; }
        public DbSet<AuditErrorLogs> ErrorAuditLogs { get; set; }
    }
}
