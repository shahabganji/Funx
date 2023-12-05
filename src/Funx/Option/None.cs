namespace Funx.Option
{
    public struct None
    {
        internal static readonly None Default = new();

        public override string ToString()
        {
            return "None";
        }
    }
}
