using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
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

        async Task<List<Libro>> listLibros()
        {
            var response = await _httpClient.GetAsync("api/Libro");
            var content = await response.Content.ReadAsStringAsync();
            var lista = JsonConvert.DeserializeObject<List<Libro>>(content) ?? new List<Libro>();
            return await Task.Run(() => lista);
        }

        async Task<Libro> getLibro(int LibroId)
        {
            var response = await _httpClient.GetAsync("api/Libro/" + LibroId);
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<Libro>>(content) ?? new ApiResponse<Libro>();
            var libro = apiResponse.data;
            return await Task.Run(() => libro);
        }

        async Task<List<Autor>> listAutores()
        {
            var response = await _httpClient.GetAsync("api/Autor");
            var content = await response.Content.ReadAsStringAsync();
            var lista = JsonConvert.DeserializeObject<List<Autor>>(content) ?? new List<Autor>();
            return await Task.Run(() => lista);
        }

        async Task<string> insertar(Libro libro)
        {
            var json = JsonConvert.SerializeObject(libro);
            var body = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await _httpClient.PostAsync("api/Libro", body);
            var response = await request.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<object>>(response) ?? new ApiResponse<object>();
            var message = apiResponse.message;
            return await Task.Run(() => message);
        }

        async Task<string> actualizar(Libro libro)
        {
            var json = JsonConvert.SerializeObject(libro);
            var body = new StringContent(json, Encoding.UTF8, "application/json");
            var request = await _httpClient.PutAsync("api/Libro", body);
            var response = await request.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<object>>(response) ?? new ApiResponse<object>();
            var message = apiResponse.message;
            return await Task.Run(() => message);
        }

        [HttpGet]

        public async Task<IActionResult> Index()
        {
            var lista = await listLibros();
            return View(lista);
        }

        [HttpGet]

        public async Task<IActionResult> Create()
        {
            var autores = await listAutores();
            ViewBag.autores = new SelectList(autores, "AutorId", "Nombre");
            return View(new Libro());

        }

        [HttpPost]

        public async Task<IActionResult> Create(Libro libro)
        {
            var message = await insertar(libro);
            TempData["message"] = message;
            return RedirectToAction("Index");
        }

        [HttpGet]

        public async Task<IActionResult> Edit(int LibroId)
        {
            var libro = await getLibro(LibroId);
            if (libro == null)
            {
                TempData["message"] = "El libro a editar no existe!";
                return RedirectToAction("Index");


            }
            else {
                var autores = await listAutores();
                ViewBag.autores = new SelectList(autores, "AutorId", "Nombre", libro.AutorId);
                return View(libro);

            }
        }

        [HttpPost]

        public async Task<IActionResult> Edit(Libro libro)
        { 
            var message = await actualizar(libro);
            TempData["message"] = message;
            return RedirectToAction("Index");
        }
            
            

        



    }
}
