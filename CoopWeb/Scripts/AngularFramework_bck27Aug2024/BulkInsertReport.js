var BulkInsertModule = angular.module("BulkInsertMod", ['angularUtils.directives.dirPagination']);
BulkInsertModule.controller("BulkInsertCnt", function ($scope, $http) {
    $scope.SocityExcelDetailsData = [];
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 35;
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
        if (window.location.toString().toLowerCase().indexOf("court") > -1) {
            $scope.GetSocityExcelDetailsListCourt();
        }
        else {
            $scope.GetSocityExcelDetailsList();
        }
        // Get List of all ACT
        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
        //    success(function (data, status) {
        //        $scope.ActList = data;
        //    }).
        //    error(function (data, status) {
        //    });

        //$('#txtRegistrationDate1').datepicker({
        //    showOn: "both",
        //    changeMonth: true,
        //    buttonImage: "../images/calender.png",
        //    changeYear: true,
        //    dateFormat: "dd/mm/yy",
        //    maxDate: '0D'
        //});

        //$(".se-pre-con").fadeOut("slow");
    };
    // END : Call default functions on page load
    $scope.GetSocityExcelDetailsList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.SocityExcelDetailsData = [];
        $http.post(SiteUrl + "/MIS/GetBulkInsertByExcelReport").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                    $scope.ExlFileShow = true;
                    $scope.SocityExcelDetailsData = JSON.parse(data.DataList);
            }).
            error(function (data, status) {
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
    $scope.GetExlSociety = function (Id, StatusId) {
        $(".se-pre-con").fadeIn("slow");
        $scope.ExlSocietyDetailsData = [];
        $scope.ExlSocietyDetailsDataFail = [];
        $http.post(SiteUrl + "/Society/GetExlSocietyDetails", { "ExlId": Id, "StatusId": StatusId }).
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

    $scope.GetSocityExcelDetailsListCourt = function () {
        $scope.SocityExcelDetailsData = [];
        $http.post(SiteUrl + "/Society/GetSocityExcelDetailsCourt").
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
    $scope.GetExlSocietyCourt = function (Id, StatusId) {
        $(".se-pre-con").fadeIn("slow");
        $scope.ExlSocietyDetailsData = [];
        $scope.ExlSocietyDetailsDataFail = [];
        $http.post(SiteUrl + "/Society/GetExlSocietyDetailsCourt", { "ExlId": Id, "StatusId": StatusId }).
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
                    // alert(data);
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

    //COURT
    // START : Save Bulk Insert COURT
    $scope.SaveBulkInsertCourt = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.BulkInsertFileCourt == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Upload Excel Sheet' : '\nकृपया एक्सेल शीट अपलोड करें |';
            if (message.length > 27) {
                ShowModelAlert("Court Case Bulk Insert", message);
                return false;
            }
        }

        var formData = new FormData();
        formData.append("ExcelSheetFile", $scope.BulkInsertFileCourt);
        $http.post(SiteUrl + "/Society/InsertSocityByExcelCourt", formData,
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
                ShowModelAlert("Insert Court Data", "Error While Upload Excel Sheet");
            });
    };
    // END :Save COURT 
    //END

    //START : Clear Next Hearing Date
    $scope.ClearNextHearing = function () {
        $scope.Hearing.NextHearingDate = '';
    }
    //END : Clear Next Hearing Date

    //START : GET Next Hearing Date
    $scope.GetNextHearingDate = function (item) {
        $scope.NextHearingDetails = item;
        console.log($scope.NextHearingDetails);
    }
    //END : GET Next Hearing Date



    //START : Update Next Hearing Date
    $scope.UpdateNextHearingDate = function () {
        if ($('#txtRegistrationDate1').val() == '') {
            alert('Please provide Next Hearing Date.');
            return;
        }

        $http.post("../CourtCase/UpdateLastHearingDate/", { "Id": $scope.NextHearingDetails.ID, "NextHearingDate": $('#txtRegistrationDate1').val() }).
            success(function (data, status) {
                var result = parseInt(data, 10);
                if (result == -1) {
                    alert('Error!!!');
                }
                else if (result > 0) {
                    alert('Next Hearing date updated successfully.');
                }
                location.reload();
            }).
            error(function (data, status) {
                $scope.ElectionOfficerData = [];
                alert('Error while fetching Society List For Election Officer.');
            });
    }
    $scope.OldEditData = { SocietyName: "", RegistrationNo: "", RegistrationDate: "", Name: "", ContactNo: "", Aadh: "" };
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

    $scope.SaveEditData = function (item, index) {

        if (item.SocietyName != '' && item.RegistrationNo != '' && item.RegistrationDate != '' && item.Name != '' && item.ContactNo != '' && item.Aadh) {

            if (item.Aadh.length < 12 || item.Aadh.length > 12) {
                ShowOkMessage("Error", "Aadhaar must be 12 digit number", function () { });
            } else {
                $(".se-pre-con").fadeIn("slow");
                $http.post("../Society/UpdateExcelSocityDetailsByExcelUpload/", { SocietyId: item.SocietyID, AdministrativeId: item.AdministrativeId, SocietyName: item.SocietyName, RegistrationNo: item.RegistrationNo, RegistrationDate: item.RegistrationDate, Name: item.Name, ContactNo: item.ContactNo, Aadh: item.Aadh }).
                    success(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        if (data == "-3") {
                            ShowOkMessage("Error", "Please Enter valid  Registration Date", function () { });
                        } else if (data != "1") {
                            ShowOkMessage("Error", "There is some problem, Please try after some time", function () { });
                        } else {
                            $scope.ExlSocietyDetailsData[index].show = true;
                        }
                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        alert('Error while fetching Society List For Election Officer.');
                    });
            }


        } else {
            ShowOkMessage("Error", "Please Fill All Values", function () { });
        }


    }

    //END : Update Next Hearing Date

    $scope.DeleteDuplicateData = function (id) {
        ShowAlertMessage('Warning', 'Are you sure you want to delete this Record?', function () {
            $http.post("../Society/DeleteFaildRecord/", { SocietyID: id }).
                success(function (data, status) {
                    if (data == 1) {
                        alert("Data has been removed !");
                    }

                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    alert('Error while fetching Society.');
                });
        });
    }


    $scope.getActualSociety = function (id) {
        $http.post(SiteUrl + "/Society/GetSocietyDeleteFaildRecord", { "RegistrationNo": id }).
            success(function (data, status) {
                if (data == "") {
                    $scope.SocietiesDetails = [];
                    alert("There is no data available!");
                    $('#myModals').modal('hide');
                }
                else {
                    $scope.SocietiesDetails = data;
                    $scope.myModals = true;
                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert('Error while fetching Society List For Election Officer.');
            });
    }

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