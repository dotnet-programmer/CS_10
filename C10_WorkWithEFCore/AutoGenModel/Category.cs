﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkWithEFCore.AutoGen;

[Index("CategoryName", Name = "CategoryName")]
public partial class Category
{
	[Key]
	public long CategoryId { get; set; }

	[Column(TypeName = "nvarchar (15)")]
	public string CategoryName { get; set; } = null!;

	[Column(TypeName = "ntext")]
	public string? Description { get; set; }

	[Column(TypeName = "image")]
	public byte[]? Picture { get; set; }

	[InverseProperty("Category")]
	public virtual ICollection<Product> Products { get; } = new List<Product>();
}