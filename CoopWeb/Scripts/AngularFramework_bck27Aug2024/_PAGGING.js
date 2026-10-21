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
    if ($scope.Filter.FilterType == 'SubmissionDate') {
        $scope.showSearchDate = true;
        $scope.showSearchText = false;
        $scope.AddCalender();
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
    $scope.GetElectionSocietyListData();
}
    /*------------End Paging and Filter-----------------*/
