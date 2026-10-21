var MasterMod = angular.module('PLDBLoanMastersMOD', ['angularUtils.directives.dirPagination']);
MasterMod.controller("PLDBLoanMastersCONT", function ($scope, $http) {

    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;

    $scope.MasterModel = {
        master_id: 0,
        name_en: "",
        name_hi: "",
        is_active: true,
        scheme_id: "0",
        Farming_NonFarming: "0",
        sector_id: "",
        effective_date: "",
        normal_interest_rate: "",
        penal_interest_rate: "",
        purpose_id: '0',
        RepaymentPeriod: "",
        DueInstallment: "",
        GracePeriod: "",
        OrderOfPurpose: ""

    }

    $scope.ResetAddOld = function () {
        $scope.LoanSectorList = [];
        $scope.MasterModel = {
            master_id: 0,
            name_en: "",
            name_hi: "",
            is_active: true,
            scheme_id: "0",
            sector_id: "",
            Farming_NonFarming: "",
            effective_date: "",
            normal_interest_rate: "",
            penal_interest_rate: "",

            RepaymentPeriod: "",
            DueInstallment: "",
            GracePeriod: "",


        }

    }

    $scope.ResetAddNew = function () {
        $scope.MasterModel = {
            master_id: 0,
            name_en: "",
            name_hi: "",
            is_active: true,
            scheme_id: "0",
            sector_id: "",
            Farming_NonFarming: "",
            effective_date: "",
            normal_interest_rate: "",
            penal_interest_rate: "",

            RepaymentPeriod: "",
            DueInstallment: "",
            GracePeriod: "",
            is_active: false,
            OrderOfPurpose:""


        }
        $scope.PurposeImageIcon.FileName = "";
        $scope.PurposeImageIcon.IsUploadFileName = false;

        $scope.LoanSectorList.forEach(function (item, index) {
            item.IsShow = false;
        });

    }

    $scope.ClearSearch = function () {
        $scope.q = "";
    }
    $scope.AddNewPurpose = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.purposeList = false;
        $scope.addPurpose = true;
        $("#addModal").removeClass("modal-backdrop");
        $(".se-pre-con").fadeOut("slow");

        //$("#addModal").modal("show");
    }
    $scope.AddNewSchemeOne = function () {

        $("#addModal").modal("show");
    }

    $scope.Back = function () {
        $(".se-pre-con").fadeIn("slow");
        $scope.addPurpose = false;
        $scope.purposeList = true;
        $(".se-pre-con").fadeOut("slow");
    }


    // on page load
    $scope.OnPageLoad = function () {

        //$(".se-pre-con").fadeIn("slow");
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
            $scope._language = 'hi-in';
            $http.get("../Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
            // $(".se-pre-con").fadeOut("slow");
        }
        $(".blockUI").fadeIn("slow");
        if ($("#MasterType").val() == "Scheme") {
            $scope.GetSchemeMaster("").then(function () { $(".blockUI").fadeOut("slow"); });
        } else if ($("#MasterType").val() == "Sector") {
            $scope.GetSchemeMaster(" and is_active=1").then(function () {
                $scope.GetSectorMaster("").then(function () { $(".blockUI").fadeOut("slow"); });
            });
        }
        else if ($("#MasterType").val() == "Purpose") {
            $scope.GetSchemeMaster(" and is_active=1").then(function () {
                $scope.GetPurposeMaster("").then(function () {
                    $scope.purposeList = true;
                    $scope.addPurpose = false;
                    $(".blockUI").fadeOut("slow");
                });


            });
        }
        //$scope.purposeList = true;
        //$scope.addPurpose = false;
        $scope.GetPurposeMaster("");
    };

    /*PLDB Loan Scheme Master*/
    $scope.LoanSchemeList = [];
    $scope.GetSchemeMaster = function (WhereCase) {
        return $http.post(SiteUrl + "/PLDB/GetSchemeMaster", { "WhereCase": WhereCase }).
            success(function (data, status) {
                $scope.LoanSchemeList = [];
                $scope.LoanSchemeList = data;
                $scope.TotalRecord = $scope.LoanSchemeList.length;
                //$(".blockUI").fadeOut("slow");
            }).
            error(function (data, status) {
                alert("Error while fetching Society Details.");
                //$(".blockUI").fadeOut("slow");
            });

    }
    $scope.SaveSchemeMasterData = function () {
        if ($scope.ValidateSchemeMaster()) {

            $(".blockUI").fadeIn("slow");

            var formData = {
                Id: $scope.MasterModel.master_id,
                IsActive: $scope.MasterModel.is_active,
                NameEn: $scope.MasterModel.name_en,
                NameHi: $scope.MasterModel.name_hi,
                PerposeId: $scope.MasterModel.purpose_id,
                SectorId: $scope.MasterModel.sector_id
            }

            $http.post(SiteUrl + "/PLDB/SchemeMaster", formData).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (data.Status == 2) {
                        ShowOkMessage("Warning", $scope._language == 'en-us' ? "Scheme Name Already Exist." : "योजना का नाम पहले से मौजूद है", function () { });
                    }
                    else if (data.Status == 3) {
                        ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    }
                    else {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "Scheme Name successfully submited." : "योजना का नाम सफलतापूर्वक सबमिट किया गया", function () {
                            $(".se-pre-con").fadeIn("slow");
                            $("#addModal").modal("hide");
                            $scope.GetSchemeMaster("");
                            $(".se-pre-con").fadeOut("slow");
                            //window.location = "/pldb/schememaster";
                        });
                    }
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                });
        }
    }

    //$scope.EditScheme = function (index) {
    //    $scope.ResetAddNew();
    //    $("#addModal").modal("show");
    //    $scope.LoanSchemeList.forEach(function (itm, indx) {
    //        if (index == indx) {
    //            //$scope.MasterModel.master_id = itm.scheme_id;
    //            $scope.MasterModel.name_en = itm.scheme_name_en;
    //            $scope.MasterModel.name_hi = itm.scheme_name_hi;
    //            $scope.MasterModel.is_active = itm.is_active;
    //            $scope.MasterModel.purpose_id = itm.purpose_id == undefined ? 0 : itm.purpose_id;
    //        }
    //    });
    //}

    $scope.EditScheme = function (itm) {
        
        // $scope.ResetAddNew();
        $("#addModal").modal("show");
        //$scope.LoanSchemeList.forEach(function (itm, indx) {
        //    if (item == itm) {
        $scope.MasterModel.master_id = itm.scheme_id;
        $scope.MasterModel.name_en = itm.scheme_name_en;
        $scope.MasterModel.name_hi = itm.scheme_name_hi;
        $scope.MasterModel.is_active = itm.is_active == 1 ? true : false;;
        //$scope.MasterModel.purpose_id = itm.Purpose_id.toString();//== undefined ? 0 : itm.purpose_id.ToString();
        // $scope.MasterModel.sector_id = itm.sector_id;
        $scope.MasterModel.Farming_NonFarming = itm.Farming_NonFarming;
        $scope.LoadSector(itm.Farming_NonFarming);
        $scope.MasterModel.sector_id = itm.sector_id.toString();        
        //  }
        //});
    }

    $scope.ValidateSchemeMaster = function () {
        $("#name_en").removeClass("notValid");
        $("#name_hi").removeClass("notValid");
        var message = _language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.MasterModel.PerposeId == '0') {
            message += _language == 'en-us' ? '\n <br />Please Select Purpose .' : '\n <br />कृपया प्रयोजन चुनें।';
            $("#name_hi").addClass("notValid");
        }

        if ($scope.MasterModel.name_en.replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please enter scheme name in English.' : '\n <br />कृपया योजना का नाम अंग्रेजी में दर्ज करें।';
            $("#name_en").addClass("notValid");
        }
        if ($scope.MasterModel.name_hi.replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please enter scheme name in Hindi.' : '\n <br />कृपया योजना का नाम हिंदी में दर्ज करें।';
            $("#name_hi").addClass("notValid");
        }

        if (message.length > 29) {
            ShowModelAlert("Scheme Master", message);
            return false;
        }
        return true;
    }
    /*End PLDB Loan Scheme Master*/


    /*PLDB Loan Sector Master*/
    $scope.LoanSectorList = [];
    $scope.GetSectorMaster = function (WhereCase) {
        return $http.post(SiteUrl + "/PLDB/GetSectorMaster", { "WhereCase": WhereCase }).
            success(function (data, status) {
                $scope.LoanSectorList = [];
                $scope.LoanSectorList = data;
                $scope.TotalRecord = $scope.LoanSchemeList.length;

            }).
            error(function (data, status) {
                alert("Error while fetching Society Details.");

            });
    }

    $scope.GetSectorMaster("");
    $scope.SaveSectorMasterData = function () {
        if ($scope.ValidateSectorMaster()) {

            $(".blockUI").fadeIn("slow");

            var formData = {
                Id: $scope.MasterModel.master_id,
                IsActive: $scope.MasterModel.is_active,
                NameEn: $scope.MasterModel.name_en,
                NameHi: $scope.MasterModel.name_hi,
                SchemeId: $scope.MasterModel.scheme_id,
                Farming_NonFarming: $scope.MasterModel.Farming_NonFarming
            }

            $http.post(SiteUrl + "/PLDB/SectorMaster", formData).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (data.Status == 2) {
                        ShowOkMessage("Warning", $scope._language == 'en-us' ? "Sector Name Already Exist." : "सेक्टर का नाम पहले से मौजूद है", function () { });
                    }
                    else if (data.Status == 3) {

                        ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    }
                    else {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "Sector Name successfully submited." : "सेक्टर का नाम सफलतापूर्वक सबमिट किया गया", function () {

                            $(".se-pre-con").fadeIn("slow");
                            $("#addModal").modal("hide");
                            $scope.GetSectorMaster("");
                            $(".se-pre-con").fadeOut("slow");
                            // window.location.href = "/pldb/sectormaster";
                        });
                    }
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                });
        }
    }
    $scope.ValidateSectorMaster = function () {
        // alert($scope.MasterModel.Farming_NonFarming);
        $("#name_en").removeClass("notValid");
        $("#name_hi").removeClass("notValid");
        $("#scheme_id").removeClass("notValid");
        var message = _language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        if ($scope.MasterModel.Farming_NonFarming == "undefined" || $scope.MasterModel.Farming_NonFarming == "") {
            message += _language == 'en-us' ? '\n <br />Please select Loan Type .' : '\n <br />कृपया ऋण प्रकार का चयन करें।';
            $("#farming").addClass("notValid");
        }
        if ($scope.MasterModel.name_en == "") {
            message += _language == 'en-us' ? '\n <br />Please enter sector name in English.' : '\n <br />कृपया सेक्टर का नाम अंग्रेजी में दर्ज करें।';
            $("#name_en").addClass("notValid");
        }
        if ($scope.MasterModel.name_hi == "") {
            message += _language == 'en-us' ? '\n <br />Please enter sector name in Hindi.' : '\n <br />कृपया सेक्टर का नाम हिंदी में दर्ज करें।';
            $("#name_hi").addClass("notValid");
        }


        if (message.length > 29) {
            ShowOkMessage("Sector Master", message, function () {
            })

            return false;
        }
        return true;
    }
    $scope.EditSector = function (itm) {
        $scope.ResetAddNew();
        $("#addModal").modal("show");
        $scope.MasterModel.master_id = itm.sector_id;
        // $scope.MasterModel.scheme_id = itm.scheme_id.toString();
        $scope.MasterModel.name_en = itm.sector_name_en;
        $scope.MasterModel.name_hi = itm.sector_name_hi;
        $scope.MasterModel.is_active = itm.is_active == 1 ? true : false;
        $scope.MasterModel.Farming_NonFarming = itm.Farming_NonFarming;
    }
    /*End PLDB Loan Sector Master*/

    /*PLDB Purpose Master*/
    $scope.LoanPurposeList = [];
    $scope.MasterModel.FarmingSector_id = "";
    $scope.MasterModel.NonFarmingSector_id = "";

    $scope.LoadSector = function (type) {
        //$(".blockUI").fadeIn("slow");
        if (type == "Farming") {
            $scope.MasterModel.sector_id = $scope.MasterModel.FarmingSector_id;
        } else {
            $scope.MasterModel.sector_id = $scope.MasterModel.NonFarmingSector_id;
        }

        $scope.LoanSectorList.forEach(function (item, index) {
            if (type == item.Farming_NonFarming) {
                item.IsShow = true;
            } else {
                item.IsShow = false;
            }

        });
    }




    $scope.LoadSecheme = function (type) {
        //$(".blockUI").fadeIn("slow");
        if (type == "Farming") {
            $scope.MasterModel.sector_id = $scope.MasterModel.FarmingSector_id;
        } else {
            $scope.MasterModel.sector_id = $scope.MasterModel.NonFarmingSector_id;
        }

        $scope.LoanSchemeList.forEach(function (item, index) {
            if (type == item.Farming_NonFarming) {
                item.IsShow = true;
            } else {
                item.IsShow = false;
            }

        });
    }

    //$scope.LoadSector = function () {
    //    $(".blockUI").fadeIn("slow");
    //    $scope.GetSectorMaster($scope.MasterModel.Farming_NonFarming).then(function () { $(".blockUI").fadeOut("slow"); });
    //};

    $scope.GetPurposeMaster = function (WhereCase) {
        return $http.post(SiteUrl + "/PLDB/GetPurposeMaster", { "WhereCase": WhereCase }).
            success(function (data, status) {
                $scope.LoanPurposeList = [];
                $scope.LoanPurposeList = data;
                $scope.TotalRecord = $scope.LoanSchemeList.length;

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
                alert("Error while fetching Society Details.");

            });
    }
    $scope.SetStatusSector = function (sector_id, ActionId) {


        if (ActionId == 2) {
            ShowAlertMessage("Warning", $scope._language == 'en-us' ? "Are you sure to delete this record ?." : 'क्या आप इस रिकॉर्ड को हटाना सुनिश्चित कर रहे हैं? कृपया रिकॉर्ड को हटाने के लिए "Yes" बटन पर क्लिक करें ।', function () {
                $(".se-pre-con").fadeIn("slow");
                $http.post(SiteUrl + "/PLDB/DeleteandstatusSector", { Id: sector_id, Action: ActionId }).
                    success(function (data, status) {
                        //if (data > 0 && ActionId == 2) {
                        // alert('Record delete sucessfully!');
                        //}
                        $scope.GetSectorMaster("");
                        $(".se-pre-con").fadeOut("slow");
                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                    });

            });
        }
        else {
            $http.post(SiteUrl + "/PLDB/DeleteandstatusSector", { Id: sector_id, Action: ActionId }).
                success(function (data, status) {
                    //if (data > 0 && ActionId == 2) {
                    // alert('Record delete sucessfully!');
                    //}
                    $scope.GetSectorMaster("");
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                });
        }
    };

    $scope.SetStatusForscheme = function (scheme_id, ActionId) {

        if (ActionId == 2) {
            ShowAlertMessage("Warning", $scope._language == 'en-us' ? "Are you sure to delete this record ?." : 'क्या आप इस रिकॉर्ड को हटाना सुनिश्चित कर रहे हैं? कृपया रिकॉर्ड को हटाने के लिए "Yes" बटन पर क्लिक करें ।', function () {
                $(".se-pre-con").fadeIn("slow");
                $http.post(SiteUrl + "/PLDB/Deletescheme", { Id: scheme_id, Action: ActionId }).
                    success(function (data, status) {
                        //if (data > 0 && ActionId == 2) {
                        //alert('Record delete sucessfully!');
                        //}
                        $scope.GetSchemeMaster("");
                        $(".se-pre-con").fadeOut("slow");
                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                    });
            });
        }
        else {
            $http.post(SiteUrl + "/PLDB/Deletescheme", { Id: scheme_id, Action: ActionId }).
                success(function (data, status) {
                    //if (data > 0 && ActionId == 2) {
                        //alert('Record delete sucessfully!');
                    //}
                    $scope.GetSchemeMaster("");
                    $(".se-pre-con").fadeOut("slow");
                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                });
        }      
    };


    /*PLDB Purpose Master*/
    //File DropDown in schememaster
    //$scope.GetPurposeMasterList = function (WhereCase) {
    //    $(".se-pre-con").fadeIn("slow");
    //    return $http.get(SiteUrl + "/PLDB/GetPurposeMasterSchemeMasterList", { "WhereCase": WhereCase }).
    //        success(function (data, status) {
    //            $scope.LoanPurposeListData = [];
    //            $scope.LoanPurposeListData = data;
    //            $scope.TotalRecord = $scope.LoanSchemeList.length;
    //            $(".se-pre-con").fadeOut("slow");
    //            $scope.colsort = "";
    //            $scope.reverseSort = false;

    //            $scope.sortData = function (column) {
    //                $scope.reverseSort = ($scope.colsort == column) ? !$scope.reverseSort : false;
    //                $scope.colsort = column;
    //            };

    //            $scope.getSortClass = function (column) {
    //                if ($scope.colsort == column) {
    //                    return $scope.reverseSort ? 'arrow-down' : 'arrow-up'
    //                }
    //                return '';
    //            };

    //        }).
    //        error(function (data, status) {
    //            $(".se-pre-con").fadeOut("slow");
    //            alert("Error while fetching Society Details.");

    //        });
    //}

    $scope.SavePurposeMasterData = function () {
        if ($scope.ValidatePurposeMaster()) {

            $(".blockUI").fadeIn("slow");

            var formData = {
                Id: $scope.MasterModel.master_id,
                IsActive: $scope.MasterModel.is_active,
                NameEn: $scope.MasterModel.name_en,
                NameHi: $scope.MasterModel.name_hi,
                SectorId: $scope.MasterModel.sector_id,
                SchemeId: $scope.MasterModel.scheme_id,
                EffectiveDate: $("#EffectiveDate").val().split('/')[2] + "/" + $("#EffectiveDate").val().split('/')[1] + '/' + $("#EffectiveDate").val().split('/')[0],
                NormalInterestRate: $scope.MasterModel.normal_interest_rate,
                PenalInterestRate: $scope.MasterModel.penal_interest_rate,
                Farming_NonFarming: $scope.MasterModel.Farming_NonFarming,
                RepaymentPeriod: $scope.MasterModel.RepaymentPeriod,
                DueInstallment: $scope.MasterModel.DueInstallment,
                GracePeriod: $scope.MasterModel.GracePeriod,
                PurposeImageIcon: $scope.PurposeImageIcon.FileName,
                OrderOfPurpose: $scope.MasterModel.OrderOfPurpose

            }

            $http.post(SiteUrl + "/PLDB/PurposeMaster", formData).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (data.Status == 2) {
                        $(".blockUI").fadeOut("slow");
                        ShowOkMessage("Warning", $scope._language == 'en-us' ? "Purpose Name Already Exist." : "उद्देश्य का नाम पहले से मौजूद है", function () { });
                    }
                    else if (data.Status == 3) {

                        ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    }
                    else {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "Purpose Name successfully submited." : "उद्देश्य का नाम सफलतापूर्वक सबमिट किया गया", function () {
                            $(".se-pre-con").fadeOut("slow");
                            $scope.GetPurposeMaster("");
                            $scope.addPurpose = false;
                            $scope.purposeList = true;
                            //window.location.href = "/pldb/purposemaster";

                        });
                    }
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                });
        }
    }

    $scope.ValidatePurposeMaster = function () {
        $("#name_en").removeClass("notValid");
        $("#name_hi").removeClass("notValid");
        $("#scheme_id").removeClass("notValid");
        $("#sector_id").removeClass("notValid");
        $("#EffectiveDate").removeClass("notValid");
        $("#normal_interest_rate").removeClass("notValid");
        $("#penal_interest_rate").removeClass("notValid");
        var message = _language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';

        //if ($scope.MasterModel.scheme_id == "0") {
        //    message += _language == 'en-us' ? '\n <br />Please select scheme name.' : '\n <br />कृपया योजना का नाम चुनें।';
        //    $("#scheme_id").addClass("notValid");
        //}

        if ($scope.MasterModel.scheme_id == "0") {
            message += _language == 'en-us' ? '\n <br />Please select scheme.' : '\n <br />कृपया योजना का नाम चुनें।';
            $("#sector_id").addClass("notValid");
        }

        if ($scope.MasterModel.Farming_NonFarming == "") {
            message += _language == 'en-us' ? '\n <br />Please select Farming and Non-Farming.' : '\n <br />कृपया खेती और गैर-खेती का चयन करें।';
            $("#sector_id").addClass("notValid");
        }


        if ($scope.MasterModel.name_en.replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please enter Purpose name in English.' : '\n <br />कृपया उद्देश्य का नाम अंग्रेजी में दर्ज करें।';
            $("#name_en").addClass("notValid");
        }
        else {
            $("#name_en").removeClass("notValid");
        }

        if ($scope.MasterModel.name_hi.replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please enter Purpose name in Hindi.' : '\n <br />कृपया उद्देश्य का नाम हिंदी में दर्ज करें।';
            $("#name_hi").addClass("notValid");
        }
        else {
            $("#name_hi").removeClass("notValid");
        }

        if ($("#EffectiveDate").val().replace(/ /g, "") == "") {
            message += _language == 'en-us' ? '\n <br />Please select effective date.' : '\n <br />कृपया प्रभावी होने की तिथि चुनें।।';
            $("#EffectiveDate").addClass("notValid");
        }
        else {
            $("#EffectiveDate").removeClass("notValid");
        }

        if ($scope.MasterModel.normal_interest_rate == "") {
            message += _language == 'en-us' ? '\n <br />Please enter normal interest rate.' : '\n <br />कृपया सामान्य ब्याज दर दर्ज करें।';
            $("#normal_interest_rate").addClass("notValid");
        }
        else {
            $("#normal_interest_rate").removeClass("notValid");
        }

        if ($scope.MasterModel.penal_interest_rate == "") {
            message += _language == 'en-us' ? '\n <br />Please enter penal interest rate.' : '\n <br />कृपया दंड ब्याज दर दर्ज करें।';
            $("#penal_interest_rate").addClass("notValid");
        }
        else {
            $("#penal_interest_rate").removeClass("notValid");
        }

        if ($scope.PurposeImageIcon.FileName == '' || $scope.PurposeImageIcon.FileName == undefined) {
            if (message.indexOf($scope._language == 'en-us' ? 'Please upload the file' : 'कृपया फाइल अपलोड करें') < 0) {
                message += $scope._language == 'en-us' ? '<br />Please upload the file.' : '<br />कृपया फाइल  अपलोड करें।.';
            }
        }

        if ($scope.MasterModel.OrderOfPurpose == "") {
            message += _language == 'en-us' ? '\n <br />Please enter Order of purpose.' : '\n <br />कृपया उद्देश्य का क्रम दर्ज करें।';
            $("#OrderOfPurpose").addClass("notValid");
        }
        else {
            $("#OrderOfPurpose").removeClass("notValid");
        }

        if (message.length > 29) {
            ShowModelAlert("Sector Master", message);
            return false;
        }
        return true;
    }

    $scope.update = function (itm) {
        $(".se-pre-con").fadeIn("slow");
        $scope.purposeList = false;
        $scope.addPurpose = true;
        $("#addModal").removeClass("modal-backdrop");
        $(".se-pre-con").fadeOut("slow");
        if (itm.Farming_NonFarming == "Farming") {
            $scope.MasterModel.FarmingSector_id = itm.sector_id.toString();
            $scope.MasterModel.NonFarmingSector_id = "";
        } else {
            $scope.MasterModel.FarmingSector_id = "";
            $scope.MasterModel.NonFarmingSector_id = itm.sector_id.toString();
        }

        $("#EffectiveDate").val(itm.EffectiveDate);
        $scope.MasterModel.effective_date = itm.EffectiveDate;
        $scope.MasterModel.normal_interest_rate = itm.normal_interest_rate;
        $scope.MasterModel.penal_interest_rate = itm.penal_interest_rate;
        $scope.MasterModel.is_active = itm.is_active == 1 ? true : false;
        $scope.MasterModel.Farming_NonFarming = itm.Farming_NonFarming;
        $scope.MasterModel.RepaymentPeriod = itm.RepaymentPeriod;

        $scope.LoadSecheme(itm.Farming_NonFarming);
        $scope.MasterModel.master_id = itm.purpose_id.toString();
        $scope.MasterModel.name_en = itm.purpose_name_en;
        $scope.MasterModel.name_hi = itm.purpose_name_hi;

        $scope.MasterModel.DueInstallment = itm.DueInstallment.toString();
        $scope.MasterModel.GracePeriod = itm.GracePeriod;
        $scope.MasterModel.scheme_id = itm.scheme_id.toString();
        $scope.PurposeImageIcon.FileName = itm.PurposeImageIcon;
        $scope.PurposeImageIcon.IsUploadFileName = itm.PurposeImageIcon == "" ? false : true;;
        $scope.MasterModel.OrderOfPurpose = itm.OrderOfPurpose;
        //$scope.MasterModel.sector_id = itm.sector_id.toString();
        $(".blockUI").fadeOut("slow");
    };

    //Delete Status Purchase Center
    $scope.SetStatus = function (purpose_id, ActionId) {

        if (ActionId == 2) {
            ShowAlertMessage("Warning", $scope._language == 'en-us' ? "Are you sure to delete this record ?." : 'क्या आप इस रिकॉर्ड को हटाना सुनिश्चित कर रहे हैं? कृपया रिकॉर्ड को हटाने के लिए "Yes" बटन पर क्लिक करें ।', function () {
                $(".se-pre-con").fadeIn("slow");
                $http.post(SiteUrl + "/PLDB/ChangedeleteStatusPurposeMaster", { "Id": purpose_id, "ActionId": ActionId }).
                    success(function (data, status) {
                       
                        $scope.GetPurposeMaster("");
                        $(".se-pre-con").fadeOut("slow");

                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                    });

            });
        }
        else {
            $http.post(SiteUrl + "/PLDB/ChangedeleteStatusPurposeMaster", { "Id": purpose_id, "ActionId": ActionId }).
                success(function (data, status) {
                    $(".se-pre-con").fadeIn("slow");
                    $scope.GetPurposeMaster("");
                    $(".se-pre-con").fadeOut("slow");

                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                });
        }
    };

    /*End PLDB Purpose Master*/
    $scope.OnPageLoad();





    $scope.FileType = "";

    $scope.AddFile = function (type) {
        $scope.FileType = type;
    }
    $scope.ChangeFile = function (type) {
        if (type == 1) {
            $scope.PurposeImageIcon.IsUploadFileName = false;
            $scope.PurposeImageIcon.FileName = "";
        }
    }

    $scope.UploadFile = function (id, type) {
        try {
            // FormData
            if (window.FormData !== undefined) {
                var fileUpload = $("#" + id).get(0);
                var files = fileUpload.files;
                //var blob = b64toBlob(realData, contentType, 512);
                var formData = new FormData();
                formData.append("pldbfile", files[0]);
                $(".blockUI").show();
                $scope.xhr(SiteUrl + "/PLDB/UploadLoanFile", formData, type);
            } else {
                alert("FormData is not supported.");
            }
        }
        catch (ex) { alert(ex) }
    }

    $scope.xhr = function (url, data, type) {
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
                            $scope.PurposeImageIcon.FileName = request.responseText;
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

    $scope.PurposeImageIcon = {
        IsUploadFileName: false,
        FileName: "",
    }




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
                        } else {
                            if (scope.FileType == 1) {
                                scope.PurposeImageIcon.IsUploadFileName = true;
                                scope.PurposeImageIcon.FileName = "";
                            }
                            scope.$apply();
                        }
                    }

                });
            };
        }
    };
});




