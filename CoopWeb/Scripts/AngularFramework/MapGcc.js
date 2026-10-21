var MasterMod = angular.module('UserSocietyMappingValidModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("UserSocietyMappingValidCont", function ($scope, $http, $timeout) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 1000;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.Datacontainer = [];
    $scope.OTPStatus = false;

    $scope.divAADHARVerificationStep_1 = false;
    $scope.divAADHARVerificationStep_2 = false;
    $scope.showSoceitygride = false;
    $scope.disableTxtRegNo = false;

    $scope.IsActdisabled = true;
    $scope.RegistrationNo = '';
    $scope.divAADHARVerificationStep_1 = false;
    $scope.getdisable = false;


    $scope.SSOMAPRequest = { RequestId: "0", TransferDistraict: "", TransferRemarks: "", RequestBy: 0, MapSSOId: "" };

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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
            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }
        $(".se-pre-con").fadeOut("slow");
    };

    $scope.ClearTransFormForm = function () {
        $scope.SSOMAPRequest = { RequestId: "0", TransferDistraict: "", TransferRemarks: "", RequestBy: 0, MapSSOId: "" };
    }

    $scope.SocietyDetails = [];
    $scope.MapRegistrationNo = "";


    $scope.GetGccForMap = function () {
        if ($scope.MapRegistrationNo == '') {
            ShowOkMessage("", "Please Enter Registration Number", function () { });
        }
        else {
            $(".blockUI").fadeIn("slow");
            return $http.post("/GopalCreditCard/GetSSOMapRequestList", JSON.stringify({ registrationNo: $scope.MapRegistrationNo, pacsId: ($scope.Pacsid ?? 0) })).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    $scope.RequestForMAPList = data;
                }).
                error(function (data, status) {
                    $scope.RequestForMAPList = [];
                    $(".blockUI").fadeOut("slow");
                });
        }
    };
    $scope.showTransModel = function (item) {

        $("#transModals").modal('show');
        $scope.SSOMAP = item;
        $scope.SSOMAPRequest.RequestBy = item.RequestBy;
        $scope.SSOMAPRequest.RequestId = item.RequestId;
        $scope.SSOMAPRequest.TransferDistraict = "";
        $scope.SSOMAPRequest.TransferRemarks = "";
    }
    $scope.SubmitSSOMAPRequest = function () {

        if ($scope.SSOMAPRequest.RequestBy == "-1" && ($scope.SSOMAPRequest.MapSSOId == undefined || $scope.SSOMAPRequest.MapSSOId == "")) {
            var msg = _language == 'en-us' ? 'Please enter SSO ID' : 'Please enter SSO ID';
            ShowOkMessage("", msg, function () { });
        }
        else if ($scope.SSOMAPRequest.TransferRemarks == undefined || $scope.SSOMAPRequest.TransferRemarks == "") {
            var msg = _language == 'en-us' ? 'Please enter remark for request' : 'Please enter remark for request';
            ShowOkMessage("", msg, function () { });
        } else {
            $(".se-pre-con").fadeIn("slow");

            $http.post(SiteUrl + "/GopalCreditCard/UpdateGccMAPRequest", { GccIdEnc: $scope.SSOMAP.GccIdEnc, SSOId: $scope.SSOMAPRequest.MapSSOId == undefined ? "" : $scope.SSOMAPRequest.MapSSOId, Remarks: $scope.SSOMAPRequest.TransferRemarks }).
                success(function (data, status) {
                    if (data > 0) {
                        $(".se-pre-con").fadeOut("slow");
                        $("#transModals").modal('hide');
                        ShowOkMessage("", "Request has been successfully updated.", function () { $scope.GetSocietyForMap() });
                    }
                    else if (data == -2) {
                        $(".se-pre-con").fadeOut("slow");
                        ShowOkMessage("", "Same SSOId already exist for this application.", function () { });
                    }

                    else {
                        $(".se-pre-con").fadeOut("slow");
                        ShowOkMessage("", "There is some problem please try after some time.", function () { });
                    }
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("", "There is some problem please try after some time.", function () { });
                });
        }
    }

    $scope.ApplicationView = function (eId) {
        if (eId != '') {
            window.location = "../GopalCreditCard/GopalApplicationFormView?eId=" + eId;
        }
    }
    $scope.OnPageLoad();
});






