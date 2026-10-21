
var MasterMod = angular.module('FertilizerDemandDetailsMod', ['angularUtils.directives.dirPagination']);

MasterMod.controller("FertilizerDemandDetailsCtrl", function ($scope, $rootScope, $http, $timeout) {
    $scope._language = 'en-us';
    $scope.isNoRecord = false;
    $scope.colspan = 0;
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.FertilizerDataList = [];
    $scope.FertilizerCFDataList = [];
    $scope.FertilizerPDataList = [];
    $scope.selectedSocietyType = "";
    $scope.DISTRICT_ID = "0";
    $scope.SocietyType = "";
    $scope.ReportType = "1";


    /*------------Paging and Filter-----------------*/

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
        $scope.GetFertilizerDemandDetail(false);
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
                $scope.GetFertilizerDemandDetail(false);
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
        //alert($scope.ReportType);
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
        if ($scope.ReportType == "1") {
            $("#RP1").show();
            $("#RP2").hide();
            $("#RP3").hide();
        } else if ($scope.ReportType == "2") {
            $("#RP1").hide();
            $("#RP2").show();
            $("#RP3").hide();
        }
        else if ($scope.ReportType == "3") {
            $("#RP1").hide();
            $("#RP2").hide();
            $("#RP3").show();
        }
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
        }
        $("#RP1").show();
        $("#RP2").hide();
        $("#RP3").hide();
        $scope.GetFertilizerDemandDetail(false);
    };
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $("#DivisionId").val()).
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };

    $scope.GetDistrictList();
    $scope.ClearSearch = function () {
        $scope.DISTRICT_ID = "0";
        $scope.SocietyType = "";
        $scope.ReportType = "1";
        $scope.GetFertilizerDemandDetail();
    };
    $scope.exportToExcel = function () {
        $scope.GetFertilizerDemandDetail(true);
    };
    $scope.GetFertilizerDemandDetail = function (isExport) {
        $(".se-pre-con").fadeIn("slow");

        if (!isExport)
            $rootScope.DataPagingDetails = {};
        var jsonParameters = {
            "TotalRecords": ($scope.DataPaging.TotalRecords ?? 0),
            "PageNumber": $scope.DataPaging.currentPageID,
            "PageSize": $scope.pageSize,
            "SortingColumn": $scope.DataPaging.sortExpression,
            "SortingOrder": $scope.DataPaging.sortDirection,
            "SearchParameter": $scope.DataPaging.searchParameters
        };

        return $http.post(SiteUrl + "/Mis/GetFertilizerDemandMultiDetail?actionType=GetFertilizerDetailforExcel&type=0&DemandRequestId=0", JSON.stringify({
            Details: jsonParameters, isExport: isExport
        })).success(function (responcedata, status) {
            var data = responcedata;
            if (isExport) {
                $scope.downloadFile(data);
            }
            else {
                if (data != null && data.length > 0) {
                    $scope.isNoRecord = false;
                    if ($scope.ReportType == "1") {
                        $scope.FertilizerDataList = data;
                        $scope.FertilizerCFDataList = [];
                        $scope.FertilizerPDataList = [];
                    } else if ($scope.ReportType == "2") {
                        $scope.FertilizerCFDataList = data;
                        $scope.FertilizerDataList = [];
                        $scope.FertilizerPDataList = [];
                    } else {
                        $scope.FertilizerPDataList = data;
                        $scope.FertilizerDataList = [];
                        $scope.FertilizerCFDataList = [];
                    }
                    if ($scope.FertilizerDataList != null && $scope.FertilizerDataList.length > 0) {
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
                                return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                            }
                            return '';
                        };
                    }
                    if ($scope.FertilizerCFDataList != null && $scope.FertilizerCFDataList.length > 0) {
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
                                return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                            }
                            return '';
                        };
                    }
                    if ($scope.FertilizerPDataList != null && $scope.FertilizerPDataList.length > 0) {
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
                                return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                            }
                            return '';
                        };
                    }
                }
                else {
                    $scope.FertilizerDataList = [];
                    $scope.FertilizerCFDataList = [];
                    $scope.FertilizerPDataList = [];
                    $scope.isNoRecord = true;
                    $scope.colspan = 11;
                }
            }

            $(".se-pre-con").fadeOut("slow");

        })
            .error(function (data1, status) {
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