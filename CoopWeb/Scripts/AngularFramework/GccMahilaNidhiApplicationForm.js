var ArrSelectTraningUser = new Array();
function TraningUserSelected() {
    this.UserId = '0';
}

var AppTrainning = angular.module("GopalCreditCardAppEntryForm", ['angularUtils.directives.dirPagination']);
AppTrainning.controller("GopalCreditCard", function ($scope, $http) {
    // Save Sms Template 
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivSms = true;
    $scope.newdiv = "";
    $scope.Trainning = '';
    $scope.GCCRegistrationStatus = false;
    $scope.DistrictList = [];
    // on page load

    $scope.divTabulation = true;
    $scope.ActionId = 1;

    const urlParams = new URLSearchParams(window.location.search);

    $scope.encId = urlParams.get('GCCEId');

    $scope.Encrypted_JAN_MEMBER_ID = urlParams.get('Encrypted_JAN_MEMBER_ID');
    $scope.Encrypted_JAN_AADHAR = urlParams.get('Encrypted_JAN_AADHAR');
    $scope.MobileNo = urlParams.get('MobileNo');
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json?id=3").
                success(function (data, status) {

                    $scope.translation = data;
                    //$scope._language = "en-us";
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json?id=3").
                success(function (data, status) {

                    $scope.translation = data;
                    //$scope._language = "";
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }



        //$scope.AlertPopUpHeading = $scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::';


        $scope.ImageValidationMessage = '';
        $scope.FileCollection = [];



        // Initialie Main form control
        $scope.GopalCreditCardModel = {}
        $scope.GopalCreditCardModel.gcc_id = 0;
        $scope.GopalCreditCardModel.StatusId = 0;
        $scope.GopalCreditCardModel.IsMemberOfDCS = null;
        $scope.GopalCreditCardModel.IsPACSLAMPSMember = null;
        $scope.GopalCreditCardModel.IsAddressSameAsJanAadhar = false;
        $scope.GopalCreditCardModel.Address_ENG = '';
        $scope.GopalCreditCardModel.PinCode = '';
        $scope.GopalCreditCardModel.District_Code = "0";
        $scope.GopalCreditCardModel.Block_Code = "0";
        $scope.GopalCreditCardModel.GP_Code = "0";
        $scope.GopalCreditCardModel.Village_Code = "0";
        $scope.GopalCreditCardModel.City_Code = "0";
        $scope.GopalCreditCardModel.Ward_Code = "0";

        $scope.GopalCreditCardModel.LoanDistrictId = '0';
        $scope.GopalCreditCardModel.LoanBankID = "0";
        $scope.GopalCreditCardModel.LoanBranchId = "0";
        $scope.GopalCreditCardModel.PACS_ID = "0";

        $scope.RecommendationOfDCSDOC_Path = '';
        $scope.GopalCreditCardModel.RecommendationOfDCSDOC_Path = '';
        $scope.GopalCreditCardModel.isValidRecommendationOfDCSDOC = false;

        $scope.DocumentOfProposedWork_Path = '';
        $scope.GopalCreditCardModel.DocumentOfProposedWork_Path = '';
        $scope.GopalCreditCardModel.isValidDocumentOfProposedWork = false;

        $scope.GopalCreditCardModel.CopyOfDCSPassbook_Path = '';
        $scope.CopyOfDCSPassbook_Path = '';
        $scope.GopalCreditCardModel.isValidCopyOfDCSPassbook = false;


        $scope.AadharDoc = '';
        $scope.GopalCreditCardModel.AadharDoc = '';
        $scope.GopalCreditCardModel.isValidAadharDoc = false;

        $scope.PanCardDoc = '';
        $scope.GopalCreditCardModel.PanCardDoc = '';
        $scope.GopalCreditCardModel.isValidPanCardDoc = false;


        $scope.G1Photo = '';
        $scope.GopalCreditCardModel.G1Photo = '';
        $scope.GopalCreditCardModel.isValidG1Photo = false;

        $scope.G2Photo = '';
        $scope.GopalCreditCardModel.G2Photo = '';
        $scope.GopalCreditCardModel.isValidG2Photo = false;

        $scope.G1Signature = '';
        $scope.GopalCreditCardModel.G1Signature = '';
        $scope.GopalCreditCardModel.isValidG1Signature = false;

        $scope.G2Signature = '';
        $scope.GopalCreditCardModel.G2Signature = '';
        $scope.GopalCreditCardModel.isValidG2Signature = false;


        $scope.AgriculturalPropertyDoc = '';
        $scope.GopalCreditCardModel.AgriculturalPropertyDoc = '';
        $scope.GopalCreditCardModel.isValidAgriculturalPropertyDoc = false;

        $scope.OtherPropertyDoc = '';
        $scope.GopalCreditCardModel.OtherPropertyDoc = '';
        $scope.GopalCreditCardModel.isValidOtherPropertyDoc = false;


        $scope.GopalCreditCardModel.IsRecommendationOfDCSUploaded = true;
        $scope.GopalCreditCardModel.IsMahilaNidhiMember = true;

        $scope.ShowJanAadharOTPValidation = false;
        $scope.ShowGopalCreditCardForm = false;
        $scope.ShowFamilyMemberGrid = false;



        //$scope.GopalCreditCardModel.Mn_ClfID = '0';
        //$scope.GopalCreditCardModel.Mn_VoID = '0';
        //$scope.GopalCreditCardModel.Mn_ShgId = '0';
        //$scope.GopalCreditCardModel.Mn_BlockID = '0';
        //$scope.GopalCreditCardModel.Mn_DistrictId = '0';

        $scope.GopalCreditCardModel.Mn_ClusterId = "0";
        $scope.GopalCreditCardModel.Mn_VoID = "0";
        $scope.GopalCreditCardModel.Mn_ShgId = "0";
        $scope.GopalCreditCardModel.Mn_BlockID = "0";
        $scope.GopalCreditCardModel.Mn_DistrictId = "0";



        $scope.GopalCreditCardModel.SHG_BankName = '';
        $scope.GopalCreditCardModel.SHG_BranchName = '';
        $scope.GopalCreditCardModel.SHG_IFSC_Code = '';
        $scope.GopalCreditCardModel.SHG_AccountNo = '';
        $scope.GopalCreditCardModel.SHG_Mem_BankName = '';
        $scope.GopalCreditCardModel.SHG_Mem_BranchName = '';
        $scope.GopalCreditCardModel.SHG_Mem_IFSC_Code = '';
        $scope.GopalCreditCardModel.SHG_Mem_AccountNo = '';

        $scope.GetDistrictWithoutDivisionList();
        $scope.GetDistrictList();
        //$scope.DashboardData();
        $scope.GetCategoryList();

        $scope.GetDistrictList();
        if ($scope.Encrypted_JAN_MEMBER_ID != null || $scope.Encrypted_JAN_AADHAR != null) {

            $scope.JanAadharNo = '';
            //alert(decryptData($scope.Encrypted_JAN_AADHAR));


            $scope.PassValidationJanOTP($scope.MobileNo, $scope.Encrypted_JAN_MEMBER_ID, $scope.Encrypted_JAN_AADHAR);

        }
        else if ($scope.encId != null) {

            $scope.GetFormEditData(0, $scope.encId);

        }


        // $scope.$apply();
        //$scope.PLDBList();
    };

    $scope.DashboardData = function (ActionId, GopalCreditCardId, GopalCreditCardAppNo) {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/GopalCreditCard/GetDashBoardData", { "ActionId": ActionId == undefined ? 0 : parseInt(ActionId), "GopalCreditCardId": GopalCreditCardId == undefined ? 0 : parseInt(GopalCreditCardId), "GopalCreditCardAppNo": GopalCreditCardAppNo == undefined ? 0 : parseInt(GopalCreditCardAppNo) }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.TableData = [];
                $scope.TableData = data;
                if ($scope.TableData.length > 0) {
                    setTimeout(function () {
                        $($($('#SocietyListDeshBoard tbody tr:first')[0]).find('td:eq(0)')[0]).find('input').prop('checked', true);
                    }, 3000);
                }

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
                $scope.TableData = [];
                $(".se-pre-con").fadeOut("slow");
            });

    }
    //$scope.ApplicationView = function (eId, eRno, EncGccId) {
    //    if (eId != '' && eRno != '') {
    //       // window.location = "../GopalCreditCard/GopalApplicationFormView?eId=" + eId + "&eRno=" + eRno;
    //        window.location = "../GopalCreditCard/GopalApplicationFormView?eId=" + eId ;
    //       // window.location = "../GopalCreditCard/GopalApplicationFormView_v1?eId=" + EncGccId;
    //    }
    //}
    $scope.AddNewGopalCreditCardApp = function (value) {
        if (value == 2) {

            $scope.ActionId = 2;
            //$scope.ShowSubmitBtn = false;
            $scope.divTabulation = false;
            $scope.ShowJanAadharOTPValidation = true;
            //$scope.divAddBtn = false;
            $scope.grid = false;
            $scope.JanAadharNo = "";
            $scope.ShowFamilyMemberGrid = false;
        }
        else if (value == 1) {
            window.location.reload();
            $scope.ActionId = 1;
            //$scope.ShowSubmitBtn = false;
            $scope.divTabulation = true;
            $scope.ShowJanAadharOTPValidation = false;
            //$scope.divAddBtn = false;
            $scope.grid = false;
        }

    }
    $scope.numberOnly = function (event) {

        const charCode = (event.which) ? event.which : event.keyCode;
        if (charCode === 46) {
            // Check if there is already a decimal point in the input
            if (event.target.value.includes('.')) {
                return false;
            }
            return true;
        }
        // Allow numbers (0-9)
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        //if (charCode > 31 && (charCode < 48 || charCode > 57 || charCode === 46)) {
        //  return false;
        //}
        return true;
    }
    $scope.IsAddressSameAsJanAadhar_Changed = function (event) {


        if ($scope.GopalCreditCardModel.IsAddressSameAsJanAadhar == true) {
            $scope.GopalCreditCardModel.IsRural = $scope.GopalCreditCardModel.Original_IsRural;
            $scope.GopalCreditCardModel.District_Code = $scope.GopalCreditCardModel.Original_District_Code;
            $scope.GopalCreditCardModel.Block_Code = $scope.GopalCreditCardModel.Original_Block_Code;
            $scope.GopalCreditCardModel.City_Code = $scope.GopalCreditCardModel.Original_City_Code;
            $scope.GopalCreditCardModel.GP_Code = $scope.GopalCreditCardModel.Original_GP_Code;
            $scope.GopalCreditCardModel.Village_Code = $scope.GopalCreditCardModel.Original_Village_Code;
            $scope.GopalCreditCardModel.Address_ENG = $scope.GopalCreditCardModel.Original_Address_ENG;
            $scope.GopalCreditCardModel.Address_HI = $scope.GopalCreditCardModel.Original_Address_HI;
            $scope.GopalCreditCardModel.PinCode = $scope.GopalCreditCardModel.Original_PinCode;

            $scope.RuralUrbanBlock();
            $scope.GramPanchayatList();
            $scope.VillageList();
            //$scope.GetCityList();
            //$scope.GetWardList();
        }
        else {
            $scope.GopalCreditCardModel.District_Code = "0";
            //$scope.GopalCreditCardModel.CityTypes = "0";
            $scope.GopalCreditCardModel.City_Code = "0";
            $scope.GopalCreditCardModel.Ward_Code = "0";
            $scope.GopalCreditCardModel.Block_Code = "0";
            $scope.GopalCreditCardModel.GP_Code = "0";
            $scope.GopalCreditCardModel.Village_Code = "0";
            $scope.GopalCreditCardModel.Address_ENG = "";
            $scope.GopalCreditCardModel.Address_HI = "";
            $scope.GopalCreditCardModel.PinCode = "";
            $scope.GopalCreditCardModel.IsRural = null;
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
        }

        return true;
    }
    $scope.RuralUrbanBlock = function (V) {

        var SelectedVal = $scope.GopalCreditCardModel.IsRural == 0 ? false : true;
        if ($scope.Validate_UrbanArea(SelectedVal)) {
            if (V != 1 && $scope.GopalCreditCardModel.IsAddressSameAsJanAadhar != true) {
                $scope.GopalCreditCardModel.CityTypes = "0";
                $scope.GopalCreditCardModel.City_Code = "0";
                $scope.GopalCreditCardModel.Ward_Code = "0";
                $scope.GopalCreditCardModel.Block_Code = "0";
                $scope.GopalCreditCardModel.GP_Code = "0";
                $scope.GopalCreditCardModel.Village_Code = "0";

                $scope.CityListdata = [];
                $scope.Blocksdata = [];
                $scope.GramPanchayats = [];
                $scope.Villages = [];
                $scope.Ward = [];

            }
            if (SelectedVal == true || SelectedVal == true) {
                $scope.UrbanDiv = false;
                $scope.RuralDiv = true;
                $scope.BlockListByOffAdd(1);
            }
            else if (SelectedVal == false || SelectedVal == false) {
                $scope.UrbanDiv = true;
                $scope.RuralDiv = false;

                //$scope.BlockListByOffAdd(1)
            }
            else {
                $scope.UrbanDiv = false;
                $scope.RuralDiv = false;
            }
        }
    };
    $scope.Validate_UrbanArea = function (SelectedVal) {

        //var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';
        var message = "";
        if (SelectedVal == false) {
            message += $scope._language == 'en-us' ? '\n <br /> Residents of urban areas are not eligible to apply for Gopal Credit Card.' : '\n <br /> शहरी क्षेत्रों के निवासी गोपाल क्रेडिट कार्ड के लिए आवेदन करने के पात्र नहीं हैं|';
            //$scope.GopalCreditCardModel.IsRural = "";
            $scope.GopalCreditCardModel.District_Code = "0";
            $scope.ResetDataForAddress(1, 0);
        }

        if (message.length > 0) {
            ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', message);
            return false;
        }
        return true;
    }
    $scope.GetDistrictWithoutDivisionList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    $scope.ResetDataForAddress = function (val, Level) {
        if (val == 1) {
            if (Level < 3) {
                $scope.GopalCreditCardModel.IsRural = undefined;
                $scope.CityListdata = [];
                $scope.Blocksdata = [];
                $scope.GramPanchayats = [];
                $scope.Villages = [];
                $scope.Ward = [];
                $scope.GetCityList();
                $scope.BlockListByOffAdd(val);
                $scope.UrbanDiv = false;
                $scope.RuralDiv = false;

                $scope.GopalCreditCardModel.Block_Code = "0";
                $scope.GopalCreditCardModel.GP_Code = "0";
                $scope.GopalCreditCardModel.Village_Code = "0";
                $scope.GopalCreditCardModel.City_Code = "0";
                $scope.GopalCreditCardModel.Ward_Code = "0";
            }
        }
        else if (val == 2) {
            if (Level < 3) {
                $scope.GopalCreditCardModel.IsRuralcopy = undefined;
                $scope.CityListdatacopy = [];
                $scope.Blocksdatacopy = [];
                $scope.GramPanchayatscopy = [];
                $scope.Villagescopy = [];
                $scope.Wardcopy = [];
            }
        }
    }
    $scope.BlockListByOffAdd = function (id) {
        $(".se-pre-con").fadeIn("slow");
        $scope.Blocksdata = [];
        $scope.GramPanchayats = [];
        $scope.Villages = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY_DISTRICTCODE&MasterCode=" + $scope.GopalCreditCardModel.District_Code).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Blocksdata = data;
            }).
            error(function (data, status) {
                $scope.Blocksdata = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.GramPanchayatList = function (id) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT_BLOCKCODE&MasterCode=" + $scope.GopalCreditCardModel.Block_Code).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.GramPanchayats = data;
            }).
            error(function (data, status) {
                $scope.GramPanchayats = [];
                $(".se-pre-con").fadeOut("slow");
            });

    };
    $scope.VillageList = function (id) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=VILLAGE_GPCODE&MasterCode=" + $scope.GopalCreditCardModel.GP_Code).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Villages = data;
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Villages = [];
            });

    };
    $scope.GetCityList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITY_DISTRICTCODE&MasterCode=" + $scope.GopalCreditCardModel.District_Code).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.CityList = data;
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.CityList = [];
            });
    };
    $scope.GetWardList = function (id) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD_CITYCODE&MasterCode=" + $scope.GopalCreditCardModel.City_Code).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Ward = data;
            }).
            error(function (data, status) {
                $scope.Ward = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.GetJanAadharDetail = function () {
        if ($scope.GopalCreditCardJanaadharValidate()) {
            /*   if ($scope.Validate_CheckGCCRegistrationStatus()) {*/
            $(".se-pre-con").fadeIn("slow");
            $scope.JanMemberList = '';
            $http.post(SiteUrl + "/GopalCreditCard/GetJanAadhaarMembersList?JanAadharNo=" + encodeURIComponent(encryptData($scope.JanAadharNo))).
                success(function (data, status) {

                    if (data.length > 0) {
                        $scope.JanMemberList = data;
                        $scope.ShowFamilyMemberGrid = true;
                        $(".se-pre-con").fadeOut("slow");
                    }
                    //else if (data==-1)
                    //{

                    //    alert("Already exist JanAadhaar NO");
                    //    $scope.ShowFamilyMemberGrid = false;
                    //    $(".se-pre-con").fadeOut("slow");

                    //}
                    else {
                        alert("No data found");
                        $scope.ShowFamilyMemberGrid = false;
                        $(".se-pre-con").fadeOut("slow");
                    }
                }).
                error(function (data, status) {
                    $scope.ShowFamilyMemberGrid = false;
                    alert("Error while fetching Year Target Data");
                    $(".se-pre-con").fadeOut("slow");
                });
            //   }
        }
    }
    $scope.Validate_CheckGCCRegistrationStatus = function () {
        var message = "";

        $scope.CheckGCCRegistrationStatus();

        if ($scope.GCCRegistrationStatus == true) {
            message += $scope._language == 'en-us' ? '\n <br /> Application for Gopal Credit Card Scheme has Already been Made with Janadhaar Number ' + $scope.JanAadharNo + '. Hence You are not Eligible to Apply for Gopal Credit Card Scheme.' : '\n <br /> जनाधार नंबर ' + $scope.JanAadharNo + ' से पहले ही गोपाल क्रेडिट कार्ड योजना के लिए आवेदन किया जा चुका है। अतः आप गोपाल क्रेडिट कार्ड योजना मैं आवेदन के लिए योग्य नहीं हैं।';
            $scope.GCCRegistrationStatus = false;
            $scope.ShowFamilyMemberGrid = false;
            $scope.JanAadharNo = "";
        }
        if (message.length > 0) {
            ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', message);
            return false;
        }
        return true;
    }
    $scope.CheckGCCRegistrationStatus = function () {
        $(".se-pre-con").fadeIn("slow");
        $.ajax({
            type: "POST",
            url: SiteUrl + "/GopalCreditCard/CheckGCCRegistrationStatus?JanAadharNo=" + encodeURIComponent(encryptData($scope.JanAadharNo)),
            data: "",
            dataType: 'json',
            contentType: false,
            processData: false,
            async: false,
            success: function (data, status) {
                if (data.Status == false) {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.GCCRegistrationStatus = true;
                }
            },
            error: function (data, status) {
                alert("errror");
                $(".se-pre-con").fadeOut("slow");
            }
        });

        //
        //$(".se-pre-con").fadeIn("slow");
        //$http.post(SiteUrl + "/GopalCreditCard/CheckGCCRegistrationStatus?JanAadharNo=" + $scope.JanAadharNo, { async: false }).
        //    success(function (data, status) {
        //        $(".se-pre-con").fadeOut("slow");

        //        
        //        if (data.Status == false) {

        //            $scope.ShowFamilyMemberGrid = false;
        //            $scope.JanAadharNo = "";
        //            $scope.GCCRegistrationStatus = true;

        //        }
        //    }).
        //    error(function (data, status) {
        //        $scope.ShowFamilyMemberGrid = false;
        //        alert("Error while fetching Year Target Data");
        //        $(".se-pre-con").fadeOut("slow");
        //    });
    }
    $scope.GopalCreditCardJanaadharValidate = function () {
        var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';
        if ($scope.JanAadharNo == undefined || $scope.JanAadharNo == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter JanAadhar Number' : '\n <br /> कृपया जन आधार नंबर दर्ज करें |';
        }

        if (message.length > 23) {
            ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', message);
            return false;
        }
        return true;
    }
    $scope.VerifyOTPViewPopup = function (mobileNo, Encrypted_JAN_MEMBER_ID, Encrypted_JAN_AADHAR) {

        $scope.divTabulation = false;
        $scope.divSearch = false;

        $(".se-pre-con").fadeIn("slow");
        $(".se-pre-con").fadeOut("slow");
        $(".blockUI").fadeOut("slow");
        $scope.SendAndVerifyOTP = {};
        $scope.SendAndVerifyOTP.Masked_MOBILE_NO = mobileNo;
        $scope.SendAndVerifyOTP.Encrypted_JAN_MEMBER_ID = Encrypted_JAN_MEMBER_ID;
        $scope.SendAndVerifyOTP.Encrypted_JAN_AADHAR = Encrypted_JAN_AADHAR;
        //$scope.SendAndVerifyOTP.Encrypted_JAN_AADHAR = encodeURIComponent(encryptData(Encrypted_JAN_AADHAR));

        $scope.SendAndVerifyOTP.tid = "";
        $("#modelPopupVerifyOTP").modal("show");
    };


    $scope.PassValidationJanOTP = function (mobileNo, Encrypted_JAN_MEMBER_ID, Encrypted_JAN_AADHAR) {

        $scope.divTabulation = false;
        $scope.divSearch = false;

        $(".se-pre-con").fadeIn("slow");
        $(".se-pre-con").fadeOut("slow");
        $(".blockUI").fadeOut("slow");
        $scope.SendAndVerifyOTP = {};
        $scope.SendAndVerifyOTP.Encrypted_JAN_MEMBER_ID = Encrypted_JAN_MEMBER_ID;
        $scope.SendAndVerifyOTP.Encrypted_JAN_AADHAR = Encrypted_JAN_AADHAR;
        $scope.SendAndVerifyOTP.tid = "";


        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/GopalCreditCard/PassValidateJanAadharOTP", $scope.SendAndVerifyOTP).
            success(function (data, status) {
                if (data.Status == true) {
                    $scope.ShowJanAadharOTPValidation = false;
                    $scope.ShowGopalCreditCardForm = true;
                    $scope.GopalCreditCardModel = data;
                    $scope.GopalCreditCardModel.PurposeOfLoan = "0";
                    $scope.GopalCreditCardModel.Mn_ClusterId = "0";
                    $scope.GopalCreditCardModel.Mn_VoID = "0";
                    $scope.GopalCreditCardModel.Mn_ShgId = "0";
                    $scope.GopalCreditCardModel.Mn_BlockID = "0";
                    $scope.GopalCreditCardModel.Mn_DistrictId = "0";
                    $scope.GopalCreditCardModel.Category = "0";
                    $scope.GopalCreditCardModel.IsMahilaNidhiMember = true;
                    var resultCat = $scope.CategoryList.filter(x => x.CODENAME == data.Category);
                    $scope.GopalCreditCardModel.Category = resultCat.length ? resultCat[0].CODEID : '0';
                    $scope.IsAddressSameAsJanAadhar_Changed();
                    $scope.ActionId = 0;
                    // $scope.GetCCBDetails();//madhuri
                }
                else {
                    ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', $scope._language == 'en-us' ? 'OTP has not been validated' : 'OTP मान्य नहीं किया गया है');
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.SendAndVerifyOTP.tid = "";
                $(".se-pre-con").fadeOut("slow");
            });

        // $("#modelPopupVerifyOTP").modal("show");
    };



    $scope.Cancelpopup = function () {
        $("#modelPopupVerifyOTP").modal("hide");
    }


    var timerId;
    $("#btnReSendOTP").hide();
    $scope.SetTimer = function () {
        clearTimeout(timerId);
        var timeLeft = 60;
        var elem = document.getElementById('countdown');
        timerId = setInterval(countdown, 1000);
        $("#countdown").css("visibility", "visible");
        $("#btnReSendOTP").hide();
        function countdown() {
            if (timeLeft == 0) {
                clearTimeout(timerId);
                $("#btnReSendOTP").show();
                $("#btnSendOTP").hide();
                $("#countdown").css("visibility", "hidden");
            } else {
                elem.innerHTML = timeLeft + ' seconds remaining';
                timeLeft--;
            }
        }
    };

    $scope.GenrateJanAadharOTP = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/GopalCreditCard/GenrateJanAadharOTP", $scope.SendAndVerifyOTP).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.SendAndVerifyOTP.tid = data;
                    $scope.SetTimer();
                }
                else {
                    $scope.SendAndVerifyOTP.tid = "";
                    ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', $scope._language == 'en-us' ? 'An Error Occurred Please Try Again Later' : 'कुछ त्रुटि घटित हुई है, कृपया बाद में पुन: प्रयास करें');
                    $("#modelPopupVerifyOTP").modal("hide");
                }

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.SendAndVerifyOTP.tid = "";
                $(".se-pre-con").fadeOut("slow");
            });
    }
    $scope.ValidateJanAadharOTP = function () {

        //  if ($scope.JanaadharValidate_FieldValidator()) {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/GopalCreditCard/ValidateJanAadharOTP", $scope.SendAndVerifyOTP).
            success(function (data, status) {
                if (data.Status == true) {
                    $scope.ShowJanAadharOTPValidation = false;
                    $scope.ShowGopalCreditCardForm = true;
                    $scope.GopalCreditCardModel = data;
                    //$scope.GopalCreditCardModel.LoanDistrictId = '0';
                    //$scope.GopalCreditCardModel.LoanBankID = "0";
                    //$scope.GopalCreditCardModel.LoanBranchId = "0";
                    //$scope.GopalCreditCardModel.PACS_ID = "0";
                    //$scope.GopalCreditCardModel.CurrentLoanDistrictId = "0";
                    //$scope.GopalCreditCardModel.CurrentLoanBankID = "0";
                    //$scope.GopalCreditCardModel.CurrentLoanBranchId = "0";
                    $scope.GopalCreditCardModel.PurposeOfLoan = "0";

                    $scope.GopalCreditCardModel.Mn_ClusterId = "0";
                    $scope.GopalCreditCardModel.Mn_VoID = "0";
                    $scope.GopalCreditCardModel.Mn_ShgId = "0";
                    $scope.GopalCreditCardModel.Mn_BlockID = "0";
                    $scope.GopalCreditCardModel.Mn_DistrictId = "0";


                    $scope.GopalCreditCardModel.IsMahilaNidhiMember = true;
                    var resultCat = $scope.CategoryList.filter(x => x.CODENAME == data.Category)
                    $scope.GopalCreditCardModel.Category = "0";
                    $scope.GopalCreditCardModel.Category = resultCat.length ? resultCat[0].CODEID : '0';
                    $scope.IsAddressSameAsJanAadhar_Changed();
                    $("#modelPopupVerifyOTP").modal("hide");
                    $scope.ActionId = 0;
                    $scope.GetCCBDetails();//madhuri
                }
                else {
                    ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', $scope._language == 'en-us' ? 'OTP has not been validated' : 'OTP मान्य नहीं किया गया है');
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.SendAndVerifyOTP.tid = "";
                $(".se-pre-con").fadeOut("slow");
            });
        //  }
    }
    $scope.JanaadharValidate_FieldValidator = function () {
        var AlertPopUpHeading = $scope._language == 'en-us' ? 'JanAadhar OTP Validate :: ' : 'जन आधार ओटीपी सत्यापित करें ::';
        var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';
        if ($scope.SendAndVerifyOTP.JanAadharOTP == undefined || $scope.SendAndVerifyOTP.JanAadharOTP == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter JanAadhar OTP' : '\n <br /> कृपया जनाधार ओटीपी दर्ज करें |';
        }

        if (message.length > 23) {
            ShowModelAlert(AlertPopUpHeading, message);
            return false;
        }
        return true;

    }

    $scope.IsPACSLAMPSMember_Changed = function (value) {
        // 
        if (value == 1) {
            //   $scope.GopalCreditCardModel.IsPACSLAMPSMemberCheckedStatus = true;
        }
        if (value == 0) {
            //   $scope.GopalCreditCardModel.IsPACSLAMPSMemberCheckedStatus = false;
            $scope.GopalCreditCardModel.PACSLAMPSMembershipNo = "";

            var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';
            if ($scope.JanAadharNo == undefined || $scope.JanAadharNo == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Apply at the time of loan sanction' : '\n <br /> कृपया ऋण स्वीकृति के समय आवेदन करें |';
            }

            if (message.length > 23) {
                ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', message);
                return false;
            }


        }
    };
    $scope.IsMemberOfDCS_Changed = function (value) {
        var SelectedVal = $scope.GopalCreditCardModel.IsMemberOfDCS == 'false' ? false : true;
        if (value == 0) {

            $scope.GopalCreditCardModel.DCSRegistrationNo = "";
            $scope.GopalCreditCardModel.DCSName = "";
            $scope.GopalCreditCardModel.DCSOwnerName = "";
            $scope.GopalCreditCardModel.DCSOwnerMobileNo = "";
            $scope.GopalCreditCardModel.DCSMembershipNo = "";
            $scope.GopalCreditCardModel.DCSAddress = "";

            var message = $scope._language == 'en-us' ? '\n <br /> In case the cow keeper does not sell the milk to the dairy cooperative society, he is not eligible to apply for Gopal Credit Card..' : '\n <br />गौ पालक द्वारा दूध का बेचान डेयरी सहकारी समिति को नही किये जाने की स्थिति में, गोपाल क्रेडिट कार्ड में आवेदन करने के लिए पात्र नहीं हैं।';

            if (message.length > 0) {
                ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', message);

            }
        }

    };
    $scope.DoesTheMilkSaleToDCS_Changed = function (value) {
        //if (value == 0)
        //    $scope.Validate_DoesTheMilkSaleToDCS($scope.GopalCreditCardModel.DoesTheMilkSaleToDCS);
        //else
        $scope.GopalCreditCardModel.EstimatedAmountOfMilkSoldToDCS = "";

    };
    $scope.Validate_DoesTheMilkSaleToDCS = function (SelectedVal) {
        var message = "";
        if (SelectedVal == false) {
            message += $scope._language == 'en-us' ? '\n <br /> In case the cattle keeper does not sell the milk to the dairy cooperative society, they are not eligible to apply for Gopal Credit Card.' : '\n <br /> पशुपालक द्वारा दूध का बेचान डेयरी सहकारी समिति को नही किये जाने की स्थिति में, गोपाल क्रेडिट कार्ड में आवेदन करने के लिए पात्र नहीं हैं। '

            //message += $scope._language == 'en-us' ? '\n <br /> In case the cow keeper does not sell the milk to the dairy cooperative society, they are not eligible to apply for Gopal Credit Card.' : '\n <br /> गौपालक द्वारा दूध का बेचान डेयरी सहकारी समिति को नही किये जाने की स्थिति में, गोपाल क्रेडिट कार्ड में आवेदन करने के लिए पात्र नहीं हैं। ';
            $scope.GopalCreditCardModel.EstimatedAmountOfMilkSoldToDCS = "";
            $scope.GopalCreditCardModel.DoesTheMilkSaleToDCS = "";
        }
        if (message.length > 0) {
            ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', message);
            return false;
        }
        return true;
    }

    $scope.IsPACSBankAccountAvailable_Changed = function (value) {
        if (value == 0) {

            $scope.Validate_IsPACSBankAccountAvailable($scope.GopalCreditCardModel.IsPACSBankAccountAvailable);
        }
        else {

            $scope.GopalCreditCardModel.BankAccountNo = "";
            $scope.GopalCreditCardModel.BankIFSCCode = "";
            $scope.GopalCreditCardModel.BankName = "";
            $scope.GopalCreditCardModel.BankBranchName = "";
        }
    };
    $scope.Validate_IsPACSBankAccountAvailable = function (SelectedVal) {
        //var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';
        var message = "";
        if (SelectedVal == false) {
            message += $scope._language == 'en-us' ? '\n <br /> Please open your account in the branch of Central Cooperative Bank after loan is approved.' : '\n <br /> कृपया ऋण स्वीकृति होने पर केंद्रीय सहकारी बैंक की शाखा में अपना खाता खुलवाये।';

            $scope.GopalCreditCardModel.IsPACSBankAccountAvailable = "";
            $scope.GopalCreditCardModel.IsPACSBankAccountAvailable_Status = false;
            $scope.GopalCreditCardModel.BankAccountNo = "";
            $scope.GopalCreditCardModel.BankIFSCCode = "";
            $scope.GopalCreditCardModel.BankName = "";
            $scope.GopalCreditCardModel.BankBranchName = "";
        }

        if (message.length > 0) {
            ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', message);
            return false;
        }
        return true;
    }

    $scope.GetDCSDetailByRegistrationNo = function () {

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/GopalCreditCard/Get_DCSDetailByRegistrationNo?dscRno=" + $scope.GopalCreditCardModel.DCSRegistrationNo).
            success(function (data, status) {

                if (data != null) {
                    $scope.GopalCreditCardModel.DCSName = data.DCSName;
                    $scope.GopalCreditCardModel.DCSOwnerName = data.DCSOwnerName;
                    $scope.GopalCreditCardModel.DCSOwnerMobileNo = data.DCSOwnerMobileNo;
                    $scope.GopalCreditCardModel.DCSAddress = data.DCSAddress;
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.GopalCreditCardModel.DCSName = "";
                $scope.GopalCreditCardModel.DCSOwnerName = "";
                $scope.GopalCreditCardModel.DCSOwnerMobileNo = "";
                $scope.GopalCreditCardModel.DCSAddress = "";
                $(".se-pre-con").fadeOut("slow");
            });

    }
    $scope.IsCopyOfDCSPassbookAttached_Changed = function (value) {      
        if (value == 0) {
            $scope.Validate_IsPACSBankAccountAvailable($scope.GopalCreditCardModel.IsCopyOfDCSPassbookAttached);
            $scope.GopalCreditCardModel.CopyOfDCSPassbook_Path = "";
        }
    };
    $scope.IsRecommendationOfDCSUploaded_Changed = function (value) {
        $scope.GopalCreditCardModel.IsRecommendationOfDCSUploaded = true;

        var SelectedVal = $scope.GopalCreditCardModel.IsRecommendationOfDCSUploaded == 'false' ? false : true;
        if ($scope.Validate_IsRecommendationOfDCSUploaded(SelectedVal)) {
            if (SelectedVal == true || SelectedVal == true) {
                $scope.GopalCreditCardModel.IsRecommendationOfDCSUploaded_Status = true;
            }
            else if (SelectedVal == false || SelectedVal == false) {
                $scope.GopalCreditCardModel.IsRecommendationOfDCSUploaded_Status = false;
                $scope.GopalCreditCardModel.RecommendationOfDCSDOC_Path = "";
            }
            else {
                $scope.GopalCreditCardModel.IsRecommendationOfDCSUploaded = "";
                $scope.GopalCreditCardModel.IsRecommendationOfDCSUploaded_Status = false;
                $scope.GopalCreditCardModel.RecommendationOfDCSDOC_Path = "";
            }
        }
    };

    $scope.Validate_IsRecommendationOfDCSUploaded = function (SelectedVal) {
        //var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';
        //var message = "";
        //if (SelectedVal == false) {
        //    message += $scope._language == 'en-us' ? '\n <br /> It is Mandatory to Upload the Recommendation of the Dairy Co-Operative Society.' : '\n <br /> डेयरी सहकारी समिति की अनुशंषा अपलोड करना अनिवार्य है। ';
        //    $scope.GopalCreditCardModel.IsRecommendationOfDCSUploaded = "";
        //    $scope.GopalCreditCardModel.IsRecommendationOfDCSUploaded_Status = false;
        //    $scope.GopalCreditCardModel.RecommendationOfDCSDOC_Path = "";
        //}

        //if (message.length > 0) {
        //    ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', message);
        //    return false;
        //}
        return true;
    }

    $scope.ValidateAndUploadFile = function (file, Type) {
        //alert('hi');

        //this.isValidAttendanceSheetUpload = false;
        $scope.GopalCreditCardModel.isValidDocument = false;
        if (file && file[0]) {

            $(".se-pre-con").fadeIn("slow");

            if (Type == "RecommendationOfDCSDOC")
                $scope.GopalCreditCardModel.isValidRecommendationOfDCSDOC = true;
            if (Type == "DocumentOfProposedWork")
                $scope.GopalCreditCardModel.isValidDocumentOfProposedWork = true;
            if (Type == "CopyOfDCSPassbook")
                $scope.GopalCreditCardModel.isValidCopyOfDCSPassbook = true;
            if (Type == "AadharDoc")
                $scope.GopalCreditCardModel.isValidAadharDoc = true;
            if (Type == "PanCardDoc")
                $scope.GopalCreditCardModel.isValidPanCardDoc = true;


            if (Type == "G1Photo")
                $scope.GopalCreditCardModel.isValidG1Photo = true;
            if (Type == "G2Photo")
                $scope.GopalCreditCardModel.isValidG2Photo = true;
            if (Type == "G1Signature")
                $scope.GopalCreditCardModel.isValidG1Signature = true;
            if (Type == "G2Signature")
                $scope.GopalCreditCardModel.isValidG2Signature = true;

            if (Type == "AgriculturalPropertyDoc")
                $scope.GopalCreditCardModel.isValidAgriculturalPropertyDoc = true;

            if (Type == "OtherPropertyDoc")
                $scope.GopalCreditCardModel.isValidOtherPropertyDoc = true;



            var payload = new FormData();
            payload.append("FileTypeName", Type);
            payload.append("file", file[0], file[0].name);


            $.ajax({
                type: "POST",
                url: SiteUrl + "/GopalCreditCard/UploadDocument_WithUploadtedFileType_Insameformat",
                data: payload,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (data, status) {
                    $(".se-pre-con").fadeOut("slow");

                    if (data.State == true) {
                        if (Type == "RecommendationOfDCSDOC") {
                            $scope.GopalCreditCardModel.RecommendationOfDCSDOC_Path = data.Content;
                        }
                        if (Type == "DocumentOfProposedWork") {
                            $scope.GopalCreditCardModel.DocumentOfProposedWork_Path = data.Content;
                        }
                        if (Type == "CopyOfDCSPassbook") {
                            $scope.GopalCreditCardModel.CopyOfDCSPassbook_Path = data.Content;
                        }
                        if (Type == "AadharDoc") {
                            $scope.GopalCreditCardModel.AadharDoc = data.Content;
                            $scope.AadharDoc = $scope.GopalCreditCardModel.AadharDoc;
                        }
                        if (Type == "PanCardDoc") {
                            $scope.GopalCreditCardModel.PanCardDoc = data.Content;
                            $scope.PanCardDoc = $scope.GopalCreditCardModel.PanCardDoc;
                        }


                        if (Type == "AgriculturalPropertyDoc") {
                            $scope.GopalCreditCardModel.AgriculturalPropertyDoc = data.Content;
                            $scope.AgriculturalPropertyDoc = data.Content;
                        }
                        if (Type == "OtherPropertyDoc") {
                            $scope.GopalCreditCardModel.OtherPropertyDoc = data.Content;
                            $scope.OtherPropertyDoc = data.Content;

                        }
                        if (Type == "G1Photo") {
                            $scope.GopalCreditCardModel.G1Photo = data.Content;
                            $scope.G1Photo = data.Content;
                        }
                        if (Type == "G2Photo") {
                            $scope.GopalCreditCardModel.G2Photo = data.Content;
                            $scope.G2Photo = data.Content;

                        }
                        if (Type == "G1Signature") {
                            $scope.GopalCreditCardModel.G1Signature = data.Content;
                            $scope.G1Signature = data.Content;
                        }
                        if (Type == "G2Signature") {
                            $scope.GopalCreditCardModel.G2Signature = data.Content;
                            $scope.G2Signature = data.Content;
                        }

                        //$scope.JanMemberList = data;
                    }
                    else {
                        alert(data.ErrorMessage);
                    }
                    $scope.$apply();
                },
                error: function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    alert("errror");
                }
            });
        }
    }
    $scope.ValidateAndUploadFileErrorMessage = function (errorMsg) {

        var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';
        //if ($scope.JanAadharNo == undefined || $scope.JanAadharNo == '0') {
        //message += $scope._language == 'en-us' ? '\n <br /> Please Enter JanAadhar Number' : '\n <br /> कृपया जन आधार नंबर दर्ज करें |';
        message += errorMsg;
        //}

        if (message.length > 23) {
            ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', message);
            return false;
        }
        return true;
    }

    //$scope.PLDBList = function () {
    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=-1").
    //        success(function (data, status) {
    //            $scope.PLDB = data;
    //        }).
    //        error(function (data, status) {
    //            $scope.PLDB = [];
    //        });
    //};
    $scope.PLDBBranches = [];

    $scope.BankMasterList = [];

    $scope.Reset_GCCForm = function (branch_ID) {
        $scope.GopalCreditCardModel = {};
        $scope.BankMasterList = "";
        $scope.PLDBBranches = "";
        $scope.PACSNameList = "";
        $scope.GopalCreditCardModel.LoanDistrictId = "0";
        $scope.GopalCreditCardModel.LoanBankID = "0";
        $scope.GopalCreditCardModel.LoanBranchId = "0";
        $scope.GopalCreditCardModel.PACS_ID = "0";

        $scope.GopalCreditCardModel.District_Code = "0";
        $scope.GopalCreditCardModel.City_Code = "0";
        $scope.GopalCreditCardModel.Ward_Code = "0";
        $scope.GopalCreditCardModel.Block_Code = "0";
        $scope.GopalCreditCardModel.GP_Code = "0";
        $scope.GopalCreditCardModel.Village_Code = "0";
        $scope.GopalCreditCardModel.Address_ENG = "";
        $scope.GopalCreditCardModel.Address_HI = "";
        $scope.GopalCreditCardModel.PinCode = "";
        $scope.GopalCreditCardModel.IsRural = null;
        $scope.UrbanDiv = false;
        $scope.RuralDiv = false;
        $scope.GopalCreditCardModel.IsPACSBankAccountAvailable = "";
        $scope.GopalCreditCardModel.IsPACSBankAccountAvailable_Status = false;
        $scope.GopalCreditCardModel.BankAccountNo = "";
        $scope.GopalCreditCardModel.BankIFSCCode = "";
        $scope.GopalCreditCardModel.BankName = "";
        $scope.GopalCreditCardModel.BankBranchName = "";
    };

    //$scope.SaveAuditorAssignmentOld = function () {


    $scope.PartialValidateForm = function () {
        var message = $scope._language == 'en-us' ? 'Validate following :: ' : 'निम्नलिखित मान्य करें :';
        message += '<br />';

        if ($scope.GopalCreditCardModel.Name_ENG == "" || $scope.GopalCreditCardModel.Name_ENG == null) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Applicant Name' : '\nकृपया आवेदक का नाम दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.FatherName_ENG == "" || $scope.GopalCreditCardModel.FatherName_ENG == null) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Father Name' : '\nकृपया पिता का नाम दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.AadharNo == "" || $scope.GopalCreditCardModel.AadharNo == null) {
            message += $scope._language == 'en-us' ? '\nPlease Enter AADHAR No' : '\nकृपया आधार नंबर दर्ज करें|';
            message += '<br />';
        }
        //if ($scope.GopalCreditCardModel.PANCardNo == "" || $scope.GopalCreditCardModel.PANCardNo == null) {
        //    message += $scope._language == 'en-us' ? '\nPlease Enter PAN No' : '\nकृपया पैन नंबर दर्ज करें|';
        //    message += '<br />';
        //}
        if ($scope.GopalCreditCardModel.MobileNo == "" || $scope.GopalCreditCardModel.MobileNo == null) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Mobile No' : '\nकृपया मोबाइल नंबर दर्ज करें|';
            message += '<br />';
        }




        if (($scope.GopalCreditCardModel.MemberPhoto_Base64 == "" || $scope.GopalCreditCardModel.MemberPhoto_Base64 == null) && ($scope.GopalCreditCardModel.MemberPhoto == "" || $scope.GopalCreditCardModel.MemberPhoto == null)) {
            message += $scope._language == 'en-us' ? '\nPlease Upload Applicant Photo' : '\nकृपया आवेदक का फोटो अपलोड करें|';
            message += '<br />';
        }

        if ($scope.GopalCreditCardModel.HouseBuildingNo == "" || $scope.GopalCreditCardModel.HouseBuildingNo == null || $scope.GopalCreditCardModel.HouseBuildingNo.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter House / Building Number' : '\nकृपया मकान/भवन संख्या दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.LaneStreetName == "" || $scope.GopalCreditCardModel.LaneStreetName == null || $scope.GopalCreditCardModel.LaneStreetName.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Lane / Street Name' : '\nकृपया लेन / स्ट्रीट का नाम दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.Locality == "" || $scope.GopalCreditCardModel.Locality == null || $scope.GopalCreditCardModel.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Locality' : '\nकृपया स्थान दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.Address_ENG == "" || $scope.GopalCreditCardModel.Address_ENG == null || $scope.GopalCreditCardModel.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Address' : '\nकृपया पता दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.PinCode == "" || $scope.GopalCreditCardModel.PinCode == null || $scope.GopalCreditCardModel.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Pin Code' : '\nकृपया पिन कोड दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.District_Code == "" || $scope.GopalCreditCardModel.District_Code == null || $scope.GopalCreditCardModel.District_Code == "0") {
            message += $scope._language == 'en-us' ? '\nPlease Select District' : '\nकृपया जिला चुनें|';
            message += '<br />';
        }

        if ($scope.GopalCreditCardModel.IsRural == null) {
            message += $scope._language == 'en-us' ? '\nPlease Select Area (Rural / Urban)' : '\nकृपया क्षेत्र चुनें (ग्रामीण / शहरी)|';
            message += '<br />';
        }
        else if ($scope.GopalCreditCardModel.IsRural == false || $scope.GopalCreditCardModel.IsRural == "false") {
            if ($scope.GopalCreditCardModel.City_Code == "" || $scope.GopalCreditCardModel.City_Code == null || $scope.GopalCreditCardModel.City_Code == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select City' : '\nकृपया शहर चुनें|';
                message += '<br />';
            }
            if ($scope.GopalCreditCardModel.Ward_Code == "" || $scope.GopalCreditCardModel.Ward_Code == null || $scope.GopalCreditCardModel.Ward_Code == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select Ward Number' : '\nकृपया वार्ड संख्या चुनें|';
                message += '<br />';
            }
        }
        else if ($scope.GopalCreditCardModel.IsRural == true || $scope.GopalCreditCardModel.IsRural == "true") {
            if ($scope.GopalCreditCardModel.Block_Code == "" || $scope.GopalCreditCardModel.Block_Code == null || $scope.GopalCreditCardModel.Block_Code == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select Panchyat Samity' : '\nकृपया पंचायत समिति का चयन करें|';
                message += '<br />';
            }
            if ($scope.GopalCreditCardModel.GP_Code == "" || $scope.GopalCreditCardModel.GP_Code == null || $scope.GopalCreditCardModel.GP_Code == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select Gram Panchyat' : '\nकृपया ग्राम पंचायत का चयन करें|';
                message += '<br />';
            }
            if ($scope.GopalCreditCardModel.Village_Code == "" || $scope.GopalCreditCardModel.Village_Code == null || $scope.GopalCreditCardModel.Village_Code == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select Village' : '\nकृपया गांव का चयन करें|';
                message += '<br />';
            }
        }

        if (message.length > 29) {
            ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', message);
            return false;
        } else {
            return true;
        }
    }
    $scope.ValidateForm = function () {
        var message = $scope._language == 'en-us' ? 'Validate following :: ' : 'निम्नलिखित मान्य करें :';
        message += '<br />';

        if ($scope.GopalCreditCardModel.Name_ENG == "" || $scope.GopalCreditCardModel.Name_ENG == null) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Applicant Name' : '\nकृपया आवेदक का नाम दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.FatherName_ENG == "" || $scope.GopalCreditCardModel.FatherName_ENG == null) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Father Name' : '\nकृपया पिता का नाम दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.AadharNo == "" || $scope.GopalCreditCardModel.AadharNo == null) {
            message += $scope._language == 'en-us' ? '\nPlease Enter AADHAR No' : '\nकृपया आधार नंबर दर्ज करें|';
            message += '<br />';
        }
        //if ($scope.GopalCreditCardModel.PANCardNo == "" || $scope.GopalCreditCardModel.PANCardNo == null) {
        //    message += $scope._language == 'en-us' ? '\nPlease Enter PAN No' : '\nकृपया पैन नंबर दर्ज करें|';
        //    message += '<br />';
        //}
        if ($scope.GopalCreditCardModel.MobileNo == "" || $scope.GopalCreditCardModel.MobileNo == null) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Mobile No' : '\nकृपया मोबाइल नंबर दर्ज करें|';
            message += '<br />';
        }


        if (($scope.GopalCreditCardModel.MemberPhoto_Base64 == "" || $scope.GopalCreditCardModel.MemberPhoto_Base64 == null) && ($scope.GopalCreditCardModel.MemberPhoto == "" || $scope.GopalCreditCardModel.MemberPhoto == null)) {
            message += $scope._language == 'en-us' ? '\nPlease Upload Applicant Photo' : '\nकृपया आवेदक का फोटो अपलोड करें|';
            message += '<br />';
        }

        if ($scope.GopalCreditCardModel.HouseBuildingNo == "" || $scope.GopalCreditCardModel.HouseBuildingNo == null || $scope.GopalCreditCardModel.HouseBuildingNo.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter House / Building Number' : '\nकृपया मकान/भवन संख्या दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.LaneStreetName == "" || $scope.GopalCreditCardModel.LaneStreetName == null || $scope.GopalCreditCardModel.LaneStreetName.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Lane / Street Name' : '\nकृपया लेन / स्ट्रीट का नाम दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.Locality == "" || $scope.GopalCreditCardModel.Locality == null || $scope.GopalCreditCardModel.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Locality' : '\nकृपया स्थान दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.Address_ENG == "" || $scope.GopalCreditCardModel.Address_ENG == null || $scope.GopalCreditCardModel.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Address' : '\nकृपया पता दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.PinCode == "" || $scope.GopalCreditCardModel.PinCode == null || $scope.GopalCreditCardModel.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Pin Code' : '\nकृपया पिन कोड दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.District_Code == "" || $scope.GopalCreditCardModel.District_Code == null || $scope.GopalCreditCardModel.District_Code == "0") {
            message += $scope._language == 'en-us' ? '\nPlease Select District' : '\nकृपया जिला चुनें|';
            message += '<br />';
        }

        if ($scope.GopalCreditCardModel.janAaadharFamilyMemberList.length > 0) {
            // if (($scope.GopalCreditCardModel.janAaadharFamilyMemberList.filter(x => x.Age<26)).length != 0)
            if (($scope.GopalCreditCardModel.janAaadharFamilyMemberList.filter(x => x.IsMemberSelected == 1)).length == 0) {
                message += $scope._language == 'en-us' ? '\nPlease Select Family Member' : '\nकृपया परिवार के सदस्य का चयन करें|';
                message += '<br />';
            }
        }



        if ($scope.GopalCreditCardModel.IsRural == null) {
            message += $scope._language == 'en-us' ? '\nPlease Select Area (Rural / Urban)' : '\nकृपया क्षेत्र चुनें (ग्रामीण / शहरी)|';
            message += '<br />';
        }
        else if ($scope.GopalCreditCardModel.IsRural == false || $scope.GopalCreditCardModel.IsRural == "false") {
            if ($scope.GopalCreditCardModel.City_Code == "" || $scope.GopalCreditCardModel.City_Code == null || $scope.GopalCreditCardModel.City_Code == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select City' : '\nकृपया शहर चुनें|';
                message += '<br />';
            }
            if ($scope.GopalCreditCardModel.Ward_Code == "" || $scope.GopalCreditCardModel.Ward_Code == null || $scope.GopalCreditCardModel.Ward_Code == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select Ward Number' : '\nकृपया वार्ड संख्या चुनें|';
                message += '<br />';
            }
        }
        else if ($scope.GopalCreditCardModel.IsRural == true || $scope.GopalCreditCardModel.IsRural == "true") {
            if ($scope.GopalCreditCardModel.Block_Code == "" || $scope.GopalCreditCardModel.Block_Code == null || $scope.GopalCreditCardModel.Block_Code == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select Panchyat Samity' : '\nकृपया पंचायत समिति का चयन करें|';
                message += '<br />';
            }
            if ($scope.GopalCreditCardModel.GP_Code == "" || $scope.GopalCreditCardModel.GP_Code == null || $scope.GopalCreditCardModel.GP_Code == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select Gram Panchyat' : '\nकृपया ग्राम पंचायत का चयन करें|';
                message += '<br />';
            }
            if ($scope.GopalCreditCardModel.Village_Code == "" || $scope.GopalCreditCardModel.Village_Code == null || $scope.GopalCreditCardModel.Village_Code == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select Village' : '\nकृपया गांव का चयन करें|';
                message += '<br />';
            }
        }

        //if ($scope.GopalCreditCardModel.IsPACSBankAccountAvailable == null) {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please open your account in the branch of Central Cooperative Bank after loan is approved.' : '\n <br /> कृपया ऋण स्वीकृति होने पर केंद्रीय सहकारी बैंक की शाखा में अपना खाता खुलवाये।';
        //    message += '<br />';
        //}
        //else {
        if ($scope.GopalCreditCardModel.SHG_BankName == "" || $scope.GopalCreditCardModel.SHG_BankName == null || $scope.GopalCreditCardModel.SHG_BankName.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter SHG Bank Name' : '\nकृपया एस.एच.जी. बैंक का नाम दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.SHG_BranchName == "" || $scope.GopalCreditCardModel.SHG_BranchName == null || $scope.GopalCreditCardModel.SHG_BranchName.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Bank SHG Branch Name' : '\nकृपया एस.एच.जी. बैंक शाखा का नाम दर्ज करें|';

            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.SHG_IFSC_Code == "" || $scope.GopalCreditCardModel.SHG_IFSC_Code == null || $scope.GopalCreditCardModel.SHG_IFSC_Code.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Bank SHG IFSC Code' : '\nकृपया एस.एच.जी. बैंक IFSC कोड दर्ज करें|';

            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.SHG_AccountNo == "" || $scope.GopalCreditCardModel.SHG_AccountNo == null || $scope.GopalCreditCardModel.SHG_AccountNo.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter SHG Account Number' : '\nकृपया एस.एच.जी. खाता संख्या दर्ज करें|';

            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.SHG_Mem_BankName == "" || $scope.GopalCreditCardModel.SHG_Mem_BankName == null || $scope.GopalCreditCardModel.SHG_Mem_BankName.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter SHG Member Bank Name' : '\nकृपया एस.एच.जी. सदस्य बैंक का नाम दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.SHG_Mem_BranchName == "" || $scope.GopalCreditCardModel.SHG_Mem_BranchName == null || $scope.GopalCreditCardModel.SHG_Mem_BranchName.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Bank SHG Member Branch Name' : '\nकृपया एस.एच.जी. सदस्य बैंक शाखा का नाम दर्ज करें|';

            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.SHG_Mem_IFSC_Code == "" || $scope.GopalCreditCardModel.SHG_Mem_IFSC_Code == null || $scope.GopalCreditCardModel.SHG_Mem_IFSC_Code.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Bank SHG Member IFSC Code' : '\nकृपया एस.एच.जी. सदस्य बैंक IFSC कोड दर्ज करें|';

            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.SHG_Mem_AccountNo == "" || $scope.GopalCreditCardModel.SHG_Mem_AccountNo == null || $scope.GopalCreditCardModel.SHG_Mem_AccountNo.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter SHG Member Account Number' : '\nकृपया एस.एच.जी. सदस्य खाता संख्या दर्ज करें|';

            message += '<br />';
        }



        if ($scope.GopalCreditCardModel.IsMemberOfDCS == null) {
            message += $scope._language == 'en-us' ? '\nPlease Select Local Dairy Cooperative Society (DCS) Membership' : '\nकृपया स्थानीय डेयरी सहकारी समिति (DCS) सदस्यता का चयन करें|';
            message += '<br />';
        }
        else if ($scope.GopalCreditCardModel.IsMemberOfDCS == true || $scope.GopalCreditCardModel.IsMemberOfDCS == "true") {

            if ($scope.GopalCreditCardModel.DCSName == "undefined" || $scope.GopalCreditCardModel.DCSName == null || $scope.GopalCreditCardModel.DCSName.length == 0) {
                message += $scope._language == 'en-us' ? '\nPlease Enter Local Dairy Cooperative Society (DCS) Name' : '\nकृपया स्थानीय डेयरी सहकारी समिति (डीसीएस) पंजीकरण संख्या दर्ज करें|';
                message += '<br />';
            }
            if ($scope.GopalCreditCardModel.DCSMembershipNo == "undefined" || $scope.GopalCreditCardModel.DCSMembershipNo == null || $scope.GopalCreditCardModel.DCSMembershipNo.length == 0) {
                message += $scope._language == 'en-us' ? '\nPlease Enter Local Dairy Cooperative Society (DCS) Membership Number' : '\nकृपया स्थानीय डेयरी सहकारी समिति (DCS) की सदस्यता संख्या दर्ज करें|';
                message += '<br />';
            }
            if ($scope.GopalCreditCardModel.EstimatedAmountOfMilkSoldToDCS == "" || $scope.GopalCreditCardModel.EstimatedAmountOfMilkSoldToDCS == null || $scope.GopalCreditCardModel.EstimatedAmountOfMilkSoldToDCS == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Enter Estimated Amount of Milk Sold to Local Dairy Cooperative Society (DCS)' : '\nकृपया स्थानीय डेयरी सहकारी समिति (DCS) को बेचे गए दूध की अनुमानित मात्रा दर्ज करें|';
                message += '<br />';
            }
        }
              

        // }


        //if ($scope.GopalCreditCardModel.LoanDistrictId == "" || $scope.GopalCreditCardModel.LoanDistrictId == null || $scope.GopalCreditCardModel.LoanDistrictId == "0") {
        //    message += $scope._language == 'en-us' ? '\nPlease Select District for Loan' : '\nकृपया ऋण के लिए जिला चुनें|';
        //    message += '<br />';
        //}
        //if ($scope.GopalCreditCardModel.LoanBankID == "" || $scope.GopalCreditCardModel.LoanBankID == null || $scope.GopalCreditCardModel.LoanBankID == "0") {
        //    message += $scope._language == 'en-us' ? '\nPlease Select Bank for Loan' : '\nकृपया ऋण के लिए बैंक का चयन करें|';
        //    message += '<br />';
        //}
        //if ($scope.GopalCreditCardModel.LoanBranchId == "" || $scope.GopalCreditCardModel.LoanBranchId == null || $scope.GopalCreditCardModel.LoanBranchId == "0") {
        //    message += $scope._language == 'en-us' ? '\nPlease Select Bank Branch for Loan' : '\nकृपया ऋण के लिए बैंक शाखा का चयन करें|';
        //    message += '<br />';
        //}
        //if ($scope.GopalCreditCardModel.PACS_ID == "" || $scope.GopalCreditCardModel.PACS_ID == null || $scope.GopalCreditCardModel.PACS_ID == "0") {
        //    message += $scope._language == 'en-us' ? '\nPlease Select PACS Name for Loan' : '\nकृपया ऋण के लिए पैक्स का नाम चुनें|';
        //    message += '<br />';
        //}

        //if ($scope.GopalCreditCardModel.IsMemberOfDCS == null) {
        //    message += $scope._language == 'en-us' ? '\nPlease Select Local Dairy Cooperative Society (DCS) Membership' : '\nकृपया स्थानीय डेयरी सहकारी समिति (DCS) सदस्यता का चयन करें|';
        //    message += '<br />';
        //}
        //else if ($scope.GopalCreditCardModel.IsMemberOfDCS == true || $scope.GopalCreditCardModel.IsMemberOfDCS == "true") {

        //    if ($scope.GopalCreditCardModel.DCSRegistrationNo == "undefined" || $scope.GopalCreditCardModel.DCSRegistrationNo == null || $scope.GopalCreditCardModel.DCSRegistrationNo.length == 0) {
        //        message += $scope._language == 'en-us' ? '\nPlease Enter Local Dairy Cooperative Society (DCS) Registration Number' : '\nकृपया स्थानीय डेयरी सहकारी समिति (DCS) का नाम दर्ज करें|';
        //        message += '<br />';
        //    }

        //    if ($scope.GopalCreditCardModel.DCSName == "undefined" || $scope.GopalCreditCardModel.DCSName == null || $scope.GopalCreditCardModel.DCSName.length == 0) {
        //        message += $scope._language == 'en-us' ? '\nPlease Enter Local Dairy Cooperative Society (DCS) Name' : '\nकृपया स्थानीय डेयरी सहकारी समिति (डीसीएस) पंजीकरण संख्या दर्ज करें|';
        //        message += '<br />';
        //    }
        //    if ($scope.GopalCreditCardModel.DCSOwnerName == "undefined" || $scope.GopalCreditCardModel.DCSOwnerName == null || $scope.GopalCreditCardModel.DCSOwnerName.length == 0) {
        //        message += $scope._language == 'en-us' ? '\nPlease Enter Local Dairy Cooperative Society (DCS) Administrator/Chairman Name' : '\nकृपया स्थानीय डेयरी सहकारी समिति (DCS) व्यवस्थापक/अध्यक्ष का नाम दर्ज करें|';
        //        message += '<br />';
        //    }
        //    if ($scope.GopalCreditCardModel.DCSOwnerMobileNo == "undefined" || $scope.GopalCreditCardModel.DCSOwnerMobileNo == null || $scope.GopalCreditCardModel.DCSOwnerMobileNo.length == 0) {
        //        message += $scope._language == 'en-us' ? '\nPlease Enter Local Dairy Cooperative Society (DCS) Administrator/Chairman Mobile Number' : '\nकृपया स्थानीय डेयरी सहकारी समिति (DCS) के व्यवस्थापक/अध्यक्ष का मोबाइल नंबर दर्ज करें|';
        //        message += '<br />';
        //    }
        //    if ($scope.GopalCreditCardModel.DCSMembershipNo == "undefined" || $scope.GopalCreditCardModel.DCSMembershipNo == null || $scope.GopalCreditCardModel.DCSMembershipNo.length == 0) {
        //        message += $scope._language == 'en-us' ? '\nPlease Enter Local Dairy Cooperative Society (DCS) Membership Number' : '\nकृपया स्थानीय डेयरी सहकारी समिति (DCS) की सदस्यता संख्या दर्ज करें|';
        //        message += '<br />';
        //    }
        //    if ($scope.GopalCreditCardModel.DCSAddress == "undefined" || $scope.GopalCreditCardModel.DCSAddress == null || $scope.GopalCreditCardModel.DCSAddress.length == 0) {
        //        message += $scope._language == 'en-us' ? '\nPlease Enter Local Dairy Cooperative Society (DCS) Address' : '\nकृपया स्थानीय डेयरी सहकारी समिति (DCS) का पता दर्ज करें|';
        //        message += '<br />';
        //    }
        //}
        //else if ($scope.GopalCreditCardModel.IsMemberOfDCS == false || $scope.GopalCreditCardModel.IsMemberOfDCS == "false") {

        //    message = $scope._language == 'en-us' ? '\n <br />Please Select Local Dairy Cooperative Society (DCS) Membership In case the cow keeper does not sell the milk to the dairy cooperative society, he is not eligible to apply for Gopal Credit Card..' : '\n <br />कृपया स्थानीय डेयरी सहकारी समिति (DCS) सदस्यता का चयन करें| गौ पालक द्वारा दूध का बेचान डेयरी सहकारी समिति को नही किये जाने की स्थिति में, गोपाल क्रेडिट कार्ड में आवेदन करने के लिए पात्र नहीं हैं।';
        //    message += '<br />';
        //}

        //if ($scope.GopalCreditCardModel.IsResideInPACSLAMPSArea == null) {
        //    message += $scope._language == 'en-us' ? '\nPlease Select PACS / LAMPS Area' : '\nकृपया PACS / LAMPS क्षेत्र का चयन करें|';
        //    message += '<br />';
        //}

        //if ($scope.GopalCreditCardModel.IsPACSLAMPSMember == null) {
        //    message += $scope._language == 'en-us' ? '\nPlease Select PACS / LAMPS Membership' : '\nकृपया PACS / LAMPS सदस्यता का चयन करें|';
        //    message += '<br />';
        //}
        //else if ($scope.GopalCreditCardModel.IsPACSLAMPSMember == true || $scope.GopalCreditCardModel.IsPACSLAMPSMember == "true") {
        //    if ($scope.GopalCreditCardModel.PACSLAMPSMembershipNo == "" || $scope.GopalCreditCardModel.PACSLAMPSMembershipNo == null || $scope.GopalCreditCardModel.PACSLAMPSMembershipNo.length == 0) {
        //        message += $scope._language == 'en-us' ? '\nPlease Enter PACS / LAMPS Membership Detail' : '\nकृपया PACS / LAMPS सदस्यता विवरण दर्ज करें|';
        //        message += '<br />';
        //    }
        //}
        //if ($scope.GopalCreditCardModel.DoesTheMilkSaleToDCS == null) {
        //    message += $scope._language == 'en-us' ? '\nPlease select, Does the cattle keeper sell the milk to the dairy cooperative society?' : '\nकृपया सेलेक्ट करें,क्या पशुपालक द्वारा दूध का बेचान डेयरी सहकारी समिति को किया जाता है या नहीं|';
        //    message += '<br />';
        //}
        //else 
        //if ($scope.GopalCreditCardModel.DoesTheMilkSaleToDCS == true || $scope.GopalCreditCardModel.DoesTheMilkSaleToDCS == "true") {
        //    if ($scope.GopalCreditCardModel.EstimatedAmountOfMilkSoldToDCS == "" || $scope.GopalCreditCardModel.EstimatedAmountOfMilkSoldToDCS == null || $scope.GopalCreditCardModel.EstimatedAmountOfMilkSoldToDCS == "0") {
        //        message += $scope._language == 'en-us' ? '\nPlease Enter Estimated Amount of Milk Sold to Local Dairy Cooperative Society (DCS)' : '\nकृपया स्थानीय डेयरी सहकारी समिति (DCS) को बेचे गए दूध की अनुमानित मात्रा दर्ज करें|';
        //        message += '<br />';
        //    }
        //}
        //else if ($scope.GopalCreditCardModel.DoesTheMilkSaleToDCS == false || $scope.GopalCreditCardModel.DoesTheMilkSaleToDCS == "false") {
        //    message += $scope._language == 'en-us' ? '\n <br /> In case the cattle keeper does not sell the milk to the dairy cooperative society, they are not eligible to apply for Gopal Credit Card.' : '\n <br /> पशुपालक द्वारा दूध का बेचान डेयरी सहकारी समिति को नही किये जाने की स्थिति में, गोपाल क्रेडिट कार्ड में आवेदन करने के लिए पात्र नहीं हैं। '
        //    message += '<br />';
        //}


        //if ($scope.GopalCreditCardModel.IsRecommendationOfDCSUploaded == null) { //5Dec2024
        //    message += $scope._language == 'en-us' ? '\nPlease Select Obtain and upload the recommendation of the dairy cooperative society' : '\nकृपया चयन करें कि DCS की अनुशंसा प्राप्त कर अपलोड करें|';
        //    message += '<br />';
        //}
        //else if ($scope.GopalCreditCardModel.IsRecommendationOfDCSUploaded == true || $scope.GopalCreditCardModel.IsRecommendationOfDCSUploaded == "true") {
        //    if ($scope.GopalCreditCardModel.isValidRecommendationOfDCSDOC == false || $scope.GopalCreditCardModel.RecommendationOfDCSDOC_Path == "" || $scope.GopalCreditCardModel.RecommendationOfDCSDOC_Path == null) {
        //        message += $scope._language == 'en-us' ? '\nPlease Upload Obtain and upload the recommendation of the dairy cooperative society' : '\nकृपया डीसीएस दस्तावेज़ की वैध अनुशंसा अपलोड करें|';
        //        message += '<br />';
        //    }
        //}


        if ($scope.GopalCreditCardModel.DesiredCreditLimitLoanAmount == "" || $scope.GopalCreditCardModel.DesiredCreditLimitLoanAmount == null || $scope.GopalCreditCardModel.DesiredCreditLimitLoanAmount == "0") {
            message += $scope._language == 'en-us' ? '\nPlease Enter Desired Credit Limit of Loan Amount' : '\nकृपया ऋण राशि की वांछित क्रेडिट सीमा दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.LoanAmountRecommendedByDCS == "" || $scope.GopalCreditCardModel.LoanAmountRecommendedByDCS == null || $scope.GopalCreditCardModel.LoanAmountRecommendedByDCS == "0") {
            message += $scope._language == 'en-us' ? '\nPlease Enter Loan Amount Recommendation' : '\nकृपया अनुशंसित ऋण राशि दर्ज करें|';
            message += '<br />';
        }
        //if ($scope.GopalCreditCardModel.IsCopyOfDCSPassbookAttached == null) {
        //    message += $scope._language == 'en-us' ? '\nPlease Upload a Valid Copy of Bank Passbook.' : '\nकृपया बैंक पासबुक की वैध प्रति अपलोड करें|';
        //    message += '<br />';
        //}
        //if ($scope.GopalCreditCardModel.PBBankAccountNo == null || $scope.GopalCreditCardModel.PBBankAccountNo == '' || $scope.GopalCreditCardModel.PBBankAccountNo.length == 0) {
        //    message += $scope._language == 'en-us' ? '\nPlease enter the recipient account number.' : '\nकृपया प्राप्तकर्ता खाता संख्या दर्ज करें|';
        //    message += '<br />';
        //}
        //else if ($scope.GopalCreditCardModel.IsCopyOfDCSPassbookAttached == true) {
        //    if ($scope.GopalCreditCardModel.isValidCopyOfDCSPassbook == false || $scope.GopalCreditCardModel.CopyOfDCSPassbook_Path == "" || $scope.GopalCreditCardModel.CopyOfDCSPassbook_Path == null) {
        //        message += $scope._language == 'en-us' ? '\nPlease Upload a Valid Copy of Bank Passbook.' : '\nकृपया बैंक पासबुक की वैध प्रति अपलोड करें|';
        //        message += '<br />';
        //    }
        //}
        //if ($scope.GopalCreditCardModel.isValidDocumentOfProposedWork == false || $scope.GopalCreditCardModel.DocumentOfProposedWork_Path == "" || $scope.GopalCreditCardModel.DocumentOfProposedWork_Path == null) {
        //    message += $scope._language == 'en-us' ? '\nPlease Upload Valid Document of Proposed Work.' : '\nकृपया प्रस्तावित कार्य का वैध दस्तावेज़ अपलोड करें|';
        //    message += '<br />';
        //}
        if ($scope.GopalCreditCardModel.ECSMandateConfirmation == false) {
            message += $scope._language == 'en-us' ? '\nPlease Select Declareation the ECS Mandate of 12 Equal Monthly Installments of the Sanctioned Loan Amount.' : '\nकृपया स्वीकृत ऋण राशि की 12 समान मासिक किस्तों के ईसीएस आदेश की घोषणा का चयन करें।';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.GopalCreditCardLoanDeclaration == false) {
            message += $scope._language == 'en-us' ? '\nPlease Declare that no Other Member of my Gopalak Family has Taken Loan Under the Said Scheme Earlier or Subsequently.' : '\nकृपया घोषणा करे कि मेरे गोपालक परिवार के किसी अन्य सदस्य ने पहले या बाद में उक्त योजना के तहत ऋण नहीं लिया है।';
            message += '<br />';
        }

        if ($scope.GopalCreditCardModel.G1Name == null || $scope.GopalCreditCardModel.G1Name == '') {
            message += $scope._language == 'en-us' ? '\nPlease Enter the Guarantor Details.' : '\nकृपया जमानतदार का विवरण दर्ज करें।';
            message += '<br />';
        }
        //if ($scope.GopalCreditCardModel.G1PANCardNo == null || $scope.GopalCreditCardModel.G1PANCardNo == '') {
        //    message += $scope._language == 'en-us' ? '\nPlease Enter the Guarantor Pancard Details' : '\nकृपया जमानतदार का पैन कार्ड विवरण दर्ज करें।';
        //    message += '<br />';
        //}
        if ($scope.GopalCreditCardModel.G1AadharNo == null || $scope.GopalCreditCardModel.G1AadharNo == '') {
            message += $scope._language == 'en-us' ? '\nPlease Enter the Guarantor Aadhar Details' : '\nकृपया जमानतदार का आधार विवरण दर्ज करें।';
            message += '<br />';
        }


        if ($scope.GopalCreditCardModel.G2Name == null || $scope.GopalCreditCardModel.G2Name == '') {
            message += $scope._language == 'en-us' ? '\nPlease Enter the Guarantor Details.' : '\nकृपया जमानतदार का विवरण दर्ज करें।';
            message += '<br />';
        }
        //if ($scope.GopalCreditCardModel.G2PANCardNo == null || $scope.GopalCreditCardModel.G2PANCardNo == '') {
        //    message += $scope._language == 'en-us' ? '\nPlease Enter the Guarantor Pancard Details' : '\nकृपया जमानतदार का पैन कार्ड विवरण दर्ज करें।';
        //    message += '<br />';
        //}
        if ($scope.GopalCreditCardModel.G2AadharNo == null || $scope.GopalCreditCardModel.G2AadharNo == '') {
            message += $scope._language == 'en-us' ? '\nPlease Enter the Guarantor Aadhar Details' : '\nकृपया जमानतदार का आधार विवरण दर्ज करें।';
            message += '<br />';
        }



        if (($scope.GopalCreditCardModel.G1AadharNo != null && $scope.GopalCreditCardModel.G1AadharNo != '') && ($scope.GopalCreditCardModel.G2AadharNo != null && $scope.GopalCreditCardModel.G2AadharNo != '')) {
            if ($scope.GopalCreditCardModel.G1AadharNo == $scope.GopalCreditCardModel.G2AadharNo) {
                message += $scope._language == 'en-us' ? '\nPlease enter individual Aadhaar details of Guarantor' : '\nकृपया जमानतदार का अलग-अलग आधार का विवरण दर्ज करें';
                message += '<br />';
            }

        }
        if (($scope.GopalCreditCardModel.G1PANCardNo != null && $scope.GopalCreditCardModel.G1PANCardNo != '') && ($scope.GopalCreditCardModel.G2PANCardNo != null && $scope.GopalCreditCardModel.G2PANCardNo != '')) {
            if ($scope.GopalCreditCardModel.G1PANCardNo == $scope.GopalCreditCardModel.G2PANCardNo) {
                message += $scope._language == 'en-us' ? '\nPlease enter individual PANCard details of Guarantor' : '\nकृपया जमानतदार का अलग-अलग पैन कार्ड का विवरण दर्ज करें';
                message += '<br />';
            }

        }
        if ($scope.GopalCreditCardModel.DesiredCreditLimitLoanAmount != '' && $scope.GopalCreditCardModel.DesiredCreditLimitLoanAmount != null && $scope.GopalCreditCardModel.DesiredCreditLimitLoanAmount != undefined) {
            if (parseInt($scope.GopalCreditCardModel.DesiredCreditLimitLoanAmount) > 100000) {
                message += $scope._language == 'en-us' ? '\nPlease enter amount up to 1 Lakh Tk' : '\nकृपया 1 लाख तक की राशि दर्ज करें';
                message += '<br />';
            }
        }

        if ($scope.GopalCreditCardModel.G1Photo == undefined || $scope.GopalCreditCardModel.G1Photo == null || $scope.GopalCreditCardModel.G1Photo == '' || $scope.GopalCreditCardModel.G2Photo == undefined || $scope.GopalCreditCardModel.G2Photo == null || $scope.GopalCreditCardModel.G2Photo == '') {
            message += $scope._language == 'en-us' ? '\nPlease upload the Photo of the guarantor ' : '\nकृपया जमानतदार का फोटो अपलोड करें';
            message += '<br />';

        }
        if ($scope.GopalCreditCardModel.G1Signature == undefined || $scope.GopalCreditCardModel.G1Signature == null || $scope.GopalCreditCardModel.G1Signature == '' || $scope.GopalCreditCardModel.G2Signature == undefined || $scope.GopalCreditCardModel.G2Signature == null || $scope.GopalCreditCardModel.G2Signature == '') {
            message += $scope._language == 'en-us' ? '\nPlease upload the signature of the guarantor' : '\nकृपया जमानतदार के हस्ताक्षर अपलोड करें';
            message += '<br />';

        }
        if ($scope.GopalCreditCardModel.AadharDoc == undefined || $scope.GopalCreditCardModel.AadharDoc == null || $scope.GopalCreditCardModel.AadharDoc == '') {
            message += $scope._language == 'en-us' ? '\nPlease upload Aadhaar Card Document' : '\nकृपया आधार कार्ड दस्तावेज़ अपलोड करें';
            message += '<br />';

        }

        

            if ($scope.GopalCreditCardModel.Mn_DistrictId == "" || $scope.GopalCreditCardModel.Mn_DistrictId == null || $scope.GopalCreditCardModel.Mn_DistrictId == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select District for Mahila Nidhi' : '\nकृपया महिला निधि के लिए जिला चुनें|';
                message += '<br />';
            }
            if ($scope.GopalCreditCardModel.Mn_BlockID == "" || $scope.GopalCreditCardModel.Mn_BlockID == null || $scope.GopalCreditCardModel.Mn_BlockID == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select Block for Mahila Nidhi' : '\nकृपया महिला निधि के लिए खंड का चयन करें|';
                message += '<br />';
            }
            if ($scope.GopalCreditCardModel.Mn_ClusterId == "" || $scope.GopalCreditCardModel.Mn_ClusterId == null || $scope.GopalCreditCardModel.Mn_ClusterId == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select CLF Branch for Mahila Nidhi' : '\nकृपया महिला निधि के लिए सीएलएफ का चयन करें|';
                message += '<br />';
            }
            if ($scope.GopalCreditCardModel.Mn_VoID == "" || $scope.GopalCreditCardModel.Mn_VoID == null || $scope.GopalCreditCardModel.Mn_VoID == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select VO Name for Mahila Nidhi' : '\nकृपया महिला निधि के लिए वीओ का नाम चुनें|';
                message += '<br />';
            }
            if ($scope.GopalCreditCardModel.Mn_ShgId == "" || $scope.GopalCreditCardModel.Mn_ShgId == null || $scope.GopalCreditCardModel.Mn_ShgId == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select SHG Name for Mahila Nidhi' : '\nकृपया महिला निधि के लिए एसएचजी नाम चुनें|';
                message += '<br />';
            }



        

        if (message.length > 29) {
            ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', message);
            return false;
        } else {
            return true;
        }
    }

    $scope.FillGuarantorDetails = function () {
        $scope.GuarantorDetailsModel = [];
        $scope.GuarantorDetailsModel.push({
            id: 0,
            Name: $scope.GopalCreditCardModel.G1Name,
            FatherSpouseName: $scope.GopalCreditCardModel.G1FatherSpouseName,
            DOB: $scope.GopalCreditCardModel.G1DOB,
            JanAadharNumber: $scope.GopalCreditCardModel.G1JanAadharNumber,
            AadharNo: $scope.GopalCreditCardModel.G1AadharNo,
            PANCardNo: $scope.GopalCreditCardModel.G1PANCardNo,
            MobileNo: $scope.GopalCreditCardModel.G1MobileNo,
            Address: $scope.GopalCreditCardModel.G1Address,
            GuarranterLandProperty: $scope.GopalCreditCardModel.G1GuarranterLandProperty,
            Photo: $scope.GopalCreditCardModel.G1Photo,
            Signature: $scope.GopalCreditCardModel.G1Signature
        });

        $scope.GuarantorDetailsModel.push({
            id: 0,
            Name: $scope.GopalCreditCardModel.G2Name,
            FatherSpouseName: $scope.GopalCreditCardModel.G2FatherSpouseName,
            DOB: $scope.GopalCreditCardModel.G2DOB,
            JanAadharNumber: $scope.GopalCreditCardModel.G2JanAadharNumber,
            AadharNo: $scope.GopalCreditCardModel.G2AadharNo,
            PANCardNo: $scope.GopalCreditCardModel.G2PANCardNo,
            MobileNo: $scope.GopalCreditCardModel.G2MobileNo,
            Address: $scope.GopalCreditCardModel.G2Address,
            GuarranterLandProperty: $scope.GopalCreditCardModel.G2GuarranterLandProperty,
            Photo: $scope.GopalCreditCardModel.G2Photo,
            Signature: $scope.GopalCreditCardModel.G2Signature
        });
        $scope.GopalCreditCardModel.GuarantorDetailsList = $scope.GuarantorDetailsModel;
    }

    $scope.Save_GopalCreditCardForm = function (StatusId) {
        if (StatusId == 1) {
            if (!$scope.PartialValidateForm())
                return;
        }
        else {
            if (!$scope.ValidateForm())
                return;
        }

        $scope.GopalCreditCardModel.CropLoanDetailsList = $scope.CropLoanDetailsList;
        $scope.GopalCreditCardModel.AgricultureLandDetailList = null;
        $scope.FillGuarantorDetails();


        //var encryptedAadhaarNo = "";
        //if ($scope.GopalCreditCardModel.AadharNo != null && $scope.GopalCreditCardModel.AadharNo != "")
        //    encryptedAadhaarNo = encryptData($scope.GopalCreditCardModel.AadharNo);       
        //$scope.GopalCreditCardModel.AadharNo = encryptedAadhaarNo;


        if ($scope.GopalCreditCardModel.AadharNo != null && $scope.GopalCreditCardModel.AadharNo != "")
            $scope.GopalCreditCardModel.AadharNo = encodeURIComponent(encryptData($scope.GopalCreditCardModel.AadharNo));


        if ($scope.GopalCreditCardModel.PANCardNo != null && $scope.GopalCreditCardModel.PANCardNo != "")
            $scope.GopalCreditCardModel.PANCardNo = encodeURIComponent(encryptData($scope.GopalCreditCardModel.PANCardNo));

        if ($scope.GopalCreditCardModel.JanAadharNo != null && $scope.GopalCreditCardModel.JanAadharNo != "")
            $scope.GopalCreditCardModel.JanAadharNo = encodeURIComponent(encryptData($scope.GopalCreditCardModel.JanAadharNo));


        //    Guarrantor details

        $scope.GopalCreditCardModel.GuarantorDetailsList.forEach(member => {
            if (member.JanAadharNo != null && member.JanAadharNo != "") {
                member.JanAadharNo = encodeURIComponent(encryptData(member.JanAadharNo));
            }
            if (member.AadharNo != null && member.AadharNo != "") {
                member.AadharNo = encodeURIComponent(encryptData(member.AadharNo));
            }
            if (member.PANCardNo != null && member.PANCardNo != "") {
                member.PANCardNo = encodeURIComponent(encryptData(member.PANCardNo));
            }
        });

        //GopalFamilyDetails       

        $scope.GopalCreditCardModel.janAaadharFamilyMemberList.forEach(member => {
            if (member.JanAadharNo != null && member.JanAadharNo != "") {
                member.JanAadharNo = encodeURIComponent(encryptData(member.JanAadharNo));
            }
        });
        
        $scope.ShgDataView();

        $scope.GopalCreditCardModel.StatusId = StatusId;
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/GopalCreditCard/Save_GopalCreditCardForm", $scope.GopalCreditCardModel,
            {
                dataType: false,
                contentType: false,
                processData: false,
            }
        ).
            success(function (data, status) {

                $(".se-pre-con").fadeOut("slow");
                if (data.Status == true) {
                    //$scope.Reset_GCCForm();
                    // ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', 'Gopal Credit Card Registration Data Save Sucessfully');
                    alert($scope._language == 'en-us' ? 'Gopal Credit Card Registration Data Save Sucessfully' : 'गोपाल क्रेडिट कार्ड पंजीकरण डेटा सफलतापूर्वक सहेजा गया।');
                    //if (StatusId == 2)
                    //    window.location = '../GopalCreditCard/GopalApplicationFormPart2?gccEncId=' + data.Content;
                    //else
                    // window.location.reload();
                    window.location = '../GopalCreditCard/GopalApplicationForm';
                }
                else if (data.Status == false) {
                    // $scope.Reset_GCCForm();
                    if (data.ErrorMessage == "GCC_APPLICATIONALREADYEXIST") {
                        ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', $scope._language == 'en-us' ? 'Application for Gopal Credit Card Scheme has Already been Made with Janadhaar Number ' + $scope.GopalCreditCardModel.JanAadharNo + '. Hence You are not Eligible to Apply for Gopal Credit Card Scheme.' : 'जनाधार नंबर ' + $scope.GopalCreditCardModel.JanAadharNo + ' से पहले ही गोपाल क्रेडिट कार्ड योजना के लिए आवेदन किया जा चुका है। अतः आप गोपाल क्रेडिट कार्ड योजना मैं आवेदन के लिए योग्य नहीं हैं।');
                    }
                    if (data.ErrorMessage == "GCC_IsRural_False") {
                        ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', $scope._language == 'en-us' ? 'Residents of urban areas are not eligible to apply for Gopal Credit Card.' : 'शहरी क्षेत्रों के निवासी गोपाल क्रेडिट कार्ड के लिए आवेदन करने के पात्र नहीं हैं|');
                    }
                    //if (data.ErrorMessage == "GCC_DoesTheMilkSaleToDCS_False") {
                    //    ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', $scope._language == 'en-us' ? 'In case the cow keeper does not sell the milk to the dairy cooperative society, they are not eligible to apply for Gopal Credit Card.' : 'गौपालक द्वारा दूध का बेचान डेयरी सहकारी समिति को नही किये जाने की स्थिति में, गोपाल क्रेडिट कार्ड में आवेदन करने के लिए पात्र नहीं हैं।');
                    //}
                    if (data.ErrorMessage == "GCC_IsPACSBankAccountAvailable_False") {
                        ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', $scope._language == 'en-us' ? 'Please Ensure to Open an Account in PACS Bank before Applying for Gopal Credit Card Scheme.' : 'कृपया गोपाल क्रेडिट कार्ड योजना में आवेदन करने से पूर्व पैक्स बैंक में खाता खुलवाना सुनिश्चियत करें।');
                    }
                    //if (data.ErrorMessage == "GCC_IsRecommendationOfDCSUploaded_False") {
                    //    ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', $scope._language == 'en-us' ? 'It is Mandatory to Upload the Recommendation of the Dairy Co-Operative Society.' : 'डेयरी सहकारी समिति की अनुशंषा अपलोड करना अनिवार्य है।');
                    //}
                    //window.location = '../GopalCreditCard/GopalApplicationForm';
                }
                else if (data == -1) {
                    // $scope.Reset_GCCForm();
                    ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', 'Registered Detail Already Exists');
                }
                else {
                    // $scope.Reset_GCCForm();
                    ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', 'An Error Occurred Please Try Again Later');
                }

            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                // $$scope.Reset_GCCForm();
            });

    }



    $scope.GetCCBDetails = function () {
        $http.post("../gopalCreditCard/GetCurrentUserCCBDetail").
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.CCBDetails = data[0];
                    $scope.GopalCreditCardModel.LoanDistrictId = ($scope.CCBDetails.DistrictId).toString();
                    document.getElementById('DistId').setAttribute('disabled', 'disabled');
                    //  $scope.LoanBankList();
                }
            }).
            error(function (data, status) {
                $scope.CCBDetails = '';
            });
    }


    $scope.DivtringGrid = true;
    $scope.GetFormEditData = function (StatusId, GCCEId, GCCERNO) {
        if (StatusId == 1 || StatusId == 13 || 1 == 1) {
            //$http.post(SiteUrl + "/GopalCreditCard/Get_GopalApplicationFormViewData1/?GCCEId=" + GCCEId + "&GCCERNO=" + GCCERNO).
            $http.post(SiteUrl + "/GopalCreditCard/Get_GopalApplicationFormViewData_v1/?GCCEId=" + GCCEId).
                success(function (data, status) {
                    $scope.GopalCreditCardModel = data;


                    $scope.GopalCreditCardModel.IsCopyOfDCSPassbookAttached = (data.CopyOfDCSPassbook_Path != '' || data.PBBankAccountNo != '') ? true : false;
                    $scope.GopalCreditCardModel.IsPACSBankAccountAvailable = (data.BankAccountNo != '' || data.BankIFSCCode || data.BankName || data.BankBranchName) ? true : false;
                    if (data.CropLoanDetailsList != null && data.CropLoanDetailsList.length > 0)
                        $scope.CropLoanDetailsList = data.CropLoanDetailsList;
                    if (data.AgricultureLandDetailList != null && data.AgricultureLandDetailList.length > 0)
                        $scope.AgricultureLandDetailList = data.AgricultureLandDetailList;
                    if (data.GuarantorDetailsList[0] != null) {
                        $scope.GopalCreditCardModel.G1Name = data.GuarantorDetailsList[0].Name;
                        $scope.GopalCreditCardModel.G1FatherSpouseName = data.GuarantorDetailsList[0].FatherSpouseName;
                        $scope.GopalCreditCardModel.G1DOB = data.GuarantorDetailsList[0].DOB != null ? new Date(data.GuarantorDetailsList[0].DOB) : null;
                        $scope.GopalCreditCardModel.G1JanAadharNumber = data.GuarantorDetailsList[0].JanAadharNumber;
                        $scope.GopalCreditCardModel.G1AadharNo = data.GuarantorDetailsList[0].AadharNo;
                        $scope.GopalCreditCardModel.G1PANCardNo = data.GuarantorDetailsList[0].PANCardNo;
                        $scope.GopalCreditCardModel.G1MobileNo = data.GuarantorDetailsList[0].MobileNo;
                        $scope.GopalCreditCardModel.G1Address = data.GuarantorDetailsList[0].Address;
                        $scope.GopalCreditCardModel.G1GuarranterLandProperty = data.GuarantorDetailsList[0].GuarranterLandProperty;
                        $scope.GopalCreditCardModel.G1Photo = data.GuarantorDetailsList[0].Photo;
                        $scope.GopalCreditCardModel.G1Signature = data.GuarantorDetailsList[0].Signature;
                    }
                    if (data.GuarantorDetailsList[1] != null) {
                        $scope.GopalCreditCardModel.G2Name = data.GuarantorDetailsList[1].Name;
                        $scope.GopalCreditCardModel.G2FatherSpouseName = data.GuarantorDetailsList[1].FatherSpouseName;
                        $scope.GopalCreditCardModel.G2DOB = data.GuarantorDetailsList[1].DOB != null ? new Date(data.GuarantorDetailsList[1].DOB) : null;
                        $scope.GopalCreditCardModel.G2JanAadharNumber = data.GuarantorDetailsList[1].JanAadharNumber;
                        $scope.GopalCreditCardModel.G2AadharNo = data.GuarantorDetailsList[1].AadharNo;
                        $scope.GopalCreditCardModel.G2PANCardNo = data.GuarantorDetailsList[1].PANCardNo;
                        $scope.GopalCreditCardModel.G2MobileNo = data.GuarantorDetailsList[1].MobileNo;
                        $scope.GopalCreditCardModel.G2Address = data.GuarantorDetailsList[1].Address;
                        $scope.GopalCreditCardModel.G2GuarranterLandProperty = data.GuarantorDetailsList[1].GuarranterLandProperty;
                        $scope.GopalCreditCardModel.G2Photo = data.GuarantorDetailsList[1].Photo;
                        $scope.GopalCreditCardModel.G2Signature = data.GuarantorDetailsList[1].Signature;
                    }

                    //$scope.GopalCreditCardModel.MemberPhoto_Base64 = data.MemberPhoto;
                    $scope.GGG_Id = data.gcc_id;
                    $scope.RuralUrbanBlock(1);
                    $scope.GramPanchayatList();
                    $scope.VillageList();
                    // $scope.LoanBankList();
                    //$scope.Get_LoanBankBranch();
                    //$scope.Get_LoanBranch_PACS();
                    //$scope.CurrentLoanBankList();
                    //$scope.Get_CurrentLoanBankBranch('0');
                    $scope.GopalCreditCardModel.CurrentLoanBranchId = '' + $scope.GopalCreditCardModel.CurrentLoanBranchId;
                    //document.getElementById('DistId').setAttribute('disabled', 'disabled');
                    //document.getElementById('BranchId').setAttribute('disabled', 'disabled');
                    //document.getElementById('BankID').setAttribute('disabled', 'disabled');
                    //document.getElementById('PacsId').setAttribute('disabled', 'disabled');

                    $scope.RecommendationOfDCSDOC_Path = $scope.GopalCreditCardModel.RecommendationOfDCSDOC_Path ?? '';
                    $scope.DocumentOfProposedWork_Path = $scope.GopalCreditCardModel.DocumentOfProposedWork_Path ?? '';
                    $scope.CopyOfDCSPassbook_Path = $scope.GopalCreditCardModel.CopyOfDCSPassbook_Path ?? '';
                    $scope.AadharDoc = $scope.GopalCreditCardModel.AadharDoc ?? '';
                    $scope.PanCardDoc = $scope.GopalCreditCardModel.PanCardDoc ?? '';
                    $scope.G1Photo = $scope.GopalCreditCardModel.G1Photo ?? '';
                    $scope.G2Photo = $scope.GopalCreditCardModel.G2Photo ?? '';
                    $scope.G1Signature = $scope.GopalCreditCardModel.G1Signature ?? '';
                    $scope.G2Signature = $scope.GopalCreditCardModel.G2Signature ?? '';
                    $scope.AgriculturalPropertyDoc = $scope.GopalCreditCardModel.AgriculturalPropertyDoc ?? '';
                    $scope.OtherPropertyDoc = $scope.GopalCreditCardModel.OtherPropertyDoc ?? '';


                    $scope.GopalCreditCardModel.Mn_ClusterId = $scope.GopalCreditCardModel.Mn_ClusterId.toString();
                    $scope.GopalCreditCardModel.Mn_VoID = $scope.GopalCreditCardModel.Mn_VoID.toString();
                    $scope.GopalCreditCardModel.Mn_ShgId = $scope.GopalCreditCardModel.Mn_ShgId.toString();
                    $scope.GopalCreditCardModel.Mn_BlockID = $scope.GopalCreditCardModel.Mn_BlockID.toString();
                    $scope.GopalCreditCardModel.Mn_DistrictId = $scope.GopalCreditCardModel.Mn_DistrictId.toString();

                    $scope.GetShgBlockList();
                    $scope.GetShgCLFList();
                    $scope.GetShgVoList();
                    $scope.GetShgList();


                    //   $scope.GopalCreditCardModel.IsPACSLAMPSMember = $scope.GopalCreditCardModel.IsPACSLAMPSMember == true ? 1 : 0


                    $scope.ShowGopalCreditCardForm = true;
                    $scope.divTabulation = false;
                    $scope.ShowJanAadharOTPValidation = false;


                    DecryptDECValue();
                }).
                error(function (data, status) {
                    $scope.DistrictList = [];
                });
        }
    };

    function DecryptDECValue() {


        const keysUsingDEC = [];

        // Get all elements with an ng-value attribute
        const elements = document.querySelectorAll('[ng-value]');

        // Iterate over elements to find | DEC usage
        elements.forEach(element => {
            const ngValue = element.getAttribute('ng-value');
            const match = /GopalCreditCardModel\.([a-zA-Z0-9_]+)\s*\|\s*DEC/.exec(ngValue);
            if (match) {
                keysUsingDEC.push(match[1]); // Extract the key name
            }
        });

        //const keysUsingDEC = ['AadharNo', 'PANCardNo', 'AnotherField'];

        angular.forEach(keysUsingDEC, function (key) {
            if ($scope.GopalCreditCardModel[key]) {
                $scope.GopalCreditCardModel[key] = decryptData($scope.GopalCreditCardModel[key]);
                // $scope.GopalCreditCardModel[key] = $scope.maskData(decryptData($scope.GopalCreditCardModel[key]), 4);

            }
        });
    }


    //$scope.maskData = function (value, visibleDigits) {
    //    if (value && value.length > visibleDigits) {
    //        const masked = 'X'.repeat(value.length - visibleDigits) + value.slice(-visibleDigits);
    //        return masked;
    //    }
    //    return value;  // Return value if it's shorter than the visible digits
    //};
    function AllpanelDisable() {

        document.querySelectorAll('input').forEach(input => {
            input.disabled = true;
        });
        document.querySelectorAll('select').forEach(select => {
            select.disabled = true;
        });
        document.querySelectorAll('i').forEach(icon => {
            icon.style.pointerEvents = 'none'; // Prevents clicks or any other interactions
            icon.style.opacity = '0.5';        // Visually indicates it's disabled
            icon.style.cursor = 'not-allowed'; // Changes the cursor to indicate it's not interactable
        });
    }
    function EnablePanel(div_name) {
        var inputs = "";
        const div = document.getElementById(div_name);

        if (div_name == 'PersonalDetail') {

            inputs = $('#' + div_name + ' input:not([name="PashupalakName"],[name="PashupalakFName"],[name="PashupalakDOB"],[type="file"])');
        }
        else {
            inputs = $('#' + div_name + ' input:not([type="file"])');
        }

        const selects = $('#' + div_name + ' select');
        const seli = $('#' + div_name + ' i');

        $(inputs).each(function (k, v) {
            $(v).prop("disabled", false);
        });

        $(selects).each(function (k, v) {
            $(v).prop("disabled", false);
        });
        $(seli).each(function (k, v) {
            this.style.pointerEvents = 'auto'; // Restores default interaction
            this.style.opacity = '1';          // Restores full opacity
            this.style.cursor = 'default';     // Changes cursor back to normal
        });

    }

    function EnableDoc(id) {

        $('#' + id).prop('disabled', false);

        $('#' + id).val('');

        // Access the AngularJS scope
        const scope = angular.element(document.body).scope();

        // Find the input element
        const inputElement = document.getElementById(id);

        // Retrieve the AngularJS model controller
        const ngModelController = angular.element(inputElement).data('$ngModelController');

        if (ngModelController) {
            // Clear the model value
            ngModelController.$setViewValue(null);
            ngModelController.$render();
        }

    }



    $scope.AgricultureLandDetailList = [];
    var cIndex = 0;

    //$scope.PushAgricultureLandDetailList = function () {

    //    $scope.AgricultureLandDetailList.push({
    //        cIndex: 0,
    //        id: 0,
    //        Village: '',
    //        PatwarCircle: '',
    //        Irrigated: 0,
    //        NonIrrigated: 0,
    //        KhasraNumber: '',
    //        Totalland: 0,
    //        // AgricultureLandImg:'',
    //        IsDelete: false

    //    });

    //}
    //$scope.PushAgricultureLandDetailList();

    $scope.CropLoanDetailsList = [];
    var cIndex = 0;
    $scope.PushCropLoanDetailsList = function () {
        $scope.CropLoanDetailsList.push({
            cIndex: 0,
            id: 0,
            financeInstituteName: '',
            NOD: 0,
            OD: 0,
            IsDelete: false

        });

    }

    $scope.PushCropLoanDetailsList();





    $scope.AddMoreAg = function () {
        var cIndex = $scope.CropLoanDetailsList.length;
        $scope.PushCropLoanDetailsList();
    }

    $scope.AddMoreALand = function () {
        var cIndex = $scope.AgricultureLandDetailList.length;
        $scope.PushAgricultureLandDetailList();
    }
    $scope.removeAgItem = function (cIndex) {
        for (var i = 0; i < $scope.CropLoanDetailsList.length; i++) {
            if ($scope.CropLoanDetailsList[i].cIndex === cIndex) {
                $scope.CropLoanDetailsList[i].IsDelete = true;
                break;
            }
        }
        cIndex = 0;
        angular.forEach($scope.CropLoanDetailsList, function (item) {
            item.cIndex = cIndex;
            cIndex++;
        });
        cIndex = 0;
    }

    $scope.removeAlItem = function (cIndex) {
        for (var i = 0; i < $scope.AgricultureLandDetailList.length; i++) {
            if ($scope.AgricultureLandDetailList[i].cIndex === cIndex) {
                $scope.AgricultureLandDetailList[i].IsDelete = true;
                break;
            }
        }
        cIndex = 0;
        angular.forEach($scope.AgricultureLandDetailList, function (item) {
            item.cIndex = cIndex;
            cIndex++;
        });
        cIndex = 0;
    }


    //Get Category List for dropdown
    $scope.GetCategoryList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=-1").
            success(function (data, status) {
                $scope.CategoryList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });
    }

    function setTwoNumberDecimal(event) {
        this.value = parseFloat(this.value).toFixed(2);
    }


    function isNullOrEmpty(value) {
        return value === null || value === undefined || value === '';
    }

    $scope.Get_GccDocuments = function () {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/GopalCreditCard/GetGccDoc/?GccId=" + $scope.GGG_Id).
            success(function (data, status) {
                //console.log(data);
                $scope.GccDocuments = data.Table.filter(x => x.Type.trim() == 'D' && x.status == 'O');
                $scope.GccObjectionPanel = data.Table.filter(x => x.Type.trim() == 'P' && x.status == 'O');
                $(".se-pre-con").fadeOut("slow");


                console.log($scope.GccObjectionPanel);
                console.log($scope.GccDocuments);

                AllpanelDisable();

                $scope.GccObjectionPanel.forEach((panel, i) => {
                    EnablePanel(panel.FieldShortName);

                });
                $scope.GccDocuments.forEach((panel, i) => {
                    if (!isNullOrEmpty(panel.FieldShortName))
                        EnableDoc(panel.FieldShortName);
                });

            }).
            error(function (data, status) {
                $scope.GccDocuments = [];
                $scope.GccObjectionPanel = [];

                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.KioskUserType = $('#hdnUserType').val();
    $scope.GetFeeVerification13851 = function () {
        $(".blockUI").show();
        // return $http.post("/GopalCreditCard/GetEmitraTransaction", JSON.stringify({ SearchText: $scope.GopalCreditCardModel.RegistrationNo, ServiceType: "9828", UserType: "K" })).
        return $http.post("/GopalCreditCard/GetEmitraTransaction", JSON.stringify({ SearchText: $scope.GopalCreditCardModel.RegistrationNo, ServiceType: "13851", UserType: "K" })).
            success(function (result, status) {
                $(".blockUI").hide();
                var Msg = '';
                Msg += '\n <br /> STATUS: ' + (result.MSG == undefined ? result.STATUS : result.MSG);
                Msg += '\n <br /> RECEIPTNO: ' + result.RECEIPTNO;
                Msg += '\n <br /> DATE: ' + result.TRANSACTIONDATE;
                Msg += '\n <br /> AMOUNT: ' + (result.AMT == undefined ? result.PAIDAMOUNT : result.AMT);
                Msg += '\n <br /> TRANSACTIONSTATUS: ' + (result.TRANSACTIONSTATUS == undefined ? result.RESPONSEMESSAGE : result.TRANSACTIONSTATUS);
                ShowOkMessage("Transaction Status", Msg, function () {
                    window.location.href = window.location.href;
                });
                //$scope.PrintShow = true;
                //$scope.FailedTransactionList = [];           
            }).
            error(function (data, status) {
                $(".blockUI").hide();
                $scope.FailedTransactionList = [];
                alert('Error while fetching failed transactionList data.');
            });

    }


    $scope.decryptData = function (input) {

        return decryptData(input);
    }


    //shg


    $scope.GetDistrictList = function () {
        $(".se-pre-con").fadeIn("slow");

        // $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MN_DISTRICT&MasterCode=0").
        $http.get(SiteUrl + "/RajeevikaMaster/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.ShgDistrictList = data;
                //  $scope.GopalCreditCardModel.Mn_DistrictId = '0';
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.ShgDistrictList = [];
                $(".se-pre-con").fadeOut("slow");

            });
    };

    $scope.GetShgBlockList = function (id) {

        if (id == '0')
        {

            $scope.GopalCreditCardModel.Mn_BlockID = '0';
            $scope.GopalCreditCardModel.Mn_ClusterId = '0';
            $scope.GopalCreditCardModel.Mn_VoID = '0';
            $scope.GopalCreditCardModel.Mn_ShgId = '0';
        }

        $(".se-pre-con").fadeIn("slow");
        //  $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MN_BLOCK&MasterCode=" + $scope.GopalCreditCardModel.Mn_DistrictId ?? 0).
        $http.get(SiteUrl + "/RajeevikaMaster/GetMasterData/?MasterType=BLOCK&MasterCode=" + $scope.GopalCreditCardModel.Mn_DistrictId ?? 0).
            success(function (data, status) {
                $scope.ShgBlockList = data;             
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.ShgBlockList = [];
                $(".se-pre-con").fadeOut("slow");

            });
    };
    $scope.GetShgCLFList = function (id) {
        $(".se-pre-con").fadeIn("slow");
        if (id == '0') {            
            $scope.GopalCreditCardModel.Mn_ClusterId = '0';
            $scope.GopalCreditCardModel.Mn_VoID = '0';
            $scope.GopalCreditCardModel.Mn_ShgId = '0';
        }

        $http.get(SiteUrl + "/RajeevikaMaster/GetMasterData/?MasterType=CLF&" + "MasterCode=" + ($scope.GopalCreditCardModel.Mn_DistrictId ?? 0) + "&MasterCode2=" + ($scope.GopalCreditCardModel.Mn_BlockID ?? 0)).
            success(function (data, status) {
                $scope.CLFList = data;                
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.CLFList = [];
                $(".se-pre-con").fadeOut("slow");

            });
    };
    $scope.GetShgVoList = function (id) {
        $(".se-pre-con").fadeIn("slow");
        if (id == '0')
        {
            $scope.GopalCreditCardModel.Mn_VoID = '0';
            $scope.GopalCreditCardModel.Mn_ShgId = '0';
        }

        $http.get(SiteUrl + "/RajeevikaMaster/GetMasterData/?MasterType=VO&MasterCode=" + $scope.GopalCreditCardModel.Mn_ClusterId ?? 0).
            success(function (data, status) {
                $scope.VoList = data;              
                $(".se-pre-con").fadeOut("slow");

            }).
            error(function (data, status) {
                $scope.VoList = [];
                $(".se-pre-con").fadeOut("slow");

            });
    };
    $scope.GetShgList = function (id) {
        $(".se-pre-con").fadeIn("slow");
        if (id == '0') {
            $scope.GopalCreditCardModel.Mn_ShgId = '0';
        }

        $http.get(SiteUrl + "/RajeevikaMaster/GetMasterData/?MasterType=SHG&MasterCode=" + $scope.GopalCreditCardModel.Mn_VoID ?? 0).
            success(function (data, status) {
                $scope.SHGList = data;                
                $(".se-pre-con").fadeOut("slow");

            }).
            error(function (data, status) {
                $scope.SHGList = [];
                $(".se-pre-con").fadeOut("slow");

            });
    };



    //$scope.CheckShgMembersExist = function () {
    //    $(".se-pre-con").fadeIn("slow");
    //    debugger;
    //    if ($scope.GopalCreditCardModel.AadharNo == '' || $scope.JanAadharNo == '') {
    //        alert($scope._language == 'en-us' ? '\nPlease Enter AADHAR No' : '\nकृपया आधार नंबर दर्ज करें|');
    //        $scope.GopalCreditCardModel.IsMahilaNidhiMember = true;
    //        $(".se-pre-con").fadeOut("slow");
    //        return;
    //    }

    //    $http.get(SiteUrl + "/RajeevikaMaster/CheckShgMembersExist/?JanAadharNo=" + encodeURIComponent(encryptData($scope.JanAadharNo)) + "&AadharNo=" + encodeURIComponent(encryptData($scope.GopalCreditCardModel.AadharNo)) ?? '').
    //        success(function (data, status) {
    //            if (data != null && data[0].IsExist == "1") {
    //                $scope.IsMahilaNidhiMember = true;
    //            }
    //            else {
    //                alert($scope._language == 'en-us' ? '\nYou are not eligible for Mahila Nidhi as you are not a member of Mahila Nidhi.' : '\nआप महिला निधि के लिए पात्र नहीं हैं क्योंकि आप महिला निधि की सदस्य नहीं हैं|');
    //                $scope.IsMahilaNidhiMember = false;
    //                $scope.GopalCreditCardModel.IsMahilaNidhiMember = true;
    //                $scope.MahilaNidhiMember(0);
    //            }

    //            $(".se-pre-con").fadeOut("slow");

    //        }).
    //        error(function (data, status) {
    //            $scope.GopalCreditCardModel.IsMahilaNidhiMember = false;
    //            $scope.IsMahilaNidhiMember = false;
    //            $scope.MahilaNidhiMember(0);
    //            $(".se-pre-con").fadeOut("slow");

    //        });
    //};


    $scope.MahilaNidhiMember = function (val) {

        if (val == 0) {
            $scope.GopalCreditCardModel.Mn_ClusterId = '0';
            $scope.GopalCreditCardModel.Mn_VoID = '0';
            $scope.GopalCreditCardModel.Mn_ShgId = '0';
            $scope.GopalCreditCardModel.Mn_BlockID = '0';
            $scope.GopalCreditCardModel.Mn_DistrictId = '0';
        }
        else if (val == 1) {
            $scope.CheckShgMembersExist();
        }

        if (val == 0) {

        }
        else {

        }
    };



    $scope.ShgDataView = function () {
        debugger;
        if (
            !$scope.ShgDistrictList?.length ||
            !$scope.ShgBlockList?.length ||
            !$scope.CLFList?.length ||
            !$scope.VoList?.length ||
            !$scope.SHGList?.length
        ) {
            return; // Exit if any of the lists is undefined or empty
        }
        const district = $scope.ShgDistrictList.find(item => item.DISTRICT_ID == $scope.GopalCreditCardModel.Mn_DistrictId);
        $scope.GopalCreditCardModel.Mn_DistrictName = district ? district.DISTRICT_ENG : '';
        //$scope.GopalCreditCardModel.ShgDistrictHi = district ? district.DISTRICT_ENG : "";

        const block = $scope.ShgBlockList.find(item => item.BLOCK_ID == $scope.GopalCreditCardModel.Mn_BlockID);
        $scope.GopalCreditCardModel.Mn_BlockName = block ? block.BLOCK_ENG : "";
       // $scope.GopalCreditCardModel.ShgBlockHi = block ? block.BLOCK_MANGAL : "";

        const CLF = $scope.CLFList.find(item => item.ClfId == $scope.GopalCreditCardModel.Mn_ClusterId);
        $scope.GopalCreditCardModel.Mn_ClusterName = CLF ? CLF.ClfName : "";
       // $scope.GopalCreditCardModel.ShgClfHi = CLF ? CLF.ClfNameHi : "";

        const VO = $scope.VoList.find(item => item.VoId == $scope.GopalCreditCardModel.Mn_VoID);
        $scope.GopalCreditCardModel.Mn_VoName = VO ? VO.VoName : "";
      //  $scope.GopalCreditCardModel.ShgVoHi = VO ? VO.VoNameHi : "";

        const SHG = $scope.SHGList.find(item => item.ShgId == $scope.GopalCreditCardModel.Mn_ShgId);
        $scope.GopalCreditCardModel.Mn_ShgName = SHG ? SHG.ShgName : "";
       // $scope.GopalCreditCardModel.ShgHi = SHG ? SHG.ShgNameHi : "";
    }
    $scope.$watchGroup([
        'ShgDistrictList',
        'ShgBlockList',
        'CLFList',
        'VoList',
        'SHGList'
    ], function (newValues, oldValues) {
        if (newValues.every(list => list && list.length)) {
            $scope.ShgDataView(); // Call the function when all lists have values
        }
    });




    //$scope.$watchGroup([
    //    'ShgDistrictList',
    //    'ShgBlockList',
    //    'CLFList',
    //    'VoList',
    //    'SHGList'
    //], function (newValues, oldValues) {
    //    if (newValues.every(list => list && list.length)) {
    //        $scope.ShgDataView(); // Call the function when all lists have values
    //    }
    //});


    $scope.OnPageLoad();
});
AppTrainning.filter('DEC', function () {
    //debugger;
    return function (input) {
        if (!input)
            return input;
        return decryptData(input);
    };
});
