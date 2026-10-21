var WomanSavingCreditSocietyModule = angular.module("CoopDairyFederationMPRRPTMod", ['angularUtils.directives.dirPagination']);
WomanSavingCreditSocietyModule.controller("CoopDairyFederationMPRRPTCont", function ($rootScope, $scope, $http) {
    //$scope.GetWomanSavingCreditSociety = '',


        $scope.OnPageLoad = function () {

            $scope._language = _language;
            if (_language == 'en-us') {
                $http.get(SiteUrl + "/Translations/Translation_EN.json").
                    success(function (data, status) {
                        $scope.translation = data;
                        $scope.GetWomanSavingCreditSociety();
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
            };
        }

    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];
    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
    }

    $scope.ClearSearch = function () {
        $scope.Filter.SerachText = "";
    }

 

    $scope.OnPageLoad();
    //$scope.GetWomanSavingCreditSociety();
});


