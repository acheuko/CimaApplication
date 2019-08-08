angular.module("UploadFile", ["kendo.directives"]);

function UploadFileController($scope) {

    var dropZone = document.getElementById('droptarget');
    dropZone.addEventListener('dragover', handleDragOver, false);
    dropZone.addEventListener('dragleave', handleDragOver, false);
    dropZone.addEventListener('drop', handleFileSelect, false);

    $scope.uploadFile = function () {
        alert("Upload files ... ");
    };
    $scope.cancelUpload = function () {
        alert("Cancel upload ...");
    };

    $scope.chooseFile = function () {
        $("#fileselect").click();
    }

  

}

// récupération des fichiers sélectionnés
function handleFileSelect(evt) {

        evt.stopPropagation();
        evt.preventDefault();
        $("#file-upload-outline").removeClass('file-upload-outline-dropover');
        $(".file-upload-text").show();
        $(".file-upload-drop-text").hide();
       files = evt.dataTransfer.files; // FileList object.

    // files is a FileList of File objects. List some properties.
    var output = [];
    for (var i = 0, f; f = files[i]; i++) {
        parseFile(output, f);
    }
    
    $("#filesList").append('<tbody>' + output.join('') + '</tbody>');
}

// selection des fichiers via le bouton parcourir
function selectFiles(files) {
    
    // files is a FileList of File objects. List some properties.
    var output = [];
    for (var i = 0, f; f = files[i]; i++) {
        parseFile(output, f);
    }

    $("#filesList").append('<tbody>' + output.join('') + '</tbody>');
}

function handleDragOver(evt) {
    evt.stopPropagation();
    evt.preventDefault();
    if (evt.type == "dragover") {
        $("#file-upload-outline").addClass('file-upload-outline-dropover');
        $(".file-upload-text").hide();
        $(".file-upload-drop-text").show();

        evt.dataTransfer.dropEffect = 'copy'; // Explicitly show this is a copy.
    } else {
        $("#file-upload-outline").removeClass('file-upload-outline-dropover');
        $(".file-upload-text").show();
        $(".file-upload-drop-text").hide();

        evt.dataTransfer.dropEffect = 'move';
    }
}


function parseFile(outputArray, f) {
     outputArray.push('<tr>',
        '<td class="icon"><i class="fa fa-file"></i></td>',
        '<td class="name">', escape(f.name), '</td>',
        '<td class="actions"><i class="fa fa-times"></i></td>',
        '</tr> ');
}
