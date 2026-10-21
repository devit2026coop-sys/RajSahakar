

var ArrSocietyElectionOfficer = new Array();

function SocietyElectionOfficer() {
    this.ElectionMasterId = 0;
    this.SocietyId = 0;
    this.OfficerId = 0;
}
var MasterMod = angular.module('PublishMOD', ['angularUtils.directives.dirPagination']);
MasterMod.controller("PublishCONT", function ($scope, $http) {

    $scope.StepInElection = 0;
    var ArrSelectSociety = new Array();
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1; 
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.OrderId = '';
    //$scope.SelectSociety = [];

    // on page load
    $scope.OnPageLoad = function () {
        var url = window.location.search;
        $scope.OrderId = (url.split('Id')[1]);
        $scope.OrderId = $scope.OrderId.substring(1, $scope.OrderId.length);
        $(".se-pre-con").fadeIn("slow");
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
            $(".se-pre-con").fadeOut("slow");
        }


        $scope.ClearUnitForm = function () {
            $scope.UnitMaster.DepartmentId = 0;
            $scope.UnitMaster.UnitNameEn = '';
            $scope.UnitMaster.UnitNameHi = '';
            $scope.UnitMaster.IsActive = false;
            $scope.UnitMaster.UnitId = 0;
        };

        $scope.Department = [];
        $http.get("../Master/GetMasterData/?MasterType=Department&MasterCode=-1").
            success(function (data, status) {

                $scope.Department = data;
            }).
            error(function (data, status) {
                $scope.Department = [];
            });

    };

    // START :Get List of Societies

    // GET Society Type
    $scope.GetSocietyTypeList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=" + '').
            success(function (data, status) {
                $scope.SocietyTypeList = data;
            }).
            error(function (data, status) {
                $scope.SocietyTypeList = data;
            });
    };
    //END    
    $scope.GetSocietyList = function () {
        $http.post("../Election/GetSocietyListByOrderID/?OredrId=" + $scope.OrderId).
            success(function (data, status) {
                $scope.SocietyListdataview = data;
                $scope.GetElectionQuestionList();
               // $scope.MyDefaultValue = "1002";
                //$scope.SocietyList = $scope.SocietyListdataview;
                //var newdab = data.filter(x => x.StratDate == '12/31/9999' ? '' : StratDate)
                //alert(newdab);
            }).
            error(function (data, status) {
                $scope.SocietyListdataview = [];
                alert('Error while fetching society list data.');
            });
    };

    $scope.GetSocietyListByElectionOffice = function () {
        $http.post("../Election/ElectionOfficerForSociety/?OredrId=" + $scope.OrderId).
            success(function (data, status) {
                $scope.SocietyListElectionOfficer = data;
                $scope.GetSocietyList();
                //$scope.SocietyList = $scope.SocietyListdataview;
                //var newdab = data.filter(x => x.StratDate == '12/31/9999' ? '' : StratDate)
                //alert(newdab);
            }).
            error(function (data, status) {
                $scope.SocietyListElectionOfficer = [];
                alert('Error while fetching society list data.');
            });
    };

    $scope.GetElectionQuestionList = function () {
        $http.post("../Election/GetElectionQuestion/?OredrId=" + $scope.OrderId).
            success(function (data, status) {
                $scope.ElectionQuestionData = data;

                if (data != null && data.length > 0) {
                    $scope.StepInElection = data[0].StepInElection;
                }
            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                alert('Error while fetching society list data.');
            });
    };



    // END :Get List of Societies
    $scope.SavePublishData = function () {
        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();
        DataArray = [];
        var RemainingElectionOfficer = 0;
        $('#tblPublish tr').each(function () {
            if ($(this).find('td:eq(2)').find('select').val() > 0) {
                var objSocietyElectionOfficerList = new SocietyElectionOfficer();
                objSocietyElectionOfficerList.ElectionMasterIdEnc = $scope.OrderId;
                objSocietyElectionOfficerList.SocietyId = $(this).find('td:eq(1)').find('input').val();
                objSocietyElectionOfficerList.OfficerId = $(this).find('td:eq(2)').find('select').val();
                ArrSocietyElectionOfficer.push(objSocietyElectionOfficerList);
            }
            else {
                RemainingElectionOfficer = RemainingElectionOfficer + 1;
            }
        });
        //debugger        
        //$.ajax({
        //    url: '../../Election/InsertSocietyElectionOfficer',
        //    data: JSON.stringify({
        //        "ObjListSocietyElecOffi": {
        //            "DataArray": ArrSocietyElectionOfficer,
        //        }
        //    }),
        //    type: 'POST',
        //    contentType: 'application/json',
        //    dataType: 'json',
        //    beforeSend: function () {
        //    },
        //    success: function (data) {              
        //    },
        //})
        if (RemainingElectionOfficer==0){
        $.ajax({
            url: '../Election/InsertSocietyElectionOfficer',
            data: JSON.stringify({
                "ObjListSocietyElecOffi": {
                    "DataArray": ArrSocietyElectionOfficer,
                }
            }),
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
            },
            success: function (data) {
                if ((data.length) > 0) {
                    alert("Save Sucessfully");
                    window.location = '../Election/ElectionSocietyList';
                }
                else {
                    alert("Error While Data Insert");
                  //  window.location = '/Election/ElectionSocietyList';
                }
                $(".se-pre-con").fadeOut("slow");
            },
            error: function (e) {
                message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                alert(message);
                $(".se-pre-con").fadeOut("slow");
            },
            complete: function () {
                $(".se-pre-con").fadeOut("slow");
            }

            })
        }
        else {
            alert("Please select election officer for every society in order.");
        }


    };

    $scope.Chkdsociety = function (SocietyIdENC) {
        var _SelectedSocietyForElection = new SocietySelected();
        _SelectedSocietyForElection.SocietyIdEnc = SocietyIdENC;
        ArrSelectSociety.push(_SelectedSocietyForElection);
    }

    $scope.allChkdsociety = function () {
        angular.forEach($scope.SocietyListdataview, function (value, index) {
            var _SelectedSocietyForElection = new SocietySelected();
            _SelectedSocietyForElection.SocietyIdEnc = value.SocietyIdENC;
            if ($('#select-all').prop('checked') == true) {
                ArrSelectSociety.push(_SelectedSocietyForElection);
            }
            else {
                ArrSelectSociety = [];
            }
        })
    }

    $scope.OnPageLoad();
    $scope.GetSocietyListByElectionOffice();

    //$scope.SaveTrainning = function () {
    //    $.ajax({
    //        url: '../Trainning/SaveTrainning',
    //        data: JSON.stringify({
    //            "_Model": {
    //                "lstTrainningUsers": ArrSelectTraningUser,
    //            }
    //        }),
    //        type: 'POST',
    //        contentType: 'application/json',
    //        dataType: 'json',
    //        beforeSend: function () {
    //        },
    //        success: function (data) {
    //            if ((data.length) > 0) {
    //                alert("Save Sucessfully");
    //                window.location = '/Trainning/Trainning';
    //            }
    //            else {
    //                alert("Error While Data Insert");
    //                window.location = '/Trainning/Trainning';
    //            }
    //        },
    //    })
    //};


    //$scope.ChkdtraningUsr = function (UserId) {
    //    var _SelectedChkdtraningUsr = new TraningUserSelected();
    //    _SelectedChkdtraningUsr.UserId = UserId;
    //    ArrSelectTraningUser.push(_SelectedChkdtraningUsr);
    //}

});






