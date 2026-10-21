
var UserMenu = angular.module("UserMenu", ['angularUtils.directives.dirPagination']);


UserMenu.directive('onlyLettersInput', onlyLettersInput);

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

UserMenu.controller("Menu", function ($scope, $http) {


    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 400;


    $scope.InsertMenu = function () {
        // var menuActionCollection = [];
        var tempMenuActions = '';
        angular.forEach($scope.MenuActionList, function (value) {
            if (value.Checked) {
                tempMenuActions += value.MenuActionId + ',';
            }
        });
        tempMenuActions = tempMenuActions.substr(0, tempMenuActions.length - 1)
        $scope.menu.ActionIds = tempMenuActions;

        if (!$scope.menu.SubMenu) {
            $scope.menu.SubMenuId = "0";
        }
        if ($scope.menu.SubMenu && $scope.menu.SubMenuId == "0") {
            ShowOkMessage("", "Please select submenu from list", function () { });
        } else {
            $http.post(SiteUrl + "/Menu/SaveMenu", $scope.menu).
                success(function (data, status) {
                    $(".se-pre-con").fadeIn("slow");
                    var message = '';
                    if (data == 1) {
                        message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                        $(".se-pre-con").fadeOut("slow");
                    }
                    else if (data == 2) {
                        message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
                        $(".se-pre-con").fadeOut("slow");
                    }
                    else {
                        message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                        $(".se-pre-con").fadeOut("slow");
                    }
                    alert(message);
                    window.location.reload();
                    if ($scope.ParentId == '') {
                        $scope.ParentId = 0;
                    }

                    if (data.Status) {
                        $scope.menu = '';
                        $scope.ClearMenuForm();
                        $scope.MenuList();
                        //$scope.MenuList('and 1=1');
                        $scope.MenuActionList();
                    }
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    alert("Error while Menu insert");
                });
        }




    };

    $scope.pageChangeHandler = function (num) {
    };

    $scope.ShowHide = function () {

        $scope.IsVisible = $scope.Show;
    }



    $scope.GetSubMenu = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Menu/GetAllSubMenuList/?ParentId=" + $scope.menu.ParentId).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.submenu = data;

            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };




    $scope.AllMenus = [];
    $scope.BindMenu = function (MenuId) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Menu/GetAllMenuList/?WhereClause=" + MenuId).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");

                //$scope.AllMenus = data.DataList;

                data.DataList.forEach(function (item, index) {

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
                        IsChecked: item.IsChecked
                    });
                    if (item.MenuLevel2 != undefined &&  item.MenuLevel2.length > 0) {
                        item.MenuLevel2.forEach(function (item2, index) {
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
                                IsChecked: item2.IsChecked
                            });

                            if (item2.MenuLevel3 !=undefined && item2.MenuLevel3.length > 0) {
                                item2.MenuLevel3.forEach(function (item3, index) {
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
                                        IsChecked: item3.IsChecked
                                    });

                                });
                            }
                        });
                    }
                });
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    }
    $scope.BindMenu("");
    $scope.update = function (MenuId) {
        $http.get(SiteUrl + "/Menu/MenuList/?WhereClause=" + MenuId).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                var menuActionCollection = [];

                if (data[0].ParentId == 0) {
                    $scope.IsVisible = true;
                }
                else {
                    $scope.IsVisible = false;
                }

                angular.forEach($scope.MenuActionList, function (val) {
                    val.Checked = false;
                });
                if (data[0].MenuActionId != "") {
                    var str = data[0].MenuActionId;
                    var str_array = str.split(',');

                    angular.forEach(str_array, function (Datavalue) {
                        angular.forEach($scope.MenuActionList, function (val) {
                            if (Datavalue == val.MenuActionId) {
                                val.Checked = true;
                            }
                        });
                    });
                }
                $scope.menu = data[0];
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.deleterecord = function (id) {
        $http.get(SiteUrl + "/Menu/DeleteMenu?id=" + id).then(function (d) {
            //MenuList('and 1=1');
            MenuList();
        }, function (error) {
            alert('Failed');
        });
    };

    $scope.SetStatus = function (MenuId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;
        $http.post(SiteUrl + "/Menu/ChangeStatus", { Id: MenuId, Action: ActionId }).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                // $scope.MenuList('and 1=1');
                $scope.MenuList();
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
            });
    };


    $scope.MenuListparent = function (WhereClause) {
        $http.get(SiteUrl + "/Menu/MenuListforParent/?WhereClause=" + WhereClause).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.parentmenu = data;//data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                alert("Error while menu insert");
            });
    };

    $scope.MenuList = function (WhereClause) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Menu/MenuList/?WhereClause=" + WhereClause).
            success(function (data, status) {
                //$(".se-pre-con").fadeIn("slow");
                $scope.Menus = data;//data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while menu insert");
            });
    };

    $scope.MenuActionList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Menu/MenuActionList", { WhereClause: ' and IsActive =1 ' }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.MenuActionList = data;
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
                alert("Error while fetching MenAction list.");
                $scope.showLoader = false;
            });
    };

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
    //start on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $(".se-pre-con").fadeOut("slow");



        $scope.ClearMenuForm = function () {
            $scope.menu = '';
        };
    }


    $scope.MenuListbyparents = function () {
        $scope.MenuList('ParentId' + $scope.menu.ParentId);

    };
    //Get list of all users on page list
    $scope.getTranslation($scope);
    // $scope.MenuList('and 1=1');
    $scope.MenuList();
    $scope.MenuListparent();
    $scope.IsVisible = false;
    $scope.MenuActionList();
    $scope.OnPageLoad();


    //Clear user Role

});








