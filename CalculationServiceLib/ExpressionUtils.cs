namespace CalculationServiceLib;

public static class ExpressionUtils
{
    public static string[] ParseExpression(string expression)
    {
        var result = new string[1];
        var j = -1;
        for (var i = 0; i < expression.Length; i++)
        {
            if (!char.IsNumber(expression[i]))
            {
                j++;
                Array.Resize(ref result, j + 1);
                result[j] = expression[i].ToString();
            }
            else
            {
                if (i == 0 || !char.IsNumber(expression[i - 1]))
                {
                    j++;
                    Array.Resize(ref result, j + 1);
                    result[j] += expression[i];
                }
                else
                {
                    result[j] += expression[i];
                }
            }
        }

        return result;
    }

    public static List<string> CreateRPN(string[] tokens)
    {
        var result = new List<string>();
        var stack = new List<string>();
        var priority = new List<int>();
        for (var i = 0; i < tokens.Count(); i++)
        {
            double tempIntVar;
            if (Double.TryParse(tokens[i], out tempIntVar))
            {
                result.Add(tokens[i]);
            }
            else
            {
                switch (tokens[i])
                {
                    case "*":
                    case "/":
                    {
                        while ((priority.Count != 0) && (priority[^1] >= 2))
                        {
                            result.Add(stack[^1]);
                            stack.RemoveAt(stack.Count - 1);
                            priority.RemoveAt(priority.Count - 1);
                        }

                        stack.Add(tokens[i]);
                        priority.Add(2);
                        break;
                    }
                    case "+":
                    case "-":
                    {
                        while ((priority.Count != 0) && (priority[^1] >= 1))
                        {
                            result.Add(stack[^1]);
                            stack.RemoveAt(stack.Count - 1);
                            priority.RemoveAt(priority.Count - 1);
                        }

                        stack.Add(tokens[i]);
                        priority.Add(1);
                        break;
                    }
                    case "(":
                    {
                        stack.Add(tokens[i]);
                        priority.Add(0);
                        break;
                    }
                    case ")":
                    {
                        while (stack[^1] != "(")
                        {
                            result.Add(stack[^1]);
                            stack.RemoveAt(stack.Count - 1);
                            priority.RemoveAt(priority.Count - 1);
                        }

                        stack.RemoveAt(stack.Count - 1);
                        priority.RemoveAt(priority.Count - 1);
                        break;
                    }
                }
            }
        }

        while (stack.Count != 0)
        {
            result.Add(stack[^1]);
            stack.RemoveAt(stack.Count - 1);
            priority.RemoveAt(priority.Count - 1);
        }

        return result;
    }
}