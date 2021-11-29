$(document).ready(function () {
    $('#ProductSE').autocomplete({
        minLength: 1,
        source: function (request, response) {
            $.ajax({
                url: '/Home/AutoComplete/',
                data: "{'prefix': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response(data);
                },
                error: function (err) {
                    alert(err);
                }
            });
        },
        focus: updateTextBox,
        select: updateTextBox
    })
        .autocomplete('instance')._renderItem = function (ul, item) {
            return $('<li>')
                .append("<img class='https://res.cloudinary.com/czydobre-pl/image/upload/v1638100053/CzyDobre-images/CzyDobre_6o3p6S7z6r1638100053_snapshot_2021_06_30_23_00_22_13117.png' />" )
                .append('<a>' + item.label + '</a>')
                .appendTo(ul);
        };

    function updateTextBox(event, ui) {
        $(this).val(ui.item.label);
        return false;
    }
});

/*
 $(function () {
    $("#ProductSE").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Home/AutoComplete/',
                data: "{'prefix': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;
                    }))
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },

        minLength: 1
    });
});
 */