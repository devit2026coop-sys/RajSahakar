var LoginMod = angular.module("Login", []);
LoginMod.controller("ValidateLogin", function ($scope, $http) {

    $scope.SaveMenuAction = function () {
        $http.post(SiteUrl + "/Menu/SaveMenuAction", $scope.objLogin).//$scope.userrole
            success(function (data, status) {
                var _Response = angular.fromJson(data);
                if (_Response.data > 0) {
                    $scope.userrole = null;
                }
            }).
            error(function (data, status) {
                alert("Error while Menu Action insert");
            });
    };
});
