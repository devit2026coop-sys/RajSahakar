var RSWC_MIS_Controller = angular.module("RSWC_MIS_Cnt", ['angularUtils.directives.dirPagination']);
RSWC_MIS_Controller.controller("RSWC_MIS_Mod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridResult = true;
    $scope.GridResult = false;
    $scope.RSWCViewList = [];
    $scope.KVSSList = [];
    $scope.PurchaseCenterList = [];
    $scope.RSWC = [];

    // START : Page load Function
    $scope.OnPageLoad = function () {
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
    // END : Page load Function

    // START : Get Financial Year List
    $scope.FinancialYearList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=0").
            success(function (data, status) {
                $scope.FYList = data;
            }).
            error(function (data, status) {
                $scope.FYList = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : Get Financial Year List

    // START : On District Change
    $scope.DistrictChange = function () {
        $scope.GetWarehouseList();
        $scope.GetKVSSList();
        $scope.GetPurchaseCenterList();

        $scope.RSWC.WarehouseId = '0';
        $scope.RSWC.KVSSId = '0';
        $scope.RSWC.PurchaseCenterId = '0';
    };
    // END : On District Change

    // START : Warehouse List
    $scope.GetWarehouseList = function () {
        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/Master/RajfedWarehouseList1", { parameters: [{ "Key": "RWM.DistrictId", "Value": $scope.RSWC.DistrictId }] }).
            success(function (data, status) {
                $scope.Warehouses = data;
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.Warehouses = [];
                $(".blockUI").fadeOut("slow");
            });
    };
    // END : Warehouse List

    // START : KVSS List
    $scope.GetKVSSList = function () {
        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/FertilizerSupply/GetKVSS", { "DistrictId": + $scope.RSWC.DistrictId }).
            success(function (data, status) {
                $scope.KVSSList = data;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.KVSSList = [];
            });
    };
    // END : KVSS List

    // START : GetPurchaseCenterList
    $scope.GetPurchaseCenterList = function () {
        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/FertilizerSupply/GetPurchaseCenterList", { "DistrictId": +$scope.RSWC.DistrictId, "CommodityId": $scope.RSWC.CommodityId }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PurchaseCenterList = data;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PurchaseCenterList = [];
            });
    };
    // END : GetPurchaseCenterList

    // START : Get Commodity List
    $scope.GetCommodityList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=COMMODITY&MasterCode=0").
            success(function (data, status) {
                $scope.Commodities = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Commodities list");
            });
    };
    // END : Get Commodity List

    // START : Get District List
    $scope.DistrictList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=0").
            success(function (data, status) {
                $scope.Districts = data;
            }).
            error(function (data, status) {
                $scope.Districts = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : Get District List

    // START : Get Data List
    $scope.GetMISList = function () {
        var model = [
            { "Key": "cmsp.FYEAR_ID", "Value": $scope.RSWC.FIN_YEAR_MASTR_ID },
            { "Key": "dst.DISTRICT_ID", "Value": $scope.RSWC.DistrictId },
            { "Key": "cm.CommodityId", "Value": $scope.RSWC.CommodityId },
            { "Key": "rs.wharehouse_id", "Value": $scope.RSWC.WarehouseId },
            { "Key": "ss.KvssId", "Value": $scope.RSWC.KVSSId },
            { "Key": "pc.PurchaseCenterId", "Value": $scope.RSWC.PurchaseCenterId }
        ];

        $http.post(SiteUrl + "/MIS/RSWCMIS", { "parameters": model }).
            success(function (data, status) {
                $scope.GridResult = true;
                $scope.RSWCViewList = data;
            }).
            error(function (data1, status) {
                $scope.ProcessingUnits = [];
            });
    };
    // END : Get Data List

    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.ProcessingUnit = [];
        $scope.ProcessingUnit.ProcessingUnitId = '0';
        $scope.ProcessingUnit.DistrictId = '0';
        $scope.ProcessingUnit.ProcessingUnitNameEng = '';
        $scope.ProcessingUnit.ProcessingUnitNameHi = '';
        $scope.ProcessingUnit.IsOperating = null;
    };
    //END : Clear Form

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.FinancialYearList();
    $scope.DistrictList();
    $scope.GetCommodityList();
    // END : On Page Load
});