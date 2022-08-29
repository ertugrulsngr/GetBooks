function CreateOrder() {
    var id = $('#cartId').val();
    var address = $('#address').val();
    if (address == "") {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Address can not be empty!',
        });
        return;
    }
    $.ajax({
        url: "/Customer/Cart/ApproveCart",
        data: { "CartId": id, "address": address },
        type: "GET",
        success: function (data) {
            if (data.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Successful',
                    text: data.messsage
                }).then((result) => {
                    window.location = "/Customer/Order/Index"
                });
            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: data.message
                });
            } 
        }
    })
}