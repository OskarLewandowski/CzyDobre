/* UŻYCIE
 * 
 * Wymagany jest input[type="file"] o identyfikatorze Photo, a także label go zawierający o id dropSpot.
 * DropSpot reaguje na przeciągnięcie pliku/ów, odpowienio go/je umieszczając w ciągu plików cachedFiles.
 * 
 * Wymagany jest kontener dla wyświetlania załadowanych plików dropContainer.
 * 
 */

// Pliki zapisane w cache'u
var cachedFiles = [];
var activePreviewImage = false;

$("#dropSpot,#Photo").on("drag dragstart dragend dragover dragenter dragleave drop", function (e) {
    e.preventDefault();
    e.stopPropagation();
})
    .on("drop", function (e) {
        AddFiles(e.originalEvent.dataTransfer.files)
    })
    .on("change", "#Photo", function () {
        AddFiles($("#Photo")[0].files)
        $("#Photo").val('');
    });

// Wykrywanie kliknięcia na przycisk usuwania
$(document).on("click", ".drag-and-drop-remove", function (e) {
    RemoveFile($(".drag-and-drop-remove").index(this));
});

// Przejście na wybrane zdjęcie.
$(document).on("click", ".drag-and-drop-preview-image", function (e) {
    if (e.target !== e.currentTarget) return;
    PreviewImage(cachedFiles.length - 1 - $(".drag-and-drop-preview-image").index(this))
});

// Akceptuje plik lub ciąg plików. Dodaje plik do ciągu plików w cache'u oraz tworzy ich miniaturki.
function AddFiles(files) {
    // Sprawdź typ plików
    const validImageTypes = ['image/jpg', 'image/jpeg', 'image/png'];
    for (let i = 0; i < files.length; i++) {
        var file = files[i];
        var fileType = file["type"];
        if ($.inArray(fileType, validImageTypes) < 0) {
            return;
        }
    }
    // Obecne zasobniki
    var zasobniki = $(".drag-and-drop-preview-image").length;
    // Dodaj pliki do cache'a
    cachedFiles.push.apply(cachedFiles, files);
    for (let i = zasobniki; i < cachedFiles.length; i++) {
        // Tworzenie zasobnika
        $("#dropContainer").prepend("<div class='drag-and-drop-preview-miniature position-relative'><img class='drag-and-drop-preview-image' /><i class='fa fa-times drag-and-drop-remove'></i></div>");
        // Dodawanie do niego treści
        var file = cachedFiles[i];
        (function (file) {
            var reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = function (e) {
                $(".drag-and-drop-preview-image").eq(cachedFiles.length-i-1).attr("src", e.target.result);
            }
        })(file);
    }
    PreviewImage(cachedFiles.length-1);
}

// Akceptuje int. Usuwa plik wraz z miniaturką na danej pozycji.
function RemoveFile(id) {
    var removeAt = cachedFiles.length - id - 1;
    cachedFiles.splice(removeAt, 1);
    $('.drag-and-drop-preview-miniature').eq(id).remove();
    // Ukryj wyświetlacz, jeśli nie ma plików do wyświetlenia.
    if (cachedFiles.length == 0) {
        $("#previewMain").addClass("d-none");
        activePreviewImage = false;
    }
    // Jeżeli został usunięty aktywny plik, wyświetl wcześniejszy jeśli istnieje.
    else if (removeAt == activePreviewImage && removeAt != 0) {
        PreviewImage(removeAt - 1)
    }
    // Jeżeli został usunięty aktywny plik, który był pierwszym plikiem, wyświetl kolejny pierwszy.
    else if (removeAt == activePreviewImage && removeAt == 0) {
        PreviewImage(removeAt)
    }
}

// Akceptuje int. Ustawia główny podgląd dla danego pliku.
function PreviewImage(id) {
    var file = cachedFiles[id];
    (function (file) {
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function (e) {
            $("#previewMain").attr("src", e.target.result);
            $("#previewMain").removeClass("d-none");
        }
    })(file);
    activePreviewImage = id;
}

// Przygotowanie do przesyłu zdjęć
$("form").submit(function () {
    const dT = new DataTransfer();
    for (i = 0; i < cachedFiles.length;i++) {
        dT.items.add(cachedFiles[i]);
    }
    Photo.files = dT.files;
});