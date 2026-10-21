using Cooperative.Controllers;
using Cooperative.DBConnector;
using Cooperative.Filters;
using System;
using System.ComponentModel.Composition.Primitives;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using static Cooperative.Models.GopalCreditCardModel;

namespace Cooperative.Models
{
    public class MPacsMemberShipRegistrationDBModel
    {
        DBLogger objDBLogger = new DBLogger();
        private ILogger objILogger;
        private DBFactoryMSSQL objDBAccess;
        private DataTable dt;
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        public MPacsMemberShipRegistrationDBModel()
        {
            objILogger = new DBLogger();
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }

        public MPacsMemberShipRegistrationDBModel(ILogger objErrorLogger)
        {
            objILogger = objErrorLogger;
            objDBAccess = new DBFactoryMSSQL();
            dt = new DataTable();
        }


        public DataSet GetMPacsApplicationDetail(string referenceNo)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_MPacsMemberShipRegistration", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GetMembershipDataByRefNo");
                objDBAccess.AddParameterToCommand("@ReferenceNo", DBFactoryParameterType.VARCHAR, referenceNo);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPacsMemberShipRegistrationDBModel", FunctionName = "GetMPacsApplicationDetail(string referenceNo)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }
        public DataSet GetMPacsApplicationDetailBySSO(string searchText)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_MPacsMemberShipRegistration", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GetMembershipDataBySSO");
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.VARCHAR, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPacsMemberShipRegistrationDBModel", FunctionName = "GetMPacsApplicationDetailBySSO(string searchText)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }
        public DataSet GetMPacsApplicationDetailByUser(ref DataPagingModel Details, string searchText, int districtId = 0, int bankId = 0, int branchId = 0, string StatusType = "PS")

        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_MPacsMemberShipRegistration", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GetMembershipListByUser");
                objDBAccess.AddParameterToCommand("@UserId", DBFactoryParameterType.Int, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@MPacsPacsId", DBFactoryParameterType.Int, _LoggedInUser.GccId);
                objDBAccess.AddParameterToCommand("@RoleId", DBFactoryParameterType.Int, _LoggedInUser.RoleId);
                objDBAccess.AddParameterToCommand("@LevelId", DBFactoryParameterType.Int, _LoggedInUser.LevelId);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, _LoggedInUser.DistrictId == 0 ? districtId : _LoggedInUser.DistrictId);
                objDBAccess.AddParameterToCommand("@MPacsBankId", DBFactoryParameterType.Int, bankId);
                objDBAccess.AddParameterToCommand("@MPacsBranchId", DBFactoryParameterType.Int, branchId);
                objDBAccess.AddParameterToCommand("@SearchText", DBFactoryParameterType.NVarchar, searchText);
                objDBAccess.AddParameterToCommand("@PageSize", DBFactoryParameterType.Int, Details.PageSize);
                objDBAccess.AddParameterToCommand("@PageNumber", DBFactoryParameterType.Int, Details.PageNumber);

                objDBAccess.AddParameterToCommand("@StatusType", DBFactoryParameterType.NVarchar, StatusType);



                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPacsMemberShipRegistrationDBModel", FunctionName = "GetMPacsApplicationDetailByUser(string searchText, int districtId = 0, int bankId = 0, int branchId = 0)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }
        public DataSet GetMPacsApplicationDetailByEncId(string encId)
        {
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_MPacsMemberShipRegistration", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "GetMembershipDetailById");
                objDBAccess.AddParameterToCommand("@EncId", DBFactoryParameterType.NVarchar, encId);
                return objDBAccess.GetDataSet();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPacsMemberShipRegistrationDBModel", FunctionName = "GetMPacsApplicationDetailByUser(string encId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
            }

            return null;
        }

        public string SaveMPacsMembershipData(MPacsMemberShipRegistrationModel mPacsMember)
        {
            string Result = "-1";
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = objDBAccess.CreateCommand("p_MPacsMemberShipRegistration", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.VARCHAR, "Insert");
                objDBAccess.AddParameterToCommand("@Name", DBFactoryParameterType.NVarchar, mPacsMember.Name);
                objDBAccess.AddParameterToCommand("@DOB", DBFactoryParameterType.VARCHAR, mPacsMember.DOB);
                objDBAccess.AddParameterToCommand("@Age", DBFactoryParameterType.Int, mPacsMember.Age);
                objDBAccess.AddParameterToCommand("@Gender", DBFactoryParameterType.VARCHAR, mPacsMember.Gender);
                objDBAccess.AddParameterToCommand("@FatherName", DBFactoryParameterType.NVarchar, mPacsMember.FatherName);
                objDBAccess.AddParameterToCommand("@MotherName", DBFactoryParameterType.NVarchar, mPacsMember.MotherName);
                objDBAccess.AddParameterToCommand("@SpouseName", DBFactoryParameterType.NVarchar, mPacsMember.SpouseName);
                objDBAccess.AddParameterToCommand("@NomineeName", DBFactoryParameterType.NVarchar, mPacsMember.NomineeName);
                objDBAccess.AddParameterToCommand("@NomineeFatherName", DBFactoryParameterType.NVarchar, mPacsMember.NomineeFatherName);
                objDBAccess.AddParameterToCommand("@NomineeRelationId", DBFactoryParameterType.Int, mPacsMember.NomineeRelationId);
                objDBAccess.AddParameterToCommand("@EducationalQualificationId", DBFactoryParameterType.Int, mPacsMember.EducationalQualificationId);
                objDBAccess.AddParameterToCommand("@PhotoPath", DBFactoryParameterType.NVarchar, mPacsMember.PhotoPath);
                objDBAccess.AddParameterToCommand("@MobileNo", DBFactoryParameterType.VARCHAR, mPacsMember.MobileNo);
                objDBAccess.AddParameterToCommand("@CategoryId", DBFactoryParameterType.Int, mPacsMember.CategoryId);
                objDBAccess.AddParameterToCommand("@JanAadharNo", DBFactoryParameterType.VARCHAR, mPacsMember.JanAadharNo);
                objDBAccess.AddParameterToCommand("@JanAadharMID", DBFactoryParameterType.VARCHAR, mPacsMember.JanAadharMID);
                objDBAccess.AddParameterToCommand("@JanAadharEnrollmentId", DBFactoryParameterType.VARCHAR, mPacsMember.JanAadharEnrollmentId);
                objDBAccess.AddParameterToCommand("@JanAadharNoEnc", DBFactoryParameterType.VARCHAR, mPacsMember.JanAadharNoEnc);
                objDBAccess.AddParameterToCommand("@HouseBuildingNo", DBFactoryParameterType.NVarchar, mPacsMember.HouseBuildingNo);
                objDBAccess.AddParameterToCommand("@LaneStreetName", DBFactoryParameterType.NVarchar, mPacsMember.LaneStreetName);
                objDBAccess.AddParameterToCommand("@Locality", DBFactoryParameterType.NVarchar, mPacsMember.Locality);
                objDBAccess.AddParameterToCommand("@Address", DBFactoryParameterType.NVarchar, mPacsMember.Address);
                objDBAccess.AddParameterToCommand("@DistrictId", DBFactoryParameterType.Int, mPacsMember.DistrictId);
                objDBAccess.AddParameterToCommand("@IsRural", DBFactoryParameterType.Bit, mPacsMember.IsRural);
                objDBAccess.AddParameterToCommand("@BlockId", DBFactoryParameterType.Int, mPacsMember.BlockId);
                objDBAccess.AddParameterToCommand("@GpId", DBFactoryParameterType.Int, mPacsMember.GpId);
                objDBAccess.AddParameterToCommand("@VillageId", DBFactoryParameterType.Int, mPacsMember.VillageId);
                objDBAccess.AddParameterToCommand("@CityId", DBFactoryParameterType.Int, mPacsMember.CityId);
                objDBAccess.AddParameterToCommand("@WardId", DBFactoryParameterType.Int, mPacsMember.WardId);
                objDBAccess.AddParameterToCommand("@PinCode", DBFactoryParameterType.VARCHAR, mPacsMember.PinCode);
                objDBAccess.AddParameterToCommand("@PresentHouseBuildingNo", DBFactoryParameterType.NVarchar, mPacsMember.PresentHouseBuildingNo);
                objDBAccess.AddParameterToCommand("@PresentLaneStreetName", DBFactoryParameterType.NVarchar, mPacsMember.PresentLaneStreetName);
                objDBAccess.AddParameterToCommand("@PresentLocality", DBFactoryParameterType.NVarchar, mPacsMember.PresentLocality);
                objDBAccess.AddParameterToCommand("@PresentAddress", DBFactoryParameterType.NVarchar, mPacsMember.PresentAddress);
                objDBAccess.AddParameterToCommand("@PresentDistrictId", DBFactoryParameterType.Int, mPacsMember.PresentDistrictId);
                objDBAccess.AddParameterToCommand("@PresentIsRural", DBFactoryParameterType.Bit, mPacsMember.PresentIsRural);
                objDBAccess.AddParameterToCommand("@PresentBlockId", DBFactoryParameterType.Int, mPacsMember.PresentBlockId);
                objDBAccess.AddParameterToCommand("@PresentGpId", DBFactoryParameterType.Int, mPacsMember.PresentGpId);
                objDBAccess.AddParameterToCommand("@PresentVillageId", DBFactoryParameterType.Int, mPacsMember.PresentVillageId);
                objDBAccess.AddParameterToCommand("@PresentCityId", DBFactoryParameterType.Int, mPacsMember.PresentCityId);
                objDBAccess.AddParameterToCommand("@PresentWardId", DBFactoryParameterType.Int, mPacsMember.PresentWardId);
                objDBAccess.AddParameterToCommand("@PresentPinCode", DBFactoryParameterType.VARCHAR, mPacsMember.PresentPinCode);
                objDBAccess.AddParameterToCommand("@MPacsDistrictId", DBFactoryParameterType.Int, mPacsMember.MPacsDistrictId);
                objDBAccess.AddParameterToCommand("@MPacsBankId", DBFactoryParameterType.Int, mPacsMember.MPacsBankId);
                objDBAccess.AddParameterToCommand("@MPacsBranchId", DBFactoryParameterType.Int, mPacsMember.MPacsBranchId);
                objDBAccess.AddParameterToCommand("@MPacsPacsId", DBFactoryParameterType.Int, mPacsMember.MPacsPacsId);
                objDBAccess.AddParameterToCommand("@OtherPacsName", DBFactoryParameterType.NVarchar, mPacsMember.OtherPacsName);
                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.VARCHAR, mPacsMember.SSOID);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.Int, (mPacsMember.UserType == "C" ? 60 : mPacsMember.CreatedBy));
                objDBAccess.AddParameterToCommand("@UserType", DBFactoryParameterType.VARCHAR, mPacsMember.UserType);
                objDBAccess.AddParameterToCommand("@CreatedByIP", DBFactoryParameterType.VARCHAR, COMMON_FUNCTION.GetIpAddress());


                Result = Convert.ToString(objDBAccess.ExecuteScalarCommand());
                objDBAccess.CloseConnection();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPacsMemberShipRegistrationDBModel", FunctionName = "SaveMPacsMembershipData(MPacsMemberShipRegistrationModel mPacsMember)" }, ex);

            }
            finally
            {
                objDBAccess.CloseConnection();
            }
            return Result;
        }


        #region  Madhuri
        public DataTable GetMembershipProceesStatusList(int StatusId, string EncId)
        {
            dt.Clear();
            try
            {
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("p_MPacsMemberShipRegistration", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@EncId", DBFactoryParameterType.NVarchar, EncId);
                objDBAccess.AddParameterToCommand("@Action", DBFactoryParameterType.NVarchar, "GetMembershipStatusById");

                dt = objDBAccess.GetTable();
            }
            catch (Exception ex)
            {
                objILogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.MPacsMemberShipRegistrationDBModel", FunctionName = "GetGCCProceesStatusListIdsByIds(int StatusId, string GCC_Id)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();
                dt.Dispose();
            }

            return dt;
        }

        [CustomAuthentication]
        public int MpacsAddApplicationFlow(MpacsApplicationFlowDataModel _Model)
        {
            int Result = 0;
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            try
            {
                string committeeApprovalReport = null;
                string destDir = HttpContext.Current.Server.MapPath("~/Content/MPacsMembership/");

                if (!Directory.Exists(destDir))
                    Directory.CreateDirectory(destDir);
                if (_Model.StatusId == 3)
                {
                    if (_Model.CommitteeApprovalReport != null && _Model.CommitteeApprovalReport != "")
                    {
                        if (File.Exists(_Model.CommitteeApprovalReport))
                        {
                            committeeApprovalReport = Path.GetFileName(_Model.CommitteeApprovalReport);
                            File.Move(_Model.CommitteeApprovalReport, Path.Combine(destDir, committeeApprovalReport));
                        }
                    }
                }
                objDBAccess.OpenConnection();
                SqlCommand cmd = objDBAccess.CreateCommand("InsertMpacsApplicationFlow", DBFactoryCommandType.PROCEDURE_COMMAND);
                objDBAccess.AddParameterToCommand("@StatusId", DBFactoryParameterType.Int, _Model.StatusId);
                objDBAccess.AddParameterToCommand("@MPacsMembershipId", DBFactoryParameterType.Int, _Model.MPacsMembershipId);
                objDBAccess.AddParameterToCommand("@EncId", DBFactoryParameterType.NVarchar, _Model.EncId);
                objDBAccess.AddParameterToCommand("@InputPacsId", DBFactoryParameterType.Int, _Model.PacsId);
                objDBAccess.AddParameterToCommand("@Remark", DBFactoryParameterType.NVarchar, _Model.Remark);
                objDBAccess.AddParameterToCommand("@ReceiptNo", DBFactoryParameterType.NVarchar, _Model.ReceiptNo);
                objDBAccess.AddParameterToCommand("@DocId", DBFactoryParameterType.Int, _Model.DocumentId);
                objDBAccess.AddParameterToCommand("@DocumentURL", DBFactoryParameterType.NVarchar, _Model.DocumentName);
                if (_Model.StatusId == 5)
                {
                    objDBAccess.AddParameterToCommand("@MembershipCertificateFile", DBFactoryParameterType.NVarchar, _Model.MembershipCertificateFile);
                }
                if (_Model.StatusId == 3)
                {
                    objDBAccess.AddParameterToCommand("@CommitteeApprovalReport", DBFactoryParameterType.NVarchar, committeeApprovalReport);
                }

                objDBAccess.AddParameterToCommand("@SSOID", DBFactoryParameterType.NVarchar, _LoggedInUser.SSOID);
                objDBAccess.AddParameterToCommand("@CreatedBy", DBFactoryParameterType.BigInt, _LoggedInUser.UserId);
                objDBAccess.AddParameterToCommand("@CreatedByIP", DBFactoryParameterType.NVarchar, COMMON_FUNCTION.GetIpAddress().ToString());

                Result = Convert.ToInt32(objDBAccess.ExecuteScalarCommand());
            }
            catch (Exception ex)
            {
                Result = 0;
                DBLogger objDBLogger = new DBLogger();
                objDBLogger.ErrorHandle(new ErrorLogEntity() { PageName = "Cooperative.Models.GopalCreditCardDBModel", FunctionName = "AddApplicationFlow(string SocietyIDEnc, int StatusId, string Remark,int UserId)" }, ex);
            }
            finally
            {
                objDBAccess.CloseConnection();

            }
            return Result;
        }



        #endregion 
    }
}

