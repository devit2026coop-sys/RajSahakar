var SocietyRegistrationModule = angular.module("SocietyRegistrationMod", ['angularUtils.directives.dirPagination']);
SocietyRegistrationModule.controller("SocietyRegistrationCnt", function ($scope, $http) {

    // START : Call default functions on page load
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;

    $scope.OnPageLoad = function () {
        //$(".se-pre-con").fadeIn("slow");    
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
            //$(".se-pre-con").fadeOut("slow");
        }
       

        // GET Society Type
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=-1").
            success(function (data, status) {
                $scope.SocietyTypeList = data;
            }).
            error(function (data, status) {
                $scope.SocietyTypeList = [];
            });


        // GET Liablity Type
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=LIABILITY&MasterCode=-1").
            success(function (data, status) {
                $scope.LiabilityTypeList = data;
            }).
            error(function (data, status) {
                $scope.LiabilityTypeList = [];
            });

        // GET Business
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=BUSINESS&MasterCode=-1").
            success(function (data, status) {
                $scope.BusinessTypeList = data;
            }).
            error(function (data, status) {
                $scope.BusinessTypeList = [];
            });

        // GET Society Class
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=SOCIETYCLASS&MasterCode=-1").
            success(function (data, status) {
                $scope.SocietyClassList = data;
            }).
            error(function (data, status) {
                $scope.SocietyClassList = [];
            });
        //$(".se-pre-con").fadeOut("slow");
    };
    // END : Call default functions on page load

    // START : Code for File Upload
    $scope.UploadByeLawsSocietyFile = function (file) {
        $scope.ByeLawsSocietyFile = file[0];
    }
    $scope.UploadEconomyViabilityFile = function (file) {
        $scope.EconomyViabilityFile = file[0];
    }
    $scope.UploadGeneralMeetingProceedingFile = function (file) {
        $scope.GeneralMeetingProceeding = file[0];
    }
    $scope.UploadBalanceSheetFile = function (file) {
        $scope.BalanceSheetFile = file[0];
    }
    $scope.UploadBearerFile = function (file) {
        $scope.BearerFile = file[0];
    }
    $scope.UploadSocietyAuditFile = function (file) {
        $scope.SocietyAuditFile = file[0];
    }    
    $scope.UploadBulkInsertFile = function (file) {
        $scope.BulkInsertFile = file[0];
    } 
    // END : Code for File Upload

    // START : Get Sub Class
    $scope.GetSubClass = function () {
        //$http.get(SiteUrl + "/Master/SubClassList/?ClassId=" + $scope.SocietyRegistration.ClassId + "&SocietyTypeId=" + $scope.SocietyRegistration.SocietyTypeId).        
        $http.get(SiteUrl + "/Master/SubClassList/?ClassId=" + $scope.SocietyRegistration.ClassId).
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

        var formData = new FormData();
        formData.append("UserId", 0);
        formData.append("SocietyName", $scope.SocietyRegistration.SocietyName);
        formData.append("SocietyTypeId", $scope.SocietyRegistration.SocietyTypeId);
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
        formData.append("GeneralMeetingDate", (dd + '/' + mm + '/' + yyyy));
        formData.append("MembersParticipatingGeneralMeeting", $scope.SocietyRegistration.MembersParticipatingGeneralMeeting);
        formData.append("GeneralMeetingProceedingFile", $scope.GeneralMeetingProceeding);
        formData.append("BalanceSheetFile", $scope.BalanceSheetFile);

        $http.post(SiteUrl+"/Society/SaveSocietyProfiling", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                               
                openCity(event, 'SocietyAdministrative');                
                //$("#BtnSocietyDetails").prop('disabled', true);
                //LoaderStart();
                //$('#modelAlertMessage').html('');
                //var errorLst = [];
                //for (var i = 0; i < Object.keys(data).length; i++) {
                //    errorLst.push(data.data[i]);
                //}
                //var errorMsg = errorLst.join("</br>");
                //ShowModelAlert("User Registration", errorMsg);
                //if (data.Status) {
                //    $scope.ClearForm();
                //    $scope.UserList();
                //}
                //LoaderStop();
            }).
            error(function (data, status) {

                ShowModelAlert("Society Profiling", "Error While Society Profiling Insert");
            });

    };
    // END :Save Society Profiling

    // START : Clear Society Profile Section
    $scope.ClearSocietyDetailsForm = function () {
        $scope.SocietyRegistration.SocietyName = '';
        $scope.SocietyRegistration.SocietyTypeId = '0';
        $scope.SocietyRegistration.BusinessId = '0';
        $scope.SocietyRegistration.LiablityId = '0';
        $scope.SocietyRegistration.MemberLiablity = '';
        $scope.SocietyRegistration.ClassId = '0';
        $scope.SubClassList = [];
        $scope.SocietyRegistration.SubClassId = '0';
        $scope.SocietyRegistration.TotalMembersApplyingForSocociety = '';
        $scope.SocietyRegistration.EntranceFeesCollected = '';
        $scope.SocietyRegistration.SingleShareValue = '';
        $scope.SocietyRegistration.PaidUpShareCapital = '';
        $scope.SocietyRegistration.ByeLawsSocietyFile = '';
        $scope.SocietyRegistration.EconomyViabilityFile = '';
        $scope.SocietyRegistration.GeneralMeetingDate = '';
        $scope.SocietyRegistration.MembersParticipatingGeneralMeeting = '';
        $scope.SocietyRegistration.GeneralMeetingProceeding = '';
        $scope.SocietyRegistration.BalanceSheetFile = '';
    };
    // END : Clear Society Profile Section

    //START : Get Occupation List
    $scope.GetOccupationList = function () {
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=OCCUPATION&MasterCode=-1").
            success(function (data, status) {
                $scope.OccupationList = data;
            }).
            error(function (data, status) {
                $scope.OccupationList = [];
            });
    };
    //START : Get Occupation List

    //START : Get Member Type List
    $scope.GetMemberTypelistList = function () {
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=MEMBERTYPE&MasterCode=-1").
            success(function (data, status) {
                $scope.MemberTypelistList = data;
            }).
            error(function (data, status) {
                $scope.MemberTypelistList = [];
            });
    };
    //START : Get Get Member Type List

    //START : Get Gender List
    $scope.GetGenderList = function () {
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=Gender&MasterCode=-1").
            success(function (data, status) {
                $scope.GenderList = data;
            }).
            error(function (data, status) {
                $scope.GenderList = [];
            });
    };
    //START : Get Gender List

    //START : Get State List
    $scope.GetStateList = function () {
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=STATE&MasterCode=-1").
            success(function (data, status) {
                $scope.Statelist = data;
            }).
            error(function (data, status) {
                $scope.Statelist = [];
            });
    };
    //START : Get State List

    //START : Get District List
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.StateId).
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    //START : Get District List

    //START : Get City List
    $scope.GetCityList = function () {
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=CITY&MasterCode=" + $scope.DistrictId).
            success(function (data, status) {
                $scope.CityList = data;
            }).
            error(function (data, status) {
                $scope.CityList = [];
            });
    };
    //START : Get City List

    //START : Get Ward List
    $scope.GetWardList = function () {
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.CityId).
            success(function (data, status) {
                $scope.WardList = data;
            }).
            error(function (data, status) {
                $scope.WardList = [];
            });
    };
    //START : Get Ward List

    //START : Get Cast List
    $scope.GetCastList = function () {
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=CAST&MasterCode=-1").
            success(function (data, status) {
                $scope.CastList = data;
            }).
            error(function (data, status) {
                $scope.CastList = [];
            });
    };
    //START : Get Cast List

    //START : Get Category List
    $scope.GetCategoryList = function () {
        $http.get(SiteUrl+"/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=" + 1).
            success(function (data, status) {
                $scope.CategoryList = data;
            }).
            error(function (data, status) {
                $scope.CategoryList = [];
            });
    };
    //START : Get Category List

    // START : Save Authorized Person Details
    $scope.SaveSocietyAuthorizedPersonnelDetails = function () {

        $http.post(SiteUrl+"/Society/SaveSocietyAuthorizedPersonnelDetail", $scope.SocietyPersonnel).
            success(function (data, status) {               
                openCity(event, 'btnSocietyOfficeDetails');
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });

    };
    // END : Save Authorized Person Details

    // START : Clear Authorized Person Details
    $scope.ClearSocietyPersonnelDetailsForm = function () {
        $scope.SocietyPersonnel.FirstName = '';
        $scope.SocietyPersonnel.LastName = '';
        $scope.SocietyPersonnel.DOB = '';
        $scope.SocietyPersonnel.IsRural = false;
        $scope.SocietyPersonnel.GENDER_ID = '0';
        $scope.SocietyPersonnel.HouseBuildingNo = '';
        $scope.SocietyPersonnel.LaneStreetName = '';
        $scope.StateId = '0';
        $scope.DistrictList = [];
        $scope.DistrictId = '0';
        $scope.CityList = [];
        $scope.CityId = '0';
        $scope.WardList = [];
        $scope.SocietyPersonnel.WARD_ID = '0';
        $scope.SocietyPersonnel.PinCode = '';
        $scope.SocietyPersonnel.TelNoWithSTDCode = '';
        $scope.SocietyPersonnel.FaxNoWithSTDCode = '';
        $scope.SocietyPersonnel.MobileNo = '';
        $scope.SocietyPersonnel.EmaiId = '';
    };
    // END : Clear Authorized Person Details

    // START : Save Registered Office Details
    $scope.SaveRegisteredOfficeDetails = function () {       
        $http.post(SiteUrl+"/Society/SaveSocietyRegisteredOfficeDetail", $scope.SocietyRegisteredOffice).
            success(function (data, status) {            
                if (data > 0) {
                    ShowModelAlert("Society Registered Office Details", "Society Registered Office Details Save Sucessfully");
                }

            }).
            error(function (data, status) {
                $scope.SocietyRegisteredOffice = [];
                ShowModelAlert("Society Registered Office Detail", "Error While Insert Society Registered Office Detail ");
            });
    };
    // END : Save Registered Office Details
    
    // START : Clear Registered Office Details
    $scope.ClearSocietyOfficeDetailsForm = function () {
        $scope.SocietyRegisteredOffice.HouseBuildingNo = '';
        $scope.SocietyRegisteredOffice.LaneStreetName = '';
        $scope.SocietyRegisteredOffice.Locality = '';
        $scope.StateId = '0';
        $scope.DistrictList = [];
        $scope.DistrictId = '0';
        $scope.CityList = [];
        $scope.CityId = '0';
        $scope.WardList = [];
        $scope.SocietyRegisteredOffice.WARD_ID = '0';
        $scope.SocietyRegisteredOffice.PinCode = '';
        $scope.SocietyRegisteredOffice.AreaOfOperation = '';
    };
    // END : Clear Registered Office Details

    // START : Save Society Bearer Details
    $scope.SaveSocietyBearer = function () {
        var formData = new FormData();

        var ddMMyyyyDate = new Date($scope.SocietyBearer.DOB);
        var dd = ddMMyyyyDate.getDate();
        var mm = ddMMyyyyDate.getMonth() + 1; //January is 0!

        var yyyy = ddMMyyyyDate.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }


        formData.append("Name", $scope.SocietyBearer.Name);
        formData.append("Aadhaar", $scope.SocietyBearer.Aadhaar);
        formData.append("GenderId", $scope.SocietyBearer.GenderId);
        formData.append("DOB", (dd + '/' + mm + '/' + yyyy));
        formData.append("OccupationId", $scope.SocietyBearer.OccupationId);
        formData.append("MemberTypeId", $scope.SocietyBearer.MemberTypeId);
        formData.append("MAddress", $scope.SocietyBearer.MAddress);
        formData.append("Photo", $scope.BearerFile);


        $http.post(SiteUrl+"/Society/SocietyBearerInsert", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                alert(data);
            }).
            error(function (data, status) {

                ShowModelAlert("Society Bearer Details", "Error While Society Bearer Insert");
            });
    };
    // END : Save Society Bearer Details

    // START : Clear Society Bearer Details
    $scope.ClearSocietyBearerDetailsForm = function () {
        $scope.SocietyBearer.Name = '';
        $scope.SocietyBearer.Aadhaar = '';
        $scope.SocietyBearer.GenderId = '0';
        $scope.SocietyBearer.DOB = '';
        $scope.SocietyBearer.OccupationId = '0';
        $scope.SocietyBearer.MemberTypeId = '0';
        $scope.SocietyBearer.MAddress = '';
        $scope.SocietyBearer.Photo = '';
    };
    // END : Clear Society Bearer Details


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
        formData.append("IsApprovedByRegistrar", $scope.SocietyAuditReport.IsApprovedByRegistrar);
        formData.append("IsSocietyWorking", $scope.SocietyAuditReport.IsSocietyWorking);
        formData.append("FinancialYear", $scope.SocietyAuditReport.FinancialYear);
        formData.append("AuditReport", $scope.SocietyAuditFile);

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
    };
    // END : Clear Society Audit Report Details

    // START : Save Bulk Insert
    $scope.SaveBulkInsert = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.RegistrationCertificateFile == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Upload Excel Sheet' : '\nकृपया एक्सेल शीट अपलोड करें |';
            if (message.length > 27) {
                ShowModelAlert("Bulk Insert", message);
                return false;
            }
        }

        var formData = new FormData();
        formData.append("ExcelSheetFile", $scope.ExcelSheetFile);
        $http.post(SiteUrl+"/Society/InsertSocityByExcel", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                alert(data);

            }).
            error(function (data, status) {
                ShowModelAlert("Bulk Insert", "Error While Upload Excel Sheet");
            });
    };
    // END :Save Society Profiling


     // START : Society Registration Report

    $scope.SocietyRegReport = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.SocietyRegDetailListData = null;     
        $http.post(SiteUrl + "/Society/SocietyRegReport", $scope.SocietyRegistration).
            success(function (data, status) {
                //alert(JSON.stringify(data));
                console.log('Function Start');
                console.log(data);
                $scope.SocietyRegDetailListData = '';
                $scope.SocietyRegDetailListData = data;
               // $scope.SocietyRegDetailListData = $scope.SocietyRegDetailListData;
               // alert($scope.SocietyRegDetailListData);

                //$scope.SocietyRegDetailListData = data.Table;
                //$scope.SocietyRegDetailListData = $scope.SocietyRegDetailListData[0];
                //$scope.SocietyRegDetailListData = data;
                $scope.colsort = "";
                $scope.reverseSort = false;
                $(".se-pre-con").fadeOut("slow");
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
                $scope.SocietyRegDetailListData = [];
            });
        $(".se-pre-con").fadeOut("slow");
    }


     // END : Society Registration Report

    //START : Get Society District List
    $scope.GetSocietyDistrictList = function () {
        $http.get(SiteUrl + "/Society/GetSocietyDistrictList").
            success(function (data, status) {
                $scope.Districtlist = data;
            }).
            error(function (data, status) {
                $scope.Districtlist = [];
            });
    };

    $scope.GetSocietyActList = function () {
        $http.get(SiteUrl + "/Society/GetSocietyActList").
            success(function (data, status) {
                $scope.Actlist = data;
            }).
            error(function (data, status) {
                $scope.Actlist = [];
            });
    };

    $scope.GetSocietyTypeList = function () {
        $http.get(SiteUrl + "/Society/GetSocietyTypeList").
            success(function (data, status) {
                $scope.Typelist = data;
            }).
            error(function (data, status) {
                $scope.Typelist = [];
            });
    };

    $scope.GetSocietyYearList = function () {
        $http.get(SiteUrl + "/Society/GetSocietyYearList").
            success(function (data, status) {
                $scope.YearList = data;
            }).
            error(function (data, status) {
                $scope.YearList = [];
            });
    };

    $scope.GetSocietyMonthList = function () {
        $http.get(SiteUrl + "/Society/GetSocietyMonthList").
            success(function (data, status) {
                $scope.MonthList = data;
            }).
            error(function (data, status) {
                $scope.MonthList = [];
            });
    };

  


    // START : Call Functions on Page Load
    $scope.OnPageLoad();
    $scope.GetGenderList();
    $scope.GetStateList();
    $scope.GetCastList();
    $scope.GetCategoryList();
    $scope.GetOccupationList();
    $scope.GetMemberTypelistList();
    $scope.SocietyRegReport();
    $scope.GetSocietyDistrictList();
    $scope.GetSocietyActList();
    $scope.GetSocietyTypeList();
    $scope.GetSocietyYearList();
    $scope.GetSocietyMonthList();
    //END : Call Functions on Page Load
});

