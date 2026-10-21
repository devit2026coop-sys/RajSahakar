
var AuditorModule = angular.module('AuditorMod', ['angularUtils.directives.dirPagination']);
AuditorModule.controller("AuditorCont", function ($scope, $http) {

    // SCOPE
    //$scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize =12;
    $scope.divAuditorData = false;

    // Code for Language label setting
    $scope.getTranslation = function () {
        $scope._language = _language;
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
    // Get List of Divisions
    $scope.GetAuditData = function () {
        if ($scope.Auditor == undefined) {
            var Message = (_language == 'en-us' ? 'Please select Audit Type Firm/Individual' : 'कृपया ऑडिट प्रकार फर्म / व्यक्तिगत चुनें');
            alert(Message);
            return;
        }
        $http.post(SiteUrl +"/Society/GetAuditData/", { IsAuditFirm: $scope.Auditor.IsIndividual }).
            success(function (data, status) {
                $scope.divAuditorData = true;
                $scope.AuditorDataForRender = data;
                $scope.colsort = "";
                $scope.reverseSort = false;
                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                    }
                    return '';
                };
            }).
            error(function (data, status) {
                $scope.AuditorDataForRender = [];
                alert('Error while fetching Auditor data.');
            });
    };

    // START : On Page List
    $scope.getTranslation();
    //END : On PAge List

});
