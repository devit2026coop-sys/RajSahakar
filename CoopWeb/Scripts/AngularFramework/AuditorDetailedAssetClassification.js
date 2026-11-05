var MasterMod = angular.module('assetClassificationMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("assetClassificationCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false; 

    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = 'en-us';
        //$(".se-pre-con").fadeIn("slow");
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

    $scope.BackTo = function () {
        window.location.href = '/Auditor/AuditorDepreciationSchedule';
    };

    $scope.NextStep = function () {
        window.location.href = '/Auditor/Auditorotherprovision';
    };
     
    $scope.OnPageLoad(); 

});






