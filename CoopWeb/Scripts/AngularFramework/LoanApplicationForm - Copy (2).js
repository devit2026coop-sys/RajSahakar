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

    $scope.ShowApplicanDetails = false;
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
        CategoryId: "",
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
        ApplicantAadharNumber: ""
    }



    $scope.LoanVehicle = {
        VehicleId: "0",
        VehicleType: "",
        Model: "",
        Capacity: "",
        AverageFuelConsumption: "",
        VehicleVendorName: "",
        ChassisPrice: "",
        BodyPrice: "",
        OtherExpenses: "",
        ApplicantOwnContribution: "",
        CommutedLoanAmount: "",
        AreaVehicleDriven: "",
        StatePermitNo: "",
        StatePermitValidity: "",
        MonthlyIncomeByVehicle: "",
        ItemsBroughtByVehicle: "",
        IncomeBasis: "",
        VehicleRunDaysInMonth: "",
        VehicleRunPerDay: "",
        ProposedBuildingAndWorkStation: "",
        FarePerKM: "",
        WorkingCapitalFileFile: "",
        ApplicantDealAfterApprovalFile: "",
        EstimatedMonthlyEarnings: "",
        OtherIncomeByVehicle: "",
        MonthlyExpenditureFuel: "",
        DriverSalaryAllowance: "",
        RoadAndOtherTaxes: "",
        RepairMaintenanceService: "",
        InsurancePremiums: "",
        OilSpareParts: "",
        GarageRent: "",
        WearAndTear: "",
        OtherMiscellaneousExpenses: "",
        InterestOnLoan: ""
    };

    $scope.LoanAmount = {
        Id: "0",
        PurposeOfLoan: "0",
        EstimatedTotalAmount: "",
        LoanAmountApplied: "",
        IsDelete: false,
        SchemeId: "0",
        SectorId: "0",
        LoanType: "0"
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
            LoanSchemeList: []
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
            ApplicantAadharNumber: ""
        }
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
        TotalArea: "",
        Address: "",
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
            IsDelete: false
        }
    }
    $scope.PropertyDetailsList = [];
    $scope.PropertyDetailsList.push($scope.PropertyDetails);

    //START : Show Hide User Registration Form
    $scope.ShowhideUserRegistrtionForm = function () {
        if ($scope.MemberRegistration.OrganizationId === '0') {
            $scope.divUserRegistration = false;
        }
        else {
            $scope.divUserRegistration = true;
        }
        $scope.ClearUserRegistrtionForm();
    };
    //START : Show Hide User Registration Form

    //START : Clear User registration Form
    $scope.ClearUserRegistrtionForm = function () {
        $scope.MemberRegistration.UserId = '0';
        $scope.MemberRegistration.OrganizationId = '0';
        $scope.MemberRegistration.Name = '';
        $scope.MemberRegistration.Age = '';
        $scope.MemberRegistration.Address = '';
        $scope.MemberRegistration.Gender = '';
        $scope.MemberRegistration.AADHARNo = '';
        $scope.MemberRegistration.Business = '';
        $scope.MemberRegistration.MobileNo = '';
        $scope.MemberRegistration.EmailId = '';
        $scope.MemberRegistration.AnimalCount = '';
        $scope.MemberRegistration.BuyCount = '';
        $scope.MemberRegistration.DepositedAmount = '';
        $scope.MemberRegistration.Disability = '';

    };
    //START : Clear User registration Form

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

        $scope.GetOrganizationList();
        $scope.GetDistrictWithoutDivisionList();
    };

    $scope.GetSocietyMemberData = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.SocietyMemberData = [];
        $http.get(SiteUrl + "/SocietyMemberRegistration/GetOrganizationUserRegistrationDetails?ApplicationIdEnc=" + "").
            success(function (data, status) {
                $scope.SocietyMemberData = data;
            }).
            error(function (data, status) {
                $scope.SocietyMemberData = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // START : Get All Organization List
    $scope.GetOrganizationList = function () {
        $scope.OrganizationList = [];
        $http.post(SiteUrl + "/SocietyMemberRegistration/GetOrganizationList").
            success(function (data, status) {
                $scope.OrganizationList = data;
            }).
            error(function (data, status) {
                $scope.OrganizationList = [];
            });
    };
    // START : Get All Organization List
    //Start :  Get List of GramPanchayat

    $scope.GetGramPanchayatData = function () {
        $scope.Member.Tehsil = $scope.MemberRegistration.Branch_ID;
        $scope.GramPanchayats = [];
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.MemberRegistration.Branch_ID).
            success(function (data, status) {

                $scope.GramPanchayats = data;
                $scope.Member.GramPanchayats = data;

            }).
            error(function (data, status) {
                $scope.GramPanchayats = [];

            });
    }

    $scope.GetGramPanchayat = function (index) {
        //$scope.Member.Tehsil = $scope.MemberRegistration.Branch_ID;
        //$scope.GramPanchayats = [];

        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.LoanMembers[index].Tehsil).
            success(function (data, status) {
                $scope.LoanMembers[index].GramPanchayats = [];
                $scope.LoanMembers[index].GramPanchayats = data;

            }).
            error(function (data, status) {
                //$scope.GramPanchayats = [];

            });
    }

    $scope.GetVillages = function (index, id) {
        $(".se-pre-con").fadeIn("slow");
        $scope.LoanMembers[index].Village = "0";
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=VILLAGE&MasterCode=" + $scope.LoanMembers[index].VillagePanchayat).
            success(function (data, status) {
                $scope.LoanMembers[index].VillageList = [];
                $scope.LoanMembers[index].VillageList = data;
                $scope.LoanMembers[index].Village = id
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                //$scope.GramPanchayats = [];
                $(".se-pre-con").fadeOut("slow");
            });
    }

    //End
    // Get List of Block
    $scope.BlockList = function () {
        $scope.Blocks = [];
        $scope.GramPanchayats = [];
        $scope.SocietyList = [];
        $scope.MemberDatail = false;
        if ($scope.MemberRegistration.DISTRICT_ID != '0') {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.MemberRegistration.DISTRICT_ID).
                success(function (data, status) {
                    $scope.Blocks = data;
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                });
        }
        $scope.MemberRegistration.BLOCK_ID = '0';
        $scope.GramPanchayats = [];
        $scope.MemberRegistration.GP_ID = '0';
        $scope.SocietyList = [];
        $scope.MemberRegistration.OrganizationId = '0';
    };
    //end block
    //START : Get District List
    $scope.GetDistrictWithoutDivisionList = function () {
        $scope.Blocks = [];
        $scope.GramPanchayats = [];
        $scope.DistrictList = [];
        $scope.SocietyList = [];
        $scope.MemberDatail = false;
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    // END
    //Start :  Get List of Society
    $scope.GetSocietyByGP_ID = function () {
        $scope.MemberDatail = false;
        $scope.SocietyList = [];
        $http.get(SiteUrl + "/Citizen/GetSocietyByGP_ID/?GP_ID=" + $scope.MemberRegistration.GP_ID + "&SocietyCatyegoryId=" + $scope.MemberRegistration.RegistrationType).
            success(function (data, status) {
                $scope.SocietyList = data;
            }).
            error(function (data, status) {
                $scope.SocietyList = [];

            });

        $scope.MemberRegistration.OrganizationId = '0';
    };
    //End
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
        //$scope.BackBtnLast = false;
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

    $scope.PLDB = [];
    $scope.ViewMemberApplication = function (ReqIdEnc) {
        // alert(ReqIdEnc);
        window.location = '../Citizen/MemberApplicationView?ApplicationIdEnc=' + ReqIdEnc;
    };
    $scope.PLDBList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=-1").
            success(function (data, status) {
                $scope.PLDB = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDB = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
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
                IsDelete: itm.IsDelete
            });
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
        var applicaionDetails = {
            load_id: $scope.LoanApplicationId, pldb_id: $scope.MemberRegistration.PLDB_ID, branch_id: $scope.MemberRegistration.Branch_ID,
            Members: members,
            LoanAmount: LoanAmount,
            LoanProperty: LoanProperty,
            PreLoan: LoadPreLoanDetails,
            IsSubmit: IsSubmit,
            GuarantorDetails: $scope.guarantor,
            VehicleDetails: $scope.vehicle
        };













        formData.append("Applicationdata", JSON.stringify(applicaionDetails));
        formData.append("LoanAmount", JSON.stringify($scope.LoanAmountDetails));
        formData.append("LoanProperty", JSON.stringify($scope.PropertyDetailsList));
        formData.append("PreLoan", JSON.stringify($scope.PreLoanDetailsList));
        formData.append("GuarantorDetails", JSON.stringify($scope.guarantor));
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
                    ShowOkMessage("Success", $scope._language == 'en-us' ? "Loan application has been submitted successfully.<br/><br/>Application No.:-<b>" + data.Result + "</b>" : "ऋण आवेदन सफलतापूर्वक जमा कर लिया गया है|<br/><br/>आवेदन संख्या:-<b>" + data.Result + "</b>", function () {
                        location.href = "../pldb/LoanApplicationlist";
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
        if ($scope.ValidateAddMember(1)) {
            if ($scope.ValidateLoanAmount()) {
                if ($scope.ValidateAddGuarantor()) {
                    if ($scope.ValidateGuarantorProperty()) {
                        if ($scope.ValidatetblPropertyDetails()) {

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
    $scope.SowApplicationGrid = function () {
        window.location = '../pldb/PLDBSchemeList';
    }
    $scope.MemberRegistration = {
        PLDB_ID: "0",
        Branch_ID: "0"
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
                        $scope.GetPropertyDetailsPanchayt(index);
                        $scope.GetPropertyDetailsVillages(index);
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
    $scope.ProcessStatus = [];
    $scope.GetLoanProcessStatus = function () {
        $http.post(SiteUrl + "/pldb/GetLoanProcessStatus").
            success(function (data, status) {
                $scope.ProcessStatus = JSON.parse(data.DataList);
            }).
            error(function (data, status) {
                $scope.ProcessStatus = [];
            });
    }
    // START : Get Relation List
    //$scope.GetBloodRelationList = function () {
    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=RELATION&MasterCode=-1").
    //        success(function (data, status) {
    //            $scope.BloodRelation = data;
    //        }).
    //        error(function (data, status) {
    //            $scope.BloodRelation = [];
    //        });
    //}
    // END: Get Relation List

    // START : Get Category  List
    $scope.GetCategoryList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=-1").
            success(function (data, status) {
                $scope.CategoryList = data;
            }).
            error(function (data, status) {
                $scope.CategoryList = [];
            });
    }
    $scope.GetCategoryList();
    // END: Get Category List

    $scope.GetFarmerCategory = function () {
        $http.get(SiteUrl + "/pldb/GetLovMaster?Id=5").
            success(function (data, status) {
                $scope.FarmerCategoryList = data;
            }).
            error(function (data, status) {
                $scope.FarmerCategoryList = [];
            });
    }

    $scope.LoanTypeList = [];
    $scope.GetLoanTypeList = function () {
        $http.get(SiteUrl + "/pldb/GetLovMaster?Id=7").
            success(function (data, status) {
                $scope.LoanTypeList = data;
            }).
            error(function (data, status) {
                $scope.LoanTypeList = [];
            });
    }
    $scope.GetLoanTypeList();

    // START : Get Category  List
    $scope.GetVillageList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=VILLAGE&MasterCode=" + $scope.MemberRegistration.GP_ID).
            success(function (data, status) {
                $scope.VillageList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.VillageList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    }
    // END: Get Category List

    //START : AddNewLandRecordGSS

    $scope.Applicaion = {
        IsApplicantShared: false,
        IsApplicantHasTransportCompany: false,
        IsDrivinglicense: false,
        IsProposedRented: false
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


        if (!$scope.LoanMembers[0].IsValidate) {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Aadhaar/Membership Number and add at least one applicant' : 'कृपया आधार / सदस्यता संख्या दर्ज करें और कम से कम एक आवेदक जोड़ें') < 0) {
                message += $scope._language == 'en-us' ? '<br />Please Enter Aadhaar/Membership Number and add at least one applicant.' : '<br />कृपया आधार / सदस्यता संख्या दर्ज करें और कम से कम एक आवेदक जोड़ें।';
            }
        }
        else {
            $scope.LoanMembers.forEach(function (item, index) {

                // alert($("#tblLandMemberDetail").find("tr").eq(index+1).find('input[type="text"]').length);

                if (item.ApplicantAadharNumber == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Membership Number' : 'कृपया सदस्यता क्रमांक दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />Please Enter Membership Number.' : '<br />कृपया सदस्यता क्रमांक दर्ज करें।';
                    }
                    $("#tblLoanMembersDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(0).addClass("notValid");
                }
           
        });
            } 

        if (message.length > 29) {
            ShowOkMessage("Add Member", message, function () { });
            return false;
        }
        return true;
    }

    $scope.ValidateVehicle = function () {

        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#panal_6").find("input,textarea").removeClass("notValid");

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
        $scope.LoanAmountDetails.forEach(function (item, index) {

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
            }
            if (item.LoanAmountApplied == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Applied Amount' : 'कृपया आवेदित ऋण राशि दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Applied Amount.' : '<br />कृपया आवेदित ऋण राशि दर्ज करें|';
                }
                $("#tblLoanAmountDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(1).addClass("notValid");
            }
        });

        if (message.length > 29) {
            ShowOkMessage("Add Member", message, function () { });
            return false;
        }
        return true;
    }

    $scope.ValidatetblPropertyDetails = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblPropertyDetails").find('input[type="text"]').removeClass("notValid");
        $("#tblPropertyDetails").find('select').removeClass("notValid");
        $scope.PropertyDetailsList.forEach(function (item, index) {
            //if (item.Village == '0') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Village' : 'कृपया ग्राम का चयन करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Select Village.' : '<br />कृपया ग्राम चयन करें|';
            //    }
            //    $("#tblPropertyDetails").find("tr").eq(index + 3).find('select').eq(0).addClass("notValid");
            //}
            //if (item.GramPanchayat == '0') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Gram Panchayat' : 'कृपया ग्राम पंचायत का चयन करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Select Gram Panchayat.' : '<br />कृपया ग्राम पंचायत का चयन करें|';
            //    }
            //    $("#tblPropertyDetails").find("tr").eq(index + 3).find('select').eq(1).addClass("notValid");
            //}
            //if (item.Tehsil == '0') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Tehsil' : 'कृपया तहसील का चयन करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Select Tehsil.' : '<br />कृपया तहसील का चयन करें|';
            //    }
            //    $("#tblPropertyDetails").find("tr").eq(index + 3).find('select').eq(2).addClass("notValid");
            //}
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

            if (item.Address == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Member Property Address' : 'कृपया आवेदक की प्रॉपर्टी का पता दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Member Property Address.' : '<br />कृपया आवेदक की प्रॉपर्टी का पता दर्ज करें|';
                }
                $("#txtAddress_" + index).addClass("notValid");
            }
            else {
                $("#txtAddress_" + index).removeClass("notValid");
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
   // $scope.GetBloodRelationList();
    $scope.getSchemeByUrl();
    //$scope.GetCategoryList();
    // $scope.LandDetailsGSS.push(new GSSLandDetailEntity());
    //$scope.GetFarmerCategory();

    $scope.LoanMembers = [];
    $scope.LoanMembers.push($scope.Member);
    
    $scope.PLDBList();

    //$(".blockUI").fadeIn("slow");
    //// END : On Page Load
    $scope.RemoveMemberRow = function (index) {
        ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove member from list.' : 'क्या आप वाकई सदस्य को सूची से हटाना चाहते हैं', function () {

            if ($scope.LoanMembers.length == 1) {
                $scope.ResetMember();
                $scope.LoanMembers = [];
                $scope.LoanMembers.push($scope.Member);
            } else {

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

    $scope.RemovePropertyRow = function (index) {
        ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove details from list.' : 'क्या आप वाकई सूची से विवरण निकालना चाहते हैं', function () {
            $scope.PropertyDetailsList[index].IsDelete = true;
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
    $scope.AddNewPropertyDetails = function () {
        if ($scope.ValidateAddMember(1)) {
            if ($scope.ValidatetblPropertyDetails()) {
                $scope.ResetPropertyDetails();
                //$scope.LoanAmountDetails = [];
                $scope.PropertyDetailsList.push($scope.PropertyDetails);
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
        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetSectorMaster", { "WhereCase": "  and se.is_active=1 and se.scheme_id=" + scheme_id }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
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

        $scope.LoadSector(1, 0).then(function () {
            $scope.LoadPurpose().then(function () {
                $scope.GetSchemeMaster().then(function () {
                   
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
        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetPurposeMaster", { "WhereCase": sector_id }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
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
                $(".blockUI").fadeOut("slow");
            });
    };

    /*-------End Sector, Scheme, Purpose----------------------------------------*/

    

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
        TotalArea: "",
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
            TotalArea: "",
            GuarantorPropertyUnitOfArea: ""
        }
    }

    $scope.GuarantorPropertyDetailsList = [];

    $scope.GuarantorPropertyDetailsList.push($scope.GuarantorPropertyDetails);

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
            if (item.MobileNo == "") {
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


    $scope.ValidateGuarantorProperty = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblGuarantorPropertyDetails").find('input[type="text"]').removeClass("notValid");
        $("#tblGuarantorPropertyDetails").find('select').removeClass("notValid");
        $scope.GuarantorPropertyDetailsList.forEach(function (item, index) {
            //if (item.Village == '0') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Village' : 'कृपया ग्राम का चयन करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Select Village.' : '<br />कृपया ग्राम चयन करें|';
            //    }
            //    $("#tblPropertyDetails").find("tr").eq(index + 3).find('select').eq(0).addClass("notValid");
            //}
            //if (item.GramPanchayat == '0') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Gram Panchayat' : 'कृपया ग्राम पंचायत का चयन करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Select Gram Panchayat.' : '<br />कृपया ग्राम पंचायत का चयन करें|';
            //    }
            //    $("#tblPropertyDetails").find("tr").eq(index + 3).find('select').eq(1).addClass("notValid");
            //}
            //if (item.Tehsil == '0') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Tehsil' : 'कृपया तहसील का चयन करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Select Tehsil.' : '<br />कृपया तहसील का चयन करें|';
            //    }
            //    $("#tblPropertyDetails").find("tr").eq(index + 3).find('select').eq(2).addClass("notValid");
            //}

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



    $scope.LoanGuarantor = [];

    $scope.LoanGuarantor.push($scope.Guarantor);

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
            $scope.GuarantorPropertyDetailsList[index].IsDelete = true;
            $scope.$apply();
        });
    }

    $scope.AddNewGuarantor = function () {
        if ($scope.ValidateAddGuarantor(2)) {
            $scope.ResetGuarantor();
            // $scope.Member.GramPanchayats = $scope.LoanMembers[$scope.LoanMembers.length - 1].GramPanchayats;
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
        AadhaarAuth(AADHAARNo, $http, function (data) {
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

    /*--------- Application Edit code block---------------*/
    
    $scope.ApplicationEdit = function () {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetApplicationDetails", { LoanId: $scope.LoanId }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                dataList = JSON.parse(data.DataList);

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
                            IsDisableScheme: true,                           
                        });
                    });
                }

                if (dataList.Table2.length > 0) {
                    $scope.LoanMembers = [];
                    dataList.Table2.forEach(function (itm, index) {
                        $scope.ShowApplicanDetails = true;
                        $scope.LoanMembers.push({
                           Id: itm.loan_memer_id, 
                           ApplicantName :itm.MemberName,
                           FatherName :itm.FatherName,
                           ApplicantAge: itm.Age + (itm.Age != '' && itm.Age > 0 ? ' Year' : ''),
                           CategoryId : $scope._language == 'en-us' ? itm.CategoryEn : itm.CategoryHi,
                           Village : $scope._language == 'en-us' ? itm.VILLAGE_ENG : itm.VILLAGE_MANGAL,
                           VillagePanchayat : $scope._language == 'en-us' ? itm.GP_ENG : itm.GP_MANGAL,
                           MobileNo :itm.MobileNo,
                           Business :itm.Business,
                           BusinessAddress :itm.Address,
                           Photo :itm.Photo,                           
                           TehsilName : $scope._language == 'en-us' ? itm.BLOCK_ENG : itm.BLOCK_MANGAL,
                           VillageName : $scope._language == 'en-us' ? itm.VILLAGE_ENG : itm.VILLAGE_MANGAL,
                           GramPanchayatName : $scope._language == 'en-us' ? itm.GP_ENG : itm.GP_MANGAL,
                           FarmerCategory : $scope._language == 'en-us' ? itm.FarmerCategoryEn : itm.FarmerCategoryHi,
                           Tehsil : $scope._language == 'en-us' ? itm.BLOCK_ENG : itm.BLOCK_MANGAL,
                           MemberShip :itm.MembershipNo,
                           MembershipNumber :itm.MembershipNo,
                           AADHARNo: itm.AADHARNo,
                           ApplicantAadharNumber: itm.AADHARNo,                                             
                           IsShow : true,
                           IsValidate: true                            
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

                        });
                    });
                });                            

            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.LoanMembers = [];
                // $(".se-pre-con").fadeOut("slow");
            });






















        //$(".clspanal").hide();
        //$(".expandDetails").hide();
        //$scope.dvApplicationForm = true;
        //$scope.divLoanApplicationList = false;
        //$scope.ShowAddNewApplication = false;
        //$(".se-pre-con").fadeIn("slow");
        //$scope.LoanDetails = th;
        //$scope.LoanApplicationId = th.loan_id;
        //$scope.IsSubmiApplication = th.issubmit;
        //$scope.MemberRegistration.PLDB_ID = th.pldb_id.toString();
        //$scope.MemberRegistration.Branch_ID = th.branch_id.toString();
        //$scope.PLDBBranchList(th.branch_id.toString());
        ////$scope.GetGramPanchayatData();
        //$scope.GetMemberList(th.loan_id, th.branch_id).then(function () {
        //    //$scope.GetAmountList(th.loan_id).then(function () {
        //        $scope.GetPropertyList(th.loan_id).then(function () {
        //            $scope.GetPreLoanList(th.loan_id).then(function () {
        //                //$(".se-pre-con").fadeOut("slow");
        //                //if (th.issubmit) {
        //                //    $(".clspanal").find("input").attr("disabled", "disabled");
        //                //    $(".clspanal").find("select").attr("disabled", "disabled");
        //                //    $(".clspanal").find("textarea").attr("disabled", "disabled");
        //                //    $(".clspanal").find("tfoot").hide();
        //                //    $(".clspanal").find("i").hide();
        //                //    $(".clspanal").eq(0).show();
        //                //    $(".clspanal").eq(1).show();
        //                //    $(".expandDetails").show();
        //                //    $(".expandDetails").eq(0).removeClass("fa-plus-circle").addClass("fa-minus-circle");
        //                //    $(".expandDetails").eq(1).removeClass("fa-plus-circle").addClass("fa-minus-circle");
        //                //} else {
        //                //    $(".clspanal").show();
        //                //}
        //            });
        //        });
        //    //});
        //});
    }

    if ($scope.LoanId != "") {
        $scope.ApplicationEdit();
    }
    
    /*--------- End Application code block */



    $scope.FileType = "";

    $scope.AddFile = function (type) {
        $scope.FileType = type;
    }

    $scope.ChangeFile = function (type) {
        if (type == 1) {
            $scope.LoanMembers[1].IsPartnershipCertifiedCopy = false;
            $scope.LoanMembers[1].PartnershipCertifiedCopy = "";
        } else if (type == 2) {
            $scope.LoanMembers[0].IsTransportCompanyRegCertification = false;
            $scope.LoanMembers[0].TransportCompanyRegCertification = "";
        } else if (type == 3) {
            $scope.LoanMembers[0].IsDrivinglicenseCopy = false;
            $scope.LoanMembers[0].DrivinglicenseCopy = "";
        } else if (type == 4) {
            $scope.LoanMembers[0].IsVehicleRantProposalCopy = false;
            $scope.LoanMembers[0].VehicleRantProposalCopy = "";
        }
    }

    $scope.UploadFile = function (id, type) {
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
                            $scope.LoanMembers[1].PartnershipCertifiedCopy = request.responseText;
                        } else if (type == 2) {
                            $scope.LoanMembers[0].TransportCompanyRegCertification = request.responseText;
                        } else if (type == 3) {
                            $scope.LoanMembers[0].DrivinglicenseCopy = request.responseText;
                        } else if (type == 4) {
                            $scope.LoanMembers[0].VehicleRantProposalCopy = request.responseText;
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
                                scope.LoanMembers[1].IsPartnershipCertifiedCopy = true;
                            } else if (scope.FileType == 2) {
                                scope.LoanMembers[0].IsTransportCompanyRegCertification = true;
                            } else if (scope.FileType == 3) {
                                scope.LoanMembers[0].IsDrivinglicenseCopy = true;
                            } else if (scope.FileType == 4) {
                                scope.LoanMembers[0].IsVehicleRantProposalCopy = true;
                            }
                            scope.$apply();
                        }
                    }

                });
            };
        }
    };
});