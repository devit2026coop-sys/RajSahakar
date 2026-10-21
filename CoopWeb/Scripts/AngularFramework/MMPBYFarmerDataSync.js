
var MasterMod = angular.module('MMPBYFarmerSyncDataMod', ['angularUtils.directives.dirPagination']);

MasterMod.controller("MMPBYFarmerSyncDataCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.districtdataList = [];
    $scope.farmerdataList = [];
    $scope.search = {
        district_code: ''
    };

    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get("/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        $scope.DistrictList();
        $scope.getFarmerDetails();
    };

    $scope.syncFarmerData = function () {
        if (!$scope.search.district_code || $scope.search.district_code.trim() === "") {
            alert("Please enter district code");
            return;
        }

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/MMPBYFarmerDetails/MMPBYFarmerDetailsByDistrictcode/?DISTRICT_CODE=" + ($scope.search.district_code ?? 0))
            .success(function (data) {
                alert(data.message);
                location.reload();
                $(".se-pre-con").fadeOut("slow");
            }).error(function () {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.DistrictList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=0").
            success(function (data, status) {
                if (data != null) {
                    $scope.districtdataList = data;
                } else {
                    $scope.districtdataList = [];
                }
            }).error(function (data, status) {
                $scope.districtdataList = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    $scope.getFarmerDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/MMPBYFarmerDetails/GetMPBYFarmerDetailData?type=" + 'GetMMPBYDetails').
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.farmerData != null) {
                    $scope.farmerdataList = JSON.parse(data.Result.Data.farmerData);
                    console.log($scope.farmerdataList);
                } else {
                    $scope.farmerdataList = [];
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.Refresh = function () {
        $scope.search.district_code = '';
    }
    $scope.OnPageLoad();
});






