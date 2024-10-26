using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommonLibrary;

[Keyless]
public partial class OrderSubtotal
{
	[Column("OrderID")]
	public int OrderId { get; set; }

	[Column(TypeName = "money")]
	public decimal? Subtotal { get; set; }
}