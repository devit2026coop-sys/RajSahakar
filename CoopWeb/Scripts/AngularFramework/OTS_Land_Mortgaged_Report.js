var MasterMod = angular.module('otsMotganedMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("otsMotganedCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.search = {};
    $scope.DistrictList = [];
    $scope.PLDBList = [];
    $scope.PLDBBranches = [];
    $scope.search.LoanDistrictId = '0';
    $scope.ProgressLandMortgagedDetail = [];
    $scope.colspan = '8';
    $scope.ReportType = 1;
    $scope.AmountType = 0;
    $scope.LandMorgatedRecovery = "Recovery in cases in which land is not Mortgaged";
    $scope.OTSLandMortgagedDetail = 'OTSLandMortgagedReport';

    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get("/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        $scope.PLDBListData();
        $scope.GetOtsLandMortgagedDetails();
    };
    $scope.GetDistrictList = function () {
        $scope.DistrictList = [];
        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0";
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0") {
                    $scope.search.DISTRICT_ID = $("#hdnDistrictId").val(); $scope.search.PLDB_ID = "0"; $scope.search.PLDB_Branch_ID = "0"; $("#DISTRICT_ID").attr("disabled", "disabled");
                    $scope.PLDBListData();
                }
            }).error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });

    };

    $scope.PLDBListData = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0";

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=" + ($scope.search.DISTRICT_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBList = data;
                $(".se-pre-con").fadeOut("slow");

                if ($("#hdnPldbId").val() != "0") {
                    $scope.search.PLDB_ID = $("#hdnPldbId").val();
                    $scope.PLDBBranchList();
                    $("#PLDB_ID").attr("disabled", "disabled");
                }
            }).
            error(function (data, status) {
                $scope.PLDBList = [];
            });
    };
    $scope.PLDBBranches = [];
    $scope.PLDBBranchList = function (branch_ID) {
        $scope.search.PLDB_Branch_ID = "0";
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + ($scope.search.PLDB_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBBranches = data;

                if ($("#hdnPldbBranchId").val() != "0") {
                    $scope.search.PLDB_Branch_ID = $("#hdnPldbBranchId").val();
                    $("#PLDB_Branch_ID").attr("disabled", "disabled");
                }

                $(".se-pre-con").fadeOut("slow");
                $scope.GetOtsLandMortgagedDetails();

            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetOtsLandMortgagedDetails = function () {
        $scope.ProgressLandMortgagedDetail = [];

        if ($("#hdnDistrictId").val() != '0') { $scope.search.DISTRICT_ID = $("#hdnDistrictId").val(); }
        if ($("#hdnPldbId").val() != '0') { $scope.search.PLDB_ID = $("#hdnPldbId").val(); }
        if ($("#hdnPldbBranchId").val() != '0') { $scope.search.PLDB_Branch_ID = $("#hdnPldbBranchId").val(); }
        if ($scope.search.PLDB_ID == 0 && $scope.search.PLDB_Branch_ID == 0) { $scope.actionType = "GetOTSLandMortgagedNormalDetails" }
        if ($scope.ReportType == '1') { $scope.actionType = "GetOTSLandMortgagedNormalDetails"; $scope.colspan = '8'; }
        else if ($scope.ReportType == '2') { $scope.actionType = "GetOTSLandMortgaged103Details"; $scope.colspan = '12'; $scope.LandMorgatedRecovery = "Recovery"; }
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/MIS/GetOTSLandMortgagedDetails/?actionType=" + $scope.actionType + "&districtId=" + ($scope.search.DISTRICT_ID ?? 0) + "&pldbId=" + ($scope.search.PLDB_ID ?? 0) + "&pldbBranchId=" + ($scope.search.PLDB_Branch_ID ?? 0)).
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.otsLandMortgagedNormalData != null) {
                    $scope.ProgressLandMortgagedDetail = JSON.parse(data.Result.Data.otsLandMortgagedNormalData);
                    $scope.colsort = "";
                    $scope.reverseSort = false;
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.ReportTypeChange = function () {
        if ($("#hdnDistrictId").val() != '0') { $scope.search.DISTRICT_ID = "0"; }
        if ($("#hdnDistrictId").val() != '0') { $scope.search.DISTRICT_ID = $("#hdnDistrictId").val(); }
        if ($("#hdnPldbId").val() != '0') { $scope.search.PLDB_ID = $("#hdnPldbId").val(); }
        if ($("#hdnPldbBranchId").val() != '0') { $scope.search.PLDB_Branch_ID = $("#hdnPldbBranchId").val(); }
        $scope.search.DISTRICT_ID = "0";
        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0";
        $scope.GetOtsLandMortgagedDetails();
    }

    $scope.ReportDownload = function () {
        debugger;
        const originalTable = document.getElementById($scope.OTSLandMortgagedDetail);
        const clonedTable = originalTable.cloneNode(true);

        Array.from(clonedTable.querySelectorAll('th, td')).forEach(cell => {
            const style = window.getComputedStyle(cell);
            if (style.display === 'none' || cell.classList.contains('ng-hide')) {
                cell.remove();
            }
        });

        const wb = XLSX.utils.table_to_book(clonedTable, { sheet: "Report" });
        wb.Sheets["Report"]['!merges'] = $scope.getExcelMerges($scope.ReportType);
        XLSX.writeFile(wb, $scope.OTSLandMortgagedDetail + ".xlsx");
    };

    $scope.getExcelMerges = function (reportType) {
        const merges = [
            { s: { r: 0, c: 0 }, e: { r: 2, c: 0 } }, // Sr No.
            { s: { r: 0, c: 1 }, e: { r: 2, c: 1 } }, // PLDB
            { s: { r: 0, c: 2 }, e: { r: 1, c: 3 } }, // Total Cases (Count, Amount)
        ];

        const recoveryStartCol = 4;
        const pendingStartCol = reportType === '1' ? 8 : 8; // Both cases start from column 8 for pending level

        if (reportType === '1') {
            merges.push(
                { s: { r: 0, c: recoveryStartCol }, e: { r: 0, c: recoveryStartCol + 1 } }, // Land Mortgage
                { s: { r: 0, c: recoveryStartCol + 2 }, e: { r: 0, c: recoveryStartCol + 5 } }, // Recovery
                { s: { r: 1, c: recoveryStartCol + 2 }, e: { r: 1, c: recoveryStartCol + 3 } }, // Complete
                { s: { r: 1, c: recoveryStartCol + 4 }, e: { r: 1, c: recoveryStartCol + 5 } }  // Partial
            );
        } else if (reportType === '2') {
            merges.push(
                { s: { r: 0, c: recoveryStartCol }, e: { r: 0, c: recoveryStartCol + 3 } }, // Recovery
                { s: { r: 1, c: recoveryStartCol }, e: { r: 1, c: recoveryStartCol + 1 } }, // Complete
                { s: { r: 1, c: recoveryStartCol + 2 }, e: { r: 1, c: recoveryStartCol + 3 } } // Partial
            );
        }

        if (reportType === '1') {
            merges.push(
                { s: { r: 0, c: pendingStartCol }, e: { r: 0, c: pendingStartCol + 7 } },       // Header colspan
                { s: { r: 1, c: pendingStartCol }, e: { r: 2, c: pendingStartCol + 1 } },       // PLDB
                { s: { r: 1, c: pendingStartCol + 2 }, e: { r: 2, c: pendingStartCol + 3 } },   // Tehsildar
                { s: { r: 1, c: pendingStartCol + 4 }, e: { r: 2, c: pendingStartCol + 5 } },   // Patwari
                { s: { r: 1, c: pendingStartCol + 6 }, e: { r: 2, c: pendingStartCol + 7 } }    // Mortgaged
            );
        } else if (reportType === '2') {
            merges.push(
                { s: { r: 0, c: pendingStartCol }, e: { r: 0, c: pendingStartCol + 11 } },
                { s: { r: 1, c: pendingStartCol }, e: { r: 1, c: pendingStartCol + 1 } },        // PLDB
                { s: { r: 1, c: pendingStartCol + 2 }, e: { r: 1, c: pendingStartCol + 3 } },    // D.R.
                { s: { r: 1, c: pendingStartCol + 4 }, e: { r: 1, c: pendingStartCol + 5 } },    // Collector
                { s: { r: 1, c: pendingStartCol + 6 }, e: { r: 1, c: pendingStartCol + 7 } },    // Tehsildar
                { s: { r: 1, c: pendingStartCol + 8 }, e: { r: 1, c: pendingStartCol + 9 } },    // Patwari
                { s: { r: 1, c: pendingStartCol + 10 }, e: { r: 1, c: pendingStartCol + 11 } }   // Mortgaged
            );
        }

        return merges;
    };

    $scope.OnPageLoad();

});
MasterMod.filter('inrCurrency', function () {
    return function (input) {
        if (!input)
            return input;
        var num = parseFloat(input);
        if (isNaN(num)) return input;
        return num.toLocaleString('en-IN', { style: 'currency', currency: 'INR' });
    };
});
MasterMod.filter('ShowInLac', function () {
    return function (input, t) {
        if (isNaN(input)) return input;
        input = parseFloat(input).toFixed(2);
        if (t == 0)
            return input;
        else
            return (input / 100000).toFixed(2).replace(/\.0$/, '');
    };
});