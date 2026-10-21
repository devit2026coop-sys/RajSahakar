
var MasterMod = angular.module('ncdDistrictMod', ['angularUtils.directives.dirPagination']);

MasterMod.controller("ncdDistrictCtrl", function ($scope, $http) {
    $scope._language = 'en-us';
    $scope.isNoRecord = false;
    $scope.colspan = 0;
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.search = {
        reportType: "1",
        actionType: "District Wise Count"
    };
    $scope.ncdDataList = [];

    if ($("#hdnDistrictId").val() != null && $("#hdnDistrictId").val() != "0") {
        $scope.DISTRICT_ID = $("#hdnDistrictId").val();
    }
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
        $scope.ReportTypeFilter();
        $scope.GetNCDDistrictWiseReport($scope.search.reportType);
    };
    $scope.GetNCDDistrictWiseReport = function (type) {
        $scope.ncdDataList = [];
        $(".se-pre-con").fadeIn("slow");
        var data = $scope.reportTypeList.filter(x => x.reportType == type);
        $scope.search.reportType = data[0].reportType;
        $scope.search.actionType = data[0].actionType;

        $http.get(SiteUrl + "/NcdData/GetDistrictWiseNCDRegistrationCount?type=" + $scope.search.reportType + "&actionType=" + $scope.search.actionType).then(function (response) {
            $(".se-pre-con").fadeOut("slow");
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null) {
                $scope.ncdDataList = JSON.parse(response.data.Result.Data.ncdDataReport);
                // Pagination
                $scope.currentPage = 1;
                $scope.totalRecords = $scope.ncdDataList.length;
                $scope.totalPages = Math.ceil($scope.totalRecords / $scope.pageSize);
                $scope.isNoRecord = $scope.totalRecords === 0;
            } else {
                $scope.isNoRecord = true;
                $scope.colspan = 3;
                $scope.ncdDataList = [];
                $scope.currentPage = 1;
                $scope.totalRecords = 0;
                $scope.totalPages = 0;
            }
        }, function (error) {
            console.error("Error loading Area Of Operations:", error);
            $scope.ncdDataList = [];
            $scope.currentPage = 1;
            $scope.totalRecords = 0;
            $scope.totalPages = 0;
            $scope.isNoRecord = true;
            $(".se-pre-con").fadeOut("slow");
        });
    };
    $scope.ReportTypeFilter = function () {
        $scope.reportTypeList = [
            {
                reportType: "1",
                actionType: "District Wise Count"
            },
            {
                reportType: "2",
                actionType: "District Wise Detail"
            }
        ];
    };
    $scope.getPagedData = function () {
        var start = ($scope.currentPage - 1) * $scope.pageSize;
        var end = start + $scope.pageSize;
        return $scope.ncdDataList.slice(start, end);
    };
    $scope.getPages = function () {
        var pages = [];
        var startPage = Math.floor(($scope.currentPage - 1) / 5) * 5 + 1;
        var endPage = Math.min(startPage + 4, $scope.totalPages);
        for (var i = startPage; i <= endPage; i++) {
            pages.push(i);
        }
        return pages;
    };
    $scope.goToPage = function (page) {
        if (page >= 1 && page <= $scope.totalPages) {
            $scope.currentPage = page;
        }
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.totalPages) {
            $scope.currentPage++;
        }
    };
    $scope.previousPage = function () {
        if ($scope.currentPage > 1) {
            $scope.currentPage--;
        }
    };

    $scope.ClearSearch = function () {
        $scope.search.reportType = "1";
        $scope.search.actionType = "District Wise Count";
        $scope.GetNCDDistrictWiseReport($scope.search.reportType);
    };

    $scope.OnPageLoad();
});