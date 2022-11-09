namespace CalculationServiceLib;

public static class ExpressionUtils
{
    public static string[] ParseExpression(string expression)
    {
        var result = new string[1];
        var j = -1;
        var i = 0;
        while (i < expression.Length)
        {

            if (!double.TryParse(expression[i].ToString(), out _))
            {
                j++;
                Array.Resize(ref result, j + 1);
                result[j] = expression[i].ToString();
                i++;
            }
            else
            {
                if (i == 0 || !double.TryParse(expression[i - 1].ToString(), out _))
                {
                    j++;
                    Array.Resize(ref result, j + 1);
                    if (i == expression.Length - 1)
                    {
                        result[j] += expression[i].ToString();
                        i++;
                    }
                    else if (expression[i + 1] == '.' || char.IsNumber(expression[i + 1]) || char.IsNumber(expression[i]))
                    {
                        var k = i;
                        var temp = "";
                        while (expression[k] != '+' && expression[k] != '-' && expression[k] != '*' &&
                               expression[k] != '/' && expression[k] != '(' && expression[k] != ')' &&
                               expression[k] != '=')
                        {
                            temp += expression[k];
                            k++;
                            if (k == expression.Length)
                            {
                                break;
                            }
                        }

                        i = k;
                        result[j] += temp;
                    }
                }
                else
                {
                    result[j] += expression[i];
                }
            }
        }

        return result;
    }

    public static List<string> CreateRpn(string[] tokens)
    {
        var result = new List<string>();
        var stack = new List<string>();
        var priority = new List<int>();
        for (var i = 0; i < tokens.Count(); i++)
        {
            if (Double.TryParse(tokens[i], out _))
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