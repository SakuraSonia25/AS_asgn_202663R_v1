using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AS_asgn_202663R_v1
{
    public partial class Verification : System.Web.UI.Page
    {
        string vcode;
        string email;
        string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASasgnDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            vcode = Session["vcode"].ToString();
            email = Session["email"].ToString();
           
        }

        protected void btn_verify_Click(object sender, EventArgs e)
        {
            if(tb_vCode.Text.Trim() == vcode)
            {
                Session["vcode"] = null;
                Session["email"] = null;
                ConfirmEmailVerified(email);
                Response.Redirect("Login.aspx", false);
            }
            else
            {
                lbl_errMsg.Text = "Wrong verification code";
                lbl_errMsg.ForeColor = Color.Red;
                lbl_errMsg.Visible = true;
            }
        }

        protected void ConfirmEmailVerified(string email)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string sql = "UPDATE user_info SET email_verified = 1 WHERE email=@email; ";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
        }
    }
}