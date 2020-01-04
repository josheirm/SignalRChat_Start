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
                //find the game - both if thens
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
                    index = i;
                    flag = 1;
                    break;
                }
            }


            if (flag == 1)
            {
                //removes register button
                Clients.Group(Group).SendAsync("IsWaiting");
                //removes both members of pair using there connections
                Groups.RemoveFromGroupAsync(Cone1, Group);
                Groups.RemoveFromGroupAsync(Cone2, Group);
                Games.RemoveAt(index);
            }


        }
        
        //passes string to buttonhandler that sends to client and has number of button pressed
        public void B1()
        {
            Buttonhandler("IsButton1_1", "IsButton1_2", 1);
        }

        
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
                // find player that matches as curreent player and get all the informaation from list
                if ((Games[k].Playeroneconn == Context.ConnectionId) || (Games[k].Playertwoconn == Context.ConnectionId))
                {

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
            {   //finds alternate player that isn't the current player
                whoseturnisnt = PlayerTwoConnId;
            }
            else
            {
                whoseturnisnt = PlayerOneConnId;
            
            }
           
            if (Context.ConnectionId == whoseturn)
            {
                //correct guess
                if (buttonAnswer == buttonnumpressed)
                {
                    //display win and lose text
                    Clients.Client(whoseturn).SendAsync(button1text, "won");
                    Clients.Client(whoseturnisnt).SendAsync(button2text, "won");
                    //rewrites register button
                    //removes group
                    //remove game from game list
                     DisconnectGame();

                }
                //incorrect guess
                else
                    //change turn to other player
                    if (whoseturn == PlayerOneConnId)
                    {
                        whoseturn = PlayerTwoConnId;
                    }
                    else
                    {
                        whoseturn = PlayerOneConnId;
                    }
                    Games[k].Whosturn = whoseturn;
                    //displays is and isn't player's turn
                    Printturn(PlayerOneConnId, PlayerTwoConnId , whoseturn);
                    
                    //disables buttons
                    Clients.Client(PlayerOneConnId).SendAsync(button1text, "other");
                    Clients.Client(PlayerTwoConnId).SendAsync(button2text, "other");
                    }
            }
        
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public ChatHub()
        {

        }

        //print new turn indicator text (your turn and not your turn)
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
        
        //produces a unique group name with static integer
        public void GetGroups()
        {
            groupname = "group" + integer;
        }
        //a player disconnected
        public override Task OnDisconnectedAsync(Exception e)
        {
            int flag = 0;
            string Cone1 = "";
            string Cone2 = "";
            string Group = "";
            int index = 0;
            //get all the info
            for (int i = 0; i < Games.Count; i++)
            {
                if (Games[i].Playeroneconn == Context.ConnectionId)
                {
                    Cone1 = Games[i].Playeroneconn;
                    Group = Games[i].Groupname;
                    //removes the first user from group
                    Groups.RemoveFromGroupAsync(Cone1, Group);
                    index = i;
                    flag = 1;
                    break;
                }
                if (Games[i].Playertwoconn == Context.ConnectionId)
                {
                    Cone2 = Games[i].Playertwoconn;
                    Group = Games[i].Groupname;
                    //removes the sencond player from connection
                    Groups.RemoveFromGroupAsync(Cone2, Group);
                    index = i;
                    flag = 1;
                    break;
                }
            }
            if (flag == 1)
            {
                //clears the text 
                Clients.Group(Group).SendAsync("Printnames0");
                //no longer a pair so needs a new register button
                Clients.Group(Group).SendAsync("IsWaiting");
                //rmoves game from the list
                Games.RemoveAt(index);
            }
            return base.OnDisconnectedAsync(e);
        }
        //not used
        public async Task SendMessage(string user, string message)
        {
           await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", user, message);

        }
        public async Task Register()
        {
            Clients A_Client = new Clients();
            A_Client.ConnectionId = Context.ConnectionId;
            //no name recorded yet
            ClientList.Add(A_Client);
            //hides button
            await Clients.Caller.SendAsync("IsRegister");
            //
            var flag1 = 0;
           
            for (int k = ClientList.Count - 1; k >= 0; --k)
            {
                
                //not the current user, means there is a pair to match up
                if ((ClientList[k].ConnectionId) != (Context.ConnectionId))
                {
                    //get connections from list 
                    PlayerOneConnId = (ClientList[k].ConnectionId);
                    PlayerTwoConnId = (Context.ConnectionId);
                    //gets a unique group name
                    GetGroups();
                    //element
                    EachGame Game1 = new EachGame();
                    
                    ////////isn't used yet (for testing purposes)
                    // random.Next(1,6);
                    buttonAnswer = 1; 
                    ////////
                    //Values set the Game1 element so that they can be used later
                    Game1.Playertwoconn = PlayerTwoConnId;
                    Game1.Playeroneconn = PlayerOneConnId;
                    Game1.Groupname = groupname;
                    Game1.Whosturn = PlayerOneConnId;
                    Game1.Answer = buttonAnswer;
                    //this list used because hubs are transient, meaning they don't keep values after method ends
                    Games.Add(Game1);

                    
                    await Groups.AddToGroupAsync(PlayerTwoConnId, groupname);
                    await Groups.AddToGroupAsync(PlayerOneConnId, groupname);
                   
                    //remove other player, group has been added to (above)
                    ClientList.RemoveAt(k);
                    //there are two players flag
                    flag1 = 1;
                    break;
                }
            }
            //there was another player 
            if (flag1 == 1)
            {
                flag1 = 0;

                for (int j = ClientList.Count - 1; j >= 0; --j)
                {

                    if ((ClientList[j].ConnectionId) == (Context.ConnectionId))
                    { 
                        //removes current player (in game container (above))
                        ClientList.RemoveAt(j);
                        //prints first yor turm and not your turn
                        await Clients.Client(PlayerOneConnId).SendAsync("Printnames1");
                        await Clients.Client(PlayerTwoConnId).SendAsync("Printnames2");
                        //changed this recently
                        //await Clients.Client(PlayerOneConnId).SendAsync("Enablebuttons");
                        //await Clients.Client(PlayerTwoConnId).SendAsync("Enablebuttons");
                        await Clients.Group(groupname).SendAsync("Enablebuttons");
                         break;
                    }

                }
            }
        }
    }

}
