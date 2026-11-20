using CoopDAL;
using Cooperative.DBConnector;
using Cooperative.Filters;
using Cooperative.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static Cooperative.Models.KVSSAuditAccountModel;

namespace Cooperative.Controllers
{
    [Error]
    [LogActionFilter]
    [CustomAuthentication]
    public class KVSSAuditAccountController : BaseController
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        // GET: Auditor
        // ============================================
        // GET TRADING / PROFIT & LOSS ACCOUNT
        // ============================================

        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings[
                "wmsprod"
            ].ConnectionString;

        [HttpGet]
        public ActionResult KVSSAuditAccount()
        {
            // IMPORTANT: This value MUST be populated by your authenticated society-selection flow.
            // Never trust a SocietyId supplied only by JavaScript.
            Session["KVSSAuthorizedSocietyId"] = "1";
            var authorizedSocietyId = Session["KVSSAuthorizedSocietyId"];
            if (authorizedSocietyId == null || Convert.ToInt32(authorizedSocietyId) <= 0)
                return new HttpStatusCodeResult(403, "Authorized society not selected.");

            ViewBag.SocietyId = Convert.ToInt32(authorizedSocietyId);
            ViewBag.FinancialYear = "2026-27"; // Replace with your reporting-FY selection.
            return View();
        }

        [HttpGet]
        public JsonResult GetAccount(
            int societyId,
            string financialYear,
            int accountId)
        {
            try
            {
                // 1. Validate request
                if (societyId <= 0 ||
                    accountId <= 0 ||
                    string.IsNullOrWhiteSpace(financialYear))
                {
                    Response.StatusCode = 400;

                    return Json(new
                    {
                        Success = false,
                        Message = "Invalid request parameters."
                    }, JsonRequestBehavior.AllowGet);
                }

                // Enforce the same authorized society scope on reads and writes.
                var authorized = Session["KVSSAuthorizedSocietyId"];
                int allowedSocietyId;
                if (authorized == null ||
                    !int.TryParse(Convert.ToString(authorized), out allowedSocietyId) ||
                    allowedSocietyId != societyId)
                {
                    Response.StatusCode = 403;
                    return Json(new { Success = false, Message = "Society access denied." },
                        JsonRequestBehavior.AllowGet);
                }

                var result = new KVSSAuditAccountResponseModel
                {
                    Header = new KVSSAuditAccountHeaderModel(),

                    Particulars =
                        new List<KVSSAuditAccountParticularModel>(),

                    Breakups =
                        new List<KVSSAuditAccountBreakupModel>()
                };

                using (SqlConnection con =
                    new SqlConnection(_connectionString))
                using (SqlCommand cmd =
                    new SqlCommand(
                        "dbo.USP_GetKVSSAuditAccount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(
                        "@SocietyId", SqlDbType.Int
                    ).Value = societyId;

                    cmd.Parameters.Add(
                        "@FinancialYear", SqlDbType.VarChar, 10
                    ).Value = financialYear.Trim();

                    cmd.Parameters.Add(
                        "@KVSSAuditAccountId", SqlDbType.Int
                    ).Value = accountId;

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        // =====================================
                        // RESULT SET 1: HEADER
                        // =====================================

                        if (dr.Read())
                        {
                            result.Header = new KVSSAuditAccountHeaderModel
                            {
                                EntryId = GetLong(dr, "EntryId"),

                                SocietyId = GetInt(dr, "SocietyId"),

                                FinancialYear =
                                    GetString(dr, "FinancialYear"),

                                KVSSAuditAccountId =
                                    GetInt(dr, "KVSSAuditAccountId"),

                                EntryStatus =
                                    GetString(dr, "EntryStatus")
                            };
                        }

                        // =====================================
                        // RESULT SET 2: PARTICULAR MASTER
                        // =====================================

                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                var item =
                                    new KVSSAuditAccountParticularModel
                                    {
                                        ParticularId =
                                            GetInt(dr, "ParticularId"),

                                        KVSSAuditAccountId =
                                            GetInt(dr, "KVSSAuditAccountId"),

                                        ParentParticularId =
                                            GetNullableInt(
                                                dr, "ParentParticularId"),

                                        ParticularNameHindi =
                                            GetString(
                                                dr, "ParticularNameHindi"),

                                        ParticularNameEnglish =
                                            GetString(
                                                dr, "ParticularNameEnglish"),

                                        EntrySide =
                                            GetString(dr, "EntrySide"),

                                        DisplayOrder =
                                            GetInt(dr, "DisplayOrder"),

                                        IsAmountApplicable =
                                            GetBool(
                                                dr, "IsAmountApplicable"),

                                        IsBreakupAllowed =
                                            GetBool(
                                                dr, "IsBreakupAllowed"),

                                        IsCalculated =
                                            GetBool(
                                                dr, "IsCalculated"),

                                        IsActive =
                                            GetBool(dr, "IsActive"),

                                        DetailId =
                                            GetLong(dr, "DetailId"),

                                        Amount =
                                            GetDecimal(dr, "Amount"),

                                        PreviousYearAmount =
                                            GetDecimal(
                                                dr, "PreviousYearAmount")
                                    };

                                result.Particulars.Add(item);
                            }
                        }

                        // =====================================
                        // RESULT SET 3: BREAKUP DETAILS
                        // =====================================

                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                var breakup =
                                    new KVSSAuditAccountBreakupModel
                                    {
                                        BreakupId =
                                            GetLong(dr, "BreakupId"),

                                        DetailId =
                                            GetLong(dr, "DetailId"),

                                        ParticularId =
                                            GetInt(dr, "ParticularId"),

                                        BreakupName =
                                            GetString(dr, "BreakupName"),

                                        Amount =
                                            GetDecimal(dr, "Amount")
                                    };

                                result.Breakups.Add(breakup);
                            }
                        }
                    }
                }

                // 2. Return AngularJS-compatible response
                return Json(
                    result,
                    JsonRequestBehavior.AllowGet
                );
            }
            catch (SqlException ex)
            {
                Trace.TraceError(
                    "KVSS GetAccount SQL Error: " + ex
                );

                Response.StatusCode = 500;

                return Json(new
                {
                    Success = false,
                    Message = "Database error while loading account."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Trace.TraceError(
                    "KVSS GetAccount Error: " + ex
                );

                Response.StatusCode = 500;

                return Json(new
                {
                    Success = false,
                    Message = "Unable to load account details."
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveAccount(KVSSAccountSaveRequest model)
        {
            try
            {
                var loggedIn = (UserEntity)Session[SessionIdentity.Admin];
                if (loggedIn == null)
                {
                    Response.StatusCode = 401;
                    return Json(new { Success = false, Message = "Session expired. Login again." });
                }

                // Required for AJAX POST: anti-forgery cookie + request header.
                System.Web.Helpers.AntiForgery.Validate(
                    Request.Cookies[System.Web.Helpers.AntiForgeryConfig.CookieName] == null
                        ? null
                        : Request.Cookies[System.Web.Helpers.AntiForgeryConfig.CookieName].Value,
                    Request.Headers["RequestVerificationToken"]);

                var selectedSociety = Session["KVSSAuthorizedSocietyId"];
                int societyId;
                if (selectedSociety == null ||
                    !int.TryParse(Convert.ToString(selectedSociety), out societyId) ||
                    societyId <= 0 || model == null || model.SocietyId != societyId)
                {
                    Response.StatusCode = 403;
                    return Json(new { Success = false, Message = "Society access denied." });
                }

                if (model.KVSSAuditAccountId != 1 && model.KVSSAuditAccountId != 2)
                {
                    Response.StatusCode = 400;
                    return Json(new { Success = false, Message = "Invalid account." });
                }
                if (String.IsNullOrWhiteSpace(model.FinancialYear) ||
                    !System.Text.RegularExpressions.Regex.IsMatch(model.FinancialYear, @"^\d{4}-\d{2}$") ||
                    model.Details == null || model.Details.Count == 0 ||
                    (model.SaveMode != "Draft" && model.SaveMode != "Next"))
                {
                    Response.StatusCode = 400;
                    return Json(new { Success = false, Message = "Invalid financial year, save mode or details." });
                }
                if (!ModelState.IsValid)
                {
                    Response.StatusCode = 400;
                    return Json(new { Success = false, Message = "Invalid request data." });
                }

                var details = model.Details;
                var breakups = model.Breakups ?? new List<KVSSAccountSaveBreakup>();
                if (details.Any(x => x.ParticularId <= 0 || x.Amount < 0) ||
                    details.Select(x => x.ParticularId).Distinct().Count() != details.Count ||
                    breakups.Any(x => x.ParticularId <= 0 || x.Amount < 0 ||
                                       String.IsNullOrWhiteSpace(x.BreakupName) || x.BreakupName.Length > 300))
                {
                    Response.StatusCode = 400;
                    return Json(new { Success = false, Message = "Invalid particulars or breakup rows." });
                }

                long entryId;
                string entryStatus;
                using (var con = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("dbo.USP_SaveKVSSAuditAccount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 60;
                    cmd.Parameters.Add("@SocietyId", SqlDbType.Int).Value = societyId;
                    cmd.Parameters.Add("@FinancialYear", SqlDbType.VarChar, 10).Value = model.FinancialYear.Trim();
                    cmd.Parameters.Add("@KVSSAuditAccountId", SqlDbType.Int).Value = model.KVSSAuditAccountId;
                    cmd.Parameters.Add("@SaveMode", SqlDbType.VarChar, 10).Value = model.SaveMode;
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = loggedIn.UserId;
                    cmd.Parameters.Add("@DetailsJson", SqlDbType.NVarChar, -1).Value = JsonConvert.SerializeObject(details);
                    cmd.Parameters.Add("@BreakupsJson", SqlDbType.NVarChar, -1).Value = JsonConvert.SerializeObject(breakups);
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) throw new Exception("Save procedure returned no result.");
                        entryId = Convert.ToInt64(reader["EntryId"]);
                        entryStatus = Convert.ToString(reader["EntryStatus"]);
                    }
                }
                return Json(new
                {
                    Success = true,
                    EntryId = entryId,
                    EntryStatus = entryStatus,
                    Message = model.SaveMode == "Next" ? "Saved. Moving to next account." : "Draft saved successfully."
                });
            }
            catch (System.Web.Mvc.HttpAntiForgeryException)
            {
                Response.StatusCode = 400;
                return Json(new { Success = false, Message = "Security token expired. Refresh page." });
            }
            catch (SqlException ex)
            {
                Trace.TraceError("KVSS SaveAccount SQL: " + ex);
                Response.StatusCode = 400;
                // Custom THROW messages from the stored procedure are safe validation messages.
                return Json(new
                {
                    Success = false,
                    Message = ex.Number >= 51000 && ex.Number < 51100
                    ? ex.Message : "Database error while saving account."
                });
            }
            catch (Exception ex)
            {
                Trace.TraceError("KVSS SaveAccount: " + ex);
                Response.StatusCode = 500;
                return Json(new { Success = false, Message = "Unable to save account." });
            }
        }


        // ============================================
        // COMMON SQL READER HELPERS
        // ============================================

        private static int GetInt(
            SqlDataReader dr, string column)
        {
            int index = dr.GetOrdinal(column);

            return dr.IsDBNull(index)
                ? 0
                : Convert.ToInt32(dr.GetValue(index));
        }

        private static int? GetNullableInt(
            SqlDataReader dr, string column)
        {
            int index = dr.GetOrdinal(column);

            return dr.IsDBNull(index)
                ? (int?)null
                : Convert.ToInt32(dr.GetValue(index));
        }

        private static long GetLong(
            SqlDataReader dr, string column)
        {
            int index = dr.GetOrdinal(column);

            return dr.IsDBNull(index)
                ? 0L
                : Convert.ToInt64(dr.GetValue(index));
        }

        private static decimal GetDecimal(
            SqlDataReader dr, string column)
        {
            int index = dr.GetOrdinal(column);

            return dr.IsDBNull(index)
                ? 0m
                : Convert.ToDecimal(dr.GetValue(index));
        }

        private static bool GetBool(
            SqlDataReader dr, string column)
        {
            int index = dr.GetOrdinal(column);

            return !dr.IsDBNull(index) &&
                   Convert.ToBoolean(dr.GetValue(index));
        }

        private static string GetString(
            SqlDataReader dr, string column)
        {
            int index = dr.GetOrdinal(column);

            return dr.IsDBNull(index)
                ? string.Empty
                : Convert.ToString(dr.GetValue(index));
        }
    }
}
