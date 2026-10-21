var AAdharModule = angular.module("AAdharMod", ['angularUtils.directives.dirPagination']);

AAdharModule.controller("AAdharCnt", function ($scope, $http) {

    $scope.GetAAdhar = function () {
        $http.get(SiteUrl + "/Citizen/AAdhar/?AAdharENC=" + '550649276302').
            success(function (data, status) {
                if (data!="") {                   
                    $('#encXml').html(data);
                    alert($('#encXml').html());                   
                    init();
                }
            }).
            error(function (data1, status) {
            });

    }

    $scope.BlockList = function () {
        $scope.Blocks = [];
        $scope.AddMoreForSportShow = true;
        if ($scope.SocietyRegistration.DISTRICT_ID != '0') {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegistration.DISTRICT_ID).
                success(function (data, status) {

                    $scope.Blocks = data;
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
                error(function (data, status) {
                    $scope.Blocks = [];
                });
        }

    };

    //START : Get City List
    $scope.GetCityList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode=" + $scope.SocietyRegistration.CityTypes).
            success(function (data, status) {
                $scope.CityList = data;
            }).
            error(function (data, status) {
                $scope.CityList = [];
            });


    };

    //START : Get District List
    $scope.GetDistrictWithoutDivisionList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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
                $scope.deshboaddata = null;
            };
        }
        $scope.GetDistrictWithoutDivisionList();
        $scope.GetCityList();
        $scope.BlockList();
        $(".se-pre-con").fadeOut("slow");

    };
    $scope.OnPageLoad();
});