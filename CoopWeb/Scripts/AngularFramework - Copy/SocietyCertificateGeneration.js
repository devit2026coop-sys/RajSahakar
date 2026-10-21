
var MasterMod = angular.module('SocietyCertificateGenerationModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("SocietyCertificateGenerationCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.Datacontainer = [];


    $scope.Societycertificate = function () {
        $http.get(SiteUrl + "/Society/SocietyCertificateData").
            success(function (data, status) {
                if (data.length == 0) {
                    ShowModelAlert("Society Mapping", 'Invalid Society Registration No.');
                }
                else
                    $scope.Societycertificatedata = data;
            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
            });
    }

    //End 
    // demo funtion 
    $scope.msg = function () {
        ShowModelAlert("Society Certificate ", 'Comming soon');
    }
    //end demo
    // on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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
        $(".se-pre-con").fadeOut("slow");
    };
    $scope.OnPageLoad();
    $scope.Societycertificate();



});






