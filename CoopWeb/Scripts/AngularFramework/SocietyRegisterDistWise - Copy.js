var SocietyRegistrationModule = angular.module("SocietyRegisterDistWiseMod", ['angularUtils.directives.dirPagination']);
SocietyRegistrationModule.controller("SocietyRegisterDistWiseCnt", function ($scope, $http) {

    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.ActId = '0';
    $scope.SocietyTypeId = '0';
    $scope.DIVISION_ID = '0';

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
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
            success(function (data, status) {
                $scope.Divisions = data;
            }).
            error(function (data, status) {
                $scope.Divisions = [];
            });
        $http.post(SiteUrl + "/MIS/GetUserLevelId").
            success(function (data, status) {
                if (data.length > 0 && data != 0) {
                    if (data==1)
                        $scope.IsState = true;
                }
            }).
            error(function (data, status) {
            });
        $scope.GetSocietyData();
    };
    $scope.Actlist = [];
    $scope.GetSocietyActList = function () {
        $http.get(SiteUrl + "/Society/GetSocietyActList").
            success(function (data, status) {
                data.forEach(function (item, index) {
                    if ($("#hdnActId").val().split(',').indexOf(item.ActId) > -1) {
                        $scope.Actlist.push(item);
                    }
                });

                if ($("#hdnActId").val().split(',').length == 1)
                {
                    $scope.ActId = $("#hdnActId").val();
                    $scope.GetSocietyTypeList();
                    $("#ddrActId").attr("disabled", "disabled");
                }

                //$scope.Actlist = data;
            }).
            error(function (data, status) {
                $scope.Actlist = [];
            });
    };
    $scope.SocietyView = function (SocietyIdEnc) {
        window.location = ' /Society/SocietyApplicationView?SocietyIDEnc=' + SocietyIdEnc + '~_MIS_SocietyRegisterDistWise~0';
    }

    // GET Society Type
    $scope.GetSocietyTypeList = function () {
        $scope.Typelist = [];
        $scope.SocietyTypeId = '0';
        if ($scope.ActId != '0') {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=" + $scope.ActId).
            success(function (data, status) {
                $scope.Typelist = data;
            }).
            error(function (data, status) {
                $scope.Typelist = [];
                });
        }
    };
    //END  
    //alert(1 + $("#hdnActId").val());
    $scope.ActId = $("#hdnActId").val()
    $scope.GetSocietyData = function () {
      
        //alert($scope.ActId);
        //$scope.ActId=

        $scope.SocietyList = [];
        $scope.SocietyCombine = [];
        $scope.DivShowCount = true; $scope.DivShowDetail = false;
        $(".se-pre-con").fadeIn("slow");
        $http.post("../MIS/GetSocietyDistWiseData", {
            "ActId": $scope.ActId == "0" ? $("#hdnActId").val() : $scope.ActId, "SocietyTypeId": $scope.SocietyTypeId, "CreatetionDate": $('#txtCreatetionDate').val().split("/").reverse().join("-"), "DIVISION_ID": $scope.DIVISION_ID
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
        $(".se-pre-con").fadeIn("slow");
        $http.post("../MIS/GetSocietyReportData", {
            "DistrictId": DistId, "StatusId": StatusId, "ActId": $scope.ActId, "SocietyTypeId": $scope.SocietyTypeId
        }).
            success(function (data, status) {
                $scope.DivShowCount = false;
                $scope.DivShowDetail = true;
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

    $scope.BackDetails = function () {
        $scope.DivShowCount =true ;
        $scope.DivShowDetail =false ;

    }

    // START : Call Functions on Page Load
    $scope.GetSocietyActList();
   // $scope.GetSocietyTypeList();
    $scope.OnPageLoad();
    //END : Call Functions on Page Load
});

