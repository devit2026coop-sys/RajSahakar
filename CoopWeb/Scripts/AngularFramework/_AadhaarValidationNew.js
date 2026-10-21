var _callback;
var _callbackFailer;
var _http
var _aadhaarNo = '';
var _transactionId = "";
var _isESign = false;
var _isShowConfirm = false
var _customMmessage = "";

var _Directory = "";
var _RegistrationNo = "";
_language == 'en-us' ? '' : '';
_Directory = "";
_RegistrationNo = "";
var _janaadhaarNo = "", _JanMemberId = "";
var _schemeName = "";

function AadhaarAuth(AADHAARNo, http, AuthType, IsESign, SchemeName, callbackSuccess, isShowConfirm, customMmessage, callbackFailer) {
    //alert(1);
    if (AADHAARNo == undefined) {
        AADHAARNo = "";
    }

    if (AADHAARNo.length != 12 && !IsESign) {
        ShowOkMessage("Warning", _language == 'en-us' ? "Invalid Aadhaar number. Aadhaar must be 12 digit number" : 'अमान्य आधार संख्या। आधार 12 अंकों की संख्या होना चाहिए', function () { });
    } else {
        $("#btnSSOAuth_SendOTP").show();
        $("#btnSSOAuth_ValidateOTP").hide();
        $("#btnSSOAuth_ReSendOTP").hide();
        _isESign = IsESign;
        if (AuthType == "OTP" || _isESign) {
            $("#lblSSOAuth_rbtnListType_1").show();
            $("#lblSSOAuth_rbtnListType_2").hide();
            $("#otpsect").show();
            $("#biosect").hide();
            $("#SSOAuth_rbtnListType_1").prop("checked", true);
            $("#SSOAuth_rbtnListType_2").prop("checked", false);
        } else if (AuthType == "BIO" && !_isESign) {
            $("#lblSSOAuth_rbtnListType_1").hide();
            $("#lblSSOAuth_rbtnListType_2").show();
            $("#SSOAuth_rbtnListType_1").prop("checked", false);
            $("#SSOAuth_rbtnListType_2").prop("checked", true);
            $("#otpsect").hide();
            $("#biosect").show();
        } else {
            $("#lblSSOAuth_rbtnListType_1").show();
            $("#lblSSOAuth_rbtnListType_2").show();
            $("#SSOAuth_rbtnListType_1").prop("checked", true);
            $("#SSOAuth_rbtnListType_2").prop("checked", false);
            $("#otpsect").show();
            $("#biosect").hide();
        }


        _transactionId = "";
        if (!_isESign) {
            $("#aadharMask").html("XXXXXXXX" + AADHAARNo.substring(8, 12));
        } else { $("#aadharMask").hide(); }
        $("#dvAuthAllow").hide();
        $("#SSOAuth_chkConsent").prop("checked", false);
        $("#SSOAuth_OTP").hide();
        $('#SSOAuth_OtpText').val('');
        var encryptedAadhaarNo = "";
        if (AADHAARNo != null && AADHAARNo != "")
            encryptedAadhaarNo = encryptData(AADHAARNo);

        //$("#otpsect").show();
        //$("#biosect").hide();
        $('.se-pre-con').hide();
        $("#dvAadhaarValidation").show();
        _callback = callbackSuccess;
        _http = http;
        _schemeName = SchemeName;

        _aadhaarNo = encryptedAadhaarNo;
        _callbackFailer = callbackFailer == undefined ? function () { } : callbackFailer;
        _isShowConfirm = isShowConfirm == undefined ? false : isShowConfirm;
        _customMmessage = customMmessage == undefined ? "" : customMmessage;
    }
}
function AadhaarAuth_v1(AADHAARNo, http, AuthType, IsESign, RegistrationNo, Directory, callbackSuccess, isShowConfirm, customMmessage, callbackFailer) {
    //alert(1);
    if (AADHAARNo == undefined) {
        AADHAARNo = "";
    }

    if (AADHAARNo.length != 12 && !IsESign) {
        ShowOkMessage("Warning", _language == 'en-us' ? "Invalid Aadhaar number. Aadhaar must be 12 digit number" : 'अमान्य आधार संख्या। आधार 12 अंकों की संख्या होना चाहिए', function () { });
    } else {
        $("#btnSSOAuth_SendOTP").show();
        $("#btnSSOAuth_ValidateOTP").hide();
        $("#btnSSOAuth_ReSendOTP").hide();
        _isESign = IsESign;
        if (AuthType == "OTP" || _isESign) {
            $("#lblSSOAuth_rbtnListType_1").show();
            $("#lblSSOAuth_rbtnListType_2").hide();
            $("#otpsect").show();
            $("#biosect").hide();
            $("#SSOAuth_rbtnListType_1").prop("checked", true);
            $("#SSOAuth_rbtnListType_2").prop("checked", false);
        } else if (AuthType == "BIO" && !_isESign) {
            $("#lblSSOAuth_rbtnListType_1").hide();
            $("#lblSSOAuth_rbtnListType_2").show();
            $("#SSOAuth_rbtnListType_1").prop("checked", false);
            $("#SSOAuth_rbtnListType_2").prop("checked", true);
            $("#otpsect").hide();
            $("#biosect").show();
        } else {
            $("#lblSSOAuth_rbtnListType_1").show();
            $("#lblSSOAuth_rbtnListType_2").show();
            $("#SSOAuth_rbtnListType_1").prop("checked", true);
            $("#SSOAuth_rbtnListType_2").prop("checked", false);
            $("#otpsect").show();
            $("#biosect").hide();
        }


        _transactionId = "";
        if (!_isESign) {
            $("#aadharMask").html("XXXXXXXX" + AADHAARNo.substring(8, 12));
        } else { $("#aadharMask").hide(); }
        $("#dvAuthAllow").hide();
        $("#SSOAuth_chkConsent").prop("checked", false);
        $("#SSOAuth_OTP").hide();
        $('#SSOAuth_OtpText').val('');

        //$("#otpsect").show();
        //$("#biosect").hide();
        $('.se-pre-con').hide();
        $("#dvAadhaarValidation").show();
        _callback = callbackSuccess;
        _http = http;

        var encryptedAadhaarNo = "";
        if (AADHAARNo != null && AADHAARNo != "")
            encryptedAadhaarNo = encryptData(AADHAARNo);
        _aadhaarNo = encryptedAadhaarNo;
        _callbackFailer = callbackFailer == undefined ? function () { } : callbackFailer;
        _isShowConfirm = isShowConfirm == undefined ? false : isShowConfirm;
        _customMmessage = customMmessage == undefined ? "" : customMmessage;
        _Directory = Directory == undefined ? "" : Directory;
        _RegistrationNo = RegistrationNo == undefined ? "" : RegistrationNo;
    }
}

function GenrateOTP_old() {
    if (!$('#SSOAuth_chkConsent').is(":checked")) {
        $("#dvAuthAllow").show();
        $("#AuthAllowMessage").html(_language == 'en-us' ? "Please allow system for your AADHAAR based Authentication/ eKYC." : 'कृपया अपने आधार आधारित प्रमाणीकरण / eKYC के लिए प्रणाली की अनुमति दें।');
    }
    else {
        $('.se-pre-con').show();

        if (!_isESign) {
            _http.get(SiteUrl + "/Citizen/GenrateOTPdata/?_AadharNo=" + encodeURIComponent(_aadhaarNo)).
                success(function (data, status) {
                    $('.se-pre-con').hide();
                    if (data[0].txnid.length > 0) {
                        if (data[0].txnid.split('#')[0] == "NO") {
                            if (_isShowConfirm) {
                                ShowAlertMessage("", _customMmessage, function () {
                                    $('.se-pre-con').hide();
                                    $('#SSOAuth_OtpText').val('');
                                    $('#btnSSOAuth_SendOTP').show();
                                    $('#btnSSOAuth_ValidateOTP').hide();
                                    $('#btnSSOAuth_ReSendOTP').hide();
                                    $("#dvAadhaarValidation").hide();
                                    _callbackFailer();
                                })
                            } else {

                                ShowOkMessage("Warning", data[0].txnid.split('#')[1], function () { });
                            }
                        }
                        else if (data[0].txnid.indexOf("DOITRJGOVAUTH") > -1) {
                            $('#btnSSOAuth_SendOTP').hide();
                            $('#btnSSOAuth_ValidateOTP').show();
                            $('#btnSSOAuth_ReSendOTP').show();
                            $("#SSOAuth_OTP").show();
                            _transactionId = data[0].txnid;//.split('#')[1];    

                            ShowOkMessage("Success", _language == 'en-us' ? 'OTP has been sent to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n', function () { });

                        }
                        else {
                            $('#btnSSOAuth_SendOTP').hide();
                            $('#btnSSOAuth_ValidateOTP').show();
                            $('#btnSSOAuth_ReSendOTP').show();
                        }
                    }
                    else {
                        if (_isShowConfirm) {
                            ShowAlertMessage("", _customMmessage, function () {
                                _callbackFailer();
                            })
                        } else {
                            var msg = _language == 'en-us' ? "Request could not be generated. OTP is not being sent. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कुछ समस्या के कारण ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।";
                            var msg1 = data[0].txnid.split('#')[1];
                            ShowOkMessage("", msg1, function () { });
                        }
                    }
                }).
                error(function (data, status) {
                    $('.se-pre-con').hide();
                    ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. OTP is not being sent. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कुछ समस्या के कारण ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                });
        } else {
            _http.get(SiteUrl + "/Citizen/GenerateAadhaarOTPForEsing").
                success(function (data, status) {
                    $('.se-pre-con').hide();
                    if (data.indexOf('NO#') >= 0) {
                        var msg = _language == 'en-us' ? "Request could not be generated. OTP is not being sent for the sign due to some problem. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।";
                        var msg1 = data.split('#')[1];
                        ShowOkMessage("", msg1, function () { });
                    } else {
                        if (data != "LOGOUT") {
                            $('#btnSSOAuth_SendOTP').hide();
                            $('#btnSSOAuth_ValidateOTP').show();
                            $('#btnSSOAuth_ReSendOTP').show();
                            $("#SSOAuth_OTP").show();
                            _transactionId = data;//.split('#')[1];  
                            ShowOkMessage("", _language == 'en-us' ? 'OTP has been sent to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n', function () {
                            });
                        }
                        else if (data == "LOGOUT") {
                            alert('Session out.\n');
                            window.location = 'https://sso.rajasthan.gov.in/signin';
                        }
                        $(".se-pre-con").fadeOut("slow");
                    }
                }).
                error(function (data, status) {
                    $('.se-pre-con').hide();
                    ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. OTP is not being sent. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कुछ समस्या के कारण ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                });
        }
    }
}
var BhamashahMemberDetail = [];
function GenrateOTP() {
    if (!$('#SSOAuth_chkConsent').is(":checked")) {
        $("#dvAuthAllow").show();
        $("#AuthAllowMessage").html(_language == 'en-us' ? "Please allow system for your AADHAAR based Authentication/ eKYC." : 'कृपया अपने आधार आधारित प्रमाणीकरण / eKYC के लिए प्रणाली की अनुमति दें।');
    }
    else {
        $('.se-pre-con').show();

        if (!_isESign) {
            _http.post(SiteUrl + "/citizen/GetJanAadhaarDetails", { JanaadhaarNo: encodeURIComponent(_aadhaarNo) }).success(function (data) {
                if (Object.keys(data).length == 0) {
                    var message = _language == 'en-us' ? 'Invalid Aadhaar Number \n' : 'अमान्य आधार संख्या \n';
                    $('.se-pre-con').hide();
                    CloseOtpPopUp();
                    ShowOkMessage("Error", _language == 'en-us' ? 'Please link Aadhaar to Jan Aadhaar.\n' : 'कृपया आधार को जनाधार से लिंक करें|\n', function () { });
                }
                else {
                    if (data[0].msg === "Unexpected JSON token when reading DataTable. Expected StartArray, got StartObject. Path '', line 1, position 1.") {
                        var message = _language == 'en-us' ? 'Invalid Aadhaar Number \n' : 'अमान्य आधार संख्या \n';
                        alert(message);
                        $('.se-pre-con').hide();
                        CloseOtpPopUp();
                        ShowOkMessage("Error", _language == 'en-us' ? 'Invalid Aadhaar Number.\n' : 'अमान्य आधार संख्या|\n', function () { });

                    }
                    else {
                        BhamashahMemberDetail = data;
                        var IsJanAadharDataFetched = true;
                        if (IsJanAadharDataFetched == true) {
                            $("#btnVerifyOTP").removeAttr("disabled");
                            _JanMemberId = BhamashahMemberDetail[0].MEMBER_ID;
                            //_janaadhaarNo = BhamashahMemberDetail[0].JAN_MEMBER_ID;
                            //_janaadhaarNo = BhamashahMemberDetail[0].JAN_MEMBER_ID;
                            //var mid = BhamashahMemberDetail[0].JANAADHAARMEMID;
                            //var aadhar = encodeURIComponent(_aadhaarNo);

                            //var ENR_id = BhamashahMemberDetail[0].ENR_ID;//$('#txtAadhaarno').val()
                            //var JanAadhar = encodeURIComponent(encryptData(BhamashahMemberDetail[0].JAN_AADHAR));
                            // var newString = SiteUrl.replace("../", "/");
                            /*if (aadhar.trim() != '') {*/
                            //_http.post(SiteUrl + "/citizen/SendRecheckOTP", { "AadharNo": encodeURIComponent(_aadhaarNo), "JanAadharId": encodeURIComponent(encryptData(BhamashahMemberDetail[0].JAN_AADHAR)), "MemberId": mid, "ENR_Id": ENR_id }).
                            //    success(function (data1, status) {
                            _http.post(SiteUrl + "/CommonFunctionsJanAadhaar/GenerateOTP", {
                                "memberId": encodeURIComponent(BhamashahMemberDetail[0].MEMBER_ID), "SchemeName": _schemeName
                            })
                                .then(function (data) {

                                    if (data.status) {

                                        if (data != "Fail") {

                                            $('#btnSSOAuth_SendOTP').hide();
                                            $('#btnSSOAuth_ValidateOTP').show();
                                            $('#btnSSOAuth_ReSendOTP').show();
                                            $("#SSOAuth_OTP").show();
                                            _transactionId = data.data.tid;
                                            ShowOkMessage(
                                                "Success",
                                                _language == 'en-us'
                                                    ? 'OTP has been sent to aaddhaar mobile number.\n'
                                                    : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n',
                                                function () { }
                                            );

                                            $('.se-pre-con').hide();
                                        }
                                        else {
                                            $('#btnSSOAuth_SendOTP').hide();
                                            $('#btnSSOAuth_ValidateOTP').show();
                                            $('#btnSSOAuth_ReSendOTP').show();
                                            $('.se-pre-con').hide();

                                            ShowOkMessage(
                                                "Error",
                                                _language == 'en-us'
                                                    ? 'Some error occurred during sending OTP.\n'
                                                    : 'ओटीपी भेजते समय कुछ त्रुटि है\n',
                                                function () { }
                                            );
                                        }
                                    }

                                })
                                .catch(function (error) {
                                    console.error(error);
                                    $('.se-pre-con').hide();
                                });
                        }
                    }
                }
            }).
                error(function (data1, status) {
                    $('.se-pre-con').hide();
                    ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. OTP is not being sent. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कुछ समस्या के कारण ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                })
        }


        else {
            _http.get(SiteUrl + "/Citizen/GenerateAadhaarOTPForEsing").
                success(function (data, status) {
                    $('.se-pre-con').hide();
                    if (data.indexOf('NO#') >= 0) {
                        var msg = _language == 'en-us' ? "Request could not be generated. OTP is not being sent for the sign due to some problem. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।";
                        var msg1 = data.split('#')[1];
                        ShowOkMessage("", msg1, function () { });
                    } else {
                        if (data != "LOGOUT") {
                            $('#btnSSOAuth_SendOTP').hide();
                            $('#btnSSOAuth_ValidateOTP').show();
                            $('#btnSSOAuth_ReSendOTP').show();
                            $("#SSOAuth_OTP").show();
                            ;//.split('#')[1];  
                            ShowOkMessage("", _language == 'en-us' ? 'OTP has been sent to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n', function () {
                            });
                        }
                        else if (data == "LOGOUT") {
                            alert('Session out.\n');
                            window.location = 'https://sso.rajasthan.gov.in/signin';
                        }
                        $(".se-pre-con").fadeOut("slow");
                    }
                }).
                error(function (data, status) {
                    $('.se-pre-con').hide();
                    ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. OTP is not being sent. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कुछ समस्या के कारण ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                });
        }
    }
}

function GenrateOTPNew() {
    if (!$('#SSOAuth_chkConsent').is(":checked")) {
        $("#dvAuthAllow").show();
        $("#AuthAllowMessage").html(_language == 'en-us' ? "Please allow system for your JanAadhaar based Authentication/ eKYC." : 'कृपया अपने जन आधार आधारित प्रमाणीकरण / eKYC के लिए प्रणाली की अनुमति दें।');
    }
    else {
        $('.se-pre-con').show();
        if (!_isESign) {
            $("#btnVerifyOTP").removeAttr("disabled");

            _http.post(SiteUrl + "/CommonFunctionsJanAadhaar/GenerateOTP", { "memberId": encodeURIComponent(_JanMemberId ? _JanMemberId : _JanMemberId), "SchemeName": _schemeName }).
                success(function (data, status) {
                    if (data.status) {
                        $('#btnSSOAuth_SendOTP').hide();
                        $('#btnSSOAuth_ValidateOTP').show();
                        $('#btnSSOAuth_ReSendOTP').show();
                        $("#SSOAuth_OTP").show();
                        _transactionId = data.tid;//.split('#')[1];    
                        ShowOkMessage("Success", data.message, function () { });
                        $('.se-pre-con').hide();
                    }
                    else {

                        $('#btnSSOAuth_SendOTP').hide();
                        $('#btnSSOAuth_ValidateOTP').show();
                        $('#btnSSOAuth_ReSendOTP').show();
                        $('.se-pre-con').hide();
                        ShowOkMessage("Error", _language == 'en-us' ? 'Some error occured during sending OTP.\n' : 'ओटीपी भेजते समय कुछ त्रुटि है\n', function () { });

                    }
                    /*
                    var message = '';
                    if (data != "Fail") {

                        $('#btnSSOAuth_SendOTP').hide();
                        $('#btnSSOAuth_ValidateOTP').show();
                        $('#btnSSOAuth_ReSendOTP').show();
                        $("#SSOAuth_OTP").show();
                        _transactionId = data.tid;//.split('#')[1];    
                        ShowOkMessage("Success", _language == 'en-us' ? 'OTP has been sent to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n', function () { });
                        $('.se-pre-con').hide();
                    }
                    else {

                        $('#btnSSOAuth_SendOTP').hide();
                        $('#btnSSOAuth_ValidateOTP').show();
                        $('#btnSSOAuth_ReSendOTP').show();
                        $('.se-pre-con').hide();
                        ShowOkMessage("Error", _language == 'en-us' ? 'Some error occured during sending OTP.\n' : 'ओटीपी भेजते समय कुछ त्रुटि है\n', function () { });

                    }
                    */
                }).error(function (data1, status) {
                    $('.se-pre-con').hide();
                    ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. OTP is not being sent. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कुछ समस्या के कारण ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                })


        }
        else {
            _http.get(SiteUrl + "/Citizen/GenerateAadhaarOTPForEsing").
                success(function (data, status) {
                    $('.se-pre-con').hide();
                    if (data.indexOf('NO#') >= 0) {
                        var msg = _language == 'en-us' ? "Request could not be generated. OTP is not being sent for the sign due to some problem. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कुछ समस्या के कारण साइन के लिए ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।";
                        var msg1 = data.split('#')[1];
                        ShowOkMessage("", msg1, function () { });
                    } else {
                        if (data != "LOGOUT") {
                            $('#btnSSOAuth_SendOTP').hide();
                            $('#btnSSOAuth_ValidateOTP').show();
                            $('#btnSSOAuth_ReSendOTP').show();
                            $("#SSOAuth_OTP").show();
                            _transactionId = data;//.split('#')[1];  
                            ShowOkMessage("", _language == 'en-us' ? 'OTP has been sent to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n', function () {
                            });
                        }
                        else if (data == "LOGOUT") {
                            alert('Session out.\n');
                            window.location = 'https://sso.rajasthan.gov.in/signin';
                        }
                        $(".se-pre-con").fadeOut("slow");
                    }
                }).
                error(function (data, status) {
                    $('.se-pre-con').hide();
                    ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. OTP is not being sent. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कुछ समस्या के कारण ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                });
        }
    }
}
function VarifyOTP_old() {
    $('.se-pre-con').show();
    if (!_isESign) {
        _http.get(SiteUrl + "/Citizen/ValidateOTP/?_AadharNo=" + encodeURIComponent(_aadhaarNo) + "&otp=" + $('#SSOAuth_OtpText').val() + "&txn=" + _transactionId + '&SocietyRegistrationNo=' + _RegistrationNo + '&_SocietyDirectory=' + _Directory).
            success(function (data, status) {
                $('.se-pre-con').hide();
                if (data[0].Column1.length > 0) {
                    if (data[0].Column1 == "Failed") {
                        ShowOkMessage("Error", data[0].Column2, function () { });
                    }
                    else if (data[0].Column1 == "Success") {
                        $('.se-pre-con').hide();
                        $('#SSOAuth_OtpText').val('');
                        $('#btnSSOAuth_SendOTP').show();
                        $('#btnSSOAuth_ValidateOTP').hide();
                        $('#btnSSOAuth_ReSendOTP').hide();
                        $("#dvAadhaarValidation").hide();

                        var dob = data[0].Column4.replace(/-/g, '/');

                        var aadhaarData = {
                            AadharNo: data[0]._aadhaarNo,
                            Name: data[0].Column2,
                            Photo: data[0].Column5,
                            FName: data[0].Column8,
                            DOB: data[0].Column4,
                            Age: data[0].Column4,
                            Gender: data[0].Column3,
                            AADHARNoEnc: data[0].Column7,
                            Phone: "",
                            Email: "",
                            Address: data[0].Column10,
                            Address2: data[0].Column11,
                            House: "",
                            Street: "",
                            lm: "",
                            loc: "",
                            vtc: "",
                            Dist: "",
                            State: "",
                            pc: "",
                            Age: getAge(dob.split('/')[2] + "/" + dob.split('/')[1] + "/" + dob.split('/')[0])
                        }
                        ShowOkMessage("", _language == 'en-us' ? "Aadhaar Number Successfully Varified" : "आधार नंबर सफलतापूर्वक सत्यापित", function () { _callback(aadhaarData); });
                    }
                }
            }).
            error(function (data, status) {
                $('.se-pre-con').hide();
                ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कृपया कुछ देर बाद प्रयास करें।", function () { });

            });
    } else {
        _http.get(SiteUrl + "/Citizen/VerifyAadhaarEsignOTP/?OTP=" + $('#SSOAuth_OtpText').val() + "&TXN=" + _transactionId).
            success(function (data, status) {
                $('.se-pre-con').hide();
                if (data.status != "1") {
                    ShowOkMessage("", data.data, function () { });

                } else {


                    $('.se-pre-con').hide();
                    $('#SSOAuth_OtpText').val('');
                    $('#btnSSOAuth_SendOTP').show();
                    $('#btnSSOAuth_ValidateOTP').hide();
                    $('#btnSSOAuth_ReSendOTP').hide();
                    $("#dvAadhaarValidation").hide();

                    //var dob = data[0].Column4.replace(/-/g, '/');

                    var aadhaarData = {
                        TransId: data.data
                    }
                    _callback(aadhaarData);
                    // ShowOkMessage("", _language == 'en-us' ? "Aadhaar Number Successfully Varified" : "आधार नंबर सफलतापूर्वक सत्यापित", function () { _callback(aadhaarData); });
                }

            }).
            error(function (data, status) {
                $('.se-pre-con').hide();
                ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कृपया कुछ देर बाद प्रयास करें।", function () { });

            });
    }
}

function VarifyOTP() {
    $('.se-pre-con').show();
    if (!_isESign) {
        //   _http.get(SiteUrl + "/Citizen/ValidateOTP/?_AadharNo=" + _aadhaarNo + "&otp=" + $('#SSOAuth_OtpText').val() + "&txn=" + _transactionId + '&SocietyRegistrationNo=' + _RegistrationNo + '&_SocietyDirectory=' + _Directory).
        //_http.post(SiteUrl + "/citizen/VerifyRecheckOTP", { "OTP": $('#SSOAuth_OtpText').val(), "SocietyDirectory": _Directory }).
        //    success(function (data, status) {
        _http.post(SiteUrl + "/CommonFunctionsJanAadhaar/ValidateOTP_FetchRequestedData", {
            "OTP": $('#SSOAuth_OtpText').val(),
            "tid": _transactionId,
            "SocietyDirectory": _Directory,
            "SchemeName": _schemeName,
            "memberId": encodeURIComponent(_JanMemberId ? _JanMemberId : _JanMemberId),
            "janAadharNo": encodeURIComponent(_janaadhaarNo)
        }).
            success(function (data, status) {
                $('.se-pre-con').hide();
                if (data.status == true && data.data.length > 0) {
                    $('.se-pre-con').hide();
                    $('#SSOAuth_OtpText').val('');
                    $('#btnSSOAuth_SendOTP').show();
                    $('#btnSSOAuth_ValidateOTP').hide();
                    $('#btnSSOAuth_ReSendOTP').hide();
                    $("#dvAadhaarValidation").hide();

                    var dob = data.data[0].DOB.replace(/-/g, '/');

                    var aadhaarData = {
                        AadharNo: data.data[0].AADHAR_REF_ID,
                        Name: data.data[0].NAME_EN,
                        Photo: data.data[0].PHOTO,
                        FName: data.data[0].FATHER_NAME_EN,
                        DOB: data.data[0].DOB,
                        Age: data.data[0].AGE,
                        Gender: data.data[0].GENDER,
                        AADHARNoEnc: data.data[0].AADHAR_REF_ID, //if aadhar
                        Phone: "",
                        Email: "",
                        Address: data.data[0].ADDRESS,
                        //Address2: data.data[0].Column11,
                        House: "",
                        Street: "",
                        lm: "",
                        loc: "",
                        vtc: "",
                        Dist: "",
                        State: "",
                        pc: "",
                        Age: getAge(dob.split('/')[2] + "/" + dob.split('/')[1] + "/" + dob.split('/')[0])
                    }
                    ShowOkMessage("", _language == 'en-us' ? "Aadhaar Number Successfully Varified" : "आधार नंबर सफलतापूर्वक सत्यापित", function () { _callback(aadhaarData); });
                } else {
                    ShowOkMessage("Error", data.message, function () { });

                }
                //if (data.data[0].length > 0) {
                //    if (data[0].Column1 == "Failed") {
                //        ShowOkMessage("Error", data[0].Column2, function () { });
                //    }
                //    else if (data.[0].Column1 == "Success") {
                //        $('.se-pre-con').hide();
                //        $('#SSOAuth_OtpText').val('');
                //        $('#btnSSOAuth_SendOTP').show();
                //        $('#btnSSOAuth_ValidateOTP').hide();
                //        $('#btnSSOAuth_ReSendOTP').hide();
                //        $("#dvAadhaarValidation").hide();

                //        var dob = data[0].Column4.replace(/-/g, '/');

                //        var aadhaarData = {
                //            AadharNo: data[0]._aadhaarNo,
                //            Name: data[0].Column2,
                //            Photo: data[0].Column5,
                //            FName: data[0].Column8,
                //            DOB: data[0].Column4,
                //            Age: data[0].Column4,
                //            Gender: data[0].Column3,
                //            AADHARNoEnc: data[0].Column7,
                //            Phone: "",
                //            Email: "",
                //            Address: data[0].Column10,
                //            Address2: data[0].Column11,
                //            House: "",
                //            Street: "",
                //            lm: "",
                //            loc: "",
                //            vtc: "",
                //            Dist: "",
                //            State: "",
                //            pc: "",
                //            Age: getAge(dob.split('/')[2] + "/" + dob.split('/')[1] + "/" + dob.split('/')[0])
                //        }
                //        ShowOkMessage("", _language == 'en-us' ? "Aadhaar Number Successfully Varified" : "आधार नंबर सफलतापूर्वक सत्यापित", function () { _callback(aadhaarData); });
                //    }
                //    else {
                //        ShowOkMessage("Error", data[0].Column2, function () { });

                //    }
                //}
            }).
            error(function (data, status) {
                $('.se-pre-con').hide();
                ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कृपया कुछ देर बाद प्रयास करें।", function () { });

            });
    } else {
        _http.get(SiteUrl + "/Citizen/VerifyAadhaarEsignOTP/?OTP=" + $('#SSOAuth_OtpText').val() + "&TXN=" + _transactionId).
            success(function (data, status) {
                $('.se-pre-con').hide();
                if (data.status != "1") {
                    ShowOkMessage("", data.data, function () { });

                } else {


                    $('.se-pre-con').hide();
                    $('#SSOAuth_OtpText').val('');
                    $('#btnSSOAuth_SendOTP').show();
                    $('#btnSSOAuth_ValidateOTP').hide();
                    $('#btnSSOAuth_ReSendOTP').hide();
                    $("#dvAadhaarValidation").hide();

                    //var dob = data[0].Column4.replace(/-/g, '/');

                    var aadhaarData = {
                        TransId: data.data
                    }
                    _callback(aadhaarData);
                    // ShowOkMessage("", _language == 'en-us' ? "Aadhaar Number Successfully Varified" : "आधार नंबर सफलतापूर्वक सत्यापित", function () { _callback(aadhaarData); });
                }

            }).
            error(function (data, status) {
                $('.se-pre-con').hide();
                ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कृपया कुछ देर बाद प्रयास करें।", function () { });

            });
    }
}

function CloseOtpPopUp() {
    $("#dvAadhaarValidation").hide();
    _callback = null;
}

function OtpType(type) {
    if (type == "otp") {
        $("#otpsect").show();
        $("#biosect").hide();
    } else {
        $("#otpsect").hide();
        $("#biosect").show();
    }

}

//function VerifyOTPByBIO() {
//    if (!$('#SSOAuth_chkConsent').is(":checked")) {
//        $("#dvAuthAllow").show();
//        $("#AuthAllowMessage").html(_language == 'en-us' ? "Please allow system for your AADHAAR based Authentication/ eKYC." : 'कृपया अपने आधार आधारित प्रमाणीकरण / eKYC के लिए प्रणाली की अनुमति दें।');
//    }
//    else {
//        //CaptureFingerPrint(); //Commented by Pradeep 13-06-2024
//    }
//}

//Commented by Pradeep 13-06-2024 Start

//function CaptureFingerPrint() {
//    var error_msg = "";
//    if (_language == 'en-us') {
//        error_msg = "Use following possible steps to resolve the issue:<br>1. Register your device from vendor or using Register Your Device link.<br>2. Download and install the latest registered device drivers and services.<br>3. Start/ restart the registered device service. <br/>4. Remove & plug in device again. <br/>5. Allow device Access from Your System Setting 'Internet Options -> Security -> Custom level -> Enable All ActivX Controls'";
//    } else {
//        error_msg = "समस्या को हल करने के लिए संभव चरणों का उपयोग करें: <br>1. अपने डिवाइस को वेंडर से पंजीकृत करें या रजिस्टर योर डिवाइस लिंक का उपयोग करें। <br>2. नवीनतम पंजीकृत डिवाइस ड्राइवर तथा सर्विसेस  डाउनलोड और स्थापित करें। <br>3. पंजीकृत डिवाइस सेवा शुरू / पुनः आरंभ करें। <br/>4. डिवाइस निकालें और  फिर से प्लगइन  करें।  <br/>5. अपने सिस्टम सेटिंग से इंटरनेट एक्सेस की अनुमति दें 'Internet Options -> Security -> Custom level -> Enable All ActivX Controls'";
//    }
//    $(".se-pre-con_bio").fadeIn("slow");

//    var ssoauth_ver = $.now();
//    var data = { p: 'https', type: 'EKYC', device: 'bio', isHttpsService: false };
//    $.getScript("../Scripts/ssoauth_capture.min.js?v=" + ssoauth_ver).done(function (script, textStatus) {
//        if (textStatus == "success") {
//            startCaptureRD({ authType: data.type, fpDevice: data.device, env: "P", isHttpsService: data.isHttpsService },
//                function (responce) {
//                    $(".se-pre-con_bio").fadeOut("slow");
//                    if (responce.status == 'success') {
//                        VerifyBioData(responce.data);
//                    } else {
//                        ShowOkMessage("Error", "" + error_msg, function () { });
//                    }
//                });
//        }
//    });
//}


//function VerifyBioData(XMLData) {
//    var data = { AadharNo: _aadhaarNo, xml: XMLData };
//    $(".se-pre-con_bio_submit").fadeIn("slow");
//    _http.post(SiteUrl + "/Citizen/ValidateBIOKYC", JSON.stringify({ Details: data })).
//        success(function (rdata, status) {
//            $(".se-pre-con_bio_submit").fadeOut("slow");
//            if (rdata.Status == "300") {
//                if (_isShowConfirm) {
//                    ShowAlertMessage("", _customMmessage, function () {
//                        try {
//                            $('.se-pre-con').hide();
//                            $('#SSOAuth_OtpText').val('');
//                            $('#btnSSOAuth_SendOTP').show();
//                            $('#btnSSOAuth_ValidateOTP').hide();
//                            $('#btnSSOAuth_ReSendOTP').hide();
//                            $("#dvAadhaarValidation").hide();
//                            _callbackFailer();
//                        } catch (ex) { }
//                    })
//                } else {   ShowOkMessage("Warning", _language == 'en-us' ? "Biometric data did not match for the given Aadhaar number." : "दिए गए आधार नंबर के लिए बायोमेट्रिक डेटा मैच नहीं हुआ", function () { });
//            }
//            } else if (rdata.Status == "K-100") {
//                if (_isShowConfirm) {
//                    ShowAlertMessage("", _customMmessage, function () {
//                        try {
//                            $('.se-pre-con').hide();
//                            $('#SSOAuth_OtpText').val('');
//                            $('#btnSSOAuth_SendOTP').show();
//                            $('#btnSSOAuth_ValidateOTP').hide();
//                            $('#btnSSOAuth_ReSendOTP').hide();
//                            $("#dvAadhaarValidation").hide();
//                            _callbackFailer();
//                        } catch (ex) { }
//                    })
//                } else {   ShowOkMessage("Warning", _language == 'en-us' ? "Invalid Aadhar No." : "अमान्य आधार संख्या", function () { });
//            }
//            } else if (rdata.Status == "811") {
//                if (_isShowConfirm) {
//                    ShowAlertMessage("", _customMmessage, function () {
//                        try {
//                            $('.se-pre-con').hide();
//                            $('#SSOAuth_OtpText').val('');
//                            $('#btnSSOAuth_SendOTP').show();
//                            $('#btnSSOAuth_ValidateOTP').hide();
//                            $('#btnSSOAuth_ReSendOTP').hide();
//                            $("#dvAadhaarValidation").hide();
//                            _callbackFailer();
//                        } catch (ex) { }
//                    })
//                } else {   ShowOkMessage("Warning", _language == 'en-us' ? "Missing biometric data in CIDR for the given Aadhaar number." : "दिए गए आधार संख्या के लिए CIDR में बॉयोमीट्रिक डेटा गुम", function () { });
//            }

//            } else if (rdata.Status == "336" || rdata.Status == "337") {
//                if (_isShowConfirm) {
//                    ShowAlertMessage("", _customMmessage, function () {
//                        try {
//                            $('.se-pre-con').hide();
//                            $('#SSOAuth_OtpText').val('');
//                            $('#btnSSOAuth_SendOTP').show();
//                            $('#btnSSOAuth_ValidateOTP').hide();
//                            $('#btnSSOAuth_ReSendOTP').hide();
//                            $("#dvAadhaarValidation").hide();
//                            _callbackFailer();
//                        } catch (ex) { }
//                    })
//                } else {
//                ShowOkMessage("Warning", _language == 'en-us' ? "UIDAI server not responding, Try again after some time." : "UIDAI सर्वर प्रतिक्रिया नहीं दे रहा है, कुछ समय बाद पुन: प्रयास करें।", function () { });
//            }
//            } else if (rdata.Status == "1") {
//                $('.se-pre-con').hide();
//                $('#SSOAuth_OtpText').val('');
//                $('#btnSSOAuth_SendOTP').show();
//                $('#btnSSOAuth_ValidateOTP').hide();
//                $('#btnSSOAuth_ReSendOTP').hide();
//                $("#dvAadhaarValidation").hide();
//                // ShowOkMessage("", _language == 'en-us' ? "Aadhaar Number Successfully Varified" : "आधार नंबर सफलतापूर्वक सत्यापित", function () { _callback(rdata.UserDetails); });
//                ShowOkMessage("", _language == 'en-us' ? "Aadhaar Number Successfully Varified" : "आधार नंबर सफलतापूर्वक सत्यापित", function () { try { _callback(rdata.UserDetails); } catch (ex) { }});
//            } else {

//                if (_isShowConfirm) {
//                    ShowAlertMessage("", _customMmessage, function () {
//                        try {
//                            $('.se-pre-con').hide();
//                            $('#SSOAuth_OtpText').val('');
//                            $('#btnSSOAuth_SendOTP').show();
//                            $('#btnSSOAuth_ValidateOTP').hide();
//                            $('#btnSSOAuth_ReSendOTP').hide();
//                            $("#dvAadhaarValidation").hide();
//                            _callbackFailer();
//                        } catch (ex) { }
//                    })
//                } else {
//                    ShowOkMessage("Warning", _language == 'en-us' ? "UIDAI server not responding, Try again after some time." : "UIDAI सर्वर प्रतिक्रिया नहीं दे रहा है, कुछ समय बाद पुन: प्रयास करें।", function () { });
//                }
//                    //ShowOkMessage("", _language == 'en-us' ? "Invalid Data. Please try again" : "Invalid Data. Please try again", function () { });
//              //  $scope.showDetails = false;
//            }
//        }).
//        error(function (data, status) {
//            $(".se-pre-con_bio_submit").fadeOut("slow");
//            ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कृपया कुछ देर बाद प्रयास करें।", function () { });
//        });

//};

//Commented by Pradeep 13-06-2024 End 

function getAge(DOB) {
    var today = new Date();
    var birthDate = new Date(DOB);
    var age = today.getFullYear() - birthDate.getFullYear();
    var m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
        age = age - 1;
    }
    return age;
}

