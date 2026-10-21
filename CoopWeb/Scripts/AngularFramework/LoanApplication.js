var CommityRegistrtionModule = angular.module('LoanApplicationMod', ['angularUtils.directives.dirPagination']);

CommityRegistrtionModule.controller('LoanApplicationCnt', function ($scope, $http, $rootScope) {

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
        $http.post(SiteUrl + "/pldb/GetLoanApplicationList", JSON.stringify({ Details: jsonParameters, Level: "1" })).
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
    $scope.GetLoanApplicationList();
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
        MemberShip: ""
    }
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
            MemberShip: ""
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
            IsDelete: false
        }
    }
    $scope.PropertyDetailsList = [];
    $scope.PropertyDetailsList.push($scope.PropertyDetails);

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

    function GSSCropDetailEntity() {
        this.LandInHactor = 0;
        this.IsIrrigated = false;
        this.SownCropName = '';
        this.IsKharif = false;
    }

    function CommercialLoanDetailEntity() {
        this.InstitutionName = '';
        this.ApprovedLoanAmt = 0;
        this.RemainingLoanAmtInTime = 0;
        this.RemainingLoanAmtTimeBarred = 0;
    }

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
        var url = window.location.href.split('?');
        if (url.length > 1) {
            var url2 = url[1].substr(17, url[1].toString().length);
            if (url2.indexOf('Error') > 0) {
                alert('some error occurred Please Try after some time');
                window.location = '../Citizen/SocietyMemberRegistration';
            }
            else
                $scope.GetMemberDetailsList(url2);
        }
        else if ($('#EUserType').val() == 'C') {
            $scope.divGrid = true;
            $scope.divViewShow = false;
            $scope.divRegistration = false;
            $scope.IsCitizen = true;
            $scope.GetSocietyMemberData();
        }
        else {
            $scope.divViewShow = false;
            $scope.divRegistration = true;
            $scope.divGrid = false;
            $scope.IsCitizen = false;
        }
        $scope.BackBtnLast = true;
    };
    // END : Code for Language label setting

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

    $scope.CommercialBankLoneDetailsCapture = function () {

        if ($scope.MemberRegistration.IsOtherBankLonee == '0') {
            $scope.IsCommercialBankDetailsShow = true;
        }
        else {
            $scope.IsCommercialBankDetailsShow = false;
            $scope.MemberRegistration["OtherBankLoanDetails"] = [];
            $scope.CommercialLoanDetail = [];
        }
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
     return   $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + $scope.MemberRegistration.PLDB_ID).
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
    $scope.GetMemberDetails = function (mId, index) {
        // 
        
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        var isValid = true;
        if ($scope.MemberRegistration.PLDB_ID == '0') {
            // message += $scope._language == 'en-us' ? '<br /> Please Select PLDB.' : '<br />कृपया पी.एल.डी.बी का चयन करें।';
            //$("#ddlDistrict").addClass("notValid");
            //isValid = false;
        }
        if ($scope.MemberRegistration.Branch_ID == '0') {
            //message += $scope._language == 'en-us' ? '<br /> Please Select branch.' : '<br />कृपया ब्रांच का चयन करें।';
            //$("#ddlblock").addClass("notValid");
            //isValid = false;
        }
        if ($scope.LoanMembers.filter(x => x.MembershipNumber === mId).length > 1) {
            message += $scope._language == 'en-us' ? '<br /> Membership Number already added please enter other number.' : '<br />सदस्यता संख्या पहले से ही जोड़ा गया कृपया अन्य नंबर दर्ज करें।';
            isValid = false;
        }
        if (!isValid) {
            // $scope.ResetMember();
            // $scope.LoanMembers = [];
            //$scope.LoanMembers.push($scope.Member);
            ShowOkMessage("", message, function () { });
        } else if (mId != '' && $scope.MemberRegistration.Branch_ID != '0' && $scope.MemberRegistration.PLDB_ID != '0') {
            $(".se-pre-con").fadeIn("slow");
            $http.get(SiteUrl + "/pldb/GetMemberDetails?MembershipNo=" + mId + "&DistrictId=" + $scope.MemberRegistration.PLDB_ID + "&BranchId=" + $scope.MemberRegistration.Branch_ID).
                success(function (result, status) {
                    //
                    //alert(data.length);
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
                        ShowOkMessage("", "Member details not found", function () {
                        });

                    } else {


                        $scope.LoanMembers[index].Name = data[0].MemberName;
                        $scope.LoanMembers[index].FatherName = data[0].FatherName;
                        $scope.LoanMembers[index].Age = data[0].Age;
                        $scope.LoanMembers[index].CategoryId = $scope._language == 'en-us' ? data[0].CategoryEn : data[0].CategoryHi;
                        $scope.LoanMembers[index].Village = $scope._language == 'en-us' ? data[0].VILLAGE_ENG : data[0].VILLAGE_MANGAL;
                        $scope.LoanMembers[index].VillagePanchayat = $scope._language == 'en-us' ? data[0].GP_ENG : data[0].GP_MANGAL;
                        $scope.LoanMembers[index].MobileNo = data[0].MobileNo;
                        $scope.LoanMembers[index].Photo = data[0].Photo;
                        //$scope.LoanMembers[index].DistrictName = data[0].DISTRICT_MANGAL;
                        $scope.LoanMembers[index].TehsilName = $scope._language == 'en-us' ? data[0].BLOCK_ENG : data[0].BLOCK_MANGAL;
                        $scope.LoanMembers[index].VillageName = $scope._language == 'en-us' ? data[0].VILLAGE_ENG : data[0].VILLAGE_MANGAL;
                        $scope.LoanMembers[index].GramPanchayatName = $scope._language == 'en-us' ? data[0].GP_ENG : data[0].GP_MANGAL;
                        $scope.LoanMembers[index].FarmerCategory = $scope._language == 'en-us' ? data[0].FarmerCategoryEn : data[0].FarmerCategoryHi;
                        $scope.LoanMembers[index].Tehsil = $scope._language == 'en-us' ? data[0].BLOCK_ENG : data[0].BLOCK_MANGAL;
                        $scope.LoanMembers[index].MemberShip = data[0].MembershipNo;

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

        var applicaionDetails = {
            Status: $scope.MemberRegistration.ProcessStatus,
            Remark: $scope.MemberRegistration.Remarks,
            Id: $scope.LoanApplicationId
        };



        $http.post(SiteUrl + "/pldb/UpdateLoanStatus", { Details: applicaionDetails, Level: "1" }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data.Result != "-1") {
                    ShowOkMessage("Success", $scope._language == 'en-us' ? "Loan application has been updated successfully.<br/><br/>Application No.:-<b>" + data.Result + "</b>" : "ऋण आवेदन सफलतापूर्वक अपडेट कर लिया गया है|<br/><br/>आवेदन संख्या:-<b>" + data.Result + "</b>", function () {
                        location.href = "../pldb/LoanApplication";
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

        if (!isValid) {
            ShowOkMessage("Error", message, function () {
            });
        } else {

            ShowAlertMessage("Alert", _language === 'en-us' ? "Are you sure you want to submit this loan application." : "क्या आप वाकई यह ऋण आवेदन जमा करना चाहते हैं?", function () {
                $scope.SaveUser(IsSubmit);

            });
        }
    }



    $scope.SaveUser1 = function (IsSubmit) {
        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();
        
        $("#dvMemberPhoto").find('input[type="file"]').each(function (indx) {
            //alert($(this)[0].files.length);
            // 
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
                IsMainMember: $scope.LoanMembers.length > 1 ? itm.IsMainMember : true
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
                IsDelete: itm.IsDelete
            });
        });


        var applicaionDetails = {
            load_id: $scope.LoanApplicationId, pldb_id: $scope.MemberRegistration.PLDB_ID, branch_id: $scope.MemberRegistration.Branch_ID,
            Members: members,
            LoanAmount: LoanAmount,
            LoanProperty: LoanProperty,
            PreLoan: $scope.PreLoanDetailsList,
            IsSubmit: IsSubmit
        };

        formData.append("Applicationdata", JSON.stringify(applicaionDetails));
        formData.append("LoanAmount", JSON.stringify($scope.LoanAmountDetails));
        formData.append("LoanProperty", JSON.stringify($scope.PropertyDetailsList));
        formData.append("PreLoan", JSON.stringify($scope.PreLoanDetailsList));


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
                        location.href = "../pldb/LoanApplicationList";
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



    $scope.SaveLoanApplication1 = function (IsSubmit) {
        if ($scope.ValidateAddMember(1)) {
            if ($scope.ValidateLoanAmount()) {
                if ($scope.ValidatetblPropertyDetails()) {
                    if (IsSubmit) {
                        ShowAlertMessage("Alert", _language === 'en-us' ? "Are you sure you want to submit this loan application." : "क्या आप वाकई यह ऋण आवेदन जमा करना चाहते हैं?", function () {
                            $scope.SaveUser1(IsSubmit);
                        });
                    } else {
                        $scope.SaveUser1(IsSubmit);
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
    $scope.ApplicationEdit = function (th) {
        //window.location.href = th.AppLoanFormUrl + "?pid=" + th.PurposeId + "&id=" + th.loanIdINc;
        window.location.href = "../pldb/LoanApplicationForm?pid=" + th.PurposeId + "&id=" + th.loanIdINc;
        return;
        $(".clspanal").hide();
        $(".expandDetails").hide();
        $scope.dvApplicationForm = true;
        $scope.divLoanApplicationList = false;
        $scope.ShowAddNewApplication = false;
        $(".se-pre-con").fadeIn("slow");


        $scope.LoanDetails = th;

        $scope.LoanApplicationId = th.loan_id;


        $scope.IsSubmiApplication = th.issubmit;
        $scope.MemberRegistration.PLDB_ID = th.pldb_id.toString();
        $scope.MemberRegistration.Branch_ID = th.branch_id.toString();
        $scope.PLDBBranchList(th.branch_id.toString());
        $scope.GetGramPanchayatData();
        $scope.GetMemberList(th.loan_id, th.branch_id).then(function () {
            $scope.GetAmountList(th.loan_id).then(function () {
                $scope.GetPropertyList(th.loan_id).then(function () {
                    $scope.GetPreLoanList(th.loan_id).then(function () {
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
                            $scope.GetLoanProcessStatus();
                        } else {
                            $(".clspanal").show();
                        }
                    });
                });
            });
        });
    }


    $scope.EditAppication = function (th) {
        $(".clspanal").hide();
        $(".expandDetails").hide();
        $scope.dvApplicationForm = true;
        $scope.divLoanApplicationList = false;
        $scope.ShowAddNewApplication = false;
        $(".se-pre-con").fadeIn("slow");


        $scope.LoanDetails = th;

        $scope.LoanApplicationId = th.loan_id;


        $scope.IsSubmiApplication = false;
        $scope.MemberRegistration.PLDB_ID = th.pldb_id.toString();
        $scope.MemberRegistration.Branch_ID = th.branch_id.toString();
        $scope.PLDBBranchList(th.branch_id.toString());
        $scope.GetGramPanchayatData();
        $scope.GetMemberList(th.loan_id, th.branch_id).then(function () {
            $scope.GetAmountList(th.loan_id).then(function () {
                $scope.GetPropertyList(th.loan_id).then(function () {
                    $scope.GetPreLoanList(th.loan_id).then(function () {
                        $(".se-pre-con").fadeOut("slow");
                        $scope.MemberRegistration.PLDB_ID = th.pldb_id.toString();
                        $scope.MemberRegistration.Branch_ID = th.branch_id.toString();
                        $(".clspanal").show();

                    });
                });
            });
        });
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

                        //$scope.GetSchemeMaster();
                        

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
        $http.post(SiteUrl + "/pldb/GetLoanProcessStatus?ActionId="+0).
            success(function (data, status) {
                $scope.ProcessStatus = JSON.parse(data.DataList);
            }).
            error(function (data, status) {
                $scope.ProcessStatus = [];
            });
    }
    $scope.GetLoanProcessStatus();
    // START : Get Relation List
    $scope.GetBloodRelationList = function () {
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
    $scope.GetEconomicSectionList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ECONOMICGROUP&MasterCode=-1").
            success(function (data, status) {
                $scope.EconomicSection = data;
            }).
            error(function (data, status) {
                $scope.EconomicSection = [];
            });
    }
    // END: Get Econiomic List

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
    //$scope.GetPurposeOfLoan = function () {
    //    $http.get(SiteUrl + "/pldb/GetLovMaster?Id=6").
    //        success(function (data, status) {
    //            $scope.PurposeOfLoanList = data;
    //        }).
    //        error(function (data, status) {
    //            $scope.PurposeOfLoanList = [];
    //        });
    //}
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

        $scope.LoanMembers.forEach(function (item, index) {

            // alert($("#tblLandMemberDetail").find("tr").eq(index+1).find('input[type="text"]').length);

            if (item.MembershipNumber == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Membership Number' : 'कृपया सदस्यता क्रमांक दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Membership Number.' : '<br />कृपया सदस्यता क्रमांक दर्ज करें।';
                }
                $("#tblLandMemberDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(0).addClass("notValid");
            }
            //if (item.Name == '') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Name' : 'कृपया नाम दर्ज करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Enter Name.' : '<br />कृपया नाम दर्ज करें।';
            //    }
            //    $("#tblLandMemberDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(1).addClass("notValid");
            //}
            //if (item.FatherName == '') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Father Name' : 'कृपया पिता का नाम दर्ज करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Enter Father Name.' : '<br />कृपया पिता का नाम दर्ज करें।';
            //    }
            //    $("#tblLandMemberDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(2).addClass("notValid");
            //}
            //if (item.Age == '') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Age' : 'कृपया उम्र दर्ज करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Enter Age.' : '<br />कृपया उम्र दर्ज करें।';
            //    }
            //    $("#tblLandMemberDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(3).addClass("notValid");
            //}

            //if (item.FarmerCategory == '0') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Farmer Category' : 'कृपया किसान की श्रेणी का चयन करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Select Farmer Category.' : '<br />कृपया किसान की श्रेणी का चयन करें।';
            //    }
            //    $("#tblLandMemberDetail").find("tr").eq(index + 1).find('select').eq(0).addClass("notValid");
            //}

            //if (item.CategoryId == '0') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Category' : 'कृपया वर्ग का चयन करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Select Category.' : '<br />कृपया वर्ग का चयन करें।';
            //    }
            //    $("#tblLandMemberDetail").find("tr").eq(index + 1).find('select').eq(1).addClass("notValid");
            //}

            //if (item.Village == '0') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Village' : 'कृपया ग्राम का चयन करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Select Village.' : '<br />कृपया ग्राम का चयन करें।';
            //    }
            //    $("#tblLandMemberDetail").find("tr").eq(index + 1).find('select').eq(2).addClass("notValid");
            //}

            //if (item.VillagePanchayat == '0') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Gram Panchayat' : 'कृपया ग्राम पंचायत का चयन करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Select Gram Panchayat.' : '<br />कृपया ग्राम पंचायत का चयन करें।';
            //    }
            //    $("#tblLandMemberDetail").find("tr").eq(index + 1).find('select').eq(3).addClass("notValid");
            //}

            //if (item.Tehsil == '0') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Tehsil' : 'कृपया तहसील का चयन करें') < 0) {
            //        message += $scope._language == 'en-us' ? '<br />Please Select Tehsil.' : '<br />कृपया तहसील का चयन करें।';
            //    }
            //    $("#tblLandMemberDetail").find("tr").eq(index + 1).find('select').eq(4).addClass("notValid");
            //}

            //if (item.MobileNo == '') {
            //    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Mobile Number' : 'कृपया मोबाइल नंबर दर्ज करें') < 0) {
            //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Mobile Number' : '\n <br /> कृपया मोबाइल नंबर दर्ज करें |';
            //    }
            //    $("#tblLandMemberDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(4).addClass("notValid");
            //}

        });

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
            if (item.Village == '0') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Select Village' : 'कृपया ग्राम का चयन करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Select Village.' : '<br />कृपया ग्राम चयन करें|';
                }
                $("#tblPropertyDetails").find("tr").eq(index + 3).find('select').eq(0).addClass("notValid");
            }
            if (item.GramPanchayat == '0') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Select Gram Panchayat' : 'कृपया ग्राम पंचायत का चयन करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Select Gram Panchayat.' : '<br />कृपया ग्राम पंचायत का चयन करें|';
                }
                $("#tblPropertyDetails").find("tr").eq(index + 3).find('select').eq(1).addClass("notValid");
            }
            if (item.Tehsil == '0') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Select Tehsil' : 'कृपया तहसील का चयन करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Select Tehsil.' : '<br />कृपया तहसील का चयन करें|';
                }
                $("#tblPropertyDetails").find("tr").eq(index + 3).find('select').eq(2).addClass("notValid");
            }
            if (item.Khasara == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter ख:न. के कुल किता' : 'कृपया ख:न. के कुल किता दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter ख:न. के कुल किता.' : '<br />कृपया ख:न. के कुल किता दर्ज करें|';
                }
                $("#tblPropertyDetails").find("tr").eq(index + 3).find('input[type="text"]').eq(0).addClass("notValid");
            }
            if (item.LoanAmountApplied == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Applied Amount' : 'कृपया आवेदित ऋण राशि दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Applied Amount.' : '<br />कृपया आवेदित ऋण राशि दर्ज करें|';
                }
                $("#tblPropertyDetails").find("tr").eq(index + 3).find('input[type="text"]').eq(1).addClass("notValid");
            }
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

    //// START : On Page Load
    $scope.getTranslation();
    $scope.GetBloodRelationList();
    $scope.GetEconomicSectionList();
    //$scope.GetCategoryList();
    // $scope.LandDetailsGSS.push(new GSSLandDetailEntity());
    //$scope.GetFarmerCategory();

    $scope.LoanMembers = [];
    $scope.LoanMembers.push($scope.Member);
    $scope.PLDBList();
    $scope.SownCropDetailGSS.push(new GSSCropDetailEntity());
    $scope.CommercialLoanDetail.push(new CommercialLoanDetailEntity());

    //$(".blockUI").fadeIn("slow");
    //// END : On Page Load
    $scope.RemoveMemberRow = function (index) {
        ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove member from list.' : 'क्या आप वाकई सदस्य को सूची से हटाना चाहते हैं', function () {
            $scope.LoanMembers[index].IsDelete = true;
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

    $scope.AddNewPreLoanDetails = function () {
        if ($scope.ValidateAddMember(1)) {
            if ($scope.ValidatetblPreLoanDetails()) {
                $scope.ResetPreLoanDetails();
                $scope.PreLoanDetailsList.push($scope.PreLoanDetails);
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
        if ($scope.ValidateAddMember(1)) {
            if ($scope.ValidateLoanAmount()) {
                if ($scope.ValidatetblPropertyDetails()) {
                    $scope.LoanApplicationLetter = true;
                    $scope.SelfDeclarationLetter = false;
                    $scope.divParentDataSelection = true;
                }
            }
        }
    }

    $scope.GetNameFromList = function (list, id, type) {
        
        if (type == 'Village') {
            var data = list.find(x => x.VILLAGE_ID === id.toString());
            return _language == 'en-us' ? data.VILLAGE_ENG : data.VILLAGE_MANGAL;
        } else if (type == 'Tehsil') {
            var data = list.find(x => x.BLOCK_ID === id.toString());
            return _language == 'en-us' ? data.BLOCK_ENG : data.BLOCK_MANGAL;
        } else if (type == 'District') {
            var data = list.find(x => x.PldbId === id.toString());
            return _language == 'en-us' ? data.PLDBName : data.PLDBName;
        } else if (type == 'Village') {
            var data = list.find(x => x.VILLAGE_ID === id.toString());
            return _language == 'en-us' ? data.VILLAGE_ENG : data.VILLAGE_MANGAL;
        } else if (type == 'Village') {
            var data = list.find(x => x.VILLAGE_ID === id.toString());
            return _language == 'en-us' ? data.VILLAGE_ENG : data.VILLAGE_MANGAL;
        } else if (type == 'Branch') {
            var data = list.find(x => x.BLOCK_ID === id.toString());
            return _language == 'en-us' ? data.BLOCK_ENG : data.BLOCK_MANGAL;
        }
    }

    $scope.LoanSchemeList = [];
    $scope.GetSchemeMaster = function () {
        return $http.post(SiteUrl + "/pldb/GetSchemeMaster", { "WhereCase": " and is_active=1" }).
            success(function (data, status) {
                $scope.LoanSchemeList = [];
                $scope.LoanSchemeList = data;
                $scope.TotalRecord = $scope.LoanSchemeList.length;

                $scope.LoanAmountDetails[0].LoanSchemeList = data;



                //$(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching Society Details.");
                //$(".blockUI").fadeOut("slow");
            });

    }
    $scope.GetSchemeMaster();
    $scope.LoanSectorList = [];
    $scope.LoadSector = function (scheme_id, index) {
        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetSectorMaster", { "WhereCase": "  and se.is_active=1 and se.scheme_id=" + scheme_id }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.LoanSectorList = [];
                $scope.LoanSectorList = data;
                $scope.LoanAmountDetails[index].LoanSectorList = data;
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
            });
    };

    $scope.LoadPurpose = function (sector_id, index) {
        $(".blockUI").fadeIn("slow");
        return $http.post(SiteUrl + "/pldb/GetPurposeMaster", { "WhereCase": "  and se.is_active=1 and se.sector_id=" + sector_id }).
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

    $scope.ChooseDefaultMember = function (index) {
        $scope.LoanMembers.forEach(function (item, indx) {
            if (index != indx) {
                item.IsMainMember = false;
            }
        });
    }
    if ($("#hdnLevelId").val() == "2" || $("#hdnLevelId").val() == "1") {
        $scope.ShowPLDB = true;
        $scope.ShowBranch = true;
    } else if ($("#hdnLevelId").val() == "3") {
        $scope.ShowPLDB = false;
        $scope.ShowBranch = true;
        $scope.MemberRegistration.PLDB_ID = $("#hdnPLDBId").val();

        $scope.PLDBBranchList().then(function () {
            //$scope.MemberRegistration.BLOCK_ID = $("#hdnPLDBBrancchId").val();
        });

    } else {
        $scope.ShowPLDB = false;
        $scope.ShowBranch = false;
        $scope.MemberRegistration.PLDB_ID = $("#hdnPLDBId").val();
        $scope.MemberRegistration.BLOCK_ID = $("#hdnPLDBBrancchId").val();
        $scope.PLDBBranchList().then(function () {
            $scope.MemberRegistration.BLOCK_ID = $("#hdnPLDBBrancchId").val();
        });
    }
});
