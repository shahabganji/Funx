using System;
using System.Collections.Generic;
using Dapper;
using Funx.Test.Api.Models;
using SqlTemplate = System.String;
using static Funx.Test.Api.Extensions.DbConnectionExtensions;

namespace Funx.Test.Api.Extensions
{
    public static class ConnectionStringExt
    {
        public static Func<SqlTemplate
                , object
                , Exceptional<ValueTuple>>
            TryExecute( this ConnectionString conn )
            => ( sql, t) =>
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
        
       
    }
}
