
var MasterMod = angular.module('AuditorAssignByDepart', ['angularUtils.directives.dirPagination']);
MasterMod.controller("AuditorAssignByDepartCnt", function ($scope, $http) {
    // SCOPE

    $scope.currentPage = 1;
    $scope.currentPageT = 1;
    $scope.pageSize = 10;
    $scope.pageSizeT = 25;
    $scope.AfterMay = true;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.showdiv = false;
    $scope.showdivRedio = false;
    $scope.Inspector = 0;
    $scope.RequestStatus = 1;
    $scope.AuditBy = 'false';
    $scope.ShowDepartmentDataOld = false;
    $scope.ShowEmpanelledCADataOld = true;
    $scope.InspectorName = "";
    $scope.AuditorId = 0;
    $scope.SocietyID = ",";
    $scope.TXN = "";
    $scope.OTP = "";
    $scope.LocalAuditerId = "";
    $scope.LocalAuditBy = "";
    $scope.LoaclIds = "";
    $scope.LaclStatus = "";
    $scope.LaclIsFirm = "";
    $scope.IsRequested = "0";
    $scope.GridNewRequest = true;
    $scope.NewRequestY = true;
    $scope.OldRequestY = false;
    $scope.InspactorNameData = [];
    $scope.RejectAssignedAuditor = [];
    $scope.ForReviewAssignedAuditor = [];
    $scope.CentralCategoryList = [];
    $scope.Divisions = [];
    $scope.selectedInspector = '';
    $scope.IsRao = '';

    $scope.AuditorAssign =  { DivisionId: "0", DistrictId:"0", AuditTillDate:"" };

    $scope.SaveAuditAssignbydeprt = function () {
        var ids = "";
        angular.forEach($scope.GetAllAuditorAssignmentByDepartmentdata, function (item) {
            if (item.IsChecked == true) {
                ids += item.Id + ",";
            }
        });
        if (ids.length > 0) {
            ids = ids.substring(0, ids.length - 1);
        }
        else {
            alert(_language == 'en-us' ? 'Please Select Society\n' : 'कृपया सोसायटी का चयन करें\n');
            return;
        }

        var Status = '';
        var IsFirm = '';
        var AuditBy = '';
        if ($scope.AuditBy != 'true') {

            if (!$scope.AuditorValidation()) {
                alert(_language == 'en-us' ? 'Please Select Auditor \n' : 'कृपया लेखा परीक्षक का चयन करें\n');
                return;
            }
            else {
                Status = $scope.RequestStatus;
                IsFirm = $scope.IsFirm;
                AuditBy = 0;
            }

        }
        else {
            if ($scope.RequestStatus == '0') {
                alert(_language == 'en-us' ? 'Please Select Request Status\n' : 'कृपया वर्तमान स्थिति का चयन करें\n');
                return;
            } else if ($scope.AuditorId == '0') {
                alert(_language == 'en-us' ? 'Please Select Auditor \n' : 'कृपया लेखा परीक्षक का चयन करें\n');
                return;
            } else {
                $scope.LocalAuditerId = $scope.AuditorId;
                Status = $scope.RequestStatus;
                IsFirm = $scope.IsFirm;
            }
            AuditBy = '1';
        }
        $("#myModal").modal("show");
        $scope.LocalAuditBy = AuditBy;
        $scope.LoaclIds = ids;
        $scope.LaclStatus = Status;
        $scope.LaclIsFirm = IsFirm;
        $scope.IsRequested = "1";
        $scope.GenOtpForEsign();

    };

    // on page load

    $scope.ddlDistrct = { DISTRICT_ID: "" }
    $scope.SearchSociety = function () {
        //if ($scope.ddlDistrct.DISTRICT_ID == undefined || $scope.ddlDistrct.DISTRICT_ID == "") {
        //    ShowOkMessage("", "Please select district of your society", function () { });
        //} else {
            $(".se-pre-con").fadeIn("slow");
            $scope.textchange = true;
            $scope.Showgride = false;
            $scope.showotpblock = false;
        $http.post(SiteUrl + "/VerifyCertificate/SearchSociety", { "RegistrationNo": $scope.SocietyRegistration.SocietyRegistrationNo}).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");                   
                    $scope.SocietyRegdata = data.DataList;
                    if (data.DataList.length <= 0) {
                        alert("No Record Found")
                    }
                }).
                error(function (data, status) {
                    $scope.SocietyRegdata = [];
                    $(".se-pre-con").fadeOut("slow");
                });
            //$(".se-pre-con").fadeOut("slow");
       // }
    }
    $scope.Societydata = [];
    $scope.GetSocietyDetails = function (SocietId) {
        $(".se-pre-con").fadeIn("slow");
        $scope.textchange = true;
        $scope.Showgride = false;
        $scope.showotpblock = false;
        $http.post(SiteUrl + "/VerifyCertificate/GetSocietyDetails", { "SocietyId": SocietId }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Societydata = data.DataList;

                $scope.OpenChangeStatus($scope.Societydata[0], 1, 1);

                if (data.DataList.length <= 0) {
                    alert("No Record Found")
                }
            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
                $(".se-pre-con").fadeOut("slow");
            });
    }

    $scope.AuditorValidation = function () {
        $scope.selectedInspector = '';
        var selectedInspector = $("#Inspector option:selected");
        selectedInspector.each(function () {
            $scope.selectedInspector += $(this).val() + ",";
        });

        if ($scope.selectedInspector.length > 0) {
            $scope.selectedInspector = $scope.selectedInspector.substring(0, $scope.selectedInspector.length - 1);
            $scope.LocalAuditerId = $scope.selectedInspector;
            return true;
        }
        else {
            return false;
        }
    }

    $scope.SaveAuditAssignbydeprtNotReqst = function () {
        var AssignedAuditorCollect = '';
        angular.forEach($scope.ForReviewAssignedAuditor, function (item) {
            AssignedAuditorCollect += item.SocietyID + ','
        });

        var ids = "";
        if ($scope.AssignedAuditorCollect != ",") {
            ids = AssignedAuditorCollect.substring(0, $scope.SocietyID.length - 1);
        }
        else {
            alert(_language == 'en-us' ? 'Please Select Society\n' : 'कृपया सोसायटी का चयन करें\n');
            return;
        }

        var AuditerId = '';
        var Status = '';
        var IsFirm = '';
        var AuditBy = '';
        if ($scope.AuditBy == 'All') {
            if ($scope.AuditByInspector == undefined) {
                alert(_language == 'en-us' ? 'Please Select Audit Type \n' : 'कृपया ऑडिट प्रकार चुनें\n');
                return;
            }
            if ($scope.AuditByInspector == false || $scope.AuditByInspector == 'false' || $scope.AuditByInspector == undefined) {
                if (!$scope.AuditorValidation()) {
                    alert(_language == 'en-us' ? 'Please Select Auditor \n' : 'कृपया लेखा परीक्षक का चयन करें\n');
                    return;
                }
            }
            else if ($scope.AuditByInspector == true || $scope.AuditByInspector == 'true' || $scope.AuditByInspector == undefined) {
                if ($scope.AuditorId == 0) {
                    alert(_language == 'en-us' ? 'Please Select Auditor \n' : 'कृपया लेखा परीक्षक का चयन करें\n');
                    return;
                }
            }

            if ($scope.FinancialYear == undefined || $scope.FinancialYear == '0' || $scope.FinancialYear == '') {
                alert(_language == 'en-us' ? 'Please select the financial year \n' : 'कृपया वित्त वर्ष का चयन करे \n');
                return;
            }
            //$scope.FinancialYear < 49
            if ($scope.AuditBy == 'All' && $('#txtAuditTillDate').val() == '') {
                alert(_language == 'en-us' ? 'Enter the date when the audit is to be completed \n' : 'ऑडिट कब तक पूर्ण की जानी है तिथि दर्ज करें \n');
                return;
            }


            if ($scope.AuditByInspector == false || $scope.AuditByInspector == 'false') {
                AuditerId = $scope.LocalAuditerId;
                IsFirm = 2;
                $scope.IsEmpanelledCA = 0;
            }

            if ($scope.AuditByInspector == true || $scope.AuditByInspector == 'true') {
                AuditerId = $scope.AuditorId;
                IsFirm = $scope.IsFirm;
                $scope.IsEmpanelledCA = 1;

            }


        }
        $("#myModal").modal("show");
        $scope.LocalAuditerId = AuditerId;
        $scope.LocalAuditBy = $scope.AuditByInspector == 'true' ? 1 : 0;
        $scope.LoaclIds = ids;
        $scope.LaclStatus = Status;
        $scope.LaclIsFirm = IsFirm;
        $scope.IsRequested = "0";
        $scope.GenOtpForEsign();

    };
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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

        $('#txtAuditTillDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            minDate: '0D'
        });


        $scope.Department = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Department&MasterCode=-1").

            success(function (data, status) {

                $scope.Department = data;
            }).
            error(function (data, status) {
                $scope.Department = [];
            });


        $scope.AuditBy = false;
        $scope.SetAuditType(0);

        $(".se-pre-con").fadeOut("slow");
    };

    // START : Change Status for SMS / delete SMS,Status
    $scope.SetStatus = function (UnitId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Master/ChangeStatusunit", { Id: UnitId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllSubUnitList();

            }).
            error(function (data, status) {
            });
    };

    $scope.GetAllAuditorAssignmentByDepartment = function (IsByEmpanelled) {

        var WhereClauseSet = IsByEmpanelled;
        $scope.showdiv = false;
        $scope.showdivRedio = false;
        $scope.IsByEmpanelledFlag = IsByEmpanelled == 1 ? true : false;


        $scope.GetAllAuditorAssignmentByDepartmentdata = [];
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Society/GetAuditorAssignmentByDepartment?WhereClause=" + WhereClauseSet + "&FYearId=" + $scope.FinancialYear + "&CategoryId=" + $scope.CategoryId + "&DistrictId=" + $scope.Filter.DistrictId + "&DivisionId=" + $scope.Filter.DivisionId + "&Resion=" + $scope.Filter.ResionId).
            success(function (data, status) {
                $scope.GetAllAuditorAssignmentByDepartmentdata = data.DataList;
                $scope.colsort = "";
                $scope.reverseSort = false;
                if (data.length > 0) {
                    if (data[0].AfterMay > 0)
                        $scope.AfterMay = false;
                    else
                        $scope.AfterMay = true;
                }

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.GetAllAuditorAssignmentByDepartmentdata = [];
                alert('Error while fetching Auditor data.');
            });
    };

    $scope.GetAuditorDataForRAO = function () {
        if ($('#ELevelId').val() == 2 && $scope.IsRao == 0) {
            $scope.DivisionsList();
            return false;
        }
        if ($('#ELevelId').val() > 1) {
            $scope.GetInspactorName(0, $scope.IsRao);
            $scope.IsDistrictDisable = true;
            $scope.AuditorAssign.DistrictId = "0";
        }
        else if ($('#ELevelId').val() == 1) {
            $scope.IsDivisionLevelUser = false;
            $scope.AuditorAssign.DivisionId = '0';
            $scope.IsDistrictDisable = true;
            $scope.AuditorAssign.DistrictId = '0';
        }
    };


    $scope.GetAuditorData = function () {
        var IsIndividual = $scope.IsFirm == 1 ? true : false;
        if (($scope.IsRao == "1" || $scope.IsRao == "4") && $scope.AuditByInspector == "false") {
            $scope.GetInspactorName($scope.AuditorAssign.DistrictId, $scope.IsRao);
        }
        else if ($scope.AuditorAssign.DistrictId > 0) {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Society/GetAuditData/", { IsAuditFirm: IsIndividual, "DistrictId": $scope.AuditorAssign.DistrictId }).
                success(function (data1, status) {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.AuditorList = [];
                    $scope.AuditorList = data1;
                }).
                error(function (data1, status) {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.AuditorList = [];
                    alert('Error while fetching Auditor data.');
                });
        }
    };
    $scope.ChangeGridType = function (IsNew, value) {

        if ((IsNew == 1 && value == 1) || (IsNew == 2 && value == 1)) {
            $scope.GridNewRequest = true;
        }
        else {
            $scope.GridNewRequest = false;
            $scope.GetFinancialYear();
            $scope.AuditByT = 'false';
        }

    };

    $scope.RejectAppointment = function () {
        debugger;
        var DData = [];
        DData = $scope.AssignedAuditor.filter(function (x) { return x.IsForceRejected == true });
        if (DData.length > 0) {
            $scope.RejectAssignedAuditor = DData;
            $("#RejectModal").modal("show");
        }
        else {

            alert('Select atleast one Society for Rejection');
        }

    };
    $scope.RemoveRow = function (index) {
        $scope.RejectAssignedAuditor.splice(index, 1);
    };

    $scope.RemoveRowForReviewAssignedAuditor = function (index) {

        $scope.ForReviewAssignedAuditor.splice(index, 1);
    };

    // START : Auditor Assignment List
    $scope.AssignedAuditorList = function (IsByEmpanelled) {
        debugger;
        if (IsByEmpanelled == undefined) {
            if ($scope.AuditByT == 'true') {
                IsByEmpanelled = true;
            }
            else
                IsByEmpanelled = false;
        }

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Society/AuditorAssignmentList", { "Id": 1, "IsEmpanelled": IsByEmpanelled, "FYearId": $scope.ApprovedFinancialYear }).
            success(function (data, status) {               
                $scope.AssignedAuditor = data;
                //$scope.colsort = "";
                //$scope.reverseSort = false;
                //$scope.sortData = function (column) {
                //    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                //    $scope.colsort = column;
                //};
                //$scope.getSortClass = function (column) {
                //    if ($scope.colsort == column) {
                //        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                //    }
                //    return '';
                //};
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $('.se-pre-con').hide();
                $scope.AssignedAuditor = [];
                alert('Error while auditor data list fetch.');
            });

    }
    // END : Auditor Assignment List

    $scope.ShowIns = function (data) {
        if (data == 1) {
            $scope.showdiv = true;
            $scope.AuditorData = false;
            $scope.FirmOrIndividual = true;
        }
        else if (data == 0) {
            $scope.showdiv = false;
            $scope.FirmOrIndividual = true;
            $scope.AuditorData = true;
            $scope.GetDistrictList();
            $scope.showdiv = false;
            $scope.IsDistrictDisable = false;

        }

    }

    $scope.DivisionsList = function () {
        $scope.Divisions = [{}];
        $(".se-pre-con").fadeIn("slow");
        $scope.IsDivisionLevelUser = false;
        $scope.IsDistrictDisable = true;
        $scope.AuditorAssign.DivisionId = "0";

        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
        //    success(function (data, status) {
              
        $scope.Divisions = $scope.FilterDivisions;
                if ($('#ELevelId').val() == 2) {
                    $scope.AuditorAssign.DivisionId = $('#EDivID').val();
                    $scope.AuditorAssign.DistrictId = 0;
                    $scope.IsDivisionLevelUser = true;
                    $http.post(SiteUrl + "/Society/GetInspactorForRAO", { "DistId": 0, "DivisionId": $scope.AuditorAssign.DivisionId, "Office": $scope.IsRao }).
                        success(function (data, status) {
                            $(".se-pre-con").fadeOut("slow");
                            $scope.InspactorNameData = data;
                            $("#Inspector").empty();
                            $('#Inspector').multiselect('destroy');
                            $.each(data, function () {
                                $("#Inspector").append($("<option></option>").val(this.UserId).html(this.OfficerNameEn));
                            });
                            $('#Inspector').multiselect({
                                includeSelectAllOption: true
                            });
                        }).
                        error(function (data, status) {
                            $scope.InspactorNameData = [];
                            alert('Error while auditor data list fetch.');
                        });
                }
            //}).
            //error(function (data, status) {
            //    $scope.Divisions = [];
            //});
    };

    $scope.DistrictDivisionReasion = [];
    $scope.FilterDivisions = [];
    $scope.FilterDistrict = [];
    $scope.SocietyDistrictList = [];
    $scope.GetDistrictDivisionReasion = function () {
        $scope.Divisions = [];
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetDistrictDivisionReasion").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.DistrictDivisionReasion = data.List;
                $scope.DistrictDivisionReasion.forEach(function (itm, index){
                    if ($scope.FilterDivisions.filter(function (x) { return x.DIVISION_ENG == itm.DIVISION_ENG }).length <= 0) {
                        $scope.FilterDivisions.push(itm);
                    }
                    if ($("#ELevelId").val() == "2") {                       
                        if ($("#EDivID").val() == itm.DIVISION_ID) {
                            if ($scope.FilterDistrict.filter(function (x) { return x.DISTRICT_ENG == itm.DISTRICT_ENG }).length <= 0) {
                                $scope.FilterDistrict.push(itm);
                                $scope.SocietyDistrictList.push(itm);
                            }
                        }
                    } else {
                        if ($scope.FilterDistrict.filter(function (x) { return x.DISTRICT_ENG == itm.DISTRICT_ENG }).length <= 0) {
                            $scope.FilterDistrict.push(itm);
                            $scope.SocietyDistrictList.push(itm);
                        }
                    }
                });
                
            }).
            error(function (data, status) {
                $scope.DistrictDivisionReasion = [];
            });
    }

    $scope.BindDistrict = function (id) {  
        $scope.FilterDistrict = [];
        if (id == "0") {
            $scope.DistrictDivisionReasion.forEach(function (itm, index) {   
                if ($scope.FilterDistrict.filter(function (x) { return x.DISTRICT_ENG == itm.DISTRICT_ENG }).length <= 0) {
                    $scope.FilterDistrict.push(itm);
                }
            });
        } else {
            $scope.FilterDistrict = $scope.DistrictDivisionReasion.filter(function (x) { return x.DIVISION_ID == id });

        }
        $scope.Filter.ResionId = "0";
        $scope.Filter.DistrictId = "0";
    }

    $scope.BindResion = function (id) {      
        //$scope.Filter.DistrictId = "0";
        $scope.FilterResion = $scope.DistrictDivisionReasion.filter(function (x) { return x.DISTRICT_ID == id });
        $scope.Filter.ResionId = "0";
    }

    $scope.DivisionsAllList = function () {
        $scope.Divisions = $scope.FilterDivisions;
        //$(".se-pre-con").fadeIn("slow");      
        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
        //    success(function (data, status) {
        //        $(".se-pre-con").fadeOut("slow");
               // $scope.Divisions = data;
                if ($('#ELevelId').val() == 2) {
                    $scope.AuditorAssign.DivisionId = $('#EDivID').val();                  
                }
            //}).
            //error(function (data, status) {
            //    $scope.Divisions = [];
            //});
    };

    $scope.GetDistrictDivisionReasion().then(function () {
        $scope.Divisions = $scope.DistrictDivisionReasion;
        if ($('#ELevelId').val() == 2) {
            $scope.AuditorAssign.DivisionId = $('#EDivID').val();
        }
    });

    //$scope.DivisionsAllList();

   
    $scope.ShowInspacotrDiv = function (ClassData) {
        $scope.SocietyName = ClassData.SocietyName;
        if ($scope.IsRao == "1" && $('#ELevelId').val() < 3) {
            $scope.AuditByInspector = "true";
            $scope.ShowIns(1);
        }
        if (ClassData.Id != "" && ClassData.Id != undefined) {
            $scope.AuditorId = ClassData.AuditorId;
            if (ClassData.IsFirm > 1) {
                $scope.IsFirm = 1;

            }
            else {
                $scope.IsFirm = ClassData.IsFirm;
            }
            if (ClassData.AuditorAssignFilePath.length > 10) {
                $(function () {
                    $("input.setEnable").attr("disabled", true);
                    $("select.setEnable").attr("disabled", true);
                    $("input.setEnable").attr("disabled", "disabled");
                    $("select.setEnable").attr("disabled", "disabled");
                });
            }
            else {
                $("input.setEnable").attr("disabled", "disabled");
                $("select.setEnable").attr("disabled", "disabled");
                $("input.setEnable").attr("disabled", false);
                $("select.setEnable").attr("disabled", false);
            }
            $scope.GetAuditorData();
            $scope.Inspector = ClassData.AuditorId;
            $scope.InspectorName = ClassData.InspName;
            if (!$scope.IsByEmpanelledFlag) {
                if ($scope.showdiv == false) {
                    $scope.showdiv = true;
                    $scope.showdivRedio = false;

                    $scope.FirmOrIndividual = true;
                    $scope.AuditByInspector = "false";



                    $('tbody input').each(function () {
                        if (this.id != ClassData.Id) {
                            $(this).attr('disabled', 'disabled');
                        }

                    });
                }
                else {
                    $('tbody input').each(function () {
                        $(this).removeAttr('disabled');
                    });
                    //$scope.showdiv = false;
                    $scope.showdivRedio = false;
                }
            }
            else {
                if ($scope.showdivRedio == false) {
                    $scope.showdivRedio = true;
                    $scope.showdiv = false;
                    $('tbody input').each(function () {
                        $(this).attr('disabled', 'disabled');
                    });
                }
                else {
                    $scope.showdivRedio = false;
                    $scope.showdiv = false;
                    $('tbody input').each(function () {
                        $(this).removeAttr('disabled');
                    });
                }
            }
        }
        else {
            $scope.showdiv = true;
            if ($scope.SocietyID.indexOf("," + ClassData.SocietyID + ",") > -1) {
                var socId = "," + ClassData.SocietyID + ",";
                var socIdTot = $scope.SocietyID;
                $scope.SocietyID = socIdTot.replace(socId, ",");
            }
            else {
                $scope.SocietyID = $scope.SocietyID + ClassData.SocietyID + ",";
            }
        }
        if ($scope.AuditBy == 'All') {
            $scope.showdiv = false;
        }
        if (ClassData.OnlyAuditor < 1) {
            //$scope.ShowEmpanelledCADataOld = true;

        }
    };

    $scope.GetInspactorName = function (DistId, OfficeID) {

        if ($('#ELevelId').val() == 1 && $scope.IsRao == "2") {
            $scope.IsDivisionLevelUser = true;
            $scope.IsDistrictDisable = true;
            $scope.AuditorAssign.DivisionId = '0';
            $scope.AuditorAssign.DistrictId = '0'
        }
        else {
            if ($('#ELevelId').val() == 1) {
                $scope.IsDivisionLevelUser = false;
                $scope.IsDistrictDisable = false;
            }
        }

        if ($('#ELevelId').val() == 1 && ($scope.IsRao == "3" || $scope.IsRao == "0")) {
            DistId = $('#EDisID').val();
            $http.post(SiteUrl + "/Society/GetInspactorForRAO", { "DistId": DistId, "DivisionId": $scope.AuditorAssign.DivisionId, "Office": $scope.IsRao }).
                success(function (data, status) {
                    $scope.InspactorNameData = data;
                    $("#Inspector").empty();
                    $('#Inspector').multiselect('destroy');
                    $.each(data, function () {
                        $("#Inspector").append($("<option></option>").val(this.UserId).html(this.OfficerNameEn));
                    });
                    $('#Inspector').multiselect({
                        includeSelectAllOption: true
                    });
                }).
                error(function (data, status) {
                    $scope.InspactorNameData = [];
                    alert('Error while auditor data list fetch.');
                });
            return false;
        }

        if ($('#EDisID').val() > 0 && $scope.IsRao == "0") {
            DistId = $('#EDisID').val();
            $http.post(SiteUrl + "/Society/GetInspactorForRAO", { "DivisionId": 0, "Office": $scope.IsRao }).
                success(function (data, status) {
                    $scope.InspactorNameData = data;
                    $("#Inspector").empty();
                    $('#Inspector').multiselect('destroy');
                    $.each(data, function () {
                        $("#Inspector").append($("<option></option>").val(this.UserId).html(this.OfficerNameEn));
                    });
                    $('#Inspector').multiselect({
                        includeSelectAllOption: true
                    });
                }).
                error(function (data, status) {
                    $scope.InspactorNameData = [];
                    alert('Error while auditor data list fetch.');
                });
            return false;
        }

        $http.get(SiteUrl + "/Society/GetInspactorName?DistId=" + DistId + "&OfficeID=" + OfficeID).
            success(function (data, status) {
                $scope.InspactorNameData = data;
                $("#Inspector").empty();
                $('#Inspector').multiselect('destroy');
                $.each(data, function () {
                    $("#Inspector").append($("<option></option>").val(this.UserId).html(this.OfficerNameEn));
                });
                $('#Inspector').multiselect({
                    includeSelectAllOption: true
                });

            }).
            error(function (data, status) {
                $scope.InspactorNameData = [];
                alert('Error while auditor data list fetch.');
            });

    };

    $scope.GenOtpForEsign = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get("../Society/GenerateAadhaarOTPForEsingAuditOrder").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.indexOf('NO#') >= 0) {
                    alert("Due to some issue OTP not send");

                } else {
                    if (data != "LOGOUT") {
                        // alert(data);
                        $scope.TXN = data;
                        alert(_language == 'en-us' ? 'OTP has been sent to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n');
                        // $scope.ShowOTPDiv = true;
                    }
                    else {
                        window.location = 'https://sso.rajasthan.gov.in/signin';
                    }
                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
            });
    };

    $scope.VerifyOTP = function () {
        if ($scope.OTP != '') {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Society/VerifyAadhaarOTPForEsingAuditOrder", { "OTP": $scope.OTP, "TXN": $scope.TXN, "AuditerId": $scope.LocalAuditerId, "AuditBy": $scope.LocalAuditBy, "Ids": $scope.LoaclIds, "Status": $scope.LaclStatus, "IsFirm": $scope.LaclIsFirm, "IsRequested": $scope.IsRequested, "DistrictId": $scope.AuditorAssign.DistrictId, "FinancialYearId": $scope.FinancialYear, "FinancialYearName": $('#ddlFinancialYear option:selected').text().trim(), "AuditTillDate": $('#txtAuditTillDate').val(), "IsForced": $scope.IsForced, "AuditByDepartmentorCA": $scope.AuditByInspector }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data != "Success") {
                        alert(data);
                    }
                    else if (data == "Success") {
                        alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n');
                        location.reload();
                    }
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");

                });
        }
        else {
            alert(_language == 'en-us' ? 'Please enter OTP\n' : 'कृपया OTP चुने|\n');
        }
        $scope.showLoader = false;
    };

    $scope.GetFinancialYear = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.FinancialYearList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;
                $scope.FinancialYearList = $scope.FinancialYearList.filter(function (x) { return x.FIN_YEAR_MASTR_ID < 38 || x.FIN_YEAR_MASTR_ID > 47 });
                $(".se-pre-con").fadeOut("slow");
                $scope.FinancialYearList.shift();
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.GetCentralCategory = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Auditor/GetCentralCategoryListForAudit").
            success(function (data, status) {
                $scope.CentralCategoryList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.CentralCategoryList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GoForDelete = function () {
        $(".se-pre-con").fadeIn("slow");
        if ($scope.DeleteResion == undefined || $scope.DeleteResion == '') {
            $(".se-pre-con").fadeOut("slow");
            alert('Please Enter Delete Reason');

            return false;
        }

        if ($scope.RejectAssignedAuditor.length > 0) {
            var DataCollect = '';
            angular.forEach($scope.RejectAssignedAuditor, function (item) {
                DataCollect += item.Id + ','
            });

            $http.post(SiteUrl + "/Society/RejectAssignAuditorRequest", { Id: DataCollect.substr(0, (DataCollect.length - 1)), DeleteResion: $scope.DeleteResion }).
                success(function (data, status) {

                    $(".se-pre-con").fadeOut("slow");

                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                });
        }

    };

    $scope.GoForReview = function () {

        var ids = "";
        if ($scope.SocietyID != ",") {
            ids = $scope.SocietyID.substring(0, $scope.SocietyID.length - 1);
        }
        else {
            alert(_language == 'en-us' ? 'Please Select Society\n' : 'कृपया सोसायटी का चयन करें\n');

            return;
        }

        $scope.selectedInspector = '';
        var selectedInspector = $("#Inspector option:selected");
        selectedInspector.each(function () {
            $scope.selectedInspector += $(this).val() + ",";
        });

        if ($scope.selectedInspector != "") {
            $scope.selectedInspector = $scope.selectedInspector.substring(0, $scope.selectedInspector.length - 1);
        }

        var S = DData = $scope.GetAllAuditorAssignmentByDepartmentdata.filter(function (x) { return x.IsChecked == true });;
        var I = $scope.selectedInspector.split(',');

        if (S.length > 1 && I.length > 1 && $scope.AuditBy == 'All') {
            alert('Alert : You Can Select Only One Society.while appointing multiple Auditor.');
            return false;
        }

        if ($('#ELevelId').val() > 2 && I.length > 1) {
            alert('Alert : You Dont Have a Rights to Assign Multiple Auditor on a Single Society.');
            return false;
        }



        var AuditerId = '';
        var Status = '';
        var IsFirm = '';
        var AuditBy = '';
        if ($scope.AuditBy == 'All') {
            if ($scope.AuditByInspector == undefined) {
                alert(_language == 'en-us' ? 'Please Select Audit Type \n' : 'कृपया ऑडिट प्रकार चुनें\n');
                return;
            }
            if ($scope.AuditByInspector == false || $scope.AuditByInspector == 'false' || $scope.AuditByInspector == undefined) {
                if (!$scope.AuditorValidation()) {
                    alert(_language == 'en-us' ? 'Please Select Auditor \n' : 'कृपया लेखा परीक्षक का चयन करें\n');
                    return;
                }
            }
            else if ($scope.AuditByInspector == true || $scope.AuditByInspector == 'true' || $scope.AuditByInspector == undefined) {
                if ($scope.AuditorId == 0) {
                    alert(_language == 'en-us' ? 'Please Select Auditor \n' : 'कृपया लेखा परीक्षक का चयन करें\n');
                    return;
                }
            }

            if ($scope.FinancialYear == undefined || $scope.FinancialYear == '0' || $scope.FinancialYear == '') {
                alert(_language == 'en-us' ? 'Please select the financial year \n' : 'कृपया वित्त वर्ष का चयन करे \n');
                return;
            }
            //$scope.FinancialYear < 49
            if ($scope.AuditBy == 'All' && $('#txtAuditTillDate').val() == '') {
                alert(_language == 'en-us' ? 'Enter the date when the audit is to be completed \n' : 'ऑडिट कब तक पूर्ण की जानी है तिथि दर्ज करें \n');
                return;
            }


            if ($scope.AuditByInspector == false || $scope.AuditByInspector == 'false') {
                AuditerId = $scope.LocalAuditerId;
                IsFirm = 2;
                $scope.IsEmpanelledCA = 0;
            }

            if ($scope.AuditByInspector == true || $scope.AuditByInspector == 'true') {
                AuditerId = $scope.AuditorId;
                IsFirm = $scope.IsFirm;
                $scope.IsEmpanelledCA = 1;

            }


        }
        $(".se-pre-con").fadeIn("slow");
        var DData = [];
        DData = $scope.GetAllAuditorAssignmentByDepartmentdata.filter(function (x) { return x.IsChecked == true });
        if (DData.length > 0) {
            $scope.ForReviewAssignedAuditor = DData;
            $("#ReviewAssignedAuditorModal").modal("show");
        }
        else {
            alert('Select atleast one Society for Auditor Appointment');
        }
        $(".se-pre-con").fadeOut("slow");

    };

    $scope.CloseForDelete = function () {

        $("#RejectModal").modal("hide");
        $scope.RejectAssignedAuditor = [];

    };
    $scope.CloseForDelete = function () {

        $("#ReviewAssignedAuditorModal").modal("hide");
        $scope.ForReviewAssignedAuditor = [];
    };

    $scope.GetGridData = function () {
        debugger;
        if ($scope.AuditBy == "false") {
            $scope.SetAuditType(0)
            $scope.GetAllAuditorAssignmentByDepartment(0);
        }
        else if ($scope.AuditBy == "true") {
            $scope.SetAuditType(1)
        }
        else if ($scope.AuditBy == "All") {
            $scope.GetAllAuditorAssignmentByDepartment(2)
        }

    };



    $scope.SetAuditType = function (IsByEmpanelled) {
        $(".se-pre-con").fadeIn("slow");
        $scope.SocietyID = ',';
        //if (IsByEmpanelled == 0)
        //  $scope.GetAllAuditorAssignmentByDepartment(IsByEmpanelled);

        if (IsByEmpanelled == 1) {
            $scope.divAuditType = true;
            //$scope.divDistrict = true;
            $scope.divAuditor = true;
            $scope.divBtn = true;
            $scope.ShowDepartmentDataOld = false;
            $scope.ShowEmpanelledCADataOld = true;

        }
        else {
            $scope.divAuditType = false;
            //$scope.divDistrict = true;
            $scope.divAuditor = false;
            $scope.divBtn = true;
            $scope.ShowDepartmentDataOld = true;
            $scope.ShowEmpanelledCADataOld = false;

        }
        $scope.AuditByInspector = undefined;
        $(".se-pre-con").fadeOut("slow");
        if (IsByEmpanelled == 2) {
            $scope.FYear = true;
            $scope.GetFinancialYear();
        }
        else {
            $scope.FinancialYear = '0'
            $scope.IsBackLogWarning = false;
            $scope.FYear = false;
        }

        if ($scope.CentralCategoryList.length == 0) {
            $scope.GetCentralCategory();
        }

        if ($('#ELevelId').val() < 3) {
            $scope.S_Category = true;
        }
        else {
            $scope.S_Category = false;
        }


    }
    $scope.ShowBackLogWarning = function (IsByEmpanelled) {
        $scope.IsBackLogWarning = $scope.FinancialYear < 1049 ? true : false;
    }



    $scope.SetAuditTypeT = function (IsByEmpanelled) {
        $(".se-pre-con").fadeIn("slow");
        $scope.AssignedAuditorList(IsByEmpanelled);
        if (IsByEmpanelled) {
            $scope.divAuditType = true;
            $scope.divDistrict = true;
            $scope.divAuditor = true;
            $scope.divBtn = true;
        }
        else {
            $scope.divAuditType = false;
            $scope.divDistrict = true;
            $scope.divAuditor = false;
            $scope.divBtn = true;
        }
        $(".se-pre-con").fadeOut("slow");
    }
    // START :Get List of Districts

    $scope.SocietyChangeModel = {
        STATE_ID: '6',
        DIVISION_ID: '0',
        DISTRICT_ID: '0',
        BLOCK_ID: '0'
    }
    $scope.allDisable = false;  
    $scope.OpenChangeStatus = function (item, flag,IsEditable) {
        if (IsEditable == 1) {
            $scope.allDisable = false;
        }
        else {
            $scope.allDisable = true;
        }
        if (flag == 2) {
            $scope.SocietyChangeModel.SocietyID = item.SocietyID
            $scope.SaveDataEdit(true, 2);
        } else {
            $scope.SocietyChangeModel = item;
            if (item.SocietyTypeId != null) {
                $scope.SocietyChangeModel.SocietyTypeId = item.SocietyTypeId.toString();
            }
            else {
                $scope.SocietyChangeModel.SocietyTypeId = 0;
            }
            if (item.ClassId != null) {
                $scope.SocietyChangeModel.ClassId = item.ClassId.toString();
            }
            else {
                $scope.SocietyChangeModel.ClassId = 0;
            }
            if (item.SubClassId != null) {
                $scope.SocietyChangeModel.SubClassId = item.SubClassId.toString();
            }
            else {
                $scope.SocietyChangeModel.SubClassId = 0;
            }

            if (item.CategoryId != null) {
                $scope.SocietyChangeModel.CategoryId = item.CategoryId.toString();
            }
            else {
                $scope.SocietyChangeModel.CategoryId = 0;
            }
            if (item.RuralUrban != null) {
                $scope.SocietyChangeModel.IsRural = !item.RuralUrban == 'Urban';

                //if (item.RuralUrban == 'Urban') {
                //    $("#sdf").prop("checked", true);
                //} else {
                //    $("#sdf").prop("checked", true);
                //}

               

            }
            $scope.SocietyChangeModel.STATE_ID = '6';
            if (item.DIVISION_ID != null) {
                $scope.SocietyChangeModel.DIVISION_ID = item.DIVISION_ID.toString();
            }
            else {
                $scope.SocietyChangeModel.DIVISION_ID = 0;
            }

            if (item.DISTRICT_ID != null) {
                $scope.SocietyChangeModel.DISTRICT_ID = item.DISTRICT_ID.toString();
            }
            else {
                $scope.SocietyChangeModel.DISTRICT_ID = 0;
            }

            $scope.GetSocietyTypeList($scope.SocietyChangeModel.ActId);
            $scope.GetSocietyClass();
            $scope.GetSubClass()
            $scope.GetSocietyCategory();
            $scope.GetDistrictList_change();
            if ($scope.SocietyChangeModel.IsRural == false) {
                $("#sdf").prop('checked', true);
                $scope.UrbanDiv = true;
                $scope.RuralDiv = false;
                $scope.GetCityListByCityTypes(1);
            }
            if ($scope.SocietyChangeModel.IsRural == true) {
                $("#sdfs").prop('checked', true);
                $scope.UrbanDiv = false;
                $scope.RuralDiv = true;
                $scope.BlockListByOffAdd(1);
            }
            $("#mdlChangeStatus").modal("show");
        }
    }

    $scope.allDisable = true;

    $scope.validateUpdateSocietyChangeModel = function () {
        var message = "";

        debugger;
        if ($scope.SocietyChangeModel.RegistrationNo == '' || $scope.SocietyChangeModel.RegistrationNo == undefined) {

            if (message.indexOf($scope._language == 'en-us' ? 'Please choose the complainant.' : 'कृपया सदस्य्ता नंबर दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया सदस्य्ता नंबर दर्ज करें।' : '<br />कृपया सदस्य्ता नंबर दर्ज करें।';
            }
        }

        if ($scope.SocietyChangeModel.SocietyName == '' || $scope.SocietyChangeModel.SocietyName == undefined) {

            if (message.indexOf($scope._language == 'en-us' ? 'Please Select Small Industry Registration' : 'कृपया समाज का नाम दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया समाज का नाम दर्ज करें।' : '<br />कृपया समाज का नाम दर्ज करें।';
            }
        }

        if ($scope.SocietyChangeModel.SocietyTypeId == '' || $scope.SocietyChangeModel.SocietyTypeId == undefined || $scope.SocietyChangeModel.SocietyTypeId == '0') {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया समाज प्रकार नाम चुनें।') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया समाज प्रकार नाम चुनें।.' : '<br />कृपया समाज प्रकार नाम चुनें।.';
            }
        }


        if ($scope.SocietyChangeModel.ClassId == '' || $scope.SocietyChangeModel.ClassId == undefined || $scope.SocietyChangeModel.ClassId == '0') {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया Class का नाम चुनें।') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया Class का नाम चुनें।' : '<br />कृपया Class का नाम चुनें।';
            }
        }

        if ($scope.SocietyChangeModel.SubClassId == '' || $scope.SocietyChangeModel.SubClassId == undefined || $scope.SocietyChangeModel.SubClassId == '0') {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया Sub-Class का नाम चुनें।') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया Sub-Class का नाम चुनें।' : '<br />कृपया Sub-Class का नाम चुनें।';
            }
        }

        if ($scope.SocietyChangeModel.CategoryId == '' || $scope.SocietyChangeModel.CategoryId == undefined || $scope.SocietyChangeModel.CategoryId == '0') {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया वर्ग का नाम चुनें।') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया वर्ग का नाम चुनें।' : '<br />कृपया वर्ग का नाम चुनें।';
            }
        }
        debugger;

        if (document.getElementById('chkchange').checked != false) {
            //if ($scope.SocietyChangeModel.HouseNo == '' || $scope.SocietyChangeModel.HouseNo == undefined) {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया घर का नंबर दर्ज करें।') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />कृपया घर का नंबर दर्ज करें। ' : '<br />कृपया घर का नंबर दर्ज करें।';
            //    }
            //}

            //if ($scope.SocietyChangeModel.LaneStreetName == '' || $scope.SocietyChangeModel.LaneStreetName == undefined) {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया फाइल अपलोड करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />कृपया फाइल  अपलोड करें।.' : '<br />कृपया फाइल  अपलोड करें।.';
            //    }
            //}

            //if ($scope.SocietyChangeModel.Locality == '' || $scope.SocietyChangeModel.Locality == undefined) {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया लेन स्ट्रीट नाम दर्ज करें।') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />कृपया लेन स्ट्रीट नाम दर्ज करें।' : '<br />कृपया लेन स्ट्रीट नाम दर्ज करें।';
            //    }
            //}

            //if ($scope.SocietyChangeModel.STATE_ID == '' || $scope.SocietyChangeModel.STATE_ID == undefined || $scope.SocietyChangeModel.STATE_ID == '0') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया राज्य  का नाम चुनें।') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />कृपया राज्य का नाम चुनें।.' : '<br />कृपया राज्य का नाम चुनें।.';
            //    }
            //}


            //if ($scope.SocietyChangeModel.DIVISION_ID == '' || $scope.SocietyChangeModel.DIVISION_ID == undefined || $scope.SocietyChangeModel.DIVISION_ID == '0') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया संभाग  का नाम चुनें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />कृपया संभाग दर्ज का नाम चुनें।.' : '<br />कृपया संभाग का नाम चुनें।.';
            //    }
            //}
            if ($scope.SocietyChangeModel.DISTRICT_ID == '' || $scope.SocietyChangeModel.DISTRICT_ID == undefined || $scope.SocietyChangeModel.DISTRICT_ID == '0') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया जिला  का नाम चुनें।.') < 0) {
                    message += $scope._language == 'en-us' ? '<br />कृपया जिला का नाम चुनें।.' : '<br />कृपया जिला का नाम चुनें।.';
                }
            }


            //if ($scope.SocietyChangeModel.IsRural == false) {

            //    if ($scope.SocietyChangeModel.BLOCK_ID == '' || $scope.SocietyChangeModel.BLOCK_ID == undefined || $scope.SocietyChangeModel.BLOCK_ID == '0') {
            //        if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया Block  का  नाम चुनें।') < 0) {
            //            message += $scope._language == 'en-us' ? '<br />कृपया Block  का  नाम चुनें।.' : '<br />कृपया Block  का नाम चुनें।.';
            //        }
            //    }
            //    if ($scope.SocietyChangeModel.GP_ID == '' || $scope.SocietyChangeModel.GP_ID == undefined || $scope.SocietyChangeModel.GP_ID == '0') {
            //        if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया  ग्राम पंचायत का  नाम चुनें।') < 0) {
            //            message += $scope._language == 'en-us' ? '<br />कृपया  ग्राम पंचायत का  नाम चुनें।.' : '<br />कृपया  ग्राम पंचायत का नाम चुनें।.';
            //        }
            //    }
            //    if ($scope.SocietyChangeModel.VILLAGE_ID == '' || $scope.SocietyChangeModel.VILLAGE_ID == undefined || $scope.SocietyChangeModel.VILLAGE_ID == '0') {
            //        if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया गाँव का नाम चुनें।') < 0) {
            //            message += $scope._language == 'en-us' ? '<br /> कृपया गाँव का नाम चुनें।.' : '<br />कृपया गाँव का  नाम चुनें।.';
            //        }
            //    }
            //    $scope.SocietyChangeModel.CITY_ID = 0;
            //    $scope.SocietyChangeModel.WARD_ID = 0;

            //}
            //if ($scope.SocietyChangeModel.IsRural == true) {

            //    if ($scope.SocietyChangeModel.CITY_ID == '' || $scope.SocietyChangeModel.CITY_ID == undefined || $scope.SocietyChangeModel.CITY_ID == '0') {
            //        if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया शहर का नाम चुनें।') < 0) {
            //            message += $scope._language == 'en-us' ? '<br />कृपया शहर का नाम चुनें।.' : '<br />कृपया शहर का नाम चुनें।.';
            //        }
            //    }
            //    if ($scope.SocietyChangeModel.WARD_ID == '' || $scope.SocietyChangeModel.WARD_ID == undefined || $scope.SocietyChangeModel.WARD_ID == '0') {
            //        if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया मुहल्ला का नाम चुनें।') < 0) {
            //            message += $scope._language == 'en-us' ? '<br /> कृपया मुहल्ला का नाम चुनें।.' : '<br />कृपया मुहल्ला का नाम चुनें।.';
            //        }
            //    }

            //    $scope.SocietyChangeModel.BLOCK_ID = 0;
            //    $scope.SocietyChangeModel.GP_ID = 0;
            //    $scope.SocietyChangeModel.VILLAGE_ID = 0;

            //}


            //if ($scope.SocietyChangeModel.PinCode == '' || $scope.SocietyChangeModel.PinCode == undefined) {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया Pincode दर्ज करें।.') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />कृपया Pincode दर्ज करें।.' : '<br />कृपया Pincode दर्ज करें।.';
            //    }
            //}

        }

        if (message.length > 29) {
            ShowOkMessage("निम्नलिखित मान्य करें", message, function () { });
            return false;
        }
        return true;

    }





    //SaveDataEdit

    $scope.SaveDataEdit = function (SubmitStatus, Falg) {
        debugger;
        $scope.SubmitStatus = SubmitStatus;
        var formData = new FormData();
        formData.append("Request_Id", Falg);
        if (Falg == 2) {
            ShowAlertMessage("Alert", "Are you sure you want to delete this society?", function () {
                formData.append("SocietyID", $scope.SocietyChangeModel.SocietyID);

                $(".se-pre-con").fadeIn("slow");
                $http.post(SiteUrl + "/Society/SaveSocietyProfilingEdit", formData,
                    {
                        withCredentials: true,
                        headers: { 'Content-Type': undefined },
                        transformRequest: angular.identity
                    }).
                    success(function (data, status) {
                        debugger;
                        $(".se-pre-con").fadeOut("slow");
                        if (data.data == '3') {
                            $(".se-pre-con").fadeOut("slow");
                            ShowModelAlert("Society Profiling", "Society successfully Deleted.");
                            if ($scope.GridNewRequest == false) {
                                $scope.AssignedAuditorList();
                            }
                            else {
                                $scope.GetAllAuditorAssignmentByDepartment(0);
                            }
                        }
                        else {
                            ShowModelAlert("Society Profiling", "Error While Society deleting");
                        }

                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        ShowModelAlert("Society Profiling", "Error While Society Profiling Insert");
                        $('#btnAdministrativedetails').trigger('click');
                    });
            });
        }
        else {
            var RegistrationDate = '';
            if ($scope.validateUpdateSocietyChangeModel()) {

                $(".se-pre-con").fadeIn("slow");
                //alert($("#RegistrationDate").val());
                $scope.SocietyChangeModel.RegistrationDate = $("#RegistrationDate").val();
                //alert($scope.SocietyChangeModel.RegistrationDate);

                $scope.SocietyChangeModel.RegistrationDate = $scope.SocietyChangeModel.RegistrationDate.replace(/-/g, '/');
                RegistrationDate = $scope.SocietyChangeModel.RegistrationDate.split('/')[2] + "-" + $scope.SocietyChangeModel.RegistrationDate.split('/')[1] + "-" + $scope.SocietyChangeModel.RegistrationDate.split('/')[0];
                //alert(RegistrationDate);
                if (document.getElementById('chkchange').checked == false) {
                    formData.append("RegistrationNo", $scope.SocietyChangeModel.RegistrationNo);//1
                    formData.append("SocietyName", $scope.SocietyChangeModel.SocietyName);//2
                    formData.append("SocietyTypeId", $scope.SocietyChangeModel.SocietyTypeId);//3
                    formData.append("ClassId", $scope.SocietyChangeModel.ClassId);
                    formData.append("SubClassId", $scope.SocietyChangeModel.SubClassId);
                    formData.append("SocietyCategoryId", $scope.SocietyChangeModel.CategoryId);
                    formData.append("SocietyID", $scope.SocietyChangeModel.SocietyID);
                    formData.append("RegistrationDate", RegistrationDate);//6
                    formData.append("ChangeStatus", 0);//6
                }
                else {


                    // formData.append("ActId", $scope.SocietyChangeModel.ActId);
                    formData.append("RegistrationNo", $scope.SocietyChangeModel.RegistrationNo);//1
                    formData.append("SocietyName", $scope.SocietyChangeModel.SocietyName);//2
                    formData.append("SocietyTypeId", $scope.SocietyChangeModel.SocietyTypeId);//3
                    formData.append("ClassId", $scope.SocietyChangeModel.ClassId);
                    formData.append("SubClassId", $scope.SocietyChangeModel.SubClassId);
                    formData.append("SocietyCategoryId", $scope.SocietyChangeModel.CategoryId);//6
                    formData.append("IsRural", true);// $scope.SocietyChangeModel.IsRural);
                    formData.append("HouseBuildingNo", "");//$scope.SocietyChangeModel.HouseNo);//8
                    formData.append("LaneStreetName", "");//$scope.SocietyChangeModel.LaneStreetName);
                    formData.append("Locality", "");//$scope.SocietyChangeModel.Locality);
                    formData.append("STATE_ID", "0");//$scope.SocietyChangeModel.STATE_ID);//11
                    formData.append("DIVISION_ID", "0");// $scope.SocietyChangeModel.DIVISION_ID);
                    formData.append("DISTRICT_ID", $scope.SocietyChangeModel.DISTRICT_ID);//13
                    formData.append("CITY_ID", "0")//$scope.SocietyChangeModel.CITY_ID);
                    formData.append("BLOCK_ID", "0");//$scope.SocietyChangeModel.BLOCK_ID);
                    formData.append("WARD_ID", "0")//$scope.SocietyChangeModel.WARD_ID);
                    formData.append("GP_ID", "0");//$scope.SocietyChangeModel.GP_ID);
                    formData.append("BLOCK_ID", "0");//$scope.SocietyChangeModel.BLOCK_ID);
                    formData.append("VILLAGE_ID", "0");//$scope.SocietyChangeModel.VILLAGE_ID);
                    formData.append("SocietyID", $scope.SocietyChangeModel.SocietyID);
                    formData.append("RegistrationDate", RegistrationDate);
                    formData.append("ChangeStatus", 1);//6
                }
                $http.post(SiteUrl + "/Society/SaveSocietyProfilingEdit", formData,
                    {
                        withCredentials: true,
                        headers: { 'Content-Type': undefined },
                        transformRequest: angular.identity
                    }).
                    success(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        if (data.data == '-1') {
                            ShowModelAlert("Error", "Society Name already exists  Please enter another Society name");
                        }
                        else if (data.data == '-2') {
                            ShowModelAlert("Error", "RegistrationNo already exists  Please enter another RegistrationNo");
                        }
                        else {
                            $("#mdlChangeStatus").modal("hide");
                            ShowModelAlert("Society Profiling", "Basic Details successfully submited");
                        }

                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        ShowModelAlert("Society Profiling", "Error While Society Profiling Insert");
                        $('#btnAdministrativedetails').trigger('click');
                    });
            }
        }

    }


    $scope.RuralUrbanBlock_change = function (V) {
        debugger
        var SelectedVal = $scope.SocietyChangeModel.IsRural;
        if (V != 1) {
            $scope.SocietyChangeModel.CityTypes = "0";
            $scope.SocietyChangeModel.CITY_ID = "0";
            $scope.SocietyChangeModel.WARD_ID = "0";
            $scope.SocietyChangeModel.BLOCK_ID = "0";
            $scope.SocietyChangeModel.GP_ID = "0";
            $scope.SocietyChangeModel.VILLAGE_ID = "0";
        }
        if (SelectedVal == 'True' || SelectedVal == 'true') {
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;

            $scope.GetCityListByCityTypes(1);
        }
        else if (SelectedVal == 'false' || SelectedVal == 'False') {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
            $scope.BlockListByOffAdd(1)
        }
        else {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
        }
    };

    $scope.BlockListByOffAdd = function (id) {
        debugger;
        $scope.Blocksdata = [];
        if ($scope.SocietyChangeModel.DISTRICT_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyChangeModel.DISTRICT_ID).
                success(function (data, status) {
                    $scope.Blocksdata = data;
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                });
        }
    };

    // START : Get List of GramPanchayat
    $scope.GramPanchayatList = function (id) {

        debugger;
        if ($scope.SocietyChangeModel.BLOCK_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyChangeModel.BLOCK_ID).
                success(function (data, status) {

                    $scope.GramPanchayats = data;
                }).
                error(function (data, status) {
                    $scope.GramPanchayats = [];

                });
        }


    };
    // END : Get List of GramPanchayat



    // START : Get List of Villages
    $scope.VillageList = function (id) {
        debugger;
        if ($scope.SocietyChangeModel.GP_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyChangeModel.GP_ID).
                success(function (data, status) {
                    $scope.Villages = data;
                }).
                error(function (data, status) {
                    $scope.Villages = [];
                });
        }
    };
    // END : Get List of Villages





    $scope.GetCityListByCityTypes = function (id) {
        if ($scope.SocietyChangeModel.DISTRICT_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode=" + $scope.SocietyChangeModel.DISTRICT_ID).
                success(function (data, status) {
                    $scope.CityListdata = data;
                }).
                error(function (data, status) {
                    $scope.CityListdata = [];
                });
        }
        // $scope.RuralUrbanBlock();
    };
    //END : Get City List

    // START : Get Ward List
    $scope.GetWardList = function (id) {
        if ($scope.SocietyChangeModel.CITY_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.SocietyRegisteredOffice.CITY_ID).
                success(function (data, status) {
                    $scope.Ward = data;
                }).
                error(function (data, status) {
                    $scope.Ward = [];
                });
        }
    };



    $scope.changeaddress = function () {
        debugger;
        if (document.getElementById('chkchange').checked == false) {

            $scope.allDisable = true;
        }
        else {
            $scope.allDisable = false;
        }

    }





    $scope.GetRAODistrictList = function () {
        if (($scope.IsRao == "1" || $scope.IsRao == "4") && $('#EDisID').val() > 0) {
            $scope.GetInspactorName($('#EDisID').val(), $scope.IsRao);
            return;
        }
        $scope.GetDistrictList();
        $scope.IsDistrictDisable = false;
        $scope.IsDivisionLevelUser = true;
        if ($('#ELevelId').val() == 1) {
            $scope.AuditorAssign.DivisionId = '0';
        }
    }

    $scope.GetDistrictList_change = function () {
        $scope.DistrictList = [];


        $scope.DistrictDivisionReasion.filter(function (x) { x.DIVISION_ID == $scope.SocietyChangeModel.DIVISION_ID }).forEach(function (itm, index) { 
            if ($scope.DistrictList.filter(function (x) { return x.DISTRICT_ENG == itm.DISTRICT_ENG }).length <= 0) {
                $scope.DistrictList.push(itm);
            }
        });
        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyChangeModel.DIVISION_ID).
        //    success(function (data, status) {
        //        $scope.DistrictList = data;
        //    }).
        //    error(function (data, status) {
        //        $scope.DistrictList = [];
        //        alert('Error while fetching district list data.');
        //    });
    };


    $scope.GetDistrictList = function () {
        $scope.DistrictList = [];
        if ($scope.IsRao == "1" || $scope.IsRao == "4") {
            $scope.DistrictDivisionReasion.filter(function (x) { x.DIVISION_ID == $('#EDivID').val() }).forEach(function (itm, index) {
                if ($scope.DistrictList.filter(function (x) { return x.DISTRICT_ENG == itm.DISTRICT_ENG }).length <= 0) {
                    $scope.DistrictList.push(itm);
                }
            });
        } else {
            $scope.DistrictDivisionReasion.forEach(function (itm, index) {
                if ($scope.DistrictList.filter(function (x) { return x.DISTRICT_ENG == itm.DISTRICT_ENG }).length <= 0) {
                    $scope.DistrictList.push(itm);
                }
            });
        }

        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + (($scope.IsRao == "1" || $scope.IsRao == "4") ? $('#EDivID').val() : "-1")).
        //    success(function (data, status) {
        //        $scope.DistrictList = data;
        //    }).
        //    error(function (data, status) {
        //        $scope.DistrictList = [];
        //        alert('Error while fetching district list data.');
        //    });
    };

    $scope.DivistionDistrictList = [];
 
    $scope.GetDistrictByDivision = function (DivisionId) {


        $scope.DistrictDivisionReasion.filter(function (x) { x.DIVISION_ID == DivisionId }).forEach(function (itm, index) {
            if ($scope.DivistionDistrictList.filter(function (x) { return x.DISTRICT_ENG == itm.DISTRICT_ENG }).length <= 0) {
                $scope.DivistionDistrictList.push(itm);
            }
        });

        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" +  DivisionId).
        //    success(function (data, status) {
        //        $scope.DivistionDistrictList = data;
        //    }).
        //    error(function (data, status) {
        //        $scope.DivistionDistrictList = [];
        //        //alert('Error while fetching district list data.');
        //    });
    };
    $scope.GetDistrictByDivision(0);
    // END :Get List of Districts
    $scope.OnPageLoad();
    //$scope.GetAllAuditorAssignmentByDepartment(0);
    $scope.AssignedAuditorList();
    $scope.SocietyChangeModel = {};






    //START : Get State List
    $scope.Statelist = [];
    $scope.Statelist.push(
        {
            "STATE_ID": 6,
            "STATE_ENG": 'Rajasthan',
            "STATE_MANGAL": 'राजस्थान'

        });

    //END : Get State List

    //START : Get Division List
    $scope.SocietyDivisions = [];
    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
        success(function (data, status) {
            $scope.SocietyDivisions = data;
        }).
        error(function (data, status) {
            $scope.SocietyDivisions = [];
        });
    //END

    // START : Set Rural/ Urban div show/hide
    $scope.RuralUrbanBlock = function (V) {
        debugger
        var SelectedVal = $scope.SocietyRegisteredOffice.IsRural;
        if (V != 1) {
            $scope.SocietyRegisteredOffice.CityTypes = "0";
            $scope.SocietyRegisteredOffice.CITY_ID = "0";
            $scope.SocietyRegisteredOffice.WARD_ID = "0";
            $scope.SocietyRegisteredOffice.BLOCK_ID = "0";
            $scope.SocietyRegisteredOffice.GP_ID = "0";
            $scope.SocietyRegisteredOffice.VILLAGE_ID = "0";
        }
        if (SelectedVal == 'True' || SelectedVal == 'true') {
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;
            $scope.GetCityListByCityTypes(1);
        }
        else if (SelectedVal == 'false' || SelectedVal == 'False') {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
            $scope.BlockListByOffAdd(1)
        }
        else {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
        }
    };

    $scope.RuralUrbanBlockCopy = function () {
        var SelectedVal = $scope.SocietyRegisteredOffice.IsRuralcopy;
        $scope.SocietyRegisteredOffice.CityTypes_copy = "0";
        $scope.SocietyRegisteredOffice.CITY_IDcopy = "0";
        $scope.SocietyRegisteredOffice.WARD_IDcopy = "0";
        $scope.SocietyRegisteredOffice.BLOCK_ID_copy = "0";
        $scope.SocietyRegisteredOffice.GP_IDcopy = "0";
        $scope.SocietyRegisteredOffice.VILLAGE_IDcopy = "0";
        if (SelectedVal == 'true' || SelectedVal == 'True') {
            $scope.UrbanDivCopy = true;
            $scope.RuralDivcopy = false;
            //$scope.GetCityListByCityTypes(2);
        }
        else if (SelectedVal == 'false' || SelectedVal == 'False') {
            $scope.UrbanDivCopy = false;
            $scope.RuralDivcopy = true;
            //$scope.BlockListByOffAdd(2)
        }
        else {
            $scope.UrbanDivCopy = false;
            $scope.RuralDivcopy = false;
        }
    };

    // START : Set Rural/ Urban div show/hide
    $scope.RuralUrbanBlockForSport = function () {

        var SelectedVal = $scope.SocietyRegisteredOffice.IsRuralAddSports;
        $scope.SocietyRegisteredOffice.CityTypes_copy = "0";
        $scope.SocietyRegisteredOffice.CITY_IDcopy = "0";
        $scope.SocietyRegisteredOffice.WARD_IDcopy = "0";
        $scope.SocietyRegisteredOffice.BLOCK_ID_copy = "0";
        $scope.SocietyRegisteredOffice.GP_IDcopy = "0";
        $scope.SocietyRegisteredOffice.VILLAGE_IDcopy = "0";
        if (SelectedVal == 'U') {
            $scope.UrbanDivSport = true;
            $scope.RuralDivSport = false;
        }
        else if (SelectedVal == 'R') {
            $scope.UrbanDivSport = false;
            $scope.RuralDivSport = true;
        }
        else {
            $scope.UrbanDivSport = false;
            $scope.RuralDivSport = false;

        }
    };

    $scope.RuralUrbanBlockForSportcopy = function (IsEdit) {

        var SelectedVal = $scope.SocietyRegisteredOffice.IsRuralSecretary;
        if (IsEdit != '1') {
            $scope.SocietyRegisteredOffice.CityTypesSecretary = "0";
            $scope.SocietyRegisteredOffice.CITY_IDSecretary = "0";
            $scope.SocietyRegisteredOffice.WARD_IDSecretary = "0";
            $scope.SocietyRegisteredOffice.BLOCK_IDSecretary = "0";
            $scope.SocietyRegisteredOffice.GP_IDSecretary = "0";
            $scope.SocietyRegisteredOffice.VILLAGE_IDSecretary = "0";
        }
        if (SelectedVal == 'true' || SelectedVal == 'True') {
            $scope.UrbanDivSportCopy = true;
            $scope.RuralDivSportcopy = false;
        }
        else if (SelectedVal == 'false' || SelectedVal == 'False') {
            $scope.UrbanDivSportCopy = false;
            $scope.RuralDivSportcopy = true;
        }
        else {
            $scope.UrbanDivSportCopy = false;
            $scope.RuralDivSportcopy = false;
        }
    };







    //**Society Type* and Society Class* and Society Sub Class* Society Category**//
    // ******Partial Code-------Start------*****

    //start Level of office data
    //$scope.Levelofficedata = function () {
    //    $scope.GetSocietyClass($scope.SocietyChangeModel.SocietyTypeId);
    //};
    //End Level of office data

    $scope.GetSocietyTypeList = function (actid) {

        $scope.SocietyChangeModel.ActId = actid;
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=" + $scope.SocietyChangeModel.ActId).
            success(function (data, status) {
                $scope.SocietyTypeList = [];
                data.forEach(function (item) {
                    if (item.SocietyTypeId == 7 || item.SocietyTypeId == 8 || item.SocietyTypeId == 9) {
                        $scope.SocietyTypeList.push(item);
                    }
                });
               // $scope.SocietyTypeList = data;
            }).
            error(function (data, status) {
                $scope.SocietyTypeList = [];
            });
    };

    // GET Society Class
    $scope.GetSocietyClass = function () {
        // $scope.SocietyChangeModel.SocietyTypeId = id;
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYCLASS&MasterCode=" + $scope.SocietyChangeModel.SocietyTypeId).
            success(function (data, status) {
                $scope.SocietyClassList = data;
            }).
            error(function (data, status) {
                $scope.SocietyClassList = [];
            });

    }
    //END

    // START : Get SocietySubClassList
    $scope.GetSubClass = function () {

        $http.get(SiteUrl + "/Master/SubClassList/?ClassId=" + $scope.SocietyChangeModel.ClassId + "&SocietyTypeId=").// + $scope.SocietyChangeModel.SocietyTypeId).
            success(function (data, status) {
                $scope.SocietySubClassList = data;
            }).
            error(function (data, status) {
                $scope.SocietySubClassList = [];
            });
        $scope.SocietyChangeModel.SocietyCategoryId = '0';
    };
    // END

    // START : Get SocietyCategoryList
    $scope.GetSocietyCategory = function () {

        $http.get(SiteUrl + "/Master/SocietyCategoryList/?ClassId=" + $scope.SocietyChangeModel.ClassId + "&SubClassId=" + $scope.SocietyChangeModel.SubClassId).
            success(function (data, status) {
                $scope.SocietyCategoryList = data;
            }).
            error(function (data, status) {
                $scope.SocietyCategoryList = [];
            });
    };
    // END : Get Sub Class


    // ******Partial Code-------End------*****

});





