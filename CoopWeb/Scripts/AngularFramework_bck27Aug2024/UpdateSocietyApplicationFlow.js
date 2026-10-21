
var MasterMod = angular.module('UpdateSocietyApplicationFlowModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("UpdateSocietyApplicationFlowCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.Department = [];
    $scope.ShowDetail = false;
    $scope.ApplicationFlowMaster = [];
    $scope.ShowUpdatePanel = false;
    $scope.ApplicationFlowMaster.SocietyRegistrationNo = '';
    $scope.ApplicationFlowMaster.StatusId = '';
    $scope.ApplicationFlowMaster.SocietyIDEnc = '';
    $scope.ApplicationFlowMaster.Remark = '';
    $scope.SocietyProceesStatus = [];
    $scope.ObjectionFlagBasic = false;
    $scope.ObjectionFlagAdmin = false;
    $scope.ObjectionFlagOffice = false;
    $scope.RoleId = 0;
    $scope.CurrentStatusId = 0;


    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl +"/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl +"/Translations/Translation_HI.json").
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
        $(".se-pre-con").fadeOut("slow");
        $scope.showLoader = false;
    };


    $scope.GetApplicationFlowSociety = function () {
        if ($scope.ValidationForm()) {
            $(".se-pre-con").fadeIn("slow");
            if ($scope.ApplicationFlowMaster.SocietyRegistrationNo != "") {
                $http.get(SiteUrl + "/ApplicationFlow/GetApplicationFlowSocietyByRegiNo/?SocietyRegiNo=" + $scope.ApplicationFlowMaster.SocietyRegistrationNo).
                    success(function (data, status) {
                        $scope.ApplicationFlowSociety = data;
                        $scope.SocietyFeesStatus();
                        $scope.ShowDetailtbl = true;

                        $scope.SocietyProceesStatus = [];
                        if (data.length > 0) {
                            var lenth = data.length;
                            $scope.CurrentStatusId = data[lenth - 1].StatusId;
                            $scope.ApplicationFlowMaster.SocietyIDEnc = data[0].SocietyIDEnc;
                            $scope.ShowDetail = true;
                            $scope.ShowUpdatePanel = true;
                            $scope.SocietyProceesStatusList();
                        }
                        else {
                            $scope.ShowDetail = false;
                            $scope.ShowUpdatePanel = false;
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
            }
        }

    };

    $scope.GetRoleList = function () {
        $scope.RoleList = [];
        $scope.RoleId = '0';
        $scope.OfficerData = [];
        if ($scope.ApplicationFlowMaster.StatusId == 3 || $scope.ApplicationFlowMaster.StatusId == 9 || $scope.ApplicationFlowMaster.StatusId == 12 || $scope.ApplicationFlowMaster.StatusId == 19 || $scope.ApplicationFlowMaster.StatusId == 23 || $scope.ApplicationFlowMaster.StatusId == 35 || $scope.ApplicationFlowMaster.StatusId == 36 || $scope.ApplicationFlowMaster.StatusId == 1058) {
            $http.post(SiteUrl + "/ApplicationFlow/GetRoleListForApplicationStatus", { "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "NextStatusId": $scope.ApplicationFlowMaster.StatusId, "CurrentStatusId": $scope.CurrentStatusId }).
                success(function (data, status) {
                    $scope.RoleList = data;
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
                }).
                error(function (data, status) {
                    $scope.OfficerData = [];
                });
        }
    };
    $scope.SocietyProceesStatusList = function () {
        $scope.SocietyProceesStatus = [];
        $http.post(SiteUrl +"/ApplicationFlow/GetStatusListBySocietyRegiNo", { "SocietyRegiNo": $scope.ApplicationFlowMaster.SocietyRegistrationNo }).
            success(function (data, status) {
                $scope.SocietyProceesStatus = data;
            }).
            error(function (data, status) {
                $scope.SocietyProceesStatus = [];
            });


    };


    $scope.UpdateSocietyStatus = function () {
        if ($scope.ApplicationFlowMaster.StatusId != 0 && $scope.ApplicationFlowMaster.Remark != "") {
            if ($scope.ApplicationFlowMaster.StatusId == 5 || $scope.ApplicationFlowMaster.StatusId == 10 || $scope.ApplicationFlowMaster.StatusId == 20) {
                if ($scope.ObjectionFlagBasic == false && $scope.ObjectionFlagAdmin == false && $scope.ObjectionFlagOffice == false) {
                    alert(_language == 'en-us' ? 'Please Select Objection Belong To.\n' : 'कृप्या आपत्ति संबंधित है चुनें|\n');
                }
                else {
                    $http.post("../ApplicationFlow/SaveSocietyObjectionBelong", { "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "ObjectionFlagBasic": $scope.ObjectionFlagBasic, "ObjectionFlagAdmin": $scope.ObjectionFlagAdmin, "ObjectionFlagOffice": $scope.ObjectionFlagOffice }).
                        success(function (data, status) {
                            $scope.saveDataInDB();
                        }).
                        error(function (data, status) {
                            alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                        });
                }
            }
            else {
                $scope.saveDataInDB();
            }
        } else {
            alert(_language == 'en-us' ? 'Please Select Status and Remark\n' : 'कृपया स्थिति और टिप्पणी जोड़ें|\n');
        }
        $scope.showLoader = false;
    }
    $scope.saveDataInDB = function ()
    {
        $scope.showLoader = true;
        if ($scope.ApplicationFlowMaster.StatusId == 3 || $scope.ApplicationFlowMaster.StatusId == 9 || $scope.ApplicationFlowMaster.StatusId == 12 || $scope.ApplicationFlowMaster.StatusId == 19 || $scope.ApplicationFlowMaster.StatusId == 23 || $scope.ApplicationFlowMaster.StatusId == 35 || $scope.ApplicationFlowMaster.StatusId == 36 || $scope.ApplicationFlowMaster.StatusId == 1058) {
            if ($scope.ApplicationFlowMaster.OfficeId == '0') {
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
                $http.post(SiteUrl + "/ApplicationFlow/SaveSocietyApplicationFlowStatus", { "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "Remark": $scope.ApplicationFlowMaster.Remark + "(Direct Update)", "StatusId": $scope.ApplicationFlowMaster.StatusId, "UserId": $scope.ApplicationFlowMaster.UserId }).
                    success(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        if (data == 1) {

                            if ($scope.ApplicationFlowMaster.StatusId == 14) {
                                $scope.BankOpeningCertificateGenerate($scope.ApplicationFlowMaster.SocietyIDEnc);
                            }
                            else if ($scope.ApplicationFlowMaster.StatusId != 17) {
                                alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n');
                                window.location = '../ApplicationFlow/UpdateSocietyApplicationFlow';
                            }
                            //else if ($scope.ApplicationFlowMaster.StatusId == 17) {
                            //    $scope.CertificateGenerate($scope.ApplicationFlowMaster.SocietyIDEnc);
                            //}
                            else {
                                alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n');
                                window.location = '../ApplicationFlow/UpdateSocietyApplicationFlow';
                            }
                        }
                        else {
                            alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                            window.location = '../ApplicationFlow/UpdateSocietyApplicationFlow';
                        }
                    }).
                    error(function (data, status) {
                        alert("Error while insert");
                    });
            });
        } else {
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/ApplicationFlow/SaveSocietyApplicationFlowStatus", { "SocietyIDEnc": $scope.ApplicationFlowMaster.SocietyIDEnc, "Remark": $scope.ApplicationFlowMaster.Remark + "(Direct Update)", "StatusId": $scope.ApplicationFlowMaster.StatusId, "UserId": $scope.ApplicationFlowMaster.UserId }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data == 1) {

                        if ($scope.ApplicationFlowMaster.StatusId == 14) {
                            $scope.BankOpeningCertificateGenerate($scope.ApplicationFlowMaster.SocietyIDEnc);
                        }
                        else if ($scope.ApplicationFlowMaster.StatusId != 17) {
                            alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n');
                            window.location = '../ApplicationFlow/UpdateSocietyApplicationFlow';
                        }
                        //else if ($scope.ApplicationFlowMaster.StatusId == 17) {
                        //    $scope.CertificateGenerate($scope.ApplicationFlowMaster.SocietyIDEnc);
                        //}
                        else {
                            alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n');
                            window.location = '../ApplicationFlow/UpdateSocietyApplicationFlow';
                        }
                    }
                    else {
                        alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                        window.location = '../ApplicationFlow/UpdateSocietyApplicationFlow';
                    }
                }).
                error(function (data, status) {
                    alert("Error while insert");
                });
        }
        $scope.showLoader = false;
    }
    $scope.BankOpeningCertificateGenerate = function (SocietyIDEnc) {
        $http.get(SiteUrl +"/ApplicationFlow/BankOpeningCertificateGenerate/?SocietyIDEnc=" + SocietyIDEnc).
            success(function (data, status) {
                alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n');
                window.location = '../ApplicationFlow/UpdateSocietyApplicationFlow';
                //$scope.ApplicationFlowMaster.Remark = "certificate generated";
                //$scope.ApplicationFlowMaster.StatusId = 17;
                //$scope.saveDataInDB();
            }).
            error(function (data1, status) {
                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                window.location = '../ApplicationFlow/UpdateSocietyApplicationFlow';

            });
    }


    //$scope.SocietyFeesStatus = function () {
    //    if ($scope.ApplicationFlowMaster.SocietyRegistrationNo != "") {
    //        $http.get("/ApplicationFlow/GetSocietyFeesStatus/?SocietyRegiNo=" + $scope.ApplicationFlowMaster.SocietyRegistrationNo).
    //            success(function (data, status) {
    //                alert(JSON.stringify(data));
    //                if (data.length > 0) {
    //                    $scope.SocietyFeedata = data;
    //                    alert(JSON.stringify($scope.SocietyFeedata));
    //                    alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n');
    //                }
    //            }).
    //            error(function (data, status) {
    //                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
    //                window.location = '../ApplicationFlow/UpdateSocietyApplicationFlow';

    //            });
    //    }
    //}


    $scope.ValidationForm = function () {
        message = "";
        if ($('#socregno').val() == '') {
            message += $scope._language == 'en-us' ? '\nPlease Enter Society Registration Number' : '\nकृपयाकृपया सोसायटी पंजीकरण संख्या दर्ज करें |';
            message += '<br />';
        }
       

        if (message.length > 5) {
            ShowModelAlert("Update Society Application Flow", message);
            return false;
        }
        return true;
    }

   

    $scope.SocietyFeesStatus = function () {
        $http.get(SiteUrl + "/ApplicationFlow/GetSocietyFeesStatus/?SocietyRegiNo=" + $scope.ApplicationFlowMaster.SocietyRegistrationNo).        
            success(function (data, status) {
                $scope.SocietyFeedata = data;                               
            }).
            error(function (data, status) {
                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');                
            });
    }

    $scope.OnPageLoad();


});