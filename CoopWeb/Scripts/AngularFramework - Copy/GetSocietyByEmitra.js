
var MasterMod = angular.module('GetSocietyByEmitraModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("GetSocietyByEmitraCont", function ($scope, $http) {

    // SCOPE
   
    $scope.divTakeSocietyNo = false;
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.Datacontainer = [];
    $scope.OTPStatus = false;
    $scope.Identity = '';
    $scope.ShowOTPVerification = false;
    $scope.divSendOTP = false;
    $scope.divShowSocietyNo = false;
    $scope.SocietyRegistration = [];
    $scope.divAADHARVerificationStep_2 = false;
    $scope.Search = {};
    $scope.Showgride2 = false;

    //START : Refresh
    $scope.Refresh = function () {
        window.location - '../Emitra/GetSocietyByEmitra';
    };
    //END : Refresh


    //START : Take Registraion No For Process
    $scope.TakeRegistraionNoForProcess = function (actionid) {
        $scope.divTakeSocietyNo = true;
        $scope.Showgride = false;
        $scope.ShowOTPVerification = false;
        $scope.divSendOTP = false;
        $scope.divShowSocietyNo = true;
        $scope.SocietyRegistration = [];
        $scope.SocietyRegistration["SocietyRegistrationNo"] = '';
        $scope.ActionId = actionid;
        $scope.ShowGridForMapWithSSO = false;
        $scope.Search = {};

    };
    //END ; Take Registraion No For Process

    //START : BackToSearch
    $scope.BackToSearch = function () {
        $scope.TakeRegistraionNoForProcess();
    };
    //END : BackToSearch


    //START : BackToSearch
    $scope.ShowAADHARNoForm = function () {
        $scope.divTakeSocietyNo = false;
        $scope.Showgride = false;
        $scope.ShowOTPVerification = false;
        $scope.divShowSocietyNo = false;
        $scope.SocietyRegistration = [];
        $scope.SocietyRegistration["SocietyRegistrationNo"] = '';
        $scope.divSendOTP = true;
    };
    //END : BackToSearch

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

    //START : Send OTP On AADHAR
    $scope.VerifyAADHARNo_Step_1_2 = function () {


        if (!$('#ckkAADHARAcceptance').prop('checked')) {
            alert('Please allow system for your AADHAAR based Authentication/ eKYC.');
            return;
        }

        if ($scope.Search.AADHARNo === undefined || $scope.Search.AADHARNo.length < 12) {
            var msg = _language === 'en-us' ? 'Please enter 12 digit AADHAAR No.' : '12 अंकों की आधार संख्या दर्ज करें';
            $scope.disablebtnSendOtp = false;
            ShowOkMessage('', msg);
            return;
        }


        $scope.divSendOTP = false;
        $scope.divAADHARVerificationStep_2 = false;
        $scope.divInsert = false;



        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/SocietyMemberRegistration/GenerateOTPForPLDOTPBMemberRegistration", { _AadharNo: $scope.Search.AADHARNo }).
            success(function (data, status) {

                if (data === 'LOGOUT') {
                    window.location = 'https://sso.rajasthan.gov.in/signin';
                }

                if (data.Result) {
                    $scope.Search.txn = data.txnid;
                    $scope.Search.AADHARNoEnc = data.AadharNo;
                    $scope.divSendOTP = false;
                    $scope.divAADHARVerificationStep_2 = true;
                    var msg1 = _language === 'en-us' ? 'THE OTP HAS BEEN SENT TO AADHAR REGISTERED MOBILE NUMBER' : 'ओटीपी आधार पंजीकृत मोबाइल नंबर पर भेजा गया है';
                    alert(msg1);
                    $(".se-pre-con").fadeOut("slow");
                }
                else {
                    $(".se-pre-con").fadeOut("slow");
                    var msg2 = _language === 'en-us' ? 'ERROR !!! INVALID AADHAAR No. OR SERVICE IS DOWN' : 'त्रुटि !!! अमान्य आधार नंबर या सेवा बंद है ';
                    $scope.divAADHARVerificationStep_1 = true;
                    alert(msg2);
                }
            }).
            error(function (data, status) {

            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END :  Send OTP On AADHAR


    //START : Verify AADHAr No.
    $scope.VerifyAADHARNo_Step_2_2 = function () {

        if ($scope.Search.EnterOTP === undefined || $scope.Search.EnterOTP.length !== 6) {
            var msg = _language === 'en-us' ? 'Please enter 6 digit OTP' : '6 अंकों की ओटीपी दर्ज करें';
            alert(msg);
            return;
        }

        $http.post(SiteUrl + "/SocietyMemberRegistration/ValidateOTPForPLDBMemberRegistration", { "otp": $scope.Search.EnterOTP, "txn": $scope.Search.txn, "_AadharNo": $scope.Search.AADHARNoEnc }).
            success(function (data, status) {

                if (data === 'LOGOUT') {
                    window.location = 'https://sso.rajasthan.gov.in/signin';
                }

                if (data.Result === false) {
                    alert(data[0].Column1 + ': Due To' + data[0].Column2);
                    return;
                }
                else if (data.Result === true) {

                    $scope.Search["Name"] = _language === 'en-us' ? data.AADHARDetails[0].Column2 : data.AADHARDetails[0].Column6;
                    $scope.Search["FatherName"] = _language === 'en-us' ? data.AADHARDetails[0].Column8 : data.AADHARDetails[0].Column9;
                    var newdate = data.AADHARDetails[0].Column4;
                    $scope.Search["DOB"] = newdate;
                    $scope.Search["AADHARNo"] = $scope.Search.AADHARNo;
                    $scope.Search["Gender"] = data.AADHARDetails[0].Column3;
                    $scope.Search["Photo"] = data.AADHARDetails[0].Column5;
                    $scope.Search["Address"] = data.AADHARDetails[0].Column10;
                    $scope.Identity = data.AADHARDetails[0].Column3;
                    
                    $scope.GetCategoryList();
                    $scope.ActList();
                    $scope.PostList();
                    $scope.divSendOTP = false;
                    $scope.divAADHARVerificationStep_2 = false;
                    $scope.Showgride2 = true;
                }
                else {
                    return;
                }
            }).
            error(function (data, status) {
                $scope.SocietyRegdatanew = [];
            });
    };
    // END : Verify AADHAr No.

    //START : Validate Society Mapping Request Data
    $scope.ValidateSocietyMappingRequestData = function () {
        var Msg = '';
        var tile = _language === 'en-us' ? 'Society Mapping Request' : 'सोसाइटी मानचित्रण अनुरोध';

        if ($scope.Search.ActId === '0') {
            Msg += _language === 'en-us' ? 'Please select Act</br>' : 'कृपया अधिनियम का चयन करें</br>';
        }


        if ($scope.Search.MobileNo === undefined || $scope.Search.MobileNo.length < 10) {
            Msg += _language === 'en-us' ? 'Please enter Mobile No.</br>' : 'कृपया मोबाइल नंबर दर्ज करें</br>';
        }

        if ($scope.Search.CategoryId === '0') {
            Msg += _language === 'en-us' ? 'Please select category</br>' : 'कृपया श्रेणी का चयन करें</br>';
        }

        if ($scope.Search.PostName === undefined || $scope.Search.PostName.length < 1) {
            Msg += _language === 'en-us' ? 'Please write post</br>' : 'कृपया पोस्ट दर्ज करें</br>';
        }

        if ($scope.Search.Occupation === undefined || $scope.Search.Occupation.length < 1) {
            Msg += _language === 'en-us' ? 'Please provide occupation</br>' : 'कृपया व्यवसाय प्रदान करें</br>';
        }

        if (Msg.length > 0) {
            ShowOkMessage(tile, Msg, function () { });
            return false;
        }

        return true;
    };
    //END : Validate Society Mapping Request Data

    //START : BackToSearch
    $scope.MapYourSociety = function (item) {
        $scope.SocietyIdEncrypted = item.SocietyIDEnc;
        $scope.SocietyRegistrationNo = item.RegistrationNo;
        $scope.ShowGridForMapWithSSO = false;
        $scope.divSendOTP = true;
        $scope.Showgride = false;
        console.log($scope.SocietyIdEncrypted);
    };
    //END : BackToSearch
    


    //START : Insert Society Mapping Request
    $scope.InsertSocietyMappingRequest = function () {

        if (!$scope.ValidateSocietyMappingRequestData()) {
            return;
        }

        $scope.Search["SocietyId"] = $scope.SocietyIdEncrypted;

        console.log($scope.Search);
        $http.post(SiteUrl + "/Citizen/SocietyMappingInsert", JSON.stringify({ model: $scope.Search })).
            success(function (data, status) {
                var title = _language === 'en-us' ? 'Society Mapping Request' : 'सोसाइटी मानचित्रण अनुरोध';
                if (parseInt(data, 10) > 0) {

                    if (parseInt($scope.ActionId,10)===3) {
                        $scope.SocietyRegistration.SocietyRegistrationNo = $scope.SocietyRegistrationNo;
                        alert(_language === 'en-us' ? 'Society Mapping Request accepted successfully. Now you are redirectiong to Profiling page.' : 'सोसाइटी मैपिंग अनुरोध सफलतापूर्वक स्वीकार किया गया | अब आप प्रोफाइल पेज पर रीडायरेक्ट किये जाएंगे।');
                        $scope.RedirectToProfilePage();
                    }
                    else {
                        alert(_language === 'en-us' ? 'Objection registered successfully.' : 'आपत्ति सफलतापूर्वक दर्ज की गई।');
                        window.location = '../Emitra/GetSocietyByEmitra';
                    }
                }
                else if (parseInt(data, 10) === -1) {
                    ShowModelAlert(title, _language === 'en-us' ? 'Error !! No Action Performed.' : 'त्रुटि !! कोई कार्रवाई नहीं की गई।');
                }
                else if (parseInt(data, 10) === -2) {
                    ShowModelAlert(title, _language === 'en-us' ? 'Request for Society mapping is already generated.' : 'सोसाइटी मैपिंग के लिए अनुरोध पहले ही प्रस्तुत हो चुका है।');
                }
            }).
            error(function (data, status) {
                console.log('Error in SSO - Society Mapping function.');
            });
    };
    //END : Insert Society Mapping Request

    //START : ResendOTP
    $scope.ResendOTP = function () {

        $scope.disablebtnSendOtp = true;
        $scope.divAADHARVerificationStep_2 = false;

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/SocietyMemberRegistration/GenerateOTPForPLDOTPBMemberRegistration", { _AadharNo: $scope.Search.AADHARNo }).
            success(function (data, status) {

                if (data === 'LOGOUT') {
                    window.location = 'https://sso.rajasthan.gov.in/signin';
                }

                if (data.Result) {
                    $scope.Search.txn = data.txnid;
                    $scope.Search.AADHARNoEnc = data.AadharNo;
                    $scope.divSendOTP = false;
                    $scope.divAADHARVerificationStep_2 = true;
                    var msg1 = _language === 'en-us' ? 'THE OTP HAS BEEN SENT TO AADHAR REGISTERED MOBILE NUMBER' : 'ओटीपी आधार पंजीकृत मोबाइल नंबर पर भेजा गया है';
                    alert(msg1);
                    $(".se-pre-con").fadeOut("slow");
                }
                else {
                    $(".se-pre-con").fadeOut("slow");
                    var msg2 = _language === 'en-us' ? 'ERROR !!! INVALID AADHAAR No. OR SERVICE IS DOWN' : 'त्रुटि !!! अमान्य आधार नंबर या सेवा बंद है ';
                    alert(msg2);
                }
            }).
            error(function (data, status) {

            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : ResendOTP


    $scope.GetSocityDataForOtp = function () {
        $scope.textchange = 'true';
        $scope.Showgride = false;
        $scope.showotpblock = false;
        $http.post(SiteUrl + "/Citizen/GetSocityDataForOtp2/?SocietyRegistrationNo=" + $scope.SocietyRegistration.SocietyRegistrationNo).
            success(function (data, status) {
                if (data.Result === false) {
                    var responseMessage = _language === 'en-us' ? 'Searched Society Registration no. not found in system. Do you want a fresh Society Registration?' : 'खोजा गया  सोसायटी पंजीकरण  संख्या सिस्टम में नहीं मिला। क्या आप एक नया सोसायटी पंजीकरण चाहते हैं ?';

                    if (confirm(responseMessage)) {
                        window.location = '../Citizen/SocietyProfile';
                    }
                    else {
                        console.log('No Action taken by user.');
                    }
                    $scope.Showgride = false;
                    $scope.textchange = false;
                }
                else {
                    $scope.SocietyRegdata = [];
                    $scope.SocietyRegdata = data.data;

                    if (parseInt($scope.ActionId,10) === 1) {
                        $scope.Showgride = true;
                        $scope.ShowGridForMapWithSSO = false;
                    }
                    else {
                        $scope.Showgride = false;
                        $scope.ShowGridForMapWithSSO = true;
                    }
                    
                    $scope.divShowSocietyNo = false;
                    $scope.SocietyRegistration["SocietyRegistrationNo"] = '';
                    //$scope.textchange = 'true';
                }

            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
            });
    }
    $scope.txtNewID = '';



    $scope.GenrateOTP = function (societyName) {

        var msg = _language === 'en-us' ? ' I hereby state that I have no objection in authenticating myself with AADHAAR based authentication system and consent to providing my AADHAAR Number or OTP for AADHAAR based Authantication/ eKYC. I also give my explicit consent for accessing the mobile number and email address from AADHAAR System' : 'मैं यह बताता हूं कि मुझे आधार आधारित प्रमाणीकरण प्रणाली और आधार आधारित स्वायत्तता / ईकेवाईसी के लिए अपना आधार नंबर या ओटीपी प्रदान करने की सहमति के साथ खुद को प्रमाणित करने में कोई आपत्ति नहीं है। मैं आधार प्रणाली से मोबाइल नंबर और ईमेल पते तक पहुंचने के लिए अपनी स्पष्ट सहमति देता हूं।';

        if (!confirm(msg)) {
            return false;
        }


        $scope.textchange = 'true';
        $scope.SocietyRegistration.SocietyRegistrationNo = societyName;
        $("#DivOTPStatus").removeClass("alert alert-danger");
        $("#DivOTPStatus").addClass("alert alert-success");
        $scope.IsOTPSended = true;
        $scope.DivMsg = true;
        $scope.textchange = 'true';
        //$scope.getotp = true;
        $scope.Showgride = true;
        $scope.showotpblock = true;
        $scope.Showgride = false;
        $http.get(SiteUrl + "/Citizen/GenrateOTPdata/?SocietyRegistrationNo=" + societyName).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.OTPStatus = true;
                    if (data[0].txnid.includes("DOITRJGOVAUTH")) {
                        $scope.SocietyRegistration.txn = data[0].txnid;
                        $scope.SocietyRegdata = data;
                        $scope.textchange = 'false';
                        $scope.OTPStatusMsg = 'THE OTP HAS BEEN SENT TO AADHAR REGISTERED MOBILE NUMBER OF CEO';
                        $scope.ShowOTPVerification = true;
                    }
                    else {

                        $scope.OTPStatusMsg = data[0].txnid;

                        $("#DivOTPStatus").removeClass("alert alert-success");
                        $("#DivOTPStatus").addClass("alert alert-danger");
                        $scope.invalidotp = true;
                        $scope.textchange = 'false';
                        $scope.OTPStatusMsg = 'SOME ERROR OCCURD IN AADHAAR SERVER. PLEASE TRY AFTER SOME TIME';
                        $scope.OTPStatus = true;
                        $scope.ShowOTPVerification = false;
                    }
                }
            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
            });

    };

    $scope.MatchOtpApproved = function () {

        //  JSON.stringify($scope.SocietyRegdata);
        $scope.textchange = 'true';
        $http.get(SiteUrl + "/Citizen/ValidateOTP/?SocietyRegistrationNo=" + $scope.SocietyRegistration.SocietyRegistrationNo + "&otp=" + $scope.SocietyRegistration.EnterOTP + "&txn=" + $scope.SocietyRegistration.txn).
            success(function (data, status) {
                $scope.SocietyRegdatanew = data;
                if (data[0].Column1 == "Failed") {
                    // alert(data[0].Column1 + ': Due To' + data[0].Column2)
                    $scope.textchange = 'false';
                    $scope.invalidotp = true;
                    $scope.validotp = false;
                    $scope.showotpblock = false;
                    $("#DivOTPStatus").removeClass("alert alert-success");
                    $("#DivOTPStatus").addClass("alert alert-danger");
                    $scope.OTPStatusMsg = data[0].Column1 + ': Due To' + data[0].Column2;
                    $scope.Identity = 'T1+Bhe8Ju9s4QjBaM66sAg==';
                    $scope.RedirectToProfilePage();
                }
                else if (data[0].Column1 == "Success") {
                    $scope.Identity = data[0].Column3;
                    $scope.invalidotp = false;
                    $scope.validotp = true;
                    $scope.showotpblock = false;
                    //alert("Our Society Successfully Mapped With Your SSOID");
                    ////window.location.href = SiteUrl + "/Citizen/CitizenDashboard";
                    $scope.textchange = 'false';
                    $scope.RedirectToProfilePage();

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
    };
    //End 

    $scope.RedirectToProfilePage = function () {
        $http.get(SiteUrl + "/Emitra/RedirectToProfilePage/?SocietyRegistrationNo=" + $scope.SocietyRegistration.SocietyRegistrationNo + "&Identity=" + $scope.Identity).
            success(function (data, status) {
                window.location = "../Citizen/SocietyProfile";
            }).
            error(function (data, status) {
                // $scope.SocietyRegdata = [];
            });
    };

    // on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope._language = _language;
        if (_language === 'en-us') {
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


    };




    $scope.OnPageLoad();




});






