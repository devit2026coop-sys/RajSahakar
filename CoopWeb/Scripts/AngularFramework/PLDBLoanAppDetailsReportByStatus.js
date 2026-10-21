var BankDetailsController = angular.module("PLDBLoanAppDetailsReportByStatusMod", ['angularUtils.directives.dirPagination']);
BankDetailsController.controller("PLDBLoanAppDetailsReportByStatusCnt", function ($scope, $rootScope, $http) {

    /*------------Paging and Filter-----------------*/


    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];
    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        if ($scope.Filter.FilterType == 'SubmissionDate') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            $scope.AddCalender();
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }

    /*------------End Paging and Filter-----------------*/

    $scope.BankMaster = [];
    $scope.ShowAddNew = false;



    $scope.PLDBdrop = false;
    $scope.PLDB = [];
    $scope.PLDBList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=-1").
            success(function (data, status) {
                $scope.PLDB = data;
                $scope.AppLoanReport.PLDB_ID = $('#PLDBId').val();
                if ($scope.AppLoanReport.PLDB_ID == '' || $scope.AppLoanReport.PLDB_ID == "0") {
                    $scope.PLDBdrop = false;
                }
                else {
                    $scope.PLDBdrop = true;
                    $scope.PLDBBranchList();
                }
                $scope.AppLoanReport.Branch_ID = '';
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDB = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };



    $scope.PLDBBranches = [];
    $scope.PLDBBranchList = function () {
        if ($scope.AppLoanReport.PLDB_ID != undefined || $scope.AppLoanReport.PLDB_ID != '') {
            return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + $scope.AppLoanReport.PLDB_ID).
                success(function (data, status) {
                    $scope.PLDBBranches = data;
                    $scope.AppLoanReport.Branch_ID = $('#PLDBbranchId').val();
                    if ($scope.AppLoanReport.Branch_ID == '' || $scope.AppLoanReport.Branch_ID == "0") {
                        $scope.AppLoanReport.Branch_ID = "";
                        $scope.PLDBbrachdrop = false;
                    }
                    else {
                        $scope.PLDBbrachdrop = true;
                    }
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $scope.PLDBBranches = [];
                });
        }
    };


    $scope.SocietyType = $("#hdnSocietyType").val();

    $scope.BackToShow = function () {
        $scope.ShowAddNew = false;
    }
    $scope.GetTotalPLDBData = function (PLDBid, BranchId) {
        if ($scope.value == 'undefined') {
            $scope.value = '';
        }
        $http.get(SiteUrl + "/PLDB/GetLoanApplicationListstatus/?StatusVal=" + $scope.value + "&PLDBid=" + PLDBid + "&BranchId=" + BranchId).
            success(function (data, status) {
                $scope.PLDBStatusData = data;

            }).
            error(function (data, status) {
                $scope.PLDB = [];
            });

    }
    // START : Page load Function
    $scope.OnPageLoad = function () {
        var Temp = window.location.href.split('?');
        var Temp2 = Temp[1].split('=');
        $scope.value = Temp2[1];
        $scope._language = _language;
        //$scope.DistrictList();
        _language = 'en-us';
        if (_language === 'en-us') {
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

        $('.SetCalender').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            minDate: '0'
        });

        var userLevel = parseInt($('#hdnUserLevel').val(), 10);
        var districtId = $('#hdnUserDistrict').val();

        if (userLevel === 3) {
            $scope.IsDisabled = true;
            $scope.GetCityList();
        }
        else {
            $scope.IsDisabled = false;
        }
    };
    // END : Page load Function









    //START : Show Div For Insert / Update
    $scope.ShowDivForInsert = function () {
        $scope.ClearForm();
        $scope.GridWarehouses = false;
        $scope.divInsert = true;
    };
    //END : Show Div For Insert / Update

    //START : Back To List
    $scope.BackToList = function () {
        //$scope.ClearForm();
        $scope.BackToShow();
    };
    //END : Back To List
    $scope.Warehouse = [];


    // START : On Page Load
    $scope.OnPageLoad();
    //$scope.GetTotalPLDBData($scope.value);
    $scope.PLDBList();
    // END : On Page Load

    $scope.StatusName = "";

    if ($scope.value == "APPLICATIONSUBMITTED") {
        $scope.StatusName = "APPLICATION SUBMITTED";
    }
    else if ($scope.value == "OBJECTIONSUBMITTED") {
        $scope.StatusName = "OBJECTION SUBMITTED";
    }
    else if ($scope.value == "OBJECTIONRESPONSE") {
        $scope.StatusName = "OBJECTION RESPONSE";
    }
    else if ($scope.value == "REJECT") {
        $scope.StatusName = "REJECT";
    }
    else {
        $scope.StatusName = "";
    }

    //Delete Bank Details

    $scope.PLDBName = "";
    $scope.BranchName = "";


    $scope.ShowPLDBDetails = function () {
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/PLDB/GetLoanApplicationListstatus/?StatusVal=" + $scope.value + "&PLDBid=0&BranchId=0&Level=1").
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PLDBDataList = JSON.parse(data.DataList);
                $scope.ShowBranchWise = false;
                $scope.ShowPLDBWise = true;
                $scope.ShowDetailsWise = false;
                if ($scope.StatusName == "") {
                    $scope.StatusName = $scope.PLDBDataList[0].ProceesStatusTextEn;
                }

            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PLDBDataList = [];
            });
    }

    $scope.ShowBranchDetails = function (PldbId, PLDBName) {
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/PLDB/GetLoanApplicationListstatus/?StatusVal=" + $scope.value + "&PLDBid=" + PldbId + "&BranchId=0&Level=2").
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.BranchDataList = JSON.parse(data.DataList);
                $scope.ShowBranchWise = true;
                $scope.ShowPLDBWise = false;
                $scope.ShowDetailsWise = false;
                if ($scope.StatusName == "") {
                    $scope.StatusName = $scope.PLDBDataList[0].ProceesStatusTextEn;
                }
                if (PLDBName == undefined) {
                    $scope.PLDBName = "PLDB :- " + $scope.PLDB.filter(function (x) { return x.PldbId == $scope.AppLoanReport.PLDB_ID })[0].PLDBName;
                } else {
                    $scope.PLDBName = "PLDB :- " + PLDBName;
                }
                $scope.BranchName = "";
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.BranchDataList = [];
            });
    }

    $scope.ShowDetails = function (BrId, BranchName) {
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/PLDB/GetLoanApplicationListstatus/?StatusVal=" + $scope.value + "&PLDBid=0&BranchId=" + BrId + "&Level=3").
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PLDBStatusData = JSON.parse(data.DataList);
                $scope.ShowBranchWise = false;
                $scope.ShowPLDBWise = false;
                $scope.ShowDetailsWise = true;
                // $scope.PLDBName = PLDBName;
                if ($scope.StatusName == "") {
                    $scope.StatusName = $scope.PLDBDataList[0].ProceesStatusTextEn;
                }
                if (BranchName == undefined) {
                    $scope.BranchName = "Branch :- " + $scope.PLDBBranches.filter(function (x) { return x.BLOCK_ID == $scope.AppLoanReport.Branch_ID })[0].BLOCK_ENG;
                } else {
                    $scope.BranchName = "Branch :- " + BranchName;
                }

                //$scope.BranchName = "Branch :- " + BranchName;
                if ($scope.PLDBName == undefined || $scope.PLDBName == "") {
                    $scope.PLDBName = "PLDB :- " + $scope.PLDB.filter(function (x) { return x.PldbId == $scope.AppLoanReport.PLDB_ID })[0].PLDBName;
                } else {
                    // $scope.PLDBName = "PLDB :- " + PLDBName;
                }

            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PLDBStatusData = [];
            });
    }

    // alert($('#PLDBId').val());
    if ($('#PLDBId').val() == "0" || $('#PLDBId').val() == "") {
        $scope.ShowBranchWise = false;
        $scope.ShowPLDBWise = true;
        $scope.ShowDetailsWise = false;
        $scope.ShowPLDBDetails();
    } else if ($('#hdnBranchId').val() != "0" && $('#hdnBranchId').val() != "") {
        $scope.ShowBranchWise = false;
        $scope.ShowPLDBWise = false;
        $scope.ShowDetailsWise = true;
        $scope.ShowDetails($('#hdnBranchId').val(), );
    } else if ($('#PLDBId').val() != "0" && $('#PLDBId').val() != "") {
        $scope.ShowBranchWise = true;
        $scope.ShowPLDBWise = false;
        $scope.ShowDetailsWise = false;
        $scope.ShowBranchDetails($('#PLDBId').val(), );
    }

    $scope.GoBack = function (type) {
        if (type == 1) {
            window.location.href = "../User/dashboard";
        } else if (type == 2) {
            $scope.ShowBranchWise = false;
            $scope.ShowPLDBWise = true;
            $scope.ShowDetailsWise = false;
            $scope.ShowPLDBDetails();
            $scope.PLDBName = "";
            $scope.BranchName = "";
        } else if (type == 3) {
            $scope.ShowBranchWise = true;
            $scope.ShowPLDBWise = false;
            $scope.ShowDetailsWise = false;
            //$scope.PLDBName = PLDBName;
            $scope.BranchName = "";
            //    $scope.ShowBranchDetails($('#PLDBId').val());
            //$scope.ShowDetails($('#hdnBranchId').val());
        }
    }


    $scope.ExportDataPLDBDetails = function () {
        
        $(".blockUI").fadeIn("slow");
        var uri = 'data:application/vnd.ms-excel;base64,'
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
        var fileName = "LoanApplication_.xls";
        var tabledata = "<tr><th colspan='3' style='text-align: center'>PLDB Loan Application Report</th></tr><tr><th colspan='3'  style='text-align: left'></th></tr><tr><th colspan='3'  style='text-align: left'>Status : " + $scope.StatusName+"</th></tr><tr><th>Sr.No</th><th>PLDB Name</th><th>Total Application</th></tr>";

        $scope.PLDBDataList.forEach(function (item, index) {
            tabledata += "<tr><td>" + (index + 1) + "</td><td>" + item.PLDBName + "</td><td>" + item.TotalCount + "</td></tr>";
        });

        var excelData = "<table  border='1'>" + tabledata+"</table>";
        excelData = excelData.replace(/₹/g, "").replace(/\n/g, "");
        var ctx = { worksheet: "Loan Application" || 'Worksheet', table: excelData }
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

    $scope.ExportDataBranchDetails = function (PldbId, PLDBName) {
        $(".blockUI").fadeIn("slow");
        var uri = 'data:application/vnd.ms-excel;base64,'
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
        var fileName = "LoanApplication_.xls";
        var PLDBNAME = $scope.BranchDataList[0].PLDBName;
        var tabledata = "<tr><th colspan='3' style='text-align: center'>PLDB Loan Application Report</th></tr><tr><th colspan='3'  style='text-align: left'></th></tr><tr><th colspan='3'  style='text-align: left'>Status : " + $scope.StatusName+"</th></tr><tr><th colspan='3'  style='text-align: left'>PLDB : " + PLDBNAME + "</th></tr><tr><th>Sr.No</th><th>Branch Name</th><th>Total Application</th></tr>";

        $scope.BranchDataList.forEach(function (item, index) {
            tabledata += "<tr><td>" + (index + 1) + "</td><td>" + item.BranchName_En + "</td><td>" + item.TotalCount + "</td></tr>";
        });

        var excelData = "<table  border='1'>" + tabledata + "</table>";

        excelData = excelData.replace(/₹/g, "").replace(/\n/g, "");
        var ctx = { worksheet: "Loan Application" || 'Worksheet', table: excelData }
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

    $scope.ExportDataDetails = function (BrId, BranchName) {
        $(".blockUI").fadeIn("slow");
        var uri = 'data:application/vnd.ms-excel;base64,'
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
        var fileName = "LoanApplication_.xls";
        var PLDBNAME = $scope.BranchDataList[0].PLDBName;
        var BRANCHNAME = $scope.PLDBStatusData[0].BranchName_En
        var tabledata = "<tr><th colspan='3' style='text-align: center'>PLDB Loan Application Report</th></tr><tr><th colspan='3'  style='text-align: left'></th></tr><tr><th colspan='3'  style='text-align: left'>Status : " + $scope.StatusName+"</th></tr><tr><th colspan='3'  style='text-align: left'>" + $scope.PLDBName + ",  " + $scope.BranchName + "</th></tr><tr><th>Sr.No</th><th>Application No.</th><th>Member Name</th><th>Father Name</th><th>Submission Date</th></tr>";        
        $scope.PLDBStatusData.forEach(function (item, index) {
            tabledata += "<tr><td>" + (index + 1) + "</td><td>" + item.application_no + "</td><td>" + item.MemberName + "</td><td>" + item.FatherName + "</td><td>" + item.FormatedDate+"</td></tr>";
        });

        var excelData = "<table  border='1'>" + tabledata + "</table>";

        excelData = excelData.replace(/₹/g, "").replace(/\n/g, "");
        var ctx = { worksheet: "Loan Application" || 'Worksheet', table: excelData }
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

    



});