using System;

namespace SeaBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            Lobby lobby = new Lobby();

            lobby.OpenLobby();
            do
            {
                lobby.StartNewRound();
                lobby.EndRound();
            } while (!lobby.IsEndGame);
            lobby.WriteGameResult();
            Console.ReadKey();
        }
    }
}
