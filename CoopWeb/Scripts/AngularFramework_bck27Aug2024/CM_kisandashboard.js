
var MasterMod = angular.module('CM_KisanDashboardApp', ['angularUtils.directives.dirPagination']);
MasterMod.controller("CM_KisanDashboardCnt", function ($rootScope, $scope, $http) {

    $scope.PMKisanMasterDataList = [];
    $scope.DashboardCount = {};

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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
       // $scope.GetFinancialYear();

        $(".se-pre-con").fadeOut("slow");
    };


    $scope.GetFinancialYear = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.FinancialYearList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;
                $scope.FinancialYearList = $scope.FinancialYearList.filter(function (x) { return x.FIN_YEAR_MASTR_ID < 38 || x.FIN_YEAR_MASTR_ID > 47 });
                $(".se-pre-con").fadeOut("slow");
                $scope.FinancialYearList.shift();
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetInstallmentType = function () {
        $http.get("/Master/GetMasterData/?MasterType=CMInstallmentType").
            success(function (data, status) {
                $scope.InstallmentTypeList = data;
            }).
            error(function (data, status) {
                $scope.InstallmentTypeList = [];
            });

    };

    $scope.GetPMKisanMasterData = function () {
        $http.get("/Master/GetMasterData/?MasterType=PMKisan").
            success(function (data, status) {
                $scope.PMKisanMasterDataList = data;
                $scope.SchemeList = $scope.PMKisanMasterDataList.filter(x=>x.MasterType==1);
                $scope.ActionList = $scope.PMKisanMasterDataList.filter(x=>x.MasterType==2);
                $scope.QuadrimesterList = $scope.PMKisanMasterDataList.filter(x=>x.MasterType==3);
                $scope.FinancialYearList = $scope.PMKisanMasterDataList.filter(x=>x.MasterType==4);
            }).
            error(function (data, status) {
                $scope.PMKisanMasterDataList = [];
            });

    };
    $scope.GetPMK_DashboardData = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/PMKisan/GetPMK_DashboardData?financialYearId=0").
            success(function (data, status) {
                $scope.Dashboard = JSON.parse(data.Result.Data.Dashboard)[0];
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.GetInstallmentType();
    $scope.GetPMKisanMasterData();
    $scope.OnPageLoad();
    $scope.GetPMK_DashboardData();
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
    return function (input,t) {
        if (isNaN(input)) return input;

        // Convert input to a number
        input = parseFloat(input);

        if (input >= 10000000) { // 1 Crore or more
            return (t == 'R' ?'₹':'') + (input / 10000000).toFixed(2).replace(/\.0$/, '') + ' Cr';
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