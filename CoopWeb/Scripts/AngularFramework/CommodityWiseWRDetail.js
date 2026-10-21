var CommodityWiseWRDetailModule = angular.module('CommodityWiseWRDetailMod', ['angularUtils.directives.dirPagination']);
CommodityWiseWRDetailModule.controller("CommodityWiseWRDetailCont", function ($rootScope, $scope, $http) {

    // SCOPE
    //$scope._language = 'en-us';
    $scope.DistrictList = [];
    $scope.Commodities = [];
    $scope.KVSSList = [];
    $scope.PurchaseCenterList = [];

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.currentPage = 1;
    $scope.WrSearch = [];

    // Code for Language label setting
    $scope.getTranslation = function () {
        $scope._language = _language;
        if (_language === 'en-us') {
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
        }
    };
    // Code for Language label setting

    //START : Get District List
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    //END : Get District List

    //START : Get Commodity List
    $scope.GetCommodityList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=COMMODITY&MasterCode=0").
            success(function (data, status) {
                $scope.Commodities = data;
            }).
            error(function (data, status) {
                Console.log('Error while getting commoties');
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
    };
    //END : Get Commodity List

    //START : Get warehouse List
    $scope.GetWarehouseList = function () {
        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/MIS/GetWarehouseList", { DistrictId: $scope.WrSearch.DistrictId }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.WarehouseList = data.Table;
                $scope.GetKVSSList('0');
                $scope.GetPurchaseCenterList('0');
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.WarehouseList = [];
                $(".blockUI").fadeOut("slow");
            });
    };
    //START : Get warehouse List

    //START : Get KVSS List
    $scope.GetKVSSList = function (id) {
        $(".blockUI").fadeIn("slow");
       
        return $http.post(SiteUrl + "/FertilizerSupply/GetKVSS", { "DistrictId": +$scope.WrSearch.DistrictId }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.KVSSList = [];
                $scope.KVSSList = data;
                $scope.GetPurchaseCenterList('0');
                $scope.WrSearch.KVSSId = id;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.KVSSList = [];
            });
    };
    //END : Get KVSS List

    //START : Get Purchase Center List
    $scope.GetPurchaseCenterList = function (id) {
        $(".blockUI").fadeIn("slow");
        $scope.WrSearch["PurchaseCenterId"] = "0";
        return $http.post(SiteUrl + "/FertilizerSupply/GetPurchaseCenterList", { "DistrictId": + $scope.WrSearch.DistrictId, "CommodityId": $scope.WrSearch.CommodityId }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PurchaseCenterList = data;
                $scope.WrSearch.PurchaseCenterId = id;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PurchaseCenterList = [];
            });
    };
    //END : Get KVSS List


    // START : WR Details
    $scope.SearchData = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/MIS/GetWRDetails", { "CommodityId": $scope.WrSearch.CommodityId, "DistrictId": $scope.WrSearch.DistrictId, "WarehouseId": $scope.WrSearch.WarehouseId, "KVSSId": $scope.WrSearch.KVSSId, "PurchaseCenterId": $scope.WrSearch.PurchaseCenterId, "Search": $scope.WrSearch.Search}).
            success(function (data, status) {
                console.log(JSON.parse(data));
                $scope.WRDetails = JSON.parse(data);
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.WRDetails = [];
                console.log('Error while fetching WR Detail data.');
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : WR Details

    // START : On Page List
    $scope.getTranslation();
    $scope.GetCommodityList();
    $scope.GetDistrictList();
    //END : On PAge List

});
