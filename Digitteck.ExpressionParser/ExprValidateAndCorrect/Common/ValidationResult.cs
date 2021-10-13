namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Common
{
    public class ValidationResult
    {
        public ArgumentError ArgumentError { get; private set; }

        public string Message { get; private set; }

        public ValidationResult(ArgumentError argumentError, string message)
        {
            this.ArgumentError = argumentError;
            this.Message = message;
        }
        public static ValidationResult OK => new ValidationResult(ArgumentError.OK, "");
    }
}
