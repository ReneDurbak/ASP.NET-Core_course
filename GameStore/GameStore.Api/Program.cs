using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
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


// GET /games
app.MapGet("games", () => games);

// GET /games/1
app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id))
    .WithName(GetGameEndpointName);



app.Run();
