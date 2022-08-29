function IncreaseCount(cartItemTempId) {
    var url = "/Customer/Cart/IncreaseCount?id=" + cartItemTempId;
    $.ajax({
        url: url,
        success: function (data) {
            location.reload();
        }
    })
}

function DecreaseCount(cartItemTempId) {
    var url = "/Customer/Cart/DecreaseCount?id=" + cartItemTempId;
    $.ajax({
        url: url,
        success: function (data) {
            location.reload();
        }
    })
}

function Delete(cartItemTempId) {
    var url = "/Customer/Cart/DeleteItem?id=" + cartItemTempId;
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Delete'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    location.reload();
                }
            })
        }
    })
    
    
}