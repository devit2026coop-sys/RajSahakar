
var MasterMod = angular.module('BeneficiaryListMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("BeneficiaryListCnt", function ($scope, $http) {
    // SCOPE
    $scope._language = 'hi-in';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.search = {};
    $scope.divClass = false;
    $scope.DistrictList = [];
    $scope.GetPldbSummary = [];
    $scope.TotalGetPldbSummary = []; $scope.BankMasterList = []; $scope.BankBranches = [];
    $scope.search.LoanDistrictId = '0';
    $scope.isLBDisabled = false;
    $scope.ReportType = 1;
    $scope.StatusType = 3;

    $scope.Islinkcalled = false;
    // on page load
    $scope.OnPageLoad = function () {
        //$scope._language = _language;
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
        $scope.GetDistrictWithoutDivisionList();
    };

    $scope.GetDistrictWithoutDivisionList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;

                if ($("#hdnDistrictId").val() != "0") {
                    $scope.search.DISTRICT_ID = $("#hdnDistrictId").val();
                    $scope.search.PLDB_ID = "0";
                    $scope.search.PLDB_Branch_ID = "0";                    
                    $("#DISTRICT_ID").attr("disabled", "disabled");
                    $scope.PLDBListData();                            
                }
                
                if ( $scope.Islinkcalled == false ) { $scope.GetPldbDistrictWiseSummary(); }           
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };

    $scope.PLDBList = [];
    $scope.PLDBListData = function () {
        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0";

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=" + ($scope.search.DISTRICT_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBList = data;
            }).
            error(function (data, status) {
                $scope.PLDBList = [];
            });
    };
    $scope.PLDBBranches = [];
    $scope.PLDBBranchList = function (branch_ID) {
        $scope.search.PLDB_Branch_ID = "0";
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + ($scope.search.PLDB_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBBranches = data;
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.Get_LoanBranch_PACS = function (branch_ID) {
        $scope.GetPldbDistrictWiseSummary();
    }

    $scope.GetPldbDistrictWiseSummary = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.GetPldbSummary = [];
        $scope.TotalGetPldbSummary = [];
        var Action = "";
        var Case = "";

        if ($scope.ReportType == '1') { Action = 'PldbDistrictWiseSummary'; }
        else if ($scope.ReportType == '2') { Action = 'PldbBankWiseSummary'; }
        else if ($scope.ReportType == '3') { Action = 'PldbBankWiseTaamilSummary'; }
        else if ($scope.ReportType == '4') { Action = 'PldbBranchWiseTaamilSummary'; }

        if ($scope.StatusType == '1') { Case ='Normal'}
        else if ($scope.StatusType == '2') { Case ='103'}
        else if ($scope.StatusType == '3') { Case = 'Both' }
       

        $http.get(SiteUrl + "/MIS/GetPldbTamilSelfieSummary/?financialYearId=" + ($scope.FinancialYearId ?? 0) + "&districtId=" + ($scope.search.DISTRICT_ID ?? 0) + "&PLDB_ID=" + ($scope.search.PLDB_ID ?? 0) + "&PLDB_Branch_ID=" + ($scope.search.PLDB_Branch_ID ?? 0) + "&Type=" + Action + "&StatusType=" + Case).
            success(function (data, status) {

                if (data.Result.Data != null && data.Result.Data.GetPldbSummary != null) {
                    $scope.GetPldbSummary = JSON.parse(data.Result.Data.GetPldbSummary);
                    $scope.TotalGetPldbSummary = JSON.parse(data.Result.Data.TotalGetPldbSummary);
                    $scope.colsort = "";
                    $scope.reverseSort = false;
                }

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.IsOverall = false;
    $scope.GopalCreditCardModel = {};
    $scope.GopalCreditCardModel.IsPendancy = 0;
    $scope.GridShow = function (val) {
        debugger;
        if (val == 0) {
            $scope.IsOverall = false;
        }
        else {
            $scope.IsOverall = true;

        }
        $scope.GetPldbDistrictWiseSummary();
    }


    $scope.PldbNameChange = function (item)
    {
        $scope.Islinkcalled = true;
        $scope.search.DISTRICT_ID = item.DISTRICT_ID;
        $scope.PLDBListData();
        $scope.search.PLDB_ID = item.PldbId;  
        $scope.GetPldbDistrictWiseSummary();
    }
    $scope.ReportTypeChange = function () {

        if ($("#hdnDistrictId").val() != '0') { $scope.search.DISTRICT_ID = "0"; }
        if ($("#hdnPldbId").val() != '0') { $scope.search.PLDB_ID = "0"; }
        if ($("#hdnPldbBranchId").val() != '0') { $scope.search.PLDB_Branch_ID = "0"; }

        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0"; 
        $scope.GetPldbDistrictWiseSummary();
    }
    $scope.PldbBranchNameChange = function (item) {
        $scope.Islinkcalled = true;
        $scope.search.DISTRICT_ID = item.DISTRICT_ID;
        $scope.PLDBListData();
        $scope.search.PLDB_ID = item.PldbId;
        $scope.PLDBBranchList();
        $scope.search.PLDB_Branch_ID = item.BranchId;
        $scope.GetPldbDistrictWiseSummary();
    }
    
    $scope.BackToPage = function () {
        window.location.reload();
    }
    $scope.OnPageLoad();

});
MasterMod.filter('inrCurrency', function () {
    return function (input) {
        if (!input)
            return input;
        var num = parseFloat(input);
        if (isNaN(num)) return input;
        return num.toLocaleString('en-IN', { style: 'currency', currency: 'INR' });
    };
});