var app = angular.module('Sprofile', []);
app.controller('SocietyP', function ($scope, $http) {

    $scope.divTabulation = true;
    $scope.divSocialProfiling = false;
    $scope.divSocietyProfilingShow = false;
    $scope.divAddBtn = true;
    $scope.Message = '';
    $scope.SocietyIdForAdministrativeDetails = 0;


    $scope.ClearExcelDataForm = function () {
        $scope.excel = null;
    };

    $scope.UploadRegistrationCertificateFile = function (file) {
        $scope.RegistrationCertificateFile = file[0];
    }

    $scope.UploadBylawsFile = function (file) {
        $scope.BylawsFile = file[0];
    }

    $scope.UploadRulesAndRegulationFile = function (file) {
        $scope.RulesAndRegulation = file[0];
    }
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

        // Get List of all ACT
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
            success(function (data, status) {
                $scope.ActList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Act list.");
                $scope.showLoader = false;
            });


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
        $(".se-pre-con").fadeOut("slow");

    };
    // END : Call default functions on page load
    $scope.AddAdministrativeDetails = function (SID, ShowTab, SocietyName, FilledProfileLevel, RegistrationNo) {
        if (FilledProfileLevel == 0) {
            $scope.divTabulation = false;
            $scope.divSocialProfiling = true;
            openCity(event, 'SocietyAdministrative');
            $("#BtnSocietyDetails").prop('disabled', true);
            $("#btnSocietyOfficeDetails").prop('disabled', true);
            $("#btnSocietyAdministrative").addClass('tablinks ng-binding active');
            $scope.divSocietyProfilingShow = false;
            $scope.divAddBtn = false;
            $scope.SocietyIdForAdministrativeDetails = SID;
            $scope.SocietyDirectory = RegistrationNo;            
        }
        else {
            $scope.SocietyIdForAdministrativeDetails = SID;
            $scope.divTabulation = false;
            $scope.divSocialProfiling = true;
            openCity(event, 'SocietyOfficeDetails');
            $('#SocietyAdministrative').hide();
            $('#SocietyDetails').hide();
            $("#btnSocietyAdministrative").prop('disabled', true);
            $("#BtnSocietyDetails").prop('disabled', true);
            $("#btnSocietyOfficeDetails").prop('disabled', false);
            $("#btnSocietyOfficeDetails").addClass('tablinks ng-binding active');
            $scope.divSocietyProfilingShow = false;
            $scope.divAddBtn = false;
            $("#DivSocietySteps").removeClass("SocietyStep_1")
            $("#DivSocietySteps").removeClass("SocietyStep_2")
            $("#DivSocietySteps").addClass("SocietyStep_3");

        }
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
    // START Back To Page
    $scope.BackToPage = function () {
        $scope.divTabulation = true;
        $scope.divAddBtn = true;
        $scope.divSocialProfiling = false;
        $scope.divSocietyProfilingShow = false;
        $scope.SocietyRegistration.CEOName = "test";
    }
    // END Back To Page
    // START Show Society Details
    $scope.ShowDetails = function (SID, ShowTab) {
        document.body.scrollTop = 0;
        document.documentElement.scrollTop = 0;
        //if (ShowTab == 'AddMoreDetails') {
        //    openCity(event, 'SocietyAdministrativeShow');
        //}
        //else if (ShowTab == 'ShowDetails') 
        //{
        //    openCity(event, 'SocietyDetailsShow')
        //}
        $scope.divTabulation = false;
        $scope.divAddBtn = false;
        $scope.divSocialProfiling = false;
        $scope.divSocietyProfilingShow = true;
        $http.post(SiteUrl+"/Society/GetSocietyDetailsBySocietyId", { "SocietyId": SID }).
            success(function (data, status) {
                $scope.SocietyDetailsData = [];
                if (data.length > 0) {
                    var BasicDetails = data[0];
                    var AdministrativeDetails = [];

                    angular.forEach(data, function (item) {
                        AdministrativeDetails.push({ "Name": item.Name, "ContactNo": item.ContactNo, "Type": item.UserType });
                    });
                    $scope.SocietyDetailsData.BasicDetails = [];
                    $scope.SocietyDetailsData.BasicDetails = BasicDetails;
                    //alert(JSON.stringify($scope.SocietyDetailsData.BasicDetails));
                    var CEODetails = data.filter(x => x.UserType == 1);
                    $scope.SocietyDetailsData.CEO = CEODetails.length > 0 ? CEODetails[0] : [];

                    var ChairmanDetails = data.filter(x => x.UserType == 2);
                    $scope.SocietyDetailsData.Chairman = ChairmanDetails.length > 0 ? ChairmanDetails[0] : [];

                    var AdministratorDetails = data.filter(x => x.UserType == 4);
                    $scope.SocietyDetailsData.Administrator = AdministratorDetails.length > 0 ? AdministratorDetails[0] : [];
                    $scope.SocietyDetailsData.Administrator.IsAdministratorAppointed = AdministratorDetails.length > 0 ? $scope.translation.Yes : $scope.translation.No;
                    $scope.IsAdministratorAppointed = AdministratorDetails.length > 0 ? true : false;

                    var MemberDetails = data.filter(x => x.UserType == 3);

                    $scope.SocietyDetailsData.MemberDetails = MemberDetails;

                    if ($scope.SocietyDetailsData.BasicDetails.IsRural == "Urban") {
                        $scope.divCity = true;
                        $scope.divBlock = false;
                        $scope.divWard = true;
                        $scope.divGP = false;
                        $scope.divVillage = false;
                    }
                    else {
                        $scope.divCity = false;
                        $scope.divBlock = true;
                        $scope.divWard = false;
                        $scope.divGP = true;
                        $scope.divVillage = true;

                    }
                }
                else {
                    $scope.SocietyDetailsData["BasicDetails"] = [];
                    $scope.SocietyDetailsData["AdministrativeDetails"] = [];
                }
            }).
            error(function (data, status) {
                alert("Error while fetching Act list.");
                $scope.showLoader = false;
            });
    }
    // END Show Society Details
    // START : Save Society Profiling
    $scope.SaveSocietyProfiling = function () {

        if ($scope.FormValidation()) {
            var formData = new FormData();
            formData.append("UserId", 0);
            formData.append("BRN", $scope.SocietyRegistration.BRN);
            formData.append("RegistrationNo", $scope.SocietyRegistration.RegistrationNo);
            formData.append("SocietyName", $scope.SocietyRegistration.SocietyName);
            formData.append("SocietyTypeId", $scope.SocietyRegistration.SocietyTypeId);
            formData.append("ActId", $scope.SocietyRegistration.ActId);
            formData.append("BusinessId", $scope.SocietyRegistration.BusinessId);
            formData.append("LiablityId", $scope.SocietyRegistration.LiablityId);
            formData.append("MemberLiablity", $scope.SocietyRegistration.MemberLiablity);
            formData.append("ClassId", $scope.SocietyRegistration.ClassId);
            formData.append("SubClassId", $scope.SocietyRegistration.SubClassId);
            formData.append("TotalMembersApplyingForSocociety", $scope.SocietyRegistration.TotalMembersApplyingForSocociety);
            formData.append("EntranceFeesCollected", $scope.SocietyRegistration.EntranceFeesCollected);
            formData.append("SingleShareValue", $scope.SocietyRegistration.SingleShareValue);
            formData.append("PaidUpShareCapital", $scope.SocietyRegistration.PaidUpShareCapital);
            formData.append("ByeLawsSocietyFile", $scope.ByeLawsSocietyFile);
            formData.append("WorkPlanShowingEconomicViabilityFile", $scope.EconomyViabilityFile);
            formData.append("GeneralMeetingDate", $('').val());
            formData.append("RegistrationDate", $('#txtRegistrationDate').val());
            //formData.append("GeneralMeetingDate", (dd + '/' + mm + '/' + yyyy));
            //formData.append("RegistrationDate", (RDdd + '/' + RDmm + '/' + RDyyyy));
            formData.append("MembersParticipatingGeneralMeeting", $scope.SocietyRegistration.MembersParticipatingGeneralMeeting);
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
        }

    };
    // END :Save Society Profiling
    // START : Add New Registration
    $scope.AddNewSociety = function () {
        $scope.divTabulation = false;
        $scope.divAddBtn = false;
        $scope.divSocialProfiling = true;
        $scope.divSocietyProfilingShow = false;
        $scope.btnBasicDetails = true;
        $scope.btnAdministrativeDetails = true;
    }
    // END : Add New Registration
    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.SocietyRegistration = null;        
        $("#flRegistrationCertificateFile").val('');
        $("#flRulesAndRegulation").val('');
        $("#flBylawsFile").val('');
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
        //$scope.SocietyRegistration.RegistrationCertificateFile = null;
        //$("#input[type='file']").val('');
        $("#flRegistrationCertificateFile").val('');
        $("#flRulesAndRegulation").val('');
        $("#flBylawsFile").val('');
    };
    // END : Clear Society Profile Section
    //validateAddForm 
    $scope.FormValidation = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        message += '<br />';

        if ($scope.SocietyRegistration.RegistrationNo == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Society Registration No.' : '\nसोसायटी पंजीकरण संख्या दर्ज करें |';
            message += '<br />';
        }
        if ($scope.SocietyRegistration.SocietyName == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Enter Society Name' : '\nकृपया सोसाइटी का नाम दर्ज करें |';
            message += '<br />';
        }

        if ($scope.SocietyRegistration.ActId == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Select Act' : '\nकृपया अधिनियम का चयन करे |';
            message += '<br />';
        }

        //if ($scope.SocietyRegistration.BRN == undefined) {
        //    message += $scope._language == 'en-us' ? '\nPlease Enter BRN No.' : '\nकृपया बीआरएन नंबर दर्ज करें |';
        //    message += '<br />';
        //}

        if ($('#txtRegistrationDate').val().length == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Select Registration Date' : '\nकृपया पंजीकरण की तारीख का चयन करे |';
            message += '<br />';
        }
        if ($scope.SocietyRegistration.SocietyTypeId == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Select Society Type' : '\nकृपया सोसाइटी का प्रकार चुनें |';
            message += '<br />';
        }
        if ($scope.SocietyRegistration.ClassId == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Select Society Class' : '\nकृपया सोसाइटी क्लास का चयन करे  |';
        }
        if ($scope.SocietyRegistration.SubClassId == 0) {
            message += $scope._language == 'en-us' ? '\nPlease Select Sub Society Class' : '\nकृपया उप सोसाइटी क्लास का चयन करे |';
            message += '<br />';
        }
        if ($scope.RegistrationCertificateFile == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Upload Registration Certificate(PDF)' : '\nकृपया पंजीकरण प्रमाण पत्र अपलोड करें (पीडीएफ) |';
            message += '<br />';
        }

        //if ($('#txtMobileNo').val().length > 0) {
        //    if ($('#txtMobileNo').val().length < 8 || $('#txtMobileNo').val().length > 11) {
        //        message += IsUserLanguageSelectedEnglish ? '\nEnter Valid Mobile Number' : '\nवैध मोबाइल नंबर दर्ज करें |';
        //    }
        //}
        //if ($('#txtEmailID').val().length > 0) {
        //    var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
        //    if (!$('#txtEmailID').val().match(mailformat)) {
        //        message += IsUserLanguageSelectedEnglish ? '\nEnter Valid Email ID' : '\nवैध ईमेल आईडी दर्ज करें |';
        //    }
        //}


        if (message.length > 27) {


            if (message.length > 29) {

                ShowModelAlert("Society Profiling", message);
                return false;
            }
            return true;


        }
        //validateAddForm for farmer ends
       

    }

    $scope.showHideDiv = function () {
        
        if ($scope.SocietyRegistration.ActId == 3) {
            $scope.showhideBylaws = true;
            $scope.showhideOther = false;
        }
        else {
            $scope.showhideBylaws = false;
            $scope.showhideOther = true;

        }
    }
    $scope.OnPageLoad();

});