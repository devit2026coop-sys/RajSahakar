var CCSConsolidatedReportController = angular.module("CCSConsolidatedReportCnt", ['angularUtils.directives.dirPagination']);
CCSConsolidatedReportController.controller("CCSConsolidatedReportMod", function ($rootScope ,$scope, $http) {

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
        $scope.DataPaging.TotalRecords = $rootScope.DataPagingDetails.TotalRecords;
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
        $scope.SearchData();
    }
    /*------------End Paging and Filter-----------------*/

    $scope.divComplaintList = false;
    $scope.IsDisabled = true;
    $scope.IsControlDisable = false;
    $scope.DistrictId = 0;
    $scope.ShowSubmitButton = true;

    $scope.ShowDivForPrint = function (complaintId) {

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Citizen/GetComplaintDetailsByComplaintId", { "ComplaintIdEnc": complaintId }).
            success(function (data, status) {
                if (data.length === 0) {
                    $("#showModal").modal("hide");
                    $scope.ApplicationDetail = {};
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.CCSConsolidatedReportHeading, _language === 'en-us' ? 'No data found' : 'कोई डेटा नहीं मिला');
                }
                else {
                    $scope.ApplicationDetail = data;
                    $("#showModal").modal("show");
                    $(".se-pre-con").fadeOut("slow");
                }
            }).
            error(function (data, status) {
                $scope.ComplaintList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

   
    //START : Get Complaint Details
    $scope.GetComplaintDetail = function () {
        //alert($scope.CCSReport.CCSId);
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/Society/GetCCSConsolidatedReportData", { Details: jsonParameters, "Type": $scope.CCSReport.Type, "fromDate": $('#txtReportFromDate').val(), "toDate": $('#txtReportToDate').val(), "districtId": ($scope.CCSReport.Type == 2 ? 0 : $scope.CCSReport.DistrictId), "CCSId": $scope.CCSReport.CCSId }).
            success(function (data, status) {
                if (data.Result === false) {
                    $scope.divComplaintList = false;
                    $scope.ReportData = [];
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.CCSConsolidatedReportHeading, _language === 'en-us' ? 'No data found' : 'कोई डेटा नहीं मिला');
                }
                else {
                    $scope.divComplaintList = true;
                    $scope.ReportData = data.DataList;
                    $rootScope.DataPagingDetails = data.DataPaging;
                    $rootScope.SetDataPaging();     

                    $scope.colsort = "";
                    $scope.reverseSort = false;
                    
                    $(".se-pre-con").fadeOut("slow");
                   // if ($scope.CCSReport.Type == 2 && $scope.DataPaging.currentPageID==1) {
                        //$scope.CCSReport.DistrictId = "";
                     ///   $scope.GetCCSListNew();
                  //  }
                }
            }).
            error(function (data, status) {
                $scope.ComplaintList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    // END :  Get Complaint Details
   
    //START : Get Complaint Details
    $scope.GetComplaintDetailNew = function () {

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Society/GetCCSocietyComplentForPrint", { "fromDate": $('#txtReportFromDate').val(), "toDate": $('#txtReportToDate').val(), "districtId": $scope.CCSReport.DistrictId, "CCSId": $scope.CCSReport.CCSId }).
            success(function (data, status) {
                if (data.Result === false) {
                    $scope.divComplaintList = false;
                    $scope.ReportData = [];
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.CCSConsolidatedReportHeading, _language === 'en-us' ? 'No data found' : 'कोई डेटा नहीं मिला');
                }
                else {

                    $scope.divComplaintList = true;
                    $scope.ReportData = data.data;

                    $scope.colsort = "";
                    $scope.reverseSort = false;

                    $scope.sortData = function (column) {
                        $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                        $scope.colsort = column;
                    };


                    $scope.getSortClass = function (column) {
                        if ($scope.colsort === column) {
                            return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                        }
                        return '';
                    };

                    $(".se-pre-con").fadeOut("slow");
                }
            }).
            error(function (data, status) {
                $scope.ComplaintList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    // END :  Get Complaint Details


    //START : CCSComplaintDetails
    $scope.ExportData = function (id) {
        $('#hdfcomplaintid').val(id);
        $('#btnGeneratePDF').click();
    };
    // END : CCSComplaintDetails

    //START : Download Zip
    $scope.DownloadZip = function (id) {
        $('#hdfcomplaintid').val(id);
        $('#btnZipDownload').click();
    };
    // END : Download Zip

    // START : Page load Function
    $scope.OnPageLoad = function () {
        $scope.CCSReport = {};
        $scope.CCSReport.Type = "1";
     
        $scope.DistrictList();
        r = $('#hdfLoggedInUserDistrictId').val();

        if (parseInt(r, 10) > 0) {
            $scope.IsControlDisable = true;
           
            $scope.CCSReport["DistrictId"] = (r).toString();
            $scope.GetCCSListNew();
            $scope.CCSReport.DistrictId = $('#hdfLoggedInUserDistrictId').val();
        }
        else {
            $scope.IsControlDisable = false;
            $scope.CCSReport.DistrictId = "";
        }
        $scope.CCSReport.CCSId = "0";
        $scope.GetComplaintDetail();
        $scope._language = _language;
        if (_language !== 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }

        $('.SetCalender').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy"
        });
    };
    // END : Page load Function


    // START : Get District List
    $scope.DistrictList = function () {
        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=0").
            success(function (data, status) {
                $scope.Districts = data;
                $scope.CCSReport.DistrictId = $scope.CCSReport.DistrictId;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.Districts = [];
                $(".se-pre-con").fadeOut("slow");
            });

    };
    // END : Get District List

    // START : Get CCS List
    $scope.GetCCSList = function () {
        $(".se-pre-con").fadeIn("slow");
        var model = [{ "Key": "SocietyRegOfficeDetails.DISTRICT_ID", "Value": $('#hdfLoggedInUserDistrictId').val() }];
        $http.post(SiteUrl + "/Society/GetCCSocietyComplentForPrint", { paramlist: model }).
            success(function (data, status) {
                $scope.CCSList = data;
                $scope.CCSReport.CCSId = '0';
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching list");
                $(".se-pre-con").fadeOut("slow");
            });
        //$(".se-pre-con").fadeIn("slow");
    };

    $scope.GetCCSListNew = function () {
        //alert($scope.CCSReport.DistrictId);
        $('#hdfLoggedInUserDistrictId').val($scope.CCSReport.DistrictId);
        $(".se-pre-con").fadeIn("slow");
        $scope.GetComplaintDetail();
        var model = [{ "Key": "SocietyRegOfficeDetails.DISTRICT_ID", "Value": $('#hdfLoggedInUserDistrictId').val() }];
        $http.post(SiteUrl + "/Society/GetCCSData", { paramlist: model, Type: $scope.CCSReport.Type }).
            success(function (data, status) {
                $scope.CCSList = data;
                $scope.CCSReport.CCSId = '0';
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching list");
                $(".se-pre-con").fadeOut("slow");
            });
        //$(".se-pre-con").fadeIn("slow");
    };

    $scope.GetCCSList = function () {
        $(".se-pre-con").fadeIn("slow");
        var model = [{ "Key": "SocietyRegOfficeDetails.DISTRICT_ID", "Value": $('#hdfLoggedInUserDistrictId').val() }];
        $http.post(SiteUrl + "/Society/CCSData", { paramlist: model }).
            success(function (data, status) {
                $scope.CCSList = data;
                $scope.CCSReport.CCSId = '';
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching list");
                $(".se-pre-con").fadeOut("slow");
            });
        //$(".se-pre-con").fadeIn("slow");
    };

    // END : Get CCS List

    //START : Clear Form
    $scope.ClearForm = function () {
        r = $('#hdfLoggedInUserDistrictId').val();
        if (parseInt(r, 10) > 0) {
            $scope.IsControlDisable = true;

            $scope.CCSReport["DistrictId"] = (r).toString();
            $scope.GetCCSListNew();
            $scope.CCSReport.DistrictId = $('#hdfLoggedInUserDistrictId').val();
        }
        else {
            $scope.IsControlDisable = false;
            $scope.CCSReport.DistrictId = "";
        }
        $scope.CCSList = [];
        $scope.CCSReport.CCSId = '';
        $('#txtReportFromDate').val('');
        $('#txtReportToDate').val('');
        $scope.divComplaintList = false;
        $scope.ReportData = [];
    };
    //END : Clear Form

    // START : On Page Load
    $scope.OnPageLoad();
    // END : On Page Load
});