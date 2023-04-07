using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReviewApp.Models;

namespace ReviewApp.EntityConfiguration;

public class PokemonOwnerConfiguration : IEntityTypeConfiguration<PokemonOwner>
{
    public void Configure(EntityTypeBuilder<PokemonOwner> builder)
    {
        builder.HasKey(po => new { po.PokemonId, po.OwnerId });

        builder.HasOne(p => p.Pokemon)
            .WithMany(po => po.PokemonOwners)
            .HasForeignKey(p => p.PokemonId);

        builder.HasOne(o => o.Owner)
            .WithMany(po => po.PokemonOwners)
            .HasForeignKey(o => o.OwnerId);
    }
}