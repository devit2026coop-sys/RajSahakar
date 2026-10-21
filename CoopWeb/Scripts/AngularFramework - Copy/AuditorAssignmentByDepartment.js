
var MasterMod = angular.module('AuditorAssignByDepart', ['angularUtils.directives.dirPagination']);
MasterMod.controller("AuditorAssignByDepartCnt", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.currentPageT = 1;
    $scope.pageSize = 12;
    $scope.pageSizeT = 10;

    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.showdiv = false;
    $scope.showdivRedio = false;
    $scope.Inspector = 0;
    $scope.RequestStatus = 1;
    $scope.AuditBy = 'true';
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
        var AuditerId = '';
        var Status = '';
        var IsFirm = '';
        var AuditBy = '';
        if ($scope.AuditBy != 'true') {
            if ($scope.Inspector == 0) {
                alert(_language == 'en-us' ? 'Please Select Inspector\n' : 'कृपया इंस्पेक्टर का चयन करें\n');
                return;
            } else {
                AuditerId = $scope.Inspector;
            }
            AuditBy = '0';
        }
        else {
            if ($scope.RequestStatus == '0') {
                alert(_language == 'en-us' ? 'Please Select Request Status\n' : 'कृपया वर्तमान स्थिति का चयन करें\n');
                return;
            } else if ($scope.AuditorId == '0') {
                alert(_language == 'en-us' ? 'Please Select Auditor \n' : 'कृपया लेखा परीक्षक का चयन करें\n');
                return;
            } else {
                AuditerId = $scope.AuditorId;
                Status = $scope.RequestStatus;
                IsFirm = $scope.IsFirm;
            }
            AuditBy = '1';
        }
        $("#myModal").modal("show");
        $scope.LocalAuditerId = AuditerId;
        $scope.LocalAuditBy =   AuditBy;
        $scope.LoaclIds =       ids;
        $scope.LaclStatus =     Status;
        $scope.LaclIsFirm = IsFirm;
        $scope.IsRequested = "1";
        $scope.GenOtpForEsign();
        //$http.post(SiteUrl + "/Society/saveAuditAssignbydeprt", { "AuditerId": AuditerId, "AuditBy":AuditBy, "Ids": ids, "Status": Status, "IsFirm": IsFirm }).
        //    success(function (data, status) {
        //        if (data == 1) {
        //            alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n');
        //        }
        //        else {
        //            alert(_language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n');
        //        }

        //        window.location = SiteUrl+"/Society/AuditorAssignmentByDepartment";

        //    }).
        //    error(function (data, status) {
        //        alert("Error while subclass insert");
        //    });
    };

    // on page load

    $scope.SaveAuditAssignbydeprtNotReqst = function () {
        var ids = "";
        if ($scope.SocietyID != ",") {
            ids = $scope.SocietyID.substring(0, $scope.SocietyID.length - 1);
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
            if ($scope.Inspector == 0) {
                alert(_language == 'en-us' ? 'Please Select Inspector\n' : 'कृपया इंस्पेक्टर का चयन करें\n');
                return;
            } else {
                AuditerId = $scope.Inspector;
            }
            AuditBy = '0';
        }
        $("#myModal").modal("show");
        $scope.LocalAuditerId = AuditerId;
        $scope.LocalAuditBy = AuditBy;
        $scope.LoaclIds = ids;
        $scope.LaclStatus = Status;
        $scope.LaclIsFirm = IsFirm;
        $scope.IsRequested = "0";
         $scope.GenOtpForEsign();
       // alert(ids);
        //$http.post(SiteUrl + "/Society/saveAuditAssignbydeprtNotRequest", { "AuditerId": AuditerId, "AuditBy": AuditBy, "Ids": ids, "Status": Status, "IsFirm": IsFirm }).
        //    success(function (data, status) {
        //        if (data == 1) {
        //            alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n');
        //        }
        //        else {
        //            alert(_language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n');
        //        }

        //        window.location = SiteUrl + "/Society/AuditorAssignmentByDepartment";

        //    }).
        //    error(function (data, status) {
            //    alert("Error while subclass insert");
            //});
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

            {

            };

        }



        $scope.Department = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Department&MasterCode=-1").

            success(function (data, status) {

                $scope.Department = data;
            }).
            error(function (data, status) {
                $scope.Department = [];
            });
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
        var WhereClauseSet = '';
        $scope.showdiv = false;
        $scope.showdivRedio = false;
        if (IsByEmpanelled==1) {
            WhereClauseSet = '1 ';
            $scope.IsByEmpanelledFlag = true;
        }
        else if(IsByEmpanelled==0) {
            WhereClauseSet = '0';
            $scope.IsByEmpanelledFlag = false;
        }
        else if(IsByEmpanelled == 2) {
            WhereClauseSet = '2 ';
            $scope.IsByEmpanelledFlag = false;
        }
        $scope.GetAllAuditorAssignmentByDepartmentdata = [];
        $http.get(SiteUrl + "/Society/GetAuditorAssignmentByDepartment?WhereClause=" + WhereClauseSet).
            success(function (data, status) {

                $scope.GetAllAuditorAssignmentByDepartmentdata = data

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
            error(function (data1, status) {

            });
    }
    $scope.GetAuditorData = function () {
        var IsIndividual = $scope.IsFirm == 1 ? true : false;
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
    // START : Auditor Assignment List
    $scope.AssignedAuditorList = function (IsByEmpanelled) {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Society/AuditorAssignmentList", { "Id": 1, "IsEmpanelled": IsByEmpanelled}).
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
            }).
            error(function (data, status) {
                $scope.AssignedAuditor = [];
                alert('Error while auditor data list fetch.');
            });
        $(".se-pre-con").fadeOut("slow");
    }
    // END : Auditor Assignment List
    $scope.ShowInspacotrDiv = function (id, InspName, AuditorId, IsFirm, AuditorAssignFilePath, SocietyID) {
        if (id != "") {
            $scope.AuditorId = AuditorId;
            if (IsFirm > 1) {
                $scope.IsFirm = 1;
            }
            else {
                $scope.IsFirm = IsFirm;
            }
            if (AuditorAssignFilePath.length > 10) {
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
            $scope.Inspector = AuditorId;
            $scope.InspectorName = InspName;
            if (!$scope.IsByEmpanelledFlag) {
                if ($scope.showdiv == false) {
                    $scope.showdiv = true;
                    $scope.showdivRedio = false;
                    $('tbody input').each(function () {
                        if (this.id != id)
                            $(this).attr('disabled', 'disabled');
                    });
                }
                else {
                    $('tbody input').each(function () {
                        $(this).removeAttr('disabled');
                    });
                    $scope.showdiv = false;
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
            if ($scope.SocietyID.indexOf("," + SocietyID + ",")>-1)
            {
                var socId = "," + SocietyID + ",";
                var socIdTot = $scope.SocietyID;
                $scope.SocietyID = socIdTot.replace(socId, ",");
            }
            else{
                $scope.SocietyID = $scope.SocietyID + SocietyID + ",";
            }
        }
    };

    $scope.GetInspactorName = function (DistId) {
        $http.get(SiteUrl + "/Society/GetInspactorName?DistId=" + DistId).
            success(function (data, status) {
                $scope.InspactorNameData = data;

                if (data != null && data.length == 0) {

                }
                else {

                }

            }).
            error(function (data, status) {
                $scope.AssignedAuditor = [];
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
                    location.reload();
                    //window.location = '../ApplicationFlow/ApplicationFlow';
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
            $http.post(SiteUrl + "/Society/VerifyAadhaarOTPForEsingAuditOrder", { "OTP": $scope.OTP, "TXN": $scope.TXN, "AuditerId": $scope.LocalAuditerId, "AuditBy": $scope.LocalAuditBy, "Ids": $scope.LoaclIds, "Status": $scope.LaclStatus, "IsFirm": $scope.LaclIsFirm, "IsRequested": $scope.IsRequested }).
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
    $scope.SetAuditType = function (IsByEmpanelled) {
        $(".se-pre-con").fadeIn("slow");
        $scope.SocietyID = '';
        $scope.GetAllAuditorAssignmentByDepartment(IsByEmpanelled);
        if (IsByEmpanelled==1) {
            $scope.divAuditType = true;
            $scope.divDistrict = true;
            $scope.divAuditor = true;
            $scope.divBtn = true;
            $scope.ShowDepartmentDataOld = false;
            $scope.ShowEmpanelledCADataOld = true;
        }
        else {
            $scope.divAuditType = false;
            $scope.divDistrict = true;
            $scope.divAuditor = false;
            $scope.divBtn = true;
            $scope.ShowDepartmentDataOld = true ;
            $scope.ShowEmpanelledCADataOld = false ;
        }
        $(".se-pre-con").fadeOut("slow");
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
    $scope.OnPageLoad();
    $scope.GetAllAuditorAssignmentByDepartment(1);
    $scope.AssignedAuditorList();

});





