using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;



public class Clients
{
    public string ConnectionId { get; set; }
    public string Name { get; set; }
}

public class EachGame
{
    public Clients Playerone { get; set; }
    public Clients Playertwo { get; set; }
}




namespace SignalRChat.Hubs
{

    
    public class ChatHub : Hub
    {

        public override Task OnConnectedAsync()
        {
            //Clients.All.SendAsync("IsButton1", "no");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception e)
        {
            //Globals.SystemLogger.LogTrace($"Starting onDisconnectedAsync for user with connectionId {Context.ConnectionId}.");
            return base.OnDisconnectedAsync(e);
        }

        string PlayerOneConnId = null;
        string PlayerTwoConnId = null;
        int buttonAnswer = 1;

        public static int whoseturn = 0;
        public static int integer = 0;
        public static readonly List<Clients> ClientList = new List<Clients>();
        //private static object _syncRoot = new object();
        //private static readonly List<EachGame> games = new List<EachGame>();
        //private static readonly Random random = new Random();

        //////////////

        //public ChatHub()
        //{
        //set answer using random here
        //}


       


    public async Task SendMessage(string user, string message)
        {
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", user, message);

        }


        

        public async Task Register()
        {
           
                Clients A_Client = new Clients();
                A_Client.ConnectionId = Context.ConnectionId;
                A_Client.Name = "a";
                ClientList.Add(A_Client);
           



                //hides button
                //await Clients.All.SendAsync("ReceiveMessage", user, message);
                await Clients.Client(Context.ConnectionId).SendAsync("IsRegister");


            //EachGame Egame = new EachGame();
           // Egame.Playerone.ConnectionId = "null";
           // Egame.Playerone.Name = "a";
            var breakout = 1;
            
            
            
            while (true)
            {


                foreach (Clients element in ClientList)
                {
                    //not the current user
                    if (element.ConnectionId != Context.ConnectionId)
                    {
                        //remove this in next for each loop
                        PlayerOneConnId = Context.ConnectionId;
                        //found a player
                        //Egame.Playertwo.ConnectionId = element.ConnectionId;
                        PlayerTwoConnId = element.ConnectionId;

                        //adds to game list, probably not needed
                        //games.Add(Egame);
                        //remove other player
                        ClientList.Remove(element);

                        //find the current player
                        foreach (Clients element2 in ClientList)
                        {
                            if (element2.ConnectionId == Context.ConnectionId)
                            {
                                //removes current player
                                ClientList.Remove(element2);

                                

                                //propagates out of while loop
                                breakout = 2;
                               
                            }
                            break;
                        }
                      

                    }

                    if (breakout == 2)
                    { break; }
                }


                if (breakout == 2)
                { break; }
            
            }

            

            
            
               
                //assigns to connections to be named player one and player two
                await Clients.Client(PlayerOneConnId).SendAsync("Printnames2");
                await Clients.Client(PlayerTwoConnId).SendAsync("Printnames1");

           

        }

        ////////////
        public async Task B1()
        {
            if (buttonAnswer == 11)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("IsButton1", "yes");
                //await Clients.Client(PlayerTwoConnId).SendAsync("IsButton1", "yes");
            }
            else
            {
                await Clients.All.SendAsync("IsButton1", "no");
            }
        }
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
           

        

        ///////////

    }


}
