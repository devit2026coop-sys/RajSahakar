var CommodityMSPController = angular.module("CommodityMSPCnt", ['angularUtils.directives.dirPagination']);
CommodityMSPController.controller("CommodityMSPMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridWarehouses = true;
    $scope.divInsert = false;
    $scope.IsDisabled = true;
    $scope.CommodityMSP = [];

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


    // START : Get Commodity List
    $scope.CommodityList = function () {
        $http.post(SiteUrl + "/Master/CommodityList/", { "CommodityId": "0" }).
            success(function (data, status) {
                $scope.Commodities = data.filter(p => p.IsActive === '1' || p.IsActive === 1 || p.IsActive === 'True' || p.IsActive === true);
            }).
            error(function () {
                $scope.Commodities = [];
            });
    };
    // END : Get Commodity List

    // START : Get Unit List
    $scope.UnitList = function () {
        $http.post(SiteUrl + "/Stock/UnitMasterList/", { "UnitId": "0" }).
            success(function (data, status) {
                $scope.Units = data.filter(p => p.IsActive === '1' || p.IsActive === 1 || p.IsActive === 'True' || p.IsActive === true);
            }).
            error(function () {
                $scope.Units = [];
            });
    };
    // END : Get Unit List

    // START : Get FinancialYear List
    $scope.FinancialYearList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR").
            success(function (data, status) {
                $scope.FinancialYears = data.filter(p => p.IS_ACTIVE === '1' || p.IS_ACTIVE === 1 || p.IS_ACTIVE === 'True' || p.IS_ACTIVE === true);
            }).
            error(function () {
                $scope.FinancialYears = [];
            });
    };
    // END : Get FinancialYear List

    // START : Get MSP List
    $scope.GetMSPList = function () {
        $http.post(SiteUrl + "/Master/CommodityMSPList", { "CommodityMSPId": '0' }).
            success(function (data, status) {
                $scope.CommodityMSPs = data;
            }).
            error(function () {
                $scope.FinancialYears = [];
            });
    };
    // END : Get MSP List

    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.CommodityMSP = [];
        $scope.CommodityMSP.FYEAR_ID = '0';
        $scope.CommodityMSP.CommodityId = '0';
        $scope.CommodityMSP.UnitId = '0';
        $scope.CommodityMSP.Price = '';
        $scope.CommodityMSP.Msp_Id = '0';
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

        if ($scope.CommodityMSP === undefined || $scope.CommodityMSP.FYEAR_ID === '0') {
            message += $scope._language === 'en-us' ? '\n <br /> Please select financial year' : '\n <br /> कृपया वित्तीय वर्ष चुने |';
        }

        if ($scope.CommodityMSP === undefined || $scope.CommodityMSP.CommodityId === '0') {
            message += $scope._language === 'en-us' ? '\n <br /> Please select commodity' : '\n <br /> कृपया जिंस चुने |';
        }

        //if ($scope.CommodityMSP === undefined || $scope.CommodityMSP.UnitId === '0') {
        //    message += $scope._language === 'en-us' ? '\n <br /> Please select unit' : '\n <br /> कृपया इकाई चुने |';
        //}

        if ($scope.CommodityMSP === undefined || $scope.CommodityMSP.Price === undefined || $scope.CommodityMSP.Price.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter price' : '\n <br /> कृपया मूल्य दर्ज करें |';
        }

        if ($scope.CommodityMSP === undefined || $scope.CommodityMSP.NafedChargesa === undefined || $scope.CommodityMSP.NafedChargesa.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Nafed Chargesa' : '\n <br /> कृपया मूल्य Nafed Chargesa दर्ज करें |';
        }

        if ($scope.CommodityMSP === undefined || $scope.CommodityMSP.HighCharges === undefined || $scope.CommodityMSP.HighCharges.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter High Charges' : '\n <br /> कृपया  मूल्य High Charges दर्ज करें |';
        }



        if (message.length > 5) {
            ShowModelAlert($scope.translation.CommodityMSP, message);
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
            Msp_Id: $scope.CommodityMSP.Msp_Id,
            CommodityId: $scope.CommodityMSP.CommodityId,
            UnitId:1,// $scope.CommodityMSP.UnitId,
            Price: $scope.CommodityMSP.Price,
            FYEAR_ID: $scope.CommodityMSP.FYEAR_ID,
            NafedChargesa: $scope.CommodityMSP.NafedChargesa,
            HighCharges: $scope.CommodityMSP.HighCharges,
            msp_nccf: $scope.CommodityMSP.msp_nccf

        };
        console.log(model);
        $http.post(SiteUrl + "/Master/CommodityMSPInsertUpdate/", { "model": model }).
            success(function (data, status) {
                if (parseInt(data, 10) > 0) {
                    $(".se-pre-con").fadeOut("slow");
                    alert(_language === 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।');
                    $scope.GetMSPList();
                    $scope.BackToList();
                }
                else if (parseInt(data, 10) === -1) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.CommodityMSP, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else if (parseInt(data, 10) === -2) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.CommodityMSP, _language === 'en-us' ? 'Duplicate data. No action performed.' : 'डुप्लिकेट डेटा। कोई कार्रवाई नहीं की गई।', function () { });
                }
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    //END : Save/Edit Data

    //START : Edit
    $scope.Edit = function (item) {
        $scope.CommodityMSP["Msp_Id"] = item.Msp_Id;
        $scope.CommodityMSP["CommodityId"] = item.CommodityId;
        $scope.CommodityMSP["UnitId"] = item.UnitId;
        $scope.CommodityMSP["Price"] = item.Price;
        $scope.CommodityMSP["FYEAR_ID"] = item.FYEAR_ID;

        $scope.CommodityMSP["NafedChargesa"] = item.NafedChargesa;
        $scope.CommodityMSP["HighCharges"] = item.HighCharges;

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

        $http.post(SiteUrl + "/Master/CommodityMSPSetStatus/", { "CommodityMSPId": Id, "ActionId": ActionId }).
            success(function (data, status) {
                if (data === -1) {
                    ShowModelAlert($scope.translation.CommodityMSP, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
                }
                else {
                    $scope.GetMSPList();
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
    $scope.GetMSPList();
    $scope.CommodityList();
    $scope.UnitList();
    $scope.FinancialYearList();
    // END : On Page Load
});