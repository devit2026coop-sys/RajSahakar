var ArrSelectSociety = new Array();

function SocietySelected() {
    this.SocietyIdEnc = '0';
}
var MasterMod = angular.module('ElectionModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("ElectionCont", function ($rootScope, $scope, $http, $sce) {
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

    $scope.ShowSelectedSocieties={
        IsChecked : false
    }

    $scope.ShowSelectedSocieties1 = {
        IsChecked: false
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
        $("#SearchText").val("");
        $scope.DataPaging.currentPageID = 1;
        $scope.DataPaging.searchParameters = [];
        $scope.searchRequest($scope.GetSocietyDetailsList);
    }

  
    /*------------End Paging and Filter-----------------*/
    var ArrSelectSociety = new Array();
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 30;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.SocietyIDEnc = 0;
    $scope.ShowSavedData = false;
    $scope.ShowDisDivUnitAndDist = false;
    $scope.ShowDisDivZonal = false;
    $scope.Divisions = [];
    //$scope.SelectSociety = [];



    $scope.SocietyTypeList = [];
    $scope.GetSocietyTypeList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=SOCIETYTYPE&MasterCode=" + '').
            success(function (data, status) {
                $scope.SocietyTypeList = data;
                //$scope.ElectionSociety.SocietyTypeId = "0";
            }).
            error(function (data, status) {
                $scope.SocietyTypeList = [];
            });
    };

    // on page load
    $scope.OnPageLoad = function () {
        //$(".se-pre-con").fadeIn("slow");


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
        //$(".se-pre-con").fadeOut("slow");
        //$scope.GetSocietyTypeList();
    };

    // START :Get List of Societies

    // GET Society Type





    //END 

    $scope.ElectionForChange = function (id) {


        $scope.ElectionSociety.CityTypes = "0";
        $scope.ElectionSociety.CITY_ID = "0";
        $scope.ElectionSociety.WARD_ID = "0";
        $scope.ElectionSociety.BlockId = "0";
        $scope.ElectionSociety.GP_ID = "0";
        $scope.ElectionSociety.VillageId = "0";
        $scope.ElectionSociety.DIVISION_ID = '0';
        $scope.ElectionSociety.DistrictId = '0';

        $scope.ElectionSociety.SocietyCategoryId = '0';

        $scope.GetDistrictList_change();
        $scope.Ward = [];
        $scope.CityListdata = [];
        $scope.Blocksdata = [];
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
      
        var CurrentStatus = 0;
        //  window.location = '../Election/ElectionImportantDates?ElectionMasterId=' + CurrentStatus.split("~")[1]
        $http.get(SiteUrl + "/Election/ElecCurrentStatus?Id=" + Id).
            success(function (data, status) {
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
             
                CurrentStatus = 0;
            });
    }

    $scope.ElectionForList = [{ ElectionForId: "1", ElectionForEng: "Election done Before 5 year", ElectionForHin: "चुनाव 5 साल से पहले किए गए" }, { ElectionForId: "1", ElectionForEng: "Election Due More than 5 years", ElectionForHin: "चुनाव हुए 5 साल से अधिक" }, { ElectionForId: "2", ElectionForEng: "Administrator Appointed", ElectionForHin: "प्रशासक नियुक्त" }, { ElectionForId: "3", ElectionForEng: "Request by CEO ", ElectionForHin: "CEO द्वारा अनुरोध" }, { ElectionForId: 4, ElectionForEng: "Casual Vacancy Election", ElectionForHin: "आकस्मिक चुनाव " }];
    $scope.ElectionSocietyTypeId = 0;
    $scope.SocityType = function (SocietyTypeId) {
        $('select').val('0');
    
        $scope.ElectionSociety.DistrictId = '0';
        $scope.ElectionSociety.CITY_ID = '0';
        $scope.ElectionSociety.BlockId = '0';
        $scope.ElectionSociety.GP_ID = '0';
        $scope.ElectionSociety.DIVISION_ID = '0';
        //$scope.ElectionSociety.ElectionFor = 0;
        $scope.ShowDisDivUnitAndDist = false;
        $scope.ElectionSocietyTypeId = SocietyTypeId;
        $scope.GetSocietyCategory($scope.ElectionSocietyTypeId);
        ////////////////////$scope.ElectionSociety.SocietyTypeId = SocietyTypeId;
        if (SocietyTypeId == 7)    //Primary Type
        {
            $scope.ShowDisDiv = true;
            $scope.ShoRuralurban = true;
            $scope.ShowElectionForDiv = true;
            $scope.ShowblockDiv = false;
            $scope.ShowWhenSearch = false;
            if (!$scope.IsEdit) {
                $scope.ElectionSociety.ElectionFor = 2;
                $scope.ElectionForChange(2);
            }
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
            if (!$scope.IsEdit) {
                $scope.ElectionSociety.ElectionFor = 1;
                $scope.ElectionForChange(1);
            }
        }
        else {
            $scope.ShowDisDiv = false;
            $scope.ShowElectionForDiv = false;
            $scope.ShowblockDiv = false;
            $scope.ShowWhenSearchbtn(); $scope.GetSocietyList(9);
        }
    }
 
    $scope.SetDivisionROfficer = function () {
       
        // LevelId = 1 for Apex , 2 fro Central , 3 for Primary
        $(".se-pre-con").fadeIn("slow");
        $http.post("../User/GetReturningOfficer", { "LevelId": "2", "PlaceID": $scope.ElectionSociety.DIVISION_ID }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.DivisionROfficer = data;

            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
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
    $scope.GetGetSocietyListType = 0;
    $scope.GetSocietyList = function (id) {
       
        $scope.GetGetSocietyListType = id;
        $scope.ArrSelectSociety = [];
        $scope.searchRequest($scope.GetSocietyDetailsList);

    }

    //bhupendra

    $scope.GetDistrictList_change = function () {
        $scope.DistrictList = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=" + $scope.ElectionSociety.DIVISION_ID).
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
                alert('Error while fetching district list data.');
            });
    };

    // START : Get Ward List
    $scope.GetWardList = function (id) {
      
        if ($scope.ElectionSociety.CITY_ID != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD&MasterCode=" + $scope.ElectionSociety.CITY_ID).
                success(function (data, status) {
                    $scope.Ward = data;
                }).
                error(function (data, status) {
                    $scope.Ward = [];
                });
        }
    };
   

    // START : Get SocietyCategoryList
    $scope.GetSocietyCategory = function (SocietyTypeId) {

        $http.get(SiteUrl + "/Master/SocietyCategoryList/?ClassId=" + '' + "&SubClassId=" + '' + "&SocietyType=" + SocietyTypeId).
            success(function (data, status) {
                $scope.SocietyCategoryList = data;

                $scope.SocietyCategoryList.push({
                    CategoryId: -1, SocietyClassId: -1, CategoryNameEn: 'Under Rule 46-A', CategoryNameHi: 'Under Rule 46-A'
                });

            }).
            error(function (data, status) {
                $scope.SocietyCategoryList = [];
            });
    };
    // END : Get Sub Class
    

    $scope.RuralUrbanBlock_change = function (V) {
       
        var SelectedVal = $scope.ElectionSociety.IsRural;
        if (V != 1) {
            $scope.ElectionSociety.CityTypes = "0";
            $scope.ElectionSociety.CITY_ID = "0";
            $scope.ElectionSociety.WARD_ID = "0";
            $scope.ElectionSociety.BlockId = "0";
            $scope.ElectionSociety.GP_ID = "0";
            $scope.ElectionSociety.VillageId = "0";
        }
        if (SelectedVal == 'True' || SelectedVal == 'true') {
            $scope.UrbanDiv = true;
            $scope.RuralDiv = false;

            $scope.GetCityListByCityTypes(1);
        }
        else if (SelectedVal == 'false' || SelectedVal == 'False') {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = true;
            $scope.BlockListByOffAdd(1)
        }
        else {
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
        }
    };
    $scope.BlockListByOffAdd = function (id) {
      
        $scope.Blocksdata = [];
        if ($scope.ElectionSociety.DistrictId != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY&MasterCode=" + $scope.ElectionSociety.DistrictId).
                success(function (data, status) {
                    $scope.Blocksdata = data;
                }).
                error(function (data, status) {
                    $scope.Blocks = [];
                });
        }
    };


    // START : Get List of GramPanchayat
    $scope.GramPanchayatList = function (id) {
        if ($scope.ElectionSociety.BlockId != '0' && id == 1) {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT&MasterCode=" + $scope.ElectionSociety.BlockId).
                success(function (data, status) {

                    $scope.GramPanchayats = data;
                }).
                error(function (data, status) {
                    $scope.GramPanchayats = [];

                });
        }


    };
    // END : Get List of GramPanchayat


    $scope.VillageList = function () {
            $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=Village&MasterCode=" + $scope.ElectionSociety.GP_ID).
                success(function (data, status) {
                    $scope.Villages = data;
                }).
                error(function (data, status) {
                    $scope.Villages = [];
                });
       
    };
   

    //bhupendra

    $scope.GetSocietyDetailsList = function () {
        var id = $scope.GetGetSocietyListType
        if (id == 7 && ($scope.ElectionSociety.ElectionFor == "0" || $scope.ElectionSociety.ElectionFor == undefined)) {
            alert("Please select " + $scope.translation.GenerateElectionFor + ".")
        } else if (id == 7 && $scope.ElectionSociety.ElectionFor == "1" && $scope.ElectionSociety.DIVISION_ID == "0") {
            alert("Please select " + $scope.translation.DivisionHeading + ".")
        } else if (id == 7 && $scope.ElectionSociety.DistrictId == "0" && ($scope.ElectionSociety.ElectionFor == "2" || $scope.ElectionSociety.ElectionFor == "3")) {
            alert("Please select " + $scope.translation.District + ".")
        } else if (id == 8 && $scope.ElectionSociety.DIVISION_ID == '0') {
            alert("Please select " + $scope.translation.DivisionHeading + ".")
        } else if (id != 9 && $scope.ElectionSociety.ElectionForId == "0") {
            alert("Please select " + $scope.translation.ElectionForSocietes + "")
        } else {
            //ElectionSociety.ElectionForId
            $(".se-pre-con").fadeIn("slow");
            // alert(JSON.stringify( $scope.ElectionSociety));
            $scope.ElectionSociety.TypeId = id
            $scope.ElectionSociety.SocietyTypeId = $scope.ElectionSocietyTypeId
            var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters };
            return $http.post(SiteUrl + "/Election/GetSocietyListElection", JSON.stringify({ Details: jsonParameters, objElectionSocietyData: $scope.ElectionSociety })).
                //$http.post("../Election/GetSocietyListElection", $scope.ElectionSociety).
                success(function (data, status) {
                    //$(".se-pre-con").fadeOut("slow");
                    $scope.ShowWhenSearchbtn();
                    //$scope.SocietyListdataview = data;
                    $scope.SocietyListdataview = JSON.parse(data.DataList);
                    $rootScope.DataPagingDetails = data.DataPaging;
                    $rootScope.SetDataPaging();
                    $(".blockUI").fadeOut("slow");

                    //ArrSelectSociety.forEach(function (item, index) {

                    //    data.forEach(function (item, indx) {
                    //        item.chkdboxValue = true;
                    //        $scope.Chkdsociety(item, item.SocietyIdENC)
                    //    });

                    //});

                    $scope.SocietyListdataview.forEach(function (item, indx) {
                        if ($scope.ArrSelectSociety.filter(x => x.SocietyID === item.SocietyID).length > 0) {
                            item.chkdboxValue = true;
                        } else {
                            item.chkdboxValue = false;
                        }
                    });
                   


                }).
                error(function (data, status) {
                    $scope.SocietyListdataview = [];
                    $(".se-pre-con").fadeOut("slow");
                    alert('Error while fetching society list data.');
                });
        }
    };






    //bhupendra fillter
    $scope.filtterId = '0';
    $scope.datewise = false;
    $scope.datewise1 = false;
    $scope.Statuswise = false;
    var fromdate = '';
    var todate = '';
    $scope.StatusId = '0';
    var date = '';

    $scope.ClearSearch_new = function () {
         fromdate = '';
         todate = '';
        $scope.StatusId = '0';
         date = '';
        $scope.searchRequest($scope.GetElectionOrderList);
    }

    $scope.FillterType = function () {

        if ($scope.filtterId == '1') {
            $scope.datewise = true;
            $scope.datewise1 = true;
            $scope.Statuswise = false;
            $scope.btnstatus = true;

            $scope.StatusId = '0';
            $('#ComplaintDate2').val('');
        }
        else if ($scope.filtterId == '2') {
            $scope.Statuswise = true;
            $scope.datewise = false;
            $scope.datewise1 = true;
            $scope.btnstatus = true;
        }
        else {
            $scope.datewise1 = false;
            $scope.btnstatus = false;
            $scope.Statuswise = false;
            $scope.datewise = false;
        }
     
    }


    $scope.Searchbyfillter = function () {
         fromdate = $('#ComplaintDate').val();
        todate = $('#ComplaintDate1').val();
        date = $('#ComplaintDate2').val();
        $scope.GetElectionOrderList(fromdate, todate);
       
    }


    $scope.GetElectionOrderList = function (fromdate, todate) {

     
        //,"statusid":0,"DateWise":''
        // alert(2344);
        // alert(JSON.stringify( $scope.ElectionSociety));
        $(".se-pre-con").fadeIn("slow");
        $http.post("../Election/GetElectionOrderList", { "FormDate": fromdate, "Todate": todate, "StatusId": $scope.StatusId, "Datewise": date }).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
              
                $scope.ElectionOrderData = data;
                $scope.ElectionOrderData.forEach(function (item, i) {
                    item.SocietiesName = $sce.trustAsHtml(item.SocietiesName);
                })
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.SocietyListdataview = [];
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
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITYLISTBY&MasterCode=" + $scope.ElectionSociety.CityTypes + "&MasterCode2=" + $scope.ElectionSociety.DistrictId).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.CityListdata = data;
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
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
                ShowOkMessage("Warning", "Please select division or district.", function () { });
                return;
            }
            if ($scope.ElectionSociety.ReturningOfficerID != '0') {
            }
            else {
                ShowOkMessage("Warning", "Please select Returning Officer.", function () { });
                return;
            }
        }
        if ($scope.ElectionSociety.StepInElection == 0) {
            ShowOkMessage("Warning", "Please select steps in election.", function () { });
            return;
        }



        if ($('input:checkbox').prop('checked') == true || ($('input:checkbox.chk').prop('checked') == true) || (JSON.stringify($scope.ArrSelectSociety)).length > 50) {
            var electionData = {
                "SocityIdlst": $scope.ArrSelectSociety, "ReturningOfficerID": $scope.ElectionSociety.ReturningOfficerID, "DIVISION_ID": $scope.ElectionSociety.DIVISION_ID,
                "DistrictId": $scope.ElectionSociety.DistrictId, "ElectionFor": $scope.ElectionSociety.ElectionFor, "StepInElection": $scope.ElectionSociety.StepInElection,
                "SocietyTypeId": $scope.ElectionSociety.SocietyTypeId, "ElectionMasterId": $scope.ElectionMasterId
            };

            //$scope._language == 'en-us' ? "You have selected only one society for ' + $scope.ElectionSociety.StepInElection+' election phase, only one phase is allowed for one society. Please click on the 'OK' button to conduct the election in one phase." : 'आपने ' + $scope.ElectionSociety.StepInElection+' चुनाव चरणों के लिए केवल एक समाज का चयन किया है, एक समाज के लिए केवल एक चरण की अनुमति है। कृपया एक चरण में  चुनाव  कराने के लिए "Yes" बटन पर क्लिक करें ।'
            if ($scope.ArrSelectSociety.length == 1 && $scope.ElectionSociety.StepInElection > 1) {
                ShowAlertMessage("Warning", $scope._language == 'en-us' ? "You have selected only one society for " + $scope.ElectionSociety.StepInElection + " election phase, only one phase is allowed for one society. Please click on the 'OK' button to conduct the election in one phase." : 'आपने ' + $scope.ElectionSociety.StepInElection + ' चुनाव चरणों के लिए केवल एक समाज का चयन किया है, एक समाज के लिए केवल एक चरण की अनुमति है। कृपया एक चरण में  चुनाव  कराने के लिए "Yes" बटन पर क्लिक करें ।', function () {
                    electionData.StepInElection = 1;
                    $(".se-pre-con").fadeIn("slow");
                    $.ajax({
                        url: '../Election/SaveElectionMasterdata',
                        data: JSON.stringify({ objElectionSocietyData: electionData }),
                        type: 'POST',
                        contentType: 'application/json',
                        dataType: 'json',
                        beforeSend: function () {
                        },
                        success: function (data) {
                            $(".se-pre-con").fadeOut("slow");
                            if ((data) != "-1") {
                                window.location = '/Election/ElectionImportantDates?ElectionMasterId=' + data;
                            }
                            else {
                                ShowOkMessage("Error", "Error while processing request", function () { });
                            }
                            $scope.MasterId = data;
                        },

                    });
                });
            } else {
                $(".se-pre-con").fadeIn("slow");
                $.ajax({
                    url: '../Election/SaveElectionMasterdata',
                    data: JSON.stringify({ objElectionSocietyData: electionData }),
                    type: 'POST',
                    contentType: 'application/json',
                    dataType: 'json',
                    beforeSend: function () {
                    },
                    success: function (data) {
                        $(".se-pre-con").fadeOut("slow");
                        if ((data) != "-1") {
                            window.location = '/Election/ElectionImportantDates?ElectionMasterId=' + data;
                        }
                        else {
                            ShowOkMessage("Error", "Error while processing request", function () { });
                        }
                        $scope.MasterId = data;
                    },

                });
            }
        }
        else {
            ShowOkMessage("Warning", "Please select at least one society for process.", function () { });
        }

    };


   
        
    $scope.ArrSelectSociety = [];

    $scope.Chkdsociety = function (item, SocietyIdENC) {
        var _SelectedSocietyForElection = new SocietySelected();
        _SelectedSocietyForElection.SocietyIdEnc = SocietyIdENC;
        _SelectedSocietyForElection.SocietyID = item.SocietyID;   
        if (item.chkdboxValue) {
            $scope.ArrSelectSociety.push(item);
        }
        else {

            var data = $scope.ArrSelectSociety;
            $scope.ArrSelectSociety = [];
            data.forEach(function (itm, idx) {
                if (itm.SocietyID != item.SocietyID)
                {
                    $scope.ArrSelectSociety.push(itm);
                }
                
            });
            //var index = ArrSelectSociety.indexOf(_SelectedSocietyForElection);
            //ArrSelectSociety.splice(index, 1);
         
        }
        console.log($scope.ArrSelectSociety);
    }

    $scope.allChkdsociety = function () {
      
        // alert($('input:checkbox').prop('disabled') == true);
        angular.forEach($scope.SocietyListdataview, function (value, index) {
            var _SelectedSocietyForElection = new SocietySelected();
            _SelectedSocietyForElection.SocietyIdEnc = value.SocietyIdENC;
            _SelectedSocietyForElection.SocietyID = item.SocietyID;  
            if ($('#select-all').prop('checked') == true) {
                $scope.ArrSelectSociety.push(value);
            }
            else {
                $scope.ArrSelectSociety = [];
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
        if (val) {
            $scope.ShowSavedData = true;
        }
        else {
            //$scope.ShowSavedData = false;
            window.location = "../Election/ElectionSocietyList"
        }


    }

    $scope.EditData = [];
    $scope.IsEdit = false;
    $scope.EditElection = function (electionId) {
        $scope.ElectionMasterId = electionId;
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Election/GetElectionForEdit?ElectionMasterId=" + electionId).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.IsEdit = true;
                //SocietyTypeId
                $scope.ElectionSociety.ElectionFor = data[0].ElectionFor;
                $scope.SocityType(data[0].SocietyTypeId);

                $scope.ElectionSociety.StepInElection = data[0].StepInElection;
                $scope.ElectionForChange(data[0].ElectionFor);

                if ($scope.ElectionSocietyTypeId == 8) {
                    $scope.ElectionSociety.DIVISION_ID = data[0].DIVISION_ID.toString();
                    $scope.SetDivisionROfficer();
                    $scope.ElectionSociety.ReturningOfficerID = data[0].ReturningOfficerID.toString();

                } else if ($scope.ElectionSocietyTypeId == 7 && $scope.ElectionSociety.ElectionFor == 1) {
                    $scope.ElectionSociety.DIVISION_ID = data[0].DIVISION_ID.toString();
                    $scope.SetDivisionROfficer();
                    $scope.ElectionSociety.ReturningOfficerID = data[0].ReturningOfficerID.toString();
                } else if ($scope.ElectionSocietyTypeId == 7 && $scope.ElectionSociety.ElectionFor == 2) {
                    $scope.ElectionSociety.DistrictId = data[0].DistrictId.toString();
                    $scope.SetDistROfficer()
                    $scope.ElectionSociety.ReturningOfficerID = data[0].ReturningOfficerID.toString();
                } else if ($scope.ElectionSocietyTypeId == 7 && $scope.ElectionSociety.ElectionFor == 3) {
                    $scope.ElectionSociety.DistrictId = data[0].DistrictId.toString();
                    $scope.SetDistROfficer()
                    $scope.ElectionSociety.ReturningOfficerID = data[0].ReturningOfficerID.toString();
                }
                $scope.ElectionSociety.ElectionFor = data[0].ElectionFor;
                $scope.SocietyListdataview = data;

                $scope.ShowWhenSearchbtn();

                data.forEach(function (item, indx) {
                    item.chkdboxValue = true;
                    $scope.Chkdsociety(item, item.SocietyIdENC)
                });

                //Chkdsociety(Class,Class.SocietyIdENC)



                $scope.EditData = data;
                $scope.CityListdata = data;
                $scope.ShowSavedData = true;
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.CityListdata = [];
            });

    }

    $scope.GetSocietylistByElection = function (Electionid) {

        $http.post("/Election/GetSocietylistByElecionId?OredrId=" + Electionid).
           success(function (data, status) {
             
               $scope.SocietyListByElection = data;


              // $scope.ArrSelectSociety1 = $scope.SocietyListByElection;
             
            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                alert('Error while Society Election Detail data.');
            });
    };



    $scope.ArrSelectSociety1 = [];
    $scope.ChkdsocietyById = function (item, SocietyId, id) {
     
        var _SelectedSocietyForElection1 = new SocietySelected();
        _SelectedSocietyForElection1.SocietyId = item.SocietyId1;
        if (item.SocietyId) {
            $scope.ArrSelectSociety1.push(item);
        }
        else {

            var data = $scope.ArrSelectSociety1
            $scope.ArrSelectSociety1 = [];
            data.forEach(function (itm, idx) {
                if (itm.SocietyID != item.SocietyID) {
                    $scope.ArrSelectSociety1.push(itm);
                }
            });
        }
    }

    $scope.allChkdsocietyList = function () {
      
        $scope.ArrSelectSociety1 = [];
        // alert($('input:checkbox').prop('disabled') == true);
        angular.forEach($scope.SocietyListByElection, function (value, index) {
            var _SelectedSocietyForElection12 = new SocietySelected();
            _SelectedSocietyForElection12.SocietyId = value.SocietyId;
            if ($('#select-all-Society').prop('checked') == true) {
                $scope.ArrSelectSociety1.push(value);
                $scope.ShowSelectedSocieties1.IsChecked = true;
            }
            else {
                $scope.ArrSelectSociety1 = [];
            }
        })
    }



    $scope.DleteElectionStatuswise = function (electionId) {
        $scope.ElectionID = electionId;
      
        $scope.ArrSelectSociety1 = [];
        $scope.GetSocietylistByElection(electionId);
       
        $("#myModal11").modal("show");
    }




    $scope.CancelElection = function () {
      
        if ($scope.validatestop()) {
          $(".se-pre-con").fadeIn("slow");
            if ($scope.SocietyListByElection.length > 1) {
                if ($('input:checkbox').prop('checked') == true || ($('input:checkbox.chk').prop('checked') == true) || (JSON.stringify($scope.ArrSelectSociety1)).length > 50) {
                    var electionData1 = {
                        "Election_SocityIds": $scope.ArrSelectSociety1,
                        "ElectionSocietyStatus": $scope.ElectionStopStatus,
                        "ElectionStopDocument": $scope.FileName,
                        "ElectionRemark": $scope.ElectionDescription,
                        "ElectionMasterId": $scope.ElectionID
                    };
                    $.ajax({
                        url: '../Election/UpdatedElectionStatus',//SaveElectionMasterdata
                        data: JSON.stringify({ objElectionSocietyDataList: electionData1 }),
                        type: 'POST',
                        contentType: 'application/json',
                        dataType: 'json',
                        beforeSend: function () {
                        },
                        success: function (data) {
                            $("#myModal11").modal("hide");
                            $(".se-pre-con").fadeOut("slow");
                            $scope.GetElectionOrderList();
                        },

                    });

                }

                else {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("Warning", "Please select at least one society for process.", function () { });
                }
                     
                  }
            else {
              
                      var electionData1 = {
                          "Election_SocityIds": $scope.SocietyListByElection,
                          "ElectionSocietyStatus": $scope.ElectionStopStatus,
                          "ElectionStopDocument": $scope.FileName,
                          "ElectionRemark": $scope.ElectionDescription,
                          "ElectionMasterId": $scope.ElectionMasterId
                };
                $.ajax({
                    url: '../Election/UpdatedElectionStatus',//SaveElectionMasterdata
                    data: JSON.stringify({ objElectionSocietyDataList: electionData1 }),
                    type: 'POST',
                    contentType: 'application/json',
                    dataType: 'json',
                    beforeSend: function () {
                    },
                    success: function (data) {
                        $("#myModal11").modal("hide");
                        $(".se-pre-con").fadeOut("slow");
                        $scope.GetElectionOrderList();
                    },

                });

       } 
        }
    }


    $scope.validatestop = function () {
      
        var message = "";
        if ($scope.ElectionStopStatus == '' || $scope.ElectionStopStatus == undefined ) {
            message += $scope._language == 'en-us' ? 'Please choose Election Status.<br />' : '<br />कृपया चुनाव की स्थिति चुनें।';
        }

        if ($scope.FileName == '' || $scope.FileName == undefined) {
            message += $scope._language == 'en-us' ? 'Please upload  election ban related  file.<br />' : '<br />कृपया चुनाव प्रतिबंध संबंधी फाइल अपलोड करें।';
        }
        if ($scope.ElectionDescription == '' || $scope.ElectionDescription == undefined) {
            message += $scope._language == 'en-us' ? 'Please Enter Election description.<br />' : '<br />कृपया इलेक्शन विवरण दर्ज करें।';
        }


        //if ($scope.SocietyListByElection.length > 1) {

        //    if ($scope.ArrSelectSociety1.SocietyId1 == '' || $scope.ArrSelectSociety1.SocietyId1 == undefined) {
        //        message += $scope._language == 'en-us' ? 'Please select Society.<br />' : '<br />कृपया सोसाइटी चुनें।';
        //    }
        //}
        if (message.length > 29) {
            ShowOkMessage("निम्नलिखित मान्य करें", message, function () { });
            return false;
        }
        return true;
    }


    $scope.ElectionReshedule = function (Electionid) {
        //
        //alert(Electionid);
        $scope.RedirectTopublishOrImpoDate(Electionid) 
    }
    $scope.OnPageLoad();
    $scope.GetElectionOrderList(fromdate, todate);
    $scope.GetDistrictList();


    $scope.ChangeFile1 = function (type) {
        if (type == 1) {
            $scope.IsUploadFileName = false;
            $scope.FileName = "";
        }
    }
    $scope.UploadFile_new = function (id, type) {
        try {
            // FormData
            if (window.FormData !== undefined) {
                var fileUpload = $("#" + id).get(0);
                var files = fileUpload.files;
                //var blob = b64toBlob(realData, contentType, 512);
                var formData = new FormData();
                formData.append("pldbfile", files[0]);
                $(".blockUI").show();
                $scope.xhr1(SiteUrl + "/Election/UploadElectionFile", formData, type);
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
                            $scope.FileName = request.responseText;
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

    $scope.FileType = "";

    $scope.AddFile1 = function (type) {
      
        $scope.FileType = type;
        $scope.IsUploadFileName = true;
    }



});

MasterMod.directive('validfile1', function validFile() {
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
                                scope.IsUploadFileName = true;
                                scope.FileName = "";
                            }
                            scope.$apply();
                        }
                    }

                });
            };
        }
    };



});

