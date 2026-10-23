var KPIValidateData = (function () {
    // Common Error Handler
    function handleError(message) {
        alert(message || "Server error occurred.");
    }

    // Validate data
    function GetKPIFormExpiryDetails($http, data, successCallback) {
        $http.get(data.SiteUrl + "KPI/GetKPIFormExpiryDetails", { params: { requireFor: data.RequireFor } }).then(function (response) {
            var result = response.data.Result;
            if (result && result.Status === "200") {
                successCallback(result.Data);
            } else {
                handleError(result ? result.Message : "Unable to get KPI expiry details.");
            }

        }).catch(function () {
            handleError("Server error occurred.");
        });
    }



    // Public methods
    return { GetKPIFormExpiryDetails: GetKPIFormExpiryDetails };

})();