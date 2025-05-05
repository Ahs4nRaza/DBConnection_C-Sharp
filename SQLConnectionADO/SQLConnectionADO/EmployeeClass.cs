using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SQLConnectionADO
{
    public class EmployeeClass(SqlConnection connectionString)
    {
        private readonly SqlConnection connectionString = connectionString;
     

        public void CreateEmployeeTable()
        {
                // Drop the table & stored procedure  if it exists
                Execute(SqlQueries.DropEmployeesTable);
                Execute(SqlQueries.DropProcedure);

                // Now create the table stored procedure
                Execute(SqlQueries.CreateEmployeesTable);
                Execute(SqlQueries.CreateProcedure);
        }

        public void InsertTestData()
        {
            Execute(SqlQueries.InsertEmployees);
        }

        public void UpdateEmployeeName(string oldName, string newName)
        {
            using var cmd = new SqlCommand("UpdateEmployeeName", connectionString)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@OldName", SqlDbType.NVarChar, 255).Value = oldName;
            cmd.Parameters.Add("@NewName", SqlDbType.NVarChar, 255).Value = newName;
            cmd.ExecuteNonQuery();
        }

        public void DisplayEmployeeRecords()
        {
            using var cmd = new SqlCommand("SELECT * FROM EMPLOYEES", connectionString);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["EMP_ID"]}, Name: {reader["EmployeeName"]}");
            }
        }

        public void DisplayAndModifyEmployeeRecords()
        {
            
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM EMPLOYEES", connectionString))
            {
                DataTable employeeTable = new DataTable();
                adapter.Fill(employeeTable);  // Fill DataTable with data from the database

                if (employeeTable.Rows.Count > 0)
                {
                    // Update the last employee name 
                    employeeTable.Rows[2]["EmployeeName"] = "Updated Name";

                    // Add a new employee 
                    DataRow newRow = employeeTable.NewRow();  
                    newRow["EmployeeName"] = "New Employee";
                    employeeTable.Rows.Add(newRow);

                    // Delete the second employee 
                    if (employeeTable.Rows.Count > 1)
                    {
                        employeeTable.Rows[1].Delete();
                    }
                }

                // Using SqlCommandBuilder to auto-generate SQL commands and update the database
                using (SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter))
                {
                    adapter.Update(employeeTable);  // Apply changes to the database
                }
            }

            Console.WriteLine("Changes saved to the database.");
            DisplayEmployeeRecords();
        }


        private void Execute(string query)
        {
            using var cmd = new SqlCommand(query, connectionString);
            cmd.ExecuteNonQuery();
        }

    }
}
