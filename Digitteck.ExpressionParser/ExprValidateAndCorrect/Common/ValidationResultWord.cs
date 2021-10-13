namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Common
{
    public class ValidationResultWord : ValidationResult
    {
        public string Context { get; private set; }
        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }

        public ValidationResultWord
            (ArgumentError argumentError, string context, int startIndex, int endIndex, string message)
            :base(argumentError, message)
        {
            this.Context = context;
            this.StartIndex = startIndex;
            this.EndIndex = endIndex;
        }
        public static new ValidationResultWord OK =>
            new ValidationResultWord(ArgumentError.OK, null, default(int), default(int), "");
    }
}
