
 angular.module('plunkr', [])
.controller('UploadController', function ($scope, FileUploadService) {
    $scope.imageSrc = "";
    $scope.SelectedFileForUpload = null;
    $scope.$on("fileProgress", function (e, progress) {
        $scope.progress = progress.loaded / progress.total;
    });
    //File Select event 
    $scope.selectFileforUpload = function (file) {
        $scope.SelectedFileForUpload = file[0];
    }
    //Save File
    $scope.SaveFile = function () {
        $scope.IsFormSubmitted = true;
        FileUploadService.UploadFile($scope.SelectedFileForUpload).then(function (d) {
            alert(d.Message);
            ClearForm();
        }, function (e) {
            alert(e);
        });

    };
})
.factory('FileUploadService', function ($http, $q) { 

         var fac = {};
         fac.UploadFile = function (file) {
             var formData = new FormData();
             formData.append("file", file);
             //We can send more data to server using append         
             // formData.append("description", description);

             var defer = $q.defer();
             $http.post(SiteUrl+"/Smsformat/SaveFiles", formData,
                 {
                     withCredentials: true,
                     headers: { 'Content-Type': undefined },
                     transformRequest: angular.identity
                 })
                 .success(function (d) {
                     defer.resolve(d);
                 })
                 .error(function () {
                     defer.reject("File Upload Failed!");
                 });

             return defer.promise;

         }
         return fac;

     });
//app.directive("ngFileSelect", function (fileReader, $timeout) {
//    return {
//        scope: {
//            ngModel: '='
//        },
//        link: function ($scope, el) {
//            function getFile(file) {
//                fileReader.readAsDataUrl(file, $scope)
//                    .then(function (result) {
//                        $timeout(function () {
//                            $scope.ngModel = result;
//                        });
//                    });
//            }

//            el.bind("change", function (e) {
//                var file = (e.srcElement || e.target).files[0];
//                getFile(file);
//            });
//        }
//    };
//});

//app.factory("fileReader", function ($q, $log) {
//    var onLoad = function (reader, deferred, scope) {
//        return function () {
//            scope.$apply(function () {
//                deferred.resolve(reader.result);
//            });
//        };
//    };

//    var onError = function (reader, deferred, scope) {
//        return function () {
//            scope.$apply(function () {
//                deferred.reject(reader.result);
//            });
//        };
//    };

//    var onProgress = function (reader, scope) {
//        return function (event) {
//            scope.$broadcast("fileProgress", {
//                total: event.total,
//                loaded: event.loaded
//            });
//        };
//    };

//    var getReader = function (deferred, scope) {
//        var reader = new FileReader();
//        reader.onload = onLoad(reader, deferred, scope);
//        reader.onerror = onError(reader, deferred, scope);
//        reader.onprogress = onProgress(reader, scope);
//        return reader;
//    };

//    var readAsDataURL = function (file, scope) {
//        var deferred = $q.defer();

//        var reader = getReader(deferred, scope);
//        reader.readAsDataURL(file);

//        return deferred.promise;
//    };

//    return {
//        readAsDataUrl: readAsDataURL
//    };
//});



