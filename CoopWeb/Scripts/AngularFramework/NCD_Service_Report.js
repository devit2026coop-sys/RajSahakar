
var MasterMod = angular.module('ncdPRMod', ['angularUtils.directives.dirPagination']);

MasterMod.controller("ncdPRCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.search = {};
    $scope.colspan = 8;
    $scope.NCDStateDetails = [];
    $scope.NCDSectorDetails = [];
    $scope.NCD_Statecode = '';
    $scope.isDisabled = true;
    $scope.search = {
        NCD_Statecode: '',
        NCD_Levelcode: '',
        NCD_Sectorcode: ''
    };

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
        $scope.SearchDetails();
    };
    $scope.SearchDetails = function () {
        $scope.NCD_Statecode = $scope.search.NCD_Statecode;
        $scope.GetNCDStateDetail();
        $scope.GetSectorDetail();
    };
    $scope.GetNCDStateDetail = function () {
        $scope.NCDStateDetails = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetNCDStateDetails").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.NCDStateDetail != null) {
                    $scope.NCDStateDetails = JSON.parse(data.Result.Data.NCDStateDetail);
                    $scope.colsort = "";
                    $scope.reverseSort = false;
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });

    };

    $scope.GetSectorDetail = function () {
        $scope.NCDSectorDetails = [];
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetNCDSectorDetails").
            success(function (data, status) {
                if (data.Result.Data != null && data.Result.Data.NCDSectorDetail != null) {
                    $scope.NCDSectorDetails = JSON.parse(data.Result.Data.NCDSectorDetail);
                    $scope.colsort = "";
                    $scope.reverseSort = false;
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.AddNCDState = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetNCDStateData/?statecode=" + $scope.search.NCD_Statecode).
            success(function (data, status) {
                console.log(data);
                //if (data.status == '200' && data != null) {
                //    alert(data.message);
                //}
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.AddNCDDistrict = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetDistrictByStatecode/?statecode=" + $scope.search.NCD_Statecode).
            success(function (data, status) {
                if (data.status == '200' && data != null) {
                    alert(data.message);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.AddNCDBlockDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetBlockDetailByStatecode/?statecode=" + $scope.search.NCD_Statecode).
            success(function (data, status) {
                if (data.status == '200' && data != null) {
                    alert(data.message);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.AddNCDVillageDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetVillageDetailByStatecode/?statecode=" + $scope.search.NCD_Statecode).
            success(function (data, status) {
                console.log(data);
                if (data.status == '200' && data != null) {
                    alert(data.message);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.AddNCDCitiesDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCitiesDetailByStatecode/?statecode=" + $scope.search.NCD_Statecode).
            success(function (data, status) {
                if (data.status == '200' && data != null) {
                    alert(data.message);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.AddNCDWardDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetWardDetailByStatecode/?statecode=" + $scope.search.NCD_Statecode).
            success(function (data, status) {
                if (data.status == '200' && data != null) {
                    alert(data.message);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.AddNCDSectorDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetSectorDetailByStatecode/?statecode=" + $scope.search.NCD_Statecode).
            success(function (data, status) {
                console.log(data);
                if (data.status == '200' && data != null) {
                    alert(data.message);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.AddNCDSubSectorDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetSubSectorDetailByStatecode/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&levelcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.AddNCDCooperativeLandDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeRegistrationsLandByStatecode/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&levelcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.AddNCDKhadiGramDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetKhadiGramByStatecode/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&levelcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.AddNCDMemberDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetMemberDetailByStatecode/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&levelcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.AddNCDAuditYearDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetSocietyAuditYearByStatecode/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&levelcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.AddNCDSociatyCategoryDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetSocietyCategoryByStatecode/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&levelcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.AddNCDAuthoritiesDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetAuthoritiesDetailByStatecode/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&levelcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.AddNCDAreaOperationDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetAreaOperationDetailByStatecode/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&levelcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.AddNCDSocietyBankDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetSocietyBankDetailByStatecode/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&levelcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.AddNCDDesignationDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetDesignationDetailByStatecode/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&levelcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.AddNCDSocietyFacilitiesDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetSocietyFacilitiesByStatecode/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&levelcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.AddNCDCoverageAreaDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCoverageAreasDetailsByStatecode/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&levelcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.AddNCDOfficeBuildingDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetOfficeBuildingDetailsByStatecode/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&levelcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.AddNCDWaterBodyDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetWaterBodyDetailsByStatecode/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&levelcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    //Added NCD Masters
    $scope.AddCooprativeRegistrationDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetStateWiseCooperativeRegistrationDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0)).
            success(function (data, status) {
                console.log("data", data)
                if (data.status === 'Success' && data != null) { alert(data.message); }
                else if (data.status === 'Failed') { alert(data.message); }
                else if (data.status === 'NotFound') { alert(data.ErrorMessage); }
                else { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.SaveNCDPACSDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeRegistrationPACSDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.SaveNCDUCBDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeRegistrationUCBDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                console.log(data)
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === 'Failed') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDFisheryDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeRegistrationFisheryDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDSugarDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeRegistrationSugarDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.SaveNCDHandloomDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeRegistrationHandloomDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDHandicraftDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeRegistrationHandicraftDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDWomenWelfareDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeRegistrationWomenWelfareDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDCreditThriftDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeRegistrationCreditThriftDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDAgroProcessingDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeRegistrationAgroProcessingDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDCmiscellaneousDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeRegistrationCmiscellaneousDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDHousingDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeHousingDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDLabourDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeLabourDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDLiveStockDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeLiveStockDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDTransportDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeTransportDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDAgricultureDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeAgricultureDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDBeeFarmingDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeBeeFarmingDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDConsumerDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeConsumerDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDMarketingDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeMarketingDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDEducationDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeEducationDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDJuteDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeJuteDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDSericultureDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeSericultureDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDSocialDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeSocialDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDTourismDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeTourismDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDTribalDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeTribalDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDKhadiGramDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeKhadiGramDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.SaveNCDDairyDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeDairyDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
                $scope.search.NCD_Statecode = '';
                $scope.search.NCD_Levelcode = '';
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDMultippurposeDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeMultipurposeDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDMiscellaneousDetail = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeMiscellaneousDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDLAMPSDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeRegistrationLAMPSDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.SaveNCDFSSDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeRegistrationFSSDetail/?statecode=" + ($scope.search.NCD_Statecode ?? 0) + "&sectorcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.BackToPage = function () {
        $scope.search = {
            NCD_Statecode: 9
        };
        $scope.SearchDetails();
    }

    $scope.OnPageLoad();

});






