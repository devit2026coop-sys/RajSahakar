
var MasterMod = angular.module('GCCSyncDataMod', ['angularUtils.directives.dirPagination']);

MasterMod.controller("GCCSyncDataCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.dccbdataList = [];
    $scope.rsobdataList = [];
    $scope.search = {
        dccb_code: '',
        jan_member_id: ''
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
        $scope.DccbcodeList();
        $scope.getRsobData();
    };

    $scope.syncGCCData = function () {
        if (!$scope.search.dccb_code || $scope.search.dccb_code.trim() === "") {
            alert("Please enter dccb code");
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

    $scope.DccbcodeList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/GCCService/GetDccbcodeData").
            success(function (data, status) {
                if (data != null && data.Result.Data.dccbData != null) {
                    $scope.dccbdataList = JSON.parse(data.Result.Data.dccbData);
                } else {
                    $scope.dccbdataList = [];
                }
            }).error(function (data, status) {
                $scope.dccbdataList = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    $scope.getRsobData = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/GCCService/GetRsobOdData?type=" + 'GetRsobDataDetail').
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.rscbData != null) {
                    $scope.rsobdataList = JSON.parse(data.Result.Data.rscbData);
                    console.log('rsobdataList', $scope.rsobdataList);
                } else {
                    $scope.rsobdataList = [];
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.Refresh = function () {
        $scope.search.dccb_code = '';
        $scope.search.jan_member_id = '';
    }
    $scope.OnPageLoad();
});






