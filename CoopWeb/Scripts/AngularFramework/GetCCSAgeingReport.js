var CCSComplaintReportModule = angular.module('GetCCSAgeingReportMod', ['angularUtils.directives.dirPagination']);
CCSComplaintReportModule.controller("GetCCSAgeingReportCnt", function ($rootScope, $scope, $http) {

    /*------START------Variables-------------------------*/

    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.divStateData = true;
    $scope.SearchType = 1;
    $scope.GetActionIdCCSComplaintReport = [];
    $scope.DistrictId = "0";
    $scope.BUDSStatusReport = [];
    $scope.BUDSStatusReport.CCSId1 = '0';
    $scope.BUDSStatusReport.UserRoleName = '';
    $scope.selectState = '0';

    $scope.ReportListType = "0";
    $scope.CCSId = "0";
    $scope.IsnotShow = false;

    $scope.BUDSStatusReport = { DistrictId: 0 };

    $scope.BUDSStatusReport.DistrictId = 0;
    $scope.GetCCSCompalainteReportData = [];
    $scope.CCSList = [];
    $scope.GetCompliantReportWithDistrictIdList = [];

    /*------ENDING------Variables-------------------------*/

    /*------START------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 20,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    };

    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];


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
    };

    $rootScope.PagingRange = function (startPage, endPage, totalPages, step) {
        step = step || 1;
        var input = [];

        for (var tStart = startPage; tStart <= endPage && tStart <= totalPages; tStart++) {
            input.push(tStart);
        }
        return input;
    };

    //$scope.searchRequest = function (callback) {
    //    //$scope.DataPaging.searchParameters = [];
    //    $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
    //    var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
    //    var indx = 1;
    //    $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

    //        if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
    //            if ($(this).is(":checked")) {
    //                var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
    //                $scope.DataPaging.searchParameters.push(parameter);
    //            }
    //        } else {
    //            if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
    //                var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
    //                $scope.DataPaging.searchParameters.push(parameter);
    //            }
    //        }
    //        if (indx == ln) {
    //            $scope.DataPaging.currentPageID = 1;
    //            callback();
    //        }
    //        ++indx;
    //    });
    //};


    $scope.searchRequest = function (callback) {

        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);

        var ln = $('#' + $scope.DataPaging.searchBoxDivId)
            .find('input:text,select,input:checkbox,input:radio').length;

        var indx = 1;

        $('#' + $scope.DataPaging.searchBoxDivId)
            .find('input:text,select,input:checkbox,input:radio')
            .each(function () {

                // Checkbox
                if ($(this).attr("type") == "checkbox") {

                    if ($(this).is(":checked")) {

                        var parameter = {
                            Key: $(this).attr('name') + '#' + indx,
                            Value: $(this).val()
                        };

                        $scope.DataPaging.searchParameters.push(parameter);
                    }
                }

                // Radio Button
                else if ($(this).attr("type") == "radio") {

                    if ($(this).is(":checked")) {

                        var parameter = {
                            Key: $(this).attr('name'),
                            Value: $(this).val()
                        };

                        $scope.DataPaging.searchParameters.push(parameter);
                    }
                }

                // Textbox / Select
                else {

                    if ($(this).val() != '' &&
                        $(this).val() != null &&
                        $(this).attr('name') != undefined) {

                        var parameter = {
                            Key: $(this).attr('name'),
                            Value: $(this).val()
                        };

                        $scope.DataPaging.searchParameters.push(parameter);
                    }
                }

                if (indx == ln) {
                    $scope.DataPaging.currentPageID = 1;
                    callback();
                }

                ++indx;
            });
    };

    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        if ($scope.Filter.FilterType == 'created_dated') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            // $scope.AddCalender();
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    };

    $scope.ClearSearch = function () {
        $scope.DISTRICT_ID = "0";
        $scope.qery = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.FilterType = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        //$scope.searchRequest($scope.GetCSSComplaintAgeingData);
        $scope.GetCSSComplaintAgeingData();
    };

    /*------ENDING------Paging and Filter-----------------*/


    //------START----on page load-----------------//

    //Get District List//
    //$scope.GetDistricts = function () {
    //    $(".se-pre-con").fadeIn("slow");
    //    $scope.Districts = [];
    //    return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
    //        success(function (data, status) {
    //            $scope.Districts = data;

    //            //$scope.BUDSStatusReport.DistrictId = $scope.BUDSStatusReport.DistrictId;
    //        }).
    //        error(function (data, status) {
    //            $scope.Districts = '';
    //        });
    //};

    $scope.CCSList = [];
    $scope.GetCCSListNew = function () { 
        var model = [{ "Key": "SocietyRegOfficeDetails.DISTRICT_ID" }];
        $http.post(SiteUrl + "/Society/GetCCSData", { paramlist: model, Type: $scope.SearchType }).
            success(function (data, status) {
                $scope.CCSList = data; 
            }).
            error(function (data, status) {
                 
            }); 
    };

    //Page Load Data//
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");

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
        }


        $scope.GetCSSComplaintAgeingData();


        /*
        //$scope.GetDistricts().then(function () {
        //    debugger;
        //    if ($('#hdnLoggedInUserDistrictId').val() != "0") {
        //        $scope.IsControlDisable = true;
        //        //alert($('#hdnLoggedInUserDistrictId').val());
        //        $scope.BUDSStatusReport.DistrictId = $('#hdnLoggedInUserDistrictId').val();
        //    }
        //    else {
        //        $scope.IsControlDisable = false;
        //    }
        //    $scope.GetBUDSStatusReport();
        //});
        */

        //$scope.GetDistricts().then(function () {

        //    var districtId = $('#hdnLoggedInUserDistrictId').val();
        //    var role = $('#hdnLoggedInUserRole').val();

        //    // Set district if exists
        //    if (districtId != "0") {
        //        $scope.BUDSStatusReport.DistrictId = districtId;
        //    }

        //    // Disable logic (FINAL)
        //    //if (districtId != "0" || !role || role.trim() === "") {
        //    //    $scope.IsControlDisable = true;
        //    //} else {
        //    //    $scope.IsControlDisable = false;
        //    //}

        //    if ((districtId != "0" && role && role === "Deputy Registrar") || (districtId = "0" && !role && role === "")) {
        //        //if ((districtId != "0" && role && role.trim() === "Deputy Registrar") || (districtId = "0" && role && role.trim() === "ADMINISTRATOR")) {
        //        $scope.IsControlDisable = true;
        //    } else {
        //        $scope.IsControlDisable = false;
        //    }

        //    //$scope.GetBUDSStatusReport();
        //});

        /*
        //if ($('#hdnLoggedInUserRole').val() != "") {
        //    debugger;
        //    $scope.BUDSStatusReport.UserRoleName = $('#hdnLoggedInUserRole').val();
        //    if ($scope.BUDSStatusReport.UserRoleName == "ADMINISTRATOR") {
        //        $scope.IsControlDisable = true;
        //    }
        //    else {
        //        $scope.IsControlDisable = false;
        //    }
        //}
        //else {
        //    $scope.IsControlDisable = false;
        //}
        */

        var role = $('#hdnLoggedInUserRole').val();
        $scope.BUDSStatusReport.UserRoleName = role;

        // Disable if role is empty
        if (!role || role.trim() === "ADMINISTRATOR") {
            $scope.IsControlDisable = true;
        } else {
            $scope.IsControlDisable = false;
        }

        $scope.GetCCSListNew();

        $(".se-pre-con").fadeOut("slow");
    };
    //------ENDING---on page load-----------------//    

    $scope.DatabyDistricts = [];

    //$scope.clean = function () {
    //    $scope.Total = 0;
    //    $scope.NoticeIsssue = 0;
    //    $scope.IstagasaIssue = 0;
    //    $scope.NotRelevant = 0;
    //    $scope.DisposeAsPerRequestFromComplainer = 0;
    //    $scope.DisposeByCort = 0;
    //    $scope.Reject = 0;
    //    $scope.Dispose = 0;
    //    $scope.InstgasaPreparedandmadeavailabletoPP = 0;
    //    $scope.IstgasaSubmittedtoCourt = 0;

    //}

    $scope.clean = function () {
        $scope.Total = 0;
        $scope.RajSahakarPortal_Complaints_Count = 0;
        $scope.Other_Complaints_Count = 0;
        $scope.Total_Complaints_Count = 0;
        $scope.Documents_Received_Count = 0;
        $scope.Notices_Issued_Count = 0;
        $scope.Hearing_Documents_Count = 0;
        $scope.Pp_Documents_Shared = 0;
        $scope.Court_Filed_Cases = 0;
        $scope.Buds_Act_FIR_Cases = 0;
        $scope.FIR_Registered_Count = 0;
    };


    //$scope.GetBUDSStatusReport = function () {
    //    $(".se-pre-con").fadeIn("slow");
    //    $scope.DatabyDistricts = [];
    //    var districtId = $scope.BUDSStatusReport.DistrictId == undefined ? "0" : $scope.BUDSStatusReport.DistrictId;
    //    var CCSId1 = $scope.BUDSStatusReport.CCSId1 == undefined ? "0" : $scope.BUDSStatusReport.CCSId1;
    //    // alert(districtId);

    //    // 23427
    //    //  $http.get(SiteUrl + "/MIS/GetBUDSStatusReport?DistrictId=" + districtId    ). "&Css=" + CCSId1 */
    //    $http.get(SiteUrl + "/MIS/GetBUDSStatusReport?DistrictId=" + districtId).
    //        success(function (data, status) {
    //            debugger;
    //            $scope.clean();
    //            $scope.DatabyDistricts = data;

    //            //$scope.DatabyDistricts.forEach(function (item, indx) {
    //            //    $scope.Total += parseInt(item.Total);
    //            //    $scope.NoticeIsssue += parseInt(item.NoticeIsssue);
    //            //    $scope.IstagasaIssue += parseInt(item.IstagasaIssue);
    //            //    $scope.NotRelevant += parseInt(item.NotRelevant);
    //            //    $scope.DisposeAsPerRequestFromComplainer += parseInt(item.DisposeAsPerRequestFromComplainer);
    //            //    $scope.DisposeByCort += parseInt(item.DisposeByCort);
    //            //    $scope.Reject += parseInt(item.Reject);
    //            //    $scope.Dispose += parseInt(item.Dispose);
    //            //    $scope.InstgasaPreparedandmadeavailabletoPP += parseInt(item.InstgasaPreparedandmadeavailabletoPP);
    //            //    $scope.IstgasaSubmittedtoCourt += parseInt(item.IstgasaSubmittedtoCourt);

    //            //    //  $scope.GetCCSListNew();
    //            //});

    //            $scope.DatabyDistricts.forEach(function (item) {
    //                debugger;
    //                $scope.Total += Number(item.Total_Complaints_Count || 0);

    //                $scope.RajSahakarPortal_Complaints_Count += Number(item.RajSahakarPortal_Complaints_Count || 0);
    //                $scope.Other_Complaints_Count += Number(item.Other_Complaints_Count || 0);
    //                $scope.Total_Complaints_Count += Number(item.Total_Complaints_Count || 0);

    //                $scope.Documents_Received_Count += Number(item.Documents_Received_Count || 0);
    //                $scope.Notices_Issued_Count += Number(item.Notices_Issued_Count || 0);
    //                $scope.Hearing_Documents_Count += Number(item.Hearing_Documents_Count || 0);

    //                $scope.Pp_Documents_Shared += Number(item.Pp_Documents_Shared || 0);
    //                $scope.Court_Filed_Cases += Number(item.Court_Filed_Cases || 0);
    //                $scope.Buds_Act_FIR_Cases += Number(item.Buds_Act_FIR_Cases || 0);
    //                $scope.FIR_Registered_Count += Number(item.FIR_Registered_Count || 0);
    //            });

    //            $scope.IsDistrictwiseNot = false;
    //            $scope.IsDistrictwise = true;
    //            $(".se-pre-con").fadeOut("slow");
    //        }).
    //        error(function (data, status) {
    //            $(".se-pre-con").fadeOut("slow");
    //            $scope.Districts = '';
    //        });
    //};
    //


    $scope.GetCSSComplaintAgeingData = function () {

        $(".se-pre-con").fadeIn("slow");

        $scope.ComplaintAgeingData = [];        

        var jsonParameters = {
            TotalRecords: $scope.DataPaging.TotalRecords,
            PageNumber: $scope.DataPaging.currentPageID,
            PageSize: $scope.pageSize,
            SortingColumn: $scope.DataPaging.sortExpression,
            SortingOrder: $scope.DataPaging.sortDirection,
            SearchParameter: $scope.DataPaging.searchParameters
        };

        $http.post(SiteUrl + "/MIS/GetCSSComplaintAgeingData" , JSON.stringify({ Details: jsonParameters }))
            .success(function (data) {

                console.log("Response", data);

                if (data && data.ComplaintAgeingData) {

                    $scope.ComplaintAgeingData = JSON.parse(data.ComplaintAgeingData);

                    $scope.DataPagingDetails = data.DataPaging;

                    $rootScope.DataPagingDetails = data.DataPaging;

                    $scope.DataPaging.currentPageID = data.DataPaging.PageNumber;

                    $rootScope.SetDataPaging();
                }
                else {

                    $scope.ComplaintAgeingData = [];
                }

                $(".se-pre-con").fadeOut("slow");
            })
            .error(function (error) {

                console.log(error);

                $scope.ComplaintAgeingData = [];

                $(".se-pre-con").fadeOut("slow");
            });
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

    $scope.GetBUDSStatusReport = function () {
        $(".se-pre-con").fadeIn("slow");

        $scope.DatabyDistricts = [];

        var districtId = $scope.BUDSStatusReport.DistrictId || "0";
        var userId = $('#hdnLoggedInUserId').val() || "0";

        // If UserId = 0 don't call API
        if (userId == "0") {
            $scope.clean();
            $scope.DatabyDistricts = [];
            $(".se-pre-con").fadeOut("slow");
            return;
        }

        $http.get(SiteUrl + "/MIS/GetBUDSStatusReport?DistrictId=" + districtId + "&UserId=" + userId)
            .success(function (data, status) {

                $scope.clean();
                // Handle null / empty response
                if (!data || data.length === 0) {
                    $scope.DatabyDistricts = [];
                    $(".se-pre-con").fadeOut("slow");
                    return;
                }

                $scope.DatabyDistricts = data;

                angular.forEach($scope.DatabyDistricts, function (item) {

                    //$scope.Total += Number(item.Total_Complaints_Count || 0);

                    $scope.RajSahakarPortal_Complaints_Count += Number(item.RajSahakarPortal_Complaints_Count || 0);
                    $scope.Other_Complaints_Count += Number(item.Other_Complaints_Count || 0);
                    $scope.Total_Complaints_Count += Number(item.RajSahakarPortal_Complaints_Count || 0) + Number(item.Other_Complaints_Count || 0);

                    $scope.Documents_Received_Count += Number(item.Documents_Received_Count || 0);
                    $scope.Notices_Issued_Count += Number(item.Notices_Issued_Count || 0);
                    $scope.Hearing_Documents_Count += Number(item.Hearing_Documents_Count || 0);

                    $scope.Pp_Documents_Shared += Number(item.Pp_Documents_Shared || 0);
                    $scope.Court_Filed_Cases += Number(item.Court_Filed_Cases || 0);
                    $scope.Buds_Act_FIR_Cases += Number(item.Buds_Act_FIR_Cases || 0);
                    $scope.FIR_Registered_Count += Number(item.FIR_Registered_Count || 0);
                });
                $scope.IsDistrictwiseNot = false;
                $scope.IsDistrictwise = true;

                $(".se-pre-con").fadeOut("slow");
            })
            .error(function () {
                $scope.clean();
                $scope.DatabyDistricts = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.BackToList = function () {
        $scope.IsnotShow = false;
    }

    $scope.IsMultiState = false;

    $scope.GetCCSList = function (ActionId, item) {

        $scope.DISTRICT_ID = item.DISTRICT_ID;
        $scope.ReportListType = ActionId;
        ////$scope.IsMultiState = item.DISTRICT_ENG.indexOf('Multi State') > -1 ? 1 : item.DISTRICT_ENG.indexOf('Single State') > -1 ? 2 : 0;
        //$scope.searchRequest($scope.GetCCSCompalainteReportList);
    }

    //$scope.CCSList = [];
    //$scope.GetCCSListNew = function () {

    //    $(".se-pre-con").fadeIn("slow");
    //    var districtId = $scope.DistrictId == undefined ? "0" : $scope.DistrictId;

    //    if (districtId == 0) {

    //        districtId = $scope.BUDSStatusReport.DistrictId == undefined ? "0" : $scope.BUDSStatusReport.DistrictId;
    //        // districtId = $scope.BUDSStatusReport.DistrictId;
    //    }
    //    //BUDSStatusReport.DistrictId
    //    $http.post(SiteUrl + "/MIS/GetCCSData", { ActionId: $scope.IsMultiState == 1 ? 2 : $scope.IsMultiState == 2 ? 1 : 0, DistrictId: (districtId == 0 ? $scope.DISTRICT_ID : districtId) }).
    //        success(function (data, status) {

    //            // $scope.CCSList = (data.DataList);
    //            $scope.CCSList = (data);
    //            $(".se-pre-con").fadeOut("slow");

    //        }).
    //        error(function (data, status) {
    //            alert("Error while fetching list");
    //            $scope.CCSList = [];
    //            $(".se-pre-con").fadeOut("slow");
    //        });
    //};
    // $scope.re = false;    

    //$scope.GetCCSListNew();

    //Get Compliant Report With District Id
    $scope.GetCompliantReportWithDistrictIdList = [];
    $scope.GetCompliantReportWithDistrictId = function () {
        alert($scope.Districts.DistrictId);

        $http.post(SiteUrl + "/MIS/GetActionIdCCSComplaintReport?ActionId=0" + "&DistrictId=" + $scope.Districts.DistrictId).
            success(function (data, status) {
                $scope.GetActionIdCCSComplaintReport = data.Table;
                //  $scope.GetCCSCompalainteReportList($scope.Districts.DistrictId);
                //$scope.tblgetsuport.show();
                //$scope.DivShowDetail = true;
            }).
            error(function (data, status) {
                $scope.GetCCSCompalainteReportData = [];
            });

    };

    $scope.OnPageLoad();

});