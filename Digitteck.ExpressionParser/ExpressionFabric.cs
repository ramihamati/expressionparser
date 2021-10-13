
using Digitteck.ExpressionParser.ExprCommon;
using Digitteck.ExpressionParser.ExprValidateAndCorrect;
using Digitteck.ExpressionParser.ExprValidateAndCorrect.Common;
using Digitteck.ExpressionParser.ExprModel;
using Digitteck.ExpressionParser.ExpressionComponentTree;
using Digitteck.ExpressionParser.ExpressionComponentTree.Base;
using Digitteck.ExpressionParser.FunctionWrapper;
using Digitteck.ExpressionParser.ExprParameter;
using System.Collections.Generic;
using System.Linq;

namespace Digitteck.ExpressionParser
{
    public class ExpressionFabric
    {
        public FunctionProvider FunctionProvider { get; private set; }

        public ParameterCollection ParameterCollection { get; private set; }

        public ExprOptions ExprOptions { get; private set; }

        public IEvaluator Evaluator { get; private set; }

        private ExpressionFabric(FunctionProvider functionProvider,
                                 ParameterCollection parameterCollection)
        {
            this.FunctionProvider = functionProvider;
            this.ParameterCollection = parameterCollection;
            this.ExprOptions = new ExprOptions();
            this.Evaluator = EvaluatorFactory.ExpressionEvaluator(functionProvider.FunctionNames,
                                                     parameterCollection.ParametersNames);
        }

        public static ExpressionFabric Create(FunctionProvider functionProvider,
                                              ParameterCollection parameterCollection)
        {
            return new ExpressionFabric(functionProvider, parameterCollection);
        }


        //return can be PInvalidComponent or can contain a child that is PInvalidComponent
        public IExprModel CreateModel(string expressionString)
        { 
            ///Check validations
            EvaluationResult evaluationResult =
              Evaluator.Evaluate(expressionString, this.ExprOptions);

            if (evaluationResult.ValidationResult.ArgumentError != ArgumentError.OK)
            {
                return new ExprModelNotValidated(evaluationResult);
            }
            ///create expression tree
            ExprComponent exprComponent =
                CreateComponentTree.Create(evaluationResult.CorrectedContext, this.FunctionProvider, this.ParameterCollection, this.ExprOptions);
            ///check for invalid components
            if (exprComponent is ExprInvalidComponent)
            {
                return new ExprModelInvalidComponent
                    ((ExprInvalidComponent)exprComponent, expressionString, evaluationResult.CorrectedContext);
            }

            ExprInvalidComponent exprInvalidComponent = GetInvalidComponentIfAny(exprComponent as ExprComplexComponent);

            if (exprInvalidComponent != null)
            {
                return new ExprModelInvalidComponent
                    (exprInvalidComponent, expressionString, evaluationResult.CorrectedContext);
            }
            ///create model
            return new ExprModel.ExprModel(this.FunctionProvider, this.ParameterCollection, exprComponent, this.ExprOptions);
        }

        private ExprInvalidComponent GetInvalidComponentIfAny(ExprComplexComponent exprComplexComponent)
        {
            foreach (ExprComponent component in exprComplexComponent.Children)
            {
                if (component is ExprInvalidComponent)
                    return component as ExprInvalidComponent;
                if (component is ExprComplexComponent)
                    return GetInvalidComponentIfAny(component as ExprComplexComponent);
            }
            return null;
        }
    }
}
