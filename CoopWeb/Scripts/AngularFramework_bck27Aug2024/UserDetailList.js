var MasterMod = angular.module('UserDetailListModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("UserDetailListCont", function ($scope, $http) {

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
    $scope.UserRegistrationSearch = [];
    $scope.UserReqData_New = [];
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
            }).
            error(function (data, status) {
                alert("Error while fetching Act list.");
                // $scope.showLoader = false;
            });
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
            success(function (data, status) {
                //  $scope.ActList = data;
                $.each(data, function () {
                    //$("#ddlAct").append($("<option></option>").val(this.ActId).html(this.ActNameEn));
                    $("#ddlAct").append('<input type="checkbox" value="' + this.ActId + '" /><label style="font-weight:bold!important; margin-left: 5px; ">' + this.ActNameEn + '</label><br />');
                });
                //$('#ddlAct').multiselect({
                //    includeSelectAllOption: true
                //});
            }).
            error(function (data, status) {
                alert("Error while fetching Act list.");
                // $scope.showLoader = false;
            });

        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ALLRESIONMAPPING&MasterCode=-1").
        //    success(function (data, status) {
        //        $scope.ResionList = data;
        //    }).
        //    error(function (data, status) {
        //        alert("Error while fetching Act list.");
        //        // $scope.showLoader = false;
        //    });

    };

    function myfonction() {
        ////  alert("fine-" + ResionIdPrimaryLocal + "" + ResionIdDairyLocal);
        //if (ResionIdPrimaryLocal != "") {
        //    //  alert(1);
        //    $("#ddlPrimaryResion option:selected").removeAttr("selected");
        //    $("#ddlPrimaryResion option:selected").prop("selected", false);
        //    if (ResionIdPrimaryLocal.indexOf(',') !== -1) {
        //        //  alert(2);
        //        $.ActIdArr = ResionIdPrimaryLocal.split(',');
        //        $.each($.ActIdArr, function (intIndex, objValue) {
        //            //     alert(objValue);
        //            $('#ddlPrimaryResion option[value=' + objValue + ']').attr('selected', true);
        //        });
        //    }
        //    else {
        //        $('#ddlPrimaryResion option[value=' + ResionIdPrimaryLocal + ']').attr('selected', true);
        //    }
        //}
        //if (ResionIdDairyLocal != "") {
        //    $("#ddlDairyResion option:selected").removeAttr("selected");
        //    $("#ddlDairyResion option:selected").prop("selected", false);
        //    if (ResionIdDairyLocal.indexOf(',') !== -1) {
        //        $.ActIdArr = ResionIdDairyLocal.split(',');
        //        $.each($.ActIdArr, function (intIndex, objValue) {
        //            $('#ddlDairyResion option[value=' + objValue + ']').attr('selected', true);
        //        });
        //    }
        //    else {
        //        $('#ddlDairyResion option[value=' + ResionIdDairyLocal + ']').attr('selected', true);
        //    }
        //}
    }
    //    START : Get ResionMapping LIST
    var ResionIdPrimaryLocal = "";
    var ResionIdDairyLocal = "";
    $scope.flag = 0;
    function GetResionMapping(DistId, OrganizationLevelsMappingID, MappingID) {
        //  alert(DistId + "------" + OrganizationLevelsMappingID + "------" + MappingID);
       // $("#ddlPrimaryResion option:selected").removeAttr("selected");
        if (DistId != 0) {
            $("#ddlPrimaryResion").html("");
            $http.get(SiteUrl + "/Master/ResionMappingByDistIdOrganizationLevelsMappingID/?OrganizationLevelsMappingID=" + OrganizationLevelsMappingID + "&DistrictId=" + DistId + "&MappingID=" + MappingID).
                success(function (data, status) {
                    //$("#ddlPrimaryResion").empty();
                    $.each(data, function () {
                        if (this.UserRoleResionMappingId == "") {
                            //$("#ddlPrimaryResion").append($("<option></option>").val(this.ResionId).html(this.ResionName));
                            $("#ddlPrimaryResion").append('<input type="checkbox" value="' + this.ResionId + '" /><label style="font-weight:bold!important; margin-left: 5px; ">' + this.ResionName + '</label><br />');

                        }
                        else
                        {
                            //$("#ddlPrimaryResion").append($("<option selected='selected'></option>").val(this.ResionId).html(this.ResionName));
                            $("#ddlPrimaryResion").append('<input type="checkbox" value="' + this.ResionId + '" /><label style="font-weight:bold!important; margin-left: 5px; ">' + this.ResionName + '</label><br />');
                            $("#ddlPrimaryResion").find('input[value=' + this.ResionId + ']').prop("checked", true);                         
                        }
                    });
                    //if ($scope.flag == 1) {
                    //    $('#ddlPrimaryResion').multiselect('destroy');
                    //}
                    //$('#ddlPrimaryResion').multiselect({
                    //    includeSelectAllOption: true
                    //});
                    //$scope.flag = 1;
                }).
                error(function (data, status) {
                    $("#ddlPrimaryResion").html("");
                    // $('#ddlPrimaryResion').multiselect('destroy');
                });
        }
        else {
            $("#ddlPrimaryResion").html("");
            //  $('#ddlPrimaryResion').multiselect('destroy');
        }
    };


    // START : Change Status for SMS / delete SMS,Status
    $scope.SaveUserRegiApprFun = function () {
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
        $scope.SaveUserRegiApprForm();
        //}
        //    }
        //}
        //else {
        //    $scope.SaveUserRegiApprForm();
        //}
    };
    $scope.SaveUserRegiApprForm = function () {
       // $scope.WorkActId = $('select#ddlAct').val();
       // var PrimaryResionIdLocal = $('select#ddlPrimaryResion').val();


        var ids = [];
        $.each($("#ddlAct").find('input[type="checkbox"]:checked'), function () {
            ids.push($(this).val());
        });

        $scope.WorkActId = ids.join(",");

        var ids1 = [];
        $.each($("#ddlPrimaryResion").find('input[type="checkbox"]:checked'), function () {
            ids1.push($(this).val());
        });

        var PrimaryResionIdLocal = ids1.join(",");



        var DairyResionIdLocal = "";
        // alert(PrimaryResionIdLocal + "----" + DairyResionIdLocal);
        $http.post(SiteUrl + "/User/UpdateUserRoleForm", { MappingID: $scope.UserRegistrationAppr.MappingID, ActId: $scope.WorkActId.toString(), RequestType: $scope.RequestType, PrimaryResionId: PrimaryResionIdLocal.toString(), DairyResionId: DairyResionIdLocal.toString() }).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                if (data[0].ExistFlag.indexOf("-") > -1) {
                    alert("User " + data[0].OfficerNameEn + " (SSO Id is " + data[0].SSO_ID + ")" + " already present.");
                    $(".se-pre-con").fadeOut("slow");
                } else if (data[0].ExistFlag.indexOf("0") > -1) {
                    alert("Data is not available.");
                    $(".se-pre-con").fadeOut("slow");
                } else if (data[0].ExistFlag.indexOf("1") > -1) {
                    alert("Record saved successfully.");
                    $(".se-pre-con").fadeOut("slow");
                }
                window.location = '../User/UserDetailList';
                // $scope.GetAllSubUnitList();
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });



    }

    $scope.CheckRoleAtCuttentLocation = function () {

        var ids = [];
        $.each($("#ddlAct").find('input[type="checkbox"]:checked'), function () {
            ids.push($(this).val());
        });

        var WorkActId1 = ids.join(",");

        var ids1 = [];
        $.each($("#ddlPrimaryResion").find('input[type="checkbox"]:checked'), function () {
            ids1.push($(this).val());
        });

        var PrimaryResionIdLocal1 = ids1.join(",");


        //var WorkActId1 = $('select#ddlAct').val();
        //var PrimaryResionIdLocal1 = $('select#ddlPrimaryResion').val();
        var DairyResionIdLocal1 = "";
        // alert(PrimaryResionIdLocal + "----" + DairyResionIdLocal);
        $http.post(SiteUrl + "/User/CheckRoleAtCuttentLocation", { MappingID: $scope.UserRegistrationAppr.MappingID, ActId: WorkActId1.toString(), RequestType: $scope.RequestType, PrimaryResionId: PrimaryResionIdLocal1.toString(), DairyResionId: DairyResionIdLocal1.toString() }).
            success(function (data, status) {
                if (data[0].ExistFlag.indexOf("-") > -1) {
                    alert("User " + data[0].OfficerNameEn + " (SSO Id is " + data[0].SSO_ID + ")" + " already present.");
                } else if (data[0].ExistFlag.indexOf("0") > -1) {
                    alert("Data is not available.");
                } else if (data[0].ExistFlag.indexOf("1") > -1) {
                    alert("This Location is Empty.");
                }
                // window.location = '../User/UserDetailList';
                // $scope.GetAllSubUnitList();
            }).
            error(function (data, status) {
            });
    }
    $scope.GetAllSubUnitList = function () {

        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/User/GetUserDetailList").
            success(function (data, status) {
                $scope.UserReqData = data;
                $scope.UserReqData_New = data;
                $(".se-pre-con").fadeOut("slow");

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

    $scope.SaveUserInactive = function (status) {

        var message = "";
        if ($scope.UserRegistrationAppr.UserId == '' || $scope.UserRegistrationAppr.UserId == undefined || $scope.UserRegistrationAppr.UserId == "0") {
            message += $scope._language == 'en-us' ? 'Please Select another user and allocated to society.' : '<br />कृपया किसी अन्य उपयोगकर्ता का चयन करें और सोसाइटी को आवंटित करें।';
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

    $scope.update = function (item) {
        $(".se-pre-con").fadeIn("slow");
        $scope.UserRegistrationAppr.MappingID = item.MappingID;
        $scope.UserRegistrationAppr.LevelId = item.LevelId;
        //alert(item.ActId);
        $("#ddlAct option:selected").removeAttr("selected"); 

        $.each($("#ddlAct").find('input[type="checkbox"]'), function () {
            $(this).prop("checked", false);
        });

        if (item.ActId != '') {
            if (item.ActId.indexOf(',') !== -1) {
                $.ActIdArr = item.ActId.split(',');
                $.each($.ActIdArr, function (intIndex, objValue) {
                    $("#ddlAct").find('input[value="' + objValue + '"]').prop("checked", true);
                    //$('#ddlAct option[value=' + objValue + ']').attr('selected', true);
                });
            }
            else {
                $('#ddlAct input[value=' + item.ActId + ']').prop("checked", true);
            }
        }

        $scope.RoleList();
        $scope.GetUserReqDetail();
    };
    $scope.update12345 = function (clss, IsNew) {

        if (IsNew) {
            $scope.UserReqData_New = [];
            $(".se-pre-con").fadeIn("slow");
            $http.get(SiteUrl + "/User/GetUserDetailList").
                success(function (data, status) {
                    $scope.UserReqData_New = data;
                    $(".se-pre-con").fadeOut("slow");
                    $('#ReplaceCheck').prop('checked', false);
                    $scope.NewOfficeId = item.OfficeId;
                    $scope.UserListActive = [];
                    $scope.UserReqData_New.filter(function (x) { return x.DesignationLevelMappingId === clss.DesignationLevelMappingId && x.OfficeId === clss.OfficeId && x.DISTRICT_ENG === clss.DISTRICT_ENG && x.IsActive == "1" }).forEach(function (item, index) {
                        $scope.UserListActive.push({
                            MappingID: item.MappingID,
                            UserId: item.UserId,
                            OfficerNameEn: item.OfficerNameEn + "," + item.RoleName_EN + "(" + item.OfficeNameEn + ")",
                            OfficerNameHi: item.OfficerNameHi + "," + item.RoleName_HI + "(" + item.OfficeNameHi + ")",
                            DesignationLevelMappingId: item.DesignationLevelMappingId
                        });
                    });
                }).
                error(function (data1, status) {

                });
        }



        $(".se-pre-con").fadeIn("slow");
        $scope.LoadActs();
        $scope.UserRegistrationAppr.MappingID = item.MappingID;
        $scope.UserRegistrationAppr.LevelId = item.LevelId;
        $scope.OrganisationId = item.OrganisationId;
        //alert(item.OrganisationId);
        $.each($("#ddlAct").find('input[type="checkbox"]'), function () {
            $(this).prop("checked", false);
        });




        if (clss.ActId.indexOf(',') !== -1) {
            $.ActIdArr = item.ActId.split(',');
            $.each($.ActIdArr, function (intIndex, objValue) {
                $("#ddlAct").find('input[value="' + objValue + '"]').prop("checked", true);
            });
        }
        else {
            $('#ddlAct input[value=' + item.ActId + ']').prop("checked", true);
        }



        $scope.RoleList();
        $scope.GetUserReqDetail();
    };
    $scope.ClearOrganisationForm = function (item) {
       // $("#ddlPrimaryResion option:selected").removeAttr("selected");

        $.each($("#ddlAct").find('input[type="checkbox"]'), function () {
            $(this).prop("checked", false);
        });
        $.each($("#ddlPrimaryResion").find('input[type="checkbox"]'), function () {
            $(this).prop("checked", false);
        });

       // $("#ddlAct option:selected").removeAttr("selected");
    };
    $scope.NewOfficeId1 = '0';
    $scope.EditIndex = -1;
$scope.MappingChangeStatus = function (MappingID, Action, UserId, OfficeId, DesignationLevelMappingId, clss,arIndex) {
        
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
                        $scope.UserReqData_New.filter(function (x) { return x.DesignationLevelMappingId === clss.DesignationLevelMappingId && x.OfficeId === clss.OfficeId && x.DISTRICT_ENG === clss.DISTRICT_ENG && x.IsActive == "1" && x.UserId != clss.UserId }).forEach(function (item, index) {
                            item.ActId.split(',').forEach(function (a, indx) {
                                if (clss.ActId.indexOf(a) > -1) {
                                    var actname = "";
                                    if (item.ActId.indexOf('1') > -1) { actname += "1958"; }
                                    if (item.ActId.indexOf('3') > -1) { actname += (actname!=""?",":"")+ "2001"; }
                                    if (item.ActId.indexOf('7') > -1) { actname += (actname != "" ? "," : "") +"2005"; }
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

    $scope.MappingChangeStatus123 = function (MappingID, Status) {
        //$http.get(SiteUrl + "/User/SelectRole").        
        $http.post(SiteUrl + "/User/MappingChangeStatus", { "MappingID": MappingID, "Action": Status }).
            success(function (data, status) {
                //$scope.Roles = data;
            }).
            error(function (data, status) {
            });
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
    $scope.GetDepartments = function () {
        $scope.Departments = [];
        $scope.Levels = [];
        $scope.UserRegistrationSearch.OrganisationId = '0';
        $scope.Organisations = [];
        $scope.RoleListData = [];
        $scope.UserRegistrationSearch.RoleId = '0';
        $scope.UserRegistrationSearch.OrganizationLevelsMappingID = '0';
        $scope.UserRegistrationSearch.OfficeId = '0';
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DEPARTMENT&MasterCode=-1").
            success(function (data, status) {
                $scope.Departments = data;
            }).
            error(function (data, status) {
                $scope.Departments = '';
            });
    };
    $scope.GetOrganisations = function () {
        //  alert($scope.UserRegistrationSearch.DepartmentId);
        $scope.Levels = [];
        $scope.UserRegistrationSearch.OrganisationId = '0';
        $scope.Organisations = [];
        $scope.UserRegistrationSearch.OrganizationLevelsMappingID = '0';
        $scope.UserRegistrationSearch.OfficeId = '0';
        $scope.RoleListData = [];
        $scope.UserRegistrationSearch.RoleId = '0';
        //$http.get("/Master/OrganisationsList/?DepartmentId=" + $scope.UserRegistrationSearch.DepartmentId + "").
        $http.get(SiteUrl + "/Master/OrganisationsList/?DepartmentId=" + $scope.UserRegistrationSearch.DepartmentId).
            success(function (data1, status) {
                $(".se-pre-con").fadeIn("slow");
                //  alert(JSON.stringify(data1));
                $scope.Organisations = data1;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Organisations = '';
            });
    };
    $scope.GetLevels = function () {
        $scope.Levels = [];
        $scope.UserRegistrationSearch.OrganizationLevelsMappingID = '0';
        $scope.UserRegistrationSearch.OfficeId = '0';
        $scope.RoleListData = [];
        $scope.UserRegistrationSearch.RoleId = '0';
        $http.get(SiteUrl + "/MasterSettings/OrganizationLevelsList/?WhereClause=AND OLM.OrganizationId=" + $scope.UserRegistrationSearch.OrganisationId).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.Levels = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.GetLevels = '';
            });
    };
    $scope.GetOfficeList = function () {
        $scope.OfficeListData = [];
        $scope.UserRegistrationSearch.OfficeId = '0';
        $http.get(SiteUrl + "/Master/OfficesList/?WhereClause=and o.isactive =1 and   o.OrganizationLevelsMappingID=" + $scope.UserRegistrationSearch.OrganizationLevelsMappingID).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.OfficeListData = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                console.log("Error while fetching designation list");
                $scope.OfficeListData = [];
                // $scope.showLoader = false;
            });
    };
    $scope.GetRoleList = function () {
        $scope.RoleListData = [];
        $scope.UserRegistrationSearch.RoleId = '0';
        $http.get(SiteUrl + "/Master/GetRoleByOrganizationLevelsMappingID/?OrganizationLevelsMappingID=" + $scope.UserRegistrationSearch.OrganizationLevelsMappingID).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.RoleListData = data;
                $scope.showLoader = false;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while fetching Role list");
                $scope.showLoader = false;
            });
    };
    $scope.ShowUserSearchList = function () {
        $http.get(SiteUrl + "/User/GetUserDetailList").
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                if ($scope.UserRegistrationSearch.DepartmentId != '0') {
                    $scope.UserReqData = data.filter(x => x.DepartmentId == $scope.UserRegistrationSearch.DepartmentId);
                }
                if ($scope.UserRegistrationSearch.OrganisationId != '0') {
                    $scope.UserReqData = data.filter(x => x.OrganisationId == $scope.UserRegistrationSearch.OrganisationId);
                }
                if ($scope.UserRegistrationSearch.OrganizationLevelsMappingID != '0') {
                    $scope.UserReqData = data.filter(x => x.OrganizationLevelsMappingID == $scope.UserRegistrationSearch.OrganizationLevelsMappingID);
                }
                if ($scope.UserRegistrationSearch.OfficeId != '0') {
                    $scope.UserReqData = data.filter(x => x.OfficeId == $scope.UserRegistrationSearch.OfficeId);
                }
                if ($scope.UserRegistrationSearch.RoleId != '0') {
                    $scope.UserReqData = data.filter(x => x.RoleId == $scope.UserRegistrationSearch.RoleId);
                }
                $(".se-pre-con").fadeOut("slow");

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
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");

            });
        //$http.post(SiteUrl + "/User/GetUserDetailListByIds", { "DepartmentId": $scope.UserRegistrationSearch.DepartmentId, "OrganisationId": $scope.UserRegistrationSearch.OrganisationId, "OrganizationLevelsMappingID": $scope.UserRegistrationSearch.OrganizationLevelsMappingID, "OfficeId": $scope.UserRegistrationSearch.OfficeId, "RoleId": $scope.UserRegistrationSearch.RoleId }).
        //    success(function (data, status) {
        //        $scope.UserReqData = data
        //    }).
        //    error(function (data, status) {
        //    });
    };
    $scope.GetAllSubUnitList();
    $scope.GetDepartments();
    $scope.OnPageLoad();

});