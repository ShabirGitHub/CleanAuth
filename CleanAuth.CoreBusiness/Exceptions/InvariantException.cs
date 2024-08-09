namespace CleanAuth.CoreBusiness.Exceptions
{
    public class InvariantException : DomainException
    {
        public InvariantException(string message)
            : base(message)
        { }
    }
}

