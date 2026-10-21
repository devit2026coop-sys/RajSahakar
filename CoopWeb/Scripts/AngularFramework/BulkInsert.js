var BulkInsertModule = angular.module("BulkInsertMod", ['angularUtils.directives.dirPagination']);
BulkInsertModule.controller("BulkInsertCnt", function ($scope, $http) {
    $scope.SocityExcelDetailsData = [];
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
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
    $scope.SaveBulkInsert = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.BulkInsertFile == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Excel Sheet ' : '\n <br /> कृपया एक्सेल शीट अपलोड करें |';
        }
        if ($scope.ActIdVal == '0') {
            message += $scope._language == 'en-us' ? '\n <br /> Please Select Act ' : '\n <br /> कृपया अधिनियम  का चयन करे  |';
        }
        if (message.length > 27) {
            ShowModelAlert("Bulk Insert", message);
            return false;
        }

        var formData = new FormData();
        formData.append("ExcelSheetFile", $scope.BulkInsertFile);
        formData.append("ActId", $scope.ActIdVal);
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Society/InsertSocityByExcel", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert(data);
                location.reload();
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
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
        if (window.location.toString().toLowerCase().indexOf("court") > -1) {
            $scope.GetSocityExcelDetailsListCourt();
        }
        else {
            $scope.GetSocityExcelDetailsList();
        }
        // Get List of all ACT
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
            success(function (data, status) {
                $scope.ActList = data;
            }).
            error(function (data, status) {
            });

        $('#txtRegistrationDate1').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });

        $(".se-pre-con").fadeOut("slow");
    };
    // END : Call default functions on page load
    $scope.GetSocityExcelDetailsList = function () {
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
                    $scope.DistrictId = data;
                    $("#ExlSampleLink").attr("href", "../Content/SocietyExcel/" + data + ".xlsx");
                }
            }).
            error(function (data, status) {
                $scope.ExlFileShow = false;
                $scope.SocityExcelDetailsData = [];
            });
    };
  
    $scope.GetExlSociety = function (Id, StatusId, TypeId) {
      
        $(".se-pre-con").fadeIn("slow");
        $scope.ExlSocietyDetailsData = [];
        $scope.ExlSocietyDetailsDataFail = [];
        $http.post(SiteUrl + "/Society/GetExlSocietyDetails", { "ExlId": Id, "StatusId": StatusId, "TypeId": TypeId}).
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
    $scope.EditShowRow = function (itemm, index) {
        
        $scope.OldEditData.SocietyName = itemm.SocietyName;
        $scope.OldEditData.RegistrationNo = itemm.RegistrationNo;
        $scope.OldEditData.RegistrationDate = itemm.RegistrationDate;
        $scope.OldEditData.Name = itemm.Name;
        $scope.OldEditData.ContactNo = itemm.ContactNo;
        $scope.OldEditData.Aadh = itemm.Aadh;
        $scope.OldEditData.SocietyTypeId = itemm.SocietyTypeId.toString();
        itemm.show = false;
        $scope.ExlSocietyDetailsData.forEach(function (item, indx) {
            if (itemm != item) {
               
                item.show = true;
            }
        });
    }

    $scope.CancelEditData = function (itemm,index) {
        itemm.SocietyName = $scope.OldEditData.SocietyName;
        itemm.RegistrationNo = $scope.OldEditData.RegistrationNo;
        itemm.RegistrationDate = $scope.OldEditData.RegistrationDate;
        itemm.Name = $scope.OldEditData.Name;
        itemm.ContactNo = $scope.OldEditData.ContactNo;
        itemm.Aadh = $scope.OldEditData.Aadh;
        $scope.OldEditData.SocietyTypeId = itemm.SocietyTypeId.toString();
        $scope.OldEditData = { SocietyName: "", RegistrationNo: "", RegistrationDate: "", Name: "", ContactNo: "", Aadh: "" };
        $scope.ExlSocietyDetailsData.forEach(function (item, indx) {
            item.show = true;
        });
    }

    $scope.SaveEditData = function (item, index) {
        //if (item.SocietyName != '' && item.RegistrationNo != '' && item.RegistrationDate != '' && item.Name != '' && item.ContactNo != '' && item.Aadh) {
        if (item.Aadh!='' && item.Aadh.length < 12 || item.Aadh.length > 12) {
                ShowOkMessage("Error", "Aadhaar must be 12 digit number", function () { });
            } else {
                $(".se-pre-con").fadeIn("slow");
                $http.post("../Society/UpdateExcelSocityDetailsByExcelUpload/", { SocietyId: item.SocietyID, AdministrativeId: item.AdministrativeId, SocietyName: item.SocietyName, RegistrationNo: item.RegistrationNo, RegistrationDate: item.RegistrationDate, Name: item.Name, ContactNo: item.ContactNo, Aadh: item.Aadh, SocietyTypeId: item.SocietyTypeId }).
                    success(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        if (data == "-3") {
                            ShowOkMessage("Error", "Please Enter valid  Registration Date", function () { });
                        } else if (data != "1") {
                            ShowOkMessage("Error", "There is some problem, Please try after some time", function () { });
                        } else {
                            item.show = true;
                        }
                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        alert('Error while fetching Society List For Election Officer.');
                    });
            }
            
        //} else {
         //   ShowOkMessage("Error", "Please Fill All Values", function () { });
       // }
    }

    //END : Update Next Hearing Date

    //GetSocietyTypeList

    $scope.GetSocietyTypeList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=" + '').
            success(function (data, status) {
                $scope.SocietyTypeList = data;
            }).
            error(function (data, status) {
                $scope.SocietyTypeList = [];
            });
    };
    $scope.GetSocietyTypeList();



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
            $http.post(SiteUrl + "/Society/GetSocietyDeleteFaildRecord", { "RegistrationNo": id}).
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

    

    // START : Call Functions on Page Load
    $scope.OnPageLoad();
    //END : Call Functions on Page Load
});