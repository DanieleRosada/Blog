"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/hubs").build();

(async function loadComment() {
    console.log("loadComment");
    let comments;
    await fetch("https://localhost:44342/api/post/" + document.getElementById("id").value +"/comment").then(res => res.json()).then(res => comments = res);
    $(".comments").empty();
    comments.forEach(function (element) {
        console.log(element);
        $(".comments").append("<div class=\"comment\"><h4>" + element.author + "</h4><p>" + element.commentText + "</p><p>" + element.insertDate + "</p></div>");
    });
})();


connection.start().catch(function (err) {
    return console.error(err.toString());
});

connection.on("ReceiveMessage", function (user, message) {
    console.log("receiveMessage")
    $(".comments").append("<div class=\"comment new\"><h4>" + user + "</h4><p>" + message + "</p><p>Now</p></div>");
    $('.new').css('background', 'green')
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("comment").value;
    var form = JSON.stringify({ "Id": document.getElementById("id").value, "CommentText": message});
    fetch("https://localhost:44342/api/post/insert/comment", {
        method: "POST",
        body: form,
        headers: {
            "Content-Type": "application/json"
        }
    });

    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

// is Writing
document.getElementById("comment").addEventListener("focus", function IsWriting() {
    console.log("focus")
    var user = document.getElementById("userInput").value;
    connection.invoke("IsWriting", user).catch(function (err) {
        return console.error(err.toString());
    });
});

connection.on("UserIsWriting", function (user) {
    console.log("UserIsWriting")
    console.log(user);
    $(".IsWriting").append("<p class=\"userWrite\">" + user + " is Writing..</p>");
});

document.getElementById("comment").addEventListener("blur", function () {
    console.log("blur")
    var user = document.getElementById("userInput").value;
    connection.invoke("IsNotWriting", user).catch(function (err) {
        return console.error(err.toString());
    });
});

connection.on("UserIsNotWriting", function (user) {
    console.log("UserIsNotWriting")
    console.log(user);
    $('.userWrite').remove(":contains('"+user+"')"); 
});