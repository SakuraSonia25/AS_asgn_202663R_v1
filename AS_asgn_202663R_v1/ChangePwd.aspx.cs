using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AS_asgn_202663R_v1
{
    public partial class ChangePwd : System.Web.UI.Page
    {
        string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASasgnDB"].ConnectionString;
        string email;
        string currPwd;
        string newPwd;
        string newPwd2;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected string HashPassword(string salt, string pwd)
        {
            SHA512Managed hashing = new SHA512Managed();
            string pwdWithSalt = pwd + salt;
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
            string currPwdhash = Convert.ToBase64String(hashWithSalt);

            return currPwdhash;

        }

        protected void btn_change_Click(object sender, EventArgs e)
        {
            email= Session["LoggedIn"].ToString();
            currPwd = tb_currPwd.Text.Trim();
            newPwd = tb_newPwd.Text.Trim();
            newPwd2 = tb_cfnewPwd.Text.Trim();

            
            string salt = getDBSalt(email);
            string dbpwd = getDBPwd(email);

            if (salt != null && salt.Length > 0 && dbpwd != null && dbpwd.Length > 0)
            {
                string currPwdhash = HashPassword(salt, currPwd);

                if (currPwdhash.Equals(dbpwd))
                { 
                    if (pwdChecker(newPwd) < 4)
                    {
                        //hvErr = true;
                        int score = pwdChecker(newPwd);
                        string status = "";
                        switch (score)
                        {
                            case 0:
                                status = "Very Weak";
                                break;
                            case 1:
                                status = "Very Weak";
                                break;
                            case 2:
                                status = "Weak";
                                break;
                            case 3:
                                status = "Medium";
                                break;
                            default:
                                break;
                        }
                        lbl_errNewPwd.Text = "Password is " + status;
                        lbl_errNewPwd.ForeColor = Color.Red;
                        lbl_errNewPwd.Visible = true;
                    }
                    else
                    {
                        string currNewhash = HashPassword(salt, newPwd);

                        if (currNewhash != GetPastPwd1(email) && currNewhash != GetPastPwd2(email))
                        {
                            lbl_errNewPwd.Text = "";
                            if (newPwd == newPwd2)
                            {
                                // change pwd function
                                ChangePassword();
                                lbl_erCfnewPwd.Text = "Password changed";
                                lbl_erCfnewPwd.ForeColor = Color.Green;
                            }
                            else
                            {
                                lbl_erCfnewPwd.Text = "Not Match with above password";
                                lbl_erCfnewPwd.ForeColor = Color.Red;
                                lbl_erCfnewPwd.Visible = true;
                            }
                        }
                        else
                        {
                            lbl_errNewPwd.Text = "This Password is the same as past passwords";
                            lbl_errNewPwd.ForeColor = Color.Red;
                            lbl_errNewPwd.Visible = true;
                        }
                   
                    }
                }
            }
            else
            {
                lbl_errCurrPwd.Text = "Incorrect Password";
                lbl_errCurrPwd.ForeColor = Color.Red;
                lbl_errCurrPwd.Visible = true;
            }

           
        }

        protected int pwdChecker(String password)
        {
            int score = 0;
            //Score 1(Complexity: Very Weak) – Min Password length of 12
            if (password.Length >= 12)
            {
                score++;
            }
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            if (Regex.IsMatch(password, "[!@#$%^&*_+]"))
            {
                score++;
            }

            return score;
        }

        protected string getDBPwd(string email)
        {
            string pwd = null;

            SqlConnection connection = new SqlConnection(ConnectionString);
            string sql = "select password FROM user_info WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["password"] != null)
                        {
                            if (reader["password"] != DBNull.Value)
                            {
                                pwd = reader["password"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return pwd;
        }

        protected string getDBSalt(string email)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(ConnectionString);
            string sql = "select salt FROM user_info WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["salt"] != null)
                        {
                            if (reader["salt"] != DBNull.Value)
                            {
                                s = reader["salt"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return s;

        }

        protected string GetPastPwd1(string email)
        {
            string pwd = null;

            SqlConnection connection = new SqlConnection(ConnectionString);
            string sql = "select PastPassword1 FROM user_info WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PastPassword1"] != null)
                        {
                            if (reader["PastPassword1"] != DBNull.Value)
                            {
                                pwd = reader["PastPassword1"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return pwd;
        }

        protected string GetPastPwd2(string email)
        {
            string pwd = null;

            SqlConnection connection = new SqlConnection(ConnectionString);
            string sql = "select PastPassword2 FROM user_info WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PastPassword2"] != null)
                        {
                            if (reader["PastPassword2"] != DBNull.Value)
                            {
                                pwd = reader["PastPassword2"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return pwd;
        }

        protected void UpdatePassword(string email, string pwd, int PastPwd)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            string sql = "";
            if (PastPwd == 0)
            {
                sql = "UPDATE user_info SET password = '" + pwd + "' WHERE email=@email; ";
            }
            else if (PastPwd == 1)
            {
                sql = "UPDATE user_info SET PastPassword1 = '" + pwd + "' WHERE email=@email; ";
            }
            else if (PastPwd == 2)
            {
                sql = "UPDATE user_info SET PastPassword2= '" + pwd + "' WHERE email=@email; ";
            }

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
        protected void ChangePassword()
        {
            string currPwdhash = HashPassword(getDBSalt(email), currPwd);;
            string currNewhash = HashPassword(getDBSalt(email), newPwd); ;
            UpdatePassword(email, currNewhash, 0);

            if (string.IsNullOrEmpty(GetPastPwd1(email)))
            {
                UpdatePassword(email, currPwdhash, 1);
            }
            else
            {
                if (string.IsNullOrEmpty(GetPastPwd2(email)))
                {
                    UpdatePassword(email, currPwdhash, 2);
                }
                else
                {
                    UpdatePassword(email, GetPastPwd2(email), 1);
                    UpdatePassword(email, currPwdhash, 2);
                }
            }
        }

        protected void btn_hmpg_Click(object sender, EventArgs e)
        {
            Response.Redirect("Homepage.aspx", false);
        }
    }
}