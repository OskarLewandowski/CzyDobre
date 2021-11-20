$(document).ready(function () {
    $("#PName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Search/GetCountries",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.PName, value: item.PName };
                    }))

                }
            })
        },
        messages: {
            noResults: "",
            results: function (count) {
                return count + (count > 1 ? ' results' : ' result ') + ' found';
            }
        }
    });
})