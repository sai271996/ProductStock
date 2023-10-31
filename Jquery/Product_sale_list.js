
$(document).ready(function () {
    var table;
    Getall();
    excel();
});

function Getall() {
    debugger
    $.ajax({
        type: 'GET',
        url: '/Sale/Getsaledata',
        async: true,
        contenType: 'application/json; charset=utf-8',
        datatype: 'json',
        data: '',
        success: function (data) {
            debugger
            var result = JSON.parse(data);
            var html = '';
            $.each(result, function (key, value) {
                html += '<tr>';
                html += '<td>' + value.Product_Id + '</td>';
                html += '<td>' + value.Product_Name + '</td>';
                html += '<td>' + value.Price + '</td>';
                html += '<td>' + value.Stocks_buy + '</td>';
                html += '<td>' + value.Total_price + '</td>';
                html += '<td>' + value.Buy_date + '</td>';
                //html += '<td>' + "<input type='button' id='btnEdit' value='Edit' class ='btn btn-primary data-id ='" + value.ID + "'/>&nbsp;&nbsp;&nbsp;<input type='button' id ='btndelete' value='Delete' lete' class ='btn btn-danger' data-id ='" + value.ID + "'/>" + '</td>';
                //html += '<td>' + "<i id='btnDelete' class='btn btn-danger glyphicon glyphicon-trash' data-id ='" + value.ID + "'></i>" + '</td>';                
                html += '</tr>';
            });
            $('.tbody').html(html);
           table =  $('#Usertable').DataTable();

        }
    });

}

function excel() {
    $('#btnreport').on('click', function ()
    {
        debugger;   
        // Make an AJAX request to the server to trigger the download
        $.ajax({
           
            url:'/Sale/DownloadExcel', // Replace with the actual URL to your download action
            type: 'GET',
            xhrFields: {
                responseType: 'blob' // Set the response type to blob
            },
            success: function (data)
            {debugger
                //Create a temporary link element to trigger the download
                var url = window.URL.createObjectURL(data);
                var a = document.createElement('a');
                a.href = url;
                a.download = 'Sale_Report.xlsx'; // Replace with the desired file name
                a.style.display = 'none';
                document.body.appendChild(a);
                a.click();
                window.URL.revokeObjectURL(url);
                alert("File downloaded successfully");
            }
        });
    });
}