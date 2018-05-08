using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void RegisterUser(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            int userId = 0;
            //string cs = "Data Source=.\\SQLEXPRESS01;Initial Catalog=SRR;Integrated Security=True";
            string cs = "Data Source=dcm.uhcl.edu;Initial Catalog=c563318sp02g4;Persist Security Info=True;User ID=c563318sp02g4;Password=7850477";
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("insert_user"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Connection = con;
                    con.Open();
                    userId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            switch (userId)
            {
                case -1:
                    Label1.Text = "Username already exists";
                    break;
                case -2:
                    Label1.Text = "Email already exists";
                    break;
                default:
                    SendActivationEmail(userId);
                    break;
            }
        }
    }

    private void SendActivationEmail(int userId)
    {
        //string cs = "Data Source=.\\SQLEXPRESS01;Initial Catalog=SRR;Integrated Security=True";
        string cs = "Data Source=dcm.uhcl.edu;Initial Catalog=c563318sp02g4;Persist Security Info=True;User ID=c563318sp02g4;Password=7850477";
        string activationCode = Guid.NewGuid().ToString();
        using (SqlConnection con = new SqlConnection(cs))
        {
            using (SqlCommand cmd = new SqlCommand("insert into UserActivation values (@UserId, @ActCode)", con))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ActCode", activationCode);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        using (MailMessage mail = new MailMessage("myEmail@email.com", txtEmail.Text))
        {
            mail.Subject = "SRRS - Activate Account";
            string body = "Hello " + txtUsername.Text.Trim() + ", ";
            body += "<br/><br/>Welcome to Study Room Reservation System";
            body += "<br/><br/><a href='" + Request.Url.AbsoluteUri.Replace("Register.aspx", "Activation.aspx?uid=" + activationCode + "'>Click here to activate your account</a>");
            body += "<br/><br/>Thank You";
            body += "<br/>Team SRRS";
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential credentials = new NetworkCredential("srrsystemuhcl@gmail.com", "srrs@1234");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = credentials;
            smtp.Port = 587;
            smtp.Send(mail);

            //Go to Login
            Response.Redirect("http://dcm.uhcl.edu/c563318sp02g4/SRRS/Login.aspx");
        }
    }
}