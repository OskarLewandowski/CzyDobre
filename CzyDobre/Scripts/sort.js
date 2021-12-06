$('#sort').change(function () {
    id = $('#sort option:selected').val();
    if (id == 0) {
        window.location.replace('/opinie/');
    }
    else {
        window.location.replace('/opinie/?id=' + id);
    }
});

var url = window.location.href;
var sort = url.substring(url.lastIndexOf('=') + 1);
document.getElementById('sort').getElementsByTagName('option')[sort].selected = 'selected'

