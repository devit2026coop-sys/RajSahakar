var TilamSangh_InitialStockReportController = angular.module("TilamSangh_InitialStockReportCnt", ['angularUtils.directives.dirPagination']);
TilamSangh_InitialStockReportController.controller("TilamSangh_InitialStockReportMod", function ($scope, $http) {

    $scope.divSearchResult = false;

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

    // START : Get Product List
    $scope.ProductList = function () {
        $http.post(SiteUrl + "/Stock/ProductList", { "ProductId": "0" }).
            success(function (data, status) {
                $scope.Products = data;
            }).
            error(function (data1, status) {
                $scope.Products = [];
            });
    };
    // END : Get Product List

    // START : Get Plant List
    $scope.GetPlantList = function () {
        $http.post(SiteUrl + "/Stock/ProcessingUnitList", { "ProcessingUnitId": "0" }).
            success(function (data, status) {
                $scope.Plants = data.filter(p => (p.IsOperating === 1 || p.IsOperating === '1' || p.IsOperating === true || p.IsOperating === 'True') && (p.IsActive === 1 || p.IsActive === '1' || p.IsActive === true || p.IsActive === 'True'));
            }).
            error(function (data1, status) {
                $scope.Plants = [];
            });
    };
    // END : Get Plant List

    // START : Get Depot List
    $scope.GetDepotList = function () {
        $http.post(SiteUrl + "/Stock/DepotList", { "DepotId": "0" }).
            success(function (data, status) {
                $scope.Depots = data.filter(p => p.IsActive === 1 || p.IsActive === '1' || p.IsActive === true || p.IsActive === 'True');
            }).
            error(function (data1, status) {
                $scope.Depots = [];
            });
    };
    // END : Get Depot List

    // START : Show Search Data
    $scope.ShowSearchData = function () {

        $(".se-pre-con").fadeIn("slow");

        if ($scope.InitialStockRpt === null || $scope.InitialStockRpt.ProductId === null || $scope.InitialStockRpt.ProductId === '0') {
            ShowModelAlert($scope.translation.InitialStockReport, _language === 'en-us' ? 'Please select Product from list.' : 'कृपया सूची से उत्पाद का चयन करें।');
            $(".se-pre-con").fadeOut("slow");
            return;
        }

        $http.post(SiteUrl + "/Stock/ShowInitialStockDataMIS", {
            "ProductId": $scope.InitialStockRpt.ProductId,
            "IsPlant": $scope.InitialStockRpt.Type === '1' ? true : false,
            "PlantDepotId": $scope.InitialStockRpt.Type === '1' ? $scope.InitialStockRpt.PlantId : $scope.InitialStockRpt.DepotId,
            "ShowAll": $scope.InitialStockRpt.Type === '1' ? true : false
        }).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.divSearchResult = true;
                    $scope.SearchData = data;
                    $(".se-pre-con").fadeOut("slow");
                }
                else {
                    $scope.divSearchResult = false;
                    $scope.SearchData = [];
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.InitialStockReport, _language === 'en-us' ? 'No Record found.' : 'कोई रिकॉर्ड नहीं मिला।');
                }
            }).
            error(function (data1, status) {
                $scope.SearchData = [];
            });
    };
    // END : Show Search Data

    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.divSearchResult = false;
        $scope.InitialStockRpt.ProductId = '0';
        $scope.InitialStockRpt.PlantId = '0';
        $scope.InitialStockRpt.DepotId = '0';
    };
    //END : Clear Form

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.GetPlantList();
    $scope.GetDepotList();
    $scope.ProductList();
    // END : On Page Load
});