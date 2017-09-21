$("body").on("click", ".delete", function () {
    var that = $(this).attr("data-id");
    if (that !== "") {
        $("#Id").val(that);
        swal({
            title: 'Are you want to delete this?',
            text: "You won't be able to undo this!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Confirm!',
            cancelButtonText: 'Cancel!',
            confirmButtonClass: 'btn btn-primary btn-lg m-r-1',
            cancelButtonClass: 'btn btn-danger btn-lg',
            buttonsStyling: false
        })
        .then(function (isConfirm) {
            if (isConfirm === true) {
                $("#deleteForm").submit();
            } else if (isConfirm === false) {
                swal({
                    title: 'Cancelled',
                    text: 'Information not deleted!',
                    type: 'info',
                    confirmButtonClass: 'btn btn-primary btn-lg',
                    buttonsStyling: false
                });
            }
        });
    }
});