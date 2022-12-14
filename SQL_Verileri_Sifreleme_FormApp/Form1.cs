using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SQL_Verileri_Sifreleme_FormApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Sql bağlantı adresimiz.
        private SqlConnection con =
            new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestDB;Integrated Security=True");

        private void Form1_Load(object sender, EventArgs e)
        {
            List(); // Veri tabanındaki veriyi listeleyen method
        }

        // Girilen veriyi şifreleyen method
        private string Sifrele(string key)
        {
            byte[] keyDizi = ASCIIEncoding.ASCII.GetBytes(key);
            string keyEncrypt = Convert.ToBase64String(keyDizi);
            return keyEncrypt;
        }

        // Şifreli veriyi cozen method
        private string Coz(string key)
        {
            byte[] keyDizi2 = Convert.FromBase64String(key);
            string keyEncrypt2 = ASCIIEncoding.ASCII.GetString(keyDizi2);
            return keyEncrypt2;
        }

        //  Sql'deki veriyi DataGridView altaran method
        private void List()
        {
            SqlDataAdapter da = new SqlDataAdapter("select Name as [Ad Soyad], PROFESSION as Meslek, salary as [Maaş] from Personel", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {

                row["Ad Soyad"] = Coz(row["Ad Soyad"].ToString());
                row["Meslek"] = Coz(row["Meslek"].ToString());
                row["Maaş"] = Coz(row["Maaş"].ToString());
            }
            dataGridView1.DataSource = dt;
        }

        // Girilen bilgileri şifreli halde veri tabanına kayıt yapan method
        public void BtnSave_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand com = new SqlCommand("insert into Personel (name,PROFESSION,salary) values(@p1,@p2,@p3)", con);
            com.Parameters.AddWithValue("@p1", Sifrele(TxtName.Text));
            com.Parameters.AddWithValue("@p2", Sifrele(TxtMeslek.Text));
            com.Parameters.AddWithValue("@p3", Sifrele(TxtSalary.Text));
            com.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Veri başarılı bir şekilde kayıt edildi.", "Bilgilendirme", MessageBoxButtons.OK,
                MessageBoxIcon.Asterisk);

            List();
        }

    }
}
