using CleanAuth.CoreBusiness.Exceptions;
using CleanAuth.CoreBusiness.RegexUtilities;

namespace CleanAuth.CoreBusiness.ValueObjects
{
    public class Email
    {
        private string _value;
        public Email(string value, string propertyName)
        {
            if (value != null)
                _value = value.ToLower();
            else
                _value = value;

            Validate(_value, propertyName);
        }

        public static void Validate(string value, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidEmailException($"{propertyName} is required.");
         
            RegexEmailValidator validator = new RegexEmailValidator();

            if (!validator.IsValidEmail(value))
                throw new InvalidEmailException($"{propertyName} is not valid.");

            if (value.Length > 120)
                throw new InvalidEmailException($"{propertyName} can be up to 120 chars long.");
        }

        public override string ToString() => _value;

        public static implicit operator string(Email email) => email._value;
    }

}
