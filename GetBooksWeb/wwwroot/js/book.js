var dataTable;
$(document).ready(function () {
    LoadDataTable();
});

function LoadDataTable() {
    console.log("halo");
    dataTable = $('#bookTable').DataTable({
        "ajax": {
            "url": "/Admin/Book/GetAll"
        },

        "columns": [
            { "data": "title" },
            { "data": "isbn" },
            { "data": "price" },
            { "data": "author" },
            {
                "data": "id",
                "class" : "text-center",
                "render": function (data) {
                    return `
                            <div class="btn-group d-flex" role="group">
							    <a href="/Admin/Book/Upsert?id=${data}" class="w-100 btn btn-primary me-2">
								    <i class="bi bi-pencil-square"></i> Edit</a>

							    <a  onClick=Delete('/Admin/Book/Delete/${data}') class="w-100 btn btn-danger">
                                    <i class="bi bi-trash-fill"></i> Delete</a>
						    </div>
                        `
                }
            }

        ]
    });
}

function Delete(url) {
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
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        Swal.fire(
                            'Deleted!',
                            data.message,
                            'success'
                        );
                    }
                    else {
                        Swal.fire(
                            'Error!',
                            data.message,
                            'error'
                        );
                    }
                }
            })
        }
    })
}