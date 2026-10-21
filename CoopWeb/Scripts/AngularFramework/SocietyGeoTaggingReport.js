
var MasterMod = angular.module('TaggingMod', ['angularUtils.directives.dirPagination']);

MasterMod.controller("TaggingCtrl", function ($scope, $http) {
    $scope._language = 'en-us';
    $scope.isNoRecord = false;
    $scope.colspan = 0;
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.geoTaggingDataList = [];
    $scope.TotalCooperativeSocieties = 0;
    $scope.GeoTaggingDone = 0;
    $scope.Ratio = 0;

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
        $scope.GetSocietyGeoTaggingDetail();
    };
    $scope.GetSocietyGeoTaggingDetail = function () {
        $scope.geoTaggingDataList = [];
        $scope.geoTaggingData = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Mis/GetSocietyGeoTaggingDetail").then(function (response) {
            $(".se-pre-con").fadeOut("slow");
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null) {
                $scope.geoTaggingData = JSON.parse(response.data.Result.Data.getTaggingCount);
                $scope.geoTaggingDataList = JSON.parse(response.data.Result.Data.geoTaggingData);
                $scope.TotalCooperativeSocieties = $scope.geoTaggingData[0].TotalCooperativeSocieties;
                $scope.GeoTaggingDone = $scope.geoTaggingData[0].GeoTaggingDone;
                $scope.Ratio = $scope.geoTaggingData[0].Ratio;
                // Pagination
                $scope.currentPage = 1;
                $scope.totalRecords = $scope.geoTaggingDataList.length;
                $scope.totalPages = Math.ceil($scope.totalRecords / $scope.pageSize);
                $scope.isNoRecord = $scope.totalRecords === 0;
            } else {
                $scope.isNoRecord = true;
                $scope.colspan = 3;
                $scope.geoTaggingDataList = [];
                $scope.currentPage = 1;
                $scope.totalRecords = 0;
                $scope.totalPages = 0;
            }
        }, function (error) {
            console.error("Error loading data:", error);
            $scope.geoTaggingDataList = [];
            $scope.currentPage = 1;
            $scope.totalRecords = 0;
            $scope.totalPages = 0;
            $scope.isNoRecord = true;
            $(".se-pre-con").fadeOut("slow");
        });
    };
    $scope.getPagedData = function () {
        var start = ($scope.currentPage - 1) * $scope.pageSize;
        var end = start + $scope.pageSize;
        return $scope.geoTaggingDataList.slice(start, end);
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
        $scope.GetSocietyGeoTaggingDetail();
    };

    $scope.OnPageLoad();
});