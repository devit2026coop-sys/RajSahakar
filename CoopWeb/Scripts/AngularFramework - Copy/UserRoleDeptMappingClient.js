var AppScheme1 = angular.module("UserRoleDeptMapping", ['angularUtils.directives.dirPagination']);
AppScheme1.controller("UserRoleDept", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 50;

    // START : Code for Language label setting
    $scope.getTranslation = function () {
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
    };
    // END : Code for Language label setting


    //Get User List for dropdown
    $scope.UserList = function () {
        $http.get(SiteUrl + "/User/SelectUser").
            success(function (data, status) {

                $scope.Users = data;
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
                $scope.showLoader = false;

            }).
            error(function (data, status) {

                $scope.showLoader = false;
            });
    };


    //Get Department List for dropdown
    $scope.DepartmentList = function () {

        $http.get(SiteUrl + "/User/SelectDepartment").
            success(function (data, status) {

                $scope.Departments = data;
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
                $scope.showLoader = false;

            }).
            error(function (data, status) {

                $scope.showLoader = false;
            });
    };

    //Get Designation List for dropdown
    $scope.DesignationList = function () {
        $http.get(SiteUrl + "/User/SelectDesignation").
            success(function (data, status) {

                $scope.Designations = data;
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
                $scope.showLoader = false;

            }).
            error(function (data, status) {

                $scope.showLoader = false;
            });
    };

    $scope.toggleAll = function () {
        var toggleStatus = $scope.isAllSelected;
        angular.forEach($scope.AssignMoreRights, function (itm) { itm.Checked = toggleStatus; });

    }

    $scope.optionToggled = function () {
        $scope.isAllSelected = $scope.AssignMoreRights.every(function (itm) { return itm.Checked; })
    }

    //Get Post List for dropdown
    $scope.PostList = function () {
        $http.get(SiteUrl + "/User/SelectPost").
            success(function (data, status) {

                $scope.Posts = data;
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
                $scope.showLoader = false;

            }).
            error(function (data, status) {
                alert("Error while fetching Post list")
                $scope.showLoader = false;
            });
    };

    //Get Role List for dropdown
    $scope.RoleList = function () {
        $http.get(SiteUrl + "/User/SelectRole").
            success(function (data, status) {

                $scope.Roles = data;
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
                $scope.showLoader = false;

            }).
            error(function (data, status) {
                alert("Error while fetching Role list")
                $scope.showLoader = false;
            });
    };

    //Clear form
    $scope.ClearAssignMoreRightsForm = function () {

        $scope.AssignMoreRights = "0";
        $scope.SingleChoise = "0";
        $scope.IsVisible = false;

        $scope.UserRoleDeptMapping();

    };

    $scope.ClearUserRoleDeptMappingForm = function () {

        $scope.UserRoleDept.RoleID = '0';
        $scope.UserRoleDept.PostId = '0';
        $scope.UserRoleDept.DESIGNATIONCODE = '0';
        $scope.UserRoleDept.DEPTCODE = '0';
        $scope.UserRoleDept.UserId = '0';

        $scope.UserRoleDept.RoleID = "0";
        $scope.UserRoleDept.PostId = "0";
        $scope.UserRoleDept.DESIGNATIONCODE = "0";
        $scope.UserRoleDept.DEPTCODE = "0";
        $scope.UserRoleDept.UserId = "0";



    };

    $scope.AdduserRoleDeptClick = function () {
        $scope.SingleChoise = false;
        $scope.userrole = '';
        $scope.IsVisible = false;
        $scope.RoleList = null;
    };

    //Insert User Role Department Mapping
    $scope.SaveUserRoleDeptMapping = function () {
        var Msg = '';


        if ($scope.UserRoleDept.UserId == '0') {
            Msg += (_language == 'en-us' ? 'Please choose User\n' : 'कृपया उपयोगकर्ता चुनें\n');
        }
        if ($scope.UserRoleDept.DEPTCODE == '0') {
            Msg += (_language == 'en-us' ? 'Please choose Department Code\n' : 'कृपया विभाग कोड चुनें\n ');
        }
        if ($scope.UserRoleDept.DESIGNATIONCODE == '0') {
            Msg += (_language == 'en-us' ? 'Please choose designation Code\n' : 'कृपया पद कोड चुनें\n');
        }
        if ($scope.UserRoleDept.PostId == '0') {
            Msg += (_language == 'en-us' ? 'Please choose Post\n' : 'कृपया पोस्ट चुनें\n');
        }
        if ($scope.UserRoleDept.RoleID == '0') {
            Msg += (_language == 'en-us' ? 'Please choose Role\n' : 'कृपया भूमिका चुनें\n');
        }

        if (Msg.length > 0) {

            Msg = (_language == 'en-us' ? 'Please validate following details : \n' : 'कृपया निम्नलिखित विवरण मान्य करें : \n') + Msg;
            
            alert(Msg);
            return;
        }




        $http.post(SiteUrl + "/User/UserRoleDeptMappingInsert", $scope.UserRoleDept).
            success(function (data, status) {
                var k = data;
                var message = '';
                window.location = '/User/UserRoleDeptMapping';
            }).
            error(function (data, status) {
                alert("Error while UserRoleDeptMappingInsert")
            });
    }

    //Get User Role Department Mapping

    $scope.UserRoleDeptMapping = function () {
        $http.get(SiteUrl + "/User/GetUserRoleDeptMapping").
            success(function (data, status) {
                $scope.GetUserRoleDeptMapping = data;//data;

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
                alert("Error while GetUserRoleDeptMapping")
            });
    };


    $scope.pageChangeHandler = function (num) {
    };

    //Update Status
    $scope.SetStatus = function (MappingID, ActionId) {

        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;
        $http.post(SiteUrl + "/User/ChangeStatusUserRoleDept", { Id: MappingID, Action: ActionId }).
            success(function (data, status) {
                $scope.UserRoleDeptMapping();
                $scope.IsVisible = false;
            }).
            error(function (data, status) {
            });
    };


    $scope.updateURDM = function (item) {
        $scope.UserRoleDept.RoleID = item.RoleId;
        $scope.UserRoleDept.PostId = item.PostId;
        $scope.UserRoleDept.DESIGNATIONCODE = item.DESIGNATIONCODE;
        $scope.UserRoleDept.DEPTCODE = item.DEPTCODE;
        $scope.UserRoleDept.UserId = item.UserId;
        $scope.UserRoleDept.IsActive = item.IsActive;


    };
    $scope.SetUncheckStatus = function (AllMenurow, ActionId, check) {
        if (ActionId == 1 || ActionId == 0) {
            if (!check) {
                AllMenurow.MenuIdChecked = '0';
            }
            else if (check) {
                AllMenurow.MenuIdChecked = '1';

            }
            else {
                AllMenurow.MenuIdChecked = '0';
            }
        }
    };

    $scope.ShowHide = function () {

        $scope.IsVisible = $scope.Show;
    }

    $scope.AssignMoreRightsFunction = function (tableData) {
    $http.post(SiteUrl + "/User/AssignMoreRights", { WhereClause: ' IsActive !=2 and MappingId=' + tableData.MappingID }).
            success(function (data, status) {
                angular.forEach(data.ChildData, function (ChildDatavalue) {
                    var MenuIdChecked = 0;
                    var MappingID = 0;
                    angular.forEach(data.ParentData, function (ParentDatavalue, e) {
                        if (ChildDatavalue.MenuId == ParentDatavalue.MenuId) {
                            ParentDatavalue.MenuIdChecked = 1;
                            ParentDatavalue.MappingId = ChildDatavalue.MappingId;
                            data.ParentData[e] = ParentDatavalue;
                            var str = ChildDatavalue.ActionID;
                            var str_array = str.split(',');
                            angular.forEach(str_array, function (Datavalue) {
                                angular.forEach(ParentDatavalue.MenuActionList, function (val) {
                                    if (Datavalue == val.Key) {
                                        val.RoleMenuActionCheck = true;
                                    }
                                });
                            });
                        }
                    });
                }
                );

                $scope.DepartmentId = tableData.DEPTCODE;
                $scope.RoleId = tableData.RoleId;
                $scope.UserId = tableData.UserId;
                $scope.MappingID = tableData.MappingID;
                $scope.AssignMoreRights = data.ParentData;
                $scope.IsVisible = true;
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
                $scope.showLoader = false;
            }).
            error(function (data, status) {
                alert("Error while AssignMoreRightsFunction list.");
                $scope.showLoader = false;
            });

    };

    function UserRolemenuAction() {
        this.RoleId = '';
        this.MenuId = '';
        this.UserId = '';
        this.MenuAction = '';
        this.DepartmentId = '';
        this.MenuActive = 0;
        this.MappingID = '';
    }
    function RolemenuAction() {
        this.RoleId = '';
        this.RoleId = '';
        this.MenuId = '';
        this.MenuAction = '';
        this.IsActive = 0;
    }
    $scope.SaveAssignMoreRights = function () {
        var RolemenuActionCollection = [];
        var tempMenuActions = '';
        angular.forEach($scope.AssignMoreRights, function (value) {
            if (value.Checked || value.MenuIdChecked == '1') {
                var item = new UserRolemenuAction();
                item.MenuId = value.MenuId;
                item.UserId = $scope.UserId;
                item.IsActive = value.IsActive;
                item.RoleId = $scope.RoleId;
                item.MappingID = $scope.MappingID;
                item.DepartmentId = $scope.DepartmentId;
                tempMenuActions = '';
                item.MenuActive = 1;
                angular.forEach(value.MenuActionList, function (value2) {
                    if (value2.RoleMenuActionCheck == '1') {
                        tempMenuActions += value2.Key + ",";
                    }
                });
                item.MenuAction = tempMenuActions.substr(0, tempMenuActions.length - 1);
                RolemenuActionCollection.push(item);
            }
        });
        $http.post(SiteUrl + "/User/SaveAssignMoreRights", { list: RolemenuActionCollection }).
            success(function (data, status) {
                if (data.Status) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    window.location = '/User/UserRoleDeptMapping';
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    window.location = '/User/UserRoleDeptMapping';

                }


            }).
            error(function (data, status) {
                alert("Error while Assign Rights insert");
            });
    };

    $scope.changeOption = function () {
        $scope.submitted = false;
    };
    $scope.getTranslation($scope);
    $scope.UserList();
    $scope.DepartmentList();
    $scope.DesignationList();
    $scope.PostList();
    $scope.RoleList();
    $scope.UserRoleDeptMapping();
    $scope.IsVisible = false;


});
