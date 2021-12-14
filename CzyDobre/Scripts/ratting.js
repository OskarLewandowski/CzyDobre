$(document).ready(function () {
    $.ajax({
        url: "/Home/GetRate",
        method: "GET",
        success: function (data) {
            data = JSON.parse(data);
            $('#ProductName1').text(data.ProductNameTop1);
            $('#ProductName2').text(data.ProductNameTop2);
            $('#ProductName3').text(data.ProductNameTop3);
            $('#ProductName4').text(data.ProductNameTop4);
            $('#CountRate1').text("Oceniło: " + data.OpinionCounterTop1);
            $('#CountRate2').text("Oceniło: " + data.OpinionCounterTop2);
            $('#CountRate3').text("Oceniło: " + data.OpinionCounterTop3);
            $('#CountRate4').text("Oceniło: " + data.OpinionCounterTop4);

            var taste1 = document.getElementById("taste-1").getElementsByTagName("span");
            [].forEach.call(taste1, function (x) {
                if (data.TasteRateTop1 >= x.getAttribute('data-rating')) {
                    x.classList.remove('fa-star-o');
                    x.classList.add('fa-star');
                }
            });
            var service1 = document.getElementById("service-1").getElementsByTagName("span");
            [].forEach.call(service1, function (x) {
                if (data.ServiceRateTop1 >= x.getAttribute('data-rating')) {
                    x.classList.remove('fa-star-o');
                    x.classList.add('fa-star');
                }
            });
            var ingredients1 = document.getElementById("ingredients-1").getElementsByTagName("span");
            [].forEach.call(ingredients1, function (x) {
                if (data.IngredientsRateTop1 >= x.getAttribute('data-rating')) {
                    x.classList.remove('fa-star-o');
                    x.classList.add('fa-star');
                }
            });


            var taste2 = document.getElementById("taste-2").getElementsByTagName("span");
            [].forEach.call(taste2, function (x) {
                if (data.TasteRateTop2 >= x.getAttribute('data-rating')) {
                    x.classList.remove('fa-star-o');
                    x.classList.add('fa-star');
                }
            });
            var service2 = document.getElementById("service-2").getElementsByTagName("span");
            [].forEach.call(service2, function (x) {
                if (data.ServiceRateTop2 >= x.getAttribute('data-rating')) {
                    x.classList.remove('fa-star-o');
                    x.classList.add('fa-star');
                }
            });
            var ingredients2 = document.getElementById("ingredients-2").getElementsByTagName("span");
            [].forEach.call(ingredients2, function (x) {
                if (data.IngredientsRateTop2 >= x.getAttribute('data-rating')) {
                    x.classList.remove('fa-star-o');
                    x.classList.add('fa-star');
                }

            });


            var taste3 = document.getElementById("taste-3").getElementsByTagName("span");
            [].forEach.call(taste3, function (x) {
                if (data.TasteRateTop3 >= x.getAttribute('data-rating')) {
                    x.classList.remove('fa-star-o');
                    x.classList.add('fa-star');
                }
            });
            var service3 = document.getElementById("service-3").getElementsByTagName("span");
            [].forEach.call(service3, function (x) {
                if (data.ServiceRateTop3 >= x.getAttribute('data-rating')) {
                    x.classList.remove('fa-star-o');
                    x.classList.add('fa-star');
                }
            });
            var ingredients3 = document.getElementById("ingredients-3").getElementsByTagName("span");
            [].forEach.call(ingredients3, function (x) {
                if (data.IngredientsRateTop3 >= x.getAttribute('data-rating')) {
                    x.classList.remove('fa-star-o');
                    x.classList.add('fa-star');
                }
            });

            var taste4 = document.getElementById("taste-4").getElementsByTagName("span");
            [].forEach.call(taste4, function (x) {
                if (data.TasteRateTop4 >= x.getAttribute('data-rating')) {
                    x.classList.remove('fa-star-o');
                    x.classList.add('fa-star');
                }
            });
            var service4 = document.getElementById("service-4").getElementsByTagName("span");
            [].forEach.call(service4, function (x) {
                if (data.ServiceRateTop4 >= x.getAttribute('data-rating')) {
                    x.classList.remove('fa-star-o');
                    x.classList.add('fa-star');
                }
            });
            var ingredients4 = document.getElementById("ingredients-4").getElementsByTagName("span");
            [].forEach.call(ingredients4, function (x) {
                if (data.IngredientsRateTop4 >= x.getAttribute('data-rating')) {
                    x.classList.remove('fa-star-o');
                    x.classList.add('fa-star');
                }
            });

        },
        error: function (err) {
        }
    });
    })