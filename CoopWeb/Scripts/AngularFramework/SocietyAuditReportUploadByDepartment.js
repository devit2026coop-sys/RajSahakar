var CitizenModule = angular.module("SocietyAuditReportUploadByDepartmentMod", ['angularUtils.directives.dirPagination']);
CitizenModule.controller("SocietyAuditReportUploadByDepartmentCnt", function ($scope, $http) {
    // Global Setting
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.divTabulation = true;
    $scope.divAddBtn = false;
    $scope.divSocietyAuditReport = false;
    $scope.divSocietyAuditReportShow = false;
    $scope.SocietyIdForSocietyAuditDetailsCitizen = 0;
    $scope.SocietyTypeList = [];

    // START : Call default functions on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.DashboardData();
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
            $(".se-pre-con").fadeOut("slow");
        }


        $scope.getSocietyfilterBySocietyType = function () {
            $scope.DashboardData($scope.SocietyRegistration.ActId, $scope.SocietyRegistration.SocietyTypeId, $scope.SocietyRegistration.ClassId);
        };

        $scope.getSocietyfilter = function () {

            $scope.SocietyRegistration.SocietyTypeId = '0';
            $scope.SocietyRegistration.ClassId = '0';
            $scope.GetSocietyTypeList();
            if ($scope.SocietyRegistration.ActId == 7) {
                $scope.SocietyClassList = [];
            }
            else {
                $scope.GetSocietyClassList();
            }
            $scope.DashboardData($scope.SocietyRegistration.ActId, $scope.SocietyRegistration.SocietyTypeId, $scope.SocietyRegistration.ClassId);

        };
        $scope.getSocietyfilterByClass = function () {

            $scope.DashboardData($scope.SocietyRegistration.ActId, $scope.SocietyRegistration.SocietyTypeId, $scope.SocietyRegistration.ClassId);

        };
        // GET Society Type
        $scope.GetSocietyTypeList = function () {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=" + $scope.SocietyRegistration.ActId).
                success(function (data, status) {
                    $scope.SocietyTypeList = data;
                }).
                error(function (data, status) {
                    $scope.SocietyTypeList = [];

                });
        };
        //$http.get(SiteUrl+"/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=-1").
        //    success(function (data, status) {
        //        $scope.SocietyTypeList = data;
        //    }).
        //    error(function (data, status) {
        //        $scope.SocietyTypeList = [];
        //    });


        // GET Society Class
        $scope.GetSocietyClassList = function () {
            $scope.showLoader = true;
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYCLASS&MasterCode=-1").
                success(function (data, status) {
                    $scope.SocietyClassList = data;
                }).
                error(function (data, status) {
                    $scope.SocietyClassList = [];
                });

        };


        //GET Financial Year
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });

        // Get List of all ACT
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
            success(function (data, status) {
                $scope.ActList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Act list.");

            });

        $(".se-pre-con").fadeOut("slow");

    };
    // END : Call default functions on page load

    $scope.UploadRegistrationCertificateFile = function (input, file) {
        if (!validateFile(input)) {
            input.value = '';
            return;
        }
        $scope.RegistrationCertificateFile = file[0];
    }

    $scope.UploadBylawsFile = function (input, file) {
        if (!validateFile(input)) {
            input.value = '';
            return;
        }
        $scope.BylawsFile = file[0];
    }

    $scope.UploadRulesAndRegulationFile = function (input, file) {
        if (!validateFile(input)) {
            input.value = '';
            return;
        }
        $scope.RulesAndRegulation = file[0];
    }

    $scope.UploadSocietyAuditFile = function (input, file) {
        if (!validateFile(input)) {
            input.value = '';
            return;
        }
        $scope.AuditReport = file[0];
    }

    $scope.UploadHousingSocietyMember = function (input, file) {
        if (!validateFile(input)) {
            input.value = '';
            return;
        }
        $scope.SocietyAuditReport.HousingSocietyMember = file[0];
    }


    $scope.AddAdministrativeDetails = function (SID, ShowTab, SocietyName) {
        $scope.showLoader = true;
        $scope.q = '';
        $scope.SocietyName = SocietyName;
        $scope.divSocietyAuditReport = true;
        $scope.divTabulation = false;
        $scope.divSocietyAuditReportShow = false;
        $scope.divAddBtn = false;
        $scope.SocietyIdForSocietyAuditDetailsCitizen = SID;
        $scope.GetAuditorAssignReportDataShow();
        $scope.showLoader = false;
    }



    // START : Get Sub Class
    $scope.GetSubClass = function () {

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYSUBCLASS&MasterCode=" + $scope.SocietyRegistration.ClassId).
            success(function (data, status) {
                $scope.SocietySubClassList = data;
            }).
            error(function (data, status) {
                $scope.SocietySubClassList = [];
            });

    };
    // END : Get Sub Class


    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.SocietyRegistration = null;
        $scope.SocietyRegistration.RegistrationCertificateFile = null;
    };
    //END : Delete User
    // START : Clear Society Profile Section
    $scope.ClearSocietyDetailsForm = function () {

        $scope.SocietyRegistration.ActId = '0';
        $scope.SocietyRegistration.BRN = '';
        $scope.SocietyRegistration.RegistrationNo = '';
        $scope.SocietyRegistration.SocietyName = '';
        $scope.SocietyRegistration.RegistrationDate = '';
        $scope.SocietyRegistration.SocietyTypeId = '0';
        $scope.SocietyRegistration.ClassId = '0';
        $scope.SocietySubClassList = [];
        $scope.SocietyRegistration.SubClassId = '0';
        $scope.SocietyRegistration.RegistrationCertificateFile = null;
    };
    // END : Clear Society Profile Section

    // START : Save Society Audit Report Details
    $scope.SaveSocietyAuditDetails = function () {

        var Msg = '';

        //if ($scope.SocietyAuditReport.IsApproved == undefined) {
        //    Msg += (_language == 'en-us' ? 'Is Audit Report Approved ? \n' : 'क्या लेखा परीक्षा रिपोर्ट अनुमोदित है\n');
        //    Msg += '<br />';
        //}

        //if ($scope.SocietyAuditReport.IsSocietyWorking == undefined) {
        //    Msg += (_language == 'en-us' ? 'Is Society Working ? \n' : 'क्या सोसाइटी कार्यशील है ?\n');
        //    Msg += '<br />';
        //}

        if ($scope.SocietyAuditReport.FinancialYear == undefined || $scope.SocietyAuditReport.FinancialYear == '0') {
            Msg += (_language == 'en-us' ? 'Please Select Financial Year \n' : 'वित्तीय वर्ष का चयन करे\n');
            Msg += '<br />';
        }

        //if ($scope.SocietyAuditReport.IsApproved == 'true') {
        //    if ($scope.SocietyAuditReport.ApprovedBy == undefined) {
        //        Msg += (_language == 'en-us' ? 'Please select Approved By \n' : 'कृपया किसके द्वारा अनुमोदित है की स्थिति का चयन करे \n');
        //        Msg += '<br />';
        //    }
        //}
        // alert($scope.AuditReport);
        debugger
        if ($scope.AuditReport == undefined) {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload Audit Report(PDF)' : '\nकृपया लेखा परीक्षा रिपोर्ट अपलोड करें(पीडीएफ) |';
            Msg += '<br />';
        }

        if ($scope.SocietyAuditReport.HousingSocietyMember == undefined) {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload Member List of housing society(Excel)' : '\nकृपया आवास सोसायटी की सदस्य सूची अपलोड करें(एक्सेल) |';
            Msg += '<br />';
        }

        if (Msg.length > 1) {
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + '<br />' + Msg;


            ShowModelAlert("Society Audit Details", Msg);
            return;
        }

        var formData = new FormData();
        formData.append("SocietyIdEnc", $scope.SocietyIdForSocietyAuditDetailsCitizen);
        formData.append("IsApproved", true);
        formData.append("IsSocietyWorking", true);
        formData.append("FinancialYear", $scope.SocietyAuditReport.FinancialYear);
        formData.append("AuditReport", $scope.AuditReport);
        formData.append("HousingSocietyMember", $scope.SocietyAuditReport.HousingSocietyMember);
        formData.append("ApprovedBy", 0);

        $scope.showLoader = true;
        $http.post(SiteUrl + "/Society/SaveHousingSocietyAuditReportDetails", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                var Result = parseInt(data, 10);

                $scope.showLoader = false;
                if (Result > 0) {
                    AltMsg = (_language == 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                    $scope.GetAuditorAssignReportDataShow();
                }
                else if (Result == -2) {
                    AltMsg = (_language == 'en-us' ? 'Data Already Exist.' : 'डेटा पहले से मौजूद है');
                }
                else if (Result == -10) {
                    AltMsg = (_language == 'en-us' ? 'Please upload valid file' : 'कृपया वैध फ़ाइल अपलोड करें।');
                }
                else {
                    AltMsg = (_language == 'en-us' ? 'Some Error Occurred please try after some time.' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                }
                ShowModelAlert("Society Audit Details", AltMsg);
                $scope.ClearSocietyAuditReportForm();
            }).
            error(function (data, status) {
                $scope.showLoader = false;
                ShowModelAlert("Society Audit Report", "Error While Society Audit Report Insert");
            });
    };
    // END :  Save Society Audit Report Details

    // START : Clear Society Audit Report Details
    $scope.ClearSocietyAuditReportForm = function () {

        //$scope.SocietyAuditReport.IsApproved = false;
        //$scope.SocietyAuditReport.IsSocietyWorking = false;
        $scope.SocietyAuditReport.FinancialYear = '0';
        //$scope.SocietyAuditReport.ApprovedBy = 0;
        $("#flAuditReport").val('');
        $("#flHousingSocietyMember").val('');
    };
    // END : Clear Society Audit Report Details



    // START Back To Page
    $scope.BackToPage = function () {
        $scope.divTabulation = true;
        $scope.divAddBtn = false;
        $scope.divSocietyAuditReport = false;
        $scope.divSocietyAuditReportShow = false;
    }
    // START Back To Page

    //start show func
    $scope.SHownew = function (item) {
        if (item == 1) {
            $scope.newdiv = true;
        }
        else {
            $scope.newdiv = false;
        }
    }
    //end



    // START : Add New Registration
    $scope.AddNewSocietyAudit = function () {
        $scope.divTabulation = false;
        $scope.divAddBtn = false;
        $scope.divSocietyAuditReport = true;

    }
    // END : Add New Registration

    // START : Add New Registration
    $scope.AddDetails = function (SID) {
        $scope.divTabulation = false;
        $scope.divAddBtn = false;
        $scope.divSocialProfiling = true;
        $scope.divSocietyProfilingShow = false;
        $scope.btnBasicDetails = true;
        $scope.btnAdministrativeDetails = false;
    }
    // END : Add New Registration


    // START : Get Sub Class

    $scope.GetAuditorAssignReportDataShow = function () {

        $http.get(SiteUrl + "/Society/GetAuditorAssignReportDataShow/?SocietyIDEnc=" + $scope.SocietyIdForSocietyAuditDetailsCitizen).

            success(function (data, status) {
                $scope.GetAuditorAssignReportDataShow1 = data;
            }).
            error(function (data, status) {

            });

    };

    // END : Get Sub Class
    //GET Load Table data
    $scope.DashboardData = function (ActId, SocietyTypeId, ClassId) {
        $scope.showLoader = true;
        $http.post(SiteUrl + "/Society/HouseSocietyListForAuditRPT", { "ActId": ActId == undefined ? 0 : parseInt(ActId), "SocietyTypeId": SocietyTypeId == undefined ? 0 : parseInt(SocietyTypeId), "ClassId": ClassId == undefined ? 0 : parseInt(ClassId) }).
            success(function (data, status) {
                $scope.TableData = [];
                data = data.filter(x => x.IsAdministrativeDetails == '3')
                $scope.TableData = data;
                $scope.showLoader = false;
            }).
            error(function (data, status) {
                $scope.TableData = [];
                $scope.showLoader = false;
            });
    }
    //END

    // START : Call Functions on Page Load
    $scope.OnPageLoad();

    //  $scope.GetAuditorAssignReportDataShow();
    //END : Call Functions on Page Load


    $scope.SetStatus = function (SocietyAuditReportId, ActionId) {

        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Society/DeleteAuditReport", { Id: SocietyAuditReportId, Action: ActionId }).
            success(function (data, status) {
                $scope.GetAuditorAssignReportDataShow();

            }).
            error(function (data, status) {
            });

    };


});

