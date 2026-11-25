var MasterMod = angular.module('auditorBankReconciliotionMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("auditorBankReconciliotionCtrl", function ($scope, $http) {
    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.Department = [];
    $scope.divClass = false;

    // on page load
    $scope.OnPageLoad = function () {
        $scope._language = 'en-us';
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

        $scope.GetIssuedCheques();
    };

    $scope.BackTo = function () {
        window.location.href = '/Auditor/AuditorAssestAndExpences';
    };

    $scope.NextStep = function () {
        window.location.href = '/Auditor/AuditorDepreciationSchedule';
    };

    $scope.IssuedChequeList = [
        {
            Date: null,
            ChequeNo: '',
            PartyName: '',
            BankName: '',
            Amount: null
        }
    ];

    $scope.AddIssuedCheque = function (index) {
        var newRow = {
            Date: '',
            ChequeNo: '',
            IssuedTo: '',
            BankName: '',
            Amount: 'Amount'
        };

        $scope.IssuedChequeList.splice(index + 1, 0, newRow);
    };

    $scope.RemoveIssuedCheque = function (index) {
        $scope.IssuedChequeList.splice(index, 1);
    };

    // Calculate Total Amount
    $scope.GetIssuedChequeTotal = function () {
        var total = 0;
        angular.forEach($scope.IssuedChequeList, function (item) {
            total += parseFloat(item.Amount) || 0;
        });

        return total;
    };


    $scope.IssuedChequeList = [];

    $scope.GetIssuedCheques = function () {

        $(".se-pre-con").fadeIn("slow");

        $http.get(SiteUrl + "/Auditor/GetIssuedCheques")
            .then(function (response) {

                console.log("Issued Cheques Response:", response.data);

                var data = response.data;

                // If API returns JSON string
                if (typeof data === 'string') {
                    data = JSON.parse(data);
                }

                if (data && data.length > 0) {

                    $scope.IssuedChequeList = data.map(function (item) {

                        return {
                            IssuedChequeId: item.IssuedChequeId,
                            Date: item.ChequeDate ? new Date(item.ChequeDate) : null,
                            ChequeNo: item.ChequeNo || '',
                            PartyName: item.PartyName || '',
                            BankName: item.BankName || '',
                            Amount: parseFloat(item.Amount) || 0,
                            IsFinalSubmit: parseInt(item.IsFinalSubmit) || 0
                        };

                    });

                }
                else {

                    // If no data found
                    $scope.IssuedChequeList = [{
                        Date: null,
                        ChequeNo: '',
                        PartyName: '',
                        BankName: '',
                        Amount: 0,
                        IsFinalSubmit: 0
                    }];

                }

                console.log("Final IssuedChequeList:", $scope.IssuedChequeList);

                $(".se-pre-con").fadeOut("slow");

            }, function (error) {

                $(".se-pre-con").fadeOut("slow");

                console.log("GetIssuedCheques Error:", error);

                alert("Unable to fetch Issued Cheques.");

            });
    };

    $scope.SaveIssuedCheque = function (action) {
        debugger;

        if (!$scope.IssuedChequeList || $scope.IssuedChequeList.length == 0) {
            alert("Please add at least one cheque.");
            return;
        }

        var invalidRow = $scope.IssuedChequeList.some(function (item) {
            return !item.Date || !item.ChequeNo || !item.PartyName || !item.BankName || item.Amount == null || item.Amount <= 0;
        });

        if (invalidRow) {
            alert("Please fill all Issued Cheque details.");
            return;
        }

        var request = angular.copy($scope.IssuedChequeList);

        angular.forEach(request, function (item) {
            item.IsFinalSubmit = (action == '1');
        });

        $(".se-pre-con").fadeIn("slow");

        $http.post(SiteUrl + "/Auditor/SaveIssuedCheques", request
        ).then(function (response) {
            $(".se-pre-con").fadeOut("slow");
            if (response.data > 0) {
                alert("Issued Cheques Saved Successfully.");
                if (action == '1') {
                    $('a[href="#tab4"]').tab('show');
                }
            }
            else {
                alert("Unable to save Issued Cheques.");
            }

        }, function (error) {
            $(".se-pre-con").fadeOut("slow");
            console.log(error);
            alert("Something went wrong.");
        });
    };

    $scope.OnPageLoad();

});






