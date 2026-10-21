var MilkModule = angular.module("MilkMod", ['angularUtils.directives.dirPagination']);

MilkModule.controller("MilkCnt", function ($scope, $http) {

    $scope.IsFirst = 1;
    $scope.IsNewReg = true;
    $scope.SocietyIdEnc = '';
    $scope.SocietyDirectory = '';
    $scope.SocietyIdForAdministrativeDetails = 0;
    $scope.IsEditMode = false;
    $scope.IsProfile = true;
    $scope.IsAdministrative = false;
    $scope.OfficeDetails = false;
    var ImageDataList = [];
    // START : Code for Language label setting
    $scope.getTranslation = function () {

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

    };
    // END : Code for Language label setting

    $scope.ClickUserVarification = function () {
        $scope.IsNewRegistration = $scope.SocietyRegistration.IsNewRegistration;
        if ($scope.SocietyRegistration.IsNewRegistration == 'true') {
            $scope.IsProfiling = false;
            $scope.IsNewReg = false;
        }
        else {
            $scope.IsProfiling = true;
            $scope.IsNewReg = true;
        }
    }
    $scope.Leveldatachange = function (item) {
        $scope.lstSocietyWorkScope = [];
        $scope.SocietyRegistration.Rural = null;
        $scope.SocietyRegistration.urban = null;

        if (item == '1') {
            $scope.RuralurbanDiv = false;
            $scope.RuralDivRadio = false;
            $scope.UrbanDivRadio = false;
            $scope.grid = false;
            $scope.Addmore = false;
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.BlockruralDiv = false;
        }
        if (item == '2') {
            $scope.RuralurbanDiv = true;
            $scope.grid = false;
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.BlockruralDiv = false;
        }

    }
    $scope.LeveldataonUrban = function (item) {

        $scope.lstSocietyWorkScope = [];
        if (item == '2') {

            $scope.UrbanDivRadio = true;
            $scope.RuralDivRadio = false;
            $scope.Addmore = true;
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.BlockruralDiv = false;
            $scope.SocietyRegistration.WorkScopeLevel_ID = '0';
            $scope.SocietyRegistration.DIVISION_ID = '0';
            $scope.SocietyRegistration.DISTRICT_ID = '0';
            $scope.SocietyRegistration.BLOCK_ID = '0';
            $scope.SocietyRegistration.GP_ID = '0';
            $scope.SocietyRegistration.VILLAGE_ID = '0';
        }
        else if (item == '1') {
            $scope.UrbanDivRadio = false;
            $scope.RuralDivRadio = true;
            $scope.Addmore = true;
            $scope.SocietyRegistration.WorkScopeLevel_ID = '0';
            $scope.SocietyRegistration.CityTypes = '0';
            $scope.SocietyRegistration.CITY_ID = '0';
            $scope.SocietyRegistration.WARD_ID = '0';
        }

    }
    //DOC UPLOAD
    $scope.UploadRegistrationCertificateFile = function (file) {
        $scope.RegistrationCertificateFile = file[0];
    }
    $scope.UploadBylawsFile = function (file) {
        $scope.BylawsFile = file[0];
    }
    $scope.UploadRulesAndRegulationFile = function (file) {
        $scope.RulesAndRegulationFile = file[0];
    }
    $scope.UploadMemberListFile = function (file) {
        $scope.MemberListFile = file[0];
    }
    //END
    //start Level of office data
    $scope.Levelofficedata = function () {
        $('#ddlDistrict').val(0);
        $('#ddlblocki').val(0);
        $('#ddlGP').val(0);
        $('#ddlVillage').val(0);
        $('#ddlDivision').val(0);
        var SelectedVal = $scope.SocietyRegistration.WorkScopeLevel_ID;
        $scope.GetSocietyClass();
        if (SelectedVal == '1') {
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.TehsilDiv = false;
            $scope.GPDiv = false;
            $scope.DistrictDiv = false;
            $scope.DivisionDiv = false;
            $scope.StateDiv = true;
            $scope.Addmore = true;
            $scope.ruralurbanb = false;
            $scope.Districtlevel3 = true;
            $scope.Districtlevel1 = false;
            $scope.Districtlevel2 = false;
            $scope.SocietyRegistration.GP_ID = 0;
            $scope.SocietyRegistration.VILLAGE_ID = 0;
            $scope.SocietyRegistration.BLOCK_ID = 0;
            $scope.SocietyRegistration.DISTRICT_ID = '0';
            $scope.District = [];
            //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyRegisteredOffice.DIVISION_ID).
            //    success(function (data, status) {
            //        $scope.District = data;
            //    }).
            //    error(function (data, status) {
            //        $scope.District = [];
            //    });

            //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=STATE&MasterCode=-1").
            //    success(function (data, status) {
            //        $scope.District = data;
            //    }).
            //    error(function (data, status) {
            //        $scope.District = [];
            //    });
        }
        else if (SelectedVal == '2') {
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.TehsilDiv = false;
            $scope.DistrictDiv = false;
            $scope.DivisionDiv = true;
            $scope.GPDiv = false;
            $scope.Addmore = true;
            $scope.ruralurbanb = false;
            $scope.Districtlevel3 = false;
            $scope.Districtlevel1 = false;
            $scope.Districtlevel2 = false;
            $scope.SocietyRegistration.GP_ID = '0';
            $scope.SocietyRegistration.VILLAGE_ID = '0';
            $scope.SocietyRegistration.BLOCK_ID = '0';
            $scope.SocietyRegistration.DISTRICT_ID = '0';

            $scope.Blocks = [];

            //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
            //    success(function (data, status) {
            //        $scope.Blocks = data;
            //    }).
            //    error(function (data, status) {
            //        $scope.Blocks = [];
            //    });
        }
        else if (SelectedVal == '3') {
            $scope.VillageDiv = false;
            $scope.PsDiv = false;
            $scope.BlockruralDiv = false;
            $scope.DistrictDiv = true;
            $scope.DivisionDiv = true;
            //$scope.StateDiv = true;
            $scope.Addmore = true;
            $scope.ruralurbanb = true;
            $scope.GPDiv = false;
            $scope.Districtlevel3 = false;
            $scope.SocietyRegistration.GP_ID = '0';
            $scope.SocietyRegistration.VILLAGE_ID = '0';
            $scope.SocietyRegistration.BLOCK_ID = '0';
            $scope.Blocks = [];

            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegistration.DISTRICT_ID).
                success(function (data, status) {
                    $scope.Blocks = data;
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                });
        }
        else if (SelectedVal == '4') {
            $scope.VillageDiv = false;
            $scope.PsDiv = true;
            $scope.GPDiv = false;
            $scope.DistrictDiv = true;
            $scope.DivisionDiv = true;
            // $scope.StateDiv = true;
            $scope.BlockruralDiv = true;
            $scope.Addmore = true;
            $scope.ruralurbanb = false;
            $scope.Districtlevel3 = true;
            $scope.Districtlevel1 = false;
            $scope.Districtlevel2 = false;
            $scope.SocietyRegistration.GP_ID = '0';
            $scope.SocietyRegistration.VILLAGE_ID = '0';
            $scope.Blocks = [];

            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegistration.DISTRICT_ID).
                success(function (data, status) {
                    $scope.Blocks = data;
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                });
        }
        else if (SelectedVal == '5') {
            $scope.BlockruralDiv = true;
            $scope.VillageDiv = false;
            $scope.PsDiv = true;
            $scope.GPDiv = true;
            $scope.DistrictDiv = true;
            $scope.DivisionDiv = true;
            //$scope.StateDiv = true;
            $scope.Addmore = true;
            //$scope.ruralurbanb = false;
            //$scope.Districtlevel3 = true;
            //$scope.Districtlevel1 = false;
            //$scope.Districtlevel2 = false;

            $scope.Blocks = [];

            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
                success(function (data, status) {
                    $scope.Blocks = data;
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                });
        }
        else if (SelectedVal == '6') {
            $scope.VillageDiv = true;
            $scope.PsDiv = true;
            $scope.GPDiv = true;
            $scope.BlockruralDiv = true;
            $scope.DistrictDiv = true;
            $scope.DivisionDiv = true;
            // $scope.StateDiv = true;
            $scope.Addmore = true;
            $scope.ruralurbanb = false;
            $scope.Districtlevel3 = true;
            $scope.Districtlevel1 = false;
            $scope.Districtlevel2 = false;

            $scope.Blocks = [];

            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
                success(function (data, status) {
                    $scope.Blocks = data;
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                });
        }
        else {
            $scope.StateDiv = false;
            $scope.DivisionDiv = false;
            $scope.DistrictDiv = false;
            $scope.TehsilDiv = false;
            $scope.PsDiv = false;
            $scope.VillageDiv = false;
            $scope.Addmore = false;
            $scope.ruralurbanb = false;
            $scope.Districtlevel1 = false;
            $scope.Districtlevel2 = false;
            $scope.CitytypDiv = false;
        }
    };
    //End Level of office data
    // GET Society Class
    $scope.GetSocietyClass = function () {

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYCLASS&MasterCode=10").
            success(function (data, status) {
                $scope.SocietyClassList = data;
            }).
            error(function (data, status) {
                $scope.SocietyClassList = [];
            });

    }
    //END
    // GET Society Type
    $scope.GetSocietyTypeList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=2").
            success(function (data, status) {
                $scope.SocietyTypeList = data;
            }).
            error(function (data, status) {
                $scope.SocietyTypeList = [];
            });
    };
    //END   
    //START : Get Division List
    $scope.Divisions = [{}];
    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
        success(function (data, status) {
            $scope.Divisions = data;
        }).
        error(function (data, status) {
            $scope.Divisions = [];
        });
    //END
    $scope.GetSocietyDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.SocietyRegistration.DIVISION_ID).
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    // Get List of Block
    $scope.BlockList = function () {
        $scope.Blocks = [];
        $scope.AddMoreForSportShow = true;
        if ($scope.SocietyRegistration.DISTRICT_ID != '0') {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegistration.DISTRICT_ID).
                success(function (data, status) {

                    $scope.Blocks = data;
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
                    $scope.Blocks = [];
                });
        }

    };
    //end block
    //Start
    $scope.GramPanchayatListForBasicD = function () {

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegistration.BLOCK_ID).
            success(function (data, status) {

                $scope.GramPanchayats = data;
            }).
            error(function (data, status) {
                $scope.GramPanchayats = [];

            });
    };
    //End
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
                    $scope.GramPanchayats = [];

                });
        }
        else if ($scope.SocietyRegisteredOffice.BLOCK_IDSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegisteredOffice.BLOCK_IDSecretary).
                success(function (data, status) {

                    $scope.GramPanchayatsSecretary = data;
                }).
                error(function (data, status) {
                    $scope.GramPanchayats = [];

                });
        }
        else if ($scope.SocietyRegisteredOffice.BLOCK_IDTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.SocietyRegisteredOffice.BLOCK_IDTreasurer).
                success(function (data, status) {

                    $scope.GramPanchayatsTreasurer = data;
                }).
                error(function (data, status) {
                    $scope.GramPanchayats = [];

                });
        }


    };
    // END : Get List of GramPanchayat
    $scope.Addmorebtn = function (ValidationFlag) {
        //if (ValidationFlag) {
        //    if (!$scope.FormValidationOthersocity(ValidationFlag)) {
        //        return false;
        //    }
        //}
        if (($scope.SocietyRegistration.CityTypes > 0 || $scope.SocietyRegistration.DIVISION_ID > 0 || $scope.SocietyRegistration.IsStatLevel) && ValidationFlag) {

            if ($scope.lstSocietyWorkScope.length != 0 && ValidationFlag == undefined) {
                if (parseInt(($scope.lstSocietyWorkScope[0]).WorkScopeLevel_ID, 10) != (parseInt($scope.SocietyRegistration.WorkScopeLevel_ID, 10))) {
                    alert('data is not at same level hence can not be added.');
                    return;
                }
                if ($scope.SocietyRegistration.DIVISION_ID != 0) {
                    var _data = $scope.lstSocietyWorkScope.filter(x => x.WorkScopeLevel_ID == $('#static').val()
                        && x.DIVISION_ID == $('#ddlDivision').val()
                        && x.DISTRICT_ID == $('#ddlDistrict').val() == null ? 0 : $('#ddlDistrict').val()
                            && x.BLOCK_ID == $('#ddlblocki').val() == null ? 0 : $('#ddlblocki').val()
                                && x.GP_ID == $('#ddlGP').val() == null ? 0 : $('#ddlGP').val()
                                    && x.VILLAGE_ID == $('#ddlVillage').val() == null ? 0 : $('#ddlVillage').val()
                        //&& x.WorkScopeLevel_ID == $('#static option:selected').val()
                    );
                }
                else if ($scope.SocietyRegistration.CityTypes != 0) {
                    var _data = $scope.lstSocietyWorkScope.filter(x =>
                        x.CityTypes == $('#staticall').val()
                        && x.CITY_ID == $('#ddlCity').val()
                        && x.WARD_ID == $('#ddlward').val()

                    );
                }
                if (_data.length > 0) {
                    alert(_language == 'en-us' ? 'record already exist\n' : 'रिकॉर्ड पहले से मौजूद है \n');
                    return;
                }

            }

            if (($scope.SocietyRegistration.IsStatLevel == 'No' || $scope.SocietyRegistration.CityTypes > '0' || $scope.SocietyRegistration.DIVISION_ID > '0') && ($scope.SocietyRegistration.WorkScopeLevel_ID != '0' || $scope.SocietyRegistration.CityTypes != '0')) {
                $scope.grid = true;
                $scope.lstSocietyWorkScope.push(
                    {
                        "WorkScopeLevel_ID": parseInt($('#static option:selected').val()),
                        "STATE_ID": 6,
                        "IsStatLevel": 0,
                        "DIVISION_ID": $('#ddlDivision').val() == null ? 0 : $('#ddlDivision').val(),
                        "DISTRICT_ID": $('#ddlDistrict').val() == null ? 0 : $('#ddlDistrict').val(),
                        "BLOCK_ID": $('#ddlblocki').val() == null ? 0 : $('#ddlblocki').val(),
                        "VILLAGE_ID": $('#ddlVillage').val() == null ? 0 : $('#ddlVillage').val(),
                        "CITY_ID": $('#ddlCity').val() == null ? 0 : $('#ddlCity').val(),
                        "WARD_ID": $('#ddlward').val() == null ? 0 : $('#ddlward').val(),
                        "GP_ID": $('#ddlGP').val() == null ? 0 : $('#ddlGP').val(),
                        "CityTypes": $('#staticall').val() == null ? 0 : $('#staticall').val(),
                        "IsRuralUrban": $('#ISrurl').prop('checked') == true ? 1 : 0,


                        "WorkScopeLevel_Name": parseInt($('#static option:selected').val()) > 0 ? $('#static option:selected').text() : 'N/A',
                        "STATE_Name": 'Rajasthan' ? 'राजस्थान' : 'N/A',
                        "DIVISION_Name": parseInt($('#ddlDivision option:selected').val()) > 0 ? $('#ddlDivision option:selected').text() : 'N/A',
                        "DISTRICT_ENG": parseInt($('#ddlDistrict option:selected').val()) > 0 ? $('#ddlDistrict option:selected').text() : 'N/A',
                        "BLOCK_Name": parseInt($('#ddlblocki option:selected').val()) > 0 ? $('#ddlblocki option:selected').text() : 'N/A',
                        "VILLAGE_Name": parseInt($('#ddlVillage option:selected').val()) > 0 ? $('#ddlVillage option:selected').text() : 'N/A',
                        "CityTypes_Name": parseInt($('#staticall option:selected').val()) > 0 ? $('#staticall option:selected').text() : 'N/A',
                        "CITY_Name": parseInt($('#ddlCity option:selected').val()) > 0 ? $('#ddlCity option:selected').text() : 'N/A',
                        "WARD_Name": parseInt($('#ddlward option:selected').val()) > 0 ? $('#ddlward option:selected').text() : 'N/A',
                        "GP_ID_Name": parseInt($('#ddlGP option:selected').val()) > 0 ? $('#ddlGP option:selected').text() : 'N/A',
                    }
                );
            }
            else if ($scope.SocietyRegistration.IsStatLevel == 'Yes') {
                $scope.grid = false;
                $scope.lstSocietyWorkScope.push(
                    {
                        "WorkScopeLevel_ID": 1,
                        "IsStatLevel": 1,
                        "STATE_ID": 6,
                        "DIVISION_ID": 0,
                        "DISTRICT_ID": 0,
                        "BLOCK_ID": 0,
                        "VILLAGE_ID": 0,
                        "CITY_ID": 0,
                        "WARD_ID": 0,
                        "GP_ID": 0,
                        "CityTypes": 0
                    }
                );
            }
            else if ($scope.SocietyRegistration.IsStatLevel == 'No' && ValidationFlag == undefined) {
                // alert("Please Choose level of work scope");
            }

            if ($scope.lstSocietyWorkScope.length > 0) {
                $scope.SaveDiv = true;
            }
            else {
                $scope.SaveDiv = false;
            }
            $scope.SocietyRegistration.WorkScopeLevel = '0';
            $scope.SocietyRegistration.STATE_ID = '0';
            $scope.SocietyRegistration.DIVISION_ID = '0';
            $scope.SocietyRegistration.DISTRICT_ID = '0';
            $scope.SocietyRegistration.TEHSIL_ID = '0';
            $scope.SocietyRegistration.BLOCK_ID = '0';
            $scope.SocietyRegistration.VILLAGE_ID = '0';
            //$scope.SocietyRegistration.Rural = null;
            //$scope.SocietyRegistration.urban = null;
            $scope.SocietyRegistration.CityTypes = '0';
            $scope.SocietyRegistration.CITY_ID = '0';
            $scope.SocietyRegistration.WARD_ID = '0';

            //$scope.SocietyRegistration.No = null;
            //$scope.SocietyRegistration.Yes = null;

            $scope.SocietyRegistration.GP_ID = '0';
            $scope.SocietyRegistration.WorkScopeLevel_ID = '0';
        }

    }
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
    $scope.GetWardListbySociety = function () {
        $scope.Ward = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ward&MasterCode=" + $scope.SocietyRegistration.CITY_ID).
            success(function (data, status) {
                $scope.Ward = data;
            }).
            error(function (data, status) {
                $scope.Ward = [];
            });
    };
    // START
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
    //END
    //START : Get City List
    $scope.GetCityList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode=" + $scope.SocietyRegistration.CityTypes).
            success(function (data, status) {
                $scope.CityList = data;
            }).
            error(function (data, status) {
                $scope.CityList = [];
            });


    };

    $scope.GetCityListByCityTypes = function (id) {

        if ($scope.SocietyRegisteredOffice.CityTypes != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode=" + $scope.SocietyRegisteredOffice.CityTypes).
                success(function (data, status) {
                    $scope.CityListdata = data;
                }).
                error(function (data, status) {
                    $scope.CityListdata = [];
                });
        }
        if ($scope.SocietyRegisteredOffice.CityTypes_copy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode=" + $scope.SocietyRegisteredOffice.CityTypes_copy).
                success(function (data, status) {
                    $scope.CityListdatacopy = data;
                }).
                error(function (data, status) {
                    $scope.CityListdata = [];
                });
        }
        if ($scope.SocietyRegisteredOffice.CityTypesSecretary != '0' && id == 3) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode=" + $scope.SocietyRegisteredOffice.CityTypesSecretary).
                success(function (data, status) {
                    $scope.CityListdataSecretary = data;
                }).
                error(function (data, status) {
                    $scope.CityListdata = [];
                });
        }
        if ($scope.SocietyRegisteredOffice.CityTypesTreasurer != '0' && id == 4) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode=" + $scope.SocietyRegisteredOffice.CityTypesTreasurer).
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
    // END : Get Ward List

    // START : Save Society Profiling
    $scope.SaveSocietyProfiling = function () {
        if ($scope.FormValidation()) {
            var formData = new FormData();
            formData.append("BRN", $scope.SocietyRegistration.BRN);
            formData.append("SocietyIDEnc", $scope.SocietyRegistration.SocietyIdEnc);
            formData.append("ActId", 3);
            formData.append("RegistrationNo", $scope.SocietyRegistration.RegistrationNo);
            formData.append("SocietyName", $scope.SocietyRegistration.SocietyName);
            formData.append("SocietyTypeId", $scope.SocietyRegistration.SocietyTypeId);
            formData.append("ClassId", $scope.SocietyRegistration.ClassId);
            formData.append("SubClassId", $scope.SocietyRegistration.SubClassId);
            formData.append("SocietyCategoryId", $scope.SocietyRegistration.SocietyCategoryId);
            formData.append("ByeLawsSocietyFile", $scope.ByeLawsSocietyFile);
            formData.append("RegistrationDate", $('#txtRegistrationDate').val().split("/").reverse().join("-"));
            formData.append("ElectionDate", $('#txtRegistrationDate').val().split("/").reverse().join("-"));
            // formData.append("NoOfBranches", $scope.SocietyRegistration.NoOfBranches);
            //formData.append("IsStateLevel", $scope.SocietyRegistration.Yes ? '1' : '2');
            formData.append("LOSocietyOfficeDetails", $scope.SocietyRegistration.WorkScopeLevel);
            formData.append("STATE_ID", $scope.SocietyRegistration.STATE_ID);
            formData.append("DIVISION_ID", $scope.SocietyRegistration.DIVISION_ID);
            formData.append("DISTRICT_ID", $scope.SocietyRegistration.DISTRICT_ID);
            formData.append("TEHSIL_ID", $scope.SocietyRegistration.TEHSIL_ID);
            formData.append("BLOCK_ID", $scope.SocietyRegistration.BLOCK_ID);
            formData.append("VILLAGE_ID", $scope.SocietyRegistration.VILLAGE_ID);
            formData.append("GeneralMeetingProceedingFile", $scope.GeneralMeetingProceeding);
            formData.append("BalanceSheetFile", $scope.BalanceSheetFile);

            formData.append("RegistrationCertificateFile", $scope.RegistrationCertificateFile);
            formData.append("RulesAndRegulationFile", $scope.RulesAndRegulationFile);
            formData.append("ByeLawsSocietyFile", $scope.BylawsFile);

            formData.append("lstSocietyWorkScope", JSON.stringify($scope.lstSocietyWorkScope));
            formData.append("IsNewRegistration", $scope.IsNewRegistration);

            $http.post(SiteUrl + "/Society/SaveSocietyProfiling", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    if ((data.data.split('@')).length > 1) {
                        ShowModelAlert("Society Profiling", "Basic Details Update Sucessfully");
                        $scope.EMitraTransactionId = (data.data.split('@')).length > 1 ? (data.data.split('@'))[1] : '0';
                        //$scope.SocietyRegistration.SocietyIdEnc = data.data.split('@')[0];
                        $scope.SocietyIdEnc = data.data.split('@')[0];
                        if ($scope.IsEditMode == false) {
                            $scope.ActId = 3;
                            $scope.SocietyDirectory = data.societydirectory;
                            $scope.SocietyIdForAdministrativeDetails = data.data;
                            ShowModelAlert("Society Profiling", "Basic Details Saved Sucessfully");
                            $scope.divSocietyProfilingShow = false;
                            $scope.divAddBtn = false;
                            $scope.lstSocietyWorkScope = [];
                            $("#SocietyStep_1").removeClass("active stepactive1");
                            $("#SocietyStep_1").addClass("disabled stepactive1");
                            $("#SocietyStep_2").addClass("active stepactive2");

                            $("#step1").removeClass("tab-pane active");
                            $("#step1").addClass("tab-pane ng-hide");
                            $("#step2").removeClass("tab-pane ng-hide");
                            $("#step2").addClass("tab-pane active");
                            $scope.IsFirst = 2;
                            $scope.IsProfile = false;
                            $scope.IsAdministrative = true;

                        }
                        else {

                            $scope.ShowDetails($scope.SocietyRegistration.SocietyIdEnc, 'ShowDetails');
                            $scope.ShowDetailsWorkscope($scope.SocietyRegistration.SocietyIdEnc);
                            $scope.SocietyIdEnc = $scope.SocietyRegistration.SocietyIdEnc;
                            var $active = $('.wizard .nav-tabs li.active');
                            prevTab($active);
                            $active.next().removeClass('disabled');
                            var $active = $('.wizardShow .nav-tabs li.active').attr('id');
                            $scope.divSocietyProfilingShow = true;
                            $scope.IsFirst = 2;
                            $scope.divSocialProfiling = false;

                            $("#SocietyStep_22").removeClass("active stepactive1");
                            $("#SocietyStep_22").addClass("disabled stepactive1");
                            $("#SocietyStep_11").removeClass("active stepactive3");

                            $("#step22").removeClass("tab-pane active");
                            $("#step22").addClass("tab-pane ng-hide");
                            $("#step11").removeClass("tab-pane ng-hide");
                            $("#step11").addClass("tab-pane active");

                            $("#SocietyStep_22").removeClass("disabled stepactive1");
                            $("#SocietyStep_22").addClass("active stepactive1");
                            $("#SocietyStep_11").addClass("disabled stepactive3");

                            $("#step22").removeClass("tab-pane ng-hide");
                            $("#step22").addClass("tab-pane active");
                            $("#step11").removeClass("tab-pane active");
                            $("#step11").addClass("tab-pane ng-hide");
                            $('.wizardShow .nav-tabs li.active .connecting-line').css({ "border-bottom-left-radius": 0, "border-top-left-radius": 0 });

                        }

                    }
                    else if (data.data.length > 10) {
                        $scope.ActId = $scope.SocietyRegistration.ActId;
                        $scope.SocietyDirectory = data.societydirectory;
                        $scope.SocietyIdForAdministrativeDetails = data.data;
                        ShowModelAlert("Society Profiling", "Basic Details Saved Sucessfully");
                        $scope.divSocietyProfilingShow = false;
                        $scope.divAddBtn = false;
                        $scope.lstSocietyWorkScope = [];
                        $("#SocietyStep_1").removeClass("active stepactive1");
                        $("#SocietyStep_1").addClass("disabled stepactive1");
                        $("#SocietyStep_2").addClass("active stepactive2");

                        $("#step1").removeClass("tab-pane active");
                        $("#step1").addClass("tab-pane ng-hide");
                        $("#step2").removeClass("tab-pane ng-hide");
                        $("#step2").addClass("tab-pane active");
                        $scope.IsFirst = 2;
                        if ($scope.ActId != "7") {
                            $scope.DivOther = true;
                            $scope.DivSport = false;
                        }
                        else {
                            $scope.DivOther = false;
                            $scope.DivSport = true;
                        }
                    }
                    $scope.ClearForm();


                }).
                error(function (data, status) {

                    ShowModelAlert("Society Profiling", "Error While Society Profiling Insert");
                    $('#btnAdministrativedetails').trigger('click');
                });


        }

    };
    //END
    //START : 
    $scope.AdministrativeBoard = function () {
        if ($scope.SocietyRegistration.IsAdministrative == "Administrative") {
            $scope.IsAdministrativeY = true;
            $scope.IsAdministrativeN = false;
            $scope.AppointedYN = false;
            $scope.CEO = true;
            $scope.Isliquidator = false;
            $scope.SocietyRegistration.IsAdministratorAppointed = false;
        }
        else if ($scope.SocietyRegistration.IsAdministrative == "Board") {
            $scope.IsAdministrativeY = true;
            $scope.IsAdministrativeN = true;
            $scope.CEO = true;
            $scope.Isliquidator = false;
        }
        else if ($scope.SocietyRegistration.IsAdministrative == "liquidator") {
            $scope.IsAdministrativeY = false;
            $scope.IsAdministrativeN = false;
            $scope.CEO = false;
            $scope.AppointedYN = false;
            $scope.SocietyRegistration.IsAdministratorAppointed = false;
            $scope.Isliquidator = true;
        }
        $("input[type=text]").val("");
        $("select").prop("selectedIndex", 0);

    };
    //END
    $scope.ClearForm = function () {
        $scope.ActId = $scope.SocietyRegistration.ActId;
        $scope.SocietyRegistration.ActId = $scope.ActId;
        $scope.SocietyRegistration.CEOCategoryId = '0';
        $scope.SocietyRegistration.ChairmanCategoryId = '0';
        $scope.SocietyRegistration.AppointedCategoryId = '0';
        $scope.SocietyRegistration.CategoryId1 = '0';
        $scope.SocietyRegistration.CategoryId2 = '0';
        $scope.SocietyRegistration.PostId1 = '0';
        $scope.SocietyRegistration.PostId2 = '0';
        $scope.SocietyRegistration.ElCategoryId1 = '0';
        $scope.SocietyRegistration.ElPostId1 = '0';
        $scope.SocietyRegistration.CEOCategoryId = '0';
        $scope.SocietyRegistration.ChairmanCategoryId = '0';
        $scope.SocietyRegistration.AppointedCategoryId = '0';
        $scope.SocietyRegistration.CategoryId1 = '0';
        $scope.SocietyRegistration.CategoryId2 = '0';
        $scope.SocietyRegistration.PostId1 = '0';
        $scope.SocietyRegistration.PostId2 = '0';
        $scope.SocietyRegistration.ElCategoryId1 = '0';
        $scope.SocietyRegistration.ElPostId1 = '0';
        $scope.SocietyRegistration.BRN = '';
        $scope.SocietyRegistration.RegistrationNo = '';
        $scope.SocietyRegistration.SocietyName = '';
        $scope.SocietyRegistration.RegistrationDate = '';
        $scope.SocietyRegistration.SocietyTypeId = '0';
        $scope.SocietyRegistration.ClassId = '0';
        $scope.SocietySubClassList = [];
        $scope.SocietyRegistration.SubClassId = '0';
        $scope.SocietyRegistration.Rural = undefined;
        $scope.SocietyRegistration.IsStatLevel = undefined;
        $scope.SocietyRegistration.WorkScopeLevel_ID = '0';
        $scope.SocietyRegistration.DIVISION_ID = '0';
        $scope.SocietyRegistration.DISTRICT_ID = '0';
        $scope.SocietyRegistration.BLOCK_ID = '0';
        $scope.SocietyRegistration.GP_ID = '0';
        $scope.SocietyRegistration.VILLAGE_ID = '0';
        $scope.SocietyRegistration.CITY_ID = '0';
        $scope.SocietyRegistration.CityTypes = '0';
        $scope.SocietyRegistration.WARD_ID = '0';
        $("#flRegistrationCertificateFile").val('');
        $("#flRulesAndRegulation").val('');
        $("#flBylawsFile").val('');
        $("#flBylawsFile").parent().find('.filesize').text('');
        $("#flRulesAndRegulation").parent().find('.filesize').text('');
        $("#flRegistrationCertificateFile").parent().find('.filesize').text('');

    };
    //validateAddForm START

    // START : Save Society Administrative Details
    $scope.SaveSocietyAdministrativeDetails = function () {
        if ($scope.SocietyRegistration.ActId == undefined && $scope.ActId > 0) {
            $scope.SocietyRegistration.ActId = $scope.ActId;
        }

        $scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdEnc;
        $scope.SocietyRegistration.ActId = 3;

        if ($scope.AdministrativeFormValidation()) {
            if ($scope.SocietyRegistration.IsAdministrative == 'Board') {
                if (!$scope.CategoryGenderFormValidation()) {
                    return;
                }
            }
            var SocietyRegistrationData = [];
            var formData = new FormData();
            formData.append("ActId", $scope.SocietyRegistration.ActId);
            formData.append("CEOSocietyFile", $scope.CEOSocietyFile);
            formData.append("ChairmanFile", $scope.ChairmanFile);
            formData.append("AppointedFile", $scope.AppointedFile);
            formData.append("AdministrativeFile1", $scope.AdministrativeFile1);
            formData.append("AdministrativeFile2", $scope.AdministrativeFile2);
            formData.append("AdministrativeFile3", $scope.AdministrativeFile3);
            formData.append("AdministrativeFile4", $scope.AdministrativeFile4);
            formData.append("AdministrativeFile5", $scope.AdministrativeFile5);
            formData.append("AdministrativeFile6", $scope.AdministrativeFile6);
            formData.append("AdministrativeFile7", $scope.AdministrativeFile7);
            formData.append("AdministrativeFile8", $scope.AdministrativeFile8);
            formData.append("AdministrativeFile9", $scope.AdministrativeFile9);
            formData.append("AdministrativeFile10", $scope.AdministrativeFile10);
            formData.append("EMitraTransactionId", $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId);

            var Count = 10;
            var tempRowNo = 1;
            var _data1 = ImageDataList.sort(function (a, b) { return a.RowNo - b.RowNo });
            for (var Y = 0; Y < ImageDataList.length; Y++) {
                for (; tempRowNo < ImageDataList[Y].RowNo; tempRowNo++) {
                    formData.append("TempOtherAdministrativeFile" + (tempRowNo + Count), ImageDataList[Y].File);
                }
                formData.append("OtherAdministrativeFile" + (ImageDataList[Y].RowNo + Count), ImageDataList[Y].File);
                tempRowNo++;
            }

            formData.append("ClassA", $scope.SocietyRegistration.TotalMembersClassA);
            formData.append("ClassB", $scope.SocietyRegistration.TotalMembersClassB);
            formData.append("ClassC", $scope.SocietyRegistration.TotalMembersClassC);
            formData.append("SocietyDirectory", $scope.SocietyDirectory);


            //$scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdForAdministrativeDetails.split('@')[0];
            $scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdEnc;


            if ($scope.SocietyRegistration.IsAdministrative == 'Administrative' || $scope.SocietyRegistration.IsAdministrative == 'Board') {
                SocietyRegistrationData.push(
                    {
                        //"SocietyId": $scope.SocietyIdForAdministrativeDetails,
                        "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                        "Name": $scope.SocietyRegistration.CEOName,
                        "IsActive": true,
                        "ContactNo": $scope.SocietyRegistration.CEOContactNo,
                        "AADHAARNo": $scope.SocietyRegistration.CEOAADHAARNo,
                        "DOB": $('#txtCEODOB').val().split("/").reverse().join("-"),
                        "Gender": $scope.SocietyRegistration.CEOGender,
                        "UserType": "1",
                        "AdministratorDesignationId": "-1",
                        "CategoryId": $scope.SocietyRegistration.CEOCategoryId,
                        "PostId": $scope.SocietyRegistration.CEOPostId,
                        "Photo": $scope.SocietyRegistration.CEOSocietyFile
                    });
            }
            if ($scope.SocietyRegistration.IsAdministrative == 'Administrative' || $scope.SocietyRegistration.IsAdministrative == 'Board') {
                SocietyRegistrationData.push(
                    {
                        //"SocietyId": $scope.SocietyIdForAdministrativeDetails,
                        "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                        "Name": $scope.SocietyRegistration.ChairmanName,
                        "IsActive": true,
                        "ContactNo": $scope.SocietyRegistration.ChairmanContactNo,
                        "AADHAARNo": $scope.SocietyRegistration.ChairmanAADHAARNo,
                        "DOB": $('#txtChairmanDOB').val().split("/").reverse().join("-"),
                        "Gender": $scope.SocietyRegistration.ChairmanGender,
                        "UserType": "2",
                        "AdministratorDesignationId": "-1",
                        "CategoryId": $scope.SocietyRegistration.ChairmanCategoryId,
                        "PostId": $scope.SocietyRegistration.ChairmanPostId,
                        "Photo": $scope.SocietyRegistration.ChairmanFile

                    });
            }
            if ($scope.SocietyRegistration.IsAdministratorAppointed == 'true' && $scope.SocietyRegistration.AppointedCategoryId > 0 && $scope.SocietyRegistration.AppointedPostId > 0) {
                SocietyRegistrationData.push(
                    {
                        //"SocietyId": $scope.SocietyIdForAdministrativeDetails,
                        "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                        "Name": $scope.SocietyRegistration.AppointedName,
                        "IsActive": true,
                        "ContactNo": $scope.SocietyRegistration.AppointedContactNo,
                        "AADHAARNo": $scope.SocietyRegistration.AppointedAADHAARNo,
                        "DOB": $('#txtAppointedDOB').val().split("/").reverse().join("-"),
                        "Gender": $scope.SocietyRegistration.AppointedGender,
                        "UserType": "3",
                        //"AdministratorDesignationId": $scope.SocietyRegistration.DESIGNATIONCODE,
                        "AdministratorDesignationId": "-1",
                        "CategoryId": $scope.SocietyRegistration.AppointedCategoryId,
                        "PostId": $scope.SocietyRegistration.AppointedPostId,
                        "Photo": $scope.SocietyRegistration.AppointedFile
                    });
            }
            else {
                SocietyRegistrationData.push(
                    {
                        //"SocietyId": $scope.SocietyIdForAdministrativeDetails,
                        "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                        "Name": "",
                        "IsActive": true,
                        "ContactNo": "",
                        "AADHAARNo": "",
                        "DOB": new Date(),
                        "Gender": "M",
                        "UserType": "3",
                        //"AdministratorDesignationId": $scope.SocietyRegistration.DESIGNATIONCODE,
                        "AdministratorDesignationId": "-1",
                        "CategoryId": 0,
                        "PostId": 0
                    });
            }

            if ($scope.SocietyRegistration.IsAdministrative == 'liquidator') {
                SocietyRegistrationData.push(
                    {
                        //"SocietyId": $scope.SocietyIdForAdministrativeDetails,
                        "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                        "Name": $scope.SocietyRegistration.liquidatorName,
                        "IsActive": true,
                        "ContactNo": $scope.SocietyRegistration.liquidatorContactNo,
                        "AADHAARNo": $scope.SocietyRegistration.liquidatorAADHAARNo,
                        "DOB": $('#txtliquidatorDOB').val().split("/").reverse().join("-"),
                        "AppointedDate": $('#txtliquidatorAppointedDate').val().split("/").reverse().join("-"),
                        "Gender": $scope.SocietyRegistration.liquidatorGender,
                        "UserType": "6",
                        "AdministratorDesignationId": "-1",
                        "CategoryId": $scope.SocietyRegistration.liquidatorCategoryId,
                        "PostId": $scope.SocietyRegistration.liquidatorPostId,
                        "Photo": $scope.SocietyRegistration.liquidatorFile
                    });
            }

            if ($scope.SocietyRegistration.IsAdministrative == 'Board') {
                $('#myTable tr').each(function () {

                    if ($(this).find('td:eq(7)').find('select').val() > 0 && $(this).find('td:eq(6)').find('select').val() > 0) {
                        SocietyRegistrationData.push(
                            {
                                //"SocietyId": $scope.SocietyIdForAdministrativeDetails,
                                "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                "Name": $(this).find('td:eq(1)').find('input').val(),
                                "IsActive": true,
                                "ContactNo": $(this).find('td:eq(2)').find('input').val(),
                                "AADHAARNo": $(this).find('td:eq(3)').find('input').val(),
                                "DOB": $(this).find('td:eq(4)').find('input').val().split("/").reverse().join("-"),
                                "Gender": $(this).find('td:eq(5)').find('input:checked').val(),
                                "CategoryId": $(this).find('td:eq(6)').find('select').val(),
                                "PostId": $(this).find('td:eq(7)').find('select').val(),
                                "UserType": "4",//Elected board Menbers
                                "AdministratorDesignationId": "-1",
                                "Photo": $(this).find('td:eq(8)').find('label').html()
                            });
                    }
                    //SocietyRegistrationData.push({ "SocietyId": $scope.SocietyIdForAdministrativeDetails, "Name": $(this).find('td:eq(1)').find('input').val(), "ContactNo": $(this).find('td:eq(2)').find('input').val(), "UserType": "3" });
                });
            }

            if ($scope.SocietyRegistration.IsAdministrative == 'Board') {
                $('#myTable2 tr').each(function () {
                    if ($(this).find('td:eq(7)').find('select').val() > 0 && $(this).find('td:eq(6)').find('select').val() > 0) {
                        SocietyRegistrationData.push(
                            {
                                //"SocietyId": $scope.SocietyIdForAdministrativeDetails,
                                "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                "Name": $(this).find('td:eq(1)').find('input').val(),
                                "IsActive": true,
                                "ContactNo": $(this).find('td:eq(2)').find('input').val(),
                                "AADHAARNo": $(this).find('td:eq(3)').find('input').val(),
                                "DOB": $(this).find('td:eq(4)').find('input').val().split("/").reverse().join("-"),
                                "Gender": $(this).find('td:eq(5)').find('input:checked').val(),
                                "CategoryId": $(this).find('td:eq(6)').find('select').val(),
                                "PostId": $(this).find('td:eq(7)').find('select').val(),
                                "UserType": "5",//Nominated member
                                "AdministratorDesignationId": "-1",
                                "Photo": $(this).find('td:eq(8)').find('label').html()
                            });
                    }
                    //SocietyRegistrationData.push({ "SocietyId": $scope.SocietyIdForAdministrativeDetails, "Name": $(this).find('td:eq(1)').find('input').val(), "ContactNo": $(this).find('td:eq(2)').find('input').val(), "UserType": "3" });
                });
            }

            formData.append("DataArray", JSON.stringify(SocietyRegistrationData));
            $http.post(SiteUrl + "/Society/SaveSocietyAdministrativeDetails", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    if ((data.split('@')).length > 1) {
                        $scope.EMitraTransactionId = (data.split('@')).length > 1 ? (data.split('@'))[1] : '0';
                        $scope.SocietyRegistration.SocietyIdEnc = data.split('@')[0];
                        $scope.SocietyIdEnc = data.split('@')[0];
                        ShowModelAlert("Society Profiling", "Society Administrative Details Update Sucessfully");
                        var $active = $('.wizard .nav-tabs li.active');
                        prevTab($active);
                        $active.next().removeClass('disabled');
                        var $active = $('.wizardShow .nav-tabs li.active').attr('id');
                        if ($scope.IsEditMode == false) {
                            //$scope.ActId = $scope.SocietyRegistration.ActId;
                            $scope.SocietyDirectory = data.societydirectory;
                            $scope.SocietyIdForAdministrativeDetails = data.data;
                            ShowModelAlert("Society Profiling", "Society Administrative Details Save Sucessfully");
                            $scope.divSocietyProfilingShow = false;
                            $scope.divAddBtn = false;
                            $scope.lstSocietyWorkScope = [];
                            $("#SocietyStep_1").addClass("disabled stepactive1");
                            $("#SocietyStep_2").removeClass("active stepactive1");
                            $("#SocietyStep_2").addClass("disabled stepactive1");
                            $("#SocietyStep_3").addClass("active stepactive2");
                            $("#step1").addClass("tab-pane ng-hide");
                            $("#step2").removeClass("tab-pane active");
                            $("#step2").addClass("tab-pane ng-hide");
                            $("#step3").addClass("tab-pane active");
                            $scope.IsFirst = 3;
                            $scope.OfficeDetails = true;
                            $scope.IsAdministrative = false;
                            $scope.IsProfile = false;
                        }
                        else {
                            alert('ELSE For Admoin');
                            $scope.ClearSocietyAdministrativeDetails();
                            $scope.divSocietyProfilingShow = true;
                            $scope.IsFirst = 3;
                            var $active = $('.wizard .nav-tabs li.active');
                            prevTab($active);
                            $active.next().removeClass('disabled');
                            var $active = $('.wizardShow .nav-tabs li.active').attr('id');
                            $scope.divSocialProfiling = false;
                            $("#SocietyStep_33").removeClass("disabled stepactive1");
                            $("#SocietyStep_33").addClass("active stepactive1");
                            $("#SocietyStep_22").addClass("disabled stepactive3");
                            $("#step33").removeClass("tab-pane ng-hide");
                            $("#step33").addClass("tab-pane active");
                            $("#step22").removeClass("tab-pane active");
                            $("#step22").addClass("tab-pane ng-hide");
                            $('.wizardShow .nav-tabs li.active .connecting-line').css({ "border-bottom-left-radius": 0, "border-top-left-radius": 0 });
                        }
                    }
                    else {
                        $scope.ClearSocietyAdministrativeDetails();
                        ShowModelAlert("Society Profiling", "Society Administrative Details Save Sucessfully");
                        if ($scope.IsEditMode == false) {
                            //$scope.ActId = $scope.SocietyRegistration.ActId;
                            $scope.SocietyDirectory = data.societydirectory;
                            $scope.SocietyIdForAdministrativeDetails = data.data;
                            ShowModelAlert("Society Profiling", "Society Administrative Details Save Sucessfully");
                            $scope.divSocietyProfilingShow = false;
                            $scope.divAddBtn = false;
                            $scope.lstSocietyWorkScope = [];

                            $("#SocietyStep_1").addClass("disabled stepactive1");
                            $("#SocietyStep_2").removeClass("active stepactive1");
                            $("#SocietyStep_2").addClass("disabled stepactive1");
                            $("#SocietyStep_3").addClass("active stepactive2");


                            $("#step1").addClass("tab-pane ng-hide");
                            $("#step2").removeClass("tab-pane active");
                            $("#step2").addClass("tab-pane ng-hide");
                            $("#step3").addClass("tab-pane active");
                            $scope.IsFirst = 3;
                            if ($scope.ActId != "7") {
                                $scope.DivOther = true;
                                $scope.DivSport = false;
                            }
                            else {
                                $scope.DivOther = false;
                                $scope.DivSport = true;
                            }
                        }
                    }
                }).
                error(function (data, status) {
                    $scope.DistrictList = [];
                });

        }


        $scope.SocietyRegisteredOffice.STATE_ID = '6';
        $scope.SocietyRegisteredOffice.STATE_IDcopy = '6';
        $scope.SocietyRegisteredOffice.STATE_IDSecretary = '6';
        $scope.SocietyRegisteredOffice.STATE_IDTreasurer = '6';
    };
    // END : Save Society Administrative Details
    $scope.FormValidation = function () {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';


        //if ($scope.SocietyRegistration.BRN == undefined && $scope.SocietyRegistration.ActId != "7") {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please Enter BRN No.' : '\n <br /> कृपया बीआरएन नंबर दर्ज करें |';
        //}
        if ($scope.SocietyRegistration.IsNewRegistration == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please select The Type Of Application.' : '\n <br /> कृपया आवेदन के प्रकार का चयन करे |';
        }


        if ($scope.SocietyRegistration.RegistrationNo == undefined && $scope.SocietyRegistration.IsNewRegistration == false) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society Registration No.' : '\n <br /> सोसायटी पंजीकरण संख्या दर्ज करें |';

        }
        if ($scope.SocietyRegistration.SocietyName == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter Society Name' : '\n <br /> कृपया सोसाइटी का नाम दर्ज करें |';

        }

        if ($('#txtRegistrationDate').val().length == 0 && $scope.SocietyRegistration.IsNewRegistration == false) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Registration Date' : '\n <br /> कृपया पंजीकरण की तारीख का चयन करे |';

        }
        //if ($('#txtElectionDate').val().length == 0 ) {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please Select Election Date' : '\n <br /> कृपया चुनाव तिथि का चयन करें |';

        //}
        if ($scope.SocietyRegistration.SocietyTypeId == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Type' : '\n <br /> कृपया सोसाइटी का प्रकार चुनें |';

        }
        if ($scope.SocietyRegistration.ClassId == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Class' : '\n <br /> कृपया सोसाइटी क्लास का चयन करे  |';

        }

        if ($scope.SocietyRegistration.SubClassId == 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Sub Society Class' : '\n <br /> कृपया उप सोसाइटी क्लास का चयन करे |';

        }
        if (($scope.RegistrationCertificateFile == undefined || $scope.RegistrationCertificateFile == "")) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Registration Certificate(PDF)' : '\n <br /> कृपया पंजीकरण प्रमाण पत्र अपलोड करें (पीडीएफ) |';

        }
        if ($scope.RulesAndRegulationFile == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Rules And Regulation(PDF)' : '\n <br /> कृपया नियम और अधिनियम अपलोड करें (पीडीएफ) |';

        }
        if ($scope.BylawsFile == undefined) {
            message += $scope._language == 'en-us' ? '\n <br />Please Upload ByeLows Certificate(PDF)' : '\nकृपया कानून प्रमाण पत्र द्वारा अपलोड करें (पीडीएफ) |';
        }

        if ($scope.SocietyRegistration.SocietyClassId == 0 && $scope.SocietyCategoryList.Count > 0) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Society Category' : '\n <br /> कृपया सोसायटी श्रेणी का चयन करें |';

        }


        if ($scope.lstSocietyWorkScope.length == '0') {
            if ($scope.SocietyRegistration.WorkScopeLevel == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Level of Society Work Scope.' : '\n <br /> कृपया सोसाइटी कार्य स्कोप स्तर चुनें |';
            }
        }

        //if ($scope.SocietyRegistration.NoOfBranches == null) {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please Enter No of Branches.' : '\n <br /> शाखाओं में से कोई भी दर्ज करें |';

        //}

        if ($scope.SocietyRegistration.IsStatLevel == 'No') {
            if ($scope.SocietyRegistration.Rural == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Choose the Rural/Urban.' : '\n <br /> कृपया ग्रामीण / शहरी चुनें|';

            }
        }


        if ($scope.SocietyRegistration.IsStatLevel == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Choose Is work scope is state level ?' : '\n <br /> क्या कार्यक्षेत्र राज्य स्तर है ?';

        }

        if ($scope.SocietyRegistration.Rural == 'false' && $scope.lstSocietyWorkScope.length < 1) {

            if ($scope.SocietyRegistration.CityTypes == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the City Type.' : '\n <br /> कृपया शहर का प्रकार चुनें |';

            }
            if ($scope.SocietyRegistration.CITY_ID == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the City .' : '\n <br /> कृपया शहर का चयन करें |';

            }
            if ($scope.SocietyRegistration.WARD_ID == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the ward .' : '\n <br /> कृपया वार्ड का चयन करें |';

            }
        }



        if ($scope.SocietyRegistration.Rural == 'true' && $scope.lstSocietyWorkScope.length < 1) {
            if ($scope.SocietyRegistration.WorkScopeLevel_ID == '0') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Choose Level of work scope.' : '\n <br /> कृपया कार्यक्षेत्र का स्तर चुनें |';

            }
        }


        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '2') {

            if ($scope.SocietyRegistration.DIVISION_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Level of Society Work Scope.' : '\n <br /> कृपया डिवीजन स्तर का चयन करें |';

        }
        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '3') {

            if ($scope.SocietyRegistration.DIVISION_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Level of Society Work Scope.' : '\n <br /> कृपया डिवीजन स्तर का चयन करें |';

            if ($scope.SocietyRegistration.DISTRICT_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the District level.' : '\n <br /> जिला स्तर का चयन करें |';

        }

        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '4') {

            if ($scope.SocietyRegistration.DIVISION_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Division level.' : '\n <br /> कृपया डिवीजन स्तर का चयन करें |';


            if ($scope.SocietyRegistration.DISTRICT_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the District level.' : '\n <br /> जिला स्तर का चयन करें |';


            if ($scope.SocietyRegistration.BLOCK_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Block level.' : '\n <br /> कृपया ब्लॉक स्तर का चयन करें|';

        }


        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '5') {

            if ($scope.SocietyRegistration.DIVISION_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Division level.' : '\n <br /> कृपया डिवीजन स्तर का चयन करें |';


            if ($scope.SocietyRegistration.DISTRICT_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the District level.' : '\n <br /> जिला स्तर का चयन करें |';



            if ($scope.SocietyRegistration.BLOCK_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Block level.' : '\n <br /> कृपया ब्लॉक स्तर का चयन करें |';


            if ($scope.SocietyRegistration.GP_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Gram panchayat level.' : '\n <br /> कृपया ग्राम पंचायत स्तर का चयन करें |';

        }
        if ($scope.SocietyRegistration.WorkScopeLevel_ID == '6') {

            if ($scope.SocietyRegistration.DIVISION_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Division level.' : '\n <br /> कृपया डिवीजन स्तर का चयन करें |';


            if ($scope.SocietyRegistration.DISTRICT_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the District level.' : '\n <br /> जिला स्तर का चयन करें |';


            if ($scope.SocietyRegistration.BLOCK_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Block level.' : '\n <br /> कृपया ब्लॉक स्तर का चयन करें |';


            if ($scope.SocietyRegistration.GP_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Gram panchayat level.' : '\n <br /> कृपया ग्राम पंचायत स्तर का चयन करें |';


            if ($scope.SocietyRegistration.VILLAGE_ID == '0')
                message += $scope._language == 'en-us' ? '\n <br /> Please Select the Village level.' : '\n <br /> कृपया गांव स्तर का चयन करें |';

        }
        if (message.length > 29) {
            ShowModelAlert("Society Profiling", message);
            return false;
        }
        return true;
    }
    //END
    //START AdministrativeFormValidation  
    $scope.AdministrativeFormValidation = function () {
        var validate = false;
        var Count = 0;

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';


        if ($scope.SocietyRegistration.IsAdministrative == undefined) {

            message += $scope._language == 'en-us' ? '\n <br /> Please Select Administrative Type' : '\n <br /> प्रशासनिक प्रकार का चयन करे |';
        }
        //CEO
        if ($scope.SocietyRegistration.IsAdministrative == 'Administrative' || $scope.SocietyRegistration.IsAdministrative == 'Board') {
            if ($scope.SocietyRegistration.CEOName == undefined || $scope.SocietyRegistration.CEOName == '') {

                message += $scope._language == 'en-us' ? '\n <br /> Please Enter CEO Name' : '\n <br /> मुख्य संचालक का नाम दर्ज करें |';
            }
            if ($scope.SocietyRegistration.CEOContactNo == undefined || $scope.SocietyRegistration.CEOContactNo == '') {

                message += $scope._language == 'en-us' ? '\n <br /> Please Enter CEO Contact No.' : '\n <br /> कृपया मुख्य संचालक का वैध मोबाइल नंबर दर्ज करें |';
            }
            if ($('#txtCEODOB').val() == '') {

                message += $scope._language == 'en-us' ? '\n <br /> Please Enter CEO Date of Birth' : '\n <br /> कृपया मुख्य संचालक की जन्म तारीख दर्ज करे |';

            }
            if ($scope.SocietyRegistration.CEOGender == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select CEO Gender Type.' : '\n <br /> कृपया मुख्य संचालक के लिंग का चयन करे |';

            }
            if ($scope.SocietyRegistration.CEOAADHAARNo == undefined || $scope.SocietyRegistration.CEOAADHAARNo == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter CEO AADHAAR No.' : '\n <br /> कृपया मुख्य संचालक का आधार नंबर दर्ज करें |';

            }
            if ($scope.SocietyRegistration.CEOPostId == "0" || $scope.SocietyRegistration.CEOPostId == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select The Post Of CEO.' : '\n <br /> कृपया मुख्य संचालक के पद का चयन करें |';

            }
            if ($scope.SocietyRegistration.CEOCategoryId == "0" || $scope.SocietyRegistration.CEOCategoryId == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select The Category Of CEO.' : '\n <br /> कृपया मुख्य संचालक के वर्ग का चयन करें |';

            }
        }
        //END

        if ($scope.SocietyRegistration.IsAdministrative == 'Administrative' || $scope.SocietyRegistration.IsAdministrative == 'Board') {
            //Chairman
            if ($scope.SocietyRegistration.ChairmanName == undefined || $scope.SocietyRegistration.ChairmanName == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Chairman Name' : '\n <br /> अध्यक्ष का नाम दर्ज करें |';

            }

            if ($('#txtChairmanDOB').val() == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Chairman Date of Birth' : '\n <br /> कृपया अध्यक्ष की जन्म तारीख दर्ज करे |';

            }

            if ($scope.SocietyRegistration.ChairmanGender == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Chairman Gender Type.' : '\n <br /> कृपया अध्यक्ष के लिंग का चयन करे |';

            }

            if ($scope.SocietyRegistration.ChairmanPostId == "0" || $scope.SocietyRegistration.ChairmanPostId == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select The Post Of Chairman.' : '\n <br /> कृपया अध्यक्ष के पद का चयन करें |';

            }
            if ($scope.SocietyRegistration.ChairmanCategoryId == "0" || $scope.SocietyRegistration.ChairmanCategoryId == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select The Category Of Chairman.' : '\n <br /> अध्यक्ष के वर्ग का चयन करें |';

            }

            if ($scope.SocietyRegistration.IsAdministratorAppointed == undefined && $scope.ActId == 3) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Administrator Appointment Status.' : '\n <br /> कृपया प्रशासक नियुक्ति स्थिति का चयन करें |';

            }

            //END
        }

        //Administrator
        if ($scope.SocietyRegistration.IsAdministratorAppointed == 'true') {
            if ($scope.SocietyRegistration.AppointedName == undefined || $scope.SocietyRegistration.AppointedName == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Administrator Name' : '\n <br /> प्रशासक का नाम दर्ज करें |';

            }

            if ($('#txtAppointedDOB').val() == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Administrator  Date of Birth' : '\n <br /> कृपया प्रशासक की जन्म तारीख दर्ज करे |';

            }

            if ($scope.SocietyRegistration.AppointedCategoryId == "0" || $scope.SocietyRegistration.AppointedCategoryId == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select The Category Of Administrator.' : '\n <br /> प्रशासक के वर्ग का चयन करें |';

            }

            if ($scope.SocietyRegistration.AppointedGender == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Administrator Gender Type.' : '\n <br /> कृपया प्रशासक के लिंग का चयन करे |';

            }
            if ($scope.SocietyRegistration.AppointedPostId == "0" || $scope.SocietyRegistration.AppointedPostId == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Administrator Post.' : '\n <br /> कृपया प्रशासक के पद का चयन करे |';

            }
            //if ($scope.SocietyRegistration.DESIGNATIONCODE == "0") {

        }
        //END

        //liquidator
        if ($scope.SocietyRegistration.IsAdministrative == 'liquidator') {
            if ($scope.SocietyRegistration.liquidatorName == undefined || $scope.SocietyRegistration.liquidatorName == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter liquidator Name' : '\n <br /> परिसमापक का नाम दर्ज करें |';

            }

            if ($('#txtliquidatorDOB').val() == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter liquidator Date of Birth' : '\n <br /> कृपया परिसमापक की जन्म तिथि दर्ज करे |';

            }

            if ($('#txtliquidatorAppointedDate').val() == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter liquidator Date of Appointment' : '\n <br /> कृपया परिसमापक की नियुक्ति तिथि दर्ज करे |';

            }

            if ($scope.SocietyRegistration.liquidatorCategoryId == "0" || $scope.SocietyRegistration.liquidatorCategoryId == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select The Category Of liquidator.' : '\n <br /> कृपया परिसमापक के वर्ग का चयन करें |';

            }

            if ($scope.SocietyRegistration.liquidatorGender == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select liquidator Gender Type.' : '\n <br /> कृपया परिसमापक के लिंग का चयन करे |';

            }
            if ($scope.SocietyRegistration.liquidatorPostId == "0" || $scope.SocietyRegistration.liquidatorPostId == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select liquidator Post.' : '\n <br /> कृपया परिसमापक के पद का चयन करे |';

            }
        }
        //END

        if ($scope.SocietyRegistration.IsAdministrative == 'Board') {
            //Menber1
            if ($scope.SocietyRegistration.DirectorsName1 == undefined || $scope.SocietyRegistration.DirectorsName1 == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter First Directors Name' : '\n <br /> प्रथम निदेशक का नाम दर्ज करें |';

            }
            if ($scope.SocietyRegistration.ContactNo1 != undefined) {
                if ($scope.SocietyRegistration.ContactNo1.length < 10) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Valid First Directors Contact No.' : '\n <br /> कृपया प्रथम निदेशक का वैध मोबाइल नंबर दर्ज करें |';

                }
            }
            if ($('#txtDOB1').val() == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter First Directors Date of Birth' : '\n <br /> कृपया प्रथम निदेशक की जन्म तारीख दर्ज करे |';

            }
            //END

            //Menber2
            if ($scope.SocietyRegistration.DirectorsName2 == undefined || $scope.SocietyRegistration.DirectorsName2 == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Second Directors Name' : '\n <br /> द्वितीय निदेशक का नाम दर्ज करें |';

            }
            if ($scope.SocietyRegistration.ContactNo2 != undefined) {
                if ($scope.SocietyRegistration.ContactNo2.length < 10) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Valid Second Directors Contact No.' : '\n <br /> कृपया द्वितीय निदेशक का वैध मोबाइल नंबर दर्ज करें |';

                }
            }
            if ($('#txtDOB2').val() == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Second Directors Date of Birth' : '\n <br /> कृपया द्वितीय निदेशक की जन्म तारीख दर्ज करे |';

            }
            //END

            //Menber3
            if ($scope.SocietyRegistration.DirectorsName3 == undefined || $scope.SocietyRegistration.DirectorsName3 == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Third Directors Name' : '\n <br /> कृपया तृतीय निदेशक का नाम दर्ज करें |';

            }
            if ($scope.SocietyRegistration.ContactNo3 != undefined) {
                if ($scope.SocietyRegistration.ContactNo3.length < 10) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Valid Third Directors Contact No.' : '\n <br /> कृपया तृतीय निदेशक का वैध मोबाइल नंबर दर्ज करें |';

                }
            }
            if ($('#txtDOB3').val() == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Third Directors Date of Birth' : '\n <br /> कृपया तृतीय निदेशक की जन्म तारीख दर्ज करे |';

            }
            //END

            //Menber4
            if ($scope.SocietyRegistration.DirectorsName4 == undefined || $scope.SocietyRegistration.DirectorsName4 == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Fourth Directors Name' : '\n <br /> कृपया चौथे  निदेशक का नाम दर्ज करें |';

            }
            if ($scope.SocietyRegistration.ContactNo4 != undefined) {
                if ($scope.SocietyRegistration.ContactNo4.length < 10) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Valid Fourth Directors Contact No.' : '\n <br /> कृपया चौथे  निदेशक का वैध मोबाइल नंबर दर्ज करें |';

                }
            }
            if ($('#txtDOB4').val() == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Fourth Directors Date of Birth' : '\n <br /> कृपया चौथे  निदेशक की जन्म तारीख दर्ज करे |';

            }
            //END

            //Menber5
            if ($scope.SocietyRegistration.DirectorsName5 == undefined || $scope.SocietyRegistration.DirectorsName5 == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Fifth Directors Name' : '\n <br /> कृपया पांचवे निदेशक का नाम दर्ज करें |';

            }
            if ($scope.SocietyRegistration.ContactNo5 != undefined) {
                if ($scope.SocietyRegistration.ContactNo5.length < 10) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Valid Fifth Directors Contact No.' : '\n <br /> कृपया पांचवे निदेशक का वैध मोबाइल नंबर दर्ज करें |';

                }
            }
            if ($('#txtDOB5').val() == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Fifth Directors Date of Birth' : '\n <br /> कृपया पांचवे निदेशक की जन्म तारीख दर्ज करे |';

            }
            //END

            //Menber6
            if ($scope.SocietyRegistration.DirectorsName6 == undefined || $scope.SocietyRegistration.DirectorsName6 == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Sixth Directors Name' : '\n <br /> कृपया छटवे निदेशक का नाम दर्ज करें |';

            }
            if ($scope.SocietyRegistration.ContactNo6 != undefined) {
                if ($scope.SocietyRegistration.ContactNo6.length < 10) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Valid Sixth Directors Contact No.' : '\n <br /> कृपया छटवे निदेशक का वैध मोबाइल नंबर दर्ज करें |';

                }
            }
            if ($('#txtDOB6').val() == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Sixth Directors Date of Birth' : '\n <br /> कृपया छटवे निदेशक की जन्म तारीख दर्ज करे |';

            }
            //END

            //Menber7
            if ($scope.SocietyRegistration.DirectorsName7 == undefined || $scope.SocietyRegistration.DirectorsName7 == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Seventh Directors Name' : '\n <br /> कृपया सातवें निदेशक का नाम दर्ज करें |';

            }
            if ($scope.SocietyRegistration.ContactNo7 != undefined) {
                if ($scope.SocietyRegistration.ContactNo7.length < 10) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Valid Seventh Directors Contact No.' : '\n <br /> कृपया सातवें निदेशक का वैध मोबाइल नंबर दर्ज करें |';

                }
            }
            if ($('#txtDOB7').val() == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Seventh Directors Date of Birth' : '\n <br /> कृपया सातवें निदेशक की जन्म तारीख दर्ज करे |';

            }
            //END

            //Menber8
            if ($scope.SocietyRegistration.DirectorsName8 == undefined || $scope.SocietyRegistration.DirectorsName8 == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Eighth Directors Name' : '\n <br /> कृपया आठवें निदेशक का नाम दर्ज करें |';

            }
            if ($scope.SocietyRegistration.ContactNo8 != undefined) {
                if ($scope.SocietyRegistration.ContactNo8.length < 10) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Valid Eighth Directors Contact No.' : '\n <br /> कृपया आठवें निदेशक का वैध मोबाइल नंबर दर्ज करें |';

                }
            }
            if ($('#txtDOB8').val() == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Eighth Directors Date of Birth' : '\n <br /> कृपया आठवें निदेशक की जन्म तारीख दर्ज करे |';

            }
            //END

            //Menber9
            if ($scope.SocietyRegistration.DirectorsName9 == undefined || $scope.SocietyRegistration.DirectorsName9 == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Ninth Directors Name' : '\n <br /> कृपया नवें निदेशक का नाम दर्ज करें |';

            }
            if ($scope.SocietyRegistration.ContactNo9 != undefined) {
                if ($scope.SocietyRegistration.ContactNo9.length < 10) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Valid Ninth Directors Contact No.' : '\n <br /> कृपया नवें निदेशक का वैध मोबाइल नंबर दर्ज करें |';

                }
            }
            if ($('#txtDOB9').val() == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Ninth Directors Date of Birth' : '\n <br /> कृपया नवें निदेशक की जन्म तारीख दर्ज करे |';

            }
            //END

            //Menber10
            if ($scope.SocietyRegistration.DirectorsName10 == undefined || $scope.SocietyRegistration.DirectorsName10 == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Tenth Directors Name' : '\n <br /> कृपया दसवें निदेशक का नाम दर्ज करें |';

            }
            if ($scope.SocietyRegistration.ContactNo10 != undefined) {
                if ($scope.SocietyRegistration.ContactNo10.length < 10) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Valid Tenth Directors Contact No.' : '\n <br /> कृपया दसवें निदेशक का वैध मोबाइल नंबर दर्ज करें |';

                }
            }
            if ($('#txtDOB10').val() == '') {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Tenth Directors Date of Birth' : '\n <br /> कृपया दसवें निदेशक की जन्म तारीख दर्ज करे |';

            }
            //END
        }

        if ($scope.SocietyRegistration.IsAdministrative == 'false') {

            if ($scope.SocietyRegistration.Gender1 == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select First Director Gender Type.' : '\n <br /> कृपया प्रथम निदेशक के लिंग का चयन करे |';

            }
            if ($scope.SocietyRegistration.Gender2 == undefined) {

                message += $scope._language == 'en-us' ? '\n <br /> Please Select Second Director Gender Type.' : '\n <br /> कृपया द्वितीय निदेशक के लिंग का चयन करे |';

            }
            if ($scope.SocietyRegistration.Gender3 == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Third Director Gender Type.' : '\n <br /> कृपया तृतीय निदेशक के लिंग का चयन करे |';

            }
            if ($scope.SocietyRegistration.Gender4 == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Fourth Director Gender Type.' : '\n <br /> कृपया चौथे निदेशक के लिंग का चयन करे |';

            }
            if ($scope.SocietyRegistration.Gender5 == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Fifth Director Gender Type.' : '\n <br /> कृपया पांचवे निदेशक के लिंग का चयन करे |';

            }
            if ($scope.SocietyRegistration.Gender6 == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Sixth Director Gender Type.' : '\n <br /> कृपया छटवे निदेशक के लिंग का चयन करे |';

            }
            if ($scope.SocietyRegistration.Gender7 == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Seventh Director Gender Type.' : '\n <br /> कृपया सातवें निदेशक के लिंग का चयन करे |';

            }
            if ($scope.SocietyRegistration.Gender8 == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Eighth Director Gender Type.' : '\n <br /> कृपया आठवें निदेशक के लिंग का चयन करे |';

            }
            if ($scope.SocietyRegistration.Gender9 == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Ninth Director Gender Type.' : '\n <br /> कृपया नवें निदेशक के लिंग का चयन करे |';

            }

            if ($scope.SocietyRegistration.Gender10 == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Tenth Director Gender Type.' : '\n <br /> कृपया दसवें निदेशक के लिंग का चयन करे |';

            }
            //Category
            if ($scope.SocietyRegistration.CategoryId1 == undefined || $scope.SocietyRegistration.CategoryId1 == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select First Director Category.' : '\n <br /> कृपया प्रथम निदेशक के वर्ग का चयन करे |';

            }
            if ($scope.SocietyRegistration.CategoryId2 == undefined || $scope.SocietyRegistration.CategoryId2 == "0") {

                message += $scope._language == 'en-us' ? '\n <br /> Please Select Second Director Category.' : '\n <br /> कृपया द्वितीय निदेशक के वर्ग का चयन करे |';

            }
            if ($scope.SocietyRegistration.CategoryId3 == undefined || $scope.SocietyRegistration.CategoryId3 == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Third Director Category.' : '\n <br /> कृपया तृतीय निदेशक के वर्ग का चयन करे |';

            }
            if ($scope.SocietyRegistration.CategoryId4 == undefined || $scope.SocietyRegistration.CategoryId4 == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Fourth Director Category.' : '\n <br /> कृपया चौथे निदेशक के वर्ग का चयन करे |';

            }
            if ($scope.SocietyRegistration.CategoryId5 == undefined || $scope.SocietyRegistration.CategoryId5 == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Fifth Director Category.' : '\n <br /> कृपया पांचवे निदेशक के वर्ग का चयन करे |';

            }
            if ($scope.SocietyRegistration.CategoryId6 == undefined || $scope.SocietyRegistration.CategoryId6 == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Sixth Director Category.' : '\n <br /> कृपया छटवे निदेशक के वर्ग का चयन करे |';

            }
            if ($scope.SocietyRegistration.CategoryId7 == undefined || $scope.SocietyRegistration.CategoryId7 == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Seventh Director Category.' : '\n <br /> कृपया सातवें निदेशक वर्ग लिंग का चयन करे |';

            }
            if ($scope.SocietyRegistration.CategoryId8 == undefined || $scope.SocietyRegistration.CategoryId8 == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Eighth Director Category.' : '\n <br /> कृपया आठवें निदेशक वर्ग लिंग का चयन करे |';

            }
            if ($scope.SocietyRegistration.CategoryId9 == undefined || $scope.SocietyRegistration.CategoryId9 == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Ninth Director Category.' : '\n <br /> कृपया नवें निदेशक के वर्ग का चयन करे |';

            }
            if ($scope.SocietyRegistration.CategoryId10 == undefined || $scope.SocietyRegistration.CategoryId10 == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Tenth Director Category.' : '\n <br /> कृपया दसवें निदेशक के वर्ग का चयन करे |';

            }

            //Post
            if ($scope.SocietyRegistration.PostId1 == undefined || $scope.SocietyRegistration.PostId1 == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select First Director Post.' : '\n <br /> कृपया प्रथम निदेशक के पद का चयन करे |';

            }
            if ($scope.SocietyRegistration.PostId2 == undefined || $scope.SocietyRegistration.PostId2 == "0") {

                message += $scope._language == 'en-us' ? '\n <br /> Please Select Second Director Post.' : '\n <br /> कृपया द्वितीय निदेशक के पद का चयन करे |';

            }
            if ($scope.SocietyRegistration.PostId3 == undefined || $scope.SocietyRegistration.PostId3 == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Third Director Post.' : '\n <br /> कृपया तृतीय निदेशक के पद का चयन करे |';

            }
            if ($scope.SocietyRegistration.PostId4 == undefined || $scope.SocietyRegistration.PostId4 == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Fourth Director Post.' : '\n <br /> कृपया चौथे निदेशक के पद का चयन करे |';

            }
            if ($scope.SocietyRegistration.PostId5 == undefined || $scope.SocietyRegistration.PostId5 == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Fifth Director Post.' : '\n <br /> कृपया पांचवे निदेशक के पद का चयन करे |';

            }
            if ($scope.SocietyRegistration.PostId6 == undefined || $scope.SocietyRegistration.PostId6 == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Sixth Director Post.' : '\n <br /> कृपया छटवे निदेशक के पद का चयन करे |';

            }
            if ($scope.SocietyRegistration.PostId7 == undefined || $scope.SocietyRegistration.PostId7 == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Seventh Director Post.' : '\n <br /> कृपया सातवें निदेशक पद लिंग का चयन करे |';

            }
            if ($scope.SocietyRegistration.PostId8 == undefined || $scope.SocietyRegistration.PostId8 == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Eighth Director Post.' : '\n <br /> कृपया आठवें निदेशक पद लिंग का चयन करे |';

            }
            if ($scope.SocietyRegistration.PostId9 == undefined || $scope.SocietyRegistration.PostId9 == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Ninth Director Post.' : '\n <br /> कृपया नवें निदेशक के पद का चयन करे |';

            }
            if ($scope.SocietyRegistration.PostId10 == undefined || $scope.SocietyRegistration.PostId10 == "0") {
                message += $scope._language == 'en-us' ? '\n <br /> Please Select Tenth Director Post.' : '\n <br /> कृपया दसवें निदेशक के पद का चयन करे |';

            }
        }

        if ($scope.SocietyRegistration.IsAdministrative == 'Board') {
            if ($scope.SocietyRegistration.TotalMembersClassA == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Total Members Class A.' : '\n <br /> कृपया कक्षा ए के कुल सदस्य दर्ज करें |';

            }
            if ($scope.SocietyRegistration.TotalMembersClassB == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Total Members Class B.' : '\n <br /> कृपया कक्षा बी के कुल सदस्य दर्ज करें |';

            }
            if ($scope.SocietyRegistration.TotalMembersClassC == undefined) {
                message += $scope._language == 'en-us' ? '\n <br /> Please Enter Total Members Class C.' : '\n <br /> कृपया कक्षा सी के कुल सदस्य दर्ज करें |';

            }
        }



        //End Sport Association

        if (message.length > 29) {
            ShowModelAlert("Administrative Form Validation", message);
            return false;
        }
        return true;
    }
    //validateAddForm for farmer ends
    // START : Get Society Category List
    $scope.GetSocietyCategory = function () {
        $http.get(SiteUrl + "/Master/SocietyCategoryList/?ClassId=" + $scope.SocietyRegistration.ClassId + "&SubClassId=" + $scope.SocietyRegistration.SubClassId).
            success(function (data, status) {
                $scope.SocietyCategoryList = data;

            }).
            error(function (data, status) {
                $scope.SocietyCategoryList = [];
            });
    };
    // END :Society Category
    // START : Get Society Category List
    $scope.GetDistrictMilkUnionList = function () {
        $http.get(SiteUrl + "/Master/DistrictMilkUnionList").
            success(function (data, status) {
                $scope.MilkUnionList = data;

            }).
            error(function (data, status) {
                $scope.MilkUnionList = [];
            });
    };
    // END :Society Category
    //START : 
    $scope.AppointedShowHide = function () {
        if ($scope.SocietyRegistration.IsAdministratorAppointed == "false") {
            $scope.AppointedYN = false;
        }
        else {
            $scope.AppointedYN = true;
            //$scope.DesignationList()
        }
    };
    //END
    // START : Get Sub Class
    $scope.GetSubClass = function () {
        $http.get(SiteUrl + "/Master/SubClassList/?ClassId=" + $scope.SocietyRegistration.ClassId).
            success(function (data, status) {
                $scope.SocietySubClassList = data;
            }).
            error(function (data, status) {
                $scope.SocietySubClassList = [];
            });
    };
    // END  Sub Class

    // START : Set Rural/ Urban div show/hide
    $scope.RuralUrbanBlock = function () {
        var SelectedVal = $scope.SocietyRegisteredOffice.IsRural;
        if (SelectedVal == 'True' || SelectedVal == 'true') {
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;
        }
        else if (SelectedVal == 'false' || SelectedVal == 'False') {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
        }
        else {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
        }
    };

    $scope.RuralUrbanBlockCopy = function () {

        var SelectedVal = $scope.SocietyRegisteredOffice.IsRuralcopy;

        if (SelectedVal == 'true' || SelectedVal == 'True') {
            $scope.UrbanDivCopy = true;
            $scope.RuralDivcopy = false;
        }
        else if (SelectedVal == 'false' || SelectedVal == 'False') {
            $scope.UrbanDivCopy = false;
            $scope.RuralDivcopy = true;
        }
        else {
            $scope.UrbanDivCopy = false;
            $scope.RuralDivcopy = false;
        }
    };
    //END

    // START : Code for File Upload For Administrative 
    $scope.UploadByeCEO = function (file) {
        $scope.CEOSocietyFile = file[0];
    }

    $scope.UploadByChairman = function (file) {
        $scope.ChairmanFile = file[0];
    }
    //END
    //start BlockListByOffAddress form
    $scope.BlockListByOffAdd = function (id) {
        $scope.Blocks = [];
        if ($scope.SocietyRegisteredOffice.DISTRICT_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
                success(function (data, status) {

                    $scope.Blocksdata = data;
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
                    $scope.Blocks = [];
                });
        }
        else if ($scope.SocietyRegisteredOffice.DISTRICT_ID_copy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.SocietyRegisteredOffice.DISTRICT_ID_copy).
                success(function (data, status) {

                    $scope.Blocksdatacopy = data;
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
                    $scope.Blocks = [];
                });
        }
    }

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.getTranslation();
        $scope.GetSocietyTypeList();
        $scope.GetSocietyClass();
        $scope.GetDistrictMilkUnionList();
        $('#txtRegistrationDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $('.VoterDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D',
            defaultDate: (new Date()).getDate() + '/' + ((new Date()).getMonth()) + '/' + ((new Date()).getFullYear() - 18).toString(),
            yearRange: "-100:-18"

        });

        //Get CEO Post List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=9").
            success(function (data, status) {
                $scope.Posts = data;
            }).
            error(function (data, status) {


            });
        // END : Post List for dropdown

        //Get Chairman Post List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=10").
            success(function (data, status) {
                $scope.ChairmanPosts = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });
        // END : Post List for dropdown

        //Get Board Member Post List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=-2").
            success(function (data, status) {
                $scope.BoardPosts = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });
        // END : Post List for dropdown

        //Get Chairman Category List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=-1").
            success(function (data, status) {
                $scope.CategoryList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });

        //START : Get State List        
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=STATE&MasterCode=-1").
            success(function (data, status) {
                $scope.Statelist = data;
                $scope.readonlyfun = true;
            }).
            error(function (data, status) {
                $scope.Statelist = [];
            });
        //END : Get State List
    }
    $scope.OnPageLoad();

});