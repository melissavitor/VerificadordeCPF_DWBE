using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VerificadordeCPF.Models;
using VerificadordeCPF.Services;

namespace VerificadordeCPF.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CpfBrasilApiClient _apiClient;

    public HomeController(ILogger<HomeController> logger, CpfBrasilApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public IActionResult Index()
    {
        return View(new CpfInputModel());
    }

    [HttpPost]
    public IActionResult ValidarBack([FromBody] CpfInputModel modelo)
    {
        var valido = VerificadorCpf.Validar(modelo.Cpf);

        return Json(new
        {
            valido,
            mensagem = valido ? "cpf correto no back" : "cpf incorreto no back"
        });
    }

    [HttpPost]
    public async Task<IActionResult> ValidarWeb([FromBody] CpfInputModel modelo)
    {
        var cpfLimpo = new string((modelo.Cpf ?? string.Empty).Where(char.IsDigit).ToArray());

        if (cpfLimpo.Length != 11)
        {
            return Json(new
            {
                valido = false,
                mensagem = "cpf incorreto na web",
                detalhe = "O CPF precisa ter 11 dígitos."
            });
        }

        var resultado = await _apiClient.ConsultarAsync(cpfLimpo);

        if (resultado.Erro is not null)
        {
            return Json(new
            {
                valido = false,
                mensagem = "cpf incorreto na web",
                detalhe = resultado.Erro
            });
        }

        return Json(new
        {
            valido = resultado.Sucesso,
            mensagem = resultado.Sucesso ? "cpf correto na web" : "cpf incorreto na web"
        });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
