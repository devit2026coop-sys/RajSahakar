var MasterMod = angular.module('otsMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("otsCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.search = {};
    $scope.ReportType = 1;
    $scope.AmountType = 1;
    $scope.otsDPR = [];
    const urlParams = new URLSearchParams(window.location.search);

    $scope.DistrictId = urlParams.get('did') ?? 0;
    $scope.VidhanSabhaId = urlParams.get('vid') ?? 0;
    $scope.ReportType = urlParams.get('rt') ?? 1;
    $scope.ReportDate = urlParams.get('rd') ?? null;
    $scope.ActionType = urlParams.get('at') ?? 1;
    $scope.ReportNo = urlParams.get('rn') ?? 3;

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
        $scope.GetOtsDailyProgressReport();
    };

    $scope.GetOtsDailyProgressReport = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/MIS/GetOtsDailyProgressViewDetails/?reportType=" + $scope.ReportType + "&actionType=" + $scope.ActionType + "&districtId=" + ($scope.DistrictId ?? 0) + "&vidhanSabhaId=" + ($scope.VidhanSabhaId ?? 0) + "&reportDate=" + ($scope.ReportDate ?? null) + "&reportNo=" + ($scope.ReportNo ?? 3)).
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
        });

        const wb = XLSX.utils.table_to_book(clonedTable, { sheet: "Report" });
        //wb.Sheets["Report"]['!merges'] = $scope.getExcelMerges($scope.ReportType);
        XLSX.writeFile(wb, "OtsDailyProgressReport.xlsx");
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