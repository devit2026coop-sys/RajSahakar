
function ConvertToBoolean(operater) {
    if (operater === 'True' || operater === 'true' || operater === true) {
        return true;
    }
    else {
        return false;
    }
}



var ProcessingUnitController = angular.module("ProcessingUnitCnt", ['angularUtils.directives.dirPagination']);
ProcessingUnitController.controller("ProcessingUnitMod", function ($scope, $http) {

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
    $scope.ProcessingUnitList = function () {
        $http.post(SiteUrl + "/Stock/ProcessingUnitList", { "ProcessingUnitId": "0" }).
            success(function (data, status) {
                $scope.ProcessingUnits = data;
                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort === column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort === column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                    }
                    return '';
                };
            }).
            error(function (data1, status) {
                $scope.ProcessingUnits = [];
            });
    };
    // END : Get Data List

    //START : Clear Form
    $scope.ClearProcessingUnitForm = function () {
        $scope.ProcessingUnit = [];
        $scope.ProcessingUnit.ProcessingUnitId = '0';
        $scope.ProcessingUnit.DistrictId = '0';
        $scope.ProcessingUnit.ProcessingUnitNameEng = '';
        $scope.ProcessingUnit.ProcessingUnitNameHi = '';
        $scope.ProcessingUnit.IsOperating = null;
    };
    //END : Clear Form

    //START : Show Div For Processing Unit Insert
    $scope.ShowDivProcessingUnitInsert = function () {
        $scope.ClearProcessingUnitForm();
        $scope.GridProcessingUnit = false;
        $scope.divInsertProcessingUnit = true;
    };
    //END : Show Div For Processing Unit Insert

    //START : Back To List
    $scope.BackToProcessingUnitList = function () {
        $scope.ClearProcessingUnitForm();
        $scope.GridProcessingUnit = true;
        $scope.divInsertProcessingUnit = false;
    };
    //END : Back To List

    //START : Validate Form
    $scope.ValidateProcessingUnit = function () {
        var message = '';
        if ($scope.ProcessingUnit === undefined || $scope.ProcessingUnit.ProcessingUnitNameEng.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Plant Name (in Eng).' : '\n <br /> कृपया प्लांट नाम (अंग्रेजी) दर्ज करें |';
        }
        if ($scope.ProcessingUnit === undefined || $scope.ProcessingUnit.ProcessingUnitNameHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Plant Name (in Hindi)' : '\n <br /> कृपया प्लांट नाम (हिंदी) दर्ज करें  |';
        }
        if ($scope.ProcessingUnit === undefined || $scope.ProcessingUnit.DistrictId === undefined || $scope.ProcessingUnit.DistrictId === '0') {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select District' : '\n <br /> कृपया जिला चुनें|';
        }

        if (message.length > 5) {

            ShowModelAlert($scope.translation.ProcessingUnit, message);
            return false;
        }
        return true;
    };
    //END : Validate Form

    //START : Save/Edit Data
    $scope.InsertProcessingUnit = function () {
        $(".se-pre-con").fadeIn("slow");

        if (!$scope.ValidateProcessingUnit()) {
            $(".se-pre-con").fadeOut("slow");
            return false;
        }

        var model = { ProcessingUnitId: $scope.ProcessingUnit.ProcessingUnitId, DistrictId: $scope.ProcessingUnit.DistrictId, ProcessingUnitNameEng: $scope.ProcessingUnit.ProcessingUnitNameEng, ProcessingUnitNameHi: $scope.ProcessingUnit.ProcessingUnitNameHi, IsOperating: ConvertToBoolean($scope.ProcessingUnit.IsOperating) };

        $http.post(SiteUrl + "/Stock/ProcessingUnitInsertUpdate/", { "model": model }).
            success(function (data, status) {
                if (parseInt(data, 10) > 0) {
                    $(".se-pre-con").fadeOut("slow");
                    alert(_language === 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।');
                    $scope.ProcessingUnitList();
                    $scope.BackToProcessingUnitList();
                }
                else if (parseInt(data, 10) === -1) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.ProcessingUnit, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else if (parseInt(data, 10) === -2) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.ProcessingUnit, _language === 'en-us' ? 'Duplicate data. No action performed.' : 'डुप्लिकेट डेटा। कोई कार्रवाई नहीं की गई।', function () { });
                }
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    //END : Save/Edit Data

    //START : Edit
    $scope.Edit = function (item) {
        var ProcessingUnit = [];
        ProcessingUnit["ProcessingUnitId"] = item.ProcessingUnitId;
        ProcessingUnit["DistrictId"] = item.DistrictId;
        ProcessingUnit["ProcessingUnitNameEng"] = item.ProcessingUnitNameEng;
        ProcessingUnit["ProcessingUnitNameHi"] = item.ProcessingUnitNameHi;
        ProcessingUnit["IsOperating"] = ConvertToBoolean(item.IsOperating);
        $scope.ProcessingUnit = ProcessingUnit;
        $scope.GridProcessingUnit = false;
        $scope.divInsertProcessingUnit = true;
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

        $http.post(SiteUrl + "/Stock/ProcessingUnitSetStatus/", { "ProcessingUnitId": Id, "ActionId": ActionId }).
            success(function (data, status) {
                if (data === -1) {
                    ShowModelAlert($scope.translation.ProcessingUnit, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else {
                    $scope.ProcessingUnitList();
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
    $scope.DistrictList();
    $scope.ProcessingUnitList();
    // END : On Page Load
});

var ProductController = angular.module("ProductCnt", ['angularUtils.directives.dirPagination']);
ProductController.controller("ProductMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridProducts = true;
    $scope.divInsertProduct = false;

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

    // START : Get Procust List
    $scope.ProductsList = function () {
        $http.post(SiteUrl + "/Stock/ProductList", { "ProductId": "0" }).
            success(function (data, status) {
                $scope.Products = data;
                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort === column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort === column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                    }
                    return '';
                };
            }).
            error(function (data1, status) {
                $scope.Products = [];
            });
    };
    // END : Get Procust List

    //START : Clear Form
    $scope.Clear = function () {
        $scope.Product = [];
        $scope.Product.ProductId = '0';
        $scope.Product.ProductNameEng = '';
        $scope.Product.ProductNameHi = '';
    };
    //END : Clear Form

    //START : Show Div For Processing Unit Insert
    $scope.ShowDivProductInsert = function () {
        $scope.GridProducts = false;
        $scope.divInsertProduct = true;
    };
    //END : Show Div For Processing Unit Insert

    //START : Back To List
    $scope.BackToList = function () {
        $scope.Clear();
        $scope.GridProducts = true;
        $scope.divInsertProduct = false;
    };
    //END : Back To List

    //START : Validate Form
    $scope.ValidateForm = function () {
        var message = '';
        if ($scope.Product === undefined || $scope.Product.ProductNameEng.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Product Name (in Eng).' : '\n <br /> कृपया उत्पाद नाम (अंग्रेजी) दर्ज करें |';
        }
        if ($scope.Product === undefined || $scope.Product.ProductNameHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Product Name (in Hindi)' : '\n <br /> कृपया उत्पाद नाम (हिंदी) दर्ज करें  |';
        }

        if (message.length > 5) {

            ShowModelAlert($scope.translation.Product, message);
            return false;
        }
        return true;
    };
    //END : Validate Form

    //START : Save/Edit
    $scope.InsertProduct = function () {
        $(".se-pre-con").fadeIn("slow");

        if (!$scope.ValidateForm()) {
            $(".se-pre-con").fadeOut("slow");
            return false;
        }

        var model = { "ProductId": $scope.Product.ProductId, "ProductNameEng": $scope.Product.ProductNameEng, "ProductNameHi": $scope.Product.ProductNameHi };
        $http.post(SiteUrl + "/Stock/ProductInsertUpdate/", { "model": model }).
            success(function (data, status) {
                if (parseInt(data, 10) > 0) {
                    $(".se-pre-con").fadeOut("slow");
                    alert($scope._language === 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।');
                    $scope.ProductsList();
                    $scope.BackToList();
                }
                else if (parseInt(data, 10) === -1) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.Product, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else if (parseInt(data, 10) === -2) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.Product, _language === 'en-us' ? 'Duplicate data. No action performed.' : 'डुप्लिकेट डेटा। कोई कार्रवाई नहीं की गई।', function () { });
                }
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });

        $(".se-pre-con").fadeOut("slow");
    };
    //END : Save/Edit

    $scope.pageChangeHandler = function (num) {
    };

    //START : Edit
    $scope.Edit = function (item) {
        var Product = [];
        Product["ProductId"] = item.ProductId;
        Product["ProductNameEng"] = item.ProductNameEng;
        Product["ProductNameHi"] = item.ProductNameHi;
        $scope.Product = Product;
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

        $http.post(SiteUrl + "/Stock/ProductSetStatus/", { "ProductId": Id, "ActionId": ActionId }).
            success(function (data, status) {
                if (data === -1) {
                    ShowModelAlert($scope.translation.ProcessingUnit, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else {
                    $scope.ProductsList();
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
    $scope.ProductsList();
    // END : On Page Load
});

var ProductSubCategoryController = angular.module("ProductSubCategoryCnt", ['angularUtils.directives.dirPagination']);
ProductSubCategoryController.controller("ProductSubCategoryMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridProducts = true;
    $scope.divInsertProduct = false;
    $scope.ProductSubCategory = [];

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

    // START : Get Procust List
    $scope.ProductSubCategoryList = function () {
        $http.post(SiteUrl + "/Stock/ProductSubCategoryList", { "ProductSubCategoryId": "0", "ProductId": "1" }).
            success(function (data, status) {
                $scope.ProductSubCategories = data;
                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort === column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort === column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                    }
                    return '';
                };
            }).
            error(function (data1, status) {
                $scope.ProductSubCategories = [];
            });
    };
    // END : Get Procust List

    //START : Clear Form
    $scope.Clear = function () {
        $scope.ProductSubCategory.ProductSubCategoryNameEng = '';
        $scope.ProductSubCategory.ProductSubCategoryNameHi = '';
        $scope.ProductSubCategory.ProductId = '0';
        $scope.ProductSubCategory.ProductSubCategoryId = '0';
    };
    //END : Clear Form

    //START : Show Div For Processing Unit Insert
    $scope.ShowDivProductInsert = function () {
        $scope.GridProducts = false;
        $scope.divInsertProduct = true;
    };
    //END : Show Div For Processing Unit Insert

    //START : Back List
    $scope.BackToList = function () {
        $scope.ShowDivProductInsert();
        $scope.GridProducts = true;
        $scope.divInsertProduct = false;
        $scope.Clear();
    };
    //END : Back To List

    //START : Validate Form
    $scope.ValidateForm = function () {
        var message = '';

        if ($scope.ProductSubCategory.ProductId < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please select product' : '\n <br /> कृपया उत्पाद का चयन करें|';
        }

        if ($scope.ProductSubCategory.ProductSubCategoryNameEng === undefined || $scope.ProductSubCategory.ProductSubCategoryNameEng.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please enter product sub category Name (in Eng).' : '\n <br />कृपया उत्पाद उप श्रेणी नाम (अंग्रेजी) प्रदान करें |';
        }
        if ($scope.ProductSubCategory.ProductSubCategoryNameHi === undefined || $scope.ProductSubCategory.ProductSubCategoryNameHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter product sub category Name (in Hindi)' : '\n <br /> कृपया उत्पाद उप श्रेणी नाम (हिंदी) प्रदान करें |';
        }

        if (message.length > 5) {
            ShowModelAlert($scope.translation.ProductSubCategory, message);
            return false;
        }
        return true;
    };
    //END : Validate Form

    //START : Save/Edit
    $scope.Insert = function () {
        $(".se-pre-con").fadeIn("slow");

        if (!$scope.ValidateForm()) {
            $(".se-pre-con").fadeOut("slow");
            return false;
        }

        var model = { "ProductSubCategoryId": $scope.ProductSubCategory.ProductSubCategoryId, "ProductId": $scope.ProductSubCategory.ProductId, "ProductSubCategoryNameEng": $scope.ProductSubCategory.ProductSubCategoryNameEng, "ProductSubCategoryNameHi": $scope.ProductSubCategory.ProductSubCategoryNameHi };
        $http.post(SiteUrl + "/Stock/ProductSubCategoryInsertUpdate/", { "model": model }).
            success(function (data, status) {
                if (parseInt(data, 10) > 0) {
                    $(".se-pre-con").fadeOut("slow");
                    alert($scope._language === 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।');
                    $scope.ProductSubCategoryList();
                    $scope.BackToList();
                }
                else if (parseInt(data, 10) === -1) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.ProductSubCategory, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else if (parseInt(data, 10) === -2) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.ProductSubCategory, _language === 'en-us' ? 'Duplicate data. No action performed.' : 'डुप्लिकेट डेटा। कोई कार्रवाई नहीं की गई।', function () { });
                }
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });

        $(".se-pre-con").fadeOut("slow");
    };
    //END : Save/Edit



    //START : Edit
    $scope.Edit = function (item) {
        var ProductSubCategory = [];
        ProductSubCategory["ProductSubCategoryId"] = item.ProductSubCategoryId;
        ProductSubCategory["ProductId"] = item.ProductId;
        ProductSubCategory["ProductSubCategoryNameEng"] = item.ProductSubCategoryNameEng;
        ProductSubCategory["ProductSubCategoryNameHi"] = item.ProductSubCategoryNameHi;
        $scope.ProductSubCategory = ProductSubCategory;
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

        $http.post(SiteUrl + "/Stock/ProductSubCategorySetStatus/", { "ProductSubCategoryId": Id, "ActionId": ActionId }).
            success(function (data, status) {
                if (data === -1) {
                    ShowModelAlert($scope.translation.ProductSubCategory, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else {
                    $scope.ProductSubCategoryList();
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
    $scope.ProductsList();
    $scope.ProductSubCategoryList();
    // END : On Page Load
});

var UnitMasterController = angular.module("UnitMasterCnt", ['angularUtils.directives.dirPagination']);
UnitMasterController.controller("UnitMasterMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridProducts = true;
    $scope.divInsertProduct = false;
    $scope.Unit = [];

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

    // START : Get Procust List
    $scope.UnitList = function () {
        $http.post(SiteUrl + "/Stock/UnitMasterList", { "UnitId": "0" }).
            success(function (data, status) {
                $scope.Units = data;
                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort === column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort === column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                    }
                    return '';
                };
            }).
            error(function (data1, status) {
                $scope.Units = [];
            });
    };
    // END : Get Procust List

    //START : Clear Form
    $scope.Clear = function () {
        $scope.Unit.UnitNameEng = '';
        $scope.Unit.UnitNameHi = '';
        $scope.Unit.UnitId = '0';
        $scope.Unit.UnitConvertMultiplier = '';
    };
    //END : Clear Form

    //START : Show
    $scope.ShowDivProductInsert = function () {
        $scope.GridProducts = false;
        $scope.divInsertProduct = true;
        $scope.Clear();
    };
    //END : Show 

    //START : Back To List
    $scope.BackToList = function () {
        $scope.ShowDivProductInsert();
        $scope.GridProducts = true;
        $scope.divInsertProduct = false;
        $scope.Clear();
    };
    //END : Back To List

    //START : Validate  Form
    $scope.Validate = function () {
        var message = '';
        if ($scope.Unit.UnitNameEng === undefined || $scope.Unit.UnitNameEng.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Unit Name (in Eng).' : '\n <br /> कृपया इकाई नाम (अंग्रेजी) दर्ज करें |';
        }
        if ($scope.Unit.UnitNameHi === undefined || $scope.Unit.UnitNameHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Unit Name (in Hindi)' : '\n <br /> कृपया इकाई नाम (हिंदी) दर्ज करें  |';
        }
        if ($scope.Unit.UnitConvertMultiplier === undefined || $scope.Unit.UnitConvertMultiplier.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Unit Convertor Coefficient' : '\n <br /> कृपया यूनिट कन्वर्ट गुणांक दर्ज करें  |';
        }

        if (message.length > 5) {

            ShowModelAlert($scope.translation.UnitMaster, message);
            return false;
        }
        return true;
    };
    //END : Validate Form

    //START : Save/Edit
    $scope.Insert = function () {
        $(".se-pre-con").fadeIn("slow");

        if (!$scope.Validate()) {
            $(".se-pre-con").fadeOut("slow");
            return false;
        }

        var model = { "UnitId": $scope.Unit.UnitId, "UnitConvertMultiplier": $scope.Unit.UnitConvertMultiplier, "UnitNameEng": $scope.Unit.UnitNameEng, "UnitNameHi": $scope.Unit.UnitNameHi };
        $http.post(SiteUrl + "/Stock/UnitMasterInsertUpdate/", { "model": model }).
            success(function (data, status) {
                if (parseInt(data, 10) > 0) {
                    $(".se-pre-con").fadeOut("slow");
                    alert($scope._language === 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।');
                    $scope.UnitList();
                    $scope.BackToList();
                }
                else if (parseInt(data, 10) === -1) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.UnitMaster, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else if (parseInt(data, 10) === -2) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.UnitMaster, _language === 'en-us' ? 'Duplicate data. No action performed.' : 'डुप्लिकेट डेटा। कोई कार्रवाई नहीं की गई।', function () { });
                }
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });

        $(".se-pre-con").fadeOut("slow");
    };
    //END : Save/Edit

    //START : Edit
    $scope.Edit = function (item) {
        var Unit = [];
        Unit["UnitId"] = item.UnitId;
        Unit["UnitConvertMultiplier"] = item.UnitConvertMultiplier;
        Unit["UnitNameEng"] = item.UnitNameEng;
        Unit["UnitNameHi"] = item.UnitNameHi;
        $scope.Unit = Unit;
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

        $http.post(SiteUrl + "/Stock/UnitMasterSetStatus/", { "UnitId": Id, "ActionId": ActionId }).
            success(function (data, status) {
                if (data === -1) {
                    ShowModelAlert($scope.translation.UnitMaster, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else {
                    $scope.UnitList();
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
    $scope.UnitList();
    // END : On Page Load
});

var PackingController = angular.module("PackingCnt", ['angularUtils.directives.dirPagination']);
PackingController.controller("PackingMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridProducts = true;
    $scope.divInsertProduct = false;
    $scope.Packing = [];

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

    // START : Get Packing List
    $scope.PackingList = function () {
        $http.post(SiteUrl + "/Stock/PackingList", { "PackingId": "0", "ProductId": "1" }).
            success(function (data, status) {
                $scope.Packings = data;
                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort === column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort === column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                    }
                    return '';
                };
            }).
            error(function (data1, status) {
                $scope.Packings = [];
            });
    };
    // END : Get Packing List

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
        $scope.Packing.PackingNameEng = '';
        $scope.Packing.PackingNameHi = '';
        $scope.Packing.UnitId = '0';
        $scope.Packing.MeasurementCount = '';
        $scope.Packing.ProductId = '0';
        $scope.Packing.PackingId = '0';
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

        if (parseInt($scope.Packing.ProductId, 10) < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please select Product' : '\n <br /> कृपया उत्पाद का चयन करें |';
        }

        if ($scope.Packing.PackingNameEng === undefined || $scope.Packing.PackingNameEng.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Packing Name (in Eng).' : '\n <br /> कृपया पैकिंग नाम (अंग्रेजी) दर्ज करें |';
        }
        if ($scope.Packing.PackingNameHi === undefined || $scope.Packing.PackingNameHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Packing Name (in Hindi)' : '\n <br /> कृपया पैकिंग नाम (हिंदी) दर्ज करें  |';
        }

        if (parseInt($scope.Packing.UnitId, 10) < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please select unit' : '\n <br /> कृपया इकाई चुनें|';
        }

        if ($scope.Packing.MeasurementCount === undefined || $scope.Packing.MeasurementCount.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please enter measurement unit' : '\n <br /> कृपया मापन इकाई दर्ज करें |';
        }

        if (message.length > 5) {

            ShowModelAlert($scope.translation.PackagingMaster, message);
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
            "PackingId": $scope.Packing.PackingId, "ProductId": $scope.Packing.ProductId,
            "UnitId": $scope.Packing.UnitId, "MeasurementCount": $scope.Packing.MeasurementCount,
            "PackingNameEng": $scope.Packing.PackingNameEng, "PackingNameHi": $scope.Packing.PackingNameHi
        };

        $http.post(SiteUrl + "/Stock/PackingInsertUpdate/", { "model": model }).
            success(function (data, status) {
                if (parseInt(data, 10) > 0) {
                    $(".se-pre-con").fadeOut("slow");
                    alert($scope._language === 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।');
                    $scope.PackingList();
                    $scope.ShowDivProductInsert();
                }
                else if (parseInt(data, 10) === -1) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.PackagingMaster, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else if (parseInt(data, 10) === -2) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.PackagingMaster, _language === 'en-us' ? 'Duplicate data. No action performed.' : 'डुप्लिकेट डेटा। कोई कार्रवाई नहीं की गई।', function () { });
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
        var Packing = [];
        Packing["PackingId"] = item.PackingId;
        Packing["ProductId"] = item.ProductId;
        Packing["PackingNameEng"] = item.PackingNameEng;
        Packing["PackingNameHi"] = item.PackingNameHi;
        Packing["UnitId"] = item.UnitId;
        Packing["MeasurementCount"] = item.MeasurementCount;
        $scope.Packing = Packing;
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

        $http.post(SiteUrl + "/Stock/PackingSetStatus/", { "PackingId": Id, "ActionId": ActionId }).
            success(function (data, status) {
                if (data === -1) {
                    ShowModelAlert($scope.translation.PackagingMaster, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else {
                    $scope.PackingList();
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
    $scope.PackingList();
    $scope.ProductsList();
    $scope.UnitList();
    // END : On Page Load
});

var RejectionReasonController = angular.module("RejectionReasonCnt", ['angularUtils.directives.dirPagination']);
RejectionReasonController.controller("RejectionReasonMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridProducts = true;
    $scope.divInsertProduct = false;
    $scope.RejectReason = [];

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

    // START : Get List
    $scope.RejectionReasonList = function () {
        $http.post(SiteUrl + "/Stock/RejectionReasonList").
            success(function (data, status) {
                $scope.RejectionReasons = data;
                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort === column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort === column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                    }
                    return '';
                };
            }).
            error(function (data1, status) {
                $scope.RejectionReasons = [];
            });
    };
    // END : Get List

    //START : Clear Form
    $scope.Clear = function () {
        $scope.RejectReason.RejectionReasonHi = '';
        $scope.RejectReason.RejectionReasonEng = '';
        $scope.RejectReason.RejectionId = '0';
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
        $scope.Clear();
    };
    //END : Back List

    //START : Validate Form
    $scope.Validate = function () {
        var message = '';
        if ($scope.RejectReason.RejectionReasonEng === undefined || $scope.RejectReason.RejectionReasonEng.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Reject Reason (in Eng).' : '\n <br /> कृपया अस्वीकृति का कारण (अंग्रेजी) दर्ज करें |';
        }
        if ($scope.RejectReason.RejectionReasonHi === undefined || $scope.RejectReason.RejectionReasonHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Reject Reason (in Hindi)' : '\n <br /> कृपया Validate (हिंदी) दर्ज करें  |';
        }

        if (message.length > 5) {

            ShowModelAlert($scope.translation.ProcessingUnit, message);
            return false;
        }
        return true;
    };
    //END : Validate Form

    //START : Save/Edit
    $scope.Insert = function () {
        $(".se-pre-con").fadeIn("slow");

        if (!$scope.Validate()) {
            $(".se-pre-con").fadeOut("slow");
            return false;
        }

        var model = { "RejectionId": $scope.RejectReason.RejectionId, "RejectionReasonEng": $scope.RejectReason.RejectionReasonEng, "RejectionReasonHi": $scope.RejectReason.RejectionReasonHi };
        $http.post(SiteUrl + "/Stock/RejectReasonInsertUpdate/", { "model": model }).
            success(function (data, status) {
                if (parseInt(data, 10) > 0) {
                    $(".se-pre-con").fadeOut("slow");
                    alert($scope._language === 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।');
                    $scope.RejectionReasonList();
                    $scope.BackToList();
                }
                else if (parseInt(data, 10) === -1) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.RejectionReasonMaster, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else if (parseInt(data, 10) === -2) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.RejectionReasonMaster, _language === 'en-us' ? 'Duplicate data. No action performed.' : 'डुप्लिकेट डेटा। कोई कार्रवाई नहीं की गई।', function () { });
                }
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });

        $(".se-pre-con").fadeOut("slow");
    };
    //END : Save/Edit

    //START : Edit
    $scope.Edit = function (item) {
        var RejectReason = [];
        RejectReason["RejectionId"] = item.RejectionId;
        RejectReason["RejectionReasonEng"] = item.RejectionReasonEng;
        RejectReason["RejectionReasonHi"] = item.RejectionReasonHi;
        $scope.RejectReason = RejectReason;
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

        $http.post(SiteUrl + "/Stock/RejectReasonSetStatus/", { "RejectionId": Id, "ActionId": ActionId }).
            success(function (data, status) {
                if (data === -1) {
                    ShowModelAlert($scope.translation.RejectionReasonMaster, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else {
                    $scope.RejectionReasonList();
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
    $scope.RejectionReasonList();
    // END : On Page Load
});

var DepotMasterController = angular.module("DepotMasterCnt", ['angularUtils.directives.dirPagination']);
DepotMasterController.controller("DepotMasterMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridDepot = true;
    $scope.divInsertDepot = false;
    $scope.Depot = [];

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
    $scope.DepotList = function () {
        $http.post(SiteUrl + "/Stock/DepotList", { "DepotId": "0" }).
            success(function (data, status) {
                $scope.Depots = data;
                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort === column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort === column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                    }
                    return '';
                };
            }).
            error(function (data1, status) {
                $scope.Depots = [];
            });
    };
    // END : Get Data List

    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.Depot.DepotNameEng = '';
        $scope.Depot.DepotNameHi = '';
        $scope.Depot.AgentNameEng = '';
        $scope.Depot.AgentNameHi = '';
        $scope.Depot.DepotId = '0';
        $scope.Depot.DistrictId = '0';
    };
    //END : Clear Form

    //START : Show Div For Insert
    $scope.ShowDivDepotInsert = function () {
        $scope.ClearForm();
        $scope.GridDepot = false;
        $scope.divInsertDepot = true;
    };
    //END : Show Div For Insert

    //START : Back To List
    $scope.BackToList = function () {
        $scope.ClearForm();
        $scope.GridDepot = true;
        $scope.divInsertDepot = false;
    };
    //END : Back To List

    //START : Validate Form
    $scope.ValidateForm = function () {
        var message = '';
        if ($scope.Depot === undefined || $scope.Depot.DepotNameEng.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Depot Name (in Eng).' : '\n <br /> कृपया डिपो नाम (अंग्रेजी) दर्ज करें |';
        }

        if ($scope.Depot === undefined || $scope.Depot.DepotNameHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Depot Name (in Hindi)' : '\n <br /> कृपया डिपो नाम (हिंदी) दर्ज करें  |';
        }

        if ($scope.Depot === undefined || $scope.Depot.DistrictId === undefined || $scope.Depot.DistrictId === '0') {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select District' : '\n <br /> कृपया जिला चुनें  |';
        }

        if ($scope.Depot === undefined || $scope.Depot.AgentNameEng.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Depot Agent Name (Eng.)' : '\n <br /> कृपया डिपो एजेंट फर्म नाम (अंग्रेजी) दर्ज करें  |';
        }

        if ($scope.Depot === undefined || $scope.Depot.AgentNameHi === undefined || $scope.Depot.AgentNameHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Depot Agent Name (Hindi)' : '\n <br /> कृपया डिपो एजेंट फर्म नाम (हिंदी) दर्ज करें  |';
        }

        if (message.length > 5) {

            ShowModelAlert($scope.translation.Depot, message);
            return false;
        }
        return true;
    };
    //END : Validate Form

    //START : Save/Edit Data
    $scope.InsertDepot = function () {
        $(".se-pre-con").fadeIn("slow");

        if (!$scope.ValidateForm()) {
            $(".se-pre-con").fadeOut("slow");
            return false;
        }

        var model = { DepotId: $scope.Depot.DepotId, DistrictId: $scope.Depot.DistrictId, DepotNameEng: $scope.Depot.DepotNameEng, DepotNameHi: $scope.Depot.DepotNameHi, AgentNameEng: $scope.Depot.AgentNameEng, AgentNameHi: $scope.Depot.AgentNameHi };

        $http.post(SiteUrl + "/Stock/DepotInsertUpdate/", { "model": model }).
            success(function (data, status) {
                if (parseInt(data, 10) > 0) {
                    $(".se-pre-con").fadeOut("slow");
                    alert(_language === 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।');
                    $scope.DepotList();
                    $scope.BackToList();
                }
                else if (parseInt(data, 10) === -1) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.Depot, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else if (parseInt(data, 10) === -2) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.Depot, _language === 'en-us' ? 'Duplicate data. No action performed.' : 'डुप्लिकेट डेटा। कोई कार्रवाई नहीं की गई।', function () { });
                }
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    //END : Save/Edit Data

    //START : Edit
    $scope.Edit = function (item) {
        var Depot = [];
        Depot["DepotId"] = item.DepotId;
        Depot["DistrictId"] = item.DistrictId;
        Depot["DepotNameEng"] = item.DepotNameEng;
        Depot["DepotNameHi"] = item.DepotNameHi;
        Depot["AgentNameEng"] = item.AgentNameEng;
        Depot["AgentNameHi"] = item.AgentNameHi;
        $scope.Depot = Depot;
        $scope.GridDepot = false;
        $scope.divInsertDepot = true;
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

        $http.post(SiteUrl + "/Stock/DepotSetStatus/", { "DepotId": Id, "ActionId": ActionId }).
            success(function (data, status) {
                if (data === -1) {
                    ShowModelAlert($scope.translation.Depot, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else {
                    $scope.DepotList();
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
    $scope.DistrictList();
    $scope.DepotList();
    // END : On Page Load
});

var EncValueController = angular.module("EncValueCnt", ['angularUtils.directives.dirPagination']);
EncValueController.controller("EncValueMod", function ($scope, $http) {

    // START : Page load Function
    $scope.Calculator = [];
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

    // START : Get Encrypted Value
    $scope.GetEncyptedValue = function () {
        $http.post(SiteUrl + "/Stock/CalculateEncValue", { "plaintTextVal": $scope.Calculator.ToBeEncrypt }).
            success(function (data, status) {
                $scope.EncryptedValue = data.Message;
            }).
            error(function (data, status) {
                $scope.EncryptedValue = 'Error';
            });
    };
    // END :  Get Encrypted Value

    // START : Get Decrypted Value
    $scope.GetDecyptedValue = function () {
        $http.post(SiteUrl + "/Stock/CalculateDecryptValue", { "encVal": $scope.Calculator.ToBeDecrypt }).
            success(function (data, status) {
                $scope.DecryptedValue = data.Message;
            }).
            error(function (data, status) {
                $scope.DecryptedValue = 'Error';
            });
    };
    // END :  Get Decrypted Value



    // START : On Page Load
    $scope.OnPageLoad();
    // END : On Page Load
});

var PriceMasterModule = angular.module("PriceMasterMod", ['angularUtils.directives.dirPagination']);
PriceMasterModule.controller("PriceMasterCnt", function ($scope, $http, $rootScope) {
    $scope.divInsert = false;
    $scope.divShowdata = true;

    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 2,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }
    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                //  _currentPageID = 1;
                callback();
            }
            ++indx;
        });
    }
    $rootScope.SetDataPaging = function () {
        if ($rootScope.DataPagingDetails.TotalRecords > 0) {
            $rootScope.DataPagingDetails.TotalPages = parseInt(($rootScope.DataPagingDetails.TotalRecords / $rootScope.DataPagingDetails.PageSize)) + (($rootScope.DataPagingDetails.TotalRecords % $rootScope.DataPagingDetails.PageSize == 0) ? 0 : 1);
            $rootScope.DataPagingDetails.StartRecord = ($rootScope.DataPagingDetails.TotalRecords == 0) ? 0 : ((($rootScope.DataPagingDetails.PageNumber - 1) * $rootScope.DataPagingDetails.PageSize) + 1);
            $rootScope.DataPagingDetails.EndRecord = ($rootScope.DataPagingDetails.TotalPages == $rootScope.DataPagingDetails.PageNumber || $rootScope.DataPagingDetails.TotalRecords == 0) ? $rootScope.DataPagingDetails.TotalRecords : ((($rootScope.DataPagingDetails.PageNumber - 1) * $rootScope.DataPagingDetails.PageSize) + $rootScope.DataPagingDetails.PageSize);


            $rootScope.DataPagingDetails.StartPageNumber = 1;
            $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.TotalPages;
            if ($rootScope.DataPagingDetails.TotalPages > 5 && $rootScope.DataPagingDetails.PageNumber > 0) {
                $rootScope.DataPagingDetails.StartPageNumber = $rootScope.DataPagingDetails.PageNumber == 1 ? 1 : $rootScope.DataPagingDetails.PageNumber - 1;
                $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.StartPageNumber + 4;
                if ($rootScope.DataPagingDetails.EndPageNumber > $rootScope.DataPagingDetails.TotalPages) {
                    $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.TotalPages;
                    $rootScope.DataPagingDetails.StartPageNumber = $rootScope.DataPagingDetails.StartPageNumber > 4 ? $rootScope.DataPagingDetails.EndPageNumber - 4 : $rootScope.DataPagingDetails.StartPageNumber;
                }
            }
        }
        $scope.DataPaging.TotalRecords = $rootScope.DataPagingDetails.TotalRecords
    }
    $rootScope.PagingRange = function (startPage, endPage, totalPages, step) {
        step = step || 1;
        var input = [];

        for (var tStart = startPage; tStart <= endPage && tStart <= totalPages; tStart++) {
            input.push(tStart);
        }
        return input;
    };
    $scope.searchRequest = function (callback) {
        //$scope.DataPaging.searchParameters = [];
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                $scope.DataPaging.currentPageID = 1;
                callback();
            }
            ++indx;
        });
    }
    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];
    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        if ($scope.Filter.FilterType == 'date') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            // $scope.AddCalender();
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }
    $scope.ClearSearch = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.FilterType = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.GetPriceMasterList();
    }
    /*------------End Paging and Filter-----------------*/

    $scope._language = 'en-us';
    $scope._language = _language;
    $scope.MasterModel = {
        PriceId: 0,
        ProductId: "0",
        SubProductId: "0",
        UnitId: "0",
        Quantity: "",
        Price: "",
        Date: "",
    }

    $scope.FinancialYear = []
    $scope.TodayDate = "";
    $scope.CurrentFY = ""
    $scope.GetCurrentDate = function () {
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!

        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        debugger;
        $scope.TodayDate = dd + '/' + mm + '/' + yyyy;
        $scope.MasterModel.FinancialYear = yyyy + '-' + (parseInt(yyyy.toString().substring(2, 4)) + 1);
        $scope.FinancialYear.push({ FinancialYear: $scope.MasterModel.FinancialYear, Id: yyyy });
        for (var i = 1; i <= 5; i++) {
            $scope.FinancialYear.push({ FinancialYear: (yyyy - i) + '-' + (parseInt((yyyy - i).toString().substring(2, 4)) + 1), Id: (yyyy - i) });
        }
    }
    $scope.GetCurrentDate();
    if (_language == 'en-us') {
        $http.get("../Translations/Translation_EN.json").
            success(function (data, status) {
                $scope.translation = data;
            }).
            error(function (data, status) {
                alert("Error while translation");
            });
    }
    else {
        $http.get("../Translations/Translation_HI.json").
            success(function (data, status) {
                $scope.translation = data;
            }).
            error(function (data, status) {
                alert("Error while translation");
            });

        $scope.ClearUnitForm = function () {
            $scope.UnitMaster = null;
        };
    }
    $scope.AddNew = function () {
        $scope.ResetModel();
        $scope.divInsert = true;
        $scope.divShowdata = false;
    };

    $scope.SaveMasterData = function () {

        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Stock/InsertDailyPrice", { "date": $("#txtPriceDate").val(), "model": $scope.PriceData }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                if (data.Result == false) {
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                }
                else {
                    ShowOkMessage("Success", $scope._language == 'en-us' ? "Price successfully submited." : "मूल्य सफलतापूर्वक सबमिट किया गया", function () {
                        window.location.href = "/Stock/PriceMaster";
                    });
                }
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
            });

    }

    $scope.ValidateMaster = function () {

        var message = _language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.MasterModel.ProductId.replace(/ /g, "") == "0") {
            message += _language == 'en-us' ? '\n <br />Please choose Product Name.' : '\n <br />कृपया उत्पाद का नाम चुनें।';
            $("#name_en").addClass("notValid");
        }
        if ($scope.MasterModel.SubProductId.replace(/ /g, "") == "0") {
            message += _language == 'en-us' ? '\n <br />Please choose Sub Product category Name.' : '\n <br />कृपया उप उत्पाद श्रेणी का नाम चुनें।';
            $("#name_hi").addClass("notValid");
        }

        if ($scope.MasterModel.UnitId.replace(/ /g, "") == "0") {
            message += _language == 'en-us' ? '\n <br />Please choose Unit.' : '\n <br />कृपया यूनिट चुनें।';
            $("#name_en").addClass("notValid");
        }
        if ($scope.MasterModel.Quantity.replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please enter quantity.' : '\n <br />कृपया मात्रा दर्ज करें।';
            $("#name_hi").addClass("notValid");
        }
        if ($scope.MasterModel.Price.replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please enter price.' : '\n <br />कृपया मूल्य दर्ज करें।';
            $("#name_hi").addClass("notValid");
        }
        if ($("#PriceDate").val() == "") {
            message += _language == 'en-us' ? '\n <br />Please choose date.' : '\n <br />कृपया तारीख चुनें।';
            $("#name_hi").addClass("notValid");
        }
        if (message.length > 29) {
            ShowModelAlert("Scheme Master", message);
            return false;
        }
        return true;
    }

    $scope.ResetModel = function () {
        $scope.MasterModel = {
            PriceId: 0,
            ProductId: "0",
            SubProductId: "0",
            UnitId: "0",
            Quantity: "",
            Price: "",
            Date: ""
        };
    }
    $scope.PriceList = [];
    $scope.GetPriceMasterList = function () {
        $(".blockUI").fadeIn("slow");
        $scope.PriceList = [];
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/Stock/GetPriceMasterList", JSON.stringify({ Details: jsonParameters })).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                // $scope.OrderList = [];
                $scope.PriceList = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");

            }).
            error(function (data, status) {
                $scope.PriceList = [];
                $(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    };
    $scope.GetPriceMasterList();


    $scope.ProductList = [];
    $scope.GetProductList = function () {
        // $(".blockUI").fadeIn("slow");
        $scope.ProductList = [];
        return $http.post(SiteUrl + "/Stock/ProductList", JSON.stringify({ ProductId: 0 })).
            success(function (data, status) {
                // $(".blockUI").fadeOut("slow");
                // $scope.OrderList = [];
                $scope.ProductList = data;
                // $(".blockUI").fadeOut("slow");

            }).
            error(function (data, status) {
                $scope.ProductList = [];
                //$(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    };
    $scope.GetProductList();







    $scope.EditMaster = function (item) {

        //$("#addModal").modal("show");
        $scope.PriceMaster = [];

        $scope.PriceMaster.ProductId = item.ProductId.toString();
        $('#txtPriceDate').val(item.ForamatedDate);
        $scope.GetPriceList();
        $scope.divInsert = true;
        $scope.divShowdata = false;
        //$scope.MasterModel = {
        //    PriceId: item.price_id,
        //    ProductId: item.ProductId.toString(),
        //    SubProductId: item.sub_product_id.toString(),
        //    UnitId: item.unit_id.toString(),
        //    Quantity: item.quantity.toString(),
        //    Price: item.price,
        //    Date: item.ForamatedDate
        //};
        //$scope.GetSubProductList().then(function () {
        //    $scope.MasterModel.SubProductId = $scope.MasterModel.SubProductId;
        //});

    };

    // START : Get Procust List
    $scope.ProductsList = function () {
        $http.post(SiteUrl + "/Stock/ProductList", { "ProductId": "0" }).
            success(function (data, status) {
                $scope.Products = data;
            }).
            error(function (data1, status) {
                $scope.Products = [];
            });
    };
    // END : Get Procust List

    // START : Get Price List
    $scope.GetPriceList = function () {

        if ($('#txtPriceDate').val().length < 10) {
            ShowModelAlert($scope.translation.PriceMaster, _language === 'en-us' ? 'Please select date first.' : 'कृपया पहले तिथि चुनें।', function () { });
            return;
        }

        $http.post(SiteUrl + "/Stock/GetPriceListByProductAndDateWise", { "ProductId": $scope.PriceMaster.ProductId, "date": $('#txtPriceDate').val() }).
            success(function (data, status) {
                if (data.Result === false) {
                    alert(data.data);
                    $scope.PriceData = [];
                    $scope.divStockEntry = false;
                    return;
                }
                $scope.PriceData = data.data;
                $scope.divStockEntry = true;
                console.log(data.data);
            }).
            error(function (data1, status) {
                $scope.PriceData = [];
                $scope.divStockEntry = false;
            });
    };
    // END : Get Price List

    // START : Go Back
    $scope.Back = function () {
        $scope.divStockEntry = false;
        $scope.PriceData = [];
        $scope.PriceMaster.ProductId = '0';
        $('#txtPriceDate').val('');
        $scope.divInsert = false;
        $scope.divShowdata = true;
    };
    // END : Go Back

    // START : Clear
    $scope.Clear = function () {
        $scope.divStockEntry = false;
        $scope.PriceData = [];
        $scope.PriceMaster.ProductId = '0';
        $('#txtPriceDate').val('');
    };
    // END : Clear


    ///At Page Load
    $scope.ProductsList();
});