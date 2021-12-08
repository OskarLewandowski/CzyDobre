$('#sort').change(function () {
    id = $('#sort option:selected').val();
    filtr1 = $('#filtr1 option:selected').val();
    filtr2 = $('#filtr2 option:selected').val();
    filtr3 = $('#filtr3 option:selected').val();
      if (filtr1 == 0 && id == 0 && filtr2 == 0 && filtr3 == 0) {
        window.location.replace('/opinie/');
    }
    else if (id != 0 && filtr1 == 0 && filtr2==0 && filtr3==0) {
        window.location.replace('/opinie/?id=' + id);
    }
    else if (id != 0 && filtr1 != 0 && filtr2 == 0 && filtr3 == 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr1=' + filtr1);
    }
    else if (id != 0 && filtr2 != 0 && filtr1 == 0 && filtr3 == 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr2=' + filtr2);
    }
    else if (id != 0 && filtr3 != 0 && filtr1 == 0 && filtr2 == 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr3=' + filtr3);
    }
    else if (id != 0 && filtr1 != 0 && filtr2 != 0 && filtr3 == 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr1=' + filtr1 + '&filtr2=' +filtr2);
    }
    else if (id != 0 && filtr1 != 0 && filtr2 == 0 && filtr3 != 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr1=' + filtr1 + '&filtr3=' + filtr3);
    }
    else if (id != 0 && filtr1 == 0 && filtr2 != 0 && filtr3 != 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr2=' + filtr2 + '&filtr3=' + filtr3);
    }
    else if (filtr1 != 0 && id != 0 && filtr2 != 0 && filtr3 != 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr1=' + filtr1 + '&filtr2=' + filtr2 + '&filtr3=' + filtr3);
    }
});

var url = window.location.href;
var sort = url.substring(url.indexOf("id=") + 3, url.indexOf('id=') + 4);
var filtr1 = url.substring(url.indexOf("filtr1=") + 7, url.indexOf('filtr1=') + 8);
var filtr2 = url.substring(url.indexOf("filtr2=") + 7, url.indexOf('filtr2=') + 8);
var filtr3 = url.substring(url.lastIndexOf('=') + 1);
var leng1 = (url.match(/id=/g) || []).length;
var leng2 = (url.match(/filtr1=/g) || []).length;
var leng3 = (url.match(/filtr2=/g) || []).length;
var leng4 = (url.match(/filtr3=/g) || []).length;
if (leng1 == 1) {
    document.getElementById('sort').getElementsByTagName('option')[sort].selected = 'selected'
}
if (leng2 == 1) {

    document.getElementById('filtr1').getElementsByTagName('option')[filtr1].selected = 'selected'
}
if (leng3 == 1)
{
    document.getElementById('filtr2').getElementsByTagName('option')[filtr2].selected = 'selected'
}
if (leng4 == 1)
{
    document.getElementById('filtr3').getElementsByTagName('option')[filtr3].selected = 'selected'
}

$('#reset').click(function () {
    window.location.replace('/opinie/');
});