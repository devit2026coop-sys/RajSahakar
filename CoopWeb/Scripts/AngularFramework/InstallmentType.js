var CitizenModule = angular.module("InstallmentMod", []);
CitizenModule.controller("InstallmentCnt", function ($scope, $http) {
    $scope.GetInstallmentType = function () {
        $http.get("/Master/GetMasterData/?MasterType=CMInstallmentType").
            success(function (data, status) {
                $scope.InstallmentTypeList = data;
            }).
            error(function (data, status) {
                $scope.InstallmentTypeList = [];
            });

    }; 
    $scope.GetCMKisanFinancialYearMasterData = function () {
        $http.get("/Master/GetMasterData/?MasterType=PMKisan&MasterCode=4").
            success(function (data, status) {
                $scope.FinancialYearList = data;
              
            }).
            error(function (data, status) {
                $scope.PMKisanMasterDataList = [];
            });

    };

    $scope.GetInstallmentType();
    $scope.GetCMKisanFinancialYearMasterData();

});

