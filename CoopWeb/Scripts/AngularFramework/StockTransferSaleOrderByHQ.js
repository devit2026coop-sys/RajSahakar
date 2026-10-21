var StockTransferSaleOrderController = angular.module("StockTransferSaleOrderCnt", ['angularUtils.directives.dirPagination']);
StockTransferSaleOrderController.controller("StockTransferSaleOrderMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridProducts = true;
    $scope.divInsertProduct = false;
    $scope.IsStockTransfer = true;
    $scope.divPackingDetails = false;
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
            dateFormat: "dd/mm/yy",
            minDate: '0'
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

    //START : Get Initial Stock Entry Form Data
    $scope.GetSalesEntryForm = function () {
        $http.post(SiteUrl + "/Stock/CaptureInitialStockByProductId", { "ProductId": $scope.HqOrder.ProductId }).
            success(function (data, status) {
                $scope.InitialStockData = data;
                $scope.divPackingDetails = true;
            }).
            error(function (data1, status) {
                $scope.InitialStockData = [];
                $scope.divPackingDetails = false;
            });
    };
    //END : Get Initial Stock Entry Form Data

    // START : Change Order Type
    $scope.ChangeOrderType = function () {
        $scope.HqOrder.DepotId = '0';
        $scope.HqOrder.FirmName = '';
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

    // START : Get Unit List
    $scope.UnitList = function () {
        $http.post(SiteUrl + "/Stock/UnitMasterList", { "UnitId": "0" }).
            success(function (data, status) {
                $scope.Units = data.filter(p => (p.IsActive === 'True' || p.IsActive === 'true' || p.IsActive === true || p.IsActive === "1"));
            }).
            error(function (data1, status) {
                $scope.Units = [];
            });
    };
    // END : Get Unit List

    // START : Get StockTransferSalesOrderByHqToPlant List
    $scope.StockTransferSalesOrderByHqToPlant = function () {
        $http.post(SiteUrl + "/Stock/StockTransferSalesOrderByHqToPlantList").
            success(function (data, status) {
                $scope.StockTransferSalesOrderList = data;
            }).
            error(function (data1, status) {
                $scope.StockTransferSalesOrderList = [];
            });
    };
    // END : Get StockTransferSalesOrderByHqToPlant List

    //START : Clear Form
    $scope.Clear = function () {
        $scope.HqOrder.OrderType = '1';
        $scope.HqOrder.PlantId = '0';
        $scope.HqOrder.ProductId = '0';
        $('#txtOrderDate').val('');
        $scope.HqOrder.FirmName = '';
        $scope.HqOrder.DepotId = '0';
        $scope.InitialStockData = [];
        $scope.divPackingDetails = false;
    };
    //END : Clear Form

    //START : Show Div For Processing Unit Insert
    $scope.ShowDivProductInsert = function () {
        $scope.GridProducts = false;
        $scope.divInsertProduct = true;
        $scope.Clear();
    };
    //END : Show Div For Processing Unit Insert

    //START : Back To List
    $scope.BackToList = function () {
        $scope.ShowDivProductInsert();
        $scope.GridProducts = true;
        $scope.divInsertProduct = false;
    };
    //END : Back To List

    //START : Validate Form
    $scope.Validate = function () {
        var message = '';

        //if ($scope.HqOrder.OrderType === undefined) {
        //    message += $scope._language === 'en-us' ? '\n <br /> Please choose Order Type' : '\n <br /> कृपया आदेश प्रकार चुनें |';
        //}

        if (parseInt($scope.HqOrder.PlantId, 10) < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please select Order For Which Plant? ' : '\n <br /> किस प्लांट के लिए ऑर्डर? चुनें |';
        }

        if (parseInt($scope.HqOrder.ProductId, 10) < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please select Product' : '\n <br /> कृपया उत्पाद का चयन करें |';
        }


        if ($('#txtOrderDate').val().length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Choose Order Date' : '\n <br /> कृपया आदेश की तिथि दर्ज करें|';
        }


        if (($scope.HqOrder.FirmName === undefined || $scope.HqOrder.FirmName.length < 1) && $scope.HqOrder.OrderType === '2') {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Firm Name for Sales Order Issued' : '\n <br /> कृपया फर्म नाम जिस हेतु बिक्री आदेश जारी दर्ज करें |';
        }

        if (parseInt($scope.HqOrder.DepotId, 10) < 1 && $scope.HqOrder.OrderType === '1') {
            message += $scope._language === 'en-us' ? '\n <br /> Please select Depot Name ' : '\n <br /> कृपया डिपो का चयन करें |';
        }

        var IsValidQuantity = true;

        angular.forEach($scope.InitialStockData.ProductSubCategoryPackingMappingList, function (i, j) {

            angular.forEach(i.ProductSubCategoryQuantityEntity, function (k, l) {

                if (IsValidQuantity === false) {
                    return;
                }
                else if ($(k.Quantity).length < 1) {
                    IsValidQuantity = false;
                }
            });
        });

        if (IsValidQuantity === false) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Provide valid data for quantity ' : '\n <br /> कृपया मात्रा के लिए मान्य डेटा प्रदान करें |';
        }

        

        if (message.length > 5) {

            ShowModelAlert($scope.translation.StockTransferSaleOrder, message);
            return false;
        }
        return true;
    };
    //END : Validate Form

    //START : Save/Edit Stock Transfer/Sales Order For Plant
    $scope.Insert = function () {
        $(".se-pre-con").fadeIn("slow");

        //if (!$scope.Validate()) {
        //    $(".se-pre-con").fadeOut("slow");
        //    return false;
        //}


        var _details = [];

        angular.forEach($scope.InitialStockData.ProductSubCategoryPackingMappingList, function (i, j) {
            angular.forEach(i.ProductSubCategoryQuantityEntity, function (k, l) {
                _details.push({ "PackagingId": k.PackingId, "Quantity": k.Quantity, "ProductSubCategoryId": k.ProductSubCategoryId });
            });
        });


        var model = {
            "IdStockTransferSaleOrderOnPlantByHQ": '0',
            "IsStockTransfer": $scope.HqOrder.OrderType === '1' ? true : false,
            "PlantId": $scope.HqOrder.PlantId,
            "ProductId": $scope.HqOrder.ProductId,
            "OrderDate": $('#txtOrderDate').val(),
            "FirmName": $scope.HqOrder.FirmName,
            "DepotId": $scope.HqOrder.DepotId,
            "OrderDetails": _details
        };

        $http.post(SiteUrl + "/Stock/InsertStockTransferSaleOrderByHqToPlant/", { "model": model }).
            success(function (data, status) {
                if (parseInt(data, 10) > 0) {
                    $(".se-pre-con").fadeOut("slow");
                    alert($scope._language === 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।');
                    $scope.DemandList();
                    $scope.ShowDivProductInsert();
                    $scope.GridProducts = true;
                    $scope.divInsertProduct = false;
                }
                else if (parseInt(data, 10) === -1) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.StockDemandbyDepot, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else if (parseInt(data, 10) === -2) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.StockDemandbyDepot, _language === 'en-us' ? 'Duplicate data. No action performed.' : 'डुप्लिकेट डेटा। कोई कार्रवाई नहीं की गई।', function () { });
                }
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });

        $(".se-pre-con").fadeOut("slow");
    };
    //END : Save/Edit Stock Transfer/Sales Order For Plant

    //START : Edit
    $scope.Edit = function (item) {
        var StockDemand = [];
        StockDemand["StockDemandId"] = item.StockDemandId;
        StockDemand["ProcessingUnitId"] = item.ProcessingUnitId;
        StockDemand["ProductId"] = item.ProductId;

        //$.ajax({
        //    url: SiteUrl + "/Stock/ProductSubCategoryList",
        //    data: {"ProductId": item.ProductId, "ProductSubCategoryId": "0"},
        //    async: false,
        //    success: function (data) {
        //        //$scope.ProductSubCategies = data;

        //    }
        //});
        StockDemand["ProductSubCategoryId"] = item.ProductSubCategoryId;
        StockDemand["UnitId"] = item.UnitId;
        StockDemand["Quantity"] = item.Quantity;
        $scope.StockDemand = StockDemand;
        $scope.GridProducts = false;
        $scope.divInsertProduct = true;
    };
    //END : Edit

    //START : Get Order Details
    $scope.GetOrderDetails = function (Id, productId) {
        $(".se-pre-con").fadeIn("slow");

        $http.post(SiteUrl + "/Stock/StockTransferSalesOrderDetails/", { "Id": Id, "ProductId": productId }).
            success(function (data, status) {
                $scope.OrderDetails = data;
                console.log(data);
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data1, status) {
                $scope.OrderDetails = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
    };
    //END : Get Order Details

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.ProcessingUnitList();
    $scope.ProductsList();
    $scope.UnitList();
    $scope.StockTransferSalesOrderByHqToPlant();
    $scope.DepotList();
    // END : On Page Load
});


var StockTransferSaleOrderAcceptanceController = angular.module("StockTransferSaleOrderAcceptanceCnt", ['angularUtils.directives.dirPagination']);
StockTransferSaleOrderAcceptanceController.controller("StockTransferSaleOrderAcceptanceMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridProducts = true;
    $scope.divInsertProduct = false;
    $scope.IsStockTransfer = true;
    $scope.divPackingDetails = false;
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
    };
    // END : Page load Function

    // START : Get StockTransferSalesOrderByHqToPlant List
    $scope.StockTransferSalesOrderByHqToPlant = function () {
        $http.post(SiteUrl + "/Stock/StockTransferSalesOrderByHqToPlantList").
            success(function (data, status) {
                $scope.StockTransferSalesOrderList = data;
            }).
            error(function (data1, status) {
                $scope.StockTransferSalesOrderList = [];
            });
    };
    // END : Get StockTransferSalesOrderByHqToPlant List


    //START : Save Acceptance
    $scope.SaveAcceptance = function (Id) {

        var confirmMesage = _language === 'en-us' ? 'Are you sure to confirm delivery of this order?' : 'क्या आप इस आदेश के वितरण की पुष्टि करना सुनिश्चित करते हैं?';

        if (!confirm(confirmMesage)) {
            return;
        }

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Stock/StockTransferSalesOrderDelivery/", { "Id": Id }).
            success(function (data, status) {
                if (parseInt(data, 10) > 0) {
                    $(".se-pre-con").fadeOut("slow");
                    alert($scope._language === 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।');
                    $scope.StockTransferSalesOrderByHqToPlant();
                }
                else if (parseInt(data, 10) === -1) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.StockTransferSalesOrderConfirmation, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else if (parseInt(data, 10) === -2) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.StockTransferSalesOrderConfirmation, _language === 'en-us' ? 'Duplicate data. No action performed.' : 'डुप्लिकेट डेटा। कोई कार्रवाई नहीं की गई।', function () { });
                }
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });

        $(".se-pre-con").fadeOut("slow");
    };
    //END : Save Acceptance

  

    //START : Get Order Details
    $scope.GetOrderDetails = function (Id, productId) {
        $(".se-pre-con").fadeIn("slow");

        $http.post(SiteUrl + "/Stock/StockTransferSalesOrderDetails/", { "Id": Id, "ProductId": productId }).
            success(function (data, status) {
                $scope.OrderDetails = data;
                console.log(data);
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data1, status) {
                $scope.OrderDetails = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
    };
    //END : Get Order Details

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.StockTransferSalesOrderByHqToPlant();
    // END : On Page Load
});