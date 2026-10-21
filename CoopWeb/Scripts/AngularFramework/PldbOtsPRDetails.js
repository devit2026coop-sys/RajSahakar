var pldbApp = angular.module("pldbApp", ['angularUtils.directives.dirPagination']);
pldbApp.controller("pldbCtrl", function ($scope, $http) {
    $scope._language = 'en-us';
    $scope.PaymentModeList = [];
    $scope.ApplicantDetails = null;
    $scope.paymentSummaryList = [];

    $scope.payFObj = {};
    $scope.ApplicationDetails = false;
    $scope.NoticeNo = "";
    //const urlParams = new URLSearchParams(window.location.search);
    //$scope.otsIdEnc = urlParams.get('otsEncId');
    //$scope.otsIdEnc = '0x020000001BAC96EAD32DD2F2D3E1BFB470447CB87A7B597BAC6E278D926C454B3240DB1C';

    $scope.otsIdEnc = "";
    $scope.ShowOTPDiv = false;
    $scope.isErrorShow = false;
    $scope.ShowReceiptDiv = true;
    $scope.OnPageLoad = function () {
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
        $scope.GetpaymentModeList();
        //  $scope.GetApplicationDetails();
    };

    $scope.GetpaymentModeList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PAYMENTMODE").
            success(function (data, status) {
                $scope.PaymentModeList = data;
            }).
            error(function (data, status) {
                $scope.PaymentModeList = [];
            });
    };
    $scope.GetApplicationDetails = function () {
        $scope.paymentSummaryList = [];
        $scope.ApplicantDetails = null;
        if ($scope.NoticeNo == '') {
            alert("Please Fill Notice No");
            return;
        }

        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/PLDB/GetOtsApplicantDetails?NoticeNo=" + $scope.NoticeNo).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data != null) {
                    $scope.ApplicationDetails = true;
                    if (data.Result.Data.ApplicantDetail != null) {
                        $scope.isErrorShow = false;
                        $scope.ShowReceiptDiv = false;
                        $scope.ApplicantDetails = JSON.parse(data.Result.Data.ApplicantDetail)[0];
                        $scope.paymentSummaryList = JSON.parse(data.Result.Data.PaymentSummary);
                    } else { $scope.isErrorShow = true; $scope.ShowReceiptDiv = true; }

                }
            }).
            error(function () {
                $scope.ApplicationDetails = true;

                alert("Error while fetching OTS application data.");
                $(".se-pre-con").fadeOut("slow");
            });
    }

    $scope.DownloadPreLoanPaymentReceipt = function (Id) {
        url = '../PLDB/ots_pr_receipt?otsIdEnc=' + $scope.otsIdEnc + '&SummaryId=' + Id;
        window.open(url, "_blank");
    }


    $scope.VerifyOTP = function () {

        $scope.DeletePaymentReceipt($scope.Id, $scope.Remark);
    }


    $scope.DeletePaymentReq = function (Id, Remark) {

        $scope.Id = Id;
        $scope.Remark = Remark;

        $http.get("../ApplicationFlow/GenerateAadhaarOTPForEsing").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.indexOf('NO#') >= 0) {
                    alert("Due to some issue OTP not send");
                    //location.reload();
                    //window.location = '../ApplicationFlow/ApplicationFlow';
                } else {
                    if (data != "LOGOUT") {
                        // alert(data);
                        $scope.TXN = data;
                        alert(_language == 'en-us' ? 'OTP has been sent to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n');
                        $scope.ShowOTPDiv = true;
                    }
                    else {
                        $(".se-pre-con").fadeOut("slow");
                        window.location = 'https://sso.rajasthan.gov.in/signin';
                    }
                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
            });


    }

    $scope.ResendOTP = function () {
        $scope.DeletePaymentReq($scope.Id, $scope.Remark);
    }
    $scope.Clear = function () {

        $scope.Id = 0;
        $scope.Remark = "";
        $scope.ShowOTPDiv = false;
    }

    $scope.DeletePaymentReceipt = function (Id, Remark) {

        //if (!confirm('Are you Sure you want to Delete this Payment Summary' ))
        //    return;

        $(".se-pre-con").fadeIn("slow");

        $http.post(SiteUrl + "/PLDB/UpdateRepaymentSummary", { "Id": Id, "Remark": Remark, "OTP": $scope.OTP, "TXN": $scope.TXN }).

            //  $http.post(SiteUrl + "/PLDB/UpdateRepaymentSummary?Id=" + Id + "&Remark=" +Remark ).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.Result.Message == "Success") {
                    $('#repaymentmodal').modal('hide');
                    $scope.ShowOTPDiv = false;
                    ShowOkMessage("Success", _language == 'en-us' ? 'Data Deleted successfully\n' : 'डेटा सफलतापूर्वक हटा दिया गया है\n', function () {

                        $scope.GetApplicationDetails();
                    });
                }
                else if (data.Result.Message == "Invalid OTP") {
                    ShowOkMessage("Error", _language == 'en-us' ? 'Invalid OTP! \n' : 'अवैध ओटीपी!\n');
                }
                else {
                    $scope.ShowOTPDiv = false;
                    ShowOkMessage("Error", _language == 'en-us' ? 'Some Error Occurred.\n' : 'कुछ त्रुटि हुई है\n', function () { });
                }
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    }

    $scope.BackToPage = function () {
        $scope.ShowReceiptDiv = true;
        $scope.ApplicationDetails = false;
        $scope.ApplicantDetails = null;
        $scope.paymentSummaryList = [];
        $scope.NoticeNo = '';
    }
    $scope.OnPageLoad();
});

pldbApp.filter('inrCurrency', function () {
    return function (input) {
        if (!input)
            return input;
        var num = parseFloat(input);
        if (isNaN(num)) return input;
        return num.toLocaleString('en-IN', { style: 'currency', currency: 'INR' });
    };
});
