var CommityRegistrtionModule = angular.module('CommityRegistrtionMod', ['angularUtils.directives.dirPagination']);

CommityRegistrtionModule.controller('CommityRegistrtionCnt', function ($scope, $http, $timeout) {

    $scope.divUserRegistration = true;
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.MemberDatail = false;
    $scope.LandDetailsGSS = [];
    $scope.SownCropDetailGSS = [];
    $scope.CommercialLoanDetail = [];
    $scope.IsCommercialBankDetailsShow = false;
    $scope.DairyDatail = false;
    $scope.divViewDairyFormShow = false;
    $scope.divParentDataSelection = true;
    $scope.divAADHARVerificationStep_1 = false;
    $scope.divAADHARVerificationStep_2 = false;
    $scope.divGssDairyParentdataSelection = true;
    $scope.divPLDBParentDataSelection = false;
    $scope.disablePrimarySelection = false;
    $scope.disablebtnSendOtp = false;


    function GSSLandDetailEntity() {
        this.VillageName = '';
        this.AccountNo = '';
        this.KhasaraNo = '';
        this.TotalLandInHactor = 0;
        this.IrrigationLand = 0;
        this.NonIrrigationLand = 0;
        this.PartInIrrigationLand = 0;
        this.PartInNonIrrigationLand = 0;
    }

    function GSSCropDetailEntity() {
        this.LandInHactor = 0;
        this.IsIrrigated = false;
        this.SownCropName = '';
        this.IsKharif = false;
    }

    function CommercialLoanDetailEntity() {
        this.InstitutionName = '';
        this.ApprovedLoanAmt = 0;
        this.RemainingLoanAmtInTime = 0;
        this.RemainingLoanAmtTimeBarred = 0;
    }

    //START : Show Hide User Registration Form
    $scope.ShowhideUserRegistrtionForm = function () {
        if ($scope.MemberRegistration.OrganizationId === '0') {
            $scope.divUserRegistration = false;
        }
        else {
            $scope.divUserRegistration = true;
        }
        $scope.ClearUserRegistrtionForm();
    };
    //START : Show Hide User Registration Form

    //START : Clear User registration Form
    $scope.ClearUserRegistrtionForm = function () {
        $scope.MemberRegistration.UserId = '0';
        $scope.MemberRegistration.OrganizationId = '0';
        $scope.MemberRegistration.Name = '';
        $scope.MemberRegistration.Age = '';
        $scope.MemberRegistration.Address = '';
        $scope.MemberRegistration.Gender = '';
        $scope.MemberRegistration.AADHARNo = '';
        $scope.MemberRegistration.Business = '';
        $scope.MemberRegistration.MobileNo = '';
        $scope.MemberRegistration.EmailId = '';
        $scope.MemberRegistration.AnimalCount = '';
        $scope.MemberRegistration.BuyCount = '';
        $scope.MemberRegistration.DepositedAmount = '';
        $scope.MemberRegistration.Disability = '';

    };
    //START : Clear User registration Form

    // START : Code for Language label setting
    $scope.getTranslation = function () {
        _language = 'hi-in';
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
        }

        $scope.GetOrganizationList();
        $scope.GetDistrictWithoutDivisionList();
        var url = window.location.href.split('?');
        if (url.length > 1) {
            var url2 = url[1].substr(17, url[1].toString().length);
            if (url2.indexOf('Error') > 0) {
                alert('some error occurred Please Try after some time');
                window.location = '../Citizen/SocietyMemberRegistration';
            }
            else
                $scope.GetMemberDetailsList(url2);
        }
        else if ($('#EUserType').val() == 'C') {
            $scope.divGrid = true;
            $scope.divViewShow = false;
            $scope.divRegistration = false;
            $scope.IsCitizen = true;
            $scope.GetSocietyMemberData();
        }
        else {
            $scope.divViewShow = false;
            $scope.divRegistration = true;
            $scope.divGrid = false;
            $scope.IsCitizen = false;
        }
        $scope.BackBtnLast = true;
    };
    // END : Code for Language label setting

    $scope.GetSocietyMemberData = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.SocietyMemberData = [];
        $http.get(SiteUrl + "/SocietyMemberRegistration/GetOrganizationUserRegistrationDetails?ApplicationIdEnc=" + "").
            success(function (data, status) {
                $scope.SocietyMemberData = data;
            }).
            error(function (data, status) {
                $scope.SocietyMemberData = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // START : Get All Organization List
    $scope.GetOrganizationList = function () {
        $scope.OrganizationList = [];
        $http.post(SiteUrl + "/SocietyMemberRegistration/GetOrganizationList").
            success(function (data, status) {
                $scope.OrganizationList = data;
            }).
            error(function (data, status) {
                $scope.OrganizationList = [];
            });
    };
    // START : Get All Organization List
    //Start :  Get List of GramPanchayat
    $scope.GramPanchayatListForBasicD = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.MemberDatail = false;
        $scope.GramPanchayats = [];
        $scope.SocietyList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.MemberRegistration.BLOCK_ID).
            success(function (data, status) {
                $scope.GramPanchayats = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.GramPanchayats = [];
                $(".se-pre-con").fadeOut("slow");

            });
        $scope.MemberRegistration.GP_ID = '0';
        $(".se-pre-con").fadeOut("slow");
    };
    //End
    // Get List of Block
    $scope.BlockList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Blocks = [];
        $scope.GramPanchayats = [];
        $scope.SocietyList = [];
        $scope.MemberDatail = false;
        if ($scope.MemberRegistration.DISTRICT_ID != '0') {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.MemberRegistration.DISTRICT_ID).
                success(function (data, status) {
                    $scope.Blocks = data;
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                    $(".se-pre-con").fadeOut("slow");
                });
        }
        $(".se-pre-con").fadeOut("slow");
        $scope.MemberRegistration.BLOCK_ID = '0';
        $scope.GramPanchayats = [];
        $scope.MemberRegistration.GP_ID = '0';
        $scope.SocietyList = [];
        $scope.MemberRegistration.OrganizationId = '0';
    };
    //end block
    //START : Get District List
    $scope.GetDistrictWithoutDivisionList = function () {
        $scope.Blocks = [];
        $scope.GramPanchayats = [];
        $scope.DistrictList = [];
        $scope.SocietyList = [];
        $scope.MemberDatail = false;
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    // END
    //Start :  Get List of Society
    $scope.GetSocietyByGP_ID = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.MemberDatail = false;
        $scope.SocietyList = [];
        $http.get(SiteUrl + "/Citizen/GetSocietyByGP_ID/?GP_ID=" + $scope.MemberRegistration.GP_ID + "&SocietyCatyegoryId=" + $scope.MemberRegistration.RegistrationType).
            success(function (data, status) {
                $scope.SocietyList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
        $scope.MemberRegistration.OrganizationId = '0';
    };
    //End
    $scope.ShowMemberDetail = function () {

        $scope.divAADHARVerificationStep_1 = true;
        $scope.disablePrimarySelection = true;

        //if ($scope.MemberRegistration.RegistrationType == '1') {
        //    $scope.MemberDatail = true;
        //    $scope.DairyDatail = false;
        //}
        //else {
        //    $scope.MemberDatail = false;
        //    $scope.DairyDatail = true;
        //}

        //$scope.divParentDataSelection = false;
        //$scope.divViewShow = false;
        //$scope.divGrid = false;
        //$scope.divRegistration = true;
        //$scope.BackBtnLast = false;
        //$scope.GetVillageList();    
    };


    //START : Verify AADHAr No.
    $scope.VerifyAADHARNo_Step_1_2 = function () {

        $scope.disablebtnSendOtp = true;
        if ($scope.MemberRegistration.AADHARNo == undefined || $scope.MemberRegistration.AADHARNo.length < 12) {
            var msg = _language === 'en-us' ? 'Please enter 12 digit AADHAAR No.' : '12 अंकों की आधार संख्या दर्ज करें';
            $scope.disablebtnSendOtp = false;
            ShowOkMessage('', msg);
            return;
        }


        $scope.divAADHARVerificationStep_1 = false;
        $scope.divAADHARVerificationStep_2 = false;
        $scope.MemberDatail = false;
        $scope.DairyDatail = false;
        $scope.PLDBMemberDatail = false;

        //Bye Pass START
        //$(".se-pre-con").fadeIn("slow");
        //var msg1 = _language === 'en-us' ? 'THE OTP HAS BEEN SENT TO AADHAR REGISTERED MOBILE NUMBER' : 'ओटीपी आधार पंजीकृत मोबाइल नंबर पर भेजा गया है';
        //$scope.divAADHARVerificationStep_1 = false;
        //$scope.divAADHARVerificationStep_2 = true;
        ////ShowOkMessage('', msg1);

        //$scope.isDisabled = true;
        //$timeout(function () {
        //    $scope.isDisabled = false;
        //}, 10000);

        //$scope.countDown = 10;
        //var timer = setInterval(function () { $scope.countDown--; if ($scope.countDown < -1) { $scope.isDisabled = false; return; } }, 1000);  

        //$(".se-pre-con").fadeOut("slow");
        //Bye Pass END

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/SocietyMemberRegistration/GenerateOTPForPLDOTPBMemberRegistration", { _AadharNo: $scope.MemberRegistration.AADHARNo }).
            success(function (data, status) {

                if (data.Result) {
                    $scope.MemberRegistration.txn = data.txnid;
                    $scope.MemberRegistration.AADHARNoEnc = data.AadharNo;
                    $scope.divAADHARVerificationStep_1 = false;
                    $scope.divAADHARVerificationStep_2 = true;
                    var msg1 = _language === 'en-us' ? 'THE OTP HAS BEEN SENT TO AADHAR REGISTERED MOBILE NUMBER' : 'ओटीपी आधार पंजीकृत मोबाइल नंबर पर भेजा गया है';
                    alert(msg1);
                    $(".se-pre-con").fadeOut("slow");
                }
                else {
                    $(".se-pre-con").fadeOut("slow");
                    var msg2 = _language === 'en-us' ? 'INVALID AADHAAR No.' : 'अमान्य आधार नंबर';
                    alert(msg2);
                }
            }).
            error(function (data, status) {

            });
        $(".se-pre-con").fadeOut("slow");
    };
    // END : Verify AADHAr No.

    //START : Verify AADHAr No.
    $scope.VerifyAADHARNo_Step_2_2 = function () {

        if ($scope.MemberRegistration.EnterOTP == undefined || $scope.MemberRegistration.EnterOTP.length != 6) {
            var msg = _language === 'en-us' ? 'Please enter 6 digit OTP' : '6 अंकों की ओटीपी दर्ज करें';
            alert(msg);
            return;
        }

        // START : Test for Verification
        //var data = { "Result": true, "Message": "Success", "AadharNo": "AeS5cagS6t6LxgjZpCq7FQ==", "txnid": "", "AADHARDetails": [{ "Column1": "Success", "Column2": "Narottam Saini", "Column3": "M", "Column4": "22-11-1985", "Column5": "../Content/NewRegistration/member_a8b5165053a245a9a4d9d095db7af468.jpg", "Column6": "नरोत्तम सैनी", "Column7": "AeS5cagS6t6LxgjZpCq7FQ==", "Column8": "S/O Om Prakash Saini", "Column9": "S/O  ओम प्रकाश सैनी" }] };

        //if (data.Result) {
        //    $scope.divAADHARVerificationStep_1 = false;
        //    $scope.divAADHARVerificationStep_2 = false;
        //    $scope.divParentDataSelection = true;
        //    alert($scope.MemberRegistration.RegistrationType);

        //    if ($scope.MemberRegistration.RegistrationType === '1') {
        //        $scope.MemberDatail = true;
        //        $scope.DairyDatail = false;
        //        $scope.PLDBMemberDatail = false;
        //    }
        //    else if ($scope.MemberRegistration.RegistrationType === '2') {
        //        alert($scope.MemberRegistration.RegistrationType);
        //        $scope.MemberDatail = false;
        //        $scope.DairyDatail = true;
        //        $scope.PLDBMemberDatail = false;
        //    }
        //    else if ($scope.MemberRegistration.RegistrationType === '3') {
        //        $scope.MemberDatail = false;
        //        $scope.DairyDatail = true;
        //        $scope.PLDBMemberDatail = true;
        //    }

        //    $scope.divViewShow = false;
        //    $scope.divGrid = false;
        //    $scope.divRegistration = true;
        //    $scope.BackBtnLast = false;
        //    $scope.GetVillageList();
            
        //    $scope.MemberRegistration.Name = data.AADHARDetails[0].Column2;
        //    $scope.MemberRegistration.FatherHusbandName = data.AADHARDetails[0].Column9;

        //    var newdate = new Date(data.AADHARDetails[0].Column4.split('-')[1] + '-' + data.AADHARDetails[0].Column4.split('-')[0] + '-' + data.AADHARDetails[0].Column4.split('-')[2]);
        //    var diff = ((newdate).getTime() - (new Date()).getTime()) / 1000;
        //    diff /= (60 * 60 * 24);

        //    //$('#txtRegistrationDate').val().split("/").reverse().join("-"));

        //    $scope.MemberRegistration.DOB = data.AADHARDetails[0].Column4.split("-").reverse().join("-");
        //    $scope.MemberRegistration.Age = Math.abs(Math.round(diff / 365.25));
        //    $scope.MemberRegistration.Gender = data.AADHARDetails[0].Column3 === 'M' ? '1' : (data.AADHARDetails[0].Column3 === 'F' ? 2 : 3);
        //    $scope.MemberRegistration.PhotoPath = data.AADHARDetails[0].Column5;
        //}
        // END : Test for Verification


        // Un-comment below code after testing
        $http.post(SiteUrl + "/SocietyMemberRegistration/ValidateOTPForPLDBMemberRegistration", { "otp": $scope.MemberRegistration.EnterOTP, "txn": $scope.MemberRegistration.txn, "_AadharNo": $scope.MemberRegistration.AADHARNoEnc }).
            success(function (data, status) {
                console.log(data);
                if (data.Result === false) {
                    alert(data[0].Column1 + ': Due To' + data[0].Column2);
                    return;
                }
                else if (data.Result === true) {
                    $scope.divAADHARVerificationStep_1 = false;
                    $scope.divAADHARVerificationStep_2 = false;
                    $scope.divParentDataSelection = false;


                    if ($scope.MemberRegistration.RegistrationType === '1') {
                        $scope.MemberDatail = true;
                        $scope.DairyDatail = false;
                        $scope.PLDBMemberDatail = false;
                    }
                    else if ($scope.MemberRegistration.RegistrationType === '2') {
                        $scope.MemberDatail = false;
                        $scope.DairyDatail = true;
                        $scope.PLDBMemberDatail = false;
                    }
                    else if ($scope.MemberRegistration.RegistrationType === '3') {
                        $scope.MemberDatail = false;
                        $scope.DairyDatail = true;
                        $scope.PLDBMemberDatail = true;
                    }

                    $scope.divParentDataSelection = false;
                    $scope.divViewShow = false;
                    $scope.divGrid = false;
                    $scope.divRegistration = true;
                    $scope.BackBtnLast = false;
                    $scope.GetVillageList();

                    // [{"Column1":"Success","Column2":"Balwant Chaudhary","Column3":"M","Column4":"20-09-1984","Column5":"../Content/NewRegistration/member_259ad6e690d04f6a90bac2a08afbf25e.jpg","Column6":"बलवंत चौधरी","Column7":"T1+Bhe8Ju9s4QjBaM66sAg==","Column8":" Mohan Lal Chaudhary","Column9":" मोहन लाल चौधरी"}]
                    //GENDER
                    $scope.MemberRegistration.Name = data.AADHARDetails[0].Column2;
                    $scope.MemberRegistration.FatherHusbandName = data.AADHARDetails[0].Column9;

                    var newdate = new Date(data.AADHARDetails[0].Column4.split('-')[1] + '-' + data.AADHARDetails[0].Column4.split('-')[0] + '-' + data.AADHARDetails[0].Column4.split('-')[2]);
                    var diff = ((newdate).getTime() - (new Date()).getTime()) / 1000;
                    diff /= (60 * 60 * 24);

                    //$('#txtRegistrationDate').val().split("/").reverse().join("-"));

                    $scope.MemberRegistration.DOB = data.AADHARDetails[0].Column4.split("-").reverse().join("-");
                    $scope.MemberRegistration.Age = Math.abs(Math.round(diff / 365.25));
                    $scope.MemberRegistration.Gender = data.AADHARDetails[0].Column3 == 'M' ? '1' : (data[0].Column3 == 'F' ? 2 : 3);
                    $scope.MemberRegistration.PhotoPath = data.AADHARDetails[0].Column5;
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

    $scope.ShowForm = function () {
        $scope.divParentDataSelection = true;
        $scope.DairyDatail = false;
        $scope.MemberDatail = false;
        $scope.divViewDairyFormShow = false;
        $scope.divViewShow = false;
        $scope.divRegistration = true;
        $scope.divGrid = false;
        $scope.ChangeType();
    };

    $scope.CommercialBankLoneDetailsCapture = function () {

        if ($scope.MemberRegistration.IsOtherBankLonee == '0') {
            $scope.IsCommercialBankDetailsShow = true;
        }
        else {
            $scope.IsCommercialBankDetailsShow = false;
            $scope.MemberRegistration["OtherBankLoanDetails"] = [];
            $scope.CommercialLoanDetail = [];
        }
    };


    $scope.ViewMemberApplication = function (ReqIdEnc) {
        // alert(ReqIdEnc);
        window.location = '../Citizen/MemberApplicationView?ApplicationIdEnc=' + ReqIdEnc;
    };




    // START : Get All All List
    $scope.GetMemberDetailsList = function (IdEnc) {

        $http.get(SiteUrl + "/SocietyMemberRegistration/GetOrganizationUserRegistrationDetails?ApplicationIdEnc=" + IdEnc).
            success(function (data, status) {
                if (data.SocietyMemberRegistration[0].ApplicationTypeId == 2) {
                    $scope.divViewDairyFormShow = true;
                    $scope.divViewShow = false;
                    $scope.MemberRegistration = data.SocietyMemberRegistration[0];
                }
                else {
                    $scope.MemberRegistration = data.SocietyMemberRegistration[0];
                    $scope.LandDetailsGSS = data.SocietyMemberLandDetail;
                    $scope.SownCropDetailGSS = data.SocietyMemberSownCropDetail;
                    $scope.CommercialLoanDetail = data.SocietyMemberOtherBankLoanDetail;
                    $scope.divViewDairyFormShow = false;
                    $scope.divViewShow = true;
                }
                if (data.PGResponse != null && data.PGResponse != "") {
                    var temp = [];
                    temp.push(data.PGResponse);
                    if (temp.length > 0) {
                        if ($scope.MemberRegistration.KioskID == 'SSO' ? (temp[0].RESPONSEMESSAGE.indexOf('Success') > 0) : (temp[0].MSG.indexOf('Success') > 0)) {
                            $scope.IsPaymentDone = true;
                            $scope.TRANSACTIONID = temp[0].TRANSACTIONID;
                            $scope.ReceiptNo = temp[0].RECEIPTNO;
                            $scope.BANK_NAME = temp[0].BANK_NAME;
                            $scope.BANK_ACC_NO = temp[0].BANK_ACC_NO;
                            if ($scope.MemberRegistration.KioskID == 'SSO') {
                                $scope.IsCityzen = true;
                                $scope.IsKiosk = false;
                                $scope.Fee = temp[0].PAIDAMOUNT;
                                $scope.RESPONSEMESSAGE = temp[0].RESPONSEMESSAGE;
                            }
                            else {
                                $scope.RESPONSEMESSAGE = temp[0].MSG;
                                $scope.Fee = temp[0].TRANSAMT;
                                $scope.IsCityzen = false;
                                $scope.IsKiosk = true;
                            }

                        }
                    }
                }

                //$scope.MemberDetailsList = data[0];
                //$scope.MemberRegistration = data[0];
                //$scope.divViewShow = true;
                //$scope.divRegistration = false;
                //$scope.divGrid = false;
            }).
            error(function (data, status) {
                $scope.MemberDetailsList = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    // START : Get All All List

    // START : Get All All List
    $scope.UpdateMemberDetails = function (item) {
        $scope.MemberRegistration.OrganizationId = item.OrganizationId;
        $scope.MemberRegistration.Name = item.Name;
        $scope.MemberRegistration.Age = item.Age;
        $scope.MemberRegistration.Address = item.Address;
        $scope.MemberRegistration.Gender = item.Gender;
        $scope.MemberRegistration.AADHARNo = item.AADHARNo;
        $scope.MemberRegistration.Business = item.Business;
        $scope.MemberRegistration.MobileNo = item.MobileNo;
        $scope.MemberRegistration.EmailId = item.EmailId;
        $scope.MemberRegistration.AnimalCount = item.AnimalCount;
        $scope.MemberRegistration.BuyCount = item.BuyCount;
        $scope.MemberRegistration.DepositedAmount = item.DepositedAmount;
        $scope.MemberRegistration.IsDisability = item.IsDisability === 'True' ? true : false;
        $scope.MemberRegistration.OrganizationId = item.OrganizationId;
        $scope.MemberRegistration.UserId = item.UserId;
        $scope.divUserRegistration = true;
    };
    // START : Get All All List


    //START : Validate PLDB Member Registration Form
    $scope.ValidatePLDBMemberRegistration = function () {
        var msg = '';

        if ($scope.MemberRegistration.PLDB_ID == undefined || $scope.MemberRegistration.PLDB_ID == '0') {
            msg += _language == 'en-us' ? 'Please select PLDB\n' : 'कृपया पी.एल.डी.बी. का चयन करें |\n';
        }

        if ($scope.MemberRegistration.Branch_ID == undefined || $scope.MemberRegistration.Branch_ID == '0') {
            msg += _language == 'en-us' ? 'Please select Branch\n' : 'कृपया ब्रांच का चयन करें |\n';
        }

        if ($scope.MemberRegistration.Name == undefined || $scope.MemberRegistration.Name.length < 1) {
            msg += _language == 'en-us' ? 'Please write applicant Name\n' : 'कृपया आवेदक का नाम लिखें |\n';
        }

        if ($scope.MemberRegistration.FatherHusbandName == undefined || $scope.MemberRegistration.FatherHusbandName.length < 1) {
            msg += _language == 'en-us' ? 'Please write Husband/Father Name\n' : 'कृपया पिता/पति का नाम लिखें|\n';
        }

        if ($scope.MemberRegistration.Age == undefined || $scope.MemberRegistration.Age.length < 1 || $scope.MemberRegistration.Age < 1) {
            msg += _language == 'en-us' ? 'Please write Age\n' : 'कृपया उम्र दर्ज करें |\n';
        }

        if ($scope.MemberRegistration.EducationalQualificationId == undefined || $scope.MemberRegistration.EducationalQualificationId == '0') {
            msg += _language == 'en-us' ? 'Please select Education Qualification\n' : 'कृपया शैक्षणिक अर्हताए (योग्यता) का चयन करें |\n';
        }

        if ($scope.MemberRegistration.CategoryId == undefined || $scope.MemberRegistration.CategoryId == '0') {
            msg += _language == 'en-us' ? 'Please select Category\n' : 'कृपया वर्ग चयन करें|\n';
        }

        if ($scope.MemberRegistration.Address == undefined || $scope.MemberRegistration.Address.length < 1) {
            msg += _language == 'en-us' ? 'Please write Permanat Address\n' : 'कृपया निवास का स्थाई पता दर्ज करें |\n';
        }

        if ($scope.MemberRegistration.CurrentAddress == undefined || $scope.MemberRegistration.CurrentAddress.length < 1) {
            msg += _language == 'en-us' ? 'Please write Current Address\n' : 'कृपया निवास का वर्तमान पता दर्ज करें |\n';
        }

        if ($scope.MemberRegistration.NomineeName == undefined || $scope.MemberRegistration.NomineeName.length < 1) {
            msg += _language == 'en-us' ? 'Please write Nominee Name with Father/Husband Name\n' : 'कृपया नामनिर्देशित का नाम और उसके पिता / पति का नाम दर्ज करें |\n';
        }

        if ($scope.MemberRegistration.NomineeRelationId == undefined || $scope.MemberRegistration.NomineeRelationId == '0') {
            msg += _language == 'en-us' ? 'Please select relation with nominee\n' : 'कृपया आवेदक के साथ नामनिर्देशिति का सम्बन्थ चयन करें|\n';
        }

        if ($scope.MemberRegistration.MobileNo == undefined || $scope.MemberRegistration.MobileNo.length != 10) {
            msg += _language == 'en-us' ? 'Please write 10 digit Mobile No.\n' : 'कृपया 10 अंकों का मोबाइल नंबर दर्ज करें|\n';
        }

        if ($scope.MemberRegistration.AADHARNo == undefined || $scope.MemberRegistration.AADHARNo.length != 12) {
            msg += _language == 'en-us' ? 'Please write AADHAAR No.\n' : 'कृपया आधार संख्या दर्ज करें|\n';
        }

        if ($scope.MemberRegistration.Business == undefined || $scope.MemberRegistration.Business.length < 1) {
            msg += _language == 'en-us' ? 'Please wite business\n' : 'कृपया व्‍यवसाय दर्ज करें |\n';
        }


        if (msg.length > 0) {
            alert(msg);
            return false;
        }

        return true;
    }
    //END : Validate PLDB Member Registration Form

    //START : ShowDeclaration
    $scope.ShowDeclaration = function () {
        if (!$scope.ValidatePLDBMemberRegistration()) {
            return false;
        }

        $scope.divAADHARVerificationStep_1 = false;
        $scope.divAADHARVerificationStep_2 = false;
        //$scope.divParentDataSelection = false;
        $scope.PLDBMemberDatail = false;
        $scope.divViewShowPLDB = true;
        $scope.EducationalQualification = $('#ddlEducationQualification :selected').text().trim();
        $scope.Category = $('#ddlCategory :selected').text().trim();
        $scope.NomineeRelation = $('#ddlNomineeRelation :selected').text().trim();
    }
    // END : ShowDeclaration






    // START : Save User
    $scope.SaveUser = function () {

        var Msg = '';

        //if ($scope.MemberRegistration.Gender === undefined) {
        //    Msg += (_language === 'en-us' ? 'Please select gender \n' : 'कृपया लिंग चुनें\n');
        //}

        //if ($scope.MemberRegistration.IsDisability === undefined) {
        //    Msg += (_language === 'en-us' ? 'Choose- Are you afflicted with any qualification for membership?\n' : 'क्या आप सदस्यता हेतु किसी नियोग्यता से ग्रसित है अथवा नहीं चुनें\n');
        //}

        if (Msg.length > 1) {
            alert(Msg);
            return;
        }

        $scope.MemberRegistration["MembershipFee"] = 100;
        $scope.MemberRegistration["ContributionAmt"] = 11;

        $scope.MemberRegistration["BLOCK_ID"] = $scope.MemberRegistration.BLOCK_ID;
        $scope.MemberRegistration["DISTRICT_ID"] = $scope.MemberRegistration.DISTRICT_ID;

        if ($scope.MemberRegistration.RegistrationType == '1') {
            $scope.MemberRegistration.IsOtherBankLonee = $scope.MemberRegistration.IsOtherBankLonee == '0' ? true : false;
        }



        //IsOtherBankLonee


        $http.post(SiteUrl + "/SocietyMemberRegistration/SaveOrganizationUserRegistrationDetails", { _Entity: $scope.MemberRegistration }).
            success(function (data, status) {
               
                // $scope.MemberDetailsList = data[0];
                //window.location = '../Citizen/SocietyMemberRegistration';
                //$scope.MemberRegistration = $scope.UserRegistration;
                //$scope.MemberRegistration.SocietyName = $('#ddlSociety option:selected').text();
                //$scope.MemberRegistration.DISTRICT_ENG = $('#ddlDistrict option:selected').text();
                //$scope.MemberRegistration.DISTRICT_MANGAL = $('#ddlDistrict option:selected').text();
                //$scope.MemberRegistration.BLOCK_ENG = $('#ddlblock option:selected').text();
                //$scope.MemberRegistration.BLOCK_MANGAL = $('#ddlblock option:selected').text();
                //$scope.MemberRegistration.GP_ENG = $('#ddlGP option:selected').text();
                //$scope.MemberRegistration.GP_MANGAL = $('#ddlGP option:selected').text();
                //$scope.divViewShow = true;
                //$scope.divRegistration = false;
                //$scope.divGrid = false;
                //$scope.EMitraTransactionId = 0;
                $scope.AppRequestID = data[0].RequestId;
            }).
            error(function (data, status) {
                alert('Error occurred.');
            });


    };
    // START : Save User

    $scope.ApplicationEdit = function () {
        if ($scope.MemberRegistration.RegistrationType == '1') {
            $scope.DairyDatail = false;
            $scope.MemberDatail = true;
        }
        else {
            $scope.DairyDatail = true;
            $scope.MemberDatail = false;
        }
        $scope.divRegistration = true;
        $scope.divViewShow = false;
        $scope.divViewDairyFormShow = false;
        $scope.divGrid = false;
    }
    // START : Change Status
    $scope.SetStatus = function (UserId, IsActive) {
        IsActive = (IsActive === 'True' ? false : true);
        $http.post(SiteUrl + "/SocietyMemberRegistration/ChangeOrganizationUserStatus", { "RequestId": UserId, "IsActive": IsActive }).
            success(function (data, status) {
                alert(data);
            }).
            error(function (data, status) {
                $scope.MemberDetailsList = [];
            });
    };
    // START : Change Status

    // START : Get Relation List
    $scope.GetBloodRelationList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=RELATION&MasterCode=-1").
            success(function (data, status) {
                $scope.BloodRelation = data;
            }).
            error(function (data, status) {
                $scope.BloodRelation = [];
            });
    }
    // END: Get Relation List


    // START : Get Econiomic  List
    $scope.GetEconomicSectionList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ECONOMICGROUP&MasterCode=-1").
            success(function (data, status) {
                $scope.EconomicSection = data;
            }).
            error(function (data, status) {
                $scope.EconomicSection = [];
            });
    }
    // END: Get Econiomic List

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

    // START : Get Category  List
    $scope.GetVillageList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=VILLAGE&MasterCode=" + $scope.MemberRegistration.GP_ID).
            success(function (data, status) {
                $scope.VillageList = data;
            }).
            error(function (data, status) {
                $scope.VillageList = [];
            });
    }
    // END: Get Category List

    //START : AddNewLandRecordGSS
    $scope.AddNewLandRecordGSS = function () {
        //GSSLandArray = $scope.SownCropDetailGSS;
        $scope.LandDetailsGSS.push(new GSSLandDetailEntity());
    }
    //END : AddNewLandRecordGSS

    //START : RemoveLandRecordGSS
    $scope.RemoveLandRecordGSS = function (index) {
        $scope.LandDetailsGSS.splice(index, 1);
    }
    //END : RemoveLandRecordGSS

    //START : AddNewRecordSownCropDetail
    $scope.AddNewRecordSownCropDetail = function () {
        $scope.SownCropDetailGSS.push(new GSSCropDetailEntity());
    }
    //END : AddNewRecordSownCropDetail

    //START : RemoveSownCropDetailGSS
    $scope.RemoveSownCropDetailGSS = function (index) {
        $scope.SownCropDetailGSS.splice(index, 1);
    }
    //END : RemoveSownCropDetailGSS

    //START : AddNewRecordCommercialLoanDetailGSS
    $scope.AddNewRecordCommercialLoanDetailGSS = function () {
        $scope.CommercialLoanDetail.push(new CommercialLoanDetailEntity());
    }
    //END : AddNewRecordCommercialLoanDetailGSS

    //START : RemoveCommercialLoanDetailGSS
    $scope.RemoveCommercialLoanDetailGSS = function (index) {
        $scope.CommercialLoanDetail.splice(index, 1);
    }
    //END : RemoveCommercialLoanDetailGSS


    //START : Validate GSS data()SaveGSSDataC
    $scope.ValidateGSSData = function () {

        var msg = '';

        if ($scope.MemberRegistration.RegistrationType == undefined) {
            msg += 'कृपया प्रकार का चयन करें |\n';
        }

        if ($scope.MemberRegistration.DISTRICT_ID == undefined || $scope.MemberRegistration.DISTRICT_ID == '0') {
            msg += 'कृपया ज़िला का चयन करें |\n';
        }

        if ($scope.MemberRegistration.BLOCK_ID == undefined || $scope.MemberRegistration.BLOCK_ID == '0') {
            msg += 'कृपया ब्लाक  का चयन करें |\n';
        }

        if ($scope.MemberRegistration.GP_ID == undefined || $scope.MemberRegistration.GP_ID == '0') {
            msg += 'कृपया ग्राम पंचायत  का चयन करें |\n';
        }

        if ($scope.MemberRegistration.OrganizationId == undefined || $scope.MemberRegistration.OrganizationId == '0') {
            msg += 'कृपया सोसायटी का चयन करें |\n';
        }

        if ($scope.MemberRegistration.VillageId == undefined || $scope.MemberRegistration.VillageId == '0') {
            msg += 'कृपया ग्राम का चयन करें |\n';
        }

        if ($scope.MemberRegistration.PatwarMandalName == undefined || $scope.MemberRegistration.PatwarMandalName.length < 1) {
            msg += 'कृपया पटवार मंडल का नाम लिखें|\n';
        }

        if ($scope.MemberRegistration.Name == undefined || $scope.MemberRegistration.Name.length < 1) {
            msg += 'कृपया कृषक सदस्य का नाम लिखें|\n';
        }

        if ($scope.MemberRegistration.FatherHusbandName == undefined || $scope.MemberRegistration.FatherHusbandName.length < 1) {
            msg += 'कृपया पिता/पति का नाम लिखें|\n';
        }

        if ($scope.MemberRegistration.NomineeName == undefined || $scope.MemberRegistration.NomineeName.length < 1) {
            msg += 'कृपया बीमा क्लेम हेतु नॉमिनी का नाम लिखें|\n';
        }

        if ($scope.MemberRegistration.NomineeRelationId == undefined || $scope.MemberRegistration.NomineeRelationId == '0') {
            msg += 'कृपया कृषक से सम्बन्ध चयन करें|\n';
        }

        if ($scope.MemberRegistration.EconomicSectionId == undefined || $scope.MemberRegistration.EconomicSectionId == '0') {
            msg += 'कृपया आर्थिक समूह चयन करें|\n';
        }

        if ($scope.MemberRegistration.CategoryId == undefined || $scope.MemberRegistration.CategoryId == '0') {
            msg += 'कृपया वर्ग चयन करें|\n';
        }

        if ($scope.MemberRegistration.MobileNo == undefined || $scope.MemberRegistration.MobileNo.length != 10) {
            msg += 'कृपया 10 अंकों का मोबाइल नंबर दर्ज करें|\n';
        }

        if ($scope.MemberRegistration.AADHARNo == undefined || $scope.MemberRegistration.AADHARNo.length < 10) {
            msg += 'कृपया आधार संख्या दर्ज करें|\n';
        }

        if ($scope.MemberRegistration.SRDHNo == undefined || $scope.MemberRegistration.SRDHNo.length < 1) {
            msg += 'कृपया एस आर डी एच नंबर दर्ज करें|\n';
        }

        if ($scope.MemberRegistration.SahkariSavingAcNo == undefined || $scope.MemberRegistration.SahkariSavingAcNo.length < 1) {
            msg += 'कृपया सहकारी बैंक बचत खाता नंबर दर्ज करें|\n';
        }

        if ($scope.MemberRegistration.SahkariSavingAcIFSC == undefined || $scope.MemberRegistration.SahkariSavingAcIFSC.length < 1) {
            msg += 'कृपया आई एफ एस सी नंबर दर्ज करें|\n';
        }

        if ($scope.MemberRegistration.IsLandOwnership == undefined) {
            msg += 'क्या आपके पास अपनी भूमि का स्वामित्व है ?\n';
        }

        if ($scope.MemberRegistration.IsOtherBankLonee == undefined) {
            msg += 'क्या आपके द्वारा अन्य वाणिज्यिक बैंकों में किसी प्रकार का ऋण लिया गया है?\n';
        }

        var IsLandDetailsValid = true;
        var IsCropDetailsValid = true;
        var IsOtherBankLoanDetailsValid = true;


        $('#tblLandDetail input').each(function () {
            if ($(this).val().length == 0) {
                $(this).css('border', 'solid 1px red');
                IsLandDetailsValid = false;
            }
            else {
                $(this).css('border', 'solid 1px');
            }
        });

        $('#tblCropDetail input').each(function () {
            if ($(this).val().length == 0) {
                $(this).css('border', 'solid 1px red');
                IsCropDetailsValid = false;
            }
            else {
                $(this).css('border', 'solid 1px');
            }
        });




        if ($scope.MemberRegistration.IsOtherBankLonee != undefined && $scope.MemberRegistration.IsOtherBankLonee == '0') {

            $('#tblOtherBankLoanDetail input').each(function () {
                if ($(this).val().length == 0) {
                    $(this).css('border', 'solid 1px red');
                    IsOtherBankLoanDetailsValid = false;
                }
                else {
                    $(this).css('border', 'solid 1px');
                }
            });
        }




        if (!IsLandDetailsValid) {
            msg += 'भूमि का विवरण प्रविष्टियां उपलब्ध करावें\n';
        }

        if (!IsCropDetailsValid) {
            msg += 'बोई जाने वाली फसल का विवरण प्रविष्टियां उपलब्ध करावें\n';
        }

        if (!IsOtherBankLoanDetailsValid) {
            msg += 'समिति एवं अन्य बैंको से लिए गए फसली ऋण / केसीसी ऋण का विवरण प्रविष्टियां उपलब्ध करावें\n';
        }

        if (msg.length > 0) {
            alert(msg);
            return false;
        }

        return true;
    }
    //END : Validate GSS data()


    //START : VALIDATE Dairy Form Data
    $scope.ValidateDairyFormData = function () {
        var msg = '';

        if ($scope.MemberRegistration.RegistrationType == undefined) {
            msg += 'कृपया प्रकार का चयन करें |\n';
        }

        if ($scope.MemberRegistration.DISTRICT_ID == undefined || $scope.MemberRegistration.DISTRICT_ID == '0') {
            msg += 'कृपया ज़िला का चयन करें |\n';
        }

        if ($scope.MemberRegistration.BLOCK_ID == undefined || $scope.MemberRegistration.BLOCK_ID == '0') {
            msg += 'कृपया ब्लाक  का चयन करें |\n';
        }

        if ($scope.MemberRegistration.GP_ID == undefined || $scope.MemberRegistration.GP_ID == '0') {
            msg += 'कृपया ग्राम पंचायत  का चयन करें |\n';
        }

        if ($scope.MemberRegistration.OrganizationId == undefined || $scope.MemberRegistration.OrganizationId == '0') {
            msg += 'कृपया सोसायटी का चयन करें |\n';
        }



        if ($scope.MemberRegistration.Name == undefined || $scope.MemberRegistration.Name.length < 1) {
            msg += 'कृपया नाम लिखें|\n';
        }

        if ($scope.MemberRegistration.Age == undefined || $scope.MemberRegistration.Age.length < 1 || $scope.MemberRegistration.Age == 0) {
            msg += 'कृपया उम्र लिखें|\n';
        }

        if ($scope.MemberRegistration.Address == undefined || $scope.MemberRegistration.Address.length < 1) {
            msg += 'कृपया पता लिखें|\n';
        }

        if ($scope.MemberRegistration.Gender == undefined) {
            msg += 'कृपया लिंग चयन करें|\n';
        }

        if ($scope.MemberRegistration.AADHARNo == undefined || $scope.MemberRegistration.AADHARNo.length < 1) {
            msg += 'कृपया आधार संख्या दर्ज करें|\n';
        }

        if ($scope.MemberRegistration.Business == undefined || $scope.MemberRegistration.Business.length < 1) {
            msg += 'कृपया व्‍यवसाय दर्ज करें|\n';
        }

        if ($scope.MemberRegistration.MobileNo == undefined || $scope.MemberRegistration.MobileNo.length != 10) {
            msg += 'कृपया 10 अंकों का मोबाइल नंबर दर्ज करें|\n';
        }



        if ($scope.MemberRegistration.EmailId == undefined || $scope.MemberRegistration.EmailId.length < 1) {
            msg += 'कृपया ईमेल आईडी दर्ज करें|\n';
        }

        if ($scope.MemberRegistration.AnimalCount == undefined || $scope.MemberRegistration.AnimalCount.length < 1) {
            msg += 'कृपया धारित दुधारू पशुओ की संख्या दर्ज करें|\n';
        }

        if ($scope.MemberRegistration.BuyCount == undefined || $scope.MemberRegistration.BuyCount.length < 1) {
            msg += 'कितने क्रय करना चाहते हैं ? कृपया दर्ज करें|\n';
        }

        if ($scope.MemberRegistration.IsDisability == undefined) {
            msg += 'क्या आप सदस्यता हेतु किसी नियोग्यता से ग्रसित हैं ? कृपया दर्ज करें|\n';
        }

        if (msg.length > 0) {
            alert(msg);
            return false;
        }

        return true;
    }
    //END : VALIDATE Dairy Form Data


    //START : Save GSS data()
    $scope.SaveGSSData = function () {

        if ($scope.MemberRegistration.RegistrationType == '1') {
            if (!$scope.ValidateGSSData()) {
                return;
            }
            $scope.MemberDatail = false;
            $scope.divViewShow = true;
        }
        else {
            if (!$scope.ValidateDairyFormData()) {
                return;
            }
            $scope.divViewDairyFormShow = true;
            $scope.DairyDatail = false;
        }


        $scope.MemberRegistration["GSSLandDetails"] = [];
        $scope.MemberRegistration["SownCropDetails"] = [];
        $scope.MemberRegistration["OtherBankLoanDetails"] = [];

        if ($scope.LandDetailsGSS.length > 0) {
            angular.forEach($scope.LandDetailsGSS, function (i, j) {
                $scope.MemberRegistration["GSSLandDetails"].push(i);
            });
        }

        if ($scope.SownCropDetailGSS.length > 0) {
            angular.forEach($scope.SownCropDetailGSS, function (i, j) {
                $scope.MemberRegistration["SownCropDetails"].push(i);
            });
        }

        if ($scope.MemberRegistration.IsLandOwnership && $scope.CommercialLoanDetail.length > 0) {
            angular.forEach($scope.CommercialLoanDetail, function (i, j) {
                $scope.MemberRegistration["OtherBankLoanDetails"].push(i);
            });
        }

        $scope.MemberRegistration["OrganizationtypeId"] = $('input[type=radio][name=rdoRegistrationType]:checked').val();


        $scope.MemberRegistration.SocietyName = $('#ddlSociety option:selected').text().trim();
        $scope.MemberRegistration.DISTRICT_ENG = $('#ddlDistrict option:selected').text().trim();
        $scope.MemberRegistration.DISTRICT_MANGAL = $('#ddlDistrict option:selected').text().trim();
        $scope.MemberRegistration.BLOCK_ENG = $('#ddlblock option:selected').text().trim();
        $scope.MemberRegistration.BLOCK_MANGAL = $('#ddlblock option:selected').text().trim();
        $scope.MemberRegistration.GP_ENG = $('#ddlGP option:selected').text().trim();
        $scope.MemberRegistration.GP_MANGAL = $('#ddlGP option:selected').text().trim();
        $scope.MemberRegistration.Village = $('#ddlVillage option:selected').text().trim();
        $scope.MemberRegistration.FarmerRelation = $('#ddlNomineeRelation option:selected').text().trim();
        $scope.MemberRegistration.EconomicGroup = $('#ddlEconomicSection option:selected').text().trim();
        $scope.MemberRegistration.Category = $('#ddlCategory option:selected').text().trim();


        $scope.SaveUser();
        //if (!$scope.ValidateGSSData()) {
        //    return;
        //}


    }
    //END : Save GSS data()

    //START : Change Society Type at radio button
    $scope.ChangeType = function () {
        $scope.MemberRegistration.DISTRICT_ID = '0';
        $scope.Blocks = [];
        $scope.MemberRegistration.BLOCK_ID = '0';
        $scope.GramPanchayats = [];
        $scope.MemberRegistration.GP_ID = '0';
        $scope.SocietyList = [];
        $scope.MemberRegistration.OrganizationId = '0';
        $scope.disablePrimarySelection = false;
        $scope.divAADHARVerificationStep_1 = false;
        $scope.divAADHARVerificationStep_2 = false;

        $scope.MemberDatail = false;
        $scope.PLDBMemberDatail = false;

        $scope.DairyDatail = false;
        $scope.divViewShow = false;


        if ($scope.MemberRegistration.RegistrationType == '1' || $scope.MemberRegistration.RegistrationType == '2') {
            $scope.divGssDairyParentdataSelection = true;
            $scope.divPLDBParentDataSelection = false;
        }
        else {
            $scope.PLDBList();
            $scope.MemberRegistration.PLDB_ID = '0';
            $scope.PLDBBranches = [];
            $scope.MemberRegistration.Branch_ID = '0';
            $scope.divGssDairyParentdataSelection = false;
            $scope.divPLDBParentDataSelection = true;
        }

    }
    // END : Change Society Type at radio button

    // START : Get List of PLDB
    $scope.PLDBList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=-1").
            success(function (data, status) {
                $scope.PLDB = data;
            }).
            error(function (data, status) {
                $scope.PLDB = [];
            });
    };
    //END : Get List of PLDB

    // START : Get List of PLDB Branch
    $scope.PLDBBranchList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + $scope.MemberRegistration.PLDB_ID).
            success(function (data, status) {
                $scope.PLDBBranches = data;
                $scope.MemberRegistration.Branch_ID = '0';
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
            });
    };
    //END : Get List of PLDB Branch

    // START : Get Econiomic  List
    $scope.GetQualificationList = function () {
        $http.get("/Master/GetMasterData/?MasterType=EDUCATIONQUALIFICATION&MasterCode=-1").
            success(function (data, status) {
                $scope.EducationQualification = data;
            }).
            error(function (data, status) {
                $scope.EducationQualification = [];
            });
    }
    // END: Get Econiomic List

    //// START : On Page Load
    $scope.getTranslation();
    $scope.GetBloodRelationList();
    $scope.GetEconomicSectionList();
    $scope.GetCategoryList();

    $scope.LandDetailsGSS.push(new GSSLandDetailEntity());
    $scope.SownCropDetailGSS.push(new GSSCropDetailEntity());
    $scope.CommercialLoanDetail.push(new CommercialLoanDetailEntity());
    $('#txtMembershipDate').datepicker({
        showOn: "both",
        changeMonth: true,
        buttonImage: "../images/calender.png",
        changeYear: true,
        dateFormat: "dd/mm/yy",
        maxDate: '0D'
    });

    //// END : On Page Load

});
