var DailyPriceController = angular.module("DailyPriceReportCnt", ['angularUtils.directives.dirPagination']);
DailyPriceController.controller("DailyPriceReportMod", function ($scope, $http) {

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

    //START : Get ShowSearchData
    $scope.ShowSearchData = function () {
        $(".se-pre-con").fadeIn("slow");

        var msg = '';
        if ($('#txtFromDate').val() === '') {
            msg += _language === 'en-us' ? 'Please select Start Date.' : 'कृपया प्रारंभ तिथि चुनें।';
        }

        if ($('#txtToDate').val() === '') {
            msg += _language === 'en-us' ? 'Please select End Date.' : 'कृपया अंतिम तिथि चुनें।';
        }


        if ($scope.HqOrder === null || $scope.HqOrder.ProductId === null || $scope.HqOrder.ProductId === '0') {
           msg+=  _language === 'en-us' ? 'Please select Product from list.' : 'कृपया सूची से उत्पाद का चयन करें।';
        }

        if (msg.length > 1) {
            ShowModelAlert($scope.translation.StockTransferSaleOrder, msg);
            $(".se-pre-con").fadeOut("slow");
            return;
        }

        var params = { "ProductId": $scope.HqOrder.ProductId, "FromDate": $('#txtFromDate').val(), "ToDate": $('#txtToDate').val() };

        $http.post(SiteUrl + "/Stock/DailyPriceReportMIS", params).
            success(function (data, status) {
                if (data.Result === false) {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.SearchData = [];
                    $scope.divSearchResult = false;
                    ShowModelAlert($scope.translation.StockTransferSaleOrder, _language === 'en-us' ? 'No data found' : 'कोई डेटा नहीं मिला', function () { });
                }
                else {
                    $scope.SearchData = data.data;
                    $scope.divSearchResult = true;
                    $(".se-pre-con").fadeOut("slow");
                }
                console.log(data.data);
            }).
            error(function (data1, status) {
                $scope.SearchData = [];
                $scope.divSearchResult = false;
                $(".se-pre-con").fadeOut("slow");
            });
    };
    //END : Get ShowSearchData

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
        $('#txtFromDate').val('');
        $('#txtToDate').val('');
        $scope.HqOrder.ProductId = '0';
        $scope.SearchData = [];
        $scope.divSearchResult = false;
    };
    //END : Clear Form

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.ProductsList();
    // END : On Page Load
});
