
var MasterMod = angular.module('UnitMastersModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("UnitMasterCont", function ($scope, $http,$rootScope) {

    // SCOPE



    $scope.DataPaging = {
        searchBoxDivId: 'search-filter',
        currentPageID: 1,
        pageSize: 10,
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
    $scope.FilterTypeChange = function () {
        $scope.Filter.SearchText = "";
        $scope.Filter.FromDate = "";
        $scope.Filter.ToDate = "";
        $scope.Filter.CCSId = '0';
        if ($scope.Filter.FilterType == 'created_dated') {
            $scope.showSearchDate = true;
            $scope.showSearchText = false;
            // $scope.AddCalender();```1    
        } else {
            $scope.showSearchDate = false;
            $scope.showSearchText = true;
        }
    }


    $scope._language = 'en-us';
  //  $scope.currentPage = 1;
    //$scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;
    $scope.UnitmasterData = []; 

    $scope.IsShowAssignForm = true;
    $scope.IsShowAssignForm1 = false;

    $scope.SaveUnitForm = function () {
        $http.post(SiteUrl + "/Master/SaveUnitForm", $scope.UnitMaster).
            success(function (data, status) {
              
                if (data > 0) {
                    alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n')
                        || $scope.ClearUnitForm();
                }
                else {
                    alert(_language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n') || $scope.ClearUnitForm();
                    return;
                }
                if ($scope.ParentId == '') {
                    $scope.ParentId = 0;
                }

                $scope.GetAllSubUnitList();
                if (data.Status) {

                    $scope.UnitMaster = null;
                    $scope.ClearUnitForm();
                    //$scope.GetAllSubClassesList();


                }
            }).
            error(function (data, status) {
                alert("Error while subclass insert");
            });


    };

    // on page load
    $scope.OnPageLoad = function () {
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

            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }

       
        $scope.AssignNew = function () {
           // _http = $http;
            $scope.IsShowAssignForm = false;
            $scope.IsShowAssignForm1 = true;
            $scope.AllDetails = false;
            addressdataset1 = {
                Housebuilding: "",
                State: "RJ",
                District: " "+$("#hdnDistrictId").val(),
                Lanestreet: "",
                Locality: "",
                Pincode: "",
                CITY_ID: "0",
                BLOCK_ID: "0",
                GP_ID: "0"

            }
            SetAddressinfo($scope.translation, 'MemberAdd', addressdataset1, $http);

        }

        $scope.Department = [];
        //$http.get(SiteUrl + "/Master/GetMasterData/?MasterType=INSPECTOR&MasterCode=-1").

        //    success(function (data, status) {
               
        //        $scope.Department = data;
        //    }).
        //    error(function (data, status) {
        //        $scope.Department = [];
        //    });
       
    };
    $scope.InspacorData = [];
    $scope.GetInspactor = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=INSPECTOR&MasterCode=-1").
            success(function (data, status) {
                $scope.InspacorData = data;      
              
            }).
            error(function (data, status) {
                $scope.AssignedAuditor = [];
                alert('Error while auditor data list fetch.');
            });
        $(".se-pre-con").fadeOut("slow");
    }


    $scope.IsShowAssignForm = false;



  

    $("#HouseBuildingNo_MemberAdd").parent().hide();
    $("#LaneStreetName_MemberAdd").parent().hide();
    $("#Locality_MemberAdd").parent().hide();
    $("#ddlstate_MemberAdd").parent().hide();
    $("#ddlDivision_MemberAdd").parent().hide();
    $("#ddlDivision_MemberAdd").parent().hide();
    //$("#district_MemberAdd").parent().hide();
    if ($("#hdnDistrictId").val() != "" && $("#hdnDistrictId").val() != "0") {
        $("#district_MemberAdd").attr("disabled", "disabled");
    }
    $("#ddlward_MemberAdd").parent().hide();
    $("#ddlVillage_MemberAdd").parent().hide();
    $("#pincode_MemberAdd").parent().hide();



    // START : Change Status for SMS / delete SMS,Status
    $scope.SetStatus = function (UnitId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Master/ChangeStatusunit", { Id: UnitId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllSubUnitList();
               
            }).
            error(function (data, status) {
            });
    };

  
    $scope.update = function (item) {
        
        $scope.UnitMaster.DepartmentId = item.departmentId;
        $scope.UnitMaster.UnitNameEn = item.UnitNameEn;
        $scope.UnitMaster.UnitNameHi = item.UnitNameHi;
        $scope.UnitMaster.IsActive = item.IsActive == 1 ? true : false;
        $scope.UnitMaster.UnitId = item.UnitId;
    };

    $scope.OnPageLoad();
    $scope.GetInspactor();

    $scope.ischeckdtrue = false;

    $scope.OtherPersonList = [];
    $scope.OtherPersonListNew = [];
    //$scope.OtherPersonList.push({ "PersonId": 0, "AdId": 0, "Name": "", "Address": "", "Post": "", "Mobile": "" });
    //$scope.OtherPersonListNew.push({ "PersonId": 0, "AdId": 0, "Name": "", "Address": "", "Post": "", "Mobile": "" });
    //Submit level to District committee
    $scope.GSSStatusList = [];

    $scope.GSSStatusList.push({ GSSSStatusId: "1", StatusNameEn: "Survey Submitted ", StatusNameHi: "Survey Submitted" })
    $scope.GSSStatusList.push({ GSSSStatusId: "2", StatusNameEn: "Close", StatusNameHi: "Close" })
    $scope.GSSStatusList.push({ GSSSStatusId: "3", StatusNameEn: "District Level Committee Report Submitted", StatusNameHi: "Submit level to District committee" })

    $scope.InspactorId = "0";
    $scope.AddGSS = function () {
     if ($scope.ValidateAddGSS()) {
         var data = GetAddressinfo('MemberAdd');
         $scope.GramPanchayatduplicat = false;
         $scope.Cityduplicat = false;
         if (data.Urban_Rural != "Urban") {
             var message1 = ' ';
             $scope.OtherPersonListNew.forEach(function (item, index) {
                 if (item.GramPanchayat_ID == data.PanchayatId) {
                     $scope.GramPanchayatduplicat = true;
                 }
             });
         }
         else {
             var message1 = ' ';
             $scope.OtherPersonListNew.forEach(function (item, index) {
                 if (item.City_Block_ID == data.CityId) {
                     $scope.Cityduplicat = true;
                 }
             });

             if (data.Urban_Rural == "Urban") {
                 data.PanchayatId = null;
             }
         }
             if (data.CityId == 0) {
                 data.City_Name = "";
             }
         if ($scope.GramPanchayatduplicat != true && $scope.Cityduplicat != true ) {


             var InspacorName = $scope._language == 'en-us' ? $scope.InspacorData.filter(function (x) { return x.MappingID == $scope.InspactorId })[0].OfficerNameEn : $scope.InspacorData.filter(function (x) { return x.MappingID == $scope.InspacorId })[0].OfficerNameHi;
             $scope.OtherPersonListNew.push({ "Inspector": InspacorName, "Urban_Rural": data.Urban_Rural, "City_Block": data.Checkboxvalue == "2" ? data.Block_Name : data.City_Name, "GramPanchayat": data.Checkboxvalue == "2" ? data.Panchayat_Name : "", "Inspector_ID": $scope.InspactorId, "City_Block_ID": data.Checkboxvalue == "2" ? data.BlockId : data.CityId, "GramPanchayat_ID": data.PanchayatId, "DistrictId": data.DistrictId, "MonthId": $scope.MonthID });
             if ($scope.checksta == true) {
                 var addressdataset1 = {
                     Housebuilding: "",
                     State: "RJ",
                     District: " " + $("#hdnDistrictId").val(),
                     Lanestreet: "",
                     Locality: "",
                     Pincode: "",
                     CITY_ID: "0",
                     BLOCK_ID: "0",
                     GP_ID: "0"

                 }
                 SetAddressinfo($scope.translation, 'MemberAdd', addressdataset1, $http);
             }
         }
         else {

             if ($scope.GramPanchayatduplicat == true) {
                 message1 += $scope._language == 'en-us' ? '<br />already added gram panchayat please another gram panchayat select.' : '<br />पहले से जोड़ी गई ग्राम पंचायत है कृपया अन्य  ग्राम पंचायत का चयन करें।';
                 if (message1.length > 1) {
                     ShowOkMessage("Inspector Error", message1, function () { });
                 }
             }
             if ($scope.Cityduplicat == true) {
                 message1 += $scope._language == 'en-us' ? '<br />already added city please another city select.' : '<br />पहले से जोड़ी गई शहर है कृपया अन्य  शहर का चयन करें।';
                 if (message1.length > 1) {
                     ShowOkMessage("Inspector Error", message1, function () { });
                 }
             }
                  
             }
             
           }
        
    }
    var InspactorId = 0;
    $scope.SaveIODetailsUser = function () {
  
        $(".se-pre-con").fadeIn("slow");
        var formData = new FormData();
        var OtherPersonListNewIns = [];
        $scope.OtherPersonListNew.forEach(function (itm) {
            OtherPersonListNewIns.push({
                Inspector_ID: itm.Inspector_ID,
                InspacorName: itm.Inspector,
                City_Block: itm.City_Block,
                City_Block_ID: itm.City_Block_ID,
                GramPanchayat:itm.GramPanchayat,
                GramPanchayat_ID: itm.GramPanchayat_ID,
                DistictId: itm.DistrictId,
                Urban_Rural: itm.Urban_Rural,
                MonthId: itm.MonthId
            });
        });

        if (OtherPersonListNewIns.length>0) {

            $http.post(SiteUrl + "/Society/SaveInspactorTargets", { "InspactorTargets": OtherPersonListNewIns }).
                success(function (data, status) {
                    if (data == "1") {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () { });
                        $scope.OtherPersonListNew = [];
                        $scope.GetInspactorTargetsCount(InspactorId);
                        $scope.IsShowAssignForm1 = false;
                        $scope.IsShowAssignForm = true;
                        $(".se-pre-con").fadeOut("slow");
                    }

                }).
                error(function (data, status) {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    $(".se-pre-con").fadeOut("slow");
                })
        } else {
            $(".se-pre-con").fadeOut("slow");
            var message = _language == 'en-us' ? 'Please Add Inspector Details. \n' : 'कृपया इंस्पेक्टर विवरण जोड़ें \n';
            if (message.length > 1) {
                ShowOkMessage("Inspector Details", message, function () { });
                return false;
            }
            return true;
        }
    }

    $scope.Status1 = '0';


   



    $scope.Backtomove = function () {
        $scope.IsShowAssignForm = true;
        $scope.IsShowAssignForm1 = false;
    }

    $scope.RemoveOtherPersonListRow = function (index) {
        $scope.OtherPersonListNew.splice(index, 1);
        $scope.$apply();
    }

    $scope.AllDetails = false;

 
    $scope.GetInspactorTargetsCount = function () {

       
        
        $http.post("/Society/GetInspactorTargetsCount", JSON.stringify({ InspactorId: $scope.InspactorId, GSSSStatusId: $scope.GSSSStatusId})).
            success(function (data, status) {
               
                $scope.InspactorTargetsList = JSON.parse(data.DataList);
                $scope.IsShowAssignForm = true;
            }).
            error(function (data, status) {
                $scope.InspactorTargetsList = [];
                alert('Error while Society Election Detail data.');
            });
    };

    $scope.ClearSearch = function () {

        $scope.InspactorId = '0';
        InspactorId = $scope.InspactorId;
        $scope.GetInspactorTargetsCount(InspactorId);
    }

    $scope.InspactorId = '0';
    $scope.getinsepectorwise = function (month, Inspector_ID, statusid) {
        $scope.Status1 = '0';
        $scope.GSSSStatusId = statusid;

        if (statusid == 1) {
            $scope.Ischeckedall = false;
        }
        $scope.InspactorId = Inspector_ID;
        //$scope.searchRequest(GetInspactorTargetsDetails());
        $scope.DataPaging.currentPageID = 1;
        $scope.GetInspactorTargetsDetails(month);
        $scope.ischeckdtrue = false;
    }
    $scope.GetInspactorTargetsDetails = function (month) {
        $(".blockUI").show();
        var jsonParameters = { "TotalRecords": $scope.DataPaging.TotalRecords, "PageNumber": $scope.DataPaging.currentPageID, "PageSize": $scope.DataPaging.pageSize, "SortingColumn": $scope.DataPaging.sortExpression, "SortingOrder": $scope.DataPaging.sortDirection, "SearchParameter": $scope.DataPaging.searchParameters, "SearchType": $scope.SearchType };
        $http.post(SiteUrl + "/Society/GetInspactorTargetsDetails", JSON.stringify({ Details: jsonParameters, InspactorId: $scope.InspactorId, StatusId: $scope.Status1, GSSSStatusId: $scope.GSSSStatusId, Flag: 2, Month: month})).
            success(function (data, status) {
                $scope.InspactorTargetsListDetils = JSON.parse(data.DataList);
                $scope.AllDetails = true;
                $scope.IsShowAssignForm = false;
                $rootScope.DataPagingDetails = data.DataPaging;
                $rootScope.SetDataPaging();
                $(".blockUI").hide();

            }).
            error(function (data, status) {
                $(".blockUI").hide();
                $scope.InspactorTargetsListDetils = [];
                alert('Error while Society Election Detail data.');
            });
    };


    $scope.GetInspactorTargetsCount(InspactorId);

    $scope.SubmitSurveyReport = function () {
        $scope.GSSSStatusId = '1';
        $scope.Status1 = '5';
        $scope.Ischeckedall = false;
        $scope.InspactorId = 0;
        $scope.DataPaging.currentPageID = 1;
        $scope.GetInspactorTargetsDetails(-1);
        $scope.ischeckdtrue = false;
        $scope.PrintDatalist = [];


    }

    $scope.BacktoMainDiv = function () {

        $scope.Ischeckedall = true;
        $scope.ischeckdtrue = false;
        $scope.InspactorId = '0';
        $scope.AllDetails = false;
        $scope.IsShowAssignForm = true;
    }


    $scope.ComplainUndeVlaue = {};
    $scope.ValidateAddGSS = function () {
        var data = GetAddressinfo('MemberAdd');
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.InspactorId == "0") {
            message += $scope._language == 'en-us' ? '<br />Please  Select Inspactor.' : '<br />कृपया इंस्पेक्टर चुनें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }


        //if ($scope.MonthID == "0" || $scope.MonthID == '' || $scope.MonthID == undefined) {
        //    message += $scope._language == 'en-us' ? '<br />Please  Select Month.' : '<br />कृपया माह का चयन करें।';
        //    //$("#txtAccountName_" + index).addClass("notValid");            
        //}


        if (data.Checkboxvalue == undefined || data.Checkboxvalue == "") {
            message += $scope._language == 'en-us' ? '<br />Please  Select Urban/Rural.' : '<br />कृपया शहरी/ग्रामीण चुनें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }

        if (data.Checkboxvalue != undefined && data.Checkboxvalue == "2" && data.BlockId=="0") {
            message += $scope._language == 'en-us' ? '<br />Please  Select Block.' : '<br />कृपया ब्लॉक चुनें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }

        if (data.Checkboxvalue != undefined && data.Checkboxvalue == "2" && data.PanchayatId == "0" || data.PanchayatId == "" && data.Checkboxvalue == "2" ) {
            message += $scope._language == 'en-us' ? '<br />Please  Select Gram Panchayat.' : '<br />कृपया ग्राम पंचायत चुनें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }

        if (data.Checkboxvalue != undefined && data.Checkboxvalue == "1" && data.CityId == "0") {
            message += $scope._language == 'en-us' ? '<br />Please  Select City .' : '<br />कृपया सिटी चुनें।';
            //$("#txtAccountName_" + index).addClass("notValid");            
        }
     
        if (message.length > 29) {
            ShowOkMessage("Society Complaint ", message, function () { });
            $scope.checksta = false;
            return false;
           
        }
        $scope.checksta = true;
        return true;
       
    }

    function _Selectedinspector() {
        this.InspactorTargetsId = '0';
    }
    $scope.ShowSelectedInspector = {
        IsChecked: false
    }

    $scope.PrintDatalist = [];
    $scope.ArrInspector = [];
    $scope.Chkdinspector = function (item) {
      
        var _SelectedinspectorList = new _Selectedinspector();
        _SelectedinspectorList.InspactorTargetsId = item.InspactorTargetsId;
        if (item.chkdboxValue) {
            $scope.ArrInspector.push(item);
            $scope.PrintDatalist.push(item)
            $scope.ischeckdtrue = true;
        }
        else {
            var data = $scope.ArrInspector;
            $scope.ArrInspector = [];
            $scope.ischeckdtrue = false;
            data.forEach(function (itm, idx) {
                if (itm.InspactorTargetsId != item.InspactorTargetsId) {
                    $scope.ArrInspector.push(item);
                    $scope.PrintDatalist.push(item);
                    $scope.ischeckdtrue = true;
                }
            });
        }
        console.log($scope.ArrInspector);
    }
    $scope.ischeckdtrue = false;


    $scope.ChkdiAllnspector = function () {
       
        if ($scope.Ischeckedall) {
            $scope.InspactorTargetsListDetils.forEach(function (itm, index) {
                itm.chkdboxValue = true;
                $scope.ArrInspector.push(itm);
                $scope.PrintDatalist.push(itm)
            });
        }
        else {
            $scope.InspactorTargetsListDetils.forEach(function (itm, index) {
                itm.chkdboxValue = false;
                $scope.ArrInspector = [];
                $scope.PrintDatalist = [];
               
            });
        }
        $scope.ischeckdtrue = $scope.InspactorTargetsListDetils.filter(function (x) { return x.chkdboxValue == true }).length > 0;
    }




    //Senction Checkbox


    $scope.isSenction = false;
    $scope.ChkdiAllSenction = function () {
    
        if ($scope.IscheckedallSenction) {
            $scope.InspactorTargetsListDetils.forEach(function (itm, index) {
                itm.chkdboxValue = true;
                $scope.ArrInspector.push(itm);
            });
        }
        else {
            $scope.InspactorTargetsListDetils.forEach(function (itm, index) {
                itm.chkdboxValue = false;
                $scope.ArrInspector = [];
            });
        }
        $scope.isSenction = $scope.InspactorTargetsListDetils.filter(function (x) { return x.chkdboxValue == true }).length > 0;
    }

    $scope.ArrInspector = [];
  
    $scope.ChkdSenction = function (item) {
        var _SelectedinspectorList = new _Selectedinspector();
        _SelectedinspectorList.InspactorTargetsId = item.InspactorTargetsId;
        if (item.chkdboxValue) {
            $scope.ArrInspector.push(item);
            $scope.isSenction = true;
        }
        else {
            var data = $scope.ArrInspector;
            $scope.ArrInspector = [];
            $scope.ischeckdtrue = false;
            data.forEach(function (itm, idx) {
                if (itm.InspactorTargetsId != item.InspactorTargetsId) {
                    $scope.ArrInspector.push(item);
                    $scope.isSenction = true;
                }
            });
        }
        console.log($scope.ArrInspector);
    }


    $scope.ApprovedForRagistration = function () {
        if ($scope.validatestop()) {
            $scope.ReqoestStatus = '8';
            $(".se-pre-con").fadeIn("slow");
            var checkedData = $scope.InspactorTargetsListDetils.filter(function (x) { return x.chkdboxValue });
            $http.post(SiteUrl + "/Society/SaveInspactorStatusDR", { InspactorStatusDetails: checkedData, Remarks: $scope.RemarksSenction, Inspectorfile: $scope.FileName, status: $scope.ReqoestStatus, TXNno: "", SenctionDate: "" }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data == "1") {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () {
                            window.location.href = "../society/InspactorTargets";
                        });
                    } else {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "There is a problem while submitting the request. Please try after some time.<br/></b>" : "अनुरोध सबमिट करते समय समस्या आ रही  है। कृपया कुछ समय बाद प्रयास करें।<br/></b>", function () { });
                    }
                }).
                error(function (data, status) {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    $(".se-pre-con").fadeOut("slow");
                });
        }   
    }

    //end--Senction

    $scope.validatestop = function () {
    
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.InspactorTargetsListDetils.filter(function (x) { return x.chkdboxValue }).length == 0) {
            message += $scope._language == 'en-us' ? '<br />Please select at least one record for process.' : '<br />कृपया प्रक्रिया के लिए कम से कम एक रिकॉर्ड का चयन करें।';
        }


        if ($scope.GSSSStatusId != 4) {
            if ($scope.ReqoestStatus == undefined || $scope.ReqoestStatus == "") {
                message += $scope._language == 'en-us' ? '<br />Please select Approved/Rreject.' : '<br />कृपया स्वीकृत/अस्वीकार का चयन करें।';
            }

            if ($scope.ReqoestStatus == 7 && ($scope.Remarks == null || $scope.Remarks == "" || $scope.Remarks == undefined)) {
                message += $scope._language == 'en-us' ? '<br />Please Enter Remarks.' : '<br />कृपया रिमार्क दर्ज़ करें।';
            }

            if ($scope.ReqoestStatus == 3 && ($scope.FileName == null || $scope.FileName == "" || $scope.FileName == undefined)) {
                message += $scope._language == 'en-us' ? '<br />Please select the proposal report file.' : '<br />कृपया प्रस्ताव रिपोर्ट फ़ाइल का चयन करें।';
            }
        }
      

        if ($scope.GSSSStatusId == 4 && ($scope.RemarksSenction == null || $scope.RemarksSenction == "" || $scope.RemarksSenction == undefined)) {
            message += $scope._language == 'en-us' ? '<br />Please Enter Remarks.' : '<br />कृपया रिमार्क दर्ज़ करें।';
        }

        if (message.length > 29) {
            ShowOkMessage("Inspector Status ", message, function () { });
            $scope.checksta = false;
            return false;

        }
        $scope.checksta = true;
        return true;
    }
    $scope.SubmitDRToDistrict = function () {
        if ($scope.validatestop()) {
            $(".se-pre-con").fadeIn("slow");
            var checkedData = $scope.InspactorTargetsListDetils.filter(function (x) { return x.chkdboxValue });
            $http.post(SiteUrl + "/Society/SaveInspactorStatusDR", { InspactorStatusDetails: checkedData, Remarks: $scope.Remarks, Inspectorfile: $scope.FileName, status: $scope.ReqoestStatus,  TXNno:"", SenctionDate:"" }).
                success(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    if (data == "1") {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () {
                            window.location.href = "../society/InspactorTargets";
                        });
                    } else {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "There is a problem while submitting the request. Please try after some time.<br/></b>" : "अनुरोध सबमिट करते समय समस्या आ रही  है। कृपया कुछ समय बाद प्रयास करें।<br/></b>", function () { });
                    }
                }).
                error(function (data, status) {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                    alert(message);
                    $(".se-pre-con").fadeOut("slow");
                });
            }        
    }

    $scope.SubmitDRToDistrict123 = function () {
        
         if ($scope.validatestop()) {
            $(".se-pre-con").fadeIn("slow");
            if ($scope.InspactorTargetsListDetils.length > 1) {
                if ($('input:checkbox').prop('checked') == true || ($('input:checkbox.chk').prop('checked') == true) || (JSON.stringify($scope.ArrSelectSociety)).length > 50) {
                   
                    $http.post(SiteUrl + "/Society/SaveInspactorStatusDR", { InspactorStatusDetails: $scope.ArrInspector, Remarks: $scope.Remarks, Inspectorfile: $scope.FileName }).
                        success(function (data, status) {
                            if (data == "1") {
                                ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () { });
                                $(".se-pre-con").fadeOut("slow");
                            }
                        }).
                        error(function (data, status) {
                            message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                            alert(message);
                            $(".se-pre-con").fadeOut("slow");
                        })
                }

                else {
                    $(".se-pre-con").fadeOut("slow");
                    ShowOkMessage("Warning", "Please select at least one Inspector Details for process.", function () { });
                }

            }
            else {

              
                $http.post(SiteUrl + "/Society/SaveInspactorStatusDR", { InspactorStatusDetails: $scope.ArrInspector, Remarks: $scope.Remarks, Inspectorfile: $scope.FileName }).
                    success(function (data, status) {
                        if (data == "1") {
                           ShowOkMessage("Success", $scope._language == 'en-us' ? " successfully submitted.<br/></b>" : "सफलतापूर्वक जमा कर लिया गया है|<br/></b>", function () { });
                            $(".se-pre-con").fadeOut("slow");
                        }
                    }).
                    error(function (data, status) {
                        message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                        alert(message);
                        $(".se-pre-con").fadeOut("slow");
                    })


            }
   }
    }
    $scope.FileName = "";
    $scope.FileType  = "";
    $scope.IsUploadFileName = false;

    $scope.AddFile1 = function (type, id) {
       
        $scope.FileType = type;
        $scope.IsUploadFileName = true;
        $scope.IsUpload = false;
    }
    $scope.ChangeFile1 = function (type, id) {
       
        if (type == 1) {
            $scope.IsUploadFileName = false;
            $scope.FileName = "";
            $scope.IsUpload = false;
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
                formData.append("MemberFile", files[0]);
                $(".blockUI").show();
                $scope.xhr1(SiteUrl + "/Society/UploadinInspactorFile", formData, type);
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
                            $scope.IsUpload = true;
                            $scope.IsUploadFileName = true;
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


    $scope.MonthList = [];
    $scope.GetAuditMonthList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=MONTH&MasterCode=-1").
            success(function (data, status) {
                $scope.MonthList = data;
            }).
            error(function (data, status) {
                $scope.MonthList = [];
            });
    };
    $scope.GetAuditMonthList();
});

MasterMod.directive('validfile', function validFile() {
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
                            scope.IsUploadFileName = false;
                        } else {
                            if (scope.FileType == 1) {
                                scope.IsUploadFileName = true;
                            }
                            scope.$apply();
                            
                        }
                    }


                });
            };
        }
    };



});




