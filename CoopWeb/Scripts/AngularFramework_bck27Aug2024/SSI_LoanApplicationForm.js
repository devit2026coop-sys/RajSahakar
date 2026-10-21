var CommityRegistrtionModule = angular.module('SSILoanApplicationMod', ['angularUtils.directives.dirPagination']);

CommityRegistrtionModule.controller('SSILoanApplicationCnt', function ($scope, $http, $rootScope) {

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
        if ($scope.Filter.FilterType == 'SubmissionDate') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            $scope.AddCalender();
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
        $scope.GetElectionSocietyListData();
    }
    /*------------End Paging and Filter-----------------*/

    $scope.IsSubmiApplication = false;
    $scope.LoanApplicationList = [];
    // $scope.LoanApplication.LOAN_ID = '0';
    $scope.GetLoanApplicationList = function () {
        $scope.ShowSocietyList = true;
        $scope.ShowVoterList = false;
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/pldb/GetLoanApplicationList", JSON.stringify({ Details: jsonParameters })).
            success(function (data, status) {

                $scope.LoanApplicationList = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching list")
                $(".blockUI").fadeOut("slow");
            });
    }
    //$scope.GetLoanApplicationList();
    $scope.GetLoanApplicationDetails = function (table, loanId) {
        $scope.ShowSocietyList = true;
        $scope.ShowVoterList = false;
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/pldb/GetLoanApplicationDetails", JSON.stringify({ Table: table, LoanId: loanId })).
            success(function (data, status) {
                $scope.ElectionSocietyListData = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching list")
                $(".blockUI").fadeOut("slow");
            });
    }
    $scope.dvApplicationForm = false;
    $scope.divLoanApplicationList = true;
    $scope.ShowApplicationForm = function () {
        $scope.dvApplicationForm = true;
        $scope.divLoanApplicationList = false;
        $scope.ShowAddNewApplication = false;
    }
    $scope.ShowAddNewApplication = true;
    $scope.divUserRegistration = true;
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.LoanApplicationId = 0;
    $scope.MemberDatail = false;
    $scope.DairyDatail = false;
    $scope.divViewDairyFormShow = false;
    $scope.divParentDataSelection = true;
    $scope.LoanApplicationLetter = true;
    $scope.SelfDeclarationLetter = false;
    $scope.FarmerCategoryList = [];
    $scope.PurposeOfLoanList = []

    $scope.PropertyUnitOfArea = [{ "value": "1", "TextEn": "हैक्टर", "TextHi": "हैक्टर" }, { "value": "2", "TextEn": "बीघा", "TextHi": "बीघा" }, { "value": "3", "TextEn": "एकड़ जमीन", "TextHi": "एकड़ जमीन" }, { "value": "4", "TextEn": "वर्ग फुट", "TextHi": "वर्ग फुट" }, { "value": "5", "TextEn": "वर्ग गज", "TextHi": "वर्ग गज" }];

    /*------- Start Loan Applicant code block--------------*/


    $scope.Member = {
        index: 0,
        Id: "0",
        ApplicantAadharNumber: "",
        ApplicantName: "",
        ApplicantAge: "",
        Educated_Experienced: "",
        IsEmployed: "",
        Source: "",
        MonthlyIncome: "",
        Cast: 0,

        CastList: [],
        IsDelete: false
    }
    $scope.ResetMember = function () {
        $scope.Member = {
            index: 0,
            Id: "0",
            ApplicantAadharNumber: "",
            ApplicantName: "",
            ApplicantAge: "",
            Educated_Experienced: "",
            IsEmployed: "",
            Source: "",
            MonthlyIncome: "",
            Cast: 0,

            CastList: [],
            IsDelete: false
        }
    }
    $scope.LoanMembers = [];
    $scope.SchemeUrlId = "";
    $scope.LoanMembers.push($scope.Member);

    $scope.LoanId = "";
    $scope.getSchemeByUrl = function () {
        var url = window.location.href.split('?');
        if (url.length > 1) {
            var url2 = url[1].split('&')[0].split('=');
            $scope.SchemeUrlId = url2[1];
            if (url[1].split('&').length > 1) {
                $scope.LoanId = url[1].split('&')[1].split('=')[1];
            }

        } else {
            $scope.SchemeUrlId = 0;
            $scope.LoanId = "";
        }
    }

    $scope.getSchemeByUrl();
    $scope.AddNewMember = function () {
        if ($scope.ValidateAddMember(2)) {

            $scope.ResetMember();
            $scope.LoanMembers.push($scope.Member);
        }
    }

    $scope.RemoveMemberRow = function (index) {
        ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove member from list.' : 'क्या आप वाकई सदस्य को सूची से हटाना चाहते हैं', function () {
            //if ($scope.LoanMembers.length == 1) {
            //    $scope.ResetMember();
            //    $scope.LoanMembers = [];
            //    $scope.LoanMembers.push($scope.Member);
            //} else {

            //    $scope.Members = $scope.LoanMembers;
            //    $scope.LoanMembers = [];
            //    $scope.Members.forEach(function (item, indx) {
            //        if (indx != index) {
            //            $scope.LoanMembers.push(item);
            //        }
            //    });
            $scope.LoanMembers[index].IsDelete = true;
            //}
            $scope.$apply();

        });
    }

    $scope.ValidateAddMember = function (IsSubmit) {

        var message = "";//$scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblLoanMembersDetail").find('input[type="text"]').removeClass("notValid");
        $("#tblLoanMembersDetail").find('select').removeClass("notValid");
        if (IsSubmit == 1 && $('input[name="IsMainMember"]:checked').length == 0 && $('input[name="IsMainMember"]').length > 1) {
            message += $scope._language == 'en-us' ? '<br /> Please Choose main member from member list.' : '<br />कृपया सदस्य सूची से मुख्य सदस्य चुनें।';
        }

        if ($scope.MemberRegistration.PLDB_ID == '0') {
            message += $scope._language == 'en-us' ? '<br /> Please Select PLDB.' : '<br />कृपया पी.एल.डी.बी का चयन करें।';
            $("#ddlDistrict").addClass("notValid");
        }
        if ($scope.MemberRegistration.Branch_ID == '0') {
            message += $scope._language == 'en-us' ? '<br /> Please Select branch.' : '<br />कृपया ब्रांच का चयन करें।';
            $("#ddlblock").addClass("notValid");
        }

        $scope.LoanMembers.forEach(function (item, index) {

            // alert($("#tblLandMemberDetail").find("tr").eq(index+1).find('input[type="text"]').length);

            if (item.ApplicantAadharNumber == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Membership Number' : 'कृपया सदस्यता क्रमांक दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Membership Number.' : '<br />कृपया सदस्यता क्रमांक दर्ज करें।';
                }
                $("#tblLoanMembersDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(0).addClass("notValid");
            }
            //if (item.ApplicantName == '') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Name' : 'कृपया नाम दर्ज करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Enter Name.' : '<br />कृपया नाम दर्ज करें।';
            //    }
            //    $("#tblLoanMembersDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(1).addClass("notValid");
            //}
            //if (item.ApplicantAge == '') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Age' : 'कृपया उम्र दर्ज करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Enter Age.' : '<br />कृपया उम्र दर्ज करें।';
            //    }
            //    $("#tblLoanMembersDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(1).addClass("notValid");
            //}
            if (item.Educated_Experienced == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Select Applicant Type' : 'कृपया आवेदक शिक्षित/अनुभवी चयन करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Select Applicant Type.' : '<br />कृपया आवेदक शिक्षित/अनुभवी चयन करें।';
                }
                $("#tblLoanMembersDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(1).addClass("notValid");
            }



            if (item.IsEmployed == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Select Employee' : 'कृपया आवेदक कार्यरत है या नहीं चयन करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Select Employee.' : '<br />कृपया आवेदक कार्यरत है या नहीं चयन करें।';
                }
                $("#tblLoanMembersDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(1).addClass("notValid");
            }
            if (item.IsEmployed == 'Yes') {
                if (item.MonthlyIncome == '' || item.MonthlyIncome == 0) {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter the  Current Salary' : 'कृपया वर्तमान वेतन दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please Enter the  Current Salary.' : '<br />कृपया वर्तमान वेतन दर्ज करें।';
                    }
                    $("#tblLoanMembersDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(1).addClass("notValid");
                }
                if (item.Source == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Source' : 'कृपया स्रोत दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please Enter Source.' : '<br />कृपया स्रोत दर्ज करें।';
                    }
                    $("#tblLoanMembersDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(1).addClass("notValid");
                }
            }

            //if (item.Cast == '') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Applicant Cast' : 'कृपया आवेदक की जाति का चयन करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Select Applicant Cast.' : '<br />कृपया आवेदक की जाति का चयन करें।';
            //    }
            //    $("#tblLoanMembersDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(1).addClass("notValid");
            //}
        });

        if (message.length > 29) {
            ShowOkMessage("निम्नलिखित मान्य करें", message, function () { });
            return false;
        }
        return true;
    }
    
    $scope.PropertyDetails = {
        Village: "0",
        VillageList: [],
        GramPanchayat: "0",
        GramPanchayatList: [],
        Tehsil: "0",
        TehsilList: $scope.PLDBBranches,
        Khasara: "",
        ChahiBi: "",
        ChahiVi: "",
        BaraniBi: "",
        BaraniVi: "",
        OtherDetails: "",
        Specifications: "",

        Address: "",
        GuarantorPropertyTotalArea: "",
        GuarantorPropertyUnitOfArea: "",
        TotalArea: "",
        IsDelete: false
    }
    $scope.ResetPropertyDetails = function () {
        $scope.PropertyDetails = {
            Village: "0",
            VillageList: [],
            GramPanchayat: "0",
            GramPanchayatList: [],
            Tehsil: "0",
            TehsilList: $scope.PLDBBranches,
            Khasara: "",
            ChahiBi: "",
            ChahiVi: "",
            BaraniBi: "",
            BaraniVi: "",
            OtherDetails: "",
            Specifications: "",

            TotalArea: "",
            Address: "",
            GuarantorPropertyTotalArea: "",
            GuarantorPropertyUnitOfArea: "",
            IsDelete: false
        }
    }
    $scope.PropertyDetailsList = [];
    $scope.PropertyDetailsList.push($scope.PropertyDetails);

    $scope.AddNewPropertyDetails = function () {
        // if ($scope.ValidateAddMember(1)) {
        // if ($scope.ValidatetblPropertyDetails()) {
        if ($scope.ValidateLandDetails()) {
            $scope.ResetPropertyDetails();
            $scope.PropertyDetailsList.push($scope.PropertyDetails);
        }
        //  }
        //  }
    }

    $scope.RemovePropertyRow = function (index) {
        ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove details from list.' : 'क्या आप वाकई सूची से विवरण निकालना चाहते हैं', function () {


            $scope.PropertyDetailsList[index].IsDelete = true;

            $scope.$apply();
        });
    }

    $scope.ValidateLandDetails = function () {
        var message = "";//$scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblLoanMembersDetail").find('input[type="text"]').removeClass("notValid");
        $("#tblLoanMembersDetail").find('select').removeClass("notValid");

        $scope.PropertyDetailsList.forEach(function (item, index) {

            //Applicant Property
            if (item.Khasara == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Khasara Number' : 'कृपया आवेदक की प्रॉपर्टी का खसरा नंबर दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Khasara Number.' : '<br />कृपया आवेदक की प्रॉपर्टी का खसरा नंबर दर्ज करें|';
                }
                $("#txtKhasara_" + index).addClass("notValid");
            }
            else {
                $("#txtKhasara_" + index).removeClass("notValid");
            }

            if (item.TotalArea == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Total Area of Land' : 'कृपया आवेदक की प्रॉपर्टी का कुल क्षेत्रफल दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Total Area of Land.' : '<br />कृपया आवेदक की प्रॉपर्टी का कुल क्षेत्रफल दर्ज करें|';
                }
                $("#txtTotalArea_" + index).addClass("notValid");
            }
            else {
                $("#txtTotalArea_" + index).removeClass("notValid");
            }

            if (item.Address == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Applicant Address' : 'कृपया आवेदक की प्रॉपर्टी का पता दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Applicant Address.' : '<br />कृपया आवेदक की प्रॉपर्टी का पता दर्ज करें|';
                }
                $("#txtAddress_" + index).addClass("notValid");
            }
            else {
                $("#txtAddress_" + index).removeClass("notValid");
            }
        });

        if (message.length > 29) {
            ShowOkMessage("निम्नलिखित मान्य करें", message, function () { });
            return false;
        }
        return true;
    }


    /*------- End Loan  Applicant code block--------------*/



    /*------- Start Bank Details code block--------------*/

    $scope.LoanBankDetails = {
        Id: "0",
        BankName: "",
        AccountType: "",
        DateOfOpenAccount: "",
        LoanFacility: "",
        IsDelete: false,
    }
    $scope.ResetLoanBankDetails = function () {
        $scope.LoanBankDetails = {
            Id: "0",
            BankName: "",
            AccountType: "",
            DateOfOpenAccount: "",
            LoanFacility: "",
            IsDelete: false,
        }
    }
    $scope.LoadBankDetails = [];
    $scope.LoadBankDetails.push($scope.LoanBankDetails);

    $scope.AddBankDetail = function () {
        if ($scope.ValidateBankDetails()) {
            $scope.ResetLoanBankDetails();

            $scope.LoadBankDetails.push($scope.LoanBankDetails);


            //$("#DateOfOpenAccount").addClass("devcalender");
        }
    }

    $scope.RemoveLoadBankDetailsRow = function (index) {
        ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove member from list.' : 'क्या आप वाकई सदस्य को सूची से हटाना चाहते हैं', function () {
            $scope.LoadBankDetails[index].IsDelete = true;
            $scope.$apply();
        });
    }

    $scope.ValidateBankDetails = function () {
        var message = "";//$scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblLoanAmountDetail").find('input[type="text"]').removeClass("notValid");
        $("#tblLoanAmountDetail").find('select').removeClass("notValid");
        $scope.LoadBankDetails.forEach(function (item, index) {

            if (item.BankId == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Bank Name' : 'कृपया बैंक का नाम दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Bank Name.' : '<br />कृपया बैंक का नाम दर्ज करें|';
                }
                $("#txtBankName_" + index).addClass("notValid");
            }
            else {
                $("#txtBankName_" + index).removeClass("notValid");
            }

            if (item.AccountType == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Account Type' : 'कृपया खाता का प्रकार दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Account Type.' : '<br />कृपया खाता का प्रकार दर्ज करें|';
                }
                $("#txtAccountType_" + index).addClass("notValid");
            }
            else {
                $("#txtAccountType_" + index).removeClass("notValid");
            }

            //if (item.DateOfOpenAccount == '' && $("#DateOfOpenAccount").val() == '') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Date of Open Account' : 'कृपया खाता खोलने की तिथि दर्ज करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Enter Date of Open Account.' : '<br />कृपया खाता खोलने की तिथि दर्ज करें|';
            //    }
            //    $("#DateOfOpenAccount").addClass("notValid");
            //}
            if (item.LoanFacility == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Loan Facility' : 'कृपया ऋण सुविधा दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Loan Facility.' : '<br />कृपया ऋण सुविधा दर्ज करें|';
                }
                $("#txtLoanFacility_" + index).addClass("notValid");
            }
            else {
                $("#txtLoanFacility_" + index).removeClass("notValid");
            }


            //var DateOfOpenAccount = $("#DateOfOpenAccount").val();

            if (!checkDateFormat(item.DateOfOpenAccount)) {

                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Valid Date of Open Account' : 'कृपया खाता खोलने की वैध तिथि दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Valid Date of Open Account.' : '<br />कृपया खाता खोलने की वैध तिथि दर्ज करें|';
                }
                $("#DateOfOpenAccount_" + index).addClass("notValid");
            }
            else {
                $("#DateOfOpenAccount_" + index).removeClass("notValid");
            }

        });

        if (message.length > 29) {
            ShowOkMessage("निम्नलिखित मान्य करें", message, function () { });
            return false;
        }
        return true;
    }

    /*------- End Bank Details code block--------------*/



    /*------- Start Pre Loan Details code block--------------*/

    $scope.PreLoanDetails = {
        index: 0,
        Id: "0",
        Organization: "",
        PreLoanAmount: "",
        RestAmount: "",
        MonthlyInstallment: "",
        Tenure: "",
        IntrestRate: "",
        Source: "",
        IsDelete: false
    }
    $scope.ResetPreLoanDetails = function () {
        $scope.PreLoanDetails = {
            index: 0,
            Id: "0",
            Organization: "",
            PreLoanAmount: "",
            RestAmount: "",
            MonthlyInstallment: "",
            Tenure: "",
            IntrestRate: "",
            Source: "",
            IsDelete: false
        }
    }

    $scope.LoadPreLoanDetails = [];
    $scope.LoadPreLoanDetails.push($scope.PreLoanDetails);

    $scope.AddNewPreLoanDetails = function () {
        if ($scope.ValidateAddPreLoanDetails()) {

            // if ($scope.ValidateAddMember(1)) {
            //  if ($scope.ValidatetblPreLoanDetails()) {
            $scope.ResetPreLoanDetails();

            $scope.LoadPreLoanDetails.push($scope.PreLoanDetails);
            // }
            //}

        }
    }

    $scope.RemovePreLoanDetailsRow = function (index) {
        ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove member from list.' : 'क्या आप वाकई सदस्य को सूची से हटाना चाहते हैं', function () {
            $scope.LoadPreLoanDetails[index].IsDelete = true;
            $scope.$apply();
        });
    }

    $scope.ValidateAddPreLoanDetails = function (IsSubmit) {

        var message = "";//$scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblLoanMembersDetail").find('input[type="text"]').removeClass("notValid");
        $("#tblLoanMembersDetail").find('select').removeClass("notValid");

        if ($scope.MemberRegistration.PLDB_ID == '0') {
            message += $scope._language == 'en-us' ? '<br /> Please Select PLDB.' : '<br />कृपया पी.एल.डी.बी का चयन करें।';
            $("#ddlDistrict").addClass("notValid");
        }
        if ($scope.MemberRegistration.Branch_ID == '0') {
            message += $scope._language == 'en-us' ? '<br /> Please Select branch.' : '<br />कृपया ब्रांच का चयन करें।';
            $("#ddlblock").addClass("notValid");
        }

        $scope.LoadPreLoanDetails.forEach(function (item, index) {

            // alert($("#tblLandMemberDetail").find("tr").eq(index+1).find('input[type="text"]').length);

            if (item.Organization == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please enter the name of the committee / bank / institution' : 'कृपया समिति/बैंक/संस्था का नाम दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please enter the name of the committee / bank / institution.' : '<br />कृपया समिति/बैंक/संस्था का नाम दर्ज करें।';
                }
                $("#txtOrganization_" + index).addClass("notValid");
            }
            else {
                $("#txtOrganization_" + index).removeClass("notValid");
            }


            if (item.PreLoanAmount == '' || item.PreLoanAmount == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Previous Loan Amount' : 'कृपया पिछली ऋण राशि दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Previous Loan Amount.' : '<br />कृपया पिछली ऋण राशि दर्ज करें।';
                }
                $("#txtPreLoanAmount_" + index).addClass("notValid");
            }
            else {
                $("#txtPreLoanAmount_" + index).removeClass("notValid");
            }

            if (item.Tenure == '' || item.Tenure == 0) {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter the Tenure' : 'कृपया कार्यकाल दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter the Tenure.' : '<br />कृपया कार्यकाल दर्ज करें।';
                }
                $("#txtTenure_" + index).addClass("notValid");
            }
            else {
                $("#txtTenure_" + index).removeClass("notValid");
            }

            if (item.IntrestRate == '' || item.IntrestRate == 0) {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Interest Rate' : 'कृपया ब्याज दर दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Interest Rate.' : '<br />कृपया ब्याज दर दर्ज करें।';
                }
                $("#txtIntrestRate_" + index).addClass("notValid");
            }
            else {
                $("#txtIntrestRate_" + index).removeClass("notValid");
            }

            if (item.MonthlyInstallment == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Monthly Installment' : 'कृपया मासिक किश्त दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Monthly Installment.' : '<br />कृपया मासिक किश्त दर्ज करें।';
                }
                $("#txtMonthlyInstallment_" + index).addClass("notValid");
            }
            else {
                $("#txtMonthlyInstallment_" + index).removeClass("notValid");
            }

            if (item.Source == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Source' : 'कृपया स्रोत दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Source.' : '<br />कृपया स्रोत दर्ज करें।';
                }
                $("#txtSource_" + index).addClass("notValid");
            }
            else {
                $("#txtSource_" + index).removeClass("notValid");
            }

            if (item.RestAmount == '' || item.RestAmount == 0) {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Rest Amount' : 'कृपया बकाया ऋण दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Rest Amount.' : '<br />कृपया बकाया ऋण दर्ज करें।';
                }
                $("#txtRestAmount_" + index).addClass("notValid");
            }
            else {
                $("#txtRestAmount_" + index).removeClass("notValid");
            }

        });

        if (message.length > 29) {
            ShowOkMessage("निम्नलिखित मान्य करें", message, function () { });
            return false;
        }
        return true;
    }

    /*------- End Pre Loan Details code block--------------*/



    /*------- Start Purpose of Loan code block--------------*/

    $scope.PurposeOfLoan = {
        index: 0,
        Id: "0",
        CapitalAsRequired: false,
        WorkingCapital: false,
        Both: false,
        Period: "",
        IsDelete: false
    }
    $scope.ResetPurposeOfLoan = function () {
        $scope.PurposeOfLoan = {
            index: 0,
            Id: "0",
            CapitalAsRequired: false,
            WorkingCapital: false,
            Both: false,
            Period: "",
            IsDelete: false
        }
    }
    $scope.LoadPurposeOfLoan = [];
    $scope.LoadPurposeOfLoan.push($scope.PurposeOfLoan);

    $scope.AddNewPurposeOfLoan = function () {
        if ($scope.ValidatePurposeOfLoan()) {

            $scope.ResetPurposeOfLoan();

            $scope.LoadPurposeOfLoan.push($scope.PurposeOfLoan);
        }
    }

    $scope.RemovePurposeOfLoanRow = function (index) {
        ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove member from list.' : 'क्या आप वाकई सदस्य को सूची से हटाना चाहते हैं', function () {
            $scope.LoadPurposeOfLoan[index].IsDelete = true;
            $scope.$apply();
        });
    }

    $scope.ValidatePurposeOfLoan = function () {
        var message = "";//$scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblLoanMembersDetail").find('input[type="text"]').removeClass("notValid");
        $("#tblLoanMembersDetail").find('select').removeClass("notValid");


        $scope.LoadPurposeOfLoan.forEach(function (item, index) {

            if (item.Period == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Loan Period' : 'कृपया ऋण अवधि दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Loan Perio.' : '<br />कृपया ऋण अवधि दर्ज करें|';
                }
                $("#txtPeriod").addClass("notValid");
            }
        });

        if (message.length > 29) {
            ShowOkMessage("निम्नलिखित मान्य करें", message, function () { });
            return false;
        }
        return true;
    }

    /*------- End Purpose of Loan code block--------------*/


    /*------- Start Loan Amount Details code block--------------*/

    $scope.LoanAmount = {
        Id: "0",
        PurposeOfLoan: "0",
        EstimatedTotalAmount: "",
        LoanAmountApplied: "",
        IsDelete: false,
        SchemeId: "0",
        SectorId: "0",
        LoanType: "0",
        LoanamountCapital: "0",
        CapitalAsRequired: false,
        WorkingCapital: false,
        Period: "",
        WorkingCapitalFile: ""

    }

    $scope.ResetAmountDetails = function () {
        $scope.LoanAmount = {
            PurposeOfLoan: "0",
            EstimatedTotalAmount: "",
            LoanAmountApplied: "",
            IsDelete: false,
            SchemeId: "0",
            SectorId: "0",
            LoanType: "0",
            LoanamountCapital: "0",
            CapitalAsRequired: false,
            WorkingCapital: false,
            Period: "",
            WorkingCapitalFile: "",
            LoanSectorList: [],
            PurposeOfLoanList: [],
            LoanSchemeList: []
        }
    }
    $scope.LoanAmountDetails = [];
    $scope.LoanAmountDetails.push($scope.LoanAmount);

    $scope.AddLandDetail = function () {
        //if ($scope.ValidateAddMember(1)) {
        if ($scope.ValidateLoanAmount()) {
            $scope.ResetAmountDetails();
            //$scope.LoanAmountDetails = [];

            $scope.LoanAmount.LoanSchemeList = $scope.LoanSchemeList;


            $scope.LoanAmountDetails.push($scope.LoanAmount);
        }
        //}
    }

    $scope.RemoveLoanAmountRow = function (index) {
        ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove amount details from list.' : 'क्या आप वाकई सूची से राशि विवरण निकालना चाहते हैं', function () {
            $scope.LoanAmountDetails[index].IsDelete = true;
            $scope.$apply();
        });
    }

    $scope.ValidateLoanAmount = function () {
        var message = "";//$scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblLoanAmountDetail").find('input[type="text"]').removeClass("notValid");
        $("#tblLoanAmountDetail").find('select').removeClass("notValid");
        $scope.LoanAmountDetails.forEach(function (item, index) {

            if (item.EstimatedTotalAmount == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Estimated Total Amount' : 'कृपया अनुमानित कुल राशि दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? 'Please Enter Estimated Total Amount.' : 'कृपया अनुमानित कुल राशि दर्ज करें|';
                }
                $("#tblLoanAmountDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(0).addClass("notValid");
            }
            if (item.LoanAmountApplied == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Applied Amount' : 'कृपया आवेदित ऋण राशि दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Applied Amount.' : '<br />कृपया आवेदित ऋण राशि दर्ज करें|';
                }
                $("#tblLoanAmountDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(1).addClass("notValid");
            }

            if (item.Period == '' || item.Period == 0) {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Loan Period' : 'कृपया ऋण अवधि दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br /> Please Enter Loan Period' : '<br />कृपया ऋण अवधि दर्ज करें|';
                }
                $("#tblLoanAmountDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(1).addClass("notValid");
            }
        });

        if (message.length > 29) {
            ShowOkMessage("निम्नलिखित मान्य करें", message, function () { });
            return false;
        }
        return true;
    }

    /*------- End Loan Amount Details code block--------------*/



    $scope.MemberRegistration = {
        Id: 0,
        RegistrationNumber: "",
        RegistrationName: "",
        RegistrationDate: "",       
        DateOfEstablishment: "",       
        OwnerName: "",
        OwnerAddress: "",
        BusinessLicense_RegiDescription: "",
        KindOfBusiness: "",
        BusinessItemName: "",
        MonthlyRequirementLivelihood: "",
        ProposedIndustrialActivity: "",
        NumberOfPerson: "",        
        SourceOfDebt: "",
        BailDocument: "",
        Repayment: "",
        RateOfInterest: "",
        CollateralSecurity: "",
        PLDB_ID: "0",
        Branch_ID: "0",
        ApplicantDealAfterApprovalFile: "",
        BusinessAndRegiCertificateFile: "",
        ProposedBuildingAndWorkStation: "",
        Quotation: "",
        IncomeTaxClearanceCertificate: ""
    }

    $scope.IndustryDetails = [];
    $scope.IndustryDetails.push($scope.LoanAmount);




  

    // START : Code for Language label setting
    $scope.getTranslation = function () {
        _language = 'hi-in';
        $scope._language = _language;

        if (_language === 'en-us') {
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

        $scope.BackBtnLast = true;
    };
    // END : Code for Language label setting

    $scope.PLDB = [];
  
   
    //Bank Name List
    $scope.BankList = [];
    $scope.PLDB = [];
    $scope.CategoryList = [];
    $scope.FarmerCategoryList = [];
    $scope.LoanTypeList = [];
    $scope.ProcessStatus = [];
    $scope.LoadMaster = function () {
        return $http.get(SiteUrl + "/pldb/LoadMaster").
            success(function (data, status) {
                $scope.PLDB = JSON.parse(data.DataList).PLDB;
                $scope.CategoryList = JSON.parse(data.DataList).CATEGORY;
                $scope.FarmerCategoryList = JSON.parse(data.DataList).FarmerCategory;
                $scope.LoanTypeList = JSON.parse(data.DataList).LoanTypeList;
                $scope.BankList = JSON.parse(data.DataList).BANKLIST;
                $scope.ProcessStatus = JSON.parse(data.DataList).ProcessStatus;
            }).
            error(function (data, status) {
                $scope.PLDB = [];
            });

    }


    $scope.UserLevel = $("#hdnUserlevel").val();
    $scope.EditAppication = function () {
        $scope.IsSubmiApplication = false;
        $(".clspanal").hide();
        $(".expandDetails").hide();
        $(".clspanal").show();


        $(".clspanal").find("input,select,textarea").each(function (e) {
            if ($(this).attr('ng-disabled') == undefined) {
                $(this).removeAttr("disabled", "disabled");
            }
        });

        //$(".clspanal").find("input").removeAttr("disabled", "disabled");
        //$(".clspanal").find("select").removeAttr("disabled", "disabled");
        //$(".clspanal").find("textarea").removeAttr("disabled", "disabled");
        $(".clspanal").find("tfoot").show();
        $(".clspanal").find("i").show();

        $scope.MemberRegistration.PLDB_ID = $scope.LoanDetails.pldb_id.toString();
        $scope.MemberRegistration.Branch_ID = $scope.LoanDetails.branch_id.toString();

        //$(".clspanal").eq(0).show();
        //$(".clspanal").eq(1).show();
        //$(".expandDetails").show();
        //$(".expandDetails").eq(0).removeClass("fa-plus-circle").addClass("fa-minus-circle");
        //$(".expandDetails").eq(1).removeClass("fa-plus-circle").addClass("fa-minus-circle");



    }




    $scope.PLDBBranchList = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + $scope.MemberRegistration.PLDB_ID).
            success(function (data, status) {
                $scope.PLDBBranches = data;

                $scope.MemberRegistration.Branch_ID = branch_ID.toString();
                $scope.MemberRegistration.Branch_ID = '0';
                $scope.PropertyDetailsList[0].TehsilList = data;
                $scope.PropertyDetailsList[0].Tehsil = '0';;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };

    // START : Get All All List
    $scope.GetMemberDetails = function (aId, index) {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        var isValid = true;
        if ($scope.MemberRegistration.PLDB_ID == '0') {
            message += $scope._language == 'en-us' ? '<br /> Please Select PLDB.' : '<br />कृपया पी.एल.डी.बी का चयन करें।';
            $("#ddlDistrict").addClass("notValid");
            isValid = false;
        }
        if ($scope.MemberRegistration.Branch_ID == '0') {
            message += $scope._language == 'en-us' ? '<br /> Please Select branch.' : '<br />कृपया ब्रांच का चयन करें।';
            $("#ddlblock").addClass("notValid");
            isValid = false;
        }
        var mId = aId;
        mId = mId.replace(/ /g, '');
        aId = aId.replace(/ /g, '');

        if (aId == '' && mId == '') {
            message += $scope._language == 'en-us' ? '<br />Please Enter Aadhaar/Membership Number.' : '<br />कृपया आधार अथवा सदस्यता क्रमांक दर्ज करें।';
            isValid = false;
        }


        if ($scope.LoanMembers.filter(function (x) { return x.MembershipNumber === mId || x.AADHARNo === aId }).length > 1) {
            message += $scope._language == 'en-us' ? '<br /> Membership or AADHARNo Number already added please enter other number.' : '<br />सदस्यता संख्या अथवा आधार पहले से ही जोड़ा गया कृपया अन्य नंबर दर्ज करें।';
            isValid = false;
        }
        if (!isValid) {
            // $scope.ResetMember();
            // $scope.LoanMembers = [];
            //$scope.LoanMembers.push($scope.Member);
            ShowOkMessage("", message, function () { });
        } else if (mId != '' || aId != '') {
            $(".se-pre-con").fadeIn("slow");
            $http.get(SiteUrl + "/pldb/GetMemberDetails?MembershipNo=" + (aId == "" ? mId : aId) + "&DistrictId=" + $scope.MemberRegistration.PLDB_ID + "&BranchId=" + $scope.MemberRegistration.Branch_ID).
                success(function (result, status) {
                    var data = JSON.parse(result.DataList);
                    if (data.length <= 0) {
                        $scope.LoanMembers[index].ApplicantName = "";
                        $scope.LoanMembers[index].FatherName = "";
                        $scope.LoanMembers[index].ApplicantAge = "";
                        $scope.LoanMembers[index].CategoryId = "";
                        $scope.LoanMembers[index].Village = "";
                        $scope.LoanMembers[index].VillagePanchayat = "";
                        $scope.LoanMembers[index].MobileNo = "";
                        $scope.LoanMembers[index].Photo = "";
                        //$scope.LoanMembers[index].DistrictName = data[0].DISTRICT_MANGAL;
                        $scope.LoanMembers[index].TehsilName = "";
                        $scope.LoanMembers[index].VillageName = "";
                        $scope.LoanMembers[index].GramPanchayatName = "";
                        $scope.LoanMembers[index].FarmerCategory = "";
                        $scope.LoanMembers[index].Tehsil = "";
                        $scope.LoanMembers[index].MemberShip = "";
                        $scope.LoanMembers[index].CategoryId = "";
                        $scope.LoanMembers[index].BusinessAddress = "";
                        $scope.LoanMembers[index].Business = "";
                        $scope.LoanMembers[index].IsValidate = false;


                        $scope.LoanMembers[index].MonthlyIncome = "";
                        $scope.LoanMembers[index].Education = "";
                        $scope.LoanMembers[index].FamilyDependents = "";
                        $scope.LoanMembers[index].MonthlyMinimumAmountForLiving = "";
                        $scope.LoanMembers[index].OfficeMobileNo = "";
                        $scope.LoanMembers[index].PartnershipCertifiedCopy = "";
                        $scope.LoanMembers[index].TransportCompanyRegCertification = "";
                        $scope.LoanMembers[index].DrivinglicenseCopy = "";
                        $scope.LoanMembers[index].VehicleRantProposalCopy = "";


                        ShowOkMessage("", "दर्ज किया गया सदस्यता/आधार संख्या उपलब्ध नहीं हैं कृपया पहले सदस्यता के लिए आवेदन करें|", function () {
                            window.location.href = "../SocietyMemberRegistration/PLDBMemberRegistration";
                        });

                    } else {


                        $scope.LoanMembers[index].ApplicantName = data[0].MemberName;
                        $scope.LoanMembers[index].FatherName = data[0].FatherName;
                        $scope.LoanMembers[index].ApplicantAge = data[0].Age + (data[0].Age != '' && data[0].Age > 0 ? ' Year' : '');
                        $scope.LoanMembers[index].CategoryId = $scope._language == 'en-us' ? data[0].CategoryEn : data[0].CategoryHi;
                        $scope.LoanMembers[index].Village = $scope._language == 'en-us' ? data[0].VILLAGE_ENG : data[0].VILLAGE_MANGAL;
                        $scope.LoanMembers[index].VillagePanchayat = $scope._language == 'en-us' ? data[0].GP_ENG : data[0].GP_MANGAL;
                        $scope.LoanMembers[index].MobileNo = data[0].MobileNo;
                        $scope.LoanMembers[index].Business = data[0].Business;
                        $scope.LoanMembers[index].BusinessAddress = data[0].Address;
                        $scope.LoanMembers[index].Photo = data[0].Photo;

                        //$scope.LoanMembers[index].DistrictName = data[0].DISTRICT_MANGAL;
                        $scope.LoanMembers[index].TehsilName = $scope._language == 'en-us' ? data[0].BLOCK_ENG : data[0].BLOCK_MANGAL;
                        $scope.LoanMembers[index].VillageName = $scope._language == 'en-us' ? data[0].VILLAGE_ENG : data[0].VILLAGE_MANGAL;
                        $scope.LoanMembers[index].GramPanchayatName = $scope._language == 'en-us' ? data[0].GP_ENG : data[0].GP_MANGAL;
                        $scope.LoanMembers[index].FarmerCategory = $scope._language == 'en-us' ? data[0].FarmerCategoryEn : data[0].FarmerCategoryHi;
                        $scope.LoanMembers[index].Tehsil = $scope._language == 'en-us' ? data[0].BLOCK_ENG : data[0].BLOCK_MANGAL;
                        $scope.LoanMembers[index].MemberShip = data[0].MembershipNo;

                        $scope.LoanMembers[index].MembershipNumber = data[0].MembershipNo;
                        $scope.LoanMembers[index].AADHARNo = data[0].AADHARNo;
                        //$scope.LoanMembers[index].CategoryId = data[0].CategoryId.toString();
                        $scope.ShowApplicanDetails = true;
                        $scope.LoanMembers[index].IsShow = true;
                        $scope.LoanMembers[index].IsValidate = true;

                        // $scope.GetVillages(index, data[0].VillageId);
                    }

                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    //alert(data);
                    $scope.MemberDetailsList = [];
                    $(".se-pre-con").fadeOut("slow");
                });
        }
    };
    // START : Get All All List

    // START : Get All All List
    $scope.UpdateMemberDetails = function (item) {
        $scope.MemberRegistration.OrganizationId = item.OrganizationId;
        $scope.MemberRegistration.Name = item.Name;
        $scope.MemberRegistration.Age = item.Age;
        $scope.MemberRegistration.Address = item.Address;
        $scope.MemberRegistration.Gender = item.Gender;
        $scope.MemberRegistration.AADHARNo = item.AADHARNo;
        $scope.MemberRegistration.Business = item.Business;
        $scope.MemberRegistration.MobileNo = item.MobileNo;
        $scope.MemberRegistration.EmailId = item.EmailId;
        $scope.MemberRegistration.AnimalCount = item.AnimalCount;
        $scope.MemberRegistration.BuyCount = item.BuyCount;
        $scope.MemberRegistration.DepositedAmount = item.DepositedAmount;
        $scope.MemberRegistration.IsDisability = item.IsDisability === 'True' ? true : false;
        $scope.MemberRegistration.OrganizationId = item.OrganizationId;
        $scope.MemberRegistration.UserId = item.UserId;

        $scope.divUserRegistration = true;
    };
    // START : Get All All List


    function checkDateFormat(DateOfOpenAccount) {
        // regular expression to match required date format
        re = /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/;
        var regs = DateOfOpenAccount.match(re);
        if (DateOfOpenAccount != '') {
            if (regs = DateOfOpenAccount.match(re)) {
                // day value between 1 and 31
                if (regs[1] < 1 || regs[1] > 31) {               
                    return false;
                }
                // month value between 1 and 12
                if (regs[2] < 1 || regs[2] > 12) {
                    return false;
                }
                // year value between 1902 and 2020
                if (regs[3] < 1902 || regs[3] > (new Date()).getFullYear()) {
                    return false;
                }
            } else {
                return false;
            }
        }
        return true;
    }

    
    // START : Save User
    $scope.SaveUser = function (IsSubmit) {
        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();

        $("#dvMemberPhoto").find('input[type="file"]').each(function (indx) {
            //alert($(this)[0].files.length);
            if ($(this).val() != "") {
                var fileUpload = $(this).get(0);
                var files = fileUpload.files;
                formData.append("MemberPhoto_" + indx, files[0]);
            }
        });


        var members = [];
        $scope.LoanMembers.forEach(function (itm) {
            if (!itm.IsDelete) {
                members.push({
                    Id: itm.Id == undefined ? 0 : itm.Id,
                    MembershipNumber: itm.MembershipNumber,
                    Name: itm.ApplicantName,                
                    Educated_Experienced: itm.Educated_Experienced,                  
                    Source: itm.Source,
                    MonthlyIncome: itm.MonthlyIncome,
                    Cast: itm.Cast,
                    IsDelete: itm.IsDelete
                });
            }
        });

        var LoanProperty = [];
        $scope.PropertyDetailsList.forEach(function (itm) {
            if (!itm.IsDelete) {
                LoanProperty.push({
                    Id: itm.Id,
                    Village: itm.Village,
                    Tehsil: itm.Tehsil,
                    GramPanchayat: itm.GramPanchayat,
                    Khasara: itm.Khasara,
                    ChahiBi: itm.ChahiBi,
                    ChahiVi: itm.ChahiVi,
                    BaraniBi: itm.BaraniBi,
                    BaraniVi: itm.BaraniVi,
                    OtherDetails: itm.OtherDetails,
                    Specifications: itm.Specifications,
                    IsDelete: itm.IsDelete,
                    Address: itm.Address,
                    TotalArea: itm.TotalArea,
                    MemberType: 1
                });
            }
        });

        var LoanAmountDetails = [];
        $scope.LoanAmountDetails.forEach(function (itm) {
            LoanAmountDetails.push({
                Id: itm.Id,
                PurposeOfLoan: itm.PurposeOfLoan,
                EstimatedTotalAmount: itm.EstimatedTotalAmount,
                LoanAmountApplied: itm.LoanAmountApplied,
                SchemeId: itm.SchemeId,
                SectorId: itm.SectorId,
                LoanType: itm.LoanType,
                LoanamountCapital: itm.WorkingCapital && itm.CapitalAsRequired ? 3 : itm.WorkingCapital ? 2 : itm.CapitalAsRequired ? 1 : 0,
                Period: itm.Period,
                IsDelete: itm.IsDelete
            });
        });

        var LoadPreLoanDetails = [];
        $scope.LoadPreLoanDetails.forEach(function (itm) {
            if (!itm.IsDelete) {
                LoadPreLoanDetails.push({
                    Id: itm.Id == undefined ? 0 : itm.Id,
                    Organization: itm.Organization,
                    LoanAmount: itm.PreLoanAmount,
                    RestAmount: itm.RestAmount,
                    MonthlyInstallment: itm.MonthlyInstallment,
                    Tenure: itm.Tenure,
                    IntrestRate: itm.IntrestRate,
                    Source: itm.Source,

                    IsDelete: itm.IsDelete
                });
            }
        });
        
        //var DateOfOpenAccount = $("#DateOfOpenAccount").val();
        //checkDateFormat(itm.DateOfOpenAccount);

       
        var LoadBankDetails = [];
        $scope.LoadBankDetails.forEach(function (itm) {
            var DateOfOpenAccount = $("#DateOfOpenAccount").val();       

            if (itm.DateOfOpenAccount != "") {
                checkDateFormat(itm.DateOfOpenAccount)
            }
            if (!itm.IsDelete) {
                LoadBankDetails.push({
                    Id: itm.Id,
                    BankName: itm.BankId,
                    AccountType: itm.AccountType,
                    DateOfOpenAccount: itm.DateOfOpenAccount,
                    LoanFacility: itm.LoanFacility,
                    IsDelete: itm.IsDelete
                });
            }
        });

        $scope.guarantor = [];
        $scope.LoanGuarantor.forEach(function (itm) {
            $scope.guarantor.push({
                Guarantor_Id: itm.Id,
                AadhaarNumber: itm.AADHARNo,
                Name: itm.Name,
                FatherName: itm.FatherName,
                DOB: itm.DOB,
                Mobile: itm.MobileNo,
                Address: itm.Address,
                Category: itm.CategoryId
            });
        });

        var GuarantorProperty = [];
        $scope.GuarantorPropertyDetailsList.forEach(function (itm) {
            if (!itm.IsDelete) {
                LoanProperty.push({
                    Id: itm.Id,
                    Village: itm.Village,
                    Tehsil: itm.Tehsil,
                    GramPanchayat: itm.GramPanchayat,
                    Khasara: itm.Khasara,
                    ChahiBi: itm.ChahiBi,
                    ChahiVi: itm.ChahiVi,
                    BaraniBi: itm.BaraniBi,
                    BaraniVi: itm.BaraniVi,
                    OtherDetails: itm.OtherDetails,
                    Specifications: itm.Specifications,
                    IsDelete: itm.IsDelete,
                    Address: itm.Address,
                    TotalArea: itm.TotalArea,
                    MemberType: 2
                });
            }
        });

        var PLDBLoanSmall_IndustryList = [];
        var RegistrationDate = $("#RegistrationDate").val();
        var DateOfEstablishment = $("#DateOfEstablishment").val();

        var applicaionDetails = {
            load_id: $scope.LoanApplicationId,
            pldb_id: $scope.MemberRegistration.PLDB_ID,
            branch_id: $scope.MemberRegistration.Branch_ID,

            RegistrationNumber: $scope.MemberRegistration.RegistrationNumber,
            RegistrationName: $scope.MemberRegistration.RegistrationName,
            RegistrationDate: RegistrationDate,
            DateOfEstablishment: DateOfEstablishment,
            OwnerName: $scope.MemberRegistration.OwnerName,
            Address: $scope.MemberRegistration.OwnerAddress,
            BusinessLicense_RegiDescription: $scope.MemberRegistration.BusinessLicense_RegiDescription,
            KindOfBusiness: $scope.MemberRegistration.KindOfBusiness,
            BusinessItemName: $scope.MemberRegistration.BusinessItemName,
            MonthlyRequirementLivelihood: $scope.MemberRegistration.MonthlyRequirementLivelihood,
            ProposedIndustrialActivity: $scope.MemberRegistration.ProposedIndustrialActivity,
            NumberOfPerson: $scope.MemberRegistration.NumberOfPerson,
            SourceOfDebt: $scope.MemberRegistration.SourceOfDebt,
            BailDocument: $scope.MemberRegistration.BailDocument,
            Repayment: $scope.MemberRegistration.Repayment,
            RateOfInterest: $scope.MemberRegistration.RateOfInterest,
            CollateralSecurity: $scope.MemberRegistration.CollateralSecurity,
            RicoFile: $scope.LoanDocument.RicoFile,
            WorkingCapitalFile: $scope.LoanDocument.WorkingCapitalFile,
            ApplicantDealAfterApprovalFile: $scope.LoanDocument.ApplicantDealAfterApprovalFile,
            BusinessAndRegiCertificateFile: $scope.LoanDocument.BusinessAndRegiCertificateFile,
            ProposedBuildingAndWorkStationFile: $scope.LoanDocument.ProposedBuildingAndWorkStation,
            QuotationFile: $scope.LoanDocument.Quotation,
            IncomeTaxClearanceCertificate: $scope.LoanDocument.IncomeTaxClearanceCertificate,

            IndustryDetails: PLDBLoanSmall_IndustryList,
            Members: members,
            LoanAmount: LoanAmountDetails,
            PreLoan: LoadPreLoanDetails,
            LoanProperty: LoanProperty,
            BankDetails: LoadBankDetails,
            GuarantorDetails: $scope.guarantor,
            IsSubmit: IsSubmit
        };

        formData.append("Applicationdata", JSON.stringify(applicaionDetails));
        formData.append("LoanProperty", JSON.stringify($scope.PropertyDetailsList));
        formData.append("GuarantorDetails", JSON.stringify($scope.guarantor));
        formData.append("GuarantorProperty", JSON.stringify($scope.GuarantorProperty));

        $http.post(SiteUrl + "/pldb/SaveLoanApplication", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.Result != "-1") {
                    ShowOkMessage("Success", $scope._language == 'en-us' ? "Loan application has been submitted successfully.<br/><br/>Application No.:-<b>" + data.Result + "</b>" : "ऋण आवेदन सफलतापूर्वक जमा कर लिया गया है|<br/><br/>आवेदन संख्या:-<b>" + data.Result + "</b>", function () {
                        location.href = "../pldb/LoanApplicationlist";
                        //location.href = "../pldb/PLDBSchemeList";
                    });
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

    $scope.SaveLoanApplication = function (IsSubmit) {
        if ($scope.ValidateLoanAmount()) {
            if ($scope.ValidateAddMember(1)) {
                if ($scope.ValidateDescriptionOfIndustry(1)) {
                    if ($scope.ValidateLandDetails()) {
                      // // //if ($scope.ValidateAddPreLoanDetails()) {
                        if ($scope.ValidateAddGuarantor()) {
                            if ($scope.ValidateGuarantorProperty()) {
                          //  //   //if ($scope.ValidateBankDetails()) {
                         //    //   //if ($scope.ValidatetblPropertyDetails()) {
                                if (IsSubmit) {
                                    ShowAlertMessage("Alert", _language === 'en-us' ? "Are you sure you want to submit this loan application." : "क्या आप वाकई यह ऋण आवेदन जमा करना चाहते हैं?", function () {
                                        $scope.SaveUser(IsSubmit);
                                    });
                                } else {
                                    $scope.SaveUser(IsSubmit);
                                }
                          //  //    //  }
                            }
                        }
                    }
                }
            }
        }
      //  //  //}
    }

    // END : Save User


    $scope.SowApplicationGrid = function () {
        window.location = '../pldb/PLDBSchemeList';
    }

    $scope.GetMemberList = function (loanId, branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $scope.GetGramPanchayatData().then(function () {
            return $http.post(SiteUrl + "/pldb/GetLoanApplicationDetails", { Table: "PLDBLoanMembers", LoanId: loanId }).
                success(function (data, status) {
                    $scope.MemberRegistration.Branch_ID = branch_ID.toString();
                    var List = JSON.parse(data.DataList);
                    //alert(List.length);

                    if (List.length > 0) {
                        $scope.LoanMembers = [];
                        List.forEach(function (itm, index) {
                            $scope.LoanMembers.push({
                                Id: itm.loan_memer_id,
                                MembershipNumber: itm.AADHARNo,
                                Name: itm.MemberName,
                                FatherName: itm.FatherName,
                                Age: itm.Age,
                                FarmerCategory: $scope._language == 'en-us' ? itm.FarmerCategoryEn : itm.FarmerCategoryHi,
                                CategoryId: $scope._language == 'en-us' ? itm.CategoryEn : itm.CategoryHi,
                                Village: $scope._language == 'en-us' ? itm.VILLAGE_ENG : itm.VILLAGE_MANGAL,
                                VillagePanchayat: $scope._language == 'en-us' ? itm.GP_ENG : itm.GP_MANGAL,
                                Tehsil: $scope._language == 'en-us' ? itm.BLOCK_ENG : itm.BLOCK_MANGAL,
                                MobileNo: itm.MobileNo,
                                Photo: itm.Photo,
                                MemberShip: itm.MembershipNo,
                                //GramPanchayats: $scope.GramPanchayats,
                                IsDelete: itm.IsDelete,
                                IsMainMember: itm.IsMainMember
                            });
                            //$scope.GetVillages(index, itm.VILLAGE_ID);      $scope.LoanMembers[index].MemberShip = data[0].MembershipNo;
                        });
                    }
                    //$(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $scope.LoanMembers = [];
                    // $(".se-pre-con").fadeOut("slow");
                });
        });

    }

    $scope.GetAmountList = function (loanId) {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetLoanApplicationDetails", { Table: "PLDBLoanAmount", LoanId: loanId }).
            success(function (data, status) {
                var List = JSON.parse(data.DataList);
                // alert(List.length);
                if (List.length > 0) {
                    $scope.LoanAmountDetails = [];
                    List.forEach(function (itm, index) {



                        $scope.LoanAmountDetails.push({
                            Id: itm.loan_amount_id,
                            PurposeOfLoan: itm.loan_purpose_id,
                            EstimatedTotalAmount: itm.estimated_total_amount,
                            LoanAmountApplied: itm.loan_amount_applied,
                            IsDelete: itm.IsDelete,
                            SchemeId: itm.scheme_id,
                            SectorId: itm.sector_id,
                            LoanType: itm.loan_type,
                            LoanSchemeList: $scope.LoanSchemeList
                        });

                        $scope.LoadSector(itm.scheme_id, index).then(function () {

                            $scope.LoadPurpose(itm.sector_id, index).then(function () {

                                //$scope.LoanAmountDetails[index].LoanSectorList=

                                $scope.LoanAmountDetails[index].PurposeOfLoan = itm.loan_purpose_id.toString();
                                $scope.LoanAmountDetails[index].SchemeId = itm.scheme_id.toString();
                                $scope.LoanAmountDetails[index].LoanType = itm.loan_type.toString();
                                $scope.LoanAmountDetails[index].SectorId = itm.sector_id.toString();
                            });
                        });

                    });
                }
            }).
            error(function (data, status) {
                $scope.LoanAmountDetails = [];
            });
    }
    $scope.GetPropertyList = function (loanId) {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetLoanApplicationDetails", { Table: "PLDBLoanProperty", LoanId: loanId }).
            success(function (data, status) {
                var List = JSON.parse(data.DataList);
                //alert(List.length);
                if (List.length > 0) {
                    $scope.PropertyDetailsList = [];
                    List.forEach(function (itm, index) {
                        $scope.PropertyDetailsList.push({
                            Id: itm.loan_property_id,
                            Village: itm.VILLAGE_ID.toString(),
                            Tehsil: itm.BLOCK_ID.toString(),
                            GramPanchayat: itm.GP_ID.toString(),
                            Khasara: itm.khasara,
                            ChahiBi: itm.area_chahi_bi,
                            ChahiVi: itm.area_chahi_vi,
                            BaraniBi: itm.area_barani_bi,
                            BaraniVi: itm.area_barani_vi,
                            OtherDetails: itm.other_property_details,
                            Specifications: itm.specifications,
                            TehsilList: $scope.PLDBBranches,
                            IsDelete: itm.IsDelete
                        });
                       // $scope.GetPropertyDetailsPanchayt(index);
                        //$scope.GetPropertyDetailsVillages(index);
                    });
                }
            }).
            error(function (data, status) {
                $scope.PropertyDetailsList = [];
            });
    }
    $scope.GetPreLoanList = function (loanId) {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetLoanApplicationDetails", { Table: "PLDPLoanMemberPreLoan", LoanId: loanId }).
            success(function (data, status) {
                var List = JSON.parse(data.DataList);
                //alert(List.length);
                if (List.length > 0) {
                    $scope.PreLoanDetailsList = [];
                    List.forEach(function (itm, index) {
                        $scope.PreLoanDetailsList.push({
                            Id: itm.loan_pre_loan_id,
                            Organization: itm.organization,
                            LoanAmount: itm.loan_amount,
                            MonthlyInstallment: itm.monthly_installment,
                            IsDelete: itm.IsDelete
                        });

                    });
                }
            }).
            error(function (data, status) {
                $scope.PreLoanDetailsList = [];
            });
    }


    // START : Change Status
    $scope.SetStatus = function (UserId, IsActive) {
        IsActive = (IsActive === 'True' ? false : true);
        $http.post(SiteUrl + "/SocietyMemberRegistration/ChangeOrganizationUserStatus", { "RequestId": UserId, "IsActive": IsActive }).
            success(function (data, status) {
                alert(data);
            }).
            error(function (data, status) {
                $scope.MemberDetailsList = [];
            });
    };
    // START : Change Status
    
 

   

    




    // Start : Validations  
    $scope.PLDBLoanSmall_IndustryList = [];
    $scope.PLDBLoanSmall_IndustryList.push($scope.MemberRegistration);
    $scope.ValidateDescriptionOfIndustry = function (IsSubmit) {

        var message = "";
        $("#tblLoanMembersDetail").find('input[type="text"]').removeClass("notValid");
        $("#tblLoanMembersDetail").find('select').removeClass("notValid");

        if ($scope.MemberRegistration.PLDB_ID == '0') {
            message += $scope._language == 'en-us' ? '<br /> Please Select PLDB.' : '<br />कृपया पी.एल.डी.बी का चयन करें।';
            $("#ddlDistrict").addClass("notValid");
        }
        if ($scope.MemberRegistration.Branch_ID == '0') {
            message += $scope._language == 'en-us' ? '<br /> Please Select branch.' : '<br />कृपया ब्रांच का चयन करें।';
            $("#ddlblock").addClass("notValid");
        }

        $scope.PLDBLoanSmall_IndustryList.forEach(function (item, index) {

            //alert($("#tblLandMemberDetail").find("tr").eq(index+1).find('input[type="text"]').length);


            if ($scope.MemberRegistration.RegistrationNumber == '' || $scope.MemberRegistration.RegistrationNumber == 0) {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Registration Number' : 'कृपया पंजीकरण संख्या दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Registration Number.' : '<br />कृपया पंजीकरण संख्या दर्ज करें।';
                }
                $("#RegistrationNumber").addClass("notValid");
            } else {
                $("#RegistrationNumber").removeClass("notValid");
            }


            if ($scope.MemberRegistration.Isregistration == '' || $scope.MemberRegistration.Isregistration == undefined) {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Select Small Industry Registration' : 'कृपया लघु उद्योग पंजीयन की स्थिति चुनें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Select Small Industry Registration.' : '<br />कृपया लघु उद्योग पंजीयन की स्थिति चुनें।';
                }
            }
            else if ($scope.MemberRegistration.Isregistration == '' || $scope.MemberRegistration.Isregistration == 'Yes') {
                if ($scope.MemberRegistration.Isregistration == 'Yes') {
                    if ($scope.MemberRegistration.RegistrationName == '') {
                        if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Registration Name' : 'कृपया पंजीकरण नाम दर्ज करें') < 0) {
                            message += $scope._language == 'en-us' ? '<br />Please Enter Registration Name.' : '<br />कृपया पंजीकरण नाम दर्ज करें।';
                        }
                        $("#RegistrationName").addClass("notValid");
                    }
                    else {
                        $("#RegistrationName").removeClass("notValid");
                    }

                    //if ($scope.MemberRegistration.RegistrationDate == '') {
                    //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Registration Date' : 'कृपया पंजीकरण तिथि चुनें') < 0) {
                    //        message += $scope._language == 'en-us' ? '<br />Please Select Registration Date.' : '<br />कृपया पंजीकरण तिथि चुनें।';
                    //    }
                    //    $("#RegistrationDate").addClass("notValid");
                    //}
                }
                else if (message.indexOf($scope._language == 'en-us' ? 'Please Select Small Industry Registration' : 'कृपया लघु उद्योग का पंजीकरण चुनें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Select Small Industry Registration.' : '<br />कृपया लघु उद्योग का पंजीकरण चुनें।';
                }
            }



            if ($scope.MemberRegistration.IsDateOfEstablishment == '' || $scope.MemberRegistration.IsDateOfEstablishment == undefined) {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Select Date of Establishment' : 'कृपया स्थापना की तिथि चुनें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Select Date of Establishment.' : '<br />कृपया स्थापना की तिथि चुनें।';
                }
            }
            //else if ($scope.MemberRegistration.IsDateOfEstablishment == 'Yes' && $scope.MemberRegistration.DateOfEstablishment == '') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Date of Establishment' : 'कृपया स्थापना की तिथि चुनें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Select Date of Establishment.' : '<br />कृपया स्थापना की तिथि चुनें।';
            //    }
            //    $("#DateOfEstablishment").addClass("notValid");
            //}


            if ($scope.MemberRegistration.AddressType == '' || $scope.MemberRegistration.AddressType == undefined) {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया पता चुनें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Select Address Type.' : '<br />कृपया पता चुनें।';
                }
            }
            if ($scope.MemberRegistration.AddressType == 'Rental') {

                if ($scope.MemberRegistration.OwnerName == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Owner Name' : 'कृपया मालिक का नाम दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please Enter Owner Name.' : '<br />कृपया मालिक का नाम दर्ज करें।';
                    }
                    $("#txtOwnerName").addClass("notValid");
                }
                else {
                    $("#txtOwnerName").removeClass("notValid");
                }

                if ($scope.MemberRegistration.OwnerAddress == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Owner Address' : 'कृपया मालिक का पता दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please Enter Owner Address.' : '<br />कृपया मालिक का पता दर्ज करें।';
                    }
                    $("#txtOwnerAddress").addClass("notValid");
                }
                else {
                    $("#txtOwnerAddress").removeClass("notValid");
                }

            }

            if ($scope.MemberRegistration.BusinessLicense_RegiDescription == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please enter business license registration Description.' : 'कृपया व्यवसाय लाइसेंस पंजीकरण का  विवरण दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please enter business license registration Description.' : '<br />कृपया व्यवसाय लाइसेंस पंजीकरण का  विवरण दर्ज करें।';
                }
                $("#txtBusinessLicense_RegiDescription").addClass("notValid");
            }
            else {
                $("#txtBusinessLicense_RegiDescription").removeClass("notValid");
            }

            if ($scope.MemberRegistration.KindOfBusiness == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Kind Of Business' : 'कृपया व्यवसाय का प्रकार दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Kind Of Business.' : '<br />कृपया व्यवसाय का प्रकार दर्ज करें।';
                }
                $("#txtkindOfBusiness").addClass("notValid");
            }
            else {
                $("#txtkindOfBusiness").removeClass("notValid");
            }

            if ($scope.MemberRegistration.BusinessItemName == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please enter Business Item Name' : 'कृपया व्यावसायिक विषय का नाम दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please enter Business Item Name.' : '<br />कृपया व्यावसायिक विषय का नाम दर्ज करें ।';
                }
                $("#txtbusinessItemName").addClass("notValid");
            }
            else {
                $("#txtbusinessItemName").removeClass("notValid");
            }


            if ($scope.MemberRegistration.MonthlyRequirementLivelihood == '' || $scope.MemberRegistration.MonthlyRequirementLivelihood == 0) {
                if (message.indexOf($scope._language == 'en-us' ? 'Please enter Monthly Requirement of Livelihood' : 'कृपया आजीविका की मासिक आवश्यकता दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please enter Monthly Requirement of Livelihood.' : '<br />कृपया आजीविका की मासिक आवश्यकता दर्ज करें।';
                }
                $("#txtMonthlyRequirementLivelihood").addClass("notValid");
            }
            else {
                $("#txtMonthlyRequirementLivelihood").removeClass("notValid");
            }


            if ($scope.MemberRegistration.ProposedIndustrialActivity == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please enter Proposed of Industrial Activity' : 'कृपया औद्योगिक गतिविधि का प्रस्ताव दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please enter Proposed of Industrial Activity.' : '<br />कृपया औद्योगिक गतिविधि का प्रस्ताव दर्ज करें।';
                }
                $("#txtproposedIndustrialActivity").addClass("notValid");
            }
            else {
                $("#txtproposedIndustrialActivity").removeClass("notValid");
            }


            if ($scope.MemberRegistration.NumberOfPerson == '' || $scope.MemberRegistration.NumberOfPerson == 0) {
                if (message.indexOf($scope._language == 'en-us' ? 'Please enter Number of Person' : 'कृपया व्यक्तियों की संख्या दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please enter Number of Person.' : '<br />कृपया व्यक्तियों की संख्या दर्ज करें।';
                }
                $("#txtnumberOfUnits").addClass("notValid");
            }
            else {
                $("#txtnumberOfUnits").removeClass("notValid");
            }


            if ($scope.MemberRegistration.FundingSoFar == 'Thence') {

                if ($scope.MemberRegistration.SourceOfDebt == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter Source of Debt' : 'कृपया ऋण का स्रोत दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please enter Source of Debt.' : '<br />कृपया ऋण का स्रोत दर्ज करें।';
                    }
                    $("#txtSourceOfDebt").addClass("notValid");
                }
                else {
                    $("#txtSourceOfDebt").removeClass("notValid");
                }


                if ($scope.MemberRegistration.BailDocument == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter Bail Document' : 'कृपया जमानत के दस्तावेज दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please enter Bail Document.' : '<br />कृपया जमानत के दस्तावेज दर्ज करें।';
                    }
                    $("#txtBailDocument").addClass("notValid");
                }
                else {
                    $("#txtBailDocument").removeClass("notValid");
                }


                if ($scope.MemberRegistration.Repayment == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter Repayment' : 'कृपया पुनः भुगतान दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please enter Repayment.' : '<br />कृपया पुनः भुगतान दर्ज करें।';
                    }
                    $("#txtRepayment").addClass("notValid");
                }
                else {
                    $("#txtRepayment").removeClass("notValid");
                }


                if ($scope.MemberRegistration.RateOfInterest == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter Rate of Interest' : 'कृपया ब्याज की दर दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please enter Rate of Interest.' : '<br />कृपया ब्याज की दर दर्ज करें।';
                    }
                    $("#txtRateOfInterest").addClass("notValid");
                }
                else {
                    $("#txtRateOfInterest").removeClass("notValid");
                }

            }



            if ($scope.MemberRegistration.IsCollateralSecurity == 'Yes') {

                if ($scope.MemberRegistration.CollateralSecurity == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter Collateral Security' : 'कृपया जमानत की सुरक्षा दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please enter Collateral Security.' : '<br />कृपया जमानत की सुरक्षा दर्ज करें।';
                    }
                    $("#txtCollateralSecurity").addClass("notValid");
                }
                else {
                    $("#txtCollateralSecurity").removeClass("notValid");
                }
            }

            ////File upload Validation

            $scope.Application = {
                IsRicoFile: false,
                IsApplicantHasApplicantDealAfterApproval: false,
                IsWorkingCapitalFile: false,
                IsProposedBuildingAndWorkStation: false,
                IsBusinessAndRegiCertificateFile: false,
                IsIncomeTaxClearanceCertificate: false,
                IsQuotation: false
            }

            if ($scope.MemberRegistration.AddressType == 'IsRico' && ($scope.LoanDocument.RicoFile == '' || $scope.LoanDocument.RicoFile == null)) {
                if (($scope.RicoFile == undefined || $scope.RicoFile == '') && $scope.RicoFile != "1") {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Upload Rico File' : '\n <br /> कृपया रिको फ़ाइल अपलोड करें |';
                }
            }
            if ($scope.LoanAmountDetails[0].WorkingCapital == true) {
                if ($scope.LoanDocument.WorkingCapitalFile == '') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Upload Working Capital File' : '\n <br /> कृपया कार्यशील पूंजी की फ़ाइल अपलोड करें|';
                }
            }
            //if (($scope.ApplicantDealAfterApprovalFile == undefined || $scope.ApplicantDealAfterApprovalFile == '') && $scope.ApplicantDealAfterApprovalFile != "1") {
            //    message += $scope._language == 'en-us' ? '\n <br /> Please Upload Applicant Deal After Approval File' : '\n <br /> कृपया अनुमोदन के बाद आवेदक डील फ़ाइल अपलोड करें |';
            //}
            //if (($scope.BusinessAndRegiCertificateFile == undefined || $scope.BusinessAndRegiCertificateFile == '') && $scope.BusinessAndRegiCertificateFile != "1") {
            //    message += $scope._language == 'en-us' ? '\n <br />  Please Upload Business and Registration Certificate File' : '\n <br /> कृपया व्यवसाय और पंजीकरण प्रमाणपत्र फ़ाइल अपलोड करें |';
            //}

            //if (($scope.RicoFile == undefined || $scope.RicoFile == '') && $scope.RicoFile != "1") {
            //    message += $scope._language == 'en-us' ? '\n <br /> Please Upload Proposed Building and Workstation' : '\n <br /> कृपया प्रस्तावित बिल्डिंग और वर्कस्टेशन अपलोड करें |';
            //}



            if ($scope.LoanDocument.ApplicantDealAfterApprovalFile == '' || $scope.LoanDocument.ApplicantDealAfterApprovalFile == null) {
                message += $scope._language == 'en-us' ? '<br />  Please Upload Applicant Deal After Approval File.' : '<br />कृपया अनुमोदन के बाद आवेदक डील फ़ाइल अपलोड करें।';
            }

            if ($scope.LoanDocument.BusinessAndRegiCertificateFile == '' || $scope.LoanDocument.BusinessAndRegiCertificateFile == null) {
                message += $scope._language == 'en-us' ? '<br /> Please Upload Business and Registration Certificate File.' : '<br />कृपया व्यवसाय और पंजीकरण प्रमाणपत्र फ़ाइल अपलोड करें।';
            }

            if ($scope.LoanDocument.ProposedBuildingAndWorkStation == '' || $scope.LoanDocument.ProposedBuildingAndWorkStation == null) {
                message += $scope._language == 'en-us' ? '<br /> Please Upload Proposed Building and Work Station.' : '<br />कृपया प्रस्तावित बिल्डिंग और वर्क स्टेशन अपलोड करें।';
            }

            if ($scope.LoanDocument.Quotation == '' || $scope.LoanDocument.Quotation == null) {
                message += $scope._language == 'en-us' ? '<br /> Please upload the proposed plot, machine, equipment, tools etc. (original Quotation of the firm).' : '<br />कृपया प्रस्तावित प्लॉट, मशीन, उपकरण, औजार आदि (फर्म का मूल कॉटेशन) अपलोड करें।';
            }

            if ($scope.LoanDocument.IncomeTaxClearanceCertificate == '' || $scope.LoanDocument.IncomeTaxClearanceCertificate == '') {
                message += $scope._language == 'en-us' ? '<br /> Please upload Income Tax Clearance Certificate.' : '<br />कृपया आयकर क्लीयरेंस प्रमाणपत्र फ़ाइल अपलोड करें।';
            }



            //if ($scope.item.AADHARNo == '') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Guarantor Aadhar Number' : 'कृपया गारंटर का आधार नंबर दर्ज करें') < 0) {
            //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Guarantor Aadhar Number' : '\n <br /> कृपया गारंटर का आधार नंबर दर्ज करें|';
            //    }
            //    $("#AADHARNo").addClass("notValid");
            //}

        });

        if (message.length > 29) {
            ShowOkMessage("निम्नलिखित मान्य करें", message, function () { });
            return false;
        }
        return true;
    }
    //End Validation



    $scope.getTranslation();
    $scope.LoanMembers = [];
    $scope.LoanMembers.push($scope.Member);
   

    $scope.ShowMemberDetail = function () {
        //if ($scope.MemberRegistration.RegistrationType == '1') {
        //    $scope.MemberDatail = true;
        //    $scope.DairyDatail = false;
        //}
        //else {
        //    $scope.MemberDatail = false;
        //    $scope.DairyDatail = true;
        //}

        //$scope.divParentDataSelection = false;
        //$scope.divViewShow = false;
        //$scope.divGrid = false;
        //$scope.divRegistration = true;
        $scope.BackBtnLast = false;
        $scope.GetVillageList();
    };

    $scope.ShowForm = function () {
        $scope.divParentDataSelection = true;
        $scope.DairyDatail = false;
        $scope.MemberDatail = false;
        $scope.divViewDairyFormShow = false;
        $scope.divViewShow = false;
        $scope.divRegistration = true;
        $scope.divGrid = false;
        $scope.ChangeType();
    };

    $scope.ShowSelfDeclarationLetter = function () {
        if ($scope.ValidateAddMember(1)) {
            if ($scope.ValidateLoanAmount()) {
                if ($scope.ValidatetblPropertyDetails()) {
                    $scope.LoanApplicationLetter = false;
                    $scope.SelfDeclarationLetter = true;
                    $scope.divParentDataSelection = false;
                }
            }
        }
    }

    $scope.ShowLoanApplication = function () {
        if ($scope.IsSubmiApplication) {
            window.location.href = "../pldb/LoanApplicationList";
        }
        else {
            window.location.href = "../pldb/PLDBSchemeList";
        }
        //if ($scope.ValidateAddMember(1)) {
        //    if ($scope.ValidateLoanAmount()) {
        //        if ($scope.ValidatetblPropertyDetails()) {
        //            $scope.LoanApplicationLetter = true;
        //            $scope.SelfDeclarationLetter = false;
        //            $scope.divParentDataSelection = true;
        //        }
        //    }
        //}
    }

    $scope.GetNameFromList = function (list, id, type) {
        try {
            if (type == 'Village') {
                var data = list.filter(function (x) { return x.VILLAGE_ID === id.toString() })[0];
                return _language == 'en-us' ? data.VILLAGE_ENG : data.VILLAGE_MANGAL;
            } else if (type == 'Tehsil') {
                var data = list.filter(function (x) { return x.BLOCK_ID === id.toString() })[0];
                return _language == 'en-us' ? data.BLOCK_ENG : data.BLOCK_MANGAL;
            } else if (type == 'District') {
                var data = list.filter(function (x) { return x.PldbId === id.toString() })[0];
                return _language == 'en-us' ? data.PLDBName : data.PLDBName;
            } else if (type == 'Village') {
                var data = list.filter(function (x) { return x.VILLAGE_ID === id.toString() })[0];
                return _language == 'en-us' ? data.VILLAGE_ENG : data.VILLAGE_MANGAL;
            } else if (type == 'Village') {
                var data = list.filter(function (x) { return x.VILLAGE_ID === id.toString() })[0];
                return _language == 'en-us' ? data.VILLAGE_ENG : data.VILLAGE_MANGAL;
            } else if (type == 'Branch') {
                var data = list.filter(function (x) { return x.BLOCK_ID === id.toString() })[0];
                return _language == 'en-us' ? data.BLOCK_ENG : data.BLOCK_MANGAL;
            }
        } catch (ex) { return ""; }
    }

    $scope.ChooseDefaultMember = function (index) {
        $scope.LoanMembers.forEach(function (item, indx) {
            if (index != indx) {
                item.IsMainMember = false;
            }
        });
    }

    $scope.OpenOtpPopUp = function (AADHAARNo, index) {
        AadhaarAuth(AADHAARNo, $http, "OTP-BIO", false,  function (data) {     
            $scope.LoanGuarantor[index].Name = data.Name;
            $scope.LoanGuarantor[index].FatherName = data.FName;
            $scope.LoanGuarantor[index].Age = data.Age;
            $scope.LoanGuarantor[index].Address = data.Address;
            $scope.LoanGuarantor[index].MobileNo = data.Phone;
            $scope.LoanGuarantor[index].IsValidate = true;
            $scope.$apply();
        });
    };

    // start Clear
    $scope.ClearIsEmpoloyee = function (item) {
        item.MonthlyIncome = '';
        item.Source = '';
    }

    $scope.ClearIsregistration = function () {
        $scope.MemberRegistration.RegistrationName = '';
        $("#RegistrationDate").val('');
    }

    $scope.ClearIsDateOfEstablishment = function () {
        $scope.MemberRegistration.FundingSoFar = false;
        $scope.MemberRegistration.IsCollateralSecurity = false;
        $("#DateOfEstablishment").val('');
        $scope.MemberRegistration.SourceOfDebt = '';
        $scope.MemberRegistration.BailDocument = '';
        $scope.MemberRegistration.Repayment = '';
        $scope.MemberRegistration.RateOfInterest = '';
        $scope.MemberRegistration.CollateralSecurity = '';
    }

    $scope.WorkingCapital = false;

    $scope.ClearAddress = function () {
        $scope.MemberRegistration.OwnerName = '';
        $scope.MemberRegistration.OwnerAddress = '';
        $scope.RicoFile = '';
    }

    $scope.ClearFunding = function () {
        $scope.MemberRegistration.SourceOfDebt = '';
        $scope.MemberRegistration.BailDocument = '';
        $scope.MemberRegistration.Repayment = '';
        $scope.MemberRegistration.RateOfInterest = '';
    }

    $scope.ClearCollateralSecurity = function () {
        $scope.MemberRegistration.CollateralSecurity = '';
    }

    // end Clear

    $scope.CheckLoanAmountCapital = function (item) {    
        if (item.CapitalAsRequired == true && item.WorkingCapital == false) {
            item.LoanamountCapital = 1;          
            item.Both = false;
        }
        else if (item.WorkingCapital == true && item.CapitalAsRequired == false) {
            item.LoanamountCapital = 2;
            item.Both = false;
        }
        else if (item.Both == true) {
            item.LoanamountCapital = 3;
            item.WorkingCapital = true;
            item.CapitalAsRequired = true;
        }
    }
    

    /*-------Sector, Scheme, Purpose Master code block-------------------------*/

    $scope.LoanSectorAllList = [];
    $scope.ChangeLoanType = function (type, index) {
        $scope.LoanAmountDetails[index].LoanSectorList = [];
        $scope.LoanAmountDetails[index].LoanSectorList = $scope.LoanSectorAllList.filter(function (x) { return x.Farming_NonFarming === type });
    }
    
    $scope.LoanSchemeList = [];
    
    $scope.GetSchemeMasterBySector = function (Id, index) {
        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetSchemeMaster", { SectorId: Id }).
            success(function (data, status) {
                $scope.LoanSchemeList = [];
                $scope.LoanSchemeList = data;
                $scope.TotalRecord = $scope.LoanSchemeList.length;
                $scope.LoanAmountDetails[index].LoanSchemeList = data;           
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                alert("Error while fetching Society Details.");
                //$(".blockUI").fadeOut("slow");
            });
    }
    
    $scope.GetSchemeMaster = function () {
        return $http.post(SiteUrl + "/pldb/GetSchemeMaster", { SchemeId: $scope.LoanAmountDetails[0].SchemeId }).
            success(function (data, status) {
                $scope.LoanSchemeList = [];
                $scope.LoanSchemeList = data;
                $scope.TotalRecord = $scope.LoanSchemeList.length;

                if ($scope.SchemeUrlId > 0) {
                    $scope.LoanAmountDetails[0].IsDisableScheme = true;
                    $scope.LoanAmountDetails[0].LoanSchemeList = data;
                    $scope.LoanAmountDetails[0].SectorId = $scope.LoanAmountDetails[0].LoanSchemeList[0].sector_id.toString();
                }
                //    $scope.LoanAmountDetails[0].LoanSchemeList = $scope.LoanSchemeList.filter(function (x) { return x.scheme_id.toString() === $scope.SchemeUrlId });
                //}
                //$scope.LoanAmountDetails[0].SchemeId = $scope.SchemeUrlId.toString();
                //$scope.LoadSector($scope.LoanAmountDetails[0].SchemeId, '1');

            }).
            error(function (data, status) {
                alert("Error while fetching Society Details.");
                //$(".blockUI").fadeOut("slow");
            });
    }

    $scope.LoanSectorList = [];
    $scope.LoadSector = function (scheme_id, index) {
      
        return $http.post(SiteUrl + "/pldb/GetSectorMaster", { "WhereCase": "  and se.is_active=1 and se.scheme_id=" + scheme_id }).
            success(function (data, status) {
               
                $scope.LoanSectorList = [];
                $scope.LoanAmountDetails[index].LoanSectorList = [];
                $scope.LoanSectorList = data;
                if ($scope.SchemeUrlId > 0) {
                    $scope.LoanAmountDetails[index].LoanSectorList = data;
                    $scope.LoanAmountDetails[index].Type = $scope.LoanAmountDetails[index].LoanSectorList[0].Farming_NonFarming;

                }
                $scope.LoanSectorAllList = data;
            }).
            error(function (data, status) {
                
            });
    };
    if ($scope.LoanId == "") {
        $(".blockUI").fadeIn("slow");
        $scope.LoadMaster().then(function () {
            $scope.LoadSector(1, 0).then(function () {
                $scope.LoadPurpose().then(function () {
                    $scope.GetSchemeMaster().then(function () {
                        $(".blockUI").fadeOut("slow");
                    });
                });
            });
        });
    }

    $scope.LoadPurposeByShemeId = function (ShemeId, index) {
        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetPurposeMaster", { SchemeId: ShemeId }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.PurposeOfLoanList = [];
                $scope.PurposeOfLoanList = data;
                $scope.LoanAmountDetails[index].PurposeOfLoanList = data;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
            });
    };
        
    $scope.LoadPurpose = function (sector_id, index) {
        
        return $http.post(SiteUrl + "/pldb/GetPurposeMaster", { "WhereCase": sector_id }).
            success(function (data, status) {
               
                $scope.PurposeOfLoanList = [];
                $scope.PurposeOfLoanList = data;

                if ($scope.SchemeUrlId > 0) {
                    $scope.LoanAmountDetails[0].PurposeOfLoanList = data;
                    $scope.LoanAmountDetails[0].PurposeOfLoanList = $scope.PurposeOfLoanList.filter(function (x) { return x.purpose_id.toString() === $scope.SchemeUrlId });
                    $scope.LoanAmountDetails[0].SchemeId = $scope.LoanAmountDetails[0].PurposeOfLoanList[0].scheme_id.toString();
                    $scope.LoanAmountDetails[0].PurposeOfLoan = $scope.SchemeUrlId.toString();
                }


            }).
            error(function (data, status) {
             
            });
    };

    /*-------End Sector, Scheme, Purpose----------------------------------------*/


    /*--------- Application Edit code block---------------*/

    $scope.ApplicationEdit = function () {
        $(".se-pre-con").fadeIn("slow");
        return $scope.LoadMaster().then(function () {
        return $http.post(SiteUrl + "/pldb/GetApplicationDetails", { LoanId: $scope.LoanId }).
            success(function (data, status) {
                
                dataList = JSON.parse(data.DataList);

                var th = dataList.Table[0];

                $scope.LoanApplicationId = th.loan_id;
                $scope.IsSubmiApplication = th.issubmit;
                $scope.MemberRegistration.PLDB_ID = th.pldb_id.toString();
                $scope.MemberRegistration.Branch_ID = th.branch_id.toString();
                $scope.PLDBBranchList(th.branch_id.toString()).then(function () { $scope.MemberRegistration.Branch_ID = th.branch_id.toString(); });

                $scope.MemberRegistration.load_id = th.loan_id.toString();

                $scope.MemberRegistration.RegistrationNumber = th.RegistrationNumber.toString();
                if (th.RegistrationName != '' && th.RegistrationName != null) {
                    $scope.MemberRegistration.Isregistration = 'Yes';
                }
                else {
                    $scope.MemberRegistration.Isregistration = 'No';
                }

                $scope.MemberRegistration.RegistrationName = th.RegistrationName;
                $scope.MemberRegistration.RegistrationDate = th.RegistrationDateFormated;

                if (th.DateOfEstablishment != '' && th.DateOfEstablishment != null) {                
                    $scope.MemberRegistration.IsDateOfEstablishment = 'Yes';
                }
                else {
                    $scope.MemberRegistration.IsDateOfEstablishment = 'No';
                }

                $scope.MemberRegistration.DateOfEstablishment = th.DateOfEstablishmentFormated;
                $scope.MemberRegistration.AddressType = 'Rental';
                $scope.MemberRegistration.OwnerName = th.OwnerName;
                $scope.MemberRegistration.OwnerAddress = th.Address;
             
                $scope.MemberRegistration.BusinessLicense_RegiDescription = th.BusinessLicense_RegiDescription;
                $scope.MemberRegistration.KindOfBusiness = th.KindOfBusiness;
                $scope.MemberRegistration.BusinessItemName = th.BusinessItemName;
                $scope.MemberRegistration.MonthlyRequirementLivelihood = th.MonthlyRequirementLivelihood.toString();
                $scope.MemberRegistration.ProposedIndustrialActivity = th.ProposedIndustrialActivity;
                $scope.MemberRegistration.NumberOfPerson = th.NumberOfPerson.toString();

                if (th.SourceOfDebt != '' && th.SourceOfDebt != null) {
                    $scope.MemberRegistration.FundingSoFar = 'Thence';
                }
                else {
                    $scope.MemberRegistration.FundingSoFar = 'OwnFund';
                }

                $scope.MemberRegistration.SourceOfDebt = th.SourceOfDebt;
                $scope.MemberRegistration.BailDocument = th.BailDocument;
                $scope.MemberRegistration.Repayment = th.Repayment.toString();
                $scope.MemberRegistration.RateOfInterest = th.RateOfInterest.toString();

                if (th.CollateralSecurity != '' && th.CollateralSecurity != null) {
                    $scope.MemberRegistration.IsCollateralSecurity = 'Yes';
                }
                else {
                    $scope.MemberRegistration.IsCollateralSecurity = 'No';
                }
                $scope.MemberRegistration.CollateralSecurity = th.CollateralSecurity;
                               
                $scope.LoanDocument.RicoFile = th.RicoFile;
                $scope.LoanDocument.IsUploadRicoFile = th.RicoFile != '' && th.RicoFile != null;

                $scope.LoanDocument.IsUploadWorkingCapitalFile = th.WorkingCapitalFile != '' && th.WorkingCapitalFile != null;
                $scope.LoanDocument.WorkingCapitalFile = th.WorkingCapitalFile;
                
                $scope.LoanDocument.IsUploadApplicantDealAfterApprovalFile = th.ApplicantDealAfterApprovalFile != '' && th.ApplicantDealAfterApprovalFile != null;
                $scope.LoanDocument.ApplicantDealAfterApprovalFile = th.ApplicantDealAfterApprovalFile;
                
                $scope.LoanDocument.IsUploadBusinessAndRegiCertificateFile = th.BusinessAndRegiCertificateFile != '' && th.BusinessAndRegiCertificateFile != null;
                $scope.LoanDocument.BusinessAndRegiCertificateFile = th.BusinessAndRegiCertificateFile;
                
                $scope.LoanDocument.IsUploadProposedBuildingAndWorkStation = th.ProposedBuildingAndWorkStationFile != '' && th.ProposedBuildingAndWorkStationFile != null;
                $scope.LoanDocument.ProposedBuildingAndWorkStation = th.ProposedBuildingAndWorkStationFile;

                $scope.LoanDocument.IsUploadQuotation = th.QuotationFile != '' && th.QuotationFile != null;
                $scope.LoanDocument.Quotation = th.QuotationFile;

                $scope.LoanDocument.IsUploadIncomeTaxClearanceCertificate = th.IncomeTaxClearanceCertificate != '' && th.IncomeTaxClearanceCertificate != null;
                $scope.LoanDocument.IncomeTaxClearanceCertificate = th.IncomeTaxClearanceCertificate;




                if (dataList.Table1.length > 0) {
                    $scope.LoanAmountDetails = [];
                    dataList.Table1.forEach(function (itm, index) {
                        $scope.LoanAmountDetails.push({
                            Id: itm.loan_amount_id,
                            PurposeOfLoan: itm.loan_purpose_id,
                            EstimatedTotalAmount: itm.estimated_total_amount,
                            LoanAmountApplied: itm.loan_amount_applied,
                            IsDelete: itm.IsDelete,
                            SchemeId: itm.scheme_id,
                            SectorId: itm.sector_id,
                            LoanType: itm.loan_type.toString(),
                            LoanSchemeList: $scope.LoanSchemeList,
                            LoanSectorList: $scope.LoanSectorList,
                            LoanamountCapital: itm.LoanamountCapital,
                            Period: itm.Period,
                            IsDisableScheme: true,
                            CapitalAsRequired: itm.LoanamountCapital == 1,
                            WorkingCapital: itm.LoanamountCapital == 2,
                            Both: itm.LoanamountCapital == 3
                        });
                        $scope.CheckLoanAmountCapital($scope.LoanAmountDetails[0]);
                    });
                }
                                             
                if (dataList.Table2.length > 0) {
                    $scope.LoanMembers = [];
                    dataList.Table2.forEach(function (itm, index) {
                        $scope.ShowApplicanDetails = true;
                       
                        $scope.LoanMembers.push({
                            Id: itm.loan_memer_id,
                            ApplicantName: itm.MemberName,
                            FatherName: itm.FatherName,
                            ApplicantAge: itm.Age + (itm.Age != '' && itm.Age > 0 ? ' Year' : ''),
                            CategoryId: $scope._language == 'en-us' ? itm.CategoryEn : itm.CategoryHi,
                            Village: $scope._language == 'en-us' ? itm.VILLAGE_ENG : itm.VILLAGE_MANGAL,
                            VillagePanchayat: $scope._language == 'en-us' ? itm.GP_ENG : itm.GP_MANGAL,
                            MobileNo: itm.MobileNo,
                            Business: itm.Business,
                            BusinessAddress: itm.Address,
                            Photo: itm.Photo,
                            TehsilName: $scope._language == 'en-us' ? itm.BLOCK_ENG : itm.BLOCK_MANGAL,
                            VillageName: $scope._language == 'en-us' ? itm.VILLAGE_ENG : itm.VILLAGE_MANGAL,
                            GramPanchayatName: $scope._language == 'en-us' ? itm.GP_ENG : itm.GP_MANGAL,
                            FarmerCategory: $scope._language == 'en-us' ? itm.FarmerCategoryEn : itm.FarmerCategoryHi,
                            Tehsil: $scope._language == 'en-us' ? itm.BLOCK_ENG : itm.BLOCK_MANGAL,
                            MemberShip: itm.MembershipNo,
                            MembershipNumber: itm.MembershipNo,
                            AADHARNo: itm.AADHARNo,
                            ApplicantAadharNumber: itm.AADHARNo,
                            Educated_Experienced: itm.Educated_Experienced,
                            Source: itm.Source,
                            MonthlyIncome: itm.MonthlyIncome,
                            IsShow: true,
                            IsValidate: true,
                            IsEmployed: (itm.MonthlyIncome.toString() != '' || itm.MonthlyIncome != 0 || itm.MonthlyIncome.toString() != null) && itm.MonthlyIncome.toString() != '0' ? 'Yes' : 'No'
                        });
                    });
                }
                               
                if (dataList.Table3.length > 0) {
                    $scope.PropertyDetailsList = [];
                    if (dataList.Table3.filter(function (x) { return x.MemberType === 1 }).length > 0) {
                        dataList.Table3.filter(function (x) { return x.MemberType === 1 }).forEach(function (itm, index) {
                            $scope.PropertyDetailsList.push({
                                Id: itm.loan_property_id,
                                Khasara: itm.khasara,
                                Address: itm.Address,
                                TotalArea: itm.TotalArea,
                                OtherDetails: itm.other_property_details,
                                Specifications: itm.specifications,
                                IsDelete: false
                            });
                        });
                    } else {
                        $scope.PropertyDetailsList.push($scope.PropertyDetails);
                    }

                    $scope.GuarantorPropertyDetailsList = [];
                    if (dataList.Table3.filter(function (x) { return x.MemberType === 2 }).length > 0) {
                        dataList.Table3.filter(function (x) { return x.MemberType === 2 }).forEach(function (itm, index) {
                            $scope.GuarantorPropertyDetailsList.push({
                                Id: itm.loan_property_id,
                                Khasara: itm.khasara,
                                Address: itm.Address,
                                TotalArea: itm.TotalArea,
                                OtherDetails: itm.other_property_details,
                                Specifications: itm.specifications,
                                IsDelete: false
                            });
                        });
                    } else {
                        $scope.GuarantorPropertyDetailsList.push($scope.GuarantorPropertyDetails);
                    }
                }

                if (dataList.Table4.length > 0) {
                    $scope.LoanGuarantor = [];
                    dataList.Table4.forEach(function (itm, index) {
                        $scope.LoanGuarantor.push({
                            Id: itm.Guarantor_Id,
                            AADHARNo: itm.AadhaarNumber,
                            Name: itm.Name,
                            FatherName: itm.FatherName,
                            Age: itm.Age,
                            Address: itm.Address,
                            MobileNo: itm.Mobile,
                            IsValidate: true,
                            DOB: itm.DOBFormated,
                            CategoryId: itm.Category.toString()
                        });
                        $scope.LoanGuarantor[index].CategoryId = itm.Category.toString();
                    });
                } else {
                    $scope.LoanGuarantor = [];
                    $scope.LoanGuarantor.push($scope.Guarantor);
                }

                if (dataList.Table6.length > 0) {
                    $scope.LoadBankDetails = [];
                    dataList.Table6.forEach(function (itm, index) {
                        $scope.LoadBankDetails.push({
                            Id: itm.Id,
                            BankId: itm.BankId,
                            BankName: itm.BankName.toString(),
                            AccountType: itm.AccountType,
                            DateOfOpenAccount: itm.DateOfEstablishment,
                            LoanFacility: itm.LoanFacility,
                            IsDelete: false
                        });
                    });
                }
                
                if (dataList.Table7.length > 0) {
                    $scope.LoadPreLoanDetails = [];
                    dataList.Table7.forEach(function (itm, index) {
                        $scope.LoadPreLoanDetails.push({
                            Id: itm.loan_pre_loan_id,
                            Organization: itm.organization,
                            PreLoanAmount: itm.loan_amount,
                            RestAmount: itm.RestAmount,
                            MonthlyInstallment: itm.monthly_installment,
                            Tenure: itm.Tenure,
                            IntrestRate: itm.IntrestRate,
                            Source: itm.Source
                        });
                    });
                }
                                                                                 
                var LoanType = $scope.LoanAmountDetails[0].LoanType;

                $scope.LoadSector(1, 0).then(function () {
                    $scope.LoadPurpose().then(function () {
                        $scope.GetSchemeMaster().then(function () {
                            var th = dataList.Table[0];
                            $(".clspanal").hide();
                            $(".expandDetails").hide();
                            $scope.dvApplicationForm = true;
                            $scope.divLoanApplicationList = false;
                            $scope.ShowAddNewApplication = false;

                            $scope.LoanDetails = th;
                            $scope.LoanApplicationId = th.loan_id;
                            $scope.IsSubmiApplication = th.issubmit;
                            $scope.MemberRegistration.PLDB_ID = th.pldb_id.toString();
                            $scope.MemberRegistration.Branch_ID = th.branch_id.toString();
                            $scope.PLDBBranchList(th.branch_id.toString()).then(function () { $scope.MemberRegistration.Branch_ID = th.branch_id.toString(); });
                            $scope.LoanAmountDetails[0].LoanType = LoanType;

                            $(".se-pre-con").fadeOut("slow");
                            if (th.issubmit) {
                                $(".clspanal").find("input").attr("disabled", "disabled");
                                $(".clspanal").find("select").attr("disabled", "disabled");
                                $(".clspanal").find("textarea").attr("disabled", "disabled");
                                $(".clspanal").find("tfoot").hide();
                                $(".clspanal").find("i").hide();
                                $(".clspanal").eq(0).show();
                                $(".clspanal").eq(1).show();
                                $(".expandDetails").show();
                                $(".expandDetails").eq(0).removeClass("fa-plus-circle").addClass("fa-minus-circle");
                                $(".expandDetails").eq(1).removeClass("fa-plus-circle").addClass("fa-minus-circle");
                            } else {
                                $(".clspanal").show();
                            }
                                                        
                            dataList.Table6.forEach(function (itm, index11) {
                                $scope.LoadBankDetails[index11].BankId = itm.BankId.toString();
                            });

                            dataList.Table4.forEach(function (itm, index11) {
                                $scope.LoanGuarantor[index11].CategoryId = itm.Category.toString();
                            });

                               
                        });
                    });
                });

            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.LoanMembers = [];
                // $(".se-pre-con").fadeOut("slow");
            });
        });
    }

    if ($scope.LoanId != "") {
        $scope.ApplicationEdit();
    }

    /*--------- End Application code block */


    /*------- Start Loan  Guarantor code block--------------*/
    $scope.Guarantor = {
        index: 0,
        Id: "0",
        AADHARNo: "",
        Name: "",
        FatherName: "",
        Age: "",
        CategoryId: "",
        MobileNo: "",
        Photo: "",
        MemberShip: "",
        Address: "",
        IsDelete: false,
        IsValidate: false
    }

    $scope.ResetGuarantor = function () {
        $scope.Guarantor = {
            index: 0,
            Id: "0",
            AADHARNo: "",
            Name: "",
            FatherName: "",
            Age: "",
            CategoryId: "0",
            MobileNo: "",
            Photo: "",
            MemberShip: "",
            Address: "",
            IsDelete: false
        }
    }

    $scope.GuarantorPropertyDetails = {
        Village: "0",
        VillageList: [],
        GramPanchayat: "0",
        GramPanchayatList: [],
        Tehsil: "0",
        TehsilList: $scope.PLDBBranches,
        Khasara: "",
        ChahiBi: "",
        ChahiVi: "",
        BaraniBi: "",
        BaraniVi: "",
        OtherDetails: "",
        Specifications: "",
        IsDelete: false,
        Address: "",
        GuarantorPropertyTotalArea: "",
        GuarantorPropertyUnitOfArea: ""
    }

    $scope.ResetGuarantorPropertyDetails = function () {
        $scope.GuarantorPropertyDetails = {
            Village: "0",
            VillageList: [],
            GramPanchayat: "0",
            GramPanchayatList: [],
            Tehsil: "0",
            TehsilList: $scope.PLDBBranches,
            Khasara: "",
            ChahiBi: "",
            ChahiVi: "",
            BaraniBi: "",
            BaraniVi: "",
            OtherDetails: "",
            Specifications: "",
            IsDelete: false,
            Address: "",
            GuarantorPropertyTotalArea: "",
            GuarantorPropertyUnitOfArea: ""
        }
    }

    $scope.GuarantorPropertyDetailsList = [];

    $scope.GuarantorPropertyDetailsList.push($scope.GuarantorPropertyDetails);

    $scope.ValidateGuarantorProperty = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblGuarantorPropertyDetails").find('input[type="text"]').removeClass("notValid");
        $("#tblGuarantorPropertyDetails").find('select').removeClass("notValid");
        $scope.GuarantorPropertyDetailsList.forEach(function (item, index) {

            if (item.Khasara == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Guarantor Property details Khasara No' : 'कृपया गारंटर की प्रॉपर्टी का खसरा संख्या दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Guarantor Property details Khasara No.' : '<br />कृपया गारंटर की प्रॉपर्टी का खसरा संख्या दर्ज करें|';
                }
                $("#tblGuarantorPropertyDetails").find("tr").eq(index + 1).find('input[type="text"]').eq(0).addClass("notValid");
            }

            if (item.TotalArea == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Guarantor Property details Total Area' : 'कृपया गारंटर की प्रॉपर्टी का कुल क्षेत्रफल दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Guarantor Property details Total Area.' : '<br />कृपया गारंटर की प्रॉपर्टी का कुल क्षेत्रफल दर्ज करें|';
                }
                $("#tblGuarantorPropertyDetails").find("tr").eq(index + 1).find('input[type="text"]').eq(1).addClass("notValid");
            }

            if (item.Address == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Guarantor Property Address' : 'कृपया गारंटर की प्रॉपर्टी का पता दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Guarantor Property Address.' : '<br />कृपया गारंटर की प्रॉपर्टी का पता दर्ज करें|';
                }
                $("#tblGuarantorPropertyDetails").find("tr").eq(index + 1).find('input[type="text"]').eq(2).addClass("notValid");
            }


            //if (item.LoanAmountApplied == '') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Applied Amount' : 'कृपया आवेदित ऋण राशि दर्ज करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Enter Applied Amount.' : '<br />कृपया आवेदित ऋण राशि दर्ज करें|';
            //    }
            //    $("#tblPropertyDetails").find("tr").eq(index + 3).find('input[type="text"]').eq(1).addClass("notValid");
            //}
        });

        if (message.length > 29) {
            ShowOkMessage("Add Member", message, function () { });
            return false;
        }
        return true;
    }


    $scope.ValidateAddGuarantor = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblLoanGuarantorDetail").find('input[type="text"]').removeClass("notValid");
        $("#tblLoanGuarantorDetail").find('select').removeClass("notValid");
        $scope.LoanGuarantor.forEach(function (item, index) {

            if (!item.IsValidate) {
                if (message.indexOf($scope._language == 'en-us' ? 'The Aadhaar number of the Guarantor has not been verified. Please enter the Aadhaar number and click on the lock icon to verify.' : 'गारंटर की आधार संख्या वैरिफाई नहीं हुई  है। कृपया आधार संख्या दर्ज करें और वैरिफाई करने के लिए लॉक आइकन पर क्लिक करें।') < 0) {
                    message += $scope._language == 'en-us' ? '<br />The Aadhaar number of the Guarantor has not been verified. Please enter the Aadhaar number and click on the lock icon to verify.' : '<br />गारंटर की आधार संख्या वैरिफाई नहीं हुई  है। कृपया आधार संख्या दर्ज करें और वैरिफाई करने के लिए लॉक आइकन पर क्लिक करें।';
                }
                $("#tblLoanGuarantorDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(0).addClass("notValid");
            }
            if (item.MobileNo1 == "") {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Guarantor MobileNo Number' : 'कृपया गारंटर का मोबाइल नंबर दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Guarantor MobileNo Number.' : '<br />कृपया गारंटर का मोबाइल नंबर दर्ज करें।';
                }
                $("#tblLoanGuarantorDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(4).addClass("notValid");
            }
            if (item.CategoryId == "") {
                if (message.indexOf($scope._language == 'en-us' ? 'Please select Guarantor cast category' : 'कृपया गारंटर का वर्ग दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please select Guarantor cast category' : '<br />कृपया गारंटर का वर्ग दर्ज करें।';
                }
                $("#tblLoanGuarantorDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(6).addClass("notValid");
            }
        });
        if (message.length > 29) {
            ShowOkMessage("Add Member", message, function () { });
            return false;
        }
        return true;
    }
    if ($scope.LoanId == "") {
        $scope.LoanGuarantor = [];
        $scope.LoanGuarantor.push($scope.Guarantor);
    }
    $scope.RemoveGuarantorRow = function (index) {
        ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove guarantor from list.' : 'क्या आप वाकई गारंटर को सूची से हटाना चाहते हैं', function () {
            if ($scope.LoanGuarantor.length == 1) {
                $scope.ResetGuarantor();
                $scope.LoanGuarantor = [];
                $scope.LoanGuarantor.push($scope.Guarantor);
                $scope.ResetGuarantorPropertyDetails();
                $scope.GuarantorPropertyDetailsList = [];
                $scope.GuarantorPropertyDetailsList.push($scope.GuarantorPropertyDetails);
            } else {

                $scope.Guarantors = $scope.LoanGuarantor;
                $scope.LoanGuarantor = [];
                $scope.Guarantors.forEach(function (item, indx) {
                    if (indx != index) {
                        $scope.LoanGuarantor.push(item);
                    }
                });

                $scope.GuarantorProperty = $scope.GuarantorPropertyDetailsList;
                $scope.GuarantorPropertyDetailsList = [];
                $scope.GuarantorProperty.forEach(function (item, indx) {
                    if (indx != index) {
                        $scope.GuarantorPropertyDetailsList.push(item);
                    }
                });
            }

            $scope.$apply();
        });
    }

    $scope.AddNewGuarantor = function () {
        if ($scope.ValidateAddGuarantor(2)) {
            $scope.ResetGuarantor();           
            $scope.LoanGuarantor.push($scope.Guarantor);
            $scope.AddNewGuarantorPropertyDetails()
        }
    }

    $scope.AddNewGuarantorPropertyDetails = function () {
        // if ($scope.ValidateAddMember(1)) {
        // if ($scope.ValidatetblPropertyDetails()) {
        $scope.ResetGuarantorPropertyDetails();
        //$scope.LoanAmountDetails = [];
        $scope.GuarantorPropertyDetailsList.push($scope.GuarantorPropertyDetails);
        // }
        //}
    }

    $scope.OpenOtpPopUp = function (AADHAARNo, index) {
        AadhaarAuth(AADHAARNo, $http, "OTP-BIO", false,  function (data) {
    
            $scope.LoanGuarantor[index].Name = data.Name;
            $scope.LoanGuarantor[index].FatherName = data.FName;
            $scope.LoanGuarantor[index].Age = data.Age;
            $scope.LoanGuarantor[index].Address = data.Address;
            $scope.LoanGuarantor[index].MobileNo = data.Phone;
            $scope.LoanGuarantor[index].IsValidate = true;
            $scope.LoanGuarantor[index].DOB = data.DOB;
            $scope.$apply();
        });
    };
    /*------- End Loan  Guarantor code block--------------*/

    $scope.FileType = "";

    $scope.AddFile = function (type) {
        $scope.FileType = type;
    }
    $scope.ChangeFile = function (type) {
        if (type == 1) {
            $scope.LoanDocument.RicoFile = "";
            $scope.LoanDocument.IsUploadRicoFile = false;
        } else if (type == 2) {
            $scope.LoanDocument.IsUploadWorkingCapitalFile = false;
            $scope.LoanDocument.WorkingCapitalFile = "";
        } else if (type == 3) {
            $scope.LoanDocument.IsUploadApplicantDealAfterApprovalFile = false;
            $scope.LoanDocument.ApplicantDealAfterApprovalFile = "";
        } else if (type == 4) {
            $scope.LoanDocument.IsUploadBusinessAndRegiCertificateFile = false;
            $scope.LoanDocument.BusinessAndRegiCertificateFile = "";
        } else if (type == 5) {
            $scope.LoanDocument.IsUploadProposedBuildingAndWorkStation = false;
            $scope.LoanDocument.ProposedBuildingAndWorkStation = "";
        } else if (type == 6) {
            $scope.LoanDocument.IsUploadQuotation = false;
            $scope.LoanDocument.Quotation = "";
        } else if (type == 7) {
            $scope.LoanDocument.IsUploadIncomeTaxClearanceCertificate = false;
            $scope.LoanDocument.IncomeTaxClearanceCertificate = "";
        }
    }

    $scope.UploadFile = function (id, type) {
        try {
            // FormData
            if (window.FormData !== undefined) {
                var fileUpload = $("#" + id).get(0);
                var files = fileUpload.files;
                //var blob = b64toBlob(realData, contentType, 512);
                var formData = new FormData();
                formData.append("pldbfile", files[0]);
                $(".blockUI").show();
                $scope.xhr(SiteUrl + "/pldb/UploadLoanFile", formData, type);
            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }

    $scope.xhr = function (url, data, type) {
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
                            $scope.LoanDocument.RicoFile = request.responseText;
                        } else if (type == 2) {
                            $scope.LoanDocument.WorkingCapitalFile = request.responseText;
                        } else if (type == 3) {
                            $scope.LoanDocument.ApplicantDealAfterApprovalFile = request.responseText;
                        } else if (type == 4) {
                            $scope.LoanDocument.BusinessAndRegiCertificateFile = request.responseText;
                        } else if (type == 5) {
                            $scope.LoanDocument.ProposedBuildingAndWorkStation = request.responseText;
                        } else if (type == 6) {
                            $scope.LoanDocument.Quotation = request.responseText;
                        } else if (type == 7) {
                            $scope.LoanDocument.IncomeTaxClearanceCertificate = request.responseText;
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
    
    $scope.LoanDocument = {
        RicoFile: "",
        IsUploadRicoFile: false,
        IsUploadWorkingCapitalFile: false,
        WorkingCapitalFile: "",
        IsUploadApplicantDealAfterApprovalFile: false,
        ApplicantDealAfterApprovalFile: "",
        IsUploadBusinessAndRegiCertificateFile: false,
        BusinessAndRegiCertificateFile: "",
        IsUploadProposedBuildingAndWorkStation: false,
        ProposedBuildingAndWorkStation: "",
        IsUploadQuotation: false,
        Quotation: "",
        IsUploadIncomeTaxClearanceCertificate: false,
        IncomeTaxClearanceCertificate: "",

    }
});
CommityRegistrtionModule.directive('validfile', function validFile() {
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
                                scope.LoanDocument.RicoFile = "";
                                scope.LoanDocument.IsUploadRicoFile = true;
                            } else if (scope.FileType == 2) {
                                scope.LoanDocument.IsUploadWorkingCapitalFile = true;
                                scope.LoanDocument.WorkingCapitalFile = "";
                            } else if (scope.FileType == 3) {
                                scope.LoanDocument.IsUploadApplicantDealAfterApprovalFile = true;
                                scope.LoanDocument.ApplicantDealAfterApprovalFile = "";
                            } else if (scope.FileType == 4) {
                                scope.LoanDocument.IsUploadBusinessAndRegiCertificateFile = true;
                                scope.LoanDocument.BusinessAndRegiCertificateFile = "";
                            } else if (scope.FileType == 5) {
                                scope.LoanDocument.IsUploadProposedBuildingAndWorkStation = true;
                                scope.LoanDocument.ProposedBuildingAndWorkStation = "";
                            } else if (scope.FileType == 6) {
                                scope.LoanDocument.IsUploadQuotation = true;
                                scope.LoanDocument.Quotation = "";
                            }
                            else if (scope.FileType == 7) {
                                scope.LoanDocument.IsUploadIncomeTaxClearanceCertificate = true;
                                scope.LoanDocument.IncomeTaxClearanceCertificate = "";
                            }
                            scope.$apply();
                        }
                    }

                });
            };
        }
    };
});