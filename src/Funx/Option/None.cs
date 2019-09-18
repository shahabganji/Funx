namespace Funx.Option
{
    public struct None
    {
        internal static readonly None Default = new None();

        public override string ToString()
        {
            return "None";
        }
    }
}