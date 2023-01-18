"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

$(".sendButton").prop('disabled', true);

connection.on("ReceiveMessage", function (message,user) {
    var ul = $("#messagesList");
    var text = $(`<li class="d-flex flex-row-reverse">
                    <div class="message-data text-right">
                        <span class="message-data-time">${user}</span>
                        <img src=\"https://bootdey.com/img/Content/avatar/avatar7.png\" alt="avatar">
                    </div>
                    <div class="message other-message float-right"> ${message}</div>
                </li>`)
    ul.append(text)
    $("#messageInput").val('');
});

connection.on("SetOnline", function (names) {
    $(names).each(function (index, item) {
        console.log(item.username)
        $(`#${item.username}`).html(`<i class="fa fa-circle online"></i> <span class="small">Online</span> `)
    })
});


connection.start().then(function () {
    $(".sendButton").prop('disabled', false);
}).catch(function (err) {
    return console.error(err.toString());
});

$(document).on('click', '.sendButton', function (event) {
    var message = $("#messageInput").val();
    connection.invoke("SendMessage", message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});