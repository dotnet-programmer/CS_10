namespace UczniowieIKursy;

public class Student
{
	public int StudentId { get; set; }
	public string? Name { get; set; }
	public string? Surname { get; set; }

	public ICollection<Course>? Courses { get; set; }
}