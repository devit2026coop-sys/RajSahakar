var ArrSelectTraningUser = new Array();
function TraningUserSelected() {
    this.UserId = '0';
}

var AppTrainning = angular.module("GopalCreditCardAppEntryForm", ['angularUtils.directives.dirPagination']);
AppTrainning.directive('maxLengthAndValidate', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            var disallowedChars = /[^0-9]/g;
            ngModel.$parsers.push(function (value) {
                if (!value) return '';
                var cleanValue = value.replace(disallowedChars, '');
                if (cleanValue.length > 6) {
                    cleanValue = cleanValue.substring(0, 6);
                }
                if (cleanValue !== value) {
                    ngModel.$setViewValue(cleanValue);
                    ngModel.$render();
                }

                return cleanValue;
            });
        }
    };
});
AppTrainning.controller("GopalCreditCard", function ($scope, $http) {
    // Save Sms Template 
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivSms = true;
    $scope.newdiv = "";
    $scope.Trainning = '';
    $scope.GCCRegistrationStatus = false;
    $scope.G1DetailFetched = false;
    $scope.G2DetailFetched = false;
    $scope.DistrictList = [];
    $scope.AadharNoEnc = "";
    $scope.signedBy = "4";
    $scope.esignLocation = "";
    $scope.esignDesignation = "";
    $scope.esignFileName = "";
    $scope.gcceIdEsign = "0";
    $scope.GKhasradata = [];
    $scope.GIrrdata = [];
    $scope.objEG = {};
    $scope.eSignData = {};
    // on page load

    $scope.divTabulation = true;
    $scope.ActionId = 1;
    $scope.dataPayload = {};
    $scope.esignData = {};

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



        // Initialise Main form control
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

        $scope.GopalCreditCardModel.ShgClfID = '0';
        $scope.GopalCreditCardModel.ShgVoID = '0';
        $scope.GopalCreditCardModel.ShgId = '0';
        $scope.GopalCreditCardModel.ShgBlockID = '0';
        $scope.GopalCreditCardModel.ShgDistrictId = '0';

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
        $scope.GopalCreditCardModel.IsMahilaNidhiMember = false;

        $scope.ShowJanAadharOTPValidation = false;
        $scope.ShowGopalCreditCardForm = false;
        $scope.ShowFamilyMemberGrid = false;

        //Added new 22-01-2026
        $scope.GopalCreditCardModel.IsWorking = false;
        $scope.GopalCreditCardModel.IsInfrastructure = false;
        $scope.GopalCreditCardModel.IsApparatus = false;
        $scope.workingChildError = false;
        $scope.infrastructureChildError = false;
        $scope.apparatusChildError = false;
        $scope.GopalCreditCardModel.Fodder = false;
        $scope.GopalCreditCardModel.Water = false;
        $scope.GopalCreditCardModel.Medicines = false;
        $scope.GopalCreditCardModel.Insecticides = false;
        $scope.GopalCreditCardModel.Shed = false;
        $scope.GopalCreditCardModel.Kheli = false;
        $scope.GopalCreditCardModel.DairyMattresses = false;
        $scope.GopalCreditCardModel.MilkApparatus = false;
        $scope.isApplicationStatus = 'new';

        $scope.GopalCreditCardModel.IsWorkingChild = '';
        $scope.IsWorkingChildList = [];
        $scope.GopalCreditCardModel.IsInfrastructureChild = '';
        $scope.IsInfrastructureChildList = [];
        $scope.GopalCreditCardModel.IsApparatusChild = '';
        $scope.IsApparatusChildList = [];

        $scope.workingMapping = { Fodder: 1, Water: 2, Medicines: 3, Insecticides: 4 };
        $scope.infrastructureMapping = { Shed: 1, Kheli: 2, DairyMattresses: 3 };
        $scope.apparatusMapping = { MilkApparatus: 1 };
        $scope.ApplicationFlowMaster = {};

        $scope.GetDistrictWithoutDivisionList();
        $scope.DashboardData();
        $scope.GetCategoryList();
        //$scope.PLDBList();
    };

    $scope.DashboardData = function (ActionId, GopalCreditCardId, GopalCreditCardAppNo) {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/GopalCreditCard/GetDashBoardData", { "ActionId": ActionId == undefined ? 0 : parseInt(ActionId), "GopalCreditCardId": GopalCreditCardId == undefined ? 0 : parseInt(GopalCreditCardId) }).
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
    $scope.ApplicationView = function (eId, item) {
        if (item.IsMahilaNidhi) {
            window.location = "../GopalCreditCard/GccMahilaNidhiApplicationFormView?eId=" + eId;
        }
        else if (eId != '') {
            // window.location = "../GopalCreditCard/GopalApplicationFormView?eId=" + eId + "&eRno=" + eRno;
            // window.location = "../GopalCreditCard/GopalApplicationFormView_v1?eId=" + EncGccId;
            window.location = "../GopalCreditCard/GopalApplicationFormView?eId=" + eId;

        }
    }
    $scope.openModelPopupGKK = function (rowId) {
        $scope.activeRowId = rowId;
        $('#modelPopupGKK').modal();
    }
    $scope.GetGKKdata = function (villageCodeG, khataNo, khasraNo) {
        if ($scope.ValidateGKKForm()) {
            $(".se-pre-con").fadeIn("slow");
            $http.post("/CommonServices/Girdawari_GetKhasraData", { "villageCode": villageCodeG, "khataNo": khataNo, "khasraNo": khasraNo }).
                success(function (data, status) {
                    //if (data.Status == 0) {
                    //    alert("No Record Found from Girdhawari Portal");
                    //    break;
                    //}
                    $scope.GKhasradata = JSON.parse(data.DataJson);
                    $scope.GIrrdata = $scope.GKhasradata[0].Crops;
                    //console.log($scope.GKhasradata);
                    //console.log($scope.GKhasradata[0].Crops); 
                    setTimeout(function () {
                        $scope.AgricultureLandDetailList.forEach((v, i) => {
                            if (v.rowId == $scope.activeRowId) {
                                v.LandOwnereName = $scope.GKhasradata[0].OwnerName;
                                v.KhataNumber = $scope.GKhasradata[0].KhataNo;
                                v.KhasraNumber = $scope.GKhasradata[0].KhasraNo;
                                v.PatwarCircle = $scope.GKhasradata[0].PatwarCircle;
                                v.Village = $scope.GKhasradata[0].Village;
                                v.Irrigated = $scope.GIrrdata[0].IrrigatedArea;
                                v.NonIrrigated = $scope.GIrrdata[0].NonIrrigatedArea;
                                $("#modelPopupGKK").modal("hide");
                            }
                        }); $scope.$apply();
                        $scope.objEG = {};
                    }, 1000);
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $scope.GKhasradata = [];
                    $(".se-pre-con").fadeOut("slow");
                });
        }
    }

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

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0")
            .success(function (data, status) {
                if ($scope.IsFormEdit) {
                    $scope.DistrictList = data;
                }
                else {
                    // Districts to remove
                    var removeDistricts = ["Ajmer", "Sawai Madhopur", "Sirohi", "Udaipur"];

                    // Filter data
                    $scope.DistrictList = data.filter(function (item) {
                        return removeDistricts.indexOf(item.DISTRICT_ENG) === -1;
                    });
                }

            })
            .error(function (data, status) {
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
        //if (id && id != '0') {
        //    $scope.GopalCreditCardModel.Block_Code = "0";
        //    $scope.GopalCreditCardModel.GP_Code = "0";
        //    $scope.GopalCreditCardModel.Village_Code = "0";
        //}

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

    $scope.BlockListG = function (id) {
        //debugger;
        if (id && id != '0') {
            $scope.objEG.Block_CodeG = "0";
            $scope.objEG.GP_CodeG = "0";
            $scope.objEG.Village_CodeG = "0";
        }

        $(".se-pre-con").fadeIn("slow");
        $scope.Blocksdata = [];
        $scope.GramPanchayats = [];
        $scope.Villages = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY_DISTRICTCODE&MasterCode=" + $scope.objEG.District_CodeG).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Blocksdata = data;
            }).
            error(function (data, status) {
                $scope.Blocksdata = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.GramPanchayatListG = function (id) {
        if (id && id != '0') {
            $scope.objEG.GP_CodeG = "0";
            $scope.objEG.Village_CodeG = "0";
        }

        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT_BLOCKCODE&MasterCode=" + $scope.objEG.Block_CodeG).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.GramPanchayats = data;
            }).
            error(function (data, status) {
                $scope.GramPanchayats = [];
                $(".se-pre-con").fadeOut("slow");
            });

    };
    $scope.VillageListG = function (id) {
        if (id && id != '0') {
            $scope.objEG.Village_CodeG = "0";
        }
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=VILLAGE_GPCODE&MasterCode=" + $scope.objEG.GP_CodeG).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Villages = data;
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Villages = [];
            });

    };

    $scope.ValidateGKKForm = function () {
        var message = $scope._language == 'en-us' ? 'Validate following :: ' : 'निम्नलिखित मान्य करें :';
        message += '<br />';

        if ($scope.objEG.District_CodeG == "" || $scope.objEG.District_CodeG == null || $scope.objEG.District_CodeG == "0") {
            message += $scope._language == 'en-us' ? '\nPlease Select District' : '\nकृपया जिला चुनें|';
            message += '<br />';
        }
        if ($scope.objEG.Block_CodeG == "" || $scope.objEG.Block_CodeG == null || $scope.objEG.Block_CodeG == "0") {
            message += $scope._language == 'en-us' ? '\nPlease Select Panchyat Samity' : '\nकृपया पंचायत समिति का चयन करें|';
            message += '<br />';
        }
        if ($scope.objEG.GP_CodeG == "" || $scope.objEG.GP_CodeG == null || $scope.objEG.GP_CodeG == "0") {
            message += $scope._language == 'en-us' ? '\nPlease Select Gram Panchyat' : '\nकृपया ग्राम पंचायत का चयन करें|';
            message += '<br />';
        }
        if ($scope.objEG.Village_CodeG == "" || $scope.objEG.Village_CodeG == null || $scope.objEG.Village_CodeG == "0") {
            message += $scope._language == 'en-us' ? '\nPlease Select Village' : '\nकृपया गांव का चयन करें|';
            message += '<br />';
        }
        if ($scope.objEG.GKhataNo == "" || $scope.objEG.GKhataNo == null) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Khata No' : '\nकृपया खाता नंबर दर्ज करें|';
            message += '<br />';
        }
        if ($scope.objEG.GKhasraNo == "" || $scope.objEG.GKhasraNo == null) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Khasra No' : '\nकृपया खसरा नंबर दर्ज करें|';
            message += '<br />';
        }

        if (message.length > 29) {
            ShowModelAlert($scope._language == 'en-us' ? 'Agricultural Land Details :: ' : 'कृषि भूमि विवरण ::', message);
            return false;
        } else {
            return true;
        }
    }

    $scope.GramPanchayatList = function (id) {
        if (id && id != '0') {
            $scope.GopalCreditCardModel.GP_Code = "0";
            $scope.GopalCreditCardModel.Village_Code = "0";
        }

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
        if (id && id != '0') {
            $scope.GopalCreditCardModel.Village_Code = "0";
        }
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


    $scope.GetJanAadharDetail1 = function () {
        if ($scope.GopalCreditCardJanaadharValidate()) {
            if ($scope.Validate_CheckGCCRegistrationStatus()) {
                $(".se-pre-con").fadeIn("slow");
                $scope.JanMemberList = '';
                $http.post(SiteUrl + "/GopalCreditCard/GetJanAadhaarMembersList?JanAadharNo=" + encodeURIComponent(encryptData($scope.JanAadharNo))).
                    success(function (data, status) {

                        if (data.length > 0) {
                            $scope.JanMemberList = data;
                            $scope.ShowFamilyMemberGrid = true;
                            $(".se-pre-con").fadeOut("slow");
                        }
                        else if (data.length == 0) {
                            //alert("This Jan Aadhaar No. is already registered  ");
                            alert("The entered Jan Aadhaar Number is either already registered or could not be found.Please verify and try again.");
                            $scope.ShowFamilyMemberGrid = false;
                            $(".se-pre-con").fadeOut("slow");

                        }
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
            }

        }
    }

    $scope.GetJanAadharDetail = function () {
        if ($scope.GopalCreditCardJanaadharValidate()) {
            if ($scope.Validate_CheckGCCRegistrationStatus()) {
                $(".se-pre-con").fadeIn("slow");
                $scope.JanMemberList = '';
                JanAadhaarAuth($scope.JanAadharNo, "", "GCC", "", "", $http, false, "", "", function (data) {
                    if (data.length > 0) {
                        $scope.JanMemberList = data;
                        $scope.ShowFamilyMemberGrid = true;
                        $(".se-pre-con").fadeOut("slow");
                    }
                    else if (data.length == 0) {
                        alert("The entered Jan Aadhaar Number is either already registered or could not be found.Please verify and try again.");
                        $scope.ShowFamilyMemberGrid = false;
                        $(".se-pre-con").fadeOut("slow");

                    }
                    else {
                        alert("No data found");
                        $scope.ShowFamilyMemberGrid = false;
                        $(".se-pre-con").fadeOut("slow");
                    }
                });
            }
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

    $scope.CheckShgMembersExist_V1 = function (mobileNo, Encrypted_JAN_MEMBER_ID, Encrypted_JAN_AADHAR, callback) {
        $http.get(SiteUrl + "/RajeevikaMaster/CheckShgMembersExist/?JanAadharNo=" +
            encodeURIComponent(Encrypted_JAN_AADHAR) +
            "&AadharNo=" + encodeURIComponent(Encrypted_JAN_MEMBER_ID))
            .then(function (response) {
                var data = response.data;
                if (data && data.length > 0) {
                    // You can call callback with true and data
                    if (typeof callback === "function") {
                        callback(true, data);
                    }
                } else {
                    // No data found, still call callback
                    if (typeof callback === "function") {
                        callback(false, null);
                    }
                }
            }, function (error) {
                if (typeof callback === "function") {
                    callback(false, null);
                }
            })
            .finally(function () {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.VerifyOTPViewPopup_1 = function (mobileNo, Encrypted_JAN_MEMBER_ID, Encrypted_JAN_AADHAR, item) {
        $(".se-pre-con").fadeIn("slow");
        var message = "";
        if ($scope.GopalCreditCardModel.IsMahilaNidhiMember) {
            $scope.CheckShgMembersExist_V1(mobileNo, Encrypted_JAN_MEMBER_ID, Encrypted_JAN_AADHAR, function (isSuccess, data) {

                if (isSuccess) {
                    $(".se-pre-con").fadeOut("slow");
                    // alert(decrypt(Encrypted_JAN_AADHAR));
                    // Proceed with redirection using the response data
                    var redirectUrl = "../GopalCreditCard/GccMahilaNidhiApplicationForm?" + "MobileNo=" + encodeURIComponent(mobileNo) +
                        "&Encrypted_JAN_MEMBER_ID=" + encodeURIComponent(Encrypted_JAN_MEMBER_ID) +
                        "&Encrypted_JAN_AADHAR=" + encodeURIComponent(Encrypted_JAN_AADHAR);

                    //var redirectUrl = SiteUrl + "/GopalCreditCard/GccMahilaNidhiApplicationForm?"+ + "MobileNo=" + encodeURIComponent(mobileNo) +
                    //    "&Encrypted_JAN_MEMBER_ID=" + encodeURIComponent(Encrypted_JAN_MEMBER_ID) +
                    //    "&Encrypted_JAN_AADHAR=" + encodeURIComponent(Encrypted_JAN_AADHAR);

                    window.location.href = redirectUrl;

                } else {

                    alert(_tra);
                    $(".se-pre-con").fadeOut("slow");
                    $(".blockUI").fadeOut("slow");
                    $scope.SendAndVerifyOTP = {};
                    $scope.SendAndVerifyOTP.Masked_MOBILE_NO = mobileNo;
                    $scope.SendAndVerifyOTP.Encrypted_JAN_MEMBER_ID = Encrypted_JAN_MEMBER_ID;
                    $scope.SendAndVerifyOTP.Encrypted_JAN_AADHAR = Encrypted_JAN_AADHAR;
                    //$scope.SendAndVerifyOTP.Encrypted_JAN_AADHAR = encodeURIComponent(encryptData(Encrypted_JAN_AADHAR));

                    $scope.SendAndVerifyOTP.tid = "";
                    $("#modelPopupVerifyOTP").modal("show");
                }
            });
        }
        else {
            if ($scope.GopalCreditCardModel.IsMahilaNidhiMember) {
                message += $scope._language == 'en-us' ? '\n <br />' + item.Name + " is not a member of Mahila Nidhi" : '\n <br />' + item.NAME_HINDI + ' महिला निधि सदस्य नहीं है|';
            }
            $scope.GopalCreditCardModel.IsMahilaNidhiMember = false;

            $(".se-pre-con").fadeOut("slow");
            $(".blockUI").fadeOut("slow");
            $scope.SendAndVerifyOTP = {};
            $scope.SendAndVerifyOTP.Masked_MOBILE_NO = mobileNo;
            $scope.SendAndVerifyOTP.Encrypted_JAN_MEMBER_ID = Encrypted_JAN_MEMBER_ID;
            $scope.SendAndVerifyOTP.Encrypted_JAN_AADHAR = Encrypted_JAN_AADHAR;
            //$scope.SendAndVerifyOTP.Encrypted_JAN_AADHAR = encodeURIComponent(encryptData(Encrypted_JAN_AADHAR));

            $scope.SendAndVerifyOTP.tid = "";
            $("#modelPopupVerifyOTP").modal("show");

        }
        var AlertPopUpHeading = $scope._language == 'en-us' ? 'Mahila Nidhi :: ' : 'महिला निधि ::';
        // var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';


        if (message.length > 5) {
            ShowModelAlert(AlertPopUpHeading, message);
            return false;
        }


    };

    $scope.VerifyOTPViewPopup = function (mobileNo, Encrypted_JAN_MEMBER_ID, Encrypted_JAN_AADHAR, item) {
        $(".se-pre-con").fadeIn("slow");
        var message = "";
        if ($scope.GopalCreditCardModel.IsMahilaNidhiMember) {
            $scope.CheckShgMembersExist_V1(mobileNo, Encrypted_JAN_MEMBER_ID, Encrypted_JAN_AADHAR, function (isSuccess, data) {

                if (isSuccess) {
                    $(".se-pre-con").fadeOut("slow");
                    var redirectUrl = "../GopalCreditCard/GccMahilaNidhiApplicationForm?" + "MobileNo=" + encodeURIComponent(mobileNo) +
                        "&Encrypted_JAN_MEMBER_ID=" + encodeURIComponent(Encrypted_JAN_MEMBER_ID) +
                        "&Encrypted_JAN_AADHAR=" + encodeURIComponent(Encrypted_JAN_AADHAR);
                    window.location.href = redirectUrl;

                } else {

                    $(".se-pre-con").fadeOut("slow");
                    $(".blockUI").fadeOut("slow");
                    $scope.SendAndVerifyOTP = {};
                    $scope.SendAndVerifyOTP.Masked_MOBILE_NO = mobileNo;
                    $scope.SendAndVerifyOTP.Encrypted_JAN_MEMBER_ID = Encrypted_JAN_MEMBER_ID;
                    $scope.SendAndVerifyOTP.Encrypted_JAN_AADHAR = Encrypted_JAN_AADHAR;
                    //$scope.SendAndVerifyOTP.Encrypted_JAN_AADHAR = encodeURIComponent(encryptData(Encrypted_JAN_AADHAR));

                    $scope.SendAndVerifyOTP.tid = "";
                    $("#modelPopupVerifyOTP").modal("show");
                }
            });
        }
        else {
            if ($scope.GopalCreditCardModel.IsMahilaNidhiMember) {
                message += $scope._language == 'en-us' ? '\n <br />' + item.Name + " is not a member of Mahila Nidhi" : '\n <br />' + item.NAME_HINDI + ' महिला निधि सदस्य नहीं है|';
            }
            $scope.GopalCreditCardModel.IsMahilaNidhiMember = false;

            //$(".se-pre-con").fadeOut("slow");
            //$(".blockUI").fadeOut("slow");
            //$scope.SendAndVerifyOTP = {};
            //$scope.SendAndVerifyOTP.Masked_MOBILE_NO = mobileNo;
            //$scope.SendAndVerifyOTP.Encrypted_JAN_MEMBER_ID = Encrypted_JAN_MEMBER_ID;
            //$scope.SendAndVerifyOTP.Encrypted_JAN_AADHAR = Encrypted_JAN_AADHAR;
            ////$scope.SendAndVerifyOTP.Encrypted_JAN_AADHAR = encodeURIComponent(encryptData(Encrypted_JAN_AADHAR));

            //$scope.SendAndVerifyOTP.tid = "";
            //$("#modelPopupVerifyOTP").modal("show");

            JanAadhaarAuth("", Encrypted_JAN_MEMBER_ID, "GCC", "", "", $http, false, "", "", function (data) {
                if (data && Object.keys(data).length > 0) {
                    {
                        $scope.ShowJanAadharOTPValidation = false;
                        $scope.ShowMPacsForm = true;
                        $scope.ShowJanAadharOTPValidation = false;
                        $scope.ShowGopalCreditCardForm = true;
                        $scope.GopalCreditCardModel = data;

                        $scope.GopalCreditCardModel.StatusId = 0;
                        if ($scope.CategoryList.filter(x => x.CODENAME == $scope.GopalCreditCardModel.Category).length > 0)
                            $scope.GopalCreditCardModel.CategoryId = $scope.CategoryList.filter(x => x.CODENAME == $scope.GopalCreditCardModel.Category)[0].CODEID;
                        $scope.GopalCreditCardModel.Name_ENG = $scope.GopalCreditCardModel.Name;
                        $scope.GopalCreditCardModel.Name_HI = $scope.GopalCreditCardModel.Name_HI;
                        $scope.GopalCreditCardModel.DOB = $scope.GopalCreditCardModel.DOB;
                        $scope.GopalCreditCardModel.Encrypted_JanAadharMID = $scope.GopalCreditCardModel.Encrypted_JanAadharMID;
                        $scope.GopalCreditCardModel.Encrypted_JanAadharNo = $scope.GopalCreditCardModel.Encrypted_JanAadharNo;
                        $scope.GopalCreditCardModel.FatherName_ENG = $scope.GopalCreditCardModel.FatherName;
                        $scope.GopalCreditCardModel.MotherName = $scope.GopalCreditCardModel.MotherName;
                        $scope.GopalCreditCardModel.Gender = $scope.GopalCreditCardModel.Gender;
                        $scope.GopalCreditCardModel.JanAadharEnrollmentId = $scope.GopalCreditCardModel.JanAadharEnrollmentId;
                        $scope.GopalCreditCardModel.JanAadharNo = $scope.GopalCreditCardModel.JanAadharNo;

                        $scope.GopalCreditCardModel.Original_IsRural = $scope.GopalCreditCardModel.IsRural;
                        $scope.GopalCreditCardModel.Original_District_Code = $scope.GopalCreditCardModel.District_Code;
                        $scope.GopalCreditCardModel.Original_Block_Code = $scope.GopalCreditCardModel.Block_Code;
                        $scope.GopalCreditCardModel.Original_City_Code = $scope.GopalCreditCardModel.City_Code;
                        $scope.GopalCreditCardModel.Original_GP_Code = $scope.GopalCreditCardModel.GP_Code;
                        $scope.GopalCreditCardModel.Original_Village_Code = $scope.GopalCreditCardModel.Village_Code;
                        $scope.GopalCreditCardModel.Original_Address_ENG = $scope.GopalCreditCardModel.Address;
                        $scope.GopalCreditCardModel.Original_Address_HI = $scope.GopalCreditCardModel.Address;
                        $scope.GopalCreditCardModel.Original_PinCode = $scope.GopalCreditCardModel.PinCode;
                        $scope.GopalCreditCardModel.MemberPhoto_Base64 = $scope.GopalCreditCardModel.Photo_Base64;


                        $scope.GopalCreditCardModel.LoanDistrictId = '0';
                        $scope.GopalCreditCardModel.LoanBankID = "0";
                        $scope.GopalCreditCardModel.LoanBranchId = "0";
                        $scope.GopalCreditCardModel.PACS_ID = "0";
                        $scope.GopalCreditCardModel.CurrentLoanDistrictId = "0";
                        $scope.GopalCreditCardModel.CurrentLoanBankID = "0";
                        $scope.GopalCreditCardModel.CurrentLoanBranchId = "0";
                        $scope.GopalCreditCardModel.PurposeOfLoan = "0";

                        $scope.GopalCreditCardModel.IsMahilaNidhiMember = false;
                        var resultCat = $scope.CategoryList.filter(x => x.CODENAME == data.Category)
                        $scope.GopalCreditCardModel.Category = resultCat.length ? resultCat[0].CODEID : '0';
                        $scope.GopalCreditCardModel.IsAddressSameAsJanAadhar = true
                        $scope.IsAddressSameAsJanAadhar_Changed(1);
                        $scope.ActionId = 0;
                        $scope.GetCCBDetails();//madhuri

                        //$scope.janAadharData = data;
                        //if ($scope.janAadharData) {

                        //    $scope.mPacs.Encrypted_JanAadharMID = $scope.janAadharData.Encrypted_JanAadharMID;
                        //    $scope.mPacs.Encrypted_JanAadharNo = $scope.janAadharData.Encrypted_JanAadharNo;
                        //}
                        ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration ' : 'Gopal Credit Card Registration', $scope._language == 'en-us' ? 'OTP has been successfully verified.' : 'ओटीपी सफलतापूर्वक सत्यापित हो गया है।');

                        $("#modelPopupVerifyOTP").modal("hide");
                    }
                    $(".se-pre-con").fadeOut("slow");
                }
                else if (data.length == 0) {
                    ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration ' : 'Gopal Credit Card Registration', $scope._language == 'en-us' ? 'OTP has not been validated' : 'OTP मान्य नहीं किया गया है');

                    $(".se-pre-con").fadeOut("slow");

                }
                else {
                    alert("No data found");
                    $(".se-pre-con").fadeOut("slow");
                }

            });

        }
        var AlertPopUpHeading = $scope._language == 'en-us' ? 'Mahila Nidhi :: ' : 'महिला निधि ::';
        // var message = $scope._language == 'en-us' ? 'Validate  following :: ' : 'निम्नलिखित मान्य करें :';


        if (message.length > 5) {
            ShowModelAlert(AlertPopUpHeading, message);
            return false;
        }


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
                }
                $scope.SetTimer();
                //else {
                //    $scope.SendAndVerifyOTP.tid = "";
                //    ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', $scope._language == 'en-us' ? 'An Error Occurred Please Try Again Later' : 'कुछ त्रुटि घटित हुई है, कृपया बाद में पुन: प्रयास करें');
                //    $("#modelPopupVerifyOTP").modal("hide");
                //}

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.SendAndVerifyOTP.tid = "";
                $(".se-pre-con").fadeOut("slow");
            });
    }
    $scope.ValidateJanAadharOTP = function () {

        if ($scope.JanaadharValidate_FieldValidator()) {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/GopalCreditCard/ValidateJanAadharOTP", $scope.SendAndVerifyOTP).
                success(function (data, status) {
                    if (data.Status == true) {
                        $scope.ShowJanAadharOTPValidation = false;
                        $scope.ShowGopalCreditCardForm = true;
                        $scope.GopalCreditCardModel = data;
                        $scope.GopalCreditCardModel.StatusId = 0;
                        $scope.GopalCreditCardModel.LoanDistrictId = '0';
                        $scope.GopalCreditCardModel.LoanBankID = "0";
                        $scope.GopalCreditCardModel.LoanBranchId = "0";
                        $scope.GopalCreditCardModel.PACS_ID = "0";
                        $scope.GopalCreditCardModel.CurrentLoanDistrictId = "0";
                        $scope.GopalCreditCardModel.CurrentLoanBankID = "0";
                        $scope.GopalCreditCardModel.CurrentLoanBranchId = "0";
                        $scope.GopalCreditCardModel.PurposeOfLoan = "0";

                        $scope.GopalCreditCardModel.IsMahilaNidhiMember = false;
                        var resultCat = $scope.CategoryList.filter(x => x.CODENAME == data.Category)
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
        }
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

        //if (SelectedVal == true || SelectedVal == true) {
        //    $scope.GopalCreditCardModel.IsMemberOfDCSCheckedStatus = true;
        //}
        //else if (SelectedVal == false || SelectedVal == false) {
        //    $scope.GopalCreditCardModel.IsMemberOfDCSCheckedStatus = false;
        //    $scope.GopalCreditCardModel.DCSRegistrationNo = "";
        //    $scope.GopalCreditCardModel.DCSName = "";
        //    $scope.GopalCreditCardModel.DCSOwnerName = "";
        //    $scope.GopalCreditCardModel.DCSOwnerMobileNo = "";
        //    $scope.GopalCreditCardModel.DCSMembershipNo = "";
        //    $scope.GopalCreditCardModel.DCSAddress = "";

        //}
        //else {
        //    $scope.GopalCreditCardModel.IsMemberOfDCS = false;
        //    $scope.GopalCreditCardModel.IsMemberOfDCSCheckedStatus = false;
        //}

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

        //var SelectedVal = $scope.GopalCreditCardModel.IsPACSBankAccountAvailable == 'false' ? false : true;
        //if ($scope.Validate_IsPACSBankAccountAvailable(SelectedVal)) {
        //    if (SelectedVal == true || SelectedVal == true) {
        //        $scope.GopalCreditCardModel.IsPACSBankAccountAvailable_Status = true;
        //    }
        //    else if (SelectedVal == false || SelectedVal == false) {
        //        $scope.GopalCreditCardModel.IsPACSBankAccountAvailable_Status = false;
        //        $scope.GopalCreditCardModel.BankAccountNo = "";
        //        $scope.GopalCreditCardModel.BankIFSCCode = "";
        //        $scope.GopalCreditCardModel.BankName = "";
        //        $scope.GopalCreditCardModel.BankBranchName = "";
        //    }
        //    else {
        //        $scope.GopalCreditCardModel.IsPACSBankAccountAvailable = "";
        //        $scope.GopalCreditCardModel.IsPACSBankAccountAvailable_Status = false;
        //        $scope.GopalCreditCardModel.BankAccountNo = "";
        //        $scope.GopalCreditCardModel.BankIFSCCode = "";
        //        $scope.GopalCreditCardModel.BankName = "";
        //        $scope.GopalCreditCardModel.BankBranchName = "";
        //    }
        //}

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

        // var SelectedVal = $scope.GopalCreditCardModel.IsCopyOfDCSPassbookAttached == 'false' ? false : true;

        //if (SelectedVal == true || SelectedVal == true) {
        //    $scope.GopalCreditCardModel.IsCopyOfDCSPassbookAttached_Status = true;
        //}
        //else if (SelectedVal == false || SelectedVal == false) {
        //    $scope.GopalCreditCardModel.IsCopyOfDCSPassbookAttached_Status = false;
        //    $scope.GopalCreditCardModel.CopyOfDCSPassbook_Path = "";
        //}
        //else {
        //    $scope.GopalCreditCardModel.IsCopyOfDCSPassbookAttached = "";
        //    $scope.GopalCreditCardModel.IsCopyOfDCSPassbookAttached_Status = false;
        //    $scope.GopalCreditCardModel.CopyOfDCSPassbook_Path = "";
        //}


        //if ($scope.GopalCreditCardModel.IsCopyOfDCSPassbookAttached)
        //{
        //    $scope.GopalCreditCardModel.IsCopyOfDCSPassbookAttached_Status = true;
        //}
        //else if (!$scope.GopalCreditCardModel.IsCopyOfDCSPassbookAttached)
        //{
        //    $scope.GopalCreditCardModel.IsCopyOfDCSPassbookAttached_Status = false;
        //    $scope.GopalCreditCardModel.CopyOfDCSPassbook_Path = "";
        //}
        //else {
        //    $scope.GopalCreditCardModel.IsCopyOfDCSPassbookAttached = "";
        //    $scope.GopalCreditCardModel.IsCopyOfDCSPassbookAttached_Status = false;
        //    $scope.GopalCreditCardModel.CopyOfDCSPassbook_Path = "";
        //}

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

    $scope.ValidateAndUploadFile = function (file, Type, size) {
        //alert('hi');

        //this.isValidAttendanceSheetUpload = false;

        $scope.GopalCreditCardModel.isValidDocument = false;
        if (file && file[0]) {
            //  var file = file[0];
            // var maxSize = 2048 * 1024;
            var maxSize = size * 1024; // element.getAttribute("checkmaxsize") 
            if (file[0].size > maxSize) { return; }

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
    $scope.LoanBankList = function (DistrictId) {

        if ($scope.GGG_Id == undefined || $scope.GGG_Id == null || $scope.GGG_Id == 0 || (DistrictId && DistrictId != "0")) {
            $scope.BankMasterList = [];
            $scope.PLDBBranches = [];
            $scope.PACSNameList = [];

            $scope.GopalCreditCardModel.LoanBankID = "0";
            $scope.GopalCreditCardModel.LoanBranchId = "0";
            $scope.GopalCreditCardModel.PACS_ID = "0";
        }

        $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=" + $scope.GopalCreditCardModel.LoanDistrictId).
            success(function (data, status) {
                $scope.BankMasterList = data;
                if ($scope.CCBDetails != undefined && $scope.CCBDetails != null && $scope.CCBDetails != '' && $scope.CCBDetails.CCBId != '') {
                    $scope.GopalCreditCardModel.LoanBankID = ($scope.CCBDetails.CCBId).toString();
                    $scope.Get_LoanBankBranch();
                    document.getElementById('BankID').setAttribute('disabled', 'disabled');
                }

                //$scope.GopalCreditCardModel.LoanBankID = ($scope.CCBDetails.CCBId??0);
            }).
            error(function (data, status) {
                $scope.BankMasterList = [];
            });
    };
    $scope.CurrentLoanBankList = function () {
        if ($scope.GGG_Id == undefined || $scope.GGG_Id == null || $scope.GGG_Id == 0) {
            $scope.GopalCreditCardModel.CurrentLoanBankID = "0";
            $scope.GopalCreditCardModel.CurrentLoanBranchId = "0";
        }
        $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=" + $scope.GopalCreditCardModel.CurrentLoanDistrictId).
            success(function (data, status) {
                $scope.CurrentBankMasterList = data;
            }).
            error(function (data, status) {
                $scope.CurrentBankMasterList = [];
            });
    };
    $scope.Get_LoanBankBranch = function (branch_ID) {
        if ($scope.GGG_Id == undefined || $scope.GGG_Id == null || $scope.GGG_Id == 0) {
            $scope.PLDBBranches = [];
            $scope.PACSNameList = [];

            $scope.GopalCreditCardModel.LoanBranchId = "0";
            $scope.GopalCreditCardModel.PACS_ID = "0";
        }


        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBankBranch/?LoanDistrictId=" + $scope.GopalCreditCardModel.LoanDistrictId + "&LoanBankId=" + $scope.GopalCreditCardModel.LoanBankID).
            success(function (data, status) {
                $scope.PLDBBranches = data;

                if ($scope.CCBDetails != undefined && $scope.CCBDetails != null && $scope.CCBDetails != '' && $scope.CCBDetails.Branch_ID) {
                    $scope.GopalCreditCardModel.LoanBranchId = $scope.CCBDetails.Branch_ID.toString();
                    $scope.Get_LoanBranch_PACS();
                    document.getElementById('BranchId').setAttribute('disabled', 'disabled');
                }


                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.Get_CurrentLoanBankBranch = function (branch_ID) {
        if ($scope.GGG_Id == undefined || $scope.GGG_Id == null || $scope.GGG_Id == 0) {
            $scope.GopalCreditCardModel.CurrentLoanBranchId = "0";
        }

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
    $scope.Get_LoanBranch_PACS = function (branch_ID) {
        //alert(branch_ID);
        if ($scope.GGG_Id == undefined || $scope.GGG_Id == null || $scope.GGG_Id == 0 || (branch_ID && branch_ID != "0")) {
            $scope.GopalCreditCardModel.PACS_ID = "0";
        }

        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBranch_PACS/?LoanBranchId=" + $scope.GopalCreditCardModel.LoanBranchId).
            success(function (data, status) {
                $scope.PACSNameList = data;


                if ($scope.CCBDetails != undefined && $scope.CCBDetails != null && $scope.CCBDetails != '' && $scope.CCBDetails.PACS_ID) {
                    $scope.GopalCreditCardModel.PACS_ID = $scope.CCBDetails.PACS_ID.toString();
                    document.getElementById('PacsId').setAttribute('disabled', 'disabled');
                }


                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PACSNameList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

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
        if ($scope.GopalCreditCardModel.PANCardNo == "" || $scope.GopalCreditCardModel.PANCardNo == null) {
            message += $scope._language == 'en-us' ? '\nPlease Enter PAN No' : '\nकृपया पैन नंबर दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.MobileNo == "" || $scope.GopalCreditCardModel.MobileNo == null) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Mobile No' : '\nकृपया मोबाइल नंबर दर्ज करें|';
            message += '<br />';
        }




        //if (($scope.GopalCreditCardModel.MemberPhoto_Base64 == "" || $scope.GopalCreditCardModel.MemberPhoto_Base64 == null) && ($scope.GopalCreditCardModel.MemberPhoto == "" || $scope.GopalCreditCardModel.MemberPhoto == null)) {
        //    message += $scope._language == 'en-us' ? '\nPlease Upload Applicant Photo' : '\nकृपया आवेदक का फोटो अपलोड करें|';
        //    message += '<br />';
        //}

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
        if ($scope.GopalCreditCardModel.BankAccountNo == "" || $scope.GopalCreditCardModel.BankAccountNo == null || $scope.GopalCreditCardModel.BankAccountNo.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Account Number' : '\nकृपया खाता संख्या दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.BankIFSCCode == "" || $scope.GopalCreditCardModel.BankIFSCCode == null || $scope.GopalCreditCardModel.BankIFSCCode.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Bank IFSC Code' : '\nकृपया बैंक IFSC कोड दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.BankName == "" || $scope.GopalCreditCardModel.BankName == null || $scope.GopalCreditCardModel.BankName.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Bank Name' : '\nकृपया बैंक का नाम दर्ज करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.BankBranchName == "" || $scope.GopalCreditCardModel.BankBranchName == null || $scope.GopalCreditCardModel.BankBranchName.length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Bank Branch Name' : '\nकृपया बैंक शाखा का नाम दर्ज करें|';
            message += '<br />';
        }
        //}
        if ($scope.GopalCreditCardModel.LoanDistrictId == "" || $scope.GopalCreditCardModel.LoanDistrictId == null || $scope.GopalCreditCardModel.LoanDistrictId == "0") {
            message += $scope._language == 'en-us' ? '\nPlease Select District for Loan' : '\nकृपया ऋण के लिए जिला चुनें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.LoanBankID == "" || $scope.GopalCreditCardModel.LoanBankID == null || $scope.GopalCreditCardModel.LoanBankID == "0") {
            message += $scope._language == 'en-us' ? '\nPlease Select Bank for Loan' : '\nकृपया ऋण के लिए बैंक का चयन करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.LoanBranchId == "" || $scope.GopalCreditCardModel.LoanBranchId == null || $scope.GopalCreditCardModel.LoanBranchId == "0") {
            message += $scope._language == 'en-us' ? '\nPlease Select Bank Branch for Loan' : '\nकृपया ऋण के लिए बैंक शाखा का चयन करें|';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.PACS_ID == "" || $scope.GopalCreditCardModel.PACS_ID == null || $scope.GopalCreditCardModel.PACS_ID == "0") {
            message += $scope._language == 'en-us' ? '\nPlease Select PACS Name for Loan' : '\nकृपया ऋण के लिए पैक्स का नाम चुनें|';
            message += '<br />';
        }

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
        if ($scope.GopalCreditCardModel.IsPACSLAMPSMember == null) {
            message += $scope._language == 'en-us' ? '\nPlease Select PACS / LAMPS Membership' : '\nकृपया PACS / LAMPS सदस्यता का चयन करें|';
            message += '<br />';
        }
        else if ($scope.GopalCreditCardModel.IsPACSLAMPSMember == true || $scope.GopalCreditCardModel.IsPACSLAMPSMember == "true") {
            if ($scope.GopalCreditCardModel.PACSLAMPSMembershipNo == "" || $scope.GopalCreditCardModel.PACSLAMPSMembershipNo == null || $scope.GopalCreditCardModel.PACSLAMPSMembershipNo.length == 0) {
                message += $scope._language == 'en-us' ? '\nPlease Enter PACS / LAMPS Membership Detail' : '\nकृपया PACS / LAMPS सदस्यता विवरण दर्ज करें|';
                message += '<br />';
            }
        }
        if ($scope.GopalCreditCardModel.DoesTheMilkSaleToDCS == null) {
            message += $scope._language == 'en-us' ? '\nPlease select, Does the cattle keeper sell the milk to the dairy cooperative society?' : '\nकृपया सेलेक्ट करें,क्या पशुपालक द्वारा दूध का बेचान डेयरी सहकारी समिति को किया जाता है या नहीं|';
            message += '<br />';
        }
        if (!($scope.GopalCreditCardModel.IsWorking || $scope.GopalCreditCardModel.IsInfrastructure || $scope.GopalCreditCardModel.IsApparatus)) {
            message += $scope._language === 'en-us' ? '\nPlease select Purpose of Loan.' : '\nकृपया लोन का उद्देश्य चयन करें।';
            message += '<br />';
        }
        else
            if ($scope.GopalCreditCardModel.DoesTheMilkSaleToDCS == true || $scope.GopalCreditCardModel.DoesTheMilkSaleToDCS == "true") {
                if ($scope.GopalCreditCardModel.EstimatedAmountOfMilkSoldToDCS == "" || $scope.GopalCreditCardModel.EstimatedAmountOfMilkSoldToDCS == null || $scope.GopalCreditCardModel.EstimatedAmountOfMilkSoldToDCS == "0") {
                    message += $scope._language == 'en-us' ? '\nPlease Enter Estimated Amount of Milk Sold to Local Dairy Cooperative Society (DCS)' : '\nकृपया स्थानीय डेयरी सहकारी समिति (DCS) को बेचे गए दूध की अनुमानित मात्रा दर्ज करें|';
                    message += '<br />';
                }
            }
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
        if ($scope.GopalCreditCardModel.GopalCreditCardDeclaration2 == false) {
            message += $scope._language == 'en-us' ? '\nI also hereby my consent for all kind of available insurance.' : '\nमैं इसके द्वारा सभी प्रकार के उपलब्ध बीमा के लिए भी अपनी सहमति देता हूँ।';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.GopalCreditCardDeclaration == false) {
            message += $scope._language == 'en-us' ? '\nI declare that the information given above is correct and true to the best of my knowledge. This loan facility will be used for the same purpose for which it has been sanctioned. I accept all the conditions of the Committee/LAMPS/Bank regarding advancing of loan under the scheme and I will accept all the conditions prescribed by the Committee/LAMPS/Bank/Cooperative Department from time to time.' : '\nमैं घोषणा करता/करती हूँ कि ऊपर दी गई सूचना  मेरी जानकारी में पूर्ण रूप से सही एवं सत्य है। यह ऋण सुविधा उसी उद्देश्य हेतु प्रयुक्त की जावेगी जिसके लिये स्वीकृत की गई है।मुझे योजनान्तर्गत ऋण अग्रिम करने के संबंध में समिति/लैम्पस/बैंक की सभी शर्तें स्वीकार है तथा समिति/लैम्पस/बैंक/सहकारी विभाग द्वारा समय-समय पर निर्धारित की जाने वाली सभी शर्ते मुझे स्वीकार होंगी।';
            message += '<br />';
        }
        if ($scope.GopalCreditCardModel.GopalCreditCardDeclaration4 == false) {
            message += $scope._language == 'en-us' ? '\nI hereby declare that I will execute all necessary post sanction documents as per attached draft/list.' : '\nमैं इसके द्वारा यह घोषणा करता हूं कि मैं अटैच्ड ड्राफ्ट/लिस्ट के अनुसार सभी ज़रूरी पोस्ट सैंक्शन डॉक्यूमेंट्स को पूरा करूंगा।';
            message += '<br />';
        }

        if ($scope.GopalCreditCardModel.G1Name == null || $scope.GopalCreditCardModel.G1Name == '') {
            message += $scope._language == 'en-us' ? '\nPlease Enter the Guarantor Details.' : '\nकृपया जमानतदार का विवरण दर्ज करें।';
            message += '<br />';
        }
        if ($scope.G1DetailFetched == false) {
            message += $scope._language == 'en-us' ? '\nPlease verify the guarantor details.' : '\nकृपया जमानतदार का विवरण सत्यापन करें।';
            message += '<br />';
        }
        if ($scope.G2DetailFetched == false) {
            message += $scope._language == 'en-us' ? '\nPlease verify the guarantor details.' : '\nकृपया जमानतदार का विवरण सत्यापन करें।';
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
                message += $scope._language == 'en-us' ? '\nPlease enter individual aadhaar details of guarantor' : '\nकृपया जमानतदार का अलग-अलग आधार का विवरण दर्ज करें';
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
                message += $scope._language == 'en-us' ? '\nPlease enter amount up to 1 Lakh' : '\nकृपया 1 लाख तक की राशि दर्ज करें';
                message += '<br />';
            }
        }

        if ($scope.GopalCreditCardModel.G1Photo == undefined || $scope.GopalCreditCardModel.G1Photo == null || $scope.GopalCreditCardModel.G1Photo == '' || $scope.GopalCreditCardModel.G2Photo == undefined || $scope.GopalCreditCardModel.G2Photo == null || $scope.GopalCreditCardModel.G2Photo == '') {
            message += $scope._language == 'en-us' ? '\nPlease upload the Photo of the guarantor ' : '\nकृपया जमानतदार का फोटो अपलोड करें';
            message += '<br />';

        }
        //if ($scope.GopalCreditCardModel.G1Signature == undefined || $scope.GopalCreditCardModel.G1Signature == null || $scope.GopalCreditCardModel.G1Signature == '' || $scope.GopalCreditCardModel.G2Signature == undefined || $scope.GopalCreditCardModel.G2Signature == null || $scope.GopalCreditCardModel.G2Signature == '') {
        //    message += $scope._language == 'en-us' ? '\nPlease upload the signature of the guarantor' : '\nकृपया जमानतदार के हस्ताक्षर अपलोड करें';
        //    message += '<br />';

        //}
        //if ($scope.GopalCreditCardModel.AadharDoc == undefined || $scope.GopalCreditCardModel.AadharDoc == null || $scope.GopalCreditCardModel.AadharDoc == '') {
        //    message += $scope._language == 'en-us' ? '\nPlease upload Aadhaar Card Document' : '\nकृपया आधार कार्ड दस्तावेज़ अपलोड करें';
        //    message += '<br />';

        //}

        if ($scope.GopalCreditCardModel.IsMahilaNidhiMember ?? false) {

            if ($scope.GopalCreditCardModel.ShgDistrictId == "" || $scope.GopalCreditCardModel.ShgDistrictId == null || $scope.GopalCreditCardModel.ShgDistrictId == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select District for Mahila Nidhi' : '\nकृपया महिला निधि के लिए जिला चुनें|';
                message += '<br />';
            }
            if ($scope.GopalCreditCardModel.ShgBlockID == "" || $scope.GopalCreditCardModel.ShgBlockID == null || $scope.GopalCreditCardModel.ShgBlockID == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select Block for Mahila Nidhi' : '\nकृपया महिला निधि के लिए खंड का चयन करें|';
                message += '<br />';
            }
            if ($scope.GopalCreditCardModel.ShgClfID == "" || $scope.GopalCreditCardModel.ShgClfID == null || $scope.GopalCreditCardModel.ShgClfID == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select CLF Branch for Mahila Nidhi' : '\nकृपया महिला निधि के लिए सीएलएफ का चयन करें|';
                message += '<br />';
            }
            if ($scope.GopalCreditCardModel.ShgVoID == "" || $scope.GopalCreditCardModel.ShgVoID == null || $scope.GopalCreditCardModel.ShgVoID == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select VO Name for Mahila Nidhi' : '\nकृपया महिला निधि के लिए वीओ का नाम चुनें|';
                message += '<br />';
            }
            if ($scope.GopalCreditCardModel.ShgId == "" || $scope.GopalCreditCardModel.ShgId == null || $scope.GopalCreditCardModel.ShgId == "0") {
                message += $scope._language == 'en-us' ? '\nPlease Select SHG Name for Mahila Nidhi' : '\nकृपया महिला निधि के लिए एसएचजी नाम चुनें|';
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

    $scope.FillGuarantorDetails = function () {
        $scope.GuarantorDetailsModel = [];
        $scope.GuarantorDetailsModel.push({
            id: 0,
            Name: $scope.GopalCreditCardModel.G1Name,
            FatherSpouseName: $scope.GopalCreditCardModel.G1FatherSpouseName,
            DOB: $scope.GopalCreditCardModel.G1DOB,
            Gender: $scope.GopalCreditCardModel.G1Gender,
            JanMemberId: $scope.GopalCreditCardModel.G1JanMemberId,
            JanAadharNumber: $scope.GopalCreditCardModel.G1JanAadharNumber,
            AadharNo: $scope.GopalCreditCardModel.G1AadharNo,
            PANCardNo: $scope.GopalCreditCardModel.G1PANCardNo,
            MobileNo: $scope.GopalCreditCardModel.G1MobileNo,
            Address: $scope.GopalCreditCardModel.G1Address,
            GuarranterLandProperty: $scope.GopalCreditCardModel.G1GuarranterLandProperty,
            Photo: $scope.GopalCreditCardModel.G1Photo,
            Signature: $scope.GopalCreditCardModel.G1Signature,
            IsVerified: $scope.G1DetailFetched
        });

        $scope.GuarantorDetailsModel.push({
            id: 0,
            Name: $scope.GopalCreditCardModel.G2Name,
            FatherSpouseName: $scope.GopalCreditCardModel.G2FatherSpouseName,
            DOB: $scope.GopalCreditCardModel.G2DOB,
            Gender: $scope.GopalCreditCardModel.G2Gender,
            JanMemberId: $scope.GopalCreditCardModel.G2JanMemberId,
            JanAadharNumber: $scope.GopalCreditCardModel.G2JanAadharNumber,
            AadharNo: $scope.GopalCreditCardModel.G2AadharNo,
            PANCardNo: $scope.GopalCreditCardModel.G2PANCardNo,
            MobileNo: $scope.GopalCreditCardModel.G2MobileNo,
            Address: $scope.GopalCreditCardModel.G2Address,
            GuarranterLandProperty: $scope.GopalCreditCardModel.G2GuarranterLandProperty,
            Photo: $scope.GopalCreditCardModel.G2Photo,
            Signature: $scope.GopalCreditCardModel.G2Signature,
            IsVerified: $scope.G2DetailFetched
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
        $scope.GopalCreditCardModel.AgricultureLandDetailList = $scope.AgricultureLandDetailList;
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
                    alert($scope._language == 'en-us' ? 'Your application has been submitted successfully.' : 'आपका आवेदन सफलतापूर्वक सबमिट हो गया है।');
                    //if (StatusId == 2)
                    //    window.location = '../GopalCreditCard/GopalApplicationFormPart2?gccEncId=' + data.Content;
                    //else
                    window.location.reload();
                }
                else if (data.Status == false) {
                    // $scope.Reset_GCCForm();
                    if (data.ErrorMessage == "GCC_APPLICATION_OD") {
                        ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', $scope._language == 'en-us' ? 'The amount in your crop loan account is overdue. Please deposit the overdue amount to avail of the Gopal Credit Card scheme.' : 'आपके फसली ऋण खाते में राशि अवधिपार है। गोपाल केडिट कार्ड योजना का लाभ लेने के लिये अवधिपार राशि जमा करावें।');
                    }
                    if (data.ErrorMessage == "GCC_APPLICATION_RETRY") {
                        ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', $scope._language == 'en-us' ? 'The service is currently unavailable, please try again later.' : 'यह सर्विस अभी उपलब्ध नहीं है, कृपया बाद में फिर से कोशिश करें।');
                    }
                    if (data.ErrorMessage == "GCC_APPLICATION_GURANTOR") {
                        ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', $scope._language == 'en-us' ? 'Please verify the both guarantors' : 'कृपया दोनों गारंटर को वेरिफ़ाई करें।');
                    }
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

                    if (data.ErrorMessage == "GCC_APPLICATION_RENEWAL_ALLOWED") {
                        ShowModelAlert(
                            $scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::',
                            $scope._language == 'en-us' ? 'Your previous Gopal Credit Card application is eligible for renewal. Please proceed to update and resubmit your application.' : 'आपका पूर्व गोपाल क्रेडिट कार्ड आवेदन नवीनीकरण के लिए पात्र है। कृपया आवेदन को अपडेट कर पुनः जमा करें।'
                        );
                    }
                    if (data.ErrorMessage == "GCC_Closer_Application") {
                        ShowModelAlert(
                            $scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::',
                            $scope._language == 'en-us' ? 'Your previous Gopal Credit Card application has been closed. You cannot submit a new application at this time.' : 'आपका पूर्व गोपाल क्रेडिट कार्ड आवेदन बंद हो चुका है। आप इस समय नया आवेदन नहीं कर सकते हैं।'
                        );
                        setTimeout(function () {
                            window.location = '../GopalCreditCard/GopalApplicationForm';
                        }, 2000);
                    }
                    ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', data.ErrorMessage);
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
                    $scope.LoanBankList();
                }
            }).
            error(function (data, status) {
                $scope.CCBDetails = '';
            });
    }


    $scope.DivtringGrid = true;
    $scope.GetFormEditData = function (StatusId, GCCEId, item) {
        $scope.IsFormEdit = true;
        if (StatusId == 34) {
            $scope.IsReApply = true;
        } else {
            $scope.IsReApply = false;
            $scope.IsPacsEditable = false;
        }
        $scope.DistrictList = [];
        $scope.GetDistrictWithoutDivisionList();

        if (StatusId == 1 || StatusId == 12 || StatusId == 13 || StatusId == 14 || StatusId == 34) {
            $http.post(SiteUrl + "/GopalCreditCard/Get_GopalApplicationFormViewData_v1/?GCCEId=" + GCCEId).
                success(function (data, status) {
                    $scope.GopalCreditCardModel = data;
                    if (StatusId == 1 || StatusId == 12 || StatusId == 13 || StatusId == 14 || StatusId == 34)
                        $scope.GopalCreditCardModel.LoanAmountRecommendedByDCS = data.LoanAmountRecommendedByDCS;
                    else
                        $scope.GopalCreditCardModel.LoanAmountRecommendedByDCS = 0;
                    $scope.GGG_Id = data.gcc_id;
                    debugger;
                    if (item.IsEligibleForRenewable) {
                        $scope.GopalCreditCardModel.RenewRefGccId = data.gcc_id;
                        if (StatusId == 12 || StatusId == 13 || StatusId == 14) {
                            $scope.GopalCreditCardModel.StatusId = data.StatusId; $scope.GopalCreditCardModel.gcc_id = data.gcc_id, $scope.GGG_Id = data.gcc_id;
                        }
                        else {
                            $scope.GopalCreditCardModel.StatusId = 0; $scope.GopalCreditCardModel.gcc_id = 0, $scope.GGG_Id = 0;
                        }
                        $scope.GopalCreditCardModel.IsEligibleForRenewable = item.IsEligibleForRenewable;
                    }

                    $scope.GopalCreditCardModel.IsSatisfiedWithInttCalcByPacs = item.IsSatisfiedWithInttCalcByPacs;

                    $scope.GopalCreditCardModel.IsCopyOfDCSPassbookAttached = (data.CopyOfDCSPassbook_Path != '' || data.PBBankAccountNo != '') ? true : false;
                    $scope.GopalCreditCardModel.IsPACSBankAccountAvailable = (data.BankAccountNo != '' || data.BankIFSCCode || data.BankName || data.BankBranchName) ? true : false;
                    if (data.CropLoanDetailsList != null && data.CropLoanDetailsList.length > 0)
                        $scope.CropLoanDetailsList = data.CropLoanDetailsList;
                    if (data.AgricultureLandDetailList != null && data.AgricultureLandDetailList.length > 0)
                        $scope.AgricultureLandDetailList = data.AgricultureLandDetailList;
                    $scope.AgricultureLandDetailList.forEach((v, i) => {

                        v.rowId = i + 1;
                    });
                    // if (data.GuarantorDetailsList[0] != null && data.GuarantorDetailsList.length > 0 ) {
                    if (data.GuarantorDetailsList?.length > 0) {
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
                        if (StatusId == 12 || StatusId == 13 || StatusId == 14) {
                            $scope.G1DetailFetched = false;
                        }
                        else {
                            $scope.G1DetailFetched = data.GuarantorDetailsList[0].IsVerified;
                        }
                    }
                    if (data.GuarantorDetailsList?.length > 0 && data.GuarantorDetailsList[1] != null) {
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
                        if (StatusId == 12 || StatusId == 13 || StatusId == 14) {
                            $scope.G2DetailFetched = false;
                        }
                        else {
                            $scope.G2DetailFetched = data.GuarantorDetailsList[1].IsVerified;
                        }
                    }

                    //$scope.GopalCreditCardModel.MemberPhoto_Base64 = data.MemberPhoto;

                    //$scope.GopalCreditCardModel.LoanDistrictId = String(data.LoanDistrictId);    
                    $scope.GopalCreditCardModel.LoanDistrictId = data.LoanDistrictId ? String(data.LoanDistrictId) : "0";
                    $scope.GopalCreditCardModel.LoanBankID = String(data.LoanBankID);
                    $scope.GopalCreditCardModel.LoanBranchId = String(data.LoanBranchId);
                    $scope.GopalCreditCardModel.PACS_ID = String(data.PACS_ID);
                    $scope.GopalCreditCardModel.PACS_ID_bkp = String(data.PACS_ID);

                    $scope.BankMasterList = [];
                    $scope.PLDBBranches = [];
                    $scope.PACSNameList = [];

                    var districtId = parseInt($scope.GopalCreditCardModel.LoanDistrictId);
                    if (isNaN(districtId)) {
                        districtId = 0;
                    }

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

                    $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=" + districtId)
                        .success(function (banks) {
                            $scope.BankMasterList = banks || [];

                            if ($scope.GopalCreditCardModel.LoanBankID && $scope.GopalCreditCardModel.LoanBankID != '0') {
                                $(".se-pre-con").fadeIn("slow");
                                $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBankBranch/?LoanDistrictId=" + $scope.GopalCreditCardModel.LoanDistrictId + "&LoanBankId=" + $scope.GopalCreditCardModel.LoanBankID)
                                    .success(function (branches) {
                                        $scope.PLDBBranches = branches || [];

                                        if ($scope.GopalCreditCardModel.LoanBranchId && $scope.GopalCreditCardModel.LoanBranchId != '0') {
                                            $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBranch_PACS/?LoanBranchId=" + $scope.GopalCreditCardModel.LoanBranchId)
                                                .success(function (pacs) {
                                                    $scope.PACSNameList = pacs || [];
                                                    setTimeout(function () {
                                                        try {
                                                            document.getElementById('DistId')?.setAttribute('disabled', 'disabled');
                                                            document.getElementById('BankID')?.setAttribute('disabled', 'disabled');
                                                            document.getElementById('BranchId')?.setAttribute('disabled', 'disabled');
                                                            //document.getElementById('PacsId')?.setAttribute('disabled', 'disabled');
                                                        } catch (e) { console.warn(e); }
                                                        $(".se-pre-con").fadeOut("slow");
                                                        $scope.$applyAsync();
                                                    }, 300);
                                                })
                                                .error(function () {
                                                    $scope.PACSNameList = [];
                                                    $(".se-pre-con").fadeOut("slow");
                                                });
                                        } else {
                                            $(".se-pre-con").fadeOut("slow");
                                        }
                                    })
                                    .error(function () {
                                        $scope.PLDBBranches = [];
                                        $(".se-pre-con").fadeOut("slow");
                                    });
                            } else {
                                $(".se-pre-con").fadeOut("slow");
                            }
                        })
                        .error(function () {
                            $scope.BankMasterList = [];
                        });

                    $scope.GopalCreditCardModel.CurrentLoanDistrictId = String(data.CurrentLoanDistrictId);
                    $scope.GopalCreditCardModel.CurrentLoanBankID = String(data.CurrentLoanBankID);
                    $scope.GopalCreditCardModel.CurrentLoanBranchId = String(data.CurrentLoanBranchId);
                    loadCurrentLoanBankList(data);

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


                    if (StatusId == 13 || StatusId == 14) {
                        // $scope.Get_GccDocuments();
                    }
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


    function loadCurrentLoanBankList(data) {
        var districtId = data.CurrentLoanDistrictId;
        if (!districtId || districtId === "0") return;
        $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=" + districtId)
            .success(function (banks) {
                $scope.CurrentBankMasterList = banks || [];
                $scope.GopalCreditCardModel.CurrentLoanBankID = String(data.CurrentLoanBankID);
                loadCurrentLoanBranchList(data);
            });
    }

    function loadCurrentLoanBranchList(data) {
        var bankId = data.CurrentLoanBankID;
        if (!bankId || bankId === "0") return;
        $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBankBranch/?LoanDistrictId="
            + data.CurrentLoanDistrictId
            + "&LoanBankId=" + bankId)
            .success(function (branches) {
                $scope.CurrentPLDBBranches = branches || [];
                $scope.GopalCreditCardModel.CurrentLoanBranchId = String(data.CurrentLoanBranchId);
            });
    }



    $scope.$watch("IsPacsEditable", function (newVal, oldVal) {
        if (newVal === false) {
            if ($scope.GopalCreditCardModel.PACS_ID_bkp) {
                $scope.GopalCreditCardModel.PACS_ID = $scope.GopalCreditCardModel.PACS_ID_bkp;
            }
        }
    });

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

    $scope.PushAgricultureLandDetailList = function (rowId) {

        $scope.AgricultureLandDetailList.push({
            cIndex: 0,
            id: 0,
            Village: '',
            PatwarCircle: '',
            Irrigated: 0,
            NonIrrigated: 0,
            KhasraNumber: '',
            Totalland: 0,
            // AgricultureLandImg:'',
            IsDelete: false,
            rowId: rowId

        });

    }
    $scope.PushAgricultureLandDetailList(1);

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
        $scope.PushAgricultureLandDetailList(cIndex + 1);
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
                $scope.GccDocuments = data.Table.filter(x => x.Type.trim() == 'D' && x.status == 'O');
                $scope.GccObjectionPanel = data.Table.filter(x => x.Type.trim() == 'P' && x.status == 'O');
                $(".se-pre-con").fadeOut("slow");

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
        $http.get(SiteUrl + "/RajeevikaMaster/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.ShgDistrictList = data;
                $scope.GopalCreditCardModel.ShgDistrictId = '0';
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
                $scope.GopalCreditCardModel.ShgBlockID = '0';
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
                $scope.GopalCreditCardModel.ShgClfID = '0';
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
                $scope.GopalCreditCardModel.ShgVoID = '0';
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
                $scope.GopalCreditCardModel.ShgId = '0';
                $(".se-pre-con").fadeOut("slow");

            }).
            error(function (data, status) {
                $scope.SHGList = [];
                $(".se-pre-con").fadeOut("slow");

            });
    };


    $scope.isPurposeofLoan = function () {
        $scope.GopalCreditCardModel.LoanAmountRecommendedByDCS = 0;
        let desiredLoanAmount = $scope.GopalCreditCardModel.DesiredCreditLimitLoanAmount || 0;
        if ($scope.GopalCreditCardModel.IsWorking) {
            $scope.GopalCreditCardModel.LoanAmountRecommendedByDCS = Math.round($scope.GopalCreditCardModel.LoanAmountRecommendedByDCS + (($scope.GopalCreditCardModel.IsEligibleForRenewable || $scope.GopalCreditCardModel.RenewRefGccId > 0) ? desiredLoanAmount * 0.60 : desiredLoanAmount * 0.40));
        }
        if ($scope.GopalCreditCardModel.IsInfrastructure) {
            $scope.GopalCreditCardModel.LoanAmountRecommendedByDCS = Math.round($scope.GopalCreditCardModel.LoanAmountRecommendedByDCS + (($scope.GopalCreditCardModel.IsEligibleForRenewable || $scope.GopalCreditCardModel.RenewRefGccId > 0) ? desiredLoanAmount * 0.25 : desiredLoanAmount * 0.45));
        }
        if ($scope.GopalCreditCardModel.IsApparatus) {
            $scope.GopalCreditCardModel.LoanAmountRecommendedByDCS = Math.round($scope.GopalCreditCardModel.LoanAmountRecommendedByDCS + desiredLoanAmount * 0.15);
        }
    };

    //GCC Esign process

    $scope.DocumentEsign = function (GCCEId, item) {
        $scope.GGG_Id = GCCEId;
        $scope.signedBy = 4;
        $scope.AadharNoEnc = item.AadharNoEnc;
        $scope.esignLocation = item.PACSName_ENG;
        $scope.esignDesignation = "Citizen";
        $scope.esignFileName = item.ApplicantSignedForm;
        $scope.gcceIdEsign = item.GopalCreditCardId;
        //console.log(item)
        //if ($scope.GGG_Id != null && item) {
        //    var url = "/Content/GopalCreditCard/GSSProfiling/" + item.ApplicantSignedForm;
        //    window.open(url, '_blank');
        //}
        //return;
        $http.post(SiteUrl + "/GopalCreditCard/Get_GopalApplicationFormViewData_v1/?GCCEId=" + GCCEId).success(function (data, status) {
            if (data != null) {
                $scope.GopalCreditCardModel = data;
                $scope.DocumentEsignView(GCCEId, item);
                //$scope.GenerateEsignOtp();
            }

        }).error(function (data, status) {
            $scope.GopalCreditCardModel = {};
        });
    };


    $scope.DocumentEsignView = function (GCCEId, item) {
        $scope.GGG_Id = GCCEId;
        $scope.eSignData = item;
        $("#modelPopupDocumentView").modal("show");
    };

    $scope.ViewDocument = function () {
        if ($scope.GGG_Id != null && $scope.eSignData) {
            var url = "/Content/GopalCreditCard/GSSProfiling/" + $scope.eSignData.ApplicantSignedForm;
            window.open(url, '_blank');
        }
        if ($scope.GGG_Id != null && $scope.eSignData && $scope.eSignData.ApplicantSignedFormPacs != null) {
            var url = "/Content/GopalCreditCard/GSSProfiling/" + $scope.eSignData.ApplicantSignedFormPacs;
            window.open(url, '_blank');
        }

    }
    $scope.GCCApplicationESign = function (signedBy, location, designation, fileName, gccId) {
        $scope.showLoader = true;
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/GopalCreditCard/GCCApplicationEsign", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": $scope.ApplicationFlowMaster.TXN, "signedBy": signedBy, "location": location, "designation": designation, "fileName": fileName, "gccid": gccId }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data) {
                    $scope.showLoader = false;
                    $("#modelPopupOTP").modal("hide");
                    $("#modelPopupDocumentView").modal("hide");
                    if (data.status > 0) {
                        ShowOkMessage("Success", _language == 'en-us' ? 'Esign Successfully Done\n' : 'ई-साइन सफलतापूर्वक हो गया\n', function () { window.location.reload(); });
                    }
                    else if (data.status == -1) {
                        ShowOkMessage("Error", _language == 'en-us' ? 'The application cannot be forwarded because there is no user specified \n' : 'आवेदन अग्रेषित नहीं किया जा सकता क्योंकि कोई उपयोगकर्ता निर्दिष्ट नहीं है \n');
                    }
                    else if (data.status == -2) {
                        ShowOkMessage("Error", _language == 'en-us' ? 'Invalid OTP! \n' : 'अवैध ओटीपी!\n');
                    }
                    else if (data.status == -3) {
                        ShowOkMessage("Error", _language == 'en-us' ? 'Duplicate Jan Aadhaar Number  One Jan Aadhaar (Family) -> One Loan \n' : 'डुप्लीकेट जन आधार नंबर एक जन आधार (परिवार) -> एक ऋण\n');
                    }
                    else {

                        $(".se-pre-con").fadeOut("slow");
                        ShowOkMessage("Error", _language == 'en-us' ? 'Some issue at Esign service end \n' : 'ई-साइन सर्विस पर कुछ समस्या है \n');

                    }
                }
            }).error(function (data, status) {
                $scope.showLoader = false;
                $(".se-pre-con").fadeOut("slow");
                alert("Error while insert");
            });
    };

    $scope.GenerateEsignOtp = function () {
        $scope.GopalCreditCardFormModel = $scope.GopalCreditCardModel;
        $scope.signedBy = 4;
        $scope.esignLocation = $scope.eSignData.PACSName_ENG;
        $scope.esignDesignation = "Citizen";
        $scope.esignFileName = $scope.eSignData.ApplicantSignedForm;
        $scope.gcceIdEsign = $scope.eSignData.GopalCreditCardId;
        $scope.esignFileName = $scope.eSignData.ApplicantSignedForm + ($scope.eSignData.ApplicantSignedFormPacs ? '#' + $scope.eSignData.ApplicantSignedFormPacs : '');
        $(".se-pre-con").fadeIn("slow");
        debugger
        $http.get("../ApplicationFlow/GenerateAadhaarOTPForEsignGcc?userType=4&aadharNoEnc=" + encodeURIComponent($scope.AadharNoEnc)).success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            if (data.indexOf('NO#') >= 0) {
                if (data.includes('XXX') || data.includes('Unexpected character')) {
                    alert("Aadhaar Not valid, update Aadhaar First");
                }
                else {
                    alert(data.replace('NO# NA', ''));
                }
                $("#modelPopupOTP").modal("hide");
            } else {
                if (data != "LOGOUT") {
                    $scope.ApplicationFlowMaster.TXN = data;
                    alert(_language == 'en-us' ? 'OTP has been sent to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n');
                    $("#modelPopupOTP").modal("show");
                }
                else {
                    $(".se-pre-con").fadeOut("slow");
                    $("#modelPopupOTP").modal("hide");
                    window.location = 'https://sso.rajasthan.gov.in/signin';
                }
            }
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
            alert(_language == 'en-us' ? 'Session out, Login again \n' : 'कुछ त्रुटि हुई है \n');
        });
    }

    $scope.GetDistrictList();

    $scope.VerifyOTP = function () {
        if (!$scope.ApplicationFlowMaster.OTP) {
            alert("Please enter valid otp");
            return;
        }

        $scope.GCCApplicationESign($scope.signedBy, $scope.esignLocation, $scope.esignDesignation, $scope.esignFileName, $scope.gcceIdEsign);
    };
    $scope.clearOTPModel = function () {
        $("#modelPopupOTP").modal("hide");
        $scope.ApplicationFlowMaster.OTP = '';
    }

    // Added Narpat
    $scope.RedirectToESPportal = function () {
        $scope.eSignData = $scope.eSignData;
        $scope.GGG_Id = $scope.GGG_Id;
        $scope.signedBy = 4;
        $scope.AadharNoEnc = $scope.eSignData.AadharNoEnc;
        $scope.esignLocation = $scope.eSignData.Name_ENG;
        $scope.esignDesignation = "Citizen";
        $scope.esignFileName = $scope.eSignData.ApplicantSignedForm;
        $scope.gcceIdEsign = $scope.eSignData.GopalCreditCardId;
        $scope.GCCApplicationLatter();
    };

    $scope.GCCApplicationLatter = function () {
        $scope.ESignGCCApplication().then(function (response) {
            var data = response.data;
            if (!data.Success) {
                alert("Document generation failed");
                return;
            }

            $scope.esignData = {
                SiteUrl: SiteUrl,
                DocType: data.DocType,
                ReturnPath: data.ReturnPath,
                DocumentPath: data.DocPath,
                DocumentName: data.DocName,
                docid: data.docid,
                SocietyIDEnc: "",
                Remark: $scope.eSignData.Name_ENG,
                StatusId: $scope.signedBy,
                IsAmendment: "",
                AmendmendAmount: "",
                ReceiptNumber: "",
                Location: $scope.esignLocation,
                Desination: $scope.esignDesignation
            };


            if (!$scope.esignData.DocumentPath || !$scope.esignData.DocumentName) {
                alert("Not valid document");
                return;
            }


            EsignCommonData.validateEsign($http, $scope.esignData, function () {
                EsignCommonData.startEsign($http, $scope.esignData, function (xml) {
                    EsignCommonData.redirectToESP(xml);
                });
            });

        }).catch(function (err) {

            console.error(err);
            alert("Error generating eSign certificate");

        });
    };

    $scope.ESignGCCApplication = function () {
        $scope.showLoader = true;
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/GCCESignApplication", {
            signedBy: $scope.signedBy,
            fileName: $scope.esignFileName,
            gccid: $scope.gcceIdEsign
        }).then(function (response) {

            $scope.showLoader = false;
            $(".se-pre-con").fadeOut("slow");
            return response;
        }).catch(function (error) {

            $scope.showLoader = false;
            $(".se-pre-con").fadeOut("slow");
            alert("Error while insert");
            throw error;
        });
    };


    $scope.OnPageLoad();


    $scope.fetchJanAadharDetails = function (type, aadharNo) {
        $scope.MemberDetail = [];
        GetMemberDetails(aadharNo);
        if ($scope.MemberDetail[0].Message == "Success") {
            JanAadhaarAuth("", $scope.MemberDetail[0].AADHAR_REF_NO, "GCCG", "", "", $http, false, "", "", function (data) {

                if (data && Object.keys(data).length > 0) {
                    $(".se-pre-con").fadeOut("slow");
                    var message1 = '';
                    if (!isValidDate(data.DOB)) {

                        message1 = _language == 'en-us' ? 'DOB is incorrect. Please update your Aadhaar. \n' : 'जन्म तिथि गलत है। कृपया अपना आधार अपडेट करें। \n';
                        $(".se-pre-con").fadeOut("slow");
                        alert(message1);
                        return;

                    }

                    if (data.Gender == 'Male')
                        data.Gender = 'M';
                    else if (data.Gender == 'Female')
                        data.Gender = 'F'
                    else
                        data.Gender = 'T';

                    if (type == 1) {
                        $scope.GopalCreditCardModel.G1Name = data.Name;
                        $scope.GopalCreditCardModel.G1Gender = data.Gender;
                        $scope.GopalCreditCardModel.G1JanMemberId = data.MemberId;
                        $scope.GopalCreditCardModel.G1JanAadharNumber = $scope.MemberDetail[0].JAN_AADHAR;
                        $scope.GopalCreditCardModel.G1FatherSpouseName = data.FatherName;
                        $scope.GopalCreditCardModel.G1DOB = new Date(parseDDMMYYYY(data.DOB));
                        $scope.GopalCreditCardModel.G1MobileNo = data.MobileNo;
                        $scope.GopalCreditCardModel.G1Address = data.Address;
                        $scope.GopalCreditCardModel.G1Photo = data.Photo;
                        $scope.GopalCreditCardModel.G1Photo_Base64 = data.Photo;
                        $scope.G1DetailFetched = true;
                    }
                    else {

                        $scope.GopalCreditCardModel.G2Name = data.Name;
                        $scope.GopalCreditCardModel.G2Gender = data.Gender;
                        $scope.GopalCreditCardModel.G2JanMemberId = data.MemberId;
                        $scope.GopalCreditCardModel.G2JanAadharNumber = $scope.MemberDetail[0].JAN_AADHAR;
                        $scope.GopalCreditCardModel.G2FatherSpouseName = data.FatherName;
                        $scope.GopalCreditCardModel.G2DOB = new Date(parseDDMMYYYY(data.DOB));
                        $scope.GopalCreditCardModel.G2MobileNo = data.MobileNo;
                        $scope.GopalCreditCardModel.G2Address = data.Address;
                        $scope.GopalCreditCardModel.G2Photo = data.Photo;
                        $scope.GopalCreditCardModel.G2Photo_Base64 = data.Photo;
                        $scope.G2DetailFetched = true;
                    }

                    ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card' : 'Gopal Credit Card', $scope._language == 'en-us' ? 'OTP has been successfully verified.' : 'ओटीपी सफलतापूर्वक सत्यापित हो गया है।');
                    $("#modelPopupVerifyOTP").modal("hide");
                    $(".se-pre-con").fadeOut("slow");
                }
                else {
                    ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card' : 'Gopal Credit Card', $scope._language == 'en-us' ? 'OTP has not been validated' : 'OTP मान्य नहीं किया गया है');
                    $(".se-pre-con").fadeOut("slow");
                }
            });
        }
        else {
            ShowModelAlert('Gopal Credit Card', $scope.MemberDetail[0].Message);
            $(".se-pre-con").fadeOut("slow");
        }
    }
    function GetMemberDetails(JanaadhaarNo) {
        $.ajax({
            url: "../GopalCreditCard/GetGuarantorJanAadhaarDetails",
            method: "POST",
            data: {
                JanaadhaarNo: encodeURIComponent(encryptData(JanaadhaarNo)), loanBankID: $scope.GopalCreditCardModel.LoanBankID
            },
            dataType: "json",
            async: false,
            success: function (data1) {
                if (Object.keys(data1).length == 0) {
                    var message = _language == 'en-us' ? 'Invalid Aadhaar Number \n' : 'अमान्य आधार संख्या \n';
                    alert(message);
                    $scope.MemberDetail = [];
                }
                else {
                    if (data1[0].msg === "Unexpected JSON token when reading DataTable. Expected StartArray, got StartObject. Path '', line 1, position 1.") {
                        var message = _language == 'en-us' ? 'Invalid Aadhaar Number \n' : 'अमान्य आधार संख्या \n';
                        alert(message);
                        $scope.MemberDetail = [];
                    }
                    else {
                        $scope.MemberDetail = data1;

                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //window.location = "~/Error/General";
            }
        });
    }
    function parseDDMMYYYY(dateStr) {
        if (!dateStr) return null;

        var parts = dateStr.split('/'); // DD/MM/YYYY
        return parts[2].toString() + '-' + parts[1].toString() + '-' + parts[0].toString();
    }
    function isValidDate(dateStr) {
        if (!dateStr || dateStr.trim() === '') {
            return false; // null, undefined, or blank
        }
        if (dateStr == "01-01-1900" || dateStr === "01/01/1900") { return false; }

        dateStr = dateStr.replace(/\//g, '-');
        const [day, month, year] = dateStr.split('-').map(Number);

        // Check basic structure
        if (!day || !month || !year) return false;

        const date = new Date(year, month - 1, day); // JS months are 0-based

        return (
            date.getFullYear() === year &&
            date.getMonth() === month - 1 &&
            date.getDate() === day
        );
    }

});
AppTrainning.filter('DEC', function () {
    return function (input) {
        if (!input)
            return input;
        return decryptData(input);
    };
});
