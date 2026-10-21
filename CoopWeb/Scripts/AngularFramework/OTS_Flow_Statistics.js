
var MasterMod = angular.module('otsMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("otsCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.search = {};
    $scope.DistrictList = [];
    $scope.search.LoanDistrictId = '0';
    $scope.ReportType = 1;

    $scope.GetPldbOtsSchemaList
    $scope.PLDBList = [];
    $scope.PLDBBranches = [];
    $scope.FilterType = "0";
    $scope.isNoRecord = false;
    $scope.colspan = "8";
    $scope.PLDBOtsReport = "";

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
        $scope.GetDistrictList();
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

                $scope.GetPldbOtsPendencySummaryDetails();
            }).error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });

    };
    $scope.PLDBListData = function () {
        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0";
        $scope.PLDBList = [];
        $scope.PLDBBranches = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=" + ($scope.search.DISTRICT_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBList = data;
                if ($scope.search.DISTRICT_ID > 0) { $scope.FilterType = 1; }
                $(".se-pre-con").fadeOut("slow");
                if ($("#hdnPldbId").val() != "0") {
                    $scope.search.PLDB_ID = $("#hdnPldbId").val(); $scope.PLDBBranchList();
                    $("#PLDB_ID").attr("disabled", "disabled");
                }
                $scope.GetPldbOtsPendencySummaryDetails();
            }).
            error(function (data, status) {
                $scope.PLDBList = [];
            });
    };
    $scope.PLDBBranchList = function (branch_ID) {
        $scope.search.PLDB_Branch_ID = "0";
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + ($scope.search.PLDB_ID ?? 0)).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.PLDBBranches = data;
                if ($scope.search.PLDB_ID > 0) { $scope.FilterType = 2; }
                if ($("#hdnPldbBranchId").val() != "0") {
                    $scope.search.PLDB_Branch_ID = $("#hdnPldbBranchId").val(); $("#PLDB_Branch_ID").attr("disabled", "disabled");
                }
                $scope.GetPldbOtsPendencySummaryDetails();
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetPldbOtsPendencySummaryDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.GetPldbOtsSchemaList = [];

        if ($("#hdnDistrictId").val() != '0') { $scope.search.DISTRICT_ID = $("#hdnDistrictId").val(); }
        if ($("#hdnPldbId").val() != '0') { $scope.search.PLDB_ID = $("#hdnPldbId").val(); }
        if ($("#hdnPldbBranchId").val() != '0') { $scope.search.PLDB_Branch_ID = $("#hdnPldbBranchId").val(); }
        var Action = "";
        if ($scope.search.DISTRICT_ID > 0 && $scope.search.PLDB_ID > 0 && $scope.search.PLDB_Branch_ID > 0) { $scope.FilterType = 3; }
        if ($scope.search.DISTRICT_ID == 0 && $scope.search.PLDB_ID == 0 && $scope.search.PLDB_Branch_ID == 0) {
            $scope.FilterType = 0; $scope.search.PLDB_ID = "0"; $scope.search.PLDB_Branch_ID = "0"; $scope.PLDBList = []; $scope.PLDBBranches = [];
        }
        if ($scope.ReportType == '1') {
            $scope.PLDBOtsReport = 'OTS-Flow-Statistics-Pendency';
            if ($scope.FilterType == '1') { Action = 'GetPendencySummaryByDistrict'; } else if ($scope.FilterType == '2') { Action = "GetPendencySummaryByPldb"; }
            else if ($scope.FilterType == '3') { Action = "GetPendencySummaryByPldbBranch"; } else { Action = 'GetByPendencySummary'; }
        }
        else if ($scope.ReportType == '2') {
            $scope.PLDBOtsReport = 'OTS-Flow-Statistics-OverAll';
            if ($scope.FilterType == '1') { Action = 'GetByOverAllSummaryByDistrict'; }
            else if ($scope.FilterType == '2') { Action = 'GetByOverAllSummaryByPldb'; }
            else if ($scope.FilterType == '3') { Action = 'GetByOverAllSummaryByPldbBranch'; }
            else { Action = 'GetByOverAllSummary' };
        }

        $http.get(SiteUrl + "/MIS/GetPldbOtsSchemaSummaryDetails/?districtId=" + ($scope.search.DISTRICT_ID ?? 0) + "&PLDB_ID=" + ($scope.search.PLDB_ID ?? 0) + "&PLDB_Branch_ID=" + ($scope.search.PLDB_Branch_ID ?? 0) + "&Type=" + Action).
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.GetPldbOtsSchemaSummary != null) {
                    $scope.GetPldbOtsSchemaList = JSON.parse(data.Result.Data.GetPldbOtsSchemaSummary);
                    if ($scope.GetPldbOtsSchemaList.length == 0) { $scope.isNoRecord = true; } else { $scope.isNoRecord = false; }
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
        //$scope.FilterType = 0;
        $scope.GetPldbOtsPendencySummaryDetails();
    }
    $scope.ReportDownload = function () {
        let csv = '';
        const rows = document.querySelectorAll("#" + $scope.PLDBOtsReport + " tr");
        let maxCols = 0;

        rows.forEach(row => {
            const style = window.getComputedStyle(row);
            if (!row.classList.contains('ng-hide') && style.display !== 'none') {
                const visibleCols = Array.from(row.querySelectorAll("th, td")).filter(col => {
                    const cellStyle = window.getComputedStyle(col);
                    return !col.classList.contains('ng-hide') && cellStyle.display !== 'none';
                });
                if (visibleCols.length > maxCols) maxCols = visibleCols.length;
            }
        });

        rows.forEach(row => {
            const style = window.getComputedStyle(row);
            if (row.classList.contains('ng-hide') || style.display === 'none') {
                return;
            }

            const allCols = row.querySelectorAll("th, td");
            const rowData = [];

            let visibleCellCount = 0;

            for (let i = 0; i < allCols.length; i++) {
                const col = allCols[i];
                const cellStyle = window.getComputedStyle(col);

                if (col.classList.contains('ng-hide') || cellStyle.display === 'none') {
                    continue;
                }

                let cellText = col.innerText != null ? col.innerText.trim() : '';
                rowData.push('"' + cellText.replace(/"/g, '""') + '"');
                visibleCellCount++;
            }
            while (visibleCellCount < maxCols) {
                rowData.push('""');
                visibleCellCount++;
            }
            csv += rowData.join(",") + '\n';
        });

        const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = $scope.PLDBOtsReport + '.csv';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    };

    $scope.BackToPage = function () {
        window.location.reload();
    }
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

