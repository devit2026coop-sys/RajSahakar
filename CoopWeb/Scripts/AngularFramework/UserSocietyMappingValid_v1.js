var MasterMod = angular.module('UserSocietyMappingValidModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("UserSocietyMappingValidCont", function ($scope, $http, $timeout) {

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
    $scope.SocietyAmendment = {};

    $scope.ResendOtp = function () {
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

    $scope.GetCategoryList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=-1").
            success(function (data, status) {
                $scope.CategoryList = data;
            }).
            error(function (data, status) {
                $scope.CategoryList = [];
            });
    }
    $scope.PostList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=-1").
            success(function (data, status) {
                $scope.PostData = data;
            }).
            error(function (data, status) {
                $scope.PostData = [];
            });
    };

    $scope.GetSocityDataForOtp = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.textchange = 'true';
        $scope.Showgride = false;
        $scope.showotpblock = false;
        $http.post(SiteUrl + "/Citizen/GetSocityDataForOtp2", { "SocietyRegistrationNo": $scope.SocietyRegistration.SocietyRegistrationNo, "DistrictId": $scope.ddlDistrct.DISTRICT_ID }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                console.log(data);
                $scope.showSoceitygride = true;
                var MessageHeading = _language === 'en-us' ? "User Society Mapping" : 'उपयोगकर्ता सोसायटी मानचित्रण';
                if (data.Result === false) {
                    $scope.Showgride = false;
                    $scope.textchange = false;
                    $scope.showSoceitygride = false;
                    var msg1 = 'आपकी सोसाइटी की जानकारी राज-सहकार पोर्टल पर उपलब्ध नहीं  है। संशोधन  का आवेदन करने से पहले आपको अपनी सोसाइटी की जानकारी दर्ज कर प्रोफाइलिंग करनी होगी।  <br/><span style="font-size:15px;color: red;"> Note:- आप द्वारा प्रोफाइलिंग के दौरान दर्ज की गयी जानकारी का विभाग द्वारा सत्यापन किया जायेगा उसके बाद आप संशोधन हेतु आवेदन कर पाएंगे। </span> '
                    ShowAlertMessage_v1("", "प्रोफाइलिंग करें", "रद्द करें", msg1, function () {
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
                        SetAddressinfo($scope.translation, 'MemberAdd', addressdataset1, $http).then(function () {

                        });

                        $scope.$apply();
                    });

                }
                else {
                    $scope.SocietyRegdata = [];
                    $scope.SocietyRegdata = data.data;
                    $scope.IsSearchResult = true;
                    $scope.IsShowProfiling = false;
                    $scope.IsShowAmendment = false;
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
                $(".se-pre-con").fadeOut("slow");
            });
        //$(".se-pre-con").fadeOut("slow");
    };
    $scope.BackToListClipk = function () {
        $scope.IsSearchResult = true;
        $scope.GetSocietyDetails = true;
        $scope.IsShowProfiling = false;
        $scope.IsShowAmendment = false;
    }
    $scope.SelectUserForVerify = function (items, index) {
        $scope.datasetUsers.forEach(function (item1, index) {
            item1.ischecked = false;
            $scope.Showwhenclick = true;
        });
        items.ischecked = true;
        $scope.divAADHARVerificationStep_1 = false;
        $scope.Showwhenclick = true;
        $scope.NowShowAdhardiv = items.ischecked;

    }
    $scope.getlistchairman = function (SocietyIdEnc) {
        //alert(SocietyIdEnc);
        $scope.GetUserListtoVerify(SocietyIdEnc);
        $scope.ShowAlertForUserwithData = true;

    }
    $scope.GetUserListtoVerify = function (SocietyIdEnc) {
        $http.get(SiteUrl + "/Citizen/GetUserListtoVerify/?SocietyIdEnc=" + SocietyIdEnc).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.datasetUsers = data;
                    $scope.ShowAlertForUserwithData = true;
                    console.log($scope.datasetUsers);
                }
            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
            });
    }
    $scope.SocietyIDEncget = "";
    //END : Het Society data for OTP
    $scope.getDis = false;
    $scope.SocietyRegdataSelected = [];
    $scope.showAmendmentSubmitedText = false;
    $scope.showAmendmentText = false;
    $scope.IsShowSSOMAPNoMAP = false
    $scope.SelectSocietyForVerify = function (item, index) {
        //alert(SocietyID);
        if (item.IsObjection) {
            ShowOkMessage("Alert", _language === 'en-us' ? "Please contact the concerned Registrar Office regarding this committee" : "कृपया इस समिति के संबंध में संबंधित रजिस्ट्रार ऑफिस से संपर्क करे । ", function () { });
            $(".se-pre-con").fadeOut("slow");
            return;
        }
        $scope.SocietyRegdata.forEach(function (item1, index) {
            item1.ischecked = false;
        });
        item.ischecked = true;
        var MessageHeading = _language === 'en-us' ? "User Society Mapping" : 'उपयोगकर्ता सोसायटी मानचित्रण';
        if (item.IsSsoFound === true && item.IsSameSSO === false) {
            var msg3 = 'यह सोसाइटी किसी और की SSO ID से मैप है। क्या आप इसे अपनी SSO ID से मैप करना चाहते है ?';
            $scope.SocietyRegdataSelected = [];
            ShowAlertMessage("", msg3, function () {
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
        } else if (item.IsSsoFound === false) {
            var data = item;
            $scope.SocietyAmendment.SocietyIdEnc = data.SocietyIDEnc + '~' + data.SocietyDirectory + '~' + data.ActId;
            $scope.IsSearchResult = false;
            $scope.IsShowProfiling = false;
            $scope.IsShowAmendment = false;
            $scope.IsShowSSOMAP = false;
            $scope.IsShowSSOMAPNoMAP = true
            $scope.GetSocietyDetails = false;
            $scope.IsWaitingForApproval = false;
            $scope.SocietyRegdataSelected.push(item);
            $scope.SocietyIDEncget = $scope.SocietyRegdataSelected[0].SocietyIDEnc;
            $scope.$apply();
        } else {
            ShowAlertMessage("", "आपके द्वारा चयन की गयी सोसाइटी आपकी SSO ID से पहले से मैप हैं। क्यां आप अपनी सोसाइटी की विस्तृत जानकारी देखना चाहते हैं ? ", function () {
                window.location = "../Citizen/SocietyApplicationForm";
            });

        }
    }
    //START : Verify AADHAr No.
    $scope.VerifyAADHARNo_Step_1_2_Old = function (AdhaNoPure) {

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


    $scope.VerifyAADHARNo_Step_1_2 = function (AdhaNoPure) {
        GetMemberDetails(AdhaNoPure)
        var tile = _language === 'en-us' ? 'Society Mapping Request' : 'सोसाइटी मानचित्रण अनुरोध';
        $scope.disablebtnSendOtp = true;
        JanAadhaarAuth("", $scope.BhamashahMemberDetail[0].AADHAR_REF_NO, "COOP", "", "", $http, false, "", "", function (data) {

            if (data && Object.keys(data).length > 0) {
                $(".se-pre-con").fadeOut("slow");
                $scope.GetCategoryList();
                //$scope.ActList();
                $scope.PostList();
                $scope.divAADHARVerificationStep_2 = false;
                $scope.GetSocietyDetails = false;
                $scope.Showgride2 = true;
                $scope.Showgride3 = false;
                $scope.$apply();
            }

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

    function GetMemberDetails(aadhaarNo) {
        $.ajax({
            url: "../citizen/GetJanAadhaarDetails",
            method: "POST",
            data: {
                JanaadhaarNo: encodeURIComponent(encryptData(aadhaarNo))
            },
            dataType: "json",
            async: false,
            success: function (data1) {
                if (Object.keys(data1).length == 0) {
                    var message = _language == 'en-us' ? 'Invalid Aadhaar Number \n' : 'अमान्य आधार संख्या \n';
                    alert(message);
                    $scope.BhamashahMemberDetail = [];
                }
                else {
                    if (data1[0].msg === "Unexpected JSON token when reading DataTable. Expected StartArray, got StartObject. Path '', line 1, position 1.") {
                        var message = _language == 'en-us' ? 'Invalid Aadhaar Number \n' : 'अमान्य आधार संख्या \n';
                        alert(message);
                        $scope.BhamashahMemberDetail = [];
                    }
                    else {
                        $scope.BhamashahMemberDetail = data1;
                        $scope.IsJanAadharDataFetched = true;

                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                window.location = "~/Error/General";
            }
        });
    }


    $scope.selectSSOMAPCeo = function (item) {
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
        GetMemberDetails(AdhaNoPure);
        JanAadhaarAuth("", $scope.BhamashahMemberDetail[0].AADHAR_REF_NO, "COOP", "", "", $http, false, "", "", function (data) {

            if (data && Object.keys(data).length > 0) {
                if (data.Gender == 'Male')
                    data.Gender = 'M';
                else if (data.Gender == 'Female')
                    data.Gender = 'F'
                else
                    data.Gender = 'T';
                $(".se-pre-con").fadeOut("slow");
                $scope.MemberRegistration["Name"] = data.Name;
                $scope.MemberRegistration["FatherName"] = data.FatherName;
                //var newdate = data.AADHARDetails[0].Column4;
                $scope.MemberRegistration["DOB"] = data.DOB;
                $scope.MemberRegistration["AADHARNo"] = AdhaNoPure;
                $scope.MemberRegistration["Gender"] = data.Gender;
                $scope.MemberRegistration["Photo"] = data.Photo;
                $scope.MemberRegistration["Address"] = data.Address;
                $scope.MemberRegistration["SocietyId"] = $scope.SocietyId;
                $scope.MemberRegistration["AADHARNoEnc"] = data.AADHARNoEnc??null;
                $scope.GetCategoryList();
                $scope.ShowAADHARNoForRequest = true;
                $scope.divAADHARVerificationStep_2 = false;
                $scope.GetSocietyDetails = false;
                $scope.Showgride2 = true;
                $scope.Showgride3 = false;
                $scope.$apply();
            }

        }, true, $scope._language == 'en-us' ? "INVALID AADHAAR No. OR SERVICE IS DOWN. Do you want to map this society.? If yes, click on Yes button." : "अमान्य आधार नंबर या सेवा बंद है | क्यां आप इस सोसाइटी को आधिकारिक रूप से मैप करना चाहते हैं | यदि हाँ तो Yes बटन पर क्लिक करें |", function () {
            $scope.MemberRegistration["SocietyId"] = $scope.SocietyId;
            $scope.SocietyNameForDecalre = $scope.SocietyRegdata[0].SocietyName;
            $scope.divAADHARVerificationStep_2 = false;
            $scope.GetSocietyDetails = false;
            $scope.Showgride2 = false;
            $scope.Showgride3 = true;
            $scope.$apply();
        });


        //AadhaarAuth(AdhaNoPure, $http, "OTP-BIO", false, function (data) {
        //    $scope.MemberRegistration["Name"] = data.Name;
        //    $scope.MemberRegistration["FatherName"] = data.FName;
        //    //var newdate = data.AADHARDetails[0].Column4;
        //    $scope.MemberRegistration["DOB"] = data.DOB;
        //    $scope.MemberRegistration["AADHARNo"] = $scope.MemberRegistration.AADHARNo;
        //    $scope.MemberRegistration["Gender"] = data.Gender;
        //    $scope.MemberRegistration["Photo"] = data.Photo;
        //    $scope.MemberRegistration["Address"] = data.Address;
        //    $scope.MemberRegistration["SocietyId"] = $scope.SocietyId;
        //    $scope.MemberRegistration["AADHARNoEnc"] = data.AADHARNoEnc;
        //    $scope.GetCategoryList();
        //    //$scope.ActList();
        //    //$scope.PostList();
        //    $scope.ShowAADHARNoForRequest = true;
        //    $scope.divAADHARVerificationStep_2 = false;
        //    $scope.GetSocietyDetails = false;
        //    $scope.Showgride2 = true;
        //    $scope.Showgride3 = false;
        //    $scope.$apply();
        //}, true, $scope._language == 'en-us' ? "INVALID AADHAAR No. OR SERVICE IS DOWN. Do you want to map this society.? If yes, click on Yes button." : "अमान्य आधार नंबर या सेवा बंद है | क्यां आप इस सोसाइटी को आधिकारिक रूप से मैप करना चाहते हैं | यदि हाँ तो Yes बटन पर क्लिक करें |", function () {
        //    $scope.MemberRegistration["SocietyId"] = $scope.SocietyId;
        //    $scope.SocietyNameForDecalre = $scope.SocietyRegdata[0].SocietyName;
        //    $scope.divAADHARVerificationStep_2 = false;
        //    $scope.GetSocietyDetails = false;
        //    $scope.Showgride2 = false;
        //    $scope.Showgride3 = true;
        //    $scope.$apply();
        //});

        $scope.divAADHARVerificationStep_1 = false;
        $scope.divAADHARVerificationStep_2 = false;


    };

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
    $scope.DistrictList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Districts = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=" + '0').
            success(function (data, status) {
                $scope.divDisticts = true;
                $scope.Districts = data;
                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                    }
                    return '';
                };
            }).
            error(function (data, status) {
                $scope.Districts = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    $scope.OnPageLoad();
    $scope.DistrictList();
    $scope.SocietyDetails = [];
    $scope.MapRegistrationNo = "";
    $scope.UploadFile = function (id, type, index, model, UploadType, fileext) {
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
    $scope.xhr = function (url, data, type, index, model, UploadType) {
        try {
            var request = new XMLHttpRequest();
            request.onreadystatechange = function () {
                if (request.readyState == 4 && request.status == 200) {
                    $(".blockUI").hide();
                    $("#filuploadProgress").hide();
                    $("#filuploadProgress").find("div").html("");
                    $("#filuploadProgress").find("div").attr("aria-valuenow", "1");
                    $("#filuploadProgress").find("div").attr("style", "width:1%");
                    if (request.responseText == "") {
                        ShowOkMessage("Error", $scope == 'en-us' ? "There is some problem in uploading file on server. Please try after some time" : "सर्वर पर फ़ाइल अपलोड करने में कुछ समस्या है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    } else {
                        $scope.FileCollection.forEach(function (itm, indx) {
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


            request.upload.onloadstart = function () {
                //percentage.innerHTML = 'Upload started...';
            };
            $("#filuploadProgress").show();
            request.upload.onprogress = function (event) {
                $("#filuploadProgress").find("div").html(Math.round(event.loaded / event.total * 100) + "%");
                $("#filuploadProgress").find("div").attr("aria-valuenow", Math.round(event.loaded / event.total * 100));
                $("#filuploadProgress").find("div").attr("style", "width:" + Math.round(event.loaded / event.total * 100) + "%");
            };

            request.upload.onload = function (result) {

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

    $scope.xhrProfile = function (url, data, type, index, model, UploadType) {
        try {
            var request = new XMLHttpRequest();
            request.onreadystatechange = function () {
                if (request.readyState == 4 && request.status == 200) {
                    $(".blockUI").hide();
                    $("#filuploadProgress").hide();
                    $("#filuploadProgress").find("div").html("");
                    $("#filuploadProgress").find("div").attr("aria-valuenow", "1");
                    $("#filuploadProgress").find("div").attr("style", "width:1%");

                    if (request.responseText.indexOf('@') > -1) {
                        ShowOkMessage("Error", request.responseText.split('@')[1], function () { });
                    } else
                        if (request.responseText == "") {
                            ShowOkMessage("Error", $scope == 'en-us' ? "There is some problem in uploading file on server. Please try after some time" : "सर्वर पर फ़ाइल अपलोड करने में कुछ समस्या है। कृपया कुछ देर बाद प्रयास करें", function () { });
                        } else {
                            $scope.FileCollection.forEach(function (itm, indx) {
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


            request.upload.onloadstart = function () {
                //percentage.innerHTML = 'Upload started...';
            };
            $("#filuploadProgress").show();
            request.upload.onprogress = function (event) {
                $("#filuploadProgress").find("div").html(Math.round(event.loaded / event.total * 100) + "%");
                $("#filuploadProgress").find("div").attr("aria-valuenow", Math.round(event.loaded / event.total * 100));
                $("#filuploadProgress").find("div").attr("style", "width:" + Math.round(event.loaded / event.total * 100) + "%");
            };

            request.upload.onload = function (result) {

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

    $scope.IsFileUpload = function (id) {
        return $scope.FileCollection.filter(function (x) { return x.FileId === id && x.IsFileUpload === true }).length > 0;
    }
    $scope.GetFileUploaded = function (id) {
        try {
            return $scope.FileCollection.filter(function (x) { return x.FileId === id && x.IsFileUpload === true })[0].FileName;
        } catch (ex) {
            return "";
        }
    }

    $scope.ChangeFile = function (id) {
        var FileCollection = $scope.FileCollection;
        $scope.FileCollection = [];
        FileCollection.forEach(function (item, index) {
            if (item.FileId != id) {
                $scope.FileCollection.push(item);
            }
        });
        $("#" + id).val("");
        $("#" + id).parent().parent().css('border', 'none');
    }


});

MasterMod.directive('validfile', function validFile() {
    var validFormats = ['jpg', 'gif'];
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$validators.validFile = function () {
                elem.on('change', function () {
                    if (elem[0].files.length > 0) {
                        const fsize = elem[0].files[0].size;
                        const file = Math.round((fsize / 1024));
                        var id = elem.attr("id");
                        var uploadType = elem.attr("dat-type");
                        if (uploadType != 'profile') {
                            // The size of the file. 
                            if (file >= 2048) {
                                ShowOkMessage("Warning", _language == 'en-us' ? "File too Big, please select a file less than 2mb" : "फ़ाइल बहुत बड़ी है, कृपया 2mb से कम फ़ाइल का चयन करें", function () { });
                                elem[0].value = "";
                            } else {

                                if (scope.FileCollection.filter(function (x) { return x.FileId === id }).length <= 0) {
                                    scope.FileCollection.push({ 'Id': id, 'FileType': '', 'File': '', 'Title': '', FileName: "", FileId: id, IsFileUpload: true });
                                } else {
                                    scope.FileCollection.forEach(function (itm, indx) {
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
                                ShowOkMessage("Warning", _language == 'en-us' ? "File too Big, please select a file less than " + sizeinMB + "mb" : "फ़ाइल बहुत बड़ी है, कृपया " + sizeinMB + "mb से कम फ़ाइल का चयन करें", function () { });
                                elem[0].value = "";
                            } else {

                                if (scope.FileCollection.filter(function (x) { return x.FileId === id }).length <= 0) {
                                    scope.FileCollection.push({ 'Id': id, 'FileType': '', 'File': '', 'Title': '', FileName: "", FileId: id, IsFileUpload: true });
                                } else {
                                    scope.FileCollection.forEach(function (itm, indx) {
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