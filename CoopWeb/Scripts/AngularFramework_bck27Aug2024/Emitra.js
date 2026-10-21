var EmitraMod = angular.module('EmitraModule', ['angularUtils.directives.dirPagination']);
EmitraMod.controller("EmitraCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.Datacontainer = [];
    $scope.PrintShow = false;
    $scope.recordstblshow = false;
    $scope.VerificationList = [];
    $scope.getfunSocietyPayment = function () {
        $scope.textchange = 'true';
        $scope.Showgride = true;
        $scope.showotpblock = false;
        $http.get(SiteUrl + "/Emitra/EmitraFeePayment/?SocietyRegistrationNo=" + $scope.SocietyRegistration.SocietyRegistrationNo).
            success(function (data, status) {
                if (data.length == 0) {
                    ShowModelAlert(_language == 'en-us' ? "Emitra Fee Payment" : 'ई-मित्रा शुल्क भुगतान', _language == 'en-us' ? 'This society is not eligible for payment now.' : 'यह सोसाइटी अभी पेमेंट के लिए योग्य  नहीं  है');

                    $scope.Showgride = false;
                    $scope.textchange = false;
                }
                else {
                    $scope.SocietyRegdata = data;
                    if (data[0].StatusId == '33') {
                        $('#abc').show();
                        $scope.IsForPayment = true;
                    }
                    else {
                        $('#abc').hide();
                        $scope.IsForPayment = false;
                    }
                    $scope.textchange = 'true';
                }
            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
            });
    }
    $scope.UploadEnclouserFile = function (file) {
        $scope.BankPassbookFile = file[0];
    }


    $scope.SaveBankDetails = function () {
        //alert($scope.SocietyRegdata[0].SocietyIdEnc);
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.BankPassbookFile == undefined || $scope.BankPassbookFile == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Account Opening Certificate(PDF)' : '\n <br /> कृपया खाता खोलने का प्रमाण पत्र अपलोड करें (पीडीएफ) |';

        }
        if ($('#txtOpeningBalance').val() == '' || $('#txtOpeningBalance').val() == undefined || $('#txtOpeningBalance').val() == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Account Opening Certificate(PDF)' : '\n <br /> कृपया खाता खोलने का प्रमाण पत्र अपलोड करें (पीडीएफ) |';
        }
        if (message.length > 29) {
            ShowModelAlert("Society Profiling", message);
            return false;
        }


        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();
        formData.append("SocietyIDEnc", $scope.SocietyRegdata[0].SocietyIdEnc);
        formData.append("BankPassbookFile", $scope.BankPassbookFile);
        formData.append("OpeningBalance", $('#txtOpeningBalance').val());
        formData.append("@flag", 1);
        $http.post(SiteUrl + "/society/SaveBankDetails", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                $scope.DivResponce = true;
                $('#Responce').html(data)
                $(".se-pre-con").fadeOut("slow");
                $scope.recordstblshow = false;
                //location.reload();
            }).
            error(function (data, status) {
                $scope.hidebankbtn = true;
                $(".se-pre-con").fadeOut("slow");
            });

        $(".se-pre-con").fadeOut("slow");
    };



    $scope.GetfSocietyForBankCUpload = function () {
        $scope.textchange = 'true';
        $(".se-pre-con").fadeIn("slow");
        $scope.showotpblock = false;
        $http.get(SiteUrl + "/Emitra/EmitraUploadBankCertificate/?SocietyRegistrationNo=" + $scope.SocietyRegistration.SocietyRegistrationNo).
            success(function (data, status) {
                if (data.length == 0) {
                    ShowModelAlert(_language == 'en-us' ? "Emitra Fee Payment" : 'ई-मित्रा अपलोड सोसाइटी बैंक सर्टिफिकेट', _language == 'en-us' ? 'This society is not eligible for Upload Bank Certificate now.' : 'यह सोसाइटी अभीबैंक सर्टिफिकेट अपलोड के लिए योग्य  नहीं  है');


                    $scope.textchange = false;
                }
                else {
                    $scope.SocietyRegdata = data;
                    if (data[0].StatusId == '14') {
                        $('#abc').show();
                        $scope.IsForPayment = true;
                        $scope.recordstblshow = true;

                    }
                    else {
                        $('#abc').hide();
                        $scope.IsForPayment = false;
                        $scope.recordstblshow = false;
                    }
                    $scope.textchange = 'true';

                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
    }


    $scope.PaymentInitiatByEmitra = function (Registration, IsPermanentSave) {
        $('#abc').attr("disabled", "disabled");
        $http.post(SiteUrl + "/Emitra/FeePaymentByEmitra", { "RegistrationNo": Registration, "IsPermanentSave": IsPermanentSave }).
            success(function (data, status) {

                if (data.split('@')[0] == 'Transaction Successfully Done') {
                    var Msg = '';
                    Msg += '\n <br /> STATUS: Transaction Successfully Done';
                    Msg += '\n <br /> RECEIPTNO: ' + data.split('@')[1];
                    Msg += '\n <br /> DATE: ' + data.split('@')[3];
                    ShowModelAlert("Transaction Status", Msg);
                    $scope.PrintShow = true;
                    return false;
                }
                else {
                    alert(data.split('@')[0]);
                    $scope.PrintShow = false;
                }
            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
            });
    }
    $scope.SearchText = "";
    // START :Get List of Societies
    $scope.GetSocietyList = function () {
        $http.post(SiteUrl + "/Citizen/GetSocietyForFees", { "SearchText": $scope.SearchText }).
            success(function (data, status) {

                $scope.SocietyList = data.DataList;
            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                //  alert('Error while fetching society list data.');
            });
    };
    // END :Get List of Societies

    $scope.PrintPage = function (SocietyIdEnc) {
        var URL = "../Society/SocietyApplicationView?SocietyIDEnc=" + SocietyIdEnc + "~_Citizen_SocietyProfile~";
        window.open(URL, '_blank');
    };
    $scope.GetFeeVerification_old = function (SocietyIdENC, EMitraAppRequestID, SERICEID, UniqueId) {
        $(".blockUI").show();
        $http.post(SiteUrl + "/Citizen/GetTransactionFinalStatus", { "SocietyIdENC": SocietyIdENC, "EMitraAppRequestID": EMitraAppRequestID, "SERICEID": SERICEID, "UniqueId": UniqueId }).
            success(function (result, status) {
                var Msg = '';
                $(".blockUI").hide();
                Msg += '\n <br /> STATUS: ' + (result.MSG == undefined ? result.STATUS : result.MSG);
                Msg += '\n <br /> RECEIPTNO: ' + result.RECEIPTNO;
                Msg += '\n <br /> DATE: ' + result.TRANSACTIONDATE;
                Msg += '\n <br /> AMOUNT: ' + (result.AMT == undefined ? result.PAIDAMOUNT : result.AMT);
                Msg += '\n <br /> TRANSACTIONSTATUS: ' + (result.TRANSACTIONSTATUS == undefined ? result.RESPONSEMESSAGE : result.TRANSACTIONSTATUS);
                ShowModelAlert("Transaction Status", Msg);
                $scope.PrintShow = true;
                $scope.FailedTransactionList = [];

            }).
            error(function (data, status) {
                $(".blockUI").hide();
                $scope.FailedTransactionList = [];
                alert('Error while fetching failed transactionList data.');
            });
    }
   
    $scope.GetFeeVerification = function (SocietyIdENC, EMitraAppRequestID, SERICEID, UniqueId, RegistrationNo) {
        $(".blockUI").show();

        //var data = { "SocietyIdENC": '0x02000000CE78F2A1DDC20B632D4D0E97E9F2255A90F69E72E99553DD4530CD9C802DCB57EA805BA6BF128AB43D056E6F4F7B438B', "EMitraAppRequestID": '400136', "SERICEID": 5531, "UniqueId": 402301, "RegistrationNo": 'COOP/2021/DAUSA/200679' }
        var data = { "SocietyIdENC": SocietyIdENC, "EMitraAppRequestID": EMitraAppRequestID, "SERICEID": SERICEID, "UniqueId": UniqueId, "RegistrationNo": RegistrationNo };
        $http.post(SiteUrl + "/Citizen/GetPaymentTransaction", data).
            success(function (result, status) {
                var Msg = '';
                $(".blockUI").hide();
                Msg += '\n <br /> STATUS: ' + (result.MSG == undefined ? result.STATUS : result.MSG);
                Msg += '\n <br /> RECEIPTNO: ' + result.RECEIPTNO;
                Msg += '\n <br /> DATE: ' + result.TRANSACTIONDATE;
                Msg += '\n <br /> AMOUNT: ' + (result.AMT == undefined ? result.PAIDAMOUNT : result.AMT);
                Msg += '\n <br /> TRANSACTIONSTATUS: ' + (result.TRANSACTIONSTATUS == undefined ? result.RESPONSEMESSAGE : result.TRANSACTIONSTATUS);
                ShowModelAlert("Transaction Status", Msg);
                $scope.PrintShow = true;
                $scope.FailedTransactionList = [];

            }).
            error(function (data, status) {
                $(".blockUI").hide();
                $scope.FailedTransactionList = [];
                alert('Error while fetching failed transactionList data.');
            });
    }
    //$scope.GetFeeVerification();

    $scope.FailedTransactionDetails = function () {
        $(".blockUI").show();
        $http.post(SiteUrl + "/Citizen/GetFailedTransactionList", { "SocietyIdENC": $scope.FailedTransaction.SocietyIdENC, "SocietyRegno": $scope.FailedTransaction.SocietyRegistrationNo, "VerificationId": $scope.FailedTransaction.VerificationId }).
            success(function (data, status) {
                $(".blockUI").hide();
                $scope.FailedTransactionList = data;
                if (data.length > 0)
                    $scope.ShowTable = true;
                else {
                    $scope.ShowTable = false;
                    alert('Record Not Found');
                }
            }).
            error(function (data, status) {
                $(".blockUI").hide();
                $scope.FailedTransactionList = [];
                alert('Error while fetching failed transactionList data.');
            });
    }

    $scope.BackTo = function () {
    };

    // on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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
        $scope.GetSocietyList();

        $http.post(SiteUrl + "/Citizen/CheckUserType").
            success(function (data, status) {
                if (data == "K")
                    $scope.VerificationList.push({ "Id": "3", "VerificationFor": "Society Fees" }, { "Id": "2", "VerificationFor": "Bank Certificate " });
                else
                    $scope.VerificationList.push({ "Id": "3", "VerificationFor": "Society Fees" });
            }).
            error(function (data, status) {
            });
    };

    $scope.ApplicationList = [];

    $scope.SerachTransaction = function () {
        if ($scope.SearchText == '') {
            ShowOkMessage("", "Please Enter Registration Number", function () { });
        } else {
            $scope.GetApplication($scope.SearchText);
        }
    }

    $scope.GetTransaction = function () {
        if ($scope.FailedTransaction.SocietyIdENC != '' && $scope.FailedTransaction.SocietyIdENC != undefined) {
            $scope.GetApplication($scope.FailedTransaction.SocietyIdENC);
        }
    }




    $scope.GetApplication = function (RegistrationNo) {
        if (RegistrationNo != '' && RegistrationNo != undefined) {
            $(".blockUI").fadeIn("slow");
            return $http.post("/Society/GetEmitraTransaction", JSON.stringify({ SearchText: RegistrationNo, ServiceType: 5531 })).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (JSON.parse(data.DataList).length <= 0) {
                        $scope.ApplicationList = [];
                        ShowOkMessage("", "No Record Found", function () { });
                    } else {
                        $scope.ApplicationList = JSON.parse(data.DataList);
                    }
                }).
                error(function (data, status) {
                    $scope.CertificateList = [];
                    $(".blockUI").fadeOut("slow");
                });
        }       
    }

    $scope.GetFeeVerification = function (SocietyIdENC, EMitraAppRequestID, SERICEID, UniqueId, RegistrationNo, ApplicationStatus) {
        $(".blockUI").show();
        //var data = { "SocietyIdENC": '0x02000000CE78F2A1DDC20B632D4D0E97E9F2255A90F69E72E99553DD4530CD9C802DCB57EA805BA6BF128AB43D056E6F4F7B438B', "EMitraAppRequestID": '400136', "SERICEID": 5531, "UniqueId": 402301, "RegistrationNo": 'COOP/2021/DAUSA/200679' }
        var data = { "SocietyIdENC": SocietyIdENC, "EMitraAppRequestID": EMitraAppRequestID, "SERICEID": SERICEID, "UniqueId": UniqueId, "RegistrationNo": RegistrationNo, ApplicationStatus: ApplicationStatus };
        $http.post(SiteUrl + "/Citizen/GetPaymentTransaction", data).
            success(function (result, status) {
                var Msg = '';
                $(".blockUI").hide();
                Msg += '\n <br /> STATUS: ' + (result.MSG == undefined ? result.STATUS : result.MSG);
                Msg += '\n <br /> RECEIPTNO: ' + result.RECEIPTNO;
                Msg += '\n <br /> DATE: ' + result.TRANSACTIONDATE;
                Msg += '\n <br /> AMOUNT: ' + (result.AMT == undefined ? result.PAIDAMOUNT : result.AMT);
                Msg += '\n <br /> TRANSACTIONSTATUS: ' + (result.TRANSACTIONSTATUS == undefined ? result.RESPONSEMESSAGE : result.TRANSACTIONSTATUS);
                ShowModelAlert("Transaction Status", Msg);
                $scope.GetApplication(RegistrationNo);
                $scope.PrintShow = true;
                $scope.FailedTransactionList = [];
            }).
            error(function (data, status) {
                $(".blockUI").hide();
                $scope.FailedTransactionList = [];
                alert('Error while fetching failed transactionList data.');
            });
    }

    $scope.OnPageLoad();
});