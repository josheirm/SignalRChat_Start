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
        public static int integer = 0;
        private String firstregister = "1";
        public String groupname = "";
        public int amtplayers = 0;
        public String PlayerOneConnId = "10";
        public String PlayerTwoConnId = "10";
        public int buttonAnswer = 1;
        public static int firststart = 1;
        public static int whoseturn = 0;
        
        public static readonly List<Clients> ClientList = new List<Clients>();
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

            var groupname1 = "";
            for (int a = 0; a < integer; a++)
            {
                groupname1 = "group" + a;


                Clients.Group(groupname1).SendAsync("Printnames0");
                Clients.Group(groupname1).SendAsync("IsWaiting");
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
            //if (firstregister == "1")
            //{
                groupname = "group" + integer;
            //    integer++;
            //    firstregister = "2";
            //}
            
            
            

                Clients A_Client = new Clients();
                A_Client.ConnectionId = Context.ConnectionId;
                //A_Client.Name = "a";
                ClientList.Add(A_Client);
                //hides button
                //asynch
                await Clients.Caller.SendAsync("IsRegister");
                var flag1 = 0;
                //foreach (Clients element in ClientList)
                for (int k = ClientList.Count - 1; k >= 0; --k)
                {
                    //PlayerOneConnId = "";
                    //PlayerTwoConnId = "";

                    //not the current user - open page, gets removed after used
                    if ((ClientList[k].ConnectionId) != (Context.ConnectionId))
                    {
                        PlayerTwoConnId = ClientList[k].ConnectionId;
                        PlayerOneConnId = Context.ConnectionId;

                        //await2:
                        await Groups.AddToGroupAsync(PlayerTwoConnId, groupname);
                        await Groups.AddToGroupAsync(PlayerOneConnId, groupname);
                        integer++;
                        //firstregister = 1;

                        //remove other player
                        ClientList.RemoveAt(k);
                        //remove current player flag
                        flag1 = 1;
                        break;
                    }
                }
                //there was another player so remove other
                if (flag1 == 1)
                {
                    flag1 = 0;

                    for (int j = ClientList.Count - 1; j >= 0; --j)
                    {

                        if ((ClientList[j].ConnectionId) == (Context.ConnectionId))
                        {
                            //removes current player
                            ClientList.RemoveAt(j);
                            //prints names

                            await Clients.Client(PlayerOneConnId).SendAsync("Printnames1");
                            await Clients.Client(PlayerTwoConnId).SendAsync("Printnames2");

                            break;
                        }

                    }
                }
                
            
        }

       
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
           
    }


}
