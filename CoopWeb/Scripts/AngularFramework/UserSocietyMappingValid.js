
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

    $scope.ResendOtp = function () {
        //alert('');
        $scope.showResendBtn = true;
        $scope.VerifyAADHARNo_Step_1_2();
    };

    $scope.divAADHARVerificationStep_1 = false;
    //START : Het Society data for OTP
    $scope.getdisable = false;
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
                    var msg1 = _language === 'en-us' ? 'Your Society is not added. Do you want to add your society ?' : 'आपकी सोसायटी को जोड़ा नहीं गया है। क्या आप सोसायटी को जोड़ना चाहते हैं?';
                    if (confirm(msg1)) {
                        window.location = "../Citizen/SocietyApplicationForm";
                    }
                    else {
                        return false;
                    }

                }
                else {
                    $scope.SocietyRegdata = [];
                    $scope.SocietyRegdata = data.data;
                    //    if (data.data[0].IsSsoFound === true && data.data[0].IsSameSSO === false && data.data.length==1) {
                    //        $scope.getdisable = true;
                    //        var msg3 = _language === 'en-us' ? 'This society is already mapped with SSO other than you.Would you like to file a complaint?' : 'यह सोसाइटी पहले से ही आपके अलावा अन्य एसएसओ के साथ मैप हो चुकी है। क्या आप शिकायत दर्ज करना चाहेंगे ?';
                    //        if (confirm(msg3)) {
                    //            $scope.disableTxtRegNo = true;
                    //            var complaintMessage = _language === 'en-us' ? 'For Complaint file following details required.' : 'शिकायत दर्ज करने के लिए निम्नलिखित विवरण आवश्यक है।';
                    //           // $scope.SocietyId = data.data[0].SocietyIDEnc;

                    //            if (data.data[0].ActId.length > 0) {
                    //                $scope.IsActdisabled = true;
                    //                $scope.MemberRegistration.ActId = data.data[0].ActId;
                    //            }
                    //            else {
                    //                $scope.IsActdisabled = true;
                    //            }

                    //            ShowModelAlert(MessageHeading, complaintMessage);
                    //            $scope.getotp = true;
                    //            $scope.SocietyRegdata = [];
                    //            $scope.SocietyRegdata = data;
                    //            $scope.textchange = 'true';
                    //            $scope.divAADHARVerificationStep_1 = false;
                    //            $scope.isDisabled = false;
                    //            $scope.MemberRegistration.AADHARNo = data.data[0].LoggedInUserMaskedAADHAR;
                    //            $scope.MemberRegistration.AdhaNoPure = data.data[0].LoggedInUserAADHAR;
                    //            $scope.MemberRegistration.AADHARNoEnc = data.data[0].AADHARNoEnc;

                    //        }

                    //        else {
                    //            return;
                    //        }

                    //    }
                    //    else if (data.data[0].IsSsoFound === true && data.data[0].IsSameSSO === true && data.data.length == 1) {
                    //        var msg5 = _language === 'en-us' ? 'You have already mapped this society with your own SSO Id.' : 'आप पहले ही अपने SSO Id से इस सोसाइटी की मैपिंग कर चुके हैं।';
                    //        ShowModelAlert(MessageHeading, msg5);
                    //        return;
                    //    }
                    //    else {
                    //        $scope.disableTxtRegNo = true;
                    //        var msg4 = _language === 'en-us' ? 'For society profiling following details required.' : 'सोसायटी प्रोफाइलिंग के लिए निम्नलिखित विवरण आवश्यक है।';
                    //        //$scope.SocietyId = data.data[0].SocietyIDEnc;

                    //        if (data.data[0].ActId.length > 0) {
                    //            $scope.IsActdisabled = true;
                    //            $scope.MemberRegistration.ActId = data.data[0].ActId;
                    //        }
                    //        else {
                    //            $scope.IsActdisabled = true;
                    //        }

                    //        //ShowModelAlert(MessageHeading, msg4);
                    //        $scope.getotp = true;
                    //        $scope.SocietyRegdata = [];
                    //        $scope.SocietyRegdata = data.data;
                    //        $scope.textchange = 'true';
                    //        $scope.divAADHARVerificationStep_1 = false;
                    //        $scope.isDisabled = false;
                    //        //$scope.MemberRegistration.AADHARNo = data.data[0].LoggedInUserMaskedAADHAR;
                    //        //$scope.MemberRegistration.AdhaNoPure = data.data[0].LoggedInUserAADHAR;                        
                    //    }

                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
                $(".se-pre-con").fadeOut("slow");
            });
        //$(".se-pre-con").fadeOut("slow");
    };
    //END : Het Society data for OTP
    $scope.RequestForMAPList = [];
    $scope.getListAmenmentList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/citizen/GetSSOMapRequestList").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.RequestForMAPList = data;
            }).
            error(function (data, status) {
                $scope.RequestForMAPList = [];
            });

    }
    $scope.SSOMAPRequest = { RequestId: "0", TransferDistraict: "", TransferRemarks: "", RequestBy: 0, MapSSOId:""};
    $scope.showTransModel = function (item) {
     
        $("#transModals").modal('show');
        $scope.SSOMAP = item;
        $scope.SSOMAPRequest.RequestBy = item.RequestBy;
        $scope.SSOMAPRequest.RequestId = item.RequestId;      
        $scope.SSOMAPRequest.TransferDistraict = "";
        $scope.SSOMAPRequest.TransferRemarks = "";
    }



    $scope.SubmitSSOMAPRequest = function () {
        
       // formData.append("isAmendment", $scope.SSOMAPRequest.isAmendment == undefined ? "0" : $scope.SSOMAPRequest.isAmendment);
       

        if ($scope.SSOMAPRequest.RequestBy == "-1" && ($scope.SSOMAPRequest.MapSSOId == undefined || $scope.SSOMAPRequest.MapSSOId == "")) {
                var msg = _language == 'en-us' ? 'Please enter SSO ID' : 'Please enter SSO ID';
                ShowOkMessage("", msg, function () { });
            } else
            if ($scope.SSOMAPRequest.Status == undefined || $scope.SSOMAPRequest.Status == "") {
                var msg = _language == 'en-us' ? 'Please select status' : 'Please select status';
                ShowOkMessage("", msg, function () { });
            } else if ($scope.SSOMAPRequest.TransferRemarks == undefined || $scope.SSOMAPRequest.TransferRemarks == "") {
                var msg = _language == 'en-us' ? 'Please enter remark for request' : 'Please enter remark for request';
                ShowOkMessage("", msg, function () { });
            } else {
                $(".se-pre-con").fadeIn("slow");

                //var URL = "UpdateCCSCompaintTransferRequest";
                
                $http.post(SiteUrl + "/Society/UpdateSocietyMAPRequest", { SocietyId: $scope.SSOMAP.SocietyIDInc, SSOId: $scope.SSOMAPRequest.MapSSOId == undefined ? "" : $scope.SSOMAPRequest.MapSSOId, RequestId: $scope.SSOMAPRequest.RequestId, Remarks: $scope.SSOMAPRequest.TransferRemarks, Status: $scope.SSOMAPRequest.Status, isAmendment : $scope.SSOMAPRequest.isAmendment == undefined ? "0" : $scope.SSOMAPRequest.isAmendment }).
                    success(function (data, status) {
                        if (data == 1) {
                            $(".se-pre-con").fadeOut("slow");
                            $("#transModals").modal('hide');
                            ShowOkMessage("", "Request has been successfully updated.", function () { $scope.GetSocietyForMap() });
                        } else {
                            $(".se-pre-con").fadeOut("slow");
                            ShowOkMessage("", "There is some problem please try after some time.", function () { });
                        }
                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        ShowOkMessage("", "There is some problem please try after some time.", function () { });
                    });
            
        }
    }


    //$scope.PopupforApprove = function () {
    //    ShowAlertMessage("", $scope._language == 'en-us' ? 'Do you want approve.? If yes, click on Yes button.' : 'क्या आप इसे मंजूर करना चाहते हैं| यदि हाँ तो Yes बटन पर क्लिक करें |

    //}
    $scope.PopupforApprove = function () {
        var msgs = 'Do you want approve.? If yes, click on Yes button.';
        ShowAlertMessage("", msgs, function () {
            $scope.IsSearchResult = false;
            $scope.IsShowProfiling = true;
            $scope.IsShowAmendment = false;
            $scope.divOtherSocietys == true;
            $scope.GetSocietyDetails = false;
            $scope.IsProfiling = true;
            $scope.SubmitSocietyForMap(Society);

        });
    }

    $scope.getDis = false;
    $scope.SelectSocietyForVerify = function (item,index) {
        //alert(SocietyID);
        $scope.SocietyRegdata.forEach(function (item1, index) {
            item1.ischecked = false;
        });
        item.ischecked = true;
        var MessageHeading = _language === 'en-us' ? "User Society Mapping" : 'उपयोगकर्ता सोसायटी मानचित्रण';
        if (item.IsSsoFound === true && item.IsSameSSO === false) {          
            var msg3 = _language === 'en-us' ? 'This society is already mapped with SSO other than you. Please contact the relevant department to map your society with your SSO ID.' : 'यह सोसाइटी पहले से ही आपके अलावा अन्य एसएसओ के साथ मैप हो चुकी है। कृपया अपनी सोसाइटी को SSO ID  से मैप करने के लिए संबंधित विभाग से संपर्क करें|';
            ShowOkMessage(MessageHeading, msg3, function () { });
        }
        else if (item.IsSsoFound === true && item.IsSameSSO === true) {
            var msg5 = _language === 'en-us' ? 'You have already mapped this society with your own SSO Id.' : 'आप पहले ही अपने SSO Id से इस सोसाइटी की मैपिंग कर चुके हैं।';
            ShowOkMessage(MessageHeading, msg5, function () { });          
        }
        else {
            var newScoietyId = item.SocietyID;
            $scope.divAADHARVerificationStep_1 = true;
            $scope.Showgride3 = false;
            $scope.getDis = true;
            console.log($scope.SocietyRegdata);
            //$scope.SocietyRegdata = $scope.SocietyRegdata.filter(x => x.SocietyID == newScoietyId);
            $scope.MemberRegistration.AADHARNo = (item.AadharNo == (undefined || "") ? item.LoggedInUserMaskedAADHAR : item.AadharNo);
            $scope.MemberRegistration.AdhaNoPure = (item.AdhaNoPure == (undefined || "") ? item.LoggedInUserAADHAR : item.AdhaNoPure);
            $scope.MemberRegistration.AADHARNoEnc = item.AADHARNoEnc;
            $scope.MemberRegistration.SocietyId = item.SocietyID;
            $scope.SocietyId = item.SocietyIDEnc;
            $scope.RegistrationNo = item.RegistrationNo;
        }
    }
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
        $scope.MemberRegistration.AdhaNoPure = $scope.MemberRegistration.AdhaNoPure.replace(/ /g, '');
        AadhaarAuth($scope.MemberRegistration.AdhaNoPure, $http, "OTP-BIO", false,function (data) {            
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
            $scope.ActList();
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

        //$scope.isDisabled = true;
        //$timeout(function () {
        //    $scope.isDisabled = false;
        //}, 10000);

        //$scope.countDown = 10;
        //var timer = setInterval(function () { $scope.countDown--; if ($scope.countDown < -1) { $scope.isDisabled = false; return; } }, 1000);

        //$(".se-pre-con").fadeOut("slow");

        //Bye Pass END
       // $scope.MemberRegistration.AdhaNoPure="432534469557";
        //$(".se-pre-con").fadeIn("slow");
        //$http.post(SiteUrl + "/SocietyMemberRegistration/GenerateOTPForPLDOTPBMemberRegistration", { _AadharNo: $scope.MemberRegistration.AdhaNoPure }).
        //    success(function (data, status) {
        //        $(".se-pre-con").fadeOut("slow");
        //        if (data.Result === "1" || data.Result === true) {
        //            $scope.MemberRegistration.txn = data.txnid;
        //            $scope.MemberRegistration.AADHARNoEnc = data.AadharNo;
        //                $scope.divAADHARVerificationStep_1 = false;
        //            $scope.divAADHARVerificationStep_2 = true;
        //            //$(".se-pre-con").fadeOut("slow");
        //            var msg1 = _language === 'en-us' ? 'THE OTP HAS BEEN SENT TO AADHAR REGISTERED MOBILE NUMBER' : 'ओटीपी आधार पंजीकृत मोबाइल नंबर पर भेजा गया है';
        //            ShowOkMessage(tile, msg1, function () { });
        //        }
        //        else {
        //            //$(".se-pre-con").fadeOut("slow");
        //            var msg2 = _language === 'en-us' ? 'ERROR !!! INVALID AADHAAR No. OR SERVICE IS DOWN' : 'त्रुटि !!! अमान्य आधार नंबर या सेवा बंद है ';
        //            //msg2 = (data.Message).splt('#')[1];
        //            //ShowOkMessage(tile, msg2, function () { });
        //            $scope.divAADHARVerificationStep_1 = true;
        //            $scope.divAADHARVerificationStep_2 = false;
        //            $scope.isDisabled = false;

        //            ShowAlertMessage("", $scope._language == 'en-us' ? 'INVALID AADHAAR No. OR SERVICE IS DOWN. Do you want to map this society.? If yes, click on Yes button.' : 'अमान्य आधार नंबर या सेवा बंद है | क्यां आप इस सोसाइटी को आधिकारिक रूप से मैप करना चाहते हैं | यदि हाँ तो Yes बटन पर क्लिक करें |', function () {
        //                //$scope.MemberRegistration["Name"] = _language === 'en-us' ? data.AADHARDetails[0].Column2 : data.AADHARDetails[0].Column6;
        //                //$scope.MemberRegistration["FatherName"] = _language === 'en-us' ? data.AADHARDetails[0].Column8 : data.AADHARDetails[0].Column9;
        //                //var newdate = data.AADHARDetails[0].Column4;
        //                //$scope.MemberRegistration["DOB"] = newdate;
        //                //$scope.MemberRegistration["AADHARNo"] = $scope.MemberRegistration.AADHARNo;
        //                //$scope.MemberRegistration["Gender"] = data.AADHARDetails[0].Column3;
        //                //$scope.MemberRegistration["Photo"] = data.AADHARDetails[0].Column5;
        //                //$scope.MemberRegistration["Photo"] = data.AADHARDetails[0].Column5;
        //                //$scope.MemberRegistration["Address"] = data.AADHARDetails[0].Column10;
        //                $scope.MemberRegistration["SocietyId"] = $scope.SocietyId;
        //                //$scope.GetCategoryList();
        //                //$scope.ActList();
        //                //$scope.PostList();
        //                $scope.SocietyNameForDecalre= $scope.SocietyRegdata[0].SocietyName;
        //                $scope.divAADHARVerificationStep_2 = false;
        //                $scope.GetSocietyDetails = false;
        //                $scope.Showgride2 = false;
        //                $scope.Showgride3 = true;
        //                $scope.$apply();
        //            });


        //        }
        //    }).
        //    error(function (data, status) {
        //        $(".se-pre-con").fadeOut("slow");
        //    });
      
    };
    // END : Verify AADHAr No.

    //START : Verify AADHAr No.
    $scope.VerifyAADHARNo_Step_2_2 = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.MemberRegistration.RegNo = $scope.SocietyRegistration.SocietyRegistrationNo;
        var tile = _language === 'en-us' ? 'Society Mapping/Objection Request' : 'सोसाइटी मानचित्रण/आपत्ति अनुरोध';

        if ($scope.MemberRegistration.EnterOTP == undefined || $scope.MemberRegistration.EnterOTP.length != 6) {
            $(".se-pre-con").fadeOut("slow");
            var msg = _language === 'en-us' ? 'Please enter 6 digit OTP' : '6 अंकों की ओटीपी दर्ज करें';
            ShowOkMessage(tile, msg, function () { });
            return;
        }
        // Un-comment below code after testing
        $http.post(SiteUrl + "/SocietyMemberRegistration/ValidateOTPForPLDBMemberRegistration", { "otp": $scope.MemberRegistration.EnterOTP, "txn": $scope.MemberRegistration.txn, "_AadharNo": $scope.MemberRegistration.AADHARNoEnc }).
            success(function (data, status) {
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
                    $scope.ActList();
                    $scope.PostList();
                    $scope.divAADHARVerificationStep_2 = false;
                    $scope.GetSocietyDetails = false;
                    $scope.Showgride2 = true;
                    $scope.Showgride3 = false;
                }
                else {
                    return;
                }
            }).
            error(function (data, status) {
                $scope.SocietyRegdatanew = [];
                $(".se-pre-con").fadeOut("slow");
            });

    }
    // END : Verify AADHAr No.


    //START : Validate Society Mapping Request Data
    $scope.ValidateSocietyMappingRequestData = function () {
        var Msg = '';
        var tile = _language === 'en-us' ? 'Society Mapping Request' : 'सोसाइटी मानचित्रण अनुरोध';

        //if ($scope.MemberRegistration.ActId === '0') {
        //    Msg += _language === 'en-us' ? 'Please select Act</br>' : 'कृपया अधिनियम का चयन करें</br>';
        //}


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
        $scope.MemberRegistration.RegNo = $scope.RegistrationNo;
        $(".se-pre-con").fadeIn("slow");
        if (!$scope.ValidateSocietyMappingRequestData()) {
            $(".se-pre-con").fadeOut("slow");
            return;
        }

        $http.post(SiteUrl + "/Citizen/SocietyMappingInsert", { model: $scope.MemberRegistration }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                var title = _language === 'en-us' ? 'Society Mapping Request' : 'सोसाइटी मानचित्रण अनुरोध';
                if (parseInt(data, 10) > 0) {
                    alert(_language === 'en-us' ? 'Society Mapping Request accepted successfully.' : 'सोसाइटी मैपिंग अनुरोध सफलतापूर्वक स्वीकार किया गया', function () { window.location = '../citizen/usersocietymappingvalid'; });
                    window.location = "../Citizen/Index";
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
    $scope.GetSocietyForMap = function () {
        //if ($scope.MapRegistrationNo  == '') {
        //    ShowOkMessage("", "Please Enter Registration Number", function () { });
        //} else {
        $(".blockUI").fadeIn("slow");
        return $http.post("/citizen/GetSSOMapRequestList", JSON.stringify({ SearchText: $scope.MapRegistrationNo })).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.RequestForMAPList = data;
            }).
            error(function (data, status) {
                $scope.RequestForMAPList = [];
                $(".blockUI").fadeOut("slow");
            });
        //}
    };

    //$scope.isAmendment = function (Amendmen) {
    //    $scope.Amendmen = Amendmen;
        
        
        
      
    //    //}
    //};

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
        $scope.GetSocietyForMap();
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
 

    $scope.SubmitSocietyForMap = function (Society) {
        var MapSSOId = Society.MapSSOId == undefined ? "" : Society.MapSSOId.replace(/ /g, '');
        if (MapSSOId == '') {
            ShowOkMessage("", "Please Enter SSO ID ", function () { });
        } else {
            $(".blockUI").fadeIn("slow");
            return $http.post("/Society/SubmitSocietyForMap", JSON.stringify({ SocietyId: Society.SocietyIDInc, SSOId: MapSSOId})).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (data == true) {
                        ShowOkMessage("", "Society Mapped Successfully with SSO ID " + MapSSOId + " ", function () { window.location.href ="../Society/MapSociety" });
                    } else {
                        ShowOkMessage("", "There is some problem while updating society with new SSO Id. Please Try after some time", function () { });
                    }
                    
                }).
                error(function (data, status) {                   
                    $(".blockUI").fadeOut("slow");
                });
        }
    }


    $scope.SubmitCitizonSocietyForMap = function (Society) {
        var MapSSOId = $("#hdnSSOID").val();
        //if (MapSSOId == '') {
        //    ShowOkMessage("", "Please Enter SSO ID ", function () { });
        //} else {
            $(".blockUI").fadeIn("slow");
        return $http.post("/Society/SubmitSocietyForMap", JSON.stringify({ SocietyId: $scope.MemberRegistration.SocietyId, SSOId: MapSSOId })).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (data == true) {
                        ShowOkMessage("", "Society Mapped Successfully with SSO ID " + MapSSOId + " ", function () { window.location.href = "../Citizen/UserSocietyMappingValid" });
                    } else {
                        ShowOkMessage("", "There is some problem while updating society with new SSO Id. Please Try after some time", function () { });
                    }

                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                });
        //}
    }


});






