var CommityRegistrtionModule = angular.module('PLDBLoanApplicationMod', ['angularUtils.directives.dirPagination']);

CommityRegistrtionModule.controller('PLDBLoanApplicationCnt', function ($scope, $http, $rootScope) {



    $scope.onload = function () {
        const urlParams = new URLSearchParams(window.location.search);
        $scope.PldbOtsEncId = urlParams.get('eId');

        $scope.mode = urlParams.get('mode');

        $scope.isViewMode = ($scope.mode === 'v');
        $scope.isEditMode = ($scope.mode === 'e');
        $scope.isNewMode = (!$scope.mode || $scope.mode === 'n');

        if ($scope.PldbOtsEncId && ($scope.isViewMode || $scope.isEditMode)) {
            $scope.GetFormEditData($scope.PldbOtsEncId);
        }

        if ($scope.isViewMode) {
            setTimeout(function () {
                $('input, select, textarea').prop('disabled', true);
                $('input[type=submit], button[type=submit]').hide();
            }, 600);
        }

        // Initialize default values
        $scope.IsEditable = !$scope.isViewMode;

        $scope.IsFinalSubmit = false;

        $scope.IsEditable = true;

        $scope.LoanApplicationLetterTitle = "";
        $scope.ShowApplicanDetails = false;
        $scope.IsShowPartialSave = false;
        $scope.IsSubmiApplication = false;
        $scope.LoanApplicationList = [];
        $scope.PendingLevelList = [];
        $scope.MLPendingLevelList = [];

        $scope.GetPLDBSectorNewODList = [];
        $scope.FinancialYearList();
        $scope.dvApplicationForm = false;
        $scope.divLoanApplicationList = true;
        $scope.ShowApplicationForm = function () {
            $scope.dvApplicationForm = true;
            $scope.divLoanApplicationList = false;
            $scope.ShowAddNewApplication = false;
        }
        $scope.ShowAddNewApplication = true;
        $scope.divUserRegistration = true;
        $scope.IsPLDBFreezed = false;

        $scope.IsShowPLDB = true;
        $scope.IsShowBranch = true;

        $scope.maxCertDate = new Date();
        $scope.maxCertDate.setDate($scope.maxCertDate.getDate() - 1);
        $scope.maxCertDate = $scope.maxCertDate.toISOString().split("T")[0];

        $scope.LoadSector();
        $scope.PLDB = {};

        var levelid = $("#hdnLevelId").val();
        var districtid = $("#hdnDistrictId").val();
        var pldbid = $("#hdnPLDBId").val();
        var branchid = $("#hdnBranchId").val();

        $scope.LoginContext = {
            levelId: $("#hdnLevelId").val(),
            districtId: $("#hdnDistrictId").val(),
            pldbId: $("#hdnPLDBId").val(),
            branchId: $("#hdnBranchId").val()
        };

        $scope.IsDistrictFreezed = false;
        $scope.IsPLDBFreezed = false;
        $scope.IsBranchFreezed = false;

        if ($scope.LoginContext.levelId == "1") {
            $scope.PLDB.PldbId = "0";
        }

        //  BRANCH LOGIN (branchId present)
        if ($scope.LoginContext.branchId && $scope.LoginContext.branchId !== "0") {
            $scope.IsDistrictFreezed = true;
            $scope.IsPLDBFreezed = true;
            $scope.IsBranchFreezed = true;
            $scope.PLDB.PldbId = $scope.LoginContext.pldbId;
            $scope.PLDB.BranchId = $scope.LoginContext.branchId;
            $scope.PLDBBranchList($scope.LoginContext.branchId);
        }

        //PLDB LOGIN (no branch, but PLDB present)
        else if ($scope.LoginContext.pldbId && $scope.LoginContext.pldbId !== "0") {
            $scope.IsPLDBFreezed = true;
            $scope.IsBranchFreezed = false;
            $scope.PLDB.PldbId = $scope.LoginContext.pldbId;
            $scope.PLDBBranchList();
        }

        $scope.PLDB.TehsilId = "";
        $scope.Tehsil = [];

    }

    $scope.PLDBBranchList = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + $scope.PLDB.PldbId).
            success(function (data, status) {

                $scope.PLDBBranches = data;
                if (branch_ID) {
                    $scope.PLDB.BranchId = branch_ID.toString();
                }
                else if (!$scope.IsBranchFreezed) {
                    $scope.PLDB.BranchId = '0';
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    //jaya
    $scope.GetPLDBSectorNewODList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBSECTORODNEW").
            success(function (data, status) {
                $scope.GetPLDBSectorNewODList = data;
            }).
            error(function (data, status) {
                $scope.GetPLDBSectorNewODList = [];
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

    $scope.getTranslation = function () {

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
                var levelId = $("#hdnLevelId").val();
                var pldbid = $("#hdnPLDBId").val();
                var branchId = $("#hdnBranchId").val();

                var isBranchLogin = !branchId;

                //PLDB LOGIN CASE
                if (levelId == "3" && pldbid && pldbid != "0") {

                    var matchedPLDB = $scope.PLDBList.find(function (x) {
                        return x.PldbId == pldbid;
                    });

                    if (matchedPLDB) {
                        $scope.PLDB.PldbId = matchedPLDB.PldbId;
                        $scope.PLDB.PldbName = matchedPLDB.PLDBName;
                        $scope.IsPLDBFreezed = true;
                        $scope.PLDBBranchList();
                    }
                }

                // BRANCH LOGIN
                if (isBranchLogin) {

                    $scope.IsDistrictFreezed = true;
                    $scope.IsPLDBFreezed = true;
                    $scope.IsBranchFreezed = true;
                    var matchedPLDB = $scope.PLDBList.find(x => x.PldbId == pldbid);
                    if (matchedPLDB) {
                        $scope.PLDB.PldbId = matchedPLDB.PldbId;
                        $scope.PLDB.PldbName = matchedPLDB.PLDBName;
                    }
                    $scope.PLDBBranchList(branchId);

                    $scope.PLDB.IsRural = 'true';
                    $scope.RuralDiv = true;
                    $scope.UrbanDiv = false;
                    $scope.BlockListByOffAdd(1);
                }


                $scope.CategoryList = JSON.parse(data.DataList).CATEGORY;

                if ($scope.CategoryList && $scope.CategoryList.length > 0 && $scope.PLDB && $scope.PLDB.CategoryName != "" && ($scope.PLDB.CategoryId == undefined || $scope.PLDB.CategoryId == 0 || $scope.PLDB.CategoryId == "0")) {

                    //$scope.PLDB.CategoryId = $scope.CategoryList.filter(function (x) { return x.CODENAME.toString() == $scope.PLDB.CategoryName })[0].CODEID;
                    //$scope.PLDB.CategoryName = $scope.CategoryList.filter(function (x) { return x.CODEID.toString() == $scope.PLDB.CategoryId })[0].CODENAME;

                }
                else {
                    //$scope.PLDB.CategoryId = $scope.PLDB_OTSSchemeData ? $scope.PLDB_OTSSchemeData.CategoryId.toString() : '0';
                }
            }).
            error(function (data, status) {
                $scope.PLDB = [];
            });
    }



    $scope.LoadMaster();

    //end

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
                if ($scope.LoanSectorList && $scope.LoanSectorList.length > 0 && $scope.PLDB && $scope.PLDB.Sector != "" && $scope.IsEditable
                    && ($scope.PLDB.SectorId == undefined || $scope.PLDB.SectorId == 0 || $scope.PLDB.SectorId == "0")
                ) {
                    var matchedVillage = $scope.LoanSectorList.find(function (x) {
                        return x.sector_name_en.toLowerCase() == $scope.PLDB.Sector.toLowerCase();
                    });

                    if (matchedVillage) {
                        $scope.PLDB.SectorId = matchedVillage.SectorId;
                        $scope.PLDB.Sector = matchedVillage.Sector;
                    }
                }
                else {
                    $scope.PLDB.SectorId = $scope.PLDB ? $scope.PLDB.SectorId.toString() : "0";
                    $scope.PLDB.SectorId2 = $scope.PLDB ? $scope.PLDB.SectorId2.toString() : "0";
                    $scope.PLDB.SectorId3 = $scope.PLDB ? $scope.PLDB.SectorId3.toString() : "0";
                    $scope.PLDB.SectorId4 = $scope.PLDB ? $scope.PLDB.SectorId4.toString() : "0";
                    $scope.PLDB.SectorId5 = $scope.PLDB ? $scope.PLDB.SectorId5.toString() : "0";
                    $scope.PLDB.SectorId6 = $scope.PLDB ? $scope.PLDB.SectorId6.toString() : "0";
                    $scope.PLDB.SectorId7 = $scope.PLDB ? $scope.PLDB.SectorId7.toString() : "0";
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
                var loginDistrictId = $("#hdnDistrictId").val();
                if (loginDistrictId && loginDistrictId !== "0") {

                    var dist = $scope.DistrictList.find(x =>
                        x.DISTRICT_ID == loginDistrictId
                    );

                    if (dist) {
                        $scope.PLDB.DistrictId = dist.DISTRICT_ID;
                        $scope.PLDB.DistrictName = dist.DISTRICT_ENG;
                        //$scope.IsDistrictFreezed = true;

                        $scope.PLDB.IsRural = 'true';
                        $scope.UrbanDiv = false;
                        $scope.RuralDiv = true;

                        $scope.TehsilList();
                        $scope.getAssembly();
                        $scope.BlockListByOffAdd(1);
                    }
                }
                else {
                    $scope.IsDistrictFreezed = false; // ADMIN CASE
                }

                if ($scope.DistrictList && $scope.DistrictList.length > 0 && $scope.PLDB_OTSSchemeData && $scope.PLDB_OTSSchemeData.DistrictName != "" && ($scope.PLDB_OTSSchemeData.DistrictId == undefined || $scope.PLDB_OTSSchemeData.DistrictId == 0 || $scope.PLDB_OTSSchemeData.DistrictId == "0")) {
                    // Find matching DistrictId
                    var matchedDistrict = $scope.DistrictList.find(function (x) {
                        return x.DISTRICT_ENG.toLowerCase() == $scope.PLDB_OTSSchemeData.DistrictName.toLowerCase();
                    });

                    if (matchedDistrict) {
                        $scope.PLDB.DistrictId = matchedDistrict.DISTRICT_ID;
                        $scope.PLDB.DistrictName = matchedDistrict.DISTRICT_ENG;

                        $scope.PLDB.IsRural = 'false';
                        $scope.RuralUrbanBlock(false);
                    }
                }
                else {
                    //$scope.PLDB.DistrictId = $scope.PLDB_OTSSchemeData ? $scope.PLDB_OTSSchemeData.DistrictId.toString() : '0';
                }
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });


    };

    $scope.GetDistrictList();

    $scope.onAreaTypeChange = function () {

        if ($scope.PLDB.AreaType == 'Urban') {
            $scope.PLDB.IsRural = 0;
            $scope.PLDB.BlockId = '0';
            $scope.PLDB.GP_ID = '0';
            $scope.PLDB.VId = '0';
            $scope.GetCityListByCityTypes(1);

        }
        else {
            $scope.PLDB.IsRural = 1;
            $scope.PLDB.CityId = '0';
            $scope.PLDB.WardId = '0';
            $scope.Ward = [];
            $scope.BlockListByOffAdd(1);
        }
    };


    $scope.GetCityList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITY&MasterCode=" + $scope.PLDB.DistrictId).
            success(function (data, status) {
                $scope.CityList = data;
            }).
            error(function (data, status) {
                $scope.CityList = [];
            });
    };


    $scope.GetWardList = function (id) {
        if ($scope.PLDB.CityId != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.PLDB.CityId).
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


    $scope.GetWardListbySociety = function () {
        $scope.Ward = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ward&MasterCode=" + $scope.PLDB.CityId).
            success(function (data, status) {
                $scope.Ward = data;
            }).
            error(function (data, status) {
                $scope.Ward = [];
            });
    };


    $scope.TehsilList = function () {
        if (!$scope.PLDB.DistrictId) {
            $scope.Tehsil = [];
            return;
        }
        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=tehsil&MasterCode=" + $scope.PLDB.DistrictId
        ).success(function (data) {
            $scope.Tehsil = data;   // dropdown data
            $(".se-pre-con").fadeOut("slow");

        }).error(function () {
            $scope.Tehsil = [];
            $(".se-pre-con").fadeOut("slow");
        });
    };


    $scope.getAssembly = function () {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ASSEMBLY&MasterCode=" + $scope.PLDB.DistrictId).
            then(function (resp) {
                $scope.AssemblyList = resp.data;
                $(".se-pre-con").fadeOut("slow");
                return resp.data;
            }).
            catch(function () {
                $scope.AssemblyList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    }


    //$scope.TehsilList();

    //// Reset dependent location fields
    //$scope._resetLocationDependent = function () {
    //    $scope.PLDB.BlockId = $scope.PLDB.BlockId || '0';
    //    $scope.PLDB.GpId = $scope.PLDB.GpId || '0';
    //    $scope.PLDB.VId = $scope.PLDB.VId || '0';

    //    $scope.Blocksdata = $scope.Blocksdata || [];
    //    $scope.GramPanchayats = $scope.GramPanchayats || [];
    //    $scope.Villages = $scope.Villages || [];
    //};

    //// Called when user changes district in UI
    //$scope.OnDistrictChange = function () {
    //    try {
    //        // ensure model exists
    //        $scope.PLDB = $scope.PLDB || {};
    //        // if user selected "0" or empty => hide rural and reset
    //        if (!$scope.PLDB.DistrictId || $scope.PLDB.DistrictId === '0') {
    //            //$scope.PLDB.IsRural = 'false';
    //            //$scope.UrbanDiv = true;
    //            //$scope.RuralDiv = false;
    //            $scope._resetLocationDependent();
    //            return;
    //        }

    //        // District selected -> treat as rural by default (as your app design)
    //        //$scope.PLDB.IsRural = 'true';
    //        //$scope.UrbanDiv = false;
    //        //$scope.RuralDiv = true;

    //        // Reset dependent selects before loading new data
    //        $scope.PLDB.BlockId = '0';
    //        $scope.PLDB.GpId = '0';
    //        $scope.PLDB.VId = '0';
    //        $scope.Blocksdata = [];
    //        $scope.GramPanchayats = [];
    //        $scope.Villages = [];

    //        // Load blocks for selected district
    //        // BlockListByOffAdd uses $scope.PLDB.DistrictId internally in your code
    //        // It already does an $http.get and populates $scope.Blocksdata.
    //        // Use .then style to be safe (your function uses success(), but still return promise).
    //        var promise = $scope.BlockListByOffAdd(1);
    //        if (promise && typeof promise.then === 'function') {
    //            promise.then(function () {
    //                // optional: auto-select block if PLDB has BlockName from DB (only for "load" case)
    //                if ($scope.PLDB && $scope.PLDB.BlockId && $scope.PLDB.BlockId !== '0') {
    //                    $scope.PLDB.BlockId = $scope.PLDB.BlockId.toString();
    //                    // load gram panchayats now
    //                    $scope.GramPanchayatList(1);
    //                }
    //            });
    //        } else {
    //            // fallback: call GramPanchayatList after a small delay (if BlockListByOffAdd uses success callback)
    //            setTimeout(function () {
    //                $scope.$applyAsync(function () {
    //                    if ($scope.PLDB.BlockId && $scope.PLDB.BlockId !== '0') $scope.GramPanchayatList(1);
    //                });
    //            }, 300);
    //        }

    //        // Also load city list for urban inputs if needed
    //        $scope.GetCityListByCityTypes(1);

    //    } catch (err) {
    //        console.error("OnDistrictChange error", err);
    //    }
    //};


    //End: Get District List

    $scope.OnDistrictChange = function () {
        if ($scope.PLDB.DistrictId && $scope.PLDB.DistrictId !== "0") {
            $scope.PLDB.IsRural = 'true';
            //$scope.UrbanDiv = false;
            //$scope.RuralDiv = true;       
            $scope.TehsilList();
            $scope.getAssembly();
            $scope.BlockListByOffAdd(1);
            $scope.GetCityListByCityTypes(1);
        } else {
            $scope.RuralDiv = false;
            //$scope.GetCityListByCityTypes();
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
                        && ($scope.PLDB.BlockId == undefined || $scope.PLDB.BlockId == 0 || $scope.PLDB.BlockId == "0")
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
                        $scope.PLDB.BlockId = $scope.PLDB ? $scope.PLDB.BlockId.toString() : '0';
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
                        && ($scope.PLDB.GP_ID == undefined || $scope.PLDB.GP_ID == 0 || $scope.PLDB.GP_ID == "0")) {
                        var matchedGP = $scope.GramPanchayats.find(function (x) {
                            return x.GP_ENG.toLowerCase() == $scope.PLDB.GpName.toLowerCase();
                        });

                        if (matchedGP) {
                            $scope.PLDB.GP_ID = matchedGP.GP_ID;
                            $scope.PLDB.GpName = matchedGP.GP_ENG;
                            $scope.VillageList(1);
                        }
                    }
                    else {
                        $scope.PLDB.GP_ID = $scope.PLDB ? $scope.PLDB.GP_ID.toString() : '0';
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

        if ($scope.PLDB.CityId != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.PLDB.CityId).
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
        if ($scope.PLDB.GP_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.PLDB.GP_ID).
                success(function (data, status) {
                    $scope.Villages = data;

                    if ($scope.Villages && $scope.Villages.length > 0 && $scope.PLDB_OTSSchemeData && $scope.PLDB_OTSSchemeData.VName != "" && $scope.IsEditable
                        && ($scope.PLDB.VId == undefined || $scope.PLDB.VId == 0 || $scope.PLDB.VId == "0")
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
                        $scope.PLDB.VId = $scope.PLDB ? $scope.PLDB.VId.toString() : '0';
                    }
                }).
                error(function (data, status) {
                    $scope.Villages = [];
                });
        }

    };
    //End

    $scope.DateFormat = function (date) {

        if (date) {
            var FormatDate = new Date(date ?? new Date());
            return (FormatDate.getMonth() + 1) + '/' + FormatDate.getDate() + '/' + FormatDate.getFullYear();
        }
        else {
            return null;
        }
    }

    $scope.toOneZero = function (val) {
        return (val === true || val === "1" || val === 1) ? 1 : 0;
    }

    //Save PLDB Loan Application Form

    $scope.Save_PLDBForm = function (StatusId) {
        debugger;

        if (!$scope.PLDB.ApplicantName || !$scope.PLDB.FatherHusbandName || !$scope.PLDB.Gender) {
            alert("Please fill all required fields before submitting.");
            return;
        }

        if (!$scope.PLDB.FarmerType) {
            alert("Please select Farmer Type");
            return;
        }

        if ($scope.PLDB.CategoryName === null || $scope.PLDB.CategoryName === undefined || $scope.PLDB.CategoryName === 0 || $scope.PLDB.CategoryName === "0") {
            alert("Please Select Category.");
            return;
        }

        if (!$scope.PLDB.LoanAcceptDate) {
            alert("Please Fill the Loan Accept Date");
            return;
        }

        if (!$scope.PLDB.SubsidyLoanAmount) {
            alert("Please Fill the Subsidy Loan Amount");
            return;
        }

        if (!$scope.PLDB.LoanAcceptPay) {
            alert("Please Fill the Loan Accept Amount");
            return;
        }

        if (!$scope.PLDB.Interestrate) {
            alert("Please Fill the Interest Rate Amount");
            return;
        }


        if (!$scope.PLDB.AreaType) {
            alert("Please Select Urban / Rural");
            return;
        }


        if ($scope.PLDB.AreaType == 'Rural') {

            if ($scope.PLDB.BlockId === "0") {
                alert("Please Select Block");
                return;
            }

            //if ($scope.PLDB.GP_ID === "0") {
            //    alert("Please Select Gram Panchayat");
            //    return;
            //}

            //if ($scope.PLDB.VId === "0") {
            //    alert("Please Select Village");
            //    return;
            //}

            if ($scope.PLDB.TehsilId === "0") {
                alert("Please Select Tehsil")
                return;
            }
        }

        if ($scope.PLDB.AreaType == 'Urban') {

            if ($scope.PLDB.CityId === "0") {
                alert("Please Select City");
                return;
            }

            if ($scope.PLDB.WardId === "0") {
                alert("Please Select Ward");
                return;
            }
        }

        if (StatusId === true) {
            if (!$scope.PLDB.LoanDisbursement.LoanDocument) {
                alert("Please Upload the Document");
                return;
            }
        }

        if (!$scope.PLDB.MobileNo) {
            alert("Please Enter Mobile No");
            return;
        }

        if (!$scope.PLDB.LoanAccNo) {
            alert("Please enter loan account no.");
            return;
        }

        if ($scope.PLDB.SectorId === null || $scope.PLDB.SectorId === undefined || $scope.PLDB.SectorId === 0 || $scope.PLDB.SectorId === "0") {
            alert("Please Select Sector.");
            return;
        }

        if ($scope.PLDB.PurposeId === null || $scope.PLDB.PurposeId === undefined || $scope.PLDB.PurposeId === 0 || $scope.PLDB.PurposeId === "0") {
            alert("Please Select Purpose.");
            return;
        }

        if ($scope.PLDB.PldbId === "0") {
            alert("Please Select PLDB.");
            return;
        }

        if ($scope.PLDB.FinancialYearId === "0") {
            alert("Please Select Financial Year.");
            return;
        }

        if ($scope.PLDB.BranchId === "0") {
            alert("Please Select PLDB Branch");
            return;
        }

        if ($scope.PLDB.DistrictId === "0") {
            alert("Please Select District")
            return;
        }

        if ($scope.PLDB.AssemblyId === "0") {
            alert("Please Select Assembly")
            return;
        }

        // Confirmation for Final Submit
        if (StatusId === true) {
            var isConfirm = confirm("Are you sure you want to Final Submit this application?");
            if (!isConfirm) {
                return;
            }
        }

        $(".se-pre-con").fadeIn("slow");

        $scope.PLDB.GuarantorDetails = $scope.PLDB.GuarantorDetails || {};
        $scope.PLDB.LoanDisbursement = $scope.PLDB.LoanDisbursement || {};

        $scope.PLDB.VName = $scope.GetNameFromList($scope.Villages, $scope.PLDB.VId, 'Village');
        $scope.PLDB.DistrictName = $scope.GetNameFromList($scope.DistrictList, $scope.PLDB.DistrictId, 'District');
        $scope.PLDB.BlockName = $scope.GetNameFromList($scope.Blocksdata, $scope.PLDB.BlockId, 'Block');
        $scope.PLDB.GpName = $scope.GetNameFromList($scope.GramPanchayats, $scope.PLDB.GpId, 'GramPanchayat');

        $scope.PLDB.LoanApplicationDate = $scope.DateFormat($scope.PLDB.LoanApplicationDate);
        $scope.PLDB.LoanAcceptDate = $scope.DateFormat($scope.PLDB.LoanAcceptDate);
        $scope.PLDB.bankPartyEnrollementNumberDate = $scope.DateFormat($scope.PLDB.bankPartyEnrollementNumberDate);
        $scope.PLDB.AuthorizationLetterIssueDate = $scope.DateFormat($scope.PLDB.AuthorizationLetterIssueDate);

        $scope.PLDB.IsFinalSubmit = StatusId;

        $scope.PLDB.BillDate = $scope.DateFormat($scope.PLDB.BillDate);
        $scope.PLDB.Accept_NAccpetDate = $scope.DateFormat($scope.PLDB.Accept_NAccpetDate);

        $scope.PLDB.LoanDisbursement.FirstInstallmentDate = $scope.DateFormat($scope.PLDB.LoanDisbursement.FirstInstallmentDate);

        $scope.PLDB.LoanDisbursement.SecondInstallmentDate = $scope.DateFormat($scope.PLDB.LoanDisbursement.SecondInstallmentDate);

        $scope.PLDB.LoanDisbursement.ThirdInstallmentDate = $scope.DateFormat($scope.PLDB.LoanDisbursement.ThirdInstallmentDate);

        $scope.PLDB.LoanDisbursement.BillDate = $scope.DateFormat($scope.PLDB.LoanDisbursement.BillDate);

        $scope.PLDB.LoanDisbursement.CertificateFirstInstallmentDate = $scope.DateFormat($scope.PLDB.LoanDisbursement.CertificateFirstInstallmentDate);

        $scope.PLDB.LoanDisbursement.CertificateSecondInstallmentDate = $scope.DateFormat($scope.PLDB.LoanDisbursement.CertificateSecondInstallmentDate);

        $scope.PLDB.LoanDisbursement.CertificateThirdInstallmentDate = $scope.DateFormat($scope.PLDB.LoanDisbursement.CertificateThirdInstallmentDate);

        $scope.PLDB.LoanDisbursement.LoanDocument = $scope.PLDB.LoanDisbursement.LoanDocument;

        $scope.PLDB.IsLoanDetailsAgrimMICRCheckTaken = $scope.toOneZero($scope.PLDB.IsLoanDetailsAgrimMICRCheckTaken);

        $scope.PLDB.BAIsLandMapAttached = $scope.toOneZero($scope.PLDB.BAIsLandMapAttached);

        $scope.PLDB.IsOtherGramPanchayat = $scope.toOneZero($scope.PLDB.IsOtherGramPanchayat);

        $scope.PLDB.AIsJamabandiAttached = $scope.toOneZero($scope.PLDB.AIsJamabandiAttached);

        $scope.PLDB.AIsLandMapAttached = $scope.toOneZero($scope.PLDB.AIsLandMapAttached);

        $scope.PLDB.AIsOwnershipAffidavitAttached = $scope.toOneZero($scope.PLDB.AIsOwnershipAffidavitAttached);

        $scope.PLDB.AIsKhasraGirdawariCopyAttached = $scope.toOneZero($scope.PLDB.AIsKhasraGirdawariCopyAttached);

        $scope.PLDB.BApplicantregistrationAttact = $scope.toOneZero($scope.PLDB.BApplicantregistrationAttact);

        $scope.PLDB.IsEstimateQuotationAttached = $scope.toOneZero($scope.PLDB.IsEstimateQuotationAttached);

        $scope.PLDB.IsRegistrationCertificateReceived = $scope.toOneZero($scope.PLDB.IsRegistrationCertificateReceived);

        $scope.PLDB.GuarantorDetails.IsFirstAgrimMICRCheckTaken = $scope.toOneZero($scope.PLDB.GuarantorDetails.IsFirstAgrimMICRCheckTaken);

        $scope.PLDB.GuarantorDetails.IsSecondAgrimMICRCheckTaken = $scope.toOneZero($scope.PLDB.GuarantorDetails.IsSecondAgrimMICRCheckTaken);

        $scope.PLDB.LoanDisbursement.IsApprovalOnNotesheet = $scope.toOneZero($scope.PLDB.LoanDisbursement.IsApprovalOnNotesheet);

        /*     add By Bhavesh*/
        $scope.PLDB.LoanDisbursement.IsIssueofPassbook = $scope.toOneZero($scope.PLDB.LoanDisbursement.IsIssueofPassbook);


        $http.post(SiteUrl + "/PLDB/Save_PldbLoanApplicationDetails", $scope.PLDB).
            success(function (data, status) {
                if (data == 0) {

                    $scope.PLDB.PLDB_OTS_SchemeID = data;
                    console.log("data",data);
                    if (StatusId === true) {
                        alert('Data Saved Successfully');
                        $scope.PLDB.IsFinalSubmit = true;
                        $scope.PLDB = {};             
                        window.location = SiteUrl + "PLDB/PLDBLoanApplicationForm?eId=" + encodeURIComponent(data.encGccid) + "&mode=e";
                    }
                    else if (StatusId === false) {
                        alert('Partial Save Successful');                  
                        window.location = SiteUrl + "PLDB/PLDBLoanApplicationForm?eId=" + encodeURIComponent(data.encGccid) + "&mode=e";
                    }
                }
                else {
                    alert('Data Finally Submit Successfully');
                    $scope.PLDB = {};                   
                    window.location = SiteUrl + "PLDB/PLDBLoanApplicationForm?eId=" + encodeURIComponent(data.encGccid) + "&mode=e";
                }

            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });

    }



    $scope.Save_PLDBFormUploadDoc = function (StatusId) {
        debugger;

        $(".se-pre-con").fadeIn("slow");

       // $scope.PLDB.GuarantorDetails = $scope.PLDB.GuarantorDetails || {};
        $scope.PLDB.LoanDisbursement = $scope.PLDB.LoanDisbursement || {};

        // KEEP OLD DOCUMENT VALUES SAFE
        if (!$scope.PLDB.LoanDisbursement) {
            $scope.PLDB.LoanDisbursement = {};
        }

        $scope.PLDB.LoanDisbursement.LoanDocument =
            $scope.PLDB.LoanDisbursement.LoanDocument || $("#LoanDocument").val();

        $scope.PLDB.LoanDisbursement.LoanDocument2 =
            $scope.PLDB.LoanDisbursement.LoanDocument2 || $("#LoanDocument2").val();

        $scope.PLDB.LoanDisbursement.LoanDocument3 =
            $scope.PLDB.LoanDisbursement.LoanDocument3 || $("#LoanDocument3").val();  

        $scope.PLDB.VName = $scope.GetNameFromList($scope.Villages, $scope.PLDB.VId, 'Village');
        $scope.PLDB.DistrictName = $scope.GetNameFromList($scope.DistrictList, $scope.PLDB.DistrictId, 'District');
        $scope.PLDB.BlockName = $scope.GetNameFromList($scope.Blocksdata, $scope.PLDB.BlockId, 'Block');
        $scope.PLDB.GpName = $scope.GetNameFromList($scope.GramPanchayats, $scope.PLDB.GpId, 'GramPanchayat');

        $scope.PLDB.LoanApplicationDate = $scope.DateFormat($scope.PLDB.LoanApplicationDate);
        $scope.PLDB.LoanAcceptDate = $scope.DateFormat($scope.PLDB.LoanAcceptDate);
        $scope.PLDB.bankPartyEnrollementNumberDate = $scope.DateFormat($scope.PLDB.bankPartyEnrollementNumberDate);
        $scope.PLDB.AuthorizationLetterIssueDate = $scope.DateFormat($scope.PLDB.AuthorizationLetterIssueDate);

        $scope.PLDB.IsFinalSubmit = StatusId;

        $scope.PLDB.BillDate = $scope.DateFormat($scope.PLDB.BillDate);
        $scope.PLDB.Accept_NAccpetDate = $scope.DateFormat($scope.PLDB.Accept_NAccpetDate);

        $scope.PLDB.LoanDisbursement.FirstInstallmentDate = $scope.DateFormat($scope.PLDB.LoanDisbursement.FirstInstallmentDate);

        $scope.PLDB.LoanDisbursement.SecondInstallmentDate = $scope.DateFormat($scope.PLDB.LoanDisbursement.SecondInstallmentDate);

        $scope.PLDB.LoanDisbursement.ThirdInstallmentDate = $scope.DateFormat($scope.PLDB.LoanDisbursement.ThirdInstallmentDate);

        $scope.PLDB.LoanDisbursement.BillDate = $scope.DateFormat($scope.PLDB.LoanDisbursement.BillDate);

        $scope.PLDB.LoanDisbursement.CertificateFirstInstallmentDate = $scope.DateFormat($scope.PLDB.LoanDisbursement.CertificateFirstInstallmentDate);

        $scope.PLDB.LoanDisbursement.CertificateSecondInstallmentDate = $scope.DateFormat($scope.PLDB.LoanDisbursement.CertificateSecondInstallmentDate);

        $scope.PLDB.LoanDisbursement.CertificateThirdInstallmentDate = $scope.DateFormat($scope.PLDB.LoanDisbursement.CertificateThirdInstallmentDate);

        $scope.PLDB.LoanDisbursement.LoanDocument = $scope.PLDB.LoanDisbursement.LoanDocument;

        $scope.PLDB.IsLoanDetailsAgrimMICRCheckTaken = $scope.toOneZero($scope.PLDB.IsLoanDetailsAgrimMICRCheckTaken);

        $scope.PLDB.BAIsLandMapAttached = $scope.toOneZero($scope.PLDB.BAIsLandMapAttached);

        $scope.PLDB.AIsJamabandiAttached = $scope.toOneZero($scope.PLDB.AIsJamabandiAttached);

        $scope.PLDB.AIsLandMapAttached = $scope.toOneZero($scope.PLDB.AIsLandMapAttached);

        $scope.PLDB.AIsOwnershipAffidavitAttached = $scope.toOneZero($scope.PLDB.AIsOwnershipAffidavitAttached);

        $scope.PLDB.AIsKhasraGirdawariCopyAttached = $scope.toOneZero($scope.PLDB.AIsKhasraGirdawariCopyAttached);

        $scope.PLDB.BApplicantregistrationAttact = $scope.toOneZero($scope.PLDB.BApplicantregistrationAttact);

        $scope.PLDB.IsEstimateQuotationAttached = $scope.toOneZero($scope.PLDB.IsEstimateQuotationAttached);

        $scope.PLDB.IsRegistrationCertificateReceived = $scope.toOneZero($scope.PLDB.IsRegistrationCertificateReceived);

        $scope.PLDB.GuarantorDetails.IsFirstAgrimMICRCheckTaken = $scope.toOneZero($scope.PLDB.GuarantorDetails.IsFirstAgrimMICRCheckTaken);

        $scope.PLDB.GuarantorDetails.IsSecondAgrimMICRCheckTaken = $scope.toOneZero($scope.PLDB.GuarantorDetails.IsSecondAgrimMICRCheckTaken);

        $scope.PLDB.LoanDisbursement.IsApprovalOnNotesheet = $scope.toOneZero($scope.PLDB.LoanDisbursement.IsApprovalOnNotesheet);

        /*     add By Bhavesh*/
        $scope.PLDB.LoanDisbursement.IsIssueofPassbook = $scope.toOneZero($scope.PLDB.LoanDisbursement.IsIssueofPassbook);


        $http.post(SiteUrl + "/PLDB/Save_PldbLoanApplicationDetails", $scope.PLDB).
            success(function (data, status) {
                if (data.Result > 0) {

                    $scope.PLDB.PLDB_OTS_SchemeID = data;
                    if (StatusId === false) {
                        alert('Document Saved Successfully');
                        $scope.PLDB.IsFinalSubmit = false;
                        $scope.PLDB = {};                     
                        window.location = SiteUrl + "PLDB/PLDBLoanApplicationForm?eId=" + encodeURIComponent(data.encGccid) + "&mode=e";
                       
                    }
                    else {
                        alert('Document Saved Successfully');
                        window.location.reload();                       
                        window.location = SiteUrl + "PLDB/PLDBLoanApplicationForm?eId=" + encodeURIComponent(data.encGccid) + "&mode=e";                       
                    }
                }
                else {
                    alert('Document Saved Successfully');
                    $scope.PLDB = {};
                    window.location.reload();               
                }

            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });

    }

    $scope.ClearOtherGPFields = function () {

        if ($scope.PLDB.IsOtherGramPanchayat != 1) {
            $scope.PLDB.GP_Other = '';
            $scope.PLDB.Village_Other = '';
        }
    };


    $scope.FinancialYearList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1")
            .success(function (data, status) {

                $scope.FinancialYearList = data.filter(function (item) {
                    return item.FIN_YEAR_NAME == "2025-26" || item.FIN_YEAR_NAME == "2026-27";
                });                

            })
            .error(function (data, status) {

                $scope.FinancialYearList = [];
            });
    }

    $scope.GetFormEditData = function (PldbOtsId) {

        $http.post(SiteUrl + "/PLDB/GetPLDBLoanApplicationListFormViewData/?PldbOtsSchemeId=" + PldbOtsId).
            success(function (data, status) {

                $scope.PLDB = $scope.PLDB_OTSSchemeData;

                $scope.PLDB = JSON.parse(data.Main)[0];
                $scope.GuarantorList = JSON.parse(data.Guarantor); // List of guarantors
                $scope.Disbursement = JSON.parse(data.Disbursement); // Loan disbursement              

                //here PLDB Loan Application Details

                $scope.PLDB.PANNumber = $scope.PLDB.PANNumber;
                $scope.PLDB.GP_Other = $scope.PLDB.GP_Other;
                $scope.PLDB.Village_Other = $scope.PLDB.Village_Other;
                $scope.PLDB.FinancialYearId = ($scope.PLDB.FinancialYearId || '0').toString();;
                $scope.PLDB.AIsJamabandiAttached = $scope.PLDB.AIsJamabandiAttached ? 1 : 0;
                $scope.PLDB.IsRegistrationCertificateReceived = $scope.PLDB.IsRegistrationCertificateReceived ? 1 : 0;
                $scope.PLDB.IsEstimateQuotationAttached = $scope.PLDB.IsEstimateQuotationAttached ? 1 : 0;
                $scope.PLDB.AIsKhasraGirdawariCopyAttached = $scope.PLDB.AIsKhasraGirdawariCopyAttached ? 1 : 0;
                $scope.PLDB.AIsLandMapAttached = $scope.PLDB.AIsLandMapAttached ? 1 : 0;
                $scope.PLDB.AIsOwnershipAffidavitAttached = $scope.PLDB.AIsOwnershipAffidavitAttached ? 1 : 0;
                $scope.PLDB.IsLoanDetailsAgrimMICRCheckTaken = $scope.PLDB.IsLoanDetailsAgrimMICRCheckTaken ? 1 : 0;
                $scope.PLDB.BApplicantregistrationAttact = $scope.PLDB.BApplicantregistrationAttact ? 1 : 0;
                $scope.PLDB.BAIsLandMapAttached = $scope.PLDB.BAIsLandMapAttached ? 1 : 0;
                $scope.PLDB.IsOtherGramPanchayat = $scope.PLDB.IsOtherGramPanchayat ? 1 : 0;

                $scope.PLDB.SectorId = ($scope.PLDB.SectorId || '0').toString();
                $scope.PLDB.SectorId2 = ($scope.PLDB.SectorId2 || '0').toString();
                $scope.PLDB.SectorId3 = ($scope.PLDB.SectorId3 || '0').toString();
                $scope.PLDB.SectorId4 = ($scope.PLDB.SectorId4 || '0').toString();
                $scope.PLDB.SectorId5 = ($scope.PLDB.SectorId5 || '0').toString();
                $scope.PLDB.SectorId6 = ($scope.PLDB.SectorId6 || '0').toString();
                $scope.PLDB.SectorId7 = ($scope.PLDB.SectorId7 || '0').toString();

                $scope.LoadSector();
                $scope.PLDB.PldbId = ($scope.PLDB.PldbId || '0').toString();
                $scope.PLDB.BranchId = ($scope.PLDB.BranchId || '0').toString();
                $scope.PLDB.PurposeId = ($scope.PLDB.PurposeId || '0').toString();
                $scope.PLDB.PurposeId2 = ($scope.PLDB.PurposeId2 || '0').toString();
                $scope.PLDB.PurposeId3 = ($scope.PLDB.PurposeId3 || '0').toString();
                $scope.PLDB.PurposeId4 = ($scope.PLDB.PurposeId4 || '0').toString();
                $scope.PLDB.PurposeId5 = ($scope.PLDB.PurposeId5 || '0').toString();
                $scope.PLDB.PurposeId6 = ($scope.PLDB.PurposeId6 || '0').toString();
                $scope.PLDB.PurposeId7 = ($scope.PLDB.PurposeId7 || '0').toString();
                $scope.PLDB.TehsilId = ($scope.PLDB.TehsilId || '0').toString();
                $scope.PLDB.CityId = ($scope.PLDB.CityId || '0').toString();
                $scope.PLDB.WardId = ($scope.PLDB.WardId || '0').toString();

                $scope.PLDB.AreaType = ($scope.PLDB.IsRural == 1) ? 'Rural' : 'Urban';

                if ($scope.PLDB.AreaType == 'Rural') {

                    $scope.BlockListByOffAdd(1);

                    setTimeout(function () {
                        $scope.$applyAsync(function () {

                            if ($scope.PLDB.BlockId !== '0') {
                                $scope.GramPanchayatList(1);
                            }

                            if ($scope.PLDB.GP_ID !== '0') {
                                $scope.VillageList(1);
                            }

                        });
                    }, 400);

                } else {

                    // Urban flow
                    $scope.GetCityListByCityTypes(1);

                    setTimeout(function () {
                        $scope.$applyAsync(function () {

                            if ($scope.PLDB.CityId !== '0') {
                                $scope.GetWardListbySociety();
                            }

                        });
                    }, 400);
                }



                $scope.TehsilList();
                $scope.PLDB.AssemblyId = ($scope.PLDB.AssemblyId || '0').toString();
                $scope.getAssembly();
                $scope.LoadPurposeByShemeId($scope.PLDB.SectorId);
                $scope.LoadPurposeByShemeId2($scope.PLDB.SectorId2);
                $scope.LoadPurposeByShemeId3($scope.PLDB.SectorId3);
                $scope.LoadPurposeByShemeId4($scope.PLDB.SectorId4);
                $scope.LoadPurposeByShemeId5($scope.PLDB.SectorId5);
                $scope.LoadPurposeByShemeId6($scope.PLDB.SectorId6);
                $scope.LoadPurposeByShemeId7($scope.PLDB.SectorId7);
                $scope.PLDBBranchList($scope.PLDB.BranchId);

                if ($scope.PLDB.LoanAcceptDate != null)
                    $scope.PLDB.LoanAcceptDate = new Date($scope.PLDB.LoanAcceptDate);

                if ($scope.PLDB.LoanApplicationDate != null)
                    $scope.PLDB.LoanApplicationDate = new Date($scope.PLDB.LoanApplicationDate);

                if ($scope.PLDB.bankPartyEnrollementNumberDate != null)
                    $scope.PLDB.bankPartyEnrollementNumberDate = new Date($scope.PLDB.bankPartyEnrollementNumberDate);

                if ($scope.PLDB.AuthorizationLetterIssueDate != null)
                    $scope.PLDB.AuthorizationLetterIssueDate = new Date($scope.PLDB.AuthorizationLetterIssueDate);

                if ($scope.PLDB.Accept_NAccpetDate != null)
                    $scope.PLDB.Accept_NAccpetDate = new Date($scope.PLDB.Accept_NAccpetDate);

                // here GuarantorList

                if ($scope.GuarantorList && $scope.GuarantorList.length > 0) {

                    if (!$scope.PLDB.GuarantorDetails) {
                        $scope.PLDB.GuarantorDetails = {};
                    }

                    $scope.PLDB.GuarantorDetails.First_Name = $scope.GuarantorList[0].First_Name;
                    $scope.PLDB.GuarantorDetails.First_MobileNo = $scope.GuarantorList[0].First_MobileNo;
                    $scope.PLDB.GuarantorDetails.First_AadharNo = $scope.GuarantorList[0].First_AadharNo;
                    $scope.PLDB.GuarantorDetails.First_Bhumi_Area = $scope.GuarantorList[0].First_Bhumi_Area;
                    $scope.PLDB.GuarantorDetails.First_Bhumi_Mulyaa = $scope.GuarantorList[0].First_Bhumi_Mulyaa;
                    $scope.PLDB.GuarantorDetails.First_Other_Area = $scope.GuarantorList[0].First_Other_Area;
                    $scope.PLDB.GuarantorDetails.First_Other_Mulyaa = $scope.GuarantorList[0].First_Other_Mulyaa;
                    $scope.PLDB.GuarantorDetails.First_OccupationOrServiceType = $scope.GuarantorList[0].First_OccupationOrServiceType;
                    $scope.PLDB.GuarantorDetails.First_OccupationOrServiceTypeId = ($scope.GuarantorList[0].First_OccupationOrServiceTypeId || '').toString();
                    $scope.PLDB.GuarantorDetails.First_ChequeNumber = $scope.GuarantorList[0].First_ChequeNumber;
                    $scope.PLDB.GuarantorDetails.First_Bank = $scope.GuarantorList[0].First_Bank;
                    $scope.PLDB.GuarantorDetails.First_Branch = $scope.GuarantorList[0].First_Branch;
                    $scope.PLDB.GuarantorDetails.Second_Name = $scope.GuarantorList[0].Second_Name;
                    $scope.PLDB.GuarantorDetails.Second_MobileNo = $scope.GuarantorList[0].Second_MobileNo;
                    $scope.PLDB.GuarantorDetails.Second_AadharNo = $scope.GuarantorList[0].Second_AadharNo;
                    $scope.PLDB.GuarantorDetails.Second_Bhumi_Area = $scope.GuarantorList[0].Second_Bhumi_Area;
                    $scope.PLDB.GuarantorDetails.Second_Bhumi_Mulyaa = $scope.GuarantorList[0].Second_Bhumi_Mulyaa;
                    $scope.PLDB.GuarantorDetails.Second_Other_Area = $scope.GuarantorList[0].Second_Other_Area;
                    $scope.PLDB.GuarantorDetails.Second_Other_Mulyaa = $scope.GuarantorList[0].Second_Other_Mulyaa;
                    $scope.PLDB.GuarantorDetails.Second_OccupationOrServiceType = $scope.GuarantorList[0].Second_OccupationOrServiceType;
                    $scope.PLDB.GuarantorDetails.Second_OccupationOrServiceTypeId = ($scope.GuarantorList[0].Second_OccupationOrServiceTypeId || '').toString();
                    $scope.PLDB.GuarantorDetails.Second_ChequeNumber = $scope.GuarantorList[0].Second_ChequeNumber;
                    $scope.PLDB.GuarantorDetails.Second_Bank = $scope.GuarantorList[0].Second_Bank;
                    $scope.PLDB.GuarantorDetails.Second_Branch = $scope.GuarantorList[0].Second_Branch;
                    $scope.PLDB.GuarantorDetails.IsFirstAgrimMICRCheckTaken = $scope.GuarantorList[0].IsFirstAgrimMICRCheckTaken == true ? 1 : 0;
                    $scope.PLDB.GuarantorDetails.IsSecondAgrimMICRCheckTaken = $scope.GuarantorList[0].IsSecondAgrimMICRCheckTaken == true ? 1 : 0;

                }

                //Loan Disbursement
                if ($scope.Disbursement && $scope.Disbursement.length > 0) {

                    if (!$scope.PLDB.LoanDisbursement) {
                        $scope.PLDB.LoanDisbursement = {};
                    }

                    $scope.PLDB.LoanDisbursement.IsApprovalOnNotesheet = $scope.Disbursement[0].IsApprovalOnNotesheet == true ? 1 : 0;
                    /*added by Bhavesh*/
                    $scope.PLDB.LoanDisbursement.IsIssueofPassbook = $scope.Disbursement[0].IsIssueofPassbook == true ? 1 : 0;

                    $scope.PLDB.LoanDisbursement.FirstInstallmentMoney = $scope.Disbursement[0].FirstInstallmentMoney;
                    $scope.PLDB.LoanDisbursement.SecondInstallmentMoney = $scope.Disbursement[0].SecondInstallmentMoney;
                    $scope.PLDB.LoanDisbursement.ThirdInstallmentMoney = $scope.Disbursement[0].ThirdInstallmentMoney;
                    $scope.PLDB.LoanDisbursement.VendorChequeNumber = $scope.Disbursement[0].VendorChequeNumber;
                    $scope.PLDB.LoanDisbursement.ReceiptNumber = $scope.Disbursement[0].ReceiptNumber;
                    $scope.PLDB.LoanDisbursement.VendorAmount = $scope.Disbursement[0].VendorAmount;
                    $scope.PLDB.LoanDisbursement.BillNumber = $scope.Disbursement[0].BillNumber;
                    $scope.PLDB.LoanDisbursement.GSTNo = $scope.Disbursement[0].GSTNo;
                    $scope.PLDB.LoanDisbursement.LoanTimeInYear = $scope.Disbursement[0].LoanTimeInYear;
                    $scope.PLDB.LoanDisbursement.InstallmentYearWise = $scope.Disbursement[0].InstallmentYearWise;
                    $scope.PLDB.LoanDisbursement.InsuranceMedicalCertificate = $scope.Disbursement[0].InsuranceMedicalCertificate;
                    $scope.PLDB.LoanDisbursement.LoanSupervisorName = $scope.Disbursement[0].LoanSupervisorName;
                    $scope.PLDB.LoanDisbursement.BranchSecretaryName = $scope.Disbursement[0].BranchSecretaryName;
                    $scope.PLDB.LoanDisbursement.SecretaryName = $scope.Disbursement[0].SecretaryName;
                    $scope.PLDB.LoanDisbursement.Member1 = $scope.Disbursement[0].Member1;
                    $scope.PLDB.LoanDisbursement.Member2 = $scope.Disbursement[0].Member2;
                    $scope.PLDB.LoanDisbursement.Member3 = $scope.Disbursement[0].Member3;
                    $scope.PLDB.LoanDisbursement.Member4 = $scope.Disbursement[0].Member4;
                    $scope.PLDB.LoanDisbursement.Member5 = $scope.Disbursement[0].Member5;
                    $scope.PLDB.LoanDisbursement.AdministratorName = $scope.Disbursement[0].AdministratorName;
                    $scope.PLDB.LoanDisbursement.LoanDocument = $scope.Disbursement[0].LoanDocument;
                    $scope.PLDB.LoanDisbursement.LoanDocument2 = $scope.Disbursement[0].LoanDocument2;
                    $scope.PLDB.LoanDisbursement.LoanDocument3 = $scope.Disbursement[0].LoanDocument3;

                    if ($scope.Disbursement[0].FirstInstallmentDate != null)
                        $scope.PLDB.LoanDisbursement.FirstInstallmentDate = new Date($scope.Disbursement[0].FirstInstallmentDate);

                    if ($scope.Disbursement[0].SecondInstallmentDate != null)
                        $scope.PLDB.LoanDisbursement.SecondInstallmentDate = new Date($scope.Disbursement[0].SecondInstallmentDate);

                    if ($scope.Disbursement[0].ThirdInstallmentDate != null)
                        $scope.PLDB.LoanDisbursement.ThirdInstallmentDate = new Date($scope.Disbursement[0].ThirdInstallmentDate);

                    if ($scope.Disbursement[0].BillDate != null)
                        $scope.PLDB.LoanDisbursement.BillDate = new Date($scope.Disbursement[0].BillDate);

                    if ($scope.Disbursement[0].CertificateFirstInstallmentDate != null)
                        $scope.PLDB.LoanDisbursement.CertificateFirstInstallmentDate = new Date($scope.Disbursement[0].CertificateFirstInstallmentDate);

                    if ($scope.Disbursement[0].CertificateSecondInstallmentDate != null)
                        $scope.PLDB.LoanDisbursement.CertificateSecondInstallmentDate = new Date($scope.Disbursement[0].CertificateSecondInstallmentDate);

                    if ($scope.Disbursement[0].CertificateThirdInstallmentDate != null)
                        $scope.PLDB.LoanDisbursement.CertificateThirdInstallmentDate = new Date($scope.Disbursement[0].CertificateThirdInstallmentDate);

                    $scope.PLDB.LoanDisbursement.InstallmentYearWiseId = ($scope.Disbursement[0].InstallmentYearWiseId || '').toString();


                }

                $scope.PLDB.DistrictId = ($scope.PLDB.DistrictId || '0').toString();

                if ($scope.isViewMode || $scope.isEditMode) {
                    if ($scope.PLDB.IsRural == 'true' || $scope.PLDB.IsRural == true) {
                        $scope.PLDB.AreaType = 'Rural';
                    } else {
                        $scope.PLDB.AreaType = 'Urban';
                    }
                    $scope.TehsilList();
                    $scope.getAssembly();
                    $scope.BlockListByOffAdd(1);
                    setTimeout(function () {
                        $scope.$applyAsync(function () {
                            if ($scope.PLDB.BlockId && $scope.PLDB.BlockId !== '0') {
                                $scope.GramPanchayatList(1);
                            }
                            if ($scope.PLDB.GP_ID && $scope.PLDB.GP_ID !== '0') {
                                $scope.VillageList(1);
                            }
                        });
                    }, 400);
                }
            }).
            error(function (data, status) {
                $scope.PLDB = [];
            });
    }

    $scope.validateLoanAcceptPay = function () {
        if ($scope.PLDB.LoanAcceptPay > $scope.PLDB.SubsidyLoanAmount) {
            $scope.PLDB.LoanAcceptPay = $scope.PLDB.SubsidyLoanAmount;
            $scope.loanError = true;
        } else {
            $scope.loanError = false;
        }
    };

    //sector 1
    $scope.LoadPurposeByShemeId = function (ShemeId, index) {

        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetPurposeMaster", { SchemeId: $scope.PLDB.SectorId }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PurposeOfLoanList = [];
                $scope.PurposeOfLoanList = data;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
            });
    };


    //sector 2

    $scope.LoadPurposeByShemeId2 = function (ShemeId, index) {

        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetPurposeMaster", { SchemeId: $scope.PLDB.SectorId2 }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PurposeOfLoanList2 = [];
                $scope.PurposeOfLoanList2 = data;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
            });
    };


    //sector 3

    $scope.LoadPurposeByShemeId3 = function (ShemeId, index) {

        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetPurposeMaster", { SchemeId: $scope.PLDB.SectorId3 }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PurposeOfLoanList3 = [];
                $scope.PurposeOfLoanList3 = data;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
            });
    };


    //sector 4

    $scope.LoadPurposeByShemeId4 = function (ShemeId, index) {

        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetPurposeMaster", { SchemeId: $scope.PLDB.SectorId4 }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PurposeOfLoanList4 = [];
                $scope.PurposeOfLoanList4 = data;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
            });
    };


    //sector 5

    $scope.LoadPurposeByShemeId5 = function (ShemeId, index) {

        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetPurposeMaster", { SchemeId: $scope.PLDB.SectorId5 }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PurposeOfLoanList5 = [];
                $scope.PurposeOfLoanList5 = data;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
            });
    };

    //sector 6

    $scope.LoadPurposeByShemeId6 = function (ShemeId, index) {
        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetPurposeMaster", { SchemeId: $scope.PLDB.SectorId6 }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PurposeOfLoanList6 = [];
                $scope.PurposeOfLoanList6 = data;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
            });
    };


    //sector 7

    $scope.LoadPurposeByShemeId7 = function (ShemeId, index) {

        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetPurposeMaster", { SchemeId: $scope.PLDB.SectorId7 }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PurposeOfLoanList7 = [];
                $scope.PurposeOfLoanList7 = data;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
            });
    };



    $scope.GetSchemeMasterBySector = function (Id, index) {
        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetSchemeMaster", { SectorId: Id }).
            success(function (data, status) {
                $scope.LoanSchemeList = [];
                $scope.LoanSchemeList = data;
                $scope.TotalRecord = $scope.LoanSchemeList.length;
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                alert("Error while fetching Society Details.");
            });
    }

    $scope.BackToPage = function () {
        window.location = '../PLDB/PLDBSummaryODApplicationList';
    }

    $scope.$watchGroup(
        [
            'PLDB.ODPrincipal',
            'PLDB.ODInsurancePremium',
            'PLDB.ODInterest',
            'PLDB.ODPenalInterast',
            'PLDB.CDPrincipal',
            'PLDB.CDInterest',
            'PLDB.CDPenalInterest'

        ],
        function (newValues, oldValues) {

            var principal = parseFloat($scope.PLDB.ODPrincipal) || 0;
            var premium = parseFloat($scope.PLDB.ODInsurancePremium) || 0;
            var interest = parseFloat($scope.PLDB.ODInterest) || 0;
            var penal = parseFloat($scope.PLDB.ODPenalInterast) || 0;

            var CDPrincipal = parseFloat($scope.PLDB.CDPrincipal) || 0;
            var CDInterest = parseFloat($scope.PLDB.CDInterest) || 0;
            var CDPenalInterest = parseFloat($scope.PLDB.CDPenalInterest) || 0;

            $scope.PLDB.ODTotal = (principal + premium + interest + penal).toFixed(2);
            $scope.PLDB.CDTotal = (CDPrincipal + CDInterest + CDPenalInterest).toFixed(2);
        }
    );

    $scope.SelectReceiptFile = function (files) {

        if (files && files.length > 0) {

            if (!$scope.PLDB) {
                $scope.PLDB = {};
            }

            if (!$scope.PLDB.LoanDisbursement) {
                $scope.PLDB.LoanDisbursement = {};
            }

            $scope.PLDB.LoanDisbursement.LoanDocument = files[0].name;

            $scope.UploadLoanDocument(files);
        }
    };
   
    //LoanDocument 1
    $scope.UploadLoanDocument = function (files) {

        if (!files || !files.length) {
            alert("No file selected");
            return;
        }

        let file = files[0];
        let fd = new FormData();
        fd.append("LoanDocument", file);

        $http.post(SiteUrl + "/PLDB/UploadLoanDocument", fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
            .then(function (response) {
                let r = response.data;
                if (r && r.success) {
                    $scope.PLDB.LoanDisbursement.LoanDocument = r.fileName;
                    alert("Document Uploaded Successfully");
                }
                else {
                    alert(r.message || "Upload Failed");
                }
            })
            .catch(function (error) {
                alert("Error uploading document");
            });
    };


    //LoanDocument 2
    //$scope.SelectReceiptFile2 = function (files) {

    //    if (files && files.length > 0) {
    //        $scope.PLDB = $scope.PLDB || {};
    //        $scope.PLDB.LoanDisbursement = $scope.PLDB.LoanDisbursement || {};
    //        $scope.PLDB.LoanDisbursement.LoanDocument2 = files[0].name;
    //        $scope.UploadLoanDocument2(files);
    //    }
    //};

    $scope.SelectReceiptFile2 = function (files) {

        if (files && files.length > 0) {

            if (!$scope.PLDB) {
                $scope.PLDB = {};
            }

            if (!$scope.PLDB.LoanDisbursement) {
                $scope.PLDB.LoanDisbursement = {};
            }

            $scope.PLDB.LoanDisbursement.LoanDocument2 = files[0].name;

            $scope.UploadLoanDocument2(files);
        }
    };

    //LoanDocument 2
    $scope.UploadLoanDocument2 = function (files) {

        if (!files || !files.length) {
            alert("No file selected");
            return;
        }

        let file = files[0];
        let fd = new FormData();
        fd.append("LoanDocument2", file);

        $http.post(SiteUrl + "/PLDB/UploadLoanDocument2", fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
            .then(function (response) {
                let r = response.data;
                if (r && r.success) {
                    $scope.PLDB.LoanDisbursement.LoanDocument2 = r.fileName;
                    alert("Document Uploaded Successfully");
                }
                else {
                    alert(r.message || "Upload Failed");
                }
            })
            .catch(function (error) {
                alert("Error uploading document");
            });
    };



    //LoanDocument 3
    $scope.SelectReceiptFile3 = function (files) {

        if (files && files.length > 0) {

            if (!$scope.PLDB) {
                $scope.PLDB = {};
            }

            if (!$scope.PLDB.LoanDisbursement) {
                $scope.PLDB.LoanDisbursement = {};
            }

            $scope.PLDB.LoanDisbursement.LoanDocument3 = files[0].name;

            $scope.UploadLoanDocument3(files);
        }
    };

    //LoanDocument 3
    $scope.UploadLoanDocument3 = function (files) {

        if (!files || !files.length) {
            alert("No file selected");
            return;
        }

        let file = files[0];
        let fd = new FormData();
        fd.append("LoanDocument3", file);

        $http.post(SiteUrl + "/PLDB/UploadLoanDocument3", fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
            .then(function (response) {
                let r = response.data;
                if (r && r.success) {
                    $scope.PLDB.LoanDisbursement.LoanDocument3 = r.fileName;
                    alert("Document Uploaded Successfully");
                }
                else {
                    alert(r.message || "Upload Failed");
                }
            })
            .catch(function (error) {
                alert("Error uploading document");
            });
    };
   

    $scope.RemoveLoanDocument = function () {
        if (!confirm("Are you sure you want to remove this document?"))
            return;

        var fileName = $scope.PLDB.LoanDisbursement.LoanDocument;

        $http.post(SiteUrl + "/PLDB/DeleteLoanDocument", { fileName: fileName })
            .then(function () {
                $scope.PLDB.LoanDisbursement.LoanDocument = "";
                document.getElementById("LoanDocument").value = "";
            }, function () {
                alert("Failed to remove document");
            });
    };


    $scope.RemoveLoanDocument2 = function () {
        if (!confirm("Are you sure you want to remove this document?"))
            return;

        var fileName = $scope.PLDB.LoanDisbursement.LoanDocument2;

        $http.post(SiteUrl + "/PLDB/DeleteLoanDocument2", { fileName: fileName })
            .then(function () {
                $scope.PLDB.LoanDisbursement.LoanDocument2 = "";
                document.getElementById("LoanDocument2").value = "";
            }, function () {
                alert("Failed to remove document");
            });
    };


    $scope.RemoveLoanDocument3 = function () {
        if (!confirm("Are you sure you want to remove this document?"))
            return;

        var fileName = $scope.PLDB.LoanDisbursement.LoanDocument3;

        $http.post(SiteUrl + "/PLDB/DeleteLoanDocument3", { fileName: fileName })
            .then(function () {
                $scope.PLDB.LoanDisbursement.LoanDocument3 = "";
                document.getElementById("LoanDocument3").value = "";
            }, function () {
                alert("Failed to remove document");
            });
    };


    $scope.GetPLDBSectorNewODList();
    $scope.GetSchemeMasterBySector();

    $scope.onload();

});
