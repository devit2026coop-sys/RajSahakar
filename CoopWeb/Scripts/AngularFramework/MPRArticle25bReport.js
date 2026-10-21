var MasterMod = angular.module('MPRArticle25bReportMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("MPRArticle25bReportCnt", function ($scope, $rootScope, $http) {
     
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;

    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $("#DivisionId").val()).
            success(function (data, status) {
                $scope.DistrictList = data;
                //if ($("#hdnDistrictId").val() != "0") {
                //    $scope.DISTRICT_ID = $("#hdnDistrictId").val()
                //    $("#ddrDistrictId").attr("disabled", "disabled");
                //} 
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };

    $scope.GetDistrictList();

    $scope.clean = function () {
        $scope.PyActive = 0;
        $scope.PyInactive = 0;
        $scope.PyUnderLiquidation = 0;
        $scope.PyTotal = 0;
        $scope.CmgmActive = 0;
        $scope.CmgmInactive = 0;
        $scope.CmgmUnderLiquidation = 0;
        $scope.CmTotal = 0;
        $scope.EmgmActive = 0;
        $scope.EmgmInactive = 0;
        $scope.EmgmUnderLiquidation = 0;
        $scope.EmTotal = 0;
    }

    $scope.GetMPRArticle25bList = function () {
        $http.get(SiteUrl + "/MPR/GetMPRArticle25bReport").success(function (data) {
            //console.log(data, "MPRArticle25bReportList");
            $scope.clean();
            $scope.MPRArticle25bReportList = JSON.parse(data.Result.Data.MPRArticle25bReportList);
            $scope.MPRArticle25bReportList.forEach(function (item, indx) {
                $scope.PyActive += parseInt(item.PyActive);
                $scope.PyInactive += parseInt(item.PyInactive);
                $scope.PyUnderLiquidation += parseInt(item.PyUnderLiquidation);
                $scope.PyTotal += parseInt(item.PyTotal);
                $scope.CmgmActive += parseInt(item.CmgmActive);
                $scope.CmgmInactive += parseInt(item.CmgmInactive);
                $scope.CmgmUnderLiquidation += parseInt(item.CmgmUnderLiquidation);
                $scope.CmTotal += parseInt(item.CmTotal);
                $scope.EmgmActive += parseInt(item.EmgmActive);
                $scope.EmgmInactive += parseInt(item.EmgmInactive);
                $scope.EmgmUnderLiquidation += parseInt(item.EmgmUnderLiquidation);
                $scope.EmTotal += parseInt(item.EmTotal);
            })
        }).error(function () {
            alert("Error");
        });
    };

    $scope.OnPageLoad = function () {
        //$(".se-pre-con").fadeIn("slow");
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

            //$scope.ClearUnitForm = function () {
            //    $scope.UnitMaster = null;
            //};
        }
        $scope.GetMPRArticle25bList();
    };

    $scope.OnPageLoad(); 
});
  
