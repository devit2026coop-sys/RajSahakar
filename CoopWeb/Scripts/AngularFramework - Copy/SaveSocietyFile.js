
var MasterMod = angular.module('SaveSocietyFileModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("SaveSocietyFileCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];

    $scope.SaveSocietyFile = function () {
        $scope.showLoader = true;
        //$http.post(SiteUrl + "/FileUploaders/SaveSocietyAllFile", { "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "Remark": $scope.ApplicationFlowMaster.Remark, "StatusId": $scope.ApplicationFlowMaster.StatusId, "UserId": $scope.ApplicationFlowMaster.UserId }).
        $http.post(SiteUrl + "/FileUploaders/SaveSocietyAllFile").
            success(function (data, status) {
                if (data > 0) {                    
                        alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n');
                        location.reload();
                }
                else {
                    alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                    wlocation.reload();
                }
            }).
            error(function (data, status) {
                alert("Error while insert");
            });
        $scope.showLoader = false;
    }
    

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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
                $scope.UnitMaster = null;
            };
        }
      //  $scope.GetCurrentUserId();
        $scope.showLoader = false;
    };

    $scope.OnPageLoad();


});