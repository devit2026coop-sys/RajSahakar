var TilamSangh_StockTransferTrackingByHqController = angular.module("TilamSangh_StockPurchaseTrackingByHqCnt", ['angularUtils.directives.dirPagination']);
TilamSangh_StockTransferTrackingByHqController.controller("TilamSangh_StockPurchaseTrackingByHqMod", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.GridProcessingUnit = true;
    $scope.divInsertProcessingUnit = false;

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

    // START : Get Purchase Orders List
    $scope.GetPurchaseOrderList = function () {
        $http.post(SiteUrl + "/Stock/GetShowInitialStockData").
            success(function (data, status) {
                $scope.PurchaseOrders = data;
                console.log(data);
            }).
            error(function (data1, status) {
                $scope.PurchaseOrders = [];
            });
    };
    // END : Get Purchase Orders List

    //START : Clear Form
    $scope.ClearForm = function () {
        $scope.HqResponse.IsApprovedByHQ = '1';
        $scope.HqResponse.RejectionIdByHQ = '0';
        $scope.HqResponse.IsDebitNote = 2;
        $scope.DebitNoteFile = '';
        $scope.HqResponse.RemarkByHQ = '';
    };
    //END : Clear Form

    //START : Validate Form
    $scope.ValidateForm = function () {
        var message = '';

        if (parseInt($scope.HqResponse.IsApprovedByHQ) === 2 && parseInt($scope.HqResponse.RejectionIdByHQ) < 1) {
            message += $scope._language === 'en-us' ? '\n <br /> Please select rejection reason.' : '\n <br /> कृपया अस्वीकृति कारण चुनें|';
        }

        if (parseInt($scope.HqResponse.IsDebitNote) === 1 && $scope.DebitNoteFile === undefined) {
            message += $scope._language === 'en-us' ? '\n <br /> Please provide Debit Note File.' : '\n <br /> कृपया डेबिट नोट फ़ाइल प्रदान करें।';
        }

        if ($scope.HqResponse.RemarkByHQ === undefined || $scope.HqResponse.RemarkByHQ.length === 0) {
            message += $scope._language === 'en-us' ? '\n <br /> Please write comment' : '\n <br /> कृपया टिप्पणी लिखें';
        }

        if (message.length > 5) {
            ShowModelAlert($scope.translation.StockPurchaseVerificationByHq, message);
            return false;
        }
        return true;
    };
    //END : Validate Form

    $scope.pageChangeHandler = function (num) {
    };

    // START : Upload File
    $scope.UploadDeliveryApprovedFile = function (file) {
        $scope.AceptanceFile = file[0];
    };
    //END : Upload File

    //START : Set Details Id
    $scope.SetDetails = function (id, status) {
        $scope.PurchaseRegisterDetailId = id;
        if (status === false) {
            ShowModelAlert($scope.translation.StockPurchaseVerificationByHq, _language === 'en-us' ? 'Action has been taken. No further action required.' : 'कार्रवाई की गई है। आगे कोई कार्रवाई की आवश्यकता नहीं है।', function () { });
        }
    };
    //END : Set Details Id

    // START : Upload File
    $scope.UploadFile = function (file) {
        $scope.DebitNoteFile = file[0];
    };
    //END : Upload File

    //START : Save/Edit Stock Delivery Status
    $scope.InsertHqResponseOnPurchaseOrder = function () {

        if (!$scope.ValidateForm()) {
            return false;
        }

        $(".se-pre-con").fadeIn("slow");
        var isAccepted = parseInt($scope.HqResponse.IsApprovedByHQ);
        var isDebitNote = parseInt($scope.HqResponse.IsDebitNote);
        var formData = new FormData();
        formData.append("PurchaseRegisterDetailId", $scope.PurchaseRegisterDetailId);
        formData.append("IsAccepted", isAccepted === 1 ? true : false);
        formData.append("RejectionId", isAccepted === 1 ? '0' : $scope.HqResponse.RejectionIdByHQ);
        formData.append("IsDebitNote", isDebitNote === 1 ? true : false);
        formData.append("RemarkByHQ", $scope.HqResponse.RemarkByHQ);
        formData.append("AcceptanceFile", $scope.DebitNoteFile);
        formData.append("PlantId", $scope.PurchaseOrders.ProcessingUnitId);
        formData.append("ProductSubCategoryId", $scope.PurchaseOrders.ProductSubCategoryId);

        formData.append("mode", 2);

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
    //END : Save/Edit Save/Edit Stock Delivery Status

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
    $scope.VehicleDetailStatus = function (purchaseRegisterId, detailsId) {
        $(".se-pre-con").fadeIn("slow");
        $http.post(SiteUrl + "/Stock/StockPurchaseDetailById", { "PurchaseRegisterId": 1, "PurchaseRegisterDetailId": 0 }).
            success(function (data, status) {
                $scope.VehicleDetails = data;
                console.log(data);
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