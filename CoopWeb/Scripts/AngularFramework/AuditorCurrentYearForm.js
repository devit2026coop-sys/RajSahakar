var MasterMod = angular.module('auditorCurrentYearMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("auditorCurrentYearCtrl", function ($scope, $http) {

    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.AuditMPRData = [];
    $scope.search = {
        CurrentFYName: "",
        CurrentFY: 0
    };

    $scope.TotalCommittee = "0";
    $scope.TotalDepartment = "0";

    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = 'en-us';
        $(".se-pre-con").fadeIn("slow");
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").success(function (data, status) {
                $scope.translation = data;
            }).error(function (data, status) {
                alert("Error while translation");
            });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").success(function (data, status) {
                $scope.translation = data;
            }).error(function (data, status) {
                alert("Error while translation");
            });
        }

        $scope.SetCurrentFY();
        $scope.GetAuditMPRCommiteeData();
    };

    $scope.GetAuditMPRCommiteeData = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Auditor/GetAuditMPRMasterData").success(function (response, status) {
            if (response.Result != null && response.Result.Status == 200) {
                if (response.Result.Data != null && response.Result.Data.MPRData != null) {
                    $scope.AuditMPRData = JSON.parse(response.Result.Data.MPRData);
                    $scope.AuditMPRData.forEach(function (item) {
                        item.TotalCommittees = item.TotalCommittees ?? 0;
                        item.department = item.department ?? 0;
                        item.Value2 = item.Value2 ?? 0;
                        item.Value3 = item.Value3 ?? 0;
                        item.Value4 = item.Value4 ?? 0;
                        item.Value5 = item.Value5 ?? 0;
                        item.Value6 = item.Value6 ?? 0;
                        item.Value7 = item.Value7 ?? 0;
                        item.Value8 = item.Value8 ?? 0;
                        item.Value9 = item.Value9 ?? 0;
                        item.Value10 = item.Value10 ?? 0;
                        item.Value11 = item.Value11 ?? 0;
                        item.Value12 = item.Value12 ?? 0;
                        item.Value13 = item.Value13 ?? 0;
                    });
                }
                else { $scope.AuditMPRData = []; }
            }
            else {
                $scope.AuditMPRData = [];
            }
            $(".se-pre-con").fadeOut("slow");
        }).error(function (data1, status) {
            $scope.AuditMPRData = [];
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.TotalCalculate = function () {
        let subTotal = 0;
        let totalDepartment = 0;
        $scope.AuditMPRData.forEach(function (item) {
            subTotal += parseFloat(item.TotalCommittees ?? 0);
            totalDepartment += parseFloat(item.department ?? 0);
        });
        $scope.TotalCommittee = subTotal.toFixed(2);
        $scope.TotalDepartment = totalDepartment.toFixed(2);

    };

    $scope.SetCurrentFY = function () {
        $http.get(SiteUrl + "/Auditor/GetCurrentFinancialYearName")
            .success(function (data) {
                if (data != null) {
                    $scope.search.CurrentFYName = data[0].CurrentFYName;
                    $scope.search.CurrentFY = data[0].CurrentFY;
                }
            }).error(function () {
                alert("Unable to load Current Financial Year.");
            });
    };

    $scope.OnPageLoad();
});






