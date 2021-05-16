namespace Funx
{
    public class Error
    {
        public virtual string Message { get; }
        
        public Error(string message) => this.Message = message;
    }
}
