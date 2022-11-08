using System.Linq.Expressions;

namespace CalculationServiceLib;

public class ExpressionHandler
{
    private readonly Dictionary<string, IExpressionFactory> _dictionary = new Dictionary<string, IExpressionFactory>()
    {
        {"+", new AddExpressionFactory()},
        {"-", new SubtractExpressionFactory()},
        {"*", new MultiplyExpressionFactory()},
        {"/", new DivideExpressionFactory()},
    };

    public void Handle(List<Expression> stack, string token)
    {
        IExpressionFactory factory = _dictionary[token];
        if (factory != null)
        {
            factory.Create(stack);
        }
    }

    interface IExpressionFactory
    {
        void Create(List<Expression> stack);
    }

    private class AddExpressionFactory : IExpressionFactory
    {
        public void Create(List<Expression> stack)
        {
            Expression right = stack[^1];
            stack.RemoveAt(stack.Count - 1);
            Expression left = stack[^1];
            stack.RemoveAt(stack.Count - 1);
            stack.Add(Expression.Add(left, right));
        }
    }

    private class MultiplyExpressionFactory : IExpressionFactory
    {
        public void Create(List<Expression> stack)
        {
            Expression right = stack[^1];
            stack.RemoveAt(stack.Count - 1);
            Expression left = stack[^1];
            stack.RemoveAt(stack.Count - 1);
            stack.Add(Expression.Multiply(left, right));
        }
    }

    private class SubtractExpressionFactory : IExpressionFactory
    {
        public void Create(List<Expression> stack)
        {
            Expression right = stack[^1];
            stack.RemoveAt(stack.Count - 1);
            Expression left = stack[^1];
            stack.RemoveAt(stack.Count - 1);
            stack.Add(Expression.Subtract(left, right));
        }
    }

    private class DivideExpressionFactory : IExpressionFactory
    {
        public void Create(List<Expression> stack)
        {
            Expression right = stack[^1];
            stack.RemoveAt(stack.Count - 1);
            Expression left = stack[^1];
            stack.RemoveAt(stack.Count - 1);
            Expression<Func<double>> temp = Expression.Lambda<Func<double>>(right);
            Func<double> res = temp.Compile();
            double result = res();
            if (result != 0)
                stack.Add(Expression.Divide(left, right));
            else
                throw new Exception("right cannot be zero");
        }
    }
}