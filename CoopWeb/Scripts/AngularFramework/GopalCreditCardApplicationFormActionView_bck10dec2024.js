var ArrSelectTraningUser = new Array();

function TraningUserSelected() {
    this.UserId = '0';
}

var AppTrainning = angular.module("GopalCreditCardAppEntryForm", ['angularUtils.directives.dirPagination']);
AppTrainning.controller("GopalCreditCard", function ($scope, $location, $http) {
    // Save Sms Template 
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivSms = true;
    $scope.newdiv = "";
    $scope.Trainning = '';

    //madhuri
    $scope.showGccReportFile = false;
    $scope.ShowOTPDiv = false;
    $scope.ApplicationFlowMaster = {}
    $scope.GccApplicationFlowModel = {}

    $scope.GccEditablePanelShow = false;
    const urlParams = new URLSearchParams(window.location.search);
    // on page load
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

        $scope.GetCategoryList();
        $scope.GetDistrictWithoutDivisionList();


        $scope.GCCEId = urlParams.get('eId');
        $scope.GCCERNO = urlParams.get('eRno');


        $scope.GetCurrentUserId();
        $scope.GetCurrentRoleId();


    };
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
        // debugger;
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
                    //    debugger;
                    if (data.length > 0) {
                        $scope.JanMemberList = data;
                    }
                }).
                error(function (data, status) {
                    alert("Error while fetching Year Target Data");
                });
        }
    }
    $scope.GetFormViewData = function () {
        // $http.post(SiteUrl + "/GopalCreditCard/Get_GopalApplicationFormViewData1/?GCCEId=" + encodeURIComponent($scope.GCCEId) + "&GCCERNO=" + encodeURIComponent($scope.GCCERNO)).
        $http.post(SiteUrl + "/GopalCreditCard/Get_GopalApplicationFormViewData_v1/?GCCEId=" + $scope.GCCEId).

            success(function (data, status) {
                $scope.GopalCreditCardModel = data;
                $scope.GopalCreditCardModel.IsCopyOfDCSPassbookAttached = (data.CopyOfDCSPassbook_Path != '' || data.PBBankAccountNo != '') ? true : false;
                $scope.GopalCreditCardModel.IsPACSBankAccountAvailable = (data.BankAccountNo != '' || data.BankIFSCCode || data.BankName || data.BankBranchName) ? true : false;
                $scope.CropLoanDetailsList = data.CropLoanDetailsList;
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

                $scope.GopalCreditCardModel.MemberPhoto_Base64 = data.MemberPhoto;
                $scope.GGG_Id = data.gcc_id;

                $scope.GetApplicationFlowGCC();

                $scope.RuralUrbanBlock();
                $scope.GramPanchayatList();
                $scope.VillageList();
                $scope.LoanBankList();
                $scope.Get_LoanBankBranch();
                $scope.Get_LoanBranch_PACS();
                $scope.CurrentLoanBankList();
                $scope.Get_CurrentLoanBankBranch('0');
                $scope.GopalCreditCardModel.CurrentLoanDistrictId = '' + $scope.GopalCreditCardModel.CurrentLoanDistrictId;
                $scope.GopalCreditCardModel.CurrentLoanBankID = '' + $scope.GopalCreditCardModel.CurrentLoanBankID;
                $scope.GopalCreditCardModel.CurrentLoanBranchId = '' + $scope.GopalCreditCardModel.CurrentLoanBranchId;
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




                $scope.IsMemberOfDCS_Changed();
                $scope.IsPACSLAMPSMember_Changed();
                $scope.DoesTheMilkSaleToDCS_Changed();
                $scope.Get_GccOtherDocuments();


                // $scope.GGG_Id = data.gcc_id;

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

                guarrantorMergeExist();
                DecryptDECValue();
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    function DecryptDECValue() {
        const keysUsingDEC = [];     
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

        //  debugger;

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
        //debugger;
        var printContents = document.getElementById(divName).innerHTML;

        if (navigator.userAgent.toLowerCase().indexOf('chrome') > -1) {
            var popupWin = window.open('', '_blank', 'width=600,height=600,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no');
            popupWin.window.focus();
            popupWin.document.write('<!DOCTYPE html><html><head>' +
                '<link rel="stylesheet" type="text/css" href="style.css" />' +
                '</head><body onload="window.print()"><div class="reward-body">' + printContents + '</div></body></html>');
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

    $scope.DivtringGrid = true;

    //madhuri
    $scope.BackToPage = function () {
        window.location = '../gopalCreditCard/ApplicationFlowGCC';
    }
    $scope.GetCurrentUserId = function () {
        $http.get("../ApplicationFlow/GetCurrentUserId").
            success(function (data, status) {
                $scope.BaseUserId = data;
            }).
            error(function (data, status) {
                $scope.BaseUserId = '';
            });
    }
    $scope.GetCurrentRoleId = function () {
        $http.get("../gopalCreditCard/GetCurrentRoleId").
            success(function (data, status) {
                $scope.BaseRoleId = data;
            }).
            error(function (data, status) {
                $scope.BaseRoleId = '';
            });
    }
    $scope.GetApplicationFlowGCC = function () {
        $scope.showLoader = true;
        $http.get("../gopalCreditCard/GetApplicationFlowGCC/?GCC_Id=" + $scope.GGG_Id).
            success(function (data, status) {
                $scope.ApplicationFlowGCC = data;

                $scope.GSSProceesStatusList(data[0].StatusId, $scope.GGG_Id);
                $scope.ApplicationFlowMaster.UserId = data[0].UserId; //last UserId
                $scope.ApplicationFlowMaster.RoleId = data[0].RoleID; //last RoleID


                if (data[0].RoleID == 10226 && data[0].StatusId == 7) {
                    $scope.Get_GccDocuments();

                    $scope.GccEditablePanelShow = true;
                }

                else {
                    $scope.GccEditablePanelShow = false;
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
            error(function (data1, status) {

            });
        $scope.showLoader = true;
    }


    $scope.SaveAppFlow = function () {

        if ($scope.GccReportFile == undefined && ($scope.ApplicationFlowMaster.StatusId == 7 || $scope.ApplicationFlowMaster.StatusId == 8)) {

            if ($scope.ApplicationFlowMaster.StatusId == 7) {
                alert('en-us' ? '\n  Please Upload Inspection Report(PDF)' : '\n  कृपया मौका रिपोर्ट अपलोड करें(पीडीएफ) |');
            }
            else if ($scope.ApplicationFlowMaster.StatusId == 8) {

                alert('en-us' ? '\n  Please Upload Branch Level Report(PDF)' : '\n  कृपया शाखा स्तर रिपोर्ट अपलोड करें(पीडीएफ) |');
            }
            else {
                alert('en-us' ? '\n  Please Upload File(PDF)' : '\n  कृपया फ़ाइल अपलोड करें(पीडीएफ) |');
            }
            return;
        }


        $scope.GopalCreditCardFormModel = $scope.GopalCreditCardModel;

        //if ($scope.ApplicationFlowMaster.StatusId == 16 || $scope.ApplicationFlowMaster.StatusId == 17 || $scope.ApplicationFlowMaster.StatusId == 15) {
        //    if (!confirm((_language == 'en-us' ? '\n  Are you sure you want to reject this application ?' : '\n  क्या आप वाकई इस एप्लिकेशन को अस्वीकार करना चाहते हैं?'))) {
        //        return;
        //    }
        //}

        if ($scope.ApplicationFlowMaster.StatusId == 19) {
            if ($scope.ApplicationFlowMaster.LoanAmountRecommendedByDCS == undefined || $scope.ApplicationFlowMaster.LoanAmountRecommendedByDCS == '') {
                alert('en-us' ? '\n  Please Enter Loan Amount Recommended by the Dairy Co-Operative Society (DCS):' : '\n  कृपया डेयरी सहकारी समिति (DCS) द्वारा अनुशंसित ऋण राशि दर्ज करें: |');
                return;
            }
        }
        if ($scope.ApplicationFlowMaster.LoanAmountRecommendedByDCS != '' && $scope.ApplicationFlowMaster.LoanAmountRecommendedByDCS != null && $scope.ApplicationFlowMaster.LoanAmountRecommendedByDCS != undefined) {

            if (parseInt($scope.ApplicationFlowMaster.LoanAmountRecommendedByDCS) > 100000) {
                alert('en-us' ? '\n Please enter amount up to 1 Lakh Tk:' : '\n  कृपया 1 लाख तक की राशि दर्ज करें |');
                return;
                //message += $scope._language == 'en-us' ? '\nPlease enter amount up to 1 Lakh Tk' : '\nकृपया 1 लाख तक की राशि दर्ज करें';
                //message += '<br />';
            }
        }


        if ($scope.ApplicationFlowMaster.StatusId != 0 && $scope.ApplicationFlowMaster.Remark != "") {

            //if ($scope.ApplicationFlowMaster.StatusId != 4 && $scope.ApplicationFlowMaster.StatusId != 19) {
            //    $scope.saveDataInDB();
            //}
            //else {
            $(".se-pre-con").fadeIn("slow");

            //AadhaarAuthEsign($http,function (authData) {
            //    //$scope.InsertAction(authData.TransId);
            //    $scope.ApplicationFlowMaster.TXN = authData.TransId;
            //    $scope.VerifyOTP();

            //});


            $http.get("../ApplicationFlow/GenerateAadhaarOTPForEsing").
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data.indexOf('NO#') >= 0) {
                        alert("Due to some issue OTP not send");   
                        //location.reload();
                        //window.location = '../ApplicationFlow/ApplicationFlow';
                    } else {
                        if (data != "LOGOUT") {
                            // alert(data);
                            $scope.ApplicationFlowMaster.TXN = data;
                            alert(_language == 'en-us' ? 'OTP has been sent to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n');
                            $scope.ShowOTPDiv = true;                            
                        }
                        else {
                            $(".se-pre-con").fadeOut("slow");
                            window.location = 'https://sso.rajasthan.gov.in/signin';
                        }
                    }
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                });
        }
        else {
            alert(_language == 'en-us' ? 'Please Select Status and Remark\n' : 'कृपया स्थिति और टिप्पणी जोड़ें|\n');
        }
    }

    $scope.SaveGSSApplicationFlowStatus = function () {
        if ($scope.GccEditablePanelShow && $scope.levelId == 8) {
            $scope.SaveObjectionFieldInDB();
        }
        else {
            $scope.SaveAppFlow();
        }

        //if ((($scope.ApplicationFlowMaster.IsRecommendationOfDCSDOC_Path == undefined || $scope.ApplicationFlowMaster.IsRecommendationOfDCSDOC_Path == '') && $scope.GopalCreditCardModel.RecommendationOfDCSDOC_Path != '') ||
        //    (($scope.ApplicationFlowMaster.IsDocumentOfProposedWork_Path == undefined || $scope.ApplicationFlowMaster.IsDocumentOfProposedWork_Path == '') && $scope.GopalCreditCardModel.DocumentOfProposedWork_Path != '') ||
        //    (($scope.ApplicationFlowMaster.IsCopyOfDCSPassbook_Path == undefined || $scope.ApplicationFlowMaster.IsCopyOfDCSPassbook_Path == '') && $scope.GopalCreditCardModel.CopyOfDCSPassbook_Path != '')
        //) {
        //    ShowOkMessage("", _language == 'en-us' ? 'Please Take Action on Documents\n' : 'कृपया दस्तावेजों पर कार्रवाई करें\n');
        //    return;
        //}

    };

    $scope.StatusChange = function () {
        var ddlStatus = $scope.ddlStatus;
        if (ddlStatus != '')
            $scope.GccProceesStatus = $scope.GccProceesStatus_Temp.filter(x => x.ProcessStatusType == ddlStatus);

        $scope.Appstatus1 = true;
        if ($scope.GccProceesStatus.length == 1) {
            $scope.Appstatus1 = false;
            $scope.ApplicationFlowMaster.StatusId = $scope.GccProceesStatus[0].GccProceesStatusId;
        }

        $scope.ApplicationFlowMaster.LoanAmountRecommendedByDCS = 0;
        $scope.ApplicationFlowMaster.PurposeOfLoan = "0";
        $scope.ApplicationFlowMaster.PurposeOfLoanOther = '';
    }
    $scope.getDistinctProceesStatus = function () {
        const statusMap = new Map();
        $scope.GccProceesStatus_Temp.forEach(item => {
            if (!statusMap.has(item.ProcessStatusType)) {
                statusMap.set(item.ProcessStatusType, item);
            }
        });
        return Array.from(statusMap.values());
    };


    $scope.GSSProceesStatusList = function (StatusId, GccId) {
        $scope.GccProceesStatus_Temp = [];

        $http.post("../gopalCreditCard/GSSProceesStatusList", { "StatusId": StatusId, "GCC_Id": GccId }).
            success(function (data, status) {
                $scope.GccProceesStatus_Temp = data;
                $scope.ProceesStatus = $scope.getDistinctProceesStatus();

                if ($scope.ProceesStatus.length == 1) {
                    $scope.Appstatus1 = false;
                    $scope.ApplicationFlowMaster.StatusId = $scope.ProceesStatus[0].GccProceesStatusId;
                }




                //if ($scope.GccProceesStatus_Temp.length == 1) { $scope.Appstatus = false; $scope.GccProceesStatus = $scope.GccProceesStatus_Temp; }
                //else { $scope.Appstatus = true; $scope.ProceesStatus = $scope.getDistinctProceesStatus(); }    
            }).
            error(function (data, status) {
                $scope.GccProceesStatus_Temp = [];
            });

    };

    //$scope.GCCDocResStatus = function () {
    //    if ($scope.ApplicationFlowGCC[0].DocResStatus != undefined && $scope.ApplicationFlowGCC[0].DocResStatus != "") {
    //        let arr = ($scope.ApplicationFlowGCC[0].DocResStatus.toString()).split(":");
    //        for (var i = 0; i < arr.length; i++) {
    //            if (arr[i].includes("Doc_DCS")) {
    //                $scope.ApplicationFlowMaster.IsRecommendationOfDCSDOC_Path = (arr[i].split("-"))[1];
    //            }
    //            else if (arr[i].includes("Doc_PW")) {
    //                $scope.ApplicationFlowMaster.IsDocumentOfProposedWork_Path = (arr[i].split("-"))[1];
    //            }
    //            else if (arr[i].includes("Doc_PB")) {
    //                $scope.ApplicationFlowMaster.IsCopyOfDCSPassbook_Path = (arr[i].split("-"))[1];
    //            }
    //            else if (arr[i].includes("Doc_Jam")) {
    //                $scope.ApplicationFlowMaster.IsCopyOfJamabandi = (arr[i].split("-"))[1];
    //            }
    //            else if (arr[i].includes("Doc_Lease")) {
    //                $scope.ApplicationFlowMaster.ICopyOfLease = (arr[i].split("-"))[1];
    //            }
    //        }
    //    }

    //}


    $scope.UploadReportFile = function (file) {
        $scope.GccReportFile = file[0];
        $scope.UploadeReportFileInDB();
    }
    $scope.saveDataInDB = function () {
        $scope.showLoader = true;
        var DocumentCheck = "";


        if ($scope.GccReportFile == undefined && ($scope.ApplicationFlowMaster.StatusId == 7 || $scope.ApplicationFlowMaster.StatusId == 8)) {

            if ($scope.ApplicationFlowMaster.StatusId == 7) {
                alert('en-us' ? '\n  Please Upload Inspection Report(PDF)' : '\n  कृपया मौका रिपोर्ट अपलोड करें(पीडीएफ) |');
            }
            else if ($scope.ApplicationFlowMaster.StatusId == 8) {

                alert('en-us' ? '\n  Please Upload Branch Level Report(PDF)' : '\n  कृपया शाखा स्तर रिपोर्ट अपलोड करें(पीडीएफ) |');
            }
            else {
                alert('en-us' ? '\n  Please Upload File(PDF)' : '\n  कृपया फ़ाइल अपलोड करें(पीडीएफ) |');
            }
            return;
        }


        $scope.GopalCreditCardFormModel = $scope.GopalCreditCardModel;

        if ($scope.ApplicationFlowMaster.StatusId == 16 || $scope.ApplicationFlowMaster.StatusId == 17 || $scope.ApplicationFlowMaster.StatusId == 15) {
            if (!confirm((_language == 'en-us' ? '\n  Are you sure you want to reject this application ?' : '\n  क्या आप वाकई इस एप्लिकेशन को अस्वीकार करना चाहते हैं?'))) {
                return;
            }
        }

        if ($scope.ApplicationFlowMaster.StatusId == 19) {
            if ($scope.ApplicationFlowMaster.LoanAmountRecommendedByDCS == undefined || $scope.ApplicationFlowMaster.LoanAmountRecommendedByDCS == '') {
                alert('en-us' ? '\n  Please Enter Loan Amount Recommended by the Dairy Co-Operative Society (DCS):' : '\n  कृपया डेयरी सहकारी समिति (DCS) द्वारा अनुशंसित ऋण राशि दर्ज करें: |');
                return;
            }

        }


        $scope.ApplicationFlowMaster.DocResStatus = DocumentCheck;
        $scope.ApplicationFlowMaster.GccId = $scope.GGG_Id;


        $(".se-pre-con").fadeIn("slow");
        //$http.post(SiteUrl + "/gopalCreditCard/SaveGSSApplicationFlowStatus", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": $scope.ApplicationFlowMaster.TXN, "GccId": $scope.GGG_Id, "Remark": $scope.ApplicationFlowMaster.Remark, "StatusId": $scope.ApplicationFlowMaster.StatusId, "UserId": $scope.ApplicationFlowMaster.UserId, "DocResStatus": DocumentCheck, "_Model": $scope.GopalCreditCardModel }).
        $http.post(SiteUrl + "/gopalCreditCard/SaveGSSApplicationFlowStatus_V1", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": $scope.ApplicationFlowMaster.TXN, "_Model": $scope.GopalCreditCardModel, "_AppFlowModel": $scope.ApplicationFlowMaster }).

            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data > 0) {
                    ShowOkMessage("Success", _language == 'en-us' ? 'Data saved successfully\n' : 'डेटा सहेजा गया है\n', function () { window.location.reload(); });
                }
                else if (data == -1) {
                    ShowOkMessage("Error", _language == 'en-us' ? 'The application cannot be forwarded because there is no user specified \n' : 'आवेदन अग्रेषित नहीं किया जा सकता क्योंकि कोई उपयोगकर्ता निर्दिष्ट नहीं है \n');
                }
                else if (data == -2) {
                    ShowOkMessage("Error", _language == 'en-us' ? 'Invalid OTP! \n' : 'अवैध ओटीपी!\n');
                }
                else {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("Error", _language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');

                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while insert");
            });

        $scope.showLoader = false;
    };

    $scope.ClearAppFlowForm = function () {
        $scope.ApplicationFlowMaster.Remark = '';
        $scope.ApplicationFlowMaster.StatusId = '0';
        $scope.ApplicationFlowMaster.PurposeOfLoan = "0";
        $scope.ApplicationFlowMaster.PurposeOfLoanOther = '';
        $scope.GccReportFile = '';
        $scope.ShowOTPDiv = false;
        window.location = '../gopalCreditCard/ApplicationFlowGCC';

    }
    $scope.GccDocumentsModel = {};

    $scope.UploadeReportFileInDB = function () {

        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();

        if ($scope.ApplicationFlowMaster.StatusId == 7) {
            $scope.GccDocumentsModel.DocId = 1;//MokaReport
        }
        else if ($scope.ApplicationFlowMaster.StatusId == 8) {

            $scope.GccDocumentsModel.DocId = 2;//Branch Level Report
        }
        else if ($scope.ApplicationFlowMaster.StatusId == 4) {

            $scope.GccDocumentsModel.DocId = 5;//Branch Level Report mool dastavej
        }
        else {
            $scope.GccDocumentsModel.DocId = 4;
        }


        $scope.GccDocumentsModel.id = 0;
        $scope.GccDocumentsModel.GccId = $scope.GopalCreditCardModel.gcc_id;

        $scope.GccDocumentsModel.RegistrationNo = $scope.GopalCreditCardModel.RegistrationNo;

        formData.append("id", 0);
        formData.append("DocId", $scope.GccDocumentsModel.DocId);
        formData.append("GccId", $scope.GopalCreditCardModel.gcc_id);
        formData.append("RegistrationNo", $scope.GopalCreditCardModel.RegistrationNo);
        formData.append("GccReportFile", $scope.GccReportFile);

        $http.post(SiteUrl + "/gopalCreditCard/SaveGCCVerifyReportFile", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {

                $scope.ApplicationFlowMaster.DocumentURL = data.fileName;
                $scope.ApplicationFlowMaster.DocId = data.docId;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
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
                    //  $scope.gcc.GopalCreditCardIdEnc = $scope.GccIdEnc;
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

    $scope.checkInput = function (event) {
        var key = event.keyCode || event.which;
        var forbiddenKeys = [34, 39, 47]; // ASCII codes for ", ', and /

        if (forbiddenKeys.indexOf(key) !== -1) {
            event.preventDefault();
        }
    };


    //Get Category List for dropdown
    $scope.GetCategoryList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=-1").
            success(function (data, status) {
                $scope.CategoryList = data;
                $scope.GetFormViewData();
            }).
            error(function (data, status) {
                alert("Error while fetching Post list");
                $scope.GetFormViewData();
            });
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


    $scope.GccDocuments = [];
    $scope.GccOtherDocuments = [];

    $scope.Get_GccOtherDocuments = function () {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/GopalCreditCard/GetGccOtherDoc/?GccId=" + $scope.GGG_Id).
            success(function (data, status) {
                $scope.GccOtherDocuments = data.Table;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.GccOtherDocuments = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.Get_GccDocuments = function () {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/GopalCreditCard/GetGccDoc/?GccId=" + $scope.GGG_Id).
            success(function (data, status) {
                console.log(data);
                $scope.GccDocuments = data.Table.filter(x => x.Type.trim() == 'D');
                $scope.GccObjectionPanel = data.Table.filter(x => x.Type.trim() == 'P');
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.GccDocuments = [];
                $scope.GccObjectionPanel = [];

                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.ValidationObjectionField = function () {
        var message = "";
        var AlertPopUpHeading = " Objection Field Validation";
        var Alldoc = $scope.GccDocuments.filter(x => x.status == null || x.status == '');
        var AllPanel = $scope.GccObjectionPanel.filter(x => x.status == null || x.status == '');

        var AllObjdoc = $scope.GccDocuments.filter(x => x.status == 'O');
        var AllObjPanel = $scope.GccObjectionPanel.filter(x => x.status == 'O');


        if (Alldoc.length > 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Take action on All Document' : '\n <br /> कृपया सभी दस्तावेज़ों पर कार्रवाई करें |';
        }
        if (AllPanel.length > 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Take action on All Objection Field' : '\n <br /> कृपया सभी आपत्ति क्षेत्रों पर कार्रवाई करें |';
        }
        if ($scope.ApplicationFlowMaster.StatusId == 0 || $scope.ApplicationFlowMaster.StatusId == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Status ' : '\n <br /> कृपया स्थिति जोड़ें|';
        }
        if ($scope.GccReportFile == undefined && ($scope.ApplicationFlowMaster.StatusId == 7 || $scope.ApplicationFlowMaster.StatusId == 8)) {
            if ($scope.ApplicationFlowMaster.StatusId == 8) {

                message += $scope._language == 'en-us' ? '\n  Please Upload Branch Level Report(PDF)' : '\n  कृपया शाखा स्तर रिपोर्ट अपलोड करें(पीडीएफ) |';
            }
            else {
                message += $scope._language == 'en-us' ? '\n  Please Upload File(PDF)' : '\n  कृपया फ़ाइल अपलोड करें(पीडीएफ) |';
            }

        }

        if ($scope.ApplicationFlowMaster.Remark == undefined || $scope.ApplicationFlowMaster.Remark == "") {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Remark' : '\n <br /> कृपया टिप्पणी जोड़ें|';
        }


        if ($scope.ApplicationFlowMaster.StatusId == 8) {
            if (AllObjdoc.length > 0 || AllObjPanel.length > 0) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Approve All Objection Field' : '\n <br /> कृपया सभी आपत्ति क्षेत्र स्वीकृत करें |';
            }

            if ($scope.ApplicationFlowMaster.PurposeOfLoan == 0 || $scope.ApplicationFlowMaster.PurposeOfLoan == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Purpose of Loan' : '\n <br /> कृपया ऋण का उद्देश्य दर्ज करें |';
            }
            if ($scope.ApplicationFlowMaster.PurposeOfLoan == 5 && (ApplicationFlowMaster.PurposeOfLoanOther == undefined || ApplicationFlowMaster.PurposeOfLoanOther == '')) {
                message += $scope._language == 'en-us' ? '\n <br /> Please enter Other loan purpose ' : '\n <br /> कृपया अन्य ऋण उद्देश्य दर्ज करें|';
            }
            if ($scope.ApplicationFlowMaster.LoanAmountRecommendedByDCS == undefined || $scope.ApplicationFlowMaster.LoanAmountRecommendedByDCS == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Loan Amount Recommended by the Dairy Co-Operative Society (DCS):' : '\n <br /> कृपया डेयरी सहकारी समिति (DCS) द्वारा अनुशंसित ऋण राशि दर्ज करें: |';

            }
        }


        if ($scope.ApplicationFlowMaster.StatusId == 13) {
            if (AllObjdoc.length == 0 && AllObjPanel.length == 0) {
                message += $scope._language == 'en-us' ? '\n <br />All Field are approved. So Please select the document or panel where you have an objection' : '\n <br />सभी क्षेत्र स्वीकृत हैं, इसलिए कृपया वह दस्तावेज़ या पैनल चुनें जहाँ आपको आपत्ति है |';
            }
        }

        if (message.length > 23) {
            ShowModelAlert(AlertPopUpHeading, message);
            return false;
        }
        return true;
    };


    $scope.GccObjDocuments = {}
    $scope.SaveObjectionFieldInDB = function () {
        if ($scope.ValidationObjectionField()) {
            var message = "";
            var AlertPopUpHeading = " Objection Field Validation";

            $scope.GccEditableDocumentslist = [];
            $scope.GccObjDocuments = [...$scope.GccDocuments, ...$scope.GccObjectionPanel]
            $scope.GccEditableDocumentslist = {
                GccEditableDocuments: $scope.GccObjDocuments
            };
            $http.post(SiteUrl + "/gopalCreditCard/SaveGccEditableField", { "_Entity": $scope.GccEditableDocumentslist }).
                success(function (data, status) {
                    if (data == 'Success') {
                        $scope.SaveAppFlow();
                    }
                }).
                error(function (data, status) {
                });
        }
    }

    $scope.VerifyOTP = function () {

        $scope.saveDataInDB();
    }


    function guarrantorMergeExist() {

        $scope.fileExists = false;

        // Path to the file you want to check
        $scope.filePath = SiteUrl + "Content/GopalCreditCard/GSSProfiling/" + $scope.GopalCreditCardModel.RegistrationNo.replace(/\//g, '') +"/GuarantorMerge.pdf";

        // Make a HEAD request to check if the file exists
        $http.head($scope.filePath).then(
            function (response) {
                // File exists
                $scope.GuarrantorMergeRptExists = true;

            },
            function (error) {
                // File does not exist
                $scope.GuarrantorMergeRptExists = false;
            }
        );

    }
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


    $scope.levelId = $('#hdnLevelId').val();
    $scope.OnPageLoad();
});