var AppClasses = angular.module("KVSSReportMod", ['angularUtils.directives.dirPagination']);
AppClasses.controller("KVSSReportCnt", function ($scope, $http, $rootScope) {
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
        if ($scope.Filter.FilterType == 'Date') {
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
        $scope.GetSupplyReport();
        $scope.searchRequest($scope.GetSupplyReport);
    }
    /*------------End Paging and Filter-----------------*/

    $scope.Type = "FY";
    $scope.ReportType = "Quantity";
    $scope._language = 'en-us';
    $scope._language = _language;
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

        $scope.ClearUnitForm = function () {
            $scope.UnitMaster = null;
        };
    }
    $scope.ReportList = [];
    $scope.GetSupplyReport = function () {
        // $scope.Filter.Year = "";
        $(".blockUI").fadeIn("slow");
        $scope.ReportList = [];
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/FertilizerSupply/GetAgricultureDirectorReport", JSON.stringify({ Details: jsonParameters, Type: $scope.Type, ReportType: $scope.ReportType })).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                // $scope.OrderList = [];
                $scope.ReportList = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.ReportList = [];
                $(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });
    };
    $scope.GetSupplyReport();

    $scope.ReportTypeChange = function () {

        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        if ($scope.Type == 'Date') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            // $scope.AddCalender();
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }


        $scope.searchRequest($scope.GetSupplyReport);
    }
    $scope.ReportExportList = [];
    $scope.ExportData = function () {
        $(".blockUI").fadeIn("slow");
        $scope.ReportExportList = [];
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": 1, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "IsAllRecords": true };
        return $http.post(SiteUrl + "/FertilizerSupply/GetAgricultureDirectorReportExport", JSON.stringify({ Details: jsonParameters, Type: $scope.Type, ReportType: $scope.ReportType })).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                // $scope.OrderList = [];
                //$scope.ReportExportList = JSON.parse(data.DataList);   
                var bytes = $scope.base64ToArrayBuffer(data);
                //var byte = $scope.toUTF8Array(data);
                var blob = new Blob([bytes], { type: "application/pdf" });
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                var fileName = "Report_" + $scope.Type;
                link.download = fileName;
                link.click();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.ReportExportList = [];
                $(".blockUI").fadeOut("slow");
                //alert('Error while fetching society list data.');
            });

    }


    $scope.base64ToArrayBuffer = function (base64) {
        var binaryString = window.atob(base64);
        var binaryLen = binaryString.length;
        var bytes = new Uint8Array(binaryLen);
        for (var i = 0; i < binaryLen; i++) {
            var ascii = binaryString.charCodeAt(i);
            bytes[i] = ascii;
        }
        return bytes;
    }


    $scope.FinancialYear = []

    $scope.YearList = []
    $scope.MasterModel = {
        FinancialYear: ""
    }
    $scope.MasterModel.FinancialYear = "";
    $scope.TodayDate = "";
    $scope.CurrentFY = ""
    $scope.GetCurrentDate = function () {
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!

        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        
        $scope.TodayDate = dd + '/' + mm + '/' + yyyy;
        $scope.MasterModel.FinancialYear = yyyy + '-' + (parseInt(yyyy.toString().substring(2, 4)) + 1);
        $scope.FinancialYear.push({ FinancialYear: $scope.MasterModel.FinancialYear, Id: yyyy });
        for (var i = 1; i <= 10; i++) {
            $scope.FinancialYear.push({ FinancialYear: (yyyy - i) + '-' + (parseInt((yyyy - i).toString().substring(2, 4)) + 1), Id: (yyyy - i) });
        }

        $scope.YearList.push({ year: yyyy });
        for (var i = 1; i <= 10; i++) {
            $scope.YearList.push({ year: (yyyy - i) });
        }


    }
    $scope.GetCurrentDate();

    $scope.SelectReportType = function (type) {
        $scope.ReportType = type;
        $scope.searchRequest($scope.GetSupplyReport);
    }

    $scope.KVSSList = []
    $scope.KVSSList = function () {
        // $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/FertilizerSupply/GetKVSSByDistrictId", { "WhereClause": " " }).
            success(function (data, status) {
                //  $(".blockUI").fadeOut("slow");
                $scope.KVSSList = data;
                //if ($scope.FertilizerList.length == 1) {
                //    $scope.MasterModel.FertilizerId = $scope.FertilizerList[0].fertilizer_id.toString();
                //    $scope.MasterModel.Unit = $scope._language == 'en-us' ? $scope.FertilizerList[0].unit_en : $scope.FertilizerList[0].unit_hi;
                //}
            }).
            error(function (data, status) {
                $scope.KVSSList = [];
                //alert('Error while fetching society list data.');
            });
    };
    $scope.KVSSList();

    $scope.SocietyList = [];
    $scope.GetSocietyList = function () {
        if ($scope.ReportType == 'GSSWise') {
            $(".blockUI").fadeIn("slow");
            $http.post(SiteUrl + "/FertilizerSupply/GetGSSByBlockId", { BlockId: $scope.Filter.KVSSID }).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    $scope.SocietyList = [];
                    $scope.SocietyList = data;                  
                }).
                error(function (data, status) {
                    $scope.SocietyList = [];
                    $(".blockUI").fadeOut("slow");
                    alert('Error while fetching society list data.');
                });
        };
    }
});