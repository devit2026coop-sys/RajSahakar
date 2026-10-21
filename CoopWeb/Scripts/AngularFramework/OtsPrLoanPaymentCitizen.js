var pldbApp = angular.module("pldbApp", ['angularUtils.directives.dirPagination']);
pldbApp.controller("pldbCtrl", function ($scope, $http) {
    $scope._language = 'en-us';
    $scope.payObj = {};
    $scope.obj = {};
    $scope.obj.LoanAccNo = "";
    $scope.obj.MobileNo = "";
    $scope.obj.RegistrationNo = "";
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
        $scope.PLDBListData();
    };

    $scope.SearchApplicationDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/PLDB/SearchOtsCitizenApplicantDetails?LoanAccNo=" + $scope.obj.LoanAccNo + "&MobileNo=" + $scope.obj.MobileNo + "&RegistrationNo=" + $scope.obj.RegistrationNo + "&PldbId=" + $scope.obj.PldbId + "&BranchId=" + $scope.obj.BranchId).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data != null) {
                    $scope.ApplicantDetailsList = JSON.parse(data.Result.Data.ApplicantDetail);
                }
            }).
            error(function () {
                alert("Error while fetching OTS application data.");
                $(".se-pre-con").fadeOut("slow");
            });
    }

    $scope.today = new Date();
    $scope.AccountCloseDate = new Date($scope.today);
    $scope.getDaysAfter31Mar2025 = function () {
        let fromDate = new Date("2025-03-31");
        let today = new Date();
        let diffInMs = today.getTime() - fromDate.getTime();
        let diffInDays = Math.floor(diffInMs / (1000 * 60 * 60 * 24));
        return diffInDays > 0 ? diffInDays : 0;
    };
    $scope.GetApplicationDetails = function (EncPldbOtsSchemeId) {
        $(".se-pre-con").fadeIn("slow");
        var dateObj = new Date(($scope.AccountCloseDate ?? $scope.today));
        $scope.FormattedAccountCloseDate = (dateObj.getMonth() + 1) + '/' + dateObj.getDate() + '/' + dateObj.getFullYear();
        $http.get(SiteUrl + "/PLDB/GetOtsApplicantDetails?otsIdEnc=" + EncPldbOtsSchemeId + "&summaryId=0" + "&type=0&isAccountCloseInSinglePayment=" + $scope.IsAccountClose + "&accountCloseDate=" + ($scope.FormattedAccountCloseDate ?? null)).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data != null) {
                    $scope.ApplicantDetails = JSON.parse(data.Result.Data.ApplicantDetail)[0];
                    $scope.ApplicantDetails.AlreadyPaidAmount = parseFloat($scope.ApplicantDetails.AlreadyPaidAmount);
                    $scope.ApplicantDetails.EstimatedTotalAmount = parseFloat($scope.ApplicantDetails.EstimatedTotalAmount);

                    $scope.payObj.PaidAmount = $scope.ApplicantDetails.EstimatedTotalAmount - $scope.ApplicantDetails.AlreadyPaidAmount;
                    $scope.paymentSummaryList = JSON.parse(data.Result.Data.PaymentSummary);
                    if ($scope.ApplicantDetails.IsAccountCloseInSinglePayment == 1)
                    {
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
    
    $scope.DownloadPreLoanPaymentReceipt = function (Id, OtsIdEnc) {
        url = '../PLDB/ots_pr_receipt?otsIdEnc=' + OtsIdEnc + '&SummaryId=' + Id;
        window.open(url, "_blank");
    }

    $scope.PLDBList = [];
    $scope.PLDBListData = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.obj.PldbId = "0";
        $scope.obj.BranchId = "0";

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=0").
            success(function (data, status) {
                $scope.PLDBList = data;
                $(".se-pre-con").fadeOut("slow");

                if ($("#hdnPldbId").val() != "0") {
                    $scope.PLDBBranchList();
                }
            }).
            error(function (data, status) {
                $scope.PLDBList = [];
            });
    };
    $scope.PLDBBranches = [];
    $scope.PLDBBranchList = function (branch_ID) {
        $scope.obj.BranchId = "0";
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + ($scope.obj.PldbId ?? 0)).
            success(function (data, status) {
                $scope.PLDBBranches = data;
                $(".se-pre-con").fadeOut("slow");

            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.OnPageLoad();


    $scope.downloadFile = function (OTS) {
        angular.forEach($scope.OTSList, function (item) {
            item.selected = false;
        });
        $scope.SelectedOTSList = [];
        $scope.SelectedOTSList.push(OTS);
        $scope.DownloadMultipleOTSForm();
    }


    $scope.DownloadMultipleOTSForm = function () {
        if ($scope.SelectedOTSList.length <= 0) {
            alert("Please select Pdf which should be download");
            return;
        }
        $scope.SelectedOTS = {};
        $scope.SelectedOTS.PLDB_OTSSchemeData = $scope.SelectedOTSList;

        $http.post(SiteUrl + "/PLDB/BulkOTSFilesGenerated", { "_Model": $scope.SelectedOTS }, {
            responseType: 'arraybuffer'
        }).

            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.byteLength > 0) {

                    const blob = new Blob([data], { type: 'application/pdf' });

                    // Create a download link
                    const url = URL.createObjectURL(blob);
                    const a = document.createElement('a');
                    a.href = url;
                    var filename = "";
                    if ($scope.SelectedOTSList.length == 1) {
                        filename = $scope.SelectedOTSList[0].ApplicantName + "_" + $scope.SelectedOTSList[0].LoanAccNo.replace(/\//g, '-') + ".pdf";
                    }
                    else if ($scope.SelectedOTSList.length > 1 && $("#hdnPldbBranchId").val() != "0") {
                        filename = $scope.SelectedOTSList[0].PLDBBranch + "OtsApplicationList.pdf";

                    }
                    else if ($scope.SelectedOTSList.length > 1 && $("#hdnPldbId").val() != "0") {
                        filename = $scope.SelectedOTSList[0].PLDBName + "OtsApplicationList.pdf";

                    }
                    else {
                        filename = "BulkOtsApplicationList.pdf";
                    }
                    a.download = filename;
                    document.body.appendChild(a); // Needed for Firefox
                    a.click();
                    document.body.removeChild(a);
                    URL.revokeObjectURL(url);

                    angular.forEach($scope.OTSList, function (item) {
                        item.selected = false;
                    });
                    $scope.SelectedOTSList = [];
                }
                else {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("Error", _language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');

                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while insert");
            });
    }
});

