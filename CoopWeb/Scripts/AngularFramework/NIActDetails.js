var pldbApp = angular.module("pldbApp", ['angularUtils.directives.dirPagination']);
pldbApp.controller("pldbCtrl", function ($scope, $http) {
    $scope._language = 'en-us';
    $scope.niObj = {};

    const urlParams = new URLSearchParams(window.location.search);
    $scope.otsIdEnc = urlParams.get('otsEncId');
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

        $scope.GetApplicationDetails();
    };


    $scope.GetApplicationDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/PLDB/GetOtsApplicantDetails?otsIdEnc=" + $scope.otsIdEnc + "&summaryId=0" + "&type=1").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data != null) {
                    $scope.ApplicantDetails = JSON.parse(data.Result.Data.ApplicantDetail)[0];
                    if ($scope.ApplicantDetails != null) {
                        $scope.niObj.IsChequesCollectedFromLoanee = $scope.ApplicantDetails.IsChequesCollectedFromLoanee == 'True' ? true : false;
                        $scope.niObj.IsChequesPresentedBankForClearing = $scope.ApplicantDetails.IsChequesPresentedBankForClearing == 'True' ? true : false;
                        if ($scope.niObj.IsChequesPresentedBankForClearing) {
                            $scope.niObj.ChequeNo = $scope.ApplicantDetails.ChequeNo;
                            $scope.niObj.Bankdetails = $scope.ApplicantDetails.Bankdetails;
                            $scope.niObj.ChequeDate = new Date(($scope.ApplicantDetails.ChequeDate ?? null));
                            $scope.niObj.ChequeAmt = parseFloat($scope.ApplicantDetails.ChequeAmt);
                        }
                        $scope.niObj.IsChequeDishonoured = $scope.ApplicantDetails.IsChequeDishonoured == 'True' ? true : false;
                        $scope.niObj.IsChequeDishonourActionTaken = $scope.ApplicantDetails.IsChequeDishonourActionTaken == 'True' ? true : false;
                        $scope.niObj.IsSettlementProposed = $scope.ApplicantDetails.IsSettlementProposed == 'True' ? true : false;
                        if ($scope.niObj.IsChequeDishonourActionTaken) {
                            $scope.niObj.ActionTakenDate = new Date(($scope.ApplicantDetails.ActionTakenDate ?? null));
                            $scope.niObj.CaseNo = $scope.ApplicantDetails.CaseNo;
                            $scope.niObj.CourtDetails = $scope.ApplicantDetails.CourtDetails;
                            $scope.niObj.LastHearingDate = new Date(($scope.ApplicantDetails.LastHearingDate ?? null)); 
                            $scope.niObj.NextHearingDate = new Date(($scope.ApplicantDetails.NextHearingDate ?? null));
                            $scope.niObj.Judgement = $scope.ApplicantDetails.Judgement;
                            $scope.niObj.JudgementFavourOfBank = $scope.ApplicantDetails.JudgementFavourOfBank;
                            $scope.niObj.JudgementFavourOfLoanee = $scope.ApplicantDetails.JudgementFavourOfLoanee;
                            $scope.niObj.RecoveryAmt = parseFloat($scope.ApplicantDetails.RecoveryAmt);
                            $scope.niObj.DishonourActionReason = $scope.ApplicantDetails.DishonourActionReason;
                        }
                        $scope.niObj.OTSSettlementProposed = $scope.ApplicantDetails.OTSSettlementProposed;
                    }

                }
            }).
            error(function () {
                alert("Error while fetching OTS application data.");
                $(".se-pre-con").fadeOut("slow");
            });
    }

    $scope.SaveNIActDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        if ($scope.niObj.IsChequesPresentedBankForClearing == true) {
            var chequeDateObj = new Date(($scope.niObj.ChequeDate ?? new Date()));
            $scope.niObj.ChequeDate = chequeDateObj.getFullYear() + '/' + (chequeDateObj.getMonth() + 1) + '/' + chequeDateObj.getDate();
        }
        else
            $scope.niObj.ChequeDate = null;

        if ($scope.niObj.IsChequeDishonourActionTaken == true) {
            var actionTakenDateObj = new Date(($scope.niObj.ActionTakenDate ?? new Date()));
            $scope.niObj.ActionTakenDate = actionTakenDateObj.getFullYear() + '/' + (actionTakenDateObj.getMonth() + 1) + '/' + actionTakenDateObj.getDate();

            var lastHearingDateObj = new Date(($scope.niObj.LastHearingDate ?? new Date()));
            $scope.niObj.LastHearingDate = lastHearingDateObj.getFullYear() + '/' + (lastHearingDateObj.getMonth() + 1) + '/' + lastHearingDateObj.getDate();

            var nextHearingDateObj = new Date(($scope.niObj.NextHearingDate ?? new Date()));
            $scope.niObj.NextHearingDate = nextHearingDateObj.getFullYear() + '/' + (nextHearingDateObj.getMonth() + 1) + '/' + nextHearingDateObj.getDate();
        }
        else {
            $scope.niObj.ActionTakenDate = null;
            $scope.niObj.LastHearingDate = null;
            $scope.niObj.NextHearingDate = null;
        }
        $scope.niObj.OtsIdEnc = $scope.otsIdEnc;
        $scope.niObj.IsChequesCollectedFromLoanee = $scope.niObj.IsChequesCollectedFromLoanee == 1 ? true: false;
        $scope.niObj.IsChequesPresentedBankForClearing = $scope.niObj.IsChequesPresentedBankForClearing == 1 ? true: false;
        $scope.niObj.IsChequeDishonoured = $scope.niObj.IsChequeDishonoured == 1 ? true: false;
        $scope.niObj.IsChequeDishonourActionTaken = $scope.niObj.IsChequeDishonourActionTaken == 1 ? true : false;
        $scope.niObj.IsSettlementProposed = $scope.niObj.IsSettlementProposed == 1 ? true : false;

        $scope.niObj.ChequeAmt = ($scope.niObj.ChequeAmt ?? 0);
        $scope.niObj.RecoveryAmt = ($scope.niObj.RecoveryAmt ?? 0);

        $http.post(SiteUrl + "/PLDB/SaveNIActDetails", $scope.niObj).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.Result.Message == "Success") {
                    alert($scope._language == 'en-us' ? 'Data Saved Sucessfully' : ' डेटा सफलतापूर्वक सहेजा गया।');
                    window.location.reload();
                }
                else {
                    ShowModelAlert($scope._language == 'en-us' ? 'OTS 2025-26 Scheme :: ' : 'OTS 2025-26 Scheme ::', 'An Error Occurred Please Try Again Later');
                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    }

    $scope.OnPageLoad();
});
