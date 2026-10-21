using Cooperative.Models;
using System;
using System.Data;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cooperative.Filters;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Cooperative.Controllers
{
    [LogActionFilter]
    [CustomAuthentication]
    public class InspectionController : Controller
    {
        UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
        // GET: Inspection
        public ActionResult Inspection()
        {
            return View();
        }
        public ActionResult InspectionQuestions(string InspectionID)
        {
            ViewBag.InspectionIDEnc = InspectionID;
            return View();
        }



        [HttpPost]
        public string InspectionData(SocietyRegistrationEntity objSocietyRegistrationEntity)
        {
            InspectionModel objInspectionModel = new InspectionModel(new FileLogger());

            if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"].Value == "KIOSK" && Request.Cookies["SSO_ID"] != null && System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc] != null)
            {
                objSocietyRegistrationEntity.UserType = "K";
                // objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
                objSocietyRegistrationEntity.SocietyIDEnc = System.Web.HttpContext.Current.Session[SessionIdentity.SocietyIDEnc].ToString().Split('~')[0];
            }
            else if (Request.Cookies["RAJSSO"] != null && Request.Cookies["SSOROLE"] != null && Request.Cookies["SSO_ID"] != null)
            {
                objSocietyRegistrationEntity.UserType = Request.Cookies["SSOROLE"].Value == "KIOSK" ? "K" : (Request.Cookies["SSOROLE"].Value == "CITIZEN" ? "C" : "D");
                objSocietyRegistrationEntity.SSOID = Request.Cookies["SSO_ID"].Value;
            }
            DataTable DT = objInspectionModel.InspectionData(objSocietyRegistrationEntity);
            foreach (DataRow Dr in DT.Rows)
            {
                Dr["InspectionIDEnc"] = COMMON_FUNCTION.Encrypt(Dr["InspectionIDEnc"].ToString());
            }
            //DT.Rows[0]["InspectionIDEnc"] = COMMON_FUNCTION.Encrypt(DT.Rows[0]["InspectionID"].ToString());

            //DT.Rows[0]["InspectionIDEnc"] = COMMON_FUNCTION.Encrypt(DT.Rows[0]["InspectionID"].ToString());

            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpPost]
        public string InspectionForSocietyOfUser(int SocietyTypeId = 0, int ClassId = 0 , int FYID=0)
        {
            InspectionModel objInspectionModel = new InspectionModel(new FileLogger());


            DataTable DT = objInspectionModel.InspectionForSocietyOfUser(SocietyTypeId, ClassId, FYID);
            foreach (DataRow Dr in DT.Rows)
            {
                Dr["InspectionIDEnc"] = COMMON_FUNCTION.Encrypt(Dr["InspectionID"].ToString());
            }
            //DT.Rows[0]["InspectionIDEnc"] = COMMON_FUNCTION.Encrypt(DT.Rows[0]["InspectionID"].ToString());


            return COMMON_FUNCTION.DataTableToJSONString(DT);
        }

        [HttpPost]
        public JsonResult SaveInspection(InspectiondataEntity ObjInspection)
        {
            if (!ModelState.IsValid)
                return Json(JsonModelError.Get(ModelState.Values));

            ObjInspection.CreatedBy = _LoggedInUser.UserId;
            ObjInspection.UpdatedBy = _LoggedInUser.UserId;
            // ObjInspection.Year = ObjInspection.DateOfStartInspection.Year;
            ObjInspection.Tenure = 0;// ObjInspection.Tenure;

            InspectionModel objInspectionModel = new InspectionModel(new FileLogger());
            return Json(objInspectionModel.AddInspection(ObjInspection));

        }

        [HttpPost]
        [ActionName("SaveInspectionReplyByOfficer")]
        public string SaveInspectionReply(InspectionAnsSave ObjInspectionAnsSave)
        {
            //_Entity.CreatedBy = _Entity.UpdatedBy = _LoggedInUser.UserId;


            string flag = "0";
            //string fileName = string.Empty;
            string ReturnJSON = "";
            //string pathString = "~/Content/Compliance/";

            try
            {

                InspectionModel objInspectionModel = new InspectionModel(new FileLogger());
                if (ObjInspectionAnsSave != null && ObjInspectionAnsSave.InspectionAnsList.Count > 0)
                {
                    UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
                    ObjInspectionAnsSave.InspectionAnsList[0].CreatedBy = _LoggedInUser.UserType == "C" ? 0 : _LoggedInUser.UserId;
                    ObjInspectionAnsSave.InspectionAnsList[0].SSO_ID = _LoggedInUser.SSOID;
                    ObjInspectionAnsSave.InspectionAnsList[0].CurrentUserType = _LoggedInUser.UserType == "C" ? 0 : 1;
                    flag = (objInspectionModel.InsertInspectionReply(ObjInspectionAnsSave)).ToString();
                }
            }
            catch (Exception ex)
            {

            }

            ReturnJSON = "{\"data\":\"" + flag.ToString() + "\"}";
            return ReturnJSON;

        }

        [HttpPost]
        public JsonResult UploadInspectionFile(InspectiondataEntity objInspectiondataEntity)
        {
            string flag = "0";
            string fileName = string.Empty;
            string SocRegNo = string.Empty;
            string pathString = "~/Content/SocietyProfiling/";

            InspectionModel objInspectionModel = new InspectionModel(new FileLogger());

            DataTable DT = objInspectionModel.InspectionQuestionData(Convert.ToInt32(objInspectiondataEntity.InspectionID));

            if (DT != null && DT.Rows.Count > 0)
            {
                SocRegNo = DT.Rows[0]["SOCDirect"].ToString();

                pathString += Regex.Replace(SocRegNo, "[^a-zA-Z0-9_]+", "");
                if (!System.IO.File.Exists(pathString))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(pathString));
                }

                foreach (string files in Request.Files)
                {

                    HttpPostedFileBase file = Request.Files[files];
                    fileName = Guid.NewGuid().ToString().Replace("/", "").Replace("-", "").ToUpper() + Path.GetExtension(file.FileName);
                    file.SaveAs(Path.Combine(Server.MapPath(pathString), fileName));
                    if (files.ToString() == "InspectionFile")
                    {
                        objInspectiondataEntity.InspectionFile = fileName;
                    }
                }
            }

            return Json(objInspectionModel.UploadInspectionFile(objInspectiondataEntity));
        }


        [HttpPost]
        public string InspectionQuestionData(string InspectionID)
        {
            UserEntity _LoggedInUser = (UserEntity)System.Web.HttpContext.Current.Session[SessionIdentity.Admin];
            var IsDisable = false;
            int Inspectionid = 0;
            InspectionModel objInspectionModel = new InspectionModel(new FileLogger());

            if (InspectionID.Contains(" "))
            {
                InspectionID = (InspectionID.Replace(' ', '+'));
            }
            Inspectionid = int.Parse(COMMON_FUNCTION.Decrypt(InspectionID));
            DataTable DT = objInspectionModel.InspectionQuestionData(Inspectionid);
            string Divstr = "";
            if (DT != null && DT.Rows.Count > 0)
            {
                Divstr = "<input type=\"hidden\" id='hdfUserType' value=" + _LoggedInUser.UserType.ToString() + " ><input type=\"hidden\" id='hdfInspectionid' value=" + Inspectionid.ToString() + " ><table width='100%' id='MainTable' class='table devtable'>";
                Divstr += "<thead><tr><th align='center' style=\"border: 1px solid black\"> Sr.No  </th>";
                Divstr += "<th align='center' style=\"border: 1px solid black\">  Question  </th>";
                Divstr += "<th align='center' style=\"border: 1px solid black\">  Is Objection</th>";
                Divstr += "<th align='center' style=\"border: 1px solid black\">  ANSWER  </th>";
                Divstr += "<th align='center' style=\"border: 1px solid black\"> Remark  </th>";
                if (_LoggedInUser.UserType == "C" || DT.Rows[0]["CEORemark"].ToString().Length > 1)
                {
                    Divstr += "<th align='center' style=\"border: 1px solid black\">  CEO Remark </th> ";
                }
                if (_LoggedInUser.UserType == "C")
                {
                    IsDisable = true;
                }
                Divstr += "</tr></thead><tbody>";
                int count = 0;
                int tblcount = 0;
                int TempGroupId = 0;

                int TempQId = 0;
                int TempQType = 0;

                foreach (DataRow DR in DT.Rows)
                {

                    if (DR["GroupId"].ToString() == "0" && DR["IsTable"].ToString() == "False")
                    {
                        count = count + 1;
                        Divstr += "<tr id=tr_" + DR["QuestionId"].ToString() + ">";
                        if (DR["IsObjection"].ToString() == "Checked")
                        {
                            Divstr += "<td align='center' style=\"border: 1px solid black;background-color:#ffcccc\" >" + count + "  </td>";
                        }
                        else
                        {
                            Divstr += "<td align='center' style=\"border: 1px solid black;\" >" + count + "  </td>";
                        }

                        Divstr += "<td align='left' style=\"border: 1px solid black;\"  >" + DR["QuestionHi"].ToString() + "  </td>";
                        Divstr += "<td align='left' style=\"border: 1px solid black;\"><input " + (IsDisable == true ? "disabled" : "") + " type=\"checkbox\" name=\"IsObjection\" " + DR["IsObjection"].ToString() + " style=\"border: 1px solid black;\" ></td>";
                        if (DR["QuestionId"].ToString() == "1")
                        {
                            Divstr += "<td align='center' style=\"border: 1px solid black\" ><textarea " + (IsDisable == true ? "disabled" : "") + " name=\"ContactNo\" style=\"margin: 0px 8.84766px 0px 8.4375px; width: 357px; height: 57px;\" class=\"form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse\"  placeholder=\"उत्तर\" id=" + DR["QuestionId"].ToString() + " autocomplete =\"off\">" + DR["SocietyName"].ToString() + " / " + DR["RegistrationDate"].ToString() + "</textarea></td>";
                        }
                        else
                        {
                            Divstr += "<td align='center' style=\"border: 1px solid black\" ><textarea " + (IsDisable == true ? "disabled" : "") + " name=\"ContactNo\" style=\"margin: 0px 8.84766px 0px 8.4375px; width: 357px; height: 57px;\" class=\"form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse\"   placeholder=\"उत्तर\" id=" + DR["QuestionId"].ToString() + " autocomplete =\"off\">" + DR["Answear"].ToString() + "</textarea></td>";
                        }

                        Divstr += "<td style=\"border: 1px solid black\" ><textarea " + (IsDisable == true ? "disabled" : "") + " name=\"ContactNo\" style=\"margin: 0px 8.84766px 0px 8.4375px; width: 357px; height: 57px;\" class=\"form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse\"   placeholder =\"Remark(टिप्पणी)\" id=" + DR["QuestionId"].ToString() + " autocomplete =\"off\">" + DR["InspectorRemark"].ToString() + "</textarea></td >";

                        if (_LoggedInUser.UserType == "C" || DR["CEORemark"].ToString().Length > 1)
                        {
                            Divstr += "<td style=\"border: 1px solid black\" ><textarea rows=\"2\" cols=\"30\"  name=\"ContactNo\" style=\"margin: 0px 8.84766px 0px 8.4375px; width: 357px; height: 57px;\" class=\"form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse\"   placeholder=\"CEO टिप्पणी\" id='" + DR["AnswerId"].ToString() + "' autocomplete =\"off\">" + DR["CEORemark"].ToString() + "</textarea></td >";
                            //Divstr += "<td style=\"border: 1px solid black\" ><textarea " + (_LoggedInUser.UserType != "C" ? "disabled" : "") + "  rows=\"2\" cols=\"30\"  name=\"ContactNo\" class=\"form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse\"   placeholder=\"CEO टिप्पणी\" id='" + DR["AnswerId"].ToString() + "' autocomplete =\"off\">" + DR["CEORemark"].ToString() + "</textarea></td >";
                        }
                        Divstr += "</tr>";
                    }
                    else if (int.Parse(DR["GroupId"].ToString()) > 0 && DR["IsTable"].ToString() == "False")
                    {
                        if (TempGroupId != int.Parse(DR["GroupId"].ToString()))
                        {
                            count = count + 1;
                            TempGroupId = int.Parse(DR["GroupId"].ToString());
                            if (DR["IsObjection"].ToString() == "Checked")
                            {
                                Divstr += "<tr style=\"background-Color:#BC5F5F\">";
                            }
                            else
                            {
                                Divstr += "<tr>";
                            }

                            if (DR["IsObjection"].ToString() == "Checked")
                            {
                                Divstr += "<td align='center' style=\"border: 1px solid black;background-color:#ffcccc\" >" + count + "  </td>";
                            }
                            else
                            {
                                Divstr += "<td align='center' style=\"border: 1px solid black;\" >" + count + "  </td>";
                            }

                            //Divstr += "<td align='center' style=\"border: 1px solid black\" >" + count + "  </td>";
                            Divstr += "<td colspan=\"5\" align='left' style=\"border: 1px solid black\" ><b>" + DR["GroupHi"].ToString() + " </b></td></tr>";
                        }

                        Divstr += "<tr id=tr_" + DR["QuestionId"].ToString() + ">";


                        if (DR["IsObjection"].ToString() == "Checked")
                        {
                            Divstr += "<td align='center' style=\"border: 1px solid black;background-color:#ffcccc\" ></td>";
                        }
                        else
                        {
                            Divstr += "<td align='center' style=\"border: 1px solid black\" ></td>";
                        }
                        Divstr += "<td align='left' style=\"border: 1px solid black\" >&nbsp;&nbsp;&nbsp;" + DR["QuestionHi"].ToString() + "  </td>";
                        Divstr += "<td align='left' style=\"border: 1px solid black\" ><input " + (IsDisable == true ? "disabled" : "") + " id=Chk_" + DR["QuestionId"].ToString() + " type=\"checkbox\" " + DR["IsObjection"].ToString() + " name=\"IsObjection\" ></td>";
                        Divstr += "<td align='center' style=\"border: 1px solid black\" ><textarea " + (IsDisable == true ? "disabled" : "") + " name=\"ContactNo\" style=\"border: 1px solid black; margin: 0px 8.84766px 0px 8.4375px; width: 357px; height: 57px;\" class=\"form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse\" placeholder=\"उत्तर\" id=" + DR["QuestionId"].ToString() + " autocomplete =\"off\">" + DR["Answear"].ToString() + "</textarea></td>";
                        Divstr += "<td style=\"border: 1px solid black\" ><textarea " + (IsDisable == true ? "disabled" : "") + " name=\"ContactNo\" style=\"border: 1px solid black; margin: 0px 8.84766px 0px 8.4375px; width: 357px; height: 57px;\" class=\"form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse\" placeholder=\"Remark(टिप्पणी)\" id=" + DR["QuestionId"].ToString() + " autocomplete =\"off\">" + DR["InspectorRemark"].ToString() + "</textarea></td >";
                        if (_LoggedInUser.UserType == "C" || DR["CEORemark"].ToString().Length > 1)
                        {
                            Divstr += "<td style=\"border: 1px solid black\" ><textarea name=\"ContactNo\" style=\"border: 1px solid black; margin: 0px 8.84766px 0px 8.4375px; width: 357px; height: 57px;\"  class=\"form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse\" placeholder=\"CEO टिप्पणी\" id=" + DR["AnswerId"].ToString() + " autocomplete =\"off\">" + DR["CEORemark"].ToString() + "</textarea></td >";
                            //Divstr += "<td style=\"border: 1px solid black\" ><textarea " + (_LoggedInUser.UserType != "C" ? "disabled" : "") + " name=\"ContactNo\" style=\"border: 1px solid black\" class=\"form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse\" placeholder=\"CEO टिप्पणी\" id=" + DR["AnswerId"].ToString() + " autocomplete =\"off\">" + DR["CEORemark"].ToString() + "</textarea></td >";
                        }
                        Divstr += "</tr>";
                    }
                    else if (int.Parse(DR["GroupId"].ToString()) > 0 && DR["IsTable"].ToString() == "True")
                    {
                        if (DR["IsObjection"].ToString() == "Checked")
                        {
                            Divstr += "<tr style=\"background-Color:#BC5F5F\">";
                        }
                        else
                        {
                            Divstr += "<tr>";
                        }


                        if (TempGroupId != int.Parse(DR["GroupId"].ToString()))
                        {
                            count = count + 1;
                            TempGroupId = int.Parse(DR["GroupId"].ToString());
                            TempQId = int.Parse(DR["QuestionId"].ToString());

                            if (DR["IsObjection"].ToString() == "Checked")
                            {
                                Divstr += "<tr style=\"background-Color:#BC5F5F\">";
                            }
                            else
                            {
                                Divstr += "<tr>";
                            }

                            //Divstr += "<td align='center' style=\"border: 1px solid black\" >" + count + "  </td>";
                            if (DR["IsObjection"].ToString() == "Checked")
                            {
                                Divstr += "<td align='center' style=\"border: 1px solid black;background-color:#ffcccc\" >" + count + "  </td>";
                            }
                            else
                            {
                                Divstr += "<td align='center' style=\"border: 1px solid black;\" >" + count + "  </td>";
                            }

                            Divstr += "<td colspan=\"5\" align='left' style=\"border: 1px solid black\" ><b>" + DR["QuestionHi"].ToString() + " </b></td></tr>";
                        }
                        else
                        {
                            if (TempQId != int.Parse(DR["QuestionId"].ToString()))
                            {
                                //Divstr += "<td align='center' style=\"border: 1px solid black\" ></td>";
                                if (DR["IsObjection"].ToString() == "Checked")
                                {
                                    Divstr += "<td align='center' style=\"border: 1px solid black;background-color:#ffcccc\" ></td>";
                                }
                                else
                                {
                                    Divstr += "<td align='center' style=\"border: 1px solid black;\" ></td>";
                                }

                                Divstr += "<td colspan=\"5\" align='left' style=\"border: 1px solid black\" ><b>" + DR["QuestionHi"].ToString() + " </b></td></tr>";
                            }
                            TempGroupId = int.Parse(DR["GroupId"].ToString());
                            TempQId = int.Parse(DR["QuestionId"].ToString());
                        }
                        //Divstr += "<td align='center' style=\"border: 1px solid black\" >" + count + "  </td>";
                        //Divstr += "<td colspan=\"5\" align='left' style=\"border: 1px solid black\" >" + DR["QuestionHi"].ToString() + "  </td></tr>";
                        DataTable dt = objInspectionModel.GetTableHeaderDataByQuestionId(DR["QuestionId"].ToString(), Inspectionid);
                        DataView view = new DataView(dt);
                        DataTable distinctValues = view.ToTable(true, "TableId", "HeaderHi");
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            tblcount++;
                            int C = dt.Rows.Count;
                            Divstr += "<tr>";
                            Divstr += "<td align='left' style=\"border: 1px solid black\" ><input type=\"hidden\" id=\"hfQuestionId\" value='" + DR["QuestionId"].ToString() + "' ></input></td>";
                            Divstr += "<td colspan=\"5\" align='center' style=\"border: 1px solid black\" >";
                            //Divstr += "<table width='100%' id=tbl_" + tblcount + " class='table devtable'>";
                            Divstr += "<table width='100%' id=tbl_" + DR["QuestionId"].ToString() + " class='table devtable'>";
                            Divstr += "<thead><tr>";
                            foreach (DataRow dr in distinctValues.Rows)
                            {
                                Divstr += "<th align='center' style=\"border: 1px solid black\">  " + dr["HeaderHi"].ToString() + "   </th>";
                            }
                            Divstr += "<th align = 'center' style =\"border: 1px solid black\"></th>";
                            Divstr += "</tr></thead><tr>";
                            int HeaderCount = distinctValues.Rows.Count;
                            int TempCount = 0;
                            int RowCount = 0;

                            foreach (DataRow dr in dt.Rows)
                            {

                                TempCount = TempCount + 1;
                                if (TempCount <= HeaderCount)
                                {
                                    Divstr += "<td><label style=\"display: none; \">" + (dr["UserType"].ToString().Trim() == "" ? "1" : dr["UserType"].ToString().Trim()) + "</label><input " + (IsDisable == true ? "disabled" : "") + " type=\"text\" name=\"ContactNo\" value='" + dr["Answear"].ToString() + "' class=\"form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse\" id=" + dr["TableId"].ToString() + " autocomplete =\"off\"></td >";
                                    if (HeaderCount == TempCount)
                                    {
                                        if (RowCount == 0)
                                            Divstr += "<td><input type=\"button\" class=\"btn btn-primary dev-btn ng-binding\" value=\"जोड़ें\" onclick=\"AddNewrow(" + DR["QuestionId"].ToString() + "," + HeaderCount + ")\"></td ></tr>";
                                        else
                                        {
                                            if (IsDisable == true && _LoggedInUser.UserType == "C")
                                            {
                                                Divstr += "<td></td ></tr>";
                                            }
                                            else
                                            {
                                                Divstr += "<td><input type=\"button\" class=\"btn btn-primary dev-btn ng-binding\" value=\"REMOVE\" onclick=\"Removerow(this)\"></td ></tr>";
                                            }
                                        }
                                        RowCount = 1;
                                    }
                                }
                                else
                                {
                                    TempCount = 1;
                                    Divstr += "<tr><td><label style=\"display: none; \">" + (dr["UserType"].ToString().Trim() == "" ? "1" : dr["UserType"].ToString().Trim()) + "</label><input " + (IsDisable == true ? "disabled" : "") + " type=\"text\" name=\"ContactNo\" value='" + dr["Answear"].ToString() + "' class=\"form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse\" id=" + dr["TableId"].ToString() + " autocomplete =\"off\"></td >";

                                }
                                //Divstr += "</tr><tr><td><input " + (IsDisable == true ? "disabled" : "") + " type=\"text\" name=\"ContactNo\" value='" + dr["Answear"].ToString() + "' class=\"form-control ng-valid ng-touched ng-not-empty ng-dirty ng-valid-parse\" id=" + dr["TableId"].ToString() + " autocomplete =\"off\"></td >";

                            }

                            //Divstr += "<td><input type=\"button\" class=\"btn btn-primary dev-btn ng-binding\" value=\"जोड़ें\" onclick=\"AddNewrow(" + DR["QuestionId"].ToString() + "," + C + ")\"></td ></tr><tr>";
                            Divstr += "</table>";
                            Divstr += "</td></tr>";

                        }
                    }
                }

                Divstr += "</tbody>";
                Divstr += "</table>";
                Divstr += "@#" + DT.Rows[0]["Isliquideter"].ToString();
            }
            else
            {
                Divstr = "-1";
            }
            return Divstr;//COMMON_FUNCTION.DataTableToJSONString(DT);

        }


        #region Inspection Initiate

        public ActionResult InspectionInitiate()
        {
            return View();
        }

        [HttpGet]
        public string GetSocietyListPrimary(string FYID = "")
        {
            InspectionModel _citobj = new InspectionModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(_citobj.GetSocietyListPrimary(FYID));
        }

        [HttpGet]
        public string InspectionOfficerForSociety()
        {
            InspectionModel _citobj = new InspectionModel(new FileLogger());
            return COMMON_FUNCTION.DataTableToJSONString(_citobj.GetInspectionOfficerForSociety());
        }


        [HttpPost]
        public string SaveInspectionOfficerSocietyData(FormData onjInspectionBasicData)
        {
            List<InspectionBasicData> objData = new List<InspectionBasicData>();
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();

            try
            {
                objData = Request.Form["DataArray"].ToString().Equals("[]") ? new List<InspectionBasicData>() : (List<InspectionBasicData>)s.Deserialize(Request.Form["DataArray"].ToString(), typeof(List<InspectionBasicData>));
            }
            catch (Exception ex)
            {
                return CommonMessageFactory.Response("Provided input data is not in correct format.");
            }

            InspectionModel _citobj = new InspectionModel(new FileLogger());
            return (_citobj.SaveInspectionOfficerSocietyData(objData));
        }
        public class FormData
        {
        }
        #endregion

        [HttpPost]
        public string InspectionDataForDashboard(string FinancialYearId)
        {
            DataTable DT = null;
            DataTable DT2 = null;
            int _FIN_YEAR_ID = 0;
            int.TryParse(FinancialYearId, out _FIN_YEAR_ID);
            InspectionModel objAuditorModel = new InspectionModel(new DBLogger());
            DT = objAuditorModel.GetSocietyDataByLevelIdYearId(_LoggedInUser.LevelId, Convert.ToInt32(_LoggedInUser.UserId), _FIN_YEAR_ID);

            InspectionModel objAuditorModel2 = new InspectionModel(new DBLogger());
            DT2 = objAuditorModel2.GetSocietyInspectionAuditStatus(_LoggedInUser.LevelId, Convert.ToInt32(_LoggedInUser.UserId), _FIN_YEAR_ID);

            return "{\"data1\":" + COMMON_FUNCTION.DataTableToJSONString(DT) + ",\"data2\": " + COMMON_FUNCTION.DataTableToJSONString(DT2) + "}";
        }

        [HttpPost]
        public string GetAuditorDashBoardData(string ActionId,string ActId="")
        {
            DataTable DT = null;

            int _ActionId = 0,_ActId=0;
            int.TryParse(ActionId, out _ActionId);
            int.TryParse(ActId, out _ActId);

            InspectionModel objAuditorModel = new InspectionModel(new DBLogger());

            int _UserLevel = _LoggedInUser.LevelId;
            int _Id = 0;

            if (_UserLevel == 1)
                _Id = _LoggedInUser.StateId;
            else if (_UserLevel == 2)
                _Id = _LoggedInUser.DivisionId;
            else if (_UserLevel == 3)
                _Id = _LoggedInUser.DistrictId;
            else if (_UserLevel == 4)
                _Id = _LoggedInUser.BlockId;
            else if (_UserLevel == 5)
                _Id = _LoggedInUser.GPId;
            else if (_UserLevel == 6)
                _Id = _LoggedInUser.VillageId;
            else
                _Id = -1;

            //var Result = objAuditorModel.GetAuditorDashBoardData(_UserLevel, _Id,_ActionId,_ActId);
            var Result = objAuditorModel.GetAuditorDashBoardDataNew(_LoggedInUser.UserRoleDeptMappingId, _ActId);

            return JsonConvert.SerializeObject(Result, Newtonsoft.Json.Formatting.Indented);
        }
    }
}