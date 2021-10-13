using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Digitteck.ExpressionParser.ExprCommon.Binding
{
    public static class PropertyNav
    {
        /// <summary>
        /// Returns the last property name accessed as string. 
        /// Exp : X=>X.ABC.DEF ==> DEF
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetLastPropertyName<T>(Expression<Func<T, object>> expression)
        {
            if (expression.Body is MemberExpression)
            {
                var member = expression.Body as MemberExpression;
                if (member != null)
                {
                    return member.Member.Name;
                }
            }
            else if (expression.Body is UnaryExpression)
            {
                var member = expression.Body as UnaryExpression;
                if (member != null)
                {
                    return ((MemberExpression)member.Operand).Member.Name;
                }
            }
            return String.Empty;
        }

        public static string GetPropertyPath<T>(Expression<Func<T, object>> expression)
        {
            var unformatted = expression.Body.ToString();
            if (unformatted.Contains("("))
            {
                int startRemoveIndex = unformatted.IndexOf("(");
                int endRemoveIndex = unformatted.IndexOf(")");
                unformatted = unformatted.Substring(startRemoveIndex + 1, endRemoveIndex - startRemoveIndex - 1);
            }

            if (!unformatted.Contains("."))
                return null;
            //just extra
            var removeparts = unformatted.TakeWhile((ch) => ch != '}' && ch != ')' && ch != '{' && ch != '(').ToList();

            StringBuilder listvalues = new StringBuilder();
            //first is the object short name x in x=>x.hey
            //second is the point .
            for (var index = 2; index < removeparts.Count; index++)
                listvalues.Append(removeparts[index]);

            return listvalues.ToString();
        }
    }
}
