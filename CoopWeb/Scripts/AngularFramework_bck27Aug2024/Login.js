
var LoginMod = angular.module('LoginModule', []);
LoginMod.controller("LoginCont", function ($scope, $http, $window) {
    // Log in
    $scope.UserLogin = function (UserCredential) {
        $http.post(SiteUrl+"/User/LoginCheck", $scope.UserCredential).
            success(function (data, status) {
                var host = $window.location.host;
                var landingUrl = "http://" + host + "/User/DashBoard";
                $window.location.href = landingUrl;
            }).
            error(function (data, status) {
            });
    };
});



var SSOMappingModule = angular.module('SSOMappingMod', []);
SSOMappingModule.controller("SSOMappingCont", function ($scope, $http, $window) {

    // START : Clear Form Fields
    $scope.ClearForm = function () {
        $scope.Mapping = '';
    };
    // END : Clear Form Fields

    // START : Mapping of User
    $scope.MapUser = function () {
       
    };
    // END : Mapping of User
});