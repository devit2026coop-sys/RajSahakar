var AssetLiabilitySocietyWiseController = angular.module("AssetLiabilitySocietyWiseCnt", ['angularUtils.directives.dirPagination']);
AssetLiabilitySocietyWiseController.controller("AssetLiabilitySocietyWiseMod", function ($scope, $http) {


    // START : Page load Function
    $scope.OnPageLoad = function () {
        $scope._language = _language;

        if (_language === 'en-us') {
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
    // END : Page load Function

    // START : Get Data List
    $scope.AssetLiabilitySocietyWiseList = function () {
        $http.post(SiteUrl + "/Stock/LiabilityAssetListSocietyWise", { "SocietyId": "1" }).
            success(function (data, status) {
                console.log(data);
                $scope.AssetLiabilities = data;
            }).
            error(function (data1, status) {
                $scope.AssetLiabilities = [];
            });
    };
    // END : Get Data List

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.AssetLiabilitySocietyWiseList();
    // END : On Page Load
});