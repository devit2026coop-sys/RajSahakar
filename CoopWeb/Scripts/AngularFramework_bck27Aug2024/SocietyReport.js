var CommityRegistrtionModule = angular.module('SocietyReportMod', ['angularUtils.directives.dirPagination']);

CommityRegistrtionModule.controller('SocietyReportCnt', function ($scope, $http, $rootScope) {

    /*------------Paging and Filter-----------------*/


    $scope.LiabilityList = [];
    $scope.AssetList = [];
    $scope.InvestmentList = [];
    $scope.divSocietyHealthReport = false;

    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 20,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }

    function InvestmentObj() {
        this.WhereDid = '';
        this.Amount = '';
        this.SFinancialHealthId = 2;
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
        debugger;
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').each(function () {

            if (($(this).attr("type") == "checkbox" || $(this).attr("type") == "radio") && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            }

            else {
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
        if ($scope.Filter.FilterType == 'RegistrationDate') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
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
        $scope.GetSocietyBasicDetailList();

    }
    /*------------End Paging and Filter-----------------*/


    $scope.UpdateStatus = function () {
        // $scope.searchRequest($scope.GetSocietyBasicDetailList);
        if ($('input[type=radio][name=SetStatus]:checked').val() == 3) {
            $scope.Isliquidator = true;
            $scope.IsReject = false;
        }
        else if ($('input[type=radio][name=SetStatus]:checked').val() == 4) {
            $scope.IsReject = true;
            $scope.Isliquidator = false;
        }
        else {
            $scope.Isliquidator = false;
            $scope.IsReject = false;
        }
    };


    $scope.ActionFilter = function (ActionId) {
        debugger;
        var ActionId = "and Societies.IsActive=" + ActionId
        //  $scope.searchRequest();
        // $scope.GetSocietyBasicDetailList1($scope.DataPaging.searchParameters);

        $scope.ShowSocietyList = true;
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": ActionId };
        $http.post(SiteUrl + "/mis/GetSocietyReport", JSON.stringify({ Details: jsonParameters, SearchType: $scope.SearchType })).
            success(function (data, status) {
                debugger;
                $scope.SocietyBasicDetailList = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching list")
                $(".blockUI").fadeOut("slow");
            });
    };

    $scope.SearchType = "1";
    $scope.ChangeSelction = function () {
        $scope.SearchType = $('input[type=radio][name=rdoSelect]:checked').val();
        if ($('input[type=radio][name=rdoSelect]:checked').val() === '3') {
            $scope.GetSocietyBasicDetailList();
        }
        else {
            $scope.GetSocietyBasicDetailList();
        }

    };

    $scope.GetFlowProcessStatus = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FLOWPROCESSSTATUS&MasterCode=0").
            success(function (data, status) {
                $scope.StatusList = data;
                if ($("#hdnDistrictId").val() != "0") {
                    $("#ddrStatus").hide();//("disabled", "disabled");
                }
                // $scope.BindFY();

            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                //alert('Error while fetching district list data.');
            });
    };
    $scope.GetFlowProcessStatus();
    $scope.SocietyBasicDetailList = [];
    // $scope.LoanApplication.LOAN_ID = '0';
    $scope.GetSocietyBasicDetailList = function () {
        $scope.ShowSocietyList = true;
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/mis/GetSocietyReport", JSON.stringify({ Details: jsonParameters, SearchType: $scope.SearchType })).
            success(function (data, status) {

                $scope.SocietyBasicDetailList = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching list")
                $(".blockUI").fadeOut("slow");
            });
    }
    $scope.GetDistrictUser = function () {
        $http.post(SiteUrl + "/Society/GetUserDistrictId").
            success(function (data, status) {
                if (data.length > 0 && data != 0) {
                    $scope.divStateUser = false;
                }
            }).
            error(function (data, status) {
            });
    };

    $scope.GetHealthCount = function () {
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": 1, "PageSize": 1000000, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/SocietyFinancialHealth/GetSocietyBasicDetail", JSON.stringify({ Details: jsonParameters })).
            success(function (data, status) {

                data = JSON.parse(data.DataList);
                var TotalCount = 0, HealthReportCompleted = 0;
                angular.forEach(data, function (j, i) {
                    if (j.SocietyHealthInserted === '1') {
                        HealthReportCompleted = HealthReportCompleted + 1;
                    }

                    TotalCount = TotalCount + 1;
                });
                console.log(HealthReportCompleted);
                $scope.TotalCount = TotalCount;
                $scope.HealthReportCompleted = HealthReportCompleted;
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while Health Count Records");
                $(".blockUI").fadeOut("slow");
            });

        $(".blockUI").fadeOut("slow");
    };

    $scope.GetDistrictUser();
    $scope.divStateUser = true;
    $scope.GetSocietyBasicDetailList();
    $scope.GetLoanApplicationDetails = function (table, loanId) {
        $scope.ShowSocietyList = true;
        $scope.ShowVoterList = false;
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/PldbLoan/GetLoanApplicationDetails", JSON.stringify({ Table: table, LoanId: loanId })).
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
    $scope.PurposeOfLoanList = [];
    $scope.Member = {
        index: 0,
        Id: "0",
        AADHARNo: "",
        Name: "",
        FatherName: "",
        DOB: "",
        CategoryId: "0",
        MobileNo: "",
        Address: "",
        IsDelete: false
    }



    $scope.LoanAmountDetails = [];
    $scope.LoanAmountDetails.push($scope.LoanAmount);

    $scope.ResetMember = function () {
        $scope.Member = {
            index: 0,
            Id: "0",
            AADHARNo: "",
            Name: "",
            FatherName: "",
            DOB: "",
            CategoryId: "0",
            MobileNo: "",
            Address: "",
            IsDelete: false
        }
    }



    $scope.PreLoanDetails = {
        Id: "0",
        Organization: "",
        LoanAmount: "",
        MonthlyInstallment: "",
        IsDelete: false
    }

    $scope.PreLoanDetailsList = [];
    $scope.PreLoanDetailsList.push($scope.PreLoanDetails);

    $scope.InvestmentList.push(new InvestmentObj());



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
        $scope.MemberRegistration.DOB = '';
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

    $scope.GetLiabilityAssetList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=LIABILITYASSET&MasterCode=0").
            success(function (data, status) {
                $scope.LiabilityAssetList = data;
                $scope.LiabilityList = data.filter(x => x.IsLiability == 'True');
                $scope.AssetList = data.filter(x => x.IsLiability == 'False');
                $scope.InvestmentDetails = [];
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.LiabilityAssetList = [];
                $(".se-pre-con").fadeOut("slow");
            });

        $('.SetCalender').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            minDate: '0'
        });







    };






    $scope.PLDBBranchList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + $scope.MemberRegistration.PLDB_ID).
            success(function (data, status) {
                $scope.PLDBBranches = data;
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
    $scope.UpdateMemberDetails = function (item) {
        $scope.MemberRegistration.OrganizationId = item.OrganizationId;
        $scope.MemberRegistration.Name = item.Name;
        $scope.MemberRegistration.DOB = item.DOB;
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

    $scope.SocietyDetails = [];
    $scope.SubHeadList = [];

    function SubHeads() {
        this.Name = '';
        this.Amount = '';
    }



    $scope.AddSubHead = function () {
        $scope.SubHeadList.push(SubHeads());
    };

    //START : AddNew Row in Investment Details
    $scope.AddNewInvestmentRow = function () {
        $scope.InvestmentList.push(new InvestmentObj());
    };
    //END : AddNew Row in Investment Details


    //START : Remove Investment Details Row
    $scope.RemoveInvestmentRow = function (index) {
        $scope.InvestmentList.splice(index, 1);
    };
    //END : Remove Investment Details Row


    // START : Validate Form
    $scope.ValidateForm = function () {
        var message = '';
        if ($scope.SocietyDetails.AADHARNo === undefined || $scope.SocietyDetails.AADHARNo.length < 12) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter AADHAR No. in 12 digits' : '\n <br /> कृपया 12 अंकों का आधार संख्या दर्ज करें  |';
        }

        if ($scope.SocietyDetails.Name === undefined || $scope.SocietyDetails.Name.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Name' : '\n <br /> कृपया नाम दर्ज करें  |';
        }


        if ($scope.SocietyDetails.FatherName === undefined || $scope.SocietyDetails.FatherName.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Father Name' : '\n <br /> कृपया पिता का नाम दर्ज करें  |';
        }


        if ($('#txtDOB').val() == "" || $('#txtDOB').val() == null) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter DOB' : '\n <br /> कृपया जन्म तिथि दर्ज करें  |';
        }

        if ($scope.SocietyDetails.CategoryId === '0') {
            message += $scope._language === 'en-us' ? '\n <br /> Please choose category.' : '\n <br /> कृपया श्रेणी चुनें।  |';
        }

        if ($scope.SocietyDetails.Address === undefined || $scope.SocietyDetails.Name.Address < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Address' : '\n <br /> कृपया पता दर्ज करें |';
        }

        if ($scope.SocietyDetails.MobileNo === undefined || $scope.SocietyDetails.MobileNo.length < 10) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Mobile No. in 10 digits' : '\n <br /> कृपया 10 अंकों का  मोबाइल नंबर दर्ज करें  |';
        }

        var invaliddata = 0;
        angular.forEach($scope.LiabilityList, function (j, i) {
            if (j.Amount === undefined || j.Amount.length < 1) {
                invaliddata = invaliddata + 1;
            }
        });

        if (invaliddata > 0) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Amount in all liability head(s).' : '\n <br /> कृपया सभी देयता राशि(ओं) में राशि दर्ज करें |';
        }

        var invaliddata2 = 0;
        angular.forEach($scope.AssetList, function (j, i) {
            if (j.Amount === undefined || j.Amount.length < 1) {
                invaliddata2 = invaliddata2 + 1;
            }
        });

        if (invaliddata2 > 0) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Amount in all asset head(s).' : '\n <br /> कृपया सभी एसेट हेड्स में राशि दर्ज करें |';
        }

        var invaliddata3 = 0;
        angular.forEach($scope.InvestmentList, function (j, i) {
            if (j.WhereDid === undefined || j.WhereDid.length < 1 || j.Amount === undefined || j.Amount.length < 1) {
                invaliddata3 = invaliddata3 + 1;
            }
        });

        if (invaliddata3 > 0) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter values for Investment details.' : '\n <br /> कृपया निवेश विवरण के लिए मान दर्ज करें|';
        }

        if (message.length > 5) {

            ShowModelAlert($scope.translation.SocietyFinancialStatus, message);
            return false;
        }
        return true;


    };
    //END : Validate Form

    //START : Save Financial Health Record
    $scope.TXN = '';
    $scope.CCSociety = [];
    //$scope.CCSociety.StatusId = '';
    //$scope.CCSociety.liquidatorName='';
    $scope.SaveFinancialHealthRecord = function () {
        $(".se-pre-con").fadeIn("slow");
        if ($scope.CCSociety.StatusId == undefined || $scope.CCSociety.StatusId == "" || $scope.CCSociety.StatusId == null) {
            alert("Please select Status");
            $(".se-pre-con").fadeOut("slow");
            return;
        }
        else {
            if ($scope.CCSociety.StatusId == '3' || $scope.CCSociety.StatusId == 3) {
                if ($scope.CCSociety.liquidatorName == undefined || $scope.CCSociety.liquidatorName == "" || $scope.CCSociety.liquidatorName == null) {
                    alert("Please Enter liquidator Name");
                    $(".se-pre-con").fadeOut("slow");
                    return;
                }
                else if ($scope.CCSociety.liquidatorPostName == undefined || $scope.CCSociety.liquidatorPostName == "" || $scope.CCSociety.liquidatorPostName == null) {
                    alert("Please Enter liquidator Post Name");
                    $(".se-pre-con").fadeOut("slow");
                    return;
                }
                else if ($scope.CCSociety.liquidatorContactNo == undefined || $scope.CCSociety.liquidatorContactNo == "" || $scope.CCSociety.liquidatorContactNo == null) {
                    alert("Please Enter liquidator Contact No");
                    $(".se-pre-con").fadeOut("slow");
                    return;
                }
                else if ($('#txtliquidatorAppointedDate').val() == "") {
                    alert("Please Enter liquidator Appointed Date");
                    $(".se-pre-con").fadeOut("slow");
                    return;
                } else if ($scope.CCSociety.SSOID == undefined || $scope.CCSociety.SSOID == "" || $scope.CCSociety.SSOID == null || $scope.CCSociety.SSOID == undefined) {
                    alert("Please Enter liquidator SSOID");
                    $(".se-pre-con").fadeOut("slow");
                    return;
                }
            }
            else if ($scope.CCSociety.StatusId == '4' || $scope.CCSociety.StatusId == 4) {
                if ($scope.RejectFileReject == undefined) {
                    alert($scope._language == 'en-us' ? '\nPlease Upload Fle of Rejection(PDF)' : '\nकृपया फ़ाइल अपलोड करें(पीडीएफ) |');
                    $(".se-pre-con").fadeOut("slow");
                    return;
                }
                else if ($('#txtRejectionDate').val() == "") {
                    alert("Please Enter Rejection Date");
                    $(".se-pre-con").fadeOut("slow");
                    return;
                }
            }
        }

        AadhaarAuth("", $http, "OTP", true, function (data) {
            //$scope.LoanGuarantor[index].Name = data.Name;
            //$scope.LoanGuarantor[index].FatherName = data.FName;
            //$scope.LoanGuarantor[index].Age = data.Age;
            //$scope.LoanGuarantor[index].Address = data.Address;
            //$scope.LoanGuarantor[index].MobileNo1 = data.Phone;
            //$scope.LoanGuarantor[index].IsValidate = true;
            //$scope.LoanGuarantor[index].DOB = data.DOB;
            //$scope.$apply();
            $scope.UpdateCCSStatus();
        });

        ////////////$http.get("../Society/SendOtpToAadhar").
        ////////////    success(function (data, status) {
        ////////////        $(".se-pre-con").fadeOut("slow");
        ////////////        if (data.indexOf('NO#') >= 0) {
        ////////////            alert("Due to some issue OTP not send");
        ////////////            location.reload();
        ////////////            //window.location = '../ApplicationFlow/ApplicationFlow';
        ////////////        } else {
        ////////////            if (data != "LOGOUT") {
        ////////////                // alert(data);
        ////////////                $scope.TXN = data;
        ////////////                alert(_language == 'en-us' ? 'OTP has been sent to aaddhaar mobile number.\n' : 'ओटीपी को मोबाइल नंबर पर भेजा गया है|\n');
        ////////////                $scope.ShowOTPDiv = true;
        ////////////            }
        ////////////            else {
        ////////////                $(".se-pre-con").fadeOut("slow");
        ////////////                window.location = 'https://sso.rajasthan.gov.in/signin';
        ////////////            }
        ////////////        }
        ////////////    }).
        ////////////    error(function (data, status) {
        ////////////        $(".se-pre-con").fadeOut("slow");
        ////////////        alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
        ////////////    });

        //////////////////////////// start old save data
        //if (!$scope.ValidateForm()) {
        //    $(".se-pre-con").fadeOut("slow");
        //    return;
        //}

        //var tempAssetList = [];

        //angular.forEach($scope.AssetList, function (j, i) {

        //    tempAssetList.push({
        //        "AssetLiabilityId": j.LiabilityAssetMasterId, "Amount": j.Amount, "IsLiability": false, "SocietyId": $scope.SocietyId
        //    });
        //});

        //var tempLiabilityList = [];

        //angular.forEach($scope.LiabilityList, function (j, i) {
        //    tempLiabilityList.push({
        //        "AssetLiabilityId": j.LiabilityAssetMasterId, "Amount": j.Amount, "IsLiability": true, "SocietyId": $scope.SocietyId
        //    });
        //});



        //var model = {
        //    "AADHARNo": $scope.SocietyDetails.AADHARNo,
        //    "Name": $scope.SocietyDetails.Name,
        //    "FatherName": $scope.SocietyDetails.FatherName,
        //    "DOB": $('#txtDOB').val(),
        //    "CategoryId": $scope.SocietyDetails.CategoryId,
        //    "Address": $scope.SocietyDetails.Address,
        //    "MobileNo": $scope.SocietyDetails.MobileNo,
        //    "SocietyId": $scope.SocietyId,
        //    "AssetList": tempAssetList,
        //    "LiabilityList": tempLiabilityList,
        //    "InvestmentDetails": $scope.InvestmentList
        //};



        //$http.post(SiteUrl + "/SocietyFinancialHealth/SaveFinancialHealthRecord/", { "model": model }).
        //    success(function (data, status) {
        //        if (parseInt(data, 10) > 0) {
        //            $(".se-pre-con").fadeOut("slow");
        //            alert(_language === 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया।');
        //            window.location.reload();
        //        }
        //        else if (parseInt(data, 10) === -1) {
        //            $(".se-pre-con").fadeOut("slow");
        //            ShowModelAlert($scope.translation.SocietyFinancialStatus, _language === 'en-us' ? 'Error!!!' : 'त्रुटि !!!', function () { });
        //        }
        //        else if (parseInt(data, 10) === -2) {
        //            $(".se-pre-con").fadeOut("slow");
        //            ShowModelAlert($scope.translation.SocietyFinancialStatus, _language === 'en-us' ? 'Duplicate data. No action performed.' : 'डुप्लिकेट डेटा। कोई कार्रवाई नहीं की गई।', function () { });
        //        }
        //    }).
        //    error(function (data1, status) {
        //    $(".se-pre-con").fadeOut("slow");
        //});


        ////////////////////////// end old save data





        //$(".blockUI").show("slow");
        //var formData = new FormData();
        //var members = [];
        //$scope.CEOMembers.forEach(function (itm) {
        //    members.push({
        //        Id: itm.Id,
        //        AadharNo: itm.AADHARNo,
        //        Name: itm.Name,
        //        FatherName: itm.FatherName,
        //        DOB: itm.DOB,
        //        CategoryId: itm.CategoryId,
        //        Address: itm.Address,
        //        MobileNo: itm.MobileNo,
        //        IsDelete: itm.IsDelete
        //    });
        //});

        //var applicaionDetails = {
        //    load_id: $scope.LoanApplicationId, pldb_id: $scope.MemberRegistration.PLDB_ID, branch_id: $scope.MemberRegistration.Branch_ID,
        //    Members: members,
        //    TotalMembers: LoanAmount,
        //    LoanProperty: LoanProperty,
        //    PreLoan: $scope.PreLoanDetailsList
        //};

        //formData.append("LiabilityList", JSON.stringify($scope.AssetList));
        //formData.append("LiabilityList", JSON.stringify($scope.LiabilityList));
        //formData.append("InvestmentDetailsList", JSON.stringify($scope.InvestmentDetailsList));
        //formData.append("CEOMembers", JSON.stringify($scope.CEOMembers));
        //formData.append("SocietyIdEnc", $scope.CEOMembers);


        //$http.post(SiteUrl + "/SocietyFinancialHealth/SaveFinancialHealthRecord", formData,
        //    {
        //        withCredentials: true,
        //        headers: { 'Content-Type': undefined },
        //        transformRequest: angular.identity
        //    }).
        //    success(function (data, status) {
        //        $(".blockUI").hide("slow");
        //        if (data.Result == "OK") {
        //            ShowOkMessage("", $scope._language == 'en-us' ? "Loan application has been submitted successfully." : "ऋण आवेदन सफलतापूर्वक जमा कर लिया गया है|", function () {
        //                location.href = "../PldbLoan/LoanApplication";
        //            });
        //        } else {
        //            ShowOkMessage("", $scope._language == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें| ", function () {

        //            });
        //        }
        //    }).
        //    error(function (data, status) {
        //        $(".blockUI").hide("slow");
        //    });


    }
    //END : Save Financial Health Record

    //START : Save Financial Health Record

    $scope.UpdateCCSStatus = function () {
        var Statusdate = "";
        if ($scope.CCSociety.StatusId == '3' || $scope.CCSociety.StatusId == 3) {
            Statusdate = $('#txtliquidatorAppointedDate').val().split("/").reverse().join("-");
        }
        else if ($scope.CCSociety.StatusId == '4' || $scope.CCSociety.StatusId == 4) {
            Statusdate = $('#txtRejectionDate').val().split("/").reverse().join("-");
        }
        var formData = new FormData();
        formData.append("RejectFile", $scope.RejectFileReject);
        formData.append("OTP", $scope.CCSociety.OTP);
        formData.append("TXN", $scope.TXN);
        formData.append("SocietyIDEnc", $scope.SocietyId);
        formData.append("StatusId", $scope.CCSociety.StatusId);
        formData.append("liquidatorName", $scope.CCSociety.liquidatorName);
        formData.append("Statusdate", Statusdate);
        formData.append("liquidatorPostName", $scope.CCSociety.liquidatorPostName);
        formData.append("liquidatorContactNo", $scope.CCSociety.liquidatorContactNo);
        formData.append("liquidatorSSOID", $scope.CCSociety.SSOID);
        formData.append("SocietyRegiNo", $scope.RegistrationNoLocal);
        //if ($scope.CCSociety.OTP != '') {
        $(".se-pre-con").fadeIn("slow");
        //$http.post(SiteUrl + "/Society/VlidateOTPOfAadhar", { "OTP": $scope.CCSociety.OTP, "TXN": $scope.TXN, "SocietyIDEnc": $scope.SocietyId, "StatusId": $scope.CCSociety.StatusId, "liquidatorName": $scope.CCSociety.liquidatorName, "liquidatorAppointedDate": Statusdate, "liquidatorPostName": $scope.CCSociety.Statusdate, "liquidatorContactNo": $scope.CCSociety.liquidatorContactNo ,""}).
        $http.post(SiteUrl + "/Society/UpdateCCSStatus", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data != "Success") {
                    alert(data);
                }
                else if (data == "Success") {
                    alert('Society Updated Successfully');
                    location.reload();
                }
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert(_language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                location.reload();
            });
        //}
        //else {
        //    alert(_language == 'en-us' ? 'Please enter OTP\n' : 'कृपया OTP चुने|\n');
        //}
    };
    //END : Save Financial Health Record

    //START : Show Application View Form
    $scope.ShowSocietyHealthData = function (SocietyId) {
        $(".se-pre-con").fadeIn("slow");

        $scope.dvApplicationForm = false;
        $scope.divLoanApplicationList = false;
        $scope.divSocietyHealthReport = true;
        $(".se-pre-con").fadeOut("slow");



        $http.post(SiteUrl + "/SocietyFinancialHealth/ShowSocietyHealthRecord/", { "SocietyIdEnc": SocietyId }).
            success(function (data, status) {
                $scope.SocietyHealthRecord = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data1, status) {
                $scope.SocietyHealthRecord = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
    };
    //START : Show Application View Form
    $scope.UrbanDiv = false;
    $scope.RuralDiv = false;

    $scope.RuralUrbanBlock = function (V) {
        var SelectedVal = $scope.IsRural == 'false' ? false : true;
        if (V != 1) {
     
            $scope.CITY_ID = "0";          
            $scope.BLOCK_ID = "0";     
        }
        if (SelectedVal == true || SelectedVal == true) {
            $scope.BLOCK_ID = "0";    
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

    $scope.GetCityListByCityTypes = function (id) {
       
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $scope.SocietyRegisteredOffice.DISTRICT_ID).
                success(function (data, status) {
                    $scope.CityListdata = data;
                }).
                error(function (data, status) {
                    $scope.CityListdata = [];
                });       

      

        // $scope.RuralUrbanBlock();
    };

    $scope.GetDistrictList = function (id) {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + id).
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    }

    $scope.GetDistrictList(0);

    $scope.ResetDataForAddress = function (val, Level) {
        if (val == 1) {
            if (Level < 3) {
                $scope.IsRural = undefined;
                $scope.CityListdata = [];
                $scope.Blocksdata = [];
                $scope.GramPanchayats = [];
                $scope.Villages = [];
                $scope.Ward = [];
            }
        }        
    }

    $scope.SowApplicationGrid = function () {
        window.location = '../Pldb/PLDBSchemeList';
    }


    $scope.BackToList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.dvApplicationForm = false;
        $scope.divLoanApplicationList = true;
        $scope.divSocietyHealthReport = false;
        $(".se-pre-con").fadeOut("slow");
    };
    $scope.SocietyNameLocal = '';
    $scope.RegistrationNoLocal = '';

    $scope.UploadSocietyRejectionFile = function (file) {
        $scope.RejectFileReject = file[0];
    };
    $scope.ApplicationEdit = function (societyIdEnc, DISTRICT_ID, SocietyName, RegistrationNo) {
        $('#txtliquidatorAppointedDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $('#txtRejectionDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $scope.SocietyNameLocal = SocietyName;
        $scope.RegistrationNoLocal = RegistrationNo;
        $scope.dvApplicationForm = true;
        $scope.divLoanApplicationList = false;
        $scope.ShowAddNewApplication = false;
        $scope.ShowOTPDiv = false;

        $scope.SocietyId = societyIdEnc;
    };

    $scope.ShowStatusDetail = function (StatusId, Col1, Col2, Col3, Col4) {
        $scope.LiquidatorPopupDiv = false;
        $scope.RejectionPopupDiv = false;
        if (StatusId == 2) {
            $scope.RejectionPopupDiv = true;
            //  alert("Status:" + StatusId + ", RejectionDate: " + Col1 + ", RejectionFileNamePath:" + Col2);
            $scope.RejectionDate = Col1;
            $scope.RejectionFileNamePath = Col2;
        }
        else {
            $scope.LiquidatorPopupDiv = true;
            $scope.LiquidatorName = Col1;
            $scope.AppointedDate = Col2;
            $scope.PostName = Col3;
            $scope.ContactNo = Col4;
            //  alert("Status:" + StatusId + ", Liquidator Name:" + Col1 + ", Appointed Date:" + Col2 + ", Post Name:" + Col3 + ", Contact No:" + Col4);
        }
    };


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
        $http.get(SiteUrl + "/PldbLoan/GetLovMaster?Id=5").
            success(function (data, status) {
                $scope.FarmerCategoryList = data;
            }).
            error(function (data, status) {
                $scope.FarmerCategoryList = [];
            });
    }
    $scope.GetPurposeOfLoan = function () {
        $http.get(SiteUrl + "/PldbLoan/GetLovMaster?Id=6").
            success(function (data, status) {
                $scope.PurposeOfLoanList = data;
            }).
            error(function (data, status) {
                $scope.PurposeOfLoanList = [];
            });
    }



    // START : Get Category  List
    // START : Get Category  Listsavef
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



    $scope.ValidateAddMember = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblLandMemberDetail").find('input[type="text"]').removeClass("notValid");
        $("#tblLandMemberDetail").find('select').removeClass("notValid");


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
            if (item.Name == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Name' : 'कृपया नाम दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Name.' : '<br />कृपया नाम दर्ज करें।';
                }
                $("#tblLandMemberDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(1).addClass("notValid");
            }
            if (item.FatherName == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Father Name' : 'कृपया पिता का नाम दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Father Name.' : '<br />कृपया पिता का नाम दर्ज करें।';
                }
                $("#tblLandMemberDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(2).addClass("notValid");
            }
            if (item.DOB == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Age' : 'कृपया उम्र दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Age.' : '<br />कृपया उम्र दर्ज करें।';
                }
                $("#tblLandMemberDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(3).addClass("notValid");
            }

            if (item.FarmerCategory == '0') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Select Farmer Category' : 'कृपया किसान की श्रेणी का चयन करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Select Farmer Category.' : '<br />कृपया किसान की श्रेणी का चयन करें।';
                }
                $("#tblLandMemberDetail").find("tr").eq(index + 1).find('select').eq(0).addClass("notValid");
            }

            if (item.CategoryId == '0') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Select Category' : 'कृपया वर्ग का चयन करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Select Category.' : '<br />कृपया वर्ग का चयन करें।';
                }
                $("#tblLandMemberDetail").find("tr").eq(index + 1).find('select').eq(1).addClass("notValid");
            }

            if (item.Village == '0') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Select Village' : 'कृपया ग्राम का चयन करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Select Village.' : '<br />कृपया ग्राम का चयन करें।';
                }
                $("#tblLandMemberDetail").find("tr").eq(index + 1).find('select').eq(2).addClass("notValid");
            }

            if (item.VillagePanchayat == '0') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Select Gram Panchayat' : 'कृपया ग्राम पंचायत का चयन करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Select Gram Panchayat.' : '<br />कृपया ग्राम पंचायत का चयन करें।';
                }
                $("#tblLandMemberDetail").find("tr").eq(index + 1).find('select').eq(3).addClass("notValid");
            }

            if (item.Tehsil == '0') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Select Tehsil' : 'कृपया तहसील का चयन करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Select Tehsil.' : '<br />कृपया तहसील का चयन करें।';
                }
                $("#tblLandMemberDetail").find("tr").eq(index + 1).find('select').eq(4).addClass("notValid");
            }

            if (item.MobileNo == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Mobile Number' : 'कृपया मोबाइल नंबर दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '\n <br /> Please Enter Mobile Number' : '\n <br /> कृपया मोबाइल नंबर दर्ज करें |';
                }
                $("#tblLandMemberDetail").find("tr").eq(index + 1).find('input[type="text"]').eq(4).addClass("notValid");
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
    $scope.GetCategoryList();
    // $scope.LandDetailsGSS.push(new GSSLandDetailEntity());
    $scope.GetFarmerCategory();
    $scope.GetPurposeOfLoan();
    $scope.CEOMembers = [];
    $scope.CEOMembers.push($scope.Member);
    $scope.ShowLoanApplication = function () {
        if ($scope.ValidateAddMember()) {
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
        debugger;
        if (type == 'Village') {
            var data = list.find(x => x.VILLAGE_ID === id);
            return _language == 'en-us' ? data.VILLAGE_ENG : data.VILLAGE_MANGAL;
        } else if (type == 'Tehsil') {
            var data = list.find(x => x.BLOCK_ID === id);
            return _language == 'en-us' ? data.BLOCK_ENG : data.BLOCK_MANGAL;
        } else if (type == 'District') {
            var data = list.find(x => x.PldbId === id);
            return _language == 'en-us' ? data.PLDBName : data.PLDBName;
        } else if (type == 'Village') {
            var data = list.find(x => x.VILLAGE_ID === id);
            return _language == 'en-us' ? data.VILLAGE_ENG : data.VILLAGE_MANGAL;
        } else if (type == 'Village') {
            var data = list.find(x => x.VILLAGE_ID === id);
            return _language == 'en-us' ? data.VILLAGE_ENG : data.VILLAGE_MANGAL;
        }
    }
    $scope.GetLiabilityAssetList();
    $scope.GetHealthCount();


    $scope.GetSocietyTypeList = function (actid) {
        $(".blockUI").fadeIn("slow");
        //$scope.SocietyChangeModel.ActId = actid;
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=" + actid).
            success(function (data, status) {
                $scope.SocietyTypeList = data;
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.SocietyTypeList = [];
                $(".blockUI").fadeOut("slow");
            });
    };

    $scope.GetSocietyTypeList(0);

    // GET Society Class
    $scope.GetSocietyClass = function () {
        $(".blockUI").fadeIn("slow");
        // $scope.SocietyChangeModel.SocietyTypeId = id;
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYCLASS&MasterCode=" + $scope.SocietyChangeModel.SocietyTypeId).
            success(function (data, status) {
                $scope.SocietyClassList = data;
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.SocietyClassList = [];
                $(".blockUI").fadeOut("slow");
            });

    }
    //END

    // START : Get SocietySubClassList
    $scope.GetSubClass = function () {
        $(".blockUI").fadeIn("slow");
        $http.get(SiteUrl + "/Master/SubClassList/?ClassId=" + $scope.SocietyChangeModel.ClassId + "&SocietyTypeId=" + $scope.SocietyChangeModel.SocietyTypeId).
            success(function (data, status) {
                $scope.SocietySubClassList = data;
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.SocietySubClassList = [];
                $(".blockUI").fadeOut("slow");
            });
        $scope.SocietyChangeModel.SocietyCategoryId = '0';
    };
    // END

    // START : Get SocietyCategoryList
    $scope.GetSocietyCategory = function () {
        $(".blockUI").fadeIn("slow");
        $http.get(SiteUrl + "/Master/SocietyCategoryList/?ClassId=" + $scope.SocietyChangeModel.ClassId + "&SubClassId=" + $scope.SocietyChangeModel.SubClassId).
            success(function (data, status) {
                $scope.SocietyCategoryList = data;
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                $scope.SocietyCategoryList = [];
            });
    };
    // END : Get Sub Class

});
