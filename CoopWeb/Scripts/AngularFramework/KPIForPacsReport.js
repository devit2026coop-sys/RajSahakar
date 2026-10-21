
var MasterMod = angular.module('ApplicationFlowKPIPacsModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ApplicationKPIPacsFlowCont", function ($scope, $rootScope, $http) {
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 300,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }

    $scope.GopalCreditCardModel = [];
    $scope.godown = [];

    $scope.GetSocietyBySSO = function (ActId, SocietyTypeId, ClassId) {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Godown/CitizenDashboardDataWithDepartmentUser", {
            "ActId": ActId == undefined ? 0 : parseInt(ActId),
            "SocietyTypeId": SocietyTypeId == undefined ? 0 : parseInt(SocietyTypeId),
            "ClassId": ClassId == undefined ? 0 : parseInt(ClassId)
        }).success(function (data, status) {
            $scope.TableData = data.filter(x => x.IsObjection == false);
            $scope.SocietyDataList = $scope.TableData;
            $scope.SocietyList = $scope.SocietyDataList;
            $(".se-pre-con").fadeOut("slow");
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            $scope.TableData = [];
            $scope.SocietyList = [];
        });
    };


    $scope.GetSocietyList = function () {
        $scope.SocietyList = [];
        $http.get(SiteUrl + "/Godown/GetSocietyData/?Type=" + $scope.type + "&DistrictId=" + $('#district_MemberAdd').val() + "&BlockId=" + ($scope.UralUrban == 1 ? $("#ddlblocki_MemberAdd").val() : 0) + "&CityId=" + ($scope.UralUrban == 1 ? 0 : $("#ddlcity_MemberAdd").val()) + "&GPId=" + ($scope.UralUrban == 1 ? $("#ddlGP_MemberAdd").val() : 0)).
            success(function (data, status) {
                $scope.SocietyList = data;

            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                alert('Error while fetching SocietyList  data.');
            });
    };

    $scope.kpiList = [];

    $scope.searchRequest = function () {
        $http.get(SiteUrl + "/Society/GetKPIForPacsReport", {
            params: { scoietyid: $scope.godown.SocietyID }
        }).success(function (response) {
            $scope.kpiList = JSON.parse(response);
            $(".se-pre-con").fadeOut();
        }).error(function () {
            $(".se-pre-con").fadeOut();
        });
    };


    $scope.OnPageLoad = function () {
       /* $(".se-pre-con").fadeIn("slow");*/
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

        var levelId = parseInt($("#hdnLevelIdId").val(), 10);
        $scope.showClearButton = (levelId === 1);

  
        $scope.GetSocietyBySSO();
    };


    $scope.OnPageLoad();

});
