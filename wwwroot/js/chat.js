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
    $("#messagesList").html("It's your turn!!");

});

connection.on("Printnames2", function () {

    //$('#registerButton').hide();
    $("#messagesList").html("It's not your turn!!");

});

////

connection.on("Enablebuttons", function () {

    
    $('#one').prop('disabled', false);
    $('#two').prop('disabled', false);
    $('#three').prop('disabled', false);
    $('#four').prop('disabled', false);
    $('#five').prop('disabled', false);
    

});

////

connection.on("IsButton1_1", function (message) {
    if (message == "won")
    {
        $("#messagesList").html("You have won!");
    }
    else
    {
        $('#one').prop('disabled', true);
    }
});

connection.on("IsButton1_2", function (message) {
    if (message == "won")
    {
        $("#messagesList").html("You have lost!");
    }
    else
    {
        $('#one').prop('disabled', true);
    } 
});
////////////////

connection.on("IsButton2_1", function (message) {
    if (message == "won") {
        $("#messagesList").html("You have won!");
    }
    else {
        $('#two').prop('disabled', true);
    }
});

connection.on("IsButton2_2", function (message) {
    if (message == "won") {
        $("#messagesList").html("You have lost!");
    }
    else {
        $('#two').prop('disabled', true);
    }
});

connection.on("IsButton3_1", function (message) {
    if (message == "won") {
        $("#messagesList").html("You have won!");
    }
    else {
        $('#three').prop('disabled', true);
    }
});

connection.on("IsButton3_2", function (message) {
    if (message == "won") {
        $("#messagesList").html("You have lost!");
    }
    else {
        $('#three').prop('disabled', true);
    }
});

connection.on("IsButton4_1", function (message) {
    if (message == "won") {
        $("#messagesList").html("You have won!");
    }
    else {
        $('#four').prop('disabled', true);
    }
});

connection.on("IsButton4_2", function (message) {
    if (message == "won") {
        $("#messagesList").html("You have lost!");
    }
    else {
        $('#four').prop('disabled', true);
    }
});

connection.on("IsButton5_1", function (message) {
    if (message == "won") {
        $("#messagesList").html("You have won!");
    }
    else {
        $('#five').prop('disabled', true);
    }
});

connection.on("IsButton5_2", function (message) {
    if (message == "won") {
        $("#messagesList").html("You have lost!");
    }
    else {
        $('#five').prop('disabled', true);
    }
});




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


document.getElementById("one").addEventListener("click", function (event) {

    connection.invoke("B1").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

///////////////
document.getElementById("two").addEventListener("click", function (event) {

    connection.invoke("B2").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("three").addEventListener("click", function (event) {

    connection.invoke("B3").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("four").addEventListener("click", function (event) {

    connection.invoke("B4").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("five").addEventListener("click", function (event) {

    connection.invoke("B5").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

///////////////