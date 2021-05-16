namespace Funx.Test.Api.Models
{
    public sealed class ConnectionString
    {
        private string _value;

        public ConnectionString(string connectionString)
        {
            this._value = connectionString;
        }

        public static implicit operator string(ConnectionString c)
            => c._value;
        
        public static implicit  operator ConnectionString(string s)
                => new ConnectionString(s);

        public override string ToString() => _value;
    }
}
