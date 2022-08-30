var dataTable;

var modalWrap = null;


var priceFormatter = new Intl.NumberFormat('tr-TR', {
    style: 'currency',
    currency: 'TRY',
});

function formatDate(date) {
    return date.replace("T", " ").substring(0, date.indexOf('.'));
}

const OrderStatus = {
    WaitingForApproval : 0,
    Approved: 1,
    Cancelled: 2 
}

$(document).ready(function () {
    LoadDataTable();
});

function LoadDataTable() {
    
    dataTable = $('#orderTable').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll"
        },
        "columns": [
            { "data": "id" },
            { "data": "user.email" },
            { "data": "createdDate", render: function (data) { return `${formatDate(data)}` } },
            { "data": "status", render: function (data) { return `${Object.keys(OrderStatus).find(key => OrderStatus[key] === data)}` } },
            {
                "data": "id",
                "class": "text-center",
                "render": function (data) {
                    return `
                            <div class="btn-group d-flex" role="group">
							    <a onclick=ShowDetails('${data}') class="w-100 btn btn-info"> Details</a>
						    </div>
                        `
                }
            }
        ]
    });
}

function ShowDetails(id) {
    $.ajax({
        url: "/Admin/Order/Get?id=" + id,
        type: "GET",
        success: function (data) {
            ShowModal(data, id);
        }
    })
}

function ShowModal(data, id) {
    if (modalWrap != null) {
        modalWrap.remove();
    }
    modalWrap = document.createElement('div');
    modalWrap.innerHTML = `
        <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Ordered Items</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="dropdown mb-3">
                            <button class="btn btn-primary dropdown-toggle" type="button" id="dropdownMenuButton1" data-bs-toggle="dropdown" aria-expanded="false">
                                Change Status
                            </button>
                            <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1">
                                <li><button onclick=ChangeStatus(${id},"0") class="dropdown-item">Waiting For Approval</button></li>
                                <li><button onclick=ChangeStatus(${id},"1") class="dropdown-item">Approved</li>
                                <li><button onclick=ChangeStatus(${id},"2") class="dropdown-item">Cancelled</button></li>
                            </ul>
                        </div>
                        <table id="cartItemsTable" class="table table-bordered table-striped text-center" style="width:100%">
		                    <thead>
			                    <tr>
				                    <th style="width:50%">Book Title</th>
				                    <th style="width:15%">Price</th>
				                    <th style="width:15%">Count</th>
			                    </tr>
		                    </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

    `;
    document.body.append(modalWrap);
    InsertToTable(data.data);
    var modal = new bootstrap.Modal(modalWrap.querySelector('.modal'));
    modal.show();
}

function InsertToTable(data) {
    var tableRef = document.getElementById('cartItemsTable').getElementsByTagName('tbody')[0];
    for (let index = 0; index < data.length; index++) {
        tableRef.insertRow().innerHTML = `
                <tr>
					<td>${data[index].bookTitle}</td>
					<td>${priceFormatter.format(data[index].price)}</td>
					<td>${data[index].count}</td>
				</tr>
            `;
    }
}

function ChangeStatus(id, statusId) {
    $.ajax({
        url: "/Admin/Order/ChangeStatus",
        data: { "id": id, "status": statusId },
        type: "GET",
        success: function (data) {
            if (data.success) {
                dataTable.ajax.reload();
                Swal.fire({
                    icon: 'success',
                    title: 'Successful',
                    text: data.messsage
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