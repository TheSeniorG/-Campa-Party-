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

    //AÑADIR JUGADOR A LA PARTIDA
    public void AddPlayer(string playerName)
    {
        /*
        // CREAR LA INFORMACIÓN DEL JUGADOR
        var player = new BsonDocument
        {
            { "nombre", playerName },
            { "puntuacion", 0 }
        };

        // FILTRO PARA AÑADIR EL JUGADOR A LA PARTIDA
        var filter = Builders<BsonDocument>.Filter.Eq("_id", id_game);

        // ACTUALIZAR LA CLASIFICACIÓN CON EL NUEVO JUGADOR
        var update = Builders<BsonDocument>.Update.Push("clasificacion.jugador", player);
        collection.UpdateOne(filter, update);
        */
    }

    // AÑADIR INICIO DE UN MINIJUEGO
    public void SetMinigameStart(string minigameName)
    {
        /*
        // GENERAR UNA ID ÚNICA PARA EL MINIJUEGO
        id_minigame = ObjectId.GenerateNewId();

        // FILTRAR POR LA PARTIDA
        var filter = Builders<BsonDocument>.Filter.Eq("_id", id_game);

        // CREAR UN DOCUMENTO PARA EL NUEVO MINIJUEGO
        var minijuego = new BsonDocument{
                { "id_minijuego", id_minigame },
                { "nombre", minigameName },
                { "fecha_inicio", DateTime.Now },
                { "fecha_final", "---" },
                { "clasificacion", new BsonDocument() } // AGREGAR LA CLASIFICACIÓN DE LA PARTIDA
        };

        // AÑADIR EL MINIJUEGO A LA PARTIDA
        var update = Builders<BsonDocument>.Update.Push("minijuegos", minijuego);
        collection.UpdateOne(filter, update);
        */
    }

    //METODO PARA AÑADIR EL FINAL DE UN MINIJUEGO I ACTUALIZAR LAS PUNTUACIONES TOTALES
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

        // CREAR LA NUEVA CLASIFICACIÓN DE JUGADORES CON SUS PUNTUACIONES
        foreach (var kvp in scores)
        {
            // CREAR EL DOCUMENTO DE JUGADOR CON EL NOMBRE BASADO EN EL ORDEN Y LA PUNTUACIÓN ACTUALIZADA
            var jugador = new BsonDocument
            {
                { "nombre", "PLAYER " + kvp.Key },
                { "puntuacion", kvp.Value }
            };

            // ACTUALIZAR LA CLASIFICACIÓN DEL MINIJUEGO CON LA NUEVA CLASIFICACIÓN
            var playMinigameScore = Builders<BsonDocument>.Update.Push("minijuegos.$.clasificacion.jugador", jugador);
            collection.UpdateOne(minigameFilter, playMinigameScore);

            // CREAR UNA ACTUALIZACIÓN UTILIZANDO EL OPERADOR $INC Y LA RUTA AL CAMPO ESPECÍFICO FUERA DEL ARRAY MINIJUEGOS
            var update = Builders<BsonDocument>.Update.Inc("clasificacion.jugador.$[jugador].puntuacion", kvp.Value);

            // DEFINIR LAS OPCIONES DE ACTUALIZACIÓN CON EL ARRAY DE FILTROS DE POSICIÓN
            var options = new UpdateOptions { ArrayFilters = new List<ArrayFilterDefinition> { new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("jugador.nombre", "PLAYER " + kvp.Key)) } };

            // EJECUTAR LA ACTUALIZACIÓN
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

        // CREAR UN DOCUMENTO DE ACTUALIZACIÓN PARA ACTUALIZAR LA FECHA FINAL DE LA PARTIDA
        var update = Builders<BsonDocument>.Update.Set("fecha_final", DateTime.Now);

        // ACTUALIZAR LA COLECCIÓN PARA ESTABLECER LA FECHA FINAL DE LA PARTIDA
        collection.UpdateOne(filter, update);
    */
    }
}