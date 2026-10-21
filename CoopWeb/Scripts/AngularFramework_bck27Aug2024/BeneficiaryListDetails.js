
var MasterMod = angular.module('BeneficiaryDetailsListMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("BeneficiaryDetailsListCnt", function ($rootScope, $scope, $location, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.divClass = false;
    $scope.BeneficiaryDetailList = [];
    $scope.TotalBeneficiaryDetailList = [];

    const urlParams = new URLSearchParams(window.location.search);
    $scope.requestType = urlParams.get('type');
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
        $scope.GetBeneficiaryList(urlParams.get('type'), false);
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
                $scope.GetBeneficiaryList(urlParams.get('type'), false);
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

            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }

    };

    $scope.GetBeneficiaryList = function (type, isExport) {
        $(".se-pre-con").fadeIn("slow");
        if (!isExport)
            $rootScope.DataPagingDetails = {};

        var url = ($location.absUrl());
        var params = url.split('?');
        var params_1 = params[1].split('&');

        $scope.districtId = params_1[0].split('=')[1] != undefined ? params_1[0].split('=')[1] : -1;
        $scope.FinancialYearId = params_1[1].split('=')[1] != undefined ? params_1[1].split('=')[1] : -1;
        $scope.InstallmentTypeId = params_1[2].split('=')[1] != undefined ? params_1[2].split('=')[1] : -1;
        $scope.subDistrictName = decodeURIComponent(params_1[4].split('=')[1] != undefined ? params_1[4].split('=')[1] : '');
        $scope.villageName = decodeURIComponent(params_1[5].split('=')[1] != undefined ? params_1[5].split('=')[1] : '');

        var jsonParameters = { "TotalRecords": ($scope.DataPaging.TotalRecords ?? 0), "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };

        return $http.post(SiteUrl + "/PMKisan/GetBeneficiaryDetailList", JSON.stringify({ Details: jsonParameters, financialYearId: ($scope.FinancialYearId ?? 0), installmentType: ($scope.InstallmentTypeId ?? 0), districtId: ($scope.districtId ?? 0), type: type, subDistrictName: $scope.subDistrictName, villageName: $scope.villageName, isExport: isExport })).

            /*$http.post(SiteUrl + "/PMKisan/GetBeneficiaryDetailList/?financialYearId=" + ($scope.FinancialYearId ?? 0) + "&installmentType=" + ($scope.InstallmentTypeId ?? 0) + "&districtId=" + ($scope.districtId ?? 0)).*/
            success(function (data, status) {
                if (isExport) {
                    $scope.downloadFile(data);
                }
                else {
                    if (data != null && data.length>0) {
                        $scope.BeneficiaryDetailList = data; // JSON.parse(data.Result.Data.BeneficiaryDetailList);
                        //$scope.TotalBeneficiaryDetailList = JSON.parse(data.Result.Data.TotalBeneficiaryDetailList);

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
                console.log(data);
                $(".se-pre-con").fadeOut("slow");
                $scope.BeneficiaryDetailList = [];
                $scope.TotalBeneficiaryDetailList = []
            });
    };
    $scope.exportExcel = function () {
        $scope.GetBeneficiaryList(urlParams.get('type'), true);
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
    $scope.GetBeneficiaryList(urlParams.get('type'), false);

});

MasterMod.filter('inrNumber', function () {
    return function (input) {
        if (!input) {
            return input;
        }
        var numberString = input.toString();
        var lastThree = numberString.substring(numberString.length - 3);
        var otherNumbers = numberString.substring(0, numberString.length - 3);
        if (otherNumbers !== '') {
            lastThree = ',' + lastThree;
        }
        var result = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
        return result;
    };
});

MasterMod.filter('inrCurrency', function () {
    return function (input) {
        if (!input) return input;

        var num = parseFloat(input);
        if (isNaN(num)) return input;
        return num.toLocaleString('en-IN', { style: 'currency', currency: 'INR' });
    };
});
MasterMod.filter('indianFormat', function () {
    return function (input, t) {
        if (isNaN(input)) return input;

        // Convert input to a number
        input = parseFloat(input);

        if (input >= 10000000) { // 1 Crore or more
            return (t == 'R' ? '₹' : '') + (input / 10000000).toFixed(2).replace(/\.0$/, '') + ' Cr';
        } else if (input >= 100000) { // 1 Lakh to less than 1 Crore
            return (t == 'R' ? '₹' : '') + (input / 100000).toFixed(2).replace(/\.0$/, '') + ' Lac';
        }
        //else if (input >= 1000) { // 1 Thousand to less than 1 Lakh
        //	return (input / 1000).toFixed(2).replace(/\.0$/, '') + ' K';
        //}
        else {
            return (t == 'R' ? '₹' : '') + input; // Less than 1 Thousand
        }
    };
});