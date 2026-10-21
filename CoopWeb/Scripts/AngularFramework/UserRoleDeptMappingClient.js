var AppScheme1 = angular.module("UserRoleDeptMapping", ['angularUtils.directives.dirPagination']);
AppScheme1.controller("UserRoleDept", function ($scope, $rootScope, $http) {
    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 50,
        sortExpression: '',
        sortDirection: '1',
        searchParameters: []
    }
    //$scope.currentPage = 1;
    $scope.pageSize = 50;
    $rootScope.pageRequest = function (pageid, callback) {
        $scope.DataPaging.currentPageID = parseInt(pageid);
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else if ($(this).attr("type") == "radio" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
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
        //$scope.DataPaging.searchParameters = [];
        $scope.DataPaging.searchParameters.splice(0, $scope.DataPaging.searchParameters.length);
        var ln = $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').length;
        var indx = 1;
        $('#' + $scope.DataPaging.searchBoxDivId).find('input:text,select,input:checkbox,input:radio').each(function () {

            if ($(this).attr("type") == "checkbox" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
                    $scope.DataPaging.searchParameters.push(parameter);
                }
            } else if ($(this).attr("type") == "radio" && $(this).attr('type') != undefined) {
                if ($(this).is(":checked")) {
                    var parameter = { Key: $(this).attr('name'), Value: $(this).val() }
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
    $scope.ClearSearch = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.FilterType = "";
        $scope.showSearchDate = false;
        $scope.showSearchText = true;
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.SearchData();
    }
    $scope.DataPaging.currentPageID = 1;

    

    // START : Code for Language label setting
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
    };
    // END : Code for Language label setting


    //Get User List for dropdown
    $scope.UserList = function () {
        $http.get(SiteUrl + "/User/SelectUser").
            success(function (data, status) {

                $scope.Users = data;
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
                $scope.showLoader = false;

            }).
            error(function (data, status) {

                $scope.showLoader = false;
            });
    };


    //Get Department List for dropdown
    $scope.DepartmentList = function () {

        $http.get(SiteUrl + "/User/SelectDepartment").
            success(function (data, status) {

                $scope.Departments = data;
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
                $scope.showLoader = false;

            }).
            error(function (data, status) {

                $scope.showLoader = false;
            });
    };

    //Get Designation List for dropdown
    $scope.DesignationList = function () {
        $http.get(SiteUrl + "/User/SelectDesignation").
            success(function (data, status) {

                $scope.Designations = data;
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
                $scope.showLoader = false;

            }).
            error(function (data, status) {

                $scope.showLoader = false;
            });
    };

    $scope.toggleAll = function () {
        var toggleStatus = $scope.isAllSelected;
        angular.forEach($scope.AssignMoreRights, function (itm) { itm.Checked = toggleStatus; });

    }

    $scope.optionToggled = function () {
        $scope.isAllSelected = $scope.AssignMoreRights.every(function (itm) { return itm.Checked; })
    }

    //Get Post List for dropdown
    $scope.PostList = function () {
        $http.get(SiteUrl + "/User/SelectPost").
            success(function (data, status) {

                $scope.Posts = data;
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
                $scope.showLoader = false;

            }).
            error(function (data, status) {
                alert("Error while fetching Post list")
                $scope.showLoader = false;
            });
    };

    //Get Role List for dropdown
    $scope.RoleList = function () {
        $http.get(SiteUrl + "/User/SelectRole").
            success(function (data, status) {

                $scope.Roles = data;
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
                $scope.showLoader = false;

            }).
            error(function (data, status) {
                alert("Error while fetching Role list")
                $scope.showLoader = false;
            });
    };

    //Clear form
    $scope.ClearAssignMoreRightsForm = function () {

        $scope.AssignMoreRights = "0";
        $scope.SingleChoise = "0";
        $scope.IsVisible = false;

        $scope.UserRoleDeptMapping();

    };

    $scope.ClearUserRoleDeptMappingForm = function () {

        $scope.UserRoleDept.RoleID = '0';
        $scope.UserRoleDept.PostId = '0';
        $scope.UserRoleDept.DESIGNATIONCODE = '0';
        $scope.UserRoleDept.DEPTCODE = '0';
        $scope.UserRoleDept.UserId = '0';

        $scope.UserRoleDept.RoleID = "0";
        $scope.UserRoleDept.PostId = "0";
        $scope.UserRoleDept.DESIGNATIONCODE = "0";
        $scope.UserRoleDept.DEPTCODE = "0";
        $scope.UserRoleDept.UserId = "0";



    };

    $scope.AdduserRoleDeptClick = function () {
        $scope.SingleChoise = false;
        $scope.userrole = '';
        $scope.IsVisible = false;
        $scope.RoleList = null;
    };

    //Insert User Role Department Mapping
    $scope.SaveUserRoleDeptMapping = function () {
        var Msg = '';


        if ($scope.UserRoleDept.UserId == '0') {
            Msg += (_language == 'en-us' ? 'Please choose User\n' : 'कृपया उपयोगकर्ता चुनें\n');
        }
        if ($scope.UserRoleDept.DEPTCODE == '0') {
            Msg += (_language == 'en-us' ? 'Please choose Department Code\n' : 'कृपया विभाग कोड चुनें\n ');
        }
        if ($scope.UserRoleDept.DESIGNATIONCODE == '0') {
            Msg += (_language == 'en-us' ? 'Please choose designation Code\n' : 'कृपया पद कोड चुनें\n');
        }
        if ($scope.UserRoleDept.PostId == '0') {
            Msg += (_language == 'en-us' ? 'Please choose Post\n' : 'कृपया पोस्ट चुनें\n');
        }
        if ($scope.UserRoleDept.RoleID == '0') {
            Msg += (_language == 'en-us' ? 'Please choose Role\n' : 'कृपया भूमिका चुनें\n');
        }

        if (Msg.length > 0) {

            Msg = (_language == 'en-us' ? 'Please validate following details : \n' : 'कृपया निम्नलिखित विवरण मान्य करें : \n') + Msg;
            
            alert(Msg);
            return;
        }




        $http.post(SiteUrl + "/User/UserRoleDeptMappingInsert", $scope.UserRoleDept).
            success(function (data, status) {
                var k = data;
                var message = '';
                window.location = '/User/UserRoleDeptMapping';
            }).
            error(function (data, status) {
                alert("Error while UserRoleDeptMappingInsert")
            });
    }

    //Get User Role Department Mapping

    $scope.UserRoleDeptMapping = function () {
        $(".se-pre-con").fadeIn("slow");
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
        $http.post(SiteUrl + "/User/GetUserRoleDeptMapping", JSON.stringify({ Details: jsonParameters })).
            success(function (responcedata, status) {
                $(".se-pre-con").fadeOut("slow");
                var data = JSON.parse(responcedata.DataList);
                $scope.GetUserRoleDeptMapping = data;//data;
                
                $rootScope.DataPagingDetails = responcedata.DataPaging;
                $rootScope.DataPagingDetails.TotalRecords = $scope.GetUserRoleDeptMapping[0].TotalRow;
                $rootScope.SetDataPaging();

                $scope.colsort = "";
                $scope.reverseSort = false;
                //$rootScope.DataPagingDetails = responcedata.DataPaging;
                //$rootScope.SetDataPaging();

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
                $(".se-pre-con").fadeOut("slow");
                alert("Error while GetUserRoleDeptMapping")
            });
    };


    $scope.pageChangeHandler = function (num) {
    };

    //Update Status
    $scope.SetStatus = function (MappingID, ActionId) {

        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;
        $http.post(SiteUrl + "/User/ChangeStatusUserRoleDept", { Id: MappingID, Action: ActionId }).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.UserRoleDeptMapping();
                $scope.IsVisible = false;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
            });
    };


    $scope.updateURDM = function (item) {
        $scope.UserRoleDept.RoleID = item.RoleId;
        $scope.UserRoleDept.PostId = item.PostId;
        $scope.UserRoleDept.DESIGNATIONCODE = item.DESIGNATIONCODE;
        $scope.UserRoleDept.DEPTCODE = item.DEPTCODE;
        $scope.UserRoleDept.UserId = item.UserId;
        $scope.UserRoleDept.IsActive = item.IsActive;


    };
    $scope.SetUncheckStatus = function (AllMenurow, ActionId, check) {
        if (ActionId == 1 || ActionId == 0) {
            if (!check) {
                AllMenurow.MenuIdChecked = '0';
            }
            else if (check) {
                AllMenurow.MenuIdChecked = '1';

            }
            else {
                AllMenurow.MenuIdChecked = '0';
            }
        }
    };

    $scope.ShowHide = function () {

        $scope.IsVisible = $scope.Show;
    }

    $scope.AssignMoreRightsFunction = function (tableData) {
    $http.post(SiteUrl + "/User/AssignMoreRights", { WhereClause: ' IsActive !=2 and MappingId=' + tableData.MappingID }).
        success(function (data, status) {
            $(".se-pre-con").fadeIn("slow");
                angular.forEach(data.ChildData, function (ChildDatavalue) {
                    var MenuIdChecked = 0;
                    var MappingID = 0;
                    angular.forEach(data.ParentData, function (ParentDatavalue, e) {
                        if (ChildDatavalue.MenuId == ParentDatavalue.MenuId) {
                            ParentDatavalue.MenuIdChecked = 1;
                            ParentDatavalue.MappingId = ChildDatavalue.MappingId;
                            data.ParentData[e] = ParentDatavalue;
                            var str = ChildDatavalue.ActionID;
                            var str_array = str.split(',');
                            angular.forEach(str_array, function (Datavalue) {
                                angular.forEach(ParentDatavalue.MenuActionList, function (val) {
                                    if (Datavalue == val.Key) {
                                        val.RoleMenuActionCheck = true;
                                    }
                                });
                            });
                        }
                    });
                }
                );

                $scope.DepartmentId = tableData.DEPTCODE;
                $scope.RoleId = tableData.RoleId;
                $scope.UserId = tableData.UserId;
                $scope.MappingID = tableData.MappingID;
                $scope.AssignMoreRights = data.ParentData;
                $scope.IsVisible = true;
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
            $scope.showLoader = false;
            $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while AssignMoreRightsFunction list.");
                $scope.showLoader = false;
            });

    };

    function UserRolemenuAction() {
        this.RoleId = '';
        this.MenuId = '';
        this.UserId = '';
        this.MenuAction = '';
        this.DepartmentId = '';
        this.MenuActive = 0;
        this.MappingID = '';
    }
    function RolemenuAction() {
        this.RoleId = '';
        this.RoleId = '';
        this.MenuId = '';
        this.MenuAction = '';
        this.IsActive = 0;
    }
    $scope.SaveAssignMoreRights = function () {
        var RolemenuActionCollection = [];
        var tempMenuActions = '';
        angular.forEach($scope.AssignMoreRights, function (value) {
            if (value.Checked || value.MenuIdChecked == '1') {
                var item = new UserRolemenuAction();
                item.MenuId = value.MenuId;
                item.UserId = $scope.UserId;
                item.IsActive = value.IsActive;
                item.RoleId = $scope.RoleId;
                item.MappingID = $scope.MappingID;
                item.DepartmentId = $scope.DepartmentId;
                tempMenuActions = '';
                item.MenuActive = 1;
                angular.forEach(value.MenuActionList, function (value2) {
                    if (value2.RoleMenuActionCheck == '1') {
                        tempMenuActions += value2.Key + ",";
                    }
                });
                item.MenuAction = tempMenuActions.substr(0, tempMenuActions.length - 1);
                RolemenuActionCollection.push(item);
            }
        });
        $http.post(SiteUrl + "/User/SaveAssignMoreRights", { list: RolemenuActionCollection }).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");

                if (data.Status) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                    alert(message);
                    window.location = '/User/UserRoleDeptMapping';
                    $(".se-pre-con").fadeOut("slow");
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    window.location = '/User/UserRoleDeptMapping';
                    $(".se-pre-con").fadeOut("slow");

                }


            }).
            error(function (data, status) {
                alert("Error while Assign Rights insert");
            });
    };

    $scope.changeOption = function () {
        $scope.submitted = false;
    };
    $scope.getTranslation($scope);
    $scope.UserList();
    $scope.DepartmentList();
    $scope.DesignationList();
    $scope.PostList();
    $scope.RoleList();
    $scope.UserRoleDeptMapping();
    $scope.IsVisible = false;


});
