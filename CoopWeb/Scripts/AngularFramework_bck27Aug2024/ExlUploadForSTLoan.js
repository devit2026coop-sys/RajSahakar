var BulkInsertModule = angular.module("ExlUploadForSTLoanMod", []);
BulkInsertModule.controller("ExlUploadForSTLoanCnt", function ($scope, $http) {
    $scope.SocityExcelDetailsData = [];
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.ExlFileShow = false;
    $scope.ExlSocietyShow = false;
    $scope.ExlSocietyShowFail = false;
    // START : Code for File Upload
    $scope.UploadBulkInsertFile = function (file) {
        $scope.BulkInsertFile = file[0];
    }
    
    //END

    // START : Save Bulk Insert
    $scope.SaveBulkInsert = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.BulkInsertFile == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Upload Excel Sheet' : '\nकृपया एक्सेल शीट अपलोड करें |';
            if (message.length > 27) {
                ShowModelAlert("Bulk Insert", message);
                return false;
            }
        }

        var formData = new FormData();
        formData.append("ExcelSheetFile", $scope.BulkInsertFile);
        $http.post(SiteUrl + "/EntryForms/SaveStLoanFormsExl", formData,
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
                ShowModelAlert("Bulk Insert", "Error While Upload Excel Sheet");
            });
    };
    // END :Save Society Profiling
              
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
           // $scope.GetExcelDetailsList();
        $(".se-pre-con").fadeOut("slow");
    };
    // END : Call default functions on page load
    $scope.GetExcelDetailsList = function () {
        $scope.SocityExcelDetailsData = [];
        $http.post(SiteUrl + "/Society/GetSocityExcelDetails").
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.ExlFileShow = true;
                }
                $scope.SocityExcelDetailsData = data;
            }).
            error(function (data, status) {
                $scope.ExlFileShow = false;
                $scope.SocityExcelDetailsData = [];
            });
        $http.post(SiteUrl + "/Society/GetUserDistrictId").
            success(function (data, status) {
                if (data.length > 0 && data != 0) {
                    $("#ExlSampleLink").attr("href", "../Content/SocietyExcel/" + data + ".xlsx");
                }
            }).
            error(function (data, status) {
                $scope.ExlFileShow = false;
                $scope.SocityExcelDetailsData = [];
            });
    };
    $scope.GetExlSociety = function (Id, StatusId) {
        $(".se-pre-con").fadeIn("slow");
        $scope.ExlSocietyDetailsData = [];
        $scope.ExlSocietyDetailsDataFail = [];
        $http.post(SiteUrl + "/Society/GetExlSocietyDetails", { "ExlId": Id, "StatusId": StatusId }).
            success(function (data, status) {
                if (StatusId == 1) {
                    if (data.length > 0) {
                        $scope.ExlSocietyShow = true;
                        $scope.ExlSocietyShowFail = false;
                    }
                    $scope.ExlSocietyDetailsDataFail = [];
                    $scope.ExlSocietyDetailsData = data;
                }
                else {
                    if (data.length > 0) {
                        $scope.ExlSocietyShow = false;
                        $scope.ExlSocietyShowFail = true;
                    }
                    $scope.ExlSocietyDetailsData = [];
                    $scope.ExlSocietyDetailsDataFail = data;
                }
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



    // START : Call Functions on Page Load
    $scope.OnPageLoad();
    //END : Call Functions on Page Load
});