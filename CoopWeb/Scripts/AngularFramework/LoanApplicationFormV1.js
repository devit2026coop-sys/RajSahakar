var CommityRegistrtionModule = angular.module('LoanApplicationMod', ['angularUtils.directives.dirPagination']);

CommityRegistrtionModule.controller('LoanApplicationCnt', function ($scope, $http, $rootScope) {

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


    $scope.ShowVehicleLoanApplication = false;
    $scope.LoanSSILoanApplicationForm = false;
    $scope.LoanApplicationLetterTitle = "";
    $scope.ShowApplicanDetails = false;
    $scope.IsShowPartialSave = false;
    $scope.IsSubmiApplication = false;
    $scope.LoanApplicationList = [];
    // $scope.LoanApplication.LOAN_ID = '0';

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
    $scope.LandDetailsGSS = [];
    $scope.SownCropDetailGSS = [];
    $scope.CommercialLoanDetail = [];
    $scope.IsCommercialBankDetailsShow = false;
    $scope.DairyDatail = false;
    $scope.divViewDairyFormShow = false;
    $scope.divParentDataSelection = true;
    $scope.LoanApplicationLetter = true;
    $scope.SelfDeclarationLetter = false;
    $scope.FarmerCategoryList = [];
    $scope.PurposeOfLoanList = []
    $scope.Member = {
        index: 0,
        Id: "0",
        MembershipNumber: "",
        AADHARNo: "",
        Name: "",
        FatherName: "",
        Age: "",
        FarmerCategory: "0",
        CategoryId: "0",
        Village: "0",
        VillagePanchayat: "0",
        Tehsil: "0",
        MobileNo: "",
        GramPanchayats: [],
        VillageList: [],
        Photo: "",
        VillageName: "",
        TehsilName: "",
        GramPanchayatName: "",
        DistrictName: "",
        IsDelete: false,
        IsMainMember: false,
        MemberShip: "",
        BusinessAddress: "",
        PermanentAddress: "",
        Business: "",
        IsValidate: false,
        MonthlyIncome: "",
        Education: "",
        FamilyDependents: "",
        MonthlyMinimumAmountForLiving: "",
        OfficeMobileNo: "",
        PartnershipCertifiedCopy: "",
        TransportCompanyRegCertification: "",
        DrivinglicenseCopy: "",
        VehicleRantProposalCopy: "",
        QuotationlCopy: "",
        PropertyDetailsList: []
    }

    $scope.PropertyDetails = {
        Village: "",//bhu
        VillageList: [],
        GramPanchayat: "0",
        GramPanchayatList: [],
        Tehsil: "",//bhu
        TehsilList: $scope.PLDBBranches,
        Khasara: "",
        ChahiBi: "",
        ChahiVi: "",
        BaraniBi: "",
        BaraniVi: "",
        OtherDetails: "",
        Specifications: "",
        IsDelete: false,
        TotalArea: "",
        Address: "",
        PropertyDocument: "",
        Isupload: false//bhupendra

    }
    $scope.Member.PropertyDetailsList.push($scope.PropertyDetails);

    $scope.IsShowPLDB = true;
    $scope.IsShowBranch = true;
    $scope.MemberRegistration = {};
    $scope.MemberRegistration.PLDB_ID = "0";
    $scope.MemberRegistration.Branch_ID = "0";
    $scope.MemberRegistration.DistrictId = "0";


    $scope.Od31 = $scope.Od31 || [];
    $scope.Od = $scope.Od || [];
    $scope.Cd = $scope.Cd || [];
    $scope.legalActions = $scope.legalActions || [];
    $scope.legalAction = $scope.legalAction || [];




    $scope.PLDB = {
        //ApplicantDetails: {
        //    Name: "",
        //    FatherHusbandName: "",
        //    Age: 0,
        //    CategoryId: 0,
        //    Gender: "",
        //    JanAadharNumber: "",
        //    MobileNo: "",
        //    EducationalQualificationId: "", // Ensure it's initialized
        //    NoOfLoaneeBeneficiary: 0,
        //    OtherPropertyDetails: "",
        //    CurrentDlcValue: 0.00,
        //    IslandMortgaged: 0,
        //    landMortgaged: "",
        //    PLDB_ID: "",
        //    Branch_ID: 0,
        //    NomineeName: "",
        //    NomineeRelationId: "",
        //    NomineeJanAadharNumber: "",
        //    NomineeMobileNo: "",
        //    NomineeBusiness: ""
        //},
        //BankDetails: [{
        //    BankId: "0",
        //    AccountNo: "",
        //    AccountType: "",
        //    DateOfOpenAccount: "",
        //    LoanFacility: ""
        //}],
        ApplicantDetails: [{}],
        BankDetails: [{}],
        LoanDetails: [{}],
        PermanentAddress: {},
        TempAddress: [{}]


        //LoanDetails: [{

        //    Type: ""
        //    , SectorId: ""
        //    , SchemeId: ""
        //    , PurposeOfLoanId: ""
        //    , LoanTypeId: 0
        //    , EstimatedTotalAmount: 0
        //    , LoanAmountApplied: 0
        //    , LoanCategory: 0
        //    , PeriodYears: "",

        //    Pre_CooperativeBankName: "",
        //    Pre_LoanAmount: 0.00,
        //    Pre_RestAmount: 0.00,
        //    Pre_MonthlyInstallment: 0,
        //    Pre_Tenure: 0,
        //    Pre_InterestRate: 0.00,
        //    Pre_Source: ""

        //}]

    }

    


    $scope.PLDBBranchList = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + $scope.PLDB.ApplicantDetails.PLDB_ID).
            success(function (data, status) {
                $scope.PLDBBranches = data;

                $scope.MemberRegistration.Branch_ID = branch_ID.toString();
                $scope.MemberRegistration.Branch_ID = '0';
                //$scope.PropertyDetailsList[0].TehsilList = data;
                //$scope.PropertyDetailsList[0].Tehsil = '0';;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };




    $scope.LoanAmount = {
        Id: "0",
        PurposeOfLoan: "0",
        EstimatedTotalAmount: "",
        LoanAmountApplied: "",
        IsDelete: false,
        SchemeId: "0",
        SectorId: "0",
        LoanType: "0",
        LoanSectorList: [],
        PurposeOfLoanList: [],
        LoanSchemeList: [],
        Period: "",
        CapitalAsRequired: "",
        WorkingCapital: ""
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
            LoanSectorList: [],
            PurposeOfLoanList: [],
            LoanSchemeList: [],
            Period: "",
            CapitalAsRequired: "",
            WorkingCapital: ""
        }
    }
    $scope.LoanAmountDetails = [];
    $scope.LoanAmountDetails.push($scope.LoanAmount);

    $scope.ResetMember = function () {
        $scope.Member = {
            index: 0,
            MemberId: "",
            MembershipNumber: "",
            AADHARNo: "",
            Name: "",
            FatherName: "",
            Age: "",
            FarmerCategory: "",
            CategoryId: "",
            Village: "",
            VillagePanchayat: "",
            Tehsil: "",
            MobileNo: "",
            GramPanchayats: "",
            VillageList: "",
            VillageName: "",
            TehsilName: "",
            GramPanchayatName: "",
            DistrictName: "",
            IsDelete: false,
            IsMainMember: false,
            MemberShip: "",
            BusinessAddress: "",
            PermanentAddress: "",
            Business: "",
            IsValidate: false,
            MonthlyIncome: "",
            Education: "",
            FamilyDependents: "",
            MonthlyMinimumAmountForLiving: "",
            OfficeMobileNo: "",
            PartnershipCertifiedCopy: "",
            TransportCompanyRegCertification: "",
            DrivinglicenseCopy: "",
            VehicleRantProposalCopy: "",
            QuotationlCopy: "",
            PropertyDetailsList: []
        }
        $scope.Member.PropertyDetailsList.push($scope.PropertyDetails);

    }


    $scope.ResetPropertyDetails = function () {
        $scope.PropertyDetails = {
            Village: "",//bhu
            VillageList: [],
            GramPanchayat: "0",
            GramPanchayatList: [],
            Tehsil: "",//bhu
            TehsilList: $scope.PLDBBranches,
            Khasara: "",
            ChahiBi: "",
            ChahiVi: "",
            BaraniBi: "",
            BaraniVi: "",
            OtherDetails: "",
            Specifications: "",
            IsDelete: false,
            TotalArea: "",
            Address: "",
            PropertyDocument: "",
            Isupload: false//bhupendra
        }
    }
    $scope.PropertyDetailsList = [];
    $scope.PropertyDetailsList.push($scope.PropertyDetails);

    $scope.MemberRegistration = {

        PLDB_ID: "0",
        Branch_ID: "0",
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
        IncomeTaxClearanceCertificate: "",
        Isregistration: "",
        IsDateOfEstablishment: "",
        AddressType: "",
        FundingSoFar: "",
        IsCollateralSecurity: ""

    }



    $scope.PreLoanDetails = {
        Id: "0",
        Organization: "",
        LoanAmount: "",
        MonthlyInstallment: "",
        IsDelete: false
    }
    $scope.ResetPreLoanDetails = function () {
        $scope.PreLoanDetails = {
            Id: "0",
            Organization: "",
            LoanAmount: "",
            MonthlyInstallment: "",
            IsDelete: false
        }
    }
    $scope.PreLoanDetailsList = [];
    $scope.PreLoanDetailsList.push($scope.PreLoanDetails);
    $scope.UserLevel = $("#hdnUserlevel").val();
    // START : Code for Language label setting
    $scope.getTranslation = function () {
        //_language = 'hi-in';
        $scope._language = _language;

        if (_language === 'en-us') {
            $http.get(SiteUrl + "/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                    $scope.LoanApplicationLetterTitle = $scope.translation.AgriculturalLoanApplicationLetter;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                    $scope.LoanApplicationLetterTitle = $scope.translation.AgriculturalLoanApplicationLetter;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }


    };

    $scope.PLDBList = [];
    $scope.CategoryList = [];
    $scope.FarmerCategoryList = [];
    $scope.LoanTypeList = [];
    $scope.ProcessStatus = [];

    $scope.PropertyDocumentType = [{ "Id": "Pay Slip", "NameEn": "Pay Slip", "NameHi": "वेतन पर्ची" }, { "Id": "Income Tax Return", "NameEn": "Income Tax Return", "NameHi": "आयकर रिटर्न" }, { "Id": "Building Lease", "NameEn": "Building Lease", "NameHi": "भवन का पट्टा" }, { "Id": "RC of vehicle", "NameEn": "RC of vehicle", "NameHi": "वाहन की आर.सी." }, { "Id": "KhasaraNo", "NameEn": "Khasara No", "NameHi": "खसरा नं" }, { "Id": "OtherType", "NameEn": "OtherType", "NameHi": "अन्य कोई" }];

    $scope.LoadMaster = function () {
        return $http.get(SiteUrl + "/pldb/LoadMaster").
            success(function (data, status) {
                $scope.PLDBList = JSON.parse(data.DataList).PLDB;
                $scope.CategoryList = JSON.parse(data.DataList).CATEGORY;
                $scope.FarmerCategoryList = JSON.parse(data.DataList).FarmerCategory;
                $scope.LoanTypeList = JSON.parse(data.DataList).LoanTypeList;
                $scope.BankList = JSON.parse(data.DataList).BANKLIST;
                // $scope.ProcessStatus = JSON.parse(data.DataList).ProcessStatus;
                $scope.Guarantor.CategoryList = $scope.CategoryList;

                if ($("#hdnUserType").val() != 'K' && $("#hdnUserType").val() != 'C') {
                    if ($("#hdnPLDBId").val() != "" && $("#hdnPLDBId").val() != "0") {
                        $scope.MemberRegistration.PLDB_ID = $("#hdnPLDBId").val();
                        $scope.IsShowPLDB = false;

                        $scope.PLDBBranchList($("#hdnBranchId").val()).then(function () {
                            if ($("#hdnBranchId").val() != "" && $("#hdnBranchId").val() != "0") {
                                $scope.Pldb.Branch_ID = $("#hdnBranchId").val();
                                //$scope.ShowMemberRegistrationForm();
                                $scope.IsShowBranch = false;
                                $scope.PLDBName = $scope.PLDB.filter(function (x) { return x.PldbId.toString() === $scope.Pldb.PLDB_ID })[0].PLDBNameHi;
                                $scope.BranchName = $scope._language == 'en-us' ? $scope.PLDBBranches.filter(function (x) { return x.BLOCK_ID.toString() === $scope.Pldb.Branch_ID })[0].BLOCK_ENG : $scope.PLDBBranches.filter(function (x) { return x.BLOCK_ID.toString() === $scope.Pldb.Branch_ID })[0].BLOCK_MANGAL;
                            }
                        });
                    }
                }

            }).
            error(function (data, status) {
                $scope.PLDB = [];
            });

    }

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




    //bhupendra

    $scope.GarnterAadharcardlist = [];

    // START : Get All All List
    $scope.GetMemberDetails = function (aId, mId, index) {



        $scope.GarnterAadharcardlist.push(aId);




        //$scope.GarnterAadharcardlist.push({
        //   AadharNo: aId
        //});

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
                        $scope.LoanMembers[index].Name = "";
                        $scope.LoanMembers[index].FatherName = "";
                        $scope.LoanMembers[index].Age = "";
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
                        $scope.LoanMembers[index].Education = "";
                        $scope.LoanMembers[index].QuotationlCopy = "";

                        ShowAlertMessage("", "दर्ज किया गया सदस्यता/आधार संख्या उपलब्ध नहीं हैं कृपया पहले सदस्यता के लिए आवेदन करें|", function () {
                            window.location.href = "../SocietyMemberRegistration/PLDBMemberRegistration";
                        });

                    } else {


                        $scope.LoanMembers[index].Name = data[0].MemberName;
                        $scope.LoanMembers[index].FatherName = data[0].FatherName;
                        $scope.LoanMembers[index].Age = data[0].Age + (data[0].Age != '' && data[0].Age > 0 ? ' Year' : '');
                        $scope.LoanMembers[index].CategoryId = $scope._language == 'en-us' ? data[0].CategoryEn : data[0].CategoryHi;

                        $scope.LoanMembers[index].Village = $scope._language == 'en-us' ? data[0].VILLAGE_ENG : data[0].VILLAGE_MANGAL;
                        $scope.LoanMembers[index].VillagePanchayat = $scope._language == 'en-us' ? data[0].GP_ENG : data[0].GP_MANGAL;
                        $scope.LoanMembers[index].MobileNo = data[0].MobileNo;
                        $scope.LoanMembers[index].Business = data[0].Business;
                        $scope.LoanMembers[index].BusinessAddress = data[0].Address;
                        $scope.LoanMembers[index].Photo = data[0].Photo;
                        $scope.LoanMembers[index].PermanentAddress = data[0].CurrentAddress;
                        //$scope.LoanMembers[index].DistrictName = data[0].DISTRICT_MANGAL;
                        $scope.LoanMembers[index].TehsilName = $scope._language == 'en-us' ? data[0].BLOCK_ENG : data[0].BLOCK_MANGAL;
                        $scope.LoanMembers[index].VillageName = $scope._language == 'en-us' ? data[0].VILLAGE_ENG : data[0].VILLAGE_MANGAL;
                        $scope.LoanMembers[index].GramPanchayatName = $scope._language == 'en-us' ? data[0].GP_ENG : data[0].GP_MANGAL;
                        $scope.LoanMembers[index].FarmerCategory = $scope._language == 'en-us' ? data[0].FarmerCategoryEn : data[0].FarmerCategoryHi;
                        $scope.LoanMembers[index].Tehsil = $scope._language == 'en-us' ? data[0].BLOCK_ENG : data[0].BLOCK_MANGAL;
                        $scope.LoanMembers[index].MemberShip = data[0].MembershipNo;
                        $scope.LoanMembers[index].Education = data[0].QualificationHi;

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



    // START : Save User
    $scope.SaveUser = function (IsSubmit) {

        ///alert(IsSubmit);

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

        //bhupendra loanmember

        var members = [];
        $scope.LoanMembers.forEach(function (itm) {
            members.push({
                Id: itm.Id == undefined ? 0 : itm.Id,
                MembershipNumber: itm.MemberShip,
                //Name: itm.Name,
                //FatherName: itm.FatherName,
                //Age: itm.Age,
                //FarmerCategory: itm.FarmerCategory,
                //CategoryId: itm.CategoryId,
                //Village: itm.Village,
                //VillagePanchayat: itm.VillagePanchayat,
                //Tehsil: itm.Tehsil,
                //MobileNo: itm.MobileNo,
                //Photo: itm.Photo,
                IsDelete: itm.IsDelete,
                IsMainMember: $scope.LoanMembers.length > 1 ? itm.IsMainMember : true,
                MemberBusiness: itm.Business,
                MonthlyIncome: itm.MonthlyIncome,
                EducationalQualification: itm.Education,
                FamilyDependents: itm.FamilyDependents,
                MonthlyMinimumAmountForLiving: itm.MonthlyMinimumAmountForLiving,
                BusinessAddress: itm.BusinessAddress,
                PermanentAddress: itm.PermanentAddress,
                TelephoneBusiness: itm.OfficeMobileNo,
                TelephoneResidence: itm.MobileNo,
                PartnershipCertifiedCopy: itm.PartnershipCertifiedCopy,
                TransportCompanyRegCertification: itm.TransportCompanyRegCertification,
                DrivinglicenseCopy: itm.DrivinglicenseCopy,
                VehicleRantProposalCopy: itm.VehicleRantProposalCopy,
                QuotationCopy: itm.QuotationlCopy
            });
        });

        var LoanAmount = [];
        $scope.LoanAmountDetails.forEach(function (itm) {
            LoanAmount.push({
                Id: itm.Id,
                PurposeOfLoan: itm.PurposeOfLoan,
                EstimatedTotalAmount: itm.EstimatedTotalAmount,
                LoanAmountApplied: itm.LoanAmountApplied,
                SchemeId: itm.SchemeId,
                SectorId: itm.SectorId,
                LoanType: itm.LoanType,
                LoanamountCapital: itm.WorkingCapital,//itm.WorkingCapital && itm.CapitalAsRequired ? 3 : itm.WorkingCapital ? 2 : itm.CapitalAsRequired ? 1 : 0,
                Period: itm.Period,
                IsDelete: itm.IsDelete

            });
        });

        var LoanProperty = [];


        $scope.LoanMembers.forEach(function (itm123) {
            itm123.PropertyDetailsList.forEach(function (itm, index) {
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
                        MemberType: 1,
                        PropertyDocument: itm.PropertyDocument,
                        Member: itm123.MembershipNumber,
                        Isupload: false,//bhupendra,
                        VillageName: itm.Village,
                        TehsilName: itm.Tehsil,


                    });
                }
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
                    MemberType: 2,
                    PropertyDocumentType: itm.PropertyDocumentType,
                    PropertyDocument: itm.GPropertyDocument,
                    Member: itm.Member,
                    Isupload: itm.Isupload//bhupendra
                });
            }
        });
        $scope.guarantor1 = [];
        $scope.LoanGuarantor.forEach(function (itm) {
            $scope.guarantor1.push({
                Guarantor_Id: itm.Id,
                AadhaarNumber: itm.AADHARNo,
                Name: itm.Name,
                FatherName: itm.FatherName,
                DOB: itm.DOB,
                Mobile: itm.MobileNo1,
                Address: itm.Address,
                Category: itm.CategoryId1
            });
        });

        $scope.vehicle = [];
        $scope.vehicle.push({
            VehicleId: $scope.LoanVehicle.VehicleId,
            VehicleType: $scope.LoanVehicle.VehicleType,
            Model: $scope.LoanVehicle.Model,
            Capacity: $scope.LoanVehicle.Capacity,
            AverageFuelConsumption: $scope.LoanVehicle.AverageFuelConsumption,
            VehicleVendorName: $scope.LoanVehicle.VehicleVendorName,
            ChassisPrice: $scope.LoanVehicle.ChassisPrice,
            BodyPrice: $scope.LoanVehicle.BodyPrice,
            OtherExpenses: $scope.LoanVehicle.OtherExpenses,
            ApplicantOwnContribution: $scope.LoanVehicle.ApplicantOwnContribution,
            CommutedLoanAmount: $scope.LoanVehicle.CommutedLoanAmount,
            AreaVehicleDriven: $scope.LoanVehicle.AreaVehicleDriven,
            StatePermitNo: $scope.LoanVehicle.StatePermitNo,
            StatePermitValidity: $scope.LoanVehicle.StatePermitValidity,
            MonthlyIncomeByVehicle: $scope.LoanVehicle.MonthlyIncomeByVehicle,
            ItemsBroughtByVehicle: $scope.LoanVehicle.ItemsBroughtByVehicle,
            IncomeBasis: $scope.LoanVehicle.IncomeBasis,
            VehicleRunDaysInMonth: $scope.LoanVehicle.VehicleRunDaysInMonth,
            VehicleRunPerDay: $scope.LoanVehicle.VehicleRunPerDay,
            ProposedBuildingAndWorkStation: $scope.LoanVehicle.ProposedBuildingAndWorkStation,
            FarePerKM: $scope.LoanVehicle.FarePerKM,
            WorkingCapitalFileFile: $scope.LoanVehicle.WorkingCapitalFileFile,
            ApplicantDealAfterApprovalFile: $scope.LoanVehicle.ApplicantDealAfterApprovalFile,
            EstimatedMonthlyEarnings: $scope.LoanVehicle.EstimatedMonthlyEarnings,
            OtherIncomeByVehicle: $scope.LoanVehicle.OtherIncomeByVehicle,
            MonthlyExpenditureFuel: $scope.LoanVehicle.MonthlyExpenditureFuel,
            DriverSalaryAllowance: $scope.LoanVehicle.DriverSalaryAllowance,
            RoadAndOtherTaxes: $scope.LoanVehicle.RoadAndOtherTaxes,
            RepairMaintenanceService: $scope.LoanVehicle.RepairMaintenanceService,
            InsurancePremiums: $scope.LoanVehicle.InsurancePremiums,
            OilSpareParts: $scope.LoanVehicle.OilSpareParts,
            GarageRent: $scope.LoanVehicle.GarageRent,
            WearAndTear: $scope.LoanVehicle.WearAndTear,
            OtherMiscellaneousExpenses: $scope.LoanVehicle.OtherMiscellaneousExpenses,
            InterestOnLoan: $scope.LoanVehicle.InterestOnLoan
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

        var checkDate1;
        var LoadBankDetails = [];
        $scope.LoadBankDetails.forEach(function (itm) {
            var DateOfOpenAccount = $("#DateOfOpenAccount").val();

            if (itm.DateOfOpenAccount != "") {
                var checkDate = $scope.ValidateBankDetailsOnSave(itm.DateOfOpenAccount);

                if (checkDate == false) { checkDate1 = checkDate }
            }
            if (!itm.IsDelete) {
                LoadBankDetails.push({
                    Id: itm.Id,
                    BankName: itm.BankId,
                    AccountType: itm.AccountType,
                    DateOfOpenAccount: itm.DateOfOpenAccount,
                    LoanFacility: itm.LoanFacility,
                    IsDelete: itm.IsDelete,
                    AccountNo: itm.AccountNo,
                    Member: itm.Member
                });
            }
        });

        if (checkDate1 == false) {
            $(".se-pre-con").fadeOut("slow");
            return false;
        }

        $scope.IndustryDetails = [];

        if ($scope.LoanSSILoanApplicationForm) {
            var RegistrationDate = $("#RegistrationDate").val();
            var DateOfEstablishment = $("#DateOfEstablishment").val();
            $scope.IndustryDetails.push({
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
            });
        } else {
            $scope.IndustryDetails.push({
                RegistrationNumber: null,
                RegistrationName: null,
                RegistrationDate: null,
                DateOfEstablishment: null,
                OwnerName: null,
                Address: null,
                BusinessLicense_RegiDescription: null,
                KindOfBusiness: null,
                BusinessItemName: null,
                MonthlyRequirementLivelihood: null,
                ProposedIndustrialActivity: null,
                NumberOfPerson: null,
                SourceOfDebt: null,
                BailDocument: null,
                Repayment: null,
                RateOfInterest: null,
                CollateralSecurity: null,
                RicoFile: null,
                WorkingCapitalFile: null,
                ApplicantDealAfterApprovalFile: null,
                BusinessAndRegiCertificateFile: null,
                ProposedBuildingAndWorkStationFile: null,
                QuotationFile: null,
                IncomeTaxClearanceCertificate: null,
            });
        }


        $scope.vehicle = [];
        if ($scope.ShowVehicleLoanApplication) {
            $scope.vehicle.push({
                VehicleId: $scope.LoanVehicle.VehicleId,
                VehicleType: $scope.LoanVehicle.VehicleType,
                Model: $scope.LoanVehicle.Model,
                Capacity: $scope.LoanVehicle.Capacity,
                AverageFuelConsumption: $scope.LoanVehicle.AverageFuelConsumption,
                VehicleVendorName: $scope.LoanVehicle.VehicleVendorName,
                ChassisPrice: $scope.LoanVehicle.ChassisPrice,
                BodyPrice: $scope.LoanVehicle.BodyPrice,
                OtherExpenses: $scope.LoanVehicle.OtherExpenses,
                ApplicantOwnContribution: $scope.LoanVehicle.ApplicantOwnContribution,
                CommutedLoanAmount: $scope.LoanVehicle.CommutedLoanAmount,
                AreaVehicleDriven: $scope.LoanVehicle.AreaVehicleDriven,
                StatePermitNo: $scope.LoanVehicle.StatePermitNo,
                StatePermitValidity: $scope.LoanVehicle.StatePermitValidity,
                MonthlyIncomeByVehicle: $scope.LoanVehicle.MonthlyIncomeByVehicle,
                ItemsBroughtByVehicle: $scope.LoanVehicle.ItemsBroughtByVehicle,
                IncomeBasis: $scope.LoanVehicle.IncomeBasis,
                VehicleRunDaysInMonth: $scope.LoanVehicle.VehicleRunDaysInMonth,
                VehicleRunPerDay: $scope.LoanVehicle.VehicleRunPerDay,
                ProposedBuildingAndWorkStation: $scope.LoanVehicle.ProposedBuildingAndWorkStation,
                FarePerKM: $scope.LoanVehicle.FarePerKM,
                WorkingCapitalFileFile: $scope.LoanVehicle.WorkingCapitalFileFile,
                ApplicantDealAfterApprovalFile: $scope.LoanVehicle.ApplicantDealAfterApprovalFile,
                EstimatedMonthlyEarnings: $scope.LoanVehicle.EstimatedMonthlyEarnings,
                OtherIncomeByVehicle: $scope.LoanVehicle.OtherIncomeByVehicle,
                MonthlyExpenditureFuel: $scope.LoanVehicle.MonthlyExpenditureFuel,
                DriverSalaryAllowance: $scope.LoanVehicle.DriverSalaryAllowance,
                RoadAndOtherTaxes: $scope.LoanVehicle.RoadAndOtherTaxes,
                RepairMaintenanceService: $scope.LoanVehicle.RepairMaintenanceService,
                InsurancePremiums: $scope.LoanVehicle.InsurancePremiums,
                OilSpareParts: $scope.LoanVehicle.OilSpareParts,
                GarageRent: $scope.LoanVehicle.GarageRent,
                WearAndTear: $scope.LoanVehicle.WearAndTear,
                OtherMiscellaneousExpenses: $scope.LoanVehicle.OtherMiscellaneousExpenses,
                InterestOnLoan: $scope.LoanVehicle.InterestOnLoan
            });
        }

        var applicaionDetails = {
            load_id: $scope.LoanApplicationId,
            pldb_id: $scope.MemberRegistration.PLDB_ID,
            branch_id: $scope.MemberRegistration.Branch_ID,
            Members: members,
            LoanAmount: LoanAmount,
            LoanProperty: LoanProperty,
            PreLoan: LoadPreLoanDetails,
            IsSubmit: IsSubmit,
            GuarantorDetails: $scope.guarantor1,
            VehicleDetails: $scope.vehicle,
            BankDetails: LoadBankDetails,
            IndustryDetails: $scope.IndustryDetails
        };


        formData.append("Applicationdata", JSON.stringify(applicaionDetails));
        formData.append("LoanAmount", JSON.stringify($scope.LoanAmountDetails));
        formData.append("LoanProperty", JSON.stringify($scope.PropertyDetailsList));
        formData.append("PreLoan", JSON.stringify($scope.PreLoanDetailsList));
        formData.append("GuarantorDetails", JSON.stringify($scope.guarantor1));
        formData.append("VehicleDetails", JSON.stringify($scope.vehicle));
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

                    var mssg = "";
                    if (IsSubmit) {
                        mssg = $scope._language == 'en-us' ? "Loan application has been submitted successfully.<br/><br/>Application No.:-<b>" + data.Result + "</b>" : "ऋण आवेदन सफलतापूर्वक जमा कर लिया गया है|<br/><br/>आवेदन संख्या:-<b>" + data.Result + "</b>";
                    } else {
                        mssg = $scope._language == 'en-us' ? "Loan application has been partialy saved successfully.<br/><br/>Application No.:-<b>" + data.Result + "</b>" : "ऋण आवेदन सफलतापूर्वक आंशिक सेव कर लिया गया है|<br/><br/>आवेदन संख्या:-<b>" + data.Result + "</b>";
                    }

                    ShowOkMessage("Success", mssg, function () {

                        if ($scope.SchemeUrlId > 0) {
                            location.reload();
                        } else if ($("#hdnUserlevel").val() != "0") {
                            location.href = "../pldb/LoanApplication";
                        } else {

                            location.href = "../pldb/LoanApplicationlist";
                        }

                    });
                } else {
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें| ", function () {

                    });
                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                ShowOkMessage("Error", $scope._lang$scope.ValidateAddMemberuage == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें| ", function () {
                });
            });
    };



    //$scope.ValidateVehicleDocument = function (type) {
    //    
    //    alert('ddd');
    //    $scope.value = $scope.LoanMembers[0].QuotationlCopy;
    //    alert($scope.value);
    //    if ($scope.value == '') {
    //        message += $scope._language == 'en-us' ? '<br /> Please Select PLDB.' : '<br />वाहन की कीमत का कोटशन दस्तावेज़।';
    //       // $("#ddlDistrict").addClass("notValid");
    //    }
    //}

    $scope.SaveLoanApplication = function (IsSubmit) {


        if (!IsSubmit) {

            if ($scope.ValidateLoanAmount()) {
                IsSubmit = true;
                if (IsSubmit) {
                    ShowAlertMessage("Alert", _language === 'en-us' ? "Are you sure you want to submit this loan application." : "क्या आप वाकई यह ऋण आवेदन जमा करना चाहते हैं?", function () {
                        IsSubmit = false;
                        $scope.SaveUser(IsSubmit);
                    });
                } else {
                    $scope.SaveUser(IsSubmit);
                }
            }
        }
        else {

            if ($scope.ValidateLoanAmount()) {
                if ($scope.ValidateAddMember(1)) {
                    if ($scope.ValidatetblPropertyDetails(2)) {
                        if ($scope.ValidateAddGuarantor(2)) {
                            if ($scope.ValidateGuarantorProperty(2)) {
                                if ($scope.ValidateVehicle()) {
                                    if ($scope.ValidateDescriptionOfIndustry(1)) {

                                        if (IsSubmit) {
                                            ShowAlertMessage("Alert", _language === 'en-us' ? "Are you sure you want to submit this loan application." : "क्या आप वाकई यह ऋण आवेदन जमा करना चाहते हैं?", function () {
                                                $scope.SaveUser(IsSubmit);
                                            });
                                        } else {
                                            $scope.SaveUser(IsSubmit);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
    }
    $scope.SowApplicationGrid = function () {
        window.location = '../pldb/PLDBSchemeList';
    }
    $scope.UserLevel = $("#hdnUserlevel").val();
    $scope.EditAppication = function () {
        $scope.IsSubmiApplication = false;
        $scope.IsShowPartialSave = false;
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

    $scope.SaveApplicationStatus = function (IsSubmit) {

        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();
        var mobile = "";
        $scope.LoanMembers.forEach(function (item, indx) {
            mobile += (mobile != '' ? '#' : '') + item.MobileNo
        })
        var applicaionDetails = {
            Status: $scope.MemberRegistration.ProcessStatus,
            Remark: $scope.MemberRegistration.Remarks,
            Id: $scope.LoanApplicationId,
            ApprovedAmount: $scope.MemberRegistration.ApprovedAmount,
            NameHi: $scope.LoanDetails.application_no,
            NameEn: $scope.ProcessStatus.filter(function (x) { return x.status_id.toString() === $scope.MemberRegistration.ProcessStatus.toString() })[0].ProceesStatusTextEn,
            Mobile: mobile
        };
        var documents = [];
        $scope.LOVFilesList.forEach(function (item, index) {
            if (item.IsUploadFileName) {
                documents.push({
                    Document_Id: item.Id,
                    Loan_Id: $scope.LoanApplicationId,
                    Document_Type: item.FileType,
                    Document_Url: item.FileName,
                    Is_Active: item.Is_Active,
                    Is_Delete: item.Is_Delete,
                });
            }
        });


        var LandAmunt = [];
        $scope.LoanMembers.forEach(function (itm123) {
            itm123.PropertyDetailsList.forEach(function (itm, index) {
                LandAmunt.push({
                    Id: itm.Id,
                    Loan_Id: $scope.LoanApplicationId,
                    PureProfitAmount: $scope.MemberRegistration.PureIncome,
                    LandValueAmount: itm.Rate,
                    LoanPayLimitAmount: itm.TotalLandValueSixty,
                    YearlyInstallment: $scope.MemberRegistration.AnnualInstalment,
                    PureSaving: $scope.MemberRegistration.PureSaving
                });
            });
        });

        $http.post(SiteUrl + "/pldb/UpdateLoanStatus", { Details: applicaionDetails, Level: "1", LVODocuments: documents, PldbLandAmount: LandAmunt }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.Result != "-1") {
                    ShowOkMessage("Success", $scope._language == 'en-us' ? "Loan application has been updated successfully." : "ऋण आवेदन सफलतापूर्वक अपडेट कर लिया गया है।", function () {
                        location.reload();
                    });
                } else {
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें।", function () {

                    });
                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें| ", function () {
                });
            });
    };

    $scope.SaveLoanApplicationStatus = function (IsSubmit) {

        var isValid = true;

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.MemberRegistration.ProcessStatus == '0' || $scope.MemberRegistration.ProcessStatus == undefined) {
            message += $scope._language == 'en-us' ? '<br /> Please Choose Application Status.' : '<br />कृपया आवेदन की स्थिति चुनें।';
            isValid = false;
        }
        if ($scope.MemberRegistration.Remarks == '' || $scope.MemberRegistration.Remarks == undefined) {
            message += $scope._language == 'en-us' ? '<br /> Please enter remarks.' : '<br />कृपया टिप्पणी दर्ज करें।';
            isValid = false;
        }

        if ($scope.LoanDetails.status == '12' && ($scope.MemberRegistration.ApprovedAmount == undefined || $scope.MemberRegistration.ApprovedAmount == '')) {
            message += $scope._language == 'en-us' ? '<br /> Please enter Approved Amount.' : '<br />कृपया स्वीकृत राशि दर्ज करें।';
            isValid = false;
        } else

            if ($scope.LoanDetails.status && (parseInt($scope.MemberRegistration.ApprovedAmount) > $scope.LoanAmountDetails[0].LoanAmountApplied)) {
                message += $scope._language == 'en-us' ? '<br /> Please enter the approved amount less than or equal to the applied amount ' + $scope.LoanAmountDetails[0].LoanAmountApplied + '.' : '<br />कृपया स्वीकृत राशि आवेदित ऋण राशि ' + $scope.LoanAmountDetails[0].LoanAmountApplied + ' से कम या उसके बराबर के  दर्ज करें।';
                isValid = false;
            }


        if ($scope.LoanDetails.status == '12') {
            $scope.LOVFilesList.forEach(function (item, indx) {
                $("#lvoFileType_" + indx).removeClass("notValid");
                if (item.IsUploadFileName && item.FileType == '') {
                    message += $scope._language == 'en-us' ? '<br />Please Enter selected type docuemnt.' : '<br />कृपया चयनित दस्तावेज़ का प्रकार दर्ज करें।';
                    $("#lvoFileType_" + indx).addClass("notValid");
                    isValid = false;
                }
                if (item.IsUploadFileName && item.FileName == '') {
                    message += $scope._language == 'en-us' ? '<br />Please upload selected docuemnt.' : '<br />कृपया चयनित दस्तावेज़ अपलोड करें।';
                    isValid = false;
                }
            });
        }

        if (!isValid) {
            ShowOkMessage("Error", message, function () {
            });
        } else {

            ShowAlertMessage("Alert", _language === 'en-us' ? "Are you sure you want to submit this loan application." : "क्या आप वाकई यह ऋण आवेदन जमा करना चाहते हैं?", function () {
                $scope.SaveApplicationStatus(IsSubmit);

            });
        }
    }







    //START : AddNewLandRecordGSS

    $scope.Applicaion = {
        IsApplicantShared: false,
        IsApplicantHasTransportCompany: false,
        IsDrivinglicense: false,
        IsProposedRented: false,
        IsQuotation: false
    }

    $scope.ValidateAddMember = function (IsSubmit) {


        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblLandMemberDetail").find('input[type="text"]').removeClass("notValid");
        $("#tblLandMemberDetail").find('select').removeClass("notValid");
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

        if ($scope.Applicaion.IsQuotation && $scope.LoanMembers[1].QuotationlCopy == '') {
            message += $scope._language == 'en-us' ? '<br /> Please upload a copy of the price quotation of the vehicle..' : '<br />कृपया वाहन की कीमत का कोटशन प्रति अपलोड करें।';
        }

        if ($scope.Applicaion.IsApplicantShared && $scope.LoanMembers[1].PartnershipCertifiedCopy == '') {
            message += $scope._language == 'en-us' ? '<br /> Please upload certified copy of partner registration/partnership records.' : '<br />कृपया सांझेदार की रजिस्ट्रेशन/सांझेदारी अभिलेख की प्रमाणित प्रति अपलोड करें।';
        }



        if ($scope.Applicaion.IsApplicantHasTransportCompany && $scope.LoanMembers[0].TransportCompanyRegCertification == '') {
            message += $scope._language == 'en-us' ? '<br /> Please upload the registration copy of the applicant transport company.' : '<br />कृपया आवेदक की ट्रांसपोर्ट की कम्पनी की रजिस्ट्रेशन प्रति अपलोड करें।';
        }

        if ($scope.Applicaion.IsDrivinglicense && $scope.LoanMembers[0].DrivinglicenseCopy == '') {
            message += $scope._language == 'en-us' ? '<br /> Please upload a certified copy of the driver driving license.' : '<br />कृपया ड्राईवर की ड्राईविंग लाइसेंस की प्रमाणित प्रति अपलोड करें।';
        }

        if ($scope.Applicaion.IsProposedRented && $scope.LoanMembers[0].VehicleRantProposalCopy == '') {
            message += $scope._language == 'en-us' ? '<br /> Vehicles are proposed to be given on rent to other organization, so upload a copy of the proposal from concerned.' : '<br />वाहन अन्य संस्था को किराए पर दिया जाना  प्रस्तावित हैं अतः सम्बंधित से प्रस्ताव की प्रति अपलोड करें।';
        }

        if (!$scope.LoanMembers[0].IsValidate) {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Aadhaar/Membership Number and add at least one applicant' : 'कृपया आधार / सदस्यता संख्या दर्ज करें और कम से कम एक आवेदक जोड़ें') < 0) {
                message += $scope._language == 'en-us' ? '<br />Please Enter Aadhaar/Membership Number and add at least one applicant.' : '<br />कृपया आधार / सदस्यता संख्या दर्ज करें और कम से कम एक आवेदक जोड़ें।';
            }
        } else {
            $scope.LoanMembers.forEach(function (item, index) {

                // alert($("#tblLandMemberDetail").find("tr").eq(index+1).find('input[type="text"]').length);

                if (item.MembershipNumber == '' && item.AADHARNo == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Aadhaar/Membership Number' : 'कृपया आधार अथवा सदस्यता क्रमांक दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please Enter Aadhaar/Membership Number.' : '<br />कृपया आधार अथवा सदस्यता क्रमांक दर्ज करें।';
                    }
                    $("#MembershipNumber_" + index).addClass("notValid");
                    $("#AADHARNo_" + index).addClass("notValid");
                }
                //if (item.Business == '') {
                //    if (message.indexOf($scope._language == 'en-us' ? 'Please Service/Business Name' : 'कृपया वर्त्तमान में सेवा/व्यवसाय दर्ज करें') < 0) {
                //        message += $scope._language == 'en-us' ? '<br />Please Service/Business Name.' : '<br />कृपया वर्त्तमान में सेवा/व्यवसाय दर्ज करें।';
                //    }
                //    $("#BusinessName_" + index).addClass("notValid");
                //}


                $("#panal_1").find("input,textarea").removeClass("notValid");
                //if (item.BusinessAddress == '') {
                //    if (message.indexOf($scope._language == 'en-us' ? 'Please Business Address' : 'कृपया निवास/व्यवसाय स्थल का पूर्ण पता दर्ज करें') < 0) {
                //        message += $scope._language == 'en-us' ? '<br />Please Business Address.' : '<br />कृपया निवास/व्यवसाय स्थल का पूर्ण पता दर्ज करें।';
                //    }
                //    $("#BusinessAddress_" + index).addClass("notValid");
                //}
                //if (item.PermanentAddress == '') {
                //    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Permanent Address' : 'कृपया स्थाई निवास का पूर्ण पता दर्ज करें') < 0) {
                //        message += $scope._language == 'en-us' ? '<br />Please Enter PermanentAddress.' : '<br />कृपया स्थाई निवास का पूर्ण पता दर्ज करें।';
                //    }
                //    $("#PermanentAddress_" + index).addClass("notValid");
                //}

                if (item.MobileNo == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Residence telephone' : 'कृपया टेलीफोन/मोबाइल 1 दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please Enter Residence telephone.' : '<br />कृपया टेलीफोन/मोबाइल 1 दर्ज करें।';
                    }
                    $("#MobileNo_" + index).addClass("notValid");
                }

                //if (item.OfficeMobileNo == '') {
                //    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Office telephone' : 'कृपया व्यवसाय का टेलीफोन दर्ज करें') < 0) {
                //        message += $scope._language == 'en-us' ? '<br />Please Enter Office telephone.' : '<br />कृपया व्यवसाय का टेलीफोन दर्ज करें।';
                //    }
                //    $("#OfficeMobileNo_" + index).addClass("notValid");
                //}

                //if (item.Education == '') {
                //    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter educational qualification' : 'कृपया आवेदक की शेक्षणिक योग्यता दर्ज करें') < 0) {
                //        message += $scope._language == 'en-us' ? '<br />Please Enter educational qualification.' : '<br />कृपया आवेदक की शेक्षणिक योग्यता दर्ज करें।';
                //    }
                //    $("#Education_" + index).addClass("notValid");
                //}

                //if (item.MonthlyIncome == '') {
                //    if (message.indexOf($scope._language == 'en-us' ? 'Please Enetr Yearly Income' : 'कृपया आवेदक की वार्षिक आय दर्ज करें') < 0) {
                //        message += $scope._language == 'en-us' ? '<br />Please Enetr Yearly Income.' : '<br />कृपया वार्षिक की मासिक आय दर्ज करें।';
                //    }
                //    $("#MonthlyIncome_" + index).addClass("notValid");
                //}

                //if (item.FamilyDependents == '') {
                //    if (message.indexOf($scope._language == 'en-us' ? 'Please Enetr the number of dependents of the applicant family' : 'कृपया आवेदक के परिवार के आश्रितों की संख्या दर्ज करें') < 0) {
                //        message += $scope._language == 'en-us' ? '<br />Please Enetr the number of dependents of the applicant family.' : '<br />कृपया आवेदक के परिवार के आश्रितों की संख्या दर्ज करें।';
                //    }
                //    $("#FamilyDependents_" + index).addClass("notValid");
                //}

                //if (item.MobileNo == '') {
                //    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Mobile Number' : 'कृपया मोबाइल नंबर दर्ज करें') < 0) {
                //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Mobile Number' : '\n <br /> कृपया मोबाइल नंबर दर्ज करें |';
                //    }
                //    $("#tblLandMemberDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(4).addClass("notValid");
                //}

            });

        }


        if (message.length > 29) {
            ShowOkMessage("Add Member", message, function () { });
            return false;
        }
        return true;
    }

    $scope.ValidateVehicle = function () {


        if (!$scope.ShowVehicleLoanApplication) {
            return true;
        }

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#panal_6").find("input,textarea").removeClass("notValid");


        if ($scope.Applicaion.IsQuotation == false && $scope.LoanMembers[0].QuotationlCopy == '') {
            message += $scope._language == 'en-us' ? '<br /> Please upload a copy of the price quotation of the vehicle..' : '<br />कृपया वाहन की कीमत का कोटशन प्रति अपलोड करें।';
        }

        if ($scope.LoanVehicle.VehicleType == '') {
            message += $scope._language == 'en-us' ? '<br /> Please Enter Vehicle Type.' : '<br />कृपया वाहन का नाम एवं प्रकार दर्ज करें।';
            $("#VehicleType_1").addClass("notValid");
        }

        if ($scope.LoanVehicle.Model == '') {
            message += $scope._language == 'en-us' ? '<br /> Please Enter Vehicle Model.' : '<br />कृपया वाहन का मॉडल दर्ज करें।';
            $("#Model_1").addClass("notValid");
        }
        if ($scope.LoanVehicle.Capacity == '') {
            message += $scope._language == 'en-us' ? '<br /> Please Enter Vehicle Capacity.' : '<br />कृपया भार वाहन की क्षमता दर्ज करें।';
            $("#Capacity_1").addClass("notValid");
        }

        if ($scope.LoanVehicle.VehicleVendorName == '') {
            message += $scope._language == 'en-us' ? '<br /> Please Enter Vehicle Vendor Name.' : '<br />कृपया वाहन विक्रेता का नाम दर्ज करें।';
            $("#VehicleVendorName_1").addClass("notValid");
        }
        if ($scope.LoanVehicle.ChassisPrice == '') {
            message += $scope._language == 'en-us' ? '<br /> Please Enter Chassis Price.' : '<br />कृपया चेसिसि का मूल्य दर्ज करें।';
            $("#ChassisPrice_1").addClass("notValid");
        }

        if ($scope.LoanVehicle.BodyPrice == '') {
            message += $scope._language == 'en-us' ? '<br /> Please Enter Vehicle Body Price.' : '<br />कृपया वाहन कि बॉडी का मूल्य दर्ज करें।';
            $("#BodyPrice_").addClass("notValid");
        }
        if ($scope.LoanVehicle.ApplicantOwnContribution == '') {
            message += $scope._language == 'en-us' ? '<br /> Please Enter Applicant Own Contribution.' : '<br />कृपया आवेदक का स्वयं का योगदान करें।';
            $("#ApplicantOwnContribution_1").addClass("notValid");
        }

        if (message.length > 29) {
            ShowOkMessage("Add Member", message, function () { });
            return false;
        }
        return true;


    }




    $scope.ValidateLoanAmount = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblLoanAmountDetail").find('input[type="text"]').removeClass("notValid");
        $("#tblLoanAmountDetail").find('select').removeClass("notValid");


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


        $scope.LoanAmountDetails.forEach(function (item, index) {


            //if (item.Type != 'Farming') {
            //    return true;
            //}
            if (item.PurposeOfLoan == '0') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Select Purpose Of Loan' : 'कृपया ऋण का उद्देश्य चयन करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Select Purpose Of Loan.' : '<br />कृपया ऋण का उद्देश्य चयन करें|';
                }
                $("#tblLoanAmountDetail").find("tr").eq(index + 1).find('select').eq(0).addClass("notValid");
            }
            if (item.EstimatedTotalAmount == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Estimated Total Amount' : 'कृपया अनुमानित कुल राशि दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Estimated Total Amount.' : '<br />कृपया अनुमानित कुल राशि दर्ज करें|';
                }
                $("#tblLoanAmountDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(0).addClass("notValid");
            } else
                if (($scope.LoanAmountDetails[0].PurposeOfLoan == 72 || $scope.LoanAmountDetails[0].PurposeOfLoan == 73) && parseInt(item.EstimatedTotalAmount) > 50000) {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Estimated Total Amount less than or equal to 50000' : 'कृपया अनुमानित कुल राशि 50000 से कम या उसके बराबर दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please Enter Estimated Total Amount less than or equal to 50000' : '<br />कृपया अनुमानित कुल राशि 50000 से कम या उसके बराबर दर्ज करें|';
                    }
                    $("#tblLoanAmountDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(0).addClass("notValid");
                }
            /*validate Loan amount*/
            if (item.LoanAmountApplied == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Applied Amount' : 'कृपया आवेदित ऋण राशि दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Applied Amount.' : '<br />कृपया आवेदित ऋण राशि दर्ज करें|';
                }
                $("#tblLoanAmountDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(1).addClass("notValid");
            } else if (($scope.LoanAmountDetails[0].PurposeOfLoan == 72 || $scope.LoanAmountDetails[0].PurposeOfLoan == 73) && parseInt(item.LoanAmountApplied) > 50000) {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Applied Amount less than or equal to 50000' : 'कृपया आवेदित ऋण राशि 50000 से कम या उसके बराबर दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Applied Amount less than or equal to 50000' : '<br />कृपया आवेदित ऋण राशि 50000 से कम या उसके बराबर दर्ज करें|';
                }
                $("#tblLoanAmountDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(1).addClass("notValid");
            }
            $("#WorkingCapital_" + index).removeClass("notValid");

            if (item.WorkingCapital == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'कृपया ऋण की आवश्यकता का चयन करें' : 'कृपया ऋण की आवश्यकता का चयन करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />कृपया ऋण की आवश्यकता का चयन करें' : '<br />कृपया ऋण की आवश्यकता का चयन करें';
                }
                $("#WorkingCapital_" + index).addClass("notValid");
            }

            if (item.Period == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Loan Period' : 'कृपया ऋण अवधि दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Loan Perio.' : '<br />कृपया ऋण अवधि दर्ज करें|';
                }
                $("#txtPeriod").addClass("notValid");
            }


        });

        if (message.length > 29) {
            ShowOkMessage("Add Member", message, function () { });
            return false;
        }
        return true;
    }

    $scope.ValidatetblPropertyDetails = function (type) {

        /*disable validation for स्वरोजगार क्रेडिट कार्ड योजना and महिला विकास योजना */
        if (($scope.LoanAmountDetails[0].PurposeOfLoan == 72 || $scope.LoanAmountDetails[0].PurposeOfLoan == 73) && type == 2) {
            return true;
        }

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblPropertyDetails").find('input[type="text"]').removeClass("notValid");
        $("#tblPropertyDetails").find('select').removeClass("notValid");
        $scope.PropertyDetailsList.forEach(function (item, index) {
            if (item.Khasara == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Member Property details Khasara No' : 'कृपया आवेदक की प्रॉपर्टी का खसरा संख्या दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Member Property details Khasara No.' : '<br />कृपया आवेदक की प्रॉपर्टी का खसरा संख्या दर्ज करें|';
                }
                $("#tblPropertyDetails").find("tr").eq(index + 1).find('input[type="text"]').eq(0).addClass("notValid");
            }

            if (item.TotalArea == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Member Property details Total Area' : 'कृपया आवेदक की प्रॉपर्टी का कुल क्षेत्रफल दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Member Property details Total Area.' : '<br />कृपया आवेदक की प्रॉपर्टी का कुल क्षेत्रफल दर्ज करें|';
                }
                $("#tblPropertyDetails").find("tr").eq(index + 1).find('input[type="text"]').eq(1).addClass("notValid");
            }

            if (item.ChahiVi == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Member Property details share of area' : 'कृपया आवेदक की प्रॉपर्टी का हिस्सा(%) क्षेत्रफल दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Member Property details share of area.' : '<br />कृपया आवेदक की प्रॉपर्टी का हिस्सा(%) क्षेत्रफल दर्ज करें|';
                }
                $("#tblPropertyDetails").find("tr").eq(index + 1).find('input[type="text"]').eq(2).addClass("notValid");
            }

            //if (item.Address == '') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Member Property Address' : 'कृपया आवेदक की प्रॉपर्टी का पता दर्ज करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Enter Member Property Address.' : '<br />कृपया आवेदक की प्रॉपर्टी का पता दर्ज करें|';
            //    }
            //    $("#txtAddress_" + index).addClass("notValid");
            //}
            //else {
            //    $("#txtAddress_" + index).removeClass("notValid");
            //}

        });

        if (message.length > 29) {
            ShowOkMessage("Add Member", message, function () { });
            return false;
        }
        return true;
    }

    $scope.ValidatetblPreLoanDetails = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblPreLoanDetails").find('input[type="text"]').removeClass("notValid");
        $scope.PreLoanDetailsList.forEach(function (item, index) {
            if (item.Organization == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Name of institution' : 'कृपया संस्था का नाम दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Name of institution.' : '<br />कृपया संस्था का नाम दर्ज करें|';
                }
                $("#tblPreLoanDetails").find("tr").eq(index + 1).find('input[type="text"]').eq(0).addClass("notValid");
            }
            if (item.LoanAmount == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Loan Amount' : 'कृपया ऋण राशि दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Loan Amount.' : '<br />कृपया ऋण राशि दर्ज करें|';
                }
                $("#tblPreLoanDetails").find("tr").eq(index + 1).find('input[type="text"]').eq(1).addClass("notValid");
            }
            if (item.MonthlyInstallment == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Monthly Installment' : 'कृपया मासिक किश्त दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Monthly Installment.' : '<br />कृपया मासिक किश्त दर्ज करें|';
                }
                $("#tblPreLoanDetails").find("tr").eq(index + 1).find('input[type="text"]').eq(2).addClass("notValid");
            }
        });

        if (message.length > 29) {
            ShowOkMessage("Add Member", message, function () { });
            return false;
        }
        return true;
    }

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

    //// START : On Page Load
    $scope.getTranslation();
    //$scope.GetBloodRelationList();
    //$scope.GetEconomicSectionList();
    $scope.getSchemeByUrl();
    //$scope.GetCategoryList();
    // $scope.LandDetailsGSS.push(new GSSLandDetailEntity());
    //$scope.GetFarmerCategory();

    $scope.LoanMembers = [];
    $scope.LoanMembers.push($scope.Member);


    ////bhupendra 
    ////आवेदक की भूमि व अन्य सम्पत्ति का विवरण -row
    //$scope.RemoveMemberLandProperty = function (index) {
    //    
    //    ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove amount details from list.' : 'क्या आप वाकई सूची से आवेदक की भूमि व अन्य सम्पत्ति का विवरण निकालना चाहते हैं', function () {
    //        $scope.PropertyDetailsList[index].IsDelete = true;
    //        $scope.$apply();
    //    });
    //}


    //$scope.PLDBList();

    //// END : On Page Load
    $scope.RemoveMemberRow = function (index) {

        ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove member from list.' : 'क्या आप वाकई सदस्य को सूची से हटाना चाहते हैं', function () {
            if ($scope.LoanMembers.length == 1) {
                $scope.ResetMember();
                $scope.LoanMembers = [];
                $scope.GarnterAadharcardlist = [];//bhupendra
                $scope.LoanMembers.push($scope.Member);
            } else {
                $scope.GarnterAadharcardlist.splice(index, 1);//bhupendra
                $scope.Members = $scope.LoanMembers;
                $scope.LoanMembers = [];
                $scope.Members.forEach(function (item, indx) {
                    if (indx != index) {
                        $scope.LoanMembers.push(item);
                    }
                });
                //$scope.LoanMembers[index].IsDelete = true;
            }

            $scope.$apply();
        });
    }


    $scope.RemoveLoanAmountRow = function (index) {
        ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove amount details from list.' : 'क्या आप वाकई सूची से राशि विवरण निकालना चाहते हैं', function () {
            $scope.LoanAmountDetails[index].IsDelete = true;
            $scope.$apply();
        });
    }

    $scope.RemovePropertyRow = function (item, index) {

        ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove details from list.' : 'क्या आप वाकई सूची से विवरण निकालना चाहते हैं', function () {


            item.PropertyDetailsList[index].IsDelete = true;
            //$scope.Propertys = item.PropertyDetailsList;
            //$scope.PropertyDetailsList = [];
            //$scope.Propertys.forEach(function (item, indx) {
            //    if (indx != index) {
            //        
            //        $scope.PropertyDetailsList.push(item);
            //    }
            //});

            item.PropertyDetailsList.splice(index, 1);
            $scope.CalculateHissaValue(item, item.PropertyDetailsList[index]);


            $scope.$apply();



        });
    }

    $scope.RemovePreLoanRow = function (index) {
        ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove details from list.' : 'क्या आप वाकई सूची से विवरण निकालना चाहते हैं', function () {
            $scope.PreLoanDetailsList[index].IsDelete = true;
            $scope.$apply();
        });
    }


    $scope.GetPropertyDetailsPanchayt = function (index) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.PropertyDetailsList[index].Tehsil).
            success(function (data, status) {
                $scope.PropertyDetailsList[index].GramPanchayatList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.GramPanchayats = [];
                $(".se-pre-con").fadeOut("slow");
            });

    }

    $scope.GetPropertyDetailsVillages = function (index) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=VILLAGE&MasterCode=" + $scope.PropertyDetailsList[index].GramPanchayat).
            success(function (data, status) {
                $scope.PropertyDetailsList[index].VillageList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.GramPanchayats = [];
                $(".se-pre-con").fadeOut("slow");
            });

    }
    $scope.AddNewMember = function () {
        if ($scope.ValidateAddMember(2)) {

            $scope.ResetPropertyDetails();
            $scope.ResetMember();
            $scope.Member.GramPanchayats = $scope.LoanMembers[$scope.LoanMembers.length - 1].GramPanchayats;

            $scope.LoanMembers.push($scope.Member);
        }
    }




    $scope.AddLandDetail = function () {
        if ($scope.ValidateAddMember(1)) {
            if ($scope.ValidateLoanAmount()) {
                $scope.ResetAmountDetails();
                //$scope.LoanAmountDetails = [];

                $scope.LoanAmount.LoanSchemeList = $scope.LoanSchemeList;


                $scope.LoanAmountDetails.push($scope.LoanAmount);
            }
        }
    }
    $scope.AddNewPropertyDetails = function (item, idx) {

        if ($scope.ValidateAddMember(1)) {


            var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
            $("#tblPropertyDetails").find('input[type="text"]').removeClass("notValid");
            $("#tblPropertyDetails").find('select').removeClass("notValid");
            item.PropertyDetailsList.forEach(function (item, index) {
                if (item.Khasara == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Member Property details Khasara No' : 'कृपया आवेदक की प्रॉपर्टी का खसरा संख्या दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please Enter Member Property details Khasara No.' : '<br />कृपया आवेदक की प्रॉपर्टी का खसरा संख्या दर्ज करें|';
                    }
                    $("#Khasara_" + idx + "_" + index).addClass("notValid");
                    //$("#tblPropertyDetails").find("tr").eq(index + 1).find('input[type="text"]').eq(0).addClass("notValid");
                }

                if (item.TotalArea == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Member Property details Total Area' : 'कृपया आवेदक की प्रॉपर्टी का कुल क्षेत्रफल दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please Enter Member Property details Total Area.' : '<br />कृपया आवेदक की प्रॉपर्टी का कुल क्षेत्रफल दर्ज करें|';
                    }
                    $("#TotalArea_" + idx + "_" + index).addClass("notValid");
                    //$("#tblPropertyDetails").find("tr").eq(index + 1).find('input[type="text"]').eq(1).addClass("notValid");
                }

                if (item.ChahiVi == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Member Property details share of area' : 'कृपया आवेदक की प्रॉपर्टी का हिस्सा(%) क्षेत्रफल दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please Enter Member Property details share of area.' : '<br />कृपया आवेदक की प्रॉपर्टी का हिस्सा(%) क्षेत्रफल दर्ज करें|';
                    }
                    $("#ChahiVi_" + idx + "_" + index).addClass("notValid");
                    //$("#tblPropertyDetails").find("tr").eq(index + 1).find('input[type="text"]').eq(2).addClass("notValid");
                }

            });

            if (message.length > 29) {
                ShowOkMessage("Add Member", message, function () { });
                return false;
            } else {
                $scope.ResetPropertyDetails();
                //$scope.LoanAmountDetails = [];
                item.PropertyDetailsList.push($scope.PropertyDetails);
            }

        }
    }






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
                var data = list.filter(function (x) { return x.VILLAGE_ID === id.toString() })[0];
                return _language == 'en-us' ? data.VILLAGE_ENG : data.VILLAGE_MANGAL;
            } else if (type == 'Branch') {
                var data = list.filter(function (x) { return x.BLOCK_ID === id.toString() })[0];
                return _language == 'en-us' ? data.BLOCK_ENG : data.BLOCK_MANGAL;
            }
        } catch (ex) {
            return "";
        }
    }


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


    /*-------Sector, Scheme, Purpose Master code block-------------------------*/

    $scope.LoanSectorAllList = [];
    $scope.ChangeLoanType = function (type, index) {

        $scope.PLDB.LoanDetails[index].LoanSectorList = [];
        $scope.PLDB.LoanDetails[index].LoanSectorList = $scope.LoanSectorAllList.filter(function (x) { return x.Farming_NonFarming === type });
    }


    $scope.LoanSchemeList = [];

    $scope.IsShowGarantor = true;
    $scope.GetSchemeMasterBySector = function (Id, index) {
        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetSchemeMaster", { SectorId: Id }).
            success(function (data, status) {
                $scope.LoanSchemeList = [];
                $scope.LoanSchemeList = data;
                $scope.TotalRecord = $scope.LoanSchemeList.length;
                $scope.PLDB.LoanDetails[index].LoanSchemeList = data;

                //if ($scope.LoanAmountDetails[0].SectorId == "2") {
                //    $scope.IsShowGarantor = false;
                //} else {
                //    $scope.IsShowGarantor = true;
                //}

                //if (($scope.LoanAmountDetails[0].PurposeOfLoan == 72 || $scope.LoanAmountDetails[0].PurposeOfLoan == 73)) {
                //    $scope.IsShowGarantor = false;
                //} else {
                //    $scope.IsShowGarantor = true;
                //}

                //    $scope.LoanAmountDetails[0].LoanSchemeList = $scope.LoanSchemeList.filter(function (x) { return x.scheme_id.toString() === $scope.SchemeUrlId });
                //}
                //$scope.LoanAmountDetails[0].SchemeId = $scope.SchemeUrlId.toString();
                //$scope.LoadSector($scope.LoanAmountDetails[0].SchemeId, '1');
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
                $(".blockUI").fadeOut("slow");
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
                $scope.PLDB.LoanDetails[index].PurposeOfLoanList = data;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
            });
    };
    $scope.PropertyTotalArea = 0;
    $scope.PropertyTotalHissaValue = 0;
    $scope.CalculateHissaValue = function (itmmm, item) {

        try {
            if
                (item.ChahiVi.indexOf('/') > -1) {
                item.BaraniBi = ((item.TotalArea * item.ChahiVi.split('/')[0]) / item.ChahiVi.split('/')[1]).toFixed(2);
                item.BaraniVi = item.BaraniBi < 1 ? 'सीमांत कृषक' : item.BaraniBi > 1 && item.BaraniBi < 2 ? 'लघु कृषक' : 'अन्य किसान';
                item.LandInBigha = (item.BaraniBi * 10000 / 2530).toFixed(2);

                itmmm.PropertyTotalArea = 0;
                itmmm.PropertyTotalHissaValue = 0;
                itmmm.TLandInBigha = 0;
                itmmm.PropertyDetailsList.forEach(function (itm, indx) {
                    itmmm.PropertyTotalArea += parseFloat(itm.TotalArea);
                    //PropertyTotalHissa += item.TotalHissaValue;
                    itmmm.PropertyTotalHissaValue += parseFloat(itm.BaraniBi);
                    itmmm.TLandInBigha += parseFloat(itm.LandInBigha);
                });
                itmmm.TtotalHissaVarg = itmmm.PropertyTotalHissaValue < 1 ? 'सीमांत कृषक' : itmmm.PropertyTotalHissaValue > 1 && itmmm.PropertyTotalHissaValue < 2 ? 'लघु कृषक' : 'अन्य किसान';



            } else {
                item.BaraniBi = (item.TotalArea * item.ChahiVi.split('/')[0]);
                item.BaraniVi = item.BaraniBi < 1 ? 'सीमांत कृषक' : item.BaraniBi > 1 && item.BaraniBi < 2 ? 'लघु कृषक' : 'अन्य किसान';
                itmmm.PropertyTotalArea = 0;
                itmmm.PropertyTotalHissaValue = 0;
                item.LandInBigha = (item.BaraniBi * 10000 / 2530).toFixed(2);
                itmmm.TLandInBigha = 0;
                itmmm.PropertyDetailsList.forEach(function (itm, indx) {
                    itmmm.PropertyTotalArea += parseFloat(itm.TotalArea);
                    //PropertyTotalHissa += item.TotalHissaValue;
                    itmmm.PropertyTotalHissaValue += parseFloat(itm.BaraniBi);
                    itmmm.TLandInBigha += parseFloat(itm.LandInBigha);
                });
                itmmm.TtotalHissaVarg = itmmm.PropertyTotalHissaValue < 1 ? 'सीमांत कृषक' : itmmm.PropertyTotalHissaValue > 1 && itmmm.PropertyTotalHissaValue < 2 ? 'लघु कृषक' : 'अन्य किसान';



            }
        } catch (ex) {
            item.TotalHissaValue = "";
            item.HissaVarg = "";
        }
    }

    $scope.SetloanPeriod = function () {
        $scope.PLDB.LoanDetails[0].Period = $scope.PurposeOfLoanList.filter(function (x) { return x.purpose_id.toString() === $scope.PLDB.LoanDetails[0].PurposeOfLoan.toString() })[0].LoanPeriod;


    }

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
                    $scope.LoanAmountDetails[0].Period = $scope.LoanAmountDetails[0].PurposeOfLoanList[0].LoanPeriod;
                    if ($scope.LoanAmountDetails[0].PurposeOfLoanList[0].FormType == 1) {
                        $scope.ShowVehicleLoanApplication = false;
                        $scope.LoanSSILoanApplicationForm = true;
                        //$scope.LoanApplicationLetterTitle = $scope.translation.NonAgriculturalLoanApplicationLetter == undefined ? $scope.translation.NonAgriculturalLoanApplicationLetter : $scope.LoanApplicationLetterTitle ;
                    } else if ($scope.LoanAmountDetails[0].PurposeOfLoanList[0].FormType == 2) {
                        $scope.ShowVehicleLoanApplication = true;
                        $scope.LoanSSILoanApplicationForm = false;
                        //$scope.LoanApplicationLetterTitle = $scope.translation.NonAgriculturalLoanApplicationLetter == undefined ? $scope.translation.NonAgriculturalLoanApplicationLetter : $scope.LoanApplicationLetterTitle ;
                    } else {
                        $scope.ShowVehicleLoanApplication = false;
                        $scope.LoanSSILoanApplicationForm = false;
                        //$scope.LoanApplicationLetterTitle = $scope.translation.AgriculturalLoanApplicationLetter == undefined ? $scope.translation.AgriculturalLoanApplicationLetter : $scope.LoanApplicationLetterTitle;
                    }
                }


            }).
            error(function (data, status) {

            });
    };

    /*-------End Sector, Scheme, Purpose----------------------------------------*/



    /*--------- Application Edit code block---------------*/

   

    if ($scope.LoanId != "") {
        $scope.ApplicationEdit();
    }

    /*--------- End Application code block */

    // Start : Validations  
    $scope.PLDBLoanSmall_IndustryList = [];
    $scope.PLDBLoanSmall_IndustryList.push($scope.MemberRegistration);
    $scope.ValidateDescriptionOfIndustry = function (IsSubmit) {


        if (!$scope.LoanSSILoanApplicationForm) {
            return true;
        }

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

            if ($scope.MemberRegistration.IsDateOfEstablishment == 'Yes') {

                if ($scope.MemberRegistration.FundingSoFar == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please choose how funding has been provided in the ongoing activities.' : 'चालू गतिविधियों में अब तक वित्त पोषण कैसे दिया गया कृपया चुनें |') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please choose how funding has been provided in the ongoing activities..' : '<br />चालू गतिविधियों में अब तक वित्त पोषण कैसे दिया गया कृपया चुनें|';
                    }
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

            if ($scope.MemberRegistration.IsCollateralSecurity == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please choose Collateral Security' : 'कृपया जमानत की सुरक्षा चुनें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please choose Collateral Security.' : '<br />कृपया जमानत की सुरक्षा चुनें।';
                }
                $("#txtCollateralSecurity").addClass("notValid");
            }
            else {
                $("#txtCollateralSecurity").removeClass("notValid");
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



            $scope.Application = {
                IsRicoFile: false,
                IsApplicantHasApplicantDealAfterApproval: false,
                IsWorkingCapitalFile: false,
                IsProposedBuildingAndWorkStation: false,
                IsBusinessAndRegiCertificateFile: false,
                IsIncomeTaxClearanceCertificate: false,
                IsQuotation: false
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


            //Start File upload validation

            if ($scope.MemberRegistration.AddressType == 'IsRico' && ($scope.LoanDocument.RicoFile == '' || $scope.LoanDocument.RicoFile == null)) {
                if (($scope.RicoFile == undefined || $scope.RicoFile == '') && $scope.RicoFile != "1") {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Upload Rico File' : '\n <br /> कृपया रिको फ़ाइल अपलोड करें |';
                }
            }

            if ($scope.LoanAmountDetails[0].WorkingCapital == "2") {
                if ($scope.LoanDocument.WorkingCapitalFile == '') {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Upload Working Capital File' : '\n <br /> कृपया कार्यशील पूंजी की फ़ाइल अपलोड करें|';
                }
            }

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

            //End File upload validation

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

    $scope.ChooseDefaultMember = function (index) {
        $scope.LoanMembers.forEach(function (item, indx) {
            if (index != indx) {
                item.IsMainMember = false;
            }
        });
    }


    $scope.PropertyUnitOfArea = [{ "value": "1", "TextEn": "हैक्टर", "TextHi": "हैक्टर" }, { "value": "2", "TextEn": "बीघा", "TextHi": "बीघा" }, { "value": "3", "TextEn": "एकड़ जमीन", "TextHi": "एकड़ जमीन" }, { "value": "4", "TextEn": "वर्ग फुट", "TextHi": "वर्ग फुट" }, { "value": "5", "TextEn": "वर्ग गज", "TextHi": "वर्ग गज" }];


    /*------- Start Loan  Guarantor code block--------------*/
    $scope.Guarantor = {
        index: 0,
        Id: "0",
        AADHARNo: "",
        Name: "",
        FatherName: "",
        Age: "",
        CategoryId1: "",
        MobileNo1: "",
        Photo: "",
        MemberShip: "",
        Address: "",
        IsDelete: false,
        IsValidate: false,
        CategoryList: $scope.CategoryList
    }

    $scope.ResetGuarantor = function () {
        $scope.Guarantor = {
            index: 0,
            Id: "0",
            AADHARNo: "",
            Name: "",
            FatherName: "",
            Age: "",
            CategoryId1: "",
            MobileNo1: "",
            Photo: "",
            MemberShip: "",
            Address: "",
            IsDelete: false,
            IsValidate: false
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
        GuarantorPropertyUnitOfArea: "",
        TotalArea: "",
        GPropertyDocument: "",
        PropertyDocumentTypeList: $scope.PropertyDocumentType
    }

   
    $scope.OpenOtpPopUp = function (AADHAARNo, index) {

        var isValid = true;
        if ($scope.LoanMembers[0].AADHARNo == "") {
            var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
            message += $scope._language == 'en-us' ? '<br /> Please Member Information Enter.' : '<br />कृपया आवेदक का विवरण का चयन करें।';
            ShowOkMessage("", message, function () { });
        }
        else {
            if ($scope.LoanMembers.length <= 0) {
                var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
                message += $scope._language == 'en-us' ? '<br /> Please Member Information Enter.' : '<br />कृपया आवेदक का विवरण का चयन करें।';
                ShowOkMessage("", message, function () { });
            }
            else {

                debugger
                $scope.GarnterAadharcardlist.forEach(function (item, index) {

                    if (AADHAARNo == item) {
                        isValid = false;
                    }
                });
                if (!isValid) {

                    var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
                    message += $scope._language == 'en-us' ? '<br /> Please Enter Another Aadhaar Card Number.' : '<br />कृपया आवेदक का विवरण मैं आधार पहले से ही जोड़ा गया कृपया अन्य नंबर दर्ज करें।';
                    ShowOkMessage("", message, function () { });
                }
                else {
                    AadhaarAuth(AADHAARNo, $http, "OTP-BIO", false, function (data) {
                        $scope.LoanGuarantor[index].Name = data.Name;
                        $scope.LoanGuarantor[index].FatherName = data.FName;
                        $scope.LoanGuarantor[index].Age = data.Age;
                        $scope.LoanGuarantor[index].Address = data.Address;
                        $scope.LoanGuarantor[index].MobileNo1 = data.Phone;
                        $scope.LoanGuarantor[index].IsValidate = true;
                        $scope.LoanGuarantor[index].DOB = data.DOB;
                        $scope.$apply();
                    });
                }
            }
        }
    };
    /*------- End Loan  Guarantor code block--------------*/


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


    $scope.ValidateBankDetailsOnSave = function (DateOfOpenAccount) {
        var message = "";//$scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';


        $("#tblLoanAmountDetail").find('input[type="text"]').removeClass("notValid");
        $("#tblLoanAmountDetail").find('select').removeClass("notValid");
        $scope.LoadBankDetails.forEach(function (item, index) {


            if (!checkDateFormat(DateOfOpenAccount)) {

                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Valid Date of Open Account' : 'कृपया खाता खोलने की सही तिथि दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Valid Date of Open Account.' : '<br />कृपया खाता खोलने की सही तिथि दर्ज करें|';
                }
                $("#DateOfOpenAccount").addClass("notValid");
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

    /*------- End Bank Details code block--------------*/



    $scope.FileType = "";
    $scope.FileTypeIndex = 0;
    $scope.ParantIndex = 0;

    $scope.AddFile = function (type, index) {

        $scope.FileType = type;
        var ccc = index;
        $scope.FileTypeIndex = index;


    }

    $scope.AddPropertyFile = function (type, ParantIndex, index) {
        $scope.ParantIndex = ParantIndex;
        $scope.FileType = type;
        var ccc = index;
        $scope.FileTypeIndex = index;
    }


    $scope.ChangeFile = function (id, type, index) {

        if (type == 8) {
            $scope.PropertyDetailsList[index].IsUploadPropertyDocument = false;
            $scope.PropertyDetailsList[index].PropertyDocument = "";
            $scope.PropertyDetailsList[index].Isupload = false;//bhupendra
        }
        else if (type == 9) {
            $scope.GuarantorPropertyDetailsList[index].IsUploadGPropertyDocument = false;
            $scope.GuarantorPropertyDetailsList[index].GPropertyDocument = "";
        }

        if ($scope.LoanSSILoanApplicationForm) {
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
        } else if ($scope.ShowVehicleLoanApplication) {

            if (type == 1) {
                $scope.LoanMembers.forEach(function (item, i) { item.PartnershipCertifiedCopy = ""; item.IsPartnershipCertifiedCopy = false; });
            } else if (type == 2) {
                $scope.LoanMembers.forEach(function (item, i) { item.TransportCompanyRegCertification = ""; item.IsTransportCompanyRegCertification = false; });
            } else if (type == 3) {
                $scope.LoanMembers.forEach(function (item, i) { item.DrivinglicenseCopy = ""; item.IsDrivinglicenseCopy = false; });
            } else if (type == 4) {
                $scope.LoanMembers.forEach(function (item, i) { item.VehicleRantProposalCopy = ""; item.IsVehicleRantProposalCopy = false; });
            } else if (type == 11) {

                $scope.LoanMembers.forEach(function (item, i) { item.QuotationlCopy = ""; item.IsQuotation = false; });

            }
        }
        $("#" + id).val("");
    }

    $scope.ChangePropartyFile = function (id, type, index, parentIndex) {

        if (type == 8) {
            $scope.LoanMembers[parentIndex].PropertyDetailsList[index].IsUploadPropertyDocument = false;
            $scope.LoanMembers[parentIndex].PropertyDetailsList[index].PropertyDocument = "";
            $scope.LoanMembers[parentIndex].PropertyDetailsList[index].Isupload = false;//bhupendra
        }
        else if (type == 9) {
            $scope.GuarantorPropertyDetailsList[index].IsUploadGPropertyDocument = false;
            $scope.GuarantorPropertyDetailsList[index].GPropertyDocument = "";
        }

        if ($scope.LoanSSILoanApplicationForm) {
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
        } else if ($scope.ShowVehicleLoanApplication) {
            if (type == 1) {
                $scope.LoanMembers.forEach(function (item, i) { item.PartnershipCertifiedCopy = ""; item.IsPartnershipCertifiedCopy = false; });
            } else if (type == 2) {
                $scope.LoanMembers.forEach(function (item, i) { item.TransportCompanyRegCertification = ""; item.IsTransportCompanyRegCertification = false; });
            } else if (type == 3) {
                $scope.LoanMembers.forEach(function (item, i) { item.DrivinglicenseCopy = ""; item.IsDrivinglicenseCopy = false; });
            } else if (type == 4) {
                $scope.LoanMembers.forEach(function (item, i) { item.VehicleRantProposalCopy = ""; item.IsVehicleRantProposalCopy = false; });
            } else if (type == 11) {
                $scope.LoanMembers.forEach(function (item, i) { item.QuotationlCopy = ""; item.IsQuotation = false; });

            }
        }

        $("#" + id).val("");
    }


    $scope.UploadPropartyFile = function (id, type, index, parentIndex) {
        $scope.ParantIndex = parentIndex;
        $scope.UploadFile(id, type, index);
    }

    $scope.UploadFile = function (id, type, index) {


        if (type == 11 && index == 0) {
            $scope.Applicaion.IsQuotation = true;
        }


        try {    // FormData
            if (window.FormData !== undefined) {
                var fileUpload = $("#" + id).get(0);
                var files = fileUpload.files;
                //if (files.length <= 0) {
                //    //alert("Please select image to upload");
                //} else {

                //var block = $("#" + type + "_" + id + "_" + indx).attr("src").split(";");
                // Get the content type of the image
                // var contentType = block[0].split(":")[1];// In this case "image/gif"
                // get the real base64 content of the file
                //var realData = block[1].split(",")[1];// In this case "R0lGODlhPQBEAPeoAJosM...."

                // Convert it to a blob to upload
                //var blob = b64toBlob(realData, contentType, 512);

                var formData = new FormData();
                formData.append("pldbfile", files[0]);
                $(".blockUI").show();
                $scope.xhr(SiteUrl + "/pldb/UploadLoanFile", formData, type, index);
            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }

    $scope.UploadFileLVO = function (id, type) {
        try {
            // FormData
            if (window.FormData !== undefined) {
                var fileUpload = $("#" + id).get(0);
                var files = fileUpload.files;
                //var blob = b64toBlob(realData, contentType, 512);
                var formData = new FormData();
                formData.append("pldbfile", files[0]);
                $(".blockUI").show();
                $scope.xhrLVO(SiteUrl + "/pldb/UploadLoanFile", formData, type);
            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }

    $scope.deleteFime = function (index) {


        if ($scope.LOVFilesList[index].Id > 0) {
            $scope.LOVFilesList[index].Is_Delete = true
        } else {
            var list = $scope.LOVFilesList;
            $scope.LOVFilesList = [];
            list.forEach(function (item, indx) {
                if (indx != index) {
                    $scope.LOVFilesList.push(item);
                }
            });
        }
    }

    $scope.ChangeLVOFile = function (type, index) {
        $scope.LOVFilesList[index].IsUploadFileName = false;
        $scope.LOVFilesList[index].FileName = "";
        $("#LovFileName_" + index).val("");
    }

    $scope.xhrLVO = function (url, data, index) {
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

                        $scope.LOVFilesList[index].FileName = request.responseText;

                        if ($scope.LOVFilesList.filter(function (x) { return x.IsUploadFileName === false }).length <= 0) {
                            $scope.LOVFilesList.push({
                                //IsUploadFileName: "",
                                Id: 0,
                                FileName: "",
                                IsUploadFileName: false,
                                Is_Active: true,
                                Is_Delete: false
                            });



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


    $scope.xhr = function (url, data, type, index) {
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


                        if (type == 8) {
                            $scope.LoanMembers[$scope.ParantIndex].PropertyDetailsList[index].PropertyDocument = request.responseText;
                            $scope.LoanMembers[$scope.ParantIndex].PropertyDetailsList[index].Isupload = false;
                        }
                        else if (type == 9) {
                            $scope.GuarantorPropertyDetailsList[index].GPropertyDocument = request.responseText;

                        }

                        if ($scope.LoanSSILoanApplicationForm) {
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
                        }
                        else if ($scope.ShowVehicleLoanApplication) {
                            if (type == 1) {
                                $scope.LoanMembers.forEach(function (item, i) { item.PartnershipCertifiedCopy = request.responseText });
                            } else if (type == 2) {
                                $scope.LoanMembers.forEach(function (item, i) { item.TransportCompanyRegCertification = request.responseText });
                            } else if (type == 3) {
                                $scope.LoanMembers.forEach(function (item, i) { item.DrivinglicenseCopy = request.responseText });
                            } else if (type == 4) {
                                $scope.LoanMembers.forEach(function (item, i) { item.VehicleRantProposalCopy = request.responseText });
                            } else if (type == 11) {
                                $scope.LoanMembers.forEach(function (item, i) { item.QuotationlCopy = request.responseText });
                            }


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

                //if (($(".addprofilePhoto").length >= uploadIndex + 1) || ($(".addFamilyPhoto").length >= uploadIndex + 1)) {
                //    $(".blockUI").hide();
                //    LoadView('_PhotosVideos', 'tb-photosvideos');
                //}
                //++uploadIndex;
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

    $scope.LOVFiles = {
        Id: 0, FileType: "", FileName: "", IsUploadFileName: false, Is_Active: true, Is_Delete: false
    };
    $scope.LOVFilesList = [];
    $scope.LOVFilesList1 = [];
    $scope.LoanStatusList = [];
    $scope.LOVFilesList.push($scope.LOVFiles);

    $scope.addamount = [];
    $scope.CalculateLandValue = function (itmmm, item) {

        // try {



        item.TotalLandValue = (item.LandInBigha * item.Rate).toFixed(2);
        item.TotalLandValueSixty = (item.TotalLandValue * 60 / 100).toFixed(2);

        //item.Rate = item.Rate;







        //    if (item.ChahiVi.indexOf('/') > -1) {
        //        item.BaraniBi = ((item.TotalArea * item.ChahiVi.split('/')[0]) / item.ChahiVi.split('/')[1]).toFixed(2);
        //        item.BaraniVi = item.BaraniBi < 1 ? 'सीमांत कृषक' : item.BaraniBi > 1 && item.BaraniBi < 2 ? 'लघु कृषक' : 'अन्य किसान';



        itmmm.TTotalLandValue = 0;
        itmmm.TTotalLandValueSixty = 0;
        itmmm.PropertyDetailsList.forEach(function (itm, indx) {

            itmmm.TTotalLandValue += parseFloat(itm.TotalLandValue == undefined ? 0 : itm.TotalLandValue);
            //PropertyTotalHissa += item.TotalHissaValue;
            itmmm.TTotalLandValueSixty += parseFloat(itm.TotalLandValueSixty == undefined ? 0 : itm.TotalLandValueSixty);

        });


        $scope.LVOTTotalLandValue = 0;
        $scope.LVOTTotalLandValueSixty = 0;

        $scope.LoanMembers.forEach(function (itmm1, index) {

            itmm1.PropertyDetailsList.forEach(function (itm, indx) {

                //itmmm.TTotalLandValue += parseFloat(itm.TotalLandValue == undefined ? 0 : itm.TotalLandValue);
                //PropertyTotalHissa += item.TotalHissaValue;
                //itmmm.TTotalLandValueSixty += parseFloat(itm.TotalLandValueSixty == undefined ? 0 : itm.TotalLandValueSixty);
                $scope.LVOTTotalLandValue += (parseFloat(itm.TotalLandValue == undefined ? 0 : itm.TotalLandValue));
                $scope.LVOTTotalLandValueSixty += (parseFloat(itm.TotalLandValueSixty == undefined ? 0 : itm.TotalLandValueSixty));

            });
            //  $scope.land1 += += (parseFloat(itm.TTotalLandValue == undefined ? 0 : itm.TTotalLandValue));
        })

        $scope.MemberRegistration.Total75 = $scope.LVOTTotalLandValueSixty - (($scope.MemberRegistration.PureIncome == undefined || $scope.MemberRegistration.PureIncome == NaN ? 0 : $scope.MemberRegistration.PureIncome) * 75 / 100);

        //$scope.addamount.forEach(function (itm, index) {

        //});

        //
        //$scope.Landvaluedetails.forEach(function (itm, index) {
        //    $scope.Landvaluedetails1 += (itm.Rate);
        //})

        //        itmmm.TtotalHissaVarg = itmmm.PropertyTotalHissaValue < 1 ? 'सीमांत कृषक' : itmmm.PropertyTotalHissaValue > 1 && itmmm.PropertyTotalHissaValue < 2 ? 'लघु कृषक' : 'अन्य किसान';


        //    } else {
        //        item.BaraniBi = (item.TotalArea * item.ChahiVi.split('/')[0]);
        //        item.BaraniVi = item.BaraniBi < 1 ? 'सीमांत कृषक' : item.BaraniBi > 1 && item.BaraniBi < 2 ? 'लघु कृषक' : 'अन्य किसान';
        //        itmmm.PropertyTotalArea = 0;
        //        itmmm.PropertyTotalHissaValue = 0;

        //        itmmm.PropertyDetailsList.forEach(function (itm, indx) {
        //            itmmm.PropertyTotalArea += parseFloat(itm.TotalArea);
        //            //PropertyTotalHissa += item.TotalHissaValue;
        //            itmmm.PropertyTotalHissaValue += parseFloat(itm.BaraniBi);
        //        });
        //        itmmm.TtotalHissaVarg = itmmm.PropertyTotalHissaValue < 1 ? 'सीमांत कृषक' : itmmm.PropertyTotalHissaValue > 1 && itmmm.PropertyTotalHissaValue < 2 ? 'लघु कृषक' : 'अन्य किसान';

        //    }
        //} catch (ex) {
        //    item.TotalHissaValue = "";
        //    item.HissaVarg = "";
        //}
    }



    $scope.CalculateLandValue1 = function (itmmm, item) {

        // try {



        item.TotalLandValue = (item.LandInBigha * item.land_amount_value).toFixed(2);
        item.TotalLandValueSixty = (item.TotalLandValue * 60 / 100).toFixed(2);

        //item.Rate = item.Rate;







        //    if (item.ChahiVi.indexOf('/') > -1) {
        //        item.BaraniBi = ((item.TotalArea * item.ChahiVi.split('/')[0]) / item.ChahiVi.split('/')[1]).toFixed(2);
        //        item.BaraniVi = item.BaraniBi < 1 ? 'सीमांत कृषक' : item.BaraniBi > 1 && item.BaraniBi < 2 ? 'लघु कृषक' : 'अन्य किसान';



        itmmm.TTotalLandValue = 0;
        itmmm.TTotalLandValueSixty = 0;
        itmmm.PropertyDetailsList.forEach(function (itm, indx) {

            itmmm.TTotalLandValue += parseFloat(itm.TotalLandValue == undefined ? 0 : itm.TotalLandValue);
            //PropertyTotalHissa += item.TotalHissaValue;
            itmmm.TTotalLandValueSixty += parseFloat(itm.TotalLandValueSixty == undefined ? 0 : itm.TotalLandValueSixty);


        });
        //bhupendra add

        $scope.LVOTTotalLandValue = 0;
        $scope.LVOTTotalLandValueSixty = 0;

        $scope.LoanMembers.forEach(function (itm, index) {

            $scope.LVOTTotalLandValue += (parseFloat(itm.TTotalLandValue == undefined || itm.TTotalLandValue == NaN ? 0 : itm.TTotalLandValue));
            $scope.LVOTTotalLandValueSixty += (parseFloat(itm.TTotalLandValueSixty == undefined || itm.TTotalLandValueSixty == NaN ? 0 : itm.TTotalLandValueSixty));

            //  $scope.land1 += += (parseFloat(itm.TTotalLandValue == undefined ? 0 : itm.TTotalLandValue));
        })



        //$scope.addamount.forEach(function (itm, index) {

        //});

        //
        //$scope.Landvaluedetails.forEach(function (itm, index) {
        //    $scope.Landvaluedetails1 += (itm.Rate);
        //})

        //        itmmm.TtotalHissaVarg = itmmm.PropertyTotalHissaValue < 1 ? 'सीमांत कृषक' : itmmm.PropertyTotalHissaValue > 1 && itmmm.PropertyTotalHissaValue < 2 ? 'लघु कृषक' : 'अन्य किसान';


        //    } else {
        //        item.BaraniBi = (item.TotalArea * item.ChahiVi.split('/')[0]);
        //        item.BaraniVi = item.BaraniBi < 1 ? 'सीमांत कृषक' : item.BaraniBi > 1 && item.BaraniBi < 2 ? 'लघु कृषक' : 'अन्य किसान';
        //        itmmm.PropertyTotalArea = 0;
        //        itmmm.PropertyTotalHissaValue = 0;

        //        itmmm.PropertyDetailsList.forEach(function (itm, indx) {
        //            itmmm.PropertyTotalArea += parseFloat(itm.TotalArea);
        //            //PropertyTotalHissa += item.TotalHissaValue;
        //            itmmm.PropertyTotalHissaValue += parseFloat(itm.BaraniBi);
        //        });
        //        itmmm.TtotalHissaVarg = itmmm.PropertyTotalHissaValue < 1 ? 'सीमांत कृषक' : itmmm.PropertyTotalHissaValue > 1 && itmmm.PropertyTotalHissaValue < 2 ? 'लघु कृषक' : 'अन्य किसान';

        //    }
        //} catch (ex) {
        //    item.TotalHissaValue = "";
        //    item.HissaVarg = "";
        //}
    }




    $scope.updateaddress = function () {

        $scope.PLDB.TempAddress.IsRuralcopy = $scope.PLDB.PermanentAddress.IsRural;




        $scope.RuralUrbanBlockCopy();
        if ($scope.Pldb.IsSameCEOAddress == false || $scope.Pldb.IsSameCEOAddress == false) {
            $scope.allDisable = false;
            $scope.Pldb.HouseBuildingNocopy = null;
            $scope.Pldb.LaneStreetNamecopy = null;
            $scope.Pldb.Localitycopy = null;
            $scope.Pldb.STATE_IDcopy = '6';
            $scope.Pldb.GP_ID_copy = '0';
            $scope.Pldb.VILLAGE_IDcopy = '0';
            $scope.Pldb.CITY_IDcopy = '0';
            $scope.Pldb.WARD_IDcopy = '0';
            $scope.Pldb.PinCodecopy = '0';
            $scope.Pldb.AreaOfOperationcopy = null;
            $scope.Pldb.CityTypes_copy = '0';
            $scope.Pldb.DIVISION_ID_copy = '0';
            $scope.Pldb.DISTRICT_ID_copy = '0';
            $scope.Pldb.BLOCK_ID_copy = '0';
        }
        else if ($scope.Pldb.IsSameCEOAddress == true || $scope.Pldb.IsSameCEOAddress == true) {
            $scope.allDisable = true;
            $scope.Pldb.HouseBuildingNocopy = $scope.Pldb.HouseBuildingNo;
            $scope.Pldb.LaneStreetNamecopy = $scope.Pldb.LaneStreetName;
            $scope.Pldb.Localitycopy = $scope.Pldb.Locality;
            $scope.Pldb.STATE_IDcopy = '6';
            $scope.Pldb.SocietyIdcopy = $scope.Pldb.SocietyId;
            $scope.GramPanchayatscopy = $scope.GramPanchayats;
            $scope.Pldb.GP_IDcopy = $scope.Pldb.GP_ID;
            $scope.Villagescopy = $scope.Villages;
            $scope.Pldb.VILLAGE_IDcopy = $scope.Pldb.VILLAGE_ID;
            $scope.CityListdatacopy = $scope.CityListdata;
            $scope.Pldb.CITY_IDcopy = $scope.Pldb.CITY_ID;
            $scope.Wardcopy = $scope.Ward;
            $scope.Pldb.WARD_IDcopy = $scope.Pldb.WARD_ID;
            $scope.Pldb.PinCodecopy = $scope.Pldb.PinCode;
            $scope.Pldb.AreaOfOperationcopy = $scope.Pldb.AreaOfOperation;
            $scope.Pldb.CityTypes_copy = '0';
            $scope.Divisionscopy = $scope.Divisions;
            $scope.Pldb.DIVISION_ID_copy = $scope.Pldb.DIVISION_ID;
            $scope.DistrictListcopy = $scope.DistrictList;
            $scope.Pldb.DISTRICT_ID_copy = $scope.Pldb.DISTRICT_ID;
            $scope.Blocksdatacopy = $scope.Blocksdata;
            $scope.Pldb.BLOCK_ID_copy = $scope.Pldb.BLOCK_ID;
            if ($scope.Pldb.IsRuralAdd == true) {
                $scope.Pldb.IsRuralAddcopy = true;
                $scope.RuralUrbanBlock();
                $scope.RuralUrbanBlockCopy();
            }
            if ($scope.Pldb.IsRuralAdd == false) {
                $scope.Pldb.IsRuralAddcopy = false;
                $scope.RuralUrbanBlock();
                $scope.RuralUrbanBlockCopy();
            }
        }

    };

    //START : Get State List
    $scope.Statelist = [];
    $scope.Statelist.push(
        {
            "STATE_ID": 6,
            "STATE_ENG": 'Rajasthan',
            "STATE_MANGAL": 'राजस्थान'

        });

    //END : Get State List

    //START : Get Division List
    $scope.Divisions = [{}];
    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
        success(function (data, status) {
            $scope.Divisions = data;
        }).
        error(function (data, status) {
            $scope.Divisions = [];
        });
    //END

    //start: Get District List
    $scope.GetDistrictList = function (id) {
        if ($scope.PLDB.PermanentAddress.DIVISION_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.PLDB.PermanentAddress.DIVISION_ID).
                success(function (data, status) {              
                    $scope.DistrictList = data;                    
                }).
                error(function (data, status) {
                    $scope.DistrictList = [];
                });
        }
        else if ($scope.PLDB.TempAddress.DIVISION_ID_copy != '0' && id == 2) {

            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.PLDB.TempAddress.DIVISION_ID_copy).
                success(function (data, status) {
                    $scope.DistrictListcopy = data;

                }).
                error(function (data, status) {
                    $scope.DistrictListcopy = [];
                });

        }
    };

    //End: Get District List

    //start: Get RuralUrban List
    $scope.RuralUrbanBlock = function (V) {
        var SelectedVal = $scope.PLDB.PermanentAddress.IsRural == 'false' ? false : true;
        if (V != 1) {
            $scope.PLDB.PermanentAddress.CityTypes = "0";
            $scope.PLDB.PermanentAddress.CITY_ID = "0";
            $scope.PLDB.PermanentAddress.WARD_ID = "0";
            $scope.PLDB.PermanentAddress.BLOCK_ID = "0";
            $scope.PLDB.PermanentAddress.GP_ID = "0";
            $scope.PLDB.PermanentAddress.VILLAGE_ID = "0";
        }
        if (SelectedVal == true || SelectedVal == true) {
            $scope.PLDB.PermanentAddress.BLOCK_ID = "0";
            $scope.PLDB.PermanentAddress.GP_ID = "0";
            $scope.PLDB.PermanentAddress.VILLAGE_ID = "0";
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;
            $scope.GetCityListByCityTypes(1);
        }
        else if (SelectedVal == false || SelectedVal == false) {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
            $scope.BlockListByOffAdd(1)
        }
        else {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
        }
    };

    $scope.RuralUrbanBlockCopy = function () {
        var SelectedVal = $scope.PLDB.TempAddress.IsRuralcopy == 'false' ? false : true;
        $scope.PLDB.TempAddress.CityTypes_copy = "0";
        $scope.PLDB.TempAddress.CITY_IDcopy = "0";
        $scope.PLDB.TempAddress.WARD_IDcopy = "0";
        $scope.PLDB.TempAddress.BLOCK_ID_copy = "0";
        $scope.PLDB.TempAddress.GP_IDcopy = "0";
        $scope.PLDB.TempAddress.VILLAGE_IDcopy = "0";
        if (SelectedVal == true || SelectedVal == true) {
            $scope.UrbanDivCopy = true;
            $scope.RuralDivcopy = false;
            //$scope.GetCityListByCityTypes(2);
        }
        else if (SelectedVal == false || SelectedVal == false) {
            $scope.UrbanDivCopy = false;
            $scope.RuralDivcopy = true;
            //$scope.BlockListByOffAdd(2)
        }
        else {
            $scope.UrbanDivCopy = false;
            $scope.RuralDivcopy = false;
        }
    };
    //End: Get RuralUrban List

    // START : Set Rural/ Urban div show/hide
    $scope.GetCityListByCityTypes = function (id) {
        if ($scope.PLDB.PermanentAddress.DISTRICT_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.PLDB.PermanentAddress.DISTRICT_ID).
                success(function (data, status) {
                    $scope.CityListdata = data;
                }).
                error(function (data, status) {
                    $scope.CityListdata = [];
                });
        }

        if ($scope.PLDB.TempAddress.DISTRICT_ID_copy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.PLDB.TempAddress.DISTRICT_ID_copy).
                success(function (data, status) {
                    $scope.CityListdatacopy = data;
                }).
                error(function (data, status) {
                    $scope.DistrictListcopy = [];
                });
        }
    };
    //start BlockListByOffAddress form
    $scope.BlockListByOffAdd = function (id) {
        if ($scope.PLDB.PermanentAddress.DISTRICT_ID != '0' && id == 1) {
            $scope.Blocksdata = [];
            $scope.GramPanchayats = [];
            $scope.Villages = [];
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.PLDB.PermanentAddress.DISTRICT_ID).
                success(function (data, status) {
                    $scope.Blocksdata = data;
                }).
                error(function (data, status) {
                    $scope.Blocksdata = [];
                });
        }
        else if ($scope.PLDB.TempAddress.DISTRICT_ID_copy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.PLDB.TempAddress.DISTRICT_ID_copy).
                success(function (data, status) {
                    $scope.Blocksdatacopy = data;
                }).
                error(function (data, status) {
                    $scope.Blocksdatacopy = [];
                });
        }
    };
    //END

    // START : Get List of GramPanchayat
    $scope.GramPanchayatList = function (id) {

        if ($scope.PLDB.PermanentAddress.BLOCK_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.PLDB.PermanentAddress.BLOCK_ID).
                success(function (data, status) {

                    $scope.GramPanchayats = data;
                }).
                error(function (data, status) {
                    $scope.GramPanchayats = [];

                });
        }
        else if ($scope.PLDB.TempAddress.BLOCK_ID_copy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.PLDB.TempAddress.BLOCK_ID_copy).
                success(function (data, status) {

                    $scope.GramPanchayatscopy = data;
                }).
                error(function (data, status) {
                    $scope.GramPanchayatscopy = [];

                });
        }
    };
    //End : Get List of GramPanchayat

    // START : Get Ward List
    $scope.GetWardList = function (id) {
        if ($scope.PLDB.PermanentAddress.CITY_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.PLDB.PermanentAddress.CITY_ID).
                success(function (data, status) {
                    $scope.Ward = data;
                }).
                error(function (data, status) {
                    $scope.Ward = [];
                });
        }
        else if ($scope.PLDB.TempAddress.CITY_IDcopy != '0' && id == 2 && $scope.PLDB.TempAddress.CITY_IDcopy != undefined) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.PLDB.TempAddress.CITY_IDcopy).
                success(function (data, status) {
                    $scope.Wardcopy = data;
                }).
                error(function (data, status) {
                    $scope.Wardcopy = [];
                });
        }
    };

    //End
    //Start: Get Village List
    $scope.VillageList = function (id) {
        if ($scope.PLDB.PermanentAddress.GP_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.PLDB.PermanentAddress.GP_ID).
                success(function (data, status) {
                    $scope.Villages = data;
                }).
                error(function (data, status) {
                    $scope.Villages = [];
                });
        }
        else if ($scope.PLDB.TempAddress.GP_IDcopy != '0' && id == 2) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.PLDB.TempAddress.GP_IDcopy).
                success(function (data, status) {
                    $scope.Villagescopy = data;

                }).
                error(function (data, status) {
                    $scope.Villagescopy = [];
                });
        }
    };
    //End
    // START : Get Relation List
    $scope.GetBloodRelationList = function () {
        $scope.BloodRelation = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=RELATION&MasterCode=-1").
            success(function (data, status) {
                $scope.BloodRelation = data;
            }).
            error(function (data, status) {
                $scope.BloodRelation = [];
            });
    }
    // END: Get Relation List


    // START : Get Econiomic  List
  
    $scope.GetQualificationList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=EDUCATIONQUALIFICATION&MasterCode=-1").
            success(function (data, status) {
                $scope.EducationQualification = [];
                $scope.EducationQualification = data;                
            }).
            error(function (data, status) {
                $scope.EducationQualification = [];
            });

    }
    // END: Get Econiomic List  
    $scope.OdTotal = function () {
        $scope.Od.ODTotal = (parseFloat($scope.Od.InsurancePremium) || 0) + (parseFloat($scope.Od.ODInterest) || 0) + (parseFloat($scope.Od.ODPrincipal) || 0);
        $scope.Od.ODTotal = $scope.Od.ODTotal.toFixed(2);
    }
    $scope.Od31Total = function () {
        $scope.Od31.OutODTotal = (parseFloat($scope.Od31.OutInsurancePremium) || 0) + (parseFloat($scope.Od31.OutODInterest) || 0) + (parseFloat($scope.Od31.OutODPrincipal) || 0);
        $scope.Od31.OutODTotal = $scope.Od31.ODTotal.toFixed(2);
    }

    $scope.CdTotal = function () {

        $scope.CD.TotalInterest = (parseFloat($scope.CD.Interest) || 0) + (parseFloat($scope.CD.CDPenalInterest) || 0);
        $scope.CD.TotalInterest = $scope.CD.TotalInterest.toFixed(2);

        $scope.CD.CDTotal = (parseFloat($scope.CD.Principal) || 0) + (parseFloat($scope.CD.Interest) || 0) + (parseFloat($scope.CD.CDPenalInterest) || 0);
        $scope.CD.CDTotal = $scope.CD.CDTotal.toFixed(2);

    }
    $scope.UaTotal = function () {

        $scope.UA.TotalOutstanding = (parseFloat($scope.UA.PrincipalOutstanding) || 0) + (parseFloat($scope.UA.TotalInterest) || 0);
        $scope.UA.TotalOutstanding = $scope.UA.TotalOutstanding.toFixed(2);

        $scope.UA.TotalRecoverableAmount = (parseFloat($scope.UA.InterestUpto30062024) || 0) + (parseFloat($scope.UA.InterestUpto31032025) || 0) + (parseFloat($scope.UA.PrincipalOutstanding) || 0) + (parseFloat($scope.UA.TotalInterest) || 0);
        $scope.UA.TotalRecoverableAmount = $scope.UA.TotalRecoverableAmount.toFixed(2);

    }

    $scope.GetBloodRelationList();
    $scope.GetQualificationList();
    

    $scope.Save_PLDBForm = function (StatusId) {
        //if (StatusId == 1) {
        //    if (!$scope.PartialValidateForm())
        //        return;
        //}
        //else {
        //    if (!$scope.ValidateForm())
        //        return;
        //}

        //$scope.PLDB_Data = {
        //    ApplicantDetails: [{}],
        //    BankDetails: [{}],
        //    LoanDetails: [{}],
        //    PermanentAddress: [{}],
        //    TempAddress: [{}]
        //}
       
        //$scope.PLDB_Data.ApplicantDetails = $scope.PLDB_Data.ApplicantDetails;
        //$scope.PLDB_Data.BankDetails = $scope.PLDB.BankDetails;
        //$scope.PLDB_Data.ApplicantDetails = $scope.PLDB_Data.ApplicantDetails;
        //$scope.PLDB_Data.ApplicantDetails = $scope.PLDB_Data.ApplicantDetails;
        //$scope.PLDB_Data.ApplicantDetails = $scope.PLDB_Data.ApplicantDetails;


        //var encryptedAadhaarNo = "";
        //if ($scope.GopalCreditCardModel.AadharNo != null && $scope.GopalCreditCardModel.AadharNo != "")
        //    encryptedAadhaarNo = encryptData($scope.GopalCreditCardModel.AadharNo);       
        //$scope.GopalCreditCardModel.AadharNo = encryptedAadhaarNo;

      //  $scope.PLDB.Address = $scope.PLDB.PermanentAddress;

        console.log($scope.PLDB);


        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/PLDB/Save_PldbApplicationForm", $scope.PLDB
            //{
            //    dataType: false,
            //    contentType: false,
            //    processData: false,
            //}
        ).
            success(function (data, status) {

                $(".se-pre-con").fadeOut("slow");
                if (data.Status == true) {
                    //$scope.Reset_GCCForm();
                    // ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', 'Gopal Credit Card Registration Data Save Sucessfully');
                    alert($scope._language == 'en-us' ? 'Gopal Credit Card Registration Data Save Sucessfully' : 'गोपाल क्रेडिट कार्ड पंजीकरण डेटा सफलतापूर्वक सहेजा गया।');
                    //if (StatusId == 2)
                    //    window.location = '../GopalCreditCard/GopalApplicationFormPart2?gccEncId=' + data.Content;
                    //else
                    window.location.reload();
                }
                else if (data.Status == false) {
                    // $scope.Reset_GCCForm();
                    if (data.ErrorMessage == "GCC_APPLICATIONALREADYEXIST") {
                        ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', $scope._language == 'en-us' ? 'Application for Gopal Credit Card Scheme has Already been Made with Janadhaar Number ' + $scope.GopalCreditCardModel.JanAadharNo + '. Hence You are not Eligible to Apply for Gopal Credit Card Scheme.' : 'जनाधार नंबर ' + $scope.GopalCreditCardModel.JanAadharNo + ' से पहले ही गोपाल क्रेडिट कार्ड योजना के लिए आवेदन किया जा चुका है। अतः आप गोपाल क्रेडिट कार्ड योजना मैं आवेदन के लिए योग्य नहीं हैं।');
                    }
                    if (data.ErrorMessage == "GCC_IsRural_False") {
                        ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', $scope._language == 'en-us' ? 'Residents of urban areas are not eligible to apply for Gopal Credit Card.' : 'शहरी क्षेत्रों के निवासी गोपाल क्रेडिट कार्ड के लिए आवेदन करने के पात्र नहीं हैं|');
                    }
                    //if (data.ErrorMessage == "GCC_DoesTheMilkSaleToDCS_False") {
                    //    ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', $scope._language == 'en-us' ? 'In case the cow keeper does not sell the milk to the dairy cooperative society, they are not eligible to apply for Gopal Credit Card.' : 'गौपालक द्वारा दूध का बेचान डेयरी सहकारी समिति को नही किये जाने की स्थिति में, गोपाल क्रेडिट कार्ड में आवेदन करने के लिए पात्र नहीं हैं।');
                    //}
                    if (data.ErrorMessage == "GCC_IsPACSBankAccountAvailable_False") {
                        ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', $scope._language == 'en-us' ? 'Please Ensure to Open an Account in PACS Bank before Applying for Gopal Credit Card Scheme.' : 'कृपया गोपाल क्रेडिट कार्ड योजना में आवेदन करने से पूर्व पैक्स बैंक में खाता खुलवाना सुनिश्चियत करें।');
                    }
                    //if (data.ErrorMessage == "GCC_IsRecommendationOfDCSUploaded_False") {
                    //    ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', $scope._language == 'en-us' ? 'It is Mandatory to Upload the Recommendation of the Dairy Co-Operative Society.' : 'डेयरी सहकारी समिति की अनुशंषा अपलोड करना अनिवार्य है।');
                    //}
                    //window.location = '../GopalCreditCard/GopalApplicationForm';
                }
                else if (data == -1) {
                    // $scope.Reset_GCCForm();
                    ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', 'Registered Detail Already Exists');
                }
                else {
                    // $scope.Reset_GCCForm();
                    ShowModelAlert($scope._language == 'en-us' ? 'Gopal Credit Card Registration :: ' : 'गोपाल क्रेडिट कार्ड पंजीकरण ::', 'An Error Occurred Please Try Again Later');
                }

            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                // $$scope.Reset_GCCForm();
            });

    }




});


CommityRegistrtionModule.directive('validfile', function validFile() {
    var validFormats = ['jpg', 'gif'];
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$validators.validFile = function () {
                elem.on('change', function () {
                    var id = elem.attr("id") == undefined ? "" : elem.attr("id");
                    //  alert(id.split('_')[0]);
                    if (id.split('_')[0] == "LovFileName") {
                        if (elem[0].files.length > 0) {
                            const fsize = elem[0].files[0].size;
                            const file = Math.round((fsize / 1024));

                            var indx = id.split()[1];
                            if (file >= 4096) {
                                ShowOkMessage("Warning", scope._language == 'en-us' ? "File too Big, please select a file less than 4mb" : "फ़ाइल बहुत बड़ी है, कृपया 4mb से कम फ़ाइल का चयन करें", function () { });
                                elem[0].value = "";
                            } else {
                                //if (scope.FileType == 1) {
                                //alert(id.split('_')[1]);
                                scope.LOVFilesList[id.split('_')[1]].IsUploadFileName = true;
                                scope.LOVFilesList[id.split('_')[1]].FileName = "";
                                //}
                                scope.$apply();
                            }
                        }
                    } else {
                        if (elem[0].files.length > 0) {
                            const fsize = elem[0].files[0].size;
                            const file = Math.round((fsize / 1024));
                            // The size of the file. 
                            //alert(1);
                            if (file >= 4096) {
                                ShowOkMessage("Warning", scope_language == 'en-us' ? "File too Big, please select a file less than 4mb" : "फ़ाइल बहुत बड़ी है, कृपया 4mb से कम फ़ाइल का चयन करें", function () { });
                                elem[0].value = "";
                            } else {


                                if (scope.FileType == 8) {
                                    scope.LoanMembers[scope.ParantIndex].PropertyDetailsList[scope.FileTypeIndex].IsUploadPropertyDocument = true;
                                    scope.LoanMembers[scope.ParantIndex].PropertyDetailsList[scope.FileTypeIndex].PropertyDocument = "";
                                    scope.LoanMembers[scope.ParantIndex].PropertyDetailsList[scope.FileTypeIndex].Isupload = true;//bhupendra
                                }
                                else if (scope.FileType == 9) {
                                    scope.GuarantorPropertyDetailsList[scope.FileTypeIndex].IsUploadGPropertyDocument = true;
                                    scope.GuarantorPropertyDetailsList[scope.FileTypeIndex].GPropertyDocument = "";
                                }

                                if (scope.LoanSSILoanApplicationForm) {
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
                                } else if (scope.ShowVehicleLoanApplication) {

                                    if (scope.FileType == 1) {
                                        scope.LoanMembers.forEach(function (item, i) { item.PartnershipCertifiedCopy = ""; item.IsPartnershipCertifiedCopy = true });
                                    } else if (scope.FileType == 2) {
                                        scope.LoanMembers.forEach(function (item, i) { item.TransportCompanyRegCertification = ""; item.IsTransportCompanyRegCertification = true });
                                    } else if (scope.FileType == 3) {
                                        scope.LoanMembers.forEach(function (item, i) { item.DrivinglicenseCopy = ""; item.IsDrivinglicenseCopy = true });
                                    } else if (scope.FileType == 4) {
                                        scope.LoanMembers.forEach(function (item, i) { item.VehicleRantProposalCopy = ""; item.IsVehicleRantProposalCopy = true });
                                    }
                                    else if (scope.FileType == 11) {
                                        scope.LoanMembers.forEach(function (item, i) { item.QuotationlCopy = ""; item.IsQuotation = true });
                                    }


                                }
                                scope.$apply();
                            }
                        }
                    }
                });
            };
        }
    };
});
