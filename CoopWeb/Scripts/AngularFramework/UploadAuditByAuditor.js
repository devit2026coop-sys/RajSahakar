var AssignAuditorModule = angular.module('UploadAuditByAuditorMod', ['angularUtils.directives.dirPagination']);
AssignAuditorModule.controller("UploadAuditByAuditorCont", function ($scope, $http) {



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
    $scope.ExtraFileCounter = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];



    // Code for Language label setting
    $scope.getTranslation = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
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
                $scope.FinancialYearList = data;



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
                    maxDate: '0D'
                });



                $('#AudiAppoOrderDate').datepicker({
                    showOn: "both",
                    changeMonth: true,
                    buttonImage: "../images/calender.png",
                    changeYear: true,
                    dateFormat: "dd/mm/yy",
                    maxDate: '31/05/2021'
                });
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    };



    $scope.pageChangeHandler = function (num) {



    };





    $scope.GetCourtSectionList = function () {
        $scope.CourtSectionListData = [];
        $http.get(SiteUrl + "/Citizen/CourtSectionList").
            success(function (data, status) {
                $scope.CourtSectionListData = data;
            }).
            error(function (data1, status) {
                $scope.CourtSectionListData = [];
            });
    }




    $scope.ResetOnFyearChange = function () {
        
        $scope.AuditorAssign.SocietyIdENC = '0';
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
        $scope.GetSocietyList();
    };
    $scope.AuditorAssignOldData = [];



    $scope.CurrentYearAssignAuditor = function (Flag) {
        //if (Flag == false) {
        // alert("You can't request auditor appointment.(Last date has been gone)");
        //}
        $scope.CurrentYearAuditorAppointed = Flag;
    }



    $scope.HaveAuditReportSelection = function (Flag) {
        $scope.CurrentYearAuditorAppointedHaveAuditReport = Flag;
        $scope.ShowOldYearData = Flag;
    }




    //bhuendra1
    //$scope.GetCourtSectionList();



    $scope.ShowOldYearDataDetail = function (FYearId) {
        if ($scope.AuditorAssign.SocietyIdENC == 0) {
            alert("Please Selact Society.");
            $scope.AuditorAssign.FinancialYear = '0';
            return;
        }




        $scope.AuditorAppointedDateSelection = undefined;
        $scope.AuditorAssign.IsEmpanelledCA = undefined;
        $scope.ShowEmpanelledCADataOld = false;
        $scope.ShowDepartmentDataOld = false;



        $scope.IsDepartmentalAuditor = 0;




        if (FYearId > 48) {
            $scope.ShowCurrentYearData = true;
            $scope.ShowOldYearData = false;
        }
        else {
            $scope.ShowCurrentYearData = false;
            $scope.ShowOldYearData = true;
        }
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Auditor/GetSocietyAuditDataByFinYearId", { "SocietyIdENC": $scope.AuditorAssign.SocietyIdENC, "FinYearId": FYearId }).
            success(function (data, status) {
                if (data.Table.length > 0) {
                    $scope.AuditorAssignOldData = data.Table[0];
                    $scope.AuditorAssign = data.Table[0];
                    $scope.AuditorAssign.FinancialYear = FYearId;



                    $scope.IsDepartmentalAuditor = $scope.AuditorAssign.IsDepartmentalAuditor;



                    if ($scope.AuditorAssign.IsEmpanelledCA) {
                        $scope.AuditorAssign.IsEmpanelledCA = $scope.AuditorAssign.IsEmpanelledCA;
                        $scope.divAuditType = true;
                        $scope.divDistrict = true;
                        $scope.divAuditor = true;
                        $scope.divBtn = true;
                        $scope.ShowDepartmentDataOld = false;
                        $scope.ShowEmpanelledCADataOld = true;
                        $scope.CurrentYearAuditorAppointedByDepart = false;
                    }
                    else {
                        $scope.AuditorAssign.IsEmpanelledCA = $scope.AuditorAssign.IsEmpanelledCA;
                        $scope.divAuditType = false;
                        $scope.divDistrict = true;
                        $scope.divAuditor = false;
                        $scope.divBtn = true;
                        $scope.ShowDepartmentDataOld = true;
                        $scope.ShowEmpanelledCADataOld = false;
                        $scope.CurrentYearAuditorAppointedByDepart = true;
                    }
                    if ($scope.AuditorAssign.IsIrregularity) {
                        $scope.AuditorAssign.IsIrregularity = 1;
                    }
                    if ($scope.AuditorAssign.IsActionisrequired) {
                        $scope.AuditorAssign.IsActionisrequired = 1;
                        $scope.AuditorAssign.SectionId = $scope.AuditorAssign.SectionId;



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
                    $(".se-pre-con").fadeOut("slow");
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
                    $(".se-pre-con").fadeOut("slow");
                }
                // $scope.DeshboardLeft = data.Table1;



            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.SocietyList = [];
                alert('Error while fetching society list data.');
            });
    };



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
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Auditor/GetSocietyListAssignedToAuditor", { "FYearId": $scope.AuditorAssign.FinancialYear }).
            success(function (data, status) {
                $scope.SocietyList = data;
                $scope.AssignedAuditor = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.SocietyList = [];
                alert('Error while fetching society list data.');
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



    // START :Get List of Districts
    $scope.GetAuditCategoryList = function () {
        $http.get(SiteUrl + "/Lov/GetLovValuesByListId/?ListId=8").
            success(function (data, status) {
                $scope.AuditCategoryList = data;
            }).
            error(function (data, status) {
                $scope.AuditCategoryList = [];
                alert('Error while fetching Audit Category list.');
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
        if ($scope.ShowOldYearData || ($scope.CurrentYearAuditorAppointed)) {
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
                    // var valHffile = $("#hfxls" + id).val();
                    // if (valHffile.length == 0) {
                    // $("#File_Excel_" + id).css('border', 'solid 1px red');
                    // IsFileMissing = true;
                    // }
                    //}
                    //else {
                    // $("#File_Excel_" + id).css('border', 'solid 1px white');
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
                    // var valHffile = $("#hfxls" + id).val();
                    // if (valHffile.length == 0) {
                    // $("#File_Excel_" + id).css('border', 'solid 1px red');
                    // IsFileMissing = true;
                    // }
                    //}
                    //else {
                    // $("#File_Excel_" + id).css('border', 'solid 1px white');
                    //}
                }
            });



            $('#AddMoreFilestbl tbody tr').each(function () {
                var id = $($(this).find('td')[0]).find('label').html();
                var txtFileNameval = $($(this).find('td')[1]).find('input').val();



                //if (txtFileNameval == '' || txtFileNameval == undefined) {
                // $($($(this).find('td')[1]).find('input')).css('border', 'solid 1px red');
                //}
                //else {
                // $($($(this).find('td')[1]).find('input')).css('border', 'solid 1px white');
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



                    var id_excel = $($($(this).find('td')[3]).find('input'))[0].files.length;
                    if (id_excel == 0) {
                        var valHfofile = $("#hfxlsOther" + id).val();
                        //alert(valHfofile);
                        if (valHfofile.length == 0) {
                            $($($(this).find('td')[3]).find('input')).css('border', 'solid 1px red');
                            IsFileMissing = true;
                        }
                    }
                    else {
                        $($($(this).find('td')[3]).find('input')).css('border', 'solid 1px white');
                    }
                }



            });
        }



        var OrderPDF = true;
        var ResolutionPDF = true;
        for (var Y = 0; Y < $scope.FileCollection.length; Y++) {
            var _TEMP1 = $scope.FileCollection[Y];



            if (_TEMP1.Id.indexOf("-3") > -1) {
                OrderPDF = false
            }
        }



        //if ((parseInt($scope.AuditorAssign.Id, 10) > 0)) {



        // if (OrderPDF) {
        // message += $scope._language == 'en-us' ? '\n Please upload Auditor Appointment Order PDF.' : '\n कृपया ऑडिटर अपॉइंटमेंट ऑर्डर पीडीएफ अपलोड करें |';
        // message += '<br />';
        // }
        //}



        if (IsFileMissing == true) {
            message += $scope._language == 'en-us' ? '\nPlease upload file where contain red border' : '\nकृपया फ़ाइल अपलोड करें जहाँ लाल सीमा है |';
            message += '<br />';
        }
        //$('#AddMoreFilestbl tbody tr').each(function () {
        // if ($(this).find('td:eq(2)').find('input').val().length == 0) {
        // $(this).css('border', 'solid 1px red');
        // // return;
        // }
        // else {
        // $(this).css('border', 'solid 1px');
        // }
        //});





        // Add Files



        if ($scope.AuditorAssign.SocietyIdENC == '0') {
            message += $scope._language == 'en-us' ? '\nPlease Select Society.' : '\nकृपया सोसाइटी का चयन करे |';
            message += '<br />';
        }
        if ($scope.AuditorAssign.IsEmpanelledCA == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Select Audit By.' : '\nकृपया लेखा परीक्षा का चयन करे |';
            message += '<br />';
        }
        if ($scope.IsDepartmentalAuditor != 1) {
            if ($scope.AuditorAssign.Auditorfees == '' || $scope.AuditorAssign.Auditorfees == undefined || $scope.AuditorAssign.Auditorfees == '0') {
                message += $scope._language == 'en-us' ? '\n Please fill auditor fees.' : '\n कृपया ऑडिट फीस भरें |';
                message += '<br />';
            }
        }



        if ($scope.ShowOldYearData || ($scope.CurrentYearAuditorAppointed)) {



            if ($("#AuditStartDate").val() == '') {
                message += $scope._language == 'en-us' ? '\n Please fill Audit Start Date.' : '\n कृपया लेखा परीक्षा प्रारंभ दिनांक भरें |';
                message += '<br />';
            }



            if ($("#AuditEndDate").val() == '') {
                message += $scope._language == 'en-us' ? '\n Please fill Audit End Date.' : '\n कृपया लेखा परीक्षा समाप्ति दिनांक भरें |';
                message += '<br />';
            }



            if ($scope.AuditorAssign.IsDeterminedFormat == undefined) {
                message += $scope._language == 'en-us' ? '\n Please select the audit report is in the format by the department.' : '\n कृपया ऑडिट रिपोर्ट विभाग द्वारा निर्धारित प्रारूप में है या नहीं का चयन करे |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.IsApprovedByGeneralAssembly == undefined) {
                message += $scope._language == 'en-us' ? '\n Please select the audit report is approved by the General Assembly or not.' : '\n क्या ऑडिट रिपोर्ट आम सभा से अनुमोदित है या नहीं का चयन करे |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.IsDesiredDocumentAttached == undefined) {
                message += $scope._language == 'en-us' ? '\n Please select the desired document in the audit report or not.' : '\n क्या ऑडिट रिपोर्ट मैं वांछित दस्तावेज संगलन है या नहीं का चयन करे|';
                message += '<br />';
            }
            if ($scope.AuditorAssign.IsAllAuditReportDocumentsAttached == undefined) {
                message += $scope._language == 'en-us' ? '\n Please select all accounts are included in audit report or not.' : '\n क्या ऑडिट रिपोर्ट में समस्त लेखे संगलन है या नहीं का चयन करे |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.IsComplianceReportApprovedByGeneralAssembly == undefined) {
                message += $scope._language == 'en-us' ? '\n Please Select audit compliance report is approved by the General Assembly or not.' : '\n क्या ऑडिट अनुपालना रिपोर्ट आम सभा से अनुमोदित है या नहीं का चयन करे |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.ClassificationId == '' || $scope.AuditorAssign.ClassificationId == undefined || $scope.AuditorAssign.ClassificationId == '0') {
                message += $scope._language == 'en-us' ? '\n Please select the classification of audit report.' : '\n कृपया ऑडिट रिपोर्ट का वर्गीकरण दर्ज करे |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.TotalActivMembers == '' || $scope.AuditorAssign.TotalActivMembers == undefined) {
                message += $scope._language == 'en-us' ? '\n Please enter the total number of elected members.' : '\n कृपया समिति में निर्वाचित संचालक मंडल या प्रशासक कार्यरत सदस्य कुल संख्या दर्ज करे |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.ACategory == '' || $scope.AuditorAssign.ACategory == undefined) {
                message += $scope._language == 'en-us' ? '\n Please enter the number of A category members.' : '\n कृपया अ श्रेणी सदस्यों की संख्या दर्ज करे|';
                message += '<br />';
            }
            if ($scope.AuditorAssign.BCategory == '' || $scope.AuditorAssign.BCategory == undefined) {
                message += $scope._language == 'en-us' ? '\n Please enter the number of B category members.' : '\n कृपया ब श्रेणी सदस्यों की संख्या दर्ज करे |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.CCategory == '' || $scope.AuditorAssign.CCategory == undefined) {
                message += $scope._language == 'en-us' ? '\n Please enter the number of C category members.' : '\n कृपया स श्रेणी सदस्यों की संख्या दर्ज करे |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.CeoName == '' || $scope.AuditorAssign.CeoName == undefined) {
                message += $scope._language == 'en-us' ? '\n Please enter the name of the Chief Executive Officer.' : '\n कृपया मुख्य कार्यकारी अधिकारी का नाम दर्ज करे |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.IsIrregularity == undefined || $scope.AuditorAssign.IsIrregularity == null) {
                message += $scope._language == 'en-us' ? '\n Please select an audit report or not.' : '\n क्या ऑडिट रिपोर्ट मैं कोई अनीयमितता है या नहीं का चयन करे |';
                message += '<br />';
            }
            else {
                if ($scope.AuditorAssign.IsIrregularity == 1) {
                    if ($scope.AuditorAssign.IrregularityType == '' || $scope.AuditorAssign.IrregularityType == undefined) {
                        message += $scope._language == 'en-us' ? '\n Please enter Type of irregularity.' : '\n कृपया अनियमितता का प्रकार दर्ज करे |';
                        message += '<br />';
                    }
                    if ($scope.AuditorAssign.DescriptionofIrregularity == '' || $scope.AuditorAssign.DescriptionofIrregularity == undefined) {
                        message += $scope._language == 'en-us' ? '\n Please enter a brief description.' : '\n कृपया संक्षिप्त विवरण दर्ज करे |';
                        message += '<br />';
                    }
                    if ($scope.AuditorAssign.ResponsibleOfficerName == '' || $scope.AuditorAssign.ResponsibleOfficerName == undefined) {
                        message += $scope._language == 'en-us' ? '\n Please enter the name of the responsible officer / personnel.' : '\n कृपया उत्तरदायी अधिकारी / कार्मिक का नाम दर्ज करे |';
                        message += '<br />';
                    }
                    if ($scope.AuditorAssign.IrregularityType != 1) {



                        if ($scope.AuditorAssign.IrregularityAmount == '' || $scope.AuditorAssign.IrregularityAmount == undefined) {
                            message += $scope._language == 'en-us' ? '\n Please enter the amount.' : '\n कृपया रकम दर्ज करे |';
                            message += '<br />';
                        }
                    }



                }
            }






            if ($scope.AuditorAssign.IsProfit == undefined) {
                message += $scope._language == 'en-us' ? '\n Please select whether there is a profit and loss situation.' : '\n कृपया लाभ हानि की स्थिति है या नहीं का चयन करे |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.TotalMembers == '' || $scope.AuditorAssign.TotalMembers == undefined) {
                message += $scope._language == 'en-us' ? '\n Please enter total members.' : '\n कृपया कुल सदस्य दर्ज करे |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.AccumulateProfitLoss == '' || $scope.AuditorAssign.AccumulateProfitLoss == undefined) {
                message += $scope._language == 'en-us' ? '\n Please enter accumulated profit or loss.' : '\n कृपया संचित लाभ या हानि दर्ज करे |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.Grossprofitor == '' || $scope.AuditorAssign.Grossprofitor == undefined) {
                message += $scope._language == 'en-us' ? '\n Please enter gross profit or loss.' : '\n कृपया सकल लाभ या हानि दर्ज करे |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.TotalAmonut == '' || $scope.AuditorAssign.TotalAmonut == undefined) {
                message += $scope._language == 'en-us' ? '\n Please enter the total share amount.' : '\n कृपया कुल हिस्सा राशि दर्ज करे |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.Membersshareamount == '' || $scope.AuditorAssign.Membersshareamount == undefined) {
                message += $scope._language == 'en-us' ? '\n Please enter the members share amount.' : '\n कृपया सदस्यों की हिस्सा राशि दर्ज करे |';
                message += '<br />';
            }
            //
            if ($scope.AuditorAssign.StateGovtAmount == '' || $scope.AuditorAssign.StateGovtAmount == undefined) {
                message += $scope._language == 'en-us' ? '\n Please enter the state government share amount.' : '\n कृपया राज्य सरकार की हिस्सा राशि दर्ज करे |';
                message += '<br />';
            }
            if ($scope.AuditorAssign.IsActionisrequired == undefined || $scope.AuditorAssign.IsActionisrequired == null) {
                message += $scope._language == 'en-us' ? '\n Please seect Whether action is required / proposed under any act or not.' : '\n क्या कोई अधिनियमा अंतर्गत कार्यवाही अपेक्षित है/ प्रस्तावित की गई है या नहीं का चयन करे |';
                message += '<br />';
            }
            else {
                if ($scope.AuditorAssign.IsActionisrequired == 1) {



                    if ($scope.AuditorAssign.ActionDescription == '' || $scope.AuditorAssign.ActionDescription == undefined) {
                        message += $scope._language == 'en-us' ? '\n Please Enter brief details.' : '\n कृपया संक्षिप्त विवरण दर्ज करे |';
                        message += '<br />';
                    }
                    if ($scope.AuditorAssign.IrregularityOfficerName == '' || $scope.AuditorAssign.IrregularityOfficerName == undefined) {
                        message += $scope._language == 'en-us' ? '\n Please enter the name of the responsible officer / personnel.' : '\n कृपया उत्तरदायी अधिकारी / कार्मिक का नाम दर्ज करे |';
                        message += '<br />';
                    }
                    if ($scope.AuditorAssign.SectionId == '' || $scope.AuditorAssign.SectionId == undefined || $scope.AuditorAssign.SectionId == '0') {
                        message += $scope._language == 'en-us' ? '\n Please enter the section.' : '\n कृपया अनुभाग दर्ज करे |';
                        message += '<br />';
                    }
                }
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
            if ($scope.divAuditor && $scope.ShowCurrentYearData) {
                if ($scope.IsDepartmentalAuditor != 1) {
                    if ($("#FeeDateFirmCurrentYear").val() == '') {
                        message += $scope._language == 'en-us' ? '\n Please fill fees date' : '\n कृपया फीस तिथि भरें |';
                        message += '<br />';
                    }
                    //else if (!($("#FeeDateFirmCurrentYear").val().indexOf('2021') > -1 || $("#FeeDateFirmCurrentYear").val().indexOf('2022') > -1)) {
                    //    message += $scope._language == 'en-us' ? '\n Please fill proper fee date for this financial year' : '\n कृपया इस वित्तीय वर्ष के लिए सही फीस तिथि भरें |';
                    //    message += '<br />';
                    //}
                    else if (!($("#FeeDateFirmCurrentYear").val().indexOf('2023') > -1 || $("#FeeDateFirmCurrentYear").val().indexOf('2024') > -1)) {
                        message += $scope._language == 'en-us' ? '\n Please fill proper fee date for this financial year' : '\n कृपया इस वित्तीय वर्ष के लिए सही फीस तिथि भरें |';
                        message += '<br />';
                    }
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
                //else if (!($("#FeeDateCurrentDepart").val().indexOf('2020') > -1 || $("#FeeDateCurrentDepart").val().indexOf('2021') > -1)) {
                //    message += $scope._language == 'en-us' ? '\n Please fill proper fee date for this financial year' : '\n कृपया इस वित्तीय वर्ष के लिए सही फीस तिथि भरें |';
                //    message += '<br />';
                //}
                else if (!($("#FeeDateCurrentDepart").val().indexOf('2022') > -1 || $("#FeeDateCurrentDepart").val().indexOf('2023') > -1)) {
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





        if ($scope.ShowOldYearData || ($scope.CurrentYearAuditorAppointed)) {
            formData.append("AuditStartDate", $("#AuditStartDate").val().split("/").reverse().join("-"));
            formData.append("AuditEndDate", $("#AuditEndDate").val().split("/").reverse().join("-"));
            formData.append("IsDeterminedFormat", $scope.AuditorAssign.IsDeterminedFormat == "1" ? true : false);
            formData.append("IsApprovedByGeneralAssembly", $scope.AuditorAssign.IsApprovedByGeneralAssembly == "1" ? true : false);
            formData.append("IsDesiredDocumentAttached", $scope.AuditorAssign.IsDesiredDocumentAttached == "1" ? true : false);
            formData.append("IsAllAuditReportDocumentsAttached", $scope.AuditorAssign.IsAllAuditReportDocumentsAttached == "1" ? true : false);
            formData.append("IsComplianceReportApprovedByGeneralAssembly", $scope.AuditorAssign.IsComplianceReportApprovedByGeneralAssembly == "1" ? true : false);



            formData.append("ClassificationId", $scope.AuditorAssign.ClassificationId);
            formData.append("TotalActivMembers", $scope.AuditorAssign.TotalActivMembers);
            formData.append("ACategory", $scope.AuditorAssign.ACategory);
            formData.append("BCategory", $scope.AuditorAssign.BCategory);
            formData.append("CCategory", $scope.AuditorAssign.CCategory);
            formData.append("CeoName", $scope.AuditorAssign.CeoName);
            formData.append("IsIrregularity", $scope.AuditorAssign.IsIrregularity == "1" ? true : false);
            if ($scope.AuditorAssign.IsIrregularity == 1) {
                formData.append("IrregularityType", $scope.AuditorAssign.IrregularityType);
                formData.append("DescriptionofIrregularity", $scope.AuditorAssign.DescriptionofIrregularity);
                formData.append("ResponsibleOfficerName", $scope.AuditorAssign.ResponsibleOfficerName);



                if ($scope.AuditorAssign.IrregularityType != 1) {
                    formData.append("IrregularityAmount", $scope.AuditorAssign.IrregularityAmount);
                }
            }
            formData.append("IsActionisrequired", $scope.AuditorAssign.IsActionisrequired == "1" ? true : false);
            if ($scope.AuditorAssign.IsActionisrequired == 1) {
                formData.append("ActionDescription", $scope.AuditorAssign.ActionDescription);
                formData.append("IrregularityOfficerName", $scope.AuditorAssign.IrregularityOfficerName);
                formData.append("SectionId", $scope.AuditorAssign.SectionId);
            }
            formData.append("IsProfit", $scope.AuditorAssign.IsProfit == "1" ? true : false);
            formData.append("TotalMembers", $scope.AuditorAssign.TotalMembers);
            formData.append("AccumulateProfitLoss", $scope.AuditorAssign.AccumulateProfitLoss);
            formData.append("Grossprofitor", $scope.AuditorAssign.Grossprofitor);
            formData.append("TotalAmonut", $scope.AuditorAssign.TotalAmonut);
            formData.append("Membersshareamount", $scope.AuditorAssign.Membersshareamount);
            formData.append("StateGovtAmount", $scope.AuditorAssign.StateGovtAmount);



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



            $http.post(SiteUrl + "/Auditor/AuditorUploadAuditFile", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    if (parseInt(data, 10) > 0) {
                        var Msg = (_language == 'en-us' ? 'Data Saved Successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                    }
                    else if (parseInt(data, 10) == -2) {
                        var Msg = (_language == 'en-us' ? 'Duplicate data.' : 'डुप्लिकेट डेटा।');
                    }
                    else if (parseInt(data, 10) == -1 || parseInt(data, 10) == 0) {
                        var Msg = (_language == 'en-us' ? 'Some Error Occurred Please try After Some Time' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                    }
                    alert(Msg);
                    window.location.reload();
                }).
                error(function (data, status) {
                    alert("Error while auditor assignment insert");
                });
        }
    }
    // END : Save A A New



    $scope.SaveAuditorAssignment = function (IsPermanentSave) {
        message = "";
        var formData = new FormData();
        formData.append("SocietyIdENC", $scope.AuditorAssign.SocietyIdENC);
        formData.append("FinancialYear", $scope.AuditorAssign.FinancialYear);
        if ($scope.AuditorAssign.SocietyIdENC == '0') {
            message += $scope._language == 'en-us' ? '\nPlease Select Society' : '\nकृपया सोसाइटी का चयन करे |';
            message += '<br />';
        }
        else {
            formData.append("SocietyIDEnc", $scope.AuditorAssign.SocietyIdENC);
        }
        if ($scope.AuditorAssign.IsEmpanelledCA == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Select Audit By' : '\nकृपया लेखा परीक्षा का चयन करे |';
            message += '<br />';
        }
        else {
            formData.append("IsEmpanelledCA", $scope.AuditorAssign.IsEmpanelledCA);
        }
        if ($scope.AuditorAssign.IsEmpanelledCA == 'true') {
            if ($scope.AuditorAssign.IsFirm == undefined) {
                message += $scope._language == 'en-us' ? '\n Please Select the Audit Type' : '\nकृपया लेखापरीक्षा प्रकार का चयन करें |';
                message += '<br />';
            }
            else {
                formData.append("IsFirm", $scope.AuditorAssign.IsFirm);
            }
        }
        formData.append("IsEmpanelledCA", $scope.AuditorAssign.IsEmpanelledCA);



        if ($scope.AuditorAssign.IsEmpanelledCA == 'true') {
            if ($scope.AuditorAssign.AuditorId == '0') {
                message += $scope._language == 'en-us' ? '\n Please Select the Auditor for Audit' : '\n कृपया लेखा परीक्षा के लिए ऑडिटर का चयन करें |';
                message += '<br />';
            }
            else {
                formData.append("AuditorId", $scope.AuditorAssign.AuditorId);
            }
        }
        if ($scope.AuditorAssign.DistrictId == undefined || $scope.AuditorAssign.DistrictId == '0') {
            message += $scope._language == 'en-us' ? '\nPlease Select District' : '\nजिला का चयन करें |';
            message += '<br />';
        }
        else {
            formData.append("DistrictId", $scope.AuditorAssign.DistrictId);
        }
        if ($scope.AuditorAssign.IsEmpanelledCA == 'true') {
            if ($scope.AuditorAssign.Auditorfees == '' || $scope.AuditorAssign.Auditorfees == undefined || $scope.AuditorAssign.Auditorfees == '0') {
                message += $scope._language == 'en-us' ? '\n Please fill auditor fees' : '\n कृपया ऑडिट फीस भरें |';
                message += '<br />';
            }
            else {
                formData.append("Auditorfees", $scope.AuditorAssign.Auditorfees);
            }
        }
        if ($scope.AuditorAssign.IsEmpanelledCA == 'true') {
            if ($scope.AuditorAssign.LastFinancialYear == '0') {
                message += $scope._language == 'en-us' ? '\n Please select last audit year' : '\n कृपया पिछले ऑडिट वर्ष का चयन करें |';
                message += '<br />';
            }
            else {
                formData.append("LastFinancialYear", $scope.AuditorAssign.LastFinancialYear);
            }
        }
        if (message.length > 5) {
            ShowModelAlert("Assign Auditor Client", message);
            return false;
        }
        else {
            //alert($scope.AuditorAssign.IsEmpanelledCA);
            $scope.AuditorAssign.SocietyIDEnc = $scope.AuditorAssign.SocietyIdENC;
            $scope.AuditorAssign.IsPermanentSave = IsPermanentSave;
            formData.append("IsPermanentSave", $scope.AuditorAssign.IsPermanentSave);
            $http.post(SiteUrl + "/Society/AuditorAssignmentInsertNew", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
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
                    else if (parseInt(data, 10) == 0) {
                        var Msg = (_language == 'en-us' ? 'Some Error Occurred Please try After Some Time' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                    }



                    //$scope.AssignedAuditorList();



                    alert(Msg);
                    // window.location.reload();



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
                // $scope.AssignedAuditorList();
            }).
            error(function (data, status) {
            });
    };




    $scope.dd = function (id) {
        alert(id);
    }



    $scope.ShowOldYearDataDetailLower = function (FinancialYear, SocietyIdEnc) {



        $scope.AuditorAssign.SocietyIdENC = SocietyIdEnc;
        $scope.ShowOldYearDataDetail(FinancialYear);



        $scope.GetCourtSectionList();
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
            if (!(file[0].name.indexOf(".xlsx") > -1 || file[0].name.indexOf(".xls") > -1)) {
                alert("You have selected invalid Filetype. \n Valid Filetype .xlsx,.xls")
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
        $http.post(SiteUrl + "/Auditor/AuditorList/").
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
                // $scope.divAuditorData = false;
                //}
                //else {
                // $scope.divAuditorData = true;
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
    // $scope.GetSocietyList();
    $scope.GetDistrictList();
    $scope.GetAuditCategoryList();
    $scope.GetCourtSectionList();
    // $scope.AssignedAuditorList();
    $scope.FileUploadTypeList();
    if ($('#lblUserType').html() == 'D') {
        // $scope.DashboardData();
    }
    //END : On PAge List
    $scope.Audit = {};
    $scope.PrintACKNOWLEDGEMENT = function (Audit) {
        //$scope.Audit = audit;



        var htmlPrint = "";
        htmlPrint += '<div style="padding-left:20px;padding-right:20px;padding-top:50px">';
        htmlPrint += '<table cellpadding="5" cellspacing="0" style="width: 100%;">';
        htmlPrint += '<tbody>';
        htmlPrint += '<tr><td style="padding: 8px 35px 0px 15px; text-align: center; height: 20px;"><label style="font-size: 25px;"> Acknowledgement of Society Audit </label><br><br></td></tr>';
        htmlPrint += '<tr>';
        htmlPrint += '<td style="padding: 8px 35px 0px 15px; text-align: center; height: 20px;">';
        htmlPrint += '<table cellpadding="5" cellspacing="0" border="1" style="width:100%;">';
        htmlPrint += '<tbody>';
        htmlPrint += '<tr><th style="text-align:left;padding: 15px;font-size: 15px !important;"> Auditor </th><td style="text-align:left;padding: 15px;font-size: 15px !important;"> ' + Audit.FirmName + ' </td></tr>';
        htmlPrint += '<tr>';
        htmlPrint += '<th style="text-align:left;padding: 15px;font-size: 15px !important;"> EmpanelledNo </th>';
        htmlPrint += '<td style="text-align:left;padding: 15px;font-size: 15px !important;">' + Audit.EmpanelledNo + ' </td>';
        htmlPrint += '</tr>';
        htmlPrint += '<tr><th style="text-align:left;padding: 15px;font-size: 15px !important;"> Society Name </th><td style="text-align:left;padding: 15px;font-size: 15px !important;"> ' + Audit.SocietyName + ' </td></tr>';
        htmlPrint += '<tr><th style="text-align:left;padding: 15px;font-size: 15px !important;"> Society RegistrationNo </th><td style="text-align:left;padding: 15px;font-size: 15px !important;"> ' + Audit.RegistrationNo + ' </td></tr>';
        htmlPrint += '<tr><th style="text-align:left;padding: 15px;font-size: 15px !important;"> Financial Year </th><td style="text-align:left;padding: 15px;font-size: 15px !important;">' + Audit.FIN_YEAR_NAME + ' </td></tr>';
        htmlPrint += '<tr><th style="text-align:left;padding: 15px;font-size: 15px !important;"> Audit Date </th><td style="text-align:left;padding: 15px;font-size: 15px !important;">' + Audit.AuditDate + ' </td></tr>';
        htmlPrint += '</tbody>';
        htmlPrint += '</table>';
        htmlPrint += '</td>';
        htmlPrint += '</tr>';
        htmlPrint += '<tr><td style="padding: 8px 35px 0px 15px; text-align: center; height: 20px;"><label style="font-size: 13px;"> (This is computer generated Acknowledment Receipt) </label><br><br></td></tr>';
        htmlPrint += '</tbody>';
        htmlPrint += '</table>';
        htmlPrint += '</div>';





        var divContents = htmlPrint;
        var a = window.open();
        a.document.write('<html>');
        a.document.write('<body>');
        a.document.write(divContents);
        a.document.write('</body></html>');
        a.print();
        a.close();
    }
});