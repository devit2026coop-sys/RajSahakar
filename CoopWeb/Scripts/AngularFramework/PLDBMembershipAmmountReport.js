
var UserMenu = angular.module("PLDBMembershipAmmountReportMOD", ['angularUtils.directives.dirPagination']);


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

UserMenu.controller("PLDBMembershipAmmountReportCONT", function ($scope, $http) {


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
        tempMenuActions = tempMenuActions.substr(0, tempMenuActions.length - 1);
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
    $scope.pldbdata = '';   
    $scope.ShowbranchRport = false;
    $scope.MembershipAmmountReport = function (id) { 
        $http.get(SiteUrl + "/MIS/MembershipAmmountReportdata/?PldbId=" + id).
            success(function (data, status) {              
                if (id != 0) {
                    $scope.ShowbranchRport = true;
                    $scope.DataSetforbranches = data;
                }
                else {
                    $scope.ShowbranchRport = false;
                    $scope.DataSet = data;
                }
            }).
            error(function (data, status) {
                $scope.SocietySubClassList = [];
                $scope.ShowbranchRport = false;
            });
    };

    

    $scope.BindFY = function () {

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                //alert(JSON.stringify(data));
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    }

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
 
    }


    
    //Get list of all users on page list
    $scope.getTranslation($scope);
    $scope.OnPageLoad();
    $scope.MembershipAmmountReport('0');
    $scope.BindFY();
    //$scope.LoadMaster();
    //$scope.PLDBBranchList('0');
    //Clear user Role

});










