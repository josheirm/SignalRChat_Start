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
    public String Groupname { get; set; }
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
        public String groupname = "A";
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

        public void GetGroups()
        {
            integer++;
            groupname = "group" + integer;
        }





        public override Task OnDisconnectedAsync(Exception e)
        {
            int flag = 0;
            //List<EachGame> Games = new List<EachGame>();
            ///ClientList CList2 = new ClientList;
            //Clients A_Client = new Clients();
            //A_Client.ConnectionId = Context.ConnectionId;
            //A_Client.Name = "a";


            string Cone1 = "";
            string Cone2 = "";
            string Group = "";
            int index = 0;
            for (int i = 0; i < Games.Count; i++)
            {
                if (Games[i].Playeroneconn == Context.ConnectionId)
                {
                    Cone1 = Games[i].Playeroneconn;
                    Cone2 = Games[i].Playertwoconn;
                    Group = Games[i].Groupname;
                    Groups.RemoveFromGroupAsync(Cone1, Group);
                    index = i;
                    flag = 1;
                    break;
                }
                if (Games[i].Playertwoconn == Context.ConnectionId)
                {
                    Cone1 = Games[i].Playeroneconn;
                    Cone2 = Games[i].Playertwoconn;
                    Group = Games[i].Groupname;
                    Groups.RemoveFromGroupAsync(Cone2, Group);
                    index = i;
                    flag = 1;
                    break;
                }
                
                //ClientList2[i].ConnectionId = ClientList[i].ConnectionId;
            }
            //is Context.ConnectionId is this the disconeected client?
            //Who is remaining client? 

            //perhaps check which player is remaining and use
            //a list with playeroneconnid and playertwoconnid
            //and their group, Would a list work in this function
            //global variables don't!

            //however when the program gets large it starts to act
            //strangely, and removing any broadcast can seem to solve the 
            //problem.


            //Groups.RemoveFromGroupAsync(PlayerTwoConnId, groupname);
            //Clients.Client(Context.ConnectionId).SendAsync("Printnames0");

            //groupname doesn't retain value
            //blanks out diplay

            if (flag == 1)
            {
                Clients.Group(Group).SendAsync("Printnames0");
                //no longer a pair so needs a new register button
                Clients.Group(Group).SendAsync("IsWaiting");

                Games.RemoveAt(index);
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
            //doesn't work
            //if (firstregister == "1")
            //{
            //    groupname = "group" + integer;
            //    //integer++;
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
                        PlayerOneConnId = Context .ConnectionId;

                    GetGroups();
                    EachGame Game1 = new EachGame();
                    Game1.Playeroneconn = PlayerOneConnId;
                    Game1.Playertwoconn = PlayerTwoConnId;
                    Game1.Groupname = groupname;
                    Games.Add(Game1);

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
