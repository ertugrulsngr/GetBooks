
function AddToCart() {
	var id = $('#bookId').val();

	$.ajax({
		url: '/Customer/Cart/AddToCart?id=' + id,
		type: 'GET',
		success: function (data) {
			if (data.success) {
				toastr.success(data.message);
				if (data.newCount != null) {
					$("#cartItemCount").text("("+data.newCount+")");
                }
			}
			else {
				toastr.error(data.message)
            }
		},
		statusCode: {
			401: function () {
				var url = '/Shop/Book/Details?bookId=' + id
				window.location = "/Identity/Account/Login?ReturnUrl=" + url;
			}
		}
	})
}