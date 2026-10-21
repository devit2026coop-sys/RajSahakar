var MasterMod = angular.module('SocietyKPIUnitOfficeReportModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("SocietKPIUnitOfficeReportECont", function ($scope, $rootScope, $http) {
    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 300,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }
    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else if ($(this).attr("type") == "radio" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                //  _currentPageID = 1;
                callback();
            }
            ++indx;
        });
    }
    $rootScope.SetDataPaging = function () {
        if ($rootScope.DataPagingDetails.TotalRecords > 0) {
            $rootScope.DataPagingDetails.TotalPages = parseInt(($rootScope.DataPagingDetails.TotalRecords / $rootScope.DataPagingDetails.PageSize)) + (($rootScope.DataPagingDetails.TotalRecords % $rootScope.DataPagingDetails.PageSize == 0) ? 0 : 1);
            $rootScope.DataPagingDetails.StartRecord = ($rootScope.DataPagingDetails.TotalRecords == 0) ? 0 : ((($rootScope.DataPagingDetails.PageNumber - 1) * $rootScope.DataPagingDetails.PageSize) + 1);
            $rootScope.DataPagingDetails.EndRecord = ($rootScope.DataPagingDetails.TotalPages == $rootScope.DataPagingDetails.PageNumber || $rootScope.DataPagingDetails.TotalRecords == 0) ? $rootScope.DataPagingDetails.TotalRecords : ((($rootScope.DataPagingDetails.PageNumber - 1) * $rootScope.DataPagingDetails.PageSize) + $rootScope.DataPagingDetails.PageSize);


            $rootScope.DataPagingDetails.StartPageNumber = 1;
            $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.TotalPages;
            if ($rootScope.DataPagingDetails.TotalPages > 5 && $rootScope.DataPagingDetails.PageNumber > 0) {
                $rootScope.DataPagingDetails.StartPageNumber = $rootScope.DataPagingDetails.PageNumber == 1 ? 1 : $rootScope.DataPagingDetails.PageNumber - 1;
                $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.StartPageNumber + 4;
                if ($rootScope.DataPagingDetails.EndPageNumber > $rootScope.DataPagingDetails.TotalPages) {
                    $rootScope.DataPagingDetails.EndPageNumber = $rootScope.DataPagingDetails.TotalPages;
                    $rootScope.DataPagingDetails.StartPageNumber = $rootScope.DataPagingDetails.StartPageNumber > 4 ? $rootScope.DataPagingDetails.EndPageNumber - 4 : $rootScope.DataPagingDetails.StartPageNumber;
                }
            }
        }
        $scope.DataPaging.TotalRecords = $rootScope.DataPagingDetails.TotalRecords
    }
    $rootScope.PagingRange = function (startPage, endPage, totalPages, step) {
        step = step || 1;
        var input = [];

        for (var tStart = startPage; tStart <= endPage && tStart <= totalPages; tStart++) {
            input.push(tStart);
        }
        return input;
    };
    $scope.searchRequest = function (callback) {
        //$scope.DataPaging.searchParameters = [];
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else if ($(this).attr("type") == "radio" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                $scope.DataPaging.currentPageID = 1;
                callback();
            }
            ++indx;
        });
    }

    $scope.SearchText = "";
    $scope.Filter = [];
    $scope.ClearSearch = function () {
        $scope.Month = String($scope.PreviousMonthId);
        $scope.PLDB = {
            PldbId: "0"
        };
        $scope.SelectedKPIAreaId = "0";
        $scope.PLDBReportData = [];
        $scope.ShowPLDBReport = false;
    };

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.ShowSummary = true;
    $scope.ShowDetails = false;

    $scope.SearchPLDBReportold = function () {
        debugger;
        if (!$scope.Month || $scope.Month == "0") {
            toastr.warning("Please select Month.");
            return;
        }

        if (!$scope.FinYear || $scope.FinYear == "0") {
            toastr.warning("Please select Financial Year.");
            return;
        }

        if (!$scope.SelectedKPIAreaId ||
            $scope.SelectedKPIAreaId == "0") {
            toastr.warning("Please select KPI Area.");
            return;
        }

        var selectedArea = ($scope.KPIAreaList || []).find(function (item) {
            return String(item.KPIAreaId) === String($scope.SelectedKPIAreaId);
        });

        $scope.SelectedKPIAreaName = selectedArea ? selectedArea.KPIArea : "";

        $scope.ShowPLDBReport = false;
        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/KPI/GetUnitOfficeKPIReport", {
            params: { month: $scope.Month, fy: $scope.FinYear, kpiAreaId: $scope.SelectedKPIAreaId }
        }).then(function (response) {

            var rawData = response.data || [];

            console.log('rawData', rawData);

            if (typeof rawData === "string") {
                rawData = JSON.parse(rawData);
            }

            $scope.KPIIndicators = [];
            $scope.PLDBReportData = [];


            var officeId = item.OfficeId;
            var officeName = item.OfficeNameEn;

            var exists = districts.some(function (d) {
                return String(d.DistrictId) === String(districtId) &&
                    String(d.OfficeId) === String(officeId);
            });

            if (districtId != null && !exists) {
                districts.push({
                    DivisionId: divisionId,
                    Zone: zoneName,
                    DistrictId: districtId,
                    District: districtName,
                    OfficeId: officeId,
                    officeName: officeName
                });
            }


            var districts = [];

            rawData.forEach(function (item) {

                var districtId = item.DISTRICT_ID;
                var districtName = item.District;
                var divisionId = item.DIVISION_ID;
                var zoneName = item.Zone;

                var exists = districts.some(function (d) {
                    return String(d.DistrictId) === String(districtId);
                });

                if (districtId != null && !exists) {
                    districts.push({ DivisionId: divisionId, Zone: zoneName, DistrictId: districtId, District: districtName });
                }
            });

            districts.sort(function (a, b) {
                var zoneCompare = (a.Zone || "").localeCompare(b.Zone || "");

                if (zoneCompare !== 0) {
                    return zoneCompare;
                }

                return (a.District || "").localeCompare(b.District || "");
            });

            var uniqueZones = [];

            districts.forEach(function (district) {
                if (uniqueZones.indexOf(district.Zone) === -1) {
                    uniqueZones.push(district.Zone);
                }
            });

            angular.forEach(districts, function (district) {


                var records = rawData.filter(function (item) {
                    return String(item.DISTRICT_ID) === String(district.DistrictId) &&
                        String(item.DIVISION_ID) === String(district.DivisionId) &&
                        String(item.OfficeId) === String(district.OfficeId);
                });


                var districtRecord = records.length > 0 ? records[0] : {};

                var row = {
                    DivisionId: district.DivisionId,
                    Zone: district.Zone,
                    DistrictId: district.DistrictId,
                    District: district.District,
                    OfficeId: district.OfficeId,
                    OfficeNameEn: district.OfficeNameEn,
                    TotalMarksObtained: Number(districtRecord.TotalMarksObtained) || 0,
                    TotalWeightedMarks: Number(districtRecord.TotalWeightedMarks) || 0,
                    PercentageObtained: Number(districtRecord.PercentageObtained) || 0,
                    Performance: districtRecord.Performance || '',
                    Category: districtRecord.Category || '',
                    Rank: Number(districtRecord.Rank) || 0,
                    KPIData: [],
                    SubTotalWeightedMarks: 0,
                    SubTotalMarksObtained: 0,
                    ShowZone: false,
                    ZoneRowSpan: 1
                };

                angular.forEach($scope.KPIIndicators, function (indicator) {

                    var item = records.find(function (x) {
                        return String(x.KPIIndicatorsId) === String(indicator.KPIIndicatorsId);
                    });

                    var kpi = {
                        KPIIndicatorsId: indicator.KPIIndicatorsId,
                        KPIIndicators: indicator.KPIIndicators,
                        Target: item ? item.Target : 0,
                        Achievement: item ? item.Achievement : 0,
                        Progress: item ? item.Progress : 0,
                        WeightedMarks: item ? item.WeightedMarks : 0,
                        MarksObtained: item ? item.MarksObtained : 0
                    };

                    row.KPIData.push(kpi);
                    row.SubTotalWeightedMarks += Number(kpi.WeightedMarks) || 0;
                    row.SubTotalMarksObtained += Number(kpi.MarksObtained) || 0;
                }
                );

                $scope.PLDBReportData.push(row);
            });

            var i = 0;

            while (i < $scope.PLDBReportData.length) {

                var start = i;
                var zone = $scope.PLDBReportData[i].Zone;

                while (i < $scope.PLDBReportData.length && $scope.PLDBReportData[i].Zone === zone) {
                    i++;
                }

                $scope.PLDBReportData[start].ShowZone = true;
                $scope.PLDBReportData[start].ZoneRowSpan = i - start;
            }

            $scope.ShowPLDBReport = true;

        }).catch(function (error) {

            $scope.PLDBReportData = [];
            $scope.KPIIndicators = [];
            $scope.ShowPLDBReport = false;

            toastr.error("Error while getting Unit Office KPI report.");

        }).finally(function () {
            $(".se-pre-con").fadeOut("slow");
        });
    };


    $scope.SearchPLDBReport = function () {
        debugger;

        if (!$scope.Month || $scope.Month == "0") {
            toastr.warning("Please select Month.");
            return;
        }

        if (!$scope.FinYear || $scope.FinYear == "0") {
            toastr.warning("Please select Financial Year.");
            return;
        }

        if (!$scope.SelectedKPIAreaId ||
            $scope.SelectedKPIAreaId == "0") {
            toastr.warning("Please select KPI Area.");
            return;
        }

        var selectedArea = ($scope.KPIAreaList || []).find(function (item) {
            return String(item.KPIAreaId) === String($scope.SelectedKPIAreaId);
        });

        $scope.SelectedKPIAreaName = selectedArea
            ? selectedArea.KPIArea
            : "";

        $scope.ShowPLDBReport = false;
        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/KPI/GetUnitOfficeKPIReport", {
            params: {
                month: $scope.Month,
                fy: $scope.FinYear,
                kpiAreaId: $scope.SelectedKPIAreaId
            }
        }).then(function (response) {

            var rawData = response.data || [];

            console.log("rawData", rawData);

            if (typeof rawData === "string") {
                rawData = JSON.parse(rawData);
            }

            $scope.KPIIndicators = [];
            $scope.PLDBReportData = [];

            // 1. Get unique KPI indicators
            angular.forEach(rawData, function (item) {
                var exists = $scope.KPIIndicators.some(function (kpi) {
                    return String(kpi.KPIIndicatorsId) ===
                        String(item.KPIIndicatorsId);
                });

                if (!exists) {
                    $scope.KPIIndicators.push({
                        KPIIndicatorsId: item.KPIIndicatorsId,
                        KPIIndicators: item.KPIIndicators
                    });
                }
            });

            // 2. Create unique District + Unit Office combinations
            var districts = [];

            rawData.forEach(function (item) {

                var districtId = item.DISTRICT_ID;
                var districtName = item.District;
                var divisionId = item.DIVISION_ID;
                var zoneName = item.Zone;
                var officeId = item.OfficeId;
                var officeName = item.OfficeNameEn;

                var exists = districts.some(function (d) {
                    return String(d.DistrictId) === String(districtId) && String(d.DivisionId) === String(divisionId) && String(d.OfficeId) === String(officeId);
                });

                if (districtId != null && !exists) {
                    districts.push({
                        DivisionId: divisionId,
                        Zone: zoneName,
                        DistrictId: districtId,
                        District: districtName,
                        OfficeId: officeId,
                        OfficeNameEn: officeName
                    });
                }
            });

            // 3. Sort by Zone, District, and Unit Office
            districts.sort(function (a, b) {

                var zoneCompare = (a.Zone || "").localeCompare(b.Zone || "");

                if (zoneCompare !== 0) {
                    return zoneCompare;
                }

                var districtCompare = (a.District || "").localeCompare(b.District || "");

                if (districtCompare !== 0) {
                    return districtCompare;
                }

                return (a.OfficeNameEn || "").localeCompare(
                    b.OfficeNameEn || ""
                );
            });

            // 4. Build report rows for each Unit Office
            angular.forEach(districts, function (district) {

                var records = rawData.filter(function (item) {
                    return String(item.DISTRICT_ID) === String(district.DistrictId) && String(item.DIVISION_ID) === String(district.DivisionId) && String(item.OfficeId) === String(district.OfficeId);
                });

                var districtRecord = records.length > 0 ? records[0] : {};

                console.log('districtRecord', districtRecord);

                var row = {
                    DivisionId: district.DivisionId,
                    Zone: district.Zone,
                    DistrictId: district.DistrictId,
                    District: district.District,
                    OfficeId: district.OfficeId,
                    OfficeNameEn: districtRecord.OfficeNameEn,
                    OfficeEntryStatus: districtRecord.OfficeEntryStatus,
                    TotalMarksObtained: Number(districtRecord.TotalMarksObtained) || 0,
                    TotalWeightedMarks: Number(districtRecord.TotalWeightedMarks) || 0,
                    PercentageObtained: Number(districtRecord.PercentageObtained) || 0,
                    Performance: districtRecord.Performance || "",
                    Category: districtRecord.Category || "",
                    Rank: Number(districtRecord.Rank) || 0,
                    KPIData: [],
                    SubTotalWeightedMarks: 0,
                    SubTotalMarksObtained: 0,
                    ShowZone: false,
                    ZoneRowSpan: 1
                };

                // 5. Add KPI data for this specific Unit Office
                angular.forEach($scope.KPIIndicators, function (indicator) {

                    var item = records.find(function (x) {
                        return String(x.KPIIndicatorsId) === String(indicator.KPIIndicatorsId);
                    });

                    var kpi = {
                        KPIIndicatorsId: indicator.KPIIndicatorsId,
                        KPIIndicators: indicator.KPIIndicators,
                        Target: item ? item.Target : 0,
                        Achievement: item ? item.Achievement : 0,
                        Progress: item ? item.Progress : 0,
                        WeightedMarks: item ? item.WeightedMarks : 0,
                        MarksObtained: item ? item.MarksObtained : 0
                    };

                    row.KPIData.push(kpi);
                    row.SubTotalWeightedMarks += Number(kpi.WeightedMarks) || 0;
                    row.SubTotalMarksObtained += Number(kpi.MarksObtained) || 0;
                });

                $scope.PLDBReportData.push(row);
            });

            // 6. Merge Zone cells using rowspan
            var i = 0;

            while (i < $scope.PLDBReportData.length) {

                var start = i;
                var zone = $scope.PLDBReportData[i].Zone;

                while (i < $scope.PLDBReportData.length && $scope.PLDBReportData[i].Zone === zone) {
                    i++;
                }

                $scope.PLDBReportData[start].ShowZone = true;
                $scope.PLDBReportData[start].ZoneRowSpan = i - start;
            }

            $scope.ShowPLDBReport = true;

        }).catch(function (error) {

            console.error("Error loading Unit Office KPI report:", error);

            $scope.PLDBReportData = [];
            $scope.KPIIndicators = [];
            $scope.ShowPLDBReport = false;

            toastr.error("Error while getting Unit Office KPI report.");

        }).finally(function () {
            $(".se-pre-con").fadeOut("slow");
        });
    };



    // Get a KPI's values for a district row
    $scope.getKPIData = function (row, kpiId) {

        var item = (row && row.KPIData || []).find(function (x) {
            return String(x.KPIIndicatorsId) === String(kpiId);
        });

        return item || {
            Target: 0,
            Achievement: 0,
            Progress: 0,
            WeightedMarks: 0,
            MarksObtained: 0
        };
    };

    $scope.getIndicatorValue = function (row, indicatorId, field) {
        var item = row.Indicators.find(function (x) {
            return x.KPIIndicatorsId == indicatorId;
        });

        if (!item) {
            return "";
        }

        return item[field];
    };

    $scope.SetPreviousMonth = function () {

        var currentMonth = new Date().getMonth(); // 0 = January

        if (currentMonth == 0) {
            $scope.PreviousMonthId = 12;
            $scope.PreviousMonth = "December";
        }
        else {
            $scope.PreviousMonthId = currentMonth;
            $scope.PreviousMonth = new Date(new Date().getFullYear(), currentMonth - 1, 1).toLocaleString('default', { month: 'long' });
        }

        $scope.Month = String($scope.PreviousMonthId);
    };

    $scope.KPIAreaList = [];

    $scope.SelectedKPIAreaId = 0;

    $scope.GetPLDBKPIArea = function () {

        $http.get(SiteUrl + "/KPI/GetUnitOfficeKPIArea")
            .then(function (response) {
                $scope.KPIAreaList = response.data;
            }, function (error) {
                console.log("KPI Area Error:", error);
                $scope.KPIAreaList = [];
            });
    };

    $scope.GetKPIOffices = function (kpiFor, Month, FinYear) {
        $http.get('/MIS/GetKPIOffices', {
            params: { KPIFor: kpiFor, Month: Month, FinancialYearId: FinYear }
        }).then(function (response) {
            $scope.Offices = response.data;
        }, function (error) {
            console.log(error);
        });

    };

    $scope.LoadMaster = function () {

        return $http.get(SiteUrl + "/pldb/LoadMaster")
            .then(function (response) {
                $scope.PLDBList = JSON.parse(response.data.DataList).PLDB || [];

                var districtid = $('#hdnDistrictId').val();
                var LoginPldbId = $('#hdnPldbId').val();

                if (LoginPldbId && LoginPldbId !== '0') {

                    $scope.$evalAsync(function () {
                        $scope.PLDB = $scope.PLDB || {};
                        $scope.PLDB.PldbId = LoginPldbId.toString();
                        $scope.CheckPLDBAdminLock();
                    });
                }
            }, function () {
                $scope.PLDBList = [];
            });
    };

    $scope.LoadMaster();

    $scope.getPLDBKPINumber = function (kpiAreaId, index) {

        var number = 0;
        var previousAreaId = null;

        for (var i = 0; i <= index; i++) {

            var currentAreaId = $scope.PLDBReportData[i].KPIAreaId;

            if (String(currentAreaId) !== String(previousAreaId)) {
                number++;
                previousAreaId = currentAreaId;
            }
        }

        return number;
    };

    $scope.getPLDBKPIAreaRowSpan = function (kpiAreaId, index) {

        var count = 0;

        for (var i = index; i < $scope.PLDBReportData.length; i++) {

            if (String($scope.PLDBReportData[i].KPIAreaId) === String(kpiAreaId)) {
                count++;
            }
            else {
                break;
            }
        }

        return count;
    };

    $scope.GetFinancialYear = function () {
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1")
            .success(function (data) {
                if (data != null) {
                    $scope.FinancialYearList = data
                        .map(function (item) {
                            return {
                                FinYearId: item.FIN_YEAR_MASTR_ID,
                                FinYearName: item.FIN_YEAR_START + '-' + item.FIN_YEAR_END
                            };
                        })
                        .filter(function (item) {
                            return item.FinYearId == '1086';
                        });

                    if ($scope.FinancialYearList.length > 0) {
                        $scope.FinYear = '1086';
                    }
                }
                else {
                    $scope.FinancialYearList = [];
                }

            }).error(function () {
                $scope.FinancialYearList = [];
            });
    };

    $scope.OfficeList = function () {
        $scope.OfficeListData = [];
        $http.get(SiteUrl + "/Master/OfficesList/?WhereClause=and o.isactive =1 and   o.OrganizationLevelsMappingID=" + $scope.UserRegistration.OrganizationLevelsMappingID).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.OfficeListData = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while fetching designation list");
                $scope.OfficeListData = [];
                $scope.showLoader = false;
            });
    };

    $scope.getKPIAreaRowSpan = function (kpiAreaId, index) {

        if (!$scope.ReportData || !$scope.ReportData.length) {
            return 1;
        }

        var count = 1;

        for (var i = index + 1; i < $scope.ReportData.length; i++) {

            if ($scope.ReportData[i].KPIAreaId == kpiAreaId) {
                count++;
            } else {
                break;
            }
        }

        return count;
    };

    $scope.getKPINumber = function (kpiAreaId, index) {

        var number = 0;
        var previousKPIAreaId = null;

        for (var i = 0; i <= index; i++) {

            if ($scope.ReportData[i].KPIAreaId != previousKPIAreaId) {
                number++;
                previousKPIAreaId = $scope.ReportData[i].KPIAreaId;
            }
        }

        return number;
    };

    $scope.OnPageLoad = function () {
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });

            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }

        $scope.GetFinancialYear();

        $scope.PLDB = {
            PldbId: 0
        };

        $scope.GetPLDBKPIArea();
        $scope.SetPreviousMonth();
        $scope.ShowPLDBReport = false;
        $scope.ShowViewButton = false;
        $scope.SelectedPLDBName = "";
        //$scope.ShowPLDBDetails = false;
        $scope.ShowPLDBDetails = false;
        $scope.SelectedPLDBName = "";
        $scope.PLDBReportData = [];
        $scope.GopalCreditCardModel = {};
        $scope.Divisions = [{}];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
            success(function (data, status) {
                $scope.Divisions = data;
            }).
            error(function (data, status) {
                $scope.Divisions = [];
            });
    };

    $scope.OnPageLoad();

});
function tableToExcel(tableid, filename = '') {

    var table = document.getElementById(tableid);
    var html = table.outerHTML.replace(/ /g, '%20');

    var file = filename ? filename + '.xls' : 'excel_data.xls';

    var downloadlink = document.createElement("a");
    document.body.appendChild(downloadlink);

    downloadlink.href = 'data:application/vnd.ms-excel,' + html;
    downloadlink.download = file;

    downloadlink.click();
    document.body.removeChild(downloadlink);
}

MasterMod.filter('inrCurrency', function () {
    return function (input) {
        if (!input)
            return input;
        var num = parseFloat(input);
        if (isNaN(num)) return input;
        return num.toLocaleString('en-IN', { style: 'currency', currency: 'INR' });
    };
});
