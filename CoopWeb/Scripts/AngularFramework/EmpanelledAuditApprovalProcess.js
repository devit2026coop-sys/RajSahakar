
var MasterMod = angular.module('EmpanelledAuditApprovalProcessModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("EmpanelledAuditApprovalProcessCont", function ($scope, $http, $rootScope) {
    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 50,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }
    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
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
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else {
                if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }
            if (indx == ln) {
                // Added condition to fetch data for the current financial year on 13-04-2026
                $scope.DataPaging.searchParameters.push({
                    Key: "FinancialYear",
                    Value: $scope.CurrentFinYear
                });

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
    $scope.FinancialYearList = [];
    $scope.StatusList = [];

    $scope.StatusList.push({ Id: 1, NameEn: "Submit for Approval", NameHi: "Submit for Approval" });
    //$scope.StatusList.push({ Id: 2, NameEn: "Request for Payment", NameHi: "Request for Payment" });
    // $scope.StatusList.push({ Id: 3, NameEn: "Payment Recieved", NameHi: "Payment Recieved" });
    $scope.StatusList.push({ Id: 4, NameEn: "Objection", NameHi: "Objection" });
    $scope.StatusList.push({ Id: 5, NameEn: "Responce On Objection", NameHi: "Responce On Objection" });
    $scope.StatusList.push({ Id: 6, NameEn: "Reject", NameHi: "Reject" });
    $scope.StatusList.push({ Id: 8, NameEn: "Send Request For Payment", NameHi: "Send Request For Payment" });
    $scope.StatusList.push({ Id: 7, NameEn: "Approved", NameHi: "Approved" });

    //$scope.StatusList.push({ Id: 1, NameEn: "Submit for Approval", NameHi: "Submit for Approval" });
    //$scope.StatusList.push({ Id: 2, NameEn: "Request for Payment", NameHi: "Request for Payment" });
    //$scope.StatusList.push({ Id: 3, NameEn: "Objection", NameHi: "Objection" });
    //$scope.StatusList.push({ Id: 4, NameEn: "Reject", NameHi: "Reject" });
    //$scope.StatusList.push({ Id: 5, NameEn: "Approved", NameHi: "Approved" });

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
    $scope.PartnerList.push({ "AuditPartnershipId": 0, "Name": "", "PartnerName": "", "MembershipNo": "", "MembershipDate": "" });


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
            $scope.PartnerList.push({ "Name": "", "PartnerName": "", "MembershipNo": "", "MembershipDate": "" });
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
            message += $scope._language == 'en-us' ? '<br />Please enter ApplicantNameEn.' : '<br />कृपया आवेदक(इंग्लिश) का नाम दर्ज करें।';
        }
        if ($scope.Empanelled.ApplicantNameHi == '' || $scope.Empanelled.ApplicantNameHi == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter ApplicantNameHi.' : '<br />कृपया आवेदक(हिंदी) का नाम दर्ज करें।';
        }

        if ($scope.Empanelled.RegistrationNo == '' || $scope.Empanelled.RegistrationNo == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter RegistrationNo.' : '<br />कृपया पंजीकरण नंबर दर्ज करें।';
        }
        if ($("#RegDateID").val() == '' || $("#RegDateID").val() == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter RegistrationDate.' : '<br />कृपया पंजीकरण तिथि दर्ज करें।';
        }

        if ($scope.Empanelled.AddressEn == '' || $scope.Empanelled.AddressEn == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter AddressEn.' : '<br />कृपया पता(इंग्लिश) दर्ज करें।';
        }
        if ($scope.Empanelled.AddressHi == '' || $scope.Empanelled.AddressHi == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter AddressHi.' : '<br />कृपया पता(हिंदी) दर्ज करें।';
        }


        if ($scope.Empanelled.PinCode == '' || $scope.Empanelled.PinCode == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter PinCode.' : '<br />कृपया पिनकोड दर्ज करें।';
        }
        if ($scope.Empanelled.ContactNo == '' || $scope.Empanelled.ContactNo == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter ContactNo.' : '<br />कृपया संपर्क नंबर दर्ज करें।';
        }

        if ($scope.Empanelled.FaxNo == '' || $scope.Empanelled.FaxNo == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter FaxNo.' : '<br />कृपया फैक्स नंबर दर्ज करें।';
        }
        if ($scope.Empanelled.MobileNo == '' || $scope.Empanelled.MobileNo == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter MobileNo.' : '<br />कृपया मोबाइल नंबर दर्ज करें।';
        }
        if ($scope.Empanelled.EmailId == '' || $scope.Empanelled.EmailId == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter EmailId.' : '<br />कृपया ईमेल आईडी दर्ज करें।';
        }

        if ($scope.Empanelled.RBICode == '' || $scope.Empanelled.RBICode == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter RBI Unique Code.' : '<br />कृपया उपयोगकर्ता का नाम दर्ज करें।';
        }
        if ($("#RegistrationDateID").val() == '' || $("#RegistrationDateID").val() == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter RBI-Registration Date.' : '<br />कृपया उपयोगकर्ता का नाम दर्ज करें।';
        }
        if ($scope.Empanelled.NabardCode == '' || $scope.Empanelled.NabardCode == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter Nabard Panel Code.' : '<br />कृपया उपयोगकर्ता का नाम दर्ज करें।';
        }
        if ($("#NbardRegistrationDateID").val() == '' || $("#NbardRegistrationDateID").val() == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter Nabard-RegistrationDate.' : '<br />कृपया उपयोगकर्ता का नाम दर्ज करें।';
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



        if ($scope.Empanelled.Isshowfile == '' || $scope.Empanelled.Isshowfile == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please select Course Complete.' : '<br />कृपया कोर्स पूरा का प्रकार चयन करें।';
        }
        else {
            if ($scope.Empanelled.Isshowfile == '1') {
                if ($scope.Empanelled.FileName == '' || $scope.Empanelled.FileName == undefined) {
                    message += $scope._language == 'en-us' ? '<br />Please select Course Completed File.' : '<br />कृपया कोर्स कम्प्लीट फाइल का चयन करें।';
                }
            }
        }


        if ($scope.Empanelled.ExpieranceAreaRemark == '' || $scope.Empanelled.ExpieranceAreaRemark == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter Expierance Area Remark.' : '<br />कृपया व्यय क्षेत्र टिप्पणी दर्ज करें।';
        }
        if ($scope.Empanelled.Ammount == '' || $scope.Empanelled.Ammount == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter Ammount.' : '<br />कृपया अम्माउंट दर्ज करें।';
        }
        if ($scope.Empanelled.AmmountDetails == '' || $scope.Empanelled.AmmountDetails == undefined) {
            message += $scope._language == 'en-us' ? '<br />Please enter AmmountDetails.' : '<br />कृपया अम्माउंट विवरण दर्ज करें।';
        }
        if (message.length > 29) {
            ShowOkMessage("Auditor Request", message, function () { });
            return false;
        }
        return true;
    }

    $scope.PartialSubmit = function (status) {

        if (status == false) {
            if ($scope.ValidatePartnerList(status)) {
                if ($scope.ValidateAuditFirmList(status)) {
                    if ($scope.ValidateTraningDetailsList(status)) {
                        $scope.SaveUser(status);
                    }
                }
            };
        }
        else {
            if ($scope.validateAuditEmpanelled(status)) {
                if ($scope.ValidateAuditFirmList(status)) {
                    if ($scope.ValidateTraningDetailsList(status)) {
                        $scope.SaveUser(status);
                    }
                }
            };
        }
        //$scope.SaveUser(status);
    }

    $scope.SaveUser = function (status) {
        if ($scope.Empanelled.SetStatus == '' || $scope.Empanelled.SetStatus == undefined) {
            ShowOkMessage("Auditor Request", $scope._language == 'en-us' ? '<br />Please select application status.' : '<br />Please select application status।', function () { });
        } else
            if ($scope.Empanelled.remark == '' || $scope.Empanelled.remark == undefined) {
                ShowOkMessage("Auditor Request", $scope._language == 'en-us' ? '<br />Please enter remarks for application status.' : '<br />कृपया आवेदन की स्थिति के लिए टिप्पणी दर्ज करें।', function () { });
            } else {

                //$scope.StatusList.push({ Id: 1, NameEn: "Submit for Approval", NameHi: "Submit for Approval" });
                //$scope.StatusList.push({ Id: 2, NameEn: "Request for Payment", NameHi: "Request for Payment" });
                //$scope.StatusList.push({ Id: 3, NameEn: "Payment Recieved", NameHi: "Payment Recieved" });
                //$scope.StatusList.push({ Id: 4, NameEn: "Objection", NameHi: "Objection" });
                //$scope.StatusList.push({ Id: 5, NameEn: "Responce On Objection", NameHi: "Responce On Objection" });
                //$scope.StatusList.push({ Id: 6, NameEn: "Reject", NameHi: "Reject" });
                //$scope.StatusList.push({ Id: 8, NameEn: "Send Request For Payment", NameHi: "Send Request For Payment" });
                //$scope.StatusList.push({ Id: 7, NameEn: "Approved", NameHi: "Approved" });


                var message = "";
                var messageSuccess = "";
                if ($scope.Empanelled.SetStatus == 8) {
                    message = $scope._language == 'en-us' ? 'Are you sure you want to request for payment to the applicant by approving this application?' : 'क्या आप इस एप्लिकेशन को स्वीकृत कर आवेदक को पेमेंट के लिए अनुरोध करना चाहते हैं?';
                    messageSuccess = $scope._language == 'en-us' ? 'The application has been accepted successfully.' : 'आवेदन सफलतापूर्वक स्वीकार कर लिया गया है।';
                } else
                    if ($scope.Empanelled.SetStatus == 7) {
                        message = $scope._language == 'en-us' ? 'Are you sure you want to Approve this application?' : 'क्या आप इस एप्लिकेशन को स्वीकृत करना चाहते हैं?';
                        messageSuccess = $scope._language == 'en-us' ? 'The application has been accepted successfully.' : 'आवेदन सफलतापूर्वक स्वीकार कर लिया गया है।';
                    } else if ($scope.Empanelled.SetStatus == 4) {
                        message = $scope._language == 'en-us' ? 'Are you sure you want to send Objection this application?' : 'क्या आप इस एप्लिकेशन पर आपत्ति लगाना चाहते हैं?';
                        messageSuccess = $scope._language == 'en-us' ? 'Objection on this application has been successfully submited.' : 'इस आवेदन पर आपत्ति सफलतापूर्वक दर्ज कर ली गई है।';
                    } if ($scope.Empanelled.SetStatus == 6) {
                        message = $scope._language == 'en-us' ? 'Are you sure you want to Reject this application?' : 'क्या आप वाकई इस एप्लिकेशन को अस्वीकार करना चाहते हैं?';
                        messageSuccess = $scope._language == 'en-us' ? 'This application has been Rejected successfully.?' : 'इस आवेदन को सफलतापूर्वक अस्वीकार कर दिया गया है।';
                    }

                ShowAlertMessage("Alert", message, function () {
                    $(".se-pre-con").fadeIn("slow");
                    var formData = new FormData();
                    var applicationDetails = {
                        AuditEmpanelledId: $scope.Empanelled.AuditEmpanelledId,
                        Action: 1,
                        Status: $scope.Empanelled.SetStatus,
                        remark: $scope.Empanelled.remark
                    };
                    formData.append("Applicationdata", JSON.stringify(applicationDetails));
                    formData.append("AuditformPartnership1", JSON.stringify($scope.PartnerList));
                    formData.append("AuditfirmAddressdetails1", JSON.stringify($scope.AuditFirmList));
                    formData.append("FirmParemenentTraniningDetails1", JSON.stringify($scope.TraningDetailsList));
                    $(".se-pre-con").fadeIn("slow");
                    $http.post(SiteUrl + "/Auditor/SaveAuditEmpanelled", JSON.stringify({ ApplicationModel: applicationDetails })).success(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        if (data.Result != "-1" && data.Result != "0") {
                            ShowOkMessage("Success", messageSuccess, function () {
                                window.location.href = "../auditor/EmpanelledAuditApprovalProcess";
                            });
                        } else {
                            ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें| ", function () {

                            });
                        }
                        // $scope.$apply();
                    }).
                        error(function (data, status) {
                            $(".se-pre-con").fadeOut("slow");
                            ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें| ", function () {
                                $scope.$apply();
                            });
                        });
                    //$scope.$apply();
                });
            }

    }


    $scope.GetAuditorEmpanelledList = function () {
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/Auditor/GetAuditEmpanelledList", JSON.stringify({ Details: jsonParameters })).
            success(function (data, status) {
                $scope.GetAuditorEmpanelled = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $scope.GetAuditorEmpanelled.forEach(function (x, i) {
                    x.IsShow = true;
                    x.IsChecked = false;
                });
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.GetAuditorEmpanelled = [];
                alert('Error while Society Election Detail data.');
            });
    }

    $scope.ExportData = function () {
        $(".blockUI").fadeIn("slow");
        $scope.ReportExportList = [];

        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/Auditor/ExportAuditEmpanelled", JSON.stringify({ Details: jsonParameters })).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");



                var uri = 'data:application/vnd.ms-excel;base64,'
                    , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
                    , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
                    , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }


                var ctx = { worksheet: name || 'Worksheet', table: data }
                window.location.href = uri + base64(format(template, ctx));


                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
            });

    }
    $scope.EsignData = function () {

        var AuditorsId = "";
        $scope.GetAuditorEmpanelled.forEach(function (x, i) {
            if (x.IsChecked) {
                AuditorsId += (AuditorsId == "" ? "" : ",") + x.AuditEmpanelledId;
            }
        });
        if (AuditorsId == "") {
            ShowOkMessage("", "Please select at least one record.", function () {

            });
        } else {
            AadhaarAuth("", $http, "", true, function (authData) {
                $(".blockUI").fadeIn("slow");
                $scope.ReportExportList = [];
                var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
                $http.post(SiteUrl + "/Auditor/GenerateAuditEmpanelledCertificate", JSON.stringify({ Details: jsonParameters, Auditors: AuditorsId, TXN: authData.TransId })).
                    success(function (data, status) {
                        //$(".blockUI").fadeOut("slow");
                        $scope.pageRequest($scope.DataPaging.currentPageID, $scope.GetAuditorEmpanelledList);
                    }).
                    error(function (data, status) {
                        $(".blockUI").fadeOut("slow");
                    });
            });
        }
    }
    $scope.base64ToArrayBuffer = function (base64) {
        var binaryString = window.atob(base64);
        var binaryLen = binaryString.length;
        var bytes = new Uint8Array(binaryLen);
        for (var i = 0; i < binaryLen; i++) {
            var ascii = binaryString.charCodeAt(i);
            bytes[i] = ascii;
        }
        return bytes;
    }



    $scope.GetAuditorEmpanelledList();
    $scope.AuditorEmpView12 = function (AuditEmpanelledId) {
        $scope.Empanelled.AuditEmpanelledId = AuditEmpanelledId;
        $http.post(SiteUrl + "/Auditor/GetAuditEmpanelledDetailsById", JSON.stringify({ AuditEmpanelledID: AuditEmpanelledId })).
            success(function (data, status) {
                $scope.GetAuditorAllList = JSON.parse(data.DataList);
                $scope.Empanelled = $scope.GetAuditorAllList.Table[0];
                $scope.PartnerList = [];
                $scope.AuditFirmList = [];
                $scope.TraningDetailsList = [];
                if ($scope.Empanelled.ApplicantType == '4'
                ) {
                    $scope.Showpartnertbl = true;
                }
                $scope.Empanelled.Isshowfile = $scope.Empanelled.IsCompletecourse;

                if ($scope.Empanelled.Isshowfile == 1) {
                    $("#inlineRadio2").prop("checked", true);
                    $scope.Truefile = true;
                    $scope.Empanelled.IsUploadFileName = true;
                    $scope.Empanelled.FileName = $scope.Empanelled.CertificateName;
                }
                $scope.GetAuditorAllList.Table1.forEach(function (itm, index) {
                    $scope.PartnerList.push({
                        AuditPartnershipId: itm.AuditPartnershipId,
                        Name: itm.Name,
                        PartnerName: itm.PartnerName,
                        MembershipNo: itm.MembershipNumber,
                        MembershipDate: itm.MembershipDate

                    });
                });
                $scope.GetAuditorAllList.Table2.forEach(function (itm, index) {
                    $scope.AuditFirmList.push({
                        AuditfirmAddressId: itm.AuditfirmAddressId,
                        Name: itm.Name,
                        MobileNo: itm.MobileNo
                    });
                });

                $scope.GetAuditorAllList.Table3.forEach(function (itm, index) {
                    $scope.TraningDetailsList.push({
                        FirmParemenentTraniningId: itm.FirmParemenentTraniningId,
                        AssistantName: itm.AssistantName,
                        Education: itm.Education,
                        Experiance: itm.Experiance
                    });
                });



                $scope.EmpanelledDivForm = true;
                $scope.EmpanelledDiv = false;
                $scope.CheckIsubmit = false;

            }).
            error(function (data, status) {
                $scope.GetAuditorAllList = [];
                alert('Error while Society Election Detail data.');
            });

    }
    $scope.StatusListUpdateList = [];
    $scope.AuditorPayment = [];
    $scope.AuditorEmpView = function (AuditEmpanelledId) {
        $scope.Empanelled.AuditEmpanelledId = AuditEmpanelledId;
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Auditor/GetAuditEmpanelledDetailsById", JSON.stringify({ AuditEmpanelledID: AuditEmpanelledId })).
            success(function (data, status) {
                $scope.GetAuditorAllList = JSON.parse(data.DataList);
                $scope.ShowApply = false;
                $scope.Empanelled = $scope.GetAuditorAllList.Table[0];
                $scope.AuditorPayment = JSON.parse($scope.Empanelled.ResponseString);

                //$scope.StatusList.push({ Id: 1, NameEn: "Submit for Approval", NameHi: "Submit for Approval" });
                //$scope.StatusList.push({ Id: 2, NameEn: "Request for Payment", NameHi: "Request for Payment" });
                //$scope.StatusList.push({ Id: 3, NameEn: "Payment Recieved", NameHi: "Payment Recieved" });

                //$scope.StatusList.push({ Id: 4, NameEn: "Objection", NameHi: "Objection" });
                //$scope.StatusList.push({ Id: 5, NameEn: "Responce On Objection", NameHi: "Responce On Objection" });
                //$scope.StatusList.push({ Id: 6, NameEn: "Reject", NameHi: "Reject" });
                //$scope.StatusList.push({ Id: 8, NameEn: "Send Request For Payment", NameHi: "Send Request For Payment" });
                //$scope.StatusList.push({ Id: 7, NameEn: "Approved", NameHi: "Approved" });
                $scope.StatusListUpdateList = [];
                if ($scope.Empanelled.Status == 1 || $scope.Empanelled.Status == 5) {
                    $scope.StatusListUpdateList = $scope.StatusList.filter(function (x) { return x.Id == 8 || x.Id == 4 || x.Id == 6 });
                } else if ($scope.Empanelled.Status == 2) {
                    $scope.StatusListUpdateList = $scope.StatusList.filter(function (x) { return x.Id == 6 });
                } else if ($scope.Empanelled.Status == 3) {
                    //$scope.StatusListUpdateList = $scope.StatusList.filter(function (x) { return x.Id == 8 });
                    $scope.StatusListUpdateList = $scope.StatusList.filter(function (x) { return x.Id ==7 });
                } else if ($scope.Empanelled.Status == 4) {
                    $scope.StatusListUpdateList = [];//$scope.SatatusList.filter(function (x) { return x.Id == 6 || x.Id== });
                }


                if ($scope.Empanelled.ApplicantType == 2) {
                    $scope.Empanelled.ApplicantType1 = $scope.Empanelled.IsPartnerFirm == null || !$scope.Empanelled.IsPartnerFirm ? 3 : 4;
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

                $scope.IsShowPreview = !($scope.Empanelled.Status == 0 || $scope.Empanelled.Status == 2);

                //$scope.Empanelled.Acceptence = $scope.Empanelled.Status == 0 || $scope.Empanelled.Status == 2;
                //$scope.Empanelled.Acceptence2 = $scope.Empanelled.Status == 0 || $scope.Empanelled.Status == 2;
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
                $scope.CheckIsubmit = $scope.Empanelled.IsSubmit;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.GetAuditorAllList = [];
                $(".se-pre-con").fadeOut("slow");
                alert('Error while Society Election Detail data.');
            });

    }


    $scope.ShowApplicationForm = function () {
        $scope.Empanelled = [];

        $scope.OtherPersonList = [];
        $scope.TraningDetailsList = [];
        $scope.AuditFirmList = [];
        $scope.PartnerList = [];

        $scope.OtherPersonList.push({ "PersonId": 0, "Name": "", "Address": "", "Post": "" });
        $scope.TraningDetailsList.push({ "FirmParemenentTraniningId": 0, "AssistantName": "", "Education": "", "Experiance": "" });
        $scope.AuditFirmList.push({ "AuditfirmAddressId": 0, "Name": "", "MobileNo": "" });
        $scope.PartnerList.push({ "AuditPartnershipId": 0, "Name": "", "PartnerName": "", "MembershipNo": "", "MembershipDate": "" });

        $scope.CheckIsubmit = false;

        $scope.EmpanelledDivForm = true;
        $scope.EmpanelledDiv = false;
    }

    $scope.Showpartnertbl = false;
    $scope.ShowPartnerlist = function (id) {
        if (id == 4)
            $scope.Showpartnertbl = true;
        else
            $scope.Showpartnertbl = false;
    }


    //File Save--start

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

    $scope.GetFinancialYear = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Auditor/GetSectorDetailsByIsCredit?actionName=GetFinYearDetail")
            .success(function (data) {
                if (data.Result.Data && data.Result.Data.FinYearData) {
                    $scope.FinancialYearList = JSON.parse(data.Result.Data.FinYearData);
                    if ($scope.FinancialYearList.length > 0) {
                        $scope.CurrentFinYear = $scope.FinancialYearList[0].FinYearId;
                    }
                } else {
                    $scope.FinancialYearList = [];
                }
                // calling call back
                $scope.searchRequest(function () {
                    $scope.GetAuditorEmpanelledList();
                });

                //$scope.searchRequest();
                $(".se-pre-con").fadeOut("slow");
            }).error(function () {
                $(".se-pre-con").fadeOut("slow");
            });
    };

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
        //$scope._language = 'en-us';

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
        $scope.GetFinancialYear();
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

    $scope.RegistrationDocuments = [];
    $scope.RegistrationDocuments.push({ Id: 'FirmPracticeCertificate', DocNameEn: "Certificate of Practice (self-attested May date) issued by the Institute of Chartered Accountants of India.", DocNameHi: "भारतीय चार्टर्ड अकाउंटेंट संस्थान द्वारा जारी 'सर्टिफिकेट  ऑफ प्रेक्टिस'  (स्वप्रमाणित मय दिनांक )", FileName: "", IsFileUploaded: false, IsShow: true });
    $scope.RegistrationDocuments.push({ Id: 'FirmMembershipCertificate', DocNameEn: "Certificate of Membership (self-attested May date) issued by the Institute of Chartered Accountants of India.", DocNameHi: "भारतीय चार्टर्ड अकाउंटेंट संस्थान द्वारा जारी 'सर्टिफिकेट ऑफ मैम्बरशिप' (स्वप्रमाणित मय दिनांक )", FileName: "", IsFileUploaded: false, IsShow: true });
    $scope.RegistrationDocuments.push({ Id: 'FirmRegistrationCertificate', DocNameEn: "Latest certificate of registration number and date issued by the Institute of Chartered Accountants of Indian audit firm (Firm Constitution Certificate)", DocNameHi: "लेखा परीक्षा फर्म का भारतीय चार्टर्ड अकाउंटेंट संस्थान द्वारा जारी पंजीयन क्रमांक एवं दिनांक का नवीनतम प्रमाण पत्र(Firm Constitution Certificate)", FileName: "", IsFileUploaded: false, IsShow: true });
    // $scope.RegistrationDocuments.push({ Id: 'FeeChallan', DocNameEn: "Copy of challan for registration fee of Rs. 12000", DocNameHi: "पंजीकरण शुल्क राशि रू 12000 के चालान की प्रति", FileName: "", IsFileUploaded: false, IsShow: true });
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
    $scope.SelectForEsign = function () {
        if ($scope.GetAuditorEmpanelled.filter(function (x) { return x.IsChecked }).length >= 10) {
            $scope.GetAuditorEmpanelled.forEach(function (x, i) {
                if (x.IsChecked) {
                    x.IsShow = true;
                } else {
                    x.IsShow = false;
                }
            });
        } else {
            $scope.GetAuditorEmpanelled.forEach(function (x, i) {
                x.IsShow = true;
            });
        }
    }
});

MasterMod.directive('validfile1', function validFile() {
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
                                scope.Empanelled.IsUploadFileName = true;
                                scope.Empanelled.FileName = "";
                            }
                            scope.$apply();
                        }
                    }

                });
            };
        }
    };



});






