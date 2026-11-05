var MasterMod = angular.module('auditorEquityLiabilitiesMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("auditorEquityLiabilitiesCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.AuditorMasterData = [];
    $scope.ShareCapitalOtherRows = [];
    $scope.ReservesFundsOtherRows = [];
    $scope.grantsSubsidiesOtherRows = [];
    $scope.memberDepositOtherRows = [];
    $scope.otherDepositRows = [];
    $scope.borrowingsRows = [];
    $scope.amt = { memberDepsoitAmt: "0" }


    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = 'en-us';
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

            $scope.GetAllAuditorList();
            $scope.ClearAuditorForm = function () {
                $scope.AuditorMaster = null;
                $(".se-pre-con").fadeOut("slow");

            };
        }



        $scope.Department = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Department&MasterCode=-1").

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
        $http.get(SiteUrl + "/Auditor/AuditorList").
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

    $scope.NextStep = function () {
        window.location.href = '/Auditor/AuditorSaleLiabilities';

    };


    //Added new dyanamic rows as per new
    $scope.shareCapitalAddRow = function () {
        $scope.ShareCapitalOtherRows.push({
            OtherName: '',
            TotalAmt: ''
        });
    };

    $scope.shareCapitalRemoveRow = function (index) {
        $scope.ShareCapitalOtherRows.splice(index, 1);
    };

    $scope.reservesFundsAddRow = function () {
        $scope.ReservesFundsOtherRows.push({
            OtherName: '',
            TotalAmt: ''
        });
    };

    $scope.reservesFundsRemoveRow = function (index) {
        $scope.ReservesFundsOtherRows.splice(index, 1);
    };

    $scope.grantsSubsidiesAddRow = function () {
        $scope.grantsSubsidiesOtherRows.push({
            OtherName: '',
            TotalAmt: ''
        });
    };

    $scope.grantsSubsidiesRemoveRow = function (index) {
        $scope.grantsSubsidiesOtherRows.splice(index, 1);
    };

    $scope.memberDepositAddRow = function () {
        $scope.memberDepositOtherRows.push({
            OtherName: '',
            TotalAmt: ''
        });
    };

    $scope.memberDepositRemoveRow = function (index) {
        $scope.memberDepositOtherRows.splice(index, 1);
        $scope.calculateMemberDepositAmount();
    };

    $scope.calculateMemberDepositAmount = function () {
        let subTotal = 0;
        $scope.memberDepositOtherRows.forEach(function (row) {
            subTotal += parseFloat(row.TotalAmt ?? 0);
        });
        $scope.amt.memberDepsoitAmt = subTotal.toFixed(2);
    };
    $scope.otherDepositAddRow = function () {
        $scope.otherDepositRows.push({
            OtherName: '',
            TotalAmt: ''
        });
    };
    $scope.otherDepositRemoveRow = function (index) {
        $scope.otherDepositRows.splice(index, 1);
    };



    $scope.borrowingsAddRow = function () {
        $scope.borrowingsRows.push({
            OtherName: '',
            TotalAmt: ''
        });
    };
    $scope.borrowingsRemoveRow = function (index) {
        $scope.borrowingsRows.splice(index, 1);
    };

    $scope.OnPageLoad();
    $scope.GetAllAuditorList();

});






