using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Funx.Extensions;
using Funx.Test.Api.Controllers;
using Funx.Test.Api.Controllers.Queries;
using Funx.Test.Api.Models;
using Microsoft.Extensions.DependencyInjection;
using SqlTemplate = System.String;

namespace Funx.Test.Api.Extensions.IoC
{
    using static Funx.Test.Api.Extensions.DbConnectionExtensions;

    public static class ServiceProviderExtensions
    {
        public static void AddEmployeeServices(this IServiceCollection services, ConnectionString connectionString)
        {
            var insertCommand = Sql.TryExecute
                .Apply(connectionString)
                .Apply("Insert command");
            insertCommand(new { Id = Guid.NewGuid() , FirstName = "Saeed" });
            
            var queryById = Sql.Query<Employee>()
                    .Apply(connectionString)
                    .Apply(Sql.Queries.GetEmployee);
            
            var queryEmployees = connectionString.Query<Employee>();
            var queryByLastName = queryEmployees.Apply(Sql.Queries.GetEmployeesByLastName);

            Option<Employee> LookupEmployee(FindEmployee emp) => queryById(new {Id = emp.Id}).FirstOrDefault();

            IEnumerable<Employee> FindEmployeeByLastName(string lastName)
                => queryByLastName(new {LastName = lastName});

            services.AddSingleton<Func<FindEmployee, Option<Employee>>>(provider => LookupEmployee);
            services.AddSingleton<Func<string, IEnumerable<Employee>>>(provider => FindEmployeeByLastName);
        }
    }

    public static class Sql
    {
        public static Func<ConnectionString
                , SqlTemplate
                , object
                , Exceptional<ValueTuple>>
            TryExecute
            => (conn, sql, t) =>
            {
                try
                {
                    Connect(conn, connection => connection.Execute(sql, t));
                }
                catch (Exception ex)
                {
                    return ex;
                }

                return new ValueTuple();
            };

        public static Func<ConnectionString,SqlTemplate, object, IEnumerable<T>> Query<T>()
            => (connectionString, sql, param)
                => Connect(connectionString, connection => connection.Query<T>(sql, param));
        
        public static Func<SqlTemplate, object, IEnumerable<T>> Query<T>(
            this ConnectionString connectionString)
            => (sql, param)
                => Connect(connectionString, connection => connection.Query<T>(sql, param));
        public class Queries
        {
            private static SqlTemplate sel = "SELECT * FROM EMPLOYEES";
            public static SqlTemplate GetEmployee { get; } = $"{sel} WHERE ID = @Id";
            public static SqlTemplate GetEmployeesByLastName { get; } = $"{sel} WHERE LASTNAME = @LastName";
        }
    }
}
