$(function () {
    $("#CatSE").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Home/AutoCompleteCategory/',
                data: "{'prefix': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;
                    }))
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
     
        
    });
});