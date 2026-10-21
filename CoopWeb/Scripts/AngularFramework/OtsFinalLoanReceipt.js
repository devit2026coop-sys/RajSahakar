
var Report = angular.module("OTSLoanReceiptModule", ['angularUtils.directives.dirPagination']);
Report.controller("OTSLoanReceiptCont", function ($scope, $http) {

    var _language = 'en-hi';
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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
        }

        $scope.showLoader = false;
    };


    const urlParams = new URLSearchParams(window.location.search);
    $scope.otsIdEnc = urlParams.get('otsIdEnc');
    $scope.OTSRePaymentSummaryId = urlParams.get('SummaryId');
 
    $scope.ApplicantDetails = {};
    $scope.paymentSummaryList = [];
    $scope.OldPaymentSummaryList = [];
    $scope.CurrentPaymentSummaryList = [];
    $scope.GetApplicationDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get("../PLDB/GetOtsApplicantDetails?otsIdEnc=" + $scope.otsIdEnc+"&summaryId=0&type=1").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data != null) {
                    $scope.ApplicantDetails = JSON.parse(data.Result.Data.ApplicantDetail)[0];
                    $scope.paymentSummaryList = JSON.parse(data.Result.Data.PaymentSummary);
                    $scope.QrData = data.Result.Data.QrData;
                    $scope.FinalPaymentSummaryList = JSON.parse(data.Result.Data.FinalPaymentSummaryList);
                    $scope.PenalInt = (parseFloat($scope.FinalPaymentSummaryList[0].CurrentPenalInterest)).toFixed(2);
                    $scope.AlreadyPaidAmount = 0;
                    angular.forEach($scope.paymentSummaryList, function (item) {
                        $scope.AlreadyPaidAmount = parseFloat($scope.AlreadyPaidAmount) + parseFloat(item.PaidAmount);
                    });

                    $scope.CurrentPaymentSummaryList = $scope.paymentSummaryList.filter(x => x.OTSRePaymentSummaryId == $scope.OTSRePaymentSummaryId);
                    $scope.OldPaymentSummaryList = $scope.paymentSummaryList.filter(x => parseInt(x.OTSRePaymentSummaryId) < parseInt($scope.OTSRePaymentSummaryId));
                }
            }).
            error(function () {
                alert("Error while fetching OTS application data.");
                $(".se-pre-con").fadeOut("slow");
            });
    }


  
    $scope.OnPageLoad();
    $scope.GetApplicationDetails();
});