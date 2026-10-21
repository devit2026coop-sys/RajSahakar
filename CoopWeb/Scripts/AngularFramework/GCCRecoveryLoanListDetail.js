
var MasterMod = angular.module('ApplicationFlowRecoveryModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ApplicationRecoveryFlowCont", function ($scope, $rootScope, $http) {
    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 300,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }
    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else if ($(this).attr("type") == "radio" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                 //_currentPageID = 1;
                callback();
            }
            ++indx;
        });
    }
    $rootScope.SetDataPaging = function () {
        if ($rootScope.DataPagingDetails.TotalRecords > 0) {
            $rootScope.DataPagingDetails.TotalPages = parseInt(($rootScope.DataPagingDetails.TotalRecords / $rootScope.DataPagingDetails.PageSize)) + (($rootScope.DataPagingDetails.TotalRecords % $rootScope.DataPagingDetails.PageSize == 0) ? 0 : 1);
            $rootScope.DataPagingDetails.StartRecord = ($rootScope.DataPagingDetails.TotalRecords == 0) ? 0 : ((($rootScope.DataPagingDetails.PageNumber - 1) * $rootScope.DataPagingDetails.PageSize) + 1);
            $rootScope.DataPagingDetails.EndRecord = ($rootScope.DataPagingDetails.TotalPages == $rootScope.DataPagingDetails.PageNumber || $rootScope.DataPagingDetails.TotalRecords == 0) ? $rootScope.DataPagingDetails.TotalRecords : ((($rootScope.DataPagingDetails.PageNumber - 1) * $rootScope.DataPagingDetails.PageSize) + $rootScope.DataPagingDetails.PageSize);


            $rootScope.DataPagingDetails.StartPageNumber = 1;
            $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.TotalPages;
            if ($rootScope.DataPagingDetails.TotalPages > 5 && $rootScope.DataPagingDetails.PageNumber > 0) {
                $rootScope.DataPagingDetails.StartPageNumber = $rootScope.DataPagingDetails.PageNumber == 1 ? 1 : $rootScope.DataPagingDetails.PageNumber - 1;
                $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.StartPageNumber + 4;
                if ($rootScope.DataPagingDetails.EndPageNumber > $rootScope.DataPagingDetails.TotalPages) {
                    $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.TotalPages;
                    $rootScope.DataPagingDetails.StartPageNumber = $rootScope.DataPagingDetails.StartPageNumber > 4 ? $rootScope.DataPagingDetails.EndPageNumber - 4 : $rootScope.DataPagingDetails.StartPageNumber;
                }
            }
        }
        $scope.DataPaging.TotalRecords = $rootScope.DataPagingDetails.TotalRecords;
    }
    $rootScope.PagingRange = function (startPage, endPage, totalPages, step) {
        step = step || 1;
        var input = [];

        for (var tStart = startPage; tStart <= endPage && tStart <= totalPages; tStart++) {
            input.push(tStart);
        }
        return input;
    };
    $scope.searchRequest = function (callback) {
        //$scope.DataPaging.searchParameters = [];
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,input[type="date"],select,input:checkbox,input:radio').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,input[type="date"],select,input:checkbox,input:radio').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else if ($(this).attr("type") == "radio" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                $scope.DataPaging.currentPageID = 1;
                callback();
            }
            ++indx;
        });
    }

    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];
    $scope.PendingLevelList = [];
    $scope.MLPendingLevelList = [];


    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        if ($scope.Filter.FilterType == 'wrs.created_date') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;            
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }
    $scope.ClearSearch = function () {
        $scope.DISTRICT_ID = "";
        $scope.PLDB_ID = "0";
        $scope.PLDB_Branch_ID = "0";
        $scope.StatusId = "";
        $scope.q = "";
        $scope.Cases103 = "B";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        window.location = "../MIS/GCCRecoveryLoanListDetail";
    }
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;

    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0") {
                    $scope.GopalCreditCardModel.LoanDistrictId = $("#hdnDistrictId").val();                  
                    $("#ddrDistrictId").hide();
                }
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };
    $scope.GetDistrictList();

    $scope.GccLoanApplicationList = function () {
       
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        
        return $http.post(SiteUrl + "/MIS/GetGccLoanApplicationList", JSON.stringify({ Details: jsonParameters }))
            .success(function (responcedata) {             
                var data = JSON.parse(responcedata.DataList);
                $scope.OTSList = data;
                $rootScope.DataPagingDetails = responcedata.DataPaging;
                $rootScope.SetDataPaging();
                $scope.colsort = "";
                $scope.reverseSort = false;
                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $(".se-pre-con").fadeOut("slow");
                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                    }
                    return '';
                };
                $(".se-pre-con").fadeOut("slow");
            })
            .error(function () {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetCcbList = function () {
        $scope.BranchList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CCB_LIST&MasterCode=0" + ($scope.GopalCreditCardModel.LoanDistrictId ?? 0)).
            success(function (data, status) {
                $scope.BankList = data;                
                $scope.GetCcbBranchList();
                if ($scope.hdnLevelId > 0 && $scope.hdnCcbId > 0) {
                    if ($scope.hdnLevelId == 7) {
                        $scope.BankList = data.filter(x => x.CCBId == $scope.CcbIds.CCBId);
                        $scope.search.MPacsBankId = "" + $scope.CcbIds.CCBId;
                        $scope.isBankDisabled = true;
                        $scope.GetCcbBranchList();
                    }
                    else if ($scope.hdnLevelId == 8 || $scope.hdnLevelId == 9) {
                        $scope.search.MPacsBankId = "" + $scope.CcbIds.CCBId;
                        $scope.GetCcbBranchList();
                    }
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.BankList = [];
            });
    };

    $scope.GetCcbBranchList = function () {
        $(".se-pre-con").fadeIn("slow");
      
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CCB_BANK_LIST&MasterCode=" + ($scope.CCBBANK_ID ?? 0)).
            success(function (data, status) {
                $scope.BranchList = data;
                if ($scope.hdnLevelId > 0 && $scope.hdnCcbId > 0) {
                    if ($scope.hdnLevelId == 8) {
                        $scope.BranchList = data.filter(x => x.Branch_ID == $scope.CcbIds.Branch_ID);
                        $scope.search.MPacsBranchId = "" + $scope.CcbIds.Branch_ID;
                        $scope.search.MPacsBankId = "" + $scope.CcbIds.CCBId;
                        $scope.isBranchDisabled = true;
                        $scope.isBankDisabled = true;
                    }
                    else {
                        $scope.BranchList = data;
                    }
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.BranchList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                var dist = $("#hdnDistrictId").val();
                if (dist != "0") {
                    $scope.GopalCreditCardModel.LoanDistrictId = dist;
                    //changes
                    //$scope.LoanBankList(dist);                    
                }
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };
   $scope.GetDistrictList();

    //old
    $scope.LoanBankLists = function (DistrictId) {
        var levelId = $("#hdnLevelId").val();
        var mappedId = $("#hdnGccId").val();

        if ($scope.GGG_Id == undefined || $scope.GGG_Id == null || $scope.GGG_Id == 0 || (DistrictId && DistrictId != "0")) {
            $scope.BankMasterList = [];
            $scope.PLDBBranches = [];
            $scope.PACSNameList = [];
            $scope.GopalCreditCardModel.LoanBankID = "0";
            $scope.GopalCreditCardModel.LoanBranchId = "0";
            $scope.GopalCreditCardModel.PACS_ID = "0";
        }

        $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=" + DistrictId).
            success(function (data, status) {            
                $scope.BankMasterList = data;
                if (levelId == "8" || levelId == "9") {
                    var CCBId = data[0].CCBId;
                    $scope.GopalCreditCardModel.LoanBankID = CCBId.toString();
                    $scope.IsBankFreezed = true;
                    $scope.Get_LoanBankBranch(CCBId);
                }
            }).
            error(function (data, status) {
                $scope.BankMasterList = [];
            });
    };

    //new
    $scope.LoanBankList = function () {
        var levelId = $("#hdnLevelId").val();
        var mappedId = $("#hdnGccId").val(); 

        if ($scope.GGG_Id == undefined || $scope.GGG_Id == null || $scope.GGG_Id == 0 || (DistrictId && DistrictId != "0")) {
            $scope.BankMasterList = [];
            $scope.PLDBBranches = [];
            $scope.PACSNameList = [];
        }

        $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=0" ).
            success(function (data, status) {
                $scope.BankMasterList = data;  

                if (levelId == "1") {
                    var CCBId = data[0].CCBId.toString();        
                    $scope.IsBankFreezed = false;
                    $scope.Get_LoanBankBranch(CCBId);
                    return;
                }

                if ( levelId == "8" || levelId == "9") {
                    var CCBId = data[0].CCBId.toString();                   
                    $scope.GopalCreditCardModel.LoanBankID = CCBId.toString();
                    $scope.IsBankFreezed = true;
                    $scope.Get_LoanBankBranch(CCBId);         
                }                    
            }).
            error(function (data, status) {
                $scope.BankMasterList = [];
            });
    };


    $scope.Get_LoanBankBranch = function (branch_ID) {
        var districtId = $("#hdnDistrictId").val();
        var levelId = $("#hdnLevelId").val();
        var mappedId = $("#hdnGccId").val();    

        if ($scope.GGG_Id == undefined || $scope.GGG_Id == null || $scope.GGG_Id == 0) {
            $scope.PLDBBranches = [];
            $scope.PACSNameList = [];
            $scope.GopalCreditCardModel.LoanBranchId = "0";
            $scope.GopalCreditCardModel.PACS_ID = "0";
        }

        //$(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBankBranch/?LoanDistrictId=" + districtId + "&LoanBankId=" + branch_ID).
            success(function (data, status) {
                $scope.PLDBBranches = data;                

                if (levelId == "8") {
                    var filtered = data.filter(x => x.Branch_ID == mappedId);
                    $scope.PLDBBranches = filtered;
                    $scope.GopalCreditCardModel.LoanBranchId = mappedId.toString();
                    $scope.IsBranchFreezed = true;                                       
                }

                if (levelId == "9") {
                    var filtered = data.filter(x => x.CCB_ID == branch_ID);
                    $scope.PLDBBranches = filtered;
                    $scope.GopalCreditCardModel.LoanBankID = filtered.CCBId;
                    $scope.GopalCreditCardModel.LoanBranchId = branch_ID.toString();
                    $scope.IsBranchFreezed = true;
                    $scope.Get_LoanBranch_PACS(branch_ID);                  
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.Get_LoanBranch_PACS = function (branch_ID) {
        var levelId = $("#hdnLevelId").val();
        var mappedId = $("#hdnGccId").val(); 

        if ($scope.GGG_Id == undefined || $scope.GGG_Id == null || $scope.GGG_Id == 0 || (branch_ID && branch_ID != "0")) {
            $scope.GopalCreditCardModel.PACS_ID = "0";
        }

        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBranch_PACS/?LoanBranchId=" + $scope.GopalCreditCardModel.LoanBranchId).
            success(function (data, status) {
                $scope.PACSNameList = data;               
                if (levelId == "8") {
                    var pacs = data.find(x => x.PACS_ID == mappedId);
                    if (pacs) {
                        $scope.GopalCreditCardModel.PACS_ID = mappedId;
                    }
                }
                if (levelId == "9") {                    
                    $scope.PACSNameList = data;                 
                    if (mappedId && data.some(x => x.PACS_ID == mappedId)) {
                        $scope.GopalCreditCardModel.PACS_ID = mappedId;
                    }
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PACSNameList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.InitializeLoginFilters = function () {
        var levelId = $("#hdnLevelId").val();
        var districtId = $("#hdnDistrictId").val();        
        //var districtId = "7";        
        var mappedId = $("#hdnGccId").val();

        // LEVEL 1 = ADMIN LOGIN
        if (levelId == "1" && districtId == "0" ) {            
            $scope.GopalCreditCardModel.LoanBankID = "0";
            $scope.GopalCreditCardModel.LoanBranchId = "0";
            $scope.GopalCreditCardModel.PACS_ID = "0";
            $scope.IsBankFreezed = false;
            $scope.IsBranchFreezed = false;
            $scope.IsPacsFreezed = false;
            return; 
        }

        setTimeout(function () {
            // LEVEL 7 = BANK LOGIN
            if (levelId == "7") {
                $scope.LoanBankLists(districtId);
                $scope.$watch('BankMasterList', function (newVal) {
                    if (!newVal || newVal.length == 0) return;
                    var mappedId = $("#hdnGccId").val();
                    var bank = newVal.find(x => x.CCBId == mappedId);
                    if (bank) {
                        $scope.GopalCreditCardModel.LoanBankID = bank.CCBId.toString();
                        $scope.IsBankFreezed = true;
                        $scope.Get_LoanBankBranch(bank.CCBId);
                    }
                });
            }

            // LEVEL 8 = BRANCH MANAGER LOGIN
            if (levelId == "8") {
                $scope.LoanBankLists(districtId);
                $scope.$watch('BankMasterList', function (newVal) {
                    if (!newVal || newVal.length == 0) return;
                    var selectedBranch = $scope.PLDBBranches?.find(x => x.Branch_ID == mappedId);
                    if (selectedBranch) {
                        $scope.GopalCreditCardModel.LoanBankID = selectedBranch.CCBId.toString();
                        $scope.IsBankFreezed = true;
                        $scope.Get_LoanBankBranch($scope.GopalCreditCardModel.LoanBankID);
                    }
                });
                $scope.$watch('PLDBBranches', function (branchList) {
                    if (!branchList || branchList.length == 0) return;
                    $scope.GopalCreditCardModel.LoanBranchId = mappedId.toString();
                    $scope.IsBranchFreezed = true;
                    $scope.Get_LoanBranch_PACS(mappedId);
                });               
            }

            // LEVEL 8 = PACS MANAGER LOGIN
            if (levelId == "9") {                     
                $scope.LoanBankLists(districtId);          
                var branchWatch = $scope.$watch('PLDBBranches', function (branches) {
                    if (!branches || branches.length == 0) return;
                    let found = false;
                    branches.forEach(function (br) {                      
                        $http.post(
                            SiteUrl + "/GopalCreditCard/Get_LoanBranch_PACS/?LoanBranchId=" + br.Branch_ID
                        )
                            .success(function (pacsList) {                               
                                let pacsRow = pacsList.find(x => x.PACS_ID == mappedId);                             
                                if (pacsRow && !found) {
                                    found = true;
                                    $scope.GopalCreditCardModel.LoanBankID = br.CCB_ID.toString();;
                                    $scope.IsBankFreezed = true;
                                    $scope.GopalCreditCardModel.LoanBranchId = br.Branch_ID.toString();
                                    $scope.IsBranchFreezed = true;
                                    $scope.PACSNameList = pacsList;
                                    $scope.GopalCreditCardModel.PACS_ID = mappedId.toString();
                                    $scope.IsPacsFreezed = true;
                                    branchWatch();
                                }
                            });
                    });
                });
            }
            $scope.$apply();
        }, 200);
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
        $scope.showLoader = false;

        var LevelId = parseInt($('#hdnLevelId').val());       
        $scope.showClearButton = (LevelId === 1);

        $scope.LoanBankList();

        setTimeout(() => {
            $scope.InitializeLoginFilters();
            $(".se-pre-con").fadeOut("slow");
        }, 700);

        $scope.GccLoanApplicationList(); 

    };

    $scope.OnPageLoad();


    //madhuri

    $scope.PLDBList = [];
    $scope.PLDBListData = function () {
        $scope.PLDB_ID = "0";
        $scope.PLDB_Branch_ID = "0";

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=" + ($scope.DISTRICT_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBList = data;
                if ($("#hdnPldbId").val() != "0") {
                    $scope.PLDB_ID = $("#hdnPldbId").val();
                    $scope.PLDBBranchList();
                    $("#PLDB_ID").attr("disabled", "disabled");
                }
            }).
            error(function (data, status) {
                $scope.PLDBList = [];
            });
    };


    $scope.PLDBBranches = [];
    $scope.PLDBBranchList = function (branch_ID) {
        $scope.PLDB_Branch_ID = "0";
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + ($scope.PLDB_ID ?? 0)).
            success(function (data, status) {
                $scope.PLDBBranches = data;

                if ($("#hdnPldbBranchId").val() != "0") {
                    $scope.PLDB_Branch_ID = $("#hdnPldbBranchId").val();
                    $("#PLDB_Branch_ID").attr("disabled", "disabled");
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };  

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
