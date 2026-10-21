var RoleRights = angular.module("RoleRights", ['angularUtils.directives.dirPagination']);

RoleRights.controller("RoleRightsController", function ($scope, $http) {
    $scope.currentPage = 1;
    $scope.pageSize = 50;

    $scope.translation1 = function () {
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
    }


    $scope.RoleRightsList = function () {
      
        //$http.get(SiteUrl+"/User/AllMenuList").
            success(function (data, status) {
                

                angular.forEach(data, function (val) {
                    val.MenuActionList = angular.fromJson(val.MenuActionList);
                });

                $scope.AllMenuList = data;
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
                alert("Error while fetching AllMenuList list.");
                $scope.showLoader = false;
            });
    };

    //Clear user Role
    $scope.ClearRoleForm = function () {
        $scope.userrole = null;
    };

    //Get list of all users on page list
    //$scope.AllMenuList();
    $scope.translation1();

});