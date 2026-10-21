var ArrSelectSociety = new Array();

function SocietySelected() {
    this.SocietyIdEnc = '0';
}
var MasterMod = angular.module('ElectionModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ElectionCont", function ($scope, $http) {

    var ArrSelectSociety = new Array();
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.SocietyIDEnc = 0;
    $scope.ShowSavedData = false;
    $scope.ShowDisDivUnitAndDist = false;
    $scope.ShowDisDivZonal = false;
    $scope.Divisions = [];
    //$scope.SelectSociety = [];

    // on page load
    $scope.OnPageLoad = function () {
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
        }


        $scope.ClearUnitForm = function () {
            //$scope.UnitMaster = null;
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
        //START : Get Division List
        $scope.Divisions = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=division&MasterCode=-1").
            success(function (data, status) {
                $scope.Divisions = data;
            }).
            error(function (data, status) {
                $scope.Divisions = [];
            });
        //END
        $(".se-pre-con").fadeOut("slow");

    };

    // START :Get List of Societies

    // GET Society Type
    $scope.GetSocietyTypeList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=" + '').
            success(function (data, status) {
                $scope.SocietyTypeList = data;
            }).
            error(function (data, status) {
                $scope.SocietyTypeList = [];
            });
    };
    //END 

    $scope.ElectionForChange = function (id) {
        $scope.ElectionSociety.DIVISION_ID = '0';
        $scope.ElectionSociety.DistrictId = '0';
        $scope.ElectionSociety.CityTypes = '0';
        $scope.ElectionSociety.CITY_ID = '0';
        $scope.ElectionSociety.BlockId = '0';
        $scope.ElectionSociety.GP_ID = '0';
        $scope.SocietyListdataview = [];
        $scope.ShowWhenSearch = false;
        if (id == 1) {
            $scope.ShowDisDivUnitAndDist = false;
            $scope.ShowDisDivZonal = true;
        }
        else {
            $scope.ShowDisDivUnitAndDist = true;
            $scope.ShowDisDivZonal = false;
        }
    };
    $scope.RedirectTopublishOrImpoDate = function (Id) {
        debugger;
        var CurrentStatus = 0;
      //  window.location = '../Election/ElectionImportantDates?ElectionMasterId=' + CurrentStatus.split("~")[1]
        $http.get(SiteUrl + "/Election/ElecCurrentStatus?Id=" + Id).
            success(function (data, status) {
                debugger;
                CurrentStatus = data;
                //if (CurrentStatus.split("~")[0] == "1") {
                    window.location = '../Election/ElectionImportantDates?ElectionMasterId=' + CurrentStatus.split("~")[1]
                //}
                //else if (CurrentStatus.split("~")[0] == "2") {
                //  //  window.location = '../Election/Publish?Id=' + CurrentStatus.split("~")[1];
                //}
                //else if (CurrentStatus.split("~")[0] == "3") {
                //    alert('All Election Officers assigned to society.');
                //}
            }).
            error(function (data, status) {
                debugger;
                CurrentStatus = 0;
            });
    }

    $scope.SocityType = function (SocietyTypeId) {
        $('select').val('0');
        $scope.ElectionSociety.DistrictId = '0';
        $scope.ElectionSociety.CITY_ID = '0';
        $scope.ElectionSociety.BlockId = '0';
        $scope.ElectionSociety.GP_ID = '0';
        $scope.ElectionSociety.DIVISION_ID = '0';
        $scope.ElectionSociety.ElectionFor = 0;
        $scope.ShowDisDivUnitAndDist = false;
        $scope.ElectionSociety.SocietyTypeId = SocietyTypeId;
        if (SocietyTypeId == 7)    //Primary Type
        {
            $scope.ShowDisDiv = true;
            $scope.ShoRuralurban = true;
            $scope.ShowElectionForDiv = true;
            $scope.ShowblockDiv = false;
            $scope.ShowWhenSearch = false;
        }
        else if (SocietyTypeId == 8) {     //Central type
            $scope.ShowblockDiv = true;
            $scope.ShowDisDivZonal = false;
            $scope.ShowElectionForDiv = false;
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
            $scope.DisDiv = false;
            $scope.ShowDisDiv = true;
            $scope.ShoRuralurban = false;
            $scope.ShowWhenSearch = false;
        }
        else {
            $scope.ShowDisDiv = false;
            $scope.ShowElectionForDiv = false;
            $scope.ShowblockDiv = false;
            $scope.ShowWhenSearchbtn(); $scope.GetSocietyList(9);
        }
    }
    $scope.GetElectionOrderList = function () {
        // alert(2344);
        // alert(JSON.stringify( $scope.ElectionSociety));
        $http.post("../Election/GetElectionOrderList").
            success(function (data, status) {
                $scope.ElectionOrderData = data;

                $scope.ElectionOrderData.forEach(function (item, index) {
                    item.IsElectionOrderPDFSign = item.IsElectionOrderPDFSign.toLowerCase() == 'true' ? true : false;
                });

            }).
            error(function (data, status) {
                $scope.SocietyListdataview = [];
                alert('Error while fetching society list data.');
            });
    };
    $scope.SetDivisionROfficer = function () {
        // LevelId = 1 for Apex , 2 fro Central , 3 for Primary
        $http.post("../User/GetReturningOfficer", { "LevelId": "2", "PlaceID": $scope.ElectionSociety.DIVISION_ID}).
            success(function (data, status) {
                $scope.DivisionROfficer = data;

            }).
            error(function (data, status) {
                $scope.DivisionROfficer = [];
                alert('Error while fetching Division Returing officer list data.');
            });
    };

    $scope.SetDistROfficer = function () {
        // LevelId = 1 for Apex , 2 fro Central , 3 for Primary
        $(".se-pre-con").fadeIn("slow");
        $http.post("../User/GetReturningOfficer", { "LevelId": "3", "PlaceID": $scope.ElectionSociety.DistrictId }).
            success(function (data, status) {
                $scope.DistROfficer = data;
                $(".se-pre-con").fadeOut("slow");

            }).
            error(function (data, status) {
                $scope.DistROfficer = [];
                alert('Error while fetching District Returning Officer list data.');
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.GetSocietyList = function (id) {
        if (id != 9) {
            if ($scope.ElectionSociety.ElectionFor == '1' || $scope.ElectionSociety.ElectionFor == '2') {
                if ($scope.ElectionSociety.DistrictId != '0') {
                }
                else if ($scope.ElectionSociety.DIVISION_ID != '0') {
                }
                else {
                    alert("Please select division or district.")
                    return;
                }
            }
        }
        $(".se-pre-con").fadeIn("slow");
        // alert(JSON.stringify( $scope.ElectionSociety));
        $scope.ElectionSociety.TypeId = id
        $http.post("../Election/GetSocietyListElection", $scope.ElectionSociety).
            success(function (data, status) {
                $scope.SocietyListdataview = data;
                $(".se-pre-con").fadeOut("slow");

            }).
            error(function (data, status) {
                $scope.SocietyListdataview = [];
                $(".se-pre-con").fadeOut("slow");
                alert('Error while fetching society list data.');
            });
    };

    $scope.GetDistrictList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    }

    $scope.BlockList = function () {
        $scope.BlocksList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.ElectionSociety.DistrictId).
            success(function (data, status) {
                $scope.BlocksList = data;
            }).
            error(function (data, status) {
                $scope.BlocksList = [];
            });

    }

    $scope.GramPanchayatList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.ElectionSociety.BlockId).
            success(function (data, status) {
                $scope.GramPanchayatList = data;
            }).
            error(function (data, status) {
                $scope.GramPanchayatList = [];
            });
    }
    // END :Get List of Societies

    $scope.RuralUrbanBlock = function () {
        $scope.ElectionSociety.DistrictId = '0';
        $scope.ElectionSociety.CityTypes = '0';
        $scope.ElectionSociety.CITY_ID = '0';
        $scope.ElectionSociety.BlockId = '0';
        $scope.ElectionSociety.GP_ID = '0';
        $scope.SocietyListdataview = [];
        $scope.ShowWhenSearch = false;
        var SelectedVal = $scope.ElectionSociety.IsRural;
        if (SelectedVal == 'True' || SelectedVal == 'true') {
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;
            $scope.DisDiv = true;

        }
        else if (SelectedVal == 'false' || SelectedVal == 'False') {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
            $scope.DisDiv = true;
        }
        else {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
            $scope.DisDiv = false;
        }
    };


    $scope.GetCityListByCityTypes = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode=" + $scope.ElectionSociety.CityTypes + "&MasterCode2=" + $scope.ElectionSociety.DistrictId).
            success(function (data, status) {
                $scope.CityListdata = data;
            }).
            error(function (data, status) {
                $scope.CityListdata = [];
            });
    }

    $scope.SaveElectionMaster = function () {
        if ($scope.ElectionSociety.ElectionFor == '1' || $scope.ElectionSociety.ElectionFor == '2') {
            if ($scope.ElectionSociety.DistrictId != '0') {
            }
            else if ($scope.ElectionSociety.DIVISION_ID != '0') {
            }
            else {
                alert("Please select division or district.")
                return;
            }
            if ($scope.ElectionSociety.ReturningOfficerID != '0') {
            }
            else {
                alert("Please select Returning Officer.")
                return;
            }
        }
        if ($scope.ElectionSociety.StepInElection == 0)
        {
            alert("Please select steps in election.")
            return;
        }
       
        if ($('input:checkbox').prop('checked') == true || ($('input:checkbox.chk').prop('checked') == true) || (JSON.stringify(ArrSelectSociety)).length > 50) {
            $(".se-pre-con").fadeIn("slow");
            $.ajax({
                url: '../Election/SaveElectionMasterdata',
                data: JSON.stringify({
                    "objElectionSocietyData": {
                        "SocityIdlst": ArrSelectSociety, "ReturningOfficerID": $scope.ElectionSociety.ReturningOfficerID, "DIVISION_ID": $scope.ElectionSociety.DIVISION_ID,
                        "DistrictId": $scope.ElectionSociety.DistrictId, "ElectionFor": $scope.ElectionSociety.ElectionFor, "StepInElection": $scope.ElectionSociety.StepInElection,
                        "SocietyTypeId":$scope.ElectionSociety.SocietyTypeId
                    }
                }),
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                beforeSend: function () {
                },
                success: function (data) {
                    $(".se-pre-con").fadeOut("slow");
                    if ((data) != "-1") {
                        alert("Save Sucessfully");
                        window.location = '/Election/ElectionImportantDates?ElectionMasterId=' + data;
                    }
                    else {
                        alert("Error While Data Insert");
                        window.location = '../Election/ElectionSocietyList';
                    }
                    $scope.MasterId = data;
                },

            })
        }
        else
            alert('Please Select The Societies');
    };

      $scope.Chkdsociety = function (item, SocietyIdENC) {
        var _SelectedSocietyForElection = new SocietySelected();
        _SelectedSocietyForElection.SocietyIdEnc = SocietyIdENC;     
        if (item.chkdboxValue) {
            ArrSelectSociety.push(_SelectedSocietyForElection);
        }
        else {
            ArrSelectSociety.pop(_SelectedSocietyForElection);
        }
        console.log(ArrSelectSociety);
    }

    $scope.allChkdsociety = function () {
        // alert($('input:checkbox').prop('disabled') == true);
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

    $scope.ShowWhenSearchbtn = function () {
        // if ($scope.ElectionSociety.IsShowNewOld)
        $scope.ShowWhenSearch = true;
        // else
        //   $scope.ShowWhenSearch = false;

    }
    $scope.FunShowGrid = function (val) {
        if (val)
            $scope.ShowSavedData = true;
        else
            $scope.ShowSavedData = false;

    }

    $scope.OnPageLoad();
    //   $scope.GetSocietyList();
    $scope.GetElectionOrderList();
    $scope.GetDistrictList();
});






