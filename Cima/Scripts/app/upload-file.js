angular.module("KendoDemos", ["kendo.directives"]);

function MyCtrl($scope) {
    $scope.dropTargetText = "Drag here";

    $scope.draggableHint = function () {
        return $("#draggable").clone();
    }

    $scope.onDragEnd = function () {
        var draggable = $("#draggable");

        if (!draggable.data("kendoDraggable").dropped) {
            // drag ended outside of any droptarget
            $scope.dropTargetText = "Try again!";
        }

        draggable.removeClass("hollow");
    }

    $scope.onDragStart = function () {
        $scope.$apply(function () {
            $scope.draggableClass = "hollow";
            $scope.dropTargetText = "(Drop here)";
        });
    }

    $scope.onDragEnter = function (e) {
        $scope.$apply(function () {
            $scope.dropTargetText = "Now drop...";
        });
    }

    $scope.onDragLeave = function (e) {
        $scope.$apply(function () {
            $scope.dropTargetText = "(Drop here)";
        });
    }

    $scope.onDrop = function (e) {
        $scope.$apply(function () {
            $scope.dropTargetText = "You did great!";
            $scope.draggableClass = "";
        });
    }

    $scope.onClick1 = function () {
        alert("Primary button click");
    };
    $scope.onClick2 = function () {
        alert("Second button click");
    };
}

