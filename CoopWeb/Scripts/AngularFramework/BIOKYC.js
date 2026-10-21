var AppClasses = angular.module("biokycMod", ['angularUtils.directives.dirPagination']);
AppClasses.controller("biokycCnt", function ($scope, $http, $rootScope) {

    $scope._language = 'en-us';
    $scope._language = _language;
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

        $scope.ClearUnitForm = function () {
            $scope.UnitMaster = null;
        };
    }

    $scope.messagebox = false;
    $scope.OtpDilogBox = false;
    $scope.FillOtp = false;

    $scope.ShowOtpDilogBox = function () {
        $("#myOtpDilogBox").modal("show");
        $('#SSOAuth_OTP').hide();

        $('#btnSSOAuth_SendOTP').show();
        $('#btnSSOAuth_ValidateOTP').hide();
    }
    $scope.HideCheckAllowMessage = function () {
        $scope.messagebox = false;
        $scope.FillOtp = false;
        $('#SSOAuth_OTP').hide();
        $('#btnSSOAuth_SendOTP').show();
        $('#btnSSOAuth_ValidateOTP').hide();
    }
    $scope.CloseOtpBox = function () {
        $scope.messagebox = false;
        $scope.OtpDilogBox = false;
        $scope.FillOtp = false;
        $('#SSOAuth_OTP').hide();
        $("#myOtpDilogBox").modal("hide");
        $('#ssoauthloading').hide();
        $("#SSOAuth_chkConsent").prop("checked", false);
        $('#btnSSOAuth_SendOTP').show();
        $('#btnSSOAuth_ValidateOTP').hide();
    };

    $scope.Filter = {
        AadharNo:""
    }

    $scope.UserDetails = {};

    $scope.showDetails = false;
    $scope.VerifyOTP = function (isSubmit) {      
        $scope.showLoader = true;
        // alert($scope.OTP + "------" + $('#lbltrn').html() + "---" + SiteUrl);
        if ($scope.Filter.AadharNo != '') {
            $(".se-pre-con").fadeIn("slow");        
             $http.post(SiteUrl + "/pldb/VerifyBIOKYC?AadharNo=" + $scope.Filter.AadharNo).
                 success(function (data, status) {
                    
                    $(".se-pre-con").fadeOut("slow");
                    //$('#myOTPModal').modal("hide");
                    if (data.Status == "1") {
                        $scope.UserDetails = data.UserDetails;
                        ShowOkMessage("", _language == 'en-us' ? 'Successfully Varified' : "Successfully Varified", function () {
                            $('#myOTPModal').modal("hide");
                            $scope.showDetails = true;
                        });
                        $scope.showDetails = true;
                        //window.location = '../Election/ElectionSocietyList';
                    } else  {
                        ShowOkMessage("", _language == 'en-us' ? "Invalid Data. Please try again." : "Invalid Data. Please try again.", function () { });
                        $scope.showDetails = false;
                    }  

                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("", _language == 'en-us' ? "Invalid Data. Please try again." : "Invalid Data. Please try again.", function () { }); //return false;
                });
        }
        else {
            ShowOkMessage("Alert", _language == 'en-us' ? "Please enter Aadhar No." : "Please enter Aadhar No.", function () { return false; });
        }
        $scope.showLoader = false;
    };
    
});