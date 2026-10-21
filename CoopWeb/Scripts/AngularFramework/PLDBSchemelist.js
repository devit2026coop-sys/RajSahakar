

var MasterMod = angular.module('PLDBLoanMastersMOD', ['angularUtils.directives.dirPagination']);
MasterMod.controller("PLDBLoanMastersCONT", function ($scope, $http) {


    $scope._language = 'hi-in';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    _language = 'hi-in'

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
    $scope.getTranslation();
    $scope.Selectsector = function (id) {
        //if (id == 1) {
        //    $scope.Isfarming = true;
        //}
        //else if (id == 2) {
        //    $scope.Isfarming = false;
        //}
        $http.post(SiteUrl + "/pldb/SchemebyTypeList", { "id": id }).
            success(function (data, status) {
                $scope.SchemebyTypeList = [];
                $scope.SchemebyTypeList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Society Details.");
            });
    }

    $scope.AllPurpose = [];
    $scope.LoadSchemes = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/pldb/PurposebyTypeList", { "id": 0 }).
            success(function (data, status) {
                $scope.AllPurpose = data;
                $scope.PurposebyTypeList = [];
                $scope.PurposebyTypeList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while fetching Society Details.");
            });
    }
    $scope.LoadSchemes();

    $scope.SelectPurpose = function (id) {
        $scope.PurposebyTypeList = [];
        if (id == 1) {
            $scope.AllPurpose.forEach(function (item, index) {
                if (item.Farming_NonFarming == 'Farming')
                    $scope.PurposebyTypeList.push(item);
            });
        } else {
            $scope.AllPurpose.forEach(function (item, index) {
                if (item.Farming_NonFarming == 'NonFarming')
                    $scope.PurposebyTypeList.push(item);
            });
        }
        //$http.post(SiteUrl + "/pldb/PurposebyTypeList", { "id": id }).
        //    success(function (data, status) {
        //        $scope.PurposebyTypeList = [];
        //        $scope.PurposebyTypeList = data;
        //    }).
        //    error(function (data, status) {
        //        alert("Error while fetching Society Details.");
        //    });
    }


});