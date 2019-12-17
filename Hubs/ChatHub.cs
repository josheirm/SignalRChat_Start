using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;



using System;
using System.Web;
//using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;


//public class Clients
//{
//    public string ConnectionId { get; set; }
//    public string Name { get; set; }
//}

//public class ClientsList
//{
//    public static List<Clients> ClientList = new List<Clients>();
//}


namespace SignalRChat.Hubs
{
   
    public class ChatHub : Hub 
    {
        //////////////

        

        public static int whoseturn = 0;
        public static int integer = 0;

       

        //////////////

        public async Task SendMessage(string user, string message)
        {
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", user, message);

        }

        public async Task Register(string user, string message)
        {
            //Clients A_Client = new Clients();
            //A_Client.ConnectionId = Context.ConnectionId;
            //A_Client.Name = "a";

            integer++;
            if (integer == 2)
            {

                //await Clients.All.SendAsync("ReceiveMessage", user, message);
                await Clients.Client(Context.ConnectionId).SendAsync("IsRegister",user, message);
            }
        }


    }
}
