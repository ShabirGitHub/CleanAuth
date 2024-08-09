namespace CleanAuth.CoreBusiness.Exceptions
{
    public class InvalidEmailException : DomainException
    {
        public InvalidEmailException(string message)
            : base(message)
        { }
    }
}
