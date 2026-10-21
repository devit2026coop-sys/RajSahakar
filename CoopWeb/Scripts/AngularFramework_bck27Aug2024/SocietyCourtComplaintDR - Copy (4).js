var AppClasses = angular.module("CourtModuleMod", ['angularUtils.directives.dirPagination']);
AppClasses.controller("CourtModuleCnt", function ($scope, $http, $rootScope) {
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


        //if (($scope.SelectPage.filter(x => x === $scope.DataPaging.currentPageID).length > 0)) {
        //    $("#chk_all").prop("checked", true);
        //} else {
        //    $("#chk_all").prop("checked", false);
        //}
    }

    
    $scope.OnPageLoad = function () {
        $('#From_Time').timepicker({
            showOn: "both",
            buttonImage: "../images/Time.png",
            buttonImageOnly: true,
        });

        $scope.Divisions = [{}];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
            success(function (data, status) {
                $scope.Divisions = data;
            }).
            error(function (data, status) {
                $scope.Divisions = [];
            });


        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ACT&MasterCode=-1").
            success(function (data, status) {
                //  $scope.ActList = data;
                $.each(data, function () {
                    $("#ddlAct").append($("<option></option>").val(this.ActId).html(this.ActNameEn));
                });
                //$('#ddlAct').multiselect({
                //    includeSelectAllOption: true
                //});
            }).
            error(function (data, status) {
                alert("Error while fetching Act list.");
                // $scope.showLoader = false;
            });

       // $scope.GetAllSubUnitList();
        $scope.GetApprovedSocietyList();
        $scope.GetApprovedSocietyAssignForIO();
        $scope.DistrictList();
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
        $scope.Filter.CCSId = '0';
        if ($scope.Filter.FilterType == 'created_dated') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            // $scope.AddCalender();
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }
    $scope.ClearSearch = function () {
        $("#searchText").val("");
        $("#CaseStatusId").val("");
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.GetApprovedSocietyList();
    }
    /*------------End Paging and Filter-----------------*/
    $scope._language = 'en-us';
    $scope._language = _language;
    if (_language == 'en-us') {
        $http.get("../Translations/Translation_EN.json").
            success(function (data, status) {
                $scope.translation = data;
            }).
            error(function (data, status) {
                alert("Error while translation");
            });
      
    }
    else {
        $http.get("../Translations/Translation_HI.json").
            success(function (data, status) {
                $scope.translation = data;
            }).
            error(function (data, status) {
                alert("Error while translation");
            });

    }


   
    $scope.insertinvited = function (value) {

      
        if (document.getElementById('build').checked && value == 1 ) {
            $scope.ComplainUnderSection.Formation = 1;
            $scope.checkstatus = true;
        }
        else if (document.getElementById('build').checked == false && value == 1) {
            $scope.ComplainUnderSection.Formation = 0;
            $scope.checkstatus = false;
            $("#ComplaintDate").removeClass("notValid");
            $("#ComplaintDate1").removeClass("notValid");
            $("#HtmlContent6").removeClass("notValid");
        }



        if (document.getElementById('business').checked && value == 2) {
            $scope.ComplainUnderSection.Business = 2;
            $scope.checkstatus1 = true;
        }
        else if (document.getElementById('business').checked == false && value == 2) {
            $scope.ComplainUnderSection.Business = 0;
            $scope.checkstatus1 = false;
            $("#ComplaintDate2").removeClass("notValid");
            $("#ComplaintDate3").removeClass("notValid");
            $("#HtmlContent5").removeClass("notValid");
        }


        if (document.getElementById('member').checked && value == 3) {
            $scope.ComplainUnderSection.Financial = 3;
            $scope.checkstatus2 = true;
        }
        else if (document.getElementById('member').checked == false && value == 3) {
            $scope.ComplainUnderSection.Financial = 0;
            $scope.checkstatus2 = false;
            $("#ComplaintDate4").removeClass("notValid");
            $("#ComplaintDate5").removeClass("notValid");
            $("#HtmlContent4").removeClass("notValid");
        }

        //if (value==2) {
        //    $scope.ComplainUndeVlaue.Builtype2 = 2;
          
        //    $scope.checkstatus1 = true;
        //}
        //else if ($scope.checkstatus1 == true && value == 2)
        //{ $scope.ComplainUndeVlaue.Builtype2 = 0;}


        //if (value==3) {
        //    $scope.ComplainUndeVlaue.Builtype3 = 3;
          
        //    $scope.checkstatus2 = true;
        //}
        //else if ($scope.checkstatus2 == true && value == 3) { $scope.ComplainUndeVlaue.Builtype3 = 0; }
    }

    


    $scope.SocietyId = "";
    $scope.StatusId = "";
    $scope.Complain_ID = "";
    $scope.currentDetails = "";
    $scope.StatusDetail = function (id) {
        if ($scope.ComplainUnderSection.Status == 4 ||$scope.ComplainUnderSection.Status == 5 || $scope.ComplainUnderSection.Status == 7) {
            $scope.currentDetails = $("#HtmlContent2").val("");
            $("#HtmlContent2").val("");
            $("#HtmlContent2").summernote("code", "");
           // $scope.GetAllSubUnitList(0);
            return false;
        } else {
            $("#HtmlContent2").summernote("code", $scope.currentDetails);
        }

    }
    $scope.Department = [];
   
   

    $scope.DepartmentList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DEPARTMENT&MasterCode=-1").
            success(function (data, status) {
                $scope.Department = data;
                if (data.length > 0) {
                    var DeoptCode = data.filter(function (x) { return x.DEPTNAMEENGLISH == 'COOPERATIVE' })[0].ID;
                    if (DeoptCode != undefined && DeoptCode != '' && DeoptCode != null) {
                       $scope.UserRegistration.DEPTCODE = DeoptCode;
                        $scope.GetAllSubOrganisationList();
                    }
                    else {
                        $scope.UserRegistration.DEPTCODE = '0';
                    }
                }
            }).
            error(function (data, status) {
                alert("Error while fetching department list");
                $scope.Department = [];
            });
    };

    $scope.GetAllSubOrganisationList = function () {
        $scope.Level = [];
        $http.get(SiteUrl + "/Master/OrganisationsList/?DepartmentId=" + $scope.UserRegistration.DEPTCODE).
            success(function (data, status) {
                $scope.OrganisationData = data;
                if (data.length > 0) {
                    if (data.filter(function (x) { return x.OrganisationNameEn == 'COOPERATIVE' }).length > 0) {
                        var OrgCode = data.filter(function (x) { return x.OrganisationNameEn == 'COOPERATIVE' })[0].OrganisationId;
                        if (OrgCode != undefined && OrgCode != '' && OrgCode != null) {
                            $scope.UserRegistration.OrganisationId = OrgCode;
                            $scope.LevelList();
                        }
                        else {
                            $scope.UserRegistration.OrganisationId = '0';
                        }
                    }
                }
            }).
            error(function (data1, status) {

            });
    };

    $scope.LevelList = function () {
        $scope.Level = [];
        $http.get(SiteUrl + "/MasterSettings/OrganizationLevelsList/?WhereClause=AND OLM.OrganizationId=" + $scope.UserRegistration.OrganisationId).
            success(function (data, status) {
                $scope.Level = data;
            }).
            error(function (data, status) {
            });
    };
    // Get List of Designations 
    //$scope.DesignationList = function () {
    //    $scope.Designations = [];
    //    $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=designation&MasterCode=" + $scope.UserRegistration.DEPTCODE).
    //        success(function (data, status) {
    //            $scope.Designations = data;
    //        }).
    //        error(function (data, status) {
    //            $scope.Designations = [];
    //        });
    //};
    //START : Get ResionMapping LIST
    $scope.GetResionMapping = function () {
        //alert($scope.UserRegistration.DistrictId);
        $scope.ResionIDs = "";
        $scope.ResionList = [];
        if ($scope.UserRegistration.OfficeId == 3 || $scope.UserRegistration.OfficeId == 41 || $scope.UserRegistration.OfficeId == 19) {  //
            if ($scope.UserRegistration.DistrictId != '0') {
                $scope.IsResion = true;
                // alert($scope.UserRegistration.DistrictId);
                $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=ResionMapping&MasterCode=" + $scope.UserRegistration.DistrictId).
                    success(function (data, status) {
                        $scope.IsResion = true;
                        data = data.filter(function (x) { return x.OrganisationId == $scope.UserRegistration.OrganisationId });
                        $("#ddlResion").empty();
                        //alert($scope.UserRegistration.OrganisationId);
                        $.each(data, function () {
                            $("#ddlResion").append($("<option></option>").val(this.ResionId).html(this.ResionName));
                        });
                        if ($scope.flag == 1) {
                            $('#ddlResion').multiselect('destroy');
                        }
                        $('#ddlResion').multiselect({
                            includeSelectAllOption: true
                        });
                        $scope.flag = 1;
                    }).
                    error(function (data, status) {
                        $scope.ResionList = [];
                        $scope.IsResion = false;
                    });
            }
            else {
                $scope.IsResion = false;
            }
        }
        else {
            $scope.IsResion = false;
        }
    };

    $scope.GetDesignationList = function () {
        debugger;
        $scope.Designations = [];


        $http.get(SiteUrl + "/MasterSettings/GetAllDesignationByOrganizationLevelsMappingID/?OrganizationLevelsMappingID=" + $scope.UserRegistration.OrganizationLevelsMappingID + "&OfficeId=" + $scope.UserRegistration.OfficeId).
            success(function (data, status) {
                $scope.Designations = data;
            }).
            error(function (data, status) {
                $scope.Designations = [];
            });
    };


    $scope.GetValuesForDesignationList = function () {
        debugger;
        $scope.Designations = [];
        $scope.UserRegistration.DistrictId = '0';
        $scope.UserRegistration.IsRuralUrban = false;
        $scope.UserRegistration.TehsilId = '0';
        $scope.UserRegistration.CityId = '0';
        $scope.UserRegistration.WardId = '0';
        $scope.UserRegistration.BlockId = '0';

        $scope.UserRegistration.ProcessingUnitId = '0';
        $scope.UserRegistration.WarehouseId = '0';
        $scope.UserRegistration.DepotId = '0';
        $scope.ProcessingUnitList = [];
        $scope.DEPORTList = [];
        $scope.PLDBBranches = [];
        $scope.WAREHOUSEList = [];

        var Level_OrganizationLevelsMappingID = $scope.UserRegistration.LevelId;
        if (Level_OrganizationLevelsMappingID.indexOf("~") >= 0) {
            var arrayOfStrings = Level_OrganizationLevelsMappingID.split("~");
            $scope.UserRegistration.OrganizationLevelsMappingID = arrayOfStrings[1];
            $scope.UserRegistration.WorkScopeLevel_ID = arrayOfStrings[0];

        }
        else {
            $scope.UserRegistration.OrganizationLevelsMappingID = 0;
            $scope.UserRegistration.WorkScopeLevel_ID = 0;
        }
        $scope.Levelofficedata();
        $scope.RoleList();
        $scope.OfficeList();
    };



    $scope.GetCityList = function () {
        $scope.ResionIDs = "";
        $scope.UserRegistration.OfficeId = '0';
        $scope.ResionList = [];
        $scope.IsResion = false;
        if ($scope.UserRegistration.DistrictId != '0') {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITY&MasterCode=" + $scope.UserRegistration.DistrictId).
                success(function (data, status) {
                    $scope.CityList = data;
                }).

                error(function (data, status) {
                    $scope.CityList = [];
                });
            //}
            //if ($scope.UserRegistration.IsRuralUrban == 'false') {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.UserRegistration.DistrictId).
                success(function (data, status) {
                    $scope.Blocks = data;
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                });


            $scope.ProcessingUnitList = [];
            $scope.WAREHOUSEList = [];
            $scope.DEPORTList = [];

            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PLANT&MasterCode=" + $scope.UserRegistration.DistrictId).
                success(function (data, status) {
                    $scope.ProcessingUnitList = data;
                }).
                error(function (data, status) {
                    $scope.ProcessingUnitList = [];
                });

            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DEPORT&MasterCode=" + $scope.UserRegistration.DistrictId).
                success(function (data, status) {
                    $scope.DEPORTList = data;
                }).
                error(function (data, status) {
                    $scope.DEPORTList = [];
                });

            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WAREHOUSE&MasterCode=" + $scope.UserRegistration.DistrictId).
                success(function (data, status) {
                    $scope.WAREHOUSEList = data;
                }).
                error(function (data, status) {
                    $scope.WAREHOUSEList = [];
                });
        }
        $scope.GramPanchayats = [];
    };

    $scope.OfficeList = function () {
        debugger;
        $scope.OfficeListData = [];
        $http.get(SiteUrl + "/Master/OfficesList/?WhereClause=and o.isactive =1 and   o.OrganizationLevelsMappingID=" + $scope.UserRegistration.OrganizationLevelsMappingID).
            success(function (data, status) {
                $scope.OfficeListData = data;
            }).
            error(function (data, status) {
                alert("Error while fetching designation list");
                $scope.OfficeListData = [];
                $scope.showLoader = false;
            });
    };

    //start Level of office data
    $scope.Levelofficedata = function () {
        if ($scope.UserRegistration.WorkScopeLevel_ID == '1') {
            $scope.RuralUrbanRedioDiv = false;
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
            $scope.IsDistrict = false;
            $scope.IsDivision = false;
            $scope.StateDiv = true;
            $scope.UserRegistration.GP_ID = '0';
            $scope.UserRegistration.VILLAGE_ID = '0';
            $scope.UserRegistration.BLOCK_ID = '0';
            $scope.UserRegistration.CITY_ID = '0';
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];
        }
        else if ($scope.UserRegistration.WorkScopeLevel_ID == '2') {
            $scope.RuralUrbanRedioDiv = false;
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
            $scope.IsDistrict = false;
            $scope.IsDivision = true;
            $scope.UserRegistration.GP_ID = '0';
            $scope.UserRegistration.VILLAGE_ID = '0';
            $scope.UserRegistration.BLOCK_ID = '0';
            $scope.UserRegistration.CITY_ID = '0';
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];

        }
        else if ($scope.UserRegistration.WorkScopeLevel_ID == '3') {
            $scope.RuralUrbanRedioDiv = false;
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
            $scope.IsDistrict = true;
            $scope.IsDivision = false;
            $scope.RuralUrbanRedioDiv = false;
            $scope.UserRegistration.GP_ID = '0';
            $scope.UserRegistration.VILLAGE_ID = '0';
            $scope.UserRegistration.BLOCK_ID = '0';
            $scope.UserRegistration.CITY_ID = '0';
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];
        }
        else if ($scope.UserRegistration.WorkScopeLevel_ID == '4') {
            $scope.RuralUrbanRedioDiv = true;
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
            $scope.IsDistrict = true;
            //$scope.IsDivision = true;
            $scope.IsDivision = false;
            $scope.UserRegistration.IsRuralUrban = 'false';
            $scope.UserRegistration.GP_ID = '0';
            $scope.UserRegistration.VILLAGE_ID = '0';
            $scope.UserRegistration.CITY_ID = '0';
            $scope.UserRegistration.BLOCK_ID = '0';
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];
        }
        else if ($scope.UserRegistration.WorkScopeLevel_ID == '5') {
            $scope.RuralUrbanRedioDiv = true;
            $scope.IsDistrict = true;
            //$scope.IsDivision = true;
            $scope.IsDivision = false;
            $scope.RuralurbanDiv = true;
            $scope.Addmore = true;
            $scope.UserRegistration.IsRuralUrban = 'false';
            $scope.RuralUrbanBlock();
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];
        }
        else if ($scope.UserRegistration.WorkScopeLevel_ID == '6') {
            $scope.RuralUrbanRedioDiv = true;
            $scope.IsDistrict = true;
            //$scope.IsDivision = true;
            $scope.IsDivision = false;
            $scope.RuralurbanDiv = true;
            $scope.IsBlock = true;
            $scope.UserRegistration.IsRuralUrban = 'false';
            $scope.RuralUrbanBlock();
            $scope.Blocks = [];
            $scope.CityList = [];
            $scope.GramPanchayats = [];
        }
        else {
            $scope.StateDiv = false;
            $scope.DivisionDiv = false;
            $scope.IsDistrict = false;
            $scope.CitytypDiv = false;
        }
    };
    //End Level of office data

    // START : Set Rural/ Urban div show/hide
    $scope.RuralUrbanBlock = function () {
        var SelectedVal = $scope.UserRegistration.IsRuralUrban;
        $("#ddlPanchayatSamiti").val('0');
        $("#ddlGramPanchayat").val('0');
        $("#ddlVillage").val('0');
        $("#ddlCity").val('0');
        $scope.UserRegistration.DivisionId = '0';
        $scope.UserRegistration.DistrictId = '0';
        $scope.UserRegistration.TehsilId = '0';
        $scope.UserRegistration.CityId = '0';
        $scope.UserRegistration.WardId = '0';
        $scope.UserRegistration.BlockId = '0';
        $scope.UserRegistration.ProcessingUnitId = '0';
        $scope.UserRegistration.WarehouseId = '0';
        $scope.UserRegistration.DepotId = '0';
        $scope.UserRegistration.GPId = '0';
        $scope.UserRegistration.VillageId = '0';
        $scope.CityList = [];
        $scope.Blocks = [];
        $scope.GramPanchayats = [];
        if (SelectedVal == 'true') {
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;
        }
        else if (SelectedVal == 'false') {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
        }
        else {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
        }
    };
    // END : Set Rural/ Urban div show/hide

    // Language Selector
    $scope.RoleList = function () {
        

    };

  
    $scope.DepartmentList();
    $scope.EncloserFilesList = [];

    $scope.DistrictList = function () {
        $scope.Districts = [{}];

        // $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=-1" + $scope.UserRegistration.DivisionId).
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=district&MasterCode=-1").
            success(function (data, status) {
                $scope.Districts = data;
            }).
            error(function (data, status) {
                $scope.Districts = [{}];
            });
    };


   
    //var gg = $scope.EncloserFilesList.length;
    //alert(gg);

 


    //$scope.AddFilemulti = function (type, index) {
    //    debugger;
    //    $scope.FileType = type;
    //    var ccc = index;
    //    $scope.FileTypeIndex = index;


    //}


    //$scope.UploadFilemulti = function (id, type) {
    //    debugger;
    //    try {
    //        // FormData
    //        if (window.FormData !== undefined) {
    //            var fileUpload = $("#" + id).get(0);
    //            var files = fileUpload.files;
    //            //var blob = b64toBlob(realData, contentType, 512);
    //            var formData = new FormData();
    //            formData.append("pldbfile", files[0]);
    //            $(".blockUI").show();
    //            $scope.xhrLVO(SiteUrl + "/pldb/UploadLoanFile", formData, type);
    //        } else {
    //            alert("FormData is not supported.");
    //        }
    //    }
    //    catch (ex) { alert(ex) }
    //}

    //$scope.deleteFime = function (index) {


    //    if ($scope.EncloserFilesList[index].Id > 0) {
    //        $scope.EncloserFilesList[index].Is_Delete = true
    //    } else {
    //        var list = $scope.LOVFilesList;
    //        $scope.EncloserFilesList = [];
    //        list.forEach(function (item, indx) {
    //            if (indx != index) {
    //                $scope.EncloserFilesList.push(item);
    //            }
    //        });
    //    }
    //}

    //$scope.ChangeFileuser = function (type, index) {

    //    $scope.LOVFilesList[index].IsUploadFileName = false;
    //    $scope.LOVFilesList[index].FileName = "";
    //    $("#LovFileName_" + index).val("");
    //}


    $scope.AddNew = function () {
        debugger;
        var SocietyInfo = {};
        $scope.SocietyId = SocietyInfo.SocietyID;
        $scope.SocietyInfo = SocietyInfo
        ////  $("#addModal").modal("show");
        $scope.showPriView = true;
        $scope.GetComplainStatus1();
       
    }

    $scope.LoadIOList = function (id) {
        $scope.GetIONameList();
    }

    $scope.AddNewIO = function (item) {
        $scope.SocietyId = item.SocietyId;
        $scope.StatusId = item.StatusId
        $scope.Complain_ID = item.ComplainID;
        $("#addModal").modal("show");
    }

    //------------------Start Complainant Details -----------------------------//




    $scope.ComplainantDetails = {
        CDetailID: "0",
        Name: "",
        MobileNo: "",
        Address: "",
        Post: "",
        IsSocietyMember: "",
        Ctype: "",
        IsActive: "",
        IsDelete: false
    }

    $scope.ResetComplainantDetails = function () {
        $scope.ComplainantDetails = {
            CDetailID: "0",
            Name: "",
            MobileNo: "",
            Address: "",
            Post: "",
            IsSocietyMember: "",
            Ctype: "",
            IsActive: "",
            IsDelete: false
        }
    }
    $scope.ComplainantDetailsList = [];
    $scope.ComplainantDetailsList.push($scope.ComplainantDetails);

    $scope.AddComplainantDetails = function () {
        if ($scope.ValidateComplainantDetails()) {
            $scope.ResetComplainantDetails();

            $scope.ComplainantDetailsList.push($scope.ComplainantDetails);
        }
    }

    $scope.RemoveComplainantDetailsRow = function (index) {
        ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove amount details from list.' : 'क्या आप वाकई सूची से राशि विवरण निकालना चाहते हैं', function () {
            $(".blockUI").fadeOut("slow");
            $scope.ComplainantDetailsList[index].IsDelete = true;
            $scope.$apply();
            $(".blockUI").fadeOut("slow");
        });
    }

    $scope.ValidateComplainantDetails = function () {
        var message = "";// $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblComplainantDetails").find('input[type="text"]').removeClass("notValid");
        $("#tblComplainantDetails").find('select').removeClass("notValid");

        $scope.ComplainantDetailsList.forEach(function (item, index) {

            //if ($scope.ComplainUnderSection.ComplantBy == '2') {
            //    if (item.Name == '') {

            //        if (message.indexOf($scope._language == 'en-us' ? 'Please enter a Name' : 'कृपया नाम दर्ज करें') < 0) {
            //            message += $scope._language == 'en-us' ? 'कृपया नाम दर्ज करें।' : 'कृपया नाम दर्ज करें।';
            //        }
            //        $("#txtName").addClass("notValid");
            //    }
            //    else {
            //        $("#txtName").removeClass("notValid");
            //    }

            //    if (item.Address == '') {
            //        if (message.indexOf($scope._language == 'en-us' ? 'Please enter an Address' : 'कृपया पता दर्ज करें') < 0) {
            //            message += $scope._language == 'en-us' ? '<br />कृपया पता दर्ज करें।' : '<br />कृपया पता दर्ज करें।';
            //        }
            //        $("#Address").addClass("notValid");
            //    }
            //    else {
            //        $("#Address").removeClass("notValid");
            //    }

            //    if (item.MobileNo == '') {
            //        if (message.indexOf($scope._language == 'en-us' ? 'Please enter Mobile Number' : 'कृपया मोबाइल नंबर दर्ज करें') < 0) {
            //            message += $scope._language == 'en-us' ? '<br />कृपया मोबाइल नंबर दर्ज करें।' : '<br />कृपया मोबाइल नंबर दर्ज करें।';
            //        }
            //        $("#txtMobile").addClass("notValid");
            //    }
            //    else {
            //        $("#txtMobile").removeClass("notValid");
            //    }

            //    if (item.IsSocietyMember && !item.IsSocietyMember) {
            //        if (item.IsSocietyMember == '') {
            //            if (message.indexOf($scope._language == 'en-us' ? 'This person is a member of the institution / committee? Choose' : 'यह व्यक्ति संस्था/समिति का सदस्य है? चुनें') < 0) {
            //                message += $scope._language == 'en-us' ? '<br />यह व्यक्ति संस्था/समिति का सदस्य है? चुनें।' : '<br />यह व्यक्ति संस्था/समिति का सदस्य है? चुनें।';
            //            }
            //        }
            //    }
            //}

        });

        if (message.length > 29) {
            ShowOkMessage("Add Member", message, function () { });
            return false;
        }
        return true;
    }




    //------------------End Complainant Details -----------------------------//







    //------------------Start Details of the complaint against -----------------------------//




    $scope.ComplaintAgainst = {

        CDetailID: "0",
        Name: "",
        MobileNo: "",
        Address: "",
        Post: "",
        IsSocietyMember: "",
        Ctype: "",
        IsActive: "",
        IsDelete: false
    }

    $scope.ResetComplaintAgainst = function () {
        $scope.ComplaintAgainst = {
            CDetailID: "0",
            Name: "",
            MobileNo: "",
            Address: "",
            Post: "",
            IsSocietyMember: "",
            Ctype: "",
            IsActive: "",
            IsDelete: false
        }
    }
    $scope.ComplaintAgainstList = [];
    $scope.ComplaintAgainstList.push($scope.ComplaintAgainst);

    $scope.AddComplaintAgainst = function () {
        if ($scope.ValidateComplaintAgainst()) {
            $scope.ResetComplaintAgainst();

            $scope.ComplaintAgainstList.push($scope.ComplaintAgainst);
        }
    }

    $scope.RemoveComplaintAgainstRow = function (index) {
        ShowAlertMessage("", $scope._language == 'en-us' ? 'Are you sure you want to remove amount details from list.' : 'क्या आप वाकई सूची से राशि विवरण निकालना चाहते हैं', function () {
            $scope.ComplaintAgainstList[index].IsDelete = true;
            $scope.$apply();
        });
    }

    $scope.ValidateComplaintAgainst = function () {
        var message = "";// $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        $("#tblComplainantDetails").find('input[type="text"]').removeClass("notValid");
        $("#tblComplainantDetails").find('select').removeClass("notValid");

        $scope.ComplaintAgainstList.forEach(function (item, index) {

            if ($scope.ComplainUnderSection.IsComplentForSociety == '2') {
                if (item.Name == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Registration Name' : 'जिसके खिलाफ शिकायत की गई है कृपया उसका नाम दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />जिसके खिलाफ शिकायत की गई है कृपया उसका नाम दर्ज करें।' : '<br />जिसके खिलाफ शिकायत की गई है कृपया उसका नाम दर्ज करें।';
                    }
                    $("#txtSocietyname").addClass("notValid");
                }
                else {
                    $("#txtSocietyname").removeClass("notValid");
                }

                if (item.Address == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Registration Name' : 'जिसके खिलाफ शिकायत की गई है कृपया उसका पता दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />जिसके खिलाफ शिकायत की गई है कृपया उसका पता दर्ज करें।' : '<br />जिसके खिलाफ शिकायत की गई है कृपया उसका पता दर्ज करें।';
                    }
                    $("#txtAddress").addClass("notValid");
                }
                else {
                    $("#txtAddress").removeClass("notValid");
                }

                if (item.Post == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Registration Name' : 'जिसके खिलाफ शिकायत की गई है कृपया उसका पद दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />जिसके खिलाफ शिकायत की गई है कृपया उसका पद दर्ज करें।' : '<br />जिसके खिलाफ शिकायत की गई है कृपया उसका पद दर्ज करें।';
                    }
                    $("#txtPost").addClass("notValid");
                }
                else {
                    $("#txtPost").removeClass("notValid");
                }
            }

        });

        if (message.length > 29) {
            ShowOkMessage("Add Member", message, function () { });
            return false;
        }
        return true;
    }




    //------------------End Details of the complaint against -----------------------------//



    $scope.ComplainUnderSection = {

        ComplainID: "0",
        SocietyId: "",
        //SSOID: "",
        // UserID: "",
        ComplantBy: "",
        IsComplentForSociety: "",
        Formation_ComplaintDetails: "",
        Business_ComplaintDetails: "",
        Financial_ComplaintDetails: "",

        StatusId: "",
        ComplainDate: "",
        IsActive: "",
        // CreatedBy: "",
        CreationDate: "0",
        //UpdatedBy: "0",
        UpdationDate: "",
        CreatedIP: "",
        UpdatedIP: "",
        ComplainDate: "",
        FromComplainDate1:"",
        ToComplainDate1: "",
         FromComplainDate2: "",
        ToComplainDate2: "",
         FromComplainDate3: "",
        ToComplainDate3: "",
        Financial: "0",
        Business: "0",
        Formation :"0"







    }


    //$scope.FileUpload = "";
    //$scope.ComplaintDate = "";
    //$scope.IO_Name = "";
    //$scope.MemberType = "";
    //$scope.SocietyType = "";


    $scope.ComplainUnderSection = [];
    $scope.ComplainUnderSection.push();
    $scope.validateComplainUnderSection = function () {

        var message = "";
        $("#tblLoanMembersDetail").find('input[type="text"]').removeClass("notValid");
        $("#tblLoanMembersDetail").find('select').removeClass("notValid");


        if ($("#tbleSocietyList").find("input[type='radio']:checked").length == 0) {
            message += $scope._language == 'en-us' ? 'Please choose society from society filter.' : 'कृपया सोसाइटी फ़िल्टर से सोसाइटी चुनें';
        }


        if ($scope.ComplainUnderSection.ComplantBy == '' || $scope.ComplainUnderSection.ComplantBy == undefined) {

            if (message.indexOf($scope._language == 'en-us' ? 'Please choose the complainant.' : 'कृपया शिकायतकर्ता चुनें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया शिकायतकर्ता चुनें।' : '<br />कृपया शिकायतकर्ता चुनें।';
            }
        }

        //if ($scope.ComplainUnderSection.ComplantBy == '' || $scope.ComplainUnderSection.IsComplentForSociety == undefined) {

        //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Small Industry Registration' : 'जिसके लिए शिकायत है| चुनें') < 0) {
        //        message += $scope._language == 'en-us' ? '<br />जिसके लिए शिकायत है| चुनें।' : '<br />जिसके लिए शिकायत है| चुनें।';
        //    }
        //}


        if ($scope.ComplainUnderSection.ComplantBy != '4') {

            if ($scope.ComplainTrail.FileName == '' || $scope.ComplainTrail.FileName == undefined) {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया फाइल अपलोड करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />कृपया फाइल  अपलोड करें।.' : '<br />कृपया फाइल  अपलोड करें।.';
                }
            }
        }
      

        if ($scope.EncloserFilesList.length == 1 ) {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Select Encloser File' : 'कृपया एन्क्लोज़र फ़ाइल अपलोड करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया एन्क्लोज़र फ़ाइल  अपलोड करें।.' : '<br />कृपया एन्क्लोज़र फ़ाइल  अपलोड करें।.';
            }
        }

        
        //if ($("#ComplaintDate").val() == '') {
        //    if (message.indexOf($scope._language == 'en-us' ? 'Please enter business license registration Description.' : 'कृपया शिकायत की तारीख दर्ज करें') < 0) {
        //        message += $scope._language == 'en-us' ? '<br />कृपया शिकायत की तारीख दर्ज करें।' : '<br />कृपया शिकायत की तारीख दर्ज करें।';
        //    }
        //    $("#ComplaintDate").addClass("notValid");
        //}
        //else {
        //    $("#ComplaintDate").removeClass("notValid");
        //}

        //if ($("#ComplaintDate1").val() == '') {
        //    if (message.indexOf($scope._language == 'en-us' ? 'Please enter business license registration Description.' : 'कृपया शिकायत की तारीख दर्ज करें') < 0) {
        //        message += $scope._language == 'en-us' ? '<br />कृपया शिकायत की तारीख दर्ज करें।' : '<br />कृपया शिकायत की तारीख दर्ज करें।';
        //    }
        //    $("#ComplaintDate1").addClass("notValid");
        //}
        //else {
        //    $("#ComplaintDate1").removeClass("notValid");
        //}

        if ($("#ComplaintDate6").val() == '') {
            if (message.indexOf($scope._language == 'en-us' ? 'Please enter business license registration Description.' : 'कृपया शिकायत की तारीख दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया शिकायत की तारीख दर्ज करें।' : '<br />कृपया शिकायत की तारीख दर्ज करें।';
            }
            $("#ComplaintDate6").addClass("notValid");
        }
        else {
            $("#ComplaintDate6").removeClass("notValid");
        }

        if ($scope.IODetail.UserId == '0') {
            if (message.indexOf($scope._language == 'en-us' ? 'Please enter business license registration Description.' : 'कृपया जांच अधिकारी का नाम चुनें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया जांच अधिकारी का नाम चुनें।' : '<br />कृपया जांच अधिकारी का नाम चुनें।';
            }
            $("#ddlIO_Name").addClass("notValid");
        }
        else {
            $("#ddlIO_Name").removeClass("notValid");
        }

        debugger;

        if ($scope.ComplainUnderSection.Period == '' || $scope.ComplainUnderSection.Period == undefined) {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Period' : 'कृपया अवधि दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया अवधि दर्ज करें|.' : '<br />कृपया अवधि दर्ज करें|';
            }
            $("#InvestigationPeriod").addClass("notValid");
        } else {
            $("#InvestigationPeriod").removeClass("notValid");
        }

        //if ($scope.SocietyInfo.ClassId == 6 &&($scope.ComplainUnderSection.Scheme == '' || $scope.ComplainUnderSection.Scheme == undefined)) {
        //    if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Period' : 'कृपया योजना का नाम दर्ज करें') < 0) {
        //        message += $scope._language == 'en-us' ? '<br />कृपया योजना का नाम दर्ज करें|.' : '<br />कृपया योजना का नाम दर्ज करें|';
        //    }
        //    $("#txtScheme").addClass("notValid");
        //} else {
        //    $("#txtScheme").removeClass("notValid");
        //}


        if ($("#HtmlContent").val() == '') {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Period' : 'कृपया शिकायतकर्ता का विस्तृत विवरण दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया शिकायतकर्ता का विस्तृत विवरण दर्ज करें|.' : '<br />कृपया शिकायतकर्ता का विस्तृत विवरण दर्ज करें|';
            }
            $("#HtmlContent").addClass("notValid");
        } else {
            $("#HtmlContent").removeClass("notValid");
        }


        if ($scope.UserRegistration.WorkScopeLevel_ID == 0 || $scope.UserRegistration.WorkScopeLevel_ID == undefined) {

            debugger;
            if (message.indexOf($scope._language == 'en-us' ? 'Please Select Work Scope Level' : 'कृपया कार्य स्कोप स्तर का चयन करे') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया कार्य स्कोप स्तर का चयन करे|' : '<br />कृपया कार्य स्कोप स्तर का चयन करे|';
            }

            $("#ddllevel").addClass("notValid");
        }
        else {
            $("#ddllevel").removeClass("notValid");
        }

        if ($scope.ComplainTrail.Remark == '' || $scope.ComplainTrail.Remark == undefined) {
            if (message.indexOf($scope._language == 'en-us' ? 'कृपया टिप्पणी दर्ज करें' : 'कृपया टिप्पणी दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया टिप्पणी दर्ज  करें।' : '<br />कृपया टिप्पणी दर्ज करें।';
            }
            $("#Remark").addClass("notValid");
        }
        else {
            $("#Remark").removeClass("notValid");
        }
    

        if ($scope.UserRegistration.OfficeId == '' || $scope.UserRegistration.OfficeId == undefined || $scope.UserRegistration.OfficeId == '0') {
            if (message.indexOf($scope._language == 'en-us' ? 'कृपया कार्यालय दर्ज करें' : 'कृपया कार्यालय दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया कार्यालय दर्ज करें' : '<br />कृपया कार्यालय दर्ज करें।';
            }

            $("#ddlofficedata").addClass("notValid");
        }
        else {
            $("#ddlofficedata").removeClass("notValid");
        }


        if ($scope.UserRegistration.DesignationLevelMappingId == '' || $scope.UserRegistration.DesignationLevelMappingId == undefined || $scope.UserRegistration.DesignationLevelMappingId == '0') {
            if (message.indexOf($scope._language == 'en-us' ? 'कृपया पद दर्ज करें' : 'कृपया पद दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया पद दर्ज करें' : '<br />कृपया पद दर्ज करें।';
            }
            $("#ddldesination").addClass("notValid");
        }
        else {
            $("#ddldesination").removeClass("notValid");
        }
        //division
        if ($scope.UserRegistration.WorkScopeLevel_ID != undefined && $scope.UserRegistration.WorkScopeLevel_ID == 2) {
            if ($scope.UserRegistration.DivisionId == '0' || $scope.UserRegistration.DivisionId == undefined || $scope.UserRegistration.DivisionId == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'कृपया संभाग दर्ज करें' : 'कृपया संभाग दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />कृपया संभाग दर्ज करें' : '<br />कृपया संभाग दर्ज करें।';
                }
                $("#ddlDivision").addClass("notValid");
            }
            else {
                $("#ddlDivision").removeClass("notValid");
            }
        }


        if ($scope.UserRegistration.WorkScopeLevel_ID != undefined && ($scope.UserRegistration.WorkScopeLevel_ID > 2)) {
            if ($scope.UserRegistration.DistrictId == '0' || $scope.UserRegistration.DistrictId == undefined || $scope.UserRegistration.DistrictId == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Select District' : 'कृपया जिले का चयन करे') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Select District' : '<br />\nकृपया जिले का चयन करे।';
                }
                $("#ddlDistrict").addClass("notValid");
            }
            else {
                $("#ddlDistrict").removeClass("notValid");
            }
        }


        if ($scope.UserRegistration.WorkScopeLevel_ID != undefined && $scope.UserRegistration.WorkScopeLevel_ID > 3) {
            if ($scope.UserRegistration.IsRuralUrban != undefined && ($scope.UserRegistration.IsRuralUrban == true || $scope.UserRegistration.IsRuralUrban == 'true')) {
                if ($scope.UserRegistration.CITY_ID != undefined && $scope.UserRegistration.CITY_ID == 0) {
                    message += $scope._language == 'en-us' ? '\n<br />Please Select City' : '\nकृपया शहर का चयन करे |';
                    $("#ddlCity").addClass("notValid");
                }
                else {
                    $("#ddlCity").removeClass("notValid");
                }
            }
            else if ($scope.UserRegistration.IsRuralUrban != undefined && ($scope.UserRegistration.IsRuralUrban == false || $scope.UserRegistration.IsRuralUrban == 'false')) {
                if ($scope.UserRegistration.BlockId != undefined && $scope.UserRegistration.BlockId == 0) {
                    message += $scope._language == 'en-us' ? '\n<br />Please Select Block' : '\nकृपया खंड का चयन करे |';
                    $("#ddlPanchayatSamiti").addClass("notValid");
                }
                else {
                    $("#ddlPanchayatSamiti").removeClass("notValid");
                }
            }
        }

        if ($scope.UserRegistration.WorkScopeLevel_ID != undefined && $scope.UserRegistration.WorkScopeLevel_ID > 4) {
            if ($scope.UserRegistration.GPId != undefined && $scope.UserRegistration.GPId == 0) {
                message += $scope._language == 'en-us' ? '\n<br />Please Select Gram Panchayat' : '\nकृपया ग्राम पंचायत का चयन करे |';
                $("#ddlGramPanchayat").addClass("notValid");
            }
            else {
                $("#ddlGramPanchayat").removeClass("notValid");
            }
        }

        if ($scope.UserRegistration.WorkScopeLevel_ID != undefined && $scope.UserRegistration.WorkScopeLevel_ID > 5) {
            if ($scope.UserRegistration.VillageId != undefined && $scope.UserRegistration.VillageId == 0) {
                message += $scope._language == 'en-us' ? '\n<br />Please Select Village' : '\nकृपया ग्राम का चयन करे |';
                $("#ddlVillage").addClass("notValid");
            }
            else {
                $("#ddlVillage").removeClass("notValid");
            }
        }


        if (document.getElementById('build').checked == false && document.getElementById('business').checked == false && document.getElementById('member').checked == false) {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Select Investigation Point' : 'कृपया जाँच बिंदु  का चयन करे।') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया जाँच बिंदु  का चयन करे।' : '<br />\nकृपया जाँच बिंदु  का चयन करे।';
            }
        }
        else {

            if (document.getElementById('build').checked == true) {
                if ($("#ComplaintDate").val() == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter Formation Related From Date' : 'कृपया शिकायत की तारीख से दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />कृपया गठन से सम्बन्धीत शिकायत की तारीख से दर्ज करें।' : '<br />कृपया गठन से सम्बन्धीत शिकायत की तारीख से दर्ज करें।';
                    }
                    $("#ComplaintDate").addClass("notValid");
                }
                else {
                    $("#ComplaintDate").removeClass("notValid");
                }


                if ($("#ComplaintDate1").val() == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter business license registration Description.' : 'कृपया गठन से सम्बन्धीत शिकायत की तारीख दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />कृपया गठन से सम्बन्धीत शिकायत की तारीख तक दर्ज करें।' : '<br />कृपया गठन से सम्बन्धीत शिकायत की तारीख दर्ज करें।';
                    }
                    $("#ComplaintDate1").addClass("notValid");
                }
                else {
                    $("#ComplaintDate1").removeClass("notValid");
                }


                if ($("#HtmlContent6").val() == '') {
                    $("#HtmlContent6").addClass("notValid");
                }
                else {
                    $("#HtmlContent6").removeClass("notValid");
                }
            }


            if (document.getElementById('business').checked == true) {
                debugger;

                if ($("#ComplaintDate2").val() == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter business license registration Description.' : 'कृपया कारोबार सम्बन्धीत शिकायत की तारीख से दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />कृपया कारोबार सम्बन्धीत शिकायत की तारीख से दर्ज करें।' : '<br />कृपया कारोबार सम्बन्धीत शिकायत की तारीख से दर्ज करें।';
                    }
                    $("#ComplaintDate2").addClass("notValid");
                }
                else {
                    $("#ComplaintDate2").removeClass("notValid");
                }


                if ($("#ComplaintDate3").val() == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter business license registration Description.' : 'कृपया कारोबार सम्बन्धीत शिकायत की तारीख तक दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />कृपया कारोबार सम्बन्धीत शिकायत की तारीख तक दर्ज करें।' : '<br />कृपया कारोबार सम्बन्धीत शिकायत की तारीख तक दर्ज करें।';
                    }
                    $("#ComplaintDate3").addClass("notValid");
                }
                else {
                    $("#ComplaintDate3").removeClass("notValid");
                }


                if ($("#HtmlContent5").val() == '') {
                    $("#HtmlContent5").addClass("notValid");
                }
                else {
                    $("#HtmlContent5").removeClass("notValid");
                }
            }


            if (document.getElementById('member').checked == true) {
                debugger;

                if ($("#ComplaintDate4").val() == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter business license registration Description.' : 'कृपया वितीय स्थिति शिकायत की तारीख से दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />कृपया वितीय स्थिति शिकायत की तारीख से दर्ज करें।' : '<br />कृपया वितीय स्थिति शिकायत की तारीख से दर्ज करें।';
                    }
                    $("#ComplaintDate4").addClass("notValid");
                }
                else {
                    $("#ComplaintDate4").removeClass("notValid");
                }


                if ($("#ComplaintDate5").val() == '') {
                    if (message.indexOf($scope._language == 'en-us' ? 'Please enter business license registration Description.' : 'कृपया वितीय स्थिति शिकायत की तारीख तक दर्ज करें') < 0) {
                        message += $scope._language == 'en-us' ? '<br />कृपया वितीय स्थिति शिकायत की तारीख तक दर्ज करें।' : '<br />कृपया वितीय स्थिति शिकायत की तारीख तक दर्ज करें।';
                    }
                    $("#ComplaintDate5").addClass("notValid");
                }
                else {
                    $("#ComplaintDate5").removeClass("notValid");
                }


                if ($("#HtmlContent4").val() == '') {
                    $("#HtmlContent4").addClass("notValid");
                }
                else {
                    $("#HtmlContent4").removeClass("notValid");
                }
            }

            if (document.getElementById('idAnyMatter').checked == true) {

                if ($scope.ComplainUndeVlaue.AnyOtherMatterText == undefined || $scope.ComplainUndeVlaue.AnyOtherMatterText == '') {
                    message += $scope._language == 'en-us' ? '\n<br />Please Enter Enquiry Related Matter ' : '\nकृपया अन्य संबंधित बात दर्ज करें |';
                  
                }
            }

        }
       

      



        if (message.length > 29) {
            ShowOkMessage("निम्नलिखित मान्य करें", message, function () { });
            return false;
        }
        return true;
    }



    $scope.ComplainUnderSection = [];
    $scope.ComplainUnderSection.push();
    $scope.validateUpdateComplainUnderSection = function () {

        debugger;
        var message = "";
        $("#tblLoanMembersDetail").find('input[type="text"]').removeClass("notValid");
        $("#tblLoanMembersDetail").find('select').removeClass("notValid");



        if ($scope.ComplainUnderSection.ComplantBy == '' || $scope.ComplainUnderSection.ComplantBy == undefined) {

            if (message.indexOf($scope._language == 'en-us' ? 'Please choose the complainant.' : 'कृपया शिकायतकर्ता चुनें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया शिकायतकर्ता चुनें।' : '<br />कृपया शिकायतकर्ता चुनें।';
            }
        }

        //if ($scope.ComplainUnderSection.IsComplentForSociety == '' || $scope.ComplainUnderSection.IsComplentForSociety == undefined) {

        //    if (message.indexOf($scope._language == 'en-us' ? 'Please Select Small Industry Registration' : 'जिसके लिए शिकायत है| चुनें') < 0) {
        //        message += $scope._language == 'en-us' ? '<br />जिसके लिए शिकायत है| चुनें।' : '<br />जिसके लिए शिकायत है| चुनें।';
        //    }
        //}

        if ($scope.ComplainTrail.FileName == '' || $scope.ComplainTrail.FileName == undefined) {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया फाइल अपलोड करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया फाइल  अपलोड करें।.' : '<br />कृपया फाइल  अपलोड करें।.';
            }
        }

        if ($("#ComplaintDate").val() == '') {
            if (message.indexOf($scope._language == 'en-us' ? 'Please enter business license registration Description.' : 'कृपया शिकायत की तारीख दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया शिकायत की तारीख दर्ज करें।' : '<br />कृपया शिकायत की तारीख दर्ज करें।';
            }
            $("#ComplaintDate").addClass("notValid");
        }
        else {
            $("#ComplaintDate").removeClass("notValid");
        }

        if ($scope.IODetail.UserId == '0') {
            if (message.indexOf($scope._language == 'en-us' ? 'Please enter business license registration Description.' : 'कृपया जांच अधिकारी का नाम चुनें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया जांच अधिकारी का नाम चुनें।' : '<br />कृपया जांच अधिकारी का नाम चुनें।';
            }
            $("#ddlIO_Name").addClass("notValid");
        }
        else {
            $("#ddlIO_Name").removeClass("notValid");
        }
        if ($scope.ComplainUnderSection.Status == 7) {

            if ($("#HtmlContent2").val() == '') {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Result' : 'कृपया विस्तृत विवरण दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />कृपया विस्तृत विवरण दर्ज करें|.' : '<br />कृपया विवरण दर्ज करें|';
                }
                $("#HtmlContent2").addClass("notValid");
            } else {
                $("#HtmlContent2").removeClass("notValid");
            }
        }
        if ($scope.ComplainUnderSection.Period == '' || $scope.ComplainUnderSection.Period == undefined) {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Period' : 'कृपया अवधि दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया  अवधि दर्ज करें|.' : '<br />कृपया  अवधि दर्ज करें|.';
            }
            $("#InvestigationPeriod").addClass("notValid");
        } else {
            $("#InvestigationPeriod").removeClass("notValid");
        }

        if ($scope.SocietyInfo.ClassId == 6 && ($scope.ComplainUnderSection.Scheme == '' || $scope.ComplainUnderSection.Scheme == undefined)) {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Period' : 'कृपया योजना का नाम दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया योजना का नाम दर्ज करें|.' : '<br />कृपया योजना का नाम दर्ज करें|';
            }
            $("#txtScheme").addClass("notValid");
        } else {
            $("#txtScheme").removeClass("notValid");
        }


        if ($("#HtmlContent").val() == '') {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Period' : 'कृपया शिकायतकर्ता का विस्तृत विवरण दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया शिकायतकर्ता का विस्तृत विवरण दर्ज करें|.' : '<br />कृपया शिकायतकर्ता का विस्तृत विवरण दर्ज करें|';
            }
            $("#HtmlContent").addClass("notValid");
        } else {
            $("#HtmlContent").removeClass("notValid");
        }


        if ($scope.ComplainTrail.Remark == '' || $scope.ComplainTrail.Remark == undefined) {
            if (message.indexOf($scope._language == 'en-us' ? 'कृपया टिप्पणी दर्ज करें' : 'कृपया टिप्पणी दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया टिप्पणी दर्ज  करें।' : '<br />कृपया टिप्पणी दर्ज करें।';
            }
            $("#Remark").addClass("notValid");
        }
        else {
            $("#Remark").removeClass("notValid");
        }
        
        if (($scope.ComplainUnderSection.Status == '' || $scope.ComplainUnderSection.Status == undefined || $scope.ComplainUnderSection.Status == '0') && $scope.ComplainUnderSection.StatusId == 3) {
            if (message.indexOf($scope._language == 'en-us' ? 'कृपया स्थिति दर्ज चुनें' : 'कृपया स्थिति दर्ज चुनें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया स्थिति दर्ज  चुनें।' : '<br />कृपया स्थिति दर्ज चुनें।';
            }
            $("#Remark").addClass("notValid");
        }
        else {
            $("#Remark").removeClass("notValid");
        }

        if ($scope.ComplainUnderSection.StatusId == 6 && $scope.ComplainUnderSection.Status == "") {
            message += $scope._language == 'en-us' ? '<br />कृपया स्थिति दर्ज  चुनें।' : '<br />कृपया स्थिति दर्ज चुनें।';
            $("#CaseStatusId").addClass("notValid");

        }


      

        if (message.length > 29) {
            ShowOkMessage("निम्नलिखित मान्य करें", message, function () { });
            return false;
        }
        return true;
    }



    $scope.IODetail = {
        IODetailID: "0",
        ComplainID: "",
        SSOID: "",
        UserId: "",
        IsActive: ""

    }

    $scope.ComplainTrail = {
        ComplainTrailID: "0",
        ComplainID: "",
        ComplaintStatusId: "",
        FileName: "",
        Remark: "",
        IsActive: ""

    }


    $scope.SocietyChangeModel = {};

    //-----------SaveUser----------------------Bhupendra

    $scope.SaveUser = function (TxnNo) {
        debugger;
        //TxnNo
        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();

        var ComplainantDetailsList = [];
        $scope.ComplainantDetailsList.forEach(function (itm) {
            ComplainantDetailsList.push({
                CDetailID: itm.CDetailID == undefined ? 0 : itm.CDetailID,
                Name: itm.Name,
                MobileNo: itm.MobileNo,
                Address: itm.Address,
                Post: itm.Post,
                IsSocietyMember: itm.IsSocietyMember == "1" ? true : false,
                Ctype: true,
                IsActive: itm.IsActive,
                IsDelete: itm.IsDelete
            });
        });

        var ComplaintAgainstList = [];
        $scope.ComplaintAgainstList.forEach(function (itm) {
            ComplaintAgainstList.push({
                CDetailID: itm.CDetailID == undefined ? 0 : itm.CDetailID,
                Name: itm.Name,
                Address: itm.Address,
                Post: itm.Post,
                Ctype: false,
                IsActive: itm.IsActive == 'True' ? 1 : 0,
                IsDelete: itm.IsDelete
            });
        });
        var IODetailList = [];
        IODetailList.push({
            IODetailID: $scope.IODetail.IODetailID == undefined ? 0 : $scope.IODetail.IODetailID,
            UserId: $scope.IODetail.UserId,
            IsActive: $scope.IODetail.IsActive,
            OfficerNameEn: $scope.UserReqData.filter(function (x) { return x.UserId.toString() === $scope.IODetail.UserId.toString() })[0].OfficerNameEn,
            OfficerNameHi: $scope.UserReqData.filter(function (x) { return x.UserId.toString() === $scope.IODetail.UserId.toString() })[0].OfficerNameHi,
            SSOID: $scope.UserReqData.filter(function (x) { return x.UserId.toString() === $scope.IODetail.UserId.toString() })[0].SSO_ID,
            Period: $scope.ComplainUnderSection.Period,
            DEPT_ID: $scope.UserRegistration.DEPTCODE == undefined ? 0 : $scope.UserRegistration.DEPTCODE,
            OrganisationId: $scope.UserRegistration.OrganisationId,
            LevelId: $scope.UserRegistration.WorkScopeLevel_ID,
            DIVISION_ID: $scope.UserRegistration.DivisionId,
            DISTRICT_ID: $scope.UserRegistration.DistrictId,
            CITY_ID: $scope.UserRegistration.CITY_ID,
            BLOCK_ID: $scope.UserRegistration.BLOCK_ID,
            OfficeId: $scope.UserRegistration.OfficeId,
            DesignationLevelMappingId: $scope.UserRegistration.DesignationLevelMappingId
        });


        //By Chief Executive Officer table add
        //var ChiefExecutive = [];
        var PersonDetailsList1 = [];
        $scope.OtherPersonList.forEach(function (itm) {
            debugger;
            PersonDetailsList1.push({
                CPersonId: itm.PersonId,
                Name: itm.Name,
                Address: itm.Address,
                Post: itm.Post
            });
        });



        var ComplainTrail = [];
        ComplainTrail.push({
            ComplainTrailID: $scope.ComplainTrail.ComplainTrailID == undefined ? 0 : $scope.ComplainTrail.ComplainTrailID,
            // ComplainID: $scope.IODetail.ComplainID,  
            ComplaintStatusId: $scope.ComplainTrail.ComplaintStatusId,
            FileName: $scope.ComplainTrail.FileName,
            Remark: $scope.ComplainTrail.Remark,
            IsActive: $scope.ComplainTrail.IsActive
        });



        var encloserfile = '';
        var EncloserFilesList = [];
        $scope.EncloserFilesList.forEach(function (itm) {
            if (itm.FileName != '' && itm.FileName != 'undefined') {
                encloserfile += (encloserfile != '' ? ',' : '') + itm.FileName;
               // encloserfile += ',';
            }
        });



        var ComplainDate = $("#ComplaintDate6").val();
        var complainDetails = $("#HtmlContent").val();
        var Investigation1date = $("#ComplaintDate").val();
        var Investigation1todate = $("#ComplaintDate1").val();
        var Investigation2date = $("#ComplaintDate2").val();
        var Investigation2todate = $("#ComplaintDate3").val();
        var Investigation3date = $("#ComplaintDate4").val();
        var Investigation3todate = $("#ComplaintDate5").val();
        
        //var SocietyId = $("#SocietyID").val();
        var SocietyId = $("#tbleSocietyList").find("input[type='radio']:checked").val();
        var applicationDetails = {

            OtherDescription: $scope.ComplainUndeVlaue.AnyOtherMatterText,
            Investigation1: document.getElementById('build').checked == true ? true : false,
            Investigation2: document.getElementById('business').checked == true ? true : false,
            Investigation3: document.getElementById('member').checked == true ? true : false,
            Investigation1date: Investigation1date,
            Investigation1todate: Investigation1todate,
            Investigation2date: Investigation2date,
            Investigation2todate: Investigation2todate,
            Investigation3date: Investigation3date,
            Investigation3todate: Investigation3todate,
            Investigation1Detilas: $("#HtmlContent6").val() != "" || $("#HtmlContent6").val() != null ? $("#HtmlContent6").val() : " ",
            Investigation2Detilas: $("#HtmlContent5").val() != "" || $("#HtmlContent5").val() != null ? $("#HtmlContent5").val() : " ",
            Investigation3Detilas: $("#HtmlContent4").val() != "" || $("#HtmlContent4").val() != null ? $("#HtmlContent4").val() : " ",
            EncloserFileName: encloserfile,
            ComplainID: $scope.ComplainUnderSection.ComplainID == undefined ? 0 : $scope.ComplainUnderSection.ComplainID,
            SocietyId: SocietyId,//$scope.SocietyId,//SocietyId,
            ComplantBystatus: $scope.ComplainUnderSection.ComplantBy,
            SSOID: $scope.SSOID,
            UserID: $scope.UserID,
            IsComplentForSociety: $scope.ComplainUnderSection.IsComplentForSociety == "2" ? true : false,
            StatusId: $scope.ComplainUnderSection.StatusId,
            ComplainDate: ComplainDate,
            IsActive: $scope.ComplainUnderSection.IsActive,
            Scheme: $scope.ComplainUnderSection.Scheme,
            SocietyName: ($scope.SocietyInfo.ClassId == 6 ? "गृह निर्माण सहकारी समिति " : "") + $scope.SocietyInfo.SocietyName + " " + ($scope.SocietyInfo.ClassId == 6 ? $scope.ComplainUnderSection.Scheme : ""),
            CityType: $scope.CityType,
            ComplainantDetailsList: ComplainantDetailsList,
            ComplaintAgainstList: ComplaintAgainstList,
            IODetailList: IODetailList,
            ComplainTrailList: ComplainTrail,
            PersonDetailsList: PersonDetailsList1,
            TXN:TxnNo
        }
        formData.append("Applicationdata", JSON.stringify(applicationDetails));
        formData.append("ComplainantDetailsList", JSON.stringify($scope.ComplainantDetailsList));
        formData.append("IODetailList", JSON.stringify($scope.IODetailList));
        formData.append("ComplainTrailList", JSON.stringify($scope.ComplainTrailList));
        formData.append("PersonDetailsList", JSON.stringify($scope.OtherPersonList));//bhupendra

        $http.post(SiteUrl + "/SocietyComplaintCourtModule/SaveComplainUnderSection", JSON.stringify({ ApplicationModel: applicationDetails })).success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");

            if (data.Result != "-1" && data.Result != "0") {
                ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () {
                    window.location.href = "../SocietyComplaintCourtModule/SocietyComplaintCourtModule";
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



    //$scope.AddNewGuarantor = function () {
    //    if ($scope.ValidateAddGuarantor(1)) {
    //        $scope.ResetGuarantor();
    //        // $scope.Member.GramPanchayats = $scope.LoanMembers[$scope.LoanMembers.length - 1].GramPanchayats;
    //        $scope.LoanGuarantor.push($scope.Guarantor);
    //        $scope.AddNewGuarantorPropertyDetails()
    //    }
    //}

    //------------------Start Save User-----------------------------//

    //$scope.SocietyInfo = [];

    //$scope.SaveUser = function (TransId) {
    //    $(".se-pre-con").fadeIn("slow");
    //    var formData = new FormData();


    //    var ComplainantDetailsList = [];
    //    $scope.ComplainantDetailsList.forEach(function (itm) {
    //        ComplainantDetailsList.push({
    //            CDetailID: itm.CDetailID == undefined ? 0 : itm.CDetailID,
    //            Name: itm.Name,
    //            MobileNo: itm.MobileNo,
    //            Address: itm.Address,
    //            Post: itm.Post,
    //            IsSocietyMember: itm.IsSocietyMember == "1" ? true : false,
    //            Ctype: true,
    //            IsActive: itm.IsActive,
    //            IsDelete: itm.IsDelete
    //        });
    //    });



    //    var ComplaintAgainstList = [];
    //    $scope.ComplaintAgainstList.forEach(function (itm) {
    //        ComplaintAgainstList.push({
    //            CDetailID: itm.CDetailID == undefined ? 0 : itm.CDetailID,
    //            Name: itm.Name,
    //            Address: itm.Address,
    //            Post: itm.Post,
    //            Ctype: false,
    //            IsActive: itm.IsActive == 'True' ? 1 : 0,
    //            IsDelete: itm.IsDelete
    //        });
    //    });



    //    var IODetailList = [];
    //    IODetailList.push({
    //        IODetailID: $scope.IODetail.IODetailID == undefined ? 0 : $scope.IODetail.IODetailID,
    //        UserId: $scope.IODetail.UserId,
    //        IsActive: $scope.IODetail.IsActive,
    //        OfficerNameEn: $scope.UserReqData.filter(function (x) { return x.UserId.toString() === $scope.IODetail.UserId.toString() })[0].OfficerNameEn,
    //        OfficerNameHi: $scope.UserReqData.filter(function (x) { return x.UserId.toString() === $scope.IODetail.UserId.toString() })[0].OfficerNameHi,
    //        SSOID: $scope.UserReqData.filter(function (x) { return x.UserId.toString() === $scope.IODetail.UserId.toString() })[0].SSO_ID,
    //        Period: $scope.ComplainUnderSection.Period,
    //    });

    //    var ComplainTrail = [];
    //    ComplainTrail.push({
    //        ComplainTrailID: $scope.ComplainTrail.ComplainTrailID == undefined ? 0 : $scope.ComplainTrail.ComplainTrailID,
    //        // ComplainID: $scope.IODetail.ComplainID,  
    //        ComplaintStatusId: $scope.ComplainTrail.ComplaintStatusId,
    //        FileName: $scope.ComplainTrail.FileName,
    //        Remark: $scope.ComplainTrail.Remark,
    //        IsActive: $scope.ComplainTrail.IsActive
    //    });

    //    var ComplainDate = $("#ComplaintDate").val();
    //    var complainDetails = $("#HtmlContent").val();
    //    var SocietyId = $("#SocietyID").val();
    //    var applicationDetails = {
    //        ComplainID: $scope.ComplainUnderSection.ComplainID == undefined ? 0 : $scope.ComplainUnderSection.ComplainID,
    //        SocietyId: $scope.SocietyId,
    //        ComplantBy: $scope.ComplainUnderSection.ComplantBy == "2" ? true : false,
    //        //SSOID: $scope.SSOID,
    //        // UserID: $scope.UserID,
    //        IsComplentForSociety: $scope.ComplainUnderSection.IsComplentForSociety == "2" ? true : false,
    //        ComplaintDetails: $("#HtmlContent").val() != "" || $("#HtmlContent").val() != null ? $("#HtmlContent").val() : "",//$scope.ComplainUnderSection.ComplaintDetails,
    //        StatusId: $scope.ComplainUnderSection.StatusId,
    //        ComplainDate: ComplainDate,
    //        IsActive: $scope.ComplainUnderSection.IsActive,
    //        Scheme: $scope.ComplainUnderSection.Scheme,
    //        SocietyName: ($scope.SocietyInfo.ClassId == 6 ? "गृह निर्माण सहकारी समिति " : "") + $scope.SocietyInfo.SocietyName + " " + ($scope.SocietyInfo.ClassId == 6 ? $scope.ComplainUnderSection.Scheme : ""),
    //        //CityHi: $scope.SocietyInfo.CityHi,
    //        //CityEn: $scope.SocietyInfo.CityEn,
    //        //CityHi: $scope.UserReqData.filter(function (x) { return x.UserId.toString() === $scope.IODetail.UserId.toString() })[0].DISTRICT_ENG,
    //        //CityEn: $scope.UserReqData.filter(function (x) { return x.UserId.toString() === $scope.IODetail.UserId.toString() })[0].DISTRICT_MANGAL,
    //        CityType: $scope.CityType,
    //        ComplainantDetailsList: ComplainantDetailsList,
    //        ComplaintAgainstList: ComplaintAgainstList,
    //        IODetailList: IODetailList,
    //        ComplainTrailList: ComplainTrail,
    //        TXN: TransId
    //    }
    //    formData.append("Applicationdata", JSON.stringify(applicationDetails));
    //    formData.append("ComplainantDetailsList", JSON.stringify($scope.ComplainantDetailsList));
    //    formData.append("IODetailList", JSON.stringify($scope.IODetailList));
    //    formData.append("ComplainTrailList", JSON.stringify($scope.ComplainTrailList));

    //    $http.post(SiteUrl + "/SocietyComplaintCourtModule/SaveComplainUnderSection", JSON.stringify({ ApplicationModel: applicationDetails })).success(function (data, status) {
    //        $(".se-pre-con").fadeOut("slow");

    //        if (data.Result != "-1" && data.Result != "0") {
    //            ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () {
    //                window.location.href = "../SocietyComplaintCourtModule/SocietyComplaintCourtModule";
    //            });
    //        } else {
    //            ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें| ", function () {

    //            });
    //        }
    //    }).
    //        error(function (data, status) {
    //            $(".se-pre-con").fadeOut("slow");
    //            ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technical error. Please try after some time." : "कुछ तकनीकी त्रुटि है। कृपया कुछ देर बाद प्रयास करें| ", function () {
    //            });
    //        });

    //};

    $scope.UpdateUser = function (TransId) {
        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();


        var ComplainantDetailsList = [];
        $scope.ComplainantDetailsList.forEach(function (itm) {
            ComplainantDetailsList.push({
                CDetailID: itm.CDetailID == undefined ? 0 : itm.CDetailID,
                Name: itm.Name,
                MobileNo: itm.MobileNo,
                Address: itm.Address,
                Post: itm.Post,
                IsSocietyMember: itm.IsSocietyMember == "1" ? true : false,
                Ctype: true,
                IsActive: itm.IsActive,
                IsDelete: itm.IsDelete
            });
        });



        var ComplaintAgainstList = [];
        $scope.ComplaintAgainstList.forEach(function (itm) {
            ComplaintAgainstList.push({
                CDetailID: itm.CDetailID == undefined ? 0 : itm.CDetailID,
                Name: itm.Name,
                Address: itm.Address,
                Post: itm.Post,
                Ctype: false,
                IsActive: 1,
                IsDelete: itm.IsDelete
            });
        });



        var IODetailList = [];
        IODetailList.push({
            IODetailID: $scope.IODetail.IODetailID == undefined ? 0 : $scope.IODetail.IODetailID,
            UserId: $scope.IODetail.UserId,
            IsActive: 1,
            OfficerNameEn: $scope.UserReqData.filter(function (x) { return x.UserId.toString() === $scope.IODetail.UserId.toString() })[0].OfficerNameEn,
            OfficerNameHi: $scope.UserReqData.filter(function (x) { return x.UserId.toString() === $scope.IODetail.UserId.toString() })[0].OfficerNameHi,
            SSOID: $scope.UserReqData.filter(function (x) { return x.UserId.toString() === $scope.IODetail.UserId.toString() })[0].SSO_ID,
            Period: $scope.ComplainUnderSection.Period,
        });

        var ComplainTrail = [];
        ComplainTrail.push({
            ComplainTrailID: $scope.ComplainTrail.ComplainTrailID == undefined ? 0 : $scope.ComplainTrail.ComplainTrailID,
            // ComplainID: $scope.IODetail.ComplainID,  
            ComplaintStatusId: $scope.ComplainTrail.ComplaintStatusId,
            FileName: $scope.ComplainTrail.FileName,
            Remark: $scope.ComplainTrail.Remark,
            IsActive: $scope.ComplainTrail.IsActive
        });

        var ComplainDate = $("#ComplaintDate").val();
        var complainDetails = $("#HtmlContent").val();
        var SocietyId = $("#SocietyID").val();
        var applicationDetails = {
            ComplainID: $scope.ComplainUnderSection.ComplainID == undefined ? 0 : $scope.ComplainUnderSection.ComplainID,
            SocietyId: $scope.SocietyId,
            ComplantBy: $scope.ComplainUnderSection.ComplantBy == "2" ? true : false,
            //SSOID: $scope.SSOID,
            // UserID: $scope.UserID,
            IsComplentForSociety: $scope.ComplainUnderSection.IsComplentForSociety == "2" ? true : false,
            ComplaintDetails: $("#HtmlContent").val() != "" || $("#HtmlContent").val() != null ? $("#HtmlContent").val() : "",//$scope.ComplainUnderSection.ComplaintDetails,
            StatusId: $scope.ComplainUnderSection.Status,
            ComplainDate: ComplainDate,
            IsActive: $scope.ComplainUnderSection.IsActive,
            Scheme: $scope.ComplainUnderSection.Scheme,
            SocietyName: ($scope.SocietyInfo.ClassId == 6 ? "गृह निर्माण सहकारी समिति " : "") + $scope.SocietyInfo.SocietyName + " " + $scope.ComplainUnderSection.Scheme,
            //CityHi: $scope.SocietyInfo.CityHi,
            //CityEn: $scope.SocietyInfo.CityEn,
            //CityHi: $scope.UserReqData.filter(function (x) { return x.UserId.toString() === $scope.IODetail.UserId.toString() })[0].DISTRICT_ENG,
            //CityEn: $scope.UserReqData.filter(function (x) { return x.UserId.toString() === $scope.IODetail.UserId.toString() })[0].DISTRICT_MANGAL,
            CityType: $scope.CityType,
            ComplainantDetailsList: ComplainantDetailsList,
            ComplaintAgainstList: ComplaintAgainstList,
            IODetailList: IODetailList,
            ComplainTrailList: ComplainTrail,
            TXN: TransId,
            ComplaintDetailsForAcceptAndPublish: $("#HtmlContent2").val() != "" || $("#HtmlContent2").val() != null ? $("#HtmlContent2").val() : ""
        }
        formData.append("Applicationdata", JSON.stringify(applicationDetails));
        formData.append("ComplainantDetailsList", JSON.stringify($scope.ComplainantDetailsList));
        formData.append("IODetailList", JSON.stringify($scope.IODetailList));
        formData.append("ComplainTrailList", JSON.stringify($scope.ComplainTrailList));

        $http.post(SiteUrl + "/SocietyComplaintCourtModule/UpdateComplainUnderSection", JSON.stringify({ ApplicationModel: applicationDetails })).success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");

            if (data.Result != "-1" && data.Result != "0") {

                var statusMessage = $scope._language == 'en-us' ? " successfully updated.<br/></b>" : "सफलतापूर्वक अपडेट कर लिया गया है|<br/></b>";
                if ($scope.ComplainUnderSection.Status == "7") {
                    statusMessage = $scope._language == 'en-us' ? "Result published Sucessfully" : "Result published Sucessfully<br/></b>";
                }
                if ($scope.ComplainUnderSection.Status == "6") {
                    statusMessage = $scope._language == 'en-us' ? "Report Accepted Sucessfully" : "Report Accepted Sucessfully<br/></b>";
                }
                if ($scope.ComplainUnderSection.Status == "4") {
                    statusMessage = $scope._language == 'en-us' ? "Report reopened Sucessfully" : "Report reopened Sucessfully<br/></b>";
                }
                if ($scope.ComplainUnderSection.Status == "5") {
                    statusMessage = $scope._language == 'en-us' ? "Report reopened and change IO Sucessfully" : "Report reopened and change IO Sucessfully<br/></b>";
                }
                if ($scope.ComplainUnderSection.Status == "8") {
                    statusMessage = $scope._language == 'en-us' ? "Report rejected and closed Sucessfully" : "Report rejected and closed Sucessfully<br/></b>";
                }
                ShowOkMessage("Success", statusMessage , function () {
                    window.location.href = "../SocietyComplaintCourtModule/SocietyComplaintCourtModule";
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

    $scope.UpdateComplaintAgainstSociety = function () {
        if ($scope.validateUpdateComplainUnderSection()) {
            if ($scope.ValidateComplainantDetails()) {
                if ($scope.ValidateComplaintAgainst()) {
                    //      $("#addModal").modal("hide");

                    if ($scope.ComplainUnderSection.Status == "5" || $scope.ComplainUnderSection.Status == "4" || $scope.ComplainUnderSection.Status == "7") {
                        AadhaarAuth("", $http, "OTP", true, function (data) {
                            $("#addModal").modal("show");
                            $scope.UpdateUser(data.TransId);
                        });
                    } else {
                        $scope.UpdateUser(0);
                    }
                }
            }
        }
    }

    $scope.SaveComplaintAgainstSociety = function () {
        if ($scope.validateComplainUnderSection()) {
            if ($scope.ValidateComplainantDetails()) {
                if ($scope.ValidateComplaintAgainst()) {
                    if (document.getElementById('idotherperson').checked == true) {
                        if ($scope.ValidatePersonList()) {
                            debugger;
                            $("#addModal").modal("hide");
                            AadhaarAuth("", $http, "OTP", true, function (data) {
                                $("#addModal").modal("show");
                                debugger;
                                $scope.SaveUser(data.TransId);

                          //  $scope.SaveUser();

                            });
                        }
                    }
                    else {
                        debugger;
                        $("#addModal").modal("hide");
                        debugger;
                            AadhaarAuth("", $http, "OTP", true, function (data) {
                                $("#addModal").modal("show");
                                $scope.SaveUser(data.TransId);
                        
                           // $scope.SaveUser();

                            });
                        
                    }

                    
                    
                }
            }
        }
    }

    $scope.FilterComplainStatus = [];
    $scope.ComplainStatusList = [];
    $scope.GetComplainStatus = function () {
        return $http.get(SiteUrl + "/SocietyComplaintCourtModule/GetComplainStatus/?ActionId=1").
            success(function (data, status) {
                $scope.FilterComplainStatus = JSON.parse(data.DataList);
            }).
            error(function (data, status) {
                $scope.FilterComplainStatus = [];

            });
    }
    $scope.ComplainTrailList = [];
    $scope.GetComplainTrail = function (ComplainID) {

        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/SocietyComplaintCourtModule/GetComplainTrail/?ComplainID=" + ComplainID).
            success(function (data, status) {
                $scope.ComplainTrailList = JSON.parse(data.DataList);
                $scope.ComplainTrailList.forEach(function (itm, index) {
                    if (itm.ComplaintStatusId == 3) {
                        itm.drFiles = itm.FileName.split('#');
                    }
                });
                $(".se-pre-con").fadeOut("slow");
                $("#mdlTrailList").modal("show");

            }).
            error(function (data, status) {
                $scope.ComplainTrailList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    }

    $scope.GetComplainStatus1 = function (ActionId) {
        return $http.get(SiteUrl + "/SocietyComplaintCourtModule/GetComplainStatus/?ActionId=" + ActionId).
            success(function (data, status) {
                $scope.ComplainStatusList = JSON.parse(data.DataList);
            }).
            error(function (data, status) {
                $scope.ComplainStatusList = [];

            });
    }
    $scope.GetComplainStatus();
    $scope.GetComplainStatus1()

    //------------------End Save User -----------------------------//








    //------------------Start Save IO Details-----------------------------//

    $scope.IODetails = [];
    $scope.IODetails.push();
    $scope.validateIODetails = function () {

        var message = "";
        $("#tblLoanMembersDetail").find('input[type="text"]').removeClass("notValid");
        $("#tblLoanMembersDetail").find('select').removeClass("notValid");


        if ($("#HtmlContent").val() == '') {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Period' : 'कृपया शिकायतकर्ता का विस्तृत विवरण दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया शिकायतकर्ता का विस्तृत विवरण दर्ज करें|.' : '<br />कृपया शिकायतकर्ता का विस्तृत विवरण दर्ज करें|';
            }
            $("#HtmlContent").addClass("notValid");
        } else {
            $("#HtmlContent").removeClass("notValid");
        }


        if ($("#ComplaintDate").val() == '') {
            if (message.indexOf($scope._language == 'en-us' ? 'Please enter business license registration Description.' : 'कृपया शिकायत की तारीख दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया शिकायत की तारीख दर्ज करें।' : '<br />कृपया शिकायत की तारीख दर्ज करें।';
            }
            $("#ComplaintDate").addClass("notValid");
        }
        else {
            $("#ComplaintDate").removeClass("notValid");
        }

        if ($scope.ComplainTrail.Places == '' || $scope.ComplainTrail.Places == undefined) {
            if (message.indexOf($scope._language == 'en-us' ? 'Please Select Address Type' : 'कृपया स्थान दर्ज करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />कृपया स्थान दर्ज  करें।.' : '<br />कृपया स्थान दर्ज  करें।.';
            }
        }


        if (message.length > 29) {
            ShowOkMessage("निम्नलिखित मान्य करें", message, function () { });
            return false;
        }
        return true;
    }





    $scope.SaveIODetails = function (TransId) {
        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();

        var IODetailList = [];
        IODetailList.push({
            IODetailID: $scope.IODetail.IODetailID == undefined ? 0 : $scope.IODetail.IODetailID,
            UserId: $scope.IODetail.UserId,
            IsActive: $scope.IODetail.IsActive,

            IODetails: $("#HtmlContent").val() != "" || $("#HtmlContent").val() != null ? $("#HtmlContent").val() : "",
        });

        var ComplainTrail = [];
        ComplainTrail.push({
            ComplainTrailID: $scope.ComplainTrail.ComplainTrailID == undefined ? 0 : $scope.ComplainTrail.ComplainTrailID,
            // ComplainID: $scope.IODetail.ComplainID,  
            ComplaintStatusId: $scope.StatusId,
            FileName: $scope.ComplainTrail.FileName,
            Remark: $scope.ComplainTrail.Remark,
            IsActive: $scope.ComplainTrail.IsActive
        });

        var ComplainDate = $("#ComplaintDate").val();
        var complainDetails = $("#HtmlContent").val();
        var SocietyId = $("#SocietyID").val();
        var applicationDetails = {

            ComplainID: $scope.Complain_ID,
            IODetailList: IODetailList,
            ComplainTrailList: ComplainTrail,
            TXN: TransId

        }
        formData.append("Applicationdata", JSON.stringify(applicationDetails));
        formData.append("ComplainantDetailsList", JSON.stringify($scope.ComplainantDetailsList));
        formData.append("IODetailList", JSON.stringify($scope.IODetailList));
        formData.append("ComplainTrailList", JSON.stringify($scope.ComplainTrailList));

        $http.post(SiteUrl + "/SocietyComplaintCourtModule/SaveIODetails", JSON.stringify({ ApplicationModel: applicationDetails })).success(function (data, status) {
            $(".se-pre-con").fadeOut("slow");


            //$http.post(SiteUrl + "/SocietyComplaintCourtModule/SaveComplainUnderSection", formData,
            //    {
            //        withCredentials: true,
            //        headers: { 'Content-Type': undefined },
            //        transformRequest: angular.identity
            //    }).success(function (data, status) {
            //        $(".se-pre-con").fadeOut("slow");
            if (data.Result != "-1") {


                //ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully IO submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () {
                ShowOkMessage("Success", $scope._language == 'en-us' ? "IO successfully submitted. <br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () {
                    window.location.href = "../SocietyComplaintCourtModule/SocietyComplaintAssignIO";
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

    $scope.SaveIODetailsUser = function () {
        if ($scope.validateIODetails()) {
            $scope.StatusId = "0",
                $("#addModal").modal("hide");
            AadhaarAuth("", $http, "OTP", true, function (data) {
                $("#addModal").modal("show");
                $scope.SaveIODetails(data.TransId);
            });
        }
    }

    $scope.SaveIODetailsTransferDR = function () {
        $scope.StatusId = "3",
            $scope.SaveIODetails();
    }







    //------------------End Save IO Details -----------------------------//














    $scope.GetAllSubUnitList = function (UserId) {
        //DesignationLevelMappingId
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/SocietyComplaintCourtModule/GetinveticationListUser/?DesignationLevelMappingId=" + $scope.UserRegistration.DesignationLevelMappingId + "&UserId=" + (UserId == undefined ? 0 : UserId)).
            success(function (data, status) {
                $scope.UserReqData = data;
                $(".se-pre-con").fadeOut("slow");

                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };


                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                    }
                    return '';
                };
            }).
            error(function (data1, status) {

            });
    }
    $scope.RequestType = 1;
    $scope.AddNewCompaint = function (type) {
        $("#searchText").val("");
        $("#CaseStatusId").val("");
        // $scope.RequestType = type;
        $scope.GetApprovedSocietyList();
    }

    $scope.RegisteredComplaint = function (type) {
        $("#searchText").val("");
        $("#CaseStatusId").val("");
        // $scope.RequestType = type;
        $scope.GetApprovedSocietyList();
    }

    $scope.GetApprovedSocietyList = function () {
        $scope.RequestType=2
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/SocietyComplaintCourtModule/GetApprovedSocietyList", JSON.stringify({ Details: jsonParameters, RequestType: $scope.RequestType })).
            //return $http.post(SiteUrl + "/SocietyComplaintCourtModule/GetApprovedSocietyList").
            success(function (data, status) {
                $scope.GetApprovedSocietyACT2001List = [];
                //$scope.GetApprovedSocietyACT2001List = data;
                $scope.GetApprovedSocietyACT2001List = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();

                if ($scope.RequestType==2) {
                    $scope.GetApprovedSocietyACT2001List.forEach(function (itm, index) {
                        if (itm.IOReport != '') {
                            itm.drFiles = itm.IOReport.split('#');
                        }
                    });
                }

                $(".blockUI").fadeOut("slow");
                //$scope.TotalRecord = $scope.LoanSchemeList.length;

                //$scope.colsort = "";
                //$scope.reverseSort = false;

                //$scope.sortData = function (column) {
                //    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                //    $scope.colsort = column;
                //};

                //$scope.getSortClass = function (column) {
                //    if ($scope.colsort == column) {
                //        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                //    }
                //    return '';
                //};
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                alert("Error while fetching Society Details.");

            });
    }

    $scope.IsViewComplaint = false;
    $scope.ComplainUndeVlaue = { AnyOtherMatter: false, AnyOtherMatterText: "", AnyOtherPerson: false };
    $scope.SocietyInfo = {};
    $scope.GetComplainDetails = function (ComplainID) {

        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/SocietyComplaintCourtModule/GetComplainDetails?ComplainID=" + ComplainID).
            success(function (data, status) {
                $scope.DataList = JSON.parse(data.DataList);
                $scope.showPriView = true;
                $scope.IsViewComplaint = true;;

                $scope.GetAllSocietydetails = $scope.DataList.Table;
                $("#tbleSocietyList").find("input[type='radio']").first().prop("checked", true);
                $scope.ComplainUnderSection.Formation = '';
                $scope.checkstatus = false;
                $("#ComplaintDate").val("");
                $("#ComplaintDate1").val("");
                $("#HtmlContent6").summernote("code", "");

                $scope.ComplainUnderSection.Business = '';
                $scope.checkstatus1 = false;
                $("#ComplaintDate2").val("");
                $("#ComplaintDate3").val("");
                $("#HtmlContent5").summernote("code", "");

                $scope.ComplainUnderSection.Financial = '';
                $scope.checkstatus2 = false;
                $("#ComplaintDate4").val("");
                $("#ComplaintDate5").val("");
                $("#HtmlContent4").summernote("code", "");
                $scope.SocietyInfo = $scope.DataList.Table;
                $scope.DataList.Table.forEach(function (item, index) {
                    $scope.ComplainUnderSection.ComplainID = item.ComplainID;
                    $scope.ComplainUnderSection.ComplantBy = !item.ComplantBy ? "1" : "2";
                    $scope.ComplainUnderSection.IsComplentForSociety = !item.IsComplentForSociety ? "1" : "2";
                    $scope.ComplainUnderSection.StatusId = item.StatusId;
                   // $scope.ComplainUnderSection.Status = item.StatusId;



                    if (item.Investigation1) {
                        $scope.ComplainUnderSection.Formation = '1';
                        $scope.checkstatus = true;
                        $("#ComplaintDate").val(item.Investigation1dateFormated);
                        $("#ComplaintDate1").val(item.Investigation1todateFormated);
                        $("#HtmlContent6").summernote("code", item.Investigation1Detilas);
                    }

                    if (item.Investigation2) {
                        $scope.ComplainUnderSection.Business = '2';
                        $scope.checkstatus1 = true;
                        $("#ComplaintDate2").val(item.Investigation2dateFormated);
                        $("#ComplaintDate3").val(item.Investigation2todateFormated);
                        $("#HtmlContent5").summernote("code", item.Investigation2Detilas);
                    }

                    if (item.Investigation3) {
                        $scope.ComplainUnderSection.Financial = '3';
                        $scope.checkstatus2 = true;
                        $("#ComplaintDate4").val(item.Investigation3dateFormated);
                        $("#ComplaintDate5").val(item.Investigation3todateFormated);
                        $("#HtmlContent4").summernote("code", item.Investigation3Detilas);
                    }


                    if (item.OtherDescription != null && item.OtherDescription != '') {
                        $scope.ComplainUndeVlaue.AnyOtherMatter = true;
                        $scope.ComplainUndeVlaue.AnyOtherMatterText = item.OtherDescription;
                    }
                    $("#ComplaintDate").val(item.ComplainDateFormated);
                    $scope.ComplainUnderSection.ComplainDate = item.ComplainDateFormated;
                    $("#SocietyID").val(item.SocietyId);
                    $scope.SocietyId = item.SocietyId;
                    // $("#HtmlContent").val(item.ComplaintDetails);
                    $scope.IODetail.UserId = item.IOUserId.toString();
                    $scope.ComplainTrail.Remark = item.Remarks;
                    $scope.ComplainTrail.IsUploadFileName = item.FileName == "" ? false : true;
                    $scope.ComplainTrail.FileName = item.FileName;
                    $("#HtmlContent2").summernote("code", item.ComplaintDetails);
                    $scope.ComplainUnderSection.ComplaintDetails = item.ComplaintDetails;
                    $("#divComplaintDetails").html(item.ComplaintDetails);
                    $scope.ComplainUnderSection.Scheme = item.YognaName;
                    $scope.ComplainUnderSection.Period = item.Period;

                    $scope.GetAllSubUnitList($scope.IODetail.UserId);

                    if (item.EncloserFileName != null && item.EncloserFileName != '') {
                        item.EncloserFileName.split(',').forEach(function (itm, indz) {
                            $scope.FileCollection.push({ 'Id': 'flRemovalType_' + indz, 'FileType': '', 'File': '', 'Title': '', FileName: itm, FileId: 'flRemovalType_' + indz, IsFileUpload: true });
                            $scope.EncloserFilesList.push({
                                Id: 'flRemovalType_' + indz,
                                FileType: '',
                                FileName: itm,
                                IsUploadFileName: true,
                                Is_Active: true,
                                Is_Delete: false
                            });
                        });                        
                    }

                    $scope.GetComplainStatus1(item.StatusId);


                    //$("#ComplaintForm").find("input").attr("disabled", "disabled");
                    //$("#ComplaintForm").find("table").find("input").attr("disabled", "disabled");
                    //$("#ComplaintForm").find("select").attr("disabled", "disabled");
                    //$("#ComplaintForm").find("textarea").attr("disabled", "disabled");
                    //$("#ComplaintForm").find("tfoot").hide();
                    //$("#ComplaintForm").find("i").hide();
                    //$("#ComplaintForm").eq(0).show();
                    //$("#ComplaintForm").eq(1).show();
                    //$("#ComplaintForm").show();
                    //$("#ComplaintForm").eq(0).removeClass("fa-plus-circle").addClass("fa-minus-circle");
                    //$("#ComplaintForm").eq(1).removeClass("fa-plus-circle").addClass("fa-minus-circle");


                });
                $scope.OtherPersonList.push({ "PersonId": 0, "Name": "", "Address": "", "Post": "" });
                if ($scope.DataList.Table2.length > 0) {
                    $scope.ComplainUndeVlaue.AnyOtherPerson = true;
                    $scope.OtherPersonList = [];
                    $scope.DataList.Table2.forEach(function (itm, indx) {
                        $scope.OtherPersonList.push({ "PersonId": itm.PersonId, "Name": itm.Name, "Address": itm.Address, "Post": itm.Post });
                    });
                }

                if ($scope.DataList.Table[0].ComplantBy) {
                    $scope.ComplainantDetailsList = [];
                    if ($scope.DataList.Table1.filter(function (x) { return x.Ctype === true }).length > 0) {
                        $scope.DataList.Table1.filter(function (x) { return x.Ctype === true }).forEach(function (item, index) {
                            $scope.ComplainantDetailsList.push({
                                Name: item.Name,
                                Address: item.Address,
                                MobileNo: item.MobileNo,
                                IsSocietyMember: item.IsSocietyMember,
                                CDetailID: item.CDetailID,
                                Post: "",
                                Ctype: "",
                                IsActive: "",
                                IsDelete: false
                            });
                        });
                    }
                }

                if ($scope.DataList.Table[0].IsComplentForSociety) {
                    $scope.ComplaintAgainstList = [];
                    if ($scope.DataList.Table1.filter(function (x) { return x.Ctype === false }).length > 0) {
                        $scope.DataList.Table1.filter(function (x) { return x.Ctype === false }).forEach(function (item, index) {
                            $scope.ComplaintAgainstList.push({
                                Name: item.Name,
                                Address: item.Address,
                                MobileNo: item.MobileNo,
                                IsSocietyMember: item.IsSocietyMember,
                                CDetailID: item.CDetailID,
                                Post: item.Post,
                                Ctype: "",
                                IsActive: "",
                                IsDelete: false
                            });
                        });
                    }
                }

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data1, status) {

            });
    }

    $scope.ResetAddNew = function () {
        $scope.ComplainUnderSection.ComplainID = 0;
        $scope.ComplainUnderSection.ComplantBy = "1";
        $scope.ComplainUnderSection.IsComplentForSociety = "1";
        //$scope.ComplainUnderSection.StatusId;
        $("#ComplaintDate").val("");
        $scope.ComplainUnderSection.ComplainDate = "";
        $("#SocietyID").val("");
        $scope.SocietyId = "";
        // $("#HtmlContent").val(item.ComplaintDetails);
        $scope.IODetail.UserId = "0";
        $scope.ComplainTrail.Remark = "";
        $scope.ComplainTrail.IsUploadFileName = false
        $scope.ComplainTrail.FileName = "";
        $("#HtmlContent2").summernote("code", "");
        $scope.ComplainantDetailsList = [];
        $scope.ComplainantDetailsList.push($scope.ComplainantDetails);
        $scope.ComplaintAgainstList = [];
        $scope.ComplaintAgainstList.push($scope.ComplaintAgainst);
        $scope.showPriView = false;
        $scope.IsViewComplaint = false;;
    }

    $scope.GetApprovedSocietyAssignForIO = function () {
        return $http.post(SiteUrl + "/SocietyComplaintCourtModule/GetApprovedSocietyAssignIOList").
            success(function (data, status) {
                $scope.GetApprovedSocietyAssignForIOList = [];
                // $scope.GetApprovedSocietyAssignForIOList = data;
                //$scope.TotalRecord = $scope.LoanSchemeList.length;








                if (data.Table.length > 0) {
                    $scope.GetApprovedSocietyAssignForIOList = [];
                    $scope.GetApprovedSocietyAssignForIOList = data.Table;
                }








                //if (dataList.Table3.length > 0) {
                //    $scope.PropertyDetailsList = [];
                //    if (dataList.Table3.filter(function (x) { return x.MemberType === 1 }).length > 0) {
                //        dataList.Table3.filter(function (x) { return x.MemberType === 1 }).forEach(function (itm, index) {
                //            $scope.PropertyDetailsList.push({
                //                Id: itm.loan_property_id,
                //                Khasara: itm.khasara,
                //                Address: itm.Address,
                //                TotalArea: itm.TotalArea,
                //                OtherDetails: itm.other_property_details,
                //                Specifications: itm.specifications,
                //                IsDelete: false
                //            });
                //        });
                //    } else {
                //        $scope.PropertyDetailsList.push($scope.PropertyDetails);
                //    }

                //    $scope.GuarantorPropertyDetailsList = [];
                //    if (dataList.Table3.filter(function (x) { return x.MemberType === 2 }).length > 0) {
                //        dataList.Table3.filter(function (x) { return x.MemberType === 2 }).forEach(function (itm, index) {

                //            $scope.GuarantorPropertyDetailsList.push({
                //                Id: itm.loan_property_id,
                //                Khasara: itm.khasara,
                //                Address: itm.Address,
                //                TotalArea: itm.TotalArea,
                //                OtherDetails: itm.other_property_details,
                //                Specifications: itm.specifications,
                //                IsDelete: false,
                //                GPropertyDocument: itm.PropertyDocument,
                //                PropertyDocumentType: $scope.PropertyDocumentType.filter(function (x) { return x.PropertyDocumentType === itm.PropertyDocumentType }).length > 0 ? itm.PropertyDocumentType : "OtherType",
                //                PropertyDocumentTypeList: $scope.PropertyDocumentType,
                //                PropertyDocumentTypeOther: $scope.PropertyDocumentType.filter(function (x) { return x.PropertyDocumentType === itm.PropertyDocumentType }).length > 0 ? "" : itm.PropertyDocumentType,
                //                IsUploadGPropertyDocument: itm.PropertyDocument != "" && itm.PropertyDocument != null
                //            });
                //        });
                //    } else {
                //        $scope.GuarantorPropertyDetailsList.push($scope.GuarantorPropertyDetails);
                //    }
                //}





                $scope.colsort = "";
                $scope.reverseSort = false;

                $scope.sortData = function (column) {
                    $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
                    $scope.colsort = column;
                };

                $scope.getSortClass = function (column) {
                    if ($scope.colsort == column) {
                        return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
                    }
                    return '';
                };
            }).
            error(function (data, status) {
                alert("Error while fetching Society Details.");

            });
    }


    $scope.SocietyChangeModel1 = [];
    $scope.GetAllSocietydetails = [];

    $scope.societydiv = false;

    $scope.SocietyChangeModel = {
        SocietyCategoryId: '0',
        SubClassId: '0',
        ClassId: '0',
        SocietyTypeId: '0',
        DISTRICT_ID: '0',
        SearchText: ''

    }

    $scope.ClearSearch_new = function () {

        $scope.SocietyChangeModel = {
            SocietyCategoryId: '0',
            SubClassId: '0',
            ClassId: '0',
            SocietyTypeId: '0',
            DISTRICT_ID: '0',
            SearchText: ''

        }
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.getSocietydetails($scope.SocietyChangeModel);
    }

    //bhupendra
    $scope.getSocietydetails = function (SocietyChangeModel) {
        $scope.SocietyChangeModel1 = SocietyChangeModel;
        debugger
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/SocietyComplaintCourtModule/GetSocietydetailsData?Districtid", JSON.stringify({ Details: jsonParameters, Districtid: $scope.SocietyChangeModel1.DISTRICT_ID, SocietyTypeid: $scope.SocietyChangeModel1.SocietyTypeId, Classid: $scope.SocietyChangeModel1.ClassId, Subclassid: $scope.SocietyChangeModel1.SubClassId, Searchbytext: $scope.SocietyChangeModel1.SearchText })).
            success(function (data, status) {
                debugger;
                $scope.societydiv = true;
                $scope.GetAllSocietydetails = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $scope.colsort = "";
                $scope.reverseSort = false;
                if (data.length > 0) {
                    if (data[0].AfterMay > 0)
                        $scope.AfterMay = false;
                    else
                        $scope.AfterMay = true;
                }

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.GetAllAuditorAssignmentByDepartmentdata = [];
                alert('Error while fetching Auditor data.');
            });
    }

    //bhupendra
    //$scope.getSocietydetails = function (SocietyChangeModel) {
    //    $scope.SocietyChangeModel1 = SocietyChangeModel;
    //    debugger
    //    $(".se-pre-con").fadeIn("slow");
    //    $http.post(SiteUrl + "/SocietyComplaintCourtModule/GetSocietydetailsData?Districtid=" + $scope.SocietyChangeModel1.DISTRICT_ID + "&SocietyTypeid=" + $scope.SocietyChangeModel1.SocietyTypeId + "&Classid=" + $scope.SocietyChangeModel1.ClassId + "&Subclassid=" + $scope.SocietyChangeModel1.SubClassId).
    //        success(function (data, status) {
    //            debugger;
    //            $scope.societydiv = true;
    //            $scope.GetAllSocietydetails= data;
    //            $scope.colsort = "";
    //            $scope.reverseSort = false;
    //            if (data.length > 0) {
    //                if (data[0].AfterMay > 0)
    //                    $scope.AfterMay = false;
    //                else
    //                    $scope.AfterMay = true;
    //            }

    //            $(".se-pre-con").fadeOut("slow");
    //        }).
    //        error(function (data1, status) {
    //            $(".se-pre-con").fadeOut("slow");
    //            $scope.GetAllAuditorAssignmentByDepartmentdata = [];
    //            alert('Error while fetching Auditor data.');
    //        });
    //}





    $scope.LetterData = {
        ComplaintId: "",
        Date: "",
        Time: "",
        Place: "",
        CEOName: "",
        CEOMobile: "",
        complainant_letter: "",
        ceo_letter: "",
        ComplainerName: "",
        MobileNo: "",
        SocietyName: "",
        ReceiptNo: ""
    }
    $scope.GetFilterbyAction = function () {


    }


    $scope.FileType = "";

    $scope.AddFile1 = function (type) {
        debugger
        $scope.FileType = type;
        $scope.ComplainTrail.IsUploadFileName = true;
    }
    $scope.ChangeFile1 = function (type) {
        debugger
        if (type == 1) {
            $scope.ComplainTrail.IsUploadFileName = false;
            $scope.ComplainTrail.FileName = "";
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
                $scope.xhr1(SiteUrl + "/SocietyComplaintCourtModule/UploadComplainFile", formData, type);
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
                            $scope.ComplainTrail.FileName = request.responseText;
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


    $scope.OnPageLoad();
    
    $scope.EncloserFilesList.push({
        Id: '',
        FileType: '',
        FileName: '',
        IsUploadFileName: false,
        Is_Active: true,
        Is_Delete: false
    });
   

    $scope.FileType = "";

    $scope.AddFile = function (type, index) {
       
        $scope.FileType = type;
        var ccc = index;
        $scope.FileTypeIndex = index;

    }
    $scope.UploadFile = function (id, type, model,index) {
        try {
          
            if (window.FormData !== undefined) {
                var fileUpload = $("#" + id).get(0);
                var files = fileUpload.files;
                //var blob = b64toBlob(realData, contentType, 512);
                var formData = new FormData();
                formData.append("pldbfile", files[0]);
                $(".blockUI").show();
                $scope.xhr(SiteUrl + "/SocietyComplaintCourtModule/UploadComplainFile", formData, type, model, index);
            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }

    $scope.xhr = function (url, data, type, model,index) {
      
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
                       
                        $scope.FileCollection.forEach(function (itm, indx) {
                           
                            if (itm.FileId == model) {
                                itm.FileName = request.responseText;
                                itm.File = request.responseText;
                                $("#" + model).parent().parent().css('border', 'none');
                                $scope.EncloserFilesList[index].FileName = request.responseText;
                                $scope.EncloserFilesList[index].IsUploadFileName = itm.IsFileUpload;
                                $scope.EncloserFilesList[index].Id = itm.FileId;
                                $scope.EncloserFilesList[index].FileType = type;
                              
                            }
                        });
                       
                        if ($scope.changefilestatus != model) {
                            $scope.EncloserFilesList.push({
                                Id: '',
                                FileType: '',
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

    $scope.ComplainTrail = {
        IsUploadFileName: false,
        FileName: "",

    }

    $scope.filesModal = { 'Id': 0, 'FileType': '', 'File': '', 'Title': '', FileName: "", FileId: "", IsFileUpload: false }
    $scope.FileCollection = [];

    $scope.IsFileUpload = function (id) {
        
        return $scope.FileCollection.filter(function (x) { return x.FileId === id && x.IsFileUpload === true }).length > 0;
    }
    $scope.GetFileUploaded = function (id) {
        
        try {
            return $scope.FileCollection.filter(function (x) { return x.FileId === id && x.IsFileUpload === true })[0].FileName;
        } catch (ex) {
            return "";
        }
    }


    $scope.ChangeFile = function (id,ind) {
       
       $scope.EncloserFilesList[ind].FileName = '';
        $scope.EncloserFilesList[ind].IsUploadFileName = false;
        $scope.changefilestatus = id;
        var FileCollection = $scope.FileCollection;
        $scope.FileCollection = [];
        FileCollection.forEach(function (item, index) {
            if (item.FileId != id) {
                $scope.FileCollection.push(item);
            }
        });
        $("#" + id).val("");
        $("#" + id).parent().parent().css('border', 'none');
    }



    //**Society Type* and Society Class* and Society Sub Class* Society Category**//
    // ******Partial Code-------Start------*****
    $scope.SocietyChangeModel = {};
    $scope.GetDistrictList = function () {
        debugger;
        $scope.IsRao = 0;
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.IsRao).
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };
    //start Level of office data
    //$scope.Levelofficedata = function (Cflag) {

     
    //    $scope.GetSocietyClass();
       
         
      
     
    //};
    //End Level of office data

    $scope.GetSocietyTypeList = function () {
        debugger;
        $scope.SocietyChangeModel.ActId = 0;
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=" + $scope.SocietyChangeModel.ActId).
            success(function (data, status) {
                $scope.SocietyTypeList = data;
            }).
            error(function (data, status) {
                $scope.SocietyTypeList = [];
            });
    };

    // GET Society Class
    $scope.GetSocietyClass = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYCLASS&MasterCode=" + $scope.SocietyChangeModel.SocietyTypeId).
            success(function (data, status) {
                $scope.SocietyClassList = data;
            }).
            error(function (data, status) {
                $scope.SocietyClassList = [];
            });

    }
    //END

    // START : Get SocietySubClassList
    $scope.GetSubClass = function () {

        $http.get(SiteUrl + "/Master/SubClassList/?ClassId=" + $scope.SocietyChangeModel.ClassId + "&SocietyTypeId=" + $scope.SocietyChangeModel.SocietyTypeId).
            success(function (data, status) {
                $scope.SocietySubClassList = data;
            }).
            error(function (data, status) {
                $scope.SocietySubClassList = [];
            });
        $scope.SocietyChangeModel.SocietyCategoryId = '0';
    };
    // END

    // START : Get SocietyCategoryList
    $scope.GetSocietyCategory = function () {

        $http.get(SiteUrl + "/Master/SocietyCategoryList/?ClassId=" + $scope.SocietyChangeModel.ClassId + "&SubClassId=" + $scope.SocietyChangeModel.SubClassId).
            success(function (data, status) {
                $scope.SocietyCategoryList = data;
            }).
            error(function (data, status) {
                $scope.SocietyCategoryList = [];
            });
    };
    // END : Get Sub Class
    $scope.GetDistrictList();
    $scope.GetSocietyTypeList();

     // ******Partial Code-------End------*****

    $scope.OtherPersonList = [];

    $scope.OtherPersonList.push({ "PersonId":0, "Name": "", "Address": "", "Post": "" });


    $scope.AddOtherPersonList = function () {
       
        if ($scope.ValidatePersonList()) {
            $scope.OtherPersonList.push({ "PersonId": 0, "Name": "", "Address": "", "Post": "" });
        }
       
    }

    $scope.RemoveOtherPersonListRow = function (index) {
        $scope.OtherPersonList.splice(index, 1);
        $scope.$apply();
    }
    

    $scope.ValidatePersonList = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        $scope.OtherPersonList.forEach(function (item, index) {

            if (!item.Name) {
                if (message.indexOf($scope._language == 'en-us' ? 'Please enter Name.' : 'कृपया उपयोगकर्ता का नाम दर्ज करें।') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please enter Name.' : '<br />कृपया उपयोगकर्ता का नाम दर्ज करें।';
                }
                $("#txtAccountName_" + index).addClass("notValid");
            }
            else {
                $("#txtAccountName_" + index).removeClass("notValid");
            }
            if (item.Address == "") {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Address' : 'कृपया उपयोगकर्ता का पता दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Address.' : '<br />कृपया उपयोगकर्ता का पता दर्ज करें।';
                }
                $("#txtAccountAddress_" + index).addClass("notValid");
            }
            else {
                $("#txtAccountAddress_" + index).removeClass("notValid");
            }
            if (item.Post == "") {
                if (message.indexOf($scope._language == 'en-us' ? 'Please Enter Post' : 'कृपया उपयोगकर्ता का पद दर्ज करें') < 0) {
                    message += $scope._language == 'en-us' ? '<br />Please Enter Post' : '<br />कृपया उपयोगकर्ता का पद दर्ज करें।';
                }
                $("#txtAccountPostHeading_" + index).addClass("notValid");
            }
            else {
                $("#txtAccountPostHeading_" + index).removeClass("notValid");
            }

        });
        if (message.length > 29) {
            ShowOkMessage("Society Complaint ", message, function () { });
            return false;
        }
        return true;
    }
    
});


AppClasses.directive('validfile', function validFile() {
    var validFormats = ['jpg', 'gif'];
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$validators.validFile = function () {
                elem.on('change', function () {
                    if (elem[0].files.length > 0) {
                        const fsize = elem[0].files[0].size;
                        const file = Math.round((fsize / 1024));
                        var id = elem.attr("id");

                        //var DDLIdLocal = $("#" + id).attr("DDLID");
                        //var DDLIdVal = $("#" + DDLIdLocal).val();

                        //if (!$scope.checkFileLengthPage(file[0].size)) {                       
                        //    alert(_language === 'en-us' ? "File size exceeded.\n Please select files have size less then 2 MB or Equivalent" : "फ़ाइल का आकार से अधिक हो गई है|\nकृपया चुनिंदा फ़ाइलों का आकार 2 MB से कम करें|");
                        //    $(row).val('');
                        //    $(row).parent().find('.filesize').text('');
                        //    return;
                        //} 


                        //var rowDataSplitter = DDLIdLocal.split('_');

                        // The size of the file. 
                        if (file >= 2048) {
                            ShowOkMessage("Warning", scope == 'en-us' ? "File too Big, please select a file less than 2mb" : "फ़ाइल बहुत बड़ी है, कृपया 2mb से कम फ़ाइल का चयन करें", function () { });
                            elem[0].value = "";
                        } else {

                            if (scope.FileCollection.filter(function (x) { return x.FileId === id }).length <= 0) {
                                scope.FileCollection.push({ 'Id': id, 'FileType': '', 'File': '', 'Title': '', FileName: "", FileId: id, IsFileUpload: true });
                            } else {
                                scope.FileCollection.forEach(function (itm, indx) {
                                    if (itm.FileId == id) {
                                        itm.IsFileUpload = true;
                                    }
                                });
                            }

                            //scope.AuditorAssign["IsUpload" + id + ""] = true;
                            //scope.AuditorAssign["" + id + ""] = "";

                            scope.$apply();
                        }
                    }

                });
            };
        }
    };
});

AppClasses.directive('validfile1', function validFile() {
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
                                scope.ComplainTrail.IsUploadFileName = true;
                                scope.ComplainTrail.FileName = "";
                            }
                            scope.$apply();
                        }
                    }

                });
            };
        }
    };



});