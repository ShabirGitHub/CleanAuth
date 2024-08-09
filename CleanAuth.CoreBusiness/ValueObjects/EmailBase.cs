using CleanAuth.CoreBusiness.Exceptions;
using CleanAuth.CoreBusiness.RegexUtilities;

namespace CleanAuth.CoreBusiness.ValueObjects
{
    public abstract class EmailBase : ValueObject<EmailBase>
    {
        private string _value;

        public EmailBase(string value)
        {
            if (value != null)
                _value = value.ToLower();
            else
                _value = value;
        }

        public static implicit operator string(EmailBase email)
        {
            return email != null ? email._value : null;
        }

        public override string ToString()
        {
            if (_value != null)
                return _value;

            return null;
        }

        protected static void Validate(string value, string propertyName)
        {
            RegexEmailValidator validator = new RegexEmailValidator();

            if (!validator.IsValidEmail(value))
                throw new InvalidEmailException($"{propertyName} is not valid.");

            if (value.Length > 120)
                throw new InvalidEmailException($"{propertyName} can be up to 120 chars long.");
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object> { _value };
        }

        public static IEnumerable<EmailBase> FromSemicolonSeparatedEmails<TEmail>(string semicolonSepratedEmails, string propertyName = "Email")
            where TEmail : EmailBase
        {
            if (string.IsNullOrEmpty(semicolonSepratedEmails))
                return Enumerable.Empty<TEmail>();

            var emails = semicolonSepratedEmails.Split(';');
            emails = emails.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            if (typeof(TEmail) == typeof(Email))
                return emails.Select(x => new Email(x, propertyName));
            else
                return emails.Select(x => new OptionalEmail(x, propertyName));
        }

        public static string ToSemicolonSeparatedEmails<TEmail>(IEnumerable<TEmail> emails) where TEmail : EmailBase
        {
            if (!emails.Any())
                return null;

            return string.Join(";", emails.Select(x => x.ToString()));
        }
    }
}
