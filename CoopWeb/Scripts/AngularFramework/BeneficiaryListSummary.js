
var MasterMod = angular.module('BeneficiaryListMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("BeneficiaryListCnt", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.search = {};
    $scope.divClass = false;
    $scope.DistrictList = [];
    $scope.SubDistrictList = [];
    $scope.VillageList = [];
    $scope.BeneficiaryList = [];
    $scope.TotalBeneficiaryList = [];
    $scope.FinancialYearList = [];
    $scope.InstallmentTypeList = [];
    $scope.InstallmentTypeId = 0;
    $scope.FinancialYearId = 0;
    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get("/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("/Translations/Translation_HI.json").
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
        $scope.GetPMKisanMasterData();
        $scope.GetInstallmentType();
    };
    $scope.GetInstallmentType = function () {
        $http.get("/Master/GetMasterData/?MasterType=CMInstallmentType").
            success(function (data, status) {
                $scope.InstallmentTypeList = data;
                if ($scope.InstallmentTypeList.length > 0) {
                    $scope.InstallmentTypeId = $scope.InstallmentTypeList[0].MasterInstallmentID;
                }
                $scope.GetBeneficiaryList();
            }).
            error(function (data, status) {
                $scope.InstallmentTypeList = [];
            });
    };
    function getCurrentFinancialYear() {
        var today = new Date();
        var year = today.getFullYear();
        var month = today.getMonth() + 1; // Months are zero-based
        var startYear, endYear;
        if (month >= 4) { // April (4th month) starts the new financial year
            startYear = year;
            endYear = year + 1;
        } else {
            startYear = year - 1;
            endYear = year;
        }
        return startYear + '-' + (endYear.toString().slice(-2));
    }
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                debugger;
                if ($scope.search.DistId > 0) {
                    $scope.DistrictList = data.filter(x => x.DISTRICT_ID == $scope.search.DistId);
                    $scope.selectedDistrictId = data.filter(x => x.DISTRICT_ID == $scope.search.DistId)[0].DISTRICT_ID;
                    $scope.search.districtId = $scope.selectedDistrictId;
                    $scope.GetSubDistrictList();
                }
                else {
                    $scope.DistrictList = data;                                     
                    $scope.search.subDistrictName = '';
                    $scope.search.villageName = '';
                }

            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };



    $scope.GetSubDistrictList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CM_SUB_DISTRICT&MasterCode=" + $scope.search.districtId).
            success(function (data, status) {
                $scope.SubDistrictList = data;
                $scope.search.subDistrictName = '';
                $scope.search.villageName = '';
                $scope.GetBeneficiaryList();
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.SubDistrictList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.GetVillageList = function () {
        $scope.GetBeneficiaryList();     
    };
    $scope.GetPMKisanMasterData = function () {
        $http.get("/Master/GetMasterData/?MasterType=CMKISAN&MasterCode=4").
            success(function (data, status) {
                $scope.FinancialYearList = data;   
                var currentFY = getCurrentFinancialYear();
                var found = $scope.FinancialYearList.find(function (FY) {
                    return FY.TitleEn === currentFY;
                });
                if (found) {
                    $scope.FinancialYearId = found.CMK_CommonMasterId;
                } else {
                    $scope.FinancialYearId = '0';
                }
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    };
    $scope.GetBeneficiaryList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.BeneficiaryList = [];
        $scope.TotalBeneficiaryList = [];
        $http.get(SiteUrl + "/MIS/GetBeneficiaryList/?districtId=" + ($scope.search.districtId ?? 0) ).
            success(function (data, status) {
                $scope.BeneficiaryList = JSON.parse(data.Result.Data.BeneficiaryList);
                $scope.TotalBeneficiaryList = JSON.parse(data.Result.Data.TotalBeneficiaryList);
                $scope.colsort = "";
                $scope.reverseSort = false;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.OnPageLoad();
    $scope.GetDistrictList();
    var hdnDistId = $('#hdnDistId').val();
    if (hdnDistId > 0) {
        $scope.isLBDisabled = true;
        $scope.search.DistId = hdnDistId
    } else
        $scope.isLBDisabled = false;
    
});

MasterMod.filter('inrNumber', function () {
    return function (input) {
        if (!input) {
            return input;
        }
        var numberString = input.toString();
        var lastThree = numberString.substring(numberString.length - 3);
        var otherNumbers = numberString.substring(0, numberString.length - 3);
        if (otherNumbers !== '') {
            lastThree = ',' + lastThree;
        }
        var result = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
        return result;
    };
});

MasterMod.filter('inrCurrency', function () {
    return function (input) {
        if (!input) return input;

        var num = parseFloat(input);
        if (isNaN(num)) return input;
        return num.toLocaleString('en-IN', { style: 'currency', currency: 'INR' });
    };
});
MasterMod.filter('indianFormat', function () {
    return function (input, t) {
        if (isNaN(input)) return input;

        // Convert input to a number
        input = parseFloat(input);

        if (input >= 10000000) { // 1 Crore or more
            return (t == 'R' ? '₹' : '') + (input / 10000000).toFixed(2).replace(/\.0$/, '') + ' Cr';
        } else if (input >= 100000) { // 1 Lakh to less than 1 Crore
            return (t == 'R' ? '₹' : '') + (input / 100000).toFixed(2).replace(/\.0$/, '') + ' Lac';
        }
        //else if (input >= 1000) { // 1 Thousand to less than 1 Lakh
        //	return (input / 1000).toFixed(2).replace(/\.0$/, '') + ' K';
        //}
        else {
            return (t == 'R' ? '₹' : '') + input; // Less than 1 Thousand
        }
    };
});
