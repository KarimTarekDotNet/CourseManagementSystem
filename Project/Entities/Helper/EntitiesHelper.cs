namespace Project.Entities.Helper
{
    public static class EntitiesHelper
    {
        public static string UpdateFirstName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("First name is required.");

            if (value.Any(char.IsDigit))
                throw new ArgumentException("First name cannot contain digits.");

            return value.Trim();
        }

        public static string UpdateLastName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Last name is required.");

            if (value.Any(char.IsDigit))
                throw new ArgumentException("Last name cannot contain digits.");

            return value.Trim();
        }

        public static string UpdateStringValue(string? value, string propName)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Any(char.IsDigit))
                throw new ArgumentException($"{propName} cannot contain digits or be whitespace.");

            return value.Trim();
        }

        public static string UpdateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");
            if (email.Length > 100)
                throw new ArgumentException("Email is too long.");

            return email.Trim();
        }

        public static string UpdatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("phone number is required.");

            return phoneNumber.Trim();
        }
    }
}
