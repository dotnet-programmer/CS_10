namespace Library;

public class Employee : Person
{
	public string? EmployeeCode { get; set; }
	public DateTime DateOfEmployment { get; set; }

	public new void WriteLineInConsole()
		=> Console.WriteLine(
			format: "{0}, data urodzenia {1:dd/MM/yy}, data zatrudnienia {2:dd/MM/yy}",
			arg0: Name,
			arg1: DateOfBirth,
			arg2: DateOfEmployment);

	public override string ToString()
		=> $"Kod pracownika {Name} to {EmployeeCode}";
}