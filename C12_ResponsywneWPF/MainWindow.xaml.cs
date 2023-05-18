using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Windows;

namespace ResponsywneWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow() => InitializeComponent();

	//private const string connectionString =
	//  "Data Source=AORUS\\SQLEXPRESS;" +
	//  "Initial Catalog=Northwind;" +
	//  "Integrated Security=true;" +
	//  "MultipleActiveResultSets=true;";

	private const string connectionString = "Server=AORUS\\SQLEXPRESS;Database=Northwind;User Id=dbuser;Password=1234;TrustServerCertificate=True;";

	private const string sql = "WAITFOR DELAY '00:00:05';SELECT EmployeeId, FirstName, LastName FROM Employees";

	private void PobierzPracownikowSyncButton_Click(object sender, RoutedEventArgs e)
	{
		Stopwatch stoper = Stopwatch.StartNew();
		using (SqlConnection polaczenie = new(connectionString))
		{
			polaczenie.Open();
			SqlCommand polecenie = new(sql, polaczenie);
			SqlDataReader czytanieDanych = polecenie.ExecuteReader();
			while (czytanieDanych.Read())
			{
				string pracownik = string.Format("{0}: {1} {2}",
				  czytanieDanych.GetInt32(0),
				  czytanieDanych.GetString(1),
				  czytanieDanych.GetString(2));
				PracownicyListBox.Items.Add(pracownik);
			}
			czytanieDanych.Close();
			polaczenie.Close();
		}
		PracownicyListBox.Items.Add($"Sync: {stoper.ElapsedMilliseconds:N0}ms");
	}

	private async void PobierzPracownikowAsyncButton_Click(object sender, RoutedEventArgs e)
	{
		Stopwatch stoper = Stopwatch.StartNew();
		using (SqlConnection polaczenie = new(connectionString))
		{
			await polaczenie.OpenAsync();
			SqlCommand polecenie = new(sql, polaczenie);
			SqlDataReader czytanieDanych = await polecenie.ExecuteReaderAsync();
			while (await czytanieDanych.ReadAsync())
			{
				string pracownik = string.Format("{0}: {1} {2}",
				  await czytanieDanych.GetFieldValueAsync<int>(0),
				  await czytanieDanych.GetFieldValueAsync<string>(1),
				  await czytanieDanych.GetFieldValueAsync<string>(2));

				PracownicyListBox.Items.Add(pracownik);
			}
			await czytanieDanych.CloseAsync();
			await polaczenie.CloseAsync();
		}
		PracownicyListBox.Items.Add($"Async: {stoper.ElapsedMilliseconds:N0}ms");
	}
}