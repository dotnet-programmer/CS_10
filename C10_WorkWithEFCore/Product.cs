using System.ComponentModel.DataAnnotations;  // [Required] i [StringLength]
using System.ComponentModel.DataAnnotations.Schema;  // [Column]

namespace C10_WorkWithEFCore;

public class Product
{
	public int ProductID { get; set; }   // klucz główny

	[Required]
	[StringLength(40)]
	public string ProductName { get; set; } = null!;

	[Column("UnitPrice", TypeName = "money")]
	public decimal? Price { get; set; }  // nazwa właściwości != nazwa kolumny

	[Column("UnitsInStock")]
	public short? UnitsInStock { get; set; }

	public bool Discontinued { get; set; }

	// te dwie właściwości definiują relację tej tabeli z tabelą Categories
	public int CategoryID { get; set; }

	// Dzięki dodaniu słowa "virtual" EF Core może pokryć te właściwości w klasach wywiedzionych,
	// aby zaimplementować specjalne funkcje, takie jak ładowanie opóźnione.
	public virtual Category Category { get; set; } = null!;
}