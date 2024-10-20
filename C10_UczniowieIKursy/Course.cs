using System.ComponentModel.DataAnnotations;

namespace UczniowieIKursy;

public class Course
{
	public int CourseId { get; set; }

	[Required]
	[StringLength(60)]
	public string? Name { get; set; }

	public ICollection<Student>? Students { get; set; }
}