
var MasterMod = angular.module('grftApp', ['angularUtils.directives.dirPagination']);
MasterMod.controller("grftCnt", function ($rootScope, $scope, $http) {

    $scope.PMKisanMasterDataList = [];

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

    $scope.GetInstallmentType();
    $scope.GetPMKisanMasterData();
    $scope.OnPageLoad();
});
