var SocietyRegistrationModule = angular.module("SocietyRegisterDistWiseMod", ['angularUtils.directives.dirPagination']);
SocietyRegistrationModule.controller("SocietyRegisterDistWiseCnt", function ($scope, $http) {

    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.ActId = '0';
    $scope.SocietyTypeId = '0';

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
        $scope.GetSocietyData();
    };

    $scope.GetSocietyActList = function () {
        $http.get(SiteUrl + "/Society/GetSocietyActList").
            success(function (data, status) {
                $scope.Actlist = data;
            }).
            error(function (data, status) {
                $scope.Actlist = [];
            });
    };

    $scope.GetSocietyTypeList = function () {
        $http.get(SiteUrl + "/Society/GetSocietyTypeList").
            success(function (data, status) {
                $scope.Typelist = data;
            }).
            error(function (data, status) {
                $scope.Typelist = [];
            });
    };

    $scope.GetSocietyData = function () {
        $scope.SocietyList = [];
        $scope.SocietyCombine = [];
        $scope.DivShowCount = true; $scope.DivShowDetail = false;
        $http.post("../MIS/GetSocietyDistWiseData", {
            "ActId": $scope.ActId, "SocietyTypeId": $scope.SocietyTypeId, "CreatetionDate": $('#txtCreatetionDate').val().split("/").reverse().join("-")
        }).
            success(function (data, status) {
                if (data.length === 0) {
                    var msg = _language == 'en-us' ? 'No data found.' : 'कोई डेटा नहीं मिला।';
                    ShowModelAlert('', msg);
                    return;
                }
                else {
                    $scope.SocietyCombine = data.Table;
                    // alert(JSON.stringify($scope.SocietyCombine));
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                alert('Error while fetching society district wise data.');
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.ShowDetail = function (DistId, StatusId) {
        $scope.SocietyList = [];
        $http.post("../MIS/GetSocietyReportData", {
            "DistrictId": DistId, "StatusId": StatusId, "ActId": $scope.ActId, "SocietyTypeId": $scope.SocietyTypeId
        }).
            success(function (data, status) {
                if (data.length === 0) {
                    var msg = _language == 'en-us' ? 'No data found.' : 'कोई डेटा नहीं मिला।';
                    ShowModelAlert('', msg);
                    return;
                }
                else {
                    // $scope.CCSComplaintCombine = data.Table;
                    $scope.SocietyList = data.Table;
                    // alert(JSON.stringify($scope.CCSComplaintCombine));
                    // alert(JSON.stringify($scope.SocietyList));
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.CCSComplaintList = [];
                alert('Error while fetching society data.');
                $(".se-pre-con").fadeOut("slow");
            });
        $scope.DivShowDetail = true;
    };

    // START : Call Functions on Page Load
    $scope.GetSocietyActList();
    $scope.GetSocietyTypeList();
    $scope.OnPageLoad();
    //END : Call Functions on Page Load
});

