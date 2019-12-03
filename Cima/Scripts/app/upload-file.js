angular.module("UploadFile", ["kendo.directives"]);

function UploadFileController($scope) {

    var dropZone = document.getElementById('droptarget');
    dropZone.addEventListener('dragover', handleDragOver, false);
    dropZone.addEventListener('dragleave', handleDragOver, false);
    dropZone.addEventListener('drop', handleFileSelect, false);

    $scope.uploadFile = function () {
        $(".upload-spinner").show();
       
        $.ajax({
            type: 'POST',
            url: '/UploadFile/SaveFileToLanding',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                if (response != "FAILURE") {
                    $(".upload-spinner").hide();
                    $("#filesList").html('');
                   
                }
            },
            failure: function (response) {
                alert("erreur uploading files");
            }
        });
    };

    $scope.cancelUpload = function () {
        $(".cancel-spinner").show();
        $.ajax({
            type: 'POST',
            url: '/UploadFile/CancelUploadingTmpFile',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                if (response != "FAILURE") {
                    $(".cancel-spinner").hide();
                    $("#filesList tbody").remove();
                }
            },
            failure: function (response) {
                alert("erreur cancel operation");
            }
        });
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
       uploadingFiles(f, output);
    }
    
   
}

// selection des fichiers via le bouton parcourir
function selectFiles(files) {
    
    // files is a FileList of File objects. List some properties.
    for (var i = 0, f; f = files[i]; i++) {
        uploadingFiles(f);
    }

}


// 
function handleDragOver(evt) {
    evt.stopPropagation();
    evt.preventDefault();
    if (evt.type == "dragover") {
        $("#file-upload-outline").addClass('file-upload-outline-dropover');
        $(".file-upload-text").hide();
        $(".file-upload-drop-text").show();
        $(".file-upload-errors").hide();
        evt.dataTransfer.dropEffect = 'copy'; // Explicitly show this is a copy.
    } else if (evt.type == "dragleave"){
        $("#file-upload-outline").removeClass('file-upload-outline-dropover');
        $(".file-upload-text").show();
        $(".file-upload-drop-text").hide();
        $(".file-upload-errors").hide();
        evt.dataTransfer.dropEffect = 'move';
    }
}

// 
function parseFile(outputArray, f) {
     outputArray.push('<tr>',
        '<td class="icon"><i class="fa fa-file"></i></td>',
        '<td class="name">', escape(f.name), '</td>',
         '<td class="actions"><i id="', escape(f.name),'" class="fa fa-times" onclick="deleteFiles(this);"></i></td>',
        '</tr> ');
}



// chargement des fichiers dans la table temporaire
function uploadingFiles(f) {

    var reader = new FileReader();

    reader.onload = function (event) {
        $(".file-upload-progress").slideDown();
        var RawData = event.target.result;
        var idCampagne = $("#selectedcampagne").val();
        $.ajax({
            type: 'POST',
            url: '/UploadFile/UploadingFile',
            data: JSON.stringify({ filename: f.name, filesize: f.size, file: RawData, idCampagne: idCampagne }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                $(".file-upload-progress").slideUp();
                if (response != "FAILURE") {
                    var outputArray = [];
                    parseFile(outputArray, f);

                    $("#filesList").append('<tbody>' + outputArray.join('') + '</tbody>');

                    $(".file-upload-errors").hide();

                }
                else {
                    
                    $(".file-upload-errors").show();
                }

            },
            failure: function (response) {
                alert("erreur uploading");
            }
        });


    }

    reader.readAsText(f);  
}

// supprimer le fichier chargé
function  deleteFiles(el) {

    var filename = el.id;

    $.ajax({
        type: 'POST',
        url: '/UploadFile/DeleteUploadedFile',
        data: JSON.stringify({ filename: filename }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {
            if (response != "FAILURE") {
                el.closest('tr').remove();
            }
        },
        failure: function (response) {
            alert("erreur deleting row");
        }
    });
}




