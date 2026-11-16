(function(){
 'use strict';
 angular.module('departmentProgressApp',[]).controller('DepartmentProgressCtrl',['$scope','$http',function($scope,$http){
  $scope.districtId=1; $scope.reportDate=new Date(); $scope.rows=[]; $scope.busy=false; $scope.message='';
  function dateValue(){var d=$scope.reportDate;return d.getFullYear()+'-'+String(d.getMonth()+1).padStart(2,'0')+'-'+String(d.getDate()).padStart(2,'0');}
  var keys=['TotalCommittees','ElectionDepartment','ElectionParty','PendingApplications','NewDepartment','NewParty','OldDepartment','OldParty'];
  $scope.total=function(key){return $scope.rows.reduce(function(sum,r){return sum+(Number(r[key])||0);},0);};
  $scope.load=function(){ $scope.message=''; $http.get(window.progressUrls.get,{params:{districtId:$scope.districtId,reportDate:dateValue()}}).then(function(res){$scope.rows=res.data;},function(){$scope.message='Load failed';});};
  $scope.clear=function(){ $scope.rows.forEach(function(r){keys.forEach(function(k){r[k]=0;});}); $scope.message='';};
  $scope.save=function(){
   if($scope.busy || !$scope.rows.length) return;
   for(var i=0;i<$scope.rows.length;i++) for(var j=0;j<keys.length;j++) {var v=$scope.rows[i][keys[j]];if(v===null || v==='' || !Number.isInteger(Number(v)) || Number(v)<0){$scope.message='Only non-negative whole numbers allowed';return;}}
   $scope.busy=true;$scope.message='';
   var token=document.querySelector('input[name="__RequestVerificationToken"]').value;
   $http.post(window.progressUrls.save,{DistrictId:$scope.districtId,ReportDate:dateValue(),Rows:$scope.rows},{headers:{'RequestVerificationToken':token,'Content-Type':'application/json'}}).then(function(res){$scope.message=res.data.message;},function(){$scope.message='Save failed';}).finally(function(){$scope.busy=false;});
  };
  $scope.load();
 }]);
})();
