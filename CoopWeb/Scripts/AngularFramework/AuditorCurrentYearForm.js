var MasterMod = angular.module('ApplicationAuditorCurrentYearFormModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("CooperativeAuditorCurrentYearFormCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.AuditorMasterData = [];

    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = 'en-us';
        $(".se-pre-con").fadeIn("slow");
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

            $scope.GetAllAuditorList();
            $scope.ClearAuditorForm = function () {
                $scope.AuditorMaster = null;
                $(".se-pre-con").fadeOut("slow");

            };
        }



        $scope.Department = [];
        $http.get(SiteUrl +"/Master/GetMasterData/?MasterType=Department&MasterCode=-1").

            success(function (data, status) {

                $scope.Department = data;
            }).
            error(function (data, status) {
                $scope.Department = [];
            });

    };

    $scope.SetStatus = function (UnitId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Master/ChangeStatusunit", { Id: UnitId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllSubUnitList();

            }).
            error(function (data, status) {
            });
    };
    $scope.GetAllAuditorList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl +"/Auditor/AuditorList").
            success(function (data, status) {
                $scope.AuditData = data;
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
            error(function (data1, status) {
            });
        $(".se-pre-con").fadeOut("slow");
    }

    $scope.update = function (item) {

        $scope.AuditorMaster.DepartmentId = item.departmentId;
        $scope.AuditorMaster.UnitNameEn = item.UnitNameEn;
        $scope.AuditorMaster.UnitNameHi = item.UnitNameHi;
        $scope.AuditorMaster.IsActive = item.IsActive == 1 ? true : false;
        $scope.AuditorMaster.UnitId = item.UnitId;
    };

    $scope.OnPageLoad();
    $scope.GetAllAuditorList();

});






