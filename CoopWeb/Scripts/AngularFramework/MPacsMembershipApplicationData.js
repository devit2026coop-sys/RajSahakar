var PLDBMemberRegistrtionModule = angular.module('mPacsMod', ['angularUtils.directives.dirPagination']);
PLDBMemberRegistrtionModule.controller('mPacsCnt', function ($rootScope, $scope, $http, $rootScope) {
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.Filter = {};
    $scope.search = {};
    $scope.mPacsDataList = [];
    $scope.CcbIds = {};
    $scope.divViewShow = false;
    $scope.DistrictList = [];
    $scope.selectedDistrictId = 0;
    $scope.isDistrictDisabled = false;
    $scope.isBankDisabled = false;
    $scope.isBranchDisabled = false;
    $scope.search.districtId = 0;

    $scope.hdnCcbId = $('#hdnCcbId').val();
    $scope.hdnLevelId = $('#hdnLevelId').val();

    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 50,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: [],
        TotalRecords: 0,
    }

    //$scope.DataPaging.PageSize = $scope.obj.pageSize ?? 50;

    $scope.pagechange = function () {
        $scope.DataPaging = {
            searchBoxDivId: 'search-filter',
            currentPageID: 1,
            pageSize: $scope.obj.pageSize ?? 50,
            sortExpression: '',
            sortDirection: '1',
            searchParameters: [],
            TotalRecords: 0,
        }
        $scope.GetMPacsApplicationDetailByUser();
    }


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
                $scope.GetMPacsApplicationDetailByUser();
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
                $scope.DataPaging.currentPageID = 1;
                //callback($scope.DistrictId_ss, $scope.DistrictId_ss);
            }
            ++indx;
        });
    }


    // on page load


    $scope.OnPageLoad = function () {
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
        $scope.GetCcbIdsByMapping();
        $scope.GetDistrictList();

    };
    $scope.GetDistrictList = function () {
        $scope.hdnDistId = $('#hdnDistrictId').val();
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                if ($scope.hdnDistId > 0) {
                    $scope.DistrictList = data.filter(x => x.DISTRICT_ID == $scope.hdnDistId);
                    $scope.selectedDistrictId = data.filter(x => x.DISTRICT_ID == $scope.hdnDistId)[0].DISTRICT_ID;
                    $scope.search.districtId = $scope.selectedDistrictId;
                    $scope.isDistrictDisabled = true;
                }
                else {
                    $scope.DistrictList = data;
                }
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };

    $scope.GetCcbIdsByMapping = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MAPPING_GCC_ID").
            success(function (data, status) {
                if (data != null && data.length > 0)
                    $scope.CcbIds = data[0];
                $scope.GetCcbList();
            }).
            error(function (data, status) {
                $scope.CcbIds = [];
            });
    };

    $scope.GetCcbList = function () {
        $scope.BranchList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CCB_LIST&MasterCode=0").
            success(function (data, status) {

                $scope.BankList = data;
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
            }).
            error(function (data, status) {
                $scope.BankList = [];
            });
    };

    $scope.GetCcbBranchList = function () {
        $(".se-pre-con").fadeIn("slow");
        if ($scope.hdnLevelId == 8) {
            $scope.search.MPacsBankId = $scope.CcbIds.CCBId;
        }
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CCB_BANK_LIST&MasterCode=" + ($scope.search.MPacsBankId ?? 0)).
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
                //$scope.pagechange();
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.BranchList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.ClearSearch = function () {
        $scope.Filter.SearchText = null;
        $scope.GetMPacsApplicationDetailByUser();
    };

    $scope.ViewMemberApplication = function (encId) {
        $scope.GetMPacsApplicationDetailById(encId);
    }

    $scope.GetMPacsApplicationDetailByUser = function () {
        $(".se-pre-con").fadeIn("slow");
        $rootScope.DataPagingDetails = {};
        var jsonParameters = { "TotalRecords": ($scope.DataPaging.TotalRecords ?? 0), "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };

        $http.post(SiteUrl + "/MPacsMembership/GetMPacsApplicationDetailByUser", JSON.stringify({ Details: jsonParameters, SearchText: $scope.Filter.SearchText, bankId: ($scope.search.MPacsBankId ?? 0), branchId: ($scope.search.MPacsBranchId ?? 0) })).
            success(function (data, status) {
                $scope.mPacsDataList = JSON.parse(data.Result);
                $scope.divGrid = true;
                if ($scope.DataPaging.currentPageID == 1 && $scope.mPacsDataList.length > 0) {
                    $scope.TotalRecords = $scope.mPacsDataList[0].TotalRecords;
                }
                $rootScope.DataPagingDetails.TotalRecords = $scope.TotalRecords;
                $rootScope.DataPagingDetails.PageNumber = $scope.DataPaging.currentPageID;
                $rootScope.DataPagingDetails.PageSize = $scope.DataPaging.pageSize;
                $rootScope.SetDataPaging();
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
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.mPacsDataList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.GetMPacsApplicationDetailById = function (encId) {
        $(".se-pre-con").fadeIn("slow");
        $scope.mPacsView = {};
        $http.get(SiteUrl + "/MPacsMembership/GetMPacsApplicationDetailByEncId?encId=" + encId).
            success(function (data, status) {
                $scope.mPacsView = JSON.parse(data.Result)[0];
                $scope.divGrid = false;
                $scope.divViewShow = true;

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.mPacsDataList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.OnPageLoad();
    $scope.GetMPacsApplicationDetailByUser();
});
