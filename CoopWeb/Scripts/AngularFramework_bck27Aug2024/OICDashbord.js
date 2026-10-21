
var MasterMod = angular.module('OICDashbordModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("OICDashbordCont", function ($scope, $http) {

    // SCOPE
    //$scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50000;
    $scope.Acts = [];
    $scope.Districts = [];
    $scope.OICDashbordData = [];
    $scope.DistrictId = '0';
    $scope.ActId = '0';

    $scope.divAct = false;



    $scope.GetOICDashbordData = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.OICDashbordData = [];
            $http.post(SiteUrl + "/MIS/GetOICDashbordData", { "DistrictId": $scope.DistrictId, "ActId": $scope.ActId }).
                success(function (data, status) {
                $scope.OICDashbordData = data;
                $scope.divAct = true;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.OICDashbordData = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    // Code for Language label setting
    $scope.getTranslation = function () {
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
    };
    $scope.DistrictList = function () {
        $scope.Districts = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=-1").
            success(function (data, status) {
                $scope.Districts = data;
            }).
            error(function (data, status) {
                $scope.Districts = [];
            });
    };
    $scope.ActList = function () {
        $scope.Acts = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Act&MasterCode=-1").
            success(function (data, status) {
                $scope.Acts = data;
            }).
            error(function (data, status) {
                $scope.Acts = [];
            });
    };
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.getTranslation();
        $scope.DistrictList();
        $scope.ActList();
        $(".se-pre-con").fadeOut("slow");
    };

    $scope.OnPageLoad(); 
});