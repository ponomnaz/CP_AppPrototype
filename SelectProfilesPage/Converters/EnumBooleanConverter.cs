using System.Globalization;

namespace CP_AppPrototype.SelectProfilesPage.Converters
{
    /// <summary>
    /// Converts an enum value to a boolean and vice versa, based on a provided parameter.
    /// This is useful for binding enum values to boolean properties (e.g., checkbox states).
    /// </summary>
    public class EnumBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Converts the enum value to a boolean based on the given parameter.
        /// </summary>
        /// <param name="value">The enum value to be converted to a boolean.</param>
        /// <param name="targetType">The target type (expected to be boolean).</param>
        /// <param name="parameter">The value to compare against the enum (usually a string).</param>
        /// <param name="culture">Culture information (unused in this case).</param>
        /// <returns>Returns true if the enum value matches the parameter; otherwise, false.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Ensure the value is not null and parameter is a string.
            if (value != null && parameter is string parameterString)
            {
                // Return true if the enum value matches the parameter; otherwise, false.
                return value.ToString().Equals(parameterString, StringComparison.OrdinalIgnoreCase);
            }

            // Default return false if the conditions are not met.
            return false;
        }

        /// <summary>
        /// Converts a boolean value back to the corresponding enum value, using the provided parameter.
        /// </summary>
        /// <param name="value">The boolean value indicating whether to convert back.</param>
        /// <param name="targetType">The target enum type to convert to.</param>
        /// <param name="parameter">The string representing the enum value to be parsed.</param>
        /// <param name="culture">Culture information (unused in this case).</param>
        /// <returns>The enum value if the boolean is true and the parameter is valid, otherwise null.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is a boolean and is true
            if (value is bool boolValue && boolValue && parameter is string parameterString)
            {
                try
                {
                    // Try to parse the enum based on the provided parameter
                    return Enum.Parse(targetType, parameterString, true); // Case-insensitive parse
                }
                catch (ArgumentException)
                {
                    // Return null if the parsing fails (invalid enum value)
                    return null;
                }
            }

            // Return null if conditions are not met (either false or invalid parameter)
            return null;
        }
    }
}
