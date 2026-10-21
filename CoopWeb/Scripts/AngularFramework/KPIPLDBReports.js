var MasterMod = angular.module('SocietyKPIPLDBReportModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("SocietKPIPLDBReportECont", function ($scope, $rootScope, $http) {
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

    $scope.KPITypeChanged = function () {
        $scope.Offices = [];
        $scope.OfficeId = null;

        //$scope.PLDB = [];
        $scope.PldbId = null;


        if (!$scope.Month) {
            alert("Please select Month.");
            return;
        }

        if (!$scope.KPIFor) {
            alert("Please select KPI Report Type.");
            return;
        }


        if ($scope.KPIFor == "PLDB") {

            if (!$scope.PLDB || !$scope.PLDB.PldbId || $scope.PLDB.PldbId == "0") {
                alert("Please select PLDB.");
                return;
            }

            var pldbId = $scope.PLDB.PldbId;

            var selectedPLDB = $scope.PLDBList.find(function (item) {
                return String(item.PldbId) === String(pldbId);
            });

            if (selectedPLDB) {
                $scope.SelectedPLDBName = selectedPLDB.PLDBName;
            }

            $scope.ShowViewButton = true;

            return;
        }
    };

    $scope.getKPIData = function (pldb, indicatorId) {

        if (!pldb || !pldb.KPIData) {
            return {};
        }

        var data = pldb.KPIData.find(function (x) {
            return x.KPIIndicatorsId == indicatorId;
        });

        return data || {};
    };

    //old
    //$scope.SearchPLDBReport = function () {

    //    if (!$scope.Month || $scope.Month == "0") {
    //        toastr.warning("Please select Month.");
    //        return;
    //    }

    //    if (!$scope.SelectedKPIAreaId || $scope.SelectedKPIAreaId == "0") {
    //        toastr.warning("Please select KPI Area.");
    //        return;
    //    }

    //    var selectedArea = $scope.KPIAreaList.find(function (item) {
    //        return String(item.KPIAreaId) === String($scope.SelectedKPIAreaId);
    //    });

    //    if (selectedArea) {
    //        $scope.SelectedKPIAreaName = selectedArea.KPIArea;
    //    } else {
    //        $scope.SelectedKPIAreaName = "";
    //    }

    //    $scope.ShowPLDBReport = true;

    //    $(".se-pre-con").fadeIn("slow");

    //    $http.get(SiteUrl + "/KPI/GetPLDBKPIAreaWiseResult", {
    //        params: { kpiFor: "PLDB", month: $scope.Month, fy: $scope.FinYear, kpiAreaId: $scope.SelectedKPIAreaId }
    //    }).then(function (response) {

    //        $(".se-pre-con").fadeOut("slow");           

    //        var rawData = response.data || [];            
    //        $scope.PLDBReportData = [];
    //        $scope.KPIIndicators = [];

    //        angular.forEach(rawData, function (item) {

    //            var exists = $scope.KPIIndicators.some(function (indicator) {
    //                return String(indicator.KPIIndicatorsId) === String(item.KPIIndicatorsId);
    //            });

    //            if (!exists) {
    //                $scope.KPIIndicators.push({
    //                    KPIIndicatorsId: item.KPIIndicatorsId,
    //                    KPIIndicators: item.KPIIndicators
    //                });
    //            }
    //        });



    //        angular.forEach($scope.PLDBList, function (pldbMaster) {

    //            var firstPLDBRecord = rawData.find(function (x) {
    //                return String(x.PldbId) === String(pldbMaster.PldbId);
    //            });

    //            var pldb = {
    //                PLDBId: pldbMaster.PldbId,
    //                PLDBName: pldbMaster.PLDBName,
    //                KPIArea: rawData.length > 0 ? rawData[0].KPIArea  : "",
    //                KPIAreaId: $scope.SelectedKPIAreaId,
    //                SubTotalWeightedMarks: firstPLDBRecord ? firstPLDBRecord.SubTotalWeightedMarks : 0,
    //                SubTotalMarksObtained: firstPLDBRecord ? firstPLDBRecord.SubTotalMarksObtained : 0,
    //                SubTotalRank: firstPLDBRecord ? firstPLDBRecord.SubTotalRank : "",
    //                KPIData: []
    //            };               

    //            angular.forEach($scope.KPIIndicators, function (indicator) {
    //                var item = rawData.find(function (x) {
    //                    return String(x.PldbId) === String(pldbMaster.PldbId) && String(x.KPIIndicatorsId) === String(indicator.KPIIndicatorsId);
    //                });                 

    //                if (item) {
    //                    pldb.KPIData.push({
    //                        KPIIndicatorsId: item.KPIIndicatorsId,
    //                        KPIIndicators: item.KPIIndicators,
    //                        Target: item.Target,
    //                        Achievement: item.Achievement,
    //                        WeightedMarks: item.WeightedMarks,
    //                        MarksObtained: item.MarksObtained,
    //                        inputtype: item.inputtype
    //                    });
    //                }
    //                else {

    //                    pldb.KPIData.push({
    //                        KPIIndicatorsId: indicator.KPIIndicatorsId,
    //                        KPIIndicators: indicator.KPIIndicators,
    //                        Target: "0",
    //                        Achievement: "0",
    //                        WeightedMarks: "0",
    //                        MarksObtained: "0",
    //                        inputtype: "-"
    //                    });
    //                }
    //            });             

    //            $scope.PLDBReportData.push(pldb);

    //        });

    //    }, function (error) {           
    //        $(".se-pre-con").fadeOut("slow");
    //        console.log( "PLDB Report Error:", error );
    //        $scope.PLDBReportData = [];
    //        $scope.KPIIndicators = [];
    //        toastr.error( "Error while getting PLDB report." );
    //    });

    //};

    $scope.SearchPLDBReport = function () {

        if (!$scope.Month || $scope.Month == "0") {
            toastr.warning("Please select Month.");
            return;
        }

        if (!$scope.SelectedKPIAreaId || $scope.SelectedKPIAreaId == "0") {
            toastr.warning("Please select KPI Area.");
            return;
        }

        var selectedArea = $scope.KPIAreaList.find(function (item) {
            return String(item.KPIAreaId) === String($scope.SelectedKPIAreaId);
        });

        $scope.SelectedKPIAreaName = selectedArea ? selectedArea.KPIArea : "";

        $scope.ShowPLDBReport = true;

        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/KPI/GetPLDBKPIAreaWiseResult", {
            params: { kpiFor: "PLDB", month: $scope.Month, fy: $scope.FinYear, kpiAreaId: $scope.SelectedKPIAreaId }
        }).then(function (response) {

            $(".se-pre-con").fadeOut("slow");

            var rawData = response.data || [];

            $scope.PLDBReportData = [];
            $scope.KPIIndicators = [];

            angular.forEach(rawData, function (item) {

                var exists = $scope.KPIIndicators.some(function (indicator) {
                    return String(indicator.KPIIndicatorsId) === String(item.KPIIndicatorsId);
                });

                if (!exists) {
                    $scope.KPIIndicators.push({
                        KPIIndicatorsId: item.KPIIndicatorsId,
                        KPIIndicators: item.KPIIndicators
                    });
                }
            });

            angular.forEach($scope.PLDBList, function (pldbMaster) {
                var pldbRecords = rawData.filter(function (item) {
                    return String(item.PldbId) === String(pldbMaster.PldbId);
                });

                var firstPLDBRecord = pldbRecords.length > 0 ? pldbRecords[0] : null;

                var pldb = {
                    PLDBId: pldbMaster.PldbId,
                    PLDBName: pldbMaster.PLDBName,
                    KPIArea: firstPLDBRecord ? firstPLDBRecord.KPIArea : $scope.SelectedKPIAreaName,
                    KPIAreaId: $scope.SelectedKPIAreaId,
                    SubTotalWeightedMarks: firstPLDBRecord ? firstPLDBRecord.SubTotalWeightedMarks : 0,
                    SubTotalMarksObtained: firstPLDBRecord ? firstPLDBRecord.SubTotalMarksObtained : 0,
                    SubTotalRank: firstPLDBRecord ? firstPLDBRecord.SubTotalRank : 0,
                    KPIData: []
                };

                angular.forEach($scope.KPIIndicators, function (indicator) {

                    var item = pldbRecords.find(function (x) {
                        return String(x.KPIIndicatorsId) === String(indicator.KPIIndicatorsId);
                    });

                    if (item) {
                        pldb.KPIData.push({
                            KPIIndicatorsId: item.KPIIndicatorsId,
                            KPIIndicators: item.KPIIndicators,
                            Target: item.Target,
                            Achievement: item.Achievement,
                            WeightedMarks: item.WeightedMarks,
                            MarksObtained: item.MarksObtained,
                            inputtype: item.inputtype
                        });
                    }
                    else {

                        pldb.KPIData.push({
                            KPIIndicatorsId: indicator.KPIIndicatorsId,
                            KPIIndicators: indicator.KPIIndicators,
                            Target: "0",
                            Achievement: "0",
                            WeightedMarks: "0",
                            MarksObtained: "0",
                            inputtype: "-"
                        });
                    }
                });

                $scope.PLDBReportData.push(pldb);

            });

            $scope.PLDBReportData.sort(function (a, b) {
                var rankA = parseInt(a.SubTotalRank) || 999999;
                var rankB = parseInt(b.SubTotalRank) || 999999;
                return rankA - rankB;
            });

        }, function (error) {
            $(".se-pre-con").fadeOut("slow");
            console.log("PLDB Report Error:", error);
            $scope.PLDBReportData = [];
            $scope.KPIIndicators = [];
            toastr.error("Error while getting PLDB report.");
        });

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

        $http.get(SiteUrl + "/KPI/GetPLDBKPIArea")
            .then(function (response) {
                console.log("KPI Area:", response.data);
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
            console.log('$scope.Offices', $scope.Offices);
        }, function (error) {
            console.log(error);
        });

    };

    $scope.SearchPLDB = function () {

        if (!$scope.PldbId || !$scope.PldbId || $scope.PldbId == "0") {
            alert("Please select PLDB.");
            return;
        }
        var selectedPLDB = $scope.PLDBList.find(function (item) {
            return String(item.PldbId) === String(pldbId);
        });

        if (selectedPLDB) {
            $scope.SelectedPLDBName = selectedPLDB.PLDBName;
        }

        $scope.ShowViewButton = true;
    };

    $scope.GetKPIReport = function (officeId, financialYearId, month, kpiFor, DistrictId) {

        var formData = { OfficeId: officeId, FY: financialYearId, Month: month, KPIFor: kpiFor, DistrictId: DistrictId };
        $http({ method: "POST", url: '/MIS/GetKPIReport', data: JSON.stringify(formData) }).then(function (response) {
            $scope.ReportData = response.data;

            if ($scope.ReportData.length > 0) {

                var first = $scope.ReportData[0];

                $scope.OfficeName = first.OfficeNameEn;
                $scope.MonthName = first.MonthName;
                $scope.FY = first.FY;

                switch (first.KPIFor) {

                    case "SA":
                        $scope.ReportHeading = $scope.OfficeName + " क्षेत्रीय अंकेक्षण अधिकारी का " + $scope.MonthName + " (" + $scope.FY + ") प्रगति रिपोर्ट कार्ड";
                        break;

                    case "RAO":
                        $scope.ReportHeading = $scope.OfficeName + " विशेष लेखा परीक्षक का " + $scope.MonthName + " (" + $scope.FY + ") प्रगति रिपोर्ट कार्ड";
                        break;

                    case "UpbhoktaBhandars":
                        $scope.ReportHeading = $scope.OfficeName + " होलसेल भण्डार की " + $scope.MonthName + " (" + $scope.FY + ") तक प्रगति रिपोर्ट";
                        break;

                    default:
                        $scope.ReportHeading = "";
                        break;
                }
            }

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

    $scope.ViewPLDB = function () {
        var pldbId = $scope.PLDB.PldbId;

        if (!pldbId || pldbId == "0") {
            alert("Please select PLDB.");
            return;
        }

        $http.get(SiteUrl + "/KPI/GetPLDBKPIResult", {
            params: { pldbId: pldbId, month: $scope.Month, kpiFor: "PLDB" }
        }).then(function (response) {
            $scope.PLDBReportData = response.data;
            $scope.PLDBTotalWeightedMarks = 0;
            $scope.PLDBTotalMarksObtained = 0;

            angular.forEach($scope.PLDBReportData, function (item) {
                $scope.PLDBTotalWeightedMarks += parseFloat(item.WeightedMarks || 0);
                $scope.PLDBTotalMarksObtained += parseFloat(item.MarksObtained || 0);
            });

            $scope.PLDBTotalWeightedMarks = $scope.PLDBTotalWeightedMarks.toFixed(2);
            $scope.PLDBTotalMarksObtained = $scope.PLDBTotalMarksObtained.toFixed(2);
            $scope.ShowSummary = false;
            $scope.ShowPLDBDetails = true;
            $scope.ShowViewButton = false;

        }, function (error) {
            console.log("PLDB Details Error:", error);
            $scope.PLDBReportData = [];
        });
    };

    $scope.BackFromPLDB = function () {

        $scope.ShowPLDBDetails = false;
        $scope.ShowSummary = true;

        // Show PLDB summary row again
        $scope.ShowViewButton = true;

        // Keep selected PLDB
        // $scope.PLDBReportData = [];

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
