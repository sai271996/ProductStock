var id = "";
$(document).ready(function () {
   
    debugger
    Getall();
    
    $("#btncalc").click(function () {
        debugger
         
         var price = $('#txtprice').val();
         var stocks = $('#txtstock').val();

         var total = price * stocks;

        $('#txttotalprice').val(total);
    });

    Insertdata();
    bind();
    update();
});

function Insertdata() {
    debugger

    $("#btnsave").click(function () {
        debugger
       var Name = $("#txtproudct").val();
       var prices = $('#txtprice').val();
       var totalstocks = $('#txtstock').val();
       var totalprice = $('#txttotalprice').val()

        if (Name == "" || prices == "" || totalstocks == "" || totalprice == "") {
            alert('Please fill out all the fields')
        }
        else if (Name != null && prices != null && totalstocks != null && totalprice != null) {


            var register = { 'Product_Name': Name, 'Price': prices, 'Total_stock': totalstocks, 'Total_price': totalprice }

            $.ajax({
                type: 'post',
                url: "/Home/Insert",
                //contentType: "application/json; charset=utf-8",
                datatype: 'json',
                data: register,
                success: function (response) {
                    debugger
                    console.log(response);
                    if (response == "success") {
                        // $("#Registerform")[0].reset();
                        alert('New Product Registered successfully');
                        clear();
                        Getall();

                    }
                }
            });

        }
    });

}

function  Getall() {
    debugger
    $.ajax({
        type: 'GET',
        url: '/Home/Getalldata',
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
                html += '<td>' + value.Product_ID + '</td>';
                html += '<td>' + value.Product_Name + '</td>';
                html += '<td>' + value.Price + '</td>';
                html += '<td>' + value.Total_stock + '</td>';
                html += '<td>' + value.Total_price + '</td>';
                html += '<td>' + "<input type='button' id='btnEdit' value='Edit' class ='btn btn-primary data-id ='" + value.ID + "'/>&nbsp;&nbsp;&nbsp;<input type='button' id ='btndelete' value='Delete' lete' class ='btn btn-danger' data-id ='" + value.ID + "'/>" + '</td>';
                //html += '<td>' + "<i id='btnDelete' class='btn btn-danger glyphicon glyphicon-trash' data-id ='" + value.ID + "'></i>" + '</td>';                
                html += '</tr>';
            });
            $('.tbody').html(html);
            $('#Usertable').DataTable();
            
        }
        
    });

}

function bind() {

    $('#Usertable tbody').on('click', '[id*=btnEdit]', function () {

        var tr = $(this).parents('tr');
        id = tr.find("td").eq(0).text();
        var names = tr.find("td").eq(1).text();
        var pricess = tr.find("td").eq(2).text();
        var stockss = tr.find("td").eq(3).text();
        var totalpricess = tr.find("td").eq(4).text();


        $("#txtproudct").val(names);
        $("#txtprice").val(pricess);
        $("#txtstock").val(stockss);
        $("#txttotalprice").val(totalpricess);

    });
}

function update() {


    $("#btnupdate").click(function () {
        debugger
        var product_id = id;
        var product_name = $("#txtproudct").val();
        var priceproduct = $('#txtprice').val();
        var totalstocks = $('#txtstock').val();
        var Totalprices = $("#txttotalprice").val();

        var update = { 'Product_ID': product_id, 'Product_Name': product_name, 'Price': priceproduct, 'Total_stock': totalstocks, 'Total_price': Totalprices };

        $.ajax({
            type: 'POST',
            url: '/Sale/Update',
            //async: true,
            contenType: 'application/json; charset=utf-8',
            datatype: 'json',
            data: update,
            success: function (data)
            {
                debugger;
                if (data == "success")
                    alert('Product Details have been updated')
                clear();
                Getall();
            }
        });
    });

}

function clear() {
    $("#txtproudct").val("");
    $('#txtprice').val("");
    $('#txtstock').val("");
    $("#txttotalprice").val("");
}
