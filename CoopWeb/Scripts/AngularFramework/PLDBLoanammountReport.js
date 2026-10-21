
var UserMenu = angular.module("PLDBLoanammountReportMOD", ['angularUtils.directives.dirPagination']);


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

UserMenu.controller("PLDBLoanammountReportCONT", function ($scope, $http) {


    // SCOPE
    $scope.currentPage = 1;
    $scope.pageSize = 400;


 

    $scope.pageChangeHandler = function (num) {
    };
    $scope.pldbdata = '';
  
    $scope.ShowbranchRport = false;
    $scope.MembershipAmmountReport = function (PldbId) {  
        $http.get(SiteUrl + "/MIS/MembershipAmmountReportdata/?PldbId=" + PldbId).
            success(function (data, status) {
                $scope.DataSet = data;
                $scope.ShowbranchRport = false;
            }).
            error(function (data, status) {
                $scope.SocietySubClassList = [];
                $scope.ShowbranchRport = false;
            });
    };
    $scope.PLDBID = '';
    $scope.ShowforBRnach = false;
    $scope.PLDBLoanammountReportList = function (PldbId) {
        $scope.PLDBID = PldbId;
        $http.get(SiteUrl + "/MIS/PLDBLoanammountReports/?PldbId=" + $scope.PLDBID).
            success(function (data, status) {
                if ($scope.PLDBID == 0) {
                    $scope.ShowforBRnach = false;
                    $scope.LoanAmtdata = data;
                }
                else {
                    $scope.ShowforBRnach = true;
                    $scope.LoanAmtdataForBranch = data;
                }
                $scope.ShowbranchRport = true;
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
    $scope.PLDBLoanammountReportList('0');
    $scope.BindFY();

});










