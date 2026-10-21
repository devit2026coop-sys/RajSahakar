
var MasterMod = angular.module('ncdMod', ['angularUtils.directives.dirPagination']);

MasterMod.controller("ncdCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.NCDStateDetails = [];
    $scope.NCDDistrictDetail = [];
    $scope.NCDDistrictDetails = [];
    $scope.NCDSectorDetails = [];
    $scope.NCDBlockDetails = [];
    $scope.NCDGramPanchayatDetails = [];
    $scope.NCDVillageDetails = [];
    $scope.NCDAuditCategories = [];
    $scope.NCDRegistrationAuthorities = [];
    $scope.NCDCoopBanks = [];
    $scope.SocietyFunctionalStatusList = [];
    $scope.NcdDesignationsList = [];
    $scope.FinancialYearList = [];
    $scope.FilteredNCDSectorDetails = [];
    $scope.NCDDistrictDetailsse = [];
    $scope.NCDBlockDetailsse = [];
    $scope.bank_type = []
    $scope.NCDFacilities = []
    $scope.sector_operation = "";
    $scope.NCDOfficeBuildingTypes = [];
    $scope.primary_activity = "";
    $scope.NCD_Statecode = '';
    $scope.bank_type = [];
    $scope.DistrictList = [];
    $scope.totalMembers = 0;


    $scope.sector = { operation_area_location: "2" };
    $scope.ncd = {
        operation_area_location: "2",
        water_body_type_id: "0"
    };
    if (!$scope.ncd.is_msme) {
        $scope.ncd.is_msme = "No";
    }

    $scope.societyItems = [
        { name: 'Seeds', hasLicense: false, sellsItem: false },
        { name: 'Food grains Procurement on MSP', hasLicense: false, sellsItem: false },
        { name: 'PDS Shop', hasLicense: false, sellsItem: false },
        { name: 'Fertilizer', hasLicense: false, sellsItem: false },
        { name: 'Pesticide', hasLicense: false, sellsItem: false },
        { name: 'Agricultural Equipment on Rent', hasLicense: false, sellsItem: false }
    ];

    $scope.PaidShareCalculate = function () {
        $scope.PaidUpTotal = 0;
        $scope.ncd.PaidUpTotal = $scope.ncd.paid_up.members + $scope.ncd.paid_up.government;
    }


    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get("/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get("/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }


        $scope.GetNCDStateDetail();
        $scope.GetDistrictByStatecode(8);
        $scope.GetBlockByStateDistrict();
        $scope.GetBlockByDistrict();
        /*$scope.GetGramPanchayatByDistrictBlock();*/
        $scope.GetGramPanchayatByStateDistrictBlock();
        $scope.GetVillageByStateDistrictBlockGramPanchayat();
        $scope.GetNCDAuditCategories();
        //$scope.GetNCDCooperativeSocietyBanks();
        $scope.GetNCDRegistrationAuthorities();
        $scope.GetSocietyFunctionalStatus();
        $scope.GetDistricts();
        $scope.GetBlocksByDistrict();
        $scope.GetGramPanchayatByDistrictBlockse();
        $scope.GetGramPanchayatByDistrictBlocksec();
        $scope.GetVillageByDistrictBlockGram();
        $scope.GetNcdDesignations();
        $scope.GetFinancialYears();
        $scope.filterBankType();
        $scope.GetBeeBeehiveTypes();
        $scope.GetBeeBeehiveTypes2();

        //Added by jaya

        $scope.loadDistrictList();

        //Consumer Cooperative Society Details
        $scope.GetFacilities();
        $scope.GetNcdOfficeBuildingTypes();
        $scope.filterPrimaryActivity();
        $scope.GetAreaOfOperations($scope.sector.operation_area_location);
    };

    $scope.BankTypes = [];
    $scope.BankTypes = [
        { value: 0, label: 'Cooperative Bank' },
        { value: 1, label: 'Commercial Bank' }
    ];


    $scope.GetNCDStateDetail = function () {

        $scope.NCDStateDetails = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetNCDStateDetails").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.NCDStateDetail != null) {
                    $scope.NCDStateDetails = JSON.parse(data.Result.Data.NCDStateDetail);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });

    };

    $scope.NCDSectorDetails = [];
    $scope.FilteredNCDSectorDetails = [];



    // called when Credit/Non-Credit changes
    $scope.filterPrimaryActivity = function (sid) {
        var sid = $scope.ncd.sector_of_operation_type;
        if (!sid) {
            $scope.FilteredNCDSectorDetails = [];
            return;
        }
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetSectorDetailsByIsCredit?actionName=GetSectorByCredit&sectorId=" + sid)
            .success(function (data) {
                if (data.Result.Data && data.Result.Data.NCDSectorDetail) {
                    $scope.FilteredNCDSectorDetails = JSON.parse(data.Result.Data.NCDSectorDetail);
                } else {
                    $scope.FilteredNCDSectorDetails = [];
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function () {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.GetDistrictByStatecode = function (statecode) {
        if (!statecode) {
            $scope.NCDDistrictDetails = [];
            return;
        }
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetDistrictwithStatecodeData?statecode=" + 8).
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.NCDDistrictDetail != null) {
                    $scope.NCDDistrictDetails = JSON.parse(data.Result.Data.NCDDistrictDetail);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    // Fetch blocks by state & district
    $scope.GetBlockByStateDistrict = function (statecode, districtcode) {
        if (!statecode || !districtcode) {
            $scope.NCDBlockDetails = [];
            return;
        }
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetBlockByStateDistrict?statecode=" + statecode + "&districtcode=" + districtcode)
            .success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.NCDBlockDetail != null) {
                    $scope.NCDBlockDetails = JSON.parse(data.Result.Data.NCDBlockDetail);
                }
                $(".se-pre-con").fadeOut("slow");
            })
            .error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetGramPanchayatByStateDistrictBlock = function (statecode, districtcode, blockcode) {
        if (!statecode || !districtcode || !blockcode) {
            $scope.NCDGramPanchayatDetails = [];
            return;
        }

        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetGramPanchayatByStateDistrictBlock?statecode=" + statecode + "&districtcode=" + districtcode + "&blockcode=" + blockcode)
            .success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.NCDGramPanchayatDetail != null) {
                    $scope.NCDGramPanchayatDetails = JSON.parse(data.Result.Data.NCDGramPanchayatDetail);
                }
                $(".se-pre-con").fadeOut("slow");
            })
            .error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetVillageByStateDistrictBlockGramPanchayat = function (statecode, districtcode, blockcode, grampanchayatcode) {

        if (!statecode || !districtcode || !blockcode || !grampanchayatcode) {
            $scope.NCDVillageDetails = [];
            return;
        }

        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetVillageByStateDistrictBlockGramPanchayat?statecode=" + statecode + "&districtcode=" + districtcode + "&blockcode=" + blockcode + "&grampanchayatcode=" + grampanchayatcode)
            .success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.NCDVillageDetail != null) {
                    $scope.NCDVillageDetails = JSON.parse(data.Result.Data.NCDVillageDetail);
                }
                $(".se-pre-con").fadeOut("slow");
            })
            .error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    //District Only

    $scope.GetBlockByDistrict = function (districtcode, statecode) {
        // district must exist

        if (!districtcode) {
            $scope.NCDBlockDetails = [];
            return;
        }

        $(".se-pre-con").fadeIn("slow");

        // Build API URL
        var url = SiteUrl + "/NcdData/GetBlockByStateDistrict?districtcode=" + districtcode;
        if (statecode) {
            url += "&statecode=" + statecode; // only if provided
        }

        $http.get(url)
            .success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.NCDBlockDetail != null) {
                    $scope.NCDBlockDetails = JSON.parse(data.Result.Data.NCDBlockDetail);
                }
                $(".se-pre-con").fadeOut("slow");
            })
            .error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetGramPanchayatByDistrictBlockse = function (districtcode, blockcode) {
        // District is mandatory
        if (!districtcode) {
            $scope.NCDGramPanchayatDetails = [];
            return;
        }

        $(".se-pre-con").fadeIn("slow");

        // Build URL dynamically
        var url = SiteUrl + "/NCDService/GetGramPanchayatByStateDistrictBlock?districtcode=" + districtcode;
        if (statecode) {
            url += "&statecode=" + statecode;
        }
        if (blockcode) {
            url += "&blockcode=" + blockcode;
        }

        $http.get(url)
            .success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.NCDGramPanchayatDetail != null) {
                    $scope.NCDGramPanchayatDetails = JSON.parse(data.Result.Data.NCDGramPanchayatDetail);
                }
                $(".se-pre-con").fadeOut("slow");
            })
            .error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.NCDAuditCategories = [];
    /* Fetch audit categories*/
    $scope.GetNCDAuditCategories = function () {

        $scope.NCDAuditCategories = [];
        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/NcdData/GetNCDAuditCategories")
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data.Result.Status === "200") {
                    if (response.data.Result.Data.NCDAuditCategoryDetail) {
                        $scope.NCDAuditCategories = JSON.parse(response.data.Result.Data.NCDAuditCategoryDetail);
                    }
                } else {
                    console.error("Error fetching audit categories:", response.data.Result.Message);
                }
            }, function (error) {
                $(".se-pre-con").fadeOut("slow");
                console.error("HTTP error while fetching audit categories:", error);
            });
    };
    $scope.NCDCoopBanks = [];
    $scope.NCDRegistrationAuthorities = [];

    // Fetch registration authorities
    $scope.GetNCDRegistrationAuthorities = function () {
        $scope.NCDRegistrationAuthorities = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetNCDRegistrationAuthorities")
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data && response.data.Result && response.data.Result.Status === "200") {
                    if (response.data.Result.Data.NCDRegistrationAuthorityDetail) {
                        $scope.NCDRegistrationAuthorities = JSON.parse(response.data.Result.Data.NCDRegistrationAuthorityDetail);
                    }
                } else {
                    console.error("Error fetching registration authorities:",
                        response.data && response.data.Result
                            ? response.data.Result.Message
                            : "Invalid response format");
                }
            }, function (error) {
                $(".se-pre-con").fadeOut("slow");
                console.error("HTTP error while fetching registration authorities:", error);
            });
    };

    $scope.SocietyFunctionalStatusList = [];

    // Fetch society functional statuses
    $scope.GetSocietyFunctionalStatus = function () {
        $scope.SocietyFunctionalStatusList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetSocietyFunctionalStatus")
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data && response.data.Result && response.data.Result.Status === "200") {
                    if (response.data.Result.Data.SocietyFunctionalStatusDetail) {
                        $scope.SocietyFunctionalStatusList = JSON.parse(response.data.Result.Data.SocietyFunctionalStatusDetail);
                    }
                } else {
                    console.error("Error fetching functional status:",
                        response.data && response.data.Result
                            ? response.data.Result.Message
                            : "Invalid response format");
                }
            }, function (error) {
                $(".se-pre-con").fadeOut("slow");
                console.error("HTTP error while fetching functional status:", error);
            });
    };

    // Fetch NCD Designations
    $scope.GetNcdDesignations = function () {
        $scope.NcdDesignationsList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetNcdDesignations")
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data && response.data.Result && response.data.Result.Status === "200") {
                    if (response.data.Result.Data.NcdDesignationDetail) {
                        $scope.NcdDesignationsList = JSON.parse(response.data.Result.Data.NcdDesignationDetail);
                    }
                } else {
                    console.error("Error fetching designations:",
                        response.data && response.data.Result
                            ? response.data.Result.Message
                            : "Invalid response format");
                }
            }, function (error) {
                $(".se-pre-con").fadeOut("slow");
                console.error("HTTP error while fetching designations:", error);
            });
    };

    // Fetch Financial Years
    $scope.GetFinancialYears = function () {
        $scope.FinancialYearList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetFinancialYears")
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data && response.data.Result && response.data.Result.Status === "200") {
                    if (response.data.Result.Data.FinancialYearDetail) {
                        $scope.FinancialYearList = JSON.parse(response.data.Result.Data.FinancialYearDetail);
                    }
                } else {
                    console.error("Error fetching financial years:",
                        response.data && response.data.Result
                            ? response.data.Result.Message
                            : "Invalid response format");
                }
            }, function (error) {
                $(".se-pre-con").fadeOut("slow");
                console.error("HTTP error while fetching financial years:", error);
            });
    };

    //Independent District Paste Here
    $scope.GetDistricts = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetNCDDistrictDetailsoo")
            .success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.NCDDistrictDetailso != null) {
                    $scope.NCDDistrictDetailsse = JSON.parse(data.Result.Data.NCDDistrictDetailso);
                }
                else {
                    $scope.NCDDistrictDetailsse = [];
                }
                $(".se-pre-con").fadeOut("slow");
            })
            .error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.GetBlocksByDistrict = function (districtcode) {
        if (!districtcode) {
            $scope.NCDBlockDetailsse = [];
            return;
        }

        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetNCDBlockDetails?districtcode=" + districtcode)
            .then(function (response) {
                if (response.data && response.data.Result && response.data.Result.Data) {
                    $scope.NCDBlockDetailsse = JSON.parse(response.data.Result.Data.NCDBlockDetailso);
                } else {
                    $scope.NCDBlockDetailsse = [];
                }
                $(".se-pre-con").fadeOut("slow");
            }, function (error) {
                console.error("Error loading Blocks:", error);
                $scope.NCDBlockDetailsse = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetGramPanchayatByDistrictBlocksec = function (districtcode, blockcode) {
        if (!districtcode || !blockcode) {
            $scope.NCDGramPanchayatDetails = [];
            return;
        }
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetNCDGramPanchayatDetails?districtcode=" + districtcode + "&blockcode=" + blockcode)
            .then(function (response) {
                if (response.data && response.data.Result && response.data.Result.Data) {
                    $scope.NCDGramPanchayatDetailse = JSON.parse(response.data.Result.Data.NCDGramPanchayatDetails);
                } else {
                    $scope.NCDGramPanchayatDetailse = [];
                }
                $(".se-pre-con").fadeOut("slow");
            }, function (error) {
                console.error("Error loading Gram Panchayats:", error);
                $scope.NCDGramPanchayatDetailse = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetVillageByDistrictBlockGram = function (districtcode, blockcode, gram_panchayat_code) {
        if (!districtcode || !blockcode || !gram_panchayat_code) {
            $scope.NCDVillageDetailsec = [];
            return;
        }

        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetNCDVillageDetails?districtcode=" + districtcode + "&blockcode=" + blockcode + "&grampanchayatcode=" + gram_panchayat_code)
            .then(function (response) {
                if (response.data && response.data.Result && response.data.Result.Data) {
                    $scope.NCDVillageDetailsec = JSON.parse(response.data.Result.Data.NCDVillageDetails);
                } else {
                    $scope.NCDVillageDetailsec = [];
                }
                $(".se-pre-con").fadeOut("slow");
            }, function (error) {
                console.error("Error loading Villages:", error);
                $scope.NCDVillageDetailsec = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.filterBankType = function (selectedBankTypes) {
        $scope.NCDCoopBanks = [];
        $(".se-pre-con").fadeIn("slow");
        var bankTypeId = 0;
        $http.get(SiteUrl + "/NcdData/GetNCDCooperativeSocietyBanks", { params: { BankTypeId: bankTypeId } })
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data && response.data.Result && response.data.Result.Status === "200") {
                    $scope.NCDCoopBanks = JSON.parse(response.data.Result.Data.NCDCoopBankDetail);
                } else {
                    console.error("Error fetching banks:", response.data.Result.Message);
                }
            }, function (error) {
                $(".se-pre-con").fadeOut("slow");
                console.error("HTTP error while fetching banks:", error);
            });
    };

    $scope.NCDBeeBeehiveTypes = [];
    $scope.ncd = $scope.ncd || {};
    // Fetch dropdown values
    $scope.GetBeeBeehiveTypes = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetBeeBeehiveTypes")
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data.Result.Status === "200") {
                    if (response.data.Result.Data.BeeBeehiveTypes) {
                        $scope.NCDBeeBeehiveTypes = JSON.parse(response.data.Result.Data.BeeBeehiveTypes);
                    }
                } else {
                    console.error("Error fetching data:", response.data.Result.Message);
                }
            }, function (error) {
                $(".se-pre-con").fadeOut("slow");
                console.error("HTTP error:", error);
            });
    };

    $scope.NCDBeeBeehiveTypes2 = [];
    $scope.GetBeeBeehiveTypes2 = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetBeeBeehiveTypes2")
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data.Result.Status === "200") {
                    if (response.data.Result.Data.BeeBeehiveTypes) {
                        $scope.NCDBeeBeehiveTypes2 = JSON.parse(response.data.Result.Data.BeeBeehiveTypes);
                    }
                } else {
                    console.error("Error fetching data:", response.data.Result.Message);
                }
            }, function (error) {
                $(".se-pre-con").fadeOut("slow");
                console.error("HTTP error:", error);
            });
    };
    $scope.GetFacilities = function (type) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetFacilities", { params: { type: type } })
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data.Result.Status === "200") {
                    if (response.data.Result.Data.Facilities) {
                        $scope.NCDFacilities = JSON.parse(response.data.Result.Data.Facilities);
                    }
                } else {
                    console.error("Error fetching data:", response.data.Result.Message);
                }
            }, function (error) {
                $(".se-pre-con").fadeOut("slow");
                console.error("HTTP error:", error);
            });
    };


    $scope.GetNcdOfficeBuildingTypes = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetNcdOfficeBuildingTypes")
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data.Result.Status === "200") {
                    if (response.data.Result.Data.NCDOfficeBuildingTypes) {
                        $scope.NCDOfficeBuildingTypes = JSON.parse(response.data.Result.Data.NCDOfficeBuildingTypes);
                    }
                } else {
                    console.error("Error fetching data:", response.data.Result.Message);
                }
            }, function (error) {
                $(".se-pre-con").fadeOut("slow");
                console.error("HTTP error:", error);
            });
    };

    //30-09-2025
    $scope.GetAreaOfOperations = function (operation_area_location) {
        if (!operation_area_location) {
            $scope.AreaofOperationList = [];
            return;
        }

        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/NcdData/GetAreaOperationDetails?operation_area_location=" + operation_area_location)
            .then(function (response) {
                if (response.data != null && response.data.Result != null && response.data.Result.Data != null) {
                    $scope.AreaofOperationList = JSON.parse(response.data.Result.Data.OperationDetails);
                    console.log($scope.AreaofOperationList);
                } else {
                    $scope.AreaofOperationList = [];
                }
                $(".se-pre-con").fadeOut("slow");
            }, function (error) {
                console.error("Error loading Area Of Operations:", error);
                $scope.AreaofOperationList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.AddMemberDetail = function (value) {
        value = parseInt(value);
        $scope.ncd.members_of_society = ($scope.ncdm.m_general ?? 0) +
            ($scope.ncdm.m_sc ?? 0) +
            ($scope.ncdm.m_st ?? 0) +
            ($scope.ncdm.m_obc ?? 0) +
            ($scope.ncdm.f_general ?? 0) +
            ($scope.ncdm.f_sc ?? 0) +
            ($scope.ncdm.f_st ?? 0) +
            ($scope.ncdm.f_obc ?? 0) +
            ($scope.ncdm.t_general ?? 0) +
            ($scope.ncdm.t_sc ?? 0) +
            ($scope.ncdm.t_st ?? 0) +
            ($scope.ncdm.t_obc ?? 0);

    };
    $scope.SaveData = function () {
        debugger;
        $scope.ncd.memberArray = { ...$scope.ncdm };
        $scope.ncd.sectorDetailsU = { ...$scope.sector };
        //$scope.ncd.childArray = { ...$scope.ncdc };    
        //return;
        $http.post(SiteUrl + "/NcdData/SaveNcdData", { ncdJson: JSON.stringify($scope.ncd) })
            .then(function (res) {
                console.log(res);
                debugger;
                //$scope.saving = false;
                //if (resp.data && resp.data.Result && resp.data.Result.Status === "200") {
                //    alert("Saved successfully. New Id: " + resp.data.Result.Data.NewId);
                //    // optional: clear form or redirect
                //} else {
                //    console.error("Save failed:", resp.data);
                //    alert("Save failed: " + (resp.data.Result ? resp.data.Result.Message : 'Unknown'));
                //}
            }, function (err) {
                $scope.saving = false;
                console.error("HTTP error:", err);
                alert("HTTP error while saving. See console.");
            });
    };



    //Added by jaya

    $scope.loadDistrictList = function () {
        debugger;

        // Build filter query params
        var params = {
            stateId: $scope.ncd.state_code && $scope.ncd.state_code > 0 ? $scope.ncd.state_code : null,
            districtId: $scope.ncd.district_code && $scope.ncd.district_code > 0 ? $scope.ncd.district_code : null,
            functionalStatusId: $scope.ncd.functional_status && $scope.ncd.functional_status > 0 ? $scope.ncd.functional_status : null
        };

        $http({
            method: "GET",
            url: SiteUrl + "/NcdData/GetDistrictDashboard",
            params: params
        }).success(function (response) {
            if (response.Result.Status == "200") {
                // Parse JSON into array
                $scope.DistrictList = JSON.parse(response.Result.Data.DistrictDashboard);

                console.log($scope.DistrictList);

                // Calculate totals
                $scope.totalCooperatives = 0;
                $scope.totalMembers = 0;
                angular.forEach($scope.DistrictList, function (item) {

                    console.log(item);

                    $scope.totalCooperatives += parseInt(item.NumberOfCooperatives) || 0;
                    $scope.totalMembers += parseInt(item.NumberOfMembers) || 0;
                });
                
            } else {
                ShowOkMessage("Error", "No records found.");
            }
        }).error(function () {
            ShowOkMessage("Error", "Something went wrong while fetching sector details.");
        });
    };


    //$scope.loadSectorList = function () {
    //    debugger;

    //    var params = {
    //        fromDate: $scope.ncd.from_date ? $scope.ncd.from_date : null,
    //        toDate: $scope.ncd.to_date ? $scope.ncd.to_date : null,
    //        districtId: $scope.ncd.district_code && $scope.ncd.district_code > 0 ? $scope.ncd.district_code : null,
    //        functionalStatusId: $scope.ncd.functional_status && $scope.ncd.functional_status > 0 ? $scope.ncd.functional_status : null
    //    };

    //    $http({
    //        method: "GET",
    //        url: SiteUrl + "/NcdData/GetSectorDashboard",
    //        params: params
    //    }).success(function (response) {
    //        if (response.Result.Status == "200") {
    //            // Parse the JSON data
    //            $scope.SectorList = JSON.parse(response.Result.Data.SectorDashboard);

    //            // Arrays for chart
    //            var labels = [];
    //            var societies = [];
    //            var members = [];

    //            // Totals
    //            $scope.totalSocieties = 0;
    //            $scope.totalMembers = 0;

    //            angular.forEach($scope.SectorList, function (item) {
    //                labels.push(item.SectorName);
    //                societies.push(parseInt(item.NumberOfSocieties) || 0);
    //                members.push(parseInt(item.NumberOfMembers) || 0);

    //                $scope.totalSocieties += parseInt(item.NumberOfSocieties) || 0;
    //                $scope.totalMembers += parseInt(item.NumberOfMembers) || 0;
    //            });

    //            // Draw Chart
    //            setTimeout(function () {
    //                var ctx = document.getElementById("sectorChart").getContext("2d");
    //                if ($scope.chartInstance) {
    //                    $scope.chartInstance.destroy(); // destroy old chart before reloading
    //                }

    //                $scope.chartInstance = new Chart(ctx, {
    //                    type: 'bar',
    //                    data: {
    //                        labels: labels,
    //                        datasets: [
    //                            {
    //                                label: "Number of Societies",
    //                                data: societies,
    //                                backgroundColor: "rgba(54, 162, 235, 0.6)"
    //                            },
    //                            {
    //                                label: "Number of Members",
    //                                data: members,
    //                                backgroundColor: "rgba(255, 99, 132, 0.6)"
    //                            }
    //                        ]
    //                    },
    //                    options: {
    //                        responsive: true,
    //                        maintainAspectRatio: false,
    //                        plugins: {
    //                            legend: {
    //                                position: 'top'
    //                            },
    //                            title: {
    //                                display: true,
    //                                text: 'Sector-wise Cooperative Society Dashboard'
    //                            }
    //                        },
    //                        scales: {
    //                            x: {
    //                                ticks: {
    //                                    autoSkip: false,
    //                                    maxRotation: 60,
    //                                    minRotation: 45,
    //                                    font: { size: 10 }
    //                                }
    //                            },
    //                            y: {
    //                                beginAtZero: true
    //                            }
    //                        }
    //                    }
    //                });
    //            }, 200);

    //        } else {
    //            ShowOkMessage("Error", "No records found.");
    //        }
    //    }).error(function () {
    //        ShowOkMessage("Error", "Something went wrong while fetching sector details.");
    //    });
    //};

    $scope.ClearSearch = function () {
        // Clear filter values    
        $scope.ncd.state_code = null;
        $scope.ncd.district_code = null;
        $scope.ncd.functional_status = null;

        // Reload sector list without filters
        $scope.loadDistrictList();
    };


    $scope.OnPageLoad();

});