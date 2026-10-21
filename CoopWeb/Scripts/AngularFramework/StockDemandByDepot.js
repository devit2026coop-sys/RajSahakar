var StockDemandController = angular.module("StockDemandCnt", ['angularUtils.directives.dirPagination']);
StockDemandController.controller("StockDemandMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridProducts = true;
    $scope.divInsertProduct = false;
    $scope.StockDemand = [];

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

    // START : Get Product Sub List
    $scope.ProductsSubList = function () {
        $http.post(SiteUrl + "/Stock/ProductSubCategoryList", {
            "ProductId": $scope.StockDemand.ProductId,"ProductSubCategoryId":"0" }).
            success(function (data, status) {
                $scope.ProductSubCategies = data.filter(p => (p.IsActive === 'True' || p.IsActive === 'true' || p.IsActive === true));
            }).
            error(function (data1, status) {
                $scope.ProductSubCategies = [];
            });
    };
    // END : Get Product Sub List

    // START : Get Demand List
    $scope.DemandList = function () {
        $http.post(SiteUrl + "/Stock/StockDemandByDepotTilamSanghList", {"DemandId": "0"}).
            success(function (data, status) {
                $scope.Demands = data;
            }).
            error(function (data1, status) {
                $scope.Demands = [];
            });
    };
    // END : Get Demand List

    //START : Clear Form
    $scope.Clear = function () {
        $scope.StockDemand.ProcessingUnitId = '0';
        $scope.StockDemand.ProductId = '0';
        $scope.ProductSubCategies = [];
        $scope.StockDemand.ProductSubCategoryId = '0';
        $scope.StockDemand.UnitId = '0';
        $scope.StockDemand.Quantity = '';
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

        if (parseInt($scope.StockDemand.ProcessingUnitId, 10) < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please select Processing Unit' : '\n <br /> कृपया प्रोसेसिंग यूनिट चुनें |';
        }

        if (parseInt($scope.StockDemand.ProductId, 10) < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please select Product' : '\n <br /> कृपया उत्पाद का चयन करें |';
        }

        if (parseInt($scope.StockDemand.ProductSubCategoryId, 10) < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please select Product Sub Category ' : '\n <br /> कृपया उत्पाद उप श्रेणी का चयन करें |';
        }

        if (parseInt($scope.StockDemand.UnitId, 10) < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please select Unit' : '\n <br /> कृपया इकाई का चयन करें|';
        }

        
        if ($scope.StockDemand.Quantity === undefined || $scope.StockDemand.Quantity.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Quantity' : '\n <br /> कृपया मात्रा दर्ज करें |';
        }

        if (message.length > 5) {

            ShowModelAlert($scope.translation.StockDemandbyDepot, message);
            return false;
        }
        return true;
    };
    //END : Validate Form

    //START : Save/Edit Processing Unit
    $scope.Insert = function () {
        $(".se-pre-con").fadeIn("slow");

        if (!$scope.Validate()) {
            $(".se-pre-con").fadeOut("slow");
            return false;
        }

        var model = {
            "StockDemandId": $scope.StockDemand.StockDemandId, "ProcessingUnitId": $scope.StockDemand.ProcessingUnitId,
            "ProductId": $scope.StockDemand.ProductId, "ProductSubCategoryId": $scope.StockDemand.ProductSubCategoryId,
            "UnitId": $scope.StockDemand.UnitId, "Quantity": $scope.StockDemand.Quantity
        };

        $http.post(SiteUrl + "/Stock/InsertStockDemandByDepot/", { "model": model }).
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
    //END : Save/Edit Processing Unit

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

    //START : Set Status
    $scope.SetStatus = function (Id, ActionId) {
        $(".se-pre-con").fadeIn("slow");

        if (ActionId === 2) {
            var confirmMsg = _language === 'en-us' ? 'Are you sure to delete this record?' : 'क्या आप इस रिकॉर्ड को हटाना सुनिश्चित कर रहे हैं?';

            if (!confirm(confirmMsg)) {
                $(".se-pre-con").fadeOut("slow");
                return false;
            }
        }

        $http.post(SiteUrl + "/Stock/DemandSetStatus/", { "DemandId": Id, "ActionId": ActionId }).
            success(function (data, status) {
                if (data === -1) {
                    ShowModelAlert($scope.translation.StockDemandbyDepot, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else {
                    $scope.DemandList();
                }

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    //END : Set Status

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.ProcessingUnitList();
    $scope.ProductsList();
    $scope.UnitList();
    $scope.DemandList();
    // END : On Page Load
});