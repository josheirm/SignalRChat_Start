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
        public string groupname = "";
        public int amtplayers = 0;
        public string PlayerOneConnId = "10";
        public string PlayerTwoConnId = "10";
        public int buttonAnswer = 1;
        public static int firststart = 1;
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
            
                
            
                //Clients.Group("group1",PlayerOneConnId).Printnames0;
                //Clients.Client(PlayerOneConnId).SendAsync("IsWaiting");
                //Clients.User(PlayerTwoConnId).SendAsync("Printnames0");
                //Clients.User(PlayerTwoConnId).SendAsync("IsWaiting");



            //has been a delete
            //string[] lines = { "1" };
            //System.IO.File.WriteAllLines(@"C:\Users\Public\WriteText.txt", lines);
            ////string[] UserLeft = { "1" };

            ////which player was deleted
            //if (PlayerOneConnId == Context.ConnectionId)
            //{
            //    string[] UserLeft = { "1" };
            //    System.IO.File.WriteAllLines(@"C:\Users\Public\UserLeft.txt", lines);


            //}


            //else
            //{
            //    string[] UserLeft = { "2" };
            //    System.IO.File.WriteAllLines(@"C:\Users\Public\UserLeft.txt", lines);

            //}



            return base.OnDisconnectedAsync(e);
    }

        

        public async Task SendMessage(string user, string message)
        {
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", user, message);

        }



        //when function is run and one client is disconnected with X on tab, other client acts as though the other client is still their and prints : 2nd user not your turn

        //why is Printnames2() executing like there are two clients and one client has been disconnected
        public async Task Register()
        {
            //string groupname = "group" + integer;

            //amtplayers++;
            //string text = System.IO.File.ReadAllText(@"C:\Users\Public\WriteText.txt");
            ////reset so delete took place already and if set back to one is a delete
            //string[] lines = { "0" };
            //System.IO.File.WriteAllLines(@"C:\Users\Public\WriteText.txt", lines);


            ////is a deleted
            //if (text == "1\r\n")
            //{
            //    //wihch user deleted
            //    string text2 = System.IO.File.ReadAllText(@"C:\Users\Public\UserLeft.txt");
            //    string[] lines2 = { "0" };
            //    System.IO.File.WriteAllLines(@"C:\Users\Public\UserLeft.txt", lines2);
            //    string remainingplayer = "0";
            //    string holder = "A";
            //    //find client to delete
            //    if (text2 == "1\r\n")
            //    {
            //        //to delete
            //        holder = PlayerOneConnId;
            //        remainingplayer = PlayerTwoConnId;
            //        //Clients.Caller.SendAsync("IsWaiting");
            //        await Clients.Group("group1").SendAsync("Printnames0");

            //    }
            //    // file holds 2
            //    else
            //    {   //to delete
            //        holder = PlayerTwoConnId;
            //        remainingplayer = PlayerOneConnId;
            //        //Clients.Caller.SendAsync("IsWaiting");
            //        await Clients.Group("group1").SendAsync("Printnames0");

            //    }
            //    //removes correct client
            //    for (int i = ClientList.Count - 1; i >= 0; --i)
            //    {
            //        if (ClientList[i].ConnectionId == holder)
            //        {
            //            ClientList.RemoveAt(i);
            //            await Groups.RemoveFromGroupAsync(holder, "group1");
            //            break;
            //        }
            //    }
            //    //clear text of remaining client
            //    //Clients.Group("group1").SendAsync("Printnames0");
            //    ///remaining player, above removed disconnected
            //    //Clients.Group("group1").SendAsync("Printnames0");
            //    //return (1);
            //}
            //else
            {//end is deleted 

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
                        await Groups.AddToGroupAsync(PlayerTwoConnId, "group1");
                        await Groups.AddToGroupAsync(PlayerOneConnId, "group1");

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
                //return (1);
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
