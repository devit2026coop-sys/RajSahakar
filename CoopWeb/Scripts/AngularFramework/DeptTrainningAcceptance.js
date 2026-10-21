var ArrSelectTraningUser = new Array();
var SocietyRegistrationData = [];
function clickTraningUserSelected() {
    this.UserId = '0';
}


var AppClasses = angular.module("AppClassesFormat", ['angularUtils.directives.dirPagination']);
AppClasses.controller("Classes", function ($scope, $http) {
    // Save Sms Template 
    $scope.currentPage = 1;
    $scope.pageSize = 100;
    $scope.OrgId = 0;
    // on page load
    $scope.OnPageLoad = function () {
        if (window.location.href.indexOf('=') > -1) {
            $scope.GetTrainningListById(window.location.href.split('=')[1])
        }
        else {
            $scope.GetAllTrainningList();
        }
        $(".se-pre-con").fadeIn("slow");
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
        $scope.ClearClassForm = function () {
            $scope.Classes = '';
        };
        $(".se-pre-con").fadeOut("slow");

        //$scope.GetAllMailData();

    };
    // Language Selector at page load

    // Get List of Channel 
    $scope.ChannelList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Channels = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Channel&MasterCode=-1").
            success(function (data, status) {
                //var _Result = JSON.parse(data);
                $scope.Channels = data;
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
                $scope.Channels = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    // Get List of Channel 
    $scope.TargetList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.Targets = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Target&MasterCode=-1").
            success(function (data, status) {
                //var _Result = JSON.parse(data);
                $scope.Targets = data;
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
                $scope.Targets = [];
            });
        $(".se-pre-con").fadeOut("slow");
    };

    $scope.GetAllTrainningList = function () {
        $scope.trainninggrid = true;
        $http.post(SiteUrl + "/Trainning/TrainningList", $scope.Trainning).
            success(function (data, status) {
                $scope.TrainningData = data.filter(x => x.ChannelId != '');
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
                $scope.WorkFlowExecution = [];
            });
    }

    $scope.GetTrainningListById = function (SID) {
        $scope.ShowDiv = true;
        // alert($scope.Trainning.IsAccepted);
        // $scope.Trainning.IsAccepted = 'true';
        $scope.divaccept = true;
        $scope.trainninggrid = false;
        $scope.TrainningData = null;
        $http.post(SiteUrl + "/Trainning/GetTrainningListById", { "Trainningid": SID }).
            success(function (data, status) {
                $scope.TraningOrgdata = data;
                $scope.SingleTrainningData = data[0];
                var orgid = $scope.SingleTrainningData.OrganisationId;
                $scope.LevelList(orgid);
            }).
            error(function (data, status) {
                $scope.TrainningData = [];
            });

        $scope.trainninggrid = false;

    }


    $scope.ShowHide = function () {
        if ($scope.Trainning.IsAccepted == 'true')
            $scope.ShowDiv = true;
        else
            $scope.ShowDiv = false;
    };

    $scope.GetOrganisations = function () {
        $http.get(SiteUrl + "/Master/OrganisationsList").
            success(function (data1, status) {
                $scope.Organisations = data1;
            }).
            error(function (data, status) {
                $scope.Organisations = '';
            });
    };

    $scope.OfficeList = function () {
        var Level_fordesignation = $scope.Trainning.LevelId;
        if (Level_fordesignation.indexOf("~") >= 0) {
            var arrayOfStrings = Level_fordesignation.split("~");
            $scope.Trainning.OrganizationLevelsMappingID = arrayOfStrings[1];
            $scope.Trainning.WorkScopeLevel_ID = arrayOfStrings[0];

        }
        else {
            $scope.UserRegistration.OrganizationLevelsMappingID = 0;
            $scope.UserRegistration.WorkScopeLevel_ID = 0;
        }

        $scope.Trainning.OfficeId = "0";
        $scope.OfficeListData = [];
        $http.get(SiteUrl + "/Master/OfficesList/?WhereClause=and o.isactive =1 " + ($scope.Trainning.OrganizationLevelsMappingID && $scope.Trainning.OrganizationLevelsMappingID != 0 ? " and l.LevelId=" + $scope.Trainning.OrganizationLevelsMappingID : "")).
            success(function (data, status) {
                $(".se-pre-con").fadeIn("slow");
                $scope.OfficeListData = data;
                //$scope.OfficeListData = $filter('filter')(data, function (item, index, array) {
                //    return array.findIndex(obj => obj.OfficeNameEn === item.OfficeNameEn) === index;
                //});

                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                alert("Error while fetching designation list");
                $scope.OfficeListData = [];
                $scope.showLoader = false;
            });
    };


    $scope.GetDesignationList = function () {
        var Level_fordesignation = $scope.Trainning.LevelId;
        if (Level_fordesignation.indexOf("~") >= 0) {
            var arrayOfStrings = Level_fordesignation.split("~");
            $scope.Trainning.OrganizationLevelsMappingID = arrayOfStrings[1];
            $scope.Trainning.WorkScopeLevel_ID = arrayOfStrings[0];

        }
        else {
            $scope.Trainning.OrganizationLevelsMappingID = 0;
            $scope.Trainning.WorkScopeLevel_ID = 0;
        }

        //  $http.get(SiteUrl + "/MasterSettings/GetAllDesignationByOrganizationLevelsMappingID/?OrganizationLevelsMappingID=" + $scope.Trainning.OrganizationLevelsMappingID + "&OfficeId=" + $scope.Trainning.OfficeId).
        $http.get(SiteUrl + "/MasterSettings/GetAllDesignationByOrganizationLevelsMappingID/?OrganizationLevelsMappingID=" + $scope.Trainning.OrganizationLevelsMappingID + "&OfficeId=" + $scope.Trainning.WorkScopeLevel_ID).
            success(function (data, status) {
                $scope.Designations = data;
            }).
            error(function (data, status) {
                $scope.Designations = [];
            });

        $scope.Trainning.DesignationLevelMappingId = '0';
        $scope.divuser = false;
    };

    //Start: Get Unit List
    $scope.LevelList = function (id) {
        $http.get(SiteUrl + "/MasterSettings/OrganizationLevelsList/?WhereClause=AND OLM.OrganizationId=" + id).
            success(function (data, status) {
                $scope.Level = data;
            }).
            error(function (data, status) {
            });
    };
    //End 

    $scope.GetTrainningAcceptUserList = function () {
        $scope.divuser = true;
        $http.post(SiteUrl + "/Trainning/TrainningAcceptUserList", $scope.Trainning).
            success(function (data1, status) {
                $scope.AcceptUSerLst = data1;
            }).
            error(function (data, status) {
                $scope.Organisations = '';
            });
    };

    $scope.ClickToChkUsers = function (UserId, MappingID) {
        var _SelectedToChkUsers = new clickTraningUserSelected();
        _SelectedToChkUsers.UserId = UserId;
        _SelectedToChkUsers.DepartMappingId = MappingID;
        ArrSelectTraningUser.push(_SelectedToChkUsers);
    }

    $scope.SaveTrainningUsersList = function () {
        if (ArrSelectTraningUser.length === 0) {
            var msg = _language == 'en-us' ? 'Please select user' : 'कृपया उपयोगकर्ता का चयन करें';
            ShowModelAlert($scope.translation.DeptTrainningAcceptance, msg, function () { });
            return false;
        }

        $.ajax({
            url: '../Trainning/SaveTrainningSelectUser',
            data: JSON.stringify({

                "_Model": {
                    "TrainningId": $scope.SingleTrainningData.TrainningId,
                    "lstTrainningUsers": ArrSelectTraningUser,
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                var k = data;

                var message = '';

                if (k == 1) {
                    alert("Save Sucessfully");
                    window.location = '../Trainning/DeptTrainningAcceptance';
                    $scope.GetTrainningAcceptUserList();
                }
                else {
                    alert("User Already Alloted");
                    //window.location = '/Trainning/Trainning';
                    //window.location = '/Trainning/DeptTrainningAcceptance';
                }
            },

        })


    };

    $scope.TrainningReject = function () {
        $.ajax({
            url: SiteUrl + '/Trainning/TrainningReject',
            data: JSON.stringify({

                "_Model": {
                    "TrainningId": $scope.SingleTrainningData.TrainningId,
                    "Remark": $scope.Trainning.Remark,
                    "IsAccepted": -1

                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                var k = data;

                var message = '';

                if (k == 1) {
                    alert("Trainning Rejected Sucessfully");
                    window.location = '/Trainning/DeptTrainningAcceptance';
                    $scope.GetTrainningAcceptUserList();
                }
                else {
                    alert("User Already Alloted");
                    //window.location = '/Trainning/Trainning';
                    //window.location = '/Trainning/DeptTrainningAcceptance';
                }
            },

        })


    };

    $scope.BackToPage = function () {
        window.location = "../Trainning/DeptTrainningAcceptance";
    }

    $scope.OnPageLoad();
    $scope.GetOrganisations();
    //$scope.GetTrainningAcceptUserList();
});


$(document).ready(function () {
    $("#ckbCheckAll").click(function () {

        $(".checkBoxClass").prop('checked', this.checked);
    });
    $('#TblUser tr').each(function () {
        if ($(this).find('td:eq(1)').find('checkbox:checked')) {
            SocietyRegistrationData.push(
                {
                    "PostId": $(this).find('td:eq(1)').find('checkbox:checked'),
                    "UserType": "4",//Elected board Menbers
                    "AdministratorDesignationId": "-1",
                    "Photo": $(this).find('td:eq(8)').find('label').html()
                });
        }
    });
});
