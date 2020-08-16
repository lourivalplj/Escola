using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Escola.Models;
using System.IO;
using System.Web;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

namespace Escola.Controllers
{
    public class AlunosController : Controller
    {
        private readonly EscolaContext _context;

        private readonly IConfiguration _configuration;

        public AlunosController(EscolaContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        public async Task<IActionResult> Index([FromQuery]string nome)
        {
            int id = 0;
            id = Convert.ToInt32(Request.RouteValues.Values.ElementAt(2));
            ViewBag.IdProfessor = id;
            ViewBag.NomeProfessor = nome;
            
            var alunos = await _context.Alunos.Where<Alunos>(p => p.idProfessor == id).ToListAsync();            
            return View(alunos);
        }

        public async Task<IActionResult> Importar()
        {
            string[] args = Request.RouteValues.Values.ElementAt(2).ToString().Split('?');

            var id = Convert.ToInt32(args[0]);
            var nome = args[1];

            if (Request.Form.Files.Count > 0)
            {

                DateTime ultimaAtualizacao;
                string data = _configuration.GetValue<string>("Bloqueio:LastUpdate");
                TimeSpan tempoBloqueio = TimeSpan.Parse(_configuration.GetValue<string>("Bloqueio:Time"));
                if (!String.IsNullOrEmpty(data))
                    ultimaAtualizacao = Convert.ToDateTime(data);
                else
                    ultimaAtualizacao = DateTime.Now;

               

                TimeSpan tempoDecorrido = DateTime.Now.Subtract(ultimaAtualizacao);

                if (tempoDecorrido < tempoBloqueio)
                {
                    ViewBag.Message = "Sistema bloqueado para importação";
                }
                else
                {
                    var file = Request.Form.Files[0];

                    if (file != null)
                    {
                        var result = new StringBuilder();
                        using (var reader = new StreamReader(file.OpenReadStream()))
                        {
                            while (reader.Peek() >= 0)
                            {
                                result.AppendLine(reader.ReadLine());

                                string[] dados = result.ToString().Split("||");

                                Alunos aluno = new Alunos();
                                aluno.Nome = dados[0];
                                aluno.ValorMensalidade = Convert.ToDecimal(dados[1]);
                                aluno.DataVencimento = Convert.ToDateTime(dados[2].Replace("\r\n", ""));
                                aluno.idProfessor = id;

                                _context.Add(aluno);
                                await _context.SaveChangesAsync();

                                result.Clear();
                            }
                        }

                    }
                    SetAppSettingValue("LastUpdate", DateTime.Now.ToString(), AppDomain.CurrentDomain.BaseDirectory);                    
                }               
            }
            else
            {
                ViewBag.Message = "Necessário selecionar um arquivo";
               
            }

            return RedirectToAction("Index", "Alunos", new { id = id, nome = nome });
            
        }
    
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var alunos =  _context.Alunos.Find(id);
            _context.Alunos.Remove(alunos);
            _context.SaveChanges();

            return NoContent();
        }

        public static void SetAppSettingValue(string key, string value, string appSettingsJsonFilePath = null)
        {
            if (appSettingsJsonFilePath == null)
            {
                appSettingsJsonFilePath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "appsettings.json");
            }
            else
            {
                appSettingsJsonFilePath = System.IO.Path.Combine(appSettingsJsonFilePath, "appsettings.json");
            }

            var json = System.IO.File.ReadAllText(appSettingsJsonFilePath);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);


            jsonObj["Bloqueio"][key] = value;

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

            System.IO.File.WriteAllText(appSettingsJsonFilePath, output);
        }
    }
}
