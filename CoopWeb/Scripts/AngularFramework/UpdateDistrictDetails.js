

var Role = angular.module("UserRole", ['angularUtils.directives.dirPagination']);
Role.controller("Role", function ($scope, $http, $rootScope) {
  
    /*------------Paging and Filter-----------------*/
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
    //Insert user role details in db
    $scope.UploadFile = function (file) {
        $scope.File = file[0];
    }
    $scope.InsertSave = function () {
        if (!$scope.validatdata()) {
            return false;
        }
        else {
            var formData = new FormData();
            formData.append("DistrictId", $scope.DistrictId);
            formData.append("RegistrationNo", $scope.Registration);
            formData.append("SocietyId", $scope.SocietyId);
          
            $(".se-pre-con").fadeIn("slow");
            $http.post(SiteUrl + "/Society/UpdateDistrict", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (parseInt(data, 10) > 0) {
                         Msg = (_language == 'en-us' ? 'Data Saved Successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                        alert(Msg);
                        window.location.reload();
                    }
                    else if (parseInt(data, 10) == -2) {
                         Msg = (_language == 'en-us' ? 'Duplicate data.' : 'डुप्लिकेट डेटा।');
                        alert(Msg);
                    }
                    else if (parseInt(data, 10) == -1 || parseInt(data, 10) == 0) {
                         Msg = (_language == 'en-us' ? 'Some Error Occurred Please try After Some Time' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                        alert(Msg);
                    }

                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    alert("Error while auditor assignment insert");

                });
        }


    };

    $scope.pageChangeHandler = function (num) {
    };
   
 
    $scope.validatdata = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'Validate following:';


        if ($scope.DistrictId == '0' || $scope.DistrictId == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please select District' : '\n <br /> Please select District |';

        }
        if ($scope.Registration == '' || $scope.Registration == undefined) {
            message += $scope._language == 'en-us' ? '\n <br /> Please enter Registration' : '\n <br /> Please enter Registration |';

        }
        if (message.length > 29) {
            ShowModelAlert("Update District", message);
            return false;
        }
        return true;

    }


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

    // Get List of all Role
    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.StateId).
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };



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
    $scope.SocietyDistrictdataset = [];
    $scope.GSocietyListdata = function () {
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "SearchType": $scope.SearchType };
        return $http.post(SiteUrl + "/society/GSocietyList", JSON.stringify({ Details: jsonParameters })).
            success(function (data, status) {
                $scope.SocietyDistrictdataset = JSON.parse(data.DataList);
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

    //$scope.GSocietyListdata = function () {
    //    alert('f');
    //    $http.get(SiteUrl + "/login/CheckUserSession").then(function (SessionStatus) {
    //        if (SessionStatus.Status == false) {
    //            ShowOkMessage("", _language == 'en-us' ? "Your current session is logged out " : "आपका वर्तमान सेशन लॉग आउट हो गया  है", function () { window.location = "http://sso.rajasthan.gov.in/" });
    //        } else {
    //            $(".blockUI").fadeIn("slow");
    //            var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "SearchType": $scope.SearchType };
    //            return $http.post(SiteUrl + "/society/GSocietyList", JSON.stringify({ Details: jsonParameters, RequestType: $scope.ListType })).
    //                success(function (data, status) {
    //                    $(".blockUI").fadeOut("slow");
    //                    // $scope.OrderList = [];                      
    //                    $scope.SocietySet = JSON.parse(data.DataList);
    //                    $scope.NewDt = $scope.CCSComplaintList;
    //                    $rootScope.DataPagingDetails = data.DataPaging;
    //                    $rootScope.SetDataPaging();
    //                    $(".blockUI").fadeOut("slow");

    //                    //if ($scope.CCSComplaintListSelected.length > 0) {
    //                    //    $scope.CCSComplaintListSelected.forEach(function (itm, idx) {
    //                    //        if ($scope.CCSComplaintList.filter(function (x) { return x.ComplaintId === itm.ComplaintId }).length > 0) {
    //                    //            $scope.CCSComplaintList.filter(function (x) { return x.ComplaintId === itm.ComplaintId })[0].IsChecked = true;
    //                    //        }
    //                    //    });
    //                    //}

    //                   // $scope.GetCCSListNew();
    //                }).
    //                error(function (data, status) {
    //                    $scope.CCSComplaintList = [];
    //                    $(".blockUI").fadeOut("slow");
    //                    //alert('Error while fetching society list data.');
    //                });
    //        }

    //    });


    //};
    


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

    $scope.AddClick = function (SocietyId, RegistrationNo) {
        $scope.Registration = RegistrationNo;
        $scope.SocietyId = SocietyId;
    };
    // START : Get Data for Update
    $scope.update = function (tableData) {
        $scope.userrole = tableData;
    };

    $scope.SetStatus = function (GodownId, ActionId) {

        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/society/ChangeRoleStatus", { Id: GodownId, Action: ActionId }).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.GetGodownlist();
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
            });
    };






    $scope.SaveAssignRights = function () {
        $(".blockUI").fadeIn("slow");
        var RolemenuActionCollection = [];
        var tempMenuActions = '';
        //angular.forEach($scope.AllMenuList, function (value) {
        //    if (value.Checked || value.MenuIdChecked == '1') {
        //        var item = new RolemenuAction();
        //        item.MenuId = value.MenuId;
        //        item.IsActive = value.IsActive;
        //        item.RoleId = $scope.RoleId;
        //        tempMenuActions = '';
        //        angular.forEach(value.MenuActionList, function (value2) {
        //            if (value2.RoleMenuActionCheck == '1') {
        //                tempMenuActions += value2.Key + ",";
        //            }
        //        });
        //        item.MenuAction = tempMenuActions.substr(0, tempMenuActions.length - 1);
        //        RolemenuActionCollection.push(item);
        //    }
        //});

        $scope.AllMenus.forEach(function (value, index) {
            if (value.IsChecked) {
                var item = new RolemenuAction();
                item.MenuId = value.MenuId;
                item.IsActive = value.IsActive;
                item.RoleId = $scope.RoleId;
                tempMenuActions = '';
                angular.forEach(value.MenuActionList, function (value2) {
                    if (value2.RoleMenuActionCheck == '1') {
                        tempMenuActions += value2.Key + ",";
                    }
                });
                item.MenuAction = tempMenuActions.substr(0, tempMenuActions.length - 1);
                RolemenuActionCollection.push(item);
            }
        });

        $http.post(SiteUrl + "/User/SaveAssignRights", { list: RolemenuActionCollection }).
            success(function (data2, status) {
                $(".blockUI").fadeIn("slow");
                if (data2.Status) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    window.location = '../User/UserRole';
                    $(".se-pre-con").fadeOut("slow");
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    window.location = '../User/UserRole';
                    $(".se-pre-con").fadeOut("slow");

                }


            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while Assign Rights insert");
            });


    };
    //on page load start

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
    $scope.GetDistrictList();
    $scope.GSocietyListdata();


});