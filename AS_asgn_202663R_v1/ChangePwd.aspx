<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePwd.aspx.cs" Inherits="AS_asgn_202663R_v1.ChangePwd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script>
        function validatePwd() {
            var str = document.getElementById('<%=tb_newPwd.ClientID %>').value;

            if (str.length < 12) {
                document.getElementById("lbl_errNewPwd").innerHTML = "Password Length Must be at Least 12 Characters";
                document.getElementById("lbl_errNewPwd").style.color = "Red";
                return ("too short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_errNewPwd").innerHTML = "Password require at least 1 number";
                document.getElementById("lbl_errNewPwd").style.color = "Red";
                return("no_number")
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_errNewPwd").innerHTML = "Password require at least 1 uppercase letter";
                document.getElementById("lbl_errNewPwd").style.color = "Red";
                return ("no_uppercase")
            }
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_errNewPwd").innerHTML = "Password require at least 1 lowercase letter";
                document.getElementById("lbl_errNewPwd").style.color = "Red";
                return ("no_lowercase")
            }
            else if (str.search(/[@$&%^*!_-]/) == -1) {
                document.getElementById("lbl_errNewPwd").innerHTML = "Password require at least 1 special character";
                document.getElementById("lbl_errNewPwd").style.color = "Red";
                return ("no_specialchara")
            }

            document.getElementById("lbl_errNewPwd").innerHTML = "Excellent!"
            document.getElementById("lbl_errNewPwd").style.color = "Green";
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
        #btn_change, #btn_hmpg{
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
            <legend>Change Password</legend>
            <table>
                <tr>
                    <td><asp:Label ID="lbl_currPwd" runat="server" Text="Current Password"></asp:Label> </td>
                    <td><asp:TextBox ID="tb_currPwd" runat="server" ></asp:TextBox></td>
                    <td><asp:Label ID="lbl_errCurrPwd" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td><asp:Label ID="lbl_newPwd" runat="server" Text="New Password"></asp:Label> </td>
                    <td><asp:TextBox ID="tb_newPwd" runat="server" onkeyup="javascript:validatePwd()"></asp:TextBox></td>
                    <td><asp:Label ID="lbl_errNewPwd" runat="server" Text=""></asp:Label></td>
                </tr>
                 <tr>
                    <td><asp:Label ID="lbl_cfnewPwd" runat="server" Text="Confirm New Password"></asp:Label> </td>
                    <td><asp:TextBox ID="tb_cfnewPwd" runat="server" ></asp:TextBox></td>
                    <td><asp:Label ID="lbl_erCfnewPwd" runat="server" Text=""></asp:Label></td>
                </tr>
                 <tr>
                    <td><asp:Button ID="btn_hmpg" runat="server" Text="Back to homepage" style="margin-top:15px" OnClick="btn_hmpg_Click"/></td>
                    <td><asp:Button ID="btn_change" runat="server" Text="Change" style="margin-top:15px" OnClick="btn_change_Click"/></td>
                    <td></td>
                </tr>
            </table>
        </fieldset>
    </form>
</body>
</html>

