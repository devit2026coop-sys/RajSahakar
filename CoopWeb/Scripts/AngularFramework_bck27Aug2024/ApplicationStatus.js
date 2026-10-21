
var MasterMod = angular.module('ApplicationStatusModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ApplicationStatusCont", function ($scope, $rootScope, $http) {
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

    $scope.ApplicationList = [];
    $scope.GetApplication = function () {
        if ($("#txtSearch").val() == '') {
            ShowOkMessage("", "Please Enter Registration Number/BRN Number/Society Name", function () { });
        } else {
            $(".blockUI").fadeIn("slow");
            return $http.post("/VerifyCertificate/GetCertificate", JSON.stringify({ ApplicationNo: $scope.Filter.SearchText })).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (data.DataList.length <= 0) {
                        $scope.ApplicationList = [];
                        ShowOkMessage("", "No Record Found", function () { });
                    } else {
                        $scope.ApplicationList =data.DataList;
                    }
                }).
                error(function (data, status) {
                    $scope.CertificateList = [];
                    $(".blockUI").fadeOut("slow");
                });
        }
    }


    $scope.SocietyView = function (AppFlow) {       
            $scope.showLoader = true;
        window.location = "../ApplicationFlow/SocietyApplicationStatus?SocietyIDEnc=" + AppFlow.SocietyIDInc + "~_ApplicationFlow_ApplicationFlow";
            $scope.showLoader = false;        
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
            if ($scope.ApplicationFlowMaster.StatusId != 17 && $scope.ApplicationFlowMaster.StatusId != 14) {
                if ($scope.ApplicationFlowMaster.StatusId == 5 || $scope.ApplicationFlowMaster.StatusId == 10 || $scope.ApplicationFlowMaster.StatusId == 20) {
                    if ($scope.ObjectionFlagBasic == false && $scope.ObjectionFlagAdmin == false && $scope.ObjectionFlagOffice == false) {
                        alert(_language == 'en-us' ? 'Please Select Objection Belong To.\n' : 'कृप्या आपत्ति संबंधित है चुनें|\n');
                    }
                    else {
                        $(".se-pre-con").fadeIn("slow");
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
                $(".se-pre-con").fadeIn("slow");
                $http.get("../ApplicationFlow/GenerateAadhaarOTPForEsing").
                    success(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        if (data.indexOf('NO#') >= 0) {
                            alert("Due to some issue OTP not send");
                            //location.reload();
                            //window.location = '../ApplicationFlow/ApplicationFlow';
                        } else {
                            if (data != "LOGOUT") {
                                // alert(data);
                                $scope.ApplicationFlowMaster.TXN = data;
                                alert(_language == 'en-us' ? 'OTP has been sent to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n');
                                $scope.ShowOTPDiv = true;
                            }
                            else {
                                $(".se-pre-con").fadeOut("slow");
                                window.location = 'https://sso.rajasthan.gov.in/signin';
                            }
                        }
                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                    });
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
                $http.post(SiteUrl + "/ApplicationFlow/SaveSocietyApplicationFlowStatus", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": $scope.ApplicationFlowMaster.TXN, "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "Remark": $scope.ApplicationFlowMaster.Remark, "StatusId": $scope.ApplicationFlowMaster.StatusId, "UserId": $scope.ApplicationFlowMaster.UserId }).
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
        } else

        {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/ApplicationFlow/SaveSocietyApplicationFlowStatus", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": $scope.ApplicationFlowMaster.TXN, "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "Remark": $scope.ApplicationFlowMaster.Remark, "StatusId": $scope.ApplicationFlowMaster.StatusId, "UserId": $scope.ApplicationFlowMaster.UserId }).
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

    $scope.ChangeStatus = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/ApplicationFlow/SaveSocietyStatus", { "SocietyIDEnc": $scope.ApplicationFlowSociety[0].SocietyIDEnc, "StatusId": $scope.ApplicationFlowMaster.StatusId, "RegistrationNo": $scope.ApplicationFlowSociety[0].RegistrationNo }).
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
  

    // on page load

    $scope.GetCurrentUserId = function () {
        $http.get(SiteUrl + "/ApplicationFlow/GetCurrentUserId").
            success(function (data, status) {
                $scope.BaseUserId = data;
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

    $scope.ShowsatatusDetails = function () {
        //$("#ReviewAssignedAuditorModal").modal("show");
        alert(JSON.stringify($scope.ApplicationFlowSociety));
    }
    $scope.SaveUpdateStatus = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.post("../ApplicationFlow/SaveSocietyObjectionBelong", { "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "ObjectionFlagBasic": $scope.ObjectionFlagBasic, "ObjectionFlagAdmin": $scope.ObjectionFlagAdmin, "ObjectionFlagOffice": $scope.ObjectionFlagOffice }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.saveDataInDB();
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
            });
        $(".se-pre-con").fadeIn("slow");
    }

    $scope.GetAppliFlowList = function () {
        $(".se-pre-con").fadeIn("slow");

        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/ApplicationFlow/GetSocietyAppliStatusList", JSON.stringify({ Details: jsonParameters })).
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



    $scope.OnPageLoad = function () {
       // $(".se-pre-con").fadeIn("slow");
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
        $scope.GetCurrentUserId();
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
    //$scope.SelectRadioBtn();


});






