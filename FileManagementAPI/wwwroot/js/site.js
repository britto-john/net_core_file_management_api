function onLoadDocuments() {
    $.get("/api", function (data) {

        var table = '';
        $.each(data, function (index, value) {
            data += '<tr>';
            data += '<td>' + (index+1) + '</td>';
            data += '<td>' + value + '</td>';
            data += "<td>";
            data += "<a href=\"/api/" + value + "\" ><i class='fas fa-download' ></i></a>";
            data += "&nbsp;&nbsp;<a onclick=\"ShowConfirmDialog('" + value + "')\" ><i class='fas fa-trash' ></i></a>";
            data += "</td>";
            data += '</tr>';
        });

        $(".table > tbody").html(data);
    });
}

function onUploadFile() {

    var formData = new FormData();
    formData.append('file', $('#file')[0].files[0]);

    $.ajax({
        url: '/api',
        type: 'POST',
        data: formData,
        processData: false,  
        contentType: false,  
        success: function (data) {
            onLoadDocuments();
            $('#fileModal').modal('hide');
        }
    });
}

function onDeleteDocumnet() {
    $.ajax({
        url: '/api/' + $("#fileName").val(),
        method: 'DELETE',
        contentType: 'application/json',
        success: function (result) {
            onLoadDocuments();
        },
        error: function (request, msg, error) {
            console.log(msg);
        }
    });
}

// ------------------------------------------------------------------------------
// Utility Methods
// ------------------------------------------------------------------------------
function ShowConfirmDialog(filename) {
    $("#fileName").val(filename);
    $("#confirmModal").modal();
}