using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id).HasConversion
                (orderId => orderId.Value,
                dbId => OrderId.Of(dbId));

            builder.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .IsRequired();

            builder.HasMany(o => o.OrderItems)
                .WithOne()
                .HasForeignKey(oi => oi.OrderId)
                //.IsRequired()
                //.OnDelete(DeleteBehavior.Cascade)
                ;

            builder.ComplexProperty(
                o => o.OrderName, nameBuilder =>
                {
                    nameBuilder.Property(n => n.Value)
                        .HasColumnName(nameof(Order.OrderName))
                        .IsRequired()
                        .HasMaxLength(256);
                });

            builder.ComplexProperty(
                o => o.ShippingAddress, addressBuilder =>
                {
                    addressBuilder.Property(n => n.FirstName)
                        .IsRequired()
                        .HasMaxLength(256);

                    addressBuilder.Property(n => n.LastName)
                        .IsRequired()
                        .HasMaxLength(256);

                    addressBuilder.Property(n => n.EmailAddress)
                    .HasMaxLength(50);

                    addressBuilder.Property(n => n.AddressLine)
                        .IsRequired()
                        .HasMaxLength(256);

                    addressBuilder.Property(n => n.Country)
                        .HasMaxLength(100);

                    addressBuilder.Property(n => n.State)
                            .HasMaxLength(100);

                    addressBuilder.Property(n => n.ZipCode)
                        .IsRequired()
                        .HasMaxLength(20);
                });

            builder.ComplexProperty(
                o => o.BillingAddress, addressBuilder =>
                {
                    addressBuilder.Property(n => n.FirstName)
                        .IsRequired()
                        .HasMaxLength(256);
                    addressBuilder.Property(n => n.LastName)
                        .IsRequired()
                        .HasMaxLength(256);
                    addressBuilder.Property(n => n.EmailAddress)
                    .HasMaxLength(50);
                    addressBuilder.Property(n => n.AddressLine)
                        .IsRequired()
                        .HasMaxLength(256);
                    addressBuilder.Property(n => n.Country)
                        .HasMaxLength(100);
                    addressBuilder.Property(n => n.State)
                            .HasMaxLength(100);
                    addressBuilder.Property(n => n.ZipCode)
                        .IsRequired()
                        .HasMaxLength(20);
                });

            builder.ComplexProperty(
                o => o.Payment, paymentBuilder =>
                {
                    paymentBuilder.Property(p => p.CardName)
                        .HasMaxLength(50);
                    paymentBuilder.Property(p => p.CardNumber)
                        .HasMaxLength(24);
                    paymentBuilder.Property(p => p.Expiration)
                        .HasMaxLength(10);
                    paymentBuilder.Property(p => p.CVV)
                        .HasMaxLength(3);
                    paymentBuilder.Property(p => p.PaymentMethod);
                });

            builder.Property(o => o.Status)
                .HasDefaultValue(OrderStatus.Draf)
                .HasConversion(
                s => s.ToString(),
                dbStatus =>
                (OrderStatus)Enum.Parse(typeof(OrderStatus),
                dbStatus));

            builder.Property(o => o.TotalPrice);

        }
    }
}
