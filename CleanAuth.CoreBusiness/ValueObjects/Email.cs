namespace CleanAuth.CoreBusiness.ValueObjects
{
    public class Email : EmailBase
    {
        public Email(string value, string propertyName)
            : base(value)
        {
            Validate(value, propertyName);
        }

        public static new void Validate(string value, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException($"{propertyName} is required.");

            EmailBase.Validate(value, propertyName);
        }
    }
}
