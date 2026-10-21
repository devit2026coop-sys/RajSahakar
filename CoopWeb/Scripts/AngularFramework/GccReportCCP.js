
var MasterMod = angular.module('BeneficiaryListMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("BeneficiaryListCnt", function ($scope, $http) {
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
    $scope.FromDate = 0;
    $scope.ToDate = 0;

  

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
        $scope.GopalCreditCardModel.Type = 0;
        $scope.LoanBankList();
        $scope.FinancialYearList();
      /*  $scope.GridShow();*/
        var hdnDistId = $('#hdnDistrictId').val();
        if (hdnDistId > 0) {
            $scope.isLBDisabled = true;
            $scope.search.LoanDistrictId = hdnDistId
        } else
            $scope.isLBDisabled = false;
    };

    
    //GET Financial Year
    $scope.FinancialYearList = function () {
        debugger
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;

                // Set default year here
                //angular.forEach($scope.FinancialYearList, function (item) {
                //    if (item.FIN_YEAR_NAME === '2025-26') {   // change to 2025-26 if needed
                //        $scope.search.FinancialYearId = item.FIN_YEAR_MASTR_ID;
                //    }
                //});
                angular.forEach($scope.FinancialYearList, function (item) {
                    //if (item.FIN_YEAR_MASTR_ID == $scope.search.FinancialYearId) {
                    if (item.FIN_YEAR_NAME === '2025-26') {
                        $scope.search.FinancialYearId = item.FIN_YEAR_MASTR_ID;
                        $scope.FromDate = item.FromDate;
                        $scope.ToDate = item.ToDate;
                    }
                });

           
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    }


    $scope.ConstituencyList = [];
    $scope.GetConstituencyList = function () {
        if ($("#district_MemberAdd").val() != "" && $("#district_MemberAdd").val() != undefined) {

            $http.post(SiteUrl + "/FertilizerSupply/GetConstituencyListByDistrictId", { DistrictId: $('#district_MemberAdd').val() }).
                success(function (data, status) {
                    $scope.ConstituencyList = data;
                }).
                error(function (data, status) {
                    $scope.ConstituencyList = [];
                    alert('Error while fetching Constituency List  data.');
                });
        }
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

    $scope.GetDistrictWithoutDivisionList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                $scope.search.LoanBankID = '0';
                $scope.search.LoanBranchId = '0';
                $scope.GetGccSummary();
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
                $scope.GetGccSummary();
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
                $scope.GetGccSummary();
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.BankBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.Get_LoanBranch_PACS = function (branch_ID) {
        $scope.GetGccSummary();
    }

    $scope.GetGccSummary = function () {
        debugger
        $(".se-pre-con").fadeIn("slow");
        $scope.GccSummary = [];
        $scope.TotalGccSummary = [];
          $scope.FromDate = $('#txtFromDate').val();
        $scope.ToDate = $('#txtToDate').val();

        /*$http.get(SiteUrl + "/MIS/GetCCPGccSummary/?financialYearId=" + ($scope.search.FinancialYearId ?? 0) + "&districtId=" + ($scope.search.LoanDistrictId ?? 0) + "&CCBId=" + ($scope.search.LoanBankID ?? 0) + "&BranchId=" + ($scope.search.LoanBranchId ?? 0) + "&PacsId=" + ($scope.search.PacsId ?? 0) + "&Overall=" + $scope.IsOverall + "FromDate" + $scope.FromDate, +"ToDate" + $scope.ToDate, + "&Type=" + $scope.GopalCreditCardModel.Type  )*/
        $http.get(SiteUrl + "/MIS/GetCCPGccSummary/?financialYearId=" + ($scope.search.FinancialYearId ?? 0) + "&districtId=" + ($scope.search.LoanDistrictId ?? 0) + "&CCBId=" + ($scope.search.LoanBankID ?? 0) + "&BranchId=" + ($scope.search.LoanBranchId ?? 0) + "&PacsId=" + ($scope.search.PacsId ?? 0) + "&Overall=" + $scope.IsOverall + "&FromDate=" + $scope.FromDate + "&ToDate=" + $scope.ToDate + "&Type=" + $scope.GopalCreditCardModel.Type
             ).success(function (data, status) {
                debugger

            if (data.Result.Data != null && data.Result.Data.GccSummary != null) {
                 $scope.GccSummary = typeof data.Result.Data.GccSummary === "string"
                     ? JSON.parse(data.Result.Data.GccSummary)
                     : data.Result.Data.GccSummary;

                 $scope.TotalGccSummary = typeof data.Result.Data.TotalGccSummary === "string"
                     ? JSON.parse(data.Result.Data.TotalGccSummary)
                     : data.Result.Data.TotalGccSummary;

                 console.log($scope.GccSummary);

                    //$scope.GccSummary = JSON.parse(data.Result.Data.GccSummary);
                    //console.log(data.Result.Data.GccSummary);
                    //$scope.TotalGccSummary = JSON.parse(data.Result.Data.TotalGccSummary);
                    ///*	$scope.TotalBeneficiaryList = JSON.parse(data.Result.Data.TotalBeneficiaryList);*/
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
    $scope.Type = false;
    $scope.GopalCreditCardModel = {};
    $scope.GopalCreditCardModel.IsPendancy = 0;
    //$scope.GridShow = function (val) {

    //    $scope.GopalCreditCardModel.Type = val;

    //    // Same functionality, just renamed
    //    $scope.IsNew = (val == 0);
    //    $scope.IsRenew = (val == 1);

    //    $scope.GetGccSummary();
    //}
    //$scope.GridShow = function (val) {
    //    $scope.GopalCreditCardModel.Type = val; // 0 or 1
    //    $scope.GetGccSummary();
    //}
    //$scope.GridShow = function (val) {
    //    if ($scope.GopalCreditCardModel.Type == val) {
    //        $scope.GetGccSummary(); // force reload
    //    } else {
    //        $scope.GopalCreditCardModel.Type = val;
    //        $scope.GetGccSummary();
    //    }
    //}
    //$scope.GridShow = function (val) {
    //    debugger
    //    if (val == 0) {
    //        $scope.GopalCreditCardModel.Type = false;
    //    }
    //    else {
    //        $scope.GopalCreditCardModel.Type = true;

    //    }
    //    $scope.GetGccSummary();
    //}
    //$scope.GridShow = function (val) {
    //    if (val == 0) {
    //        $scope.IsOverall = false;
    //    }
    //    else {
    //        $scope.IsOverall = true;

    //    }
    //    $scope.GetGccSummary();
    //}

    // Call on page load
    // $scope.GetStatusList();

    //// created by jaya
    //$scope.exportAllToExcel = function () {

    //    //var params = {
    //    //    financialYearId: $scope.search.FinancialYearId,
    //    //    districtId: $scope.search.DistrictId,
    //    //    CCBId: $scope.search.CCBId,
    //    //    BranchId: $scope.search.BranchId,
    //    //    PacsId: $scope.search.PacsId,
    //    //    Overall: $scope.search.Overall,
    //    //    DistrictWise: $scope.search.DistrictWise,
    //    //    blockId: $scope.search.BlockId,
    //    //    FromDate: formatDate($scope.FromDate),
    //    //    ToDate: formatDate($scope.ToDate),
    //    //    Type: $scope.search.Type   // ✅ IMPORTANT
    //    //};

    //    var params = {
    //        Details: {
    //            PageNumber: 1,
    //            PageSize: 100000,
    //            Type: $scope.search.Type   // ✅ IMPORTANT
    //        },
    //        //FromDate: formatDate($scope.FromDate),
    //        //ToDate: formatDate($scope.ToDate)
    //    };

    //    $http({
    //        url: SiteUrl + "/MIS/GetCCPGccSummary_Export", // ✅ NEW API
    //        method: "POST",
    //        data: params
    //    }).then(function (response) {

    //        var data = response.data;

    //        if (!Array.isArray(data)) {
    //            alert("Invalid data for Excel export");
    //            return;
    //        }

    //        var worksheet = XLSX.utils.json_to_sheet(data);
    //        var workbook = XLSX.utils.book_new();

    //        XLSX.utils.book_append_sheet(workbook, worksheet, "Report");

    //        XLSX.writeFile(workbook, "Society_Report.xlsx");

    //    }, function (error) {
    //        console.error(error);
    //        alert("Export failed.");
    //    });
    //};


   /* Export*/
    $scope.exportAllToExcel = function () {

        var params = {
            financialYearId: $scope.search.FinancialYearId || 0,
            districtId: $scope.search.DistrictId || 0,
            CCBId: $scope.search.LoanBankID || 0,
            BranchId: $scope.search.LoanBranchId || 0,
            PacsId: $scope.search.PacsId || 0,
            blockId: $scope.search.BlockId || 0,

            FromDate: $scope.FromDate || null,
            ToDate: $scope.ToDate || null,

            Type: $scope.GopalCreditCardModel.Type || 0   // 🔥 IMPORTANT
        };

        $http({url: SiteUrl + "/MIS/GetCCPGccSummary_Export", method: "POST",data: params

            }).then(function (response) {

            var data = response.data;

            if (!Array.isArray(data)) {
                alert("Invalid data for Excel export");
                return;
            }

            var worksheet = XLSX.utils.json_to_sheet(data);
            var workbook = XLSX.utils.book_new();

            XLSX.utils.book_append_sheet(workbook, worksheet, "Report");

            XLSX.writeFile(workbook, "Society_Report.xlsx");

        }, function (error) {
            console.error(error);
            alert("Export failed.");
        });
    };
    

    $scope.OnPageLoad();

});
