
var MasterMod = angular.module('SocietyAmendmentModule', ['angularUtils.directives.dirPagination']);
MasterMod.controller("SocietyAmendmentCont", function ($scope, $http) {
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.ShowTab = false;
    $scope.ByLaws = [];
    $scope.ActId = 0;
    $scope.SocietyDirectory = "";
    $scope.SocietyIdEnc = "";
    $scope.MinimMemeber = 5;

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope._language = _language;
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

            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }
        $scope.showLoader = false;
        $scope.GetSocietyList();
        $('#txtAGMByeLawsDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $('#txtAGMHeldDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $('#txtBODByeLawsDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $('#txtByLawsAGMDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $('#txtBODMeetingHeldDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });
        $('#txtNameChangeAGMDate').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            maxDate: '0D'
        });

        //GET Financial Year
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=FINANCIALYEAR&MasterCode=-1").
            success(function (data, status) {
                $scope.FinancialYearList = data;
            }).
            error(function (data, status) {
                $scope.FinancialYearList = [];
            });
        //Get Category List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=CATEGORY&MasterCode=-1").
            success(function (data, status) {
                $scope.CategoryList = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });
        //Get Post List for dropdown
        $http.get(SiteUrl + "/Master/GetMasterData/?MasterType=POST&MasterCode=10").
            success(function (data, status) {
                $scope.Posts = data;
            }).
            error(function (data, status) {
                alert("Error while fetching Post list")

            });

        //$(".se-pre-con").fadeOut("slow");
    };

    // START :Get List of Societies
    $scope.GetSocietyList = function () {
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Citizen/GetSocietyList", { "WhereClause": " " }).
            success(function (data, status) {
                //$scope.SocietyList = data.filter(x => (x.IsSameCEOAddress == 'True') && (x.AddressFor == '0'));
                $scope.SocietyList = data;
                $(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                $scope.SocietyList = [];
                $(".blockUI").fadeOut("slow");
                alert('Error while fetching society list data.');
            });
    };
    // END :Get List of Societies

    $scope.OnChangeSociety = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.ShowTab = true;
        $scope.GetByLawsList();
        //$scope.GetAGM_BODMList();
        $(".se-pre-con").fadeOut("slow");
        // $scope.GetMemberList();
        //$scope.GetBalanceSheet();
        //$scope.BalanceSheetFileList();
        //OnTabClick(event, 'ByLaws');
    };
    // START :Get List of ByLaws List
    $scope.GetByLawsList = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.ResetByLawsModal();
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            // alert(SocietyIdEnc);
            $http.get("../Citizen/GetByLawsList?SocietyIdEnc=" + SocietyIdEnc).
                success(function (data, status) {
                    $scope.ByLawsListData = data;
                    $(".se-pre-con").fadeOut("slow");
                    //alert(JSON.stringify($scope.ByLawsListData));
                }).
                error(function (data, status) {
                    $scope.ByLawsListData = [];
                    $(".se-pre-con").fadeOut("slow");
                    alert('Error while fetching society list data.');
                });
        }
    };
    $scope.SocietyNameChangeList = [];
    $scope.GetSocietyNameChangeList = function () {
        $(".se-pre-con").fadeIn("slow");


        $scope.ResetByLawsModal();



        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            // alert(SocietyIdEnc);
            $http.get("../Citizen/GetSocietyNameChangeList?SocietyIdEnc=" + SocietyIdEnc).
                success(function (data, status) {
                    $scope.SocietyNameChangeList = data;
                    $(".se-pre-con").fadeOut("slow");
                    //alert(JSON.stringify($scope.ByLawsListData));
                }).
                error(function (data, status) {
                    $scope.ByLawsListData = [];
                    $(".se-pre-con").fadeOut("slow");
                    alert('Error while fetching society list data.');
                });
        }
    };

    // END :Get List of ByLaws List

    // START :Get List of AGM_BODM List
    $scope.GetAGM_BODMList = function () {
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            //alert(SocietyIdEnc);
            $http.get("../Citizen/GetAGM_BODMList?SocietyIdEnc=" + SocietyIdEnc).
                success(function (data, status) {
                    $scope.AGMListData = data.filter(x => x.Is_AGM_BODM == 'True');
                    $scope.BODMListData = data.filter(x => x.Is_AGM_BODM == 'False');
                }).
                error(function (data, status) {
                    $scope.BODMListData = [];
                    $scope.AGMListData = [];
                    alert('Error while fetching society list data.');
                });
        }
    };
    // END :Get List of AGM_BODM List

    // START :Get List of Member List
    $scope.GetMemberList = function () {
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        if (Datasplit.length > 2) {
            var SocietyIdEnc = Datasplit[0];
            var ActId = Datasplit[2];
            $scope.ActId = ActId;
            $scope.SocietyDirectory = Datasplit[1];
            $scope.SocietyIdEnc = SocietyIdEnc;
            if (ActId == 1) {
                $scope.MinimMemeber = 7;
            }
            else if (ActId == 3) {
                $scope.MinimMemeber = 15;
            }
            else if (ActId == 7) {
                $scope.MinimMemeber = 5;
            }
            $(".se-pre-con").fadeIn("slow");
            //alert(SocietyIdEnc);
            //$http.get("../Citizen/GetAGM_BODMList?SocietyIdEnc=" + SocietyIdEnc).
            $http.post(SiteUrl + "/Society/GetSocietyDetailsBySocietyId", { "SocietyId": SocietyIdEnc, "EMitraTransactionId": 0 }).
                success(function (data, status) {

                    var ALLMEMBERS = [];
                    var Nomi_ALLMEMBERS = [];
                    if (ActId != 7) {
                        ALLMEMBERS = data.filter(x => (x.IsSameCEOAddress == 'True') && (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS'));
                    }
                    else if (ActId == 7) {
                        ALLMEMBERS = data.filter(x => (x.IsSameCEOAddress == 'True') && (x.AddressFor == '0') && (x.MEMBERTYPE == 'GENERALMEMBERS' || x.MEMBERTYPE == 'ELECTEDMEMBERS'));
                    }
                    if (ActId == 3) {
                        Nomi_ALLMEMBERS = data.filter(x => (x.IsSameCEOAddress == 'True') && (x.MEMBERTYPE == 'NOMINATEDMEMBERS'));
                    }
                    var TableData10NomiAll = [];
                    var TableData10All = [];

                    var startID = 1;
                    var X = 7;
                    if (ActId == 1) {
                        X = (ALLMEMBERS.length > 6 ? ALLMEMBERS.length : 7)
                    }
                    else if (ActId == 7) {
                        X = (ALLMEMBERS.length > 4 ? ALLMEMBERS.length : 5);
                        startID = 1001;
                    }
                    else {
                        X = (ALLMEMBERS.length > 14 ? ALLMEMBERS.length : 15)
                    }
                    for (var x = 0; x < ALLMEMBERS.length; x++) {
                        TableData10All.push(
                            {
                                "Id": x + startID,
                                "Name": ALLMEMBERS[x].Name,
                                "POSTNAME": ALLMEMBERS[x].POSTNAME,
                                "Aadh": ALLMEMBERS[x].AadharNo,
                                "Gender": ALLMEMBERS[x].Gender == "F" ? 'FEMALE' : 'MALE',
                                "DOB": ALLMEMBERS[x].DOB,
                                "ContactNo": ALLMEMBERS[x].ContactNo,
                                "Photo": ALLMEMBERS[x].Photo,
                                "CategoryName": ALLMEMBERS[x].Category,
                                "Occupation": ALLMEMBERS[x].Occupation,
                                "CatId": ALLMEMBERS[x].CategoryId,
                                "Address": ALLMEMBERS[x].Address,
                                "Status": ALLMEMBERS[x].MEMBERTYPE == 'ELECTEDMEMBERS' ? 'true' : 'false',
                                "MEMBERTYPE": ALLMEMBERS[x].MEMBERTYPE
                            });
                    }

                    for (var x = ALLMEMBERS.length; x < X; x++) {
                        TableData10All.push(
                            {
                                "Id": x + startID,
                                "Name": '',
                                "POSTNAME": '',
                                "Aadh": '',
                                "Gender": '',
                                "DOB": '',
                                "ContactNo": '',
                                "Photo": '../Content/Images/Balnk.png',
                                "CategoryName": '',
                                "Occupation": '',
                                "Address": '',
                                "CatId": 3,
                                "Status": 'false',
                                "MEMBERTYPE": ''
                            });
                    }
                    if (ActId == 7) {
                        $scope.TableData10AllSports = TableData10All;
                        $scope.TableData10All = [];
                    }
                    else {
                        if (ALLMEMBERS.length > 0) {
                            $scope.SocietyAmendment.TotalMembersClassA = ALLMEMBERS[0].AdminDetailTotalMembersClassA;
                            $scope.SocietyAmendment.TotalMembersClassB = ALLMEMBERS[0].AdminDetailTotalMembersClassB;
                            $scope.SocietyAmendment.TotalMembersClassC = ALLMEMBERS[0].AdminDetailTotalMembersClassC;
                        }
                        $scope.TableData10All = TableData10All;
                        $scope.TableData10AllSports = [];
                    }


                    //Nominated Member
                    if (ActId == 3) {
                        for (var x = 0; x < Nomi_ALLMEMBERS.length; x++) {
                            TableData10NomiAll.push(
                                {
                                    "Id": x + 501,
                                    "Name": ALLMEMBERS[x].Name,
                                    "POSTNAME": ALLMEMBERS[x].POSTNAME,
                                    "Aadh": ALLMEMBERS[x].AadharNo,
                                    "Gender": ALLMEMBERS[x].Gender == "F" ? 'FEMALE' : 'MALE',
                                    "DOB": ALLMEMBERS[x].DOB,
                                    "ContactNo": ALLMEMBERS[x].ContactNo,
                                    "Photo": ALLMEMBERS[x].Photo,
                                    "CategoryName": ALLMEMBERS[x].Category,
                                    "Occupation": ALLMEMBERS[x].Occupation,
                                    "CatId": ALLMEMBERS[x].CategoryId,
                                    "Status": ALLMEMBERS[x].MEMBERTYPE == 'ELECTEDMEMBERS' ? 'true' : 'false',
                                    "MEMBERTYPE": ALLMEMBERS[x].NominatedBy,
                                    "NominatedBy": ALLMEMBERS[x].NOMINATEDBY
                                });
                        }
                        for (var x = Nomi_ALLMEMBERS.length; x < 1; x++) {
                            TableData10NomiAll.push(
                                {
                                    "Id": x + 501,
                                    "Name": '',
                                    "POSTNAME": '',
                                    "Aadh": '',
                                    "Gender": '',
                                    "DOB": '',
                                    "ContactNo": '',
                                    "Photo": '../Content/Images/Balnk.png',
                                    "CategoryName": '',
                                    "Occupation": '',
                                    "CatId": 3,
                                    "Status": 'false',
                                    "MEMBERTYPE": ''
                                });
                        }
                        $scope.TableData10NomiAll = TableData10NomiAll;
                    }
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                    ALLMEMBERS = [];
                    alert('Error while fetching society list data.');
                });
        }
    };
    // END :Get List of Member List

    $scope.SocietyMemberExcelList = [];
    $scope.GetSocietyNameChangeList = function () {
        $(".se-pre-con").fadeIn("slow");


        $scope.ResetByLawsModal();



        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            // alert(SocietyIdEnc);
            $http.get("../Citizen/GetChangeSocietyMemberExcelList?SocietyIdEnc=" + SocietyIdEnc).
                success(function (data, status) {
                    $scope.SocietyMemberExcelList = data;
                    $(".se-pre-con").fadeOut("slow");
                    //alert(JSON.stringify($scope.ByLawsListData));
                }).
                error(function (data, status) {
                    $scope.ByLawsListData = [];
                    $(".se-pre-con").fadeOut("slow");
                    alert('Error while fetching society list data.');
                });
        }
    };



    // START :Get List of BalanceSheet 
    $scope.GetBalanceSheet = function () {
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            var ActId = Datasplit[2];
            $http.get("../Citizen/GetBalanceSheetCurrent?SocietyIdEnc=" + SocietyIdEnc).
                success(function (data, status) {
                    $scope.BalanceSheetOld = data;
                }).
                error(function (data, status) {
                    $scope.BODMListData = [];
                    $scope.AGMListData = [];
                    alert('Error while fetching society list data.');
                });
        }
    };
    // END :Get List of BalanceSheet 
    // START :Get List of BalanceSheetFileListData
    $scope.BalanceSheetFileList = function () {
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        if (Datasplit.length > 1) {
            var SocietyIdEnc = Datasplit[0];
            $http.get("../Citizen/GetBalanceSheetAllList?SocietyIdEnc=" + SocietyIdEnc).
                success(function (data, status) {
                    $scope.BalanceSheetFileListData = data;
                }).
                error(function (data, status) {
                    $scope.BODMListData = [];
                    $scope.AGMListData = [];
                    alert('Error while fetching society list data.');
                });
        }
    };
    // END :Get List of BalanceSheetFileListData

    $scope.UploadBylawsFile = function (file) {
        $scope.ByeLawsSocietyFile = file[0];
    };
    $scope.UploadResolutionFile = function (file) {
        $scope.ResolutionFile = file[0];
    };
    $scope.UploadAGM1File = function (file) {
        $scope.AGM1File = file[0];
    };
    $scope.UploadAGM2File = function (file) {
        $scope.AGM2File = file[0];
    };
    $scope.UploadAGM3File = function (file) {
        $scope.AGM3File = file[0];
    };
    $scope.UploadAGM_PDF = function (file) {
        $scope.AGM_PDF = file[0];
    };
    $scope.UploadBSheetFileExl = function (file) {
        $scope.InsertBSheetFileExl = file[0];
    }
    $scope.UploadBODAGM_PDF = function (file) {
        $scope.BODAGM_PDF = file[0];
    };

    $scope.UploadMembersListExcelFile = function (file) {
        $scope.MembersListExcelFile = file[0];
    }


    $scope.SaveSocietyMemberExcel = function () {
        var Msg = '';       
        if ($scope.SocietyAmendment.RegiYearId == undefined || $scope.SocietyAmendment.RegiYearId == '0') {
            Msg += (_language == 'en-us' ? 'Please Select Registered Year \n' : 'पंजीकृत वर्ष का चयन करे\n');
            Msg += '<br />';
        }
        if ($scope.MembersListExcelFile == undefined || $scope.MembersListExcelFile == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload Valid Member Excel Sheet.' : '\nकृपया मान्य सदस्य एक्सेल शीट अपलोड करें ।';
            Msg += '<br />';
        }
        if (Msg.length > 1) {
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + '<br />' + Msg;
            ShowModelAlert("Society Amendment Details", Msg);
            return;
        }
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        formData.append("SocietyIdEnc", Datasplit[0]);
        formData.append("SocietyDirectory", Datasplit[1]);
        formData.append("RegiYearId", $scope.SocietyAmendment.RegiYearId);
        formData.append("AGM3File", $scope.MembersListExcelFile);
        formData.append("Remark", $scope.SocietyAmendment.Remark == undefined ? "" : $scope.SocietyAmendment.Remark);
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Citizen/SaveSocietyMemberExcel", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");

                $("#InnerModelPopup").addClass("alert alert-danger fade in");
                if (data.data.indexOf("ErrorInMemberdata") > -1) {
                    ShowModelAlert("Error", "<b>Error In Uploaded Members Excel. All Column values is mandatory.Please fill correct values and re-upload. </b><br/><br/>" + data.data.split('@')[1]);
                    return;
                }
                var AltMsg = "";
                var Result = parseInt(data.data, 10);
                if (Result > 0) {
                    AltMsg = (_language == 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                    ShowOkMessage("", AltMsg, function () {
                        location.reload();
                    });
                  
                }
                else if (Result == -2) {
                    AltMsg = (_language == 'en-us' ? 'Data Already Exist.' : 'डेटा पहले से मौजूद है');
                    ShowOkMessage("Error", AltMsg, function () { });
                    return;
                }
                else {
                    AltMsg = (_language == 'en-us' ? 'Some Error Occurred please try after some time.' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                    ShowOkMessage("Error", AltMsg, function () { });
                    return;
                }
                //alert(AltMsg);
                
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                //alert('An Error Occurred Please Try Again Later');
                ShowOkMessage("Error", 'An Error Occurred Please Try Again Later', function () { });
                location.reload();
            });
    };



    $scope.SaveByLaws = function () {
        var Msg = '';

        if ($scope.SocietyAmendment.SocietyIdEnc == undefined || $scope.SocietyAmendment.SocietyIdEnc == '0') {
            Msg += (_language == 'en-us' ? 'Please Select Society \n' : 'सोसाइटी का चयन करे\n');
            Msg += '<br />';
        }
        if ($scope.SocietyAmendment.RegiYearId == undefined || $scope.SocietyAmendment.RegiYearId == '0') {
            Msg += (_language == 'en-us' ? 'Please Select Registered Year \n' : 'पंजीकृत वर्ष का चयन करे\n');
            Msg += '<br />';
        }
        if ($scope.SocietyAmendment.ResolutionNo == undefined || $scope.SocietyAmendment.ResolutionNo == '') {
            Msg += (_language == 'en-us' ? 'Please Enter ResolutionNo \n' : 'संकल्प नं दर्ज करे\n');
            Msg += '<br />';
        }
        if ($("#txtByLawsAGMDate").val() == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Paased in Which AGM Date  \n' : 'जिसमें पास किया गया एजीएम दिनांक दर्ज करे\n');
            Msg += '<br />';
        }
        if ($scope.ByeLawsSocietyFile == undefined || $scope.ByeLawsSocietyFile == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload ByeLaws File(PDF)' : '\nकृपया बाय लॉ अपलोड करें(पीडीएफ) |';
            Msg += '<br />';
        }
        //if ($scope.ResolutionFile == undefined) {
        //    Msg += $scope._language == 'en-us' ? '\nPlease Upload Resolution File(PDF)' : '\nकृपया संकल्प पीडीएफ अपलोड करें(पीडीएफ) |';
        //    Msg += '<br />';
        //}
        if ($scope.AGM1File == undefined || $scope.AGM1File == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload AGM I File(PDF)' : '\nकृपया एजीएम I पीडीएफ अपलोड करें(पीडीएफ) |';
            Msg += '<br />';
        }
        if ($scope.AGM2File == undefined || $scope.AGM2File == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload AGM II File(PDF)' : '\nकृपया एजीएम II पीडीएफ अपलोड करें(पीडीएफ) |';
            Msg += '<br />';
        }
        if ($scope.AGM3File == undefined || $scope.AGM3File == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload AGM III (PDF)' : '\nकृपया एजीएम III पीडीएफ अपलोड करें(पीडीएफ) |';
            Msg += '<br />';
        }
        if (Msg.length > 1) {
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + '<br />' + Msg;
            ShowModelAlert("Society Amendment Details", Msg);
            return;
        }
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        formData.append("SocietyIdEnc", Datasplit[0]);
        formData.append("SocietyDirectory", Datasplit[1]);
        formData.append("RegiYearId", $scope.SocietyAmendment.RegiYearId);
        formData.append("ResolutionNo", $scope.SocietyAmendment.ResolutionNo);
        formData.append("PaasedAGMDate", $("#txtByLawsAGMDate").val().split("/").reverse().join("-"));
        formData.append("ByeLawsFile", $scope.ByeLawsSocietyFile);
        //formData.append("ResolutionFile", $scope.ResolutionFile);
        formData.append("AGM1File", $scope.AGM1File);
        formData.append("AGM2File", $scope.AGM2File);
        formData.append("AGM3File", $scope.AGM3File);
        formData.append("Remark", $scope.SocietyAmendment.Remark == undefined ? "" : $scope.SocietyAmendment.Remark);
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Citizen/InsertByLaws", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                var AltMsg = "";
                var Result = parseInt(data, 10);
                if (Result > 0) {
                    AltMsg = (_language == 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                }
                else if (Result == -2) {
                    AltMsg = (_language == 'en-us' ? 'Data Already Exist.' : 'डेटा पहले से मौजूद है');
                }
                else {
                    AltMsg = (_language == 'en-us' ? 'Some Error Occurred please try after some time.' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                }
                alert(AltMsg);
                location.reload();
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                alert('An Error Occurred Please Try Again Later');
                location.reload();
            });
    };

    $scope.ResetByLawsModal = function () {

        $scope.SocietyAmendment.RegiYearId = '0';
        $scope.SocietyAmendment.ResolutionNo = '';
        $scope.SocietyAmendment.Name = '';
        $("#txtByLawsAGMDate").val('');
        $scope.ByeLawsSocietyFile = '';
        $scope.AGM1File = '';
        $scope.AGM2File = '';
        $scope.AGM3File = '';
        $('#flNameAGM1PDF').val('');
        $('#flNameAGM2PDF').val('');
        $('#flNameAGM3PDF').val('');
        $('#flAGM1PDF').val('');
        $('#flAGM2PDF').val('');
        $('#flAGM3PDF').val('');
        $("#txtNameChangeAGMDate").val("");
        $('span[class="filesize"]').html("");
        $('#flMembersListExcelFile').val("");
        $scope.MembersListExcelFile = '';
    }

    $scope.SaveSocietyNameChange = function () {
        var Msg = '';
      
        if ($scope.SocietyAmendment.SocietyIdEnc == undefined || $scope.SocietyAmendment.SocietyIdEnc == '0') {
            Msg += (_language == 'en-us' ? 'Please Select Society \n' : 'सोसाइटी का चयन करे\n');
            Msg += '<br />';
        }
        if ($scope.SocietyAmendment.RegiYearId == undefined || $scope.SocietyAmendment.RegiYearId == '0') {
            Msg += (_language == 'en-us' ? 'Please Select Registered Year \n' : 'पंजीकृत वर्ष का चयन करे\n');
            Msg += '<br />';
        }
        if ($scope.SocietyAmendment.ResolutionNo == undefined || $scope.SocietyAmendment.ResolutionNo == '') {
            Msg += (_language == 'en-us' ? 'Please Enter ResolutionNo \n' : 'संकल्प नं दर्ज करे\n');
            Msg += '<br />';
        }
        if ($scope.SocietyAmendment.Name == undefined || $scope.SocietyAmendment.Name == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Name \n' : 'नाम दर्ज करे\n');
            Msg += '<br />';
        }
        if ($("#txtNameChangeAGMDate").val() == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Paased in Which AGM Date  \n' : 'जिसमें पास किया गया एजीएम दिनांक दर्ज करे\n');
            Msg += '<br />';
        }
        if ($scope.ByeLawsSocietyFile == undefined || $scope.ByeLawsSocietyFile == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload ByeLaws File(PDF)' : '\nकृपया बाय लॉ अपलोड करें(पीडीएफ) |';
            Msg += '<br />';
        }
        //if ($scope.ResolutionFile == undefined) {
        //    Msg += $scope._language == 'en-us' ? '\nPlease Upload Resolution File(PDF)' : '\nकृपया संकल्प पीडीएफ अपलोड करें(पीडीएफ) |';
        //    Msg += '<br />';
        //}
        if ($scope.AGM1File == undefined || $scope.AGM1File == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload AGM I File(PDF)' : '\nकृपया एजीएम I पीडीएफ अपलोड करें(पीडीएफ) |';
            Msg += '<br />';
        }
        if ($scope.AGM2File == undefined || $scope.AGM2File == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload AGM II File(PDF)' : '\nकृपया एजीएम II पीडीएफ अपलोड करें(पीडीएफ) |';
            Msg += '<br />';
        }
        if ($scope.AGM3File == undefined || $scope.AGM3File == '') {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload AGM III (PDF)' : '\nकृपया एजीएम III पीडीएफ अपलोड करें(पीडीएफ) |';
            Msg += '<br />';
        }
        if (Msg.length > 1) {
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + '<br />' + Msg;
            ShowModelAlert("Society Amendment Details", Msg);
            return;
        }
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        formData.append("SocietyIdEnc", Datasplit[0]);
        formData.append("SocietyDirectory", Datasplit[1]);
        formData.append("RegiYearId", $scope.SocietyAmendment.RegiYearId);
        formData.append("ResolutionNo", $scope.SocietyAmendment.ResolutionNo);
        formData.append("PaasedAGMDate", $("#txtByLawsAGMDate").val().split("/").reverse().join("-"));
        formData.append("ByeLawsFile", $scope.ByeLawsSocietyFile);
        //formData.append("ResolutionFile", $scope.ResolutionFile);
        formData.append("AGM1File", $scope.AGM1File);
        formData.append("AGM2File", $scope.AGM2File);
        formData.append("AGM3File", $scope.AGM3File);
        formData.append("Name", $scope.SocietyAmendment.Name);
        formData.append("Remark", $scope.SocietyAmendment.Remark == undefined ? "" : $scope.SocietyAmendment.Remark);
        $(".blockUI").fadeIn("slow");
        $http.post(SiteUrl + "/Citizen/InsertSocietyNameChange", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                $(".blockUI").fadeOut("slow");
                var AltMsg = "";
                var Result = parseInt(data, 10);
                if (Result > 0) {
                    AltMsg = (_language == 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                }
                else if (Result == -2) {
                    AltMsg = (_language == 'en-us' ? 'Data Already Exist.' : 'डेटा पहले से मौजूद है');
                }
                else {
                    AltMsg = (_language == 'en-us' ? 'Some Error Occurred please try after some time.' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                }
                alert(AltMsg);
                location.reload();
            }).
            error(function (data, status) {
                $(".blockUI").fadeOut("slow");
                alert('An Error Occurred Please Try Again Later');
                location.reload();
            });
    };
    

    $scope.SaveAGM = function () {
        var Msg = '';

        if ($scope.SocietyAmendment.SocietyIdEnc == undefined || $scope.SocietyAmendment.SocietyIdEnc == '0') {
            Msg += (_language == 'en-us' ? 'Please Select Society \n' : 'सोसाइटी  का चयन करे\n');
            Msg += '<br />';
        }
        if ($("#txtAGMByeLawsDate").val() == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Mandated Date as Per Bye Laws  \n' : 'बाय लॉ की दिनांक दर्ज करे\n');
            Msg += '<br />';
        }
        if ($("#txtAGMHeldDate").val() == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Date on Which Meeting Held  \n' : 'बैठक की दिनांक दर्ज करे\n');
            Msg += '<br />';
        }

        if ($scope.AGM_PDF == undefined) {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload Meeting PDF' : '\nकृपया बैठक की पीडीएफ अपलोड करें |';
            Msg += '<br />';
        }
        if (Msg.length > 1) {
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + '<br />' + Msg;
            ShowModelAlert("Society Amendment Details", Msg);
            return;
        }
        // $scope.SocietyAmendment.Is_AGM_BODM = true;
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        formData.append("SocietyIdEnc", Datasplit[0]);
        formData.append("SocietyDirectory", Datasplit[1]);
        formData.append("ByeLawsDate", $("#txtAGMByeLawsDate").val().split("/").reverse().join("-"));
        formData.append("MeetingDate", $("#txtAGMHeldDate").val().split("/").reverse().join("-"));
        formData.append("MeetingPDF", $scope.AGM_PDF);
        formData.append("Is_AGM_BODM", true);
        formData.append("Remark", $scope.SocietyAmendment.Remark);

        $http.post(SiteUrl + "/Citizen/InsertAGM_BODM", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                var AltMsg = "";
                var Result = parseInt(data, 10);
                if (Result > 0) {
                    AltMsg = (_language == 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                }
                else if (Result == -2) {
                    AltMsg = (_language == 'en-us' ? 'Data Already Exist.' : 'डेटा पहले से मौजूद है');
                }
                else {
                    AltMsg = (_language == 'en-us' ? 'Some Error Occurred please try after some time.' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                }
                alert(AltMsg);
                location.reload();
            }).
            error(function (data, status) {
                alert('An Error Occurred Please Try Again Later');
                location.reload();
            });
    };

    $scope.SaveBODAM = function () {
        var Msg = '';

        if ($scope.SocietyAmendment.SocietyIdEnc == undefined || $scope.SocietyAmendment.SocietyIdEnc == '0') {
            Msg += (_language == 'en-us' ? 'Please Select Society \n' : 'सोसाइटी का चयन करे\n');
            Msg += '<br />';
        }
        if ($("#txtBODByeLawsDate").val() == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Mandated Date as Per Bye Laws  \n' : 'बाय लॉ की दिनांक दर्ज करे\n');
            Msg += '<br />';
        }
        if ($("#txtBODMeetingHeldDate").val() == '') {
            Msg += (_language == 'en-us' ? 'Please Enter Date on Which Meeting Held  \n' : 'बैठक की दिनांक दर्ज करे\n');
            Msg += '<br />';
        }

        if ($scope.BODAGM_PDF == undefined) {
            Msg += $scope._language == 'en-us' ? '\nPlease Upload Meeting PDF' : '\nकृपया बैठक की पीडीएफ अपलोड करें |';
            Msg += '<br />';
        }
        if (Msg.length > 1) {
            Msg = (_language == 'en-us' ? 'Please validate following :- \n' : 'कृपया निम्नलिखित मान्य करें : - \n') + '<br />' + Msg;
            ShowModelAlert("Society Amendment Details", Msg);
            return;
        }
        // $scope.SocietyAmendment.Is_AGM_BODM = true;
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        //alert($("#txtBODByeLawsDate").val() + "-----" + $("#txtBODMeetingHeldDate").val());
        var formData = new FormData();
        formData.append("SocietyIdEnc", Datasplit[0]);
        formData.append("SocietyDirectory", Datasplit[1]);
        formData.append("ByeLawsDate", $("#txtBODByeLawsDate").val().split("/").reverse().join("-"));
        formData.append("MeetingDate", $("#txtBODMeetingHeldDate").val().split("/").reverse().join("-"));
        formData.append("MeetingPDF", $scope.BODAGM_PDF);
        formData.append("Is_AGM_BODM", false);
        formData.append("Remark", $scope.SocietyAmendment.Remark);

        $http.post(SiteUrl + "/Citizen/InsertAGM_BODM", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                var AltMsg = "";
                var Result = parseInt(data, 10);
                if (Result > 0) {
                    AltMsg = (_language == 'en-us' ? 'Data saved successfully.' : 'डेटा सफलतापूर्वक सहेजा गया');
                }
                else if (Result == -2) {
                    AltMsg = (_language == 'en-us' ? 'Data Already Exist.' : 'डेटा पहले से मौजूद है');
                }
                else {
                    AltMsg = (_language == 'en-us' ? 'Some Error Occurred please try after some time.' : 'कुछ त्रुटि हुई कुछ समय बाद कोशिश करें');
                }
                alert(AltMsg);
                location.reload();
            }).
            error(function (data, status) {
                alert('An Error Occurred Please Try Again Later');
                location.reload();
            });
    };

    $scope.SaveMemberList = function () {
        if ($scope.ActId != "7") {
            
            var SocietyRegistrationData = [];
            var formData = new FormData();
            formData.append("ActId", $scope.ActId);
            // formData.append("EMitraTransactionId", $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId);
            formData.append("ClassA", $scope.SocietyAmendment.TotalMembersClassA);
            formData.append("ClassB", $scope.SocietyAmendment.TotalMembersClassB);
            formData.append("ClassC", $scope.SocietyAmendment.TotalMembersClassC);
            formData.append("SocietyDirectory", $scope.SocietyDirectory);
            $scope.IsAdministrative = 'Board';
            $scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdEnc;
            //if ($scope.SocietyRegistration.IsAdministratorAppointed == 'true' && $scope.SocietyRegistration.AppointedCategoryId > 0 && $scope.SocietyRegistration.AppointedPostId > 0) {
            //    SocietyRegistrationData.push(
            //        {
            //            //"SocietyId": $scope.SocietyIdForAdministrativeDetails,
            //            "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
            //            "Name": $scope.SocietyRegistration.AppointedName,
            //            "IsActive": true,
            //            "ContactNo": $scope.SocietyRegistration.AppointedContactNo,
            //            "AADHAARNo": $scope.SocietyRegistration.AppointedAADHAARNo,
            //            "DOB": $('#txtAppointedDOB').val().split("/").reverse().join("-"),
            //            "Gender": $scope.SocietyRegistration.AppointedGender,
            //            "UserType": "3",
            //            //"AdministratorDesignationId": $scope.SocietyRegistration.DESIGNATIONCODE,
            //            "AdministratorDesignationId": "-1",
            //            "CategoryId": $scope.SocietyRegistration.AppointedCategoryId,
            //            "PostId": $scope.SocietyRegistration.AppointedPostId,
            //            "Photo": $scope.SocietyRegistration.AppointedFile,
            //            "MEMBERTYPE": "ELECTEDMEMBERS"
            //        });
            //}
            //if ($scope.SocietyRegistration.IsAdministrative == 'liquidator') {
            //    var LiqPh = $scope.SocietyRegistration.liquidatorFile.split('/');
            //    if (LiqPh.length > 3) {
            //        $scope.SocietyRegistration.liquidatorFile = LiqPh[3];
            //    }
            //    SocietyRegistrationData.push(
            //        {
            //            //"SocietyId": $scope.SocietyIdForAdministrativeDetails,
            //            "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
            //            "Name": $scope.SocietyRegistration.liquidatorName,
            //            "IsActive": true,
            //            "ContactNo": $scope.SocietyRegistration.liquidatorContactNo,
            //            "AADHAARNo": $scope.SocietyRegistration.liquidatorAADHAARNo,
            //            "DOB": $('#txtliquidatorDOB').val().split("/").reverse().join("-"),
            //            "AppointedDate": $('#txtliquidatorAppointedDate').val().split("/").reverse().join("-"),
            //            "Gender": $scope.SocietyRegistration.liquidatorGender,
            //            "UserType": "4",
            //            "AdministratorDesignationId": "-1",
            //            "CategoryId": $scope.SocietyRegistration.liquidatorCategoryId,
            //            "PostId": $scope.SocietyRegistration.liquidatorPostId,
            //            "Photo": $scope.SocietyRegistration.liquidatorFile
            //        });
            //}
            if ($scope.IsAdministrative == 'Board') {
                var Count = 0;
                $('#TblElectedMember tr').each(function () {
                    if ($(this).find('td:eq(7)').find('input').val() != '' && $(this).find('td:eq(6)').find('select').val() > 0) {
                        SocietyRegistrationData.push(
                            {
                                "AADHAARNo": $(this).find('td:eq(1)').find('input').val(),
                                "Name": $(this).find('td:eq(2)').find('input').val(),
                                "ContactNo": $(this).find('td:eq(3)').find('input').val(),
                                "DOB": $(this).find('td:eq(4)').find('input').val().split("/").reverse().join("-"),
                                "Gender": ($(this).find('td:eq(5)').find('label').html()).charAt(0),//text
                                "CategoryId": $(this).find('td:eq(6)').find('select').val(),
                                "PostId": '0',
                                "PostName": $(this).find('td:eq(7)').find('input').val(),
                                "Photo": ($(this).find('td:eq(8)').find('img'))[0].src,
                                "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                "IsActive": true,
                                "UserType": "4",//Elected board Menbers
                                "AdministratorDesignationId": "-1",
                                "Occupation": $(this).find('td:eq(9)').find('input').val(),
                                "MEMBERTYPE": ($(this).find('td:eq(10)').find('input').prop('checked') ? "ELECTEDMEMBERS" : 'GENERALMEMBERS')

                            });
                    }
                    //SocietyRegistrationData.push({ "SocietyId": $scope.SocietyIdForAdministrativeDetails, "Name": $(this).find('td:eq(1)').find('input').val(), "ContactNo": $(this).find('td:eq(2)').find('input').val(), "UserType": "3" });
                });

                $('#TblNominatedMember tr').each(function () {
                    if ($(this).find('td:eq(1)').find('input').val() != '' && $(this).find('td:eq(6)').find('select').val() > 0 && $(this).find('td:eq(7)').find('input').val() != '' && $(this).find('td:eq(8)').find('select').val() > 0) {
                        SocietyRegistrationData.push(
                            {
                                "AADHAARNo": $(this).find('td:eq(1)').find('input').val(),
                                "Name": $(this).find('td:eq(2)').find('input').val(),
                                "ContactNo": $(this).find('td:eq(3)').find('input').val(),
                                "DOB": $(this).find('td:eq(4)').find('input').val().split("/").reverse().join("-"),
                                "Gender": ($(this).find('td:eq(5)').find('label').html()).charAt(0),//text
                                "CategoryId": $(this).find('td:eq(6)').find('select').val(),
                                "PostId": '0',
                                "PostName": $(this).find('td:eq(7)').find('input').val(),
                                "NominatedBy": $(this).find('td:eq(8)').find('select').val(),
                                "Photo": ($(this).find('td:eq(9)').find('img'))[0].src,
                                "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                "IsActive": true,
                                "UserType": "5",//Nominated board Menbers
                                "AdministratorDesignationId": "-1",
                                "MEMBERTYPE": "NOMINATEDMEMBERS"
                            });
                    }

                });
            }
        }
        else {

            if ($scope.ValidateNominatedMember(1, true)) {
                var SocietyRegistrationData = [];
                var formData = new FormData();
                formData.append("ActId", $scope.ActId);
                //formData.append("EMitraTransactionId", $scope.EMitraTransactionId == undefined ? 0 : $scope.EMitraTransactionId);



                var formDataLocal = new FormData();

                formData.append("ClassA", 0);
                formData.append("ClassB", 0);
                formData.append("ClassC", 0);
                formData.append("SocietyDirectory", $scope.SocietyDirectory);
                //अध्यक्ष
                $scope.SocietyIdForAdministrativeDetails = $scope.SocietyIdEnc;;
                $('#TblSportElectedMember tr').each(function () {
                    if ($(this).find('td:eq(1)').find('input').val() != '' && $(this).find('td:eq(6)').find('select').val() > 0) {
                        //  alert('IN');
                        SocietyRegistrationData.push(
                            {
                                "AADHAARNo": $(this).find('td:eq(1)').find('input').val(),
                                "Name": $(this).find('td:eq(2)').find('input').val(),
                                "ContactNo": $(this).find('td:eq(3)').find('input').val(),
                                "DOB": $(this).find('td:eq(4)').find('input').val().split("/").reverse().join("-"),
                                "Gender": ($(this).find('td:eq(5)').find('label').html()).charAt(0),//text
                                "CategoryId": $(this).find('td:eq(6)').find('select').val(),
                                "PostId": '0',
                                "PostName": $(this).find('td:eq(7)').find('input').val(),
                                "Address": $(this).find('td:eq(8)').find('textarea').val(),
                                "Photo": ($(this).find('td:eq(9)').find('img'))[0].src,
                                "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                                "IsActive": true,
                                "UserType": "4",//Elected board Menbers
                                "AdministratorDesignationId": "-1",
                                "MEMBERTYPE": ($(this).find('td:eq(10)').find('input').prop('checked') ? "ELECTEDMEMBERS" : 'GENERALMEMBERS')
                            });
                    }
                });
                //$('#SportTableAdd tr').each(function () {
                //    SocietyRegistrationData.push(
                //        {

                //            //"SocietyId": $scope.SocietyIdForAdministrativeDetails,
                //            "SocietyIdEnc": $scope.SocietyIdForAdministrativeDetails,
                //            "Name": $(this).find('td:eq(1)').find('input').val(),
                //            "IsActive": true,
                //            "ContactNo": $(this).find('td:eq(2)').find('input').val(),
                //            "AADHAARNo": $(this).find('td:eq(3)').find('input').val(),
                //            "DOB": $(this).find('td:eq(4)').find('input').val().split("/").reverse().join("-"),
                //            "Gender": ($(this).find('td:eq(5)').find('label').html()).charAt(0),//text
                //            "CategoryId": $(this).find('td:eq(6)').find('select').val() > 0 ? $(this).find('td:eq(6)').find('select').val() : 0,
                //            "PostId": $(this).find('td:eq(7)').find('select').val() > 0 ? $(this).find('td:eq(7)').find('select').val() : 0,
                //            "Address": '',
                //            "UserType": "5",//Nominated member
                //            "AdministratorDesignationId": "-1",
                //            "Photo": ($(this).find('td:eq(8)').find('img'))[0].src,

                //        });
                //    //SocietyRegistrationData.push({ "SocietyId": $scope.SocietyIdForAdministrativeDetails, "Name": $(this).find('td:eq(1)').find('input').val(), "ContactNo": $(this).find('td:eq(2)').find('input').val(), "UserType": "3" });
                //});
            }
        }
        formData.append("DataArray", JSON.stringify(SocietyRegistrationData));
        $http.post(SiteUrl + "/Society/SaveSocietyAdministrativeDetailsAmendment", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                ShowModelAlert("Society Amendment Details", "Saved Successfully");
            }).
            error(function (data, status) {
                $scope.DistrictList = [];
            });


    }







    //start aadhar verify
    $scope.Otpsend = function () {
        if ($('#SSOAuth_chkConsent').prop("checked")) {
            $scope.messagebox = false;
            $scope.FillOtp = true
            //SEND OTP
            $scope.GenrateOTP();
        }
        else {
            $scope.messagebox = true;
            $scope.FillOtp = false;
        }


    };
    $scope.ClosePopUp = function () {
        $scope.messagebox = false;

    };
    $scope.CloseOtpBox = function () {
        $scope.messagebox = false;
        $scope.OtpDilogBox = false;
        $scope.FillOtp = false;
        $('#ssoauthloading').hide();
        $("#SSOAuth_chkConsent").prop("checked", false);
    };
    $scope.OpenOtpPopUp = function (AADHAARNo, FOR, TRID) {
        if (AADHAARNo.length < 12 || AADHAARNo.length > 12) {
            alert('Invalid Aadhaar Number');
            return;
        }
        var arrylist = AADHAARNo.split('');
        $scope.MaskingAADHAARNo = arrylist[0] + arrylist[1] + 'XXXXXXXX' + arrylist[10] + arrylist[11];
        $scope.AADHAARNo = AADHAARNo;
        $scope.messagebox = false;
        $scope.OtpDilogBox = true;
        $scope.FillOtp = false;
        $scope.FOR = FOR;
        $scope.CurrentRow = TRID;
        if ($scope.FOR == 'CEO') {
            $('#txtCEOAADHAARNo').attr("disabled", "disabled");
        }
        if ($scope.FOR == 'CHAIRMAN') {
            $('#txtChairmanAADHAARNo').attr("disabled", "disabled");
        }
        if ($scope.FOR == 'APPOINTED') {
            $('#txtAppointedAADHAARNo').attr("disabled", "disabled");
        }
        if ($scope.FOR == 'lIQUIDATOR') {
            $('#txtliquidatorAADHAARNo').attr("disabled", "disabled");
        }
        if ($scope.FOR == 'SPCHAIRMAN') {
            $('#txtSPCEOAADHAARNo').attr("disabled", "disabled");
        }
        if ($scope.FOR == 'SPSECRETARY') {
            $('#txtSecretaryAadhar').attr("disabled", "disabled");
        }
        if ($scope.FOR == 'TREASURER') {
            $('#txtTreasurerAadhar').attr("disabled", "disabled");
        }

    };
    $scope.OpenOtpPopUpOther = function (AADHAARNo, FOR, TRID) {
        // alert(AADHAARNo+"--"+ FOR+"--"+ TRID)
        //  alert($('#' + TRID).find('td:eq(1)').find('input').val());
        $('#OTPBOX').css("display", "block");
        if ($('#' + TRID).find('td:eq(1)').find('input').val().length < 12 || $('#' + TRID).find('td:eq(1)').find('input').val().length > 12) {
            alert('Invalid Aadhaar Number');
            return;
        }
        $("#SSOAuth_chkConsent").prop("checked", false);
        $('#SSOAuth_OtpText').val('');
        $('#ssoauthloading').hide();
        var arrylist = $('#' + TRID).find('td:eq(1)').find('input').val().split('');

        $scope.MaskingAADHAARNo = arrylist[0] + arrylist[1] + 'XXXXXXXX' + arrylist[10] + arrylist[11];
        $scope.AADHAARNo = $('#' + TRID).find('td:eq(1)').find('input').val();
        var num = $scope.AADHAARNo;
        if (isNaN(num)) {
            alert("Please enter only number in aadhar.");
            return;
            //document.write(num1 + " is not a number <br/>");
        } else {
            //document.write(num1 + " is a number <br/>");
        }
        $scope.messagebox = false;
        $scope.OtpDilogBox = true;
        $scope.FillOtp = false;
        $scope.FOR = FOR;
        $scope.CurrentRow = TRID;
    };
    $scope.OpenOtpPopUpOtherFun = function () {
        $("#SSOAuth_chkConsent").prop("checked", false);
        $('#SSOAuth_OtpText').val('');
        $('#ssoauthloading').hide();
        var TRID = $("#hdRowId").val();
        // alert(TRID)
        var arrylist = $('#' + TRID).find('td:eq(1)').find('input').val().split('');
        //   alert(arrylist.length)
        if ($('#' + TRID).find('td:eq(1)').find('input').val().length < 12 || $('#' + TRID).find('td:eq(1)').find('input').val().length > 12) {
            alert('Invalid Aadhaar Number');
            return;
        }
        $scope.MaskingAADHAARNo = arrylist[0] + arrylist[1] + 'XXXXXXXX' + arrylist[10] + arrylist[11];
        $scope.AADHAARNo = $('#' + TRID).find('td:eq(1)').find('input').val();
        $scope.messagebox = false;
        $scope.OtpDilogBox = true;
        $scope.FillOtp = false;
        $scope.CurrentRow = TRID;
    };
    $scope.GenrateOTP = function () {
        $('#ssoauthloading').show();
        $http.get(SiteUrl + "/Citizen/GenrateOTPdata/?_AadharNo=" + $scope.AADHAARNo).
            success(function (data, status) {
                if (data[0].txnid.length > 0) {
                    $scope.OTPStatus = true;
                    if (data[0].txnid.includes("DOITRJGOVAUTH") && data[0].txnid.split('#')[0] == "NO") {
                        $('#btnSSOAuth_SendOTP').hide();
                        $('#btnSSOAuth_ValidateOTP').show();
                        $scope.txn = data[0].txnid.split('#')[1];
                        $('#ssoauthloading').hide();
                    }
                    else if (data[0].txnid.split('#')[0] == "NO") {
                        alert(data[0].txnid.split('#')[1]);
                        $('#ssoauthloading').hide();
                    }
                    else {
                        $('#ssoauthloading').hide();
                        $('#btnSSOAuth_SendOTP').hide();
                        $('#btnSSOAuth_ValidateOTP').show();
                        $scope.txn = data[0].txnid;
                        //  $scope.Showgride = false;                       
                    }
                }
                else {
                    alert('Some Error Occured Please Try Again');
                    $('#ssoauthloading').hide();
                }
            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
            });
    }
    $scope.ValidateOTP = function () {
        $('#ssoauthloading').show();
        $http.get(SiteUrl + "/Citizen/ValidateOTP/?_AadharNo=" + $scope.AADHAARNo + "&otp=" + $('#SSOAuth_OtpText').val() + "&txn=" + $scope.txn).
            success(function (data, status) {
                if (data[0].Column1.length > 0) {
                    if (data[0].Column1 == "Failed") {
                        $('#ssoauthloading').hide();
                        $scope.AADHAARNo = '';
                        $('#SSOAuth_OtpText').val('');
                        $scope.txn = '';
                        alert(data[0].Column2);
                        $('#btnSSOAuth_SendOTP').show();
                        $('#btnSSOAuth_ValidateOTP').hide();
                    }
                    else if (data[0].Column1 == "Success") {
                        $('#ssoauthloading').hide();
                        $scope.txn = '';
                        $scope.AADHAARNo = '';
                        $('#SSOAuth_OtpText').val('');
                        $('#btnSSOAuth_SendOTP').show();
                        $('#btnSSOAuth_ValidateOTP').hide();
                        if ($scope.FOR == 'CEO') {
                            $scope.SocietyRegistration.CEOName = data[0].Column2;
                            $scope.SocietyRegistration.CEOGender = data[0].Column3;
                            $scope.SocietyRegistration.CEODOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.CEOSocietyFile = data[0].Column5;
                            $scope.SocietyRegistration.CEOSocietyFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.CEOProfile = true;
                            $scope.CEORemove = true;
                            $scope.GETCEO = false;
                        }
                        else if ($scope.FOR == 'CHAIRMAN') {
                            $scope.SocietyRegistration.ChairmanName = data[0].Column2;
                            $scope.SocietyRegistration.ChairmanGender = data[0].Column3;
                            $scope.SocietyRegistration.ChairmanDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.ChairmanFile = data[0].Column5;
                            $scope.SocietyRegistration.ChairmanFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.ChairmanProfile = true;
                            $scope.CHAIRMANRemove = true;
                            $scope.GETCHAIRMAN = false;
                        }
                        else if ($scope.FOR == 'APPOINTED') {
                            $scope.SocietyRegistration.AppointedName = data[0].Column2;
                            $scope.SocietyRegistration.AppointedGender = data[0].Column3;
                            $scope.SocietyRegistration.AppointedDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.AppointedFile = data[0].Column5;
                            $scope.SocietyRegistration.AppointedFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.AdministratorProfile = true;
                            $scope.APPOINTEDRemove = true;
                            $scope.GETAPPOINTED = false;
                        }
                        else if ($scope.FOR == 'lIQUIDATOR') {
                            $scope.SocietyRegistration.liquidatorName = data[0].Column2;
                            $scope.SocietyRegistration.liquidatorGender = data[0].Column3;
                            $scope.SocietyRegistration.liquidatorDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.liquidatorFile = data[0].Column5;
                            $scope.SocietyRegistration.liquidatorFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.liquidatorProfile = true;
                            $scope.lIQUIDATORRemove = true;
                            $scope.GETlIQUIDATOR = false;
                        }
                        else if ($scope.FOR == 'SPCHAIRMAN') {
                            $scope.SocietyRegistration.CEOName = data[0].Column2;
                            $scope.SocietyRegistration.CEOGender = data[0].Column3;
                            $scope.SocietyRegistration.CEODOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.CEOSocietyFile = data[0].Column5;
                            $scope.SocietyRegistration.CEOSocietyFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.CEOSportsProfile = true;
                            $scope.SPCHAIRMANRemove = true;
                            $scope.GETSPCHAIRMAN = false;
                        }
                        else if ($scope.FOR == 'SPSECRETARY') {
                            $scope.SocietyRegistration.ChairmanName = data[0].Column2;
                            $scope.SocietyRegistration.ChairmanGender = data[0].Column3;
                            $scope.SocietyRegistration.ChairmanDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.ChairmanFile = data[0].Column5;
                            $scope.SocietyRegistration.ChairmanFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.ChairmanSportsProfile = true;
                            $scope.SPSECRETARYRemove = true;
                            $scope.GETSPSECRETARY = false;
                        }
                        else if ($scope.FOR == 'TREASURER') {
                            $scope.SocietyRegistration.AppointedName = data[0].Column2;
                            $scope.SocietyRegistration.AppointedGender = data[0].Column3;
                            $scope.SocietyRegistration.AppointedDOB = ((data[0].Column4).replace('-', '/')).replace('-', '/');
                            $scope.ChairmanFile = data[0].Column5;
                            $scope.SocietyRegistration.AppointedFile = data[0].Column5;
                            $scope.AppointedFile = data[0].Column5;
                            $scope.CloseOtpBox();
                            $scope.TreasurerProfile = true;
                            $scope.TREASURERRemove = true;
                            $scope.GETTREASURER = false;
                        }
                        else {
                            $scope.CurrentRowDataFill(data[0])
                        }

                    }
                }
            }).
            error(function (data, status) {
                $scope.SocietyRegdata = [];
            });
    }
    $scope.ClearAadharData = function (Clear) {
        if (Clear == 'CEO') {
            $scope.SocietyRegistration.CEOName = '';
            $scope.SocietyRegistration.CEOGender = '';
            $scope.SocietyRegistration.CEODOB = '';
            $scope.CEOSocietyFile = '';
            $scope.SocietyRegistration.CEOSocietyFile = '';
            $scope.CEOProfile = false;
            $scope.CEORemove = false;
            $scope.GETCEO = true;
        }
        if (Clear == 'CHAIRMAN') {
            $scope.SocietyRegistration.ChairmanName = '';
            $scope.SocietyRegistration.ChairmanGender = '';
            $scope.SocietyRegistration.ChairmanDOB = '';
            $scope.ChairmanFile = '';
            $scope.SocietyRegistration.ChairmanFile = '';
            $scope.ChairmanProfile = false;
            $scope.CHAIRMANRemove = false;
            $scope.GETCHAIRMAN = true;
        }
        if (Clear == 'APPOINTED') {
            $scope.SocietyRegistration.AppointedName = '';
            $scope.SocietyRegistration.AppointedDOB = '';
            $scope.SocietyRegistration.AppointedGender = '';
            $scope.AppointedFile = '';
            $scope.SocietyRegistration.AppointedFile = '';
            $scope.AdministratorProfile = false;
            $scope.APPOINTEDRemove = false;
            $scope.GETAPPOINTED = true;
        }
        if (Clear == 'lIQUIDATOR') {
            $scope.SocietyRegistration.liquidatorName = '';
            $scope.SocietyRegistration.liquidatorDOB = '';
            $scope.SocietyRegistration.liquidatorGender = '';
            $scope.liquidatorFile = '';
            $scope.SocietyRegistration.liquidatorFile = '';
            $scope.liquidatorProfile = false;
            $scope.lIQUIDATORRemove = false;
            $scope.GETlIQUIDATOR = true;
        }
        if (Clear == 'SPCHAIRMAN') {
            $('#txtSPCEOAADHAARNo').removeAttr("disabled");
            $('#txtSPORTCEOContactNo').removeAttr("disabled");
            $('#ddlCEOSPORTCategory').removeAttr("disabled");
            $scope.SocietyRegistration.CEOAADHAARNo = '';
            $scope.SocietyRegistration.CEOName = '';
            $scope.SocietyRegistration.CEOGender = '';
            $scope.SocietyRegistration.CEODOB = '';
            $scope.CEOSocietyFile = '';
            $scope.SocietyRegistration.CEOContactNo = '';
            $scope.SocietyRegistration.CEOSocietyFile = '';
            $scope.CEOSportsProfile = false;
            $scope.SPCHAIRMANRemove = false;
            $scope.GETSPCHAIRMAN = true;
            $scope.SocietyRegistration.CEOCategoryId = '0';
        }
        if (Clear == 'TREASURER') {
            $scope.SocietyRegistration.AppointedAADHAARNo = '';
            $scope.SocietyRegistration.AppointedContactNo = '';
            $scope.SocietyRegistration.AppointedCategoryId = '0'
            $('#txtTreasurerAadhar').removeAttr("disabled");
            $('#txtAppointedContactNo').removeAttr("disabled");
            $('#ddlAppointedSPORTCategory').removeAttr("disabled");
            $scope.SocietyRegistration.AppointedGender = '';
            $scope.SocietyRegistration.AppointedDOB = '';
            //$scope.CEOSocietyFile = '';
            $scope.SocietyRegistration.AppointedFile = '';
            $scope.TreasurerProfile = false;
            $scope.TREASURERRemove = false;
            $scope.GETTREASURER = true;

        }
        if (Clear == 'SPSECRETARY') {
            $('#txtSecretaryAadhar').removeAttr("disabled");
            $('#txtSPChairmanContactNo').removeAttr("disabled");
            $('#ddlChairmanSPORTCategory').removeAttr("disabled");
            $scope.ChairmanSportsProfile = false;
            $scope.SocietyRegistration.ChairmanAADHAARNo = '';
            $scope.SocietyRegistration.ChairmanName = '';
            $scope.SocietyRegistration.ChairmanGender = '';
            $scope.SocietyRegistration.ChairmanDOB = '';
            $scope.ChairmanFile = '';
            $scope.SocietyRegistration.ChairmanFile = '';
            $scope.ChairmanSportsProfile = false;
            $scope.SPSECRETARYRemove = false;
            $scope.GETSPSECRETARY = true;
            $scope.SocietyRegistration.ChairmanContactNo = '';
            $scope.SocietyRegistration.ChairmanCategoryId = 0;
        }
    }
    $scope.ClearAadharDataFromTable = function (TR) {
        if (TR == undefined)
            TR = $("#hdRowId").val();

        $('#' + TR).find('td:eq(2)').find('input').val('');
        $('#' + TR).find('td:eq(1)').find('input').removeAttr("disabled");
        $('#' + TR).find('td:eq(1)').find('input').val('');
        $('#' + TR).find('td:eq(3)').find('input').val('');
        $('#' + TR).find('td:eq(3)').find('input').removeAttr("disabled");
        $('#' + TR).find('td:eq(4)').find('input').val('');
        $('#' + TR).find('td:eq(5)').find('label').text('');
        $('#' + TR).find('td:eq(6)').find('select').val('0');
        $('#' + TR).find('td:eq(7)').find('input').val('');
        $('#' + TR).find('td:eq(6)').find('select').removeAttr("disabled");
        $('#' + TR).find('td:eq(7)').find('input').removeAttr("disabled");
        if (TR > 1000 && TR < 2000) {
            $('#' + TR).find('td:eq(8)').find('textarea').val('');
            ($('#' + TR).find('td:eq(9)').find('img'))[0].src = '../Content/Images/Balnk.png';
            $('#' + TR).find('td:eq(10)').find('input').val('');
            ($('#' + TR).find('td:eq(11)').find('input')).prop('checked', false);;
            $($('#' + TR).find('td:eq(12)').find('i')[1]).hide();
            $($('#' + TR).find('td:eq(12)').find('i')[0]).show();
        }
        else if (TR > 500 && TR < 2000) {
            $('#' + TR).find('td:eq(8)').find('select').val('0');
            ($('#' + TR).find('td:eq(9)').find('img'))[0].src = '../Content/Images/Balnk.png';
            $('#' + TR).find('td:eq(10)').find('input').val('');
            ($('#' + TR).find('td:eq(11)').find('input')).prop('checked', false);;
            $($('#' + TR).find('td:eq(12)').find('i')[1]).hide();
            $($('#' + TR).find('td:eq(12)').find('i')[0]).show();
        }
        else {
            ($('#' + TR).find('td:eq(8)').find('img'))[0].src = '../Content/Images/Balnk.png';
            $('#' + TR).find('td:eq(9)').find('input').val('');
            ($('#' + TR).find('td:eq(10)').find('input')).prop('checked', false);;
            $($('#' + TR).find('td:eq(11)').find('i')[1]).hide();
            $($('#' + TR).find('td:eq(11)').find('i')[0]).show();
        }
    }
    $scope.CurrentRowDataFill = function (data) {
        var TR = $scope.CurrentRow;
        // alert(TR);
        if (TR > 500 && TR < 1000) {
            //alert("in Nominated");
            $('#' + TR).find('td:eq(2)').find('input').val(data.Column2);
            $('#' + TR).find('td:eq(1)').find('input').attr("disabled", "disabled");
            $('#' + TR).find('td:eq(4)').find('input').val(((data.Column4).replace('-', '/')).replace('-', '/'));
            $('#' + TR).find('td:eq(5)').find('label').text(data.Column3 == 'M' ? 'MALE' : 'FEMALE');

            ($('#' + TR).find('td:eq(9)').find('img'))[0].src = data.Column5;
            var jElement = $('#' + TR).find('td:eq(9)').find('img');
            jElement.css('border', 'rgb(222, 221, 221)');

        }
        else {
            $('#' + TR).find('td:eq(2)').find('input').val(data.Column2);
            $('#' + TR).find('td:eq(1)').find('input').attr("disabled", "disabled");
            $('#' + TR).find('td:eq(4)').find('input').val(((data.Column4).replace('-', '/')).replace('-', '/'));
            $('#' + TR).find('td:eq(5)').find('label').text(data.Column3 == 'M' ? 'MALE' : 'FEMALE');
            if (TR > 1000 && TR < 2000) {
                ($('#' + TR).find('td:eq(9)').find('img'))[0].src = data.Column5;
                $($('#' + TR).find('td:eq(10)').find('i')[0]).hide();
                $($('#' + TR).find('td:eq(10)').find('i')[1]).show();
                var jElement = $('#' + TR).find('td:eq(9)').find('img');
                jElement.css('border', 'rgb(222, 221, 221)');
                var jElement = $('#' + TR).find('td:eq(8)').find('textarea');
                jElement.css('border', 'rgb(222, 221, 221)');
            }
            else {
                ($('#' + TR).find('td:eq(8)').find('img'))[0].src = data.Column5;
                $($('#' + TR).find('td:eq(9)').find('i')[0]).hide();
                $($('#' + TR).find('td:eq(9)').find('i')[1]).show();
                var jElement = $('#' + TR).find('td:eq(8)').find('img');
                jElement.css('border', 'rgb(222, 221, 221)');
            }
        }

        var jElement = $('#' + TR).find('td:eq(1)').find('input');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(2)').find('input');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(3)').find('input');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(4)').find('input');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(5)').find('label');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(6)').find('select');
        jElement.css('border', 'rgb(222, 221, 221)');

        var jElement = $('#' + TR).find('td:eq(7)').find('select');
        jElement.css('border', 'rgb(222, 221, 221)');

        $scope.CloseOtpBox();
        //307691961366
    }
    //end aadhar verify

    $scope.AddMoreMemberDetailsNew = function (Flag, Isvalidate) {
        if (Flag == 0) {
            if ($scope.ValidateNominatedMember(Flag, Isvalidate)) {
                var rows = $('#TblNominatedMember tr').length + 500 + 1;
                if (_language == 'hi-in') {
                    $('#TblNominatedMember').append('<tr id=' + rows + '>' +
                        '<td>' + (parseInt($('#TblNominatedMember tr').length) + 1) + '</td>' +
                        '<td><input type="text" name="AdhareNo" class="form-control" placeholder="आधार संख्या"  maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                        '<td><input disabled="disabled" type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर"  maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                        '<td><input  disabled="disabled" class="form-control AddDate" placeholder="जन्म की तारीख">' +
                        '<td><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                        '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '" ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                        '<td><div style="width:100%"><input type="text" class="form-control" placeholder="पद नाम"></div></td >' +
                        '<td><div style="width:100%"><select class="form-control"><option value="0" disabled>--- चुने ---</option><option value="1" class="ng-binding">सरकार द्वारा</option><option value="2" class="ng-binding">नाबार्ड द्वारा</option><option value="3" class="ng-binding">आरबीआई द्वारा</option><option value="4" class="ng-binding">तकनीकी निदेशक द्वारा</option></select ></div ></td >' +
                        '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                        '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle info-m2"></i><i onclick="SetAngularValueForRemove(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                        '</tr>');
                }
                else {
                    $('#TblNominatedMember').append('<tr id=' + rows + '>' +
                        '<td>' + (parseInt($('#TblNominatedMember tr').length) + 1) + '</td>' +
                        '<td><input type="text" name="AdhareNo" class="form-control" placeholder="AADHAARNo" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                        '<td><input type= "text" disabled="disabled" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Director Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="MobileNo" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                        '<td><input disabled="disabled" name="DOB" class="form-control AddDate" placeholder="DOB" autocomplete="off">' +
                        '<td><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                        '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '"><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                        '<td><div style="width:100%"><input type="text" class="form-control" placeholder="Post Name"></div></td >' +
                        '<td><div style="width:100%"><select class="form-control"><option value="0" disabled>--- Select ---</option><option value="1" class="ng-binding">By Government</option><option value="2" class="ng-binding">By Nabard</option><option value="3" class="ng-binding">By RBI</option><option value="4" class="ng-binding">By Technical Director</option></select ></div ></td >' +
                        '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                        '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle info-m2"></i><i onclick="SetAngularValueForRemove(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                        '</tr>');
                }
            }
            else {
                //  alert('false');
            }
        }
        if (Flag == 1) {
            if ($scope.ValidateNominatedMember(Flag, Isvalidate)) {
                var rows = ($('#TblSportElectedMember tr').length) + 1001;
                if (_language == 'hi-in') {
                    $('#TblSportElectedMember').append('<tr id=' + rows + '>' +
                        '<td>' + (parseInt($('#TblSportElectedMember tr').length) + 1) + '</td>' +
                        '<td><input type="text" name="AdhareNo" class="form-control" placeholder="आधार संख्या"  maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                        '<td><input disabled="disabled" type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर"  maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                        '<td><input  disabled="disabled" class="form-control AddDate" placeholder="जन्म की तारीख">' +
                        '<td><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                        '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '" ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                        '<td><div style="width:100%"><input type="text" class="form-control" placeholder="पद"></div></td >' +
                        '<td><textarea type="text" name="Address" class="form-control" placeholder="Address" autocomplete="off"></textarea></td >' +
                        '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                        '<td><input type="checkbox" class="checkbox-input"></td>' +
                        '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle info-m2"></i><i ng-show="SHOWREMOVE" onclick="SetAngularValueForRemove(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                        '</tr>');
                }
                else {
                    $('#TblSportElectedMember').append('<tr id=' + rows + '>' +
                        '<td>' + (parseInt($('#TblSportElectedMember tr').length) + 1) + '</td>' +
                        '<td><input type="text" name="AdhareNo" class="form-control" placeholder="AADHAARNo" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                        '<td><input type= "text" disabled="disabled" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Director Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="MobileNo" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                        '<td><input disabled="disabled" name="DOB" class="form-control AddDate" placeholder="DOB" autocomplete="off">' +
                        '<td><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                        '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '"><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                        '<td><div style="width:100%"><input type="text" class="form-control" placeholder="POST"></div></td >' +
                        '<td><textarea type="text" name="Address" class="form-control" placeholder="पता" autocomplete="off"></textarea></td >' +
                        '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                        '<td><input type="checkbox" class="checkbox-input"></td>' +
                        '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle info-m2"></i><i onclick="SetAngularValueForRemove(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                        '</tr>');
                }
            }
        }
        if (Flag == 2) {
            if ($scope.ValidateNominatedMember(Flag)) {
                var rows = $('#TblSportNominatedMember tr').length + 1;
                if (_language == 'hi-in') {
                    $('#TblSportNominatedMember').append('<tr id=' + rows + '>' +
                        '<td>' + (parseInt($('#TblSportNominatedMember tr').length) + 1) + '</td>' +
                        '<td><input type="text" name="AdhareNo" class="form-control" placeholder="आधार संख्या"  maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                        '<td><input disabled="disabled" type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर"  maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                        '<td><input  disabled="disabled" class="form-control AddDate" placeholder="जन्म की तारीख">' +
                        '<td><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                        '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '" ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                        '<td><div style="width:100%"><select class="form-control" id="Post' + rows + '"><option value="0" disabled>{{translation.SelectOption}}</option></select></div></td >' +
                        '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                        '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle info-m2"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ></td >' +
                        '</tr>');
                }
                else {
                    $('#TblSportNominatedMember').append('<tr id=' + rows + '>' +
                        '<td>' + (parseInt($('#TblSportNominatedMember tr').length) + 1) + '</td>' +
                        '<td><input type="text" name="AdhareNo" class="form-control" placeholder="AADHAARNo" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                        '<td><input type= "text" disabled="disabled" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Director Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="MobileNo" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                        '<td><input disabled="disabled" name="DOB" class="form-control AddDate" placeholder="DOB" autocomplete="off">' +
                        '<td><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                        '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '"><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                        '<td><div style="width:100%"><select class="form-control" id="Post' + rows + '"><option value="0" disabled>{{translation.SelectOption}}</option></select></div></td >' +
                        '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                        '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle info-m2"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ></td >' +
                        '</tr>');
                }
            }
        }
        if (Flag == 3) {
            if ($scope.AdministrativeFormValidation(Isvalidate)) {
                var rows = $('#TblElectedMember tr').length + 1;
                if (_language == 'hi-in') {
                    $('#TblElectedMember').append('<tr id=' + rows + '>' +
                        '<td>' + (parseInt($('#TblElectedMember tr').length) + 1) + '</td>' +
                        '<td><input type="text" name="AdhareNo" class="form-control" placeholder="आधार संख्या"  maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                        '<td><input disabled="disabled" type= "text" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "निदेशक का नाम"  autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="मोबाइल नंबर"  maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                        '<td><input  disabled="disabled" class="form-control AddDate" placeholder="जन्म की तारीख">' +
                        '<td><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                        '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '" ><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                        '<td><div style="width:100%"><input type="text" class="form-control" placeholder="पद"></div></td >' +
                        '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                        '<td><input type="text" class="form-control" placeholder="व्यवसाय"></td>' +
                        '<td><input type="checkbox" class="checkbox-input"></td>' +
                        '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle green-m1"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +

                        '</tr>');
                }
                else {
                    $('#TblElectedMember').append('<tr id=' + rows + '>' +
                        '<td>' + (parseInt($('#TblElectedMember tr').length) + 1) + '</td>' +
                        '<td><input type="text" name="AdhareNo" class="form-control" placeholder="AADHAARNo" maxlength="12" minlength="12" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 59" autocomplete="off"></td >' +
                        '<td><input type= "text" disabled="disabled" name= "DirectorsName" class="form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse" placeholder= "Director Name" autocomplete= "off" style= "border: 1px solid rgb(204, 204, 204);" ></td >' +
                        '<td><input type="text" name="ContactNo" class="form-control ng-pristine ng-valid ng-empty ng-valid-minlength ng-valid-maxlength ng-touched" placeholder="MobileNo" maxlength="10" minlength="10" onkeypress="return event.charCode >= 48 &amp;&amp; event.charCode <= 57" autocomplete="off"></td >' +
                        '<td><input disabled="disabled" name="DOB" class="form-control AddDate" placeholder="DOB" autocomplete="off">' +
                        '<td><div style="width:100%"> <label style="width:40% !important;"></label></td ></div >' +
                        '<td><div style="width:100%"><select class="form-control" id="Category' + rows + '"><option value="0" disabled>{{ translation.SelectOption }}</option></select ></div ></td >' +
                        '<td><div style="width:100%"><input type="text" class="form-control" placeholder="Post"></div></td >' +
                        '<td><img class="img-thumbnail" src="../Content/Images/Balnk.png" height="50" width="50" /></td>' +
                        '<td><input type="text" class="form-control" placeholder="Occupation"></td>' +
                        '<td><input type="checkbox" class="checkbox-input"></td>' +
                        '<td><i onclick="SetAngularValue(' + rows + ')" class="fa fa-info-circle green-m1"></i><i ng-click="ClearAadharDataFromTable(' + rows + ')" class="fa fa-minus-circle red-m1" ></i ><i class="fa fa-trash clsShowHide btn btn btn-xs btn-danger tooltips" id="Remove_' + rows + '" onclick="RemoveRowAdminiAct2001(this,' + rows + ')"></i></td >' +
                        '</tr>');
                }
            }
            else {

            }
        }
        //FIll Category DropDown 

        var ddHTML = _language == 'en-us' ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        angular.forEach($scope.CategoryList, function (item) {
            if (_language == 'en-us') {
                ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            }
            else {
                ddHTML += '<option value="' + (item.CODEID) + '">' + item.CODENAME + '</option>';
            }
        });
        $("#Category" + rows).html(ddHTML);
        //END
        //FIll Post DropDown
        var ddHTML = _language == 'en-us' ? '<option value="0">-- Select --</option>' : '<option value="0">-- चुने --</option>';
        angular.forEach($scope.ChairmanPosts, function (item) {
            if (_language == 'en- us') {
                ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_NAME + '</option>';
            }
            else {
                ddHTML += '<option value="' + (item.CAD_ID) + '">' + item.CAD_SERV + '</option>';
            }
        });
        $("#Post" + rows).html(ddHTML);
        //END


    };

    $scope.ValidateNominatedMember = function (FindTablebyId, Isvalidate) {
        if (Isvalidate == false)
            return true;
        else {
            var validate = true;
            var table = ''
            if (FindTablebyId == 0) {
                table = '#TblNominatedMember tr';
            }
            else if (FindTablebyId == 1) {
                table = '#TblSportElectedMember tr';
            }
            else if (FindTablebyId == 2) {
                table = '#TblSportNominatedMember tr';
            }

            $(table).each(function () {
                if ($(this).find('td:eq(1)').find('input').val() != '' || FindTablebyId != 0) {
                    if ($(this).find('td:eq(1)').find('input').val() == '') {
                        var jElement = $(this).find('td:eq(1)').find('input');
                        jElement.css('border', 'solid 1px red');
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(1)').find('input');
                        jElement.css('border', 'rgb(222, 221, 221)');
                    }

                    if ($(this).find('td:eq(2)').find('input').val() == undefined || $(this).find('td:eq(2)').find('input').val() == '') {
                        var jElement = $(this).find('td:eq(2)').find('input');
                        jElement.css('border', 'solid 1px red');
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(2)').find('input');
                        jElement.css('border', 'rgb(222, 221, 221)');
                    }

                    if ($(this).find('td:eq(3)').find('input').val() == undefined || $(this).find('td:eq(3)').find('input').val() == '') {
                        var jElement = $(this).find('td:eq(3)').find('input');
                        jElement.css('border', 'solid 1px red');
                        validate = false;
                        $(this).find('td:eq(3)').find('input').removeAttr("disabled");
                    }
                    else {
                        var jElement = $(this).find('td:eq(3)').find('input');
                        jElement.css('border', 'rgb(222, 221, 221)');
                    }
                    //DOB
                    if ($(this).find('td:eq(4)').find('input').val() == undefined || $(this).find('td:eq(4)').find('input').val() == '') {
                        var jElement = $(this).find('td:eq(4)').find('input');
                        jElement.css('border', 'solid 1px red');
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(4)').find('input');
                        jElement.css('border', 'rgb(222, 221, 221)');
                    }
                    //GEN
                    if ($(this).find('td:eq(5)').find('label').html() == '') {
                        var jElement = $(this).find('td:eq(5)').find('label');
                        jElement.css('border', 'solid 1px red');
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(5)').find('label');
                        jElement.css('border', 'rgb(222, 221, 221)');

                    }
                    //CATEGORY
                    if ($(this).find('td:eq(6)').find('select').val() == null || $(this).find('td:eq(6)').find('select').val() == "0") {
                        var jElement = $(this).find('td:eq(6)').find('select');
                        jElement.css('border', 'solid 1px red');
                        $(this).find('td:eq(6)').find('select').removeAttr("disabled");
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(6)').find('select');
                        jElement.css('border', 'rgb(222, 221, 221)');

                    }
                    //POST
                    if ($(this).find('td:eq(7)').find('input').val() == null || $(this).find('td:eq(7)').find('input').val() == "") {
                        var jElement = $(this).find('td:eq(7)').find('input');
                        jElement.css('border', 'solid 1px red');
                        $(this).find('td:eq(7)').find('input').removeAttr("disabled");
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(7)').find('input');
                        jElement.css('border', 'rgb(222, 221, 221)');

                    }
                    //Nominated By Whom	
                    if (FindTablebyId == 0) {
                        if ($(this).find('td:eq(8)').find('select').val() == null || $(this).find('td:eq(8)').find('select').val() == "0") {
                            var jElement = $(this).find('td:eq(8)').find('select');
                            jElement.css('border', 'solid 1px red');
                            $(this).find('td:eq(8)').find('select').removeAttr("disabled");
                            validate = false;
                        }
                        else {
                            var jElement = $(this).find('td:eq(8)').find('select');
                            jElement.css('border', 'rgb(222, 221, 221)');

                        }
                    }

                    if (FindTablebyId == 1) {
                        if ($(this).find('td:eq(8)').find('textarea').val() == '') {
                            var jElement = $(this).find('td:eq(8)').find('textarea');
                            jElement.css('border', 'solid 1px red');
                            validate = false;
                        }
                        else {
                            var jElement = $(this).find('td:eq(8)').find('textarea');
                            jElement.css('border', 'rgb(222, 221, 221)');
                        }
                    }

                    if ($(this).find('td:eq(9)').find('img')[0].src.indexOf("Balnk.png") > 0) {
                        var jElement = $(this).find('td:eq(9)').find('img');
                        jElement.css('border', 'solid 1px red');
                        validate = false;
                    }
                    else {
                        var jElement = $(this).find('td:eq(9)').find('img');
                        jElement.css('border', 'rgb(222, 221, 221)');

                    }
                }
            });
            return validate;
        }
    }
    //START AdministrativeFormValidation  
    $scope.AdministrativeFormValidation = function () {
        var validate = true;
        var Count = 0;
        // alert('OA');
        var message = $scope._language == 'en-us' ? ' Validate  following : ' : 'निम्नलिखित मान्य करें :';
        //if ($scope.ActId != "7") {
        //if ($scope.SocietyRegistration.IsAdministrative == undefined) {

        //    message += $scope._language == 'en-us' ? '\n <br /> Please Select Administrative Type' : '\n <br /> प्रशासनिक प्रकार का चयन करे |';
        //}
        ////Administrator
        //if ($scope.SocietyRegistration.IsAdministratorAppointed == 'true') {
        //    if ($scope.SocietyRegistration.AppointedName == undefined || $scope.SocietyRegistration.AppointedName == '') {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Administrator Name' : '\n <br /> प्रशासक का नाम दर्ज करें |';

        //    }

        //    if ($('#txtAppointedDOB').val() == '') {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Administrator  Date of Birth' : '\n <br /> कृपया प्रशासक की जन्म तारीख दर्ज करे |';

        //    }

        //    if ($scope.SocietyRegistration.AppointedCategoryId == "0" || $scope.SocietyRegistration.AppointedCategoryId == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select The Category Of Administrator.' : '\n <br /> प्रशासक के वर्ग का चयन करें |';

        //    }

        //    if ($scope.SocietyRegistration.AppointedGender == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select Administrator Gender Type.' : '\n <br /> कृपया प्रशासक के लिंग का चयन करे |';

        //    }
        //    if ($scope.SocietyRegistration.AppointedPostId == "0" || $scope.SocietyRegistration.AppointedPostId == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select Administrator Post.' : '\n <br /> कृपया प्रशासक के पद का चयन करे |';

        //    }
        //    //if ($scope.SocietyRegistration.DESIGNATIONCODE == "0") {

        //}
        ////END

        ////liquidator
        //if ($scope.SocietyRegistration.IsAdministrative == 'liquidator') {
        //    if ($scope.SocietyRegistration.liquidatorName == undefined || $scope.SocietyRegistration.liquidatorName == '') {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter liquidator Name' : '\n <br /> परिसमापक का नाम दर्ज करें |';

        //    }

        //    if ($('#txtliquidatorDOB').val() == '') {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter liquidator Date of Birth' : '\n <br /> कृपया परिसमापक की जन्म तिथि दर्ज करे |';

        //    }

        //    if ($('#txtliquidatorAppointedDate').val() == '') {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter liquidator Date of Appointment' : '\n <br /> कृपया परिसमापक की नियुक्ति तिथि दर्ज करे |';

        //    }

        //    if ($scope.SocietyRegistration.liquidatorCategoryId == "0" || $scope.SocietyRegistration.liquidatorCategoryId == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select The Category Of liquidator.' : '\n <br /> कृपया परिसमापक के वर्ग का चयन करें |';

        //    }

        //    if ($scope.SocietyRegistration.liquidatorGender == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select liquidator Gender Type.' : '\n <br /> कृपया परिसमापक के लिंग का चयन करे |';

        //    }
        //    if ($scope.SocietyRegistration.liquidatorPostId == "0" || $scope.SocietyRegistration.liquidatorPostId == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select liquidator Post.' : '\n <br /> कृपया परिसमापक के पद का चयन करे |';

        //    }
        //}
        ////END
        //if ($scope.SocietyRegistration.IsAdministrative == 'Board') {
        var Count = 0;
        $('#TblElectedMember tr').each(function () {
            if ($(this).find('td:eq(1)').find('input').val() == undefined || $(this).find('td:eq(1)').find('input').val() == '') {
                var jElement = $(this).find('td:eq(1)').find('input');
                jElement.css('border', 'solid 1px red');
                validate = false;
            }
            else {
                var jElement = $(this).find('td:eq(1)').find('input');
                jElement.css('border', 'rgb(222, 221, 221)');
            }

            if ($(this).find('td:eq(2)').find('input').val() == undefined || $(this).find('td:eq(2)').find('input').val() == '') {
                var jElement = $(this).find('td:eq(2)').find('input');
                jElement.css('border', 'solid 1px red');
                validate = false;
            }
            else {
                var jElement = $(this).find('td:eq(2)').find('input');
                jElement.css('border', 'rgb(222, 221, 221)');
            }

            if ($(this).find('td:eq(3)').find('input').val() == undefined || $(this).find('td:eq(3)').find('input').val() == '') {
                var jElement = $(this).find('td:eq(3)').find('input');
                jElement.css('border', 'solid 1px red');
                validate = false;
                $(this).find('td:eq(3)').find('input').removeAttr("disabled");
            }
            else {
                var jElement = $(this).find('td:eq(3)').find('input');
                jElement.css('border', 'rgb(222, 221, 221)');
            }

            if ($(this).find('td:eq(4)').find('input').val() == undefined || $(this).find('td:eq(4)').find('input').val() == '') {
                var jElement = $(this).find('td:eq(4)').find('input');
                jElement.css('border', 'solid 1px red');
                validate = false;
            }
            else {
                var jElement = $(this).find('td:eq(4)').find('input');
                jElement.css('border', 'rgb(222, 221, 221)');
            }

            if ($(this).find('td:eq(5)').find('label').html() == '') {
                var jElement = $(this).find('td:eq(5)').find('label');
                jElement.css('border', 'solid 1px red');
                validate = false;
            }
            else {
                var jElement = $(this).find('td:eq(5)').find('label');
                jElement.css('border', 'rgb(222, 221, 221)');
            }

            if ($(this).find('td:eq(6)').find('select').val() == null || $(this).find('td:eq(6)').find('select').val() == "0") {
                var jElement = $(this).find('td:eq(6)').find('select');
                jElement.css('border', 'solid 1px red');
                $(this).find('td:eq(6)').find('select').removeAttr("disabled");
                validate = false;
            }
            else {
                var jElement = $(this).find('td:eq(6)').find('select');
                jElement.css('border', 'rgb(222, 221, 221)');
            }
            if ($(this).find('td:eq(7)').find('input').val() == '') {
                var jElement = $(this).find('td:eq(7)').find('input');
                jElement.css('border', 'solid 1px red');
                validate = false;
            }
            else {
                var jElement = $(this).find('td:eq(7)').find('input');
                jElement.css('border', 'rgb(222, 221, 221)');
            }

            if ($(this).find('td:eq(8)').find('img')[0].src.indexOf("Balnk.png") > 0) {
                var jElement = $(this).find('td:eq(8)').find('img');
                jElement.css('border', 'solid 1px red');
                validate = false;
            }
            else {
                var jElement = $(this).find('td:eq(8)').find('img');
                jElement.css('border', 'rgb(222, 221, 221)');
            }

            if ($(this).find('td:eq(9)').find('input').val() == '') {
                var jElement = $(this).find('td:eq(9)').find('input');
                jElement.css('border', 'solid 1px red');
                validate = false;
            }
            else {
                var jElement = $(this).find('td:eq(9)').find('input');
                jElement.css('border', 'rgb(222, 221, 221)');
            }

            if ($(this).find('td:eq(10)').find('input').prop('checked') == true) {
                Count = Count + 1;
            }
        });

        //if ($scope.ActId == "3" && Count < 12) {
        //    var r = confirm($scope._language == 'en-us' ? 'Minimum 12 officers should be in the executive / governing board, do you want to proceed further?' : 'न्यूनतम 12 पदाधिकारी /कार्यकारिणी /संचालक मंडल में होने चाहिए क्या आप आगे प्रोसेस करना चाहते है ?');
        //    if (r == false) {
        //        return false;
        //    }
        //}
        //else if ($scope.ActId == "1" && Count < 7) {
        //    var r = confirm($scope._language == 'en-us' ? 'Minimum 7 officers should be in the executive / governing board, do you want to proceed further?' : 'न्यूनतम 7 पदाधिकारी /कार्यकारिणी /संचालक मंडल में होने चाहिए क्या आप आगे प्रोसेस करना चाहते है ?');
        //    if (r == false) {
        //        return false;
        //    }
        //}
        //}

        //if ($scope.SocietyRegistration.IsAdministrative == 'false') {


        //}

        //if ($scope.SocietyRegistration.IsAdministrative == 'Board' && $scope.SocietyRegistration.ActId == "3") {
        //    if ($scope.SocietyRegistration.TotalMembersClassA == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Total Members Class A.' : '\n <br /> कृपया कक्षा ए के कुल सदस्य दर्ज करें |';
        //    }
        //    if ($scope.SocietyRegistration.TotalMembersClassB == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Total Members Class B.' : '\n <br /> कृपया कक्षा बी के कुल सदस्य दर्ज करें |';
        //    }
        //    if ($scope.SocietyRegistration.TotalMembersClassC == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Total Members Class C.' : '\n <br /> कृपया कक्षा सी के कुल सदस्य दर्ज करें |';
        //    }
        //}

        //}
        //else {

        //    if ($('#txtSportCEODOB').val() == '') {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter Chairman Date of Birth' : '\n <br /> कृपया अध्यक्ष की जन्म तारीख दर्ज करे |';

        //    }

        //    if ($scope.SocietyRegistration.CEOGender == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select Chairman Gender Type.' : '\n <br /> कृपया अध्यक्ष के लिंग का चयन करे |';

        //    }

        //    if ($scope.SocietyRegistration.CEOCategoryId == "0" || $scope.SocietyRegistration.CEOCategoryId == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select The Category Of Chairman.' : '\n <br /> कृपया अध्यक्ष के वर्ग का चयन करें |';

        //    }

        //    if ($scope.CEOSocietyFile == '' || $scope.CEOSocietyFile == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select The  File of Chairman.' : '\n <br /> कृपया अध्यक्ष की फ़ाइल का चयन करें|';

        //    }
        //    //}
        //    if ($('#txtSportChairmanDOB').val() == '') {

        //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter the Secretary DOB.' : '\n <br /> कृपया सचिव की जन्म तारीख दर्ज करें |';

        //    }

        //    if ($scope.SocietyRegistration.ChairmanGender == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select Secretary Gender Type.' : '\n <br /> कृपया सचिव के लिंग का चयन करे |';

        //    }

        //    if ($scope.ChairmanFile == '' || $scope.ChairmanFile == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select The File of Secretary.' : '\n <br /> कृपया सचिव की फ़ाइल का चयन करें|';

        //    }
        //    if ($scope.SocietyRegistration.ChairmanCategoryId == "0" || $scope.SocietyRegistration.ChairmanCategoryId == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select The Category Of Secretary.' : '\n <br /> कृपया सचिव की श्रेणी का चयन करें |';

        //    }



        //    //Treasurer

        //    if ($('#txtSportAppointedDOB').val() == '') {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Enter the DOB Of Treasurer.' : '\n <br /> कृपया कोषाध्यक्ष की जन्म तारीख दर्ज करें |';

        //    }
        //    if ($scope.AppointedFile == '' || $scope.AppointedFile == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Choose the File of Treasurer.' : '\n <br /> कृपया कोषाध्यक्ष की फाइल चुनें |';

        //    }
        //    if ($scope.SocietyRegistration.AppointedCategoryId == '0' || $scope.SocietyRegistration.AppointedCategoryId == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select The Category Of treasurer.' : '\n <br /> कृपया कोषाध्यक्ष की श्रेणी का चयन करें|';

        //    }

        //    if ($scope.SocietyRegistration.AppointedGender == undefined) {
        //        message += $scope._language == 'en-us' ? '\n <br /> Please Select Secretary treasurer Type.' : '\n <br /> कृपया कोषाध्यक्ष के लिंग का चयन करे |';

        //    }

        //}
        ////End Sport Association

        if (message.length > 24 || validate == false) {
            if (message.length > 24) {
                ShowModelAlert("Administrative Form Validation", message);
            }
            else {
                ShowModelAlert("Administrative Form Validation", $scope._language == 'en-us' ? 'Validate the value of red border' : 'लाल सीमा के मूल्य को मान्य करें');
            }
            return false;
        }
        return true;
    }
    //validateAddForm for farmer ends

    // START : Save Balance Sheet Insert
    $scope.SaveBalanceSheetInsert = function () {
        var message = $scope._language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        if ($scope.SocietyAmendment.RegiYearId == undefined || $scope.SocietyAmendment.RegiYearId == '0') {
            message += (_language == 'en-us' ? 'Please Select Registered Year \n' : 'पंजीकृत वर्ष का चयन करे\n');
            message += '<br />';
        }
        if ($scope.InsertBSheetFileExl == undefined) {
            message += $scope._language == 'en-us' ? '\nPlease Upload Excel Sheet' : '\nकृपया एक्सेल शीट अपलोड करें |';
            message += '<br />';
        }
        if (message.length > 27) {
            ShowModelAlert("Belence Sheet Insert", message);
            return false;
        }
        // alert(11);
        var Datasplit = $scope.SocietyAmendment.SocietyIdEnc.split("~");
        var formData = new FormData();
        //  alert($scope.SocietyAmendment.RegiYearId);
        formData.append("SocietyIdEnc", Datasplit[0]);
        formData.append("SocietyDirectory", Datasplit[1]);
        formData.append("RegiYearId", $scope.SocietyAmendment.RegiYearId);
        formData.append("ExcelBalanceSheetFile", $scope.InsertBSheetFileExl);
        $http.post(SiteUrl + "/Citizen/InsertBalanceSheetByExcel", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                //alert(1);
                $scope.BalanceSheetNew = data;
                //alert(2); 
                $scope.NewBSheet = true;
                //alert(3);
                $scope.BalanceSheetFileList();
            }).
            error(function (data, status) {
                ShowModelAlert("Insert of Balance Sheet", "Error While Upload Excel Sheet");
            });
    };
    // END :Save Balance Sheet Insert

    $scope.messagebox = false;



    $scope.OnPageLoad();
});