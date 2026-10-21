
var Role = angular.module("UserRole", ['angularUtils.directives.dirPagination']);
Role.controller("Role", function ($scope, $http, $rootScope) {
    /*------------Paging and Filter-----------------*/
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.divClass = false;
    $scope.SportAssociation = '';

    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 100,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    };
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
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.FilterType = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.GetWarehouseList();
    }
    /*------------End Paging and Filter-----------------*/
    //$scope.InsertSave = function () {
       
    //    if (!$scope.validatdata()) {
    //        return false;
    //    }
    //    else {
    //        var formData = new FormData();
    //        formData.append("GodownId", $scope.godownid);
    //        formData.append("Status", $scope.godown.status);
    //        formData.append("Description", $scope.godown.Description);
    //        formData.append("ActualCost", $scope.godown.ActualCost);
    //        formData.append("Longitude", $scope.godown.Longitude);
    //        formData.append("Latitude", $scope.godown.Latitude);
    //        formData.append("CompletionDate", $('#idddate').val());



    //        if (document.getElementById("godownPhoto").files.length > 0) {
    //            formData.append("file", document.getElementById("godownPhoto").files[0]);
    //        }
    //        $(".se-pre-con").fadeIn("slow");
    //        $http.post(SiteUrl + "/Society/UpdateStatusGodown", formData,
    //            {
    //                withCredentials: true,
    //                headers: { 'Content-Type': undefined },
    //                transformRequest: angular.identity
    //            }).
    //            success(function (data, status) {
    //                $(".se-pre-con").fadeOut("slow");
    //                if (parseInt(data, 10) > 0) {
    //                    var Msg = (_language == 'en-us' ? 'Data Update Successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
    //                    alert(Msg);
    //                    window.location.reload();
    //                }                   
    //                else if (parseInt(data, 10) == -1 || parseInt(data, 10) == 0) {
    //                    var Msg = (_language == 'en-us' ? 'Some Error Occurred Please try After Some Time' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
    //                    alert(Msg);
    //                }

    //            }).
    //            error(function (data, status) {
    //                $(".se-pre-con").fadeOut("slow");
    //                alert("Error while auditor assignment insert");

    //            });
    //    }


    //};

    $scope.pageChangeHandler = function (num) {
    };
   
 
    //$scope.validatdata = function () {
       
    //    var message = $scope._language == 'en-us' ? 'Validate following : ' : 'Validate following:';
      

    //    if ($scope.godown.status == '0' || $scope.godown.status == undefined) {
    //        message += $scope._language == 'en-us' ? '\n <br /> Please select Status' : '\n <br /> Please select Status ';

    //    }

     
    //    if ($('#idddate').val() == '' || $('#idddate').val() == undefined) {
    //        message += $scope._language == 'en-us' ? '\n <br /> Please select Completion Date' : '\n <br /> Please select Completion Date ';

    //    }
        
    //    if ($scope.godown.Longitude == '0' || $scope.godown.Longitude == undefined) {
    //        message += $scope._language == 'en-us' ? '\n <br /> Please select Longitude' : '\n <br /> Please select Longitude ';

    //    }
    //    if ($scope.godown.Latitude == '0' || $scope.godown.Latitude == undefined) {
    //        message += $scope._language == 'en-us' ? '\n <br /> Please select Latitude' : '\n <br /> Please select Latitude ';

    //    }
    //    if ($scope.godown.Latitude == '0' || $scope.godown.Latitude == undefined) {
    //        message += $scope._language == 'en-us' ? '\n <br /> Please select Photo' : '\n <br /> Please select Photo ';

    //    }
    //    if ($scope.godown.ActualCost == '0' || $scope.godown.ActualCost == undefined) {
    //        message += $scope._language == 'en-us' ? '\n <br /> please select actualcost' : '\n <br /> please select ActualCost ';

    //    }
       
    //    if (message.length > 29) {
    //        ShowModelAlert("Godown Status", message);
    //        return false;
    //    }
    //    return true;

    //}
    $scope.GetStatusList = function () {
        $http.get(SiteUrl + "/society/GetGodownStatus").
            success(function (data, status) {
                $scope.StatusList = data;
            }).
            error(function (data, status) {
                $scope.Level = [];
            });
    };
  

    $scope.GetStatusList();

    // START : Code for Language label setting
    $scope.getTranslation = function () {
        $scope.language = _language;
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
    };

    //Start: Get Unit List
    $scope.LevelList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=LEVELS&MasterCode=0").
            success(function (data, status) {
                $scope.LevelListItem = data;
            }).
            error(function (data, status) {
                $scope.Level = [];
            });
    };
    //End 

    $scope.godown = [];
    $scope.GetGodownlist = function () {
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "SearchType": $scope.SearchType };
        return $http.post(SiteUrl + "/MIS/Godownlists", JSON.stringify({ Details: jsonParameters })).
            success(function (data, status) {
                

                $scope.godown = JSON.parse(data.DataList);
                alert($scope.godown );
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while fetching role list.");
                $scope.showLoader = false;
            });



    };

    $scope.GridData2 = function () {
        $(".se-pre-con").fadeIn("slow");

        $http.post(SiteUrl + "/MIS/Godownlists").
            success(function (data, status) {
                
                if (data.length > 0) {
                   
                    $scope.godown = data;

                    $(".se-pre-con").fadeOut("slow");

                }
                else
                    $scope.godown = [];

            }).
            error(function (data, status) {
                alert("Error while fetching Year Target Data");
            });

    }



    $scope.RoleListByLevel = function () {
        $scope.RoleList('and Role.LevelID=' + $scope.userrole.LevelID);
    };
    $scope.MenuList = function () {
        $http.get(SiteUrl +"/Menu/MenuList").
            success(function (data, status) {
                $scope.Menus = data;//data;
            }).
            error(function (data, status) {
                alert("Error while fetching menu list");
            });
    };
    $scope.MenuListbyparent = function () {
            $scope.AssignRights(Mdata);
    };



    $scope.toggleAll = function () {
        var toggleStatus = $scope.isAllSelected;
        angular.forEach($scope.AllMenuList, function (itm) { itm.Checked = toggleStatus; });

    }

    $scope.optionToggled = function () {
        //$scope.isAllSelected = $scope.AllMenuList.every(function (itm) { return itm.Checked; })
    }


    //Clear user Role
    $scope.ClearRoleForm = function () {
        $scope.userrole = '';
        $scope.SingleChoise = false;
        $scope.userrole.LevelID = '0';


    };

    $scope.ClearAssignRightsForm = function () {
        window.location = '../User/UserRole';
    };

    $scope.AddClick = function (id) {
         $scope.godownid = id;
    };
    // START : Get Data for Update
    //$scope.update = function (tableData) {
    //    $scope.userrole = tableData;
    //};

    //$scope.SetStatus = function (GodownId, ActionId) {

    //    if (ActionId == 2)
    //        if (!confirm($scope.translation.DeleteConfirm))
    //            return false;

    //    $http.post(SiteUrl + "/society/ChangeRoleStatus", { Id: GodownId, Action: ActionId }).
    //        success(function (data, status) {
    //            $(".se-pre-con").fadeIn("slow");
    //            $scope.GetGodownlist();
    //            $(".se-pre-con").fadeOut("slow");
    //        }).
    //        error(function (data, status) {
    //            $(".se-pre-con").fadeOut("slow");
    //        });
    //};





    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $(".se-pre-con").fadeOut("slow");
       

    }
    //on page load end
    //Get list of all users on page list
    $scope.getTranslation();
    $scope.IsVisible = false;
   // $scope.RoleList('and 1=1');
    $scope.OnPageLoad();
    $scope.LevelList();
    $scope.MenuList();
    //$scope.SingleChoise = false;
    //$scope.GetGodownlist();
    $scope.GridData2();


});