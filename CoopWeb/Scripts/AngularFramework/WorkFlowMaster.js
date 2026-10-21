var WorkFlowMasterModule = angular.module("WorkFlowMasterMod", ['angularUtils.directives.dirPagination']);
WorkFlowMasterModule.controller("WorkFlowMasterCnt", function ($scope, $http) {
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.rows = [1];
    $scope.counter = 0;
    $scope.lstWorkFlowScope = [];
    $scope.isonerow = true;
    $scope.isForm = true;
    $scope.WorkFlowMaster = [];

    $scope.OnPageLoad = function () {
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

            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }

        $scope.ModuleList();
        $scope.LevelList();
        $scope.ActionHeadList();
        $scope.ActionList();
        $scope.WorkFlowConfigList();
        //$scope.GetDepartments();
    };
    $scope.ViewDetails = function () {
        $scope.isForm = false;
        $scope.ClearWorkFLowForm();
    };

    $scope.GoBack = function () {
        $scope.isForm = true;
    };

    //$scope.GetDepartments = function () {
    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DEPARTMENT&MasterCode=-1").
    //        success(function (data, status) {
    //            $scope.Departments = data;
    //        }).
    //        error(function (data, status) {
    //            $scope.Departments = '';
    //        });
    //};

    //$scope.GetSchemeName = function () {
    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SCHEMENAME&MasterCode=" + $scope.WorkFlowMaster.DepartmentId).
    //        success(function (data, status) {
    //            $scope.SchemeNames = data;
    //        }).
    //        error(function (data, status) {
    //            $scope.SchemeNames = '';
    //        });
    //};

    $scope.ClearWorkFLowForm = function () {
        $scope.WorkFlowMaster.DepartmentId = '0';
        $scope.SchemeNames = [];
        $scope.WorkFlowMaster.ModuleNameEn = '0';
        $scope.SubModuleData = [];
        $scope.WorkFlowMaster.LevelNameEn = '0';
        $scope.Roles = [];
        //$("#tblgetsuport").find("tr:gt(1)").remove();
        $scope.rows = [1];
        $scope.WorkFlowMaster.ID = 0;
    };

    $scope.UpdateWorkFlow = function (item) {
        //$scope.WorkFlowMaster.DepartmentId = item.DepartmentId;
        //$scope.GetSchemeName();
        //$scope.WorkFlowMaster.SchemeNameId = item.SchemeNameId;
        $scope.WorkFlowMaster.ModuleNameEn = item.moduleid;
        $scope.SubModuleList();
        $scope.WorkFlowMaster.SubModuleNameEn = item.submoduleid;
        $scope.WorkFlowMaster.LevelNameEn = item.LevelID;
        $scope.RoleList();
        $scope.WorkFlowMaster.RoleName_EN = item.roleid;
        $scope.WorkFlowMaster.ID = item.workflowmasterid;
        //$scope.GetStageList();
        $http.get(SiteUrl + "/WorkFlowMaster/WorkFlowDetails?workflowmasterid=" + item.workflowmasterid).
            success(function (data, status) {
                $scope.RoleList();

                $('#tblgetsuport tbody').find("tr:not(:first)").remove();
                //$("#DDLACTID1").val(data[0].actionid);
                //$("#actioncode1").val(data[0].actioncode);
                //$("#DDLROLEID1").val(data[0].nextroleid);
                //$("#actionpriority1").val(data[0].actionpriority);
                for (var i = 1; i < data.length; i++) {
                    $scope.addRow();
                }


                setTimeout(function () {
                    for (var j = 1; j <= data.length; j++) {
                        var data1 = data[j - 1];
                        $("#DDLACTHID" + j).val(data1.actionHeadId);
                        $("#DDLACTID" + j).val(data1.actionid);
                        $("#actioncode" + j).val(data1.actioncode);
                        $("#ddllevel" + j).val(data1.LevelId);
                        $("#DDLOfficeGroupID" + j).val(data1.OfficeId);
                        $scope.RoleListSetNextRoleId(j, data1.nextroleid);                        
                        $("#actionpriority" + j).val(data1.actionpriority);
                        $("#completiondays" + j).val(data1.CompletionDays);
                    }
                    $scope.WorkFlowMaster.Stage = item.Stageid.toString();
                }, 1000);



                //$("#DDLROLEID2").val(data[1].nextroleid);
                //$("#actionpriority2").val(data[1].actionpriority);
                //for (var i = 2; i <= data.length; i++) {
                //    $('#tblgetsuport tbody').append($('#tblgetsuport tbody tr:last').clone());
                //    $('#tblgetsuport tbody tr:last').find('.DDLACTID').val(data[i - 1].actionid);
                //    $('#tblgetsuport tbody tr:last').find('.actioncode').val(data[i - 1].actioncode);
                //    $('#tblgetsuport tbody tr:last').find('.actionpriority').val(data[i - 1].actionpriority);
                //    $('#tblgetsuport tbody tr:last').find('.DDLROLEID').val(data[i - 1].nextroleid);
                //    $('#tblgetsuport tbody tr:last').find('.DDLROLEID option[value=' + data[i - 1].nextroleid+']').attr('selected', 'selected');
                //}
                //$('#tblgetsuport tbody tr').each(function (i) {
                //    $(this).closest('tr').find('td:eq(0)').text(parseInt(i + 1));
                //});
            }).
            error(function (data, status) {
                alert("Error while work flow details list.");
                $scope.WorkFlowDetails = [];

            });
        $scope.isForm = true;

    };

    $scope.clonerow = function () {
        $scope.isonerow = false;
        $('#tblgetsuport tbody').append($('#tblgetsuport tbody tr:last').clone());

        $('#tblgetsuport tr').each(function (i) {
            if (i === 1)
                return;
            $(this).find('td').eq(0).html(i);
            var selectinput = $(this).find('.DDLACTID');
            var textinput = $(this).find('.actioncode');
            var selectinput1 = $(this).find('.DDLROLEID');
            var textinput1 = $(this).find('.actionpriority');
            var span1 = $(this).find('.deleterow');
            selectinput.eq(0).attr('id', 'DDLACTID' + i);
            textinput.eq(0).attr('id', 'actioncode' + i);
            selectinput1.eq(0).attr('id', 'DDLROLEID' + i);
            textinput1.eq(0).attr('id', 'actionpriority' + i);
            span1.eq(0).attr('id', 'deleterow' + i);
            //span1.eq(0).attr('ng-click', 'removeItem('+i-1+')');
            //$scope.RoleList();
            var ddHTML;
            ddHTML = _language == 'en-us' ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
            angular.forEach($scope.Roles, function (item) {
                if (_language == 'en-us') {
                    ddHTML += '<option value="' + (item.RoleID) + '">' + item.RoleName_EN + '</option>';
                }
                else {
                    ddHTML += '<option value="' + (item.RoleID) + '">' + item.RoleName_HI + '</option>';
                }
            });

            $("#DDLROLEID" + i).html(ddHTML);
        });

    };
    $scope.WorkFlowConfigList = function () {
        $http.get(SiteUrl + "/WorkFlowMaster/WorkFlowConfigList").
            success(function (data, status) {
                $scope.WorkFlowConfigData = [];
                $scope.WorkFlowConfigData = data;
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
                alert("Error while module list.");
                $scope.WorkFlowConfigData = [];

            });
    };


    $scope.ModuleList = function () {
        $scope.WorkFlowMaster.Stage = '0';
        $scope.WorkFlowMaster.SubModuleNameEn = '0';
        $http.get(SiteUrl + "/WorkFlowMaster/ModuleList").
            success(function (data, status) {
                $scope.ModuleData = [];
                $scope.ModuleData = data;

            }).
            error(function (data, status) {
                alert("Error while module list.");
                $scope.ModuleData = [];

            });
    };


    $scope.SubModuleList = function () {
        var MID = $('#ddlmodule').val();
        if (MID == null) {
            MID = $scope.WorkFlowMaster.ModuleNameEn;
        }
        $(".se-pre-con").fadeIn("slow");
        $scope.SubModuleData = null;
        $http.post(SiteUrl + "/WorkFlowMaster/SubModuleList", { WhereClause: MID }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.SubModuleData = [];
                $scope.SubModuleData = data;
            }).
            error(function (data, status) {
                alert("Error while submodule list.");
                $scope.SubModuleData = [];
            });

        if (MID == 1) {
            $scope.GodownActionList();
        }
    };
    $scope.GodownActionList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.ActionListData = null;
        $http.get(SiteUrl + "/WorkFlowMaster/GodownActionList").
            success(function (data, status) {
                //$scope.counter = data.length;
                $(".se-pre-con").fadeOut("slow");
                $scope.ActionListData = [];
                $scope.ActionListData = data;
            }).
            error(function (data, status) {
                alert("Error while Godown Action list.");
                $scope.ActionListData = [];
            });

    };
    $scope.ActionList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.ActionListData = null;
        $http.get(SiteUrl + "/WorkFlowMaster/ActionList").
            success(function (data, status) {
                //$scope.counter = data.length;
                $(".se-pre-con").fadeOut("slow");
                $scope.ActionListData = [];
                $scope.ActionListData = data;
            }).
            error(function (data, status) {
                alert("Error while Action list.");
                $scope.ActionListData = [];
            });

    };
    $scope.GodownActionList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.ActionListData = null;
        $http.get(SiteUrl + "/WorkFlowMaster/GodownActionList").
            success(function (data, status) {
                //$scope.counter = data.length;
                $(".se-pre-con").fadeOut("slow");
                $scope.ActionListData = [];
                $scope.ActionListData = data;
            }).
            error(function (data, status) {
                alert("Error while Action list.");
                $scope.ActionListData = [];
            });

    };
    $scope.ActionHeadList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.ActionHeadListData = null;
        $http.get(SiteUrl + "/WorkFlowMaster/ActionHeadList").
            success(function (data, status) {
                //$scope.counter = data.length;
                $(".se-pre-con").fadeOut("slow");
                $scope.ActionHeadListData = [];
                $scope.ActionHeadListData = data;
            }).
            error(function (data, status) {
                alert("Error while Action list.");
                $scope.ActionHeadListData = [];
            });

    };

    $scope.RoleList = function () {
        var MID = $('#ddllevel').val();
        if (MID == null) {
            MID = $scope.WorkFlowMaster.LevelNameEn;
        }
        $http.get(SiteUrl + "/User/RoleList/?WhereClause=" + 'and Role.LevelID=' + MID).
            success(function (data, status) {
                $scope.Roles = [];
                $scope.Roles = data;

            }).
            error(function (data, status) {
                alert("Error while fetching role list.");
                $scope.Roles = [];
            });
    };
    $scope.GetStageList = function () {
        $scope.stage = [];
        $http.get(SiteUrl + "Master/GetMasterData/?MasterType=AGRILOV_VALUES&MasterCode=WorkflowMasterStage").
            success(function (data, status) {
                $scope.stage = data
            }).
            error(function (data, status) {
                alert("Error while fetching Stage list.");
                $scope.stage = [];
            });
    }

    $scope.RoleListE = function (i) {

        var MID = $('#ddllevel' + i).val();

        $http.get(SiteUrl + "/User/RoleList/?WhereClause=" + 'and Role.LevelID=' + MID).
            success(function (data, status) {
                var ddHTML = _language == 'en-us' ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
                angular.forEach(data, function (item) {
                    if (_language == 'en-us') {
                        ddHTML += '<option value="' + (item.RoleID) + '">' + item.RoleName_EN + '</option>';
                    }
                    else {
                        ddHTML += '<option value="' + (item.RoleID) + '">' + item.RoleName_HI + '</option>';
                    }
                });
                $("#DDLROLEID" + i).html(ddHTML);

            }).
            error(function (data, status) {
                alert("Error while fetching role list.");
                $scope.RolesE = [];
            });
    };
    $scope.RoleListSetNextRoleId = function (i, NextRoleId) {

        var MID = $('#ddllevel' + i).val();

        $http.get(SiteUrl + "/User/RoleList/?WhereClause=" + 'and Role.LevelID=' + MID).
            success(function (data, status) {
                var ddHTML = _language == 'en-us' ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
                angular.forEach(data, function (item) {
                    if (NextRoleId == item.RoleID) {
                        if (_language == 'en-us') {
                            ddHTML += '<option value="' + (item.RoleID) + '" selected>' + item.RoleName_EN + '</option>';
                        }
                        else {
                            ddHTML += '<option value="' + (item.RoleID) + '" selected>' + item.RoleName_HI + '</option>';
                        }
                    }
                    else {
                        if (_language == 'en-us') {
                            ddHTML += '<option value="' + (item.RoleID) + '">' + item.RoleName_EN + '</option>';
                        }
                        else {
                            ddHTML += '<option value="' + (item.RoleID) + '">' + item.RoleName_HI + '</option>';
                        }
                    }
                });
                $("#DDLROLEID" + i).html(ddHTML);

            }).
            error(function (data, status) {
                alert("Error while fetching role list.");
                $scope.RolesE = [];
            });
    };

    $scope.LevelList = function () {
        $http.get(SiteUrl + "/WorkFlowMaster/LevelList").
            success(function (data, status) {
                $scope.LevelData = [];
                $scope.LevelData = data;

            }).
            error(function (data, status) {
                alert("Error while fetching Level list.");
                $scope.LevelData = [];
            });
    };

    $scope.addrolemap = function (MID) {
        $scope.lstWorkFlowScope = [];
        $('#tblgetsuport tbody tr').each(function () {
            $scope.lstWorkFlowScope.push(
                {
                    "workflowmasterid": MID,
                    "actioncode": $(this).find('.actioncode').val() == null ? '' : $(this).find('.actioncode').val(),
                    "actionid": $(this).find('.DDLACTID').val() == null ? 0 : $(this).find('.DDLACTID').val(),
                    "actionheadid": $(this).find('.DDLACTHID').val() == null ? 0 : $(this).find('.DDLACTHID').val(),
                    "levelid": $(this).find('.ddllevel').val() == null ? 0 : $(this).find('.ddllevel').val(),
                    "nextroleid": $(this).find('.DDLROLEID').val() == null ? 0 : $(this).find('.DDLROLEID').val(),
                    "actionpriority": $(this).find('.actionpriority').val() == null ? '' : $(this).find('.actionpriority').val(),
                    "OfficeId": $(this).find('.DDLOfficeGroupID').val() == null ? 0 : $(this).find('.DDLOfficeGroupID').val(),
                    "completiondays": ($(this).find('.completiondays').val() == null || $(this).find('.completiondays').val() == "") ? '0' : $(this).find('.completiondays').val()
                }
            );
        }
        );
    };

    $scope.SetStatusWorkFlow = function (ID, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/WorkFlowMaster/ChangeStatusWorkflow", { ID: ID, Action: ActionId }).
            success(function (data, status) {

                $scope.WorkFlowConfigList('AND 1=1');

            }).
            error(function (data, status) {
            });
    };

    $scope.GetWorkflowDetails = function (workflowmasterid) {
        $http.get(SiteUrl + "/WorkFlowMaster/WorkFlowDetails?workflowmasterid=" + workflowmasterid).
            success(function (data, status) {
                $scope.WorkFlowDetails = [];
                $scope.WorkFlowDetails = data;
            }).
            error(function (data, status) {
                alert("Error while work flow details list.");
                $scope.WorkFlowDetails = [];

            });
    };

    $scope.SaveWorkFlow = function () {
        if ($scope.ValidateDetails()) {
            if ($scope.ValidateDetailsNew()) {
                var formData = new FormData();
                var MID = $('#ddlmodule').val();
                var SMID = $('#ddlsubmodule').val();
                var RID = $('#ddlrole option:selected').val() == null ? '' : $('#ddlrole option:selected').val();
                formData.append("Workflowmasterid", $scope.WorkFlowMaster.ID);
                formData.append("SchemeNameId", $scope.WorkFlowMaster.SchemeNameId);
                formData.append("ModuleId", MID);
                formData.append("SubModuleId", SMID);
                formData.append("Stage", $("#ddlstage").val() );
                formData.append("LevelId", $scope.WorkFlowMaster.LevelNameEn);
                formData.append("Roleid", RID);
                $http.post(SiteUrl + "/WorkFlowMaster/SaveWorkFlow", formData,
                    {
                        withCredentials: true,
                        headers: { 'Content-Type': undefined },
                        transformRequest: angular.identity
                    }).
                    success(function (data, status) {
                        var k = data;
                        if (k > 0) {
                            message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                            $scope.addrolemap(k);
                            $scope.SaveWorkFlowDetails();
                            return;
                        }
                        else if (k == -2) {
                            message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
                        }
                        else {
                            message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                        }
                        bootbox.alert(message, function () {
                            location.reload();
                        })


                    }).
                    error(function (data, status) {
                        alert("Error while case registration insert");
                        $scope.ClearCaseRegister();
                        $scope.SubformShow = false;
                    });
            }
        }
    };

    $scope.ValidateDetailsNew = function () {
        var mesg = "";
        $('#tblgetsuport tbody tr').each(function () {
            if ($(this).find(".DDLACTID").val() == '' || $(this).find(".DDLACTID").val() == undefined) {
                mesg += $scope._language == 'en-us' ? '\nPlease select action' : '\nकृपया एक्शन चुनें|';
                mesg += '<br />';
            }
            if ($(this).find(".actioncode").val() == '' || $(this).find(".actioncode").val() == undefined) {
                mesg += $scope._language == 'en-us' ? '\nPlease enter list code' : '\nकृपया सूची कोड दर्ज करें|';
                mesg += '<br />';
            }
            if ($(this).find(".DDLROLEID").val() == '' || $(this).find(".DDLROLEID").val() == undefined || $(this).find(".DDLROLEID").val() == '0' || $(this).find(".DDLROLEID").val() == 0) {
                mesg += $scope._language == 'en-us' ? '\nPlease select next role' : '\nकृपया अगली भूमिका का चयन करें|';
                mesg += '<br />';
            }
            if ($(this).find(".actionpriority").val() == '' || $(this).find(".actionpriority").val() == undefined) {
                mesg += $scope._language == 'en-us' ? '\nPlease enter priority' : '\nकृपया प्राथमिकता दर्ज करें|';
                mesg += '<br />';
            }
        });

        if (mesg.length > 10) {
            ShowModelAlert("Role Map Details", $scope._language == 'en-us' ? '\nPlease fill all role map details' : '\nकृपया सभी भूमिका मानचित्र विवरण भरें|');
            return false;
        }

        return true;
    }

    $scope.ValidateDetails = function () {
        var message = "";
        if ($('#ddlmodule').val() == '' || $('#ddlmodule').val() == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease select Module' : '\nकृपया मॉड्यूल का चयन करें|';
            message += '<br />';
        }
        if ($('#ddlsubmodule').val() == '' || $('#ddlsubmodule').val() == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease select Sub Module' : '\nकृपया उप मॉड्यूल का चयन करें|';
            message += '<br />';
        }
        if ($('#ddlmodule').val() == 8) {
            if ($("#ddlstage").val() == 0 || $('#ddlstage').val() == undefined) {
                message += $scope._language == 'en-us' ? '\nPlease select Stage' : '\nकृपया चरण का चयन करें|';
                message += '<br />';
            }
        }
       
        if ($('#ddllevel').val() == '' || $('#ddllevel').val() == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease select role level' : '\nकृपया भूमिका का स्तर चुनें|';
            message += '<br />';
        }
        if ($('#ddlrole').val() == '' || $('#ddlrole').val() == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease select role' : '\nकृपया भूमिका चुनें|';
            message += '<br />';
        }

        if (message.length > 10) {
            ShowModelAlert("Role Map Details", message);
            return false;
        }

        return true;
    }


    $scope.SaveWorkFlowDetails = function () {
        var formData = new FormData();
        formData.append("lstWorkFlowScope", JSON.stringify($scope.lstWorkFlowScope));
        $http.post(SiteUrl + "/WorkFlowMaster/SaveWorkFlowDetails", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                var k = data;
                if (k > 0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                }
                else if (k == -2) {
                    message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                }
                bootbox.alert(message, function () {
                    location.reload();
                })
            }).
            error(function (data, status) {
                alert("Error while Menu insert");
            });
    };






    //$scope.addRow = function () {
    //    //$scope.counter = $('#tblgetsuport tbody tr').length - 1;
    //    //$scope.counter = $scope.counter + 1;

    //    //console.log($scope.rows);
    //    $scope.clonerow();
    //    $scope.rows.push($('#tblgetsuport tbody tr').length-1);
    //    $scope.isonerow = false;
    //};


    $scope.addRow = function () {
        //$scope.counter = $('#tblgetsuport tbody tr').length;
        $scope.counter = $scope.rows.length;
        $scope.rows.push($scope.counter + 1);
        console.log($scope.rows);
        $scope.isonerow = false;
    };

    $scope.removeItem = function (x) {
        $scope.rows.splice(x, 1);
    }

    //END : Call Functions on Page Load

    $scope.OnPageLoad();
});