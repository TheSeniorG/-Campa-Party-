using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

public class DatabaseAccess : MonoBehaviour
{

    readonly MongoClient client = new MongoClient("mongodb+srv://rpadilladam:ra329656XD@cluster0.7eznjbv.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;

    private ObjectId id_game;
    private ObjectId id_minigame;

    void Start()
    {
        database = client.GetDatabase("CampaParty");
        collection = database.GetCollection<BsonDocument>("Partida");
    }

    //INICIO DE PARTIDA
    public void SetStartGame()
    {
        /*
        // INSERTAR UN NUEVO DOCUMENTO PARA LA PARTIDA
        var document = new BsonDocument
        {
            { "clasificacion", new BsonDocument() },
            { "fecha_inicio", DateTime.Now },
            { "fecha_final", "---" },
            { "minijuegos", new BsonArray() }
        };
        collection.InsertOne(document);

        // GUARDAR EL ID DE LA PARTIDA
        id_game = document["_id"].AsObjectId;
        */
    }

    //A�ADIR JUGADOR A LA PARTIDA
    public void AddPlayer(string playerName)
    {
        /*
        // CREAR LA INFORMACI�N DEL JUGADOR
        var player = new BsonDocument
        {
            { "nombre", playerName },
            { "puntuacion", 0 }
        };

        // FILTRO PARA A�ADIR EL JUGADOR A LA PARTIDA
        var filter = Builders<BsonDocument>.Filter.Eq("_id", id_game);

        // ACTUALIZAR LA CLASIFICACI�N CON EL NUEVO JUGADOR
        var update = Builders<BsonDocument>.Update.Push("clasificacion.jugador", player);
        collection.UpdateOne(filter, update);
        */
    }

    // A�ADIR INICIO DE UN MINIJUEGO
    public void SetMinigameStart(string minigameName)
    {
        /*
        // GENERAR UNA ID �NICA PARA EL MINIJUEGO
        id_minigame = ObjectId.GenerateNewId();

        // FILTRAR POR LA PARTIDA
        var filter = Builders<BsonDocument>.Filter.Eq("_id", id_game);

        // CREAR UN DOCUMENTO PARA EL NUEVO MINIJUEGO
        var minijuego = new BsonDocument{
                { "id_minijuego", id_minigame },
                { "nombre", minigameName },
                { "fecha_inicio", DateTime.Now },
                { "fecha_final", "---" },
                { "clasificacion", new BsonDocument() } // AGREGAR LA CLASIFICACI�N DE LA PARTIDA
        };

        // A�ADIR EL MINIJUEGO A LA PARTIDA
        var update = Builders<BsonDocument>.Update.Push("minijuegos", minijuego);
        collection.UpdateOne(filter, update);
        */
    }

    //METODO PARA A�ADIR EL FINAL DE UN MINIJUEGO I ACTUALIZAR LAS PUNTUACIONES TOTALES
    public void SetMiniGameEnd(Dictionary<int, int> scores)
    {
        /*
        // FILTRAR POR LA PARTIDA Y EL MINIJUEGO ACTUAL
        var minigameFilter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("_id", id_game),
            Builders<BsonDocument>.Filter.Eq("minijuegos.id_minijuego", id_minigame));

        // ACTUALIZAR LA FECHA FINAL DEL MINIJUEGO
        var dateUpdate = Builders<BsonDocument>.Update.Set("minijuegos.$.fecha_final", DateTime.UtcNow);
        collection.UpdateOne(minigameFilter, dateUpdate);

        // CREAR LA NUEVA CLASIFICACI�N DE JUGADORES CON SUS PUNTUACIONES
        foreach (var kvp in scores)
        {
            // CREAR EL DOCUMENTO DE JUGADOR CON EL NOMBRE BASADO EN EL ORDEN Y LA PUNTUACI�N ACTUALIZADA
            var jugador = new BsonDocument
            {
                { "nombre", "PLAYER " + kvp.Key },
                { "puntuacion", kvp.Value }
            };

            // ACTUALIZAR LA CLASIFICACI�N DEL MINIJUEGO CON LA NUEVA CLASIFICACI�N
            var playMinigameScore = Builders<BsonDocument>.Update.Push("minijuegos.$.clasificacion.jugador", jugador);
            collection.UpdateOne(minigameFilter, playMinigameScore);

            // CREAR UNA ACTUALIZACI�N UTILIZANDO EL OPERADOR $INC Y LA RUTA AL CAMPO ESPEC�FICO FUERA DEL ARRAY MINIJUEGOS
            var update = Builders<BsonDocument>.Update.Inc("clasificacion.jugador.$[jugador].puntuacion", kvp.Value);

            // DEFINIR LAS OPCIONES DE ACTUALIZACI�N CON EL ARRAY DE FILTROS DE POSICI�N
            var options = new UpdateOptions { ArrayFilters = new List<ArrayFilterDefinition> { new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("jugador.nombre", "PLAYER " + kvp.Key)) } };

            // EJECUTAR LA ACTUALIZACI�N
            collection.UpdateOne(minigameFilter, update, options);
        }
        */
    }

    //AGREGAR FECHA DE FINALIZACON DE LA PARTIDA
    public void SetEndGame()
    {
        /*
        // FILTRAR LA PARTIDA POR ID
        var filter = Builders<BsonDocument>.Filter.Eq("_id", id_game);

        // CREAR UN DOCUMENTO DE ACTUALIZACI�N PARA ACTUALIZAR LA FECHA FINAL DE LA PARTIDA
        var update = Builders<BsonDocument>.Update.Set("fecha_final", DateTime.Now);

        // ACTUALIZAR LA COLECCI�N PARA ESTABLECER LA FECHA FINAL DE LA PARTIDA
        collection.UpdateOne(filter, update);
    */
    }
}