var GoDownController = angular.module("GoDownCnt", ['angularUtils.directives.dirPagination']);
GoDownController.controller("GoDownMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridGodowns = true;
    $scope.divInsert = false;
    $scope.IsDisabled = true;
    $scope.Godown = [];

    // START : Page load Function
    $scope.OnPageLoad = function () {
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

        var userLevel = parseInt($('#hdnUserLevel').val(), 10);
        var districtId = $('#hdnUserDistrict').val();
        var warehouseId = $('#hdnWarehouseId').val();
        $scope.Godown["DistrictId"] = districtId;
        $scope.Godown.DistrictId = districtId;
        $scope.Godown.WarehouseId = warehouseId;
        if (userLevel === 3) {
            $scope.IsDisabled = true;           
        }
        else {
            $scope.IsDisabled = false;
        }
        $scope.GetWarehouseList();
       

    };
    // END : Page load Function

    // START : Get Warehouse List
    $scope.GetWarehouseList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Master/RajfedWarehouseList/", { "WarehouseId": $scope.Godown.WarehouseId}).
            success(function (data, status) {
                $scope.Warehouses = data;
            }).
            error(function (data, status) {
                $scope.Warehouses = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : Get Warehouse List

    //START : UrbanRuralChange 
    $scope.UrbanRuralChange = function () {
        if ($scope.Godown.UrbanRural === '2') {
            $scope.BlockList();
        }
        else {
            $scope.GetCityList();
        }
    };
    //END : UrbanRuralChange

    // START : Get City Type List
    $scope.CityTypeList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYTYPE&MasterCode=0").
            success(function (data, status) {
                $scope.CityTypes = data;
            }).
            error(function (data, status) {
                $scope.CityTypes = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : Get City Type List

    // START : Get City List By City Type
    $scope.GetCityList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.CityList = [];
        $scope.Godown.CityId = '0';
        $scope.Ward = [];
        $scope.Godown.WardId = '0';
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITY&MasterCode=" + $scope.Godown.DistrictId+"&MasterCode2=0").
            success(function (data, status) {
                $scope.CityList = data;
            }).
            error(function (data, status) {
                $scope.CityList = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // START : Get City List By City Type

    // START : Get Ward List
    $scope.GetWardList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Ward = [];
        $scope.Godown.WardId = '0';
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ward&MasterCode=" + $scope.Godown.CityId).
            success(function (data, status) {
                $scope.Ward = data;
            }).
            error(function (data, status) {
                $scope.Ward = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // START : Get Ward List

    // START : Get Division List
    $scope.DivisionList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DIVISION&MasterCode=0").
            success(function (data, status) {
                $scope.Divisions = data;
            }).
            error(function (data, status) {
                $scope.Divisions = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : Get Division List

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
        $scope.Godown.BlockId = '0';
        $scope.GramPanchayats = [];
        $scope.Godown.GPId = '0';
        $scope.Villages = [];
        $scope.Godown.VillageId = '0';
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.Godown.DistrictId).
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

    // START : Get GP List
    $scope.GramPanchayatList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.GramPanchayats = [];
        $scope.Godown.GPId = '0';
        $scope.Villages = [];
        $scope.Godown.VillageId = '0';
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.Godown.BlockId).
            success(function (data, status) {
                $scope.GramPanchayats = data;
            }).
            error(function (data, status) {
                $scope.GramPanchayats = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : Get GP List

    // START : Get Village List
    $scope.VillageList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Villages = [];
        $scope.Godown.VillageId = '0';
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.Godown.GPId).
            success(function (data, status) {
                $scope.Villages = data;
            }).
            error(function (data, status) {
                $scope.Villages = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // START : Get Village List


    // START : Get Data List
    $scope.GodownList = function () {        
        $http.post(SiteUrl + "/Master/RajfedGoDownList", { "GoDownId": "0", "WarehouseId": $scope.Godown.WarehouseId}).
            success(function (data, status) {
                $scope.Godowns = data;
                $scope.TotalGodowns = data.length;
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
                $scope.Godowns = [];
            });
    };
    // END : Get Data List

    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.Godown = [];
        $scope.Godown.GoDownId = '0';
        $scope.Godown.WarehouseId = '0';
        $scope.Godown.GoDownTypeId = '1';
        $scope.Godown.GoDownNameEng = '';
        $scope.Godown.GoDownNameHi = '';
        $scope.Godown.UrbanRural = '1';        
        //$scope.Godown.CityTypeId = '0';
        //$scope.CityList = [];
        $scope.Godown.CityId = '0';
        $scope.Ward = [];
        $scope.Godown.WardId = '0';
        //$scope.Godown.DivisionId = '0';
        //$scope.Districts = [];
        $scope.Godown.DistrictId = $('#hdnUserDistrict').val();
        $scope.Godown.WarehouseId = $('#hdnWarehouseId').val();
        $scope.Blocks = [];
        $scope.Godown.BlockId = '0';
        //$scope.GramPanchayats = [];
        //$scope.Godown.GPId = '0';
        //$scope.Villages = [];
        //$scope.Godown.VillageId = '0';
    };
    //END : Clear Form

    //START : Show Div For Insert / Update
    $scope.ShowDivForInsert = function () {
        $scope.ClearForm();
        $scope.GridGodowns = false;
        $scope.divInsert = true;
    };
    //END : Show Div For Insert / Update

    //START : Back To List
    $scope.BackToList = function () {
        //$scope.ClearForm();
        $scope.GridGodowns = true;
        $scope.divInsert = false;
    };
    //END : Back To List

    //START : Validate Form
    $scope.ValidateForm = function () {
        var message = '';

        if ($scope.Godown.WarehouseId === undefined || $scope.Godown.WarehouseId === '0') {
            message += $scope._language === 'en-us' ? '\n <br /> Please select wearehouse.' : '\n <br /> कृपया वेयरहाउस चुने |';
        }
        if ($scope.Godown.GoDownTypeId === undefined) {
            message += $scope._language === 'en-us' ? '\n <br /> Please select Go Down Type' : '\n <br /> कृपया गोदाम प्रकार चुने|';
        }

        if ($scope.Godown === undefined || $scope.Godown.GoDownNameEng === undefined || $scope.Godown.GoDownNameEng.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Godown Name (in Eng).' : '\n <br /> कृपया गोदाम नाम (अंग्रेज़ी) दर्ज करें |';
        }

        if ($scope.Godown === undefined || $scope.Godown.GoDownNameHi === undefined || $scope.Godown.GoDownNameHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Godown Name (in Hindi)' : '\n <br /> कृपया गोदाम नाम (हिंदी) दर्ज करें  |';
        }

        if ($scope.Godown.UrbanRural === undefined) {
            message += $scope._language === 'en-us' ? '\n <br /> Please select Urban/Rural Type' : '\n <br /> कृपया शहरी / ग्रामीण चुने|';
        }

        if ($scope.Godown === undefined || $scope.Godown.DistrictId === undefined || $scope.Godown.DistrictId === '0') {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select District' : '\n <br /> कृपया ज़िला चुनें|';
        }

        if ($scope.Godown === undefined || $scope.Godown.PIN === undefined || $scope.Godown.PIN.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter PIN' : '\n <br /> कृपया पिन दर्ज करें  |';
        }


        if ($scope.Godown.UrbanRural === 1 || $scope.Godown.UrbanRural === '1') {

            //if ($scope.Godown === undefined || $scope.Godown.CityTypeId === undefined || $scope.Godown.CityTypeId === '0') {
            //    message += $scope._language === 'en-us' ? '\n <br /> Please Select District' : '\n <br /> कृपया सिटी टाइप चुनें|';
            //}
            if ($scope.Godown === undefined || $scope.Godown.CityId === undefined || $scope.Godown.CityId === '0') {
                message += $scope._language === 'en-us' ? '\n <br /> Please Select City' : '\n <br /> कृपया शहर चुनें|';
            }
            if ($scope.Godown === undefined || $scope.Godown.WardId === undefined || $scope.Godown.WardId === '0') {
                message += $scope._language === 'en-us' ? '\n <br /> Please Select ward' : '\n <br /> कृपया वार्ड चुनें|';
            }
        }

        if ($scope.Godown.UrbanRural === 2 || $scope.Godown.UrbanRural === '2') {
            //if ($scope.Godown === undefined || $scope.Godown.DivisionId === undefined || $scope.Godown.DivisionId === '0') {
            //    message += $scope._language === 'en-us' ? '\n <br /> Please Select Division' : '\n <br /> कृपया संभाग चुनें|';
            //}
            
            if ($scope.Godown === undefined || $scope.Godown.BlockId === undefined || $scope.Godown.BlockId === '0') {
                message += $scope._language === 'en-us' ? '\n <br /> Please Select Block/Tehsil' : '\n <br /> कृपया ब्लाक चुनें|';
            }
            //if ($scope.Godown === undefined || $scope.Godown.GPId === undefined || $scope.Godown.GPId === '0') {
            //    message += $scope._language === 'en-us' ? '\n <br /> Please Select Gram Panchayat' : '\n <br /> कृपया ग्रामपंचायत चुनें|';
            //}
            //if ($scope.Godown === undefined || $scope.Godown.VillageId === undefined || $scope.Godown.VillageId === '0') {
            //    message += $scope._language === 'en-us' ? '\n <br /> Please Select Village' : '\n <br /> कृपया ग्राम चुनें|';
            //}
        }

        if (message.length > 5) {

            ShowModelAlert($scope.translation.GoDown, message);
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
            GoDownId: $scope.Godown.GoDownId,
            WarehouseId: $scope.Godown.WarehouseId,
            GoDownNameEng: $scope.Godown.GoDownNameEng,
            GoDownNameHi: $scope.Godown.GoDownNameHi,
            GoDownTypeId: $scope.Godown.GoDownTypeId,
            IsRural: $scope.Godown.UrbanRural === 2 || $scope.Godown.UrbanRural === '2' ? true : false,

            CityTypeId: $scope.Godown.UrbanRural === 1 || $scope.Godown.UrbanRural === '1' ? $scope.Godown.CityTypeId : '0',
            CityId: $scope.Godown.UrbanRural === 1 || $scope.Godown.UrbanRural === '1' ? $scope.Godown.CityId : '0',
            WardId: $scope.Godown.UrbanRural === 1 || $scope.Godown.UrbanRural === '1' ? $scope.Godown.WardId : '0',

            DivisionId: $scope.Godown.UrbanRural === 2 || $scope.Godown.UrbanRural === '2' ? $scope.Godown.DivisionId : '0',
            DistrictId: $scope.Godown.UrbanRural === 2 || $scope.Godown.UrbanRural === '2' ? $scope.Godown.DistrictId : '0',
            BlockId: $scope.Godown.UrbanRural === 2 || $scope.Godown.UrbanRural === '2' ? $scope.Godown.BlockId : '0',
            GPId: $scope.Godown.UrbanRural === 2 || $scope.Godown.UrbanRural === '2' ? $scope.Godown.GPId : '0',
            VillageId: $scope.Godown.UrbanRural === 2 || $scope.Godown.UrbanRural === '2' ? $scope.Godown.VillageId : '0',
            
            Address: $scope.Godown.Address
        };

        console.log(model);

        $http.post(SiteUrl + "/Master/RajfedGoDownInsertUpdate/", { "model": model }).
            success(function (data, status) {
                if (parseInt(data, 10) > 0) {
                    $(".se-pre-con").fadeOut("slow");
                    alert(_language === 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।');
                    $scope.GodownList();
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
       
        $scope.Godown["GoDownId"] = item.GoDownId;
        $scope.Godown["WarehouseId"] = item.WarehouseId;
        $scope.Godown["GoDownNameEng"] = item.GoDownNameEng;
        $scope.Godown["GoDownNameHi"] = item.GoDownNameHi;
        $scope.Godown["GoDownTypeId"] = item.GoDownTypeId;
        $scope.Godown["UrbanRural"] = item.IsRural === true || item.IsRural === 'True' ? '2' : '1';

        if (item.IsRural === true || item.IsRural === 'True') {
            $scope.Godown["DivisionId"] = item.DivisionId;
            $scope.DistrictList();
            $scope.Godown["DistrictId"] = item.DistrictId;
            $scope.BlockList();
            $scope.Godown["BlockId"] = item.BlockId;
            $scope.GramPanchayatList();
            $scope.Godown["GPId"] = item.GPId;
            $scope.VillageList();
            $scope.Godown["VillageId"] = item.VillageId;
        }
        else {
            $scope.Godown["CityTypeId"] = item.CityTypeId;
            $scope.GetCityList();
            $scope.Godown["CityId"] = item.CityId;
            $scope.GetWardList();
            $scope.Godown["WardId"] = item.WardId;
        }
       
        $scope.Godown["Address"] = item.Address;
        $scope.GridGodowns = false;
        $scope.divInsert = true;
        console.log($scope.Godown);
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

        $http.post(SiteUrl + "/Master/RajfedGoDownSetStatus/", { "GoDownId": Id, "ActionId": ActionId }).
            success(function (data, status) {
                if (data === -1) {
                    ShowModelAlert($scope.translation.GoDown, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else {
                    $scope.GodownList();
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
    
    $scope.GodownList();
    // END : On Page Load
});