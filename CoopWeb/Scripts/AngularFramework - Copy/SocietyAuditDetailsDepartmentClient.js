var CitizenModule = angular.module("DepartmentSocietyAuditMod", []);
CitizenModule.controller("DepartmentSocietyAuditCnt", function ($scope, $http) {
    // Global Setting
    $scope.divTabulation = true;
    $scope.divAddBtn = false;
    $scope.divSocietyAuditReport = false;
    $scope.SocietyIdForSocietyAuditDetails = 0;
    $scope.divSocietyAuditReportShow = false;

    // START : Call default functions on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope._language = _language; // Set LAnguiage
        if (_language == 'en-us') {
            $http.get(SiteUrl+"/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl+"/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }

        // GET Society Type
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=-1").
            success(function (data, status) {
                $scope.SocietyTypeList = data;
            }).
            error(function (data, status) {
                $scope.SocietyTypeList = [];
            });


        // GET Society Class
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=SOCIETYCLASS&MasterCode=-1").
            success(function (data, status) {
                $scope.SocietyClassList = data;
            }).
            error(function (data, status) {
                $scope.SocietyClassList = [];
            });


        //GET Financial Year
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });

        // Get List of all ACT
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
            success(function (data, status) {
                $scope.ActList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Act list.");
                $(".se-pre-con").fadeOut("slow");
            });

    };
    // END : Call default functions on page load

    $scope.UploadRegistrationCertificateFile = function (file) {
        $scope.RegistrationCertificateFile = file[0];
    }

    $scope.UploadBylawsFile = function (file) {
        $scope.BylawsFile = file[0];
    }

    $scope.UploadRulesAndRegulationFile = function (file) {
        $scope.RulesAndRegulation = file[0];
    }

    

    $scope.UploadSocietyAuditFile = function (file) {
        $scope.AuditReport = file[0];
    }

    $scope.AddAdministrativeDetails = function (SID, ShowTab, SocietyName) {
        $scope.SocietyName = SocietyName;
        $scope.divSocietyAuditReport = true;
        $scope.divTabulation = false;
        $scope.divSocietyAuditReportShow = false;
        $scope.divAddBtn = false;        
        $scope.SocietyIdForSocietyAuditDetails = SID;
    }

    // START : Get Sub Class
    $scope.GetSubClass = function () {
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=SOCIETYSUBCLASS&MasterCode=" + $scope.SocietyRegistration.ClassId).
            success(function (data, status) {
                $scope.SocietySubClassList = data;
            }).
            error(function (data, status) {
                $scope.SocietySubClassList = [];
            });

    };
    // END : Get Sub Class

    // START : Save Society Profiling
    $scope.SaveSocietyProfiling = function () {

        var ddMMyyyyDate = new Date($scope.SocietyRegistration.GeneralMeetingDate);
        var dd = ddMMyyyyDate.getDate();
        var mm = ddMMyyyyDate.getMonth() + 1; //January is 0!

        var yyyy = ddMMyyyyDate.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }

        var RDddMMyyyyDate = new Date($scope.SocietyRegistration.RegistrationDate);
        var RDdd = RDddMMyyyyDate.getDate();
        var RDmm = RDddMMyyyyDate.getMonth() + 1; //January is 0!

        var RDyyyy = RDddMMyyyyDate.getFullYear();
        if (RDdd < 10) {
            RDdd = '0' + RDdd;
        }
        if (RDmm < 10) {
            RDmm = '0' + RDmm;
        }


        var formData = new FormData();
        formData.append("BRN", $scope.SocietyRegistration.BRN);
        formData.append("ActId", $scope.SocietyRegistration.ActId);
        formData.append("RegistrationNo", $scope.SocietyRegistration.RegistrationNo);
        formData.append("SocietyName", $scope.SocietyRegistration.SocietyName);
        formData.append("SocietyTypeId", $scope.SocietyRegistration.SocietyTypeId);

        formData.append("ClassId", $scope.SocietyRegistration.ClassId);
        formData.append("SubClassId", $scope.SocietyRegistration.SubClassId);
        formData.append("ByeLawsSocietyFile", $scope.ByeLawsSocietyFile);
        formData.append("RegistrationDate", $('#txtRegistrationDate').val());
        formData.append("GeneralMeetingProceedingFile", $scope.GeneralMeetingProceeding);
        formData.append("BalanceSheetFile", $scope.BalanceSheetFile);
        formData.append("RegistrationCertificateFile", $scope.RegistrationCertificateFile);
        formData.append("BylawsFile", $scope.BylawsFile);
        formData.append("RulesAndRegulation", $scope.RulesAndRegulation);


        $http.post(SiteUrl+"/Society/SaveSocietyProfiling", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                if (data > 0) {
                    ShowModelAlert("Society Profiling", "Society Profiling Insert Sucessfully");
                }
                //LoaderStart();
                //$('#modelAlertMessage').html('');
                //var errorLst = [];
                //for (var i = 0; i < Object.keys(data).length; i++) {
                //    errorLst.push(data.data[i]);
                //}
                //var errorMsg = errorLst.join("</br>");
                //ShowModelAlert("User Registration", errorMsg);
                //if (data.Status) {
                $scope.ClearForm();
                //    $scope.UserList();
                //}
                //LoaderStop();
            }).
            error(function (data, status) {

                ShowModelAlert("Society Profiling", "Error While Society Profiling Insert");
            });

    };
    // END :Save Society Profiling

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

        if ($scope.SocietyAuditReport.IsApprovedByRegistrar == undefined) {
            Msg += (_language == 'en-us' ? 'Is Audit Report Approved By Registrar (Latest Available) ? \n' : 'क्या लेखा परीक्षा रिपोर्ट रजिस्ट्रार द्वारा स्वीकृत है\n');
        }

        if ($scope.SocietyAuditReport.IsSocietyWorking == undefined) {
            Msg += (_language == 'en-us' ? 'Is Society Working ? \n' : 'क्या सोसाइटी कार्यशील है ?\n');
        }

        if ($scope.SocietyAuditReport.FinancialYear == undefined || $scope.SocietyAuditReport.FinancialYear == '0') {
            Msg += (_language == 'en-us' ? 'Financial Year For Which Last Audit Report is Available \n' : 'वित्तीय वर्ष जिसके लिए अंतिम लेखा परीक्षा रिपोर्ट उपलब्ध है\n');
        }

        if (Msg.length > 1) {
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + Msg;
            alert(Msg);
            return;
        }

        var formData = new FormData();
        formData.append("SocietyId", $scope.SocietyIdForSocietyAuditDetails);
        //formData.append("SocietyId", $scope.hdfSocietyId);
        formData.append("IsApprovedByRegistrar", $scope.SocietyAuditReport.IsApprovedByRegistrar);
        formData.append("IsSocietyWorking", $scope.SocietyAuditReport.IsSocietyWorking);
        formData.append("FinancialYear", $scope.SocietyAuditReport.FinancialYear);
        formData.append("AuditReport", $scope.AuditReport);

        $http.post(SiteUrl+"/Society/SaveSocietyAuditReportDetails", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                var Result = parseInt(data, 10);

                if (Result > 0) {
                    AltMsg = (_language == 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                    alert(AltMsg);
                }

                $scope.ClearSocietyAuditReportForm();
            }).
            error(function (data, status) {
                ShowModelAlert("Society Audit Report", "Error While Society Audit Report Insert");
            });
    };
    // END :  Save Society Audit Report Details

    // START : Clear Society Audit Report Details
    $scope.ClearSocietyAuditReportForm = function () {
        $scope.SocietyAuditReport.IsApprovedByRegistrar = false;
        $scope.SocietyAuditReport.IsSocietyWorking = false;
        $scope.SocietyAuditReport.FinancialYear = '0';
        $('#flAuditReport').val('');

    };
    // END : Clear Society Audit Report Details

    // START Show Society Details
    $scope.ShowDetails = function (SID) {
        $scope.divTabulation = false;
        $scope.divAddBtn = false;
        $scope.divSocietyAuditReport = false;
        $scope.divSocietyAuditReportShow = true;
        $http.post(SiteUrl+"/Society/GetSocietyAuditDetailsBySocietyId", { "SocietyId": SID }).
            success(function (data, status) {
                $scope.SocietyAuditDetails = data;
            }).
            error(function (data, status) {
                $scope.SocietyAuditDetails = [];
            });
    }
    // END Show Society Details

    // START Back To Page
    $scope.BackToPage = function () {
        $scope.divTabulation = true;
        $scope.divAddBtn = false;
        $scope.divSocietyAuditReport = false;
        $scope.divSocietyAuditReportShow = false;
    }
    // START Back To Page



    // START : Save Society Administrative Details
    $scope.SaveSocietyAdministrativeDetails = function () {
        var SocietyRegistrationData = [];
        SocietyRegistrationData.push({ "Name": $scope.SocietyRegistration.CEOName, "ContactNo": $scope.SocietyRegistration.CEOContactNo, "UserType": "1" });
        SocietyRegistrationData.push({ "Name": $scope.SocietyRegistration.ChairmanName, "ContactNo": $scope.SocietyRegistration.ChairmanContactNo, "UserType": "2" });

        $('.table tbody > tr').each(function () {

            SocietyRegistrationData.push({ "Name": $(this).find('td:eq(1)').find('input').val(), "ContactNo": $(this).find('td:eq(2)').find('input').val(), "UserType": "3" });
        });

        $http.post(SiteUrl+"/Society/SaveSocietyAdministrativeDetails", { "SocietyRegistrationData": SocietyRegistrationData, "ClassA": $scope.SocietyRegistration.TotalMembersClassA, "ClassB": $scope.SocietyRegistration.TotalMembersClassB, "ClassC": $scope.SocietyRegistration.TotalMembersClassC }).
            success(function (data, status) {
                ShowModelAlert("Society Administrative Details", data.data[0]);
                $scope.ClearSocietyAdministrativeDetails();
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    // END : Save Society Administrative Details

    $scope.ClearSocietyAdministrativeDetails = function () {        
        $('input').val('');
    }

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




    // START : Call Functions on Page Load
    $scope.OnPageLoad();
    //END : Call Functions on Page Load
});

