namespace CleanAuth.CoreBusiness.ValueObjects
{
    public class OptionalEmail : EmailBase
    {
        public OptionalEmail(string value, string propertyName)
            : base(value)
        {
            Validate(value, propertyName);
        }

        public static new void Validate(string value, string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(value))
                EmailBase.Validate(value, propertyName);
        }
    }
}
