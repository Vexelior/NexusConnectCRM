var openTickets = $('#openTickets');
var newTickets = $('#newTickets');
var pendingTickets = $('#pendingTickets');
var closedTickets = $('#closedTickets');
var rejectedTickets = $('#rejectedTickets');
var ticketsDisplay = $('#ticketsDisplay');

$(function () {
    $.ajax({
        url: 'HelpInfo/GetOpenTickets',
        type: 'GET',
        success: function (result) {
            ticketsDisplay.html(result);
        }
    });
});

$(openTickets).on('click', function () {
    ticketsDisplay.empty();

    newTickets.removeClass('active');
    pendingTickets.removeClass('active');
    closedTickets.removeClass('active');
    rejectedTickets.removeClass('active');
    this.classList.add('active');

    $.ajax({
        url: 'HelpInfo/GetOpenTickets',
        type: 'GET',
        success: function (result) {
            ticketsDisplay.html(result);
        }
    });
});

$(newTickets).on('click', function () {
    ticketsDisplay.empty();

    openTickets.removeClass('active');
    pendingTickets.removeClass('active');
    closedTickets.removeClass('active');
    rejectedTickets.removeClass('active');
    this.classList.add('active');

    $.ajax({
        url: 'HelpInfo/GetRespondedTickets',
        type: 'GET',
        success: function (result) {
            ticketsDisplay.html(result);
        }
    });
});

$(pendingTickets).on('click', function () {
    ticketsDisplay.empty();

    newTickets.removeClass('active');
    openTickets.removeClass('active');
    closedTickets.removeClass('active');
    rejectedTickets.removeClass('active');

    this.classList.add('active');
    $.ajax({
        url: 'HelpInfo/GetPendingTickets',
        type: 'GET',
        success: function (result) {
            ticketsDisplay.html(result);
        }
    });
});

$(closedTickets).on('click', function () {
    ticketsDisplay.empty();

    newTickets.removeClass('active');
    openTickets.removeClass('active');
    pendingTickets.removeClass('active');
    rejectedTickets.removeClass('active');
    this.classList.add('active');

    $.ajax({
        url: 'HelpInfo/GetClosedTickets',
        type: 'GET',
        success: function (result) {
            ticketsDisplay.html(result);
        }
    });
});

$(rejectedTickets).on('click', function () {
    ticketsDisplay.empty();

    newTickets.removeClass('active');
    openTickets.removeClass('active')
    closedTickets.removeClass('active');
    pendingTickets.removeClass('active');

    this.classList.add('active');

    $.ajax({
        url: 'HelpInfo/GetRejectedTickets',
        type: 'GET',
        success: function (result) {
            ticketsDisplay.html(result);
        }
    });
});
