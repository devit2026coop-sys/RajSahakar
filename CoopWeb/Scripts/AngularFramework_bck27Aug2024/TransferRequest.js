var TransferRequestModule = angular.module('TransferRequestMod', ['angularUtils.directives.dirPagination'])
TransferRequestModule.controller("TransferRequestCnt", function ($scope, $http) {
    

    $scope.UserTransferRequestData = function () {
        $scope.TransferRequestData = [];
        $http.get(SiteUrl +"/User/UserTransferRequest").
            success(function (data, status) {
                $scope.TransferRequestData = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Role list");
            });
    };
    $scope.UserTransferRequestData();

    // START : Code for Language label setting
    $scope.getTranslation = function () {
        $scope.language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl +"/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl +"/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
    };
    $scope.getTranslation();

   

    $scope.AddRoleClick = function (MapId) {
        $(this).attr("data-target", "#myModal");
        $scope.MappingFromId = MapId;
    };
  
});





