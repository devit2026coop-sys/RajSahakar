var TilamSanghInitialStockDepotController = angular.module("TilamSanghInitialStockDepotCnt", ['angularUtils.directives.dirPagination']);
TilamSanghInitialStockDepotController.controller("TilamSanghInitialStockDepotMod", function ($scope, $http) {

    $scope.divStockEntry = false;

    $scope.divSearch = true;
    $scope.divInsert = false;
    $scope.divSearchData = false;

    //START : ShowInsertBlock
    $scope.ShowInsertBlock = function () {
        $scope.ProductIdForSearch = '0';
        $scope.InitialStock.ProductId = '0';
        $scope.divSearch = false;
        $scope.divInsert = true;
    };
    //END : ShowInsertBlock

    //START : Back
    $scope.Back = function () {
        $scope.ProductIdForSearch = '0';
        $scope.InitialStock.ProductId = '0';
        $scope.divSearch = true;
        $scope.divInsert = false;
    };
    //END : Back 

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


    //START : OnProcessingUnitChange
    $scope.OnProcessingUnitChange = function () {
        $scope.InitialStock.ProductId = '0';
        $scope.divStockEntry = false;
    };
    //END : OnProcessingUnitChange

    //START : Back
    $scope.Back = function () {
        $scope.InitialStock.ProcessingUnitId = '0';
        $scope.InitialStock.ProductId = '0';
        $scope.divStockEntry = false;
    };
    //END : OnProcessingUnitChange


    //START : Show Initial Stock For Product
    $scope.GetInitialStockData = function () {

        if ($scope.ProductIdForSearch === '0') {
            ShowModelAlert($scope.translation.InitialStockDepot, $scope._language === 'en-us' ? 'Please select product from list.' : 'कृपया सूची से उत्पाद का चयन करें', function () { });
            return;
        }

        $http.post(SiteUrl + "/Stock/ShowInitialStockData", { "ProductId": $scope.ProductIdForSearch, "ShowAll": false, "IsPlant": false }).
            success(function (data, status) {

                $scope.InitialStockDataShow = data;
                $scope.divSearchData = true;

                //if (data.length < 1) {

                //}
                //else {
                //    $scope.divSearchData = false;
                //    ShowModelAlert($scope.translation.InitialStockDepot, $scope._language === 'en-us' ? 'No Data found.' : 'कोई डेटा नहीं मिला।', function () { })
                //}


            }).
            error(function (data1, status) {
                $scope.InitialStockData = [];
            });
    };
    //END : Show Initial Stock For Product

    //START : Get Initial Stock Entry Form Data
    $scope.GetInitialStockEntryForm = function () {

        $http.post(SiteUrl + "/Stock/CaptureInitialStockByProductId", { "ProductId": $scope.InitialStock.ProductId, "ShowAll": false }).
            success(function (data, status) {
                $scope.InitialStockData = data;
                $scope.divStockEntry = true;
            }).
            error(function (data1, status) {
                $scope.InitialStockData = [];
            });
    };
    //END : Get Initial Stock Entry Form Data

    //START : Insert Initial Stock Data
    $scope.SaveInitialStock = function () {

        $scope.InitialStockData["ProductId"] = $scope.InitialStock.ProductId;
        $scope.InitialStockData["IsPlant"] = false;

        $http.post(SiteUrl + "/Stock/InsertInitialStock", { "model": $scope.InitialStockData }).
            success(function (data, status) {

                if (parseInt(data, 10) > 0) {
                    ShowModelAlert($scope.translation.InitialStockDepot, _language === 'en-us' ? 'Initial stock entry saved successfully.' : 'प्रारंभिक स्टॉक प्रविष्टि सफलतापूर्वक सहेजी गई।');
                }
                else if (parseInt(data, 10) === -1) {
                    ShowModelAlert($scope.translation.InitialStockDepot, _language === 'en-us' ? 'Error!!! No Action Performed.' : 'त्रुटि !!! कोई कार्रवाई नहीं की गई।');
                }
                else if (parseInt(data, 10) === -2) {
                    ShowModelAlert($scope.translation.InitialStockDepot, _language === 'en-us' ? 'Initial stock already exists. No action performed.' : 'प्रारंभिक स्टॉक पहले से मौजूद है। कोई कार्रवाई नहीं की गई।');
                }
                else {
                    ShowModelAlert($scope.translation.InitialStockDepot, _language === 'en-us' ? 'Error!! Contact to Administrator.' : 'त्रुटि !! व्यवस्थापक से संपर्क करें।');
                }
            }).
            error(function (data1, status) {
                $scope.InitialStockData = [];
            });
    };
    //END : Insert Initial Stock Data

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.ProductList();
    // END : On Page Load
});