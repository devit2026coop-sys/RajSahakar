
var MasterMod = angular.module('gbdbtApp', ['angularUtils.directives.dirPagination']);
MasterMod.controller("gbdbtCnt", function ($rootScope, $scope, $http) {

    $scope.cmKisanMasterDataList = [];

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

    $scope.GetCMKisanMasterData = function () {
        $http.get("/Master/GetMasterData/?MasterType=CMKisan").
            success(function (data, status) {
                $scope.cmKisanMasterDataList = data;
                $scope.SchemeList = $scope.cmKisanMasterDataList.filter(x=>x.MasterType==1);
                $scope.QuadrimesterList = $scope.cmKisanMasterDataList.filter(x=>x.MasterType==3);
                $scope.FinancialYearList = $scope.cmKisanMasterDataList.filter(x=>x.MasterType==4);
            }).
            error(function (data, status) {
                $scope.cmKisanMasterDataList = [];
            });

    };
    $scope.GenerateBeneficiaryListDBT = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post("/PMKisan/GenerateBeneficiaryListDBT", $scope.objRFT).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                console.log(data);
                $scope.Get_RFTDBT_List();
            }).
            error(function (data, status) {
                console.log(data);
                $(".se-pre-con").fadeOut("slow");
            });

    };

    $scope.Get_RFTDBT_List = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post("/PMKisan/Get_RFTDBT_List").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.rbtDbtList = JSON.parse(data.Result.Data);
            }).
            error(function (data, status) {
                console.log(data);
                $(".se-pre-con").fadeOut("slow");
            });

    };

    $scope.GetInstallmentType();
    $scope.GetCMKisanMasterData();
    $scope.Get_RFTDBT_List();
    $scope.OnPageLoad();
});
