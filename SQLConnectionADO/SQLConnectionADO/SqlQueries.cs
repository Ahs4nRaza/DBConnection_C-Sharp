using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLConnectionADO
{
    public static  class SqlQueries
    {
       public static readonly string DropEmployeesTable = "DROP TABLE IF EXISTS EMPLOYEES;";

       public static readonly string CreateEmployeesTable = @"
        CREATE TABLE EMPLOYEES (
            EMP_ID INT PRIMARY KEY IDENTITY(1,1),
            EmployeeName NVARCHAR(255) NOT NULL
        );";

       public static readonly string InsertEmployees = @"
        INSERT INTO EMPLOYEES (EmployeeName) VALUES ('John Doe'), ('Jane Smith'), ('Jack Hill');";

       public static readonly string DropProcedure = @"
        IF OBJECT_ID('UpdateEmployeeName', 'P') IS NOT NULL
            DROP PROCEDURE UpdateEmployeeName;";

       public static readonly string CreateProcedure = @"
        CREATE PROCEDURE UpdateEmployeeName
            @OldName NVARCHAR(255),
            @NewName NVARCHAR(255)
        AS
        BEGIN
            UPDATE EMPLOYEES
            SET EmployeeName = @NewName
            WHERE EmployeeName = @OldName;
        END";
    }

}
