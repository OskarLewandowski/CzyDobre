/* UŻYCIE
 * 
 * Wymagany jest input[type="file"] o identyfikatorze Icon, a także label go zawierający o id dropSpot.
 * DropSpot reaguje na upuszczenie pliku, odpowienio go umieszczając w input'cie.
 * 
 * Wymagany jest kontener dla wyświetlania załadowanych plików dropContainer.
 * 
 */

var activePreviewImage = false;

$("#dropSpot,#Icon").on("drag dragstart dragend dragover dragenter dragleave drop", function (e) {
    e.preventDefault();
    e.stopPropagation();
})
    .on("drop", function (e) {
        AddFile(e.originalEvent.dataTransfer.files);
    })
    .on("change", "#Icon", function () {
        PreviewImage();
    });

// Wykrywanie kliknięcia na przycisk usuwania
$(document).on("click", ".drag-and-drop-remove", function (e) {
    RemoveFile();
});

// Akceptuje plik. Dodaje plik do input'a.
function AddFile(files) {
    // Sprawdź typ pliku
    const validImageTypes = ['image/jpg', 'image/jpeg', 'image/png'];
    var file = files[0];
    var fileType = file["type"];
    if ($.inArray(fileType, validImageTypes) < 0) {
        return;
    }
    // Transfer pliku do input'a
    const dT = new DataTransfer();
    dT.items.add(files[0]);
    Icon.files = dT.files;
    PreviewImage();
}

// Akceptuje int. Usuwa plik.
function RemoveFile() {
    // Ukryj wyświetlacz, jeśli nie ma pliku do wyświetlenia.
    $("#previewMain").addClass("d-none");

    $("input[type='file']").val([]);
    activePreviewImage = false;
    PreviewImage();
}

// Ustawia podgląd na plik w input'cie.
function PreviewImage() {
    var file = $("input[type='file']").prop("files")[0];
    if (file != null) {
        (function (file) {
            var reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = function (e) {
                $("#previewMain").attr("src", e.target.result);
                $("#previewMain").removeClass("d-none");
            }
        })(file);
        activePreviewImage = true;
    }
    else {
        return;
    }
}