var CCSConsolidatedReportController = angular.module("CCSConsolidatedReportCnt", ['angularUtils.directives.dirPagination']);
CCSConsolidatedReportController.controller("CCSConsolidatedReportMod", function ($scope, $http) {

    $scope.divComplaintList = false;
    $scope.IsDisabled = true;
    $scope.IsControlDisable = false;
   
    $scope.ShowSubmitButton = true;
    //$scope.CCSReport = [];
    //$scope.CCSReport["DistrictId"] = '0';
    //$scope.CCSReport["CCSId"] = '0';
    $scope.currentPage = 1;
    $scope.pageSize = 100;

    $scope.ShowDivForPrint = function (complaintId) {

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Citizen/GetComplaintDetailsByComplaintId", { "ComplaintIdEnc": complaintId }).
            success(function (data, status) {
                if (data.length === 0) {
                    $("#showModal").modal("hide");
                    $scope.ApplicationDetail = {};
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.CCSConsolidatedReportHeading, _language === 'en-us' ? 'No data found' : 'कोई डेटा नहीं मिला');
                }
                else {
                    $scope.ApplicationDetail = data;
                    $("#showModal").modal("show");
                    $(".se-pre-con").fadeOut("slow");
                }
            }).
            error(function (data, status) {
                $scope.ComplaintList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
   

    //START : Get Complaint Details
    $scope.GetComplaintDetail = function () {

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Citizen/GetCCSConsolidatedReportData", { "fromDate": $('#txtReportFromDate').val(), "toDate": $('#txtReportToDate').val(), "districtId": $scope.CCSReport.DistrictId, "CCSId": $scope.CCSReport.CCSId }).
            success(function (data, status) {
                if (data.Result === false) {
                    $scope.divComplaintList = false;
                    $scope.ReportData = [];
                    $(".se-pre-con").fadeOut("slow");
                    ShowModelAlert($scope.translation.CCSConsolidatedReportHeading, _language === 'en-us' ? 'No data found' : 'कोई डेटा नहीं मिला');
                }
                else {
                    $scope.divComplaintList = true;
                    $scope.ReportData = data.data;


                    $scope.colsort = "";
                    $scope.reverseSort = false;

                    $scope.sortData = function (column) {
                        $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                        $scope.colsort = column;
                    };


                    $scope.getSortClass = function (column) {
                        if ($scope.colsort === column) {
                            return $scope.reverseSort ? 'arrow-down' : 'arrow-up';
                        }
                        return '';
                    };

                    $(".se-pre-con").fadeOut("slow");
                }
            }).
            error(function (data, status) {
                $scope.ComplaintList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    // END :  Get Complaint Details

    //START : CCSComplaintDetails
    $scope.ExportData = function (id) {
        $('#hdfcomplaintid').val(id);
        $('#btnGeneratePDF').click();
    };
    // END : CCSComplaintDetails

    //START : Download Zip
    $scope.DownloadZip = function (id) {
        $('#hdfcomplaintid').val(id);
        $('#btnZipDownload').click();
    };
    // END : Download Zip

    // START : Page load Function
    $scope.OnPageLoad = function () {

        $scope.LoggedInSerDistrictId = $('#hdfLoggedInUserDistrictId').val();

        if (parseInt($scope.LoggedInSerDistrictId,10)>0) {
            $scope.IsControlDisable = true;
            $scope.CCSReport = {};
            $scope.CCSReport["DistrictId"] = ($scope.LoggedInSerDistrictId).toString();
            $scope.GetCCSList();
        }
        else {
            $scope.IsControlDisable = false;
        }

        $scope.DistrictList();
        $scope._language = _language;
        if (_language !== 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }

        $('.SetCalender').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy"
        });
    };
    // END : Page load Function


    // START : Get District List
    $scope.DistrictList = function () {
        $(".se-pre-con").fadeIn("slow");
        
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=0").
            success(function (data, status) {
                $scope.Districts = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.Districts = [];
                $(".se-pre-con").fadeOut("slow");
            });
       
    };
    // END : Get District List

    // START : Get CCS List
    $scope.GetCCSList = function () {
        $(".se-pre-con").fadeIn("slow");
        var model = [{ "Key": "SocietyRegOfficeDetails.DISTRICT_ID", "Value": $scope.CCSReport.DistrictId }];
        $http.post(SiteUrl + "/Citizen/GetCreditCooperativeSocieties", { paramlist: model }).
            success(function (data, status) {
                $scope.CCSList = data;
                $scope.CCSReport.CCSId = '0';
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching list");
                $(".se-pre-con").fadeOut("slow");
            });
        //$(".se-pre-con").fadeIn("slow");
    };
    // END : Get CCS List

    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.CCSReport.DistrictId = '0';
        $scope.CCSList = [];
        $scope.CCSReport.CCSId = '0';
        $('#txtReportFromDate').val('');
        $('#txtReportToDate').val('');
        $scope.divComplaintList = false;
        $scope.ReportData = [];
    };
    //END : Clear Form

    // START : On Page Load
    $scope.OnPageLoad();
    // END : On Page Load
});