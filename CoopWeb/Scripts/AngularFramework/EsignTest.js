var ArrSelectTraningUser = new Array();
function TraningUserSelected() {
    this.UserId = '0';
}

var AppTrainning = angular.module("GopalCreditCardAppEntryForm", ['angularUtils.directives.dirPagination']);
AppTrainning.directive('maxLengthAndValidate', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            var disallowedChars = /[^0-9]/g;
            ngModel.$parsers.push(function (value) {
                if (!value) return '';
                var cleanValue = value.replace(disallowedChars, '');
                if (cleanValue.length > 6) {
                    cleanValue = cleanValue.substring(0, 6);
                }
                if (cleanValue !== value) {
                    ngModel.$setViewValue(cleanValue);
                    ngModel.$render();
                }

                return cleanValue;
            });
        }
    };
});
AppTrainning.controller("GopalCreditCard", function ($scope, $http) {
    // Save Sms Template 
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivSms = true;
    $scope.newdiv = "";
    $scope.Trainning = '';
    $scope.GCCRegistrationStatus = false;
    $scope.G1DetailFetched = false;
    $scope.G2DetailFetched = false;
    $scope.DistrictList = [];
    $scope.llx = "200";
    $scope.lly = "50";
    $scope.signedBy = 0;
    // on page load

    $scope.divTabulation = true;
    $scope.ActionId = 1;

    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json?id=3").
                success(function (data, status) {

                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json?id=3").
                success(function (data, status) {

                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }

        $scope.ShowJanAadharOTPValidation = false;
        $scope.ApplicationFlowMaster = {};
    };

    //GCC Esign process

    $scope.DocumentEsign = function (signedBy) {

        $scope.GenerateEsignOtp();
        $scope.signedBy = signedBy;
    };

    $scope.GCCApplicationESign = function (signedBy) {
        $scope.showLoader = true;
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/GopalCreditCard/GCCApplicationEsignTest", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": $scope.ApplicationFlowMaster.TXN, "signedBy": signedBy, "llx": $scope.llx, "lly": $scope.lly, "location": "GUDHA GORJI", designation:"GURANTOR 1" }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data) {
                    $scope.showLoader = false;
                    $("#modelPopupOTP").modal("hide");
                    if (data > 0) {
                        ShowOkMessage("Success", _language == 'en-us' ? 'Data saved successfully\n' : 'डेटा सहेजा गया है\n', function () { window.location.reload(); });
                    }
                    else if (data == -1) {
                        ShowOkMessage("Error", _language == 'en-us' ? 'The application cannot be forwarded because there is no user specified \n' : 'आवेदन अग्रेषित नहीं किया जा सकता क्योंकि कोई उपयोगकर्ता निर्दिष्ट नहीं है \n');
                    }
                    else if (data == -2) {
                        ShowOkMessage("Error", _language == 'en-us' ? 'Invalid OTP! \n' : 'अवैध ओटीपी!\n');
                    }
                    else if (data == -3) {
                        ShowOkMessage("Error", _language == 'en-us' ? 'Duplicate Jan Aadhaar Number  One Jan Aadhaar (Family) -> One Loan \n' : 'डुप्लीकेट जन आधार नंबर एक जन आधार (परिवार) -> एक ऋण\n');
                    }
                    else {
                        $(".se-pre-con").fadeOut("slow");
                        ShowOkMessage("Error", _language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');

                    }
                }
            }).error(function (data, status) {
                $scope.showLoader = false;
                $(".se-pre-con").fadeOut("slow");
                alert("Error while insert");
            });
    };

    $scope.GenerateEsignOtp = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get("../ApplicationFlow/GenerateAadhaarOTPForEsignGcc?userType=2&aadharNoEnc=" + "RBHhrpwjSHqwqTqHRRD1Cg==").success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            if (data.indexOf('NO#') >= 0) {
                alert("Due to some issue OTP not send");
                $("#modelPopupOTP").modal("hide");
            } else {
                if (data != "LOGOUT") {
                    $scope.ApplicationFlowMaster.TXN = data;
                    alert(_language == 'en-us' ? 'OTP has been sent to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n');
                    $("#modelPopupOTP").modal("show");
                }
                else {
                    $(".se-pre-con").fadeOut("slow");
                    $("#modelPopupOTP").modal("hide");
                    window.location = 'https://sso.rajasthan.gov.in/signin';
                }
            }
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
        });
    }


    $scope.VerifyOTP = function () {
        if (!$scope.ApplicationFlowMaster.OTP) {
            alert("Please enter valid otp");
            return;
        }

        $scope.GCCApplicationESign($scope.signedBy);
    };
    $scope.clearOTPModel = function () {
        $("#modelPopupOTP").modal("hide");
        $scope.ApplicationFlowMaster.OTP = '';
    }


    $scope.OnPageLoad();

});

