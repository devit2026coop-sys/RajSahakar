var TilamSanghSalesRegisterController = angular.module("TilamSanghSalesRegisterCnt", ['angularUtils.directives.dirPagination']);
TilamSanghSalesRegisterController.controller("TilamSanghSalesRegisterMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridProcessingUnit = true;
    $scope.divInsertProcessingUnit = false;

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

    // START : Get Processing Unit List
    $scope.ProcessingUnitList = function () {
        $http.post(SiteUrl + "/Stock/ProcessingUnitList", { "ProcessingUnitId": "0" }).
            success(function (data, status) {
                $scope.ProcessingUnits = data;
            }).
            error(function (data1, status) {
                $scope.ProcessingUnits = [];
            });
    };
    // END : Get Processing Unit List

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

    //START : Clear Processing Unit Form
    $scope.ClearProcessingUnitForm = function () {
        $scope.ProcessingUnit.ProcessingUnitNameEng = '';
        $scope.ProcessingUnit.ProcessingUnitNameHi = '';
        $scope.ProcessingUnit.ProcessingUnitId = '0';
        $('input[type=checkbox]').prop('checked', false);
    };
    //END : Clear Processing Unit Form

    //START : Show Div For Processing Unit Insert
    $scope.ShowDivProcessingUnitInsert = function () {
        $scope.GridProcessingUnit = false;
        $scope.divInsertProcessingUnit = true;
    };
    //END : Show Div For Processing Unit Insert

    //START : Back To Processing Unit List
    $scope.BackToProcessingUnitList = function () {
        $scope.ShowDivProcessingUnitInsert();
        $scope.GridProcessingUnit = true;
        $scope.divInsertProcessingUnit = false;
    };
    //END : Back To Processing Unit List

    //START : Validate Processing Unit Form
    $scope.ValidateProcessingUnit = function () {
        var message = '';
        if ($scope.ProcessingUnit === undefined || $scope.ProcessingUnit.ProcessingUnitNameEng.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Processing Unit Name (in Eng).' : '\n <br /> कृपया प्रसंस्करण इकाई नाम (अंग्रेजी) दर्ज करें |';
        }
        if ($scope.ProcessingUnit === undefined || $scope.ProcessingUnit.ProcessingUnitNameHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Processing Unit Name (in Hindi)' : '\n <br /> कृपया प्रसंस्करण इकाई नाम (हिंदी) दर्ज करें  |';
        }

        if (message.length > 5) {

            ShowModelAlert($scope.translation.ProcessingUnit, message);
            return false;
        }
        return true;
    };
    //END : Validate Processing Unit Form

    //START : Save/Edit Processing Unit
    $scope.InsertProcessingUnit = function () {
        $(".se-pre-con").fadeIn("slow");

        if (!$scope.ValidateProcessingUnit()) {
            $(".se-pre-con").fadeOut("slow");
            return false;
        }

        var objProcessingUnit = {};
        $http.post(SiteUrl + "/Society/GetAuditData/", objProcessingUnit).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.RequestForm = true;
                    $(".se-pre-con").fadeOut("slow");
                }
                else {
                    $scope.RequestForm = true;
                    $scope.AuditorRequest.AuditorId = 0;
                    alert((_language === 'en-us' ? 'data1\n' : 'data2\n'));
                    $(".se-pre-con").fadeOut("slow");
                }
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });

        $(".se-pre-con").fadeOut("slow");
    };
    //END : Save/Edit Processing Unit

    $scope.pageChangeHandler = function (num) {
    };

    //START : Set Data For Update
    $scope.SetDataForUpdate = function (item) {
        $scope.showtringdiv = true;
        $scope.Trainning = item;
        $scope.Trainning.TrainningId = item.TrainningId;
        $scope.Trainning.ProgrammeNameEn = item.ProgrammeNameEn;
        $scope.Trainning.IsActive = item.IsActive === 1 ? true : false;
    };
    //END : Set Data For Update

    //START : Get Initial Stock Entry Form Data
    $scope.GetSalesEntryForm = function () {
        $http.post(SiteUrl + "/Stock/CaptureInitialStockByProductId", { "ProductId": $scope.InitialStock.ProductId }).
            success(function (data, status) {
                $scope.InitialStockData = data;
            }).
            error(function (data1, status) {
                $scope.InitialStockData = [];
            });
    };
    //END : Get Initial Stock Entry Form Data

    //START : Insert Initial Stock Data
    $scope.SaveStockSalesEntry = function () {

        $scope.InitialStockData["ProcessingUnitId"] = $scope.InitialStock.ProcessingUnitId;
        $scope.InitialStockData["ProductId"] = $scope.InitialStock.ProductId;

        $http.post(SiteUrl + "/Stock/InsertStockSales", { "model": $scope.InitialStockData }).
            success(function (data, status) {

                if (parseInt(data, 10) > 0) {
                    ShowModelAlert($scope.translation.InitialStock, _language === 'en-us' ? 'Initial stock entry saved successfully.' : 'स्टॉक बिक्री प्रविष्टि सफलतापूर्वक सहेजी गई।');
                }
                else if (parseInt(data, 10) === -1) {
                    ShowModelAlert($scope.translation.InitialStock, _language === 'en-us' ? 'Error!!! No Action Performed.' : 'त्रुटि !!! कोई कार्रवाई नहीं की गई।');
                }
                else if (parseInt(data, 10) === -2) {
                    ShowModelAlert($scope.translation.InitialStock, _language === 'en-us' ? 'Initial stock already exists. No action performed.' : 'स्टॉक बिक्री पहले से मौजूद है। कोई कार्रवाई नहीं की गई।');
                }
                else {
                    ShowModelAlert($scope.translation.InitialStock, _language === 'en-us' ? 'Error!! Contact to Administrator.' : 'त्रुटि !! व्यवस्थापक से संपर्क करें।');
                }
            }).
            error(function (data1, status) {
                $scope.InitialStockData = [];
            });
    };
    //END : Insert Initial Stock Data

    //START : Set Status
    $scope.SetStatus = function (TrainningId, ActionId) {
        $http.post(SiteUrl + "/Trainning/DeleteandstatusTrainning", { Id: TrainningId, Action: ActionId == true ? "1" : "0" }).
            success(function (data, status) {

                $scope.GetAllTrainningList();

            }).
            error(function (data, status) {
            });
    };
    //END : Set Status

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.ProcessingUnitList();
    $scope.ProductList();
    // END : On Page Load
});