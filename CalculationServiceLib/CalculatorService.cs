using System.Linq.Expressions;

namespace CalculationServiceLib;

public class CalculatorService
{
    public string CalculateExpression(string expression)
    {
        string[] tokens = ExpressionUtils.ParseExpression(expression);
        List<string> rpn = ExpressionUtils.CreateRpn(tokens);
        Expression<Func<double>> lambda = Expression.Lambda<Func<double>>(GetExpressionTree(rpn));
        Func<double> myDelegate = lambda.Compile();
        double result = myDelegate();
        return result.ToString();
    }

    private Expression GetExpressionTree(List<string> rpn)
    {
        ExpressionHandler expressionHandler = new ExpressionHandler();
        List<Expression> expressionStack = new List<Expression>();
        foreach (string token in rpn)
        {
            double tempIntVar;
            if (Double.TryParse(token, out tempIntVar))
            {
                expressionStack.Add(Expression.Constant(tempIntVar));
            }
            else
            {
                expressionHandler.Handle(expressionStack, token);
            }
        }

        return expressionStack[0];
    }
}