var MasterMod = angular.module('ReApplySummaryListMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ReApplySummaryListCnt", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.search = {};
    $scope.divClass = false;
    $scope.DistrictList = [];
    $scope.GccSummary = [];
    $scope.TotalGccSummary = []; $scope.BankMasterList = []; $scope.BankBranches = [];
    $scope.search.LoanDistrictId = '0';
    $scope.isLBDisabled = false;

  

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
        //$scope.GetDistrictWithoutDivisionList();
        $scope.LoanBankList();
        $scope.FinancialYearList();
        var hdnDistId = $('#hdnDistrictId').val();
        if (hdnDistId > 0) {
            $scope.isLBDisabled = true;
            $scope.search.LoanDistrictId = hdnDistId
        } else
            $scope.isLBDisabled = false;
    };

    //GET Financial Year
    $scope.FinancialYearList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    }


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

    $scope.GetDistrictWithoutDivisionList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                $scope.search.LoanBankID = '0';
                $scope.search.LoanBranchId = '0';
                $scope.GetGccReApplySummary();
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };

    $scope.LoanBankList = function () {
        $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=" + $scope.search.LoanDistrictId).
            success(function (data, status) {
                if ($scope.search.LoanDistrictId > 0) {
                    $scope.BankMasterList = data.filter(x => x.DistrictId == $scope.search.LoanDistrictId);
                    $scope.selectedCCBId = data.filter(x => x.DistrictId == $scope.search.LoanDistrictId)[0].CCBId;
                    $scope.search.LoanBankID = $scope.selectedCCBId;
                    $scope.Get_LoanBankBranch($scope.selectedCCBId);
                }
                else {
                    $scope.BankMasterList = data;
                    $scope.search.LoanBankID = '0';
                    $scope.search.LoanBranchId = '0';
                }
                $scope.GetGccReApplySummary();
            }).
            error(function (data, status) {
                $scope.BankMasterList = [];
            });
    };

    $scope.Get_LoanBankBranch = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBankBranch/?LoanDistrictId=" + $scope.search.LoanDistrictId + "&LoanBankId=" + $scope.search.LoanBankID).
            success(function (data, status) {
                $scope.BankBranches = data;
                $scope.search.LoanBranchId = '0';
                $scope.GetGccReApplySummary();
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.BankBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.Get_LoanBranch_PACS = function (branch_ID) {
        $scope.GetGccReApplySummary();
    }

    $scope.GetGccReApplySummary = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.GccRenewSummary = [];
        $scope.TotalGccRenewSummary = [];
        $http.get(SiteUrl + "/MIS/GetGccRenewSummary/?financialYearId=" + ($scope.search.FinancialYearId ?? 0) + "&districtId=" + ($scope.search.LoanDistrictId ?? 0) + "&CCBId=" + ($scope.search.LoanBankID ?? 0) + "&BranchId=" + ($scope.search.LoanBranchId ?? 0) + "&PacsId=" + ($scope.search.PacsId ?? 0) + "&Overall=" + $scope.IsOverall).
            success(function (data, status) {

                if (data.Result.Data != null && data.Result.Data.GccRenewSummary != null) {
                    $scope.GccRenewSummary = JSON.parse(data.Result.Data.GccRenewSummary);
                    $scope.TotalGccRenewSummary = JSON.parse(data.Result.Data.TotalGccRenewSummary);
                    /*	$scope.TotalBeneficiaryList = JSON.parse(data.Result.Data.TotalBeneficiaryList);*/
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
        $scope.GetGccReApplySummary();
    }


    $scope.OnPageLoad();

});
