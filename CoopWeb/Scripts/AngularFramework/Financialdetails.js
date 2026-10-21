
var MasterMod = angular.module('finaMod', ['angularUtils.directives.dirPagination']);
MasterMod.config(['$locationProvider', function ($locationProvider) {
    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });
}]);
MasterMod.controller("finaCtrl", function ($scope, $rootScope, $http, $location) {

    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.search = {};
    $scope.districtDataList = [];
    $scope.FinancialDataList = [];
    $scope.isNoRecord = false;
    $scope.SectorDataList = [];
    $scope.FinancialUpdatedList = [];
    $scope.isCell = 0;
    $scope.areaDataList = [{ id: 1, name: 'Urban' },
    { id: 2, name: 'Rural' }];

    var params = new URLSearchParams(window.location.search);
    $scope.search = {
        societyname: params.get("societyname") || '',
        location: params.get("location") || 0,
        districtcode: params.get("district_code") || 0,
        sectorcode: params.get("sectorcode") || 0,
        financial_update: params.get("financial_update"),
    };

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
        $scope.getFinancialTrunoverProfitLossDetail(false);
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
                $scope.getFinancialTrunoverProfitLossDetail(false);
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
        $scope.getDistrictDetailByState();
        $scope.getSectorDetail();
        $scope.getFinancialTrunoverProfitLossDetail(false);
        if ($scope.search.financial_update != null) {
            $scope.getFinancialUpdatedData();
        }
    };

    $scope.getFinancialUpdatedData = function () {
        $scope.FinancialUpdatedList = [{ id: 0, name: "No" },
        { id: 1, name: "Yes" }];
        if ($scope.FinancialUpdatedList != null && $scope.FinancialUpdatedList.length > 0) {
            if ($scope.search.financial_update != null) {
                $scope.search.financial_update = $scope.FinancialUpdatedList.filter(a => a.id == $scope.search.financial_update)[0].id + '';
            }
        }
    };

    $scope.getDistrictDetailByState = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetDistrictwithStatecodeData?statecode=" + 8).success(function (data, status) {
            if (data.Result.Data != null && data.Result.Data.NCDDistrictDetail != null) {
                $scope.districtDataList = JSON.parse(data.Result.Data.NCDDistrictDetail);
                if ($scope.districtDataList != null && $scope.districtDataList.length > 0) {
                    if ($scope.search.districtcode != null && $scope.search.districtcode > 0) {
                        $scope.search.districtcode = $scope.districtDataList.filter(x => x.district_code == $scope.search.districtcode)[0].district_code;
                    }
                }
            }
            $(".se-pre-con").fadeOut("slow");
        }).error(function (data, status) {
            $scope.districtDataList = [];
            $(".se-pre-con").fadeOut("slow");
        });
    };
    $scope.getFinancialTrunoverProfitLossDetail = function (isExport) {
        if ($scope.search.financial_update != null && $scope.search.financial_update == 0) {
            $scope.type = 'GetFinancialNoUpdatedSocieties';
            $scope.isCell = 1;
        } else if ($scope.search.financial_update != null && $scope.search.financial_update == 1) { $scope.type = 'GetFinancialTrunoverReport'; $scope.isCell = 0; }
        else { $scope.type = 'GetFinancialTrunoverReport'; $scope.isCell = 0; }
        $(".se-pre-con").fadeIn("slow");
        if (!isExport)
            $rootScope.DataPagingDetails = {};
        var jsonParameters = { "TotalRecords": ($scope.DataPaging.TotalRecords ?? 0), "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection };
        return $http.post(SiteUrl + "/NcdData/GetFinancialTrunoverProfitLossDetail", JSON.stringify({ Details: jsonParameters, societyname: $scope.search.societyname, location: ($scope.search.location ?? 0), district_code: ($scope.search.districtcode ?? 0), sectorcode: ($scope.search.sectorcode ?? 0), actionType: $scope.type, isExport: isExport })).
            success(function (data, status) {
                if (isExport) {
                    $scope.downloadFile(data);
                }
                else {
                    if (data != null && data.length > 0) {
                        $scope.isNoRecord = false;
                        $scope.FinancialDataList = data;
                        if ($scope.FinancialDataList != null && $scope.FinancialDataList.length > 0) {
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
                    } else {
                        $scope.FinancialDataList = [];
                        $scope.isNoRecord = true;
                        $scope.colspan = 10;
                    }
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.FinancialDataList = [];
            });
    };

    $scope.searchFilterFinancialData = function () {
        if ((!$scope.search.societyname || $scope.search.societyname.trim() === '') && (!$scope.search.location || $scope.search.location == 0)
            && (!$scope.search.districtcode || $scope.search.districtcode == 0) && (!$scope.search.sectorcode || $scope.search.sectorcode == 0)) {
            alert('Please select at least one search filter');
            return;
        }
        let searchParams = new URLSearchParams();
        if ($scope.search.societyname) searchParams.set('societyname', $scope.search.societyname);
        if ($scope.search.location) searchParams.set('location', $scope.search.location);
        if ($scope.search.districtcode) searchParams.set('district_code', $scope.search.districtcode);
        if ($scope.search.sectorcode) searchParams.set('sectorcode', $scope.search.sectorcode);
        if ($scope.search.financial_update) searchParams.set('financial_update', $scope.search.financial_update);
        window.history.replaceState(null, '', '/NcdData/Financialdetails?' + searchParams.toString());
        $scope.getFinancialTrunoverProfitLossDetail(false);
    };

    $scope.ClearFilter = function () {
        $scope.search = { societyname: "", location: "0", districtcode: "0", sectorcode: "0" };
        window.location.href = '/NcdData/Financialdetails';
    }

    $scope.exportExcel = function () {
        $scope.getFinancialTrunoverProfitLossDetail(true);
    }
    $scope.getSectorDetail = function () {
        $scope.SectorDataList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetNCDSectorDetails").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.NCDSectorDetail != null) {
                    $scope.SectorDataList = JSON.parse(data.Result.Data.NCDSectorDetail);
                    $scope.colsort = "";
                    $scope.reverseSort = false;
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $scope.SectorDataList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
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

