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


    $scope.RemovalTypeList = [{ CODENAME: 'DEATH', CODENAME_MANGAL: 'DEATH', CODEID: '1' }, { CODENAME: 'REMOVEL', CODENAME_MANGAL: 'REMOVEL', CODEID: '3' }, { CODENAME: 'RESIGNATION', CODENAME_MANGAL: 'RESIGNATION', CODEID:'3'}]

    $scope.SocietyRegisteredOffice = {
        DISTRICT_ID: "0",
        DIVISION_ID:"0",
        CityTypes: "0",
        CITY_ID: "0",
        WARD_ID: "0",
        BLOCK_ID: "0",
        GP_ID: "0",
        VILLAGE_ID: "0"
    };



    $scope.RuralUrbanBlock = function (V) {
        var SelectedVal = $scope.SocietyRegisteredOffice.IsRural != undefined && $scope.SocietyRegisteredOffice.IsRural == 1 ? 'True' :'False';
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
        $http.post(SiteUrl + "/Citizen/GetSocietyList", { "WhereClause": " " }).
            success(function (data, status) {
                //$scope.SocietyList = data.filter(x => (x.IsSameCEOAddress == 'True') && (x.AddressFor == '0'));
                $scope.SocietyList = data;
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
                return $http.post(SiteUrl + "/Society/GetSocietyDetailsBySocietyId", { "SocietyId": SocietyIdEnc, "EMitraTransactionId": 0 }).
                    success(function (data, status) {

                        var ALLMEMBERS = [];
                        var Nomi_ALLMEMBERS = [];
                        if (ActId != 7) {
                            ALLMEMBERS = data.filter(x => (x.IsSameCEOAddress == 'True') && (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS'));
                        }
                        else if (ActId == 7) {
                            ALLMEMBERS = data.filter(x => (x.IsSameCEOAddress == 'True') && (x.AddressFor == '0') && (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS'));
                        }
                        if (ActId == 3) {
                            Nomi_ALLMEMBERS = data.filter(x => (x.IsSameCEOAddress == 'True') && (x.MEMBERTYPE == 'NOMINATEDMEMBERS'));
                        }
                        var TableData10NomiAll = [];
                        var TableData10All = [];
                        $scope.TableData10AllCopy = [];
                        $scope.TableData10AllSportsCopy = [];
                        $scope.TableData10NomiAllCopy = [];

                        var startID = 1;
                        var X = 7;
                        if (ActId == 1) {
                            X = (ALLMEMBERS.length > 6 ? ALLMEMBERS.length : 7)
                        }
                        else if (ActId == 7) {
                            X = (ALLMEMBERS.length > 4 ? ALLMEMBERS.length : 5);
                            startID = 1001;
                        }
                        else {
                            X = (ALLMEMBERS.length > 14 ? ALLMEMBERS.length : 15)
                        }
                        for (var x = 0; x < ALLMEMBERS.length; x++) {
                            TableData10All.push(
                                {
                                    "Id": x + startID,
                                    "Name": ALLMEMBERS[x].Name,
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
                                    "Status": ALLMEMBERS[x].MEMBERTYPE == 'ELECTEDMEMBERS' ? 'true' : 'false',
                                    "MEMBERTYPE": ALLMEMBERS[x].MEMBERTYPE,
                                    "IsValidate": true,
                                    "IsNew": false,
                                    "IsDelete": false,
                                    "AdministrativeId": ALLMEMBERS[x].AdministrativeId,
                                    "AadharNo2": ALLMEMBERS[x].AadharNo2,
                                    "RemovalType":"0"
                                });
                        }

                        for (var x = ALLMEMBERS.length; x < X; x++) {
                            TableData10All.push(
                                {
                                    "Id": x + startID,
                                    "Name": '',
                                    "POSTNAME": '',
                                    "Aadh": '',
                                    "Gender": '',
                                    "DOB": '',
                                    "ContactNo": '',
                                    "Photo": '../Content/Images/Balnk.png',
                                    "CategoryName": '',
                                    "Occupation": '',
                                    "Address": '',
                                    "CatId": "3",
                                    "CategoryId": "3",
                                    "Status": 'false',
                                    "MEMBERTYPE": '',
                                    "IsValidate": false,
                                    "IsNew": true,
                                    "IsDelete": false,
                                    "AdministrativeId": 0,
                                    "AadharNo2": "",
                                    "RemovalType":"0"
                                });
                        }
                        if (ActId == 7) {
                            $scope.TableData10AllSports = TableData10All;
                            $scope.TableData10All = [];

                            $scope.TableData10AllCopy = [];
                            $scope.TableData10AllSportsCopy = [];
                            //$scope.TableData10AllSportsCopy = TableData10All;

                            TableData10All.forEach(function (itm, indx) {
                                $scope.TableData10AllSportsCopy.push({
                                    "Id": indx,
                                    "Name": itm.Name,
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
                                    "Status": itm.MEMBERTYPE == 'ELECTEDMEMBERS' ? 'true' : 'false',
                                    "MEMBERTYPE": itm.MEMBERTYPE,
                                    "IsValidate": true,
                                    "IsNew": false,
                                    "IsDelete": false,
                                    "AdministrativeId": itm.AdministrativeId,
                                    "AadharNo2": itm.AadharNo2,
                                    "RemovalType": itm.RemovalType
                                });
                            })

                        }
                        else {
                            if (ALLMEMBERS.length > 0) {
                                $scope.SocietyAmendment.TotalMembersClassA = ALLMEMBERS[0].AdminDetailTotalMembersClassA;
                                $scope.SocietyAmendment.TotalMembersClassB = ALLMEMBERS[0].AdminDetailTotalMembersClassB;
                                $scope.SocietyAmendment.TotalMembersClassC = ALLMEMBERS[0].AdminDetailTotalMembersClassC;
                            }
                            $scope.TableData10All = TableData10All;
                            $scope.TableData10AllSports = [];

                            $scope.TableData10AllCopy = [];
                            $scope.TableData10AllSportsCopy = [];

                            TableData10All.forEach(function (itm, indx) {
                                $scope.TableData10AllCopy.push(
                                    {
                                        "Id": indx,
                                        "Name": itm.Name,
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
                                        "Status": itm.MEMBERTYPE == 'ELECTEDMEMBERS' ? 'true' : 'false',
                                        "MEMBERTYPE": itm.MEMBERTYPE,
                                        "IsValidate": true,
                                        "IsNew": false,
                                        "IsDelete": false,
                                        "AdministrativeId": itm.AdministrativeId,
                                        "AadharNo2": itm.AadharNo2,
                                        "RemovalType": itm.RemovalType
                                    }
                                );
                            })

                        }


                        //Nominated Member
                        if (ActId == 3) {
                            for (var x = 0; x < Nomi_ALLMEMBERS.length; x++) {
                                TableData10NomiAll.push(
                                    {
                                        "Id": x + 501,
                                        "Name": ALLMEMBERS[x].Name,
                                        "POSTNAME": ALLMEMBERS[x].POSTNAME,
                                        "Aadh": ALLMEMBERS[x].AadharNo,
                                        "Gender": ALLMEMBERS[x].Gender == "F" ? 'FEMALE' : 'MALE',
                                        "DOB": ALLMEMBERS[x].DOB,
                                        "ContactNo": ALLMEMBERS[x].ContactNo,
                                        "Photo": ALLMEMBERS[x].Photo,
                                        "CategoryName": ALLMEMBERS[x].Category,
                                        "Occupation": ALLMEMBERS[x].Occupation,
                                        "CatId": ALLMEMBERS[x].CategoryId,
                                        "Status": ALLMEMBERS[x].MEMBERTYPE == 'ELECTEDMEMBERS' ? 'true' : 'false',
                                        "MEMBERTYPE": ALLMEMBERS[x].NominatedBy,
                                        "NominatedBy": ALLMEMBERS[x].NOMINATEDBY,
                                        "IsValidate": true,
                                        "AdministrativeId": ALLMEMBERS[x].AdministrativeId,
                                        "AadharNo2": "",
                                        "RemovalType":"0"
                                    });
                            }
                            for (var x = Nomi_ALLMEMBERS.length; x < 1; x++) {
                                TableData10NomiAll.push(
                                    {
                                        "Id": x + 501,
                                        "Name": '',
                                        "POSTNAME": '',
                                        "Aadh": '',
                                        "Gender": '',
                                        "DOB": '',
                                        "ContactNo": '',
                                        "Photo": '../Content/Images/Balnk.png',
                                        "CategoryName": '',
                                        "Occupation": '',
                                        "CatId": 3,
                                        "Status": 'false',
                                        "MEMBERTYPE": '',
                                        "IsValidate": false,
                                        "AdministrativeId": 0,
                                        "AadharNo2": "",
                                        "RemovalType":"0"
                                    });
                            }
                            $scope.TableData10NomiAll = TableData10NomiAll;
                            $scope.TableData10NomiAllCopy = [];

                            TableData10NomiAll.forEach(function (itm, indx) {
                                $scope.TableData10NomiAllCopy.push({
                                    "Id": indx,
                                    "Name": itm.Name,
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
                                    "Status": itm.MEMBERTYPE == 'ELECTEDMEMBERS' ? 'true' : 'false',
                                    "MEMBERTYPE": itm.MEMBERTYPE,
                                    "IsValidate": true,
                                    "IsNew": false,
                                    "IsDelete": false,
                                    "AdministrativeId": itm.AdministrativeId,
                                    "AadharNo2": itm.AadharNo2,
                                    "RemovalType": itm.RemovalType
                                });
                            })
                        }
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
    $scope.OnChangeSociety = function (item) {
        $(".se-pre-con").fadeIn("slow");
        $scope.ShowTab = true;

        var data = $scope.SocietyList.filter(function (x) { return x.SocietyIdENC.toString() === $scope.SocietyAmendment.SocietyIdEnc.split("~")[0] });
        $scope.IsSocietyEdit = data[0].IsAmendment == 'True' ? false : true;
        $scope.IsEditApplication = $scope.IsSocietyEdit;

        $scope.GetMemberList().then(function () {
            if (!$scope.IsSocietyEdit) {
                $scope.GetSocietyNameChangeList().then(function () {
                    $(".se-pre-con").fadeOut("slow");
                });
            } else {
                $(".se-pre-con").fadeOut("slow");
            }
        });


        //  $scope.GetByLawsList();
        //$scope.GetAGM_BODMList();
        // $(".se-pre-con").fadeOut("slow");
        // $scope.GetMemberList();
        //$scope.GetBalanceSheet();
        //$scope.BalanceSheetFileList();
        //OnTabClick(event, 'ByLaws');
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
            return $http.post(SiteUrl + "/Society/GetMemberAmendmentBySocietyId", { "SocietyId": SocietyIdEnc }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.MemberAmendmentList = [];
                    $scope.MemberAmendmentList = data;
                    $scope.MemberAmendmentList.forEach(function (item, indx) {
                       if (item.RequestType == 1) {
                            //$scope.FileCollection.push({ 'Id': 'flRemovalType_' + indx, 'FileType': '', 'File': item.RemovalFile, 'Title': '', FileName: item.RemovalFile, FileId: 'flRemovalType_' + indx, IsFileUpload: true });
                            item.IsDelete = true;
                            //item.NewAdministrativeId = item.AdministrativeId;
                            //item.RemovalType = item.RemovalType.toString();
                        }
                    });
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
    $scope.GetSocietyNameChangeList = function () {
        //$(".se-pre-con").fadeIn("slow");


        $scope.ResetByLawsModal();



        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            // alert(SocietyIdEnc);
            return $http.get("../Citizen/GetSocietyNameChangeList?SocietyIdEnc=" + SocietyIdEnc).
                success(function (data, status) {
                    $scope.SocietyNameChangeList = data;

                    $scope.allDisable = true;

                    $scope.SocietyAmendment.ChangeByeLaws = $scope.SocietyNameChangeList[0].AmendmentType.indexOf('1') > -1 ? true : false;
                    $scope.SocietyAmendment.ChangeName = $scope.SocietyNameChangeList[0].AmendmentType.indexOf('2') > -1 ? true : false;
                    $scope.SocietyAmendment.ChaneAddress = $scope.SocietyNameChangeList[0].AmendmentType.indexOf('3') > -1 ? true : false;
                    $scope.SocietyAmendment.ChaneExecutiveMember = $scope.SocietyNameChangeList[0].AmendmentType.indexOf('4') > -1 ? true : false;

                    $scope.FileCollection = [];




                    if ($scope.SocietyNameChangeList[0].flResoloutionNotice != '') {
                        $scope.FileCollection.push({ 'Id': 'flResoloutionNotice', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flResoloutionNotice, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flResoloutionNotice, FileId: 'flResoloutionNotice', IsFileUpload: true });
                    }
                    if ($scope.SocietyNameChangeList[0].flAttendanceSheet != '') {
                        $scope.FileCollection.push({ 'Id': 'flAttendanceSheet', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flAttendanceSheet, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flAttendanceSheet, FileId: 'flAttendanceSheet', IsFileUpload: true });
                    }
                    if ($scope.SocietyNameChangeList[0].ByeLawsFile != '') {
                        $scope.FileCollection.push({ 'Id': 'flBylawsFileName', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].ByeLawsFile, 'Title': '', FileName: $scope.SocietyNameChangeList[0].ByeLawsFile, FileId: 'flBylawsFileName', IsFileUpload: true });
                    }
                    if ($scope.SocietyNameChangeList[0].flByLowComparativeDocument != '') {
                        $scope.FileCollection.push({ 'Id': 'flByLowComparativeDocument', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flByLowComparativeDocument, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flByLowComparativeDocument, FileId: 'flByLowComparativeDocument', IsFileUpload: true });
                    }
                    if ($scope.SocietyNameChangeList[0].flAGMDeliveryProof != '') {
                        $scope.FileCollection.push({ 'Id': 'flAGMDeliveryProof', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flAGMDeliveryProof, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flAGMDeliveryProof, FileId: 'flAGMDeliveryProof', IsFileUpload: true });
                    }
                    if ($scope.SocietyNameChangeList[0].flSGMNotice != '') {
                        $scope.FileCollection.push({ 'Id': 'flSGMNotice', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flSGMNotice, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flSGMNotice, FileId: 'flSGMNotice', IsFileUpload: true });
                    }
                    if ($scope.SocietyNameChangeList[0].flSGMAttendanceSheet != '') {
                        $scope.FileCollection.push({ 'Id': 'flSGMAttendanceSheet', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flSGMAttendanceSheet, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flSGMAttendanceSheet, FileId: 'flSGMAttendanceSheet', IsFileUpload: true });
                    }

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


    $scope.ValidateBRNNo = "";
    $scope.ValidateSocietyName = "";
    $scope.SaveByLaws = function () {
        var Msg = '';
        var formData = new FormData();
        formData.append("AmendmentId", $scope.SocietyAmendment.AmendmentId);//$scope.ByeLawsSocietyFile);


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
        }
        //if ($scope.SocietyAmendment.RegiYearId == undefined || $scope.SocietyAmendment.RegiYearId == '0') {
        //    Msg += (_language == 'en-us' ? 'Please Select Registered Year \n' : 'पंजीकृत वर्ष का चयन करे\n');
        //    Msg += '<br />';
        //}



        //if ($scope.SocietyAmendment.ResolutionNo == undefined || $scope.SocietyAmendment.ResolutionNo == '') {
        //    Msg += (_language == 'en-us' ? 'Please Enter ResolutionNo \n' : 'संकल्प नं दर्ज करे\n');
        //    Msg += '<br />';
        //}


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




        //if ($scope.SocietyAmendment.ChangeName && ($scope.SocietyAmendment.Name == undefined || $scope.SocietyAmendment.Name == '')) {
        //    Msg += (_language == 'en-us' ? 'Please Enter society new name \n' : 'Please Enter society new name\n');
        //    Msg += '<br />';
        //}


        if ($("#txtByLawsAGMDate").val() == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Paased in Which AGM Date  \n' : 'जिसमें पास किया गया एजीएम दिनांक दर्ज करे\n');
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


            if ($scope.SocietyRegisteredOffice.IsRural != undefined && $scope.SocietyRegisteredOffice.IsRural == 1 ) {
                if ($scope.SocietyRegisteredOffice.CITY_ID == undefined || $scope.SocietyRegisteredOffice.CITY_ID == "0") {
                    Msg += (_language == 'en-us' ? 'Please Enter Address-City  \n' : 'Please Enter Address-City  \n\n');
                    Msg += '<br />';
                }
                if ($scope.SocietyRegisteredOffice.WARD_ID == undefined || $scope.SocietyRegisteredOffice.WARD_ID == "0") {
                    Msg += (_language == 'en-us' ? 'Please Enter Address-Ward  \n' : 'Please Enter Address-Ward  \n\n');
                    Msg += '<br />';
                }

            } else if ($scope.SocietyRegisteredOffice.IsRural != undefined && $scope.SocietyRegisteredOffice.IsRural == 2){
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
            $scope.TableData10AllSports
            $scope.TableData10NomiAll
            $scope.TableData10All
            if ($scope.TableData10AllSports != undefined && $scope.TableData10AllSports.length > 0 && $scope.TableData10AllSports.filter(function (x) { return x.IsDelete }).length > 0) {
                $scope.TableData10AllSports.filter(function (x) { return x.IsDelete }).forEach(function (itm, indx) {
                    var index = $scope.TableData10AllSports.findIndex(x => x === itm);
                    if ($scope.GetFileUploaded('flRemovalType_' + index) == '' || itm.RemovalType == '0') {
                        Msg += (_language == 'en-us' ? 'Please choose member removal reason with attachment file\n' : 'Please choose member removal reason with attachment file\n');
                        Msg += '<br />';
                    }
                });
            }
            if ($scope.TableData10NomiAll != undefined && $scope.TableData10NomiAll.length > 0 && $scope.TableData10NomiAll.filter(function (x) { return x.IsDelete }).length > 0) {
                $scope.TableData10NomiAll.filter(function (x) { return x.IsDelete }).forEach(function (itm, indx) {
                    var index = $scope.TableData10NomiAll.findIndex(x => x === itm);
                    if ($scope.GetFileUploaded('flRemovalType_' + index) == '' || itm.RemovalType == '0') {
                        Msg += (_language == 'en-us' ? 'Please choose member removal reason with attachment file\n' : 'Please choose member removal reason with attachment file\n');
                        Msg += '<br />';
                    }
                });
            }
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




        if ($scope.GetFileUploaded('flResoloutionNotice') == undefined || $scope.GetFileUploaded('flResoloutionNotice') == "") {//$scope.ByeLawsSocietyFile == undefined || $scope.ByeLawsSocietyFile == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload Board meeting resoloution Notice with delivery proof' : '\nPlease Upload Board meeting resoloution Notice with delivery proof';
            Msg += '<br />';
        }


        if ($scope.GetFileUploaded('flAttendanceSheet') == undefined || $scope.GetFileUploaded('flAttendanceSheet') == "") {//$scope.ByeLawsSocietyFile == undefined || $scope.ByeLawsSocietyFile == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload Board meeting resoloution MOM And Attendance Sheet' : '\nPlease Upload Board meeting resoloution MOM And Attendance Sheet';
            Msg += '<br />';
        }


        if ($scope.GetFileUploaded('flBylawsFileName') == undefined || $scope.GetFileUploaded('flBylawsFileName') == "") {//$scope.ByeLawsSocietyFile == undefined || $scope.ByeLawsSocietyFile == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload ByeLaws File' : '\nPlease Upload ByeLaws File';
            Msg += '<br />';
        }

        if ($scope.GetFileUploaded('flByLowComparativeDocument') == undefined || $scope.GetFileUploaded('flByLowComparativeDocument') == "") {//$scope.ByeLawsSocietyFile == undefined || $scope.ByeLawsSocietyFile == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload Comparative Document Of ByLow' : '\nPlease Upload Comparative Document Of ByLow';
            Msg += '<br />';
        }

        if ($scope.GetFileUploaded('flAGMDeliveryProof') == undefined || $scope.GetFileUploaded('flAGMDeliveryProof') == "") {//$scope.AGM1File == undefined || $scope.AGM1File == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload AGM Notice with delivery proof' : '\nPlease Upload AGM Notice with delivery proof';
            Msg += '<br />';
        }
        if ($scope.GetFileUploaded('flSGMNotice') == undefined || $scope.GetFileUploaded('flSGMNotice') == "") {//if ($scope.AGM2File == undefined || $scope.AGM2File == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload SGM (Special Gernal meeting)  Notice' : '\nPlease Upload SGM (Special Gernal meeting)  Notice';
            Msg += '<br />';
        }
        if ($scope.GetFileUploaded('flSGMAttendanceSheet') == undefined || $scope.GetFileUploaded('flSGMAttendanceSheet') == "") {//if ($scope.AGM3File == undefined || $scope.AGM3File == '') {
            Msg += $scope._language == 'en-us' ? '\n Please Upload SGM (Special Gernal meeting)  MOM And Attendance Sheet' : '\n Please Upload SGM (Special Gernal meeting)  MOM And Attendance Sheet';
            Msg += '<br />';
        }
        if (Msg.length > 1) {
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + '<br />' + Msg;
            ShowModelAlert("Society Amendment Details", Msg);
            return;
        }

        formData.append("ByeLawsFile", $scope.GetFileUploaded('flBylawsFileName'));//$scope.ByeLawsSocietyFile);
        //formData.append("ResolutionFile", $scope.ResolutionFile);
        formData.append("flAGMDeliveryProof", $scope.GetFileUploaded('flAGMDeliveryProof'));//$scope.AGM1File);
        formData.append("flSGMNotice", $scope.GetFileUploaded('flSGMNotice'));//$scope.AGM2File);
        formData.append("flSGMAttendanceSheet", $scope.GetFileUploaded('flSGMAttendanceSheet'));// $scope.AGM3File);
        formData.append("flByLowComparativeDocument", $scope.GetFileUploaded('flByLowComparativeDocument'));// $scope.AGM3File);
        formData.append("flAttendanceSheet", $scope.GetFileUploaded('flAttendanceSheet'));// $scope.AGM3File);

        formData.append("flResoloutionNotice", $scope.GetFileUploaded('flResoloutionNotice'));// $scope.AGM3File);


        if ($scope.SocietyAmendment.ChaneAddress) {

            formData.append("HouseBuildingNo", $scope.SocietyRegisteredOffice.HouseBuildingNo);
            formData.append("LaneStreetName", $scope.SocietyRegisteredOffice.LaneStreetName);
            formData.append("Locality", $scope.SocietyRegisteredOffice.Locality);
            formData.append("DIVISION_ID", $scope.SocietyRegisteredOffice.DIVISION_ID);
            formData.append("DISTRICT_ID", $scope.SocietyRegisteredOffice.DISTRICT_ID);
            formData.append("IsRural", $scope.SocietyRegisteredOffice.IsRural);
            if ($scope.SocietyRegisteredOffice.IsRural=='True') {
                formData.append("CITY_ID", $scope.SocietyRegisteredOffice.CITY_ID);
                formData.append("WARD_ID", $scope.SocietyRegisteredOffice.WARD_ID);
            } else {
                formData.append("BLOCK_ID", $scope.SocietyRegisteredOffice.BLOCK_ID);
                formData.append("GP_ID", $scope.SocietyRegisteredOffice.GP_ID);
                formData.append("VILLAGE_ID", $scope.SocietyRegisteredOffice.VILLAGE_ID);
            }
            formData.append("PinCode", $scope.SocietyRegisteredOffice.PinCode);
        }




        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");

        if ($scope.SocietyAmendment.ChaneExecutiveMember) {

            if ($scope.ActId != "7") {
                debugger;
                var SocietyRegistrationData = [];

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

                        $scope.TableData10All.forEach(function (itm, indx) {
                            if ($scope.TableData10AllCopy.filter(function (x) {
                                return x.AdministrativeId === itm.AdministrativeId && (x.ContactNo != itm.ContactNo
                                    || x.CategoryId != itm.CategoryId || x.POSTNAME != itm.POSTNAME || x.Occupation != itm.Occupation)
                            }).length > 0) {
                                SocietyRegistrationData.push(
                                    {
                                        "AdministrativeId": itm.NewAdministrativeId == undefined ? 0 : itm.NewAdministrativeId,
                                        "OldAdministrativeId": itm.AdministrativeId,
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
                                        "AdministrativeId": itm.NewAdministrativeId == undefined ? 0 : itm.NewAdministrativeId,
                                        "OldAdministrativeId": itm.AdministrativeId,
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
                                        "RemovalType": itm.RemovalType,
                                        "RemovalFile": itm.IsDelete?$scope.GetFileUploaded('flRemovalType_' + indx):''
                                    });
                            }
                        });
                    }
                    if ($scope.TableData10NomiAll != undefined) {
                        $scope.TableData10NomiAll.forEach(function (itm, indx) {
                            if ($scope.TableData10NomiAllCopy.filter(function (x) {
                                return x.AdministrativeId === itm.AdministrativeId && (x.ContactNo != itm.ContactNo
                                    || x.CategoryId != itm.CategoryId || x.POSTNAME != itm.POSTNAME || x.Occupation != itm.Occupation)
                            }).length > 0) {
                                SocietyRegistrationData.push(
                                    {
                                        "AdministrativeId": itm.NewAdministrativeId == undefined ? 0 : itm.NewAdministrativeId,
                                        "OldAdministrativeId": itm.AdministrativeId,
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
                                        "AdministrativeId": itm.NewAdministrativeId == undefined ? 0 : itm.NewAdministrativeId,
                                        "OldAdministrativeId": itm.AdministrativeId,
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
                                        "RemovalType": itm.RemovalType,
                                        "RemovalFile": itm.IsDelete ? $scope.GetFileUploaded('flRemovalType_' + indx) : ''
                                    });
                            }
                        });
                    }
                }
            }
            else {

                if ($scope.ValidateNominatedMember(1, true)) {
                    var SocietyRegistrationData = [];

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

                        $scope.TableData10AllSports.forEach(function (item, indx) {
                            if ($scope.TableData10AllSportsCopy.filter(function (x) {
                                return x.AdministrativeId === item.AdministrativeId && (x.ContactNo != item.ContactNo
                                    || x.CategoryId != item.CategoryId || x.POSTNAME != item.POSTNAME || x.Occupation != item.Occupation)
                            }).length > 0) {
                                SocietyRegistrationData.push(
                                    {
                                        "AdministrativeId": item.NewAdministrativeId == undefined ? 0 : itm.NewAdministrativeId,
                                        "OldAdministrativeId": item.AdministrativeId, "AADHAARNo": item.AadharNo2,
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
                            } else if ((item.IsNew === true || item.IsDelete === true)) {
                                SocietyRegistrationData.push(
                                    {
                                        "AdministrativeId": item.NewAdministrativeId == undefined ? 0 : itm.NewAdministrativeId,
                                        "OldAdministrativeId": item.AdministrativeId, "AADHAARNo": item.AadharNo2,
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
                                        "RequestType": item.IsDelete ? 1 : 3,
                                        "RemovalType": item.RemovalType,
                                        "RemovalFile": itm.IsDelete ? $scope.GetFileUploaded('flRemovalType_' + indx) : ''
                                    });

                            }
                        });
                    }

                }
            }
            formData.append("DataArray", JSON.stringify(SocietyRegistrationData));
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

        formData.append("SocietyIdEnc", Datasplit[0]);
        formData.append("SocietyDirectory", Datasplit[1]);
        formData.append("RegiYearId", $scope.SocietyAmendment.RegiYearId);
        formData.append("ResolutionNo", $scope.SocietyAmendment.ResolutionNo);
        formData.append("PaasedAGMDate", $("#txtByLawsAGMDate").val().split("/").reverse().join("-"));
        formData.append("ByeLawsFile", $scope.GetFileUploaded('flBylawsFileName'));//$scope.ByeLawsSocietyFile);
        //formData.append("ResolutionFile", $scope.ResolutionFile);
        formData.append("flAGMDeliveryProof", $scope.GetFileUploaded('flAGMDeliveryProof'));//$scope.AGM1File);
        formData.append("flSGMNotice", $scope.GetFileUploaded('flSGMNotice'));//$scope.AGM2File);
        formData.append("flSGMAttendanceSheet", $scope.GetFileUploaded('flSGMAttendanceSheet'));// $scope.AGM3File);
        formData.append("flByLowComparativeDocument", $scope.GetFileUploaded('flByLowComparativeDocument'));// $scope.AGM3File);
        formData.append("flAttendanceSheet", $scope.GetFileUploaded('flAttendanceSheet'));// $scope.AGM3File);
        
        formData.append("flResoloutionNotice", $scope.GetFileUploaded('flResoloutionNotice'));// $scope.AGM3File);
        formData.append("AmendmentType", AmendmentType);// $scope.AGM3File);
        if ($scope.SocietyAmendment.ChangeName) {
            formData.append("Name", $scope.ValidateSocietyName);// $scope.AGM3File);
            formData.append("BRN", $scope.SocietyAmendment.BRN);// $scope.AGM3File);
        }

        formData.append("Remark", $scope.SocietyAmendment.Remark == undefined ? "" : $scope.SocietyAmendment.Remark);
        $(".blockUI").fadeIn("slow");
        //$http.post(SiteUrl + "/Citizen/InsertByLaws", formData,
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



    $scope.SaveMemberList = function () {
        if ($scope.ActId != "7") {
            debugger;
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

                    $scope.TableData10All.forEach(function (itm, indx) {
                        if ($scope.TableData10AllCopy.filter(function (x) {
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
                    $scope.TableData10NomiAll.forEach(function (itm, indx) {
                        if ($scope.TableData10NomiAllCopy.filter(function (x) {
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

                    $scope.TableData10AllSports.forEach(function (itm, indx) {
                        if ($scope.TableData10AllSportsCopy.filter(function (x) {
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
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data == "1") {
                    ShowModelAlert("Society Amendment Details", "Saved Successfully");
                    window.location = "../Citizen/SocietyAmendment"
                } else {
                    alert("There is some problem while submiting data. Please try after some time.")
                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.DistrictList = [];
            });


    }


    $scope.RemoveRowAdminiAct2001 = function (index, ListType) {
        ShowAlertMessage("Alert", _language == 'en-us' ? 'Are you sure you want to delete this Member?' : 'क्या आप वाकई इस सदस्य को हटाना चाहते हैं?', function () {
            if (ListType == 'TableData10All') {
                $scope.TableData10All[index].IsDelete = true;
            } else if (ListType == 'TableData10AllSports') {
                $scope.TableData10AllSports[index].IsDelete = true;
            } else if (ListType == 'TableData10NomiAll') {
                $scope.TableData10NomiAll[index].IsDelete = true;
            }
            $scope.$apply();
        });
    }

    $scope.UndoRemoveRowAdminiAct2001 = function (index, ListType) {
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

            AadhaarAuth($scope.AADHAARNo, $http, "OTP-BIO", false, function (data) {

                if (ListType == 'TableData10All') {
                    $scope.TableData10All[TRID].IsValidate = true;
                } else if (ListType == 'TableData10AllSports') {
                    $scope.TableData10AllSports[TRID].IsValidate = true;
                } else if (ListType == 'TableData10NomiAll') {
                    $scope.TableData10NomiAll[TRID].IsValidate = true;
                }




                $scope.$apply();
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
    $scope.AddMoreMemberDetailsNew = function (Flag, Isvalidate) {
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
                            "RemovalType":"0"
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
                            "RemovalType":"0"
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
                            "RemovalType":"0"
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
                            "RemovalType":"0"
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
                            "RemovalType":"0"
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
                            "RemovalType":"0"
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
                            "RemovalType":"0"
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
                            "RemovalType":"0"
                        });
                }
            }
            else {

            }
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
                                itm.FileName = request.responseText;
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

        $scope.FileCollection = [];

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


        if ($scope.SocietyNameChangeList[0].flResoloutionNotice != '') {
            $scope.FileCollection.push({ 'Id': 'flResoloutionNotice', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flResoloutionNotice, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flResoloutionNotice, FileId: 'flResoloutionNotice', IsFileUpload: true });
        }
        if ($scope.SocietyNameChangeList[0].flAttendanceSheet != '') {
            $scope.FileCollection.push({ 'Id': 'flAttendanceSheet', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flAttendanceSheet, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flAttendanceSheet, FileId: 'flAttendanceSheet', IsFileUpload: true });
        }
        if ($scope.SocietyNameChangeList[0].ByeLawsFile != '') {
            $scope.FileCollection.push({ 'Id': 'flBylawsFileName', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].ByeLawsFile, 'Title': '', FileName: $scope.SocietyNameChangeList[0].ByeLawsFile, FileId: 'flBylawsFileName', IsFileUpload: true });
        }
        if ($scope.SocietyNameChangeList[0].flByLowComparativeDocument != '') {
            $scope.FileCollection.push({ 'Id': 'flByLowComparativeDocument', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flByLowComparativeDocument, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flByLowComparativeDocument, FileId: 'flByLowComparativeDocument', IsFileUpload: true });
        }
        if ($scope.SocietyNameChangeList[0].flAGMDeliveryProof != '') {
            $scope.FileCollection.push({ 'Id': 'flAGMDeliveryProof', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flAGMDeliveryProof, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flAGMDeliveryProof, FileId: 'flAGMDeliveryProof', IsFileUpload: true });
        }
        if ($scope.SocietyNameChangeList[0].flSGMNotice != '') {
            $scope.FileCollection.push({ 'Id': 'flSGMNotice', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flSGMNotice, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flSGMNotice, FileId: 'flSGMNotice', IsFileUpload: true });
        }
        if ($scope.SocietyNameChangeList[0].flSGMAttendanceSheet != '') {
            $scope.FileCollection.push({ 'Id': 'flSGMAttendanceSheet', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flSGMAttendanceSheet, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flSGMAttendanceSheet, FileId: 'flSGMAttendanceSheet', IsFileUpload: true });
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
                $scope.MemberAmendmentList.forEach(function (item, indx) {
                    if (item.RequestType == 3) {
                        item.IsValidate = true;
                        item.IsNew = true;
                        item.Aadh = item.AadharNo;
                        item.NewAdministrativeId = item.AdministrativeId;
                        item.DOB = item.DOB1;
                        item.Photo = '../Content/SocietyProfiling/' + $scope.SocietyDirectory + '/' + item.Photo;
                        if ($scope.TableData10All != undefined && $scope.TableData10All.length > 0) {
                            $scope.TableData10All.push(item);
                        }
                        if ($scope.TableData10AllSports != undefined && $scope.TableData10AllSports.length > 0) {
                            $scope.TableData10AllSports.push(item);
                        }
                        if ($scope.TableData10NomiAll != undefined && $scope.TableData10NomiAll.length > 0) {
                            $scope.TableData10NomiAll.push(item);
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
                            });
                        }
                        if ($scope.TableData10AllSports != undefined && $scope.TableData10AllSports.length > 0) {
                            $scope.TableData10AllSports.filter(function (x) { return x.AdministrativeId === item.oldAdministrativeId }).forEach(function (itm, ind) {
                                itm.ContactNo = item.ContactNo;
                                itm.Occupation = item.Occupation;
                                itm.CategoryId = item.CategoryId;
                                itm.CatId = item.CategoryId;
                                itm.POSTNAME = item.POSTNAME;
                                itm.NewAdministrativeId = item.AdministrativeId;
                            });

                        }
                        if ($scope.TableData10NomiAll != undefined && $scope.TableData10NomiAll.length > 0) {
                            $scope.TableData10NomiAll.filter(function (x) { return x.AdministrativeId === item.oldAdministrativeId }).forEach(function (itm, ind) {
                                itm.ContactNo = item.ContactNo;
                                itm.Occupation = item.Occupation;
                                itm.CategoryId = item.CategoryId;
                                itm.CatId = item.CategoryId;
                                itm.POSTNAME = item.POSTNAME;
                                itm.NewAdministrativeId = item.AdministrativeId;
                            });
                        }


                    } else if (item.RequestType == 1) {
                        if ($scope.TableData10All != undefined && $scope.TableData10All.length > 0) {
                            var newItem = $scope.TableData10All.filter(function (x) { return x.AdministrativeId === item.oldAdministrativeId })[0];
                            var index1 = $scope.TableData10All.findIndex(x => x === newItem);
                            $scope.FileCollection.push({ 'Id': 'flRemovalType_' + index1, 'FileType': '', 'File': item.RemovalFile, 'Title': '', FileName: item.RemovalFile, FileId: 'flRemovalType_' + index1, IsFileUpload: true });
                            newItem.IsDelete = true;
                            newItem.NewAdministrativeId = item.AdministrativeId;
                            newItem.RemovalType = item.RemovalType.toString();
                        }
                        if ($scope.TableData10AllSports != undefined && $scope.TableData10AllSports.length > 0) {

                            var newItem = $scope.TableData10AllSports.filter(function (x) { return x.AdministrativeId === item.oldAdministrativeId })[0];
                            var index1 = $scope.TableData10AllSports.findIndex(x => x === newItem);
                            $scope.FileCollection.push({ 'Id': 'flRemovalType_' + index1, 'FileType': '', 'File': item.RemovalFile, 'Title': '', FileName: item.RemovalFile, FileId: 'flRemovalType_' + index1, IsFileUpload: true });
                            newItem.IsDelete = true;
                            newItem.NewAdministrativeId = item.AdministrativeId;
                            newItem.RemovalType = item.RemovalType.toString();


                            //$scope.TableData10AllSports.filter(function (x) { return x.AdministrativeId === item.oldAdministrativeId })[0].IsDelete = true;
                            //$scope.TableData10AllSports.filter(function (x) { return x.AdministrativeId === item.oldAdministrativeId })[0].NewAdministrativeId = item.AdministrativeId;
                            //$scope.TableData10AllSports.filter(function (x) { return x.AdministrativeId === item.oldAdministrativeId })[0].RemovalType = item.RemovalType.toString();

                        }
                        if ($scope.TableData10NomiAll != undefined && $scope.TableData10NomiAll.length > 0) {
                            var newItem = $scope.TableData10NomiAll.filter(function (x) { return x.AdministrativeId === item.oldAdministrativeId })[0];
                            var index1 = $scope.TableData10NomiAll.findIndex(x => x === newItem);
                            $scope.FileCollection.push({ 'Id': 'flRemovalType_' + index1, 'FileType': '', 'File': item.RemovalFile, 'Title': '', FileName: item.RemovalFile, FileId: 'flRemovalType_' + index1, IsFileUpload: true });
                            newItem.IsDelete = true;
                            newItem.NewAdministrativeId = item.AdministrativeId;
                            newItem.RemovalType = item.RemovalType.toString();
                            //$scope.TableData10NomiAll.filter(function (x) { return x.AdministrativeId === item.oldAdministrativeId })[0].IsDelete = true;
                            //$scope.TableData10NomiAll.filter(function (x) { return x.AdministrativeId === item.oldAdministrativeId })[0].NewAdministrativeId = item.AdministrativeId;
                            //$scope.TableData10NomiAll.filter(function (x) { return x.AdministrativeId === item.oldAdministrativeId })[0].RemovalType = item.RemovalType.toString();
                        }
                    }
                }); 

                  //if (!$scope.IsSocietyEdit) {
                  //  $scope.GetSocietyNameChangeList().then(function () {
                  //      $(".se-pre-con").fadeOut("slow");
                  //  });
                  //} else {
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

                        //var DDLIdLocal = $("#" + id).attr("DDLID");
                        //var DDLIdVal = $("#" + DDLIdLocal).val();

                        //if (!$scope.checkFileLengthPage(file[0].size)) {                       
                        //    alert(_language === 'en-us' ? "File size exceeded.\n Please select files have size less then 2 MB or Equivalent" : "फ़ाइल का आकार से अधिक हो गई है|\nकृपया चुनिंदा फ़ाइलों का आकार 2 MB से कम करें|");
                        //    $(row).val('');
                        //    $(row).parent().find('.filesize').text('');
                        //    return;
                        //} 


                        //var rowDataSplitter = DDLIdLocal.split('_');

                        // The size of the file. 
                        if (file >= 2048) {
                            ShowOkMessage("Warning", scope == 'en-us' ? "File too Big, please select a file less than 2mb" : "फ़ाइल बहुत बड़ी है, कृपया 2mb से कम फ़ाइल का चयन करें", function () { });
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