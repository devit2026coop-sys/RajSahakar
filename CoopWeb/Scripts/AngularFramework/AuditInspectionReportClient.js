var AuditInspectionReportMod = angular.module('AuditInspectionReportModule', ['angularUtils.directives.dirPagination']);
AuditInspectionReportMod.controller("AuditInspectionReportCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.divInspectionReport = false;
    $scope.divSocietyDetail = false;
    $scope.btnExportToExcel = false;

    // START : Page Load
    $scope.OnPageLoad = function () {
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

        // START : GET Financial Year
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;
               // $scope.Inspection = {};
                //$scope.Inspection["FinancialYearId"] = DefaultYearId;
            }).
            error(function (data, status) {
                alert("Error while fetching Financial Year list")
                $scope.FinancialYearList = [];
            });
        // END : GET Financial Year

        // START :Get List of Districts

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=-1").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });

        // END :Get List of Districts


        // START :Get ACT List

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
            success(function (data, status) {
                $scope.ActList = data;
            }).
            error(function (data, status) {
                $scope.ActList = [];
                alert('Error while fetching district list data.');
            });

        // END :Get ACT List


        // START : Delete this Code
        $http.post(SiteUrl + "/CommonServices/MobileAppData/", { "FIN_YEAR_ID": "49" }).
            success(function (data, status) {
                console.log(data);
            }).
            error(function (data, status) {
            });
        // END : Delete this Code
    }
    // END : Page Load

    // START : Get Inspection Report
    $scope.GetInspectionReport = function (FilterId,DistrictId) {

        if ($scope.Inspection.FinancialYearId == "0") {
            var Message = _language == 'en-us' ? 'Please choose Financial Year' : 'कृपया वित्तीय वर्ष चुनें';
            alert(Message);
            return;
        }

        var _DistrictId = DistrictId == 0 ? parseInt($scope.Inspection.DistrictId, 10) : DistrictId;
        $http.post(SiteUrl + "/MIS/GetInspectionReport/", { "FinancialYearId": parseInt($scope.Inspection.FinancialYearId, 10), "ActId": parseInt($scope.Inspection.ActId, 10), "DistrictId": _DistrictId, "FilterId": parseInt(FilterId, 10) }).
            success(function (data, status) {
                if (FilterId == 0) {
                    $scope.divInspectionReport = true;
                    $scope.divSocietyDetail = false;

                    $scope.SocietyData=[]
                    $scope.InspectionReportData = data;

                    $scope.btnExportToExcel = false;
                    if (data.length > 0) {
                        $scope.btnExportToExcel = true;
                    }
                    
                }
                else {
                    if (data.length > 0) {
                        $scope.divInspectionReport = true;
                        $scope.divSocietyDetail = true;
                        $scope.SocietyData = data;
                    }
                    else {
                        $scope.divInspectionReport = true;
                        $scope.divSocietyDetail = false;
                        $scope.SocietyData = [];
                        var Message = _language == 'en-us' ? 'No Record Found' : 'कोई रिकॉर्ड नहीं मिला';
                        alert(Message);
                    }
                }
            }).
            error(function (data, status) {
                $scope.SocietyData = []
                $scope.InspectionReportData = [];
                alert('Error while fetching Inspection data.');
            });
    }
    // END : Get Inspection Report

    //START : Clear
    $scope.Clear = function () {

        $scope.divInspectionReport = false;
        $scope.divSocietyDetail = false;
        $scope.SocietyData = []
        $scope.InspectionReportData = [];
        $scope.Inspection.FinancialYearId = '0';
        $scope.Inspection.ActId = '0';
        $scope.Inspection.DistrictId = '0';
        $scope.btnExportToExcel = false;
    }
    //END : Clear

    // START : Functions on Page Load
    $scope.OnPageLoad();
    //var DefaultYearId = 1;//$scope.DistrictList[0].FIN_YEAR_MASTR_ID;
    
    //$scope.GetInspectionReport(0);
    // END : Functions on Page Load
});






