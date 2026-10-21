var AppSmsFormat = angular.module("AppSmsFormat", ['angularUtils.directives.dirPagination']);
AppSmsFormat.controller("SmsFormat", function ($scope, $http) {
    // Save Sms Template 

    $scope.currentPage = 1;
    $scope.pageSize = 12;
    $scope.DivSms = true;
    $scope.newdiv = "";


    $scope.SaveSmsFormat = function () {
        $http.post(SiteUrl+"/Smsformat/SaveSmsFormat", $scope.Smsformat).
            sucess(function (data, status) {
                alert(data);
                var msg1 = "";

                if (data > 0) {
                    msg1 = (_language == 'en-us' ? 'Data Save Sucess\n' : 'डेटा सहेजा गया है\n')
                        || $scope.ClearTemplateForm();
                }
                else {
                    alert(_language == 'en-us' ? 'Some Error  is Occured \n' : 'कुछ त्रुटि हुई है\n') || $scope.ClearTemplateForm();
                    return;
                }
                var _Response = angular.fromJson(data);
                if (_Response.data > 0) {
                    $scope.Smsformat = null;
                }
            }).
            error(function (data, status) {
                alert("Error while Sms Template insert");
            });
    };

    $scope.pageChangeHandler = function (num) {
    };


    // Save Mail Template 
    $scope.SaveMailFormat = function () {
        $http.post(SiteUrl+"/Smsformat/SaveMailFormat", $scope.Smsformat).
            sucess(function (data, status) {
                $scope.GetAllMailData();
                var _Response = angular.fromJson(data);
                if (_Response.data > 0) {
                    $scope.Smsformat = null;
                }
            }).
            error(function (data, status) {
                alert("Error while Sms Template insert");
            });
    };

    // Update Template  
    $scope.update = function (tableData) {
        var Smsformat = [];
        Smsformat.TemplateHeadingEN = tableData.TemplateHeadingEN;
        Smsformat.TemplateHeadingHI = tableData.TemplateHeadingHI;
        Smsformat.TemplateBodyEN = tableData.TemplateBodyEN;
        Smsformat.TemplateBodyHI = tableData.TemplateBodyHI;
        Smsformat.IsActive = tableData.IsActive;
        $scope.Smsformat = tableData;
    };


    // START : Change Status for SMS / delete SMS,Status
    $scope.SetStatus = function (TemplateId, ActionId) {
        if (ActionId == 2)
            if (!confirm($scope.translation.DeleteConfirm))
                return false;

        $http.post(SiteUrl+"/Smsformat/ChangeStatusSMS", { Id: TemplateId, Action: ActionId }).
            success(function (data, status) {
                $scope.GetAllSmsData();
                $scope.GetAllMailData();
            }).
            error(function (data, status) {
            });
    };
    // END : Change Status for SMS / delete SMS,Status


    // START : Code for Language label setting
    $scope.getTranslation = function () {
        $scope.language = _language;
        if (_language == 'en-us') {
            $http.get(SiteUrl+"/Translations/Translation_EN.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl+"/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
    };
    // END : Code for Language label setting


    // Delete Template  
    $scope.deleterecord = function (id) {
        $http.get(SiteUrl+"/Smsformat/DeleteTemplate?id=" + id).then(function (d) {
            $scope.GetAllSmsData();
        }, function (error) {
            alert('Failed');
        });
    };

    // Get List of Sms Template
    $scope.GetAllSmsData = function () {
        if (_language == 'en-us') {
            $http.get(SiteUrl+"/Translations/Translation_EN.json").
                success(function (data, status) {

                    //$scope.newdiv = (_language == 'en-us' ? 'Data is not available' : 'डेटा उपलब्ध नहीं है ');
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }
        else {
            $http.get(SiteUrl+"/Translations/Translation_HI.json").
                success(function (data, status) {
                    $scope.translation = data;
                }).
                error(function (data, status) {
                    alert("Error while translation");
                });
        }



        $http.get(SiteUrl+"/Smsformat/SmsFormatList").
            success(function (data, status) {
                if (angular.toJson(data).length < 1) {
                    $scope.DivSms = false;
                    $scope.newdiv = (_language == 'en-us' ? 'Data is not available' : 'डेटा उपलब्ध नहीं है ');
                    return;
                }

                $scope.DivSms = true;
                $scope.SmsData = data;
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
                alert("Error while fetching Sms Template.");
                $scope.showLoader = false;
            });
    };

    // on page load
    $scope.OnPageLoad = function () {
        $(".se-pre-con").fadeIn("slow");
        $http.get(SiteUrl+"/Smsformat/GetSms").
            success(function (data2, status) {
                if (data2[0].ActionID != "") {
                    var str = data2[0].SmsFormatList;
                    var str_array = str.split(',');
                    angular.forEach(str_array, function (Datavalue) {
                        if (Datavalue == 'Add') {

                            $scope.IsAdd = true;
                        }
                        else if (Datavalue == 'Edit') {
                            //$scope.tdEdit.style.display = "none";
                            $scope.IsEdit = true;
                        }
                        else if (Datavalue == 'Take Action') {
                            //$scope.tdEdit.style.display = "none";
                            $scope.IsTakeAction = true;
                        }
                        else if (Datavalue == 'Status') {
                            //$scope.tdEdit.style.display = "none";
                            $scope.IsStatus = true;
                        }
                        else if (Datavalue == 'delete') {
                            //$scope.tdEdit.style.display = "none";
                            $scope.IsDelete = true;
                        }
                        else {
                            $scope.IsAdd = false;
                            $scope.IsEdit = false;
                            $scope.IsTakeAction = false;
                            $scope.IsStatus = false;
                            $scope.IsDelete = false;
                        }
                    });
                   
                }
            
            }).
            error(function (data1, status) {
                $(".se-pre-con").fadeOut("slow");
                $scope.WorkFlowExecution = [];
              
            });
       
        // on page load End

        // Get List of Sms Template
        $scope.GetAllMailData = function () {
            if (_language == 'en-us') {
                $http.get(SiteUrl+"/Translations/Translation_EN.json").
                    success(function (data, status) {

                        if (angular.toJson(data).length < 1) {
                            $scope.DivSms = false;
                            $scope.newdiv = (_language == 'en-us' ? 'Data is not available' : 'डेटा उपलब्ध नहीं है ');
                            return;
                        }
                        $scope.translation = data;
                    }).
                    error(function (data, status) {
                        alert("Error while translation");
                    });
            }
            else {
                $http.get(SiteUrl+"/Translations/Translation_HI.json").
                    success(function (data, status) {
                        $scope.translation = data;
                    }).
                    error(function (data, status) {
                        alert("Error while translation");
                    });
            }

            $http.get(SiteUrl+"/Smsformat/MailFormatList").
                success(function (data, status) {
                    $scope.MailData = data;
                }).
                error(function (data, status) {
                    alert("Error while fetching Sms Template.");
                    $scope.showLoader = false;
                });
        };



        //Clear Template form
        $scope.ClearTemplateForm = function () {
            $scope.Smsformat = null;
        };

        //Get list of sms template page list
        $scope.GetAllSmsData();

        $scope.GetAllMailData();
        $(".se-pre-con").fadeOut("slow");
    };
    // Language Selector at page load
    $scope.getTranslation($scope);
    $scope.OnPageLoad();


});

