var AppClasses = angular.module("AppSocietyTypeForm", ['angularUtils.directives.dirPagination']);
AppClasses.controller("SocietyTypeCtr", function ($scope, $http) {

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.DivSms = true;
    $scope.newdiv = "";
    $scope.SocietyTypeData = [];
    $scope.SocietyType = [];


    $scope.SaveSocietyType = function () {       
        $http.post(SiteUrl +"/Master/SaveSocietyType", { "TypeameEng": $scope.SocietyType.TypeameEng, "TypeNameHin": $scope.SocietyType.TypeNameHin, "IsActive": $scope.SocietyType.IsActive, "Amount": $scope.SocietyType.Amount, "SocietyTypeId": $scope.SocietyType.SocietyTypeId }).    
            success(function (data, status) {
               
                var k = data;
                var message = '';

                if (k == 1) {
                    message = _language == 'en-us' ? 'Data Save successfully\n' : 'डेटा सहेजा गया है\n';
                }
                else if (k == -2) {
                    message = _language == 'en-us' ? 'Duplicate Eentry\n' : 'डुप्लिकेट प्रविष्टि\n';
                }
                else {
                    message = _language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n';
                }
                alert(message);
                //ShowModelAlert(_language == 'en-us' ? 'Sport Association' : 'खेल एसोसिएशन', message);
                window.location = '/master/SocietyType';
                //if (data > 0) {
                //    alert(_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n')
                //        || $scope.ClearSocietyTypeForm();

                //}
                //else {
                //    alert(_language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n') || $scope.ClearSocietyTypeForm();
                //    return;
                //}
                //if ($scope.ParentId == '') {
                //    $scope.ParentId = 0;
                //}
               
                $scope.GetAllSocietyType();
                if (data.Status) {
                   // $scope.SocietyType = null;
                    $scope.ClearSocietyTypeForm();
                  //  $scope.GetAllBuisness();


                }
            }).
            error(function (data, status) {
                alert("Error while Menu insert");
            });


    };
  
    // START : Clear form
    $scope.ClearSocietyTypeForm = function () {
     $scope.SocietyType.SocietyTypeId = '0';
     $scope.SocietyType.TypeameEng = '';
     $scope.SocietyType.TypeNameHin = '';
     $scope.SocietyType.Amount = '0';
     $sco$scope.SocietyType.IsActive = false;
    };

    $scope.pageChangeHandler = function (num) {
    };

    // Update Template  
    $scope.update = function (item) {
        //alert(item.SocietyTypeId + "--" + item.TypeameEng + "--" + item.TypeNameHin + "--" + item.Amount + "--" + item.IsActive);
        $scope.SocietyType.SocietyTypeId = item.SocietyTypeId;
        $scope.SocietyType.TypeameEng = item.TypeameEng;
        $scope.SocietyType.TypeNameHin = item.TypeNameHin;
        $scope.SocietyType.Amount = item.Amount;
        $scope.SocietyType.IsActive = item.IsActive == 1 ? true : false;
        
    };

    $scope.SetStatus = function (TemplateId, ActionId) {
       
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl + "/Master/ChangeStatusSocietyType", { Id: TemplateId, Action: ActionId }).
            success(function (data, status) {

                $scope.GetAllSocietyType();

            }).
            error(function (data, status) {
            });
    };
    // Get List of CLASS Template
    $scope.GetAllSocietyType = function () {
        $http.get(SiteUrl +"/SocietyType/SocietyTypeList").
            success(function (data, status) {
                if (angular.toJson(data).length < 1) {
                    $scope.DivSms = false;
                    $scope.newdiv = (_language == 'en-us' ? 'Data is not available' : 'डेटा उपलब्ध नहीं है ');
                    return;
                }
                $scope.ClassesData = data;
                $scope.colsort = "";
                $scope.reverseSort = false;    
                $scope.showLoader = false;

            }).
            error(function (data, status) {
                alert("Error while fetching MenAction list.");
                $scope.showLoader = false;
            });
    };


    // on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        if (_language == 'en-us') {
            $http.get(SiteUrl +"/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl +"/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });

            $scope.ClearSocietyTypeForm = function () {
                $scope.SocietyType = null;
            };
        }
        //Get list of sms template page list
        $scope.GetAllSocietyType();
        $(".se-pre-con").fadeOut("slow");
    };
    // Language Selector at page load

    $scope.GetAllSocietyType = function () {
        $http.get(SiteUrl +"/Master/SocietyTypeList").
            success(function (data, status) {
                $scope.SocietyTypeData = data
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
    }

    $scope.OnPageLoad();


});

