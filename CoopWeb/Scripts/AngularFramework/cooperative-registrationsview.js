
var MasterMod = angular.module('ncdMod', ['angularUtils.directives.dirPagination']);

MasterMod.controller("ncdCtrl", function ($scope, $http) {
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
    $scope.NcdListData = [];
    $scope.searchText = "";
    $scope.searchTextsearchText = "";
    $scope.formattedDate = "";
    $scope.exportToExcel = [];
    $scope.ncd = {};
    $scope.ncdm = {};
    $scope.fss = {};
    $scope.marketing = {
        license_to_sell: "",
        sell_the_item: ""
    };

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

        $scope.GetSocietyFunctionalStatus();
        $scope.GetsectorOperationDetails();
        $scope.GetDistricts();

        $scope.KhadiGram = {
            products: [
                { item_name: '', yearly_production: '', unit: '', value_in_rs: '' }
            ]
        }

        $scope.BeeFarmingItems = [{ id: 1, name: "Honey" }, { id: 2, name: "Bee Pollen" }];

        $scope.Date = function (value) {
            if (!value) return "";
            var timestamp = parseInt(value.replace(/[^0-9]/g, ""));
            var date = new Date(timestamp);
            return date.toLocaleDateString("en-GB");
        };

        const urlParams = new URLSearchParams(window.location.search);
        const cooperativeId = urlParams.get('cooperativeId');
        const sectorOperation = urlParams.get('sectorOperation'); // <-- new line

        if ($scope.GetListDataforView) {

            $scope.GetListDataforView(cooperativeId, sectorOperation); // pass both if needed
        } else {
            console.error("No cooperativeId found in URL");
        }
    };

    $scope.isPacsPrimaryActivity = function (sectorOperation) {
        if (sectorOperation == 20) { $scope.is_pacsTitle = 'FSS Detail'; }
        else if (sectorOperation == 22) { $scope.is_pacsTitle = 'LAMPS Detail'; }
        else if (sectorOperation == 1) { $scope.is_pacsTitle = 'Pacs Detail'; }
        else if (sectorOperation == 7);
    }

    $scope.is_profitMaking = function () {
        $scope.showSecondBox = true;
        if ($scope.ncd.is_profit_making == "1") {
            $scope.isTrunOverText = "Annual Turnover (₹)";
        } else {
            $scope.isTrunOverText = "Annual Payment (₹)";
        }
    };

    $scope.is_profitMaking = function () {
        $scope.isTrunOverText = '';
        $scope.ngModel = '';
        $scope.isTextName = '';
        if ($scope.ncd.is_profit_making == 0) {
            $scope.isTrunOverText = 'Net Loss of the Cooperative Society';
            $scope.isTextName = 'annual_loss';
        } else {
            $scope.isTrunOverText = 'Net Profit of the Cooperative Society'; //yes
            $scope.isTextName = 'annual_turnover';
        }
    };

    $scope.GetsectorOperationDetails = function () {
        $scope.sectorOperationList = [
            { id: 1, name: 'Cooperative Bank' },
            { id: 2, name: 'Commercial Bank' }
        ];

    };

    $scope.getBankTypeName = function (id) {
        if (!id) return "N/A";
        const item = $scope.sectorOperationList.find(x => x.id == id);
        return item ? item.name : "N/A";
    };


    $scope.GetListDataforView = function (cooperativeId, sectorOperation) {
        $(".se-pre-con").fadeIn("slow");

        if (!cooperativeId) {
            console.error("Missing cooperativeId parameter in GetListDataforView");
            $(".se-pre-con").fadeOut("slow");
            return;
        }
        let params = {
            actionName: "GetNcdListDataView",
            cooperativeId: cooperativeId || null,
            FunctionalStatus: $scope.FunctionalStatus || null,
            sectorOperation: sectorOperation || $scope.ncd?.sector_of_operation_type || null,
            cooperative_society_name: $scope.searchText || null,
            registrationNo: $scope.registration_number || null
        };

        $http({
            method: 'GET',
            url: SiteUrl + "/NcdData/GetNcdBasicDetailsView?cooperativeId=" + cooperativeId,
            params: params
        }).then(function (response) {
            $(".se-pre-con").fadeOut("slow");
            if (response.data && response.data.Result && response.data.Result.Status === "200") {
                $scope.ncd = response.data.Result.Data.basic;
                $scope.isPacsPrimaryActivity(sectorOperation);
                if ($scope.fss = response.data.Result.Data.fss) {
                    $scope.ncd.sectorOperation = [1, 20, 22];
                } else if ($scope.tsd = response.data.Result.Data.tsd) {
                    $scope.ncd.sectorOperation = 18;
                } else if ($scope.consumer = response.data.Result.Data.consumer) {
                    $scope.ncd.sectorOperation = 80;
                } else if ($scope.aacs = response.data.Result.Data.aacs) {
                    $scope.ncd.sectorOperation = 77;
                } else if ($scope.crp = response.data.Result.Data.crp) {
                    $scope.ncd.sectorOperation = 31;
                } else if ($scope.Handicraft = response.data.Result.Data.Handicraft) {
                    $scope.ncd.sectorOperation = 14;
                } else if ($scope.handloom = response.data.Result.Data.handloom) {
                    $scope.ncd.sectorOperation = 13;
                } else if ($scope.Housing = response.data.Result.Data.Housing) {
                    $scope.ncd.sectorOperation = 47;
                } else if ($scope.KhadiGram = response.data.Result.Data.KhadiGram) {
                    $scope.ncd.sectorOperation = 117;
                } else if ($scope.dairy = response.data.Result.Data.dairy) {
                    $scope.ncd.sectorOperation = 9;
                } else if ($scope.ucb = response.data.Result.Data.ucb) {
                    $scope.ncd.sectorOperation = 7;
                } else if ($scope.beeCoop = response.data.Result.Data.beeCoop) {
                    $scope.ncd.sectorOperation = 79;
                } else if ($scope.Fishery = response.data.Result.Data.Fishery) {
                    $scope.ncd.sectorOperation = 10;
                } else if ($scope.Jute = response.data.Result.Data.Jute) {
                    $scope.ncd.sectorOperation = 90;
                } else if ($scope.Labour = response.data.Result.Data.Labour) {
                    $scope.ncd.sectorOperation = 51;
                } else if ($scope.education = response.data.Result.Data.education) {
                    $scope.ncd.sectorOperation = 84;
                } else if ($scope.livestock = response.data.Result.Data.livestock) {
                    $scope.ncd.sectorOperation = 54;
                } else if ($scope.marketing = response.data.Result.Data.marketing) {
                    $scope.ncd.sectorOperation = 82;
                } else if ($scope.csm = response.data.Result.Data.csm) {
                    $scope.ncd.sectorOperation = 35;
                } else if ($scope.ncsm = response.data.Result.Data.ncsm) {
                    $scope.ncd.sectorOperation = 29;
                } else if ($scope.multi = response.data.Result.Data.multi) {
                    $scope.ncd.sectorOperation = 16;
                } else if ($scope.sericulture = response.data.Result.Data.sericulture) {
                    $scope.ncd.sectorOperation = 96;
                } else if ($scope.social = response.data.Result.Data.social) {
                    $scope.ncd.sectorOperation = 98;
                } else if ($scope.sugar = response.data.Result.Data.sugar) {
                    $scope.ncd.sectorOperation = 11;
                } else if ($scope.tourism = response.data.Result.Data.tourism) {
                    $scope.ncd.sectorOperation = 99;
                } else if ($scope.transport = response.data.Result.Data.transport) {
                    $scope.ncd.sectorOperation = 68;
                } else if ($scope.tribal = response.data.Result.Data.tribal) {
                    $scope.ncd.sectorOperation = 102;
                } else if ($scope.women = response.data.Result.Data.women) {
                    $scope.ncd.sectorOperation = 15;
                }

                $scope.ncdm = {
                    m_general: $scope.ncd.m_general || 0,
                    f_general: $scope.ncd.f_general || 0,
                    t_general: $scope.ncd.t_general || 0,
                    msc: $scope.ncd.msc || 0,
                    fsc: $scope.ncd.fsc || 0,
                    tsc: $scope.ncd.tsc || 0,
                    mst: $scope.ncd.mst || 0,
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
                    total_total: $scope.ncd.total_total || 0,
                };
            }

            else {
                console.error("Unexpected API response format:", response.data);
                $scope.ncd = {};
                $scope.fss = {};
                $scope.ncdm = {};
            }
        }, function (error) {
            $(".se-pre-con").fadeOut("slow");
            console.error("Error loading data:", error);
        });

    };

    /*------------Export Data-----------------*/

    $scope.exportToExcel = function () {
        if (!$scope.filteredList || $scope.filteredList.length === 0) {
            alert("No data to export!");
            console.log(filteredList);
            return;
        }
        const data = $scope.filteredList.map((item, index) => ({
            "S.No": index + 1,
            "Cooperative Society Name": item.cooperative_society_name || "",
            "Operation Area": item.operation_area_location || "",
            "State": item.name || "",
            "District": item.DistrictName || "",
            "Block": item.blockname || "",
            "Registration No": item.registration_number || "",
            "Members": item.members_of_society || "",
            "Functional Status": item.FunctionalStatus || "",
            "Cooperative ID": item.cooperative_id || "",
            "Reference Year": item.reference_year || "",
            "Date of Registration": item.date_registration ? new Date(item.date_registration).toLocaleDateString('en-GB') : "",
            "Created Date": item.CreatedDate ? new Date(item.CreatedDate).toLocaleDateString('en-GB') : ""
        }));
        const worksheet = XLSX.utils.json_to_sheet(data);
        const workbook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workbook, worksheet, "NCD_List");
        XLSX.writeFile(workbook, "NCDListData.xlsx");
    };

    $scope.FilteredNCDSectorDetails = [];
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

    $scope.SocietyFunctionalStatusList = [];
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

    $scope.goBack = function () {
        window.location.href = '/NCDData/NcdBasicDetailsList';
    };

    $scope.OnPageLoad();


});