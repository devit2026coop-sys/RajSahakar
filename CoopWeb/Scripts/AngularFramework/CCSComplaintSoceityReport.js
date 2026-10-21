var FertilizerReportModule = angular.module('CCSComplaintReportMod', ['angularUtils.directives.dirPagination']);
FertilizerReportModule.controller("CCSComplaintReportCnt", function ($scope, $rootScope, $http) {

    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 50,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
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
                callback();
            }
            ++indx;
        });
    }
    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];
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
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.FilterType = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.GetComplaintData(2);
    }
    /*------------End Paging and Filter-----------------*/



    // on page load
    $scope.OnPageLoad = function () {
      
        if (_language == 'en-us') {
            $http.get("../Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("../Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });

        }
        $scope.DistrictList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
        //$scope.GetSocietyBasicDetailList();
        $scope.GetComplaintData(1);
        
    };
    //END page load 
    $scope.SearchType = 3;
    $scope.ChangeSelction = function () {
        $scope.SearchType = $('input[type=radio][name=rdoSelect]:checked').val();
        $scope.GetComplaintData(1);
        //if ($('input[type=radio][name=rdoSelect]:checked').val() === '3') {
        //    $scope.GetSocietyBasicDetailList();
        //}
        //else {
        //    $scope.GetSocietyBasicDetailList();
        //}

    };
    $scope.Reset = function () {
        $scope.DivShowDetail = false;
        $scope.DivShowCount = false;
        $scope.DISTRICT_ID = '0';
        $scope.CCSComplaintList = [];
        $scope.SocietyId = '0';
        $scope.SearchType = 1;
        //$scope.GetSocietyBasicDetailList();

    };
    $scope.SetCCSociety = function () {
        //if($scope.DISTRICT_ID != '0') {
        $scope.GetSocietyBasicDetailList();
        // }
    };
    $scope.GetSocietyBasicDetailList = function () {
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Society/GetCCSocietyList", { "SearchType": $scope.SearchType, "DistrictId": $scope.DISTRICT_ID }).
            success(function (data, status) {
                $scope.SocietyBasicDetailList = data;
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching list");
                $(".blockUI").fadeOut("slow");
            });
        $(".blockUI").fadeOut("slow");
    };
    $scope.CCSComplaintDetailsList = [];
    $scope.CCSComplaintCombine = [];
    $scope.DivShowCount = true;
    $scope.SocietyID = "0";
    $scope.GetComplaintData = function (type) {
        //$scope.CCSComplaintList = [];
        //$scope.CCSComplaintCombine = [];
        $scope.DivShowCount = true;
        $scope.DivShowDetail = false;
        $(".blockUI").fadeIn("slow");
        var npageSize = $scope.DataPaging.pageSize;
        var ncurrentPageID = $scope.DataPaging.currentPageID;
        if (type == 1)
        {
            npageSize = 10000;
            ncurrentPageID=1
        }
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": ncurrentPageID, "PageSize": npageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/MIS/GetComplaintReport", JSON.stringify({ Details: jsonParameters, "DistrictId": $scope.DISTRICT_ID == 'undefined' ? '0' : $scope.DISTRICT_ID, "SocietyId": $scope.SocietyID, "SearchType": $scope.SearchType })).
            success(function (data, status) {
               // $scope.CCSComplaintCombine = [];
                $scope.CCSComplaintDetailsList = [];
                if (type == 1) {
                    $scope.CCSComplaintCombine = JSON.parse(data.DataList);
                    $scope.total = 0;
                    $scope.DivShowCount = true;
                    $scope.total = data.TotalAmount;
                    $scope.TotalCSS = data.TotalCSS;
                    //$scope.CCSComplaintCombine.forEach(function (item,index) {
                    //    $scope.total += item.toamt;
                    //});                 
                  
                } else {
                    $scope.DivShowCount = false;
                    $scope.CCSComplaintDetailsList = JSON.parse(data.DataList);
                    $rootScope.DataPagingDetails = data.DataPaging;
                    $rootScope.SetDataPaging();
                }
                $(".se-pre-con").fadeOut("slow");

            }).
            error(function (data, status) {
                $scope.CCSComplaintCombine = [];
                alert('Error while fetching data.');
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.ListType = '';
    $scope.GetshowdtailsList = function (item) {
        $scope.DISTRICT_ID = '0';
        $scope.SocietyID = item.SocietyID;
        //$scope.searchRequest($scope.GetComplaintData);
        $scope.GetComplaintData(2);
    }

    $scope.ShowDetail = function () {
        $(".blockUI").fadeIn("slow");
        $scope.CCSComplaintList = [];
        $scope.currentPage = 1;

        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/MIS/GetComplaintReport", JSON.stringify({ Details: jsonParameters, "DistrictId": $scope.DISTRICT_ID == 'undefined' ? '0' : $scope.DISTRICT_ID, "SocietyId": $scope.SocietyID, "SearchType": $scope.SearchType })).
            success(function (data, status) {
                $scope.DivShowCount = false;
                $scope.DivShowDetail = true;
                $(".se-pre-con").fadeOut("slow");
                if (JSON.parse(data.DataList).length === 0) {
                    var msg = _language == 'en-us' ? 'No data found.' : 'कोई डेटा नहीं मिला।';
                    ShowModelAlert('', msg);
                    return;
                }
                else {
                    $scope.CCSComplaintList = JSON.parse(data.DataList);;
                    $rootScope.DataPagingDetails = data.DataPaging;
                    $rootScope.SetDataPaging();
                }

            }).
            error(function (data, status) {
                $scope.CCSComplaintList = [];
                alert('Error while fetching society data.');
                $(".se-pre-con").fadeOut("slow");
            });
        //$scope.DivShowDetail = true;
        //$scope.DivShowCount = false;
        //$(".se-pre-con").fadeOut("slow");
    };


    $scope.BackToCount = function () { 
        $scope.SocietyID = "0";
        $scope.DivShowCount = true;
    }

    $scope.OnPageLoad();

});