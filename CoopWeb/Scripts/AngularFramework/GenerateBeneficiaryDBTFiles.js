
var MasterMod = angular.module('gbdbtApp', ['angularUtils.directives.dirPagination']);
MasterMod.controller("gbdbtCnt", function ($rootScope, $scope, $http) {
    $scope.objRFT = {};
    $scope.dbtFileList = [];
    $scope.TXNId = "";
    $scope.EsignOTP = "";
    $scope.IS_BILL_GENERATED = false;
    $scope.IS_BILL_ESIGNED = false;
    $scope.IS_XML_GENERATED = false;
    $scope.IS_XML_ESIGNED = false;
    $scope.IS_XML_ENCRYPTED = false;
    $scope.selectedItem = {};

    $scope.dataPayload = {};
    $scope.esignData = {};

    // $scope.ShowOTPDiv = false;
    const urlParams = new URLSearchParams(window.location.search);
    $scope.id_enc = urlParams.get('id_enc');
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
        }
        $(".se-pre-con").fadeOut("slow");
        $scope.Get_RFTDBT_Details();

    };

    $scope.Get_RFTDBT_Details = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post("/PMKisan/Get_RFTDBT_Details", { id_enc: $scope.id_enc }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.objRFT = JSON.parse(data.Result.Data)[0];

            }).error(function (data, status) {
                console.log(data);
                $(".se-pre-con").fadeOut("slow");
            });

    };
    $scope.GenerateBeneficiaryDBTFiles = function () {
        const dbtDate = document.getElementById('dbtDate').value;
        const selectedDate = new Date(dbtDate);
        $(".se-pre-con").fadeIn("slow");
        $http.post("/PMKisan/GenerateBeneficiaryDBTFiles", { id_enc: $scope.id_enc, dbt_date: selectedDate.toISOString().split('T')[0] }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.Result.Message == 'Success')
                    alert("Files generated successfully.");
                $scope.GetBeneficiaryDBTFiles();
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });

    };
    $scope.GenetateXML = function (id_enc) {
        if (!confirm("Are you want to generate XML document?"))
            return;
        $(".se-pre-con").fadeIn("slow");
        $http.post("/PMKisan/GenerateDBT_XML", { id_enc: id_enc }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.Result.Message == 'Success')
                    alert("SLIA document generated successfully.");
                $scope.GetBeneficiaryDBTFiles();
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.eSignXML = function (item) {
        if (!confirm("Are you want to eSign this docuent?"))
            return;
        $scope.selectedItem = item;
        $('#esignModal').modal();
    };
    $scope.eSignXMLDoc = function (id_enc) {
        $(".se-pre-con").fadeIn("slow");
        $http.post("/PMKisan/EsignXML", { id_enc: id_enc, password: $scope.EsignPassword, type: 1 }).
            success(function (data, status) {
                $('#esignModal').modal('hide');
                $(".se-pre-con").fadeOut("slow");
                if (data.Result.Data == 1) {
                    alert("Document eSigned successfully.");
                    $scope.GetBeneficiaryDBTFiles();
                }
                else if (data.Result.Data == 2)
                    alert("You entered incorrect password.");
                else
                    alert("Server error occurred.");
                $scope.EsignPassword = "";
            }).
            error(function (data, status) {
                $('#esignModal').modal('hide');
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.encryptXML = function (id_enc) {
        if (!confirm("Are you want to encrypt this docuent?"))
            return;
        $(".se-pre-con").fadeIn("slow");
        $http.post("/PMKisan/EncryptXML", { id_enc: id_enc }).
            success(function (data, status) {
                if (data.Result.Message == 'Success') {
                    alert("Document encrypted successfully.");
                    $scope.GetBeneficiaryDBTFiles();
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetBeneficiaryDBTFiles = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post("/PMKisan/GetBeneficiaryDBTFiles", { id_enc: $scope.id_enc }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (JSON.parse(data.Result.Data).length > 0) {
                    $scope.dbtFileList = JSON.parse(data.Result.Data);
                    if ($scope.dbtFileList.filter(item => item.IS_XML_GENERATED === 'True').length > 0) {
                        $scope.IS_XML_GENERATED = true;
                    }
                    if ($scope.dbtFileList.filter(item => item.IS_BILL_ESIGNED === 'True').length > 0) {
                        $scope.IS_BILL_ESIGNED = true;
                    }
                    if ($scope.dbtFileList.filter(item => item.IS_XML_ESIGNED === 'True').length > 0) {
                        $scope.IS_XML_ESIGNED = true;
                    }
                    if ($scope.dbtFileList.filter(item => item.IS_XML_ENCRYPTED === 'True').length > 0) {
                        $scope.IS_XML_ENCRYPTED = true;
                    }
                    if ($scope.dbtFileList.filter(item => item.IS_BILL_GENERATED === 'True').length > 0) {
                        $scope.IS_BILL_GENERATED = true;
                    }
                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GenetateRFT_BILL = function (id_enc) {
        if (!confirm("Are you want to generate RFT Bill?"))
            return;
        $(".se-pre-con").fadeIn("slow");
        $http.post("/PMKisan/GenerateRFT_BILL", { id_enc: id_enc }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");

                $scope.GetBeneficiaryDBTFiles();
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.EsignRFT_BILL = function () {
        if (!confirm("Are you want to esign RFT Bill?"))
            return;

        if ($scope.EsignOTP == null || $scope.EsignOTP == undefined || $scope.EsignOTP == "") {
            alert(_language == 'en-us' ? 'Please Enter Valid OTP.\n' : 'कृपया मान्य ओटीपी दर्ज करें|\n');
            return;
        }



        $(".se-pre-con").fadeIn("slow");

        $http.post("/PMKisan/EsignRFT_BILL", { id_enc: $scope.IdEnc, TXN: $scope.TXNId, OTP: $scope.EsignOTP }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $('#otpModal').modal('hide');
                $scope.IdEnc = 0;
                ShowOkMessage("", data.Result.Message);
                $scope.GetBeneficiaryDBTFiles();
            }).
            error(function (data, status) {
                $scope.IdEnc = 0;
                $('#otpModal').modal('hide');
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.GenerateOTP = function (id_enc) {
        $scope.TXNId = "";
        $scope.EsignOTP = "";
        $scope.IdEnc = id_enc;


        $http.get("../ApplicationFlow/GenerateAadhaarOTPForEsing").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.indexOf('NO#') >= 0) {
                    alert("Due to some issue OTP not send");
                } else {
                    if (data != "LOGOUT") {
                        $scope.TXNId = data;
                        alert(_language == 'en-us' ? 'OTP has been sent to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n');
                        // $scope.ShowOTPDiv = true;
                        $('#otpModal').modal('show');
                        $(".se-pre-con").fadeOut("slow");
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



        /*
     AadhaarAuthEsign($http, function (authData) {
         // debugger;                       
         if (authData.TransId != '' && (authData.TransId).indexOf('NO') < 0) {
             $scope.TXNId = data;
             VerifyOTP();

         }
         else if (authData.TransId == "NO#Cancelled By User") { }
         else {
             alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
         }
        
     });*/

        //$scope.VerifyOTP = function () {

        //    $scope.saveDataInDB();
        //}


    }
    $scope.VerifyOTP = function () {

        $scope.EsignRFT_BILL();
    }
    $scope.ResendOTP = function () {
        $scope.EsignOTP = "";
        $scope.GenerateOTP($scope.IdEnc);

    }
    $scope.downloadZip = function (type, isEsigned) {
        $http({
            method: 'GET',
            url: '/PMKisan/DownloadAllBill?id_enc=' + $scope.id_enc + '&fileType=' + (type ?? 1) + '&isEsigned=' + (isEsigned ?? 0),
            responseType: 'arraybuffer'
        }).then(function (response) {
            var fileName = "";
            if ((type ?? 1) == 1)
                fileName = "RFT_BILL.zip";
            else if ((type ?? 1) == 2)
                fileName = "XML_SLIA_FILES.zip";
            else if ((type ?? 1) == 2 && (isEsigned ?? 0) == 1)
                fileName = "XML_SLIA_ESIGNED_FILES.zip";
            else
                fileName = "XML_ENCRYPTED_SLIA_FILES.zip";
            var blob = new Blob([response.data], { type: 'application/zip' });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = fileName;
            link.click();
        }, function (error) {
            console.error('Error downloading ZIP file:', error);
        });
    };

    //Esign 2.1 Added By Narpat Kumawat on 15-06-2026
    $scope.RedirectToESPportal = function (IdEnc) {
        $scope.IdEnc = IdEnc;
        if (!confirm("Are you want to esign RFT Bill?"))
            return;

        $scope.EsignRFTBill();
    };
    $scope.EsignRFTBill = function () {
        $scope.GenerateRFTBill().then(function (data) {
            $scope.dataPayload = data.data;
            $scope.esignData = {
                SiteUrl: SiteUrl,
                DocType: $scope.dataPayload.DocType,
                ReturnPath: $scope.dataPayload.ReturnPath,
                DocumentPath: $scope.dataPayload.DocPath,
                DocumentName: $scope.dataPayload.DocName,
                docid: $scope.IdEnc,
                SocietyIDEnc: $scope.IdEnc,
                Remark: "",
                StatusId: 0,
                IsAmendment: "",
                AmendmendAmount: "",
                ReceiptNumber: "",
                Location: $scope.dataPayload.Location,
                Desination: $scope.dataPayload.Designation
            };
            if (!$scope.esignData.DocumentPath || !$scope.esignData.DocumentName) {
                alert("Not valid document");
                return;
            }
            EsignCommonData.validateEsign($http, $scope.esignData, function () {
                EsignCommonData.startEsign($http, $scope.esignData, function (xml) {
                    EsignCommonData.redirectToESP(xml);
                });
            });

        }).catch(function (err) {
            console.error(err);
            alert("Error generating eSign certificate");
        });
    };
    $scope.GenerateRFTBill = function () {
        $(".se-pre-con").fadeIn("slow");
        return $http.post("/PMKisan/GenerateRFTBillLatter", { id_enc: $scope.IdEnc }).success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            return data;
        }).error(function (data, status) {
            $scope.IdEnc = 0;
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.GetBeneficiaryDBTFiles();
    $scope.OnPageLoad();
});
