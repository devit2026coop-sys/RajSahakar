var CommityRegistrtionModule = angular.module('LoanApplicationMod', ['angularUtils.directives.dirPagination']);

CommityRegistrtionModule.controller('LoanApplicationCnt', function ($scope, $http, $rootScope) {


    const urlParams = new URLSearchParams(window.location.search);
    $scope.PldbOtsEncId = urlParams.get('eId');
    $scope.mode = urlParams.get('mode');
    if ($scope.mode == 'v') {
        setTimeout(function () {
            $('input,select').each(function (k, v) {
                $(v).prop("disabled", "disabled");
                $('#menuSearch').removeAttr("disabled");
            });
            $('button').not('#backbtn').hide();
        }, 300);
    }
    else {
        setTimeout(function () {
            $('input,select').each(function (k, v) {
                $(v).prop("disabled", "disabled");
                //$('#menuSearch,.janaadharno,.mobileno,.applicant-name,.father-name,.caste-category,.gender,.nominee-name,.nominee-mobile,.nominee-janaadhar,.pending-level,.ml-pending-level,.mutation-no,.ml-mutation-no,.mutation-date,.ml-mutation-date').removeAttr("disabled");
                $('#menuSearch,.janaadharno,.mobileno,.applicant-name,.father-name,.caste-category,.gender,.nominee-name,.nominee-mobile,.nominee-janaadhar').removeAttr("disabled");
            });
            //$('button').not('#backbtn').hide();
        }, 300);
    }

    $scope.IsEditable = true;

    $scope.LoanApplicationLetterTitle = "";
    $scope.ShowApplicanDetails = false;
    $scope.IsShowPartialSave = false;
    $scope.IsSubmiApplication = false;
    $scope.LoanApplicationList = [];
    $scope.PendingLevelList = [];
    $scope.MLPendingLevelList = [];
    // $scope.LoanApplication.LOAN_ID = '0';

    $scope.dvApplicationForm = false;
    $scope.divLoanApplicationList = true;
    $scope.ShowApplicationForm = function () {
        $scope.dvApplicationForm = true;
        $scope.divLoanApplicationList = false;
        $scope.ShowAddNewApplication = false;
    }
    $scope.ShowAddNewApplication = true;
    $scope.divUserRegistration = true;

    $scope.IsShowPLDB = true;
    $scope.IsShowBranch = true;


    $scope.PLDBBranchList = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + $scope.PLDB.PLDB_Id).
            success(function (data, status) {
                $scope.PLDBBranches = data;

                $scope.PLDB.Branch_ID = branch_ID.toString();
                $scope.PLDB.Branch_ID = '0';
                //$scope.PropertyDetailsList[0].TehsilList = data;
                //$scope.PropertyDetailsList[0].Tehsil = '0';;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetPendingLevelList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=OTSPENDINGLEVEL").
            success(function (data, status) {
                $scope.PendingLevelList = data;
            }).
            error(function (data, status) {
                $scope.PendingLevelList = [];
            });
    };

    $scope.GetMLPendingLevelList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=OTSPENDINGLEVEL&MasterCode=1").
            success(function (data, status) {
                $scope.MLPendingLevelList = data;
            }).
            error(function (data, status) {
                $scope.MLPendingLevelList = [];
            });
    };


    $scope.UserLevel = $("#hdnUserlevel").val();
    // START : Code for Language label setting
    $scope.getTranslation = function () {
        //_language = 'hi-in';
        $scope._language = _language;

        if (_language === 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                    $scope.LoanApplicationLetterTitle = $scope.translation.AgriculturalLoanApplicationLetter;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                    $scope.LoanApplicationLetterTitle = $scope.translation.AgriculturalLoanApplicationLetter;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }


    };

    $scope.PLDBList = [];
    $scope.CategoryList = [];


    $scope.LoadMaster = function () {
        return $http.get(SiteUrl + "/pldb/LoadMaster").
            success(function (data, status) {
                $scope.PLDBList = JSON.parse(data.DataList).PLDB;
                $scope.CategoryList = JSON.parse(data.DataList).CATEGORY;

                if ($scope.CategoryList && $scope.CategoryList.length > 0 && $scope.PLDB_OTSSchemeData && $scope.PLDB_OTSSchemeData.CategoryName != "" && ($scope.PLDB_OTSSchemeData.CategoryId == undefined || $scope.PLDB_OTSSchemeData.CategoryId == 0 || $scope.PLDB_OTSSchemeData.CategoryId == "0")) {

                    $scope.PLDB.CategoryId = $scope.CategoryList.filter(function (x) { return x.CODENAME.toString() == $scope.PLDB.CategoryName })[0].CODEID;
                    $scope.PLDB.CategoryName = $scope.CategoryList.filter(function (x) { return x.CODEID.toString() == $scope.PLDB.CategoryId })[0].CODENAME;

                }
                else {
                    $scope.PLDB.CategoryId = $scope.PLDB_OTSSchemeData.CategoryId.toString();
                }
            }).
            error(function (data, status) {
                $scope.PLDB = [];
            });
    }



    //// START : On Page Load
    $scope.getTranslation();

    $scope.GetNameFromList = function (list, id, type) {
        try {
            if (type == 'PLDB') {
                var data = list.filter(function (x) { return x.PldbId === id.toString() })[0];
                return _language == 'en-us' ? data.PLDBName : data.PLDBName;
            }
            else if (type == 'Branch') {
                var data = list.filter(function (x) { return x.BLOCK_ID === id.toString() })[0];
                return _language == 'en-us' ? data.BLOCK_ENG : data.BLOCK_MANGAL;
            }


            else if (type == 'Category') {
                var data = list.filter(function (x) { return x.CODEID === id.toString() })[0];
                return data.CODENAME;
            }
            else if (type == 'District') {
                var data = list.filter(function (x) { return x.DISTRICT_ID === id.toString() })[0];
                return data.DISTRICT_ENG;
            }

            else if (type == 'Block') {
                var data = list.filter(function (x) { return x.BLOCK_ID === id.toString() })[0];
                return data.BLOCK_ENG;
            }
            else if (type == 'GramPanchayat') {
                var data = list.filter(function (x) { return x.GP_ID === id.toString() })[0];
                return data.GP_ENG;
            }
            else if (type == 'Village') {
                var data = list.filter(function (x) { return x.VILLAGE_ID === id.toString() })[0];
                return data.VILLAGE_ENG;
            }

        } catch (ex) {
            return "";
        }
    }

    /*-------Sector, Scheme, Purpose Master code block-------------------------*/

    $scope.LoanSectorAllList = [];
    $scope.ChangeLoanType = function (type, index) {

        $scope.PLDB.LoanDetails[index].LoanSectorList = [];
        $scope.PLDB.LoanDetails[index].LoanSectorList = $scope.LoanSectorAllList.filter(function (x) { return x.Farming_NonFarming == type });
    }


    $scope.LoanSectorList = [];
    $scope.LoadSector = function (scheme_id, index) {

        return $http.post(SiteUrl + "/pldb/GetSectorMaster", { "WhereCase": "  and se.is_active=1 " }).
            success(function (data, status) {
                $scope.LoanSectorList = [];
                $scope.LoanSectorList = data;

                if ($scope.LoanSectorList && $scope.LoanSectorList.length > 0 && $scope.PLDB_OTSSchemeData && $scope.PLDB_OTSSchemeData.Sector != "" && $scope.IsEditable
                    && ($scope.PLDB_OTSSchemeData.SectorId == undefined || $scope.PLDB_OTSSchemeData.SectorId == 0 || $scope.PLDB_OTSSchemeData.SectorId == "0")
                ) {
                    var matchedVillage = $scope.LoanSectorList.find(function (x) {
                        return x.sector_name_en.toLowerCase() == $scope.PLDB_OTSSchemeData.Sector.toLowerCase();
                    });

                    if (matchedVillage) {
                        $scope.PLDB.SectorId = matchedVillage.SectorId;
                        $scope.PLDB.Sector = matchedVillage.Sector;
                    }
                }
                else {
                    $scope.PLDB.SectorId = $scope.PLDB_OTSSchemeData.SectorId.toString();
                }

            }).
            error(function (data, status) {
                $scope.LoanSectorList = [];
                $(".blockUI").fadeOut("slow");
            });
    };



    //start: Get District List
    $scope.GetDistrictList = function (id) {

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT").
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($scope.DistrictList && $scope.DistrictList.length > 0 && $scope.PLDB_OTSSchemeData && $scope.PLDB_OTSSchemeData.DistrictName != "" && ($scope.PLDB_OTSSchemeData.DistrictId == undefined || $scope.PLDB_OTSSchemeData.DistrictId == 0 || $scope.PLDB_OTSSchemeData.DistrictId == "0")) {
                    // Find matching DistrictId
                    var matchedDistrict = $scope.DistrictList.find(function (x) {
                        return x.DISTRICT_ENG.toLowerCase() == $scope.PLDB_OTSSchemeData.DistrictName.toLowerCase();
                    });

                    if (matchedDistrict) {
                        $scope.PLDB.DistrictId = matchedDistrict.DISTRICT_ID;
                        $scope.PLDB.DistrictName = matchedDistrict.DISTRICT_ENG;

                        //$scope.PLDB.DistrictId = $scope.DistrictList.filter(function (x) { return x.DISTRICT_ENG.toString() === $scope.PLDB_OTSSchemeData.DistrictName })[0].DISTRICT_ID;
                        //$scope.PLDB.DistrictName = $scope.DistrictList.filter(function (x) { return x.DISTRICT_ID.toString() === $scope.PLDB.DistrictId })[0].DISTRICT_ID;

                        $scope.PLDB.IsRural = 'false';
                        $scope.RuralUrbanBlock(false);

                    }
                }
                else {
                    $scope.PLDB.DistrictId = $scope.PLDB_OTSSchemeData.DistrictId.toString();
                }
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });


    };

    //End: Get District List

    //start: Get RuralUrban List
    $scope.RuralUrbanBlock = function (V) {
        var SelectedVal = $scope.PLDB.IsRural == 'false' ? false : true;
        if (V != 1) {
            $scope.PLDB.CityTypes = "0";
            $scope.PLDB.CITY_ID = "0";
            $scope.PLDB.WARD_ID = "0";
            $scope.PLDB.BlockId = "0";
            $scope.PLDB.GpId = "0";
            $scope.PLDB.VId = "0";
        }
        if (SelectedVal == true || SelectedVal == true) {
            $scope.PLDB.BlockId = "0";
            $scope.PLDB.GpId = "0";
            $scope.PLDB.VId = "0";
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;
            $scope.GetCityListByCityTypes(1);
        }
        else if (SelectedVal == false || SelectedVal == false) {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
            $scope.BlockListByOffAdd(1)
        }
        else {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
        }
    };


    //End: Get RuralUrban List

    // START : Set Rural/ Urban div show/hide
    $scope.GetCityListByCityTypes = function (id) {
        if ($scope.PLDB.DistrictId != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.PLDB.DistrictId).
                success(function (data, status) {
                    $scope.CityListdata = data;
                }).
                error(function (data, status) {
                    $scope.CityListdata = [];
                });
        }

    };
    //start BlockListByOffAddress form
    $scope.BlockListByOffAdd = function (id) {
        if ($scope.PLDB.DistrictId != '0' && id == 1) {
            $scope.Blocksdata = [];
            $scope.GramPanchayats = [];
            $scope.Villages = [];
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.PLDB.DistrictId).
                success(function (data, status) {
                    $scope.Blocksdata = data;

                    if ($scope.Blocksdata && $scope.Blocksdata.length > 0 && $scope.PLDB_OTSSchemeData && $scope.PLDB_OTSSchemeData.BlockName != "" && $scope.IsEditable
                        && ($scope.PLDB_OTSSchemeData.BlockId == undefined || $scope.PLDB_OTSSchemeData.BlockId == 0 || $scope.PLDB_OTSSchemeData.BlockId == "0")
                    ) {
                        var matchedBlock = $scope.Blocksdata.find(function (x) {
                            return x.BLOCK_ENG.toLowerCase() == $scope.PLDB_OTSSchemeData.BlockName.toLowerCase();
                        });

                        if (matchedBlock) {
                            $scope.PLDB.BlockId = matchedBlock.BLOCK_ID;
                            $scope.PLDB.BlockName = matchedBlock.DISTRICT_ENG;

                            $scope.GramPanchayatList(1);
                        }
                    }
                    else {
                        $scope.PLDB.BlockId = $scope.PLDB_OTSSchemeData.BlockId.toString();
                    }
                }).
                error(function (data, status) {
                    $scope.Blocksdata = [];
                });
        }

    };
    //END

    // START : Get List of GramPanchayat
    $scope.GramPanchayatList = function (id) {

        if ($scope.PLDB.BlockId != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.PLDB.BlockId).
                success(function (data, status) {
                    $scope.GramPanchayats = data;

                    if ($scope.GramPanchayats && $scope.GramPanchayats.length > 0 && $scope.PLDB_OTSSchemeData && $scope.PLDB_OTSSchemeData.GpName != "" && $scope.IsEditable
                        && ($scope.PLDB_OTSSchemeData.GpId == undefined || $scope.PLDB_OTSSchemeData.GpId == 0 || $scope.PLDB_OTSSchemeData.GpId == "0")) {
                        var matchedGP = $scope.GramPanchayats.find(function (x) {
                            return x.GP_ENG.toLowerCase() == $scope.PLDB_OTSSchemeData.GpName.toLowerCase();
                        });

                        if (matchedGP) {
                            $scope.PLDB.GpId = matchedGP.GP_ID;
                            $scope.PLDB.GpName = matchedGP.GP_ENG;
                            $scope.VillageList(1);
                        }
                    }
                    else {
                        $scope.PLDB.GpId = $scope.PLDB_OTSSchemeData.GpId.toString();
                    }
                }).
                error(function (data, status) {
                    $scope.GramPanchayats = [];

                });
        }

    };
    //End : Get List of GramPanchayat

    // START : Get Ward List
    $scope.GetWardList = function (id) {
        if ($scope.PLDB.CITY_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.PLDB.CITY_ID).
                success(function (data, status) {
                    $scope.Ward = data;
                }).
                error(function (data, status) {
                    $scope.Ward = [];
                });
        }

    };

    //End
    //Start: Get Village List
    $scope.VillageList = function (id) {
        if ($scope.PLDB.GpId != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.PLDB.GpId).
                success(function (data, status) {
                    $scope.Villages = data;

                    if ($scope.Villages && $scope.Villages.length > 0 && $scope.PLDB_OTSSchemeData && $scope.PLDB_OTSSchemeData.VName != "" && $scope.IsEditable
                        && ($scope.PLDB_OTSSchemeData.VId == undefined || $scope.PLDB_OTSSchemeData.VId == 0 || $scope.PLDB_OTSSchemeData.VId == "0")
                    ) {
                        var matchedVillage = $scope.Villages.find(function (x) {
                            return x.VILLAGE_ENG.toLowerCase() == $scope.PLDB_OTSSchemeData.VName.toLowerCase();
                        });

                        if (matchedVillage) {
                            $scope.PLDB.VId = matchedVillage.VILLAGE_ID;
                            $scope.PLDB.VName = matchedVillage.VILLAGE_ENG;
                        }
                    }
                    else {
                        $scope.PLDB.VId = $scope.PLDB_OTSSchemeData.VId.toString();
                    }


                }).
                error(function (data, status) {
                    $scope.Villages = [];
                });
        }

    };
    //End

    $scope.Save_PLDBForm = function (StatusId) {
        $(".se-pre-con").fadeIn("slow");

        $scope.PLDB.CategoryName = $scope.GetNameFromList($scope.CategoryList, $scope.PLDB.CategoryId, 'Category');
        $scope.PLDB.VName = $scope.GetNameFromList($scope.Villages, $scope.PLDB.VId, 'Village');
        $scope.PLDB.DistrictName = $scope.GetNameFromList($scope.DistrictList, $scope.PLDB.DistrictId, 'District');
        $scope.PLDB.BlockName = $scope.GetNameFromList($scope.Blocksdata, $scope.PLDB.BlockId, 'Block');
        $scope.PLDB.GpName = $scope.GetNameFromList($scope.GramPanchayats, $scope.PLDB.GpId, 'GramPanchayat');

        if ($scope.PLDB.PendingLevelId == 6) {
            var mutationDateObj = new Date(($scope.PLDB.MutationDate ?? new Date()));
            $scope.PLDB.MutationDate = mutationDateObj.getFullYear() +'/'+(mutationDateObj.getMonth() + 1) + '/' + mutationDateObj.getDate();
        }
        else
            $scope.PLDB.MutationDate = null;
        if ($scope.PLDB.LandMortgagedPendingLevelId == 6) {
            var mlMutationDateObj = new Date(($scope.PLDB.LandMortgagedMutationDate ?? new Date()));
            $scope.PLDB.LandMortgagedMutationDate = mlMutationDateObj.getFullYear() + '/' + (mlMutationDateObj.getMonth() + 1) + '/' + mlMutationDateObj.getDate();
        }
        else
            $scope.PLDB.LandMortgagedMutationDate = null;

        $http.post(SiteUrl + "/PLDB/Save_PldbApplicationForm", $scope.PLDB).
            success(function (data, status) {

                $(".se-pre-con").fadeOut("slow");
                //if (data.Status == true)
                if (data > 0) {
                    alert($scope._language == 'en-us' ? 'Data Save Sucessfully' : ' डेटा सफलतापूर्वक सहेजा गया।');
                    window.location.reload();
                }
                else {
                    // $scope.Reset_GCCForm();
                    ShowModelAlert($scope._language == 'en-us' ? 'OTS 2025-26 Scheme :: ' : 'OTS 2025-26 Scheme ::', 'An Error Occurred Please Try Again Later');
                }

            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                // $$scope.Reset_GCCForm();
            });

    }


    $scope.GetFormEditData = function (PldbOtsId) {
        $http.post(SiteUrl + "/PLDB/GetOTSListApplicationFormViewData/?PldbOtsSchemeId=" + PldbOtsId).
            success(function (data, status) {
                $scope.PLDB_OTSSchemeData = JSON.parse(data.DataList)[0];
                $scope.PLDB = $scope.PLDB_OTSSchemeData;
                console.log($scope.PLDB);
                $scope.LoadMaster();
                $scope.GetDistrictList(0);
                $scope.GetPendingLevelList();
                $scope.GetMLPendingLevelList();
                $scope.PLDB.PendingLevelId = $scope.PLDB.PendingLevelId + "";
                $scope.PLDB.LandMortgagedPendingLevelId = $scope.PLDB.LandMortgagedPendingLevelId + "";
                if ($scope.PLDB.MutationDate != null)
                    $scope.PLDB.MutationDate = new Date($scope.PLDB.MutationDate);
                if ($scope.PLDB.LandMortgagedMutationDate != null)
                $scope.PLDB.LandMortgagedMutationDate = new Date($scope.PLDB.LandMortgagedMutationDate);
                $scope.PLDB.PLDB_Id = $scope.PLDB_OTSSchemeData.PLDB_Id.toString();
                $scope.IsShowPLDB = false;
                $scope.PLDBBranchList($scope.PLDB_OTSSchemeData.PLDB_BranchId.toString()).then(function () {
                    if ($scope.PLDB_OTSSchemeData.PLDB_BranchId.toString() != "" && $scope.PLDB_OTSSchemeData.PLDB_BranchId.toString() != "0") {
                        $scope.PLDB.Branch_ID = $scope.PLDB_OTSSchemeData.PLDB_BranchId.toString();
                        $scope.IsShowBranch = false;
                        // $scope.PLDBName = $scope.PLDBList.filter(function (x) { return x.PldbId.toString() === $scope.PLDB.PLDB_Id })[0].PLDBName;
                        $scope.PLDBBranch = $scope._language == 'en-us' ? $scope.PLDBBranches.filter(function (x) { return x.BLOCK_ID.toString() == $scope.PLDB.Branch_ID })[0].BLOCK_ENG : $scope.PLDBBranches.filter(function (x) { return x.BLOCK_ID.toString() === $scope.PLDB.Branch_ID })[0].BLOCK_MANGAL;
                    }
                });
                $scope.LoadSector();
            }).
            error(function (data, status) {
                $scope.PLDB_OTSSchemeData = [];
            });
    }


    $scope.BackToPage = function () {
        window.location = '../PLDB/OTSApplicationList';
    }

    if ($scope.PldbOtsEncId != '') { $scope.GetFormEditData($scope.PldbOtsEncId); }

});
