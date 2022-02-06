<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AS_asgn_202663R_v1.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style>
        fieldset {
            width: 400px;
            margin:auto;
        }
        h1, #lbl_errMsg{
            text-align:center;
        }
        table{
            width:300px;
            margin: 5% auto 5% auto;
        }
        legend{
            text-align:center;
        }
        #btn_login, #btn_register{
            background-color: dodgerblue;
            color: lightgoldenrodyellow;
            border-radius: 4px;
            padding:5px 10px;
            border: none;
        }
    </style>
    <title>Login</title>
    <script src="https://www.google.com/recaptcha/api.js"></script>
     <script>
           function onSubmit(token) {
             document.getElementById("demo-form").submit();
           }
     </script>
</head>
<body>
    <h1><img src="https://img.icons8.com/external-soft-fill-juicy-fish/60/000000/external-supplies-school-soft-fill-soft-fill-juicy-fish.png"/>SITConnect </h1>
    <form id="form1" runat="server">
        &nbsp;<fieldset style="width:500px">
            <legend>Login</legend>
            <asp:Label ID="lbl_errMsg" runat="server" Text="Hi" Visible="false"></asp:Label>
            <table>
                <tr>
                    <td><asp:Label ID="lbl_email" runat="server" Text="Email"></asp:Label> </td>
                    <td><asp:TextBox ID="tb_email" runat="server" ></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label ID="lbl_pwd" runat="server" Text="Password"></asp:Label> </td>
                    <td><asp:TextBox ID="tb_pwd" runat="server" TextMode="Password"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="2"><asp:Button ID="btn_login" runat="server" Text="Login" style="margin-top:15px" OnClick="btn_login_Click" />  <asp:Button ID="btn_register" runat="server" Text="Register" style="margin-top:15px" OnClick="btn_register_Click" /></td>
                </tr>
            </table>
               <button class="g-recaptcha" 
            data-sitekey="6LfOS1oeAAAAAHiU_ZRc30kDVUmfewpE1xg8GsZ4" 
            data-callback='onSubmit' 
            data-action='submit'>Submit</button>
        </fieldset>

    </form>
</body>
</html>