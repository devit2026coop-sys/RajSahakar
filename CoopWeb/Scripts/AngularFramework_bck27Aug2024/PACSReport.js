var PACSReportModule = angular.module('PACSReportMod', ['angularUtils.directives.dirPagination']);
PACSReportModule.controller("PACSReportCnt", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.BlockData = [];
    $scope.divStateData = true;
    $scope.divDistrictWisePACS = false;
    $scope.divBlockWisePACS = false;
    $scope.DistrictName = '';
    $scope.divSocietyWisePACS = false;
    $scope.divSocietyMemberWisePACS = false;

    // on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        if (_language == 'en-us') {
            $http.get("../Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("../Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
            
        }

        // GET State and District Level
        $http.post("../MIS/GetPACSReportData", { "LevelId": 0, "DistrictId": "-1" }).
            success(function (data, status) {
                $scope.StateData = data.filter(P => P.LevelId == '0');
                $scope.DistrictData = data.filter(P => P.LevelId == '1');
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.StateData = [];
                $(".se-pre-con").fadeOut("slow");
                $scope.DistrictData = [];
                alert('Error while fetching State and District PACS data.');
                $(".se-pre-con").fadeOut("slow");
            });
        
    };


    // START : Show District Wise PACS data
    $scope.ShowDistrictData = function () {
        $scope.divDistrictWisePACS = true;
        $scope.divBlockWisePACS = false;
    }
    //END : Show District Wise PACS data

    // START :Get List of Societies
    $scope.GetFilterData = function (item, filterid) {
        $(".se-pre-con").fadeIn("slow");
        $scope.divStateData = false;
        $scope.divDistrictWisePACS = false;
        $scope.divBlockWisePACS = false;
        $scope.divSocietyWisePACS = false;
        $scope.divMemberWisePACS = false;

        if (filterid == 1) {
            $scope.divBlockWisePACS = true;
        }
        else if (filterid == 3) {
            $scope.divSocietyWisePACS = true;
        }
        else if (filterid == 4) {
            $scope.divMemberWisePACS = true;
        }

        $http.post("../MIS/GetPACSReportData", { "LevelId": filterid, "DistrictId": item.Id }).
            success(function (data, status) {
                if (filterid == 1) {
                    $scope.BlockData = data;
                    $scope.DistrictName = item.LevelHi;
                }
                
                if (filterid == 3) {
                    $scope.SocietyData = data;
                }

                if (filterid == 4) {
                    $scope.SocietyMemberData = data;
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.BlockData = [];
                $scope.divBlockWisePACS = false;
                alert('Error while fetching Block Wise PACS data.');
            });
        
    };
    // END :Get List of Societies
   

    $scope.BackTODCCB = function (id) {
        $scope.divStateData = false;
        $scope.divDistrictWisePACS = false;
        $scope.divBlockWisePACS = false;
        $scope.divSocietyWisePACS = false;
        $scope.divMemberWisePACS = false;

        $scope.divStateData = true;
        $scope[id] = true;
    }

    $scope.OnPageLoad();
});