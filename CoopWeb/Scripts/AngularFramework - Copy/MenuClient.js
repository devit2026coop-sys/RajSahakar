
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
    $scope.pageSize = 12;


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
        $http.post(SiteUrl + "/Menu/SaveMenu", $scope.menu).
             success(function (data, status) {
                 var message = '';
                 if (data == 1) {
                     message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                 }
                 else if (data == 2) {
                     message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
                 }
                 else {
                     message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
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
            }).
            error(function (data, status) {
                alert("Error while Menu insert");
            });


    };

    $scope.pageChangeHandler = function (num) {
    };

    $scope.ShowHide = function () {

        $scope.IsVisible = $scope.Show;
    }





    $scope.update = function (MenuId) {
        $http.get(SiteUrl + "/Menu/MenuList/?WhereClause="+ MenuId ).
            success(function (data, status) {

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
            }).
            error(function (data, status) {
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
               // $scope.MenuList('and 1=1');
                $scope.MenuList();
            }).
            error(function (data, status) {
            });
    };


    $scope.MenuListparent = function (WhereClause) {
        $http.get(SiteUrl + "/Menu/MenuListforParent/?WhereClause=" + WhereClause).
            success(function (data, status) {
                $scope.parentmenu = data;//data;
            }).
            error(function (data, status) {
                alert("Error while menu insert");
            });
    };

    $scope.MenuList = function (WhereClause) {
        $http.get(SiteUrl + "/Menu/MenuList/?WhereClause=" + WhereClause).
            success(function (data, status) {
                $scope.Menus = data;//data;
            }).
            error(function (data, status) {
                alert("Error while menu insert");
            });
    };

    $scope.MenuActionList = function () {
        $http.post(SiteUrl + "/Menu/MenuActionList", { WhereClause: ' and IsActive =1 ' }).
            success(function (data, status) {

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








