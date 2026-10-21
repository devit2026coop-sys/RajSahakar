
var MasterMod = angular.module('RajfedSocietyDetailMod', ['angularUtils.directives.dirPagination']);
MasterMod.controller("RajfedSocietyDetailCont", function ($scope, $http) {

    // SCOPE
    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;

    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
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

            $scope.ClearUnitForm = function () {
                $scope.UnitMaster = null;
            };
        }
        $scope.DashboardData();
        $scope.showLoader = false;
        $(".se-pre-con").fadeOut("slow");
    };



    //GET Load Table data
    $scope.DashboardData = function () {
        $http.post(SiteUrl + "/Society/RajfedSocietyDetailList").
            success(function (data, status) {
                $scope.TableData = [];
                $scope.TableData = data;
                if ($scope.TableData.length > 0) {
                    //setTimeout(function () {
                    //    $($($('#SocietyListDeshBoard tbody tr:first')[0]).find('td:eq(0)')[0]).find('input').prop('checked', true);
                    //}, 3000);
                    //$scope.TableDataForFilter = $scope.TableData;
                    //$scope.GetSocietiesForDeshBoard($scope.TableData[0].SocietyIdENC);
                    //$scope.GetSocietyDownloadList($scope.TableData[0].SocietyIdENC);
                    //$scope.PaidUpShareCapital = ($scope.TableData[0]).PaidUpShareCapital;
                    //$scope.NoOfBranches = ($scope.TableData[0]).NoOfBranches;
                    //$scope.Tot_ELECTEDMEMBERS = ($scope.TableData[0]).Tot_ELECTEDMEMBERS;
                    //$scope.Tot_GENERALMEMBERS = ($scope.TableData[0]).Tot_GENERALMEMBERS;
                    //$scope.Tot_MEMBERS = parseInt(($scope.TableData[0]).Tot_GENERALMEMBERS) + parseInt(($scope.TableData[0]).Tot_ELECTEDMEMBERS);
                }

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
                $scope.TableData = [];
            });
        $scope.showLoader = false;

    };

    $scope.OnPageLoad();
    //END

    $scope.ShowDetails = function (SocietyIdENC) {
        if (SocietyIdENC != '') {
            window.location = "../Society/SocietyApplicationView?SocietyIDEnc=" + SocietyIdENC + "~_Society_RajfedSocietyDetail~undefined";
        }
    };
});