using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TaskRegistrationProject
{
    public partial class UserInfo : System.Web.UI.Page
    {
        OracleConnection conStr;
        OracleDataAdapter oda;
        DataTable dt;
        string sQuery = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            conStr = new OracleConnection(ConfigurationManager.ConnectionStrings["Connection"].ToString());
            if (!IsPostBack)
            {
                #region City Name
                DataTable dtCityName = new DataTable();
                dtCityName = getCity();

                ddlCity.DataSource = dtCityName;
                ddlCity.DataTextField = "CITYNAME";
                ddlCity.DataValueField = "STATEID";
                ddlCity.DataBind();
                #endregion

                #region Course Name
                DataTable dtCourseName = new DataTable();
                dtCourseName = getCourse();

                ddlCourse.DataSource = dtCourseName;
                ddlCourse.DataTextField = "COURSE_NAME";
                ddlCourse.DataValueField = "COURSE_NAME";
                ddlCourse.DataBind();
                #endregion
            }
        }

        public DataTable getCity()
        {
            DataTable dtCity = new DataTable();
            sQuery = "SELECT CITYNAME,STATEID FROM CITIES";
            oda = new OracleDataAdapter(sQuery, conStr);
            oda.Fill(dtCity);

            return dtCity;
        }

        public DataTable getCourse()
        {
            DataTable dtCourse = new DataTable();
            sQuery = "SELECT COURSE_NAME FROM COURSE";
            oda = new OracleDataAdapter(sQuery, conStr);
            oda.Fill(dtCourse);

            return dtCourse;
        }

        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dtState = new DataTable();
                sQuery = "SELECT STATENAME,COUNTRYID FROM STATES WHERE STATEID=:STATEID";
                oda = new OracleDataAdapter(sQuery, conStr);
                oda.SelectCommand.Parameters.AddWithValue(":STATEID", ddlCity.SelectedValue);
                oda.Fill(dtState);

                if (dtState.Rows.Count > 0)
                {
                    txtState.Text = dtState.Rows[0]["STATENAME"].ToString();

                    DataTable dtCountry = new DataTable();
                    sQuery = "SELECT COUNTRYNAME FROM COUNTRIES WHERE COUNTRYID =:COUNTRYID";
                    oda = new OracleDataAdapter(sQuery, conStr);
                    oda.SelectCommand.Parameters.AddWithValue(":COUNTRYID", dtState.Rows[0]["COUNTRYID"].ToString());
                    oda.Fill(dtCountry);

                    if (dtCountry.Rows.Count > 0)
                    {
                        txtCountry.Text = dtCountry.Rows[0]["COUNTRYNAME"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            dt = getUserInfoDetails(txtSearch.Text);

            if (dt.Rows.Count > 0)
            {
                registerIdMsg.Visible = true;
                registerBtn.Visible = true;

                lblMsg.Text = "Register ID : " + txtSearch.Text;
                lblMsg.ForeColor = System.Drawing.Color.Black;

                Session["RegisterDetails"] = dt;
                Session["RegisterID"] = txtSearch.Text; 

                txtSearch.Text = "";
            }
            else
            {
                registerIdMsg.Visible = true;
                registerBtn.Visible = false;

                lblMsg.Text = "Register ID is Not Found.";
                lblMsg.ForeColor = System.Drawing.Color.Red;

                txtSearch.Text = "";
            }
        }


        public DataTable getUserInfoDetails(string searchVal)
        {
            DataTable dtUserDetails = new DataTable();
            sQuery = "SELECT NAME,TO_CHAR(DOB,'DD/MM/YYYY') AS DOB,TO_CHAR(TRANSACTION_DATE,'DD/MM/YYYY') AS TRANSACTION_DATE,ADDRESS,CITY,STATE,ZIPCODE,COUNTRY,PHONENUMBER,EMAIL,COURSE,MEMBERSHIPTYPE,PAYMENTMETHOD,COMMENTS FROM USERINFORMATION WHERE REGISTER_ID=:REGISTER_ID";
            oda = new OracleDataAdapter(sQuery, conStr);
            oda.SelectCommand.Parameters.AddWithValue(":REGISTER_ID", searchVal);
            oda.Fill(dtUserDetails);

            return dtUserDetails;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string dob = txtDOB.Text;
            string address = txtAddress.Text;
            string city = ddlCity.SelectedItem.Text;
            string state = txtState.Text;
            string zipCode = txtZip.Text;
            string country = txtCountry.Text;
            string phoneNumber = txtPhone.Text;
            string email = txtEmail.Text;
            string membershipType = rblMembership.SelectedValue;
            string course = ddlCourse.SelectedValue;
            string paymentMethod = rblPayment.SelectedValue;
            string comments = txtComments.Text;

            try
            {
                string formattedDOB = DateTime.Parse(dob).ToString("dd/MM/yyyy");

                sQuery = "UPDATE USERINFORMATION SET  NAME = :NAME, DOB = TO_DATE('" + formattedDOB + "', 'DD/MM/YYYY'), ADDRESS = :ADDRESS," +
                    " CITY = :CITY, STATE = :STATE, ZIPCODE = :ZIPCODE, COUNTRY = :COUNTRY, PHONENUMBER = :PHONENUMBER, EMAIL = :EMAIL, " +
                    "MEMBERSHIPTYPE = :MEMBERSHIPTYPE, COURSE = :COURSE, PAYMENTMETHOD = :PAYMENTMETHOD, COMMENTS = :COMMENTS WHERE REGISTER_ID = :REGISTER_ID";

                OracleCommand cmd = new OracleCommand(sQuery, conStr);
                cmd.Parameters.AddWithValue(":NAME", name);
                cmd.Parameters.AddWithValue(":ADDRESS", address);
                cmd.Parameters.AddWithValue(":CITY", city);
                cmd.Parameters.AddWithValue(":STATE", state);
                cmd.Parameters.AddWithValue(":ZIPCODE", zipCode);
                cmd.Parameters.AddWithValue(":COUNTRY", country);
                cmd.Parameters.AddWithValue(":PHONENUMBER", phoneNumber);
                cmd.Parameters.AddWithValue(":EMAIL", email);
                cmd.Parameters.AddWithValue(":MEMBERSHIPTYPE", membershipType);
                cmd.Parameters.AddWithValue(":COURSE", course);
                cmd.Parameters.AddWithValue(":PAYMENTMETHOD", paymentMethod);
                cmd.Parameters.AddWithValue(":COMMENTS", comments);
                cmd.Parameters.AddWithValue(":REGISTER_ID", Session["RegisterID"].ToString());

                conStr.Open();

                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    showToast("Data Update Successfully!!");
                }
                else
                {
                    showToast("Data Update Failed!!", "error");
                }
            }
            catch (Exception ex)
            {
                showToast("Something Went Wrong.", "error");
            }
            finally
            {
                conStr.Close();
                registerDetails.Visible = false;
                searchRegister.Visible = true;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                sQuery = "DELETE USERINFORMATION WHERE REGISTER_ID=:REGISTER_ID";
                OracleCommand cmd = new OracleCommand(sQuery, conStr);
                cmd.Parameters.AddWithValue(":REGISTER_ID", Session["RegisterID"].ToString());

                conStr.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    showToast("Data Deleted Successfully!!");
                }
                else
                {
                    showToast("Data Deteted Failed!!", "error");
                }
            }
            catch (Exception ex)
            {
                showToast("Something Went Wrong.", "error");

            }
            finally
            {
                conStr.Close();
               
                registerDetails.Visible = false;
                registerIdMsg.Visible = true;
                searchRegister.Visible = true;
            }
        }

        private void showToast(string msg, string type = "success")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "<script>toastr." + type + "('" + msg + "');</script>", false);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            registerIdMsg.Visible = false;

            DataTable dt = new DataTable();

            dt = (DataTable)Session["RegisterDetails"];

            if (dt.Rows.Count > 0)
            {
                registerDetails.Visible = true;
                searchRegister.Visible = false;

                lblRegisterID.Text = "Register ID : " + Session["RegisterID"].ToString();

                string formattedDate = DateTime.Parse(dt.Rows[0]["DOB"].ToString()).ToString("yyyy-MM-dd");

                txtName.Text = dt.Rows[0]["NAME"].ToString();
                txtDOB.Text = formattedDate;
                txtAddress.Text = dt.Rows[0]["ADDRESS"].ToString();
                ddlCity.SelectedValue = getCityValue(dt.Rows[0]["CITY"].ToString());
                txtState.Text = dt.Rows[0]["STATE"].ToString();
                txtCountry.Text = dt.Rows[0]["COUNTRY"].ToString();
                txtZip.Text = dt.Rows[0]["ZIPCODE"].ToString();
                txtPhone.Text = dt.Rows[0]["PHONENUMBER"].ToString();
                txtEmail.Text = dt.Rows[0]["EMAIL"].ToString();
                rblMembership.Text = dt.Rows[0]["MEMBERSHIPTYPE"].ToString();
                ddlCourse.SelectedItem.Text = dt.Rows[0]["COURSE"].ToString();
                rblPayment.Text = dt.Rows[0]["PAYMENTMETHOD"].ToString();
                txtComments.Text = dt.Rows[0]["COMMENTS"].ToString();
            }
            else
            {
                registerDetails.Visible = false;
                searchRegister.Visible = true;
            }
        }

        public string getCityValue(string cityName)
        {
            sQuery = "SELECT STATEID FROM CITIES WHERE CITYNAME =:CITYNAME";
            oda = new OracleDataAdapter(sQuery, conStr);
            oda.SelectCommand.Parameters.AddWithValue(":CITYNAME", cityName);
            dt = new DataTable();
            oda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["STATEID"].ToString();
            }
            return "1";
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Response.Redirect("PrintPDF.aspx");
        }
    }
}