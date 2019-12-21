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
    public String Playeroneconn { get; set; }
    public String Playertwoconn { get; set; }
}

public class Variables
{

    public string PlayerOneConnId = "a";

}





namespace SignalRChat.Hubs
{

    
    public class ChatHub : Hub
    {

        public string PlayerOneConnId = "10";
        public string PlayerTwoConnId = "10";
        public int buttonAnswer = 1;

        public static int whoseturn = 0;
        public static int integer = 0;
        public static readonly List<Clients> ClientList = new List<Clients>();
        //private static object _syncRoot = new object();
        public static readonly List<EachGame> Games = new List<EachGame>();
        //private static readonly Random random = new Random();



        public override Task OnConnectedAsync()
        {
            Clients.All.SendAsync("IsButton1", "no");
            return base.OnConnectedAsync();
        }

        



        public ChatHub()
        {
           

        }



        
        
        

    public override Task OnDisconnectedAsync(Exception e)
    {
        foreach (EachGame element in Games)
        {
            if (element.Playeroneconn == Context.ConnectionId)
            {
                //set page to none
                element.Playeroneconn = null;
                //both pages are now none
                if (element.Playertwoconn == null)
                {
                    Games.Remove(element);
                }
                else if (element.Playertwoconn != "10")
                {
                    //alter connection that stays 
                    PlayerTwoConnId = null;
                    //Clients.Client(element.Playertwoconn).SendAsync("Printnames0");
                    Clients.Client(element.Playertwoconn).SendAsync("IsWaiting");
                }
                break;
            }
            else if (element.Playertwoconn == Context.ConnectionId)
            {
                //set page to none
                element.Playertwoconn = null;
                //both pages are now none
                if (element.Playeroneconn == null)
                {
                    Games.Remove(element);
                }
                else if (element.Playeroneconn != "10")
                {
                    //alter connection that stays
                    PlayerOneConnId = null;
                    //Clients.Client(element.Playeroneconn).SendAsync("Printnames0");
                    Clients.Client(element.Playeroneconn).SendAsync("IsWaiting");
                }
                break;
            }
        }
        return base.OnDisconnectedAsync(e);
    }

        

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
            
        EachGame Egame = new EachGame();
        Egame.Playeroneconn = null;
        Egame.Playertwoconn = null;
        var breakout = 1;
           
        foreach (Clients element in ClientList)
        {
            //not the current user - open page, gets removed after used
            if (element.ConnectionId != Context.ConnectionId)
            {
                //adds to game list, probably not needed
                Egame.Playeroneconn = Context.ConnectionId;
                Egame.Playertwoconn = element.ConnectionId;
                Games.Add(Egame);

                //remove this in next for each loop
                PlayerOneConnId = Context.ConnectionId;
                PlayerTwoConnId = element.ConnectionId;
                //remove other player
                ClientList.Remove(element);
                //find the current player
                foreach (Clients element2 in ClientList)
                {
                    if (element2.ConnectionId == Context.ConnectionId)
                    {
                        //removes current player
                        ClientList.Remove(element2);
                        //assigns to connections to be named player one and player two
                        await Clients.Client(PlayerOneConnId).SendAsync("Printnames2");
                        await Clients.Client(PlayerTwoConnId).SendAsync("Printnames1");
                        //breaks out of all braces 
                        breakout = 2;
                        break;
                    }
                           
                }
                if (breakout == 2)
                { break; }
            }
        }
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
