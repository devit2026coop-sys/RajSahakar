
var MasterMod = angular.module('FertiMngEntryModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("FertiMngEntryCont", function ($scope, $http) {

    // SCOPE
    //$scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Divisions = [];
    $scope.Districts = [];
    $scope.Tehsil = [];
    $scope.Block = [];
    $scope.FertiMng = [];

    $scope.divPanchayatSamiti = false;
    $scope.divcity = false;
    $scope.divDisticts = false;
    $scope.DivGP = false;
    $scope.DivTehsil = false;
    $scope.DivFerti = false;
    $scope.DivWard = false;
    $scope.FertiMng.DistrictId = '0';
    $scope.FertiMng.BlockId = '0';
    $scope.FertiMng.KVSSId = '0';
    $scope.FertiMng.GPId = '0';
    $scope.FertiMng.GSSId = '0';
    $scope.FertiMng.TotReceipt = "";
    $scope.FertiMng.TotSale = "";
    $scope.FertiMng.TotStockEnd = "";
    $scope.FertiMng.FertiId = '0';
    $scope.StockVal = 0;

    // Code for Language label setting
    $scope.getTranslation = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
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

    $scope.pageChangeHandler = function (num) {
    };
    $scope.SetFertId = function () {
        $scope.FertiMng.FertiId = '0'
    }
    // Get List of Districts
    $scope.DistrictList = function () {
        // $(".se-pre-con").fadeIn("slow");
        $scope.Districts = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=0").
            success(function (data, status) {
                $scope.divDisticts = true;
                $scope.Districts = data;
            }).
            error(function (data, status) {
                $scope.Districts = [];
            });
        //  $(".se-pre-con").fadeOut("slow");
    };

    // Get List of Block
    $scope.BlockList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Blocks = [];
        //alert($scope.FertiMng.DistrictId);
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.FertiMng.DistrictId).
            success(function (data, status) {
                $scope.divPanchayatSamiti = true;
                $scope.Blocks = data;
            }).
            error(function (data, status) {
                $scope.Blocks = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Get List of Fin Year

    $scope.SetFinancialYearList = function () {
        // $(".se-pre-con").fadeIn("slow");
        $scope.FinancialYearList = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
        // $(".se-pre-con").fadeOut("slow");
    };

    $scope.GetFertiList = function () {
        $scope.FertiList = [];

        $http.get(SiteUrl + "/lov/GetLovValuesByListId?ListId=2").
            success(function (data, status) {
                $scope.FertiList = data;
            }).
            error(function (data, status) {
                $scope.FertiList = [];
            });
    };


    $scope.ShowEntryForm = function () {
        $scope.DivForm = true;
        $scope.DivtringGrid = false;
    };

    $scope.BackForm = function () {
        window.location.reload();
    };

    $scope.GetMonthList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MONTH&MasterCode=-1").
            success(function (data, status) {
                $scope.MonthList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    };
    // Get List of KVSS
    $scope.KVSSList = function () {
        //$(".se-pre-con").fadeIn("slow");
        $scope.KVSSList = [];
        $http.post(SiteUrl + "/EntryForms/KVSSociety", { "DistrictId": $scope.FertiMng.DistrictId }).
            success(function (data, status) {
                $scope.divPanchayatSamiti = true;
                $scope.KVSSList = data;
            }).
            error(function (data, status) {
                $scope.Blocks = [];
            });
        // $(".se-pre-con").fadeOut("slow");
    };

    $scope.GSSList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.GSSList = [];
        $http.post(SiteUrl + "/EntryForms/GetGSSocietyList", { "BlockId": $scope.FertiMng.BlockId }).
            success(function (data, status) {
                $scope.GSSList = data;
            }).
            error(function (data, status) {
                $scope.GSSList = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Get List of GramPanchayat
    $scope.GramPanchayatList = function () {
      //  $(".se-pre-con").fadeIn("slow");
        $scope.GramPanchayats = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.FertiMng.BlockId).
            success(function (data, status) {
                $scope.DivGP = true;
                $scope.GramPanchayats = data;
            }).
            error(function (data, status) {
                $scope.GramPanchayats = [];
            });
      //  $(".se-pre-con").fadeOut("slow");
    };

    $scope.FertiReportList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.DataList = [];

        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();
        formData.append("FinYearId", $scope.FertiMng.FinYearId);
        formData.append("MonthId", $scope.FertiMng.MonthId);
        formData.append("DistrictId", $scope.FertiMng.DistrictId);
        formData.append("BlockId", $scope.FertiMng.BlockId);
        formData.append("KVSSId", $scope.FertiMng.KVSSId);
        formData.append("GPId", $scope.FertiMng.GPId);
        formData.append("GSSId", $scope.FertiMng.GSSId);
        formData.append("FertiId", $scope.FertiMng.FertiId);
        $http.post(SiteUrl + "/EntryForms/GetFertiMNGReportList", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.DataList = data;
                }
            }).
            error(function (data, status) {
            });
        $(".se-pre-con").fadeOut("slow");
    };

    $scope.DeleteFertiMNGReport = function (FertiMNGReportId) {
        $(".se-pre-con").fadeIn("slow");
        $scope.GramPanchayats = [];
        $http.post(SiteUrl + "/EntryForms/DeleteFertiMNGReport", { "FertiMNGReportId": FertiMNGReportId }).
            success(function (data, status) {
                if (data == "1") {
                    alert(_language == 'en-us' ? "Record Deleted successfully." : "रिकॉर्ड सफलतापूर्वक मिटाया गया।");
                    location.reload();
                } else if (data == "2") {
                    alert(_language == 'en-us' ? "You can delete only latest Entry of fertilizer of GSS." : "आप केवल GSS के उर्वरक की नवीनतम एंट्री को हटा सकते हैं।");
                }
                else {
                    alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                    location.reload();
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                $(".se-pre-con").fadeOut("slow");
            });
    };

    // Language Selector
    $scope.getTranslation($scope);

    // Get list of all users on page list
    $scope.DistrictList();

    $scope.SaveFerti = function () {
        var formData = new FormData();
        if ($scope.FertiMng.FinYearId == '0') {
            alert("Please Select Fincial Year.")
            return;
        }
        if ($scope.FertiMng.MonthId == '0') {
            alert("Please Select Month.")
            return;
        }
        if ($scope.FertiMng.DistrictId == '0') {
            alert("Please Select District.")
            return;
        }
        if ($scope.FertiMng.BlockId == '0') {
            alert("Please Select Panchayat Samiti.")
            return;
        }
        if ($scope.FertiMng.KVSSId == '0') {
            alert("Please Select KVSS.")
            return;
        }
        if ($scope.FertiMng.GSSId == '0') {
            alert("Please Select KVSS.")
            return;
        }
        if ($scope.FertiMng.FertiId == '0') {
            alert("Please Select Fertilizer.")
            return;
        }
        if ($scope.FertiMng.TotReceipt.length == 0 || $scope.FertiMng.TotReceipt == ".") {
            alert("Please Enter value in Total Receipt During the Month.")
            return;
        }
        else if ($scope.FertiMng.TotReceipt.toString().split(".").length > 2) {
            alert("Please Enter proper value in Total Receipt During the Month.")
            return;
        }
        if ($scope.FertiMng.TotSale.length == 0 || $scope.FertiMng.TotSale == ".") {
            alert("Please Enter value in Total Sale During the Month.")
            return;
        }
        else if ($scope.FertiMng.TotSale.toString().split(".").length > 2) {
            alert("Please Enter proper value in Total Sale During the Month.")
            return;
        }
        if ($scope.FertiMng.TotStockEnd.length == 0 || $scope.FertiMng.TotStockEnd == ".") {
            alert("Please Enter value in Stock in hand at end of Month.")
            return;
        }
        else if ($scope.FertiMng.TotStockEnd.toString().split(".").length > 2) {
            alert("Please Enter proper value in Stock in hand at end of Month.")
            return;
        }
        formData.append("FinYearId", $scope.FertiMng.FinYearId);
        formData.append("MonthId", $scope.FertiMng.MonthId);
        formData.append("DistrictId", $scope.FertiMng.DistrictId);
        formData.append("BlockId", $scope.FertiMng.BlockId);
        formData.append("KVSSId", $scope.FertiMng.KVSSId);
        formData.append("GPId", $scope.FertiMng.GPId);
        formData.append("GSSId", $scope.FertiMng.GSSId);
        formData.append("FertiId", $scope.FertiMng.FertiId);
        formData.append("TotReceipt", $scope.FertiMng.TotReceipt);
        formData.append("TotSale", $scope.FertiMng.TotSale);
        formData.append("TotStockEnd", $scope.FertiMng.TotStockEnd);
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/EntryForms/SaveFertiMNGReport", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                if (data == "1") {
                    alert(_language == 'en-us' ? "Record saved successfully." : "रिकॉर्ड सफलतापूर्वक सहेजा गया।");
                    location.reload();
                }
                else if (data == "-2") {
                    alert(_language == "en-us" ? "Duplicate data found." : "डुप्लिकेट डेटा मिला।");
                }
                else if (data == "-3") {
                    alert(_language == "en-us" ? "You can not enter older data than previous entry of Fertilizer of this GSS." : "आप इस GSS के उर्वरक की पिछली प्रविष्टि से पुराना डेटा दर्ज नहीं कर सकते हैं।");
                }
                else {
                    alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.GetStockVal = function () {
        $scope.StockVal = 0;
        $scope.FertiMng.TotStockEnd = '';
        $scope.StockHaveValue = false;
        var formData = new FormData();
        var Flag = 0;
        if ($scope.FertiMng.FinYearId == '0') {
            Flag = 1;
        }
        if ($scope.FertiMng.MonthId == '0') {
            Flag = 1;
        }
        if ($scope.FertiMng.GSSId == '0') {
            Flag = 1;
        }
        if ($scope.FertiMng.FertiId == '0') {
            Flag = 1;
        }
        if (Flag == 0) {
            formData.append("FinYearId", $scope.FertiMng.FinYearId);
            formData.append("MonthId", $scope.FertiMng.MonthId);
            formData.append("DistrictId", $scope.FertiMng.DistrictId);
            formData.append("BlockId", $scope.FertiMng.BlockId);
            formData.append("KVSSId", $scope.FertiMng.KVSSId);
            formData.append("GPId", $scope.FertiMng.GPId);
            formData.append("GSSId", $scope.FertiMng.GSSId);
            formData.append("FertiId", $scope.FertiMng.FertiId);
            $http.post(SiteUrl + "/EntryForms/GetStockVal", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    if (data.length > 0) {
                        $scope.StockVal = data;
                        if (data != 0) {
                            $scope.StockHaveValue = true;
                            $scope.FertiMng.TotStockEnd = $scope.StockVal;
                        }
                    }
                    else
                        $scope.StockVal = 0;
                }).
                error(function (data, status) {
                });
        }
    };
    $scope.AddStockVal = function () {
        //alert($scope.FertiMng.TotReceipt.length);
        if ($scope.FertiMng.TotReceipt.length > 0 && $scope.FertiMng.TotSale.length > 0) {
           // alert($scope.FertiMng.TotSale.length);
            if ($scope.FertiMng.TotReceipt.length == 0 || $scope.FertiMng.TotReceipt == ".") {
                alert("Please Enter value in Total Receipt During the Month.")
                return;
            }
            else if ($scope.FertiMng.TotReceipt.split(".").length > 2) {
                alert("Please Enter proper value in Total Receipt During the Month.")
                return;
            }
            if ($scope.FertiMng.TotSale.length == 0 || $scope.FertiMng.TotSale == ".") {
                alert("Please Enter value in Total Sale During the Month.")
                return;
            }
            else if ($scope.FertiMng.TotSale.split(".").length > 2) {
                alert("Please Enter proper value in Total Sale During the Month.")
                return;
            }
            if ($scope.StockHaveValue){
                $scope.FertiMng.TotStockEnd = ((parseFloat($scope.StockVal) + parseFloat($scope.FertiMng.TotReceipt)) - parseFloat($scope.FertiMng.TotSale));
            }
        }
        else {
            $scope.FertiMng.TotStockEnd = parseFloat($scope.StockVal);
        }

    }
    $scope.GetMonthList();
    $scope.SetFinancialYearList();
    $scope.GetFertiList();
    $scope.DivtringGrid = true;
    $scope.DivForm = false;
    $scope.FertiReportList();
});