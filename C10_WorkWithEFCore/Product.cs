using System.ComponentModel.DataAnnotations;  // [Required] i [StringLength]
using System.ComponentModel.DataAnnotations.Schema;  // [Column]

namespace BibliotekaWspolna;

public class Product
{
	public int ProductID { get; set; }   // klucz główny

	[Required]
	[StringLength(40)]
	public string ProductName { get; set; } = null!;

	[Column("UnitPrice", TypeName = "money")]
	public decimal? Koszt { get; set; }  // nazwa właściwości != nazwa kolumny

	[Column("UnitsInStock")]
	public short? WMagazynie { get; set; }

	public bool Discontinued { get; set; }

	// te dwie właściwości definiują relację tej tabeli z tabelą Categories
	public int CategoryID { get; set; }

	public virtual Category Category { get; set; } = null!;
}