var AssignAuditorModule = angular.module('SocietyAuditprogressMod', ['angularUtils.directives.dirPagination']);
AssignAuditorModule.controller("SocietyAuditprogressCont", function ($rootScope, $scope, $http) {
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
        $scope.Filter.GroupBy = "";

    }
    /*------------End Paging and Filter-----------------*/
    function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, '\\$&');
        var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    }

    if (window.location.href.indexOf('?fy') > -1) {
        $scope.Filter.FinancialYear=getParameterByName('fy');
    }


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
                //}
                //else {
                //    $scope.Divisions = data;

                //}
            }).
            error(function (data, status) {
                $scope.Divisions = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };


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


    $scope.GetSocietyTypeList();



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
    $rootScope.total_Count = 0;
    $rootScope.totalAppointed = 0;
    $rootScope.totalPartialSubmit = 0;
    $rootScope.totalCompleted = 0;
    $scope.ShowDivisionName = false;
    $scope.ShowDistrictName = false;
    $scope.ShowSocietyTypeName = false;
    $scope.GetSocietyCountList = [];
    $scope.GetCountSocietyAssignedToAuditorList = function (FIN_YEAR_NAME, DistrictId, SocietyTypeId) {
        var District_ID = $scope.Filter.DistrictId;

        //if (isPageLoad && isBackFy) {
        //    var parameter = { Key: 'FIN_YEAR_NAME', Value: $scope.Filter.FinancialYear }
        //    $scope.DataPaging.searchParameters.push(parameter);
        //}

        //isPageLoad = false;
        //var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };

        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/MIS/GetCountSocietyAuditprogress?FIN_YEAR_NAME=" + $scope.Filter.FinancialYear + "&DistrictId=" + $scope.Filter.DistrictId + "&SocietyTypeId=" + $scope.Filter.SocietyTypeId + "&GroupById=" + $scope.Filter.GroupBy).
      
            success(function (data, status) {
           
                var currentdate = new Date();
                var datetime = + currentdate.getDate() + "/"
                    + (currentdate.getMonth() + 1) + "/"
                    + currentdate.getFullYear() + " @ "
                    + currentdate.getHours() + ":"
                    + currentdate.getMinutes() + ":"
                    + currentdate.getSeconds();
                $scope.dateformate = datetime;
              //  data.DataList.Table.forEach(function (data, index) {
                $scope.GetSocietyCountList = [];
              
                if ($scope.Filter.FinancialYear == "undefined" || $scope.Filter.DistrictId == "undefined" || $scope.Filter.DistrictId == "") {
                        $scope.ShowDistrictName = true;
                        $scope.ShowDivisionName = false;
                        $scope.ShowSocietyTypeName = false;
                        dataList = JSON.parse(data.DataList);
                         $scope.GetSocietyCountList = JSON.parse(data.DataList);

                        //$scope.GetSocietyCountList.push(dataList.Table1[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table2[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table3[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table4[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table5[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table6[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table7[0]);

                        //$scope.GetSocietyCountList.push(dataList.Table8[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table9[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table10[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table11[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table12[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table13[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table14[0]);

                        //$scope.GetSocietyCountList.push(dataList.Table15[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table16[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table17[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table18[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table19[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table20[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table21[0]);

                        //$scope.GetSocietyCountList.push(dataList.Table22[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table23[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table24[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table25[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table26[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table27[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table28[0]);

                        //$scope.GetSocietyCountList.push(dataList.Table29[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table30[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table31[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table32[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table33[0]);

                    //$rootScope.total_Count = dataList.Table33[0].TotalCount
                    //$rootScope.totalAppointed = dataList.Table33[0].TotalAppointed
                    //$rootScope.totalPartialSubmit = dataList.Table33[0].TotalPartialSubmit
                    //$rootScope.totalCompleted = dataList.Table33[0].TotalCompleted

                    $rootScope.total_Count = 0;
                    $rootScope.totalAppointed = 0;
                    $rootScope.totalPartialSubmit = 0;
                    $rootScope.totalCompleted = 0;

                    $scope.GetSocietyCountList.forEach(function (item, indx) {
                        $rootScope.total_Count += item.TOTAL;
                        $rootScope.totalAppointed += item.Appointed;
                        $rootScope.totalPartialSubmit += item.PARTIALSUBMIT;
                        $rootScope.totalCompleted += item.Completed;
                    });

                        $(".se-pre-con").fadeOut("slow");
                    }
                 else {
                        $scope.ShowSocietyTypeName = false;
                        $scope.ShowDivisionName = false;
                        $scope.ShowDistrictName = true;

                       // dataList = JSON.parse(data.DataList);

                        $scope.GetSocietyCountList = JSON.parse(data.DataList);

                       // $scope.GetSocietyCountList.push(dataList[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table2[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table3[0]);
                        //$scope.GetSocietyCountList.push(dataList.Table4[0]);
                    //$scope.GetSocietyCountList.forEach(function (item, indx) {
                    //    $rootScope.total_Count += item.TOTAL
                    //    $rootScope.totalAppointed += item.Appointed
                    //    $rootScope.totalPartialSubmit += item.PARTIALSUBMIT
                    //    $rootScope.totalCompleted += item.Completed
                    //});
                        $(".se-pre-con").fadeOut("slow");
                    }
                           
                $(".se-pre-con").fadeOut("slow");
                         
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
        //alert($scope.Filter.FinancialYear);

        isPageLoad = false;
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/MIS/GetSocietyListAssignedToAuditor", JSON.stringify({ Details: jsonParameters, Id: SocietyCountId, FIN_YEAR_NAME: $scope.Filter.FinancialYear, DistrictId: $scope.Filter.DistrictId, GroupByDivisionId: GroupByDivisionId, GroupByDistrictId: GroupByDistrictId, GroupBySocietyTypeId: SocietyTypeId})).
            success(function (data, status) {
                //// $scope.SocietyList = data;
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
        //  $(".se-pre-con").fadeIn("slow");

        $scope.CountSocietyAuditorList = true;
        $scope.GetSocietyListAssignedToAuditor(SocietyCountId, DIVISION_ID, DISTRICT_ID, SocietyTypeId);
    }
    
});