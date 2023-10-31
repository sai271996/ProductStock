$(document).ready(function () {

    $("#btncalc").click(function () {
        debugger

        var price = $('#txtprice').val();
        var stocks = $('#txtstock').val();

        var total = price * stocks;

        $('#txttotalprice').val(total);
    });

    currentdate();
    productselect();
    buyproduct();
});

function currentdate() {

    $('#btndate').click(function () {
        debugger
        var currentdate = new Date();

        var todaydate = currentdate.getDate() + "-" + (currentdate.getMonth() + 1) + "-" + currentdate.getFullYear();

        $('#txtdate').val(todaydate);
    });

}
function productselect() {
    var selectedValue =""
    $('#Productlist').change(function () {
        debugger
        selectedValue = $(this).val();

       var id = { 'Product_ID': selectedValue };
        debugger
        $.ajax({
            type: 'GET',
            url: '/Sale/proudctprice_Totalstock_bind',
            //async: true,
            //contenType: 'application/json; charset=utf-8',
            datatype: 'json',
            data: id,
            success: function (data) {
                debugger
                var result = JSON.parse(data);
                var price = "";
                var stocks = "";
                $.each(result, function (key, value) {

                    price = value.Price;
                    stocks = value.Total_stock;

                });
                $('#txtprice').val(price);
                $('#txtavilable').val(stocks);
                $('#txtproduct_id').val(selectedValue);
            }
        });
    });
}

function buyproduct() {

    $('#btnBuy').click(function () {
        debugger
        var date = $("#txtdate").val()
        var selectedCountry = $('#Productlist option:selected').text();
        var id = $("#txtproduct_id").val();
        var price = $("#txtprice").val();
        var stock = $("#txtstock").val();
        var totalprice = $("#txttotalprice").val();

        var update = { 'Product_Id': id, 'Product_Name': selectedCountry, 'Price': price, 'Stocks_buy': stock, 'Total_price': totalprice, 'Buy_date': date };

        $.ajax({
            type: 'post',
            url: "/Sale/buyproduct",
            //async: true,
            //contentType: "application/json; charset=utf-8",
            datatype: 'json',
            data: update,
            success: function (response) {
                debugger
                console.log(response);
                if (response == "success") {
                    // $("#Registerform")[0].reset();
                    alert(`Product ${selectedCountry} has been sale`);
                    clear();
                    

                }
            },
            error: function (error) {
                console.log(error)
            }
        });
    });
}

function clear() {
    $('#txtdate').val("");
    $('#txtproduct_id').val("");
    $('#txtprice').val("");
    $('#txtavilable').val("");
    $('#txtstock').val("");
    $("#txttotalprice").val("");
}