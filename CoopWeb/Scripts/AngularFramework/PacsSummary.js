
var MasterMod = angular.module('pacsMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("pacsCtrl", function ($rootScope, $scope, $location, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.search = {};
    $scope.DistrictList = [];
    $scope.ReportType = 1;
    $scope.search.ReportType = "";

    $scope.IsPldbcalled = true;
    $scope.pacsSummaryDataList = [];
    $scope.isNoRecord = false;

    $scope.reportTypeList = [
        { id: 1, name: 'Details' },
        { id: 2, name: 'Count' }
    ];
    //console.log($scope.search.ReportType)
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 50,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: [],
        TotalRecords: 0,
    }

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
        $scope.getPacsSummaryDetails();
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
                $scope.getPacsSummaryDetails(false);
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
                callback($scope.SocietyCountId_ss, $scope.DistrictId_ss);
            }
            ++indx;
        });
    }

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
        }
        $scope.GetDistrictList();
    };
    $scope.GetDistrictList = function () {
        $scope.DistrictList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0") {
                    $scope.search.DISTRICT_ID = $("#hdnDistrictId").val();
                    $("#DISTRICT_ID").attr("disabled", "disabled");
                }
            }).error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });

    };
    $scope.getPacsSummaryDetails = function (isExport) {
        if (!$scope.search.ReportType) { alert("Please select Report Type"); return; }
        else if (!$scope.search.FromDate) { alert("Please select From Date"); return; }
        else if (!$scope.search.ToDate) { alert("Please select To Date"); return; }

        var fromDate = new Date($scope.search.FromDate);
        var toDate = new Date($scope.search.ToDate);
        if (fromDate > toDate) { alert("From Date cannot be greater than To Date"); return; }

        $(".se-pre-con").fadeIn("slow");
        $scope.pacsSummaryDataList = [];
        var actionType = "";
        if ($scope.search.ReportType == '1') { actionType = 'SummaryDetails'; $scope.colspan = 5; }
        else if ($scope.search.ReportType == '2') { actionType = 'SummaryCount'; $scope.colspan = 3; }
        if (!isExport)
        $rootScope.DataPagingDetails = {};
        var jsonParameters = { "TotalRecords": ($scope.DataPaging.TotalRecords ?? 0), "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection };
        return $http.post(SiteUrl + "/MIS/GetPacsSummaryDetails", JSON.stringify({ Details: jsonParameters, FromDate: $scope.validateDate($scope.search.FromDate), ToDate: $scope.validateDate($scope.search.ToDate), actionType: actionType, isExport: isExport })).success(function (data, status) {
            if (isExport) {
                $scope.downloadFile(data);
            }
            else {
                if (data != null && data.length > 0) {                 
                    $scope.pacsSummaryDataList = data;                    

                    //For Total
                    $scope.totalCnt = 0;
                    angular.forEach($scope.pacsSummaryDataList, function (item) {
                        $scope.totalCnt += Number(item.Cnt || 0);
                    });
                    
                    if ($scope.DataPaging.currentPageID == 1) {
                        $scope.TotalRecords = data[0].TotalRecords;
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
                }
            }
            $(".se-pre-con").fadeOut("slow");
        }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.pacsSummaryDataList = [];
            });
    };


    $scope.validateDate = function (date) {
        var d = new Date(date);
        var month = ('0' + (d.getMonth() + 1)).slice(-2);
        var day = ('0' + d.getDate()).slice(-2);
        var year = d.getFullYear();
        return year + '-' + month + '-' + day;
    }

    $scope.ClearFilter = function () {
        $scope.search.ReportType = "";
        $scope.search.FromDate = "";
        $scope.search.ToDate = "";
        $scope.IsPldbcalled = true;
    }

    $scope.exportExcel = function () {
        $scope.getPacsSummaryDetails(true);
    }
    $scope.downloadFile = function (fileUrl) {
        const fileName = fileUrl.split('/').pop();
        $http({
            url: fileUrl,
            method: 'GET',
            responseType: 'blob'
        }).then(function (response) {
            var blob = new Blob([response.data], { type: response.headers('Content-Type') });
            var downloadUrl = URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = downloadUrl;
            a.download = fileName;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        }).catch(function (error) {
            console.error('Error downloading file:', error);
        });
    };
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

