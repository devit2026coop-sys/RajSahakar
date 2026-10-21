
var MokaReport = angular.module("MokaReportModule", ['angularUtils.directives.dirPagination']);
MokaReport.controller("MokaReportCont", function ($scope, $http, $location, $rootScope) {


    //var url = ($location.absUrl());
    //var params = url.split('?');
    //var params_1 = params[1].split('&');


    //var index = params_1[0].indexOf('=');
    //var index1 = params_1[1].indexOf('=');

    var _language = 'en-hi';
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get("../Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("../Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }

        $scope.showLoader = false;
    };


    //$scope.GCCEId = params_1[0].substring(index + 1);
    //$scope.GCCERNO = params_1[1].substring(index1 + 1);
    const urlParams = new URLSearchParams(window.location.search);
    $scope.GCCEId = urlParams.get('eId');

    $scope.GopalCreditCardModel = [];

    $scope.getMokaReportData = function () {
        if ($scope.GCCEId != '' && $scope.GCCERNO != '') {
            // $http.post("../GopalCreditCard/Get_GopalApplicationFormViewData1/?GCCEId=" + $scope.GCCEId + "&GCCERNO=" + $scope.GCCERNO).
            $http.post("../GopalCreditCard/Get_GopalApplicationFormViewData_v1/?GCCEId=" + $scope.GCCEId ).
                success(function (data, status) {
                    console.log(data);
                    $scope.GopalCreditCardModel = data;
                }).
                error(function (data, status) {
                    $scope.GopalCreditCardModel = [];
                });
        }
    }
    $scope.OnPageLoad();
    $scope.getMokaReportData();
});