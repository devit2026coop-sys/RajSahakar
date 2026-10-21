
var AuditReportDivisionWiseMod = angular.module('AuditReportDivisionWiseModule', ['zingchart-angularjs']);
AuditReportDivisionWiseMod.controller("AuditReportDivisionWiseController", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.tblDivisionWiseData = false;

    $scope.YearChange = function () {
        $scope.AuditReport.MonthId = '0';
        $scope.DivisionReport = [];
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

    $scope.AuditProgrssDivisionWise = function (Status) {
        if ($scope.DivisionReport.length > 0) {
            var DivData = [];
            angular.forEach($scope.DivisionReport, function (j) {
                if (Status == 'AuditableSocieties') {
                    DivData.push(
                        {
                            "DivisionName": j.DIVISION_ENG,
                            "Result": j.Auditable_Societies
                        });
                    $scope.AuditProgrssLable = 'Auditable Societies';
                }
                else if (Status == 'AuditedSocietiesTillLastMonth') {
                    DivData.push(
                        {
                            "DivisionName": j.DIVISION_ENG,
                            "Result": j.Audited_Societies_Till_Last_Month
                        });

                    $scope.AuditProgrssLable = 'Audited Societies Till Last Month';
                }
                else if (Status == 'AuditedSocietiesCurrentMonth') {
                    DivData.push(
                        {
                            "DivisionName": j.DIVISION_ENG,
                            "Result": j.Audited_Societies_Current_Month
                        });
                    $scope.AuditProgrssLable = 'Audited Societies Current Month';
                }
                else if (Status == 'AuditedSocietiesTillCurrentMonth') {
                    DivData.push(
                        {
                            "DivisionName": j.DIVISION_ENG,
                            "Result": j.Audited_Societies_Till_Current_Month
                        });
                    $scope.AuditProgrssLable = 'Audited Societies Till Current Month';
                }
                else if (Status == 'RemainingSocietiesForAudit') {
                    DivData.push(
                        {
                            "DivisionName": j.DIVISION_ENG,
                            "Result": j.Remaining_From_Audit
                        });
                    $scope.AuditProgrssLable = 'Remaining Societies For Audit';
                }

            });
            $scope.DivisionReportDataCus = DivData;
        }

    }


    $scope.GetDivisionReport = function () {

        if ($scope.AuditReport.MonthId == '0') {
            $scope.DivisionReport = [];
            $scope.tblDivisionWiseData = false;
            return;
        }
        


        $http.get(SiteUrl + "/User/GetDivisionReport?FYearID=" + $scope.AuditReport.YearId + "&MonthId=" + $scope.AuditReport.MonthId).
            success(function (data, status) {
                try {
                    var _Auditable_Societies = 0;
                    var _Audited_Societies_Till_Last_Month = 0;
                    var _Audited_Societies_Current_Month = 0;
                    var _Audited_Societies_Till_Current_Month = 0;
                    var _Remaining_From_Audit = 0;

                    $scope.DivisionReport = data;
                    angular.forEach(data, function (j) {
                        _Auditable_Societies += parseInt(j.Auditable_Societies);
                        _Audited_Societies_Till_Last_Month += parseInt(j.Audited_Societies_Till_Last_Month);
                        _Audited_Societies_Current_Month += parseInt(j.Audited_Societies_Current_Month);
                        _Audited_Societies_Till_Current_Month += parseInt(j.Audited_Societies_Till_Current_Month);
                        _Remaining_From_Audit += parseInt(j.Remaining_From_Audit);
                    });
                    $scope.AuditableSocieties = _Auditable_Societies;
                    $scope.AuditedSocietiesTillLastMonth = _Audited_Societies_Till_Last_Month;
                    $scope.AuditedSocietiesCurrentMonth = _Audited_Societies_Current_Month;
                    $scope.AuditedSocietiesTillCurrentMonth = _Audited_Societies_Till_Current_Month;
                    $scope.RemainingSocietiesForAudit = _Remaining_From_Audit;
                    $scope.AuditProgrssDivisionWise('AuditableSocieties');
                    $scope.AuditProgrssLable = 'Auditable Societies';
                    $scope.tblDivisionWiseData = true;
                    console.log($scope.DivisionReport);
                }
                catch (err) {
                    $scope.DivisionReport = [];
                    alert("Error occurred in GetDivisionReport. " + err.message);
                }
            }).
            error(function (data1, status) {

            });
    }

    $scope.OnPageLoad();

});






