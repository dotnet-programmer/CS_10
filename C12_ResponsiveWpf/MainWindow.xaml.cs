using System.Diagnostics;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace ResponsywneWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	private const string ConnectionString = "Server=AORUS\\SQLEXPRESS;Database=Northwind;User Id=dbuser;Password=;TrustServerCertificate=True;";
	private const string Sql = "WAITFOR DELAY '00:00:05';SELECT EmployeeId, FirstName, LastName FROM Employees";

	public MainWindow()
		=> InitializeComponent();

	private void GetEmployeeSyncButton_Click(object sender, RoutedEventArgs e)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		using (SqlConnection connection = new(ConnectionString))
		{
			connection.Open();
			SqlCommand command = new(Sql, connection);
			SqlDataReader readingData = command.ExecuteReader();
			while (readingData.Read())
			{
				string employee = string.Format("{0}: {1} {2}",
				  readingData.GetInt32(0),
				  readingData.GetString(1),
				  readingData.GetString(2));
				EmployeeListBox.Items.Add(employee);
			}
			readingData.Close();
			connection.Close();
		}
		EmployeeListBox.Items.Add($"Sync: {stopwatch.ElapsedMilliseconds:N0}ms");
	}

	private async void GetEmployeeAsyncButton_Click(object sender, RoutedEventArgs e)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		using (SqlConnection connection = new(ConnectionString))
		{
			await connection.OpenAsync();
			SqlCommand command = new(Sql, connection);
			SqlDataReader readingData = await command.ExecuteReaderAsync();
			while (await readingData.ReadAsync())
			{
				string employee = string.Format("{0}: {1} {2}",
				  await readingData.GetFieldValueAsync<int>(0),
				  await readingData.GetFieldValueAsync<string>(1),
				  await readingData.GetFieldValueAsync<string>(2));

				EmployeeListBox.Items.Add(employee);
			}
			await readingData.CloseAsync();
			await connection.CloseAsync();
		}
		EmployeeListBox.Items.Add($"Async: {stopwatch.ElapsedMilliseconds:N0}ms");
	}
}