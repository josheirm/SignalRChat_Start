"use strict";



var integer = 0;
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();




//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;




connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});



connection.on("IsRegister", function () {

    
    $('#registerButton').hide();
   
   
});

connection.on("IsWaiting", function () {

    
    $('#registerButton').show();


});


connection.on("Printnames0", function () {

    //$('#registerButton').hide();
    $("#messagesList").html("1  ");

});


connection.on("Printnames1", function () {

    //$('#registerButton').hide();
    $("#messagesList").html("Player one its your turn!");

});

connection.on("Printnames2", function () {
    
    //$('#registerButton').hide();
    $("#messagesList").html("Player two its not your turn!");

});

//////////////
connection.on("IsButton1", function (message) {

    
    if (message == "yes") {
        $("#messagesList").html("You have won!");
    }
    else
    {
        $('#one').prop('disabled', true);
    }

    
});

//////////////


connection.start().then(function () {
    document.getElementById("sendButton").disabled = true;
}).catch(function (err) {
    return console.error(err.toString());
});



document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});


document.getElementById("registerButton").addEventListener("click", function (event) {
    
    connection.invoke("Register").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

///////////////

document.getElementById("one").addEventListener("click", function (event) {

    connection.invoke("B1").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

///////////////
