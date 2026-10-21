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


var _janaadhaarNo = "", _JanMemberId = "";
var _schemeName = "";


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


function JanAadhaarAuth(JanAadhaarNo, JanMemberId, SchemeName, AADHAARNo, AuthType, http, IsESign, RegistrationNo, Directory, callbackSuccess, isShowConfirm, customMmessage, callbackFailer) {
    //alert(1);
    if (AADHAARNo == undefined) {
        AADHAARNo = "";
    }
    if (JanAadhaarNo == undefined) {
        JanAadhaarNo = "";
    }

    if ((JanAadhaarNo.length != 10 && AADHAARNo.length != 12 && JanMemberId == "") && !IsESign) {
        if (AADHAARNo != "" && AADHAARNo.length != 12)
            ShowOkMessage("Warning", _language == 'en-us' ? "Invalid Aadhaar number. Aadhaar must be 12 digit number" : 'अमान्य आधार संख्या। आधार 12 अंकों की संख्या होना चाहिए', function () { });
        else if (JanAadhaarNo != "" && JanAadhaarNo.length != 10)
            ShowOkMessage("Warning", _language == 'en-us' ? "Invalid JanAadhaar number. JnaAadhaar must be 10 digit number" : 'अमान्य जनआधार संख्या। आधार 10 अंकों की संख्या होना चाहिए', function () { });

    }
    else if (JanAadhaarNo != "") {
        if (JanAadhaarNo != null && JanAadhaarNo != "") { encryptedJanAadhaarNo = encryptData(JanAadhaarNo); _janaadhaarNo = encryptedJanAadhaarNo; }
        _callback = callbackSuccess;
        _http = http;
        _schemeName = SchemeName;
        FetchFamilyDetails();
    }
    else {
        _isESign = IsESign;
        _transactionId = "";
        _callback = callbackSuccess;
        _http = http;
        var encryptedAadhaarNo = "", encryptedJanAadhaarNo = "", encryptedJanMemberId = "";
        if (AADHAARNo != null && AADHAARNo != "") {
            encryptedAadhaarNo = encryptData(AADHAARNo);
            _aadhaarNo = encryptedAadhaarNo;
        }
        if (JanAadhaarNo != null && JanAadhaarNo != "") {
            encryptedJanAadhaarNo = encryptData(JanAadhaarNo);
            _janaadhaarNo = encryptedJanAadhaarNo;
        }
        if (JanMemberId != null && JanMemberId != "") {
            encryptedJanMemberId = isNumber(JanMemberId) ? encryptData(JanMemberId) : JanMemberId;
            _JanMemberId = encryptedJanMemberId;
        }
        _callbackFailer = callbackFailer == undefined ? function () { } : callbackFailer;
        _isShowConfirm = isShowConfirm == undefined ? false : isShowConfirm;
        _customMmessage = customMmessage == undefined ? "" : customMmessage;
        _Directory = Directory == undefined ? "" : Directory;
        _RegistrationNo = RegistrationNo == undefined ? "" : RegistrationNo;
        _schemeName = SchemeName;
        _JanMemberId = JanMemberId;

        if (!_isESign) {
            $("#aadharMask").html("XXXXXXXX" + AADHAARNo.substring(8, 12));
        } else { $("#aadharMask").hide(); }

        $("#btnSSOAuth_SendOTP").show();
        $("#btnSSOAuth_ValidateOTP").hide();
        $("#btnSSOAuth_ReSendOTP").hide();

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

        $("#dvAuthAllow").hide();
        $("#SSOAuth_chkConsent").prop("checked", false);
        $("#SSOAuth_OTP").hide();
        $('#SSOAuth_OtpText').val('');

        //$("#otpsect").show();
        //$("#biosect").hide();
        $('.se-pre-con').hide();
        $("#dvAadhaarValidation").show();
    }
}
function GenerateOTP() {
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
                    else if (data.status == false) {
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
function ValidateOTP() {
    $('.se-pre-con').show();
    if (!_isESign) {
        //   _http.get(SiteUrl + "/Citizen/ValidateOTP/?_AadharNo=" + _aadhaarNo + "&otp=" + $('#SSOAuth_OtpText').val() + "&txn=" + _transactionId + '&SocietyRegistrationNo=' + _RegistrationNo + '&_SocietyDirectory=' + _Directory).
        _http.post(SiteUrl + "/CommonFunctionsJanAadhaar/ValidateOTP_FetchRequestedData", {
            "OTP": $('#SSOAuth_OtpText').val(),
            "tid": _transactionId,
            "SocietyDirectory": _Directory,
            "SchemeName": _schemeName,
            "memberId": encodeURIComponent(_JanMemberId ? _JanMemberId : _JanMemberId),
            "janAadharNo": encodeURIComponent(_janaadhaarNo)
        }).
            success(function (response, status) {
                $('.se-pre-con').hide();
                if (response.status && response.data != null) {
                    $('.se-pre-con').hide();
                    $('#SSOAuth_OtpText').val('');
                    $('#btnSSOAuth_SendOTP').show();
                    $('#btnSSOAuth_ValidateOTP').hide();
                    $('#btnSSOAuth_ReSendOTP').hide();
                    $("#dvAadhaarValidation").hide();

                    debugger
                    var aadhaarData = {
                        AadharNo: "",
                        Name: response.data[0].NAME_EN,
                        Name_HI: response.data[0].NAME_HINDI,
                        FatherName: response.data[0].FATHER_NAME_EN,
                        MotherName: response.data[0].MOTHER_NAME_EN,
                        DOB: response.data[0].DOB,
                        Gender: response.data[0].GENDER,
                        Photo: response.data[0].PHOTO ?? "member_0e54dbb10ee44883bb4b21264da4398c.jpg",
                        Photo_Base64: response.data[0].PHOTO ?? "",
                        MobileNo: response.data[0].MOBILE_NO,
                        Encrypted_JanAadharMID: response.data[0].Encrypted_JAN_MEMBER_ID,
                        Encrypted_JanAadharNo: response.data[0].Encrypted_JanAadharNo,
                        JanAadharNo: response.data[0].JanAadharNo,
                        MemberId: response.data[0].SRDR_MID,
                        AADHAR_REF_ID: response.data[0].AADHAR_REF_ID,
                        JanAadharEnrollmentId: response.data[0].ENR_ID,
                        IsRural: response.data[0].IS_RURAL == 'Y' ? 1 : 0,
                        District_Code: response.data[0].DISTRICT_CD,
                        Block_Code: response.data[0].IS_RURAL == "Y" ? response.data[0].BLOCK_CITY_CD : null,
                        GP_Code: response.data[0].IS_RURAL == "Y" ? response.data[0].GP_WARD_CD : null,
                        Village_Code: response.data[0].IS_RURAL == "Y" ? response.data[0].VILLAGE_CD : null,
                        City_Code: response.data[0].IS_RURAL == "N" ? response.data[0].BLOCK_CITY_CD : null,
                        Ward_Code: response.data[0].IS_RURAL == "N" ? response.data[0].GP_WARD_CD : null,
                        Address: response.data[0].ADDRESS,
                        PinCode: response.data[0].PIN_CODE,
                        Category: response.data[0].CATEGORY_DESC_ENG,
                        Cast: response.data[0].CAST,
                        Cast: response.data[0].MARITAL_STATUS,
                        janAaadharFamilyMemberList: response.data[0].janAaadharFamilyMemberList,
                        Email: "",
                        Address2: "",
                        House: "",
                        Street: "",
                        lm: "",
                        loc: "",
                        vtc: "",
                        Dist: "",
                        State: "",
                        pc: "",
                        Age: response.data[0].AGE,
                        tid: response.data[0].tid,
                        transactionId: response.data[0].transactionId
                    }
                    _callback(aadhaarData);
                    //  ShowOkMessage("Success", data.message, function () { });
                }
                else {
                    if (response.message != "") {
                        ShowOkMessage("Error", response.message, function () { });
                        $("#dvAadhaarValidation").hide();
                        _callback = null;
                    }
                    else {
                        ShowOkMessage("Error", data.message, function () { });
                    }


                }

                /*
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
                    
                }*/
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
function FetchFamilyDetails() {
    _http.post(SiteUrl + "CommonFunctionsJanAadhaar/FetchMemberList", { JanaadhaarNo: _janaadhaarNo, SchemeName: _schemeName }).
        success(function (data, status) {
            $('.se-pre-con').hide();
            if (data.length > 0) {
                $(".se-pre-con").fadeOut("slow");
                _callback(data);
                //ShowOkMessage("", _language == 'en-us' ? "Aadhaar Number Successfully Varified" : "आधार नंबर सफलतापूर्वक सत्यापित", function () { _callback(data); });
            }
            else {
                ShowOkMessage("Error", "", function () { });
            }

        }).
        error(function (data, status) {
            $('.se-pre-con').hide();
            ShowOkMessage("", _language == 'en-us' ? "Request could not be generated. Please try after some time." : "अनुरोध सबमिट नहीं हो पा रहा है। कृपया कुछ देर बाद प्रयास करें।", function () { });
        });
}

function isNumber(str) {
    return !isNaN(str) && str.trim() !== "";
}
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

