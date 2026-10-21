var MasterMod = angular.module('SocietyMergeListMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("SocietyMergeListCnt", function ($rootScope, $scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.search = {};
    $scope.divClass = false;
    $scope.DistrictList = [];
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

    //for Searching Function 
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

        var hdnDistId = $('#hdnDistrictId').val();     
        if (hdnDistId > 0) {
            $scope.isLBDisabled = true;
            $scope.search.districtId = hdnDistId
        } else
            $scope.isLBDisabled = false;
        $scope.GetSocietyMergeReport();     

    };

    $scope.Back = function () {
        $scope.ShowSummary = true;
        $scope.DetailList = [];
    };

    $scope.DistrictList = [];
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0")
            .success(function (data, status) {
                $scope.DistrictList = data;
                var hdnDistId = $('#hdnDistrictId').val();

                if (hdnDistId && hdnDistId != "0") {
                    $scope.search.districtId = hdnDistId;
                    $scope.isLBDisabled = true;
                }
                else {
                    $scope.search.districtId = "0";
                    $scope.isLBDisabled = false;
                }

            })
            .error(function (data, status) {
                $scope.DistrictList = [];
            });
    };

    $scope.GetDistrictList();

    $scope.Clear = function () {
        $scope.search.districtId = '0';     
        $scope.DataPagingDetails = {};      
        $scope.pageSize = 50;
        $scope.GetSocietyMergeReport();
    };

    //Main List
    $scope.GetSocietyMergeReport = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.SocietySummary = [];

        $http.post(SiteUrl + "/MIS/GetAllMergeSociety?Action=getAllMergeSociety&DistrictId=" + $scope.search.districtId)
            .success(function (data) {
                if (data && data.SocietySummary) {
                    $scope.SocietySummary = JSON.parse(data.SocietySummary);
                    //$scope.TotalCount = 0;
                    //angular.forEach($scope.SocietySummary, function (item) {
                    //    $scope.TotalCount += Number(item.ct) || 0;
                    //});
                }
                else {
                    $scope.SocietySummary = [];
                }
                $(".se-pre-con").fadeOut("slow");
            })
            .error(function (error) {
                console.log("Error", error);
                $scope.SocietySummary = [];
                $(".se-pre-con").fadeOut("slow");

            });
    };

    $scope.ShowSummary = true;
    $scope.DetailList = [];

    //Society Merger Detail List
    $scope.GetDetails = function (distId,flagId) {
        //debugger;
        $(".se-pre-con").fadeIn("slow");
        var postData = { DistrictId: distId, FlagId: flagId, TotalRecords: $scope.DataPaging.TotalRecords, PageNumber: $scope.DataPaging.currentPageID, PageSize: $scope.pageSize, SortingColumn: $scope.DataPaging.sortExpression, SortingOrder: $scope.DataPaging.sortDirection, SearchParameter: $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/MIS/GetActiveSocietyDetail" , postData)
            .success(function (data) {
                $scope.DetailList = JSON.parse(data.DataList);               
                $scope.DataPaging.TotalRecords = data.DataPaging.TotalRecords;               
                $scope.DataPaging.PageNumber = data.DataPaging.PageNumber;                
                $scope.DataPaging.PageSize = data.DataPaging.PageSize;               
                $rootScope.DataPagingDetails = $scope.DataPaging;              
                $rootScope.SetDataPaging();
                $(".se-pre-con").fadeOut("slow");
                $scope.ShowSummary = false;
            })
            .error(function () {
                $scope.DetailList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.OnPageLoad();

});
