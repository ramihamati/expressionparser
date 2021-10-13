namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Common
{
    public class ValidationResultChar : ValidationResult
    {
        public char Context { get; private set; }
        public int Index { get; private set; }

        public ValidationResultChar(ArgumentError argumentError, char context, int index, string message)
            :base(argumentError, message)
        {
            this.Context = context;
            this.Index = index;
        }
        public static new ValidationResultChar OK => 
            new ValidationResultChar(ArgumentError.OK, default(char), default(int), "");
    }
}
