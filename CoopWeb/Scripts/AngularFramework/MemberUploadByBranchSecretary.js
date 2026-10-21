var BulkInsertModule = angular.module("MemberUploadMod", ['angularUtils.directives.dirPagination']);
BulkInsertModule.controller("MemberUploadCnt", function ($scope, $http) {
    $scope.MemberExcelDetailsData = [];
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

    $scope.UploadBulkInsertFile = function (file) {
        $scope.BulkInsertFile = file[0];
    };
    //END

    $scope.ClearBulkInsertForm = function () {
        $("#fluBulkInsert").val('')
    }


    // START : Save Bulk Insert
    $scope.SaveUploadMember = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.BulkInsertFile == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please Upload Excel Sheet ' : '\n <br /> कृपया एक्सेल शीट अपलोड करें |';
        }
        //if ($scope.ActIdVal == '0') {
        //    message += $scope._language == 'en-us' ? '\n <br /> Please Select Act ' : '\n <br /> कृपया अधिनियम  का चयन करे  |';
        //}
        if (message.length > 27) {
            ShowModelAlert("Bulk Insert", message);
            return false;
        }

        var formData = new FormData();
        formData.append("ExcelSheetFile", $scope.BulkInsertFile);
        //formData.append("ActId", $scope.ActIdVal);
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/SocietyMemberRegistration/InsertSocityByExcel", formData,
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
            $scope.GetMeberExcelDetailsListCourt();
        }
        else {
            $scope.GetMeberExcelDetailsList();
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
    $scope.GetMeberExcelDetailsList = function () {
        $scope.MemberExcelDetailsData = [];
        $http.post(SiteUrl + "/SocietyMemberRegistration/GetMemberExcelDetails").
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.ExlFileShow = true;
                }
                $scope.MemberExcelDetailsData = data;
            }).
            error(function (data, status) {
                $scope.ExlFileShow = false;
                $scope.MemberExcelDetailsData = [];
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
                $scope.MemberExcelDetailsData = [];
            });
    };
    $scope.GetExlSociety = function (Id, StatusId) {
        $(".se-pre-con").fadeIn("slow");
        $scope.ExlMemberDetailsData = [];
        $scope.ExlMemberDetailsDataFail = [];
        $http.post(SiteUrl + "/SocietyMemberRegistration/GetExlMemberDetails", { "ExlId": Id, "StatusId": StatusId }).
            success(function (data, status) {
                if (StatusId == 1) {
                    if (data.length > 0) {
                        $scope.ExlSocietyShow = true;
                        $scope.ExlFileShow = false;
                        $scope.ExlSocietyShowFail = false;
                    }
                    $scope.ExlMemberDetailsDataFail = [];
                    $scope.ExlMemberDetailsData = data;

                    $scope.ExlMemberDetailsData.forEach(function (item, indx) {
                        item.show = true;
                    });
                    $(".se-pre-con").fadeOut("slow");
                }
                else {
                    if (data.length > 0) {
                        $scope.ExlSocietyShow = false;
                        $scope.ExlSocietyShowFail = true;
                    }
                    $scope.ExlMemberDetailsData = [];
                    $scope.ExlMemberDetailsDataFail = data;
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.ExlSocietyShow = false;
                $scope.ExlSocietyShowFail = false;
                $scope.ExlMemberDetailsData = [];
                $scope.ExlMemberDetailsDataFail = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    $scope.DeleterExlSociety = function (Id, StatusId, RequestId) {

        $scope.ExlMemberDetailsData = [];
        $scope.ExlMemberDetailsDataFail = [];
        ShowAlertMessage("Warning", $scope._language == 'en-us' ? "Are you sure to delete this record ?." : 'क्या आप इस रिकॉर्ड को हटाना सुनिश्चित कर रहे हैं? कृपया रिकॉर्ड को हटाने के लिए "Yes" बटन पर क्लिक करें ।', function () {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/SocietyMemberRegistration/DeleteExlMemberDetails", { "ExlId": Id, "StatusId": StatusId, "RequestId": RequestId }).
                success(function (data, status) {

                    $scope.ExlMemberDetailsDataFail = [];
                    // $scope.ExlMemberDetailsData = data;
                    $scope.GetMeberExcelDetailsList();
                    $scope.ExlSocietyShow = false;
                    //location.reload();
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $scope.ExlMemberDetailsData = [];
                    $scope.ExlMemberDetailsDataFail = [];
                    $(".se-pre-con").fadeOut("slow");
                });
        })
    };
    
    $scope.GetMeberExcelDetailsListCourt = function () {
        $scope.MemberExcelDetailsData = [];
        $http.post(SiteUrl + "/Society/GetSocityExcelDetailsCourt").
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.ExlFileShow = true;
                }
                $scope.MemberExcelDetailsData = data;
            }).
            error(function (data, status) {
                $scope.ExlFileShow = false;
                $scope.MemberExcelDetailsData = [];
            });
    };

   
    $scope.OldEditData = { Name: "", FatherHusbandName: "", MobileNo: "", AADHARNo: "", OldMembershipNo: "", MembershipDate: "" };
    $scope.EditShowRow = function (index) {

        $scope.OldEditData.Name = $scope.ExlMemberDetailsData[index].Name;
        $scope.OldEditData.FatherHusbandName = $scope.ExlMemberDetailsData[index].FatherHusbandName;
        $scope.OldEditData.MobileNo = $scope.ExlMemberDetailsData[index].MobileNo;
        $scope.OldEditData.AADHARNo = $scope.ExlMemberDetailsData[index].AADHARNo;
        $scope.OldEditData.OldMembershipNo = $scope.ExlMemberDetailsData[index].OldMembershipNo;
        $scope.OldEditData.MembershipDate = $scope.ExlMemberDetailsData[index].MembershipDate;



        $scope.ExlMemberDetailsData.forEach(function (item, indx) {
            if (index == indx) {
                item.show = false;
            } else {
                item.show = true;
            }
        });
    }

    $scope.CancelEditData = function (index) {
        $scope.ExlMemberDetailsData[index].Name = $scope.OldEditData.Name;
        $scope.ExlMemberDetailsData[index].FatherHusbandName = $scope.OldEditData.FatherHusbandName;
        $scope.ExlMemberDetailsData[index].MobileNo = $scope.OldEditData.MobileNo;
        $scope.ExlMemberDetailsData[index].AADHARNo = $scope.OldEditData.AADHARNo;
        $scope.ExlMemberDetailsData[index].OldMembershipNo = $scope.OldEditData.OldMembershipNo;
        $scope.ExlMemberDetailsData[index].MembershipDate = $scope.OldEditData.MembershipDate;
        $scope.OldEditData = { Name: "", FatherHusbandName: "", MobileNo: "", AADHARNo: "", OldMembershipNo: "", MembershipDate: "" };
        $scope.ExlMemberDetailsData.forEach(function (item, indx) {
            item.show = true;
        });
    }

    $scope.SaveEditData = function (item, index) {

        if (item.Name != '' && item.FatherHusbandName != '' && item.MobileNo != '' && item.AADHARNo != '' && item.OldMembershipNo != '' && item.MembershipDate) {

            if (item.AADHARNo.length < 12 || item.AADHARNo.length > 12) {
                ShowOkMessage("Error", "Aadhaar must be 12 digit number", function () { });
            } else {
                $(".se-pre-con").fadeIn("slow");
                $http.post("../SocietyMemberRegistration/UpdateExcelMemberDetailsByExcelUpload/", { RequestId: item.RequestId, MemberName: item.Name, FatherHusbandName: item.FatherHusbandName, MobileNo: item.MobileNo, AADHARNo: item.AADHARNo, MembershipNo: item.OldMembershipNo, MembershipDate: item.MembershipDate }).
                    success(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        if (data == "-3") {
                            ShowOkMessage("Error", "Please Enter valid  Registration Date", function () { });
                        } else if (data != "1") {
                            ShowOkMessage("Error", "There is some problem, Please try after some time", function () { });
                        } else {
                            $scope.ExlMemberDetailsData[index].show = true;
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

    // START : Call Functions on Page Load
    $scope.OnPageLoad();
    //END : Call Functions on Page Load
});