
var AssignAuditorModule = angular.module('AssignAuditorMod', ['angularUtils.directives.dirPagination']);
AssignAuditorModule.controller("AssignAuditorCont", function ($rootScope, $scope, $http) {

    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 20,
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
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
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
    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];
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
    $scope.ClearSearch = function () {
        $scope.Filter.SearchText = "";

        if ($("#hdnActId").val().split(',').length > 1) {
            $scope.ActId = "0";
        }

        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        //$scope.ShowOldYearDataDetail();

        $scope.searchRequest($scope.ShowOldYearDataDetail)

    }
    /*------------End Paging and Filter-----------------*/


    $scope.Actlist = [];
    $scope.GetSocietyActList = function () {
        $http.get(SiteUrl + "/Society/GetSocietyActList").
            success(function (data, status) {
                data.forEach(function (item, index) {
                    if ($("#hdnActId").val().split(',').indexOf(item.ActId) > -1) {
                        $scope.Actlist.push(item);
                    }
                });

                if ($("#hdnActId").val().split(',').length == 1) {
                    $scope.ActId = $("#hdnActId").val();
                    $("#ddrActId").attr("disabled", "disabled");
                }

                //$scope.Actlist = data;
            }).
            error(function (data, status) {
                $scope.Actlist = [];
            });
    };



    // SCOPE
    //$scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.divAuditType = false;
    $scope.divDistrict = false;
    $scope.divAuditor = false;
    $scope.divBtn = false;
    $scope.divAuditorData = true;
    $scope.currentPage = 1;
    $scope.ShowCurrentYearData = false;
    $scope.ShowOldYearData = false;
    $scope.FileCollection = [];
    $scope.AuditorAssign = [];
    $scope.ExtraFileCounter = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

    // Code for Language label setting
    $scope.getTranslation = function () {
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
        }
        //GET Financial Year  

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                //alert(JSON.stringify(data));
                $scope.FinancialYearList = data;//.filter(function (x) { return x.FIN_YEAR_END != data[0].FIN_YEAR_END });
                $scope.GetSocietyActList();
                $('#FeeDateFirm').datepicker({
                    showOn: "both",
                    changeMonth: true,
                    buttonImage: "../images/calender.png",
                    changeYear: true,
                    dateFormat: "dd/mm/yy",
                    maxDate: '0D'
                });
                $('#AuditStartDate').datepicker({
                    showOn: "both",
                    changeMonth: true,
                    buttonImage: "../images/calender.png",
                    changeYear: true,
                    dateFormat: "dd/mm/yy",
                    maxDate: '0D'
                });
                $('#AuditEndDate').datepicker({
                    showOn: "both",
                    changeMonth: true,
                    buttonImage: "../images/calender.png",
                    changeYear: true,
                    dateFormat: "dd/mm/yy",
                    maxDate: '0D'
                });
                $('#FeeDateFirmCurrentYear').datepicker({
                    showOn: "both",
                    changeMonth: true,
                    buttonImage: "../images/calender.png",
                    changeYear: true,
                    dateFormat: "dd/mm/yy",
                    maxDate: '0D'
                });
                $('#FeeDate').datepicker({
                    showOn: "both",
                    changeMonth: true,
                    buttonImage: "../images/calender.png",
                    changeYear: true,
                    dateFormat: "dd/mm/yy",
                    maxDate: '0D'
                });
                $('#FeeDateCurrentDepart').datepicker({
                    showOn: "both",
                    changeMonth: true,
                    buttonImage: "../images/calender.png",
                    changeYear: true,
                    dateFormat: "dd/mm/yy",
                    maxDate: '0D'
                });


                $('#ResolutionDate').datepicker({
                    showOn: "both",
                    changeMonth: true,
                    buttonImage: "../images/calender.png",
                    changeYear: true,
                    dateFormat: "dd/mm/yy",
                    maxDate: '31/05/2020',
                    minDate: '01/04/2018'
                });

                $('#AudiAppoOrderDate').datepicker({
                    showOn: "both",
                    changeMonth: true,
                    buttonImage: "../images/calender.png",
                    changeYear: true,
                    maxDate: '31/05/2020',
                    minDate: '01/04/2019'
                });

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };





    $scope.ResetOnSocietyChange = function () {
        $scope.AuditorAssign.FinancialYear = '0'
        $scope.ShowCurrentYearData = false;
        $scope.CurrentYearAuditorAppointed = false;
        $scope.CurrentYearAuditorAppointedHaveAuditReport = false;
        $scope.ShowOldYearData = false;
        $scope.ShowEmpanelledCADataOld = false;
        $scope.ShowDepartmentDataOld = false;
        $scope.divAuditType = false;
        $scope.divDistrict = false;
        $scope.divAuditor = false;
        $scope.divBtn = false;
    };
    $scope.AuditorAssignOldData = [];

    $scope.CurrentYearAssignAuditor = function (Flag) {
        //if (Flag == false) {
        //    alert("You can't request auditor appointment.(Last date has been gone)");
        //    return false;
        //}
        $scope.CurrentYearAuditorAppointed = true;
    }

    $scope.HaveAuditReportSelection = function (Flag) {
        $scope.CurrentYearAuditorAppointedHaveAuditReport = Flag;
    }

    $scope.ShowOldYearDataDetail2 = function (FYearId) {

        $(".se-pre-con").fadeIn("slow");
        if ($scope.AuditorAssign.SocietyIdENC == 0) {
            alert("Please Selact Society.");
            $scope.AuditorAssign.FinancialYear = '0';
            return;
        }
        $scope.AuditorAppointedDateSelection = undefined;
        $scope.AuditorAssign.IsEmpanelledCA = undefined;
        $scope.ShowEmpanelledCADataOld = false;
        $scope.ShowDepartmentDataOld = false;
        if (FYearId > 47) {
            $scope.ShowCurrentYearData = true;
            $scope.ShowOldYearData = false;
        }
        else {
            $scope.ShowCurrentYearData = false;
            $scope.ShowOldYearData = true;
        }
        $http.post(SiteUrl + "/Auditor/GetSocietyAuditDataByFinYearId", { "SocietyIdENC": $scope.AuditorAssign.SocietyIdENC, "FinYearId": FYearId }).
            success(function (data, status) {
                if (data.Table.length > 0) {
                    $scope.AuditorAssignOldData = data.Table[0];
                    $scope.AuditorAssign = data.Table[0];
                    $scope.AuditorAssign.FinancialYear = FYearId;
                    $scope.AuditorAssign.HaveAuditReport = $scope.AuditorAssign.HaveAuditReport != null ? $scope.AuditorAssign.HaveAuditReport.toString() : false;
                    if ($scope.AuditorAssign.IsEmpanelledCA) {
                        $scope.AuditorAssign.IsEmpanelledCA = $scope.AuditorAssign.IsEmpanelledCA.toString();
                        $scope.divAuditType = true;
                        $scope.divDistrict = true;
                        $scope.divAuditor = true;
                        $scope.divBtn = true;
                        $scope.ShowDepartmentDataOld = false;
                        $scope.ShowEmpanelledCADataOld = true;
                        $scope.CurrentYearAuditorAppointedByDepart = false;
                    }
                    else {
                        $scope.AuditorAssign.IsEmpanelledCA = $scope.AuditorAssign.IsEmpanelledCA.toString();
                        $scope.divAuditType = false;
                        $scope.divDistrict = true;
                        $scope.divAuditor = false;
                        $scope.divBtn = true;
                        $scope.ShowDepartmentDataOld = true;
                        $scope.ShowEmpanelledCADataOld = false;
                        $scope.CurrentYearAuditorAppointedByDepart = true;
                        // $scope.AuditorAssign.ApprovedBy = $scope.AuditorAssign.ApprovedBy;
                    }
                    if ($scope.AuditorAssign.HaveAuditReport) {
                        $scope.CurrentYearAuditorAppointedHaveAuditReport = true;
                    }
                    else {
                        $scope.CurrentYearAuditorAppointedHaveAuditReport = false;
                    }
                    $scope.GetAuditorDataSetData();
                    $scope.AuditorAssign.DistrictId = $scope.AuditorAssignOldData.DistrictId.toString();
                    $scope.AuditorAssign.AuditorId = $scope.AuditorAssignOldData.AuditorId.toString();
                    $scope.AuditorAssign.LastFinancialYear = $scope.AuditorAssign.LastFinancialYear.toString();
                    if ($scope.AuditorAssignOldData.FeeDateForShow != null)
                        $scope.AuditorAssign.FeeDate = $scope.AuditorAssignOldData.FeeDateForShow.toString();
                    if ($scope.AuditorAssignOldData.AuditStartDateShow != null)
                        $scope.AuditorAssign.AuditStartDate = $scope.AuditorAssignOldData.AuditStartDateShow.toString();
                    if ($scope.AuditorAssignOldData.AuditEndDateShow != null)
                        $scope.AuditorAssign.AuditEndDate = $scope.AuditorAssignOldData.AuditEndDateShow.toString();
                    if ($scope.AuditorAssignOldData.ResolutionDateShow != null) {
                        $scope.AuditorAssign.ResolutionDate = $scope.AuditorAssignOldData.ResolutionDateShow.toString();
                        $scope.CurrentYearAuditorAppointed = true;
                        $scope.AuditorAppointedDateSelection = 'true';
                        $scope.ShowEmpanelledCADataOld = false;
                    }
                    else {
                        $scope.CurrentYearAuditorAppointed = false;
                        $scope.AuditorAppointedDateSelection = 'false';
                    }
                    if ($scope.AuditorAssignOldData.AudiAppoOrderDateShow != null)
                        $scope.AuditorAssign.AudiAppoOrderDate = $scope.AuditorAssignOldData.AudiAppoOrderDateShow.toString();

                    $scope.FileUploadTypes = [];
                    $scope.FileUploadTypes = data.Table1;

                    $scope.recordsOther = [];
                    $scope.recordsOther = data.Table2;
                    if (data.Table2.length > 0) {
                        $scope.OtherFileUpload.AddMoreFile = data.Table2.length.toString();
                        $scope.GetAddMoreFilesDivOldData();
                    }
                    else {
                        $scope.OtherFileUpload.AddMoreFile = '1';
                        $scope.GetAddMoreFilesDiv();
                    }
                    //alert(JSON.stringify($scope.records));

                }
                else {
                    var SociId = $scope.AuditorAssign.SocietyIdENC;
                    $scope.AuditorAssign = [];
                    $scope.AuditorAssign.SocietyIdENC = SociId
                    $scope.AuditorAssign.FinancialYear = FYearId;
                    $scope.OtherFileUpload.AddMoreFile = '1';
                    $scope.GetAddMoreFilesDiv();
                    $scope.FileUploadTypeList();
                    $scope.CurrentYearAuditorAppointed = false;
                    $scope.CurrentYearAuditorAppointedHaveAuditReport = false;
                    $scope.ShowEmpanelledCADataOld = false;
                    $scope.ShowDepartmentDataOld = false;
                    $scope.divAuditType = false;
                    $scope.divDistrict = false;
                    $scope.divAuditor = false;
                    $scope.divBtn = false;
                }
                // $scope.DeshboardLeft = data.Table1;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                $(".se-pre-con").fadeOut("slow");
                alert('Error while fetching society list data.');
            });
    };




    $scope.ShowOldYearDataDetail = function () {
        //$scope.TableData = [];
        $(".se-pre-con").fadeIn("slow");

        var parameter = { Key: "ActId", Value: $scope.ActId == "0" ? $("#hdnActId").val() : $scope.ActId }
        $scope.DataPaging.searchParameters.push(parameter);

        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/Auditor/GetSocietyAuditDataByRoleFinYearId", JSON.stringify({ Details: jsonParameters, FinYearId: $scope.AuditorAssign.FinancialYear })).
            success(function (data, status) {
                //alert(JSON.stringify(data));
                //$scope.TableData = data;
                //if (data.Table.length > 0) {
                //    $scope.TableData = data.Table;
                //    //alert(JSON.stringify($scope.records));

                //}
                // $scope.OrderList = [];
                $scope.TableData = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");

            }).
            error(function (data, status) {
                $scope.TableData = [];
                $(".se-pre-con").fadeOut("slow");
                alert('Error while fetching society list data.');
            });
    };


    $scope.ShowDetails = function (SocietyIdENC, SocietyName) {
        if ($scope.AuditorAssign.FinancialYear != "0") {
            //$scope.SocietyIdENC = SocietyIdENC;
            $scope.SocietyName = SocietyName;
            $scope.FincialYear = $("#DDLFinYear option:selected").text();
            $scope.AuditorAssign.SocietyIdENC = SocietyIdENC;
            //alert($scope.SocietyName + "----" + $scope.FincialYear + "----" + $scope.AuditorAssign.FinancialYear + "----" + $scope.AuditorAssign.SocietyIdENC);
            $scope.ShowOldYearDataDetail2($scope.AuditorAssign.FinancialYear);
            // $scope.ShowCurrentYearData = true;
        }
        else {
            alert("Please Choose Finacial year !");
            return;
        }
    };

    $scope.BackForm = function () {
        window.location.reload();
    }
    $scope.GetAddMoreFilesDivOldData = function () {

        $scope.divMoreFilesAdd = true;
        $scope.records = [];
        var X = $scope.OtherFileUpload.AddMoreFile;
        records = [];
        var _Counter = $scope.MaxCountAddMoreFile = Math.max.apply(Math, $scope.FileUploadTypes.map(function (o) { return o.Id; }));
        for (var x = 1; x <= X; x++) {
            records.push(
                {
                    "Id": _Counter + x,
                    "DDLID": "DDLDocTypeID" + (_Counter + x),
                    "UploadFileNamePDF": $scope.recordsOther[x - 1].UploadFileNamePDF,
                    "UploadFileNameXLS": $scope.recordsOther[x - 1].UploadFileNameXLS,
                    "Name": $scope.recordsOther[x - 1].Name
                });
        }
        $scope.records = records;
    }

    $scope.ShowByEmpanelledAgencyDataOld = function (IsCA) {
        if (IsCA == true) {
            $scope.ShowDepartmentDataOld = false;
            $scope.ShowEmpanelledCADataOld = true;
        }
        else {
            $scope.ShowDepartmentDataOld = true;
            $scope.ShowEmpanelledCADataOld = false;
        }
    };

    // START :Get List of Societies
    $scope.GetSocietyList = function () {
        $http.post(SiteUrl + "/Citizen/GetSocietyListNotAssignedToInspector", { "WhereClause": " " }).
            success(function (data, status) {

                $scope.SocietyList = data;
            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                // alert('Error while fetching society list data.');
            });
    };
    // END :Get List of Societies

    // START :Get List of Districts
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=-1").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };
    // END :Get List of Districts

    // START : Get Auditor Data
    $scope.GetAuditorData = function () {
        var IsIndividual = $scope.AuditorAssign.IsFirm == 1 ? true : false;
        //var DistrictId = $scope.AuditorAssign.DistrictId;

        $scope.AuditorAssign.DistrictId = '0';
        var DistrictId = $scope.AuditorAssign.DistrictId;
        $scope.AuditorList = [];
        $scope.AuditorAssign.AuditorId = '0';

        $scope.GetAuditorDataSetData();
    }
    $scope.GetAuditorDataSetData = function () {
        var IsIndividual = $scope.AuditorAssign.IsFirm == 1 ? true : false;
        var DistrictId = 0;
        $http.post(SiteUrl + "/Society/GetAuditData/", { IsAuditFirm: IsIndividual, "DistrictId": DistrictId }).
            success(function (data1, status) {
                $scope.AuditorList = [];
                $scope.AuditorList = data1;
            }).
            error(function (data1, status) {

                $scope.AuditorList = [];
                alert('Error while fetching Auditor data.');
            });
    }
    // END : Get Auditor Data

    // START : Get Auditor Data
    $scope.GetAuditorDataByDist = function () {
        var IsIndividual = $scope.AuditorAssign.IsFirm == 1 ? true : false;
        var DistrictId = $scope.AuditorAssign.DistrictId;

        //$scope.AuditorAssign.DistrictId = '0';
        $scope.AuditorList = [];
        $scope.AuditorAssign.AuditorId = '0';



        $http.post(SiteUrl + "/Society/GetAuditData/", { IsAuditFirm: IsIndividual, "DistrictId": DistrictId }).
            success(function (data, status) {
                $scope.AuditorList = [];
                $scope.AuditorList = data;


            }).
            error(function (data, status) {

                $scope.AuditorList = [];
                alert('Error while fetching Auditor data.');
            });
    }
    // END : Get Auditor Data

    // START : Get Auditor Data
    $scope.SetAuditType = function (IsByEmpanelled) {
        $scope.AuditorList = [];
        $scope.AuditorAssign.IsFirm = undefined;
        $scope.AuditorAssign.DistrictId = '0';
        $scope.AuditorAssign.AuditorId = '0';
        if (IsByEmpanelled) {
            $scope.divAuditType = true;
            $scope.divDistrict = true;
            $scope.divAuditor = true;
            $scope.CurrentYearAuditorAppointedByDepart = false;
            $scope.divBtn = true;
        }
        else {
            $scope.divAuditType = false;
            $scope.divDistrict = true;
            $scope.divAuditor = false;
            $scope.CurrentYearAuditorAppointedByDepart = true;
            $scope.divBtn = true;
        }
    }
    // END : Get Auditor Data

    // START : Clear Data
    $scope.ClearForm = function () {
        //$scope.divAuditType = false;
        //$scope.divDistrict = false;
        //$scope.divAuditor = false;
        //$scope.divBtn = false;
        //$scope.AuditorAssign.SocietyId = '0';
        //$scope.AuditorAssign.IsEmpanelledCA = undefined;
        //$scope.AuditorAssign.SocietyIdENC = '0';
        window.location.reload();

    }
    // END : Clear Data

    // START : Audit Type change (Radio button option change)
    $scope.AuditTypeChange = function () {
        $scope.AuditorAssign.DistrictId = '0';
        $scope.AuditorList = [];
        $scope.AuditorAssign.AuditorId = '0';
    }
    // END : Audit Type change (Radio button option change)

    // START : Save A A New
    $scope.SaveAuditorAssignmentOld = function (IsPermanentSave) {
        var message = "";
        var IsFileMissing = false;
        if ($scope.ShowOldYearData || ($scope.CurrentYearAuditorAppointed && $scope.CurrentYearAuditorAppointedHaveAuditReport)) {
            $('#TblEnclouser tbody tr').each(function () {
                var id = $($(this).find('td')[0]).find('label').html();
                if ((id == "1" || id == "2") && IsPermanentSave == 0) {
                    // alert(id);
                    var vidFileLength = $("#File_PDF_" + id)[0].files.length;
                    if (vidFileLength === 0) {
                        var valHfPDFfile = $("#hfpdf" + id).val();
                        if (valHfPDFfile.length == 0) {
                            $("#File_PDF_" + id).css('border', 'solid 1px red');
                            IsFileMissing = true;
                        }
                    }
                    else {
                        $("#File_PDF_" + id).css('border', 'solid 1px white');

                    }
                    //var vidFileLength2 = $("#File_Excel_" + id)[0].files.length;
                    //if (vidFileLength2 === 0) {
                    //    var valHffile = $("#hfxls" + id).val();
                    //    if (valHffile.length == 0) {
                    //        $("#File_Excel_" + id).css('border', 'solid 1px red');
                    //        IsFileMissing = true;
                    //    }
                    //}
                    //else {
                    //    $("#File_Excel_" + id).css('border', 'solid 1px white');
                    //}
                }
                if ((id == "1" || id == "2" || id == "3" || id == "4" || id == "5" || id == "6" || id == "7" || id == "8" || id == "9" || id == "10" || id == "11" || id == "12") && IsPermanentSave == 1) {
                    // alert(id);
                    var vidFileLength = $("#File_PDF_" + id)[0].files.length;
                    if (vidFileLength === 0) {
                        var valHfPDFfile = $("#hfpdf" + id).val();
                        if (valHfPDFfile.length == 0) {
                            $("#File_PDF_" + id).css('border', 'solid 1px red');
                            IsFileMissing = true;
                        }
                    }
                    else {
                        $("#File_PDF_" + id).css('border', 'solid 1px white');

                    }
                    //var vidFileLength2 = $("#File_Excel_" + id)[0].files.length;
                    //if (vidFileLength2 === 0) {
                    //    var valHffile = $("#hfxls" + id).val();
                    //    if (valHffile.length == 0) {
                    //        $("#File_Excel_" + id).css('border', 'solid 1px red');
                    //        IsFileMissing = true;
                    //    }
                    //}
                    //else {
                    //    $("#File_Excel_" + id).css('border', 'solid 1px white');
                    //}
                }
            });

            $('#AddMoreFilestbl tbody tr').each(function () {
                var id = $($(this).find('td')[0]).find('label').html();
                var txtFileNameval = $($(this).find('td')[1]).find('input').val();

                //if (txtFileNameval == '' || txtFileNameval == undefined) {
                //    $($($(this).find('td')[1]).find('input')).css('border', 'solid 1px red');
                //}
                //else {
                //    $($($(this).find('td')[1]).find('input')).css('border', 'solid 1px white');
                //}
                if (txtFileNameval != '' && txtFileNameval != undefined) {
                    var id_Pdf = $($($(this).find('td')[2]).find('input'))[0].files.length;
                    if (id_Pdf == 0) {
                        var valHfPDFofile = $("#hfpdfOther" + id).val();
                        //alert(valHfPDFofile);
                        if (valHfPDFofile.length == 0) {
                            $($($(this).find('td')[2]).find('input')).css('border', 'solid 1px red');
                            IsFileMissing = true;
                        }
                    }
                    else {
                        $($($(this).find('td')[2]).find('input')).css('border', 'solid 1px white');
                    }

                    //var id_excel = $($($(this).find('td')[3]).find('input'))[0].files.length;
                    //if (id_excel == 0) {
                    //    var valHfofile = $("#hfxlsOther" + id).val();
                    //    //alert(valHfofile);
                    //    if (valHfofile.length == 0) {
                    //        $($($(this).find('td')[3]).find('input')).css('border', 'solid 1px red');
                    //        IsFileMissing = true;
                    //    }
                    //}
                    //else {
                    //    $($($(this).find('td')[3]).find('input')).css('border', 'solid 1px white');
                    //}
                }

            });
        }

        if (IsFileMissing == true) {
            message += $scope._language == 'en-us' ? '\nPlease upload file where contain red border' : '\nकृपया फ़ाइल अपलोड करें जहाँ लाल सीमा है |';
            message += '<br />';
        }
        //$('#AddMoreFilestbl tbody tr').each(function () {
        //    if ($(this).find('td:eq(2)').find('input').val().length == 0) {
        //        $(this).css('border', 'solid 1px red');
        //        // return;
        //    }
        //    else {
        //        $(this).css('border', 'solid 1px');
        //    }
        //});



        // Add Files

        if ($scope.AuditorAssign.SocietyIdENC == '0') {
            message += $scope._language == 'en-us' ? '\nPlease Select Society' : '\nकृपया सोसाइटी का चयन करे |';
            message += '<br />';
        }
        if ($scope.AuditorAssign.IsEmpanelledCA == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Select Audit By' : '\nकृपया लेखा परीक्षा  का चयन करे |';
            message += '<br />';
        }
        if ($scope.AuditorAssign.Auditorfees == '' || $scope.AuditorAssign.Auditorfees == undefined || $scope.AuditorAssign.Auditorfees == '0') {
            message += $scope._language == 'en-us' ? '\n Please fill auditor fees' : '\n कृपया ऑडिट फीस भरें |';
            message += '<br />';
        }

        if ($scope.ShowOldYearData || ($scope.CurrentYearAuditorAppointed && $scope.CurrentYearAuditorAppointedHaveAuditReport)) {
            if ($("#AuditStartDate").val() == '') {
                message += $scope._language == 'en-us' ? '\n Please fill Audit Start Date' : '\n कृपया लेखा परीक्षा प्रारंभ दिनांक भरें |';
                message += '<br />';
            }

            if ($("#AuditEndDate").val() == '') {
                message += $scope._language == 'en-us' ? '\n Please fill Audit End Date' : '\n कृपया लेखा परीक्षा समाप्ति दिनांक भरें |';
                message += '<br />';
            }
        }

        if ($scope.ShowDepartmentDataOld) {
            if ($("#FeeDate").val() == '') {
                message += $scope._language == 'en-us' ? '\n Please fill fees date' : '\n कृपया फीस तिथि भरें |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.ChallanNo == '' || $scope.AuditorAssign.ChallanNo == undefined) {
                message += $scope._language == 'en-us' ? '\n Please fill challan no' : '\n कृपया चालान नंबर भरें |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.InspectorName == '' || $scope.AuditorAssign.InspectorName == undefined) {
                message += $scope._language == 'en-us' ? '\n Please fill Inspector Name' : '\n कृपया इंस्पेक्टर का नाम भरें |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.ApprovedBy == '' || $scope.AuditorAssign.ApprovedBy == undefined || $scope.AuditorAssign.ApprovedBy == '0') {
                message += $scope._language == 'en-us' ? '\n Please select approved by' : '\n कृपया किसके द्वारा अनुमोदित भरें |';
                message += '<br />';
            }

        }
        else if ($scope.ShowEmpanelledCADataOld) {
            if ($("#FeeDateFirm").val() == '') {
                message += $scope._language == 'en-us' ? '\n Please fill fees date' : '\n कृपया फीस तिथि भरें |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.DistrictId == undefined || $scope.AuditorAssign.DistrictId == '0') {
                message += $scope._language == 'en-us' ? '\nPlease Select District' : '\nजिला का चयन करें |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.IsFirm == undefined) {
                message += $scope._language == 'en-us' ? '\n Please Select the Audit Type' : '\nकृपया लेखापरीक्षा प्रकार का चयन करें |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.AuditorId == '0') {
                message += $scope._language == 'en-us' ? '\n Please Select the Auditor for Audit' : '\n कृपया लेखा परीक्षा के लिए ऑडिटर का चयन करें |';
                message += '<br />';
            }
        }
        else if ($scope.CurrentYearAuditorAppointed) {
            if ($("#ResolutionDate").val() == '') {
                message += $scope._language == 'en-us' ? '\n Please fill Resolution Date' : '\n कृपया संकल्प दिनांक भरें |';
                message += '<br />';
            }
            if ($("#AudiAppoOrderDate").val() == '') {
                message += $scope._language == 'en-us' ? '\n Please fill Auditor Appointment Order Date' : '\n कृपया लेखा परीक्षक नियुक्ति आदेश दिनांक भरें |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.DistrictId == undefined || $scope.AuditorAssign.DistrictId == '0') {
                message += $scope._language == 'en-us' ? '\nPlease Select District' : '\nजिला का चयन करें |';
                message += '<br />';
            }
            if ($scope.divAuditor) {
                if ($("#FeeDateFirmCurrentYear").val() == '') {
                    message += $scope._language == 'en-us' ? '\n Please fill fees date' : '\n कृपया फीस तिथि भरें |';
                    message += '<br />';
                }
                else if (!($("#FeeDateFirmCurrentYear").val().indexOf('2019') > -1 || $("#FeeDateFirmCurrentYear").val().indexOf('2020') > -1 || $("#FeeDateFirmCurrentYear").val().indexOf('2021') > -1)) {
                    message += $scope._language == 'en-us' ? '\n Please fill proper fee date for this financial year' : '\n कृपया इस वित्तीय वर्ष के लिए सही फीस तिथि भरें |';
                    message += '<br />';
                }
                if ($scope.AuditorAssign.AuditorId == '0') {
                    message += $scope._language == 'en-us' ? '\n Please Select the Auditor for Audit' : '\n कृपया लेखा परीक्षा के लिए ऑडिटर का चयन करें |';
                    message += '<br />';
                }
                if ($scope.AuditorAssign.IsFirm == undefined) {
                    message += $scope._language == 'en-us' ? '\n Please Select the Audit Type' : '\nकृपया लेखापरीक्षा प्रकार का चयन करें |';
                    message += '<br />';
                }
            }
            if ($scope.CurrentYearAuditorAppointedByDepart) {
                if ($("#FeeDateCurrentDepart").val() == '') {
                    message += $scope._language == 'en-us' ? '\n Please fill fees date' : '\n कृपया फीस तिथि भरें |';
                    message += '<br />';
                }
                else if (!($("#FeeDateCurrentDepart").val().indexOf('2019') > -1 || $("#FeeDateCurrentDepart").val().indexOf('2020') > -1)) {
                    message += $scope._language == 'en-us' ? '\n Please fill proper fee date for this financial year' : '\n कृपया इस वित्तीय वर्ष के लिए सही फीस तिथि भरें |';
                    message += '<br />';
                }
                if ($scope.AuditorAssign.ChallanNo == '' || $scope.AuditorAssign.ChallanNo == undefined) {
                    message += $scope._language == 'en-us' ? '\n Please fill challan no' : '\n कृपया चालान नंबर भरें |';
                    message += '<br />';
                }
                if ($scope.AuditorAssign.InspectorName == '' || $scope.AuditorAssign.InspectorName == undefined) {
                    message += $scope._language == 'en-us' ? '\n Please fill Inspector Name' : '\n कृपया इंस्पेक्टर का नाम भरें |';
                    message += '<br />';
                }
                if ($scope.AuditorAssign.ApprovedBy == '' || $scope.AuditorAssign.ApprovedBy == undefined || $scope.AuditorAssign.ApprovedBy == '0') {
                    message += $scope._language == 'en-us' ? '\n Please select approved by' : '\n कृपया किसके द्वारा अनुमोदित भरें |';
                    message += '<br />';
                }
            }
            var OrderPDF = true;
            var ResolutionPDF = true;
            for (var Y = 0; Y < $scope.FileCollection.length; Y++) {
                var _TEMP1 = $scope.FileCollection[Y];
                if (_TEMP1.Id.indexOf("-2") > -1) {
                    ResolutionPDF = false
                }
                if (_TEMP1.Id.indexOf("-3") > -1) {
                    OrderPDF = false
                }
            }
            if (!(parseInt($scope.AuditorAssign.Id, 10) > 0)) {
                if (ResolutionPDF) {
                    message += $scope._language == 'en-us' ? '\n Please upload Resolution PDF.' : '\n कृपया रिज़ॉल्यूशन पीडीएफ अपलोड करें |';
                    message += '<br />';
                }
                if (OrderPDF) {
                    message += $scope._language == 'en-us' ? '\n Please upload Auditor Appointment Order PDF.' : '\n कृपया ऑडिटर अपॉइंटमेंट ऑर्डर पीडीएफ अपलोड करें |';
                    message += '<br />';
                }
            }

        }

        var _Counter = Math.max.apply(Math, $scope.FileUploadTypes.map(function (o) { return o.Id; }));



        //$($('#AddMoreFilestbl tbody tr td:nth-child(1)').find('input')).each(function (i,j) {

        //});

        // Add data in form collection 
        var formData = new FormData();
        formData.append("SocietyIdENC", $scope.AuditorAssign.SocietyIdENC);
        formData.append("FinancialYear", $scope.AuditorAssign.FinancialYear);
        formData.append("IsEmpanelledCA", $scope.AuditorAssign.IsEmpanelledCA);
        formData.append("Auditorfees", $scope.AuditorAssign.Auditorfees);
        formData.append("Id", $scope.AuditorAssign.Id);
        if ($scope.ShowOldYearData || ($scope.CurrentYearAuditorAppointed && $scope.CurrentYearAuditorAppointedHaveAuditReport)) {
            formData.append("AuditStartDate", $("#AuditStartDate").val().split("/").reverse().join("-"));
            formData.append("AuditEndDate", $("#AuditEndDate").val().split("/").reverse().join("-"));
        }
        if (IsPermanentSave == 1)
            formData.append("IsPermanentSave", true);
        else
            formData.append("IsPermanentSave", false);
        if ($scope.ShowDepartmentDataOld) {
            formData.append("FeeDate", $("#FeeDate").val().split("/").reverse().join("-"));
            formData.append("ChallanNo", $scope.AuditorAssign.ChallanNo);
            formData.append("InspectorName", $scope.AuditorAssign.InspectorName);
            formData.append("ApprovedBy", $scope.AuditorAssign.ApprovedBy);

            formData.append("IsFirm", 0);
            formData.append("DistrictId", 0);
            formData.append("AuditorId", 0);
            formData.append("LastFinancialYear", 0);
            formData.append("HaveAuditReport", true);
        }
        else if ($scope.ShowEmpanelledCADataOld) {
            formData.append("FeeDate", $("#FeeDateFirm").val().split("/").reverse().join("-"));
            formData.append("ChallanNo", 0);
            formData.append("InspectorName", '');
            formData.append("ApprovedBy", 0);
            formData.append("HaveAuditReport", true);

            formData.append("IsFirm", $scope.AuditorAssign.IsFirm);
            formData.append("DistrictId", $scope.AuditorAssign.DistrictId);
            formData.append("AuditorId", $scope.AuditorAssign.AuditorId);
            formData.append("LastFinancialYear", $scope.AuditorAssign.LastFinancialYear);
        }
        else {
            if ($scope.divAuditor) {
                formData.append("FeeDate", $("#FeeDateFirmCurrentYear").val().split("/").reverse().join("-"));
                formData.append("IsFirm", $scope.AuditorAssign.IsFirm);
                formData.append("DistrictId", $scope.AuditorAssign.DistrictId);
                formData.append("AuditorId", $scope.AuditorAssign.AuditorId);
                formData.append("LastFinancialYear", $scope.AuditorAssign.LastFinancialYear);
            }
            else if ($scope.CurrentYearAuditorAppointedByDepart) {
                formData.append("FeeDate", $("#FeeDateCurrentDepart").val().split("/").reverse().join("-"));
                formData.append("ChallanNo", $scope.AuditorAssign.ChallanNo);
                formData.append("InspectorName", $scope.AuditorAssign.InspectorName);
                formData.append("ApprovedBy", $scope.AuditorAssign.ApprovedBy);
                formData.append("DistrictId", $scope.AuditorAssign.DistrictId);
            }
            formData.append("ResolutionDate", $("#ResolutionDate").val().split("/").reverse().join("-"));
            formData.append("AudiAppoOrderDate", $("#AudiAppoOrderDate").val().split("/").reverse().join("-"));
            formData.append("HaveAuditReport", $scope.AuditorAssign.HaveAuditReport);
        }

        // $scope.FileCollection.push({ 'Id': rowDataSplitter[2], 'FileType': rowDataSplitter[1], 'File': file[0], 'Title': _Title });
        for (var Y = 0; Y < $scope.FileCollection.length; Y++) {
            var _TEMP1 = $scope.FileCollection[Y];
            formData.append(_TEMP1.FileType + 'β' + _TEMP1.Id + 'β' + _TEMP1.Title, _TEMP1.File);
        }


        if (message.length > 5) {
            ShowModelAlert("Assign Auditor Client", message);
            return false;
        }
        else {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Society/AuditorAssignmentInsertOld", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (parseInt(data, 10) > 0) {
                        var Msg = (_language == 'en-us' ? 'Data Saved Successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                        alert(Msg);
                        window.location.reload();
                    }
                    else if (parseInt(data, 10) == -2) {
                        var Msg = (_language == 'en-us' ? 'Duplicate data.' : 'डुप्लिकेट डेटा।');
                        alert(Msg);
                    }
                    else if (parseInt(data, 10) == -1 || parseInt(data, 10) == 0) {
                        var Msg = (_language == 'en-us' ? 'Some Error Occurred Please try After Some Time' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                        alert(Msg);
                    }

                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    alert("Error while auditor assignment insert");
                    //$scope.ClearCaseRegister();
                    //$scope.SubformShow = false;
                });




            //$http.post(SiteUrl + "/Society/AuditorAssignmentInsert/", { _Model: formData }).
            //    success(function (data, status) {
            //        $scope.ClearForm();
            //        if (parseInt(data, 10) > 0) {
            //            var Msg = (_language == 'en-us' ? 'Auditor Assignment Successfully' : 'लेखा परीक्षक असाइनमेंट सफलतापूर्वक कर दिया गया है ');
            //        }
            //        else if (parseInt(data, 10) == -2) {
            //            var Msg = (_language == 'en-us' ? 'Duplicate data.' : 'डुप्लिकेट डेटा।');
            //        }
            //        else if (parseInt(data, 10) == -1) {
            //            var Msg = (_language == 'en-us' ? 'Some Error Occurred Please try After Some Time' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
            //        }

            //        $scope.AssignedAuditorList();

            //        alert(Msg);
            //    }).
            //    error(function (data, status) {
            //        alert('Error while auditor data insert.');
            //    });
        }
    }
    // END : Save A A New

    $scope.SaveAuditorAssignment = function (IsPermanentSave) {
        message = "";
        if ($scope.AuditorAssign.SocietyIdENC == '0') {
            message += $scope._language == 'en-us' ? '\nPlease Select Society' : '\nकृपया सोसाइटी का चयन करे |';
            message += '<br />';
        }
        if ($scope.AuditorAssign.IsEmpanelledCA == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Select Audit By' : '\nकृपया लेखा परीक्षा  का चयन करे |';
            message += '<br />';
        }
        if ($scope.AuditorAssign.IsEmpanelledCA == 'true') {
            if ($scope.AuditorAssign.IsFirm == undefined) {
                message += $scope._language == 'en-us' ? '\n Please Select the Audit Type' : '\nकृपया लेखापरीक्षा प्रकार का चयन करें |';
                message += '<br />';
            }
        }
        if ($scope.AuditorAssign.IsEmpanelledCA == 'true') {
            if ($scope.AuditorAssign.AuditorId == '0') {
                message += $scope._language == 'en-us' ? '\n Please Select the Auditor for Audit' : '\n कृपया लेखा परीक्षा के लिए ऑडिटर का चयन करें |';
                message += '<br />';
            }
        }
        if ($scope.AuditorAssign.DistrictId == undefined || $scope.AuditorAssign.DistrictId == '0') {
            message += $scope._language == 'en-us' ? '\nPlease Select District' : '\nजिला का चयन करें |';
            message += '<br />';
        }
        if ($scope.AuditorAssign.IsEmpanelledCA == 'true') {
            if ($scope.AuditorAssign.Auditorfees == '' || $scope.AuditorAssign.Auditorfees == undefined || $scope.AuditorAssign.Auditorfees == '0') {
                message += $scope._language == 'en-us' ? '\n Please fill auditor fees' : '\n कृपया ऑडिट फीस भरें |';
                message += '<br />';
            }
        }
        if ($scope.AuditorAssign.IsEmpanelledCA == 'true') {
            if ($scope.AuditorAssign.LastFinancialYear == '0') {
                message += $scope._language == 'en-us' ? '\n Please select last audit year' : '\n कृपया पिछले ऑडिट वर्ष का चयन करें |';
                message += '<br />';
            }
        }
        if (message.length > 5) {
            ShowModelAlert("Assign Auditor Client", message);
            return false;
        }
        else {
            //alert($scope.AuditorAssign.IsEmpanelledCA);
            //alert(JSON.stringify($scope.AuditorAssign));
            $scope.AuditorAssign.IsPermanentSave = IsPermanentSave
            $http.post(SiteUrl + "/Society/AuditorAssignmentInsertNew/", { _Model: $scope.AuditorAssign }).
                success(function (data, status) {
                    // $scope.ClearForm();
                    if (parseInt(data, 10) > 0) {
                        var Msg = (_language == 'en-us' ? 'Auditor Assignment Successfully' : 'लेखा परीक्षक असाइनमेंट सफलतापूर्वक कर दिया गया है ');
                    }
                    else if (parseInt(data, 10) == -2) {
                        var Msg = (_language == 'en-us' ? 'Duplicate data.' : 'डुप्लिकेट डेटा।');
                    }
                    else if (parseInt(data, 10) == -1) {
                        var Msg = (_language == 'en-us' ? 'Some Error Occurred Please try After Some Time' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                    }

                    //$scope.AssignedAuditorList();

                    alert(Msg);
                    window.location.reload();

                }).
                error(function (data, status) {
                    alert('Error while auditor data insert.');
                });
        }
    }


    $scope.SetDeleteStatusSAA = function (SAAId) {
        var ActionId = 2;
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Society/DeleteSocietyAuditorAssigments", { Id: SAAId, Action: ActionId }).
            success(function (data, status) {
                $scope.GetSocietyList();
                $scope.AssignedAuditorList();
            }).
            error(function (data, status) {
            });
    };


    //START: Generate Add More Files DIV
    $scope.checkFileLengthPage = function (fileSized) {

        if (fileSized < 2100000) {
            return true;
        }
        return false;
    }

    $scope.GetAddMoreFilesDiv = function () {

        $scope.divMoreFilesAdd = true;
        $scope.records = [];
        var X = $scope.OtherFileUpload.AddMoreFile;
        records = [];
        var _Counter = $scope.MaxCountAddMoreFile = Math.max.apply(Math, $scope.FileUploadTypes.map(function (o) { return o.Id; }));
        for (var x = 1; x <= X; x++) {
            records.push(
                {
                    "Id": _Counter + x,
                    "DDLID": "DDLDocTypeID" + (_Counter + x),
                    "UploadFileNamePDF": "",
                    "UploadFileNameXLS": "",
                    "Name": ""
                });
        }
        $scope.records = records;
    }
    //END : Generate Add More Files DIV

    // START : Upload Files
    $scope.UploadEnclouserFile = function (file, row) {
        var DDLIdLocal = $(row).attr("DDLID");
        var DDLIdVal = $("#" + DDLIdLocal).val();

        if (!$scope.checkFileLengthPage(file[0].size)) {
            alert(_language === 'en-us' ? "File size exceeded.\n Please select files have size less then 2 MB or Equivalent" : "फ़ाइल का आकार से अधिक हो गई है|\nकृपया चुनिंदा फ़ाइलों का आकार 2 MB से कम करें|");
            $(row).val('');
            $(row).parent().find('.filesize').text('');
            return;
        }

        var _Counter = Math.max.apply(Math, $scope.FileUploadTypes.map(function (o) { return o.Id; }));

        var rowDataSplitter = DDLIdLocal.split('_');

        if (rowDataSplitter[1] == 'PDF') {
            if (!(file[0].name.indexOf(".pdf") > -1)) {
                alert("Please PDF File Only.")
                file[0] = null;
                $(row).val(null);
                return false;
            }
        }
        else if ($(row).attr("filetype") == 'worddocument') {
            if (!(file[0].name.indexOf(".doc") > -1)) {
                alert("You have selected invalid Filetype. \n Valid Filetype .doc,.docx.")
                file[0] = null;
                $(row).val(null);
                return false;
            }
        }
        else if ($(row).attr("filetype") == 'excel') {
            if (!(file[0].name.indexOf(".doc") > -1)) {
                alert("You have selected invalid Filetype. \n Valid Filetype .xlsx")
                file[0] = null;
                $(row).val(null);
                return false;
            }
        }

        var _Title = 'NA';
        if (parseInt(rowDataSplitter[2], 10) > _Counter) {
            _Title = $('#Name' + rowDataSplitter[2]).val() == undefined ? 'NA' : $('#Name' + rowDataSplitter[2]).val();
        }
        if ($scope.FileCollection.length == 0 && file.length > 0) {
            $scope.FileCollection.push({ 'Id': rowDataSplitter[2], 'FileType': rowDataSplitter[1], 'File': file[0], 'Title': _Title });
        }
        else {
            if ($scope.FileCollection.filter(X => X.Id == rowDataSplitter[2] && X.FileType == rowDataSplitter[1]).length == 0) {
                $scope.FileCollection.push({ 'Id': rowDataSplitter[2], 'FileType': rowDataSplitter[1], 'File': file[0], 'Title': _Title });
            }
            else {
                if (file.length == 0) {
                    angular.forEach($scope.FileCollection, function (i, item) {
                        if (item.Id == rowDataSplitter[2] && item.FileType == rowDataSplitter[1]) {
                            $scope.FileCollection.splice(i, 1);
                        }
                    });
                }
                else {
                    angular.forEach($scope.FileCollection, function (item) {
                        if (item.Id == rowDataSplitter[2] && item.FileType == rowDataSplitter[1]) {
                            item.File = file[0];
                        }
                    });
                }
            }
        }
    }
    //END : Upload Files

    // START :FileUploadTypeList
    $scope.FileUploadTypeList = function () {
        $http.post(SiteUrl + "/Society/GetUploadFileType/").
            success(function (data, status) {
                $scope.FileUploadTypes = data;
            }).
            error(function (data, status) {
                $scope.FileUploadTypes = [];
                alert('Error while File Upload Type List fetch.');
            });
    }
    //END: FileUploadTypeList


    // START : Auditor Assignment List
    $scope.AssignedAuditorList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Society/AuditorAssignmentList/").
            success(function (data, status) {
                $scope.AssignedAuditor = data;
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

                //if (data != null && data.length == 0) {
                //    $scope.divAuditorData = false;
                //}
                //else {
                //    $scope.divAuditorData = true;
                //}

            }).
            error(function (data, status) {
                $scope.AssignedAuditor = [];
                alert('Error while auditor data list fetch.');
            });
        $(".se-pre-con").fadeOut("slow");
    }
    // END : Auditor Assignment List
    //GET Load Table data
    $scope.DashboardData = function (ActId, SocietyTypeId, ClassId) {
        $http.post(SiteUrl + "/Citizen/CitizenDashboardDataWithDepartmentUser", { "ActId": ActId == undefined ? 0 : parseInt(ActId), "SocietyTypeId": SocietyTypeId == undefined ? 0 : parseInt(SocietyTypeId), "ClassId": ClassId == undefined ? 0 : parseInt(ClassId) }).
            success(function (data, status) {
                $scope.TableData = [];
                $scope.TableData = data;

                if ($scope.TableData.length > 0) {



                    // $scope.GetSocietyDownloadList($scope.TableData[0].SocietyIdENC);                  
                }

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
            }).
            error(function (data, status) {
                $scope.TableData = [];
            });

    }
    //END
    $scope.GenPDF = function (SAAId) {
        printDataJS(SAAId);
    }
    // START : Get Auditor data on deistrict and firm type change
    $scope.GetAuditorDataByFirmType = function () {
        $scope.GetAuditorData();
    }
    // END : Get Auditor data on deistrict and firm type change
    $scope.ViewDocuments = function (Id) {
        $(".se-pre-con").fadeIn("slow");
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
    // START : On Page List
    $scope.getTranslation();
    $scope.GetSocietyList();
    $scope.GetDistrictList();
    $scope.AssignedAuditorList();
    $scope.FileUploadTypeList();
    //if ($('#lblUserType').html() == 'D') {
    //    $scope.DashboardData();
    //}
    //END : On PAge List

});