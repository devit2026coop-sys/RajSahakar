
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

    $scope.divClass = false;
    $scope.IsPldbcalled = true;
    $scope.GetPldbOtsSummary = [];
    $scope.GetPldbOtsSummaryDetail = [];
    //$scope.GetOtsAmountSummary = [];
    //$scope.PldbTotalAmount = "0.00";
    //$scope.TotalAmountSummary = "0.00";
    $scope.PLDBList = [];
    $scope.PLDBBranches = [];
    //$scope.Iscolspan = 1;
    $scope.isNoRecord = false;
    $scope.PLDBOtsRepaymentReport = 'PLDB-OTS-Repayment-Report';
    $scope.PLDBOtsRepaymentSummaryReport = 'PLDB-OTS-Repayment-Summary-Report';

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
                    $scope.search.DISTRICT_ID = $("#hdnDistrictId").val();
                    $scope.search.PLDB_ID = "0";
                    $scope.search.PLDB_Branch_ID = "0";
                    $("#DISTRICT_ID").attr("disabled", "disabled");
                    $scope.PLDBListData();
                }

                $scope.GetPldbOtsSummaryList();
            }).error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });

    };
    $scope.PLDBListData = function () {
        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0";
        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0";
        $scope.PLDBList = [];
        $scope.PLDBBranches = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=" + ($scope.search.DISTRICT_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBList = data;
                if ($scope.search.DISTRICT_ID > 0) {
                    $scope.ReportType = 2;
                }
                $(".se-pre-con").fadeOut("slow");

                if ($("#hdnPldbId").val() != "0") {
                    $scope.search.PLDB_ID = $("#hdnPldbId").val();
                    $scope.PLDBBranchList();
                    $("#PLDB_ID").attr("disabled", "disabled");
                }

                $scope.divClass = false;
                $scope.IsPldbcalled = true;
                $scope.GetPldbOtsSummaryList();
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
                if ($scope.search.DISTRICT_ID > 0 && $scope.search.PLDB_ID > 0) {
                    $scope.ReportType = 3;
                    //$scope.Iscolspan = 3
                }
                if ($("#hdnPldbBranchId").val() != "0") {
                    $scope.search.PLDB_Branch_ID = $("#hdnPldbBranchId").val();
                    $("#PLDB_Branch_ID").attr("disabled", "disabled");
                }
                $scope.GetPldbOtsSummaryList();
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.GetPldbOtsSummaryList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.GetPldbOtsSummary = [];
        $scope.GetOtsAmountSummary = [];
        $scope.GetPldbOtsSummaryDetail = [];
        if ($("#hdnDistrictId").val() != '0') { $scope.search.DISTRICT_ID = $("#hdnDistrictId").val(); $scope.ReportType = 2; }
        if ($("#hdnPldbId").val() != '0') { $scope.search.PLDB_ID = $("#hdnPldbId").val(); $scope.ReportType = 3; /*$scope.Iscolspan = 3*/ }
        if ($("#hdnPldbBranchId").val() != '0') { $scope.search.PLDB_Branch_ID = $("#hdnPldbBranchId").val(); }
        if ($scope.search.DISTRICT_ID > 0 && $scope.search.PLDB_ID > 0 && $scope.search.PLDB_Branch_ID > 0) {
            $scope.ReportType = 4; $scope.divClass = false; $scope.IsPldbcalled = true; /*$scope.Iscolspan = 5;*/
        }
        if ($scope.search.DISTRICT_ID == 0 && $scope.search.PLDB_ID == 0 && $scope.search.PLDB_Branch_ID == 0) {
            $scope.ReportType = 1; $scope.search.PLDB_ID = "0"; $scope.search.PLDB_Branch_ID = "0"; $scope.PLDBList = []; $scope.PLDBBranches = [];
        }
        var Action = "";

        if ($scope.ReportType == '1') { Action = 'GetPldbOtsDistrictWiseSummary'; $scope.colspan = 3; }
        else if ($scope.ReportType == '2') { Action = 'GetOtsByPldb'; $scope.colspan = 3; }
        else if ($scope.ReportType == '3') { Action = 'GetOtsBypldbBranch'; }
        else if ($scope.ReportType == '4') { Action = 'PldbOtsSummaryRpt'; $scope.colspan = 7; }

        $http.get(SiteUrl + "/MIS/GetPldbOtsSummary/?districtId=" + ($scope.search.DISTRICT_ID ?? 0) + "&PLDB_ID=" + ($scope.search.PLDB_ID ?? 0) + "&PLDB_Branch_ID=" + ($scope.search.PLDB_Branch_ID ?? 0) + "&Type=" + Action).
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.GetPldbOtsSummary != null) {
                    $scope.GetPldbOtsSummary = JSON.parse(data.Result.Data.GetPldbOtsSummary);
                    //$scope.GetOtsAmountSummary = JSON.parse(data.Result.Data.GetPldbTotalAmount);
                    if ($scope.GetPldbOtsSummary.length == 0) { $scope.isNoRecord = true; } else { $scope.isNoRecord = false; }
                    //if ($scope.GetOtsAmountSummary != null && $scope.GetPldbOtsSummary != null) { $scope.PldbTotalAmount = $scope.GetOtsAmountSummary[0].SubTotalAmount; }
                    $scope.colsort = "";
                    $scope.reverseSort = false;
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetPldbOtsSummaryDetails = function (item) {
        $(".se-pre-con").fadeIn("slow");
        $scope.GetPldbOtsSummaryDetail = [];
        $scope.GetOtsAmountSummary = [];

        $http.get(SiteUrl + "/MIS/GetPldbOtsSummaryDetail/?OtsId=" + item).
            success(function (data, status) {
                $scope.divClass = true;
                $scope.IsPldbcalled = false;
                if (data.Result.Data != null && data.Result.Data.GetPldbOtsSummaryDetail != null) {
                    $scope.GetPldbOtsSummaryDetail = JSON.parse(data.Result.Data.GetPldbOtsSummaryDetail);
                    //$scope.GetOtsAmountSummary = JSON.parse(data.Result.Data.OtsTotalAmountSummary);
                    //if ($scope.GetOtsAmountSummary != null && $scope.GetPldbOtsSummaryDetail != null) {
                    //$scope.TotalAmountSummary = $scope.GetOtsAmountSummary[0].SubTotalAmount;
                    //}
                    $scope.colsort = "";
                    $scope.reverseSort = false;
                }

                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    //Download Excel
    $scope.ReportDownload = function () {
        let csv = '';
        const rows = document.querySelectorAll("#" + $scope.PLDBOtsRepaymentReport + " tr");
        let maxCols = 0;

        rows.forEach(row => {
            const style = window.getComputedStyle(row);
            if (!row.classList.contains('ng-hide') && style.display !== 'none') {
                const visibleCols = Array.from(row.querySelectorAll("th, td")).filter(col => {
                    const cellStyle = window.getComputedStyle(col); return !col.classList.contains('ng-hide') && cellStyle.display !== 'none';
                });
                if (visibleCols.length > maxCols) maxCols = visibleCols.length;
            }
        });

        rows.forEach(row => {
            const style = window.getComputedStyle(row);
            if (row.classList.contains('ng-hide') || style.display === 'none') { return; }
            if ($scope.search.PLDB_ID > 0 || $scope.search.PLDB_Branch_ID > 0) {
                if (row.classList.contains('ng-hide') || style.display === 'none') { return; }
            }
            else {
                if (row.classList.contains('ng-hide') || style.display === 'none') { return; } //|| row.closest('tfoot') !== null
            }
            const allCols = row.querySelectorAll("th, td");
            const rowData = [];
            let visibleCellCount = 0;

            for (let i = 0; i < allCols.length; i++) {
                const col = allCols[i];
                const cellStyle = window.getComputedStyle(col);

                if (col.classList.contains('ng-hide') || cellStyle.display === 'none') { continue; }
                let cellText = col.innerText != null ? col.innerText.trim().replace(/[^\dA-Za-z\s.,]/g, '') : '';
                cellText = cellText.replace(/"/g, '""');
                rowData.push('"' + cellText + '"');
                visibleCellCount++;
            }
            while (visibleCellCount < maxCols) { rowData.push('""'); visibleCellCount++; }
            csv += rowData.join(",") + '\n';
        });

        const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = $scope.PLDBOtsRepaymentReport + '.csv';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    };


    //Download Excel
    $scope.ReportSummaryDownload = function () {
        let csv = '';
        const rows = document.querySelectorAll("#" + $scope.PLDBOtsRepaymentSummaryReport + " tr");
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
            //if (row.classList.contains('ng-hide') || style.display === 'none') { return; }
            //row.closest('tfoot') !== null
            //if (row.classList.contains('ng-hide') || style.display === 'none' || row.closest('tfoot') !== null) { return; }

            const allCols = row.querySelectorAll("th, td");
            const rowData = [];

            let visibleCellCount = 0;

            for (let i = 0; i < allCols.length; i++) {
                const col = allCols[i];
                const cellStyle = window.getComputedStyle(col);

                if (col.classList.contains('ng-hide') || cellStyle.display === 'none') { continue; }
                let cellText = col.innerText != null ? col.innerText.trim().replace(/[^\dA-Za-z\s.,]/g, '') : '';
                cellText = cellText.replace(/"/g, '""');
                rowData.push('"' + cellText + '"');
                visibleCellCount++;
            }

            while (visibleCellCount < maxCols) { rowData.push('""'); visibleCellCount++; }
            csv += rowData.join(",") + '\n';
        });

        // Step 3: Append Total Amount from <tfoot>
        //const tfoot = document.querySelector("#" + $scope.PLDBOtsRepaymentSummaryReport + " tfoot");
        //if (tfoot) {
        //    const totalRow = tfoot.querySelector("tr");
        //    if (totalRow && totalRow.style.display !== 'none' && !totalRow.classList.contains('ng-hide')) {
        //        const cols = totalRow.querySelectorAll("th, td");
        //        let label = '', value = '';

        //        for (let i = 0; i < cols.length; i++) {
        //            const cell = cols[i];
        //            const cellStyle = window.getComputedStyle(cell);
        //            if (cellStyle.display === 'none' || cell.classList.contains('ng-hide')) continue;

        //            const text = cell.innerText.trim();
        //            if (!label && text.toLowerCase().includes("total")) {
        //                label = text.replace(/"/g, '""');
        //            } else if (!value && /[\d.,]/.test(text)) {
        //                value = text.replace(/[^\d.,]/g, '');
        //            }
        //        }

        //        if (label && value) {
        //            const colspan = 2;

        //            const footerRow = [];
        //            footerRow.push(`"${label}"`);
        //            for (let i = 1; i < colspan; i++) {
        //                footerRow.push('""');
        //            }
        //            footerRow.push(`"${value}"`);
        //            for (let i = footerRow.length; i < maxCols; i++) {
        //                footerRow.push('""');
        //            }
        //            csv += footerRow.join(",") + '\n';
        //        }
        //    }
        //}

        const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = $scope.PLDBOtsRepaymentSummaryReport + '.csv';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    };

    $scope.ClearFilter = function () {
        $scope.search.DISTRICT_ID = "0";
        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0";
        $scope.GetPldbOtsSummaryList();
        $scope.divClass = false;
        $scope.IsPldbcalled = true;
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

