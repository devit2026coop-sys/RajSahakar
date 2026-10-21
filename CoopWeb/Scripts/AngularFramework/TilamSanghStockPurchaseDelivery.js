var TilamSanghStockPurchaseDeliveryController = angular.module("TilamSanghStockPurchaseDeliveryCnt", ['angularUtils.directives.dirPagination']);
TilamSanghStockPurchaseDeliveryController.controller("TilamSanghStockPurchaseDeliveryMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridProcessingUnit = true;
    $scope.divInsertProcessingUnit = false;
    $scope.PurchaseDetails = [];
    $scope.PurchaseRegister = [];

    //START : Purchase Details Entity
    function PurchaseRegisterDetailEntity() {
        this.VehicleNo = '';
        this.Quantity = 0;
    }
    //END : Stock Purchase Details

    //START : Add New Row For Stock Details
    $scope.AddPurchaseDetails = function () {
        $scope.PurchaseDetails.push(new PurchaseRegisterDetailEntity());
    };
    //END : Add New Row For Stock Details

    //START : Remove Row For Stock Details
    $scope.RemovePurchaseDetails = function (index) {
        $scope.PurchaseDetails.splice(index, 1);
    };
    //END : Remove Row For Stock Details

    // START : Page load Function
    $scope.OnPageLoad = function () {

        $scope._language = _language;

        if (_language === 'en-us') {
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

        $('.SetCalender').datepicker({
            showOn: "both",
            changeMonth: true,
            buttonImage: "../images/calender.png",
            changeYear: true,
            dateFormat: "dd/mm/yy",
            minDate: '0'
        });

        $scope.PurchaseDetails.push(new PurchaseRegisterDetailEntity());
    };
    // END : Page load Function

    // START : Get Processing Unit List
    $scope.ProcessingUnitList = function () {
        $http.post(SiteUrl + "/Stock/ProcessingUnitList", { "ProcessingUnitId": "0" }).
            success(function (data, status) {
                $scope.ProcessingUnits = data.filter(p => (p.IsOperating === 'True' || p.IsOperating === true) && (p.IsActive === 'True' || p.IsActive === true));
            }).
            error(function (data1, status) {
                $scope.ProcessingUnits = [];
            });
    };
    // END : Get Processing Unit List

    // START : Get Product List
    $scope.ProductList = function () {
        $http.post(SiteUrl + "/Stock/ProductList", { "ProductId": "0" }).
            success(function (data, status) {
                $scope.Products = data;
            }).
            error(function (data1, status) {
                $scope.Products = [];
            });
    };
    // END : Get Product List


    // START : Get Product Sub Category List
    $scope.ProductSubCategoryList = function () {
        $http.post(SiteUrl + "/Stock/ProductSubCategoryList", { "ProductSubCategoryId": "0", "ProductId": $scope.PurchaseRegister.ProductId }).
            success(function (data, status) {
                $scope.ProductSubCategories = data;
            }).
            error(function (data1, status) {
                $scope.ProductSubCategories = [];
            });
    };
    // END : Get Product Sub Category List

    // START : Get Unit List
    $scope.UnitList = function () {
        $http.post(SiteUrl + "/Stock/UnitMasterList", { "UnitId": "0" }).
            success(function (data, status) {
                $scope.Units = data;
            }).
            error(function (data1, status) {
                $scope.Units = [];
            });
    };
    // END : Get Unit List

    //START : Clear Processing Unit Form
    $scope.ClearProcessingUnitForm = function () {
        $scope.ProcessingUnit.ProcessingUnitNameEng = '';
        $scope.ProcessingUnit.ProcessingUnitNameHi = '';
        $scope.ProcessingUnit.ProcessingUnitId = '0';
        $('input[type=checkbox]').prop('checked', false);
    };
    //END : Clear Processing Unit Form

    //START : Show Div For Processing Unit Insert
    $scope.ShowDivProcessingUnitInsert = function () {
        $scope.GridProcessingUnit = false;
        $scope.divInsertProcessingUnit = true;
    };
    //END : Show Div For Processing Unit Insert

    //START : Back To Processing Unit List
    $scope.BackToProcessingUnitList = function () {
        $scope.ShowDivProcessingUnitInsert();
        $scope.GridProcessingUnit = true;
        $scope.divInsertProcessingUnit = false;
    };
    //END : Back To Processing Unit List

    //START : Validate Form
    $scope.ValidateForm = function () {
        var message = '';
        if ($scope.PurchaseRegister.ProcessingUnitId === '0') {
            message += $scope._language === 'en-us' ? '\n <br /> Please select Processing Unit/Plant' : '\n <br /> कृपया प्रसंस्करण इकाई/प्लांट चुने |';
        }
        if ($scope.PurchaseRegister.PartyName === undefined || $scope.PurchaseRegister.PartyName.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Party Name (in Eng)' : '\n <br /> कृपया पार्टी नाम (इंग्लिश) दर्ज करें  |';
        }
        if ($scope.PurchaseRegister.PartyNameHi === undefined || $scope.PurchaseRegister.PartyNameHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Party Name (in Hindi)' : '\n <br /> कृपया पार्टी नाम (हिंदी) दर्ज करें  |';
        }
        if ($scope.PurchaseRegister.BrokerName === undefined || $scope.PurchaseRegister.BrokerName.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Broker Name (in Eng)' : '\n <br /> कृपया ब्रोकर नाम (इंग्लिश) दर्ज करें  |';
        }
        if ($scope.PurchaseRegister.BrokerNameHi === undefined || $scope.PurchaseRegister.BrokerNameHi.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Broker Name (in Hindi)' : '\n <br /> कृपया ब्रोकर नाम (हिंदी) दर्ज करें  |';
        }
        if ($scope.PurchaseRegister.ProductId === "0") {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select Product' : '\n <br /> कृपया उत्पाद चुने |';
        }
        if ($scope.PurchaseRegister.ProductSubCategoryId === "0") {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select Product Sub Category' : '\n <br /> कृपया उत्पाद उप श्रेणी चुने |';
        }
        if ($scope.PurchaseRegister.UnitId === "0") {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select Unit' : '\n <br /> कृपया यूनिट चुने |';
        }
        if ($scope.PurchaseRegister.Quantity === undefined || $scope.PurchaseRegister.Quantity.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Quantity' : '\n <br /> कृपया मात्रा दर्ज करें  |';
        }
        if ($scope.PurchaseRegister.RateUnitId === "0") {
            message += $scope._language === 'en-us' ? '\n <br /> Please Select Rate Unit' : '\n <br /> कृपया दर मात्रक चुने|';
        }
        if ($scope.PurchaseRegister.Rate === undefined || $scope.PurchaseRegister.Rate.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Rate' : '\n <br /> कृपया दर (रु.में) दर्ज करें  |';
        }
        if ($scope.PurchaseRegister.RateQuantity === undefined || $scope.PurchaseRegister.RateQuantity.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Rate Quantity' : '\n <br /> कृपया मात्रा दर्ज करें  |';
        }
        if ($scope.PurchaseRegister.PurchasePaymentDate === undefined || $scope.PurchaseRegister.PurchasePaymentDate.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Purchase Payment Date' : '\n <br /> कृपया भुगतान तिथि दर्ज करें  |';
        }
        if ($scope.PurchaseRegister.ExpectedDeliveryDate === undefined || $scope.PurchaseRegister.ExpectedDeliveryDate.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Expected Delivery Date' : '\n <br /> कृपया अपेक्षित वितरण की तारीख दर्ज करें  |';
        }
        if ($scope.PurchaseRegister.SaleContractNo === undefined || $scope.PurchaseRegister.SaleContractNo.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Sale Contract Number' : '\n <br /> कृपया बिक्री अनुबंध संख्या दर्ज करें  |';
        }
        if ($scope.PurchaseRegister.Conditions === undefined || $scope.PurchaseRegister.Conditions.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Condition(s)' : '\n <br /> कृपया शर्तेँ दर्ज करें  |';
        }
        if ($scope.PurchaseRegister.Remark === undefined || $scope.PurchaseRegister.Remark.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter Remark' : '\n <br /> कृपया टिप्पणी दर्ज करें |';
        }

        var IsValidStockDetails = true;
        var quantity = 0;
        angular.forEach($scope.PurchaseDetails, function (i, j) {
            quantity += parseFloat(j.Quantity);

            if (IsValidStockDetails === false) {
                return;
            }
            else {
                if ((j.VehicleNo === undefined || j.VehicleNo.length === 0) || (j.Quantity === undefined || j.VehicleNo.Quantity === 0)) {
                    IsValidStockDetails = false;
                }
            }
        });

        if (IsValidStockDetails === false) {
            message += $scope._language === 'en-us' ? '\n <br /> Please Enter all details for Stock Details' : '\n <br /> कृपया स्टॉक खरीद सम्बंधित समस्त विवरण दर्ज करें|';
        }

        if (parseFloat(quantity) !== parseFloat($scope.PurchaseRegister.Quantity)) {
            message += $scope._language === 'en-us' ? '\n <br /> Stock Details quantity is not matched with total quantity.' : '\n <br /> स्टॉक विवरण मात्रा कुल मात्रा के साथ मेल नहीं खाती है|';
        }

        if (message.length > 5) {

            ShowModelAlert($scope.translation.StockPurchase, message);
            return false;
        }
        return true;
    };
    //END : Validate ValidateForm

    //START : Save/Edit Stock Purchase
    $scope.SaveStockPurchase = function () {
        $(".se-pre-con").fadeIn("slow");

        if (!$scope.ValidateForm()) {
            $(".se-pre-con").fadeOut("slow");
            return false;
        }

        var obj = {
            "ProcessingUnitId": $scope.PurchaseRegister.ProcessingUnitId,
            "PartyName": $scope.PurchaseRegister.PartyName,
            "PartyNameHi": $scope.PurchaseRegister.PartyNameHi,
            "BrokerName": $scope.PurchaseRegister.BrokerName,
            "BrokerNameHi": $scope.PurchaseRegister.BrokerNameHi,
            "ProductId": $scope.PurchaseRegister.ProductId,
            "ProductSubCategoryId": $scope.PurchaseRegister.ProductSubCategoryId,
            "UnitId": $scope.PurchaseRegister.UnitId,
            "Quantity": $scope.PurchaseRegister.Quantity,
            "RateUnitId": $scope.PurchaseRegister.RateUnitId,
            "Rate": $scope.PurchaseRegister.Rate,
            "RateQuantity": $scope.PurchaseRegister.RateQuantity,
            "PurchasePaymentDate": $('#txtPurchasedate').val(),
            "ExpectedDeliveryDate": $('#txtDeliverydate').val(),
            "SaleContractNo": $scope.PurchaseRegister.SaleContractNo,
            "Conditions": $scope.PurchaseRegister.Conditions,
            "Remark": $scope.PurchaseRegister.Remark,
            "StockDetails": $scope.PurchaseDetails
        };

        $http.post(SiteUrl + "/Stock/InsertStockPurchase/", { "model": obj }).
            success(function (data, status) {
                if (data.length > 0) {
                    $scope.RequestForm = true;
                    $(".se-pre-con").fadeOut("slow");
                    window.location.reload();
                }
                else {
                    $scope.RequestForm = true;
                    alert((_language === 'en-us' ? 'data1\n' : 'data2\n'));
                    $(".se-pre-con").fadeOut("slow");
                }
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
            });

        $(".se-pre-con").fadeOut("slow");
    };
    //END : Save/Edit Stock Purchase

    $scope.pageChangeHandler = function (num) {
    };

    //START : Set Data For Update
    $scope.SetDataForUpdate = function (item) {
        $scope.showtringdiv = true;
        $scope.Trainning = item;
        $scope.Trainning.TrainningId = item.TrainningId;
        $scope.Trainning.ProgrammeNameEn = item.ProgrammeNameEn;
        $scope.Trainning.IsActive = item.IsActive === 1 ? true : false;
    };
    //END : Set Data For Update

    //START : Set Status
    $scope.SetStatus = function (TrainningId, ActionId) {
        $http.post(SiteUrl + "/Trainning/DeleteandstatusTrainning", { Id: TrainningId, Action: ActionId == true ? "1" : "0" }).
            success(function (data, status) {

                $scope.GetAllTrainningList();

            }).
            error(function (data, status) {
            });
    };
    //END : Set Status

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.ProcessingUnitList();
    $scope.ProductList();
    $scope.UnitList();
    // END : On Page Load
});


var TilamSanghStockPurchaseDeliveryController2 = angular.module("TilamSanghStockPurchaseDeliveryCnt2", ['angularUtils.directives.dirPagination']);
TilamSanghStockPurchaseDeliveryController2.controller("TilamSanghStockPurchaseDeliveryMod2", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridProcessingUnit = true;
    $scope.divInsertProcessingUnit = false;
    $scope.IsDeliveryAccepted = true;

    // START : Page load Function
    $scope.OnPageLoad = function () {

        $scope._language = _language;

        if (_language === 'en-us') {
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
    // END : Page load Function

    // START : Get Stock Purchase List
    $scope.StockPurchaseList = function () {
        $http.post(SiteUrl + "/Stock/StockPurchaseRequestList", { "ProcessingUnitId": "1" }).
            success(function (data, status) {
                $scope.StockRequestDispatch = data;
            }).
            error(function (data1, status) {
                $scope.StockRequestDispatch = [];
            });
    };
    // END : Get Stock Purchase List

    // START : Get Rejection Reasons List
    $scope.RejectReasonList = function () {
        $http.post(SiteUrl + "/Stock/RejectionReasonList").
            success(function (data, status) {
                $scope.RejectionReasons = data;
            }).
            error(function (data1, status) {
                $scope.RejectionReasons = [];
            });
    };
    // END : Get Rejection Reasons List

    //START: Change Radio Button for Action/decision
    $scope.RadioChangeDeliveryStatus = function () {
        var val = parseInt($scope.StockDelivery.IsPurchaseAccepted, 10);
        if (val === 1) {
            $scope.IsDeliveryAccepted = true;
        }
        else {
            $scope.IsDeliveryAccepted = false;
        }
    };
    //START: Change Radio Button for Action/decision

    //START : Clear Processing Unit Form
    $scope.ClearProcessingUnitForm = function () {
        $scope.ProcessingUnit.ProcessingUnitNameEng = '';
        $scope.ProcessingUnit.ProcessingUnitNameHi = '';
        $scope.ProcessingUnit.ProcessingUnitId = '0';
        $('input[type=checkbox]').prop('checked', false);
    };
    //END : Clear Processing Unit Form

    //START : Show Div For Processing Unit Insert
    $scope.ShowDivProcessingUnitInsert = function () {
        $scope.GridProcessingUnit = false;
        $scope.divInsertProcessingUnit = true;
    };
    //END : Show Div For Processing Unit Insert

    //START : Back To Processing Unit List
    $scope.BackToProcessingUnitList = function () {
        $scope.ShowDivProcessingUnitInsert();
        $scope.GridProcessingUnit = true;
        $scope.divInsertProcessingUnit = false;
    };
    //END : Back To Processing Unit List

    //START : Validate Stock Delivery Form
    $scope.ValidateStockDeliveryForm = function () {
        var message = '';
        if ($scope.StockDelivery === undefined || $scope.StockDelivery.IsPurchaseAccepted < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please chosse Yes/No for Is Delivery Accepted?' : '\n <br /> कृपया डिलीवरी के लिए हां/नहीं का चयन करें?';
        }
        if ($scope.StockDelivery.IsPurchaseAccepted !== undefined) {
            if (parseInt($scope.StockDelivery.IsPurchaseAccepted) === 1 && ($scope.BulkInsertFile === undefined || $scope.BulkInsertFile.length === 0)) {
                message += $scope._language === 'en-us' ? '\n <br /> Please provide acceptance file' : '\n <br /> कृपया स्वीकृति फ़ाइल प्रदान करें|';
            }
            if (parseInt($scope.StockDelivery.IsPurchaseAccepted) === 2 && $scope.StockDelivery.RejectionReasonId < 1) {
                message += $scope._language === 'en-us' ? '\n <br /> Please select rejection reason.' : '\n <br /> कृपया अस्वीकृति कारण चुनें।|';
            }
        }

        if ($scope.StockDelivery.Remarks === undefined || $scope.StockDelivery.Remarks.length < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please write comments for acceptance/rejection.' : '\n <br /> कृपया स्वीकृति / अस्वीकृति के लिए टिप्पणी लिखें।';
        }


        if (message.length > 5) {
            ShowModelAlert($scope.translation.StockDeliveryApprovalDepot, message);
            return false;
        }
        return true;
    };
    //END : Validate Stock Delivery Form

    //START : Show Delevery Status Div
    $scope.ShowDeleveryStatusDiv = function (item) {
        $scope.PurchaseRegisterId = item.PurchaseRegisterId;
    };
    //START : Show Delevery Status Div

    //START : Upload File
    $scope.UploadDeliveryApprovedFile = function (file) {
        $scope.BulkInsertFile = file[0];
    };
    //START : Upload File

    //START : Save/Edit Stock Delivery Status
    $scope.SaveStockDeliveryStatus = function () {


        if (!$scope.ValidateStockDeliveryForm()) {
            return false;
        }

        $(".se-pre-con").fadeIn("slow");
        var rdoSelectedValue = parseInt($scope.StockDelivery.IsPurchaseAccepted);
        var formData = new FormData();
        formData.append("PurchaseRegisterId", $scope.PurchaseRegisterId);
        formData.append("IsPurchaseAccepted", rdoSelectedValue);
        formData.append("RejectionReasonId", rdoSelectedValue === 1 ? '0' : $scope.StockDelivery.RejectionReasonId);
        formData.append("Remarks", $scope.StockDelivery.Remarks);
        formData.append("FileName", $scope.BulkInsertFile);
        $http.post(SiteUrl + "/Stock/SaveStockDeliveryStatus", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                var result = parseInt(data);

                if (result > 0) {
                    alert('Data Saved succesfully.');
                }
                else {
                    alert('Error');
                }
            }).
            error(function (data, status) {
                alert("Error while data save");
            });

        $(".se-pre-con").fadeOut("slow");
    };
    //END : Save/Edit Save/Edit Stock Delivery Status

    $scope.pageChangeHandler = function (num) {
    };

    //START : Set Data For Update
    $scope.SetDataForUpdate = function (item) {
        $scope.showtringdiv = true;
        $scope.Trainning = item;
        $scope.Trainning.TrainningId = item.TrainningId;
        $scope.Trainning.ProgrammeNameEn = item.ProgrammeNameEn;
        $scope.Trainning.IsActive = item.IsActive === 1 ? true : false;
    };
    //END : Set Data For Update

    //START : Set Status
    $scope.SetStatus = function (TrainningId, ActionId) {
        $http.post(SiteUrl + "/Trainning/DeleteandstatusTrainning", { Id: TrainningId, Action: ActionId == true ? "1" : "0" }).
            success(function (data, status) {

                $scope.GetAllTrainningList();

            }).
            error(function (data, status) {
            });
    };
    //END : Set Status

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.StockPurchaseList();
    $scope.RejectReasonList();
    // END : On Page Load
});