
var MasterMod = angular.module('ApplicationFlowModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ApplicationFlowCont", function ($scope, $rootScope, $http) {
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
    $scope.showSearchDate = false;
    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];
    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        if ($scope.Filter.FilterType == 'wrs.created_date') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            // $scope.AddCalender();
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }
    $scope.ClearSearch = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.FilterType = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.SearchData();
    }
    /*------------End Paging and Filter-----------------*/
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = [];
    $scope.ShowOTPDiv = false;
    $scope.ObjectionFlagBasic = false;
    $scope.ObjectionFlagAdmin = false;
    $scope.ObjectionFlagOffice = false;
    $scope.RoleId = 0;
    $scope.SaveSocietyApplicationFlowStatus = function () {
        // $scope.ApplicationFlowMaster.Remark = $("#txtRemark").val();
        $scope.showLoader = true;        
        if ($scope.ApplicationFlowMaster.StatusId != 0 && $scope.ApplicationFlowMaster.Remark != "") {
            if ($scope.ApplicationFlowMaster.StatusId != 17 && $scope.ApplicationFlowMaster.StatusId != 14 ) {
                if ($scope.ApplicationFlowMaster.StatusId == 5 || $scope.ApplicationFlowMaster.StatusId == 10 || $scope.ApplicationFlowMaster.StatusId == 20) {
                    if ($scope.ObjectionFlagBasic == false && $scope.ObjectionFlagAdmin == false && $scope.ObjectionFlagOffice == false) {
                        alert(_language == 'en-us' ? 'Please Select Objection Belong To.\n' : 'कृप्या आपत्ति संबंधित है चुनें|\n');
                    }
                    else {
                        $(".se-pre-con").fadeIn("slow");
                        $scope.ObjectionFlagBasic = true;
                        $http.post("../ApplicationFlow/SaveSocietyObjectionBelong", { "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "ObjectionFlagBasic": $scope.ObjectionFlagBasic, "ObjectionFlagAdmin": $scope.ObjectionFlagAdmin, "ObjectionFlagOffice": $scope.ObjectionFlagOffice }).
                            success(function (data, status) {
                                $(".se-pre-con").fadeOut("slow");
                                $scope.saveDataInDB();
                            }).
                            error(function (data, status) {
                                $(".se-pre-con").fadeOut("slow");
                                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                            });
                    }
                }
                else {
                    $scope.saveDataInDB();
                }
            }
            else {
                // $scope.saveDataInDB();
                // $http.get("../ApplicationFlow/GenerateAadhaarOTP").
              


                AadhaarAuth("", $http, "", true, function (authData) {
                    //$scope.InsertAction(authData.TransId);
                    $scope.ApplicationFlowMaster.TXN = authData.TransId;
                    if ($scope.ApplicationFlowMaster.StatusId == 14) {
                        $scope.saveDataInDB();
                    } else {

                        $(".se-pre-con").fadeIn("slow");
                        $http.post(SiteUrl + "/ApplicationFlow/VerifyAadhaarOTPForEsing", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": authData.TransId, "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "StatusId": $scope.ApplicationFlowMaster.StatusId, "Remark": $scope.ApplicationFlowMaster.Remark, "IsAmendment": $scope.IsSocietyAmendment }).
                            success(function (data, status) {
                                $(".se-pre-con").fadeOut("slow");
                                if (data == "Fail,-2") {
                                    ShowOkMessage("", "SECRETARY dose not exist in member list", function () {
                                        // location.reload();
                                    });
                                } else
                                    if (data == "Fail" || data == "0" || data == 0) {
                                        ShowOkMessage("Error", "Certificate not Generate. There is some problem.Please try after some time or contact on support.", function () { });
                                    } else
                                        if (data != "Success") {
                                          //  alert(data);
                                        }
                                        else if (data == "Success") {
                                            alert('Application for Society Registration Approved');
                                            location.reload();
                                        }
                                $scope.$apply();
                            }).
                            error(function (data, status) {
                                $(".se-pre-con").fadeOut("slow");
                                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                                location.reload();
                            });
                    }

                });


                //$http.get("../ApplicationFlow/GenerateAadhaarOTPForEsing").
                //    success(function (data, status) {
                //        $(".se-pre-con").fadeOut("slow");
                //        if (data.indexOf('NO#') >= 0) {
                //            alert("Due to some issue OTP not send");
                //            //location.reload();
                //            //window.location = '../ApplicationFlow/ApplicationFlow';
                //        } else {
                //            if (data != "LOGOUT") {
                //                // alert(data);
                //                $scope.ApplicationFlowMaster.TXN = data;
                //                alert(_language == 'en-us' ? 'OTP has been sent to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n');
                //                $scope.ShowOTPDiv = true;
                //            }
                //            else {
                //                $(".se-pre-con").fadeOut("slow");
                //                window.location = 'https://sso.rajasthan.gov.in/signin';
                //            }
                //        }
                //    }).
                //    error(function (data, status) {
                //        $(".se-pre-con").fadeOut("slow");
                //        alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                //    });
            }
        }
        else {
            alert(_language == 'en-us' ? 'Please Select Status and Remark\n' : 'कृपया स्थिति और टिप्पणी जोड़ें|\n');
        }
        $scope.showLoader = false;
    };

    $scope.ClearAppFlowForm = function () {
        $scope.ApplicationFlowMaster.Remark = '';
        $scope.ApplicationFlowMaster.StatusId = '0';
        $scope.ApplicationFlowMaster.OfficeId = '0';
        $scope.ReportSocietyFile = '';
        $scope.ShowOTPDiv = false;
    }

    $scope.UploadReportFile = function (file) {
        $scope.ReportSocietyFile = file[0];
    }
    $scope.UploadeReportFileInDB = function () {
        var formData = new FormData();
        formData.append("SocietyReportFile", $scope.ReportSocietyFile);
        formData.append("SocietyIDEnc", $scope.ApplicationFlowMaster.SocietyIDEnc);
        $http.post(SiteUrl + "/ApplicationFlow/SaveSocietyVerifyReportFile", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
            }).
            error(function (data, status) {
            });
    }
    $scope.saveDataInDB = function () {
        $scope.showLoader = true;
        if ($scope.ApplicationFlowMaster.StatusId == 3 || $scope.ApplicationFlowMaster.StatusId == 9 || $scope.ApplicationFlowMaster.StatusId == 12 || $scope.ApplicationFlowMaster.StatusId == 19 || $scope.ApplicationFlowMaster.StatusId == 23 || $scope.ApplicationFlowMaster.StatusId == 35 || $scope.ApplicationFlowMaster.StatusId == 36 || $scope.ApplicationFlowMaster.StatusId == 1058) {
            if ($scope.ApplicationFlowMaster.OfficeId == '0' || $scope.ApplicationFlowMaster.OfficeId == 0) {
                alert("Please Select Officer.");
                return;
            }
            else {
                $scope.ApplicationFlowMaster.UserId = $scope.ApplicationFlowMaster.OfficeId;
            }
        }

        if ($scope.ApplicationFlowMaster.StatusId == 1065) {
            if ($scope.ApplicationFlowMaster.AmendmendAmount == '' || $scope.ApplicationFlowMaster.AmendmendAmount == undefined || $scope.ApplicationFlowMaster.AmendmendAmount=="0") {
                alert('en-us' ? '\n  Please enter amount for amendment' : '\n  कृपया संशोधन के लिए राशि दर्ज करें |');
                return;
            }
            if ($scope.ApplicationFlowMaster.ReceiptNumber == '' || $scope.ApplicationFlowMaster.ReceiptNumber == undefined || $scope.ApplicationFlowMaster.ReceiptNumber == "0") {
                alert('en-us' ? '\n  Please enter amount receipt number for amendment' : '\n  संशोधन के लिए कृपया राशि रसीद संख्या दर्ज करें |');
                return;
            }
        }

        if ($scope.ReportSocietyFile == undefined && $scope.ApplicationFlowMaster.StatusId == 36) {
            alert('en-us' ? '\n  Please Upload File(PDF)' : '\n  कृपया फ़ाइल अपलोड करें(पीडीएफ) |');
            return;
        }
        else if ($scope.ReportSocietyFile != undefined && $scope.ApplicationFlowMaster.StatusId == 36) {
            $scope.UploadeReportFileInDB();
        }
        if ($scope.ApplicationFlowMaster.StatusId == 13 || $scope.ApplicationFlowMaster.StatusId == 21 || $scope.ApplicationFlowMaster.StatusId == 6) {
            ShowAlertMessage("Warning", 'en-us' ? '\n  Are you sure you want to reject this society application ?' : '\n  क्या आप वाकई इस सोसायटी एप्लिकेशन को अस्वीकार करना चाहते हैं?', function () {
                $(".se-pre-con").fadeIn("slow");
                $http.post(SiteUrl + "/ApplicationFlow/SaveSocietyApplicationFlowAmendment1958Status", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": $scope.ApplicationFlowMaster.TXN, "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "Remark": $scope.ApplicationFlowMaster.Remark, "StatusId": $scope.ApplicationFlowMaster.StatusId, "UserId": $scope.ApplicationFlowMaster.UserId, "AmendmendAmount": $scope.ApplicationFlowMaster.AmendmendAmount, "ReceiptNumber": $scope.ApplicationFlowMaster.ReceiptNumber, "AmendmentId": $scope.AmendmentId }).
                    success(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        if (data == 1) {
                            if ($scope.ApplicationFlowMaster.StatusId != 17) {
                                ShowOkMessage("", _language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n', function () { location.reload(); });
                            }
                            else if ($scope.ApplicationFlowMaster.StatusId == 17) {
                                $scope.CertificateGenerate($scope.ApplicationFlowMaster.SocietyIDEnc);
                            }
                            else {
                                ShowOkMessage("", _language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n', function () { location.reload(); });
                            }
                        }
                        else if (data == -2) {
                            ShowOkMessage("", "SECRETARY dose not exist in member list", function () { });
                        } else {
                            $(".se-pre-con").fadeOut("slow");
                            if ($scope.ApplicationFlowMaster.StatusId == 14 && data.toString() == "Invalid OTP!") {
                                ShowOkMessage("", data.toString(), function () { });
                            } else {
                                ShowOkMessage("", _language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n', function () { });
                            }
                        }
                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        alert("Error while insert");
                    });

            });
        } else {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/ApplicationFlow/SaveSocietyApplicationFlowAmendment1958Status", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": $scope.ApplicationFlowMaster.TXN, "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "Remark": $scope.ApplicationFlowMaster.Remark, "StatusId": $scope.ApplicationFlowMaster.StatusId, "UserId": $scope.ApplicationFlowMaster.UserId, "AmendmendAmount": $scope.ApplicationFlowMaster.AmendmendAmount, "ReceiptNumber": $scope.ApplicationFlowMaster.ReceiptNumber}).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data == 1) {
                        if ($scope.ApplicationFlowMaster.StatusId != 17) {
                            ShowOkMessage("", _language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n', function () { location.reload(); });
                        }
                        else if ($scope.ApplicationFlowMaster.StatusId == 17) {
                            $scope.CertificateGenerate($scope.ApplicationFlowMaster.SocietyIDEnc);
                        }
                        else {
                            ShowOkMessage("", _language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n', function () { location.reload(); });
                        }
                    }
                    else if (data == -2) {
                        ShowOkMessage("", "SECRETARY dose not exist in member list", function () { });
                    } else {
                        $(".se-pre-con").fadeOut("slow");
                        if ($scope.ApplicationFlowMaster.StatusId == 14 && data.toString() == "Invalid OTP!") {
                            ShowOkMessage("", data.toString(), function () { });
                        } else {
                            ShowOkMessage("", _language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n', function () { });
                        }
                    }
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    alert("Error while insert");
                });
        }

        $scope.showLoader = false;
    };


    //Amendement Pending Department Details 
    $scope.OpenPendingAmendementDaysPopup = function (SocietyIDEnc) { 
        $scope.PendingAmendemnetDaysDetails = [];
        $scope.TotalAmendementDays = 0;

        return $http.get(SiteUrl + "/ApplicationFlow/GetDeptAmendmentPendingDaysDetails", { params: { "SocietyIDEnc": SocietyIDEnc } }).
            success(function (data, status) {
                $scope.PendingAmendemnetDaysDetails = data;             
                angular.forEach($scope.PendingAmendemnetDaysDetails, function (item) {
                    $scope.TotalAmendementDays += Number(item.Days) || 0;
                });                
                $('#PendingAmendementDaysModal').modal('show');
            }).
            error(function (data, status) {
                $scope.PendingAmendemnetDaysDetails = [];
            });
    };

    $scope.CertificateGenerate = function (SocietyIDEnc) {

        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/ApplicationFlow/CertificateGenerate/?SocietyIDEnc=" + SocietyIDEnc).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n');
                location.reload();
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                location.reload();
            });
    }
    $scope.BankOpeningCertificateGenerate = function (SocietyIDEnc) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/ApplicationFlow/BankOpeningCertificateGenerate/?SocietyIDEnc=" + SocietyIDEnc).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n');
                location.reload();
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                location.reload();
            });
    }
    $scope.VerifyOTP = function () {
        if ($scope.ApplicationFlowMaster.OTP != '') {
            $(".se-pre-con").fadeIn("slow");
            if ($scope.ApplicationFlowMaster.StatusId == 14) {
                $scope.saveDataInDB();
            } else {
               // $scope.IsSocietyAmendment = false;
                $http.post(SiteUrl + "/ApplicationFlow/VerifyAadhaarOTPForEsing", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": $scope.ApplicationFlowMaster.TXN, "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "StatusId": $scope.ApplicationFlowMaster.StatusId, "Remark": $scope.ApplicationFlowMaster.Remark, "IsAmendment": $scope.IsSocietyAmendment}).
                    success(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        if (data == "Fail,-2") {
                            ShowOkMessage("", "SECRETARY dose not exist in member list", function () {
                                // location.reload();
                            });
                        } else
                            if (data == "Fail" || data == "0" || data == 0) {
                                ShowOkMessage("Error", "Certificate not Generate. There is some problem.Please try after some time or contact on support.", function () { });
                            } else
                                if (data != "Success") {
                                  //  alert(data);
                                }
                                else if (data == "Success") {
                                    alert('Application for Society Registration Approved');
                                    location.reload();
                                }
                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                        location.reload();
                    });
            }
        }
        else {
            alert(_language == 'en-us' ? 'Please enter OTP\n' : 'कृपया OTP चुने|\n');
        }
    };

    // on page load
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0") {
                    $("#ddrDistrictId").hide();//("disabled", "disabled");
                }
                // $scope.BindFY();

            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };
    $scope.GetDistrictList();

    $scope.GetFlowProcessStatus = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FLOWPROCESSSTATUS&MasterCode=0").
            success(function (data, status) {
                $scope.StatusList = data;
                if ($("#hdnDistrictId").val()!= "0") {
                    $("#ddrStatus").hide();//("disabled", "disabled");
                }
                // $scope.BindFY();

            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };
    $scope.GetFlowProcessStatus();
    $scope.GetCurrentUserId = function () {
        $http.get(SiteUrl + "/ApplicationFlow/GetCurrentUserId").
            success(function (data, status) {
                $scope.BaseUserId = data;
             //   alert($scope.BaseUserId);
            }).
            error(function (data, status) {
                $scope.BaseUserId = '';
            });
        
    };

    $scope.SocietyProceesStatusList = function (StatusId, ActId, SocietyIDEnc) {
        $scope.SocietyProceesStatus = [];
        return $http.post(SiteUrl + "/ApplicationFlow/SocietyProceesStatusList", { "StatusId": StatusId, "WorkOnActId": ActId, "SocietyIDEnc": SocietyIDEnc }).
            success(function (data, status) {
                $scope.SocietyProceesStatus = data;
            }).
            error(function (data, status) {
                $scope.SocietyProceesStatus = [];
            });
    };


    $scope.GetAppliFlowList = function () {
        $(".se-pre-con").fadeIn("slow");

        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/ApplicationFlow/GetSocietyAmendment1958FlowList?IsAmendment=" + ($scope.IsAmendment == undefined ? false : $scope.IsAmendment), JSON.stringify({ Details: jsonParameters })).
            //$http.get(SiteUrl + "/ApplicationFlow/GetSocietyAppliFlowList").
            success(function (responcedata, status) {
                var data = JSON.parse(responcedata.DataList);
                //var radioValue = $("input[name='gender']:checked").val();
                //if (radioValue == 'R') {
                //    $scope.ApplicationFlowList = data.filter(function (x) { return x.StatusId == 6 || x.StatusId == 13 || x.StatusId == 21 || x.StatusId == 16 || x.StatusId == 18 || x.StatusId == 34 || x.StatusId == 29 }); // Rejected(6,13,21) AutoClose(16,18,34,29)
                //}
                //else if (radioValue == 'A') {
                //    $scope.ApplicationFlowList = data.filter(function (x) { return x.StatusId == 17 || x.StatusId == 26 }); // Approved(17,26)
                //}
                //else if (radioValue == 'O') {
                //    $scope.ApplicationFlowList = data.filter(function (x) { return x.StatusId == 5 || x.StatusId == 10 || x.StatusId == 20 || x.StatusId == 27 }); // Objection(5,10,20,27)
                //}
                //else if (radioValue == 'PL') {
                //    $scope.ApplicationFlowList = data.filter(function (x) { return x.StatusId == 1 }); // Partial Submited
                //}
                //else if (radioValue == 'PA') {
                //    $scope.ApplicationFlowList = data.filter(function (x) { return x.StatusId != 1 && x.StatusId != 5 && x.StatusId != 10 && x.StatusId != 20 && x.StatusId != 27 && x.StatusId != 6 && x.StatusId != 13 && x.StatusId != 21 && x.StatusId != 16 && x.StatusId != 18 && x.StatusId != 34 && x.StatusId != 29 && x.StatusId != 17 && x.StatusId != 26 }); // Pending
                //}
                //else {
                //    $scope.ApplicationFlowList = data.filter(function (x) { return x.UserId == $scope.BaseUserId }); // Panding Self
                //}
                //var dateCreated = Date.parse(created);
                

                //
                //var lastWeek = Date.today().add(-7).days();
                //var 
                //if ($scope.ApplicationFlowList.ActionDate)
                //{
                //    "<div style='color:red'>" + $scope.ApplicationFlowList.ActionDate + "</div";
                //}
                $scope.ApplicationFlowList = data;
                $rootScope.DataPagingDetails = responcedata.DataPaging;
                $rootScope.SetDataPaging();

                $scope.colsort = "";
                $scope.reverseSort = false;
                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $(".se-pre-con").fadeOut("slow");
                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                    }
                    return '';
                };
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });

    };




    $scope.getColor = function (AppFlow) {
        var color = "red";
        var datediffrent = "30";
        //alert(AppFlow.DateDiff);
        if (AppFlow.DateDiff >= datediffrent) {
            color = 'red';
        }

        return color;
    };


    $scope.gettextColor = function (AppFlow) {
        var textcolor = "white";
        var datediffrent = "30";
        //alert(AppFlow.DateDiff);
        if (AppFlow.DateDiff >= datediffrent) {
            textcolor = 'white';
        }

        return textcolor;
    };

    $scope.GetApplicationFlowSociety = function (SocietyIDEnc) {
        $scope.showLoader = true;
        //$(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/ApplicationFlow/GetApplicationFlowSociety/?SocietyIDEnc=" + SocietyIDEnc).
            success(function (data, status) {

                $scope.ApplicationFlowSociety = data;
                $scope.colsort = "";
                $scope.reverseSort = false;
                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };
                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                    }
                    return '';
                };
                // $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data1, status) {
                //$(".se-pre-con").fadeOut("slow");
            });
        $scope.showLoader = true;
    };
    $scope.CurrentStatusId = 0;
    $scope.update = function (item) {        
        $scope.IsSocietyAmendment = item.IsAmendment;
       // alert(item.UserId);
        $scope.showLoader = true;
        $(".se-pre-con").fadeIn("slow");
        $scope.GetApplicationFlowSociety(item.SocietyIDEnc).then(function () {
            $scope.SocietyProceesStatusList(item.StatusId, item.ActId, item.SocietyIDEnc).then(function () {
                $(".se-pre-con").fadeOut("slow");
                $("#myModal").modal('show');
            });
        });

        $scope.CurrentStatusId = item.StatusId;
        $scope.ApplicationFlowMaster.SocietyIDEnc = item.SocietyIDEnc;
        $scope.ApplicationFlowMaster.ActId = item.ActId;
        $scope.ApplicationFlowMaster.SocietyName = item.SocietyName;
        $scope.ApplicationFlowMaster.RegistrationNo = item.RegistrationNo;
        $scope.ApplicationFlowMaster.UserId = item.UserId;
       
        $scope.showLoader = false;

    };

    $scope.updateNotAssign = function (item) {
        $scope.IsSocietyAmendment = item.IsAmendment;
        $scope.showLoader = true;
        $(".se-pre-con").fadeIn("slow");
        $scope.GetApplicationFlowSociety(item.SocietyIDEnc).then(function () {
            $scope.GetUserForSocietyAssign(item.SocietyIDEnc).then(function () {
                $(".se-pre-con").fadeOut("slow");
                $("#myModalNotAssign").modal('show');
            });
        });

        $scope.CurrentStatusId = item.StatusId;
        $scope.ApplicationFlowMaster.SocietyIDEnc = item.SocietyIDEnc;
        $scope.ApplicationFlowMaster.ActId = item.ActId;
        $scope.ApplicationFlowMaster.SocietyName = item.SocietyName;
        $scope.ApplicationFlowMaster.RegistrationNo = item.RegistrationNo;
        $scope.ApplicationFlowMaster.UserId = item.UserId;
        $scope.showLoader = false;

    };

    $scope.OfficerNotAssignData = [];
    $scope.DivTextForNotMaaped = "";
    $scope.GetUserForSocietyAssign = function (SocietyIDEnc) {
        $scope.OfficerNotAssignData = [];
        $scope.DivTextForNotMaaped = "";
        return $http.post(SiteUrl + "/ApplicationFlow/GetUserForSocietyAssign", { "SocietyID": SocietyIDEnc }).
            success(function (data, status) {
                $scope.OfficerNotAssignData = JSON.parse(data.DataList);
                if ($scope.OfficerNotAssignData.length <= 0) {
                    $scope.DivTextForNotMaaped = "* Officer not found for this type of society. Please make sure that officer of your district assigned the Act " + ($scope.ApplicationFlowMaster.ActId == 1 ? "1958" : $scope.ApplicationFlowMaster.ActId == 3 ? "2001" : "2005") + ""
                } else {
                    $scope.DivTextForNotMaaped = "";                }
            }).
            error(function (data, status) {
                $scope.OfficerNotAssignData = [];
            });
    };

    $scope.SavePermanentSocietyData = function () {
        $scope.OfficerNotAssignData = [];
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/Society/SocietyAssignOfficerSave", { "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data == "-1") {
                    ShowOkMessage("", "* Officer not found for this type of society. Please make sure that officer of your district assigned the Act " + ($scope.ApplicationFlowMaster.ActId == 1 ? "1958" : $scope.ApplicationFlowMaster.ActId == 3?"2001":"2005")+"", function () { });
                } else {
                    ShowOkMessage("", "Society assigned successfully", function () { });
                }
            }).
            error(function (data, status) {
                $scope.OfficerNotAssignData = [];
            });
    };

    $scope.GetRoleList = function () {
        $scope.RoleList = [];
        $scope.RoleId = '0';
        $scope.OfficerData = [];
        if ($scope.ApplicationFlowMaster.StatusId == 3 || $scope.ApplicationFlowMaster.StatusId == 9 || $scope.ApplicationFlowMaster.StatusId == 12 || $scope.ApplicationFlowMaster.StatusId == 19 || $scope.ApplicationFlowMaster.StatusId == 23 || $scope.ApplicationFlowMaster.StatusId == 35 || $scope.ApplicationFlowMaster.StatusId == 36 || $scope.ApplicationFlowMaster.StatusId == 1058) {
            $http.post(SiteUrl + "/ApplicationFlow/GetRoleListForApplicationStatus", { "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "NextStatusId": $scope.ApplicationFlowMaster.StatusId, "CurrentStatusId": $scope.CurrentStatusId }).
                success(function (data, status) {
                    $scope.RoleList = data;
                    if (data.length > 0) {
                        $("#ddlRole").attr("disabled", 'disabled');
                        $scope.RoleId = data[0].RoleID;
                        $scope.GetOfficer();
                    }
                }).
                error(function (data, status) {
                    $scope.RoleList = [];
                });
        }
    };

    $scope.GetOfficer = function () {
        $scope.OfficerData = [];
        if ($scope.ApplicationFlowMaster.StatusId == 3 || $scope.ApplicationFlowMaster.StatusId == 9 || $scope.ApplicationFlowMaster.StatusId == 12 || $scope.ApplicationFlowMaster.StatusId == 19 || $scope.ApplicationFlowMaster.StatusId == 23 || $scope.ApplicationFlowMaster.StatusId == 35 || $scope.ApplicationFlowMaster.StatusId == 36 || $scope.ApplicationFlowMaster.StatusId == 1058) {
            $http.post(SiteUrl + "/ApplicationFlow/GetOfficerBySocietyIDEnc", { "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "NextStatusId": $scope.ApplicationFlowMaster.StatusId, "CurrentStatusId": $scope.CurrentStatusId, "RoleId": $scope.RoleId }).
                success(function (data, status) {
                    $scope.ApplicationFlowMaster.OfficeId = '0';
                    $scope.OfficerData = data;
                    if (data.length > 0) {
                        $("#ddlOffice").attr("disabled", 'disabled');
                        $scope.ApplicationFlowMaster.OfficeId = data[0].userid;
                    }
                }).
                error(function (data, status) {
                    $scope.OfficerData = [];
                });
        }
    };

    $scope.SocietyView = function (AppFlow) {
        //if (AppFlow.IsAmendment) {
        //    $scope.showLoader = true;
        //    window.location = "../ApplicationFlow/SocietyActionViewAmendment?SocietyIDEnc=" + AppFlow.SocietyIDEnc + "~_ApplicationFlow_ApplicationFlow";
        //    $scope.showLoader = false;
        //} else {
            $scope.showLoader = true;
            window.location = "../ApplicationFlow/SocietyActionView?SocietyIDEnc=" + AppFlow.SocietyIDEnc + "~_ApplicationFlow_ApplicationFlow";
            $scope.showLoader = false;
      //  }
    };

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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
      
        $scope.showLoader = false;
    };

    $scope.SelectRadioBtn = function () {
        var url = window.location.search;
        $scope.statusId = (url.split('=')[1]);
        if ($scope.statusId == undefined) {
            $scope.searchRequest($scope.GetAppliFlowList);
        }
        else if ($scope.statusId == 'PL') {
            $("#zero").prop("checked", true);
            $scope.searchRequest($scope.GetAppliFlowList);
        }
        else if ($scope.statusId == 'TS') {
            $("#five").prop("checked", true);
            $scope.searchRequest($scope.GetAppliFlowList);
        }
        else if ($scope.statusId == 'RS') {
            $("#two").prop("checked", true);
            $scope.searchRequest($scope.GetAppliFlowList);
        }
        else if ($scope.statusId == 'PS') {
            $("#four").prop("checked", true);
            $scope.searchRequest($scope.GetAppliFlowList);
        }
        else if ($scope.statusId == 'UP') {
            $("#four").prop("checked", true);
            $scope.searchRequest($scope.GetAppliFlowList);
        }
        else if ($scope.statusId == 'PP') {
            $("#four").prop("checked", true);
            $scope.searchRequest($scope.GetAppliFlowList);
        }
        else if ($scope.statusId === 'RejS') {
            $("#one").prop("checked", true);
            $scope.searchRequest($scope.GetAppliFlowList);
        }
    }

    $scope.OnPageLoad();
    $scope.SelectRadioBtn();
    $scope.GetCurrentUserId();

    $scope.FileCollection = [];
    $scope.IsFileUpload = function (id) {
        return $scope.FileCollection.filter(function (x) { return x.FileId === id && x.IsFileUpload === true }).length > 0;
    }
    $scope.GetFileUploaded = function (id) {
        try {
            return $scope.FileCollection.filter(function (x) { return x.FileId === id && x.IsFileUpload === true })[0].FileName;
        } catch (ex) {
            return "";
        }
    }


    $scope.RemovalTypeList = [{ CODENAME: 'DEATH', CODENAME_MANGAL: 'DEATH', CODEID: '1' }, { CODENAME: 'REMOVEL', CODENAME_MANGAL: 'REMOVEL', CODEID: '3' }, { CODENAME: 'RESIGNATION', CODENAME_MANGAL: 'RESIGNATION', CODEID: '3' }]

    $scope.AmendmentDdetails = function (item) {
        $(".se-pre-con").fadeIn("slow");
        //return $http.get(SiteUrl + "/ApplicationFlow/GetAmendmentChanges/?SocietyIDEnc=" + item.SocietyIDEnc).
        //    success(function (data, status) {
        //        //$(".se-pre-con").fadeOut("slow");
        //        $("#myAmendmentInfo").modal("show");
        //        $scope.SocietyAmendmentList = JSON.parse(data);
        //        $scope.GetMemberAmendmentBySocietyId(item.SocietyIDEnc);
        //    }).
        //    error(function (data1, status) {
        //        $(".se-pre-con").fadeOut("slow");
        //    });
        //$scope.showLoader = true;
        $scope.GetMemberList(item).then(function () {
            $scope.GetSocietyNameChangeList(item).then(function () {
                $(".se-pre-con").fadeOut("slow");
                $("#myAmendmentInfo").modal("show");
            });
        });
    }

    $scope.SocietyAmendment = {};
    $scope.SocietyRegisteredOffice = {};
    $scope.SocietyNameChangeList = [];
    $scope.GetSocietyNameChangeList = function (item121) {
        //$(".se-pre-con").fadeIn("slow");



        var SocietyIdEnc = item121.SocietyIDEnc;
        // alert(SocietyIdEnc);
        return $http.get("../Citizen/GetSocietyNameChangeList?SocietyIdEnc=" + SocietyIdEnc + '&AmendmentId=' + item121.AmendmentId).
            success(function (data, status) {
                var dataAll = JSON.parse(data.DataList);
                $scope.FileCollection = [];
                $scope.SocietyNameChangeList = dataAll.Table;
                var documents = dataAll.Table1;
                $scope.allDisable = true;
                $scope.FileCollection = [];

                documents.forEach(function (item, index) {
                    $scope.FileCollection.push({ 'Id': item.DocumentType, 'FileType': '', 'File': item.DocumentURL, 'Title': '', FileName: item.DocumentURL, FileId: item.DocumentType, IsFileUpload: true });
                });

                $scope.allDisable = true;

                $scope.SocietyAmendment.ChangeByeLaws = $scope.SocietyNameChangeList[0].AmendmentType.indexOf('1') > -1 ? true : false;
                $scope.SocietyAmendment.ChangeName = $scope.SocietyNameChangeList[0].AmendmentType.indexOf('2') > -1 ? true : false;
                $scope.SocietyAmendment.ChaneAddress = $scope.SocietyNameChangeList[0].AmendmentType.indexOf('3') > -1 ? true : false;
                $scope.SocietyAmendment.ChaneExecutiveMember = $scope.SocietyNameChangeList[0].AmendmentType.indexOf('4') > -1 ? true : false;

                //$scope.FileCollection = [];

                $scope.checkNameChange();


                //if ($scope.SocietyNameChangeList[0].flResoloutionNoticeURL != '') {
                //    $scope.FileCollection.push({ 'Id': 'flResoloutionNotice', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flResoloutionNoticeURL, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flResoloutionNoticeURL, FileId: 'flResoloutionNotice', IsFileUpload: true });
                //}
                //if ($scope.SocietyNameChangeList[0].flAttendanceSheetURL != '') {
                //    $scope.FileCollection.push({ 'Id': 'flAttendanceSheet', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flAttendanceSheetURL, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flAttendanceSheetURL, FileId: 'flAttendanceSheet', IsFileUpload: true });
                //}
                //if ($scope.SocietyNameChangeList[0].ByeLawsFileURL != '') {
                //    $scope.FileCollection.push({ 'Id': 'flBylawsFileName', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].ByeLawsFileURL, 'Title': '', FileName: $scope.SocietyNameChangeList[0].ByeLawsFileURL, FileId: 'flBylawsFileName', IsFileUpload: true });
                //}
                //if ($scope.SocietyNameChangeList[0].flByLowComparativeDocumentURL != '') {
                //    $scope.FileCollection.push({ 'Id': 'flByLowComparativeDocument', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flByLowComparativeDocumentURL, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flByLowComparativeDocumentURL, FileId: 'flByLowComparativeDocument', IsFileUpload: true });
                //}
                //if ($scope.SocietyNameChangeList[0].flAGMDeliveryProofURL != '') {
                //    $scope.FileCollection.push({ 'Id': 'flAGMDeliveryProof', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flAGMDeliveryProofURL, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flAGMDeliveryProofURL, FileId: 'flAGMDeliveryProof', IsFileUpload: true });
                //}
                //if ($scope.SocietyNameChangeList[0].flSGMNoticeURL != '') {
                //    $scope.FileCollection.push({ 'Id': 'flSGMNotice', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flSGMNoticeURL, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flSGMNoticeURL, FileId: 'flSGMNotice', IsFileUpload: true });
                //}
                //if ($scope.SocietyNameChangeList[0].flSGMAttendanceSheetURL != '') {
                //    $scope.FileCollection.push({ 'Id': 'flSGMAttendanceSheet', 'FileType': '', 'File': $scope.SocietyNameChangeList[0].flSGMAttendanceSheetURL, 'Title': '', FileName: $scope.SocietyNameChangeList[0].flSGMAttendanceSheetURL, FileId: 'flSGMAttendanceSheet', IsFileUpload: true });
                //}

                if ($scope.SocietyNameChangeList[0].AmendmentType.indexOf('3') > -1) {

                    $scope.SocietyRegisteredOffice.STATE_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].STATE_ENG : $scope.SocietyNameChangeList[0].STATE_MANGAL;
                    $scope.SocietyRegisteredOffice.HouseBuildingNo = $scope.SocietyNameChangeList[0].HouseBuildingNo;
                    $scope.SocietyRegisteredOffice.LaneStreetName = $scope.SocietyNameChangeList[0].LaneStreetName;
                    $scope.SocietyRegisteredOffice.Locality = $scope.SocietyNameChangeList[0].Locality;
                    $scope.SocietyRegisteredOffice.DIVISION_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].DIVISION_ENG : $scope.SocietyNameChangeList[0].DIVISION_MANGAL;
                    $scope.SocietyRegisteredOffice.DISTRICT_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].DISTRICT_ENG : $scope.SocietyNameChangeList[0].DISTRICT_MANGAL;
                    $scope.SocietyRegisteredOffice.PinCode = $scope.SocietyNameChangeList[0].PinCode;

                    $scope.SocietyRegisteredOffice.CITY_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].CITY_ENG : $scope.SocietyNameChangeList[0].CITY_MANGAL;
                    $scope.SocietyRegisteredOffice.WARD_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].WARD_ENG : $scope.SocietyNameChangeList[0].WARD_MANGAL;


                    $scope.SocietyRegisteredOffice.BLOCK_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].BLOCK_ENG : $scope.SocietyNameChangeList[0].BLOCK_MANGAL;
                    $scope.SocietyRegisteredOffice.GP_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].GP_ENG : $scope.SocietyNameChangeList[0].GP_MANGAL;
                    $scope.SocietyRegisteredOffice.VILLAGE_ID = _language == 'en-us' ? $scope.SocietyNameChangeList[0].VILLAGE_ENG : $scope.SocietyNameChangeList[0].VILLAGE_MANGAL;



                }


                // $(".se-pre-con").fadeOut("slow");
                //alert(JSON.stringify($scope.ByLawsListData));
            }).
            error(function (data, status) {
                $scope.ByLawsListData = [];
                //$(".se-pre-con").fadeOut("slow");
                alert('Error while fetching society list data.');
            });

    };
    $scope.GetMemberList = function (item) {
        $scope.ExecutiveMemberAmendmentList = [];
        $scope.GeneralMemberAmendmentList = [];
        return $scope.GetMemberAmendmentBySocietyId(item);
    };
    $scope.ExecutiveMemberAmendmentList = [];
    $scope.GeneralMemberAmendmentList = [];
    $scope.GetMemberAmendmentBySocietyId = function (item121) {
        //var Datasplit = SocietyIDEnc;

        var SocietyIdEnc = item121.SocietyIDEnc;
        var ActId = item121.ActId;
        $scope.ActId = ActId;
        $scope.SocietyDirectory = item121.SocietyDirectory;
        $scope.SocietyIdEnc = SocietyIdEnc;
        if (ActId == 1) {
            $scope.MinimMemeber = 7;
        }
        else if (ActId == 3) {
            $scope.MinimMemeber = 15;
        }
        else if (ActId == 7) {
            $scope.MinimMemeber = 5;
        }
        $(".se-pre-con").fadeIn("slow");
        //alert(SocietyIdEnc);
        //$http.get("../Citizen/GetAGM_BODMList?SocietyIdEnc=" + SocietyIdEnc).
        return $http.post(SiteUrl + "/Society/GetMemberAmendmentBySocietyId", { "SocietyId": SocietyIdEnc, "AmendmentId": item121.AmendmentId}).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.ExecutiveMemberAmendmentList = [];
                //$scope.MemberAmendmentList = data;
                data.forEach(function (item, index) {
                    if (item.MEMBERTYPE == 'ELECTEDMEMBERS') {
                        if ($scope.ExecutiveMemberAmendmentList.filter(function (x) { return x.Name == item.Name && x.FatherName == item.FatherName && x.Aadh == item.Aadh }).length <= 0) {
                            if (item.RequestType == 1) {
                                item.IsDelete = true;
                            }
                            $scope.ExecutiveMemberAmendmentList.push(item);
                        }
                    } else {
                        if ($scope.GeneralMemberAmendmentList.filter(function (x) { return x.Name == item.Name && x.FatherName == item.FatherName && x.Aadh == item.Aadh }).length <= 0) {
                            if (item.RequestType == 1) {
                                item.IsDelete = true;
                            }
                            $scope.GeneralMemberAmendmentList.push(item);
                        }
                    }
                });
                //$scope.MemberAmendmentList.forEach(function (item, indx) {
                //    if (item.RequestType == 1) {
                //        item.IsDelete = true;
                //    }
                //});
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                ALLMEMBERS = [];
                alert('Error while fetching society list data.');
            });

    };

    $scope.AmendmentDocuments = [];
    $scope.SocietyAmendment = { ChangeName: false };
    $scope.AmendmentDocuments.push({ Id: 2, Name: "Board meeting resolution Notice with delivery proof", FileId: "Board meeting resolution Notice with delivery proof", IsShow: true });
    $scope.AmendmentDocuments.push({ Id: 3, Name: "Board meeting resolution MOM And Attendance Sheet", FileId: "Board meeting resolution MOM And Attendance Sheet", IsShow: true });
    $scope.AmendmentDocuments.push({ Id: 4, Name: "1st SGM(Special General meeting) Notice with delivery proof", FileId: "1st SGM(Special General meeting) Notice with delivery proof", IsShow: true });
    $scope.AmendmentDocuments.push({ Id: 5, Name: "1st SGM(Special General meeting) MOM And Attendance Sheet", FileId: "1st SGM(Special General meeting) MOM And Attendance Sheet", IsShow: true });
    $scope.AmendmentDocuments.push({ Id: 6, Name: "2nd SGM(Special General meeting) Notice with delivery proof", FileId: "2nd SGM(Special General meeting) Notice with delivery proof", IsShow: true });
    $scope.AmendmentDocuments.push({ Id: 7, Name: "2nd SGM(Special General meeting) MOM And Attendance Sheet", FileId: "2nd SGM(Special General meeting) MOM And Attendance Sheet", IsShow: true });
    $scope.AmendmentDocuments.push({ Id: 8, Name: "Comparative Document Of By-Laws", FileId: "Comparative Document Of By-Laws", IsShow: true });
     // $scope.AmendmentDocuments.push({ Id: 8, Name: "Original Certificate To be deposited in Registrar office", FileId: "Original_Certificate_To_be_deposited_in_Registrar_office", IsShow:true });
    $scope.checkNameChange = function () {
        $scope.AmendmentDocuments = [];
        if ($scope.SocietyAmendment.ChangeName) {
            $scope.AmendmentDocuments.push({ Id: 1, Name: "Notice to Registrar Signed by Secretary & Seven Members", FileId: "Notice to Registrar Signed by Secretary & Seven Members", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 2, Name: "Board meeting resolution Notice with delivery proof", FileId: "Board meeting resolution Notice with delivery proof", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 3, Name: "Board meeting resolution MOM And Attendance Sheet", FileId: "Board meeting resolution MOM And Attendance Sheet", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 4, Name: "1st SGM(Special General meeting) Notice with delivery proof", FileId: "1st SGM(Special General meeting) Notice with delivery proof", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 5, Name: "1st SGM(Special General meeting) MOM And Attendance Sheet", FileId: "1st SGM(Special General meeting) MOM And Attendance Sheet", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 6, Name: "2nd SGM(Special General meeting) Notice with delivery proof", FileId: "2nd SGM(Special General meeting) Notice with delivery proof", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 7, Name: "2nd SGM(Special General meeting) MOM And Attendance Sheet", FileId: "2nd SGM(Special General meeting) MOM And Attendance Sheet", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 8, Name: "Comparative Document Of By-Laws", FileId: "Comparative Document Of By-Laws", IsShow: true });

        } else if ($scope.SocietyAmendment.AmendmentSection == 4) {
            // $scope.AmendmentDocuments.push({ Id: 1, Name: "Notice to Registrar Signed by Secretary & Seven Members", FileId: "Notice_to_Registrar_Signed_by_Secretary_Seven_Members", IsShow: false});
            $scope.AmendmentDocuments.push({ Id: 1, Name: "Board meeting resolution Notice with delivery proof", FileId: "Board meeting resolution Notice with delivery proof", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 2, Name: "Board meeting resolution MOM And Attendance Sheet", FileId: "Board meeting resolution MOM And Attendance Sheet", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 3, Name: "AGM(Annual General meeting) Notice with delivery proof", FileId: "AGM(Annual General meeting) Notice with delivery proof", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 4, Name: "AGM(Annual General meeting) MOM And Attendance Sheet", FileId: "AGM(Annual General meeting) MOM And Attendance Sheet", IsShow: true });

        } else {
            // $scope.AmendmentDocuments.push({ Id: 1, Name: "Notice to Registrar Signed by Secretary & Seven Members", FileId: "Notice_to_Registrar_Signed_by_Secretary_Seven_Members", IsShow: false});
            $scope.AmendmentDocuments.push({ Id: 2, Name: "Board meeting resolution Notice with delivery proof", FileId: "Board meeting resolution Notice with delivery proof", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 3, Name: "Board meeting resolution MOM And Attendance Sheet", FileId: "Board meeting resolution MOM And Attendance Sheet", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 4, Name: "1st SGM(Special General meeting) Notice with delivery proof", FileId: "1st SGM(Special General meeting) Notice with delivery proof", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 5, Name: "1st SGM(Special General meeting) MOM And Attendance Sheet", FileId: "1st SGM(Special General meeting) MOM And Attendance Sheet", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 6, Name: "2nd SGM(Special General meeting) Notice with delivery proof", FileId: "2nd SGM(Special General meeting) Notice with delivery proof", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 7, Name: "2nd SGM(Special General meeting) MOM And Attendance Sheet", FileId: "2nd SGM(Special General meeting) MOM And Attendance Sheet", IsShow: true });
            $scope.AmendmentDocuments.push({ Id: 8, Name: "Comparative Document Of By-Laws", FileId: "Comparative Document Of By-Laws", IsShow: true });
        }

    }
});






