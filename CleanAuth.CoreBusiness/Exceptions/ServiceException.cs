namespace CleanAuth.CoreBusiness.Exceptions
{
    public class ServiceException : DomainException
    {
        public ServiceException(string message)
            : base(message)
        { }

        public ServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
