using System;
using System.Data;
using System.Data.SqlClient;

namespace Funx.Test.Api.Extensions
{
    public static class DbConnectionExtensions
    {
        public static R Using<TDisposable, R>(TDisposable disposable
            , Func<TDisposable, R> f) where TDisposable : IDisposable
        {
            using (disposable) return f(disposable);
        }
        
        public static void Using<TDisposable>(TDisposable disposable
            , Action<TDisposable> f) where TDisposable : IDisposable
        {
            using (disposable) f(disposable);
        }
        
        public static R Connect<R>(string connStr, Func<IDbConnection, R> f)
            => Using(new SqlConnection(connStr)
                , conn => { conn.Open(); return f(conn); });
        
        public static void Connect(string connStr, Action<IDbConnection> f)
            => Using(new SqlConnection(connStr)
                , conn => { conn.Open(); f(conn); });

    }
}
