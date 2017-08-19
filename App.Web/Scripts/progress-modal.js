$(function () {
    // Reference the auto-generated proxy for the hub.
    var progress = $.connection.progressHub;
    console.log(progress);

    // Create a function that the hub can call back to display messages.
    function progressBarModal(showHide) {

        if (showHide === "show") {
            $("#progressModal").modal("show");
            window.progressBarActive = true;

        } else {
            $("#progressModal").modal("hide");
            window.progressBarActive = false;
        }
    }

    progress.client.AddProgress = function (message, percentage) {
        progressBarModal("show");
        $("#status").html(message);
        $("#progressbar").width(percentage).html(percentage).attr("aria-valuenow",percentage);
        //if (percentage === "100%") {
        //    progressBarModal("hide");
        //}
    };

    $.connection.hub.start().done(function () {
        var connectionId = $.connection.hub.id;
        console.log(connectionId);
    });
});

