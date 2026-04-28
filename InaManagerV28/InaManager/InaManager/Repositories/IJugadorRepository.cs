using System;
using System.Collections.Generic;
using System.Net;
using InaManager.Models;

namespace InaManager.Repositories
{
    public interface IJugadorRepository
    {
        // --- MÉTODOS DE AUTENTICACIÓN Y BÁSICOS ---
        bool ValidateJugador(NetworkCredential credential);
        JugadorModel GetJugadorByUsername(string username);
        IEnumerable<JugadorModel> GetAll();
        void AddJugador(JugadorModel jugador); // Me aseguro de que esté en la interfaz también
        void UpdateJugador(JugadorModel jugador);

        // --- MÉTODOS DE SUPERTÉCNICAS ---
        List<SuperTecnicaModel> GetTecnicasDeJugador(int idJugador);

        // Obtiene TODAS las técnicas que existen en el juego (para el ComboBox de aprender)
        List<SuperTecnicaModel> GetCatalogoCompleto();

        // Guarda la relación en la base de datos (aprender)
        void AsignarTecnica(int idJugador, int idTecnica);

        // Borra la relación de la base de datos (olvidar)
        void RetirarTecnica(int idJugador, int idTecnica);

        // --- NUEVOS MÉTODOS PARA GESTIÓN DE PLANTILLA ---
        void IntercambiarJugadores(int idJugador1, int idJugador2);
        void ActualizarEstadoJugador(int idJugador, string nuevaPosicion, bool esTitular, bool estaConvocado);
        IEnumerable<JugadorModel> GetByTeam();

        void DeleteJugador(int idJugador);
    }
}