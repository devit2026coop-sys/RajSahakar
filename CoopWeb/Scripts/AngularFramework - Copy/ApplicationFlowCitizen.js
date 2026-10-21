
var MasterMod = angular.module('ApplicationFlowCitizenModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ApplicationFlowCitizenCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.SocietyIdEnc = '';

    $scope.GetApplicationFlowCitizenSociety = function (SocietyIDEnc) {
        $scope.showLoader = true;
        $http.get(SiteUrl +"/ApplicationFlow/GetApplicationFlowSociety/?SocietyIDEnc=" + SocietyIDEnc).
            success(function (data, status) {
                $scope.ApplicationFlowCitizenSociety = data;
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
        $scope.showLoader = true;
    }   

    
    
    $scope.DashboardData = function () {

        $http.post(SiteUrl + "/Citizen/CitizenDashboardData", { "ActId":  0 , "SocietyTypeId": 0 , "ClassId": 0 }).
            success(function (data, status) {
                $scope.SocietyList = [];
                $scope.SocietyList = data;               
            }).
            error(function (data, status) {
                $scope.TableData = [];
            });

    }

    // on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get("/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });

            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }
        $scope.DashboardData();
        $(".se-pre-con").fadeOut("slow");
    };

   
    


    $scope.update = function (item) {
        $scope.showLoader = true;
        $scope.GetApplicationFlowCitizenSociety(item);
        $scope.showLoader = false;

    };
    
    $scope.OnPageLoad();

});






