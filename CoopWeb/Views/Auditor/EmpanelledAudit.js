
var MasterMod = angular.module('EmpanelledAuditModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("EmpanelledAuditCont", function ($scope, $http) {

    // SCOPE
    _language = 'hi-in';
    $scope._language = 'hi-in';
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
    $scope.IsShowPreview = false;
    $scope.ApplicantName = "";
    $scope.ApppicantFathaerName = "";
    $scope.ApplicantAddress = ""
    $scope.ShowApply = true;
    // START : Code for Language label setting
    $scope.getTranslation = function () {
        _language = 'hi-in';
        $scope._language = _language;

        if (_language === 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                    var addressdataset1 = {
                        Housebuilding: "",
                        State: "6",
                        District: "  ",
                        Lanestreet: "",
                        Locality: "",
                        Pincode: ""
                    }
                    SetAddressinfo($scope.translation, 'MemberAdd', addressdataset1, $http);
                }).
                error(function (data, status) {
                    alert("Error while translation-En");
                });
        }
        else {
            $http.get("/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                    var addressdataset1 = {
                        Housebuilding: "",
                        State: "6",
                        District: "",
                        Lanestreet: "",
                        Locality: "",
                        Pincode: ""
                    }
                    SetAddressinfo($scope.translation, 'MemberAdd', addressdataset1, $http);
                }).
                error(function (data, status) {
                    alert("Error while translation-Hi");
                });
        }
    };
    $scope.getTranslation();
    // END : Code for Language label setting

    $scope.OtherPersonList = [];
    $scope.TraningDetailsList = [];
    $scope.AuditFirmList = [];
    $scope.PartnerList = [];



    //$scope.OtherPersonList.push({ "PersonId": 0, "Name": "", "Address": "", "Post": "" });
    //$scope.TraningDetailsList.push({ "AssistantName": "", "Education": "", "Experiance": "" });
    //$scope.AuditFirmList.push({ "Name": "", "MobileNo": "" });
    //$scope.PartnerList.push({ "Name": "", "PartnerName": "", "MembershipNo": "", "MembershipDate": "" });

    $scope.OtherPersonList.push({ "PersonId": 0, "Name": "", "Address": "", "Post": "" });
    $scope.TraningDetailsList.push({ "FirmParemenentTraniningId": 0, "AssistantName": "", "Education": "", "Experiance": "" });
    $scope.AuditFirmList.push({ "AuditfirmAddressId": 0, "Name": "", "MobileNo": "" });
    $scope.PartnerList.push({ "AuditPartnershipId": 0, "Name": "", "PartnerName": "", "MembershipNo": "", "MembershipDate": "", "IsUploadAuditCertificateFile": false, "AuditCertificateFileName": "", "IsUploadMembershipCertificateFile": false, "MembershipCertificateFileName": "", "IsUploadAuditCertificateFile": false, "AuditCertificateFileName": "" });




    $scope.AddTraningDetailsList = function () {

        if ($scope.ValidateTraningDetailsList()) {
            $scope.TraningDetailsList.push({ "PersonId": 0, "Name": "", "Address": "", "Post": "" });
        }

    }

    $scope.RemoveTraningDetailsListRow = function (index) {
        $scope.TraningDetailsList.splice(index, 1);
        $scope.$apply();
    }

    $scope.ifyesval = function (id) {
        if (id == 1)
            $scope.Truefile = true;
        else
            $scope.Truefile = false;
    }


    $scope.ValidateTraningDetailsList = function (status) {
        debugger;
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';


        if (status == false) {
            $scope.TraningDetailsList.forEach(function (item, index) {

                if (item.AssistantName != "") {
                    if (item.Education == "") {
                        if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Education Details' : 'कृपया शिक्षा विवरण दर्ज करें') < 0) {
                            message += $scope._language == 'en-us' ? '<br />Please Enter Education Details.' : '<br />कृपया शिक्षा विवरण दर्ज करें।';
                        }
                        $("#txtEducation_" + index).addClass("notValid");
                    }
                    else {
                        $("#txtEducation_" + index).removeClass("notValid");
                    }
                    if (item.Experiance == "") {
                        if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Experiance' : 'कृपया प्रयोग दर्ज करें') < 0) {
                            message += $scope._language == 'en-us' ? '<br />Please Enter Experiance' : '<br />कृपया प्रयोग दर्ज करें।';
                        }
                        $("#txtExperiance_" + index).addClass("notValid");
                    }
                    else {
                        $("#txtExperiance_" + index).removeClass("notValid");
                    }

                }
                else if (item.Education != "") {
                    if (!item.AssistantName) {
                        if (message.indexOf($scope._language == 'en-us' ? 'Please enter Assistant Name.' : 'कृपया सहायक नाम दर्ज करें।') < 0) {
                            message += $scope._language == 'en-us' ? '<br />Please enter Assistant Name.' : '<br />कृपया सहायक नाम दर्ज करें।';
                        }
                        $("#txtAssistantName_" + index).addClass("notValid");
                    }
                    else {
                        $("#txtAssistantName_" + index).removeClass("notValid");
                    }
                    if (item.Experiance == "") {
                        if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Experiance' : 'कृपया प्रयोग दर्ज करें') < 0) {
                            message += $scope._language == 'en-us' ? '<br />Please Enter Experiance' : '<br />कृपया प्रयोग दर्ज करें।';
                        }
                        $("#txtExperiance_" + index).addClass("notValid");
                    }
                    else {
                        $("#txtExperiance_" + index).removeClass("notValid");
                    }
                }
                else if (item.Experiance != "") {
                    if (!item.AssistantName) {
                        if (message.indexOf($scope._language == 'en-us' ? 'Please enter Assistant Name.' : 'कृपया सहायक नाम दर्ज करें।') < 0) {
                            message += $scope._language == 'en-us' ? '<br />Please enter Assistant Name.' : '<br />कृपया सहायक नाम दर्ज करें।';
                        }
                        $("#txtAssistantName_" + index).addClass("notValid");
                    }
                    else {
                        $("#txtAssistantName_" + index).removeClass("notValid");
                    }
                    if (item.Education == "") {
                        if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Education Details' : 'कृपया शिक्षा विवरण दर्ज करें') < 0) {
                            message += $scope._language == 'en-us' ? '<br />Please Enter Education Details.' : '<br />कृपया शिक्षा विवरण दर्ज करें।';
                        }
                        $("#txtEducation_" + index).addClass("notValid");
                    }
                    else {
                        $("#txtEducation_" + index).removeClass("notValid");
                    }
                }
            });
        }
        else {
            $scope.TraningDetailsList.forEach(function (item, index) {

                if (!item.AssistantName) {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter Assistant Name.' : 'कृपया सहायक नाम दर्ज करें।') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please enter Assistant Name.' : '<br />कृपया सहायक नाम दर्ज करें।';
                    }
                    $("#txtAssistantName_" + index).addClass("notValid");
                }
                else {
                    $("#txtAssistantName_" + index).removeClass("notValid");
                }
                if (item.Education == "") {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Education Details' : 'कृपया शिक्षा विवरण दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please Enter Education Details.' : '<br />कृपया शिक्षा विवरण दर्ज करें।';
                    }
                    $("#txtEducation_" + index).addClass("notValid");
                }
                else {
                    $("#txtEducation_" + index).removeClass("notValid");
                }
                if (item.Experiance == "") {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Experiance' : 'कृपया प्रयोग दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please Enter Experiance' : '<br />कृपया प्रयोग दर्ज करें।';
                    }
                    $("#txtExperiance_" + index).addClass("notValid");
                }
                else {
                    $("#txtExperiance_" + index).removeClass("notValid");
                }


            });
        }

        if (message.length > 29) {
            ShowOkMessage("Society Complaint ", message, function () { });
            return false;
        }
        return true;
    }


    // ------------------------AuditFirmList-----------


    $scope.AddAuditFirmList = function () {
        if ($scope.ValidateAuditFirmList()) {
            $scope.AuditFirmList.push({ "Name": "", "MobileNo": "" });
        }
    }

    $scope.RemoveAuditFirmListRow = function (index) {
        $scope.AuditFirmList.splice(index, 1);
        $scope.$apply();
    }


    $scope.ValidateAuditFirmList = function (status) {
        debugger;
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if (status == false) {
            $scope.AuditFirmList.forEach(function (item, index) {

                if (item.Name != "") {
                    if (item.MobileNo == "") {
                        if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Audit Person MobileNo.' : 'कृपया ऑडिट व्यक्ति का मोबाइल नं डालें।') < 0) {
                            message += $scope._language == 'en-us' ? '<br />Please Enter Audit Person MobileNo.' : '<br />कृपया ऑडिट पर्सन का मोबाइल नं डालें।';
                        }
                        $("#txtMobileNo_" + index).addClass("notValid");
                    }
                    else {
                        $("#txtMobileNo_" + index).removeClass("notValid");
                    }
                }
                else if (item.MobileNo != "") {
                    if (!item.Name) {
                        if (message.indexOf($scope._language == 'en-us' ? 'Please enter Audit Person Name.' : 'कृपया ऑडिट पर्सन नेम डालें।') < 0) {
                            message += $scope._language == 'en-us' ? '<br />Please enter Audit Person Name.' : '<br />कृपया ऑडिट पर्सन नेम डालें।';
                        }
                        $("#txtName_" + index).addClass("notValid");
                    }
                    else {
                        $("#txtName_" + index).removeClass("notValid");
                    }
                }


            });
        }
        else {
            $scope.AuditFirmList.forEach(function (item, index) {



                if (!item.Name) {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter Audit Person Name.' : 'कृपया ऑडिट पर्सन नेम डालें।') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please enter Audit Person Name.' : '<br />कृपया ऑडिट पर्सन नेम डालें।';
                    }
                    $("#txtName_" + index).addClass("notValid");
                }
                else {
                    $("#txtName_" + index).removeClass("notValid");
                }
                if (item.MobileNo == "") {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Audit Person MobileNo.' : 'कृपया ऑडिट व्यक्ति का मोबाइल नं डालें।') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please Enter Audit Person MobileNo.' : '<br />कृपया ऑडिट पर्सन का मोबाइल नं डालें।';
                    }
                    $("#txtMobileNo_" + index).addClass("notValid");
                }
                else {
                    $("#txtMobileNo_" + index).removeClass("notValid");
                }
            });
        }

        if (message.length > 29) {
            ShowOkMessage("Auditor Request", message, function () { });
            return false;
        }
        return true;
    }

    // ------------------------PartnerList-----------


    $scope.AddPartnerList = function () {

        if ($scope.ValidatePartnerList()) {
            // $scope.PartnerList.push({ "Name": "", "PartnerName": "", "MembershipNo": "", "MembershipDate": "" });
            $scope.PartnerList.push({ "AuditPartnershipId": 0, "Name": "", "PartnerName": "", "MembershipNo": "", "MembershipDate": "", "IsUploadAuditCertificateFile": false, "AuditCertificateFileName": "", "IsUploadMembershipCertificateFile": false, "MembershipCertificateFileName": "", "IsUploadAuditCertificateFile": false, "AuditCertificateFileName": "" });

        }

    }

    $scope.RemovePartnerListRow = function (index) {
        $scope.PartnerList.splice(index, 1);
        $scope.$apply();
    }


    $scope.ValidatePartnerList = function (status) {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.Empanelled.ApplicantType == 4) {

            if (status == false) {
                $scope.PartnerList.forEach(function (item, index) {
                    if (item.Name != "") {

                        if (item.PartnerName == "") {
                            if (message.indexOf($scope._language == 'en-us' ? 'Please Enter PartnerName' : 'कृपया साथी का नाम दर्ज करें') < 0) {
                                message += $scope._language == 'en-us' ? '<br />Please Enter PartnerName.' : '<br />कृपया साथी का नाम दर्ज करें।';
                            }
                            $("#txtPartnerName_" + index).addClass("notValid");
                        }
                        else {
                            $("#txtPartnerName_" + index).removeClass("notValid");
                        }
                        if (item.MembershipNo == "") {
                            if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Membership No' : 'कृपया सदस्यता संख्या दर्ज करें') < 0) {
                                message += $scope._language == 'en-us' ? '<br />Please Enter Membership No' : '<br />कृपया सदस्यता संख्या दर्ज करें।';
                            }
                            $("#txtMembershipNo_" + index).addClass("notValid");
                        }
                        else {
                            $("#txtMembershipNo_" + index).removeClass("notValid");
                        }

                        if ($("#MembershipDate_" + index).val() == "" || $("#MembershipDate_" + index).val() == undefined) {
                            if (message.indexOf($scope._language == 'en-us' ? 'Please Enter MembershipDate' : 'कृपया सदस्यता तिथि दर्ज करें') < 0) {
                                message += $scope._language == 'en-us' ? '<br />Please Enter MembershipDate' : '<br />कृपया सदस्यता तिथि दर्ज करें।';
                            }
                            $("#MembershipDate_" + index).addClass("notValid");
                        }
                        else {
                            $("#MembershipDate_" + index).removeClass("notValid");
                        }
                    }
                });
            }
            else {
                $scope.PartnerList.forEach(function (item, index) {

                    if (!item.Name) {
                        if (message.indexOf($scope._language == 'en-us' ? 'Please enter Name.' : 'कृपया नाम दर्ज करें।') < 0) {
                            message += $scope._language == 'en-us' ? '<br />Please enter Name.' : '<br />कृपया नाम दर्ज करें।';
                        }
                        $("#txtName_" + index).addClass("notValid");
                    }
                    else {
                        $("#txtName_" + index).removeClass("notValid");
                    }


                    if (item.PartnerName == "") {
                        if (message.indexOf($scope._language == 'en-us' ? 'Please Enter PartnerName' : 'कृपया साथी का नाम दर्ज करें') < 0) {
                            message += $scope._language == 'en-us' ? '<br />Please Enter PartnerName.' : '<br />कृपया साथी का नाम दर्ज करें।';
                        }
                        $("#txtPartnerName_" + index).addClass("notValid");
                    }
                    else {
                        $("#txtPartnerName_" + index).removeClass("notValid");
                    }
                    if (item.MembershipNo == "") {
                        if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Membership No' : 'कृपया सदस्यता संख्या दर्ज करें') < 0) {
                            message += $scope._language == 'en-us' ? '<br />Please Enter Membership No' : '<br />कृपया सदस्यता संख्या दर्ज करें।';
                        }
                        $("#txtMembershipNo_" + index).addClass("notValid");
                    }
                    else {
                        $("#txtMembershipNo_" + index).removeClass("notValid");
                    }

                    if ($("#MembershipDate_" + index).val() == "" || $("#MembershipDate_" + index).val() == undefined) {
                        if (message.indexOf($scope._language == 'en-us' ? 'Please Enter MembershipDate' : 'कृपया सदस्यता तिथि दर्ज करें') < 0) {
                            message += $scope._language == 'en-us' ? '<br />Please Enter MembershipDate' : '<br />कृपया सदस्यता तिथि दर्ज करें।';
                        }
                        $("#MembershipDate_" + index).addClass("notValid");
                    }
                    else {
                        $("#MembershipDate_" + index).removeClass("notValid");
                    }

                });
            }

        }
        if (message.length > 29) {
            ShowOkMessage("Auditor Request", message, function () { });
            return false;
        }
        return true;
    }

    //-----------

    $scope.Empanelled = [];

    $scope.validateAuditEmpanelled = function (status) {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.Empanelled.ApplicantNameEn == '' || $scope.Empanelled.ApplicantNameEn == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter Name of Applicant Chartered Accountants or Audit Firm (In English).' : '<br />कृपया आवेदक चार्टर्ड अकाउंटेंट्स या लेखा परीक्षा फर्म का नाम (अंग्रेजी में) दर्ज करें।';
        }
        if ($scope.Empanelled.ApplicantNameHi == '' || $scope.Empanelled.ApplicantNameHi == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter Name of Applicant Chartered Accountants or Audit Firm (In Hindi).' : '<br />कृपया आवेदक चार्टर्ड अकाउंटेंट्स या लेखा परीक्षा फर्म का नाम (हिन्दी में) दर्ज करें';
        }

        if ($scope.Empanelled.RegistrationNo == '' || $scope.Empanelled.RegistrationNo == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter Latest registration number provided by the Institute of Chartered Accountants of India to a Chartered Accountant or Audit Firm.' : '<br />कृपया चार्टर्ड अकाउंटेंट या लेखा परीक्षा फर्म को भारतीय चार्टर्ड अकाउंटेंट संस्थान द्वारा प्रदत्त नवीनतम पंजीयन क्रमांक दर्ज करें।';
        }
        if ($("#RegDateID").val() == '' || $("#RegDateID").val() == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter Latest registration date provided by the Institute of Chartered Accountants of India to a Chartered Accountant or Audit Firm.' : '<br />कृपया चार्टर्ड अकाउंटेंट या लेखा परीक्षा फर्म को भारतीय चार्टर्ड अकाउंटेंट संस्थान द्वारा प्रदत्त नवीनतम पंजीयन दिनांक दर्ज करें।';
        }
        if (!status) {
            if (message.length > 29) {
                ShowOkMessage("Auditor Request", message, function () { });
                return false;
            } else {
                return true;
            }
        }
        $scope.MemberAddress = GetAddressinfo('MemberAdd');

        if ($scope.MemberAddress.HouseBuildingNo == undefined || $scope.MemberAddress.HouseBuildingNo == '' || $scope.MemberAddress.HouseBuildingNo == ' ' || $scope.MemberAddress.HouseBuildingNo == null) {
            message += _language == 'en-us' ? 'Please enter the house/Building Number\n' : '<br />कृपया घर/भवन संख्या दर्ज करें।\n';
        }
        if ($scope.MemberAddress.LaneStreetName == undefined || $scope.MemberAddress.LaneStreetName == '' || $scope.MemberAddress.LaneStreetName == ' ' || $scope.MemberAddress.LaneStreetName == null) {
            message += _language == 'en-us' ? 'Please enter LaneStreetName\n' : '<br />कृपया पथ/गली नाम दर्ज करें।\n';
        }
        if ($scope.MemberAddress.Locality == undefined || $scope.MemberAddress.Locality == '' || $scope.MemberAddress.Locality == ' ' || $scope.MemberAddress.Locality == null) {
            message += _language == 'en-us' ? 'Please enter Locality \n' : '<br />कृपया स्थानीयता दर्ज करें।\n';
        }
        if ($scope.MemberAddress.Stateid == undefined || $scope.MemberAddress.Stateid == '' || $scope.MemberAddress.Stateid == null) {
            message += _language == 'en-us' ? 'Please select the State\n' : '<br />कृपया राज्य चुनें।\n';
        }
        if ($scope.MemberAddress.DivisionId == undefined || $scope.MemberAddress.DivisionId == '0' || $scope.MemberAddress.DivisionId == '' || $scope.MemberAddress.DivisionId == null) {
            message += _language == 'en-us' ? 'Please select the Division\n' : '<br />कृपया संभाग का चयन करें।\n';
        }
        if ($scope.MemberAddress.DistrictId == undefined || $scope.MemberAddress.DistrictId == '0' || $scope.MemberAddress.DistrictId == '' || $scope.MemberAddress.DistrictId == null) {
            message += _language == 'en-us' ? 'Please select the District\n' : '<br />कृपया जिले का चयन करें।\n';
        }
        if ($scope.MemberAddress.Pincode == undefined || $scope.MemberAddress.Pincode == '' || $scope.MemberAddress.Pincode == ' ' || $scope.MemberAddress.Pincode == null) {
            message += _language == 'en-us' ? 'Please enter Pincode\n' : '<br />कृपया पिन कोड दर्ज करें।\n';
        }

        if ($scope.MemberAddress.Urban_Rural == "" || $scope.MemberAddress.Urban_Rural == "0" || $scope.MemberAddress.Urban_Rural == undefined) {
            message += _language == 'en-us' ? 'Please select the Urban/Rural\n' : '<br /> कृपया शहरी/ग्रामीण का चयन करें\n';
        } else {
            if ($scope.MemberAddress.Urban_Rural == "Urban") {
                if ($scope.MemberAddress.CityId == undefined || $scope.MemberAddress.CityId == '' || $scope.MemberAddress.CityId == null) {
                    message += _language == 'en-us' ? 'Please select the City\n' : '<br />कृपया शहर का चयन करें।\n';
                }
                if ($scope.MemberAddress.WardId == undefined || $scope.MemberAddress.WardId == '0' || $scope.MemberAddress.WardId == null || $scope.MemberAddress.WardId == '? string: ?') {
                    message += _language == 'en-us' ? 'Please select the Ward\n' : '<br />कृपया वार्ड का चयन करें।\n';
                }
            }

            if ($scope.MemberAddress.Urban_Rural == "Rurals") {
                if ($scope.MemberAddress.BlockId == undefined || $scope.MemberAddress.BlockId == '' || $scope.MemberAddress.BlockId == null) {
                    message += _language == 'en-us' ? 'Please select the Block\n' : '<br />कृपया ब्लॉक का चयन करें।\n';
                }
                if ($scope.MemberAddress.PanchayatId == undefined || $scope.MemberAddress.PanchayatId == '0' || $scope.MemberAddress.PanchayatId == null || $scope.MemberAddress.PanchayatId == '? string: ?') {
                    message += _language == 'en-us' ? 'Please select the Gram Panchayat\n' : '<br />कृपया ग्राम पंचायत का चयन करें।\n';
                }
                if ($scope.MemberAddress.VillageId == undefined || $scope.MemberAddress.VillageId == '0' || $scope.MemberAddress.VillageId == null || $scope.MemberAddress.VillageId == '? string: ?') {
                    message += _language == 'en-us' ? 'Please select the Village\n' : '<br />कृपया गाँव का चयन करें।\n';
                }
            }
        }



        //if ($scope.Empanelled.AddressEn == '' || $scope.Empanelled.AddressEn == undefined) {
        //    message += $scope._language == 'en-us' ? '<br />Please enter Address of Applicant Chartered Accountant or Audit Firm (in English).' : '<br />कृपया आवेदक चार्टर्ड अकाउंटेंट या लेखा परीक्षा फर्म का पता (अंग्रेजी में) दर्ज करें।';
        //}
        //if ($scope.Empanelled.AddressHi == '' || $scope.Empanelled.AddressHi == undefined) {
        //    message += $scope._language == 'en-us' ? '<br />Please enter Address of Applicant Chartered Accountant or Audit Firm (in Hindi).' : '<br />कृपया आवेदक चार्टर्ड अकाउंटेंट या लेखा परीक्षा फर्म का पता (हिन्दी में) दर्ज करें।';
        //}


        //if ($scope.Empanelled.PinCode == '' || $scope.Empanelled.PinCode == undefined) {
        //    message += $scope._language == 'en-us' ? '<br />Please enter PinCode.' : '<br />कृपया पिनकोड दर्ज करें।';
        //}
        if ($scope.Empanelled.ContactNo1 == '' || $scope.Empanelled.ContactNo1 == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter Phone Number 1 (Office).' : '<br />कृपया दूरभाष नम्बर 1(आफिस) दर्ज करें।';
        }

        //if ($scope.Empanelled.FaxNo == '' || $scope.Empanelled.FaxNo == undefined) {
        //    message += $scope._language == 'en-us' ? '<br />Please enter FaxNo.' : '<br />कृपया फैक्स नंबर दर्ज करें।';
        //}
        if ($scope.Empanelled.MobileNo == '' || $scope.Empanelled.MobileNo == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter MobileNo.' : '<br />कृपया मोबाइल नंबर दर्ज करें।';
        }
        if ($scope.Empanelled.EmailId == '' || $scope.Empanelled.EmailId == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter EmailId.' : '<br />कृपया ईमेल आईडी दर्ज करें।';
        }



        if ($scope.Empanelled.IsRBIFirm) {
            if ($scope.Empanelled.RBICode == '' || $scope.Empanelled.RBICode == undefined) {
                message += $scope._language == 'en-us' ? '<br />Please enter RBI Panel Unique Code.' : '<br />कृपया आर.बी.आई पैनल का यूनिक कोड दर्ज करें।';
            }
            if ($("#RegistrationDateID").val() == '' || $("#RegistrationDateID").val() == undefined) {
                message += $scope._language == 'en-us' ? '<br />Please enter RBI-Registration Date.' : '<br />कृपया आर.बी.आई पैनल की पंजीकरण की तारीख दर्ज करें।';
            }
        }

        if ($scope.Empanelled.IsNabardFirm) {
            if ($scope.Empanelled.NabardCode == '' || $scope.Empanelled.NabardCode == undefined) {
                message += $scope._language == 'en-us' ? '<br />Please enter Nabard Panel Code.' : '<br />कृपया नाबार्ड पैनल कोड दर्ज करें।';
            }
            if ($("#NbardRegistrationDateID").val() == '' || $("#NbardRegistrationDateID").val() == undefined) {
                message += $scope._language == 'en-us' ? '<br />Please enter Nabard-RegistrationDate.' : '<br />कृपया नाबार्ड की पंजीकरण की तारीख दर्ज करें।';
            }
        }

        if ($scope.Empanelled.ApplicantType == '' || $scope.Empanelled.ApplicantType == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please select ApplicantType.' : '<br />कृपया आवेदक का प्रकार चयन करें।';
        }

        if ($scope.Empanelled.ApplicantType == 4) {

            $scope.PartnerList.forEach(function (item, index) {

                if (!item.Name) {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter Name.' : 'कृपया नाम दर्ज करें।') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please enter Name.' : '<br />कृपया नाम दर्ज करें।';
                    }
                    $("#txtName_" + index).addClass("notValid");
                }
                else {
                    $("#txtName_" + index).removeClass("notValid");
                }
                if (item.PartnerName == "") {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Partner/Working Chartered Accountant' : 'कृपया भागीदार/कार्यरत चार्टर्ड अकाउंटेंट दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please Enter Partner/Working Chartered Accountant.' : '<br />कृपया भागीदार/कार्यरत चार्टर्ड अकाउंटेंट दर्ज करें';
                    }
                    $("#txtPartnerName_" + index).addClass("notValid");
                }
                else {
                    $("#txtPartnerName_" + index).removeClass("notValid");
                }
                if (item.MembershipNo == "") {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Partner/Working Chartered Accountant Membership No' : 'कृपया भागीदार/कार्यरत चार्टर्ड अकाउंटेंट का सदस्यता संख्या दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please Enter Partner/Working Chartered Accountant Membership No' : '<br />कृपया भागीदार/कार्यरत चार्टर्ड अकाउंटेंट का सदस्यता संख्या दर्ज करें';
                    }
                    $("#txtMembershipNo_" + index).addClass("notValid");
                }
                else {
                    $("#txtMembershipNo_" + index).removeClass("notValid");
                }

                if ($("#MembershipDate_" + index).val() == "" || $("#MembershipDate_" + index).val() == undefined) {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Partner/Working Chartered Accountant MembershipDate' : 'कृपया भागीदार/कार्यरत चार्टर्ड अकाउंटेंट का सदस्यता तिथि दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please Enter Partner/Working Chartered Accountant MembershipDate' : '<br />कृपया भागीदार/कार्यरत चार्टर्ड अकाउंटेंट का सदस्यता तिथि दर्ज करें।';
                    }
                    $("#MembershipDate_" + index).addClass("notValid");
                }
                else {
                    $("#MembershipDate_" + index).removeClass("notValid");
                }

                if (!item.IsUploadPracticeCertificateFile && item.PracticeCertificateFileName == "") {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please upload Partner/Working Chartered Accountant Certificate of Practice (self-attested May date) issued by the Institute of Chartered Accountants of India' : 'कृपया भागीदार/कार्यरत चार्टर्ड अकाउंटेंट का भारतीय चार्टर्ड अकाउंटेंट संस्थान द्वारा जारी "सर्टिफिकेट ऑफ प्रेक्टिस" (स्वप्रमाणित मय दिनांक) अपलोड करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please upload Partner/Working Chartered Accountant Certificate of Practice (self-attested May date) issued by the Institute of Chartered Accountants of India' : '<br />कृपया भागीदार/कार्यरत चार्टर्ड अकाउंटेंट का भारतीय चार्टर्ड अकाउंटेंट संस्थान द्वारा जारी "सर्टिफिकेट ऑफ प्रेक्टिस" (स्वप्रमाणित मय दिनांक) अपलोड करें';
                    }
                }
                if (!item.IsUploadMembershipCertificateFile && item.MembershipCertificateFileName == "") {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please upload Partner/Working Chartered Accountant Certificate of Membership (self-attested May date) issued by the Institute of Chartered Accountants of India' : 'कृपया भागीदार/कार्यरत चार्टर्ड अकाउंटेंट का भारतीय चार्टर्ड अकाउंटेंट संस्थान द्वारा जारी "सर्टिफिकेट ऑफ मैम्बरशिप" (स्वप्रमाणित मय दिनांक) अपलोड करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please upload Partner/Working Chartered Accountant Certificate of Membership (self-attested May date) issued by the Institute of Chartered Accountants of India' : '<br />कृपया भागीदार/कार्यरत चार्टर्ड अकाउंटेंट का भारतीय चार्टर्ड अकाउंटेंट संस्थान द्वारा जारी "सर्टिफिकेट ऑफ मैम्बरशिप" (स्वप्रमाणित मय दिनांक) अपलोड करें';
                    }
                }
                //if (!item.IsUploadAuditCertificateFile && item.AuditCertificateFileName == "") {
                //    if (message.indexOf($scope._language == 'en-us' ? 'Please upload Partner/Working Chartered Accountant Diploma in System Audit or Certificate in System Audit Certificate' : 'कृपया भागीदार/कार्यरत चार्टर्ड अकाउंटेंट का डिप्लोमा इन सिस्टम ऑडिट अथवा सर्टिफिकेट इन सिस्टम ऑडिट प्रमाण-पत्र (स्वप्रमाणित मय दिनांक) अपलोड करें') < 0) {
                //        message += $scope._language == 'en-us' ? '<br />Please upload Partner/Working Chartered Accountant Diploma in System Audit or Certificate in System Audit Certificate' : '<br />कृपया भागीदार/कार्यरत चार्टर्ड अकाउंटेंट का डिप्लोमा इन सिस्टम ऑडिट अथवा सर्टिफिकेट इन सिस्टम ऑडिट प्रमाण-पत्र (स्वप्रमाणित मय दिनांक) अपलोड करें';
                //    }
                //}

            });
        }



        //if ($scope.Empanelled.Isshowfile == '' || $scope.Empanelled.Isshowfile == undefined) {
        //    message += $scope._language == 'en-us' ? '<br />Please select Course Complete.' : '<br />कृपया कोर्स पूरा का प्रकार चयन करें।';
        //}
        //else {
        //    if ($scope.Empanelled.Isshowfile == '1') {
        //        if ($scope.Empanelled.FileName == '' || $scope.Empanelled.FileName == undefined) {
        //            message += $scope._language == 'en-us' ? '<br />Please select Course Completed File.' : '<br />कृपया कोर्स कम्प्लीट फाइल का चयन करें।';
        //        }
        //    }
        //}


        if ($scope.Empanelled.ExpieranceAreaRemark == '' || $scope.Empanelled.ExpieranceAreaRemark == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter Details of field of experience (Private Firm/State Department/Semi-Government Institute/Cooperative Sector/Others.' : '<br />कृपया अनुभव का क्षेत्र (निजी फर्म/राजकीय विभाग/अर्द्धशासकीय संस्थान/सहकारी क्षेत्र/अन्य) का विवरण दर्ज करें।';
        }
        //if ($scope.Empanelled.Ammount == '' || $scope.Empanelled.Ammount == undefined) {
        //    message += $scope._language == 'en-us' ? '<br />Please enter Ammount.' : '<br />कृपया अम्माउंट दर्ज करें।';
        //}
        ////////if ($scope.Empanelled.AmmountDetails == '' || $scope.Empanelled.AmmountDetails == undefined) {
        ////////    message += $scope._language == 'en-us' ? '<br />Please enter Name of the bank branch in which funds have been deposited online / through challans under the head "0425 - Cooperation, 800 - Other Receipts, 002 - Multiple Receipts".' : '<br />कृपया बैंक शाखा का नाम, जिसमे राज्यकोष के "0425 -सहकारिता ,800 -अन्य प्राप्तियाँ , 002 -विविध प्राप्तियाँ " मद में ऑनलाइन/चालान द्वारा राशि जमा कराई गयी हैं दर्ज करें।';
        ////////}

        ////////if ($("#FeeDepositDate").val() == '' || $("#FeeDepositDate").val() == undefined) {
        ////////    message += $scope._language == 'en-us' ? '<br />Please enter Date of deposit".' : '<br />कृपया राशि जमा की दिनांक दर्ज करें।';
        ////////}

        $scope.RegistrationDocuments.filter(function (x) { return x.IsShow }).forEach(function (itm, indx) {
            if (!itm.IsFileUploaded || itm.FileName == '') {
                message += $scope._language == 'en-us' ? '<br />Please upload ' + itm.DocNameEn + '.' : '<br />कृपया ' + itm.DocNameHi + ' अपलोड करें।';
            }
        });

        if (message.length > 29) {
            ShowOkMessage("Auditor Request", message, function () { });
            return false;
        }
        return true;
    }
    $scope.backtoEdit = function () {
        $scope.IsShowPreview = false;
    }
    $scope.ShowDeclaration = function () {
        if ($scope.validateAuditEmpanelled(true)) {
            //if ($scope.ValidateAuditFirmList(true)) {
            //    if ($scope.ValidateTraningDetailsList(true)) {
            $scope.Empanelled.RegDate = $("#RegDateID").val();
            $scope.Empanelled.FeeDepositDate1 = $("#FeeDepositDate").val();
            $scope.Empanelled.NbardRegistrationDate1 = $("#RegistrationDateID").val();
            $scope.Empanelled.AddressDetials = GetAddressDispplay('MemberAdd');
            $scope.EmpanelledAddress = GetAddressinfo('MemberAdd');
            $scope.IsShowPreview = true;
            $scope.SaveUser(false);
            //    }
            // }
        }
    }

    $scope.PartialSubmit = function (status) {
        if (status == false) {
            if ($scope.validateAuditEmpanelled(status)) {
                $scope.SaveUser(status);
            }
        }
        else {
            if ($scope.validateAuditEmpanelled(status)) {
                //if ($scope.ValidateAuditFirmList(status)) {
                //    if ($scope.ValidateTraningDetailsList(status)) {
                $scope.SaveUser(status);
                //    }
                //}
            };
        }
        //$scope.SaveUser(status);
    }



    $scope.SaveUser = function (status) {

        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();

        var PartnerList1 = [];
        var AuditFirmList1 = [];
        var TraningDetailsList1 = [];

        $scope.PartnerList.forEach(function (itm, index) {
            if (!itm.IsDelete || itm.IsDelete == undefined) {
                itm.MembershipDate = $("#MembershipDate_" + index).val();
                PartnerList1.push({
                    Name: itm.Name,
                    AuditPartnershipId: itm.AuditPartnershipId,
                    PartnerName: itm.PartnerName,
                    MembershipNumber: itm.MembershipNo,
                    MembershipDate: itm.MembershipDate,
                    PracticeCertificateFileName: itm.PracticeCertificateFileName,
                    MembershipCertificateFileName: itm.MembershipCertificateFileName,
                    AuditCertificateFileName: itm.AuditCertificateFileName
                });
            }
        });

        $scope.AuditFirmList.forEach(function (itm) {
            if (!itm.IsDelete || itm.IsDelete == undefined) {
                AuditFirmList1.push({
                    AuditfirmAddressId: itm.AuditfirmAddressId,
                    Name: itm.Name,
                    MobileNo: itm.MobileNo
                });
            }
        });

        $scope.TraningDetailsList.forEach(function (itm) {
            if (!itm.IsDelete || itm.IsDelete == undefined) {
                TraningDetailsList1.push({
                    FirmParemenentTraniningId: itm.FirmParemenentTraniningId,
                    AssistantName: itm.AssistantName,
                    Education: itm.Education,
                    Experiance: itm.Experiance
                });
            }
        });

        var RegDate = $("#RegDateID").val();
        var RegistrationDate = $("#RegistrationDateID").val();
        var NbardRegistrationDate = $("#NbardRegistrationDateID").val();
        $scope.MemberAddress = GetAddressinfo('MemberAdd');

        var applicationDetails = {
            AuditEmpanelledId: $scope.Empanelled.AuditEmpanelledId,
            ApplicantNameEn: $scope.Empanelled.ApplicantNameEn,
            ApplicantNameHi: $scope.Empanelled.ApplicantNameHi,
            RegistrationNo: $scope.Empanelled.RegistrationNo,
            //RegDate: RegDate,
            RegistrationDate: RegDate,
            AddressEn: $scope.Empanelled.AddressEn,
            AddressHi: $scope.Empanelled.AddressHi,
            ContactNo1: $scope.Empanelled.ContactNo1,
            ContactNo2: $scope.Empanelled.ContactNo2,
            FaxNo: $scope.Empanelled.FaxNo,
            MobileNo: $scope.Empanelled.MobileNo,
            EmailId: $scope.Empanelled.EmailId,
            ApplicantType: $scope.Empanelled.ApplicantType,
            RBICode: $scope.Empanelled.IsRBIFirm ? $scope.Empanelled.RBICode : '',
            RBIRegistrationDate: $scope.Empanelled.IsRBIFirm ? RegistrationDate : '',
            NabardCode: $scope.Empanelled.IsNabardFirm ? $scope.Empanelled.NabardCode : '',
            NbardRegistrationDate: $scope.Empanelled.IsNabardFirm ? NbardRegistrationDate : '',
            IsCompletecourse: $scope.Empanelled.Isshowfile,
            CertificateName: $scope.Empanelled.FileName,
            ExpieranceAreaRemark: $scope.Empanelled.ExpieranceAreaRemark,
            Ammount:10000,// $scope.Empanelled.Ammount,
            AmmountDetails: $scope.Empanelled.AmmountDetails,
            FeeDepositDate: $("#FeeDepositDate").val(), //$scope.Empanelled.FeeDepositDate,
            AuditformPartnership1: PartnerList1,
            AuditfirmAddressdetails1: AuditFirmList1,
            FirmParemenentTraniningDetails1: TraningDetailsList1,
            IsSubmit: status,
            Status: $scope.Empanelled.Status,
            FirmPracticeCertificate: $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmPracticeCertificate' && x.IsFileUploaded && x.FileName != '' && x.IsShow }).length > 0 ? $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmPracticeCertificate' && x.IsFileUploaded && x.FileName != '' && x.IsShow })[0].FileName : '',
            FirmMembershipCertificate: $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmMembershipCertificate' && x.IsFileUploaded && x.FileName != '' && x.IsShow }).length > 0 ? $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmMembershipCertificate' && x.IsFileUploaded && x.FileName != '' && x.IsShow })[0].FileName : '',
            FirmRegistrationCertificate: $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmRegistrationCertificate' && x.IsFileUploaded && x.FileName != '' && x.IsShow }).length > 0 ? $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmRegistrationCertificate' && x.IsFileUploaded && x.FileName != '' && x.IsShow })[0].FileName : '',
            //FeeChallan: $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FeeChallan' && x.IsFileUploaded && x.FileName != '' && x.IsShow }).length > 0 ? $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FeeChallan' && x.IsFileUploaded && x.FileName != '' && x.IsShow })[0].FileName : '',
            FirmRegistrationNABARD: $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmRegistrationNABARD' && x.IsFileUploaded && x.FileName != '' && x.IsShow }).length > 0 ? $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmRegistrationNABARD' && x.IsFileUploaded && x.FileName != '' && x.IsShow })[0].FileName : '',
            FirmTermsConditions: $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmTermsConditions' && x.IsFileUploaded && x.FileName != '' && x.IsShow }).length > 0 ? $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmTermsConditions' && x.IsFileUploaded && x.FileName != '' && x.IsShow })[0].FileName : '',
            FirmDiplomaCertificate: $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmDiplomaCertificate' && x.IsFileUploaded && x.FileName != '' && x.IsShow }).length > 0 ? $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmDiplomaCertificate' && x.IsFileUploaded && x.FileName != '' && x.IsShow })[0].FileName : '',
            IsRural: $scope.MemberAddress.Urban_Rural == 'Rurals' ? true : false,
            HouseNo: $scope.MemberAddress.HouseBuildingNo,
            LaneStreetName: $scope.MemberAddress.LaneStreetName,
            Locality: $scope.MemberAddress.Locality,
            STATE_ID: $scope.MemberAddress.Stateid,
            DIVISION_ID: $scope.MemberAddress.DivisionId,
            DISTRICT_ID: $scope.MemberAddress.DistrictId,
            CITY_ID: $scope.MemberAddress.CityId,
            BLOCK_ID: $scope.MemberAddress.BlockId,
            WARD_ID: $scope.MemberAddress.WardId,
            GP_ID: $scope.MemberAddress.PanchayatId,
            VILLAGE_ID: $scope.MemberAddress.VillageId,
            PinCode: $scope.MemberAddress.Pincode,
            ApplicantName: $scope.ApplicantName,
            ApplicantAddress: $scope.ApppicantFathaerName,
            ApplicantFatherName: $scope.ApplicantAddress,
            ApplicantAadharNo: $scope.Empanelled.AADHARNo,
            FirmRegistrationRBI: $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmRegistrationRBI' && x.IsFileUploaded && x.FileName != '' && x.IsShow }).length > 0 ? $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmRegistrationRBI' && x.IsFileUploaded && x.FileName != '' && x.IsShow })[0].FileName : '',
            IsFirm: $scope.Empanelled.ApplicantType == 2,
            IsPartnerFirm: $scope.Empanelled.ApplicantType == 4

        };


        formData.append("Applicationdata", JSON.stringify(applicationDetails));
        formData.append("AuditformPartnership1", JSON.stringify($scope.PartnerList));
        formData.append("AuditfirmAddressdetails1", JSON.stringify($scope.AuditFirmList));
        formData.append("FirmParemenentTraniningDetails1", JSON.stringify($scope.TraningDetailsList));

        $http.post(SiteUrl + "/Auditor/SaveAuditEmpanelled", JSON.stringify({ ApplicationModel: applicationDetails })).success(function (data, status11) {
            $(".se-pre-con").fadeOut("slow");
            // alert(data.Result);
            if (data.Result != "-1" && data.Result != "0") {
                if (!status) {
                    $scope.Empanelled.AuditEmpanelledId = data.Result;
                } else {
                    ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () {
                        //if (!status) {
                        $scope.Empanelled.AuditEmpanelledId = data.Result;
                        // } else {
                        window.location.href = '../auditor/EmpanelledAudit';
                        //}
                        //$scope.GetAuditorEmpanelledList();
                        //$scope.EmpanelledDivForm = false;
                        //$scope.EmpanelledDiv = true;
                    });
                }
            } else {
                ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें| ", function () {

                });
            }
        }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें| ", function () {
                });
            });

    };


    $scope.GetAuditorEmpanelledList = function () {

        //$http.post(SiteUrl + "/Auditor/GetAuditEmpanelledAllDetails"JSON.stringify({ Details: jsonParameters})).
        $http.post(SiteUrl + "/Auditor/GetAuditEmpanelledDetails").
            success(function (data, status) {
                $scope.GetAuditorEmpanelled = JSON.parse(data.DataList);
            }).
            error(function (data, status) {
                $scope.GetAuditorEmpanelled = [];
                alert('Error while Society Election Detail data.');
            });
    }
    $scope.GetAuditorEmpanelledList();
    $scope.IsApplicationSubmited = false;
    $scope.IsPaymentDone = false;
    $scope.AuditorEmpView = function (AuditEmpanelledId) {
        $scope.Empanelled.AuditEmpanelledId = AuditEmpanelledId;
        $scope.IsPaymentDone = false
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Auditor/GetAuditEmpanelledDetailsById", JSON.stringify({ AuditEmpanelledID: AuditEmpanelledId })).
            success(function (data, status) {
                $scope.GetAuditorAllList = JSON.parse(data.DataList);
                $scope.ShowApply = false;
                $scope.Empanelled = $scope.GetAuditorAllList.Table[0];
                $scope.IsPaymentDone = $scope.Empanelled.IsPaymentDone;
                if ($scope.Empanelled.ApplicantType == 2) {
                    $scope.Empanelled.ApplicantType1 = 4
                } if ($scope.Empanelled.ApplicantType == 1) {
                    $scope.Empanelled.ApplicantType1 = 3
                }

                //$scope.Empanelled.RegistrationDate = RegistrationDate1
                //$scope.Empanelled.NbardRegistrationDate = NbardRegistrationDate1
                //$scope.Empanelled.FeeDepositDate = FeeDepositDate1

                $scope.ApplicantName = $scope.Empanelled.ApplicantName;
                $scope.ApppicantFathaerName = $scope.Empanelled.ApplicantAddress;
                $scope.ApplicantAddress = $scope.Empanelled.ApplicantFatherName;
                $scope.Empanelled.AADHARNo = $scope.Empanelled.ApplicantAadharNo;

                $scope.Empanelled.IsRBIFirm = $scope.Empanelled.RBICode != undefined && $scope.Empanelled.RBICode != '' && $scope.Empanelled.RBICode != null;
                $scope.Empanelled.IsNabardFirm = $scope.Empanelled.NabardCode != undefined && $scope.Empanelled.NabardCode != '' && $scope.Empanelled.NabardCode != null;


                $scope.IsApplicationSubmited = $scope.Empanelled.Status == 0 || $scope.Empanelled.Status == 2;

                $scope.IsShowPreview = !($scope.Empanelled.Status == 0 || $scope.Empanelled.Status == 4);

                $scope.Empanelled.Acceptence = $scope.Empanelled.Status == 0 || $scope.Empanelled.Status == 2;
                $scope.Empanelled.Acceptence2 = $scope.Empanelled.Status == 0 || $scope.Empanelled.Status == 2;

                addressdataset1 = {
                    Housebuilding: $scope.Empanelled.HouseNo,
                    State: $scope.Empanelled.STATE_ID,
                    District: " " + $scope.Empanelled.DISTRICT_ID,
                    Lanestreet: $scope.Empanelled.LaneStreetName,// + ' ' + addressdata1[1],
                    Locality: $scope.Empanelled.Locality,//+ ' ' + addressdata1[11],
                    Pincode: $scope.Empanelled.PinCode,
                    CITY_ID: $scope.Empanelled.CITY_ID,
                    BLOCK_ID: $scope.Empanelled.BLOCK_ID,
                    GP_ID: $scope.Empanelled.GP_ID,
                    WARD_Id: $scope.Empanelled.WARD_ID,
                    VILLAGE_ID: $scope.Empanelled.VILLAGE_ID,
                    IsRurals: $scope.Empanelled.IsRural
                }

                SetAddressinfo($scope.translation, 'MemberAdd', addressdataset1, $http).then(function () {
                    $scope.Empanelled.AddressDetials = GetAddressDispplay('MemberAdd');
                    $scope.EmpanelledAddress = GetAddressinfo('MemberAdd');
                });

                if ($scope.Empanelled.FirmPracticeCertificate != undefined && $scope.Empanelled.FirmPracticeCertificate != '' && $scope.Empanelled.FirmPracticeCertificate != null) {
                    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmPracticeCertificate' })[0].FileName = $scope.Empanelled.FirmPracticeCertificate;
                    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmPracticeCertificate' })[0].IsFileUploaded = true;
                }

                if ($scope.Empanelled.FirmMembershipCertificate != undefined && $scope.Empanelled.FirmMembershipCertificate != '' && $scope.Empanelled.FirmMembershipCertificate != null) {
                    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmMembershipCertificate' })[0].FileName = $scope.Empanelled.FirmMembershipCertificate;
                    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmMembershipCertificate' })[0].IsFileUploaded = true;
                }

                if ($scope.Empanelled.FirmRegistrationCertificate != undefined && $scope.Empanelled.FirmRegistrationCertificate != '' && $scope.Empanelled.FirmRegistrationCertificate != null) {
                    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmRegistrationCertificate' })[0].FileName = $scope.Empanelled.FirmRegistrationCertificate;
                    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmRegistrationCertificate' })[0].IsFileUploaded = true;
                }

                //if ($scope.Empanelled.FeeChallan != undefined && $scope.Empanelled.FeeChallan != '' && $scope.Empanelled.FeeChallan != null) {
                //    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FeeChallan' })[0].FileName = $scope.Empanelled.FeeChallan;
                //    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FeeChallan' })[0].IsFileUploaded = true;
                //}

                if ($scope.Empanelled.FirmRegistrationNABARD != undefined && $scope.Empanelled.FirmRegistrationNABARD != '' && $scope.Empanelled.FirmRegistrationNABARD != null) {
                    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmRegistrationNABARD' })[0].FileName = $scope.Empanelled.FirmRegistrationNABARD;
                    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmRegistrationNABARD' })[0].IsFileUploaded = true;
                    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmRegistrationNABARD' })[0].IsShow = true;
                }

                if ($scope.Empanelled.FirmRegistrationRBI != undefined && $scope.Empanelled.FirmRegistrationRBI != '' && $scope.Empanelled.FirmRegistrationRBI != null) {
                    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmRegistrationRBI' })[0].FileName = $scope.Empanelled.FirmRegistrationRBI;
                    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmRegistrationRBI' })[0].IsFileUploaded = true;
                    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmRegistrationRBI' })[0].IsShow = true;
                }

                if ($scope.Empanelled.FirmTermsConditions != undefined && $scope.Empanelled.FirmTermsConditions != '' && $scope.Empanelled.FirmTermsConditions != null) {
                    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmTermsConditions' })[0].FileName = $scope.Empanelled.FirmTermsConditions;
                    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmTermsConditions' })[0].IsFileUploaded = true;
                }

                if ($scope.Empanelled.FirmDiplomaCertificate != undefined && $scope.Empanelled.FirmDiplomaCertificate != '' && $scope.Empanelled.FirmDiplomaCertificate != null) {
                    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmDiplomaCertificate' })[0].FileName = $scope.Empanelled.FirmDiplomaCertificate;
                    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmDiplomaCertificate' })[0].IsFileUploaded = true;
                    $scope.RegistrationDocuments.filter(function (x) { return x.Id === 'FirmDiplomaCertificate' })[0].IsShow = true;
                    $scope.Empanelled.IsDiplomaInSystemAudit = true;
                }
                $scope.RegistrationDocumentsCopy = $scope.RegistrationDocuments.filter(function (x) { return x.IsShow });
                $scope.PartnerList = [];
                $scope.AuditFirmList = [];
                $scope.TraningDetailsList = [];

                if ($scope.Empanelled.ApplicantType == '4') {
                    $scope.Showpartnertbl = true;
                }
                $scope.Empanelled.Isshowfile = $scope.Empanelled.IsCompletecourse;

                if ($scope.Empanelled.Isshowfile == 1) {
                    $("#inlineRadio2").prop("checked", true);
                    $scope.Truefile = true;
                    $scope.Empanelled.IsUploadFileName = true;
                    $scope.Empanelled.FileName = $scope.Empanelled.CertificateName;
                }

                if ($scope.GetAuditorAllList.Table1.length > 0) {
                    $scope.GetAuditorAllList.Table1.forEach(function (itm, index) {
                        $scope.PartnerList.push({
                            AuditPartnershipId: itm.AuditPartnershipId,
                            Name: itm.Name,
                            PartnerName: itm.PartnerName,
                            MembershipNo: itm.MembershipNumber,
                            MembershipDate: itm.MembershipDate,
                            PracticeCertificateFileName: itm.PracticeCertificateFileName,
                            IsUploadPracticeCertificateFile: itm.PracticeCertificateFileName != undefined && itm.PracticeCertificateFileName != '' && itm.PracticeCertificateFileName != null,
                            MembershipCertificateFileName: itm.MembershipCertificateFileName,
                            IsUploadMembershipCertificateFile: itm.MembershipCertificateFileName != undefined && itm.MembershipCertificateFileName != '' && itm.MembershipCertificateFileName != null,
                            AuditCertificateFileName: itm.AuditCertificateFileName,
                            IsUploadAuditCertificateFile: itm.AuditCertificateFileName != undefined && itm.AuditCertificateFileName != '' && itm.AuditCertificateFileName != null
                        });
                    });
                } else {
                    $scope.PartnerList.push({ "AuditPartnershipId": 0, "Name": "", "PartnerName": "", "MembershipNo": "", "MembershipDate": "", "IsUploadAuditCertificateFile": false, "AuditCertificateFileName": "", "IsUploadMembershipCertificateFile": false, "MembershipCertificateFileName": "", "IsUploadAuditCertificateFile": false, "AuditCertificateFileName": "" });
                }

                if ($scope.GetAuditorAllList.Table2.length > 0) {
                    $scope.GetAuditorAllList.Table2.forEach(function (itm, index) {
                        $scope.AuditFirmList.push({
                            AuditfirmAddressId: itm.AuditfirmAddressId,
                            Name: itm.Name,
                            MobileNo: itm.MobileNo
                        });
                    });
                } else {
                    $scope.AuditFirmList.push({ "AuditfirmAddressId": 0, "Name": "", "MobileNo": "" });
                }
                if ($scope.GetAuditorAllList.Table3.length > 0) {
                    $scope.GetAuditorAllList.Table3.forEach(function (itm, index) {
                        $scope.TraningDetailsList.push({
                            FirmParemenentTraniningId: itm.FirmParemenentTraniningId,
                            AssistantName: itm.AssistantName,
                            Education: itm.Education,
                            Experiance: itm.Experiance
                        });
                    });
                } else {
                    $scope.TraningDetailsList.push({ "FirmParemenentTraniningId": 0, "AssistantName": "", "Education": "", "Experiance": "" });
                }
                $scope.EmpanelledDivForm = true;
                $scope.EmpanelledDiv = false;
                $scope.CheckIsubmit = $scope.Empanelled.Status == 4 || $scope.Empanelled.Status == 0 ? false : true;
                //$scope.IsPaymentDone=
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.GetAuditorAllList = [];
                $(".se-pre-con").fadeOut("slow");
                alert('Error while Society Election Detail data.');
            });

    }

    $scope.divAADHARVerificationStep_1 = false;

    $scope.ShowOTPAADHARVerication = function () {
        $scope.divAADHARVerificationStep_1 = true;
        $scope.EmpanelledDivForm = true;
        $scope.EmpanelledDiv = false;
        $scope.ShowApply = false;
    }

    $scope.ShowApplicationForm = function () {

        if ($scope.Empanelled.AADHARNo == undefined || $scope.Empanelled.AADHARNo == "" || $scope.Empanelled.AADHARNo.length < 12) {
            var msg = _language == 'en-us' ? 'Please enter 12 digit AADHAAR No.' : '12 अंकों की आधार संख्या दर्ज करें';
            ShowOkMessage("", msg, function () { });
        } else {
            AadhaarAuth($scope.Empanelled.AADHARNo, $http, "OTP-BIO", false, function (data) {
                $scope.divAADHARVerificationStep_1 = false;
                $scope.ApplicantName = data.Name;
                $scope.ApppicantFathaerName = data.FName;
                $scope.ApplicantAddress = data.Address
                $scope.Empanelled = [];
                $scope.IsApplicationSubmited = false;
                $scope.OtherPersonList = [];
                $scope.TraningDetailsList = [];
                $scope.AuditFirmList = [];
                $scope.PartnerList = [];
                $scope.Empanelled.Status = 0;
                $scope.IsShowPreview = false;
                $scope.OtherPersonList.push({ "PersonId": 0, "Name": "", "Address": "", "Post": "" });
                $scope.TraningDetailsList.push({ "FirmParemenentTraniningId": 0, "AssistantName": "", "Education": "", "Experiance": "" });
                $scope.AuditFirmList.push({ "AuditfirmAddressId": 0, "Name": "", "MobileNo": "" });
                $scope.PartnerList.push({ "AuditPartnershipId": 0, "Name": "", "PartnerName": "", "MembershipNo": "", "MembershipDate": "", "IsUploadAuditCertificateFile": false, "AuditCertificateFileName": "", "IsUploadMembershipCertificateFile": false, "MembershipCertificateFileName": "", "IsUploadAuditCertificateFile": false, "AuditCertificateFileName": "" });
                $scope.CheckIsubmit = false;
                $scope.EmpanelledDivForm = true;
                $scope.EmpanelledDiv = false;
                $scope.$apply();
            });
        }
    }

    $scope.Showpartnertbl = false;
    $scope.ShowPartnerlist = function (id) {
        //if (id == 4) 
        //    $scope.Showpartnertbl = true;        
        //else 
        //    $scope.Showpartnertbl = false; 
    }


    //File Save--start

    $scope.AddFile = function (type, FileTypeName, index) {
        $scope.FileType = type;
        $scope.FileTypeName = FileTypeName
        $scope.FileTypeIndex = index;
    }

    $scope.FileType = "";
    $scope.Empanelled.IsUploadFileName = false;
    $scope.AddFile1 = function (type) {
        debugger
        $scope.FileType = type;
        $scope.Empanelled.IsUploadFileName = true;
    }
    $scope.ChangeFile1 = function (type) {
        debugger
        if (type == 1) {
            $scope.Empanelled.IsUploadFileName = false;
            $scope.Empanelled.FileName = "";
        }
    }
    $scope.UploadFile_new = function (id, type) {
        debugger
        try {
            // FormData
            if (window.FormData !== undefined) {
                var fileUpload = $("#" + id).get(0);
                var files = fileUpload.files;
                //var blob = b64toBlob(realData, contentType, 512);
                var formData = new FormData();
                formData.append("pldbfile", files[0]);
                $(".blockUI").show();
                $scope.xhr1(SiteUrl + "/Auditor/UploadEmpanelledAuditFile", formData, type);
            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }

    $scope.xhr1 = function (url, data, type) {
        try {
            var request = new XMLHttpRequest();
            request.onreadystatechange = function () {
                if (request.readyState == 4 && request.status == 200) {
                    $(".blockUI").hide();
                    $("#filuploadProgress").hide();
                    $("#filuploadProgress").find("div").html("");
                    $("#filuploadProgress").find("div").attr("aria-valuenow", "1");
                    $("#filuploadProgress").find("div").attr("style", "width:1%");
                    if (request.responseText == "") {
                        ShowOkMessage("Error", $scope == 'en-us' ? "There is some problem in uploading file on server. Please try after some time" : "सर्वर पर फ़ाइल अपलोड करने में कुछ समस्या है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    } else {
                        if (type == 1) {
                            $scope.Empanelled.FileName = request.responseText;
                        }
                        $scope.$apply();
                    }
                }
            };

            request.upload.onloadstart = function () {
                //percentage.innerHTML = 'Upload started...';
            };
            $("#filuploadProgress").show();
            request.upload.onprogress = function (event) {
                $("#filuploadProgress").find("div").html(Math.round(event.loaded / event.total * 100) + "%");
                $("#filuploadProgress").find("div").attr("aria-valuenow", Math.round(event.loaded / event.total * 100));
                $("#filuploadProgress").find("div").attr("style", "width:" + Math.round(event.loaded / event.total * 100) + "%");
            };

            request.upload.onload = function (result) {
            }
            request.open('POST', url);
            request.send(data);
        }
        catch (ex) { alert(ex) }
    }

    $scope.ComplainTrail = {
        IsUploadFileName: false,
        FileName: "",

    }

    //File Save --End


    //----------------End------------
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
        // $scope.DivSearch = true;
        // $scope.RequestForm = false;



        $scope.EmpanelledDivForm = false;
        $scope.EmpanelledDiv = true;
        // $scope.GetAuditorDataList();

        // $(".se-pre-con").fadeOut("slow");
    };

    $scope.backtoSubmit = function () {
        $scope.EmpanelledDivForm = false;
        $scope.EmpanelledDiv = true;
        $scope.ShowApply = true;
    }


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

    //$scope.GetAuditorDataList = function () {
    //    $http.post(SiteUrl + "/Auditor/GetAuditorList").
    //        success(function (data, status) {
    //            $scope.AuditorData = [];
    //            $scope.AuditorData = data;
    //        }).
    //        error(function (data, status) {
    //            $scope.AuditorData = [];
    //        });
    //}


    $scope.OnPageLoad();
    $scope.IsFirmYN();


    $scope.UploadFile = function (id, type, indx, filetype) {
        try {
            // FormData
            if (window.FormData !== undefined) {
                var fileUpload = $("#" + id).get(0);
                var files = fileUpload.files;
                //var blob = b64toBlob(realData, contentType, 512);
                var formData = new FormData();
                formData.append("pldbfile", files[0]);
                $(".blockUI").show();
                $scope.xhrLVO(SiteUrl + "/Auditor/UploadEmpanelledAuditFile", formData, type, indx, filetype);
            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }

    $scope.xhrLVO = function (url, data, type, index, filetype) {
        try {
            var request = new XMLHttpRequest();
            request.onreadystatechange = function () {
                if (request.readyState == 4 && request.status == 200) {
                    $(".blockUI").hide();
                    $("#filuploadProgress").hide();
                    $("#filuploadProgress").find("div").html("");
                    $("#filuploadProgress").find("div").attr("aria-valuenow", "1");
                    $("#filuploadProgress").find("div").attr("style", "width:1%");
                    if (request.responseText == "") {
                        ShowOkMessage("Error", $scope == 'en-us' ? "There is some problem in uploading file on server. Please try after some time" : "सर्वर पर फ़ाइल अपलोड करने में कुछ समस्या है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    } else {
                        if (type == 1) {
                            if (filetype == 'PracticeCertificate') {
                                $scope.PartnerList[index].PracticeCertificateFileName = request.responseText;
                            } else if (filetype == 'MembershipCertificate') {
                                $scope.PartnerList[index].MembershipCertificateFileName = request.responseText;
                            } else if (filetype == 'AuditCertificate') {
                                $scope.PartnerList[index].AuditCertificateFileName = request.responseText;
                            }
                        } else if (type == 2) {
                            $scope.RegistrationDocuments.filter(function (x) { return x.IsShow })[index].IsFileUploaded = true;
                            $scope.RegistrationDocuments.filter(function (x) { return x.IsShow })[index].FileName = request.responseText;
                        }
                        $scope.$apply();
                    }
                }
            };

            request.upload.onloadstart = function () {
                //percentage.innerHTML = 'Upload started...';
            };
            $("#filuploadProgress").show();
            request.upload.onprogress = function (event) {
                $("#filuploadProgress").find("div").html(Math.round(event.loaded / event.total * 100) + "%");
                $("#filuploadProgress").find("div").attr("aria-valuenow", Math.round(event.loaded / event.total * 100));
                $("#filuploadProgress").find("div").attr("style", "width:" + Math.round(event.loaded / event.total * 100) + "%");
            };

            request.upload.onload = function (result) {
            }
            request.open('POST', url);
            request.send(data);
        }
        catch (ex) { alert(ex) }
    }

    $scope.RegistrationDocuments = [];
    $scope.RegistrationDocuments.push({ Id: 'FirmPracticeCertificate', DocNameEn: "Certificate of Practice (self-attested May date) issued by the Institute of Chartered Accountants of India.", DocNameHi: "भारतीय चार्टर्ड अकाउंटेंट संस्थान द्वारा जारी 'सर्टिफिकेट  ऑफ प्रेक्टिस'  (स्वप्रमाणित मय दिनांक )", FileName: "", IsFileUploaded: false, IsShow: true });
    $scope.RegistrationDocuments.push({ Id: 'FirmMembershipCertificate', DocNameEn: "Certificate of Membership (self-attested May date) issued by the Institute of Chartered Accountants of India.", DocNameHi: "भारतीय चार्टर्ड अकाउंटेंट संस्थान द्वारा जारी 'सर्टिफिकेट ऑफ मैम्बरशिप' (स्वप्रमाणित मय दिनांक )", FileName: "", IsFileUploaded: false, IsShow: true });
    $scope.RegistrationDocuments.push({ Id: 'FirmRegistrationCertificate', DocNameEn: "Latest certificate of registration number and date issued by the Institute of Chartered Accountants of Indian audit firm (Firm Constitution Certificate)", DocNameHi: "लेखा परीक्षा फर्म का भारतीय चार्टर्ड अकाउंटेंट संस्थान द्वारा जारी पंजीयन क्रमांक एवं दिनांक का नवीनतम प्रमाण पत्र(Firm Constitution Certificate)", FileName: "", IsFileUploaded: false, IsShow: true });
    //$scope.RegistrationDocuments.push({ Id: 'FeeChallan', DocNameEn: "Copy of challan for registration fee of Rs. 10000", DocNameHi: "पंजीकरण शुल्क राशि रू 10000 के चालान की प्रति", FileName: "", IsFileUploaded: false, IsShow: true });
    $scope.RegistrationDocuments.push({ Id: 'FirmRegistrationNABARD', DocNameEn: "If the firm is registered in the panel of NABARD, then registration number and date certificate (self attested May date)", DocNameHi: "यदि फर्म नाबार्ड के पैनल में पंजीकृत है तो पंजीकरण क्रमांक एवं दिनांक का प्रमाण पत्र (स्वप्रमाणित मय दिनांक)", FileName: "", IsFileUploaded: false, IsShow: false });
    $scope.RegistrationDocuments.push({ Id: 'FirmRegistrationRBI', DocNameEn: "If the firm is registered in the panel of Reserve Bank of India, then registration number and date certificate (self attested May date)", DocNameHi: "यदि फर्म भारतीय रिजर्व बैंक के पैनल में पंजीकृत है तो पंजीकरण क्रमांक एवं दिनांक का प्रमाण पत्र (स्वप्रमाणित मय दिनांक)", FileName: "", IsFileUploaded: false, IsShow: false });
    $scope.RegistrationDocuments.push({ Id: 'FirmTermsConditions', DocNameEn: "A copy of the terms and conditions for registration of a Chartered Accountant-Auditor or Audit Firm, signed with the firms seal on each page in the agreed form.", DocNameHi: "चार्टर्ड अकाउंटेंट-लेखा परीक्षक या लेखा परीक्षा फर्म के पंजीयन हेतु नियम एवं शर्तो की प्रति , सहमति स्वरूप प्रत्येक पृष्ठ पर फर्म की मुहर सहित हस्ताक्षर", FileName: "", IsFileUploaded: false, IsShow: true });
    $scope.RegistrationDocuments.push({ Id: 'FirmDiplomaCertificate', DocNameEn: "Self-attested copy of certificates of diploma in system audit or certificate in system audit", DocNameHi: "डिप्लोमा इन सिस्टम ऑडिट अथवा सर्टिफिकेट इन सिस्टम ऑडिट के प्रमाण-पत्रों की स्वप्रमाणित प्रति", FileName: "", IsFileUploaded: false, IsShow: false });

    $scope.RegistrationDocumentsCopy = $scope.RegistrationDocuments.filter(function (x) { return x.show });

    $scope.SetDocuments = function (id) {
        if (id == 'FirmRegistrationRBI') {
            $scope.RegistrationDocuments.filter(function (x) { return x.Id === id })[0].IsShow = $scope.Empanelled.IsRBIFirm;
        } else if (id == 'FirmRegistrationNABARD') {
            $scope.RegistrationDocuments.filter(function (x) { return x.Id === id })[0].IsShow = $scope.Empanelled.IsNabardFirm;
        } else if (id == 'FirmDiplomaCertificate') {
            $scope.RegistrationDocuments.filter(function (x) { return x.Id === id })[0].IsShow = $scope.Empanelled.IsDiplomaInSystemAudit;
        }
        $scope.RegistrationDocumentsCopy = $scope.RegistrationDocuments.filter(function (x) { return x.IsShow });
    }

    $scope.ChangeLVOFile = function (id,type, index, filetype) {
        if (type == 1) {
            if (filetype == 'PracticeCertificate') {
                $scope.PartnerList[index].IsUploadPracticeCertificateFile = false;
                $scope.PartnerList[index].PracticeCertificateFileName = "";
            } else if (filetype == 'MembershipCertificate') {
                $scope.PartnerList[index].IsUploadMembershipCertificateFile = false;
                $scope.PartnerList[index].MembershipCertificateFileName = "";
            } else if (filetype == 'AuditCertificate') {
                $scope.PartnerList[index].IsUploadAuditCertificateFile = false;
                $scope.PartnerList[index].AuditCertificateFileName = "";
            }
        } else if (type == 2) {
            $scope.RegistrationDocuments[index].IsFileUploaded = false;
            $scope.RegistrationDocuments[index].FileName = "";
        }
        $("#" + id).val("");
    }


    $scope.GetFeeVerification = function (AuditorIdENC, EMitraAppRequestID, SERICEID, UniqueId) {
        $(".blockUI").show();
        $http.post(SiteUrl + "/Citizen/GetTransactionFinalStatus", { "AuditorId": AuditorIdENC, "EMitraAppRequestID": EMitraAppRequestID, "SERICEID": SERICEID, "UniqueId": UniqueId }).
            success(function (result, status) {
                $(".blockUI").hide();
                //var Msg = '';
                //Msg += '\n <br /> STATUS: ' + (result.MSG == undefined ? result.STATUS : result.MSG);
                //Msg += '\n <br /> RECEIPTNO: ' + result.RECEIPTNO;
                //Msg += '\n <br /> DATE: ' + result.TRANSACTIONDATE;
                //Msg += '\n <br /> AMOUNT: ' + (result.AMT == undefined ? result.PAIDAMOUNT : result.AMT);
                //Msg += '\n <br /> TRANSACTIONSTATUS: ' + (result.TRANSACTIONSTATUS == undefined ? result.RESPONSEMESSAGE : result.TRANSACTIONSTATUS);
                //ShowModelAlert("Transaction Status", Msg);
                //$scope.PrintShow = true;
                //$scope.FailedTransactionList = [];
                window.location.href = window.location.href;

            }).
            error(function (data, status) {
                $(".blockUI").hide();
                $scope.FailedTransactionList = [];
                alert('Error while fetching failed transactionList data.');
            });
    }


});

MasterMod.directive('validfile', function validFile() {
    var validFormats = ['jpg', 'gif'];
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$validators.validFile = function () {
                elem.on('change', function () {
                    if (elem[0].files.length > 0) {
                        const fsize = elem[0].files[0].size;
                        const file = Math.round((fsize / 1024));
                        // The size of the file. 
                        if (file >= 4096) {
                            ShowOkMessage("Warning", scope == 'en-us' ? "File too Big, please select a file less than 4mb" : "फ़ाइल बहुत बड़ी है, कृपया 4mb से कम फ़ाइल का चयन करें", function () { });
                            elem[0].value = "";
                        } else {
                            if (scope.FileType == 1) {
                                if (scope.FileTypeName == 'PracticeCertificate') {
                                    scope.PartnerList[scope.FileTypeIndex].IsUploadPracticeCertificateFile = true;
                                    scope.PartnerList[scope.FileTypeIndex].PracticeCertificateFileName = '';
                                } else if (scope.FileTypeName == 'MembershipCertificate') {
                                    scope.PartnerList[scope.FileTypeIndex].IsUploadMembershipCertificateFile = true;
                                    scope.PartnerList[scope.FileTypeIndex].MembershipCertificateFileName = '';
                                } else if (scope.FileTypeName == 'AuditCertificate') {
                                    scope.PartnerList[scope.FileTypeIndex].IsUploadAuditCertificateFile = true;
                                    scope.PartnerList[scope.FileTypeIndex].AuditCertificateFileName = '';
                                }
                            } else
                                if (scope.FileType == 2) {
                                    scope.RegistrationDocuments.filter(function (x) { return x.IsShow })[scope.FileTypeIndex].IsFileUploaded = true;
                                    scope.RegistrationDocuments.filter(function (x) { return x.IsShow })[scope.FileTypeIndex].FileName = "";
                                }
                            scope.$apply();
                        }
                    }

                });
            };
        }
    };



});

MasterMod.directive("datepicker", function () {
    function link(scope, element, attrs) {
        // CALL THE "datepicker()" METHOD USING THE "element" OBJECT.
       // alert(element.id);
        $("#" + attrs.id).datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: $("#hdnRegstrationDate").val(),
            yearRange: "c-10:c+0"
        });
    }

    return {
        require: 'ngModel',
        link: link
    };
});





