using Microsoft.AspNetCore.Mvc;
using Tp08.Models;

namespace Tp08.Controllers;

public class JuegoController : Controller
{
    private string GetSessionKey(string username) => $"Juego_{username}";
    private const string SessionUsernameKey = "Username";

    private Juego GetJuego(string username)
    {
        if (string.IsNullOrEmpty(username))
            return new Juego();

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
        if (string.IsNullOrEmpty(username)) return;
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
        if (string.IsNullOrWhiteSpace(username))
        {
            TempData["Error"] = "Debe indicar un nombre de usuario.";
            return RedirectToAction("ConfigurarJuego");
        }

        HttpContext.Session.SetString(SessionUsernameKey, username);

        var juego = GetJuego(username);
        juego.CargarPartida(username, dificultad, categoria);
        SaveJuego(username, juego);

        return RedirectToAction("Jugar");
    }

    public IActionResult Jugar()
    {
        string? username = HttpContext.Session.GetString(SessionUsernameKey);
        if (string.IsNullOrEmpty(username))
        {

            TempData["Error"] = "No hay una partida iniciada. Por favor configure el juego.";
            return RedirectToAction("ConfigurarJuego");
        }

        var juego = GetJuego(username);

        var pregunta = juego.ObtenerProximaPregunta();

        if (pregunta == null)
        {
            ViewBag.Username = username;
            ViewBag.Puntaje = juego.GetPuntaje();
            return View("Fin");
        }

        ViewBag.Username = username;
        ViewBag.Puntaje = juego.GetPuntaje();
        ViewBag.NumPregunta = juego.GetNumeroPregunta();
        ViewBag.Pregunta = pregunta;
        ViewBag.Respuestas = juego.ObtenerProximasRespuestas(pregunta.IdPregunta);

        SaveJuego(username, juego);

        return View("Juego");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult VerificarRespuesta(int idPregunta, int idRespuesta)
    {
        string? username = HttpContext.Session.GetString(SessionUsernameKey);
        if (string.IsNullOrEmpty(username))
        {
            TempData["Error"] = "Partida no encontrada. Volvé a configurar el juego.";
            return RedirectToAction("ConfigurarJuego");
        }

        var juego = GetJuego(username);
        bool esCorrecta = juego.VerificarRespuesta(idRespuesta);

        var respuestas = juego.ObtenerProximasRespuestas(idPregunta);
        var respuestaCorrecta = respuestas.FirstOrDefault(r => r.Correcta);
        ViewBag.EsCorrecta = esCorrecta;
        ViewBag.RespuestaCorrecta = respuestaCorrecta;
        ViewBag.Respuestas = respuestas;
        ViewBag.Username = username;
        ViewBag.Puntaje = juego.GetPuntaje();
        ViewBag.NumPregunta = juego.GetNumeroPregunta();

        SaveJuego(username, juego);

        return View("Respuesta");
    }
}