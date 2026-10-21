
var MasterModule = angular.module('WebAdminMod', ['zingchart-angularjs']);
MasterModule.controller("WebAdminCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.OnPageLoad = function () {

        $(".se-pre-con").fadeIn("slow");
        
        
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

            $scope.ClearUnitForm = function () {
                $scope.deshboaddata = null;
            };
        }


        $http.get(SiteUrl + "/User/GetWebDashboard").
            success(function (data, status) {
                $scope.DashBoardData = data;
                console.log(data);
            }).
            error(function (data, status) {
                $scope.DashBoardData = [];
            });
        
        $(".se-pre-con").fadeOut("slow");

    };
    $scope.OnPageLoad();

});






