var BulkInsertModule = angular.module("AuditDistrictWiseReportMod", ['angularUtils.directives.dirPagination']);
BulkInsertModule.controller("AuditDistrictWiseReportCnt", function ($scope, $http) {
    $scope.SocityExcelDetailsData = [];
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DistrictId = 0;
    $scope.ActIdVal = '0';
    $scope.ExlFileShow = false;
    $scope.ExlSocietyShow = false;
    $scope.ExlSocietyShowFail = false;
    $scope.Hearing = [];
    $scope.Hearing["NextHearingDate"] = '';
    // START : Code for File Upload
    $scope.UploadBulkInsertFile = function (file) {
        $scope.BulkInsertFile = file[0];
    };

    $scope.UploadBulkInsertFileCourt = function (file) {
        $scope.BulkInsertFileCourt = file[0];
    };
    //END


    // START : Save Bulk Insert

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
  
            $scope.GetSocityExcelDetailsList();
        
        //$(".se-pre-con").fadeOut("slow");
    };
    // END : Call default functions on page load
    $scope.FinancialYear = "";
    $scope.ActId = 0;
    $scope.GetSocityExcelDetailsList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.SocityDetailsData = [];
        $http.post(SiteUrl + "/MIS/GetSocietyAuditDistrictWiseReport", { FinancialYear: $scope.FinancialYear, ActId: $scope.ActId }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.ExlFileShow = true;
                $scope.SocityDetailsData = JSON.parse(data.DataList);
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.ExlFileShow = false;
                $scope.SocityExcelDetailsData = [];
            });
        //$http.post(SiteUrl + "/Society/GetUserDistrictId").
        //    success(function (data, status) {
        //        if (data.length > 0 && data != 0) {
        //            $scope.DistrictId = data;
        //            $("#ExlSampleLink").attr("href", "../Content/SocietyExcel/" + data + ".xlsx");
        //        }
        //    }).
        //    error(function (data, status) {
        //        $scope.ExlFileShow = false;
        //        $scope.SocityExcelDetailsData = [];
        //    });
    };

    $scope.FinancialYearList = [];
    $scope.BindFY = function () {

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                //alert(JSON.stringify(data));
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    }



    $scope.BindFY();


    $scope.Actlist = [];
    $scope.GetSocietyActList = function () {
        $http.get(SiteUrl + "/Society/GetSocietyActList").
            success(function (data, status) {
                data.forEach(function (item, index) {
                    if ($("#hdnActId").val().split(',').indexOf(item.ActId) > -1) {
                        $scope.Actlist.push(item);
                    }
                });

                if ($("#hdnActId").val().split(',').length == 1) {
                    $scope.ActId = $("#hdnActId").val();
                    $scope.GetSocietyTypeList();
                    $("#ddrActId").attr("disabled", "disabled");
                }

                //$scope.Actlist = data;
            }).
            error(function (data, status) {
                $scope.Actlist = [];
            });
    };
    $scope.GetSocietyActList();

    $scope.GetExlSociety = function (Id, StatusId) {
        $(".se-pre-con").fadeIn("slow");
        $scope.ExlSocietyDetailsData = [];
        $scope.ExlSocietyDetailsDataFail = [];
        $http.post(SiteUrl + "/mis/GetSocietyAuditDistrictWiseReport", { "ExlId": Id, "StatusId": StatusId }).
            success(function (data, status) {
                if (StatusId == 1) {
                    if (data.length > 0) {
                        $scope.ExlSocietyShow = true;
                        $scope.ExlFileShow = false;
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

  
    $scope.ActChange = function () {
        if ($scope.ActIdVal == '1') {
            // alert($scope.ActIdVal);
            if ($scope.DistrictId != 0 || $scope.DistrictId != '0') {
                $("#ExlSampleLink").attr("href", "../Content/SocietyExcel/" + $scope.DistrictId + ".xlsx");
            } else {
                $("#ExlSampleLink").attr("href", "../Content/SocietyExcel/SocietySample.xlsx");
            }
            $("#ExlSampleLink").html("DownLoad Act 1958 Sample");
        }
        else if ($scope.ActIdVal == '3') {
            // alert($scope.ActIdVal);
            if ($scope.DistrictId != 0 || $scope.DistrictId != '0') {
                $("#ExlSampleLink").attr("href", "../Content/SocietyExcel/" + $scope.DistrictId + ".xlsx");
            } else {
                $("#ExlSampleLink").attr("href", "../Content/SocietyExcel/SocietySample.xlsx");
            }
            $("#ExlSampleLink").html("DownLoad Act 2001 Sample");
        }
        else if ($scope.ActIdVal == '111') {
            // alert($scope.ActIdVal);
            //if ($scope.DistrictId != 0 || $scope.DistrictId != '0') {
            //    $("#ExlSampleLink").attr("href", "../Content/SocietyExcel/" + $scope.DistrictId + ".xlsx");
            //} else {
            $("#ExlSampleLink").attr("href", "../Content/SocietyExcel/CCSocietySample.xlsx");
            //}
            $("#ExlSampleLink").html("DownLoad Credit Cooperative Society Sample");
        }
        else {
            // alert($scope.ActIdVal);
            $("#ExlSampleLink").attr("href", "../Content/SocietyExcel/SocietySampleSports.xlsx");
            $("#ExlSampleLink").html("DownLoad Sports Act 2005 Sample");
        }
    };
   
   

    //START : GET Next Hearing Date
    $scope.GetNextHearingDate = function (item) {
        $scope.NextHearingDetails = item;
        console.log($scope.NextHearingDetails);
    }
    //END : GET Next Hearing Date



     $scope.EditShowRow = function (index) {

        $scope.OldEditData.SocietyName = $scope.ExlSocietyDetailsData[index].SocietyName;
        $scope.OldEditData.RegistrationNo = $scope.ExlSocietyDetailsData[index].RegistrationNo;
        $scope.OldEditData.RegistrationDate = $scope.ExlSocietyDetailsData[index].RegistrationDate;
        $scope.OldEditData.Name = $scope.ExlSocietyDetailsData[index].Name;
        $scope.OldEditData.ContactNo = $scope.ExlSocietyDetailsData[index].ContactNo;
        $scope.OldEditData.Aadh = $scope.ExlSocietyDetailsData[index].Aadh;



        $scope.ExlSocietyDetailsData.forEach(function (item, indx) {
            if (index == indx) {
                item.show = false;
            } else {
                item.show = true;
            }
        });
    }

    $scope.CancelEditData = function (index) {
        $scope.ExlSocietyDetailsData[index].SocietyName = $scope.OldEditData.SocietyName;
        $scope.ExlSocietyDetailsData[index].RegistrationNo = $scope.OldEditData.RegistrationNo;
        $scope.ExlSocietyDetailsData[index].RegistrationDate = $scope.OldEditData.RegistrationDate;
        $scope.ExlSocietyDetailsData[index].Name = $scope.OldEditData.Name;
        $scope.ExlSocietyDetailsData[index].ContactNo = $scope.OldEditData.ContactNo;
        $scope.ExlSocietyDetailsData[index].Aadh = $scope.OldEditData.Aadh;
        $scope.OldEditData = { SocietyName: "", RegistrationNo: "", RegistrationDate: "", Name: "", ContactNo: "", Aadh: "" };
        $scope.ExlSocietyDetailsData.forEach(function (item, indx) {
            item.show = true;
        });
    }

    //END : Update Next Hearing Date




    $scope.ExportData = function () {
        $(".blockUI").fadeIn("slow");

        var uri = 'data:application/vnd.ms-excel;base64,'
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
        var fileName = "Offline Societies Upload By Excel";
        var excelData = "<table  border='1'><tr><th colspan='5'  style='text-align:center;font-size:20px'>" + fileName + "</th></tr> " + $("#divExportPrint1").html() + "</table>"
        excelData = excelData.replace(/₹/g, "");
        var ctx = { worksheet: "List" || 'Worksheet', table: excelData }
        //window.location.href = uri + $scope.base64($scope.format(template, ctx))
        var bytes = $scope.base64ToArrayBuffer($scope.base64($scope.format(template, ctx)));
        //var byte = $scope.toUTF8Array(data);
        var blob = new Blob([bytes], { type: "application/vnd.ms-excel" });
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);

        link.download = fileName;
        link.click();
        $(".blockUI").fadeOut("slow");
    }

    $scope.base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
    $scope.format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }

    $scope.base64ToArrayBuffer = function (base64) {
        var binaryString = window.atob(base64);
        var binaryLen = binaryString.length;
        var bytes = new Uint8Array(binaryLen);
        for (var i = 0; i < binaryLen; i++) {
            var ascii = binaryString.charCodeAt(i);
            bytes[i] = ascii;
        }
        return bytes;
    }


    // START : Call Functions on Page Load
    $scope.OnPageLoad();
    //END : Call Functions on Page Load
});