var pldbApp = angular.module("pldbApp", ['angularUtils.directives.dirPagination']);
pldbApp.controller("pldbCtrl", function ($scope, $http, $filter) {
    $scope._language = 'en-us';
    $scope.PaymentModeList = [];
    $scope.ApplicantDetails = {};
    $scope.ApplicantDetails.IsAccountCloseInSinglePayment = 0;
    $scope.payObj = {};
    $scope.AccountCloseDate = null;
    $scope.ShowPaymentOption = false;

    const urlParams = new URLSearchParams(window.location.search);
    $scope.otsIdEnc = urlParams.get('otsEncId');
    //$scope.otsIdEnc = '0x020000001BAC96EAD32DD2F2D3E1BFB470447CB87A7B597BAC6E278D926C454B3240DB1C';

    $scope.today = new Date();

    $scope.payObj.PaymentDate = $scope.today;
    $scope.AccountCloseDate = new Date($scope.today); 
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
        $scope.GetApplicationDetails();
    };

    $scope.GetpaymentModeList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PAYMENTMODE").
            success(function (data, status) {
                $scope.PaymentModeList = data;
                $scope.PaymentModeList = data.filter(x => x.PaymentModeId != 7);
            }).
            error(function (data, status) {
                $scope.PaymentModeList = [];
            });
    };
    $scope.today = new Date();

    $scope.payObj.PaymentDate = $scope.today;
    $scope.AccountCloseDate = new Date($scope.today); 
    $scope.getDaysAfter31Mar2025 = function () {
        let fromDate = new Date("2025-03-31");
        let today = new Date();
        let diffInMs = today.getTime() - fromDate.getTime();
        let diffInDays = Math.floor(diffInMs / (1000 * 60 * 60 * 24));
        return diffInDays > 0 ? diffInDays : 0;
    };
    $scope.GetApplicationDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        var dateObj = new Date(($scope.AccountCloseDate ?? $scope.today));
        //$scope.FormattedAccountCloseDate = $filter('date')(dateObj.toLocaleDateString(), 'yyyy-MM-dd');
        $scope.FormattedAccountCloseDate = (dateObj.getMonth()+1) + '/' + dateObj.getDate() + '/' + dateObj.getFullYear();
        $http.get(SiteUrl + "/PLDB/GetOtsApplicantDetails?otsIdEnc=" + $scope.otsIdEnc + "&summaryId=0" + "&type=0&isAccountCloseInSinglePayment=" + $scope.IsAccountClose + "&accountCloseDate=" + ($scope.FormattedAccountCloseDate ?? null)).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data != null) {
                    $scope.ApplicantDetails = JSON.parse(data.Result.Data.ApplicantDetail)[0];
                    $scope.ShowPaymentOption = ($scope.ApplicantDetails.PLDB_Id == $('#hdnPldbId').val() && ($scope.ApplicantDetails.PLDB_BranchId == $('#hdnPldbBranchId').val() || $('#hdnPldbBranchId').val() == 0));
                    $scope.ApplicantDetails.AlreadyPaidAmount = parseFloat($scope.ApplicantDetails.AlreadyPaidAmount);
                    $scope.ApplicantDetails.EstimatedTotalAmount = parseFloat($scope.ApplicantDetails.EstimatedTotalAmount);
                    $scope.paymentSummaryList = JSON.parse(data.Result.Data.PaymentSummary);
                    if ($scope.ApplicantDetails.IsAccountCloseInSinglePayment == 1) {
                        $scope.IsAccountClose = 1;
                        if ($scope.ApplicantDetails.AccountCloseDate != null) {
                            $scope.AccountCloseDate = new Date($scope.ApplicantDetails.AccountCloseDate);
                            $('.datecalculator').prop('disabled', 'disabled');
                        }
                    }
                    $scope.payObj.PaymentDate = $scope.AccountCloseDate ?? $scope.today;
                }
            }).
            error(function () {
                alert("Error while fetching OTS application data.");
                $(".se-pre-con").fadeOut("slow");
            });
    }
    $scope.SavePayment = function () {

        $scope.PaidAmount = parseFloat($scope.ApplicantDetails.AlreadyPaidAmount) + parseFloat($scope.payObj.PaidAmount);
        if ($scope.ApplicantDetails.EstimatedTotalAmount < $scope.PaidAmount) {
            alert("Amount is not greater than OD Amount.");
            return;
        }

        if (!confirm('Are you want to submit payment'))
            return;
        if (($scope.payObj.PaymentDate ?? null) == null) {
            alert("Please select payment date.");
            return
        }

        $(".se-pre-con").fadeIn("slow");
        $scope.payObj.OtsIdEnc = $scope.otsIdEnc;
        $scope.payObj.AccountCloseDate = $scope.FormattedAccountCloseDate;

        $scope.payObj.DepositByPerson = $scope.payObj.DepositorType == "Self" ? "Self" : $scope.payObj.DepositByPerson;
        $scope.payObj.IsAccountCloseInSinglePayment = $scope.IsAccountClose;

        var paymentDateObj = new Date(($scope.payObj.PaymentDate ?? $scope.today));
        //$scope.payObj.PaymentDate = $filter('date')(paymentDateObj.toLocaleDateString(), 'yyyy-MM-dd');
        $scope.payObj.PaymentDate = (paymentDateObj.getMonth() + 1) + '/' + paymentDateObj.getDate() + '/' + paymentDateObj.getFullYear();
        if ($scope.payObj.PaymentModeId == 2) {

            var chequeDateObj = new Date(($scope.payObj.ChequeDate));
            //$scope.payObj.ChequeDate = $filter('date')(chequeDateObj.toLocaleDateString(), 'yyyy-MM-dd');
            $scope.payObj.ChequeDate = (chequeDateObj.getMonth() + 1) + '/' + chequeDateObj.getDate() + '/' + chequeDateObj.getFullYear();

        }
        $http.post(SiteUrl + "/PLDB/SaveRepaymentSummary", $scope.payObj).success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            if (data.Result.Message == "Success") {
                $('#repaymentmodal').modal('hide');
                ShowOkMessage("Success", _language == 'en-us' ? 'Data saved successfully\n' : 'डेटा सहेजा गया है\n', function () {
                    $scope.payObj = {};
                    $scope.payObj.PaymentDate = $scope.today;
                    $scope.GetApplicationDetails();
                });
            }
            else if (data.Result.Message == "Duplicate") {
                $('#repaymentmodal').modal('hide');
                ShowOkMessage("Duplicate", _language == 'en-us' ? 'Data Already exists.\n' : 'डेटा पहले से मौजूद है\n', function () {
                    $scope.GetApplicationDetails();
                });
            }
            else if (data.Result.Message == "Loanee are not eligible to make payments.") {
                ShowOkMessage("Warning", "Loanee are not eligible to make payments. Only lenders with 25% partial payment are make payment. (ऋणदाता भुगतान करने के लिए पात्र नहीं हैं। केवल 25% आंशिक भुगतान वाले ऋणदाता ही भुगतान कर सकते हैं।)", function () {
                    $scope.payObj = {};
                    $scope.payObj.PaymentDate = $scope.today;
                    $scope.GetApplicationDetails();
                });
            }
            else {
                ShowOkMessage("Warning", data.Result.Message, function () {
                    $scope.payObj = {};
                    $scope.payObj.PaymentDate = $scope.today;
                    $scope.GetApplicationDetails();
                });
            }
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
        });
    }

    $scope.DownloadPreLoanPaymentReceipt = function (Id) {
        url = '../PLDB/ots_pr_receipt?otsIdEnc=' + $scope.otsIdEnc + '&SummaryId=' + Id;
        window.open(url, "_blank");
    }
    $scope.FinalPay = function (Id) {
        url = '../PLDB/PldbFinalLoanPayment?otsEncId=' + $scope.otsIdEnc;
        window.open(url, "_blank");
    }

    $scope.BackToPage = function () {
        window.location = '../PLDB/OTSApplicationList';
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

