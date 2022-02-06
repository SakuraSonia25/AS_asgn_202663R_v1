using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AS_asgn_202663R_v1
{
    public partial class Login : System.Web.UI.Page
    {
        string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASasgnDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

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

        protected Boolean IsEmailVerified(string email)
        {
            Boolean isVerified = false;

            SqlConnection connection = new SqlConnection(ConnectionString);
            string sql = "select email_verified FROM user_info WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["email_verified"] != null)
                        {
                            if (reader["email_verified"] != DBNull.Value)
                            {
                                isVerified = Convert.ToBoolean(Convert.ToInt32(reader["email_verified"]));
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
            return isVerified;
        }
        protected Boolean EmailExist(string email)
        {
            Boolean isExist = false;

            string sql = "select count(*) from user_info where email = @email";
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);

            try
            {
                connection.Open(); 
                int count = Convert.ToInt32(command.ExecuteScalar());

                if (count > 0)
                {
                    // yes emailid exists already
                    isExist = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }

            return isExist;
        }

        protected void SetfailAttempt(string email, string action)
        {
            
            int failAttempts = GetfailAttempts(email);
            int updatedfailAtt = failAttempts + 1;

            SqlConnection connection = new SqlConnection(ConnectionString);
            string sql = "";
            if (action == "add")
            {
                sql = "UPDATE user_info SET FailedLogin = " + updatedfailAtt + " WHERE email=@email; ";
            }
            else if (action == "reset")
            {
                sql = "UPDATE user_info SET FailedLogin = 0 WHERE email=@email; ";
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
        protected int GetfailAttempts(string email)
        {
            int failAttempts = 0;

            SqlConnection connection = new SqlConnection(ConnectionString);
            string sql = "select FailedLogin FROM user_info WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["FailedLogin"] != null)
                        {
                            if (reader["FailedLogin"] != DBNull.Value)
                            {
                                failAttempts = Convert.ToInt32(reader["FailedLogin"]);
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
            return failAttempts;
        }
        protected string GetLockoutEndTime(string email)
        {
            string LockoutEndTime = null;

            SqlConnection connection = new SqlConnection(ConnectionString);
            string sql = "select FailedDateTime FROM user_info WHERE Email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["FailedDateTime"] != null)
                        {
                            if (reader["FailedDateTime"] != DBNull.Value)
                            {
                                LockoutEndTime = (string)reader["FailedDateTime"];
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
            return LockoutEndTime;
        }

        protected void SetLockoutEndTime(string email, string datetime)
        {
            string endlocktime = datetime;

            SqlConnection connection = new SqlConnection(ConnectionString);
            string sql = "UPDATE user_info SET FailedDateTime =' " + endlocktime + "' WHERE email=@email; ";
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

        protected void CreateLoginAuditLog()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO audit_log VALUES(@account,@event,@dateTime)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@account", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@event", "Login");
                            cmd.Parameters.AddWithValue("@dateTime", DateTime.Now);
                            
                            cmd.Connection = con;

                            try
                            {
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.ToString());
                                //lb_error1.Text = ex.ToString();
                            }
                            finally
                            {
                                con.Close();
                            }


                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        protected void btn_login_Click(object sender, EventArgs e)
        {
            string pwd = tb_pwd.Text.Trim();
            string email = tb_email.Text.Trim();

            int failedAttempts = GetfailAttempts(email);

            if (EmailExist(email) && IsEmailVerified(email))
            {
                if(failedAttempts < 3) {
                    SHA512Managed hashing = new SHA512Managed();
                    string salt = getDBSalt(email);
                    string dbpwd = getDBPwd(email);

                    try
                    {
                        if (salt != null && salt.Length > 0 && dbpwd != null && dbpwd.Length > 0)
                        {
                            string pwdWithSalt = pwd + salt;
                            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                            string userHash = Convert.ToBase64String(hashWithSalt);

                            if (userHash.Equals(dbpwd))
                            {
                                if(GetLockoutEndTime(email) != "")
                                {
                                    if (Convert.ToDateTime(GetLockoutEndTime(email)) <= DateTime.Now)
                                    {
                                        SetLockoutEndTime(email, "");
                                        SetfailAttempt(email, "reset");

                                        CreateLoginAuditLog();

                                        Session["LoggedIn"] = email;

                                        // create a new GUID and save into the session
                                        string guid = Guid.NewGuid().ToString();
                                        Session["AuthToken"] = guid;

                                        // now create a new cookie with this guid value
                                        Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                                        Response.Redirect("Homepage.aspx", false);
                                    }
                                    else
                                    {
                                        lbl_errMsg.Visible = true;
                                        lbl_errMsg.Text = "Account has been locked. Try again in a minute";
                                        lbl_errMsg.ForeColor = Color.Red;

                                    }
                                    
                                }
                                else
                                {

                                    SetfailAttempt(email, "reset");

                                    CreateLoginAuditLog();

                                    Session["LoggedIn"] = email;

                                    // create a new GUID and save into the session
                                    string guid = Guid.NewGuid().ToString();
                                    Session["AuthToken"] = guid;

                                    // now create a new cookie with this guid value
                                    Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                                    Response.Redirect("Homepage.aspx", false);
                                }
                                
                                
                            }
                            else
                            {
                                lbl_errMsg.Visible = true;
                                lbl_errMsg.Text = "Email or password is not valid. Please try again.";
                                lbl_errMsg.ForeColor = Color.Red;
                                SetfailAttempt(email, "add");
                            }

                        }
                    
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }

                    finally { }
                }
                else
                {
                    if (failedAttempts == 3)
                    {
                        SetLockoutEndTime(email,DateTime.Now.AddMinutes(1).ToString());
                        SetfailAttempt(email, "reset");
                        lbl_errMsg.Visible = true;
                        lbl_errMsg.Text = "Account has been locked. Try again in a minute";
                        lbl_errMsg.ForeColor = Color.Red;
                    }

                }
            }
            else
            {
                lbl_errMsg.Visible = true;
                lbl_errMsg.Text = "Email or password is not valid. Please try again.";
                lbl_errMsg.ForeColor = Color.Red;
            }

        }

        protected void btn_register_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx", false);
        }
    }
}