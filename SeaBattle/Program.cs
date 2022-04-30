using System;

namespace SeaBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            Lobby lobby = new Lobby();
            lobby.OpenLobby();

            lobby.EndRound(); 

            //for later
            //do
            //{
            //    lobby.OpenLobby();
            //    lobby.EndRound();
            //} while (!lobby.IsEndGame);
            Console.ReadKey();
        }
    }
}
