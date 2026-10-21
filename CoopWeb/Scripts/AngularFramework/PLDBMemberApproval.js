var PLDBMemberRegistrtionModule = angular.module('PLDBMemberRegistrtionMod', ['angularUtils.directives.dirPagination']);
PLDBMemberRegistrtionModule.controller('PLDBMemberRegistrtionCnt', function ($scope, $http, $rootScope) {

    /*------------Paging and Filter-----------------*/
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 20,
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
  
    $scope.showSearchText = true;
    $scope.SearchText = "";
    $scope.Filter = [];
    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.showSearchText = false;
      
        } 
    
    $scope.ClearSearch = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FilterType = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.GetElectionSocietyListData();
    }
    
    /*------------End Paging and Filter-----------------*/


    $scope.ShowPLDB = false;
    $scope.ShowBranch = false;

    $scope.MemberRegistration = {
        PLDB_ID: "", BLOCK_ID: ""
    }
    // START : Get List of PLDB
    $scope.PLDBList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDB&MasterCode=-1").
            success(function (data, status) {
                $scope.PLDB = data;
            }).
            error(function (data, status) {
                $scope.PLDB = [];
            });
    };
    //END : Get List of PLDB

    // START : Get List of PLDB Branch
    $scope.PLDBBranchList = function () {
        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLDBBRANCH&MasterCode=" + $scope.MemberRegistration.PLDB_ID).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.PLDBBranches = data;
                $scope.MemberRegistration.Branch_ID = '';
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.PLDBBranches = [];
            });
    };
//END : Get List of PLDB Branch

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


    $scope.divUserRegistration = true;
    $scope.currentPage = 1;
    $scope.pageSize = 50;
    $scope.MemberDatail = false;
    $scope.DairyDatail = false;
    $scope.divViewDairyFormShow = false;
    $scope.divParentDataSelection = true;
    $scope.PLDBMemberDatail = false;
    $scope.divAADHARVerificationStep_1 = false;
    $scope.divAADHARVerificationStep_2 = false;
    $scope.showTitle = false;
    $scope.divViewApplicationDetailShow = false;
    $scope.showPLDBMemberRequestList = true;
    $scope.divAADHARVerificationMemberCertStep_1 = false;
    $scope.divNoData = false;
    $scope.divMemberApprovalList = false;

    // START : Verify AADHAR and Generate E-signed Certificate
    $scope.AADHARVerifyForCertificateIssue = function () {

        if ($scope.MemberCertificate.EnterOTP == undefined || $scope.MemberCertificate.EnterOTP.length != 6) {
            var msg = _language == 'en-us' ? 'Please enter 6 digit OTP' : '6 अंकों की ओटीपी दर्ज करें';
            alert(msg);
            return;
        }

        $http.post(SiteUrl + "/SocietyMemberRegistration/ValidateOTPForMembershipGenerate", { "otp": $scope.MemberCertificate.EnterOTP, "txn": $scope.MemberCertificate.txn, "_RequestIdEnc": $scope.CertificateRequestId }).
            success(function (data, status) {
                if (data[0].Column1 == "Failed") {
                    alert(data[0].Column1 + ': Due To' + data[0].Column2);
                    return;
                }
                else if (data[0].Column1 == "Success") {
                    alert('Certificate issued successfully.');
                    window.location = '../SocietyMemberRegistration/MemberApproval';
                }
                else {
                    return;
                }
                window.location = '../SocietyMemberRegistration/MemberApproval';
            }).
            error(function (data, status) {
                $scope.SocietyRegdatanew = [];
            });

    }
    // END: Verify AADHAR and Generate E-signed Certificate

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
                    alert("Error while translation-En");
                });
        }
        else {
            $http.get(SiteUrl + "/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation-Hi");
                });
        }

        $scope.GetPLDBMemberApprovalData = function () {
            $(".se-pre-con").fadeIn("slow");
            //$scope.PLDBMemberDataForApproval = [];
            var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
            $http.post(SiteUrl + "/SocietyMemberRegistration/GetPLDBMembersApprovalByUserRights", JSON.stringify({ Details: jsonParameters, Level: "0" })).
                //$http.post(SiteUrl + "/SocietyMemberRegistration/GetPLDBMembersByUserRights").
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    $scope.PLDBMemberDataForApproval = JSON.parse(data.DataList);
                    $rootScope.DataPagingDetails = data.DataPaging;
                    $rootScope.SetDataPaging();
                }).
                error(function (data, status) {
                    $scope.MemberDataForApproval = [];
                    $(".se-pre-con").fadeOut("slow");
                });

            //$http.post(SiteUrl + "/SocietyMemberRegistration/GetPLDBMembersByUserRights", { "_ActionId": "2" }).
            //    success(function (data, status) {

            //        if (data.length > 0) {
            //            $scope.showPLDBMemberRequestList = true;
            //            $scope.PLDBMemberDataForApproval = data;
            //            $scope.divGrid = true;
            //        }
            //        else {
            //            $scope.divNoData = true;
            //            $scope.showPLDBMemberRequestList = false;
            //            $scope.NoDataFound = _language === 'en-us' ? 'No Application Submitted yet.' : 'अभी तक कोई आवेदन प्रस्तुत नहीं किया गया है।';
            //        }

            //        $(".se-pre-con").fadeOut("slow");
            //    }).
            //    error(function (data, status) {
            //        $scope.MemberDataForApproval = [];
            //        $(".se-pre-con").fadeOut("slow");
            //    });
        }

        if (window.location.href.indexOf('MemberApproval') > 0) {
            $scope.GetPLDBMemberApprovalData();
        }
        else {
            $scope.GetMemberListForUsers();
        }



        //$scope.GetOrganizationList();

        //$scope.GetDistrictWithoutDivisionList();
        //var url = window.location.href.split('?');
        //if (url.length > 1) {
        //    var url2 = url[1].substr(17, url[1].toString().length);
        //    if (url2.indexOf('Error') > 0) {
        //        alert('some error occurred Please Try after some time');
        //        window.location = '../SocietyMemberRegistration/SocietyMemberRegistration';
        //    }
        //    else
        //        $scope.GetMemberDetailsList(url2);
        //}
        if ($('#EUserType').val() == 'C') {
            //    $scope.divGrid = true;
            //    $scope.divViewShow = false;
            //    $scope.divRegistration = false;
            $scope.IsCitizen = true;
            //    $scope.GetSocietyMemberData();
            //    $scope.GetMemberListForApproval();
        }
        else {
            //    $scope.divViewShow = false;
            //    $scope.divRegistration = true;
            //    $scope.divGrid = false;
            $scope.IsCitizen = false;
            //    $scope.GetMemberListForApproval();
        }
        $scope.BackBtnLast = true;
    };
    // END : Code for Language label setting

    $scope.GetSocietyMemberData = function () {
        $(".se-pre-con").fadeIn("slow");

        $scope.SocietyMemberData = [];
        $http.get(SiteUrl + "/SocietyMemberRegistration/GetPLDBUserRegistrationDetails?ApplicationIdEnc=" + "").
            success(function (data, status) {
                $scope.SocietyMemberData = data;
            }).
            error(function (data, status) {
                $scope.SocietyMemberData = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    $scope.GetMemberListForUsers = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.SocietyMemberData = [];


        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/SocietyMemberRegistration/GetPLDBMembersByUser", JSON.stringify({ Details: jsonParameters, Level: "0" })).
        //$http.post(SiteUrl + "/SocietyMemberRegistration/GetPLDBMembersByUserRights").
            success(function (data, status) {


                $scope.PLDBMemberData = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();

                //if (data.length > 0) {
                    //$scope.PLDBMemberData = data;
                    $scope.divGrid = true;
                //}
                //else {
                //    $scope.divNoData = true;
                //    $scope.NoDataFound = _language === 'en-us' ? 'No Application Submitted yet.' : 'अभी तक कोई आवेदन प्रस्तुत नहीं किया गया है।';
                //}

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.MemberDataForApproval = [];
                $(".se-pre-con").fadeOut("slow");
            });

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





    $scope.ShowMemberDetail = function () {
        if ($scope.MemberRegistration.RegistrationType == '1') {
            $scope.MemberDatail = true;
            $scope.DairyDatail = false;
        }
        else {
            $scope.MemberDatail = false;
            $scope.DairyDatail = true;
        }

        $scope.divParentDataSelection = false;
        $scope.divViewShow = false;
        $scope.divGrid = false;
        $scope.divRegistration = true;
        $scope.BackBtnLast = false;
        $scope.GetVillageList();
    };

    $scope.ShowForm = function () {
        $("#divViewApplicationDetailShow").modal("hide");
        $scope.divNoData = false;
        $scope.showPLDBMemberRequestList = false;
        $scope.divAADHARVerificationStep_1 = false;
        $scope.divAADHARVerificationStep_2 = false;
        $scope.MemberRegistration.EnterOTP = '';
        $scope.divUserRegistration1 = false;
        $scope.showTitle = false;
        $scope.MemberRegistration.PLDB_ID = '0';
        $scope.PLDBBranches = [];
        $scope.MemberRegistration.Branch_ID = '0';
        $scope.divParentDataSelection = true;
        $scope.DairyDatail = false;
        $scope.MemberDatail = false;
        $scope.divViewDairyFormShow = false;
        $scope.divViewShow = false;
        $scope.divRegistration = true;
        $scope.divGrid = false;
        $scope.divViewApplicationDetailShow = false;
        $scope.ChangeType();
        //$(".se-pre-con").fadeOut("slow");

    };

    $scope.StatusActionList = [];
    $scope.GetAllStatus = function () {
        $http.post(SiteUrl + "/SocietyMemberRegistration/GetPLDBMembershipStatus", { StatusId: 0 }).
            success(function (data2, status) {
                $scope.StatusActionList = data2;
            }).
            error(function (data, status) {
                $scope.StatusActionList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    }
    $scope.GetAllStatus();

    $scope.TackActionShow = false;
    $scope.ViewMemberApplication = function (ReqIdEnc,status) {
        $(".se-pre-con").fadeIn("slow");
        $scope.TackActionShow = status;

        $http.get(SiteUrl + "/SocietyMemberRegistration/GetPLDBUserRegistrationDetails?ApplicationIdEnc=" + ReqIdEnc).
            success(function (data, status) {
                $scope.divGrid = false;
                $scope.ViewApplication = data[0];
                $scope.MemberApplicationId = ReqIdEnc;
                $scope.divViewApplicationDetailShow = true;
                
                $("#divViewApplicationDetailShow").modal("show");

                $http.post(SiteUrl + "/SocietyMemberRegistration/GetPLDBMembershipStatus", { StatusId: data[0].ActionStatus }).
                    success(function (data2, status) {
                        $scope.NextActionList = data2;
                    }).
                    error(function (data, status) {
                        $scope.MemberDetailsList = [];
                        $(".se-pre-con").fadeOut("slow");
                    });
                $(".se-pre-con").fadeOut("slow");


                //$scope.MemberDetailsList = data[0];
                //$scope.MemberRegistration = data[0];
                //$scope.divViewShow = true;
                //$scope.divRegistration = false;
                //$scope.divGrid = false;


                if (data[0].ResponseString != null && data[0].ResponseString != "") {
                    var temp = [];
                    temp.push(JSON.parse(data[0].ResponseString));
                    if (temp.length > 0) {
                        if (data[0].KioskID == 'SSO' ? (temp[0].RESPONSEMESSAGE.indexOf('Success') > 0) : (temp[0].MSG.indexOf('Success') > 0)) {
                            $scope.IsPaymentDone = true;

                            $scope.TRANSACTIONID = temp[0].TRANSACTIONID;
                            $scope.ReceiptNo = temp[0].RECEIPTNO;
                            $scope.BANK_NAME = temp[0].BANK_NAME;
                            $scope.BANK_ACC_NO = temp[0].BANK_ACC_NO;
                            if (data[0].KioskID == 'SSO') {
                                $scope.IsCityzen = true;
                                $scope.IsKiosk = false;
                                $scope.Fee = temp[0].PAIDAMOUNT;
                                $scope.RESPONSEMESSAGE = temp[0].RESPONSEMESSAGE;
                            }
                            else {
                                $scope.RESPONSEMESSAGE = temp[0].MSG;
                                $scope.Fee = temp[0].TRANSAMT;
                                $scope.IsCityzen = false;
                                $scope.IsKiosk = true;
                            }

                        }


                        if (data[0].OldMembershipNo != '') {
                            $scope.IsPaymentDone = false;
                        }
                    }
                }
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.MemberDetailsList = [];
                $(".se-pre-con").fadeOut("slow");
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // START : Get All All List
    $scope.GetMemberDetailsList = function (IdEnc) {

        $http.get(SiteUrl + "/SocietyMemberRegistration/GetOrganizationUserRegistrationDetails?ApplicationIdEnc=" + IdEnc).
            success(function (data, status) {

                if (data.SocietyMemberRegistration[0].ApplicationTypeId == 2) {
                    $scope.divViewDairyFormShow = true;
                    $scope.divViewShow = false;
                    $scope.MemberRegistration = data.SocietyMemberRegistration[0];
                }
                else {

                    $scope.MemberRegistration = data.SocietyMemberRegistration[0];
                    $scope.LandDetailsGSS = data.SocietyMemberLandDetail;
                    $scope.SownCropDetailGSS = data.SocietyMemberSownCropDetail;
                    $scope.CommercialLoanDetail = data.SocietyMemberOtherBankLoanDetail;
                    $scope.divViewDairyFormShow = false;
                    $scope.divViewShow = true;

                }


                //$scope.MemberDetailsList = data[0];
                //$scope.MemberRegistration = data[0];
                //$scope.divViewShow = true;
                //$scope.divRegistration = false;
                //$scope.divGrid = false;
                return;

                if (data[0].ResponseString != null && data[0].ResponseString != "") {
                    var temp = [];
                    temp.push(JSON.parse(data[0].ResponseString));
                    if (temp.length > 0) {
                        if (data[0].KioskID == 'SSO' ? (temp[0].RESPONSEMESSAGE.indexOf('Success') > 0) : (temp[0].MSG.indexOf('Success') > 0)) {
                            $scope.IsPaymentDone = true;

                            $scope.TRANSACTIONID = temp[0].TRANSACTIONID;
                            $scope.ReceiptNo = temp[0].RECEIPTNO;
                            $scope.BANK_NAME = temp[0].BANK_NAME;
                            $scope.BANK_ACC_NO = temp[0].BANK_ACC_NO;
                            if (data[0].KioskID == 'SSO') {
                                $scope.IsCityzen = true;
                                $scope.IsKiosk = false;
                                $scope.Fee = temp[0].PAIDAMOUNT;
                                $scope.RESPONSEMESSAGE = temp[0].RESPONSEMESSAGE;
                            }
                            else {
                                $scope.RESPONSEMESSAGE = temp[0].MSG;
                                $scope.Fee = temp[0].TRANSAMT;
                                $scope.IsCityzen = false;
                                $scope.IsKiosk = true;
                            }

                        }
                    }
                }

                //$scope.MemberDetailsList = data[0];
                //$scope.MemberRegistration = data[0];
                //$scope.divViewShow = true;
                //$scope.divRegistration = false;
                //$scope.divGrid = false;
            }).
            error(function (data, status) {
                $scope.MemberDetailsList = [];
            });
        $(".se-pre-con").fadeOut("slow");
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
    $scope.SaveUser = function () {

        if (!$scope.ValidatePLDBMemberRegistration()) {
            return false;
        } else {


            $scope.MemberRegistration["MembershipFee"] = 100;
            $scope.MemberRegistration["ContributionAmt"] = 11;

            $scope.MemberRegistration["BLOCK_ID"] = $scope.MemberRegistration.Branch_ID;
            $scope.MemberRegistration["DISTRICT_ID"] = $scope.MemberRegistration.PLDB_ID;
            $scope.MemberRegistration["OrganizationtypeId"] = "3";
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/SocietyMemberRegistration/SavePLDBUserRegistrationDetails", { _Entity: $scope.MemberRegistration }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    
                    if (data != "-1" && data != "2") {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "Member Registration has been submitted successfully.<br/><br/>Membership No.:-<b>" + data + "</b>" : "सदस्य पंजीकरण सफलतापूर्वक जमा कर लिया गया है|<br/><br/>सदस्यता संख्या:-<b>" + data + "</b>", function () {
                            location.href = "../SocietyMemberRegistration/MemberApproval";
                        });
                    } else {
                        ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें| ", function () {

                        });
                    }
                    //else {
                    //    ShowOkMessage("Error", $scop._language == 'en-us' ? "User Already Exists." : "सदस्य पहलॆ से मौजूद है|", function () {
                    //    }
                        }).
                            error(function (data, status) {
                                $(".se-pre-con").fadeOut("slow");
                                ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें| ", function () {
                                });
                            });

                   
                }
                    
    };
// START : Save User

$scope.ApplicationEdit = function () {
    $scope.divAADHARVerificationStep_1 = false;
    $scope.divAADHARVerificationStep_2 = false;
    $scope.divParentDataSelection = false;
    $scope.PLDBMemberDatail = true;
    $scope.divViewShow = false;
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
$scope.GetQualificationList = function () {
    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=EDUCATIONQUALIFICATION&MasterCode=-1").
        success(function (data, status) {
            $scope.EducationQualification = data;
        }).
        error(function (data, status) {
            $scope.EducationQualification = [];
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

// START : Get Category  List
$scope.GetVillageList = function () {
    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=VILLAGE&MasterCode=" + $scope.MemberRegistration.GP_ID).
        success(function (data, status) {
            $scope.VillageList = data;
        }).
        error(function (data, status) {
            $scope.VillageList = [];
        });
}
// END: Get Category List

//START : Save GSS data()
$scope.SaveGSSData = function () {

    if ($scope.MemberRegistration.RegistrationType == '1') {
        if (!$scope.ValidateGSSData()) {
            return;
        }
        $scope.MemberDatail = false;
        $scope.divViewShow = true;
    }
    else {
        if (!$scope.ValidateDairyFormData()) {
            return;
        }
        $scope.divViewDairyFormShow = true;
        $scope.DairyDatail = false;
    }


    $scope.MemberRegistration["GSSLandDetails"] = [];
    $scope.MemberRegistration["SownCropDetails"] = [];
    $scope.MemberRegistration["OtherBankLoanDetails"] = [];

    if ($scope.LandDetailsGSS.length > 0) {
        angular.forEach($scope.LandDetailsGSS, function (i, j) {
            $scope.MemberRegistration["GSSLandDetails"].push(i);
        });
    }

    if ($scope.SownCropDetailGSS.length > 0) {
        angular.forEach($scope.SownCropDetailGSS, function (i, j) {
            $scope.MemberRegistration["SownCropDetails"].push(i);
        });
    }

    if ($scope.MemberRegistration.IsLandOwnership && $scope.CommercialLoanDetail.length > 0) {
        angular.forEach($scope.CommercialLoanDetail, function (i, j) {
            $scope.MemberRegistration["OtherBankLoanDetails"].push(i);
        });
    }

    $scope.MemberRegistration["OrganizationtypeId"] = $('input[type=radio][name=rdoRegistrationType]:checked').val();


    $scope.MemberRegistration.SocietyName = $('#ddlSociety option:selected').text().trim();
    $scope.MemberRegistration.DISTRICT_ENG = $('#ddlDistrict option:selected').text().trim();
    $scope.MemberRegistration.DISTRICT_MANGAL = $('#ddlDistrict option:selected').text().trim();
    $scope.MemberRegistration.BLOCK_ENG = $('#ddlblock option:selected').text().trim();
    $scope.MemberRegistration.BLOCK_MANGAL = $('#ddlblock option:selected').text().trim();
    $scope.MemberRegistration.GP_ENG = $('#ddlGP option:selected').text().trim();
    $scope.MemberRegistration.GP_MANGAL = $('#ddlGP option:selected').text().trim();
    $scope.MemberRegistration.Village = $('#ddlVillage option:selected').text().trim();
    $scope.MemberRegistration.FarmerRelation = $('#ddlNomineeRelation option:selected').text().trim();
    $scope.MemberRegistration.EconomicGroup = $('#ddlEconomicSection option:selected').text().trim();
    $scope.MemberRegistration.Category = $('#ddlCategory option:selected').text().trim();



    //if (!$scope.ValidateGSSData()) {
    //    return;
    //}


}
//END : Save GSS data()

//START : Change Society Type at radio button
$scope.ChangeType = function () {
    $scope.MemberRegistration.DISTRICT_ID = '0';
    $scope.Blocks = [];
    $scope.MemberRegistration.BLOCK_ID = '0';
    $scope.GramPanchayats = [];
    $scope.MemberRegistration.GP_ID = '0';
    $scope.SocietyList = [];
    $scope.MemberRegistration.OrganizationId = '0';
}
// END : Change Society Type at radio button


//START : Show Member Registration Form
$scope.ShowMemberRegistrationForm = function () {





    $scope.divAADHARVerificationStep_1 = true;
    $scope.divParentDataSelection = false;
    $scope.PLDBName = $('#ddlDistrict :selected').text();
    $scope.BranchName = $('#ddlblock :selected').text();
    $scope.showTitle = true;
}
// END : Show Member Registration Form

//START : Verify AADHAr No.
$scope.VerifyAADHARNo_Step_1_2 = function () {
















    $scope.divAADHARVerificationStep_1 = true;
    $scope.divParentDataSelection = false;
    if ($scope.MemberRegistration.AADHARNo == undefined || $scope.MemberRegistration.AADHARNo.length < 12) {
        var msg = _language == 'en-us' ? 'Please enter 12 digit AADHAAR No.' : '12 अंकों की आधार संख्या दर्ज करें';
        alert(msg);
        return;
    } else {
        AadhaarAuth($scope.MemberRegistration.AADHARNo, $http, function (data) {
            //$scope.LoanGuarantor[index].Name = data.Name;
            //$scope.LoanGuarantor[index].FatherName = data.FName;
            //$scope.LoanGuarantor[index].Age = data.Age;
            //$scope.LoanGuarantor[index].Address = data.Address;
            //$scope.LoanGuarantor[index].MobileNo1 = data.Phone;
            //$scope.LoanGuarantor[index].IsValidate = true;
            //$scope.LoanGuarantor[index].DOB = data.DOB;



            $scope.divAADHARVerificationStep_1 = false;
            $scope.divAADHARVerificationStep_2 = false;
            $scope.PLDBMemberDatail = true;

            $scope.MemberRegistration.Name = data.Name;
            $scope.MemberRegistration.FatherHusbandName = data.FName

            //var newdate = new Date(data.AADHARDetails[0].Column4.split('-')[1] + '-' + data.AADHARDetails[0].Column4.split('-')[0] + '-' + data.AADHARDetails[0].Column4.split('-')[2]);
            //var diff = ((newdate).getTime() - (new Date()).getTime()) / 1000;
            //diff /= (60 * 60 * 24);

            //$('#txtRegistrationDate').val().split("/").reverse().join("-"));

            $scope.MemberRegistration.DOB = data.DOB
            $scope.MemberRegistration.Age = data.Age;
            $scope.MemberRegistration.Gender = data.Gender == 'M' ? '1' : (data.Gender == 'F' ? 2 : 3);
            $scope.MemberRegistration.PhotoPath = data.Photo;

            $scope.MemberRegistration.Address = data.Address;
            $scope.MemberRegistration.CurrentAddress = data.Address;
            $scope.MemberRegistration.MobileNo = data.MobileNo;













            $scope.$apply();
        });
    }

    //$http.post(SiteUrl + "/SocietyMemberRegistration/GenerateOTPForPLDOTPBMemberRegistration", { _AadharNo: $scope.MemberRegistration.AADHARNo }).
    //    success(function (data, status) {

    //        if (data.Result) {
    //            $scope.MemberRegistration.txn = data.txnid;
    //            $scope.AADHAREnc = data.AadharNo;
    //            $scope.divAADHARVerificationStep_1 = false;
    //            $scope.divAADHARVerificationStep_2 = true;
    //            var msg1 = _language === 'en-us' ? 'THE OTP HAS BEEN SENT TO AADHAR REGISTERED MOBILE NUMBER' : 'ओटीपी आधार पंजीकृत मोबाइल नंबर पर भेजा गया है';
    //            alert(msg1);
    //            $(".se-pre-con").fadeOut("slow");
    //        }
    //        else {
    //            $(".se-pre-con").fadeOut("slow");
    //            var msg2 = _language === 'en-us' ? 'INVALID AADHAAR No.' : 'अमान्य आधार नंबर';
    //            $scope.divAADHARVerificationStep_1 = true;
    //            alert(msg2);
    //        }
    //    }).
    //    error(function (data, status) {

    //    });
};
// END : Verify AADHAr No.

//START : Verify AADHAr No.
$scope.VerifyAADHARNo_Step_2_2 = function () {

    if ($scope.MemberRegistration.EnterOTP == undefined || $scope.MemberRegistration.EnterOTP.length != 6) {
        var msg = _language == 'en-us' ? 'Please enter 6 digit OTP' : '6 अंकों की ओटीपी दर्ज करें';
        alert(msg);
        return;
    }

    $http.post(SiteUrl + "/SocietyMemberRegistration/ValidateOTPForPLDBMemberRegistration", { "otp": $scope.MemberRegistration.EnterOTP, "txn": $scope.MemberRegistration.txn, "_AadharNo": $scope.AADHAREnc }).
        success(function (data, status) {

            if (data.Result === false) {
                alert(data[0].Column1 + ': Due To' + data[0].Column2);
                return;
            }
            else if (data.Result === true) {
                $scope.divAADHARVerificationStep_1 = false;
                $scope.divAADHARVerificationStep_2 = false;
                $scope.PLDBMemberDatail = true;

                $scope.MemberRegistration.Name = data.AADHARDetails[0].Column2;
                $scope.MemberRegistration.FatherHusbandName = data.AADHARDetails[0].Column9;

                var newdate = new Date(data.AADHARDetails[0].Column4.split('-')[1] + '-' + data.AADHARDetails[0].Column4.split('-')[0] + '-' + data.AADHARDetails[0].Column4.split('-')[2]);
                var diff = ((newdate).getTime() - (new Date()).getTime()) / 1000;
                diff /= (60 * 60 * 24);

                //$('#txtRegistrationDate').val().split("/").reverse().join("-"));

                $scope.MemberRegistration.DOB = data.AADHARDetails[0].Column4.split("-").reverse().join("-");
                $scope.MemberRegistration.Age = Math.abs(Math.round(diff / 365.25));
                $scope.MemberRegistration.Gender = data.AADHARDetails[0].Column3 == 'M' ? '1' : (data.AADHARDetails[0].Column3 == 'F' ? 2 : 3);
                $scope.MemberRegistration.PhotoPath = data.AADHARDetails[0].Column5;
            }
            else {
                return;
            }
        }).
        error(function (data, status) {
            $scope.SocietyRegdatanew = [];
        });
}
// END : Verify AADHAr No.

//START : Validate PLDB Member Registration Form
$scope.ValidatePLDBMemberRegistration = function () {
    var msg = '';

    if ($scope.MemberRegistration.PLDB_ID == undefined || $scope.MemberRegistration.PLDB_ID == '0') {
        msg += _language == 'en-us' ? 'Please select PLDB\n' : 'कृपया पी.एल.डी.बी. का चयन करें |\n';
    }

    if ($scope.MemberRegistration.Branch_ID == undefined || $scope.MemberRegistration.Branch_ID == '0') {
        msg += _language == 'en-us' ? 'Please select Branch\n' : 'कृपया ब्रांच का चयन करें |\n';
    }

    if ($scope.MemberRegistration.Name == undefined || $scope.MemberRegistration.Name.length < 1) {
        msg += _language == 'en-us' ? 'Please write applicant Name\n' : 'कृपया आवेदक का नाम लिखें |\n';
    }

    if ($scope.MemberRegistration.FatherHusbandName == undefined || $scope.MemberRegistration.FatherHusbandName.length < 1) {
        msg += _language == 'en-us' ? 'Please write Husband/Father Name\n' : 'कृपया पिता/पति का नाम लिखें|\n';
    }

    if ($scope.MemberRegistration.Age == undefined || $scope.MemberRegistration.Age.length < 1 || $scope.MemberRegistration.Age < 1) {
        msg += _language == 'en-us' ? 'Please write Age\n' : 'कृपया उम्र दर्ज करें |\n';
    }

    if ($scope.MemberRegistration.EducationalQualificationId == undefined || $scope.MemberRegistration.EducationalQualificationId == '0') {
        msg += _language == 'en-us' ? 'Please select Education Qualification\n' : 'कृपया शैक्षणिक अर्हताए (योग्यता) का चयन करें |\n';
    }

    if ($scope.MemberRegistration.CategoryId == undefined || $scope.MemberRegistration.CategoryId == '0') {
        msg += _language == 'en-us' ? 'Please select Category\n' : 'कृपया वर्ग चयन करें|\n';
    }

    if ($scope.MemberRegistration.Address == undefined || $scope.MemberRegistration.Address.length < 1) {
        msg += _language == 'en-us' ? 'Please write Permanat Address\n' : 'कृपया निवास का स्थाई पता दर्ज करें |\n';
    }

    if ($scope.MemberRegistration.CurrentAddress == undefined || $scope.MemberRegistration.CurrentAddress.length < 1) {
        msg += _language == 'en-us' ? 'Please write Current Address\n' : 'कृपया निवास का वर्तमान पता दर्ज करें |\n';
    }

    if ($scope.MemberRegistration.NomineeName == undefined || $scope.MemberRegistration.NomineeName.length < 1) {
        msg += _language == 'en-us' ? 'Please write Nominee Name with Father/Husband Name\n' : 'कृपया नामनिर्देशित का नाम और उसके पिता / पति का नाम दर्ज करें |\n';
    }

    if ($scope.MemberRegistration.NomineeRelationId == undefined || $scope.MemberRegistration.NomineeRelationId == '0') {
        msg += _language == 'en-us' ? 'Please select relation with nominee\n' : 'कृपया आवेदक के साथ नामनिर्देशिति का सम्बन्थ चयन करें|\n';
    }

    if ($scope.MemberRegistration.MobileNo == undefined || $scope.MemberRegistration.MobileNo.length != 10) {
        msg += _language == 'en-us' ? 'Please write 10 digit Mobile No.\n' : 'कृपया 10 अंकों का मोबाइल नंबर दर्ज करें|\n';
    }

    if ($scope.MemberRegistration.AADHARNo == undefined || $scope.MemberRegistration.AADHARNo.length != 12) {
        msg += _language == 'en-us' ? 'Please write AADHAAR No.\n' : 'कृपया आधार संख्या दर्ज करें|\n';
    }

    if ($scope.MemberRegistration.Business == undefined || $scope.MemberRegistration.Business.length < 1) {
        msg += _language == 'en-us' ? 'Please wite business\n' : 'कृपया व्‍यवसाय दर्ज करें |\n';
    }


    if (msg.length > 0) {
        ShowOkMessage(_language == 'en-us' ? 'PLDB Member Registeration Accepted' : 'सदस्य पंजीकरण स्वीकार किया गया', msg, null)
        return false;
    }
    

    return true;
}
//END : Validate PLDB Member Registration Form

//START : ShowDeclaration
$scope.ShowDeclaration = function () {
    if (!$scope.ValidatePLDBMemberRegistration()) {
        return false;
    }

    $scope.divAADHARVerificationStep_1 = false;
    $scope.divAADHARVerificationStep_2 = false;
    $scope.divParentDataSelection = false;
    $scope.PLDBMemberDatail = false;
    $scope.divViewShow = true;
    $scope.EducationalQualification = $('#ddlEducationQualification :selected').text().trim();
    $scope.Category = $('#ddlCategory :selected').text().trim();
    $scope.NomineeRelation = $('#ddlNomineeRelation :selected').text().trim();
}
// END : ShowDeclaration

// START : Get Transaction Log
$scope.ShowTransactionLog = function (RequestId) {
    $(".se-pre-con").fadeIn("slow");
    $scope.ApplicationNo = '';
    $scope.MemberShipNo = '';
    $scope.TransactionMemberName = '';
    $scope.TransactionPLDB = '';
    $scope.TransactionBranch = '';
    $http.post("../SocietyMemberRegistration/GetTransactionLogByRequestId", { "RequestIdEnc": RequestId }).
        success(function (data, status) {
            $scope.MemberApprvalTransactionLog = [];
            if (data.length > 0) {
                $scope.MemberApprvalTransactionLog = data;
                $scope.ApplicationNo = data[0].ApplicationNo;
                $scope.MemberShipNo = data[0].MembershipNo;
                $scope.TransactionMemberName = data[0].Name;
                $scope.TransactionPLDB = data[0].PLDBName;
                $scope.TransactionBranch = data[0].BLOCK_MANGAL;
                $(".se-pre-con").fadeOut("slow");
            }
        }).
        error(function (data, status) {
            $scope.MemberApprvalTransactionLog = [];
            $(".se-pre-con").fadeOut("slow");
        });
    $(".se-pre-con").fadeOut("slow");
}
// END: Get Transaction Log






//// START : On Page Load
$scope.getTranslation();
$scope.PLDBList();
$scope.GetBloodRelationList();
$scope.GetQualificationList();
$scope.GetCategoryList();

//// END : On Page Load

$scope.GenrateOTP = function () {
    $scope.IsOTPSended = true;
    $scope.DivMsg = true;
    $scope.textchange = 'true';
    $scope.getotp = true;
    $scope.Showgride = true;
    $scope.showotpblock = true;
    $http.post(SiteUrl + "/SocietyMemberRegistration/GenerateOTPForPLDOTPBMemberRegistration", { _AadharNo: $scope.MemberRegistration.AADHARNo }).
        success(function (data, status) {

            if (data.length > 0) {
                $scope.OTPStatus = true;
                if (data[0].txnid.includes("DOITRJGOVAUTH")) {
                    $scope.MemberRegistration.txn = data[0].txnid;
                    $scope.SocietyRegdata = data;
                    $scope.textchange = 'false';
                    $scope.OTPStatusMsg = 'THE OTP HAS BEEN SENT TO AADHAR REGISTERED MOBILE NUMBER';
                }
                else {
                    $scope.OTPStatusMsg = data[0].txnid;
                    $("#DivOTPStatus").removeClass("alert alert-success");
                    $("#DivOTPStatus").addClass("alert alert-danger");
                    $scope.invalidotp = true;
                    $scope.textchange = 'false';
                }
            }
        }).
        error(function (data, status) {

        });

}
$scope.MatchOtpApproved = function () {

    $http.post(SiteUrl + "/SocietyMemberRegistration/ValidateOTPForPLDBMemberRegistration", { "otp": $scope.MemberRegistration.EnterOTP, "txn": $scope.MemberRegistration.txn, "_AadharNo": $scope.MemberRegistration.AADHARNo }).
        success(function (data, status) {
            $scope.SocietyRegdatanew = data;
            if (data[0].Column1 == "Failed") {
                alert(data[0].Column1 + ': Due To' + data[0].Column2)
                $scope.textchange = 'false';
                $scope.invalidotp = true;
                $scope.validotp = false;
                $scope.showotpblock = false;
            }
            else if (data[0].Column1 == "Success") {
                $scope.invalidotp = false;
                $scope.validotp = true;
                $scope.showotpblock = false;
                alert("Our Society Successfully Mapped With Your SSOID");
                $scope.textchange = 'false';
                window.location = "../SocietyMemberRegistration/Index";
            }
            else {
                $scope.invalidotp = false;
                $scope.showotpblock = false;
                return;
            }
        }).
        error(function (data, status) {
            $scope.SocietyRegdatanew = [];
        });
    $scope.textchange = 'false';
}

// STRAT : Back to PLDB MEmber Registration Request Form
$scope.BackToPLDBMemberRequestList = function () {
    $scope.showPLDBMemberRequestList = true;
    $scope.divViewApplicationDetailShow = false;
}
//END : Back to PLDB MEmber Registration Request Form

//START : Save PLDB Member Status

    $scope.MemberApproval = {
        StatusId: '0',
        Remark:""
    }
$scope.SavePLDBMemberStatus = function () {

    var msg = '';

    if ($scope.MemberApproval.StatusId == '0') {
        msg += _language == 'en-us' ? 'Please select status</br>' : 'कृपया स्थिति का चयन करें</br>';
    }

    if ($scope.MemberApproval.Remark === undefined || $scope.MemberApproval.Remark.length === 0) {
        //if ($scope.MemberApproval.IsApproved == '1') {
        //    msg += _language == 'en-us' ? 'Please write remark for approval' : 'कृपया स्वीकार्य के लिए टिप्पणी लिखें';
        //}
        //else if ($scope.MemberApproval.IsApproved == '0') {
        //    msg += _language == 'en-us' ? 'Please write remark for rejection' : 'कृपया अस्वीकृति के लिए टिप्पणी लिखें';
        //}
        //else if ($scope.MemberApproval.IsApproved == '2') {
        //    msg += _language == 'en-us' ? 'Please write remark for objection' : 'कृपया आपत्ति के लिए टिप्पणी लिखें';
        //}

        msg += _language == 'en-us' ? 'Please write remark</br>' : 'कृपया टिप्पणी लिखें</br>';
    }
    if (msg.length > 0) {
        ShowOkMessage("Warning", msg, function () { });
        return;
    } else {
        if ($scope.MemberApproval.StatusId === '1044') {
            ShowAlertMessage("Warning", _language == 'en-us' ? "Are you sure you want to reject this application?" : "क्या आप वाकई इस एप्लिकेशन को अस्वीकार करना चाहते हैं?", function () {
                $scope.SaveStatus("");
            })
        }else    if ($scope.MemberApproval.StatusId === '1043') {
           // $("#divViewApplicationDetailShow").modal("hide");
            AadhaarAuth("", $http, "", true, function (authData) {
               // $("#divViewApplicationDetailShow").modal("show");
                $scope.SaveStatus(authData.TransId);
            });
        } else {
            $scope.SaveStatus("");
        }        
    }
    }

    $scope.SaveStatus = function (txn) {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/SocietyMemberRegistration/ActionOnMemberApprovalRequest", { "txn": txn, "RequestId": $scope.MemberApplicationId, "Status": $scope.MemberApproval.StatusId, "Remark": $scope.MemberApproval.Remark }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data == "1") {
                    ShowOkMessage("Success", _language == 'en-us' ? 'Action recorded successfully.' : 'कार्रवाई सफलतापूर्वक दर्ज की गई।', function () {
                        $("#divViewApplicationDetailShow").modal("hide");
                        $scope.MemberApproval.StatusId = '0';
                        $scope.MemberApproval.Remark = "";
                        $scope.GetPLDBMemberApprovalData();

                    });

                    //if ($scope.MemberApproval.StatusId === '1045') {
                    //    $http.post("/SocietyMemberRegistration/SendResponseOnMembershipObjection/", { "_Entity": $scope.ObjectionData }).
                    //        success(function (data, status) {
                    //            if (data.Response != "-1") {

                    //                ShowOkMessage (_language == 'en-us' ? 'Response sent successfully.' : 'प्रतिक्रिया सफलतापूर्वक भेजी गई।');
                    //                ShowOkMessage((_language == 'en-us' ? 'Feedback on objection during member registration' : 'सदस्य पंजीकरण के दौरान आपत्ति पर प्रतिक्रिया'), msg, null);
                    //                // window.location = '/SocietyMemberRegistration/MemberRegObjectionResponse';

                    //                $('#btnTrigger').click();


                    //                $http.post(SiteUrl + "/SocietyMemberRegistration/SendOTPForMembershipCertificate").
                    //                    success(function (data, status) {

                    //                        var result = data[0];
                    //                        $scope.MemberCertificate = [];
                    //                        $scope.MemberCertificate["txn"] = result.txnid;
                    //                        $scope.OTPStatusMsg = 'THE OTP HAS BEEN SENT TO AADHAR REGISTERED MOBILE NUMBER. PLEASE ENTER OTP.';
                    //                        $scope.CertificateRequestId = $scope.MemberApplicationId;
                    //                        $(".se-pre-con").fadeOut("slow");
                    //                    }).
                    //                    error(function (data, status) {
                    //                        $(".se-pre-con").fadeOut("slow");
                    //                    });
                    //                $(".se-pre-con").fadeOut("slow");



                    //            }
                    //            else {
                    //                window.location = '../SocietyMemberRegistration/MemberApproval';
                    //            }
                    //        }).
                    //        error(function (data, status) {
                    //            console.log('Error in SaveResponseOnObjection function.');
                    //        });
                    //}
                }
                //else if (data == "-2") {
                //  ShowOkMessage (_language == 'en-us' ? 'Action Already performed. No more action required.' : 'कार्रवाई पहले ही हो चुकी है। कोई और कार्रवाई की आवश्यकता नहीं है।');
                //}
                else {//if (data == "-1") {
                    ShowOkMessage("", _language == 'en-us' ? 'Error!!! No action performed.' : 'त्रुटि !!! कोई कार्रवाई नहीं की गई।', function () { });
                    //}
                    //else {
                    //  ShowOkMessage (_language == 'en-us' ? 'Error!!!' : 'त्रुटि !!!');
                    //}
                }

            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                ShowOkMessage("", _language == 'en-us' ? 'Error!!! No action performed.' : 'त्रुटि !!! कोई कार्रवाई नहीं की गई।', function () { });

            });
    }
    $scope.EditMember = function (item) {
        $scope.MemberRegistration.PLDB_ID = item.PldbId.toString();
        //$scope.MemberRegistration.Branch_ID = item.BranchId.toString();
        $scope.MemberRegistration.RequestId = item.RequestId;
        $scope.MemberRegistration.UserId = item.RequestId;
        $scope.MemberRegistration.AADHARNo = item.AADHARNo;
        $scope.MemberRegistration.Name = item.Name;
        $scope.MemberRegistration.FatherHusbandName = item.FatherHusbandName;
        $scope.MemberRegistration.Age = item.Age;
        $scope.MemberRegistration.EducationalQualificationId = item.EducationalQualificationId;
        $scope.MemberRegistration.CategoryId = item.CategoryId;
        $scope.MemberRegistration.Address = item.Address;
        $scope.MemberRegistration.PhotoPath = item.PhotoPath;
        $scope.MemberRegistration.CurrentAddress = item.CurrentAddress;
        $scope.MemberRegistration.NomineeName = item.NomineeName;
        $scope.MemberRegistration.NomineeRelationId = item.NomineeRelationId;
        $scope.MemberRegistration.MobileNo = item.MobileNo;
        $scope.MemberRegistration.Business = item.Business;
        $scope.PLDBBranchList().then(function () {
            $scope.MemberRegistration.Branch_ID = item.BranchId.toString();
            $scope.MemberRegistration.PLDB_ID = item.PldbId.toString();
        })
    }

    //END : Save PLDB Member Status

});
