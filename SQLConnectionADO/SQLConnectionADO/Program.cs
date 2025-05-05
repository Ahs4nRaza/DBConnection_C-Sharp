using Microsoft.Data.SqlClient;
using System.Configuration;
using SQLConnectionADO;

internal class Program
{
    private static void Main()
    {
        var connectionString = ConfigurationManager.ConnectionStrings["ADOConnectionString"].ConnectionString;

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        Console.WriteLine("Connection Opened");

        var employeeClass = new EmployeeClass(connection);
        employeeClass.CreateEmployeeTable();
        employeeClass.InsertTestData();

        Console.WriteLine("Before update:");
        employeeClass.DisplayEmployeeRecords();

        employeeClass.UpdateEmployeeName("John Doe", "Test User");

        Console.WriteLine("After update:");
        employeeClass.DisplayEmployeeRecords();

        Console.WriteLine("Using SQL ADAPTER TO UPDATE, ADD AND DELETE AND EMPLOYEE");
        employeeClass.DisplayAndModifyEmployeeRecords();
        

    }
}
