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
    $scope._language = 'en-us';
    var Mdata = [];
    //Insert user role details in db
    $scope.SaveUserRole = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/User/SaveUserRole", $scope.userrole).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                window.location = '../User/UserRole';

                var _Response = angular.fromJson(data);
                if (_Response.data > 0) {
                    $scope.userrole = null;
                    window.location = '../User/UserRole';



                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
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
        $(".se-pre-con").fadeIn("slow");
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

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
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
        //$scope.isAllSelected = $scope.AllMenuList.every(function (itm) { return itm.Checked; })
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
                $(".se-pre-con").fadeIn("slow");
                $scope.RoleList('and 1=1');
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };





    $scope.roleid = '';
    $scope.AssignRights1 = function (tableData) {
        $(".se-pre-con").fadeIn("slow");
        Mdata = tableData;
        $.ajax({
            type: "POST",
            url: "../User/RoleRightsList",
            data: { WhereClause: ' and IsActive !=2 AND RoleId = ' + tableData.RoleID, MenuID: $scope.menu.ParentId },
            datatype: "json",
            success: function (data) {
                $(".se-pre-con").fadeIn("slow");
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
                $(".se-pre-con").fadeOut("slow");
            },
            failure: function (response) {
                alert("Error...");
            }
        });

    };
    $scope.AllMenus = [];
    $scope.AssignRights = function (tableData) {
        $(".se-pre-con").fadeIn("slow");
        Mdata = tableData;
        $.ajax({
            type: "POST",
            url: "../User/GetAllMenuList",
            data: { WhereClause: ' and IsActive !=2 AND RoleId = ' + tableData.RoleID, MenuID: $scope.menu.ParentId, RoleId: tableData.RoleID },
            datatype: "json",
            success: function (data) {
                $(".se-pre-con").fadeIn("slow");
                //var data = [];// $.parseJSON(data.DataList);
                $scope.AllMenus = [];
                data.DataList.forEach(function (item, index) {
                    var MenuActionList1 = JSON.parse(item.MenuActionList);
                    if (item.AssignedActionID != undefined) {
                        var str_array = item.AssignedActionID.split(',');
                        str_array.forEach(function (Datavalue) {
                            MenuActionList1.forEach(function (val) {
                                if (Datavalue == val.Key) {
                                    val.RoleMenuActionCheck = true;
                                }
                            });
                        });
                    }

                    $scope.AllMenus.push({
                        Level1En: item.MenuNameEn,
                        Level1Hi: item.MenuNameHi,
                        MenuId: item.MenuId,
                        ParentId: item.ParentId,
                        MenuNameEn: item.MenuNameEn,
                        MenuNameHi: item.MenuNameHi,
                        MenuUrl: item.MenuUrl,
                        MenuActionId: item.MenuActionId,
                        Priority: item.Priority,
                        IsActive: item.IsActive,
                        CreatedBy: item.CreatedBy,
                        UpdatedBy: item.UpdatedBy,
                        CreationDate: item.CreationDate,
                        UpdationDate: item.UpdationDate,
                        CreatedIP: item.CreatedIP,
                        UpdatedIP: item.UpdatedIP,
                        ParentMenuNameEn: item.ParentMenuNameEn,
                        ParentMenuNameHi: item.ParentMenuNameHi,
                        MenuIcon: item.MenuIcon,
                        MenuLevel: item.MenuLevel,
                        IsChecked: item.IsChecked,
                        MenuActionList: MenuActionList1,
                        IsShow: true,
                        Class: "fa-plus",
                        IsChild: item.MenuLevel2.length > 0,
                        MenuLevel2: item.MenuLevel2
                    });
                    if (item.MenuLevel2 != undefined && item.MenuLevel2.length > 0) {
                        item.MenuLevel2.forEach(function (item2, index) {


                            var MenuActionList2 = JSON.parse(item2.MenuActionList);
                          
                            if (item2.AssignedActionID != undefined) {
                                var str_array1 = item2.AssignedActionID.split(',');
                                str_array1.forEach(function (Datavalue) {
                                    MenuActionList2.forEach(function (val) {
                                        if (Datavalue == val.Key) {
                                            val.RoleMenuActionCheck = true;
                                        }
                                    });
                                });
                            }



                            $scope.AllMenus.push({
                                Level1En: "",
                                Level1Hi: "",
                                Level2En: item2.MenuNameEn,
                                Level2Hi: item2.MenuNameHi,
                                MenuId: item2.MenuId,
                                ParentId: item2.ParentId,
                                MenuNameEn: item2.MenuNameEn,
                                MenuNameHi: item2.MenuNameHi,
                                MenuUrl: item2.MenuUrl,
                                MenuActionId: item2.MenuActionId,
                                Priority: item2.Priority,
                                IsActive: item2.IsActive,
                                CreatedBy: item2.CreatedBy,
                                UpdatedBy: item2.UpdatedBy,
                                CreationDate: item2.CreationDate,
                                UpdationDate: item2.UpdationDate,
                                CreatedIP: item2.CreatedIP,
                                UpdatedIP: item2.UpdatedIP,
                                ParentMenuNameEn: item2.ParentMenuNameEn,
                                ParentMenuNameHi: item2.ParentMenuNameHi,
                                MenuIcon: item2.MenuIcon,
                                MenuLevel: item2.MenuLevel,
                                IsChecked: item2.IsChecked,
                                MenuActionList: MenuActionList2,
                                IsShow: false,
                                Class: "fa-plus",
                                IsChild: item2.MenuLevel3.length > 0,
                                MenuLevel3: item2.MenuLevel3
                            });

                            if (item2.MenuLevel3 != undefined && item2.MenuLevel3.length > 0) {
                                item2.MenuLevel3.forEach(function (item3, index) {


                                    
                                    var MenuActionList3 = JSON.parse(item3.MenuActionList);
                                    if (item3.AssignedActionID != undefined) {
                                        var str_array2 = item3.AssignedActionID.split(',');
                                        str_array2.forEach(function (Datavalue) {
                                            MenuActionList3.forEach(function (val) {
                                                if (Datavalue == val.Key) {
                                                    val.RoleMenuActionCheck = true;
                                                }
                                            });
                                        });
                                    }

                                    $scope.AllMenus.push({
                                        Level1En: "",
                                        Level1Hi: "",
                                        Level2En: "",
                                        Level2Hi: "",
                                        Level3En: item3.MenuNameEn,
                                        Level3Hi: item3.MenuNameHi,
                                        MenuId: item3.MenuId,
                                        ParentId: item3.ParentId,
                                        MenuNameEn: item3.MenuNameEn,
                                        MenuNameHi: item3.MenuNameHi,
                                        MenuUrl: item3.MenuUrl,
                                        MenuActionId: item3.MenuActionId,
                                        Priority: item3.Priority,
                                        IsActive: item3.IsActive,
                                        CreatedBy: item3.CreatedBy,
                                        UpdatedBy: item3.UpdatedBy,
                                        CreationDate: item3.CreationDate,
                                        UpdationDate: item3.UpdationDate,
                                        CreatedIP: item3.CreatedIP,
                                        UpdatedIP: item3.UpdatedIP,
                                        ParentMenuNameEn: item3.ParentMenuNameEn,
                                        ParentMenuNameHi: item3.ParentMenuNameHi,
                                        MenuIcon: item3.MenuIcon,
                                        MenuLevel: item3.MenuLevel,
                                        IsChecked: item3.IsChecked,
                                        MenuActionList: MenuActionList3,
                                        IsShow: false,
                                        Class: "fa-plus"
                                    });

                                });
                            }
                        });
                    }
                });
                

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
                $(".se-pre-con").fadeOut("slow");
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

    $scope.MenuClass = 'fa-plus';
    $scope.ShowHideAllRoles = function () {
        if ($scope.MenuClass == "fa-plus") {
            $scope.AllMenus.forEach(function (item, index) {
                item.IsShow = true;
                item.Class = "fa-minus";
                if (item.MenuLevel2 != undefined && item.MenuLevel2.length > 0) {
                    item.MenuLevel2.forEach(function (item2, i) {
                        item2.IsShow = true;
                        item2.Class = "fa-minus";
                        if (item2.MenuLevel3 != undefined && item2.MenuLevel3.length > 0) {
                            item2.MenuLevel3.forEach(function (item3, i) {
                                item3.IsShow = true;
                                item3.Class = "fa-minus";
                            })
                        }
                    })
                }
            });
            $scope.MenuClass = "fa-minus"
        } else {
            $scope.AllMenus.forEach(function (item, index) {                
                item.IsShow = item.ParentId == 0;
                item.Class = "fa-plus";              
            });
            $scope.MenuClass = "fa-plus"
        }
    }

    $scope.ShowHideRoles = function (index, parent, level) {

        if ($scope.AllMenus[index].Class == "fa-plus") {
            $scope.AllMenus.forEach(function (item, index1) {
                if (parent == item.ParentId) {
                    item.IsShow = true;  

                    
                }
            });  
            $scope.AllMenus[index].Class = "fa-minus"
        } else {
            $scope.AllMenus.forEach(function (item, index1) {
                if (parent == item.ParentId) {
                    item.IsShow = false;  
                   

                }
            });  
            $scope.AllMenus[index].Class = "fa-plus"
        }

              
    }
    $scope.SetUncheckStatus = function (parent, check) {
        if (check) {
            $scope.AllMenus.forEach(function (item, index) {
                if (parent == item.ParentId) {
                    item.IsChecked = true;
                    if (item.MenuLevel3 != undefined && item.MenuLevel3.length > 0) {
                        item.MenuLevel3.forEach(function (item2, index1) {
                            if (item2.ParentId == item.MenuId) {
                                item2.IsChecked = true;
                                $scope.AllMenus[index + index1+1].IsChecked = true;
                            }
                        });
                    }
                }
            });

        } else {
            $scope.AllMenus.forEach(function (item, index) {
                if (parent == item.ParentId) {
                    item.IsChecked = false;
                    if (item.MenuLevel3 != undefined && item.MenuLevel3.length > 0) {
                        item.MenuLevel3.forEach(function (item2, index1) {
                            if (item2.ParentId == item.MenuId) {
                                item2.IsChecked = false;
                                $scope.AllMenus[index + index1+1].IsChecked = false;
                            }
                        });
                    }
                }
            });

        }

    };
    function RolemenuAction() {
        this.RoleId = '';
        this.MenuId = '';
        this.MenuAction = '';
        this.IsActive = 0;
    }

    $scope.SaveAssignRights = function () {
        $(".blockUI").fadeIn("slow");
        var RolemenuActionCollection = [];
        var tempMenuActions = '';
        //angular.forEach($scope.AllMenuList, function (value) {
        //    if (value.Checked || value.MenuIdChecked == '1') {
        //        var item = new RolemenuAction();
        //        item.MenuId = value.MenuId;
        //        item.IsActive = value.IsActive;
        //        item.RoleId = $scope.RoleId;
        //        tempMenuActions = '';
        //        angular.forEach(value.MenuActionList, function (value2) {
        //            if (value2.RoleMenuActionCheck == '1') {
        //                tempMenuActions += value2.Key + ",";
        //            }
        //        });
        //        item.MenuAction = tempMenuActions.substr(0, tempMenuActions.length - 1);
        //        RolemenuActionCollection.push(item);
        //    }
        //});

        $scope.AllMenus.forEach(function (value, index) {
            if (value.IsChecked) {
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
                $(".blockUI").fadeIn("slow");
                if (data2.Status) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    window.location = '../User/UserRole';
                    $(".se-pre-con").fadeOut("slow");
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    window.location = '../User/UserRole';
                    $(".se-pre-con").fadeOut("slow");

                }


            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
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