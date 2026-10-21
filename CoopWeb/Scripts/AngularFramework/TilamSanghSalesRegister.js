var TilamSanghSalesRegisterController = angular.module("TilamSanghSalesRegisterCnt", ['angularUtils.directives.dirPagination']);
TilamSanghSalesRegisterController.controller("TilamSanghSalesRegisterMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridProcessingUnit = true;
    $scope.divInsertProcessingUnit = false;
    $scope.IsShow = false;

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
            dateFormat: "dd/mm/yy",
            minDate: '0'
        });
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

    //START : Back
    $scope.Back = function () {
        $scope.ProductId = '0';
        $('#txtSellingdate').val('');
        $scope.IsShow = false;
    };
    //END : Back

    $scope.pageChangeHandler = function (num) {
    };

    //START : Get Initial Stock Entry Form Data
    $scope.GetSalesEntryForm = function () {
        $http.post(SiteUrl + "/Stock/GetSalesRegisterDataForDepot", { "ProductId": $scope.ProductId }).
            success(function (data, status) {
                $scope.SalesEntryData = data;
                $scope.IsShow = true;
                console.log(data);
            }).
            error(function (data1, status) {
                $scope.SalesEntryData = [];
                $scope.IsShow = false;
            });
    };
    //END : Get Initial Stock Entry Form Data

    //START : SaveSalesEntry
    $scope.SaveSalesEntry = function () {

        if ($('#txtSellingdate').val().length === 0) {
            ShowModelAlert($scope.translation.ProductSaleByDepot, _language === 'en-us' ? 'Please select Sales Entry Date.' : 'कृपया बिक्री दिनांक चुनें।', function () { });
            return;
        }

        $scope.SalesEntryData["ProductId"] = $scope.ProductId;
        $scope.SalesEntryData["SalesDate"] = $('#txtSellingdate').val();

        $http.post(SiteUrl + "/Stock/InsertStockSales", { "model": $scope.SalesEntryData }).
            success(function (data, status) {

                if (parseInt(data, 10) > 0) {
                    ShowModelAlert($scope.translation.ProductSaleByDepot, _language === 'en-us' ? 'Record(s) saved successfully.' : 'रिकॉर्ड सफलतापूर्वक सहेजा गया');
                    $scope.Back();
                }
                else if (parseInt(data, 10) === -3) {
                    ShowModelAlert($scope.translation.ProductSaleByDepot, _language === 'en-us' ? 'sales quantity can not be greater than available quantity.Please correct the sales quantity' : 'बिक्री की मात्रा उपलब्ध मात्रा से अधिक नहीं हो सकती। कृपया बिक्री की मात्रा को सही करें।');
                }
                else if (parseInt(data, 10) === -4) {
                    ShowModelAlert($scope.translation.ProductSaleByDepot, _language === 'en-us' ? 'Quantity of each item is zero. Hence no record inserted.' : 'प्रत्येक आइटम की मात्रा शून्य है। इसलिए कोई रिकॉर्ड सहेजा नहीं गया।।');
                }
                else if (parseInt(data, 10) === -1) {
                    ShowModelAlert($scope.translation.ProductSaleByDepot, _language === 'en-us' ? 'Error!!! No Action Performed.' : 'त्रुटि !!! कोई कार्रवाई नहीं की गई।');
                }
                else if (parseInt(data, 10) === -2) {
                    ShowModelAlert($scope.translation.ProductSaleByDepot, _language === 'en-us' ? 'Duplicate entry. No action performed.' : 'डुप्लिकेट प्रविष्टि । कोई कार्रवाई नहीं की गई।');
                }
                else {
                    ShowModelAlert($scope.translation.ProductSaleByDepot, _language === 'en-us' ? 'Error!! Contact to Administrator.' : 'त्रुटि !! व्यवस्थापक से संपर्क करें।');
                }
            }).
            error(function (data1, status) {
                $scope.InitialStockData = [];
            });
    };
    //END : SaveSalesEntry

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.ProductList();
    // END : On Page Load
});