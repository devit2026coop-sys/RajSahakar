
var MasterMod = angular.module('SocietyPaymentModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("SocietyPaymentCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.Datacontainer = [];

    $scope.getfunSocietyPayment = function () {
        $scope.Showgride = true;
        $scope.showotpblock = false;
        $http.get(SiteUrl + "/Citizen/SocietyPayment").
            success(function (data, status) {
                if (data.length == 0) {
                    ShowModelAlert(_language == 'en-us' ? "Society Payment" : 'शुल्क भुगतान', _language == 'en-us' ? 'There is not any society for Payment.' : 'अभी पेमेंट के लिए कोई सोसाइटी नहीं है।');
                    $scope.Showgride = false;
                }
                else
                    $scope.SocietyRegdata = data;
            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
            });
    }
    //End 
    // on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        if (_language == 'en-us') {
            $http.get(SiteUrl +"/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl +"/Translations/Translation_HI.json").
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
    $scope.PaymentInitiatByCityzen = function (Registration) {
        $http.post(SiteUrl + "/Citizen/FeePaymentByCitizen", { "RegistrationNo": Registration }).
            success(function (data, status) {

            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
            });
    }
    $scope.OnPageLoad();
    $scope.getfunSocietyPayment();


});






