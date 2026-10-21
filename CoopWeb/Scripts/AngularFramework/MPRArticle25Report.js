var MasterMod = angular.module('MPRArticle25ReportMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("MPRArticle25ReportCnt", function ($scope, $rootScope, $http) {
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

    //$scope.SearchText = "";
    //$scope.Filter = [];
    //$scope.ClearSearch = function () {
    //    $scope.DISTRICT_ID = "";
    //    $scope.PLDB_ID = "0";
    //    $scope.PLDB_Branch_ID = "0";
    //    $scope.q = "";
    //    $scope.showSearchDate = false;
    //    $scope.showSearchText = true;
    //    $scope.DataPaging.currentPageID = 1;
    //    $scope.DataPaging.searchParameters = [];
    //    window.location = "../MPR/MPRArticle25Report";
    //}
    /*------------End Paging and Filter-----------------*/
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;

    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $("#DivisionId").val()).
            success(function (data, status) {
                $scope.DistrictList = data;
                //if ($("#hdnDistrictId").val() != "0") {
                //    $scope.DISTRICT_ID = $("#hdnDistrictId").val()
                //    $("#ddrDistrictId").attr("disabled", "disabled");
                //} 
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };

    $scope.GetDistrictList();


    $scope.clean = function () {
        $scope.TsActive = 0;
        $scope.TsInactive = 0;
        $scope.TsUnderLiquidation = 0;
        $scope.TsTotal = 0;
        $scope.PmgmActive = 0;
        $scope.PmgmInactive = 0;
        $scope.PmgmUnderLiquidation = 0;
        $scope.PmTotal = 0;
        $scope.CmgmActive = 0;
        $scope.CmgmInactive = 0;
        $scope.CmgmUnderLiquidation = 0;
        $scope.CmTotal = 0;
        $scope.EmgmActive = 0;
        $scope.EmgmInactive = 0;
        $scope.EmgmUnderLiquidation = 0;
        $scope.EmTotal = 0;
        $scope.TmgmCount = 0;
    }

    $scope.GetMPRArticle25Report = function () {
        $http.get(SiteUrl + "/MPR/GetMPRArticle25Report").success(function (data) {
            //console.log(data, "MPRArticle25ReportList");
            $scope.clean();
            $scope.MPRArticle25ReportList = JSON.parse(data.Result.Data.MPRArticle25ReportList);
            $scope.MPRArticle25ReportList.forEach(function (item, indx) {
                $scope.TsActive += parseInt(item.TsActive);
                $scope.TsInactive += parseInt(item.TsInactive);
                $scope.TsUnderLiquidation += parseInt(item.TsUnderLiquidation);
                $scope.TsTotal += parseInt(item.TsTotal);
                $scope.PmgmActive += parseInt(item.PmgmActive);
                $scope.PmgmInactive += parseInt(item.PmgmInactive);
                $scope.PmgmUnderLiquidation += parseInt(item.PmgmUnderLiquidation);
                $scope.PmTotal += parseInt(item.PmTotal);
                $scope.CmgmActive += parseInt(item.CmgmActive);
                $scope.CmgmInactive += parseInt(item.CmgmInactive);
                $scope.CmgmUnderLiquidation += parseInt(item.CmgmUnderLiquidation);
                $scope.CmTotal += parseInt(item.CmTotal);
                $scope.EmgmActive += parseInt(item.EmgmActive);
                $scope.EmgmInactive += parseInt(item.EmgmInactive);
                $scope.EmgmUnderLiquidation += parseInt(item.EmgmUnderLiquidation);
                $scope.EmTotal += parseInt(item.EmTotal);
                $scope.TmgmCount += parseInt(item.TmgmCount);
            })
        }).error(function () {
            alert("Error");
        });
    };

    //$scope.GetSocietyLocationList = function () {
    //    $(".se-pre-con").fadeIn("slow");
    //    var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
    //    return $http.post(SiteUrl + "/MIS/SocietyLocationReportList", JSON.stringify({ Details: jsonParameters })).
    //        success(function (responcedata, status) {
    //            var data = JSON.parse(responcedata.DataList);
    //            $scope.SocietyLocationList = data;
    //            console.log('$scope.SocietyLocationList', $scope.SocietyLocationList);
    //            $rootScope.DataPagingDetails = responcedata.DataPaging;
    //            $rootScope.SetDataPaging();
    //            $scope.colsort = "";
    //            $scope.reverseSort = false;
    //            $scope.sortData = function (column) {
    //                $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
    //                $scope.colsort = column;
    //            };
    //            $(".se-pre-con").fadeOut("slow");
    //            $scope.getSortClass = function (column) {
    //                if ($scope.colsort == column) {
    //                    return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
    //                }
    //                return '';
    //            };
    //        }).
    //        error(function (data1, status) {
    //            $(".se-pre-con").fadeOut("slow");
    //        });
    //};
     
    //$scope.exportAllToExcel = function () {
    //    $(".se-pre-con").fadeIn("slow");
    //    var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, PageSize: 100000, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
    //    $http.post(SiteUrl + "/MIS/SocietyLocationReportList", jsonParameters)
    //        .success(function (data) {
    //            var allData = JSON.parse(data.DataList);
    //            if (allData.length == 0) {
    //                alert("No records found");
    //                $(".se-pre-con").fadeOut("slow");
    //                return;
    //            }
    //            exportJsonToExcel(allData, "Society Location Report List");
    //            $(".se-pre-con").fadeOut("slow");
    //        })
    //        .error(function () {
    //            alert("Error while exporting data");
    //            $(".se-pre-con").fadeOut("slow");
    //        });
    //};

    //function exportJsonToExcel(data, fileName) {
    //    var excludedColumns = [
    //        "SocietyId",
    //        "ImageUrl"
    //    ];
    //    var exportData = data.map(function (item, index) {
    //        var newItem = {};
    //        newItem["S.No"] = index + 1;
    //        Object.keys(item).forEach(function (key) {
    //            if (excludedColumns.indexOf(key) === -1) {
    //                newItem[key] = item[key];
    //            }
    //        });
    //        return newItem;
    //    });

    //    var worksheet = XLSX.utils.json_to_sheet(exportData);
    //    var workbook = XLSX.utils.book_new();
    //    XLSX.utils.book_append_sheet(workbook, worksheet, "Data");
    //    XLSX.writeFile(workbook, fileName + ".xlsx");
    //}

    $scope.OnPageLoad = function () {
        //$(".se-pre-con").fadeIn("slow");
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

            //$scope.ClearUnitForm = function () {
            //    $scope.UnitMaster = null;
            //};
        }
        $scope.GetMPRArticle25Report();
    };

    $scope.OnPageLoad(); 
});
 
//function tableToExcel(tableid, filename = '') {

//    var table = document.getElementById(tableid);
//    var html = table.outerHTML.replace(/ /g, '%20');

//    var file = filename ? filename + '.xls' : 'excel_data.xls';

//    var downloadlink = document.createElement("a");
//    document.body.appendChild(downloadlink);

//    downloadlink.href = 'data:application/vnd.ms-excel,' + html;
//    downloadlink.download = file;

//    downloadlink.click();
//    document.body.removeChild(downloadlink);
//}

//MasterMod.filter('inrCurrency', function () {
//    return function (input) {
//        if (!input)
//            return input;
//        var num = parseFloat(input);
//        if (isNaN(num)) return input;
//        return num.toLocaleString('en-IN', { style: 'currency', currency: 'INR' });
//    };
//});
