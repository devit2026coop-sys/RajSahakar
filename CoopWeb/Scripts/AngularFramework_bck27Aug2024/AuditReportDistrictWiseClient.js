
var AuditReportDistrictWiseMod = angular.module('AuditReportDistrictWiseModule', ['zingchart-angularjs']);
AuditReportDistrictWiseMod.controller("AuditReportDistrictWiseController", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.GetDistrictData = false;
    $scope.IsDivision = false;

    $scope.DivisionChange = function () {
        $scope.AuditReport.YearId = '0';
        $scope.AuditReport.MonthId = '0';
        $scope.DistrictReport = [];
        $scope.tblDivisionWiseData = false;
    }

    $scope.YearChange = function () {
        $scope.AuditReport.MonthId = '0';
        $scope.DistrictReport = [];
        $scope.tblDivisionWiseData = false;
    }

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

        $scope.DivisionList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DIVISION&MasterCode=-1").
            success(function (data, status) {
                //alert(JSON.stringify(data));
                $scope.DivisionList = data;

                $http.post(SiteUrl + "/Society/GetUseDivisionId").
                    success(function (data, status) {
                        if (data.length > 0 && data != 0) {
                            $scope.AuditReport.DivisionId = data;
                            $scope.IsDivision = true;
                        }
                    }).
                    error(function (data, status) {
                    });
            }).
            error(function (data, status) {
                $scope.DivisionList = [];

            });

        $scope.FinancialYearList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                //alert(JSON.stringify(data));
                //$scope.FinancialYearList = data.filter(x => x.FIN_YEAR_END != data[0].FIN_YEAR_END);
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];

            });

        $scope.MonthList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MONTH&MasterCode=-1").
            success(function (data, status) {
                $scope.MonthList = data;
                $scope.MonthId = new Date().getMonth();
            }).
            error(function (data, status) {
                $scope.MonthList = [];
            });

        $(".se-pre-con").fadeOut("slow");

    };


    $scope.GetDistrictData = function () {

        if ($scope.AuditReport.MonthId == '0') {
            $scope.DistrictReport = [];
            $scope.tblDivisionWiseData = false;
            return;
        }
        $http.get(SiteUrl + "/User/GetDistrictReport?FYearID=" + $scope.AuditReport.YearId + "&MonthId=" + $scope.AuditReport.MonthId + "&DivisionId=" + $scope.AuditReport.DivisionId).
            success(function (data, status) {
                try {
                    $scope.DistrictReport = data;
                    $scope.tblDivisionWiseData = true;
                }
                catch (err) {
                    $scope.DistrictReport = [];
                    alert("Error occurred in GetDistrictReport. " + err.message);
                }
            }).
            error(function (data1, status) {

            });
    }

    $scope.OnPageLoad();

});