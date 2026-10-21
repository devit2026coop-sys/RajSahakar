var dpMod = angular.module('dpModule', ['angularUtils.directives.dirPagination']);
dpMod.controller("dpCont", function ($scope, $http) {
    $scope._language = 'en-us';
    $scope.result = {};
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
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                });
        }
    };

    $scope.OnPageLoad();

    $scope.GetRequestJson = function (isEncrypt) {
        $http.get(SiteUrl + "CommonServices/GetRequestJson?GID=" + $scope.obj.GID + "&isEncrypt=" + isEncrypt).
            success(function (data, status) {
                $scope.result = data.Result;

            }).
            error(function (data, status) {
            });
    };
    $scope.GetDecryptData = function (isEncrypt) {
        $http.post(SiteUrl + "CommonServices/GetDecryptedDataJson", { encryptedData: $scope.result.Result }).
            success(function (data, status) {
                $scope.result = data.Result;
            }).
            error(function (data, status) {
            });
    };
});
