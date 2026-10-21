var StockTransferSaleOrderReportController = angular.module("StockTransferSaleOrderReportCnt", ['angularUtils.directives.dirPagination']);
StockTransferSaleOrderReportController.controller("StockTransferSaleOrderReportMod", function ($scope, $http) {

    $scope.GridProducts = true;
    $scope.divInsertProduct = false;
    $scope.IsStockTransfer = true;
    $scope.divSearchResult = false;
    $scope.StockDemand = [];
    $scope.HqOrder = [];

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

        $('.SetCalender').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy"
        });
    };
    // END : Page load Function

    // START : Get Processing Data List
    $scope.ProcessingUnitList = function () {
        $http.post(SiteUrl + "/Stock/ProcessingUnitList", { "ProcessingUnitId": "0" }).
            success(function (data, status) {
                $scope.ProcessingUnits = data.filter(p => (p.IsOperating === 'True' || p.IsOperating === 'true' || p.IsOperating === true) && (p.IsActive === 'True' || p.IsActive === 'true' || p.IsActive === true));
            }).
            error(function (data1, status) {
                $scope.ProcessingUnits = [];
            });
    };
    // END : Get Processing Data List

    // START : Get Depot List
    $scope.DepotList = function () {
        $http.post(SiteUrl + "/Stock/DepotList").
            success(function (data, status) {
                $scope.Depots = data.filter(p => p.IsActive === 'True' || p.IsActive === 'true' || p.IsActive === true);
            }).
            error(function (data1, status) {
                $scope.Depots = [];
            });
    };
    // END : Get Depot List

    //START : Get ShowSearchData
    $scope.ShowSearchData = function () {
        $(".se-pre-con").fadeIn("slow");

        if ($scope.HqOrder === null || $scope.HqOrder.ProductId === null || $scope.HqOrder.ProductId === '0') {
            ShowModelAlert($scope.translation.StockTransferSaleOrder, _language === 'en-us' ? 'Please select Product from list.' : 'कृपया सूची से उत्पाद का चयन करें।');
            $(".se-pre-con").fadeOut("slow");
            return;
        }

        var params = { "OrderType": $scope.HqOrder.OrderType, "OrderForPlantId": $scope.HqOrder.PlantId, "ProductId": $scope.HqOrder.ProductId, "OrderDate": $('#txtOrderDate').val(), "DepotId": $scope.HqOrder.DepotId, "FirmName": $scope.HqOrder.FirmName };

        $http.post(SiteUrl + "/Stock/StockTransferSalesOrderReportMIS", params).
            success(function (data, status) {
                if (data.length === 0) {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.SearchData = [];
                    $scope.divSearchResult = false;
                    ShowModelAlert($scope.translation.StockTransferSaleOrder, _language === 'en-us' ? 'No data found' : 'कोई डेटा नहीं मिला', function () { });
                }
                else {
                    $scope.SearchData = data;
                    $scope.divSearchResult = true;
                    $(".se-pre-con").fadeOut("slow");
                }
            }).
            error(function (data1, status) {
                $scope.SearchData = [];
                $scope.divSearchResult = false;
                $(".se-pre-con").fadeOut("slow");
            });
    };
    //END : Get ShowSearchData

    // START : Change Order Type
    $scope.ChangeOrderType = function () {
        $scope.HqOrder.DepotId = '0';
        $scope.HqOrder.FirmName = '';
        $scope.divSearchResult = false;
        $scope.SearchData = [];
        var id = parseInt($scope.HqOrder.OrderType);
        if (id === 1) {
            $scope.IsStockTransfer = true;
        }
        else {
            $scope.IsStockTransfer = false;
        }
    };
    // END : Change Order Type

    // START : Get Product List
    $scope.ProductsList = function () {
        $http.post(SiteUrl + "/Stock/ProductList", { "ProductId": "0" }).
            success(function (data, status) {
                $scope.Products = data.filter(p => (p.IsActive === 'True' || p.IsActive === 'true' || p.IsActive === true));
            }).
            error(function (data1, status) {
                $scope.Products = [];
            });
    };
    // END : Get Product List

    //START : Clear Form
    $scope.Clear = function () {
        $scope.HqOrder.OrderType = '1';
        $scope.HqOrder.PlantId = '0';
        $scope.HqOrder.ProductId = '0';
        $('#txtOrderDate').val('');
        $scope.HqOrder.FirmName = '';
        $scope.HqOrder.DepotId = '0';
        $scope.SearchData = [];
        $scope.divSearchResult = false;
    };
    //END : Clear Form

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.ProcessingUnitList();
    $scope.ProductsList();
    $scope.DepotList();
    // END : On Page Load
});
