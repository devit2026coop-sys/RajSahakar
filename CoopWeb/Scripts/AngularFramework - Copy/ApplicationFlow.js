
var MasterMod = angular.module('ApplicationFlowModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ApplicationFlowCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
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
            if ($scope.ApplicationFlowMaster.StatusId != 17) {
                if ($scope.ApplicationFlowMaster.StatusId == 5 || $scope.ApplicationFlowMaster.StatusId == 10 || $scope.ApplicationFlowMaster.StatusId == 20) {
                    if ($scope.ObjectionFlagBasic == false && $scope.ObjectionFlagAdmin == false && $scope.ObjectionFlagOffice == false) {
                        alert(_language == 'en-us' ? 'Please Select Objection Belong To.\n' : 'कृप्या आपत्ति संबंधित है चुनें|\n');
                    }
                    else {
                        $(".se-pre-con").fadeIn("slow");
                        $http.post("../ApplicationFlow/SaveSocietyObjectionBelong", { "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc,"ObjectionFlagBasic": $scope.ObjectionFlagBasic, "ObjectionFlagAdmin": $scope.ObjectionFlagAdmin, "ObjectionFlagOffice": $scope.ObjectionFlagOffice}).
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
                            location.reload();
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
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/ApplicationFlow/SaveSocietyApplicationFlowStatus", { "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "Remark": $scope.ApplicationFlowMaster.Remark, "StatusId": $scope.ApplicationFlowMaster.StatusId, "UserId": $scope.ApplicationFlowMaster.UserId }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data == 1) {

                    if ($scope.ApplicationFlowMaster.StatusId == 14) {
                        $scope.BankOpeningCertificateGenerate($scope.ApplicationFlowMaster.SocietyIDEnc);
                    }
                    else if ($scope.ApplicationFlowMaster.StatusId != 17) {
                        alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n');
                        location.reload();
                        //window.location = '../ApplicationFlow/ApplicationFlow';
                    }
                    else if ($scope.ApplicationFlowMaster.StatusId == 17) {
                        $scope.CertificateGenerate($scope.ApplicationFlowMaster.SocietyIDEnc);
                    }
                    else {
                        alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n');
                        location.reload();
                        //window.location = '../ApplicationFlow/ApplicationFlow';
                    }

                }
                else {
                    $(".se-pre-con").fadeOut("slow");
                    alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                    window.location = '/ApplicationFlow/ApplicationFlow';
                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while insert");
            });
        $scope.showLoader = false;
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
            $http.post(SiteUrl + "/ApplicationFlow/VerifyAadhaarOTPForEsing", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": $scope.ApplicationFlowMaster.TXN, "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "StatusId": $scope.ApplicationFlowMaster.StatusId, "Remark": $scope.ApplicationFlowMaster.Remark }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data != "Success") {
                        alert(data);
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
        else {
            alert(_language == 'en-us' ? 'Please enter OTP\n' : 'कृपया OTP चुने|\n');
        }
    };

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
        $http.post(SiteUrl + "/ApplicationFlow/SocietyProceesStatusList", { "StatusId": StatusId, "WorkOnActId": ActId, "SocietyIDEnc": SocietyIDEnc }).
            success(function (data, status) {
                $scope.SocietyProceesStatus = data;
            }).
            error(function (data, status) {
                $scope.SocietyProceesStatus = [];
            });
    };


    $scope.GetAppliFlowList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/ApplicationFlow/GetSocietyAppliFlowList").
            success(function (data, status) {
                var radioValue = $("input[name='gender']:checked").val();
                if (radioValue == 'R') {
                    $scope.ApplicationFlowList = data.filter(x => x.StatusId == 6 || x.StatusId == 13 || x.StatusId == 21 || x.StatusId == 16 || x.StatusId == 18 || x.StatusId == 34 || x.StatusId == 29); // Rejected(6,13,21) AutoClose(16,18,34,29)
                }
                else if (radioValue == 'A') {
                    $scope.ApplicationFlowList = data.filter(x => x.StatusId == 17 || x.StatusId == 26); // Approved(17,26)
                }
                else if (radioValue == 'O') {
                    $scope.ApplicationFlowList = data.filter(x => x.StatusId == 5 || x.StatusId == 10 || x.StatusId == 20 || x.StatusId == 27); // Objection(5,10,20,27)
                }
                else if (radioValue == 'PL') {
                    $scope.ApplicationFlowList = data.filter(x => x.StatusId == 1); // Partial Submited
                }
                else if (radioValue == 'PA') {
                    $scope.ApplicationFlowList = data.filter(x => x.StatusId != 1 && x.StatusId != 5 && x.StatusId != 10 && x.StatusId != 20 && x.StatusId != 27 && x.StatusId != 6 && x.StatusId != 13 && x.StatusId != 21 && x.StatusId != 16 && x.StatusId != 18 && x.StatusId != 34 && x.StatusId != 29 && x.StatusId != 17 && x.StatusId != 26); // Pending
                }
                else {
                    $scope.ApplicationFlowList = data.filter(x => x.UserId == $scope.BaseUserId); // Panding Self
                }
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
            }).
            error(function (data1, status) {

            });
        $(".se-pre-con").fadeOut("slow");
    };

    $scope.GetApplicationFlowSociety = function (SocietyIDEnc) {
        $scope.showLoader = true;
        $http.get(SiteUrl + "/ApplicationFlow/GetApplicationFlowSociety/?SocietyIDEnc=" + SocietyIDEnc).
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
            }).
            error(function (data1, status) {

            });
        $scope.showLoader = true;
    };
    $scope.CurrentStatusId = 0;
    $scope.update = function (item) {
        $scope.showLoader = true;
        $scope.GetApplicationFlowSociety(item.SocietyIDEnc);
        $scope.SocietyProceesStatusList(item.StatusId, item.ActId, item.SocietyIDEnc);
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

    $scope.SocietyView = function (SocietyIDEnc) {
        $scope.showLoader = true;
        window.location = "../ApplicationFlow/SocietyActionView?SocietyIDEnc=" + SocietyIDEnc + "~_ApplicationFlow_ApplicationFlow";
        $scope.showLoader = false;
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
        $scope.GetCurrentUserId();
        $scope.showLoader = false;
    };

    $scope.SelectRadioBtn = function () {
        var url = window.location.search;
        $scope.statusId = (url.split('=')[1]);
        if ($scope.statusId == undefined) {
            $scope.GetAppliFlowList();
        }
        else if ($scope.statusId == 'PL') {
            $("#zero").prop("checked", true);
            $scope.GetAppliFlowList();
        }
        else if ($scope.statusId == 'TS') {
            $("#five").prop("checked", true);
            $scope.GetAppliFlowList();
        }
        else if ($scope.statusId == 'RS') {
            $("#two").prop("checked", true);
            $scope.GetAppliFlowList();
        }
        else if ($scope.statusId == 'PS') {
            $("#four").prop("checked", true);
            $scope.GetAppliFlowList();
        }
        else if ($scope.statusId == 'UP') {
            $("#four").prop("checked", true);
            $scope.GetAppliFlowList();
        }
        else if ($scope.statusId == 'PP') {
            $("#four").prop("checked", true);
            $scope.GetAppliFlowList();
        }
        else if ($scope.statusId === 'RejS') {
            $("#one").prop("checked", true);
            $scope.GetAppliFlowList();
        }
    }

    $scope.OnPageLoad();
    $scope.SelectRadioBtn();


});






