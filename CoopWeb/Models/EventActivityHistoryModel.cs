using Cooperative.DBConnector;
using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Web;
using Cooperative.Controllers;
using System.Linq;

namespace Cooperative.Models
{
    public class EventActivityHistoryModel
    {
        public Int32 EventActivityHistoryID { get; set; }
        public Int32 STATE_ID { get; set; }
        public Int32 DISTRICT_ID { get; set; }
        public Int32 CITY_ID { get; set; }
        public Int32 WARD_ID { get; set; }
        public Int32 BLOCK_ID { get; set; }
        public Int32 GP_ID { get; set; }
        public Int32 VILLAGE_ID { get; set; }
        public string Sector { get; set; }
        public Int32 IsRural { get; set; }
        public string EventActivityName { get; set; }
        public string BriefActivityInformation { get; set; }
        public string Objective { get; set; }
        public Int32 NoOfParticipants { get; set; }
        public string PhotoFilePath { get; set; }
        public string AudioVideoFilePath { get; set; }
        public string AchievementsOutcomes { get; set; }
        public DateTime EventHeldOn { get; set; }
        public Int64 CreatedBy { get; set; }

        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        public EventActivityHistoryModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
        }
        public EventActivityHistoryModel()
        {
            objILogger = new DBLogger();
            objDBAccess = new DBFactoryMSSQL();

        }
        public bool SaveEventActivityHistory(EventActivityHistoryModel eventActivity)
        {
            try
            {
                string month = DateTime.Now.Month.ToString();
                string photoPath = null, audioVideoFilePath = null;
                string destDir = HttpContext.Current.Server.MapPath("~/Content/EventActivity/" + month);

                if (!Directory.Exists(destDir))
                    Directory.CreateDirectory(destDir);
                if (eventActivity.PhotoFilePath != null && eventActivity.PhotoFilePath != "")
                {
                    if (File.Exists(eventActivity.PhotoFilePath))
                    {
                        photoPath = Path.GetFileName(eventActivity.PhotoFilePath);
                        File.Move(eventActivity.PhotoFilePath, Path.Combine(destDir, photoPath));
                    }
                }
                if (eventActivity.AudioVideoFilePath != null && eventActivity.AudioVideoFilePath != "")
                {
                    if (File.Exists(eventActivity.AudioVideoFilePath))
                    {
                        audioVideoFilePath = Path.GetFileName(eventActivity.AudioVideoFilePath);
                        File.Move(eventActivity.AudioVideoFilePath, Path.Combine(destDir, audioVideoFilePath));
                    }
                }
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_EventActivityHistory", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "_addEventActivityHistory");
                objDBAccess.AddParameterToCommand("@Sector", DBFactoryParameterType.VARCHAR, eventActivity.Sector);
                objDBAccess.AddParameterToCommand("@STATE_ID", DBFactoryParameterType.Int, eventActivity.STATE_ID);
                objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, eventActivity.DISTRICT_ID);
                objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.Int, eventActivity.IsRural);
                objDBAccess.AddParameterToCommand("@CITY_ID", DBFactoryParameterType.Int, eventActivity.CITY_ID);
                objDBAccess.AddParameterToCommand("@WARD_ID", DBFactoryParameterType.Int, eventActivity.WARD_ID);
                objDBAccess.AddParameterToCommand("@BLOCK_ID", DBFactoryParameterType.Int, eventActivity.BLOCK_ID);
                objDBAccess.AddParameterToCommand("@GP_ID", DBFactoryParameterType.Int, eventActivity.GP_ID);
                objDBAccess.AddParameterToCommand("@VILLAGE_ID", DBFactoryParameterType.Int, eventActivity.VILLAGE_ID);
                objDBAccess.AddParameterToCommand("@EventActivityName", DBFactoryParameterType.NVarchar, eventActivity.EventActivityName);
                objDBAccess.AddParameterToCommand("@BriefActivityInformation", DBFactoryParameterType.NVarchar, eventActivity.BriefActivityInformation);
                objDBAccess.AddParameterToCommand("@Objective", DBFactoryParameterType.NVarchar, eventActivity.Objective);
                objDBAccess.AddParameterToCommand("@NoOfParticipants", DBFactoryParameterType.Int, eventActivity.NoOfParticipants);
                objDBAccess.AddParameterToCommand("@Photo", DBFactoryParameterType.VARCHAR, month + "/" + photoPath);
                objDBAccess.AddParameterToCommand("@AudioVideo", DBFactoryParameterType.VARCHAR, month + "/" + audioVideoFilePath);
                objDBAccess.AddParameterToCommand("@AchievementsOutcomes", DBFactoryParameterType.NVarchar, eventActivity.AchievementsOutcomes);
                objDBAccess.AddParameterToCommand("@EventHeldOn", DBFactoryParameterType.VARCHAR, eventActivity.EventHeldOn);
                objDBAccess.AddParameterToCommand("@IpAddress", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, eventActivity.CreatedBy);
                return objDBAccess.ExecuteCommand() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EventActivityHistoryModel", FunctionName = "SaveEventActivityHistory(EventActivityHistoryModel eventActivity)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return false;
        }
        public DataSet GetEventActivityHistory(ref DataPagingModel Details)
        {
            try
            {
                string WhereClause = "WHERE 1 = 1 ";
                var SearchText = Details.SearchParameter.Where(x => x.Key == "SearchText").Select(x => x.Value).FirstOrDefault();
                string FromDate = "";
                string ToDate = "";
                var fromDate = Details.SearchParameter.Where(x => x.Key == "FromDate").Select(x => x.Value).FirstOrDefault();
                var toDate = Details.SearchParameter.Where(x => x.Key == "ToDate").Select(x => x.Value).FirstOrDefault();

                string month = Details.SearchParameter.Where(x => x.Key == "Month").Select(x => x.Value).FirstOrDefault();
                string whereDate = @" AND CONVERT(DATE, e.CreatedOn, 111) ";
                if (month == "0")
                {
                    if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        FromDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        ToDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        WhereClause += whereDate + " between '" + FromDate + "' and '" + ToDate + "'";
                    }
                    else if (!string.IsNullOrWhiteSpace(fromDate) && string.IsNullOrWhiteSpace(toDate))
                    {
                        FromDate = fromDate.Split('/')[2] + "-" + fromDate.Split('/')[1] + "-" + fromDate.Split('/')[0];
                        ToDate = FromDate;
                        WhereClause += whereDate + " between '" + FromDate + "' and '" + ToDate + "'";
                    }
                    else if (string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
                    {
                        ToDate = toDate.Split('/')[2] + "-" + toDate.Split('/')[1] + "-" + toDate.Split('/')[0];
                        FromDate = ToDate;
                        WhereClause += whereDate + " between '" + FromDate + "' and '" + ToDate + "'";
                    }
                }
                else
                    WhereClause += " AND DATEPART(MONTH, e.CreatedOn) = " + month + " ";

                if (SearchText != null)
                {
                    WhereClause += " AND (e.EventActivityName like '%" + SearchText + "%')";
                }

                string district = Details.SearchParameter.Where(x => x.Key == "DISTRICT_ID").Select(x => x.Value).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(district))
                {
                    WhereClause += " AND d.DISTRICT_ID IN(" + district + ")";
                }
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_EventActivityHistory", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "_getEventActivityHistory");
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.EventActivityHistoryModel", FunctionName = "GetEventActivityHistory(ref DataPagingModel Details)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return null;
        }

    }
}