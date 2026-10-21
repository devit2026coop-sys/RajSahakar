var ArrSelectTraningUser = new Array();

function TraningUserSelected() {
    this.UserId = '0';
}

var AppTrainning = angular.module("GopalCreditCardAppEntryForm", ['angularUtils.directives.dirPagination']);
AppTrainning.controller("GopalCreditCard", function ($scope, $http, $location) {
    // Save Sms Template 
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivSms = true;
    $scope.newdiv = "";
    $scope.Trainning = '';
    // on page load


    const urlParams = new URLSearchParams(window.location.search);
    $scope.OnPageLoad = function () {
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

        $scope.ImageValidationMessage = '';
        $scope.FileCollection = [];



        // Initialise Main form control
        $scope.GopalCreditCardModel = {}

        $scope.GopalCreditCardModel.LoanDistrictId = '0';
        $scope.GopalCreditCardModel.LoanBankID = "0";
        $scope.GopalCreditCardModel.LoanBranchId = "0";
        $scope.GopalCreditCardModel.PACS_ID = "0";

        $scope.GopalCreditCardModel.ShgClfID = '0';
        $scope.GopalCreditCardModel.ShgVoID = '0';
        $scope.GopalCreditCardModel.ShgId = '0';
        $scope.GopalCreditCardModel.ShgBlockID = '0';
        $scope.GopalCreditCardModel.ShgDistrictId = '0';

        $scope.GopalCreditCardModel.IsWorkingChild = '';
        $scope.IsWorkingChildList = [];
        $scope.GopalCreditCardModel.IsInfrastructureChild = '';
        $scope.IsInfrastructureChildList = [];
        $scope.GopalCreditCardModel.IsApparatusChild = '';
        $scope.IsApparatusChildList = [];

        $scope.workingMapping = { Fodder: 1, Water: 2, Medicines: 3, Insecticides: 4 };
        $scope.infrastructureMapping = { Shed: 1, Kheli: 2, DairyMattresses: 3 };
        $scope.apparatusMapping = { MilkApparatus: 1 };

        $scope.GetDistrictWithoutDivisionList();

        $scope.GCCEId = urlParams.get('eId');
        $scope.GCCERNO = urlParams.get('eRno');

        //Get Category List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=-1").
            success(function (data, status) {
                $scope.CategoryList = data;
                $scope.GetFormViewData();
            }).
            error(function (data, status) {
                alert("Error while fetching Post list");
                $scope.GetFormViewData();

            });
    };

    $scope.openPopupUdAadharV = function () {
        //alert('test');
        $('#modelPopupUdAadharV').modal();
    }
    $scope.updateAadharDataV = function (AadharNo) {

        $(".se-pre-con").fadeIn("slow");
        $http.post("/GopalCreditCard/UpdateAadharData?aadharNo=" + AadharNo + "&GCCEId=" + $scope.GopalCreditCardModel.gcc_id).
            success(function (data, status) {
                $('#modelPopupUdAadharV').modal("hide");
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
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
        if (SelectedVal == true || SelectedVal == true) {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
            $scope.BlockListByOffAdd(1);
            $scope.GramPanchayatList();
            $scope.VillageList();

        }
        else if (SelectedVal == false || SelectedVal == false) {
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;
            $scope.GetCityList();
            $scope.GetWardList();

        }
        else {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
        }
    };
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
            $scope.JanMemberList = '';
            $http.post(SiteUrl + "/GopalCreditCard/GetJanAadhaarMembersList?JanAadharNo=" + $scope.JanAadharNo).
                success(function (data, status) {
                    if (data.length > 0) {
                        $scope.JanMemberList = data;
                    }
                }).
                error(function (data, status) {
                    alert("Error while fetching Year Target Data");
                });
        }
    }
    $scope.CurrentLoanBankList = function () {
        $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=" + $scope.GopalCreditCardModel.CurrentLoanDistrictId).
            success(function (data, status) {

                $scope.CurrentBankMasterList = data;
            }).
            error(function (data, status) {
                $scope.CurrentBankMasterList = [];
            });
    };
    $scope.Get_CurrentLoanBankBranch = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBankBranch/?LoanDistrictId=" + $scope.GopalCreditCardModel.CurrentLoanDistrictId + "&LoanBankId=" + $scope.GopalCreditCardModel.CurrentLoanBankID).
            success(function (data, status) {
                $scope.CurrentPLDBBranches = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.CurrentPLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.GetFormViewData = function () {
        /*$http.post(SiteUrl + "/GopalCreditCard/Get_GopalApplicationFormViewData/?eid=0").*/
        //  $http.post(SiteUrl + "/GopalCreditCard/Get_GopalApplicationFormViewData1/?GCCEId=" + encodeURIComponent($scope.GCCEId) + "&GCCERNO=" + encodeURIComponent($scope.GCCERNO)).
        $http.post(SiteUrl + "/GopalCreditCard/Get_GopalApplicationFormViewData_v1/?GCCEId=" + $scope.GCCEId).
            success(function (data, status) {
                $scope.GopalCreditCardModel = data;
                $scope.GopalCreditCardModel.IsCopyOfDCSPassbookAttached = (data.CopyOfDCSPassbook_Path != '' || data.PBBankAccountNo != '') ? true : false;
                $scope.GopalCreditCardModel.IsPACSBankAccountAvailable = (data.BankAccountNo != '' || data.BankIFSCCode || data.BankName || data.BankBranchName) ? true : false;
                $scope.CropLoanDetailsList = data.CropLoanDetailsList;
                $scope.AgricultureLandDetailList = data.AgricultureLandDetailList;
                if (data.GuarantorDetailsList[0] != null) {
                    $scope.GopalCreditCardModel.G1Name = data.GuarantorDetailsList[0].Name;
                    $scope.GopalCreditCardModel.G1Gender = data.GuarantorDetailsList[0].Gender;
                    $scope.GopalCreditCardModel.G1FatherSpouseName = data.GuarantorDetailsList[0].FatherSpouseName;
                    $scope.GopalCreditCardModel.G1DOB = data.GuarantorDetailsList[0].DOB != null ? new Date(data.GuarantorDetailsList[0].DOB) : null;
                    $scope.GopalCreditCardModel.G1JanAadharNumber = data.GuarantorDetailsList[0].JanAadharNumber;
                    $scope.GopalCreditCardModel.G1JanMemberId = data.GuarantorDetailsList[0].JanMemberId;
                    $scope.GopalCreditCardModel.G1AadharNo = data.GuarantorDetailsList[0].AadharNo;
                    $scope.GopalCreditCardModel.G1PANCardNo = data.GuarantorDetailsList[0].PANCardNo;
                    $scope.GopalCreditCardModel.G1MobileNo = data.GuarantorDetailsList[0].MobileNo;
                    $scope.GopalCreditCardModel.G1Address = data.GuarantorDetailsList[0].Address;
                    $scope.GopalCreditCardModel.G1GuarranterLandProperty = data.GuarantorDetailsList[0].GuarranterLandProperty;
                    $scope.GopalCreditCardModel.G1Photo = data.GuarantorDetailsList[0].Photo;
                    $scope.GopalCreditCardModel.G1Signature = data.GuarantorDetailsList[0].Signature;
                    $scope.GopalCreditCardModel.G1AadharNoMasked = data.GuarantorDetailsList[0].AadharNoMasked;
                    $scope.G1DetailFetched = data.GuarantorDetailsList[0].IsVerified;
                }
                if (data.GuarantorDetailsList[1] != null) {
                    $scope.GopalCreditCardModel.G2Name = data.GuarantorDetailsList[1].Name;
                    $scope.GopalCreditCardModel.G2Gender = data.GuarantorDetailsList[1].Gender;
                    $scope.GopalCreditCardModel.G2FatherSpouseName = data.GuarantorDetailsList[1].FatherSpouseName;
                    $scope.GopalCreditCardModel.G2DOB = data.GuarantorDetailsList[1].DOB != null ? new Date(data.GuarantorDetailsList[1].DOB) : null;
                    $scope.GopalCreditCardModel.G2JanAadharNumber = data.GuarantorDetailsList[1].JanAadharNumber;
                    $scope.GopalCreditCardModel.G2JanMemberId = data.GuarantorDetailsList[1].JanMemberId;
                    $scope.GopalCreditCardModel.G2AadharNo = data.GuarantorDetailsList[1].AadharNo;
                    $scope.GopalCreditCardModel.G2PANCardNo = data.GuarantorDetailsList[1].PANCardNo;
                    $scope.GopalCreditCardModel.G2MobileNo = data.GuarantorDetailsList[1].MobileNo;
                    $scope.GopalCreditCardModel.G2Address = data.GuarantorDetailsList[1].Address;
                    $scope.GopalCreditCardModel.G2GuarranterLandProperty = data.GuarantorDetailsList[1].GuarranterLandProperty;
                    $scope.GopalCreditCardModel.G2Photo = data.GuarantorDetailsList[1].Photo;
                    $scope.GopalCreditCardModel.G2Signature = data.GuarantorDetailsList[1].Signature;
                    $scope.GopalCreditCardModel.G2AadharNoMasked = data.GuarantorDetailsList[1].AadharNoMasked;
                    $scope.G2DetailFetched = data.GuarantorDetailsList[1].IsVerified;
                }

                $scope.GopalCreditCardModel.MemberPhoto_Base64 = data.MemberPhoto;
                $scope.GGG_Id = data.gcc_id;

                //Added new
                if ($scope.GopalCreditCardModel.IsWorking && $scope.GopalCreditCardModel.IsWorkingChild != null) {
                    $scope.IsWorkingChild = $scope.GopalCreditCardModel.IsWorkingChild.split(',').map(Number);
                    angular.forEach($scope.workingMapping, function (value, key) {
                        $scope.GopalCreditCardModel[key] = $scope.IsWorkingChild.includes(value);
                    });
                }

                if ($scope.GopalCreditCardModel.IsInfrastructure && $scope.GopalCreditCardModel.IsInfrastructureChild != null) {
                    $scope.IsInfrastructureChild = $scope.GopalCreditCardModel.IsInfrastructureChild.split(',').map(Number);
                    angular.forEach($scope.infrastructureMapping, function (value, key) {
                        $scope.GopalCreditCardModel[key] = $scope.IsInfrastructureChild.includes(value);
                    });
                }

                if ($scope.GopalCreditCardModel.IsApparatus && $scope.GopalCreditCardModel.IsApparatusChild != null) {
                    $scope.IsApparatusChild = $scope.GopalCreditCardModel.IsApparatusChild.split(',').map(Number);
                    angular.forEach($scope.apparatusMapping, function (value, key) {
                        $scope.GopalCreditCardModel[key] = $scope.IsApparatusChild.includes(value);
                    });
                }

                $scope.RuralUrbanBlock();
                $scope.GramPanchayatList();
                $scope.VillageList();
                $scope.LoanBankList();
                $scope.Get_LoanBankBranch();
                $scope.Get_LoanBranch_PACS();
                $scope.CurrentLoanBankList();
                $scope.Get_CurrentLoanBankBranch('0');

                if ($scope.GopalCreditCardModel.ShgId != 0) {
                    $scope.GetDistrictList();
                    $scope.GetShgBlockList();
                    $scope.GetShgCLFList();
                    $scope.GetShgVoList();
                    $scope.GetShgList();

                    $scope.GopalCreditCardModel.IsMahilaNidhiMember = true;
                    $scope.GopalCreditCardModel.ShgDistrictId = '' + $scope.GopalCreditCardModel.ShgDistrictId;
                    $scope.GopalCreditCardModel.ShgBlockID = '' + $scope.GopalCreditCardModel.ShgBlockID;
                    $scope.GopalCreditCardModel.ShgClfID = '' + $scope.GopalCreditCardModel.ShgClfID;
                    $scope.GopalCreditCardModel.ShgVoID = '' + $scope.GopalCreditCardModel.ShgVoID;
                    $scope.GopalCreditCardModel.ShgId = '' + $scope.GopalCreditCardModel.ShgId;



                } else {
                    $scope.GopalCreditCardModel.IsMahilaNidhiMember = false;
                }

                $scope.GopalCreditCardModel.CurrentLoanDistrictId = '' + $scope.GopalCreditCardModel.CurrentLoanDistrictId;
                $scope.GopalCreditCardModel.CurrentLoanBankID = '' + $scope.GopalCreditCardModel.CurrentLoanBankID;
                $scope.GopalCreditCardModel.CurrentLoanBranchId = '' + $scope.GopalCreditCardModel.CurrentLoanBranchId;


                $scope.IsMemberOfDCS_Changed();
                $scope.IsPACSLAMPSMember_Changed();
                $scope.DoesTheMilkSaleToDCS_Changed();
                $scope.GGG_Id = data.gcc_id;
                $scope.GetApplicationFlowGCC();
                //    $scope.GetApplicationDetails();
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
                $scope.Get_GccOtherDocuments();


                DecryptDECValue();

            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    function DecryptDECValue() {


        const keysUsingDEC = [];

        // Get all elements with the class 'decryptedData'
        const elements = document.querySelectorAll('.decryptedData');


        elements.forEach(element => {
            const modelName = element.getAttribute('ng-model');
            if (modelName) {
                const match = /GopalCreditCardModel\.([a-zA-Z0-9_]+)/.exec(modelName);
                if (match) {
                    keysUsingDEC.push(match[1]); // Add the key to the array
                }
            }
        });
        angular.forEach(keysUsingDEC, function (key) {
            if ($scope.GopalCreditCardModel[key]) {
                $scope.GopalCreditCardModel[key] = $scope.maskData(decryptData($scope.GopalCreditCardModel[key]), 4);
            }
        });
    }
    $scope.maskData = function (value, visibleDigits) {
        if (value && value.length > visibleDigits) {
            const masked = 'X'.repeat(value.length - visibleDigits) + value.slice(-visibleDigits);
            return masked;
        }
        return value;  // Return value if it's shorter than the visible digits
    };



    $scope.DoesTheMilkSaleToDCS_Changed = function (value) {

        var SelectedVal = $scope.GopalCreditCardModel.DoesTheMilkSaleToDCS == 'false' ? false : true;
        if (SelectedVal == true || SelectedVal == true) {
            $scope.GopalCreditCardModel.DoesTheMilkSaleToDCS_Status = true;
        }
        else if (SelectedVal == false || SelectedVal == false) {
            $scope.GopalCreditCardModel.DoesTheMilkSaleToDCS_Status = false;
            $scope.GopalCreditCardModel.EstimatedAmountOfMilkSoldToDCS = "";
        }
        else {
            $scope.GopalCreditCardModel.DoesTheMilkSaleToDCS = "";
            $scope.GopalCreditCardModel.DoesTheMilkSaleToDCS_Status = false;
            $scope.GopalCreditCardModel.EstimatedAmountOfMilkSoldToDCS = "";
        }
    };
    $scope.IsPACSLAMPSMember_Changed = function (value) {
        var SelectedVal = $scope.GopalCreditCardModel.IsPACSLAMPSMember == 'false' ? false : true;
        if (SelectedVal == true || SelectedVal == true) {
            $scope.GopalCreditCardModel.IsPACSLAMPSMemberCheckedStatus = true;
        }
        else if (SelectedVal == false || SelectedVal == false) {
            $scope.GopalCreditCardModel.IsPACSLAMPSMemberCheckedStatus = false;
        }
    };
    $scope.IsMemberOfDCS_Changed = function (value) {
        var SelectedVal = $scope.GopalCreditCardModel.IsMemberOfDCS == 'false' ? false : true;

        if (SelectedVal == true || SelectedVal == true) {
            $scope.GopalCreditCardModel.IsMemberOfDCSCheckedStatus = true;
        }
        else if (SelectedVal == false || SelectedVal == false) {
            $scope.GopalCreditCardModel.IsMemberOfDCSCheckedStatus = false;
        }
        else {
            $scope.GopalCreditCardModel.IsMemberOfDCS = false;
            $scope.GopalCreditCardModel.IsMemberOfDCSCheckedStatus = false;
        }
    };
    $scope.LoanBankList = function () {

        $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=" + $scope.GopalCreditCardModel.LoanDistrictId).
            success(function (data, status) {
                $scope.BankMasterList = data;
            }).
            error(function (data, status) {
                $scope.BankMasterList = [];
            });
    };

    $scope.Get_LoanBankBranch = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBankBranch/?LoanDistrictId=" + $scope.GopalCreditCardModel.LoanDistrictId + "&LoanBankId=" + $scope.GopalCreditCardModel.LoanBankID).
            success(function (data, status) {
                $scope.PLDBBranches = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.Get_LoanBranch_PACS = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBranch_PACS/?LoanBranchId=" + $scope.GopalCreditCardModel.LoanBranchId).
            success(function (data, status) {
                $scope.PACSNameList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PACSNameList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.printDiv = function (divName) {
        var printContents = document.getElementById(divName).innerHTML;

        if (navigator.userAgent.toLowerCase().indexOf('chrome') > -1) {
            var popupWin = window.open('', '_blank', 'width=1000,height=600,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no');
            popupWin.window.focus();
            popupWin.document.write('<!DOCTYPE html><html><head>' +
                '<link rel="stylesheet" type="text/css" href="../Content/Stylesheets/Style.css" />' +
                '<style>table th{background-color:#cccccc!important;color:#424242!important}table td{background-color:#ffffff;}label{font-weight:normal}</style>' +
                '</head><body onload="window.print1()"><div class="reward-body">' + printContents + '</div></body></html>');
            popupWin.onbeforeunload = function (event) {
                popupWin.close();
                return '.\n';
            };
            popupWin.onabort = function (event) {
                popupWin.document.close();
                popupWin.close();
            }
        } else {
            var popupWin = window.open('', '_blank', 'width=800,height=600');
            popupWin.document.open();
            popupWin.document.write('<html><head><link rel="stylesheet" type="text/css" href="style.css" /></head><body onload="window.print()">' + printContents + '</body></html>');
            popupWin.document.close();
        }
        popupWin.document.close();

        return true;
    }
    $scope.BackToPage = function () {

        if ($scope.UserId == undefined || $scope.UserId == 60) {
            window.location = '../GopalCreditCard/GopalApplicationForm';
        }
        else {
            window.location = '../GopalCreditCard/ApplicationFlowGCC';
        }
        //window.history.back();

    }
    $scope.GetApplicationDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/GopalCreditCard/GetApplicationDetail?GccIdEnc=" + $scope.GopalCreditCardModel.GccIdDBEnc).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data != null) {
                    $scope.objGCC = data.Table[0];
                    if (data.Table1[0] != null)
                        $scope.gcc = data.Table1[0];
                    $scope.gcc.GopalCreditCardIdEnc = $scope.GccIdEnc;
                    $scope.gcc.TypesOfProperty = data.Table1[0].TypesOfProperty?.toString();
                    $scope.gcc.RegistrationNo = $scope.objGCC.RegistrationNo;
                    $scope.gcc.EarlierLoanSanctionDate = new Date($scope.gcc.EarlierLoanSanctionDate);
                }
            }).
            error(function () {
                alert("Error while fetching GCC application data.");
                $(".se-pre-con").fadeOut("slow");
            });
    }


    $scope.GetApplicationFlowGCC = function () {
        $scope.showLoader = true;
        $http.get("../gopalCreditCard/GetApplicationFlowGCC/?GCC_Id=" + $scope.GGG_Id).
            success(function (data, status) {
                //$scope.ApplicationFlowGCC = data;

                $scope.ApplicationFlowGCC = JSON.parse(data.MainFlow);
                $scope.OldApplicationFlowGCC = JSON.parse(data.OldFlow);

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
            error(function (data1, status) {

            });
        $scope.showLoader = true;
    }



    $scope.DetailsCroploantaken = [];
    var cIndex = 0;
    $scope.DetailsCroploantaken.push({
        cIndex: 0,
        id: 0,
        financeInstituteName: '',
        NOD: 0,
        OD: 0,
        IsDelete: false

    });
    $scope.AgriculturalLandDetails = [];
    var cIndex = 0;
    $scope.AgriculturalLandDetails.push({
        cIndex: 0,
        id: 0,
        Village: '',
        PatwarCircle: '',
        Irrigated: 0,
        NonIrrigated: 0,
        KhasraNumber: '',
        Totalland: 0,
        // AgricultureLandImg:'',
        IsDelete: false

    });

    $scope.UserId = $('#hdnUserId').val();
    $scope.GccDocuments = [];
    $scope.GccOtherDocuments = [];

    $scope.Get_GccOtherDocuments = function () {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/GopalCreditCard/GetGccOtherDoc/?GccId=" + $scope.GGG_Id).
            success(function (data, status) {
                $scope.GccOtherDocuments = data.Table;

                if ($scope.GopalCreditCardModel.ApplicantSignedForm != null) {
                    $scope.GccOtherDocuments.push({
                        DocId: 7,
                        DocPath: $scope.GopalCreditCardModel.ApplicantSignedForm,
                        DocumentName_En: "Esign Citizen",
                        DocumentName_Hi: "ई-साइन सिटिजन",
                        DocumentURL: $scope.GopalCreditCardModel.ApplicantSignedForm.split('/').pop(),
                        GccId: $scope.GGG_Id
                    });
                }
                if ($scope.GopalCreditCardModel.ApplicantSignedFormGuarantor != null) {
                    $scope.GccOtherDocuments.push({
                        DocId: 9,
                        DocPath: $scope.GopalCreditCardModel.ApplicantSignedFormGuarantor,
                        DocumentName_En: "Esign Guarantor",
                        DocumentName_Hi: "ई-साइन गारंटर",
                        DocumentURL: $scope.GopalCreditCardModel.ApplicantSignedFormGuarantor.split('/').pop(),
                        GccId: $scope.GGG_Id
                    });
                }
                if ($scope.GopalCreditCardModel.ApplicantSignedFormPacs != null) {
                    $scope.GccOtherDocuments.push({
                        DocId: 8,
                        DocPath: $scope.GopalCreditCardModel.ApplicantSignedFormPacs,
                        DocumentName_En: "Esign Pacs",
                        DocumentName_Hi: "ई-साइन पैक्स",
                        DocumentURL: $scope.GopalCreditCardModel.ApplicantSignedFormPacs.split('/').pop(),
                        GccId: $scope.GGG_Id
                    });
                }
                //$scope.GccOtherDocuments = data.Table.filter(x => x.DocumentName_En == 'Sanction Letter' && x.DocPath != "")
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.GccOtherDocuments = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    //shg


    $scope.GetDistrictList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/RajeevikaMaster/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.ShgDistrictList = data;
                $(".se-pre-con").fadeOut("slow");


            }).
            error(function (data, status) {
                $scope.ShgDistrictList = [];
                $(".se-pre-con").fadeOut("slow");

            });
    };

    $scope.GetShgBlockList = function () {
        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/RajeevikaMaster/GetMasterData/?MasterType=BLOCK&MasterCode=" + $scope.GopalCreditCardModel.ShgDistrictId ?? 0).
            success(function (data, status) {
                $scope.ShgBlockList = data;
                $(".se-pre-con").fadeOut("slow");


            }).
            error(function (data, status) {
                $scope.ShgBlockList = [];
                $(".se-pre-con").fadeOut("slow");

            });
    };
    $scope.GetShgCLFList = function () {
        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/RajeevikaMaster/GetMasterData/?MasterType=CLF&MasterCode=" + $scope.GopalCreditCardModel.ShgBlockID ?? 0).
            success(function (data, status) {
                $scope.CLFList = data;
                $(".se-pre-con").fadeOut("slow");


            }).
            error(function (data, status) {
                $scope.CLFList = [];
                $(".se-pre-con").fadeOut("slow");

            });
    };
    $scope.GetShgVoList = function () {
        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/RajeevikaMaster/GetMasterData/?MasterType=VO&MasterCode=" + $scope.GopalCreditCardModel.ShgClfID ?? 0).
            success(function (data, status) {
                $scope.VoList = data;
                $(".se-pre-con").fadeOut("slow");

            }).
            error(function (data, status) {
                $scope.VoList = [];
                $(".se-pre-con").fadeOut("slow");

            });
    };
    $scope.GetShgList = function () {
        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/RajeevikaMaster/GetMasterData/?MasterType=SHG&MasterCode=" + $scope.GopalCreditCardModel.ShgVoID ?? 0).
            success(function (data, status) {
                $scope.SHGList = data;
                $(".se-pre-con").fadeOut("slow");

            }).
            error(function (data, status) {
                $scope.SHGList = [];
                $(".se-pre-con").fadeOut("slow");

            });
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
        const district = $scope.ShgDistrictList.find(item => item.DISTRICT_ID == $scope.GopalCreditCardModel.ShgDistrictId);
        $scope.GopalCreditCardModel.ShgDistrictEng = district ? district.DISTRICT_ENG : '';
        $scope.GopalCreditCardModel.ShgDistrictHi = district ? district.DISTRICT_ENG : "";

        const block = $scope.ShgBlockList.find(item => item.BLOCK_ID == $scope.GopalCreditCardModel.ShgBlockID);
        $scope.GopalCreditCardModel.ShgBlockEng = block ? block.BLOCK_ENG : "";
        $scope.GopalCreditCardModel.ShgBlockHi = block ? block.BLOCK_MANGAL : "";

        const CLF = $scope.CLFList.find(item => item.ClfId == $scope.GopalCreditCardModel.ShgClfID);
        $scope.GopalCreditCardModel.ShgClfEng = CLF ? CLF.ClfName : "";
        $scope.GopalCreditCardModel.ShgClfHi = CLF ? CLF.ClfNameHi : "";

        const VO = $scope.VoList.find(item => item.VoId == $scope.GopalCreditCardModel.ShgVoID);
        $scope.GopalCreditCardModel.ShgVoEng = VO ? VO.VoName : "";
        $scope.GopalCreditCardModel.ShgVoHi = VO ? VO.VoNameHi : "";

        const SHG = $scope.SHGList.find(item => item.ShgId == $scope.GopalCreditCardModel.ShgId);
        $scope.GopalCreditCardModel.ShgEng = SHG ? SHG.ShgName : "";
        $scope.GopalCreditCardModel.ShgHi = SHG ? SHG.ShgNameHi : "";
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

    $scope.OnPageLoad();
    $scope.DivtringGrid = true;
});