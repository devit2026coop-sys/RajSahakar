var MasterMod = angular.module('SocietyMappingWiseListMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("SocietyMappingWiseListCnt", function ($rootScope, $scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.search = {};
    $scope.divClass = false;
    $scope.DistrictList = [];
    $scope.GccSummary = [];
    $scope.TotalGccSummary = []; $scope.BankMasterList = []; $scope.BankBranches = [];
    $scope.search.LoanDistrictId = '0';
    $scope.isLBDisabled = false;

    /*------START------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 20,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    };

    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];


    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                //  _currentPageID = 1;
                callback();
            }
            ++indx;
        });
    };

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
        $scope.DataPaging.TotalRecords = $rootScope.DataPagingDetails.TotalRecords
    };

    $rootScope.PagingRange = function (startPage, endPage, totalPages, step) {
        step = step || 1;
        var input = [];

        for (var tStart = startPage; tStart <= endPage && tStart <= totalPages; tStart++) {
            input.push(tStart);
        }
        return input;
    };


    $scope.searchRequest = function (callback) {
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').length;

        var indx = 1;

        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio')
            .each(function () {
                // Checkbox
                if ($(this).attr("type") == "checkbox") {
                    if ($(this).is(":checked")) {
                        var parameter = {  Key: $(this).attr('name') + '#' + indx, Value: $(this).val()  };
                        $scope.DataPaging.searchParameters.push(parameter);
                    }
                }
                // Radio Button
                else if ($(this).attr("type") == "radio") {
                    if ($(this).is(":checked")) {
                        var parameter = {  Key: $(this).attr('name'),  Value: $(this).val() };
                        $scope.DataPaging.searchParameters.push(parameter);
                    }
                }
                // Textbox / Select
                else {
                    if ($(this).val() != '' && $(this).val() != null &&  $(this).attr('name') != undefined) {
                        var parameter = {  Key: $(this).attr('name'),  Value: $(this).val() };
                        $scope.DataPaging.searchParameters.push(parameter);
                    }
                }
                if (indx == ln) {
                    $scope.DataPaging.currentPageID = 1;
                    callback();
                }
                ++indx;
            });
    };


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
        $scope.GetDistrictWithoutDivisionList();

        $scope.FinancialYearList();        
        var hdnDistId = $('#hdnDistrictId').val();
        if (hdnDistId > 0) {
            $scope.isLBDisabled = true;
            $scope.search.LoanDistrictId = hdnDistId
        } else
            $scope.isLBDisabled = false;

        $scope.LoanBankList(hdnDistId);

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


    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0") {
                    $scope.DISTRICT_ID = $("#hdnDistrictId").val();
                    $scope.PLDBListData();
                    $("#ddrDistrictId").hide();//("disabled", "disabled");
                }
                // $scope.BindFY();

            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };
    $scope.GetDistrictList();

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
                $scope.search.BLOCK_ID = '0';
                $scope.GetSocietyJurisdictionList();
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };

    $scope.BlockListByOffAdd = function (id) {
        $(".se-pre-con").fadeIn("slow");
        $scope.Blocksdata = [];
        $scope.GramPanchayats = [];
        $scope.Villages = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.search.DISTRICT_ID).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Blocksdata = data;
                $scope.GetGccSummary();
            }).
            error(function (data, status) {
                $scope.Blocksdata = [];
                $(".se-pre-con").fadeOut("slow");
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
            }).
            error(function (data, status) {
                $scope.BankMasterList = [];
            });
    };

    $scope.Get_LoanBankBranch = function (branch_ID) {
        var districtId = $('#hdnDistrictId').val();
        $scope.search.LoanDistrictId = districtId;

        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBankBranch/?LoanDistrictId=" + $scope.search.LoanDistrictId + "&LoanBankId=" + $scope.search.LoanBankID).
            success(function (data, status) {
                $scope.BankBranches = data;
                $scope.search.LoanBranchId = '0';
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.BankBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.Get_LoanBranch_PACS = function (branch_ID) {
        $scope.GccBankWiseSanctionList();
    }

    $scope.Clear = function () {
        $scope.search.ReportType = '0';
        $scope.search.FinancialYearId = '0';
        $scope.search.LoanBranchId = '0';      
        $scope.search.LoanBankID = '0'; 
        $scope.DISTRICT_ID = "0";
        $scope.StatusId = "0";
        $scope.search.IsRural = null;
        $scope.search.Type = null;      
        $scope.DataPagingDetails = {};      
        $scope.pageSize = 50;
        $scope.SocietyJurisdictionList = [];
    };


    $scope.SearchData = function () {

        if (!$scope.search.ReportType || $scope.search.ReportType == '0') {
            alert("Please select the Report Type");
            return;
        }

        if ((!$scope.DISTRICT_ID || $scope.DISTRICT_ID == '0') ) {
            alert("Please select District");
            return;
        }                

        $scope.searchRequest($scope.SocietyJurisdictionList);
    };

    //New
    $scope.GetSocietyJurisdictionList = function () {
        $(".se-pre-con").fadeIn("slow"); $scope.SocietyJurisdictionList = [];
        var jsonParameters = {  TotalRecords: $scope.DataPaging.TotalRecords,  PageNumber: $scope.DataPaging.currentPageID, PageSize: $scope.pageSize, SortingColumn: $scope.DataPaging.sortExpression,  SortingOrder: $scope.DataPaging.sortDirection, SearchParameter: $scope.DataPaging.searchParameters  };
        $http.post(SiteUrl + "/MIS/GetSocietyMappingList", JSON.stringify({ Details: jsonParameters })).success(function (data) {           
            if (data && data.SocietyMapping && data.SocietyMapping && data.SocietyMapping) {
                $scope.SocietyJurisdictionList = JSON.parse(data.SocietyMapping);                
                if (data.DataPaging) {
                    $scope.TotalRecords = data.TotalRecords;
                    //$scope.DataPagingDetails = data.DataPaging;
                  //  $rootScope.DataPagingDetails.TotalRecords = $scope.TotalRecords;
                    $rootScope.DataPagingDetails = data.DataPaging;
                    $scope.DataPaging.currentPageID = data.DataPaging.PageNumber;
                    $rootScope.DataPagingDetails.PageNumber = $scope.DataPaging.currentPageID;
                    $rootScope.DataPagingDetails.PageSize = $scope.DataPaging.pageSize;
                    $rootScope.SetDataPaging();
                }
            }
            else {
                $scope.SocietyJurisdictionList = [];               
            }

            $(".se-pre-con").fadeOut("slow");
        })
            .error(function (error) {
                console.log("Error", error);
                $scope.SocietyJurisdictionList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetFlowProcessStatus = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FLOWPROCESSSTATUS&MasterCode=0").
            success(function (data, status) {
                $scope.StatusList = data;
                if ($("#hdnDistrictId").val() != "0") {
                    $("#ddrStatus").hide();//("disabled", "disabled");
                }
                // $scope.BindFY();

            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching flow list data.');
            });
    };
    $scope.GetFlowProcessStatus();


    $scope.OnBankChange = function () {
        $scope.search.LoanBranchId = '0';
        if ($scope.search.LoanBankID != '0') {
            $scope.Get_LoanBankBranch($scope.search.LoanBankID);
        }
        else {
            $scope.BranchList = [];
        }
    };

    $scope.exportAllToExcel = function () {
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, PageSize: $scope.DataPaging.TotalRecords, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };

        $http.post(SiteUrl + "/MIS/GetSocietyMappingList", jsonParameters)
            .success(function (data) {
                var allData = JSON.parse(data.SocietyMapping);
                if (allData.length == 0) {
                    alert("No records found");
                    $(".se-pre-con").fadeOut("slow");
                    return;
                }
                exportJsonToExcel(allData, "Society Mapping List");
                $(".se-pre-con").fadeOut("slow");
            })
            .error(function () {
                alert("Error while exporting data");
                $(".se-pre-con").fadeOut("slow");
            });
    };

    function exportJsonToExcel(data, fileName) {
        var filteredData = data.map(function (item, index) {
            var newItem = {};
            newItem["S.No"] = index + 1;

            Object.keys(item).forEach(function (key) {              
                if (key !== "TotalRecords") {
                    newItem[key] = item[key];
                }
            });
            return newItem;
        });

        var worksheet = XLSX.utils.json_to_sheet(filteredData);
        var workbook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workbook, worksheet, "Data");
        XLSX.writeFile(workbook, fileName + ".xlsx");
    }


    $scope.IsOverall = false;
    $scope.GopalCreditCardModel = {};
    $scope.GopalCreditCardModel.IsPendancy = 0;
    $scope.GridShow = function (val) {
        if (val == 0) {
            $scope.IsOverall = false;
        }
        else {
            $scope.IsOverall = true;

        }        
    }

    $scope.OnPageLoad();

});
