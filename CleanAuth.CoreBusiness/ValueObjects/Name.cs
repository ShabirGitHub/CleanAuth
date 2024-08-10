using CleanAuth.CoreBusiness.Exceptions;

namespace CleanAuth.CoreBusiness.ValueObjects
{
    public class Name
    {
        private const int _maxAllowedLength = 100;
        string _value;
        public Name(string value, int maxLength, string propertyName = "Name", bool isRequired = true)    
        {
            Validate(value, maxLength, propertyName, _maxAllowedLength, isRequired);
            _value = value;
        }

        public static void Validate(string value, int maxLength, string propertyName, int allowedMaxLength, bool isRequired)
        {
            if (isRequired && string.IsNullOrWhiteSpace(value))
                throw new InvariantException($"{propertyName} cannot be empty.");

            if (isRequired && maxLength <= 0)
                throw new InvariantException($"Max length must be more than zero for a required {propertyName}.");

            if (maxLength > allowedMaxLength)
                throw new InvariantException($"{propertyName} cannot be more than {allowedMaxLength} characters.");

            if (value != null && value.Length > maxLength)
                throw new InvariantException($"{propertyName} cannot be more than {maxLength} characters.");
        }

        public override string ToString() => _value;

        public static implicit operator string(Name name) => name._value;
    }
}
