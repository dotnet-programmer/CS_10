﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CommonLibrary;

[Keyless]
public partial class ProductsByCategory
{
	[StringLength(15)]
	public string CategoryName { get; set; } = null!;

	[StringLength(40)]
	public string ProductName { get; set; } = null!;

	[StringLength(20)]
	public string? QuantityPerUnit { get; set; }

	public short? UnitsInStock { get; set; }

	public bool Discontinued { get; set; }
}