var CCSComplaintEditController = angular.module("CCSComplaintEditCnt", ['angularUtils.directives.dirPagination']);
CCSComplaintEditController.controller("CCSComplaintEditMod", function ($scope, $http) {

    $scope.divInsert = false;
    $scope.divComplaintList = false;
    $scope.divAADHARVerificationStep_2 = false;
    $scope.divAADHARVerificationStep_1 = true;

    $scope.IsDisabled = true;
    $scope.Complaint = [];
    $scope.Complaint["FileDetails"] = [];
    $scope.DocumentTypes = [];
    $scope.ShowSubmitButton = true;
    $scope.disablebtnSendOtp = false;

    //START : AADHAR Verification Check
    $scope.CheckAADHARAcceptance = function () {
        if ($('#ckkAADHARAcceptance').prop('checked')) {
            $scope.disablebtnSendOtp = false;
        }
        else {
            $scope.disablebtnSendOtp = true;
        }
    };
    //END : AADHAR Verification Check

    //START : Add New Row
    $scope.AddNewRow = function () {
        $scope.MaxId = $scope.MaxId + 1;

        $scope.Complaint.FileDetails.push({
            'FileTypeId': '1',
            'AccountNo': '',
            'UId': $scope.MaxId,
            'PaidAmount': '',
            'PayDate': '',
            'MaturityAmount': '',
            'MaturityDate': '',
            'FilePathView': '',
            'FileName': '1',
            'File': '',
            'DDLID': "DDLDocTypeID1",
            'IsNewRow': true
        });
    };
    //END: Add New Row

    //START : Add Files For Passbook/Cross Cheque
    $scope.UploadPassbook = function (file, row) {
        if (file.length === 0) {
            $(row).val('');
            $(row).parent().find('.filesize').text('');
            $scope.Passbook = null;
            $('#passbookEntry').val('');
            ShowModelAlert($scope.translation.CCSComplaintEditForm, _language === 'en-us' ? 'Please select file.' : 'कृपया फ़ाइल चुनें।', function () { });
            return;
        }

        var isValid = false;
        var filename = file[0].name;
        var message = '';
        var extension = filename.split("").reverse().join("");
        extension = extension.substring(0, extension.indexOf('.'));
        extension = extension.split("").reverse().join("").toLowerCase();

        if (extension === 'pdf' || extension === 'jpg' || extension === 'jpeg' || extension === 'png') {
            isValid = true;
        }
        else {
            message = _language === 'en-us' ? 'You have selected invalid Filetype. \n Valid Filetype .pdf,.jpg,.jpeg,.png' : 'आपने  अवैध फाइल प्रकार का चयन किया है | \n वैध फाइल प्रकार .pdf,.jpg,.jpeg,.png |';
        }


        if (!isValid) {
            $scope.Passbook = null;
            $('#passbookEntry').val('');
            $(row).val('');
            $(row).parent().find('.filesize').text('');
            ShowModelAlert($scope.translation.CCSComplaintEditForm, message, function () { });
            return;
        }



        if (!checkFileLengthPage(file[0].size)) {
            $scope.Passbook = null;
            $('#passbookEntry').val('');
            ShowModelAlert($scope.translation.CCSComplaintEditForm, _language === 'en-us' ? "File size exceeded.\n Please select files have size less then 200 KB" : "फ़ाइल का आकार से अधिक हो गई है|\nकृपया चुनिंदा फ़ाइलों का आकार 200 KB से कम करें|", function () { });
            $(row).val('');
            $(row).parent().find('.filesize').text('');
            return;
        }

        $scope.Passbook = file[0];
    };
    // END : Add Files For Passbook/Cross Cheque


    //START : Check File Size
    function checkFileLengthPage(fileSized) {

        if (fileSized < 204800) {
            //153600 - 150 KB 512000 -- 200KB 
            return true;
        }
        return false;
    }
    //END: 

    //START : Add Files in Collection
    $scope.UploadEnclouserFile = function (file, row) {
        if (file.length === 0) {
            $(row).val('');
            $(row).parent().find('.filesize').text('');
            ShowModelAlert($scope.translation.CCSComplaintEditForm, _language === 'en-us' ? 'Please select file.' : 'कृपया फ़ाइल चुनें।', function () { });
            return;
        }

        var isValid = false;
        var filename = file[0].name;
        var message = '';
        var extension = filename.split("").reverse().join("");
        extension = extension.substring(0, extension.indexOf('.'));
        extension = extension.split("").reverse().join("").toLowerCase();

        if (extension === 'pdf' || extension === 'jpg' || extension === 'jpeg' || extension === 'png') {
            isValid = true;
        }
        else {
            message = _language === 'en-us' ? 'You have selected invalid Filetype. \n Valid Filetype .pdf,.jpg,.jpeg,.png' : 'आपने  अवैध फाइल प्रकार का चयन किया है | \n वैध फाइल प्रकार .pdf,.jpg,.jpeg,.png |';
        }


        if (!isValid) {
            $(row).val('');
            $(row).parent().find('.filesize').text('');
            ShowModelAlert($scope.translation.CCSComplaintEditForm, message, function () { });
            return;
        }



        if (!checkFileLengthPage(file[0].size)) {
            ShowModelAlert($scope.translation.CCSComplaintEditForm, _language === 'en-us' ? "File size exceeded.\n Please select files have size less then 200 KB" : "फ़ाइल का आकार से अधिक हो गई है|\nकृपया चुनिंदा फ़ाइलों का आकार 200 KB से कम करें|", function () { });
            $(row).val('');
            $(row).parent().find('.filesize').text('');
            return;
        }

        var fileid = $(row).attr("fileid");
        var uidtemp = fileid.split('_')[2];

        angular.forEach($scope.Complaint.FileDetails, function (i, j) {
            if (parseInt(i.UId, 10) === parseInt(uidtemp, 10)) {
                i.File = file[0];
            }
        });

    };
    // END : Add Files in Collection


    //START : Verify AADHAr No.
    $scope.VerifyAADHARNo_Step_1_2 = function () {

        //$scope.disablebtnSendOtp = true;

        if (!$('#ckkAADHARAcceptance').prop('checked')) {
            ShowModelAlert($scope.translation.CCSComplaintEditForm, _language === 'en-us' ? 'Please allow system for your AADHAAR based Authentication/ eKYC.' : 'कृपया अपने आधार आधारित प्रमाणीकरण / eKYC के लिए प्रणाली की अनुमति दें।', function () { });
            return;
        }


        if ($scope.Complaint.AADHARNo === undefined || $scope.Complaint.AADHARNo.length < 12) {
            $scope.disablebtnSendOtp = false;
            ShowModelAlert($scope.translation.CCSComplaintEditForm, _language === 'en-us' ? 'Please enter 12 digit AADHAAR No.' : '12 अंकों की आधार संख्या दर्ज करें', function () { });
            return;
        }



        $scope.divInsert = false;
        $scope.divComplaintList = false;


        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/SocietyMemberRegistration/GenerateOTPForPLDOTPBMemberRegistration", { _AadharNo: $scope.Complaint.AADHARNo }).
            success(function (data, status) {

                if (data.Result) {
                    $scope.Complaint.txn = data.txnid;
                    $scope.Complaint.AADHARNoEnc = data.AadharNo;
                    $scope.divAADHARVerificationStep_1 = false;
                    $scope.divAADHARVerificationStep_2 = true;
                    var msg1 = _language === 'en-us' ? 'THE OTP HAS BEEN SENT TO AADHAR REGISTERED MOBILE NUMBER' : 'ओटीपी आधार पंजीकृत मोबाइल नंबर पर भेजा गया है';
                    ShowModelAlert($scope.translation.CCSComplaintEditForm, msg1, function () { });
                    $(".se-pre-con").fadeOut("slow");
                }
                else {
                    $(".se-pre-con").fadeOut("slow");
                    var msg2 = _language === 'en-us' ? 'INVALID AADHAAR No.' : 'अमान्य आधार नंबर';
                    $scope.divAADHARVerificationStep_1 = true;
                    $scope.divAADHARVerificationStep_2 = false;
                    ShowModelAlert($scope.translation.CCSComplaintEditForm, msg2, function () { });
                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });

    };
    // END : Verify AADHAr No.

    //START : ResendOTP
    $scope.ResendOTP = function () {

        $scope.disablebtnSendOtp = true;

        $scope.divInsert = false;
        $scope.divComplaintList = false;

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/SocietyMemberRegistration/GenerateOTPForPLDOTPBMemberRegistration", { _AadharNo: $scope.Complaint.AADHARNo }).
            success(function (data, status) {

                if (data.Result) {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.Complaint.txn = data.txnid;
                    $scope.Complaint.AADHARNoEnc = data.AadharNo;
                    $scope.divAADHARVerificationStep_1 = false;
                    $scope.divAADHARVerificationStep_2 = true;
                    var msg1 = _language === 'en-us' ? 'THE OTP HAS BEEN SENT TO AADHAR REGISTERED MOBILE NUMBER' : 'ओटीपी आधार पंजीकृत मोबाइल नंबर पर भेजा गया है';
                    ShowModelAlert($scope.translation.CCSComplaintEditForm, msg1, function () { });
                }
                else {
                    $(".se-pre-con").fadeOut("slow");
                    var msg2 = _language === 'en-us' ? 'INVALID AADHAAR No.' : 'अमान्य आधार नंबर';
                    $scope.divAADHARVerificationStep_1 = true;
                    $scope.divAADHARVerificationStep_2 = false;
                    ShowModelAlert($scope.translation.CCSComplaintEditForm, msg2, function () { });
                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });

    };
    // END : ResendOTP

    //START : Verify AADHAr No.
    $scope.VerifyAADHARNo_Step_2_2 = function () {

        $(".se-pre-con").fadeIn("slow");
        if ($scope.Complaint.EnterOTP === undefined || $scope.Complaint.EnterOTP.length !== 6) {
            var msg = _language === 'en-us' ? 'Please enter 6 digit OTP' : '6 अंकों की ओटीपी दर्ज करें';
            $(".se-pre-con").fadeOut("slow");
            ShowModelAlert($scope.translation.CCSComplaintEditForm, msg, function () { });
            return;
        }

        $http.post(SiteUrl + "/Citizen/ValidateOTPAndGetComplainerList", { "otp": $scope.Complaint.EnterOTP, "txn": $scope.Complaint.txn, "_AadharNo": $scope.Complaint.AADHARNoEnc }).
            success(function (data, status) {
                if (data.length === 0) {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.divComplaintList = false;
                    $scope.ComplaintList = [];
                    $scope.divAADHARVerificationStep_1 = false;
                    $scope.divAADHARVerificationStep_2 = true;
                    ShowModelAlert($scope.translation.CCSComplaintEditForm, _language === 'en-us' ? 'No data found' : 'कोई डेटा नहीं मिला', function () { });
                }
                else {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.divAADHARVerificationStep_1 = false;
                    $scope.divAADHARVerificationStep_2 = false;
                    $scope.AADHARNoEnc = $scope.Complaint.AADHARNoEnc;
                    $scope.ComplaintList = data;
                    $scope.divComplaintList = true;
                    $scope.divComplaintList = true;
                }

            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.ComplaintList = [];
            });
    };
    // END : Verify AADHAr No.


    $scope.CheckDuplicateEdit = function (ComplaintIdEnc) {
        return $http.post(SiteUrl + "/Citizen/IsDuplicateEdit", { "ComplaintIdEnc": ComplaintIdEnc }).
            success(function (data, status) {
                return data.Result;

            }).
            error(function (data, status) {
                return false;
            });
    };

    //START : Get Complaint Details
    $scope.GetComplaintDetail = function (ComplaintIdEnc) {

        $scope.CheckDuplicateEdit(ComplaintIdEnc).then(function (rs) {

            if (rs.data.Result === true) {
                $(".se-pre-con").fadeIn("slow");
                $http.post(SiteUrl + "/Citizen/GetComplaintDetailsByComplaintId", { "ComplaintIdEnc": ComplaintIdEnc }).
                    success(function (data, status) {
                        if (data.length === 0) {
                            $scope.divComplaintList = false;
                            $scope.ComplaintList = [];
                            $scope.divAADHARVerificationStep_1 = false;
                            $scope.divAADHARVerificationStep_2 = true;
                            $(".se-pre-con").fadeOut("slow");
                            ShowModelAlert($scope.translation.CCSComplaintEditForm, _language === 'en-us' ? 'No data found' : 'कोई डेटा नहीं मिला', function () { });
                        }
                        else {
                            $scope.divAADHARVerificationStep_1 = false;
                            $scope.divAADHARVerificationStep_2 = false;
                            $scope.divComplaintList = false;
                            $scope.divInsert = true;

                            var maxid = 1;
                            angular.forEach(data.FileDetails, function (i, j) {
                                i.UId = maxid;
                                i.FileTypeId = i.FileTypeId.toString();
                                maxid++;
                            });

                            $scope.MaxId = maxid;
                            $scope.ComplaintIdEnc = ComplaintIdEnc;

                            $scope.Complaint = data;
                            $scope.Complaint.DistrictId = data.DistrictId.toString();
                            $scope.GetCCSList();
                            $scope.Complaint.CCSId = data.CCSId.toString();
                            $(".se-pre-con").fadeOut("slow");
                        }
                    }).
                    error(function (data, status) {
                        $scope.ComplaintList = [];
                        $(".se-pre-con").fadeOut("slow");
                    });
            }
            else {
                ShowModelAlert($scope.translation.CCSComplaintEditForm, _language === 'en-us' ? 'Since the complaint has already been edited. So this complaint is locked for re-edit (complaint will not be re-edited)' : 'चूँकि शिकायत पहले एडिट चुकी है | अतः यह शिकायत  पुनः एडिट के लिए लॉक है (शिकायत पुनः एडिट नहीं होगी) ', function () { });
            }
        });
    };
    // END :  Get Complaint Details

    //START : CCSComplaintDetails
    $scope.ExportData = function (id) {
        $('#hdfcomplaintid').val(id);
        $('#btnGeneratePDF').click();
    };
    // END : CCSComplaintDetails


    //START : RemoveRow
    $scope.RemoveRow = function (index) {
        $scope.Complaint.FileDetails.splice(index, 1);
    };
    //END: RemoveRow

    // START : Page load Function
    $scope.OnPageLoad = function () {
        $scope.GetDocumentTypeList();
        $scope.ClearForm();
        $scope._language = _language;
        if (_language !== 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    alert("Error while translation");
                });
        }

    };
    // END : Page load Function

    // START : Get Document Type List
    $scope.GetDocumentTypeList = function () {
        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CCSCOMPLAINTDOCTYPE").
            success(function (data, status) {
                $scope.DocumentTypes = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.DocumentTypes = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    // START : Get Document Type List


    // START : Get District List
    $scope.DistrictList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Blocks = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=0").
            success(function (data, status) {
                $scope.Districts = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.Districts = [];
                $(".se-pre-con").fadeOut("slow");
            });

    };
    // END : Get District List

    // START : Get CCS List
    $scope.GetCCSList = function () {
        $(".se-pre-con").fadeIn("slow");
        var model = [{ "Key": "SocietyRegOfficeDetails.DISTRICT_ID", "Value": $scope.Complaint.DistrictId }];
        $http.post(SiteUrl + "/Citizen/GetCreditCooperativeSocieties", { paramlist: model }).
            success(function (data, status) {
                $scope.CCSList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while fetching list");
            });
    };
    // END : Get CCS List

    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.Complaint = [];
    };
    //END : Clear Form

    //START : Back To List
    $scope.BackToList = function () {
        $scope.divInsert = false;
        $scope.Complaint = [];
        $scope.divComplaintList = true;
    };
    //END : Back To List

    //START : Back To Initial
    $scope.BackToInitial = function () {
        $scope.divInsert = false;
        $scope.Complaint = [];
        $scope.divComplaintList = false;
        $scope.ComplaintList = [];
        $scope.Complaint.AADHARNo = '';
        $scope.Complaint.EnterOTP = '';
        $('#ckkAADHARAcceptance').prop('checked', false);
        $scope.divAADHARVerificationStep_2 = false;
        $scope.divAADHARVerificationStep_1 = true;
    };
    //END : Back To Initial

    // START : Date Checker
    function isDate(inputData) {
        var regex = /^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$/g;
        return regex.test(inputData) ? true : false;
    }
    // START : Date Checker

    //START : Validate Email
    function ValidateEmail(mail) {

        if (mail === undefined || mail === null || mail.length < 1) {
            return false;
        }

        if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(mail)) {
            return true;
        }
        return false;
    }
    //End : Validate Email

    //START : Validate Form
    $scope.ValidateForm = function () {
        var message = '';

        if ($scope.Complaint === undefined || $scope.Complaint.ComplainerName === undefined || $scope.Complaint.ComplainerName.length < 1) {
            message += $scope._language === 'en-us' ? '\n  Please Enter Complainant Name ' : '\n  कृपया शिकायतकर्ता का नाम दर्ज करें |';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.ComplainerFatherName === undefined || $scope.Complaint.ComplainerFatherName.length < 1) {
            message += $scope._language === 'en-us' ? '\n  Please Enter Father Name' : '\n  कृपया पिता का नाम दर्ज करें  |';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.Address === undefined || $scope.Complaint.Address.length < 1) {
            message += $scope._language === 'en-us' ? '\n  Please Enter Address ' : '\n  कृपया पता दर्ज करें |';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.MobileNo === undefined || $scope.Complaint.MobileNo.length < 10) {
            message += $scope._language === 'en-us' ? '\n  Please Enter Mobile no. in 10 digits' : '\n  कृपया 10 अंकों का मोबाइल नंबर दर्ज करें |';
        }

        if (!($scope.Complaint === undefined || $scope.Complaint.Email === undefined || $scope.Complaint.Email.length < 1)) {
            if (!ValidateEmail($scope.Complaint.Email)) {
                message += $scope._language === 'en-us' ? '\n  Please Enter valid e-mail Id' : '\n  कृपया वैध ईमेल आईडी दर्ज करें |';
            }
        }
        if ($scope.Complaint === undefined || $scope.Complaint.DistrictId === undefined || $scope.Complaint.DistrictId === '0') {
            message += $scope._language === 'en-us' ? '\n  Please Select district' : '\n  कृपया ज़िला चुने|';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.CCSId === undefined || $scope.Complaint.CCSId === '0') {
            message += $scope._language === 'en-us' ? '\n  Please Select Credit Credit Society' : '\n  कृपया क्रेडिट को-ऑपरेटिव सोसायटी नाम चुनें|';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.BranchName === undefined || $scope.Complaint.BranchName.length < 1) {
            message += $scope._language === 'en-us' ? '\n  Please Enter Branch' : '\n  कृपया ब्रांच दर्ज करें  |';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.BPNo === undefined || $scope.Complaint.BPNo.length < 1) {
            message += $scope._language === 'en-us' ? '\n  Please Enter Customer No. (BP No.)' : '\n  कृपया ग्राहक संख्या (बीपी नंबर) दर्ज करें|';
        }

        //if ($scope.Complaint === undefined || $scope.Complaint.AccountNo === undefined || $scope.Complaint.AccountNo.length < 1) {
        //    message += $scope._language === 'en-us' ? '\n  Please Enter Bank Account No.' : '\n  कृपया बैंक खाता नम्बर दर्ज करें |';
        //}

        if ($scope.Complaint === undefined || $scope.Complaint.AmountDeposited === undefined || $scope.Complaint.AmountDeposited.length < 1) {
            message += $scope._language === 'en-us' ? '\n  Please Enter Total Deposited Amount' : '\n  कृपया कुल जमा राशि दर्ज करें |';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.Remarks === undefined || $scope.Complaint.Remarks.length < 1) {
            message += $scope._language === 'en-us' ? '\n  Please Enter Remark' : '\n  कृपया टिप्पणी दर्ज करें  |';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.AccountNoInSociety === undefined || $scope.Complaint.AccountNoInSociety.length < 1) {
            message += $scope._language === 'en-us' ? '\n  Please Enter Bank A/C No. in Society' : '\n  कृपया रेमिटेंस  के बैंक खाता संख्या दर्ज करें|';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.AccountIFSCInSociety === undefined || $scope.Complaint.AccountIFSCInSociety.length < 1) {
            message += $scope._language === 'en-us' ? '\n  Please Enter IFSC Code of Bank A/C in Committee' : '\n  कृपया रेमिटेंस के बैंक खाता आई एफ एस सी कोड दर्ज करें|';
        }

        if ($scope.Complaint === undefined || $scope.Complaint.AccountHolderName === undefined || $scope.Complaint.AccountHolderName.length < 1) {
            message += $scope._language === 'en-us' ? '\n  Please Enter A/C Holder Name' : '\n  कृपया खाता धारक का नाम दर्ज करें|';
        }

        //if ($scope.Passbook === undefined || $scope.Passbook === null || $scope.Passbook.length === 0) {
        //    message += $scope._language === 'en-us' ? '\n  Please Upload Passbook/Cross Cheque Copy' : '\n  कृपया पासबुक / क्रॉस चेक कॉपी अपलोड करें ';
        //}

        var fileNotAvailable = 0;
        var CalculateAmount = 0;

        angular.forEach($scope.Complaint.FileDetails, function (i, j) {

            if (!(i.PayDate === undefined || i.PayDate.length < 1)) {
                i.PayDate = isDate(i.PayDate) ? i.PayDate : '';
            }

            if (!(i.MaturityDate === undefined || i.MaturityDate.length < 1)) {
                i.MaturityDate = isDate(i.MaturityDate) ? i.MaturityDate : '';
            }


            if (i.IsNewRow !== undefined && i.IsNewRow === true && (i.File === undefined || i.File === '')) {
                fileNotAvailable++;
            }


            if (
                i.FileTypeId === undefined || i.FileTypeId === '0' ||
                i.AccountNo === undefined || i.AccountNo.length < 1 ||
                i.PaidAmount === undefined || i.PaidAmount.length < 1 ||
                i.PayDate === undefined || i.PayDate.length < 1 ||
                i.MaturityAmount === undefined || i.MaturityAmount.length < 1 ||
                i.MaturityDate === undefined || i.MaturityDate.length < 1) {
                fileNotAvailable++;
            }
        });


        $scope.Complaint.FileDetails.forEach(function (i, j) {
            if (i.PaidAmount !== undefined && parseInt(i.PaidAmount, 10) > 0) {
                CalculateAmount += parseInt(i.PaidAmount, 10);
            }
        });



        $('#tblBody tr').each(function () {
            var path = $($(this).find('td:eq(7)').find('a')[0]).attr('href');
            if (path !== undefined && path.length < 32 && ($($(this).find('td:eq(8)').find('input')[0]).val() === undefined || $($(this).find('td:eq(8)').find('input')[0]).val() === '')) {
                var jElement = $(this).find('td:eq(8)').find('input'); console.log('1' + path.length);
                jElement.css('border', 'solid 1px red');
            }
            else {
                jElement = $(this).find('td:eq(8)').find('input');
                jElement.css('border', 'solid 1px');
            }
        });


        $('#tblBody input').each(function () {

            if ($(this).attr("filetype") === 'date') {
                $(this).val(isDate($(this).val()) ? $(this).val() : '');
            }


            if ($(this)[0].type !== 'file' && $(this).val().length === 0) {
                $(this).css('border', 'solid 1px red');
                ValidationChaeck = false;
            }
            else if ($(this)[0].type !== 'file') {
                $(this).css('border', 'solid 1px');
            }
        });

        if (fileNotAvailable > 0) {
            message += $scope._language === 'en-us' ? '\n  Please provide all records(red Marked) related to documents ' : '\n  कृपया दस्तावेजों से संबंधित सभी रिकॉर्ड(लाल रंग चिह्नित) प्रदान करें |';
        }

        if (parseInt(CalculateAmount, 10) !== parseInt($scope.Complaint.AmountDeposited, 10)) {
            message += $scope._language === 'en-us' ? '\n  The sum of the deposited amount and the total amount deposited are not equal' : '\n   जमा कराई गई राशि का योग एवं कुल जमा राशि बराबर नहीं है  |';
        }

        if (message.length > 5) {
            ShowModelAlert($scope.translation.CCSComplaintEditForm, message, function () { });
            return false;
        }
        return true;
    };
    //END : Validate Form

    //START : Save/Edit Data
    $scope.Insert = function () {

        $(".se-pre-con").fadeIn("slow");
        $scope.ShowSubmitButton = false;

        if (!$scope.ValidateForm()) {
            $(".se-pre-con").fadeOut("slow");
            $scope.ShowSubmitButton = true;
            return false;
        }


        if (!confirm($scope.translation.SaveLockMessage)) {
            $scope.ShowSubmitButton = true;
            return;
        }

        var email = $scope.Complaint === undefined || $scope.Complaint.Email === undefined || $scope.Complaint.Email.length < 1 ? '' : $scope.Complaint.Email;
        var formData = new FormData();

        formData.append("ComplaintIdEnc", $scope.ComplaintIdEnc);
        formData.append("ComplaintId", $scope.Complaint.ComplaintId);
        formData.append("ComplainerName", $scope.Complaint.ComplainerName);
        formData.append("ComplainerFatherName", $scope.Complaint.ComplainerFatherName);
        formData.append("DistrictId", $scope.Complaint.DistrictId);
        formData.append("Address", $scope.Complaint.Address);
        formData.append("MobileNo", $scope.Complaint.MobileNo);
        formData.append("Email", email);
        formData.append("CCSId", $scope.Complaint.CCSId);
        formData.append("BranchName", $scope.Complaint.BranchName);
        formData.append("AccountNo", $scope.Complaint.AccountNo === undefined || $scope.Complaint.AccountNo.length < 1 ? '' : $scope.Complaint.AccountNo);
        formData.append("IFSCCode", $scope.Complaint === undefined || $scope.Complaint.IFSCCode === undefined || $scope.Complaint.IFSCCode.length < 1 ? '' : $scope.Complaint.IFSCCode);
        formData.append("AmountDeposited", $scope.Complaint.AmountDeposited);
        formData.append("DepositDate", '');
        formData.append("AADHAR", $scope.AADHARNoEnc);
        formData.append("MembershipId", $scope.Complaint.MembershipId === undefined || $scope.Complaint.MembershipId.length < 1 ? '' : $scope.Complaint.MembershipId);
        formData.append("Remarks", $scope.Complaint.Remarks);

        formData.append("PassbookCrossCheque", $scope.Complaint.PassbookCrossChequeView);

        formData.append("AccountNoInSociety", $scope.Complaint.AccountNoInSociety);
        formData.append("AccountIFSCInSociety", $scope.Complaint.AccountIFSCInSociety);
        formData.append("AccountHolderName", $scope.Complaint.AccountHolderName);
        formData.append("RemittancePassbook", $scope.Passbook);
        formData.append("BPNo", $scope.Complaint.BPNo);

        angular.forEach($scope.Complaint.FileDetails, function (i, j) {
            formData.append('File_' + j, i.File);
            formData.append("FileDetails[" + j + "].FilePath", i.FilePathView);
            formData.append("FileDetails[" + j + "].FileTypeId", i.FileTypeId);
            formData.append("FileDetails[" + j + "].AccountNo", i.AccountNo);
            formData.append("FileDetails[" + j + "].PaidAmount", i.PaidAmount);
            formData.append("FileDetails[" + j + "].PayDate", i.PayDate);
            formData.append("FileDetails[" + j + "].MaturityAmount", i.MaturityAmount);
            formData.append("FileDetails[" + j + "].MaturityDate", i.MaturityDate);
        });

        $http.post(SiteUrl + "/Citizen/EditCCSComplaint", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {

                if (data.Result) {
                    $scope.divInsert = false;
                    $scope.Complaint = [];
                    $scope.ComplaintList = [];
                    $scope.divComplaintList = false;
                    $scope.Complaint.AADHARNo = '';
                    $scope.Complaint.EnterOTP = '';
                    $scope.divAADHARVerificationStep_1 = true;
                    $scope.divAADHARVerificationStep_2 = false;
                }
                else {
                    $scope.divInsert = true;
                }

                $scope.IsDisabled = false;
                $scope.ShowSubmitButton = true;
                $scope.DivResponce = true;
                $('#Responce').html(data.Response);
                $(".se-pre-con").fadeOut("slow");
                $("#addModal").modal("show");
            }).
            error(function (data, status) {
                $scope.ShowSubmitButton = true;
                $(".se-pre-con").fadeOut("slow");
                alert("Error");
            });
    };
    //END : Save/Edit Data

    // START : On Page Load
    $scope.OnPageLoad();
    // END : On Page Load
});