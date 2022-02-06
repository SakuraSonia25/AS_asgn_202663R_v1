<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Verification.aspx.cs" Inherits="AS_asgn_202663R_v1.Verification" %>

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
        #btn_verify{
            background-color: dodgerblue;
            color: lightgoldenrodyellow;
            border-radius: 4px;
            padding:5px 10px;
            border: none;
        }
    </style>
    <title>Verification</title>
</head>
<body>
    <h1><img src="https://img.icons8.com/external-soft-fill-juicy-fish/60/000000/external-supplies-school-soft-fill-soft-fill-juicy-fish.png"/>SITConnect </h1>
    <form id="form1" runat="server">
        &nbsp;<fieldset style="width:500px">
            <legend>Verification</legend>
            <asp:Label ID="lbl_errMsg" runat="server" Text="Hi" Visible="false"></asp:Label>
            <table>
                <tr>
                    <td><asp:Label ID="lbl_vCode" runat="server" Text="Verification Code:"></asp:Label> </td>
                    <td><asp:TextBox ID="tb_vCode" runat="server" ></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="2"><asp:Button ID="btn_verify" runat="server" Text="Verify" style="margin-top:15px" OnClick="btn_verify_Click" /></td>
                </tr>
            </table>
        </fieldset>

    </form>
</body>
</html>
