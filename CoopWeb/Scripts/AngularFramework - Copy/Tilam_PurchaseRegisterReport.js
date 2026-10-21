var TilamSangh_PurchaseRegisterReportController = angular.module("TilamSangh_PurchaseRegisterReportCnt", ['angularUtils.directives.dirPagination']);
TilamSangh_PurchaseRegisterReportController.controller("TilamSangh_PurchaseRegisterReportMod", function ($scope, $http) {

    $scope.divSearchResult = false;
    $scope.PurchaseRegisterRpt = [];

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

    // START : Get Product Sub Category List
    $scope.ProductSubCategoryList = function () {
        $http.post(SiteUrl + "/Stock/ProductSubCategoryList", { "ProductSubCategoryId": "0", "ProductId": $scope.PurchaseRegisterRpt.ProductId }).
            success(function (data, status) {
                $scope.ProductSubCategories = data;
            }).
            error(function (data1, status) {
                $scope.ProductSubCategories = [];
            });
    };
    // END : Get Product Sub Category List


    // START : Show Search Data
    $scope.ShowSearchData = function () {

        $(".se-pre-con").fadeIn("slow");

        if ($scope.PurchaseRegisterRpt === null || $scope.PurchaseRegisterRpt.ProductId === null || $scope.PurchaseRegisterRpt.ProductId === '0') {
            ShowModelAlert($scope.translation.PurchaseRegisterReport, _language === 'en-us' ? 'Please select Product from list.' : 'कृपया सूची से उत्पाद का चयन करें।');
            $(".se-pre-con").fadeOut("slow");
            return;
        }

        var inputparams = { "ProductId": $scope.PurchaseRegisterRpt.ProductId, ProductSubCategoryId: $scope.PurchaseRegisterRpt.ProductSubCategoryId, PlantId: $scope.PurchaseRegisterRpt.PlantId, ContractNo: $scope.PurchaseRegisterRpt.SaleContractNo };

        $http.post(SiteUrl + "/Stock/ShowPurchaseRegisterDataMIS", inputparams).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.divSearchResult = true;
                    $scope.SearchData = data;
                    console.log(data);
                    $(".se-pre-con").fadeOut("slow");
                }
                else {
                    $scope.divSearchResult = false;
                    $scope.SearchData = [];
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.PurchaseRegisterReport, _language === 'en-us' ? 'No Record found.' : 'कोई रिकॉर्ड नहीं मिला।');
                }
            }).
            error(function (data1, status) {
                $scope.PurchaseOrders = [];
            });
    };
    // END : Show Search Data

    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.divSearchResult = false;
        $scope.PurchaseRegisterRpt.ProductId = '0';
        $scope.PurchaseRegisterRpt.PlantId = '0';
        $scope.ProductSubCategories = [];
        $scope.PurchaseRegisterRpt.ProductSubCategoryId = '0';
        $scope.PurchaseRegisterRpt.SaleContractNo = '';
    };
    //END : Clear Form

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.GetPlantList();
    $scope.ProductList();
    // END : On Page Load
});