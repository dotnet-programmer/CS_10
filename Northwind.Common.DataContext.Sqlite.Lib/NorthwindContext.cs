using Microsoft.EntityFrameworkCore;

namespace CommonLibrary;

public partial class NorthwindContext : DbContext
{
	public NorthwindContext()
	{
	}

	public NorthwindContext(DbContextOptions<NorthwindContext> options) : base(options)
	{
	}

	public virtual DbSet<Category> Categories { get; set; }

	public virtual DbSet<Customer> Customers { get; set; }

	public virtual DbSet<Employee> Employees { get; set; }

	public virtual DbSet<EmployeeTerritory> EmployeeTerritories { get; set; }

	public virtual DbSet<Order> Orders { get; set; }

	public virtual DbSet<OrderDetail> OrderDetails { get; set; }

	public virtual DbSet<Product> Products { get; set; }

	public virtual DbSet<Shipper> Shippers { get; set; }

	public virtual DbSet<Supplier> Suppliers { get; set; }

	public virtual DbSet<Territory> Territories { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> optionsBuilder.UseSqlite("Filename=../Northwind.db");

	//protected override void OnModelCreating(ModelBuilder modelBuilder)
	//{
	//	modelBuilder.Entity<Category>(entity =>
	//	{
	//		entity.Property(e => e.CategoryId).HasDefaultValueSql();
	//	});

	//	modelBuilder.Entity<Employee>(entity =>
	//	{
	//		entity.Property(e => e.EmployeeId).HasDefaultValueSql();
	//	});

	//	modelBuilder.Entity<Order>(entity =>
	//	{
	//		entity.Property(e => e.OrderId).HasDefaultValueSql();
	//		entity.Property(e => e.Freight).HasDefaultValue(0.0);
	//	});

	//	modelBuilder.Entity<OrderDetail>(entity =>
	//	{
	//		entity.Property(e => e.Quantity).HasDefaultValue((short)1);

	//		entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails).OnDelete(DeleteBehavior.ClientSetNull);

	//		entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails).OnDelete(DeleteBehavior.ClientSetNull);
	//	});

	//	modelBuilder.Entity<Product>(entity =>
	//	{
	//		entity.Property(e => e.ProductId).HasDefaultValueSql();
	//		entity.Property(e => e.ReorderLevel).HasDefaultValue((short)0);
	//		entity.Property(e => e.UnitPrice).HasDefaultValue(0.0);
	//		entity.Property(e => e.UnitsInStock).HasDefaultValue((short)0);
	//		entity.Property(e => e.UnitsOnOrder).HasDefaultValue((short)0);
	//	});

	//	modelBuilder.Entity<Shipper>(entity =>
	//	{
	//		entity.Property(e => e.ShipperId).HasDefaultValueSql();
	//	});

	//	modelBuilder.Entity<Supplier>(entity =>
	//	{
	//		entity.Property(e => e.SupplierId).HasDefaultValueSql();
	//	});

	//	OnModelCreatingPartial(modelBuilder);
	//}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<OrderDetail>(entity =>
		{
			entity.HasKey(e => new { e.OrderId, e.ProductId });

			entity.HasOne(d => d.Order)
			  .WithMany(p => p.OrderDetails)
			  .HasForeignKey(d => d.OrderId)
			  .OnDelete(DeleteBehavior.ClientSetNull);

			entity.HasOne(d => d.Product)
			  .WithMany(p => p.OrderDetails)
			  .HasForeignKey(d => d.ProductId)
			  .OnDelete(DeleteBehavior.ClientSetNull);
		});

		modelBuilder.Entity<Product>()
		  .Property(p => p.UnitPrice)
		  .HasConversion<double>();

		OnModelCreatingPartial(modelBuilder);
	}

	private partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}