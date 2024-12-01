using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TaskRegistrationProject
{
    public partial class Registration : System.Web.UI.Page
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
                ddlCity.Items.Insert(0, new ListItem("--Select City--", "0"));
                #endregion

                #region Course Name
                DataTable dtCourseName = new DataTable();
                dtCourseName = getCourse();

                ddlCourse.DataSource = dtCourseName;
                ddlCourse.DataTextField = "COURSE_NAME";
                ddlCourse.DataValueField = "COURSE_NAME";
                ddlCourse.DataBind();
                ddlCourse.Items.Insert(0, new ListItem("--Select Course--", "0"));
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string date = txtDate.Text;
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

            string formattedDate = DateTime.Parse(date).ToString("dd/MM/yyyy");
            string formattedDOB = DateTime.Parse(dob).ToString("dd/MM/yyyy");

            try
            {

                #region Generate the RegisterID
                string fetchSequenceQuery = "SELECT UserID_SEQ.NEXTVAL FROM DUAL";
                OracleCommand cmdFetchSeq = new OracleCommand(fetchSequenceQuery, conStr);
                conStr.Open();
                int userId = Convert.ToInt32(cmdFetchSeq.ExecuteScalar());

                string randomPart = Path.GetRandomFileName().Replace(".", "").Substring(0, 4).ToUpper();
                string registrationId = "REG-" + randomPart + "-" + userId.ToString();
                #endregion

                sQuery = "INSERT INTO USERINFORMATION (USERID, TRANSACTION_DATE, NAME, DOB, ADDRESS, CITY, STATE, ZIPCODE, COUNTRY, PHONENUMBER, EMAIL,MEMBERSHIPTYPE, COURSE, PAYMENTMETHOD, COMMENTS,REGISTER_ID) ";
                sQuery += "VALUES( UserID_SEQ.NEXTVAL, TO_DATE('" + formattedDate + "', 'DD/MM/YYYY'), :NAME, TO_DATE('" + formattedDOB + "', 'DD/MM/YYYY'), :ADDRESS, :CITY, :STATE, :ZIPCODE, :COUNTRY, :PHONENUMBER, :EMAIL, :MEMBERSHIPTYPE, :COURSE, :PAYMENTMETHOD, :COMMENTS,:REGISTER_ID) ";

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

                cmd.Parameters.AddWithValue(":REGISTER_ID", registrationId);

                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    string script = $@"document.getElementById('registrationMessage').innerText = 'Your Registration ID is : {registrationId}';
                                    $('#registrationModal').modal('show');";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RegistrationSuccess", script, true);
                }
                else
                {
                    showToast("Data Insert Failed!!", "error");
                }
            }
            catch (Exception ex)
            {
                // Store the error in log table
                showToast("Something Went Wrong.", "error");
            }
            finally
            {
                conStr.Close();
                ClearFieldValues();
            }
        }
        protected void btnAlreadyRegistered_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserInfo.aspx");
        }

        protected void txtEmail_TextChanged(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            if (isEmailExists(email))
            {
                lblError.Text = "Email already exists. Please use a different one.";
            }
            else
            {
                lblError.Text = "";
            }
        }

        public bool isEmailExists(string email)
        {
            try
            {
                sQuery = "SELECT COUNT(1) FROM USERINFORMATION WHERE EMAIL =:EMAIL";
                OracleCommand cmd = new OracleCommand(sQuery, conStr);
                cmd.Parameters.AddWithValue(":EMAIL", email);

                conStr.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());  // Executes the query and returns the result

                return count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void showToast(string msg, string type = "success")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "<script>toastr." + type + "('" + msg + "');</script>", false);
        }

        public void ClearFieldValues()
        {
            txtDate.Text = string.Empty;
            txtName.Text = string.Empty;
            txtDOB.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtState.Text = string.Empty;
            txtZip.Text = string.Empty;
            txtCountry.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtComments.Text = string.Empty;

            ddlCity.ClearSelection();
            ddlCourse.ClearSelection();

            rblMembership.ClearSelection();
            rblPayment.ClearSelection();
        }
    }
}