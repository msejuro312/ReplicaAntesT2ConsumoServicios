using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class LibroController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string apiBase = "https://localhost:7229";
        public LibroController(HttpClient httpClient
            ) {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(apiBase);


        }

        async List<Libro> listLibros()
        { 
            var response = await _httpClient.GetAsync ("api/libro");
            var content = await response.Content.ReadAsStringAsync();
            var lista = JsonConvert.DeserializeObject<List<Libro>>(content);
        }
        
    }
}
