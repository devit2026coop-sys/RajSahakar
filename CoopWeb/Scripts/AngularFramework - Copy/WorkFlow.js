var AppWorkFlowModule1 = angular.module('WorkFlowMod', ['angularUtils.directives.dirPagination']);
AppWorkFlowModule1.controller("WorkFlowCont", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 12;

    //START : Code for Language label setting
    $scope.getTranslation = function () {
        $scope._language = _language;
        if (_language === 'en-us') {
            $http.get(SiteUrl+"/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl+"/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
    };
    //END : Code for Language label setting

    $scope.pageChangeHandler = function (num) {
    };


    //START : Code for page level setting


    //on page load start

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl+"/WorkFlow/Getworkflow").
            success(function (data2, status) {
                if (data2[0].ActionID != "") {
                    var str = data2[0].ListUsers;
                    var str_array = str.split(',');
                    angular.forEach(str_array, function (Datavalue) {
                        if (Datavalue == 'Add') {
                            $scope.IsAdd = true;
                        }
                        else if (Datavalue == 'Edit') {
                            //$scope.tdEdit.style.display = "none";
                            $scope.IsEdit = true;
                        }
                        else if (Datavalue == 'Take Action') {
                            //$scope.tdEdit.style.display = "none";
                            $scope.IsTakeAction = true;
                        }
                        else if (Datavalue == 'Status') {
                            //$scope.tdEdit.style.display = "none";
                            $scope.IsStatus = true;
                        }
                        else if (Datavalue == 'delete') {
                            //$scope.tdEdit.style.display = "none";
                            $scope.IsDelete = true;
                        }
                        else {
                            $scope.IsAdd = false;
                            $scope.IsEdit = false;
                            $scope.IsTakeAction = false;
                            $scope.IsStatus = false;
                            $scope.IsDelete = false;
                        }
                    });

                }

            }).
            error(function (data1, status) {

                $scope.WorkFlowExecution = [];
                $(".se-pre-con").fadeOut("slow");
            });
        //on page load end

        // Get all Menus
        $http.get(SiteUrl + "/Menu/MenuList/?WhereClause=" + 'AND M.ParentId > 0 and M.MenuId in(33,47)').
            success(function (data1, status) {
                $scope.WorkFlowMenus = data1;
                alert($scope.WorkFlowMenus);
            }).
            error(function (data, status) {
                $scope.WorkFlowMenus = '';
                alert($scope.WorkFlowMenus);
                alert('Error While Menu Action Calling');
            });

        // Get all Roles 
        $http.get(SiteUrl+"/User/RoleList").
            success(function (data2, status) {
                $scope.WorkFlowRoleRoles = data2;
            }).
            error(function (data2, status) {
                $scope.WorkFlowRoleRoles = '';
            });

        // Get all Menu Actions 
        $http.get(SiteUrl+"/WorkFlow/WorkFlowAction/?WhereClause=" + 'AND A.LovListId=3').
            success(function (data3, status) {
                $scope.WorkFlowExecution = data3;
            }).
            error(function (data, status) {
                $scope.WorkFlowExecution = '';
            });

        $scope.LoadData();


    };
    //END : Code for page level setting
    // START : Load List of Work Flow

    $scope.LoadData = function () {
        // Work Flow List
        $http.get(SiteUrl+"/WorkFlow/WorkFlowList").
            success(function (data5, status) {
                $scope.WorkFlowDataList = data5;
            }).
            error(function (data, status) {
                alert('Error while Work Flow List load.');
            });
    }

    // START : Save Work Flow
    $scope.SaveWorkFlow = function () {
        var Msg = '';


        if ($scope.WorkFlow.MenuId == '0') {
            Msg += (_language == 'en-us' ? 'Please choose Menu\n' : 'कृपया मेनू चुनें\n');
        }
        if ($scope.WorkFlow.RoleId == '0') {
            Msg += (_language == 'en-us' ? 'Please choose Role\n' : 'कृपया भूमिका चुनें\n');
        }
        if ($scope.WorkFlow.ActionID == '0') {
            Msg += (_language == 'en-us' ? 'Please choose Action\n' : 'कृपया कार्रवाई चुनें\n');
        }
        if ($scope.WorkFlow.ActionID != 26 && $scope.WorkFlow.ActionID != 27) {
            if ($scope.WorkFlow.NextRoleId == '0') {
                Msg += (_language == 'en-us' ? 'Please choose Next User Role\n' : 'कृपया अगली भूमिका चुनें\n');
            }
        }

        if (Msg.length > 0) {
            alert(Msg);
            return;
        }
        LoaderStart();
        $http.post(SiteUrl+"/WorkFlow/WorkFlowConfiguration", $scope.WorkFlow).
            success(function (data, status) {
                //var _Response = angular.fromJson(data);
                if (data > 0) {
                    ShowModelAlert('WorkFlowConfiguration', $scope.translation.Save);
                    //$scope.WorkFlow = null;
                }
                else if (data == 0) {
                    ShowModelAlert('WorkFlowConfiguration', $scope.translation.Already);
                    // $scope.WorkFlow = null;
                }
                else if (data < 0) {
                    ShowModelAlert('WorkFlowConfiguration', $scope.translation.EditError);
                    //  $scope.WorkFlow = null;
                }
                else {
                    ShowModelAlert('WorkFlowConfiguration', 'Error Occerred !!');
                    // $scope.WorkFlow = null;
                }
                LoaderStop();
                $scope.WorkFlow.MenuId = '0';
                $scope.WorkFlow.RoleId = '0';
                $scope.WorkFlow.ActionID = '0';
                $scope.WorkFlow.NextRoleId = '0';
                $scope.WorkFlow.LevelNo = '';
            }).
            error(function (data, status) {
                LoaderStop();
                ShowModelAlert('WorkFlowConfiguration', 'Error while Work Flow Configuration !!');
            });
    };
    // END : Save Work Flow

    // START : Change Status
    $scope.SetStatus = function (_Id, _status) {
        if (_status == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        _status = (_status == 0 ? 1 : (_status == 1 ? 0 : _status));
        $http.post(SiteUrl+"/WorkFlow/ChangeWorkFlowStatus", { Id: _Id, Action: _status }).
            success(function (data, status) {

                if (parseInt(data, 10) > 0) {
                    if (_status != 2)
                        alert($scope.translation.StatusChange);
                    else
                        alert($scope.translation.Deleted);
                }
                $scope.LoadData();
            }).
            error(function (data, status) {
            });
    };
    // END : Change Status

    // START : Clear Form
    $scope.ClearForm = function () {
        $scope.WorkFlow.LevelNo = '';
    };
    // END : Clear Form

    // START : Update Work Flow 
    $scope.Update = function (table) {
        $scope.WorkFlow = table;
        $scope.WorkFlow.ActionID = table.ActionId;
        $scope.WorkFlow.LevelNo = table.Level;
        $scope.WorkFlow.IsActive1 = table.IsActive;

    };
    // END : Update Work Flow 


    // Call On Page Load
    $scope.getTranslation($scope);
    $scope.OnPageLoad();



});

function selectNextUserGroup(event) {
    if ($(event).find('option:selected').val() == '26' || $(event).find('option:selected').val() == '27') {
        $('#ddlNextRole').attr('disabled', 'disabled').val('0');

    }
    else {
        $('#ddlNextRole').removeAttr('disabled', 'disabled');
    }
}