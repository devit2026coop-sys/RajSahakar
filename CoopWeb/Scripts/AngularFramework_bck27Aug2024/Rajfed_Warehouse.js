var WarehouseController = angular.module("WarehouseCnt", ['angularUtils.directives.dirPagination']);
WarehouseController.controller("WarehouseMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridWarehouses = true;
    $scope.divInsert = false;
    $scope.IsDisabled = true;
    $scope.Warehouse = [];
    $scope.WarehouseId = 0;
    // START : Page load Function
    $scope.OnPageLoad = function () {
        $scope.WarehouseId=$('#hdnWarehouseId').val();
        $scope._language = _language;
        $scope.DistrictList();
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

        var userLevel = parseInt($('#hdnUserLevel').val(), 10);
        var districtId = $('#hdnUserDistrict').val();
        $scope.Warehouse["DistrictId"] = districtId;
        $scope.Warehouse.DistrictId = districtId;

        if (userLevel === 3) {
            $scope.IsDisabled = true;
            $scope.GetCityList();
        }
        else {
            $scope.IsDisabled = false;
        }
    };
    // END : Page load Function

    //START : UrbanRuralChange 
    $scope.UrbanRuralChange = function () {
        if ($scope.Warehouse.UrbanRural === '2') {
            $scope.BlockList();
        }
        else {
            $scope.GetCityList();
        }
    };
    //END : UrbanRuralChange

    //START : District Change 
    $scope.DistrictChange = function () {
        if ($scope.Warehouse.UrbanRural === '2') {
            $scope.BlockList();
        }
        else {
            $scope.GetCityList();
        }
    };
    //END : District Change

    // START : Get City List By City Type
    $scope.GetCityList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.CityList = [];
        $scope.Warehouse.CityId = '0';

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITY&MasterCode=" + $scope.Warehouse.DistrictId + "&MasterCode2=0").
            success(function (data, status) {
                $scope.CityList = data;
            }).
            error(function (data, status) {
                $scope.CityList = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // START : Get City List By City Type

    // START : Get District List
    $scope.DistrictList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Blocks = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=0").
            success(function (data, status) {
                $scope.Districts = data;
            }).
            error(function (data, status) {
                $scope.Districts = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : Get District List

    // START : Get Block List
    $scope.BlockList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Blocks = [];
        $scope.Warehouse.BlockId = '0';
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.Warehouse.DistrictId).
            success(function (data, status) {
                $scope.Blocks = data;

            }).
            error(function (data, status) {
                $scope.Blocks = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : Get Block List

    // START : Get Data List
    $scope.WarehouseList = function () {
        $http.post(SiteUrl + "/Master/RajfedWarehouseList/", { "WarehouseId": $scope.WarehouseId }).
            success(function (data, status) {
                $scope.Warehouses = data;
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
        $scope.Warehouse = [];
        $scope.Warehouse.WarehousId = '0';
        $scope.Warehouse.WarehousNameEn = '';
        $scope.Warehouse.WarehousNameHi = '';
        $scope.Warehouse.UrbanRural = '1';
        $scope.Warehouse.IsCentralWarehouse = '2';
        $scope.CityList = [];
        $scope.Warehouse.CityId = '0';
        $scope.Warehouse.DistrictId = $('#hdnUserDistrict').val();
        $scope.Blocks = [];
        $scope.Warehouse.BlockId = '0';
        $scope.Warehouse.LicenceNo = '';
        $('#txtLicenceValidUpto').val('');
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
        //$scope.ClearForm();
        $scope.GridWarehouses = true;
        $scope.divInsert = false;
    };
    //END : Back To List

    //START : Validate Form
    $scope.ValidateForm = function () {
        var message = '';

        if ($scope.Warehouse === undefined || $scope.Warehouse.WarehousNameEn === undefined || $scope.Warehouse.WarehousNameEn.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Warehouse Name (in Eng).' : '\n <br /> कृपया वेयरहाउस नाम (अंग्रेज़ी) दर्ज करें |';
        }

        if ($scope.Warehouse === undefined || $scope.Warehouse.WarehousNameHi === undefined || $scope.Warehouse.WarehousNameHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Warehouse Name (in Hindi)' : '\n <br /> कृपया वेयरहाउस नाम (हिंदी) दर्ज करें  |';
        }

        if ($scope.Warehouse === undefined || $scope.Warehouse.LicenceNo === undefined || $scope.Warehouse.LicenceNo.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Licence No.' : '\n <br /> कृपया लाइसेंस नंबर दर्ज करें |';
        }   

        if ($('#txtLicenceValidUpto').val().length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Licence vality date' : '\n <br /> कृपया लाइसेंस वैधता तिथि दर्ज करें |';
        }

        if ($scope.Warehouse.UrbanRural === undefined) {
            message += $scope._language === 'en-us' ? '\n <br /> Please select Urban/Rural Type' : '\n <br /> कृपया शहरी / ग्रामीण चुने|';
        }
        if ($scope.Warehouse.IsCentralWarehouse === undefined) {
            message += $scope._language === 'en-us' ? '\n <br /> Please select Is Central Warehouse?' : '\n <br /> सेंट्रल वेयरहाउस है? चुने|';
        }

        if ($scope.Warehouse === undefined || $scope.Warehouse.DistrictId === undefined || $scope.Warehouse.DistrictId === '0') {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select District' : '\n <br /> कृपया ज़िला चुनें|';
        }

        if ($scope.Warehouse === undefined || $scope.Warehouse.Address === undefined || $scope.Warehouse.Address.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Address' : '\n <br /> कृपया पता दर्ज करें  |';
        }


        if ($scope.Warehouse.UrbanRural === 1 || $scope.Warehouse.UrbanRural === '1') {

            if ($scope.Warehouse === undefined || $scope.Warehouse.CityId === undefined || $scope.Warehouse.CityId === '0') {
                message += $scope._language === 'en-us' ? '\n <br /> Please Select City' : '\n <br /> कृपया शहर चुनें|';
            }
        }

        if ($scope.Warehouse.UrbanRural === 2 || $scope.Warehouse.UrbanRural === '2') {

            if ($scope.Warehouse === undefined || $scope.Warehouse.BlockId === undefined || $scope.Warehouse.BlockId === '0') {
                message += $scope._language === 'en-us' ? '\n <br /> Please Select Block/Tehsil' : '\n <br /> कृपया ब्लाक चुनें|';
            }
        }

        if (message.length > 5) {

            ShowModelAlert($scope.translation.Warehouse, message);
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
            WarehousId: $scope.Warehouse.WarehousId,
            WarehousNameEn: $scope.Warehouse.WarehousNameEn,
            WarehousNameHi: $scope.Warehouse.WarehousNameHi,
            DistrictId: $scope.Warehouse.DistrictId,
            IsCity: $scope.Warehouse.UrbanRural === 1 || $scope.Warehouse.UrbanRural === '1' ? 1 : 0,
            IsCentralWarehouse: $scope.Warehouse.IsCentralWarehouse === 1 || $scope.Warehouse.IsCentralWarehouse === '1' ? true : false,
            BlockCityId: $scope.Warehouse.UrbanRural === 1 || $scope.Warehouse.UrbanRural === '1' ? $scope.Warehouse.CityId : $scope.Warehouse.BlockId,
            Address: $scope.Warehouse.Address,
            LicenceNo: $scope.Warehouse.LicenceNo,
            LicenceValidUpTo: $('#txtLicenceValidUpto').val()
        };
        console.log(model);
        $http.post(SiteUrl + "/Master/RajfedWarehouseInsertUpdate/", { "model": model }).
            success(function (data, status) {
                if (parseInt(data, 10) > 0) {
                    $(".se-pre-con").fadeOut("slow");
                    alert(_language === 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।');
                    $scope.WarehouseList();
                    $scope.BackToList();
                }
                else if (parseInt(data, 10) === -1) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.GoDown, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else if (parseInt(data, 10) === -2) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.GoDown, _language === 'en-us' ? 'Duplicate data. No action performed.' : 'डुप्लिकेट डेटा। कोई कार्रवाई नहीं की गई।', function () { });
                }
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    //END : Save/Edit Data

    //START : Edit
    $scope.Edit = function (item) {
        $scope.Warehouse["WarehousId"] = item.WarehousId;
        $scope.Warehouse["WarehousNameEn"] = item.WarehousNameEn;
        $scope.Warehouse["WarehousNameHi"] = item.WarehousNameHi;
        $scope.Warehouse["LicenceNo"] = item.LicenceNo;
        $('#txtLicenceValidUpto').val(item.LicenceValidUpTo);
        $scope.Warehouse["UrbanRural"] = item.ISCITY === true || item.ISCITY === 'TRUE' || item.ISCITY === 'True' || item.ISCITY === '1' || item.ISCITY === 1 ? '1' : '2';
        $scope.Warehouse["IsCentralWarehouse"] = item.IsCentralWarehouse === true || item.IsCentralWarehouse === 'TRUE' || item.IsCentralWarehouse === 'True' || item.IsCentralWarehouse === '1' || item.IsCentralWarehouse === 1 ? '1' : '2';

        $scope.DistrictList();
        $scope.Warehouse["DistrictId"] = item.DISTRICT_ID;

        if (item.ISCITY === true || item.ISCITY === 'TRUE' || item.ISCITY === 'True' || item.ISCITY === '1' || item.ISCITY === 1) {
            $scope.GetCityList();
            $scope.Warehouse["CityId"] = item.CITY_ID;
        }
        else {
            $scope.BlockList();
            $scope.Warehouse["BlockId"] = item.BLOCK_ID;
        }

        $scope.Warehouse["Address"] = item.Address;
        $scope.GridWarehouses = false;
        $scope.divInsert = true;
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

        $http.post(SiteUrl + "/Master/RajfedWarehouseSetStatus/", { "WarehousId": Id, "ActionId": ActionId }).
            success(function (data, status) {
                if (data === -1) {
                    ShowModelAlert($scope.translation.Warehous, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else {
                    $scope.WarehouseList();
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
    $scope.WarehouseList();
    // END : On Page Load
});