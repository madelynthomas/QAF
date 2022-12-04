using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace QAF.DataObjects
{
    public static class DatabaseAccess
    {
        private static readonly IDataSource dataSource;

        static DatabaseAccess()
        {
            dataSource = InitializeDataSource();
        }

        private static IDataSource InitializeDataSource()
        {
            switch (ConfigurationManager.AppSettings.Get("Environment").ToLower())
            {
                case Environment.Dev:
                    return new DataSourceDev();
                case Environment.Qa:
                    return new DataSourceQa();
                case Environment.Uat:
                    return new DataSourceUat();
                default:
                    return new DataSourceQa();
            }
        }

        public static List<Student> GetStudents(int count)
        {
            Console.WriteLine("Getting student information from StudentEnrollmentAPI");

            DataTable dataTable = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection($@"
                Data Source = {dataSource.Database};
                Initial Catalog = studentenrollment;
                Integrated Security = true;"))
            {
                sqlConnection.Open();

                string query = $@"
                    SELECT TOP {count}
                        id AS student_id,
                        first_name,
                        last_name,
                        CONVERT(VARCHAR(10), date_of_birth, 101) AS date_of_birth,
                        gender,
                        grade_level
                    FROM
                        students
                    WHERE
                        is_enrolled = '1'
                        AND is_graduated = '0'";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(query, sqlConnection)
                };

                sqlDataAdapter.Fill(dataTable);

                List<Student> student = new List<Student>();

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DateTime birthday = DateTime.Parse(dataRow.ItemArray[3].ToString());

                    student.Add(new Student
                    {
                        Id = dataRow.ItemArray[0].ToString(),
                        FirstName = dataRow.ItemArray[1].ToString(),
                        LastName = dataRow.ItemArray[2].ToString(),
                        BirthdayDay = birthday.Day,
                        BirthdayMonth = birthday.Month,
                        BirthdayYear = birthday.Year,
                        Gender = dataRow.ItemArray[4].ToString(),
                        GradeLevel = dataRow.ItemArray[5].ToString()
                    });
                }

                return student;
            }
        }
    }
}
