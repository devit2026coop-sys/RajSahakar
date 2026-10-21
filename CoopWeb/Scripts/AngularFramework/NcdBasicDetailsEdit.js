
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
    $scope.dataList = [];
    $scope.isEditMode = true;



    $scope.sector = { operation_area_location: "2" };
    $scope.ncd = null;
    $scope.ncd = [];
    $scope.ncd = { cooperative_society_name: "" }

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
        $scope.GetDistrictByStatecode();
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

        //Consumer Cooperative Society Details
        $scope.GetFacilities();
        $scope.GetNcdOfficeBuildingTypes();
        $scope.filterPrimaryActivity();
        $scope.GetAreaOfOperations($scope.sector.operation_area_location);

        //const urlParams = new URLSearchParams(window.location.search);
        //const eId = urlParams.get('eId');

        //if (eId) {
        //    debugger;
        //    $scope.GetNcdDetailsById(eId);
        //    $scope.cooperative_id = eId;
        //}
        //else {
        //    alert("No Cooperative ID found in URL!");
        //}

        //console.log("Editing cooperative_id:", eId);
        //$scope.cooperative_id = eId;

        const urlParams = new URLSearchParams(window.location.search);
        const eid = urlParams.get('eId');

        if (eid) {
            $scope.GetListData1(eid);
        } else {
            console.error("No eId found in URL");
        }
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
        $http.get(SiteUrl + "/NcdData/GetDistrictwithStatecodeData?statecode=" + statecode).
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
                console.log($scope.ncd.registration_authoritie_id);

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
    console.log($scope.ncd.registration_authoritie_id);

/*    $scope.SocietyFunctionalStatusList = [];*/

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

    $scope.GetListData1 = function (eid) {
        debugger;
        $(".se-pre-con").fadeIn("slow");

        let params = {
            actionName: "GetNcdListDataView",
            eid: eid || null,
            FunctionalStatus: $scope.FunctionalStatus || null,
            sectorOperation: $scope.ncd?.sector_of_operation_type || null,
            cooperative_society_name: $scope.searchText || null,
            registrationNo: $scope.registration_number || null
        };


        $http({
            method: 'GET',

            url: SiteUrl + "/NcdData/GetNcdBasicDetailsView?eid=" + eid,

            params: params
        }).then(function (response) {
            $(".se-pre-con").fadeOut("slow");
            console.log("API Response:", response.data);

            if (response.data && response.data.Result && response.data.Result.Status === "200") {
                $scope.ncd = response.data.Result.Data.NcdListData[0];

                $scope.ncdm = {
                    m_general: $scope.ncd.m_general || 0,
                    f_general: $scope.ncd.f_general || 0,
                    t_general: $scope.ncd.t_general || 0,
                    msc: $scope.ncd.msc || 0,
                    fsc: $scope.ncd.fsc || 0,
                    tsc: $scope.ncd.tsc || 0,
                    m_st: $scope.ncd.m_st || 0,
                    fst: $scope.ncd.fst || 0,
                    tst: $scope.ncd.tst || 0,
                    m_obc: $scope.ncd.m_obc || 0,
                    f_obc: $scope.ncd.f_obc || 0,
                    t_obc: $scope.ncd.t_obc || 0,
                    m_total: $scope.ncd.m_total || 0,
                    f_total: $scope.ncd.f_total || 0,
                    t_total: $scope.ncd.t_total || 0,
                    total_general: $scope.ncd.total_general || 0,
                    total_sc: $scope.ncd.total_sc || 0,
                    total_st: $scope.ncd.total_st || 0,
                    total_obc: $scope.ncd.total_obc || 0,
                    total_total: $scope.ncd.total_total || 0
                };

            } else {
                console.error("Unexpected API response format:", response.data);
                $scope.ncd = {};
                $scope.ncdm = {};
            }
        }, function (error) {
            $(".se-pre-con").fadeOut("slow");
            console.error("Error loading data:", error);
        });

    };


    $scope.UpdateNcdData = function (cooperative_id) {
        debugger
        if (!cooperative_id) {
            alert("No Cooperative ID found to update!");
            return;
        }

        debugger;
        console.log("Updating NCD record for Cooperative ID:", cooperative_id);

        $scope.ncd = { ...$scope.ncd };
        debugger;

        let payload = {

            ncdJson: JSON.stringify($scope.ncd),
            actionName: "UpdateNcdBasicDetails",
            ncd_id: cooperative_id,
            updatedBy: 1
        };

        console.log("Payload being sent:", payload);

        $http.post("../NCDData/UpdateNCDData", payload)
            .then(function (response) {
                debugger
                if (response.data.Result.Status === "200") {
                    alert("NCD data updated successfully!");
                } else {
                    alert("Update failed: " + response.data.Result.Message);
                }
            }, function (error) {
                console.error("Error updating data:", error);
                alert("Something went wrong!");
            });


    };

    //$scope.GetNcdDetailsById = function (cooperative_id) {
    //    debugger;
    //    $(".se-pre-con").fadeIn("slow");
    //    if (!cooperative_id) {
    //        alert("No Cooperative ID provided!");
    //        return;
    //    }

    //    let params = {
    //        actionName: "GetNcdListData",
    //        cooperative_id: cooperative_id || null,
    //        FunctionalStatus: $scope.FunctionalStatus || null,
    //        sectorOperation: $scope.ncd?.sector_of_operation_type || null,
    //        /* cooperative_society_name: $scope.searchText || null,*/
    //        registrationNo: $scope.registration_number || null,
    //        cooperative_society_name: $scope.ncd.cooperative_society_name || null
    //    };
     
    //    $http({
    //        method: 'GET',

    //        url: SiteUrl + "/NcdData/GetNcdDetailsById?cooperative_id=" + cooperative_id,

    //        params: params
    //    })
    //        .then(function (response) {
    //            $(".se-pre-con").fadeOut("slow");

    //            if (response.data && response.data.Result && response.data.Result.Status === "200") {
    //                var data = JSON.parse(response.data.Result.Data.NCDEditdata);
    //                console.log(data)
    //                if (data != null && data.length > 0) {
    //                    $scope.ncd = data[0];
    //                } else {
    //                    $scope.ncd = {};
    //                    //alert("No NCD details found!");
    //                }
    //                //$scope.ncd = response.data.Result.Data.NcdListData[0];
    //                $scope.ncd = response.data.Result.Data;
    //                console.log("Js Data" ,$scope.ncd)
    //            } else {
    //                $scope.ncd = {};
    //                console.error("Unexpected API response format:", response.data);

    //                alert("No record found for this ID");
    //            }
    //        }, function (error) {
    //            $(".se-pre-con").fadeOut("slow");
    //            console.error("Error loading data:", error);

    //        });
    //};



    $scope.OnPageLoad();

});