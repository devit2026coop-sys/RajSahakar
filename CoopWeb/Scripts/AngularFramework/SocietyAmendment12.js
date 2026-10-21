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
    $scope.GetSocietyDetails = true;
    $scope.SocietyAmendment = { AmendmentSection: 0 };

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

        $scope.showLoader = false;
        $scope.GetSocietyList();
        $scope.GetAmendmentHistory();
        $('#txtByLawsAGMDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
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
        $http.post(SiteUrl + "/Citizen/GetSocityDataForOtp2", { "SocietyRegistrationNo": $scope.SocietyRegistration.SocietyRegistrationNo, "DistrictId": $scope.ddlDistrct?.DISTRICT_ID ?? 0 }).
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
    };
    $scope.SelectSocietyForVerify = function (item, index) {
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
            $scope.ChooseSociety(item);
        }

        $scope.IsSearchResult = false;
        $scope.IsShowProfiling = false;
        $scope.IsShowAmendment = false;
        $scope.IsShowSSOMAP = false;
        $scope.IsShowSSOMAPNoMAP = true
        $scope.GetSocietyDetails = false;
        $scope.IsWaitingForApproval = false;
    }
    $scope.RemovalTypeList = [{ CODENAME: 'DEATH', CODENAME_MANGAL: 'DEATH', CODEID: '1' }, { CODENAME: 'REMOVEL', CODENAME_MANGAL: 'REMOVEL', CODEID: '3' }, { CODENAME: 'RESIGNATION', CODENAME_MANGAL: 'RESIGNATION', CODEID: '3' }]


    //START : Validate Society Mapping Request Data
    $scope.ValidateSocietyMappingRequestData = function (flag) {
        var Msg = '';
        var tile = _language === 'en-us' ? 'Society Mapping Request' : 'सोसाइटी मानचित्रण अनुरोध';
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
        $(".se-pre-con").fadeIn("slow");
        if (!$scope.ValidateSocietyMappingRequestData(flag)) {
            $(".se-pre-con").fadeOut("slow");
            return;
        }

        $scope.MemberRegistration.DOB = $scope.MemberRegistration.DOB.split('-')[2] + "/" + $scope.MemberRegistration.DOB.split('-')[1] + "/" + $scope.MemberRegistration.DOB.split('-')[0]

        $scope.MemberRegistration.FileName = $scope.GetFileUploaded('MapplingFileAtt');
        $http.post(SiteUrl + "/Citizen/InsertAmendmentSenction4", { model: $scope.MemberRegistration, Flag: flag }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                var title = _language === 'en-us' ? 'Society Mapping Request' : 'सोसाइटी मानचित्रण अनुरोध';
                if (parseInt(data, 10) > 0) {
                    $scope.GetSocietyList().then(function () {

                        var sData = $scope.SocietyList.filter(function (x) { return x.SocietyID == $scope.SocietyId })[0];
                        $scope.SocietyAmendment.SocietyIdEnc = sData.SocietyIdENC + "~" + sData.SocietyDirectory + "~" + sData.ActId;
                        $scope.OnChangeSociety();

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
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
            $scope.BlockListByOffAdd(1);
        }
        else if (SelectedVal == 'false' || SelectedVal == 'False') {
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;
            $scope.GetCityListByCityTypes(1);
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
    }
    $scope.Divisions = [];

    $scope.GetApplicationFlowSociety = function (SocietyIDEnc, AmendmentId) {
        $scope.showLoader = true;
        $http.get("../ApplicationFlow/GetApplicationFlowSocietyAmendment1958/?SocietyIDEnc=" + SocietyIDEnc + "&AmendmentId=" + AmendmentId).
            success(function (data, status) {
                $scope.ApplicationFlowSociety = data;
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
        $scope.showLoader = false;
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
                $scope.SocietyList = [];// data;
                var data1 = JSON.parse(data);
                if (data1.length <= 0 && amendmenttyp == '12') {
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


    $scope.IsSocietyEdit = true;
    $scope.IsEditApplication = true;


    $scope.OnChangeSociety = function (AmendmentId) {
        $(".se-pre-con").fadeIn("slow");

        var data = $scope.SocietyList.filter(function (x) { return x.SocietyIdENC.toString() === $scope.SocietyAmendment.SocietyIdEnc.split("~")[0] });

        if (data[0].MaxAppliFlowStatusID == 17 || data[0].MaxAppliFlowStatusID == 26) {
            $scope.ShowTab = true;
            $scope.IsSocietyEdit = !data[0].IsAmendment //== 'True' ? false : true;
            $scope.IsEditApplication = $scope.IsSocietyEdit;

            $scope.SocietyAmendment.TotalMember = data[0].TotalMember;

            if (window.location.href.indexOf('SocietyAmendment') > -1 && data[0].AmendmentSection == "4") {
                $scope.IsSocietyEdit = true //== 'True' ? false : true;
                $scope.IsEditApplication = $scope.IsSocietyEdit;
            }

            if (AmendmentId != undefined) {
                $scope.GetApplicationFlowSociety($scope.SocietyAmendment.SocietyIdEnc.split("~")[0], AmendmentId);
            }
            if (!$scope.IsSocietyEdit) {
                $scope.GetSocietyNameChangeList(AmendmentId).then(function () {
                    $(".se-pre-con").fadeOut("slow");
                });
            } else {
                $(".se-pre-con").fadeOut("slow");
            }
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
            return $http.get("../Citizen/GetByLawsList?SocietyIdEnc=" + SocietyIdEnc).
                success(function (data, status) {
                    $scope.ByLawsListData = data;
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


    $scope.SocietyNameChangeList = [];
    $scope.GetSocietyNameChangeList = function (AmendmentId) {
        $scope.ResetByLawsModal();
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            return $http.get("../Citizen/GetSocietyNameChangeList?SocietyIdEnc=" + SocietyIdEnc + "&AmendmentId=" + AmendmentId).
                success(function (data, status) {
                    var dataAll = JSON.parse(data.DataList);
                    $scope.SocietyRegisteredOffice.IsRural = dataAll.Table[0].IsRural == "True" ? 1 : 0;
                    console.log(dataAll);
                    $scope.SocietyNameChangeList = dataAll.Table;
                    var documents = dataAll.Table1;
                    $scope.allDisable = true;
                    $scope.FileCollection = [];
                    documents.filter(function (x) { return x.Type != 'Other' }).forEach(function (item, index) {
                        $scope.FileCollection.push({ 'Id': item.Id, 'FileType': '', 'File': item.DocumentURL, 'Title': '', FileName: item.DocumentURL, FileId: item.DocumentType, IsFileUpload: true });
                    });
                    documents.filter(function (x) { return x.Type == 'Other' }).forEach(function (item, index) {
                        $scope.SocietyOthersDocuments.push({ Id: item.Id, Name: item.DocumentURL, FileName: item.DocumentType, FileId: 'otherfiles_' + $scope.SocietyOthersDocuments.length + 1, IsFileUpload: true });
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
                            $scope.SocietyRegisteredOffice.CITY_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].CITY_ENG : $scope.SocietyNameChangeList[0].CITY_MANGAL;
                            $scope.SocietyRegisteredOffice.WARD_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].WARD_ENG : $scope.SocietyNameChangeList[0].WARD_MANGAL;


                            $scope.SocietyRegisteredOffice.BLOCK_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].BLOCK_ENG : $scope.SocietyNameChangeList[0].BLOCK_MANGAL;
                            $scope.SocietyRegisteredOffice.GP_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].GP_ENG : $scope.SocietyNameChangeList[0].GP_MANGAL;
                            $scope.SocietyRegisteredOffice.VILLAGE_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].VILLAGE_ENG : $scope.SocietyNameChangeList[0].VILLAGE_MANGAL;
                        }
                    }
                }).
                error(function (data, status) {
                    $scope.ByLawsListData = [];
                    alert('Error while fetching society list data.');
                });
        } else {
            return 1;
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

            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
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

                if ($scope.SocietyRegisteredOffice.IsRural == undefined || ($scope.SocietyRegisteredOffice.IsRural == 1 && $scope.SocietyRegisteredOffice.IsRural == 1)) {
                    Msg += (_language == 'en-us' ? 'Please Choose Urban/Rural  \n' : 'Please Choose Urban/Rural  \n');
                    Msg += '<br />';
                }

                if ($scope.SocietyRegisteredOffice.IsRural != undefined && $scope.SocietyRegisteredOffice.IsRural == 0) {
                    if ($scope.SocietyRegisteredOffice.CITY_ID == undefined || $scope.SocietyRegisteredOffice.CITY_ID == "0") {
                        Msg += (_language == 'en-us' ? 'Please Enter Address-City  \n' : 'Please Enter Address-City  \n\n');
                        Msg += '<br />';
                    }
                    if ($scope.SocietyRegisteredOffice.WARD_ID == undefined || $scope.SocietyRegisteredOffice.WARD_ID == "0") {
                        Msg += (_language == 'en-us' ? 'Please Enter Address-Ward  \n' : 'Please Enter Address-Ward  \n\n');
                        Msg += '<br />';
                    }

                } else if ($scope.SocietyRegisteredOffice.IsRural != undefined && $scope.SocietyRegisteredOffice.IsRural == 1) {
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
            CITY_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural == 'False' && $scope.SocietyRegisteredOffice.CITY_ID != undefined ? $scope.SocietyRegisteredOffice.CITY_ID : "0",
            WARD_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural == 'False' && $scope.SocietyRegisteredOffice.WARD_ID != undefined ? $scope.SocietyRegisteredOffice.WARD_ID : "0",
            BLOCK_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural == 'True' && $scope.SocietyRegisteredOffice.BLOCK_ID != undefined ? $scope.SocietyRegisteredOffice.BLOCK_ID : "0",
            GP_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural == 'True' && $scope.SocietyRegisteredOffice.GP_ID != undefined ? $scope.SocietyRegisteredOffice.GP_ID : "0",
            VILLAGE_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural == 'True' && $scope.SocietyRegisteredOffice.VILLAGE_ID != undefined ? $scope.SocietyRegisteredOffice.VILLAGE_ID : "0",
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

                if ($scope.SocietyRegisteredOffice.IsRural == undefined || ($scope.SocietyRegisteredOffice.IsRural == 1 && $scope.SocietyRegisteredOffice.IsRural == 0)) {
                    Msg += (_language == 'en-us' ? 'Please Choose Urban/Rural  \n' : 'Please Choose Urban/Rural  \n');
                    Msg += '<br />';
                }

                if ($scope.SocietyRegisteredOffice.IsRural != undefined && $scope.SocietyRegisteredOffice.IsRural == 0) {
                    if ($scope.SocietyRegisteredOffice.CITY_ID == undefined || $scope.SocietyRegisteredOffice.CITY_ID == "0") {
                        Msg += (_language == 'en-us' ? 'Please Enter Address-City  \n' : 'Please Enter Address-City  \n\n');
                        Msg += '<br />';
                    }
                    if ($scope.SocietyRegisteredOffice.WARD_ID == undefined || $scope.SocietyRegisteredOffice.WARD_ID == "0") {
                        Msg += (_language == 'en-us' ? 'Please Enter Address-Ward  \n' : 'Please Enter Address-Ward  \n\n');
                        Msg += '<br />';
                    }

                } else if ($scope.SocietyRegisteredOffice.IsRural != undefined && $scope.SocietyRegisteredOffice.IsRural == 1) {
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
                    if (itm.CategoryId == '0' || itm.CategoryId == '' || itm.CategoryId == undefined || itm.CategoryId == null) {
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
            IsRural: $scope.SocietyAmendment.ChaneAddress && ($scope.SocietyRegisteredOffice.IsRural != undefined && $scope.SocietyRegisteredOffice.IsRural =="1") ? true : false,
            CITY_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural == '0' && $scope.SocietyRegisteredOffice.CITY_ID != undefined ? $scope.SocietyRegisteredOffice.CITY_ID : "0",
            WARD_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural == '0' && $scope.SocietyRegisteredOffice.WARD_ID != undefined ? $scope.SocietyRegisteredOffice.WARD_ID : "0",
            BLOCK_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural != '1' && $scope.SocietyRegisteredOffice.BLOCK_ID != undefined ? $scope.SocietyRegisteredOffice.BLOCK_ID : "0",
            GP_ID: $scope.SocietyAmendment.ChaneAddress && $scope.SocietyRegisteredOffice.IsRural != '1' && $scope.SocietyRegisteredOffice.GP_ID != undefined ? $scope.SocietyRegisteredOffice.GP_ID : "0",
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
        $scope.TableData10All[index].IsDelete = false;

        $scope.$apply();
        // });
    }



    $scope.messagebox = false;
    $scope.OnPageLoad();

    $scope.SocietyDirectoryFile = "";
    $scope.UploadFile = function (id, type, index, model) {
        try {    // FormData
            if (window.FormData !== undefined) {
                var fileUpload = $("#" + id).get(0);
                var files = fileUpload.files;
                var formData = new FormData();
                formData.append("pldbfile", files[0]);
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
            $scope.SocietyAmendment.PaasedAGMDate = ""
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

            if ($scope.SocietyNameChangeList[0].IsRural == 'False') {
                $scope.SocietyRegisteredOffice.IsRural == 0
                $scope.SocietyRegisteredOffice.CITY_ID = $scope.SocietyNameChangeList[0].CITY_ID;
                $scope.SocietyRegisteredOffice.WARD_ID = $scope.SocietyNameChangeList[0].WARD_ID;

            } else {
                $scope.SocietyRegisteredOffice.IsRural == 1
                $scope.SocietyRegisteredOffice.BLOCK_ID = $scope.SocietyNameChangeList[0].BLOCK_ID;
                $scope.SocietyRegisteredOffice.GP_ID = $scope.SocietyNameChangeList[0].GP_ID;
                $scope.SocietyRegisteredOffice.VILLAGE_ID = $scope.SocietyNameChangeList[0].VILLAGE_ID;
            }
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
        $scope.SocietyOthersDocuments.push({ Id: 'otherfiles_' + $scope.SocietyOthersDocuments.length + 1, Name: "", FileName: "", FileId: "", IsFileUpload: false });
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
            var fileid = id;
            //if (id.indexOf('_') > -1)
            //    fileid = id.split('_')[1];
            $http.post(SiteUrl + "/citizen/DeleteSocietyAmendmentDocument", { "Id": fileid }).
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
                $scope.xhrOtherFile(SiteUrl + "/citizen/UploadAmendmentOthersFiles?SocietyDirectory", formData, id);
            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }
    $scope.xhrOtherFile = function (url, data, model) {
        debugger;
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
                                debugger;
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


                            scope.$apply();
                        }
                    }

                });
            };
        }
    };
});