using System.ComponentModel.DataAnnotations.Schema;  // [Column]

namespace C10_WorkWithEFCore;

public class Category
{
	// aby programiści mogli dodawać produkty do kategorii,
	// musimy zainicjować właściwość nawigacyjną za pomocą pustej listy
	public Category()
		=> Products = new List<Product>();

	// te właściwości odwzorowują kolumny w bazie danych
	public int CategoryID { get; set; }

	public string CategoryName { get; set; }

	[Column(TypeName = "ntext")]
	public string Description { get; set; }

	// definiuje właściwość nawigacyjną dla powiązanych wierszy
	public virtual ICollection<Product> Products { get; set; }
}