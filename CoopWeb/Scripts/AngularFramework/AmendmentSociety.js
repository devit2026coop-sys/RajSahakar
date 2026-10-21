
var MasterMod = angular.module('UserSocietyMappingValidModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("UserSocietyMappingValidCont", function($scope, $http, $timeout) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 1000;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.Datacontainer = [];
    $scope.OTPStatus = false;

    $scope.divAADHARVerificationStep_1 = false;
    $scope.divAADHARVerificationStep_2 = false;
    $scope.showSoceitygride = false;
    $scope.disableTxtRegNo = false;
    $scope.MemberRegistration = {};
    $scope.GetSocietyDetails = true;
    $scope.IsActdisabled = true;
    $scope.RegistrationNo = '';

    $scope.ResendOtp = function() {
        //alert('');
        $scope.showResendBtn = true;
        $scope.VerifyAADHARNo_Step_1_2();
    };
    $scope.pageSize = 10;
    $scope.divAADHARVerificationStep_1 = false;
    //START : Het Society data for OTP
    $scope.getdisable = false;
    $scope.GetSocietyDetails = true;
    $scope.IsSearchResult = false;
    $scope.IsShowProfiling = false;
    $scope.IsShowAmendment = false;
    $scope.IsShowSSOMAP = false;
    $scope.Act1958 = true;
    $scope.IsWaitingForApproval = false;
    $scope.ShowButtonSaveProfile = false;



    $scope.GetSocityDataForOtp = function() {
        $(".se-pre-con").fadeIn("slow");
        $scope.textchange = 'true';
        $scope.Showgride = false;
        $scope.showotpblock = false;
        $http.post(SiteUrl + "/Citizen/GetSocityDataForOtp2", { "SocietyRegistrationNo": $scope.SocietyRegistration.SocietyRegistrationNo, "DistrictId": $scope.ddlDistrct.DISTRICT_ID }).
            success(function(data, status) {
                $(".se-pre-con").fadeOut("slow");
                console.log(data);

                var resultData = data.data.filter(function (x) { return x.ActId == 1 });

                $scope.showSoceitygride = true;
                var MessageHeading = _language === 'en-us' ? "User Society Mapping" : 'उपयोगकर्ता सोसायटी मानचित्रण';


                if (data.Result === false) {
                    $scope.Showgride = false;
                    $scope.textchange = false;
                    $scope.showSoceitygride = false;
                    var msg1 = 'आपकी सोसाइटी की जानकारी राज-सहकार पोर्टल पर उपलब्ध नहीं  है। संशोधन  का आवेदन करने से पहले आपको अपनी सोसाइटी की जानकारी दर्ज कर प्रोफाइलिंग करनी होगी।  <br/><span style="font-size:15px;color: red;"> Note:- आप द्वारा प्रोफाइलिंग के दौरान दर्ज की गयी जानकारी का विभाग द्वारा सत्यापन किया जायेगा उसके बाद आप संशोधन हेतु आवेदन कर पाएंगे। </span> '
                    ShowAlertMessage_v1("", "प्रोफाइलिंग करें", "रद्द करें", msg1, function() {
                        $scope.IsSearchResult = false;
                        $scope.IsShowProfiling = true;
                        $scope.IsShowAmendment = false;
                        $scope.divOtherSocietys == true;
                        $scope.GetSocietyDetails = false;
                        $scope.IsProfiling = true;
                        var addressdataset1 = {
                            Housebuilding: " ",
                            State: "6",
                            District: " ",
                            Lanestreet: "",
                            Locality: "",
                            Pincode: "",
                            CITY_ID: "",
                            BLOCK_ID: "",
                            GP_ID: "",
                            WARD_Id: "",
                            VILLAGE_ID: ""
                        }
                        //$scope.BrnDetails();
                        SetAddressinfo($scope.translation, 'MemberAdd', addressdataset1, $http).then(function() {

                        });

                        $scope.$apply();
                    });

                }
                else {
                    $scope.SocietyRegdata = [];
                    $scope.SocietyRegdata = resultData;
                    $scope.IsSearchResult = true;
                    $scope.IsShowProfiling = false;
                    $scope.IsShowAmendment = false;
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function(data, status) {
                $scope.SocietyRegdata = [];
                $(".se-pre-con").fadeOut("slow");
            });
        //$(".se-pre-con").fadeOut("slow");
    };
    $scope.BackToListClipk = function() {
        $scope.IsSearchResult = true;
        $scope.GetSocietyDetails = true;
        $scope.IsShowProfiling = false;
        $scope.IsShowAmendment = false;
    }
    $scope.SelectUserForVerify = function(items, index) {
        $scope.datasetUsers.forEach(function(item1, index) {
            item1.ischecked = false;
            $scope.Showwhenclick = true;
        });
        items.ischecked = true;
        $scope.divAADHARVerificationStep_1 = false;
        $scope.Showwhenclick = true;
        $scope.NowShowAdhardiv = items.ischecked;

    }
    $scope.getlistchairman = function(SocietyIdEnc) {
        //alert(SocietyIdEnc);
        $scope.GetUserListtoVerify(SocietyIdEnc);
        $scope.ShowAlertForUserwithData = true;

    }
    $scope.GetUserListtoVerify = function(SocietyIdEnc) {
        $http.get(SiteUrl + "/Citizen/GetUserListtoVerify/?SocietyIdEnc=" + SocietyIdEnc).
            success(function(data, status) {
                if (data.length > 0) {
                    $scope.datasetUsers = data;
                    $scope.ShowAlertForUserwithData = true;
                    $scope.datasetUsers = $scope.datasetUsers.filter(function (POSTNAME) { return POSTNAME.includes("SEC") || POSTNAME.includes("PRESI") });
                    console.log($scope.datasetUsers);
                }
            }).
            error(function(data, status) {
                $scope.SocietyRegdata = [];
            });
    }
    $scope.SocietyIDEncget = "";
    //END : Het Society data for OTP
    $scope.getDis = false;
    $scope.SocietyRegdataSelected = [];
    $scope.showAmendmentSubmitedText = false;
    $scope.showAmendmentText = false;
    $scope.SelectSocietyForVerify = function(item, index) {
        //alert(SocietyID);
        $scope.SocietyRegdata.forEach(function(item1, index) {
            item1.ischecked = false;
        });
        item.ischecked = true;
        var MessageHeading = _language === 'en-us' ? "User Society Mapping" : 'उपयोगकर्ता सोसायटी मानचित्रण';
        if (item.IsSsoFound === true && item.IsSameSSO === false) {
            var msg3 = 'यह सोसाइटी किसी और की SSO ID से मैप है। क्या आप इसे अपनी SSO ID से मैप करना चाहते है ?';
            $scope.SocietyRegdataSelected = [];
            ShowAlertMessage("", msg3, function() {
                var data = item;
                $scope.SocietyAmendment.SocietyIdEnc = data.SocietyIDEnc + '~' + data.SocietyDirectory + '~' + data.ActId;
                $scope.IsSearchResult = false;
                $scope.IsShowProfiling = false;
                $scope.IsShowAmendment = false;
                $scope.IsShowSSOMAP = true;
                $scope.IsShowSSOMAPNoMAP = false
                $scope.GetSocietyDetails = false;
                $scope.IsWaitingForApproval = false;
                $scope.SocietyRegdataSelected.push(item);
                $scope.SocietyIDEncget = $scope.SocietyRegdataSelected[0].SocietyIDEnc;
                $scope.$apply();
            });
        } else if (item.IsSsoFound === true && item.IsSameSSO === true && (item.ApprovalStatusId == 1 || item.ApprovalStatusId == 5 || item.ApprovalStatusId == 10 || item.ApprovalStatusId == 20 || item.ApprovalStatusId == 27)) {
            $scope.ShowDetails(item.SocietyIDEnc, 'ShowDetails');
            $scope.ShowDetailsWorkscope(item.SocietyIDEnc, 'ShowDetails')
            $scope.divOtherSocietys == true;
            $scope.ShowButtonSaveProfile = true;
            $scope.IsSearchResult = false;
            $scope.IsShowProfiling = true;
            $scope.IsShowAmendment = false;
            $scope.GetSocietyDetails = false;
            $scope.IsWaitingForApproval = false;
            $scope.nepster = true;
            $scope.showtabbar = true;
            $scope.IsSocietyNamedisabled = true;
            $scope.divOtherSocietys = true;
        }       
        else if (item.IsSsoFound === true && item.IsSameSSO === true && (item.ApprovalStatusId == 17 || item.ApprovalStatusId == 26)) {
            //var msg5 = _language === 'en-us' ? 'You have already mapped this society with your own SSO Id.' : 'आप पहले ही अपने SSO Id से इस सोसाइटी की मैपिंग कर चुके हैं।';
            //ShowOkMessage(MessageHeading, msg5, function () { });
            $scope.ShowButtonSaveProfile = false;
            var data = item;
            $scope.SocietyAmendment.SocietyIdEnc = data.SocietyIDEnc + '~' + data.SocietyDirectory + '~' + data.ActId;
            $scope.IsWaitingForApproval = false;
            $scope.ShowTab = true;
            $scope.IsSocietyEdit = data.IsAmendmentEdit == 'True' ? false : true;
            $scope.IsEditApplication = true;
            $scope.GetSocietyDetails = false;
            $scope.SocietyAmendment.TotalMember = item.TotalMember;

            $scope.GetMemberList().then(function() {
                if (!$scope.IsSocietyEdit) {
                    $scope.GetSocietyNameChangeList().then(function() {
                        $(".se-pre-con").fadeOut("slow");
                    });
                } else {
                    $(".se-pre-con").fadeOut("slow");
                }
            });


            $scope.IsSearchResult = false;
            $scope.IsShowProfiling = false;
            $scope.IsShowAmendment = true;
            $scope.IsShowSSOMAP = false;
            $scope.showAmendmentSubmitedText = false;
            $scope.showAmendmentText = true;

        } else if (item.IsSsoFound === true && item.IsSameSSO === true && item.IsAmendment=='True') {
            //var msg5 = _language === 'en-us' ? 'You have already mapped this society with your own SSO Id.' : 'आप पहले ही अपने SSO Id से इस सोसाइटी की मैपिंग कर चुके हैं।';
            //ShowOkMessage(MessageHeading, msg5, function () { });
            $scope.ShowButtonSaveProfile = false;
            var data = item;
            $scope.SocietyAmendment.SocietyIdEnc = data.SocietyIDEnc + '~' + data.SocietyDirectory + '~' + data.ActId;
            $scope.IsWaitingForApproval = false;
            $scope.ShowTab = true;
            $scope.IsSocietyEdit = data.IsAmendmentEdit == 'True' ? false : true;
            $scope.IsEditApplication = $scope.IsSocietyEdit;
            $scope.GetSocietyDetails = false;
            $scope.SocietyAmendment.TotalMember = item.TotalMember;

            $scope.GetMemberList().then(function () {
                if (!$scope.IsSocietyEdit) {
                    $scope.GetSocietyNameChangeList().then(function () {
                        $(".se-pre-con").fadeOut("slow");
                    });
                } else {
                    $(".se-pre-con").fadeOut("slow");
                }
            });


            $scope.IsSearchResult = false;
            $scope.IsShowProfiling = false;
            $scope.IsShowAmendment = true;
            $scope.IsShowSSOMAP = false;
            $scope.showAmendmentSubmitedText = false;
            $scope.showAmendmentText = true;
            $scope.showAmendmentSubmitedText = true;
            $scope.showAmendmentText = false;
        }
        else if (item.IsSsoFound === true && item.IsSameSSO === true) {
            $scope.ShowDetails(item.SocietyIDEnc, 'ShowDetails');
            $scope.ShowDetailsWorkscope(item.SocietyIDEnc, 'ShowDetails')
            $("#SocietyStep_33").addClass("active").removeClass("disabled");
            $("#dvprofilingSection").find("input").attr("disabled", "disabled");
            $("#dvprofilingSection").find("select").attr("disabled", "disabled");
            $scope.ShowButtonSaveProfile = false;
            $scope.IsWaitingForApproval = true;
            $scope.IsSearchResult = false;
            $scope.IsShowProfiling = true;
            $scope.IsShowAmendment = false;
            $scope.GetSocietyDetails = false;
            $scope.nepster = true;
            $scope.showtabbar = true;
            $scope.IsSocietyNamedisabled = true;
            $scope.divOtherSocietys = true;
        } else {
            var msg5 = _language === 'en-us' ? 'Do you want to map this society with your SSOID?. <br><span style="font-size: 15px;color: red;">*You will not get any wrong information or false facts recorded on this portal while mapping this society with your SSOID. <br />If any information is found to be inaccurate then the Cooperative Department will be free to take action against you under the necessary legal action IT Act and other established Acts.</span> ' : 'क्यां आप इस सोसाइटी को अपनी SSOID से मैप करना चाहते हैं ? <br/><span style="font-size: 15px;color: red;">*आप इस सोसाइटी को अपनी SSOID  से मैप करने के  दौरान  कोई भी गलत जानकारी अथवा मिथ्य तथ्य इस पोर्टल पर दर्ज नहीं करवाएंगे | <br /> यदि कोई जानकारी गलत पाई जाती है तो सहकारिता विभाग आपके विरुद्ध आवश्यक कानूनी कर्यवाही आईटी एक्ट एवं अन्य सपठित अधिनियमों के अंतर्गत कर्यवाही करने में स्वतंत्र होगा।</span>';
            ShowAlertMessage("", "यह सोसाइटी किसी भी SSO ID से मैप नहीं हैं। क्यां आप इस सोसाइटी को अपनी SSO ID से मैप करना चाहते हैं ?", function() {
                $scope.ShowDetails(item.SocietyIDEnc, 'ShowDetails');
                $scope.ShowDetailsWorkscope(item.SocietyIDEnc, 'ShowDetails')
                $scope.divOtherSocietys == true;
                $scope.ShowButtonSaveProfile = true;
                $scope.IsSearchResult = false;
                $scope.IsShowProfiling = true;
                $scope.IsShowAmendment = false;
                $scope.GetSocietyDetails = false;
                $scope.IsWaitingForApproval = false;
                $scope.nepster = true;
                $scope.showtabbar = true;
                $scope.IsSocietyNamedisabled = true;
                $scope.divOtherSocietys = true;

                $scope.$apply();
            });

        }
    }
    //START : Verify AADHAr No.
    $scope.VerifyAADHARNo_Step_1_2 = function (AdhaNoPure) {

        var tile = _language === 'en-us' ? 'Society Mapping Request' : 'सोसाइटी मानचित्रण अनुरोध';
        // alert($scope.MemberRegistration.AdhaNoPure);
        $scope.disablebtnSendOtp = true;
        //if ($scope.MemberRegistration.AADHARNo === undefined || $scope.MemberRegistration.AADHARNo.length < 12) {
        //    var msg = _language === 'en-us' ? 'Please enter 12 digit AADHAAR No.' : '12 अंकों की आधार संख्या दर्ज करें';
        //    $scope.disablebtnSendOtp = false;
        //    ShowOkMessage(tile, msg, function () { });
        //    return;
        //}
        //$scope.MemberRegistration.AdhaNoPure = $scope.MemberRegistration.AdhaNoPure.replace(/ /g, '');
        AadhaarAuth(AdhaNoPure, $http, "OTP-BIO", false, function (data) {
            //$scope.MemberRegistration["Name"] = data.Name;
            //$scope.MemberRegistration["FatherName"] = data.FName;
            ////var newdate = data.AADHARDetails[0].Column4;
            //$scope.MemberRegistration["DOB"] = data.DOB;
            //$scope.MemberRegistration["AADHARNo"] = $scope.MemberRegistration.AADHARNo;
            //$scope.MemberRegistration["Gender"] = data.Gender;
            //$scope.MemberRegistration["Photo"] = data.Photo;
            //$scope.MemberRegistration["Address"] = data.Address;
            //$scope.MemberRegistration["SocietyId"] = $scope.SocietyId;
            //$scope.MemberRegistration["AADHARNoEnc"] = data.AADHARNoEnc;
            $scope.GetCategoryList();
            //$scope.ActList();
            $scope.PostList();
            $scope.divAADHARVerificationStep_2 = false;
            $scope.GetSocietyDetails = false;
            $scope.Showgride2 = true;
            $scope.Showgride3 = false;
            $scope.$apply();

        }, true, $scope._language == 'en-us' ? "INVALID AADHAAR No. OR SERVICE IS DOWN. Do you want to map this society.? If yes, click on Yes button." : "अमान्य आधार नंबर या सेवा बंद है | क्यां आप इस सोसाइटी को आधिकारिक रूप से मैप करना चाहते हैं | यदि हाँ तो Yes बटन पर क्लिक करें |", function () {
            $scope.MemberRegistration["SocietyId"] = $scope.SocietyId;
            $scope.SocietyNameForDecalre = $scope.SocietyRegdata[0].SocietyName;
            $scope.divAADHARVerificationStep_2 = false;
            $scope.GetSocietyDetails = false;
            $scope.Showgride2 = false;
            $scope.Showgride3 = true;
            $scope.$apply();
        });

        $scope.divAADHARVerificationStep_1 = false;
        $scope.divAADHARVerificationStep_2 = false;


    };
    // END : Verify AADHAr No.

    $scope.selectSSOMAPCeo = function(item) {
        if ($scope.ischeck1) {
            $scope.ischeck2 = false;
        }
        if ($scope.ischeck2) {
            $scope.ischeck1 = false;
        }
    }

    $scope.ShowAADHARNoForRequest = false;
    $scope.VerifyAADHARNoForRequest = function () {

        var tile = _language === 'en-us' ? 'Society Mapping Request' : 'सोसाइटी मानचित्रण अनुरोध';
        // alert($scope.MemberRegistration.AdhaNoPure);
        $scope.disablebtnSendOtp = true;
        //if ($scope.MemberRegistration.AADHARNo === undefined || $scope.MemberRegistration.AADHARNo.length < 12) {
        //    var msg = _language === 'en-us' ? 'Please enter 12 digit AADHAAR No.' : '12 अंकों की आधार संख्या दर्ज करें';
        //    $scope.disablebtnSendOtp = false;
        //    ShowOkMessage(tile, msg, function () { });
        //    return;
        //}
        //$scope.MemberRegistration.AdhaNoPure = $scope.MemberRegistration.AdhaNoPure.replace(/ /g, '');

        var AdhaNoPure = $("#hdnCurrentAadhar").val();
        AadhaarAuth(AdhaNoPure, $http, "OTP-BIO", false, function (data) {
            $scope.MemberRegistration["Name"] = data.Name;
            $scope.MemberRegistration["FatherName"] = data.FName;
            //var newdate = data.AADHARDetails[0].Column4;
            $scope.MemberRegistration["DOB"] = data.DOB;
            $scope.MemberRegistration["AADHARNo"] = $scope.MemberRegistration.AADHARNo;
            $scope.MemberRegistration["Gender"] = data.Gender;
            $scope.MemberRegistration["Photo"] = data.Photo;
            $scope.MemberRegistration["Address"] = data.Address;
            $scope.MemberRegistration["SocietyId"] = $scope.SocietyId;
            $scope.MemberRegistration["AADHARNoEnc"] = data.AADHARNoEnc;
            $scope.GetCategoryList();
            //$scope.ActList();
            //$scope.PostList();
            $scope.ShowAADHARNoForRequest = true;
            $scope.divAADHARVerificationStep_2 = false;
            $scope.GetSocietyDetails = false;
            $scope.Showgride2 = true;
            $scope.Showgride3 = false;
            $scope.$apply();
        }, true, $scope._language == 'en-us' ? "INVALID AADHAAR No. OR SERVICE IS DOWN. Do you want to map this society.? If yes, click on Yes button." : "अमान्य आधार नंबर या सेवा बंद है | क्यां आप इस सोसाइटी को आधिकारिक रूप से मैप करना चाहते हैं | यदि हाँ तो Yes बटन पर क्लिक करें |", function () {
            $scope.MemberRegistration["SocietyId"] = $scope.SocietyId;
            $scope.SocietyNameForDecalre = $scope.SocietyRegdata[0].SocietyName;
            $scope.divAADHARVerificationStep_2 = false;
            $scope.GetSocietyDetails = false;
            $scope.Showgride2 = false;
            $scope.Showgride3 = true;
            $scope.$apply();
        });

        $scope.divAADHARVerificationStep_1 = false;
        $scope.divAADHARVerificationStep_2 = false;


    };
    //START : Verify AADHAr No.
    $scope.VerifyCEOAUTH = function(AdhaNoPure) {

        var tile = _language === 'en-us' ? 'Society Mapping Request' : 'सोसाइटी मानचित्रण अनुरोध';
        // alert($scope.MemberRegistration.AdhaNoPure);
        $scope.disablebtnSendOtp = true;
        //if ($scope.MemberRegistration.AADHARNo === undefined || $scope.MemberRegistration.AADHARNo.length < 12) {
        //    var msg = _language === 'en-us' ? 'Please enter 12 digit AADHAAR No.' : '12 अंकों की आधार संख्या दर्ज करें';
        //    $scope.disablebtnSendOtp = false;
        //    ShowOkMessage(tile, msg, function () { });
        //    return;
        //}
        //$scope.MemberRegistration.AdhaNoPure = $scope.MemberRegistration.AdhaNoPure.replace(/ /g, '');
        AadhaarAuth(AdhaNoPure, $http, "OTP-BIO", false, function(data) {
            $scope.MemberRegistration["Name"] = data.Name;
            $scope.MemberRegistration["FatherName"] = data.FName;
            //var newdate = data.AADHARDetails[0].Column4;
            $scope.MemberRegistration["DOB"] = data.DOB;
            $scope.MemberRegistration["AADHARNo"] = $scope.MemberRegistration.AADHARNo;
            $scope.MemberRegistration["Gender"] = data.Gender;
            $scope.MemberRegistration["Photo"] = data.Photo;
            $scope.MemberRegistration["Address"] = data.Address;
            $scope.MemberRegistration["SocietyId"] = $scope.SocietyId;
            $scope.MemberRegistration["AADHARNoEnc"] = data.AADHARNoEnc;
            $scope.GetCategoryList();
            //$scope.ActList();
            //$scope.PostList();
            $scope.divAADHARVerificationStep_2 = false;
            $scope.GetSocietyDetails = false;
            $scope.Showgride2 = true;
            $scope.Showgride3 = false;
            $scope.$apply();
        }, true, $scope._language == 'en-us' ? "INVALID AADHAAR No. OR SERVICE IS DOWN. Do you want to map this society.? If yes, click on Yes button." : "अमान्य आधार नंबर या सेवा बंद है | क्यां आप इस सोसाइटी को आधिकारिक रूप से मैप करना चाहते हैं | यदि हाँ तो Yes बटन पर क्लिक करें |", function() {
            $scope.MemberRegistration["SocietyId"] = $scope.SocietyId;
            $scope.SocietyNameForDecalre = $scope.SocietyRegdata[0].SocietyName;
            $scope.divAADHARVerificationStep_2 = false;
            $scope.GetSocietyDetails = false;
            $scope.Showgride2 = false;
            $scope.Showgride3 = true;
            $scope.$apply();
        });

        $scope.divAADHARVerificationStep_1 = false;
        $scope.divAADHARVerificationStep_2 = false;


    };
    // END : Verify AADHAr No.



    //START : Verify AADHAr No.
    $scope.VerifyAADHARNo_Step_2_2 = function() {
        $(".se-pre-con").fadeIn("slow");
        $scope.MemberRegistration.RegNo = $scope.SocietyRegistration.SocietyRegistrationNo;
        var tile = _language === 'en-us' ? 'Society Mapping/Objection Request' : 'सोसाइटी मानचित्रण/आपत्ति अनुरोध';

        if ($scope.MemberRegistration.EnterOTP == undefined || $scope.MemberRegistration.EnterOTP.length != 6) {
            $(".se-pre-con").fadeOut("slow");
            var msg = _language === 'en-us' ? 'Please enter 6 digit OTP' : '6 अंकों की ओटीपी दर्ज करें';
            ShowOkMessage(tile, msg, function() { });
            return;
        }
        // Un-comment below code after testing
        $http.post(SiteUrl + "/SocietyMemberRegistration/ValidateOTPForPLDBMemberRegistration", { "otp": $scope.MemberRegistration.EnterOTP, "txn": $scope.MemberRegistration.txn, "_AadharNo": $scope.MemberRegistration.AADHARNoEnc }).
            success(function(data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.Result === false || data.Result === "0") {
                    // alert(data[0].Column1 + ': Due To' + data[0].Column2);
                    alert(data.Message);
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
                    $scope.MemberRegistration["Address"] = data.AADHARDetails[0].Column10;
                    $scope.MemberRegistration["SocietyId"] = $scope.SocietyId;
                    $scope.MemberRegistration["AADHARNoEnc"] = data.AADHARDetails[0].Column7;
                    $scope.GetCategoryList();
                    //$scope.ActList();
                    $scope.PostList();
                    $scope.divAADHARVerificationStep_2 = false;
                    $scope.GetSocietyDetails = false;
                    $scope.Showgride2 = true;
                    $scope.Showgride3 = false;
                    $scope.IsProfiling = true;

                    // $scope.CheckBRN = true;
                    $scope.nepster = true;
                    $scope.showtabbar = true;
                    $scope.IsSocietyNamedisabled = true;
                    $scope.divOtherSocietys = true;
                    if ($scope.SocietyRegistration.IsNewRegistration != null && $scope.SocietyRegistration.IsNewRegistration != undefined) {
                        $scope.DivAdministrative = true;
                        $scope.DivAddressDetails = true;
                    }

                    $scope.textchange = 'false';
                    $scope.invalidotp = true;
                    $scope.validotp = false;
                    $scope.showotpblock = false;
                }
                else {
                    return;
                }
            }).
            error(function(data, status) {
                $scope.SocietyRegdatanew = [];
                $(".se-pre-con").fadeOut("slow");
            });

    }
    // END : Verify AADHAr No.


    //START : Validate Society Mapping Request Data
    $scope.ValidateSocietyMappingRequestData = function (flag) {
        var Msg = '';
        var tile = _language === 'en-us' ? 'Society Mapping Request' : 'सोसाइटी मानचित्रण अनुरोध';

        //if ($scope.MemberRegistration.ActId === '0') {
        //    Msg += _language === 'en-us' ? 'Please select Act</br>' : 'कृपया अधिनियम का चयन करें</br>';
        //}

        if ($scope.MemberRegistration.Name === undefined || $scope.MemberRegistration.Name == '') {
            Msg += _language === 'en-us' ? 'Please enter Name.</br>' : 'कृपया नाम नंबर दर्ज करें</br>';
        }

        if ($scope.MemberRegistration.MobileNo === undefined || $scope.MemberRegistration.MobileNo.length < 10) {
            Msg += _language === 'en-us' ? 'Please enter Mobile No.</br>' : 'कृपया मोबाइल नंबर दर्ज करें</br>';
        }

        if ($scope.MemberRegistration.CategoryId === '0') {
            Msg += _language === 'en-us' ? 'Please select category</br>' : 'कृपया श्रेणी का चयन करें</br>';
        }

        if ($scope.MemberRegistration.PostName === undefined || $scope.MemberRegistration.PostName.length < 1) {
            Msg += _language === 'en-us' ? 'Please select post</br>' : 'कृपया का चयन करें</br>';
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
    $scope.InsertSocietyMappingRequest = function (flag) {
        $scope.MemberRegistration.RegNo = $scope.RegistrationNo;
        $scope.MemberRegistration.SocietyIDEnc = $scope.SocietyIDEncget;
        $(".se-pre-con").fadeIn("slow");
        if (!$scope.ValidateSocietyMappingRequestData(flag)) {
            $(".se-pre-con").fadeOut("slow");
            return;
        }
        $scope.MemberRegistration.FileName = $scope.GetFileUploaded('MapplingFileAtt');
        $http.post(SiteUrl + "/Citizen/SocietyMappingInsert", { model: $scope.MemberRegistration, Flag: flag }).
            success(function (data, status) {
                //alert(parseInt(data, 10));
                $(".se-pre-con").fadeOut("slow");
                var title = _language === 'en-us' ? 'Society Mapping Request' : 'सोसाइटी मानचित्रण अनुरोध';
                if (parseInt(data, 10) > 0) {
                    ShowOkMessage("", _language === 'en-us' ? 'Society Mapping Request accepted successfully.' : 'सोसाइटी मैपिंग अनुरोध सफलतापूर्वक स्वीकार किया गया', function () {
                        window.location = "../Citizen/SocietyApplicationForm";
                    });
                }
                else if (parseInt(data, 10) === -1) {
                    ShowModelAlert(title, _language === 'en-us' ? 'Error !! No Action Performed.' : 'त्रुटि !! कोई कार्रवाई नहीं की गई।');
                }
                else if (parseInt(data, 10) === -2) {
                    ShowModelAlert(title, _language === 'en-us' ? 'Request for Society mapping is already generated.' : 'सोसाइटी मैपिंग के लिए अनुरोध पहले ही प्रस्तुत हो चुका है।।');
                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                console.log('Error in InsertSocietyMappingRequest function.');
            });
    };
    //END : Insert Society Mapping Request

    $scope.txtNewID = '';
    $scope.GenrateOTP = function() {
        $scope.IsOTPSended = true;
        $scope.DivMsg = true;
        $scope.textchange = 'true';
        $scope.getotp = true;
        $scope.Showgride = true;
        $scope.showotpblock = true;
        $http.get(SiteUrl + "/Citizen/GenrateOTPdata/?SocietyRegistrationNo=" + $scope.SocietyRegistration.SocietyRegistrationNo).
            success(function(data, status) {

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
            error(function(data, status) {
                $scope.SocietyRegdata = [];
            });

    }
    $scope.MatchOtpApproved = function() {
        $scope.textchange = 'true';
        $http.get(SiteUrl + "/Citizen/ValidateOTP/?SocietyRegistrationNo=" + $scope.SocietyRegistration.SocietyRegistrationNo + "&otp=" + $scope.SocietyRegistration.EnterOTP + "&txn=" + $scope.SocietyRegistration.txn).
            success(function(data, status) {
                $scope.SocietyRegdatanew = data;
                if (data[0].Column1 == "Failed") {
                    alert(data[0].Column1 + ': Due To' + data[0].Column2);
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
            error(function(data, status) {
                $scope.SocietyRegdatanew = [];
            });
        $scope.textchange = 'false';
    }
    //End 

    // START : Get Category  List
    $scope.GetCategoryList = function() {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=-1").
            success(function(data, status) {
                $scope.CategoryList = data;
            }).
            error(function(data, status) {
                $scope.CategoryList = [];
            });
    }
    // END: Get Category List


    //START : Act list
    $scope.ActList = function() {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
            success(function(data, status) {
                $scope.Acts = data;
            }).
            error(function(data, status) {
                $scope.Acts = [];
            });
    };
    //END : Act List

    //START : Post list
    $scope.PostList = function() {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=-1").
            success(function(data, status) {
                $scope.PostData = data;
            }).
            error(function(data, status) {
                $scope.PostData = [];
            });
    };
    //END : Post List


    // on page load
    $scope.OnPageLoad = function() {
        $(".se-pre-con").fadeIn("slow");
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function(data, status) {
                    $scope.translation = data;
                }).
                error(function(data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function(data, status) {
                    $scope.translation = data;
                }).
                error(function(data, status) {
                    alert("Error while translation");
                });

            $scope.ClearUnitForm = function() {
                $scope.UnitMaster = null;
            };
        }

        $scope.Department = [];
        $http.get("/Master/GetMasterData/?MasterType=Department&MasterCode=-1").
            success(function(data, status) {
                $scope.Department = data;
            }).
            error(function(data, status) {
                $scope.Department = [];
            });

        $(".se-pre-con").fadeOut("slow");

    };
    $scope.DistrictList = function() {
        $(".se-pre-con").fadeIn("slow");
        $scope.Districts = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=" + '0').
            success(function(data, status) {
                $scope.divDisticts = true;
                $scope.Districts = data;
                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function(column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function(column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                    }
                    return '';
                };
            }).
            error(function(data, status) {
                $scope.Districts = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    $scope.OnPageLoad();
    $scope.DistrictList();
    $scope.SocietyDetails = [];
    $scope.MapRegistrationNo = "";
    $scope.GetSocietyForMap = function() {
        if ($scope.MapRegistrationNo == '') {
            ShowOkMessage("", "Please Enter Registration Number", function() { });
        } else {
            $(".blockUI").fadeIn("slow");
            return $http.post("/Society/GetSocietyForMap", JSON.stringify({ RegistrationNo: $scope.MapRegistrationNo })).
                success(function(data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (JSON.parse(data.DataList).length <= 0) {
                        $scope.SocietyDetails = [];
                        ShowOkMessage("", "No Record Found", function() { });
                    } else {
                        $scope.SocietyDetails = JSON.parse(data.DataList);
                    }
                }).
                error(function(data, status) {
                    $scope.CertificateList = [];
                    $(".blockUI").fadeOut("slow");
                });
        }
    };

    $scope.SubmitSocietyForMap = function(Society) {
        var MapSSOId = Society.MapSSOId == undefined ? "" : Society.MapSSOId.replace(/ /g, '');
        if (MapSSOId == '') {
            ShowOkMessage("", "Please Enter SSO ID ", function() { });
        } else {
            $(".blockUI").fadeIn("slow");
            return $http.post("/Society/SubmitSocietyForMap", JSON.stringify({ SocietyId: Society.SocietyIDInc, SSOId: MapSSOId })).
                success(function(data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (data == true) {
                        ShowOkMessage("", "Society Mapped Successfully with SSO ID " + MapSSOId + " ", function() { window.location.href = "../Society/MapSociety" });
                    } else {
                        ShowOkMessage("", "There is some problem while updating society with new SSO Id. Please Try after some time", function() { });
                    }

                }).
                error(function(data, status) {
                    $(".blockUI").fadeOut("slow");
                });
        }
    }


    $scope.SubmitCitizonSocietyForMap = function(Society) {
        var MapSSOId = $("#hdnSSOID").val();
        //if (MapSSOId == '') {
        //    ShowOkMessage("", "Please Enter SSO ID ", function () { });
        //} else {
        $(".blockUI").fadeIn("slow");
        return $http.post("/Society/SubmitSocietyForMap", JSON.stringify({ SocietyId: $scope.MemberRegistration.SocietyId, SSOId: MapSSOId })).
            success(function(data, status) {
                $(".blockUI").fadeOut("slow");
                if (data == true) {
                    ShowOkMessage("", "Society Mapped Successfully with SSO ID " + MapSSOId + " ", function() { window.location.href = "../Citizen/UserSocietyMappingValid" });
                } else {
                    ShowOkMessage("", "There is some problem while updating society with new SSO Id. Please Try after some time", function() { });
                }

            }).
            error(function(data, status) {
                $(".blockUI").fadeOut("slow");
            });
        //}
    }

    /***********Profiling*********************************/
    $scope.SocietyRegistration = { ActId: "1", RegistrationNo: "", OldRegistrationNo: "", SocietyIDEnc: "" };
    $scope.SocietyAct = true;
    $scope.divBrn = true;
    $scope.divBrnbtn = true;
    $scope.SocietyRegistration.IsNewRegistration = false;
    $scope.SocietyRegistration.ActId = "1";
    $('#txtRegistrationDate').datepicker({
        showOn: "both",
        changeMonth: true,
        buttonImage: "../images/calender.png",
        changeYear: true,
        dateFormat: "dd/mm/yy",
        maxDate: '0D'
    });

    $('#txtElectionDate').datepicker({
        showOn: "both",
        changeMonth: true,
        buttonImage: "../images/calender.png",
        changeYear: true,
        dateFormat: "dd/mm/yy",
        maxDate: '0D'
    });

    $('#txtAGMDate').datepicker({
        showOn: "both",
        changeMonth: true,
        buttonImage: "../images/calender.png",
        changeYear: true,
        dateFormat: "dd/mm/yy",
        maxDate: '0D'
    });


    //Star Otheassocities grid validation
    $scope.FormValidationOthersocity = function(ValidationFlag) {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.SocietyRegistration.IsStatLevel == 'undefined' && (ValidationFlag == true || ValidationFlag == undefined)) {

            if ($scope.SocietyRegistration.Rural == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Rural/Urban' : '\n <br /> कृपया शहरी/ग्रामीण का चयन करे |';

            }
            //urban=false, rural=true
            if ($scope.SocietyRegistration.Rural == 'true') {

                if ($scope.SocietyRegistration.WorkScopeLevel_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Work Scope ' : '\n <br /> वर्कस्कोप का चयन करें|';

                }
                if ($scope.SocietyRegistration.DIVISION_ID == 0 && $scope.SocietyRegistration.WorkScopeLevel_ID >= 1) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Division Council' : '\n <br /> कृपया संभाग का चयन करे |';

                }
                if ($scope.SocietyRegistration.DISTRICT_ID == 0 && $scope.SocietyRegistration.WorkScopeLevel_ID >= 3) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select District Council' : '\n <br /> कृपया जिला परिषद का चयन करे |';

                }
                if ($scope.SocietyRegistration.BLOCK_ID == 0 && $scope.SocietyRegistration.WorkScopeLevel_ID >= 4) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Block' : '\n <br /> कृपया ब्लाक का चयन करे |';

                }
                if ($scope.SocietyRegistration.GP_ID == 0 && $scope.SocietyRegistration.WorkScopeLevel_ID >= 5) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Gram Panchayat' : '\n <br /> कृपया ग्रामपंचायत का चयन करे |';

                }
                if ($scope.SocietyRegistration.VILLAGE_ID == 0 && $scope.SocietyRegistration.WorkScopeLevel_ID >= 6) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Gram' : '\n <br /> कृपया ग्राम का चयन करे |';

                }
            }
            else if ($scope.SocietyRegistration.Rural == 'false') {
                if ($scope.SocietyRegistration.CityTypes == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select City type' : '\n <br /> कृपया सिटी टाइप का चयन करे |';

                }
                if ($scope.SocietyRegistration.CITY_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select City' : '\n <br /> कृपया शहर का चयन करे |';

                }
                if ($scope.SocietyRegistration.WARD_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Select Ward' : '\n <br /> कृपया वार्ड का चयन करे |';

                }
            }
        }
        if (message.length > 29) {
            ShowModelAlert("Society other Association Profiling", message);
            return false;
        }
        return true;
    }
    //End Otheassocities grid validation



    /****Masters****/

    $scope.GetSocietyDistrictList = function() {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyRegistration.DIVISION_ID).
            success(function(data, status) {
                $scope.AreaDistrictList = data;
            }).
            error(function(data, status) {
                $scope.AreaDistrictList = [];
            });
    }

    $scope.GetSocietyTypeList = function() {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=" + $scope.SocietyRegistration.ActId).
            success(function(data, status) {
                $scope.SocietyTypeList = data;
            }).
            error(function(data, status) {
                $scope.SocietyTypeList = [];
            });
    };
    $scope.GetSocietyTypeList();

    $scope.GetMajorActivity = function() {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MAJORACTIVITY&MasterCode=0").
            success(function(data, status) {
                $scope.MajorActivityList = data;
            }).
            error(function(data, status) {
                $scope.MajorActivityList = [];
            });
    }
    $scope.GetMajorActivity();


    // GET Activity
    $scope.GetActivity = function() {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACTIVITY&MasterCode=" + $scope.SocietyRegistration.MajorActivityId).
            success(function(data, status) {
                $scope.ActivityList = data;
            }).
            error(function(data, status) {
                $scope.ActivityList = [];
            });
        $scope.SocietyRegistration.BusinessId = '0';
    }
    //END
    // GET Business
    $scope.GetBusiness = function() {

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=BUSINESS&MasterCode=" + $scope.SocietyRegistration.ActivityId).
            success(function(data, status) {
                $scope.BusinessList = data;
            }).
            error(function(data, status) {
                $scope.BusinessList = [];
            });
    }
    //END


    // GET Society Class
    $scope.GetSocietyClass = function() {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYCLASS&MasterCode=" + $scope.SocietyRegistration.SocietyTypeId).
            success(function(data, status) {
                $scope.SocietyClassList = data;
            }).
            error(function(data, status) {
                $scope.SocietyClassList = [];
            });

    }
    //END
    //start Level of office data
    $scope.Levelofficedata = function(Cflag) {
        $scope.showtabbar = true;
        $('#ddlDistrict').val(0);
        $('#ddlblocki').val(0);
        $('#ddlGP').val(0);
        $('#ddlVillage').val(0);
        $('#ddlDivision').val(0);
        $('#ddlCity').val(0);

        if ($scope.SocietyRegistration.SocietyTypeId == 9) {
            $scope.SocietyRegistration.WorkScopeLevel_ID = 1;
            $scope.SocietyRegistration.IsStatLevel = 'Yes';
            $('#Iddstateyes').attr("disabled", "disabled");
            $('#Iddstateno').attr("disabled", "disabled");
            $scope.lstSocietyWorkScope = [];
            $scope.Addmore = false;
        }
        else {
            if ($scope.IsEditMode == true) {

            }
            else {
                if (Cflag == 1)
                    $scope.SocietyRegistration.IsStatLevel = undefined;
            }
            $scope.lstSocietyWorkScope = [];
            $scope.Addmore = true;
        }
        var SelectedVal = $scope.SocietyRegistration.WorkScopeLevel_ID;
        $scope.GetSocietyClass();
        if (SelectedVal == '3' || SelectedVal == '4' || SelectedVal == '5' || SelectedVal == '6') {
            if ($scope.Blocks.length == 0) {
                $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
                    success(function(data, status) {
                        $scope.Blocks = data;
                    }).
                    error(function(data, status) {
                        $scope.Blocks = [];
                    });
            }
        }
        if (SelectedVal == '7' || SelectedVal == '8') {

        }
        if (SelectedVal == '1') {
            $scope.UrbanDivRadio = false;
            $scope.showtabbar = true;
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.TehsilDiv = false;
            $scope.GPDiv = false;
            $scope.DistrictDiv = false;
            $scope.DivisionDiv = false;
            $scope.StateDiv = true;
            $scope.ruralurbanb = false;
            $scope.Districtlevel3 = true;
            $scope.Districtlevel1 = false;
            $scope.Districtlevel2 = false;
            $scope.SocietyRegistration.GP_ID = '0';
            $scope.SocietyRegistration.VILLAGE_ID = '0';
            $scope.SocietyRegistration.BLOCK_ID = '0';
            $scope.SocietyRegistration.DISTRICT_ID = '0';
            $scope.District = [];
            $scope.SocietyRegistration.IsStatLevel == 'Yes'
            $scope.Addmore = false;
            $scope.Addmorebtn(true);
            $scope.SocietyRegistration.AreaOfOperation = "All RAJASTHAN";

        }
        else if (SelectedVal == '2') {
            $scope.showtabbar = true;
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.TehsilDiv = false;
            $scope.DistrictDiv = false;
            $scope.DivisionDiv = true;
            $scope.GPDiv = false;
            $scope.Addmore = true;
            $scope.ruralurbanb = false;
            $scope.Districtlevel3 = false;
            $scope.Districtlevel1 = false;
            $scope.Districtlevel2 = false;
            $scope.SocietyRegistration.GP_ID = '0';
            $scope.SocietyRegistration.VILLAGE_ID = '0';
            $scope.SocietyRegistration.BLOCK_ID = '0';
            $scope.SocietyRegistration.DISTRICT_ID = '0';
            $scope.Blocks = [];
            $scope.SocietyRegistration.IsStatLevel == 'No'
            $scope.SocietyRegistration.AreaOfOperation = "";
            $scope.UrbanDivRadio = false;
        }
        else if (SelectedVal == '3') {
            $scope.showtabbar = true;
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.BlockruralDiv = false;
            $scope.DistrictDiv = true;
            $scope.DivisionDiv = true;
            $scope.Addmore = true;
            $scope.ruralurbanb = true;
            $scope.GPDiv = false;
            $scope.Districtlevel3 = false;
            $scope.SocietyRegistration.GP_ID = '0';
            $scope.SocietyRegistration.VILLAGE_ID = '0';
            $scope.SocietyRegistration.BLOCK_ID = '0';
            $scope.Blocks = [];
            $scope.SocietyRegistration.IsStatLevel == 'No'
            $scope.SocietyRegistration.AreaOfOperation = "";
            $scope.UrbanDivRadio = false;

        }
        else if (SelectedVal == '4') {
            $scope.showtabbar = true;
            $scope.VillageDiv = false;
            $scope.PsDiv = true;
            $scope.GPDiv = false;
            $scope.DistrictDiv = true;
            $scope.DivisionDiv = true;
            // $scope.StateDiv = true;
            $scope.BlockruralDiv = true;
            $scope.Addmore = true;
            $scope.ruralurbanb = false;
            $scope.Districtlevel3 = true;
            $scope.Districtlevel1 = false;
            $scope.Districtlevel2 = false;
            $scope.SocietyRegistration.GP_ID = '0';
            $scope.SocietyRegistration.VILLAGE_ID = '0';
            $scope.Blocks = [];
            $scope.SocietyRegistration.IsStatLevel == 'No'
            $scope.SocietyRegistration.AreaOfOperation = "";
            $scope.UrbanDivRadio = false;

        }
        else if (SelectedVal == '5') {
            $scope.showtabbar = true;
            $scope.BlockruralDiv = true;
            $scope.VillageDiv = false;
            $scope.PsDiv = true;
            $scope.GPDiv = true;
            $scope.DistrictDiv = true;
            $scope.DivisionDiv = true;
            $scope.Addmore = true;
            $scope.SocietyRegistration.IsStatLevel == 'No'
            $scope.SocietyRegistration.AreaOfOperation = "";
            $scope.UrbanDivRadio = false;

        }
        else if (SelectedVal == '6') {
            $scope.showtabbar = true;
            $scope.VillageDiv = true;
            $scope.PsDiv = true;
            $scope.GPDiv = true;
            $scope.BlockruralDiv = true;
            $scope.DistrictDiv = true;
            $scope.DivisionDiv = true;
            // $scope.StateDiv = true;
            $scope.Addmore = true;
            $scope.ruralurbanb = false;
            $scope.Districtlevel3 = true;
            $scope.Districtlevel1 = false;
            $scope.Districtlevel2 = false;
            $scope.UrbanDivRadio = false;
            $scope.SocietyRegistration.AreaOfOperation = "";
        }
        else if (SelectedVal == '7' || SelectedVal == '8') {
            $scope.showtabbar = true;
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.GPDiv = false;
            $scope.BlockruralDiv = false;
            $scope.DistrictDiv = true;
            $scope.DivisionDiv = true;
            // $scope.StateDiv = true;
            $scope.Addmore = true;
            $scope.ruralurbanb = false;
            $scope.Districtlevel3 = true;
            $scope.Districtlevel1 = false;
            $scope.Districtlevel2 = false;
            $scope.UrbanDivRadio = true;
            $scope.SocietyRegistration.AreaOfOperation = "";
            if (SelectedVal == '8')
                $scope.DivRadioWard = true;
            else
                $scope.DivRadioWard = false;

        }
        else {
            $scope.StateDiv = false;
            $scope.DivisionDiv = false;
            $scope.DistrictDiv = false;
            $scope.TehsilDiv = false;
            $scope.PsDiv = false;
            $scope.VillageDiv = false;
            $scope.Addmore = false;
            $scope.ruralurbanb = false;
            $scope.Districtlevel1 = false;
            $scope.Districtlevel2 = false;
            $scope.CitytypDiv = false;
        }

    };
    //End Level of office data

    /*************/

    $scope.Addmorebtn = function(ValidationFlag) {
        if (ValidationFlag) {
            if (!$scope.FormValidationOthersocity(ValidationFlag)) {
                return false;
            }
        }
        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '1') {
            $scope.grid = false;
            $scope.lstSocietyWorkScope = [];
            $scope.lstSocietyWorkScope.push(
                {
                    "WorkScopeLevel_ID": 1,
                    "IsStatLevel": 2,
                    "STATE_ID": 6,
                    "DIVISION_ID": 0,
                    "DISTRICT_ID": 0,
                    "BLOCK_ID": 0,
                    "VILLAGE_ID": 0,
                    "CITY_ID": 0,
                    "WARD_ID": 0,
                    "GP_ID": 0,
                    "CityTypes": 0
                }
            );
        }
        else if ($scope.SocietyRegistration.WorkScopeLevel_ID > 1 && ValidationFlag) {

            if ($scope.lstSocietyWorkScope.length != 0 && (ValidationFlag == undefined || ValidationFlag == true)) {
                if (parseInt(($scope.lstSocietyWorkScope[0]).WorkScopeLevel_ID, 10) != (parseInt($scope.SocietyRegistration.WorkScopeLevel_ID, 10))) {
                    alert('data is not at same level hence can not be added.');
                    return;
                }
                if ($scope.SocietyRegistration.DIVISION_ID != 0 && $scope.SocietyRegistration.CITY_ID == 0) {
                    var _data = $scope.lstSocietyWorkScope.filter(function(x) {
                        return x.WorkScopeLevel_ID == parseInt($('#static').val())
                            && x.DIVISION_ID == parseInt($('#ddlDivision').val())
                            && x.DISTRICT_ID == parseInt($('#ddlDistrict').val() == null ? 0 : $('#ddlDistrict').val())
                            && x.BLOCK_ID == parseInt($('#ddlblocki').val() == null ? 0 : $('#ddlblocki').val())
                            && x.GP_ID == parseInt($('#ddlGP').val() == null ? 0 : $('#ddlGP').val())
                            && x.VILLAGE_ID == parseInt($('#ddlVillage').val() == null ? 0 : $('#ddlVillage').val())
                    });
                }
                else if ($scope.SocietyRegistration.CITY_ID != 0 && $scope.SocietyRegistration.WARD_ID == 0) {
                    var _data = $scope.lstSocietyWorkScope.filter(function(x) {
                        return
                        x.CITY_ID == parseInt($('#ddlCity').val())
                    }
                    );
                }
                else if ($scope.SocietyRegistration.CITY_ID != 0 && $scope.SocietyRegistration.WARD_ID != 0) {
                    var _data = $scope.lstSocietyWorkScope.filter(function(x) {
                        return
                        x.CITY_ID == parseInt($('#ddlCity').val())
                            && x.WARD_ID == parseInt($('#ddlward').val())
                    }
                    );
                }
                if (_data.length > 0) {
                    alert(_language == 'en-us' ? 'record already exist\n' : 'रिकॉर्ड पहले से मौजूद है \n');
                    return;
                }

            }

            if (($scope.SocietyRegistration.IsStatLevel == 'No' || $scope.SocietyRegistration.DIVISION_ID > '0') && ($scope.SocietyRegistration.WorkScopeLevel_ID != '0')) {
                $scope.grid = true;
                //IsSportsRural
                $scope.lstSocietyWorkScope.push(
                    {
                        "WorkScopeLevel_ID": parseInt($('#static option:selected').val()),
                        "STATE_ID": 6,
                        "IsStatLevel": 0,
                        "DIVISION_ID": $('#ddlDivision').val() == null ? 0 : $('#ddlDivision').val(),
                        "DISTRICT_ID": $('#ddlDistrict').val() == null ? 0 : $('#ddlDistrict').val(),
                        "BLOCK_ID": $('#ddlblocki').val() == null ? 0 : $('#ddlblocki').val(),
                        "VILLAGE_ID": $('#ddlVillage').val() == null ? 0 : $('#ddlVillage').val(),
                        "CITY_ID": $('#ddlCity').val() == null ? 0 : $('#ddlCity').val(),
                        "WARD_ID": $('#ddlward').val() == null ? 0 : $('#ddlward').val(),
                        "GP_ID": $('#ddlGP').val() == null ? 0 : $('#ddlGP').val(),
                        "IsRuralUrban": $('#ISrurl').prop('checked') == true ? 1 : 0,


                        "WorkScopeLevel_Name": parseInt($('#static option:selected').val()) > 0 ? $('#static option:selected').text() : 'N/A',
                        "STATE_Name": 'Rajasthan' ? 'Rajasthan' : 'N/A',
                        "DIVISION_Name": parseInt($('#ddlDivision option:selected').val()) > 0 ? $('#ddlDivision option:selected').text() : 'N/A',
                        "DISTRICT_ENG": parseInt($('#ddlDistrict option:selected').val()) > 0 ? $('#ddlDistrict option:selected').text() : 'N/A',
                        "BLOCK_Name": parseInt($('#ddlblocki option:selected').val()) > 0 ? $('#ddlblocki option:selected').text() : 'N/A',
                        "VILLAGE_Name": parseInt($('#ddlVillage option:selected').val()) > 0 ? $('#ddlVillage option:selected').text() : 'N/A',
                        "CityTypes_Name": parseInt($('#staticall option:selected').val()) > 0 ? $('#staticall option:selected').text() : 'N/A',
                        "CITY_Name": parseInt($('#ddlCity option:selected').val()) > 0 ? $('#ddlCity option:selected').text() : 'N/A',
                        "WARD_Name": parseInt($('#ddlward option:selected').val()) > 0 ? $('#ddlward option:selected').text() : 'N/A',
                        "GP_ID_Name": parseInt($('#ddlGP option:selected').val()) > 0 ? $('#ddlGP option:selected').text() : 'N/A',
                    }
                );
            }
            else if ($scope.SocietyRegistration.IsStatLevel == 'Yes') {
                $scope.grid = false;
                $scope.lstSocietyWorkScope = [];
                $scope.lstSocietyWorkScope.push(
                    {
                        "WorkScopeLevel_ID": 1,
                        "IsStatLevel": 1,
                        "STATE_ID": 6,
                        "DIVISION_ID": 0,
                        "DISTRICT_ID": 0,
                        "BLOCK_ID": 0,
                        "VILLAGE_ID": 0,
                        "CITY_ID": 0,
                        "WARD_ID": 0,
                        "GP_ID": 0,
                        "CityTypes": 0
                    }
                );
            }
            else if ($scope.SocietyRegistration.IsStatLevel == 'No' && ValidationFlag == undefined) {
                // alert("Please Choose level of work scope");
            }


            $scope.SocietyRegistration.WorkScopeLevel = '0';
            $scope.SocietyRegistration.STATE_ID = '0';
            $scope.SocietyRegistration.DIVISION_ID = '0';
            $scope.SocietyRegistration.DISTRICT_ID = '0';
            $scope.SocietyRegistration.TEHSIL_ID = '0';
            $scope.SocietyRegistration.BLOCK_ID = '0';
            $scope.SocietyRegistration.VILLAGE_ID = '0';
            //$scope.SocietyRegistration.Rural = null;
            //$scope.SocietyRegistration.urban = null;
            $scope.SocietyRegistration.CityTypes = '0';
            $scope.SocietyRegistration.CITY_ID = '0';
            $scope.SocietyRegistration.WARD_ID = '0';

            //$scope.SocietyRegistration.No = null;
            //$scope.SocietyRegistration.Yes = null;

            $scope.SocietyRegistration.GP_ID = '0';
            //$scope.SocietyRegistration.WorkScopeLevel_ID = '0';
        }

    }

    $scope.GetActList = function() {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
            success(function(data, status) {
                $scope.ActList = data;
                $scope.SocietyRegistration.ActId = "1";
            }).
            error(function(data, status) {

            });
    }
    $scope.GetActList();


    $scope.BrnDetails = function() {

        // $("#txtBrn").attr("disabled", "disabled");
        $scope.CheckBRN = false;
        $scope.nepster = true;
        $scope.showtabbar = true;
        $scope.IsSocietyNamedisabled = true;
        $scope.divOtherSocietys = true;
        if ($scope.SocietyRegistration.IsNewRegistration != null && $scope.SocietyRegistration.IsNewRegistration != undefined) {
            $scope.DivAdministrative = true;
            $scope.DivAddressDetails = true;
        }

        if ($scope.SocietyRegistration.BRN == undefined || $scope.SocietyRegistration.BRN == '') {
            ShowOkMessage("", "Please enter Valid BRN Number", function() { });
        } else {
            $(".se-pre-con").fadeIn("slow");
            var formData = new FormData();
            formData.append("BRN", $scope.SocietyRegistration.BRN);
            formData.append("SocietyIdENC", $scope.SocietyIdEnc);
            formData.append("IsNewReg", false);
            $http.post(SiteUrl + "/Society/GetBrnDetails", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function(data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data.result == 'success') {
                        $("#txtBrn").attr("disabled", "disabled");
                        //$("#txtBrn").removeAttr("disabled");
                        $("#lbltemp").html(data.BRN);
                        $scope.CheckBRN = true;
                        $scope.SocietyRegistration.SocietyName = data.BO_Name;
                        $scope.nepster = true;
                        $scope.showtabbar = true;
                        // $scope.ClickUserVarification();
                        $scope.IsSocietyNamedisabled = true;
                        $scope.divOtherSocietys = true;
                        if ($scope.SocietyRegistration.IsNewRegistration != null && $scope.SocietyRegistration.IsNewRegistration != undefined) {
                            $scope.DivAdministrative = true;
                            $scope.DivAddressDetails = true;

                        }
                    }
                    else if (data.result == 'failed') {
                        $scope.showrulesfile = false;
                        $scope.ShowSubmitBtn = false;
                        $(".se-pre-con").fadeOut("slow");
                        ShowModelAlert("Society Profiling", data.BRN);
                        $scope.DivAdministrative = false;
                        $scope.DivAddressDetails = false;
                    }
                    else if (data.result == 'invalidname') {
                        $(".se-pre-con").fadeOut("slow");
                        ShowModelAlert("Society Profiling", "Invalid Society Name: " + data.BO_Name);
                        $scope.CheckBRN = false;
                        $scope.DivAdministrative = false;
                        $scope.DivAddressDetails = false;
                    }

                }).
                error(function(data, status) {
                    $scope.CheckBRN = false;
                    $scope.nepster = true;
                    $scope.showtabbar = true;
                    $(".se-pre-con").fadeOut("slow");
                    $scope.DivAdministrative = false;
                    $scope.DivAddressDetails = false;
                    ShowModelAlert("Society Profiling", "Error While Fetching BRN Details");
                });
        }



    };
    $scope.lstSocietyWorkScope = [];
    $scope.FormValidation = function() {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';



        if ($scope.SocietyRegistration.ActId == undefined || $scope.SocietyRegistration.ActId == "0") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Act' : '\n <br /> कृपया अधिनियम का चयन करे |';
        }
        if (($scope.SocietyRegistration.BRN == undefined || $scope.SocietyRegistration.BRN == '') && $scope.SocietyRegistration.ActId != "7" && $('#EUserType').val() != "D") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter BRN No.' : '\n <br /> कृपया बीआरएन नंबर दर्ज करें |';
        }
        if (($scope.SocietyRegistration.AreaOfOperation == undefined || $scope.SocietyRegistration.AreaOfOperation == '' || $scope.SocietyRegistration.AreaOfOperation == 'undefined') && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Area of Operation as per by Laws' : '\n <br /> कृपया by Laws के अनुसार कार्य क्षेत्र का विवरण दर्ज करें |';
        }
        if ($scope.SocietyRegistration.IsNewRegistration == undefined && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please select The Type Of Application.' : '\n <br /> कृपया आवेदन के प्रकार का चयन करे |';
        }
        if ($scope.SocietyRegistration.IsSportsNewRegistration == undefined && $scope.SocietyRegistration.ActId == "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please select The Type Of Application.' : '\n <br /> कृपया आवेदन के प्रकार का चयन करे |';
        }
        if (($scope.SocietyRegistration.RegistrationNo == undefined || $scope.SocietyRegistration.RegistrationNo == '') && $scope.SocietyRegistration.ActId != "7" && ($scope.SocietyRegistration.IsNewRegistration == false || $scope.SocietyRegistration.IsNewRegistration == 'false')) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society Registration No.' : '\n <br /> सोसायटी पंजीकरण संख्या दर्ज करें |';
        }
        if ($scope.SocietyRegistration.SocietyName == undefined && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society Name' : '\n <br /> कृपया सोसाइटी का नाम दर्ज करें |';
        }
        if ($('#txtRegistrationDate').val().length == 0 && $scope.SocietyRegistration.ActId != "7" && ($scope.SocietyRegistration.IsNewRegistration == false || $scope.SocietyRegistration.IsNewRegistration == 'false')) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Registration Date' : '\n <br /> कृपया पंजीकरण की तारीख का चयन करे |';
        }
        if ($('#txtElectionDate').val().length == 0 && $scope.SocietyRegistration.ActId != "7" && ($scope.SocietyRegistration.IsNewRegistration == false || $scope.SocietyRegistration.IsNewRegistration == 'false')) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Election Date' : '\n <br /> कृपया चुनाव तिथि का चयन करें |';
        }
        if ($scope.SocietyRegistration.SocietyTypeId == 0 && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Type' : '\n <br /> कृपया सोसाइटी का प्रकार चुनें |';
        }
        if ($scope.SocietyRegistration.ClassId == 0 && $scope.SocietyRegistration.ActId == 3 && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Class' : '\n <br /> कृपया सोसाइटी क्लास का चयन करे  |';
        }
        if ($scope.SocietyRegistration.SubClassId == 0 && $scope.SocietyRegistration.ActId == 3) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Sub Society Class' : '\n <br /> कृपया उप सोसाइटी क्लास का चयन करे |';
        }
        if ($scope.SocietyRegistration.SocietyClassId == 0 && $scope.SocietyCategoryList.Count > 0 && $scope.SocietyRegistration.ActId == 3) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Category' : '\n <br /> कृपया सोसायटी श्रेणी का चयन करें |';
        }

        if ($scope.lstSocietyWorkScope.length == '0') {
            if ($scope.SocietyRegistration.WorkScopeLevel_ID == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Level of Society Work Scope.' : '\n <br /> कृपया सोसाइटी कार्य स्कोप स्तर चुनें |';
            }
            if ($scope.lstSocietyWorkScope.length == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Level of Society Work Scope.' : '\n <br /> कृपया सोसाइटी कार्य स्कोप स्तर चुनें |';
            }
        }

        if (($scope.SocietyRegistration.NoOfBranches == null || $scope.SocietyRegistration.NoOfBranches == undefined || $scope.SocietyRegistration.NoOfBranches == '') && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter No of Branches.' : '\n <br /> शाखाओं में से कोई भी दर्ज करें |';
        }

        if ($scope.SocietyRegistration.IsStatLevel == 'No' && $scope.SocietyRegistration.ActId != "7") {
            if ($scope.SocietyRegistration.Rural == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Choose the Rural/Urban.' : '\n <br /> कृपया ग्रामीण / शहरी चुनें|';
            }
        }

        //if ($scope.SocietyRegistration.IsNationalLevel == undefined && $scope.SocietyRegistration.IsStatLevel == undefined && $scope.SocietyRegistration.ActId == "1") {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please Choose Area of operations ?' : '\n <br /> कृपया कार्यक्षेत्र का स्तर चुनें?';

        //}
        if (($scope.SocietyRegistration.WorkScopeLevel_ID == undefined || $scope.SocietyRegistration.WorkScopeLevel_ID == '0') && $scope.SocietyRegistration.ActId != "7" && $scope.lstSocietyWorkScope.length < 1) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Choose Area of operations ?' : '\n <br /> कृपया कार्यक्षेत्र का स्तर चुनें?';
        }

        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '7' && $scope.lstSocietyWorkScope.length < 1) {
            if ($scope.SocietyRegistration.CITY_ID == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the City .' : '\n <br /> कृपया शहर का चयन करें |';
            }
            if ($scope.SocietyRegistration.WARD_ID == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the ward .' : '\n <br /> कृपया वार्ड का चयन करें |';
            }
        }

        //if ($scope.SocietyRegistration.Rural == 'true' && $scope.lstSocietyWorkScope.length < 1) {
        //    if ($scope.SocietyRegistration.WorkScopeLevel_ID == '0') {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Choose Level of work scope.' : '\n <br /> कृपया कार्यक्षेत्र का स्तर चुनें |';
        //    }
        //}

        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '2' && $scope.lstSocietyWorkScope.length == '0') {
            if ($scope.SocietyRegistration.DIVISION_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Division Level.' : '\n <br /> कृपया डिवीजन स्तर का चयन करें |';
        }
        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '3' && $scope.lstSocietyWorkScope.length == '0') {
            if ($scope.SocietyRegistration.DIVISION_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Division level.' : '\n <br /> कृपया डिवीजन स्तर का चयन करें |';

            if ($scope.SocietyRegistration.DISTRICT_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the District level.' : '\n <br /> जिला स्तर का चयन करें |';
        }
        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '4' && $scope.lstSocietyWorkScope.length == '0') {

            if ($scope.SocietyRegistration.DIVISION_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Division level.' : '\n <br /> कृपया डिवीजन स्तर का चयन करें |';


            if ($scope.SocietyRegistration.DISTRICT_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the District level.' : '\n <br /> जिला स्तर का चयन करें |';


            if ($scope.SocietyRegistration.BLOCK_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Block level.' : '\n <br /> कृपया ब्लॉक स्तर का चयन करें|';

        }
        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '5' && $scope.lstSocietyWorkScope.length == '0') {

            if ($scope.SocietyRegistration.DIVISION_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Division level.' : '\n <br /> कृपया डिवीजन स्तर का चयन करें |';


            if ($scope.SocietyRegistration.DISTRICT_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the District level.' : '\n <br /> जिला स्तर का चयन करें |';



            if ($scope.SocietyRegistration.BLOCK_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Block level.' : '\n <br /> कृपया ब्लॉक स्तर का चयन करें |';


            if ($scope.SocietyRegistration.GP_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Gram panchayat level.' : '\n <br /> कृपया ग्राम पंचायत स्तर का चयन करें |';

        }
        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '6' && $scope.lstSocietyWorkScope.length == '0') {

            if ($scope.SocietyRegistration.DIVISION_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Division level.' : '\n <br /> कृपया डिवीजन स्तर का चयन करें |';


            if ($scope.SocietyRegistration.DISTRICT_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the District level.' : '\n <br /> जिला स्तर का चयन करें |';


            if ($scope.SocietyRegistration.BLOCK_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Block level.' : '\n <br /> कृपया ब्लॉक स्तर का चयन करें |';


            if ($scope.SocietyRegistration.GP_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Gram panchayat level.' : '\n <br /> कृपया ग्राम पंचायत स्तर का चयन करें |';


            if ($scope.SocietyRegistration.VILLAGE_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Village level.' : '\n <br /> कृपया गांव स्तर का चयन करें |';

        }
        if (($scope.SocietyRegistration.SocietyCategoryId == undefined || $scope.SocietyRegistration.SocietyCategoryId == 0) && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Category' : '\n <br /> कृपया सोसाइटी श्रेणी का चयन करें |';
        }

        if (($scope.SocietyRegistration.MajorActivityId == 0 || $scope.SocietyRegistration.MajorActivityId == undefined) && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Major Activity' : '\n <br /> कृपया प्रमुख गतिविधि का चयन करें |';
        }

        if (($scope.SocietyRegistration.ActivityId == 0 || $scope.SocietyRegistration.ActivityId == undefined) && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Activity' : '\n <br /> कृपया सोसाइटी गतिविधि का चयन करें |';
        }

        if (($scope.SocietyRegistration.BusinessId == 0 || $scope.SocietyRegistration.BusinessId == undefined) && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Business' : '\n <br /> कृपया सोसाइटी व्यापार का चयन करें |';
        }

        if (($scope.SocietyRegistration.LiablityId == 0 || $scope.SocietyRegistration.LiablityId == undefined) && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Liability Type' : '\n <br /> कृपया सोसाइटी दायित्व के प्रकार का चयन करें |';
        }

        if ($scope.SocietyRegistration.LiablityId == 1 && ($scope.SocietyRegistration.MemberLiablity == undefined || $scope.SocietyRegistration.MemberLiablity == '') && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Liability of a Member LTD Society' : '\n <br /> कृपया लिमिटेड सोसायटी के सदस्य दायित्व की संख्या करें |';
        }

        if (($scope.SocietyRegistration.TotalNumberOfMembers == undefined || $scope.SocietyRegistration.TotalNumberOfMembers == '') && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Total Number of Members Applying for the Society' : '\n <br /> कृपया सोसाइटी के लिए आवेदन करने वाले सदस्यों की कुल संख्या दर्ज करें |';
        }

        if ($('#txtEntranceFees').val() == "" && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Entrance Fees Collected' : '\n <br /> कृपया आवेदन करने वाले सदस्यों की प्रवेश शुल्क दर्ज करें|';
        }

        if (($scope.SocietyRegistration.SingleShareValue == undefined || $scope.SocietyRegistration.SingleShareValue == '') && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Value of One Share' : '\n <br /> कृपया कृपया एक शेयर का मूल्य दर्ज करें|';
        }

        if (($scope.SocietyRegistration.PaidUpShareCapital == undefined || $scope.SocietyRegistration.PaidUpShareCapital == '') && $scope.SocietyRegistration.ActId == "3") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Paid Up Share Capital' : '\n <br /> कृपया शेयर पूंजी का भुगतान दर्ज करें|';
        }

        if ($('#txtAGMDate').val().length == 0 && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select AGM Date' : '\n <br /> कृपया एजीएम की तारीख का चयन करे |';
        }

        if (($scope.SocietyRegistration.NoOfMemberInAGM == undefined || $scope.SocietyRegistration.NoOfMemberInAGM == '') && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Entrance Fees Collected' : '\n <br /> कृपया आवेदन करने वाले सदस्यों की प्रवेश शुल्क दर्ज करें|';
        }

      
        if ($scope.GetFileUploaded("RegistrationCertificate") == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Registration Certificate(PDF)' : '\n <br /> कृपया पंजीकरण प्रमाण पत्र अपलोड करें (पीडीएफ) |';
        }

        if ($scope.GetFileUploaded("ByeLawsSocietyFile") == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Rules And Regulation(PDF)' : '\n <br /> कृपया नियम और अधिनियम अपलोड करें (पीडीएफ) |';
        }
        
        if ($scope.GetFileUploaded("OldExecutiveCommittee") == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Old Executive Committee' : '\n <br /> कृपया पुरानी कार्यकारी समिति को अपलोड करें |';
        }
        if ($scope.GetFileUploaded("OldExecutiveCommittee") == '') {           
                message += $scope._language == 'en-us' ? '\n <br /> Please Upload Members List(Excel)' : '\n <br /> कृपया सदस्य सूची(एक्सेल) अपलोड करें |';    
        }    
        if ($scope.GetFileUploaded("SocietyObjects")=='') {    
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Objects(PDF)' : '\n <br /> कृपया उद्देश्य अपलोड करें (पीडीएफ) |';
            message += '<br />';
        }


        if (message.length > 29 && $scope.SubmitStatus == 1) {
            ShowModelAlert("Society Profiling", message);
            return false;
        }
        if ($scope.SubmitStatus == 0) {

            return false;
        }
        return true;
    }
    $scope.FormValidationPartial = function() {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.SocietyRegistration.ActId == undefined || $scope.SocietyRegistration.ActId == "0") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Act' : '\n <br /> कृपया अधिनियम का चयन करे |';
        }
        if (($scope.SocietyRegistration.RegistrationNo == undefined || $scope.SocietyRegistration.RegistrationNo == '') && $scope.SocietyRegistration.ActId != "7" && ($scope.SocietyRegistration.IsNewRegistration == false || $scope.SocietyRegistration.IsNewRegistration == 'false')) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society Registration No.' : '\n <br /> सोसायटी पंजीकरण संख्या दर्ज करें |';
        }
        if (($scope.SocietyRegistration.SocietyName == undefined || $scope.SocietyRegistration.SocietyName == '') && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society Name' : '\n <br /> कृपया सोसाइटी का नाम दर्ज करें |';
        }
        if ($scope.SocietyRegistration.SocietyTypeId == 0 && $scope.SocietyRegistration.ActId != "7") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Type' : '\n <br /> कृपया सोसाइटी का प्रकार चुनें |';
        }
        if ($scope.SocietyRegistration.BRN == undefined || $scope.SocietyRegistration.BRN == '' || $scope.CheckBRN == undefined || $scope.CheckBRN == false) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter BRN No. and Validate' : '\n <br /> कृपया बीआरएन नंबर दर्ज कर सत्यापित करें |';
        }
        if ($('#txtRegistrationDate').val().length == 0 && $scope.SocietyRegistration.ActId != "7" && ($scope.SocietyRegistration.IsNewRegistration == false || $scope.SocietyRegistration.IsNewRegistration == 'false')) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Registration Date' : '\n <br /> कृपया पंजीकरण की तारीख का चयन करे |';
        }


        if (message.length > 29) {
            ShowModelAlert("Society Profiling", message);
            return false;
        }
        if ($scope.SubmitStatus == 1)
            return false;
        else
            return true;
    }

    //START AdministrativeFormValidation  
    $scope.AdministrativeFormValidationProfilining = function(SubmitStatus) {
        // return true;
        var validate = true;
        var Count = 0;

        var message = $scope._language == 'en-us' ? ' Validate  following : ' : 'निम्नलिखित मान्य करें :';
       
            if ($scope.SocietyRegistration.IsAdministrative == undefined && $scope.SocietyRegistration.ActId != "1" && SubmitStatus == 1) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Administrative Type' : '\n <br /> प्रशासनिक प्रकार का चयन करे |';
            }

            //END
        
                var Count = 0;
           

                $scope.SocietyExecutiveCommitteeList.forEach(function(item, index) {
                    if (item.Aadh == undefined || item.Aadh == "") {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Aadhar number and validate' : '\n <br /> कृपया आधार नंबर दर्ज करें और मान्य करें |';
                        $("#prmember_" + index).css('border', 'solid 1px red');
                    }
                    if (item.ContactNo == undefined || item.ContactNo == "") {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Contact number' : '\n <br /> कृपया संपर्क नंबर दर्ज करें |';
                        $("#prmember_" + index).css('border', 'solid 1px red');
                    }
                    if (item.CategoryId == undefined || item.CategoryId == "") {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select Category of member' : '\n <br /> कृपया सदस्य की श्रेणी चुनें |';
                        $("#prmember_" + index).css('border', 'solid 1px red');
                    }
                    if (item.CategoryId == undefined || item.CategoryId == "") {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select Category of member' : '\n <br /> कृपया सदस्य की श्रेणी चुनें |';
                        $("#prmember_" + index).css('border', 'solid 1px red');
                    }
                    if (item.POSTNAME == undefined || item.POSTNAME == "") {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select post of member' : '\n <br /> कृपया सदस्य का पद चुनें |';
                        $("#prmember_" + index).css('border', 'solid 1px red');
                    }
                    if (item.POSTNAME == undefined || item.POSTNAME == "") {
                        message += $scope._language == 'en-us' ? '\n <br /> Please Select post of member' : '\n <br /> कृपया सदस्य का पद चुनें |';
                        $("#prmember_" + index).css('border', 'solid 1px red');
                    }
                });
              
                //validate = $scope.ValidateNominatedMember(0, true);
                //validate = $scope.ValidateElectedMember(3, true);
              //if ($scope.SocietyRegistration.ActId == "1" && Count < 1 && SubmitStatus == 1) {
              //      //var r = confirm($scope._language == 'en-us' ? 'Minimum 5 Members should be in the executive / governing board, do you want to proceed further?' : 'न्यूनतम 5 पदाधिकारी /कार्यकारिणी /संचालक मंडल में होने चाहिए क्या आप आगे प्रोसेस करना चाहते है ?');
              //      //if (r == false) {
              //      //   return false;
              //      //}					
              //      alert($scope._language == 'en-us' ? 'Minimum 1 Members should be in the executive / governing board, do you want to proceed further?' : 'न्यूनतम 1 पदाधिकारी /कार्यकारिणी /संचालक मंडल में होने चाहिए क्या आप आगे प्रोसेस करना चाहते है ?');
              //      return false;
              //  }
                        

      
        //End Sport Association

        if (message.length > 24 || validate == false) {
            if (message.length > 24) {
                ShowModelAlert("Administrative Form Validation", message);
            }
            else {
                ShowModelAlert("Administrative Form Validation", $scope._language == 'en-us' ? 'Validate the value of red border Area' : 'लाल सीमा के मूल्य को मान्य करें');
            }
            $(".se-pre-con").fadeOut("slow");
            return false;
        }
        return true;
    }
    //validateAddForm for farmer ends
      

    $scope.SocietyOfficeFormValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
       
        $scope.MemberAddress = GetAddressinfo('MemberAdd');

        if ($scope.MemberAddress.HouseBuildingNo == undefined || $scope.MemberAddress.HouseBuildingNo == '' || $scope.MemberAddress.HouseBuildingNo == ' ' || $scope.MemberAddress.HouseBuildingNo == null) {
            message += _language == 'en-us' ? '<br />Please enter the house/Building Number\n' : '<br />कृपया घर/भवन संख्या दर्ज करें।\n';
            $("#HouseBuildingNo_MemberAdd").addClass("validdata");
        }
        if ($scope.MemberAddress.LaneStreetName == undefined || $scope.MemberAddress.LaneStreetName == '' || $scope.MemberAddress.LaneStreetName == ' ' || $scope.MemberAddress.LaneStreetName == null) {
            message += _language == 'en-us' ? '<br />Please enter LaneStreetName\n' : '<br />कृपया पथ/गली नाम दर्ज करें।\n';
            $("#LaneStreetName_MemberAdd").addClass("validdata");
        }
        if ($scope.MemberAddress.Locality == undefined || $scope.MemberAddress.Locality == '' || $scope.MemberAddress.Locality == ' ' || $scope.MemberAddress.Locality == null) {
            message += _language == 'en-us' ? '<br />Please enter Locality \n' : '<br />कृपया स्थानीयता दर्ज करें।\n';
            $("#Locality_MemberAdd").addClass("validdata");
        }
        if ($scope.MemberAddress.Stateid == undefined || $scope.MemberAddress.Stateid == '' || $scope.MemberAddress.Stateid == null) {
            message += _language == 'en-us' ? '<br />Please select the State\n' : '<br />कृपया राज्य चुनें।\n';
            $("#Stateid_MemberAdd").addClass("validdata");
        }
        if ($scope.MemberAddress.DivisionId == undefined || $scope.MemberAddress.DivisionId == '0' || $scope.MemberAddress.DivisionId == '' || $scope.MemberAddress.DivisionId == null) {
            message += _language == 'en-us' ? '<br />Please select the Division\n' : '<br />कृपया संभाग का चयन करें।\n';
            $("#DivisionId_MemberAdd").addClass("validdata");
        }
        if ($scope.MemberAddress.DistrictId == undefined || $scope.MemberAddress.DistrictId == '0' || $scope.MemberAddress.DistrictId == '' || $scope.MemberAddress.DistrictId == null) {
            message += _language == 'en-us' ? '<br />Please select the District\n' : '<br />कृपया जिले का चयन करें।\n';
            $("#DistrictId_MemberAdd").addClass("validdata");
        }
        if ($scope.MemberAddress.Pincode == undefined || $scope.MemberAddress.Pincode == '' || $scope.MemberAddress.Pincode == ' ' || $scope.MemberAddress.Pincode == null) {
            message += _language == 'en-us' ? '<br />Please enter Pincode\n' : '<br />कृपया पिन कोड दर्ज करें।\n';
            $("#pincode_MemberAdd").addClass("validdata");
        }

        if ($scope.MemberAddress.Urban_Rural == "" || $scope.MemberAddress.Urban_Rural == "0" || $scope.MemberAddress.Urban_Rural == undefined) {
            message += _language == 'en-us' ? 'Please select the Urban/Rural\n' : '<br /> कृपया शहरी/ग्रामीण का चयन करें\n';
            $("#Urban_Rural_MemberAdd").addClass("validdata");
        } else {
            if ($scope.MemberAddress.Urban_Rural == "Urban") {
                if ($scope.MemberAddress.CityId == undefined || $scope.MemberAddress.CityId == '0' || $scope.MemberAddress.CityId == '' || $scope.MemberAddress.CityId == null) {
                    message += _language == 'en-us' ? '<br />Please select the City\n' : '<br />कृपया शहर का चयन करें।\n';
                    $("#ddlcity_MemberAdd").addClass("validdata");
                }
                if ($scope.MemberAddress.WardId == undefined || $scope.MemberAddress.WardId == '0' || $scope.MemberAddress.WardId == null || $scope.MemberAddress.WardId == '? string: ?') {
                    message += _language == 'en-us' ? '<br />Please select the Ward\n' : '<br />कृपया वार्ड का चयन करें।\n';
                    $("#ddlward_MemberAdd").addClass("validdata");
                }
            }
            if ($scope.MemberAddress.Urban_Rural == "Rurals") {
                if ($scope.MemberAddress.BlockId == undefined || $scope.MemberAddress.BlockId == '0' || $scope.MemberAddress.BlockId == '' || $scope.MemberAddress.BlockId == null) {
                    message += _language == 'en-us' ? '<br />Please select the Block\n' : '<br />कृपया ब्लॉक का चयन करें।\n';
                    $("#ddlblocki_MemberAdd").addClass("validdata");
                }
                if ($scope.MemberAddress.PanchayatId == undefined || $scope.MemberAddress.PanchayatId == '0' || $scope.MemberAddress.PanchayatId == null || $scope.MemberAddress.PanchayatId == '? string: ?') {
                    message += _language == 'en-us' ? '<br />Please select the Gram Panchayat\n' : '<br />कृपया ग्राम पंचायत का चयन करें।\n';
                    $("#ddlGP_MemberAdd").addClass("validdata");
                }
                if ($scope.MemberAddress.VillageId == undefined || $scope.MemberAddress.VillageId == '0' || $scope.MemberAddress.VillageId == null || $scope.MemberAddress.VillageId == '? string: ?') {
                    message += _language == 'en-us' ? '<br />Please select the Village\n' : '<br />कृपया गाँव का चयन करें।\n';
                    $("#ddlVillage_MemberAdd").addClass("validdata");
                }
            }
        }

        if (message.length > 29 && $scope.SubmitStatus == 1) {
            var _Temp = (_language == 'en-us' ? 'Society Office Details' : 'सोसायटी कार्यालय विवरण');
            ShowModelAlert(_Temp, message);
            return false;
        }
        return true;
    }


    $scope.SaveSocietyProfiling = function(SubmitStatus) {
        $scope.SubmitStatus = SubmitStatus;
        $(".se-pre-con").fadeIn("slow");
        $(".SaveSocietyInfo").hide();
        if ($scope.FormValidation() || $scope.FormValidationPartial()) {
            //ADMINISTRATIVE VALIDATION

            if ($scope.AdministrativeFormValidationProfilining(SubmitStatus) == false) {
                $(".se-pre-con").fadeOut("slow");
                $(".SaveSocietyInfo").show();
                return;
            }



            //END
            //ADDRESS DETAILS

            if ($scope.SocietyOfficeFormValidation() == false) {

                $(".se-pre-con").fadeOut("slow");
                $(".SaveSocietyInfo").show();
                return;
            }


            //END
            //

            var formData = new FormData();
            $(".se-pre-con").fadeIn("slow");

            formData.append("BRN", $scope.SocietyRegistration.BRN);
            formData.append("SocietyIDEnc", $scope.SocietyRegistration.SocietyIdEnc);
            formData.append("ActId", $scope.SocietyRegistration.ActId);
            formData.append("OldRegistrationNo", $scope.OldRegistrationNo);
            formData.append("RegistrationNo", $scope.SocietyRegistration.RegistrationNo);
            formData.append("SocietyName", $scope.SocietyRegistration.SocietyName);
            formData.append("SocietyTypeId", $scope.SocietyRegistration.SocietyTypeId);
            formData.append("ClassId", $scope.SocietyRegistration.ClassId);
            formData.append("SubClassId", $scope.SocietyRegistration.SubClassId);
           
            formData.append("RegistrationDate", $('#txtRegistrationDate').val().split("/").reverse().join("-"));
            formData.append("ElectionDate", $('#txtElectionDate').val().split("/").reverse().join("-"));
            formData.append("NoOfBranches", $scope.SocietyRegistration.NoOfBranches);          
            formData.append("LOSocietyOfficeDetails", $scope.SocietyRegistration.WorkScopeLevel);
            formData.append("STATE_ID", $scope.SocietyRegistration.STATE_ID);
            formData.append("DIVISION_ID", $scope.SocietyRegistration.DIVISION_ID);
            formData.append("DISTRICT_ID", $scope.SocietyRegistration.DISTRICT_ID);
            formData.append("TEHSIL_ID", $scope.SocietyRegistration.TEHSIL_ID);
            formData.append("BLOCK_ID", $scope.SocietyRegistration.BLOCK_ID);
            formData.append("VILLAGE_ID", $scope.SocietyRegistration.VILLAGE_ID);
           
            formData.append("lstSocietyWorkScope", JSON.stringify($scope.lstSocietyWorkScope));
            formData.append("IsNewRegistration", false);

            //NEW 05/12/2018
            formData.append("BusinessId", $scope.SocietyRegistration.BusinessId);
            formData.append("LiablityId", $scope.SocietyRegistration.LiablityId);
            formData.append("MemberLiablity", $scope.SocietyRegistration.MemberLiablity);
            formData.append("TotalMembersApplyingForSocociety", $scope.SocietyRegistration.TotalNumberOfMembers);
            formData.append("AgmDate", $('#txtAGMDate').val().split("/").reverse().join("-"));
            formData.append("TotalNumberOfMembersInAGM", $scope.SocietyRegistration.NoOfMemberInAGM);
            formData.append("EntranceFeesCollected", $('#txtEntranceFees').val());
            formData.append("SingleShareValue", $scope.SocietyRegistration.SingleShareValue);
            formData.append("PaidUpShareCapital", $scope.SocietyRegistration.PaidUpShareCapital);
            //formData.append("MemberListExcelFile", $scope.MembersListExcelFile);           
            
            formData.append("AreaOfOperation", $scope.SocietyRegistration.AreaOfOperation);
                    //ADMINISTRATION DETAILS  31032020
            formData.append("ClassA", $scope.SocietyRegistration.TotalMembersClassA);
            formData.append("ClassB", $scope.SocietyRegistration.TotalMembersClassB);
            formData.append("ClassC", $scope.SocietyRegistration.TotalMembersClassC);
            formData.append("SocietyDirectory", $scope.SocietyDirectory);
           
            formData.append("BoardType", $scope.SocietyRegistration.IsAdministrative);
            //END    


            var RegistrationCertificate1 = $scope.GetFileUploaded("RegistrationCertificate");
            var ByeLawsSocietyFile = $scope.GetFileUploaded("ByeLawsSocietyFile");
            var OldExecutiveCommittee = $scope.GetFileUploaded("OldExecutiveCommittee");
            var MemberListExcelFile = $scope.GetFileUploaded("MemberListExcelFile");
            var SocietyObjects = $scope.GetFileUploaded("SocietyObjects");
            RegistrationCertificate1 = RegistrationCertificate1.split('/')[RegistrationCertificate1.split('/').length - 1];
            ByeLawsSocietyFile = ByeLawsSocietyFile.split('/')[ByeLawsSocietyFile.split('/').length - 1];
            OldExecutiveCommittee = OldExecutiveCommittee.split('/')[OldExecutiveCommittee.split('/').length - 1];
            MemberListExcelFile = MemberListExcelFile.split('/')[MemberListExcelFile.split('/').length - 1];
            SocietyObjects = SocietyObjects.split('/')[SocietyObjects.split('/').length - 1];

            formData.append("RegistrationCertificateFile", RegistrationCertificate1);
            formData.append("ByeLawsSocietyFile", ByeLawsSocietyFile);
            formData.append("OldExecutiveCommittee", OldExecutiveCommittee);
            formData.append("MemberListExcelFile", MemberListExcelFile);
            formData.append("RulesAndRegulationFile", ByeLawsSocietyFile);
            formData.append("ObjectsFileName", SocietyObjects);
          
           
            formData.append("SocietyDirectory", $scope.SocietyDirectory);
            var SocietyRegistrationData = [];
            $scope.SocietyIdForAdministrativeDetails = 0;

            $scope.SocietyExecutiveCommitteeList.forEach(function(item, index) {
                SocietyRegistrationData.push(
                    {
                        "AADHAARNo": item.Aadh,
                        "Name": item.Name,
                        "FatherName": item.FatherName,
                        "Address": item.Address,
                        "ContactNo": item.ContactNo,
                        "DOB": item.DOB.split("-").reverse().join("-"),
                        "Gender": item.Gender,//text
                        "CategoryId": item.CategoryId,
                        "PostId": '0',
                        "PostName": item.POSTNAME,
                        "Photo": item.Photo,
                        "SocietyIdEnc": item.SocietyIdEnc == null || item.SocietyIdEnc == "" ? undefined : item.SocietyIdEnc,
                        "IsActive": true,
                        "UserType": "4",//Elected board Menbers
                        "AdministratorDesignationId": "-1",
                        "Occupation": item.Occupation,
                        "MembershipNo": '',
                        "ShareAllotted": 0,
                        "ShareCapitalAmount": 0,
                        "loaneeType": 0,
                        "MEMBERTYPE": item.Status ? "ELECTEDMEMBERS" : 'GENERALMEMBERS',
                        "SubmitStatus": $scope.SubmitStatus,
                        "AdministrativeId": item.AdministrativeId == undefined ? 0 : item.AdministrativeId
                    });
            });


            formData.append("DataArray", JSON.stringify(SocietyRegistrationData));
            //ADDRESS DETAILS     
            $scope.Address = GetAddressinfo('MemberAdd');
            $scope.SocietyRegisteredOffice = {
                SocietyRegOfficeDetailId:0,
                IsRural: $scope.Address.Urban_Rural == 'Rurals' ? true : false,
                HouseBuildingNo: $scope.Address.HouseBuildingNo,
                LaneStreetName: $scope.Address.LaneStreetName,
                Locality: $scope.Address.Locality,
                STATE_ID: $scope.Address.Stateid,
                DIVISION_ID: $scope.Address.DivisionId,
                DISTRICT_ID: $scope.Address.DistrictId,
                //CityTypes: $scope.Address.CityTypes,
                CITY_ID: $scope.Address.CityId == null ? 0 : $scope.Address.CityId,
                BLOCK_ID: $scope.Address.BlockId == null ? 0 : $scope.Address.BlockId,
                WARD_ID: $scope.Address.WardId == null ? 0 : $scope.Address.WardId,
                GP_ID: $scope.Address.PanchayatId == null ? 0 : $scope.Address.PanchayatId,
                VILLAGE_ID: $scope.Address.VillageId == null ? 0 : $scope.Address.VillageId,
                PinCode: $scope.Address.Pincode,
                SocietyIdEnc: $scope.SocietyIdEnc == null || $scope.SocietyIdEnc == "" ? undefined : $scope.SocietyIdEnc,
                CreatedBy: 0,
                UpdatedBy: 0,
                CreatedIP: '',
                UpdatedIP: '',
                IsSameCEOAddress: false,
                AddressFor: 0,
                //SocietyRegOfficeDetailId: "",
                SocietyId: "0",
                ActId: 1,
                DeleteFlag: 0
            }


            formData.append("AddressDetails", JSON.stringify($scope.SocietyRegisteredOffice));
            //END
            formData.append("SaveStatus", $scope.SubmitStatus);
            //AFTER 25122018
            //राज्य स्तरीय खेल संघ               



            if ($scope.SocietyAmendment.IsAmendment != undefined || $scope.SocietyAmendment.IsAmendment == true) {
                $scope.AmendmentDoc = [];

                $scope.AmendmentDocuments.forEach(function(item, x) {
                    if ($scope.GetFileUploaded(item.FileId) != "") {
                        $scope.AmendmentDoc.push({ DocumentType: item.FileId, DocumentName: $scope.GetFileUploaded(item.FileId) });
                    }
                });


                var AmendmentType = "";
                if ($scope.SocietyAmendment.ChangeByeLaws) {
                    AmendmentType += (AmendmentType != "" ? "," : "") + "1";
                }
                if ($scope.SocietyAmendment.ChangeName) {
                    AmendmentType += (AmendmentType != "" ? "," : "") + "2";
                }
                if ($scope.SocietyAmendment.ChaneAddress) {
                    AmendmentType += (AmendmentType != "" ? "," : "") + "3";
                }
                if ($scope.SocietyAmendment.ChaneExecutiveMember) {
                    AmendmentType += (AmendmentType != "" ? "," : "") + "4";
                }
                formData.append("AmendmentType", AmendmentType);
                formData.append("IsAmendment", true);
                formData.append("AmendmentDocuments", JSON.stringify($scope.AmendmentDoc));
            }
            //if ($scope.SocietyRegistration.IsNewRegistration == 'false' && $scope.Act1958 )
            //{
            //    if ($scope.SocietyRegistration.OldExecutiveType==2) {
            //        formData.append("AmendmentType", "4");
            //    }
            //}



            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Society/SaveSocietyProfilingNew", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function(data, status) {

                    $("#InnerModelPopup").addClass("alert alert-danger fade in");

                    if (data.data.indexOf("ErrorInMemberdata") > -1) {
                        ShowModelAlert("Error", "<b>Error In Uploaded Members Excel. All Column values is mandatory.Please fill correct values and re-upload. </b><br/><br/>" + data.data.split('@')[1]);
                        $(".SaveSocietyInfo").show();
                        $(".se-pre-con").fadeOut("slow");
                        return;
                    } else if (data.data == '-5') {
                        if ($scope.SocietyRegistration.ActId != '7')
                            ShowModelAlert("Error", "A Society already exist with this Name or Registration or BRN");
                        else
                            ShowModelAlert("Error", "A Society already exist with this Name or Registration");
                        $(".SaveSocietyInfo").show();
                    }
                    else if (data.data == '-10') {
                        $(".SaveSocietyInfo").show();
                        ShowModelAlert("Error", "YOU ALREADY SUBMITTED 2 SOCIETY WITH THE SAME SSOID");
                    }
                    else
                        if (data.data == '-1') {
                            $(".SaveSocietyInfo").show();
                            ShowModelAlert("Error", "Some error occurred please try after some time ");
                        }
                        else if ((data.data.split('@')).length > 1) {
                            //ShowModelAlert("Society Profiling", "Basic Details Update Sucessfully");
                            //if ($scope.IsEditMode == false) {
                            //    ShowModelAlert("Society Profiling", "Basic Details Saved Sucessfully");
                            //}
                            //window.location = "../Society/SocietyApplicationView?SocietyIDEnc=" + data.data.split('@')[0] + "~_Citizen_SocietyProfile~undefined";
                            var message1 = $scope.IsEditMode == false ? "Basic Details Saved Sucessfully" : "Basic Details Update Sucessfully";
                            ShowOkMessage("Success", message1, function() {
                                window.location = "../Society/SocietyApplicationView?SocietyIDEnc=" + data.data.split('@')[0] + "~_Citizen_SocietyProfile~undefined";
                            });
                        }
                        else if (data.data.length > 10) {
                            //ShowModelAlert("Society Profiling", "Basic Details Saved Sucessfully");
                            //window.location = "../Society/SocietyApplicationView?SocietyIDEnc=" + data.data.split('@')[0] + "~_Citizen_SocietyProfile~undefined";
                            ShowOkMessage("Success", "Basic Details Saved Sucessfully", function() {
                                window.location = "../Society/SocietyApplicationView?SocietyIDEnc=" + data.data.split('@')[0] + "~_Citizen_SocietyProfile~undefined";
                            });
                        }
                    $(".se-pre-con").fadeOut("slow");
                    //$scope.ClearForm();
                }).
                error(function(data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert("Society Profiling", "Error While Society Profiling Insert");
                    $('#btnAdministrativedetails').trigger('click');
                });


        }
        else {
            $(".se-pre-con").fadeOut("slow");

            $(".SaveSocietyInfo").show();
        }
    };

    $scope.OpenOtpPopUpOtherProfiling = function (AADHAARNo, FOR, TRID, ListType) {
        // alert(AADHAARNo+"--"+ FOR+"--"+ TRID)
        //  alert($('#' + TRID).find('td:eq(1)').find('input').val());
        $('#OTPBOX').css("display", "block");
        if (AADHAARNo.length > 12) {
            alert('Invalid Aadhaar Number');
            return;
        }
        $("#SSOAuth_chkConsent").prop("checked", false);
        $('#SSOAuth_OtpText').val('');
        $('#ssoauthloading').hide();
        var arrylist = AADHAARNo.split('');

        $scope.MaskingAADHAARNo = arrylist[0] + arrylist[1] + 'XXXXXXXX' + arrylist[10] + arrylist[11];
        $scope.AADHAARNo = AADHAARNo;
        var num = $scope.AADHAARNo;
        if (isNaN(num)) {
            alert("Please enter only number in aadhar.");
            return;
            //document.write(num1 + " is not a number <br/>");
        } else {

            //$scope.messagebox = false;
            //$scope.OtpDilogBox = true;
            //$scope.FillOtp = false;
            $scope.FOR = FOR;
            $scope.CurrentRow = TRID;

            AadhaarAuth($scope.AADHAARNo, $http, "OTP-BIO", false, function (data) {
                if (ListType == 'TableData10All') {
                    $scope.SocietyExecutiveCommitteeList[TRID].IsValidate = true;
                } else if (ListType == 'TableData10AllSports') {
                    $scope.TableData10AllSports[TRID].IsValidate = true;
                } else if (ListType == 'TableData10NomiAll') {
                    $scope.TableData10NomiAll[TRID].IsValidate = true;
                }
                $scope.SocietyExecutiveCommitteeList[TRID].Name = data.Name;
                $scope.SocietyExecutiveCommitteeList[TRID].FatherName = data.FName;
                $scope.SocietyExecutiveCommitteeList[TRID].DOB = data.DOB.replace(/'-'/g, '/');
                $scope.SocietyExecutiveCommitteeList[TRID].Gender = data.Gender == 'M' ? 'MALE' : 'FEMALE';
                $scope.SocietyExecutiveCommitteeList[TRID].Photo = data.Photo;
                $scope.SocietyExecutiveCommitteeList[TRID].IsValidate = true;
                $scope.SocietyExecutiveCommitteeList[TRID].AadharNo2 = AADHAARNo;
                $scope.$apply();
            });
        }
    };


    $scope.ShowDetails = function (SID, IsFirst) {
        $scope.SocietyIdEnc = SID;
        $scope.SocietyRegistration.SocietyIdEnc = SID;
        $scope.divTabulation = false;
        $scope.divAddBtn = false;
        $scope.divSocialProfiling = false;
        $scope.divSocietyProfilingShow = true;
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Society/GetSocietyDetailsBySocietyId", { "SocietyId": SID }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.SocietyDetailsData = [];
                if (data.length > 0) {
                    SocietyDataForEdit = data;
                    var BasicDetails = data[0];
                    $scope.SocietyRegistration.RegistrationNo = BasicDetails.RegistrationNo;
                    $scope.SocietyRegistration.OldRegistrationNo = BasicDetails.RegistrationNo;
                    $scope.SocietyRegistration.BRN = BasicDetails.BRN;
                    $scope.SocietyRegistration.SocietyName = BasicDetails.SocietyName;
                    $scope.SocietyRegistration.RegistrationDate = BasicDetails.RegistrationDate;
                    $scope.SocietyRegistration.ElectionDate = BasicDetails.ElectionDate;
                    $scope.SocietyRegistration.AGMDate = BasicDetails.AGMDate;
                    $scope.SocietyRegistration.SocietyTypeId = BasicDetails.SocietyTypeId;
                    $scope.Levelofficedata(1)
                    $scope.SocietyRegistration.MajorActivityId = BasicDetails.MajorActivityId;
                    $scope.GetActivity(); 
                    $scope.SocietyRegistration.ActivityId = BasicDetails.ActivityId;
                    $scope.GetBusiness();
                    $scope.SocietyRegistration.BusinessId = BasicDetails.BusinessId;
                    $scope.CheckBRN = true;
                    $scope.SocietyRegistration.TotalNumberOfMembers = BasicDetails.TotalNumberOfMembers;
                    $scope.SocietyRegistration.NoOfMemberInAGM = BasicDetails.NoOfMemberInAGM;

                    var AdministrativeDetails = [];
                    var OfficeDeatils = data;
                    var IsBRNYes = '';
                    $scope.SocietyExecutiveCommitteeList = [];
                    angular.forEach(data, function (item) {
                        if ($scope.SocietyExecutiveCommitteeList.filter(function (c) { return c.AdministrativeId == item.AdministrativeId }).length <= 0) {
                            $scope.SocietyExecutiveCommitteeList.push({
                                Aadh: item.AadharNo,
                                Name: item.Name,
                                FatherName: item.FatherName,
                                ContactNo: item.ContactNo,
                                DOB: item.DOB,
                                Gender: item.Gender,//text
                                CategoryId: item.CategoryId,
                                POSTNAME: item.POSTNAME,
                                Photo: item.Photo,
                                SocietyIdEnc: item.SocietyIdEnc,
                                Occupation: item.Occupation,
                                Status: item.MEMBERTYPE == "ELECTEDMEMBERS" ? true : false,
                                IsValidate: true,
                                AdministrativeId: item.AdministrativeId
                            });
                        }
                    });

                    var addressData = data.filter(function (x) { return x.AddressFor == 0 })[0];

                    var addressdataset1 = {
                        Housebuilding: addressData.HouseNo,
                        State: addressData.STATE_ID,
                        District: " " + addressData.DISTRICT_ID,
                        Lanestreet: addressData.LaneStreetName,// + ' ' + addressdata1[1],
                        Locality: addressData.Locality,//+ ' ' + addressdata1[11],
                        Pincode: addressData.PinCode,
                        CITY_ID: addressData.CITY_ID,
                        BLOCK_ID: addressData.BLOCK_ID,
                        GP_ID: addressData.GP_ID,
                        WARD_Id: addressData.WARD_ID,
                        VILLAGE_ID: addressData.VILLAGE_ID,
                        IsRurals: addressData.IsRural=='True' || addressData.BLOCK_ID != 0
                    }
                    SetAddressinfo($scope.translation, 'MemberAdd', addressdataset1, $http).then(function () {
                        //$scope.Empanelled.AddressDetials = GetAddressDispplay('MemberAdd');
                        //$scope.EmpanelledAddress = GetAddressinfo('MemberAdd');
                    });


                    //$scope.ProfilingDocuments.push({ Id: 1, NameEn: "Copy of the Registration Certificate issued from the Registrar Office", NameHi: "पंजीयक कार्यालय से जारी पंजीकरण प्रमाणपत्र की प्रति", FileId: "RegistrationCertificate", IsShow: true, Size: "1024", FileType: "application/pdf,image/*", Format: "" });
                    //$scope.ProfilingDocuments.push({ Id: 2, NameEn: "Upload copy of Objects of society", NameHi: "सोसाइटी के उद्देश्यों की प्रति अपलोड करें", FileId: "SocietyObjects", IsShow: true, Size: "5120", FileType: "application/pdf,image/*", Format: "/Content/SocietyProfiling/Objects.pdf" });
                    //$scope.ProfilingDocuments.push({ Id: 3, NameEn: "Copy of Rules and Regulations of society", NameHi: "सोसाइटी के नियमों और विनियमों की प्रति", FileId: "ByeLawsSocietyFile", IsShow: true, Size: "2048", FileType: "application/pdf,image/*", Format: "/Content/SocietyProfiling/rulesandregulation.pdf" });
                    //$scope.ProfilingDocuments.push({ Id: 4, NameEn: "Upload Excel Sheet of Members of the Society", NameHi: "सोसायटी के सदस्यों की एक्सेल शीट अपलोड करें", FileId: "MembersListExcelFile", IsShow: true, Size: "1024", FileType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel", Format: "/Content/SocietyExcel/MemberList.xlsx" });
                    //$scope.ProfilingDocuments.push({ Id: 5, NameEn: "Copy of the last executive issued from the Registrar Office", NameHi: "रजिस्ट्रार कार्यालय से जारी अंतिम कार्यकारी की प्रति", FileId: "OldExecutiveCommittee", IsShow: true, Size: "1024", FileType: "application/pdf,image/*", Format: "" });

                    $scope.FileCollection = [];
                    $scope.FileCollection.push({ 'Id': 'RegistrationCertificate', 'FileType': '', 'File': BasicDetails.RegistrationCertificateFile, 'Title': '', FileName: BasicDetails.RegistrationCertificateFile, FileId: 'RegistrationCertificate', IsFileUpload: true });
                    $scope.FileCollection.push({ 'Id': 'SocietyObjects', 'FileType': '', 'File': BasicDetails.ObjectsFile, 'Title': '', FileName: BasicDetails.ObjectsFile, FileId: 'SocietyObjects', IsFileUpload: true });
                    $scope.FileCollection.push({ 'Id': 'ByeLawsSocietyFile', 'FileType': '', 'File': BasicDetails.RulesAndRegulationFile, 'Title': '', FileName: BasicDetails.RulesAndRegulationFile, FileId: 'ByeLawsSocietyFile', IsFileUpload: true });
                    $scope.FileCollection.push({ 'Id': 'MembersListExcelFile', 'FileType': '', 'File': BasicDetails.MemberListFile, 'Title': '', FileName: BasicDetails.MemberListFile, FileId: 'MemberListExcelFile', IsFileUpload: true });
                    $scope.FileCollection.push({ 'Id': 'OldExecutiveCommittee', 'FileType': '', 'File': BasicDetails.OldExecutiveCommittee, 'Title': '', FileName: BasicDetails.OldExecutiveCommittee, FileId: 'OldExecutiveCommittee', IsFileUpload: true });
                                        
                }
               
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                alert("Error while fetching Society Details.");
            });

    }

    // START Show Workscope Details
    $scope.ShowDetailsWorkscope = function (SID, ValidationFlag) {
        if ($scope.lstSocietyWorkScope.length == 0) {
            $(".blockUI").fadeIn("slow");
            $http.post(SiteUrl + "/Society/GetWorkscopeBySocietyId", { "SocietyId": SID, "EMitraTransactionId": $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId }).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    $scope.WorkscopeDetailsData = [];
                    if (data.length > 0) {
                        $scope.lstSocietyWorkScope = data;
                        if ($scope.lstSocietyWorkScope.length > 0) {
                            $scope.SocietyRegistration.AreaOfOperation = $scope.lstSocietyWorkScope[0].AreaOfOperation;
                            $scope.SocietyRegistration.WorkScopeLevel_ID = $scope.lstSocietyWorkScope[0].WorkScopeLevel_ID;
                        }
                        else {
                            $scope.grid = false;
                        }

                        //alert(JSON.stringify($scope.lstSocietyWorkScope));
                        $scope.singlearray = data[0];

                        if ($scope.singlearray.IsRSSC == "True") {
                            $scope.TrueDivForPrimarybody = false;
                            $scope.hidefilediv = false;
                        }
                        else {
                            $scope.TrueDivForPrimarybody = true;
                            $scope.hidefilediv = true;
                        }
                        if ($scope.singlearray.IsRuralUrban == "0" || $scope.singlearray.IsRuralUrban == "False") {

                            $scope.IsStatelevelRural = true;
                            $scope.IsStatelevelRuralNO = false;
                            $scope.IsStatelevelF = false;
                            $scope.IsRuralUworkscope = true;
                            $scope.IsUrbanRworkscope = false;
                            $scope.RuralDivRadio = true;
                        }
                        else if ($scope.singlearray.IsRuralUrban == "1" || $scope.singlearray.IsRuralUrban == "True") {
                            $scope.IsStatelevelRuralNO = false;
                            $scope.IsStatelevelRural = false;
                            $scope.IsStatelevelF = true;
                            $scope.IsRuralUworkscope = false;
                            $scope.IsUrbanRworkscope = true;
                            $scope.RuralDivRadio = false;
                        }

                        if ($scope.singlearray.IsStatLevel == '0' && $scope.SocietyRegistration != undefined) {
                            $scope.SocietyRegistration.IsStatLevel = 'No';
                            //$scope.Addmorebtn(ValidationFlag);
                            $scope.deletebtnhide = false;

                            $scope.grid = false;


                            if ($scope.singlearray.IsRuralUrban == '1') {
                                $scope.SocietyRegistration.Rural = 'true';
                                $scope.RuralDivRadio = true;
                                if ($scope.ActId == 7) {
                                    $scope.IsStatelevelF = true;
                                    $scope.IsStatelevelRural = false;
                                }
                                else {
                                    $scope.IsStatelevelF = false;;
                                }
                            }
                            else {
                                $scope.SocietyRegistration.Rural = 'false';
                                $scope.RuralDivRadio = false;
                            }
                            $scope.grid = true;
                            $scope.InCaseofupdate = true;
                            //$scope.gridtwo = false;
                            // $scope.Addmorebtn();

                        }
                        else if ($scope.singlearray.IsStatLevel == '1') {
                            $scope.SocietyRegistration.IsStatLevel = 'Yes';
                            $scope.RuralDivRadio = false;
                        }
                        if ($scope.lstSocietyWorkScope.length > 0) {
                            $scope.DataYes = true;
                            if ($scope.ActId == 7 || $scope.ActId == '7') {
                                $scope.SocietyRegistration.DISTRICT_ID = $scope.lstSocietyWorkScope[0].DISTRICT_ID;
                                $scope.SocietyDetailsData.BasicDetails.DISTRICT_ENG = $scope.lstSocietyWorkScope[0].DISTRICT_Name;
                                $scope.SocietyDetailsData.BasicDetails.DISTRICT_MANGAL = $scope.lstSocietyWorkScope[0].DISTRICT_MANGAL;
                                $scope.BlockList();
                                if ($scope.singlearray.IsStatLevel == '0') {
                                    $scope.grid = false;
                                }
                            }
                        }
                        else {
                            $scope.DataYes = false;
                        }
                    }
                    else {
                        //$scope.SocietyDetailsData.WorkscopeDetailsData = [];
                        $scope.grid = false;
                    }


                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    alert("Error while fetching Society Details.");
                });
        }
    }
    // END Show Workscope Details


    /***************END Profiling***************************/



    /***************Society Amendment******************************************/

    $scope.ByLaws = [];
    $scope.ActId = 0;
    $scope.SocietyDirectory = "";
    $scope.SocietyIdEnc = "";
    $scope.MinimMemeber = 5;

    $scope.OnPageLoad = function() {
        $(".se-pre-con").fadeIn("slow");
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get("../Translations/Translation_EN.json").
                success(function(data, status) {
                    $scope.translation = data;
                }).
                error(function(data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("../Translations/Translation_HI.json").
                success(function(data, status) {
                    $scope.translation = data;
                }).
                error(function(data, status) {
                    alert("Error while translation");
                });

            $scope.ClearUnitForm = function() {
                $scope.UnitMaster = null;
            };
        }
        $scope.showLoader = false;
        $scope.GetSocietyList();
        $('#txtAGMByeLawsDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $('#txtAGMHeldDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $('#txtBODByeLawsDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $('#txtByLawsAGMDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $('#txtBODMeetingHeldDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $('#txtNameChangeAGMDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });

        //GET Financial Year
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function(data, status) {
                $scope.FinancialYearList = data;
            }).
            error(function(data, status) {
                $scope.FinancialYearList = [];
            });
        //Get Category List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=-1").
            success(function(data, status) {
                $scope.CategoryList = data;
            }).
            error(function(data, status) {
                alert("Error while fetching Post list")

            });
        //Get Post List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=10").
            success(function(data, status) {
                $scope.Posts = data;
            }).
            error(function(data, status) {
                alert("Error while fetching Post list")

            });

        //$(".se-pre-con").fadeOut("slow");
    };


    $scope.RemovalTypeList = [{ CODENAME: 'DEATH', CODENAME_MANGAL: 'DEATH', CODEID: '1' }, { CODENAME: 'REMOVEL', CODENAME_MANGAL: 'REMOVEL', CODEID: '3' }, { CODENAME: 'RESIGNATION', CODENAME_MANGAL: 'RESIGNATION', CODEID: '3' }]

    $scope.SocietyRegisteredOffice = {
        DISTRICT_ID: "0",
        DIVISION_ID: "0",
        CityTypes: "0",
        CITY_ID: "0",
        WARD_ID: "0",
        BLOCK_ID: "0",
        GP_ID: "0",
        VILLAGE_ID: "0"
    };



    $scope.RuralUrbanBlock = function(V) {
        var SelectedVal = $scope.SocietyRegisteredOffice.IsRural != undefined && $scope.SocietyRegisteredOffice.IsRural == 1 ? 'True' : 'False';
        if (V != 1) {
            $scope.SocietyRegisteredOffice.CityTypes = "0";
            $scope.SocietyRegisteredOffice.CITY_ID = "0";
            $scope.SocietyRegisteredOffice.WARD_ID = "0";
            $scope.SocietyRegisteredOffice.BLOCK_ID = "0";
            $scope.SocietyRegisteredOffice.GP_ID = "0";
            $scope.SocietyRegisteredOffice.VILLAGE_ID = "0";
        }
        if (SelectedVal == 'True' || SelectedVal == 'true') {
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;
            $scope.GetCityListByCityTypes(1);
        }
        else if (SelectedVal == 'false' || SelectedVal == 'False') {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
            $scope.BlockListByOffAdd(1)
        }
        else {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
        }
    };
    // Get List of Block
    $scope.BlockList = function() {
        if ($scope.SocietyRegistration.WorkScopeLevel_ID != '7' && $scope.SocietyRegistration.WorkScopeLevel_ID != '8') {
            $scope.Blocks = [];
            $scope.AddMoreForSportShow = true;
            if ($scope.SocietyRegistration.DISTRICT_ID != '0') {
                $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegistration.DISTRICT_ID).
                    success(function(data, status) {
                        $scope.Blocks = data;
                    }).
                    error(function(data, status) {
                        $scope.Blocks = [];
                    });
            }
        }
        else {
            $scope.GetCityList();
        }

    };
    //end block
    //Start
    $scope.GramPanchayatListForBasicD = function() {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegistration.BLOCK_ID).
            success(function(data, status) {
                $scope.AreaGramPanchayats = data;
            }).
            error(function(data, status) {
                $scope.AreaGramPanchayats = [];

            });
    };
    //End
    // START : Get List of Villages
    $scope.VillageList = function(id) {
        if ($scope.SocietyRegisteredOffice.GP_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegisteredOffice.GP_ID).
                success(function(data, status) {
                    $scope.Villages = data;
                }).
                error(function(data, status) {
                    $scope.Villages = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.GP_IDcopy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegisteredOffice.GP_IDcopy).
                success(function(data, status) {
                    $scope.Villagescopy = data;

                }).
                error(function(data, status) {
                    $scope.Villagescopy = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.GP_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegisteredOffice.GP_IDSecretary).
                success(function(data, status) {
                    $scope.VillagesSecretary = data;

                }).
                error(function(data, status) {
                    $scope.VillagesSecretary = [];
                });
        }

        else if ($scope.SocietyRegisteredOffice.GP_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegisteredOffice.GP_IDTreasurer).
                success(function(data, status) {
                    $scope.VillagesTreasurer = data;

                }).
                error(function(data, status) {
                    $scope.VillagesTreasurer = [];
                });
        }

    };
    // END : Get List of Villages
    // START : Get List of Villages for hide
    $scope.GetVillageForBasic = function() {
        $scope.AreaVillages = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegistration.GP_ID).
            success(function(data, status) {

                $scope.AreaVillages = data;
            }).
            error(function(data, status) {
                $scope.AreaVillages = [];
            });
    };
    // END : Get List of Villages

    //START : Get City List
    $scope.GetCityList = function() {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITY&MasterCode=" + $scope.SocietyRegistration.DISTRICT_ID).
            success(function(data, status) {
                $scope.CityList = data;
            }).
            error(function(data, status) {
                $scope.CityList = [];
            });


    };

    $scope.GetCityListByCityTypes = function(id) {
        if ($scope.SocietyRegisteredOffice.DISTRICT_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
                success(function(data, status) {
                    $scope.CityListdata = data;
                }).
                error(function(data, status) {
                    $scope.CityListdata = [];
                });
        }

        if ($scope.SocietyRegisteredOffice.DISTRICT_ID_copy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.SocietyRegisteredOffice.DISTRICT_ID_copy).
                success(function(data, status) {
                    $scope.CityListdatacopy = data;
                }).
                error(function(data, status) {
                    $scope.DistrictListcopy = [];
                });
        }
        if ($scope.SocietyRegisteredOffice.DISTRICT_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.SocietyRegisteredOffice.DISTRICT_IDSecretary).
                success(function(data, status) {
                    $scope.CityListdataSecretary = data;
                }).
                error(function(data, status) {
                    $scope.CityListdataSecretary = [];
                });
        }
        if ($scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer).
                success(function(data, status) {
                    $scope.CityListdataTreasurer = data;
                }).
                error(function(data, status) {
                    $scope.CityListdataTreasurer = [];
                });
        }

        // $scope.RuralUrbanBlock();
    };
    //END : Get City List

    // START : Get Ward List
    $scope.GetWardList = function(id) {
        if ($scope.SocietyRegisteredOffice.CITY_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.SocietyRegisteredOffice.CITY_ID).
                success(function(data, status) {
                    $scope.Ward = data;
                }).
                error(function(data, status) {
                    $scope.Ward = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.CITY_IDcopy != '0' && id == 2 && $scope.SocietyRegisteredOffice.CITY_IDcopy != undefined) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.SocietyRegisteredOffice.CITY_IDcopy).
                success(function(data, status) {
                    $scope.Wardcopy = data;
                }).
                error(function(data, status) {
                    $scope.Wardcopy = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.CITY_IDSecretary != '0' && $scope.SocietyRegisteredOffice.CITY_IDSecretary != undefined && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.SocietyRegisteredOffice.CITY_IDSecretary).
                success(function(data, status) {
                    $scope.WardSecretary = data;
                }).
                error(function(data, status) {
                    $scope.WardSecretary = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.CITY_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.SocietyRegisteredOffice.CITY_IDTreasurer).
                success(function(data, status) {
                    $scope.WardTreasurer = data;
                }).
                error(function(data, status) {
                    $scope.WardTreasurer = [];
                });
        }

    };

    // END : Get Ward List

    // START : Get Ward List
    $scope.GetWardListbySociety = function() {
        $scope.Ward = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ward&MasterCode=" + $scope.SocietyRegistration.CITY_ID).
            success(function(data, status) {
                $scope.Ward = data;
            }).
            error(function(data, status) {
                $scope.Ward = [];
            });
    };
    // START : Get List of GramPanchayat

    //start BlockListByOffAddress form
    $scope.BlockListByOffAdd = function(id) {
        if ($scope.SocietyRegisteredOffice.DISTRICT_ID != '0' && id == 1) {
            $scope.Blocksdata = [];
            $scope.GramPanchayats = [];
            $scope.Villages = [];
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
                success(function(data, status) {
                    $scope.Blocksdata = data;
                }).
                error(function(data, status) {
                    $scope.Blocksdata = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.DISTRICT_ID_copy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID_copy).
                success(function(data, status) {
                    $scope.Blocksdatacopy = data;
                }).
                error(function(data, status) {
                    $scope.Blocksdatacopy = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.DISTRICT_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_IDSecretary).
                success(function(data, status) {
                    $scope.BlocksdataSecretary = data;
                }).
                error(function(data, status) {
                    $scope.BlocksdataSecretary = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer).
                success(function(data, status) {

                    $scope.BlocksdataTreasurer = data;

                }).
                error(function(data, status) {
                    $scope.BlocksdataTreasurer = [];
                });
        }

    };
    //end
    $scope.GetCityListByCityTypes = function(id) {
        if ($scope.SocietyRegisteredOffice.DISTRICT_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
                success(function(data, status) {
                    $scope.CityListdata = data;
                }).
                error(function(data, status) {
                    $scope.CityListdata = [];
                });
        }

        if ($scope.SocietyRegisteredOffice.DISTRICT_ID_copy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.SocietyRegisteredOffice.DISTRICT_ID_copy).
                success(function(data, status) {
                    $scope.CityListdatacopy = data;
                }).
                error(function(data, status) {
                    $scope.DistrictListcopy = [];
                });
        }
        if ($scope.SocietyRegisteredOffice.DISTRICT_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.SocietyRegisteredOffice.DISTRICT_IDSecretary).
                success(function(data, status) {
                    $scope.CityListdataSecretary = data;
                }).
                error(function(data, status) {
                    $scope.CityListdataSecretary = [];
                });
        }
        if ($scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer).
                success(function(data, status) {
                    $scope.CityListdataTreasurer = data;
                }).
                error(function(data, status) {
                    $scope.CityListdataTreasurer = [];
                });
        }

        // $scope.RuralUrbanBlock();
    };
    //END : Get City List

    $scope.GetDistrictList = function(id) {
        if ($scope.SocietyRegisteredOffice.DIVISION_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyRegisteredOffice.DIVISION_ID).
                success(function(data, status) {
                    $scope.DistrictList = data;

                }).
                error(function(data, status) {
                    $scope.DistrictList = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.DIVISION_ID_copy != '0' && id == 2) {

            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyRegisteredOffice.DIVISION_ID_copy).
                success(function(data, status) {
                    $scope.DistrictListcopy = data;

                }).
                error(function(data, status) {
                    $scope.DistrictListcopy = [];
                });

        }
        else if ($scope.SocietyRegisteredOffice.DIVISION_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyRegisteredOffice.DIVISION_IDSecretary).
                success(function(data, status) {
                    $scope.DistrictListSecretary = data;
                }).
                error(function(data, status) {
                    $scope.DistrictListSecretary = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.DIVISION_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyRegisteredOffice.DIVISION_IDTreasurer).
                success(function(data, status) {
                    $scope.DistrictListTreasurer = data;
                }).
                error(function(data, status) {
                    $scope.DistrictListTreasurer = [];
                });
        }

    };
    $scope.ResetDataForAddress = function(val, Level) {
        if (val == 1) {
            if (Level < 3) {
                $scope.SocietyRegisteredOffice.IsRural = undefined;
                $scope.CityListdata = [];
                $scope.Blocksdata = [];
                $scope.GramPanchayats = [];
                $scope.Villages = [];
                $scope.Ward = [];
            }
        }
        else if (val == 2) {
            if (Level < 3) {
                $scope.SocietyRegisteredOffice.IsRuralcopy = undefined;
                $scope.CityListdatacopy = [];
                $scope.Blocksdatacopy = [];
                $scope.GramPanchayatscopy = [];
                $scope.Villagescopy = [];
                $scope.Wardcopy = [];
            }
        }
    }
    $scope.Divisions = [];
    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
        success(function(data, status) {
            $scope.Divisions = data;
        }).
        error(function(data, status) {
            $scope.Divisions = [];
        });

    // START : Get List of GramPanchayat
    $scope.GramPanchayatList = function(id) {

        if ($scope.SocietyRegisteredOffice.BLOCK_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegisteredOffice.BLOCK_ID).
                success(function(data, status) {

                    $scope.GramPanchayats = data;
                }).
                error(function(data, status) {
                    $scope.GramPanchayats = [];

                });
        }
        else if ($scope.SocietyRegisteredOffice.BLOCK_ID_copy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegisteredOffice.BLOCK_ID_copy).
                success(function(data, status) {

                    $scope.GramPanchayatscopy = data;
                }).
                error(function(data, status) {
                    $scope.GramPanchayatscopy = [];

                });
        }
        else if ($scope.SocietyRegisteredOffice.BLOCK_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegisteredOffice.BLOCK_IDSecretary).
                success(function(data, status) {

                    $scope.GramPanchayatsSecretary = data;
                }).
                error(function(data, status) {
                    $scope.GramPanchayatsSecretary = [];

                });
        }
        else if ($scope.SocietyRegisteredOffice.BLOCK_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegisteredOffice.BLOCK_IDTreasurer).
                success(function(data, status) {

                    $scope.GramPanchayatsTreasurer = data;
                }).
                error(function(data, status) {
                    $scope.GramPanchayatsTreasurer = [];

                });
        }


    };
    // END : Get List of GramPanchayat
    //Start

    // START : Get List of Villages
    $scope.VillageList = function(id) {
        if ($scope.SocietyRegisteredOffice.GP_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegisteredOffice.GP_ID).
                success(function(data, status) {
                    $scope.Villages = data;
                }).
                error(function(data, status) {
                    $scope.Villages = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.GP_IDcopy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegisteredOffice.GP_IDcopy).
                success(function(data, status) {
                    $scope.Villagescopy = data;

                }).
                error(function(data, status) {
                    $scope.Villagescopy = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.GP_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegisteredOffice.GP_IDSecretary).
                success(function(data, status) {
                    $scope.VillagesSecretary = data;

                }).
                error(function(data, status) {
                    $scope.VillagesSecretary = [];
                });
        }

        else if ($scope.SocietyRegisteredOffice.GP_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegisteredOffice.GP_IDTreasurer).
                success(function(data, status) {
                    $scope.VillagesTreasurer = data;

                }).
                error(function(data, status) {
                    $scope.VillagesTreasurer = [];
                });
        }

    };
    // END : Get List of Villages
    // START : Get Ward List
    $scope.GetWardList = function(id) {
        if ($scope.SocietyRegisteredOffice.CITY_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.SocietyRegisteredOffice.CITY_ID).
                success(function(data, status) {
                    $scope.Ward = data;
                }).
                error(function(data, status) {
                    $scope.Ward = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.CITY_IDcopy != '0' && id == 2 && $scope.SocietyRegisteredOffice.CITY_IDcopy != undefined) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.SocietyRegisteredOffice.CITY_IDcopy).
                success(function(data, status) {
                    $scope.Wardcopy = data;
                }).
                error(function(data, status) {
                    $scope.Wardcopy = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.CITY_IDSecretary != '0' && $scope.SocietyRegisteredOffice.CITY_IDSecretary != undefined && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.SocietyRegisteredOffice.CITY_IDSecretary).
                success(function(data, status) {
                    $scope.WardSecretary = data;
                }).
                error(function(data, status) {
                    $scope.WardSecretary = [];
                });
        }

        else if ($scope.SocietyRegisteredOffice.CITY_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.SocietyRegisteredOffice.CITY_IDTreasurer).
                success(function(data, status) {
                    $scope.WardTreasurer = data;
                }).
                error(function(data, status) {
                    $scope.WardTreasurer = [];
                });
        }

    };
    $scope.Statelist = [];
    $scope.Statelist.push(
        {
            "STATE_ID": 6,
            "STATE_ENG": 'Rajasthan',
            "STATE_MANGAL": 'राजस्थान'

        });

    // END : Get Ward List

    // START :Get List of Societies
    $scope.GetSocietyList = function() {
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Citizen/GetSocietyList", { "WhereClause": " " }).
            success(function(data, status) {
                //$scope.SocietyList = data.filter(x => (x.IsSameCEOAddress == 'True') && (x.AddressFor == '0'));
                $scope.SocietyList = [];// data;
                if (data.DataList.filter(function(x) { return x.ActId == 1 }).length <= 0) {
                    //ShowAlertMessage("Alert", _language == 'en-us' ? "Your SSO ID " + $("#hdncurrentSSOID").val() + " is not mapped with any society. Do you want to map a society with your SSO ID?" : "आपकी SSO ID " + $("#hdncurrentSSOID").val() + " किसी भी सोसाइटी के साथ मैप नहीं की गई है। क्यां आप अपनी SSO ID को किसी सोसाइटी से मैप करना चाहते हैं?", function () {
                    //    window.location = "../Citizen/UserSocietyMappingValid";
                    //});
                } else {
                    data.DataList.forEach(function(item, index) {
                        if (item.ActId == 1) {
                            $scope.SocietyList.push(item);
                        }
                    });
                }
                $(".blockUI").fadeOut("slow");
            }).
            error(function(data, status) {
                $scope.SocietyList = [];
                $(".blockUI").fadeOut("slow");
                alert('Error while fetching society list data.');
            });
    };
    // END :Get List of Societies

    // START :Get List of Member List
    $scope.GetMemberList = function () {
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        $scope.SocietyDirectoryFile = Datasplit[1];

        if (!$scope.IsSocietyEdit) {
            $scope.MemberAmendmentList = [];
            return $scope.GetMemberAmendmentBySocietyId();
        } else {

            if (Datasplit.length > 2) {
                var SocietyIdEnc = Datasplit[0];
                var ActId = Datasplit[2];
                $scope.ActId = ActId;
                $scope.SocietyDirectory = Datasplit[1];
                $scope.SocietyIdEnc = SocietyIdEnc;
                $scope.MinimMemeber = 5;
                $(".se-pre-con").fadeIn("slow");
                //alert(SocietyIdEnc);
                //$http.get("../Citizen/GetAGM_BODMList?SocietyIdEnc=" + SocietyIdEnc).
                return $http.post(SiteUrl + "/Society/GetSocietyDetailsBySocietyId", { "SocietyId": SocietyIdEnc, "EMitraTransactionId": 0 }).
                    success(function (data, status) {
                        var ALLMEMBERS = [];
                        ALLMEMBERS = data.filter(x => (x.IsSameCEOAddress == 'True') && (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS'));
                        var TableData10All = [];
                        $scope.TableData10AllCopy = [];
                        var startID = 1;
                        var X = 7;
                        X = (ALLMEMBERS.length > 4 ? ALLMEMBERS.length : 5);
                        for (var x = 0; x < ALLMEMBERS.length; x++) {
                            if (TableData10All.filter(function (C) { return C.AdministrativeId == ALLMEMBERS[x].AdministrativeId }).length <= 0) {
                                TableData10All.push(
                                    {
                                        "Id": x + startID,
                                        "Name": ALLMEMBERS[x].Name,
                                        "FatherName": ALLMEMBERS[x].FatherName,
                                        "POSTNAME": ALLMEMBERS[x].POSTNAME,
                                        "Aadh": ALLMEMBERS[x].AadharNo,
                                        "Gender": ALLMEMBERS[x].Gender == "F" ? 'FEMALE' : 'MALE',
                                        "DOB": ALLMEMBERS[x].DOB,
                                        "ContactNo": ALLMEMBERS[x].ContactNo,
                                        "Photo": ALLMEMBERS[x].Photo,
                                        "CategoryName": ALLMEMBERS[x].Category,
                                        "Occupation": ALLMEMBERS[x].Occupation,
                                        "CatId": ALLMEMBERS[x].CategoryId,
                                        "CategoryId": ALLMEMBERS[x].CategoryId,
                                        "Address": ALLMEMBERS[x].Address,
                                        "Status": ALLMEMBERS[x].MEMBERTYPE == 'ELECTEDMEMBERS' ? true : false,
                                        "MEMBERTYPE": ALLMEMBERS[x].MEMBERTYPE,
                                        "IsValidate": true,
                                        "IsNew": false,
                                        "IsDelete": false,
                                        "AdministrativeId": ALLMEMBERS[x].AdministrativeId,
                                        "AadharNo2": ALLMEMBERS[x].AadharNo2,
                                        "RemovalType": "0"
                                    });
                            }
                        }
                        if (TableData10All.length < 5) {
                            for (var x = TableData10All.length; x < 5; x++) {
                                TableData10All.push(
                                    {
                                        "Id": TableData10All.length + x,
                                        "Name": "",
                                        "FatherName": "",
                                        "POSTNAME": "",
                                        "Aadh": "",
                                        "Gender": "",
                                        "DOB": "",
                                        "ContactNo": "",
                                        "Photo": "",
                                        "CategoryName": "",
                                        "Occupation": "",
                                        "CatId": "",
                                        "CategoryId": "",
                                        "Address": "",
                                        "Status": false,
                                        "MEMBERTYPE": "",
                                        "IsValidate": false,
                                        "IsNew": true,
                                        "IsDelete": false,
                                        "AdministrativeId": 0,
                                        "AadharNo2": "",
                                        "RemovalType": "0"
                                    });
                            }
                        }



                        $scope.TableData10All = TableData10All;

                        $scope.TableData10AllCopy = [];

                        TableData10All.forEach(function (itm, indx) {
                            $scope.TableData10AllCopy.push(
                                {
                                    "Id": indx,
                                    "Name": itm.Name,
                                    "FatherName": itm.FatherName,
                                    "POSTNAME": itm.POSTNAME,
                                    "Aadh": itm.AadharNo,
                                    "Gender": itm == "F" ? 'FEMALE' : 'MALE',
                                    "DOB": itm.DOB,
                                    "ContactNo": itm.ContactNo,
                                    "Photo": itm.Photo,
                                    "CategoryName": itm.Category,
                                    "Occupation": itm.Occupation,
                                    "CatId": itm.CategoryId,
                                    "CategoryId": itm.CategoryId,
                                    "Address": itm.Address,
                                    "Status": itm.MEMBERTYPE == 'ELECTEDMEMBERS' ? true : false,
                                    "MEMBERTYPE": itm.MEMBERTYPE,
                                    "IsValidate": true,
                                    "IsNew": false,
                                    "IsDelete": false,
                                    "AdministrativeId": itm.AdministrativeId,
                                    "AadharNo2": itm.AadharNo2,
                                    "RemovalType": itm.RemovalType
                                }
                            );
                        });



                        $(".se-pre-con").fadeOut("slow");
                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        ALLMEMBERS = [];
                        alert('Error while fetching society list data.');
                    });
            }
        }
    };



    $scope.IsSocietyEdit = true;
    $scope.IsEditApplication = true;
    $scope.OnChangeSociety = function(item) {
        $(".se-pre-con").fadeIn("slow");


        var data = $scope.SocietyList.filter(function(x) { return x.SocietyIdENC.toString() === $scope.SocietyAmendment.SocietyIdEnc.split("~")[0] });

        if (data[0].MaxAppliFlowStatusID == 17 || data[0].MaxAppliFlowStatusID == 26) {
            $scope.ShowTab = true;
            $scope.IsSocietyEdit = data[0].IsAmendment == 'True' ? false : true;
            $scope.IsEditApplication = $scope.IsSocietyEdit;

            $scope.SocietyAmendment.TotalMember = data[0].TotalMember;

            $scope.GetMemberList().then(function() {
                if (!$scope.IsSocietyEdit) {
                    $scope.GetSocietyNameChangeList().then(function() {
                        $(".se-pre-con").fadeOut("slow");
                    });
                } else {
                    $(".se-pre-con").fadeOut("slow");
                }
            });
        }
        else {
            $scope.SocietyAmendment.SocietyIdEnc = "0";
            $(".se-pre-con").fadeOut("slow");
            ShowOkMessage("Warning", _language == 'en-us' ? "Profiling process of the selected society has not been completed. You can amend the society only when its profiling is complete and it is approved by the concerned office." : "आपके द्वारा चयन की गयी सोसाइटी की प्रोफाइलिंग प्रक्रिया का अनुमोदन राजसहाकर पोर्टल पर पूर्ण नहीं हुवा | आप सोसाइटी में संशोधन तभी कर सकते हैं जब इसकी प्रोफाइलिंग पूर्ण हो व यह सम्बंधित कार्यालय से अनुमोदित हो |  ", function() { });

        }





    };
    // START :Get List of ByLaws List
    $scope.GetByLawsList = function() {
        // $(".se-pre-con").fadeIn("slow");
        $scope.ResetByLawsModal();
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            // alert(SocietyIdEnc);
            return $http.get("../Citizen/GetByLawsList?SocietyIdEnc=" + SocietyIdEnc).
                success(function(data, status) {
                    $scope.ByLawsListData = data;
                    // $(".se-pre-con").fadeOut("slow");
                    //alert(JSON.stringify($scope.ByLawsListData));
                }).
                error(function(data, status) {
                    $scope.ByLawsListData = [];
                    //$(".se-pre-con").fadeOut("slow");
                    alert('Error while fetching society list data.');
                });
        } else {
            return 1;
        }
    };

    // END :Get List of ByLaws List

    // START :Get List of AGM_BODM List
    $scope.GetAGM_BODMList = function() {
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            //alert(SocietyIdEnc);
            $http.get("../Citizen/GetAGM_BODMList?SocietyIdEnc=" + SocietyIdEnc).
                success(function(data, status) {
                    $scope.AGMListData = data.filter(x => x.Is_AGM_BODM == 'True');
                    $scope.BODMListData = data.filter(x => x.Is_AGM_BODM == 'False');
                }).
                error(function(data, status) {
                    $scope.BODMListData = [];
                    $scope.AGMListData = [];
                    alert('Error while fetching society list data.');
                });
        }
    };
    // END :Get List of AGM_BODM List

    $scope.TableData10AllCopy = [];
    $scope.TableData10AllSportsCopy = [];
    $scope.TableData10NomiAllCopy = [];


    $scope.MemberAmendmentList = [];
    $scope.GetMemberAmendmentBySocietyId = function() {
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        if (Datasplit.length > 2) {
            var SocietyIdEnc = Datasplit[0];
            var ActId = Datasplit[2];
            $scope.ActId = ActId;
            $scope.SocietyDirectory = Datasplit[1];
            $scope.SocietyIdEnc = SocietyIdEnc;
            if (ActId == 1) {
                $scope.MinimMemeber = 7;
            }
            else if (ActId == 3) {
                $scope.MinimMemeber = 15;
            }
            else if (ActId == 7) {
                $scope.MinimMemeber = 5;
            }
            $(".se-pre-con").fadeIn("slow");
            //alert(SocietyIdEnc);
            //$http.get("../Citizen/GetAGM_BODMList?SocietyIdEnc=" + SocietyIdEnc).
            return $http.post(SiteUrl + "/Society/GetMemberAmendmentBySocietyId", { "SocietyId": SocietyIdEnc }).
                success(function(data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.MemberAmendmentList = [];
                    $scope.MemberAmendmentList = data;
                    $scope.MemberAmendmentList.forEach(function(item, indx) {
                        if (item.RequestType == 1) {
                            //$scope.FileCollection.push({ 'Id': 'flRemovalType_' + indx, 'FileType': '', 'File': item.RemovalFile, 'Title': '', FileName: item.RemovalFile, FileId: 'flRemovalType_' + indx, IsFileUpload: true });
                            item.IsDelete = true;
                            //item.NewAdministrativeId = item.AdministrativeId;
                            //item.RemovalType = item.RemovalType.toString();
                        }
                    });
                }).
                error(function(data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    ALLMEMBERS = [];
                    alert('Error while fetching society list data.');
                });
        } else {
            return [];
        }
    };
    // END :Get List of Member List


    $scope.SocietyNameChangeList = [];
    $scope.GetSocietyNameChangeList = function() {
        //$(".se-pre-con").fadeIn("slow");


        $scope.ResetByLawsModal();



        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            // alert(SocietyIdEnc);
            return $http.get("../Citizen/GetSocietyNameChangeList?SocietyIdEnc=" + SocietyIdEnc).
                success(function(data, status) {

                    var dataAll = JSON.parse(data.DataList);

                    $scope.SocietyNameChangeList = dataAll.Table;
                    var documents = dataAll.Table1;
                    $scope.allDisable = true;
                    $scope.FileCollection = [];

                    documents.forEach(function(item, index) {
                        $scope.FileCollection.push({ 'Id': item.DocumentType, 'FileType': '', 'File': item.DocumentURL, 'Title': '', FileName: item.DocumentURL, FileId: item.DocumentType, IsFileUpload: true });
                    });


                    if (!$scope.SocietyNameChangeList[0].IsPermanentSave) {
                        $scope.EditApplication($scope.SocietyNameChangeList[0])
                    } else {

                        $scope.SocietyAmendment.ChangeByeLaws = $scope.SocietyNameChangeList[0].AmendmentType.indexOf('1') > -1 ? true : false;
                        $scope.SocietyAmendment.ChangeName = $scope.SocietyNameChangeList[0].AmendmentType.indexOf('2') > -1 ? true : false;
                        $scope.SocietyAmendment.ChaneAddress = $scope.SocietyNameChangeList[0].AmendmentType.indexOf('3') > -1 ? true : false;
                        $scope.SocietyAmendment.ChaneExecutiveMember = $scope.SocietyNameChangeList[0].AmendmentType.indexOf('4') > -1 ? true : false;

                        $scope.SocietyAmendment.AmendmentSection = $scope.SocietyNameChangeList[0].Section;

                        $scope.checkNameChange();



                        //if ($scope.SocietyNameChangeList[0].flResoloutionNotice != '') {
                        //    $scope.FileCollection.push({ 'Id': 'flResoloutionNotice', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flResoloutionNotice, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flResoloutionNotice, FileId: 'flResoloutionNotice', IsFileUpload: true });
                        //}
                        //if ($scope.SocietyNameChangeList[0].flAttendanceSheet != '') {
                        //    $scope.FileCollection.push({ 'Id': 'flAttendanceSheet', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flAttendanceSheet, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flAttendanceSheet, FileId: 'flAttendanceSheet', IsFileUpload: true });
                        //}
                        //if ($scope.SocietyNameChangeList[0].ByeLawsFile != '') {
                        //    $scope.FileCollection.push({ 'Id': 'flBylawsFileName', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].ByeLawsFile, 'Title': '', FileName: $scope.SocietyNameChangeList[0].ByeLawsFile, FileId: 'flBylawsFileName', IsFileUpload: true });
                        //}
                        //if ($scope.SocietyNameChangeList[0].flByLowComparativeDocument != '') {
                        //    $scope.FileCollection.push({ 'Id': 'flByLowComparativeDocument', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flByLowComparativeDocument, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flByLowComparativeDocument, FileId: 'flByLowComparativeDocument', IsFileUpload: true });
                        //}
                        //if ($scope.SocietyNameChangeList[0].flAGMDeliveryProof != '') {
                        //    $scope.FileCollection.push({ 'Id': 'flAGMDeliveryProof', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flAGMDeliveryProof, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flAGMDeliveryProof, FileId: 'flAGMDeliveryProof', IsFileUpload: true });
                        //}
                        //if ($scope.SocietyNameChangeList[0].flSGMNotice != '') {
                        //    $scope.FileCollection.push({ 'Id': 'flSGMNotice', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flSGMNotice, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flSGMNotice, FileId: 'flSGMNotice', IsFileUpload: true });
                        //}
                        //if ($scope.SocietyNameChangeList[0].flSGMAttendanceSheet != '') {
                        //    $scope.FileCollection.push({ 'Id': 'flSGMAttendanceSheet', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flSGMAttendanceSheet, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flSGMAttendanceSheet, FileId: 'flSGMAttendanceSheet', IsFileUpload: true });
                        //}

                        if ($scope.SocietyNameChangeList[0].AmendmentType.indexOf('3') > -1) {

                            $scope.SocietyRegisteredOffice.STATE_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].STATE_ENG : $scope.SocietyNameChangeList[0].STATE_MANGAL;
                            $scope.SocietyRegisteredOffice.HouseBuildingNo = $scope.SocietyNameChangeList[0].HouseBuildingNo;
                            $scope.SocietyRegisteredOffice.LaneStreetName = $scope.SocietyNameChangeList[0].LaneStreetName;
                            $scope.SocietyRegisteredOffice.Locality = $scope.SocietyNameChangeList[0].Locality;
                            $scope.SocietyRegisteredOffice.DIVISION_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].DIVISION_ENG : $scope.SocietyNameChangeList[0].DIVISION_MANGAL;
                            $scope.SocietyRegisteredOffice.DISTRICT_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].DISTRICT_ENG : $scope.SocietyNameChangeList[0].DISTRICT_MANGAL;
                            $scope.SocietyRegisteredOffice.PinCode = $scope.SocietyNameChangeList[0].PinCode;
                            //$scope.GetDistrictList(1); //$scope.ResetDataForAddress(1, 2)

                            $scope.SocietyRegisteredOffice.CITY_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].CITY_ENG : $scope.SocietyNameChangeList[0].CITY_MANGAL;
                            $scope.SocietyRegisteredOffice.WARD_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].WARD_ENG : $scope.SocietyNameChangeList[0].WARD_MANGAL;


                            $scope.SocietyRegisteredOffice.BLOCK_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].BLOCK_ENG : $scope.SocietyNameChangeList[0].BLOCK_MANGAL;
                            $scope.SocietyRegisteredOffice.GP_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].GP_ENG : $scope.SocietyNameChangeList[0].GP_MANGAL;
                            $scope.SocietyRegisteredOffice.VILLAGE_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].VILLAGE_ENG : $scope.SocietyNameChangeList[0].VILLAGE_MANGAL;



                        }
                    }

                    // $(".se-pre-con").fadeOut("slow");
                    //alert(JSON.stringify($scope.ByLawsListData));
                }).
                error(function(data, status) {
                    $scope.ByLawsListData = [];
                    //$(".se-pre-con").fadeOut("slow");
                    alert('Error while fetching society list data.');
                });
        } else {
            return 1;
        }
    };



    $scope.GetChangeSocietyMemberExcelList = function() {
        $(".se-pre-con").fadeIn("slow");


        $scope.ResetByLawsModal();



        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            // alert(SocietyIdEnc);
            $http.get("../Citizen/GetChangeSocietyMemberExcelList?SocietyIdEnc=" + SocietyIdEnc).
                success(function(data, status) {
                    $scope.SocietyMemberExcelList = data;
                    $(".se-pre-con").fadeOut("slow");
                    //alert(JSON.stringify($scope.ByLawsListData));
                }).
                error(function(data, status) {
                    $scope.ByLawsListData = [];
                    $(".se-pre-con").fadeOut("slow");
                    alert('Error while fetching society list data.');
                });
        }
    };

    // START :Get List of BalanceSheet 
    $scope.GetBalanceSheet = function() {
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            var ActId = Datasplit[2];
            $http.get("../Citizen/GetBalanceSheetCurrent?SocietyIdEnc=" + SocietyIdEnc).
                success(function(data, status) {
                    $scope.BalanceSheetOld = data;
                }).
                error(function(data, status) {
                    $scope.BODMListData = [];
                    $scope.AGMListData = [];
                    alert('Error while fetching society list data.');
                });
        }
    };
    // END :Get List of BalanceSheet 
    // START :Get List of BalanceSheetFileListData
    $scope.BalanceSheetFileList = function() {
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            $http.get("../Citizen/GetBalanceSheetAllList?SocietyIdEnc=" + SocietyIdEnc).
                success(function(data, status) {
                    $scope.BalanceSheetFileListData = data;
                }).
                error(function(data, status) {
                    $scope.BODMListData = [];
                    $scope.AGMListData = [];
                    alert('Error while fetching society list data.');
                });
        }
    };
    // END :Get List of BalanceSheetFileListData

    $scope.UploadBylawsFile = function(file) {
        $scope.ByeLawsSocietyFile = file[0];
    };
    $scope.UploadResolutionFile = function(file) {
        $scope.ResolutionFile = file[0];
    };
    $scope.UploadAGM1File = function(file) {
        $scope.AGM1File = file[0];
    };
    $scope.UploadAGM2File = function(file) {
        $scope.AGM2File = file[0];
    };
    $scope.UploadAGM3File = function(file) {
        $scope.AGM3File = file[0];
    };
    $scope.UploadAGM_PDF = function(file) {
        $scope.AGM_PDF = file[0];
    };
    $scope.UploadBSheetFileExl = function(file) {
        $scope.InsertBSheetFileExl = file[0];
    }
    $scope.UploadBODAGM_PDF = function(file) {
        $scope.BODAGM_PDF = file[0];
    };

    $scope.UploadMembersListExcelFile = function(file) {
        $scope.MembersListExcelFile = file[0];
    }


    $scope.SaveSocietyMemberExcel = function() {
        var Msg = '';
        if ($scope.SocietyAmendment.RegiYearId == undefined || $scope.SocietyAmendment.RegiYearId == '0') {
            Msg += (_language == 'en-us' ? 'Please Select Registered Year \n' : 'पंजीकृत वर्ष का चयन करे\n');
            Msg += '<br />';
        }
        if ($scope.MembersListExcelFile == undefined || $scope.MembersListExcelFile == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload Valid Member Excel Sheet.' : '\nकृपया मान्य सदस्य एक्सेल शीट अपलोड करें ।';
            Msg += '<br />';
        }
        if (Msg.length > 1) {
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + '<br />' + Msg;
            ShowModelAlert("Society Amendment Details", Msg);
            return;
        }
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        formData.append("SocietyIdEnc", Datasplit[0]);
        formData.append("SocietyDirectory", Datasplit[1]);
        formData.append("RegiYearId", $scope.SocietyAmendment.RegiYearId);
        formData.append("AGM3File", $scope.MembersListExcelFile);
        formData.append("Remark", $scope.SocietyAmendment.Remark == undefined ? "" : $scope.SocietyAmendment.Remark);
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Citizen/SaveSocietyMemberExcel", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function(data, status) {
                $(".blockUI").fadeOut("slow");

                $("#InnerModelPopup").addClass("alert alert-danger fade in");
                if (data.data.indexOf("ErrorInMemberdata") > -1) {
                    ShowModelAlert("Error", "<b>Error In Uploaded Members Excel. All Column values is mandatory.Please fill correct values and re-upload. </b><br/><br/>" + data.data.split('@')[1]);
                    return;
                }
                var AltMsg = "";
                var Result = parseInt(data.data, 10);
                if (Result > 0) {
                    AltMsg = (_language == 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                    ShowOkMessage("", AltMsg, function() {
                        location.reload();
                    });

                }
                else if (Result == -2) {
                    AltMsg = (_language == 'en-us' ? 'Data Already Exist.' : 'डेटा पहले से मौजूद है');
                    ShowOkMessage("Error", AltMsg, function() { });
                    return;
                }
                else {
                    AltMsg = (_language == 'en-us' ? 'Some Error Occurred please try after some time.' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                    ShowOkMessage("Error", AltMsg, function() { });
                    return;
                }
                //alert(AltMsg);

            }).
            error(function(data, status) {
                $(".blockUI").fadeOut("slow");
                //alert('An Error Occurred Please Try Again Later');
                ShowOkMessage("Error", 'An Error Occurred Please Try Again Later', function() { });
                location.reload();
            });
    };


    $scope.ChangeAmendmentSection = function() {
        if ($scope.SocietyAmendment.AmendmentSection == 12) {
            $scope.SocietyAmendment.ChangeName = false;
            $scope.SocietyAmendment.ChangeByeLaws = false;
            $scope.SocietyAmendment.ChaneAddress = false;
            $scope.SocietyAmendment.ChaneExecutiveMember = false;
        } else if ($scope.SocietyAmendment.AmendmentSection == 4) {
            $scope.SocietyAmendment.ChangeName = false;
            $scope.SocietyAmendment.ChangeByeLaws = false;
            $scope.SocietyAmendment.ChaneAddress = false;
            $scope.SocietyAmendment.ChaneExecutiveMember = true;

        } else if ($scope.SocietyAmendment.AmendmentSection == 19) {
            $scope.SocietyAmendment.ChangeName = false;
            $scope.SocietyAmendment.ChangeByeLaws = false;
            $scope.SocietyAmendment.ChaneAddress = false;
            $scope.SocietyAmendment.ChaneExecutiveMember = false;
        }
    }

    $scope.ValidateBRNNo = "";
    $scope.ValidateSocietyName = "";
    $scope.SaveByLaws = function (IsPermanentSave) {
        var Msg = '';
        var formData = new FormData();
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");


        if (!$scope.SocietyAmendment.ChangeByeLaws && !$scope.SocietyAmendment.ChangeName && !$scope.SocietyAmendment.ChaneAddress && !$scope.SocietyAmendment.ChaneExecutiveMember) {
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + '<br />' + Msg;
            Msg += (_language == 'en-us' ? 'Please choose amendment type\n' : 'Please choose amendment type\n');
            Msg += '<br />';
            ShowModelAlert("Society Amendment Details", Msg);
            return;
            //return false;
        }
        if ($scope.SocietyAmendment.SocietyIdEnc == undefined || $scope.SocietyAmendment.SocietyIdEnc == '0') {
            Msg += (_language == 'en-us' ? 'Please Select Society \n' : 'सोसाइटी का चयन करे\n');
            Msg += '<br />';
            ShowModelAlert("Society Amendment Details", Msg);
            return;
        }

        if ($scope.SocietyAmendment.ChangeName && ($scope.SocietyAmendment.BRN == undefined || $scope.SocietyAmendment.BRN == '')) {
            Msg += (_language == 'en-us' ? 'Please Enter society BRN No. \n' : 'Please Enter society BRN No.\n');
            Msg += '<br />';
            ShowModelAlert("Society Amendment Details", Msg);
            return;
        } else if ($scope.SocietyAmendment.ChangeName && ($scope.SocietyAmendment.BRN.length < 16 || $scope.SocietyAmendment.BRN.length > 16)) {
            Msg += (_language == 'en-us' ? 'Please Enter 16 digit society BRN No. \n' : 'Please Enter 16 digit society BRN No. \n');
            Msg += '<br />';
            ShowModelAlert("Society Amendment Details", Msg);
            return;
        } else if ($scope.SocietyAmendment.ChangeName && $scope.ValidateBRNNo != $scope.SocietyAmendment.BRN) {
            Msg += (_language == 'en-us' ? 'Please Validate BRN No. \n' : 'Please Validate BRN No. \n');
            Msg += '<br />';
            ShowModelAlert("Society Amendment Details", Msg);
            return;
        }




        if ($("#txtByLawsAGMDate").val() == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Paased in Which AGM Date  \n' : 'जिसमें पास किया गया एजीएम दिनांक दर्ज करे\n');
            Msg += '<br />';
            ShowModelAlert("Society Amendment Details", Msg);
            return;
        }
        if (IsPermanentSave) {
            if ($scope.SocietyAmendment.ChangeName && ($scope.SocietyAmendment.BRN == undefined || $scope.SocietyAmendment.BRN == '')) {
                Msg += (_language == 'en-us' ? 'Please Enter society BRN No. \n' : 'Please Enter society BRN No.\n');
                Msg += '<br />';
            } else if ($scope.SocietyAmendment.ChangeName && ($scope.SocietyAmendment.BRN.length < 16 || $scope.SocietyAmendment.BRN.length > 16)) {
                Msg += (_language == 'en-us' ? 'Please Enter 16 digit society BRN No. \n' : 'Please Enter 16 digit society BRN No. \n');
                Msg += '<br />';
            } else if ($scope.SocietyAmendment.ChangeName && $scope.ValidateBRNNo != $scope.SocietyAmendment.BRN) {
                Msg += (_language == 'en-us' ? 'Please Validate BRN No. \n' : 'Please Validate BRN No. \n');
                Msg += '<br />';
            }


            if ($scope.SocietyAmendment.TotalMember == undefined || $scope.SocietyAmendment.TotalMember == '') {
                Msg += (_language == 'en-us' ? 'Please Enter Total members of society. \n' : 'Please Enter Total members of society. \n');
                Msg += '<br />';
            }
            if ($scope.SocietyAmendment.TotalPresantMember == undefined || $scope.SocietyAmendment.TotalPresantMember == '') {
                Msg += (_language == 'en-us' ? 'Please Enter Number of members present in the meeting \n' : 'Please Enter Number of members present in the meeting \n');
                Msg += '<br />';
            }
            if ($scope.SocietyAmendment.MeetingQuorum == undefined || $scope.SocietyAmendment.MeetingQuorum == '') {
                Msg += (_language == 'en-us' ? 'Please Enter Quorum of the meeting according to the law \n' : 'Please Enter Quorum of the meeting according to the law \n');
                Msg += '<br />';
            }
            if ($scope.SocietyAmendment.TotalVoteInSupport == undefined || $scope.SocietyAmendment.TotalVoteInSupport == '') {
                Msg += (_language == 'en-us' ? 'Please Enter How many members voted in favor of the amendment \n' : 'Please Enter How many members voted in favor of the amendment \n');
                Msg += '<br />';
            } if ($scope.SocietyAmendment.TotalVoteInOpposition == undefined || $scope.SocietyAmendment.TotalVoteInOpposition == '') {
                Msg += (_language == 'en-us' ? 'Please Enter How many members voted against the amendment \n' : 'Please Enter How many members voted against the amendment \n');
                Msg += '<br />';
            }



            if ($scope.SocietyAmendment.ChaneAddress) {

                if ($scope.SocietyRegisteredOffice.HouseBuildingNo == undefined || $scope.SocietyRegisteredOffice.HouseBuildingNo == "") {
                    Msg += (_language == 'en-us' ? 'Please Enter Address-House/Building Number  \n' : 'Please Enter Address-House/Building Number  \n\n');
                    Msg += '<br />';
                }

                if ($scope.SocietyRegisteredOffice.LaneStreetName == undefined || $scope.SocietyRegisteredOffice.LaneStreetName == "") {
                    Msg += (_language == 'en-us' ? 'Please Enter Address-Lane/Street Name  \n' : 'Please Enter Address-Lane/Street Name  \n\n');
                    Msg += '<br />';
                }

                if ($scope.SocietyRegisteredOffice.Locality == undefined || $scope.SocietyRegisteredOffice.Locality == "") {
                    Msg += (_language == 'en-us' ? 'Please Enter Address-Locality  \n' : 'Please Enter Address-Locality  \n\n');
                    Msg += '<br />';
                }

                if ($scope.SocietyRegisteredOffice.DIVISION_ID == undefined || $scope.SocietyRegisteredOffice.DIVISION_ID == "0") {
                    Msg += (_language == 'en-us' ? 'Please Enter Address-Division  \n' : 'Please Enter Address-Division  \n\n');
                    Msg += '<br />';
                }

                if ($scope.SocietyRegisteredOffice.DISTRICT_ID == undefined || $scope.SocietyRegisteredOffice.DISTRICT_ID == "0") {
                    Msg += (_language == 'en-us' ? 'Please Enter Address-District  \n' : 'Please Enter Address-District  \n\n');
                    Msg += '<br />';
                }

                if ($scope.SocietyRegisteredOffice.IsRural == undefined || ($scope.SocietyRegisteredOffice.IsRural == 1 && $scope.SocietyRegisteredOffice.IsRural == 2)) {
                    Msg += (_language == 'en-us' ? 'Please Choose Urban/Rural  \n' : 'Please Choose Urban/Rural  \n');
                    Msg += '<br />';
                }

                if ($scope.SocietyRegisteredOffice.IsRural != undefined && $scope.SocietyRegisteredOffice.IsRural == 1) {
                    if ($scope.SocietyRegisteredOffice.CITY_ID == undefined || $scope.SocietyRegisteredOffice.CITY_ID == "0") {
                        Msg += (_language == 'en-us' ? 'Please Enter Address-City  \n' : 'Please Enter Address-City  \n\n');
                        Msg += '<br />';
                    }
                    if ($scope.SocietyRegisteredOffice.WARD_ID == undefined || $scope.SocietyRegisteredOffice.WARD_ID == "0") {
                        Msg += (_language == 'en-us' ? 'Please Enter Address-Ward  \n' : 'Please Enter Address-Ward  \n\n');
                        Msg += '<br />';
                    }

                } else if ($scope.SocietyRegisteredOffice.IsRural != undefined && $scope.SocietyRegisteredOffice.IsRural == 2) {
                    if ($scope.SocietyRegisteredOffice.BLOCK_ID == undefined || $scope.SocietyRegisteredOffice.BLOCK_ID == "0") {
                        Msg += (_language == 'en-us' ? 'Please Enter Address-Block  \n' : 'Please Enter Address-Block  \n\n');
                        Msg += '<br />';
                    }
                    if ($scope.SocietyRegisteredOffice.GP_ID == undefined || $scope.SocietyRegisteredOffice.GP_ID == "0") {
                        Msg += (_language == 'en-us' ? 'Please Enter Address-Gram Panchayat  \n' : 'Please Enter Address-Gram Panchayat  \n\n');
                        Msg += '<br />';
                    }
                    if ($scope.SocietyRegisteredOffice.VILLAGE_ID == undefined || $scope.SocietyRegisteredOffice.VILLAGE_ID == "0") {
                        Msg += (_language == 'en-us' ? 'Please Enter Address- Village   \n' : 'Please Enter Address- Village   \n\n');
                        Msg += '<br />';
                    }
                }
                if ($scope.SocietyRegisteredOffice.PinCode == undefined || $scope.SocietyRegisteredOffice.PinCode == "0") {
                    Msg += (_language == 'en-us' ? 'Please Enter Address- PIN   \n' : 'Please Enter Address- PIN   \n\n');
                    Msg += '<br />';
                }


            }



            if ($scope.SocietyAmendment.ChaneExecutiveMember) {
                $scope.TableData10All
                if ($scope.TableData10All != undefined && $scope.TableData10All.length > 0 && $scope.TableData10All.filter(function (x) { return x.IsDelete }).length > 0) {
                    $scope.TableData10All.filter(function (x) { return x.IsDelete }).forEach(function (itm, indx) {
                        var index = $scope.TableData10All.findIndex(x => x === itm);
                        if ($scope.GetFileUploaded('flRemovalType_' + index) == '' || itm.RemovalType == '0') {
                            Msg += (_language == 'en-us' ? 'Please choose member removal reason with attachment file\n' : 'Please choose member removal reason with attachment file\n');
                            Msg += '<br />';
                        }
                    });
                }
            }

            if ($scope.SocietyAmendment.ChangeName) {
                if ($scope.GetFileUploaded('Notice_to_Registrar_Signed_by_Secretary_Seven_Members') == undefined || $scope.GetFileUploaded('Notice_to_Registrar_Signed_by_Secretary_Seven_Members') == "") {//$scope.ByeLawsSocietyFile == undefined || $scope.ByeLawsSocietyFile == '') {
                    Msg += $scope._language == 'en-us' ? '\nPlease Upload Copy Of Notice to Registrar Signed by Secretary & Seven Members' : '\nPlease Upload Copy Of Notice to Registrar Signed by Secretary & Seven Members';
                    Msg += '<br />';
                }
            }
            if ($scope.GetFileUploaded('Board_meeting_resolution_Notice_with_delivery_proof') == undefined || $scope.GetFileUploaded('Board_meeting_resolution_Notice_with_delivery_proof') == "") {//$scope.ByeLawsSocietyFile == undefined || $scope.ByeLawsSocietyFile == '') {
                Msg += $scope._language == 'en-us' ? '\nPlease Upload  Copy Of Board meeting resolution Notice with delivery proof' : '\nPlease Upload  Copy Of Board meeting resolution Notice with delivery proof';
                Msg += '<br />';
            }
            if ($scope.GetFileUploaded('Board_meeting_resolution_MOM_And_Attendance_Sheet') == undefined || $scope.GetFileUploaded('Board_meeting_resolution_MOM_And_Attendance_Sheet') == "") {//$scope.ByeLawsSocietyFile == undefined || $scope.ByeLawsSocietyFile == '') {
                Msg += $scope._language == 'en-us' ? '\nPlease Upload Copy Of Board meeting resolution MOM And Attendance Sheet' : '\nPlease Upload Copy Of Board meeting resolution MOM And Attendance Sheet';
                Msg += '<br />';
            }

            if ($scope.SocietyAmendment.AmendmentSection == 4) {
                if ($scope.GetFileUploaded('AGM_Notice_with_delivery_proof') == undefined || $scope.GetFileUploaded('AGM_Notice_with_delivery_proof') == "") {//$scope.ByeLawsSocietyFile == undefined || $scope.ByeLawsSocietyFile == '') {
                    Msg += $scope._language == 'en-us' ? '\nPlease Upload Copy Of AGM(Annual General meeting) Notice with delivery proof' : '\nPlease Upload Copy Of 1st SGM(Special General meeting) Notice with delivery proof';
                    Msg += '<br />';
                }
                if ($scope.GetFileUploaded('AGM_MOM_And_Attendance_Sheet') == undefined || $scope.GetFileUploaded('AGM_MOM_And_Attendance_Sheet') == "") {//$scope.AGM1File == undefined || $scope.AGM1File == '') {
                    Msg += $scope._language == 'en-us' ? '\nPlease Upload Copy Of AGM(Annual General meeting) MOM And Attendance Sheet' : '\nPlease Upload Copy Of 1st SGM(Special General meeting) MOM And Attendance Sheet';
                    Msg += '<br />';
                }

            } else {
                if ($scope.GetFileUploaded('1st_SGM_Notice_with_delivery_proof') == undefined || $scope.GetFileUploaded('1st_SGM_Notice_with_delivery_proof') == "") {//$scope.ByeLawsSocietyFile == undefined || $scope.ByeLawsSocietyFile == '') {
                    Msg += $scope._language == 'en-us' ? '\nPlease Upload Copy Of 1st SGM(Special General meeting) Notice with delivery proof' : '\nPlease Upload Copy Of 1st SGM(Special General meeting) Notice with delivery proof';
                    Msg += '<br />';
                }
                if ($scope.GetFileUploaded('1st_SGM_MOM_And_Attendance_Sheet') == undefined || $scope.GetFileUploaded('1st_SGM_MOM_And_Attendance_Sheet') == "") {//$scope.AGM1File == undefined || $scope.AGM1File == '') {
                    Msg += $scope._language == 'en-us' ? '\nPlease Upload Copy Of 1st SGM(Special General meeting) MOM And Attendance Sheet' : '\nPlease Upload Copy Of 1st SGM(Special General meeting) MOM And Attendance Sheet';
                    Msg += '<br />';
                }
                if ($scope.GetFileUploaded('2nd_SGM_Notice_with_delivery_proof') == undefined || $scope.GetFileUploaded('2nd_SGM_Notice_with_delivery_proof') == "") {//if ($scope.AGM2File == undefined || $scope.AGM2File == '') {
                    Msg += $scope._language == 'en-us' ? '\nPlease Upload Copy Of 2nd SGM(Special General meeting) Notice with delivery proof' : '\nPlease Upload Copy Of 2nd SGM(Special General meeting) Notice with delivery proof';
                    Msg += '<br />';
                }
                if ($scope.GetFileUploaded('2nd_SGM_MOM_And_Attendance_Sheet') == undefined || $scope.GetFileUploaded('2nd_SGM_MOM_And_Attendance_Sheet') == "") {//if ($scope.AGM3File == undefined || $scope.AGM3File == '') {
                    Msg += $scope._language == 'en-us' ? '\nPlease Upload Copy Of 2nd SGM(Special General meeting) MOM And Attendance Sheet' : '\nPlease Upload Copy Of 2nd SGM(Special General meeting) MOM And Attendance Sheet';
                    Msg += '<br />';
                }
                if ($scope.GetFileUploaded('Comparative_Document_Of_By_Laws') == undefined || $scope.GetFileUploaded('Comparative_Document_Of_By_Laws') == "") {//if ($scope.AGM3File == undefined || $scope.AGM3File == '') {
                    Msg += $scope._language == 'en-us' ? '\nPlease Upload Copy Of Comparative Document Of By-Laws' : '\nPlease Upload Copy Of Comparative Document Of By-Laws';
                    Msg += '<br />';
                }
            }


            if (Msg.length > 1) {
                Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + '<br />' + Msg;
                ShowModelAlert("Society Amendment Details", Msg);
                return;
            }
        }






        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var SocietyRegistrationData = [];
        if ($scope.SocietyAmendment.ChaneExecutiveMember) {

            $scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdEnc;

            if ($scope.TableData10All != undefined) {

                $scope.TableData10All.forEach(function (itm, indx) {
                    if (itm.Aadh == undefined || itm.Aadh == '') {

                    } else {
                        if ($scope.TableData10AllCopy.filter(function (x) {
                            return x.AdministrativeId == itm.AdministrativeId && x.AdministrativeId != 0 && (x.ContactNo != itm.ContactNo
                                || x.CategoryId != itm.CategoryId || x.POSTNAME != itm.POSTNAME || x.Occupation != itm.Occupation || x.Status != itm.Status)
                        }).length > 0) {
                            SocietyRegistrationData.push(
                                {
                                    "AdministrativeId": itm.NewAdministrativeId == undefined ? 0 : itm.NewAdministrativeId,
                                    "OldAdministrativeId": itm.AdministrativeId,
                                    "AADHAARNo": itm.AadharNo2,
                                    "Name": itm.Name,
                                    "FatherName": itm.FatherName,
                                    "ContactNo": itm.ContactNo,
                                    "DOB": itm.DOB.indexOf('-') > -1 ? itm.DOB.split("-").reverse().join("-") : itm.DOB.split("/").reverse().join("-"),
                                    "Gender": itm.Gender,//text
                                    "CategoryId": itm.CategoryId,
                                    "CatId": itm.CategoryId,
                                    "Occupation": itm.Occupation,
                                    "PostId": '0',
                                    "PostName": itm.POSTNAME,
                                    "Address": "",//$(this).find('td:eq(8)').find('textarea').val(),
                                    "Photo": itm.Photo,
                                    "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                    "IsActive": true,
                                    "UserType": "4",//Elected board Menbers
                                    "AdministratorDesignationId": "-1",
                                    "MEMBERTYPE": itm.Status ? "ELECTEDMEMBERS" : 'GENERALMEMBERS',
                                    "RequestType": 2,
                                    "RemovalType": itm.RemovalType
                                });

                        } else if ((itm.IsNew === true || itm.IsDelete === true || itm.AdministrativeId == 0)) {
                            SocietyRegistrationData.push(
                                {
                                    "AdministrativeId": itm.NewAdministrativeId == undefined ? 0 : itm.NewAdministrativeId,
                                    "OldAdministrativeId": itm.AdministrativeId,
                                    "AADHAARNo": itm.AadharNo2,
                                    "Name": itm.Name,
                                    "FatherName": itm.FatherName,
                                    "ContactNo": itm.ContactNo,
                                    "DOB": itm.DOB.indexOf('-') > -1 ? itm.DOB.split("-").reverse().join("-") : itm.DOB.split("/").reverse().join("-"),
                                    "Gender": itm.Gender,//text
                                    "CategoryId": itm.CategoryId,
                                    "CatId": itm.CategoryId,
                                    "Occupation": itm.Occupation,
                                    "PostId": '0',
                                    "PostName": itm.POSTNAME,
                                    "Address": "",//$(this).find('td:eq(8)').find('textarea').val(),
                                    "Photo": itm.Photo,
                                    "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                    "IsActive": true,
                                    "UserType": "4",//Elected board Menbers
                                    "AdministratorDesignationId": "-1",
                                    "MEMBERTYPE": itm.Status ? "ELECTEDMEMBERS" : 'GENERALMEMBERS',
                                    "RequestType": itm.NewAdministrativeId == 0 ? 3 : itm.IsDelete ? 1 : 3,
                                    "RemovalType": itm.RemovalType,
                                    "RemovalFile": itm.IsDelete ? $scope.GetFileUploaded('flRemovalType_' + indx) : ''
                                });
                        }
                    }
                });
            }
        }


        var AmendmentType = "";
        if ($scope.SocietyAmendment.ChangeByeLaws) {
            AmendmentType += (AmendmentType != "" ? "," : "") + "1";
        }
        if ($scope.SocietyAmendment.ChangeName) {
            AmendmentType += (AmendmentType != "" ? "," : "") + "2";
        }
        if ($scope.SocietyAmendment.ChaneAddress) {
            AmendmentType += (AmendmentType != "" ? "," : "") + "3";
        }
        if ($scope.SocietyAmendment.ChaneExecutiveMember) {
            AmendmentType += (AmendmentType != "" ? "," : "") + "4";
        }



        $scope.AmendmentDoc = [];

        $scope.AmendmentDoc.push({ DocumentType: "Notice_to_Registrar_Signed_by_Secretary_Seven_Members", DocumentName: $scope.GetFileUploaded("Notice_to_Registrar_Signed_by_Secretary_Seven_Members") });
        $scope.AmendmentDoc.push({ DocumentType: "Board_meeting_resolution_Notice_with_delivery_proof", DocumentName: $scope.GetFileUploaded("Board_meeting_resolution_Notice_with_delivery_proof") });
        $scope.AmendmentDoc.push({ DocumentType: "Board_meeting_resolution_MOM_And_Attendance_Sheet", DocumentName: $scope.GetFileUploaded("Board_meeting_resolution_MOM_And_Attendance_Sheet") });
        $scope.AmendmentDoc.push({ DocumentType: "1st_SGM_Notice_with_delivery_proof", DocumentName: $scope.GetFileUploaded("1st_SGM_Notice_with_delivery_proof") });
        $scope.AmendmentDoc.push({ DocumentType: "1st_SGM_MOM_And_Attendance_Sheet", DocumentName: $scope.GetFileUploaded("1st_SGM_MOM_And_Attendance_Sheet") });
        $scope.AmendmentDoc.push({ DocumentType: "2nd_SGM_Notice_with_delivery_proof", DocumentName: $scope.GetFileUploaded("2nd_SGM_Notice_with_delivery_proof") });
        $scope.AmendmentDoc.push({ DocumentType: "2nd_SGM_MOM_And_Attendance_Sheet", DocumentName: $scope.GetFileUploaded("2nd_SGM_MOM_And_Attendance_Sheet") });
        $scope.AmendmentDoc.push({ DocumentType: "Comparative_Document_Of_By_Laws", DocumentName: $scope.GetFileUploaded("Comparative_Document_Of_By_Laws") });

        $scope.AmendmentDoc.push({ DocumentType: "AGM_Notice_with_delivery_proof", DocumentName: $scope.GetFileUploaded("AGM_Notice_with_delivery_proof") });
        $scope.AmendmentDoc.push({ DocumentType: "AGM_MOM_And_Attendance_Sheet", DocumentName: $scope.GetFileUploaded("AGM_MOM_And_Attendance_Sheet") });
        var data = $scope.SocietyList.filter(function (x) { return x.SocietyIdENC.toString() === $scope.SocietyAmendment.SocietyIdEnc.split("~")[0] })[0];
        var postData = {
            AmendmentId: $scope.SocietyAmendment.AmendmentId,
            Section: $scope.SocietyAmendment.AmendmentSection,
            IsPermanentSave: IsPermanentSave,
            TotalMember: $scope.SocietyAmendment.TotalMember,//$scope.AGM1File)
            TotalPresantMember: $scope.SocietyAmendment.TotalPresantMember,//$s
            MeetingQuorum: $scope.SocietyAmendment.MeetingQuorum,// $scope.AGM3
            TotalVoteInSupport: $scope.SocietyAmendment.TotalVoteInSupport,// $
            TotalVoteInOpposition: $scope.SocietyAmendment.TotalVoteInOpposition,
            HouseBuildingNo: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.HouseBuildingNo != undefined ? $scope.SocietyRegisteredOffice.HouseBuildingNo : "",
            LaneStreetName: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.LaneStreetName != undefined ? $scope.SocietyRegisteredOffice.LaneStreetName : "",
            Locality: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.Locality != undefined ? $scope.SocietyRegisteredOffice.Locality : "",
            DIVISION_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.DIVISION_ID != undefined ? $scope.SocietyRegisteredOffice.DIVISION_ID : "0",
            DISTRICT_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.DISTRICT_ID != undefined ? $scope.SocietyRegisteredOffice.DISTRICT_ID : "0",
            IsRural: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural != undefined ? $scope.SocietyRegisteredOffice.IsRural : "0",
            CITY_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural == 'True' && $scope.SocietyRegisteredOffice.CITY_ID != undefined ? $scope.SocietyRegisteredOffice.CITY_ID : "0",
            WARD_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural == 'True' && $scope.SocietyRegisteredOffice.WARD_ID != undefined ? $scope.SocietyRegisteredOffice.WARD_ID : "0",
            BLOCK_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural == 'False' && $scope.SocietyRegisteredOffice.BLOCK_ID != undefined ? $scope.SocietyRegisteredOffice.BLOCK_ID : "0",
            GP_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural == 'False' && $scope.SocietyRegisteredOffice.GP_ID != undefined ? $scope.SocietyRegisteredOffice.GP_ID : "0",
            VILLAGE_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural == 'False' && $scope.SocietyRegisteredOffice.VILLAGE_ID != undefined ? $scope.SocietyRegisteredOffice.VILLAGE_ID : "0",
            PinCode: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.PinCode != undefined ? $scope.SocietyRegisteredOffice.PinCode : "0",
            ActId: $scope.ActId,
            ClassA: $scope.SocietyAmendment.TotalMembersClassA,
            ClassB: $scope.SocietyAmendment.TotalMembersClassB,
            ClassC: $scope.SocietyAmendment.TotalMembersClassC,
            SocietyDirectory: $scope.SocietyDirectory,

            SocietyIdEnc: Datasplit[0],
            SocietyDirectory: Datasplit[1],
            RegiYearId: $scope.SocietyAmendment.RegiYearId,
            ResolutionNo: $scope.SocietyAmendment.ResolutionNo,
            PaasedAGMDate: $("#txtByLawsAGMDate").val().split("/").reverse().join("-"),
            AmendmentType: AmendmentType,
            Name: $scope.SocietyAmendment.ChangeName ? $scope.ValidateSocietyName : "",
            BRN: $scope.SocietyAmendment.ChangeName ? $scope.SocietyAmendment.BRN : "",
            Remark: $scope.SocietyAmendment.Remark == undefined ? "" : $scope.SocietyAmendment.Remark,
            SocietyTypeId: data.SocietyTypeId
        }
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Citizen/InsertSocietyNameChange", JSON.stringify({ SocietyRegistrationData: postData, SocietyAmendment: postData, AmendmentDocuments: $scope.AmendmentDoc, MemberData: SocietyRegistrationData })).
            success(function(data, status) {
                $(".blockUI").fadeOut("slow");
                var AltMsg = "";
                var len = JSON.stringify(data).length;
                if (len > 10) {
                    if (data.data.indexOf("NotValidName") > -1) {
                        $(".SaveSocietyInfo").show();
                        $(".se-pre-con").fadeOut("slow");
                        var ValidNameCheck = "आप जिस सोसाइटी के नाम में संसोधन कर रहे हैं उस सोसाइटी के नाम में कुछ शब्द है जो मान्य नहीं हैं | कृपया सोसाइटी का नाम सही करके फिर से आवेदन करें | अमान्य शब्द " + data.data.replace("NotValidName", "");
                        ShowOkMessage("Warning", ValidNameCheck, function() { return; });
                    }
                } else {
                    var Result = parseInt(data, 10);
                    if (Result > 0) {
                        AltMsg = (_language == 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                        alert(AltMsg);
                        // if (IsPermanentSave) {
                        location.reload();
                        //}
                    }
                    else if (Result == -2) {
                        AltMsg = (_language == 'en-us' ? 'Society Name Already Exist.' : 'Society Name Already Exist.');
                        alert(AltMsg);
                    } else if (Result == -3) {
                        AltMsg = (_language == 'en-us' ? 'Society old name and new name is same.' : 'Society old name and new name is same.');
                        alert(AltMsg);
                    }

                    else {
                        AltMsg = (_language == 'en-us' ? 'Some Error Occurred please try after some time.' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                        alert(AltMsg);
                    }
                }

            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                alert('An Error Occurred Please Try Again Later');
                location.reload();
            });
    };

    $scope.ResetByLawsModal = function() {

        $scope.SocietyAmendment.RegiYearId = '0';
        $scope.SocietyAmendment.ResolutionNo = '';
        $scope.SocietyAmendment.Name = '';
        $("#txtByLawsAGMDate").val('');
        $scope.ByeLawsSocietyFile = '';
        $scope.AGM1File = '';
        $scope.AGM2File = '';
        $scope.AGM3File = '';
        $('#flNameAGM1PDF').val('');
        $('#flNameAGM2PDF').val('');
        $('#flNameAGM3PDF').val('');
        $('#flAGM1PDF').val('');
        $('#flAGM2PDF').val('');
        $('#flAGM3PDF').val('');
        $("#txtNameChangeAGMDate").val("");
        $('span[class="filesize"]').html("");
        $('#flMembersListExcelFile').val("");
        $scope.MembersListExcelFile = '';
    }

    $scope.SaveSocietyNameChange = function() {
        var Msg = '';

        if ($scope.SocietyAmendment.SocietyIdEnc == undefined || $scope.SocietyAmendment.SocietyIdEnc == '0') {
            Msg += (_language == 'en-us' ? 'Please Select Society \n' : 'सोसाइटी का चयन करे\n');
            Msg += '<br />';
        }
        if ($scope.SocietyAmendment.RegiYearId == undefined || $scope.SocietyAmendment.RegiYearId == '0') {
            Msg += (_language == 'en-us' ? 'Please Select Registered Year \n' : 'पंजीकृत वर्ष का चयन करे\n');
            Msg += '<br />';
        }
        if ($scope.SocietyAmendment.ResolutionNo == undefined || $scope.SocietyAmendment.ResolutionNo == '') {
            Msg += (_language == 'en-us' ? 'Please Enter ResolutionNo \n' : 'संकल्प नं दर्ज करे\n');
            Msg += '<br />';
        }

        // if ($scope.IsChangeName != undefined && $scope.IsChangeName) {
        if ($scope.SocietyAmendment.Name == undefined || $scope.SocietyAmendment.Name == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Name \n' : 'नाम दर्ज करे\n');
            Msg += '<br />';
        }
        // }
        if ($("#txtNameChangeAGMDate").val() == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Paased in Which AGM Date  \n' : 'जिसमें पास किया गया एजीएम दिनांक दर्ज करे\n');
            Msg += '<br />';
        }
        if ($scope.GetFileUploaded('flBylawsFileName') == undefined || $scope.GetFileUploaded('flBylawsFileName') == "") {//if ($scope.ByeLawsSocietyFile == undefined || $scope.ByeLawsSocietyFile == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload ByeLaws File(PDF)' : '\nकृपया बाय लॉ अपलोड करें(पीडीएफ) |';
            Msg += '<br />';
        }
        //if ($scope.ResolutionFile == undefined) {
        //    Msg += $scope._language == 'en-us' ? '\nPlease Upload Resolution File(PDF)' : '\nकृपया संकल्प पीडीएफ अपलोड करें(पीडीएफ) |';
        //    Msg += '<br />';
        //}
        if ($scope.GetFileUploaded('flNameAGM1PDF') == undefined || $scope.GetFileUploaded('flNameAGM1PDF') == "") {//if ($scope.AGM1File == undefined || $scope.AGM1File == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload AGM I File(PDF)' : '\nकृपया एजीएम I पीडीएफ अपलोड करें(पीडीएफ) |';
            Msg += '<br />';
        }
        if ($scope.GetFileUploaded('flNameAGM2PDF') == undefined || $scope.GetFileUploaded('flNameAGM2PDF') == "") {//if ($scope.AGM2File == undefined || $scope.AGM2File == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload AGM II File(PDF)' : '\nकृपया एजीएम II पीडीएफ अपलोड करें(पीडीएफ) |';
            Msg += '<br />';
        }
        if ($scope.GetFileUploaded('flNameAGM3PDF') == undefined || $scope.GetFileUploaded('flNameAGM3PDF') == "") {//if ($scope.AGM3File == undefined || $scope.AGM3File == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload AGM III (PDF)' : '\nकृपया एजीएम III पीडीएफ अपलोड करें(पीडीएफ) |';
            Msg += '<br />';
        }
        if (Msg.length > 1) {
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + '<br />' + Msg;
            ShowModelAlert("Society Amendment Details", Msg);
            return;
        }
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        formData.append("SocietyIdEnc", Datasplit[0]);
        formData.append("SocietyDirectory", Datasplit[1]);
        formData.append("RegiYearId", $scope.SocietyAmendment.RegiYearId);
        formData.append("ResolutionNo", $scope.SocietyAmendment.ResolutionNo);
        formData.append("PaasedAGMDate", $("#txtByLawsAGMDate").val().split("/").reverse().join("-"));
        formData.append("ByeLawsFile", $scope.GetFileUploaded('flBylawsFileName'));//$scope.ByeLawsSocietyFile);
        //formData.append("ResolutionFile", $scope.ResolutionFile);
        formData.append("AGM1File", $scope.GetFileUploaded('flNameAGM1PDF'));//$scope.AGM1File);
        formData.append("AGM2File", $scope.GetFileUploaded('flNameAGM2PDF'));// $scope.AGM2File);
        formData.append("AGM3File", $scope.GetFileUploaded('flNameAGM3PDF'));//$scope.AGM3File);
        formData.append("Name", $scope.SocietyAmendment.Name);
        formData.append("Remark", $scope.SocietyAmendment.Remark == undefined ? "" : $scope.SocietyAmendment.Remark);
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Citizen/InsertSocietyNameChange", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function(data, status) {
                $(".blockUI").fadeOut("slow");
                var AltMsg = "";
                var Result = parseInt(data, 10);
                if (Result > 0) {
                    AltMsg = (_language == 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                    alert(AltMsg);
                    location.reload();
                }
                else if (Result == -2) {
                    AltMsg = (_language == 'en-us' ? 'Data Already Exist.' : 'डेटा पहले से मौजूद है');
                }
                else {
                    AltMsg = (_language == 'en-us' ? 'Some Error Occurred please try after some time.' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                }

            }).
            error(function(data, status) {
                $(".blockUI").fadeOut("slow");
                alert('An Error Occurred Please Try Again Later');
                location.reload();
            });
    };


    $scope.SaveAGM = function() {
        var Msg = '';

        if ($scope.SocietyAmendment.SocietyIdEnc == undefined || $scope.SocietyAmendment.SocietyIdEnc == '0') {
            Msg += (_language == 'en-us' ? 'Please Select Society \n' : 'सोसाइटी  का चयन करे\n');
            Msg += '<br />';
        }
        if ($("#txtAGMByeLawsDate").val() == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Mandated Date as Per Bye Laws  \n' : 'बाय लॉ की दिनांक दर्ज करे\n');
            Msg += '<br />';
        }
        if ($("#txtAGMHeldDate").val() == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Date on Which Meeting Held  \n' : 'बैठक की दिनांक दर्ज करे\n');
            Msg += '<br />';
        }

        if ($scope.AGM_PDF == undefined) {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload Meeting PDF' : '\nकृपया बैठक की पीडीएफ अपलोड करें |';
            Msg += '<br />';
        }
        if (Msg.length > 1) {
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + '<br />' + Msg;
            ShowModelAlert("Society Amendment Details", Msg);
            return;
        }
        // $scope.SocietyAmendment.Is_AGM_BODM = true;
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        formData.append("SocietyIdEnc", Datasplit[0]);
        formData.append("SocietyDirectory", Datasplit[1]);
        formData.append("ByeLawsDate", $("#txtAGMByeLawsDate").val().split("/").reverse().join("-"));
        formData.append("MeetingDate", $("#txtAGMHeldDate").val().split("/").reverse().join("-"));
        formData.append("MeetingPDF", $scope.AGM_PDF);
        formData.append("Is_AGM_BODM", true);
        formData.append("Remark", $scope.SocietyAmendment.Remark);

        $http.post(SiteUrl + "/Citizen/InsertAGM_BODM", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function(data, status) {
                var AltMsg = "";
                var Result = parseInt(data, 10);
                if (Result > 0) {
                    AltMsg = (_language == 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                }
                else if (Result == -2) {
                    AltMsg = (_language == 'en-us' ? 'Data Already Exist.' : 'डेटा पहले से मौजूद है');
                }
                else {
                    AltMsg = (_language == 'en-us' ? 'Some Error Occurred please try after some time.' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                }
                alert(AltMsg);
                location.reload();
            }).
            error(function(data, status) {
                alert('An Error Occurred Please Try Again Later');
                location.reload();
            });
    };

    $scope.SaveBODAM = function() {
        var Msg = '';

        if ($scope.SocietyAmendment.SocietyIdEnc == undefined || $scope.SocietyAmendment.SocietyIdEnc == '0') {
            Msg += (_language == 'en-us' ? 'Please Select Society \n' : 'सोसाइटी का चयन करे\n');
            Msg += '<br />';
        }
        if ($("#txtBODByeLawsDate").val() == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Mandated Date as Per Bye Laws  \n' : 'बाय लॉ की दिनांक दर्ज करे\n');
            Msg += '<br />';
        }
        if ($("#txtBODMeetingHeldDate").val() == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Date on Which Meeting Held  \n' : 'बैठक की दिनांक दर्ज करे\n');
            Msg += '<br />';
        }

        if ($scope.BODAGM_PDF == undefined) {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload Meeting PDF' : '\nकृपया बैठक की पीडीएफ अपलोड करें |';
            Msg += '<br />';
        }
        if (Msg.length > 1) {
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + '<br />' + Msg;
            ShowModelAlert("Society Amendment Details", Msg);
            return;
        }
        // $scope.SocietyAmendment.Is_AGM_BODM = true;
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        //alert($("#txtBODByeLawsDate").val() + "-----" + $("#txtBODMeetingHeldDate").val());
        var formData = new FormData();
        formData.append("SocietyIdEnc", Datasplit[0]);
        formData.append("SocietyDirectory", Datasplit[1]);
        formData.append("ByeLawsDate", $("#txtBODByeLawsDate").val().split("/").reverse().join("-"));
        formData.append("MeetingDate", $("#txtBODMeetingHeldDate").val().split("/").reverse().join("-"));
        formData.append("MeetingPDF", $scope.BODAGM_PDF);
        formData.append("Is_AGM_BODM", false);
        formData.append("Remark", $scope.SocietyAmendment.Remark);

        $http.post(SiteUrl + "/Citizen/InsertAGM_BODM", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function(data, status) {
                var AltMsg = "";
                var Result = parseInt(data, 10);
                if (Result > 0) {
                    AltMsg = (_language == 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                }
                else if (Result == -2) {
                    AltMsg = (_language == 'en-us' ? 'Data Already Exist.' : 'डेटा पहले से मौजूद है');
                }
                else {
                    AltMsg = (_language == 'en-us' ? 'Some Error Occurred please try after some time.' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                }
                alert(AltMsg);
                location.reload();
            }).
            error(function(data, status) {
                alert('An Error Occurred Please Try Again Later');
                location.reload();
            });
    };



    $scope.SaveMemberList = function() {
        if ($scope.ActId != "7") {
            
            var SocietyRegistrationData = [];
            var formData = new FormData();
            formData.append("ActId", $scope.ActId);
            // formData.append("EMitraTransactionId", $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId);
            formData.append("ClassA", $scope.SocietyAmendment.TotalMembersClassA);
            formData.append("ClassB", $scope.SocietyAmendment.TotalMembersClassB);
            formData.append("ClassC", $scope.SocietyAmendment.TotalMembersClassC);
            formData.append("SocietyDirectory", $scope.SocietyDirectory);
            $scope.IsAdministrative = 'Board';
            $scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdEnc;

            if ($scope.IsAdministrative == 'Board') {
                var Count = 0;

                if ($scope.TableData10All != undefined) {

                    $scope.TableData10All.forEach(function(itm, indx) {
                        if ($scope.TableData10AllCopy.filter(function(x) {
                            return x.AdministrativeId === itm.AdministrativeId && (x.ContactNo != itm.ContactNo
                                || x.CategoryId != itm.CategoryId || x.POSTNAME != itm.POSTNAME || x.Occupation != itm.Occupation)
                        }).length > 0) {
                            SocietyRegistrationData.push(
                                {
                                    "AdministrativeId": itm.AdministrativeId,
                                    "AADHAARNo": itm.AadharNo2,
                                    "Name": itm.Name,
                                    "ContactNo": itm.ContactNo,
                                    "DOB": itm.DOB.indexOf('-') > -1 ? itm.DOB.split("-").reverse().join("-") : itm.DOB.split("/").reverse().join("-"),
                                    "Gender": itm.Gender,//text
                                    "CategoryId": itm.CategoryId,
                                    "CatId": itm.CategoryId,
                                    "Occupation": itm.Occupation,
                                    "PostId": '0',
                                    "PostName": itm.POSTNAME,
                                    "Address": "",//$(this).find('td:eq(8)').find('textarea').val(),
                                    "Photo": itm.Photo,
                                    "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                    "IsActive": true,
                                    "UserType": "4",//Elected board Menbers
                                    "AdministratorDesignationId": "-1",
                                    "MEMBERTYPE": itm.Status ? "ELECTEDMEMBERS" : 'GENERALMEMBERS',
                                    "RequestType": 2,
                                    "RemovalType": itm.RemovalType
                                });

                        } else if ((itm.IsNew === true || itm.IsDelete === true)) {
                            SocietyRegistrationData.push(
                                {
                                    "AdministrativeId": itm.AdministrativeId,
                                    "AADHAARNo": itm.AadharNo2,
                                    "Name": itm.Name,
                                    "ContactNo": itm.ContactNo,
                                    "DOB": itm.DOB.indexOf('-') > -1 ? itm.DOB.split("-").reverse().join("-") : itm.DOB.split("/").reverse().join("-"),
                                    "Gender": itm.Gender,//text
                                    "CategoryId": itm.CategoryId,
                                    "CatId": itm.CategoryId,
                                    "Occupation": itm.Occupation,
                                    "PostId": '0',
                                    "PostName": itm.POSTNAME,
                                    "Address": "",//$(this).find('td:eq(8)').find('textarea').val(),
                                    "Photo": itm.Photo,
                                    "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                    "IsActive": true,
                                    "UserType": "4",//Elected board Menbers
                                    "AdministratorDesignationId": "-1",
                                    "MEMBERTYPE": itm.Status ? "ELECTEDMEMBERS" : 'GENERALMEMBERS',
                                    "RequestType": itm.IsDelete ? 1 : 3,
                                    "RemovalType": itm.RemovalType
                                });
                        }
                    });
                }
                if ($scope.TableData10NomiAll != undefined) {
                    $scope.TableData10NomiAll.forEach(function(itm, indx) {
                        if ($scope.TableData10NomiAllCopy.filter(function(x) {
                            return x.AdministrativeId === itm.AdministrativeId && (x.ContactNo != itm.ContactNo
                                || x.CategoryId != itm.CategoryId || x.POSTNAME != itm.POSTNAME || x.Occupation != itm.Occupation)
                        }).length > 0) {
                            SocietyRegistrationData.push(
                                {
                                    "AdministrativeId": itm.AdministrativeId,
                                    "AADHAARNo": itm.AadharNo2,
                                    "Name": itm.Name,
                                    "ContactNo": itm.ContactNo,
                                    "DOB": itm.DOB.indexOf('-') > -1 ? itm.DOB.split("-").reverse().join("-") : itm.DOB.split("/").reverse().join("-"),
                                    "Gender": itm.Gender,//text
                                    "CategoryId": itm.CategoryId1,
                                    "Occupation": itm.Occupation,
                                    "PostId": '0',
                                    "PostName": itm.POSTNAME,
                                    "Address": "",//$(this).find('td:eq(8)').find('textarea').val(),
                                    "Photo": itm.Photo,
                                    "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                    "IsActive": true,
                                    "UserType": "4",//Elected board Menbers
                                    "AdministratorDesignationId": "-1",
                                    "MEMBERTYPE": itm.Status ? "ELECTEDMEMBERS" : 'GENERALMEMBERS',
                                    "RequestType": 2,
                                    "RemovalType": itm.RemovalType
                                });
                        } else if ((itm.IsNew === true || itm.IsDelete === true)) {
                            SocietyRegistrationData.push(
                                {
                                    "AdministrativeId": itm.AdministrativeId,
                                    "AADHAARNo": itm.AadharNo2,
                                    "Name": itm.Name,
                                    "ContactNo": itm.ContactNo,
                                    "DOB": itm.DOB.indexOf('-') > -1 ? itm.DOB.split("-").reverse().join("-") : itm.DOB.split("/").reverse().join("-"),
                                    "Gender": itm.Gender,//text
                                    "CategoryId": itm.CategoryId1,
                                    "Occupation": itm.Occupation,
                                    "PostId": '0',
                                    "PostName": itm.POSTNAME,
                                    "Address": "",//$(this).find('td:eq(8)').find('textarea').val(),
                                    "Photo": itm.Photo,
                                    "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                    "IsActive": true,
                                    "UserType": "4",//Elected board Menbers
                                    "AdministratorDesignationId": "-1",
                                    "MEMBERTYPE": itm.Status ? "ELECTEDMEMBERS" : 'GENERALMEMBERS',
                                    "RequestType": itm.IsDelete ? 1 : 3,
                                    "RemovalType": itm.RemovalType
                                });
                        }
                    });
                }
            }
        }
        else {

            if ($scope.ValidateNominatedMember(1, true)) {
                var SocietyRegistrationData = [];
                var formData = new FormData();
                formData.append("ActId", $scope.ActId);
                //formData.append("EMitraTransactionId", $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId);



                var formDataLocal = new FormData();

                formData.append("ClassA", 0);
                formData.append("ClassB", 0);
                formData.append("ClassC", 0);
                formData.append("SocietyDirectory", $scope.SocietyDirectory);
                //अध्यक्ष
                $scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdEnc;
                if ($scope.TableData10AllSports != undefined) {

                    $scope.TableData10AllSports.forEach(function(itm, indx) {
                        if ($scope.TableData10AllSportsCopy.filter(function(x) {
                            return x.AdministrativeId === itm.AdministrativeId && (x.ContactNo != itm.ContactNo
                                || x.CategoryId != itm.CategoryId || x.POSTNAME != itm.POSTNAME || x.Occupation != itm.Occupation)
                        }).length > 0) {
                            SocietyRegistrationData.push(
                                {
                                    "AdministrativeId": item.AdministrativeId,
                                    "AADHAARNo": item.AadharNo2,
                                    "Name": item.Name,
                                    "ContactNo": item.ContactNo,
                                    "DOB": item.DOB.indexOf('-') > -1 ? item.DOB.split("-").reverse().join("-") : item.DOB.split("/").reverse().join("-"),
                                    "Gender": item.Gender,//text
                                    "CategoryId": item.CategoryId1,
                                    "CatId": item.CategoryId1,
                                    "Occupation": item.Occupation,
                                    "PostId": '0',
                                    "PostName": item.POSTNAME,
                                    "Address": "",//$(this).find('td:eq(8)').find('textarea').val(),
                                    "Photo": item.Photo,
                                    "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                    "IsActive": true,
                                    "UserType": "4",//Elected board Menbers
                                    "AdministratorDesignationId": "-1",
                                    "MEMBERTYPE": item.Status ? "ELECTEDMEMBERS" : 'GENERALMEMBERS',
                                    "RequestType": 2,
                                    "RemovalType": item.RemovalType
                                });
                        } else if ((itm.IsNew === true || itm.IsDelete === true)) {
                            SocietyRegistrationData.push(
                                {
                                    "AdministrativeId": item.AdministrativeId,
                                    "AADHAARNo": item.AadharNo2,
                                    "Name": item.Name,
                                    "ContactNo": item.ContactNo,
                                    "DOB": item.DOB.indexOf('-') > -1 ? item.DOB.split("-").reverse().join("-") : item.DOB.split("/").reverse().join("-"),
                                    "Gender": item.Gender,//text
                                    "CategoryId": item.CategoryId1,
                                    "CatId": item.CategoryId1,
                                    "Occupation": item.Occupation,
                                    "PostId": '0',
                                    "PostName": item.POSTNAME,
                                    "Address": "",//$(this).find('td:eq(8)').find('textarea').val(),
                                    "Photo": item.Photo,
                                    "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                    "IsActive": true,
                                    "UserType": "4",//Elected board Menbers
                                    "AdministratorDesignationId": "-1",
                                    "MEMBERTYPE": item.Status ? "ELECTEDMEMBERS" : 'GENERALMEMBERS',
                                    "RequestType": item.IsDelete ? 1 : 3
                                });

                        }
                    });
                }

            }
        }
        formData.append("DataArray", JSON.stringify(SocietyRegistrationData));
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/citizen/SaveSocietyAdministrativeDetailsAmendment", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function(data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data == "1") {
                    ShowModelAlert("Society Amendment Details", "Saved Successfully");
                    window.location = "../Citizen/SocietyAmendment"
                } else {
                    alert("There is some problem while submiting data. Please try after some time.")
                }
            }).
            error(function(data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.DistrictList = [];
            });


    }


    $scope.RemoveRowAdminiAct2001 = function(index, ListType) {
        ShowAlertMessage("Alert", _language == 'en-us' ? 'Are you sure you want to delete this Member?' : 'क्या आप वाकई इस सदस्य को हटाना चाहते हैं?', function() {
            if (ListType == 'TableData10All') {
                $scope.TableData10All[index].IsDelete = true;
                if ($scope.TableData10All[index].oldAdministrativeId == 0) {
                    $scope.TableData10All.splice(index, 1);
                }

            } else if (ListType == 'TableData10AllSports') {
                $scope.TableData10AllSports[index].IsDelete = true;
                if ($scope.TableData10AllSports[index].oldAdministrativeId == 0) {
                    $scope.TableData10AllSports.splice(index, 1);
                }

            } else if (ListType == 'TableData10NomiAll') {
                $scope.TableData10NomiAll[index].IsDelete = true;
                if ($scope.TableData10NomiAll[index].oldAdministrativeId == 0) {
                    $scope.TableData10NomiAll.splice(index, 1);
                }
            }

            // $scope.TableData10NomiAll[index].IsDelete

            $scope.$apply();
        });
    }

    $scope.UndoRemoveRowAdminiAct2001 = function(index, ListType) {
        // ShowAlertMessage("Alert", _language == 'en-us' ? 'Are you sure you want to delete this Member?' : 'क्या आप वाकई इस सदस्य को हटाना चाहते हैं?', function () {
        //$scope.TableData10All[index].IsDelete = false;

        if (ListType == 'TableData10All') {
            $scope.TableData10All[index].IsDelete = false;
        } else if (ListType == 'TableData10AllSports') {
            $scope.TableData10AllSports[index].IsDelete = false;
        } else if (ListType == 'TableData10NomiAll') {
            $scope.TableData10NomiAll[index].IsDelete = false;
        }
        $scope.$apply();
        // });
    }


    //start aadhar verify
    $scope.Otpsend = function() {
        if ($('#SSOAuth_chkConsent').prop("checked")) {
            $scope.messagebox = false;
            $scope.FillOtp = true
            //SEND OTP
            $scope.GenrateOTP();
        }
        else {
            $scope.messagebox = true;
            $scope.FillOtp = false;
        }


    };
    $scope.ClosePopUp = function() {
        $scope.messagebox = false;

    };
    $scope.CloseOtpBox = function() {
        $scope.messagebox = false;
        $scope.OtpDilogBox = false;
        $scope.FillOtp = false;
        $('#ssoauthloading').hide();
        $("#SSOAuth_chkConsent").prop("checked", false);
    };
    $scope.OpenOtpPopUp = function(AADHAARNo, FOR, TRID) {
        if (AADHAARNo.length < 12 || AADHAARNo.length > 12) {
            alert('Invalid Aadhaar Number');
            return;
        }
        var arrylist = AADHAARNo.split('');
        $scope.MaskingAADHAARNo = arrylist[0] + arrylist[1] + 'XXXXXXXX' + arrylist[10] + arrylist[11];
        $scope.AADHAARNo = AADHAARNo;
        $scope.messagebox = false;
        $scope.OtpDilogBox = true;
        $scope.FillOtp = false;
        $scope.FOR = FOR;
        $scope.CurrentRow = TRID;
        if ($scope.FOR == 'CEO') {
            $('#txtCEOAADHAARNo').attr("disabled", "disabled");
        }
        if ($scope.FOR == 'CHAIRMAN') {
            $('#txtChairmanAADHAARNo').attr("disabled", "disabled");
        }
        if ($scope.FOR == 'APPOINTED') {
            $('#txtAppointedAADHAARNo').attr("disabled", "disabled");
        }
        if ($scope.FOR == 'lIQUIDATOR') {
            $('#txtliquidatorAADHAARNo').attr("disabled", "disabled");
        }
        if ($scope.FOR == 'SPCHAIRMAN') {
            $('#txtSPCEOAADHAARNo').attr("disabled", "disabled");
        }
        if ($scope.FOR == 'SPSECRETARY') {
            $('#txtSecretaryAadhar').attr("disabled", "disabled");
        }
        if ($scope.FOR == 'TREASURER') {
            $('#txtTreasurerAadhar').attr("disabled", "disabled");
        }

    };
    $scope.OpenOtpPopUpOther = function(AADHAARNo, FOR, TRID, ListType) {
        // alert(AADHAARNo+"--"+ FOR+"--"+ TRID)
        //  alert($('#' + TRID).find('td:eq(1)').find('input').val());
        $('#OTPBOX').css("display", "block");
        if (AADHAARNo.length > 12) {
            alert('Invalid Aadhaar Number');
            return;
        }
        $("#SSOAuth_chkConsent").prop("checked", false);
        $('#SSOAuth_OtpText').val('');
        $('#ssoauthloading').hide();
        var arrylist = AADHAARNo.split('');

        $scope.MaskingAADHAARNo = arrylist[0] + arrylist[1] + 'XXXXXXXX' + arrylist[10] + arrylist[11];
        $scope.AADHAARNo = AADHAARNo;
        var num = $scope.AADHAARNo;
        if (isNaN(num)) {
            alert("Please enter only number in aadhar.");
            return;
            //document.write(num1 + " is not a number <br/>");
        } else {

            //$scope.messagebox = false;
            //$scope.OtpDilogBox = true;
            //$scope.FillOtp = false;
            $scope.FOR = FOR;
            $scope.CurrentRow = TRID;

            AadhaarAuth_v1($scope.AADHAARNo, $http, "OTP-BIO", false, "", $scope.SocietyDirectory, function(data) {

                if (ListType == 'TableData10All') {
                    $scope.TableData10All[TRID].IsValidate = true;
                } else if (ListType == 'TableData10AllSports') {
                    $scope.TableData10AllSports[TRID].IsValidate = true;
                } else if (ListType == 'TableData10NomiAll') {
                    $scope.TableData10NomiAll[TRID].IsValidate = true;
                }




                // $scope.$apply();
                if ($scope.FOR == 'CEO') {
                    $scope.SocietyRegistration.CEOName = data.Name;
                    $scope.SocietyRegistration.CEOGender = data.Gender;
                    $scope.SocietyRegistration.CEODOB = data.DOB;
                    $scope.CEOSocietyFile = data.Photo;
                    $scope.SocietyRegistration.CEOSocietyFile = data.Photo;
                    $scope.CloseOtpBox();
                    $scope.CEOProfile = true;
                    $scope.CEORemove = true;
                    $scope.GETCEO = false;
                }
                else if ($scope.FOR == 'CHAIRMAN') {
                    $scope.SocietyRegistration.ChairmanName = data.Name;
                    $scope.SocietyRegistration.ChairmanGender = data.Gender;
                    $scope.SocietyRegistration.ChairmanDOB = data.Photo;
                    $scope.ChairmanFile = data.Photo;
                    $scope.SocietyRegistration.ChairmanFile = data.Phone;
                    $scope.CloseOtpBox();
                    $scope.ChairmanProfile = true;
                    $scope.CHAIRMANRemove = true;
                    $scope.GETCHAIRMAN = false;
                }
                else if ($scope.FOR == 'APPOINTED') {
                    $scope.SocietyRegistration.AppointedName = data.Name;
                    $scope.SocietyRegistration.AppointedGender = data.Gender;
                    $scope.SocietyRegistration.AppointedDOB = data.DOB;
                    $scope.AppointedFile = data.Photo;
                    $scope.SocietyRegistration.AppointedFile = data.Photo;
                    $scope.CloseOtpBox();
                    $scope.AdministratorProfile = true;
                    $scope.APPOINTEDRemove = true;
                    $scope.GETAPPOINTED = false;
                }
                else if ($scope.FOR == 'lIQUIDATOR') {
                    $scope.SocietyRegistration.liquidatorName = data.Name;
                    $scope.SocietyRegistration.liquidatorGender = data.Gender;
                    $scope.SocietyRegistration.liquidatorDOB = data.DOB;
                    $scope.liquidatorFile = data.Photo;
                    $scope.SocietyRegistration.liquidatorFile = data.Photo;
                    $scope.CloseOtpBox();
                    $scope.liquidatorProfile = true;
                    $scope.lIQUIDATORRemove = true;
                    $scope.GETlIQUIDATOR = false;
                }
                else if ($scope.FOR == 'SPCHAIRMAN') {
                    $scope.SocietyRegistration.CEOName = data.Name;
                    $scope.SocietyRegistration.CEOGender = data.Gender;
                    $scope.SocietyRegistration.CEODOB = data.DOB;
                    $scope.CEOSocietyFile = data.Photo;
                    $scope.SocietyRegistration.CEOSocietyFile = data.Photo;
                    $scope.CloseOtpBox();
                    $scope.CEOSportsProfile = true;
                    $scope.SPCHAIRMANRemove = true;
                    $scope.GETSPCHAIRMAN = false;
                }
                else if ($scope.FOR == 'SPSECRETARY') {
                    $scope.SocietyRegistration.ChairmanName = data.Name;
                    $scope.SocietyRegistration.ChairmanGender = data.Gender;
                    $scope.SocietyRegistration.ChairmanDOB = data.DOB;
                    $scope.ChairmanFile = data.Photo;
                    $scope.SocietyRegistration.ChairmanFile = data.Photo;
                    $scope.CloseOtpBox();
                    $scope.ChairmanSportsProfile = true;
                    $scope.SPSECRETARYRemove = true;
                    $scope.GETSPSECRETARY = false;
                }
                else if ($scope.FOR == 'TREASURER') {
                    $scope.SocietyRegistration.AppointedName = data.Name;
                    $scope.SocietyRegistration.AppointedGender = data.Gender;
                    $scope.SocietyRegistration.AppointedDOB = date.DOB;
                    $scope.ChairmanFile = data.Photo;
                    $scope.SocietyRegistration.AppointedFile = data.Photo;
                    $scope.AppointedFile = data.Photo;
                    $scope.CloseOtpBox();
                    $scope.TreasurerProfile = true;
                    $scope.TREASURERRemove = true;
                    $scope.GETTREASURER = false;
                }
                else {
                    if (ListType == 'TableData10All') {
                        $scope.TableData10All[TRID].Name = data.Name;
                        $scope.TableData10All[TRID].DOB = data.DOB.replace(/'-'/g, '/');
                        $scope.TableData10All[TRID].Gender = data.Gender == 'M' ? 'MALE' : 'FEMALE';
                        $scope.TableData10All[TRID].Photo = data.Photo;
                        $scope.TableData10All[TRID].IsValidate = true;
                        $scope.TableData10All[TRID].AadharNo2 = $scope.AADHAARNo;
                    } else if (ListType == 'TableData10AllSports') {
                        $scope.TableData10AllSports[TRID].Name = data.Name;
                        $scope.TableData10AllSports[TRID].DOB = data.DOB.replace(/'-'/g, '/');
                        $scope.TableData10AllSports[TRID].Gender = data.Gender == 'M' ? 'MALE' : 'FEMALE';
                        $scope.TableData10AllSports[TRID].Photo = data.Photo;
                        $scope.TableData10AllSports[TRID].IsValidate = true;
                        $scope.TableData10AllSports[TRID].AadharNo2 = $scope.AADHAARNo;
                    } else if (ListType == 'TableData10NomiAll') {
                        $scope.TableData10NomiAll[TRID].Name = data.Name;
                        $scope.TableData10NomiAll[TRID].DOB = data.DOB.replace(/'-'/g, '/');
                        $scope.TableData10NomiAll[TRID].Gender = data.Gender == 'M' ? 'MALE' : 'FEMALE';
                        $scope.TableData10NomiAll[TRID].Photo = data.Photo;
                        $scope.TableData10NomiAll[TRID].IsValidate = true;
                        $scope.TableData10NomiAll[TRID].AadharNo2 = $scope.AADHAARNo;
                    }
                }
            });


        }

    };

    $scope.OpenOtpPopUpOtherFun = function () {
        $("#SSOAuth_chkConsent").prop("checked", false);
        $('#SSOAuth_OtpText').val('');
        $('#ssoauthloading').hide();
        var TRID = $("#hdRowId").val();
        // alert(TRID)
        var arrylist = $('#' + TRID).find('td:eq(1)').find('input').val().split('');
        //   alert(arrylist.length)
        if ($('#' + TRID).find('td:eq(1)').find('input').val().length < 12 || $('#' + TRID).find('td:eq(1)').find('input').val().length > 12) {
            alert('Invalid Aadhaar Number');
            return;
        }
        $scope.MaskingAADHAARNo = arrylist[0] + arrylist[1] + 'XXXXXXXX' + arrylist[10] + arrylist[11];
        $scope.AADHAARNo = $('#' + TRID).find('td:eq(1)').find('input').val();
        $scope.messagebox = false;
        $scope.OtpDilogBox = true;
        $scope.FillOtp = false;
        $scope.CurrentRow = TRID;
    };
    $scope.GenrateOTP = function() {
        $('#ssoauthloading').show();
        $http.get(SiteUrl + "/Citizen/GenrateOTPdata/?_AadharNo=" + $scope.AADHAARNo).
            success(function(data, status) {
                if (data[0].txnid.length > 0) {
                    $scope.OTPStatus = true;
                    if (data[0].txnid.includes("DOITRJGOVAUTH") && data[0].txnid.split('#')[0] == "NO") {
                        $('#btnSSOAuth_SendOTP').hide();
                        $('#btnSSOAuth_ValidateOTP').show();
                        $scope.txn = data[0].txnid.split('#')[1];
                        $('#ssoauthloading').hide();
                    }
                    else if (data[0].txnid.split('#')[0] == "NO") {
                        alert(data[0].txnid.split('#')[1]);
                        $('#ssoauthloading').hide();
                    }
                    else {
                        $('#ssoauthloading').hide();
                        $('#btnSSOAuth_SendOTP').hide();
                        $('#btnSSOAuth_ValidateOTP').show();
                        $scope.txn = data[0].txnid;
                        //  $scope.Showgride = false;                       
                    }
                }
                else {
                    alert('Some Error Occured Please Try Again');
                    $('#ssoauthloading').hide();
                }
            }).
            error(function(data, status) {
                $scope.SocietyRegdata = [];
            });
    }
    $scope.ValidateOTP = function() {
        $('#ssoauthloading').show();
        $http.get(SiteUrl + "/Citizen/ValidateOTP/?_AadharNo=" + $scope.AADHAARNo + "&otp=" + $('#SSOAuth_OtpText').val() + "&txn=" + $scope.txn).
            success(function(data, status) {
                if (data[0].Column1.length > 0) {
                    if (data[0].Column1 == "Failed") {
                        $('#ssoauthloading').hide();
                        $scope.AADHAARNo = '';
                        $('#SSOAuth_OtpText').val('');
                        $scope.txn = '';
                        alert(data[0].Column2);
                        $('#btnSSOAuth_SendOTP').show();
                        $('#btnSSOAuth_ValidateOTP').hide();
                    }
                    else if (data[0].Column1 == "Success") {
                        $('#ssoauthloading').hide();
                        $scope.txn = '';
                        $scope.AADHAARNo = '';
                        $('#SSOAuth_OtpText').val('');
                        $('#btnSSOAuth_SendOTP').show();
                        $('#btnSSOAuth_ValidateOTP').hide();



                        if ($scope.FOR == 'CEO') {
                            $scope.SocietyRegistration.CEOName = data[0].Column2;
                            $scope.SocietyRegistration.CEOGender = data[0].Column3;
                            $scope.SocietyRegistration.CEODOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.CEOSocietyFile = data[0].Column5;
                            $scope.SocietyRegistration.CEOSocietyFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.CEOProfile = true;
                            $scope.CEORemove = true;
                            $scope.GETCEO = false;
                        }
                        else if ($scope.FOR == 'CHAIRMAN') {
                            $scope.SocietyRegistration.ChairmanName = data[0].Column2;
                            $scope.SocietyRegistration.ChairmanGender = data[0].Column3;
                            $scope.SocietyRegistration.ChairmanDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.ChairmanFile = data[0].Column5;
                            $scope.SocietyRegistration.ChairmanFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.ChairmanProfile = true;
                            $scope.CHAIRMANRemove = true;
                            $scope.GETCHAIRMAN = false;
                        }
                        else if ($scope.FOR == 'APPOINTED') {
                            $scope.SocietyRegistration.AppointedName = data[0].Column2;
                            $scope.SocietyRegistration.AppointedGender = data[0].Column3;
                            $scope.SocietyRegistration.AppointedDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.AppointedFile = data[0].Column5;
                            $scope.SocietyRegistration.AppointedFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.AdministratorProfile = true;
                            $scope.APPOINTEDRemove = true;
                            $scope.GETAPPOINTED = false;
                        }
                        else if ($scope.FOR == 'lIQUIDATOR') {
                            $scope.SocietyRegistration.liquidatorName = data[0].Column2;
                            $scope.SocietyRegistration.liquidatorGender = data[0].Column3;
                            $scope.SocietyRegistration.liquidatorDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.liquidatorFile = data[0].Column5;
                            $scope.SocietyRegistration.liquidatorFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.liquidatorProfile = true;
                            $scope.lIQUIDATORRemove = true;
                            $scope.GETlIQUIDATOR = false;
                        }
                        else if ($scope.FOR == 'SPCHAIRMAN') {
                            $scope.SocietyRegistration.CEOName = data[0].Column2;
                            $scope.SocietyRegistration.CEOGender = data[0].Column3;
                            $scope.SocietyRegistration.CEODOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.CEOSocietyFile = data[0].Column5;
                            $scope.SocietyRegistration.CEOSocietyFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.CEOSportsProfile = true;
                            $scope.SPCHAIRMANRemove = true;
                            $scope.GETSPCHAIRMAN = false;
                        }
                        else if ($scope.FOR == 'SPSECRETARY') {
                            $scope.SocietyRegistration.ChairmanName = data[0].Column2;
                            $scope.SocietyRegistration.ChairmanGender = data[0].Column3;
                            $scope.SocietyRegistration.ChairmanDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.ChairmanFile = data[0].Column5;
                            $scope.SocietyRegistration.ChairmanFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.ChairmanSportsProfile = true;
                            $scope.SPSECRETARYRemove = true;
                            $scope.GETSPSECRETARY = false;
                        }
                        else if ($scope.FOR == 'TREASURER') {
                            $scope.SocietyRegistration.AppointedName = data[0].Column2;
                            $scope.SocietyRegistration.AppointedGender = data[0].Column3;
                            $scope.SocietyRegistration.AppointedDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.ChairmanFile = data[0].Column5;
                            $scope.SocietyRegistration.AppointedFile = data[0].Column5;
                            $scope.AppointedFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.TreasurerProfile = true;
                            $scope.TREASURERRemove = true;
                            $scope.GETTREASURER = false;
                        }
                        else {
                            $scope.CurrentRowDataFill(data[0])
                        }

                    }
                }
            }).
            error(function(data, status) {
                $scope.SocietyRegdata = [];
            });
    }
    $scope.ClearAadharData = function(Clear) {
        if (Clear == 'CEO') {
            $scope.SocietyRegistration.CEOName = '';
            $scope.SocietyRegistration.CEOGender = '';
            $scope.SocietyRegistration.CEODOB = '';
            $scope.CEOSocietyFile = '';
            $scope.SocietyRegistration.CEOSocietyFile = '';
            $scope.CEOProfile = false;
            $scope.CEORemove = false;
            $scope.GETCEO = true;
        }
        if (Clear == 'CHAIRMAN') {
            $scope.SocietyRegistration.ChairmanName = '';
            $scope.SocietyRegistration.ChairmanGender = '';
            $scope.SocietyRegistration.ChairmanDOB = '';
            $scope.ChairmanFile = '';
            $scope.SocietyRegistration.ChairmanFile = '';
            $scope.ChairmanProfile = false;
            $scope.CHAIRMANRemove = false;
            $scope.GETCHAIRMAN = true;
        }
        if (Clear == 'APPOINTED') {
            $scope.SocietyRegistration.AppointedName = '';
            $scope.SocietyRegistration.AppointedDOB = '';
            $scope.SocietyRegistration.AppointedGender = '';
            $scope.AppointedFile = '';
            $scope.SocietyRegistration.AppointedFile = '';
            $scope.AdministratorProfile = false;
            $scope.APPOINTEDRemove = false;
            $scope.GETAPPOINTED = true;
        }
        if (Clear == 'lIQUIDATOR') {
            $scope.SocietyRegistration.liquidatorName = '';
            $scope.SocietyRegistration.liquidatorDOB = '';
            $scope.SocietyRegistration.liquidatorGender = '';
            $scope.liquidatorFile = '';
            $scope.SocietyRegistration.liquidatorFile = '';
            $scope.liquidatorProfile = false;
            $scope.lIQUIDATORRemove = false;
            $scope.GETlIQUIDATOR = true;
        }
        if (Clear == 'SPCHAIRMAN') {
            $('#txtSPCEOAADHAARNo').removeAttr("disabled");
            $('#txtSPORTCEOContactNo').removeAttr("disabled");
            $('#ddlCEOSPORTCategory').removeAttr("disabled");
            $scope.SocietyRegistration.CEOAADHAARNo = '';
            $scope.SocietyRegistration.CEOName = '';
            $scope.SocietyRegistration.CEOGender = '';
            $scope.SocietyRegistration.CEODOB = '';
            $scope.CEOSocietyFile = '';
            $scope.SocietyRegistration.CEOContactNo = '';
            $scope.SocietyRegistration.CEOSocietyFile = '';
            $scope.CEOSportsProfile = false;
            $scope.SPCHAIRMANRemove = false;
            $scope.GETSPCHAIRMAN = true;
            $scope.SocietyRegistration.CEOCategoryId = '0';
        }
        if (Clear == 'TREASURER') {
            $scope.SocietyRegistration.AppointedAADHAARNo = '';
            $scope.SocietyRegistration.AppointedContactNo = '';
            $scope.SocietyRegistration.AppointedCategoryId = '0'
            $('#txtTreasurerAadhar').removeAttr("disabled");
            $('#txtAppointedContactNo').removeAttr("disabled");
            $('#ddlAppointedSPORTCategory').removeAttr("disabled");
            $scope.SocietyRegistration.AppointedGender = '';
            $scope.SocietyRegistration.AppointedDOB = '';
            //$scope.CEOSocietyFile = '';
            $scope.SocietyRegistration.AppointedFile = '';
            $scope.TreasurerProfile = false;
            $scope.TREASURERRemove = false;
            $scope.GETTREASURER = true;

        }
        if (Clear == 'SPSECRETARY') {
            $('#txtSecretaryAadhar').removeAttr("disabled");
            $('#txtSPChairmanContactNo').removeAttr("disabled");
            $('#ddlChairmanSPORTCategory').removeAttr("disabled");
            $scope.ChairmanSportsProfile = false;
            $scope.SocietyRegistration.ChairmanAADHAARNo = '';
            $scope.SocietyRegistration.ChairmanName = '';
            $scope.SocietyRegistration.ChairmanGender = '';
            $scope.SocietyRegistration.ChairmanDOB = '';
            $scope.ChairmanFile = '';
            $scope.SocietyRegistration.ChairmanFile = '';
            $scope.ChairmanSportsProfile = false;
            $scope.SPSECRETARYRemove = false;
            $scope.GETSPSECRETARY = true;
            $scope.SocietyRegistration.ChairmanContactNo = '';
            $scope.SocietyRegistration.ChairmanCategoryId = 0;
        }
    }
    $scope.ClearAadharDataFromTable = function(TR) {
        if (TR == undefined)
            TR = $("#hdRowId").val();

        $('#' + TR).find('td:eq(2)').find('input').val('');
        $('#' + TR).find('td:eq(1)').find('input').removeAttr("disabled");
        $('#' + TR).find('td:eq(1)').find('input').val('');
        $('#' + TR).find('td:eq(3)').find('input').val('');
        $('#' + TR).find('td:eq(3)').find('input').removeAttr("disabled");
        $('#' + TR).find('td:eq(4)').find('input').val('');
        $('#' + TR).find('td:eq(5)').find('label').text('');
        $('#' + TR).find('td:eq(6)').find('select').val('0');
        $('#' + TR).find('td:eq(7)').find('input').val('');
        $('#' + TR).find('td:eq(6)').find('select').removeAttr("disabled");
        $('#' + TR).find('td:eq(7)').find('input').removeAttr("disabled");
        if (TR > 1000 && TR < 2000) {
            $('#' + TR).find('td:eq(8)').find('textarea').val('');
            ($('#' + TR).find('td:eq(9)').find('img'))[0].src = '../Content/Images/Balnk.png';
            $('#' + TR).find('td:eq(10)').find('input').val('');
            ($('#' + TR).find('td:eq(11)').find('input')).prop('checked', false);;
            $($('#' + TR).find('td:eq(12)').find('i')[1]).hide();
            $($('#' + TR).find('td:eq(12)').find('i')[0]).show();
        }
        else if (TR > 500 && TR < 2000) {
            $('#' + TR).find('td:eq(8)').find('select').val('0');
            ($('#' + TR).find('td:eq(9)').find('img'))[0].src = '../Content/Images/Balnk.png';
            $('#' + TR).find('td:eq(10)').find('input').val('');
            ($('#' + TR).find('td:eq(11)').find('input')).prop('checked', false);;
            $($('#' + TR).find('td:eq(12)').find('i')[1]).hide();
            $($('#' + TR).find('td:eq(12)').find('i')[0]).show();
        }
        else {
            ($('#' + TR).find('td:eq(8)').find('img'))[0].src = '../Content/Images/Balnk.png';
            $('#' + TR).find('td:eq(9)').find('input').val('');
            ($('#' + TR).find('td:eq(10)').find('input')).prop('checked', false);;
            $($('#' + TR).find('td:eq(11)').find('i')[1]).hide();
            $($('#' + TR).find('td:eq(11)').find('i')[0]).show();
        }
    }
    $scope.CurrentRowDataFill = function(data) {
        var TR = $scope.CurrentRow;
        // alert(TR);
        if (TR > 500 && TR < 1000) {
            //alert("in Nominated");
            $('#' + TR).find('td:eq(2)').find('input').val(data.Name);
            $('#' + TR).find('td:eq(1)').find('input').attr("disabled", "disabled");
            $('#' + TR).find('td:eq(4)').find('input').val(data.DOB);
            $('#' + TR).find('td:eq(5)').find('label').text(data.Gender == 'M' ? 'MALE' : 'FEMALE');

            ($('#' + TR).find('td:eq(9)').find('img'))[0].src = data.Photo;
            var jElement = $('#' + TR).find('td:eq(9)').find('img');
            jElement.css('border', 'rgb(222, 221, 221)');

        }
        else {
            $('#' + TR).find('td:eq(2)').find('input').val(data.Name);
            $('#' + TR).find('td:eq(1)').find('input').attr("disabled", "disabled");
            $('#' + TR).find('td:eq(4)').find('input').val(data.DOB);
            $('#' + TR).find('td:eq(5)').find('label').text(data.Gender == 'M' ? 'MALE' : 'FEMALE');
            if (TR > 1000 && TR < 2000) {
                ($('#' + TR).find('td:eq(9)').find('img'))[0].src = data.Photo;
                $($('#' + TR).find('td:eq(10)').find('i')[0]).hide();
                $($('#' + TR).find('td:eq(10)').find('i')[1]).show();
                var jElement = $('#' + TR).find('td:eq(9)').find('img');
                jElement.css('border', 'rgb(222, 221, 221)');
                var jElement = $('#' + TR).find('td:eq(8)').find('textarea');
                jElement.css('border', 'rgb(222, 221, 221)');
            }
            else {
                ($('#' + TR).find('td:eq(8)').find('img'))[0].src = data.Photo;
                $($('#' + TR).find('td:eq(9)').find('i')[0]).hide();
                $($('#' + TR).find('td:eq(9)').find('i')[1]).show();
                var jElement = $('#' + TR).find('td:eq(8)').find('img');
                jElement.css('border', 'rgb(222, 221, 221)');
            }
        }

        var jElement = $('#' + TR).find('td:eq(1)').find('input');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(2)').find('input');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(3)').find('input');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(4)').find('input');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(5)').find('label');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(6)').find('select');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(7)').find('select');
        jElement.css('border', 'rgb(222, 221, 221)');

        $scope.CloseOtpBox();
        //307691961366
    }
    //end aadhar verify
    $scope.TableData10All = [];
    $scope.AddMoreMemberDetailsNew = function(Flag, Isvalidate) {
        if (Flag == 0) {
            if ($scope.ValidateNominatedMember(Flag, Isvalidate)) {
                var rows = $('#TblNominatedMember tr').length + 500 + 1;
                if (_language == 'hi-in') {
                    //$('#TblNominatedMember').append('<tr id=' + rows + '>' +
                    //    '<td>' + (parseInt($('#TblNominatedMember tr').length) + 1) + '</td>' +
                    //    '<td><input type="text" name="AdhareNo" class="form-control" placeholder="आधार संख्या"  maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                    //    '<td><input disabled="disabled" type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                    //    '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर"  maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                    //    '<td><input  disabled="disabled" class="form-control AddDate" placeholder="जन्म की तारीख">' +
                    //    '<td><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                    //    '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '" ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                    //    '<td><div style="width:100%"><input type="text" class="form-control" placeholder="पद नाम"></div></td >' +
                    //    '<td><div style="width:100%"><select class="form-control"><option value="0" disabled>--- चुने ---</option><option value="1" class="ng-binding">सरकार द्वारा</option><option value="2" class="ng-binding">नाबार्ड द्वारा</option><option value="3" class="ng-binding">आरबीआई द्वारा</option><option value="4" class="ng-binding">तकनीकी निदेशक द्वारा</option></select ></div ></td >' +
                    //    '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                    //    '<td><i ng-click="OpenOtpPopUpOther(' + "''" + ',' + "''" +',' + rows + ')" class="fa fa-info-circle info-m2"></i><i onclick="SetAngularValueForRemove(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                    //    '</tr>');
                    $scope.TableData10All.push(
                        {
                            "Id": rows,
                            "Name": '',
                            "POSTNAME": '',
                            "Aadh": '',
                            "Gender": '',
                            "DOB": '',
                            "ContactNo": '',
                            "Photo": '../Content/Images/Balnk.png',
                            "CategoryName": '',
                            "Occupation": '',
                            "CategoryId": "0",
                            "CatId": "0",
                            "Status": 'false',
                            "MEMBERTYPE": '',
                            "IsValidate": false,
                            "IsNew": true,
                            "IsDelete": false,
                            "AdministrativeId": 0,
                            "AadharNo2": "",
                            "RemovalType": "0"
                        });

                }
                else {
                    //$('#TblNominatedMember').append('<tr id=' + rows + '>' +
                    //    '<td>' + (parseInt($('#TblNominatedMember tr').length) + 1) + '</td>' +
                    //    '<td><input type="text" name="AdhareNo" class="form-control" placeholder="AADHAARNo" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                    //    '<td><input type= "text" disabled="disabled" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Director Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                    //    '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="MobileNo" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                    //    '<td><input disabled="disabled" name="DOB" class="form-control AddDate" placeholder="DOB" autocomplete="off">' +
                    //    '<td><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                    //    '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '"><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                    //    '<td><div style="width:100%"><input type="text" class="form-control" placeholder="Post Name"></div></td >' +
                    //    '<td><div style="width:100%"><select class="form-control"><option value="0" disabled>--- Select ---</option><option value="1" class="ng-binding">By Government</option><option value="2" class="ng-binding">By Nabard</option><option value="3" class="ng-binding">By RBI</option><option value="4" class="ng-binding">By Technical Director</option></select ></div ></td >' +
                    //    '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                    //    '<td><i ng-click="OpenOtpPopUpOther(' + "''" + ',' + "''" +',' + rows + ')" class="fa fa-info-circle info-m2"></i><i onclick="SetAngularValueForRemove(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                    //    '</tr>');

                    $scope.TableData10All.push(
                        {
                            "Id": rows,
                            "Name": '',
                            "POSTNAME": '',
                            "Aadh": '',
                            "Gender": '',
                            "DOB": '',
                            "ContactNo": '',
                            "Photo": '../Content/Images/Balnk.png',
                            "CategoryName": '',
                            "Occupation": '',
                            "CategoryId": "0",
                            "CatId": "0",
                            "Status": 'false',
                            "MEMBERTYPE": '',
                            "IsValidate": false,
                            "IsNew": true,
                            "IsDelete": false,
                            "AdministrativeId": 0,
                            "AadharNo2": "",
                            "RemovalType": "0"
                        });
                }
            }
            else {
                //  alert('false');
            }
        }
        if (Flag == 1) {
            if ($scope.ValidateNominatedMember(Flag, Isvalidate)) {
                var rows = ($('#TblSportElectedMember tr').length) + 1001;
                if (_language == 'hi-in') {
                    //$('#TblSportElectedMember').append('<tr id=' + rows + '>' +
                    //    '<td>' + (parseInt($('#TblSportElectedMember tr').length) + 1) + '</td>' +
                    //    '<td><input type="text" name="AdhareNo" class="form-control" placeholder="आधार संख्या"  maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                    //    '<td><input disabled="disabled" type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                    //    '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर"  maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                    //    '<td><input  disabled="disabled" class="form-control AddDate" placeholder="जन्म की तारीख">' +
                    //    '<td><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                    //    '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '" ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                    //    '<td><div style="width:100%"><input type="text" class="form-control" placeholder="पद"></div></td >' +
                    //    '<td><textarea type="text" name="Address" class="form-control" placeholder="Address" autocomplete="off"></textarea></td >' +
                    //    '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                    //    '<td><input type="checkbox" class="checkbox-input"></td>' +
                    //    '<td><i ng-click="OpenOtpPopUpOther(' + "''" + ',' + "''" +',' + rows + ')" class="fa fa-info-circle info-m2"></i><i ng-show="SHOWREMOVE" onclick="SetAngularValueForRemove(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                    //    '</tr>');

                    $scope.TableData10All.push(
                        {
                            "Id": rows,
                            "Name": '',
                            "POSTNAME": '',
                            "Aadh": '',
                            "Gender": '',
                            "DOB": '',
                            "ContactNo": '',
                            "Photo": '../Content/Images/Balnk.png',
                            "CategoryName": '',
                            "Occupation": '',
                            "CategoryId": "0",
                            "CatId": "0",
                            "Status": 'false',
                            "MEMBERTYPE": '',
                            "IsValidate": false,
                            "IsNew": true,
                            "IsDelete": false,
                            "AdministrativeId": 0,
                            "AadharNo2": "",
                            "RemovalType": "0"
                        });

                }
                else {
                    //$('#TblSportElectedMember').append('<tr id=' + rows + '>' +
                    //    '<td>' + (parseInt($('#TblSportElectedMember tr').length) + 1) + '</td>' +
                    //    '<td><input type="text" name="AdhareNo" class="form-control" placeholder="AADHAARNo" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                    //    '<td><input type= "text" disabled="disabled" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Director Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                    //    '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="MobileNo" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                    //    '<td><input disabled="disabled" name="DOB" class="form-control AddDate" placeholder="DOB" autocomplete="off">' +
                    //    '<td><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                    //    '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '"><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                    //    '<td><div style="width:100%"><input type="text" class="form-control" placeholder="POST"></div></td >' +
                    //    '<td><textarea type="text" name="Address" class="form-control" placeholder="पता" autocomplete="off"></textarea></td >' +
                    //    '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                    //    '<td><input type="checkbox" class="checkbox-input"></td>' +
                    //    '<td><i ng-click="OpenOtpPopUpOther(' + "''" + ',' + "''" +',' + rows + ')" class="fa fa-info-circle info-m2"></i><i onclick="SetAngularValueForRemove(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                    //    '</tr>');
                    $scope.TableData10All.push(
                        {
                            "Id": rows,
                            "Name": '',
                            "POSTNAME": '',
                            "Aadh": '',
                            "Gender": '',
                            "DOB": '',
                            "ContactNo": '',
                            "Photo": '../Content/Images/Balnk.png',
                            "CategoryName": '',
                            "Occupation": '',
                            "CategoryId": "0",
                            "CatId": "0",
                            "Status": 'false',
                            "MEMBERTYPE": '',
                            "IsValidate": false,
                            "IsNew": true,
                            "IsDelete": false,
                            "AdministrativeId": 0,
                            "AadharNo2": "",
                            "RemovalType": "0"
                        });
                }
            }
        }
        if (Flag == 2) {
            if ($scope.ValidateNominatedMember(Flag)) {
                var rows = $('#TblSportNominatedMember tr').length + 1;
                if (_language == 'hi-in') {
                    //$('#TblSportNominatedMember').append('<tr id=' + rows + '>' +
                    //    '<td>' + (parseInt($('#TblSportNominatedMember tr').length) + 1) + '</td>' +
                    //    '<td><input type="text" name="AdhareNo" class="form-control" placeholder="आधार संख्या"  maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                    //    '<td><input disabled="disabled" type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                    //    '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर"  maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                    //    '<td><input  disabled="disabled" class="form-control AddDate" placeholder="जन्म की तारीख">' +
                    //    '<td><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                    //    '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '" ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                    //    '<td><div style="width:100%"><select class="form-control" id="Post' + rows + '"><option value="0" disabled>{{translation.SelectOption}}</option></select></div></td >' +
                    //    '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                    //    '<td><i ng-click="OpenOtpPopUpOther(' + "''" + ',' + "''" +',' + rows + ')" class="fa fa-info-circle info-m2"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ></td >' +
                    //    '</tr>');
                    $scope.TableData10All.push(
                        {
                            "Id": rows,
                            "Name": '',
                            "POSTNAME": '',
                            "Aadh": '',
                            "Gender": '',
                            "DOB": '',
                            "ContactNo": '',
                            "Photo": '../Content/Images/Balnk.png',
                            "CategoryName": '',
                            "Occupation": '',
                            "CategoryId": "0",
                            "CatId": "0",
                            "Status": 'false',
                            "MEMBERTYPE": '',
                            "IsValidate": false,
                            "IsNew": true,
                            "IsDelete": false,
                            "AdministrativeId": 0,
                            "AadharNo2": "",
                            "RemovalType": "0"
                        });
                }
                else {
                    //$('#TblSportNominatedMember').append('<tr id=' + rows + '>' +
                    //    '<td>' + (parseInt($('#TblSportNominatedMember tr').length) + 1) + '</td>' +
                    //    '<td><input type="text" name="AdhareNo" class="form-control" placeholder="AADHAARNo" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                    //    '<td><input type= "text" disabled="disabled" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Director Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                    //    '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="MobileNo" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                    //    '<td><input disabled="disabled" name="DOB" class="form-control AddDate" placeholder="DOB" autocomplete="off">' +
                    //    '<td><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                    //    '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '"><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                    //    '<td><div style="width:100%"><select class="form-control" id="Post' + rows + '"><option value="0" disabled>{{translation.SelectOption}}</option></select></div></td >' +
                    //    '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                    //    '<td><i ng-click="OpenOtpPopUpOther(' + "''" + ',' + "''" +',' + rows + ')" class="fa fa-info-circle info-m2"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ></td >' +
                    //    '</tr>');
                    $scope.TableData10All.push(
                        {
                            "Id": rows,
                            "Name": '',
                            "POSTNAME": '',
                            "Aadh": '',
                            "Gender": '',
                            "DOB": '',
                            "ContactNo": '',
                            "Photo": '../Content/Images/Balnk.png',
                            "CategoryName": '',
                            "Occupation": '',
                            "CategoryId": "0",
                            "CatId": "0",
                            "Status": 'false',
                            "MEMBERTYPE": '',
                            "IsValidate": false,
                            "IsNew": true,
                            "IsDelete": false,
                            "AdministrativeId": 0,
                            "AadharNo2": "",
                            "RemovalType": "0"
                        });
                }
            }
        }
        if (Flag == 3) {
            if ($scope.AdministrativeFormValidation(Isvalidate)) {
                var rows = $('#TblElectedMember tr').length + 1;
                if (_language == 'hi-in') {
                    //$('#TblElectedMember').append('<tr id=' + rows + '>' +
                    //    '<td>' + (parseInt($('#TblElectedMember tr').length) + 1) + '</td>' +
                    //    '<td><input type="text" name="AdhareNo" class="form-control" placeholder="आधार संख्या"  maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                    //    '<td><input disabled="disabled" type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                    //    '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर"  maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                    //    '<td><input  disabled="disabled" class="form-control AddDate" placeholder="जन्म की तारीख">' +
                    //    '<td><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                    //    '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '" ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                    //    '<td><div style="width:100%"><input type="text" class="form-control" placeholder="पद"></div></td >' +
                    //    '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                    //    '<td><input type="text" class="form-control" placeholder="व्यवसाय"></td>' +
                    //    '<td><input type="checkbox" class="checkbox-input"></td>' +
                    //    '<td><i ng-click="OpenOtpPopUpOther(' + "''" + ',' + "''" +',' + rows + ')" class="fa fa-info-circle green-m1"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                    //    '</tr>');
                    $scope.TableData10All.push(
                        {
                            "Id": rows,
                            "Name": '',
                            "POSTNAME": '',
                            "Aadh": '',
                            "Gender": '',
                            "DOB": '',
                            "ContactNo": '',
                            "Photo": '../Content/Images/Balnk.png',
                            "CategoryName": '',
                            "Occupation": '',
                            "CategoryId": "0",
                            "CatId": "0",
                            "Status": 'false',
                            "MEMBERTYPE": '',
                            "IsValidate": false,
                            "IsNew": true,
                            "IsDelete": false,
                            "AdministrativeId": 0,
                            "AadharNo2": "",
                            "RemovalType": "0"
                        });
                }
                else {
                    //$('#TblElectedMember').append('<tr id=' + rows + '>' +
                    //    '<td>' + (parseInt($('#TblElectedMember tr').length) + 1) + '</td>' +
                    //    '<td><input type="text" name="AdhareNo" class="form-control" placeholder="AADHAARNo" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                    //    '<td><input type= "text" disabled="disabled" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Director Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                    //    '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="MobileNo" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                    //    '<td><input disabled="disabled" name="DOB" class="form-control AddDate" placeholder="DOB" autocomplete="off">' +
                    //    '<td><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                    //    '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '"><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                    //    '<td><div style="width:100%"><input type="text" class="form-control" placeholder="Post"></div></td >' +
                    //    '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                    //    '<td><input type="text" class="form-control" placeholder="Occupation"></td>' +
                    //    '<td><input type="checkbox" class="checkbox-input"></td>' +
                    //    '<td><i ng-click="OpenOtpPopUpOther(' + "''" + ',' + "''" +',' + rows + ')" class="fa fa-info-circle green-m1"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                    //    '</tr>');
                    $scope.TableData10All.push(
                        {
                            "Id": rows,
                            "Name": '',
                            "POSTNAME": '',
                            "Aadh": '',
                            "Gender": '',
                            "DOB": '',
                            "ContactNo": '',
                            "Photo": '../Content/Images/Balnk.png',
                            "CategoryName": '',
                            "Occupation": '',
                            "CategoryId": "0",
                            "CatId": "0",
                            "Status": 'false',
                            "MEMBERTYPE": '',
                            "IsValidate": false,
                            "IsNew": true,
                            "IsDelete": false,
                            "AdministrativeId": 0,
                            "AadharNo2": "",
                            "RemovalType": "0"
                        });
                }
            }
            else {

            }
        }
        //FIll Category DropDown 

        var ddHTML = _language == 'en-us' ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        angular.forEach($scope.CategoryList, function(item) {
            if (_language == 'en-us') {
                ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            }
            else {
                ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            }
        });
        $("#Category" + rows).html(ddHTML);
        //END
        //FIll Post DropDown
        var ddHTML = _language == 'en-us' ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        angular.forEach($scope.ChairmanPosts, function(item) {
            if (_language == 'en- us') {
                ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_NAME + '</option>';
            }
            else {
                ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_SERV + '</option>';
            }
        });
        $("#Post" + rows).html(ddHTML);
        //END


    };

    $scope.ValidateNominatedMember = function(FindTablebyId, Isvalidate) {
        if (Isvalidate == false)
            return true;
        else {
            var validate = true;
            var table = ''
            if (FindTablebyId == 0) {
                table = '#TblNominatedMember tr';
            }
            else if (FindTablebyId == 1) {
                table = '#TblSportElectedMember tr';
            }
            else if (FindTablebyId == 2) {
                table = '#TblSportNominatedMember tr';
            }

            $(table).each(function() {
                var thhhhh = $(this).find('td:eq(11)').find('input[type="hidden"]').val();
                $(this).find('td').find('input').css('border', 'rgb(222, 221, 221)');
                if (thhhhh == 'false') {
                    if ($(this).find('td:eq(1)').find('input').val() != '' || FindTablebyId != 0) {
                        if ($(this).find('td:eq(1)').find('input').val() == '') {
                            var jElement = $(this).find('td:eq(1)').find('input');
                            jElement.css('border', 'solid 1px red');
                            validate = false;
                        }
                        else {
                            var jElement = $(this).find('td:eq(1)').find('input');
                            jElement.css('border', 'rgb(222, 221, 221)');
                        }

                        if ($(this).find('td:eq(2)').find('input').val() == undefined || $(this).find('td:eq(2)').find('input').val() == '') {
                            var jElement = $(this).find('td:eq(2)').find('input');
                            jElement.css('border', 'solid 1px red');
                            validate = false;
                        }
                        else {
                            var jElement = $(this).find('td:eq(2)').find('input');
                            jElement.css('border', 'rgb(222, 221, 221)');
                        }

                        if ($(this).find('td:eq(3)').find('input').val() == undefined || $(this).find('td:eq(3)').find('input').val() == '') {
                            var jElement = $(this).find('td:eq(3)').find('input');
                            jElement.css('border', 'solid 1px red');
                            validate = false;
                            $(this).find('td:eq(3)').find('input').removeAttr("disabled");
                        }
                        else {
                            var jElement = $(this).find('td:eq(3)').find('input');
                            jElement.css('border', 'rgb(222, 221, 221)');
                        }
                        //DOB
                        if ($(this).find('td:eq(4)').find('input').val() == undefined || $(this).find('td:eq(4)').find('input').val() == '') {
                            var jElement = $(this).find('td:eq(4)').find('input');
                            jElement.css('border', 'solid 1px red');
                            validate = false;
                        }
                        else {
                            var jElement = $(this).find('td:eq(4)').find('input');
                            jElement.css('border', 'rgb(222, 221, 221)');
                        }
                        //GEN
                        if ($(this).find('td:eq(5)').find('label').html() == '') {
                            var jElement = $(this).find('td:eq(5)').find('label');
                            jElement.css('border', 'solid 1px red');
                            validate = false;
                        }
                        else {
                            var jElement = $(this).find('td:eq(5)').find('label');
                            jElement.css('border', 'rgb(222, 221, 221)');

                        }
                        //CATEGORY
                        if ($(this).find('td:eq(6)').find('select').val() == null || $(this).find('td:eq(6)').find('select').val() == "0") {
                            var jElement = $(this).find('td:eq(6)').find('select');
                            jElement.css('border', 'solid 1px red');
                            $(this).find('td:eq(6)').find('select').removeAttr("disabled");
                            validate = false;
                        }
                        else {
                            var jElement = $(this).find('td:eq(6)').find('select');
                            jElement.css('border', 'rgb(222, 221, 221)');

                        }
                        //POST
                        if ($(this).find('td:eq(7)').find('input').val() == null || $(this).find('td:eq(7)').find('input').val() == "") {
                            var jElement = $(this).find('td:eq(7)').find('input');
                            jElement.css('border', 'solid 1px red');
                            $(this).find('td:eq(7)').find('input').removeAttr("disabled");
                            validate = false;
                        }
                        else {
                            var jElement = $(this).find('td:eq(7)').find('input');
                            jElement.css('border', 'rgb(222, 221, 221)');

                        }
                        //Nominated By Whom	
                        if (FindTablebyId == 0) {
                            if ($(this).find('td:eq(8)').find('select').val() == null || $(this).find('td:eq(8)').find('select').val() == "0") {
                                var jElement = $(this).find('td:eq(8)').find('select');
                                jElement.css('border', 'solid 1px red');
                                $(this).find('td:eq(8)').find('select').removeAttr("disabled");
                                validate = false;
                            }
                            else {
                                var jElement = $(this).find('td:eq(8)').find('select');
                                jElement.css('border', 'rgb(222, 221, 221)');

                            }
                        }

                        if (FindTablebyId == 1) {
                            if ($(this).find('td:eq(8)').find('textarea').val() == '') {
                                var jElement = $(this).find('td:eq(8)').find('textarea');
                                jElement.css('border', 'solid 1px red');
                                validate = false;
                            }
                            else {
                                var jElement = $(this).find('td:eq(8)').find('textarea');
                                jElement.css('border', 'rgb(222, 221, 221)');
                            }
                        }

                        if ($(this).find('td:eq(9)').find('img')[0].src.indexOf("Balnk.png") > 0) {
                            var jElement = $(this).find('td:eq(9)').find('img');
                            jElement.css('border', 'solid 1px red');
                            validate = false;
                        }
                        else {
                            var jElement = $(this).find('td:eq(9)').find('img');
                            jElement.css('border', 'rgb(222, 221, 221)');

                        }
                    }
                }
            });
            return validate;
        }
    }
    //START AdministrativeFormValidation  
    $scope.AdministrativeFormValidation = function() {
        var validate = true;
        var Count = 0;
        // alert('OA');
        var message = $scope._language == 'en-us' ? ' Validate  following : ' : 'निम्नलिखित मान्य करें :';

        var Count = 0;


        $('#TblElectedMember tr').each(function() {
            var thhhhh = $(this).find('td:eq(11)').find('input[type="hidden"]').val();
            $(this).find('td').find('input').css('border', 'rgb(222, 221, 221)');
            if (thhhhh == 'false') {
                if ($(this).find('td:eq(1)').find('input').val() == undefined || $(this).find('td:eq(1)').find('input').val() == '') {
                    var jElement = $(this).find('td:eq(1)').find('input');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(1)').find('input');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }

                if ($(this).find('td:eq(2)').find('input').val() == undefined || $(this).find('td:eq(2)').find('input').val() == '') {
                    var jElement = $(this).find('td:eq(2)').find('input');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(2)').find('input');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }

                if ($(this).find('td:eq(3)').find('input').val() == undefined || $(this).find('td:eq(3)').find('input').val() == '') {
                    var jElement = $(this).find('td:eq(3)').find('input');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                    $(this).find('td:eq(3)').find('input').removeAttr("disabled");
                }
                else {
                    var jElement = $(this).find('td:eq(3)').find('input');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }

                if ($(this).find('td:eq(4)').find('input').val() == undefined || $(this).find('td:eq(4)').find('input').val() == '') {
                    var jElement = $(this).find('td:eq(4)').find('input');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(4)').find('input');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }

                if ($(this).find('td:eq(5)').find('label').html() == '') {
                    var jElement = $(this).find('td:eq(5)').find('label');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(5)').find('label');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }

                if ($(this).find('td:eq(6)').find('select').val() == null || $(this).find('td:eq(6)').find('select').val() == "0") {
                    var jElement = $(this).find('td:eq(6)').find('select');
                    jElement.css('border', 'solid 1px red');
                    $(this).find('td:eq(6)').find('select').removeAttr("disabled");
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(6)').find('select');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }
                if ($(this).find('td:eq(7)').find('input').val() == '') {
                    var jElement = $(this).find('td:eq(7)').find('input');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(7)').find('input');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }

                if ($(this).find('td:eq(8)').find('img')[0].src.indexOf("Balnk.png") > 0) {
                    var jElement = $(this).find('td:eq(8)').find('img');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(8)').find('img');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }

                if ($(this).find('td:eq(9)').find('input').val() == '') {
                    var jElement = $(this).find('td:eq(9)').find('input');
                    jElement.css('border', 'solid 1px red');
                    validate = false;
                }
                else {
                    var jElement = $(this).find('td:eq(9)').find('input');
                    jElement.css('border', 'rgb(222, 221, 221)');
                }
            }
            if ($(this).find('td:eq(10)').find('input').prop('checked') == true) {
                Count = Count + 1;
            }
        });

        //if ($scope.ActId == "3" && Count < 12) {
        //    var r = confirm($scope._language == 'en-us' ? 'Minimum 12 officers should be in the executive / governing board, do you want to proceed further?' : 'न्यूनतम 12 पदाधिकारी /कार्यकारिणी /संचालक मंडल में होने चाहिए क्या आप आगे प्रोसेस करना चाहते है ?');
        //    if (r == false) {
        //        return false;
        //    }
        //}
        //else if ($scope.ActId == "1" && Count < 7) {
        //    var r = confirm($scope._language == 'en-us' ? 'Minimum 7 officers should be in the executive / governing board, do you want to proceed further?' : 'न्यूनतम 7 पदाधिकारी /कार्यकारिणी /संचालक मंडल में होने चाहिए क्या आप आगे प्रोसेस करना चाहते है ?');
        //    if (r == false) {
        //        return false;
        //    }
        //}
        //}

        //if ($scope.SocietyRegistration.IsAdministrative == 'false') {


        //}

        //if ($scope.SocietyRegistration.IsAdministrative == 'Board' && $scope.SocietyRegistration.ActId == "3") {
        //    if ($scope.SocietyRegistration.TotalMembersClassA == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Total Members Class A.' : '\n <br /> कृपया कक्षा ए के कुल सदस्य दर्ज करें |';
        //    }
        //    if ($scope.SocietyRegistration.TotalMembersClassB == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Total Members Class B.' : '\n <br /> कृपया कक्षा बी के कुल सदस्य दर्ज करें |';
        //    }
        //    if ($scope.SocietyRegistration.TotalMembersClassC == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Total Members Class C.' : '\n <br /> कृपया कक्षा सी के कुल सदस्य दर्ज करें |';
        //    }
        //}

        //}
        //else {

        //    if ($('#txtSportCEODOB').val() == '') {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Chairman Date of Birth' : '\n <br /> कृपया अध्यक्ष की जन्म तारीख दर्ज करे |';

        //    }

        //    if ($scope.SocietyRegistration.CEOGender == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select Chairman Gender Type.' : '\n <br /> कृपया अध्यक्ष के लिंग का चयन करे |';

        //    }

        //    if ($scope.SocietyRegistration.CEOCategoryId == "0" || $scope.SocietyRegistration.CEOCategoryId == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select The Category Of Chairman.' : '\n <br /> कृपया अध्यक्ष के वर्ग का चयन करें |';

        //    }

        //    if ($scope.CEOSocietyFile == '' || $scope.CEOSocietyFile == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select The  File of Chairman.' : '\n <br /> कृपया अध्यक्ष की फ़ाइल का चयन करें|';

        //    }
        //    //}
        //    if ($('#txtSportChairmanDOB').val() == '') {

        //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter the Secretary DOB.' : '\n <br /> कृपया सचिव की जन्म तारीख दर्ज करें |';

        //    }

        //    if ($scope.SocietyRegistration.ChairmanGender == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select Secretary Gender Type.' : '\n <br /> कृपया सचिव के लिंग का चयन करे |';

        //    }

        //    if ($scope.ChairmanFile == '' || $scope.ChairmanFile == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select The File of Secretary.' : '\n <br /> कृपया सचिव की फ़ाइल का चयन करें|';

        //    }
        //    if ($scope.SocietyRegistration.ChairmanCategoryId == "0" || $scope.SocietyRegistration.ChairmanCategoryId == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select The Category Of Secretary.' : '\n <br /> कृपया सचिव की श्रेणी का चयन करें |';

        //    }



        //    //Treasurer

        //    if ($('#txtSportAppointedDOB').val() == '') {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter the DOB Of Treasurer.' : '\n <br /> कृपया कोषाध्यक्ष की जन्म तारीख दर्ज करें |';

        //    }
        //    if ($scope.AppointedFile == '' || $scope.AppointedFile == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Choose the File of Treasurer.' : '\n <br /> कृपया कोषाध्यक्ष की फाइल चुनें |';

        //    }
        //    if ($scope.SocietyRegistration.AppointedCategoryId == '0' || $scope.SocietyRegistration.AppointedCategoryId == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select The Category Of treasurer.' : '\n <br /> कृपया कोषाध्यक्ष की श्रेणी का चयन करें|';

        //    }

        //    if ($scope.SocietyRegistration.AppointedGender == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select Secretary treasurer Type.' : '\n <br /> कृपया कोषाध्यक्ष के लिंग का चयन करे |';

        //    }

        //}
        ////End Sport Association

        if (message.length > 24 || validate == false) {
            if (message.length > 24) {
                ShowModelAlert("Administrative Form Validation", message);
            }
            else {
                ShowModelAlert("Administrative Form Validation", $scope._language == 'en-us' ? 'Validate the value of red border' : 'लाल सीमा के मूल्य को मान्य करें');
            }
            return false;
        }
        return true;
    }
    //validateAddForm for farmer ends

    // START : Save Balance Sheet Insert
    $scope.SaveBalanceSheetInsert = function() {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.SocietyAmendment.RegiYearId == undefined || $scope.SocietyAmendment.RegiYearId == '0') {
            message += (_language == 'en-us' ? 'Please Select Registered Year \n' : 'पंजीकृत वर्ष का चयन करे\n');
            message += '<br />';
        }
        if ($scope.InsertBSheetFileExl == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Upload Excel Sheet' : '\nकृपया एक्सेल शीट अपलोड करें |';
            message += '<br />';
        }
        if (message.length > 27) {
            ShowModelAlert("Belence Sheet Insert", message);
            return false;
        }
        // alert(11);
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        //  alert($scope.SocietyAmendment.RegiYearId);
        formData.append("SocietyIdEnc", Datasplit[0]);
        formData.append("SocietyDirectory", Datasplit[1]);
        formData.append("RegiYearId", $scope.SocietyAmendment.RegiYearId);
        formData.append("ExcelBalanceSheetFile", $scope.InsertBSheetFileExl);
        $http.post(SiteUrl + "/Citizen/InsertBalanceSheetByExcel", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function(data, status) {
                //alert(1);
                $scope.BalanceSheetNew = data;
                //alert(2); 
                $scope.NewBSheet = true;
                //alert(3);
                $scope.BalanceSheetFileList();
            }).
            error(function(data, status) {
                ShowModelAlert("Insert of Balance Sheet", "Error While Upload Excel Sheet");
            });
    };
    // END :Save Balance Sheet Insert

    $scope.messagebox = false;
    $scope.OnPageLoad();

    $scope.SocietyDirectoryFile = "";
    $scope.UploadFile = function(id, type, index, model, UploadType, fileext) {
        try {    // FormData
            if (window.FormData !== undefined) {
                var fileUpload = $("#" + id).get(0);
                var files = fileUpload.files;
                //if (files.length <= 0) {
                //    //alert("Please select image to upload");
                //} else {

                //var block = $("#" + type + "_" + id + "_" + indx).attr("src").split(";");
                // Get the content type of the image
                // var contentType = block[0].split(":")[1];// In this case "image/gif"
                // get the real base64 content of the file
                //var realData = block[1].split(",")[1];// In this case "R0lGODlhPQBEAPeoAJosM...."

                // Convert it to a blob to upload
                //var blob = b64toBlob(realData, contentType, 512);

                var formData = new FormData();
                formData.append("pldbfile", files[0]);
                //formData.append("Directory", "~/Content/AuditReport/");

                var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
                $scope.SocietyDirectoryFile = Datasplit[1];
                formData.append("SocietyDirectory", Datasplit[1]);
                $(".blockUI").show();
                if (UploadType != 'profile') {
                    $scope.xhr(SiteUrl + "/citizen/UploadAmendmentFiles?SocietyDirectory=", formData, type, index, model);
                } else {
                    var RegistrationNo = $scope.SocietyRegistration.RegistrationNo == undefined ? "" : $scope.SocietyRegistration.RegistrationNo.replace(/ /g, '').replace(/\\/g, "").replace(/-/g, "").replace(/\//g, "");
                    $scope.SocietyDirectoryFile = RegistrationNo;
                    var OldRegistrationNo = $scope.SocietyRegistration.OldRegistrationNo == undefined ? "" : $scope.SocietyRegistration.OldRegistrationNo.replace(/ /g, '').replace(/\\/g, "").replace(/-/g, "").replace(/\//g, "");
                    var SocietyIDEnc = $scope.SocietyRegistration.SocietyIDEnc == undefined ? "" : $scope.SocietyRegistration.SocietyIDEnc;
                    var filetype = fileext == 'MembersListExcelFile' ? "excel" : "";
                    formData.append("OldRegistrationNo", $scope.SocietyRegistration.OldRegistrationNo);
                    $scope.xhrProfile(SiteUrl + "/Society/UploadProfilingFiles?RegistrationNo=" + RegistrationNo + "&OldRegistrationNo=" + OldRegistrationNo + "&SocietyIDEnc=" + SocietyIDEnc + "&filetype=" + filetype, formData, type, index, model);
                }

            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }
    $scope.xhr = function(url, data, type, index, model, UploadType) {
        try {
            var request = new XMLHttpRequest();
            request.onreadystatechange = function() {
                if (request.readyState == 4 && request.status == 200) {
                    $(".blockUI").hide();
                    $("#filuploadProgress").hide();
                    $("#filuploadProgress").find("div").html("");
                    $("#filuploadProgress").find("div").attr("aria-valuenow", "1");
                    $("#filuploadProgress").find("div").attr("style", "width:1%");
                    if (request.responseText == "") {
                        ShowOkMessage("Error", $scope == 'en-us' ? "There is some problem in uploading file on server. Please try after some time" : "सर्वर पर फ़ाइल अपलोड करने में कुछ समस्या है। कृपया कुछ देर बाद प्रयास करें", function() { });
                    } else {
                        $scope.FileCollection.forEach(function(itm, indx) {
                            if (itm.FileId == model) {
                                itm.FileName = $scope.SocietyDirectoryFile + '/' + request.responseText;
                                itm.File = request.responseText;
                                $("#" + model).parent().parent().css('border', 'none');
                            }
                        });
                        //$scope["" + model + ""]  = request.responseText;                        
                        $scope.$apply();
                    }
                }
            };


            request.upload.onloadstart = function() {
                //percentage.innerHTML = 'Upload started...';
            };
            $("#filuploadProgress").show();
            request.upload.onprogress = function(event) {
                $("#filuploadProgress").find("div").html(Math.round(event.loaded / event.total * 100) + "%");
                $("#filuploadProgress").find("div").attr("aria-valuenow", Math.round(event.loaded / event.total * 100));
                $("#filuploadProgress").find("div").attr("style", "width:" + Math.round(event.loaded / event.total * 100) + "%");
            };

            request.upload.onload = function(result) {

                //if (($(".addprofilePhoto").length >= uploadIndex + 1) || ($(".addFamilyPhoto").length >= uploadIndex + 1)) {
                //    $(".blockUI").hide();
                //    LoadView('_PhotosVideos', 'tb-photosvideos');
                //}
                //++uploadIndex;
            }
            request.open('POST', url);
            request.send(data);
        }
        catch (ex) { alert(ex) }
    }

    $scope.xhrProfile = function(url, data, type, index, model, UploadType) {
        try {
            var request = new XMLHttpRequest();
            request.onreadystatechange = function() {
                if (request.readyState == 4 && request.status == 200) {
                    $(".blockUI").hide();
                    $("#filuploadProgress").hide();
                    $("#filuploadProgress").find("div").html("");
                    $("#filuploadProgress").find("div").attr("aria-valuenow", "1");
                    $("#filuploadProgress").find("div").attr("style", "width:1%");

                    if (request.responseText.indexOf('@') > -1) {
                        ShowOkMessage("Error", request.responseText.split('@')[1], function() { });
                    } else
                        if (request.responseText == "") {
                            ShowOkMessage("Error", $scope == 'en-us' ? "There is some problem in uploading file on server. Please try after some time" : "सर्वर पर फ़ाइल अपलोड करने में कुछ समस्या है। कृपया कुछ देर बाद प्रयास करें", function() { });
                        } else {
                            $scope.FileCollection.forEach(function(itm, indx) {
                                if (itm.FileId == model) {
                                    itm.FileName = $scope.SocietyDirectoryFile + '/' + request.responseText;
                                    itm.File = request.responseText;
                                    $("#" + model).parent().parent().css('border', 'none');
                                }
                            });
                            //$scope["" + model + ""]  = request.responseText;                        
                            $scope.$apply();
                        }
                }
            };


            request.upload.onloadstart = function() {
                //percentage.innerHTML = 'Upload started...';
            };
            $("#filuploadProgress").show();
            request.upload.onprogress = function(event) {
                $("#filuploadProgress").find("div").html(Math.round(event.loaded / event.total * 100) + "%");
                $("#filuploadProgress").find("div").attr("aria-valuenow", Math.round(event.loaded / event.total * 100));
                $("#filuploadProgress").find("div").attr("style", "width:" + Math.round(event.loaded / event.total * 100) + "%");
            };

            request.upload.onload = function(result) {

                //if (($(".addprofilePhoto").length >= uploadIndex + 1) || ($(".addFamilyPhoto").length >= uploadIndex + 1)) {
                //    $(".blockUI").hide();
                //    LoadView('_PhotosVideos', 'tb-photosvideos');
                //}
                //++uploadIndex;
            }
            request.open('POST', url);
            request.send(data);
        }
        catch (ex) { alert(ex) }
    }

    $scope.filesModal = { 'Id': 0, 'FileType': '', 'File': '', 'Title': '', FileName: "", FileId: "", IsFileUpload: false }
    $scope.FileCollection = [];

    $scope.IsFileUpload = function(id) {
        return $scope.FileCollection.filter(function(x) { return x.FileId === id && x.IsFileUpload === true }).length > 0;
    }
    $scope.GetFileUploaded = function(id) {
        try {
            return $scope.FileCollection.filter(function(x) { return x.FileId === id && x.IsFileUpload === true })[0].FileName;
        } catch (ex) {
            return "";
        }
    }

    $scope.ChangeFile = function(id) {
        var FileCollection = $scope.FileCollection;
        $scope.FileCollection = [];
        FileCollection.forEach(function(item, index) {
            if (item.FileId != id) {
                $scope.FileCollection.push(item);
            }
        });
        $("#" + id).val("");
        $("#" + id).parent().parent().css('border', 'none');
    }

    $scope.txtBrndisabled = false;

    $scope.EditApplication = function (itm) {
        $scope.IsEditApplication = true;
        $scope.IsSocietyEdit = true;
        $scope.allDisable = false;
        $scope.SocietyAmendment.AmendmentId = itm.ChangeID;
        $scope.SocietyAmendment.ChangeByeLaws = $scope.SocietyNameChangeList[0].AmendmentType.indexOf('1') > -1 ? true : false;
        $scope.SocietyAmendment.ChangeName = $scope.SocietyNameChangeList[0].AmendmentType.indexOf('2') > -1 ? true : false;
        $scope.SocietyAmendment.ChaneAddress = $scope.SocietyNameChangeList[0].AmendmentType.indexOf('3') > -1 ? true : false;
        $scope.SocietyAmendment.ChaneExecutiveMember = $scope.SocietyNameChangeList[0].AmendmentType.indexOf('4') > -1 ? true : false;
        $scope.SocietyAmendment.AmendmentSection = $scope.SocietyNameChangeList[0].Section;
        // $scope.FileCollection = [];
        $scope.checkNameChange();
        $scope.SocietyAmendment.TotalMember = $scope.SocietyNameChangeList[0].TotalMember;
        $scope.SocietyAmendment.TotalPresantMember = $scope.SocietyNameChangeList[0].TotalPresantMember;
        $scope.SocietyAmendment.MeetingQuorum = $scope.SocietyNameChangeList[0].MeetingQuorum;
        $scope.SocietyAmendment.TotalVoteInSupport = $scope.SocietyNameChangeList[0].TotalVoteInSupport;
        $scope.SocietyAmendment.TotalVoteInOpposition = $scope.SocietyNameChangeList[0].TotalVoteInOpposition;

        $scope.SocietyAmendment.ResolutionNo = $scope.SocietyNameChangeList[0].ResolutionNo;


        if ($scope.SocietyAmendment.ChangeName) {
            $scope.SocietyAmendment.Name = $scope.SocietyNameChangeList[0].Name;
            $scope.ValidateBRNNo = $scope.SocietyNameChangeList[0].BRN;
            $scope.SocietyAmendment.BRN = $scope.SocietyNameChangeList[0].BRN;
            $scope.ValidateSocietyName = $scope.SocietyNameChangeList[0].Name;
            //$("#txtBrn").attr("disabled", "disabled");
            $("#btnValidate").hide();
            $("#btnChange").show();
            $scope.txtBrndisabled = true;
        }



        $scope.SocietyAmendment.PaasedAGMDate = $scope.SocietyNameChangeList[0].PaasedAGMDateSmall.replace(/-/g, '/');


        //if ($scope.SocietyNameChangeList[0].flResoloutionNotice != '') {
        //    $scope.FileCollection.push({ 'Id': 'flResoloutionNotice', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flResoloutionNotice, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flResoloutionNotice, FileId: 'flResoloutionNotice', IsFileUpload: true });
        //}
        //if ($scope.SocietyNameChangeList[0].flAttendanceSheet != '') {
        //    $scope.FileCollection.push({ 'Id': 'flAttendanceSheet', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flAttendanceSheet, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flAttendanceSheet, FileId: 'flAttendanceSheet', IsFileUpload: true });
        //}
        //if ($scope.SocietyNameChangeList[0].ByeLawsFile != '') {
        //    $scope.FileCollection.push({ 'Id': 'flBylawsFileName', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].ByeLawsFile, 'Title': '', FileName: $scope.SocietyNameChangeList[0].ByeLawsFile, FileId: 'flBylawsFileName', IsFileUpload: true });
        //}
        //if ($scope.SocietyNameChangeList[0].flByLowComparativeDocument != '') {
        //    $scope.FileCollection.push({ 'Id': 'flByLowComparativeDocument', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flByLowComparativeDocument, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flByLowComparativeDocument, FileId: 'flByLowComparativeDocument', IsFileUpload: true });
        //}
        //if ($scope.SocietyNameChangeList[0].flAGMDeliveryProof != '') {
        //    $scope.FileCollection.push({ 'Id': 'flAGMDeliveryProof', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flAGMDeliveryProof, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flAGMDeliveryProof, FileId: 'flAGMDeliveryProof', IsFileUpload: true });
        //}
        //if ($scope.SocietyNameChangeList[0].flSGMNotice != '') {
        //    $scope.FileCollection.push({ 'Id': 'flSGMNotice', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flSGMNotice, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flSGMNotice, FileId: 'flSGMNotice', IsFileUpload: true });
        //}
        //if ($scope.SocietyNameChangeList[0].flSGMAttendanceSheet != '') {
        //    $scope.FileCollection.push({ 'Id': 'flSGMAttendanceSheet', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flSGMAttendanceSheet, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flSGMAttendanceSheet, FileId: 'flSGMAttendanceSheet', IsFileUpload: true });
        //}

        if ($scope.SocietyNameChangeList[0].AmendmentType.indexOf('3') > -1) {

            $scope.SocietyRegisteredOffice.STATE_ID = $scope.SocietyNameChangeList[0].STATE_ID;
            $scope.SocietyRegisteredOffice.HouseBuildingNo = $scope.SocietyNameChangeList[0].HouseBuildingNo;
            $scope.SocietyRegisteredOffice.LaneStreetName = $scope.SocietyNameChangeList[0].LaneStreetName;
            $scope.SocietyRegisteredOffice.Locality = $scope.SocietyNameChangeList[0].Locality;
            $scope.SocietyRegisteredOffice.DIVISION_ID = $scope.SocietyNameChangeList[0].DIVISION_ID.toString();
            $scope.SocietyRegisteredOffice.DISTRICT_ID = $scope.SocietyNameChangeList[0].DISTRICT_ID.toString();
            $scope.SocietyRegisteredOffice.PinCode = $scope.SocietyNameChangeList[0].PinCode;
            $scope.GetDistrictList(1); //$scope.ResetDataForAddress(1, 2)

            if ($scope.SocietyNameChangeList[0].IsRural == 'True') {
                $scope.SocietyRegisteredOffice.IsRural == 1
                $scope.SocietyRegisteredOffice.CITY_ID = $scope.SocietyNameChangeList[0].CITY_ID;
                $scope.SocietyRegisteredOffice.WARD_ID = $scope.SocietyNameChangeList[0].WARD_ID;

            } else {
                $scope.SocietyRegisteredOffice.IsRural == 2
                $scope.SocietyRegisteredOffice.BLOCK_ID = $scope.SocietyNameChangeList[0].BLOCK_ID;
                $scope.SocietyRegisteredOffice.GP_ID = $scope.SocietyNameChangeList[0].GP_ID;
                $scope.SocietyRegisteredOffice.VILLAGE_ID = $scope.SocietyNameChangeList[0].VILLAGE_ID;
            }
        }

        if ($scope.SocietyNameChangeList[0].AmendmentType.indexOf('4') > -1) {
            $scope.GetMemberList().then(function () {

                var newTableData = [];
                var newBlanckRowData = [];
                $scope.MemberAmendmentList.forEach(function (item, indx) {
                    if (item.RequestType == 3) {
                        item.IsValidate = true;
                        item.IsNew = true;
                        item.Aadh = item.AadharNo;
                        item.NewAdministrativeId = item.AdministrativeId;
                        item.DOB = item.DOB1;
                        item.Status = item.MEMBERTYPE != undefined && item.MEMBERTYPE == 'ELECTEDMEMBERS' ? true : false;
                        item.Photo = '../Content/SocietyProfiling/' + $scope.SocietyDirectory + '/' + item.Photo;
                        if ($scope.TableData10All != undefined && $scope.TableData10All.length > 0) {
                            $scope.TableData10All.push(item);
                        }

                    } else if (item.RequestType == 2) {

                        if ($scope.TableData10All != undefined && $scope.TableData10All.length > 0) {
                            $scope.TableData10All.filter(function (x) { return x.AdministrativeId === item.oldAdministrativeId }).forEach(function (itm, ind) {
                                itm.ContactNo = item.ContactNo;
                                itm.Occupation = item.Occupation;
                                itm.CategoryId = item.CategoryId;
                                itm.CatId = item.CategoryId;
                                itm.POSTNAME = item.POSTNAME;
                                itm.NewAdministrativeId = item.AdministrativeId;
                                itm.Status = item.MEMBERTYPE != undefined && item.MEMBERTYPE == 'ELECTEDMEMBERS' ? true : false;
                            });
                        }

                    } else if (item.RequestType == 1) {
                        if ($scope.TableData10All != undefined && $scope.TableData10All.length > 0) {
                            var newItem = $scope.TableData10All.filter(function (x) { return x.AdministrativeId === item.oldAdministrativeId })[0];
                            var index1 = $scope.TableData10All.findIndex(x => x === newItem);
                            var RemovalFile1 = item.RemovalFile != undefined && item.RemovalFile != "" ? item.RemovalFile.split('/')[item.RemovalFile.split('/').length - 1] : "";
                            $scope.FileCollection.push({ 'Id': 'flRemovalType_' + index1, 'FileType': '', 'File': RemovalFile1, 'Title': '', FileName: RemovalFile1, FileId: 'flRemovalType_' + index1, IsFileUpload: true });
                            newItem.IsDelete = true;
                            newItem.NewAdministrativeId = item.AdministrativeId;
                            newItem.RemovalType = item.RemovalType.toString();
                        }
                    }
                });

                $(".se-pre-con").fadeOut("slow");
                //}
            });
        }
    }
      //  $scope.SocietyRegistration = {};
    $scope.BrnDetails1 = function() {


        if ($scope.SocietyAmendment.BRN == undefined || $scope.SocietyAmendment.BRN == '' || $scope.SocietyAmendment.BRN.length < 16 || $scope.SocietyAmendment.BRN.length > 16) {
            var Msgq = (_language == 'en-us' ? 'Please enter 16 digit BRN No.\n' : 'Please enter 16 digit BRN No.\n');
            ShowModelAlert("Society Amendment", Msgq);
            return;
        } else {
            $(".se-pre-con").fadeIn("slow");
            var formData = new FormData();
            formData.append("BRN", $scope.SocietyAmendment.BRN);
            formData.append("SocietyIdENC", $scope.SocietyAmendment.SocietyIdEnc.split('~')[0]);
            $http.post(SiteUrl + "/citizen/GetBrnDetails", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function(data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data.result == 'success') {
                        // $("#txtBrn").attr("disabled", "disabled");
                        $scope.txtBrndisabled = true;
                        $("#btnValidate").hide();
                        $("#btnChange").show();
                        //$("#txtBrn").removeAttr("disabled");
                        $scope.SocietyAmendment.Name = data.BO_Name;
                        $scope.ValidateBRNNo = data.BRN;
                        $scope.ValidateSocietyName = data.BO_Name;
                    }
                    else if (data.result == 'failed') {
                        $(".se-pre-con").fadeOut("slow");
                        ShowModelAlert("Society Profiling", data.BRN);
                    }
                    else if (data.result == 'invalidname') {
                        $(".se-pre-con").fadeOut("slow");
                        ShowModelAlert("Society Profiling", "Invalid Society Name: " + data.BO_Name);
                        $scope.CheckBRN = false;
                    }

                }).
                error(function(data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert("Society Profiling", "Error While Fetching BRN Details");
                });
        }
    }

    $scope.ChangeBRN = function() {
        $("#btnValidate").show(); $("#btnChange").hide();
        $scope.txtBrndisabled = false;//$("#txtBrn").removeAttr("disabled");
    }

    $scope.AmendmentDocuments = [];
    $scope.SocietyAmendment = { ChangeName: false };
    //$scope.AmendmentDocuments.push({ Id: 1, Name: "Notice to Registrar Signed by Secretary & Seven Members", FileId: "Notice_to_Registrar_Signed_by_Secretary_Seven_Members", IsShow: $scope.SocietyAmendment.ChangeName });
    $scope.AmendmentDocuments.push({ Id: 2, Name: "Board meeting resolution Notice with delivery proof", FileId: "Board_meeting_resolution_Notice_with_delivery_proof", IsShow: true });
    $scope.AmendmentDocuments.push({ Id: 3, Name: "Board meeting resolution MOM And Attendance Sheet", FileId: "Board_meeting_resolution_MOM_And_Attendance_Sheet", IsShow: true });
    $scope.AmendmentDocuments.push({ Id: 4, Name: "1st SGM(Special General meeting) Notice with delivery proof", FileId: "1st_SGM_Notice_with_delivery_proof", IsShow: true });
    $scope.AmendmentDocuments.push({ Id: 5, Name: "1st SGM(Special General meeting) MOM And Attendance Sheet", FileId: "1st_SGM_MOM_And_Attendance_Sheet", IsShow: true });
    $scope.AmendmentDocuments.push({ Id: 6, Name: "2nd SGM(Special General meeting) Notice with delivery proof", FileId: "2nd_SGM_Notice_with_delivery_proof", IsShow: true });
    $scope.AmendmentDocuments.push({ Id: 7, Name: "2nd SGM(Special General meeting) MOM And Attendance Sheet", FileId: "2nd_SGM_MOM_And_Attendance_Sheet", IsShow: true });
    $scope.AmendmentDocuments.push({ Id: 8, Name: "Comparative Document Of By-Laws", FileId: "Comparative_Document_Of_By_Laws", IsShow: true });
    // $scope.AmendmentDocuments.push({ Id: 8, Name: "Original Certificate To be deposited in Registrar office", FileId: "Original_Certificate_To_be_deposited_in_Registrar_office", IsShow:true });

    $scope.checkNameChange = function() {
        $scope.AmendmentDocuments = [];
        if ($scope.SocietyAmendment.ChangeName) {
            $scope.AmendmentDocuments.push({ Id: 1, Name: "Notice to Registrar Signed by Secretary & Seven Members", FileId: "Notice_to_Registrar_Signed_by_Secretary_Seven_Members", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 2, Name: "Board meeting resolution Notice with delivery proof", FileId: "Board_meeting_resolution_Notice_with_delivery_proof", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 3, Name: "Board meeting resolution MOM And Attendance Sheet", FileId: "Board_meeting_resolution_MOM_And_Attendance_Sheet", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 4, Name: "1st SGM(Special General meeting) Notice with delivery proof", FileId: "1st_SGM_Notice_with_delivery_proof", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 5, Name: "1st SGM(Special General meeting) MOM And Attendance Sheet", FileId: "1st_SGM_MOM_And_Attendance_Sheet", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 6, Name: "2nd SGM(Special General meeting) Notice with delivery proof", FileId: "2nd_SGM_Notice_with_delivery_proof", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 7, Name: "2nd SGM(Special General meeting) MOM And Attendance Sheet", FileId: "2nd_SGM_MOM_And_Attendance_Sheet", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 8, Name: "Comparative Document Of By-Laws", FileId: "Comparative_Document_Of_By_Laws", IsShow: true });

        } else if ($scope.SocietyAmendment.AmendmentSection == 4) {
            // $scope.AmendmentDocuments.push({ Id: 1, Name: "Notice to Registrar Signed by Secretary & Seven Members", FileId: "Notice_to_Registrar_Signed_by_Secretary_Seven_Members", IsShow: false});
            $scope.AmendmentDocuments.push({ Id: 1, Name: "Board meeting resolution Notice with delivery proof", FileId: "Board_meeting_resolution_Notice_with_delivery_proof", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 2, Name: "Board meeting resolution MOM And Attendance Sheet", FileId: "Board_meeting_resolution_MOM_And_Attendance_Sheet", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 3, Name: "AGM(Annual General meeting) Notice with delivery proof", FileId: "AGM_Notice_with_delivery_proof", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 4, Name: "AGM(Annual General meeting) MOM And Attendance Sheet", FileId: "AGM_MOM_And_Attendance_Sheet", IsShow: true });

        } else {
            // $scope.AmendmentDocuments.push({ Id: 1, Name: "Notice to Registrar Signed by Secretary & Seven Members", FileId: "Notice_to_Registrar_Signed_by_Secretary_Seven_Members", IsShow: false});
            $scope.AmendmentDocuments.push({ Id: 2, Name: "Board meeting resolution Notice with delivery proof", FileId: "Board_meeting_resolution_Notice_with_delivery_proof", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 3, Name: "Board meeting resolution MOM And Attendance Sheet", FileId: "Board_meeting_resolution_MOM_And_Attendance_Sheet", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 4, Name: "1st SGM(Special General meeting) Notice with delivery proof", FileId: "1st_SGM_Notice_with_delivery_proof", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 5, Name: "1st SGM(Special General meeting) MOM And Attendance Sheet", FileId: "1st_SGM_MOM_And_Attendance_Sheet", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 6, Name: "2nd SGM(Special General meeting) Notice with delivery proof", FileId: "2nd_SGM_Notice_with_delivery_proof", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 7, Name: "2nd SGM(Special General meeting) MOM And Attendance Sheet", FileId: "2nd_SGM_MOM_And_Attendance_Sheet", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 8, Name: "Comparative Document Of By-Laws", FileId: "Comparative_Document_Of_By_Laws", IsShow: true });
        }

    }

    /***************END Amendment**********************************************/

    $scope.ProfilingFileCollection = [];
    $scope.ProfilingFileCollection.push({ 'Id': "", 'FileType': '', 'File': '', 'Title': '', FileName: "", FileId: "", IsFileUpload: false });

    $scope.ProfilingDocuments = [];
    $scope.ProfilingDocuments.push({ Id: 1, NameEn: "Copy of the Registration Certificate issued from the Registrar Office", NameHi: "पंजीयक कार्यालय से जारी पंजीकरण प्रमाणपत्र की प्रति", FileId: "RegistrationCertificate", IsShow: true, Size: "1024", FileType: "application/pdf,image/*", Format: "" });
    $scope.ProfilingDocuments.push({ Id: 2, NameEn: "Upload copy of Objects of society", NameHi: "सोसाइटी के उद्देश्यों की प्रति अपलोड करें", FileId: "SocietyObjects", IsShow: true, Size: "5120", FileType: "application/pdf,image/*", Format: "/Content/SocietyProfiling/Objects.pdf" });
    $scope.ProfilingDocuments.push({ Id: 3, NameEn: "Copy of Rules and Regulations of society", NameHi: "सोसाइटी के नियमों और विनियमों की प्रति", FileId: "ByeLawsSocietyFile", IsShow: true, Size: "2048", FileType: "application/pdf,image/*", Format: "/Content/SocietyProfiling/rulesandregulation.pdf" });
    $scope.ProfilingDocuments.push({ Id: 4, NameEn: "Upload Excel Sheet of Members of the Society", NameHi: "सोसायटी के सदस्यों की एक्सेल शीट अपलोड करें", FileId: "MemberListExcelFile", IsShow: true, Size: "1024", FileType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel", Format: "/Content/SocietyExcel/MemberList.xlsx" });
    $scope.ProfilingDocuments.push({ Id: 5, NameEn: "Copy of the last executive issued from the Registrar Office", NameHi: "रजिस्ट्रार कार्यालय से जारी अंतिम कार्यकारी की प्रति", FileId: "OldExecutiveCommittee", IsShow: true, Size: "1024", FileType: "application/pdf,image/*", Format: "" });

    $scope.SocietyExecutiveCommitteeList = [];
    $scope.SocietyExecutiveCommitteeList.push({
        Id: 0,
        IsEdit: false,
        Aadh: "959813183295",
        Name: "Balwant",
        FatherName: "Mohan lal",
        ContactNo: "23423432343223",
        DOB: "20-09-1984",
        Gender: "Male",
        CategoryId: "3",
        CatId: "3",
        POSTNAME: "Adhayksh",
        Photo: "",
        Occupation: "dsadasd",
        MembershipNo: "",
        ShareAllotted: "",
        ShareCapitalAmount: "",
        loaneeType1: "",
        loaneeType1: "",
        Status: 1,
        IsValidate:true
    });
});

MasterMod.directive('validfile', function validFile() {
    var validFormats = ['jpg', 'gif'];
    return {
        require: 'ngModel',
        link: function(scope, elem, attrs, ctrl) {
            ctrl.$validators.validFile = function() {
                elem.on('change', function() {
                    if (elem[0].files.length > 0) {
                        const fsize = elem[0].files[0].size;
                        const file = Math.round((fsize / 1024));
                        var id = elem.attr("id");
                        var uploadType = elem.attr("dat-type");
                        if (uploadType != 'profile') {
                            // The size of the file. 
                            if (file >= 2048) {
                                ShowOkMessage("Warning", _language == 'en-us' ? "File too Big, please select a file less than 2mb" : "फ़ाइल बहुत बड़ी है, कृपया 2mb से कम फ़ाइल का चयन करें", function() { });
                                elem[0].value = "";
                            } else {

                                if (scope.FileCollection.filter(function(x) { return x.FileId === id }).length <= 0) {
                                    scope.FileCollection.push({ 'Id': id, 'FileType': '', 'File': '', 'Title': '', FileName: "", FileId: id, IsFileUpload: true });
                                } else {
                                    scope.FileCollection.forEach(function(itm, indx) {
                                        if (itm.FileId == id) {
                                            itm.IsFileUpload = true;
                                        }
                                    });
                                }

                            }
                        } else {
                            // The size of the file. 
                            var uploadType = parseInt(elem.attr("dat-filesize"));
                            var sizeinMB = uploadType / 1024;
                            if (file >= uploadType) {
                                ShowOkMessage("Warning", _language == 'en-us' ? "File too Big, please select a file less than " + sizeinMB + "mb" : "फ़ाइल बहुत बड़ी है, कृपया " + sizeinMB + "mb से कम फ़ाइल का चयन करें", function() { });
                                elem[0].value = "";
                            } else {

                                if (scope.FileCollection.filter(function(x) { return x.FileId === id }).length <= 0) {
                                    scope.FileCollection.push({ 'Id': id, 'FileType': '', 'File': '', 'Title': '', FileName: "", FileId: id, IsFileUpload: true });
                                } else {
                                    scope.FileCollection.forEach(function(itm, indx) {
                                        if (itm.FileId == id) {
                                            itm.IsFileUpload = true;
                                        }
                                    });
                                }
                            }
                        }
                        scope.$apply();
                    }
                });
            };
        }
    };
});