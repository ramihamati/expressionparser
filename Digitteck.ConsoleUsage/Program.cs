using System;
using Digitteck.ExpressionParser;
using Digitteck.ExpressionParser.ExprModel;
using Digitteck.ExpressionParser.ExprCommon.WrappedFunctions;
using Digitteck.ExpressionParser.ExpressionComponentTree.Base;
using Digitteck.ExpressionParser.FunctionWrapper;
using Digitteck.ExpressionParser.ExprParameter;
using Digitteck.ExpressionParser.ExprParameter.Bases;
using Digitteck.ExpressionParser.FunctionWrapper.Convertible;
using Digitteck.ExpressionParser.ExprCommon.Wrapped;

namespace ConsoleApp1
{
    public class Room
    {
        public Spatial Spatial { get; set; }
    }
    public class Spatial
    {
        public double Perimeter { get; set; }
        public double Area { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            ConverterManager converterManager = ConverterManager.Create(typeof(Convertibles));
            FunctionProvider functionProvider = FunctionProvider.Create(converterManager, typeof(Functions), typeof(Functions2));

            ParameterInt32 height = new ParameterInt32("height", 20);
            ParameterDouble volume = new ParameterDouble("volume", 22);
            ParameterInstance myRoom = new ParameterInstance("myRoom", new Room { Spatial = new Spatial { Area = 100.5, Perimeter = 22.5} });

            ParameterCollection parameterCollection = new ParameterCollection(height, volume, myRoom);

            ExpressionFabric expressionFabric = ExpressionFabric.Create(functionProvider, parameterCollection);

            String expr11 = "2147483600+1000";
            String expr12 = "myRoom.Spatial.Area + Sqrt(myRoom.Spatial.Perimeter) * (-1)";

            string expr2 = "-Max(List(1,2,3,4,5,6, Max(List(10,22)) ))";
            string expr1 = "Max(List(12,2, (1+2)))";
            string expr3 = "Max(List(1,2,3)) + Sqrt(1)";

            IExprModel model1 = expressionFabric.CreateModel(expr2);

            switch (model1)
            {
                case ExprModelInvalidComponent emiData:
                    Console.WriteLine(emiData.ExprInvalidComponent.ErrorMessage);
                    break;
                case ExprModelNotValidated emnData:
                    Console.WriteLine("Corrected is " + emnData.CorrectedContext);
                    Console.WriteLine(emnData.ValidationResult.Message);
                    Console.WriteLine("Err : " + emnData.ValidationResult.ArgumentError);
                    break;
                default:
                    EvaluateComponentResult<ExprComponent> result = model1.GetResult();
                    if (!result.IsValid)
                        Console.WriteLine(result.ErrorMessage);
                    else
                    {
                        ParameterObject paramResult = ((ExprParameterComponent)result.ExprComponent).ParameterBase;

                        Console.WriteLine(paramResult.GetValue());
                    }
                    break;

            }
            ConverterManager converterManager1 = ConverterManager.Create(typeof(Convertible));

            FunctionProvider fp = FunctionProvider.Create(converterManager1,typeof(Functions));

            ParameterDouble data1 = new ParameterDouble("data1", -2);
            ParameterDouble data2 = new ParameterDouble("data2", -12);
            ParameterObject[] doubles = new ParameterObject[] { data1, data2 };

            ParameterDoubleArr doubleArr1 = new ParameterDoubleArr(
                "doubleList", doubles);

            ParameterDoubleArr doubleArr2 = new ParameterDoubleArr(
                "doubleList", new double[] { 1, 33 });

            FunctionBase sqrt = fp.GetByName("Max").Value;

            var result1 = sqrt.Invoke(new ParameterObject[] { doubleArr1 });


            Console.WriteLine(result1.Errors);
            Console.WriteLine(result1.Result.GetValue());
            //////var aa = 1;

        }
    }
}
