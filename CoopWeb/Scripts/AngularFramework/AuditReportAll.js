var AuditReportForDashboardModule = angular.module("AuditReportAllMod", []);
AuditReportForDashboardModule.controller("AuditReportAllCnt", function ($scope, $http) {
    $scope.AuditReport = [];
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.AuditReport.TypeId = 1;
    $scope.AuditReport.SaveStatus = 1;
    $scope.HouseAuditSocietyShow = false;
    $scope.OtherAuditSocietyShow = false;
    $scope.HouseSocietyDetailsData = [];
    $scope.OtherSocietyDetailsData = [];
    // START : Code for File Upload
    $scope.ClearReport = function () {
        $scope.AuditReport.Year = '0';
        $scope.AuditReport.Month = '0';
        $scope.AuditReport.TypeId = 1;
        $scope.AuditReport.SaveStatus = 1;
        $scope.HouseAuditSocietyShow = false;
        $scope.OtherAuditSocietyShow = false;
        $scope.HouseSocietyDetailsData = [];
        $scope.OtherSocietyDetailsData = [];
    };



    // START : Call default functions on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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

        }
        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
        //    success(function (data, status) {
        //        $scope.FinancialYearList = data;
        //    }).
        //    error(function (data, status) {
        //        $scope.FinancialYearList = [];

        //    });

        $http.post(SiteUrl + "/Auditor/GetAuditYearList").
            success(function (data, status) {
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];

            });

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=-1").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : Call default functions on page load
    $scope.TotalRecord=0;
    $scope.GetAuditReport = function () {
        $scope.TotalRecord = 0;
        $scope.HouseSocietyDetailsData = [];
        $scope.OtherSocietyDetailsData = [];
        $scope.ShowCurrentYearData = false;
        $scope.ShowOldYearData = false;
        $scope.CurrentYearAuditorAppointed = false;
        $scope.CurrentYearAuditorAppointedHaveAuditReport = false;
        $scope.HouseAuditSocietyShow = false;
        $scope.OtherAuditSocietyShow = false;

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.AuditReport.Year == '0') {
            message += $scope._language == 'en-us' ? '<br/>Please Select Year' : '<br/>कृपया वर्ष चुनें';
        }

        //if ($scope.AuditReport.DistrictId == '0') {
        //    message += $scope._language == 'en-us' ? '<br/>Please Select Month' : '<br/>कृपया माह का चयन करें';
        //}

        if (message.length > 27) {
            ShowModelAlert("Audit Report Show", message);
            return false;
        }

        $(".se-pre-con").fadeIn("slow");
        if ($scope.AuditReport.TypeId == 1) {
            $http.post(SiteUrl + "/Auditor/GetHouseAuditReport", { "Year": $scope.AuditReport.Year, "DistrictId": $scope.AuditReport.DistrictId}).
                success(function (data, status) {
                    if (data.length > 0) {
                        //alert(data.length);
                        $scope.TotalRecord = data.length;
                        $scope.HouseSocietyDetailsData = data;
                        $scope.HouseAuditSocietyShow = true;
                    }
                    else {
                        var msg = _language == 'en-us' ? 'Data not found.' : 'डेटा नहीं मिला।';
                        alert(msg);
                    }
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) { 
                    $(".se-pre-con").fadeOut("slow");
                });
        }
        else if ($scope.AuditReport.TypeId == 2) {
            $http.post(SiteUrl + "/Auditor/GetNotHouseAuditReport", { "Year": $scope.AuditReport.Year, "DistrictId": $scope.AuditReport.DistrictId, "SaveStatus": $scope.AuditReport.SaveStatus} ).
                success(function (data, status) {
                    if (data.length > 0) {
                        //alert(data.length);
                        $scope.TotalRecord = data.length;
                        $scope.OtherSocietyDetailsData = data;
                        $scope.OtherAuditSocietyShow = true;
                    }
                    else {
                        var msg = _language == 'en-us' ? 'Data not found.' : 'डेटा नहीं मिला।';
                        alert(msg);
                    }
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                });
        }
    };
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
    };

    $scope.SocietyName = '';
    $scope.FIN_YEAR_NAME = '';
    $scope.ShowOldYearDataDetail = function (SocietyIdENC, FYearId, SocietyName, FIN_YEAR_NAME) {

        //if ($scope.AuditorAssign.SocietyIdENC == 0) {
        //    alert("Please Selact Society.");
        //    $scope.AuditorAssign.FinancialYear = '0';
        //    return;
        //}
        $scope.SocietyName = SocietyName;
        $scope.FIN_YEAR_NAME = FIN_YEAR_NAME;
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
        $http.post(SiteUrl + "/Auditor/GetSocietyAuditDataByFinYearId", { "SocietyIdENC":SocietyIdENC, "FinYearId": FYearId }).
            success(function (data, status) {
                if (data.Table.length > 0) {
                    $scope.AuditorAssignOldData = data.Table[0];
                    $scope.AuditorAssign = data.Table[0];
                    $scope.AuditorAssign.FinancialYear = FYearId;
                    $scope.AuditorAssign.HaveAuditReport = $scope.AuditorAssign.HaveAuditReportVal.toString();
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
            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                alert('Error while fetching society list data.');
            });
    };
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
    };

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
    };
    // START : Call Functions on Page Load
    $scope.OnPageLoad();
    //END : Call Functions on Page Load
});