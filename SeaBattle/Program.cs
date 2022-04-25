using System;

namespace SeaBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            Lobby lobby = new Lobby();
            lobby.OpenLobby();

            Game game = new Game();
            game.StartNewRound(lobby.GameType);
            lobby.EndRound(); 

            //for later
            //do
            //{
            //    lobby.OpenLobby();
            //    Game game = new Game();
            //    game.StartNewRound(lobby.GameType);

            //    //lobby.EndRound();
            //} while (!lobby.IsEndGame);
            Console.ReadKey();
        }
    }
}
