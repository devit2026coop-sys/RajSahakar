var SocietyMappingRequestApprovalMod = angular.module('SocietyMappingRequestApprovalMod', ['angularUtils.directives.dirPagination']);
SocietyMappingRequestApprovalMod.controller('SocietyMappingRequestApprovalCnt', function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.divGrid = true;
    $scope.UserRegistration = [];
    $scope.UserRegistration.ActionRemark = '';
    $scope.UserRegistration.ActionStatus = '';
    $scope.UserRegistration.RequestId = 0;
    $scope.ProcessData = [];
    $scope.isControlDisable = false;


    // START : Code for Language label setting
    $scope.getTranslation = function () {
        $scope._language = _language;
        if (_language === 'en-us') {
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

        $scope.GetSocietyMappingRequestData();

    };
    // END : Code for Language label setting

    $scope.GetSocietyMappingRequestData = function () {
        $(".se-pre-con").fadeIn("slow");
        
        $http.post(SiteUrl + "/User/GetSocietyMappingRequestList").
            success(function (data, status) {
                if (data.length < 1) {
                    $scope.divGrid = false;
                    ShowOkMessage($scope.translation.ApproveSocietyMappingRequest, _language === 'en-us' ? 'No data found' : 'कोई डेटा नहीं मिला', function () { });
                }
                else {
                    $scope.divGrid = true;
                }
                $scope.SocietyMappingRequest = data;
            }).
            error(function (data, status) {
                $scope.SocietyMappingRequest = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    $scope.GetSocietyMappingActionData = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.SocietyMemberData = [];
        $http.post("/WorkFlow/WorkFlowActionList", { TypeId: 1, RequestId: $scope.RequestIdCalculated, MenuURL:'/User/ApproveSocietyMappingRequest'}).
            success(function (data, status) {
                $scope.ActionsList = data;
            }).
            error(function (data, status) {
                $scope.SocietyMappingRequest = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };


    $scope.ViewDetails = function (data1) {
        $(".se-pre-con").fadeIn("slow");
        $scope.RequestDetails = data1;
        $scope.RequestIdCalculated = data1.SocietyMappingRequestId;
        $scope.SocietyMappingRequestIdEnc = data1.SocietyMappingRequestIdEnc;
        
        $scope.GetSocietyMappingActionData();

        $scope.divGrid = false;

        $scope.divViewShow = true;
        $(".se-pre-con").fadeOut("slow");
    };

    //START : GEt NEXT ROLE LIST
    $scope.GetNextRoleList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.ProcessData.NextRoleId = '0';
        $scope.ProcessData.NextUserId = '0';
        $scope.isControlDisable = false;
        $scope.NextRoleList = [];
        if (!($scope.ProcessData.NextActionId === '26' || $scope.ProcessData.NextActionId === '1044' || $scope.ProcessData.NextActionId === '13')) {

            $scope.isControlDisable = false;

            $http.post(SiteUrl + "/WorkFlow/WorkFlowNextRoleList", { TypeId: 1, ActionId: $scope.ProcessData.NextActionId, MenuURL: '/User/ApproveSocietyMappingRequest' }).
                success(function (data, status) {
                    $scope.NextRoleList = data;
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $scope.NextRoleList = [];
                    $(".se-pre-con").fadeOut("slow");
                });
        }
        else {
            $scope.isControlDisable = true;
        }
        $(".se-pre-con").fadeOut("slow");
    };
    //END : Get Next RoleList


    //START : GEt NEXT USER LIST
    $scope.GetNextUserList_ByWorkFlow = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.NextUserList = [];
        $http.post(SiteUrl + "/WorkFlow/WorkFlowNextUserList", { TypeId: 1, RoleId: $scope.ProcessData.NextRoleId }).
            success(function (data, status) {
                $scope.NextUserList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.NextUserList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetNextUserList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.NextUserList = [];
        $http.post(SiteUrl + "/ApplicationFlow/GetOfficerBySocietyIDEnc", { SocietyIDEnc: $scope.SocietyMappingRequestIdEnc, NextStatusId: $scope.ProcessData.NextRoleId, CurrentStatusId: '0', RoleId: $scope.ProcessData.NextRoleId }).
            success(function (data, status) {
                $scope.NextUserList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.NextUserList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    //END : Get Next USER LISR

    //START : Show Log
    $scope.ShowMappingLog = function (_RequestId) {

        $(".se-pre-con").fadeIn("slow");
        $scope.LogData = [];
        $http.post("/User/GetSSocietyMappingRequestActivityLog", { RequestId: _RequestId}).
            success(function (data, status) {
                $scope.LogData = data;
            }).
            error(function (data, status) {
                $scope.LogData = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    //END Show Log

   

    // START : Get All All List
    $scope.ClearUserApprovalForm = function () {
        window.location.reload();
    };
    // START : Get All All List
    $scope.SetBack = function () {
        $scope.UserRegistration.ActionRemark = '';
        $scope.UserRegistration.ActionStatus = '';
        $scope.UserRegistration.RequestId = 0;
        $scope.divViewShow = false;
    };


    // START : Process Request 
    $scope.ProcessRequest = function () {

        var title = _language === 'en-us' ? 'Approve Society Mapping Request' : 'सोसाइटी मानचित्रण अनुरोध प्रतिक्रिया';
        var Msg = '';
        $scope.UserRegistration.RequestId = $('#hfRequestId').val();

        if ($scope.ProcessData.NextActionId === undefined || $scope.ProcessData.NextActionId === '0') {
            Msg += (_language === 'en-us' ? 'Please select Action <br/>' : 'कृपया गतिविथि  का चयन करें<br/>');
        }

        if (!($scope.ProcessData.NextActionId === '26' || $scope.ProcessData.NextActionId === '1044' || $scope.ProcessData.NextActionId === '13')) {

            if ($scope.ProcessData.NextRoleId === undefined || $scope.ProcessData.NextRoleId === '0') {
                Msg += (_language === 'en-us' ? 'Please select Role <br/>' : 'कृपया भूमिका का चयन करें<br/>');
            }


            if ($scope.ProcessData.NextUserId === undefined || $scope.ProcessData.NextUserId === '0') {
                Msg += (_language === 'en-us' ? 'Please Select User <br/>' : 'कृपया उपयोगकर्ता का चयन करें<br/>');
            }
        }

        

        if ($scope.ProcessData.Remark === undefined || $scope.ProcessData.Remark.length < 1) {
            Msg += (_language === 'en-us' ? 'Please enter Remark<br/>' : 'कृपया टिप्पणी दें<br/>');
        }

        if (Msg.length > 1) {
            ShowOkMessage(title, Msg, function () { });
            return;
        }

        var req = {
            RequestId: $scope.UserRegistration.RequestId,
            NextActionId: $scope.ProcessData.NextActionId,
            NextRoleId: $scope.ProcessData.NextRoleId,
            NextUserId: $scope.ProcessData.NextUserId,
            Remark: $scope.ProcessData.Remark
        };
       
        $http.post(SiteUrl + "/User/SaveSocietyMappingProcessRequest", req).
            success(function (data, status) {
                // $scope.MemberDetailsList = data[0];
                alert("Data saved successfully.");
                window.location.reload();
            }).
            error(function (data, status) {
                alert('Error occurred.');
            });


    };
    // START : Process Request

    //// START : On Page Load
    $scope.getTranslation();


    //// END : On Page Load

});
