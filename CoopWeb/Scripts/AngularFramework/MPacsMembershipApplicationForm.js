var PLDBMemberRegistrtionModule = angular.module('mPacsMod', ['angularUtils.directives.dirPagination']);
PLDBMemberRegistrtionModule.controller('mPacsCnt', function ($scope, $http, $rootScope) {
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.Filter = {};
    $scope.DivSms = true;
    $scope.UrbanDiv = false;
    $scope.RuralDiv = true;
    $scope.PresentUrbanDiv = false;
    $scope.PresentRuralDiv = true;
    $scope.newdiv = "";
    $scope.janAadharData = {};
    $scope.mPacs = {};
    $scope.mPacsView = {};
    $scope.ShowJanAadharOTPValidation = false;
    $scope.ShowMPacsForm = false;
    $scope.divViewShow = false;
    $scope.divGrid = true;
    $scope.DistrictList = [];
    $scope.CityList = [];
    $scope.Blocksdata = [];
    $scope.GramPanchayats = [];
    $scope.Villages = [];
    $scope.Ward = [];
    $scope.PresentCityList = [];
    $scope.PresentBlocksdata = [];
    $scope.PresentGramPanchayats = [];
    $scope.PresentVillages = [];
    $scope.PresentWard = [];
    $scope.EducationQualification = [];
    $scope.BankList = [];
    $scope.BranchList = [];
    $scope.PacsList = [];
    $scope.mPacsDataList = [];


    // on page load


    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json?id=3").
                success(function (data, status) {

                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json?id=3").
                success(function (data, status) {

                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }

        $scope.GetDistrictWithoutDivisionList();
        $scope.GetCcbList();
        $scope.GetCategoryList();
        $scope.GetQualificationList();
        $scope.GetBloodRelationList();
        $scope.RuralUrbanBlock(1);
        $scope.PresentRuralUrbanBlock(1);
    };
    $scope.filterByAge = function (item) {
        return item.Age >= 18;
    };
    $scope.ClearSearch = function () {
        $scope.Filter.SearchText = null;
        $scope.GetMPacsApplicationDetailBySSO();
    };

    $scope.PreviewApplication = function () {
        $scope.divViewShow = true;

        $scope.mPacsView = { ...$scope.mPacs };
        if ($scope.BloodRelation.filter(x => x.Id == $scope.mPacs.NomineeRelationId).length > 0) {
            $scope.mPacsView.NomineeRelation = $scope.BloodRelation.filter(x => x.Id == $scope.mPacs.NomineeRelationId)[0].RelationEng;
        }
        if ($scope.EducationQualification.filter(x => x.QualificationId == $scope.mPacs.EducationalQualificationId).length > 0) {
            $scope.mPacsView.Qualification = $scope.EducationQualification.filter(x => x.QualificationId == $scope.mPacs.EducationalQualificationId)[0].QualificationEng;
        }
        if ($scope.CategoryList.filter(x => x.CODEID == $scope.mPacs.CategoryId).length > 0) {
            $scope.mPacsView.Category = $scope.CategoryList.filter(x => x.CODEID == $scope.mPacs.CategoryId)[0].CODENAME;
        }
        if ($scope.DistrictList.filter(x => x.DISTRICT_ID == $scope.mPacs.DistrictId).length > 0) {
            $scope.mPacsView.DistrictName = $scope.DistrictList.filter(x => x.DISTRICT_ID == $scope.mPacs.DistrictId)[0].DISTRICT_ENG;
        }

        if ($scope.mPacs.IsRural) {

            if ($scope.Blocksdata.filter(x => x.BLOCK_ID == $scope.mPacs.BlockId).length > 0) {
                $scope.mPacsView.BlockName = $scope.Blocksdata.filter(x => x.BLOCK_ID == $scope.mPacs.BlockId)[0].BLOCK_ENG;
            }
            if ($scope.GramPanchayats.filter(x => x.GP_ID == $scope.mPacs.GpId).length > 0) {
                $scope.mPacsView.GpName = $scope.GramPanchayats.filter(x => x.GP_ID == $scope.mPacs.GpId)[0].GP_ENG;
            }
            if ($scope.Villages.filter(x => x.VILLAGE_ID == $scope.mPacs.VillageId).length > 0) {
                $scope.mPacsView.VillageName = $scope.Villages.filter(x => x.VILLAGE_ID == $scope.mPacs.VillageId)[0].VILLAGE_ENG;
            }
        }
        else {
            if ($scope.CityList.filter(x => x.CITY_ID == $scope.mPacs.CityId).length > 0) {
                $scope.mPacsView.CityName = $scope.CityList.filter(x => x.CITY_ID == $scope.mPacs.CityId)[0].CITY_ENG;
            }
            if ($scope.Ward.filter(x => x.WARD_ID == $scope.mPacs.WardId).length > 0) {
                $scope.mPacsView.WardName = $scope.Ward.filter(x => x.WARD_ID == $scope.mPacs.WardId)[0].WARD_ENG;
            }
        }

        if ($scope.DistrictList.filter(x => x.DISTRICT_ID == $scope.mPacs.PresentDistrictId).length > 0) {
            $scope.mPacsView.PresentDistrictName = $scope.DistrictList.filter(x => x.DISTRICT_ID == $scope.mPacs.PresentDistrictId)[0].DISTRICT_ENG;
        }
        if ($scope.mPacs.PresentIsRural) {

            if ($scope.PresentBlocksdata.filter(x => x.BLOCK_ID == $scope.mPacs.PresentBlockId).length > 0) {
                $scope.mPacsView.PresentBlockName = $scope.PresentBlocksdata.filter(x => x.BLOCK_ID == $scope.mPacs.PresentBlockId)[0].BLOCK_ENG;
            }
            if ($scope.PresentGramPanchayats.filter(x => x.GP_ID == $scope.mPacs.PresentGpId).length > 0) {
                $scope.mPacsView.PresentGpName = $scope.PresentGramPanchayats.filter(x => x.GP_ID == $scope.mPacs.PresentGpId)[0].GP_ENG;
            }
            if ($scope.PresentVillages.filter(x => x.VILLAGE_ID == $scope.mPacs.PresentVillageId).length > 0) {
                $scope.mPacsView.PresentVillageName = $scope.PresentVillages.filter(x => x.VILLAGE_ID == $scope.mPacs.PresentVillageId)[0].VILLAGE_ENG;
            }
        }
        else {
            if ($scope.PresentCityList.filter(x => x.CITY_ID == $scope.mPacs.PresentCityId).length > 0) {
                $scope.mPacsView.PresentCityName = $scope.PresentCityList.filter(x => x.CITY_ID == $scope.mPacs.PresentCityId)[0].CITY_ENG;
            }
            if ($scope.PresentWard.filter(x => x.WARD_ID == $scope.mPacs.PresentWardId).length > 0) {
                $scope.mPacsView.PresentWardName = $scope.PresentWard.filter(x => x.WARD_ID == $scope.mPacs.PresentWardId)[0].WARD_ENG;
            }
        }
        if ($scope.DistrictList.filter(x => x.DISTRICT_ID == $scope.mPacs.MPacsDistrictId).length > 0) {
            $scope.mPacsView.MPacsDistrictName = $scope.DistrictList.filter(x => x.DISTRICT_ID == $scope.mPacs.MPacsDistrictId)[0].DISTRICT_ENG;
        }
        if ($scope.BankList.filter(x => x.CCBId == $scope.mPacs.MPacsBankId).length > 0) {
            $scope.mPacsView.BankName = $scope.BankList.filter(x => x.CCBId == $scope.mPacs.MPacsBankId)[0].CCBNameEn;
        }
        if ($scope.BranchList.filter(x => x.Branch_ID == $scope.mPacs.MPacsBranchId).length > 0) {
            $scope.mPacsView.BranchName = $scope.BranchList.filter(x => x.Branch_ID == $scope.mPacs.MPacsBranchId)[0].BranchNameEn;
        }
        if ($scope.PacsList.filter(x => x.PACS_ID == $scope.mPacs.MPacsPacsId).length > 0) {
            $scope.mPacsView.PacsName = $scope.PacsList.filter(x => x.PACS_ID == $scope.mPacs.MPacsPacsId)[0].PACSNameEn;
        }
        else {
            $scope.mPacsView.PacsName = $scope.mPacs.OtherPacsName;
        }
    }

    $scope.ViewMemberApplication = function (encId) {
        $scope.GetMPacsApplicationDetailById(encId);
    }
    $scope.ShowForm = function () {
        $scope.divGrid = false;
        $scope.ShowJanAadharOTPValidation = true;
    }

    $scope.GetCategoryList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=-1").
            success(function (data, status) {
                $scope.CategoryList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });
    }
    $scope.GetQualificationList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=EDUCATIONQUALIFICATION&MasterCode=-1").
            success(function (data, status) {
                $scope.EducationQualification = data;
            }).
            error(function (data, status) {
                $scope.EducationQualification = [];
            });
    }
    $scope.GetBloodRelationList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=RELATION&MasterCode=-1").
            success(function (data, status) {
                $scope.BloodRelation = data;
            }).
            error(function (data, status) {
                $scope.BloodRelation = [];
            });
    }
    $scope.IsAddressSameAsJanAadhar_Changed = function (event) {
        if ($scope.mPacs.IsAddressSameAsJanAadhar == true) {
            $scope.mPacs.IsRural = $scope.janAadharData.IsRural;

            if ($scope.DistrictList.filter(x => x.DISTRICT_CODE == $scope.janAadharData.District_Code).length > 0) {
                $scope.mPacs.DistrictId = $scope.DistrictList.filter(x => x.DISTRICT_CODE == $scope.janAadharData.District_Code)[0].DISTRICT_ID;
                $scope.RuralUrbanBlock(($scope.mPacs.IsRural == true ? 1 : 0));
            }
            $scope.mPacs.Address = $scope.janAadharData.Address;
            $scope.mPacs.PinCode = $scope.janAadharData.PinCode;
        }
        else {
            $scope.mPacs.DistrictId = "0";
            $scope.mPacs.CityId = "0";
            $scope.mPacs.WardId = "0";
            $scope.mPacs.BlockId = "0";
            $scope.mPacs.GpId = "0";
            $scope.mPacs.VillageId = "0";
            $scope.mPacs.Address = "";
            $scope.mPacs.PinCode = "";
            $scope.mPacs.IsRural = true;
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
        }

        return true;
    }

    $scope.IsAddressSameAsPermanent_Changed = function () {
        if ($scope.mPacs.IsAddressSameAsPermanent == true) {

            $scope.mPacs.PresentIsRural = $scope.mPacs.IsRural;
            $scope.mPacs.PresentDistrictId = $scope.mPacs.DistrictId;

            $scope.PresentBlocksdata = $scope.Blocksdata;
            $scope.PresentGramPanchayats = $scope.GramPanchayats;
            $scope.PresentVillages = $scope.Villages;
            $scope.PresentCityList = $scope.CityList;
            $scope.PresentWard = $scope.Ward;

            if ($scope.mPacs.PresentIsRural == true) {
                $scope.PresentUrbanDiv = false;
                $scope.PresentRuralDiv = true;
                $scope.mPacs.PresentBlockId = $scope.mPacs.BlockId;
                $scope.mPacs.PresentGpId = $scope.mPacs.GpId;
                $scope.mPacs.PresentVillageId = $scope.mPacs.VillageId;
            } else {
                $scope.PresentUrbanDiv = true;
                $scope.PresentRuralDiv = false;
                $scope.mPacs.PresentCityId = $scope.mPacs.CityId;
                $scope.mPacs.PresentWardId = $scope.mPacs.WardId;
            }
            $scope.mPacs.PresentHouseBuildingNo = $scope.mPacs.HouseBuildingNo;
            $scope.mPacs.PresentLaneStreetName = $scope.mPacs.LaneStreetName;
            $scope.mPacs.PresentLocality = $scope.mPacs.Locality;
            $scope.mPacs.PresentAddress = $scope.mPacs.Address;
            $scope.mPacs.PresentPinCode = $scope.mPacs.PinCode;
        }
        else {
            $scope.mPacs.PresentDistrictId = "0";
            $scope.mPacs.PresentCityId = "0";
            $scope.mPacs.PresentWardId = "0";
            $scope.mPacs.PresentBlockId = "0";
            $scope.mPacs.PresentGpId = "0";
            $scope.mPacs.PresentVillageId = "0";
            $scope.mPacs.PresentHouseBuildingNo = "";
            $scope.mPacs.PresentLaneStreetName = "";
            $scope.mPacs.PresentLocality = "";
            $scope.mPacs.PresentAddress = "";
            $scope.mPacs.PresentPinCode = "";
            $scope.mPacs.PresentIsRural = true;
            $scope.PresentUrbanDiv = false;
            $scope.PresentRuralDiv = true;
        }

        return true;
    }

    $scope.RuralUrbanBlock = function (isRural) {

        $scope.mPacs.CityId = "0";
        $scope.mPacs.WardId = "0";
        $scope.mPacs.BlockId = "0";
        $scope.mPacs.GpId = "0";
        $scope.mPacs.VillageId = "0";

        $scope.CityList = [];
        $scope.Blocksdata = [];
        $scope.GramPanchayats = [];
        $scope.Villages = [];
        $scope.Ward = [];
        if (isRural) {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
            $scope.BlockListByOffAdd();
        }
        else {
            $scope.GetCityList();
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;

        }
    };

    $scope.PresentRuralUrbanBlock = function (isRural) {

        $scope.mPacs.PresentCityId = "0";
        $scope.mPacs.PresentWardId = "0";
        $scope.mPacs.PresentBlockId = "0";
        $scope.mPacs.PresentGpId = "0";
        $scope.mPacs.PresentVillageId = "0";

        //$scope.PresentCityList = [];
        //$scope.PresentBlocksdata = [];
        //$scope.PresentGramPanchayats = [];
        //$scope.PresentVillages = [];
        //$scope.PresentWard = [];
        if (isRural) {
            $scope.PresentUrbanDiv = false;
            $scope.PresentRuralDiv = true;
            //$scope.PresentBlockListByOffAdd();
        }
        else {
            //$scope.GetPresentCityList();
            $scope.PresentUrbanDiv = true;
            $scope.PresentRuralDiv = false;
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
            $scope.CityList = [];
            $scope.Blocksdata = [];
            $scope.GramPanchayats = [];
            $scope.Villages = [];
            $scope.Ward = [];
            $scope.GetCityList();
            $scope.BlockListByOffAdd();

            $scope.mPacs.BlockId = "0";
            $scope.mPacs.GpId = "0";
            $scope.mPacs.VillageId = "0";
            $scope.mPacs.CityId = "0";
            $scope.mPacs.WardId = "0";
        }
    }
    $scope.PresentResetDataForAddress = function (val, Level) {
        $scope.mPacs.DistrictName = $scope.DistrictList.filter(x => x.DISTRICT_ID == $scope.mPacs.PresentDistrictId)[0].DISTRICT_ENG;
        if (val == 1) {
            $scope.PresentCityList = [];
            $scope.PresentBlocksdata = [];
            $scope.PresentGramPanchayats = [];
            $scope.PresentVillages = [];
            $scope.PresentWard = [];
            $scope.PresentBlockListByOffAdd();
            $scope.GetPresentCityList();

            $scope.mPacs.PresentBlockId = "0";
            $scope.mPacs.PresentGpId = "0";
            $scope.mPacs.PresentVillageId = "0";
            $scope.mPacs.PresentCityId = "0";
            $scope.mPacs.PresentWardId = "0";
        }
    }

    $scope.BlockListByOffAdd = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Blocksdata = [];
        $scope.GramPanchayats = [];
        $scope.Villages = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + ($scope.mPacs.DistrictId ?? 0)).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Blocksdata = data;
                if ($scope.mPacs.IsAddressSameAsJanAadhar)
                    if ($scope.Blocksdata.filter(x => x.BLOCK_CODE == $scope.janAadharData.Block_Code).length > 0) {
                        $scope.mPacs.BlockId = $scope.Blocksdata.filter(x => x.BLOCK_CODE == $scope.janAadharData.Block_Code)[0].BLOCK_ID;
                        $scope.GetGramPanchayatList();
                    }
                $scope.PresentBlocksdata = data;
            }).
            error(function (data, status) {
                $scope.Blocksdata = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.GetGramPanchayatList = function () {
        //$scope.mPacs.GpId = "0";
        //$scope.mPacs.VillageId = "0";

        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + ($scope.mPacs.BlockId ?? 0)).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.GramPanchayats = data;
                if ($scope.mPacs.IsAddressSameAsJanAadhar)
                    if ($scope.GramPanchayats.filter(x => x.GP_CODE == $scope.janAadharData.GP_Code).length > 0) {
                        $scope.mPacs.GpId = $scope.GramPanchayats.filter(x => x.GP_CODE == $scope.janAadharData.GP_Code)[0].GP_ID;
                        $scope.GetVillageList();
                    }
                $scope.PresentGramPanchayats = data;
            }).
            error(function (data, status) {
                $scope.GramPanchayats = [];
                $(".se-pre-con").fadeOut("slow");
            });

    };
    $scope.GetVillageList = function () {
        //$scope.mPacs.VillageId = "0";
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=VILLAGE&MasterCode=" + ($scope.mPacs.GpId ?? 0)).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Villages = data;
                if ($scope.mPacs.IsAddressSameAsJanAadhar)
                    if ($scope.Villages.filter(x => x.VILLAGE_CODE == $scope.janAadharData.Village_Code).length > 0)
                        $scope.mPacs.VillageId = $scope.Villages.filter(x => x.VILLAGE_CODE == $scope.janAadharData.Village_Code)[0].VILLAGE_ID;
                $scope.PresentVillages = data;
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Villages = [];
            });

    };
    $scope.GetCityList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITY&MasterCode=" + ($scope.mPacs.DistrictId ?? 0)).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.CityList = data;
                if ($scope.mPacs.IsAddressSameAsJanAadhar)
                    if ($scope.CityList.filter(x => x.CITY_CODE == $scope.janAadharData.City_Code).length > 0) {
                        $scope.mPacs.CityId = $scope.CityList.filter(x => x.CITY_CODE == $scope.janAadharData.City_Code)[0].CITY_ID;
                        $scope.GetWardList();
                    }
                $scope.PresentCityList = data;
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.CityList = [];
            });
    };
    $scope.GetWardList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + ($scope.mPacs.CityId ?? 0)).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Ward = data;
                $scope.PresentWard = data;
            }).
            error(function (data, status) {
                $scope.Ward = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    //Start Present Address data


    $scope.PresentBlockListByOffAdd = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.PresentBlocksdata = [];
        $scope.PresentGramPanchayats = [];
        $scope.PresentVillages = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + ($scope.mPacs.PresentDistrictId)).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.PresentBlocksdata = data;
            }).
            error(function (data, status) {
                $scope.PresentBlocksdata = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.GetPresentGramPanchayatList = function () {
        $scope.mPacs.PresentGpId = "0";
        $scope.mPacs.PresentVillageId = "0";

        $scope.mPacs.BlockName = $scope.PresentBlocksdata.filter(x => x.BLOCK_ID == $scope.mPacs.PresentBlockId)[0].BLOCK_ENG;
        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + ($scope.mPacs.PresentBlockId ?? 0)).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.PresentGramPanchayats = data;
            }).
            error(function (data, status) {
                $scope.PresentGramPanchayats = [];
                $(".se-pre-con").fadeOut("slow");
            });

    };
    $scope.GetPresentVillageList = function () {
        $scope.mPacs.PresentVillageId = "0";
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=VILLAGE&MasterCode=" + ($scope.mPacs.PresentGpId ?? 0)).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.PresentVillages = data;
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.PresentVillages = [];
            });

    };
    $scope.PresentVillageChange = function () {
        $scope.mPacs.VillageName = $scope.PresentVillages.filter(x => x.VILLAGE_ID == $scope.mPacs.PresentVillageId)[0].VILLAGE_ENG;
    };


    $scope.GetPresentCityList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITY&MasterCode=" + ($scope.mPacs.PresentDistrictId ?? 0)).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.PresentCityList = data;
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.PresentCityList = [];
            });
    };
    $scope.GetPresentWardList = function (id) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + ($scope.mPacs.PresentCityId ?? 0)).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.PresentWard = data;
            }).
            error(function (data, status) {
                $scope.PresentWard = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    //End Present Address data



    $scope.GetCcbList = function () {
        $scope.BranchList = [];
        $scope.PacsList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CCB_LIST&MasterCode=" + ($scope.mPacs.MPacsDistrictId??0)).
            success(function (data, status) {
                $scope.BankList = data;
            }).
            error(function (data, status) {
                $scope.BankList = [];
            });
    };
    $scope.GetCcbBranchList = function () {
        $scope.BranchList = [];
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CCB_BANK_LIST&MasterCode=" + $scope.mPacs.MPacsBankId).
            success(function (data, status) {
                $scope.BranchList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.BranchList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetPacsList = function () {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PACS_LIST&MasterCode=" + $scope.mPacs.MPacsBranchId).
            success(function (data, status) {
                $scope.PacsList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PacsList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.PacsChangeData = function () {
        if ($scope.mPacs.MPacsPacsId == -1)
            $scope.mPacs.PacsName = $scope.mPacs.OtherPacsName;
        else
            $scope.mPacs.PacsName = $scope.PacsList.filter(x => x.PACS_ID == $scope.mPacs.MPacsPacsId)[0].PACSNameEn;
    };



    $scope.GetJanAadharDetail = function () {
        if ($scope.JanaadharValidate()) {
            $(".se-pre-con").fadeIn("slow");
            $scope.JanMemberList = '';
           
                JanAadhaarAuth($scope.JanAadharNo, "", "GCC1", "", "", $http, false, "", "", function (data) {
                    /* JanAadhaarAuth("", "", "GCC", "420928155572","OTP", $http, false, "", "", function (data) {*/
                    console.log(data);
                    debugger;
                    if (data.length > 0) {
                        $scope.JanMemberList = data;
                        $scope.ShowFamilyMemberGrid = true;
                        $(".se-pre-con").fadeOut("slow");
                    }
                    else if (data.length == 0) {
                        //alert("This Jan Aadhaar No. is already registered  ");
                        alert("The entered Jan Aadhaar Number is either already registered or could not be found. Please verify and try again.");
                        $scope.ShowFamilyMemberGrid = false;
                        $(".se-pre-con").fadeOut("slow");

                    }
                    else {
                        alert("No data found");
                        $scope.ShowFamilyMemberGrid = false;
                        $(".se-pre-con").fadeOut("slow");
                    }

                });
           


            /*
            $http.post(SiteUrl + "/MPacsMembership/GetJanAadhaarMembersList?JanAadharNo=" + encodeURIComponent(encryptData($scope.JanAadharNo))).
                success(function (data, status) {

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
                }).
                error(function (data, status) {
                    $scope.ShowFamilyMemberGrid = false;
                    alert("Error while fetching Year Target Data");
                    $(".se-pre-con").fadeOut("slow");
                });

                */
        }
    }
    $scope.VerifyOTPViewPopup = function (mobileNo, Encrypted_JAN_MEMBER_ID, Encrypted_JAN_AADHAR, item) {
      //  $(".se-pre-con").fadeIn("slow");
     /*   var message = "";
        $(".se-pre-con").fadeOut("slow");
        $(".blockUI").fadeOut("slow");
        $scope.SendAndVerifyOTP = {};
        $scope.SendAndVerifyOTP.Masked_MOBILE_NO = mobileNo;
        $scope.SendAndVerifyOTP.Encrypted_JAN_MEMBER_ID = Encrypted_JAN_MEMBER_ID;
        $scope.SendAndVerifyOTP.Encrypted_JAN_AADHAR = Encrypted_JAN_AADHAR;

        $scope.SendAndVerifyOTP.tid = "";
        $("#modelPopupVerifyOTP").modal("show");

        if (message.length > 5) {
            ShowModelAlert(AlertPopUpHeading, message);
            return false;
        }
        */

        JanAadhaarAuth("", Encrypted_JAN_MEMBER_ID, "GCC1", "", "", $http, false, "", "", function (data) {
            /* JanAadhaarAuth("", "", "GCC", "420928155572","OTP", $http, false, "", "", function (data) {*/
            console.log(data);
            debugger;
            if (data && Object.keys(data).length > 0 ) {
               // $scope.SendAndVerifyOTP.tid = data;

                //if (data.Status == true)
                {
                    $scope.ShowJanAadharOTPValidation = false;
                    $scope.ShowMPacsForm = true;
                    $scope.janAadharData = data;
                    if ($scope.janAadharData) {

                        $scope.mPacs.Name = $scope.janAadharData.Name;
                        //$scope.mPacs.Address = $scope.janAadharData.Address;
                        $scope.mPacs.Age = $scope.janAadharData.Age;
                        if ($scope.CategoryList.filter(x => x.CODENAME == $scope.janAadharData.Category).length > 0)
                            $scope.mPacs.CategoryId = $scope.CategoryList.filter(x => x.CODENAME == $scope.janAadharData.Category)[0].CODEID;
                        $scope.mPacs.DOB = $scope.janAadharData.DOB;
                        $scope.mPacs.Encrypted_JanAadharMID = $scope.janAadharData.Encrypted_JanAadharMID;
                        $scope.mPacs.Encrypted_JanAadharNo = $scope.janAadharData.Encrypted_JanAadharNo;
                        $scope.mPacs.FatherName = $scope.janAadharData.FatherName;
                        $scope.mPacs.MotherName = $scope.janAadharData.MotherName;
                        $scope.mPacs.Gender = $scope.janAadharData.Gender;
                        $scope.mPacs.JanAadharEnrollmentId = $scope.janAadharData.JanAadharEnrollmentId;
                        $scope.mPacs.JanAadharNo = $scope.janAadharData.JanAadharNo;
                        $scope.mPacs.Photo_Base64 = $scope.janAadharData.Photo_Base64;
                        $scope.mPacs.MobileNo = $scope.janAadharData.MobileNo;
                        $scope.mPacs.NomineeRelationId = "0";
                        $scope.mPacs.EducationalQualificationId = "0";
                        $scope.mPacs.IsAddressSameAsJanAadhar = true;
                        $scope.mPacs.MPacsDistrictId = "0";
                        $scope.mPacs.MPacsBankId = "0";
                        $scope.mPacs.MPacsBranchId = "0";
                        $scope.mPacs.MPacsPacsId = "0";
                        $scope.mPacs.OtherPacsName = null;
                        $scope.IsAddressSameAsJanAadhar_Changed(1);
                    }
                    ShowModelAlert($scope._language == 'en-us' ? 'M.PACS Member Registration Form ' : 'M.PACS Member Registration Form', $scope._language == 'en-us' ? 'OTP has been successfully verified.' : 'ओटीपी सफलतापूर्वक सत्यापित हो गया है।');

                    $("#modelPopupVerifyOTP").modal("hide");
                }
                //else {
                //    ShowModelAlert($scope._language == 'en-us' ? 'M.PACS Member Registration Form ' : 'M.PACS Member Registration Form', $scope._language == 'en-us' ? 'OTP has not been validated' : 'OTP मान्य नहीं किया गया है');
                //}



                //$scope.ValidateJanAadharOTP();
       
                $(".se-pre-con").fadeOut("slow");
            }
            else if (data.length == 0) {
                //alert("This Jan Aadhaar No. is already registered  ");
                //ShowModelAlert(AlertPopUpHeading, message);
                ShowModelAlert($scope._language == 'en-us' ? 'M.PACS Member Registration Form ' : 'M.PACS Member Registration Form', $scope._language == 'en-us' ? 'OTP has not been validated' : 'OTP मान्य नहीं किया गया है');
                
                $(".se-pre-con").fadeOut("slow");

            }
            else {
                alert("No data found");              
                $(".se-pre-con").fadeOut("slow");
            }

        });




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
        $http.post(SiteUrl + "/MPacsMembership/GenrateJanAadharOTP", $scope.SendAndVerifyOTP).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.SendAndVerifyOTP.tid = data;
                }
                $scope.SetTimer();

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
            $http.post(SiteUrl + "/MPacsMembership/ValidateJanAadharOTP", $scope.SendAndVerifyOTP).
                success(function (data, status) {
                    if (data.Status == true) {
                        $scope.ShowJanAadharOTPValidation = false;
                        $scope.ShowMPacsForm = true;
                        $scope.janAadharData = data;
                        if ($scope.janAadharData) {

                            $scope.mPacs.Name = $scope.janAadharData.Name;
                            //$scope.mPacs.Address = $scope.janAadharData.Address;
                            $scope.mPacs.Age = $scope.janAadharData.Age;
                            if ($scope.CategoryList.filter(x => x.CODENAME == $scope.janAadharData.Category).length > 0)
                                $scope.mPacs.CategoryId = $scope.CategoryList.filter(x => x.CODENAME == $scope.janAadharData.Category)[0].CODEID;
                            $scope.mPacs.DOB = $scope.janAadharData.DOB;
                            $scope.mPacs.Encrypted_JanAadharMID = $scope.janAadharData.Encrypted_JanAadharMID;
                            $scope.mPacs.Encrypted_JanAadharNo = $scope.janAadharData.Encrypted_JanAadharNo;
                            $scope.mPacs.FatherName = $scope.janAadharData.FatherName;
                            $scope.mPacs.MotherName = $scope.janAadharData.MotherName;
                            $scope.mPacs.Gender = $scope.janAadharData.Gender;
                            $scope.mPacs.JanAadharEnrollmentId = $scope.janAadharData.JanAadharEnrollmentId;
                            $scope.mPacs.JanAadharNo = $scope.janAadharData.JanAadharNo;
                            $scope.mPacs.Photo_Base64 = $scope.janAadharData.Photo_Base64;
                            $scope.mPacs.MobileNo = $scope.janAadharData.MobileNo;
                            $scope.mPacs.NomineeRelationId = "0";
                            $scope.mPacs.EducationalQualificationId = "0";
                            $scope.mPacs.IsAddressSameAsJanAadhar = true;
                            $scope.mPacs.MPacsDistrictId = "0";
                            $scope.mPacs.MPacsBankId = "0";
                            $scope.mPacs.MPacsBranchId = "0";
                            $scope.mPacs.MPacsPacsId = "0";
                            $scope.mPacs.OtherPacsName = null;
                            $scope.IsAddressSameAsJanAadhar_Changed(1);
                        }
                        ShowModelAlert($scope._language == 'en-us' ? 'M.PACS Member Registration Form ' : 'M.PACS Member Registration Form', $scope._language == 'en-us' ? 'OTP has been successfully verified.' : 'ओटीपी सफलतापूर्वक सत्यापित हो गया है।');

                        $("#modelPopupVerifyOTP").modal("hide");
                    }
                    else {
                        ShowModelAlert($scope._language == 'en-us' ? 'M.PACS Member Registration Form ' : 'M.PACS Member Registration Form', $scope._language == 'en-us' ? 'OTP has not been validated' : 'OTP मान्य नहीं किया गया है');
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
        var AlertPopUpHeading = $scope._language == 'en-us' ? 'JanAadhar OTP Validate' : 'जन आधार ओटीपी सत्यापित करें';
        var message = $scope._language == 'en-us' ? 'Validate  following' : 'निम्नलिखित मान्य करें';
        if ($scope.SendAndVerifyOTP.JanAadharOTP == undefined || $scope.SendAndVerifyOTP.JanAadharOTP == '') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter JanAadhar OTP' : '\n <br /> कृपया जनाधार ओटीपी दर्ज करें |';
        }

        if (message.length > 23) {
            ShowModelAlert(AlertPopUpHeading, message);
            return false;
        }
        return true;

    }


    $scope.JanaadharValidate = function () {
        var message = $scope._language == 'en-us' ? 'Validate  following' : 'निम्नलिखित मान्य करें';
        if ($scope.JanAadharNo == undefined || $scope.JanAadharNo == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Enter JanAadhar Number' : '\n <br /> कृपया जन आधार नंबर दर्ज करें |';
        }

        if (message.length > 23) {
            ShowModelAlert($scope._language == 'en-us' ? 'M.PACS Member Registration Form' : 'M.PACS Member Registration Form', message);
            return false;
        }
        return true;
    }
    $scope.SaveMPacsMembershipData = function () {
        $http.post(SiteUrl + "/MPacsMembership/SaveMPacsMembershipData", $scope.mPacs).
            success(function (data, status) {
                if (data.Status) {
                    $scope.ShowJanAadharOTPValidation = false;
                    $scope.ShowMPacsForm = false;
                    $scope.mPacs = {};
                    $scope.divGrid = true;
                    ShowModelAlert($scope._language == 'en-us' ? 'M.PACS Member Registration ' : 'M.PACS Member Registration', $scope._language == 'en-us' ? 'Your application submitted successfully.' : 'आपका आवेदन सफलतापूर्वक सबमिट हो गया है।');
                    $scope.GetMPacsApplicationDetailBySSO();
                }
                else {
                    ShowModelAlert($scope._language == 'en-us' ? 'M.PACS Member Registration ' : 'M.PACS Member Registration', $scope._language == 'en-us' ? 'Some error occurred while submitting form.ome error occurred' : 'फॉर्म सबमिट करते समय कुछ त्रुटि हुई।');
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    }


    $scope.GetMPacsApplicationDetailBySSO = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/MPacsMembership/GetMPacsApplicationDetailBySSO", JSON.stringify({ SearchText: $scope.Filter.SearchText })).
            success(function (data, status) {
                $scope.mPacsDataList = JSON.parse(data.Result);
                $scope.divGrid = true;

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.mPacsDataList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetMPacsApplicationDetailById = function (encId) {
        $(".se-pre-con").fadeIn("slow");
        $scope.mPacsView = {};
        $http.get(SiteUrl + "/MPacsMembership/GetMPacsApplicationDetailByEncId?encId="+encId).
            success(function (data, status) {
                $scope.mPacsView = JSON.parse(data.Result)[0];
                $scope.divGrid = false;
                $scope.divViewShow = true;

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.mPacsDataList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.PrintApplication = function () {
    
        $("#buttons").hide();
        const element = document.getElementById("printpdf");
        const opt = {
            margin: 0.5,
            filename: 'Registrationform.pdf',
            image: { type: 'jpeg', quality: 0.98 },
            html2canvas: { scale: 2 },  // Higher scale = better quality
            jsPDF: { unit: 'mm', format: 'a4', orientation: 'landscape' }
        };
        html2pdf().set(opt).from(element).save().then(() => {
            setTimeout(() => {
                $("#buttons").show();
            }, 2000); 
        });
        
    }

    $scope.OnPageLoad();
    $scope.GetMPacsApplicationDetailBySSO();
});
