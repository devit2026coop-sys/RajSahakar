var MasterMod = angular.module('ApplicationFlowGssMpacsTargetDetailsModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ApplicationGssMpacsTargetDetailsFlowCont", function ($scope, $rootScope, $http) {
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 20,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }
    
    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters = [];

        var elements = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio');
        if (elements.length === 0) {
            callback();
            return;
        }

        elements.each(function () {
             if ($(this).attr("type") == "checkbox") {
                if ($(this).is(":checked")) {
                    $scope.DataPaging.searchParameters.push({
                        Key: $(this).attr('name'),
                        Value: $(this).val()
                    });
                }
            }
            else if ($(this).attr("type") == "radio") {
                if ($(this).is(":checked")) {
                    $scope.DataPaging.searchParameters.push({
                        Key: $(this).attr('name'),
                        Value: $(this).val()
                    });
                }
            }
            else {
                if ($(this).val() != '' && $(this).attr('name')) {
                    $scope.DataPaging.searchParameters.push({
                        Key: $(this).attr('name'),
                        Value: $(this).val()
                    });
                }
            }
        });
        callback();
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
        $scope.DataPaging.searchParameters = [];
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
    $scope.header = {};
    $scope.GridData = [];
    let params = JSON.parse(sessionStorage.getItem("LoanDetailParams"));

    if (!params) {
        alert("Invalid request");
        return;
    }

    $scope.header = params;
    $scope.DistrictId = params.DistrictId;
    $scope.DocType = params.DocType;

    $scope.ClearSearch = function () {
        $scope.PLDB.PldbId = "0";
        $scope.PLDB.BranchId = "0";
        $scope.PLDB_ID = "0";
        $scope.PLDB_Branch_ID = "0";
        $scope.StatusId = "";
        $scope.q = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.GSSMpacsTargetDetailList();
    }
    /*------------End Paging and Filter-----------------*/
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;  

    $scope.GSSMpacsTargetDetailList = function () {
        $(".se-pre-con").fadeIn("slow");
        var postData = { DistrictId: $scope.DistrictId, Type: $scope.DocType, TotalRecords: $scope.DataPaging.TotalRecords,  PageNumber: $scope.DataPaging.currentPageID,  PageSize: $scope.pageSize, SortingColumn: $scope.DataPaging.sortExpression, SortingOrder: $scope.DataPaging.sortDirection, SearchParameter: $scope.DataPaging.searchParameters  };
        $http.post(SiteUrl + "/MIS/GetGSSMpacsTargetCountDetail",postData).then(function (response) {                     
            $scope.GridData = JSON.parse(response.data.DataList);          
            $scope.DataPaging.TotalRecords = response.data.DataPaging.TotalRecords;           
            $scope.DataPaging.PageNumber = response.data.DataPaging.PageNumber;
            $scope.DataPaging.PageSize = response.data.DataPaging.PageSize;         
            $rootScope.DataPagingDetails = $scope.DataPaging;     
            $rootScope.SetDataPaging();
        }).catch(function () {
            alert("Error loading details");
        }).finally(function () {
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.exportAllToExcel = function () {
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { DistrictId: $scope.DistrictId, Type: $scope.DocType, TotalRecords: 0, PageNumber: 1, PageSize: 100000,SortingColumn: $scope.DataPaging.sortExpression,SortingOrder: $scope.DataPaging.sortDirection,SearchParameter: $scope.DataPaging.searchParameters};

        $http.post(SiteUrl + "/MIS/GetGSSMpacsTargetCountDetail", jsonParameters)
            .success(function (data) {
                var allData = JSON.parse(data.DataList);
                if (allData.length == 0) {
                    alert("No records found");
                    $(".se-pre-con").fadeOut("slow");
                    return;
                }
                exportJsonToExcel(allData, "GSS Mpacs Target Details List");
                $(".se-pre-con").fadeOut("slow");
            })
            .error(function () {
                alert("Error while exporting data");
                $(".se-pre-con").fadeOut("slow");
            });
    };


    function exportJsonToExcel(data, fileName) {
        var filteredData = data.map(function (item) {

            var newItem = Object.assign({}, item);

            // Remove unwanted columns
            delete newItem.encGccid;
            delete newItem.EncPldbOtsSchemeId;
            delete newItem.PLDB_OTS_SchemeID;
            delete newItem.OtherDoc_New;
            delete newItem.TaamilDoc_New;

            return newItem;
        });

        var worksheet = XLSX.utils.json_to_sheet(filteredData);

        var workbook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workbook, worksheet, "Data");
        XLSX.writeFile(workbook, fileName + ".xlsx");
    }
    
    $scope.goBack = function () {
        window.location.href = "/MIS/GSSMpacsTargetList";
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

        $scope.showLoader = false;    
        $scope.GSSMpacsTargetDetailList();       
    };

    $scope.OnPageLoad();

});
