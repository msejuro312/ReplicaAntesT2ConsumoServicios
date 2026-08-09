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

        async Task<List<Libro>> listLibros()
        { 
            var response = await _httpClient.GetAsync ("api/Libro");
            var content = await response.Content.ReadAsStringAsync();
            var lista = JsonConvert.DeserializeObject<List<Libro>>(content) ?? new List<Libro>();
            return await Task.Run(()=>lista);
        }

        async Task<Libro> getLibro(int LibroId) 
        {
            var response = await _httpClient.GetAsync("api/Libro/"+LibroId);
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<Libro>>(content) ?? new ApiResponse<Libro>();
            var libro = apiResponse.data;
            return await Task.Run(()=>libro);
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
            var request = await _httpClient.PostAsJsonAsync("api/Libro",json);
            var response = await request.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<object>>(response) ?? new ApiResponse<object>();
            var message = apiResponse.message;
            return await Task.Run(() => message);
        }
        
    }
}
