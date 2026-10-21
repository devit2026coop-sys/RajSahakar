var ArrSelectTraningUser = new Array();

function TraningUserSelected() {
    this.UserId = '0';
}

var AppTrainning = angular.module("GopalCreditCardAppEntryForm", ['angularUtils.directives.dirPagination']);
AppTrainning.controller("GopalCreditCard", function ($scope, $location, $http) {
    // Save Sms Template 
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.DivSms = true;
    $scope.newdiv = "";
    $scope.Trainning = '';

    //madhuri
    $scope.showGccReportFile = false;
    $scope.ShowOTPDiv = false;


    // on page load
    $scope.OnPageLoad = function () {
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

        $scope.ImageValidationMessage = '';
        $scope.FileCollection = [];



        // Initialise Main form control
        $scope.GopalCreditCardModel = {}

        $scope.GopalCreditCardModel.LoanDistrictId = '0';
        $scope.GopalCreditCardModel.LoanBankID = "0";
        $scope.GopalCreditCardModel.LoanBranchId = "0";
        $scope.GopalCreditCardModel.PACS_ID = "0";

        $scope.GetDistrictWithoutDivisionList();



        var url = ($location.absUrl());
        var params = url.split('?');
        var params_1 = params[1].split('&');


        var index = params_1[0].indexOf('=');
        var index1 = params_1[1].indexOf('=');

     
        $scope.GCCEId = params_1[0].substring(index + 1);
        $scope.GCCERNO = params_1[1].substring(index1 + 1);

    
        $scope.GetFormViewData();

        $scope.GetCurrentUserId();
        $scope.GetCurrentRoleId();
    };
    $scope.IsAddressSameAsJanAadhar_Changed = function (event) {


        if ($scope.GopalCreditCardModel.IsAddressSameAsJanAadhar == true) {
            $scope.GopalCreditCardModel.IsRural = $scope.GopalCreditCardModel.Original_IsRural;
            $scope.GopalCreditCardModel.District_Code = $scope.GopalCreditCardModel.Original_District_Code;
            $scope.GopalCreditCardModel.Block_Code = $scope.GopalCreditCardModel.Original_Block_Code;
            $scope.GopalCreditCardModel.City_Code = $scope.GopalCreditCardModel.Original_City_Code;
            $scope.GopalCreditCardModel.GP_Code = $scope.GopalCreditCardModel.Original_GP_Code;
            $scope.GopalCreditCardModel.Village_Code = $scope.GopalCreditCardModel.Original_Village_Code;
            $scope.GopalCreditCardModel.Address_ENG = $scope.GopalCreditCardModel.Original_Address_ENG;
            $scope.GopalCreditCardModel.Address_HI = $scope.GopalCreditCardModel.Original_Address_HI;
            $scope.GopalCreditCardModel.PinCode = $scope.GopalCreditCardModel.Original_PinCode;

            $scope.RuralUrbanBlock();
            
        }
        else {
            $scope.GopalCreditCardModel.District_Code = "0";
            //$scope.GopalCreditCardModel.CityTypes = "0";
            $scope.GopalCreditCardModel.City_Code = "0";
            $scope.GopalCreditCardModel.Ward_Code = "0";
            $scope.GopalCreditCardModel.Block_Code = "0";
            $scope.GopalCreditCardModel.GP_Code = "0";
            $scope.GopalCreditCardModel.Village_Code = "0";
            $scope.GopalCreditCardModel.Address_ENG = "";
            $scope.GopalCreditCardModel.Address_HI = "";
            $scope.GopalCreditCardModel.PinCode = "";
            $scope.GopalCreditCardModel.IsRural = null;
            $scope.UrbanDiv = false;
            $scope.RuralDiv = false;
        }

        return true;
    }
    $scope.RuralUrbanBlock = function (V) {
        
        var SelectedVal = $scope.GopalCreditCardModel.IsRural == 0 ? false : true;
            if (SelectedVal == true || SelectedVal == true) {
                $scope.UrbanDiv = false;
                $scope.RuralDiv = true;
                $scope.BlockListByOffAdd(1);
                $scope.GramPanchayatList();
                $scope.VillageList();
                
            }
            else if (SelectedVal == false || SelectedVal == false) {
                $scope.UrbanDiv = true;
                $scope.RuralDiv = false;
                $scope.GetCityList();
                $scope.GetWardList();
                
            }
            else {
                $scope.UrbanDiv = false;
                $scope.RuralDiv = false;
            }
    };
    $scope.GetDistrictWithoutDivisionList = function () {
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=DISTRICT&MasterCode=0").
            success(function (data, status) {
                $scope.DistrictList = data;
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    $scope.ResetDataForAddress = function (val, Level) {
        if (val == 1) {
            if (Level < 3) {
                $scope.GopalCreditCardModel.IsRural = undefined;
                $scope.CityListdata = [];
                $scope.Blocksdata = [];
                $scope.GramPanchayats = [];
                $scope.Villages = [];
                $scope.Ward = [];
                $scope.GetCityList();
                $scope.BlockListByOffAdd(val);
                $scope.UrbanDiv = false;
                $scope.RuralDiv = false;

                $scope.GopalCreditCardModel.Block_Code = "0";
                $scope.GopalCreditCardModel.GP_Code = "0";
                $scope.GopalCreditCardModel.Village_Code = "0";
                $scope.GopalCreditCardModel.City_Code = "0";
                $scope.GopalCreditCardModel.Ward_Code = "0";
            }
        }
        else if (val == 2) {
            if (Level < 3) {
                $scope.GopalCreditCardModel.IsRuralcopy = undefined;
                $scope.CityListdatacopy = [];
                $scope.Blocksdatacopy = [];
                $scope.GramPanchayatscopy = [];
                $scope.Villagescopy = [];
                $scope.Wardcopy = [];
            }
        }
    }
    $scope.BlockListByOffAdd = function (id) {
        $(".se-pre-con").fadeIn("slow");
        $scope.Blocksdata = [];
        $scope.GramPanchayats = [];
        $scope.Villages = [];
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=PANCHAYATSAMITY_DISTRICTCODE&MasterCode=" + $scope.GopalCreditCardModel.District_Code).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Blocksdata = data;
            }).
            error(function (data, status) {
                $scope.Blocksdata = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.GramPanchayatList = function (id) {
       // debugger;
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=GRAMPANCHAYAT_BLOCKCODE&MasterCode=" + $scope.GopalCreditCardModel.Block_Code).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.GramPanchayats = data;
            }).
            error(function (data, status) {
                $scope.GramPanchayats = [];
                $(".se-pre-con").fadeOut("slow");
            });

    };
    $scope.VillageList = function (id) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=VILLAGE_GPCODE&MasterCode=" + $scope.GopalCreditCardModel.GP_Code).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Villages = data;
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Villages = [];
            });

    };
    $scope.GetCityList = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CITY_DISTRICTCODE&MasterCode=" + $scope.GopalCreditCardModel.District_Code).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.CityList = data;
            }).
            error(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.CityList = [];
            });
    };
    $scope.GetWardList = function (id) {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=WARD_CITYCODE&MasterCode=" + $scope.GopalCreditCardModel.City_Code).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.Ward = data;
            }).
            error(function (data, status) {
                $scope.Ward = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.GetJanAadharDetail = function () {
        if ($scope.GopalCreditCardJanaadharValidate()) {
            $scope.JanMemberList = '';
            $http.post(SiteUrl + "/GopalCreditCard/GetJanAadhaarMembersList?JanAadharNo=" + $scope.JanAadharNo).
                success(function (data, status) {
                //    debugger;
                    if (data.length > 0) {
                        $scope.JanMemberList = data;
                    }
                }).
                error(function (data, status) {
                    alert("Error while fetching Year Target Data");
                });
        }
    }
    $scope.GetFormViewData = function () {
        $http.post(SiteUrl + "/GopalCreditCard/Get_GopalApplicationFormViewData1/?GCCEId=" + $scope.GCCEId +"&GCCERNO=" + $scope.GCCERNO).
            success(function (data, status) {
             //   debugger;
                $scope.GopalCreditCardModel = data;               
                $scope.GGG_Id = data.gcc_id;
                $scope.GetApplicationFlowGCC();
                $scope.RuralUrbanBlock();
                $scope.LoanBankList();
                $scope.Get_LoanBankBranch();
                $scope.Get_LoanBranch_PACS();
                $scope.IsMemberOfDCS_Changed();
                $scope.IsPACSLAMPSMember_Changed();
                $scope.DoesTheMilkSaleToDCS_Changed();
                $scope.GetApplicationDetails();
               

            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });
    };
    $scope.DoesTheMilkSaleToDCS_Changed = function (value) {

        var SelectedVal = $scope.GopalCreditCardModel.DoesTheMilkSaleToDCS == 'false' ? false : true;
            if (SelectedVal == true || SelectedVal == true) {
                $scope.GopalCreditCardModel.DoesTheMilkSaleToDCS_Status = true;
            }
            else if (SelectedVal == false || SelectedVal == false) {
                $scope.GopalCreditCardModel.DoesTheMilkSaleToDCS_Status = false;
                $scope.GopalCreditCardModel.EstimatedAmountOfMilkSoldToDCS = "";
            }
            else {
                $scope.GopalCreditCardModel.DoesTheMilkSaleToDCS = "";
                $scope.GopalCreditCardModel.DoesTheMilkSaleToDCS_Status = false;
                $scope.GopalCreditCardModel.EstimatedAmountOfMilkSoldToDCS = "";
            }
    };
    $scope.IsPACSLAMPSMember_Changed = function (value) {
        var SelectedVal = $scope.GopalCreditCardModel.IsPACSLAMPSMember == 'false' ? false : true;
        if (SelectedVal == true || SelectedVal == true) {
            $scope.GopalCreditCardModel.IsPACSLAMPSMemberCheckedStatus = true;
        }
        else if (SelectedVal == false || SelectedVal == false) {
            $scope.GopalCreditCardModel.IsPACSLAMPSMemberCheckedStatus = false;
        }
    };
    $scope.IsMemberOfDCS_Changed = function (value) {

      //  debugger;

        var SelectedVal = $scope.GopalCreditCardModel.IsMemberOfDCS == 'false' ? false : true;

        if (SelectedVal == true || SelectedVal == true) {
            $scope.GopalCreditCardModel.IsMemberOfDCSCheckedStatus = true;
        }
        else if (SelectedVal == false || SelectedVal == false) {
            $scope.GopalCreditCardModel.IsMemberOfDCSCheckedStatus = false;
        }
        else {
            $scope.GopalCreditCardModel.IsMemberOfDCS = false;
            $scope.GopalCreditCardModel.IsMemberOfDCSCheckedStatus = false;
        }
    };
    $scope.LoanBankList = function () {

        $http.post(SiteUrl + "/GopalCreditCard/Get_CCBList/?id=" + $scope.GopalCreditCardModel.LoanDistrictId).
            success(function (data, status) {

                $scope.BankMasterList = data;
                
            }).
            error(function (data, status) {
                $scope.BankMasterList = [];
            });
    };

    $scope.Get_LoanBankBranch = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBankBranch/?LoanDistrictId=" + $scope.GopalCreditCardModel.LoanDistrictId + "&LoanBankId=" + $scope.GopalCreditCardModel.LoanBankID).
            success(function (data, status) {
                $scope.PLDBBranches = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PLDBBranches = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.Get_LoanBranch_PACS = function (branch_ID) {
        $(".se-pre-con").fadeIn("slow");
        return $http.post(SiteUrl + "/GopalCreditCard/Get_LoanBranch_PACS/?LoanBranchId=" + $scope.GopalCreditCardModel.LoanBranchId).
            success(function (data, status) {
                $scope.PACSNameList = data;
                $(".se-pre-con").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.PACSNameList = [];
                $(".se-pre-con").fadeOut("slow");
            });
    };
    $scope.printDiv = function (divName) {
        //debugger;
        var printContents = document.getElementById(divName).innerHTML;

        if (navigator.userAgent.toLowerCase().indexOf('chrome') > -1) {
            var popupWin = window.open('', '_blank', 'width=600,height=600,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no');
            popupWin.window.focus();
            popupWin.document.write('<!DOCTYPE html><html><head>' +
                '<link rel="stylesheet" type="text/css" href="style.css" />' +
                '</head><body onload="window.print()"><div class="reward-body">' + printContents + '</div></body></html>');
            popupWin.onbeforeunload = function (event) {
                popupWin.close();
                return '.\n';
            };
            popupWin.onabort = function (event) {
                popupWin.document.close();
                popupWin.close();
            }
        } else {
            var popupWin = window.open('', '_blank', 'width=800,height=600');
            popupWin.document.open();
            popupWin.document.write('<html><head><link rel="stylesheet" type="text/css" href="style.css" /></head><body onload="window.print()">' + printContents + '</body></html>');
            popupWin.document.close();
        }
        popupWin.document.close();

        return true;
    }
   
    $scope.DivtringGrid = true;

    //madhuri
    $scope.BackToPage = function () {       
        window.location = '../gopalCreditCard/ApplicationFlowGCC';
    }
    $scope.GetCurrentUserId = function () {
        $http.get("../ApplicationFlow/GetCurrentUserId").
            success(function (data, status) {
                $scope.BaseUserId = data;
            }).
            error(function (data, status) {
                $scope.BaseUserId = '';
            });
    }
    $scope.GetCurrentRoleId = function () {
        $http.get("../gopalCreditCard/GetCurrentRoleId").
            success(function (data, status) {
                $scope.BaseRoleId = data;
            }).
            error(function (data, status) {
                $scope.BaseRoleId = '';
            });
    }
    $scope.GetApplicationFlowGCC = function () {
        $scope.showLoader = true;
        $http.get("../gopalCreditCard/GetApplicationFlowGCC/?GCC_Id=" + $scope.GGG_Id).
            success(function (data, status) {
                $scope.ApplicationFlowGCC = data;     

                $scope.GCCDocResStatus();
                $scope.GSSProceesStatusList(data[0].StatusId, $scope.GGG_Id);
                $scope.ApplicationFlowMaster.UserId = data[0].UserId; //last UserId
                $scope.ApplicationFlowMaster.RoleId = data[0].RoleID; //last RoleID

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
        $scope.showLoader = true;
    }

    

    $scope.SaveGSSApplicationFlowStatus = function () {
        if ((($scope.ApplicationFlowMaster.IsRecommendationOfDCSDOC_Path == undefined || $scope.ApplicationFlowMaster.IsRecommendationOfDCSDOC_Path == '') && $scope.GopalCreditCardModel.RecommendationOfDCSDOC_Path != '') ||
            (($scope.ApplicationFlowMaster.IsDocumentOfProposedWork_Path == undefined || $scope.ApplicationFlowMaster.IsDocumentOfProposedWork_Path == '') && $scope.GopalCreditCardModel.DocumentOfProposedWork_Path != '') ||
            (($scope.ApplicationFlowMaster.IsCopyOfDCSPassbook_Path == undefined || $scope.ApplicationFlowMaster.IsCopyOfDCSPassbook_Path == '') && $scope.GopalCreditCardModel.CopyOfDCSPassbook_Path != '')
        ) {
            ShowOkMessage("", _language == 'en-us' ? 'Please Take Action on Documents\n' : 'कृपया दस्तावेजों पर कार्रवाई करें\n');
            return;
        }
        if ($scope.ApplicationFlowMaster.StatusId != 0 && $scope.ApplicationFlowMaster.Remark != "") {

            if ($scope.ApplicationFlowMaster.StatusId !=4) {              
                    $scope.saveDataInDB();               
            }
            else {
                AadhaarAuth("", $http, "", true, function (authData) {       
                    $scope.ApplicationFlowMaster.TXN = authData.TransId;
                    $(".se-pre-con").fadeIn("slow");
                    $scope.saveDataInDB();                
                    
                });
            }
        }
        else {
            alert(_language == 'en-us' ? 'Please Select Status and Remark\n' : 'कृपया स्थिति और टिप्पणी जोड़ें|\n');
        }
    };


    $scope.StatusChange = function () {
        var ddlStatus = $scope.ddlStatus;
        if (ddlStatus != '')
            $scope.GccProceesStatus = $scope.GccProceesStatus_Temp.filter(x => x.ProcessStatusType == ddlStatus);

        $scope.Appstatus1 = true;
        if ($scope.GccProceesStatus.length == 1) {
            $scope.Appstatus1 = false;
            $scope.ApplicationFlowMaster.StatusId = $scope.GccProceesStatus[0].GccProceesStatusId;
        }

    }
    $scope.getDistinctProceesStatus = function () {
        const statusMap = new Map();
        $scope.GccProceesStatus_Temp.forEach(item => {
            if (!statusMap.has(item.ProcessStatusType)) {
                statusMap.set(item.ProcessStatusType, item);
            }
        });
        return Array.from(statusMap.values());
    };


    $scope.GSSProceesStatusList = function (StatusId,GccId) {
        $scope.GccProceesStatus_Temp = [];   

        $http.post("../gopalCreditCard/GSSProceesStatusList", { "StatusId": StatusId, "GCC_Id": GccId }).
            success(function (data, status) {
                $scope.GccProceesStatus_Temp = data;               
                $scope.ProceesStatus = $scope.getDistinctProceesStatus(); 
                //if ($scope.GccProceesStatus_Temp.length == 1) { $scope.Appstatus = false; $scope.GccProceesStatus = $scope.GccProceesStatus_Temp; }
                //else { $scope.Appstatus = true; $scope.ProceesStatus = $scope.getDistinctProceesStatus(); }    
            }).
            error(function (data, status) {
                $scope.GccProceesStatus_Temp = [];
            });
        
    };

    $scope.GCCDocResStatus = function () {
        if ($scope.ApplicationFlowGCC[0].DocResStatus != undefined && $scope.ApplicationFlowGCC[0].DocResStatus != "") {
            let arr = ($scope.ApplicationFlowGCC[0].DocResStatus.toString()).split(":");
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].includes("Doc_DCS")) {
                    $scope.ApplicationFlowMaster.IsRecommendationOfDCSDOC_Path = (arr[i].split("-"))[1];
                }
                else if (arr[i].includes("Doc_PW")) {
                    $scope.ApplicationFlowMaster.IsDocumentOfProposedWork_Path = (arr[i].split("-"))[1];
                }
                else if (arr[i].includes("Doc_PB")) {
                    $scope.ApplicationFlowMaster.IsCopyOfDCSPassbook_Path = (arr[i].split("-"))[1];
                }
                else if (arr[i].includes("Doc_Jam")) {
                    $scope.ApplicationFlowMaster.IsCopyOfJamabandi = (arr[i].split("-"))[1];
                }
                else if (arr[i].includes("Doc_Lease")) {
                    $scope.ApplicationFlowMaster.ICopyOfLease = (arr[i].split("-"))[1];
                }
            }
        }

    }


    $scope.UploadReportFile = function (file) {
        $scope.GccReportFile = file[0];
    }
    $scope.saveDataInDB = function () {
        $scope.showLoader = true;       

      
        var DocumentCheck = "";
        if (($scope.ApplicationFlowMaster.IsRecommendationOfDCSDOC_Path != undefined && $scope.ApplicationFlowMaster.IsRecommendationOfDCSDOC_Path != '') && $scope.GopalCreditCardModel.RecommendationOfDCSDOC_Path != '') {
            DocumentCheck = DocumentCheck +"Doc_DCS-"+$scope.ApplicationFlowMaster.IsRecommendationOfDCSDOC_Path+":"
        }
        if (($scope.ApplicationFlowMaster.IsDocumentOfProposedWork_Path != undefined && $scope.ApplicationFlowMaster.IsDocumentOfProposedWork_Path != '') && $scope.GopalCreditCardModel.DocumentOfProposedWork_Path != '') {
            DocumentCheck = DocumentCheck + "Doc_PW-" + $scope.ApplicationFlowMaster.IsDocumentOfProposedWork_Path + ":"
        }
        if (($scope.ApplicationFlowMaster.IsCopyOfDCSPassbook_Path != undefined && $scope.ApplicationFlowMaster.IsCopyOfDCSPassbook_Path != '') && $scope.GopalCreditCardModel.CopyOfDCSPassbook_Path != '') {
            DocumentCheck = DocumentCheck + "Doc_PB-" + $scope.ApplicationFlowMaster.IsCopyOfDCSPassbook_Path + ":"
        }
        if (($scope.ApplicationFlowMaster.IsCopyOfJamabandi != undefined && $scope.ApplicationFlowMaster.IsCopyOfJamabandi != '') && $scope.gcc.JamabandiDoc != '') {
            DocumentCheck = DocumentCheck + "Doc_Jam-" + $scope.ApplicationFlowMaster.IsCopyOfJamabandi + ":"
        }
        if (($scope.ApplicationFlowMaster.ICopyOfLease != undefined && $scope.ApplicationFlowMaster.ICopyOfLease != '') && $scope.gcc.CopyOfLease != '') {
            DocumentCheck = DocumentCheck + "Doc_Lease-" + $scope.ApplicationFlowMaster.ICopyOfLease + ":"
        }

 
        //if ($scope.ReportGccFile == undefined && ($scope.ApplicationFlowMaster.StatusId == 16 || $scope.ApplicationFlowMaster.StatusId == 17)) {
        //    alert('en-us' ? '\n  Please Upload File(PDF)' : '\n  कृपया फ़ाइल अपलोड करें(पीडीएफ) |');
        //    return;
        //}
        //else if ($scope.ReportGccFile != undefined && ($scope.ApplicationFlowMaster.StatusId == 16 || $scope.ApplicationFlowMaster.StatusId == 17)) {
        //    $scope.UploadeReportFileInDB();
        //}

        $scope.GopalCreditCardFormModel = $scope.GopalCreditCardModel;

        if ($scope.ApplicationFlowMaster.StatusId == 16 || $scope.ApplicationFlowMaster.StatusId == 17) {
            if (!confirm((_language == 'en-us' ? '\n  Are you sure you want to reject this application ?' : '\n  क्या आप वाकई इस एप्लिकेशन को अस्वीकार करना चाहते हैं?'))){
                return;
            }
        }
                $(".se-pre-con").fadeIn("slow");
                $http.post(SiteUrl + "/gopalCreditCard/SaveGSSApplicationFlowStatus", { "OTP": $scope.ApplicationFlowMaster.OTP, "TXN": $scope.ApplicationFlowMaster.TXN, "GccId": $scope.GGG_Id, "Remark": $scope.ApplicationFlowMaster.Remark, "StatusId": $scope.ApplicationFlowMaster.StatusId, "UserId": $scope.ApplicationFlowMaster.UserId, "DocResStatus": DocumentCheck, "_Model": $scope.GopalCreditCardModel }).
                    success(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        if (data == 1) {                            
                            ShowOkMessage("Success", _language == 'en-us' ? 'Data saved successfully\n' : 'डेटा सहेजा गया है\n', function () { window.location.reload(); });
                           
                        }
                         else {
                            $(".se-pre-con").fadeOut("slow");                           
                                ShowOkMessage("Error", _language == 'en-us' ? 'Some Error is Occured \n' : 'कुछ त्रुटि हुई है \n');
                         
                        }
                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                        alert("Error while insert");
                    });

        $scope.showLoader = false;
    };

    $scope.ClearAppFlowForm = function () {
        $scope.ApplicationFlowMaster.Remark = '';
        $scope.ApplicationFlowMaster.StatusId = '0';       
        $scope.ReportGccFile = '';
        $scope.ShowOTPDiv = false;
        window.location = '../gopalCreditCard/ApplicationFlowGCC';

    }
    $scope.UploadeReportFileInDB = function () {
        var formData = new FormData();
        formData.append("GccReportFile", $scope.GccReportFile);
        formData.append("GCCId", $scope.GCC_Id);
        formData.append("RegistrationNo", $scope.GopalCreditCardModel.RegistrationNo);
        $http.post(SiteUrl + "/gopalCreditCard/SaveGCCVerifyReportFile", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
            }).
            error(function (data, status) {
            });
    }
    $scope.GetApplicationDetails = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl + "/GopalCreditCard/GetApplicationDetail?GccIdEnc=" + $scope.GopalCreditCardModel.GccIdDBEnc).
            success(function (data, status) {
                $(".se-pre-con").fadeOut("slow");
                if (data != null) {
                    $scope.objGCC = data.Table[0];
                    if (data.Table1[0] != null)
                        $scope.gcc = data.Table1[0];
                  //  $scope.gcc.GopalCreditCardIdEnc = $scope.GccIdEnc;
                    $scope.gcc.TypesOfProperty = data.Table1[0].TypesOfProperty?.toString();
                    $scope.gcc.RegistrationNo = $scope.objGCC.RegistrationNo;
                    $scope.gcc.EarlierLoanSanctionDate = new Date($scope.gcc.EarlierLoanSanctionDate);
                }
            }).
            error(function () {
                alert("Error while fetching GCC application data.");
                $(".se-pre-con").fadeOut("slow");
            });
    }
   
    $scope.checkInput = function (event) {
        var key = event.keyCode || event.which;
        var forbiddenKeys = [34, 39, 47]; // ASCII codes for ", ', and /

        if (forbiddenKeys.indexOf(key) !== -1) {
            event.preventDefault();
        }
    };

});

