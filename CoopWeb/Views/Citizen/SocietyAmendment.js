
var MasterMod = angular.module('SocietyAmendmentModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("SocietyAmendmentCont", function ($scope, $http) {
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.ShowTab = false;
    $scope.ByLaws = [];
    $scope.ActId = 0;
    $scope.SocietyDirectory = "";
    $scope.SocietyIdEnc = "";
    $scope.MinimMemeber = 5;
    $scope.GetSocietyDetails = true;
    $scope.MemberRegistration = {};
    $scope.SocietyAmendment = { AmendmentSection: 0 };
    //$scope.ShowTab = false;

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

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get("../Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("../Translations/Translation_HI.json").
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

        $scope.DistrictList();
        $scope.showLoader = false;
        $scope.GetSocietyList();
        $scope.GetAmendmentHistory();
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
            success(function (data, status) {
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
        //Get Category List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=-1").
            success(function (data, status) {
                $scope.CategoryList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });
        //Get Post List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=10").
            success(function (data, status) {
                $scope.Posts = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });

        //$(".se-pre-con").fadeOut("slow");
    };

    // START : Get Amendment History
    $scope.AmendmentHistoryList = [];
    $scope.GetAmendmentHistory = function () {
        $http.post(SiteUrl + "/Society/GetAmendmentHistory").
            success(function (data, status) {
                $scope.AmendmentHistoryList = data.DataList;
            }).
            error(function (data, status) {
                $scope.AmendmentHistoryList = [];
            });
    }
    // END: Get Amendment History


    $scope.ChooseSociety = function (item) {
        $scope.SocietyAmendment = { AmendmentSection: 4 };
        $scope.SocietyAmendment.AmendmentSection == 4;
        $scope.ChangeAmendmentSection(); $scope.checkNameChange();
        var sData = $scope.SocietyList.filter(function (x) { return x.SocietyID == item.SocietyID })[0];
        $scope.SocietyAmendment.SocietyIdEnc = sData.SocietyIdENC + "~" + sData.SocietyDirectory + "~" + sData.ActId;
        $scope.OnChangeSociety(item.ChangeID);
    }

    // START : Get Category  List
    $scope.CategoryList = [];
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
    $scope.SelectSocietyForVerify = function (item, index) {
        //alert(SocietyID);
        $scope.SocietyRegdata.forEach(function (item1, index) {
            item1.ischecked = false;
        });
        item.ischecked = true;
        var MessageHeading = _language === 'en-us' ? "User Society Mapping" : 'उपयोगकर्ता सोसायटी मानचित्रण';
       
        var data = item;
        $scope.SocietyId = data.SocietyID;
        $scope.SocietyAmendment.SocietyIdEnc = data.SocietyIDEnc + '~' + data.SocietyDirectory + '~' + data.ActId;

        if (item.IsSsoFound === true && item.IsSameSSO === false) {

        } else if (item.IsSsoFound === false) {

        } else {
           // $scope.SocietyAmendment.AmendmentSection == 4
            $scope.ChooseSociety($scope.SocietyId);
           // $scope.OnChangeSociety()
        }

            $scope.IsSearchResult = false;
            $scope.IsShowProfiling = false;
            $scope.IsShowAmendment = false;
            $scope.IsShowSSOMAP = false;
            $scope.IsShowSSOMAPNoMAP = true
            $scope.GetSocietyDetails = false;
            $scope.IsWaitingForApproval = false;
            //$scope.SocietyRegdataSelected.push(item);
            //$scope.SocietyIDEncget = $scope.SocietyRegdataSelected[0].SocietyIDEnc;
            //$scope.$apply();
        //} else {
        //    ShowAlertMessage("", "आपके द्वारा चयन की गयी सोसाइटी आपकी SSO ID से पहले से मैप हैं। क्यां आप अपनी सोसाइटी की विस्तृत जानकारी देखना चाहते हैं ? ", function () {
        //        window.location = "../Citizen/SocietyApplicationForm";
        //    });

        //}
    }
    $scope.RemovalTypeList = [{ CODENAME: 'DEATH', CODENAME_MANGAL: 'DEATH', CODEID: '1' }, { CODENAME: 'REMOVEL', CODENAME_MANGAL: 'REMOVEL', CODEID: '3' }, { CODENAME: 'RESIGNATION', CODENAME_MANGAL: 'RESIGNATION', CODEID: '3' }]
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
        //$scope.MemberRegistration.SocietyIDEnc = $scope.SocietyIDEncget;
        $(".se-pre-con").fadeIn("slow");
        if (!$scope.ValidateSocietyMappingRequestData(flag)) {
            $(".se-pre-con").fadeOut("slow");
            return;
        }

        $scope.MemberRegistration.DOB = $scope.MemberRegistration.DOB.split('-')[2] + "/" + $scope.MemberRegistration.DOB.split('-')[1] + "/" + $scope.MemberRegistration.DOB.split('-')[0]

        $scope.MemberRegistration.FileName = $scope.GetFileUploaded('MapplingFileAtt');
        $http.post(SiteUrl + "/Citizen/InsertAmendmentSenction4", { model: $scope.MemberRegistration, Flag: flag }).
            success(function (data, status) {
                //alert(parseInt(data, 10));
                $(".se-pre-con").fadeOut("slow");
                var title = _language === 'en-us' ? 'Society Mapping Request' : 'सोसाइटी मानचित्रण अनुरोध';
                if (parseInt(data, 10) > 0) {
                    //ShowOkMessage("", _language === 'en-us' ? 'Society Mapping Request accepted successfully.' : 'सोसाइटी मैपिंग अनुरोध सफलतापूर्वक स्वीकार किया गया', function () {
                    $scope.GetSocietyList().then(function () {

                        var sData = $scope.SocietyList.filter(function (x) { return x.SocietyID == $scope.SocietyId })[0];
                        $scope.SocietyAmendment.SocietyIdEnc = sData.SocietyIdENC + "~" + sData.SocietyDirectory + "~" + sData.ActId;
                        $scope.OnChangeSociety();

                    });
                    //});
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



    $scope.RuralUrbanBlock = function (V) {
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
    //start BlockListByOffAddress form
    $scope.BlockListByOffAdd = function (id) {
        if ($scope.SocietyRegisteredOffice.DISTRICT_ID != '0' && id == 1) {
            $scope.Blocksdata = [];
            $scope.GramPanchayats = [];
            $scope.Villages = [];
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
                success(function (data, status) {
                    $scope.Blocksdata = data;
                }).
                error(function (data, status) {
                    $scope.Blocksdata = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.DISTRICT_ID_copy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID_copy).
                success(function (data, status) {
                    $scope.Blocksdatacopy = data;
                }).
                error(function (data, status) {
                    $scope.Blocksdatacopy = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.DISTRICT_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_IDSecretary).
                success(function (data, status) {
                    $scope.BlocksdataSecretary = data;
                }).
                error(function (data, status) {
                    $scope.BlocksdataSecretary = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer).
                success(function (data, status) {

                    $scope.BlocksdataTreasurer = data;

                }).
                error(function (data, status) {
                    $scope.BlocksdataTreasurer = [];
                });
        }

    };
    //end
    $scope.GetCityListByCityTypes = function (id) {
        if ($scope.SocietyRegisteredOffice.DISTRICT_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
                success(function (data, status) {
                    $scope.CityListdata = data;
                }).
                error(function (data, status) {
                    $scope.CityListdata = [];
                });
        }

        if ($scope.SocietyRegisteredOffice.DISTRICT_ID_copy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.SocietyRegisteredOffice.DISTRICT_ID_copy).
                success(function (data, status) {
                    $scope.CityListdatacopy = data;
                }).
                error(function (data, status) {
                    $scope.DistrictListcopy = [];
                });
        }
        if ($scope.SocietyRegisteredOffice.DISTRICT_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.SocietyRegisteredOffice.DISTRICT_IDSecretary).
                success(function (data, status) {
                    $scope.CityListdataSecretary = data;
                }).
                error(function (data, status) {
                    $scope.CityListdataSecretary = [];
                });
        }
        if ($scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.SocietyRegisteredOffice.DISTRICT_IDTreasurer).
                success(function (data, status) {
                    $scope.CityListdataTreasurer = data;
                }).
                error(function (data, status) {
                    $scope.CityListdataTreasurer = [];
                });
        }

        // $scope.RuralUrbanBlock();
    };
    //END : Get City List

    $scope.GetDistrictList = function (id) {
        if ($scope.SocietyRegisteredOffice.DIVISION_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyRegisteredOffice.DIVISION_ID).
                success(function (data, status) {
                    $scope.DistrictList = data;

                }).
                error(function (data, status) {
                    $scope.DistrictList = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.DIVISION_ID_copy != '0' && id == 2) {

            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyRegisteredOffice.DIVISION_ID_copy).
                success(function (data, status) {
                    $scope.DistrictListcopy = data;

                }).
                error(function (data, status) {
                    $scope.DistrictListcopy = [];
                });

        }
        else if ($scope.SocietyRegisteredOffice.DIVISION_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyRegisteredOffice.DIVISION_IDSecretary).
                success(function (data, status) {
                    $scope.DistrictListSecretary = data;
                }).
                error(function (data, status) {
                    $scope.DistrictListSecretary = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.DIVISION_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyRegisteredOffice.DIVISION_IDTreasurer).
                success(function (data, status) {
                    $scope.DistrictListTreasurer = data;
                }).
                error(function (data, status) {
                    $scope.DistrictListTreasurer = [];
                });
        }

    };
    $scope.ResetDataForAddress = function (val, Level) {
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

    $scope.GetApplicationFlowSociety = function (SocietyIDEnc, AmendmentId) {
        $scope.showLoader = true;
        $http.get("../ApplicationFlow/GetApplicationFlowSocietyAmendment1958/?SocietyIDEnc=" + SocietyIDEnc + "&AmendmentId=" + AmendmentId).
            success(function (data, status) {
                $scope.ApplicationFlowSociety = data;
                // $scope.BaseUserId = data.filter(x => x.StatusId == 3)[0].UserId;
                //  alert(($scope.BaseUserId));
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

    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
        success(function (data, status) {
            $scope.Divisions = data;
        }).
        error(function (data, status) {
            $scope.Divisions = [];
        });

    // START : Get List of GramPanchayat
    $scope.GramPanchayatList = function (id) {

        if ($scope.SocietyRegisteredOffice.BLOCK_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegisteredOffice.BLOCK_ID).
                success(function (data, status) {

                    $scope.GramPanchayats = data;
                }).
                error(function (data, status) {
                    $scope.GramPanchayats = [];

                });
        }
        else if ($scope.SocietyRegisteredOffice.BLOCK_ID_copy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegisteredOffice.BLOCK_ID_copy).
                success(function (data, status) {

                    $scope.GramPanchayatscopy = data;
                }).
                error(function (data, status) {
                    $scope.GramPanchayatscopy = [];

                });
        }
        else if ($scope.SocietyRegisteredOffice.BLOCK_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegisteredOffice.BLOCK_IDSecretary).
                success(function (data, status) {

                    $scope.GramPanchayatsSecretary = data;
                }).
                error(function (data, status) {
                    $scope.GramPanchayatsSecretary = [];

                });
        }
        else if ($scope.SocietyRegisteredOffice.BLOCK_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegisteredOffice.BLOCK_IDTreasurer).
                success(function (data, status) {

                    $scope.GramPanchayatsTreasurer = data;
                }).
                error(function (data, status) {
                    $scope.GramPanchayatsTreasurer = [];

                });
        }


    };
    // END : Get List of GramPanchayat
    //Start

    // START : Get List of Villages
    $scope.VillageList = function (id) {
        if ($scope.SocietyRegisteredOffice.GP_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegisteredOffice.GP_ID).
                success(function (data, status) {
                    $scope.Villages = data;
                }).
                error(function (data, status) {
                    $scope.Villages = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.GP_IDcopy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegisteredOffice.GP_IDcopy).
                success(function (data, status) {
                    $scope.Villagescopy = data;

                }).
                error(function (data, status) {
                    $scope.Villagescopy = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.GP_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegisteredOffice.GP_IDSecretary).
                success(function (data, status) {
                    $scope.VillagesSecretary = data;

                }).
                error(function (data, status) {
                    $scope.VillagesSecretary = [];
                });
        }

        else if ($scope.SocietyRegisteredOffice.GP_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.SocietyRegisteredOffice.GP_IDTreasurer).
                success(function (data, status) {
                    $scope.VillagesTreasurer = data;

                }).
                error(function (data, status) {
                    $scope.VillagesTreasurer = [];
                });
        }

    };
    // END : Get List of Villages
    // START : Get Ward List
    $scope.GetWardList = function (id) {
        if ($scope.SocietyRegisteredOffice.CITY_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.SocietyRegisteredOffice.CITY_ID).
                success(function (data, status) {
                    $scope.Ward = data;
                }).
                error(function (data, status) {
                    $scope.Ward = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.CITY_IDcopy != '0' && id == 2 && $scope.SocietyRegisteredOffice.CITY_IDcopy != undefined) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.SocietyRegisteredOffice.CITY_IDcopy).
                success(function (data, status) {
                    $scope.Wardcopy = data;
                }).
                error(function (data, status) {
                    $scope.Wardcopy = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.CITY_IDSecretary != '0' && $scope.SocietyRegisteredOffice.CITY_IDSecretary != undefined && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.SocietyRegisteredOffice.CITY_IDSecretary).
                success(function (data, status) {
                    $scope.WardSecretary = data;
                }).
                error(function (data, status) {
                    $scope.WardSecretary = [];
                });
        }

        else if ($scope.SocietyRegisteredOffice.CITY_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.SocietyRegisteredOffice.CITY_IDTreasurer).
                success(function (data, status) {
                    $scope.WardTreasurer = data;
                }).
                error(function (data, status) {
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
    $scope.GetSocietyList = function () {
        $(".blockUI").fadeIn("slow");

        var amendmenttyp = window.location.href.indexOf('AmendmentSectionFour') > -1 ? '4' : '12';

        return $http.post(SiteUrl + "/Citizen/GetSocietieForAmendment?ActId=1&AmendmentType=" + amendmenttyp).
            success(function (data, status) {
                //$scope.SocietyList = data.filter(x => (x.IsSameCEOAddress == 'True') && (x.AddressFor == '0'));
                $scope.SocietyList = [];// data;
                var data1 = JSON.parse(data);
                if (data1.length <= 0 && amendmenttyp=='12') {
                    ShowAlertMessage("Alert", _language == 'en-us' ? "Your SSO ID " + $("#hdncurrentSSOID").val() + " is not mapped with any society. Do you want to map a society with your SSO ID?" : "आपकी SSO ID " + $("#hdncurrentSSOID").val() + " किसी भी सोसाइटी के साथ मैप नहीं की गई है। क्यां आप अपनी SSO ID को किसी सोसाइटी से मैप करना चाहते हैं?", function () {
                        window.location = "../Citizen/UserSocietyMappingValid";
                    });
                } else {

                    $scope.SocietyList = data1;

                }
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
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
                return $http.post(SiteUrl + "/Society/SocietyDetailsByAmendmentSocietyId", { "SocietyId": SocietyIdEnc, "EMitraTransactionId": 0 }).
                    success(function (data, status) {
                        var ALLMEMBERS = [];
                        ALLMEMBERS = data.filter(x => (x.IsSameCEOAddress == true) && (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS'));
                        var TableData10All = [];
                        $scope.TableData10AllCopy = [];
                        var startID = 1;
                        var X = 7;
                        X = (ALLMEMBERS.length > 4 ? ALLMEMBERS.length : 5);
                        for (var x = 0; x < ALLMEMBERS.length; x++) {

                            if (TableData10All.filter(function (a) { return a.Name == ALLMEMBERS[x].Name && a.Aadh == ALLMEMBERS[x].AadharNo && a.FatherName == ALLMEMBERS[x].FatherName }).length <= 0) {
                                if (TableData10All.filter(function (C) { return C.AdministrativeId == ALLMEMBERS[x].AdministrativeId }).length <= 0) {
                                    TableData10All.push(
                                        {
                                            "Id": x + startID,
                                            "Name": ALLMEMBERS[x].Name,
                                            "FatherName": ALLMEMBERS[x].FatherName,
                                            "POSTNAME": ALLMEMBERS[x].POSTNAME,
                                            "Aadh": ALLMEMBERS[x].AadharNo,
                                            "Gender": ALLMEMBERS[x].Gender == "F" ? 'F' : 'M',
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
                                            "OldAdministrativeId": ALLMEMBERS[x].AdministrativeId,
                                            "AadharNo2": ALLMEMBERS[x].AadharNo2,
                                            "RemovalType": "0"
                                        });
                                }
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
                                        "OldAdministrativeId": 0,
                                        "AadharNo2": "",
                                        "RemovalType": "0"
                                    });
                            }
                        }



                        $scope.TableData10All = TableData10All;

                        $scope.TableData10AllCopy = [];

                        TableData10All.forEach(function (itm, indx) {
                            if (TableData10All.filter(function (a) { return a.Name == itm.Name && a.Aadh == itm.AadharNo && a.FatherName == itm.FatherName }).length <= 0) {
                                $scope.TableData10AllCopy.push(
                                    {
                                        "Id": indx,
                                        "Name": itm.Name,
                                        "FatherName": itm.FatherName,
                                        "POSTNAME": itm.POSTNAME,
                                        "Aadh": itm.AadharNo,
                                        "Gender": itm == "F" ? 'F' : 'M',
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
                                        "OldAdministrativeId": itm.AdministrativeId,
                                        "AadharNo2": itm.AadharNo2,
                                        "RemovalType": itm.RemovalType
                                    }
                                );
                            }
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
    $scope.OnChangeSociety123 = function (item) {
        $(".se-pre-con").fadeIn("slow");


        var data = $scope.SocietyList.filter(function (x) { return x.SocietyIdENC.toString() === $scope.SocietyAmendment.SocietyIdEnc.split("~")[0] });

        $scope.ShowTab = true;
        $scope.IsSocietyEdit = data[0].IsAmendment == 'True' ? false : true;
        $scope.IsEditApplication = $scope.IsSocietyEdit;

        $scope.SocietyAmendment.TotalMember = data[0].TotalMember;

        $scope.GetMemberList().then(function () {
            if (!$scope.IsSocietyEdit) {
                $scope.GetSocietyNameChangeList().then(function () {
                    $(".se-pre-con").fadeOut("slow");
                });
            } else {
                $(".se-pre-con").fadeOut("slow");
            }
        });





    };

    $scope.OnChangeSociety = function (AmendmentId) {
        $(".se-pre-con").fadeIn("slow");

        var data = $scope.SocietyList.filter(function (x) { return x.SocietyIdENC.toString() === $scope.SocietyAmendment.SocietyIdEnc.split("~")[0] });

        if (data[0].MaxAppliFlowStatusID == 17 || data[0].MaxAppliFlowStatusID == 26) {
            $scope.ShowTab = true;
            $scope.IsSocietyEdit = !data[0].IsAmendment //== 'True' ? false : true;
            $scope.IsEditApplication = $scope.IsSocietyEdit;

            $scope.SocietyAmendment.TotalMember = data[0].TotalMember;
            if (AmendmentId != undefined) {
                $scope.GetApplicationFlowSociety($scope.SocietyAmendment.SocietyIdEnc.split("~")[0], AmendmentId);
            }
            $scope.GetMemberList().then(function () {
                if (!$scope.IsSocietyEdit) {
                    $scope.GetSocietyNameChangeList().then(function () {
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
            ShowOkMessage("Warning", _language == 'en-us' ? "Profiling process of the selected society has not been completed. You can amend the society only when its profiling is complete and it is approved by the concerned office." : "आपके द्वारा चयन की गयी सोसाइटी की प्रोफाइलिंग प्रक्रिया का अनुमोदन राजसहाकर पोर्टल पर पूर्ण नहीं हुवा | आप सोसाइटी में संशोधन तभी कर सकते हैं जब इसकी प्रोफाइलिंग पूर्ण हो व यह सम्बंधित कार्यालय से अनुमोदित हो |  ", function () { });

        }





    };
    // START :Get List of ByLaws List
    $scope.GetByLawsList = function () {
        // $(".se-pre-con").fadeIn("slow");
        $scope.ResetByLawsModal();
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            // alert(SocietyIdEnc);
            return $http.get("../Citizen/GetByLawsList?SocietyIdEnc=" + SocietyIdEnc).
                success(function (data, status) {
                    $scope.ByLawsListData = data;
                    // $(".se-pre-con").fadeOut("slow");
                    //alert(JSON.stringify($scope.ByLawsListData));
                }).
                error(function (data, status) {
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
    $scope.GetAGM_BODMList = function () {
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            //alert(SocietyIdEnc);
            $http.get("../Citizen/GetAGM_BODMList?SocietyIdEnc=" + SocietyIdEnc).
                success(function (data, status) {
                    $scope.AGMListData = data.filter(x => x.Is_AGM_BODM == 'True');
                    $scope.BODMListData = data.filter(x => x.Is_AGM_BODM == 'False');
                }).
                error(function (data, status) {
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
    $scope.GetMemberAmendmentBySocietyId = function () {
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
            return $http.post(SiteUrl + "/Society/GetMemberListAmendmentBySocietyId", { "SocietyId": SocietyIdEnc }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.MemberAmendmentList = [];
                    //$scope.MemberAmendmentList = data;

                    data.forEach(function (item, index) {
                        if ($scope.MemberAmendmentList.filter(function (x) { return x.Name == item.Name && x.FatherName == item.FatherName && x.Aadh == item.Aadh }).length <= 0) {


                            if ($scope.MemberAmendmentList.filter(function (x) { return x.AdministrativeId == item.AdministrativeId }).length <= 0) {
                                if (item.RequestType == 1) {
                                    item.IsDelete = true;
                                }
                                $scope.MemberAmendmentList.push(item);
                            }
                        }
                    });

                    //$scope.MemberAmendmentList.forEach(function (item, indx) {
                    //    if (item.RequestType == 1) {
                    //        //$scope.FileCollection.push({ 'Id': 'flRemovalType_' + indx, 'FileType': '', 'File': item.RemovalFile, 'Title': '', FileName: item.RemovalFile, FileId: 'flRemovalType_' + indx, IsFileUpload: true });
                    //        item.IsDelete = true;
                    //        //item.NewAdministrativeId = item.AdministrativeId;
                    //        //item.RemovalType = item.RemovalType.toString();
                    //    }
                    //});
                }).
                error(function (data, status) {
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
    $scope.GetSocietyNameChangeList = function (AmendmentId) {
        //$(".se-pre-con").fadeIn("slow");


        $scope.ResetByLawsModal();



        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            // alert(SocietyIdEnc);
            return $http.get("../Citizen/GetSocietyNameChangeList?SocietyIdEnc=" + SocietyIdEnc + "&AmendmentId=" + AmendmentId).
                success(function (data, status) {

                    var dataAll = JSON.parse(data.DataList);

                    $scope.SocietyNameChangeList = dataAll.Table;
                    var documents = dataAll.Table1;
                    $scope.allDisable = true;
                    $scope.FileCollection = [];

                    documents.filter(function (x) { return x.Type!='Other' }).forEach(function (item, index) {
                        $scope.FileCollection.push({ 'Id': item.DocumentType, 'FileType': '', 'File': item.DocumentURL, 'Title': '', FileName: item.DocumentURL, FileId: item.DocumentType, IsFileUpload: true });
                    });


                    documents.filter(function (x) { return x.Type == 'Other' }).forEach(function (item, index) {
                         $scope.SocietyOthersDocuments.push({ Id: 'otherfiles_' + $scope.SocietyOthersDocuments.length + 1, Name: item.DocumentURL, FileName: item.DocumentType, FileId: 'otherfiles_' + $scope.SocietyOthersDocuments.length + 1, IsFileUpload: true });
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
                error(function (data, status) {
                    $scope.ByLawsListData = [];
                    //$(".se-pre-con").fadeOut("slow");
                    alert('Error while fetching society list data.');
                });
        } else {
            return 1;
        }
    };



    $scope.GetChangeSocietyMemberExcelList = function () {
        $(".se-pre-con").fadeIn("slow");


        $scope.ResetByLawsModal();



        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            // alert(SocietyIdEnc);
            $http.get("../Citizen/GetChangeSocietyMemberExcelList?SocietyIdEnc=" + SocietyIdEnc).
                success(function (data, status) {
                    $scope.SocietyMemberExcelList = data;
                    $(".se-pre-con").fadeOut("slow");
                    //alert(JSON.stringify($scope.ByLawsListData));
                }).
                error(function (data, status) {
                    $scope.ByLawsListData = [];
                    $(".se-pre-con").fadeOut("slow");
                    alert('Error while fetching society list data.');
                });
        }
    };

    // START :Get List of BalanceSheet 
    $scope.GetBalanceSheet = function () {
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            var ActId = Datasplit[2];
            $http.get("../Citizen/GetBalanceSheetCurrent?SocietyIdEnc=" + SocietyIdEnc).
                success(function (data, status) {
                    $scope.BalanceSheetOld = data;
                }).
                error(function (data, status) {
                    $scope.BODMListData = [];
                    $scope.AGMListData = [];
                    alert('Error while fetching society list data.');
                });
        }
    };
    // END :Get List of BalanceSheet 
    // START :Get List of BalanceSheetFileListData
    $scope.BalanceSheetFileList = function () {
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            $http.get("../Citizen/GetBalanceSheetAllList?SocietyIdEnc=" + SocietyIdEnc).
                success(function (data, status) {
                    $scope.BalanceSheetFileListData = data;
                }).
                error(function (data, status) {
                    $scope.BODMListData = [];
                    $scope.AGMListData = [];
                    alert('Error while fetching society list data.');
                });
        }
    };
    // END :Get List of BalanceSheetFileListData

    $scope.UploadBylawsFile = function (file) {
        $scope.ByeLawsSocietyFile = file[0];
    };
    $scope.UploadResolutionFile = function (file) {
        $scope.ResolutionFile = file[0];
    };
    $scope.UploadAGM1File = function (file) {
        $scope.AGM1File = file[0];
    };
    $scope.UploadAGM2File = function (file) {
        $scope.AGM2File = file[0];
    };
    $scope.UploadAGM3File = function (file) {
        $scope.AGM3File = file[0];
    };
    $scope.UploadAGM_PDF = function (file) {
        $scope.AGM_PDF = file[0];
    };
    $scope.UploadBSheetFileExl = function (file) {
        $scope.InsertBSheetFileExl = file[0];
    }
    $scope.UploadBODAGM_PDF = function (file) {
        $scope.BODAGM_PDF = file[0];
    };

    $scope.UploadMembersListExcelFile = function (file) {
        $scope.MembersListExcelFile = file[0];
    }


    $scope.SaveSocietyMemberExcel = function () {
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
            success(function (data, status) {
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
                    ShowOkMessage("", AltMsg, function () {
                        location.reload();
                    });

                }
                else if (Result == -2) {
                    AltMsg = (_language == 'en-us' ? 'Data Already Exist.' : 'डेटा पहले से मौजूद है');
                    ShowOkMessage("Error", AltMsg, function () { });
                    return;
                }
                else {
                    AltMsg = (_language == 'en-us' ? 'Some Error Occurred please try after some time.' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                    ShowOkMessage("Error", AltMsg, function () { });
                    return;
                }
                //alert(AltMsg);

            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                //alert('An Error Occurred Please Try Again Later');
                ShowOkMessage("Error", 'An Error Occurred Please Try Again Later', function () { });
                location.reload();
            });
    };


    $scope.ChangeAmendmentSection = function () {
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
    $scope.SaveByLaws1234 = function (IsPermanentSave) {
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
                                || x.CategoryId != itm.CategoryId || x.POSTNAME != itm.POSTNAME || x.Occupation != itm.Occupation || x.Status != itm.Status || x.AadharNo2 != itm.Aadh)
                        }).length > 0 || $scope.MemberAmendmentList.filter(function (x) { return x.AadharNo2 == itm.AadharNo2 && x.RequestType == 2 }).length > 0) {
                            SocietyRegistrationData.push(
                                {
                                    "AdministrativeId": itm.AdministrativeId,// itm.NewAdministrativeId == undefined ? 0 : itm.NewAdministrativeId,
                                    "OldAdministrativeId": itm.OldAdministrativeId,
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
                                    "AdministrativeId": itm.AdministrativeId,// itm.NewAdministrativeId == undefined ? 0 : itm.NewAdministrativeId,
                                    "OldAdministrativeId": itm.OldAdministrativeId,
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
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                var AltMsg = "";
                var len = JSON.stringify(data).length;
                if (len > 10) {
                    if (data.data.indexOf("NotValidName") > -1) {
                        $(".SaveSocietyInfo").show();
                        $(".se-pre-con").fadeOut("slow");
                        var ValidNameCheck = "आप जिस सोसाइटी के नाम में संसोधन कर रहे हैं उस सोसाइटी के नाम में कुछ शब्द है जो मान्य नहीं हैं | कृपया सोसाइटी का नाम सही करके फिर से आवेदन करें | अमान्य शब्द " + data.data.replace("NotValidName", "");
                        ShowOkMessage("Warning", ValidNameCheck, function () { return; });
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
                $(".memberAlldata").removeClass("redborder");
                $scope.TableData10All.forEach(function (itm, indx) {
                        if (itm.Aadh == '' || itm.Aadh == undefined || itm.Aadh == null) {
                            Msg += (_language == 'en-us' ? 'Please enter member aadhar number and validate \n' : 'Please enter member aadhar number and validate\n');
                            Msg += '<br />';
                            $("#memberauth_" + indx).addClass("redborder");
                        }
                        if (itm.ContactNo == '' || itm.ContactNo == undefined || itm.ContactNo == null) {
                            Msg += (_language == 'en-us' ? 'Please enter member ContactNo number \n' : 'Please enter member ContactNo number\n');
                            Msg += '<br />';
                            $("#membercontact_" + indx).addClass("redborder");
                        }
                        if (itm.Occupation == '' || itm.Occupation == undefined || itm.Occupation == null) {
                            Msg += (_language == 'en-us' ? 'Please enter member Occupation \n' : 'Please enter member Occupation\n');
                            Msg += '<br />';
                            $("#memberoccupation_" + indx).addClass("redborder");
                        }
                        if (itm.POSTNAME == '' || itm.POSTNAME == undefined || itm.POSTNAME == null) {
                            Msg += (_language == 'en-us' ? 'Please enter member POST NAME \n' : 'Please enter member POST NAME\n');
                            Msg += '<br />';
                            $("#memberpost_" + indx).addClass("redborder");
                    }
                    if (itm.CategoryId == '0' ||itm.CategoryId == '' || itm.CategoryId == undefined || itm.CategoryId == null) {
                        Msg += (_language == 'en-us' ? 'Please enter member Category \n' : 'Please enter member Category\n');
                        Msg += '<br />';
                        $("#memberpost_" + indx).addClass("redborder");
                    }

                    if (itm.IsNew && itm.IsValidate == false) {
                        Msg += (_language == 'en-us' ? 'Please validate aadhar number \n' : 'Please validate aadhar number\n');
                        Msg += '<br />';
                        $("#memberauth_" + indx).addClass("redborder");
                    }


                   

                    });
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

                    if (itm.Name != '' && itm.Name != undefined && itm.Aadh != '' && itm.Aadh != undefined) {
                        SocietyRegistrationData.push(
                            {
                                "AdministrativeId": itm.AdministrativeId,// itm.NewAdministrativeId == undefined ? 0 : itm.NewAdministrativeId,
                                "OldAdministrativeId": itm.OldAdministrativeId,
                                "AADHAARNo": itm.AadharNo2 == undefined || itm.AadharNo2 == "" ? itm.Aadh : itm.AadharNo2,
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
                                "RequestType": itm.IsDelete ? 1 : itm.IsNew === true ? 2 : 3,
                                "RemovalType": itm.RemovalType,
                                "RemovalFile": itm.IsDelete ? $scope.GetFileUploaded('flRemovalType_' + indx) : ''
                            });
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
            CITY_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural == '1' && $scope.SocietyRegisteredOffice.CITY_ID != undefined ? $scope.SocietyRegisteredOffice.CITY_ID : "0",
            WARD_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural == '1' && $scope.SocietyRegisteredOffice.WARD_ID != undefined ? $scope.SocietyRegisteredOffice.WARD_ID : "0",
            BLOCK_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural != '1' && $scope.SocietyRegisteredOffice.BLOCK_ID != undefined ? $scope.SocietyRegisteredOffice.BLOCK_ID : "0",
            GP_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural!= '1' && $scope.SocietyRegisteredOffice.GP_ID != undefined ? $scope.SocietyRegisteredOffice.GP_ID : "0",
            VILLAGE_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural != '1' && $scope.SocietyRegisteredOffice.VILLAGE_ID != undefined ? $scope.SocietyRegisteredOffice.VILLAGE_ID : "0",
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


        var postdata = JSON.stringify({ SocietyRegistrationData: postData, SocietyAmendment: postData, AmendmentDocuments: $scope.AmendmentDoc, MemberData: SocietyRegistrationData });
        //var postdata = '{"SocietyRegistrationData":{"AmendmentId":1985,"Section":"4","IsPermanentSave":false,"TotalMember":1,"TotalPresantMember":8,"MeetingQuorum":100,"TotalVoteInSupport":8,"TotalVoteInOpposition":0,"HouseBuildingNo":"","LaneStreetName":"","Locality":"","DIVISION_ID":"0","DISTRICT_ID":"0","IsRural":"0","CITY_ID":"0","WARD_ID":"0","BLOCK_ID":"0","GP_ID":"0","VILLAGE_ID":"0","PinCode":"0","ActId":"1","SocietyDirectory":"25JAIPUR195051","SocietyIdEnc":"0x010000000A80EACDE761BD9ADB56A6D7B5615E7E7DC0220B97B3F932152FFB415ECB8B56","RegiYearId":"0","ResolutionNo":"","PaasedAGMDate":"2021-07-02","AmendmentType":"4","Name":"","BRN":"","Remark":"..","SocietyTypeId":"6"},"SocietyAmendment":{"AmendmentId":1985,"Section":"4","IsPermanentSave":false,"TotalMember":1,"TotalPresantMember":8,"MeetingQuorum":100,"TotalVoteInSupport":8,"TotalVoteInOpposition":0,"HouseBuildingNo":"","LaneStreetName":"","Locality":"","DIVISION_ID":"0","DISTRICT_ID":"0","IsRural":"0","CITY_ID":"0","WARD_ID":"0","BLOCK_ID":"0","GP_ID":"0","VILLAGE_ID":"0","PinCode":"0","ActId":"1","SocietyDirectory":"25JAIPUR195051","SocietyIdEnc":"0x010000000A80EACDE761BD9ADB56A6D7B5615E7E7DC0220B97B3F932152FFB415ECB8B56","RegiYearId":"0","ResolutionNo":"","PaasedAGMDate":"2021-07-02","AmendmentType":"4","Name":"","BRN":"","Remark":"..","SocietyTypeId":"6"},"AmendmentDocuments":[{"DocumentType":"Notice_to_Registrar_Signed_by_Secretary_Seven_Members","DocumentName":""},{"DocumentType":"Board_meeting_resolution_Notice_with_delivery_proof","DocumentName":"25JAIPUR195051/EBB7F800D75145FAB97C9078F18AE112.pdf"},{"DocumentType":"Board_meeting_resolution_MOM_And_Attendance_Sheet","DocumentName":"25JAIPUR195051/691A1DA3C98549378D7E81FE908A1F01.pdf"},{"DocumentType":"1st_SGM_Notice_with_delivery_proof","DocumentName":""},{"DocumentType":"1st_SGM_MOM_And_Attendance_Sheet","DocumentName":""},{"DocumentType":"2nd_SGM_Notice_with_delivery_proof","DocumentName":""},{"DocumentType":"2nd_SGM_MOM_And_Attendance_Sheet","DocumentName":""},{"DocumentType":"Comparative_Document_Of_By_Laws","DocumentName":""},{"DocumentType":"AGM_Notice_with_delivery_proof","DocumentName":"25JAIPUR195051/002AC2DA1B53435A92B770D012D04478.pdf"},{"DocumentType":"AGM_MOM_And_Attendance_Sheet","DocumentName":"25JAIPUR195051/8412D55325E3433A800A262D5DCA0A62.pdf"}],"MemberData":[{"AdministrativeId":"13151","AADHAARNo":"838657574650","Name":"Archana S. Mankotia","FatherName":"W/O Sangram Singh Mankotia","ContactNo":"9829169395","DOB":"1962-12-08","Gender":"F","CategoryId":"3","CatId":"3","Occupation":"social service","PostId":"0","PostName":"CHAIRMAN/PRESIDENT","Address":"","Photo":"../Content/SocietyProfiling/25JAIPUR195051/member_4a8693c924f24dac858a89780fc57f5d.jpg","SocietyIdEnc":"0x010000000A80EACDE761BD9ADB56A6D7B5615E7E7DC0220B97B3F932152FFB415ECB8B56","IsActive":true,"UserType":"4","AdministratorDesignationId":"-1","MEMBERTYPE":"ELECTEDMEMBERS","RequestType":2,"RemovalType":"0","RemovalFile":""},{"AdministrativeId":"13550","AADHAARNo":"397223051760","Name":"Nirmala Durlabhji","FatherName":"W/O Yogendra Durlabhji","ContactNo":"0000000000","DOB":"1956-06-19","Gender":"F","CategoryId":"3","CatId":"3","Occupation":"buss","PostId":"0","PostName":"mem","Address":"","Photo":"../Content/SocietyProfiling/25JAIPUR195051/member_cf96dbed42664f88846d41722df8d268.jpg","SocietyIdEnc":"0x010000000A80EACDE761BD9ADB56A6D7B5615E7E7DC0220B97B3F932152FFB415ECB8B56","IsActive":true,"UserType":"4","AdministratorDesignationId":"-1","MEMBERTYPE":"ELECTEDMEMBERS","RequestType":2,"RemovalType":"0","RemovalFile":""},{"AdministrativeId":"13554","AADHAARNo":"952324743130","Name":"Madhuwanti Singh","FatherName":"W/O: Suneel Kumar Singh","ContactNo":"0000000000","DOB":"1956-03-15","Gender":"F","CategoryId":"3","CatId":"3","Occupation":"fd","PostId":"0","PostName":"mmm","Address":"","Photo":"../Content/SocietyProfiling/25JAIPUR195051/member_9d01f18a22fb428d99c107bc07605d9d.jpg","SocietyIdEnc":"0x010000000A80EACDE761BD9ADB56A6D7B5615E7E7DC0220B97B3F932152FFB415ECB8B56","IsActive":true,"UserType":"4","AdministratorDesignationId":"-1","MEMBERTYPE":"ELECTEDMEMBERS","RequestType":2,"RemovalType":"0","RemovalFile":""},{"AdministrativeId":"13573","AADHAARNo":"977751041554","Name":"Prafulla Kumari","FatherName":"W/O Lt. Gen. Himmeth Singh","ContactNo":"0000000000","DOB":"1938-03-09","Gender":"F","CategoryId":"3","CatId":"3","Occupation":"bhjbjh","PostId":"0","PostName":"mmm","Address":"","Photo":"../Content/SocietyProfiling/25JAIPUR195051/member_987243c767cc4d0ba52a4f469cbe0665.jpg","SocietyIdEnc":"0x010000000A80EACDE761BD9ADB56A6D7B5615E7E7DC0220B97B3F932152FFB415ECB8B56","IsActive":true,"UserType":"4","AdministratorDesignationId":"-1","MEMBERTYPE":"ELECTEDMEMBERS","RequestType":2,"RemovalType":"0","RemovalFile":""},{"AdministrativeId":"13582","AADHAARNo":"886543983257","Name":"Urvashi Devi","FatherName":"D/O Jaideep Singh","ContactNo":"0000000000","DOB":"1949-07-22","Gender":"F","CategoryId":"3","CatId":"3","Occupation":"jhjkh","PostId":"0","PostName":"mm","Address":"","Photo":"../Content/SocietyProfiling/25JAIPUR195051/member_f9525da45b944935889561e49efb8d5f.jpg","SocietyIdEnc":"0x010000000A80EACDE761BD9ADB56A6D7B5615E7E7DC0220B97B3F932152FFB415ECB8B56","IsActive":true,"UserType":"4","AdministratorDesignationId":"-1","MEMBERTYPE":"ELECTEDMEMBERS","RequestType":2,"RemovalType":"0","RemovalFile":""},{"AdministrativeId":"13589","AADHAARNo":"886804584996","Name":"Maharani Uttara Devi","FatherName":"W/O Maharao Brijraj Singh","ContactNo":"0000000000","DOB":"1942-07-25","Gender":"F","CategoryId":"3","CatId":"3","Occupation":"mkj","PostId":"0","PostName":"mm","Address":"","Photo":"../Content/SocietyProfiling/25JAIPUR195051/member_c058fae368ff493da8fed6516b51f9e2.jpg","SocietyIdEnc":"0x010000000A80EACDE761BD9ADB56A6D7B5615E7E7DC0220B97B3F932152FFB415ECB8B56","IsActive":true,"UserType":"4","AdministratorDesignationId":"-1","MEMBERTYPE":"ELECTEDMEMBERS","RequestType":2,"RemovalType":"0","RemovalFile":""},{"AdministrativeId":0,"OldAdministrativeId":0,"AADHAARNo":"566228284627","Name":"Hanuman Meena","FatherName":"S/O Jagdish Narayan Meena","ContactNo":"0000000000","DOB":"1990-08-20","Gender":"M","CategoryId":"3","CatId":"3","Occupation":"0","PostId":"0","PostName":"0000","Address":"","Photo":"../Content/NewRegistration/member_3a06cbdceccc4192901fc11a61c1ab3d.jpg","SocietyIdEnc":"0x010000000A80EACDE761BD9ADB56A6D7B5615E7E7DC0220B97B3F932152FFB415ECB8B56","IsActive":true,"UserType":"4","AdministratorDesignationId":"-1","MEMBERTYPE":"ELECTEDMEMBERS","RequestType":2,"RemovalType":"0","RemovalFile":""}]}';
        $http.post(SiteUrl + "/Citizen/InsertSocietyNameChangeV1", postdata).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                var AltMsg = "";
                var len = JSON.stringify(data).length;
                if (len > 10) {
                    if (data.data.indexOf("NotValidName") > -1) {
                        $(".SaveSocietyInfo").show();
                        $(".se-pre-con").fadeOut("slow");
                        var ValidNameCheck = "आप जिस सोसाइटी के नाम में संसोधन कर रहे हैं उस सोसाइटी के नाम में कुछ शब्द है जो मान्य नहीं हैं | कृपया सोसाइटी का नाम सही करके फिर से आवेदन करें | अमान्य शब्द " + data.data.replace("NotValidName", "");
                        ShowOkMessage("Warning", ValidNameCheck, function () { return; });
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

    $scope.ResetByLawsModal = function () {

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

    $scope.SaveSocietyNameChange = function () {
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
            success(function (data, status) {
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
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                alert('An Error Occurred Please Try Again Later');
                location.reload();
            });
    };


    $scope.SaveAGM = function () {
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
            success(function (data, status) {
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
            error(function (data, status) {
                alert('An Error Occurred Please Try Again Later');
                location.reload();
            });
    };

    $scope.SaveBODAM = function () {
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
            success(function (data, status) {
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
            error(function (data, status) {
                alert('An Error Occurred Please Try Again Later');
                location.reload();
            });
    };

    $scope.RemoveRowAdminiAct2001 = function (index, ListType) {
        ShowAlertMessage("Alert", _language == 'en-us' ? 'Are you sure you want to delete this Member?' : 'क्या आप वाकई इस सदस्य को हटाना चाहते हैं?', function () {
            $scope.TableData10All[index].IsDelete = true;
            if ($scope.TableData10All[index].oldAdministrativeId == 0) {
                $scope.TableData10All.splice(index, 1);
            }
            $scope.$apply();
        });

    }

    $scope.UndoRemoveRowAdminiAct2001 = function (index, ListType) {
        // ShowAlertMessage("Alert", _language == 'en-us' ? 'Are you sure you want to delete this Member?' : 'क्या आप वाकई इस सदस्य को हटाना चाहते हैं?', function () {
        //$scope.TableData10All[index].IsDelete = false;
        $scope.TableData10All[index].IsDelete = false;

        $scope.$apply();
        // });
    }


    //start aadhar verify
    $scope.Otpsend = function () {
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
    $scope.ClosePopUp = function () {
        $scope.messagebox = false;

    };
    $scope.CloseOtpBox = function () {
        $scope.messagebox = false;
        $scope.OtpDilogBox = false;
        $scope.FillOtp = false;
        $('#ssoauthloading').hide();
        $("#SSOAuth_chkConsent").prop("checked", false);
    };
    $scope.OpenOtpPopUp = function (AADHAARNo, FOR, TRID) {
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
    $scope.OpenOtpPopUpOther = function (AADHAARNo, FOR, TRID, ListType) {
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

            AadhaarAuth_v1($scope.AADHAARNo, $http, "OTP-BIO", false, "", $scope.SocietyDirectory, function (data) {
                $scope.TableData10All[TRID].IsValidate = true;
                $scope.TableData10All[TRID].Name = data.Name;
                $scope.TableData10All[TRID].FatherName = data.FName;
                $scope.TableData10All[TRID].DOB = data.DOB.replace(/'-'/g, '/');
                $scope.TableData10All[TRID].Gender = data.Gender == 'M' ? 'M' : 'F';
                $scope.TableData10All[TRID].Photo = data.Photo;
                $scope.TableData10All[TRID].IsValidate = true;
                $scope.TableData10All[TRID].AadharNo2 = $scope.AADHAARNo;
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
    $scope.GenrateOTP = function () {
        $('#ssoauthloading').show();
        $http.get(SiteUrl + "/Citizen/GenrateOTPdata/?_AadharNo=" + $scope.AADHAARNo).
            success(function (data, status) {
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
            error(function (data, status) {
                $scope.SocietyRegdata = [];
            });
    }
    $scope.ValidateOTP = function () {
        $('#ssoauthloading').show();
        $http.get(SiteUrl + "/Citizen/ValidateOTP/?_AadharNo=" + $scope.AADHAARNo + "&otp=" + $('#SSOAuth_OtpText').val() + "&txn=" + $scope.txn).
            success(function (data, status) {
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
            error(function (data, status) {
                $scope.SocietyRegdata = [];
            });
    }
    $scope.ClearAadharData = function (Clear) {
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
    $scope.ClearAadharDataFromTable = function (TR) {
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
    $scope.CurrentRowDataFill = function (data) {
        var TR = $scope.CurrentRow;
        // alert(TR);
        if (TR > 500 && TR < 1000) {
            //alert("in Nominated");
            $('#' + TR).find('td:eq(2)').find('input').val(data.Name);
            $('#' + TR).find('td:eq(1)').find('input').attr("disabled", "disabled");
            $('#' + TR).find('td:eq(4)').find('input').val(data.DOB);
            $('#' + TR).find('td:eq(5)').find('label').text(data.Gender == 'M' ? 'M' : 'F');

            ($('#' + TR).find('td:eq(9)').find('img'))[0].src = data.Photo;
            var jElement = $('#' + TR).find('td:eq(9)').find('img');
            jElement.css('border', 'rgb(222, 221, 221)');

        }
        else {
            $('#' + TR).find('td:eq(2)').find('input').val(data.Name);
            $('#' + TR).find('td:eq(1)').find('input').attr("disabled", "disabled");
            $('#' + TR).find('td:eq(4)').find('input').val(data.DOB);
            $('#' + TR).find('td:eq(5)').find('label').text(data.Gender == 'M' ? 'M' : 'F');
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
    $scope.AddMoreMemberDetailsNew = function (Flag, Isvalidate) {

        if ($scope.AdministrativeFormValidation(Isvalidate)) {
            var rows = $scope.TableData10All.length + 1;

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
                    "OldAdministrativeId": 0,
                    "AadharNo2": "",
                    "RemovalType": "0"
                });

        }
        else {

        }

        //FIll Category DropDown 

        var ddHTML = _language == 'en-us' ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        angular.forEach($scope.CategoryList, function (item) {
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
        angular.forEach($scope.ChairmanPosts, function (item) {
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

    $scope.ValidateNominatedMember = function (FindTablebyId, Isvalidate) {
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

            $(table).each(function () {
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
    $scope.AdministrativeFormValidation = function () {
        var validate = true;
        var Count = 0;
        // alert('OA');
        var message = $scope._language == 'en-us' ? ' Validate  following : ' : 'निम्नलिखित मान्य करें :';

        var Count = 0;


        $('#TblElectedMember tr').each(function () {
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
    $scope.SaveBalanceSheetInsert = function () {
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
            success(function (data, status) {
                //alert(1);
                $scope.BalanceSheetNew = data;
                //alert(2); 
                $scope.NewBSheet = true;
                //alert(3);
                $scope.BalanceSheetFileList();
            }).
            error(function (data, status) {
                ShowModelAlert("Insert of Balance Sheet", "Error While Upload Excel Sheet");
            });
    };
    // END :Save Balance Sheet Insert

    $scope.messagebox = false;
    $scope.OnPageLoad();

    $scope.SocietyDirectoryFile = "";
    $scope.UploadFile = function (id, type, index, model) {
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
                $scope.xhr(SiteUrl + "/citizen/UploadAmendmentFiles?SocietyDirectory=", formData, type, index, model);
            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }
    $scope.xhr = function (url, data, type, index, model) {
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
        if ($scope.SocietyNameChangeList[0].PaasedAGMDateSmall != null && $scope.SocietyNameChangeList[0].PaasedAGMDateSmall != undefined && $scope.SocietyNameChangeList[0].PaasedAGMDateSmall != "") {
            $scope.SocietyAmendment.PaasedAGMDate = $scope.SocietyNameChangeList[0].PaasedAGMDateSmall.replace(/-/g, '/');
        } else {
            $scope.SocietyAmendment.PaasedAGMDate=""
        }

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
                var newBlanckRowData = $scope.TableData10All;
               // $scope.TableData10All = [];

                //if ($scope.MemberAmendmentList.length == 0) {
                    //newBlanckRowData.forEach(function (item, indx) {
                    //    if (item.AdministrativeId != 0) {
                    //        $scope.TableData10All.push(item);
                    //    }
                    //});
                //}
                if ($scope.MemberAmendmentList.length > 0) {
                    $scope.TableData10All = [];
                }
                $scope.MemberAmendmentList.forEach(function (item, indx) {


                    //if (item.RequestType == 3) {
                        item.IsValidate = true;
                        item.IsNew = true;
                        item.Aadh = item.AadharNo;
                        item.NewAdministrativeId = item.AdministrativeId;
                        item.DOB = item.DOB1;
                        item.Status = item.MEMBERTYPE != undefined && item.MEMBERTYPE == 'ELECTEDMEMBERS' ? true : false;
                        item.Photo = '../Content/SocietyProfiling/' + $scope.SocietyDirectory + '/' + item.Photo;
                        //if ($scope.TableData10All != undefined && $scope.TableData10All.length > 0) {
                            $scope.TableData10All.push({
                                "Id": $scope.TableData10All.length + 1,
                                "Name": item.Name,
                                "FatherName": item.FatherName,
                                "POSTNAME": item.POSTNAME,
                                "Aadh": item.AadharNo,
                                "Gender": item.Gender == "F" ? 'F' : 'M',
                                "DOB": item.DOB,
                                "ContactNo": item.ContactNo,
                                "Photo": item.Photo,
                                "CategoryName": item.Category,
                                "Occupation": item.Occupation,
                                "CatId": item.CategoryId,
                                "CategoryId": item.CategoryId,
                                "Address": item.Address,
                                "Status": item.MEMBERTYPE != undefined && item.MEMBERTYPE == 'ELECTEDMEMBERS' ? true : false,
                                "MEMBERTYPE": item.MEMBERTYPE,
                                "IsValidate": true,
                                "IsNew": true,
                                "IsDelete": item.RequestType == 1,
                                "AdministrativeId": item.AdministrativeId,
                                "OldAdministrativeId": item.OldAdministrativeId,
                                "AadharNo2": item.AadharNo2,
                                "RemovalType": "0"
                            });


                            //$scope.TableData10All.push(item);
                        //}

                    //} else if (item.RequestType == 2) {
                    //    if ($scope.TableData10All != undefined && $scope.TableData10All.length > 0) {
                    //        $scope.TableData10All.filter(function (x) { return x.AdministrativeId === item.oldAdministrativeId }).forEach(function (itm, ind) {
                    //            itm.ContactNo = item.ContactNo;
                    //            itm.Occupation = item.Occupation;
                    //            itm.CategoryId = item.CategoryId;
                    //            itm.CatId = item.CategoryId;
                    //            itm.POSTNAME = item.POSTNAME;
                    //            itm.NewAdministrativeId = item.AdministrativeId;
                    //            itm.Status = item.MEMBERTYPE != undefined && item.MEMBERTYPE == 'ELECTEDMEMBERS' ? true : false;
                    //        });
                    //    }
                    //} else if (item.RequestType == 1) {
                    //    if ($scope.TableData10All != undefined && $scope.TableData10All.length > 0) {
                    //        var newItem = $scope.TableData10All.filter(function (x) { return x.AdministrativeId === item.oldAdministrativeId })[0];
                    //        var index1 = $scope.TableData10All.findIndex(x => x === newItem);
                    //        var RemovalFile1 = item.RemovalFile != undefined && item.RemovalFile != "" ? item.RemovalFile.split('/')[item.RemovalFile.split('/').length - 1] : "";
                    //        $scope.FileCollection.push({ 'Id': 'flRemovalType_' + index1, 'FileType': '', 'File': RemovalFile1, 'Title': '', FileName: RemovalFile1, FileId: 'flRemovalType_' + index1, IsFileUpload: true });
                    //        newItem.IsDelete = true;
                    //        newItem.NewAdministrativeId = item.AdministrativeId;
                    //        newItem.RemovalType = item.RemovalType.toString();
                    //    }
                    //}
                });

                $(".se-pre-con").fadeOut("slow");
                //}
            });
        }
    }
    //  $scope.SocietyRegistration = {};
    $scope.BrnDetails = function () {


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
                success(function (data, status) {
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
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert("Society Profiling", "Error While Fetching BRN Details");
                });
        }
    }

    $scope.ChangeBRN = function () {
        $("#btnValidate").show(); $("#btnChange").hide();
        $scope.txtBrndisabled = false;//$("#txtBrn").removeAttr("disabled");
    }

    $scope.AmendmentDocuments = [];
    $scope.SocietyAmendment = { ChangeName: false };
    //$scope.AmendmentDocuments.push({ Id: 1, Name: "Notice to Registrar Signed by Secretary & Seven Members", FileId: "Notice_to_Registrar_Signed_by_Secretary_Seven_Members", IsShow: $scope.SocietyAmendment.ChangeName });
    $scope.AmendmentDocuments.push({ Id: 2, Name: "Board meeting resolution Notice with delivery proof", FileId: "Board_meeting_resolution_Notice_with_delivery_proof", IsShow: true });
    $scope.AmendmentDocuments.push({ Id: 3, Name: "Board meeting resolution MOM And Attendance Sheet", FileId: "Board meeting resolution MOM And Attendance Sheet", IsShow: true });
    $scope.AmendmentDocuments.push({ Id: 4, Name: "1st SGM(Special General meeting) Notice with delivery proof", FileId: "1st_SGM_Notice_with_delivery_proof", IsShow: true });
    $scope.AmendmentDocuments.push({ Id: 5, Name: "1st SGM(Special General meeting) MOM And Attendance Sheet", FileId: "1st_SGM_MOM_And_Attendance_Sheet", IsShow: true });
    $scope.AmendmentDocuments.push({ Id: 6, Name: "2nd SGM(Special General meeting) Notice with delivery proof", FileId: "2nd_SGM_Notice_with_delivery_proof", IsShow: true });
    $scope.AmendmentDocuments.push({ Id: 7, Name: "2nd SGM(Special General meeting) MOM And Attendance Sheet", FileId: "2nd_SGM_MOM_And_Attendance_Sheet", IsShow: true });
    $scope.AmendmentDocuments.push({ Id: 8, Name: "Comparative Document Of By-Laws", FileId: "Comparative_Document_Of_By_Laws", IsShow: true });
    // $scope.AmendmentDocuments.push({ Id: 8, Name: "Original Certificate To be deposited in Registrar office", FileId: "Original_Certificate_To_be_deposited_in_Registrar_office", IsShow:true });

    $scope.checkNameChange = function () {
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
    $scope.SocietyOthersDocuments = [];
    //$scope.SocietyOthersDocuments.push({ Id: $scope.SocietyOthersDocuments.length + 1, Name: "", FileId: "" });
    $scope.AddOthersDocuments = function () {
        $scope.SocietyOthersDocuments.push({ Id:'otherfiles_'+ $scope.SocietyOthersDocuments.length + 1, Name: "", FileName: "", FileId: "", IsFileUpload: false });
    }

    $scope.ChangeOtherFile = function (id) {
        var SocietyOthersDocuments = $scope.SocietyOthersDocuments;
        //$scope.SocietyOthersDocuments = [];
        $scope.SocietyOthersDocuments.forEach(function (item, index) {
            if (item.Id == id) {
                item.Name = "";
                item.IsFileUpload = false;
            }
        });
        $("#" + id).val("");
        $("#" + id).parent().parent().css('border', 'none');
    }
    $scope.IsOtherFileUpload = function (id) {
        return $scope.SocietyOthersDocuments.filter(function (x) { return x.Id == id && x.IsFileUpload == true }).length > 0;
    }
    $scope.GetOtherFileUploaded = function (id) {
        try {
            return $scope.SocietyOthersDocuments.filter(function (x) { return x.Id == id && x.IsFileUpload == true })[0].Name;
        } catch (ex) {
            return "";
        }
    }
    $scope.DeleteOtherDocument = function (index, id) {
        ShowAlertMessage("Alert", "Are you sure you want to delete this document", function () {
            $scope.SocietyOthersDocuments.splice(index, 1);
            $http.post(SiteUrl + "/citizen/DeleteSocietyAmendmentDocument", { "Id": id }).
                success(function (data, status) {
                    //$(".blockUI").fadeOut("slow");                   
                }).
                error(function (data, status) {
                    //$(".blockUI").fadeOut("slow");                  
                });

        });

    }





    $scope.UploadOtherFile = function (id, index) {
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
                formData.append("DocumentTitle", $("#txtOtherDocuemnt_" + index).val());
                formData.append("SocietyIDEnc", Datasplit[0]);
                formData.append("AmendmentId", $scope.SocietyAmendment.AmendmentId);
             
                $(".blockUI").show();
                $scope.xhrOtherFile(SiteUrl + "/citizen/UploadAmendmentOthersFiles?SocietyDirectory", formData,  id);
            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }
    $scope.xhrOtherFile = function (url, data,  model) {
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
                        $scope.SocietyOthersDocuments.forEach(function (itm, indx) {
                            if (itm.Id == model) {
                                itm.Name = $scope.SocietyDirectoryFile + '/' + request.responseText;
                                itm.File = request.responseText;
                                $("#" + model).parent().parent().css('border', 'none');
                                itm.IsFileUpload = true;
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

                            //scope.AuditorAssign["IsUpload" + id + ""] = true;
                            //scope.AuditorAssign["" + id + ""] = "";

                            scope.$apply();
                        }
                    }

                });
            };
        }
    };
});

MasterMod.directive('validotherfile', function validFile() {
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
                        var maxLength = 2048;
                        //var maxLength = scope.SocietyDocuments.filter(function (x) { return x.FileId == id })[0].Size;
                        // The size of the file. 
                        if (file >= parseInt(maxLength)) {
                            ShowOkMessage("Warning", _language == 'en-us' ? "File too Big, please select a file less than " + parseInt(maxLength / 1022) + " MB" : "फ़ाइल बहुत बड़ी है, कृपया " + maxLength / 1022 + " MB से कम फ़ाइल का चयन करें", function () { });
                            elem[0].value = "";
                        } else {

                            if (scope.SocietyOthersDocuments.filter(function (x) { return x.Id == id }).length <= 0) {
                                scope.SocietyOthersDocuments.push({ Id: id, Name: "", FileId: id, IsFileUpload: true });
                            } else {
                                scope.SocietyOthersDocuments.forEach(function (itm, indx) {
                                    if (itm.Id == id) {
                                        itm.IsFileUpload = true;
                                    }
                                });
                            }

                            //scope.AuditorAssign["IsUpload" + id + ""] = true;
                            //scope.AuditorAssign["" + id + ""] = "";

                            scope.$apply();
                        }
                    }

                });
            };
        }
    };
});