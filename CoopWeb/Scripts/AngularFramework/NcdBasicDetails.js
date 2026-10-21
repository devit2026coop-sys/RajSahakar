
var MasterMod = angular.module('ncdMod', ['angularUtils.directives.dirPagination']);
MasterMod.directive('percentInput', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            function fromUser(text) {
                if (!text) return text;
                let val = text.trim();
                if (!/^\d{0,3}%?$/.test(val)) { ngModelCtrl.$setViewValue(ngModelCtrl.$modelValue || ''); ngModelCtrl.$render(); return ngModelCtrl.$modelValue || ''; }
                let numberPart = val.replace('%', '');
                let num = parseInt(numberPart, 10);
                if (isNaN(num)) { ngModelCtrl.$setViewValue(''); ngModelCtrl.$render(); return ''; }
                if (num > 100) { num = 100; }
                let hasPercent = val.indexOf('%') !== -1;
                let newViewValue = hasPercent ? num + '%' : num.toString();
                if (newViewValue !== val) { ngModelCtrl.$setViewValue(newViewValue); ngModelCtrl.$render(); }
                return newViewValue;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
});
MasterMod.directive('numbersOnly', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            const maxLength = 15;
            element.on('input', function () {
                let value = element.val();
                let clean = value.replace(/[^0-9.]/g, '');
                const parts = clean.split('.');
                if (parts.length > 2) {
                    clean = parts[0] + '.' + parts.slice(1).join('');
                }
                if (clean.replace('.', '').length > maxLength) {
                    const [intPart, decPart] = clean.split('.');
                    if (intPart.length > maxLength) {
                        clean = intPart.substring(0, maxLength) + (decPart ? '.' + decPart : '');
                    } else if (decPart) {
                        const remaining = maxLength - intPart.length;
                        clean = intPart + '.' + decPart.substring(0, remaining);
                    }
                }
                if (value !== clean) {
                    element.val(clean);
                }
            });
        }
    };
});
MasterMod.directive('disableBeforeDate', function () {
    return {
        restrict: 'A',
        link: function (scope, element) {
            const today = new Date().toISOString().slice(0, 10);
            scope.$evalAsync(() => {
                element.attr('max', today);
            });
            scope.$watch(() => element.attr('max'), function () {
                element.attr('max', today);
            });
            element.on('keydown', function (e) {
                e.preventDefault();
            });
            element.on('paste drop', function (e) {
                e.preventDefault();
            });
        }
    };
});

MasterMod.directive('select2', function () {
    return {
        restrict: 'A', require: 'ngModel', scope: { ngModel: '=', ngChange: '&?' },
        link: function (scope, element, attrs, ngModelCtrl) {
            $(element).select2();
            $(element).on('change', function () {
                var value = $(element).val();
                scope.$applyAsync(function () {
                    ngModelCtrl.$setViewValue(value);
                    if (attrs.ngChange) {
                        scope.ngChange();
                    }
                });
            });
            ngModelCtrl.$render = function () {
                $(element).val(ngModelCtrl.$viewValue).trigger("change.select2");
            };
        }
    };
});

MasterMod.directive("mobileNumber", function () {
    return {
        restrict: "A",
        require: "ngModel",
        link: function (scope, element, attrs, ngModel) {
            element.on("input", function () {
                let val = element.val();
                val = val.replace(/[^0-9]/g, "");
                if (val.length > 10) val = val.substring(0, 10);

                element.val(val);
                ngModel.$setViewValue(val);
                ngModel.$render();
            });
            ngModel.$validators.mobileNumber = function (value) {
                if (!value) return false;
                const validFormat = /^[6-9]\d{9}$/.test(value);
                const notRepeated = !/^(\d)\1{9}$/.test(value);
                return validFormat && notRepeated;
            };
            element.on("blur", function () {
                ngModel.$setTouched();
                scope.$apply();
            });
        }
    };
});

MasterMod.directive("landlineNumber", function () {
    return {
        restrict: "A",
        link: function (scope, element) {
            element.on("input", function () {
                let val = element.val();
                val = val.replace(/[^0-9]/g, "");
                if (val.length > 12) {
                    val = val.substring(0, 12);
                }
                element.val(val);
            });

        }
    };
});
MasterMod.directive('filterSearchbox', function ($timeout) {
    return {
        restrict: 'A', require: 'ngModel', link: function (scope, element, attrs, ngModelCtrl) {
            $timeout(function () {
                $(element).select2({
                    placeholder: "Select option",
                    allowClear: true,
                    width: '100%'
                });
                $(element).on('change', function () {
                    scope.$applyAsync(function () {
                        const value = $(element).val();
                        ngModelCtrl.$setViewValue(value);
                        if (attrs.ngChange) { scope.$parent.$eval(attrs.ngChange); }
                    });
                });
                ngModelCtrl.$render = function () { $(element).val(ngModelCtrl.$viewValue).trigger('change.select2'); };
            }, 100);
            scope.$watch(() => element.children().length, function () {
                $timeout(() => {
                    $(element).trigger('change.select2');
                });
            });
        }
    };
});
MasterMod.directive('allowLetters', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            ngModelCtrl.$parsers.push(function (value) {
                if (!value) return value;
                var regex = /[^a-zA-Z\s]/g;
                var cleartxt = value.replace(regex, '');
                if (cleartxt !== value) {
                    ngModelCtrl.$setViewValue(cleartxt);
                    ngModelCtrl.$render();
                }
                return cleartxt;
            });
        }
    };
});




MasterMod.controller("ncdCtrl", function ($scope, $http) {
    $scope._language = 'en-us';
    $scope.NCDStateDetails = [];
    $scope.NCDDistrictDetails = [];
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
    $scope.bank_type = [];
    $scope.facilitiesList = [];
    $scope.typesofWomenCoopList = [];
    $scope.sector_operation = "";
    $scope.NCDOfficeBuildingTypes = [];
    $scope.primary_activity = "";
    $scope.NCD_Statecode = '';
    $scope.is_pacsTitle = '';
    $scope.isRegistrationTxt = false;
    $scope.isShowRemoveBtn = false;
    $scope.isShowforEdit = 1;
    $scope.secondaryActivityList = [];
    $scope.waterBodyTypeList = [];
    $scope.urbanBodyTypeDetailDataList = [];
    $scope.open = false;
    $scope.searchText = "";
    $scope.selectAllBanks = false;
    $scope.IsCooperativeNameTxt = true;
    $scope.IsBanktxtEnable = false;
    $scope.sectorListDataOld = [];
    $scope.UrbanCategoryLocalList = [];
    $scope.wardDetailDataList = [];
    $scope.bothSectorList = [];
    $scope.commonProductProduceList = [];
    $scope.NCDBeeBeehiveTypes = [];
    $scope.parts = [];
    $scope.ncdm = {
        m_total: 0,
        f_total: 0,
        t_total: 0,
        total_general: 0,
        total_sc: 0,
        total_st: 0,
        total_obc: 0,
        total_total: 0,
    }
    $scope.sectorDetail = {
        operation_area_location: "2",
        area_of_operation_id: "0"
    };
    $scope.ncd = {
        location_of_head_quarter: "2",
        is_coastal: "0",
        is_affiliated_union_federation: "0",
        financial_audit: "1",
        is_profit_making: "0",
        is_dividend_paid: "0",
        is_msme: "0",
        deregistered: "0",
        operation_area_location: $scope.sectorDetail.operation_area_location,
        sectorListData: [{
            operation_area_location: $scope.sectorDetail.operation_area_location,
            area_of_operation_id: $scope.sectorDetail.area_of_operation_id ?? "0",
            // Rural Fields
            AreaofOperationRuralDetailsId: 0,
            district_code: '',
            block_code: '',
            panchayat_code: '',
            village_code: '',
            // Urban Fields
            AreaofOperationUrbanDetailsId: 0,
            local_body_type_code: '',
            local_body_code: '',
            locality_ward_code: '',
            is_deleted: "0"
        }],
        bank_typeList: [],
        cooperative_society_bank_id: [],
        unionLevelList: []
    };
    if (!$scope.ncd.is_msme) {
        $scope.ncd.is_msme = "No";
    }
    $scope.pacs = {
        has_building: "0",
        is_socitey_has_land: "0",
        pesticide_distribution: "0",
        fertilizer_distribution: "0",
        is_pmksk: "0",
        seed_distribution: "0",
        is_foodgrains: "0",
        agricultural_implements: "0",
        dry_storage: "0",
        is_lgs_program: "0",
        cold_storage: "0",
        milk_unit: "0",
        food_processing: "0",
        pack_involved_fish_catch: "0",
        is_csc: "0",
        is_fpo: "0",
        is_lpg_distributership: "0",
        is_bcp_pump: "0",
        is_dpp_diesel: "0",
        is_jak: "0",
        is_pm_kusum_scheme: "0",
        fair_price: "0",
        is_paani_samity: "0",
        is_consumer_business: "0",
        is_tourism: "0",
        is_cash_kendra_facilities: "0",
        land: {
            land_owned: null,
            land_leased: null,
            land_allotted: null,
            land_total: null
        }
    };

    $scope.tsd = {
        has_building: "0",
        facilities: []
    };
    $scope.csm = {
        has_building: "0",
        paid_up_total: 0,
        members_of_society: 0,
        facilities: []
    };
    $scope.ucb = {
        has_building: "0",
        has_nafcub: "0",
        is_computerized: "0",
        have_ifsc: "0",
        have_corebanking: "0",
        have_doorstepservice: "0",
        is_aeps: "0",
        offer_debitcard: "0",
        have_internetbanking: "0",
        offer_creditcard: "0",
        cibil_membership: "0",
        conducting_gab: "0",
        cgtmsemli_member: "0",
        is_saf_to_cust: "0",
        fswm_comp: "0",
        is_gov_scheme_implemented: "0",
        schemesdetail: [{
            ncd_society_id: 0,
            gov_scheme_name: '',
            gov_scheme_type: '',
            total_amount: ''
        }]
    }
    $scope.dairy = {
        credit_facility: "0",
        milk_collection_unit: "0",
        transport_milk: "0",
        bulk_milk_unit: "0",
        milk_testing: "0",
        processing: "0",
        is_bank_mitra: "0",
        is_micro_atm: "0",
        credit_facility: "0"
    };
    $scope.women = {
        has_building: "0",
        is_raw_material_taken: "0",
        facilities: [],
        type_society: ""
    };
    $scope.aacs = {
        has_building: "0",
        has_pool_land: "0",
        has_gov_land: "0",
        is_member_work: "0",
        society_common_pool: "0",
        harvesting: "0",
        member_vested_right: "0",
        is_utilize_pool: "0",
        total: 0,
        paid_up_total: 0,
        type_society: [],
        irrigation_means: [],
        farming_mech: []
    };
    $scope.crp = {
        has_building: "0",
        processing_unit: "0",
        processing_by_members: "0",
        product_taken: "0",
        material_available: "0",
        wastes_generated: "0",
        operate_shops: "0",
        product_sale_out_of_area: "0",
        work_divided: "0",
        waste_management_facility: "0",
    };

    $scope.beeCoop = {
        common_yard: "0",
        rear_by_member: "0",
        guidance_by_member: "0",
        is_bee_plant_grow: "0",
        is_cleaning_process: "0",
        is_waste_facility: "0",
        honey_brand: "0",
        is_operate_retail: "0",
        is_product_sale_out: "0",
        own_brand_honey: "0",
        has_building: "0",
        facilities: [],
        type_bee: [],
        type_product: [],
        type_behives: []

    };
    $scope.consumer = {
        has_building: "0",
        has_store: "0",
        facilities: []
    };
    $scope.Fishery = {
        credit_facility: "0",
        fuel_distribution: "0",
        marketing: "0",
        cold_storage: "0",
        is_fpo_fisheries: "0",
        transportation: "0",
    };
    $scope.Handicraft = {
        has_building: "0",
        is_product_sale_out: "0",
        common_work_place: "0",
        is_raw_provide: "0",
        is_raw_easy_avail: "0",
        is_work_by_member: "0",
        is_training_provide: "0",
        is_waste_generate: "0",
        is_operate_retail: "0",
        is_waste_facility: "0",
        facilities: [],
        type_raw: [],
        type_produce: []
    };
    $scope.handloom = {
        has_building: "0",
        operated_member_themself: "0",
        is_user_work_divide: "0",
        raw_material_available: "0",
        waste_generate: "0",
        waste_available: "0",
        operate_retail: "0",
        product_sale_out: "0",
        raw_product_taken: "0"
    };

    $scope.Jute = {
        has_building: "0",
        common_work_place: "0",
        is_work_by_member: "0",
        is_raw_provide: "0",
        is_raw_easy_avail: "0",
        is_waste_generate: "0",
        is_operate_retail: "0",
        is_product_sale_out: "0",
        is_waste_facility: "0",
        is_training_provide: "0",
        facilities: [],
        type_raw: [],
        type_produce: []
    };

    $scope.KhadiGram = {
        do_you_want_to_enter_type_products_produced: "0",
        operated_member_themself: "0",
        is_user_work_divide: "0",
        raw_product_taken: "0",
        raw_material_available: "0",
        waste_generate: "0",
        operate_retail: "0",
        product_sale_out: "0",
        waste_available: "0",
        has_building: "0",
        products: [
            {
                ncd_coop_reg_khadi_gram_prod_id: 0,
                item_name: '',
                yearly_production: '',
                unit: '',
                value_in_rs: ''
            }
        ]
    };

    $scope.education = {
        has_building: "0",
        has_land: "0",
        course_international_participant: "0",
        society_recruit: "0",
        facilities: [],
        level_and_duration_of_course: [],
        land: {
            land_owned: 0,
            land_leased: 0,
            land_allotted: 0,
            land_total: 0
        }
    }

    $scope.Labour = {
        has_building: "0",
        work_allot_state_dist_federation: "0",
        work_guide_state_dist_federation: "0",
        concession_state_gov: "0",
        concession_centre_gov: "0",
        facilities: []
    }

    $scope.housing = {
        has_building: "0",
        has_land: "0",
        loan_facilities: "0",
        facilities: []
    };

    $scope.marketing = {
        has_building: "0",
        has_land: "0",
        has_warehouses: "0",
        sell_the_item: [],
        liecense_to_sell: []
    };

    $scope.Sugar = {
        has_building: "0",
        retail_shops: "0",
        state_government_benefits: "0",
        waste_management: "0",
        loan_facility: "0",
        sugercane_input_provided: "0",
        central_government_benefits: "0",
        product_produced: []
    };

    $scope.Miscellaneous = {
        has_building: "0"
    };

    $scope.Multipurpose = {
        has_building: "0",
        has_storage: "0",
        provide_raw: "0",
        is_operate_retail: "0",
        is_product_sale_out: "0",
        guidance_by_member: "0",
        facilities: [],
        sec_activity: []
    };
    $scope.LiveStock = {
        has_building: "0",
        is_training_provide: "0",
        common_work_place: "0",
        is_work_by_member: "0",
        is_poultry_feed: "0",
        is_collected_from_member: "0",
        is_waste_facility: "0",
        is_operate_retail: "0",
        is_product_sale_out: "0",
        facilities: [],
        type_produce: [],
        type_society: []
    };

    $scope.Transport = {
        has_building: "0",
        member_themself: "0",
        is_user_transport_facility: "0",
    };
    $scope.Sericulture = {
        has_building: "0",
        common_work_place: "0",
        is_work_by_member: "0",
        is_rear_appliance: "0",
        is_waste_facility: "0",
        is_operate_retail: "0",
        is_training_provide: "0",
        facilities: [],
        type_society: []
    }
    $scope.SocialWelfare = {
        has_building: "0",
        has_common: "0",
        is_operate_by_member: "0",
        guidance_by_member: "0",
        is_operate_vehicle: "0",
        facilities: [],
        type_social_culture_activity: []
    }
    $scope.Tourism = {
        has_building: "0",
        guidance_by_member: "0",
        any_resource_taken: "0",
        is_right_vested: "0",
        pool_resource: "0",
        facilities: []
    };
    $scope.Tribal = {
        has_building: "0",
        state_district_federation: "0",
        society_provide_raw_material: "0",
        facilities: []
    }


    $scope.societyItems = [
        { id: 1, name: 'Seeds', hasLicense: false, sellsItem: false },
        { id: 2, name: 'Food grains Procurement on MSP', hasLicense: false, sellsItem: false },
        { id: 3, name: 'PDS Shop', hasLicense: false, sellsItem: false },
        { id: 4, name: 'Fertilizer', hasLicense: false, sellsItem: false },
        { id: 5, name: 'Pesticide', hasLicense: false, sellsItem: false },
        { id: 6, name: 'Agricultural Equipment on Rent', hasLicense: false, sellsItem: false }
    ];

    $scope.$watchGroup(['ncd.address_line', 'ncd.village_code', 'ncd.gram_panchayat_code', 'ncd.block_code', 'ncd.district_code', 'ncd.pincode'], function () {
        $scope.fillAddress();
    });
    $scope.PaidShareCalculate = function () {
        $scope.beeCoop.paid_up_total = 0.00;
        $scope.beeCoop.paid_up_total = parseFloat(((parseFloat($scope.beeCoop.paid_up_members) || 0) + (parseFloat($scope.beeCoop.paid_up_government_bodies) || 0)).toFixed(2));
    }
    $scope.PacsLandAreaCalculate = function () {
        $scope.pacs.land.land_total = 0.00;
        $scope.pacs.land.land_total = parseFloat(((parseFloat($scope.pacs.land.land_owned) || 0) + (parseFloat($scope.pacs.land.land_leased) || 0) + (parseFloat($scope.pacs.land.land_allotted) || 0)).toFixed(2));
    }

    $scope.PaidShareCapitalCalculate = function () {
        $scope.csm.paid_up_total = 0.00;
        $scope.csm.paid_up_total = parseFloat(((parseFloat($scope.csm.paid_up_members) || 0) + (parseFloat($scope.csm.paid_up_government_bodies) || 0)).toFixed(2));
    }
    $scope.MemberCooperativeCalculate = function () {
        $scope.csm.members_of_society = 0.00;
        $scope.csm.members_of_society = parseFloat(((parseFloat($scope.csm.individual_member) || 0) + (parseFloat($scope.csm.institutional_member) || 0)).toFixed(2));
    }

    $scope.AgricultureCooperativeCalculate = function () {
        $scope.aacs.total = 0.00;
        $scope.aacs.total = parseFloat($scope.aacs.individual_member ?? 0) + parseFloat($scope.aacs.institutional_member ?? 0);
    }

    $scope.AgriculturePaidShareCalculate = function () {
        $scope.aacs.paid_up_total = 0.00;
        $scope.aacs.paid_up_total = parseFloat($scope.aacs.paid_up_members ?? 0) + parseFloat($scope.aacs.paid_up_government_bodies ?? 0);
    }

    $scope.ProcessingCooperativeCalculate = function () {
        $scope.crp.total_member = 0.00;
        $scope.crp.total_member = parseFloat($scope.crp.individual_member) + parseFloat($scope.crp.institutional_member);
    }

    $scope.ProcessingPaidShareCalculate = function () {
        $scope.crp.paid_up_total = 0.00;
        $scope.crp.paid_up_total = parseFloat($scope.crp.paid_up_members) + parseFloat($scope.crp.paid_up_government_bodies);
    }

    $scope.HandicraftCoopCalculate = function () {
        $scope.Handicraft.total_members = 0.00;
        $scope.Handicraft.total_members = parseFloat($scope.Handicraft.individual_member ?? 0) + parseFloat($scope.Handicraft.institutional_member ?? 0);
    }

    $scope.HandicraftCoopShareCalculate = function () {
        $scope.Handicraft.paid_up_total = 0.00;
        $scope.Handicraft.paid_up_total = parseFloat($scope.Handicraft.paid_up_members ?? 0) + parseFloat($scope.Handicraft.paid_up_government_bodies ?? 0);
    }

    $scope.HandloomMemberCalculate = function () {
        $scope.handloom.total_members = 0.00;
        $scope.handloom.total_members = parseFloat($scope.handloom.individual_member ?? 0) + parseFloat($scope.handloom.institutional_member ?? 0);
    }

    $scope.HandiloomCoopShareCalculate = function () {
        $scope.handloom.paid_up_total = 0.00;
        $scope.handloom.paid_up_total = parseFloat($scope.handloom.paid_up_members ?? 0) + parseFloat($scope.handloom.paid_up_government_bodies ?? 0);
    }

    $scope.KhadiGramMemberCalculate = function () {
        $scope.KhadiGram.total_member = 0.00;
        $scope.KhadiGram.total_member = parseFloat($scope.KhadiGram.individual_member) + parseFloat($scope.KhadiGram.institutional_member);
    }

    $scope.KhadiGramCoopShareCalculate = function () {
        $scope.KhadiGram.paid_up_total = 0.00;
        $scope.KhadiGram.paid_up_total = parseFloat($scope.KhadiGram.paid_up_members ?? 0) + parseFloat($scope.KhadiGram.paid_up_government_bodies ?? 0);
    }

    $scope.EducationLandCalculate = function () {
        $scope.education.land.land_total = parseFloat($scope.education.land.land_owned ?? 0) + parseFloat($scope.education.land.land_leased ?? 0) + parseFloat($scope.education.land.land_allotted ?? 0);
    }

    $scope.EducationMemberDetailCalculate = function () {
        $scope.education.members_of_society = 0.00;
        $scope.education.members_of_society = parseFloat($scope.education.individual_member ?? 0) + parseFloat($scope.education.institutional_member ?? 0);
    }

    $scope.EducationShareCapitalCalculate = function () {
        $scope.education.paid_up_total = 0.00;
        $scope.education.paid_up_total = parseFloat($scope.education.paid_up_members ?? 0) + parseFloat($scope.education.paid_up_government_bodies ?? 0);
    }

    $scope.SocialWelfareCoopShareCalculate = function () {
        $scope.SocialWelfare.paid_up_total = 0.00;
        $scope.SocialWelfare.paid_up_total = parseFloat($scope.SocialWelfare.paid_up_members ?? 0) + parseFloat($scope.SocialWelfare.paid_up_government_bodies ?? 0);
    }
    $scope.IsEducationLandToggle = function () {
        $("input, select, textarea").removeClass("invalid-field");
        if ($scope.education.has_land == 0) {
            $scope.education.land.land_owned = 0;
            $scope.education.land.land_leased = 0;
            $scope.education.land.land_allotted = 0;
            $scope.education.land.land_total = 0;
        }
    }

    $scope.HousingLandCalculate = function () {
        $scope.housing.land.land_total = 0.00;
        $scope.housing.land.land_total = parseFloat($scope.housing.land.land_owned ?? 0) + parseFloat($scope.housing.land.land_leased ?? 0) + parseFloat($scope.housing.land.land_allotted ?? 0);
    }

    $scope.HousingSharePaidCalculate = function () {
        $scope.housing.paid_up_total = 0.00;
        $scope.housing.paid_up_total = parseFloat($scope.housing.paid_up_members ?? 0) + parseFloat($scope.housing.paid_up_government_bodies ?? 0);
    }

    $scope.MarketingLandCalculate = function () {
        $scope.marketing.land.land_total = 0.00;
        $scope.marketing.land.land_total = parseFloat($scope.marketing.land.land_owned ?? 0) + parseFloat($scope.marketing.land.land_leased ?? 0) + parseFloat($scope.marketing.land.land_allotted ?? 0);
    }

    $scope.MarketingSharePaidCalculate = function () {
        $scope.marketing.paid_up_total = 0.00;
        $scope.marketing.paid_up_total = parseFloat($scope.marketing.paid_up_members ?? 0) + parseFloat($scope.marketing.paid_up_government_bodies ?? 0);
    };


    $scope.SugarSharePaidCalculate = function () {
        $scope.Sugar.paid_up_total = 0.00;
        $scope.Sugar.paid_up_total = parseFloat($scope.Sugar.paid_up_members ?? 0) + parseFloat($scope.Sugar.paid_up_government_bodies ?? 0);
    }

    $scope.SugarAreaCalculate = function () {
        $scope.Sugar.total_area = 0.00;
        $scope.Sugar.total_area = parseFloat($scope.Sugar.build_up_area ?? 0) + parseFloat($scope.Sugar.open_land_area ?? 0);
    };

    $scope.MiscellaneousShareCalculate = function () {
        $scope.Miscellaneous.paid_up_total = 0.00;
        $scope.Miscellaneous.paid_up_total = parseFloat($scope.Miscellaneous.paid_up_members ?? 0) + parseFloat($scope.Miscellaneous.paid_up_government_bodies ?? 0);
    };

    $scope.MultipurposeSharePaidCalculate = function () {
        $scope.Multipurpose.paid_up_total = 0.00;
        $scope.Multipurpose.paid_up_total = parseFloat($scope.Multipurpose.paid_up_members ?? 0) + parseFloat($scope.Multipurpose.paid_up_government_bodies ?? 0);
    };

    $scope.LabourSharePaidCalculate = function () {
        $scope.Labour.paid_up_total = 0.00;
        $scope.Labour.paid_up_total = parseFloat($scope.Labour.paid_up_members ?? 0) + parseFloat($scope.Labour.paid_up_government_bodies ?? 0);
    };

    $scope.TransportCoopCalculate = function () {
        $scope.Transport.members_of_society = 0.00;
        $scope.Transport.members_of_society = parseFloat($scope.Transport.individual_member ?? 0) + parseFloat($scope.Transport.institutional_member ?? 0);
    }

    $scope.TransportSharePaidCalculate = function () {
        $scope.Transport.paid_up_total = 0.00;
        $scope.Transport.paid_up_total = parseFloat($scope.Transport.paid_up_members ?? 0) + parseFloat($scope.Transport.paid_up_government_bodies ?? 0);
    };

    $scope.TourismMemberCoopCalculate = function () {
        $scope.Tourism.members_of_society = 0.00;
        $scope.Tourism.members_of_society = parseFloat($scope.Tourism.individual_member ?? 0) + parseFloat($scope.Tourism.institutional_member ?? 0);
    };

    $scope.TourismCoopSharePaidCalculate = function () {
        $scope.Tourism.paid_up_total = 0.00;
        $scope.Tourism.paid_up_total = parseFloat($scope.Tourism.paid_up_members ?? 0) + parseFloat($scope.Tourism.paid_up_government_bodies ?? 0);
    };

    $scope.TribalCoopSharePaidCalculate = function () {
        $scope.Tribal.paid_up_total = 0.00;
        $scope.Tribal.paid_up_total = parseFloat($scope.Tribal.paid_up_members ?? 0) + parseFloat($scope.Tribal.paid_up_government_bodies ?? 0);
    };
    //get params values
    var params = new URLSearchParams(window.location.search);
    let cooperativeId = params.get("cooperativeId");
    if (params.get("ncdId") != null) {
        cooperativeId = params.get("ncdId");
    }
    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get("/Translations/Translation_EN.json").success(function (data, status) {
                $scope.translation = data;
            }).error(function (data, status) {
                alert("Error while translation");
            });
        }
        else {
            $http.get("/Translations/Translation_HI.json").success(function (data, status) {
                $scope.translation = data;
            }).error(function (data, status) {
                alert("Error while translation");
            });
        }


        $scope.GetDistrictByStatecode(8);
        $scope.GetNCDAuditCategories();
        $scope.GetNCDRegistrationAuthorities();
        $scope.GetSocietyFunctionalStatus();
        $scope.GetGramPanchayatByDistrictBlock();
        $scope.GetNcdDesignations();
        $scope.GetFinancialYears();
        $scope.GetBeeBeehiveTypes();
        //Consumer Cooperative Society Details
        $scope.GetAreaOfOperations($scope.sectorDetail.operation_area_location);
        $scope.GetCategoryUrbanLocalList();
        $scope.GetsectorOperationDetails();
        $scope.GetNcdOfficeBuildingTypes();
        if (cooperativeId && cooperativeId.trim() !== "") {
            $scope.GetDetailsByCooperative(cooperativeId);
            $scope.isRegistrationTxt = true;
            $scope.isShowRemoveBtn = true;
            $scope.isShowforEdit = 0;
            $scope.isDisabled = true;
            $scope.IsCooperativeNameTxt = false;
        }
    };

    $scope.BankTypes = [];
    $scope.BankTypes = [{ value: 0, label: 'Cooperative Bank' }, { value: 1, label: 'Commercial Bank' }];
    $scope.societyTypeList = [{ id: 1, name: "Agriculture & Allied" }, { id: 2, name: "Non-Agriculture" }]
    $scope.FilteredNCDSectorDetails = [];
    // called when Credit/Non-Credit changes
    $scope.filterPrimaryActivity = function (sector_of_operation_type) {
        $("input, select, textarea").removeClass("invalid-field");
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetSectorDetailsByIsCredit?actionName=GetSectorByCredit&sectorId=" + sector_of_operation_type).success(function (data) {
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
                    $scope.fillAddress();
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
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
                    $scope.fillAddress();
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
                    $scope.fillAddress();
                }
                $(".se-pre-con").fadeOut("slow");
            })
            .error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.GetGramPanchayatByDistrictBlock = function (districtcode, blockcode) {
        // District is mandatory
        if (!districtcode) {
            $scope.NCDGramPanchayatDetails = [];
            return;
        }
        $(".se-pre-con").fadeIn("slow");
        var url = SiteUrl + "/NCDService/GetGramPanchayatByStateDistrictBlock?districtcode=" + districtcode;
        if (statecode) { url += "&statecode=" + statecode; }
        if (blockcode) { url += "&blockcode=" + blockcode; }

        $http.get(url).success(function (data, status) {
            if (data.Result.Data != null && data.Result.Data.NCDGramPanchayatDetail != null) {
                $scope.NCDGramPanchayatDetails = JSON.parse(data.Result.Data.NCDGramPanchayatDetail);
            }
            $(".se-pre-con").fadeOut("slow");
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
        });
    };


    //Fetch audit categories
    $scope.GetNCDAuditCategories = function () {
        $scope.NCDAuditCategories = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetNCDAuditCategories").then(function (response) {
            $(".se-pre-con").fadeOut("slow");
            if (response.data.Result.Status == "200") {
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

    // Retrieve Registration Authorities details (updated on 30-04-2026)
    $scope.GetNCDRegistrationAuthorities = function () {
        $scope.NCDRegistrationAuthorities = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetNCDRegistrationAuthorities")
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data && response.data.Result && response.data.Result.Status == "200") {
                    if (response.data.Result.Data.NCDRegistrationAuthorityDetail) {
                        $scope.NCDRegistrationAuthorities = JSON.parse(response.data.Result.Data.NCDRegistrationAuthorityDetail);
                    }
                } else {
                    console.error("Error fetching registration authorities:",
                        response.data && response.data.Result ? response.data.Result.Message : "Invalid response format");
                }
            }, function (error) {
                $(".se-pre-con").fadeOut("slow");
                console.error("HTTP error while fetching registration authorities:", error);
            });
    };

    // Retrieve society functional status list  (updated on 30-04-2026)
    $scope.GetSocietyFunctionalStatus = function () {
        $scope.SocietyFunctionalStatusList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetSocietyFunctionalStatus")
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data && response.data.Result && response.data.Result.Status == "200") {
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

    // Retrieve NCD designation list  (updated on 30-04-2026)
    $scope.GetNcdDesignations = function () {
        $scope.NcdDesignationsList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetNcdDesignations")
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data && response.data.Result && response.data.Result.Status == "200") {
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

    // Retrieve Financial Years list  (updated on 30-04-2026)
    $scope.GetFinancialYears = function () {
        $scope.FinancialYearList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetFinancialYears")
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data && response.data.Result && response.data.Result.Status == "200") {
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

    $scope.filterBankType = function (bankType) {
        let arr = $scope.toArray(bankType);
        $scope.ncd.bank_type = arr.join(",");
        $scope.NCDCoopBanks = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetNCDCooperativeSocietyBanks?bankTypeId=" + $scope.ncd.bank_type ?? '0')
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data && response.data.Result && response.data.Result.Status == "200") {
                    let list = JSON.parse(response.data.Result.Data.NCDCoopBankDetail);
                    let selectedIds = $scope.toArray($scope.ncd.cooperative_society_bank_id);
                    $scope.NCDCoopBanks = list.map(x => ({
                        id: x.id.toString(),
                        bank_name: x.bank_name,
                        selected: selectedIds.includes(x.id.toString())
                    }));

                    $scope.selectAllBanks = $scope.NCDCoopBanks.length && $scope.NCDCoopBanks.every(x => x.selected);
                }
            });
    };


    $scope.ncd = $scope.ncd || {};
    $scope.GetBeeBeehiveTypes = function () {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/NcdData/GetBeeBeehiveTypes").then(function (response) {
            var data = response.data;
            if (data && data.Result && data.Result.Status === "200") {
                if (data.Result.Data && data.Result.Data.BeeBeehiveTypes) {
                    $scope.NCDBeeBeehiveTypes =
                        JSON.parse(data.Result.Data.BeeBeehiveTypes);
                } else {
                    $scope.NCDBeeBeehiveTypes = [];
                }

            } else {
                console.error("Error fetching BeeBeehive Types:", data?.Result?.Message || "Invalid response format");
                $scope.NCDBeeBeehiveTypes = [];
            }

        }).catch(function (error) {
            console.error("HTTP error while fetching BeeBeehive Types:", error);
            $scope.NCDBeeBeehiveTypes = [];
        }).finally(function () { $(".se-pre-con").fadeOut("slow"); });
    };

    $scope.GetNcdOfficeBuildingTypes = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetNcdOfficeBuildingTypes")
            .then(function (response) {
                $(".se-pre-con").fadeOut("slow");
                if (response.data.Result.Status == "200") {
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
        $scope.ncd.sectorListData = [];
        $scope.sectorDetail.area_of_operation_id = "0";
        $scope.areaofOperationList = [];
        if (!operation_area_location) {
            $scope.areaofOperationList = [];
            return;
        }

        $scope.selectedArea = operation_area_location == 1 ? "Urban" : operation_area_location == 2 ? "Rural" : "Both";
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetAreaOperationDetails?operation_area_location=" + operation_area_location).then(function (response) {
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null) {
                $scope.areaofOperationList = JSON.parse(response.data.Result.Data.OperationDetails);
            } else { $scope.areaofOperationList = []; }
            if (cooperativeId == null || cooperativeId == "") {
                if (operation_area_location == 1) { // Urban
                    $scope.ncd.sectorListData.push({
                        AreaofOperationUrbanDetailsId: 0,
                        AreaofOperationRuralDetailsId: 0,
                        operation_area_location: operation_area_location,
                        local_body_type_code: '',
                        local_body_code: '',
                        locality_ward_code: '',
                        // Keep urban fields empty
                        district_code: '',
                        block_code: '',
                        panchayat_code: '',
                        village_code: '',
                        area_of_operation_id: $scope.sectorDetail.area_of_operation_id,
                        is_deleted: "0"
                    });
                }
                else if (operation_area_location == 2) { // Rural
                    $scope.ncd.sectorListData.push({
                        AreaofOperationUrbanDetailsId: 0,
                        AreaofOperationRuralDetailsId: 0,
                        operation_area_location: operation_area_location,
                        district_code: '',
                        block_code: '',
                        panchayat_code: '',
                        village_code: '',
                        // Keep rural fields empty
                        local_body_type_code: '',
                        local_body_code: '',
                        locality_ward_code: '',
                        area_of_operation_id: $scope.sectorDetail.area_of_operation_id,
                        is_deleted: "0"
                    });
                }
                else if (operation_area_location == 3) { // Both Rural + Urban
                    $scope.ncd.sectorListData.push({
                        AreaofOperationUrbanDetailsId: 0,
                        AreaofOperationRuralDetailsId: 0,
                        operation_area_location: operation_area_location,
                        district_code: '',
                        block_code: '',
                        panchayat_code: '',
                        village_code: '',
                        // Keep both fields empty
                        local_body_type_code: '',
                        local_body_code: '',
                        locality_ward_code: '',
                        area_of_operation_id: $scope.sectorDetail.area_of_operation_id,
                        is_deleted: "0"
                    });
                }

                $scope.ncd.operation_area_locationNew = 0;
            }
            else {
                $scope.ncd.operation_area_locationNew = operation_area_location;
                if ($scope.ncd.operation_area_locationOld == $scope.ncd.operation_area_locationNew) {
                    $scope.ncd.sectorListData = $scope.sectorListDataOld;
                }
                else {
                    if (operation_area_location == 1 && $scope.ncd.operation_area_locationNew != 0) { // Urban
                        $scope.ncd.sectorListData.push({
                            AreaofOperationUrbanDetailsId: 0,
                            AreaofOperationRuralDetailsId: 0,
                            operation_area_location: operation_area_location,
                            local_body_type_code: '',
                            local_body_code: '',
                            locality_ward_code: '',
                            // Keep uran fields empty
                            district_code: '',
                            block_code: '',
                            panchayat_code: '',
                            village_code: '',
                            area_of_operation_id: $scope.sectorDetail.area_of_operation_id,
                            is_deleted: "0"
                        });
                    }
                    else if (operation_area_location == 2 && $scope.ncd.operation_area_locationNew != 0) { // Rural
                        $scope.ncd.sectorListData.push({
                            AreaofOperationUrbanDetailsId: 0,
                            AreaofOperationRuralDetailsId: 0,
                            operation_area_location: operation_area_location,
                            district_code: '',
                            block_code: '',
                            panchayat_code: '',
                            village_code: '',
                            // Keep rural fields empty
                            local_body_type_code: '',
                            local_body_code: '',
                            locality_ward_code: '',
                            area_of_operation_id: $scope.sectorDetail.area_of_operation_id,
                            is_deleted: "0"
                        });
                    }
                    else if (operation_area_location == 3 && $scope.ncd.operation_area_locationNew != 0) { // Both Rural + Urban
                        $scope.ncd.sectorListData.push({
                            AreaofOperationUrbanDetailsId: 0,
                            AreaofOperationRuralDetailsId: 0,
                            operation_area_location: operation_area_location,
                            district_code: '',
                            block_code: '',
                            panchayat_code: '',
                            village_code: '',
                            // Keep both fields empty
                            local_body_type_code: '',
                            local_body_code: '',
                            locality_ward_code: '',
                            area_of_operation_id: $scope.sectorDetail.area_of_operation_id,
                            is_deleted: "0"
                        });
                    }
                }
            }

            $(".se-pre-con").fadeOut("slow");
        }, function (error) {
            console.error("Error loading Area Of Operations:", error);
            $scope.areaofOperationList = [];
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

        $scope.ncdm.m_total = ($scope.ncdm.m_general ?? 0) +
            ($scope.ncdm.m_sc ?? 0) +
            ($scope.ncdm.m_st ?? 0) +
            ($scope.ncdm.m_obc ?? 0);
        $scope.ncdm.f_total = ($scope.ncdm.f_general ?? 0) +
            ($scope.ncdm.f_sc ?? 0) +
            ($scope.ncdm.f_st ?? 0) +
            ($scope.ncdm.f_obc ?? 0);
        $scope.ncdm.t_total = ($scope.ncdm.t_general ?? 0) +
            ($scope.ncdm.t_sc ?? 0) +
            ($scope.ncdm.t_st ?? 0) +
            ($scope.ncdm.t_obc ?? 0);

        $scope.ncdm.total_general = ($scope.ncdm.m_general ?? 0) +
            ($scope.ncdm.f_general ?? 0) +
            ($scope.ncdm.t_general ?? 0);
        $scope.ncdm.total_obc = ($scope.ncdm.m_obc ?? 0) +
            ($scope.ncdm.f_obc ?? 0) +
            ($scope.ncdm.t_obc ?? 0);
        $scope.ncdm.total_sc = ($scope.ncdm.m_sc ?? 0) +
            ($scope.ncdm.f_sc ?? 0) +
            ($scope.ncdm.t_sc ?? 0);
        $scope.ncdm.total_st = ($scope.ncdm.m_st ?? 0) +
            ($scope.ncdm.f_st ?? 0) +
            ($scope.ncdm.t_st ?? 0);

        $scope.ncdm.total_total = ($scope.ncdm.total_general ?? 0) +
            ($scope.ncdm.total_sc ?? 0) +
            ($scope.ncdm.total_st ?? 0) +
            ($scope.ncdm.total_obc ?? 0);

    };
    $scope.GetCooperativeDataByRegistrationNo = function () {
        if (!$scope.ncd.registration_number) {
            alert("Please Enter Registration No.");
            return;
        }
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetCooperativeDataByRegistrationNo?RegistrationNo=" + $scope.ncd.registration_number).success(function (data, status) {
            let basicDetailsList = JSON.parse(data?.Result?.Data?.BasicDetails);
            if (Array.isArray(basicDetailsList) && basicDetailsList.length > 0) {
                $scope.ncd = basicDetailsList[0] || {};
                $scope.ncd.operation_area_location = ($scope.ncd.operation_area_location == 0 || $scope.ncd.operation_area_location == null) ? 2 : $scope.ncd.operation_area_location;
                $scope.ncd.location_of_head_quarter = ($scope.ncd.location_of_head_quarter == 0 || $scope.ncd.location_of_head_quarter == null) ? 2 : $scope.ncd.location_of_head_quarter;
                $scope.ncd.cooperative_society_bank_id = ($scope.ncd.cooperative_society_bank_id ?? '') + '';
                $scope.ncd.bank_type = $scope.ncd.bank_type ?? '';
                $scope.ncd.deregistered = ($scope.ncd.deregistered == undefined || $scope.ncd.deregistered == null) ? "0" : $scope.ncd.deregistered;
                $scope.ncd.bank_typeList = $scope.ncd.bank_type ? $scope.ncd.bank_type.split(',').map(x => parseInt(x.trim())) : [];
                if ($scope.ncd.date_registration) {
                    $scope.ncd.date_registration = new Date($scope.convertDate($scope.ncd.date_registration));
                }
                $scope.GetAreaOfOperations($scope.ncd.operation_area_location);
                if ($scope.ncd.is_coastal == 1) {
                    $scope.GetWaterBodyTypeList();
                }
                $scope.ncdm = JSON.parse(data?.Result?.Data?.MemberDetails) || [];
                $scope.isRegistrationTxt = false;
                $scope.isShowRemoveBtn = true;
                $scope.isShowforEdit = 2;
                $scope.IsCooperativeNameTxt = false;

            } else {
                alert("Record not found or already exists");
            }

            $(".se-pre-con").fadeOut("slow");
        })
            .error(function () {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.IsEnableRegistrationNo = function () {
        $scope.isRegistrationTxt = false;
        $scope.isShowRemoveBtn = false;
        $scope.isShowforEdit = 1;
        $scope.getDefaultNcdDetails();
        $scope.GetAreaOfOperations($scope.ncd.operation_area_location);
        $scope.FilteredNCDSectorDetails = [];
    };

    $scope.getDefaultNcdDetails = function () {
        $scope.ncd.registration_number = '';
        $scope.ncd.cooperative_society_name = '';
        $scope.ncd.sector_of_operation_type = '0';
        $scope.ncd.sector_of_operation = '0';
        $scope.ncd.registration_authoritie_id = '0';
        $scope.ncd.functional_status = '0';
        $scope.ncd.state_code = '8';
        $scope.ncd.contact_person = '';
        $scope.ncd.email = '';
        $scope.ncd.pincode = '';
        $scope.ncd.mobile = '';
        $scope.ncd.operation_area_location = "2";
        $scope.ncd.location_of_head_quarter = "2";
    };

    $scope.SaveData = function () {
        //if (cooperativeId != null && cooperativeId.length > 10) {
        //    alert("Edit and update functionality is not available, we are working on it this option will be available soon.");
        //    return;
        //}
        if (!confirm('Are you want to save data?'))
            return;
        const mainRules = [
            { object: 'ncd', field: 'registration_number', message: 'Please enter Registration Number!' },
            { object: 'ncd', field: 'cooperative_society_name', message: 'Please enter Cooperative Society Name!' },
            { object: 'ncd', field: 'sector_of_operation_type', message: 'Please select Sector of Operation!', invalidValues: ['0'] },
            { object: 'ncd', field: 'sector_of_operation', message: 'Please select Primary Activity!', invalidValues: ['0'] },
            { object: 'ncd', field: 'registration_authoritie_id', message: 'Please select Registration Authority!', invalidValues: ['0'] },
            { object: 'ncd', field: 'date_registration', message: "Please select Date of Registration or it can't be a future date!" },
            { object: 'ncd', field: 'functional_status', message: 'Please select Functional Status!', invalidValues: ['0'] },
            { object: 'ncd', field: 'operation_area_location', message: 'Please select Location of Registered Office!', invalidValues: ['0'] },
            { object: 'ncd', field: 'district_code', message: 'Please select District!', invalidValues: ['0'] },
            { object: 'ncd', field: 'contact_person', message: 'Please enter Contact Person!' },
            { object: 'ncd', field: 'designation', message: 'Please select Designation!', invalidValues: ['0'] }, {
                object: 'ncd', field: 'mobile', message: 'Enter a valid mobile number; repeated digits or invalid numbers are not allowed.!', minLength: 10, maxLength: 10, custom: function (value) {
                    if (!value) return false;
                    const validFormat = /^[6-9]\d{9}$/.test(value);
                    const notRepeated = !/^(\d)\1{9}$/.test(value);
                    return validFormat && notRepeated;
                }
            },
            //{ object: 'ncd', field: 'email', message: 'Please enter email address!' },
            {
                object: 'ncd',
                field: 'email',
                message: 'Please enter valid email address!',
                custom: function (value) {
                    if (!value) return false;
                    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
                    return emailRegex.test(value);
                }
            },
            { object: 'ncd', field: 'is_coastal', message: 'Please select Water Body!' },
            { object: 'ncd', field: 'address_line', message: 'Please enter address line!' },
            { object: 'ncd', field: 'full_address', message: 'Please enter correspondence address!' },
            { object: 'ncd', field: 'pincode', message: 'Please enter valid pincode!', minLength: 6, maxLength: 6 },
            { object: 'sectorDetail', field: 'area_of_operation_id', message: 'Please select Area of Operation!', invalidValues: ['0'] },
            { object: 'ncd', field: 'members_of_society', message: 'Please enter members!' },
            { object: 'ncdm', field: 'm_general', message: 'Please enter male general!' },
            { object: 'ncdm', field: 'm_sc', message: 'Please enter male sc!' },
            { object: 'ncdm', field: 'm_st', message: 'Please enter male st!' },
            { object: 'ncdm', field: 'm_obc', message: 'Please enter male obc!' },
            { object: 'ncdm', field: 'f_general', message: 'Please enter female general!' },
            { object: 'ncdm', field: 'f_sc', message: 'Please enter female sc!' },
            { object: 'ncdm', field: 'f_st', message: 'Please enter female st!' },
            { object: 'ncdm', field: 'f_obc', message: 'Please enter female obc!' },
            { object: 'ncdm', field: 't_general', message: 'Please enter transgender general!' },
            { object: 'ncdm', field: 't_sc', message: 'Please enter transgender sc!' },
            { object: 'ncdm', field: 't_st', message: 'Please enter transgender st!' },
            { object: 'ncdm', field: 't_obc', message: 'Please enter transgender obc!' },
        ];
        if ($scope.ncd.financial_audit == 1) { mainRules.push({ object: 'ncd', field: 'annual_turnover', message: 'Please enter Annual Turnover!' }); }
        if ($scope.ncd.financial_audit == 1) { mainRules.push({ object: 'ncd', field: 'audit_complete_year', message: 'Please select years of latest audit complete!', invalidValues: [0] }, { object: 'ncd', field: 'category_audit', message: 'Please select category of audit!', invalidValues: [0] }); }
        if ($scope.ncd.is_dividend_paid == 1) { mainRules.push({ object: 'ncd', field: 'dividend_rate', message: 'Please enter dividend rate!' }); }
        if ($scope.ncd.financial_audit == 1) {
            if ($scope.ncd.is_profit_making == 1) {
                mainRules.push({ object: 'ncd', field: 'annualPayment', message: 'Please enter annual loss!' });
            } else {
                mainRules.push({ object: 'ncd', field: 'annualPayment', message: 'Please enter annual profit!' });
            }
        }
        if ($scope.ncd.is_msme == 1) { mainRules.push({ object: 'ncd', field: 'udyam_no', message: 'Please enter udyam no!' }); }
        if ($scope.ncd.location_of_head_quarter == 1) {
            mainRules.push({ object: 'ncd', field: 'urban_local_body_type_code', message: 'Please select category of urban local body!', invalidValues: ['0'] },
                { object: 'ncd', field: 'urban_local_body_code', message: 'Please select urban local body!', invalidValues: ['0'] },
                { object: 'ncd', field: 'locality_ward_code', message: 'Please select locality or ward!', invalidValues: ['0'] });
        }
        if ($scope.ncd.location_of_head_quarter == 2) {
            mainRules.push({ object: 'ncd', field: 'block_code', message: 'Please select block !', invalidValues: ['0'] },
                { object: 'ncd', field: 'gram_panchayat_code', message: 'Please select pram panchayat !', invalidValues: ['0'] },
                { object: 'ncd', field: 'village_code', message: 'Please select village!', invalidValues: ['0'] });
        }
        if ($scope.ncd.is_coastal == 1) { mainRules.push({ object: 'ncd', field: 'water_body_type_id', message: 'Please select water type !', invalidValues: ['0'] }) }

        if ($scope.ncd.sector_of_operation == 77) {
            if ($scope.aacs.has_building == "1") { mainRules.push({ object: 'aacs', field: 'building_type', message: 'Please select aacs building type !', invalidValues: ['0'] }); }
            if (!$scope.aacs.type_society || $scope.aacs.type_society.length === 0) {
                mainRules.push({ object: 'aacs', field: 'type_society', message: 'Please select aacs primary activity !' });
            }
            mainRules.push({ object: 'aacs', field: 'individual_member', message: 'Please enter individual member !' },
                { object: 'aacs', field: 'institutional_member', message: 'Please enter institutional member !' },
                { object: 'aacs', field: 'total_deposit', message: 'Please enter total deposit!' });
        }
        else if ($scope.ncd.sector_of_operation == 31) {
            mainRules.push({ object: 'crp', field: 'individual_member', message: 'Please enter individual member !' },
                { object: 'crp', field: 'institutional_member', message: 'Please enter institutional member !' },
                { object: 'crp', field: 'total_member', message: 'Please enter total_member!' },
                { object: 'crp', field: 'total_deposit', message: 'Please enter total deposit!' },
                { object: 'crp', field: 'type_society', message: 'Please select type of society!', invalidValues: ['0'] });
            if ($scope.crp.has_building == "1") { mainRules.push({ object: 'crp', field: 'building_type', message: 'Please select crp building type !', invalidValues: ['0'] }); }
            if ($scope.crp.processing_unit == "1") { mainRules.push({ object: 'crp', field: 'processing_unit_number', message: 'Please enter processing unit number!' }); }
            if ($scope.crp.operate_shops == "1") { mainRules.push({ object: 'crp', field: 'operate_shops_number', message: 'Please enter operate shops number!' }); }
        }
        else if ($scope.ncd.sector_of_operation == 79) {
            if (!$scope.beeCoop.type_bee || $scope.beeCoop.type_bee.length === 0) { mainRules.push({ object: 'beeCoop', field: 'type_bee', message: 'Please select bees Rearing the Cooperative Society!' }); }
            if ($scope.beeCoop.has_building == "1") { mainRules.push({ object: 'beeCoop', field: 'building_type', message: 'Please select Bee farming building type !', invalidValues: ['0'] }); }
            mainRules.push({ object: 'beeCoop', field: 'total_deposit', message: 'Please enter total deposit!' },);
            if ($scope.beeCoop.common_yard == "1") { mainRules.push({ object: 'beeCoop', field: 'no_of_behives', message: 'Please enter no of behives!' }); }
            if (!$scope.beeCoop.type_product || $scope.beeCoop.type_product.length === 0) { mainRules.push({ object: 'beeCoop', field: 'type_product', message: 'Please select type of product!' }); }
            if ($scope.beeCoop.is_operate_retail == "1") { mainRules.push({ object: 'beeCoop', field: 'no_of_retail', message: 'Please enter no of retail!' }); }
        }
        else if ($scope.ncd.sector_of_operation == 80) {
            if ($scope.consumer.has_building == "1") { mainRules.push({ object: 'consumer', field: 'building_type', message: 'Please select consumer building type !', invalidValues: ['0'] }); }
            if ($scope.consumer.has_store == "1") { mainRules.push({ object: 'consumer', field: 'no_of_outlets', message: 'Please enter no of outlets!' }); }

            mainRules.push({ object: 'consumer', field: 'total_deposit', message: 'Please enter total deposit!' },);
        }
        else if ($scope.ncd.sector_of_operation == 18) {
            if ($scope.tsd.has_building == "1") { mainRules.push({ object: 'tsd', field: 'building_type', message: 'Please select credit & Thrift building type !', invalidValues: ['0'] }); }
            mainRules.push({ object: 'tsd', field: 'total_deposit', message: 'Please enter total deposit!' },);
            if (!$scope.tsd.facilities || $scope.tsd.facilities.length === 0) { mainRules.push({ object: 'tsd', field: 'facilities', message: 'Please select facilities!' }); }
        }
        else if ($scope.ncd.sector_of_operation == 9) {
            mainRules.push({ object: 'dairy', field: 'milk_collection', message: 'Please enter milk collection!' },);
            if ($scope.dairy.credit_facility == "1") { mainRules.push({ object: 'dairy', field: 'credit_provided', message: 'Please enter credit provided!', invalidValues: ['0'] }); }
            if ($scope.dairy.milk_collection_unit == "1") { mainRules.push({ object: 'dairy', field: 'milk_collection_capicity', message: 'Please enter milk collection capicity!' }); }
            if ($scope.dairy.is_bank_mitra == "1") { mainRules.push({ object: 'dairy', field: 'bank_mitra_details', message: 'Please enter bank mitra details!' }); }
            if ($scope.dairy.is_micro_atm == "1") { mainRules.push({ object: 'dairy', field: 'micro_atm_details', message: 'Please enter micro bank details!' }); }
            mainRules.push({ object: 'dairy', field: 'total_deposit', message: 'Please enter total deposit!' },);
            mainRules.push({ object: 'dairy', field: 'pack_total_outstanding_loan', message: 'Please enter total outstading loan!' },);
            mainRules.push({ object: 'dairy', field: 'other_facility', message: 'Please enter other facility!' });
        }
        else if ($scope.ncd.sector_of_operation == 84) {
            mainRules.push({ object: 'education', field: 'type_society', message: 'Please select type of society!', invalidValues: ['0'] },);
            if ($scope.education.type_society == 1) {
                mainRules.push({ object: 'education', field: 'level_of_edu', message: 'Please select level of education course!', invalidValues: ['0'] });
            } else if ($scope.education.type_society == 2) {
                mainRules.push({ object: 'education', field: 'duration_of_course', message: 'Please select level of education course!', invalidValues: ['0'] });
            }
            else if ($scope.education.type_society == 3) {
                if (!$scope.education.level_and_duration_of_course || $scope.education.level_and_duration_of_course === 0) {
                    mainRules.push({ object: 'education', field: 'level_and_duration_of_course', message: 'Please select level of education course!', invalidValues: ['0'] });
                }
            }
            if ($scope.education.has_building == "1") { mainRules.push({ object: 'education', field: 'building_type', message: 'Please select building type!', invalidValues: ['0'] }); }
            mainRules.push({ object: 'education', field: 'individual_member', message: 'please enter individual member' },
                { object: 'education', field: 'institutional_member', message: 'please enter institutional member' },
                { object: 'education', field: 'authorised_share', message: 'please enter authorised share' },
                { object: 'education', field: 'course_in_audit', message: 'please enter course in audit' },
                { object: 'education', field: 'stu_in_audit', message: 'please enter student in audit' },
                { object: 'education', field: 'training_course_in_audit', message: 'please enter courses conducted in the audited year' },
                { object: 'education', field: 'participants_in_audit', message: 'please enter attended training in the audited year' });
            if ($scope.education.course_international_participant == "1") {
                mainRules.push(
                    { object: 'education', field: 'no_of_training_course', message: 'please enter no of training course' },
                    { object: 'education', field: 'attended_training', message: 'please enter attended training' });
            }
            if ($scope.education.society_recruit == "1") {
                mainRules.push(
                    { object: 'education', field: 'no_regular_faculty', message: 'please enter no of regular faculty' },
                    { object: 'education', field: 'no_other_faculty', message: 'please enter number of visiting faculties:' });
            }
        }
        else if ($scope.ncd.sector_of_operation == 10) {
            mainRules.push({ object: 'Fishery', field: 'annual_fish_catch', message: 'Please enter annual total fish catch!' },
                { object: 'Fishery', field: 'total_deposit', message: 'Please enter total deposit!' },
                { object: 'Fishery', field: 'other_facility', message: 'Please enter other facility!' });
            if ($scope.Fishery.credit_facility == "1") { mainRules.push({ object: 'Fishery', field: 'total_credit_provided', message: 'Please enter total credit provided in the financial year!' }); }
            if ($scope.Fishery.is_fpo_fisheries == "1") { mainRules.push({ object: 'Fishery', field: 'fpo_fisheries_details', message: 'Please enter FPO details!' }); }
        }
        else if ($scope.ncd.sector_of_operation == 14) {
            if ($scope.Handicraft.has_building == "1") { mainRules.push({ object: 'Handicraft', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
            mainRules.push({ object: 'Handicraft', field: 'individual_member', message: 'Please enter individual member!' },
                { object: 'Handicraft', field: 'institutional_member', message: 'Please enter institutional member!' },
                { object: 'Handicraft', field: 'authorised_share', message: 'Please enter authorised share!' },
                { object: 'Handicraft', field: 'total_deposit', message: 'Please enter total deposit!' });
            if ($scope.Handicraft.common_work_place == "1") { mainRules.push({ object: 'Handicraft', field: 'workplace_operate', message: 'Please enter work places or manufacturing units operated by cooperative society!' }) }
            if ($scope.Handicraft.is_operate_retail == "1") { mainRules.push({ object: 'Handicraft', field: 'no_of_retail', message: 'Please enter no of retail!' }) }
        }
        else if ($scope.ncd.sector_of_operation == 13) {
            if ($scope.handloom.has_building == "1") { mainRules.push({ object: 'handloom', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
            mainRules.push({ object: 'handloom', field: 'individual_member', message: 'Please enter individual member!' },
                { object: 'handloom', field: 'institutional_member', message: 'Please enter institutional member!' },
                { object: 'handloom', field: 'authorised_share', message: 'Please enter authorised share!' },
                { object: 'handloom', field: 'total_deposit', message: 'Please enter total deposit!' },
                { object: 'handloom', field: 'power_loom_type', message: 'Please enter power loom type!' },
                { object: 'handloom', field: 'hand_loom_type', message: 'Please enter hand loom type!' });
            if ($scope.handloom.operated_member_themself == "1") { mainRules.push({ object: 'handloom', field: 'no_of_loom', message: 'Please enter no of loom!' }) }
        }
        else if ($scope.ncd.sector_of_operation == 47) {
            if ($scope.housing.has_building == "1") { mainRules.push({ object: 'housing', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
            mainRules.push({ object: 'housing', field: 'type_society', message: 'Please select type of society!', invalidValues: ['0'] },
                { object: 'housing', field: 'total_deposit', message: 'Please enter total deposit!' },
                { object: 'housing', field: 'annual_expenses', message: 'Please enter annual expenses!' },
                { object: 'housing', field: 'number_of_houses_audit_year', message: 'Please enter number of houses audit year!' },
                { object: 'housing', field: 'number_of_houses_during_year', message: 'Please enter number of houses during year!' },
                { object: 'housing', field: 'number_of_houses_construction', message: 'Please enter number of houses construction!' });
        }
        else if ($scope.ncd.sector_of_operation == 90) {
            if ($scope.Jute.has_building == "1") { mainRules.push({ object: 'Jute', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
            mainRules.push({ object: 'Jute', field: 'authorised_share', message: 'Please enter authorised share!' },
                { object: 'Jute', field: 'total_deposit', message: 'Please enter total deposit!' });
            if ($scope.Jute.common_work_place == "1") { mainRules.push({ object: 'Jute', field: 'workplace_operate', message: 'Please enter workplace operate!' }); }
            if ($scope.Jute.is_operate_retail == "1") { mainRules.push({ object: 'Jute', field: 'no_of_retail', message: 'Please enter no of retail!' }); }
        }
        else if ($scope.ncd.sector_of_operation == 117) {
            if ($scope.KhadiGram.has_building == "1") { mainRules.push({ object: 'KhadiGram', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
            mainRules.push({ object: 'KhadiGram', field: 'individual_member', message: 'Please enter individual member!' }, { object: 'KhadiGram', field: 'institutional_member', message: 'Please enter institutional member!' }, { object: 'KhadiGram', field: 'total_member', message: 'Please enter total member!' }); if ($scope.KhadiGram.operated_member_themself == "1") { if ($scope.KhadiGram.is_user_work_divide == "1") { mainRules.push({ object: 'KhadiGram', field: 'no_of_loom', message: 'Please enter no of loom!' }); } }
            if ($scope.KhadiGram.waste_available == "1") { mainRules.push({ object: 'KhadiGram', field: 'no_of_retail', message: 'Please enter no of retail!' }); }
        }
        else if ($scope.ncd.sector_of_operation == 51) { if ($scope.Labour.has_building == "1") { mainRules.push({ object: 'Labour', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); } }
        else if ($scope.ncd.sector_of_operation == 54) {
            if ($scope.LiveStock.has_building == "1") { mainRules.push({ object: 'LiveStock', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
            mainRules.push({ object: 'LiveStock', field: 'total_deposit', message: 'Please enter total deposit!' });
            if ($scope.LiveStock.is_operate_retail == "1") { mainRules.push({ object: 'LiveStock', field: 'no_of_retail', message: 'Please enter no of retail!' }); }
        }
        else if ($scope.ncd.sector_of_operation == 82) {
            if ($scope.marketing.has_building == "1") { mainRules.push({ object: 'marketing', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
            mainRules.push({ object: 'marketing', field: 'total_deposit', message: 'Please enter total deposit!' });
            if ($scope.marketing.is_operate_retail == "1") { mainRules.push({ object: 'marketing', field: 'no_of_retail', message: 'Please enter no of retail!' }); }
            if ($scope.marketing.has_land == "1") { mainRules.push({ object: 'marketing', field: 'land.land_owned', message: 'Please enter land owned!' }, { object: 'marketing', field: 'land.land_leased', message: 'Please enter land leased!' }, { object: 'marketing', field: 'land.land_allotted', message: 'Please enter land allotted!' }); }
            if ($scope.marketing.has_warehouses == "1") { mainRules.push({ object: 'marketing', field: 'capacity_warehouses', message: 'Please enter capacity warehouses!' }); }
        }
        else if ($scope.ncd.sector_of_operation == 35) {
            if ($scope.csm.has_building == "1") { mainRules.push({ object: 'csm', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
            mainRules.push({ object: 'csm', field: 'total_deposit', message: 'Please enter total deposit!' },
                { object: 'csm', field: 'loan_outstanding', message: 'Please enter loan outstanding!' },
                { object: 'csm', field: 'individual_member', message: 'Please enter individual member!' },
                { object: 'csm', field: 'institutional_member', message: 'Please enter institutional member!' });
        }
        else if ($scope.ncd.sector_of_operation == 29) {
            if ($scope.Miscellaneous.has_building == "1") { mainRules.push({ object: 'Miscellaneous', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
            mainRules.push({ object: 'Miscellaneous', field: 'total_deposit', message: 'Please enter total deposit!' });
        }
        else if ($scope.ncd.sector_of_operation == 16) {
            if ($scope.Multipurpose.has_building == "1") { mainRules.push({ object: 'Multipurpose', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
            if (!$scope.Multipurpose.sec_activity || $scope.Multipurpose.sec_activity.length === 0) { mainRules.push({ object: 'Multipurpose', field: 'sec_activity', message: 'Please select Primary multipurpose the Cooperative Society!' }); }
            mainRules.push({ object: 'Multipurpose', field: 'total_deposit', message: 'Please enter total deposit!' });
            if ($scope.Multipurpose.has_storage == "1") { mainRules.push({ object: 'Multipurpose', field: 'storage_capacity', message: 'Please enter storage capacity!' }); }
            if ($scope.Multipurpose.is_operate_retail == "1") { mainRules.push({ object: 'Multipurpose', field: 'no_of_retail', message: 'Please enter no of retail!' }); }
        }
        else if ($scope.ncd.sector_of_operation == 1 || $scope.ncd.sector_of_operation == 20 || $scope.ncd.sector_of_operation == 22) {
            if ($scope.pacs.has_building == "1") { mainRules.push({ object: 'pacs', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
            mainRules.push({ object: 'pacs', field: 'pack_total_outstanding_loan', message: 'Please enter  total outstanding loan!' }, { object: 'pacs', field: 'total_deposit', message: 'Please enter  total deposit!' });
            if ($scope.pacs.pesticide_distribution == "1") { mainRules.push({ object: 'pacs', field: 'pesticide_distribution_qty', message: 'Please enter pesticide distribution qty!' }); }
            if ($scope.pacs.fertilizer_distribution == "1") { mainRules.push({ object: 'pacs', field: 'fertilizer_distribution_qty', message: 'Please enter fertilizer distribution qty!' }, { object: 'pacs', field: 'fertilizer_distribution_details', message: 'Please enter fertilizer distribution details!' }); }
            if ($scope.pacs.is_pmksk == "1") { mainRules.push({ object: 'pacs', field: 'pmksk_details', message: 'Please enter pmksk details!' }); }
            if ($scope.pacs.seed_distribution == "1") { mainRules.push({ object: 'pacs', field: 'seed_distribution_qty', message: 'Please enter seed distribution qty!' }); }
            if ($scope.pacs.is_foodgrains == "1") { mainRules.push({ object: 'pacs', field: 'foodgrains_qty', message: 'Please enter foodgrains qty!' }); }
            if ($scope.pacs.agricultural_implements == "1") { mainRules.push({ object: 'pacs', field: 'agricultural_implements_text', message: 'Please enter agricultural implements!' }); }
            if ($scope.pacs.dry_storage == "1") { mainRules.push({ object: 'pacs', field: 'dry_storage_capicity', message: 'Please enter dry storage capicity!' }); }
            if ($scope.pacs.is_lgs_program == "1") { mainRules.push({ object: 'pacs', field: 'lgs_capacity', message: 'Please enter lgs  capicity!' }, { object: 'pacs', field: 'lgs_program_details', message: 'Please enter lgs  program details!' }); }
            if ($scope.pacs.cold_storage == "1") { mainRules.push({ object: 'pacs', field: 'cold_storage_capicity', message: 'Please enter cold storage capicity!' }); }
            if ($scope.pacs.milk_unit == "1") { mainRules.push({ object: 'pacs', field: 'milk_capicity_unit', message: 'Please enter milk storage unit!' }); }
            if ($scope.pacs.pack_involved_fish_catch == "1") { mainRules.push({ object: 'pacs', field: 'pack_annual_fish_catch', message: 'Please enter pack annual fish catch!' }); }
            if ($scope.pacs.food_processing == "1") { mainRules.push({ object: 'pacs', field: 'food_processing_type', message: 'Please enter food processing type!' }); }
            if ($scope.pacs.is_csc == "1") { mainRules.push({ object: 'pacs', field: 'csc_revenue', message: 'Please enter csc revenue!' }, { object: 'pacs', field: 'csc_details', message: 'Please enter csc details!' }); }
            if ($scope.pacs.is_fpo == "1") { mainRules.push({ object: 'pacs', field: 'fpo_details', message: 'Please enter fpo details!' }); }
            if ($scope.pacs.is_lpg_distributership == "1") { mainRules.push({ object: 'pacs', field: 'lpg_distributership_details', message: 'Please enter lpg distributership details!' }); }
            if ($scope.pacs.is_bcp_pump == "1") { mainRules.push({ object: 'pacs', field: 'bcp_pump_details', message: 'Please enter bcp pump details!' }); }
            if ($scope.pacs.is_dpp_diesel == "1") { mainRules.push({ object: 'pacs', field: 'dpp_diesel_details', message: 'Please enter dpp diesel details!' }); }
            if ($scope.pacs.is_jak == "1") { mainRules.push({ object: 'pacs', field: 'jak_qty', message: 'Please enter jak qty!' }); }
            if ($scope.pacs.is_pm_kusum_scheme == "1") { mainRules.push({ object: 'pacs', field: 'pm_kusum_scheme_details', message: 'Please enter pm kusum_scheme_details!' }); }
            if ($scope.pacs.fair_price == "1") { mainRules.push({ object: 'pacs', field: 'fair_price_qty', message: 'Please enter fair price qty!' }, { object: 'pacs', field: 'fair_price_details', message: 'Please enter fair price details!' }); }
            if ($scope.pacs.is_paani_samity == "1") { mainRules.push({ object: 'pacs', field: 'paani_samity_details', message: 'Please enter paani samity details!' }); }
            if ($scope.pacs.is_consumer_business == "1") { mainRules.push({ object: 'pacs', field: 'consumer_business_details', message: 'Please enter pconsumer business details!' }); }
            if ($scope.pacs.is_tourism == "1") { mainRules.push({ object: 'pacs', field: 'tourism_details', message: 'Please enter tourism details!' }); }
            if ($scope.pacs.is_cash_kendra_facilities == "1") { mainRules.push({ object: 'pacs', field: 'cash_kendra_facilities_details', message: 'Please enter case kendra facilities details!' }); }
        }
        else if ($scope.ncd.sector_of_operation == 96) {
            if ($scope.Sericulture.has_building == "1") { mainRules.push({ object: 'Sericulture', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
            mainRules.push({ object: 'Sericulture', field: 'authorised_share', message: 'Please enter authorised share!' });
            if ($scope.Sericulture.common_work_place == "1") { mainRules.push({ object: 'Sericulture', field: 'no_rear_house', message: 'Please enter no of rear house!' }); }
            if ($scope.Sericulture.is_operate_retail == "1") { mainRules.push({ object: 'Sericulture', field: 'no_of_retail', message: 'Please enter no of retail!' }); }
        }
        else if ($scope.ncd.sector_of_operation == 98) {
            mainRules.push({ object: 'SocialWelfare', field: 'type_society', message: 'Please select type of society!', invalidValues: ['0'] },
                { object: 'SocialWelfare', field: 'total_deposit', message: 'Please enter total deposit!' });
            if ($scope.SocialWelfare.has_building == "1") { mainRules.push({ object: 'SocialWelfare', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
            if (!$scope.SocialWelfare.type_social_culture_activity || $scope.SocialWelfare.type_social_culture_activity.length === 0) { mainRules.push({ object: 'SocialWelfare', field: 'type_social_culture_activity', message: 'Please select Social & Cultural activities!' }); }
            if ($scope.SocialWelfare.is_operate_vehicle == "1") { mainRules.push({ object: 'SocialWelfare', field: 'no_of_vehicle', message: 'Please number of vehicle operated!' }); }
        }
        else if ($scope.ncd.sector_of_operation == 11) {
            if ($scope.Sugar.has_building == "1") { mainRules.push({ object: 'Sugar', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
            mainRules.push({ object: 'Sugar', field: 'authorised_share', message: 'Please enter authorised share!' }, { object: 'Sugar', field: 'total_deposit', message: 'Please enter total deposit!' },
                { object: 'Sugar', field: 'suger_mills_no', message: 'Please enter number of suger mills!' }, { object: 'Sugar', field: 'build_up_area', message: 'Please enter build up area!' },
                { object: 'Sugar', field: 'open_land_area', message: 'Please enter open up area!' }, { object: 'Sugar', field: 'total_area', message: 'Please enter total area!' },
                { object: 'Sugar', field: 'liecensed_capicity', message: 'Please enter liecensed capicity!' }, { object: 'Sugar', field: 'installed_capicity', message: 'Please enter installed capicity!' },
                { object: 'Sugar', field: 'crushing_period_start', message: 'Please select crushing period start date!' }, { object: 'Sugar', field: 'crushing_period_end', message: 'Please select crushing period end date!' });

            if (!$scope.Sugar.product_produced || $scope.Sugar.product_produced.length === 0) { mainRules.push({ object: 'Sugar', field: 'product_produced', message: 'Please select product produced!' }); }
            if ($scope.Sugar.retail_shops == "1") { mainRules.push({ object: 'Sugar', field: 'retail_shops_no', message: 'Please enter retail shops number!' }); }
            if ($scope.Sugar.retail_shops == "1") { mainRules.push({ object: 'Sugar', field: 'retail_shops_no', message: 'Please enter retail shops number!' }); }
        }
        else if ($scope.ncd.sector_of_operation == 99) {
            mainRules.push({ object: 'Tourism', field: 'type_society', message: 'Please select type of society !', invalidValues: ['0'] },
                { object: 'Tourism', field: 'individual_member', message: 'Please enter individual member!' }, { object: 'Tourism', field: 'institutional_member', message: 'Please enter institutional member!' },
                { object: 'Tourism', field: 'authorised_share', message: 'Please enter authorised share!' }, { object: 'Tourism', field: 'total_deposit', message: 'Please enter total deposit!' });
            if ($scope.Tourism.has_building == "1") { mainRules.push({ object: 'Tourism', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
        }
        else if ($scope.ncd.sector_of_operation == 68) {
            mainRules.push({ object: 'Transport', field: 'type_society', message: 'Please select type of society !', invalidValues: ['0'] },
                { object: 'Transport', field: 'individual_member', message: 'Please enter individual member!' }, { object: 'Transport', field: 'institutional_member', message: 'Please enter institutional member!' },
                { object: 'Transport', field: 'total_deposit', message: 'Please enter total deposit!' });
            if ($scope.Transport.has_building == "1") { mainRules.push({ object: 'Transport', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
        }
        else if ($scope.ncd.sector_of_operation == 102) {
            mainRules.push({ object: 'Tribal', field: 'type_society', message: 'Please select type of society !', invalidValues: ['0'] },
                { object: 'Tribal', field: 'total_deposit', message: 'Please enter total deposit!' });
            if ($scope.Tribal.has_building == "1") { mainRules.push({ object: 'Tribal', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
        }
        else if ($scope.ncd.sector_of_operation == 7) {
            mainRules.push({ object: 'ucb', field: 'ucb_branch', message: 'Please enter number of ucb branches!' },
                { object: 'ucb', field: 'networth', message: 'Please enter networth!' },
                { object: 'ucb', field: 'authorised_share', message: 'Please enter authorised share!' },
                { object: 'ucb', field: 'annual_income', message: 'Please enter annual income!' },
                { object: 'ucb', field: 'annual_ucb_expenditr', message: 'Please enter annual ucb expenditr!' },
                { object: 'ucb', field: 'asset_ucb', message: 'Please enter ucb asset!' },
                { object: 'ucb', field: 'liability_ucb', message: 'Please enter liability ucb!' },
                { object: 'ucb', field: 'total_deposit', message: 'Please enter total deposit!' },
                { object: 'ucb', field: 'loan_outstanding', message: 'Please enter loan outstanding!' });
            if ($scope.ucb.has_building == "1") { mainRules.push({ object: 'ucb', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
            if ($scope.ucb.is_computerized == "1") { mainRules.push({ object: 'ucb', field: 'no_computer_working', message: 'Please enter number of working computer!' }); }
        }
        else if ($scope.ncd.sector_of_operation == 15) {
            mainRules.push({ object: 'women', field: 'type_society', message: 'Please select type of society!', invalidValues: ['0'] },
                { object: 'women', field: 'total_deposit', message: 'Please enter total deposit!' });
            if ($scope.women.has_building == "1") { mainRules.push({ object: 'women', field: 'building_type', message: 'Please select building type !', invalidValues: ['0'] }); }
        }


        const urbanRules = [
            { object: 'ncd', field: 'urban_local_body_type_code', message: 'Selelct Category of Urban Local Body!', invalidValues: ['0'] },
            { object: 'ncd', field: 'urban_local_body_code', message: 'Select Urban Local Body!', invalidValues: ['0'] },
            { object: 'ncd', field: 'locality_ward_code', message: 'Select Locality/Ward!', invalidValues: ['0'] }
        ];

        const ruralRules = [
            { object: 'ncd', field: 'block_code', message: 'Select Block!', invalidValues: ['0'] },
            { object: 'ncd', field: 'gram_panchayat_code', message: 'Select Gram Panchayat!', invalidValues: ['0'] },
            { object: 'ncd', field: 'village_code', message: 'Select Village!', invalidValues: ['0'] },
            { object: 'ncd', field: 'pincode', message: 'Please enter Pincode!' }
        ];
        if (!$scope.validateFields(mainRules)) return;

        if ($scope.ncd.location_of_head_quarter == 1) {
            if (!$scope.validateFields(urbanRules)) return;
        } else {
            if (!$scope.validateFields(ruralRules)) return;
        }

        if (!$scope.validateSectorList($scope.ncd.sectorListData, $scope.sectorDetail.operation_area_location)) return;


        $scope.ncd.annual_profit = "0";
        $scope.ncd.annual_loss = "0";
        $scope.ncd.operation_area_location = $scope.sectorDetail.operation_area_location
        if ($scope.ncd.is_profit_making == 0) {
            $scope.ncd.annual_loss = $scope.ncd.annualPayment
        } else {
            $scope.ncd.annual_profit = $scope.ncd.annualPayment;
        }

        $scope.ncd.udyam_no = $scope.ncd.is_msme == 0 ? null : $scope.ncd.is_msme;
        $scope.ncd.water_body_type_id = $scope.ncd.is_coastal == 0 ? "0" : $scope.ncd.water_body_type_id;
        $scope.ncd.audit_complete_year = $scope.ncd.financial_audit == 0 ? "0" : $scope.ncd.audit_complete_year;
        $scope.ncd.category_audit = $scope.ncd.financial_audit == 0 ? "0" : $scope.ncd.category_audit;
        $scope.ncd.annualPayment = $scope.ncd.financial_audit == 0 ? "0" : $scope.ncd.annualPayment;
        $scope.ncd.annual_profit = $scope.ncd.financial_audit == 0 ? "0" : $scope.ncd.annual_profit;
        $scope.ncd.annual_loss = $scope.ncd.financial_audit == 0 ? "0" : $scope.ncd.annual_loss;
        $scope.ncd.is_profit_making = $scope.ncd.financial_audit == 0 ? "0" : $scope.ncd.is_profit_making;
        $scope.ncd.dividend_rate = $scope.ncd.is_dividend_paid == 0 ? 0 : $scope.ncd.dividend_rate;
        $scope.ncd.affiliated_union_federation_level = $scope.ncd.is_affiliated_union_federation == 0 ? "0" : $scope.ncd.affiliated_union_federation_level;
        $scope.ncd.affiliated_union_federation_name = $scope.ncd.is_affiliated_union_federation == 0 ? null : $scope.ncd.affiliated_union_federation_name;
        $scope.ncd.annual_turnover = $scope.ncd.financial_audit == 0 ? "0" : $scope.ncd.annual_turnover;
        $scope.ncd.deregistered = ($scope.ncd.deregistered == undefined || $scope.ncd.deregistered == null) ? "0" : $scope.ncd.deregistered;
        $scope.ncd.date_deregistered = $scope.ncd.deregistered == 0 ? null : $scope.ncd.date_deregistered;
        $scope.ncd.area_of_operation_id = $scope.sectorDetail.area_of_operation_id;
        $scope.ncd.memberArray = { ...$scope.ncdm };

        $scope.ncd.sectorListData.map(x => {
            x.area_of_operation_id = $scope.sectorDetail.area_of_operation_id;
            x.NCDBlockListDetails = null;
            x.NCDGramPanchayatListDetails = null;
            x.NCDVillageListDetails = null;
            x.urbanBodyTypeDetailDataList = null;
            x.wardDetailDataList = null;
        })

        if ($scope.ncd.sector_of_operation == 84) {
            $scope.education.type_society = +$scope.education.type_society;
            if ($scope.education.type_society == 1) { $scope.education.duration_of_course = 0; $scope.education.level_and_duration_of_course = []; }
            else if ($scope.education.type_society == 2) { $scope.education.level_of_edu = 0; $scope.education.level_and_duration_of_course = []; }
            else if ($scope.education.type_society == 3) { $scope.education.level_of_edu = 0; $scope.education.duration_of_course = 0; }
        }

        if ($scope.ncd.sector_of_operation == 1 || $scope.ncd.sector_of_operation == 20 || $scope.ncd.sector_of_operation == 22) {
            $scope.pacs.building_type = $scope.pacs.has_building == 0 ? 0 : $scope.pacs.building_type;
            if ($scope.pacs.is_socitey_has_land == 1) {
                $scope.pacs.land.land_owned = ($scope.pacs.is_socitey_has_land == 0) ? 0 : ($scope.pacs.land.land_owned || 0);
                $scope.pacs.land.land_leased = ($scope.pacs.is_socitey_has_land == 0) ? 0 : ($scope.pacs.land.land_leased || 0);
                $scope.pacs.land.land_allotted = ($scope.pacs.is_socitey_has_land == 0) ? 0 : ($scope.pacs.land.land_allotted || 0);
                $scope.pacs.land.land_total = ($scope.pacs.is_socitey_has_land == 0) ? 0 : ($scope.pacs.land.land_total || 0);
            }
            $scope.pacs.pesticide_distribution_qty = $scope.pacs.pesticide_distribution == 0 ? 0 : $scope.pacs.pesticide_distribution_qty;
            $scope.pacs.pmksk_details = $scope.pacs.is_pmksk == 0 ? null : $scope.pacs.pmksk_details;
            $scope.pacs.fertilizer_distribution_qty = $scope.pacs.fertilizer_distribution == 0 ? 0 : $scope.pacs.fertilizer_distribution_qty;
            $scope.pacs.fertilizer_distribution_details = $scope.pacs.fertilizer_distribution == 0 ? null : $scope.pacs.fertilizer_distribution_details;
            $scope.pacs.seed_distribution_qty = $scope.pacs.seed_distribution == 0 ? 0 : $scope.pacs.seed_distribution_qty;
            $scope.pacs.foodgrains_qty = $scope.pacs.is_foodgrains == 0 ? 0 : $scope.pacs.foodgrains_qty;
            $scope.pacs.agricultural_implements_text = $scope.pacs.agricultural_implements == 0 ? null : $scope.pacs.agricultural_implements_text;
            $scope.pacs.dry_storage_capicity = $scope.pacs.dry_storage == 0 ? 0 : $scope.pacs.dry_storage_capicity;
            $scope.pacs.lgs_capacity = $scope.pacs.is_lgs_program == 0 ? 0 : $scope.pacs.lgs_capacity;
            $scope.pacs.lgs_program_details = $scope.pacs.is_lgs_program == 0 ? null : $scope.pacs.lgs_program_details;
            $scope.pacs.cold_storage_capicity = $scope.pacs.cold_storage == 0 ? 0 : $scope.pacs.cold_storage_capicity;
            $scope.pacs.milk_capicity_unit = $scope.pacs.milk_unit == 0 ? 0 : $scope.pacs.milk_capicity_unit;
            $scope.pacs.pack_annual_fish_catch = $scope.pacs.pack_involved_fish_catch == 0 ? 0 : $scope.pacs.pack_annual_fish_catch;
            $scope.pacs.food_processing_type = $scope.pacs.food_processing == 0 ? null : $scope.pacs.food_processing_type;
            $scope.pacs.csc_revenue = $scope.pacs.is_csc == 0 ? 0 : $scope.pacs.csc_revenue;
            $scope.pacs.csc_details = $scope.pacs.is_csc == 0 ? null : $scope.pacs.csc_details;
            $scope.pacs.fpo_details = $scope.pacs.is_fpo == 0 ? null : $scope.pacs.fpo_details;
            $scope.pacs.lpg_distributership_details = $scope.pacs.is_lpg_distributership == 0 ? null : $scope.pacs.lpg_distributership_details;
            $scope.pacs.bcp_pump_detailss = $scope.pacs.is_bcp_pump == 0 ? null : $scope.pacs.bcp_pump_detailss;
            $scope.pacs.dpp_diesel_details = $scope.pacs.is_dpp_diesel == 0 ? null : $scope.pacs.dpp_diesel_details;
            $scope.pacs.jak_qty = $scope.pacs.is_jak == 0 ? 0 : $scope.pacs.jak_qty;
            $scope.pacs.pm_kusum_scheme_details = $scope.pacs.is_pm_kusum_scheme == 0 ? null : $scope.pacs.pm_kusum_scheme_details;
            $scope.pacs.fair_price_qty = $scope.pacs.fair_price == 0 ? 0 : $scope.pacs.fair_price_qty;
            $scope.pacs.fair_price_details = $scope.pacs.fair_price == 0 ? null : $scope.pacs.fair_price_details;
            $scope.pacs.paani_samity_details = $scope.pacs.is_paani_samity == 0 ? null : $scope.pacs.paani_samity_details;
            $scope.pacs.consumer_business_details = $scope.pacs.is_consumer_business == 0 ? null : $scope.pacs.consumer_business_details;
            $scope.pacs.tourism_details = $scope.pacs.is_tourism == 0 ? null : $scope.pacs.tourism_details;
            $scope.pacs.cash_kendra_facilities_details = $scope.pacs.is_cash_kendra_facilities == 0 ? null : $scope.pacs.cash_kendra_facilities_details;
            $scope.pacs.pack_total_outstanding_loan = ($scope.pacs.pack_total_outstanding_loan == null || $scope.pacs.pack_total_outstanding_loan == undefined) ? 0 : $scope.pacs.pack_total_outstanding_loan;
            $scope.pacs.pack_revenue_non_credit = ($scope.pacs.pack_revenue_non_credit == null || $scope.pacs.pack_revenue_non_credit == undefined) ? 0 : $scope.pacs.pack_revenue_non_credit;
            $scope.pacs.authorised_share = ($scope.pacs.authorised_share == null || $scope.pacs.authorised_share == undefined) ? 0 : $scope.pacs.authorised_share;
            $scope.pacs.paid_up_share = ($scope.pacs.paid_up_share == null || $scope.pacs.paid_up_share == undefined) ? 0 : $scope.pacs.paid_up_share;
            $scope.pacs.total_deposit = ($scope.pacs.total_deposit == null || $scope.pacs.total_deposit == undefined) ? 0 : $scope.pacs.total_deposit;
            $scope.ncd.pacs = { ...$scope.pacs };
        }
        else if ($scope.ncd.sector_of_operation == 7) {
            $scope.ucb.has_building = ($scope.ucb.has_building == null || $scope.ucb.has_building == 0) ? 0 : $scope.ucb.has_building;
            $scope.ucb.ucb_branch = $scope.ucb.ucb_branch == null ? 0 : $scope.ucb.ucb_branch;
            $scope.ucb.networth = $scope.ucb.networth == null ? 0 : $scope.ucb.networth;
            $scope.ucb.building_type = $scope.ucb.has_building == 0 ? 0 : $scope.ucb.building_type;
            $scope.ucb.no_computer_working = $scope.ucb.is_computerized == 0 ? 0 : $scope.ucb.no_computer_working;
            $scope.ucb.authorised_share = $scope.ucb.authorised_share == null ? 0 : $scope.ucb.authorised_share;
            $scope.ucb.annual_turn_over = $scope.ucb.annual_turn_over == null ? 0 : $scope.ucb.annual_turn_over;
            $scope.ucb.annual_income = $scope.ucb.annual_income == null ? 0 : $scope.ucb.annual_income;
            $scope.ucb.annual_ucb_expenditr = $scope.ucb.annual_ucb_expenditr == null ? 0 : $scope.ucb.annual_ucb_expenditr;
            $scope.ucb.asset_ucb = $scope.ucb.asset_ucb == null ? 0 : $scope.ucb.asset_ucb;
            $scope.ucb.liability_ucb = $scope.ucb.liability_ucb == null ? 0 : $scope.ucb.liability_ucb;
            $scope.ucb.total_deposit = $scope.ucb.total_deposit == null ? 0 : $scope.ucb.total_deposit;
            $scope.ucb.loan_outstanding = $scope.ucb.loan_outstanding == null ? 0 : $scope.ucb.loan_outstanding;
            $scope.ucb.Paid_up_share = $scope.ucb.Paid_up_share == null ? 0 : $scope.ucb.Paid_up_share;
            $scope.ncd.ucb = { ...$scope.ucb };
        }
        else if ($scope.ncd.sector_of_operation == 9) {
            $scope.dairy.milk_collection = ($scope.dairy.milk_collection == null || $scope.dairy.milk_collection == undefined) ? 0 : $scope.dairy.milk_collection;
            $scope.dairy.authorised_share = ($scope.dairy.authorised_share == null || $scope.dairy.authorised_share == undefined) ? 0 : $scope.dairy.authorised_share;
            $scope.dairy.paid_up_share = ($scope.dairy.paid_up_share == null || $scope.dairy.paid_up_share == undefined) ? 0 : $scope.dairy.paid_up_share;
            $scope.dairy.total_deposit = ($scope.dairy.total_deposit == null || $scope.dairy.total_deposit == undefined) ? 0 : $scope.dairy.total_deposit;
            $scope.dairy.pack_total_outstanding_loan = ($scope.dairy.pack_total_outstanding_loan == null || $scope.dairy.pack_total_outstanding_loan == undefined) ? 0 : $scope.dairy.pack_total_outstanding_loan;
            $scope.dairy.credit_provided = $scope.dairy.credit_facility == 0 ? 0 : $scope.dairy.credit_provided;
            $scope.dairy.milk_collection_capicity = $scope.dairy.milk_collection_unit == 0 ? 0 : $scope.dairy.milk_collection_capicity;
            $scope.dairy.bank_mitra_details = $scope.dairy.is_bank_mitra == 0 ? null : $scope.dairy.bank_mitra_details;
            $scope.dairy.micro_atm_details = $scope.dairy.is_micro_atm == 0 ? null : $scope.dairy.micro_atm_details;
            $scope.ncd.dairy = { ...$scope.dairy };
        }
        else if ($scope.ncd.sector_of_operation == 10) {
            $scope.Fishery.annual_fish_catch = ($scope.Fishery.annual_fish_catch == null || $scope.Fishery.annual_fish_catch == undefined) ? 0 : $scope.Fishery.annual_fish_catchl;
            $scope.Fishery.authorised_share = ($scope.Fishery.authorised_share == null || $scope.Fishery.authorised_share == undefined) ? 0 : $scope.Fishery.authorised_share;
            $scope.Fishery.paid_up_share = ($scope.Fishery.paid_up_share == null || $scope.Fishery.paid_up_share == undefined) ? 0 : $scope.Fishery.paid_up_share;
            $scope.Fishery.total_deposit = ($scope.Fishery.total_deposit == null || $scope.Fishery.total_deposit == undefined) ? 0 : $scope.Fishery.total_deposit;
            $scope.Fishery.pack_total_outstanding_loan = ($scope.Fishery.pack_total_outstanding_loan == null || $scope.Fishery.pack_total_outstanding_loan == undefined) ? 0 : $scope.Fishery.pack_total_outstanding_loan;
            $scope.Fishery.total_credit_provided = $scope.Fishery.credit_facility == 0 ? 0 : $scope.Fishery.total_credit_provided;
            $scope.Fishery.fpo_fisheries_details = $scope.Fishery.is_fpo_fisheries == 0 ? null : $scope.Fishery.fpo_fisheries_details;
            $scope.ncd.Fishery = { ...$scope.Fishery };
        }
        else if ($scope.ncd.sector_of_operation == 13) {
            $scope.handloom.building_type = $scope.handloom.has_building == 0 ? 0 : $scope.handloom.building_type;
            $scope.handloom.no_of_retail = $scope.handloom.waste_available == 0 ? 0 : $scope.handloom.no_of_retail;
            $scope.handloom.is_user_work_divide = $scope.handloom.operated_member_themself == 0 ? 0 : $scope.handloom.is_user_work_divide;
            $scope.handloom.no_of_loom = ($scope.handloom.no_of_loom == null || $scope.handloom.no_of_loom == undefined) ? 0 : $scope.handloom.no_of_loom;
            $scope.handloom.pack_total_outstanding_loan = ($scope.handloom.pack_total_outstanding_loan == null || $scope.handloom.pack_total_outstanding_loan == undefined) ? 0 : $scope.handloom.pack_total_outstanding_loan;
            $scope.handloom.total_deposit = ($scope.handloom.total_deposit == null || $scope.handloom.total_deposit == undefined) ? 0 : $scope.handloom.total_deposit;
            $scope.handloom.annual_turn_over = ($scope.handloom.annual_turn_over == null || $scope.handloom.annual_turn_over == undefined) ? 0 : $scope.handloom.annual_turn_over;
            $scope.handloom.paid_up_members = ($scope.handloom.paid_up_members == null || $scope.handloom.paid_up_members == undefined) ? 0 : $scope.handloom.paid_up_members;
            $scope.handloom.paid_up_government_bodies = ($scope.handloom.paid_up_government_bodies == null || $scope.handloom.paid_up_government_bodies == undefined) ? 0 : $scope.handloom.paid_up_government_bodies;
            $scope.handloom.paid_up_total = ($scope.handloom.paid_up_total == null || $scope.handloom.paid_up_total == undefined) ? 0 : $scope.handloom.paid_up_total;
            $scope.handloom.authorised_share = ($scope.handloom.authorised_share == null || $scope.handloom.authorised_share == undefined) ? 0 : $scope.handloom.authorised_share;
            $scope.handloom.total_members = ($scope.handloom.total_members == null || $scope.handloom.total_members == undefined) ? 0 : $scope.handloom.total_members;
            $scope.handloom.institutional_member = ($scope.handloom.institutional_member == null || $scope.handloom.institutional_member == undefined) ? 0 : $scope.handloom.institutional_member;
            $scope.handloom.individual_member = ($scope.handloom.individual_member == null || $scope.handloom.individual_member == undefined) ? 0 : $scope.handloom.individual_member;
            $scope.ncd.handloom = { ...$scope.handloom };
        }
        else if ($scope.ncd.sector_of_operation == 14) {
            $scope.Handicraft.building_type = $scope.Handicraft.has_building == 0 ? 0 : $scope.Handicraft.building_type;
            $scope.Handicraft.individual_member = ($scope.Handicraft.individual_member == null || $scope.Handicraft.individual_member == undefined) ? 0 : $scope.Handicraft.individual_member;
            $scope.Handicraft.institutional_member = ($scope.Handicraft.institutional_member == null || $scope.Handicraft.institutional_member == undefined) ? 0 : $scope.Handicraft.institutional_member;
            $scope.Handicraft.total_members = ($scope.Handicraft.total_members == null || $scope.Handicraft.total_members == undefined) ? 0 : $scope.Handicraft.total_members;
            $scope.Handicraft.authorised_share = ($scope.Handicraft.authorised_share == null || $scope.Handicraft.authorised_share == undefined) ? 0 : $scope.Handicraft.authorised_share;
            $scope.Handicraft.paid_up_members = ($scope.Handicraft.paid_up_members == null || $scope.Handicraft.paid_up_members == undefined) ? 0 : $scope.Handicraft.paid_up_members;
            $scope.Handicraft.paid_up_government_bodies = ($scope.Handicraft.paid_up_government_bodies == null || $scope.Handicraft.paid_up_government_bodies == undefined) ? 0 : $scope.Handicraft.paid_up_government_bodies;
            $scope.Handicraft.paid_up_total = ($scope.Handicraft.paid_up_total == null || $scope.Handicraft.paid_up_total == undefined) ? 0 : $scope.Handicraft.paid_up_total;
            $scope.Handicraft.annual_turn_over = ($scope.Handicraft.annual_turn_over == null || $scope.Handicraft.annual_turn_over == undefined) ? 0 : $scope.Handicraft.annual_turn_over;
            $scope.Handicraft.total_deposit = ($scope.Handicraft.total_deposit == null || $scope.Handicraft.total_deposit == undefined) ? 0 : $scope.Handicraft.total_deposit;
            $scope.Handicraft.pack_total_outstanding_loan = ($scope.Handicraft.pack_total_outstanding_loan == null || $scope.Handicraft.pack_total_outstanding_loan == undefined) ? 0 : $scope.Handicraft.pack_total_outstanding_loan;
            $scope.Handicraft.workplace_operate = $scope.Handicraft.common_work_place == 0 ? 0 : $scope.Handicraft.workplace_operate;
            $scope.Handicraft.is_training_provide = $scope.Handicraft.is_work_by_member == 0 ? 0 : $scope.Handicraft.is_training_provide;
            $scope.Handicraft.is_waste_facility = $scope.Handicraft.is_waste_generate == 0 ? 0 : $scope.Handicraft.is_waste_facility;
            $scope.Handicraft.no_of_retail = $scope.Handicraft.is_operate_retail == 0 ? 0 : $scope.Handicraft.no_of_retail;
            $scope.ncd.Handicraft = { ...$scope.Handicraft };
        }
        else if ($scope.ncd.sector_of_operation == 15) {
            $scope.women.building_type = $scope.women.has_building == 0 ? null : $scope.women.building_type;
            $scope.ncd.women = { ...$scope.women };
        }
        else if ($scope.ncd.sector_of_operation == 18) {
            $scope.tsd.building_type = ($scope.tsd.has_building == 0 || $scope.tsd.has_building == undefined) ? 0 : $scope.tsd.building_type;
            $scope.tsd.authorised_share = ($scope.tsd.authorised_share == undefined || $scope.tsd.authorised_share == null) ? 0 : $scope.tsd.authorised_share;
            $scope.tsd.paid_up_share = ($scope.tsd.paid_up_share == undefined || $scope.tsd.paid_up_share == null) ? 0 : $scope.tsd.paid_up_share;
            $scope.tsd.total_deposit = ($scope.tsd.total_deposit == undefined || $scope.tsd.total_deposit == null) ? 0 : $scope.tsd.total_deposit;
            $scope.tsd.pack_total_outstanding_loan = ($scope.tsd.pack_total_outstanding_loan == undefined || $scope.tsd.pack_total_outstanding_loan == null) ? 0 : $scope.tsd.pack_total_outstanding_loan;
            $scope.ncd.tsd = { ...$scope.tsd };
        }
        else if ($scope.ncd.sector_of_operation == 31) {
            $scope.crp.type_society = ($scope.crp.type_society == null || $scope.crp.type_society == '' || $scope.crp.type_society == undefined) ? 0 : $scope.crp.type_society;
            $scope.crp.building_type = $scope.crp.has_building == 0 ? 0 : $scope.crp.building_type;
            $scope.crp.individual_member = ($scope.crp.individual_member == null || $scope.crp.individual_member == undefined) ? 0 : $scope.crp.individual_member;
            $scope.crp.institutional_member = ($scope.crp.institutional_member == null || $scope.crp.institutional_member == undefined) ? 0 : $scope.crp.institutional_member;
            $scope.crp.total_member = ($scope.crp.total_member == null || $scope.crp.total_member == undefined) ? 0 : $scope.crp.total_member;
            $scope.crp.authorised_share = ($scope.crp.authorised_share == null || $scope.crp.authorised_share == undefined) ? 0 : $scope.crp.authorised_share;
            $scope.crp.paid_up_members = ($scope.crp.paid_up_members == null || $scope.crp.paid_up_members == undefined) ? 0 : $scope.crp.paid_up_members;
            $scope.crp.paid_up_government_bodies = ($scope.crp.paid_up_government_bodies == null || $scope.crp.paid_up_government_bodies == undefined) ? 0 : $scope.crp.paid_up_government_bodies;
            $scope.crp.paid_up_total = ($scope.crp.paid_up_total == null || $scope.crp.paid_up_total == undefined) ? 0 : $scope.crp.paid_up_total;
            $scope.crp.annual_turn_over = ($scope.crp.annual_turn_over == null || $scope.crp.annual_turn_over == undefined) ? 0 : $scope.crp.annual_turn_over;
            $scope.crp.total_deposit = ($scope.crp.total_deposit == null || $scope.crp.total_deposit == undefined) ? 0 : $scope.crp.total_deposit;
            $scope.crp.pack_total_outstanding_loan = ($scope.crp.pack_total_outstanding_loan == null || $scope.crp.pack_total_outstanding_loan == undefined) ? 0 : $scope.crp.pack_total_outstanding_loan;
            $scope.crp.processing_unit_number = $scope.crp.processing_unit == 0 ? 0 : $scope.crp.processing_unit_number;
            $scope.crp.work_divided = $scope.crp.processing_by_members == 0 ? 0 : $scope.crp.work_divided;
            $scope.crp.waste_management_facility = $scope.crp.wastes_generated == 0 ? 0 : $scope.crp.waste_management_facility;
            $scope.crp.operate_shops_number = $scope.crp.operate_shops == 0 ? 0 : $scope.crp.operate_shops_number;

            $scope.ncd.crp = { ...$scope.crp };
        }
        else if ($scope.ncd.sector_of_operation == 35) {
            $scope.csm.building_type = $scope.csm.has_building == 0 ? null : $scope.csm.building_type;
            $scope.ncd.csm = { ...$scope.csm };
        }
        else if ($scope.ncd.sector_of_operation == 77) {
            $scope.aacs.building_type = $scope.aacs.has_building == 0 ? null : $scope.aacs.building_type;
            $scope.aacs.individual_member = ($scope.aacs.individual_member == null || $scope.aacs.individual_member == undefined) ? 0 : $scope.aacs.individual_member;
            $scope.aacs.institutional_member = ($scope.aacs.institutional_member == null || $scope.aacs.institutional_member == undefined) ? 0 : $scope.aacs.institutional_member;
            $scope.aacs.total = ($scope.aacs.total == null || $scope.aacs.total == undefined) ? 0 : $scope.aacs.total;
            $scope.aacs.authorised_share = ($scope.aacs.authorised_share == null || $scope.aacs.authorised_share == undefined) ? 0 : $scope.aacs.authorised_share;
            $scope.aacs.paid_up_members = ($scope.aacs.paid_up_members == null || $scope.aacs.paid_up_members == undefined) ? 0 : $scope.aacs.paid_up_members;
            $scope.aacs.paid_up_government_bodies = ($scope.aacs.paid_up_government_bodies == null || $scope.aacs.paid_up_government_bodies == undefined) ? 0 : $scope.aacs.paid_up_government_bodies;
            $scope.aacs.paid_up_total = ($scope.aacs.paid_up_total == null || $scope.aacs.paid_up_total == undefined) ? 0 : $scope.aacs.paid_up_total;
            $scope.aacs.annual_turn_over = ($scope.aacs.annual_turn_over == null || $scope.aacs.annual_turn_over == undefined) ? 0 : $scope.aacs.annual_turn_over;
            $scope.aacs.total_deposit = ($scope.aacs.total_deposit == null || $scope.aacs.total_deposit == undefined) ? 0 : $scope.aacs.total_deposit;
            $scope.aacs.pack_total_outstanding_loan = ($scope.aacs.pack_total_outstanding_loan == null || $scope.aacs.pack_total_outstanding_loan == undefined) ? 0 : $scope.aacs.pack_total_outstanding_loan;
            $scope.aacs.pack_total_outstanding_loan = ($scope.aacs.pack_total_outstanding_loan == null || $scope.aacs.pack_total_outstanding_loan == undefined) ? 0 : $scope.aacs.pack_total_outstanding_loan;
            $scope.ncd.aacs = { ...$scope.aacs };
        }
        else if ($scope.ncd.sector_of_operation == 79) {
            $scope.beeCoop.building_type = $scope.beeCoop.has_building == 0 ? 0 : $scope.beeCoop.building_type;
            $scope.beeCoop.no_of_retail = $scope.beeCoop.is_operate_retail == 0 ? 0 : $scope.beeCoop.no_of_retail;
            $scope.beeCoop.guidance_by_member = $scope.beeCoop.rear_by_member == 0 ? 0 : $scope.beeCoop.guidance_by_member;
            $scope.beeCoop.authorised_share = ($scope.beeCoop.authorised_share == null || $scope.beeCoop.authorised_share == undefined) ? 0 : $scope.beeCoop.authorised_share;
            $scope.beeCoop.paid_up_members = ($scope.beeCoop.paid_up_members == null || $scope.beeCoop.paid_up_members == undefined) ? 0 : $scope.beeCoop.paid_up_members;
            $scope.beeCoop.paid_up_government_bodies = ($scope.beeCoop.paid_up_government_bodies == null || $scope.beeCoop.paid_up_government_bodies == undefined) ? 0 : $scope.beeCoop.paid_up_government_bodies;
            $scope.beeCoop.paid_up_total = ($scope.beeCoop.paid_up_total == null || $scope.beeCoop.paid_up_total == undefined) ? 0 : $scope.beeCoop.paid_up_total;
            $scope.beeCoop.annual_turn_over = ($scope.beeCoop.annual_turn_over == null || $scope.beeCoop.annual_turn_over == undefined) ? 0 : $scope.beeCoop.annual_turn_over;
            $scope.beeCoop.total_deposit = ($scope.beeCoop.total_deposit == null || $scope.beeCoop.total_deposit == undefined) ? 0 : $scope.beeCoop.total_deposit;
            $scope.beeCoop.pack_total_outstanding_loan = ($scope.beeCoop.pack_total_outstanding_loan == null || $scope.beeCoop.pack_total_outstanding_loan == undefined) ? 0 : $scope.beeCoop.pack_total_outstanding_loan;
            $scope.beeCoop.no_of_behives = $scope.beeCoop.common_yard == 0 ? 0 : $scope.beeCoop.no_of_behives;
            $scope.ncd.beeCoop = { ...$scope.beeCoop };
        }
        else if ($scope.ncd.sector_of_operation == 80) {
            $scope.consumer.building_type = $scope.consumer.has_building == 0 ? 0 : $scope.consumer.building_type;
            $scope.consumer.no_of_outlets = $scope.consumer.has_store == 0 ? 0 : $scope.consumer.no_of_outlets;
            $scope.consumer.authorised_share = ($scope.consumer.authorised_share == null || $scope.consumer.authorised_share == undefined) ? 0 : $scope.consumer.authorised_share;
            $scope.consumer.paid_up_share = ($scope.consumer.paid_up_share == null || $scope.consumer.paid_up_share == undefined) ? 0 : $scope.consumer.paid_up_share;
            $scope.consumer.annual_turn_over = ($scope.consumer.annual_turn_over == null || $scope.consumer.annual_turn_over == undefined) ? 0 : $scope.consumer.annual_turn_over;
            $scope.consumer.total_deposit = ($scope.consumer.total_deposit == null || $scope.consumer.total_deposit == undefined) ? 0 : $scope.consumer.total_deposit;
            $scope.consumer.pack_total_outstanding_loan = ($scope.consumer.pack_total_outstanding_loan == null || $scope.consumer.pack_total_outstanding_loan == undefined) ? 0 : $scope.consumer.pack_total_outstanding_loan;
            $scope.ncd.consumer = { ...$scope.consumer };
        }
        else if ($scope.ncd.sector_of_operation == 117) {
            $scope.KhadiGram.building_type = $scope.KhadiGram.has_building == 0 ? 0 : $scope.KhadiGram.building_type;
            $scope.KhadiGram.type_of_activities_khadi_gram = (KhadiGram.type_of_activities_khadi_gram == null || KhadiGram.type_of_activities_khadi_gram == '' || KhadiGram.type_of_activities_khadi_gram == undefined) ? 0 : KhadiGram.type_of_activities_khadi_gram;
            $scope.KhadiGram.individual_member = ($scope.KhadiGram.individual_member == null || $scope.KhadiGram.individual_member == undefined) ? 0 : $scope.KhadiGram.individual_member;
            $scope.KhadiGram.institutional_member = ($scope.KhadiGram.institutional_member == null || $scope.KhadiGram.institutional_member == undefined) ? 0 : $scope.KhadiGram.institutional_member;
            $scope.KhadiGram.total_member = ($scope.KhadiGram.total_member == null || $scope.KhadiGram.total_member == undefined) ? 0 : $scope.KhadiGram.total_member;
            $scope.KhadiGram.authorised_share = ($scope.KhadiGram.authorised_share == null || $scope.KhadiGram.authorised_share == undefined) ? 0 : $scope.KhadiGram.authorised_share;
            $scope.KhadiGram.power_loom_type = ($scope.KhadiGram.power_loom_type == null || $scope.KhadiGram.power_loom_type == undefined) ? 0 : $scope.KhadiGram.power_loom_type;
            $scope.KhadiGram.hand_loom_type = ($scope.KhadiGram.hand_loom_type == null || $scope.KhadiGram.hand_loom_type == undefined) ? 0 : $scope.KhadiGram.hand_loom_type;
            $scope.KhadiGram.hand_loom_other_type = ($scope.KhadiGram.hand_loom_other_type == null || $scope.KhadiGram.hand_loom_other_type == undefined) ? 0 : $scope.KhadiGram.hand_loom_other_type;
            $scope.KhadiGram.no_of_loom = ($scope.KhadiGram.no_of_loom == null || $scope.KhadiGram.no_of_loom == undefined) ? 0 : $scope.KhadiGram.no_of_loom;
            $scope.KhadiGram.no_of_retail = $scope.KhadiGram.waste_available == 0 ? 0 : $scope.KhadiGram.no_of_retail;
            $scope.KhadiGram.is_user_work_divide = $scope.KhadiGram.operated_member_themself == 0 ? 0 : $scope.KhadiGram.is_user_work_divide;
            $scope.KhadiGram.waste_available = $scope.KhadiGram.raw_material_available == 0 ? 0 : $scope.KhadiGram.waste_available;
            $scope.KhadiGram.total_deposit = ($scope.KhadiGram.total_deposit == null || $scope.KhadiGram.total_deposit == undefined) ? 0 : $scope.KhadiGram.total_deposit;
            $scope.KhadiGram.pack_total_outstanding_loan = ($scope.KhadiGram.pack_total_outstanding_loan == null || $scope.KhadiGram.pack_total_outstanding_loan == undefined) ? 0 : $scope.KhadiGram.pack_total_outstanding_loan;
            $scope.ncd.KhadiGram = { ...$scope.KhadiGram };
        }
        else if ($scope.ncd.sector_of_operation == 84) {
            $scope.education.building_type = $scope.education.has_building == 0 ? 0 : $scope.education.building_type;
            $scope.education.type_society = ($scope.education.type_society == '' || $scope.education.type_society == null || $scope.education.type_society == undefined) ? 0 : $scope.education.type_society;
            $scope.education.level_of_edu = ($scope.education.level_of_edu == '' || $scope.education.level_of_edu == null || $scope.education.level_of_edu == undefined) ? 0 : $scope.education.level_of_edu;
            $scope.education.duration_of_course = ($scope.education.duration_of_course == '' || $scope.education.duration_of_course == null || $scope.education.duration_of_course == undefined) ? 0 : $scope.education.duration_of_course;
            $scope.education.no_of_training_course = $scope.education.course_international_participant == 0 ? 0 : $scope.education.no_of_training_course;
            $scope.education.no_regular_faculty = $scope.education.society_recruit == 0 ? 0 : $scope.education.no_regular_faculty;
            $scope.education.no_other_faculty = $scope.education.society_recruit == 0 ? 0 : $scope.education.no_other_faculty;
            $scope.education.land.land_owned = $scope.education.has_land == 0 ? 0 : $scope.education.land.land_owned;
            $scope.education.land.land_leased = $scope.education.has_land == 0 ? 0 : $scope.education.land.land_leased;
            $scope.education.land.land_allotted = $scope.education.has_land == 0 ? 0 : $scope.education.land.land_allotted;
            $scope.education.land.land_total = $scope.education.has_land == 0 ? 0 : $scope.education.land.land_total;
            $scope.education.individual_member = ($scope.education.individual_member == null || $scope.education.individual_member == undefined) ? 0 : $scope.education.individual_member;
            $scope.education.institutional_member = ($scope.education.institutional_member == null || $scope.education.institutional_member == undefined) ? 0 : $scope.education.institutional_member;
            $scope.education.members_of_society = ($scope.education.members_of_society == null || $scope.education.members_of_society == undefined) ? 0 : $scope.education.members_of_society;
            $scope.education.authorised_share = ($scope.education.authorised_share == null || $scope.education.authorised_share == undefined) ? 0 : $scope.education.authorised_share;
            $scope.education.paid_up_members = ($scope.education.paid_up_members == null || $scope.education.paid_up_members == undefined) ? 0 : $scope.education.paid_up_members;
            $scope.education.paid_up_government_bodies = ($scope.education.paid_up_government_bodies == null || $scope.education.paid_up_government_bodies == undefined) ? 0 : $scope.education.paid_up_government_bodies;
            $scope.education.paid_up_total = ($scope.education.paid_up_total == null || $scope.education.paid_up_total == undefined) ? 0 : $scope.education.paid_up_total;
            $scope.education.annual_turn_over = ($scope.education.annual_turn_over == null || $scope.education.annual_turn_over == undefined) ? 0 : $scope.education.annual_turn_over;
            $scope.education.course_in_audit = ($scope.education.course_in_audit == null || $scope.education.course_in_audit == undefined) ? 0 : $scope.education.course_in_audit;
            $scope.education.stu_in_audit = ($scope.education.stu_in_audit == null || $scope.education.stu_in_audit == undefined) ? 0 : $scope.education.stu_in_audit;
            $scope.education.training_course_in_audit = ($scope.education.training_course_in_audit == null || $scope.education.training_course_in_audit == undefined) ? 0 : $scope.education.training_course_in_audit;
            $scope.education.participants_in_audit = ($scope.education.participants_in_audit == null || $scope.education.participants_in_audit == undefined) ? 0 : $scope.education.participants_in_audit;
            $scope.education.attended_training = ($scope.education.attended_training == null || $scope.education.attended_training == undefined) ? 0 : $scope.education.attended_training;
            $scope.education.total_deposit = ($scope.education.total_deposit == null || $scope.education.total_deposit == undefined) ? 0 : $scope.education.total_deposit;
            $scope.education.pack_total_outstanding_loan = ($scope.education.pack_total_outstanding_loan == null || $scope.education.pack_total_outstanding_loan == undefined) ? 0 : $scope.education.pack_total_outstanding_loan;
            $scope.ncd.education = { ...$scope.education };
        }
        else if ($scope.ncd.sector_of_operation == 47) {
            $scope.housing.has_land = ($scope.housing.has_land == 0 || $scope.housing.has_land == null || $scope.housing.has_land == undefined) ? 0 : $scope.housing.has_land;
            $scope.housing.type_society = ($scope.housing.type_society == null || $scope.housing.type_society == undefined) ? 0 : $scope.housing.type_society;
            $scope.housing.building_type = $scope.housing.has_building == 0 ? 0 : $scope.housing.building_type;
            $scope.housing.authorised_share = ($scope.housing.authorised_share == null || $scope.housing.authorised_share == undefined) ? 0 : $scope.housing.authorised_share;
            $scope.housing.paid_up_members = ($scope.housing.paid_up_members == null || $scope.housing.paid_up_members == undefined) ? 0 : $scope.housing.paid_up_members;
            $scope.housing.paid_up_government_bodies = ($scope.housing.paid_up_government_bodies == null || $scope.housing.paid_up_government_bodies == undefined) ? 0 : $scope.housing.paid_up_government_bodies;
            $scope.housing.paid_up_total = ($scope.housing.paid_up_total == null || $scope.housing.paid_up_total == undefined) ? 0 : $scope.housing.paid_up_total;
            $scope.housing.annual_turn_over = ($scope.housing.annual_turn_over == null || $scope.housing.annual_turn_over == undefined) ? 0 : $scope.housing.annual_turn_over;
            $scope.housing.annual_expenses = ($scope.housing.annual_expenses == null || $scope.housing.annual_expenses == undefined) ? 0 : $scope.housing.annual_expenses;
            $scope.housing.number_of_houses_audit_year = ($scope.housing.number_of_houses_audit_year == null || $scope.housing.number_of_houses_audit_year == undefined) ? 0 : $scope.housing.number_of_houses_audit_year;
            $scope.housing.number_of_houses_during_year = ($scope.housing.number_of_houses_during_year == null || $scope.housing.number_of_houses_during_year == undefined) ? 0 : $scope.housing.number_of_houses_during_year;
            $scope.housing.number_of_houses_construction = ($scope.housing.number_of_houses_construction == null || $scope.housing.number_of_houses_construction == undefined) ? 0 : $scope.housing.number_of_houses_construction;
            $scope.housing.total_deposit = ($scope.housing.total_deposit == null || $scope.housing.total_deposit == undefined) ? 0 : $scope.housing.total_deposit;
            $scope.housing.pack_total_outstanding_loan = ($scope.housing.pack_total_outstanding_loan == null || $scope.housing.pack_total_outstanding_loan == undefined) ? 0 : $scope.housing.pack_total_outstanding_loan;
            $scope.ncd.housing = { ...$scope.housing };
        }
        else if ($scope.ncd.sector_of_operation == 82) {
            $scope.marketing.building_type = $scope.marketing.has_building == 0 ? 0 : $scope.marketing.building_type;
            $scope.marketing.capacity_warehouses = $scope.marketing.has_warehouses == 0 ? 0 : $scope.marketing.capacity_warehouses;
            $scope.marketing.land.land_owned = $scope.marketing.has_land == 0 ? 0 : $scope.marketing.land.land_owned;
            $scope.marketing.land.land_leased = $scope.marketing.has_land == 0 ? 0 : $scope.marketing.land.land_leased;
            $scope.marketing.land.land_allotted = $scope.marketing.has_land == 0 ? 0 : $scope.marketing.land.land_allotted;
            $scope.marketing.land.land_total = $scope.marketing.has_land == 0 ? 0 : $scope.marketing.land.land_total;
            $scope.marketing.authorised_share = ($scope.marketing.authorised_share == null || $scope.marketing.authorised_share == undefined) ? 0 : $scope.marketing.authorised_share;
            $scope.marketing.paid_up_members = ($scope.marketing.paid_up_members == null || $scope.marketing.paid_up_members == undefined) ? 0 : $scope.marketing.paid_up_members;
            $scope.ncd.marketing = { ...$scope.marketing };
        }
        else if ($scope.ncd.sector_of_operation == 11) {
            $scope.Sugar.retail_shops_no = $scope.Sugar.retail_shops == 0 ? null : $scope.Sugar.retail_shops_no;
            $scope.Sugar.building_type = $scope.Sugar.has_building == 0 ? null : $scope.Sugar.building_type;
            $scope.ncd.Sugar = { ...$scope.Sugar };
        }
        else if ($scope.ncd.sector_of_operation == 29) {
            $scope.Miscellaneous.building_type = $scope.Miscellaneous.has_building == 0 ? null : $scope.Miscellaneous.building_type;
            $scope.ncd.Miscellaneous = { ...$scope.Miscellaneous };
        }
        else if ($scope.ncd.sector_of_operation == 16) {
            $scope.Multipurpose.building_type = $scope.Multipurpose.has_building == 0 ? null : $scope.Multipurpose.building_type;
            $scope.Multipurpose.storage_capacity = $scope.Multipurpose.has_storage == 0 ? null : $scope.Multipurpose.storage_capacity;
            $scope.Multipurpose.guidance_by_member = $scope.Multipurpose.provide_raw == 0 ? 0 : $scope.Multipurpose.guidance_by_member;
            $scope.ncd.Multipurpose = { ...$scope.Multipurpose };
        }
        else if ($scope.ncd.sector_of_operation == 51) {
            $scope.Labour.type_society = ($scope.Labour.type_society == null || $scope.Labour.type_society == '' || $scope.Labour.type_society == undefined) ? 0 : $scope.Labour.type_society;
            $scope.Labour.building_type = $scope.Labour.has_building == 0 ? 0 : $scope.Labour.building_type;
            $scope.Labour.authorised_share = ($scope.Labour.authorised_share == null || $scope.Labour.authorised_share == undefined) ? 0 : $scope.Labour.authorised_share;
            $scope.Labour.paid_up_members = ($scope.Labour.paid_up_members == null || $scope.Labour.paid_up_members == undefined) ? 0 : $scope.Labour.paid_up_members;
            $scope.Labour.paid_up_government_bodies = ($scope.Labour.paid_up_government_bodies == null || $scope.Labour.paid_up_government_bodies == undefined) ? 0 : $scope.Labour.paid_up_government_bodies;
            $scope.Labour.paid_up_total = ($scope.Labour.paid_up_total == null || $scope.Labour.paid_up_total == undefined) ? 0 : $scope.Labour.paid_up_total;
            $scope.Labour.annual_turn_over = ($scope.Labour.annual_turn_over == null || $scope.Labour.annual_turn_over == undefined) ? 0 : $scope.Labour.annual_turn_over;
            $scope.Labour.total_deposit = ($scope.Labour.total_deposit == null || $scope.Labour.total_deposit == undefined) ? 0 : $scope.Labour.total_deposit;
            $scope.Labour.pack_total_outstanding_loan = ($scope.Labour.pack_total_outstanding_loan == null || $scope.Labour.pack_total_outstanding_loan == undefined) ? 0 : $scope.Labour.pack_total_outstanding_loan;
            $scope.ncd.Labour = { ...$scope.Labour };
        }
        else if ($scope.ncd.sector_of_operation == 54) {
            $scope.LiveStock.building_type = $scope.LiveStock.has_building == 0 ? 0 : $scope.LiveStock.building_type;
            $scope.LiveStock.authorised_share = ($scope.LiveStock.authorised_share == null || $scope.LiveStock.authorised_share == undefined) ? 0 : $scope.LiveStock.authorised_share;
            $scope.LiveStock.annual_turn_over = ($scope.LiveStock.annual_turn_over == null || $scope.LiveStock.annual_turn_over == undefined) ? 0 : $scope.LiveStock.annual_turn_over;
            $scope.LiveStock.total_deposit = ($scope.LiveStock.total_deposit == null || $scope.LiveStock.total_deposit == undefined) ? 0 : $scope.LiveStock.total_deposit;
            $scope.LiveStock.pack_total_outstanding_loan = ($scope.LiveStock.pack_total_outstanding_loan == null || $scope.LiveStock.pack_total_outstanding_loan == undefined) ? 0 : $scope.LiveStock.pack_total_outstanding_loan;
            $scope.LiveStock.Paid_up_share = ($scope.LiveStock.Paid_up_share == null || $scope.LiveStock.Paid_up_share == undefined) ? 0 : $scope.LiveStock.Paid_up_share;
            $scope.LiveStock.is_training_provide = $scope.LiveStock.is_work_by_member == 0 ? 0 : $scope.LiveStock.is_training_provide;
            $scope.LiveStock.no_of_retail = $scope.LiveStock.is_operate_retail == 0 ? 0 : $scope.LiveStock.no_of_retail;

            $scope.ncd.LiveStock = { ...$scope.LiveStock };
        }
        else if ($scope.ncd.sector_of_operation == 68) {
            $scope.Transport.building_type = $scope.Transport.has_building == 0 ? null : $scope.Transport.building_type;
            $scope.ncd.Transport = { ...$scope.Transport };
        }
        else if ($scope.ncd.sector_of_operation == 90) {
            $scope.Jute.building_type = $scope.Jute.has_building == 0 ? 0 : $scope.Jute.building_type;
            $scope.Jute.authorised_share = ($scope.Jute.authorised_share == null || $scope.Jute.authorised_share == undefined) ? 0 : $scope.Jute.authorised_share;
            $scope.Jute.annual_turn_over = ($scope.Jute.annual_turn_over == null || $scope.Jute.annual_turn_over == undefined) ? 0 : $scope.Jute.annual_turn_over;
            $scope.Jute.workplace_operate = $scope.Jute.common_work_place == 0 ? 0 : $scope.Jute.workplace_operate;
            $scope.Jute.is_training_provide = $scope.Jute.is_work_by_member == 0 ? 0 : $scope.Jute.is_training_provide;
            $scope.Jute.is_waste_facility = $scope.Jute.is_waste_generate == 0 ? 0 : $scope.Jute.is_waste_facility;
            $scope.Jute.no_of_retail = $scope.Jute.is_operate_retail == 0 ? 0 : $scope.Jute.no_of_retail;
            $scope.Jute.total_deposit = ($scope.Jute.total_deposit == null || $scope.Jute.total_deposit == undefined) ? 0 : $scope.Jute.total_deposit;
            $scope.Jute.pack_total_outstanding_loan = ($scope.Jute.pack_total_outstanding_loan == null || $scope.Jute.pack_total_outstanding_loan == undefined) ? 0 : $scope.Jute.pack_total_outstanding_loan;
            $scope.Jute.Paid_up_share = ($scope.Jute.Paid_up_share == null || $scope.Jute.Paid_up_share == undefined) ? 0 : $scope.Jute.Paid_up_share;
            $scope.ncd.Jute = { ...$scope.Jute };
        }
        else if ($scope.ncd.sector_of_operation == 96) {
            $scope.Sericulture.building_type = $scope.Sericulture.has_building == 0 ? null : $scope.Sericulture.building_type;
            $scope.Sericulture.no_of_retail = $scope.Sericulture.is_operate_retail == 0 ? null : $scope.Sericulture.no_of_retail;
            $scope.Sericulture.no_rear_house = $scope.Sericulture.common_work_place == 0 ? null : $scope.Sericulture.no_rear_house;
            $scope.Sericulture.is_training_provide = $scope.Sericulture.is_work_by_member == 0 ? 0 : $scope.Sericulture.is_training_provide;
            $scope.ncd.Sericulture = { ...$scope.Sericulture };
        }
        else if ($scope.ncd.sector_of_operation == 98) {
            $scope.SocialWelfare.building_type = $scope.SocialWelfare.has_building == 0 ? null : $scope.SocialWelfare.building_type;
            $scope.SocialWelfare.no_of_vehicle = $scope.SocialWelfare.is_operate_vehicle == 0 ? null : $scope.SocialWelfare.no_of_vehicle;
            $scope.SocialWelfare.guidance_by_member = $scope.SocialWelfare.is_operate_by_member == 0 ? 0 : $scope.SocialWelfare.guidance_by_member;
            $scope.ncd.SocialWelfare = { ...$scope.SocialWelfare };
        }
        else if ($scope.ncd.sector_of_operation == 99) {
            $scope.Tourism.building_type = $scope.Tourism.has_building == 0 ? null : $scope.Tourism.building_type;
            $scope.ncd.Tourism = { ...$scope.Tourism };
        }
        else if ($scope.ncd.sector_of_operation == 102) {
            $scope.Tribal.building_type = $scope.Tribal.has_building == 0 ? null : $scope.Tribal.building_type;
            $scope.ncd.Tribal = { ...$scope.Tribal };
        }
        $scope.fixNullStrings($scope.ncd);
        //return;

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/NcdData/SaveNcdData", { ncdJson: JSON.stringify($scope.ncd), cooperative_id: (cooperativeId ?? "") }).then(function (res) {
            $scope.saving = false;
            if (res.data && res.data.Result && res.data.Result.Status == "200") {
                if (res.data.Result.Data.ncdData !== null) {
                    $scope.ncdData = JSON.parse(res.data.Result.Data.ncdData);
                    if ($scope.ncdData[0].cooperativeId !== '' && $scope.ncdData[0].status == 1) {
                        alert($scope.ncdData[0].message);
                        //toastr.success($scope.ncdData[0].message);
                        location.reload();
                    }
                    else {
                        alert("Some error occured.");
                    }
                }
            }
            $(".se-pre-con").fadeOut("slow");
        }, function (err) {
            $scope.saving = false;
            console.error("HTTP error:", err);
            alert("HTTP error while saving. See console.");
            $(".se-pre-con").fadeOut("slow");
        });
    };

    //01-10-2025
    $scope.GetCategoryUrbanLocalList = function () {
        $scope.UrbanCategoryLocalList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetUrbanCategoryLocalBody").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.urbanCategoryData != null) {
                    $scope.UrbanCategoryLocalList = JSON.parse(data.Result.Data.urbanCategoryData);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    }

    $scope.GetUrbanLocalBodyByTypecode = function (bodyTypeCode) {
        $scope.urbanBodyTypeDetailList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetUrbanLocalBodyByTypecode?localbody_type_code=" + bodyTypeCode).
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.urbanBodyTypeDetail != null) {
                    $scope.urbanBodyTypeDetailList = JSON.parse(data.Result.Data.urbanBodyTypeDetail);
                    $scope.fillAddress();
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    }

    $scope.GetWardDetails = function (local_body_code) {
        $scope.wardDetailList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetWardDetailsByCode?local_body_code=" + local_body_code).
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.WardDetails != null) {
                    $scope.wardDetailList = JSON.parse(data.Result.Data.WardDetails);
                    $scope.fillAddress();
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    }

    $scope.GetWaterBodyTypeList = function () {
        $scope.waterBodyTypeList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetWaterBodyTypeDetails").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.WaterBodyDetails != null) {
                    $scope.waterBodyTypeList = JSON.parse(data.Result.Data.WaterBodyDetails);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.GetAgricultureFarmingMechanisms = function () {
        $scope.farmingMechanismsList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetAgricultureFarmingMechanisms").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.FarmingMechanismDetail != null) {
                    $scope.farmingMechanismsList = JSON.parse(data.Result.Data.FarmingMechanismDetail);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetLivestockProductProduce = function () {
        $scope.livestockProduceList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetLivestockProductProduce").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.LivestockProduceDetails != null) {
                    $scope.livestockProduceList = JSON.parse(data.Result.Data.LivestockProduceDetails);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetJuteProductProduce = function () {
        $scope.juteProduceList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetJuteProductProduce").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.juteProduceDetails != null) {
                    $scope.juteProduceList = JSON.parse(data.Result.Data.juteProduceDetails);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.GetSugarProductProduce = function () {
        $scope.sugarProductProcudList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetSugarProductProduce").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.sugarProduceDetails != null) {
                    $scope.sugarProductProcudList = JSON.parse(data.Result.Data.sugarProduceDetails);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetJuteRawMeterialDetail = function () {
        $scope.juteRawMeterialList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetJuteRawMeterialDetail").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.juteRawMeterialDetail != null) {
                    $scope.juteRawMeterialList = JSON.parse(data.Result.Data.juteRawMeterialDetail);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetHandicraftRawMeterialDetail = function () {
        $scope.handicraftRawMeterialList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetHandicraftRawMeterialDetail").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.handicraftRawMeterialDetail != null) {
                    $scope.handicraftRawMeterialList = JSON.parse(data.Result.Data.handicraftRawMeterialDetail);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.GetHandicraftProductProduced = function () {
        $scope.handicraftProductList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetHandicraftProductProduced").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.handicraftProductDetail != null) {
                    $scope.handicraftProductList = JSON.parse(data.Result.Data.handicraftProductDetail);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetTypeSocialCulturalActivities = function () {
        $scope.typeSociaActivitiesList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetTypeSocialCulturalActivities").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.typeSociaActivitiesDetails != null) {
                    $scope.typeSociaActivitiesList = JSON.parse(data.Result.Data.typeSociaActivitiesDetails);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetTypeofBeesCooperative = function () {
        $scope.BeesItems = [];
        $scope.TypesBeesCoopList = [];
        $scope.TypesBeesItemCoopList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetTypeofBeesCooperative").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.BeesCoopDetails != null) {
                    $scope.BeesItems = JSON.parse(data.Result.Data.BeesCoopDetails);
                    $scope.TypesBeesCoopList = $scope.BeesItems.filter(x => x.type_bee == "1");
                    $scope.TypesBeesItemCoopList = $scope.BeesItems.filter(x => x.type_bee == "2");
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.GetBeeFarmingProductProduced = function () {
        $scope.BeeFarmingItems = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetBeeFarmingProductProduced").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.beeFarmingProductDetail != null) {
                    $scope.BeeFarmingItems = JSON.parse(data.Result.Data.beeFarmingProductDetail);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $scope.BeeFarmingItems = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.ToggleChange = function () {
        if ($scope.ncd.is_coastal == 1) {
            $scope.GetWaterBodyTypeList();
            $scope.ncd.water_body_type_id = $scope.ncd.water_body_type_id ? String($scope.ncd.water_body_type_id) : '0';
            $scope.GetAgricultureFarmingMechanisms();
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
            $scope.isTextName = 'annual_profit';
        }
    };

    $scope.GetsectorOperationDetails = function () {
        $scope.sectorOperationList = [
            { id: 1, name: 'Cooperative Bank' },
            { id: 2, name: 'Commercial Bank' }
        ];

    };

    $scope.isPacsPrimaryActivity = function (sector_of_operation_type) {
        $("input, select, textarea").removeClass("invalid-field");
        if (sector_of_operation_type == 20) { $scope.is_pacsTitle = 'FSS Detail'; } else if (sector_of_operation_type == 22) { $scope.is_pacsTitle = 'LAMPS Detail'; }
        else if (sector_of_operation_type == 1) {
            $scope.is_pacsTitle = 'Pacs Detail';
        }
        $scope.GetSocietyFacilityByPrimaryActivity(sector_of_operation_type);
        $scope.GetTypeOfWomenCooperative(sector_of_operation_type);
        $scope.GetSecondryActivityBySectorOperation(sector_of_operation_type);
        if ($scope.ncd.sector_of_operation == 77) {
            $scope.GetWaterBodyTypeList();
            $scope.GetAgricultureFarmingMechanisms();
        }
        else if ($scope.ncd.sector_of_operation == 79) { $scope.GetTypeofBeesCooperative(); $scope.GetBeeFarmingProductProduced(); }
        else if ($scope.ncd.sector_of_operation == 117) { $scope.GetKhadiGramSocietyActivity(); }
        else if ($scope.ncd.sector_of_operation == 54) { $scope.GetLivestockProductProduce(); }
        else if ($scope.ncd.sector_of_operation == 68) { $scope.GetTransportVehicleOwenership($scope.ncd.sector_of_operation); }
        else if ($scope.ncd.sector_of_operation == 90) { $scope.GetJuteProductProduce(); $scope.GetJuteRawMeterialDetail(); }
        else if ($scope.ncd.sector_of_operation == 98) { $scope.GetTypeSocialCulturalActivities(); }
        else if ($scope.ncd.sector_of_operation == 14) { $scope.GetHandicraftRawMeterialDetail(); $scope.GetHandicraftProductProduced(); }
        else if ($scope.ncd.sector_of_operation == 11) { $scope.GetSugarProductProduce(); }
    }

    $scope.GetSocietyFacilityByPrimaryActivity = function (sector_of_operation) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetSocietyFacilityByPrimaryActivity?primaryId=" + sector_of_operation).then(function (response) {
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null) {
                $scope.facilitiesList = JSON.parse(response.data.Result.Data.facilitiesDetails);
            } else {
                $scope.facilitiesList = [];
            }
            $(".se-pre-con").fadeOut("slow");
        }, function (error) {
            console.error("Error loading sector Of Operations:", error);
            $scope.facilitiesList = [];
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.GetEducationCourseOfferedBySector = function (sector_of_operation) {
        $scope.EducationCourseList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetEducationCourseOfferedBySector?sectorId=" + sector_of_operation).then(function (response) {
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null) {
                $scope.EducationCourseList = JSON.parse(response.data.Result.Data.EducationCourseDatail);
            } else {
                $scope.EducationCourseList = [];
            }
            $(".se-pre-con").fadeOut("slow");
        }, function (error) {
            console.error("Error loading sector Of Operations:", error);
            $scope.EducationCourseList = [];
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.GetKhadiGramSocietyActivity = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetKhadiGramSocietyActivity").then(function (response) {
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null) {
                $scope.KhadiGramSocietyActivityList = JSON.parse(response.data.Result.Data.KhadiGramSocietyActivityDetails);
            } else {
                $scope.KhadiGramSocietyActivityList = [];
            }
            $(".se-pre-con").fadeOut("slow");
        }, function (error) {
            console.error("Error loading sector Of Operations:", error);
            $scope.KhadiGramSocietyActivityList = [];
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.GetSecondryActivityBySectorOperation = function (sector_of_operation) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetSecondryActivityBySectorOperation?sectorOperation=" + sector_of_operation).then(function (response) {
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null) {
                $scope.secondaryActivityList = JSON.parse(response.data.Result.Data.sectorOperationDetails);
            } else {
                $scope.secondaryActivityList = [];
            }
            $(".se-pre-con").fadeOut("slow");
        }, function (error) {
            console.error("Error loading sector Of Operations:", error);
            $scope.secondaryActivityList = [];
            $(".se-pre-con").fadeOut("slow");
        });
    };
    $scope.GetTransportVehicleOwenership = function (sector_of_operation) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetTransportVehicleOwenership?sectorOperation=" + sector_of_operation).then(function (response) {
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null) {
                $scope.vehicleOwershipList = JSON.parse(response.data.Result.Data.vehicleOwenership);
            } else {
                $scope.vehicleOwershipList = [];
            }
            $(".se-pre-con").fadeOut("slow");
        }, function (error) {
            console.error("Error loading sector Of Operations:", error);
            $scope.vehicleOwershipList = [];
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.GetTypeOfWomenCooperative = function (sector_of_operation) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetTypeOfWomenCooperative?primaryId=" + sector_of_operation).then(function (response) {
            if (response.data != null && response.data.Result != null && response.data.Result.Data != null) {
                $scope.typesofWomenCoopList = JSON.parse(response.data.Result.Data.womenCoopDetails);
            } else {
                $scope.typesofWomenCoopList = [];
            }
            $(".se-pre-con").fadeOut("slow");
        }, function (error) {
            console.error("Error loading sector Of Operations:", error);
            $scope.typesofWomenCoopList = [];
            $(".se-pre-con").fadeOut("slow");
        });
    };
    $scope.convertDate = function (dateStr) {
        if (!dateStr) return ''; let parts = dateStr.split('-'); if (parts.length !== 3) return dateStr;
        let dd = parts[2]; let mm = parts[1]; let yyyy = parts[0]; return `${yyyy}-${mm}-${dd}`;
    };
    $scope.addSector = function () {
        var lastData = $scope.ncd.sectorListData[$scope.ncd.sectorListData.length - 1];

        // ---- RURAL ----
        if ($scope.sectorDetail.operation_area_location == "2") {
            if (lastData && lastData.district_code && lastData.block_code && lastData.panchayat_code && lastData.village_code) {
                $scope.ncd.sectorListData.push({
                    AreaofOperationUrbanDetailsId: 0,
                    AreaofOperationRuralDetailsId: 0,
                    operation_area_location: $scope.sectorDetail.operation_area_location,
                    district_code: '',
                    block_code: '',
                    panchayat_code: '',
                    village_code: '',
                    // keep Urban empty
                    local_body_type_code: '',
                    local_body_code: '',
                    locality_ward_code: '',
                    area_of_operation_id: $scope.sectorDetail.area_of_operation_id,
                    is_deleted: "0"
                });
            } else {
                alert("Please fill all rural fields in the last row before adding a new one.");
            }
        }

        // ---- URBAN ----
        else if ($scope.sectorDetail.operation_area_location == "1") {
            if (lastData && lastData.local_body_type_code && lastData.local_body_code && lastData.locality_ward_code) {
                $scope.ncd.sectorListData.push({
                    AreaofOperationUrbanDetailsId: 0,
                    AreaofOperationRuralDetailsId: 0,
                    operation_area_location: $scope.sectorDetail.operation_area_location,
                    local_body_type_code: '',
                    local_body_code: '',
                    locality_ward_code: '',
                    // keep Rural empty
                    district_code: '',
                    block_code: '',
                    panchayat_code: '',
                    village_code: '',
                    area_of_operation_id: $scope.sectorDetail.area_of_operation_id,
                    is_deleted: "0"
                });
            } else {
                alert("Please fill all urban fields in the last row before adding a new one.");
            }
        }
        //---- Both ----
        else if ($scope.sectorDetail.operation_area_location == "3") {
            if (lastData && lastData.district_code && lastData.block_code && lastData.panchayat_code && lastData.village_code && lastData.local_body_type_code && lastData.local_body_code && lastData.locality_ward_code) {
                $scope.ncd.sectorListData.push({
                    AreaofOperationUrbanDetailsId: 0,
                    AreaofOperationRuralDetailsId: 0,
                    operation_area_location: $scope.sectorDetail.operation_area_location,
                    local_body_type_code: '',
                    local_body_code: '',
                    locality_ward_code: '',
                    // keep Rural empty
                    district_code: '',
                    block_code: '',
                    panchayat_code: '',
                    village_code: '',
                    area_of_operation_id: $scope.sectorDetail.area_of_operation_id,
                    is_deleted: "0"
                });
            } else {
                alert("Please fill all urban fields in the last row before adding a new one.");
            }
        }
    };
    $scope.GetBlockByStateDistrictBySectorDynamic = function (sector, statecode, districtcode) {
        if (!statecode || !districtcode) {
            sector.NCDBlockListDetails = [];
            return;
        }
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetBlockByStateDistrict?statecode=" + statecode + "&districtcode=" + districtcode).success(function (data) {
            if (data.Result.Data && data.Result.Data.NCDBlockDetail) {
                sector.NCDBlockListDetails = JSON.parse(data.Result.Data.NCDBlockDetail);
            }
            $(".se-pre-con").fadeOut("slow");
        }).error(function () {
            $(".se-pre-con").fadeOut("slow");
        });
    };
    $scope.GetGramPanchayatByStateDistrictBlockBySectorDynamic = function (sector, statecode, districtcode, blockcode) {
        if (!statecode || !districtcode || !blockcode) {
            sector.NCDGramPanchayatListDetails = [];
            return;
        }

        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetGramPanchayatByStateDistrictBlock?statecode=" + statecode + "&districtcode=" + districtcode + "&blockcode=" + blockcode).success(function (data) {
            if (data.Result.Data && data.Result.Data.NCDGramPanchayatDetail) {
                sector.NCDGramPanchayatListDetails = JSON.parse(data.Result.Data.NCDGramPanchayatDetail);
            }
            $(".se-pre-con").fadeOut("slow");
        }).error(function () {
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.GetVillageByStateDistrictBlockGramPanchayatBySectorDynamic = function (sector, statecode, districtcode, blockcode, panchayatcode) {
        if (!statecode || !districtcode || !blockcode || !panchayatcode) {
            sector.NCDVillageListDetails = [];
            return;
        }
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetVillageByStateDistrictBlockGramPanchayat?statecode=" + statecode + "&districtcode=" + districtcode + "&blockcode=" + blockcode + "&grampanchayatcode=" + panchayatcode).success(function (data) {
            if (data.Result.Data && data.Result.Data.NCDVillageDetail) {
                sector.NCDVillageListDetails = JSON.parse(data.Result.Data.NCDVillageDetail);
            }
            $(".se-pre-con").fadeOut("slow");
        }).error(function () {
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.GetUrbanLocalBodyByTypecodeDynamic = function (sector, local_body_type_code) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetUrbanLocalBodyByTypecode?localbody_type_code=" + local_body_type_code).success(function (data) {
            if (data.Result.Data != null && data.Result.Data.urbanBodyTypeDetail != null) {
                sector.urbanBodyTypeDetailDataList = JSON.parse(data.Result.Data.urbanBodyTypeDetail);
            }
            $(".se-pre-con").fadeOut("slow");
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
        });
    }

    $scope.GetWardDetailsDynamic = function (sector, local_body_code) {
        if (!local_body_code) {
            sector.wardDetailList = [];
            return;
        }
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetWardDetailsByCode?local_body_code=" + local_body_code).success(function (data, status) {
            if (data.Result.Data != null && data.Result.Data.WardDetails != null) {
                sector.wardDetailDataList = JSON.parse(data.Result.Data.WardDetails);
            }
            $(".se-pre-con").fadeOut("slow");
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
        });
    }
    $scope.removeSector = function (index) {
        var list = $scope.ncd.sectorListData;
        var activeCount = list.filter(function (x) {
            return x.is_deleted !== 1;
        }).length;

        if (activeCount <= 1) {
            alert("At least one sector record is mandatory.");
            return;
        }

        var sector = list[index];
        if ((sector.AreaofOperationUrbanDetailsId && sector.AreaofOperationUrbanDetailsId > 0) || (sector.AreaofOperationRuralDetailsId && sector.AreaofOperationRuralDetailsId > 0)) {
            sector.is_deleted = 1;
        }
        else {
            list.splice(index, 1);
        }
    };


    $scope.isLastVisibleSector = function (index) {
        var lastIndex = -1;

        angular.forEach($scope.ncd.sectorListData, function (sector, i) {
            if (sector.is_deleted == 0) {
                lastIndex = i;
            }
        });
        return index == lastIndex;
    };

    $scope.addUCBScheme = function () {
        var lastScheme = $scope.ucb.schemesdetail[$scope.ucb.schemesdetail.length - 1];
        if (lastScheme.gov_scheme_name && lastScheme.gov_scheme_type !== '' && lastScheme.total_amount) {
            $scope.ucb.schemesdetail.push({
                ncd_society_id: 0,
                gov_scheme_name: '',
                gov_scheme_type: '',
                total_amount: ''
            });
        } else {
            alert("Please fill out all fields in the last row before adding a new one.");
        }
    };
    $scope.removeUCBScheme = function (index) {
        $scope.ucb.schemesdetail.splice(index, 1);
    };

    $scope.addRowKhadiGramProduct = function () {
        var lastRow = $scope.KhadiGram.products[$scope.KhadiGram.products.length - 1];
        if (lastRow.item_name !== '' && lastRow.yearly_production !== '' && lastRow.unit !== '' && lastRow.value_in_rs !== '') {
            $scope.KhadiGram.products.push({ item_name: '', yearly_production: '', unit: '', value_in_rs: '' });
        } else {
            alert("Please fill out all fields in the last row before adding a new one.");
        }
    };

    $scope.removeRow = function (index) {
        $scope.KhadiGram.products.splice(index, 1);
    };

    $scope.onProductOptionChange = function () {
        if ($scope.KhadiGram.do_you_want_to_enter_type_products_produced == 1) {
            if ($scope.KhadiGram.products.length == 0) {
                $scope.KhadiGram.products.push({ ncd_coop_reg_khadi_gram_prod_id: 0, item_name: '', yearly_production: '', unit: '', value_in_rs: '' });
            }
        } else {
            $scope.KhadiGram.products = [];
            $scope.KhadiGram.total_value_in_rs = '';
        }
    };

    $scope.GetTotalProduct = function () {
        $scope.KhadiGram.total_value_in_rs = 0;
        angular.forEach($scope.KhadiGram.products, function (item) {
            $scope.KhadiGram.total_value_in_rs += parseFloat(item.value_in_rs) || 0;
        });
        return $scope.KhadiGram.total_value_in_rs;
    };


    $scope.updateSocietyItems = function () {
        $scope.marketing.sell_the_item = $scope.societyItems.filter(item => item.hasLicense).map(item => item.id);
        $scope.marketing.liecense_to_sell = $scope.societyItems.filter(item => item.sellsItem).map(item => item.id);
    };

    $scope.GetDetailsByCooperative = function (ncdId) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetNcdCooperativeData?ncdId=" + ncdId).then(function (response) {
            if (response.data != null && response.data.CooperativeRegistrations != null) {
                $(".se-pre-con").fadeOut("slow");
                $scope.cooperativeDetailData = response.data.CooperativeRegistrations;
                if ($scope.cooperativeDetailData != null && $scope.cooperativeDetailData.length > 0) {
                    $scope.ncd = $scope.cooperativeDetailData[0];
                    $scope.ncd.deregistered = ($scope.ncd.deregistered == undefined || $scope.ncd.deregistered == null) ? "0" : $scope.ncd.deregistered;
                    $scope.ncd.date_registration = new Date($scope.convertDate($scope.cooperativeDetailData[0].date_registration));
                    if ($scope.cooperativeDetailData[0].date_deregistered != null) {
                        $scope.ncd.date_deregistered = new Date($scope.convertDate($scope.cooperativeDetailData[0].date_deregistered));
                    }
                    $scope.filterPrimaryActivity($scope.ncd.sector_of_operation_type);
                    $scope.GetBlockByStateDistrict($scope.ncd.state_code, $scope.ncd.district_code);
                    $scope.GetGramPanchayatByStateDistrictBlock($scope.ncd.state_code, $scope.ncd.district_code, $scope.ncd.block_code);
                    $scope.GetVillageByStateDistrictBlockGramPanchayat($scope.ncd.state_code, $scope.ncd.district_code, $scope.ncd.block_code, $scope.ncd.gram_panchayat_code);
                    $scope.GetCategoryUrbanLocalList();
                    $scope.GetUrbanLocalBodyByTypecode($scope.ncd.urban_local_body_type_code);
                    $scope.GetWardDetails($scope.ncd.urban_local_body_code);
                    $scope.ncd.operation_area_locationOld = $scope.ncd.operation_area_location;
                    $scope.ncd.operation_area_locationNew = 0;
                    $scope.GetAreaOfOperations($scope.ncd.operation_area_location);

                    $scope.ncd.is_coastal = ($scope.ncd.is_coastal == "" || $scope.ncd.is_coastal == null ? "0" : $scope.ncd.is_coastal);
                    $scope.ncd.financial_audit = ($scope.ncd.financial_audit == "" || $scope.ncd.financial_audit == null ? "0" : $scope.ncd.financial_audit);
                    if ($scope.ncd.is_profit_making == 0) {
                        $scope.ncd.annualPayment = parseFloat(parseFloat($scope.ncd.annual_loss).toFixed(2));
                    } else {
                        $scope.ncd.annualPayment = parseFloat(parseFloat($scope.ncd.annual_profit).toFixed(2));
                    }

                    if ($scope.ncd.financial_audit == 1) {
                        $scope.ncd.annual_turnover = parseFloat(parseFloat($scope.ncd.annual_turnover).toFixed(2));
                    }
                    else {
                        $scope.ncd.annual_turnover = 0.00;
                    }

                    $scope.GetsectorOperationDetails();
                    if ($scope.ncd.bank_type) {
                        $scope.ncd.bank_typeList = $scope.ncd.bank_type.split(",").map(x => parseInt(x.trim()));
                    } else {
                        $scope.ncd.bank_typeList = [];
                    }


                    $scope.ncdm = response.data.MembersDetails[0];
                    $scope.ncd.sector_of_operation_type = $scope.ncd.sector_of_operation_type + '';
                    $scope.ncd.sector_of_operation = $scope.ncd.sector_of_operation + '';
                    $scope.ncd.district_code = $scope.ncd.district_code + '';
                    $scope.ncd.state_code = $scope.ncd.state_code + '';
                    $scope.ncd.block_code = $scope.ncd.block_code + '';
                    $scope.ncd.gram_panchayat_code = $scope.ncd.gram_panchayat_code + '';
                    $scope.ncd.village_code = $scope.ncd.village_code + '';
                    $scope.ncd.operation_area_location = $scope.ncd.operation_area_location + '';
                    $scope.ncd.registration_authoritie_id = $scope.ncd.registration_authoritie_id + '';
                    $scope.ncd.functional_status = $scope.ncd.functional_status + '';
                    $scope.ncd.designation = $scope.ncd.designation + '';
                    $scope.ncd.bank_type = $scope.ncd.bank_type + '';

                    $scope.ncd.urban_local_body_type_code = $scope.ncd.urban_local_body_type_code + '';
                    $scope.ncd.urban_local_body_code = $scope.ncd.urban_local_body_code + '';
                    $scope.ncd.locality_ward_code = $scope.ncd.locality_ward_code + '';

                    if ($scope.ncd.bank_type !== null && $scope.ncd.bank_type !== "") {
                        $scope.ncd.bank_type = $scope.ncd.bank_type.toString().split(',');
                        $scope.filterBankType($scope.ncd.bank_type);
                        if ($scope.ncd.cooperative_society_bank_id)
                            $scope.ncd.cooperative_society_bank_id = $scope.ncd.cooperative_society_bank_id.toString().split(',');
                    }

                    $scope.ncd.audit_complete_year = $scope.ncd.audit_complete_year == 0 ? '' : String($scope.ncd.audit_complete_year);
                    //$scope.ncd.audit_complete_year = $scope.ncd.audit_complete_year + '';

                    if ($scope.ncd.is_coastal == 1) {
                        $scope.GetWaterBodyTypeList();
                        $scope.ncd.water_body_type_id = $scope.ncd.water_body_type_id + '';
                    }
                    $scope.ncd.category_audit = $scope.ncd.category_audit + '';
                    $scope.sectorDetail.operation_area_location = $scope.ncd.operation_area_location;
                    if ($scope.sectorDetail.operation_area_location == 1) {
                        $scope.ncd.sectorListData = response.data.AreaOfOperationLevelUrban;
                        $scope.sectorListDataOld = response.data.AreaOfOperationLevelUrban;

                    }
                    else if ($scope.sectorDetail.operation_area_location == 2) {
                        $scope.ncd.sectorListData = response.data.AreaOfOperationLevel;
                        $scope.sectorListDataOld = response.data.AreaOfOperationLevel;
                    }
                    else if ($scope.sectorDetail.operation_area_location == 3) {
                        $scope.bothSectorList = [];
                        var ruralList = response.data.AreaOfOperationLevel || [];
                        var urbanList = response.data.AreaOfOperationLevelUrban || [];
                        ruralList = Array.isArray(ruralList) ? ruralList : [ruralList];
                        urbanList = Array.isArray(urbanList) ? urbanList : [urbanList];
                        $scope.bothSectorList = [];
                        ruralList.forEach(function (rural) {
                            var urban = urbanList.find(function (u) {
                                return u.ncd_id == rural.ncd_id && u.state_code == rural.state_code && u.district_code == rural.district_code;
                            }) || {};
                            var bothSectorObj = {
                                area_of_operation_id: rural.area_of_operation_id,
                                state_code: rural.state_code,
                                district_code: rural.district_code,
                                block_code: rural.block_code,
                                panchayat_code: rural.panchayat_code,
                                village_code: rural.village_code,
                                gp_village_all: rural.gp_village_all,
                                local_body_type_code: urban.local_body_type_code,
                                local_body_code: urban.local_body_code,
                                locality_ward_code: urban.locality_ward_code
                            };
                            $scope.bothSectorList.push(bothSectorObj);
                        });
                        $scope.ncd.sectorListData = $scope.bothSectorList;
                        $scope.sectorListDataOld = $scope.ncd.sectorListData;
                    }
                    if ($scope.ncd.sectorListData.length > 0) {
                        $scope.sectorDetail.area_of_operation_id = $scope.ncd.sectorListData[0].area_of_operation_id + '';
                        $.each($scope.ncd.sectorListData, function (i, v) {
                            if ($scope.ncd.operation_area_location == 1) {
                                $scope.GetDistrictByStatecode(v.state_code);
                                $scope.GetCategoryUrbanLocalList();
                                $scope.GetUrbanLocalBodyByTypecodeDynamic(v, v.local_body_type_code);
                                $scope.GetWardDetailsDynamic(v, v.local_body_code);
                                $scope.ncd.sectorListData.map(x => {
                                    x.state_code = x.state_code + '';
                                    x.district_code = x.district_code + '';
                                    x.local_body_type_code = x.local_body_type_code + '';
                                    x.local_body_code = x.local_body_code + '';
                                    x.locality_ward_code = x.locality_ward_code + '';

                                });
                            }
                            else if ($scope.ncd.operation_area_location == 2) {
                                $scope.GetDistrictByStatecode(v.state_code);
                                $scope.GetBlockByStateDistrictBySectorDynamic(v, v.state_code, v.district_code);
                                $scope.GetGramPanchayatByStateDistrictBlockBySectorDynamic(v, v.state_code, v.district_code, v.block_code);
                                $scope.GetVillageByStateDistrictBlockGramPanchayatBySectorDynamic(v, v.state_code, v.district_code, v.block_code, v.panchayat_code);
                                $scope.ncd.sectorListData.map(x => {
                                    x.state_code = x.state_code + '';
                                    x.district_code = x.district_code + '';
                                    x.block_code = x.block_code + '';
                                    x.panchayat_code = x.panchayat_code + '';
                                    x.village_code = x.village_code + '';
                                })
                            }
                            else if ($scope.ncd.operation_area_location == 3) {
                                $scope.GetDistrictByStatecode(v.state_code);
                                $scope.GetCategoryUrbanLocalList();
                                $scope.GetUrbanLocalBodyByTypecodeDynamic(v, v.local_body_type_code);
                                $scope.GetWardDetailsDynamic(v, v.local_body_code);
                                $scope.GetBlockByStateDistrictBySectorDynamic(v, v.state_code, v.district_code);
                                $scope.GetGramPanchayatByStateDistrictBlockBySectorDynamic(v, v.state_code, v.district_code, v.block_code);
                                $scope.GetVillageByStateDistrictBlockGramPanchayatBySectorDynamic(v, v.state_code, v.district_code, v.block_code, v.panchayat_code);
                                $scope.ncd.sectorListData.map(x => {
                                    x.state_code = x.state_code + '';
                                    x.district_code = x.district_code + '';
                                    x.block_code = x.block_code + '';
                                    x.panchayat_code = x.panchayat_code + '';
                                    x.village_code = x.village_code + '';
                                    x.local_body_type_code = x.local_body_type_code + '';
                                    x.local_body_code = x.local_body_code + '';
                                    x.locality_ward_code = x.locality_ward_code + '';
                                });
                            }
                        })
                    }
                    else {
                        //$scope.sectorDetail.operation_area_location = "2"
                        //$scope.sectorDetail.area_of_operation_id = "3"
                        //$scope.ncd.area_of_operation_id = $scope.sectorDetail.area_of_operation_id + '';
                        //$scope.ncd.sectorListData = [{
                        //    operation_area_location: $scope.sectorDetail.operation_area_location,
                        //    area_of_operation_id: $scope.sectorDetail.area_of_operation_id,
                        //    // Rural Fields
                        //    district_code: '',
                        //    block_code: '',
                        //    panchayat_code: '',
                        //    village_code: '',
                        //    // Urban Fields
                        //    local_body_type_code: '',
                        //    local_body_code: '',
                        //    locality_ward_code: '',
                        //}];
                        //$scope.ncd.sectorListData[0].area_of_operation_id + '';

                        $scope.ncd.sectorListData.push({
                            AreaofOperationUrbanDetailsId: 0,
                            AreaofOperationRuralDetailsId: 0,
                            district_code: '',
                            block_code: '',
                            panchayat_code: '',
                            village_code: '',
                            // Keep rural fields empty
                            local_body_type_code: '',
                            local_body_code: '',
                            locality_ward_code: '',
                            operation_area_location: $scope.ncd.operation_area_location,
                            area_of_operation_id: $scope.sectorDetail.area_of_operation_id,
                            is_deleted: "0"
                        });
                    }

                    $scope.AddMemberDetail(1);

                    if (params.get("ncdId") != null) {
                        setTimeout(function () {
                            $('input,select,textarea,button').prop('disabled', 'disabled');
                            $('#saveBtn').hide();
                        }, 1000);
                    }
                    if ($scope.ncd.sector_of_operation == 7) {
                        $scope.ucb = response.data.SectorData[0];
                        $scope.ucb.schemesdetail = response.data.SocietyImplementingSchemes;
                        if ($scope.ucb.paid_up_share_date != null)
                            $scope.ucb.paid_up_share_date = new Date($scope.convertDate($scope.ucb.paid_up_share_date));

                        if ($scope.ucb.has_building != null) {
                            $scope.GetNcdOfficeBuildingTypes();
                            $scope.ucb.building_type = $scope.ucb.building_type + ''
                        }
                    }
                    else if ($scope.ncd.sector_of_operation == 9) {
                        $scope.dairy = response.data.SectorData[0];
                        if ($scope.dairy.paid_up_share_date != null)
                            $scope.dairy.paid_up_share_date = new Date($scope.convertDate($scope.dairy.paid_up_share_date));
                    }

                    else if ($scope.ncd.sector_of_operation == 1 || $scope.ncd.sector_of_operation == 20 || $scope.ncd.sector_of_operation == 22) {
                        $scope.pacs = response.data.SectorData[0];
                        $scope.isPacsPrimaryActivity($scope.ncd.sector_of_operation);
                        if ($scope.pacs != null) {
                            if ($scope.pacs.paid_up_share_date != null)
                                $scope.pacs.paid_up_share_date = new Date($scope.convertDate($scope.pacs.paid_up_share_date));

                            $scope.pacs.land = response.data.CooperativeRegistrationsLands[0];
                            if ($scope.pacs.has_building != null) {
                                $scope.GetNcdOfficeBuildingTypes();
                                $scope.pacs.building_type = $scope.pacs.building_type == null ? '' : String($scope.pacs.building_type);
                                //$scope.pacs.building_type = $scope.pacs.building_type + ''
                            }
                        }

                    }
                    else if ($scope.ncd.sector_of_operation == 15) {
                        $scope.women = response.data.SectorData[0];
                        $scope.GetTypeOfWomenCooperative($scope.ncd.sector_of_operation);
                        $scope.women.type_society = $scope.women.type_society + '';
                        if ($scope.women.paid_up_share_date != null)
                            $scope.women.paid_up_share_date = new Date($scope.convertDate($scope.women.paid_up_share_date));

                        if ($scope.women.has_building != null) {
                            $scope.GetNcdOfficeBuildingTypes();
                            $scope.women.building_type = $scope.women.building_type == null ? '' : String($scope.women.building_type);
                            //$scope.women.building_type = $scope.women.building_type + ''
                        }

                        $scope.GetSocietyFacilityByPrimaryActivity($scope.ncd.sector_of_operation);
                        if ($scope.women.facilities) {
                            $scope.women.facilities = $scope.women.facilities.toString().split(",");
                        }
                    }

                    else if ($scope.ncd.sector_of_operation == 18) {
                        $scope.tsd = response.data.SectorData[0];
                        $scope.tsd.Paid_up_share_date = new Date($scope.convertDate($scope.tsd.Paid_up_share_date));
                        if ($scope.tsd.has_building != null) {
                            $scope.GetNcdOfficeBuildingTypes();
                            $scope.tsd.building_type = $scope.tsd.building_type == null ? '' : String($scope.tsd.building_type);
                            //$scope.tsd.building_type = $scope.tsd.building_type + '';
                        }
                        $scope.GetSocietyFacilityByPrimaryActivity($scope.ncd.sector_of_operation);
                        if ($scope.tsd.facilities) {
                            $scope.tsd.facilities = $scope.tsd.facilities.toString().split(",");
                        }
                        $scope.ncd.dividend_rate = parseFloat(parseFloat($scope.ncd.dividend_rate).toFixed(2));
                    }
                    else if ($scope.ncd.sector_of_operation == 35) {
                        $scope.csm = response.data.SectorData[0];
                        $scope.csm.Paid_up_share_date = new Date($scope.convertDate($scope.csm.Paid_up_share_date));
                        if ($scope.csm.has_building != null) {
                            $scope.GetNcdOfficeBuildingTypes();
                            $scope.csm.building_type = $scope.csm.building_type == null ? '' : String($scope.csm.building_type);
                            //$scope.csm.building_type = $scope.csm.building_type + '';
                        }
                        $scope.GetSocietyFacilityByPrimaryActivity($scope.ncd.sector_of_operation);
                        if ($scope.csm.facilities) {
                            $scope.csm.facilities = $scope.csm.facilities.toString().split(",");
                        }
                        $scope.csm.members_of_society = parseFloat(((parseFloat($scope.csm.individual_member) || 0) + (parseFloat($scope.csm.institutional_member) || 0)).toFixed(2));
                    }
                    else if ($scope.ncd.sector_of_operation == 77) {
                        $scope.aacs = response.data.SectorData[0];
                        if ($scope.aacs.paid_up_share_date != null)
                            $scope.aacs.paid_up_share_date = new Date($scope.convertDate($scope.aacs.paid_up_share_date));
                        if ($scope.aacs.has_building != null) {
                            $scope.GetNcdOfficeBuildingTypes();
                            $scope.aacs.building_type = $scope.aacs.building_type == null ? '' : String($scope.aacs.building_type);
                            //$scope.aacs.building_type = $scope.aacs.building_type + '';
                        }
                        $scope.GetSecondryActivityBySectorOperation($scope.ncd.sector_of_operation);
                        $scope.GetAgricultureFarmingMechanisms();
                        if ($scope.aacs.farming_mech)
                            $scope.aacs.farming_mech = $scope.aacs.farming_mech.split(',');

                        if ($scope.aacs.irrigation_means)
                            $scope.aacs.irrigation_means = $scope.aacs.irrigation_means.split(',');

                        if ($scope.aacs.type_society)
                            $scope.aacs.type_society = $scope.aacs.type_society.split(',');

                        $scope.aacs.total = parseFloat($scope.aacs.individual_member) + parseFloat($scope.aacs.institutional_member);

                    }
                    else if ($scope.ncd.sector_of_operation == 10) {
                        $scope.Fishery = response.data.SectorData[0];
                        if ($scope.Fishery.paid_up_share_date != null)
                            $scope.Fishery.paid_up_share_date = new Date($scope.convertDate($scope.Fishery.paid_up_share_date));
                    }
                    else if ($scope.ncd.sector_of_operation == 117) {
                        $scope.KhadiGram = response.data.SectorData[0];
                        if ($scope.KhadiGram.paid_up_share_date != null)
                            $scope.KhadiGram.paid_up_share_date = new Date($scope.convertDate($scope.KhadiGram.paid_up_share_date));
                        if ($scope.KhadiGram.has_building != null) {
                            $scope.KhadiGram.building_type = $scope.KhadiGram.building_type == null ? '' : String($scope.KhadiGram.building_type);
                            //$scope.KhadiGram.building_type = $scope.KhadiGram.building_type + '';
                        }

                        $scope.GetKhadiGramSocietyActivity();
                        $scope.KhadiGram.type_of_activities_khadi_gram = $scope.KhadiGram.type_of_activities_khadi_gram + '';
                        $scope.KhadiGram.total_member = parseFloat($scope.KhadiGram.individual_member) + parseFloat($scope.KhadiGram.institutional_member);
                        if ($scope.KhadiGram.do_you_want_to_enter_type_products_produced == 1) {
                            $scope.KhadiGram.products = response.data.KhadiSectorOtherData;
                            $scope.GetTotalProduct();
                        }
                    }
                    else if ($scope.ncd.sector_of_operation == 31) {
                        $scope.crp = response.data.SectorData[0];
                        if ($scope.crp.paid_up_share_date != null)
                            $scope.crp.paid_up_share_date = new Date($scope.convertDate($scope.crp.paid_up_share_date));
                        $scope.crp.type_society = $scope.crp.type_society + '';
                        if ($scope.crp.has_building != null) {
                            $scope.GetNcdOfficeBuildingTypes();
                            $scope.crp.building_type = $scope.crp.building_type == null ? '' : String($scope.crp.building_type);
                            //$scope.crp.building_type = $scope.crp.building_type + '';
                        }
                    }
                    else if ($scope.ncd.sector_of_operation == 79) {
                        $scope.beeCoop = response.data.SectorData[0];
                        if ($scope.beeCoop.paid_up_share_date != null)
                            $scope.beeCoop.paid_up_share_date = new Date($scope.convertDate($scope.beeCoop.paid_up_share_date));
                        if ($scope.beeCoop.has_building != null) {
                            $scope.GetNcdOfficeBuildingTypes();
                            $scope.beeCoop.building_type = $scope.beeCoop.building_type == null ? '' : String($scope.beeCoop.building_type);
                            //$scope.beeCoop.building_type = $scope.beeCoop.building_type + '';
                        }
                        $scope.GetTypeofBeesCooperative();
                        $scope.GetBeeFarmingProductProduced();
                        //$scope.GetBeeFarming();
                        $scope.GetSocietyFacilityByPrimaryActivity($scope.ncd.sector_of_operation);
                        if ($scope.beeCoop.type_bee) { $scope.beeCoop.type_bee = $scope.beeCoop.type_bee.toString().split(","); }
                        if ($scope.beeCoop.type_behives) { $scope.beeCoop.type_behives = $scope.beeCoop.type_behives.toString().split(","); }
                        if ($scope.beeCoop.type_product) {
                            $scope.beeCoop.type_product = $scope.beeCoop.type_product.toString().split(',');
                        } else {
                            $scope.beeCoop.type_product = [];
                        }

                        if ($scope.beeCoop.facilities) {
                            $scope.beeCoop.facilities = $scope.beeCoop.facilities.toString().split(",");
                        }
                    }
                    else if ($scope.ncd.sector_of_operation == 80) {
                        $scope.consumer = response.data.SectorData[0];
                        if ($scope.consumer.paid_up_share_date != null)
                            $scope.consumer.paid_up_share_date = new Date($scope.convertDate($scope.consumer.paid_up_share_date));
                        if ($scope.consumer.has_building != null) {
                            $scope.GetNcdOfficeBuildingTypes();
                            $scope.consumer.building_type = $scope.consumer.building_type == null ? '' : String($scope.consumer.building_type);
                            //$scope.consumer.building_type = $scope.consumer.building_type + '';
                        }
                        $scope.GetSocietyFacilityByPrimaryActivity($scope.ncd.sector_of_operation);
                        if ($scope.consumer.facilities) {
                            $scope.consumer.facilities = $scope.consumer.facilities.toString().split(",");
                        }
                    }
                    else if ($scope.ncd.sector_of_operation == 84) {
                        $scope.education = response.data.SectorData[0];
                        $scope.education.land = response.data.CooperativeRegistrationsLands[0];
                        if ($scope.education.paid_up_share_date != null)
                            $scope.education.paid_up_share_date = new Date($scope.convertDate($scope.education.paid_up_share_date));
                        $scope.education.type_society = $scope.education.type_society + '';
                        if ($scope.education.has_building != null) {
                            $scope.education.building_type = $scope.education.building_type == null ? '' : String($scope.education.building_type);
                            //$scope.education.building_type = $scope.education.building_type + '';
                        }

                        $scope.IsEducationLandToggle();
                        $scope.EducationMemberDetailCalculate();
                        $scope.GetSocietyFacilityByPrimaryActivity($scope.ncd.sector_of_operation);
                        $scope.GetTypeOfWomenCooperative($scope.ncd.sector_of_operation);
                        $scope.GetEducationCourseOfferedBySector($scope.education.type_society);
                        $scope.education.level_of_edu = $scope.education.level_of_edu + '';
                        $scope.education.duration_of_course = $scope.education.duration_of_course + '';
                        if ($scope.education.facilities) {
                            $scope.education.facilities = $scope.education.facilities.toString().split(",");
                        }
                        if ($scope.education.level_and_duration_of_course) {
                            $scope.education.level_and_duration_of_course = $scope.education.level_and_duration_of_course.toString().split(",");
                        }
                    }
                    else if ($scope.ncd.sector_of_operation == 14) {
                        $scope.Handicraft = response.data.SectorData[0];
                        if ($scope.Handicraft.paid_up_share_date != null)
                            $scope.Handicraft.paid_up_share_date = new Date($scope.convertDate($scope.Handicraft.paid_up_share_date));
                        if ($scope.Handicraft.has_building != null) {
                            $scope.Handicraft.building_type = $scope.Handicraft.building_type == null ? '' : String($scope.Handicraft.building_type);
                            //$scope.Handicraft.building_type = $scope.Handicraft.building_type + '';
                        }

                        $scope.HandicraftCoopCalculate();
                        $scope.GetSocietyFacilityByPrimaryActivity($scope.ncd.sector_of_operation);
                        $scope.GetHandicraftRawMeterialDetail();
                        $scope.GetHandicraftProductProduced();
                        if ($scope.Handicraft.type_raw) {
                            $scope.Handicraft.type_raw = $scope.Handicraft.type_raw.toString().split(",");
                        }

                        if ($scope.Handicraft.type_produce) {
                            $scope.Handicraft.type_produce = $scope.Handicraft.type_produce.toString().split(",");
                        }

                        if ($scope.Handicraft.facilities) {
                            $scope.Handicraft.facilities = $scope.Handicraft.facilities.toString().split(",");
                        }

                    }
                    else if ($scope.ncd.sector_of_operation == 13) {
                        $scope.handloom = response.data.SectorData[0];
                        if ($scope.handloom.paid_up_share_date != null)
                            $scope.handloom.paid_up_share_date = new Date($scope.convertDate($scope.handloom.paid_up_share_date));

                        if ($scope.handloom.has_building != null) {
                            $scope.handloom.building_type = $scope.handloom.building_type == null ? '' : String($scope.handloom.building_type);
                            //$scope.handloom.building_type = $scope.handloom.building_type + '';
                        }
                        $scope.HandloomMemberCalculate();
                    }
                    else if ($scope.ncd.sector_of_operation == 47) {
                        $scope.housing = response.data.SectorData[0];
                        if ($scope.housing.paid_up_share_date != null)
                            $scope.housing.paid_up_share_date = new Date($scope.convertDate($scope.housing.paid_up_share_date));

                        $scope.housing.land = response.data.CooperativeRegistrationsLands[0];
                        $scope.GetSocietyFacilityByPrimaryActivity($scope.ncd.sector_of_operation);
                        $scope.GetTypeOfWomenCooperative($scope.ncd.sector_of_operation);
                        $scope.housing.type_society = $scope.housing.type_society + '';
                        if ($scope.housing.facilities) {
                            $scope.housing.facilities = $scope.housing.facilities.toString().split(",");
                        }

                        if ($scope.housing.has_building != null) {
                            $scope.housing.building_type = $scope.housing.building_type == null ? '' : String($scope.housing.building_type);
                            //$scope.housing.building_type = $scope.housing.building_type + '';
                        }
                    }
                    else if ($scope.ncd.sector_of_operation == 82) {
                        $scope.marketing = response.data.SectorData[0];
                        $scope.marketing.land = response.data.CooperativeRegistrationsLands[0];
                        if ($scope.marketing.paid_up_share_date != null)
                            $scope.marketing.paid_up_share_date = new Date($scope.convertDate($scope.marketing.paid_up_share_date));
                        if ($scope.marketing.has_building != null) {
                            $scope.marketing.building_type = $scope.marketing.building_type == null ? '' : String($scope.marketing.building_type);
                            //$scope.marketing.building_type = $scope.marketing.building_type + '';
                        }
                        $scope.marketing.sell_the_item = $scope.marketing.sell_the_item
                            ? $scope.marketing.sell_the_item.split(',').map(Number) : [];
                        $scope.marketing.liecense_to_sell = $scope.marketing.liecense_to_sell
                            ? $scope.marketing.liecense_to_sell.split(',').map(Number) : [];

                        $scope.societyItems.forEach(item => {
                            item.sellsItem = $scope.marketing.sell_the_item.includes(item.id);
                            item.hasLicense = $scope.marketing.liecense_to_sell.includes(item.id);
                        });
                    }
                    else if ($scope.ncd.sector_of_operation == 11) {
                        $scope.Sugar = response.data.SectorData[0];
                        if ($scope.Sugar.paid_up_share_date != null) {
                            $scope.Sugar.paid_up_share_date = new Date($scope.convertDate($scope.Sugar.paid_up_share_date));
                        }
                        if ($scope.Sugar.has_building != null) {
                            $scope.Sugar.building_type = $scope.Sugar.building_type == null ? '' : String($scope.Sugar.building_type);
                            //$scope.Sugar.building_type = $scope.Sugar.building_type + '';
                        }
                        if ($scope.Sugar.crushing_period_start != null) {
                            $scope.Sugar.crushing_period_start = new Date($scope.convertDate($scope.Sugar.crushing_period_start));
                        }
                        if ($scope.Sugar.crushing_period_end) {
                            $scope.Sugar.crushing_period_end = new Date($scope.convertDate($scope.Sugar.crushing_period_end));
                        }
                        $scope.GetSugarProductProduce();
                        if ($scope.Sugar.product_produced) {
                            $scope.Sugar.product_produced = $scope.Sugar.product_produced.toString().split(",");
                        }
                    }
                    else if ($scope.ncd.sector_of_operation == 29) {
                        $scope.Miscellaneous = response.data.SectorData[0];
                        if ($scope.Miscellaneous.paid_up_share_date != null)
                            $scope.Miscellaneous.paid_up_share_date = new Date($scope.convertDate($scope.Miscellaneous.paid_up_share_date));
                        if ($scope.Miscellaneous.has_building != null) {
                            $scope.Miscellaneous.building_type = $scope.Miscellaneous.building_type == null ? '' : String($scope.Miscellaneous.building_type);
                            //$scope.Miscellaneous.building_type = $scope.Miscellaneous.building_type + '';
                        }
                    }
                    else if ($scope.ncd.sector_of_operation == 16) {
                        $scope.Multipurpose = response.data.SectorData[0];
                        if ($scope.Multipurpose.paid_up_share_date != null)
                            $scope.Multipurpose.paid_up_share_date = new Date($scope.convertDate($scope.Multipurpose.paid_up_share_date));

                        if ($scope.Multipurpose.has_building != null) {
                            $scope.Multipurpose.building_type = $scope.Multipurpose.building_type == null ? '' : String($scope.Multipurpose.building_type);
                            //$scope.Multipurpose.building_type = $scope.Multipurpose.building_type + '';
                        }
                        $scope.GetSocietyFacilityByPrimaryActivity($scope.ncd.sector_of_operation);
                        if ($scope.Multipurpose.facilities) {
                            $scope.Multipurpose.facilities = $scope.Multipurpose.facilities.toString().split(",");
                        }
                        $scope.GetTypeOfWomenCooperative($scope.ncd.sector_of_operation);
                        if ($scope.Multipurpose.sec_activity) {
                            $scope.Multipurpose.sec_activity = $scope.Multipurpose.sec_activity.toString().split(",");
                        }

                    }
                    else if ($scope.ncd.sector_of_operation == 51) {
                        $scope.Labour = response.data.SectorData[0];
                        if ($scope.Labour.paid_up_share_date != null)
                            $scope.Labour.paid_up_share_date = new Date($scope.convertDate($scope.Labour.paid_up_share_date));
                        if ($scope.Labour.has_building != null) {
                            $scope.Labour.building_type = $scope.Labour.building_type == null ? '' : String($scope.Labour.building_type);
                            //$scope.Labour.building_type = $scope.Labour.building_type + '';
                        }
                        $scope.GetSocietyFacilityByPrimaryActivity($scope.ncd.sector_of_operation);
                        $scope.GetSecondryActivityBySectorOperation($scope.ncd.sector_of_operation)
                        if ($scope.Labour.facilities) {
                            $scope.Labour.facilities = $scope.Labour.facilities.toString().split(",");
                        }

                        $scope.Labour.type_society = $scope.Labour.type_society + '';
                    }
                    else if ($scope.ncd.sector_of_operation == 54) {
                        $scope.LiveStock = response.data.SectorData[0];
                        if ($scope.LiveStock.paid_up_share_date != null)
                            $scope.LiveStock.paid_up_share_date = new Date($scope.convertDate($scope.LiveStock.paid_up_share_date));
                        if ($scope.LiveStock.has_building != null) {
                            $scope.LiveStock.building_type = $scope.LiveStock.building_type == null ? '' : String($scope.LiveStock.building_type);
                            //$scope.LiveStock.building_type = $scope.LiveStock.building_type + '';
                        }
                        $scope.GetSocietyFacilityByPrimaryActivity($scope.ncd.sector_of_operation);
                        $scope.GetSecondryActivityBySectorOperation($scope.ncd.sector_of_operation);
                        $scope.GetLivestockProductProduce();
                        if ($scope.LiveStock.facilities) {
                            $scope.LiveStock.facilities = $scope.LiveStock.facilities.toString().split(",");
                        }
                        if ($scope.LiveStock.type_society) {
                            $scope.LiveStock.type_society = $scope.LiveStock.type_society.toString().split(",");
                        }
                        if ($scope.LiveStock.type_produce) {
                            $scope.LiveStock.type_produce = $scope.LiveStock.type_produce.toString().split(",");
                        }
                    }
                    else if ($scope.ncd.sector_of_operation == 68) {
                        $scope.Transport = response.data.SectorData[0];
                        if ($scope.Transport.paid_up_share_date != null)
                            $scope.Transport.paid_up_share_date = new Date($scope.convertDate($scope.Transport.paid_up_share_date));
                        if ($scope.Transport.has_building != null) {
                            $scope.Transport.building_type = $scope.Transport.building_type == null ? '' : String($scope.Transport.building_type);
                            //$scope.Transport.building_type = $scope.Transport.building_type + '';
                        }
                        $scope.TransportCoopCalculate();
                        $scope.GetSecondryActivityBySectorOperation($scope.ncd.sector_of_operation);
                        $scope.GetTransportVehicleOwenership($scope.ncd.sector_of_operation);
                        $scope.Transport.type_society = $scope.Transport.type_society + '';
                        $scope.Transport.type_owner = $scope.Transport.type_owner + '';
                    }
                    else if ($scope.ncd.sector_of_operation == 90) {
                        $scope.Jute = response.data.SectorData[0];
                        if ($scope.Jute.paid_up_share_date != null)
                            $scope.Jute.paid_up_share_date = new Date($scope.convertDate($scope.Jute.paid_up_share_date));
                        if ($scope.Jute.has_building != null) {
                            $scope.Jute.building_type = $scope.Jute.building_type == null ? '' : String($scope.Jute.building_type);
                            //$scope.Jute.building_type = $scope.Jute.building_type + '';
                        }
                        $scope.GetJuteProductProduce();
                        $scope.GetJuteRawMeterialDetail();
                        $scope.GetSocietyFacilityByPrimaryActivity($scope.ncd.sector_of_operation);
                        if ($scope.Jute.facilities) {
                            $scope.Jute.facilities = $scope.Jute.facilities.toString().split(",");
                        }

                        if ($scope.Jute.type_raw) {
                            $scope.Jute.type_raw = $scope.Jute.type_raw.toString().split(",");
                        }

                        if ($scope.Jute.type_produce) {
                            $scope.Jute.type_produce = $scope.Jute.type_produce.toString().split(",");
                        }

                    } else if ($scope.ncd.sector_of_operation == 96) {
                        $scope.Sericulture = response.data.SectorData[0];
                        if ($scope.Sericulture.paid_up_share_date != null)
                            $scope.Sericulture.paid_up_share_date = new Date($scope.convertDate($scope.Sericulture.paid_up_share_date));
                        if ($scope.Sericulture.has_building != null) {
                            $scope.Sericulture.building_type = $scope.Sericulture.building_type == null ? '' : String($scope.Sericulture.building_type);
                            //$scope.Sericulture.building_type = $scope.Sericulture.building_type + '';
                        }
                        $scope.GetSocietyFacilityByPrimaryActivity($scope.ncd.sector_of_operation);
                        $scope.GetSecondryActivityBySectorOperation($scope.ncd.sector_of_operation);
                        if ($scope.Sericulture.facilities) {
                            $scope.Sericulture.facilities = $scope.Sericulture.facilities.toString().split(",");
                        }
                        if ($scope.Sericulture.type_society) {
                            $scope.Sericulture.type_society = $scope.Sericulture.type_society.toString().split(",");
                        }
                    }
                    else if ($scope.ncd.sector_of_operation == 98) {
                        $scope.SocialWelfare = response.data.SectorData[0];
                        if ($scope.SocialWelfare.paid_up_share_date != null)
                            $scope.SocialWelfare.paid_up_share_date = new Date($scope.convertDate($scope.SocialWelfare.paid_up_share_date));
                        if ($scope.SocialWelfare.has_building != null) {
                            $scope.SocialWelfare.building_type = $scope.SocialWelfare.building_type == null ? '' : String($scope.SocialWelfare.building_type);
                            //$scope.SocialWelfare.building_type = $scope.SocialWelfare.building_type + '';
                        }
                        $scope.GetSocietyFacilityByPrimaryActivity($scope.ncd.sector_of_operation);
                        $scope.GetSecondryActivityBySectorOperation($scope.ncd.sector_of_operation);
                        $scope.GetTypeSocialCulturalActivities();
                        if ($scope.SocialWelfare.facilities) {
                            $scope.SocialWelfare.facilities = $scope.SocialWelfare.facilities.toString().split(",");
                        }
                        $scope.SocialWelfare.type_society = $scope.SocialWelfare.type_society + '';
                        if ($scope.SocialWelfare.type_social_culture_activity) {
                            $scope.SocialWelfare.type_social_culture_activity = $scope.SocialWelfare.type_social_culture_activity.toString().split(",");
                        }

                    }
                    else if ($scope.ncd.sector_of_operation == 99) {
                        $scope.Tourism = response.data.SectorData[0];
                        if ($scope.Tourism.paid_up_share_date != null)
                            $scope.Tourism.paid_up_share_date = new Date($scope.convertDate($scope.Tourism.paid_up_share_date));
                        if ($scope.Tourism.has_building != null) {
                            $scope.Tourism.building_type = $scope.Tourism.building_type == null ? '' : String($scope.Tourism.building_type);
                            //$scope.Tourism.building_type = $scope.Tourism.building_type + '';
                        }
                        $scope.GetSocietyFacilityByPrimaryActivity($scope.ncd.sector_of_operation);
                        $scope.GetSecondryActivityBySectorOperation($scope.ncd.sector_of_operation);
                        if ($scope.Tourism.facilities) {
                            $scope.Tourism.facilities = $scope.Tourism.facilities.toString().split(",");
                        }
                        $scope.Tourism.type_society = $scope.Tourism.type_society + '';
                        $scope.TourismMemberCoopCalculate();
                    }
                    else if ($scope.ncd.sector_of_operation == 102) {
                        $scope.Tribal = response.data.SectorData[0];
                        if ($scope.Tribal.paid_up_share_date != null)
                            $scope.Tribal.paid_up_share_date = new Date($scope.convertDate($scope.Tribal.paid_up_share_date));
                        if ($scope.Tribal.has_building != null) {
                            $scope.Tribal.building_type = $scope.Tribal.building_type == null ? '' : String($scope.Tribal.building_type);
                            //$scope.Tribal.building_type = $scope.Tribal.building_type + '';
                        }
                        $scope.GetSocietyFacilityByPrimaryActivity($scope.ncd.sector_of_operation);
                        $scope.GetSecondryActivityBySectorOperation($scope.ncd.sector_of_operation);
                        if ($scope.Tribal.facilities) {
                            $scope.Tribal.facilities = $scope.Tribal.facilities.toString().split(",");
                        }
                        $scope.Tribal.type_society = $scope.Tribal.type_society + '';
                        $scope.TribalCoopSharePaidCalculate();
                    }

                }
            } else {
                $scope.secondaryActivityList = [];
            }
            $(".se-pre-con").fadeOut("slow");

        }, function (error) {
            console.error("Error loading sector Of Operations:", error);
            $scope.secondaryActivityList = [];
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.UpdateSelectedBankName = function () {
        if (!$scope.NCDCoopBanks) {
            $scope.ncd.cooperative_society_bank_id = [];
            $scope.selectAllBanks = false;
            return;
        }

        $scope.ncd.cooperative_society_bank_id = $scope.NCDCoopBanks.filter(function (x) { return !!x.selected; }).map(function (x) { return x.id; });
        let isSelectedId = parseInt($scope.ncd.cooperative_society_bank_id);
        if (isSelectedId == 38) {
            $scope.IsBanktxtEnable = true;
        }
        else {
            $scope.IsBanktxtEnable = false;
            $scope.ncd.other_bank = "";
        }

        $scope.selectAllBanks = $scope.NCDCoopBanks.length > 0 && $scope.NCDCoopBanks.every(function (x) { return !!x.selected; });
    };

    $scope.ToggleSelectAllBanks = function () {
        if (!$scope.NCDCoopBanks) return;

        angular.forEach($scope.NCDCoopBanks, function (item) {
            item.selected = !!$scope.selectAllBanks;
        });
        $scope.UpdateSelectedBankName();
    };
    $scope.getSelectedBankText = function () {
        let count = $scope.NCDCoopBanks.filter(x => x.selected).length;
        if (count == 0) return "Select Option";
        if ($scope.selectAllBanks) return "All selected (" + count + ")";
        return count + "selected";
    };

    $scope.AccountToggleDropdown = function (e) {
        e.stopPropagation();
        $scope.open = !$scope.open;
    };

    document.addEventListener('click', function () {
        $scope.$apply(function () { $scope.open = false; });
    });

    $scope.AccountIsSelected = function (id) {
        return $scope.ncd.bank_typeList.indexOf(id) !== -1;
    };

    $scope.BankAccountToggleItem = function (event, id) {
        event.stopPropagation();

        const idx = $scope.ncd.bank_typeList.indexOf(id);
        if (idx > -1)
            $scope.ncd.bank_typeList.splice(idx, 1);
        else
            $scope.ncd.bank_typeList.push(id);

        $scope.filterBankType($scope.ncd.bank_typeList);
    };

    $scope.AccountIsAllSelected = function () {
        return $scope.ncd.bank_typeList.length == $scope.sectorOperationList.length;
    };

    $scope.BankAccountToggleAll = function (event) {
        event.stopPropagation();

        if ($scope.AccountIsAllSelected()) {
            $scope.ncd.bank_typeList = [];
        } else {
            $scope.ncd.bank_typeList = $scope.sectorOperationList.map(x => x.id);
        }
        $scope.filterBankType($scope.ncd.bank_typeList);
    };
    $scope.getBankAccountSelectedText = function () {
        if ($scope.ncd.bank_typeList.length == 0)
            return "Select Option";

        if ($scope.AccountIsAllSelected())
            return "All selected (" + $scope.ncd.bank_typeList.length + ")";

        return $scope.ncd.bank_typeList.length + " selected";
    };

    $scope.toArray = function (val) {
        if (!val) return [];
        if (Array.isArray(val)) return val.map(String);
        if (typeof val == "string" && val.includes(",")) {
            return val.split(",").map(s => s.trim());
        }
        return [val.toString()];
    }


    // Validate fields with Toastr + red border
    $scope.validateFieldsWorking = function (rules) {
        console.log(rules)
        let isValid = true;

        for (let r of rules) {

            const obj = $scope[r.object];
            if (!obj) continue;

            const value = obj[r.field];

            const element = document.querySelector(
                `[ng-model="${r.object}.${r.field}"]`
            );

            let invalid = false;

            // Check empty
            if (!value) {
                invalid = true;
            }

            // Check invalidValues (for normal select like '0')
            if (r.invalidValues && value &&
                r.invalidValues.includes(value.toString().trim())) {
                invalid = true;
            }

            // Check multi-select array empty
            if (Array.isArray(value) && value.length === 0) {
                invalid = true;
            }

            if (invalid) {

                if (element) {

                    // Normal textbox / select
                    element.classList.add('invalid-field');

                    // If Select2
                    const select2Container = $(element).next('.select2-container');
                    if (select2Container.length) {
                        select2Container
                            .find('.select2-selection')
                            .addClass('invalid-field');
                    }
                }

                toastr.error(r.message);
                isValid = false;

            } else {

                if (element) {
                    element.classList.remove('invalid-field');

                    const select2Container = $(element).next('.select2-container');
                    if (select2Container.length) {
                        select2Container
                            .find('.select2-selection')
                            .removeClass('invalid-field');
                    }
                }
            }
        }

        return isValid;
    };
    $scope.validateFields = function (rules) {

        let isValid = true;

        for (let r of rules) {

            const obj = $scope[r.object];
            if (!obj) continue;

            const value = obj[r.field];

            const element = document.querySelector(
                `[ng-model="${r.object}.${r.field}"]`
            );

            let invalid = false;

            if (value === null || value === undefined || value === '') {
                invalid = true;
            }

            if (r.invalidValues &&
                r.invalidValues.includes(value)) {
                invalid = true;
            }

            if (Array.isArray(value) && value.length === 0) {
                invalid = true;
            }
            if (value && r.minLength && value.toString().length < r.minLength) {
                invalid = true;
            }

            if (value && r.maxLength && value.toString().length > r.maxLength) {
                invalid = true;
            }

            if (!invalid && r.custom && typeof r.custom === 'function') {
                if (!r.custom(value)) {
                    invalid = true;
                }
            }

            if (!invalid && r.field === 'date_registration') {
                const registrationDate = new Date(value);
                const today = new Date();
                registrationDate.setHours(0, 0, 0, 0);
                today.setHours(0, 0, 0, 0);

                if (isNaN(registrationDate.getTime())) {
                    invalid = true;
                    message = 'Invalid Date of Registration!';
                } else if (registrationDate > today) {
                    invalid = true;
                    message = 'Date of Registration cannot be in the future!';
                }
            }

            if (invalid) {
                if (element) {
                    element.classList.add('invalid-field');

                    const select2Container = $(element).next('.select2-container');
                    if (select2Container.length) {
                        select2Container
                            .find('.select2-selection')
                            .addClass('invalid-field');
                    }
                }

                toastr.error(r.message);
                isValid = false;

            } else {
                if (element) {
                    element.classList.remove('invalid-field');

                    const select2Container = $(element).next('.select2-container');
                    if (select2Container.length) {
                        select2Container
                            .find('.select2-selection')
                            .removeClass('invalid-field');
                    }
                }
            }

        }

        return isValid;
    };
    $scope.validateSectorList = function (sectorList, locationType) {
        let isValid = true;
        sectorList = sectorList.filter(x => x.is_deleted == 0);
        sectorList.forEach((sector, index) => {
            let rowNumber = index + 1;
            // District
            let districtEl = document.querySelector(`[ng-model="ncd.sectorListData[${index}].district_code"]`);
            if (!sector.district_code || sector.district_code == "0") {
                toastr.error("Please select District in sector " + rowNumber);
                if (districtEl) districtEl.classList.add('invalid-field');
                isValid = false;
            } else if (districtEl) {
                districtEl.classList.remove('invalid-field');
            }

            if (locationType == 1 || locationType == 3) { // Urban
                let lbTypeEl = document.querySelector(`[ng-model="ncd.sectorListData[${index}].local_body_type_code"]`);
                let lbEl = document.querySelector(`[ng-model="ncd.sectorListData[${index}].local_body_code"]`);
                let wardEl = document.querySelector(`[ng-model="ncd.sectorListData[${index}].locality_ward_code"]`);

                if (!sector.local_body_type_code || sector.local_body_type_code == "0") {
                    toastr.error("Please select Urban Local Body Type in sector " + rowNumber);
                    if (lbTypeEl) lbTypeEl.classList.add('invalid-field');
                    isValid = false;
                } else if (lbTypeEl) lbTypeEl.classList.remove('invalid-field');

                if (!sector.local_body_code || sector.local_body_code == "0") {
                    toastr.error("Please select Urban Local Body in sector " + rowNumber);
                    if (lbEl) lbEl.classList.add('invalid-field');
                    isValid = false;
                } else if (lbEl) lbEl.classList.remove('invalid-field');

                if (!sector.locality_ward_code || sector.locality_ward_code == "0") {
                    toastr.error("Please select Ward / Locality in sector " + rowNumber);
                    if (wardEl) wardEl.classList.add('invalid-field');
                    isValid = false;
                } else if (wardEl) wardEl.classList.remove('invalid-field');

            } else if (locationType == 2 || locationType == 3) { // Rural
                let blockEl = document.querySelector(`[ng-model="ncd.sectorListData[${index}].block_code"]`);
                let panchayatEl = document.querySelector(`[ng-model="ncd.sectorListData[${index}].panchayat_code"]`);
                let villageEl = document.querySelector(`[ng-model="ncd.sectorListData[${index}].village_code"]`);

                if (!sector.block_code || sector.block_code == "0") {
                    toastr.error("Please select Block in sector " + rowNumber);
                    if (blockEl) blockEl.classList.add('invalid-field');
                    isValid = false;
                } else if (blockEl) blockEl.classList.remove('invalid-field');

                if (!sector.panchayat_code || sector.panchayat_code == "0") {
                    toastr.error("Please select Gram Panchayat in sector " + rowNumber);
                    if (panchayatEl) panchayatEl.classList.add('invalid-field');
                    isValid = false;
                } else if (panchayatEl) panchayatEl.classList.remove('invalid-field');

                if (!sector.village_code || sector.village_code == "0") {
                    toastr.error("Please select Village in sector " + rowNumber);
                    if (villageEl) villageEl.classList.add('invalid-field');
                    isValid = false;
                } else if (villageEl) villageEl.classList.remove('invalid-field');
            }
        });

        return isValid;
    };

    $scope.fixNullStrings = function (obj) {
        if (!obj || typeof obj !== 'object') return;

        Object.keys(obj).forEach(key => {
            if (obj[key] == 'null' || obj[key] == '') {
                obj[key] = null;
            } else if (typeof obj[key] == 'object') {
                // Recursively fix nested objects or arrays
                $scope.fixNullStrings(obj[key]);
            }
        });
    }

    $scope.addUnion = function () {
        $scope.ncd.unionLevelList.push({ union_level: '' });
    };
    $scope.removeUnionLevel = function (index) {
        $scope.ncd.unionLevelList.splice(index, 1);
    };
    $scope.isAffiliatedUnionToggle = function () {
        if ($scope.ncd.is_affiliated_union_federation == 0) {
            $scope.ncd.unionLevelList = [];
        }
        $scope.getAffiliatedUnionFeederationLevel()
    };

    $scope.getAffiliatedUnionFeederationLevel = function () {
        $scope.AffiliatedFeederationList = [
            { id: 1, name: "1-11" },
            { id: 2, name: "2-12" },
            { id: 3, name: "3-13" },
            { id: 4, name: "4-14" },
            { id: 5, name: "5-15" }
        ];

        //$(".se-pre-con").fadeIn("slow");
        //$http.get(SiteUrl + "/NcdData/GetAffiliatedUnionFeederationDetails").
        //    success(function (data, status) {
        //        if (data.Result.Data != null && data.Result.Data.affiliatedFeederationDetails != null) {
        //            $scope.AffiliatedFeederationList = JSON.parse(data.Result.Data.affiliatedFeederationDetails);
        //        }
        //        $(".se-pre-con").fadeOut("slow");
        //    }).error(function (data, status) {
        //        $scope.AffiliatedFeederationList = [];
        //        $(".se-pre-con").fadeOut("slow");
        //    });
    };

    $scope.locationHeadQuarterToggle = function (type) {
        $scope.ncd.full_address = '';
        $scope.ncd.address_line = '';
        $scope.ncd.pincode = '';
        $scope.parts = [];
        if (type == 1) {
            $scope.ncd.district_code = "0";
            $scope.ncd.block_code = "0";
            $scope.ncd.gram_panchayat_code = "0";
            $scope.ncd.village_code = "0";
        } else if (type == 2) {
            $scope.ncd.urban_local_body_type_code = "0";
            $scope.ncd.urban_local_body_code = "0";
            $scope.ncd.locality_ward_code = "0";
        }
    }

    $scope.fillAddress = function () {
        if (!$scope.ncd) return;
        $scope.parts = [];
        if ($scope.ncd.address_line) {
            $scope.parts.push($scope.ncd.address_line);
        }
        if ($scope.ncd.location_of_head_quarter == 1) {
            if ($scope.wardDetailList && $scope.ncd.locality_ward_code) {
                var v = $scope.wardDetailList.filter(x => x.ward_code == $scope.ncd.locality_ward_code);
                if (v.length) $scope.parts.push(v[0].ward_name);
            }
            if ($scope.urbanBodyTypeDetailList && $scope.ncd.urban_local_body_code) {
                var v = $scope.urbanBodyTypeDetailList.filter(x => x.localbody_code == $scope.ncd.urban_local_body_code);
                if (v.length) $scope.parts.push(v[0].local_body_name);
            }
            if ($scope.UrbanCategoryLocalList && $scope.ncd.urban_local_body_type_code) {
                var v = $scope.UrbanCategoryLocalList.filter(x => x.localbody_type_code == $scope.ncd.urban_local_body_type_code);
                if (v.length) $scope.parts.push(v[0].localbody_type_name);
            }
        }
        if ($scope.ncd.location_of_head_quarter == 2) {
            if ($scope.NCDVillageDetails && $scope.ncd.village_code) {
                var v = $scope.NCDVillageDetails.filter(x => x.village_code == $scope.ncd.village_code);
                if (v.length) $scope.parts.push(v[0].village_name);
            }
            if ($scope.NCDGramPanchayatDetails && $scope.ncd.gram_panchayat_code) {
                var g = $scope.NCDGramPanchayatDetails.filter(x => x.gram_panchayat_code == $scope.ncd.gram_panchayat_code);
                if (g.length) $scope.parts.push(g[0].gram_panchayat_name);
            }
            if ($scope.NCDBlockDetails && $scope.ncd.block_code) {
                var b = $scope.NCDBlockDetails.filter(x => x.block_code == $scope.ncd.block_code);
                if (b.length) $scope.parts.push(b[0].block_name);
            }
        }
        if ($scope.NCDDistrictDetails && $scope.ncd.district_code) {
            var d = $scope.NCDDistrictDetails.filter(x => x.district_code == $scope.ncd.district_code);
            if (d.length) $scope.parts.push(d[0].district_name + ', RAJASTHAN');
        }

        if ($scope.ncd.pincode && $scope.ncd.pincode.length == 6) {
            $scope.parts.push($scope.ncd.pincode);
        }
        $scope.parts.push();
        $scope.ncd.full_address = $scope.parts.join(', ');
    };

    //Added Common product by society
    $scope.getCommonProductProduceBySociety = function (sectorOperation) {
        $scope.commonProductProduceList = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NcdData/GetCommonProductProduceBySociety?sector_of_operation=" + sectorOperation).success(function (data, status) {
            if (data.Result.Data != null && data.Result.Data.produceDetails != null) {
                $scope.commonProductProduceList = JSON.parse(data.Result.Data.produceDetails);
                $scope.sugarProductProcudList = JSON.parse(data.Result.Data.produceDetails);
            }
            $(".se-pre-con").fadeOut("slow");
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.isToggleChangeSchemeDetail = function () {
        if ($scope.ucb.is_gov_scheme_implemented == 0) {
            $scope.ucb.schemesdetail = [{
                ncd_society_id: 0,
                gov_scheme_name: '',
                gov_scheme_type: '',
                total_amount: ''
            }]
        }
    };




    $scope.OnPageLoad();

});
