
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
    $scope.validatdata = function () {
        var msg = $scope._language == 'en-us' ? 'Validate following : ' : 'Validate following:';

        if ($('#SourceDirectoryName').val() == undefined || $('#SourceDirectoryName').val() == '' || $('#SourceDirectoryName').val() == null) {
            msg += '<br />  Please Fill the Source Directory';
        }
        if ($('#DestinationDirectoryName').val() == undefined || $('#DestinationDirectoryName').val() == '' || $('#DestinationDirectoryName').val() == null) {
            msg += '<br /> Please Fill the Destination Directory';
        }
        if (msg.length > 29) {
            ShowModelAlert("File", msg);
            return false;
        }
        return true;
    };
    $scope.CopyandMoveDirectory = function (act) {
        debugger;

        if (!$scope.validatdata()) {
            return false;
        }
        var confirmResult = confirm("Are you sure you want to " + act + " this directory?");
        if (!confirmResult) { return; }

        $http.post(SiteUrl + "/MoveFileOneDestinationToOther/CopyandMoveDirectory?sourceDirectory=" + $('#SourceDirectoryName').val() + "&targetDirectory=" + $('#DestinationDirectoryName').val() + "&act=" + act,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                if (data.length > 1) {
                    alert("File " + act + " Successfully.")
                    //$scope.Directory.WebConfigText = data;
                    $scope.OnPageLoad();
                    window.location = SiteUrl + "MoveFileOneDestinationToOther/MoveFile";
                }
                $scope.ClearForm();
            }).
            error(function (data, status) {

            });
    };
    $scope.AllFilesInDirectory = function () {
        $scope.DirFilesList = [];
        debugger;
        $(".se-pre-con").fadeIn("slow");
        if ($('#SourceDirectoryName').val() == undefined || $('#SourceDirectoryName').val() == '') {
            $('#SourceDirectoryName').val('');
            //alert(baseUrl);
        }

        $http.post(SiteUrl + "/MoveFileOneDestinationToOther/AllFilesInDirectory?sourceDirectory=" + $('#SourceDirectoryName').val(),
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.IsShowDirFiles = true;
                    $scope.DirFilesList = data;
                }
                //  $scope.ClearForm();
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.DirFilesList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.ClearForm = function () {

        $('#SourceDirectoryName').val('');
        $('#DestinationDirectoryName').val('');
        $scope.DirFilesList = [];
        $scope.IsShowDirFiles = false;
        $scope.IsTextFile = false;
        $scope.Viewtext = ""
    };


    $scope.replaceSlash = function () {

        var str = $('#SourceDirectoryName').val();
        var coopIndex = str.search(/coop/i);
        if (coopIndex !== -1) {
            var slashesAfterCoop = str.substring(coopIndex).match(/\//g);
            if (slashesAfterCoop) {
                var newStr = str.substring(0, coopIndex) + str.substring(coopIndex).replace(/\//g, "");
                $('#SourceDirectoryName').val(newStr);
            }
        }

        var str_d = $('#DestinationDirectoryName').val();
        var coopInd_d = str_d.search(/coop/i);
        if (coopInd_d !== -1) {
            var slashesAfterCoop_d = str_d.substring(coopInd_d).match(/\//g);
            if (slashesAfterCoop_d) {
                var newStr_d = str_d.substring(0, coopInd_d) + str_d.substring(coopInd_d).replace(/\//g, "");
                $('#DestinationDirectoryName').val(newStr_d);
            }
        }
    };

    $scope.OnPageLoad();

    $scope.uploadMultipleFiles = function () {
        if ($scope.obj.dirName != '' || $scope.obj.dirName != null) {


            var confirmResult = confirm("Are you sure you want to upload files");
            if (!confirmResult) { return; }
            var formData = new FormData();
            var uploadfiles = $("#fu").get(0);
            var uploadedfiles = uploadfiles.files;
            for (var i = 0; i < uploadedfiles.length; i++) {
                formData.append(uploadedfiles[i].name, uploadedfiles[i]);
            }
            formData.append("dirName", $scope.obj.dirName);
            debugger
            $http.post(SiteUrl + "/MoveFileOneDestinationToOther/UploadMultipleFile", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    alert("File Successfully uploaded.");
                }).
                error(function (data, status) {

                });
        }
    };


    $scope.ReadDirectoryData = function (value) {
        if ($('#SourceDirectoryName').val() == undefined || $('#SourceDirectoryName').val() == '') {
            $('#SourceDirectoryName').val('');
            ShowModelAlert("File", " Please Fill the Source Directory");
            return;            
        }     
        
        $http.post(SiteUrl + "/MoveFileOneDestinationToOther/ReadDirectoryData?sourceDirectory=" + $('#SourceDirectoryName').val() , 
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                if (data.length > 1) {
                    $scope.Viewtext = data;             
                    $scope.IsShowDirFiles = false;
                    $scope.IsTextFile = true;
                }
              
            }).
            error(function (data, status) {
                $scope.Viewtext = "";                
            });
    };
});






