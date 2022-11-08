using System.Diagnostics;
using System.Text.Encodings.Web;
using Calculator.Models;
using Microsoft.AspNetCore.Mvc;

namespace Calculator.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(string strInput)
    {
        string strOutput = ExpressionHandler(strInput);
        ViewData["strOutput"] = strOutput;
        return View();
    }

    private static string ExpressionHandler(string expression)
    {
        var result = "";
        try
        {
            var service = new CalculationServiceLib.CalculatorService();
            expression = expression.Replace(" ", "");
            result = service.CalculateExpression(expression);
            result = double.TryParse(result, out var res) ? res.ToString() : "Error occured";
        }
        catch
        {
            result = "Error occured";
        }
        return result;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}