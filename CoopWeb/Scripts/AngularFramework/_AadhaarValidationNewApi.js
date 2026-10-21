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





function AadhaarAuth(AADHAARNo, http, AuthType, IsESign, callbackSuccess, isShowConfirm, customMmessage, callbackFailer) {
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
        _aadhaarNo = AADHAARNo;
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
        _aadhaarNo = AADHAARNo;
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
            _http.get(SiteUrl + "/Citizen/GenrateOTPdata/?_AadharNo=" + _aadhaarNo).
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
            $.ajax({
                url: "../citizen/GetJanAadhaarDetails",
                method: "POST",
                data: {
                    JanaadhaarNo: _aadhaarNo
                },
                dataType: "json",
                async: false,
                success: function (data) {
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
                                var mid = BhamashahMemberDetail[0].JANAADHAARMEMID;
                                var aadhar = _aadhaarNo;

                                var ENR_id = BhamashahMemberDetail[0].ENR_ID;//$('#txtAadhaarno').val()
                                var JanAadhar = BhamashahMemberDetail[0].JAN_AADHAR;;
                                // var newString = SiteUrl.replace("../", "/");
                                /*if (aadhar.trim() != '') {*/
                                _http.post(SiteUrl + "/citizen/SendRecheckOTP", { "AadharNo": aadhar, "JanAadharId": JanAadhar, "MemberId": mid, "ENR_Id": ENR_id }).
                                    success(function (data1, status) {

                                        var message = '';
                                        if (data1 != "Fail") {

                                            $('#btnSSOAuth_SendOTP').hide();
                                            $('#btnSSOAuth_ValidateOTP').show();
                                            $('#btnSSOAuth_ReSendOTP').show();
                                            $("#SSOAuth_OTP").show();
                                            //_transactionId = data[0].txnid;//.split('#')[1];    
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
                                    });
                            }
                        }
                    }
                },
                //error: function (jqXHR, textStatus, errorThrown) {

                //    $scope.CloseOtpPopUp();
                //    //window.location = "~/Error/General";
                //}
                error: function (data1, status) {
                    $('.se-pre-con').hide();
                    ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. OTP is not being sent. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कुछ समस्या के कारण ओटीपी नहीं भेजा जा रहा  है। कृपया कुछ देर बाद प्रयास करें।", function () { });
                }

            });
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
        _http.get(SiteUrl + "/Citizen/ValidateOTP/?_AadharNo=" + _aadhaarNo + "&otp=" + $('#SSOAuth_OtpText').val() + "&txn=" + _transactionId + '&SocietyRegistrationNo=' + _RegistrationNo + '&_SocietyDirectory=' + _Directory).
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
        _http.post(SiteUrl + "/citizen/VerifyRecheckOTP", { "OTP": $('#SSOAuth_OtpText').val(), "SocietyDirectory": _Directory }).
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
                    else {
                        ShowOkMessage("Error", data[0].Column2, function () { });

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


function AadharAuthEsign(http, AuthType, IsESign, RegistrationNo, Directory, callbackSuccess, isShowConfirm, customMmessage, callbackFailer) {

    _callback = callbackSuccess;
    _http = http;
    _isESign = IsESign;
    _callbackFailer = callbackFailer == undefined ? function () { } : callbackFailer;
    _isShowConfirm = isShowConfirm == undefined ? false : isShowConfirm;
    _customMmessage = customMmessage == undefined ? "" : customMmessage;

    if (_isESign ) {
        GenerateEsignOTP(_callback);
    }
}

function GenerateEsignOTP(_callback) {
    _http.get("../ApplicationFlow/GenerateAadhaarOTPForEsing_Test/?callback=" + _callback)
        .then(function (response) {
            console.log(response.data);
            var responseHtml = response.data
            if (response.data != '') {
                // Create a new div and insert the response HTML into it
                var tempDiv = document.createElement('div');
                tempDiv.innerHTML = responseHtml;
                // Append the generated form to the body
                document.body.appendChild(tempDiv);
                var form = tempDiv.querySelector('form');
                form.onsubmit = function (event) {
                    event.preventDefault(); // Prevent the default form submission

                    // Create FormData to handle the form inputs
                    var formData = new FormData(form);
                    // Send the form data via AJAX (using AngularJS $http.post)
                    $http({
                        method: 'POST',
                        url: form.action, // Get the form's action URL
                        data: formData,
                        headers: { 'Content-Type': undefined },
                        transformRequest: angular.identity
                    })
                        .then(function (response) {
                            console.log("Form submission response: ", response.data);
                            // Handle the response from the server here
                        })
                        .catch(function (error) {
                            console.error('Error during form submission:', error);
                        });
                };

                // Manually trigger the form submission
                form.submit();
            }

            //$scope.result = response.data.message;
        })
        .catch(function (error) {
            console.error('Error:', error);
        });
};
$scope.EsignPdf = function () {
    debugger;
    //COMMON_FUNCTION.EsinghPDF_Test($('#txnId').val(), $('#txnId').val(), _Model.LoanBankName_ENG, "Managing Director")
    $http.get("../ApplicationFlow/VerifyAadhaarOTPForEsing_Test/?txnId=" + $('#txnId').val() + "&pdfPath=" + $('#PdfPath').val())
        .then(function (data) {
            console.log(data);
            // $scope.result = response.data.message;
        })
        .catch(function (error) {
            console.error('Error:', error);
        });
};

$scope.DecResponseData = function () {
    debugger;
    _callback = $('#Esign_callback').val();

    $http.get("../ApplicationFlow/GenerateAadhaarOTPForEsingDec_Test/")
        .then(function (data) {           
            $('#txnId').val(data.data);
            // $scope.result = response.data.message;
            ShowOkMessage("", _language == 'en-us' ? "Aadhaar Number Successfully Varified" : "आधार नंबर सफलतापूर्वक सत्यापित", function () { _callback(data.data); });
        })
        .catch(function (error) {
            console.error('Error:', error);
        });
};



if ($('#TxnIdDecData').val() != null && $('#TxnIdDecData').val() != '' && $('#Esign_callback').val() != null && $('#Esign_callback').val() != '' ) {
    debugger;
    $scope.DecResponseData();
}

