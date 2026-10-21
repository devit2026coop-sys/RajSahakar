var CommityRegistrtionModule = angular.module('SocietyDetails_MPR_RPTMod', ['angularUtils.directives.dirPagination']);

CommityRegistrtionModule.controller('SocietyDetails_MPR_RPTCnt', function ($scope, $http, $rootScope) {

    $scope.currentPage = 1;
    $scope.pageSize = 100;

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
    //$scope.searchRequest = function (callback) {
    //    //$scope.DataPaging.searchParameters = [];
    //    $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
    //    var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').length;
    //    var indx = 1;
    //    $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox').each(function () {

    //        if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
    //            if ($(this).is(":checked")) {
    //                var parameter = { Key: $(this).attr('name') + '#' + indx, Value: $(this).val() }
    //                $scope.DataPaging.searchParameters.push(parameter);
    //            }
    //        } else {
    //            if ($(this).val() != '' && $(this).val() != null && $(this).attr('name') != undefined) {
    //                var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
    //                $scope.DataPaging.searchParameters.push(parameter);
    //            }
    //        }
    //        if (indx == ln) {
    //            $scope.DataPaging.currentPageID = 1;
    //            callback();
    //        }
    //        ++indx;
    //    });
    //}
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
  
    /*------------End Paging and Filter-----------------*/
    //$scope.FilterSearch = {};

    //$scope.FilterSearch.Month = (new Date().getMonth() + 1).toString();
    //$scope.FilterSearch.Year = (new Date().getFullYear()).toString();
    //$("#ddrYear").val(new Date().getFullYear());
    //$("#ddrMonth").val((new Date().getMonth() + 1));

    //$scope.FinancialYearList = [];
    //$scope.BindFY = function () {

    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
    //        success(function (data, status) {
    //            //alert(JSON.stringify(data));
    //            $scope.FinancialYearList = data;
    //            $scope.FilterSearch.Month = (new Date().getMonth() + 1).toString();
    //            $scope.FilterSearch.Year = (new Date().getFullYear()).toString();
    //            //$scope.GetSocietyRegistrationTimeSheet();
    //        }).
    //        error(function (data, status) {
    //            $scope.FinancialYearList = [];
    //        });
    //}

    //$scope.Divisions = [{}];
    //$scope.IsShowDivision = false;
    //if ($("#hdnDivisionId").val() != "0" && $("#hdnDivisionId").val() != "0") {

    //    $("#ddlDivision").val($("#hdnDivisionId").val())
    //    $("#ddlDivision").attr("disabled", "disabled");
    //    $("#ddlDistrictId").val($("#hdnDistrictId").val())
    //    $("#ddlDistrictId").attr("disabled", "disabled");
    //    $scope.FilterSearch.DIVISION_ID = $("#hdnDivisionId").val();
    //    $scope.FilterSearch.DistrictId = $("#hdnDistrictId").val();
    //    $scope.IsShowDivision = true;
    //} 

    //if ($("#hdnDivisionId").val() != "0") {
    //    $("#ddlDivision").val($("#hdnDivisionId").val())
    //    $("#ddlDivision").attr("disabled", "disabled");
    //    $scope.FilterSearch.DIVISION_ID = $("#hdnDivisionId").val();
    //    //$scope.FilterSearch.DistrictId = $("#hdnDistrictId").val();
    //    $scope.IsShowDivision = true;

    //}   

    //if ($("#hdnDistrictId").val() != "0") {
    //    //alert($("#hdnDistrictId").val());
    //    $("#ddlDistrictId").val($("#hdnDistrictId").val())
    //    $("#ddlDistrictId").attr("disabled", "disabled");
    //    //$scope.FilterSearch.DIVISION_ID = $("#hdnDivisionId").val();
    //    $scope.FilterSearch.DistrictId = $("#hdnDistrictId").val();
    //    $scope.IsShowDivision = false;

    //} else {
    //    $scope.IsShowDivision = true;

    //}

    //$scope.BindDivisions = function () {
    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
    //        success(function (data, status) {
    //            $scope.Divisions = data;
    //        }).
    //        error(function (data, status) {
    //            $scope.Divisions = [];
    //        });
    //}
    //$scope.BindDivisions();
    //$scope.BindFY();



    //$scope.MonthList = [
    //    { Id: "1", NameEn: "January", NameHi: "जनवरी" },
    //    { Id: "2", NameEn: "February", NameHi: "फ़रवरी" },
    //    { Id: "3", NameEn: "March", NameHi: "मार्च" },
    //    { Id: "4", NameEn: "April", NameHi: "अप्रैल" },
    //    { Id: "5", NameEn: "May", NameHi: "मई" },
    //    { Id: "6", NameEn: "June", NameHi: "जून" },
    //    { Id: "7", NameEn: "July", NameHi: "जुलाई" },
    //    { Id: "8", NameEn: "August", NameHi: "अगस्त" },
    //    { Id: "9", NameEn: "September", NameHi: "सितंबर" },
    //    { Id: "10", NameEn: "October", NameHi: "अक्टूबर" },
    //    { Id: "11", NameEn: "November", NameHi: "नवंबर" },
    //    { Id: "12", NameEn: "December", NameHi: "दिसंबर" }];

    $scope.TimeSheet = { Type: '1' };
    $scope.IsSubmiApplication = false;
   
    // $scope.LoanApplication.LOAN_ID = '0';

   // $scope.selectedMlonth = (_language == 'en-us' ? $scope.MonthList.filter(function (x) { return x.Id === $scope.FilterSearch.Month })[0].NameEn: $scope.MonthList.filter(function (x) { return x.Id === $scope.FilterSearch.Month })[0].NameHi) + " " + $scope.FilterSearch.Year+" ";

    $scope.GetSocietyRegistrationTimeSheet = function () {
        $scope.ShowSocietyList = true;
        $scope.ShowVoterList = false;
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/mis/GetAuditMPRMonthWise", JSON.stringify({ Details: jsonParameters, type: 1 })).
            success(function (data, status) {

                $scope.SocietyList = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                alert("Error while fetching list")
                $(".blockUI").fadeOut("slow");
            });
    }
    //$scope.GetSocietyRegistrationTimeSheet();
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
    
    // START : Code for Language label setting


    $scope.getTranslation = function () {
       // _language = 'hi-in';
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
    };
    $scope.getTranslation();

    //$scope.GetDistrictList = function () {
    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
    //        success(function (data, status) {
    //            $scope.DistrictList = data;
    //        }).
    //        error(function (data, status) {
    //            $scope.DistrictList = [];
    //        });
    //};
    //$scope.GetDistrictList();

    //$scope.DistrictList = [];
    //$scope.GetDistrictList = function () {
    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
    //        success(function (data, status) {
    //            $scope.DistrictList = data;
    //        }).
    //        error(function (data, status) {
    //            $scope.DistrictList = [];
    //        });
    //};
    //$scope.GetDistrictList();



    //Madhuri
      $scope.urR = 2;
  //  $scope.var = 2;
    $scope.getUrban = function (id) {
       
        $scope.urR = 0;
       
        
        $(".Rurals_MemberAdd").show();
        $(".cityward_MemberAdd").hide();
        $(".pincode_MemberAdd").show();
        $scope.getblock();
    }

    $scope.getRural = function (id) {
        $scope.urR = 1;
       
        //UralUrban
        $(".Rurals_MemberAdd").hide();
        $(".cityward_MemberAdd").show();
        $(".pincode_MemberAdd").show();
        $scope.loadcity()
    }



    $scope.getblock = function (disid) {

        $scope.BlockDataList = [];
        if ($("#ddlDistrictId").val() != "" && $("#ddlDistrictId").val() != undefined) {
            //$scope.GetKVSSList($scope.godown.isGSSKVSS);
            return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $("#ddlDistrictId").val()).
                success(function (data, status) {
                    $scope.BlockDataList = data;
                    //$('#ddlblocki_MemberAdd').append('<option value="0"  class="ng-binding">--- Select ---</option>');
                    //for (var i = 0; i < BlockDataList.length; i++) {

                    //    $('#ddlblocki_MemberAdd').append('<option value="' + BlockDataList[i].BLOCK_ID + '">' + BlockDataList[i].BLOCK_ENG + '</option>');
                    //    //BLOCK_MANGAL
                    //}
                    // _callback(Addressgetinfo);
                }).
                error(function (data, status) {
                    BlockDataList = [];
                });
        }
    }


    $scope.getGrampanchyat = function (divisionid) {
        $scope.GramPancyatList = [];
        if ($("#ddlblocki_MemberAdd").val() != "" && $("#ddlblocki_MemberAdd").val() != undefined) {
            //$scope.GetKVSSList($scope.godown.isGSSKVSS);
            return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $("#ddlblocki_MemberAdd").val()).
                success(function (data, status) {

                    $scope.GramPancyatList = data;
                    //$('#ddlGP_MemberAdd').append('<option value=""  class="ng-binding">--- Select ---</option>');

                    //for (var i = 0; i < GramPancyatList.length; i++) {

                    //    $('#ddlGP_MemberAdd').append('<option value="' + GramPancyatList[i].GP_ID + '">' + GramPancyatList[i].GP_ENG + '</option>');
                    //}//GP_MANGAL
                    // _callback(Addressgetinfo);
                }).
                error(function (data, status) {
                    GramPancyatList = [];

                });
        }
    }

    $scope.getvillage = function (pnchid) {
        //VillageList = [];
        $scope.VillageList = [];
        if ($("#ddlGP_MemberAdd").val() != "" && $("#ddlGP_MemberAdd").val() != undefined) {
            return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $("#ddlGP_MemberAdd").val()).
                success(function (data, status) {
                    $scope.VillageList = data;
                    //$('#ddlVillage_MemberAdd').append('<option value="0"  class="ng-binding">--- Select ---</option>');

                    //for (var i = 0; i < VillageList.length; i++) {

                    //    $('#ddlVillage_MemberAdd').append('<option value="' + VillageList[i].VILLAGE_ID + '">' + VillageList[i].VILLAGE_ENG + '</option>');
                    //}//VILLAGE_MANGAL
                    // _callback(Addressgetinfo);
                }).
                error(function (data, status) {
                    $scope.Villages = [];
                });
        }
    }



    $scope.loadcity = function () {
        $scope.Citylist = [];
        if ($("#ddlDistrictId").val() != "" && $("#ddlDistrictId").val() != undefined) {
            //$scope.GetKVSSList($scope.godown.isGSSKVSS);
            return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode2=" + $("#ddlDistrictId").val()).
                success(function (data, status) {
                    $scope.Citylist = data;
                    //$('#ddlcity_MemberAdd').append('<option value="0"  class="ng-binding">--- Select ---</option>');
                    //for (var i = 0; i < Citylist.length; i++) {

                    //    $('#ddlcity_MemberAdd').append('<option value="' + Citylist[i].CITY_ID + '">' + Citylist[i].CITY_ENG + '</option>');
                    //}
                    // _callback(Addressgetinfo);
                }).
                error(function (data, status) {
                    $scope.Citylist = [];
                });
        }
    }

    $scope.DivisionList = [];
    $scope.GetDistrictDivisionReasion = function () {
        $scope.DivisionList = [];
        $(".se-pre-con").fadeIn("slow");

       // return $http.get(SiteUrl + "/Master/GetDistrictDivisionReasion").
        return $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Division&MasterCode=0").
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.DivisionList = data;
                
                //data.List.forEach(function (itm, index) {
                //    if ($scope.DivisionList.filter(function (x) { return x.DIVISION_ENG == itm.DIVISION_ENG }).length <= 0) {
                //        $scope.DivisionList.push(itm);
                //    }
                //});
            }).
            error(function (data, status) {
                $scope.DivisionList = [];
            });
    }

    $scope.GetDistrictDivisionReasion();



   // $http.post(SiteUrl + "/FertilizerSupply/GetConstituencyListByDistrictId", { DistrictId: $('#district_MemberAdd').val() }).

    $scope.DistrictList = [];
    $scope.GetDistrictList = function () {
      
        debugger;
       
        $http.post(SiteUrl + "MIS/GetDistrictByDivison/", { "DivisonId" : $('#DivisionId').val() }).
            success(function (data, status) {
                debugger;
                $scope.DistrictList = data;
                //$(".Urb_Ru").show();

            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
   // $scope.GetDistrictList();

    $scope.DistrictChange = function () {
       // $scope.GetDistrictList();
        $(".UrbRur").show();
        $(".Rurals_MemberAdd").hide();
        $(".cityward_MemberAdd").hide();
        
        $scope.UralUrban = '';


        if ($scope.UralUrban == 1) {
            $scope.ddlblocki_MemberAdd = ""
            $scope.getblock();
        } else {
            $scope.ddlcity_MemberAdd = "";
            $scope.loadcity();
        }

       
        $scope.ddlblocki_MemberAdd = '0';
        $scope.ddlcity_MemberAdd = '0';
        $scope.ddlVillage_MemberAdd = '0';
        $scope.ActID = '0';
        $scope.ddlGP_MemberAdd = '0';
        $scope.urR = 2;
    }

    $scope.DivisonChange = function () {
        $scope.GetDistrictList();
        $(".Distri").show();
        $(".UrbRur").hide();

        //$(".Rurals_MemberAdd").hide();
        //$(".cityward_MemberAdd").hide();
                       
        //$('#ddlDistrictId').val(0);
        $scope.FilterSearch.DistrictId = '0';
        $scope.ddlblocki_MemberAdd = '0';
        $scope.ddlcity_MemberAdd = '0';
        $scope.ddlVillage_MemberAdd = '0';
        $scope.ActID = '0';
        $scope.ddlGP_MemberAdd = '0';
        $scope.urR = 2;

    }





    $scope.SocietyList = [];
    $scope.GridData = [];
    $scope.GetGridData = function () {
        $(".blockUI").fadeIn("slow");
     
        $http.post(SiteUrl + "MIS/GetSocietiesDetails/", {
            "DivisonId": ($('#DivisionId').val()            == '' ? 0 : $('#DivisionId').val()),
            "DistrictId":($('#ddlDistrictId').val()         == '' ? 0 : $('#ddlDistrictId').val()),
            "blockId":   ($('#ddlblocki_MemberAdd').val()   == '' ? 0 : $('#ddlblocki_MemberAdd').val()),
            "CityId":    ($('#ddlcity_MemberAdd').val()     == '' ? 0 : $('#ddlcity_MemberAdd').val()),
            "Gp":        ($('#ddlGP_MemberAdd').val()       == '' ? 0 : $('#ddlGP_MemberAdd').val()),
            "villageId": ($('#ddlVillage_MemberAdd').val()  == '' ? 0 : $('#ddlVillage_MemberAdd').val()),
            "ActId"      : ($('#ActID').val() == '' ? 0 : $('#ActID').val()),
            "RuralUrban":  $scope.urR
        }).
            success(function (data, status) {
                 $scope.SocietyList = data;
                //$(".Urb_Ru").show();

                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching data .");
                $scope.SocietyList = [];
            });
      };

    $scope.GetGridData();


    $scope.showHideDivision = function (DivId) {
        debugger;
        if (DivId != -1) {
            $('.divItemForHide').hide();
            $('.item_' + DivId).show();
        }
        else {
            $('.divItemForHide').show();
        }
    }
    
    $scope.GetSocietydetail = function (id) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/mis/SocietiesDetailsId/?Gid=" + id).
            success(function (data, status) {
                debugger;
                $scope.SocietyList = data;
                
                $scope.colsort = "";
                $scope.reverseSort = false;
   

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while fetching role list.");
                $scope.showLoader = false;
            });
    };

    



    // END : Code for Language label setting
});
