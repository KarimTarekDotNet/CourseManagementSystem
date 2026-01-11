using System.Text.RegularExpressions;

namespace Project.Checks
{
    public static class Guard
    {
        public static string AgainstNullOrWhiteSpace(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{nameof(value)} cannot be empty");

            return value.Trim();
        }

        public static string AgainstDigit(string value)
        {
            if (value.Any(char.IsDigit))
                throw new ArgumentException($"{nameof(value)} cannot contain digits");
            return value;
        }
        public static string AgainstDigitAndMayBeNull(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            value = AgainstDigit(value);
            return value;
        }

        public static string AgainstNullOrWhiteSpaceAndDigits(string value)
        {
            value = AgainstNullOrWhiteSpace(value);
            value = AgainstDigit(value);
            return value;
        }

        public static int AgainstNonPositive(int value)
        {
            if (value <= 0)
                throw new ArgumentException($"{nameof(value)} must be greater than zero");

            return value;
        }

        public static string AgainstInvalidEmail(string email)
        {
            email = AgainstNullOrWhiteSpace(email);

            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                throw new ArgumentException("Invalid email format");
            return email;
        }

        public static string AgainstInvalidPhone(string phone)
        {
            phone = AgainstNullOrWhiteSpace(phone);

            if (!Regex.IsMatch(phone, @"^\d{11}$"))
                throw new ArgumentException("Invalid phone number");

            return phone;
        }
    }
}
