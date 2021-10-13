namespace Digitteck.ExpressionParser.FunctionWrapper.Base
{
    public class ValidatorsResult
    {
        public bool IsValid { get; private set; }

        public string ErrorMessages { get; private set; }

        public ValidatorsResult(bool isValid , string errorMessage)
        {
            this.IsValid = isValid;
            this.ErrorMessages = errorMessage;
        }

        public static ValidatorsResult OK => new ValidatorsResult(true, "");
    }
}
