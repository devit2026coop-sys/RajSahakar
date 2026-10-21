var ComplaintList = angular.module("ComplaintListModuleMod", ['angularUtils.directives.dirPagination']);
ComplaintList.controller("ComplaintListModuleCnt", function ($scope, $http, $rootScope) {



    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 20,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
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
        $scope.GetSocietyComplaintList();
    }

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

    $scope.SocietyComplaintList = [];

    $scope.GetSocietyComplaintList = function () {
        
        //$http.post(SiteUrl + "/Citizen/GetSocietyComplaintList").
        $scope.RequestType = 3
        $(".blockUI").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        return $http.post(SiteUrl + "/Citizen/GetSocietyComplaintList", JSON.stringify({ Details: jsonParameters, RequestType: $scope.RequestType })).
            success(function (data, status) {
                debugger;
               

                $scope.SocietyComplaintList = JSON.parse(data.DataList);
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
             
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
                $scope.SocietyComplaintList = [];
                alert('Error while fetching Auditor data.');
            });
    }
    
    $scope.GetSocietyComplaintList();
    $scope.ComplainantDetailsList = [];
    $scope.ComplaintAgainstList = [];
    $scope.ComplainUnderSection = [];
    $scope.OtherPersonList = [];
    $scope.FileCollection = [];
    $scope.ComplainTrail = [];
    $scope.IODetail = [];
    $scope.ComplainUndeVlaue = [];
    $scope.EncloserFilesList = []   ;
    $scope.showPriView = false;
    $scope.IsViewComplaint = false;
    $scope.tablediv = true;
    $scope.Encloserstatus = false;
    $scope.GetComplainDetails = function (ComplainID) {
        debugger;

        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/SocietyComplaintCourtModule/GetComplainDetails?ComplainID=" + ComplainID).
            success(function (data, status) {
           
                $scope.DataList = JSON.parse(data.DataList);
                $scope.showPriView = true;
                $scope.IsViewComplaint = true;
                $scope.tablediv = false;

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

                $scope.DataList.Table.forEach(function (item, index) {
                    $scope.ComplainUnderSection.ComplainID = item.ComplainID;
                    $scope.ComplainUnderSection.ComplantBy = !item.ComplantBy ? "1" : "2";

                    $scope.ComplainUnderSection.ComplantBystatus = item.ComplantBystatus;

                    
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

                    $scope.ComplainUnderSection.OfficerNameEn = item.OfficerNameEn;
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

                    if (item.EncloserFileName != null && item.EncloserFileName != '') {

                        $scope.Encloserstatus = true;
                        debugger;
                        item.EncloserFileName.split(',').forEach(function (itm, indz) {

                            $scope.FileCollection.push({ 'Id': 'flRemovalType_' + indz, 'FileType': '', 'File': '', 'Title': '', FileName: itm, FileId: 'flRemovalType_' + indz, IsFileUpload: true });

                            if (itm!="") {
                                $scope.EncloserFilesList.push({
                                    Id: 'flRemovalType_' + indz,
                                    FileType: '',
                                    FileName: itm,
                                    IsUploadFileName: true,
                                    Is_Active: true,
                                    Is_Delete: false
                                });
                            }
                           
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

                if ($scope.DataList.Table[0].ComplantBystatus) {
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

                //if ($scope.DataList.Table[0].IsComplentForSociety) {
                //    $scope.ComplaintAgainstList = [];
                //    if ($scope.DataList.Table1.filter(function (x) { return x.Ctype === false }).length > 0) {
                //        $scope.DataList.Table1.filter(function (x) { return x.Ctype === false }).forEach(function (item, index) {
                //            $scope.ComplaintAgainstList.push({
                //                Name: item.Name,
                //                Address: item.Address,
                //                MobileNo: item.MobileNo,
                //                IsSocietyMember: item.IsSocietyMember,
                //                CDetailID: item.CDetailID,
                //                Post: item.Post,
                //                Ctype: "",
                //                IsActive: "",
                //                IsDelete: false
                //            });
                //        });
                //    }
                //}

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data1, status) {

            });
    }

    
    $scope.IsFileUpload = function (id) {

        return $scope.FileCollection.filter(function (x) { return x.FileId === id && x.IsFileUpload === true }).length > 0;
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


    $scope.ResetAddNew = function () {

        $scope.showPriView = false;
        $scope.IsViewComplaint = false;
        $scope.tablediv = true;


        $scope.ComplainUnderSection.ComplainID = 0;
        $scope.ComplainUnderSection.ComplantBystatus = "1";
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
        $scope.EncloserFilesList = [];
        $scope.OtherPersonList = [];
        $scope.FileCollection = [];

        
    }

    $scope.GetAllSubUnitList = function (DesignationLevelMappingId) {
        debugger;
        //DesignationLevelMappingId
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/User/GetinveticationListUser/?DesignationLevelMappingId=" + DesignationLevelMappingId).
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

    $scope.ComplainTrailList = [];
    $scope.GetComplainTrail = function (ComplainID) {
        debugger;

        $(".se-pre-con").fadeIn("slow");
        return $http.get(SiteUrl + "/SocietyComplaintCourtModule/GetComplainTrail/?ComplainID=" + ComplainID).
            success(function (data, status) {
                debugger;
                $scope.ComplainTrailList = JSON.parse(data.DataList);
                debugger;
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


});