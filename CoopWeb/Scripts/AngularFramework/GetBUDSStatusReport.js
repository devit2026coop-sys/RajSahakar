var CCSComplaintReportModule = angular.module('GetBUDSStatusReportMod', ['angularUtils.directives.dirPagination']);
CCSComplaintReportModule.controller("GetBUDSStatusReportCnt", function ($rootScope, $scope, $http) {

    /*------START------Variables-------------------------*/

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.divStateData = true;
    $scope.SearchType = 1;
    $scope.GetActionIdCCSComplaintReport = [];
    $scope.DistrictId = "0";
    $scope.BUDSStatusReport = [];
    $scope.BUDSStatusReport.CCSId1 = '0';
    $scope.BUDSStatusReport.UserRoleName = '';

    $scope.ReportListType = "0";
    //$scope.Filter.CCSId = '0';
    $scope.IsnotShow = false;
    $scope.selectedStatus = "A";
    $scope.BUDSStatusReport = { DistrictId: 0, FromDate: null, ToDate: null };

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
        $scope.qery = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.FilterType = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.searchRequest($scope.GetCCSCompalainteReportList);
    };

    /*------ENDING------Paging and Filter-----------------*/


    //------START----on page load-----------------//

    //Get District List//
    $scope.GetDistricts = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Districts = [];
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.Districts = data;

                $scope.BUDSStatusReport.DistrictId = $scope.BUDSStatusReport.DistrictId;
            }).
            error(function (data, status) {
                $scope.Districts = '';
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

        var roleid = $('#hdnRoleId').val();
        console.log("roleid", roleid);


        $scope.GetDistricts().then(function () {
            var districtId = $('#hdnLoggedInUserDistrictId').val();
            var role = $('#hdnLoggedInUserRole').val();
            var RoleId = $('#hdnRoleId').val();

            if (districtId != "0") {
                $scope.BUDSStatusReport.DistrictId = districtId;
            }

            if (RoleId == "1") {
                $scope.BUDSStatusReport.DistrictId = "0";
            }

            if ((districtId != "0" && role && role.trim() === "Deputy Registrar") || (districtId = "0" && !role && role.trim() === "")) {
                $scope.IsControlDisable = true;
            } else {
                $scope.IsControlDisable = false;
            }

            $scope.GetBUDSStatusReport();
        });


        var role = $('#hdnLoggedInUserRole').val();
        $scope.BUDSStatusReport.UserRoleName = role;

        // Disable if role is empty
        if (!role || role.trim() === "ADMINISTRATOR") {
            $scope.IsControlDisable = true;
        } else {
            $scope.IsControlDisable = false;
        }


        $(".se-pre-con").fadeOut("slow");
    };
    //------ENDING---on page load-----------------//    

    $scope.DatabyDistricts = [];

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
        $scope.DisposeRequestComplainer_Count = 0;
    };

    $scope.GetBUDSStatusReport = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.DatabyDistricts = [];
        $scope.BUDSStatusReport.FromDate = $scope.dateFormat($scope.BUDSStatusReport.FromDate);
        $scope.BUDSStatusReport.ToDate = $scope.dateFormat($scope.BUDSStatusReport.ToDate);
        return $http.get(SiteUrl + "/MIS/GetBUDSStatusReport" + "?DistrictId=" + ($scope.BUDSStatusReport.DistrictId ?? 0) + "&FromDate=" + $scope.BUDSStatusReport.FromDate + "&ToDate=" + $scope.BUDSStatusReport.ToDate).success(function (data, status) {
            $scope.clean();
            if (!data || data.length === 0) {
                $scope.DatabyDistricts = [];
                $(".se-pre-con").fadeOut("slow");
                return;
            }

            $scope.DatabyDistricts = data;
            angular.forEach($scope.DatabyDistricts, function (item) {
                $scope.RajSahakarPortal_Complaints_Count += Number(item.RajSahakarPortal_Complaints_Count || 0);
                $scope.Other_Complaints_Count += Number(item.Other_Complaints_Count || 0);
                $scope.Total_Complaints_Count += Number(item.RajSahakarPortal_Complaints_Count || 0) + Number(item.Other_Complaints_Count || 0);

                $scope.Documents_Received_Count += Number(item.Documents_Received_Count || 0) + Number(item.TotalUploadedDocument_ON || 0);
                $scope.Notices_Issued_Count += Number(item.Notices_Issued_Count || 0) + Number(item.NoticeIsssue_ON || 0);
                $scope.Hearing_Documents_Count += Number(item.Hearing_Documents_Count || 0);

                $scope.Pp_Documents_Shared += Number(item.Pp_Documents_Shared || 0) + Number(item.InstgasaPreparedandmadeavailabletoPP_ON || 0);
                $scope.Court_Filed_Cases += Number(item.Court_Filed_Cases || 0) + Number(item.IstgasaSubmittedtoCourt_ON || 0);
                $scope.Buds_Act_FIR_Cases += Number(item.Buds_Act_FIR_Cases || 0);
                $scope.FIR_Registered_Count += Number(item.FIR_Registered_Count || 0) + Number(item.FIRRegistered_ON || 0);
                $scope.DisposeRequestComplainer_Count += Number(item.DisposeRequestComplainer_ON || 0);
            });
            $scope.IsDistrictwiseNot = false;
            $scope.IsDistrictwise = true;
            $(".se-pre-con").fadeOut("slow");
        }).error(function () {
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
    }

    $scope.CCSList = [];
    $scope.GetCCSListNew = function () {
        $(".se-pre-con").fadeIn("slow");
        var districtId = $scope.DistrictId == undefined ? "0" : $scope.DistrictId;
        if (districtId == 0) {

            districtId = $scope.BUDSStatusReport.DistrictId == undefined ? "0" : $scope.BUDSStatusReport.DistrictId;
        }
        $http.post(SiteUrl + "/MIS/GetCCSData", { ActionId: $scope.IsMultiState == 1 ? 2 : $scope.IsMultiState == 2 ? 1 : 0, DistrictId: (districtId == 0 ? $scope.DISTRICT_ID : districtId) }).
            success(function (data, status) {
                $scope.CCSList = (data);
                $(".se-pre-con").fadeOut("slow");
                console.log(data);

            }).
            error(function (data, status) {
                alert("Error while fetching list");
                $scope.CCSList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.GetCCSListNew();

    $scope.GetCompliantReportWithDistrictIdList = [];
    $scope.GetCompliantReportWithDistrictId = function () {
        alert($scope.Districts.DistrictId);

        $http.post(SiteUrl + "/MIS/GetActionIdCCSComplaintReport?ActionId=0" + "&DistrictId=" + $scope.Districts.DistrictId).
            success(function (data, status) {
                $scope.GetActionIdCCSComplaintReport = data.Table;
            }).
            error(function (data, status) {
                $scope.GetCCSCompalainteReportData = [];
            });

    };

    $scope.dateFormat = function (date) {
        if (!date) return '';
        var d = new Date(date);
        var day = String(d.getDate()).padStart(2, '0');
        var month = String(d.getMonth() + 1).padStart(2, '0');
        var year = d.getFullYear();
        return day + '-' + month + '-' + year;
    }


    $scope.ClearBUDSStatusReport = function () {
        $scope.BUDSStatusReport.DistrictId = "0";
        $scope.BUDSStatusReport.FromDate = "0";
        $scope.BUDSStatusReport.ToDate = "0";
        $scope.GetBUDSStatusReport();
    };
    $scope.OnPageLoad();

});