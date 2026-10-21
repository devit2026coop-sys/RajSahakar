
var MasterMod = angular.module('BeneficiaryListMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("BeneficiaryListCnt", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.search = {};
    $scope.divClass = false;
    $scope.DistrictList = [];
    $scope.GetPldbSummary = [];
    $scope.TotalGetPldbSummary = []; $scope.BankMasterList = []; $scope.BankBranches = [];
    $scope.search.LoanDistrictId = '0';
    $scope.isLBDisabled = false;

    $scope.ReportType = 4;
    $scope.Islinkcalled = false;
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
        $scope.PLDBListData();

        //$scope.LoanBankList();
        //var hdnDistId = $('#hdnDistrictId').val();
        //if (hdnDistId > 0) {
        //    $scope.isLBDisabled = true;
        //    $scope.search.LoanDistrictId = hdnDistId
        //} else
        //    $scope.isLBDisabled = false;
    };

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

    //$scope.GetDistrictWithoutDivisionList = function () {
    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
    //        success(function (data, status) {
    //            $scope.DistrictList = data;

    //            if ($("#hdnDistrictId").val() != "0") {
    //                $scope.search.DISTRICT_ID = $("#hdnDistrictId").val();
    //                $scope.search.PLDB_ID = "1";
    //                $scope.search.PLDB_Branch_ID = "1";                    
    //                $("#DISTRICT_ID").attr("disabled", "disabled");
    //                $scope.PLDBListData();                            
    //            }
                
    //            if ($scope.Islinkcalled == false) { $scope.GetPldbBankWiseSummary(); }           
    //        }).
    //        error(function (data, status) {
    //            $scope.DistrictList = [];
    //        });
    //};

    //$scope.BlockListByOffAdd = function (id) {
    //    $(".se-pre-con").fadeIn("slow");
    //    $scope.Blocksdata = [];
    //    $scope.GramPanchayats = [];
    //    $scope.Villages = [];
    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.search.DISTRICT_ID).
    //        success(function (data, status) {
    //            $(".se-pre-con").fadeOut("slow");
    //            $scope.Blocksdata = data;
    //            $scope.GetPldbDistrictWiseSummary();
    //        }).
    //        error(function (data, status) {
    //            $scope.Blocksdata = [];
    //            $(".se-pre-con").fadeOut("slow");
    //        });
    //};
   


    $scope.PLDBList = [];
    $scope.PLDBListData = function () {
        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0";

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=" + ($scope.search.DISTRICT_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBList = data;

                //if ($("#hdnPldbId").val() != "0") {
                //    $scope.search.PLDB_ID = $("#hdnPldbId").val();
                //    $scope.PLDBBranchList();
                //    $("#PLDB_ID").attr("disabled", "disabled");
                //}
                //if ($("#hdnPldbBranchId").val() == "0" && $scope.Islinkcalled== false) { $scope.GetPldbBranchWiseTaamilSummary(); }
               
            }).
            error(function (data, status) {
                $scope.PLDBList = [];
            });
    };

    $scope.PLDBBranches = [];
    //$scope.PLDBBranchList = function (branch_ID) {
    //    $scope.search.PLDB_Branch_ID = "0";
    //    $(".se-pre-con").fadeIn("slow");
    //    return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + ($scope.search.PLDB_ID ?? 0)).
    //        success(function (data, status) {
    //            $scope.PLDBBranches = data;

    //            //if ($("#hdnPldbBranchId").val() != "0") {
    //            //    $scope.search.PLDB_Branch_ID = $("#hdnPldbBranchId").val();
    //            //    $("#PLDB_Branch_ID").attr("disabled", "disabled");

    //            //    if($scope.Islinkcalled == false) { $scope.GetPldbBranchWiseTaamilSummary(); }
    //            //}
    //            // $(".se-pre-con").fadeOut("slow");
                
    //        }).
    //        error(function (data, status) {
    //            $scope.PLDBBranches = [];
    //            $(".se-pre-con").fadeOut("slow");
    //        });
    //};


    $scope.Get_LoanBranch_PACS = function (branch_ID) {
        $scope.GetPldbBankWiseSummary();
    }

    $scope.GetPldbBranchWiseTaamilSummary = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.GetPldbSummary = [];
        $scope.TotalGetPldbSummary = [];

        var Action = "";

        if ($scope.ReportType == '1') { Action = 'PldbDistrictWiseSummary'; }
        else if ($scope.ReportType == '2') { Action = 'PldbBankWiseSummary'; }
        else if ($scope.ReportType == '3') { Action = 'PldbBankWiseTaamilSummary'; }
        else if ($scope.ReportType == '4') { Action = 'PldbBranchWiseTaamilSummary'; }

        $http.get(SiteUrl + "/MIS/GetPldbDistrictWiseSummary/?financialYearId=" + ($scope.FinancialYearId ?? 0) + "&districtId=" + ($scope.search.DISTRICT_ID ?? 0) + "&PLDB_ID=" + ($scope.search.PLDB_ID ?? 0) + "&PLDB_Branch_ID=" + ($scope.search.PLDB_Branch_ID ?? 0) + "&Type=" + Action).
            success(function (data, status) {

                if (data.Result.Data != null && data.Result.Data.GetPldbSummary != null) {
                    $scope.GetPldbSummary = JSON.parse(data.Result.Data.GetPldbSummary);
                    $scope.TotalGetPldbSummary = JSON.parse(data.Result.Data.TotalGetPldbSummary);
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
        $scope.GetPldbBankWiseSummary();
    }


    $scope.PldbNameChange = function (item)
    {
        $scope.Islinkcalled = true;
        $scope.search.DISTRICT_ID = item.DISTRICT_ID;
        //$scope.PLDBListData();
        $scope.search.PLDB_ID = item.PldbId;  
        $scope.GetPldbBankWiseSummary();
    }
    $scope.ReportTypeChange = function () {

        if ($("#hdnDistrictId").val() != '0') { $scope.search.DISTRICT_ID = "0"; }
        if ($("#hdnPldbId").val() != '0') { $scope.search.PLDB_ID = "0"; }
        if ($("#hdnPldbBranchId").val() != '0') { $scope.search.PLDB_Branch_ID = "0"; }

        $scope.search.PLDB_ID = "0";
        $scope.search.PLDB_Branch_ID = "0"; 
        $scope.GetPldbBankWiseSummary();
    }
    $scope.PldbBranchNameChange = function (item) {
        $scope.Islinkcalled = true;
        $scope.search.DISTRICT_ID = item.DISTRICT_ID;
        //$scope.PLDBListData();
        $scope.search.PLDB_ID = item.PldbId;
       // $scope.PLDBBranchList();
        $scope.search.PLDB_Branch_ID = item.BranchId;
        
        $scope.GetPldbBankWiseSummary();

        
    }
    
    $scope.BackToPage = function () {
        window.location.reload();

    }
    $scope.OnPageLoad();

});
