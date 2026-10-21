
var UploadMod = angular.module('MoveFileModule', ['angularUtils.directives.dirPagination']);
UploadMod.controller("MoveFileCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.IsRootDir = false;
    $scope.IsWebConfig = false;

    // on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    alert("Error while translation");

                });

        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    alert("Error while translation");
                });


        }

    };
  

    $scope.permissions = { Read: false, Write: false, Modify: false, FullAccess:false };
    $scope.submitPermissions = function (action) {
        if (action == 1)
            $scope.permissions.Action = 1;
        else
            $scope.permissions.Action = 0;
        $scope.DirFilesList = [];
        if ($scope.permissions.SourceDirName == undefined || $scope.permissions.SourceDirName == '') {
            alert("Please Enter the Menu Name");
            return;
        }
        if (!$scope.permissions.Read && !$scope.permissions.Write && !$scope.permissions.Modify) {
            alert("Please select Action");
            return;
        }

        $http.post(SiteUrl + "/User/SetUserPermission/", $scope.permissions).
            success(function (data, status) {
                if (data.length > 0) {
                alert("Permission Update Successfully;")
                }
                //  $scope.ClearForm();
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {            
                $(".se-pre-con").fadeOut("slow");
            });
    };
   
    $scope.updateFullAccess=function(permission){
        $scope.permissions.Read = permission;
        $scope.permissions.Write = permission;
        $scope.permissions.Modify = permission;
        $scope.permissions.FullAccess = permission;
    }

    $scope.OnPageLoad();

  
});
