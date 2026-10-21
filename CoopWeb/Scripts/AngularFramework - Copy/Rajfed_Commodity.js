var CommodityController = angular.module("CommodityCnt", ['angularUtils.directives.dirPagination']);
CommodityController.controller("CommodityMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridWarehouses = true;
    $scope.divInsert = false;
    $scope.IsDisabled = true;
    $scope.Warehouse = [];

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
  

    // START : Get Data List
    $scope.CommodityList = function () {
        $http.post(SiteUrl + "/Master/CommodityList/", { "CommodityId": "0" }).
            success(function (data, status) {
                $scope.Commodities = data;
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
            error(function () {
                $scope.Warehouses = [];
            });
    };
    // END : Get Data List

    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.Commodity = [];
        $scope.Commodity.CommodityNameEn = '';
        $scope.Commodity.CommodityNameHi = '';
        $scope.Commodity.IsKharif = '1';
        $scope.Commodity.CommodityId = '0';
    };
    //END : Clear Form

    //START : Show Div For Insert / Update
    $scope.ShowDivForInsert = function () {
        $scope.ClearForm();
        $scope.GridWarehouses = false;
        $scope.divInsert = true;
    };
    //END : Show Div For Insert / Update

    //START : Back To List
    $scope.BackToList = function () {
        $scope.ClearForm();
        $scope.GridWarehouses = true;
        $scope.divInsert = false;
    };
    //END : Back To List

    //START : Validate Form
    $scope.ValidateForm = function () {
        var message = '';

        if ($scope.Commodity === undefined || $scope.Commodity.CommodityNameEn === undefined || $scope.Commodity.CommodityNameEn.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Commodity Name (in Eng).' : '\n <br /> कृपया कमोडिटी नाम (अंग्रेज़ी) दर्ज करें |';
        }

        if ($scope.Commodity === undefined || $scope.Commodity.CommodityNameHi === undefined || $scope.Commodity.CommodityNameHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Commodity Name (in Hindi)' : '\n <br /> कृपया कमोडिटी नाम (हिंदी) दर्ज करें  |';
        }

        if ($scope.Commodity.IsKharif === undefined) {
            message += $scope._language === 'en-us' ? '\n <br /> Please select Crop Type' : '\n <br /> कृपया फसल का प्रकार चुने|';
        }

        if (message.length > 5) {

            ShowModelAlert($scope.translation.Commodity, message);
            return false;
        }
        return true;
    };
    //END : Validate Form

    //START : Save/Edit Data
    $scope.Insert = function () {
        $(".se-pre-con").fadeIn("slow");

        if (!$scope.ValidateForm()) {
            $(".se-pre-con").fadeOut("slow");
            return false;
        }

        var model = {
            CommodityId: $scope.Commodity.CommodityId,
            CommodityNameEn: $scope.Commodity.CommodityNameEn,
            CommodityNameHi: $scope.Commodity.CommodityNameHi,
            IsKharif: $scope.Commodity.IsKharif === 1 || $scope.Commodity.IsKharif === '1' ? true : false
        };
        console.log(model);
        $http.post(SiteUrl + "/Master/CommodityInsertUpdate/", { "model": model }).
            success(function (data, status) {
                if (parseInt(data, 10) > 0) {
                    $(".se-pre-con").fadeOut("slow");
                    alert(_language === 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।');
                    $scope.CommodityList();
                    $scope.BackToList();
                }
                else if (parseInt(data, 10) === -1) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.Commodity, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else if (parseInt(data, 10) === -2) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.Commodity, _language === 'en-us' ? 'Duplicate data. No action performed.' : 'डुप्लिकेट डेटा। कोई कार्रवाई नहीं की गई।', function () { });
                }
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    //END : Save/Edit Data

    //START : Edit
    $scope.Edit = function (item) {
        $scope.Commodity["CommodityId"] = item.CommodityId;
        $scope.Commodity["CommodityNameEn"] = item.CommodityNameEn;
        $scope.Commodity["CommodityNameHi"] = item.CommodityNameHi;
        $scope.Commodity["IsKharif"] = item.IsKharif === true || item.IsKharif === 'TRUE' || item.IsKharif === 'True' || item.IsKharif === '1' || item.IsKharif === 1 ? '1' : '2';
        $scope.GridWarehouses = false;
        $scope.divInsert = true;
        console.log($scope.Commodity);
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

        $http.post(SiteUrl + "/Master/CommoditySetStatus/", { "CommodityId": Id, "ActionId": ActionId }).
            success(function (data, status) {
                if (data === -1) {
                    ShowModelAlert($scope.translation.Commodity, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else {
                    $scope.CommodityList();
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
    $scope.CommodityList();
    // END : On Page Load
});