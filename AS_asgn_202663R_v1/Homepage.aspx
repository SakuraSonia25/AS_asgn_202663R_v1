<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="AS_asgn_202663R_v1.Homepage" %>

<!DOCTYPE html>
<html>
<head>
    <style>
        h1, p {
            text-align:center;
        }
        p{
            font-size: 20px;
        }
        #btn_logout, #btn_chgPwd {
            background-color: dodgerblue;
            color: lightgoldenrodyellow;
            border-radius: 4px;
            padding: 5px 10px;
            margin: 0 48% 0 48%;
            border: none;
        }
    </style>
    <meta charset="utf-8" />
    <title>Homepage</title>
</head>
<body>
    <form id="form1" runat="server">
    <h1><img src="https://img.icons8.com/external-soft-fill-juicy-fish/60/000000/external-supplies-school-soft-fill-soft-fill-juicy-fish.png" />SITConnect </h1>
    <p>Welcome to the homepage :)</p>

    <p><asp:Button ID="btn_chgPwd" runat="server" Text="Change Password" OnClick="ToChangePwd"/></p>
    <p><asp:Button ID="btn_logout" runat="server" Text="Logout" OnClick="LogoutMe"/></p>
    </form>
    
</body>
</html>
