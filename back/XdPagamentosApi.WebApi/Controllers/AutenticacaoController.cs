using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XdPagamentosApi.WebApi.Controllers
{
    public class AutenticacaoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
