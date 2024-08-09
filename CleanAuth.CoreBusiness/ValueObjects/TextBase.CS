using CleanAuth.CoreBusiness.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAuth.CoreBusiness.ValueObjects
{
    public abstract class TextBase : ValueObject<TextBase>, IComparable<TextBase>
    {
        private readonly string _value;

        public TextBase(string value, int maxLength, string propertyName, int allowedMaxLength, bool isRequired = true)
        {
            Validate(value, maxLength, propertyName, allowedMaxLength, isRequired);
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

        public static implicit operator string(TextBase obj)
        {
            if (obj == null)
                return null;

            return obj._value;
        }

        public override string ToString()
        {
            return _value;
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<string> { _value };
        }

        public int CompareTo(TextBase other)
        {
            return this._value.CompareTo(other._value);
        }
    }
}
