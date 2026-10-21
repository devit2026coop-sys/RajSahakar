var FertilizerReportModule = angular.module('CCSComplaintReportMod', ['angularUtils.directives.dirPagination']);
FertilizerReportModule.controller("CCSComplaintReportCnt", function ($scope, $http) {

    // SCOPE

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.divStateData = true;
    $scope.SearchType = 1;
    $scope.divDistrictWiseFertilizer = false;
    $scope.DistrictName = '';
    $scope.divRetailerData = false;
    $scope.divRetailerFertilizerTransData = false;
    $scope.DISTRICT_ID = '0';
    $scope.SocietyId = '0';

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
        $scope.DistrictList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
        $scope.GetSocietyBasicDetailList();
        $scope.GetComplaintData();
        $(".se-pre-con").fadeOut("slow");
    };
    //END page load 
    $scope.ChangeSelction = function () {
        $scope.SearchType = $('input[type=radio][name=rdoSelect]:checked').val();
        //if ($('input[type=radio][name=rdoSelect]:checked').val() === '3') {
        //    $scope.GetSocietyBasicDetailList();
        //}
        //else {
        //    $scope.GetSocietyBasicDetailList();
        //}

    };
    $scope.Reset = function () {
        $scope.DivShowDetail = false;
        $scope.DivShowCount = false;
        $scope.DISTRICT_ID = '0';
        $scope.CCSComplaintList = [];
        $scope.SocietyId = '0';
        $scope.SearchType = 1;
        //$scope.GetSocietyBasicDetailList();

    };
    $scope.SetCCSociety = function () {
        //if($scope.DISTRICT_ID != '0') {
            $scope.GetSocietyBasicDetailList();
       // }
    };
    $scope.GetSocietyBasicDetailList = function () {
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Society/GetCCSocietyList", { "SearchType": $scope.SearchType, "DistrictId": $scope.DISTRICT_ID}).
            success(function (data, status) {
                $scope.SocietyBasicDetailList = data;
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching list");
                $(".blockUI").fadeOut("slow");
            });
        $(".blockUI").fadeOut("slow");
    };

    $scope.GetComplaintData = function () {   
        $scope.CCSComplaintList = [];
        $scope.CCSComplaintCombine = [];    
        $scope.DivShowCount = true;
        $scope.DivShowDetail = false;
        $(".blockUI").fadeIn("slow");
        $http.post("../MIS/GetComplaintReportData", {
            "DistrictId": $scope.DISTRICT_ID, "SocietyId": $scope.SocietyId, "SearchType": $scope.SearchType}).
            success(function (data, status) {
                if (data.length === 0) {
                    var msg = _language == 'en-us' ? 'No data found.' : 'कोई डेटा नहीं मिला।';
                    ShowModelAlert('', msg);
                    return;
                }
                else {
                    $scope.CCSComplaintCombine = data.Table;
                    //$scope.CCSComplaintList = data.Table1;
                   // alert(JSON.stringify($scope.CCSComplaintCombine));
                   // alert(JSON.stringify($scope.CCSComplaintList));
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.CCSComplaintList = [];
                alert('Error while fetching data.');
                $(".se-pre-con").fadeOut("slow");
            });
    };



    $scope.ShowDetail = function (item) {
        $(".blockUI").fadeIn("slow");
        $scope.CCSComplaintList = [];
        $scope.currentPage = 1;
        $http.post("../MIS/GetComplaintReportData", {
            "DistrictId": item.DISTRICT_ID, "SocietyId": $scope.SocietyId, "SearchType": $scope.SearchType
        }).
            success(function (data, status) {
                if (data.length === 0) {
                    var msg = _language == 'en-us' ? 'No data found.' : 'कोई डेटा नहीं मिला।';
                    ShowModelAlert('', msg);
                    return;
                }
                else {
                   // $scope.CCSComplaintCombine = data.Table;
                    $scope.CCSComplaintList = data.Table;
                    // alert(JSON.stringify($scope.CCSComplaintCombine));
                    // alert(JSON.stringify($scope.CCSComplaintList));
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.CCSComplaintList = [];
                alert('Error while fetching data.');
                $(".se-pre-con").fadeOut("slow");
            });
        $scope.DivShowDetail = true;
        $scope.DivShowCount = false;

    };

    $scope.BackToCount = function () {
        $scope.DivShowDetail =false ;
        $scope.DivShowCount =true ;
    }

    $scope.OnPageLoad();

});