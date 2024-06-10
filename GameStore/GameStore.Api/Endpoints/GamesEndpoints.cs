using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games = [
    new (
        1,
        "GTA 5",
        "Action",
        25.5M,
        new DateOnly(2013,7,15)),
    new (
        2,
        "Mafia 2",
        "Action",
        20M,
        new DateOnly(2010,7,15)),
    new (
        3,
        "CS:GO",
        "Action",
        0M,
        new DateOnly(1992,7,15)),
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app){

        var group = app.MapGroup("games").WithParameterValidation();

        // GET /games
        group.MapGet("/", () => games);

        // GET /games/1
        group.MapGet("/{id}", (int id) => {
            GameDto? game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) => {
            Game game = newGame.ToEntity();
            game.Genre = dbContext.Genres.Find(newGame.GenreId);


            dbContext.Games.Add(game);
            dbContext.SaveChanges();


            return Results.CreatedAtRoute(
                GetGameEndpointName, 
                new {id = game.Id}, 
                game.ToDto()
            );
        });

        // PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) => {

            var index = games.FindIndex(game => game.Id == id);

            if(index == -1){
            return Results.NotFound();
            }

            games[index] = new GameDto(
            id,
            updatedGame.Name,
            updatedGame.Genre,
            updatedGame.Price,
            updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });

        // DELETE /games/1
        group.MapDelete("/{id}", (int id) => {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });
        
        return group;
    }
}
