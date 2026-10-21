var MasterMod = angular.module('ApplicationFlowPLDBLoanWiseModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ApplicationPLDBLoanWiseFlowCont", function ($scope, $rootScope, $http) {
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
                //  _currentPageID = 1;
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
        $scope.DataPaging.TotalRecords = $rootScope.DataPagingDetails.TotalRecords
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

    $scope.IsPldbFreezed = false;
    $scope.IsPldbBranchFreezed = false;


    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        if ($scope.Filter.FilterType == 'wrs.created_date') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            // $scope.AddCalender();
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }
    $scope.ClearSearch = function () { 
        $scope.PLDB.PldbId = "0";
        $scope.PLDB.BranchId = "0";
        $scope.PLDB_ID = "0";
        $scope.PLDB_Branch_ID = "0";
        $scope.StatusId = "";
        $scope.q = "";
        $scope.PLDB.FinancialYearId = "0";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.PLDBLoanApplicationPldbWiseList();
    }
    /*------------End Paging and Filter-----------------*/
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;


    //Pldb Branch
    $scope.PLDBBranchList = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + $scope.PLDB.PldbId).
            success(function (data, status) {
                $scope.PLDBBranches = data;
                var districtid = $('#hdnDistrictId').val();
                var LoginPldbBranchId = $('#hdnPldbBranchId').val();

                if (LoginPldbBranchId && LoginPldbBranchId != '0' && districtid) {
                    $scope.PLDB.BranchId = LoginPldbBranchId.toString();
                    $scope.IsPldbBranchFreezed = true;

                } else {
                    $scope.PLDB.BranchId = branch_ID ? branch_ID.toString() : '0';
                }

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    //Pldb Bank
    $scope.PLDBList = [];

    $scope.LoadMaster = function () {

        return $http.get(SiteUrl + "/pldb/LoadMaster")
            .then(function (response) {
                $scope.PLDBList = JSON.parse(response.data.DataList).PLDB || [];

                var districtid = $('#hdnDistrictId').val();
                var LoginPldbId = $('#hdnPldbId').val();

                if (LoginPldbId && LoginPldbId !== '0' && districtid) {

                    // wait till dropdown options are ready
                    $scope.$evalAsync(function () {

                        $scope.PLDB = $scope.PLDB || {};
                        $scope.PLDB.PldbId = LoginPldbId.toString();
                        $scope.IsPldbFreezed = true;

                        // load branches AFTER PLDB is set
                        $scope.PLDBBranchList('0');
                    });
                }
            }, function () {
                $scope.PLDBList = [];
            });
    };

    $scope.LoadMaster();

    $scope.PLDBLoanApplicationPldbWiseList = function () {
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/MIS/GetPLBDLoanApplicationPldbWise", JSON.stringify({ Details: jsonParameters }))
            .then(function (response) {
                var data = response.data;
                if (!data.status) {
                    alert(data.message);
                    $(".se-pre-con").fadeOut("slow");
                    return;
                }
                $scope.pldbLoanApplicationList = data.data;
                $(".se-pre-con").fadeOut("slow");
            })
            .catch(function (error) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.FinancialYearList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1")
            .success(function (data, status) {

                $scope.FinancialYearList = data.filter(function (item) {
                    return item.FIN_YEAR_NAME == "2025-26" || item.FIN_YEAR_NAME == "2026-27";
                });

            })
            .error(function (data, status) {

                $scope.FinancialYearList = [];
            });
    }

    $scope.openPldbCountDetails = function (item) {

        if (!item || !item.PldbId) return;

        var params = {
            PldbId: item.PldbId || 0,
            BranchId: ($scope.PLDB && $scope.PLDB.BranchId && $scope.PLDB.BranchId != '0') ? $scope.PLDB.BranchId : 0,
            FinancialYearId: item.FinancialYearId || 0
        };
        sessionStorage.setItem("LoanDetailParams", JSON.stringify(params));

        window.location.href = "/MIS/PLBDLoanApplicationDetailList";
    };

    $scope.openPldbDetails = function (item, statusType) {

        if (!item || !item.PldbId) return;

        var params = {
            PldbId: item.PldbId || 0,
            StatusType: statusType,
            financialYearId : $scope.PLDB.FinancialYearId || 0,
            branchId : $scope.PLDB.BranchId || 0
        };

        sessionStorage.setItem("LoanDetailParams", JSON.stringify(params));

        window.location.href = "/MIS/PLBDLoanApplicationStatusDetailList";

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
        }


        var LevelId = parseInt($("#hdnLevelId").val(), 10);
        // Clear button visibility
        $scope.showClearButton = (LevelId === 1);

        $scope.FinancialYearList();

        $scope.showLoader = false;
        $scope.IsDistrictFreezed = false;
        $scope.PLDBLoanApplicationPldbWiseList();
    };

    $scope.OnPageLoad();

});
