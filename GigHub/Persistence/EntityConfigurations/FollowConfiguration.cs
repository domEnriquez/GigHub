using GigHub.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace GigHub.Persistence.EntityConfigurations
{
    public class FollowConfiguration : EntityTypeConfiguration<Follow>
    {
        public FollowConfiguration()
        {
            HasKey(f => new { f.FollowerId, f.FolloweeId });

            Property(f => f.FollowerId)
                .HasColumnOrder(1);

            Property(f => f.FolloweeId)
                .HasColumnOrder(2);
        }
    }
}