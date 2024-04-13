using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Register__Form
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        public string _connectionStr = "Data Source=(local);Initial Catalog=OJT;Integrated Security=True";

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = txtName.Text;
                string userEmail = txtEmail.Text;
                string userRole = cobRole.Text;

                if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(cobRole.Text))
                {
                    MessageBox.Show("Please fill all field...", "Warning", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }

                if(IsEmailDuplicated(userEmail) )
                {
                    MessageBox.Show("User with this email already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Clear();
                    return;
                }

                SqlConnection conn = new(_connectionStr);
                conn.Open();
                string query = @"INSERT INTO Register_Form_DB_Table
           (Name
           ,Email
           ,UserRole
           ,isActive) VALUES (@Name,@Email,@UserRole,@isActive)";
                SqlCommand cmd = new(query, conn);
                cmd.Parameters.AddWithValue("@Name", userName);
                cmd.Parameters.AddWithValue("@Email", userEmail);
                cmd.Parameters.AddWithValue("@UserRole", userRole);
                cmd.Parameters.AddWithValue("@isActive", true);
                int result = cmd.ExecuteNonQuery();
                conn.Close();

                if(result > 0)
                {
                    Clear();
                    MessageBox.Show("Create Successfully", "information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return;
                }else
                {
                    MessageBox.Show("Creating Fail","Warning",MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                


                

            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool IsEmailDuplicated(string email)
        {
            try
            {
                SqlConnection conn = new(_connectionStr);
                conn.Open();
                string query = @"SELECT [UserId]
      ,[Name]
      ,[Email]
      ,[UserRole]
      ,[isActive]
  FROM [dbo].[Register_Form_DB_Table] WHERE Email = @Email AND isActive = @isActive";
                SqlCommand cmd = new(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@isActive", true);
                SqlDataAdapter adapter = new(cmd);
                DataTable dt = new();
                adapter.Fill(dt);
                conn.Close();

                return dt.Rows.Count > 0;

            } catch (Exception ex)
            {
                throw new Exception( ex.Message);
            }
        }

        public void Clear()
        {
            txtEmail.Text = "";
            txtName.Text = "";
            cobRole.Text = "";
        }

  
    }
}
