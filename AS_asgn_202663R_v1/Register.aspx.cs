using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AS_asgn_202663R_v1
{
    public partial class Register : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ASasgnDB"].ConnectionString;
        byte[] Key;
        byte[] IV;
        string hashedpwd;
        string salt;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected Boolean HasWhiteSpace(string s)
        {       
            if (s != null)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (char.IsWhiteSpace(s[i]))
                        return true;
                }
            }
           
            return false;
        }

        protected void btn_register_Click(object sender, EventArgs e)
        {
            Boolean hvErr = false;
            lbl_errFname.Text = "";
            lbl_errLname.Text = "";
            lbl_errCC.Text = "";
            lbl_errEmail.Text = "";
            lbl_errbday.Text = "";
            lbl_errPwd.Text = "";

            // validation
            if (string.IsNullOrEmpty(tb_fname.Text.Trim()))
            {
                hvErr = true;
                lbl_errFname.Text = "This Field is Required";
                lbl_errFname.ForeColor = Color.Red;
            }
            if (string.IsNullOrEmpty(tb_lname.Text.Trim()))
            {
                hvErr = true;
                lbl_errLname.Text = "This Field is Required";
                lbl_errLname.ForeColor = Color.Red;
            }
            if (string.IsNullOrEmpty(tb_bday.Text.Trim()))
            {
                hvErr = true;
                lbl_errbday.Text = "This Field is Required";
                lbl_errbday.ForeColor = Color.Red;
            }
            if (string.IsNullOrEmpty(fileup_photo.ToString()))
            {
                hvErr = true;
                lbl_errPhoto.Text = "This Field is Required";
                lbl_errPhoto.ForeColor = Color.Red;
            }
            if (!isValidCreditCardNo(tb_creditCard.Text.Trim()))
            {
                hvErr = true;
                lbl_errCC.Text = "Please provide a valid credit card number!";
                lbl_errCC.ForeColor = Color.Red;
            }
            if (!isValidEmail(tb_email.Text.Trim()))
            {
                hvErr = true;
                lbl_errEmail.Text = "Please provide a valid Email!";
                lbl_errEmail.ForeColor = Color.Red;
            }
            if (EmailExist(tb_email.Text.Trim()))
            {
                hvErr = true;
                lbl_errEmail.Text = "This email has been used before!";
                lbl_errEmail.ForeColor = Color.Red;
            }
            if (HasWhiteSpace(tb_pwd.Text))
            {
                hvErr = true;
                lbl_errPwd.Text = "Password cannot contain spacing";
                lbl_errPwd.ForeColor = Color.Red;
            }
            if (pwdChecker(tb_pwd.Text) < 4)
            {
                hvErr = true;
                int score = pwdChecker(tb_pwd.Text);
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
                lbl_errPwd.Text = "Password is " + status;
                lbl_errPwd.ForeColor = Color.Red;
            }
            if (!hvErr)
            {
                // hash salt password
                hashedpwd = hashSaltPassword(tb_pwd.Text.Trim());

                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();
                Key = cipher.Key;
                IV = cipher.IV;

                // create acct
                createAccount();
                // email verification
                string name = tb_fname.Text.Trim() + " " + tb_lname.Text.Trim();

                SendEmailConfirmation(name, tb_email.Text.Trim());
                Response.Redirect("Verification.aspx", false);
            }

        }

        // Validation functions
        protected Boolean isValidCreditCardNo(string creditcardno)
        {
            var isValid = false;

            Regex visaRegEx = new Regex(@"^(?:4[0-9]{12}(?:[0-9]{3})?)$");
            Regex mastercardRegEx = new Regex(@"^(?:5[1-5][0-9]{14})$");
            Regex amexpRegEx = new Regex(@"^(?:3[47][0-9]{13})$");
            Regex discovRegEx = new Regex(@"^(?:6(?:011|5[0-9][0-9])[0-9]{12})$");

            if (Regex.IsMatch(creditcardno, visaRegEx.ToString()))
            {
                isValid = true;
            }
            if (mastercardRegEx.Match(creditcardno).Success)
            {
                isValid = true;
            }
            if (Regex.IsMatch(creditcardno, amexpRegEx.ToString()))
            {
                isValid = true;
            }
            if (Regex.IsMatch(creditcardno, discovRegEx.ToString()))
            {
                isValid = true;
            }

            return isValid;
        }
        protected Boolean isValidEmail(string email)
        {
            Regex emailFormat = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match m = emailFormat.Match(email);
            return m.Success;
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
        protected string hashSaltPassword(string pwd)
        {
            string finalHash;

            //Generate random "salt"
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];

            //Fills array of bytes with a cryptographically strong sequence of random values.
            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);

            SHA512Managed hashing = new SHA512Managed();
            string pwdWithSalt = pwd + salt;

            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
            finalHash = Convert.ToBase64String(hashWithSalt);

            return finalHash;
        }

        protected void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO user_info VALUES(@fname, @lname,@creditCardInfo,@email,@password,@salt,@dateOfBirth,@userType,@photo,@email_verified,@Key,@IV,@FailedLogin,@FailedDateTime,@PastPassword1,'',@PwdDateTime)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@fname", tb_fname.Text.Trim());
                            cmd.Parameters.AddWithValue("@lname", tb_lname.Text.Trim());
                            cmd.Parameters.AddWithValue("@creditCardInfo", encryptData(tb_creditCard.Text.Trim()));
                            cmd.Parameters.AddWithValue("@email", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@password", hashedpwd);
                            cmd.Parameters.AddWithValue("@salt", salt);
                            cmd.Parameters.AddWithValue("@dateOfBirth", tb_bday.Text);
                            cmd.Parameters.AddWithValue("@userType", "student");
                            cmd.Parameters.AddWithValue("@photo", fileup_photo.ToString().Trim());
                            cmd.Parameters.AddWithValue("@email_verified", 0);
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@FailedLogin", 0);
                            cmd.Parameters.AddWithValue("@FailedDateTime", "");
                            cmd.Parameters.AddWithValue("@PastPassword1", hashedpwd);
                            cmd.Parameters.AddWithValue("@PwdDateTime", DateTime.Now);
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

        protected string encryptData(string data)
        {
            byte[] cipherText;
            string cipherString;

            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
                cipherString = Convert.ToBase64String(cipherText);

                //Encrypt
                //cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
                //cipherString = Convert.ToBase64String(cipherText);
                //Console.WriteLine("Encrypted Text: " + cipherString);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return cipherString;
        }

        protected void SendEmailConfirmation(string receiverName, string receiverEmail)
        {
            string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
            string emailPwd = System.Configuration.ConfigurationManager.AppSettings["password"].ToString();

            // create a new GUID and save into the session
            string guid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            Session["vcode"] = guid;
            Session["email"] = tb_email.Text.Trim();

            // Uri url = Request.Url;
            // int port = url.Port;
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(senderEmail);
                message.To.Add(new MailAddress(receiverEmail));
                message.Subject = "Verification Code";
                message.IsBodyHtml = true;
                message.Body = "<p> Dear " + receiverName + ", </p> <br> <p> Verification Code: " + guid + "</p>"; ;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(senderEmail, emailPwd);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        protected Boolean EmailExist(string email)
        {
            Boolean isExist = false;

            string sql = "select count(*) from user_info where email = @email";
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
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

    }

}