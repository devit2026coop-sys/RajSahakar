var SocietyRegistrationModule = angular.module("UploadAuditByAuditorMod", ['angularUtils.directives.dirPagination']);
SocietyRegistrationModule.controller("UploadAuditByAuditorCont", function ($rootScope, $scope, $http, $location) {

    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 25,
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
                //callback();
                $scope.GetSocietyListAssignedToAuditor($scope.SocietyCountId_ss, $scope.DistrictId_ss, false)
            }
            ++indx;
        });


        //if (($scope.SelectPage.filter(x => x === $scope.DataPaging.currentPageID).length > 0)) {
        //    $("#chk_all").prop("checked", true);
        //} else {
        //    $("#chk_all").prop("checked", false);
        //}
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
    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.DivShowCount = true;
    $scope.DivShowDetail = false;
    $scope.SearchText = "";
    $scope.obj = {};
    $scope.SocietyCombine = [];
    $scope.Filter = [];

    var params = new URLSearchParams(window.location.search);
    $scope.Filter.FinancialYear = params.get("FinYear") || "";

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
    }
    /*------------End Paging and Filter-----------------*/



    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.obj.pageSize = '25';
    $scope.DIVISION_ID = '0';


    $scope.Reset = function () {
        $scope.DIVISION_ID = "0";
        $scope.Filter.DistrictId = '';
        //  $("#hdnDistrictId").val('0');
        $scope.GetSocietyData();
    }


    $scope.OnPageLoad = function () {
        //$(".se-pre-con").fadeIn("slow");    
        $scope._language = _language; // Set LAnguiage
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
            //$(".se-pre-con").fadeOut("slow");
        }
        $('#txtCreatetionDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $scope.Divisions = [{}];
        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
        //    success(function (data, status) {
        //        $scope.Divisions = data;
        //    }).
        //    error(function (data, status) {
        //        $scope.Divisions = [];
        //    });
        $http.post(SiteUrl + "/MIS/GetUserLevelId").
            success(function (data, status) {
                if (data.length > 0 && data != 0) {
                    if (data == 1)
                        $scope.IsState = true;
                }
            }).
            error(function (data, status) {
            });
        $scope.GetDistrictList();
        $scope.GetSocietyData();
        $scope.GetSocietyTypeList();
        $scope.GetMonthList();
    };

    $scope.SocietyView = function (SocietyIdEnc) {
        window.location = ' /Society/SocietyApplicationView?SocietyIDEnc=' + SocietyIdEnc + '~_MIS_SocietyRegisterDistWise~0';
    }

    var isPageLoad = true;
    var isBackFy = false;
    $scope.FinancialYearList = [];

    $scope.GetSocietyData = function () {
        $scope.SocietyList = [];
        $scope.DivShowCount = true;
        $scope.DivShowDetail = false;
        $scope.AssignedAuditor = [];

        if ($("#DivisionId").val() != "0") {
            $scope.Filter.Division_Id = $("#DivisionId").val()
            $("#DivisionId").attr("disabled", "disabled");
        }


        if ($("#hdnDistrictId").val() != "0") {
            $scope.Filter.DistrictId = $("#hdnDistrictId").val()
            $("#ddrDistrictId").attr("disabled", "disabled");
        }
        $(".se-pre-con").fadeIn("slow");
        $http.get("../MIS/GetCountSocietyAssignedToAuditor_v1?FIN_YEAR_NAME=" + ($scope.Filter.FinancialYear ?? '') + "&Division_Id=" + ($scope.Filter.Division_Id ?? '') + "&DistrictId=" + ($scope.Filter.DistrictId ?? '') + "&Month=" + ($scope.Filter.Month ?? '') + "&IsEmpanelledCA=" + ($scope.Filter.IsEmpanelledCA ?? '')
        ).success(function (data, status) {
            $scope.obj.pageSize = $scope.DataPaging.pageSize = "25";
            $scope.obj.FilterText = "";
            $scope.obj.auditBy = "";
            $scope.obj.societyTypeId = "0";
            $scope.SocietyCombine = [];
            if (data.length === 0) {
                var msg = _language == 'en-us' ? 'No data found.' : 'कोई डेटा नहीं मिला।';
                ShowModelAlert('', msg);
                $(".se-pre-con").fadeOut("slow");
                return;
            }
            else {
                $scope.SocietyCombine = JSON.parse(data.DataList);
                $(".se-pre-con").fadeOut("slow");
            }
        }).
            error(function (data, status) {
                alert('Error while fetching society district wise data.');
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.GetSocietyListAssignedToAuditor = function (SocietyCountId, DistrictId, isExport) {

        if (!isExport)
            $rootScope.DataPagingDetails = {};

        $scope.SocietyCountId_ss = SocietyCountId;
        $scope.DistrictId_ss = DistrictId;
        $(".blockUI").fadeIn("slow");
        if (isPageLoad && isBackFy) {
            var parameter = { Key: 'FIN_YEAR_NAME', Value: $scope.Filter.FinancialYear }
            $scope.DataPaging.searchParameters.push(parameter);
        }
        if ($("#DivisionId").val() == "0" && $("#DivisionId").val() == "" && $("#DivisionId").val() == undefined) {
            var parameter = { Key: 'A.Division_Id', Value: "" }
            $scope.DataPaging.searchParameters.push(parameter);
        }
        isPageLoad = true;
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/MIS/GetSocietyListAssignedToAuditor", JSON.stringify({ Details: jsonParameters, Id: $scope.SocietyCountId_ss, FIN_YEAR_NAME: $scope.Filter.FinancialYear, DistrictId: $scope.DistrictId_ss, isExport: isExport, Month: ($scope.Filter.Month ?? 0), IsEmpanelledCA: ($scope.Filter.IsEmpanelledCA ?? ''), AuditBy: ($scope.obj.auditBy ?? ''), SocietyTypeId: ($scope.obj.societyTypeId ?? 0) })).
            success(function (data, status) {
                if (isExport) {
                    $scope.downloadFile(data.DataList);
                }
                else {
                    $scope.AssignedAuditor = [];
                    var AssignedAuditorList = JSON.parse(data.DataList);
                    $rootScope.DataPagingDetails = data.DataPaging;
                    if (AssignedAuditorList?.length > 0) {
                        if ($scope.DataPaging.pageSize != "" && $scope.DataPaging.pageSize != null) {
                            $rootScope.DataPagingDetails.TotalRecords = AssignedAuditorList[0].TotalRecords;
                        }
                        else {
                            $rootScope.DataPagingDetails.TotalRecords = 0;
                            $scope.DataPaging.TotalRecords = 0;
                            $scope.DataPaging.currentPageID = 1;
                        }
                        $scope.DivShowCount = false;
                        $scope.DivShowDetail = true;
                    }
                    $rootScope.SetDataPaging();
                    $scope.AssignedAuditor = AssignedAuditorList;

                    // console.log($scope.AssignedAuditor);
                }
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.AssignedAuditor = [];
                $(".blockUI").fadeOut("slow");
                alert('Error while fetching society list data.');
                $scope.AssignedAuditor = [];
            });
    };

    $scope.ViewDocuments = function (Id) {
        $(".se-pre-con").fadeIn("slow");
        $("#myModal").modal("show");
        $http.get(SiteUrl + "/Auditor/GetAuditUplodedDocument?Id=" + Id).
            success(function (data, status) {
                $scope.DocumentList = data;
            }).
            error(function (data, status) {
                $scope.DocumentList = [];
                alert('Error while Document fetch.');
            });
        $(".se-pre-con").fadeOut("slow");
    }

    $scope.ViewSocietyList = function (SocietyCountId, DistrictId) {
        $scope.AssignedAuditor = [];
        $scope.DataPaging.TotalRecords = 0;
        $scope.DataPaging.currentPageID = 1;
        $scope.CountSocietyAuditorList = true;
        $scope.GetSocietyListAssignedToAuditor(SocietyCountId, DistrictId, false);
    }

    $scope.DetailsReset = function () {
        //  $("#txtFromDate").val("");
        //   $("#txtToDate").val("");
        $("#search").val("");
        $scope.obj.auditBy = "";
        $scope.obj.societyTypeId = "0";
        $scope.obj.FilterText = "";
        $scope.DataPaging.searchParameters = [];
        // $scope.ShowSearchDetail();
        $scope.GetSocietyListAssignedToAuditor($scope.SocietyCountId_ss, $scope.DistrictId_ss, false);
    }

    $scope.GetDetail = function (DistId, StatusId) {
        $scope.DataPaging = {
            searchBoxDivId: 'search-filter',
            currentPageID: 1,
            pageSize: 25,
            sortExpression: '',
            sortDirection: '1',
            searchParameters: []
        }
        $("#txtFromDate").val("");
        $("#txtToDate").val("");
        $scope.ShowDetail(DistId, StatusId)
    }


    $scope.ShowSearchDetail = function () {
        $scope.ShowDetail($scope.CurrentDistId, $scope.CurrentStatusId)
    }

    $scope.GetDistrictList = function () {
        if ($("#DivisionId").val() != "0") {
            $scope.Filter.Division_Id = $("#DivisionId").val()
            $("#DivisionId").attr("disabled", "disabled");
        }
        //   $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $("#DivisionId").val()).
        $http.get(SiteUrl + "/Master/GeAuditDistrictID/?DivisionId=" + ($scope.Filter.Division_Id ?? '')).

            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0") {
                    $scope.Filter.DistrictId = $("#hdnDistrictId").val()
                    $("#ddrDistrictId").attr("disabled", "disabled");
                }
                if ($("#hdnDistrictId").val() != "0" && $("#hdnDistrictId").val() != "" && $("#hdnDistrictId").val() != undefined) {
                    //data.forEach(function (item, index) {
                    //    if ($("#hdnDistrictId").val().split(',').indexOf(item.DIVISION_ID) > -1) {
                    //        $scope.Divisions.push(item);
                    //    }
                    //});
                    if ($("#hdnDistrictId").val().split(',').length == 1) {
                        //$scope.DIVISION_ID = $("#hdnDistrictId").val();
                        $("#ddrDistrictId").attr("disabled", "disabled");
                    }
                }
                $scope.BindFY();

            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };

    $scope.BindFYOld = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;

                isBackFy = getUrlVars()["year"] == '' || getUrlVars()["year"] == undefined ? false : true;
                if (isBackFy) {
                    $scope.Filter.FinancialYear = getUrlVars()["year"];
                }
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    }
    $scope.BindFY = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").success(function (data, status) {
            $scope.FinancialYearList = data;
            if ($scope.FinancialYearList && $scope.FinancialYearList.length > 0) {
                if ($scope.Filter.FinancialYear !== null && $scope.Filter.FinancialYear !== "") {
                    //var FilterData = $scope.FinancialYearList.filter(Number(x => x.FIN_YEAR_MASTR_ID) == Number($scope.Filter.FinancialYear[0].FIN_YEAR_MASTR_ID));
                    var FilterData = $scope.FinancialYearList.filter(function (x) {
                        return Number(x.FIN_YEAR_MASTR_ID) === Number($scope.Filter.FinancialYear);
                    });
                    $scope.Filter.FinancialYear = FilterData[0].FIN_YEAR_MASTR_ID.toString();
                }
                else { $scope.Filter.FinancialYear = ""; }
            }

            isBackFy = getUrlVars()["year"] != '' && getUrlVars()["year"] != undefined;
            if (isBackFy) {
                $scope.Filter.FinancialYear = getUrlVars()["year"].toString();
            }
        }).error(function (data, status) {
            $scope.FinancialYearList = [];
        });
    }

    $scope.ShowDetail = function (DistId, StatusId) {
        $scope.CurrentDistId = DistId;
        $scope.CurrentStatusId = StatusId;

        $scope.SocietyList = [];
        $scope.q = "";
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": 1, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/MIS/GetSocietyReportData", JSON.stringify({ Details: jsonParameters, "DistrictId": DistId, "StatusId": StatusId, "ActId": $scope.ActId, "SocietyTypeId": $scope.SocietyTypeId, "IsOnline": $scope.IsOnline })).
            success(function (data, status) {
                $scope.DivShowCount = false;
                $scope.DivShowDetail = true;
                $(".se-pre-con").fadeOut("slow");
                if (JSON.parse(data.DataList).length === 0) {
                    var msg = _language == 'en-us' ? 'No data found.' : 'कोई डेटा नहीं मिला।';
                    ShowModelAlert('', msg);
                    return;
                }
                else {
                    $scope.SocietyList = JSON.parse(data.DataList);
                    $rootScope.DataPagingDetails = data.DataPaging;
                    $rootScope.SetDataPaging();
                }

            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                alert('Error while fetching society data.');
                $(".se-pre-con").fadeOut("slow");
            });
        $scope.DivShowDetail = true;
    };

    $scope.BackDetails = function () {
        $("#search").val("");
        $scope.obj.auditBy = "";
        $scope.obj.societyTypeId = "0";
        $scope.DataPaging.searchParameters = [];
        $scope.DivShowCount = true;
        $scope.DivShowDetail = false;

    }

    $scope.exportExcel = function () {
        $scope.GetSocietyListAssignedToAuditor($scope.SocietyCountId_ss, $scope.DistrictId_ss, true);
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


    $scope.GetMonthList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MONTH").
            success(function (data, status) {
                $scope.MonthList = data;
            }).
            error(function (data, status) {
                $scope.MonthList = [];
            });
    };

    $scope.GetSocietyTypeList = function () {
        $scope.Typelist = [];
        $scope.obj.societyTypeId = '0';

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=" + 3)
            .then(function (res) {
                $scope.Typelist = res.data;
            });
    };


    $scope.OnPageLoad();
    //END : Call Functions on Page Load
});
