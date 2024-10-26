using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommonLibrary;

[Keyless]
public partial class SummaryOfSalesByQuarter
{
	[Column(TypeName = "datetime")]
	public DateTime? ShippedDate { get; set; }

	[Column("OrderID")]
	public int OrderId { get; set; }

	[Column(TypeName = "money")]
	public decimal? Subtotal { get; set; }
}