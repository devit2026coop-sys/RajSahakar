
var MAction = angular.module("MenuAction", ['angularUtils.directives.dirPagination']);

MAction.directive('onlyLettersInput', onlyLettersInput);

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

MAction.controller("Action", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 12;

    $scope.SaveMenuAction = function () {
        $http.post(SiteUrl+"/Menu/MenuActionInsert", $scope.menuaction).//$scope.userrole
            success(function (data, status) {
                 var msg1 = "";
                if (data > 0) {
                    msg1 = (_language == 'en-us' ? 'Data Save Sucessfully\n' : 'डेटा सहेजा गया है\n')
                        || $scope.ClearMenuActionForm();
                }
                else {
                    alert(_language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n') || $scope.ClearMenuActionForm();
                    return;
                }
                
                if (data.Status) {
                    
                    $scope.ClearMenuActionForm();
                    $scope.menuaction = null;
                    $scope.MenuActionList();
                   
                }
            }).
            error(function (data, status) {
                alert("Error while Menu Action insert");
            });
    };

    $scope.pageChangeHandler = function (num) {
    };

    // START : Code for Language label setting
    $scope.getTranslation = function () {
        $scope.language = _language;
        if (_language == 'en-us') {
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

    // Get List of all Menu ction
    $scope.MenuActionList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl+"/Menu/MenuActionList").
            success(function (data, status) {

                $scope.MenuActions = data;
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
            error(function (data, status) {
                alert("Error while fetching MenAction list.");
              
            });
    };

    //Clear Menu Action
    $scope.ClearMenuAction = function () {
        $scope.menuaction = null;
    };

    

    //Clear user Role
    $scope.ClearMenuActionForm = function () {
        $scope.menuaction = null;
    };

    $scope.update = function (MenuActionId) {
     
        $http.post(SiteUrl+"/Menu/MenuActionList", { WhereClause: ' AND MenuActionId=' + MenuActionId }).
            success(function (data, status) {
                $scope.menuaction = data[0];
               
            }).
            error(function (data, status) {
            });
    };

    $scope.SetStatus = function (MenuActionId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl+"/Menu/ChangeMenuActionStatus", { Id: MenuActionId, Action: ActionId }).
            success(function (data, status) {

                $scope.MenuActionList();
            }).
            error(function (data, status) {
            });
    };

    //Clear user Role
    $scope.ClearMenuActionForm = function () {
        $scope.menuaction = null;
    };
    $scope.getTranslation();
    $scope.MenuActionList();
});

