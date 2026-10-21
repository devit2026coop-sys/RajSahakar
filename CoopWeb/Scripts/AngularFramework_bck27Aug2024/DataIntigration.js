var SocietyRegistrationModule = angular.module("LovMod", ['angularUtils.directives.dirPagination']);
SocietyRegistrationModule.controller("LovCnt", function ($rootScope, $scope, $http, $location) {




    $scope._language = 'en-us';
 


    $scope.OnPageLoad = function () {
        //$(".se-pre-con").fadeIn("slow");    
        $scope._language = _language; // Set LAnguiage
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
            //$(".se-pre-con").fadeOut("slow");
        }
        $('#txtCreatetionDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $scope.Divisions = [{}];
        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
        //    success(function (data, status) {
        //        $scope.Divisions = data;
        //    }).
        //    error(function (data, status) {
        //        $scope.Divisions = [];
        //    });
        $http.post(SiteUrl + "/MIS/GetUserLevelId").
            success(function (data, status) {
                if (data.length > 0 && data != 0) {
                    if (data == 1)
                        $scope.IsState = true;
                }
            }).
            error(function (data, status) {
            });
        
    };

    
    $scope.GetDataFromProd = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/CommonServices/GetDataFromProd?RegistarionNo=" + $scope.LOV.ListCode).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");           
            }).
            error(function (data, status) {
                alert("Error")
                $(".se-pre-con").fadeOut("slow");
            });
    };
   
    $scope.OnPageLoad();
    
});

