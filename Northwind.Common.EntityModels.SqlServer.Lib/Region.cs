﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLibrary;

[Table("Region")]
public partial class Region
{
	[Key]
	[Column("RegionID")]
	public int RegionId { get; set; }

	[StringLength(50)]
	public string RegionDescription { get; set; } = null!;

	[InverseProperty("Region")]
	public virtual ICollection<Territory> Territories { get; set; } = [];
}