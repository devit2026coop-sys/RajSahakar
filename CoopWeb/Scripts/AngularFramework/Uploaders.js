
var UploadMod = angular.module('UploadModule', ['angularUtils.directives.dirPagination']);
UploadMod.controller("UploadCont", function ($scope, $http) {

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
            $scope.DirectoryList();
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
            $scope.DirectoryList();

        }



    };


    $scope.DirectoryList = function () {
        $scope.PDirectoryList = [];
        $http.get(SiteUrl + "/FileUploaders/GetDirectoryData/?ID=" + "").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.PDirectoryList = data;
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    }
    $scope.FormValidation = function () {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        //if ($scope.SocietyRegistration.ActId == "7") {
        //    if ($scope.SocietyRegistration.IsStatLevel == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select Sports Association level' : '\n <br /> कृपया क्रीड़ा संगम का स्तर का चयन करे |';

        //    }
        //}
        if (message.length > 29) {
            ShowModelAlert("FILE UPLOADING", message);
            return false;
        }
        return true;
    };

    $scope.UploadFile = function (file) {
        $scope.File = file[0];
    }
    // START : Save DIR
    $scope.SaveDir = function () {
        var formData = new FormData();
        angular.forEach($scope.PDirectoryList, function (item) {
            if (item.ID == $scope.Directory.ID) {
                {
                    if (item.NAME == 'ROOT') {
                        formData.append("IsNewDirectory", 1);
                        formData.append("NewDirectory", $scope.Directory.DIR);

                    }
                    formData.append("DirectoryName", item.NAME);
                }
            }
        });
        angular.forEach($scope.SDirectoryList, function (item) {
            if (item.IsDirectory2 == $scope.Directory.IsDirectory2) {
                formData.append("SubDirectoryName", $scope.Directory.SubDirectoryName == undefined ? item.NAME : $scope.Directory.SubDirectoryName);
            }
        });

        if ($scope.SDirectoryList.length <= 0) {

            formData.append("SubDirectoryName", "ROOT");

        }


        if ($scope.FormValidation()) {
            formData.append("File", $scope.File);
        }
        formData.append("IsWeb", $scope.Directory.IsWeb);
        $http.post(SiteUrl + "/FileUploaders/SaveDirectory", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                if (data.length > 1) {
                    $scope.Directory.WebConfigText = data;
                    $scope.OnPageLoad();
                    window.location = SiteUrl+"FileUploaders/FileUploaders";
                }
                $scope.ClearForm();
            }).
            error(function (data, status) {

            });
    };

    $scope.ClearForm = function () {
        $scope.IsDirectoryYes = false;
        $scope.IsDirectoryNO = false;
        $scope.Directory.ID = 0;
        $scope.Directory.IsDirectory2 = 0;
        $scope.Directory.DirectoryName = '';
        $scope.Directory.SubDirectoryName = '';
    };
    $scope.GetSubDir = function (id) {
        $scope.IsRootDir = false;
        $scope.IsDirectoryYes = false;
        //$scope.IsDirectoryNO = false;



        angular.forEach($scope.PDirectoryList, function (item) {
            if (item.NAME == 'ROOT' && item.ID == id)
                $scope.IsRootDir = true;

        });
        //IsDirectoryYes
        $scope.IsRootDir == true ? true : false;
        $scope.IsSubDir = ($scope.IsRootDir == true ? false : true);

        $scope.IsDirectoryNO = $scope.IsSubDir;

        $http.get(SiteUrl + "/FileUploaders/GetDirectoryData/?ID=" + $scope.Directory.ID).
            success(function (data, status) {
                $scope.SDirectoryList = data;
            }).
            error(function (data, status) {
            });
    }

    $scope.IsDirectoryShowHide = function (Directory) {
        $scope.labelheading = true;

        if (Directory.IsDirectory2.split('#')[1] > 0) {
            $scope.IsDirectoryYes = true;
            $scope.IsDirectoryNO = false;
        }
        else {
            $scope.IsDirectoryYes = false;
            $scope.IsDirectoryNO = true;
        }
    }

    // START : Set Rural/ Urban div show/hide
    $scope.ShowWebConfig = function (value) {
        $scope.IsWebConfig = value;
        var formData = new FormData();
        formData.append("IsWeb", $scope.Directory.IsWeb);
        $http.post(SiteUrl + "/FileUploaders/SaveDirectory", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                if (data.length > 1) {
                    $scope.Directory.WebConfigText = data;
                    //$scope.OnPageLoad();
                   // window.location = SiteUrl + "/FileUploaders/FileUploaders";
                }
                $scope.ClearForm();
            }).
            error(function (data, status) {

            });
    };

    $scope.OnPageLoad();


});






