using Digitteck.ExpressionParser.ExprCommon;
using System.Text;
using System.Linq;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Helper;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;

namespace Digitteck.ExpressionParser.ExprValidateAndCorrect.Bases
{
    public abstract class ExpressionWordValidate : IValidator
    {
        /// Will proceed only with words that pass this testing. Is Proceed Is null then this
        /// condition is ignored
        private Proceed<string> ProceedWith;

        protected virtual Proceed<string> Proceed(ExprOptions exprOptions)
        {
            return null;
        }
        /// <summary>
        /// Will receive a string, decompose it by groups containing only chInNaming (names) and evaluate those names
        /// If a name is not valid it will return the error code for it
        /// </summary>
        public ValidationResult Validate(string expression, ExprOptions exprOptions)
        {
            StringBuilder wordBuilder = new StringBuilder();

            this.ProceedWith = Proceed(exprOptions);

            StringDestructureIterate stringDestructure =
                StringDestructureIterate.By(expression,
                    ch => exprOptions.AllCharactersInNamesAndNumbers.Contains(ch));

            ValidationResultWord result = ValidationResultWord.OK;
            var met = stringDestructure.ElementsMeetCondition.Select(x=>x.Context).ToList();
            var not = stringDestructure.ElementsDoNotMeetCondition.Select(x => x.Context).ToList();

            stringDestructure.ForEachIfConditionMet((str, startIndex, endIndex) =>
             {
                 bool __proceed = true;
                 if (this.ProceedWith != null)
                 {
                     if (this.ProceedWith.ShouldNotProceed(str))
                     {
                         __proceed = false;
                     }
                 }
                 if (__proceed)
                 {
                     result = ValidateName(str, startIndex, endIndex, expression, exprOptions);
                     if (result.ArgumentError != ArgumentError.OK)
                     {
                         stringDestructure.StopIterating();
                     }
                 }
             });
            stringDestructure.Eval();

            return result;
        }
        /// <summary>
        /// Method to be implemented by child. Will be called to validate the words of the expression 
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="startIndex">Name start index</param>
        /// <param name="endIndex">Name end index (position of last character)</param>
        /// <param name="expression">Context</param>
        /// <returns></returns>
        protected abstract ValidationResultWord ValidateName(
              string name, int startIndex, int endIndex, string expression, ExprOptions exprOptions);
    }
}
