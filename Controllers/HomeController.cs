using Microsoft.AspNetCore.Mvc;
using Tp08.Models;

namespace Tp08.Controllers;

public class JuegoController : Controller
{
    private string GetSessionKey(string username)
    {
        return $"Juego_{username}";
    }

    private Juego GetJuego(string username)
    {
        string? data = HttpContext.Session.GetString(GetSessionKey(username));
        Juego? juego = Objeto.StringToObject<Juego>(data);

        if (juego == null)
        {
            juego = new Juego();
            SaveJuego(username, juego);
        }

        return juego;
    }

    private void SaveJuego(string username, Juego juego)
    {
        string data = Objeto.ObjectToString(juego);
        HttpContext.Session.SetString(GetSessionKey(username), data);
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ConfigurarJuego()
    {
        var juego = new Juego();
        ViewBag.Categorias = juego.ObtenerCategorias();
        ViewBag.Dificultades = juego.ObtenerDificultades();
        return View();
    }

    public IActionResult Comenzar(string username, int dificultad, int categoria)
    {
        var juego = GetJuego(username);
        juego.CargarPartida(username, dificultad, categoria);
        SaveJuego(username, juego);

        TempData["Username"] = username;

        return RedirectToAction("Jugar");
    }

    public IActionResult Jugar()
    {
        string username = TempData["Username"]?.ToString() ?? "Anonimo";

        var juego = GetJuego(username);
        SaveJuego(username, juego);

        var pregunta = juego.ObtenerProximaPregunta();

        if (pregunta == null)
        {
            return View("Fin");
        }

        ViewBag.Pregunta = pregunta;
        ViewBag.Respuestas = juego.ObtenerProximasRespuestas(pregunta.IdPregunta);
        ViewBag.Username = username;

        return View("Juego");
    }

    [HttpPost]
    public IActionResult VerificarRespuesta(string username, int idPregunta, int idRespuesta)
    {
        var juego = GetJuego(username);

        bool esCorrecta = juego.VerificarRespuesta(idRespuesta);

        ViewBag.EsCorrecta = esCorrecta;
        ViewBag.Respuestas = juego.ObtenerProximasRespuestas(idPregunta);
        ViewBag.RespuestaCorrecta = ViewBag.Respuestas.FirstOrDefault(r => r.correcta);
        ViewBag.Username = username;

        SaveJuego(username, juego);

        return View("Respuesta");
    }
}
