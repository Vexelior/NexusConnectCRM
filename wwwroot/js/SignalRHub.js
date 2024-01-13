const chatConnection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
const notifyConnection = new signalR.HubConnectionBuilder().withUrl("/notify").build();

/* ------------------------------
* Chat Notification
------------------------------ */
var fullName = "";

$(function () {
    $.ajax({
        url: "/IdentityHelper/GetName",
        type: "GET",
        success: function (result) {
            fullName = result;
        },
        error: function (error) {
            return "Invalid User";
        }
    });
});

chatConnection.start().catch(function (err) {
    return console.error(err.toString());
});

notifyConnection.start().catch(function (err) {
    return console.error(err.toString());
});

document.getElementsByClassName("chat-btn")[0].addEventListener("click", function () {
    document.getElementsByClassName("chat-btn")[0].innerHTML = `<i class="fa fa-comment comment"></i><i class="fa fa-x close"></i>`;
});

var senderName = "";
var employeeName = "Help Desk";
var receiverId = "";
var newReceiverId = "";

$(function () {
    $.ajax({
        url: "/IdentityHelper/GetUserId",
        type: "GET",
        data: { usersName: employeeName },
        success: function (result) {
            receiverId = result;
        },
        error: function (error) {
            return `Error: ${error}`;
        }
    });
});

var senderId = "";
$(function () {
    $.ajax({
        url: "/IdentityHelper/GetUserId",
        type: "GET",
        data: { usersName: fullName },
        success: function (result) {
            senderId = result;
        },
        error: function (error) {
            return `Error: ${error}`;
        }
    });
});

document.getElementById("sendMessage").addEventListener("click", function (event) {
    var message = document.getElementById("Message").value;

    if (message == "") {
        return;
    }

    if (senderId == undefined || senderId == "") {
        $(function () {
            $.ajax({
                url: "/IdentityHelper/GetUserId",
                type: "GET",
                data: { usersName: fullName },
                success: function (result) {
                    senderId = result;
                },
                error: function (error) {
                    return `Error: ${error}`;
                }
            });
        });
    }

    if (fullName == employeeName) {
        var receiverName = document.getElementById("receiverName").innerHTML;
        $.ajax({
            url: "/IdentityHelper/GetUserId",
            type: "GET",
            data: { usersName: receiverName },
            success: function (result) {
                newReceiverId = result;
                chatConnection.invoke("SendMessageToUser", newReceiverId, fullName, senderId, message).catch(function (err) {
                    return console.error(err.toString());
                });
                chatConnection.invoke("DisplayMessageToSelf", fullName, senderId, message).catch(function (err) {
                    return console.error(err.toString());
                });
            },
            error: function (error) {
                return `Error: ${error}`;
            }
        });
    } else {
        chatConnection.invoke("SendMessageToEmployee", receiverId, fullName, senderId, message).catch(function (err) {
            return console.error(err.toString());
        });
        chatConnection.invoke("DisplayMessageToSelf", fullName, senderId, message).catch(function (err) {
            return console.error(err.toString());
        });
    }

    document.getElementById("Message").value = "";
    event.preventDefault();
});

chatConnection.on("ReceiveMessageFromUser", function (senderName, senderId, message) {
    $(`<li><strong><u><span id="receiverName" style="font-size: 16px !important;">${senderName}</span></u></strong>: ${message}</li>`).appendTo("#chat-messages");
    document.getElementsByClassName("chat-btn")[0].innerHTML = "";
    $('.chat-btn')[0].innerHTML = `<span class="badge rounded-pill bg-danger">!</span>`;
    document.getElementById("chat-notification").muted = false;
    document.getElementById("chat-notification").play();
});

chatConnection.on("ReceiveMessageFromEmployee", function (senderName, senderId, message) {
    $(`<li><strong><u><span id="receiverName" style="font-size: 16px !important;">${senderName}</span></u></strong>: ${message}</li>`).appendTo("#chat-messages");
    document.getElementsByClassName("chat-btn")[0].innerHTML = "";
    $('.chat-btn')[0].innerHTML = `<span class="badge rounded-pill bg-danger">!</span>`;
    document.getElementById("chat-notification").muted = false;
    document.getElementById("chat-notification").play();
});

chatConnection.on("ReceiveMessageFromSelf", function (senderName, senderId, message) {
    $(`<li><strong><u><i><span id="receiverName" style="font-size: 16px !important;">You</span></i></u></strong>: ${message}</li>`).appendTo("#chat-messages");
});


/* ------------------------------
* Help Response Notification
------------------------------ */
var recipient;
var sender;
var response;

try {
    recipient = document.getElementById("Help_Author").value;
    sender = document.getElementById("Responder").value;
    response = document.querySelector('textarea');

    document.getElementById("helpResponseForm").addEventListener("submit", function (event) {
        var totalEmployeeRespondedTickets;

        $.ajax({
            url: "/IdentityHelper/GetTotalEmployeeRespondedTickets",
            type: "GET",
            data: { userId: recipient },
            success: function (result) {
                totalEmployeeRespondedTickets = result;
            },
            error: function (error) {
                return "Invalid User";
            }
        });

        console.log(response);

        if (response.value != "") {
            notifyConnection.invoke("Notify", sender, recipient, totalEmployeeRespondedTickets).catch(function (err) {
                return console.error(err.toString());
            });
        } else {
            alert("Please enter a response.");
            event.preventDefault();
        }
    });
} catch (e) {
    // Do nothing
}

notifyConnection.on("ReceiveNotification", function (sender, receiver, message) {
    $(`<div class="toast-container position-fixed top-0 end-0 p-3">
          <div id="liveToast" class="toast" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="toast-header">
                <i class="fa fa-exclamation-circle me-2 text-warning"></i>
              <strong class="me-auto">Notification</strong>
              <small>Just now...</small>
              <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
            <div class="toast-body">
              <p>One of your tickets has been responded to.</p>
              <p>Please check your tickets <a href="/HelpInfo">here</a>.</p>
            </div>
          </div>
        </div>`)
        .appendTo("body");

    $('.toast').toast('show');

    document.getElementById("ticket-notification").muted = false;
    document.getElementById("ticket-notification").play();
});