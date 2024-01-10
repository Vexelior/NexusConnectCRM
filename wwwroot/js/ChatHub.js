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

const connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

connection.start().catch(function (err) {
    return console.error(err.toString());
});

document.getElementsByClassName("chat-btn")[0].addEventListener("click", function () {
    document.getElementsByClassName("chat-btn")[0].innerHTML = `<i class="fa fa-comment comment"></i><i class="fa fa-x close"></i>`;
});

document.getElementById("sendMessage").addEventListener("click", function (event) {
    var message = document.getElementById("Message").value;
    connection.invoke("SendMessage", fullName, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

connection.on("ReceiveMessage", function (user, message) {
    if (user == fullName) {
        $(`<li><strong><u><i>You</u></i></strong>: ${message}</li>`).appendTo("#chat-messages");
    } else {
        $(`<li><strong><u>${user}</u></strong>: ${message}</li>`).appendTo("#chat-messages");
        // clear 
        document.getElementsByClassName("chat-btn")[0].innerHTML = "";
        $('.chat-btn')[0].innerHTML = `<span class="badge rounded-pill bg-danger">!</span>`;
    }
    document.getElementById("Message").value = "";
});
