var CommityRegistrtionModule = angular.module('LoanApplicationMod', ['angularUtils.directives.dirPagination']);

CommityRegistrtionModule.controller('LoanApplicationCnt', function ($scope, $http, $rootScope, $timeout) {

    $scope.onload = function () {
    const urlParams = new URLSearchParams(window.location.search);
    $scope.PldbOtsEncId = urlParams.get('eId');

        $scope.mode = urlParams.get('mode');
        $scope.isViewMode = ($scope.mode === 'v');    
     
        if ($scope.PldbOtsEncId && $scope.PldbOtsEncId !== "") {
            $scope.GetFormEditData($scope.PldbOtsEncId);
        }     
        $scope.IsEditable = !$scope.isViewMode;
        
        if ($scope.isViewMode) {
            setTimeout(function () {
                $('input, select, textarea ,.datepicker, .ui-datepicker').prop('disabled', true);
                $('input[type=submit], button[type=submit]').hide();               
            }, 600);
        }    

        $scope.DistrictFreezed = false;
        $scope.PLDBFreezed = false;   
        $scope.PLDBBranchFreezed = false;        


    $scope.IsEditable = true;
    $scope.isEditLoading = false;

    $scope.LoanApplicationLetterTitle = "";
    $scope.ShowApplicanDetails = false;
    $scope.IsShowPartialSave = false;
    $scope.IsSubmiApplication = false;
    $scope.LoanApplicationList = [];
    $scope.PendingLevelList = [];
    $scope.MLPendingLevelList = [];
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

    $scope.PLDB = {};
    }

    $scope.PLDBBranchList = function (branch_ID) {       
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + $scope.PLDB.PLDB_Id).
            success(function (data, status) {
                $scope.PLDBBranches = data;         

                var PLDBBranchLoginId = $('#hdnPLDBBranchId').val();
                if (PLDBBranchLoginId && PLDBBranchLoginId != "0") {
                    $scope.PLDB.PLDB_BranchId = PLDBBranchLoginId;
                    $scope.PLDBBranchFreezed = true;
                    return;
                } else if ($scope.PLDB.PLDB_BranchId != "0") {                   
                    $scope.PLDB.PLDB_BranchId = branch_ID.toString();          
                }                          

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
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
                var PLDBLoginId = $('#hdnPLDBId').val();               
                if (PLDBLoginId && PLDBLoginId != "0") {
                    $scope.PLDB.PLDB_Id = PLDBLoginId.toString();
                    $scope.PLDBFreezed = true;
                    $scope.PLDBBranchList($scope.PLDB.PLDB_Id);
                    return;
                }

                if ($scope.CategoryList && $scope.CategoryList.length > 0 && $scope.PLDB_OTSSchemeData && $scope.PLDB_OTSSchemeData.CategoryName != "" && ($scope.PLDB_OTSSchemeData.CategoryId == undefined || $scope.PLDB_OTSSchemeData.CategoryId == 0 || $scope.PLDB_OTSSchemeData.CategoryId == "0")) {

                    $scope.PLDB.CategoryId = $scope.CategoryList.filter(function (x) { return x.CODENAME.toString() == $scope.PLDB.CategoryName })[0].CODEID;
                    $scope.PLDB.CategoryName = $scope.CategoryList.filter(function (x) { return x.CODEID.toString() == $scope.PLDB.CategoryId })[0].CODENAME;
                }
                else {
                    $scope.PLDB.CategoryId = $scope.PLDB_OTSSchemeData ? $scope.PLDB_OTSSchemeData.CategoryId.toString() : '0';
                }
            }).
            error(function (data, status) {
                $scope.PLDB = [];
            });
    }

    $scope.LoadMaster();
    
    $scope.getTranslation();

    $scope.GetNameFromList = function (list, id, type) {
        debugger;
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
            } else if (type == 'PLDBBranch') {
                var data = list.filter(function (x) { return x.BLOCK_ID === id.toString() })[0];
                return _language == 'en-us' ? data.BLOCK_ENG : data.BLOCK_ENG;
               // return data.BLOCK_ENG;
            }

        } catch (ex) {
            return "";
        }
    }

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
                    $scope.PLDB.SectorId = $scope.PLDB_OTSSchemeData ? $scope.PLDB_OTSSchemeData.SectorId.toString() : "0";
                }

            }).
            error(function (data, status) {
                $scope.LoanSectorList = [];
                $(".blockUI").fadeOut("slow");
            });
    };

    $scope.GetDistrictList = function (id) {

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT").
            success(function (data, status) {
                $scope.DistrictList = data;
               
                var DistrictLoginId = $('#hdnDistrictId').val();
               
                if (DistrictLoginId && DistrictLoginId != "0") {
                    $scope.PLDB.DistrictId = DistrictLoginId.toString();
                    $scope.DistrictFreezed = true;   
                    return;
                }

                if ($scope.DistrictList && $scope.DistrictList.length > 0 && $scope.PLDB_OTSSchemeData && $scope.PLDB_OTSSchemeData.DistrictName != "" && ($scope.PLDB_OTSSchemeData.DistrictId == undefined || $scope.PLDB_OTSSchemeData.DistrictId == 0 || $scope.PLDB_OTSSchemeData.DistrictId == "0")) {
                    var matchedDistrict = $scope.DistrictList.find(function (x) {
                        return x.DISTRICT_ENG.toLowerCase() == $scope.PLDB_OTSSchemeData.DistrictName.toLowerCase();
                    });

                    if (matchedDistrict) {
                        $scope.PLDB.DistrictId = matchedDistrict.DISTRICT_ID;
                        $scope.PLDB.DistrictName = matchedDistrict.DISTRICT_ENG;

                        //$scope.PLDB.DistrictId = $scope.DistrictList.filter(function (x) { return x.DISTRICT_ENG.toString() === $scope.PLDB_OTSSchemeData.DistrictName })[0].DISTRICT_ID;
                        //$scope.PLDB.DistrictName = $scope.DistrictList.filter(function (x) { return x.DISTRICT_ID.toString() === $scope.PLDB.DistrictId })[0].DISTRICT_ID;
                       
                        //$scope.PLDB.IsRural = 'false';
                   
                        //$scope.RuralUrbanBlock(false);                   

                    }
                }
                else {
                    $scope.PLDB.DistrictId = $scope.PLDB ? $scope.PLDB.DistrictId.toString() : '0';
                }
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });


    };

    $scope.GetDistrictList();

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
                data.forEach(function (item) {
                    if (item.OtsPendingLevelId == 6) {
                        item.Name = 'Mortgage Done';
                    }
                });
                $scope.MLPendingLevelList = data;               
            }).
            error(function (data, status) {
                $scope.MLPendingLevelList = [];
            });
    };

    //$scope.RuralUrbanBlock = function (isRural) {

    //    isRural = (isRural === true);

    //    if (isRural) {
    //        $scope.PLDB.BlockId = "0";
    //        $scope.PLDB.GpId = "0";
    //        $scope.PLDB.VId = "0";
    //        $scope.UrbanDiv = true;
    //        $scope.RuralDiv = false;
    //        $scope.BlockListByOffAdd(1);
    //        $scope.getAssembly();
    //    }
    //    else {
    //        $scope.PLDB.CityTypes = "0";
    //        $scope.PLDB.CITY_ID = "0";
    //        $scope.PLDB.WARD_ID = "0";
    //        $scope.UrbanDiv = false;
    //        $scope.RuralDiv = true;
    //        $scope.GetCityListByCityTypes(1);
    //        $scope.getAssembly();
    //    }
    //    $scope.TehsilList();
    //    $scope.getAssembly();
    //};


    $scope.RuralUrbanBlock = function (isRural) {

        isRural = (isRural === true);

        if (isRural) {
            $scope.PLDB.BlockId = "0";
            $scope.PLDB.GpId = "0";
            $scope.PLDB.VId = "0";

            $scope.PLDB.BlockName = "";
            $scope.PLDB.GpName = "";
            $scope.PLDB.VName = "";

            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;

            $scope.BlockListByOffAdd(1);
        }
        else {
            $scope.PLDB.CITY_ID = "0";
            $scope.PLDB.WARD_ID = "0";

            $scope.PLDB.CityName = "";
            $scope.PLDB.WardName = "";

            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;

            $scope.GetCityListByCityTypes(1);
        }

        $scope.TehsilList();
        $scope.getAssembly();
    };

    $scope.$watch('PLDB.DistrictId', function (newVal, oldVal) {
        if (!newVal || newVal === '0') return;
       
        if ($scope.isEditLoading) return;       
        if ($scope.PLDB.IsRural === true) {
           
            $scope.GetCityListByCityTypes(1);
        } else {
            $scope.GramPanchayatList(1);
            $scope.VillageList(1);
            $scope.BlockListByOffAdd(1);
        }

        $scope.TehsilList();
        $scope.getAssembly();
    });

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

    //start : Block List Data
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
    //End : Block List Data

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
                        $scope.PLDB.GpId = $scope.PLDB ? $scope.PLDB.GpId.toString() : '0';
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

    //End Ward List

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
                        $scope.PLDB.VId = $scope.PLDB ? $scope.PLDB.VId.toString() : '0';
                    }
                }).
                error(function (data, status) {
                    $scope.Villages = [];
                });
        }

    };

    $scope.DateFormat = function (date) {
        if (!date) return null;
        let d = new Date(date);

        let year = d.getFullYear();
        let month = ('0' + (d.getMonth() + 1)).slice(-2);
        let day = ('0' + d.getDate()).slice(-2);

        return `${year}-${month}-${day}`; 
    };

    $scope.GetCategoryName = function (id) {
        switch (parseInt(id)) {
            case 1: return "General";
            case 2: return "SC";
            case 3: return "ST";
            case 4: return "OBC";
            case 5: return "Minority";
            default: return "";
        }
    };


    $scope.Save_PLDBForm = function (StatusId) {  
        if (!$scope.PLDB.ApplicantName || !$scope.PLDB.FatherHusbandName || !$scope.PLDB.Gender || !$scope.PLDB.MobileNo ||  !$scope.PLDB.LoanAccNo) {           
            alert("Please fill the required fields");
            return;
        }  

        if ($scope.PLDB.CategoryId === "0") {
            alert("Please select the Category");
            return;    
        }

        if ($scope.PLDB.DistrictId === "0") {
            alert("Please select the District");
            return;  
        }      

        if ($scope.PLDB.PLDB_Id === "0") {
            alert("Please select the PLDB");
            return;  
        }      

        if ($scope.PLDB.PLDB_BranchId === "0") {
            alert("Please select the PLDB Branch");
            return;
        }       


        if (!$scope.PLDB.Sector || $scope.PLDB.Sector.trim() === "") {
            alert("Please enter the Sector");
            return;
        }

        if (!$scope.PLDB.Purpose || $scope.PLDB.Purpose.trim() === "") {
            alert("Please enter the Purpose");
            return;
        }

        if ($scope.PLDB.IsRural === null || $scope.PLDB.IsRural === undefined) { 
            alert("Please select Urban / Rural");
            return;
        }

        $(".se-pre-con").fadeIn("slow");
       
        $scope.PLDB.CategoryName = $scope.GetCategoryName($scope.PLDB.CategoryId);        
        $scope.PLDB.DistrictName = $scope.GetNameFromList($scope.DistrictList, $scope.PLDB.DistrictId, 'District');
        $scope.PLDB.PLDBName = ($scope.GetNameFromList($scope.PLDBList, $scope.PLDB.PLDB_Id, 'PLDB') || "").toUpperCase();
        $scope.PLDB.PLDBBranch = ($scope.GetNameFromList($scope.PLDBBranches, $scope.PLDB.PLDB_BranchId, 'PLDBBranch') || "").toUpperCase();
      
        $scope.PLDB.MutationDate = $scope.DateFormat(($scope.PLDB.MutationDate));
        $scope.PLDB.ChequeDate = $scope.DateFormat(($scope.PLDB.ChequeDate));
        $scope.PLDB.ActionTakenDate = $scope.DateFormat(($scope.PLDB.ActionTakenDate));
        $scope.PLDB.LastHearingDate = $scope.DateFormat(($scope.PLDB.LastHearingDate));
        $scope.PLDB.NextHearingDate = $scope.DateFormat(($scope.PLDB.NextHearingDate));
        $scope.PLDB.Notice99Date = $scope.DateFormat(($scope.PLDB.Notice99Date));
        $scope.PLDB.Order99Date = $scope.DateFormat(($scope.PLDB.Order99Date));
        $scope.PLDB.Order100Date = $scope.DateFormat(($scope.PLDB.Order100Date));
        $scope.PLDB.LandAuctionDate = $scope.DateFormat(($scope.PLDB.LandAuctionDate));
        $scope.PLDB.ChequeDate = $scope.DateFormat(($scope.PLDB.ChequeDate));
        $scope.PLDB.DateOfFirstDisbursement = $scope.DateFormat(($scope.PLDB.DateOfFirstDisbursement));
        $scope.PLDB.DateOfSanction = $scope.DateFormat(($scope.PLDB.DateOfSanction));
        $scope.PLDB.DateOfOD = $scope.DateFormat(($scope.PLDB.DateOfOD));       

        $scope.PLDB.IsNotice99 = $scope.PLDB.IsNotice99 == 1 ? 1 : 0;
        $scope.PLDB.IsOrder99 = $scope.PLDB.IsOrder99 == 1 ? 1 : 0;
        $scope.PLDB.IsNotice100 = $scope.PLDB.IsNotice100 == 1 ? 1 : 0;
        $scope.PLDB.IsLandAuction = $scope.PLDB.IsLandAuction == 1 ? 1 : 0;
        $scope.PLDB.IsMutation = $scope.PLDB.IsMutation == 1 ? 1 : 0;      
        $scope.PLDB.IsRural = $scope.PLDB.IsRural ? 1 : 0;
        $scope.PLDB.IsDeathCase = $scope.PLDB.IsDeathCase == 1 ? 1 : 0;      

        $scope.PLDB.IsChequesCollectedFromLoanee = $scope.PLDB.IsChequesCollectedFromLoanee == 1 ? 1 : 0;
        $scope.PLDB.IsChequesPresentedBankForClearing = $scope.PLDB.IsChequesPresentedBankForClearing == 1 ? 1 : 0;
        $scope.PLDB.IsChequeDishonoured = $scope.PLDB.IsChequeDishonoured == 1 ? 1 : 0;
        $scope.PLDB.IsChequeDishonourActionTaken = $scope.PLDB.IsChequeDishonourActionTaken == 1 ? 1 : 0; 

        //console.log("PLDB", $scope.PLDB);

        $http.post(SiteUrl + "/PLDB/Save_PldbSummaryODApplicationForm", $scope.PLDB).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data > 0) {

                    if ($scope.PLDB.PLDB_OTS_SchemeID > 0) {
                        alert('Data Updated Successfully');
                    } else {
                        alert('Data Saved Successfully');
                    }                   
                    window.location.reload();
                }
                else {
                    ShowModelAlert($scope._language == 'en-us' ? 'OTS 2025-26 Scheme :: ' : 'OTS 2025-26 Scheme ::', 'An Error Occurred Please Try Again Later');
                }

            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });

    }


    $scope.GetFormEditData = function (PldbOtsId) {

        $http.post(SiteUrl + "/PLDB/GetSubsidySummaryListApplicationFormViewData/?PldbOtsSchemeId=" + PldbOtsId).
            success(function (data, status) {
                $scope.PLDB_OTSSchemeData = JSON.parse(data.DataList)[0];
                $scope.PLDB = $scope.PLDB_OTSSchemeData;

                console.log("$scope.PLDB", $scope.PLDB);


                $scope.isEditLoading = true;
                $scope.LoadMaster();
                $scope.GetDistrictList(0);  
                $scope.TehsilList();
                $scope.getAssembly();
                $scope.GetPendingLevelList();
                $scope.GetMLPendingLevelList();

                $timeout(function () {
                    $scope.isEditLoading = false;
                    $scope.PLDB.CITY_ID = $scope.PLDB.CITY_ID + "";
                    $scope.PLDB.WARD_ID = $scope.PLDB.WARD_ID + "";
                    $scope.PLDB.PLDB_Id = $scope.PLDB.PLDB_Id.toString();
                    $scope.PLDB.PLDB_BranchId = $scope.PLDB.PLDB_BranchId.toString();                   
                    $scope.PLDBBranchList($scope.PLDB.PLDB_BranchId);                  
                    $scope.PLDB.Purpose = $scope.PLDB.Purpose + "";                          
                    $scope.PLDB.IsRural = ($scope.PLDB.IsRural == 1 || $scope.PLDB.IsRural === "1" || $scope.PLDB.IsRural === true || $scope.PLDB.IsRural === "true");                    
                    $scope.RuralUrbanBlock($scope.PLDB.IsRural);               
                    $scope.PLDB.RecoveryAmt = $scope.PLDB.RecoveryAmt;
                    $scope.PLDB.ChequeAmt = $scope.PLDB.ChequeAmt;                  
                    $scope.PLDB.IsMutation = ($scope.PLDB.IsMutation + "");                   
                    $scope.PLDB.BlockId = $scope.PLDB.BlockId.toString();
                    $scope.BlockListByOffAdd(1);                          
                    $scope.PLDB.DateOfFirstDisbursement = $scope.PLDB.DateOfFirstDisbursement;                    
                }, 0);
                              
                $scope.PLDB.IsRural = $scope.PLDB.IsRural == "1" ? true : false;
                $scope.PLDB.IsMutation = $scope.PLDB.IsMutation == "1" ? '1' : '0';    
                $scope.PLDB.RecoveryAmt = Number($scope.PLDB.RecoveryAmt) || 0;
                $scope.PLDB.ChequeAmt = $scope.PLDB.ChequeAmt;
                $scope.PLDB.DateOfSanction = $scope.PLDB.DateOfSanction;
                $scope.PLDB.GpId = $scope.PLDB.GpId.toString();
                $scope.PLDB.VId = $scope.PLDB.VId.toString();
                $scope.GramPanchayatList(1);
                $scope.VillageList(1);
                $scope.GetCityListByCityTypes(1);
                $scope.GetWardList(1);           
                $scope.LoadPurposeByShemeId($scope.PLDB.Purpose);         
                $scope.PLDB.PendingLevelId = $scope.PLDB.PendingLevelId + "";
                $scope.PLDB.LandMortgagedPendingLevelId = $scope.PLDB.LandMortgagedPendingLevelId + "";
                if ($scope.PLDB.MutationDate != null)
                    $scope.PLDB.MutationDate = new Date($scope.PLDB.MutationDate);
                if ($scope.PLDB.LandMortgagedMutationDate != null)
                    $scope.PLDB.LandMortgagedMutationDate = new Date($scope.PLDB.LandMortgagedMutationDate);

                if ($scope.PLDB.LandMortgagedland) {
                    $scope.PLDB.LandMortgagedland = parseFloat($scope.PLDB.LandMortgagedland);
                }
              
                $scope.IsShowPLDB = true;                  

                $scope.GetSchemeMasterBySector($scope.PLDB.SectorId);             

                if ($scope.PLDB.ActionTakenDate != null)
                    $scope.PLDB.ActionTakenDate = new Date($scope.PLDB.ActionTakenDate);

                if ($scope.PLDB.LastHearingDate != null)
                    $scope.PLDB.LastHearingDate = new Date($scope.PLDB.LastHearingDate);

                if ($scope.PLDB.NextHearingDate != null)
                    $scope.PLDB.NextHearingDate = new Date($scope.PLDB.NextHearingDate);

                if ($scope.PLDB.Notice99Date != null)
                    $scope.PLDB.Notice99Date = new Date($scope.PLDB.Notice99Date);

                if ($scope.PLDB.Order99Date != null)
                    $scope.PLDB.Order99Date = new Date($scope.PLDB.Order99Date);

                if ($scope.PLDB.Order100Date != null)
                    $scope.PLDB.Order100Date = new Date($scope.PLDB.Order100Date);

                if ($scope.PLDB.LandAuctionDate != null)
                    $scope.PLDB.LandAuctionDate = new Date($scope.PLDB.LandAuctionDate);
               
                if ($scope.PLDB.ChequeDate) {
                    var parts = $scope.PLDB.ChequeDate.split("/"); 
                    $scope.PLDB.ChequeDate = new Date(parts[2], parts[1] - 1, parts[0]); 
                }

                if ($scope.PLDB.DateOfFirstDisbursement) {
                    var parts = $scope.PLDB.DateOfFirstDisbursement.split("/"); 
                    $scope.PLDB.DateOfFirstDisbursement = new Date(parts[2], parts[1] - 1, parts[0]); 
                }

                if ($scope.PLDB.DateOfSanction) {
                    var parts = $scope.PLDB.DateOfSanction.split("/"); 
                    $scope.PLDB.DateOfSanction = new Date(parts[2], parts[1] - 1, parts[0]); 
                }               

                if ($scope.PLDB.DateOfOD) {
                    var parts = $scope.PLDB.DateOfOD.split("/"); 
                    $scope.PLDB.DateOfOD = new Date(parts[2], parts[1] - 1, parts[0]); 
                }

                $scope.PLDB.IsNotice99 = $scope.PLDB.IsNotice99 == "1" ? '1' : '0';
                $scope.PLDB.IsOrder99 = $scope.PLDB.IsOrder99 == "1" ? '1' : '0';
                $scope.PLDB.IsNotice100 = $scope.PLDB.IsNotice100 == "1" ? '1' : '0';
                $scope.PLDB.IsLandAuction = $scope.PLDB.IsLandAuction == "1" ? '1' : '0';             
                $scope.PLDB.IsDeathCase = $scope.PLDB.IsDeathCase == "1" ? '1' : '0';
                $scope.LoadSector();
            }).
            error(function (data, status) {
                $scope.PLDB_OTSSchemeData = [];
            });
    }


    $scope.LoadPurposeByShemeId = function (ShemeId, index) {

        //var sector = $scope.LoanSectorList.find(s => s.sector_id == $scope.PLDB.SectorId);
        //var schemeId = sector ? sector.scheme_id : null;
        
        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetPurposeMaster", { SchemeId: $scope.PLDB.SectorId }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PurposeOfLoanList = [];
                $scope.PurposeOfLoanList = data;
              //  $scope.LoanAmountDetails[index].PurposeOfLoanList = data;
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
               // $scope.LoanAmountDetails[index].LoanSchemeList = data;

                //if ($scope.LoanAmountDetails[0].SectorId == "2") {
                //    $scope.IsShowGarantor = false;
                //} else {
                //    $scope.IsShowGarantor = true;
                //}

                //    $scope.LoanAmountDetails[0].LoanSchemeList = $scope.LoanSchemeList.filter(function (x) { return x.scheme_id.toString() === $scope.SchemeUrlId });
                //}
                //$scope.LoanAmountDetails[0].SchemeId = $scope.SchemeUrlId.toString();
                //$scope.LoadSector($scope.LoanAmountDetails[0].SchemeId, '1');
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                alert("Error while fetching Society Details.");
                //$(".blockUI").fadeOut("slow");
            });
    }

    $scope.BackToPage = function () {
        window.location = '../PLDB/PLDBSummaryODApplicationList';
    }

    $scope.TehsilList = function () {
        if (!$scope.PLDB.DistrictId) {
            $scope.Tehsil = [];
            return;
        }

        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=tehsil&MasterCode=" + $scope.PLDB.DistrictId
        ).success(function (data) {
            $scope.Tehsil = data; 
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
                //error(function (data, status) {
                $scope.AssemblyList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    }

    // if ($scope.PldbOtsEncId != '') { $scope.GetFormEditData($scope.PldbOtsEncId); }

    $scope.$watchGroup(['PLDB.Principal', 'PLDB.OtherCharges', 'PLDB.IntSubvention'], function () {
        $scope.PLDB.ReccoveryTotalAmount =
            (parseFloat($scope.PLDB.Principal) || 0) +
            (parseFloat($scope.PLDB.OtherCharges) || 0) +
            (parseFloat($scope.PLDB.IntSubvention) || 0);
    });

    //$scope.loadAddressChain = function () {
    //    $scope.BlockListByOffAdd(1).then(function () {
    //        $timeout(function () {
    //            $scope.PLDB.BlockId = String($scope.PLDB.BlockId);
    //        });
    //        // GP
    //        return $scope.GramPanchayatList(1);
    //    })
    //        .then(function () {
    //            $timeout(function () {
    //                $scope.PLDB.GpId = String($scope.PLDB.GpId);
    //            });
    //            // VILLAGE
    //            return $scope.VillageList(1);
    //        })
    //        .then(function () {
    //            $timeout(function () {
    //                $scope.PLDB.VId = String($scope.PLDB.VId);
    //            });

    //        });
    //};

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
            // Safely convert each to number; treat blank/null as 0
            var principal = parseFloat($scope.PLDB.ODPrincipal) || 0;
            var premium = parseFloat($scope.PLDB.ODInsurancePremium) || 0;
            var interest = parseFloat($scope.PLDB.ODInterest) || 0;
            var penal = parseFloat($scope.PLDB.ODPenalInterast) || 0;

            var CDPrincipal = parseFloat($scope.PLDB.CDPrincipal) || 0;
            var CDInterest = parseFloat($scope.PLDB.CDInterest) || 0;
            var CDPenalInterest = parseFloat($scope.PLDB.CDPenalInterest) || 0;

            // Calculate total and round to 2 decimals
            $scope.PLDB.ODTotal = (principal + premium + interest + penal).toFixed(2);
            $scope.PLDB.CDTotal = (CDPrincipal + CDInterest + CDPenalInterest ).toFixed(2);
        }
    );

    $scope.GetSchemeMasterBySector();
    $scope.LoadSector();
    $scope.GetPendingLevelList();
    $scope.GetMLPendingLevelList();

    $scope.onload();

});
