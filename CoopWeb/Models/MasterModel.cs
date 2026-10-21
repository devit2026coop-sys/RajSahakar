using Cooperative.Controllers;
using Cooperative.DBConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Linq;
using CoopDAL;

namespace Cooperative.Models
{
    public class MasterModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public MasterModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public string GetMasterDataDetails(string MasterType = "", string MasterCode = "", string MasterCode2 = "")
        {
            //UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            string _MasterData = "{}";
            int Code = 0;

            if (string.IsNullOrEmpty(MasterType) || !int.TryParse(MasterCode, out Code))
            {
                // Response.Redirect("~/Views/Error/General.cshtml");
            }

            MasterModel objRajSewaDwaarModel = new MasterModel(new DBLogger());


            switch (MasterType.ToUpper())
            {
                case "TITLE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.TitleList()); break; }
                case "DEPARTMENT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.DepartmentList()); break; }
                case "DESIGNATION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.DesignationtList(Code)); break; }
                case "POST": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.PostList(Code)); break; }
                case "GENDER": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GenderList()); break; }
                case "RELIGION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.ReligionList()); break; }
                case "CAST": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.CastList()); break; }
                case "CATEGORY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.CategoryList()); break; }
                case "STATE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.StateList()); break; }
                case "DIVISION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.DivisionList()); break; }
                case "DISTRICT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.DistrictList(Code)); break; }
                case "TEHSIL": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.TehsilList(Code)); break; }
                case "PANCHAYATSAMITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.PanchayatSamitiBlockList(Code)); break; }
                case "GRAMPANCHAYAT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GramPanchayatList(Code)); break; }
                case "VILLAGE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.VillageList(Code)); break; }
                case "CITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.CityList(Code, MasterCode2)); break; }
                case "CITYLISTBY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.CityByCityTypeList(Code, MasterCode2)); break; }

                case "WARD": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.WardList(Code)); break; }
                case "FINANCIALYEAR": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.FinancialYearList()); break; }
                case "SOCIETYTYPE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.SocietyTypeList(Code)); break; }
                case "BUSINESS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.BusinessList(MasterCode)); break; }
                case "LIABILITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.LiablityList()); break; }
                case "SOCIETYCLASS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.SocietyClassList(Code)); break; }
                case "SOCIETYSUBCLASS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.SocietySubClassList(Code, MasterCode2)); break; }

                case "UNITS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.UnitListByDeptId(Code)); break; }
                case "LEVELS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.LevelList()); break; }
                case "OCCUPATION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.OccupationList()); break; }
                case "MEMBERTYPE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.MemberTypeList()); break; }
                case "ACT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.ACTList()); break; }
                case "CLASSES": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.ClassesList()); break; }
                case "SubClasses": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.ClassesList()); break; }
                case "Buissness": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.BusinessList(MasterCode)); break; }
                // case "Unit": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetAllSubUnitList('')); break; }
                case "Organisation": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.OrganisationsList()); break; }
                //case "PurchaseCenter": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.PurchaseCenterList()); break; }                 
                case "Office": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.OfficesList()); break; }
                case "RESIONMAPPING": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.ResionMappingList(MasterCode)); break; }
                case "CHANNEL": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.ChannelList()); break; }
                case "TARGET": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.TargetList()); break; }
                case "USERS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.UsersList()); break; }
                case "MAJORACTIVITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetMajorActivityList("0")); break; }
                case "ACTIVITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetActivityList(MasterCode)); break; }
                case "MONTH": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetMasterMonthList()); break; }
                case "RELATION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetMasterRelationList()); break; }
                case "ECONOMICGROUP": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetMasterEconomicList()); break; }
                case "PLDB": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetMasterPLDBList(MasterCode)); break; }
                case "PLDBBRANCH": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetMasterPLDBBranchList(MasterCode)); break; }
                case "EDUCATIONQUALIFICATION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetEducationQualificationList()); break; }
                case "FERTILIZER": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetFertilizerList()); break; }
                case "MENUCONFIGURATION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.MenuListForConfiguration()); break; }
                case "CONFIGURATIONACTION": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.ActionListForConfiguration()); break; }
                case "DEALERNATURE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.DealerNature()); break; }
                case "LIABILITYASSET": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.LiabilityAssetMasterList(Convert.ToInt32(MasterCode))); break; }
                case "CITYTYPE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.CityTypeList(Convert.ToInt32(MasterCode))); break; }
                //case "COMMODITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.RajfedCommodityMasterList(Convert.ToInt32(MasterCode))); break; }
                case "COMMODITY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.RajfedCommodityMasterList(Convert.ToInt32(MasterCode))); break; }

                case "PLANT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.PlantList(Convert.ToInt32(MasterCode))); break; }
                case "DEPORT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.DepotList(Convert.ToInt32(MasterCode))); break; }
                case "WAREHOUSE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.WarehouseList(Convert.ToInt32(MasterCode))); break; }

                case "CCSCOMPLAINTDOCTYPE": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.CCSComplaintDocumentTypeList()); break; }
                case "RAJFEDRO": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.RajfedROList()); break; }
                case "DISTRICTBYRO": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetAllDistrictListByRO(Code)); break; }
                case "DISTRICTBYRODISTRICT": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetDistrictListByRODistrict(Code)); break; }
                case "WAREHOUSEBYRO": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetWarehousetByRODistrict(Code)); break; }
                case "BANKLIST": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetBankMasterList()); break; }
                case "CCB": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetCCBData(Code)); break; }
                case "GRIEVANCECATEGORY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GrievanceCategoryList()); break; }//bhupendra
                //case "GSS_DAIRY": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetGSS_DAIRYList(Code, _LoggedInUser.SSOID)); break; }//bhupendra
                //case "INSPECTOR": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetInspector(_LoggedInUser.UserRoleDeptMappingId, _LoggedInUser.DistrictId)); break; }//bhupendra
                //case "FLOWPROCESSSTATUS": { _MasterData = COMMON_FUNCTION.DataTableToJSONString(objRajSewaDwaarModel.GetSocietyFlowProcessStatus(_LoggedInUser.UserRoleDeptMappingId, _LoggedInUser.DistrictId)); break; }
                default: break;

            }
            return _MasterData;
        }



        public DataTable SocietyTypeList(int ActId = 0)
        {
            dt.Clear();
            string WhereClause = string.Empty;
            if (ActId > 0)
            {
                WhereClause = " and Actid=" + ActId;
            }
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyTypeList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "SocietyTypeList(int ActId = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable BusinessList(string MasterCode = "0")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("BusinessList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActivityId", DBFactoryParameterType.Int, MasterCode);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "BusinessList(string MasterCode='" + MasterCode + "')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable LiablityList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LiabilitiesList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "LiablityList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable SocietyClassList(int SocietyTypeId = 0)
        {
            dt.Clear();
            try
            {
                string WhereClause = string.Empty;
                if (SocietyTypeId > 0)
                {
                    WhereClause = " and (SocityTypeId=-1 or SocityTypeId=" + SocietyTypeId + ")";
                }

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyClassList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "SocietyClassList(int SocietyTypeId = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable SocietySubClassList(int ClassId, string MasterCode2)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietySubClassList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ClassId", DBFactoryParameterType.Int, ClassId);
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, int.Parse(MasterCode2));

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "SocietySubClassList(int ClassId,string MasterCode2)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }



        public DataTable OccupationList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("OccupationList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DataTable OccupationList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable MemberTypeList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MemberTypeList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DataTable MemberTypeList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable ACTList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ActList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DataTable ACTList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable FunctionalStatusList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FunctionalStatusList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DataTable FunctionalStatusList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable TitleList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("TitleList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "TitleList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable DepartmentList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DepartmentList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DepartmentList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable DesignationtList(int MasterCode = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DesignationtList", DBFactoryCommandType.PROCEDURE_COMMAND);
                if (MasterCode == -2)
                {
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, " and IS_FOR_ADMINISTRATOR=1 and IS_ACTIVE in(0,1) ");
                }
                else if (MasterCode == -1)
                {
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, "  and IS_ACTIVE in(1) ");
                }
                else if (MasterCode == -3)
                {
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, "  AND IS_ACTIVE in (1,0) ");
                }
                else if (MasterCode > 0)
                {
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, "  and IS_ACTIVE in(1) AND DEPTCODE=" + MasterCode);
                }
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DesignationtList(int MasterCode = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable PostList(int MasterCode = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetPostList", DBFactoryCommandType.PROCEDURE_COMMAND);


                if (MasterCode == -2)
                {
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, " and (DESIGNATIONCODE=10 OR DESIGNATIONCODE=9) ");
                }
                else if (MasterCode == 10)
                {
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, " and (DESIGNATIONCODE=10) ");
                }
                else
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, " and 1=1");



                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "PostList(int MasterCode = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable StateList(int STATE_ID = 6)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("StateList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@STATE_ID", DBFactoryParameterType.Int, STATE_ID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "StateList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GenderList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GenderList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GenderList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable ReligionList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ReligionList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "ReligionList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable OrganisationsList()
        {
            dt.Clear();
            try
            {
                OrganisationMasterModel OrgModel = new OrganisationMasterModel();
                dt = OrgModel.GetAllOrganisations(" and o.isactive =1 ");
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "OrganisationsList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable OfficesList()
        {
            dt.Clear();
            try
            {
                OfficeMasterModel OrgModel = new OfficeMasterModel();
                dt = OrgModel.GetAllOffices(" and o.isactive =1 ");
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "OfficesList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable ChannelList()
        {
            dt.Clear();
            try
            {
                ChannelMasterModel ChannelModel = new ChannelMasterModel();
                dt = ChannelModel.GetAllChannels(" and isactive =1 ");
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "ChannelList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable RajfedROList()
        {
            dt.Clear();
            try
            {
                ChannelMasterModel ChannelModel = new ChannelMasterModel();
                dt = ChannelModel.GetAllRajfedROs();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "RajfedROList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetAllDistrictListByRO(int Id)
        {
            dt.Clear();
            try
            {
                ChannelMasterModel ChannelModel = new ChannelMasterModel();
                dt = ChannelModel.GetAllDistrictListByRO(Id);
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "RajfedROList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetDistrictListByRODistrict(int Id)
        {
            dt.Clear();
            try
            {
                ChannelMasterModel ChannelModel = new ChannelMasterModel();
                dt = ChannelModel.GetDistrictListByRODistrict(Id);
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetDistrictListByRODistrict()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetWarehousetByRODistrict(int Id)
        {
            dt.Clear();
            try
            {
                ChannelMasterModel ChannelModel = new ChannelMasterModel();
                dt = ChannelModel.GetWarehousetByRODistrict(Id);
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetWarehousetByRODistrict()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable TargetList()
        {
            dt.Clear();
            try
            {
                TargetMasterModel TargetModel = new TargetMasterModel();
                dt = TargetModel.GetAllTarget(" and isactive =1 ");
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "ChannelList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable UsersList()
        {
            dt.Clear();
            try
            {
                UsersMasterModel UserModel = new UsersMasterModel();
                dt = UserModel.GetAllUsers(" and isactive =1 ");
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "ChannelList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }




        public DataTable CastList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CastList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "CastList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable CategoryList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CategoryList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "CategoryList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable DivisionList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DivisionList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DivisionList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable DistrictList(int DivisionId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DistrictList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DistrictList(int DivisionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        public DataTable DISTRICT_WR(int DivisionId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DistrictList_WR", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DistrictList(int DivisionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        public DataTable MN_DistrictList(int DivisionId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MN_DistrictList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "MN_DistrictList(int DivisionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        public DataTable AuditDistrictList(int DivisionId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AuditDistrictList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, DivisionId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DistrictList(int DivisionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        public DataTable TehsilList(int DistrictId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("TehsilList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "TehsilList(int DistrictId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable PanchayatSamitiBlockList(int DistrictId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PanchayatSamitiList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "PanchayatSamitiBlockList(int DistrictId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable AssemblyList(int DistrictId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("AssemblyList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "AssemblyList(int DistrictId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        public DataTable MN_BlockList(int DistrictId)//MahilaNidhi
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MahilaNidhiBlockList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "PanchayatSamitiBlockList(int DistrictId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        public DataTable GramPanchayatList(int BLOCK_ID)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GramPanchayatList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, BLOCK_ID);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GramPanchayatList(int BLOCK_ID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable VillageList(int GramPanchayatId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("VillageList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.Int, GramPanchayatId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "VillageList(int GramPanchayatId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }



        public DataTable CityList(int DistrictId, String CityTypeId = "0")
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CityList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                // objDBAccess.AddParameterToCommand("@CITY_TYPE", DBFactoryParameterType.Int, CITY_TYPE);

                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "CityList(int District, String CityTypeId = '0')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }


        public DataTable CityByCityTypeList(int CITY_TYPE, string DistrictID = "")
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CityList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictID.Length > 0 ? (int.Parse(DistrictID)) : 0);
                objDBAccess.AddParameterToCommand("@CITY_TYPE", DBFactoryParameterType.Int, CITY_TYPE);

                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "CityByCityTypeList(int CITY_TYPE, string DistrictID = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        /// <summary>
        /// For get data of city list as per tehsilId wise // Created by Ritesh // 20180213 //
        /// </summary>
        /// <param name="TehsilId" type="int" ></param>
        /// <returns>City List</returns>
        public DataTable CityListByTehsilId(int TehsilId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CityList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TehsilId", DBFactoryParameterType.Int, TehsilId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "CityListByTehsilId(int Tehsil)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        /// <summary>
        /// For get data of Ward List as per CityId wise // Created by Ritesh // 20180213 //
        /// </summary>
        /// <param name="CityId" type="int" ></param>
        /// <returns>Ward List</returns>
        public DataTable WardList(int CityId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WardList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, CityId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "WardList(int CityId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        /// <summary>
        /// For get data of Widget List as per WidgetTypeId wise // Created by Ritesh // 20180213 //
        /// </summary>
        /// <param name="WidgetTypeId" type="int" ></param>
        /// <returns>Widget List</returns>
        public DataTable WidgetList(int WidgetTypeId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WidgetList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WidgetTypeId", DBFactoryParameterType.Int, WidgetTypeId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "WidgetList(int WidgetTypeId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable FinancialYearList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FinancialYearList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "FinancialYearList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable Classes()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertClasses", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "Classes()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable ClassesList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ClassesList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "ClassesList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable GetPaymentModeList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetPaymentModeList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetPaymentModeList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable GetOtsProcessStatus()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetOtsProcessStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetOtsProcessStatus()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable GetOtsPendingLevel(int IsLandMortgaged)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_GetOtsPendingLevel", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@IsLandMortgaged", DBFactoryParameterType.Int, IsLandMortgaged);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable GetOtsPendingLevel(int IsLandMortgaged)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public int ChangeDesignationStatus(int DesignationId, bool IsActive, int ActionId)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DesignationChangeStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DesignationId", DBFactoryParameterType.Int, DesignationId);
                objDBAccess.AddParameterToCommand("@Status", DBFactoryParameterType.Bit, IsActive);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);

                int.TryParse(objDBAccess.ExecuteScalarCommand().ToString(), out Result);
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "int ChangeDesignationStatus(int DesignationId, bool IsActive, int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return Result;
        }

        public DataTable IsTableExists(string TableName)
        {
            objDBAccess.OpenConnection();
            SqlCommand cmd = objDBAccess.CreateCommand("SELECT ISNULL(OBJECT_ID('" + TableName + "', N'U'),'-1')", DBFactoryCommandType.TEXT_COMMAND);
            return objDBAccess.GetTable();
        }

        public int CreateTable(string query)
        {
            objDBAccess.OpenConnection();
            SqlCommand cmd = objDBAccess.CreateCommand(query, DBFactoryCommandType.TEXT_COMMAND);
            return objDBAccess.ExecuteCommand();
        }

        public void BulkInsertToDb(DataTable dt, string _TableName)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["wmsprod"].ConnectionString);
            SqlBulkCopy objbulk = new SqlBulkCopy(con);

            if (_TableName == "BDH_CITY") //1 
            {
                objbulk.DestinationTableName = "City";
                objbulk.ColumnMappings.Add("CITY_ID", "CITY_ID");
                objbulk.ColumnMappings.Add("DISTRICT_ID", "DISTRICT_ID");
                objbulk.ColumnMappings.Add("CITY_CODE", "CITY_CODE");
                objbulk.ColumnMappings.Add("CITY_ENG", "CITY_ENG");
                objbulk.ColumnMappings.Add("CITY_MANGAL", "CITY_MANGAL");
                objbulk.ColumnMappings.Add("CITY_TYPE", "CITY_TYPE");
                objbulk.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                objbulk.ColumnMappings.Add("MODIFICATION_DATE", "MODIFICATION_DATE");
                objbulk.ColumnMappings.Add("VERSION", "VERSION");
                objbulk.ColumnMappings.Add("IS_ACTIVE", "IS_ACTIVE");
                objbulk.ColumnMappings.Add("REMARKS", "REMARKS");
            }
            else if (_TableName == "BDH_WARD") //2
            {
                objbulk.DestinationTableName = "Ward";
                objbulk.ColumnMappings.Add("WARD_ID", "WARD_ID");
                objbulk.ColumnMappings.Add("WARD_CODE", "WARD_CODE");
                objbulk.ColumnMappings.Add("WARD_ENG", "WARD_ENG");
                objbulk.ColumnMappings.Add("WARD_MANGAL", "WARD_MANGAL");
                objbulk.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                objbulk.ColumnMappings.Add("MODIFICATION_DATE", "MODIFICATION_DATE");
                objbulk.ColumnMappings.Add("VERSION", "VERSION");
                objbulk.ColumnMappings.Add("IS_ACTIVE", "IS_ACTIVE");
                objbulk.ColumnMappings.Add("IS_DELETED", "IS_DELETED");
                objbulk.ColumnMappings.Add("CITY_ID", "CITY_ID");
                objbulk.ColumnMappings.Add("REMARKS", "REMARKS");
            }
            else if (_TableName == "EMITRA_STATE") //3
            {
                objbulk.DestinationTableName = "State";
                objbulk.ColumnMappings.Add("STATE_ID", "STATE_ID");
                objbulk.ColumnMappings.Add("STATE_CODE", "STATE_CODE");
                objbulk.ColumnMappings.Add("STATE_ENG", "STATE_ENG");
                objbulk.ColumnMappings.Add("STATE_MANGAL", "STATE_MANGAL");
                objbulk.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                objbulk.ColumnMappings.Add("MODIFICATION_DATE", "MODIFICATION_DATE");
                objbulk.ColumnMappings.Add("IS_ACTIVE", "IS_ACTIVE");
                objbulk.ColumnMappings.Add("IS_DELETED", "IS_DELETED");
                objbulk.ColumnMappings.Add("CAPITAL", "CAPITAL");
                objbulk.ColumnMappings.Add("REMARKS", "REMARKS");
            }
            else if (_TableName == "PRAJ_BLOCK") //4
            {
                objbulk.DestinationTableName = "Block";
                objbulk.ColumnMappings.Add("BLOCK_ID", "BLOCK_ID");
                objbulk.ColumnMappings.Add("BLOCK_CODE", "BLOCK_CODE");
                objbulk.ColumnMappings.Add("BLOCK_ENG", "BLOCK_ENG");
                objbulk.ColumnMappings.Add("BLOCK_MANGAL", "BLOCK_MANGAL");
                objbulk.ColumnMappings.Add("BLOCK_STATUS", "BLOCK_STATUS");
                objbulk.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                objbulk.ColumnMappings.Add("MODIFICATION_DATE", "MODIFICATION_DATE");
                objbulk.ColumnMappings.Add("VERSION", "VERSION");
                objbulk.ColumnMappings.Add("IS_ACTIVE", "IS_ACTIVE");
                objbulk.ColumnMappings.Add("IS_DELETED", "IS_DELETED");
                objbulk.ColumnMappings.Add("DISTRICT_ID", "DISTRICT_ID");
                objbulk.ColumnMappings.Add("REMARKS", "REMARKS");
            }
            else if (_TableName == "PRAJ_DISTRICT") //5
            {
                objbulk.DestinationTableName = "District";
                objbulk.ColumnMappings.Add("DISTRICT_ID", "DISTRICT_ID");
                objbulk.ColumnMappings.Add("DISTRICT_CODE", "DISTRICT_CODE");
                objbulk.ColumnMappings.Add("DISTRICT_ENG", "DISTRICT_ENG");
                objbulk.ColumnMappings.Add("DISTRICT_MANGAL", "DISTRICT_MANGAL");
                objbulk.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                objbulk.ColumnMappings.Add("MODIFICATION_DATE", "MODIFICATION_DATE");
                objbulk.ColumnMappings.Add("IS_ACTIVE", "IS_ACTIVE");
                objbulk.ColumnMappings.Add("IS_DELETED", "IS_DELETED");
                objbulk.ColumnMappings.Add("DIVISION_ID", "DIVISION_ID");
                objbulk.ColumnMappings.Add("REMARKS", "REMARKS");
                objbulk.ColumnMappings.Add("IS_TSP", "IS_TSP");
            }
            else if (_TableName == "PRAJ_DIVISION") //6
            {
                objbulk.DestinationTableName = "Division";
                objbulk.ColumnMappings.Add("DIVISION_ID", "DIVISION_ID");
                objbulk.ColumnMappings.Add("DIVISION_CODE", "DIVISION_CODE");
                objbulk.ColumnMappings.Add("DIVISION_ENG", "DIVISION_ENG");
                objbulk.ColumnMappings.Add("DIVISION_MANGAL", "DIVISION_MANGAL");
                objbulk.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                objbulk.ColumnMappings.Add("MODIFICATION_DATE", "MODIFICATION_DATE");
                objbulk.ColumnMappings.Add("VERSION", "VERSION");
                objbulk.ColumnMappings.Add("IS_ACTIVE", "IS_ACTIVE");
                objbulk.ColumnMappings.Add("IS_DELETED", "IS_DELETED");
                objbulk.ColumnMappings.Add("REMARKS", "REMARKS");
            }
            else if (_TableName == "PRAJ_GP") //7
            {
                objbulk.DestinationTableName = "GramPanchayat";
                objbulk.ColumnMappings.Add("GP_ID", "GP_ID");
                objbulk.ColumnMappings.Add("GP_CODE", "GP_CODE");
                objbulk.ColumnMappings.Add("GP_ENG", "GP_ENG");
                objbulk.ColumnMappings.Add("GP_MANGAL", "GP_MANGAL");
                objbulk.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                objbulk.ColumnMappings.Add("MODIFICATION_DATE", "MODIFICATION_DATE");
                objbulk.ColumnMappings.Add("VERSION", "VERSION");
                objbulk.ColumnMappings.Add("IS_ACTIVE", "IS_ACTIVE");
                objbulk.ColumnMappings.Add("IS_DELETED", "IS_DELETED");
                objbulk.ColumnMappings.Add("BLOCK_ID", "BLOCK_ID");
                objbulk.ColumnMappings.Add("REMARKS", "REMARKS");
            }
            else if (_TableName == "PRAJ_VILLAGE") //8
            {
                objbulk.DestinationTableName = "Village";
                objbulk.ColumnMappings.Add("VILLAGE_ID", "VILLAGE_ID");
                objbulk.ColumnMappings.Add("VILLAGE_CODE", "VILLAGE_CODE");
                objbulk.ColumnMappings.Add("VILLAGE_ENG", "VILLAGE_ENG");
                objbulk.ColumnMappings.Add("VILLAGE_MANGAL", "VILLAGE_MANGAL");
                objbulk.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                objbulk.ColumnMappings.Add("MODIFICATION_DATE", "MODIFICATION_DATE");
                objbulk.ColumnMappings.Add("VERSION", "VERSION");
                objbulk.ColumnMappings.Add("IS_ACTIVE", "IS_ACTIVE");
                objbulk.ColumnMappings.Add("IS_DELETED", "IS_DELETED");
                objbulk.ColumnMappings.Add("GP_ID", "GP_ID");
                objbulk.ColumnMappings.Add("PATWAR_ID", "PATWAR_ID");
                objbulk.ColumnMappings.Add("AC_ID", "AC_ID");
                objbulk.ColumnMappings.Add("PS_ID", "PS_ID");
                objbulk.ColumnMappings.Add("REMARKS", "REMARKS");
            }
            else if (_TableName == "RVNU_TEHSIL") //9
            {
                objbulk.DestinationTableName = "Tehsil";
                objbulk.ColumnMappings.Add("TEHSIL_ID", "TEHSIL_ID");
                objbulk.ColumnMappings.Add("TEHSIL_CODE", "TEHSIL_CODE");
                objbulk.ColumnMappings.Add("TEHSIL_ENG", "TEHSIL_ENG");
                objbulk.ColumnMappings.Add("TEHSIL_MANGAL", "TEHSIL_MANGAL");
                objbulk.ColumnMappings.Add("CREATION_DATE", "CREATION_DATE");
                objbulk.ColumnMappings.Add("MODIFICATION_DATE", "MODIFICATION_DATE");
                objbulk.ColumnMappings.Add("VERSION", "VERSION");
                objbulk.ColumnMappings.Add("IS_ACTIVE", "IS_ACTIVE");
                objbulk.ColumnMappings.Add("IS_DELETED", "IS_DELETED");
                objbulk.ColumnMappings.Add("DISTRICT_ID", "DISTRICT_ID");
                objbulk.ColumnMappings.Add("REMARKS", "REMARKS");
            }
            try
            {
                con.Open();
                objbulk.WriteToServer(dt);
            }

            finally { con.Close(); }

        }

        public DataTable GetBreadCrumbByMenuURL(string MenuURL)
        {
            dt.Clear();
            try
            {

                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SelectPageLevelPath", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MenuURL", DBFactoryParameterType.NVarchar, MenuURL);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetBreadCrumbByMenuURL(string MenuURL)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public string GetBreadCrumb(string lang = "en-US")
        {
            string _data = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt = GetBreadCrumbByMenuURL(HttpContext.Current.Request.Url.AbsolutePath);

                if (dt != null && dt.Rows.Count > 0)
                {
                    _data = (lang.ToLower() == "en-us" ? Convert.ToString(dt.Rows[0]["PathEn"]) : Convert.ToString(dt.Rows[0]["PathHi"]));
                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetBreadCrumb(string lang = 'en - US')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return _data;
        }

        public DataTable UnitListByDeptId(int DeptId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("UnitList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DeptId", DBFactoryParameterType.Int, DeptId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "UnitListByDeptId(int DeptId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable LevelList()
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LevelList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "LevelList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }


        public DataTable GetMajorActivityList(string MasterCode)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MajorActivityList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.Int, int.Parse(MasterCode));
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetMajorActivityList(string MasterCode)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetActivityList(string MasterCode)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ActivityList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MajorActivityId", DBFactoryParameterType.Int, int.Parse(MasterCode));
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetActivityList(string MasterCode)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable ResionMappingList(string MasterCode)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ResionMappingList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, int.Parse(MasterCode));
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "ResionMappingList(string MasterCode)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetMasterMonthList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetMasterMonthList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetMasterMonthList(string MasterCode)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetProductTypeList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetProductTypeList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetProductTypeList(string MasterCode)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetCompanyList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCompanyList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetCompanyList(string MasterCode)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetMstProductList(int ProductTypeId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetMstProductList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ProductTypeId", DBFactoryParameterType.Int, ProductTypeId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable GetMstProductList(int ProductTypeId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GetMasterRelationList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetMasterRelationList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetMasterRelationList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetMasterEconomicList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetMasterEconomicList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetMasterEconomicList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetMasterPLDBList(string MasterCode)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetPLDBList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, int.Parse(MasterCode));
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetMasterPLDBList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetMasterPLDBBranchList(string PLDBId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PLDBBranchList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.Int, Convert.ToInt32(PLDBId));
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable GetMasterPLDBBranchList(int PLDBId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetEducationQualificationList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetEducationQualificationList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable GetEducationQualificationList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetFertilizerList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("FertilizerList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable GetFertilizerList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable MenuListForConfiguration()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MenuListForConfiguration", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable MenuListForConfiguration()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable ActionListForConfiguration()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ActionListForConfiguration", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable ActionListForConfiguration()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int SaveWorkFlowConfiguration(ApplicationNextAction model)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyProcessStatusAction", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ApplicationNextActionId", DBFactoryParameterType.Int, model.ApplicationNextActionId);
                objDBAccess.AddParameterToCommand("@SocietyProceesStatusIdChild", DBFactoryParameterType.NVarchar, model.SocietyProceesStatusIdChild);
                objDBAccess.AddParameterToCommand("@SocietyProceesStatusIdParent", DBFactoryParameterType.NVarchar, model.SocietyProceesStatusIdParent);
                objDBAccess.AddParameterToCommand("@WorkOnActId", DBFactoryParameterType.Int, model.WorkOnActId);
                objDBAccess.AddParameterToCommand("@MenuId", DBFactoryParameterType.NVarchar, model.MenuId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, model.RoleId);
                objDBAccess.AddParameterToCommand("@NextRoleId", DBFactoryParameterType.Int, model.NextRoleId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, model.ActionId);
                objDBAccess.AddParameterToCommand("@LevelID", DBFactoryParameterType.Int, model.LevelID);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand().ToString());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public int SaveWorkFlowConfiguration(ApplicationNextAction model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetWorkFlowConfiguration(string WhereClause)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetWorkFlowConfiguration", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable GetWorkFlowConfiguration(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int InsertSocietyProcessStatus(SocietyProcessStatus model)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyProcessStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyProceesStatusId", DBFactoryParameterType.Int, model.SocietyProceesStatusId);
                objDBAccess.AddParameterToCommand("@ProceesStatusTextEn", DBFactoryParameterType.NVarchar, model.ProceesStatusTextEn);
                objDBAccess.AddParameterToCommand("@ProceesStatusTextHi", DBFactoryParameterType.NVarchar, model.ProceesStatusTextHi);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, model.IsActive);
                objDBAccess.AddParameterToCommand("@ActId", DBFactoryParameterType.NVarchar, model.ActId);
                objDBAccess.AddParameterToCommand("@IsEnduserRelated", DBFactoryParameterType.Int, model.IsEnduserRelated);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, model.ActionId);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand().ToString());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public int InsertSocietyProcessStatus(SocietyProcessStatus model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetSocietyProcessStatus(string WhereClause)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyProcessStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable GetSocietyProcessStatus(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable DealerNature()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DealerNatureList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable DealerNature()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetMessageBodyByActivityKey(string messagekey)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetMessageBodyByActivityKey", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ActivityKey", DBFactoryParameterType.NVarchar, messagekey);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = " public DataTable GetMessageBodyByActivityKey(string messagekey)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable PACSBranchList(string DISTRICT_ID)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("PACSBranchList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DISTRICT_ID", DBFactoryParameterType.Int, DISTRICT_ID);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "PACSBranchList(string DISTRICT_ID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable ResionMappingByDistIdOrganizationLevelsMappingID(string DistrictId, string OrganizationLevelsMappingID, string MappingID = "0")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("ResionMappingListByDistIdOrganizationLevelsMappingID", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@OrganizationLevelsMappingID", DBFactoryParameterType.Int, OrganizationLevelsMappingID);
                objDBAccess.AddParameterToCommand("@MappingID", DBFactoryParameterType.Int, MappingID);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "PACSBranchList(string DISTRICT_ID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable LiabilityAssetMasterList(int LiabilityAssetMasterId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LiabilityAssetMasterList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LiabilityAssetMasterId", DBFactoryParameterType.Int, LiabilityAssetMasterId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable LiabilityAssetMasterList(int LiabilityAssetMasterId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int InsertUpdateLiabilityAsset(LiabilityAssetMasterEntity model)
        {
            int Result = 0;

            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("LiabilityAssetInsertUpdate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LiabilityAssetMasterId", DBFactoryParameterType.Int, model.LiabilityAssetMasterId);
                objDBAccess.AddParameterToCommand("@NameEng", DBFactoryParameterType.VARCHAR, model.NameEng);
                objDBAccess.AddParameterToCommand("@NameHi", DBFactoryParameterType.NVarchar, model.NameHi);
                objDBAccess.AddParameterToCommand("@IsLiability", DBFactoryParameterType.Bit, model.IsLiability);
                objDBAccess.AddParameterToCommand("@IsFurtherExtended", DBFactoryParameterType.Bit, model.IsFurtherExtended);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, model.CreateBy);
                objDBAccess.AddParameterToCommand("@IP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, model.ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public int InsertUpdateLiabilityAsset(LiabilityAssetMasterEntity model)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int LiabilityAssetMasterSetStatus(int LiabilityAssetMasterId, int ActionId)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("LiabilityAssetMasterSetStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LiabilityAssetMasterId", DBFactoryParameterType.Int, LiabilityAssetMasterId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public int LiabilityAssetMasterSetStatus(int LiabilityAssetMasterId, int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }


        public int RajfedGoDownInsertUpdate(GoDownMasterEntity model)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Rajfed_GodownInsertUpdate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GoDownId", DBFactoryParameterType.Int, model.GoDownId);
                objDBAccess.AddParameterToCommand("@WarehouseId", DBFactoryParameterType.Int, model.WarehouseId);
                objDBAccess.AddParameterToCommand("@GoDownNameEng", DBFactoryParameterType.VARCHAR, model.GoDownNameEng);
                objDBAccess.AddParameterToCommand("@GoDownNameHi", DBFactoryParameterType.NVarchar, model.GoDownNameHi);
                objDBAccess.AddParameterToCommand("@GoDownTypeId", DBFactoryParameterType.Int, model.GoDownTypeId);
                objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.Bit, model.IsRural);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.Int, model.DivisionId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, model.DistrictId);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, model.BlockId);
                objDBAccess.AddParameterToCommand("@GPId", DBFactoryParameterType.Int, model.GPId);
                objDBAccess.AddParameterToCommand("@VillageId", DBFactoryParameterType.Int, model.VillageId);
                objDBAccess.AddParameterToCommand("@CityTypeId", DBFactoryParameterType.Int, model.CityTypeId);
                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, model.CityId);
                objDBAccess.AddParameterToCommand("@WardId", DBFactoryParameterType.Int, model.WardId);
                objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, model.Address);
                objDBAccess.AddParameterToCommand("@CreateBy", DBFactoryParameterType.Int, model.CreateBy);
                objDBAccess.AddParameterToCommand("@CreateIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, model.ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public int RajfedGoDownInsertUpdate(GoDownMasterEntity model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int RajfedGoDownSetStatus(int GoDownId, int ActionId)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Rajfed_GodownSetStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GoDownId", DBFactoryParameterType.Int, GoDownId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public int RajfedGoDownSetStatus(int GoDownId, int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataTable RajfedGoDownList(int GoDownId, int WarehouseId = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Rajfed_GodownList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GoDownId", DBFactoryParameterType.Int, GoDownId);
                objDBAccess.AddParameterToCommand("@WarehouseId", DBFactoryParameterType.Int, WarehouseId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable RajfedGoDownList(int GoDownId, int WarehouseId = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable CityTypeList(int CITY_TYPE)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CityTypeListNew", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CITY_TYPE", DBFactoryParameterType.Int, CITY_TYPE);


                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "CityList(int District)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public int RajfedWarehouseInsertUpdate(RajfedWarehouseMasterEntity model)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Insert_Update_Rajfed_Warehous_Master_Entry", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WarehousId", DBFactoryParameterType.Int, model.WarehousId);
                objDBAccess.AddParameterToCommand("@WarehousNameEn", DBFactoryParameterType.VARCHAR, model.WarehousNameEn);
                objDBAccess.AddParameterToCommand("@WarehousNameHi", DBFactoryParameterType.NVarchar, model.WarehousNameHi);
                objDBAccess.AddParameterToCommand("@IsCity", DBFactoryParameterType.Bit, model.IsCity);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, model.DistrictId);
                objDBAccess.AddParameterToCommand("@BlockCityId", DBFactoryParameterType.Int, model.BlockCityId);
                objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, model.Address);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, model.CreateBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@IsCentralWarehouse", DBFactoryParameterType.Bit, model.IsCentralWarehouse);
                objDBAccess.AddParameterToCommand("@LicenceNo", DBFactoryParameterType.NVarchar, model.LicenceNo);
                objDBAccess.AddParameterToCommand("@LicenceValidUpTo", DBFactoryParameterType.NVarchar, model.LicenceValidUpTo);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public int RajfedWarehouseInsertUpdate(RajfedWarehouseMasterEntity model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int RajfedWarehouseSetStatus(int WarehousId, int ActionId)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Rajfed_WarehouseSetStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WarehousId", DBFactoryParameterType.Int, WarehousId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public int RajfedWarehouseSetStatus(int WarehousId, int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataTable RajfedWarehouseList(string WhereClause)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetWarehousList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable RajfedWarehouseList(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable RajfedCommodityMasterList(int CommodityId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("RajfedCommodityMasterList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CommodityId", DBFactoryParameterType.Int, CommodityId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable RajfedCommodityMasterList(int CommodityId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable PlantList(int DistId)
        {
            StockModel objMasterModel = new StockModel(new DBLogger());
            string WhereClause = string.Empty;
            WhereClause += DistId == 0 ? "  " : " AND DistrictId= " + DistId.ToString();
            return (objMasterModel.ProcessingUnitList(WhereClause));
        }
        public DataTable DepotList(int DistId)
        {
            StockModel objMasterModel = new StockModel(new DBLogger());
            string WhereClause = string.Empty;
            WhereClause += DistId == 0 ? "  " : " AND DistrictId= " + DistId.ToString();
            return (objMasterModel.DepotList(WhereClause));
        }
        public DataTable WarehouseList(int DistId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                string WhereClause = string.Empty;
                WhereClause += DistId == 0 ? "  " : " AND DistrictId= " + DistId.ToString();
                SqlCommand cmd = objDBAccess.CreateCommand("WherehouseListByCondition", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereCon", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable  WarehouseList(int DistId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public int CommodityInsertUpdate(CommodityEntity model)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CommodityInsertUpdate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CommodityId", DBFactoryParameterType.Int, model.CommodityId);
                objDBAccess.AddParameterToCommand("@CommodityNameEn", DBFactoryParameterType.VARCHAR, model.CommodityNameEn);
                objDBAccess.AddParameterToCommand("@CommodityNameHi", DBFactoryParameterType.NVarchar, model.CommodityNameHi);
                objDBAccess.AddParameterToCommand("@IsKharif", DBFactoryParameterType.Bit, model.IsKharif);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, model.CreateBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, model.ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public int CommodityInsertUpdate(CommodityEntity model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int CommoditySetStatus(int CommodityId, int ActionId)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CommoditySetStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@CommodityId", DBFactoryParameterType.Int, CommodityId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public int CommoditySetStatus(int CommodityId, int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int CommodityMSPInsertUpdate(CommodityMSPEntity model)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CommodityMSPInsertUpdate", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Msp_Id", DBFactoryParameterType.Int, model.Msp_Id);
                objDBAccess.AddParameterToCommand("@CommodityId", DBFactoryParameterType.Int, model.CommodityId);
                objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.Int, model.UnitId);
                objDBAccess.AddParameterToCommand("@Price", DBFactoryParameterType.Money, model.Price);
                objDBAccess.AddParameterToCommand("@FYEAR_ID", DBFactoryParameterType.Int, model.FYEAR_ID);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, model.CreateBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, model.ActionId);
                objDBAccess.AddParameterToCommand("@HighCharges", DBFactoryParameterType.Float, model.HighCharges);
                objDBAccess.AddParameterToCommand("@msp_nccf", DBFactoryParameterType.Float, model.msp_nccf);
                objDBAccess.AddParameterToCommand("@NafedChargesa", DBFactoryParameterType.Float, model.NafedChargesa);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public int CommodityMSPInsertUpdate(CommodityMSPEntity model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public int CommodityMspSetStatus(int CommodityMSPId, int ActionId)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CommodityMSPSetStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Msp_Id", DBFactoryParameterType.Int, CommodityMSPId);
                objDBAccess.AddParameterToCommand("@ActionId", DBFactoryParameterType.Int, ActionId);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public int CommodityMspSetStatus(int CommodityMSPId, int ActionId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return Result;
        }

        public DataTable CommodityMSPList(int Msp_Id)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CommodityMSPList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Msp_Id", DBFactoryParameterType.Int, Msp_Id);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable CommodityMSPList(int Msp_Id)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable CCSComplaintDocumentTypeList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CCSComplaintDocumentTypeList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable CCSComplaintDocumentTypeList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetBankMasterList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SPGetListofBankMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable GetBankMasterList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetCCBData(int Id, string SSO_ID = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetCCBData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);
                objDBAccess.AddParameterToCommand("@SSO_ID", DBFactoryParameterType.NVarchar, SSO_ID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable GetCCBData(int Id)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GrievanceCategoryList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceCategoryList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable GrievanceCategoryList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        public DataTable GetGSS_DAIRYList(int Id, string SSO_ID)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetGSS_DAIRYList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);
                objDBAccess.AddParameterToCommand("@SSO_ID", DBFactoryParameterType.NVarchar, SSO_ID);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable GetGSS_DAIRYList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetInspector(int MappingId, int DistrictId, int DivisionId = 0, int OfficeId = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetDrInspector", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, MappingId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);
                objDBAccess.AddParameterToCommand("@DivisionId", DBFactoryParameterType.NVarchar, DivisionId);
                objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.NVarchar, OfficeId);


                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable GetDrInspector()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetSocietyFlowProcessStatus(int MappingId, int DistrictId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyFlowProcessStatus", DBFactoryCommandType.PROCEDURE_COMMAND);
                //objDBAccess.AddParameterToCommand("@MappingId", DBFactoryParameterType.Int, MappingId);
                //objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.NVarchar, DistrictId);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable GetSocietyFlowProcessStatus()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable CMInstallmentType()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CMInstallmentType", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "CMInstallmentType()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetCMK_MasterData(string MasterType)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_CMK_GetMasterData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@MasterType", DBFactoryParameterType.NVarchar, MasterType);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetCMK_MasterData()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        #region Gopal Credit Card Pankaj

        public DataTable PanchayatSamitiBlockList_DistrictCode(string DistrictCode)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PanchayatSamitiList_District_Code", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@District_Code", DBFactoryParameterType.NVarchar, DistrictCode);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "PanchayatSamitiBlockList_DistrictCode(string DistrictCode" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable GramPanchayatList_BLOCKCODE(string BLOCKCODE)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GramPanchayatList_Block_Code", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Block_Code", DBFactoryParameterType.NVarchar, BLOCKCODE);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GramPanchayatList_BLOCKCODE(string BLOCKCODE)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable VillageList_GPCODE(string GPCODE)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("VillageList_GP_CODE", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@GP_CODE", DBFactoryParameterType.NVarchar, GPCODE);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "VillageList_GPCODE(string GPCODE)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable CityList_DISTRICTCODE(string DISTRICTCODE, String CityTypeId = "0")
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("CityList_District_Code", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@District_Code", DBFactoryParameterType.NVarchar, DISTRICTCODE);
                // objDBAccess.AddParameterToCommand("@CITY_TYPE", DBFactoryParameterType.Int, CITY_TYPE);

                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "CityList_DISTRICTCODE(string DISTRICTCODE, String CityTypeId = '0')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable WardList_CITYCODE(string CITYCODE)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WardList_City_Code", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@City_Code", DBFactoryParameterType.NVarchar, CITYCODE);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "WardList_CITYCODE(string CITYCODE)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        #endregion Gopal Credit Card Pankaj

        #region To Load CCB Master Data
        public DataTable GetCcbData(int DistrictId = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_GetCcbMasterData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GetCCB");
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetCcbData(int DistrictId = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetCcbBranchData(int CCBId = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_GetCcbMasterData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GetBranch");
                objDBAccess.AddParameterToCommand("@CcbId", DBFactoryParameterType.Int, CCBId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetCcbBranchData(int CCBId = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetPacsData(int BranchId = 0)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_GetCcbMasterData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GetPacs");
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, BranchId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetPacsData(int BranchId = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


        public DataTable KPIforPacs()
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("KPIForPacsList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "KPIforPacs(string CITYCODE)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        public DataTable MemberPostList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("MemberPostList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "MemberPostList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }


        public DataTable GetCcbIdsByMappingGccId()
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_GetCcbMasterData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GetCcbIdsByGccMappingId");
                objDBAccess.AddParameterToCommand("@CcbId", DBFactoryParameterType.Int, _LoggedInUser.GccId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, _LoggedInUser.LevelId);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetCcbIdsByMappingGccId()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }

        #endregion

        #region CM KISAN
        public DataTable Cm_SubDistrict(string districtId)
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_CMK_Dashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "CM_SUB_DISTRICT");
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "Cm_SubDistrict(string districtId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }
        public DataTable Cm_Village(string districtId, string subDistrictName = "")
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_CMK_Dashboard", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "CM_VILLAGE");
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, districtId);
                objDBAccess.AddParameterToCommand("@SubDistrictName", DBFactoryParameterType.VARCHAR, subDistrictName);
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "Cm_Village(string districtId, string subDistrictName = \"\")" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }

        #endregion CM KISAN

        public DataTable PurposeOfLoan()
        {
            dt.Clear();

            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_PurposeOfLoan", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GETLIST");
                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "PurposeOfLoan()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }


        public DataTable GetPLDBSectorODNew()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_PLDBSectorODNew", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action",DBFactoryParameterType.NVarchar,"GetPLDBSectorData");
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(
                    new ErrorLogEntity()
                    {
                        PageName = "Cooperative.Models.MasterModel",
                        FunctionName = "public DataTable GetPLDBSectorODNew()"
                    },
                    ex
                );
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return dt;
        }




        public int InsertPacsData(Pacs _Model)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_pacsData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "InsertData");
                objDBAccess.AddParameterToCommand("@PacsNameHi", DBFactoryParameterType.NVarchar, _Model.PacsNameHi);
                objDBAccess.AddParameterToCommand("@PacsNameEn", DBFactoryParameterType.NVarchar, _Model.PacsNameEn);
                objDBAccess.AddParameterToCommand("@PacsCode", DBFactoryParameterType.NVarchar, _Model.PacsCode);
                objDBAccess.AddParameterToCommand("@BankID", DBFactoryParameterType.Int, _Model.BankID);
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.Int, _Model.BranchId);

                objDBAccess.AddParameterToCommand("@IS_ACTIVE", DBFactoryParameterType.NVarchar, 1);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, _LoggedInUser.CreatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());

                //Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "InsertPacsData(Pacs _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        public DataTable GetPacsList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("sp_pacsData", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetData");


                //objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);
                //objDBAccess.AddParameterToCommand("@SSO_ID", DBFactoryParameterType.NVarchar, SSO_ID);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable GetCCBData(int Id)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }


    }



    public class WidgetModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public int WidgetId { get; set; }
        public int WidgetTypeId { get; set; }
        public string WidgetTitleNameEn { get; set; }
        public string WidgetTitleNameHi { get; set; }
        public string WidgetSubTitleNameEn { get; set; }
        public string WidgetSubTitleNameHi { get; set; }
        public string WidgetColor { get; set; }
        public string WidgetDataQuery { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedIP { get; set; }
        public Int64 UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedIP { get; set; }
        public int IsActive { get; set; }
        public string Version { get; set; }
        public string Remarks { get; set; }

        public WidgetModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        public WidgetModel()
        {
            objDBAccess = new DBFactoryMSSQL();
        }
        public int AddWidgetItem()
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertWidget", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WidgetId", DBFactoryParameterType.Int, WidgetId);
                objDBAccess.AddParameterToCommand("@WidgetTypeId", DBFactoryParameterType.BigInt, WidgetTypeId);
                objDBAccess.AddParameterToCommand("@WidgetTitleNameEn", DBFactoryParameterType.NVarchar, WidgetTitleNameEn);
                objDBAccess.AddParameterToCommand("@WidgetTitleNameHi", DBFactoryParameterType.NVarchar, WidgetTitleNameHi);
                objDBAccess.AddParameterToCommand("@WidgetSubTitleNameEn", DBFactoryParameterType.NVarchar, WidgetSubTitleNameEn);
                objDBAccess.AddParameterToCommand("@WidgetSubTitleNameHi", DBFactoryParameterType.NVarchar, WidgetSubTitleNameHi);
                objDBAccess.AddParameterToCommand("@WidgetColor", DBFactoryParameterType.NVarchar, WidgetColor);
                objDBAccess.AddParameterToCommand("@WidgetDataQuery", DBFactoryParameterType.NVarchar, WidgetDataQuery);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                if (WidgetId == 0)
                {
                    objDBAccess.AddParameterToCommand("@CreatedOn", DBFactoryParameterType.DateTime, System.DateTime.Now);
                }
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                if (WidgetId > 0)
                {
                    objDBAccess.AddParameterToCommand("@UpdatedOn", DBFactoryParameterType.DateTime, System.DateTime.Now);
                }
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, IsActive);
                objDBAccess.AddParameterToCommand("@Version", DBFactoryParameterType.NVarchar, Version);
                objDBAccess.AddParameterToCommand("@Remarks", DBFactoryParameterType.NVarchar, Remarks);

                //object o = 0;// 
                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "AddWidgetItem()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }







        public DataTable GetAllWidgetList()
        {
            try
            {
                dt.Clear();
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("WidgetList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllWidgetList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


    }

    public class BuissnessRegistrationModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        public int BusinessId { get; set; }
        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Business English - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading English - Max 100 characters allowed")]
        public string BusinessNameEn { get; set; }
        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Business English - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading Hindi - Max 100 characters allowed")]
        public string BusinessNameHi { get; set; }
        public int IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }


        public BuissnessRegistrationModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        public BuissnessRegistrationModel()
        {
            objDBAccess = new DBFactoryMSSQL();
        }


        public int InsertBuissness(BuissnessRegistrationModel _Model)
        {
            int Result = 0;
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertBuissnessRegistration", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@BusinessId", DBFactoryParameterType.Int, BusinessId);
                objDBAccess.AddParameterToCommand("@BusinessNameEn", DBFactoryParameterType.NVarchar, BusinessNameEn);
                objDBAccess.AddParameterToCommand("@BusinessNameHi", DBFactoryParameterType.NVarchar, BusinessNameHi);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.NVarchar, IsActive);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());


                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "InsertBuissness(BuissnessRegistrationModel _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;

        }
        public DataTable GetAllBuisness(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("BusinessList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllBuisness(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public int ChangeStatusBusiness(int BusinessId, int Action)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("BusinessDeleteStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TempleteId", DBFactoryParameterType.Int, BusinessId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "ChangeStatusBusiness(int BusinessId, int Action)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }




        //public IList<BuissnessRegistrationModel> GetAllBuisness(BuissnessRegistrationModel obj = null)
        //{
        //    IList<BuissnessRegistrationModel> GetAllBuisnesswork = new List<BuissnessRegistrationModel>();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("BusinessList", DBFactoryCommandType.PROCEDURE_COMMAND);

        //        DataTable dt = objDBAccess.GetTable();
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            int RowCount = dt.Rows.Count;

        //            foreach (DataRow Dr in dt.Rows)
        //            {
        //                GetAllBuisnesswork.Add(new BuissnessRegistrationModel() { BusinessId = int.Parse(Dr["BusinessId"].ToString()), BusinessNameEn = Dr["BusinessNameEn"].ToString(), BusinessNameHi = Dr["BusinessNameHi"].ToString(), });
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Master", FunctionName = "GetAllSmsFormat(SubClassesModel obj = null)" }, ex);
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //        dt.Dispose();
        //    }

        //    return GetAllBuisnesswork;
        //}

    }

    public class LiablitiesRegistrationModel

    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;



        public int LiablityId { get; set; }
        public string LiablityNameEn { get; set; }
        public string LiablityNameHi { get; set; }
        public int IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }

        public LiablitiesRegistrationModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        public LiablitiesRegistrationModel()
        {
            objDBAccess = new DBFactoryMSSQL();
        }



        public int InsertLiabilities(LiablitiesRegistrationModel _Model)
        {
            int Result = 0;
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];

                var DOBConvert = Convert.ToDateTime(System.DateTime.Now);
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertLiabilitiesRegistration", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@LiablityId", DBFactoryParameterType.BigInt, _Model.LiablityId);
                objDBAccess.AddParameterToCommand("@LiablityNameEn", DBFactoryParameterType.NVarchar, _Model.LiablityNameEn);
                objDBAccess.AddParameterToCommand("@LiablityNameHi", DBFactoryParameterType.NVarchar, _Model.LiablityNameHi);
                objDBAccess.AddParameterToCommand("@CreationDate", DBFactoryParameterType.DateTime, DOBConvert);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Bit, 1);
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.Int, _Model.UpdatedIP);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress());


                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "InsertLiabilities(LiablitiesRegistrationModel _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;

        }

    }

    public class ClassesModel
    {

        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        [Key]
        public int ClassId { get; set; }
        public string classEncID { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Classes English - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading English - Max 100 characters allowed")]
        public string ClassNameEn { get; set; }
        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Classes Hindi - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading Hindi - Max 100 characters allowed")]
        public string ClassNameHi { get; set; }

        public string TemplateBodyHI { get; set; }

        public string TemplateType { get; set; }
        public int IsActive { get; set; }
        public string WhereClause { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }


        public ClassesModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public ClassesModel()
        {
            objDBAccess = new DBFactoryMSSQL();

        }

        public int AddClasses()
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertClasses", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ClassId", DBFactoryParameterType.NVarchar, ClassId);
                objDBAccess.AddParameterToCommand("@ClassNameEn", DBFactoryParameterType.NVarchar, ClassNameEn);
                objDBAccess.AddParameterToCommand("@ClassNameHi", DBFactoryParameterType.NVarchar, ClassNameHi);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.NVarchar, IsActive);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());

                // Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "AddClasses()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable GetAllClasses(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ClassesList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllClasses(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public IList<ClassesModel> GetAllClasseswork(ClassesModel obj = null)
        {
            IList<ClassesModel> ClassesEntitywork = new List<ClassesModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ClassesList", DBFactoryCommandType.PROCEDURE_COMMAND);
                if (obj != null)
                {
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, obj.WhereClause);
                };
                DataTable dt = objDBAccess.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    int RowCount = dt.Rows.Count;

                    foreach (DataRow Dr in dt.Rows)
                    {
                        ClassesEntitywork.Add(new ClassesModel() { ClassId = int.Parse(Dr["ClassId"].ToString()), ClassNameEn = Dr["ClassNameEn"].ToString(), ClassNameHi = Dr["ClassNameHi"].ToString(), });
                    }

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "IList<ClassesModel> GetAllClasseswork(ClassesModel obj = null)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return ClassesEntitywork;
        }

        //public int ChangeStatusClasses(int ClassId, int Action)
        //{
        //    int Result = -1;
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("ClassDeleteStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@TempleteId", DBFactoryParameterType.Int, ClassId);
        //        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);
        //        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Master", FunctionName = "ChangeStatusClasses(int TemplateId, int Action)" }, ex);
        //        Result = -1;
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return Result;
        //}

        public int DeleteChangeStatusClasses(int ClassId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStautsClasses", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ClassId", DBFactoryParameterType.BigInt, ClassId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DeleteChangeStatusClasses(int ClassId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

    }

    public class SubClassesModel
    {

        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public int ClassId { get; set; }
        public int SubClassId { get; set; }
        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Classes English - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading English - Max 100 characters allowed")]
        public string SubClassNameEn { get; set; }
        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Classes Hindi - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading Hindi - Max 100 characters allowed")]
        public string SubClassNameHi { get; set; }






        public int IsActive { get; set; }
        public string WhereClause { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }


        public SubClassesModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public SubClassesModel()
        {
            objDBAccess = new DBFactoryMSSQL();

        }

        public int AddSubClasses(SubClassesModel _Model)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSubClasses", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SubClassId", DBFactoryParameterType.BigInt, SubClassId);
                objDBAccess.AddParameterToCommand("@ClassId", DBFactoryParameterType.BigInt, ClassId);
                objDBAccess.AddParameterToCommand("@SubClassNameEn", DBFactoryParameterType.NVarchar, SubClassNameEn);
                objDBAccess.AddParameterToCommand("@SubClassNameHi", DBFactoryParameterType.NVarchar, SubClassNameHi);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.NVarchar, IsActive);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());

                //Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "AddSubClasses(SubClassesModel _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }
        public DataTable GetAllSocietyCategory(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyCategoryList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllSocietyCategory(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }
        public DataTable GetAllSubClasses(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SubClassesList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllSubClasses(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public IList<SubClassesModel> GetAllSubClasseswork(SubClassesModel obj = null)
        {
            IList<SubClassesModel> SubClassesEntitywork = new List<SubClassesModel>();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("ClassesList", DBFactoryCommandType.PROCEDURE_COMMAND);
                if (obj != null)
                {
                    objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, obj.WhereClause);
                };
                DataTable dt = objDBAccess.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    int RowCount = dt.Rows.Count;

                    foreach (DataRow Dr in dt.Rows)
                    {
                        SubClassesEntitywork.Add(new SubClassesModel() { ClassId = int.Parse(Dr["SubClassId"].ToString()), SubClassNameEn = Dr["SubClassNameEn"].ToString(), SubClassNameHi = Dr["SubClassNameHi"].ToString(), });
                    }

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllSubClasseswork(SubClassesModel obj = null)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return SubClassesEntitywork;
        }

        public int ChangeStatusSubClass(int SubClassId, int Action)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SubClassDeleteStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TempleteId", DBFactoryParameterType.Int, SubClassId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "ChangeStatusSubClass(int SubClassId, int Action)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }




    }

    public class SocietyTypeModal
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;



        public int SocietyTypeId { get; set; }
        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Type Name English - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading English - Max 100 characters allowed")]
        public string TypeameEng { get; set; }
        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Type Name Hindi - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading Hindi - Max 100 characters allowed")]
        public string TypeNameHin { get; set; }
        public decimal Amount { get; set; }

        public int IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdationDate { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }


        public SocietyTypeModal(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        public SocietyTypeModal()
        {
            objDBAccess = new DBFactoryMSSQL();
        }


        public int InsertSocietyType(SocietyTypeModal _Model)
        {
            int Result = 0;
            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertSocietyType", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@SocietyTypeId", DBFactoryParameterType.Int, _Model.SocietyTypeId);
                objDBAccess.AddParameterToCommand("@TypeameEng", DBFactoryParameterType.NVarchar, TypeameEng);
                objDBAccess.AddParameterToCommand("@TypeNameHin", DBFactoryParameterType.NVarchar, TypeNameHin);
                objDBAccess.AddParameterToCommand("@Amount", DBFactoryParameterType.NVarchar, Amount);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.NVarchar, IsActive);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());






                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "InsertSocietyType(SocietyTypeModal _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;

        }
        public DataTable GetAllSocietyType(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocietyTypeList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, WhereClause);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllSocietyType(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public int ChangeStatusSocietyType(int SocietyTypeId, int Action)
        {
            int Result = -1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SocityTypeDeleteStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@TempleteId", DBFactoryParameterType.Int, SocietyTypeId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "ChangeStatusSocietyType(int SocietyTypeId, int Action)" }, ex);
                Result = -1;
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


    }

    public class UnitMasterModel
    {

        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public int UnitId { get; set; }
        public int DepartmentId { get; set; }
        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Unit English - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading English - Max 100 characters allowed")]
        public string UnitNameEn { get; set; }
        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Unit Hindi - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading Hindi - Max 100 characters allowed")]
        public string UnitNameHi { get; set; }




        public int IsActive { get; set; }
        public string WhereClause { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }


        public UnitMasterModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public UnitMasterModel()
        {
            objDBAccess = new DBFactoryMSSQL();

        }

        public int AddUnit(UnitMasterModel _Model)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertUnits", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.BigInt, UnitId);
                objDBAccess.AddParameterToCommand("@DepartmentId", DBFactoryParameterType.BigInt, DepartmentId);
                objDBAccess.AddParameterToCommand("@UnitNameEn", DBFactoryParameterType.NVarchar, UnitNameEn);
                objDBAccess.AddParameterToCommand("@UnitNameHi", DBFactoryParameterType.NVarchar, UnitNameHi);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.NVarchar, IsActive);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());

                //Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "AddUnit(UnitMasterModel _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAllUnits(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetUnitIDList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllUnits(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public int Deleteunitfun(int UnitId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStatusUnit", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@UnitId", DBFactoryParameterType.BigInt, UnitId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "Deleteunitfun(int UnitId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        //public int ChangeStatusunit(int UnitId, int Action)
        //{
        //    int Result = -1;
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("UnitDeleteStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@TempleteId", DBFactoryParameterType.Int, UnitId);
        //        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);
        //        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Master", FunctionName = "ChangeStatusunit(int UnitId, int Action)" }, ex);
        //        Result = -1;
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return Result;
        //}

        //public IList<SubClassesModel> GetAllSubClasseswork(SubClassesModel obj = null)
        //{
        //    IList<SubClassesModel> SubClassesEntitywork = new List<SubClassesModel>();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("ClassesList", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        if (obj != null)
        //        {
        //            objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, obj.WhereClause);
        //        };
        //        DataTable dt = objDBAccess.GetTable();
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            int RowCount = dt.Rows.Count;
    }
    public class DesignationMasterModel
    {

        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public int DESIGNATIONCODE { get; set; }
        public int DEPTCODE { get; set; }
        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Unit English - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading English - Max 100 characters allowed")]
        public string DESIGNATIONDESCENGLISH { get; set; }
        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Unit Hindi - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading Hindi - Max 100 characters allowed")]
        public string DESIGNATIONDESCHINDI { get; set; }




        public int IS_ACTIVE { get; set; }
        public string WhereClause { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }


        public DesignationMasterModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public DesignationMasterModel()
        {
            objDBAccess = new DBFactoryMSSQL();

        }

        public int AddDesignation(DesignationMasterModel _Model)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertDesignation", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DESIGNATIONCODE", DBFactoryParameterType.BigInt, DESIGNATIONCODE);
                objDBAccess.AddParameterToCommand("@DEPTCODE", DBFactoryParameterType.BigInt, DEPTCODE);
                objDBAccess.AddParameterToCommand("@DESIGNATIONDESCENGLISH", DBFactoryParameterType.NVarchar, DESIGNATIONDESCENGLISH);
                objDBAccess.AddParameterToCommand("@DESIGNATIONDESCHINDI", DBFactoryParameterType.NVarchar, DESIGNATIONDESCHINDI);
                objDBAccess.AddParameterToCommand("@IS_ACTIVE", DBFactoryParameterType.NVarchar, IS_ACTIVE);
                //objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, CreatedBy);
                //objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, UpdatedBy);
                //objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, CreatedIP);
                //objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, UpdatedIP);

                //Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "AddDesignation(DesignationMasterModel _Model)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAllUnits(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetUnitIDList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllUnits(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public int DeleteDesignationfun(int DESIGNATIONCODE, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStatusDesignation", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@DESIGNATIONCODE", DBFactoryParameterType.BigInt, DESIGNATIONCODE);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DeleteDesignationfun(int DESIGNATIONCODE, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        //public int ChangeStatusunit(int UnitId, int Action)
        //{
        //    int Result = -1;
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("UnitDeleteStatusChange", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        objDBAccess.AddParameterToCommand("@TempleteId", DBFactoryParameterType.Int, UnitId);
        //        objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.Int, Action);
        //        Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
        //    }
        //    catch (Exception ex)
        //    {
        //        objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.Master", FunctionName = "ChangeStatusunit(int UnitId, int Action)" }, ex);
        //        Result = -1;
        //    }
        //    finally
        //    {
        //        objDBAccess.CloseConnection();
        //    }
        //    return Result;
        //}

        //public IList<SubClassesModel> GetAllSubClasseswork(SubClassesModel obj = null)
        //{
        //    IList<SubClassesModel> SubClassesEntitywork = new List<SubClassesModel>();
        //    try
        //    {
        //        objDBAccess.OpenConnection();
        //        SqlCommand cmd = objDBAccess.CreateCommand("ClassesList", DBFactoryCommandType.PROCEDURE_COMMAND);
        //        if (obj != null)
        //        {
        //            objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, obj.WhereClause);
        //        };
        //        DataTable dt = objDBAccess.GetTable();
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            int RowCount = dt.Rows.Count;
    }
    public class OrganisationMasterModel
    {

        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public int OrganisationId { get; set; }
        public int DepartmentId { get; set; }
        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Organisation English - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading English - Max 100 characters allowed")]
        public string OrganisationNameHi { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Organisation Hindi - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading Hindi - Max 100 characters allowed")]
        public string OrganisationNameEn { get; set; }




        public int IsActive { get; set; }
        public string WhereClause { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }


        public OrganisationMasterModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }


        public OrganisationMasterModel()
        {

            objDBAccess = new DBFactoryMSSQL();

        }




        public int AddOrganisation(string OrganisationNameEn, string OrganisationNameHi, string IsActive, int OrganisationId, int DepartmentId)
        {

            int Result = 0;
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertOrganisations", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OrganisationId", DBFactoryParameterType.BigInt, OrganisationId);
                objDBAccess.AddParameterToCommand("@DepartmentId", DBFactoryParameterType.Int, DepartmentId);
                objDBAccess.AddParameterToCommand("@OrganisationNameEn", DBFactoryParameterType.NVarchar, OrganisationNameEn);
                objDBAccess.AddParameterToCommand("@OrganisationNameHi", DBFactoryParameterType.NVarchar, OrganisationNameHi);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, IsActive == "True" ? 1 : 0);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());

                //Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "AddOrganisation(string OrganisationNameEn, string OrganisationNameHi, string IsActive, int OrganisationId, int DepartmentId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAllOrganisations(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetOrganisationIDList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllOrganisations(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public int DeleteOrganisationfun(int OrganisationId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStatusOrganisation", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OrganisationId", DBFactoryParameterType.BigInt, OrganisationId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DeleteOrganisationfun(int OrganisationId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

    }
    public class DepartmentMasterModel
    {

        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public int ID { get; set; }
        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Department English - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading English - Max 100 characters allowed")]
        public string DEPTNAMEHINDI { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Department Hindi - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading Hindi - Max 100 characters allowed")]
        public string DEPTNAMEENGLISH { get; set; }




        public int IsActive { get; set; }
        public string WhereClause { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }


        public DepartmentMasterModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }


        public DepartmentMasterModel()
        {

            objDBAccess = new DBFactoryMSSQL();

        }




        public int AddDepartment(string DEPTNAMEENGLISH, string DEPTNAMEHINDI, string IsActive, int ID)
        {

            int Result = 0;
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertDepartments", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ID", DBFactoryParameterType.BigInt, ID);
                objDBAccess.AddParameterToCommand("@DEPTNAMEENGLISH", DBFactoryParameterType.NVarchar, DEPTNAMEENGLISH);
                objDBAccess.AddParameterToCommand("@DEPTNAMEHINDI", DBFactoryParameterType.NVarchar, DEPTNAMEHINDI);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, IsActive == "True" ? 1 : 0);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());

                //Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "AddDepartment(string DEPTNAMEENGLISH, string DEPTNAMEHINDI, string IsActive, int ID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAllDepartments(string WhereClause = "")
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetDepartmentList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllDepartments(string WhereClause = '')" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public int ChangeStatusDepartment(int ID, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStatusDepartment", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@ID", DBFactoryParameterType.BigInt, ID);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "ChangeStatusDepartment(int ID, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

    }
    public class OfficeMasterModel
    {

        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public int OfficeId { get; set; }
        public int LevelId { get; set; }
        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Office English - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading English - Max 100 characters allowed")]
        public string OfficeNameHi { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Office Hindi - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading Hindi - Max 100 characters allowed")]
        public string OfficeNameEn { get; set; }




        public int IsActive { get; set; }
        public string WhereClause { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdationDate { get; set; }
        public long CreatedBy { get; set; }
        public Int64 UpdatedBy { get; set; }
        public string CreatedIP { get; set; }
        public string UpdatedIP { get; set; }


        public OfficeMasterModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }


        public OfficeMasterModel()
        {

            objDBAccess = new DBFactoryMSSQL();

        }




        public int AddOffice(string OfficeNameEn, string OfficeNameHi, string IsActive, int OfficeId, int OrganizationLevelsMappingID)
        {

            int Result = 0;
            try
            {

                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertOffices", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.BigInt, OfficeId);
                objDBAccess.AddParameterToCommand("@OrganizationLevelsMappingID", DBFactoryParameterType.Int, OrganizationLevelsMappingID);
                objDBAccess.AddParameterToCommand("@OfficeNameEn", DBFactoryParameterType.NVarchar, OfficeNameEn);
                objDBAccess.AddParameterToCommand("@OfficeNameHi", DBFactoryParameterType.NVarchar, OfficeNameHi);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, IsActive == "True" ? 1 : 0);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());

                //Result = objDBAccess.ExecuteCommand();
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "AddOffice(string OfficeNameEn, string OfficeNameHi, string IsActive, int OfficeId, int OrganizationLevelsMappingID)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetAllOffices(string WhereClause)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetOfficeList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllOffices(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public int ChangeStatusOffice(int OfficeId, int Action)
        {
            int Result = 0;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("DeleteStatusOffice", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@OfficeId", DBFactoryParameterType.BigInt, OfficeId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.BigInt, Action);

                Result = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "ChangeStatusOffice(int OfficeId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

    }

    public class ChannelMasterModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public int ChannelId { get; set; }
        public string ChannelName_En { get; set; }
        public string ChannelName_Hi { get; set; }

        public ChannelMasterModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        public ChannelMasterModel()
        {

            objDBAccess = new DBFactoryMSSQL();

        }
        public DataTable GetAllChannels(string WhereClause)
        {
            //dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetChannelList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllChannels(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable GetAllRajfedROs()
        {
            //dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("RajfedGetROList", DBFactoryCommandType.PROCEDURE_COMMAND);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllRajfedROs()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetAllDistrictListByRO(int Id)
        {
            //dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("RajfedGetDistrictListByRO", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllRajfedROs()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
        public DataTable GetDistrictListByRODistrict(int Id)
        {
            //dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("RajfedGetDistrictListByRODistrict", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllRajfedROs()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GetWarehousetByRODistrict(int Id)
        {
            //dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("RajfedGetWarehousetByRODistrict", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.Int, Id);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetWarehousetByRODistrict()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


    }
    public class TargetMasterModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public int ChannelId { get; set; }
        public string ChannelName_En { get; set; }
        public string ChannelName_Hi { get; set; }

        public TargetMasterModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        public TargetMasterModel()
        {

            objDBAccess = new DBFactoryMSSQL();

        }
        public DataTable GetAllTarget(string WhereClause)
        {
            //dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetTargetList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllTarget(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }
    }

    public class UsersMasterModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public int UsersId { get; set; }
        public string ChannelName_En { get; set; }
        public string ChannelName_Hi { get; set; }

        public UsersMasterModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }
        public UsersMasterModel()
        {

            objDBAccess = new DBFactoryMSSQL();

        }
        public DataTable GetAllUsers(string WhereClause)
        {
            //dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("GetUser", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@WhereClause", DBFactoryParameterType.NVarchar, !string.IsNullOrEmpty(WhereClause) ? WhereClause : string.Empty);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllUsers(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }


        public DataTable GetAllErrorList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();

                SqlCommand cmd = objDBAccess.CreateCommand("ErrorLogList", DBFactoryCommandType.PROCEDURE_COMMAND);

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllErrorList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        public DataTable GrievanceCategoryList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GrievanceCategoryList", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "public DataTable GrievanceCategoryList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return dt;
        }





    }

    #region  Purchase_Center_Master
    public class PurchaseCenterMasterModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public int PurchaseCenterId { get; set; }
        public int DistrictId { get; set; }
        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Purchase Center English - Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading English - Max 100 characters allowed")]
        public string PurchaseCenterName_En { get; set; }

        [Column(TypeName = "nvarchar"), Required(AllowEmptyStrings = false, ErrorMessage = "Purchase Center Hindi - Required ")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(100, ErrorMessage = "Template Heading Hindi - Max 100 characters allowed")]
        public string PurchaseCenterName_Hi { get; set; }

        public int CommodityId { get; set; }
        public int KVSSId { get; set; }
        public int IsActive { get; set; }


        public PurchaseCenterMasterModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public PurchaseCenterMasterModel()
        {
            objDBAccess = new DBFactoryMSSQL();
        }

        public int AddPurchaseCenterMaster(int PurchaseCenterId, int DistrictId, string PurchaseCenterName_En, string PurchaseCenterName_Hi, int CommodityId, int IsActive)
        {
            int Result = 0;

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertRajfed_PurchaseCenter", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PurchaseCenterId", DBFactoryParameterType.BigInt, PurchaseCenterId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, DistrictId);
                objDBAccess.AddParameterToCommand("@PurchaseCenterName_En", DBFactoryParameterType.NVarchar, PurchaseCenterName_En);
                objDBAccess.AddParameterToCommand("@PurchaseCenterName_Hi", DBFactoryParameterType.NVarchar, PurchaseCenterName_Hi);
                objDBAccess.AddParameterToCommand("@CommodityId", DBFactoryParameterType.Int, CommodityId);
                //objDBAccess.AddParameterToCommand("@KvssId", DBFactoryParameterType.Int, KVSSId);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, IsActive);

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "AddPurchaseCenterMaster(int PurchaseCenterId, int DistrictId, string PurchaseCenterName_En, string PurchaseCenterName_Hi,int CommodityId, int IsActive)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }


            return Result;
        }

        public DataTable GetAllPurchaseCenterList()
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetRajfed_PurchaseCenterList", DBFactoryCommandType.PROCEDURE_COMMAND);

                dt = objDBAccess.GetTable();

            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetAllPurchaseCenterList()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public int DeletePurchageCenter(int PurchaseCenterId, int Action)
        {
            int Return = 1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Delete_RajFedPurchaseCenter", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PurchaseCenterId", DBFactoryParameterType.BigInt, PurchaseCenterId);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, Action);

                Return = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DeletePurchageCenter(int PurchaseCenterId, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Return;
        }

    }
    #endregion

    #region Bank Details

    public class BankDetailsMasterModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public int Id { get; set; }

        public string SocietyName { get; set; }
        public string PLDBBranchName { get; set; }
        [DisplayName("Society")]
        public int SocietyID { get; set; }

        [DisplayName("Application No")]
        public int ApplicationNo { get; set; }

        [DisplayName("Reg ID")]
        public string RegID { get; set; }

        [DisplayName("Bank Name")]
        public string BankName { get; set; }

        [DisplayName("Bank")]
        public int BankID { get; set; }

        [DisplayName("Bank Branch")]
        public string BankBranch { get; set; }

        [DisplayName("IFSC Code")]
        public string IFSCCode { get; set; }

        public string CreatedIP { get; set; }

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Bank Account No")]
        public string BankAccountNo { get; set; }

        [DisplayName("Acc. Holder Name")]
        public string AccHolderName { get; set; }

        public string BankMaster_Name { get; set; }

        [DisplayName("IsActive")]
        public bool IsActive { get; set; }


        public int BranchId { get; set; }
        public int SocietyType { get; set; }

        public BankDetailsMasterModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public BankDetailsMasterModel()
        {
            objDBAccess = new DBFactoryMSSQL();
        }

        public int AddBankDetails(BankDetailsViewModel objModel)
        {
            int Result = 0;

            try
            {
                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertUpdate_BankDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.BigInt, objModel.BankDetails.Id);
                objDBAccess.AddParameterToCommand("@SocietyID", DBFactoryParameterType.Int, objModel.BankDetails.SocietyID);
                objDBAccess.AddParameterToCommand("@ApplicationNo", DBFactoryParameterType.Int, objModel.BankDetails.ApplicationNo);
                objDBAccess.AddParameterToCommand("@RegID", DBFactoryParameterType.VARCHAR, objModel.BankDetails.RegID);
                objDBAccess.AddParameterToCommand("@BankName", DBFactoryParameterType.VARCHAR, objModel.BankDetails.BankName);
                objDBAccess.AddParameterToCommand("@BankID", DBFactoryParameterType.Int, objModel.BankDetails.BankID);
                objDBAccess.AddParameterToCommand("@BankBranch", DBFactoryParameterType.VARCHAR, objModel.BankDetails.BankBranch);
                objDBAccess.AddParameterToCommand("@IFSCCode", DBFactoryParameterType.VARCHAR, objModel.BankDetails.IFSCCode);
                objDBAccess.AddParameterToCommand("@BankAccountNo", DBFactoryParameterType.VARCHAR, objModel.BankDetails.BankAccountNo);
                objDBAccess.AddParameterToCommand("@AccHolderName", DBFactoryParameterType.VARCHAR, objModel.BankDetails.AccHolderName);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, objModel.BankDetails.IsActive);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@UpdatedBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@UpdatedIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());
                objDBAccess.AddParameterToCommand("@BranchId", DBFactoryParameterType.NVarchar, objModel.BankDetails.BranchId);
                objDBAccess.AddParameterToCommand("@SocietyType", DBFactoryParameterType.NVarchar, objModel.BankDetails.SocietyType);
                objDBAccess.AddParameterToCommand("@CreatedBySSOId", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "AddBankDetails(BankDetailsViewModel objModel)" }, ex);
            }

            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }

        public DataTable GetMst_BankDetails(ref DataPagingModel Details, string WhereClause, int SocietyType)
        {
            //List<BankDetailsMasterModel> BankDetailsList = new List<BankDetailsMasterModel>();

            DataTable dt1 = new DataTable();
            try
            {

                var SearchText = Details.SearchParameter.Where(x => x.Key == "searchText").Select(x => x.Value).FirstOrDefault();
                if (SearchText != null)
                {
                    WhereClause = " and ApplicationNo like '%" + SearchText + "%'" + "or bd.BankName like '%" + SearchText + "%'" + "or BankBranch like'%" + SearchText + "%'" + "or IFSCCode like'%"
                    + SearchText + "%'" + "or BankAccountNo like'%" + SearchText + "%'" + "or AccHolderName like'%" + SearchText + "%'" + "or  bm.BankName like'%" + SearchText + "%'";
                }

                if (WhereClause != "")
                {
                    Details.PageNumber = 1;
                }


                UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                SqlCommand cmd = new SqlCommand();
                objDBAccess.OpenConnection();
                if (Details.PageNumber == 1)
                {
                    cmd = objDBAccess.CreateCommand("GetMst_BankDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                    objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                    objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                    objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, true);
                    objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                    objDBAccess.AddParameterToCommand("@SocietyType", DBFactoryParameterType.NVarchar, SocietyType);
                    objDBAccess.AddParameterToCommand("@MeppingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                    objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                    Details.TotalRecords = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());

                }
                if (Details.IsAllRecords)
                {
                    Details.PageSize = Details.TotalRecords;
                }

                cmd = objDBAccess.CreateCommand("GetMst_BankDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);
                objDBAccess.AddParameterToCommand("@IsRowCount", DBFactoryParameterType.Int, false);
                objDBAccess.AddParameterToCommand("@WhereCase", DBFactoryParameterType.NVarchar, WhereClause);
                objDBAccess.AddParameterToCommand("@SocietyType", DBFactoryParameterType.NVarchar, SocietyType);
                objDBAccess.AddParameterToCommand("@MeppingId", DBFactoryParameterType.NVarchar, _LoggedInUser.UserRoleDeptMappingId);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);

                dt = objDBAccess.GetTable();



                //foreach (DataRow dr in dt.Rows)
                //{
                //    BankDetailsList.Add(
                //        new BankDetailsMasterModel
                //        {
                //            Id = Convert.ToInt32(dr["Id"]),
                //            SocietyID = Convert.ToInt32(dr["SocietyID"]),
                //            ApplicationNo = Convert.ToInt32(dr["ApplicationNo"]),
                //            RegID = (string)(dr["RegID"]),
                //            BankName = (string)(dr["BankName"]),
                //            BankID = Convert.ToInt32(dr["BankID"]),
                //            BankBranch = (string)(dr["BankBranch"]),
                //            IFSCCode = (string)(dr["IFSCCode"]),
                //            CreatedIP = (string)(dr["CreatedIP"]),
                //            CreatedDate = (DateTime)(dr["CreatedDate"]),
                //            BankAccountNo = (string)(dr["BankAccountNo"]),
                //            AccHolderName = (string)(dr["AccHolderName"]),
                //            BankMaster_Name = (string)(dr["BankMaster_Name"]),
                //            IsActive = Convert.ToBoolean(dr["IsActive"]),
                //            SocietyName = (string)(dr["SocietyName"]),
                //            PLDBBranchName = (string)(dr["BranchName"])
                //        }
                // );

                //}
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetMst_BankDetails(string WhereClause)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return dt;
        }

        public int DeleteBankDetails(int Id, int Action, string Type)
        {
            int Return = 1;
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("Delete_BankDetails", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Id", DBFactoryParameterType.BigInt, Id);
                objDBAccess.AddParameterToCommand("@IsActive", DBFactoryParameterType.Int, Action);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, Type);

                Return = objDBAccess.ExecuteCommand();
            }
            catch (Exception ex)
            {
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "DeleteBankDetails(int Id, int Action)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Return;
        }

        public List<BankDetailsMasterModel> GetSocietyListByPLDBID(int PldbID)
        {
            List<BankDetailsMasterModel> BankDetailsList = new List<BankDetailsMasterModel>();

            // dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("GetSocietyListByPLDBID", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PldbId", DBFactoryParameterType.BigInt, PldbID);
                dt = objDBAccess.GetTable();


                foreach (DataRow dr in dt.Rows)
                {
                    BankDetailsList.Add(
                        new BankDetailsMasterModel
                        {
                            SocietyID = Convert.ToInt32(dr["SocietyID"]),
                            SocietyName = (string)(dr["SocietyName"])
                        }
                 );

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetListofBankMaster()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return BankDetailsList;
        }
        public List<BankDetailsMasterModel> GetBranchListByPLDBID(int PldbID)
        {
            List<BankDetailsMasterModel> BankDetailsList = new List<BankDetailsMasterModel>();

            // dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("PLDBBranchList", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@PLDBId", DBFactoryParameterType.BigInt, PldbID);
                dt = objDBAccess.GetTable();


                foreach (DataRow dr in dt.Rows)
                {
                    BankDetailsList.Add(
                        new BankDetailsMasterModel
                        {
                            SocietyID = Convert.ToInt32(dr["BLOCK_ID"]),
                            SocietyName = (string)(dr["BLOCK_ENG"])
                        }
                 );

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetListofBankMaster()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return BankDetailsList;
        }



    }

    public class BankDetailsViewModel
    {
        public List<BankDetailsMasterModel> BankDetailsList { get; set; }

        public BankDetailsMasterModel BankDetails { get; set; }

    }

    #endregion


    #region Bank Master

    public class BankMasterModel
    {
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;

        public int BankId { get; set; }
        [DisplayName("Bank Name")]
        public string BankName { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public int ModifiedBy { get; set; }

        public bool IsActive { get; set; }


        public BankMasterModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public BankMasterModel()
        {
            objDBAccess = new DBFactoryMSSQL();
        }

        public List<BankMasterModel> GetListofBankMaster()
        {
            List<BankMasterModel> BankDetailsList = new List<BankMasterModel>();

            // dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("SPGetListofBankMaster", DBFactoryCommandType.PROCEDURE_COMMAND);
                dt = objDBAccess.GetTable();


                foreach (DataRow dr in dt.Rows)
                {
                    BankDetailsList.Add(
                        new BankMasterModel
                        {
                            BankId = Convert.ToInt32(dr["BankId"]),
                            BankName = (string)(dr["BankName"]),
                            IsActive = (bool)(dr["IsActive"])
                        }
                 );

                }
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MasterModel", FunctionName = "GetListofBankMaster()" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }
            return BankDetailsList;
        }
    }
    #endregion
    public class InsertData
    {
        public string InsertDataValue { get; set; }
    }

    public class InsertDataList
    {
        public List<InsertData> lstInsertData { get; set; }
    }






}