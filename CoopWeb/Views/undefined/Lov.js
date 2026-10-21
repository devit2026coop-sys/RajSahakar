var LovModule = angular.module('LovMod', ['angularUtils.directives.dirPagination'])
LovModule.controller("LovCnt", function ($scope, $http) {

    $scope.LovValues = [];
    $scope.currentPage = 500;
    $scope.pageSize = 5;
    $scope.DivLov = true;
    $scope.newdiv = "";


    $scope.SaveLovValue = function (LovValues) {
        var Msg='';

        
        if ($scope.LOVvalue.LovListId == '0') {
            Msg += (_language == 'en-us' ? 'Please choose value for List of Values - Code\n' : 'मूल्यों की सूची - कोड चयन करें\n');
        }
        
      

        if (Msg.length > 0) {
            alert(Msg);
            return;
        }

    
        $http.post(SiteUrl+"/Lov/SaveLovValue", $scope.LovValues).
            success(function (data, status) {
                var msg1 = "";
                if (data > 0) {
                    msg1 = (_language == 'en-us' ? 'Data Save Sucessfully\n' : 'डेटा सहेजा गया है\n')
                        || $scope.ClearLovValueForm();
                }
                else {
                    alert(_language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n') || $scope.ClearLovValueForm();
                    return;
                }
               
            }).
            error(function (data, status) {
                alert("Error while Lov insert");
            });
    };

    //START : Get Language on Page Load
    $scope.OnPageLoad = function () {
        $http.get(SiteUrl+"/LOV/GetLov").

            success(function (data2, status) {

                if (data2[0].ActionID != "") {


                    var str = data2[0].FollowUpList;

                    var str_array = str.split(',');


                    angular.forEach(str_array, function (Datavalue) {
                        if (Datavalue == 'Add') {

                            $scope.IsAdd = true;
                        }
                        else if (Datavalue == 'Edit') {
                            //$scope.tdEdit.style.display = "none";
                            $scope.IsEdit = true;
                        }
                        else if (Datavalue == 'Take Action') {
                            //$scope.tdEdit.style.display = "none";
                            $scope.IsTakeAction = true;
                        }
                        else if (Datavalue == 'Status') {
                            //$scope.tdEdit.style.display = "none";
                            $scope.IsStatus = true;
                        }
                        else if (Datavalue == 'delete') {
                            //$scope.tdEdit.style.display = "none";
                            $scope.IsDelete = true;
                        }
                        else {
                            $scope.IsAdd = false;
                            $scope.IsEdit = false;
                            $scope.IsTakeAction = false;
                            $scope.IsStatus = false;
                            $scope.IsDelete = false;
                        }
                    });

                }

            }).
            error(function (data1, status) {

                $scope.WorkFlowExecution = [];
            });

        $scope._language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl+"/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl+"/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }


        $http.get(SiteUrl+"/Scheme/SchemeList").
            success(function (data, status) {
                if (angular.toJson(data).length < 1) {
                    $scope.DivLov = false;
                    $scope.newdiv = (_language == 'en-us' ? 'Data is not available' : 'डेटा उपलब्ध नहीं है ');
                    return;
                }
                $scope.SchemeList = data;
            }).
            error(function (data, status) {
                $scope.SchemeList = [];
            });

        $scope.GetLovList();

        //var myJson = [{ "Id": "1", "Value": "Jitendra" }, { "Id": "2", "Value": "Rajeev" }, { "Id": "3", "Value": "Vaibhav" }, { "Id": "4", "Value": "Govind" }];
        //var keycount = Object.keys(myJson[0]);
        //var range = [];
        //for (var i = 0; i < keycount.length; i++) {
        //    range.push(keycount[i]);
        //}

        //$scope.myJson = myJson;
        //$scope.range = range;
    };
    //END : Get Language on Page Load

    // START : Get all the LOV Items
    $scope.GetLovList = function () {
        $http.get(SiteUrl+"/Lov/LovList").
            success(function (data, status) {
                $scope.LovData = data;
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
                alert("Error while fetching Lov list.");
                $scope.showLoader = false;
            });
    }
    //END : Get all the LOV Items

    // START : Get all the LOV Items
    $scope.GetLovValueList = function () {
        $http.get(SiteUrl+"/Lov/LovValueList").
            success(function (data, status) {
                $scope.LovValueData = data;
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
                alert("Error while fetching Lov Value list.");
            });

        $http.get(SiteUrl+"/Lov/LovList").
            success(function (data, status) {
                $scope.LovddlData = data;
            }).
            error(function (data, status) {
            });
    }
    //END : Get all the LOV Items

    // START : Save LOV
    $scope.SaveLOV = function (LOV) {
        var Msg = '';
        if ($scope.LOV.SchemeId == '0')
        {
            Msg += (_language == 'en-us' ? 'Plaese select the list of value\n' :'कृपया मूल्य की सूची का चयन करें\n')

        }
        if (Msg.length > 0) {
            alert(Msg);
            return;
        }
        

        $http.post(SiteUrl+"/Lov/SaveLov", { dat: LOV }).
            success(function (data, status) {
                var msg1 = "";
                if (data > 0) {
                    msg1 = (_language == 'en-us' ? 'Data Save Sucessfully\n' : 'डेटा सहेजा गया है\n')
                        || $scope.ClearLovForm();
                }
                else {
                    alert("some error occured") || $scope.ClearLovForm();
                    return;
                }

                $scope.GetLovList();
            }).
            error(function (data, status) {
                $scope.SchemeList = [];
            });
    };
    // START : Save LOV

    //START : Update data
    $scope.LovUpdate = function (tableData) {
        $scope.LOV = tableData;
    };
    //END : Update data

    //START : Update LOV Value data
    $scope.LovValueUpdate = function (tableData) {
        $scope.LOVvalue = tableData;
    };
    //END :  Update LOV Value data

    // START : Clear form
    $scope.ClearLovForm = function () {
        $scope.LOV = null;
    };
    $scope.ClearLovValueForm = function () {
        $scope.LOVvalue = null;
    };
    // END : Clear form

    // START : Change Status
    $scope.SetStatus = function (_Id, _status) {
        if (_status == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        _status = (_status == 0 ? 1 : (_status == 1 ? 0 : _status));
        $http.post(SiteUrl+"/Lov/ChangeStatus", { Id: _Id, Status: _status }).
            success(function (data, status) {

                if (parseInt(data, 10) > 0) {
                    if (_status != 2)
                        alert($scope.translation.StatusChange);
                    else
                        alert($scope.translation.Deleted);
                }
                $scope.GetLovList();
            }).
            error(function (data, status) {
            });
    };
    // END : Change Status

    // START : Change SetStatusValue
    $scope.SetStatusValue = function (_Id, _status) {
        if (_status == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        _status = (_status == 0 ? 1 : (_status == 1 ? 0 : _status));
        $http.post(SiteUrl+"/Lov/ChangeStatusLov", { Id: _Id, Status: _status }).
            success(function (data, status) {

                if (parseInt(data, 10) > 0) {
                    if (_status != 2)
                        alert($scope.translation.StatusChange);
                    else
                        alert($scope.translation.Deleted);
                }
                $scope.GetLovList();
            }).
            error(function (data, status) {
            });
    };
    // END : Change SetStatusValue


    // Page Load event list
    $scope.OnPageLoad();
    $scope.GetLovValueList();
});
