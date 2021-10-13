<h2>1. Evaluating an expression</h2>

```
ConverterManager converterManager = ConverterManager.Create(typeof(Convertibles));
FunctionProvider functionProvider = FunctionProvider.Create(converterManager, typeof(Functions), typeof(Functions2));
 
ParameterInt32 height = new ParameterInt32("height", 20);
ParameterDouble volume = new ParameterDouble("volume", 22);
ParameterInstance myRoom = new ParameterInstance("myRoom", new Room { Spatial = new Spatial { Area = 100.5, Perimeter = 22.5} });
ParameterCollection parameterCollection = new ParameterCollection(height, volume, myRoom);
ExpressionFabric expressionFabric = ExpressionFabric.Create(functionProvider, parameterCollection);
 
string expr1 = "myRoom.Spatial.Area + Sqrt(myRoom.Spatial.Perimeter) * (-1)";
string expr2 = "-Max(List(1,2,3,4,5,6, Max(List(10,22)) ))";
string expr3 = "Max(List(12,2, (1+2)))";
 
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
        //here is the result
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
```
<h2>2. Using a function</h2>

Our function has the following method implemented:


```
[Declared]
public static ParameterDouble Sqrt([MinValue(0)] ParameterDouble value)
{
    return new ParameterDouble(ParametersManager.NextName(), Math.Sqrt(value.GetValue()));
}
```

Define some parameters:


```
ParameterDouble data1 = new ParameterDouble("data1", -2);
ParameterDouble data2 = new ParameterDouble("data2", -12);
ParameterObject[] doubles = new ParameterObject[] {data1,data2};
ParameterDoubleArr doubleArr1 = new ParameterDoubleArr
("doubleList", doubles);
```

Creating the FunctionProvider:

```
ConverterManager converterManager1 = ConverterManager.Create(typeof(Convertible));
FunctionProvider fp = FunctionProvider.Create(converterManager1,typeof(Functions))
```
Getting the function:


```
FunctionBase sqrt = fp.GetByName("Max").Value;
var result1 = sqrt.Invoke(new ParameterObject[] { doubleArr1 });
```

Evaluating:

```
Console.WriteLine(result1.Errors);
Console.WriteLine(result1.Result.GetValue());
```
<h2>3. Parametters</h2>
**Hierarchy**
All parameters (simple or array like) derive from the base object ParameterObject.
The inheritance looks like:
`IParameterObject` interface is the interface defining the starting point of parameter composition


```
public interface IParameterObject
    {
        string Name { get; }
        object GetValue();
    }
```
Interface `IParameter` defines the operations to be implemented by different types of parameters. It also acts as the base of strongly types parameters.


```
public interface IParameter<T> : IParameterObject where T: IParameter<T>
    {
        T Op_Add(T other);
        T Op_Minus(T other);
        T Op_Mult(T other);
        T Op_Div(T other);
        T Op_Mod(T other);
        T Op_Pow(T other);
    }
```

`public abstract class ParameterObject : IParameterObject`
`public abstract class Parameter : ParameterObject, IParameter<Parameter>`

IParameterArray is inherited by array type parameters (ParameterDoubleArr, etc)


```
public interface IParameterArray<T> where T : ParameterObject
public abstract class ParameterBase<T> : Parameter
```
Collection
The parameter collection can store any type of parameter (ParameterDouble, ParameterDoubleArr, etc).

<h3>3.1 ParameterDouble</h3>
Represents a class that stores a double value.

**Syntax**

`public  class ParameterDouble : ParameterBase<double>`

**Constructor**

`public ParameterDouble(string name, double value) : base(name, value)`

**Methods**

Return the value of the parameter:

`double GetValue()`

Create a new parameter with a default name:

`public static ParameterDouble Create(double value)`

**Properties**

`string Name { get; protected set; }`

**Example**

`ParameterDouble volume = new ParameterDouble("volume", 100);`

<h3>3.2 ParameterInt32</h3>

Represents a class that stores an INT32 value.

**Syntax**

`public  class ParameterInt32 : ParameterBase<int>`

**Constructor**

`public ParameterInt32(string name, Int32 value) : base(name, value)`

**Methods**

Return the value of the parameter:

`Int32 GetValue()`

Create a new parameter with a default name:

`public static ParameterInt32 Create(Int32 value)`

**Properties**

`string Name { get; protected set; }`

**Example**

ParameterInt32 volume = new ParameterInt32("volume", 100);

<h3>3.3 ParameterInt64</h3>

Represents a class that stores an INT64 value.

**Syntax**

`public  class ParameterInt64 : ParameterBase<Int64>`

**Constructor**

`public ParameterInt64(string name, double value) : base(name, value)`

**Methods**

Return the value of the parameter:

`Int64 GetValue()`

Create a new parameter with a default name:

`public static ParameterInt64 Create(Int64 value)`

**Properties**

`string Name { get; protected set; }`

**Example**

`ParameterInt64 volume = new ParameterInt64("volume", 100);`

<h3>3.4 ParameterSingle</h3>

Represents a class that stores a float value.

**Syntax**

`public  class ParameterSingle : ParameterBase<Single>`

**Constructor**

`public ParameterSingle(string name, Single value) : base(name, value)`

**Methods**

Return the value of the parameter:

`Single GetValue()`

Create a new parameter with a default name:

`public static ParameterSingle Create(Single value)`

**Properties**

`string Name { get; protected set; }`

**Example**

`ParameterSingle volume = new ParameterSingle("volume", 100);`

<h3>3.5 ParameterDoubleArr</h3>

Represents a class that stores a collection of double values

**Syntax**
`public class ParameterDoubleArr : ParameterObjectBase<double[]>, IParameterArray<ParameterDouble>`

**Constructor**
 
```
public ParameterDoubleArr(string name, double[] value) : base(name)
```
```
public ParameterDoubleArr(string name, ParameterObject[] value) : base(name)
```
**Methods**
Returns an array of ParameterDouble items

`ParameterDouble[] GetObjects()`

Returns an array of double items

`double[] GetValue()`

**Properties**

`string Name { get; protected set; }`

**Example**
 ParameterDouble data1 = new ParameterDouble("data1", -2);
 
```
ParameterDouble data2 = new ParameterDouble("data2", -12);
 ParameterObject[] doubles = new ParameterObject[] { data1, data2 };
 ParameterDoubleArr doubleArr1 = new ParameterDoubleArr(
                "doubleList", doubles)
```
```
ParameterDoubleArr doubleArr2 = new ParameterDoubleArr(
         "doubleList", new double[] { 1, 33 });
```

<h3>3.6 ParameterInt32Arr</h3>

Represents a class that stores a collection of Int32 values

**Syntax**

`public class ParameterInt32Arr : ParameterObjectBase<Int32[]>, IParameterArray<ParameterInt32>`

`public ParameterInt32Arr(string name, Int32[] value) : base(name)`
`public ParameterInt32Arr(string name, ParameterObject[] value) : base(name)`

**Methods**

Returns an array of ParameterInt32 items

`ParameterInt32Arr[] GetObjects()`

Returns an array of double items

`Int32[] GetValue()`

**Properties**
`string Name { get; protected set; }`
 
**Example**

```
ParameterInt32 data1 = new ParameterInt32("data1", -2);
ParameterInt32 data2 = new ParameterInt32("data2", -12);
ParameterObject[] integers = new ParameterObject[] { data1, data2 };
ParameterInt32Arr intArr1 = new ParameterInt32Arr(
                "intList", integers);
```

`ParameterInt32Arr intArr2 = new ParameterInt32Arr("intList", new int[] { 1, 33 });`

<h3>3.7 ParameterInt64Arr</h3>

Represents a class that stores a collection of Int64 values

**Syntax**
`public class ParameterInt64Arr : ParameterObjectBase<Int64[]>, IParameterArray<ParameterInt64>`

**Constructor**

```
public ParameterInt64Arr(string name, Int64[] value) : base(name)
public ParameterInt64Arr(string name, ParameterObject[] value) : base(name)
```

**Methods**
Returns an array of ParameterInt64 items

`ParameterInt64Arr[] GetObjects()`

Returns an array of Int64 items

`Int64[] GetValue()`

**Properties**

`string Name { get; protected set; }`

**Example**
 
```
ParameterInt64 data1 = new ParameterInt64("data1", -2);
 ParameterInt64 data2 = new ParameterInt64("data2", -12);
 ParameterObject[] integers = new ParameterObject[] { data1, data2 };
 ParameterInt64Arr intArr1 = new ParameterInt64Arr("intList", integers);
```

```
ParameterInt64Arr intArr2 = new ParameterInt64Arr(
         "intList", new Int64[] { 1, 33 });
```

<h3>3.8 ParameterSingleArr</h3>

Represents a class that stores a collection of float values

**Syntax**

`public class ParameterSingleArr : ParameterObjectBase<Single[]>, IParameterArray<ParameterSingle>`

**Constructor**

```
public ParameterSingleArr(string name, Single[] value) : base(name)
public ParameterSingleArr(string name, ParameterObject[] value) : base(name)
```

**Methods**

Returns an array of ParameterSingle items

`ParameterSingleArr[] GetObjects()`

Returns an array of float items

`Single[] GetValue()`

**Properties**

`string Name { get; protected set; }`

**Example**
 
```
ParameterSingle data1 = new ParameterSingle("data1", -2);
 ParameterSingle data2 = new ParameterSingle("data2", -12);
 ParameterObject[] singles = new ParameterObject[] { data1, data2 };
 ParameterSingleArr singleArray = new ParameterSingleArr(
                "singles", singles);
```

 
```
ParameterSingleArr singleArray = new ParameterSingleArr(
         "intList", new float[] { 1, 33 });
```

<h3>3.9 ParametersManager</h3>

Represents a static class with several functionalities

**Syntax**
`public static class ParametersManager`

**Methods**
Returns a new generated name for a parameter

`static string NextName()`

**Properties**

`static string namePrefix`

If `DoubleBeforeSingle` is true then the attempt is to make a double instead of a single. This provides more precision to your data. Default value is true

`static bool DoubleBeforeSingle`

**Static Methods**
Trying to unbox a component and create a parameter component from it. If the object is int then a new instance of ParameterInt32 is created. Works for primary data and array types of primary data int, float, double, long, int[], float[], double[], long[])

`public static Maybe<ParameterObject> TryGetComponent(object data, ExprOptions exprOptions)`

Trying to get the parameter component out of a string

`public static Maybe<ParameterObject> TryGetNumberComponent(string expression, ExprOptions exprOptions)`

<h3>3.10 Add a new Operator</h3>

To add a new operator you have several steps to accomplish and for this I will present an added operator

**1. In class ExprOptions**

```
public List<char> AllowedOperators = "+-^*/%".ToList();
public char OperatorPlusSign = '+';
```
**2. Enum OperatorType**

 
```
public enum OperatorType
    {
        Plus,
        Minus,
        Multiply,
        Divide,
        Mod,
        Power
    }
```

**3. Class ExprOperatorComponent**

 
```
public static ExprComponent Create(char charOp, ExprOptions exprOptions)
        {
            if (exprOptions.HasOperatorFormat(charOp.ToString()))
            {
                switch (charOp)
                {
                    //then add operators
                    case '+':
                        return new ExprOperatorComponent("+", exprOptions, OperatorType.Plus);
                    case '-':
                        return new ExprOperatorComponent("-", exprOptions, OperatorType.Minus);
                    case '*':
                        return new ExprOperatorComponent("*", exprOptions, OperatorType.Multiply);
                    case '/':
                        return new ExprOperatorComponent("/", exprOptions, OperatorType.Divide);
                    case '%':
                        return new ExprOperatorComponent("%", exprOptions, OperatorType.Mod);
                    case '^':
                        return new  ExprOperatorComponent("^", exprOptions, OperatorType.Power);
                    default:
                        return new ExprInvalidComponent(charOp.ToString(), exprOptions, string.Format("{0} is not a valid operator", charOp));
                }
            }
            else
            {
                return new ExprInvalidComponent(charOp.ToString(), exprOptions, string.Format("{0} is not a valid operator", charOp));
            }
        }
```

**4. Class ExprModel – define operator strategies**

 
```
Dictionary<OperatorType, IOperation> opStrategies = new Dictionary<OperatorType, IOperation>()
            {
                { OperatorType.Plus, new Add() },
                { OperatorType.Minus, new Subtract() },
                { OperatorType.Divide, new Divide()},
                { OperatorType.Multiply, new Multiply()},
                { OperatorType.Mod, new Mod()},
                { OperatorType.Power, new Power()},
            };
```

**5. In PrimaryOperations there is one class for each operation**

 
```
public class Add : IOperation
    {
        public EvaluateComponentResult<ExprParameterComponent> 
            Result(ExprParameterComponent First, ExprParameterComponent Second)
        {
            return
                Utils.EvalAndOp(First, Second, null, (_first, _second) =>
                {
                    return _first.Op_Add(_second);
                });
        }
    }
```

**6. The EvalAndOp method evaluates expressions and attempts to get appropiate parameters.** It also provides means to check parameters before the operation (needed for /0 division)

         
```
public static EvaluateComponentResult<ExprParameterComponent> EvalAndOp
                (ExprParameterComponent first,
                 ExprParameterComponent second,
                 Func<ParameterObject, ParameterObject, EvaluateComponentResult<ExprParameterComponent>> postCheck,
                 Func<Parameter, Parameter, Parameter> operation)
```
   
The ExprParameterComponent class derives from ExprComponent and it has the following properties:

   
```
public string Context { get; protected set; }
   public ExprOptions ExprOptions { get; set; }
   public ParameterObject ParameterBase { get; private set; }
```

**7. The ParameterBase class provide abstract methods to implement these operations:**

 
```
public abstract Parameter Op_Add(Parameter other);
public abstract Parameter Op_Minus(Parameter other);
public abstract Parameter Op_Mult(Parameter other);
public abstract Parameter Op_Div(Parameter other);
public abstract Parameter Op_Mod(Parameter other);
public abstract Parameter Op_Pow(Parameter other);
```

**8. Each Parameter class overrides the above method but redirects action to a sepparate class that handles also overflow data**

  
```
public override Parameter Op_Add(Parameter other)
        {
            switch (other)
            {
                case ParameterInt32 other32:
                    return OverflowAct.AddWithOverflow(this.Value, other32.GetValue());
                case ParameterInt64 other64:
                    return OverflowAct.AddWithOverflow(this.Value, other64.GetValue());
                case ParameterSingle otherSingle:
                    return OverflowAct.AddWithOverflow(this.Value, otherSingle.GetValue());
                case ParameterDouble otherDouble:
                    return OverflowAct.AddWithOverflow(this.Value, otherDouble.GetValue());
                default:
                    throw new Exception("Unreachable");
            }
        }
```

**9. An example of the final operation in the overflow class:**


```
public static class OverflowAct
    {
        //Additions of primary numerical values
        public static Parameter AddWithOverflow(Int32 first, Int32 second)
        {
            int resultInt = first + second;
            if (resultInt >= second && resultInt >= first)
                return ParameterInt32.Create(resultInt);
            long resultLong = (long)first + second;

            return ParameterInt64.Create(resultLong);
        }

        public static Parameter AddWithOverflow(Int32 first, Int64 second)
        {
            long resultLong = (long)first + second;

            return ParameterInt64.Create(resultLong);
        }
}
```

<h3>3.11 ParameterCollection</h3>

**Syntax**
`public class ParameterCollection`

**Constructor**
 
```
public ParameterCollection()
 public ParameterCollection(params ParameterObject[] parameterObjects)
```

**Properties**

`public string[] ParametersNames{get;}`

**Methods**

```
public void Add(ParameterObject parameter)
public void Add(params ParameterObject[] parameters)
public Maybe<ParameterObject> TryGetParameter(string name)
```

**Usage**
ParameterCollection is used for the creation of the expression model. All parameters used in an expression are passed in this collection for evaluation:

 
```
ParameterInt32 height = new ParameterInt32("height", 20);
 ParameterDouble volume = new ParameterDouble("volume", 22);
 ParameterInstance myRoom = new ParameterInstance("myRoom", new Room { Spatial = new Spatial { 
   Area = 100.5, Perimeter = 22.5} });

 ParameterCollection parameterCollection = new ParameterCollection(height, volume, myRoom);

 ExpressionFabric expressionFabric = ExpressionFabric.Create(functionProvider, parameterCollection);
 String expr12 = "height*volume*myRoom.Spatial.Area + Sqrt(myRoom.Spatial.Perimeter) * (-1)";
 IExprModel model1 = expressionFabric.CreateModel(expr2);
```

<h2>4. ExprCommon</h2>

This namespace provides multiple helper functions used in the parser and also some configuration options

<h3>ExprOptions</h3>

ExprOptions represents a configurator for the expression parser.

**Constructor**
public ExprOptions()

**Fields**

```
public char OpenBracket = '(';
public char ClosedBracket = ')';
public List<char> AlternativeOpenBrackets = new List<char> { '{', '[' };
public List<char> AlternativeClosedBrackets = new List<char> { '}', ']' };

public List<char> AllowedNumbers = "0123456789".ToList();
public char AllowedNamePrefix = '_';
public List<char> AllowedLetters = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm".ToList();
public List<char> AllowedOperators = "+-^*/%".ToList();
public List<char> AllowedBrackets;
public char DecimalSeparator = '.';
public char GroupSeparator = ',';

public List<char> AllAlphaNumeric;
public List<char> AllAllowedCharacters;
public List<char> AllNamingCharacters;
public List<char> AllNumberCharacters;
public List<char> AllCharactersInNamesAndNumbers;//similar to AllNaming + Punctuation
/// <summary>
/// For Singular operators tests
/// </summary>
public char OperatorMinusSign = '-';
public char OperatorPlusSign = '+';
public char OperatorMultiplySign = '*';
public char OperatorDivideSign = '-';
public char OperatorModSign = '%';
public char OperatorExpSign = '^';
```

**Methods**
Determine if a string has a numeric format. Multiple dots are not ignored, they are determined and warned in a validation sequence

`public bool ContainsNumbersAndPoint(string value)`

Determines if a string has a numeric format.

`public bool HasNumericFormat(string value`)

Determines if a string has a valid naming format, it can start with an underscore or a letter and contain letters or numbers

`public bool HasNameFormat(string value)`

 `public bool HasOperatorFormat(string value)`

Checks if a string has the shape ‘someFunction(…)’

`public bool HasFunctionFormat(string value)`

<h3>Convertibles</h3>

Convertibles is a static class that define cast-ing methods. It allows you to define conversion ways from one parameter type to another.

In the namespace RHAPPExpressionParser.ExprCommon.Wrapped you can find Convertibles class with some predefined methods but you can define also other methods / classes.

Methods must be static and marked with Convertible Attribute.

**Syntax**

Converting a ParameterInt32 to a ParameterDouble


```
[Convertible]
public static ParameterDouble Convert(ParameterInt32 value)
   {
      return new ParameterDouble(value.Name, value.GetValue());
   }
```

Why?
When evaluating an expression with no parameters (or if you have multiple parameter types) the parser will determine the type of the parameter
The following expression will result in the addition of an Int32 type with a Single type. Or if in the RHAPPExpressionParser.ExprParameter.Bases.ParameterManager you set DoubleBeforeSingle = true then the addition will be between Int32 and Double. In this case the program needs to know how to convert Int32 value

`"10+1.2"`

**Usage**

```
ConverterManager converterManager = ConverterManager.Create(typeof(Convertibles));
FunctionProvider functionProvider = FunctionProvider.Create(converterManager, typeof(Functions), typeof(Functions2));

ParameterInt32 height = new ParameterInt32("height", 20);
ParameterDouble volume = new ParameterDouble("volume", 22);
ParameterCollection parameterCollection = new ParameterCollection(height, volume);
ExpressionFabric expressionFabric = ExpressionFabric.Create(functionProvider, parameterCollection);
```

<h2>5. FunctionWrapper</h2>
FunctionWrapper namespace offers the infrastracture for mapping functions that are used in the expression string.

<h3>5.1 FunctionBase</h3>
FunctionBase is the base class that stores the function name, it’s functionality, the arguments required and the argument validation components

**Syntax**
`public class FunctionBase`

**Constructor**
The constructor is private and Create function is used to generate a new instance.
Note:Do not create a new instance of the functions you are using, this is functionality is provided by class FunctionProvider.


```
public static FunctionBase Create(MethodInfo baseMethod, 
MethodInfo[] overloadMethods, ConverterManager converterManager)
```

**Properties**

```
public string Name { get; private set; }
public MethodInfo MethodInfo { get; private set; }
public Type OwnerType { get; private set; }
public FunctionDefinitionType FunctionDefinitionType { get; private set; }
public ArgumentInfo[] Parameters { get; private set; }
public FunctionBase[] Overloads { get; private set; }
public int ParametersCount;
private ConverterManager ConverterManager { get; set; }
```

**Methods**

`RuntimeEvaluateStatus Invoke(ParameterObject[] inputData)`

<h3>5.2 RuntimeEvaluateStatus</h3>

RuntimeEvaluateStatus is returned when a function is invoked in an expression.


```
public class RuntimeEvaluateStatus
{
    public RuntimeArgumentStatus RuntimeArgumentStatus { get; set; }
 
    public string Errors { get; set; }
 
    public ParameterObject Result { get; set; }
}
public enum RuntimeArgumentStatus
    {
        OK,
        ValuesNotValidated,
        NoMathingSignatureFound,
        NaN,
        RuntimeError,
        NullResult,
        UnableToCast
    }
```

<h3>5.3 Defining Functions</h3>
Predefined functions are store in `RHAPPExpressionParser.ExprCommon.WrappedFunctions.Functions.` You can follow this example to define other functions.

**Declared Attribute**
There is 1 attribute that is needed when defining functions. The declared attribute which sets up which function is the base and which is overloaded. You can have 1 base function and multiple overloaded functions.
Note: An error is thrown is there are overloaded functions but no base function detected.

```
public class DeclaredAttribute : Attribute
{
    public FunctionDefinitionType FunctionDefinitionType { get;set;}

    public DeclaredAttribute()
    {
        this.FunctionDefinitionType = FunctionDefinitionType.Base;
    }
}
public enum FunctionDefinitionType
{
    Base,
    Overload
}
```

**Function Definition**
A function is defined in a static class and then passed to the FunctionProvider instance


```
[Declared]
public static ParameterDouble Sqrt([MinValue(0)] ParameterDouble value)
{
    return new ParameterDouble(ParametersManager.NextName(), Math.Sqrt(value.GetValue()));
}
       
[Declared(FunctionDefinitionType = FunctionDefinitionType.Overload)]
public static ParameterDouble Sqrt([MinValue(0)] ParameterInt32 value)
{
    return new ParameterDouble(ParametersManager.NextName(), Math.Sqrt(value.GetValue()));
}
```

**Argument Validation Attribute**
These attributes are argument attributes and they must inherit interface IArgumentValid.
The validity of values passed are checked before the function is invoked.
If the argument is not valid an error message is sent.
Predefined attributes are stored in
RHAPPExpressionParser.FunctionWrapper.Attributes.

  
```
public interface IArgumentValid
    {
        bool IsValid(ParameterObject Context);
        string ErrorMessage { get; }
    }
```

Example of a predefined attribute:


```
public class MinValueAttribute : System.Attribute, IArgumentValid
{
    private double MinValue { get; set; }

    public MinValueAttribute(double minValue)
    {
        this.MinValue = minValue;
    }

    public string ErrorMessage =>
        string.Format("Value must be greater then {0}", this.MinValue);

    public bool IsValid(ParameterObject Context)
    {
        switch (Context)
        {
            case ParameterInt32 contextInt32:
                return contextInt32.GetValue() >= this.MinValue;
            case ParameterInt64 contextInt64:
                return contextInt64.GetValue() >= this.MinValue;
            case ParameterSingle contextSingle:
                return contextSingle.GetValue() >= this.MinValue;
            case ParameterDouble contextDouble:
                return contextDouble.GetValue() >= this.MinValue;
            default:
                return false;
        }
    }
}
```

<h3>5.4 Params</h3>

Params attribute let’s you define a function with variable number of arguments.
Params attribute must be on the last argument of the function and that argument must be an array type inheriting IParameterArray interface (ParameterInt32Arr, ParameterDoubleArr, etc..)

**Example**


```
[Declared(FunctionDefinitionType = FunctionDefinitionType.Base)]
public static ParameterDouble Max([Params] ParameterDoubleArr data)
{
    double result = data.GetValue().ToList().Aggregate((a, b) => Math.Max(a, b));
 
    return new ParameterDouble(ParametersManager.NextName(), result);
}
```

Now you can use the function by providing double values. Or int values if you have a convertible defined between int and double (one predefined one is in the Convertibles class)

`string myExpression = "Max(1,2,3,4.5,6,Max(10,22))";`

<h3>5.5 FunctionProvider</h3>

FunctionProvider will handle determining base and overloaded functions that are to be mapped. It receives as input the class where you defined your functions and the converterManager.

**Syntax**

`public class FunctionProvider`

**Properties**

```
public FunctionBase[] FunctionsBase { get; private set; }
 
public ConverterManager ConverterManager { get; private set; }
 
public string[] FunctionNames { get; set; }
```

**Creating a new instance**
`public static FunctionProvider Create(ConverterManager converterManager, params Type[] ownerClasses)`

**Usage**

```
ConverterManager converterManager = ConverterManager.Create(typeof(Convertibles));
FunctionProvider functionProvider = FunctionProvider.Create(converterManager, typeof(Functions), typeof(Functions2));
 
ParameterInt32 height = new ParameterInt32("height", 20);
ParameterDouble volume = new ParameterDouble("volume", 22);
 
ParameterCollection parameterCollection = new ParameterCollection(height, volume);
 
ExpressionFabric expressionFabric = ExpressionFabric.Create(functionProvider, parameterCollection);
 
String expr1 = "2147483600+1000";
String expr2 = "myRoom.Spatial.Area + Sqrt(myRoom.Spatial.Perimeter) * (-1)";
 
string expr3 = "-Max(List(1,2,3,4,5,6, Max(List(10,22)) ))";
string expr4= "Max(List(1,2,3)) + Sqrt(1)";
IExprModel model1 = expressionFabric.CreateModel(expr1);
IExprModel model2 = expressionFabric.CreateModel(expr2);
IExprModel model3 = expressionFabric.CreateModel(expr3);
IExprModel model4 = expressionFabric.CreateModel(expr4);
```

<h2>6. ExpressionComponentTree</h2>

ExpressionComponentTree offers the functionality to create a tree of dependent components starting from a string expression.
All classes derive from ExprComponent.

The following expression: “1+b+max(c)” is split into the following components:
Level1:
-> ExprNumericComponent : 1
-> ExprOperatorComponent : +
-> ExprParameterComponent : b
-> ExprOperatorComponent : +
-> ExprFunctionComponent : max / c

Level 2 :
The function component contains an ExprComplexComponent with following list of components:
-> -> ExprParameterComponent : c

<h3>6.1 ExprComponent</h3>

**Syntax**


```
public abstract class ExprComponent
{
    public string Context { get; protected set; }
    //public string EncodedContext { get; private set; }
 
    public ExprOptions ExprOptions { get; set; }
 
    public ExprComponent(string ExpressionString, ExprOptions exprOptions)
    {
        this.ExprOptions = exprOptions;
 
        this.Context = ExpressionString;
    }
}
```

<h3>6.2 ExprComplexComponent</h3>
ExprComplexComponent stores a collection of Children components that will be further evaluated.
The general rule is that the Children components must be of the following pattern : NonOperatorComponent / ExprOperatorComponent / NonOperatorComponent/..

**Syntax**

`public class ExprComplexComponent : ExprComponent`

**Properties**

`public List<ExprComponent> Children { get; private set; }`

**Create**


```
public static ExprComponent Create(string context,
                                    ExprOptions exprOptions,
                                    FunctionProvider functionProvider,
                                    ParameterCollection parameterCollection)
```

<h3>6.3 ExprFunctionComponent</h3>

