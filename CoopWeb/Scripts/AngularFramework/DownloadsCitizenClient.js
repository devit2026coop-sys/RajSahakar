var MasterMod = angular.module('CitizenDownloadMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("CitizenDownloadCnt", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.Societylist = [];
    $scope.SocietyIdENC = 0;
    $scope.DLSCFileName = '';
    $scope.SOCFileName = '';
    $scope.RegistrationCertificateFile = '';
    $scope.BylawsFile = '';
    $scope.RulesAndRegulationFile = '';
    $scope.RSSCFileName = '';
    $scope.MemberListFile = '';
    $scope.BankOpeningCertificate = '';
    $scope.FinancialYear = 0;


    $scope.GetSocietylist = function () {
        $scope.Societylist = [];
        $http.post(SiteUrl + "/Citizen/CitizenDashboardData", { "ActId": 0, "SocietyTypeId": 0, "ClassId": 0 }).
            success(function (data, status) {
                $scope.Societylist = data.DataList;
                if (data.length == 1)
                {
                    $scope.SocietyIdENC = data[0].SocietyIdENC;
                    $scope.GetSocietyDownloadList();
                }
            }).
            error(function (data, status) {
                $scope.Societylist = [];
            });
    }

    $scope.ShowYearlyData = function () {
        $http.post(SiteUrl + "/Citizen/GetSocietyFileByFinancialYear", { "SocietyIdENC": $scope.SocietyIdENC, "FYearID": $scope.FinancialYear }).
            success(function (data, status) {
                if (data.length > 0)
                {
                    $scope.YearlyDataList = data;
                    $scope.FYearData = true;
                }
                else
                {
                    alert("Data not available.");
                    $scope.FYearData = false;
                    $scope.YearlyDataList = [];
                }
            }).
            error(function (data, status) {
                $scope.YearlyDataList = [];
            });
    };

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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

            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }

        //GET Financial Year
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    $scope.GetSocietyDownloadList = function () {
        $scope.DLSCFileName = '';
        $scope.SOCFileName = '';
        $scope.RegistrationCertificateFile = '';
        $scope.BylawsFile = '';
        $scope.RulesAndRegulationFile = '';
        $scope.RSSCFileName = '';
        $scope.MemberListFile = '';
        $scope.BankOpeningCertificate = '';
        $http.get(SiteUrl + "/Citizen/GetSocietyDownloadList/?SocietyIdENC=" + $scope.SocietyIdENC).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.DLSCFileName = data[0].DLSCFileName;
                    $scope.SOCFileName = data[0].SOCFileName;
                    $scope.RegistrationCertificateFile = data[0].RegistrationCertificateFile;
                    $scope.BylawsFile = data[0].BylawsFile;
                    $scope.RulesAndRegulationFile = data[0].RulesAndRegulationFile;
                    $scope.RSSCFileName = data[0].RSSCFileName;
                    $scope.MemberListFile = data[0].MemberListFile;
                    $scope.BankOpeningCertificate = data[0].BankOpeningCertificate;
                }
                $scope.colsort = "";
                $scope.reverseSort = false;
                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                    }
                    return '';
                };
            }).
            error(function (data1, status) {
            });
    }

    $scope.OnPageLoad();
    $scope.GetSocietylist();
});