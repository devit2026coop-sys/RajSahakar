var Role = angular.module("UserRole", ['angularUtils.directives.dirPagination']);

Role.directive('onlyLettersInput', onlyLettersInput);

function onlyLettersInput() {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                var transformedInput = text.replace(/[^a-zA-Z ]/g, '');
                if (transformedInput !== text) {
                    ngModelCtrl.$setViewValue(transformedInput);
                    ngModelCtrl.$render();
                }
                return transformedInput;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
};

Role.controller("Role", function ($scope, $http, $window) {

    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    var Mdata = [];
    //Insert user role details in db
    $scope.SaveUserRole = function () {

        $http.post(SiteUrl + "/User/SaveUserRole", $scope.userrole).
            success(function (data, status) {
                window.location = '../User/UserRole';

                var _Response = angular.fromJson(data);
                if (_Response.data > 0) {
                    $scope.userrole = null;
                    window.location = '../User/UserRole';



                }
            }).
            error(function (data, status) {
                alert("Error while role insert");
            });
    };


    $scope.pageChangeHandler = function (num) {
    };



    // START : Code for Language label setting
    $scope.getTranslation = function () {
        $scope.language = _language;
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

    //Start: Get Unit List
    $scope.LevelList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=LEVELS&MasterCode=0").
            success(function (data, status) {
                $scope.LevelListItem = data;
            }).
            error(function (data, status) {
                $scope.Level = [];
            });
    };
    //End 

    // Get List of all Role
    $scope.RoleList = function (WhereClause) {
        $http.get(SiteUrl + "/User/RoleList/?WhereClause=" + WhereClause).
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
                alert("Error while fetching role list.");
                $scope.showLoader = false;
            });
    };


    $scope.RoleListByLevel = function () {
        $scope.RoleList('and Role.LevelID=' + $scope.userrole.LevelID);
    };
    $scope.MenuList = function () {
        $http.get(SiteUrl +"/Menu/MenuList").
            success(function (data, status) {
                $scope.Menus = data;//data;
            }).
            error(function (data, status) {
                alert("Error while fetching menu list");
            });
    };
    $scope.MenuListbyparent = function () {
            $scope.AssignRights(Mdata);
    };



    $scope.toggleAll = function () {
        var toggleStatus = $scope.isAllSelected;
        angular.forEach($scope.AllMenuList, function (itm) { itm.Checked = toggleStatus; });

    }

    $scope.optionToggled = function () {
        $scope.isAllSelected = $scope.AllMenuList.every(function (itm) { return itm.Checked; })
    }


    //Clear user Role
    $scope.ClearRoleForm = function () {
        $scope.userrole = '';
        $scope.SingleChoise = false;
        $scope.userrole.LevelID = '0';


    };

    $scope.ClearAssignRightsForm = function () {
        window.location = '../User/UserRole';
    };

    $scope.AddRoleClick = function () {
        $scope.SingleChoise = false;
        $scope.userrole = '';
        $scope.IsVisible = false;
        $scope.userrole.LevelID = '0';
        $scope.RoleList('and 1=1');
    };
    // START : Get Data for Update
    $scope.update = function (tableData) {
        $scope.userrole = tableData;
    };

    $scope.SetStatus = function (RoleID, ActionId) {

        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/User/ChangeRoleStatus", { Id: RoleID, Action: ActionId }).
            success(function (data, status) {
                $scope.RoleList('and 1=1');
            }).
            error(function (data, status) {
            });
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

    $scope.roleid = '';
    $scope.AssignRights = function (tableData) {
        Mdata = tableData;
        $.ajax({
            type: "POST",
            url: "../User/RoleRightsList",
            data: { WhereClause: ' and IsActive !=2 AND RoleId = ' + tableData.RoleID, MenuID: $scope.menu.ParentId },
            datatype: "json",
            success: function (data) {
                var data = $.parseJSON(data);

                angular.forEach(data.ChildData, function (ChildDatavalue) {
                    var MenuIdChecked = 0;

                    angular.forEach(data.ParentData, function (ParentDatavalue) {

                        if (ChildDatavalue.MenuId == ParentDatavalue.MenuId) {
                            ParentDatavalue.MenuIdChecked = 1;

                            //
                            var str = ChildDatavalue.ActionID;
                            if (str == undefined) { return; }
                            var str_array = str.split(',');
                            angular.forEach(str_array, function (Datavalue) {
                                angular.forEach(ParentDatavalue.MenuActionList, function (val) {
                                    if (Datavalue == val.Key) {
                                        val.RoleMenuActionCheck = true;
                                    }
                                });

                            });
                            //

                        }
                    });

                });


                $scope.RoleId = tableData.RoleID;
                $scope.AllMenuList = data.ParentData;

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
                $("#btnShow").click();
            },
            failure: function (response) {
                alert("Error...");
            }
        });

    };




    $scope.ShowHide = function () {
        $scope.IsVisible = $scope.Show;
    }

    $scope.btnShowClick = function () {
    }

    function RolemenuAction() {
        this.RoleId = '';
        this.MenuId = '';
        this.MenuAction = '';
        this.IsActive = 0;
    }

    $scope.SaveAssignRights = function () {

        var RolemenuActionCollection = [];
        var tempMenuActions = '';
        angular.forEach($scope.AllMenuList, function (value) {
            if (value.Checked || value.MenuIdChecked == '1') {
                var item = new RolemenuAction();
                item.MenuId = value.MenuId;
                item.IsActive = value.IsActive;
                item.RoleId = $scope.RoleId;
                tempMenuActions = '';
                angular.forEach(value.MenuActionList, function (value2) {
                    if (value2.RoleMenuActionCheck == '1') {
                        tempMenuActions += value2.Key + ",";
                    }
                });
                item.MenuAction = tempMenuActions.substr(0, tempMenuActions.length - 1);
                RolemenuActionCollection.push(item);
            }
        });

        $http.post(SiteUrl + "/User/SaveAssignRights", { list: RolemenuActionCollection }).
            success(function (data2, status) {
                if (data2.Status) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    window.location = '../User/UserRole';
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    window.location = '../User/UserRole';

                }


            }).
            error(function (data, status) {
                alert("Error while Assign Rights insert");
            });


    };
    //on page load start

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $(".se-pre-con").fadeOut("slow");

    }
    //on page load end
    //Get list of all users on page list
    $scope.getTranslation();
    $scope.IsVisible = false;
    $scope.RoleList('and 1=1');
    $scope.OnPageLoad();
    $scope.LevelList();
    $scope.MenuList();
    //$scope.SingleChoise = false;


});