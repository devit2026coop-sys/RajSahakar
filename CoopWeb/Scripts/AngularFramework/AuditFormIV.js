var MasterMod = angular.module('AuditFormIVModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("AuditFormIVCont", function ($scope, $rootScope, $http) {
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

    $scope.SearchText = "";
    $scope.Filter = [];
    $scope.ClearSearch = function () {
        $scope.DISTRICT_ID = "";
        $scope.PLDB_ID = "0";
        $scope.PLDB_Branch_ID = "0";
        $scope.q = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        window.location = "../MIS/AuditFormIV";
    }
    /*------------End Paging and Filter-----------------*/
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;

    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $("#DivisionId").val()).
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0") {
                    $scope.DISTRICT_ID = $("#hdnDistrictId").val()
                    $("#ddrDistrictId").attr("disabled", "disabled");
                }
                // $scope.BindFY();

            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };

    $scope.GetDistrictList();


    $scope.GetAuditMPRBackYearFormIVReport = function () {
        $(".se-pre-con").fadeIn("slow");

        $scope.ShowSummary = true;

        return $http.get(SiteUrl + "/MIS/GetAuditMPRBackYearFormIVReport", {
            params: {
                FinancialYearId: $scope.FinYear,
                Month: $scope.Month
            }
        })
            .then(function (response) {

                $scope.AuditMPRBackYearFormIVReport = response.data || [];

                var excludedColumns = ["DistrictId", "DistrictName", "SortOrder", "Metric", "Total"];

                $scope.CommitteeColumns = [];

                if ($scope.AuditMPRBackYearFormIVReport.length > 0) {
                    $scope.CommitteeColumns = Object.keys($scope.AuditMPRBackYearFormIVReport[0]
                    ).filter(function (column) {
                        return excludedColumns.indexOf(column) === -1;
                    });
                }

                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };

                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? "arrow-down" : "arrow-up";
                    }

                    return "";
                };

                console.log("Report:", $scope.AuditMPRBackYearFormIVReport);
                console.log("Committee Columns:", $scope.CommitteeColumns);
            })
            .catch(function (error) {
                console.error("Error loading Form III Report:", error);
            })
            .finally(function () {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.GetFinancialYear = function () {
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1")
            .success(function (data) {
                if (data != null) {
                    $scope.FinancialYearList = data
                        .map(function (item) {
                            return {
                                FinYearId: item.FIN_YEAR_MASTR_ID,
                                FinYearName: item.FIN_YEAR_START + '-' + item.FIN_YEAR_END
                            };
                        })
                        .filter(function (item) {
                            return item.FinYearId == '1086' || item.FinYearId == '1085';
                        });

                    if ($scope.FinancialYearList.length > 0) {
                        $scope.FinYear = '1086';
                    }
                }
                else {
                    $scope.FinancialYearList = [];
                }
                $(".se-pre-con").fadeOut("slow");

            }).error(function () {
                $scope.FinancialYearList = [];
            });
    };


    //$scope.GetSocietyLocationList = function () {
    //    $(".se-pre-con").fadeIn("slow");
    //    var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
    //    return $http.post(SiteUrl + "/MIS/GetAuditMPRCurrentYearReport", JSON.stringify({ Details: jsonParameters })).
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


    $scope.exportAllToExcel = function () {
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, PageSize: 100000, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/MIS/SocietyLocationReportList", jsonParameters)
            .success(function (data) {
                var allData = JSON.parse(data.DataList);
                if (allData.length == 0) {
                    alert("No records found");
                    $(".se-pre-con").fadeOut("slow");
                    return;
                }
                exportJsonToExcel(allData, "Society Location Report List");
                $(".se-pre-con").fadeOut("slow");
            })
            .error(function () {
                alert("Error while exporting data");
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.formatCommitteeValue = function (value) {
        if (value === null || value === undefined || value === '') {
            return '0';
        }

        var num = Number(value);

        if (!isFinite(num)) {
            return value;
        }

        // Remove trailing zeros; keep maximum 2 decimal places
        return num.toFixed(2).replace(/\.?0+$/, '');
    };

    function exportJsonToExcel(data, fileName) {

        var excludedColumns = [
            "SocietyId",
            "ImageUrl"
        ];
        var exportData = data.map(function (item, index) {

            var newItem = {};
            newItem["S.No"] = index + 1;

            Object.keys(item).forEach(function (key) {

                if (excludedColumns.indexOf(key) === -1) {
                    newItem[key] = item[key];
                }

            });

            return newItem;
        });

        var worksheet = XLSX.utils.json_to_sheet(exportData);
        var workbook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workbook, worksheet, "Data");
        XLSX.writeFile(workbook, fileName + ".xlsx");
    }

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

            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }

        $scope.ShowSummary = false;
        $scope.GetFinancialYear();
    };

    $scope.OnPageLoad();




});

//$scope.tableToExcel = function () {
//    $scope.GetOTSList(true);
//}
function tableToExcel(tableid, filename = '') {

    var table = document.getElementById(tableid);
    var html = table.outerHTML.replace(/ /g, '%20');

    var file = filename ? filename + '.xls' : 'excel_data.xls';

    var downloadlink = document.createElement("a");
    document.body.appendChild(downloadlink);

    downloadlink.href = 'data:application/vnd.ms-excel,' + html;
    downloadlink.download = file;

    downloadlink.click();
    document.body.removeChild(downloadlink);
}

MasterMod.filter('inrCurrency', function () {
    return function (input) {
        if (!input)
            return input;
        var num = parseFloat(input);
        if (isNaN(num)) return input;
        return num.toLocaleString('en-IN', { style: 'currency', currency: 'INR' });
    };
});
