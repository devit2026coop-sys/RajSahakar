
var MasterMod = angular.module('UserSocietyMappingValidModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("UserSocietyMappingValidCont", function ($scope, $http, $timeout) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.Datacontainer = [];
    $scope.OTPStatus = false;

    $scope.divAADHARVerificationStep_1 = false;
    $scope.divAADHARVerificationStep_2 = false;
    $scope.disableTxtRegNo = false;
    $scope.MemberRegistration = {};
    $scope.GetSocietyDetails = true;
    $scope.IsActdisabled = true;

    $scope.ResendOtp = function () {
        $scope.showResendBtn = true;
    };


    //START : Het Society data for OTP
    $scope.GetSocityDataForOtp = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.textchange = 'true';
        $scope.Showgride = false;
        $scope.showotpblock = false;



        $http.post(SiteUrl + "/Citizen/GetSocityDataForOtp2", { "SocietyRegistrationNo": $scope.SocietyRegistration.SocietyRegistrationNo }).
            success(function (data, status) {

                console.log(data);

                var MessageHeading = _language === 'en-us' ? "User Society Mapping" : 'उपयोगकर्ता सोसायटी मानचित्रण';
                if (data.Result === false) {
                    $scope.Showgride = false;
                    $scope.textchange = false;
                    var msg1 = _language === 'en-us' ? 'Invalid Society Registration No. Would you like to new Profiling ?' : 'अमान्य सोसायटी पंजीकरण संख्या. क्या आप नई सोसाइटी प्रोफाइलिंग करना चाहते है ?';
                    if (confirm(msg1)) {
                        window.location = "../Citizen/SocietyProfile";
                    }
                    else {
                        return false;
                    }

                }
                else {

                    if (data.data[0].IsSsoFound === true && data.data[0].IsSameSSO === false) {
                        var msg3 = _language === 'en-us' ? 'This society is already mapped with SSO other than you.Would you like to file a complaint?' : 'यह सोसाइटी पहले से ही आपके अलावा अन्य एसएसओ के साथ मैप हो चुकी है। क्या आप शिकायत दर्ज करना चाहेंगे ?';
                        if (confirm(msg3)) {
                            $scope.disableTxtRegNo = true;
                            var complaintMessage = _language === 'en-us' ? 'For Complaint file following details required.' : 'शिकायत दर्ज करने के लिए निम्नलिखित विवरण आवश्यक है।';
                            $scope.SocietyId = data.data[0].SocietyIDEnc;

                            if (data.data[0].ActId.length > 0) {
                                $scope.IsActdisabled = true;
                                $scope.MemberRegistration.ActId = data.data[0].ActId;
                            }
                            else {
                                $scope.IsActdisabled = true;
                            }

                            ShowModelAlert(MessageHeading, complaintMessage);
                            $scope.getotp = true;
                            $scope.SocietyRegdata = [];
                            $scope.SocietyRegdata = data;
                            $scope.textchange = 'true';
                            $scope.divAADHARVerificationStep_1 = true;
                            $scope.isDisabled = false;
                            $scope.MemberRegistration.AADHARNo = data.data[0].LoggedInUserMaskedAADHAR;
                            $scope.MemberRegistration.AdhaNoPure = data.data[0].LoggedInUserAADHAR;
                        }
                        else {
                            return;
                        }

                    }
                    else if (data.data[0].IsSsoFound === true && data.data[0].IsSameSSO === true) {
                        var msg5 = _language === 'en-us' ? 'You have already mapped this society with your own SSO Id.' : 'आप पहले ही अपने SSO Id से इस सोसाइटी की मैपिंग कर चुके हैं।';
                        ShowModelAlert(MessageHeading, msg5);
                        return;
                    }
                    else {
                        $scope.disableTxtRegNo = true;
                        var msg4 = _language === 'en-us' ? 'For society profiling following details required.' : 'सोसायटी प्रोफाइलिंग के लिए निम्नलिखित विवरण आवश्यक है।';
                        $scope.SocietyId = data.data[0].SocietyIDEnc;

                        if (data.data[0].ActId.length > 0) {
                            $scope.IsActdisabled = true;
                            $scope.MemberRegistration.ActId = data.data[0].ActId;
                        }
                        else {
                            $scope.IsActdisabled = true;
                        }



                        ShowModelAlert(MessageHeading, msg4);
                        $scope.getotp = true;
                        $scope.SocietyRegdata = [];
                        $scope.SocietyRegdata = data.data;
                        $scope.textchange = 'true';
                        $scope.divAADHARVerificationStep_1 = true;
                        $scope.isDisabled = false;
                        $scope.MemberRegistration.AADHARNo = data.data[0].LoggedInUserMaskedAADHAR;
                        $scope.MemberRegistration.AdhaNoPure = data.data[0].LoggedInUserAADHAR;
                    }

                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
    };
    //END : Het Society data for OTP


    //START : Verify AADHAr No.
    $scope.VerifyAADHARNo_Step_1_2 = function () {

        var tile = _language === 'en-us' ? 'Society Mapping Request' : 'सोसाइटी मानचित्रण अनुरोध';
        // alert($scope.MemberRegistration.AdhaNoPure);
        $scope.disablebtnSendOtp = true;
        //if ($scope.MemberRegistration.AADHARNo === undefined || $scope.MemberRegistration.AADHARNo.length < 12) {
        //    var msg = _language === 'en-us' ? 'Please enter 12 digit AADHAAR No.' : '12 अंकों की आधार संख्या दर्ज करें';
        //    $scope.disablebtnSendOtp = false;
        //    ShowOkMessage(tile, msg, function () { });
        //    return;
        //}


        $scope.divAADHARVerificationStep_1 = false;
        $scope.divAADHARVerificationStep_2 = false;

        $scope.isDisabled = true;
        $timeout(function () {
            $scope.isDisabled = false;
        }, 10000);

        $scope.countDown = 10;
        var timer = setInterval(function () { $scope.countDown--; if ($scope.countDown < -1) { $scope.isDisabled = false; return; } }, 1000);

        //$(".se-pre-con").fadeOut("slow");

        //Bye Pass END

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/SocietyMemberRegistration/GenerateOTPForPLDOTPBMemberRegistration", { _AadharNo: $scope.MemberRegistration.AdhaNoPure }).
            success(function (data, status) {

                if (data.Result === "1" || data.Result === true) {
                    $scope.MemberRegistration.txn = data.txnid;
                    $scope.MemberRegistration.AADHARNoEnc = data.AadharNo;
                    $scope.divAADHARVerificationStep_1 = false;
                    $scope.divAADHARVerificationStep_2 = true;
                    $(".se-pre-con").fadeOut("slow");
                    var msg1 = _language === 'en-us' ? 'THE OTP HAS BEEN SENT TO AADHAR REGISTERED MOBILE NUMBER' : 'ओटीपी आधार पंजीकृत मोबाइल नंबर पर भेजा गया है';
                    ShowOkMessage(tile, msg1, function () { });
                }
                else {
                    $(".se-pre-con").fadeOut("slow");
                    var msg2 = _language === 'en-us' ? 'ERROR !!! INVALID AADHAAR No. OR SERVICE IS DOWN' : 'त्रुटि !!! अमान्य आधार नंबर या सेवा बंद है ';
                    //msg2 = (data.Message).splt('#')[1];
                    ShowOkMessage(tile, msg2, function () { });
                    $scope.divAADHARVerificationStep_1 = true;
                    $scope.divAADHARVerificationStep_2 = false;
                    $scope.isDisabled = false;
                }
            }).
            error(function (data, status) {

            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : Verify AADHAr No.

    //START : Verify AADHAr No.
    $scope.VerifyAADHARNo_Step_2_2 = function () {

        var tile = _language === 'en-us' ? 'Society Mapping/Objection Request' : 'सोसाइटी मानचित्रण/आपत्ति अनुरोध';

        if ($scope.MemberRegistration.EnterOTP == undefined || $scope.MemberRegistration.EnterOTP.length != 6) {
            var msg = _language === 'en-us' ? 'Please enter 6 digit OTP' : '6 अंकों की ओटीपी दर्ज करें';
            ShowOkMessage(tile, msg, function () { });
            return;
        }

        // START : Test for Verification
        //var data = { "Result": true, "Message": "Success", "AadharNo": "AeS5cagS6t6LxgjZpCq7FQ==", "txnid": "", "AADHARDetails": [{ "Column1": "Success", "Column2": "Narottam Saini", "Column3": "M", "Column4": "22-11-1985", "Column5": "../Content/NewRegistration/member_a8b5165053a245a9a4d9d095db7af468.jpg", "Column6": "नरोत्तम सैनी", "Column7": "AeS5cagS6t6LxgjZpCq7FQ==", "Column8": "S/O Om Prakash Saini", "Column9": "S/O  ओम प्रकाश सैनी" }] };

        //if (data.Result) {
        //    $scope.MemberRegistration["Name"] = _language === 'en-us' ? data.AADHARDetails[0].Column2 : data.AADHARDetails[0].Column6;
        //    $scope.MemberRegistration["FatherName"] = _language === 'en-us' ? data.AADHARDetails[0].Column8 : data.AADHARDetails[0].Column9;
        //    var newdate = data.AADHARDetails[0].Column4;//.split('-')[1] + '-' + data.AADHARDetails[0].Column4.split('-')[1] + '-' + data.AADHARDetails[0].Column4.split('-')[2];
        //    $scope.MemberRegistration["DOB"] = newdate;
        //    $scope.MemberRegistration["AADHARNo"] = $scope.MemberRegistration.AADHARNo;
        //    $scope.MemberRegistration["Gender"] = data.AADHARDetails[0].Column3;
        //    $scope.MemberRegistration["Photo"] = data.AADHARDetails[0].Column5;
        //    $scope.MemberRegistration["Photo"] = data.AADHARDetails[0].Column5;
        //    $scope.MemberRegistration["SocietyId"] = '0x01000000A78446A4ABB892ED1C12E8A141C78B75EDF29EA9A0B34CA5';//$scope.SocietyId;
        //    $scope.GetCategoryList();
        //    $scope.ActList();
        //    $scope.PostList();
        //    $scope.divAADHARVerificationStep_2 = false;
        //    $scope.GetSocietyDetails = false;

        //    $scope.Showgride = true;
        //    //$('#txtRegistrationDate').val().split("/").reverse().join("-"));

        //    //$scope.MemberRegistration.DOB = data.AADHARDetails[0].Column4.split("-").reverse().join("-");
        //    //$scope.MemberRegistration.Age = Math.abs(Math.round(diff / 365.25));
        //    //$scope.MemberRegistration.Gender = data.AADHARDetails[0].Column3 === 'M' ? '1' : (data.AADHARDetails[0].Column3 === 'F' ? 2 : 3);
        //    //$scope.MemberRegistration.PhotoPath = data.AADHARDetails[0].Column5;
        //}
        // END : Test for Verification


        // Un-comment below code after testing
        $http.post(SiteUrl + "/SocietyMemberRegistration/ValidateOTPForPLDBMemberRegistration", { "otp": $scope.MemberRegistration.EnterOTP, "txn": $scope.MemberRegistration.txn, "_AadharNo": $scope.MemberRegistration.AADHARNoEnc }).
            success(function (data, status) {
                console.log(data);
                if (data.Result === false || data.Result === "0") {
                    alert(data[0].Column1 + ': Due To' + data[0].Column2);
                    return;
                }
                else if (data.Result === true) {
                    $scope.MemberRegistration["Name"] = _language === 'en-us' ? data.AADHARDetails[0].Column2 : data.AADHARDetails[0].Column6;
                    $scope.MemberRegistration["FatherName"] = _language === 'en-us' ? data.AADHARDetails[0].Column8 : data.AADHARDetails[0].Column9;
                    var newdate = data.AADHARDetails[0].Column4;
                    $scope.MemberRegistration["DOB"] = newdate;
                    $scope.MemberRegistration["AADHARNo"] = $scope.MemberRegistration.AADHARNo;
                    $scope.MemberRegistration["Gender"] = data.AADHARDetails[0].Column3;
                    $scope.MemberRegistration["Photo"] = data.AADHARDetails[0].Column5;
                    $scope.MemberRegistration["Photo"] = data.AADHARDetails[0].Column5;
                    $scope.MemberRegistration["Address"] = data.AADHARDetails[0].Column10;
                    $scope.MemberRegistration["SocietyId"] = $scope.SocietyId;
                    $scope.GetCategoryList();
                    $scope.ActList();
                    $scope.PostList();
                    $scope.divAADHARVerificationStep_2 = false;
                    $scope.GetSocietyDetails = false;

                    $scope.Showgride2 = true;

                }
                else {
                    return;
                }
            }).
            error(function (data, status) {
                $scope.SocietyRegdatanew = [];
            });
    }
    // END : Verify AADHAr No.


    //START : Validate Society Mapping Request Data
    $scope.ValidateSocietyMappingRequestData = function () {
        var Msg = '';
        var tile = _language === 'en-us' ? 'Society Mapping Request' : 'सोसाइटी मानचित्रण अनुरोध';

        if ($scope.MemberRegistration.ActId === '0') {
            Msg += _language === 'en-us' ? 'Please select Act</br>' : 'कृपया अधिनियम का चयन करें</br>';
        }


        if ($scope.MemberRegistration.MobileNo === undefined || $scope.MemberRegistration.MobileNo.length < 10) {
            Msg += _language === 'en-us' ? 'Please enter Mobile No.</br>' : 'कृपया मोबाइल नंबर दर्ज करें</br>';
        }

        if ($scope.MemberRegistration.CategoryId === '0') {
            Msg += _language === 'en-us' ? 'Please select category</br>' : 'कृपया श्रेणी का चयन करें</br>';
        }

        if ($scope.MemberRegistration.PostName === undefined || $scope.MemberRegistration.PostName.length < 1) {
            Msg += _language === 'en-us' ? 'Please write post</br>' : 'कृपया पोस्ट दर्ज करें</br>';
        }

        if ($scope.MemberRegistration.Occupation === undefined || $scope.MemberRegistration.Occupation.length < 1) {
            Msg += _language === 'en-us' ? 'Please provide occupation</br>' : 'कृपया व्यवसाय प्रदान करें</br>';
        }

        if (Msg.length > 0) {
            ShowOkMessage(tile, Msg, function () { });
            return false;
        }

        return true;
    };
    //END : Validate Society Mapping Request Data

    //START : Insert Society Mapping Request
    $scope.InsertSocietyMappingRequest = function () {

        if (!$scope.ValidateSocietyMappingRequestData()) {
            return;
        }

        $http.post(SiteUrl + "/Citizen/SocietyMappingInsert", { model: $scope.MemberRegistration }).
            success(function (data, status) {
                var title = _language === 'en-us' ? 'Society Mapping Request' : 'सोसाइटी मानचित्रण अनुरोध';
                if (parseInt(data, 10) > 0) {
                    ShowModelAlert(title, _language === 'en-us' ? 'Society Mapping Request accepted successfully.' : 'सोसाइटी मैपिंग अनुरोध सफलतापूर्वक स्वीकार किया गया', function () { window.location = '../citizen/usersocietymappingvalid'; });
                    window.location = "../Citizen/usersocietymappingvalid";
                }
                else if (parseInt(data, 10) === -1) {
                    ShowModelAlert(title, _language === 'en-us' ? 'Error !! No Action Performed.' : 'त्रुटि !! कोई कार्रवाई नहीं की गई।');
                }
                else if (parseInt(data, 10) === -2) {
                    ShowModelAlert(title, _language === 'en-us' ? 'Request for Society mapping is already generated.' : 'सोसाइटी मैपिंग के लिए अनुरोध पहले ही प्रस्तुत हो चुका है।।');
                }
            }).
            error(function (data, status) {
                console.log('Error in InsertSocietyMappingRequest function.');
            });
    };
    //END : Insert Society Mapping Request

    $scope.txtNewID = '';
    $scope.GenrateOTP = function () {
        $scope.IsOTPSended = true;
        $scope.DivMsg = true;
        $scope.textchange = 'true';
        $scope.getotp = true;
        $scope.Showgride = true;
        $scope.showotpblock = true;
        $http.get(SiteUrl + "/Citizen/GenrateOTPdata/?SocietyRegistrationNo=" + $scope.SocietyRegistration.SocietyRegistrationNo).
            success(function (data, status) {

                if (data.length > 0) {
                    $scope.OTPStatus = true;
                    if (data[0].txnid.includes("DOITRJGOVAUTH")) {
                        $scope.SocietyRegistration.txn = data[0].txnid;
                        $scope.SocietyRegdata = data;
                        $scope.textchange = 'false';
                        $scope.OTPStatusMsg = 'THE OTP HAS BEEN SENT TO AADHAR REGISTERED MOBILE NUMBER OF CEO';
                    }
                    else {
                        $scope.OTPStatusMsg = data[0].txnid;
                        $("#DivOTPStatus").removeClass("alert alert-success");
                        $("#DivOTPStatus").addClass("alert alert-danger");
                        $scope.invalidotp = true;
                        $scope.textchange = 'false';
                    }
                }
            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
            });

    }
    $scope.MatchOtpApproved = function () {
        $scope.textchange = 'true';
        $http.get(SiteUrl + "/Citizen/ValidateOTP/?SocietyRegistrationNo=" + $scope.SocietyRegistration.SocietyRegistrationNo + "&otp=" + $scope.SocietyRegistration.EnterOTP + "&txn=" + $scope.SocietyRegistration.txn).
            success(function (data, status) {
                $scope.SocietyRegdatanew = data;
                if (data[0].Column1 == "Failed") {
                    alert(data[0].Column1 + ': Due To' + data[0].Column2)
                    $scope.textchange = 'false';
                    $scope.invalidotp = true;
                    $scope.validotp = false;
                    $scope.showotpblock = false;
                }
                else if (data[0].Column1 == "Success") {
                    $scope.invalidotp = false;
                    $scope.validotp = true;
                    $scope.showotpblock = false;
                    alert("Our Society Successfully Mapped With Your SSOID");
                    $scope.textchange = 'false';
                    window.location = "../Citizen/Index";
                }
                else {
                    $scope.invalidotp = false;
                    $scope.showotpblock = false;
                    return;
                }
            }).
            error(function (data, status) {
                $scope.SocietyRegdatanew = [];
            });
        $scope.textchange = 'false';
    }
    //End 

    // START : Get Category  List
    $scope.GetCategoryList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=-1").
            success(function (data, status) {
                $scope.CategoryList = data;
            }).
            error(function (data, status) {
                $scope.CategoryList = [];
            });
    }
    // END: Get Category List


    //START : Act list
    $scope.ActList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
            success(function (data, status) {
                $scope.Acts = data;
            }).
            error(function (data, status) {
                $scope.Acts = [];
            });
    };
    //END : Act List

    //START : Post list
    $scope.PostList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=-1").
            success(function (data, status) {
                $scope.PostData = data;
            }).
            error(function (data, status) {
                $scope.PostData = [];
            });
    };
    //END : Post List


    // on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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

            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }

        $scope.Department = [];
        $http.get("/Master/GetMasterData/?MasterType=Department&MasterCode=-1").
            success(function (data, status) {
                $scope.Department = data;
            }).
            error(function (data, status) {
                $scope.Department = [];
            });

        $(".se-pre-con").fadeOut("slow");

    };


    $scope.OnPageLoad();




});






