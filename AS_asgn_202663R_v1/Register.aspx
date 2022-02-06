<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="AS_asgn_202663R_v1.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script>
        function validatePwd() {
            var str = document.getElementById('<%=tb_pwd.ClientID %>').value;

            if (str.length < 12) {
                document.getElementById("lbl_errPwd").innerHTML = "Password Length Must be at Least 12 Characters";
                document.getElementById("lbl_errPwd").style.color = "Red";
                return ("too short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_errPwd").innerHTML = "Password require at least 1 number";
                document.getElementById("lbl_errPwd").style.color = "Red";
                return("no_number")
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_errPwd").innerHTML = "Password require at least 1 uppercase letter";
                document.getElementById("lbl_errPwd").style.color = "Red";
                return ("no_uppercase")
            }
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_errPwd").innerHTML = "Password require at least 1 lowercase letter";
                document.getElementById("lbl_errPwd").style.color = "Red";
                return ("no_lowercase")
            }
            else if (str.search(/[@$&%^*!_-]/) == -1) {
                document.getElementById("lbl_errPwd").innerHTML = "Password require at least 1 special character";
                document.getElementById("lbl_errPwd").style.color = "Red";
                return ("no_specialchara")
            }

            document.getElementById("lbl_errPwd").innerHTML = "Excellent!"
            document.getElementById("lbl_errPwd").style.color = "Green";
        }

        function validateCCNo() {

            var ccNum = document.getElementById("tb_creditCard").value;
            var visaRegEx = /^(?:4[0-9]{12}(?:[0-9]{3})?)$/;
            var mastercardRegEx = /^(?:5[1-5][0-9]{14})$/;
            var amexpRegEx = /^(?:3[47][0-9]{13})$/;
            var discovRegEx = /^(?:6(?:011|5[0-9][0-9])[0-9]{12})$/;
            var isValid = false;

            if (visaRegEx.test(ccNum)) {
                isValid = true;
            } else if (mastercardRegEx.test(ccNum)) {
                isValid = true;
            } else if (amexpRegEx.test(ccNum)) {
                isValid = true;
            } else if (discovRegEx.test(ccNum)) {
                isValid = true;
            }

            if (isValid) {
                document.getElementById("lbl_errCC").innerHTML = "Valid Credit Card number";
                document.getElementById("lbl_errCC").style.color = "Green";
            } else {
                document.getElementById("lbl_errCC").innerHTML = "Please provide a valid credit card number!";
                document.getElementById("lbl_errCC").style.color = "Red";
            }
        }

        function validateEmail() {
            var email = document.getElementById("tb_email").value;
            var emailformat = /\S+@\S+\.\S+/;
            if (emailformat.test(email)) {
                document.getElementById("lbl_errEmail").innerHTML = "Valid Email address";
                document.getElementById("lbl_errEmail").style.color = "Green";
            } else {
                document.getElementById("lbl_errEmail").innerHTML = "Please provide a valid Email!";
                document.getElementById("lbl_errEmail").style.color = "Red";
            }
        }
    </script>
     <style>
        fieldset {
            width: 1000px;
            margin:auto;
        }
        h1{
            text-align:center;
        }
        table{
            width:600px;
            margin: 5% auto 5% auto;
        }
        legend{
            text-align:center;
        }
        #btn_register{
            background-color: dodgerblue;
            color: lightgoldenrodyellow;
            border-radius: 4px;
            padding:5px 10px;
            border: none;
        }
    </style>
    <title>Registration</title>
</head>
<body>
    <h1><img src="https://img.icons8.com/external-soft-fill-juicy-fish/60/000000/external-supplies-school-soft-fill-soft-fill-juicy-fish.png"/>SITConnect </h1>
    <form id="form1" runat="server">
       
        <fieldset style="width:500px">
            <legend>Registration</legend>
            <table>
                <tr>
                    <td><asp:Label ID="lbl_fname" runat="server" Text="First Name"></asp:Label> </td>
                    <td><asp:TextBox ID="tb_fname" runat="server" ></asp:TextBox></td>
                    <td><asp:Label ID="lbl_errFname" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td><asp:Label ID="lbl_lname" runat="server" Text="Last Name"></asp:Label> </td>
                    <td><asp:TextBox ID="tb_lname" runat="server"></asp:TextBox></td>
                    <td><asp:Label ID="lbl_errLname" runat="server" Text=""></asp:Label></td>
                </tr>
                 <tr>
                    <td><asp:Label ID="lbl_creditCard" runat="server" Text="Credit Card Info"></asp:Label> </td>
                    <td><asp:TextBox ID="tb_creditCard" runat="server" onkeyup="javascript:validateCCNo()"></asp:TextBox></td>
                    <td><asp:Label ID="lbl_errCC" runat="server" Text=""></asp:Label></td>
                </tr>
                 <tr>
                    <td><asp:Label ID="lbl_email" runat="server" Text="Email address"></asp:Label> </td>
                    <td><asp:TextBox ID="tb_email" runat="server" TextMode="Email" onkeyup="javascript:validateEmail()"></asp:TextBox></td>
                    <td><asp:Label ID="lbl_errEmail" runat="server" Text=""></asp:Label></td>
                </tr>
                 <tr>
                    <td><asp:Label ID="lbl_pwd" runat="server" Text="Password"></asp:Label> </td>
                    <td><asp:TextBox ID="tb_pwd" runat="server" onkeyup="javascript:validatePwd()" TextMode="Password"></asp:TextBox></td>
                     <td><asp:Label ID="lbl_errPwd" runat="server" Text=""></asp:Label></td>
                </tr>
                 <tr>
                    <td><asp:Label ID="lbl_bday" runat="server" Text="Date of Birth"></asp:Label> </td>
                    <td><asp:TextBox ID="tb_bday" runat="server" TextMode="Date"></asp:TextBox ></td>
                     <td><asp:Label ID="lbl_errbday" runat="server" Text=""></asp:Label></td>
                </tr>
                 <tr>
                    <td><asp:Label ID="lbl_photo" runat="server" Text="Upload Photo"></asp:Label></td>
                    <td><asp:FileUpload ID="fileup_photo" runat="server" /></td>
                     <td><asp:Label ID="lbl_errPhoto" runat="server" Text=""></asp:Label></td>
                </tr>
                 <tr>
                    <td></td>
                    <td><asp:Button ID="btn_register" runat="server" Text="Register" style="margin-top:15px" OnClick="btn_register_Click" /></td>
                    <td></td>
                </tr>
            </table>
        </fieldset>
    </form>
</body>
</html>

