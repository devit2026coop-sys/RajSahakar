
var MasterMod = angular.module('AuditorReqModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("AuditorReqCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.AuditorMasterData = [];
    $scope.AuditorRequest = [];
    $scope.AuditorRequest.IsFirm = 0;
    $scope.AuditorRequest.PhoneNo = '';
    $scope.AuditorRequest.SSO_ID = '';
    $scope.IsFirm = false;


    // START : Get Auditor Data
    $scope.IsFirmYN = function () {
        $scope.IsFirm = $scope.AuditorRequest.IsFirm == 1 ? true : false;
        if ($scope.AuditorRequest.IsFirm == 1) {
            $scope.title = _language == 'en-us' ? ('Firm Name') : ('फर्म का नाम');
            $scope.Firm = _language == 'en-us' ? ('Firm Address') : ('फर्म का पता');
            $scope.FirmPin = _language == 'en-us' ? ('Firm Pin Code') : ('फर्म पिन कोड');
            $scope.Partnership = true;
        }
        else {

            $scope.title = _language == 'en-us' ? ('Name') : ('नाम');
            $scope.Firm = _language == 'en-us' ? ('Address') : ('पता');
            $scope.FirmPin = _language == 'en-us' ? ('Pin Code') : ('पिन कोड');
            $scope.AuditorRequest.BranchAddress = "";
            $scope.AuditorRequest.BranchPinCode = "";
            $scope.Partnership = false;
        }
    }

    $scope.IsFirmType = function () {
        $scope.Partnership = $scope.AuditorRequest.FirmType == 1 ? true : false;
    }

    $scope.ShowApplicationForm = function () {
        $scope.RequestForm = true;
    }

    

    // START :Get List of Districts
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=-1").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };
    // END :Get List of Districts
    // END : Get Auditor Data
    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = 'en-us';
       
        //$(".se-pre-con").fadeIn("slow");
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
        $scope.GetDistrictList();
        $('#txtRegistrationDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $scope.title = _language == 'en-us' ? ('Name') : ('नाम');
        $scope.DivSearch = true;
        $scope.RequestForm = false;
        $scope.GetAuditorDataList();

       // $(".se-pre-con").fadeOut("slow");
    };

    $scope.SetStatus = function (UnitId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Master/ChangeStatusunit", { Id: UnitId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllSubUnitList();

            }).
            error(function (data, status) {
            });
    };

    $scope.ClearOTPForm = function () {
        $scope.OtpText = '';
    };

    $scope.OtpVerify = function () {
        $scope.SaveAuditorRequest();
    };

    $scope.SendOtp = function () {
        var Msg = '';


        if ($('#txtRegistrationDate').val() == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Registration Date \n' : 'कृपया पंजीकरण तिथि दर्ज करें\n');
        }


        if ($scope.AuditorRequest.FirmName == undefined || $scope.AuditorRequest.FirmName == '') {
            if ($scope.AuditorRequest.IsFirm == 1)
                Msg += (_language == 'en-us' ? 'Please Enter Firm Name \n' : 'कृपया फर्म का नाम दर्ज करें\n');
            else if ($scope.AuditorRequest.IsFirm == 0) {
                Msg += (_language == 'en-us' ? 'Please Enter Name \n' : 'कृपया नाम दर्ज करें\n');
            }
        }

        if ($scope.AuditorRequest.MobileNo == undefined || $scope.AuditorRequest.MobileNo == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Mobile No. \n' : 'कृपया मोबाइल नंबर दर्ज करें\n');
        }

        if ($scope.AuditorRequest.ICFAIRegistrationNo == undefined || $scope.AuditorRequest.ICFAIRegistrationNo == '') {
            Msg += (_language == 'en-us' ? 'Please Enter ICFAI Registration No. \n' : 'कृपया ICFAI पंजीकरण संख्या दर्ज करें\n');
        }

        if ($scope.AuditorRequest.RBIPanelUniqueCode == undefined || $scope.AuditorRequest.RBIPanelUniqueCode == '') {
            Msg += (_language == 'en-us' ? 'RBI Panel Unique Code \n' : 'RBI पैनल यूनिक कोड दर्ज करें\n');
        }

        if ($scope.AuditorRequest.EnrollmentNo == undefined || $scope.AuditorRequest.EnrollmentNo == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Enrollment No. \n' : 'कृपया एनरोलमेंट नंबर दर्ज करें\n');
        }

        if ($scope.AuditorRequest.IsFirm == 1 || $scope.AuditorRequest.FirmType == 1) {

            if ($scope.AuditorRequest.NoOfPartners == undefined || $scope.AuditorRequest.NoOfPartners == '') {
                Msg += (_language == 'en-us' ? 'Please Enter No Of Partners. \n' : 'कृपया भागीदार की संख्या दर्ज करें\n');
            }
            if ($scope.AuditorRequest.BranchAddress == undefined || $scope.AuditorRequest.BranchAddress == '') {
                Msg += (_language == 'en-us' ? 'Please Enter Branch Address. \n' : 'कृपया शाखा का पता दर्ज करें\n');
            }
            if ($scope.AuditorRequest.BranchPinCode == undefined || $scope.AuditorRequest.BranchPinCode == '') {
                Msg += (_language == 'en-us' ? 'Please Enter Branch PinCode. \n' : 'कृपया शाखा पिनकोड दर्ज करें\n');
            }

        }

        if (Msg.length > 1) {
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + Msg;
            alert(Msg);
            
            //$('#myModal').css("display", "none");
            //$('#myModal').modal().hide();           
            //$("#myModal").modal('hide');
           

            
            $('#myModal').modal('hide');

            return false;
        }
        else
            $('#myModal').modal('show');

        //$(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Auditor/SendOtp/").
            success(function (data, status) {
                if (data.length > 0)
                    var temp = data.split('#');
                if (temp.length > 0) {
                    if (temp[0] == "200") {
                        $scope.ValidateotpValue = temp[1];
                    }
                }
                else {
                    $scope.RequestForm = true;
                    $scope.AuditorRequest.AuditorId = 0;
                }
            }).
            error(function (data1, status) {
            });
        //$(".se-pre-con").fadeOut("slow");
    };



    $scope.GetAuditorData = function () {
        if ($scope.AuditorRequest.SearchDeatils == undefined || $scope.AuditorRequest.SearchDeatils == '') {
            var Msg = "";
            Msg += (_language == 'en-us' ? 'Please Enter Enrollment No. or ICFAI Registration No.\n' : 'कृपया एनरोलमेंट नंबर या ICFAI पंजीकरण नंबर दर्ज करें\n');
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + Msg;
            alert(Msg);
            return false;
        }
        $http.post(SiteUrl + "/Society/GetAuditData/", { IsAuditFirm: "", "DistrictId": 0, "Search": $scope.AuditorRequest.SearchDeatils }).
            success(function (data, status) {
                if (data.length > 0)
                    $scope.FillData(data);
                else {
                    $scope.RequestForm = true;
                    $scope.AuditorRequest.AuditorId = 0;
                    alert((_language == 'en-us' ? 'Your Record Not Found In Our System kindly check your Number or fill new Request.\n' : 'हमारे सिस्टम में आपका रिकॉर्ड नहीं मिला कृपया अपना नंबर जांचें या नया अनुरोध भरें\n'));
                }
            }).
            error(function (data1, status) {
            });
        //$(".se-pre-con").fadeOut("slow");
    }
    $scope.FillData = function (Obj) {
        var data = Obj[0];
        $scope.AuditorRequest.IsFirm = data.IsFirm == 'True' ? 1 : 0;
        $scope.IsFirmYN();
        $scope.AuditorRequest.FirmType = data.FirmTypeId;
        $scope.IsFirmType();
        $scope.AuditorRequest.FirmName = data.FirmName;
        $scope.AuditorRequest.DistrictId = data.DistrictId;
        $scope.AuditorRequest.FirmAddress = data.FirmAddress;
        $scope.AuditorRequest.FirmPinCode = data.PINCode;
        $scope.AuditorRequest.NoOfPartners = data.NoOfPartners;
        $scope.AuditorRequest.BranchAddress = data.BranchAddress;
        $scope.AuditorRequest.BranchPinCode = data.BranchPinCode;
        //alert(data.AuditorId);
        $scope.AuditorRequest.AuditorId = data.AuditorId;
        var Ph = data.MobileNo.split('#');
        if (Ph.length > 2) {
            $scope.AuditorRequest.MobileNo = Ph[1];
            $scope.AuditorRequest.PhoneNo = Ph[0];
        }
        if (Ph.length > 1) {
            $scope.AuditorRequest.MobileNo = Ph[1];
            $scope.AuditorRequest.PhoneNo = Ph[0];
        }
        else {
            $scope.AuditorRequest.MobileNo = data.MobileNo;
            $scope.AuditorRequest.PhoneNo = data.PhoneNo;
        }

        $scope.AuditorRequest.ICFAIRegistrationNo = data.ICFAIRegistrationNo;
        $('#txtRegistrationDate').val(data.RegistrationDate)
        $scope.AuditorRequest.RBIPanelUniqueCode = data.RBIPanelUniqueCode;
        $scope.AuditorRequest.EnrollmentNo = data.EnrollmentNo;
        $scope.RequestForm = true;
    }

    $scope.BackToPage = function () {

    }
    $scope.SaveAuditorRequest = function () {

        var Msg = '';


        if ($('#txtRegistrationDate').val() == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Registration Date \n' : 'कृपया पंजीकरण तिथि दर्ज करें\n');
        }


        if ($scope.AuditorRequest.FirmName == undefined || $scope.AuditorRequest.FirmName == '') {
            if ($scope.AuditorRequest.IsFirm == 1)
                Msg += (_language == 'en-us' ? 'Please Enter Firm Name \n' : 'कृपया फर्म का नाम दर्ज करें\n');
            else if ($scope.AuditorRequest.IsFirm == 0) {
                Msg += (_language == 'en-us' ? 'Please Enter Name \n' : 'कृपया नाम दर्ज करें\n');
            }
        }

        if ($scope.AuditorRequest.MobileNo == undefined || $scope.AuditorRequest.MobileNo == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Mobile No. \n' : 'कृपया मोबाइल नंबर दर्ज करें\n');
        }

        if ($scope.AuditorRequest.ICFAIRegistrationNo == undefined || $scope.AuditorRequest.ICFAIRegistrationNo == '') {
            Msg += (_language == 'en-us' ? 'Please Enter ICFAI Registration No. \n' : 'कृपया ICFAI पंजीकरण संख्या दर्ज करें\n');
        }

        if ($scope.AuditorRequest.RBIPanelUniqueCode == undefined || $scope.AuditorRequest.RBIPanelUniqueCode == '') {
            Msg += (_language == 'en-us' ? 'RBI Panel Unique Code \n' : 'RBI पैनल यूनिक कोड दर्ज करें\n');
        }

        if ($scope.AuditorRequest.EnrollmentNo == undefined || $scope.AuditorRequest.EnrollmentNo == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Enrollment No. \n' : 'कृपया एनरोलमेंट नंबर दर्ज करें\n');
        }

        if ($scope.AuditorRequest.IsFirm == 1 || $scope.AuditorRequest.FirmType == 1) {

            if ($scope.AuditorRequest.NoOfPartners == undefined || $scope.AuditorRequest.NoOfPartners == '') {
                Msg += (_language == 'en-us' ? 'Please Enter No Of Partners. \n' : 'कृपया भागीदार की संख्या दर्ज करें\n');
            }
            if ($scope.AuditorRequest.BranchAddress == undefined || $scope.AuditorRequest.BranchAddress == '') {
                Msg += (_language == 'en-us' ? 'Please Enter Branch Address. \n' : 'कृपया शाखा का पता दर्ज करें\n');
            }
            if ($scope.AuditorRequest.BranchPinCode == undefined || $scope.AuditorRequest.BranchPinCode == '') {
                Msg += (_language == 'en-us' ? 'Please Enter Branch PinCode. \n' : 'कृपया शाखा पिनकोड दर्ज करें\n');
            }

        }

        if (Msg.length > 1) {
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + Msg;
            alert(Msg);
            return false;
        }
        $scope.AuditorRequest.OtpVal = $scope.OtpText;
        $scope.AuditorRequest.OtpValEnc = $scope.ValidateotpValue;
        $scope.AuditorRequest.RegistrationDate = $('#txtRegistrationDate').val().split("/").reverse().join("-");
        var formData = new FormData();
        //alert($scope.AuditorRequest.AuditorId);
        formData.append("AuditorId",       $scope.AuditorRequest.AuditorId);
        formData.append("IsFirm",           $scope.AuditorRequest.IsFirm);
        formData.append("FirmType",         $scope.AuditorRequest.FirmType);
        formData.append("FirmName",         $scope.AuditorRequest.FirmName);
        formData.append("FirmAddress",      $scope.AuditorRequest.FirmAddress);
        formData.append("DistrictId",       $scope.AuditorRequest.DistrictId);
        //formData.append("StateId",        $scope.AuditorRequest.StateId);
        formData.append("FirmPinCode",      $scope.AuditorRequest.FirmPinCode);
        formData.append("NoOfPartners",     $scope.AuditorRequest.NoOfPartners);
        formData.append("BranchAddress",    $scope.AuditorRequest.BranchAddress);
        formData.append("BranchPinCode",    $scope.AuditorRequest.BranchPinCode);
        formData.append("MobileNo", $scope.AuditorRequest.MobileNo);
        if ($scope.AuditorRequest.PhoneNo != undefined) {
            formData.append("PhoneNo", $scope.AuditorRequest.PhoneNo);
        }
        else {
            formData.append("PhoneNo", ""); 
        }
      formData.append("ICFAIRegistrationNo",$scope.AuditorRequest.ICFAIRegistrationNo);
        formData.append("RegistrationDate", $scope.AuditorRequest.RegistrationDate);
       formData.append("RBIPanelUniqueCode",$scope.AuditorRequest.RBIPanelUniqueCode);
        formData.append("EnrollmentNo",     $scope.AuditorRequest.EnrollmentNo);
        formData.append("SSO_ID",           $scope.AuditorRequest.SSO_ID);
        formData.append("OtpVal",           $scope.AuditorRequest.OtpVal);
        formData.append("OtpValEnc", $scope.AuditorRequest.OtpValEnc);
        $http.post(SiteUrl + "/Auditor/SaveAuditorRequest",  formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                var message = '';
                if (data > 0 && $scope.AuditorRequest.AuditorId == 0) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    window.location.reload();
                }
                else if (data > 0 && $scope.AuditorRequest.AuditorId > 0) {
                    message = _language == 'en-us' ? 'Record update successfully\n' : 'रिकॉर्ड सफलतापूर्वक अपडेट किया गया है\n';
                    alert(message);
                    window.location.reload();
                }
                else if (data == -2) {
                    message = _language == 'en-us' ? 'Record already exists\n' : 'रिकॉर्ड पहले से मौजूद है\n';
                    alert(message);
                    window.location.reload();
                }
                else if (data == -1) {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                }                
                else if (data == -5) {
                    message = _language == 'en-us' ? 'Invalid otp \n' : 'अमान्य ओ.टी.पी.\n';
                    alert(message);
                }
                //alert(message);
               
                //
            }).
            error(function (data, status) {
                alert("Error while Auditor Request");
            });
    };
    $scope.Clear = function () {
        $scope.AuditorRequest.FirmName = '';
        $scope.AuditorRequest.DistrictId = 0;
        $scope.AuditorRequest.FirmAddress = '';
        $scope.AuditorRequest.FirmPinCode = '';
        $scope.AuditorRequest.MobileNo = '';
        $scope.AuditorRequest.PhoneNo = '';
        $scope.AuditorRequest.ICFAIRegistrationNo = '';
        $('#txtRegistrationDate').val('');
        $scope.AuditorRequest.RBIPanelUniqueCode = '';
        $scope.AuditorRequest.EnrollmentNo = '';
        $scope.RequestForm = false;
        $scope.DivSearch = true;
        $scope.Partnership = false;
        $scope.IsFirm = false;
    }

    $scope.GetAuditorDataList = function () {
        $http.post(SiteUrl + "/Auditor/GetAuditorList").
            success(function (data, status) {
                $scope.AuditorData = [];
                $scope.AuditorData = data;
            }).
            error(function (data, status) {
                $scope.AuditorData = [];
            });
    }
    $scope.update = function (item) {


    };

    $scope.OnPageLoad();
    $scope.IsFirmYN();


});






