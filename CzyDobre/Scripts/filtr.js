$('#filtr1').change(function () {
    id = $('#sort option:selected').val();
    filtr1 = $('#filtr1 option:selected').val();
    filtr2 = $('#filtr2 option:selected').val();
    filtr3 = $('#filtr3 option:selected').val();
    if (filtr1 == 0 && id == 0 && filtr2 == 0 && filtr3 == 0) {
        window.location.replace('/opinie/');
    }
    else if (filtr1 != 0 && id == 0 && filtr2 == 0 && filtr3 == 0) {
        window.location.replace('/opinie/?filtr1=' + filtr1);
    }
    else if (filtr1 != 0 && id != 0 && filtr2 == 0 && filtr3 == 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr1=' + filtr1);
    }
    else if (filtr1 != 0 && filtr2 != 0 && id == 0 && filtr3 == 0) {
        window.location.replace('/opinie/?filtr1=' + filtr1 + '&filtr2=' + filtr2);
    }   
    else if (filtr1 != 0 && filtr3 != 0 && id == 0 && filtr2 == 0) {
        window.location.replace('/opinie/?filtr1=' + filtr1 + '&filtr3=' + filtr3);
    }
    else if (filtr1 != 0 && id != 0 && filtr2 != 0 && filtr3 == 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr1=' + filtr1 + '&filtr2=' + filtr2);
    }
    else if (filtr1 != 0 && id != 0 && filtr2 == 0 && filtr3 != 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr1=' + filtr1 + '&filtr3=' + filtr3);
    }
    else if (filtr1 != 0 && id == 0 && filtr2 != 0 && filtr3 != 0) {
        window.location.replace('/opinie/?filtr1=' + filtr1 + '&filtr2=' + filtr2 + '&filtr3=' + filtr3);
    }
    else if (filtr1 != 0 && id != 0 && filtr2 != 0 && filtr3 != 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr1=' + filtr1 + '&filtr2=' + filtr2 + '&filtr3=' + filtr3);
    }
});


$('#filtr2').change(function () {
    id = $('#sort option:selected').val();
    filtr1 = $('#filtr1 option:selected').val();
    filtr2 = $('#filtr2 option:selected').val();
    filtr3 = $('#filtr3 option:selected').val();
    if (filtr1 == 0 && id == 0 && filtr2 == 0 && filtr3 == 0) {
        window.location.replace('/opinie/');
    }
    else if (filtr2 != 0 && id == 0 && filtr1 == 0 && filtr3 == 0) {
        window.location.replace('/opinie/?filtr2=' + filtr2);
    }
    else if (filtr2 != 0 && id != 0 && filtr1 == 0 && filtr3 == 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr2=' + filtr2);
    }
    else if (filtr2 != 0 && filtr1 != 0 && id == 0 && filtr3 == 0) {
        window.location.replace('/opinie/?filtr1=' + filtr1 + '&filtr2=' + filtr2);
    }
    else if (filtr2 != 0 && filtr3 != 0 && id == 0 && filtr1 == 0) {
        window.location.replace('/opinie/?filtr2=' + filtr2 + '&filtr3=' + filtr3);
    }
    else if (filtr2 != 0 && id != 0 && filtr1 != 0 && filtr3 == 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr1=' + filtr1 + '&filtr2=' + filtr2);
    }
    else if (filtr2 != 0 && id != 0 && filtr1 == 0 && filtr3 != 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr2=' + filtr2 + '&filtr3=' + filtr3);
    }
    else if (filtr2 != 0 && id == 0 && filtr1 != 0 && filtr3 != 0) {
        window.location.replace('/opinie/?filtr1=' + filtr1 + '&filtr2=' + filtr2 + '&filtr3=' + filtr3);
    }
    else if (filtr2 != 0 && id != 0 && filtr1 != 0 && filtr3 != 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr1=' + filtr1 + '&filtr2=' + filtr2 + '&filtr3=' + filtr3);
    }
});

$('#filtr3').change(function () {
    id = $('#sort option:selected').val();
    filtr1 = $('#filtr1 option:selected').val();
    filtr2 = $('#filtr2 option:selected').val();
    filtr3 = $('#filtr3 option:selected').val();
    if (filtr1 == 0 && id == 0 && filtr2 == 0 && filtr3 == 0) {
        window.location.replace('/opinie/');
    }
    else if (filtr3 != 0 && id == 0 && filtr2 == 0 && filtr1 == 0) {
        window.location.replace('/opinie/?filtr3=' + filtr3);
    }
    else if (filtr3 != 0 && id != 0 && filtr2 == 0 && filtr1 == 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr3=' + filtr3);
    }
    else if (filtr3 != 0 && filtr2 != 0 && id == 0 && filtr1 == 0) {
        window.location.replace('/opinie/?filtr2=' + filtr2 + '&filtr3=' + filtr3);
    }
    else if (filtr3 != 0 && filtr1 != 0 && id == 0 && filtr2 == 0) {
        window.location.replace('/opinie/?filtr1=' + filtr1 + '&filtr3=' + filtr3);
    }
    else if (filtr3 != 0 && id != 0 && filtr2 != 0 && filtr1 == 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr2=' + filtr2 + '&filtr3=' + filtr3);
    }
    else if (filtr3 != 0 && id != 0 && filtr2 == 0 && filtr1 != 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr1=' + filtr1 + '&filtr3=' + filtr3);
    }
    else if (filtr3 != 0 && id == 0 && filtr2 != 0 && filtr1 != 0) {
        window.location.replace('/opinie/?filtr1=' + filtr1 + '&filtr2=' + filtr2 + '&filtr3=' + filtr3);
    }
    else if (filtr3 != 0 && id != 0 && filtr2 != 0 && filtr1 != 0) {
        window.location.replace('/opinie/?id=' + id + '&filtr1=' + filtr1 + '&filtr2=' + filtr2 + '&filtr3=' + filtr3);
    }
});