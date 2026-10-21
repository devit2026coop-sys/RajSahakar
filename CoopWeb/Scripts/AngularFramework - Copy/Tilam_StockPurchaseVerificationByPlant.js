var TilamSangh_StockPurchaseResponseByPlantController = angular.module("TilamSangh_StockPurchaseResponseByPlantCnt", ['angularUtils.directives.dirPagination']);
TilamSangh_StockPurchaseResponseByPlantController.controller("TilamSangh_StockPurchaseResponseByPlantMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridProcessingUnit = true;
    $scope.divInsertProcessingUnit = false;
    $scope.IsDeliveryAccepted = true;
    $scope.HqResponse = [];
    $scope.divOrderDetails = false;

    //START: Change Radio Button for Action/decision
    $scope.RadioChangeDeliveryStatus = function () {
        var val = parseInt($scope.PlantResponse.IsPurchaseAccepted, 10);
        if (val === 1) {
            $scope.IsDeliveryAccepted = true;
        }
        else {
            $scope.IsDeliveryAccepted = false;
        }
    };
    //START: Change Radio Button for Action/decision

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

    // START : Get Purchase Orders List
    $scope.GetPurchaseOrderList = function () {
        $http.post(SiteUrl + "/Stock/GetShowInitialStockData").
            success(function (data, status) {
                if (data.length === 2) {
                    $scope.divOrderDetails = false;
                    ShowModelAlert($scope.translation.StockTransferResponsebyPlant, _language === 'en-us' ? 'No data available' :'कोई डेटा उपलब्ध नहीं है' , function () { });
                }
                else {
                    $scope.divOrderDetails = true;
                    $scope.PurchaseOrders = data;
                }

                console.log(data);
            }).
            error(function (data1, status) {
                $scope.PurchaseOrders = [];
            });
    };
    // END : Get Purchase Orders List

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

    //START : Clear Processing Unit Form
    $scope.ClearProcessingUnitForm = function () {
        $scope.ProcessingUnit.ProcessingUnitNameEng = '';
        $scope.ProcessingUnit.ProcessingUnitNameHi = '';
        $scope.ProcessingUnit.ProcessingUnitId = '0';
        $('input[type=checkbox]').prop('checked', false);
    };
    //END : Clear Processing Unit Form


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

        if (parseInt($scope.HqResponse.IsApprovedByHQ) === 2 && parseInt($scope.HqResponse.RejectionIdByHQ) < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please select rejection reason.' : '\n <br /> कृपया अस्वीकृति कारण चुनें|';
        }

        if (parseInt($scope.HqResponse.IsDebitNote) === 1 && $scope.DebitNoteFile.length === 0) {
            message += $scope._language === 'en-us' ? '\n <br /> Please provide Debit Note File.' : '\n <br /> कृपया डेबिट नोट फ़ाइल प्रदान करें।';
        }

        if ($scope.PlantResponse.Remarks === undefined || $scope.PlantResponse.Remarks.length === 0) {
            message += $scope._language === 'en-us' ? '\n <br /> Please write comment' : '\n <br /> कृपया टिप्पणी लिखें';
        }

        if (message.length > 5) {
            ShowModelAlert($scope.translation.StockPurchaseVerificationByHq, message);
            return false;
        }
        return true;
    };
    //END : Validate Form


    //START : Set Details Id
    $scope.SetDetails = function (id, status) {
        $scope.PurchaseRegisterDetailId = id;
        if (status === false) {
            ShowModelAlert($scope.translation.StockTransferResponsebyPlant, _language === 'en-us' ? 'Action has been taken. No further action required.' : 'कार्रवाई की गई है। आगे कोई कार्रवाई की आवश्यकता नहीं है।', function () { });
        }
        
    };
    //END : Set Details Id


    $scope.pageChangeHandler = function (num) {
    };

    // START : Upload File
    $scope.UploadDeliveryApprovedFile = function (file) {
        $scope.DebitNoteFile = file[0];
    };
    //END : Upload File

    //START : InsertHqResponseOnPurchaseOrder
    $scope.SaveStockDeliveryStatus = function () {


        if (!$scope.ValidateForm()) {
            return false;
        }

        $(".se-pre-con").fadeIn("slow");
        var isAccepted = parseInt($scope.PlantResponse.IsPurchaseAccepted);
        var isQualityCheck = parseInt($scope.PlantResponse.IsQualityProduct);
        var formData = new FormData();
        formData.append("PurchaseRegisterDetailId", $scope.PurchaseRegisterDetailId);
        formData.append("IsAccepted", isAccepted === 1 ? true : false);
        formData.append("IsQualityCheckOkay", isQualityCheck === 1 ? true : false);
        formData.append("RejectionId", isAccepted === 1 ? '0' : $scope.PlantResponse.RejectionReasonId);
        formData.append("RemarkByHQ", $scope.PlantResponse.Remarks);
        formData.append("AcceptanceFile", $scope.DebitNoteFile);
        formData.append("mode", 1);

        $http.post(SiteUrl + "/Stock/SaveResponseOnPurchseDetails", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).
            success(function (data, status) {
                var result = parseInt(data);

                if (result > 0) {
                    alert('Data Saved succesfully.');
                    window.location.reload();
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
    //END : InsertHqResponseOnPurchaseOrder

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

    //START : Get Purchase Register Details/Vehicle Status
    $scope.VehicleDetailStatus = function (purchaseRegisterId) {

        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Stock/StockPurchaseDetailById", { "PurchaseRegisterId": purchaseRegisterId, "PurchaseRegisterDetailId": 0 }).
            success(function (data, status) {
                $scope.VehicleDetails = data;
            }).
            error(function (data1, status) {
                $scope.VehicleDetails = [];
                $(".se-pre-con").fadeOut("slow");
            });

        $(".se-pre-con").fadeOut("slow");
    };
    //END : Get Purchase Register Details/Vehicle Status

    // START : On Page Load
    $scope.OnPageLoad();
    $scope.RejectReasonList();
    $scope.GetPurchaseOrderList();
    // END : On Page Load
});