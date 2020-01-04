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
    public String Whosturn { get; set; }

    public int Answer { get; set; }

}

public class Variables
{
    
}





namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public String PlayerOneConnId = "10";
        public String PlayerTwoConnId = "10";
        private String whoseturn;
        //for group name
        public static int integer = 0;
        public String groupname = "A";
        public int amtplayers = 0;
        public int buttonAnswer = 5;
        public static readonly List<Clients> ClientList = new List<Clients>();
        public static readonly List<EachGame> Games = new List<EachGame>();
        public static readonly Random random = new Random();
        

        public void DisconnectGame()
        {
            int flag = 0;
            string Cone1 = "";
            string Cone2 = "";
            string Group = "";
            int index = 0;
            for (int i = 0; i < Games.Count; i++)
            {
                //find the game
                if (Games[i].Playeroneconn == Context.ConnectionId)
                {
                    Cone1 = Games[i].Playeroneconn;
                    Cone2 = Games[i].Playertwoconn;
                    Group = Games[i].Groupname;
                    
                    index = i;
                    flag = 1;
                    break;
                }
                if (Games[i].Playertwoconn == Context.ConnectionId)
                {
                    Cone1 = Games[i].Playeroneconn;
                    Cone2 = Games[i].Playertwoconn;
                    Group = Games[i].Groupname;
                    //Groups.RemoveFromGroupAsync(Cone2, Group);
                    index = i;
                    flag = 1;
                    break;
                }

            }


            if (flag == 1)
            {
                //Clients.Group(Group).SendAsync("Printnames0");
                //no longer a pair so needs a new register button
                Clients.Group(Group).SendAsync("IsWaiting");
                Groups.RemoveFromGroupAsync(Cone1, Group);
                Groups.RemoveFromGroupAsync(Cone2, Group);
                Games.RemoveAt(index);
            }


        }
        //IsButton1_1
        //IsButton1_2

        public void B1()
        {
            Buttonhandler("IsButton1_1", "IsButton1_2", 1);
        }

        //
        public void B2()
        {
            Buttonhandler("IsButton2_1", "IsButton2_2", 2);
        }

        public void B3()
        {
            Buttonhandler("IsButton3_1", "IsButton3_2", 3);
        }

        public void B4()
        {
            Buttonhandler("IsButton4_1", "IsButton4_2", 4);
        }

        public void B5()
        {
            Buttonhandler("IsButton5_1", "IsButton5_2", 5);
        }

         


        public void Buttonhandler(string button1text, string button2text, int buttonnumpressed )
        {
            int k = 0;
            for (k = 0;  k < Games.Count; k++)
            {
                //present player is in this index
                if ((Games[k].Playeroneconn == Context.ConnectionId) || (Games[k].Playertwoconn == Context.ConnectionId))
                {

                    //check these:
                    PlayerOneConnId = Games[k].Playeroneconn; 
                    PlayerTwoConnId = Games[k].Playertwoconn;
                    //Groupname = groupname;
                    whoseturn = Games[k].Whosturn;
                    buttonAnswer = Games[k].Answer;
                    break;
                }
            }
                
                


                    string whoseturnisnt = "";
            if(whoseturn == PlayerOneConnId)
            {
                whoseturnisnt = PlayerTwoConnId;
            }
            else
            {
                whoseturnisnt = PlayerOneConnId;
            
            }
            //only continues if player here is their turn
            if (Context.ConnectionId == whoseturn)
            {
                //correct guess
                if (buttonAnswer == buttonnumpressed)
                {
                    //winner
                    //Clients.Client(whoseturn).SendAsync("IsButton1_1", "won");
                    Clients.Client(whoseturn).SendAsync(button1text, "won");
                    //loser
                    Clients.Client(whoseturnisnt).SendAsync(button2text, "won");
                    DisconnectGame();

                }
                //incorrect guess
                else
                {

                    //////////
                    //Changeturn() put here:
                    if (whoseturn == PlayerOneConnId)
                    {
                        whoseturn = PlayerTwoConnId;
                    }
                    else
                    {
                        whoseturn = PlayerOneConnId;
                    }
                    Games[k].Whosturn = whoseturn;
                    ///////////

                    Printturn(PlayerOneConnId, PlayerTwoConnId , whoseturn);
                    
                    //disables buttons
                    Clients.Client(PlayerOneConnId).SendAsync(button1text, "other");
                    Clients.Client(PlayerTwoConnId).SendAsync(button2text, "other");
                    

                }
            }
        }


        public override Task OnConnectedAsync()
        {

            //Clients.All.SendAsync("IsButton1", "no");


            return base.OnConnectedAsync();
        }





        public ChatHub()
        {

            //whoseturn = "";
        }

        public void Printturn(string PlayerOneConnId, string PlayerTwoConnId, string whoseturn)
        {
            if (whoseturn == PlayerOneConnId)
            {
                
                Clients.Client(PlayerTwoConnId).SendAsync("Printnames2");
                Clients.Client(PlayerOneConnId).SendAsync("Printnames1");
            }
            else
            {
               
                Clients.Client(PlayerTwoConnId).SendAsync("Printnames1");
                Clients.Client(PlayerOneConnId).SendAsync("Printnames2");
            }
        }
        public void Changeturn(string PlayerOneConnId, string PlayerTwoConnId, string whoseturn)
        {

            if (whoseturn == PlayerOneConnId)
            {
                whoseturn = PlayerTwoConnId;
            }
            else
            {
                whoseturn = PlayerOneConnId;
            }
        }
        public void GetGroups()
        {
            integer++;
            groupname = "group" + integer;
        }





        public override Task OnDisconnectedAsync(Exception e)
        {
            int flag = 0;
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

            }


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
                    PlayerOneConnId = (ClientList[k].ConnectionId);
                    PlayerTwoConnId = (Context.ConnectionId);

                    GetGroups();
                    EachGame Game1 = new EachGame();
                    ////////
                    buttonAnswer = 1; // random.Next(1,6);
                    ////////
                    //check these:
                    Game1.Playertwoconn = PlayerTwoConnId;
                    Game1.Playeroneconn = PlayerOneConnId;
                    Game1.Groupname = groupname;
                    Game1.Whosturn = PlayerOneConnId;
                   
                    Game1.Answer = buttonAnswer;
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
                        whoseturn = PlayerOneConnId.ToString();
                        await Clients.Client(PlayerTwoConnId).SendAsync("Printnames2");
                        await Clients.Client(PlayerOneConnId).SendAsync("Enablebuttons");
                        await Clients.Client(PlayerTwoConnId).SendAsync("Enablebuttons");
                        
                        break;
                    }

                }
            }





        }

    }


}
