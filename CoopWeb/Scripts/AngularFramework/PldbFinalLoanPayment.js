var pldbApp = angular.module("pldbApp", ['angularUtils.directives.dirPagination']);
pldbApp.controller("pldbCtrl", function ($scope, $http) {
    $scope._language = 'en-us';
    $scope.PaymentModeList = [];
    $scope.ApplicantDetails = {};
    $scope.ApplicantDetails.IsAccountCloseInSinglePayment = 0;
    $scope.payFObj = {};
    $scope.dataPayload = {};
    $scope.esignData = {};
    const urlParams = new URLSearchParams(window.location.search);
    $scope.otsIdEnc = urlParams.get('otsEncId');
    //$scope.otsIdEnc = '0x020000001BAC96EAD32DD2F2D3E1BFB470447CB87A7B597BAC6E278D926C454B3240DB1C';
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
            }).
            error(function (data, status) {
                $scope.PaymentModeList = [];
            });
    };
    $scope.GetApplicationDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/PLDB/GetOtsApplicantDetails?otsIdEnc=" + $scope.otsIdEnc + "&summaryId=0" + "&type=1&isAccountCloseInSinglePayment=" + $scope.IsAccountClose).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data != null) {
                    $scope.ApplicantDetails = JSON.parse(data.Result.Data.ApplicantDetail)[0];
                    $scope.paymentSummaryList = JSON.parse(data.Result.Data.PaymentSummary);
                    $scope.FinalPaymentSummaryList = JSON.parse(data.Result.Data.FinalPaymentSummaryList);
                    $scope.AlreadyPaidAmount = 0;
                    if ($scope.ApplicantDetails.IsAccountCloseInSinglePayment == 1)
                        $scope.IsAccountClose = 1;

                    $scope.payFObj = {
                        Actual: parseFloat($scope.ApplicantDetails.Actual || 0),
                        InsurancePremium: parseFloat($scope.ApplicantDetails.InsurancePremium || 0),
                        OverdueRecoveryTotal: parseFloat($scope.ApplicantDetails.OverdueRecoveryTotal || 0),
                        CurrentActual: parseFloat($scope.ApplicantDetails.CurrentActual || 0),
                        CurrentInterest: parseFloat($scope.ApplicantDetails.CurrentInterest || 0),
                        CurrentPenalInterest: parseFloat($scope.ApplicantDetails.CurrentPenalInterest || 0) + parseFloat($scope.ApplicantDetails.CurrentPenalInterestAmount || 0),
                        CurrentRecoveryTotal: parseFloat($scope.ApplicantDetails.CurrentRecoveryTotal || 0),
                        RecoveryCharge: parseFloat($scope.ApplicantDetails.RecoveryCharge || 0),
                        CurrentOtherAmount: parseFloat($scope.ApplicantDetails.M_OtherAmount || 0),
                        SubTotal: parseFloat($scope.ApplicantDetails.SubTotal || 0),
                        AlreadyPaidAmount: parseFloat($scope.ApplicantDetails.AlreadyPaidAmount || 0),
                        CurrentPenalInterestAmount: parseFloat($scope.ApplicantDetails.CurrentPenalInterestAmount || 0),
                    };

                }
            }).
            error(function () {
                alert("Error while fetching OTS application data.");
                $(".se-pre-con").fadeOut("slow");
            });
    }
    $scope.SavePayment = function () {


        if (!confirm('Are you want to submit payment'))
            return;

        $(".se-pre-con").fadeIn("slow");

        $scope.payFObj.OtsApplicationId = $scope.ApplicantDetails.PLDB_OTS_SchemeID;
        $scope.payFObj.OtsIdEnc = $scope.otsIdEnc;
        //$scope.payFObj.CurrentOtherAmount = $scope.payFObj.CurrentPenalInterestAmount;
        $http.post(SiteUrl + "/PLDB/SaveFinalPaymentSummary", $scope.payFObj).success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            if (data.Result.Message == "Success") {
                $('#repaymentmodal').modal('hide');
                ShowOkMessage("Success", _language == 'en-us' ? 'Data saved successfully\n' : 'डेटा सहेजा गया है\n', function () {
                    $scope.GetApplicationDetails();

                });
            }
            else if (data.Result.Message == "Duplicate") {
                $('#repaymentmodal').modal('hide');
                ShowOkMessage("Duplicate", _language == 'en-us' ? 'Data Already exists.\n' : 'डेटा पहले से मौजूद है\n', function () {
                    $scope.GetApplicationDetails();
                });
            }
            else {
                ShowOkMessage("Error", _language == 'en-us' ? 'Some Error Occurred.\n' : 'कुछ त्रुटि हुई है\n', function () { });
            }
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
        });
    }

    $scope.DownloadFinalPaymentReceipt = function (Id) {
        url = '../PLDB/ots_fr_receipt?otsIdEnc=' + $scope.otsIdEnc + '&SummaryId=' + Id;
        window.open(url, "_blank");
    }


    $scope.BackToPage = function () {
        window.location = '../PLDB/OTSApplicationList';
    }

    $scope.AvdhiparVasooliTotal = function () {
        $scope.payFObj.OverdueRecoveryTotal = parseFloat($scope.payFObj.Actual) + parseFloat($scope.payFObj.InsurancePremium) + parseFloat($scope.payFObj.OtherAmount);
        $scope.payFObj.SubTotal = parseFloat($scope.payFObj.CurrentRecoveryTotal) + parseFloat($scope.payFObj.OverdueRecoveryTotal)
    }
    $scope.ChaluVasooliTotal = function () {
        $scope.payFObj.CurrentRecoveryTotal = parseFloat($scope.payFObj.CurrentActual) + parseFloat($scope.payFObj.CurrentInsurancePremium)
            + parseFloat($scope.payFObj.CurrentInterest) + parseFloat($scope.payFObj.CurrentPenalInterest) + parseFloat($scope.payFObj.CurrentOtherAmount);

        $scope.payFObj.SubTotal = parseFloat($scope.payFObj.CurrentRecoveryTotal) + parseFloat($scope.payFObj.OverdueRecoveryTotal)
    }

    $scope.FinalReceiptGenerate = function () {
        AadhaarAuth("", $http, "OTP", true, function (data) {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/PLDB/FinalReceiptGenerate", { "TXN": data.TransId, "_model": $scope.ApplicantDetails }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    //$scope.ApplicationFlowMaster.OfficeId = '0';
                    //$scope.OfficerData = data;
                    //if (data.length > 0) {
                    //    $("#ddlOffice").attr("disabled", 'disabled');
                    //    $scope.ApplicationFlowMaster.OfficeId = data[0].userid;
                    //}
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");

                });

        });


    }



    $scope.repaymentmodalOpen = function () {

        $('#repaymentmodal').modal('show');


        /* data - target="#repaymentmodal"*/
    }

    $scope.UploadeAadharJanaadharFileInDB = function (OTS) {
        //debugger;

        if ($scope.Aadhar_Janaadhar_no == "") {
            alert('Please Enter the Aadhar/ Jan-Aadhar No.');
            return;
        }

        if (!confirm('Are you sure you want to upload file?')) {
            var fileInputs = document.getElementsByClassName('fileuploader');
            for (var i = 0; i < fileInputs.length; i++) {
                fileInputs[i].value = '';
            }
            return;
        }

        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();
        formData.append("id", 0);
        formData.append("OtsIdEnc", $scope.otsIdEnc);
        formData.append("DocAadhar", ($scope.DocAadhar ?? null));
        //formData.append("DocTaamil", ($scope.DocTaamil ?? null));
        formData.append("Aadhar_Janaadhar_no", ($scope.Aadhar_Janaadhar_no ?? ''));

        $http.post(SiteUrl + "/PLDB/SaveAadharOTSFile", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                if (data.docId > 0) {
                    $('#Aadharmodal').modal('hide');
                    alert("File upload successfully");
                    $scope.GetApplicationDetails();
                }

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    }


    $scope.UploadReportFile = function (file, type) {
        const MAX_SIZE = 5 * 1024 * 1024; // 5MB
        const allowedTypes = ['image/jpeg', 'image/png', 'image/gif', 'application/pdf'];

        if (file && file[0]) {
            if (file[0].size > MAX_SIZE) {
                alert('File size exceeds 5MB');
                return;
            }

            if (!allowedTypes.includes(file[0].type)) {
                alert('Please upload a valid image or PDF file');
                return;
            }

            $scope.DocAadhar = file[0];

        } else {
            alert('No file selected');
        }

    }

    $scope.FinalGenerateCertificate = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/PLDB/FinalCetrificateGenerate", { _model: $scope.ApplicantDetails }).success(function (data) {
            $(".se-pre-con").fadeOut("slow");
            $("#FinalCertificatemodal").hide();
            if (data.Result.Message == "Success") {
                ShowOkMessage("Success", _language == 'en-us' ? 'Data saved successfully\n' : 'डेटा सहेजा गया है\n', function () { });

            }
            else if (data.Result.Message == "Duplicate") {
                ShowOkMessage("Duplicate", _language == 'en-us' ? 'Data Already exists.\n' : 'डेटा पहले से मौजूद है\n', function () { });
            }
            else {
                ShowOkMessage("Error", _language == 'en-us' ? 'Some Error Occurred.\n' : 'कुछ त्रुटि हुई है\n', function () { });
            }
        }).error(function () {
            $(".se-pre-con").fadeOut("slow");
            $("#FinalCertificatemodal").hide();
        });
    };
    $scope.RedirectToESPportal = function () {
        if ($scope.ApplicantDetails.Status == 4) {
            $scope.OTSCertificateEsign();
            $("#FinalCertificatemodal").hide();
        } else {
            $scope.FinalGenerateCertificate();
            let data = $("#hdnResponse").val();
            ShowOkMessage("", data, function () { });
        }
    };
    $scope.OTSCertificateEsign = function () {
        $scope.OTSCertificateGeneratedEsign().then(function (data) {
            $scope.dataPayload = data;
            $scope.esignData = {
                SiteUrl: SiteUrl,
                DocType: $scope.dataPayload.DocType,
                ReturnPath: $scope.dataPayload.ReturnPath,
                DocumentPath: $scope.dataPayload.DocPath,
                DocumentName: $scope.dataPayload.DocName,
                docid: $scope.ApplicantDetails.PLDB_OTS_SchemeID,
                SocietyIDEnc: $scope.ApplicantDetails.LoanAccNo,
                Remark: $scope.ApplicantDetails.Remark,
                StatusId: $scope.ApplicantDetails.Status,
                IsAmendment: "",
                AmendmendAmount: "",
                ReceiptNumber: "",
                details: $scope.ApplicantDetails,
                Location: $scope.dataPayload.Location,
                Desination: $scope.dataPayload.Designation
            };

            if (!$scope.esignData.DocumentPath || !$scope.esignData.DocumentName) {
                alert("Not valid document");
                return;
            }

            EsignCommonData.validateEsign($http, $scope.esignData, function () {
                EsignCommonData.startEsignOTS($http, $scope.esignData, function (xml) {
                    EsignCommonData.redirectToESP(xml);
                });
            });

        }).catch(function (err) {
            console.error(err);
            alert("Error generating eSign certificate");
        });
    };
    $scope.OTSCertificateGeneratedEsign = function () {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/PLDB/FinalReceiptGenerateNewEsign", {
            _model: $scope.ApplicantDetails
        }).then(function (response) {
            $(".se-pre-con").fadeOut("slow");
            return response.data;
        }).catch(function (error) {
            console.error(error);
            alert("Error generating certificate");
            throw error;
        })
    };
    $scope.BackToPage = function () {
        window.location = '../PLDB/PldbFinalLoanPayment?otsEncId=' + $scope.otsIdEnc;
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
