﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotekaWspolna;

public class Product
{
	public int ProductID { get; set; }

	[Required]
	[StringLength(40)]
	public string ProductName { get; set; } = null!;

	public int? SupplierID { get; set; }
	public int? CategoryID { get; set; }

	[StringLength(20)]
	public string? QuantityPerUnit { get; set; }

	[Column(TypeName = "money")] // Wymagane przez dostawcę SQL Server
	public decimal? UnitPrice { get; set; }

	public short? UnitsInStock { get; set; }
	public short? UnitsOnOrder { get; set; }
	public short? ReorderLevel { get; set; }
	public bool Discontinued { get; set; }
}