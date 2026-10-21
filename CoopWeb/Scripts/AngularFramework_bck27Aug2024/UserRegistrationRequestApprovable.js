
var MasterMod = angular.module('UserRegistrationApprsModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("UserRegistrationApprCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 15;
    $scope.UserDetailAll = '';
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UserRegistrationAppr = [];
    $scope.RequestType = '';
    $scope.WorkActId = '';
    $scope.UserType = 1;
    $scope.ActList = [];
    $scope.SelectAll = function (Check, counter) {
        angular.forEach($scope.UserReqData, function (item) {
            if (counter == 1) {
                item.IsChecked = Check;
            }
            else if (counter == 2) {
                item.IsChecked2 = Check;
            }

        });
    }

    $scope.ChangeUserType = function (type) {
        
        $scope.UserType = type;
        $scope.LoadActs();
        if (type == 1) {
            $scope.GetAllSubUnitList(false);
        } else {
            $scope.GetAllApprovedUnitList();
        }
    }
    $scope.NewOfficeId1 = '0';

    $scope.MappingChangeStatus12345 = function (MappingID, Action, UserId, OfficeId, DesignationLevelMappingId, Class) {
        
        $scope.NewOfficeId1 = Class.OfficeId;
        $scope.Flag = 1;
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/User/MappingChangeStatus", { "MappingID": MappingID, "Action": Action, "UserId": UserId, "Flag": $scope.Flag }).
            success(function (data, status) {
                
                $(".blockUI").fadeOut("slow");
                if ($scope.NewOfficeId1 == 3 || $scope.NewOfficeId1 == 41) {
                    if (data == '2') {
                        $scope.UserRegistrationAppr.UserId = '0';
                        $scope.MappingID = MappingID;
                        $scope.Status = Action;
                        $('#ReplaceCheck_new').prop('checked', false);
                        $scope.UserListActive = [];
                        $scope.UserReqData_New.filter(function (x) { return x.DesignationLevelMappingId === Class.DesignationLevelMappingId && x.OfficeId == $scope.NewOfficeId1 }).forEach(function (item, index) {
                            $scope.UserListActive.push({
                                UserId: item.UserId,
                                OfficerNameEn: item.OfficerNameEn,
                                OfficerNameHi: item.OfficerNameHi,
                                DesignationLevelMappingId: item.DesignationLevelMappingId
                            });
                        });
                        $("#myModal11").modal("show");
                    } else if (data == '1') {
                        Class.IsActive = Action == 1 ? 0 : 1;
                    } else {
                        ShowOkMessage("Error", "There is some error. Please try again", function () {

                        });

                    }
                }
                else if (data == '1') {
                    Class.IsActive = Action == 1 ? 0 : 1;
                } else {
                    ShowOkMessage("Error", "There is some error. Please try again", function () {
                    });

                }
                //$scope.Roles = data;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                ShowOkMessage("Error", "There is some error. Please try again", function () {
                });
            });
    };
    $scope.MappingChangeStatus = function (MappingID, Action, UserId, OfficeId, DesignationLevelMappingId, clss, arIndex) {
        
        $scope.NewOfficeId1 = clss.OfficeId;
        $scope.Flag = 1;
        $scope.EditIndex = arIndex;
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/User/MappingChangeStatus", { "MappingID": MappingID, "Action": Action, "UserId": UserId, "Flag": $scope.Flag }).
            success(function (data, status) {
                
                $(".blockUI").fadeOut("slow");
                if ($scope.NewOfficeId1 == 3 || $scope.NewOfficeId1 == 41) {
                    if (data == '2') {
                        $scope.UserRegistrationAppr.UserId = '0';
                        $scope.MappingID = MappingID;
                        $scope.Status = Action;
                        $('#ReplaceCheck_new').prop('checked', false);
                        $scope.UserListActive = [];
                        $scope.UserReqData_New.filter(function (x) { return x.RoleId === clss.RoleId && x.OfficeId === clss.OfficeId && x.DISTRICT_ENG === clss.DISTRICT_ENG && x.IsActive == "1" && x.UserId != clss.UserId }).forEach(function (item, index) {
                            item.ActId.split(',').forEach(function (a, indx) {
                                if (clss.ActId.indexOf(a) > -1) {
                                    var actname = "";
                                    if (item.ActId.indexOf('1') > -1) { actname += "1958"; }
                                    if (item.ActId.indexOf('3') > -1) { actname += (actname != "" ? "," : "") + "2001"; }
                                    if (item.ActId.indexOf('7') > -1) { actname += (actname != "" ? "," : "") + "2005"; }
                                    $scope.UserListActive.push({
                                        MappingID: item.MappingID,
                                        UserId: item.UserId,
                                        OfficerNameEn: item.OfficerNameEn + "," + item.RoleName_EN + "(" + item.OfficeNameEn + "),Act-" + "" + actname + "",
                                        OfficerNameHi: item.OfficerNameHi + "," + item.RoleName_HI + "(" + item.OfficeNameHi + "),Act-" + "" + actname + "",
                                        DesignationLevelMappingId: item.DesignationLevelMappingId
                                    });
                                    return false
                                }
                            });
                        });
                        $("#myModal11").modal("show");
                    } else if (data == '1') {
                        clss.IsActive = Action == 1 ? 0 : 1;
                    } else {
                        ShowOkMessage("Error", "There is some error. Please try again", function () {

                        });

                    }
                }
                else if (data == '1') {
                    clss.IsActive = Action == 1 ? 0 : 1;
                } else {
                    ShowOkMessage("Error", "There is some error. Please try again", function () {
                    });

                }
                //$scope.Roles = data;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                ShowOkMessage("Error", "There is some error. Please try again", function () {
                });
            });
    };

    $scope.Cancelpopup = function () {
        $scope.UserRegistrationAppr.UserId = '0';
        $("#myModal11").modal("hide");
    }



    $scope.SubmitFinal = function (status) {
        $scope.SaveUserInactive(status);
        //$("#myModal_inactive").modal("hide");
    }

    $scope.SaveUserInactive = function (status) {

        var message = "";
        if ($scope.UserRegistrationAppr.UserId == '' || $scope.UserRegistrationAppr.UserId == undefined || $scope.UserRegistrationAppr.UserId == "0") {
            message += $scope._language == 'en-us' ? 'Please Select another user and allocated to society.' : '<br />कृपया किसी अन्य उपयोगकर्ता का चयन करें और समाज को आवंटित करें।';
            if (message.length > 29) {
                ShowOkMessage("निम्नलिखित मान्य करें", message, function () { });
                return false;
            }
        }
        else {
            $(".blockUI").fadeIn("slow");
            $scope.UserId1 = $scope.UserRegistrationAppr.UserId;
            $scope.Flag = 2;
            $http.post(SiteUrl + "/User/MappingChangeStatus", { "MappingID": $scope.MappingID, "Action": $scope.Status, "UserId": $scope.UserId1, "Flag": $scope.Flag }).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    //$("#myModal11").modal("hide");
                    //$("#myModal_inactive").modal("hide");
                    // Class.IsActive = $scope.Status == 1 ? 0 : 1;

                    if (data == "1") {
                        $scope.UserReqData.filter(function (x) { return x.MappingID === $scope.MappingID })[0].IsActive = $scope.Status == 1 ? 0 : 1;
                        ShowOkMessage("Success", "Record saved successfully.", function () {
                            $("#myModal11").modal("hide");
                            $("#myModal").modal("hide");
                        });
                    } else {
                        ShowOkMessage("Error", "There is some error. Please try again", function () {
                            $("#myModal11").modal("hide");
                            $("#myModal").modal("hide");
                        });
                    }

                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    ShowOkMessage("Error", "There is some error. Please try again", function () {
                    });
                });
        }
    }

    // on page load
    $scope.OnPageLoad = function () {
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

            $scope.ClearUnitForm = function () {
                $scope.UserRegistrationAppr = null;
            };
        }
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
            success(function (data, status) {
                $scope.ActList = data;
                $scope.LoadActs();
            }).
            error(function (data, status) {
                alert("Error while fetching Act list.");
                // $scope.showLoader = false;
            });



    };


    $scope.LoadActs = function () {

        //$("#ddlAct").html("");
        //if ($scope.UserType == 2)
        //{
        //    $("#ddlAct").append('<input type="checkbox" value="0" onclick="ResetActs(this)" /><label style="font-weight:bold!important; margin-left: 5px; ">No Act</label><br />');

        //}

        $scope.ActList.forEach(function (item, indx) {
            if (item.ActId == 1) {
                $scope.IsShowAct1958 = true; $scope.ActName1958 = item.ActNameEn;
            } else if (item.ActId == 3) {
                $scope.IsShowAct2001 = true; $scope.ActName2001 = item.ActNameEn;
            } else if (item.ActId == 7) {
                $scope.IsShowAct2005 = true; $scope.ActName2005 = item.ActNameEn;
            }
        });

        //$.each($scope.ActList, function () {

        //    //$("#ddlAct").append($("<option></option>").val(this.ActId).html(this.ActNameEn));
        //    $("#ddlAct").append('<input type="checkbox" onclick="chnageReplaceOff()" value="' + this.ActId + '" /><label style="font-weight:bold!important; margin-left: 5px; ">' + this.ActNameEn + '</label><br />');
        //    //$("#DvEditUser").find("#ddlAct").append('<input type="checkbox" value="' + this.ActId + '" /><label style="font-weight:bold!important; margin-left: 5px; ">' + this.ActNameEn + '</label><br />');
        //});

        return true;

        //$('#ddlAct').multiselect({
        //    includeSelectAllOption: true
        //});

    }

    $scope.Showdesination = false;

    $scope.checkReplace = function () {
        if (document.getElementById('ReplaceCheck').checked == true) {
            $scope.Showdesination = true;
            $scope.chnageReplaceOff();
        }
        else {
            $scope.Showdesination = false;
        }

    }

    //$scope.checkReplace1 = function () {
    //    if (document.getElementById('ReplaceCheck_new').checked == true) {
    //        $scope.Showdesination1 = true;
    //    }
    //    else {
    //        $scope.Showdesination1 = false;
    //    }

    //}

    //$scope.Designations=[];

    //$scope.GetDesignationList = function () {
    //    

    //        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=designation&MasterCode=-3").
    //            success(function (data, status) {
    //                $scope.Designations = data;
    //            }).
    //            error(function (data, status) {
    //                $scope.Designations = [];
    //            });

    //};



    //    START : Get ResionMapping LIST
    $scope.flag = 0;
 //   $("#ddlPrimaryResion").html("");
    function GetResionMapping(DistId, OrganizationLevelsMappingID, MappingID) {
        //if (DistId != 0) {
        //    $("#ddlPrimaryResion").html("");
        //    $http.get(SiteUrl + "/Master/ResionMappingByDistIdOrganizationLevelsMappingID/?OrganizationLevelsMappingID=" + OrganizationLevelsMappingID + "&DistrictId=" + DistId + "&MappingID=" + MappingID).
        //        success(function (data, status) {

        //            $.each(data, function () {
        //                if (this.UserRoleResionMappingId == "") {                           
        //                    $("#ddlPrimaryResion").append('<input type="checkbox" value="' + this.ResionId + '" /><label style="font-weight:bold!important; margin-left: 5px; ">' + this.ResionName + '</label><br />');
        //                } else {                           
        //                    $("#ddlPrimaryResion").append('<input type="checkbox" value="' + this.ResionId + '" /><label style="font-weight:bold!important; margin-left: 5px; ">' + this.ResionName + '</label><br />');
        //                    $("#ddlPrimaryResion").find('input[value=' + this.ResionId + ']').prop("checked", true);

        //                }
        //            });

        //        }).
        //        error(function (data, status) {
        //            $("#ddlPrimaryResion").empty();                   
        //        });
        //}
        //else {
        //    $("#ddlPrimaryResion").empty();           
        //}
    };


    // START : Change Status for SMS / delete SMS,Status
    $scope.SaveUserRegiApprFun = function () {


        if (document.getElementById('ReplaceCheck').checked == true) {
            if ($scope.UserRegistrationAppr.UserId == '' || $scope.UserRegistrationAppr.UserId == undefined || $scope.UserRegistrationAppr.UserId == "0") {
                alert("Please select User Name");
                return;
            }
        }
        if ($scope.UserRegistrationAppr.approval == "" || $scope.UserRegistrationAppr.approval == undefined) {
            alert("Please select Approve/Reject");
            return;
        }
        else if ($scope.UserRegistrationAppr.approval == 'true') {
            if ($scope.UserRegistrationAppr.RoleID == 0) {
                alert("Please select Role");
                return;
            }
            else {
                var ActIds = $('select#ddlAct').val();


                var ids = [];
                $.each($("#ddlAct").find('input[type="checkbox"]:checked'), function () {
                    ids.push($(this).val());
                });

                ActIds = ids.join(",");


                if ($("#ddlAct").find('input[type="checkbox"]:checked').length == 0 && $scope.OrganisationId == 4) {
                    alert("Please select Act");
                    return;
                } else {

                    $scope.SaveUserRegiApprForm();
                }

            }
        }
        else {
            $scope.SaveUserRegiApprForm();
        }
    };
    $scope.SaveUserRegiApprForm = function () {
        //$scope.WorkActId = $('select#ddlAct').val();

        var ids = [];
        $.each($("#ddlAct").find('input[type="checkbox"]:checked'), function () {
            ids.push($(this).val());
        });

        $scope.WorkActId = ids.join(",");

        var ids1 = [];
        //$.each($("#ddlPrimaryResion").find('input[type="checkbox"]:checked'), function () {
        //    ids1.push($(this).val());
        //});

        var PrimaryResionIdLocal = ids1.join(",");


       
        var DairyResionIdLocal = "";        

        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/User/UpdateUserRoleApprForm", { MappingID: $scope.UserRegistrationAppr.MappingID, RoleID: $scope.UserRegistrationAppr.RoleID, IsApproved: $scope.UserRegistrationAppr.approval, ActId: $scope.WorkActId.toString(), RequestType: $scope.RequestType, PrimaryResionId: PrimaryResionIdLocal.toString(), DairyResionId: DairyResionIdLocal.toString(), UserId: $scope.UserRegistrationAppr.UserId }).
            success(function (data, status) {
                if (data.length > 0) {
                    if (data[0].ExistFlag.indexOf("-") > -1) {
                        alert("User " + data[0].OfficerNameEn + " (SSO Id is " + data[0].SSO_ID + ")" + " already present.");
                    } else if (data[0].ExistFlag.indexOf("0") > -1) {
                        //alert("Data is not available.");

                        ShowOkMessage("Success", "Data is not available.", function () {
                            // $scope.GetAllSubUnitList();
                            //$("#myModal").modal("hide");
                        });

                    } else if (data[0].ExistFlag.indexOf("1") > -1) {
                        //alert("Record saved successfully.");
                        //window.location = '../User/UserRegistrationRequestApprovable';
                        ShowOkMessage("Success", "Record saved successfully.", function () {
                            $scope.GetAllSubUnitList(false);
                            $("#myModal").modal("hide");
                        });

                    } else if (data[0].ExistFlag.indexOf("2") > -1) {
                        ShowOkMessage("Success", "Request rejected successfully.", function () {
                            $scope.GetAllSubUnitList(false);
                            $("#myModal").modal("hide");
                        });
                    }
                    $(".blockUI").fadeOut("slow");
                }

                // $scope.GetAllSubUnitList();
            }).
            error(function (data, status) {
            });
    };

    $scope.SaveUserRegiApprFunEdit = function () {
        //if ($scope.UserRegistrationAppr.approval == "" || $scope.UserRegistrationAppr.approval == undefined) {
        //    alert("Please select Approve/Reject");
        //    return;
        //}
        //else if ($scope.UserRegistrationAppr.approval == 'true') {
        //    if ($scope.UserRegistrationAppr.RoleID == 0) {
        //        alert("Please select Role");
        //        return;
        //    }
        //    else {
        //var ActIds = $('select#ddlAct').val();
        //if (ActIds == "" || ActIds == undefined) {
        //    alert("Please select Act");
        //    return;
        //}
        //else {
        $scope.SaveUserRegiApprFormEdit();
        //}
        //    }
        //}
        //else {
        //    $scope.SaveUserRegiApprForm();
        //}
    };
    $scope.SaveUserRegiApprFormEdit = function () {
 
        var ids = [];
        $.each($("#ddlAct").find('input[type="checkbox"]:checked'), function () {
            ids.push($(this).val());
        });

        $scope.WorkActId = ids.join(",");

        var ids1 = [];
        //$.each($("#ddlPrimaryResion").find('input[type="checkbox"]:checked'), function () {
        //    ids1.push($(this).val());
        //});

        var PrimaryResionIdLocal = ids1.join(",");



        var DairyResionIdLocal = "";      
        $http.post(SiteUrl + "/User/UpdateUserRoleForm", { MappingID: $scope.UserRegistrationAppr.MappingID, ActId: $scope.WorkActId.toString(), RequestType: $scope.RequestType, PrimaryResionId: PrimaryResionIdLocal.toString(), DairyResionId: DairyResionIdLocal.toString() }).
            success(function (data, status) {
                if (data[0].ExistFlag.indexOf("-") > -1) {
                    alert("User " + data[0].OfficerNameEn + " (SSO Id is " + data[0].SSO_ID + ")" + " already present.");
                } else if (data[0].ExistFlag.indexOf("0") > -1) {
                    alert("Data is not available.");
                } else if (data[0].ExistFlag.indexOf("1") > -1) {
                    alert("Record saved successfully.");
                    $("#myModal").modal("hide");
                    $scope.GetAllApprovedUnitList();
                }
                //window.location = '../User/UserDetailList';
                // $scope.GetAllSubUnitList();
            }).
            error(function (data, status) {
            });



    }



    $scope.UserReqData_New = [];
    $scope.GetAllSubUnitList = function (isExport) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/User/UserReqList?isExport=" + isExport).
            success(function (data, status) {
                $scope.UserReqData = data;
                $scope.UserReqData_New = data;

                if (data.length > 0) {
                    //$scope.OrganisationId = $scope.UserReqData[0].OrganisationId;                    
                }
                $(".se-pre-con").fadeOut("slow");

            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetAllApprovedUnitList = function () {
        $scope.UserReqData_New = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/User/GetUserDetailList").
            success(function (data, status) {
                $scope.UserReqData = data;
                $scope.UserReqData_New = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data1, status) {

            });
    }


    //$scope.update = function (item) {

    //    $('#ReplaceCheck').prop('checked', false);
    //    $scope.checkstatus = false;
    //    $scope.Showdesination = false;

    //    $scope.NewOfficeId = item.OfficeId;
    //    $scope.UserListActive = [];
    //    $scope.UserReqData_New.filter(function (x) { return x.DesignationLevelMappingId === item.DesignationLevelMappingId }).forEach(function (item, index) {
    //        $scope.UserListActive.push({
    //            UserId: item.UserId,
    //            OfficerNameEn: item.OfficerNameEn,
    //            OfficerNameHi: item.OfficerNameHi,
    //            DesignationLevelMappingId: item.DesignationLevelMappingId
    //        });
    //    });


    //    $(".se-pre-con").fadeIn("slow");
    //    $scope.LoadActs();
    //    $scope.UserRegistrationAppr.MappingID = item.MappingID;
    //    $scope.UserRegistrationAppr.LevelId = item.LevelId;       
    //    $scope.OrganisationId = item.OrganisationId;
    //  //  alert(item.OrganisationId);
    //    $.each($("#ddlAct").find('input[type="checkbox"]'), function () {
    //        $(this).prop("checked", false);
    //    });


    //    if (item.ActId.indexOf(',') !== -1) {
    //        $.ActIdArr = item.ActId.split(',');
    //        $.each($.ActIdArr, function (intIndex, objValue) {
    //            $("#ddlAct").find('input[value="' + objValue + '"]').prop("checked", true);               
    //        });
    //    }
    //    else {
    //        $('#ddlAct input[value=' + item.ActId + ']').prop("checked", true);            
    //    }

    //    $scope.RoleList();
    //    $scope.GetUserReqDetail();
    //};

    $scope.NewUserdata = {};
    $scope.IsNewUserData = false;
    $scope.update = function (clss, IsNew) {
        $scope.UserRegistrationAppr.Formation = false;
        $scope.IsNewUserData = IsNew;
        if (IsNew) {
            $scope.NewUserdata = clss;
            $scope.UserReqData_New = [];
            $(".se-pre-con").fadeIn("slow");
            $http.get(SiteUrl + "/User/GetUserDetailList").
                success(function (data, status) {
                    $scope.UserReqData_New = data;
                    $(".se-pre-con").fadeOut("slow");
                    $('#ReplaceCheck').prop('checked', false);
                    $scope.NewOfficeId = clss.OfficeId;
                    $scope.UserListActive = [];
                    $scope.UserReqData_New.filter(function (x) { return x.DesignationLevelMappingId === clss.DesignationLevelMappingId && x.OfficeId === clss.OfficeId && x.DISTRICT_ENG === clss.DISTRICT_ENG && x.IsActive == "1" && x.UserId != clss.UserId }).forEach(function (item, index) {
                        item.ActId.split(',').forEach(function (a, indx) {
                            if ($scope.NewUserdata.ActId.indexOf(a) > -1) {
                                var actname = "";
                                if (item.ActId.indexOf('1') > -1) { actname += "1958"; }
                                if (item.ActId.indexOf('3') > -1) { actname += (actname != "" ? "," : "") + "2001"; }
                                if (item.ActId.indexOf('7') > -1) { actname += (actname != "" ? "," : "") + "2005"; }
                                $scope.UserListActive.push({
                                    MappingID: item.MappingID,
                                    UserId: item.UserId,
                                    OfficerNameEn: item.OfficerNameEn + "," + item.RoleName_EN + "(" + item.OfficeNameEn + "),Act-" + "" + actname + "",
                                    OfficerNameHi: item.OfficerNameHi + "," + item.RoleName_HI + "(" + item.OfficeNameHi + "),Act-" + "" + actname + "",
                                    DesignationLevelMappingId: item.DesignationLevelMappingId
                                });
                                return false
                            }
                        });
                    });
                }).
                error(function (data1, status) {

                });
        }



        $(".se-pre-con").fadeIn("slow");
        $scope.LoadActs();
        $scope.UserRegistrationAppr.MappingID = clss.MappingID;
        $scope.UserRegistrationAppr.LevelId = clss.LevelId;
        $scope.OrganisationId = clss.OrganisationId;
        //alert(item.OrganisationId);
        $.each($("#ddlAct").find('input[type="checkbox"]'), function () {
            $(this).prop("checked", false);
        });




        if (clss.ActId.indexOf(',') !== -1) {
            $.ActIdArr = clss.ActId.split(',');
            $.each($.ActIdArr, function (intIndex, objValue) {
                $("#ddlAct").find('input[value="' + objValue + '"]').prop("checked", true);
            });
        }
        else if (clss.ActId=='') { } //madhuri
        else
        {
            $('#ddlAct input[value=' + clss.ActId + ']').prop("checked", true);
        }



        $scope.RoleList();
        $scope.GetUserReqDetail();
    };

    $scope.chnageReplaceOff = function () {
        if ($scope.IsNewUserData && $scope.UserRegistrationAppr.Formation) {
            $scope.NewUserdata = $scope.NewUserdata;
            // $scope.UserReqData_New = [];
            $(".se-pre-con").fadeIn("slow");

            var ActIds = $('select#ddlAct').val();
            var ids = [];
            $.each($("#ddlAct").find('input[type="checkbox"]:checked'), function () {
                ids.push($(this).val());
            });
            $scope.UserRegistrationAppr.UserId = "0"
            $scope.NewOfficeId = $scope.NewUserdata.OfficeId;
            $scope.UserListActive = [];
            //UserRegistrationAppr.RoleID [0].RoleId
            $scope.UserReqData_New.filter(function (x) { return x.RoleId === $scope.UserRegistrationAppr.RoleID && x.OfficeId === $scope.NewUserdata.OfficeId && x.DISTRICT_ENG === $scope.NewUserdata.DISTRICT_ENG && x.IsActive == "1" && x.UserId != $scope.NewUserdata.UserId }).forEach(function (item, index) {
                item.ActId.split(',').forEach(function (a, indx) {
                    if (ids.indexOf(a) > -1 && $scope.UserListActive.filter(function (tm) { return tm.MappingID === item.MappingID }).length == 0) {
                        var actname = "";
                        if (item.ActId.indexOf('1') > -1) { actname += "1958"; }
                        if (item.ActId.indexOf('3') > -1) { actname += (actname != "" ? "," : "") + "2001"; }
                        if (item.ActId.indexOf('7') > -1) { actname += (actname != "" ? "," : "") + "2005"; }
                        $scope.UserListActive.push({
                            MappingID: item.MappingID,
                            UserId: item.UserId,
                            OfficerNameEn: item.OfficerNameEn + "," + item.RoleName_EN + "(" + item.OfficeNameEn + "),Act-" + "" + actname + "",
                            OfficerNameHi: item.OfficerNameHi + "," + item.RoleName_HI + "(" + item.OfficeNameHi + "),Act-" + "" + actname + "",
                            DesignationLevelMappingId: item.DesignationLevelMappingId
                        });
                        return false
                    }
                });
            });
            $(".se-pre-con").fadeOut("slow");
        }


    }

    $scope.CheckRoleAtCuttentLocation = function () {

        var ids = [];
        $.each($("#ddlAct").find('input[type="checkbox"]:checked'), function () {
            ids.push($(this).val());
        });

        var WorkActId1 = ids.join(",");

        var ids1 = [];
        //$.each($("#ddlPrimaryResion").find('input[type="checkbox"]:checked'), function () {
        //    ids1.push($(this).val());
        //});

        var PrimaryResionIdLocal1 = ids1.join(",");



        var DairyResionIdLocal1 = "";

        $http.post(SiteUrl + "/User/CheckRoleAtCuttentLocation", { MappingID: $scope.UserRegistrationAppr.MappingID, ActId: WorkActId1.toString(), RequestType: $scope.RequestType, PrimaryResionId: PrimaryResionIdLocal1.toString(), DairyResionId: DairyResionIdLocal1.toString() }).
            success(function (data, status) {
                if (data[0].ExistFlag.indexOf("-") > -1) {
                    alert("User " + data[0].OfficerNameEn + " (SSO Id is " + data[0].SSO_ID + ")" + " already present.");
                } else if (data[0].ExistFlag.indexOf("0") > -1) {
                    alert("Data is not available.");
                } else if (data[0].ExistFlag.indexOf("1") > -1) {
                    alert("This Location is Empty.");
                }

            }).
            error(function (data, status) {
            });
    }

    $scope.ClearOrganisationForm = function (item) {
        $scope.UserRegistrationAppr.MappingID = '0';
        $scope.UserRegistrationAppr.RoleID = '0';
        $scope.UserRegistrationAppr.approval = '';
    };

    $scope.RoleList = function () {
        //$http.get(SiteUrl + "/User/SelectRole").        
        $http.get(SiteUrl + "/Master/SelectRole/?LevelId=" + $scope.UserRegistrationAppr.LevelId).
            success(function (data, status) {
                $scope.Roles = data;
                $scope.showLoader = false;
            }).
            error(function (data, status) {
                alert("Error while fetching Role list");
                $scope.showLoader = false;
            });
    };

    $scope.GetUserReqDetail = function () {
        $http.get(SiteUrl + "/User/GetUserReqDetail/?MappingID=" + $scope.UserRegistrationAppr.MappingID).
            success(function (data, status) {
                GetResionMapping(data[0].DistrictId, data[0].OrganizationLevelsMappingID, $scope.UserRegistrationAppr.MappingID);
                $scope.UserDetailAll = "Name :  " + data[0].OfficerNameEn + "<br/>Molile No. :  " + data[0].MobileNo + "<br/>SSO Id :  " + data[0].SSO_ID + "<br/>Level :  " + data[0].LevelText
                $scope.UserDetailAll = $scope.UserDetailAll + (data[0].DIVISION_ENG == "" ? "" : ("<br/>Division :  " + data[0].DIVISION_ENG));
                $scope.UserDetailAll = $scope.UserDetailAll + (data[0].DISTRICT_ENG == "" ? "" : ("<br/>District :  " + data[0].DISTRICT_ENG));
                $scope.UserDetailAll = $scope.UserDetailAll + (data[0].BLOCK_ENG == "" ? "" : ("<br/>Block :  " + data[0].BLOCK_ENG));
                $scope.UserDetailAll = $scope.UserDetailAll + (data[0].CITY_ENG == "" ? "" : ("<br/>City :  " + data[0].CITY_ENG));
                $scope.UserDetailAll = $scope.UserDetailAll + (data[0].AgentNameEng == "" || data[0].AgentNameEng == "undefined" ? "" : ("<br/>Depot :  " + data[0].AgentNameEng));
                $scope.UserDetailAll = $scope.UserDetailAll + (data[0].ProcessingUnitNameEng == "" || data[0].ProcessingUnitNameEng == "undefined" ? "" : ("<br/>Plant :  " + data[0].ProcessingUnitNameEng));
                $scope.UserDetailAll = $scope.UserDetailAll + (data[0].WarehousNameEn == "" || data[0].WarehousNameEn == "undefined" ? "" : ("<br/>Warehouse :  " + data[0].WarehousNameEn));
                $scope.UserDetailAll = $scope.UserDetailAll + (data[0].PLDBName == "" || data[0].PLDBName == "undefined" ? "" : ("<br/>PLDB Name :  " + data[0].PLDBName));
                $scope.UserDetailAll = $scope.UserDetailAll + (data[0].BranchName_En == "" || data[0].BranchName_En == "undefined" ? "" : ("<br/>PLDB Branch :  " + data[0].BranchName_En));

                // $("#spanUserDetail").text($scope.UserDetailAll);
                $("#spanUserDetail").html($scope.UserDetailAll + "<br/><br/>");
                $scope.RequestType = data[0].RequestType;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching Role list")
                $scope.showLoader = false;
            });
    };



    $scope.OnPageLoad();
    $scope.GetAllSubUnitList(false);

});





