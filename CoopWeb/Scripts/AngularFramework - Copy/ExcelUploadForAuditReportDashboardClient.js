var AuditReportForDashboardModule = angular.module("AuditReportForDashboardMod", []);
AuditReportForDashboardModule.controller("AuditReportForDashboardCnt", function ($scope, $http) {
    $scope.SocityExcelDetailsData = [];
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.ExlFileShow = false;
    $scope.ExlSocietyShow = false;
    $scope.ExlSocietyShowFail = false;
    $scope.ReportType = [];
    $scope.ReportType.TypeId = 0;
    $scope.divAuditReportRegionWise = false;
    $scope.divAuditReportHeadWise = false;
    $scope.ExlFileShowHeadWise = false;
    $scope.ExlSocietyShowHeadWise = false;
    $scope.ExlSocietyShowFailHeadWise = false;
    // START : Code for File Upload
    $scope.ClearReport = function () {
        $scope.AuditReport.Year = '0';
        $scope.AuditReport.Month = '0';
        $scope.AuditReportHeasWise.Year = '0';
        $scope.AuditReportHeasWise.Month = '0';
        $scope.ExlSocietyShow = false;
        $scope.ExlSocietyShowFail = false;
        $scope.ExlSocietyDetailsData = [];
        $scope.ExlSocietyDetailsDataFail = [];
        $scope.ExlDashboardShowHeadFailed = [];
        $scope.ExlDashboardShowHead = [];
        $scope.ExlDashboardShowHeadWise = false;

    }


    $scope.SetUploadType = function () {

        if ($scope.ReportType.TypeId == '1') {
            $scope.divAuditReportRegionWise = true;
            $scope.divAuditReportHeadWise = false;
            $scope.GetSocityExcelDetailsList();
            //$scope.GetAuditReportRegionWise();
        }
        else if ($scope.ReportType.TypeId == '2') {
            $scope.divAuditReportRegionWise = false;
            $scope.divAuditReportHeadWise = true;
            $scope.GetSocityExcelDetailsHeadWiseList();
            //$scope.GetAuditReportHeadWiseExcelDetailsList();
        }
        // $scope.divAuditReportHeadWise = true;
    }
    //END


    // START : Call default functions on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope._language = _language; // Set LAnguiage
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

        }

        $scope.FinancialYearList = [];
        $http.post(SiteUrl + "/CommonServices/Mobile_AuditStatusYearList").
            success(function (data, status) {
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];

            });


        $(".se-pre-con").fadeOut("slow");
    };
    // END : Call default functions on page load

    $scope.GetAuditMonthList = function () {
        //alert($scope.AuditReport.Year);
        $scope.MonthList = [];
        //$http.post(SiteUrl + "/CommonServices/Mobile_AuditStatusMonthList", { "FYearID": $scope.AuditReport.Year }).
        $http.post(SiteUrl + "/CommonServices/Mobile_GetYearMonthListForAuditReport/?Month=1").
            success(function (data, status) {
                $scope.MonthList = data;
            }).
            error(function (data, status) {
                $scope.MonthList = [];
            });
    };

    $scope.TypeChange = function () {
        $scope.ExlSocietyShow = false;
        $scope.divAuditReportHeadWise = false;
        $scope.ExlSocietyDetailsData = [];
        $scope.ExlDashboardShowHead = [];
        $scope.AuditReport.Year = '0';
        $scope.AuditReport.Month = '0';
    }


    $scope.GetAuditReport = function () {

        $scope.ExlSocietyShow = false;
        $scope.divAuditReportHeadWise = false;
        $scope.ExlSocietyDetailsData = [];
        $scope.ExlDashboardShowHead = [];

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.AuditReport.Year == '0') {
            message += $scope._language == 'en-us' ? '<br/>Please Select Year' : '<br/>कृपया वर्ष चुनें';
        }

        if ($scope.AuditReport.Month == '0') {
            message += $scope._language == 'en-us' ? '<br/>Please Select Month' : '<br/>कृपया माह का चयन करें';
        }

        if (message.length > 27) {
            ShowModelAlert("Audit Report Show", message);
            return false;
        }

        $(".se-pre-con").fadeIn("slow");
        if ($scope.ReportType.TypeId == 1) {
            $http.post(SiteUrl + "/MIS/GetAuditReportRegionWise", { "Year": $scope.AuditReport.Year, "Month": $scope.AuditReport.Month, "Type": $scope.ReportType.TypeId }).
                success(function (data, status) {

                    if (data.length > 0) {
                        $scope.ExlSocietyShow = true;
                        $scope.ExlSocietyDetailsData = data;
                    }
                    else {
                        var msg = _language == 'en-us' ? 'Data not found.' : 'डेटा नहीं मिला।' ;
                        alert(msg);
                    }

                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                });
        }
        else if ($scope.ReportType.TypeId == 2) {
            $http.post(SiteUrl + "/MIS/GetAuditReportRegionWise?Year=" + $scope.AuditReport.Year + "&Month=" + $scope.AuditReport.Month + "&Type=" + $scope.ReportType.TypeId).
                success(function (data, status) {
                    if (data.length > 0) {
                        $scope.divAuditReportHeadWise = true;
                        $scope.ExlDashboardShowHead = data;
                    }
                    else {
                        var msg = _language == 'en-us' ? 'Data not found.' : 'डेटा नहीं मिला।';
                        alert(msg);
                    }

                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                });
        }
       

    };
    $scope.GetAuditReportHeadWise = function () {


        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.AuditReportHeasWise.Year == '0') {
            message += $scope._language == 'en-us' ? '<br/>Please Select Year' : '<br/>कृपया वर्ष चुनें';
        }

        if ($scope.AuditReportHeasWise.Month == '0') {
            message += $scope._language == 'en-us' ? '<br/>Please Select Month' : '<br/>कृपया माह का चयन करें';
        }



        if (message.length > 27) {
            ShowModelAlert("Audit Report Show", message);
            return false;
        }

        $(".se-pre-con").fadeIn("slow");
        $scope.ExlSocietyDetailsData = [];
        $scope.ExlSocietyDetailsDataFail = [];
        $http.post(SiteUrl + "/MIS/GetAuditReportRegionWise?Year=" + $scope.AuditReportHeasWise.Year + "&Month=" + $scope.AuditReportHeasWise.Month + "&Type=" + $scope.ReportType.TypeId).
            success(function (data, status) {

                if (data.length > 0) {
                    $scope.ExlDashboardShowHeadWise = true;
                    $scope.ExlDashboardShowHeadWiseFailed = false;
                    $scope.ExlDashboardShowHeadWise = true;
                }
                $scope.ExlDashboardShowHeadFailed = [];
                $scope.ExlDashboardShowHead = data;

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.ExlSocietyShow = false;
                $scope.ExlSocietyShowFail = false;
                $scope.ExlSocietyDetailsData = [];
                $scope.ExlSocietyDetailsDataFail = [];
                $(".se-pre-con").fadeOut("slow");
            });

    };

    $scope.UploadBulkInsertFile = function (file) {
        $scope.BulkInsertFile = file[0];
    }

    $scope.UploadAuditReportHeadWiseExcel = function (file) {
        $scope.BulkInsertFile2 = file[0];
    }

    $scope.SaveAuditReportHeadWiseForDashboard = function () {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.BulkInsertFile2 == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Upload Excel Sheet' : '\nकृपया एक्सेल शीट अपलोड करें |';
            if (message.length > 27) {
                ShowModelAlert("Court Case Bulk Insert", message);
                return false;
            }
        }

        var formData = new FormData();
        formData.append("ExcelSheetFile", $scope.BulkInsertFile2);
        formData.append("YearId", $scope.AuditReportHeasWise.Year);
        formData.append("MonthId", $scope.AuditReportHeasWise.Month);

        $http.post(SiteUrl + "/MIS/InsertAuditReportDashboardHeadWiseByExcel", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                alert(data);
                location.reload();
            }).
            error(function (data, status) {
                ShowModelAlert("Insert Data", "Error While Upload Excel Sheet");
            });

    }



    // START : Save Bulk Insert
    $scope.SaveAuditReportForDashboard = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.BulkInsertFile == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Upload Excel Sheet' : '\nकृपया एक्सेल शीट अपलोड करें |';
            if (message.length > 27) {
                ShowModelAlert("Court Case Bulk Insert", message);
                return false;
            }
        }

        var formData = new FormData();
        formData.append("ExcelSheetFile", $scope.BulkInsertFile);
        formData.append("YearId", '2019');
        formData.append("MonthId", '10');



        $http.post(SiteUrl + "/MIS/InsertAuditReportDashboardByExcel", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                alert(data);
                location.reload();
            }).
            error(function (data, status) {
                ShowModelAlert("Insert Data", "Error While Upload Excel Sheet");
            });







    };
    // END : Save Bulk Insert
    $scope.GetSocityExcelDetailsList = function () {
        $scope.SocityExcelDetailsData = [];
        $http.post(SiteUrl + "/MIS/GetAuditReportForDashboardDetails").
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.ExlFileShow = true;
                }
                $scope.ExcelDetailsData = data;
            }).
            error(function (data, status) {
                $scope.ExlFileShow = false;
                $scope.ExcelDetailsData = [];
            });
    };


    $scope.GetSocityExcelDetailsHeadWiseList = function () {
        $scope.SocityExcelDetailsData = [];
        $http.post(SiteUrl + "/MIS/GetAuditReportHeadWiseForDashboardDetails").
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.ExlFileShowHeadWise = true;
                }
                $scope.ExcelDetailsDataHeadWise = data;
            }).
            error(function (data, status) {
                $scope.ExlFileShowHeadWise = false;
                $scope.ExcelDetailsDataHeadWise = [];
            });
    };

    $scope.GetExcelDataDetails = function (id, filter) {
        $scope.ExlSocietyDetailsData = [];
        $scope.ExlSocietyDetailsDataFail = [];
        $scope.ExlSocietyShow = false;
        $scope.ExlSocietyShowFail = false;
        $http.post(SiteUrl + "/MIS/GetExlAuditReportDashboardDetails", { ExlId: id, StatusId: filter }).
            success(function (data, status) {
                if (data.length > 0) {
                    if (filter == 1) {
                        $scope.ExlSocietyShow = true;
                        $scope.ExlSocietyDetailsData = data;
                    }
                    else if (filter == 0) {
                        $scope.ExlSocietyShowFail = true;
                        $scope.ExlSocietyDetailsDataFail = data;
                    }
                }
                else {
                    var Msg = _language == 'en-us' ? 'No data found.' : 'कोई डेटा नहीं मिला।';
                    alert(Msg);
                }
            }).
            error(function (data, status) {
                console.log('Error while fetching data from function GetExcelDataDetails');
            });
    };


    $scope.GetExcelDataDetailsHeadWise = function (id, filter) {
        $scope.ExlDashboardShowHeadWise = false;
        $scope.ExlDashboardShowHeadWiseFailed = false;
        $scope.ExlHeadWiseData = [];
        $scope.ExlDashboardShowHeadFailed = [];
        $http.post(SiteUrl + "/MIS/GetExlAuditReportHeadWiseDashboardDetails", { ExlId: id, StatusId: filter }).
            success(function (data, status) {
                console.log(data);
                if (data.length > 0) {
                    if (filter == 1) {
                        $scope.ExlDashboardShowHeadWise = true;
                        $scope.ExlHeadWiseData = data;
                    }
                    else if (filter == 0) {
                        $scope.ExlDashboardShowHeadWiseFailed = false;
                        $scope.ExlDashboardShowHeadFailed = data;
                    }
                }
                else {
                    var Msg = _language == 'en-us' ? 'No data found.' : 'कोई डेटा नहीं मिला।';
                    alert(Msg);
                }
            }).
            error(function (data, status) {
                console.log('Error while fetching data from function GetExcelDataDetailsHeadWise');
            });
    };

    // START : Call Functions on Page Load
    $scope.OnPageLoad();
    $scope.GetAuditMonthList();

    //END : Call Functions on Page Load
});