using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;




public class Clients
{
    public string ConnectionId { get; set; }
    public string Name { get; set; }
}



namespace SignalRChat.Hubs
{

    public class ChatHub : Hub
    {
        public static int whoseturn = 0;
        public static int integer = 0;
        public static readonly List<Clients> ClientList = new List<Clients>();


        //////////////

        public ChatHub()
        {
           
        }

        /////////


    public async Task SendMessage(string user, string message)
        {
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", user, message);

        }

        public async Task Register(string user, string message)
        {
            Clients A_Client = new Clients();
            A_Client.ConnectionId = Context.ConnectionId;
            A_Client.Name = "a";
            ClientList.Add(A_Client);


            integer++;
            
                //await Clients.All.SendAsync("ReceiveMessage", user, message);
                await Clients.Client(Context.ConnectionId).SendAsync("IsRegister",user, message);
            if (integer == 2)
            {

                //await Clients.All.SendAsync("Printnames");

                await Clients.Client(ClientList[0].ConnectionId).SendAsync("Printnames1");
                await Clients.Client(ClientList[1].ConnectionId).SendAsync("Printnames2");

            }

        }


    }
}
