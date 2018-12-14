"use strict";

var message_box = document.getElementById("comments_box");
var userSpace = document.getElementById("userswriting");
var connection = new signalR.HubConnectionBuilder().withUrl("/hubs").build();

var lastTyped = new Date();
var typeTimeout = setTimeout(function () {
    connection.invoke("IsNotWriting", document.getElementById("userName_Current").value);
}, 10000);

connection.on("ReceiveComments", function (user, insertDate, message, postId) {
    if (document.getElementById("postId").value == postId) {
        appendComment(user, insertDate, message);
    }
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userName_Current").value;
    var message = document.getElementById("message_Text").value;
    var postId = document.getElementById("postId").value;
    connection.invoke("SendMessage", user, postId, message).catch(function (err) {
        return console.error(err.toString());
    });
    appendComment(user, new Date().toString(), message);
    event.preventDefault();
});

document.getElementById("message_Text").addEventListener("keydown", function (event) {
    if ((lastTyped.getTime() + 5000) < new Date().getTime()) {
        connection.invoke("IsWriting", document.getElementById("userName_Current").value);
        clearTimeout(typeTimeout);
        typeTimeout = setTimeout(function () {
            connection.invoke("IsNotWriting", document.getElementById("userName_Current").value);
        }, 10000);
    }
});

// matteo, franco is writing...
connection.on("UserIsWriting", function (user) {
    let whoiswriting = '';
    whoiswriting = userSpace.textContent;
    let newWriting = '';
    if (whoiswriting.indexOf(user) === -1) {
        whoiswriting.replace(' is writing...', '');
        whoiswriting.replace(' ', '');
        let users = whoiswriting.split(',');
        newWriting = user[0];
        users.forEach((elem, index) => {
            if (index !== 0)
                newWriting += ', ' + elem;
        });
        newWriting += ' is writing...';
    }
    userSpace.innerText = newWriting;
});

connection.on("UserIsNotWriting", function (user) {
    let whoiswriting = '';
    whoiswriting = userSpace.textContent;
    let newWriting = '';
    if (whoiswriting.indexOf(user) !== -1) {
        whoiswriting.replace(' is writing...', '');
        whoiswriting.replace(' ', '');
        let users = whoiswriting.split(',');
        users = users.reduce((accumulator, current) => {
            if (current !== user)
                accumulator.push(current);
        }, []);
        newWriting = user[0];
        users.forEach((elem, index) => {
            if (index !== 0)
                newWriting += ', ' + elem;
        });
        newWriting += ' is writing...';
    }
    userSpace.innerText = newWriting;
});

function GetComment(email, date, text) {
    return `<div class="row">
                <div class="col-md-4">
                    <p>${email}</p>
                </div>
                <div class="col-md-4 col-md-offset-4">
                    <p>${date}</p>
                </div>
            </div>
            <div class="row">
                <p>${text}</p>
            </div>`;
}

function appendComment(user, insertDate, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var comm = document.createElement("div");
    comm.setAttribute("class", "col-md-9");
    comm.innerHTML = GetComment(user, insertDate, msg);
    message_box.appendChild(comm);
}