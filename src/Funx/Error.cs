namespace Funx
{
    public class Error
    {
        public virtual string Message { get; }

        protected Error()
        {
        }
        
        public Error(string message) => this.Message = message;
    }
}
