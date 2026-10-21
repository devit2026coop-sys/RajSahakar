
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


    //$scope.SaveUserReqAppForm = function () {
    //    //alert("call btn submit");
    //    debugger;
    //    var Ids = '';
    //    angular.forEach($scope.UserReqData, function (item) {
    //        debugger;
    //        if (item.IsChecked) {
    //            Ids += item.UserId + ',';
    //        }
    //    });

    //    Ids = Ids.substr(0, Ids.length - 1);

    //    var IdsRej = '';
    //    angular.forEach($scope.UserReqData, function (item) {
    //        if (item.IsChecked2) {
    //            IdsRej += item.UserId + ',';
    //        }
    //    });

    //    IdsRej = IdsRej.substr(0, IdsRej.length - 1);




    //    //{ "ClassNameEn": $scope.UserRegistrationAppr.UnitNameEn, "ClassNameHi": $scope.UserRegistrationAppr.UnitNameHi, "IsActive": $scope.UserRegistrationAppr.IsActive, "UnitId": $scope.UserRegistrationAppr.UnitId, "UnitId": $scope.UserRegistrationAppr.DepartmentId}).
    //    $http.post("/User/SaveUserReqAppForm", { "Ids": Ids, "IdsRej": IdsRej }).
    //        alert("call2");
    //    success(function (data, status) {



    //        if (Ids > 0) {
    //            alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n')
    //                || $scope.ClearUnitForm();
    //        }
    //        else {
    //            alert(_language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n') || $scope.ClearUnitForm();
    //            return;
    //        }
    //        if ($scope.ParentId == '') {
    //            $scope.ParentId = 0;
    //        }

    //        $scope.GetAllSubUnitList();
    //        if (data.Status) {

    //            $scope.UserRegistrationAppr = null;
    //            $scope.ClearUnitForm();
    //            //$scope.GetAllSubClassesList();


    //        }
    //    }).
    //        error(function (data, status) {
    //            alert("Error while subclass insert");
    //        });


    //};

    //$scope.AllChecked = function () {

    //    var Ids = '';
    //    angular.forEach($scope.UserReqData, function (item) {
    //        if (item.IsChecked) {


    //            Ids += item.UserId + ',';
    //        }
    //    });

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
                    $("#ddlAct").append($("<option></option>").val(this.ActId).html(this.ActNameEn));
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
        if (DistId != 0) {
            $http.get(SiteUrl + "/Master/ResionMappingByDistIdOrganizationLevelsMappingID/?OrganizationLevelsMappingID=" + OrganizationLevelsMappingID + "&DistrictId=" + DistId + "&MappingID=" + MappingID).
                success(function (data, status) {
                    $("#ddlPrimaryResion").empty();
                    $.each(data, function () {
                        if (this.UserRoleResionMappingId == "")
                            $("#ddlPrimaryResion").append($("<option></option>").val(this.ResionId).html(this.ResionName));
                        else
                            $("#ddlPrimaryResion").append($("<option selected='selected'></option>").val(this.ResionId).html(this.ResionName));
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
                    $("#ddlPrimaryResion").empty();
                    // $('#ddlPrimaryResion').multiselect('destroy');
                });
        }
        else {
            $("#ddlPrimaryResion").empty();
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
        var ActIds = $('select#ddlAct').val();
        if (ActIds == "" || ActIds == undefined) {
            alert("Please select Act");
            return;
        }
        else {
            $scope.SaveUserRegiApprForm();
        }
        //    }
        //}
        //else {
        //    $scope.SaveUserRegiApprForm();
        //}
    };
    $scope.SaveUserRegiApprForm = function () {
        $scope.WorkActId = $('select#ddlAct').val();
        var PrimaryResionIdLocal = $('select#ddlPrimaryResion').val();
        var DairyResionIdLocal = "";
        // alert(PrimaryResionIdLocal + "----" + DairyResionIdLocal);
        $http.post(SiteUrl + "/User/UpdateUserRoleForm", { MappingID: $scope.UserRegistrationAppr.MappingID, ActId: $scope.WorkActId.toString(), RequestType: $scope.RequestType, PrimaryResionId: PrimaryResionIdLocal.toString(), DairyResionId: DairyResionIdLocal.toString() }).
            success(function (data, status) {
                if (data[0].ExistFlag.indexOf("-") > -1) {
                    alert("User " + data[0].OfficerNameEn + " (SSO Id is " + data[0].SSO_ID + ")" + " already present.");
                } else if (data[0].ExistFlag.indexOf("0") > -1) {
                    alert("Data is not available.");
                } else if (data[0].ExistFlag.indexOf("1") > -1) {
                    alert("Record saved successfully.");
                } 
                window.location = '../User/UserDetailList';
                // $scope.GetAllSubUnitList();
            }).
            error(function (data, status) {
            });
    }
    $scope.GetAllSubUnitList = function () {
        $http.get(SiteUrl + "/User/GetUserDetailList").
            success(function (data, status) {
                $scope.UserReqData = data
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

    $scope.update = function (item) {
        $(".se-pre-con").fadeIn("slow");
        $scope.UserRegistrationAppr.MappingID = item.MappingID;
        $scope.UserRegistrationAppr.LevelId = item.LevelId;
        //alert(item.ActId);
        if (item.ActId.indexOf(',') !== -1) {
            $.ActIdArr = item.ActId.split(',');
            $.each($.ActIdArr, function (intIndex, objValue) {
                $('#ddlAct option[value=' + objValue + ']').attr('selected', true);
            });
        }
        else {
            $('#ddlAct option[value=' + item.ActId + ']').attr('selected', true);
        }

        $scope.RoleList();
        $scope.GetUserReqDetail();
    };

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
    $scope.GetAllSubUnitList();

});






