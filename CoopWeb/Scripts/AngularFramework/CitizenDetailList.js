var MasterMod = angular.module('CitizenDetailListModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("CitizenDetailListCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 15;
    $scope.UserDetailAll = '';
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UserRegistrationAppr = [];
    $scope.RequestType = '';
    $scope.WorkActId = '';
    $scope.UserRegistrationSearch = [];
    $scope.UserReqData_New = [];
    $scope.SelectAll = function (Check, counter) {
        angular.forEach($scope.UserReqData, function (item) {
            if (counter == 1) {
                item.IsChecked = Check;
            }
            else if (counter == 2) {
                item.IsChecked2 = Check;
            }

        });
    }



    // on page load
    $scope.OnPageLoad = function () {
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
                $scope.UserRegistrationAppr = null;
            };
        }
        
       
    };


    $scope.flag = 0;
   


    // Sort handler
    $scope.colsort = 'DisplayNameEn';
    $scope.reverseSort = false;

    $scope.sortData = function (column) {
        if ($scope.colsort === column) {
            $scope.reverseSort = !$scope.reverseSort;
        } else {
            $scope.colsort = column;
            $scope.reverseSort = false;
        }

      
    };

    $scope.getSortClass = function (column) {
        if ($scope.colsort === column) {
            return $scope.reverseSort ? 'fa fa-sort-desc' : 'fa fa-sort-asc';
        }
        return 'fa fa-sort';
    };




    $scope.GetAllSubUnitList = function () {
        debugger
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Login/GetCitizenDetailList").
            success(function (data, status) {
                //Date Conversion
                angular.forEach(data, function (item) {
                    if (item.DateOfBirth) {
                        // Convert "24-06-1978 00:00:00" → Date object
                        var parts = item.DateOfBirth.split(' ')[0].split('-');
                        item.DateOfBirth = new Date(parts[2], parts[1] - 1, parts[0]);
                    }
                });
                $scope.UserReqData = data;
               /* $scope.UserReqData_New = data;*/
                $(".se-pre-con").fadeOut("slow");

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
            error(function (data1, status) {

            });
    }

    
    $scope.GetAllSubUnitList();
    $scope.OnPageLoad();

});