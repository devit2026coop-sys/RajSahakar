var MasterMod = angular.module('PLDBLoanMastersMOD', ['angularUtils.directives.dirPagination']);
MasterMod.controller("PLDBLoanMastersCONT", function ($scope, $http) {

    $scope._language = 'en-us';
    $scope.currentPage = 1;
    $scope.pageSize = 12;

    $scope.FAQModel = {
        Id: 0,
        QuestionEN: "",
        QuestionHin: "",
        AnswerEN: "",
        AnswerHin: "",
        Priority: "",
        IsActive: false
    }

    $scope.ResetAddNew = function () {
        $scope.FAQModel = {
            Id: 0,
            QuestionEN: "",
            QuestionHin: "",
            AnswerEN: "",
            AnswerHin: "",
            Priority: "",
            IsActive: false
        }
    }

    $scope.ClearSearch = function () {
        $scope.q = "";
    }
    $scope.AddNewFAQ = function () {  
        $("#addModal").modal("show");
    }
    
    $scope.Back = function () {
        $(".se-pre-con").fadeIn("slow");
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
        $scope.GetFAQList("");
        $(".blockUI").fadeOut("slow");
    };

   
    /*FAQ List*/
    $scope.FAQList = [];
    $scope.GetFAQList = function (WhereCase) {
        return $http.post(SiteUrl + "/FAQ/GetFAQList", { "WhereCase": WhereCase }).
            success(function (data, status) {
                $scope.FAQList = [];
                $scope.FAQList = data;

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
  
    $scope.SaveFAQ = function () {
        if ($scope.ValidateFAQ()) {
            $(".blockUI").fadeIn("slow");
            var formData = {
                Id: $scope.FAQModel.Id,
                QuestionEN: $scope.FAQModel.QuestionEN,
                QuestionHin: $scope.FAQModel.QuestionHin,
                AnswerEN: $scope.FAQModel.AnswerEN,
                AnswerHin: $scope.FAQModel.AnswerHin,
                Priority: $scope.FAQModel.Priority,
                IsActive: $scope.FAQModel.IsActive 
            }

            $http.post(SiteUrl + "/FAQ/InsertUpdateFAQ", formData).
                success(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    if (data.Status == 2) {
                        $(".blockUI").fadeOut("slow");
                        ShowOkMessage("Warning", $scope._language == 'en-us' ? "FAQ Question Already Exist." : "FAQ प्रश्न पहले से मौजूद है", function () { });
                    }
                        else  if (data.Status == 3) {
                        $(".blockUI").fadeOut("slow");
                        ShowOkMessage("Warning", $scope._language == 'en-us' ? "FAQ Priority Already Exist." : "FAQ प्राथमिकता पहले से मौजूद है", function () { });
                    }
                    else if (data.Status == 4) {

                        ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                    }
                    else {
                        ShowOkMessage("Success", $scope._language == 'en-us' ? "Successfully submited." : "सफलतापूर्वक सबमिट किया गया", function () {
                            $("#addModal").modal("hide");
                            $scope.GetFAQList("");                           
                            $(".blockUI").fadeOut("slow");
                        });
                    }
                }).
                error(function (data, status) {
                    $(".blockUI").fadeOut("slow");
                    ShowOkMessage("Error", $scope._language == 'en-us' ? "There is some technically error occuer. Please try after some time" : "कुछ तकनीकी त्रुटि हुई है। कृपया कुछ देर बाद प्रयास करें", function () { });
                });
        }
    }

    $scope.ValidateFAQ = function () {

        var message = _language == 'en-us' ? 'Validate following : ' : 'निम्नलिखित मान्य करें :';
        
        if ($scope.FAQModel.QuestionEN == "") {
            message += _language == 'en-us' ? '\n <br />Please Enter the Question in English.' : '\n <br />कृपया अंग्रेजी में प्रश्न दर्ज करें।';
            $("#QuestionEN").addClass("notValid");
        }
        else {
            $("#QuestionEN").removeClass("notValid");
        }

        if ($scope.FAQModel.QuestionHin == "") {
            message += _language == 'en-us' ? '\n <br />Please Enter the Question in Hindi.' : '\n <br />कृपया हिंदी में प्रश्न दर्ज करें।';
            $("#QuestionHin").addClass("notValid");
        }
        else {
            $("#QuestionHin").removeClass("notValid");
        }

        if ($scope.FAQModel.AnswerEN == "") {
            message += _language == 'en-us' ? '\n <br />Please Enter the Answer in English.' : '\n <br />कृपया अंग्रेजी में उत्तर दर्ज करें।';
            $("#AnswerEN").addClass("notValid");
        }
        else {
            $("#AnswerEN").removeClass("notValid");
        }
        if ($scope.FAQModel.AnswerHin == "") {
            message += _language == 'en-us' ? '\n <br />Please Enter the Answer in Hindi.' : '\n <br />कृपया हिंदी में उत्तर दर्ज करें।';
            $("#AnswerHin").addClass("notValid");
        }
        else {
            $("#AnswerHin").removeClass("notValid");
        }

        if ($scope.FAQModel.Priority == "") {
            message += _language == 'en-us' ? '\n <br />Please Enter the FAQ Priority .' : '\n <br />कृपया FAQ प्राथमिकता दर्ज करें।।';
            $("#Priority").addClass("notValid");
        }
        else {
            $("#Priority").removeClass("notValid");
        }

        //if ($scope.FAQModel.IsActive == false) {
        //    message += _language == 'en-us' ? '\n <br />FAQ active? Please select.' : '\n <br /> FAQ सक्रिय है? कृपया चयन करें।';
        //    $("#IsActive").addClass("notValid");
        //}
        

        if (message.length > 29) {
            ShowModelAlert("FAQ", message);
            return false;
        }
        return true;
    }

    $scope.update = function (itm) {
        $(".se-pre-con").fadeIn("slow");
        $("#QuestionEN").removeClass("notValid");
        $("#QuestionHin").removeClass("notValid");
        $("#AnswerEN").removeClass("notValid");
        $("#AnswerHin").removeClass("notValid");
        $("#Priority").removeClass("notValid");


        $("#addModal").modal("show");
        $scope.FAQModel.Id = itm.Id;
        $scope.FAQModel.QuestionEN = itm.QuestionEN;
        $scope.FAQModel.QuestionHin = itm.QuestionHin;
        $scope.FAQModel.AnswerEN = itm.AnswerEN;
        $scope.FAQModel.AnswerHin = itm.AnswerHin;
        $scope.FAQModel.Priority = itm.Priority;
        $scope.FAQModel.IsActive = itm.IsActive == 1 ? true : false;
        //$scope.FAQModel.sector_id = itm.sector_id.toString();
        $(".blockUI").fadeOut("slow");
    };

    //Delete FAQ
    $scope.SetStatus = function (Id, ActionId) {

        if (ActionId == 2) {
            ShowAlertMessage("Warning", $scope._language == 'en-us' ? "Are you sure to delete this record ?." : 'क्या आप इस रिकॉर्ड को हटाना सुनिश्चित कर रहे हैं? कृपया रिकॉर्ड को हटाने के लिए "Yes" बटन पर क्लिक करें ।', function () {
                $(".se-pre-con").fadeIn("slow");
                $http.post(SiteUrl + "/FAQ/DeleteFAQ", { "Id": Id, "ActionId": ActionId }).
                    success(function (data, status) {
                       
                        $scope.GetFAQList("");
                        $(".se-pre-con").fadeOut("slow");

                    }).
                    error(function (data, status) {
                        $(".se-pre-con").fadeOut("slow");
                    });

            });
        }
        else {
            $http.post(SiteUrl + "/FAQ/DeleteFAQ", { "Id": Id, "ActionId": ActionId }).
                success(function (data, status) {
                    $(".se-pre-con").fadeIn("slow");
                    $scope.GetFAQList("");
                    $(".se-pre-con").fadeOut("slow");

                }).
                error(function (data, status) {
                    $(".se-pre-con").fadeOut("slow");
                });
        }
    };

    /*End FAQ*/
    $scope.OnPageLoad();


});






