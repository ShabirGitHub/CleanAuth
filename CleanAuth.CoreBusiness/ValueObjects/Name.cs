namespace CleanAuth.CoreBusiness.ValueObjects
{
    public class Name : TextBase
    {
        private const int _maxAllowedLength = 100;

        public Name(string value, int maxLength, string propertyName = "Name", bool isRequired = true)
            : base(value, maxLength, propertyName, _maxAllowedLength, isRequired)
        { }

        public static void Validate(string value, int maxLength, bool isRequired)
        {
            Validate(value, maxLength, "Name", _maxAllowedLength, isRequired);
        }
    }
}
