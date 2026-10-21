var AssignAuditorModule = angular.module('UploadAuditByAuditorMod', ['angularUtils.directives.dirPagination']);
AssignAuditorModule.controller("UploadAuditByAuditorCont", function ($rootScope, $scope, $http) {
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
        if ($scope.Filter.FilterType == 'wrs.created_date') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }
    $scope.ClearSearch = function () {
        $scope.Filter.FinancialYear = "";
        $scope.Filter.DistrictId = "";
        $scope.Filter.SocietyIdENC = "";
        $scope.Filter.ActId = "";
        $scope.Filter.RequestStatus = "";
        $scope.Filter.IsFirm = "";
        $scope.Filter.SerachText = "";
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.GetSocietyListAssignedToAuditor();
        $scope.Filter.SocietyTypeId = "";
        $scope.Filter.CategoryId = "";
        $scope.Filter.GroupBy = "";

    }
    /*------------End Paging and Filter-----------------*/

    $scope.ShowGroupByFilter = false;
    $scope.ClearSearchText = function ()
    {
        $scope.q = '';
    }

    $scope.CountSocietyAuditorList = false;

    $scope.getTranslation = function () {
        $scope._language = _language;
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
        //GET Financial Year

    };

    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $("#DivisionId").val()).
            success(function (data, status) {
                $scope.DistrictList = data;
                if ($("#hdnDistrictId").val() != "0") {
                    $scope.Filter.DistrictId = $("#hdnDistrictId").val()
                }                
                if ($("#hdnDistrictId").val() != "0" && $("#hdnDistrictId").val() != "" && $("#hdnDistrictId").val() != undefined) {
                    data.forEach(function (item, index) {
                        if ($("#hdnDistrictId").val().split(',').indexOf(item.DIVISION_ID) > -1) {
                            $scope.Divisions.push(item);
                        }
                    });
                    if ($("#hdnDistrictId").val().split(',').length == 1) {
                        $scope.DIVISION_ID = $("#hdnDistrictId").val();
                        $("#ddrDistrictId").attr("disabled", "disabled");
                    }
                }
                $scope.BindFY();
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };
    //$scope.IsSubmiApplication = false;

    // Get List of Divisions
    $scope.GetDivisionList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Divisions = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
            success(function (data, status) {
                //var _Result = JSON.parse(data);
                if ($("#DivisionId").val() != "0" && $("#DivisionId").val() != "" && $("#DivisionId").val() != undefined) {
                    $scope.DivisionBlock = true;
                    $scope.ShowGroupByFilter = true;
                    data.forEach(function (item, index) {
                        if ($("#DivisionId").val().split(',').indexOf(item.DIVISION_ID) > -1) {
                            $scope.Divisions.push(item);
                        }
                    });

                    if ($("#DivisionId").val().split(',').length == 1) {
                        $scope.DIVISION_ID = $("#DivisionId").val();
                        $("#ddrDivisionId").attr("disabled", "disabled");
                    }
                }
                else if ($("#hdnDistrictId").val() == "0" && $("#DivisionId").val() == "0")
                {
                    $scope.Divisions = data;
                }
            }).
            error(function (data, status) {
                $scope.Divisions = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };
    if ($("#hdnDistrictId").val() == "0" && $("#DivisionId").val() == "0") {
        $scope.DivisionBlock = true;
        $scope.ShowGroupByFilter = true;
        $scope.GetDistrictList();
        $scope.GetDivisionList();
    }
    // GET Society Type   
    $scope.GetSocietyTypeList = function () {       
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=3").
            success(function (data, status) {
                if ($("#DivisionId").val() != "0" && $("#DivisionId").val() != "" && $("#DivisionId").val() != undefined) {                    
                    var list = data.filter(function (x) { return x.SocietyTypeId !== "9" });
                    $scope.SocietyTypeList = list;  
                }
                else if ($("#hdnDistrictId").val() != "0" && $("#hdnDistrictId").val() != "" && $("#hdnDistrictId").val() != undefined) {
                    var list = data.filter(function (x) { return x.SocietyTypeId !== "8" && x.SocietyTypeId !== "9"  });
                    $scope.SocietyTypeList = list;  
                }
                else
                {
                    $scope.SocietyTypeList = data;  
                }
            }).
            error(function (data, status) {
                $scope.SocietyTypeList = [];
            });
    };
    $scope.GetCentralCategory = function (SocietyTypeId) {
        $(".se-pre-con").fadeIn("slow");
        if (SocietyTypeId == undefined) {
            $scope.SocietyTypeId = '';
        }        
            $http.post(SiteUrl + "/Auditor/CategoryListForAudit/?SocietyTypeId=" + SocietyTypeId).
                success(function (data, status) {
                    $scope.CentralCategoryList = data;
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $scope.CentralCategoryList = [];
                    $(".se-pre-con").fadeOut("slow");
                });        
        
    };  
    $scope.GetSocietyTypeList();
    $scope.GetCentralCategory();
    $scope.GetDistrictList();
    $scope.getTranslation();
    $scope.GetDivisionList();
    $scope.AssignedAuditor = [];
    $scope.FinancialYearList = [];
    var backFy = ''
    if ((new Date().getMonth() + 1) < 4) {
        backFy = new Date().getFullYear() - 2;
    } else {
        backFy = new Date().getFullYear() - 1;
    }
    var isPageLoad = true;
    var isBackFy = false;
    $scope.BindFY = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                //alert(JSON.stringify(data));
                $scope.FinancialYearList = data;

                isBackFy = getUrlVars()["year"] == '' || getUrlVars()["year"] == undefined ? false : true;
                if (isBackFy) {
                    $scope.Filter.FinancialYear = getUrlVars()["year"];// backFy + '-' + (parseInt(backFy) + 1).toString().substring(2, 4);
                }
                //setTimeout($scope.searchRequest($scope.GetSocietyListAssignedToAuditor), 1000);

            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
    }
    $scope.ShowDivisionName = false;
    $scope.ShowDistrictName = false;
    $scope.ShowSocietyTypeName = false;
    $scope.ShowSocietyRemaining = true;
    $scope.GetSocietyCountList = [];
    $scope.GetCountSocietyAssignedToAuditorList = function (FIN_YEAR_NAME, DistrictId, SocietyTypeId, CategoryId) {
        var District_ID = $scope.Filter.DistrictId;
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/MIS/GetCountSocietyAssignedToAuditor?FIN_YEAR_NAME=" + $scope.Filter.FinancialYear + "&DistrictId=" + $scope.Filter.DistrictId + "&SocietyTypeId=" + $scope.Filter.SocietyTypeId + "&GroupById=" + $scope.Filter.GroupBy + "&CategoryId=" + $scope.Filter.CategoryId).

            success(function (data, status) {
              //  data.DataList.Table.forEach(function (data, index) {
                $scope.GetSocietyCountList = [];
                $(".se-pre-con").fadeOut("slow");
                if ($scope.Filter.GroupBy != "" && $scope.Filter.GroupBy != "0" && $scope.Filter.GroupBy != undefined) {
                    if ($scope.Filter.GroupBy == "1")
                    {
                        $scope.ShowDivisionName = true;
                        $scope.ShowDistrictName = false;
                        $scope.ShowSocietyTypeName = false;
                        $scope.ShowSocietyRemaining = false;
                        dataList = JSON.parse(data.DataList);

                        $scope.GetSocietyCountList.push(dataList.Table1[0]);
                        $scope.GetSocietyCountList.push(dataList.Table2[0]);
                        $scope.GetSocietyCountList.push(dataList.Table3[0]);
                        $scope.GetSocietyCountList.push(dataList.Table4[0]);
                        $scope.GetSocietyCountList.push(dataList.Table5[0]);
                        $scope.GetSocietyCountList.push(dataList.Table6[0]);
                        $scope.GetSocietyCountList.push(dataList.Table7[0]);        
                    }
                    else if ($scope.Filter.GroupBy == "2") {
                        $scope.ShowDistrictName = true;
                        $scope.ShowDivisionName = false;
                        $scope.ShowSocietyTypeName = false;
                        $scope.ShowSocietyRemaining = false;
                        dataList = JSON.parse(data.DataList);

                        $scope.GetSocietyCountList.push(dataList.Table1[0]);
                        $scope.GetSocietyCountList.push(dataList.Table2[0]);
                        $scope.GetSocietyCountList.push(dataList.Table3[0]);
                        $scope.GetSocietyCountList.push(dataList.Table4[0]);
                        $scope.GetSocietyCountList.push(dataList.Table5[0]);
                        $scope.GetSocietyCountList.push(dataList.Table6[0]);
                        $scope.GetSocietyCountList.push(dataList.Table7[0]);

                        $scope.GetSocietyCountList.push(dataList.Table8[0]);
                        $scope.GetSocietyCountList.push(dataList.Table9[0]);
                        $scope.GetSocietyCountList.push(dataList.Table10[0]);
                        $scope.GetSocietyCountList.push(dataList.Table11[0]);
                        $scope.GetSocietyCountList.push(dataList.Table12[0]);
                        $scope.GetSocietyCountList.push(dataList.Table13[0]);
                        $scope.GetSocietyCountList.push(dataList.Table14[0]);

                        $scope.GetSocietyCountList.push(dataList.Table15[0]);
                        $scope.GetSocietyCountList.push(dataList.Table16[0]);
                        $scope.GetSocietyCountList.push(dataList.Table17[0]);
                        $scope.GetSocietyCountList.push(dataList.Table18[0]);
                        $scope.GetSocietyCountList.push(dataList.Table19[0]);
                        $scope.GetSocietyCountList.push(dataList.Table20[0]);
                        $scope.GetSocietyCountList.push(dataList.Table21[0]);

                        $scope.GetSocietyCountList.push(dataList.Table22[0]);
                        $scope.GetSocietyCountList.push(dataList.Table23[0]);
                        $scope.GetSocietyCountList.push(dataList.Table24[0]);
                        $scope.GetSocietyCountList.push(dataList.Table25[0]);
                        $scope.GetSocietyCountList.push(dataList.Table26[0]);
                        $scope.GetSocietyCountList.push(dataList.Table27[0]);
                        $scope.GetSocietyCountList.push(dataList.Table28[0]);

                        $scope.GetSocietyCountList.push(dataList.Table29[0]);
                        $scope.GetSocietyCountList.push(dataList.Table30[0]);
                        $scope.GetSocietyCountList.push(dataList.Table31[0]);
                        $scope.GetSocietyCountList.push(dataList.Table32[0]);
                        $scope.GetSocietyCountList.push(dataList.Table33[0]);      
                    }
                 else if ($scope.Filter.GroupBy == "3") {
                        $scope.ShowSocietyTypeName = true;
                        $scope.ShowDivisionName = false;
                        $scope.ShowDistrictName = false;
                        $scope.ShowSocietyRemaining = false;

                        dataList = JSON.parse(data.DataList);

                        $scope.GetSocietyCountList.push(dataList.Table1[0]);
                        $scope.GetSocietyCountList.push(dataList.Table2[0]);
                        $scope.GetSocietyCountList.push(dataList.Table3[0]);
                        $scope.GetSocietyCountList.push(dataList.Table4[0]);
                    }
                           
                }
                else {
                    $scope.ShowDistrictName = false;
                    $scope.ShowDivisionName = false;
                    $scope.ShowSocietyTypeName = false;
                    $scope.ShowSocietyRemaining = true;
                    $scope.GetSocietyCountList = JSON.parse(data.DataList);
                }                
            }).
            error(function (data, status) {
                $scope.GetSocietyCountList = [];
                alert('Error while fetching district list data.');
            });
    };
    $scope.GetCountSocietyAssignedToAuditorList();



    $scope.GetSocietyListAssignedToAuditor = function (SocietyCountId, GroupByDivisionId, GroupByDistrictId, SocietyTypeId) {
        

        $(".blockUI").fadeIn("slow");

        if (isPageLoad && isBackFy) {
            var parameter = { Key: 'FIN_YEAR_NAME', Value: $scope.Filter.FinancialYear }
            $scope.DataPaging.searchParameters.push(parameter);
        }
        if ($("#DivisionId").val() == "0" && $("#DivisionId").val() == "" && $("#DivisionId").val() == undefined) {
            var parameter = { Key: 'A.Division_Id', Value: "" }
            $scope.DataPaging.searchParameters.push(parameter);
        }

        isPageLoad = true;
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/MIS/GetSocietyListAssignedToAuditor", JSON.stringify({ Details: jsonParameters, Id: SocietyCountId, FIN_YEAR_NAME: $scope.Filter.FinancialYear, DistrictId: $scope.Filter.DistrictId, GroupByDivisionId: GroupByDivisionId, GroupByDistrictId: GroupByDistrictId, GroupBySocietyTypeId: SocietyTypeId})).
            success(function (data, status) {
                $scope.AssignedAuditor = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").fadeOut("slow");


            }).
            error(function (data, status) {
                $scope.AssignedAuditor = [];
                $(".blockUI").fadeOut("slow");
                alert('Error while fetching society list data.');
            });
    };
    //  $scope.GetSocietyListAssignedToAuditor();

    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
        success(function (data, status) {
            $scope.ActList = data;
        }).
        error(function (data, status) {
        });
    //$scope.SocietyList = [];
    //$http.get(SiteUrl + "/MIS/GetSocietyListAssignedToAuditorReport").
    //    success(function (data, status) {
    //        $scope.SocietyList = JSON.parse(data.DataList);
    //    }).
    //    error(function (data, status) {
    //        $scope.SocietyList = [];
    //    });



    // SCOPE
    //$scope._language = 'en-us';

    // Code for Language label setting

    // START :Get List of Societies

    // END :Get List of Societies

    // START :Get List of Districts
    //$scope.GetDistrictList = function () {
    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=-1").
    //        success(function (data, status) {
    //            $scope.DistrictList = data;
    //        }).
    //        error(function (data, status) {
    //            $scope.DistrictList = [];
    //            alert('Error while fetching district list data.');
    //        });
    //};

    $scope.ViewDocuments = function (Id) {
        $(".se-pre-con").fadeIn("slow");
        $("#myModal").modal("show");
        $http.get(SiteUrl + "/Auditor/GetAuditUplodedDocument?Id=" + Id).
            success(function (data, status) {
                $scope.DocumentList = data;
            }).
            error(function (data, status) {
                $scope.DocumentList = [];
                alert('Error while Document fetch.');
            });
        $(".se-pre-con").fadeOut("slow");
    }


    $scope.ViewSocietyList = function (SocietyCountId, DIVISION_ID, DISTRICT_ID, SocietyTypeId) {
        $scope.AssignedAuditor = [];         
        $scope.CountSocietyAuditorList = true;
        $scope.GetSocietyListAssignedToAuditor(SocietyCountId, DIVISION_ID, DISTRICT_ID, SocietyTypeId);
    }
    
});