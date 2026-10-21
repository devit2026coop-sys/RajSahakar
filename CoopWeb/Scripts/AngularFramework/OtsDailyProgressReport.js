var MasterMod = angular.module('otsMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("otsCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.search = {};
    $scope.DistrictList = [];
    $scope.PLDBList = [];
    $scope.PLDBBranches = [];
    $scope.search.LoanDistrictId = '0';
    $scope.ReportType = 1;
    $scope.AmountType = 1;
    $scope.otsDPR = [];
    $scope.search.DISTRICT_ID = 0;

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
    };
    $scope.today = new Date();
    $scope.search.ReportDate = new Date($scope.today);
    $scope.GetDistrictList = function () {
        $scope.DistrictList = [];
        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0";
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0") {
                    $scope.search.DISTRICT_ID = $("#hdnDistrictId").val();
                    $scope.search.PLDB_ID = "0"; $scope.search.PLDB_Branch_ID = "0";
                    $("#DISTRICT_ID").attr("disabled", "disabled");
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
                $scope.GetOtsDailyProgressReport();
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
                $scope.GetOtsDailyProgressReport();

            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetOtsDailyProgressReport = function () {
        $(".se-pre-con").fadeIn("slow");
        var dateObj = new Date(($scope.search.ReportDate ?? $scope.today));
        $scope.FormattedReportDate = dateObj.getFullYear() + '-' + (dateObj.getMonth() + 1) + '-' + dateObj.getDate();
        $http.get(SiteUrl + "/MIS/GetOtsDailyProgressReport/?reportType=" + $scope.ReportType + "&pldbId=" + ($scope.search.PLDB_ID ?? 0) + "&pldbBranchId=" + ($scope.search.PLDB_Branch_ID ?? 0) + "&reportDate=" + $scope.FormattedReportDate +"&reportNo=1").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.otsDPR != null) {
                    $scope.otsDPR = JSON.parse(data.Result.Data.otsDPR);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.ReportDownload = function () {
        const originalTable = document.getElementById('dprTable');
        const clonedTable = originalTable.cloneNode(true);

        Array.from(clonedTable.querySelectorAll('th, td')).forEach(cell => {
            const style = window.getComputedStyle(cell);
            
            if (style.display === 'none' || cell.classList.contains('ng-hide')) {
                cell.remove();
            }
            if ($(cell).find('span.ng-hide').length) {
                $(cell).find('span').remove();
            }
            const links = cell.querySelectorAll('a');
            links.forEach(link => {
                const textNode = document.createTextNode(link.textContent);
                link.replaceWith(textNode);
            });
        });

        const wb = XLSX.utils.table_to_book(clonedTable, { sheet: "Report" });
        //wb.Sheets["Report"]['!merges'] = $scope.getExcelMerges($scope.ReportType);
        var dateObj = new Date(($scope.search.ReportDate ?? $scope.today));
        XLSX.writeFile(wb, "DPR-Report-" + (dateObj.getDate() + '-' + (dateObj.getMonth() + 1) + '-' + dateObj.getFullYear())+".xlsx");
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