var MasterMod = angular.module('SocietyKPIReportModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("SocietKPIReportECont", function ($scope, $rootScope, $http) {
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
        $scope.FinYear = "0";
        $scope.KPIFor = "0";
        $scope.Month = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        window.location = "../MIS/KPIReports";
    }
    /*------------End Paging and Filter-----------------*/
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.ShowSummary = true;
    $scope.ShowDetails = false;
    $scope.AppliedKPIFor = "";


    $scope.KPIChanged = function () {
        $scope.Offices = [];
    };

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

        if ($scope.KPIFor == 'RAO' || $scope.KPIFor == 'SA' || $scope.KPIFor == 'UpbhoktaBhandars' || $scope.KPIFor == 'PACS' || $scope.KPIFor == "PLDB" || $scope.KPIFor == "UnitOffice") {
            $scope.GetKPIOffices($scope.KPIFor, $scope.Month, $scope.FinYear);
        }
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

    $scope.Search = function () {

        if ($scope.FinYear == "0") {
            alert("Please select Financial Year.");
            return;
        }

        if (!$scope.Month) {
            alert("Please select Month.");
            return;
        }

        if (!$scope.KPIFor) {
            alert("Please select KPI Report Type.");
            return;
        }

        $scope.GetKPIReport();
    };

    $scope.GetKPIReport = function (officeId, financialYearId, month, kpiFor, DistrictId) {
        var formData = { OfficeId: officeId, FY: financialYearId, Month: month, KPIFor: kpiFor, DistrictId: DistrictId };
        $http({ method: "POST", url: '/MIS/GetKPIReport', data: JSON.stringify(formData) }).then(function (response) {
            $scope.ReportData = response.data;

            if ($scope.ReportData.length > 0) {

                var first = $scope.ReportData[0];
                console.log('first', first);

                $scope.OfficeName = first.OfficeNameEn;
                $scope.MonthName = first.MonthName;
                $scope.FY = first.FY;
                $scope.DISTRICT_ENG = first.DISTRICT_ENG;

                switch (first.KPIFor) {

                    case "SA":
                        $scope.ReportHeading = $scope.OfficeName + " क्षेत्रीय अंकेक्षण अधिकारी का " + $scope.MonthName + " (" + $scope.FY + ") प्रगति रिपोर्ट कार्ड";
                        break;

                    case "RAO":
                        $scope.ReportHeading = $scope.OfficeName + " विशेष लेखा परीक्षक का " + $scope.MonthName + " (" + $scope.FY + ") प्रगति रिपोर्ट कार्ड";
                        break;

                    case "UpbhoktaBhandars":
                        $scope.ReportHeading = $scope.DISTRICT_ENG + " " + $scope.OfficeName + " होलसेल भण्डार की " + $scope.MonthName + " (" + $scope.FY + ") तक प्रगति रिपोर्ट";
                        break;

                    case "UnitOffice":
                        $scope.ReportHeading = $scope.OfficeName + " (" + $scope.DISTRICT_ENG + ") KPI For Unit Office " + $scope.MonthName + " (" + $scope.FY + ") तक प्रगति रिपोर्ट";
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

    $scope.BackToSummary = function () {
        $scope.ShowSummary = true;
        $scope.ShowDetails = false;
    };

    $scope.GetAllSubUnitList = function () {
        $scope.ShowGrid = false;
        $http.get(SiteUrl + "/Login/UserSelfReqList").
            success(function (data, status) {
                if (data == "-1") {
                    window.location.href = "https://sso.rajasthan.gov.in";
                }
                $scope.UserReqData = data;
                if (data.length > 0) {
                    $scope.ShowGrid = true;
                }
            }).
            error(function (data1, status) {

            });
    };

    $scope.DepartmentList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DEPARTMENT&MasterCode=-1").
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.Department = data;
                if (data.length > 0) {
                    var DeoptCode = data.filter(function (x) { return x.DEPTNAMEENGLISH == 'COOPERATIVE' })[0].ID;
                    if (DeoptCode != undefined && DeoptCode != '' && DeoptCode != null) {
                        $scope.UserRegistration.DEPTCODE = DeoptCode;
                        $scope.GetAllSubOrganisationList();
                        $(".se-pre-con").fadeOut("slow");
                    }
                    else {
                        $scope.UserRegistration.DEPTCODE = '0';
                        $(".se-pre-con").fadeOut("slow");
                    }
                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while fetching department list");
                $scope.Department = [];
            });
    };

    $scope.GetAllSubOrganisationList = function () {
        $scope.Level = [];
        $http.get(SiteUrl + "/Master/OrganisationsList/?DepartmentId=" + $scope.UserRegistration.DEPTCODE).
            success(function (data, status) {
                $scope.OrganisationData = data;
                if (data.length > 0) {
                    if (data.filter(function (x) { return x.OrganisationNameEn == 'COOPERATIVE' }).length > 0) {
                        var OrgCode = data.filter(function (x) { return x.OrganisationNameEn == 'COOPERATIVE' })[0].OrganisationId;
                        if (OrgCode != undefined && OrgCode != '' && OrgCode != null) {
                            $scope.UserRegistration.OrganisationId = OrgCode;
                            $scope.LevelList();
                        }
                        else {
                            $scope.UserRegistration.OrganisationId = '0';
                        }
                    }
                }
            }).
            error(function (data1, status) {

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

    $scope.GetKPIOverallRating = function (officeId, districtid) {
        $http.get(SiteUrl + "/MIS/GetKPIOverallRating", {
            params: { KPIFor: $scope.KPIFor, OfficeId: officeId, FY: $scope.FinYear || "", Month: $scope.Month || 0, DistrictId: districtid || 0 }
        }).then(function (response) {
            $scope.OverallRating = response.data[0];
        }, function (error) {
            $scope.OverallRating = null;
        });
    };

    $scope.GetKPI_PerformanceCriteria = function (criteriaForKPI) {
        $http.get(SiteUrl + "/MIS/GetKPI_PerformanceCriteria", {
            params: { criteriaForKPI: criteriaForKPI }
        }).then(function (response) {
            $scope.PerformanceCriteria = response.data;
        }, function (error) {
            console.log("Performance Criteria Error:", error);
            $scope.PerformanceCriteria = [];
        });
    };

    $scope.GetAchievementText = function (id) {
        switch (parseInt(id)) {

            case 1:
                return "Less than 5 Hours";

            case 2:
                return "5 Hrs to Less than 6 Hrs";

            case 3:
                return "6 Hrs to Less than 7 Hrs";

            case 4:
                return "7 Hrs to Less than 8 Hrs";

            case 5:
                return "8 Hrs to Less than 9 Hrs";

            case 6:
                return "9 Hrs and Greater than 9 Hrs";

            default:
                return "";
        }
    };


    $scope.ViewReport = function (item) {
        $scope.ShowSummary = false;
        $scope.ShowDetails = true;
        $scope.GetKPIReport(item.OfficeId, $scope.FinYear, $scope.Month, $scope.KPIFor, item.districtId);
        $scope.GetKPIOverallRating(item.OfficeId, item.districtId);
        $scope.GetKPI_PerformanceCriteria($scope.KPIFor);
    };

    $scope.GetPLDBKPIOverallRating = function (pldbId) {
        $http.get(SiteUrl + "/MIS/GetKPIPLDBOverallRating", {
            params: { KPIfor: "PLDB", Month: $scope.Month || 0, FY: $scope.FinYear || "", PldbId: pldbId }
        }).then(function (response) {
            var data = response.data;
            if (data && data.length > 0) {
                $scope.PLDBOverallRating = data[0];
            }
            else {
                $scope.PLDBOverallRating = null;
            }
        }, function (error) {
            console.log("PLDB Overall Rating Error:", error);
            $scope.PLDBOverallRating = null;
        });
    };

    $scope.ViewPLDB = function (item) {
        var pldbId = item.PldbId;

        if (!pldbId || pldbId == "0") {
            alert("Please select PLDB.");
            return;
        }
        // PLDB Name
        $scope.SelectedPLDBName = item.PLDBName;

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

            $scope.GetPLDBKPIOverallRating(pldbId);

            $scope.GetKPI_PerformanceCriteria($scope.KPIFor);

            $scope.ShowSummary = false;
            $scope.ShowPLDBDetails = true;
            //$scope.ShowViewButton = false;
        });
    };

    $scope.BackFromPLDB = function () {
        $scope.ShowPLDBDetails = false;
        $scope.ShowSummary = true;
        $scope.ShowViewButton = true;
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

        $scope.UserRegistration = {};
        $scope.UserRegistration = {
            OfficeId: 0
        };

        $scope.PLDB = {
            PldbId: 0
        };

        $scope.SetPreviousMonth();
        $scope.ShowViewButton = false;
        $scope.SelectedPLDBName = "";
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
