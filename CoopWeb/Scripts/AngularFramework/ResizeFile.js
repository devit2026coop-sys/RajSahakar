
var MasterMod = angular.module('ApexBankSocietyDetailMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ApexBankSocietyDetailCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 60;

    $scope.DirectoryName = '';

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });


        }

        $scope.GetDistricts();
        $scope.showLoader = false;
        $(".se-pre-con").fadeOut("slow");
    };

    //Get District List
    $scope.GetDistricts = function () {

        $scope.Districts = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {

                $scope.Districts = data;
            }).
            error(function (data, status) {
                $scope.Districts = '';
            });
    };

    $scope.UploadFile = function (file) {
        $scope.File = file[0];
    }

    $scope.SetDir = function (name) {
        $scope.DirectoryName = name;
    };

    $scope.FormValidation = function () {

        var message = 'Validate following : ';
        if ($scope.DirectoryName == undefined || $scope.DirectoryName == "" || $scope.DirectoryName == null) {
            message += $scope._language == 'DIRECTORY NOT FOUND';
        }

        if ($scope.File == undefined || $scope.File == "" || $scope.File == null) {
            message += '\n Please UploadFile';
        }

        if (message.length > 29) {
            ShowModelAlert("FILE UPLOADING", message);
            return false;
        }
        return true;
    };

    $scope.UploadFileOnServer = function () {
        var formData = new FormData();

        if ($scope.FormValidation()) {
            formData.append("File", $scope.File);
            formData.append("DirectoryName", $scope.DirectoryName);

            $http.post(SiteUrl + "/FileUploaders/SaveFileAfterResize", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    alert(data);
                    if (data == '1') {
                        alert('UploadSuccessfully');
                        $scope.DirectoryName = '';
                        $scope.File = '';
                        $scope.OnPageLoad();
                        window.location = SiteUrl + "/Society/FileResize";
                    }

                }).
                error(function (data, status) {

                });

        }


    };

    //GET Load Table data
    $scope.DashboardData = function () {
        $http.post(SiteUrl + "/Society/GetFileListForResize?DisId=" + $scope.DistrictId).
            success(function (data, status) {
                $scope.TableData = [];
                $scope.TableDataFileResize = data;

            }).
            error(function (data, status) {
                $scope.TableData = [];
            });
        $scope.showLoader = false;

    };


    $scope.OnPageLoad();
    //END

    $scope.ShowDetails = function (SocietyIdENC) {
        if (SocietyIdENC != '') {
            window.location = "../Society/SocietyApplicationView?SocietyIDEnc=" + SocietyIdENC + "~_Society_ApexBankSocietyDetail~undefined";
        }
    };
});