
var MasterMod = angular.module('ncdSyncDataMod', ['angularUtils.directives.dirPagination']);

MasterMod.controller("ncdSyncDataCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.colspan = 8;
    $scope.NCDStateDetails = [];
    $scope.NCDSectorDetails = [];
    $scope.NCD_Statecode = '';
    $scope.isDisabled = true;
    $scope.search = {
        NCD_Statecode: '8',
        NCD_Levelcode: '0',
        NCD_Sectorcode: '0',
        master: {
            page: 1,
            limit: 100000
        },
        sector: {
            page: 1,
            limit: 100000
        },
        level: {
            page: 1,
            limit: 100000
        }
    };
    $scope.LevelcodeList = [
        { id: 5, name: 'Type Of Activities Khadi Gram' },
        { id: 8, name: 'Audit Categories' },
        { id: 9, name: 'Registration Authorities' },
        { id: 10, name: 'Area Of Operations' },
        { id: 11, name: 'Cooperative Society Banks' },
        { id: 12, name: 'Designations' },
        { id: 13, name: 'Cooperative Society Facilities' },
        { id: 14, name: 'Coverage Areas' },
        { id: 15, name: 'Sub Sector' },
        { id: 16, name: 'Office Building Types' },
        { id: 17, name: 'Water Body Types' },
        { id: 19, name: 'Product Produce By Society' }
    ];
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

    // NCD Master Data Sync 
    $scope.syncNcdDistrictData = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetNcdDistrictData").success(function (response) {
            if (response.status == '200' && response != null) {
                alert(response.message);
                location.reload();
            } else {
                alert(response.message);
            }
            $(".se-pre-con").fadeOut("slow");
        }).catch(function (error) {
            console.log("Failed to fetch data. Please try again.", "Error");
            $(".se-pre-con").fadeOut("slow");
        });
    };
    $scope.syncNcdCityData = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetNcdCitiesDetails").success(function (response) {
            if (response.status == '200' && response != null) {
                alert(response.message);
                location.reload();
            } else {
                alert(response.message);
            }
            $(".se-pre-con").fadeOut("slow");
        }).catch(function (error) {
            console.log("Failed to fetch data. Please try again.", "Error");
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.syncNcdBlockData = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetNcdBlockDetail").success(function (data, status) {
            if (data.status == '200' && data != null) {
                alert(data.message);
                location.reload();
            }
            else {
                alert(data.message);
            }
            $(".se-pre-con").fadeOut("slow");
        }).catch(function (error) {
            console.log("Failed to fetch data. Please try again.", "Error");
            $(".se-pre-con").fadeOut("slow");
        });
    };

    $scope.syncNcdVillageData = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetNcdVillageDetail").
            success(function (data) {
                if (data.status == '200' && data != null) {
                    alert(data.message);
                    location.reload();
                } else { alert(data.message); }
                $(".se-pre-con").fadeOut("slow");
            }).catch(function (error) {
                console.log("Failed to fetch data. Please try again.", "Error");
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.syncNcdWardData = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetNcdWardDetails").success(function (data, status) {
            if (data.status == '200' && data != null) {
                alert(data.message);
                location.reload();
            } else {
                alert(data.message);
            }
            $(".se-pre-con").fadeOut("slow");
        }).error(function (data, status) {
            $(".se-pre-con").fadeOut("slow");
        });
    };
    $scope.syncNcdSectorMaster = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetNcdSectorDetail").
            success(function (data, status) {
                if (data.status == '200' && data != null) {
                    alert(data.message);
                    location.reload();
                } else { alert(data.message); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.syncNcdDataBySector = function (search) {
        if (!$scope.search.NCD_Sectorcode || $scope.search.NCD_Sectorcode == 0) {
            alert("Please select Sector");
            return;
        }

        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/SyncCooperativeChildTableDataBySector/?sectorcode=" + ($scope.search.NCD_Sectorcode ?? 0) + "&page=" + (search.page ?? 0) + "&limit=" + (search.limit ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); location.reload(); }
                else if (data.status === '400') { alert(data.message); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.syncMasterDataByLevelCode = function (search) {
        if (!$scope.search.NCD_Levelcode || $scope.search.NCD_Levelcode == 0) {
            alert("Please select Levelcode");
            return;
        }

        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetLevelCodeMasterData/?levelcode=" + ($scope.search.NCD_Levelcode ?? 0) + "&page=" + (search.page ?? 0)).
            success(function (data) {
                if (data.status === '200') {
                    alert(data.message);
                    location.reload();
                }
                else {
                    alert(data.message);
                    location.reload();
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.syncNcdCooperativeSocietyRegistrations = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetStateWiseCooperativeRegistrationDetail").
            success(function (data, status) {
                if (data.status === '200' && data != null) {
                    alert(data.message);
                    location.reload();
                } else {
                    alert(data.message);
                }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    }


    $scope.syncNcdAreaOfOperationRural = function (search) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetAreaofOperationRuralDetail/?page=" + (search.page ?? 0) + "&limit=" + (search.limit ?? 0)).
            success(function (data) {
                if (data.status === '200' && data != null) {
                    alert(data.message);
                    location.reload();
                }
                else if (data.status === '204') {
                    alert(data.message);
                }
                else if (data.status === '400') { alert(data.message); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data) {
                $(".se-pre-con").fadeOut("slow");
            });
    };


    $scope.syncNcdAreaOfOperationUrban = function (search) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetAreaofOperationUrbanDetail/?page=" + (search.page ?? 0) + "&limit=" + (search.limit ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); location.reload(); }
                else if (data.status === '204') {
                    alert(data.message);
                }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.syncNcdSocietyImplementingSchemeDetails = function (search) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetSocietyImplementingSchemesDetails/?page=" + (search.page ?? 0) + "&limit=" + (search.limit ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); location.reload(); }
                else if (data.status === '204') {
                    alert(data.message);
                }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.syncNcdCooperativeRegistrationLandDetails = function (search) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetCooperativeRegistrationsLandsDetails/?page=" + (search.page ?? 0) + "&limit=" + (search.limit ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); location.reload(); }
                else if (data.status === '204') {
                    alert(data.message);
                }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.syncNcdSocietyAuditYearDetails = function (search) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetSocietyAuditYearsDetails/?page=" + (search.page ?? 0) + "&limit=" + (search.limit ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); location.reload(); }
                else if (data.status === '204') {
                    alert(data.message);
                }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.syncNcdMemberDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/NCDService/GetMemberDetailsByStatecode/?levelcode=" + ($scope.search.NCD_Levelcode ?? 0)).
            success(function (data, status) {
                if (data.status === '200' && data != null) { alert(data.message); }
                else if (data.status === '204') { alert(data.message); }
                else if (data.status === '400') { alert(data.ErrorMessage); }
                $(".se-pre-con").fadeOut("slow");
            }).error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.syncNcdFinancialDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/NCDService/GetNcdFinancialDetails").success(function (data) {
            if (data && data.statuscode === 200) {
                alert(data.message);
                location.reload();
            } else {
                alert(data.message);
            }
            $(".se-pre-con").fadeOut("slow");
        }).error(function () {
            $(".se-pre-con").fadeOut("slow");
            alert("Something went wrong while syncing NCD financial details.");
        });
    };


    $scope.Refresh = function () {
        $scope.search.NCD_Levelcode = '0';
        $scope.search.NCD_Sectorcode = '0';
        $scope.SearchDetails();
    }

    $scope.OnPageLoad();

});






