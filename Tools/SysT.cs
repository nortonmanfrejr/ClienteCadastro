using System.Linq.Expressions;
using System.Web.Mvc;

namespace ClienteCadastroApplication.Tools
{
    public static class SysT
    {
        public static string AppName = "Lá Fiesta";

        #region GENERALS

        public static string GenName(Expression expression, bool removeIndexes = false)
        {
            var path = string.Empty;

            if (!(expression is LambdaExpression expr)) return path;

            if (expr.Body.NodeType == ExpressionType.Convert
                || expr.Body.NodeType == ExpressionType.ConvertChecked
                ) expr = Expression.Lambda(((UnaryExpression)expr.Body).Operand, expr.Parameters);

            path = ExpressionHelper.GetExpressionText(expr);

            //------------------------------------------------------------------------------------------

            //if (removeIndexes == true) path = RemovePathParams(path);

            return path;
        }

        public static string GenName<TClass, TProp>(Expression<Func<TClass, TProp>> expression, bool removeIndexes = false) => GenName((Expression)expression, removeIndexes);
        public static string GenName<TClass, TProp>(TClass _, Expression<Func<TClass, TProp>> expression, bool removeIndexes = false) => GenName(expression, removeIndexes);
        public static string GenName<TClass>(Expression<Func<TClass, object>> expression, bool removeIndexes = false) => GenName<TClass, object>(expression, removeIndexes);
        public static string GenName<TClass>(TClass _, Expression<Func<TClass, object>> expression, bool removeIndexes = false) => GenName<TClass, object>(expression, removeIndexes);

        #endregion GENERALS
    }
}
